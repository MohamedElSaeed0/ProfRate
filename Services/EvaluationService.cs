using Microsoft.EntityFrameworkCore;
using ProfRate.Data;
using ProfRate.DTOs;
using ProfRate.Entities;

namespace ProfRate.Services
{
    // Service للـ Evaluations - إدارة التقييمات
    public class EvaluationService
    {
        private readonly AppDbContext _context;

        public EvaluationService(AppDbContext context)
        {
            _context = context;
        }

        // إضافة تقييم جديد مع التحقق من التكرار
        public async Task<Evaluation> AddEvaluation(EvaluationDTO dto)
        {
            if (dto.StudentId <= 0 || dto.LecturerId <= 0 || dto.SubjectId <= 0 || dto.QuestionId <= 0)
            {
                throw new InvalidOperationException("بيانات التقييم غير مكتملة (معرفات غير صالحة).");
            }

            // التحقق هل قام الطالب بتقييم هذا السؤال لنفس الدكتور والمادة من قبل في الدورة الحالية؟
            var exists = await _context.Evaluations
                .AnyAsync(e => e.StudentId == dto.StudentId && 
                               e.LecturerId == dto.LecturerId && 
                               e.SubjectId == dto.SubjectId && 
                               e.QuestionId == dto.QuestionId &&
                               !e.IsArchived);

            if (exists)
            {
                throw new InvalidOperationException("لقد قمت بتقييم هذا السؤال للمحاضر والمادة مسبقاً.");
            }

            var evaluation = new Evaluation
            {
                Rating = dto.Rating,
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
        public async Task<bool> ResetEvaluations()
        {
            // الحصول على كل التقييمات النشطة
            var activeEvaluations = await _context.Evaluations
                .Where(e => !e.IsArchived)
                .ToListAsync();

            if (activeEvaluations.Any())
            {
                foreach (var eval in activeEvaluations)
                {
                    eval.IsArchived = true;
                }
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // الحصول على تقييمات محاضر معين (مجمعة وبدون أسماء طلاب)
        public async Task<List<EvaluationResponseDTO>> GetEvaluationsByLecturer(int lecturerId)
        {
            return await _context.Evaluations
                .Include(e => e.Student)
                .Include(e => e.Lecturer)
                .Include(e => e.Subject)
                .Where(e => e.LecturerId == lecturerId && !e.IsArchived)
                .GroupBy(e => new { 
                    e.StudentId, 
                    // No need for Student details here as it should be anonymous
                    e.SubjectId, 
                    SubjectName = e.Subject.SubjectName 
                })
                .Select(g => new EvaluationResponseDTO
                {
                    EvaluationId = g.First().EvaluationId,
                    Rating = (int)Math.Round(g.Average(e => e.Rating)),
                    IsArchived = false,
                    StudentName = "طالب", // إخفاء هوية الطالب
                    LecturerName = "", // Not needed for own view
                    SubjectName = g.Key.SubjectName,
                    QuestionText = $"تقييم عام ({g.Count()} أسئلة)"
                })
                .ToListAsync();
        }

        // الحصول على تقرير التقييمات لكل محاضر
        public async Task<List<EvaluationReportDTO>> GetEvaluationReport()
        {
            var report = await _context.Evaluations
                .Where(e => !e.IsArchived) // فقط التقييمات النشطة
                .GroupBy(e => new { e.LecturerId, e.Lecturer.FirstName, e.Lecturer.LastName, e.SubjectId, e.Subject.SubjectName })
                .Select(g => new EvaluationReportDTO
                {
                    LecturerId = g.Key.LecturerId,
                    LecturerName = g.Key.FirstName + " " + g.Key.LastName,
                    SubjectName = g.Key.SubjectName,
                    AverageRating = g.Average(e => e.Rating),
                    TotalEvaluations = g.Count()
                })
                .ToListAsync();

            return report;
        }

        // الحصول على كل التقييمات (مجمعة لكل طالب/دكتور/مادة)
        public async Task<List<EvaluationResponseDTO>> GetAllEvaluations()
        {
            return await _context.Evaluations
                .Include(e => e.Student)
                .Include(e => e.Lecturer)
                .Include(e => e.Subject)
                .Where(e => !e.IsArchived)
                .GroupBy(e => new { 
                    e.StudentId, 
                    StudentFirstName = e.Student.FirstName, 
                    StudentLastName = e.Student.LastName,
                    e.LecturerId, 
                    LecturerFirstName = e.Lecturer.FirstName, 
                    LecturerLastName = e.Lecturer.LastName,
                    e.SubjectId, 
                    SubjectName = e.Subject.SubjectName 
                })
                .Select(g => new EvaluationResponseDTO
                {
                    EvaluationId = g.First().EvaluationId, // مجرد ID تمثيلي
                    Rating = (int)Math.Round(g.Average(e => e.Rating)), // متوسط التقييم للأيئلة
                    IsArchived = false,
                    StudentName = g.Key.StudentFirstName + " " + g.Key.StudentLastName,
                    LecturerName = g.Key.LecturerFirstName + " " + g.Key.LecturerLastName,
                    SubjectName = g.Key.SubjectName,
                    QuestionText = $"تقييم عام ({g.Count()} أسئلة)" // نص توضيحي بدل سؤال محدد
                })
                .ToListAsync();
        }
    }
}
