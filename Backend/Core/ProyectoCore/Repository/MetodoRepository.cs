using ProyectoCore.Interface;
using ProyectoCore.Models;


namespace ProyectoCore.Repository
{
    public class MetodoRepository : IMetodoRepository
    {
        private readonly TiendaPruebaContext _context;
        public MetodoRepository(TiendaPruebaContext context)
        {
            _context = context;
        }


        public bool save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }


        public ICollection<MetodoPago> GetMetodo()
        {
            return _context.MetodoPagos.OrderBy(H => H.IdMetodo).ToList();
        }

        public MetodoPago GetMetodo(int id)
        {
            return _context.MetodoPagos.Where(e => e.IdMetodo == id).FirstOrDefault();
        }
    }
}
