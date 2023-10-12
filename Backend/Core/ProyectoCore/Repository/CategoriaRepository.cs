using ProyectoCore.Interface;
using ProyectoCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoCore.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly TiendaPruebaContext _context;

        public CategoriaRepository(TiendaPruebaContext context)
        {
            _context = context;
        }

        public async Task<List<Categorium>> GetCategoriasAsync()
        {
            return await _context.Categoria.OrderBy(c => c.IdCategoria).ToListAsync();
        }

        public async Task<Categorium> GetCategoriaAsync(int id)
        {
            return await _context.Categoria.FirstOrDefaultAsync(e => e.IdCategoria == id);
        }

        public async Task<bool> SaveAsync()
        {
            try
            {
                var saved = await _context.SaveChangesAsync();
                return saved > 0;
            }
            catch (DbUpdateException ex)
            {
                // Manejar excepciones específicas aquí si es necesario
                throw new Exception("Error al guardar en la base de datos.", ex);
            }
        }

        public async Task<List<int>> GetCategoriaIdsByPartialNamesAsync(List<string> partialNames)
        {
            var categoriaIds = await _context.Categoria
                .Where(categoria => partialNames.Any(partialName => categoria.Nombre.Contains(partialName)))
                .Select(categoria => categoria.IdCategoria)
                .ToListAsync(); 

            return categoriaIds;
        }


    }
}
