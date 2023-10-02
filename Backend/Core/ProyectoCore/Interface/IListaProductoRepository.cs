using ProyectoCore.Models;
namespace ProyectoCore.Interface
{
    public interface IListaProductoRepository
    {
        ICollection<ListaProducto> GetListaProducto();
        ListaProducto GetListaProducto(int id);
        bool save();
    }
}
