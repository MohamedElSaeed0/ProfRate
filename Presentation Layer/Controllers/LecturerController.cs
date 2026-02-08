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
        private readonly ILecturerService _lecturerService;

        public LecturerController(ILecturerService lecturerService)
        {
            _lecturerService = lecturerService;
        }

        // GET: api/lecturers/GetAll
        // الحصول على كل المحاضرين
        [HttpGet]
        [Route("GetAll")]
        [Authorize] // يتطلب تسجيل الدخول
        public async Task<IActionResult> GetAllLecturers()
        {
            var lecturers = await _lecturerService.GetAllLecturers();
            // تحويل لـ DTO بدون الباسورد
            var safeLecturers = lecturers.Select(l => new LecturerResponseDTO
            {
                LecturerId = l.LecturerId,
                FirstName = l.FirstName,
                LastName = l.LastName,
                Username = l.Username,
                AdminId = l.AdminId,
                AdminRating = l.AdminRating,
                Subjects = l.LecturerSubjects.Select(ls => ls.Subject.SubjectName).ToList()
            });
            return Ok(safeLecturers);
        }

        // GET: api/lecturers/Search?query=...
        [HttpGet]
        [Route("Search")]
        [Authorize] // يتطلب تسجيل الدخول
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var lecturers = await _lecturerService.Search(query);
            var safeLecturers = lecturers.Select(l => new LecturerResponseDTO
            {
                LecturerId = l.LecturerId,
                FirstName = l.FirstName,
                LastName = l.LastName,
                Username = l.Username,
                AdminId = l.AdminId,
                AdminRating = l.AdminRating,
                Subjects = l.LecturerSubjects.Select(ls => ls.Subject.SubjectName).ToList()
            });
            return Ok(safeLecturers);
        }

        // GET: api/lecturers/GetById/5
        // الحصول على محاضر بالـ ID
        [HttpGet]
        [Route("GetById/{id}")]
        [Authorize(Roles = "Admin")] // الأدمن فقط
        public async Task<IActionResult> GetLecturerById(int id)
        {
            var lecturer = await _lecturerService.GetLecturerById(id);
            if (lecturer == null)
            {
                return NotFound(new { message = "المحاضر غير موجود" });
            }
            // للأدمن نرجع كل البيانات عشان يقدر يعدل
            return Ok(lecturer);
        }

        // POST: api/lecturers/Add
        // إضافة محاضر جديد
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = "Admin")] // الأدمن فقط يقدر يضيف
        public async Task<IActionResult> AddLecturer([FromBody] LecturerDTO dto)
        {
            try
            {
                var lecturer = await _lecturerService.AddLecturer(dto);
                return Ok(new { message = "تمت إضافة المحاضر بنجاح", lecturer });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء إضافة المحاضر" });
            }
        }

        // PUT: api/lecturers/Update/5
        // تعديل محاضر
        [HttpPut]
        [Route("Update/{id}")]
        [Authorize(Roles = "Admin")] // الأدمن فقط يقدر يعدل
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
        [Authorize(Roles = "Admin")] // الأدمن فقط يقدر يحذف
        public async Task<IActionResult> DeleteLecturer(int id)
        {
            var result = await _lecturerService.DeleteLecturer(id);
            if (!result)
            {
                return NotFound(new { message = "المحاضر غير موجود" });
            }
            return Ok(new { message = "تم حذف المحاضر بنجاح" });
        }

        // PUT: api/lecturers/UpdateRating/5
        // تحديث تقييم الأدمن للمحاضر
        [HttpPut]
        [Route("UpdateRating/{id}")]
        [Authorize(Roles = "Admin")] // الأدمن فقط
        public async Task<IActionResult> UpdateRating(int id, [FromBody] int rating)
        {
            try
            {
                var lecturer = await _lecturerService.UpdateAdminRating(id, rating);
                if (lecturer == null)
                {
                    return NotFound(new { message = "المحاضر غير موجود" });
                }
                return Ok(new { message = "تم تحديث التقييم بنجاح", rating = lecturer.AdminRating });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
