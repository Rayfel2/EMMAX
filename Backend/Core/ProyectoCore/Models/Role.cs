using System;
using System.Collections.Generic;

namespace ProyectoCore.Models;

public partial class Role
{
    public int IdRoles { get; set; }

    public string? Rol { get; set; }

    public virtual ICollection<Usuario> IdUsuarios { get; set; } = new List<Usuario>();
    public virtual ICollection<RolesUsuario> RolesUsuarios { get; set; } = new List<RolesUsuario>();
}
