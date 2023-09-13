using System;
using System.Collections.Generic;

namespace ProyectoCore.Models;

public partial class CarritoProducto
{
    public int IdCarrito { get; set; }

    public int IdProducto { get; set; }

    public int? Cantidad { get; set; }

    public double? Precio { get; set; }

    public virtual Carrito oCarrito { get; set; } = null!;
<<<<<<< HEAD:ProyectoCore/Models/CarritoProducto.cs

    public virtual Producto oProducto { get; set; } = null!;
=======

    public virtual Producto oProducto { get; set; } = null!;

>>>>>>> 41d7c60235fdb8ff0cabafcdbdd1b70dd2eabb63:Backend/Core/ProyectoCore/Models/CarritoProducto.cs
}
