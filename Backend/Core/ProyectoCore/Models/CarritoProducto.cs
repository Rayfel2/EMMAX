using System;
using System.Collections.Generic;

namespace ProyectoCore.Models;

public partial class CarritoProducto
{
    public int IdCarrito { get; set; }

    public int IdProducto { get; set; }

    public int? Cantidad { get; set; }

    public double? Precio { get; set; }

    public virtual Carrito IdCarritoNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
