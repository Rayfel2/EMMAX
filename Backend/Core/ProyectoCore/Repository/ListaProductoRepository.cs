using ProyectoCore.Models;
using ProyectoCore.Interface;
using Microsoft.EntityFrameworkCore;

namespace ProyectoCore.Repository
{
    public class ListaProductoRepository : IListaProductoRepository
    {
        private readonly TiendaPruebaContext _context;
        public ListaProductoRepository(TiendaPruebaContext context)
        {
            _context = context;
        }
        public ICollection<ListaProducto> GetListaProducto()
        {
            return _context.ListaProducto.OrderBy(H => H.IDListaProducto).ToList();
        }

        public ListaProducto GetListaProducto(int id)
        {
            return _context.ListaProducto.Where(e => e.IDListaProducto == id).FirstOrDefault();
        }

        public bool save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        ListaProducto IListaProductoRepository.GetListaProducto(int id) // esto se agrego solo
        {
            throw new NotImplementedException();
        }
    }
}
