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

            // Calcula el hash de la contraseña proporcionada
            using (var hmac = new HMACSHA512())
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Compara el hash calculado con el hash almacenado
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                    {
                        return false; // La contraseña no coincide
                    }
                }
            }

            return true; // La contraseña coincide
        }
    }
}
