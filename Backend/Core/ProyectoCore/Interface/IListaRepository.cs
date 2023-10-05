using ProyectoCore.Models;

namespace ProyectoCore.Interface
{
    public interface IListaRepository
    {
        ICollection<ListaProducto> GetLista();
        ListaDeseo GetLista(int id);
        bool save();
    }
}
