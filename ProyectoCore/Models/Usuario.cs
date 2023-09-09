using System;
using System.Collections.Generic;

namespace ProyectoCore.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Dirección { get; set; }

    public string? Teléfono { get; set; }

    public string? Email { get; set; }

    public string? NombreUsuario { get; set; }

    public DateTime? FechaNacimiento { get; set; }

    public string? Contraseña { get; set; }

    public string? ContraseñaHash { get; set; }

    public virtual ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();

    public virtual ICollection<ListaDeseo> ListaDeseos { get; set; } = new List<ListaDeseo>();

    public virtual ICollection<Reseña> Reseñas { get; set; } = new List<Reseña>();

    public virtual ICollection<Role> IdRoles { get; set; } = new List<Role>();
}
