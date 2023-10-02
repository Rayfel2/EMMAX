using ProyectoCore.Models;
namespace ProyectoCore.Dto
{
    public class ReseñaDto
    {
   

        public int IdReseña { get; set; } // la      ñ en variables da probleams en angular

        //public int? IdUsuario { get; set; }
        public string? Usuario { get; set; }

        public int? IdProducto { get; set; }

        public int? ValorReseña { get; set; }

        public string? Comentario { get; set; }
       
    }
}
