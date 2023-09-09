using System;
using System.Collections.Generic;

namespace ProyectoCore.Models;

public partial class Reseña
{
    public int IdReseña { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdProducto { get; set; }

    public int? ValorReseña { get; set; }

    public string? Comentario { get; set; }

    public virtual Producto? oProducto { get; set; }

    public virtual Usuario? oUsuario { get; set; }
}
