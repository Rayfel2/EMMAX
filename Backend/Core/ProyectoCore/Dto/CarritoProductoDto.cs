namespace ProyectoCore.Dto
{
    public class CarritoProductoDto
    {
        public int IdCarrito { get; set; }

        //public int IdProducto { get; set; }
        public ProductoDto Producto { get; set; }

        public int? Cantidad { get; set; }

        public double? Precio { get; set; }

        
    }
    public class CarritoProductoPostDto
    {
//        public int IdCarrito { get; set; }

        public int IdProducto { get; set; }

        public int? Cantidad { get; set; }

        public double? Precio { get; set; }

    }
}
