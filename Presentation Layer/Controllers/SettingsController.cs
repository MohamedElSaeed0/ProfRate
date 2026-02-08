using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfRate.Services;

namespace ProfRate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IAppSettingsService _settingsService;

        public SettingsController(IAppSettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        // GET: api/settings/IsEvaluationOpen
        // التحقق هل التقييم مفتوح (للجميع)
        [HttpGet]
        [Route("IsEvaluationOpen")]
        [AllowAnonymous] // متاح للجميع
        public async Task<IActionResult> IsEvaluationOpen()
        {
            var isOpen = await _settingsService.IsEvaluationOpen();
            return Ok(new { isOpen });
        }

        // POST: api/settings/OpenEvaluation
        // فتح التقييم (للأدمن فقط)
        [HttpPost]
        [Route("OpenEvaluation")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> OpenEvaluation()
        {
            await _settingsService.ToggleEvaluation(true);
            return Ok(new { message = "تم فتح التقييم للطلاب", isOpen = true });
        }

        // POST: api/settings/CloseEvaluation
        // قفل التقييم (للأدمن فقط)
        [HttpPost]
        [Route("CloseEvaluation")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CloseEvaluation()
        {
            await _settingsService.ToggleEvaluation(false);
            return Ok(new { message = "تم قفل التقييم", isOpen = false });
        }
    }
}
