namespace ProyectoCore.Models
{
    public class ListaProducto
    {
        public int IDLista { get; set; }

        public int IdProducto { get; set; }

        public virtual ListaDeseo oListaDeseo { get; set; } = null!;

        public virtual Producto oProducto { get; set; } = null!;
    }
}
