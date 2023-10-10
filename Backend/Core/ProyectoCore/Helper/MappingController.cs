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
            CreateMap<Producto, ProductoDto>()
                            .ForMember(
                    dest => dest.Categoria,
                    opt => opt.MapFrom(src => src.oCategorium)
                );
            CreateMap<Categorium, CategoriaDto>();
            CreateMap<Reseña, ReseñaDto>();
            CreateMap<Usuario, UsuarioDto>();
            CreateMap<ListaDeseo, ListaDto>();
            CreateMap<ListaProducto, ListaProductoDto>();
            CreateMap<CarritoProducto, CarritoProductoDto>();
            CreateMap<Recibo, ReciboDto>();
            CreateMap<MetodoPago, MetodoDto>();


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
            CreateMap<ReciboPostDto, Recibo>();
            CreateMap<ListaPostDto, ListaDeseo>();
            CreateMap<CarritoPostDto, Carrito>();

            //put (del dto a la tabla)
            CreateMap<ProductoDto, Producto>();
        }
    }
}

