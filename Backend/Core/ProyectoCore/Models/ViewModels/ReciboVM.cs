using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoCore.Models.ViewModels
{
    public class ReciboVM
    {
        public Recibo oRecibo { get; set; }
        public List<SelectListItem> oListaCarrito { get; set; }
        public List<SelectListItem> oListaMetodoPago{ get; set; }


    }
}
