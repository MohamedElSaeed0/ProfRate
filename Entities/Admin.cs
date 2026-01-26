namespace ProfRate.Entities
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        // Navigation Properties
        public List<Student> Students { get; set; } = new List<Student>();
        public List<Lecturer> Lecturers { get; set; } = new List<Lecturer>();
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}
