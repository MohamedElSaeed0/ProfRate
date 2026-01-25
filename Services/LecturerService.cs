using Microsoft.EntityFrameworkCore;
using ProfRate.Data;
using ProfRate.DTOs;
using ProfRate.Entities;

namespace ProfRate.Services
{
    // Service للـ Lecturers - إدارة المحاضرين
    public class LecturerService
    {
        private readonly AppDbContext _context;

        public LecturerService(AppDbContext context)
        {
            _context = context;
        }

        // الحصول على كل المحاضرين
        public async Task<List<Lecturer>> GetAllLecturers()
        {
            return await _context.Lecturers.ToListAsync();
        }

        // الحصول على محاضر بالـ ID
        public async Task<Lecturer?> GetLecturerById(int id)
        {
            return await _context.Lecturers.FindAsync(id);
        }

        // إضافة محاضر جديد
        public async Task<Lecturer> AddLecturer(LecturerDTO dto)
        {
            var lecturer = new Lecturer
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username,
                Password = dto.Password,
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

            lecturer.FirstName = dto.FirstName;
            lecturer.LastName = dto.LastName;
            lecturer.Username = dto.Username;
            lecturer.Password = dto.Password;

            await _context.SaveChangesAsync();
            return lecturer;
        }

        // حذف محاضر
        public async Task<bool> DeleteLecturer(int id)
        {
            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null) return false;

            _context.Lecturers.Remove(lecturer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
