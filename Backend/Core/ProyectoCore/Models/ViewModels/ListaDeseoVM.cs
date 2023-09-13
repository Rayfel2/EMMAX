using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoCore.Models.ViewModels
{
    public class ListaDeseoVM
    {
        public ListaDeseo oListaDeseo { get; set; }
        public List<SelectListItem> oListaUsuario { get; set; }

    }
}
