namespace ProfRate.DTOs
{
    // DTO لتسجيل الدخول
    public class LoginDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty; // "Admin", "Student", "Lecturer"
    }

    // DTO للـ Response بعد تسجيل الدخول
    public class LoginResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
