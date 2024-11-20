using System;
using System.Collections.Generic;

namespace Finanzas_Personales.Models;

public partial class Usuario
{
    public int IdUsuarios { get; set; }

    public string Correo { get; set; } = null!;

    public string? Password { get; set; }

    public string? Nombre { get; set; }
}
