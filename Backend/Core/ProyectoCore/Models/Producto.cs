using System;
using System.Collections.Generic;

namespace ProyectoCore.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public int? IdCategoria { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public double? Precio { get; set; }

    public int? Stock { get; set; }

    public string? Imagen { get; set; }

    public virtual ICollection<CarritoProducto> CarritoProductos { get; set; } = new List<CarritoProducto>();

    public virtual Categorium? oCategorium { get; set; }

    public virtual ICollection<Reseña> Reseñas { get; set; } = new List<Reseña>();

    public virtual ICollection<ListaDeseo> IdLista { get; set; } = new List<ListaDeseo>();
    public virtual ICollection<ListaProducto> IDListaProducto { get; set; } = new List<ListaProducto>();
}
