using ProyectoCore.Models;
using ProyectoCore.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoCore.Repository
{
    public class ListaProductoRepository : IListaProductoRepository
    {
        private readonly TiendaPruebaContext _context;

        public ListaProductoRepository(TiendaPruebaContext context)
        {
            _context = context;
        }

        public async Task<ICollection<ListaProducto>> GetListaProductoAsync()
        {
            return await _context.ListaProducto.OrderBy(H => H.IDListaProducto).ToListAsync();
        }

        public async Task<bool> ListaProductoExistAsync(int idLista, int idProducto)
        {
            return await _context.ListaProducto.AnyAsync(cp => cp.IDListaProducto == idLista && cp.IdProducto == idProducto);
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<ICollection<ListaProducto>> GetListaProductosAsync(int id)
        {
            return await _context.ListaProducto.Where(e => e.IDListaProducto == id).ToListAsync();
        }

        public async Task<bool> CreateListaProductoAsync(ListaProducto listaProducto)
        {
            _context.Add(listaProducto);
            return await SaveAsync();
        }

        public async Task<bool> DeleteListaProductoAsync(ListaProducto listaProducto)
        {
            _context.Remove(listaProducto);
            return await SaveAsync();
        }

        public async Task<ListaProducto> GetListasProductosAsync(int IdListas, int IdProductos)
        {
            return await _context.ListaProducto
                .Where(e => e.IDListaProducto == IdListas && e.IdProducto == IdProductos)
                .FirstOrDefaultAsync();
        }
    }
}

