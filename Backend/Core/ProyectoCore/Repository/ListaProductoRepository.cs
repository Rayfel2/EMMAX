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



        public bool save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public ICollection<ListaProducto> GetListaProductos(int id) // esto se agrego solo
        {
            
            return _context.ListaProducto.Where(e => e.IDListaProducto == id).ToList();
        }
    }
}
