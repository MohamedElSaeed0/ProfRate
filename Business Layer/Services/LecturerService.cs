using Microsoft.EntityFrameworkCore;
using ProfRate.Data;
using ProfRate.DTOs;
using ProfRate.Entities;

namespace ProfRate.Services
{
    // Service للـ Lecturers - إدارة المحاضرين
    public class LecturerService : ILecturerService
    {
        private readonly AppDbContext _context;

        public LecturerService(AppDbContext context)
        {
            _context = context;
        }

        // الحصول على كل المحاضرين
        public async Task<List<Lecturer>> GetAllLecturers()
        {
            return await _context.Lecturers
                .Include(l => l.LecturerSubjects)
                .ThenInclude(ls => ls.Subject)
                .AsNoTracking()
                .OrderBy(l => l.FirstName)
                .ToListAsync();
        }

        // البحث عن محاضرين (بالاسم أو اسم المستخدم)
        public async Task<List<Lecturer>> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Lecturer>();

            // Sanitize Input
            query = query.Trim();
            if (query.Length > 100) query = query.Substring(0, 100);
            query = System.Text.RegularExpressions.Regex.Replace(query, @"[^\w\s\u0600-\u06FF]", "");

            return await _context.Lecturers
                .Include(l => l.LecturerSubjects)
                .ThenInclude(ls => ls.Subject)
                .AsNoTracking()
                .Where(l => l.Username.Contains(query) ||
                            l.FirstName.Contains(query) ||
                            l.LastName.Contains(query))
                .OrderBy(l => l.FirstName)
                .Take(100)
                .ToListAsync();
        }

        // الحصول على محاضر بالـ ID
        public async Task<Lecturer?> GetLecturerById(int id)
        {
            return await _context.Lecturers.FindAsync(id);
        }

        // إضافة محاضر جديد
        public async Task<Lecturer> AddLecturer(LecturerDTO dto)
        {
            if (await _context.Lecturers.AnyAsync(l => l.Username == dto.Username))
                throw new InvalidOperationException("اسم المستخدم موجود بالفعل");

            var lecturer = new Lecturer
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username,
                Password = dto.Password, // Reverted to Plain Text
                Gender = dto.Gender,
                AdminId = dto.AdminId
            };

            _context.Lecturers.Add(lecturer);
            await _context.SaveChangesAsync();
            return lecturer;
        }

        // تعديل محاضر
        public async Task<Lecturer?> UpdateLecturer(int id, LecturerDTO dto)
        {
            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null) return null;

            if (await _context.Lecturers.AnyAsync(l => l.Username == dto.Username && l.LecturerId != id))
                throw new InvalidOperationException("اسم المستخدم موجود بالفعل");

            lecturer.FirstName = dto.FirstName;
            lecturer.LastName = dto.LastName;
            lecturer.Username = dto.Username;
            lecturer.Password = dto.Password; // Reverted to Plain Text
            lecturer.Gender = dto.Gender;

            await _context.SaveChangesAsync();
            return lecturer;
        }

        // حذف محاضر
        // حذف محاضر (مع حذف كل بياناته المرتبطة)
        public async Task<bool> DeleteLecturer(int id)
        {
            var lecturer = await _context.Lecturers
                .Include(l => l.Evaluations)
                .Include(l => l.LecturerSubjects)
                .FirstOrDefaultAsync(l => l.LecturerId == id);

            if (lecturer == null) return false;

            // Manual Cascade Delete
            if (lecturer.Evaluations.Any())
                _context.Evaluations.RemoveRange(lecturer.Evaluations);

            if (lecturer.LecturerSubjects.Any())
                _context.LecturerSubjects.RemoveRange(lecturer.LecturerSubjects);

            // كمان لازم نمسح ارتباطه بالطلاب في المواد
            var studentSubjects = await _context.StudentSubjects.Where(ss => ss.LecturerId == id).ToListAsync();
            if (studentSubjects.Any())
                _context.StudentSubjects.RemoveRange(studentSubjects);

            _context.Lecturers.Remove(lecturer);
            await _context.SaveChangesAsync();
            return true;
        }

        // تحديث تقييم الأدمن للمحاضر
        public async Task<Lecturer?> UpdateAdminRating(int id, int rating)
        {
            if (rating < 0 || rating > 100)
                throw new InvalidOperationException("التقييم يجب أن يكون بين 0 و 100");

            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null) return null;

            // لو التقييم 0، نعتبره reset ونخليه null
            lecturer.AdminRating = rating == 0 ? null : rating;
            await _context.SaveChangesAsync();
            return lecturer;
        }
    }
}
