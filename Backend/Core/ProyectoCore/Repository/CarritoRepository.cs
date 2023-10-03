using ProyectoCore.Interface;
using ProyectoCore.Models;

namespace ProyectoCore.Repository
{
    public class CarritoRepository : ICarritoRepository
    {
        private readonly TiendaPruebaContext _context;
        public CarritoRepository(TiendaPruebaContext context)
        {
            _context = context;
        }
        public ICollection<Carrito> GetCarrito()
        {
            return _context.Carritos.OrderBy(H => H.IdCarrito).ToList();
        }

        public Carrito GetCarrito(int id)
        {
            return _context.Carritos.Where(e => e.IdUsuario == id).FirstOrDefault();
        }

        public bool save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
