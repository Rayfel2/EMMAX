using ProyectoCore.Interface;
using ProyectoCore.Models;


namespace ProyectoCore.Repository
{
    public class ReciboRepository : IReciboRepository
    {
        private readonly TiendaPruebaContext _context;
        public ReciboRepository(TiendaPruebaContext context)
        {
            _context = context;
        }


        public bool save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool CreateRecibo (Recibo Recibo)
        {
            _context.Add(Recibo);
            return save();
        }

        public ICollection<Recibo> GetRecibo()
        {
            return _context.Recibos.OrderBy(H => H.IdRecibo).ToList();
        }

        public Recibo GetRecibo(int id)
        {
            return _context.Recibos.Where(e => e.IdRecibo == id).FirstOrDefault();
        }
    }
}
