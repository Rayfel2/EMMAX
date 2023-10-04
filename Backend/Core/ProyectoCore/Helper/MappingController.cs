using AutoMapper;
using ProyectoCore.Dto;
using ProyectoCore.Models;
namespace ProyectoCore.Helper
{
    public class MappingController : Profile
    {
        public MappingController()
        {
            // Get (de la tabla al dto)
            CreateMap<Producto, ProductoDto>();
            CreateMap <Categorium, CategoriaDto>();
            CreateMap < Reseña, ReseñaDto>();
            CreateMap <Usuario, UsuarioDto>();
            CreateMap <CarritoProducto, CarritoProductoDto>();
            CreateMap <ListaProducto, ListaProductoDto>();

            // Post (del dto a la tabla)
            CreateMap<ProductoDto, Producto>();
            CreateMap<UsuarioPostDto, Usuario>();
            CreateMap<CarritoProductoDto, CarritoProducto>();

            //put (del dto a la tabla)
            CreateMap<ProductoDto, Producto>();
        }
    }
}
