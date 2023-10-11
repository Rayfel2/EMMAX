using ProyectoCore.Models;
using ProyectoCore.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoCore.Repository
{
    public class CarritoProductoRepository : ICarritoProductoRepository
    {
        private readonly TiendaPruebaContext _context;

        public CarritoProductoRepository(TiendaPruebaContext context)
        {
            _context = context;
        }

        public async Task<bool> CarritoProductoExistAsync(int idCarrito, int idProducto)
        {
            return await _context.CarritoProductos.AnyAsync(cp => cp.IdCarrito == idCarrito && cp.IdProducto == idProducto);
        }

        public async Task<List<CarritoProducto>> GetCarritoProductoAsync()
        {
            return await _context.CarritoProductos.OrderBy(H => H.IdCarrito).ToListAsync();
        }

        public async Task<List<CarritoProducto>> GetCarritoProductoAsync(int id)
        {
            return await _context.CarritoProductos.Where(e => e.IdCarrito == id).ToListAsync();
        }

        public async Task<bool> CreateCarritoProductoAsync(CarritoProducto carritoProducto)
        {
            await _context.AddAsync(carritoProducto);
            return await saveAsync();
        }

        public async Task<bool> DeleteCarritoProductoAsync(CarritoProducto carritoProducto)
        {
            _context.Remove(carritoProducto);
            return await saveAsync();
        }

        public async Task<bool> UpdateCarritoProductoAsync(CarritoProducto carritoProducto)
        {
            _context.Update(carritoProducto);
            return await saveAsync();
        }

        public async Task<CarritoProducto> GetCarritosProductosAsync(int IdCarritos, int IdProductos)
        {
            return await _context.CarritoProductos
                .Where(e => e.IdCarrito == IdCarritos && e.IdProducto == IdProductos)
                .FirstOrDefaultAsync();
        }

        private async Task<bool> saveAsync()
        {
            try
            {
                return (await _context.SaveChangesAsync()) > 0;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Manejar excepciones si es necesario
                return false;
            }
        }
    }
}
