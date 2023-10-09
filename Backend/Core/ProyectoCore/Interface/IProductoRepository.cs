using ProyectoCore.Models;

namespace ProyectoCore.Interface
{
    public interface IProductoRepository
    {
        ICollection<Producto> GetProductos();
  
        Producto GetProductos(int id);
        bool save();
        List<int> GetProductoIdsByPartialNames(List<string> partialNames);

        ICollection<Producto> GetProductosDescending();
        bool UpdateProducto(Producto Producto);
        List<Producto> GetProductoCategoria();


    }
}
