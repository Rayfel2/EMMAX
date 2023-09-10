using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoCore.Models.ViewModels
{
    public class CarritoProductoVM
    {
        public CarritoProducto oCarritoProducto { get; set; }
        public List<SelectListItem> oListaCarrito { get; set; }
        public List<SelectListItem> oListaProducto { get; set; }


    }
}
