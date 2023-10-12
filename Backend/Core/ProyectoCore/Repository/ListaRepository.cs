using ProyectoCore.Interface;
using ProyectoCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoCore.Repository
{
    public class ListaRepository : IListaRepository
    {
        private readonly TiendaPruebaContext _context;

        public ListaRepository(TiendaPruebaContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateListaAsync(ListaDeseo lista)
        {
            await _context.AddAsync(lista);
            return await SaveAsync();
        }

        public async Task<ICollection<ListaDeseo>> GetListaAsync()
        {
            return await _context.ListaDeseos.OrderBy(H => H.IdLista).ToListAsync();
        }

        public async Task<ListaDeseo> GetListaAsync(int id)
        {
            return await _context.ListaDeseos.Where(e => e.IdUsuario == id).FirstOrDefaultAsync();
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }
    }
}
