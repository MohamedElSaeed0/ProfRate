using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfRate.DTOs;
using ProfRate.Services;

namespace ProfRate.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/students/GetAll
        [HttpGet]
        [Route("GetAll")]
        [Authorize] // يتطلب تسجيل الدخول
        public async Task<IActionResult> GetAllStudents([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 20;

            var result = await _studentService.GetAllStudents(page, pageSize);
            
            // تحويل النتائج لـ DTO بدون الباسورد
            var safeResult = new
            {
                items = result.Items.Select(s => new StudentResponseDTO
                {
                    StudentId = s.StudentId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Username = s.Username,
                    AdminId = s.AdminId
                }),
                result.TotalCount,
                result.CurrentPage,
                result.PageSize,
                result.TotalPages,
                result.HasPrevious,
                result.HasNext
            };
            return Ok(safeResult);
        }

        // GET: api/students/Search?query=...
        [HttpGet]
        [Route("Search")]
        [Authorize] // يتطلب تسجيل الدخول
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var students = await _studentService.Search(query);
            var safeStudents = students.Select(s => new StudentResponseDTO
            {
                StudentId = s.StudentId,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Username = s.Username,
                AdminId = s.AdminId
            });
            return Ok(safeStudents);
        }

        // GET: api/students/GetById/5
        // الحصول على طالب بالـ ID
        [HttpGet]
        [Route("GetById/{id}")]
        [Authorize(Roles = "Admin")] // الأدمن فقط يشوف بيانات طالب معين
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound(new { message = "الطالب غير موجود" });
            }
            // للأدمن نرجع كل البيانات عشان يقدر يعدل
            return Ok(student);
        }

        // POST: api/students/Add
        // إضافة طالب جديد
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = "Admin")] // الأدمن فقط يقدر يضيف
        public async Task<IActionResult> AddStudent([FromBody] StudentDTO dto)
        {
            try
            {
                var student = await _studentService.AddStudent(dto);
                return Ok(new { message = "تمت إضافة الطالب بنجاح", student });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "حدث خطأ أثناء إضافة الطالب" });
            }
        }

        // PUT: api/students/Update/5
        // تعديل طالب
        [HttpPut]
        [Route("Update/{id}")]
        [Authorize(Roles = "Admin")] // الأدمن فقط يقدر يعدل
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentDTO dto)
        {
            var student = await _studentService.UpdateStudent(id, dto);
            if (student == null)
            {
                return NotFound(new { message = "الطالب غير موجود" });
            }
            return Ok(new { message = "تم تعديل الطالب بنجاح", student });
        }

        // DELETE: api/students/Delete/5
        // حذف طالب
        [HttpDelete]
        [Route("Delete/{id}")]
        [Authorize(Roles = "Admin")] // الأدمن فقط يقدر يحذف
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await _studentService.DeleteStudent(id);
            if (!result)
            {
                return NotFound(new { message = "الطالب غير موجود" });
            }
            return Ok(new { message = "تم حذف الطالب بنجاح" });
        }
    }
}
