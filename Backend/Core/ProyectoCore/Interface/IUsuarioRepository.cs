using ProyectoCore.Models;

namespace ProyectoCore.Interface
{
    public interface IUsuarioRepository
    {
        ICollection<Usuario> GetUsuarios();
        Usuario GetUsuario(int id);
        bool save();
        Usuario GetUsuarioByEmailAndPassword(string email, string password);
        bool VerifyPasswordHash(string password, string storedHash);
        string HashPassword(string password);

        bool UsuarioExist(int idUsuario);
        bool CreateUsuario(Usuario Usuario);
        int GetUsuarioIds(string partialNames);
        Usuario GetUltimoUsuarioAgregado();
    }
}
