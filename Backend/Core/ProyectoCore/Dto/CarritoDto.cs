namespace ProyectoCore.Dto
{
    public class CarritoDto
    {
        public int IdCarrito { get; set; }

        public int? IdUsuario { get; set; }

        public virtual ICollection<CarritoProductoDto> CarritoProducto { get; set; } = new List<CarritoProductoDto>();
    }

    public class CarritoPostDto
    {
        public int IdCarrito { get; set; }

        public int? IdUsuario { get; set; }
    }
}
