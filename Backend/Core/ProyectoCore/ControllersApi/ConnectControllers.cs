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

namespace ProyectoCore.ControllersApi
{
    [ApiController]
    [Route("[controller]")]
    public class ConnectControllers : Controller
    {

        private readonly IUsuarioRepository _RepositoryUsuario;
        private readonly ICarritoProductoRepository _RepositoryCarritoProducto;
        private readonly string _jwtSecret;
        private readonly IMapper _mapper;

        public ConnectControllers(IUsuarioRepository RepositoryUsuario, ICarritoProductoRepository RepositoryCarritoProducto, IMapper mapper, string jwtSecret)
        {
            _RepositoryUsuario = RepositoryUsuario;
            _RepositoryCarritoProducto = RepositoryCarritoProducto;
            _jwtSecret = jwtSecret;
            _mapper = mapper;
        }

        [HttpPost("/login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto loginRequest)
        {
            try
            {
                // Verificar si el correo electrónico es válido
                var usuario = _RepositoryUsuario.GetUsuarioByEmailAndPassword(loginRequest.Email, loginRequest.Contraseña);

     

                // Generar un token JWT con el ID del usuario como claim
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.Name, usuario.IdUsuario.ToString()) // ID del usuario como claim
                    }),
                    Expires = DateTime.UtcNow.AddHours(1), // Duración del token (1 hora)
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSecret)),
                        SecurityAlgorithms.HmacSha256Signature
                    )
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // Devolver el token JWT en la respuesta
                return Ok(new { Token = tokenString });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al iniciar sesión: " + ex.Message);
                return BadRequest(ModelState);
            }
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

        [Authorize] // Añade este atributo para requerir autenticación
        [HttpGet("/CarritoProducto")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CarritoProducto>))]
        public IActionResult GetCarritoProducto(int page, int pageSize)
        {
            try
            {
                // Obtén el ID del usuario autenticado
                var userId = User.FindFirst(ClaimTypes.Name)?.Value;

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

                // Filtra los productos del carrito para el usuario autenticado

                //var userCarritoProducto = allCarritoProducto.Where(cp => cp.IdUsuario == int.Parse(userId));

                // Aplicamos paginación utilizando LINQ para seleccionar los registros apropiados.
                // A nivel de rutas sería por ejemplo http://localhost:5230/Producto?page=1&pageSize=10

                //var pagedCarritoProducto = userCarritoProducto.Skip(startIndex).Take(pageSize).ToList();

                // Mapeo los elementos del carrito paginados en vez de todos


                //var CarritoProductoDtoList = _mapper.Map<List<CarritoProductoDto>>(pagedCarritoProducto);

                // return Ok(CarritoProductoDtoList);
                return Ok(1);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al obtener los CarritoProducto: " + ex.Message);
                return BadRequest(ModelState);
            }
        }


    }
}
