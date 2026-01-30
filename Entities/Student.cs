using System.ComponentModel.DataAnnotations;

namespace ProfRate.Entities
{
    // الطالب - اللي بيعمل التقييم
    public class Student
    {
        public int StudentId { get; set; }
        
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Gender { get; set; } = "Male"; // Male or Female

        // Foreign Key
        public int AdminId { get; set; }

        // Navigation Properties
        public Admin Admin { get; set; } = null!;
        public List<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
        public List<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
    }
}
