using Finanzas_Personales.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Finanzas_Personales.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly FinanzasContext _context;
        public CategoriasController(FinanzasContext context)
        {
            _context = context;
        }
        [HttpPost]
        [Route("CreateCategorias")]
        public async Task<IActionResult> CreateCategorias(Categoría objeto)
        {
            
            // Validar si ya existe una categoría con las mismas características
            var existeCategoria = await _context.Categorías.AnyAsync(m =>
                m.Nombre == objeto.Nombre);

            if (existeCategoria)
            {
                return StatusCode(StatusCodes.Status409Conflict, new
                {
                    isSuccess = false,
                    message = "La categoría ya existe en la base de datos."
                });
            }

            // Si no existe, se procede a agregar el movimiento
            var modelCategoria = new Categoría
            {
                Nombre = objeto.Nombre
            };

            await _context.Categorías.AddAsync(modelCategoria);
            await _context.SaveChangesAsync();

            if (modelCategoria.IdCategoria != 0)
            {
                return StatusCode(StatusCodes.Status200OK, new
                {
                    isSuccess = true,
                    message = "Categoría creada exitosamente."
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    isSuccess = false,
                    message = "Ocurrió un error al intentar guardar la categoría."
                });
            }
        }
        [HttpGet]
        [Route("ReadCategorias")]
        public async Task<IActionResult> ReadCategorias()
        {
            var lista = await _context.Categorías.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new { value = lista });
        }
        [HttpPut]
        [Route("UpdateCategoria/{id}")]
        public async Task<IActionResult> UpdateCategoria(int id, Categoría objeto)
        {
            var movimiento = await _context.Categorías.FindAsync(id);
            if (movimiento == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new
                {
                    isSuccess = false,
                    message = "Categoría no encontrada."
                });
            }

            // Actualizar los campos
            movimiento.Nombre = objeto.Nombre;
           
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new
            {
                isSuccess = true,
                message = "Categoría actualizado correctamente."
            });
        }
        [HttpDelete]
        [Route("DeleteCategoria/{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.Categorías.FindAsync(id);
            if (categoria == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new
                {
                    isSuccess = false,
                    message = "Categoría no encontrado."
                });
            }

            _context.Categorías.Remove(categoria);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new
            {
                isSuccess = true,
                message = "Categoría eliminada correctamente."
            });
        }
    }
}
