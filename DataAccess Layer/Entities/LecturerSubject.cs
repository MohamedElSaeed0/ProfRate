namespace ProfRate.Entities
{
    // جدول العلاقة بين المحاضرين والمواد (Many-to-Many)
    public class LecturerSubject
    {
        public int LecturerSubjectId { get; set; }

        // Foreign Keys
        public int LecturerId { get; set; }
        public int SubjectId { get; set; }

        // Navigation Properties
        public Lecturer Lecturer { get; set; } = null!;
        public Subject Subject { get; set; } = null!;
    }
}
