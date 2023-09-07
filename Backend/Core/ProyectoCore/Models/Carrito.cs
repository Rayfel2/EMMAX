using System;
using System.Collections.Generic;

namespace ProyectoCore.Models;

public partial class Carrito
{
    public int IdCarrito { get; set; }

    public int? IdUsuario { get; set; }

    public virtual ICollection<CarritoProducto> CarritoProductos { get; set; } = new List<CarritoProducto>();

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<Recibo> Recibos { get; set; } = new List<Recibo>();
}
