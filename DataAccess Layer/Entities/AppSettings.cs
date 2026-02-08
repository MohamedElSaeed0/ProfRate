namespace ProfRate.Entities
{
    // إعدادات التطبيق
    public class AppSettings
    {
        public int Id { get; set; }
        public bool IsEvaluationOpen { get; set; } = true; // هل التقييم مفتوح للطلاب؟
    }
}
