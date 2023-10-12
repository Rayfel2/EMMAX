using HotChocolate.Types.Relay;
using HotChocolate.Types;
using ProyectoCore.Models;
using HotChocolate;
using Microsoft.EntityFrameworkCore;

namespace ProyectoCore.GraphQL
{

    public class UsuarioType : ObjectType<Usuario>
    {
        protected override void Configure(IObjectTypeDescriptor<Usuario> descriptor)
        {
           // descriptor.Name("Usuario");
            descriptor.Field(u => u.IdUsuario).Type<IntType>();
            descriptor.Field(u => u.Nombre).Type<StringType>();
            descriptor.Field(u => u.Apellido).Type<StringType>();
            descriptor.Field(u => u.Dirección).Type<StringType>();
            descriptor.Field(u => u.Teléfono).Type<StringType>();
            descriptor.Field(u => u.Email).Type<StringType>();
            descriptor.Field(u => u.NombreUsuario).Type<StringType>();
            descriptor.Field(u => u.FechaNacimiento).Type<DateTimeType>();
            descriptor.Field(u => u.Contraseña).Type<StringType>();
            descriptor.Field(u => u.ContraseñaHash).Type<StringType>();
        }
    }

    public class Query
    {
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Usuario> GetUsuario([Service] TiendaPruebaContext dbContext, int userId)
        {
            // Filtra los usuarios por su ID
            return dbContext.Usuarios.Where(u => u.IdUsuario == userId);
        }
    }

}
