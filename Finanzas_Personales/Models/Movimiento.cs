using System;
using System.Collections.Generic;

namespace Finanzas_Personales.Models;

public partial class Movimiento
{
    public int IdMovimiento { get; set; }

    public string? Tipo { get; set; }

    public decimal? Monto { get; set; }

    public int? Categoría { get; set; }

    public DateTime? Fecha { get; set; }

    public int Usuario { get; set; }
}
