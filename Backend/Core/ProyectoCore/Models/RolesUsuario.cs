using System;
using System.Collections.Generic;
namespace ProyectoCore.Models
{
    public class RolesUsuario
    {
        public int IdRoles { get; set; }

        public int IdUsuario { get; set; }

        public virtual Role oRole { get; set; } = null!;

        public virtual Usuario oUsuario { get; set; } = null!;
    }
}
