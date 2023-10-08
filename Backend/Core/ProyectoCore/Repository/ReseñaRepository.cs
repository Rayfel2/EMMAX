using ProyectoCore.Interface;
using ProyectoCore.Models;


namespace ProyectoCore.Repository
{
    public class ReseñaRepository : IReseñaRepository
    {
        private readonly TiendaPruebaContext _context;
        public ReseñaRepository(TiendaPruebaContext context)
        {
            _context = context;
        }
        public ICollection<Reseña> GetReseñas()
        {
            return _context.Reseñas.OrderBy(H => H.IdReseña).ToList();
        }

        public Reseña GetReseñas(int id)
        {
            return _context.Reseñas.Where(e => e.IdReseña == id).FirstOrDefault();
        }

        public bool save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool CreateReseña (Reseña reseña)
        {
            _context.Add(reseña);
            return save();
        }
    }
}
