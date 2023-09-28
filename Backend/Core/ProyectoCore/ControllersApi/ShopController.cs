using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ProyectoCore.Interface;
using ProyectoCore.Repository;
using ProyectoCore.Models;
using ProyectoCore.Dto;

namespace ProyectoCore.ControllersApi
{
    [ApiController]
    [Route("[controller]")]
    public class ShopController : Controller
    {
        private readonly IProductoRepository _RepositoryProducto;
        private readonly IMapper _mapper;

        public ShopController(IProductoRepository RepositoryProducto, IMapper mapper)
        {
            _RepositoryProducto = RepositoryProducto;
            _mapper = mapper;
        }


        [HttpGet("/Producto")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Producto>))]
        public IActionResult GetProducto(int page, int pageSize)
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


                var allProductos = _RepositoryProducto.GetProductos();

                // Aplicamos paginación utilizando LINQ para seleccionar los registros apropiados.
                // A nivel de rutas seria por ejemplo http://localhost:5204/Producto?page=1&pageSize=10
                var pagedProductos = allProductos.Skip(startIndex).Take(pageSize).ToList();
                //.skip omite un numero de registro
                //.Take cantidad elemento que se van a tomar


                // Mapeo los empleados paginados en vez de todos
                var ProductoDtoList = _mapper.Map<List<ProductoDto>>(pagedProductos);
              

                return Ok(ProductoDtoList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al obtener los empleados: " + ex.Message);
                return BadRequest(ModelState);
            }
        }
    }

}
