using ProyectoCore.Interface;
using ProyectoCore.Models;

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
    }
}
