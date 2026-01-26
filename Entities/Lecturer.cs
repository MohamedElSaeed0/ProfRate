using System.ComponentModel.DataAnnotations;

namespace ProfRate.Entities
{
    // المحاضر - اللي بيتقيم
    public class Lecturer
    {
        public int LecturerId { get; set; }
        
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty;

        // Foreign Key
        public int AdminId { get; set; }

        // Navigation Properties
        public Admin Admin { get; set; } = null!;
        public List<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
        public List<LecturerSubject> LecturerSubjects { get; set; } = new List<LecturerSubject>();
    }
}
