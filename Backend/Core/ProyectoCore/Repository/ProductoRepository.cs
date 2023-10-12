using ProyectoCore.Interface;
using ProyectoCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoCore.Repository
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly TiendaPruebaContext _context;

        public ProductoRepository(TiendaPruebaContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateProductoAsync(Producto producto)
        {
            try
            {
                _context.Update(producto);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ICollection<Producto>> GetProductosAsync()
        {
            return await _context.Productos.OrderBy(p => p.IdProducto).ToListAsync();
        }

        public async Task<ICollection<Producto>> GetProductosDescendingAsync()
        {
            return await _context.Productos.OrderByDescending(p => p.IdProducto).ToListAsync();
        }

        public async Task<List<Producto>> GetProductoCategoriaAsync()
        {
            var productosConCategoria = await _context.Productos
                .Where(p => p.IdCategoria != null)
                .GroupBy(p => p.IdCategoria)
                .Select(group => group.First())
                .ToListAsync();

            return productosConCategoria;
        }

        public async Task<Producto> GetProductosAsync(int id)
        {
            return await _context.Productos.Where(p => p.IdProducto == id).FirstOrDefaultAsync();
        }
        public async Task<List<int>> GetProductoIdsByPartialNamesAsync(List<string> partialNames)
        {
            var productoIds = _context.Productos
                .AsEnumerable()
                .Where(producto => partialNames.Any(partialName => producto.Nombre.Contains(partialName)))
                .Select(producto => producto.IdProducto)
                .ToList();

            return productoIds;
        }

        public async Task<List<Producto>> GetProductosByProductAndCategoryIdsAsync(List<int> productIds, List<int> categoryIds)
        {
            var productos = _context.Productos
                .AsEnumerable()
                .Where(producto => productIds.Contains(producto.IdProducto) || categoryIds.Contains(Convert.ToInt32(producto.IdCategoria)))
                .ToList();

            return productos;
        }




    }
}
