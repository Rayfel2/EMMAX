using ProyectoCore.Interface;
using ProyectoCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ProyectoCore.Repository
{
    public class ReseñaRepository : IReseñaRepository
    {
        private readonly TiendaPruebaContext _context;

        public ReseñaRepository(TiendaPruebaContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Reseña>> GetReseñasAsync()
        {
            return await _context.Reseñas.OrderBy(r => r.IdReseña).ToListAsync();
        }

        public async Task<ICollection<Reseña>> GetReseñasAsyncWithId(int productId)
        {
            return await _context.Reseñas
                .Where(r => r.IdProducto == productId)
                .OrderBy(r => r.IdReseña)
                .ToListAsync();
        }



        public async Task<Reseña> GetReseñasAsync(int id)
        {
            return await _context.Reseñas.Where(r => r.IdReseña == id).FirstOrDefaultAsync();
        }

        public async Task<bool> CreateReseñaAsync(Reseña reseña)
        {
            _context.Add(reseña);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            try
            {
                return (await _context.SaveChangesAsync()) > 0;
            }
            catch (Exception)
            {
                // Manejar errores aquí si es necesario
                return false;
            }
        }
    }
}
