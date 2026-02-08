using Microsoft.EntityFrameworkCore;
using ProfRate.Data;
using ProfRate.Entities;

namespace ProfRate.Services
{
    public interface IAppSettingsService
    {
        Task<bool> IsEvaluationOpen();
        Task<bool> ToggleEvaluation(bool isOpen);
    }

    public class AppSettingsService : IAppSettingsService
    {
        private readonly AppDbContext _context;

        public AppSettingsService(AppDbContext context)
        {
            _context = context;
        }

        // التحقق هل التقييم مفتوح؟
        public async Task<bool> IsEvaluationOpen()
        {
            var settings = await _context.AppSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                // لو مفيش إعدادات، نعمل واحدة افتراضية (مفتوح)
                settings = new AppSettings { IsEvaluationOpen = true };
                _context.AppSettings.Add(settings);
                await _context.SaveChangesAsync();
            }
            return settings.IsEvaluationOpen;
        }

        // فتح/قفل التقييم
        public async Task<bool> ToggleEvaluation(bool isOpen)
        {
            var settings = await _context.AppSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                settings = new AppSettings { IsEvaluationOpen = isOpen };
                _context.AppSettings.Add(settings);
            }
            else
            {
                settings.IsEvaluationOpen = isOpen;
            }
            await _context.SaveChangesAsync();
            return settings.IsEvaluationOpen;
        }
    }
}
