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
        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var lista = await _context.Movimientos.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new { value = lista });
        }
    }
}
