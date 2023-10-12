using ProyectoCore.Interface;
using ProyectoCore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProyectoCore.Repository
{
    public class MetodoRepository : IMetodoRepository
    {
        private readonly TiendaPruebaContext _context;

        public MetodoRepository(TiendaPruebaContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<ICollection<MetodoPago>> GetMetodoAsync()
        {
            return await _context.MetodoPagos.OrderBy(H => H.IdMetodo).ToListAsync();
        }

        public async Task<MetodoPago> GetMetodoAsync(int id)
        {
            return await _context.MetodoPagos.Where(e => e.IdMetodo == id).FirstOrDefaultAsync();
        }
    }
}
