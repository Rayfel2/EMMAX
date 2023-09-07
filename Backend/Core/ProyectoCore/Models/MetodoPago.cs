using System;
using System.Collections.Generic;

namespace ProyectoCore.Models;

public partial class MetodoPago
{
    public int IdMetodo { get; set; }

    public string? TipoMetodo { get; set; }

    public virtual ICollection<Recibo> Recibos { get; set; } = new List<Recibo>();
}
