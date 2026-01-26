using Microsoft.EntityFrameworkCore;
using ProfRate.Data;
using ProfRate.DTOs;
using ProfRate.Entities;

namespace ProfRate.Services
{
    // Service للـ StudentSubjects - إدارة ربط الطلاب بالمواد
    public class StudentSubjectService
    {
        private readonly AppDbContext _context;

        public StudentSubjectService(AppDbContext context)
        {
            _context = context;
        }

        // الحصول على كل الارتباطات
        public async Task<List<StudentSubject>> GetAll()
        {
            return await _context.StudentSubjects
                .Include(ss => ss.Student)
                .Include(ss => ss.Subject)
                .Include(ss => ss.Lecturer)
                .ToListAsync();
        }

        // الحصول على مواد طالب معين
        public async Task<List<StudentSubject>> GetByStudent(int studentId)
        {
            return await _context.StudentSubjects
                .Where(ss => ss.StudentId == studentId)
                .Include(ss => ss.Subject)
                .Include(ss => ss.Lecturer)
                .ToListAsync();
        }

        // إضافة ربط جديد
        public async Task<(bool Success, string Message)> AddStudentSubject(StudentSubjectDTO dto)
        {
            // التحقق من عدم التكرار (نفس الطالب والمادة)
            var exists = await _context.StudentSubjects
                .AnyAsync(ss => ss.StudentId == dto.StudentId && ss.SubjectId == dto.SubjectId);

            if (exists)
            {
                return (false, "هذا الطالب مسجل بالفعل في هذه المادة (لا يمكن تسجيل المادة مرتين)");
            }

            var entry = new StudentSubject
            {
                StudentId = dto.StudentId,
                SubjectId = dto.SubjectId,
                LecturerId = dto.LecturerId
            };

            _context.StudentSubjects.Add(entry);
            await _context.SaveChangesAsync();
            return (true, "تم ربط الطالب بالمادة بنجاح");
        }

        // تعديل ربط
        public async Task<(bool Success, string Message)> UpdateStudentSubject(int id, StudentSubjectDTO dto)
        {
            var entry = await _context.StudentSubjects.FindAsync(id);
            if (entry == null) return (false, "هذا السجل غير موجود");

            // التحقق من عدم التكرار (لو غيرنا الطالب أو المادة)
            bool exists = await _context.StudentSubjects
                .AnyAsync(ss => ss.StudentId == dto.StudentId && ss.SubjectId == dto.SubjectId && ss.StudentSubjectId != id);

            if (exists)
            {
                return (false, "هذا الطالب مسجل بالفعل في هذه المادة");
            }

            entry.StudentId = dto.StudentId;
            entry.SubjectId = dto.SubjectId;
            entry.LecturerId = dto.LecturerId;

            await _context.SaveChangesAsync();
            return (true, "تم تعديل الربط بنجاح");
        }

        // حذف ربط
        public async Task<bool> DeleteStudentSubject(int id)
        {
            var entry = await _context.StudentSubjects.FindAsync(id);
            if (entry == null) return false;

            _context.StudentSubjects.Remove(entry);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
