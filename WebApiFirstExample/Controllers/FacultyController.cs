using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiFirstExample.Data;
using WebApiFirstExample.Model;


namespace WebApiFirstExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacultyController : ControllerBase
    {
        //Dependency injection
        private readonly ApplicationDBContext _context;
        public FacultyController(ApplicationDBContext context)
        {
            _context = context;
        }
        // We use ActionResult because its most formal to recive the correct data structure
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Faculty>>> GetAll()
        {
            return await _context.Faculties.ToListAsync(); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Faculty>> GetById(int id)
        {
            var Faculty = await _context.Faculties.FindAsync(id);
            if (Faculty == null) return NotFound();
            return Faculty;
        }

        [HttpPost]
        public async Task<ActionResult<Faculty>> Create([FromBody] Faculty Faculty)
        {
            if (Faculty == null) return BadRequest();
            _context.Faculties.Add(Faculty);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = Faculty.Id }, Faculty);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Faculty Faculty)
        {
            if (id != Faculty.Id) return BadRequest();
            if (!await _context.Faculties.AnyAsync(s => s.Id == id)) return NotFound();

            _context.Entry(Faculty).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Faculties.Any(s => s.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var Faculty = await _context.Faculties.FindAsync(id);
            if (Faculty == null) return NotFound();

            _context.Faculties.Remove(Faculty);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
