using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfRate.Data;
using ProfRate.Entities;

namespace ProfRate.Controllers
{
    [Route("api/studentsubjects")]
    [ApiController]
    public class StudentSubjectController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentSubjectController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/studentsubjects/GetAll
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.StudentSubjects
                .Include(ss => ss.Student)
                .Include(ss => ss.Subject)
                .Include(ss => ss.Lecturer)
                .ToListAsync();
            return Ok(list);
        }

        // GET: api/studentsubjects/GetByStudent/5
        [HttpGet]
        [Route("GetByStudent/{studentId}")]
        public async Task<IActionResult> GetByStudent(int studentId)
        {
            var list = await _context.StudentSubjects
                .Where(ss => ss.StudentId == studentId)
                .Include(ss => ss.Subject)
                .Include(ss => ss.Lecturer)
                .ToListAsync();
            return Ok(list);
        }

        // POST: api/studentsubjects/Add
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] ProfRate.DTOs.StudentSubjectDTO model)
        {
            // التحقق من عدم التكرار (نفس الطالب والمادة) - يمكن السماح بالتكرار إذا كان محاضر مختلف؟ 
            // المستخدم يريد: "سجلت مع واحد... لما تيجي تقيم هتقيم الي انت بتدرس معاه فقط"
            // إذن الطالب ممكن يسجل المادة الواحدة مرة واحدة فقط مع محاضر واحد؟ أو ممكن يسجل المادة مع محاضرين مختلفين؟
            // الأغلب الطالب بيسجل المادة مرة واحدة في الترم. بس لو افترضنا انه ممكن يعيدها أو ياخدها مع محاضر تاني.
            // للتسهيل وحسب طلب المستخدم: "الطالب يبقا له محاضر او محاضرين". 
            // سأجعل التحقق: الطالب + المادة + المحاضر.
            
            var exists = await _context.StudentSubjects
                .AnyAsync(ss => ss.StudentId == model.StudentId && ss.SubjectId == model.SubjectId && ss.LecturerId == model.LecturerId);
            
            if (exists)
            {
                return BadRequest(new { message = "هذا الطالب مسجل بالفعل في هذه المادة مع هذا المحاضر" });
            }

            var entry = new StudentSubject
            {
                StudentId = model.StudentId,
                SubjectId = model.SubjectId,
                LecturerId = model.LecturerId
            };

            _context.StudentSubjects.Add(entry);
            await _context.SaveChangesAsync();
            return Ok(new { message = "تم ربط الطالب بالمادة بنجاح" });
        }

        // DELETE: api/studentsubjects/Delete/5
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entry = await _context.StudentSubjects.FindAsync(id);
            if (entry == null)
            {
                return NotFound(new { message = "غير موجود" });
            }

            _context.StudentSubjects.Remove(entry);
            await _context.SaveChangesAsync();
            return Ok(new { message = "تم الحذف بنجاح" });
        }
    }
}
