using ProyectoCore.Models;

namespace ProyectoCore.Dto
{
    public class ReciboDto
    {
            public int IdRecibo { get; set; }

            public int? IdMetodoPago { get; set; }

            public int? IdCarrito { get; set; }

            public double? Subtotal { get; set; }

            public double? Impuestos { get; set; }

            public string? Campo { get; set; }   
    }
    public class ReciboPostDto
    {

        public int? IdMetodoPago { get; set; }

        public int? IdCarrito { get; set; }

        public double? Subtotal { get; set; }

        public double? Impuestos { get; set; }

        public string? Campo { get; set; }
    }
}
