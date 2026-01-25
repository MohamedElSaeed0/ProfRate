namespace ProfRate.DTOs
{
    // DTO لإضافة/تعديل طالب
    public class StudentDTO
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int AdminId { get; set; }
    }

    // DTO لإضافة/تعديل محاضر
    public class LecturerDTO
    {
        public int LecturerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int AdminId { get; set; }
    }

    // DTO لإضافة/تعديل سؤال
    public class QuestionDTO
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public int AdminId { get; set; }
    }

    // DTO لإضافة/تعديل مادة
    public class SubjectDTO
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
    }

    // DTO لإضافة تقييم
    public class EvaluationDTO
    {
        public int EvaluationId { get; set; }
        public int Rating { get; set; }
        public int StudentId { get; set; }
        public int QuestionId { get; set; }
        public int LecturerId { get; set; }
        public int SubjectId { get; set; }
    }

    // DTO لعرض تقرير التقييمات
    public class EvaluationReportDTO
    {
        public int LecturerId { get; set; }
        public string LecturerName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public int TotalEvaluations { get; set; }
    }

    public class StudentSubjectDTO
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int? LecturerId { get; set; }
    }

    public class LecturerSubjectDTO
    {
        public int LecturerId { get; set; }
        public int SubjectId { get; set; }
    }
}
