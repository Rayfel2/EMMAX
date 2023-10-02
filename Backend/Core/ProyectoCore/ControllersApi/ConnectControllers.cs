using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ProyectoCore.Interface;
using ProyectoCore.Repository;
using ProyectoCore.Models;
using ProyectoCore.Dto;
using Microsoft.Identity.Client;

namespace ProyectoCore.ControllersApi
{
    [ApiController]
    [Route("[controller]")]
    public class ConnectControllers : Controller
    {

        private readonly IUsuarioRepository _RepositoryUsuario;
        private readonly ICarritoProductoRepository _RepositoryCarritoProducto;
        private readonly IMapper _mapper;

        public ConnectControllers(IUsuarioRepository RepositoryUsuario, ICarritoProductoRepository RepositoryCarritoProducto ,IMapper mapper)
        {
            _RepositoryUsuario = RepositoryUsuario;
            _RepositoryCarritoProducto = RepositoryCarritoProducto;
            _mapper = mapper;
        }

        [HttpGet("/Usuario")] // no recuerdo si debia ser plural o singular
        [ProducesResponseType(200, Type = typeof(IEnumerable<Usuario>))]
        public IActionResult Usuario(int page, int pageSize)
        {
            try
            {
                // Evitando valores negativos
                if (page < 1)
                {
                    page = 1; // Página mínima
                }

                if (pageSize < 1)
                {
                    pageSize = 10; // Tamaño de página predeterminado
                }

                // Utilizado para determinar donde comienza cada pagina
                int startIndex = (page - 1) * pageSize;


                var allUsuarios = _RepositoryUsuario.GetUsuarios();

                // Aplicamos paginación utilizando LINQ para seleccionar los registros apropiados.
                // A nivel de rutas seria por ejemplo http://localhost:5230/categoria?page=1&pageSize=10
                var pagedUsuario = allUsuarios.Skip(startIndex).Take(pageSize).ToList();
                //.skip omite un numero de registro
                //.Take cantidad elemento que se van a tomar


                // Mapeo los empleados paginados en vez de todos
                var UsuarioDtoList = _mapper.Map<List<UsuarioDto>>(pagedUsuario);


                return Ok(UsuarioDtoList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al obtener los Usuarios: " + ex.Message);
                return BadRequest(ModelState);
            }
        }

            [HttpGet("/CarritoProducto")]
            [ProducesResponseType(200, Type = typeof(IEnumerable<CarritoProducto>))]
            public IActionResult GetCarritoProducto(int page, int pageSize)
            {
                try
                {
                    // Evitando valores negativos
                    if (page < 1)
                    {
                        page = 1; // Página mínima
                    }

                    if (pageSize < 1)
                    {
                        pageSize = 10; // Tamaño de página predeterminado
                    }

                    // Utilizado para determinar donde comienza cada pagina
                    int startIndex = (page - 1) * pageSize;


                    var allCarritoProducto = _RepositoryCarritoProducto.GetCarritoProducto();

                    // Aplicamos paginación utilizando LINQ para seleccionar los registros apropiados.
                    // A nivel de rutas seria por ejemplo http://localhost:5230/Producto?page=1&pageSize=10
                    var pagedCarritoProducto = allCarritoProducto.Skip(startIndex).Take(pageSize).ToList();
                    //.skip omite un numero de registro
                    //.Take cantidad elemento que se van a tomar


                    // Mapeo los empleados paginados en vez de todos
                    var CarritoProductoDtoList = _mapper.Map<List<CarritoProductoDto>>(pagedCarritoProducto);


                    return Ok(CarritoProductoDtoList);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error al obtener los CarritoProducto: " + ex.Message);
                    return BadRequest(ModelState);
                }
            }
        
        
    }
}
