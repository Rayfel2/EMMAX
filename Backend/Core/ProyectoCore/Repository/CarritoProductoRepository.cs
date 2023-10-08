using ProyectoCore.Models;
using ProyectoCore.Interface;

namespace ProyectoCore.Repository
{
    public class CarritoProductoRepository : ICarritoProductoRepository
    {

        private readonly TiendaPruebaContext _context;
        public CarritoProductoRepository(TiendaPruebaContext context)
        {
            _context = context;
        }

        public bool CarritoProductoExist(int idCarrito, int idProducto)
        {
            return _context.CarritoProductos.Any(cp => cp.IdCarrito == idCarrito && cp.IdProducto == idProducto);
        }

        public ICollection<CarritoProducto> GetCarritoProducto()
        {
            return _context.CarritoProductos.OrderBy(H => H.IdCarrito).ToList();
        }

        public ICollection<CarritoProducto> GetCarritoProducto(int id)
        {
            return _context.CarritoProductos.Where(e => e.IdCarrito == id).ToList();
        }

        public bool save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool CreateCarritoProducto(CarritoProducto carritoProducto)
        {
            _context.Add(carritoProducto);
            return save();
        }

        public bool DeleteCarritoProducto(CarritoProducto carritoProducto)
        {
            _context.Remove(carritoProducto);
            return save();
        }

        public CarritoProducto GetCarritosProductos(int IdCarritos, int IdProductos)
        {
            return _context.CarritoProductos
                .Where(e => e.IdCarrito == IdCarritos && e.IdProducto == IdProductos)
                .FirstOrDefault();
        }

    }
}
