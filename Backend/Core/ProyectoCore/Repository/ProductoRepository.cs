using ProyectoCore.Interface;
using ProyectoCore.Models;


namespace ProyectoCore.Repository
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly TiendaPruebaContext _context;
        public ProductoRepository(TiendaPruebaContext context)
        {
            _context = context;
        }
        public ICollection<Producto> GetProductos()
        {
            return _context.Productos.OrderBy(H => H.IdProducto).ToList();
        }

        public Producto GetProductos(int id)
        {
            return _context.Productos.Where(e => e.IdProducto == id).FirstOrDefault();
        }

        public bool save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
