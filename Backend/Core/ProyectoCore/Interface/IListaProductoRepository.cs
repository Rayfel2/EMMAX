using ProyectoCore.Models;
namespace ProyectoCore.Interface
{
    public interface IListaProductoRepository
    {
        ICollection<ListaProducto> GetListaProducto();
        ICollection<ListaProducto> GetListaProductos(int id);
        bool save();
    }
}
