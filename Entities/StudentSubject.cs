namespace ProfRate.Entities
{
    // جدول العلاقة بين الطلاب والمواد (Many-to-Many)
    public class StudentSubject
    {
        public int StudentSubjectId { get; set; }

        // Foreign Keys
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int? LecturerId { get; set; }

        // Navigation Properties
        public Student Student { get; set; } = null!;
        public Subject Subject { get; set; } = null!;
        public Lecturer? Lecturer { get; set; }
    }
}
