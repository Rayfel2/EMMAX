using ProyectoCore.Models;

namespace ProyectoCore.Interface
{
    public interface IListaRepository
    {
        ICollection<ListaProducto> GetLista();
        Carrito GetLista(int id);
        bool save();
    }
}
