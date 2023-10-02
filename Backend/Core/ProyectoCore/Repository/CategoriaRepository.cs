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

        public List<int> GetCategoriaIdsByPartialNames(List<string> partialNames)
        {
            // Utiliza LINQ para buscar los IDs de empleados cuyos nombres contienen alguna cadena parcial
            var categoriaIds = _context.Categoria
                 .AsEnumerable() // Esto carga los datos en memoria (soluciona un error de constains)
                .Where(categoria => partialNames.Any(partialName => categoria.Nombre.Contains(partialName)))
                .Select(categoria => categoria.IdCategoria)
                .ToList();

            return categoriaIds;

        }
    }
}
