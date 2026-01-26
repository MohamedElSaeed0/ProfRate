using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProfRate.Data;
using ProfRate.DTOs;
using ProfRate.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProfRate.Services
{
    // Service للـ Authentication - تسجيل الدخول وإنشاء الـ Token
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // تسجيل الدخول
        public async Task<LoginResponseDTO> Login(LoginDTO loginDto)
        {
            // البحث عن المستخدم حسب نوعه
            if (loginDto.UserType == "Admin")
            {
                var admin = await _context.Admins
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.Username == loginDto.Username && a.Password == loginDto.Password);

                if (admin != null)
                {
                    var token = GenerateToken(admin.AdminId, admin.Username, "Admin");
                    return new LoginResponseDTO
                    {
                        Success = true,
                        Message = "تم تسجيل الدخول بنجاح",
                        Token = token,
                        UserType = "Admin",
                        UserId = admin.AdminId
                    };
                }
            }
            else if (loginDto.UserType == "Student")
            {
                var student = await _context.Students
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.Username == loginDto.Username);

                if (student != null && BCrypt.Net.BCrypt.Verify(loginDto.Password, student.Password))
                {
                    var token = GenerateToken(student.StudentId, student.Username, "Student");
                    return new LoginResponseDTO
                    {
                        Success = true,
                        Message = "تم تسجيل الدخول بنجاح",
                        Token = token,
                        UserType = "Student",
                        UserId = student.StudentId
                    };
                }
            }
            else if (loginDto.UserType == "Lecturer")
            {
                var lecturer = await _context.Lecturers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(l => l.Username == loginDto.Username);

                if (lecturer != null && BCrypt.Net.BCrypt.Verify(loginDto.Password, lecturer.Password))
                {
                    var token = GenerateToken(lecturer.LecturerId, lecturer.Username, "Lecturer");
                    return new LoginResponseDTO
                    {
                        Success = true,
                        Message = "تم تسجيل الدخول بنجاح",
                        Token = token,
                        UserType = "Lecturer",
                        UserId = lecturer.LecturerId
                    };
                }
            }

            // لو مش موجود
            return new LoginResponseDTO
            {
                Success = false,
                Message = "اسم المستخدم أو كلمة المرور غير صحيحة",
                Token = "",
                UserType = "",
                UserId = 0
            };
        }

        // إنشاء الـ JWT Token
        private string GenerateToken(int userId, string username, string userType)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("UserId", userId.ToString()),
                new Claim("Username", username),
                new Claim("UserType", userType),
                new Claim(ClaimTypes.Role, userType)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
