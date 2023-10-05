using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using ProyectoCore.Interface;
using ProyectoCore.Models;
using System.Security.Cryptography;
using System.Text;

namespace ProyectoCore.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly TiendaPruebaContext _context;
        public UsuarioRepository(TiendaPruebaContext context)
        {
            _context = context;
        }

        public bool CreateUsuario(Usuario Usuario)
        {
            _context.Add(Usuario);
            return save();
        }

        public bool UsuarioExist(int idUsuario)
        {
            return _context.Usuarios.Any(p => p.IdUsuario == idUsuario);

        }
        public ICollection<Usuario> GetUsuarios()
        {
            return _context.Usuarios.OrderBy(H => H.IdUsuario).ToList();
        }

        public Usuario GetUsuario(int id)
        {
            return _context.Usuarios.Where(e => e.IdUsuario == id).FirstOrDefault();
        }

        public bool save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        // para el hash

        public Usuario GetUsuarioByEmailAndPassword(string email, string password)
        {
            // Buscar un usuario con el correo electrónico proporcionado
            var usuario = _context.Usuarios.SingleOrDefault(u => u.Email == email);

            // Verificar si se encontró un usuario con ese correo electrónico
            if (usuario == null)
            {
                return null; // Usuario no encontrado
            }

            // Verificar la contraseña
            if (!VerifyPasswordHash(password, usuario.ContraseñaHash))
            {
                return null; // Contraseña incorrecta
            }

            return usuario; // Usuario encontrado y contraseña correcta
        }


        // Método para verificar si la contraseña coincide
        public bool VerifyPasswordHash(string password, string storedHash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(storedHash))
            {
                return false; // Contraseña o hash almacenados son inválidos
            }

            using (var sha256 = SHA256.Create())
            {
                // Calcula el hash de la contraseña proporcionada
                var computedHashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var computedHashString = BitConverter.ToString(computedHashBytes).Replace("-", "").ToLower();

                // Compara el hash calculado con el hash almacenado
                return string.Equals(computedHashString, storedHash, StringComparison.OrdinalIgnoreCase);
            }
        }

        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("La contraseña no puede estar vacía o ser nula.", nameof(password));
            }

            using (var sha256 = SHA256.Create())
            {
                // Calcula el hash de la contraseña como una cadena hexadecimal
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return hashString;
            }
        }
        public int GetUsuarioIds(string partialNames)
        {
            var UsuarioId = _context.Usuarios
    .Where(Usuario => Usuario.NombreUsuario == partialNames)
    .Select(Usuario => Usuario.IdUsuario)
    .FirstOrDefault();


            return UsuarioId;

        }
    }
}
