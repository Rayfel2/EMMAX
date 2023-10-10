using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ProyectoCore.Interface;
using ProyectoCore.Repository;
using ProyectoCore.Models;
using ProyectoCore.Dto;
using System.Collections.Immutable;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging;
using Microsoft.CodeAnalysis;

namespace ProyectoCore.ControllersApi
{
    [ApiController]
    [Route("[controller]")]
    public class ShopController : Controller
    {
        private readonly IProductoRepository _RepositoryProducto;
        private readonly ICategoriaRepository _RepositoryCategoria;
        private readonly IReseñaRepository _RepositoryReseña;
        private readonly IUsuarioRepository _RepositoryUsuario;
        private readonly IMetodoRepository _RepositoryMetodo;
        private readonly IMapper _mapper;

        public ShopController(IMetodoRepository RepositoryMetodo, IUsuarioRepository RepositoryUsuario, IProductoRepository RepositoryProducto, ICategoriaRepository RepositoryCategoria, IReseñaRepository RepositoryReseña, IMapper mapper)
        {
            _RepositoryUsuario = RepositoryUsuario;
            _RepositoryCategoria = RepositoryCategoria;
            _RepositoryReseña = RepositoryReseña;
            _RepositoryProducto = RepositoryProducto;
            _mapper = mapper;
            _RepositoryMetodo = RepositoryMetodo;
        }


        [HttpGet("/Producto")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Producto>))]
        public IActionResult GetProducto(int page, int pageSize, 
            [FromQuery] List<string> categoryFilter = null,
            [FromQuery] List<string> productFilter = null,
            [FromQuery] bool recentProduct = false,
             [FromQuery] bool categoryProduct = false,
             [FromQuery] bool reviewProduct = false)
        {
            try
            {
                // Evitar valores negativos
                if (page < 1)
                {
                    page = 1; // Página mínima
                }

                if (pageSize < 1)
                {
                    pageSize = 10; // Tamaño de página predeterminado
                }

                // Utilizado para determinar dónde comienza cada página
                int startIndex = (page - 1) * pageSize;

                var allProductos = _RepositoryProducto.GetProductos();

                // Aplicar filtros según los valores proporcionados
                if (categoryFilter != null && categoryFilter.Any())
                {
                    var categoriasIds = _RepositoryCategoria.GetCategoriaIdsByPartialNames(categoryFilter);

                    allProductos = allProductos
                        .Where(i => categoriasIds.Contains(Convert.ToInt32(i.IdCategoria)))
                        .ToList();
                }

                if (productFilter != null && productFilter.Any())
                {
                    var productosIds = _RepositoryProducto.GetProductoIdsByPartialNames(productFilter);
                    var categoriasIds = _RepositoryCategoria.GetCategoriaIdsByPartialNames(productFilter);

                    allProductos = allProductos
                        .Where(i => productosIds.Contains(Convert.ToInt32(i.IdProducto)) || categoriasIds.Contains(Convert.ToInt32(i.IdCategoria)))
                        .ToList();

                }

                if (recentProduct)
                {
                    allProductos = _RepositoryProducto.GetProductosDescending();
                }

                if (categoryProduct)
                {
                    allProductos = _RepositoryProducto.GetProductoCategoria();
   
                }

                if (reviewProduct)
                {
                    foreach (var producto in allProductos)
                    {
                        // Obtener las reseñas filtradas por ID de producto, incluyendo datos de usuario
                        var reseñasFiltradas = _RepositoryReseña
                            .GetReseñas()
                            .Where(r => r.IdProducto == producto.IdProducto)
                            .ToList();

                        // Inicializar una variable para almacenar la suma de valor reseña
                        int sumaValorReseña = 0;

                        // Recorrer todas las reseñas del producto y sumar sus valores
                        foreach (var reseña in reseñasFiltradas)
                        {
                            sumaValorReseña += Convert.ToInt32(reseña.ValorReseña);
                        }
                        producto.Stock = sumaValorReseña;
                    }

                    // Ordenar los productos en función del campo Stock de manera descendente
                    allProductos = allProductos.OrderByDescending(p => p.Stock).ToList();
                }




                // Aplicar paginación utilizando LINQ para seleccionar los registros apropiados.
                // A nivel de rutas sería, por ejemplo, http://localhost:5230/Producto?page=1&pageSize=10
                var pagedProductos = allProductos.Skip(startIndex).Take(pageSize).ToList();

                // Mapear los productos paginados en lugar de todos
                var ProductoDtoList = _mapper.Map<List<ProductoDto>>(pagedProductos);

                return Ok(ProductoDtoList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al obtener los productos: " + ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet("/Producto/{idProducto}")]
        [ProducesResponseType(200, Type = typeof(Producto))]
        [ProducesResponseType(404)]
        public IActionResult GetProductoPorId(int idProducto)
        {
            try
            {
                var producto = _RepositoryProducto.GetProductos(idProducto);

                if (producto == null)
                {
                    return NotFound(); // Producto no encontrado
                }

                var ProductoDto = _mapper.Map<ProductoDto>(producto);

                return Ok(ProductoDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al obtener el producto: " + ex.Message);
                return BadRequest(ModelState);
            }
        }



        [HttpGet("/Categoria")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Categorium>))]
        public IActionResult Getcategoria(int page, int pageSize)
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


                var allcategorias = _RepositoryCategoria.GetCategorias();

                // Aplicamos paginación utilizando LINQ para seleccionar los registros apropiados.
                // A nivel de rutas seria por ejemplo http://localhost:5230/categoria?page=1&pageSize=10
                var pagedcategorias = allcategorias.Skip(startIndex).Take(pageSize).ToList();
                //.skip omite un numero de registro
                //.Take cantidad elemento que se van a tomar


                // Mapeo los empleados paginados en vez de todos
                var categoriaDtoList = _mapper.Map<List<CategoriaDto>>(pagedcategorias);


                return Ok(categoriaDtoList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al obtener los empleados: " + ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet("/reseña/{productId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Categorium>))]
        public IActionResult Getreseña(int page, int pageSize, int productId)
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

                // Obtener las reseñas filtradas por ID de producto, incluyendo datos de usuario
                var reseñasFiltradas = _RepositoryReseña
    .GetReseñas()
    .Where(r => r.IdProducto == productId)
    .Skip(startIndex)
    .Take(pageSize)
    .ToList();

                // Mapear las reseñas a objetos ReseñaDto y asignar nombres de usuario
                var reseñaDtoList = reseñasFiltradas.Select(reseña => new ReseñaDto
                {
                    IdReseña = reseña.IdReseña,
                    Usuario = _RepositoryUsuario.GetUsuario(Convert.ToInt32(reseña.IdUsuario))?.NombreUsuario,
                    IdProducto = reseña.IdProducto,
                    ValorReseña = reseña.ValorReseña,
                    Comentario = reseña.Comentario
                }).ToList();

                return Ok(reseñaDtoList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al obtener las reseñas: " + ex.Message);
                return BadRequest(ModelState);
            }
        }


        [HttpGet("/Metodo")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MetodoPago>))]
        public IActionResult GetMetodo(int page, int pageSize)
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


                var allmetodos = _RepositoryMetodo.GetMetodo();

                // Aplicamos paginación utilizando LINQ para seleccionar los registros apropiados.
                // A nivel de rutas seria por ejemplo http://localhost:5230/categoria?page=1&pageSize=10
                var pagedmetodos = allmetodos.Skip(startIndex).Take(pageSize).ToList();
                //.skip omite un numero de registro
                //.Take cantidad elemento que se van a tomar


                // Mapeo los empleados paginados en vez de todos
                var metodoDtoList = _mapper.Map<List<MetodoDto>>(pagedmetodos);


                return Ok(metodoDtoList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al obtener los empleados: " + ex.Message);
                return BadRequest(ModelState);
            }
        }
    }

}
