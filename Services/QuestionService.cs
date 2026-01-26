using Microsoft.EntityFrameworkCore;
using ProfRate.Data;
using ProfRate.DTOs;
using ProfRate.Entities;

namespace ProfRate.Services
{
    // Service للـ Questions - إدارة الأسئلة
    public class QuestionService
    {
        private readonly AppDbContext _context;

        public QuestionService(AppDbContext context)
        {
            _context = context;
        }

        // الحصول على كل الأسئلة
        public async Task<List<Question>> GetAllQuestions()
        {
            return await _context.Questions.ToListAsync();
        }

        // الحصول على سؤال بالـ ID
        public async Task<Question?> GetQuestionById(int id)
        {
            return await _context.Questions.FindAsync(id);
        }

        // إضافة سؤال جديد
        public async Task<Question> AddQuestion(QuestionDTO dto)
        {
            var question = new Question
            {
                QuestionText = dto.QuestionText,
                AdminId = dto.AdminId
            };

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return question;
        }

        // تعديل سؤال
        public async Task<Question?> UpdateQuestion(int id, QuestionDTO dto)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null) return null;

            question.QuestionText = dto.QuestionText;

            await _context.SaveChangesAsync();
            return question;
        }

        // حذف سؤال
        // حذف سؤال (مع حذف تقييماته المرتبطة)
        public async Task<bool> DeleteQuestion(int id)
        {
            var question = await _context.Questions
                .Include(q => q.Evaluations)
                .FirstOrDefaultAsync(q => q.QuestionId == id);

            if (question == null) return false;

            if (question.Evaluations.Any())
                _context.Evaluations.RemoveRange(question.Evaluations);

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
