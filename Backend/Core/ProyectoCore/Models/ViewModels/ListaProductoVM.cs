using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoCore.Models.ViewModels
{
    public class ListaProductoVM
    {
        public ListaProducto oListaProducto { get; set; }
        public List<SelectListItem> oListaDeListas { get; set; }
        public List<SelectListItem> oListaProductoProductos { get; set; }
    }
}
