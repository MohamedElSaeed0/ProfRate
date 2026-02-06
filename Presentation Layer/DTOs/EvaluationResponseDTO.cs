namespace ProfRate.DTOs
{
    public class EvaluationResponseDTO
    {
        public int EvaluationId { get; set; }
        public int Rating { get; set; }
        public bool IsArchived { get; set; }
        
        public string StudentName { get; set; } = "";
        public string ParticpantName { get; set; } = ""; 
        public string QuestionText { get; set; } = "";
        public string LecturerName { get; set; } = "";
        public string SubjectName { get; set; } = "";
    }
}
