using Microsoft.EntityFrameworkCore;
using ProfRate.Data;
using ProfRate.DTOs;
using ProfRate.Entities;

namespace ProfRate.Services
{
    // Service للـ Subjects - إدارة المواد
    public class SubjectService
    {
        private readonly AppDbContext _context;

        public SubjectService(AppDbContext context)
        {
            _context = context;
        }

        // الحصول على كل المواد
        public async Task<List<Subject>> GetAllSubjects()
        {
            return await _context.Subjects.AsNoTracking().ToListAsync();
        }

        // الحصول على مادة بالـ ID
        public async Task<Subject?> GetSubjectById(int id)
        {
            return await _context.Subjects.FindAsync(id);
        }

        // إضافة مادة جديدة
        public async Task<Subject> AddSubject(SubjectDTO dto)
        {
            var subject = new Subject
            {
                SubjectName = dto.SubjectName
            };

            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();
            return subject;
        }



        // حذف مادة
        // حذف مادة (مع حذف كل بياناتها المرتبطة)
        public async Task<bool> DeleteSubject(int id)
        {
            var subject = await _context.Subjects
                .Include(s => s.Evaluations)
                .Include(s => s.StudentSubjects)
                .Include(s => s.LecturerSubjects)
                .FirstOrDefaultAsync(s => s.SubjectId == id);

            if (subject == null) return false;

            // Manual Cascade Delete
            if (subject.Evaluations.Any())
                _context.Evaluations.RemoveRange(subject.Evaluations);

            if (subject.StudentSubjects.Any())
                _context.StudentSubjects.RemoveRange(subject.StudentSubjects);

            if (subject.LecturerSubjects.Any())
                _context.LecturerSubjects.RemoveRange(subject.LecturerSubjects);

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
