using ProyectoCore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoCore.Interface
{
    public interface IListaProductoRepository
    {
        Task<ICollection<ListaProducto>> GetListaProductoAsync();
        Task<ICollection<ListaProducto>> GetListaProductosAsync(int id);
        Task<ListaProducto> GetListasProductosAsync(int IdListas, int IdProductos);
        Task<bool> ListaProductoExistAsync(int idLista, int idProducto);
        Task<bool> CreateListaProductoAsync(ListaProducto listaProducto);
        Task<bool> DeleteListaProductoAsync(ListaProducto listaProducto);
        Task<bool> SaveAsync();
    }
}
