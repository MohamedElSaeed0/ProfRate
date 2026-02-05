using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfRate.DTOs;
using ProfRate.Services;

namespace ProfRate.Controllers
{
    [Route("api/subjects")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        // GET: api/subjects/GetAll
        // الحصول على كل المواد
        [HttpGet]
        [Route("GetAll")]
        [Authorize] // يتطلب تسجيل الدخول
        public async Task<IActionResult> GetAllSubjects()
        {
            var subjects = await _subjectService.GetAllSubjects();
            return Ok(subjects);
        }

        // GET: api/subjects/GetById/5
        // الحصول على مادة بالـ ID
        [HttpGet]
        [Route("GetById/{id}")]
        [Authorize] // يتطلب تسجيل الدخول
        public async Task<IActionResult> GetSubjectById(int id)
        {
            var subject = await _subjectService.GetSubjectById(id);
            if (subject == null)
            {
                return NotFound(new { message = "المادة غير موجودة" });
            }
            return Ok(subject);
        }

        // POST: api/subjects/Add
        // إضافة مادة جديدة
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = "Admin")] // الأدمن فقط يقدر يضيف
        public async Task<IActionResult> AddSubject([FromBody] SubjectDTO dto)
        {
            var subject = await _subjectService.AddSubject(dto);
            if (subject == null)
            {
                return BadRequest(new { message = "المادة موجودة مسبقاً" });
            }
            return Ok(new { message = "تمت إضافة المادة بنجاح", subject });
        }



        // DELETE: api/subjects/Delete/5
        // حذف مادة
        [HttpDelete]
        [Route("Delete/{id}")]
        [Authorize(Roles = "Admin")] // الأدمن فقط يقدر يحذف
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var result = await _subjectService.DeleteSubject(id);
            if (!result)
            {
                return NotFound(new { message = "المادة غير موجودة" });
            }
            return Ok(new { message = "تم حذف المادة بنجاح" });
        }
    }
}

