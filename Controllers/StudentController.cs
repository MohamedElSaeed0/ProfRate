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
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/students
        // الحصول على كل الطلاب
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentService.GetAllStudents();
            return Ok(students);
        }

        // GET: api/students/GetById/5
        // الحصول على طالب بالـ ID
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound(new { message = "الطالب غير موجود" });
            }
            return Ok(student);
        }

        // POST: api/students/Add
        // إضافة طالب جديد
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddStudent([FromBody] StudentDTO dto)
        {
            var student = await _studentService.AddStudent(dto);
            return Ok(new { message = "تمت إضافة الطالب بنجاح", student });
        }

        // PUT: api/students/Update/5
        // تعديل طالب
        [HttpPut]
        [Route("Update/{id}")]
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
