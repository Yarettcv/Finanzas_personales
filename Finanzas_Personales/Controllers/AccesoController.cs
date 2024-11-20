using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Finanzas_Personales.Custom;
using Finanzas_Personales.Models;
using Finanzas_Personales.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Finanzas_Personales.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly FinanzasContext _context;
        private readonly Utilidades _utilidades;

        public AccesoController(FinanzasContext finanzasContext, Utilidades utilidades)
        {
            _context = finanzasContext;
            _utilidades = utilidades;
        }
        [HttpPost]
        [Route("Registrarse")]
        public async Task<IActionResult> Registrarse(UsuarioDTO objeto)
        {
            var modeloUsuario = new Usuario
            {
                Nombre = objeto.Nombre,
                Correo = objeto.Correo,
                Password = _utilidades.encriptarSHA256(objeto.Clave)
            };

            await _context.Usuarios.AddAsync(modeloUsuario);
            await _context.SaveChangesAsync();

            if (modeloUsuario.IdUsuarios != 0)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO objeto)
        {
            var usuarioEncontrado = await _context.Usuarios.Where(u => u.Correo == objeto.Correo && u.Password == _utilidades.encriptarSHA256(objeto.Clave)).FirstOrDefaultAsync();

            if (usuarioEncontrado == null)
                return StatusCode(StatusCodes.Status200OK, new { isSucces = false, token = "" });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSucces = true, token = _utilidades.GenerarJWT(usuarioEncontrado) });
        }
    }
}
