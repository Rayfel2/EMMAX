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
        public bool ListaProductoExist(int idLista, int idProducto)
        {
            return _context.ListaProducto.Any(cp => cp.IDListaProducto == idLista && cp.IdProducto == idProducto);
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
        public bool CreateListaProducto(ListaProducto listaProducto)
        {
            _context.Add(listaProducto);
            return save();
        }
        public bool DeleteListaProducto(ListaProducto listaProducto)
        {
            _context.Remove(listaProducto);
            return save();
        }

        public ListaProducto GetListasProductos(int IdListas, int IdProductos)
        {
            return _context.ListaProducto
                .Where(e => e.IDListaProducto == IdListas && e.IdProducto == IdProductos)
                .FirstOrDefault();
        }
    }
}
