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

        public List<int> GetProductoIdsByPartialNames(List<string> partialNames)
        {
            // Utiliza LINQ para buscar los IDs de empleados cuyos nombres contienen alguna cadena parcial
            var productoIds = _context.Productos
                 .AsEnumerable() // Esto carga los datos en memoria (soluciona un error de constains)
                .Where(producto => partialNames.Any(partialName => producto.Nombre.Contains(partialName)))
                .Select(producto => producto.IdProducto)
                .ToList();

            return productoIds;

        }

        public bool save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
