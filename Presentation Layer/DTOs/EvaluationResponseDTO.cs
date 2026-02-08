namespace ProfRate.DTOs
{
    public class EvaluationResponseDTO
    {
        public int EvaluationId { get; set; }
        public string TextAnswer { get; set; } = ""; // إجابة الطالب النصية
        public bool IsArchived { get; set; }
        
        public int LecturerId { get; set; } // لحساب عدد الإجابات
        public string StudentName { get; set; } = "";
        public string ParticpantName { get; set; } = ""; 
        public string QuestionText { get; set; } = "";
        public string LecturerName { get; set; } = "";
        public string SubjectName { get; set; } = "";
    }
}
