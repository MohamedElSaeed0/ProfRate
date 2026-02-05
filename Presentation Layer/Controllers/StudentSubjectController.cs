using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfRate.Data;
using ProfRate.Entities;
using ProfRate.Services;
namespace ProfRate.Controllers
{
    [Route("api/studentsubjects")]
    [ApiController]
    public class StudentSubjectController : ControllerBase
    {
        private readonly IStudentSubjectService _service;

        public StudentSubjectController(IStudentSubjectService service)
        {
            _service = service;
        }

        // GET: api/studentsubjects/GetAll
        [HttpGet]
        [Route("GetAll")]
        [Authorize] // يتطلب تسجيل الدخول
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAll();
            return Ok(list);
        }

        // GET: api/studentsubjects/GetByStudent/5
        [HttpGet]
        [Route("GetByStudent/{studentId}")]
        [Authorize] // يتطلب تسجيل الدخول
        public async Task<IActionResult> GetByStudent(int studentId)
        {
            var list = await _service.GetByStudent(studentId);
            return Ok(list);
        }

        // POST: api/studentsubjects/Add
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = "Admin")] // الأدمن فقط
        public async Task<IActionResult> Add([FromBody] ProfRate.DTOs.StudentSubjectDTO model)
        {
            var result = await _service.AddStudentSubject(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(new { message = result.Message });
        }

        // PUT: api/studentsubjects/Update/5
        [HttpPut]
        [Route("Update/{id}")]
        [Authorize(Roles = "Admin")] // الأدمن فقط
        public async Task<IActionResult> Update(int id, [FromBody] ProfRate.DTOs.StudentSubjectDTO model)
        {
            var result = await _service.UpdateStudentSubject(id, model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(new { message = result.Message });
        }

        // DELETE: api/studentsubjects/Delete/5
        [HttpDelete]
        [Route("Delete/{id}")]
        [Authorize(Roles = "Admin")] // الأدمن فقط
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteStudentSubject(id);
            if (!success)
            {
                return NotFound(new { message = "غير موجود" });
            }
            return Ok(new { message = "تم الحذف بنجاح" });
        }
    }
}

