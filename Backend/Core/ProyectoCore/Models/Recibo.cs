using System;
using System.Collections.Generic;

namespace ProyectoCore.Models;

public partial class Recibo
{
    public int IdRecibo { get; set; }

    public int? IdMetodoPago { get; set; }

    public int? IdCarrito { get; set; }

    public double? Subtotal { get; set; }

    public double? Impuestos { get; set; }

    public string? Campo { get; set; }

    public virtual Carrito? IdCarritoNavigation { get; set; }

    public virtual MetodoPago? IdMetodoPagoNavigation { get; set; }
}
