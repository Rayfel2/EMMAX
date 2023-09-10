using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoCore.Models.ViewModels
{
    public class CarritoVM
    {
        public Carrito oCarrito { get; set; }
        public List<SelectListItem> oListaUsuario { get; set; }
    }
}
