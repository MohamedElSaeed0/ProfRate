using System.ComponentModel.DataAnnotations;

namespace ProfRate.Entities
{
    // التقييم - الجدول الرئيسي للتقييمات
    public class Evaluation
    {
        public int EvaluationId { get; set; }
        
        [MaxLength(400)]
        public string TextAnswer { get; set; } = string.Empty;  // إجابة الطالب النصية (400 حرف)
        
        public bool IsArchived { get; set; } = false;

        // Foreign Keys
        public int StudentId { get; set; }
        public int QuestionId { get; set; }
        public int LecturerId { get; set; }
        public int SubjectId { get; set; }

        // Navigation Properties
        public Student Student { get; set; } = null!;
        public Question Question { get; set; } = null!;
        public Lecturer Lecturer { get; set; } = null!;
        public Subject Subject { get; set; } = null!;
    }
}
