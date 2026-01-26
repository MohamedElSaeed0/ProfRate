using System.ComponentModel.DataAnnotations;

namespace ProfRate.Entities
{
    // المادة الدراسية
    public class Subject
    {
        public int SubjectId { get; set; }
        
        [MaxLength(50)]
        public string SubjectName { get; set; } = string.Empty;

        // Navigation Properties
        public List<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
        public List<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
        public List<LecturerSubject> LecturerSubjects { get; set; } = new List<LecturerSubject>();
    }
}
