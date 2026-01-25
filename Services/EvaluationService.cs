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

        // إضافة تقييم جديد
        public async Task<Evaluation> AddEvaluation(EvaluationDTO dto)
        {
            var evaluation = new Evaluation
            {
                Rating = dto.Rating,
                StudentId = dto.StudentId,
                QuestionId = dto.QuestionId,
                LecturerId = dto.LecturerId,
                SubjectId = dto.SubjectId
            };

            _context.Evaluations.Add(evaluation);
            await _context.SaveChangesAsync();
            return evaluation;
        }

        // الحصول على تقييمات محاضر معين
        public async Task<List<Evaluation>> GetEvaluationsByLecturer(int lecturerId)
        {
            return await _context.Evaluations
                .Where(e => e.LecturerId == lecturerId)
                .Include(e => e.Student)
                .Include(e => e.Question)
                .Include(e => e.Subject)
                .ToListAsync();
        }

        // الحصول على تقرير التقييمات لكل محاضر
        public async Task<List<EvaluationReportDTO>> GetEvaluationReport()
        {
            var report = await _context.Evaluations
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

        // الحصول على كل التقييمات
        public async Task<List<Evaluation>> GetAllEvaluations()
        {
            return await _context.Evaluations
                .Include(e => e.Student)
                .Include(e => e.Lecturer)
                .Include(e => e.Question)
                .Include(e => e.Subject)
                .ToListAsync();
        }
    }
}
