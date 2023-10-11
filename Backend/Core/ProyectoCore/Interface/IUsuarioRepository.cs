using ProyectoCore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoCore.Interface
{
    public interface IUsuarioRepository
    {
        Task<ICollection<Usuario>> GetUsuariosAsync();
        Task<Usuario> GetUsuarioAsync(int id);
        Task<bool> SaveAsync();
        Task<Usuario> GetUsuarioByEmailAndPasswordAsync(string email, string password);
        Task<bool> VerifyPasswordHashAsync(string password, string storedHash);
        Task<string> HashPasswordAsync(string password);

        Task<bool> UsuarioExistAsync(int idUsuario);
        Task<bool> CreateUsuarioAsync(Usuario Usuario);
        Task<int> GetUsuarioIdsAsync(string partialNames);
        Task<Usuario> GetUltimoUsuarioAgregadoAsync();
    }
}

