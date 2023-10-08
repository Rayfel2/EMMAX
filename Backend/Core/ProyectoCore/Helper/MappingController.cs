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
            CreateMap<Categorium, CategoriaDto>();
            CreateMap<Reseña, ReseñaDto>();
            CreateMap<Usuario, UsuarioDto>();
            CreateMap<ListaDeseo, ListaDto>();
            CreateMap<ListaProducto, ListaProductoDto>();
            CreateMap<CarritoProducto, CarritoProductoDto>();

            CreateMap<CarritoProducto, CarritoProductoDto>()
                .ForMember(
                    dest => dest.Producto,
                    opt => opt.MapFrom(src => src.oProducto)
                );

            CreateMap<ListaProducto, ListaProductoDto>()
                .ForMember(
                    dest => dest.Producto,
                    opt => opt.MapFrom(src => src.oProducto)
                );
            /*
             * dest => dest.Producto,
        opt => opt.MapFrom(src => new ProductoDto
        {
            Nombre = src.oProducto.Nombre,
        })
             */


            // Post (del dto a la tabla)
            CreateMap<ProductoDto, Producto>();
            CreateMap<UsuarioPostDto, Usuario>();
            CreateMap<CarritoProductoPostDto, CarritoProducto>();
            CreateMap<ListaProductoPostDto, ListaProducto>();
            CreateMap<ReseñaPostDto, Reseña>();

            //put (del dto a la tabla)
            CreateMap<ProductoDto, Producto>();
        }
    }
}

