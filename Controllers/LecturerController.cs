using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfRate.DTOs;
using ProfRate.Services;

namespace ProfRate.Controllers
{
    [Route("api/lecturers")]
    [ApiController]
    public class LecturerController : ControllerBase
    {
        private readonly LecturerService _lecturerService;

        public LecturerController(LecturerService lecturerService)
        {
            _lecturerService = lecturerService;
        }

        // GET: api/lecturers/GetAll
        // الحصول على كل المحاضرين
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllLecturers()
        {
            var lecturers = await _lecturerService.GetAllLecturers();
            return Ok(lecturers);
        }

        // GET: api/lecturers/Search?query=...
        [HttpGet]
        [Route("Search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var lecturers = await _lecturerService.Search(query);
            return Ok(lecturers);
        }

        // GET: api/lecturers/GetById/5
        // الحصول على محاضر بالـ ID
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> GetLecturerById(int id)
        {
            var lecturer = await _lecturerService.GetLecturerById(id);
            if (lecturer == null)
            {
                return NotFound(new { message = "المحاضر غير موجود" });
            }
            return Ok(lecturer);
        }

        // POST: api/lecturers/Add
        // إضافة محاضر جديد
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLecturer([FromBody] LecturerDTO dto)
        {
            var lecturer = await _lecturerService.AddLecturer(dto);
            return Ok(new { message = "تمت إضافة المحاضر بنجاح", lecturer });
        }

        // PUT: api/lecturers/Update/5
        // تعديل محاضر
        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> UpdateLecturer(int id, [FromBody] LecturerDTO dto)
        {
            var lecturer = await _lecturerService.UpdateLecturer(id, dto);
            if (lecturer == null)
            {
                return NotFound(new { message = "المحاضر غير موجود" });
            }
            return Ok(new { message = "تم تعديل المحاضر بنجاح", lecturer });
        }

        // DELETE: api/lecturers/Delete/5
        // حذف محاضر
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteLecturer(int id)
        {
            var result = await _lecturerService.DeleteLecturer(id);
            if (!result)
            {
                return NotFound(new { message = "المحاضر غير موجود" });
            }
            return Ok(new { message = "تم حذف المحاضر بنجاح" });
        }
    }
}
