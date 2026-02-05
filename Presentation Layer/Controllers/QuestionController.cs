using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfRate.DTOs;
using ProfRate.Services;

namespace ProfRate.Controllers
{
    [Route("api/questions")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        // GET: api/questions/GetAll
        // الحصول على كل الأسئلة
        [HttpGet]
        [Route("GetAll")]
        [Authorize] // يتطلب تسجيل الدخول
        public async Task<IActionResult> GetAllQuestions()
        {
            var questions = await _questionService.GetAllQuestions();
            return Ok(questions);
        }

        // GET: api/questions/GetById/5
        // الحصول على سؤال بالـ ID
        [HttpGet]
        [Route("GetById/{id}")]
        [Authorize] // يتطلب تسجيل الدخول
        public async Task<IActionResult> GetQuestionById(int id)
        {
            var question = await _questionService.GetQuestionById(id);
            if (question == null)
            {
                return NotFound(new { message = "السؤال غير موجود" });
            }
            return Ok(question);
        }

        // POST: api/questions/Add
        // إضافة سؤال جديد
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = "Admin")] // الأدمن فقط يقدر يضيف
        public async Task<IActionResult> AddQuestion([FromBody] QuestionDTO dto)
        {
            var question = await _questionService.AddQuestion(dto);
            return Ok(new { message = "تمت إضافة السؤال بنجاح", question });
        }

        // PUT: api/questions/Update/5
        // تعديل سؤال
        [HttpPut]
        [Route("Update/{id}")]
        [Authorize(Roles = "Admin")] // الأدمن فقط يقدر يعدل
        public async Task<IActionResult> UpdateQuestion(int id, [FromBody] QuestionDTO dto)
        {
            var question = await _questionService.UpdateQuestion(id, dto);
            if (question == null)
            {
                return NotFound(new { message = "السؤال غير موجود" });
            }
            return Ok(new { message = "تم تعديل السؤال بنجاح", question });
        }

        // DELETE: api/questions/Delete/5
        // حذف سؤال
        [HttpDelete]
        [Route("Delete/{id}")]
        [Authorize(Roles = "Admin")] // الأدمن فقط يقدر يحذف
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var result = await _questionService.DeleteQuestion(id);
            if (!result)
            {
                return NotFound(new { message = "السؤال غير موجود" });
            }
            return Ok(new { message = "تم حذف السؤال بنجاح" });
        }
    }
}

