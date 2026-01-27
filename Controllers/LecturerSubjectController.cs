using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfRate.Data;
using ProfRate.Entities;

namespace ProfRate.Controllers
{
    [Route("api/lecturersubjects")]
    [ApiController]
    public class LecturerSubjectController : ControllerBase
    {
        private readonly ProfRate.Services.LecturerSubjectService _service;

        public LecturerSubjectController(ProfRate.Services.LecturerSubjectService service)
        {
            _service = service;
        }

        // GET: api/lecturersubjects/GetAll
        [HttpGet]
        [Route("GetAll")]
        [Authorize] // يتطلب تسجيل الدخول
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAll();
            return Ok(list);
        }

        // GET: api/lecturersubjects/GetByLecturer/5
        [HttpGet]
        [Route("GetByLecturer/{lecturerId}")]
        [Authorize] // يتطلب تسجيل الدخول
        public async Task<IActionResult> GetByLecturer(int lecturerId)
        {
            var list = await _service.GetByLecturer(lecturerId);
            return Ok(list);
        }

        // GET: api/lecturersubjects/GetBySubject/5
        [HttpGet]
        [Route("GetBySubject/{subjectId}")]
        [Authorize] // يتطلب تسجيل الدخول
        public async Task<IActionResult> GetBySubject(int subjectId)
        {
            var list = await _service.GetBySubject(subjectId);
            return Ok(list);
        }

        // POST: api/lecturersubjects/Add
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = "Admin")] // الأدمن فقط
        public async Task<IActionResult> Add([FromBody] ProfRate.DTOs.LecturerSubjectDTO model)
        {
            var result = await _service.AddLecturerSubject(model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(new { message = result.Message });
        }

        // PUT: api/lecturersubjects/Update/5
        [HttpPut]
        [Route("Update/{id}")]
        [Authorize(Roles = "Admin")] // الأدمن فقط
        public async Task<IActionResult> Update(int id, [FromBody] ProfRate.DTOs.LecturerSubjectDTO model)
        {
            var result = await _service.UpdateLecturerSubject(id, model);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(new { message = result.Message });
        }

        // DELETE: api/lecturersubjects/Delete/5
        [HttpDelete]
        [Route("Delete/{id}")]
        [Authorize(Roles = "Admin")] // الأدمن فقط
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteLecturerSubject(id);
            if (!success)
            {
                return NotFound(new { message = "غير موجود" });
            }
            return Ok(new { message = "تم الحذف بنجاح" });
        }
    }
}

