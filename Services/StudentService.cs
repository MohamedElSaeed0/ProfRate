using Microsoft.EntityFrameworkCore;
using ProfRate.Data;
using ProfRate.DTOs;
using ProfRate.Entities;

namespace ProfRate.Services
{
    // Service للـ Students - إدارة الطلاب
    public class StudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        // الحصول على كل الطلاب
        public async Task<List<Student>> GetAllStudents()
        {
            return await _context.Students.AsNoTracking().OrderBy(s => s.FirstName).ToListAsync();
        }

        // البحث عن طلاب (بالاسم أو اسم المستخدم)
        public async Task<List<Student>> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Student>();

            return await _context.Students
                .AsNoTracking()
                .Where(s => s.Username.Contains(query) || 
                            (s.FirstName + " " + s.LastName).Contains(query))
                .OrderBy(s => s.FirstName)
                .ToListAsync();
        }

        // الحصول على طالب بالـ ID
        public async Task<Student?> GetStudentById(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        // إضافة طالب جديد
        public async Task<Student> AddStudent(StudentDTO dto)
        {
            var student = new Student
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username,
                Password = dto.Password,
                AdminId = dto.AdminId
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        // تعديل طالب
        public async Task<Student?> UpdateStudent(int id, StudentDTO dto)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == id);
            if (student == null) return null;

            student.FirstName = dto.FirstName;
            student.LastName = dto.LastName;
            student.Username = dto.Username;
            student.Password = dto.Password;

            await _context.SaveChangesAsync();
            return student;
        }

        // حذف طالب
        // حذف طالب (مع حذف كل بياناته المرتبطة)
        public async Task<bool> DeleteStudent(int id)
        {
            var student = await _context.Students
                .Include(s => s.Evaluations)
                .Include(s => s.StudentSubjects)
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null) return false;

            // Manual Cascade Delete
            if (student.Evaluations.Any())
                _context.Evaluations.RemoveRange(student.Evaluations);

            if (student.StudentSubjects.Any())
                _context.StudentSubjects.RemoveRange(student.StudentSubjects);

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
