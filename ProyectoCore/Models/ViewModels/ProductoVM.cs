using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoCore.Models.ViewModels
{
    public class ProductoVM
    {
        public Producto oProducto { get; set; }
        public List<SelectListItem> oListaCategoria { get; set; }
    }
}
