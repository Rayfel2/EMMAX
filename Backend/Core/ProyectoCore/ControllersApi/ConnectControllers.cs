using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProyectoCore.Dto;
using ProyectoCore.Interface;
using ProyectoCore.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using ProyectoCore.Models.ViewModels;

namespace ProyectoCore.ControllersApi
{
    [ApiController]
    [Route("[controller]")]
    public class ConnectControllers : Controller
    {

        private readonly IUsuarioRepository _RepositoryUsuario;
        private readonly ICarritoProductoRepository _RepositoryCarritoProducto;
        private readonly IListaProductoRepository _RepositoryListaProducto;
        private readonly ICarritoRepository _RepositoryCarrito;
        private readonly IListaRepository _RepositoryLista;
        private readonly IProductoRepository _RepositoryProducto;
        private readonly IReseñaRepository _RepositoryReseña;
        private readonly IReciboRepository _RepositoryRecibo;
        

        private readonly IMapper _mapper;

        private readonly string _jwtSecret;

        public ConnectControllers(IReciboRepository RepositoryRecibo, IReseñaRepository RepositoryReseña, IListaRepository RepositoryLista, IListaProductoRepository RepositoryListaProducto, IProductoRepository RepositoryProducto, IUsuarioRepository RepositoryUsuario, ICarritoProductoRepository RepositoryCarritoProducto, ICarritoRepository RepositoryCarrito, IMapper mapper, string jwtSecret)
        {
            _RepositoryCarrito = RepositoryCarrito;
            _RepositoryUsuario = RepositoryUsuario;
            _RepositoryCarritoProducto = RepositoryCarritoProducto;
            _jwtSecret = jwtSecret;
            _mapper = mapper;
            _RepositoryProducto = RepositoryProducto;
            _RepositoryListaProducto = RepositoryListaProducto;
            _RepositoryLista = RepositoryLista;
            _RepositoryReseña = RepositoryReseña;
            _RepositoryRecibo = RepositoryRecibo;
        }


        [HttpPost("/login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto loginRequest)
        {
            try
            {
                // Verificar si el correo electrónico es válido
                var usuario = await _RepositoryUsuario.GetUsuarioByEmailAndPasswordAsync(loginRequest.Email, loginRequest.Contraseña);
                if (usuario == null) { return NotFound(); }

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.IdUsuario.ToString()), // ID del usuario como claim
            new Claim("Email", usuario.Email), // Agregar el email como claim personalizado
        };

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "http://localhost:5230/", // Cambia esto a tu emisor JWT
                    audience: "http://localhost:5230/", // Cambia esto a tu audiencia JWT
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1), // Duración del token (1 hora)
                    signingCredentials: credentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                // Devolver el token JWT en la respuesta
                return Ok(new { Token = tokenString });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al iniciar sesión: " + ex.Message);
                return BadRequest(ModelState);
            }
        }

        [Authorize]
        [HttpGet("/Usuario")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Usuario>))]
        public async Task<IActionResult> UsuarioAsync()
        {
            try
            {
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                if (userIdClaim == null)
                {
                    // El claim "Name" no se encontró en el token
                    return BadRequest("No se encontró el claim 'Name' en el token.");
                }

                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    // No se pudo convertir el valor del claim "Name" a un entero
                    return BadRequest("No se pudo convertir 'userId' a un entero.");
                }

                var allUsuarios = await _RepositoryUsuario.GetUsuarioAsync(userId);

                return Ok(allUsuarios);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al obtener los Usuarios: " + ex.Message);
                return BadRequest(ModelState);
            }
        }

        [Authorize]
        [HttpGet("/CarritoProducto")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CarritoProductoDto>))]
        public async Task<IActionResult> GetCarritoProductoAsync(int page, int pageSize)
        {
            try
            {
                // Obtén el ID del usuario autenticado
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                if (userIdClaim == null)
                {
                    // El claim "Name" no se encontró en el token
                    return BadRequest("No se encontró el claim 'Name' en el token.");
                }

                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    // No se pudo convertir el valor del claim "Name" a un entero
                    return BadRequest("No se pudo convertir 'userId' a un entero.");
                }

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

                var userCarritoProducto = await _RepositoryCarrito.GetCarritoAsync(userId);
                if (userCarritoProducto == null)
                {
                    return NotFound();
                }

                var carritoProductos = await _RepositoryCarritoProducto.GetCarritoProductoAsync(userCarritoProducto.IdCarrito);
                var productos = await _RepositoryProducto.GetProductosAsync();

                var result = carritoProductos
                    .Join(
                        productos,
                        cp => cp.IdProducto,
                        p => p.IdProducto,
                        (cp, p) => new { CarritoProducto = cp, Producto = p }
                    )
                    .Select(result => _mapper.Map<CarritoProductoDto>(result.CarritoProducto))
                    .ToList();

                // Aplicamos paginación utilizando LINQ para seleccionar los registros apropiados.
                // A nivel de rutas sería por ejemplo http://localhost:5230/Producto?page=1&pageSize=10

                var pagedCarritoProducto = result.Skip(startIndex).Take(pageSize).ToList();

                var CarritoProductoDtoList = _mapper.Map<List<CarritoProductoDto>>(pagedCarritoProducto);

                return Ok(CarritoProductoDtoList);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al obtener los CarritoProducto: " + ex.Message);
                return BadRequest(ModelState);
            }
        }


        [Authorize]
        [HttpGet("/ListaProducto")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ListaProducto>))]
        public async Task<IActionResult> GetListaProductoAsync(int page, int pageSize)
        {
            try
            {
                // Obtén el ID del usuario autenticado
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                if (userIdClaim == null)
                {
                    // El claim "Name" no se encontró en el token
                    return BadRequest("No se encontró el claim 'Name' en el token.");
                }

                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    // No se pudo convertir el valor del claim "Name" a un entero
                    return BadRequest("No se pudo convertir 'userId' a un entero.");
                }

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

                var allListaProducto = await _RepositoryListaProducto.GetListaProductoAsync();

                // Filtra los productos del carrito para el usuario autenticado
                var userListaProducto = await _RepositoryLista.GetListaAsync(userId);
                if (userListaProducto == null)
                {
                    return NotFound();
                }

                var listaproducto = await _RepositoryListaProducto.GetListaProductosAsync(userListaProducto.IdLista);
                var productos = await _RepositoryProducto.GetProductosAsync();

                // Realiza un Join entre listaproducto y productos
                var joinedList = listaproducto
                    .Join(
                        productos,
                        cp => cp.IdProducto,
                        p => p.IdProducto,
                        (cp, p) => new { ListaProducto = cp, Producto = p }
                    );

                // Mapea el resultado del Join a ListaProductoDto
                var listaProductoDtoList = joinedList
                    .Select(result => _mapper.Map<ListaProductoDto>(result.ListaProducto))
                    .ToList();

                // Aplicamos paginación utilizando LINQ para seleccionar los registros apropiados.
                // A nivel de rutas sería, por ejemplo, http://localhost:5230/Producto?page=1&pageSize=10

                var pagedlistaproducto = listaProductoDtoList.Skip(startIndex).Take(pageSize).ToList();

                return Ok(pagedlistaproducto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al obtener los listaproductoDto: " + ex.Message);
                return BadRequest(ModelState);
            }
        }





        [HttpPost("/Registrar")]
        [Consumes("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostAsync([FromBody] UsuarioPostDto UsuarioPostDTO)
        {
            try
            {
                // Por si el DTO es null
                if (UsuarioPostDTO == null || !ModelState.IsValid) { return BadRequest(ModelState); }

                int IdUsuario = await _RepositoryUsuario.GetUsuarioIdsAsync(UsuarioPostDTO.Email);
                if (await _RepositoryUsuario.UsuarioExistAsync(IdUsuario))
                {
                    return StatusCode(666, "Usuario ya existe");
                }
                CarritoPostDto carritoPostDTO = new CarritoPostDto();
                ListaPostDto listaPostDTO = new ListaPostDto();

                var Usuario = _mapper.Map<Usuario>(UsuarioPostDTO);
                var Carrito = _mapper.Map<Carrito>(carritoPostDTO);
                var Lista = _mapper.Map<ListaDeseo>(listaPostDTO);
                Usuario.ContraseñaHash = await _RepositoryUsuario.HashPasswordAsync(Usuario.Contraseña);

                if (!await _RepositoryUsuario.CreateUsuarioAsync(Usuario))
                {
                    ModelState.AddModelError("", "Ocurrió un error al guardar");
                    return StatusCode(500, ModelState);
                }
                else
                {
                    var UltimoUsuario = await _RepositoryUsuario.GetUltimoUsuarioAgregadoAsync();
                    Carrito.IdUsuario = UltimoUsuario.IdUsuario;
                    Lista.IdUsuario = UltimoUsuario.IdUsuario;

                    if (!await _RepositoryCarrito.CreateCarritoAsync(Carrito))
                    {
                        ModelState.AddModelError("", "Ocurrió un error al guardar");
                        return StatusCode(500, ModelState);
                    }
                    else
                    {
                        if (!await _RepositoryLista.CreateListaAsync(Lista))
                        {
                            ModelState.AddModelError("", "Ocurrió un error al guardar");
                            return StatusCode(500, ModelState);
                        }
                        else
                        {
                            return Ok();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al procesar la solicitud: " + ex.Message);
                return BadRequest(ModelState);
            }
        }



        [HttpPost("/CarritoProducto")]
        [Consumes("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostCarritoAsync([FromBody] CarritoProductoPostDto CarritoProductoPostDTO)
        {
            try
            {
                // Por si el DTO es null
                if (CarritoProductoPostDTO == null || !ModelState.IsValid) { return BadRequest(ModelState); }

                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                if (userIdClaim == null)
                {
                    // El claim "Name" no se encontró en el token
                    return BadRequest("No se encontró el claim 'Name' en el token.");
                }

                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    // No se pudo convertir el valor del claim "Name" a un entero
                    return BadRequest("No se pudo convertir 'userId' a un entero.");
                }

                var userCarritoProducto = await _RepositoryCarrito.GetCarritoAsync(userId);
                if (userCarritoProducto == null)
                {
                    return NotFound();
                }

                var productoCarritoProducto = await _RepositoryProducto.GetProductosAsync(CarritoProductoPostDTO.IdProducto);
                if (productoCarritoProducto == null)
                {
                    return NotFound();
                }

                var allCarritoProducto = await _RepositoryCarritoProducto.GetCarritosProductosAsync(userCarritoProducto.IdCarrito, productoCarritoProducto.IdProducto);
                int cantidad = Convert.ToInt32(CarritoProductoPostDTO.Cantidad);

                if (await _RepositoryCarritoProducto.CarritoProductoExistAsync(userCarritoProducto.IdCarrito, productoCarritoProducto.IdProducto))
                {
                    if (allCarritoProducto.Cantidad != cantidad)
                    {
                        await ActualizarCantidadAsync(productoCarritoProducto.IdProducto, cantidad);
                        return Ok();
                    }
                    else
                    {
                        return StatusCode(666, "Ya está añadido");
                    }
                }

                var carritoProducto = _mapper.Map<CarritoProducto>(CarritoProductoPostDTO);
                carritoProducto.IdCarrito = userCarritoProducto.IdCarrito;
                var producto = await _RepositoryProducto.GetProductosAsync(carritoProducto.IdProducto);
                carritoProducto.Precio = producto.Precio;

                if (!await _RepositoryCarritoProducto.CreateCarritoProductoAsync(carritoProducto))
                {
                    ModelState.AddModelError("", "Ocurrió un error al guardar");
                    return StatusCode(500, ModelState);
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al procesar la solicitud: " + ex.Message);
                return BadRequest(ModelState);
            }
        }


        [HttpPost("/ListaProducto")]
        [Consumes("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostListaAsync([FromBody] ListaProductoPostDto ListaProductoPostDTO)
        {
            try
            {
                // Por si el DTO es null
                if (ListaProductoPostDTO == null || !ModelState.IsValid) { return BadRequest(ModelState); }

                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                if (userIdClaim == null)
                {
                    // El claim "Name" no se encontró en el token
                    return BadRequest("No se encontró el claim 'Name' en el token.");
                }

                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    // No se pudo convertir el valor del claim "Name" a un entero
                    return BadRequest("No se pudo convertir 'userId' a un entero.");
                }

                var userListaProducto = await _RepositoryLista.GetListaAsync(userId);
                if (userListaProducto == null)
                {
                    return NotFound();
                }

                var productoListaProducto = await _RepositoryProducto.GetProductosAsync(ListaProductoPostDTO.IdProducto);
                if (productoListaProducto == null)
                {
                    return NotFound();
                }

                if (await _RepositoryListaProducto.ListaProductoExistAsync(userListaProducto.IdLista, productoListaProducto.IdProducto))
                {
                    return StatusCode(666, "Ya está añadido");
                }

                var listaProducto = _mapper.Map<ListaProducto>(ListaProductoPostDTO);
                listaProducto.IDListaProducto = userListaProducto.IdLista;

                if (!await _RepositoryListaProducto.CreateListaProductoAsync(listaProducto))
                {
                    ModelState.AddModelError("", "Ocurrió un error al guardar");
                    return StatusCode(500, ModelState);
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al procesar la solicitud: " + ex.Message);
                return BadRequest(ModelState);
            }
        }



        [HttpPost("/Comentarios")]
        [Consumes("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostComentariosAsync([FromBody] ReseñaPostDto ReseñaPostDTO)
        {
            try
            {
                // Por si el DTO es null
                if (ReseñaPostDTO == null || !ModelState.IsValid) { return BadRequest(ModelState); }

                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                if (userIdClaim == null)
                {
                    // El claim "Name" no se encontró en el token
                    return BadRequest("No se encontró el claim 'Name' en el token.");
                }

                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    // No se pudo convertir el valor del claim "Name" a un entero
                    return BadRequest("No se pudo convertir 'userId' a un entero.");
                }

                var user = await _RepositoryUsuario.GetUsuarioAsync(userId);
                if (user == null)
                {
                    return NotFound("Usuario no encontrado");
                }

                var producto = await _RepositoryProducto.GetProductosAsync(Convert.ToInt32(ReseñaPostDTO.IdProducto));
                if (producto == null)
                {
                    return NotFound("Producto no encontrado");
                }

                /*
                if (await _RepositoryListaProducto.ReseñaExistAsync(userListaProducto.IdLista, productoListaProducto.IdProducto))
                {
                    return StatusCode(666, "Ya está añadido");
                }
                */

                var Reseña = _mapper.Map<Reseña>(ReseñaPostDTO);
                Reseña.IdUsuario = userId;

                if (!await _RepositoryReseña.CreateReseñaAsync(Reseña))
                {
                    ModelState.AddModelError("", "Ocurrió un error al guardar");
                    return StatusCode(500, ModelState);
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al procesar la solicitud: " + ex.Message);
                return BadRequest(ModelState);
            }
        }


        [HttpDelete("/ListaProducto/{idProducto}")]
        [Consumes("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteListaAsync(int idProducto)
        {
            try
            {
                // por si el idProducto es null
                if (idProducto == null || !ModelState.IsValid) { return BadRequest(ModelState); }

                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                if (userIdClaim == null)
                {
                    // El claim "Name" no se encontró en el token
                    return BadRequest("No se encontró el claim 'Name' en el token.");
                }

                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    // No se pudo convertir el valor del claim "Name" a un entero
                    return BadRequest("No se pudo convertir 'userId' a un entero.");
                }

                var userListaProducto = await _RepositoryLista.GetListaAsync(userId);
                if (userListaProducto == null)
                {
                    return NotFound();
                }

                var ListaProducto = await _RepositoryListaProducto.GetListasProductosAsync(userListaProducto.IdLista, idProducto);

                if (!await _RepositoryListaProducto.DeleteListaProductoAsync(ListaProducto))
                {
                    ModelState.AddModelError("", "Ocurrió un error al eliminar");
                    return StatusCode(500, ModelState);
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al procesar la solicitud: " + ex.Message);
                return BadRequest(ModelState);
            }
        }


        [HttpDelete("/CarritoProducto/{idProducto}")]
        [Consumes("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteCarritoAsync(int idProducto)
        {
            try
            {
                // por si el idProducto es null
                if (idProducto == null || !ModelState.IsValid) { return BadRequest(ModelState); }

                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                if (userIdClaim == null)
                {
                    // El claim "Name" no se encontró en el token
                    return BadRequest("No se encontró el claim 'Name' en el token.");
                }

                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    // No se pudo convertir el valor del claim "Name" a un entero
                    return BadRequest("No se pudo convertir 'userId' a un entero.");
                }

                var userCarritoProducto = await _RepositoryCarrito.GetCarritoAsync(userId);
                if (userCarritoProducto == null)
                {
                    return NotFound();
                }

                var CarritoProducto = await _RepositoryCarritoProducto.GetCarritosProductosAsync(userCarritoProducto.IdCarrito, idProducto);

                if (!await _RepositoryCarritoProducto.DeleteCarritoProductoAsync(CarritoProducto))
                {
                    ModelState.AddModelError("", "Ocurrió un error al eliminar");
                    return StatusCode(500, ModelState);
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al procesar la solicitud: " + ex.Message);
                return BadRequest(ModelState);
            }
        }


        [HttpPost("/Comprar")]
        [Consumes("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostReciboAsync([FromBody] ReciboPostDto ReciboPostDTO)
        {
            try
            {
                // por si el DTO es null
                if (ReciboPostDTO == null || !ModelState.IsValid) { return BadRequest(ModelState); }

                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                if (userIdClaim == null)
                {
                    // El claim "Name" no se encontró en el token
                    return BadRequest("No se encontró el claim 'Name' en el token.");
                }

                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    // No se pudo convertir el valor del claim "Name" a un entero
                    return BadRequest("No se pudo convertir 'userId' a un entero.");
                }

                var carrito = await _RepositoryCarrito.GetCarritoAsync(userId);
                if (carrito == null)
                {
                    return NotFound();
                }

                var allCarritoProductos = await _RepositoryCarritoProducto.GetCarritoProductoAsync(carrito.IdCarrito);
                if (allCarritoProductos == null || !allCarritoProductos.Any())
                {
                    return NotFound("No hay productos en el carrito");
                }
                double sumaValorTotal = 0;

                foreach (var allCarritoProducto in allCarritoProductos)
                {
                    double valorTotal = Convert.ToDouble(allCarritoProducto.Cantidad * allCarritoProducto.Precio);
                    sumaValorTotal += valorTotal;
                }

                var reciboProducto = _mapper.Map<Recibo>(ReciboPostDTO);
                reciboProducto.IdCarrito = carrito.IdCarrito;
                reciboProducto.Subtotal = sumaValorTotal;
                if (reciboProducto.Impuestos == null) reciboProducto.Impuestos = 1;

                if (!await _RepositoryRecibo.CreateReciboAsync(reciboProducto))
                {
                    ModelState.AddModelError("", "Ocurrió un error al guardar el recibo");
                    return StatusCode(500, ModelState);
                }
                else
                {
                    foreach (var allCarritoProducto in allCarritoProductos)
                    {
                        await PatchProductoPorIdAsync(allCarritoProducto.IdProducto, Convert.ToInt32(allCarritoProducto.Cantidad));
                        await DeleteCarritoAsync(allCarritoProducto.IdProducto);
                    }
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al procesar la solicitud: " + ex.Message);
                return BadRequest(ModelState);
            }
        }


        [HttpPatch("/Producto/{idProducto}")]
        [ProducesResponseType(200, Type = typeof(Producto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PatchProductoPorIdAsync(int idProducto, int cantidad)
        {
            try
            {
                var producto = await _RepositoryProducto.GetProductosAsync(idProducto);
                if (producto == null)
                {
                    return NotFound("Producto no encontrado"); // Producto no encontrado
                }

                var Producto = _mapper.Map<Producto>(producto);
                Producto.Stock = producto.Stock - cantidad;
                if (!await _RepositoryProducto.UpdateProductoAsync(Producto))
                {
                    ModelState.AddModelError("", "Ocurrió un error al guardar el producto");
                    return StatusCode(500, ModelState);
                }
                else { return Ok(); }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al obtener el producto: " + ex.Message);
                return BadRequest(ModelState);
            }
        }


        [HttpPatch("/CarritoProducto/{idProducto}/{cantidad}")]
        [ProducesResponseType(200, Type = typeof(Producto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ActualizarCantidadAsync(int idProducto, int cantidad)
        {
            try
            {
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                if (userIdClaim == null)
                {
                    // El claim "Name" no se encontró en el token
                    return BadRequest("No se encontró el claim 'Name' en el token.");
                }

                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    // No se pudo convertir el valor del claim "Name" a un entero
                    return BadRequest("No se pudo convertir 'userId' a un entero.");
                }

                var allCarritoProducto = await _RepositoryCarritoProducto.GetCarritoProductoAsync();

                // Filtra los productos del carrito para el usuario autenticado

                var userCarritoProducto = await _RepositoryCarrito.GetCarritoAsync(userId);
                if (userCarritoProducto == null)
                {
                    return NotFound();
                }

                //   var CarritoProducto = _RepositoryCarritoProducto.GetCarritoProducto(userCarritoProducto.IdCarrito);
                var CarritoProducto = await _RepositoryCarritoProducto.GetCarritosProductosAsync(userCarritoProducto.IdCarrito, idProducto);

                if (CarritoProducto == null)
                {
                    return NotFound("Producto no encontrado en el carrito");
                }

                var CarritoProductoList = _mapper.Map<CarritoProducto>(CarritoProducto);
                CarritoProductoList.Cantidad = cantidad;
                if (!await _RepositoryCarritoProducto.UpdateCarritoProductoAsync(CarritoProductoList))
                {
                    ModelState.AddModelError("", "Ocurrió un error al actualizar la cantidad");
                    return StatusCode(500, ModelState);
                }
                else { return Ok(); }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al obtener el producto: " + ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
