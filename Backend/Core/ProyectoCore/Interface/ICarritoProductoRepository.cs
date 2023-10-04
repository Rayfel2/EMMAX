using ProyectoCore.Models;

namespace ProyectoCore.Interface
{
    public interface ICarritoProductoRepository
    {
        ICollection<CarritoProducto> GetCarritoProducto();
        ICollection<CarritoProducto> GetCarritoProducto(int id);
        bool save();
        bool CreateCarritoProducto(CarritoProducto CarritoProducto); //post
    }
}
