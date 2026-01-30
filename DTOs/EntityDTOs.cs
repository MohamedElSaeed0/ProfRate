using System.ComponentModel.DataAnnotations;

namespace ProfRate.DTOs
{
    // DTO لإضافة/تعديل طالب
    public class StudentDTO
    {
        public int StudentId { get; set; }
        
        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [StringLength(50, ErrorMessage = "الاسم يجب أن لا يزيد عن 50 حرف")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "الاسم الأخير مطلوب")]
        [StringLength(50, ErrorMessage = "الاسم يجب أن لا يزيد عن 50 حرف")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "اسم المستخدم يجب أن يكون بين 3 و 50 حرف")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "الجنس مطلوب")]
        public string Gender { get; set; } = "Male"; // Male or Female
        
        public int AdminId { get; set; }
    }

    // DTO لإضافة/تعديل محاضر
    public class LecturerDTO
    {
        public int LecturerId { get; set; }
        
        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "الاسم الأخير مطلوب")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [MinLength(6, ErrorMessage = "كلمة المرور قصيرة جداً")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "الجنس مطلوب")]
        public string Gender { get; set; } = "Male"; // Male or Female
        
        public int AdminId { get; set; }
    }

    // DTO لإضافة/تعديل سؤال
    public class QuestionDTO
    {
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "نص السؤال مطلوب")]
        public string QuestionText { get; set; } = string.Empty;
        
        public int AdminId { get; set; }
    }

    // DTO لإضافة/تعديل مادة
    public class SubjectDTO
    {
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "اسم المادة مطلوب")]
        public string SubjectName { get; set; } = string.Empty;
    }

    // DTO لإضافة تقييم
    public class EvaluationDTO
    {
        public int EvaluationId { get; set; }
        
        [Range(1, 5, ErrorMessage = "التقييم يجب أن يكون بين 1 و 5")]
        public byte Rating { get; set; }
        
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public int LecturerId { get; set; }
        [Required]
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
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int SubjectId { get; set; }
        public int? LecturerId { get; set; }
    }

    public class LecturerSubjectDTO
    {
        [Required]
        public int LecturerId { get; set; }
        [Required]
        public int SubjectId { get; set; }
    }

    // DTO للـ Response بدون كلمة المرور - للأمان
    public class StudentResponseDTO
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public int AdminId { get; set; }
    }

    // DTO للـ Response بدون كلمة المرور - للأمان
    public class LecturerResponseDTO
    {
        public int LecturerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public int AdminId { get; set; }
    }
}
