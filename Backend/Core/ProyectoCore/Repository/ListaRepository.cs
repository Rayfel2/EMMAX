using ProyectoCore.Interface;
using ProyectoCore.Models;

namespace ProyectoCore.Repository
{
    public class ListaRepository : IListaRepository
    {
        private readonly TiendaPruebaContext _context;
        public ListaRepository(TiendaPruebaContext context)
        {
            _context = context;
        }
        public ICollection<ListaDeseo> GetLista()
        {
            return _context.ListaDeseos.OrderBy(H => H.IdLista).ToList();
        }

        public ListaDeseo GetLista(int id)
        {
            return _context.ListaDeseos.Where(e => e.IdUsuario == id).FirstOrDefault();
        }

        public bool save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        ICollection<ListaProducto> IListaRepository.GetLista()
        {
            throw new NotImplementedException();
        }


    }
}
