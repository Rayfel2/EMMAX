using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoCore.Models.ViewModels
{
    public class RolesUsuarioVM
    {
        public RolesUsuario oRolesUsuario { get; set; }
        public List<SelectListItem> oListaRoles { get; set; }
        public List<SelectListItem> oListaUsuarios { get; set; }

    }
}
