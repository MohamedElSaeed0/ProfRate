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
        private readonly AppDbContext _context;

        public LecturerSubjectController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/lecturersubjects/GetAll
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.LecturerSubjects
                .Include(ls => ls.Lecturer)
                .Include(ls => ls.Subject)
                .ToListAsync();
            return Ok(list);
        }

        // GET: api/lecturersubjects/GetByLecturer/5
        [HttpGet]
        [Route("GetByLecturer/{lecturerId}")]
        public async Task<IActionResult> GetByLecturer(int lecturerId)
        {
            var list = await _context.LecturerSubjects
                .Where(ls => ls.LecturerId == lecturerId)
                .Include(ls => ls.Subject)
                .ToListAsync();
            return Ok(list);
        }

        // GET: api/lecturersubjects/GetBySubject/5
        [HttpGet]
        [Route("GetBySubject/{subjectId}")]
        public async Task<IActionResult> GetBySubject(int subjectId)
        {
            var list = await _context.LecturerSubjects
                .Where(ls => ls.SubjectId == subjectId)
                .Include(ls => ls.Lecturer)
                .ToListAsync();
            return Ok(list);
        }

        // POST: api/lecturersubjects/Add
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] ProfRate.DTOs.LecturerSubjectDTO model)
        {
            var exists = await _context.LecturerSubjects
                .AnyAsync(ls => ls.LecturerId == model.LecturerId && ls.SubjectId == model.SubjectId);
            
            if (exists)
            {
                return BadRequest(new { message = "هذا المحاضر معين بالفعل لهذه المادة" });
            }

            var entry = new LecturerSubject
            {
                LecturerId = model.LecturerId,
                SubjectId = model.SubjectId
            };

            _context.LecturerSubjects.Add(entry);
            await _context.SaveChangesAsync();
            return Ok(new { message = "تم ربط المحاضر بالمادة بنجاح" });
        }

        // DELETE: api/lecturersubjects/Delete/5
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entry = await _context.LecturerSubjects.FindAsync(id);
            if (entry == null)
            {
                return NotFound(new { message = "غير موجود" });
            }

            _context.LecturerSubjects.Remove(entry);
            await _context.SaveChangesAsync();
            return Ok(new { message = "تم الحذف بنجاح" });
        }
    }
}
