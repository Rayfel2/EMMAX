namespace ProyectoCore.Dto
{
    public class ListaProductoDto
    {
        public int IDListaProducto { get; set; }

        // public int IdProducto { get; set; }
        public ProductoDto Producto { get; set; }
    }

    public class ListaProductoPostDto
    {
        public int IDListaProducto { get; set; }

        public int IdProducto { get; set; }
    }
}
