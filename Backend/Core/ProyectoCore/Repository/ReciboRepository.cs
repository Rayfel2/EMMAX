using Microsoft.EntityFrameworkCore;
using ProyectoCore.Interface;
using ProyectoCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoCore.Repository
{
    public class ReciboRepository : IReciboRepository
    {
        private readonly TiendaPruebaContext _context;

        public ReciboRepository(TiendaPruebaContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateReciboAsync(Recibo recibo)
        {
            _context.Add(recibo);
            return await SaveAsync();
        }

        public async Task<ICollection<Recibo>> GetReciboAsync()
        {
            return await _context.Recibos.OrderBy(r => r.IdRecibo).ToListAsync();
        }

        public async Task<Recibo> GetReciboAsync(int id)
        {
            return await _context.Recibos.FirstOrDefaultAsync(e => e.IdRecibo == id);
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
