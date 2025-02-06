using Microsoft.AspNetCore.Mvc;
using WebApiFirstExample.Data;
using WebApiFirstExample.Model;
using Microsoft.EntityFrameworkCore;

namespace WebApiFirstExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //Its importan colocate ControllerBase
    public class StudentsController : ControllerBase
    {
        //Dependency injection
        private readonly ApplicationDBContext _context;
        public StudentsController(ApplicationDBContext context)
        {
            _context = context;
        }
        // We use ActionResult because its most formal to recive the correct data structure
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetAll()
        {
            return await _context.Students.ToListAsync();  // Devuelve directamente la lista sin usar Ok()
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetById(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();
            return student;  // Devuelve el estudiante directamente
        }

        [HttpPost]
        public async Task<ActionResult<Student>> Create([FromBody] Student student)
        {
            if (student == null) return BadRequest();
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Student student)
        {
            if (id != student.Id) return BadRequest();
            if (!await _context.Students.AnyAsync(s => s.Id == id)) return NotFound();

            _context.Entry(student).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!_context.Students.Any(s => s.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();  // Devuelve 204 No Content si la actualización es exitosa
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();  // Devuelve 204 No Content si la eliminación es exitosa
        }

    }
}
