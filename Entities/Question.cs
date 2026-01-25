namespace ProfRate.Entities
{
    // السؤال - أسئلة التقييم
    public class Question
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;

        // Foreign Key
        public int AdminId { get; set; }

        // Navigation Properties
        public Admin Admin { get; set; } = null!;
        public List<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
    }
}
