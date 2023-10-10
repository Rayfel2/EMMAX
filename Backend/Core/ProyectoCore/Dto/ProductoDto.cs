namespace ProyectoCore.Dto
{
    public class ProductoDto
    {
        public int IdProducto { get; set; }

        //  public int? IdCategoria { get; set; }
        public CategoriaDto Categoria { get; set; }

        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }

        public double? Precio { get; set; }

        public int? Stock { get; set; }

        public string? Imagen { get; set; }
    }
}
