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
using Microsoft.AspNetCore.Authorization;
using ProyectoCore.Models.ViewModels;

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
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductoDto>))]
        public async Task<IActionResult> GetProductoAsync(int page, int pageSize,
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

                var productos = await _RepositoryProducto.GetProductosAsync();
                var categorias = await _RepositoryCategoria.GetCategoriasAsync();

                var allProductos = productos
                    .Join(
                        categorias,
                        cp => cp.IdCategoria,
                        p => p.IdCategoria,
                        (cp, p) => new { Producto = cp, Categoria = p }
                    )
                    .Select(result => _mapper.Map<ProductoDto>(result.Producto))
                    .ToList();

                if (categoryFilter != null && categoryFilter.Count > 0)
                {
                    // Filtrar productos por categoría
                    allProductos = allProductos.Where(p => categoryFilter.Contains(p.Categoria.Nombre)).ToList();
                }
                if (productFilter != null && productFilter.Count > 0)
                {
                    allProductos = allProductos.Where(p =>
                        productFilter.Any(filterTerm =>
                            p.Nombre.IndexOf(filterTerm, StringComparison.OrdinalIgnoreCase) >= 0
                            || p.Categoria.Nombre.IndexOf(filterTerm, StringComparison.OrdinalIgnoreCase) >= 0
                        )
                    ).ToList();
                }



                if (recentProduct)
                {
                    allProductos = allProductos.OrderByDescending(p => p.IdProducto).ToList();
                }

                if (categoryProduct)
                {
                    allProductos = allProductos.Where(p => p.Categoria.IdCategoria != null) // Filtrar productos con categoría
                        .GroupBy(p => p.Categoria.IdCategoria) // Agrupa los productos por IdCategoria
                        .Select(group => group.First()) // Selecciona el primer producto de cada grupo (categoría)
                        .ToList();
                }

                if (reviewProduct)
                {
                    foreach (var producto in allProductos)
                    {
                        // Obtener las reseñas filtradas por ID de producto, incluyendo datos de usuario
                        var reseñasFiltradasTask = _RepositoryReseña.GetReseñasAsyncWithId(producto.IdProducto);
                        await reseñasFiltradasTask; // Espera a que la tarea se complete

                        var reseñasFiltradas = await reseñasFiltradasTask; // Obtiene el resultado de la tarea

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

                return Ok(pagedProductos);
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
        public async Task<IActionResult> GetProductoPorIdAsync(int idProducto)
        {
            try
            {
                var producto = await _RepositoryProducto.GetProductosAsync(idProducto);

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
        public async Task<IActionResult> GetcategoriaAsync(int page, int pageSize)
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

                // Utilizado para determinar donde comienza cada página
                int startIndex = (page - 1) * pageSize;

                var allcategorias = await _RepositoryCategoria.GetCategoriasAsync();

                // Aplicamos paginación utilizando LINQ para seleccionar los registros apropiados.
                // A nivel de rutas sería, por ejemplo, http://localhost:5230/categoria?page=1&pageSize=10
                var pagedcategorias = allcategorias.Skip(startIndex).Take(pageSize).ToList();

                // Mapeo los empleados paginados en vez de todos
                var categoriaDtoList = _mapper.Map<List<CategoriaDto>>(pagedcategorias);

                return Ok(categoriaDtoList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al obtener las categorías: " + ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet("/reseña/{productId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReseñaDto>))]
        public async Task<IActionResult> GetReseña(int page, int pageSize, int productId)
        {
            try
            {
                // Evita valores negativos
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

                // Obtener las reseñas filtradas por ID de producto, incluyendo datos de usuario
                var reseñasFiltradas = await _RepositoryReseña.GetReseñasAsyncWithId(productId);
                var reseñasFiltradasList = reseñasFiltradas
                    .Skip(startIndex)
                    .Take(pageSize)
                    .ToList();

                // Mapear las reseñas a objetos ReseñaDto y asignar nombres de usuario
                var reseñaDtoList = new List<ReseñaDto>();

                foreach (var reseña in reseñasFiltradas)
                {
                    var usuario = await _RepositoryUsuario.GetUsuarioAsync(Convert.ToInt32(reseña.IdUsuario));
                    var reseñaDto = new ReseñaDto
                    {
                        IdReseña = reseña.IdReseña,
                        Usuario = usuario?.NombreUsuario,
                        IdProducto = reseña.IdProducto,
                        ValorReseña = reseña.ValorReseña,
                        Comentario = reseña.Comentario
                    };

                    reseñaDtoList.Add(reseñaDto);
                }

                return Ok(reseñaDtoList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al obtener las reseñas: " + ex.Message);
                return BadRequest(ModelState);
            }
        }







        [HttpGet("/Metodo")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MetodoDto>))]
        public async Task<IActionResult> GetMetodoAsync(int page, int pageSize)
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

                // Utilizado para determinar donde comienza cada página
                int startIndex = (page - 1) * pageSize;

                // Obtener métodos de pago de forma asincrónica
                var allMetodos = await _RepositoryMetodo.GetMetodoAsync();

                // Aplicar paginación utilizando LINQ
                var pagedMetodos = allMetodos.Skip(startIndex).Take(pageSize).ToList();

                // Mapear los métodos de pago a objetos MetodoDto
                var metodoDtoList = _mapper.Map<List<MetodoDto>>(pagedMetodos);

                return Ok(metodoDtoList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al obtener los métodos de pago: " + ex.Message);
                return BadRequest(ModelState);
            }
        }

    }

}
