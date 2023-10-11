using Microsoft.EntityFrameworkCore;
using ProyectoCore.Interface;
using ProyectoCore.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCore.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly TiendaPruebaContext _context;

        public UsuarioRepository(TiendaPruebaContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateUsuarioAsync(Usuario Usuario)
        {
            _context.Add(Usuario);
            return await SaveAsync();
        }

        public async Task<bool> UsuarioExistAsync(int idUsuario)
        {
            return await _context.Usuarios.AnyAsync(p => p.IdUsuario == idUsuario);
        }

        public async Task<ICollection<Usuario>> GetUsuariosAsync()
        {
            return await _context.Usuarios.OrderBy(H => H.IdUsuario).ToListAsync();
        }

        public async Task<Usuario> GetUsuarioAsync(int id)
        {
            return await _context.Usuarios.Where(e => e.IdUsuario == id).FirstOrDefaultAsync();
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<Usuario> GetUsuarioByEmailAndPasswordAsync(string email, string password)
        {
            var usuario = await _context.Usuarios.SingleOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
            {
                return null;
            }

            if (!await VerifyPasswordHashAsync(password, usuario.ContraseñaHash))
            {
                return null;
            }

            return usuario;
        }

        public async Task<int> GetUsuarioIdsAsync(string partialNames)
        {
            var UsuarioId = await _context.Usuarios
                .Where(Usuario => Usuario.Email == partialNames)
                .Select(Usuario => Usuario.IdUsuario)
                .FirstOrDefaultAsync();

            return UsuarioId;
        }

        public async Task<Usuario> GetUltimoUsuarioAgregadoAsync()
        {
            return await _context.Usuarios.OrderByDescending(u => u.IdUsuario).FirstOrDefaultAsync();
        }

        public async Task<string> HashPasswordAsync(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("La contraseña no puede estar vacía o ser nula.", nameof(password));
            }

            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return hashString;
            }
        }

        public async Task<bool> VerifyPasswordHashAsync(string password, string storedHash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(storedHash))
            {
                return false;
            }

            using (var sha256 = SHA256.Create())
            {
                var computedHashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var computedHashString = BitConverter.ToString(computedHashBytes).Replace("-", "").ToLower();

                return string.Equals(computedHashString, storedHash, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
