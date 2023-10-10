using ProyectoCore.Models;

namespace ProyectoCore.Interface
{
    public interface ICarritoRepository
    {
        ICollection<Carrito> GetCarrito();
        Carrito GetCarrito(int id);
        bool save();
        bool CreateCarrito(Carrito carrito);
    }
}
