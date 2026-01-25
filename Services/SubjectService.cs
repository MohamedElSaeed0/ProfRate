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
            return await _context.Subjects.ToListAsync();
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

        // تعديل مادة
        public async Task<Subject?> UpdateSubject(int id, SubjectDTO dto)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return null;

            subject.SubjectName = dto.SubjectName;

            await _context.SaveChangesAsync();
            return subject;
        }

        // حذف مادة
        public async Task<bool> DeleteSubject(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return false;

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
