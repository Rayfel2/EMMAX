using ProyectoCore.Models;
namespace ProyectoCore.Interface
{
    public interface IListaProductoRepository
    {
        ICollection<ListaProducto> GetListaProducto();
        ICollection<ListaProducto> GetListaProductos(int id);
        ListaProducto GetListasProductos(int IdListas, int IdProductos);
        bool ListaProductoExist(int idLista, int idProducto);
        bool CreateListaProducto(ListaProducto listaProducto);
        bool DeleteListaProducto(ListaProducto listaProducto);
        bool save();
    }
}
