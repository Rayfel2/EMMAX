using ProyectoCore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoCore.Interface
{
    public interface ICategoriaRepository
    {
        Task<List<Categorium>> GetCategoriasAsync();
        Task<Categorium> GetCategoriaAsync(int id);
        Task<bool> SaveAsync();
        Task<List<int>> GetCategoriaIdsByPartialNamesAsync(List<string> partialNames);
    }
}
