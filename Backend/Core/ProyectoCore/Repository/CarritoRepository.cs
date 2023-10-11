using ProyectoCore.Models;
using ProyectoCore.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProyectoCore.Repository
{
    public class CarritoRepository : ICarritoRepository
    {
        private readonly TiendaPruebaContext _context;

        public CarritoRepository(TiendaPruebaContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateCarritoAsync(Carrito carrito)
        {
            _context.Add(carrito);
            return await SaveAsync();
        }

        public async Task<List<Carrito>> GetCarritoAsync()
        {
            return await _context.Carritos.OrderBy(H => H.IdCarrito).ToListAsync();
        }

        public async Task<Carrito> GetCarritoAsync(int id)
        {
            return await _context.Carritos.Where(e => e.IdUsuario == id).FirstOrDefaultAsync();
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
