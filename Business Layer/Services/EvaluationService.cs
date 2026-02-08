using Microsoft.EntityFrameworkCore;
using ProfRate.Data;
using ProfRate.DTOs;
using ProfRate.Entities;

namespace ProfRate.Services
{
    // Service للـ Evaluations - إدارة التقييمات
    public class EvaluationService : IEvaluationService
    {
        private readonly AppDbContext _context;

        public EvaluationService(AppDbContext context)
        {
            _context = context;
        }

        // إضافة تقييم جديد (إجابة نصية)
        public async Task<Evaluation> AddEvaluation(EvaluationDTO dto)
        {
            if (dto.StudentId <= 0 || dto.LecturerId <= 0 || dto.SubjectId <= 0 || dto.QuestionId <= 0)
            {
                throw new InvalidOperationException("بيانات التقييم غير مكتملة (معرفات غير صالحة).");
            }

            if (string.IsNullOrWhiteSpace(dto.TextAnswer))
            {
                throw new InvalidOperationException("الإجابة مطلوبة.");
            }

            // التحقق هل قام الطالب بتقييم هذا السؤال لنفس الدكتور من قبل؟ (بغض النظر عن المادة)
            var exists = await _context.Evaluations
                .AnyAsync(e => e.StudentId == dto.StudentId &&
                               e.LecturerId == dto.LecturerId &&
                               e.QuestionId == dto.QuestionId &&
                               !e.IsArchived);

            if (exists)
            {
                throw new InvalidOperationException("لقد قمت بتقييم هذا السؤال للمحاضر مسبقاً.");
            }

            var evaluation = new Evaluation
            {
                TextAnswer = dto.TextAnswer,
                StudentId = dto.StudentId,
                QuestionId = dto.QuestionId,
                LecturerId = dto.LecturerId,
                SubjectId = dto.SubjectId,
                IsArchived = false
            };

            _context.Evaluations.Add(evaluation);
            await _context.SaveChangesAsync();
            return evaluation;
        }

        // إعادة ضبط التقييمات (أرشفة الدورة الحالية)
        // أرشفة كل التقييمات وإعادة تقييم المحاضرين لـ 0
        public async Task<bool> ResetEvaluations()
        {
            var activeEvaluations = await _context.Evaluations
                .Where(e => !e.IsArchived)
                .ToListAsync();

            if (activeEvaluations.Any())
            {
                // أرشفة التقييمات
                foreach (var eval in activeEvaluations)
                {
                    eval.IsArchived = true;
                }

                // إعادة تقييم المحاضرين لـ 0
                var lecturers = await _context.Lecturers.ToListAsync();
                foreach (var lecturer in lecturers)
                {
                    lecturer.AdminRating = null;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // الحصول على تقييمات محاضر معين (للمحاضر نفسه يشوف إجابات الطلاب)
        public async Task<List<EvaluationResponseDTO>> GetEvaluationsByLecturer(int lecturerId)
        {
            return await _context.Evaluations.AsNoTracking()
                .Include(e => e.Question)
                .Include(e => e.Subject)
                .Where(e => e.LecturerId == lecturerId && !e.IsArchived)
                .Select(e => new EvaluationResponseDTO
                {
                    EvaluationId = e.EvaluationId,
                    TextAnswer = e.TextAnswer,
                    IsArchived = e.IsArchived,
                    LecturerId = e.LecturerId,
                    StudentName = "طالب", // إخفاء هوية الطالب
                    LecturerName = "",
                    SubjectName = e.Subject.SubjectName,
                    QuestionText = e.Question.QuestionText
                })
                .ToListAsync();
        }

        // الحصول على تقرير التقييمات لكل محاضر (للأدمن)
        public async Task<List<EvaluationReportDTO>> GetEvaluationReport()
        {
            var lecturers = await _context.Lecturers.AsNoTracking()
                .Select(l => new EvaluationReportDTO
                {
                    LecturerId = l.LecturerId,
                    LecturerName = l.FirstName + " " + l.LastName,
                    SubjectName = "", // يمكن تعديلها لاحقاً
                    AverageRating = l.AdminRating ?? 0, // تقييم الأدمن
                    TotalEvaluations = _context.Evaluations.Count(e => e.LecturerId == l.LecturerId && !e.IsArchived)
                })
                .ToListAsync();

            return lecturers;
        }

        // الحصول على كل التقييمات (الإجابات النصية)
        public async Task<List<EvaluationResponseDTO>> GetAllEvaluations()
        {
            return await _context.Evaluations.AsNoTracking()
                .Include(e => e.Student)
                .Include(e => e.Lecturer)
                .Include(e => e.Subject)
                .Include(e => e.Question)
                .Where(e => !e.IsArchived)
                .Select(e => new EvaluationResponseDTO
                {
                    EvaluationId = e.EvaluationId,
                    TextAnswer = e.TextAnswer,
                    IsArchived = e.IsArchived,
                    LecturerId = e.LecturerId,
                    StudentName = e.Student.FirstName + " " + e.Student.LastName,
                    LecturerName = e.Lecturer.FirstName + " " + e.Lecturer.LastName,
                    SubjectName = e.Subject.SubjectName,
                    QuestionText = e.Question.QuestionText
                })
                .ToListAsync();
        }
    }
}
