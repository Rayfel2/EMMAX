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
                var usuario = _RepositoryUsuario.GetUsuarioByEmailAndPassword(loginRequest.Email, loginRequest.Contraseña);
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
        public IActionResult Usuario()
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
                /*
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
                */

                var allUsuarios = _RepositoryUsuario.GetUsuario(userId);

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
        [ProducesResponseType(200, Type = typeof(IEnumerable<CarritoProducto>))]
        public IActionResult GetCarritoProducto(int page, int pageSize)
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

                var allCarritoProducto = _RepositoryCarritoProducto.GetCarritoProducto();

                // Filtra los productos del carrito para el usuario autenticado

                var userCarritoProducto = _RepositoryCarrito.GetCarrito(userId);
                if (userCarritoProducto == null)
                {
                    return NotFound();
                }
                //   var CarritoProducto = _RepositoryCarritoProducto.GetCarritoProducto(userCarritoProducto.IdCarrito);

                var CarritoProducto = _RepositoryCarritoProducto.GetCarritoProducto(userCarritoProducto.IdCarrito)
            .Join(
                _RepositoryProducto.GetProductos(),
                cp => cp.IdProducto,
                p => p.IdProducto,
                (cp, p) => new { CarritoProducto = cp, Producto = p }
            )
            .Select(result => _mapper.Map<CarritoProductoDto>(result.CarritoProducto))
            .ToList();

                // Aplicamos paginación utilizando LINQ para seleccionar los registros apropiados.
                // A nivel de rutas sería por ejemplo http://localhost:5230/Producto?page=1&pageSize=10

                var pagedCarritoProducto = CarritoProducto.Skip(startIndex).Take(pageSize).ToList();

                // Mapeo los elementos del carrito paginados en vez de todos

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
        public IActionResult GetListaProducto(int page, int pageSize)
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

                var alllistaproducto = _RepositoryListaProducto.GetListaProducto();

                // Filtra los productos del carrito para el usuario autenticado

                var userlistaproducto = _RepositoryLista.GetLista(userId);
                if (userlistaproducto == null)
                {
                    return NotFound();
                }
                //   var listaproducto = _Repositorylistaproducto.Getlistaproducto(userlistaproducto.IdCarrito);

                var listaproducto = _RepositoryListaProducto.GetListaProductos(userlistaproducto.IdLista)
            .Join(
                _RepositoryProducto.GetProductos(),
                cp => cp.IdProducto,
                p => p.IdProducto,
                (cp, p) => new { listaproducto = cp, Producto = p }
            )
            .Select(result => _mapper.Map<ListaProductoDto>(result.listaproducto))
            .ToList();

                // Aplicamos paginación utilizando LINQ para seleccionar los registros apropiados.
                // A nivel de rutas sería por ejemplo http://localhost:5230/Producto?page=1&pageSize=10

                var pagedlistaproducto = listaproducto.Skip(startIndex).Take(pageSize).ToList();

                // Mapeo los elementos del carrito paginados en vez de todos

                var listaproductoDtoList = _mapper.Map<List<ListaProductoDto>>(pagedlistaproducto);

                return Ok(listaproductoDtoList);
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
        public IActionResult Post([FromBody] UsuarioPostDto UsuarioPostDTO)
        {
            // por si el DTO es null
            if (UsuarioPostDTO == null || !ModelState.IsValid) { return BadRequest(ModelState); }

            int IdUsuario = _RepositoryUsuario.GetUsuarioIds(UsuarioPostDTO.Email);
            if (_RepositoryUsuario.UsuarioExist(IdUsuario))
            {
                return StatusCode(666, "Usuario ya existe");
            }
            CarritoPostDto carritoPostDTO = new CarritoPostDto();
            ListaPostDto listaPostDTO = new ListaPostDto();

            var Usuario = _mapper.Map<Usuario>(UsuarioPostDTO);
            var Carrito = _mapper.Map<Carrito>(carritoPostDTO);
            var Lista = _mapper.Map<ListaDeseo>(listaPostDTO);
            Usuario.ContraseñaHash = _RepositoryUsuario.HashPassword(Usuario.Contraseña);

            
          

            if (!_RepositoryUsuario.CreateUsuario(Usuario))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            else
            {
                var UltimoUsuario = _RepositoryUsuario.GetUltimoUsuarioAgregado();
                Carrito.IdUsuario = UltimoUsuario.IdUsuario;
                Lista.IdUsuario = UltimoUsuario.IdUsuario;

                if (!_RepositoryCarrito.CreateCarrito(Carrito))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                } else
                { 
                    if (!_RepositoryLista.CreateLista(Lista))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }
                    else
                    {
                        return Ok();
                    }
                    
                }
            }

        }


        [HttpPost("/CarritoProducto")]
        [Consumes("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult PostCarrito([FromBody] CarritoProductoPostDto CarritoProductoPostDTO)
        {
            // por si el DTO es null
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

          

            
            var userCarritoProducto = _RepositoryCarrito.GetCarrito(userId);
            if (userCarritoProducto == null)
            {
                return NotFound();
            }
            var productoCarritoProducto = _RepositoryProducto.GetProductos(CarritoProductoPostDTO.IdProducto);
            if (productoCarritoProducto == null)
            {
                return NotFound();
            }
            var allCarritoProducto = _RepositoryCarritoProducto.GetCarritosProductos(userCarritoProducto.IdCarrito, productoCarritoProducto.IdProducto);
            int cantidad = Convert.ToInt32(CarritoProductoPostDTO.Cantidad);

            if (_RepositoryCarritoProducto.CarritoProductoExist(userCarritoProducto.IdCarrito, productoCarritoProducto.IdProducto))
            {
                if (allCarritoProducto.Cantidad != cantidad) { 
                    ActualizarCantidad(productoCarritoProducto.IdProducto, cantidad);
                    return Ok();
                }
                else { return StatusCode(666, "Ya esta añadido"); }
                
            }

            //   var CarritoProducto = _RepositoryCarritoProducto.GetCarritoProducto(userCarritoProducto.IdCarrito);
            var carritoProducto = _mapper.Map<CarritoProducto>(CarritoProductoPostDTO);
            carritoProducto.IdCarrito = userCarritoProducto.IdCarrito;
            var producto = _RepositoryProducto.GetProductos(carritoProducto.IdProducto);
            carritoProducto.Precio = producto.Precio;

            if (!_RepositoryCarritoProducto.CreateCarritoProducto(carritoProducto))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            else
            {
                return Ok();
            }
        }

        [HttpPost("/ListaProducto")]
        [Consumes("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult PostLista([FromBody] ListaProductoPostDto ListaProductoPostDTO)
        {
            // por si el DTO es null
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


            var userListaProducto = _RepositoryLista.GetLista(userId);
            if (userListaProducto == null)
            {
                return NotFound();
            }
            var productoListaProducto = _RepositoryProducto.GetProductos(ListaProductoPostDTO.IdProducto);
            if (productoListaProducto == null)
            {
                return NotFound();
            }

            if (_RepositoryListaProducto.ListaProductoExist(userListaProducto.IdLista, productoListaProducto.IdProducto))
            {
                return StatusCode(666, "Ya esta añadido");
            }

            
            var listaProducto = _mapper.Map<ListaProducto>(ListaProductoPostDTO);
            listaProducto.IDListaProducto = userListaProducto.IdLista;

            if (!_RepositoryListaProducto.CreateListaProducto(listaProducto))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            else
            {
                return Ok();
            }
        }


        [HttpPost("/Comentarios")]
        [Consumes("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult PostComentarios([FromBody] ReseñaPostDto ReseñaPostDTO)
        {
            // por si el DTO es null
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

            
            var user = _RepositoryUsuario.GetUsuario(userId);
            if (user == null)
            {
                return NotFound("Usuario no encontrado");
            }

            var producto = _RepositoryProducto.GetProductos(Convert.ToInt32(ReseñaPostDTO.IdProducto));
            if (producto == null)
            {
                return NotFound("Producto no encontrado");
            }

            /*
            if (_RepositoryListaProducto.ReseñaExist(userListaProducto.IdLista, productoListaProducto.IdProducto))
            {
                return StatusCode(666, "Ya esta añadido");
            }
            */

            var Reseña = _mapper.Map<Reseña>(ReseñaPostDTO);
            Reseña.IdUsuario = userId;

            if (!_RepositoryReseña.CreateReseña(Reseña))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            else
            {
                return Ok();
            }
        }

        [HttpDelete("/ListaProducto/{idProducto}")]
        [Consumes("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteLista(int idProducto)
        {
            // por si el DTO es null
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


            var userListaProducto = _RepositoryLista.GetLista(userId);
            if (userListaProducto == null)
            {
                return NotFound();
            }

            var ListaProducto = _RepositoryListaProducto.GetListasProductos(userListaProducto.IdLista, idProducto);



            //var listaProducto = _mapper.Map<ListaProducto>(ListaProducto);

            if (!_RepositoryListaProducto.DeleteListaProducto(ListaProducto))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            else
            {
                return Ok();
            }
        }

        [HttpDelete("/CarritoProducto/{idProducto}")]
        [Consumes("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteCarrito(int idProducto)
        {
            // por si el DTO es null
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

            var userCarritoProducto = _RepositoryCarrito.GetCarrito(userId);
            if (userCarritoProducto == null)
            {
                return NotFound();
            }

            var CarritoProducto = _RepositoryCarritoProducto.GetCarritosProductos(userCarritoProducto.IdCarrito, idProducto);


            if (!_RepositoryCarritoProducto.DeleteCarritoProducto(CarritoProducto))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            else
            {
                return Ok();
            }
        }

        [HttpPost("/Comprar")]
        [Consumes("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult PostRecibo([FromBody] ReciboPostDto ReciboPostDTO)
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
            
            var carrito = _RepositoryCarrito.GetCarrito(userId);
            if (carrito == null)
            {
                return NotFound();
            }
           

            var allCarritoProductos = _RepositoryCarritoProducto.GetCarritoProducto(carrito.IdCarrito);
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

            if (!_RepositoryRecibo.CreateRecibo(reciboProducto))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            else
            {
                foreach (var allCarritoProducto in allCarritoProductos)
                {
                    PatchProductoPorId(allCarritoProducto.IdProducto, Convert.ToInt32(allCarritoProducto.Cantidad));
                    DeleteCarrito(allCarritoProducto.IdProducto);
                }
                return Ok();
            }
        }

        [HttpPatch("/Producto/{idProducto}")]
        [ProducesResponseType(200, Type = typeof(Producto))]
        [ProducesResponseType(404)]
        public IActionResult PatchProductoPorId(int idProducto, int cantidad)
        {
            try
            {
                var producto = _RepositoryProducto.GetProductos(idProducto);
                if (producto == null)
                {
                    return NotFound("Producto no encontrado"); // Producto no encontrado
                }

                var Producto = _mapper.Map<Producto>(producto);
                Producto.Stock = producto.Stock - cantidad;
                if (!_RepositoryProducto.UpdateProducto(Producto))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                } else { return Ok(); }
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
        public IActionResult ActualizarCantidad(int idProducto, int cantidad)
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

                var allCarritoProducto = _RepositoryCarritoProducto.GetCarritoProducto();

                // Filtra los productos del carrito para el usuario autenticado

                var userCarritoProducto = _RepositoryCarrito.GetCarrito(userId);
                if (userCarritoProducto == null)
                {
                    return NotFound();
                }

                //   var CarritoProducto = _RepositoryCarritoProducto.GetCarritoProducto(userCarritoProducto.IdCarrito);
                var CarritoProducto = _RepositoryCarritoProducto.GetCarritosProductos(userCarritoProducto.IdCarrito, idProducto);

                var CarritoProductoList = _mapper.Map<CarritoProducto>(CarritoProducto);
                CarritoProductoList.Cantidad = cantidad; 
                if (!_RepositoryCarritoProducto.UpdateCarritoProducto(CarritoProductoList))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
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
