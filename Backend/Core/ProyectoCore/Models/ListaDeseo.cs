using System;
using System.Collections.Generic;

namespace ProyectoCore.Models;

public partial class ListaDeseo
{
    public int IdLista { get; set; }

    public int? IdUsuario { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<Producto> IdProductos { get; set; } = new List<Producto>();
}
