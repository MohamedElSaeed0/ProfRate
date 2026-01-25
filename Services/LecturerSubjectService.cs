using Microsoft.EntityFrameworkCore;
using ProfRate.Data;
using ProfRate.DTOs;
using ProfRate.Entities;

namespace ProfRate.Services
{
    // Service للـ LecturerSubjects - إدارة ربط المحاضرين بالمواد
    public class LecturerSubjectService
    {
        private readonly AppDbContext _context;

        public LecturerSubjectService(AppDbContext context)
        {
            _context = context;
        }

        // الحصول على كل الارتباطات
        public async Task<List<LecturerSubject>> GetAll()
        {
            return await _context.LecturerSubjects
                .Include(ls => ls.Lecturer)
                .Include(ls => ls.Subject)
                .ToListAsync();
        }

        // الحصول على مواد محاضر معين
        public async Task<List<LecturerSubject>> GetByLecturer(int lecturerId)
        {
            return await _context.LecturerSubjects
                .Where(ls => ls.LecturerId == lecturerId)
                .Include(ls => ls.Subject)
                .ToListAsync();
        }

        // الحصول على محاضرين مادة معينة
        public async Task<List<LecturerSubject >> GetBySubject(int subjectId)
        {
            return await _context.LecturerSubjects
                .Where(ls => ls.SubjectId == subjectId)
                .Include(ls => ls.Lecturer)
                .ToListAsync();
        }

        // إضافة ربط جديد
        public async Task<(bool Success, string Message)> AddLecturerSubject(LecturerSubjectDTO dto)
        {
            var exists = await _context.LecturerSubjects
                .AnyAsync(ls => ls.LecturerId == dto.LecturerId && ls.SubjectId == dto.SubjectId);
            
            if (exists)
            {
                return (false, "هذا المحاضر معين بالفعل لهذه المادة");
            }

            var entry = new LecturerSubject
            {
                LecturerId = dto.LecturerId,
                SubjectId = dto.SubjectId
            };

            _context.LecturerSubjects.Add(entry);
            await _context.SaveChangesAsync();
            return (true, "تم ربط المحاضر بالمادة بنجاح");
        }

        // حذف ربط
        public async Task<bool> DeleteLecturerSubject(int id)
        {
            var entry = await _context.LecturerSubjects.FindAsync(id);
            if (entry == null) return false;

            _context.LecturerSubjects.Remove(entry);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
