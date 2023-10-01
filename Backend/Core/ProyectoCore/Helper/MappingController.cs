﻿using AutoMapper;
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
            CreateMap <Reseña, ReseñaDto>();

            // Post (del dto a la tabla)
            CreateMap<ProductoDto, Producto>();

            //put (del dto a la tabla)
            CreateMap<ProductoDto, Producto>();
        }
    }
}
