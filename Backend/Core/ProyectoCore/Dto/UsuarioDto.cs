using ProyectoCore.Models;

namespace ProyectoCore.Dto
{
    public class UsuarioDto
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
    }

    public class UsuarioPostDto
    {
        public int IdUsuario { get;}
        public string? Nombre { get; set; }

        public string? Apellido { get; set; }

        public string? Dirección { get; set; }

        public string? Teléfono { get; set; }

        public string? Email { get; set; }

        public string? NombreUsuario { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public string? Contraseña { get; set; }

        public string? ContraseñaHash { get;}
    }
}
