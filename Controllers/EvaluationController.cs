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
        private readonly EvaluationService _evaluationService;

        public EvaluationController(EvaluationService evaluationService)
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
        public async Task<IActionResult> AddEvaluation([FromBody] EvaluationDTO dto)
        {
            var evaluation = await _evaluationService.AddEvaluation(dto);
            return Ok(new { message = "تمت إضافة التقييم بنجاح", evaluation });
        }
    }
}
