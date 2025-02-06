using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiFirstExample.Data;
using WebApiFirstExample.Model;

namespace WebApiFirstExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController :ControllerBase
    {
        //Dependency injection
        private readonly ApplicationDBContext _context;
        public DepartmentController(ApplicationDBContext context)
        {
            _context = context;
        }
        // We use ActionResult because its most formal to recive the correct data structure
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetAll()
        {
            return await _context.Departments.ToListAsync();  // Devuelve directamente la lista sin usar Ok()
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetById(int id)
        {
            var Department = await _context.Departments.FindAsync(id);
            if (Department == null) return NotFound();
            return Department;  // Devuelve el estudiante directamente
        }

        [HttpPost]
        public async Task<ActionResult<Department>> Create([FromBody] Department Department)
        {
            if (Department == null) return BadRequest();
            _context.Departments.Add(Department);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = Department.Id }, Department);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Department Department)
        {
            if (id != Department.Id) return BadRequest();
            if (!await _context.Departments.AnyAsync(s => s.Id == id)) return NotFound();

            _context.Entry(Department).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!_context.Departments.Any(s => s.Id == id))
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
            var Department = await _context.Departments.FindAsync(id);
            if (Department == null) return NotFound();

            _context.Departments.Remove(Department);
            await _context.SaveChangesAsync();
            return NoContent();  // Devuelve 204 No Content si la eliminación es exitosa
        }
    }
}
