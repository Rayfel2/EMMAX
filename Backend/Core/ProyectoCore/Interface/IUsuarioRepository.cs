using ProyectoCore.Models;

namespace ProyectoCore.Interface
{
    public interface IUsuarioRepository
    {
        ICollection<Usuario> GetUsuarios();
        Usuario GetUsuario(int id);
        bool save();
    }
}
