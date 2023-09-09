using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoCore.Models.ViewModels
{
    public class ResenaVM
    {

        public Reseña oResena { get; set; }
        public List<SelectListItem> oListaUsuario { get; set; }
        public List<SelectListItem> oListaProducto { get; set; }

    }
}
