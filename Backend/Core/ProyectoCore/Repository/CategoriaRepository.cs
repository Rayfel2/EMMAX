using ProyectoCore.Interface;
using ProyectoCore.Models;


namespace ProyectoCore.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly TiendaPruebaContext _context;
        public CategoriaRepository(TiendaPruebaContext context)
        {
            _context = context;
        }
        public ICollection<Categorium> GetCategorias()
        {
            return _context.Categoria.OrderBy(H => H.IdCategoria).ToList();
        }

        public Categorium GetCategorias(int id)
        {
            return _context.Categoria.Where(e => e.IdCategoria == id).FirstOrDefault();
        }

        public bool save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
