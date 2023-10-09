using ProyectoCore.Models;

namespace ProyectoCore.Interface
{
    public interface ICarritoProductoRepository
    {
        ICollection<CarritoProducto> GetCarritoProducto();
        ICollection<CarritoProducto> GetCarritoProducto(int id);
        bool CreateCarritoProducto(CarritoProducto carritoProducto);
        bool CarritoProductoExist(int idCarrito, int idProducto);
        bool DeleteCarritoProducto(CarritoProducto carritoProducto);
        CarritoProducto GetCarritosProductos(int IdCarritos, int IdProductos);
        
        bool UpdateCarritoProducto(CarritoProducto carritoProducto);
        bool save();
    }
}
