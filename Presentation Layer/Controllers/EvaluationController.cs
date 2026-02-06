using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfRate.DTOs;
using ProfRate.Services;

namespace ProfRate.Controllers
{
    [Route("api/evaluations")]
    [ApiController]
    public class EvaluationController : ControllerBase
    {
        private readonly IEvaluationService _evaluationService;

        public EvaluationController(IEvaluationService evaluationService)
        {
            _evaluationService = evaluationService;
        }

        // GET: api/evaluations/GetAll
        // الحصول على كل التقييمات
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllEvaluations()
        {
            var evaluations = await _evaluationService.GetAllEvaluations();
            return Ok(evaluations);
        }

        // GET: api/evaluations/GetByLecturer/5
        // الحصول على تقييمات محاضر معين
        [HttpGet]
        [Route("GetByLecturer/{lecturerId}")]
        public async Task<IActionResult> GetEvaluationsByLecturer(int lecturerId)
        {
            var evaluations = await _evaluationService.GetEvaluationsByLecturer(lecturerId);
            return Ok(evaluations);
        }

        // GET: api/evaluations/GetReport
        // الحصول على تقرير التقييمات
        [HttpGet]
        [Route("GetReport")]
        public async Task<IActionResult> GetEvaluationReport()
        {
            var report = await _evaluationService.GetEvaluationReport();
            return Ok(report);
        }

        // POST: api/evaluations/Add
        // إضافة تقييم جديد
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = "Student")] // الطلاب فقط هم من يقيمون
        public async Task<IActionResult> AddEvaluation([FromBody] EvaluationDTO dto)
        {
            try
            {
                var evaluation = await _evaluationService.AddEvaluation(dto);
                return Ok(new { message = "تمت إضافة التقييم بنجاح", evaluation });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/evaluations/Reset
        // إعادة ضبط التقييمات للأدمن فقط
        [HttpPost]
        [Route("Reset")]
        [Authorize(Roles = "Admin")] // تفعيل الصلاحية للأدمن فقط
        public async Task<IActionResult> ResetEvaluations()
        {
            var result = await _evaluationService.ResetEvaluations();
            return Ok(new { message = result ? "تمت أرشفة التقييمات السابقة وبدء دورة جديدة بنجاح" : "لا توجد تقييمات نشطة للأرشفة" });
        }
    }
}
