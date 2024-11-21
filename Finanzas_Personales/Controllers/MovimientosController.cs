using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Finanzas_Personales.Custom;
using Finanzas_Personales.Models;
using Finanzas_Personales.Models.DTOs;

namespace Finanzas_Personales.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MovimientosController : ControllerBase
    {
        private readonly FinanzasContext _context;
        public MovimientosController(FinanzasContext context)
        {
            _context = context;
        }
        [HttpPost]
        [Route("CreateMovimientos")]
        public async Task<IActionResult> CreateMovimientos(Movimiento objeto)
        {
            var userId = User.FindFirst("IdUsuarios")?.Value;
            // Validar si ya existe un movimiento con las mismas características
            var existeMovimiento = await _context.Movimientos.AnyAsync(m =>
                m.Tipo == objeto.Tipo &&
                m.Monto == objeto.Monto &&
                m.Categoría == objeto.Categoría &&
                m.Fecha == objeto.Fecha &&
                m.Usuario == objeto.Usuario);

            if (existeMovimiento)
            {
                return StatusCode(StatusCodes.Status409Conflict, new
                {
                    isSuccess = false,
                    message = "El movimiento ya existe en la base de datos."
                });
            }

            // Si no existe, se procede a agregar el movimiento
            var modelMovimiento = new Movimiento
            {
                Tipo = objeto.Tipo,
                Monto = objeto.Monto,
                Categoría = objeto.Categoría,
                Fecha = objeto.Fecha,
                Usuario = Convert.ToInt32(userId)
            };

            await _context.Movimientos.AddAsync(modelMovimiento);
            await _context.SaveChangesAsync();

            if (modelMovimiento.IdMovimiento != 0)
            {
                return StatusCode(StatusCodes.Status200OK, new
                {
                    isSuccess = true,
                    message = "Movimiento creado exitosamente."
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    isSuccess = false,
                    message = "Ocurrió un error al intentar guardar el movimiento."
                });
            }
        }
        [HttpGet]
        [Route("ReadMovimiento")]
        public async Task<IActionResult> ReadMovimiento()
        {
            var lista = await _context.Movimientos.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new { value = lista });
        }
        [HttpPut]
        [Route("UpdateMovimiento/{id}")]
        public async Task<IActionResult> UpdateMovimiento(int id, Movimiento objeto)
        {
            var movimiento = await _context.Movimientos.FindAsync(id);
            if (movimiento == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new
                {
                    isSuccess = false,
                    message = "Movimiento no encontrado."
                });
            }

            // Actualizar los campos
            movimiento.Tipo = objeto.Tipo;
            movimiento.Monto = objeto.Monto;
            movimiento.Categoría = objeto.Categoría;
            movimiento.Fecha = objeto.Fecha;

            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new
            {
                isSuccess = true,
                message = "Movimiento actualizado correctamente."
            });
        }
        [HttpDelete]
        [Route("DeleteMovimiento/{id}")]
        public async Task<IActionResult> DeleteMovimiento(int id)
        {
            var movimiento = await _context.Movimientos.FindAsync(id);
            if (movimiento == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new
                {
                    isSuccess = false,
                    message = "Movimiento no encontrado."
                });
            }

            _context.Movimientos.Remove(movimiento);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new
            {
                isSuccess = true,
                message = "Movimiento eliminado correctamente."
            });
        }
    }
}
