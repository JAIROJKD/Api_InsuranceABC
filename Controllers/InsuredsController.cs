
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api_InsuranceABC.Data;
using Api_InsuranceABC.Models;

namespace Api_InsuranceABC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuredsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InsuredsController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost("CreateInsured")]
        public async Task<IActionResult> CreateInsured([FromBody] Insured insured)
        {
            // Validar que el número de documento no sea vacío o cero
            if (insured.IdNumber == 0)
            {
                return BadRequest(new { message = "El número de documento es obligatorio y no puede estar vacío." });
            }

            // Verificar si el asegurado ya existe en la BD
            var existingInsured = await _context.Insureds
                .FirstOrDefaultAsync(i => i.IdNumber == insured.IdNumber);

            if (existingInsured != null)
            {
                return Conflict(new { message = "Ya existe un asegurado con este número de documento." });
            }

            
            insured.CreateDate = DateTime.UtcNow;
            insured.UpDateDate = DateTime.UtcNow;

            _context.Insureds.Add(insured);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInsuredById), new { id = insured.IdNumber }, insured);
        }


        [HttpGet("GetInsured")]
        public async Task<IActionResult> GetInsured([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var insureds = await _context.Insureds
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return Ok(insureds);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInsuredById(long id)
        {
            var insured = await _context.Insureds.FindAsync(id);
            if (insured == null) return NotFound();
            return Ok(insured);
        }


        [HttpPut("UpdateInsured/{id}")]
        public async Task<IActionResult> UpdateInsured(long id, [FromBody] Insured insured)
        {
            // Validar que el IdNumber no esté vacío ni sea 0
            if (insured.IdNumber == 0)
            {
                return BadRequest(new { message = "El número de documento es obligatorio y no puede estar vacío." });
            }

            // Validar que el IdNumber solo contenga números (aunque es un long en C#, en JSON puede llegar mal formateado)
            if (!long.TryParse(insured.IdNumber.ToString(), out _))
            {
                return BadRequest(new { message = "El número de documento solo puede contener números." });
            }

            // Verificar si el asegurado a actualizar existe
            var existingInsured = await _context.Insureds.FindAsync(id);
            if (existingInsured == null)
            {
                return NotFound(new { message = "El asegurado no fue encontrado." });
            }

            // Si el ID enviado en el body es diferente del ID de la URL, devolver error
            if (id != insured.IdNumber)
            {
                return BadRequest(new { message = "El ID de la URL no coincide con el ID del asegurado." });
            }

            // Verificar si el nuevo IdNumber ya existe en otro asegurado
            var duplicateInsured = await _context.Insureds
                .FirstOrDefaultAsync(i => i.IdNumber == insured.IdNumber && i.IdNumber != id);

            if (duplicateInsured != null)
            {
                return Conflict(new { message = "Ya existe un asegurado con este número de documento." });
            }

            // Actualizar datos
            existingInsured.FirstName = insured.FirstName;
            existingInsured.MiddleName = insured.MiddleName;
            existingInsured.FirstLastName = insured.FirstLastName;
            existingInsured.MiddleLastName = insured.MiddleLastName;
            existingInsured.ContactPhone = insured.ContactPhone;
            existingInsured.Email = insured.Email;
            existingInsured.DateOfBirth = insured.DateOfBirth;
            existingInsured.EstimatedValue = insured.EstimatedValue;
            existingInsured.Observations = insured.Observations;
            existingInsured.UpDateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Asegurado actualizado correctamente." });
        }


        [HttpDelete("DeleteInsured/{id}")]
        public async Task<IActionResult> DeleteInsured(long id)
        {
            // Buscar el asegurado en la base de datos
            var insured = await _context.Insureds.FindAsync(id);

            // Si no existe, devolver mensaje de error
            if (insured == null)
            {
                return NotFound(new { message = "No se encontró un asegurado con el número de documento proporcionado." });
            }

            // Eliminar asegurado
            _context.Insureds.Remove(insured);
            await _context.SaveChangesAsync();

            // Devolver confirmación de eliminación
            return Ok(new { message = "Asegurado eliminado correctamente." });
        }


        [HttpGet("FilterInsured/{idNumber}")]
        public async Task<IActionResult> FilterInsured(long idNumber)
        {
            var insureds = await _context.Insureds
                .Where(i => i.IdNumber == idNumber)
                .ToListAsync();

            if (insureds == null || insureds.Count == 0)
            {
                return NotFound(new { message = "No se encontró un asegurado." });
            }

            return Ok(insureds);
        }
    }
}
