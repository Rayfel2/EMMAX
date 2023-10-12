using Microsoft.AspNetCore.Mvc;
using ProyectoCore.Models;
using ProyectoCore.Models.ViewModels;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace ProyectoCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly TiendaPruebaContext _TiendaPruebaContext;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public HomeController(TiendaPruebaContext context)
        {
            _TiendaPruebaContext = context;
        }



        public IActionResult Index()
        {
            log.Info("se inicio el index");
            List<Producto> lista = _TiendaPruebaContext.Productos.Include(c => c.oCategorium).ToList(); // poner todos los productos en una lista, inclutendo categoria
            return View(lista);
        }



        [HttpGet]
        public IActionResult Producto_Detalle(int idProducto)
        {
            try
            {
                log.Info("se inicio el get de producto detalle");
                ProductoVM oProductoVM = new ProductoVM()
                {
                    oProducto = new Producto(),
                    oListaCategoria = _TiendaPruebaContext.Categoria.Select(categoria => new SelectListItem()
                    {
                        Text = categoria.Nombre,
                        Value = categoria.IdCategoria.ToString()
                    }).ToList(),

                };

                if (idProducto != 0)
                {
                    oProductoVM.oProducto = _TiendaPruebaContext.Productos.Find(idProducto);

                }

                return View(oProductoVM);
            }
            catch (Exception ex) 
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al traer los productos " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpPost]
        public IActionResult Producto_Detalle(ProductoVM oProductoVM)
        {
            try
            {
                log.Info("se inicio el post de producto detalle");
                if (oProductoVM.oProducto.IdProducto == 0)
                {
                    _TiendaPruebaContext.Productos.Add(oProductoVM.oProducto);
                }
                else
                {
                    _TiendaPruebaContext.Productos.Update(oProductoVM.oProducto);
                }


                _TiendaPruebaContext.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al guardar los productos " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpGet]
        public IActionResult Eliminar(int idProducto)
        {
            try
            {
                log.Info("se inicio el get para eliminar el producto");
                Producto oProducto = _TiendaPruebaContext.Productos.Include(c => c.oCategorium).Where(e => e.IdProducto == idProducto).FirstOrDefault();
                log.Info("guardado con exito");
                return View(oProducto);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al eliminar los productos " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpPost]
        public IActionResult Eliminar(Producto oProducto)
        {
            try
            {
                _TiendaPruebaContext.Productos.Remove(oProducto);
                _TiendaPruebaContext.SaveChanges();


                return View(oProducto);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al eliminar los productos  " + ex.Message);
                return BadRequest(ModelState);

            }
        }


        // categoria ---------------------------------------------------------------------------------

        public IActionResult IndexCategoria()
        {
            try
            {
                List<Categorium> lista = _TiendaPruebaContext.Categoria.ToList(); // poner todas las categorias en una lista, sin incluir otro atributo
                return View(lista);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el index de Categoria  " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpGet]
        public IActionResult Categoria_Detalle(int idCategoria)
        {
            try
            {
                CategoriaVM oCategoriaVM = new CategoriaVM()
                {
                    oCategoria = new Categorium()

                };

                if (idCategoria != 0)
                {
                    oCategoriaVM.oCategoria = _TiendaPruebaContext.Categoria.Find(idCategoria);

                    if (oCategoriaVM.oCategoria == null) // if para validar que esxista             
                    {
                        return NotFound(); // Devuelve una respuesta 404 si la categoría no se encuentra.
                    }

                }

                return View(oCategoriaVM);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el get de categoria  " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpPost]
        public IActionResult Categoria_Detalle(CategoriaVM oCategoriaVM)
        {
            try
            {
                if (oCategoriaVM.oCategoria.IdCategoria == 0)
                {
                    _TiendaPruebaContext.Categoria.Add(oCategoriaVM.oCategoria);
                }
                else
                {
                    _TiendaPruebaContext.Categoria.Update(oCategoriaVM.oCategoria);
                }


                _TiendaPruebaContext.SaveChanges();

                return RedirectToAction("IndexCategoria", "Home");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el post de categoria  " + ex.Message);
                return BadRequest(ModelState);

            }

        }

        [HttpGet]
        public IActionResult Eliminar_Categoria(int idCategoria)
        {
            try
            {
                Categorium oCategoria = _TiendaPruebaContext.Categoria.Where(e => e.IdCategoria == idCategoria).FirstOrDefault();

                if (oCategoria == null)
                {
                    return NotFound(); // Devuelve una respuesta 404 si la categoría no se encuentra.
                }

                return View(oCategoria);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al eliminar categoria  " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpPost]
        public IActionResult Eliminar_Categoria(Categorium oCategoria)
        {
            try
            {
                _TiendaPruebaContext.Categoria.Remove(oCategoria);
                _TiendaPruebaContext.SaveChanges();


                return View(oCategoria);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al eliminar la categoria  " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        //resena--------------------------------------------------------------------------------

        public IActionResult IndexResena()
        {
            try
            {
                List<Reseña> lista = _TiendaPruebaContext.Reseñas.Include(c => c.oUsuario).Include(c => c.oProducto).ToList(); // poner todas las resenas en una lista, incluyendo usuario y producto
                return View(lista);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el index de resena  " + ex.Message);
                return BadRequest(ModelState);

            }
        }



        [HttpGet]
        public IActionResult Resena_Detalle(int idResena)
        {
            try
            {
                ResenaVM oResenaVM = new ResenaVM()
                {
                    oResena = new Reseña(),
                    oListaUsuario = _TiendaPruebaContext.Usuarios.Select(Usuario => new SelectListItem()
                    {
                        Text = Usuario.NombreUsuario,
                        Value = Usuario.IdUsuario.ToString()
                    }).ToList(),

                    oListaProducto = _TiendaPruebaContext.Productos.Select(Producto => new SelectListItem()
                    {
                        Text = Producto.Nombre,
                        Value = Producto.IdProducto.ToString()
                    }).ToList(),

                };

                if (idResena != 0)
                {
                    oResenaVM.oResena = _TiendaPruebaContext.Reseñas.Find(idResena);

                }

                return View(oResenaVM);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el get de resena " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpPost]
        public IActionResult Resena_Detalle(ResenaVM oResenaVM)
        {
            try
            {
                if (oResenaVM.oResena.IdReseña == 0)
                {
                    _TiendaPruebaContext.Reseñas.Add(oResenaVM.oResena);
                }
                else
                {
                    _TiendaPruebaContext.Reseñas.Update(oResenaVM.oResena);
                }


                _TiendaPruebaContext.SaveChanges();

                return RedirectToAction("IndexResena", "Home");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el post de resena detalle  " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpGet]
        public IActionResult Eliminar_Resena(int idResena)
        {
            try
            {
                Reseña oResena = _TiendaPruebaContext.Reseñas.Include(c => c.oUsuario).Include(c => c.oProducto).Where(e => e.IdReseña == idResena).FirstOrDefault();

                return View(oResena);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al eliminar la resena  " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpPost]
        public IActionResult Eliminar_Resena(Reseña oReseña)
        {
            try
            {
                _TiendaPruebaContext.Reseñas.Remove(oReseña);
                _TiendaPruebaContext.SaveChanges();


                return View(oReseña);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al eliminar la resena  " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        //MetodoPago----------------------------------------------------------------------

        public IActionResult IndexMetodoPago()
        {
            try
            {
                List<MetodoPago> lista = _TiendaPruebaContext.MetodoPagos.ToList();
                return View(lista);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el index metodo de pago " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpGet]
        public IActionResult MetodoPago_Detalle(int idMetodoPago)
        {
            try
            {
                MetodoPagoVM oMetodoPagoVM = new MetodoPagoVM()
                {
                    oMetodoPago = new MetodoPago()

                };

                if (idMetodoPago != 0)
                {
                    oMetodoPagoVM.oMetodoPago = _TiendaPruebaContext.MetodoPagos.Find(idMetodoPago);

                }

                return View(oMetodoPagoVM);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el get metodo pago  " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpPost]
        public IActionResult MetodoPago_Detalle(MetodoPagoVM oMetodoPagoVM)
        {
            try
            {
                if (oMetodoPagoVM.oMetodoPago.IdMetodo == 0)
                {
                    _TiendaPruebaContext.MetodoPagos.Add(oMetodoPagoVM.oMetodoPago);
                }
                else
                {
                    _TiendaPruebaContext.MetodoPagos.Update(oMetodoPagoVM.oMetodoPago);
                }


                _TiendaPruebaContext.SaveChanges();

                return RedirectToAction("IndexMetodoPago", "Home");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el post metodo de pago  " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpGet]
        public IActionResult Eliminar_MetodoPago(int idMetodoPago)
        {
            try
            {
                MetodoPago oMetodoPago = _TiendaPruebaContext.MetodoPagos.Where(e => e.IdMetodo == idMetodoPago).FirstOrDefault();

                return View(oMetodoPago);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al eliminar metodopago " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpPost]
        public IActionResult Eliminar_MetodoPago(MetodoPago oMetodoPago)
        {
            try
            {
                _TiendaPruebaContext.MetodoPagos.Remove(oMetodoPago);
                _TiendaPruebaContext.SaveChanges();


                return View(oMetodoPago);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al eliminar el metodo de pago " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        //recibo---------------------------------------------------------------------------------------
        public IActionResult IndexRecibo()
        {
            try
            {
                List<Recibo> lista = _TiendaPruebaContext.Recibos.Include(c => c.oCarrito).Include(c => c.oMetodoPago).ToList(); // poner todas las resenas en una lista, incluyendo usuario y producto
                return View(lista);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en le index de recibo " + ex.Message);
                return BadRequest(ModelState);

            }
        }





        [HttpGet]
        public IActionResult Recibo_Detalle(int idRecibo)
        {
            try
            {
                ReciboVM oReciboVM = new ReciboVM()
                {
                    oRecibo = new Recibo(),
                    oListaCarrito = _TiendaPruebaContext.Carritos.Select(Carrito => new SelectListItem()
                    {
                        Text = Carrito.IdCarrito.ToString(),
                        Value = Carrito.IdCarrito.ToString()
                    }).ToList(),

                    oListaMetodoPago = _TiendaPruebaContext.MetodoPagos.Select(MetodoPago => new SelectListItem()
                    {
                        Text = MetodoPago.TipoMetodo,
                        Value = MetodoPago.IdMetodo.ToString()
                    }).ToList(),

                };

                if (idRecibo != 0)
                {
                    oReciboVM.oRecibo = _TiendaPruebaContext.Recibos.Find(idRecibo);

                }

                return View(oReciboVM);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el get de recibo  " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpPost]
        public IActionResult Recibo_Detalle(ReciboVM oReciboVM)
        {
            try
            {
                if (oReciboVM.oRecibo.IdRecibo == 0)
                {
                    _TiendaPruebaContext.Recibos.Add(oReciboVM.oRecibo);
                }
                else
                {
                    _TiendaPruebaContext.Recibos.Update(oReciboVM.oRecibo);
                }


                _TiendaPruebaContext.SaveChanges();

                return RedirectToAction("IndexRecibo", "Home");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el post de recibo " + ex.Message);
                return BadRequest(ModelState);

            }
        }





        [HttpGet]
        public IActionResult Eliminar_Recibo(int idRecibo)
        {
            try
            {
                Recibo oRecibo = _TiendaPruebaContext.Recibos.Include(c => c.oCarrito).Include(c => c.oMetodoPago).Where(e => e.IdRecibo == idRecibo).FirstOrDefault();

                return View(oRecibo);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al eliminar el recibo  " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpPost]
        public IActionResult Eliminar_Recibo(Recibo oRecibo)
        {
            try
            {
                _TiendaPruebaContext.Recibos.Remove(oRecibo);
                _TiendaPruebaContext.SaveChanges();


                return View(oRecibo);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al eliminar el recibo " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        //Role----------------------------------------------------------------------------------------------------

        public IActionResult IndexRole()
        {
            try
            {
                List<Role> lista = _TiendaPruebaContext.Roles.ToList(); // poner todas las categorias en una lista, sin incluir otro atributo
                return View(lista);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el index de rol " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpGet]
        public IActionResult Role_Detalle(int idRole)
        {
            try
            {
                RoleVM oRoleVM = new RoleVM()
                {
                    oRole = new Role()

                };

                if (idRole != 0)
                {
                    oRoleVM.oRole = _TiendaPruebaContext.Roles.Find(idRole);

                }

                return View(oRoleVM);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el get de rol " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpPost]
        public IActionResult Role_Detalle(RoleVM oRoleVM)
        {
            try
            {
                if (oRoleVM.oRole.IdRoles == 0)
                {
                    _TiendaPruebaContext.Roles.Add(oRoleVM.oRole);
                }
                else
                {
                    _TiendaPruebaContext.Roles.Update(oRoleVM.oRole);
                }


                _TiendaPruebaContext.SaveChanges();

                return RedirectToAction("IndexRole", "Home");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el post de rol " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpGet]
        public IActionResult Eliminar_Role(int idRole)
        {
            try
            {
                Role oRole = _TiendaPruebaContext.Roles.Where(e => e.IdRoles == idRole).FirstOrDefault();

                return View(oRole);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al eliminar rol " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpPost]
        public IActionResult Eliminar_Role(Role oRole)
        {
            try
            {
                _TiendaPruebaContext.Roles.Remove(oRole);
                _TiendaPruebaContext.SaveChanges();


                return View(oRole);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el eliminar rol " + ex.Message);
                return BadRequest(ModelState);

            }
        }


        //usuario----------------------------------------------------------------------------------

        public IActionResult IndexUsuario()
        {
            try
            {
                List<Usuario> lista = _TiendaPruebaContext.Usuarios.ToList(); // poner todas las categorias en una lista, sin incluir otro atributo
                return View(lista);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el index usuario " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpGet]
        public IActionResult Usuario_Detalle(int idUsuario)
        {
            try
            {
                UsuarioVM oUsuarioVM = new UsuarioVM()
                {
                    oUsuario = new Usuario()

                };

                if (idUsuario != 0)
                {
                    oUsuarioVM.oUsuario = _TiendaPruebaContext.Usuarios.Find(idUsuario);

                }

                return View(oUsuarioVM);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el get de usuario " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpPost]
        public IActionResult Usuario_Detalle(UsuarioVM oUsuarioVM)
        {
            try
            {
                if (oUsuarioVM.oUsuario.IdUsuario == 0)
                {
                    // Generar un hash de contraseña
                    string contrasena = oUsuarioVM.oUsuario.Contraseña; // Obtén la contraseña sin hash
                    string contrasenaHash = HashPassword(contrasena); // Genera el hash de la contraseña

                    // Asignar el hash de la contraseña al usuario
                    oUsuarioVM.oUsuario.ContraseñaHash = contrasenaHash;

                    _TiendaPruebaContext.Usuarios.Add(oUsuarioVM.oUsuario);
                    Carrito NuevoCarrito = new Carrito(); // creamos un nuevo carrito
                    NuevoCarrito.oUsuario = oUsuarioVM.oUsuario; // al atributo usuario de la tabla carrito, le colocamos el usuario
                    _TiendaPruebaContext.Carritos.Add(NuevoCarrito); // se agrega el carro a la base de datos

                    ListaDeseo listaDeseoNueva = new ListaDeseo(); // creamos una nueva lista
                    listaDeseoNueva.oUsuario = oUsuarioVM.oUsuario; // al atributo usuario de la tabla lista, le colocamos el usuario
                    _TiendaPruebaContext.ListaDeseos.Add(listaDeseoNueva); // se agrega la lista a la base de datos
                                                                           //---------------------------------
                    _TiendaPruebaContext.SaveChanges(); // Guardar cambios para obtener el ID del usuario asignado

                    // Crear un nuevo registro en la tabla RolesUsuario para asignar el rol con ID 7
                    RolesUsuario nuevoRolUsuario = new RolesUsuario
                    {
                        IdUsuario = oUsuarioVM.oUsuario.IdUsuario, // Obtener el ID del usuario recién creado
                        IdRoles = 7 // ID del rol que deseas asignar
                    };

                    _TiendaPruebaContext.RolesUsuario.Add(nuevoRolUsuario);
                    _TiendaPruebaContext.SaveChanges();



                    //_TiendaPruebaContext.Carritos.Add(oUsuarioVM.oUsuario.Carritos);
                }
                else
                {
                    _TiendaPruebaContext.Usuarios.Update(oUsuarioVM.oUsuario);
                }


                _TiendaPruebaContext.SaveChanges();

                return RedirectToAction("IndexUsuario", "Home");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el post de usuario  " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        private string HashPassword(string password) // metodo para generar contrasena hash
        {

            // Genera un salt aleatorio
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Configura el número de iteraciones y el tamaño de hash
            int iterations = 10000;
            int hashSize = 256 / 8;

            // Genera el hash de la contraseña usando bcrypt
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: iterations,
                numBytesRequested: hashSize));

            // Devuelve el hash de la contraseña
            return hashed;
        }

        [HttpGet]
        public IActionResult Eliminar_Usuario(int idUsuario)
        {
            try
            {
                Usuario oUsuario = _TiendaPruebaContext.Usuarios.Where(e => e.IdUsuario == idUsuario).FirstOrDefault();

                return View(oUsuario);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el eliminar usuario" + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpPost]
        public IActionResult Eliminar_Usuario(Usuario oUsuario)
        {
            try
            {
                _TiendaPruebaContext.Usuarios.Remove(oUsuario);
                _TiendaPruebaContext.SaveChanges();


                return View(oUsuario);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al eliminar el usuario  " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        //CarritoProducto-----------------------------------------------------------------------------------------

        public IActionResult IndexCarritoProducto()
        {
            try
            {
                List<CarritoProducto> lista = _TiendaPruebaContext.CarritoProductos.Include(c => c.oCarrito).Include(c => c.oProducto).ToList(); // poner todas las resenas en una lista, incluyendo usuario y producto
                return View(lista);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el index de carito producto " + ex.Message);
                return BadRequest(ModelState);

            }
        }



        [HttpGet]
        public IActionResult CarritoProducto_Detalle(int idCarrito, int idProducto)
        {
            try
            {
                CarritoProductoVM oCarritoProductoVM = new CarritoProductoVM()
                {
                    oCarritoProducto = new CarritoProducto(),
                    oListaCarrito = _TiendaPruebaContext.Carritos.Select(Carrito => new SelectListItem()
                    {
                        Text = Carrito.IdCarrito.ToString(),
                        Value = Carrito.IdCarrito.ToString()
                    }).ToList(),

                    oListaProducto = _TiendaPruebaContext.Productos.Select(Producto => new SelectListItem()
                    {
                        Text = Producto.IdProducto.ToString(),
                        Value = Producto.IdProducto.ToString()
                    }).ToList(),

                };
                var existingCarritoProducto = _TiendaPruebaContext.CarritoProductos
            .FirstOrDefault(cp => cp.IdCarrito == idCarrito
                                && cp.IdProducto == idProducto);
                ViewBag.ExistingCarritoProducto = existingCarritoProducto;
                if (idCarrito != 0 && idProducto != 0)
                {

                    oCarritoProductoVM.oCarritoProducto = _TiendaPruebaContext.CarritoProductos.Find(idCarrito, idProducto);

                }

                return View(oCarritoProductoVM);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el get de carrito producto " + ex.Message);
                return BadRequest(ModelState);


            }
        }


        [HttpPost]
        public IActionResult CarritoProducto_Detalle(CarritoProductoVM oCarritoProductoVM)
        {
            try
            {
                var carrito = oCarritoProductoVM.oCarritoProducto.IdCarrito;
                var producto = oCarritoProductoVM.oCarritoProducto.IdProducto;

                // Obtener el producto seleccionado desde la base de datos
                var selectedProduct = _TiendaPruebaContext.Productos.FirstOrDefault(p => p.IdProducto == producto);

                if (selectedProduct != null)
                {
                    // Establecer el precio del CarritoProducto igual al precio del Producto
                    oCarritoProductoVM.oCarritoProducto.Precio = selectedProduct.Precio;

                    // Comprobar si el CarritoProducto ya existe en la base de datos
                    var existingCarritoProducto = _TiendaPruebaContext.CarritoProductos
                        .FirstOrDefault(cp => cp.IdCarrito == carrito && cp.IdProducto == producto);

                    if (existingCarritoProducto == null)
                    {
                        // Si no existe, agregar el nuevo CarritoProducto
                        if (oCarritoProductoVM.oCarritoProducto.Cantidad > selectedProduct.Stock)
                        {
                            // Lanzar una excepción
                            return NotFound("la cantidad no puede ser mayor al stock");
                            //throw new Exception("La cantidad no puede ser mayor que el stock");
                        }

                        _TiendaPruebaContext.CarritoProductos.Add(oCarritoProductoVM.oCarritoProducto);
                    }
                    else
                    {
                        // Si existe, actualizar el CarritoProducto existente con el nuevo precio
                        existingCarritoProducto.Precio = oCarritoProductoVM.oCarritoProducto.Precio;
                        existingCarritoProducto.Cantidad = oCarritoProductoVM.oCarritoProducto.Cantidad;

                        // Actualizar otras propiedades según sea necesario
                        _TiendaPruebaContext.CarritoProductos.Update(existingCarritoProducto);
                    }

                    // Guardar cambios en la base de datos
                    _TiendaPruebaContext.SaveChanges();
                }

                return RedirectToAction("IndexCarritoProducto", "Home");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el post de CarritoProducto " + ex.Message);
                return BadRequest(ModelState);

            }
        }


        [HttpGet]
        public IActionResult Eliminar_CarritoProducto(int idCarrito, int idProducto)
        {
            try
            {


                CarritoProducto oCarritoProducto = _TiendaPruebaContext.CarritoProductos.Include(c => c.oCarrito).Include(c => c.oProducto).Where(e => e.IdCarrito == idCarrito && e.IdProducto == idProducto).FirstOrDefault();

                return View(oCarritoProducto);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al eliminar los CarritoProducto  " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpPost]
        public IActionResult Eliminar_CarritoProducto(CarritoProducto oCarritoProducto)
        {
            try
            {
                _TiendaPruebaContext.CarritoProductos.Remove(oCarritoProducto);
                _TiendaPruebaContext.SaveChanges();


                return View(oCarritoProducto);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al eliminar el CarritoProducto " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        //Carrito----------------------------------------------------------------------------------------------

        public IActionResult IndexCarrito()
        {
            try
            {
                List<Carrito> lista = _TiendaPruebaContext.Carritos.Include(c => c.oUsuario).ToList(); // poner todos los productos en una lista, inclutendo categoria
                return View(lista);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el index de carrito " + ex.Message);
                return BadRequest(ModelState);

            }
        }



        [HttpGet]
        public IActionResult Carrito_Detalle(int idCarrito)
        {
            try
            {
                CarritoVM oCarritoVM = new CarritoVM()
                {
                    oCarrito = new Carrito(),
                    oListaUsuario = _TiendaPruebaContext.Usuarios.Select(Usuario => new SelectListItem()
                    {
                        Text = Usuario.NombreUsuario,
                        Value = Usuario.IdUsuario.ToString()
                    }).ToList(),

                };

                if (idCarrito != 0)
                {
                    oCarritoVM.oCarrito = _TiendaPruebaContext.Carritos.Find(idCarrito);

                }

                return View(oCarritoVM);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el get de carrito " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpPost]
        public IActionResult Carrito_Detalle(CarritoVM oCarritoVM)
        {
            try
            {
                if (oCarritoVM.oCarrito.IdCarrito == 0)
                {
                    _TiendaPruebaContext.Carritos.Add(oCarritoVM.oCarrito);
                }
                else
                {
                    _TiendaPruebaContext.Carritos.Update(oCarritoVM.oCarrito);
                }


                _TiendaPruebaContext.SaveChanges();

                return RedirectToAction("IndexCarrito", "Home");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el post de carrito " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpGet]
        public IActionResult Eliminar_Carrito(int idCarrito)
        {
            try
            {
                Carrito oCarrito = _TiendaPruebaContext.Carritos.Include(c => c.oUsuario).Where(e => e.IdCarrito == idCarrito).FirstOrDefault();

                return View(oCarrito);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al eliminar el carrito " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpPost]
        public IActionResult Eliminar_Carrito(Carrito oCarrito)
        {
            try
            {
                _TiendaPruebaContext.Carritos.Remove(oCarrito);
                _TiendaPruebaContext.SaveChanges();


                return View(oCarrito);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al eliminar el carrito  " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        //ListaDeseo---------------------------------------------------------------------------------------------------------------------------------

        public IActionResult IndexListaDeseo()
        {
            try
            {
                List<ListaDeseo> lista = _TiendaPruebaContext.ListaDeseos.Include(c => c.oUsuario).ToList(); // poner todos los productos en una lista, inclutendo categoria
                return View(lista);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el index de listadeseo " + ex.Message);
                return BadRequest(ModelState);

            }
        }



        [HttpGet]
        public IActionResult ListaDeseo_Detalle(int idListaDeseo)
        {
            try
            {
                ListaDeseoVM oListaDeseoVM = new ListaDeseoVM()
                {
                    oListaDeseo = new ListaDeseo(),
                    oListaUsuario = _TiendaPruebaContext.Usuarios.Select(Usuario => new SelectListItem()
                    {
                        Text = Usuario.NombreUsuario,
                        Value = Usuario.IdUsuario.ToString()
                    }).ToList(),

                };

                if (idListaDeseo != 0)
                {
                    oListaDeseoVM.oListaDeseo = _TiendaPruebaContext.ListaDeseos.Find(idListaDeseo);

                }

                return View(oListaDeseoVM);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el get de listadeseo " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpPost]
        public IActionResult ListaDeseo_Detalle(ListaDeseoVM oListaDeseoVM)
        {
            try
            {
                if (oListaDeseoVM.oListaDeseo.IdLista == 0)
                {
                    _TiendaPruebaContext.ListaDeseos.Add(oListaDeseoVM.oListaDeseo);
                }
                else
                {
                    _TiendaPruebaContext.ListaDeseos.Update(oListaDeseoVM.oListaDeseo);
                }


                _TiendaPruebaContext.SaveChanges();

                return RedirectToAction("IndexListaDeseo", "Home");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el post de lista deseo  " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpGet]
        public IActionResult Eliminar_ListaDeseo(int idListaDeseo)
        {
            try
            {
                ListaDeseo oListaDeseo = _TiendaPruebaContext.ListaDeseos.Include(c => c.oUsuario).Where(e => e.IdLista == idListaDeseo).FirstOrDefault();

                return View(oListaDeseo);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al eliminar la listadeseo  " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpPost]
        public IActionResult Eliminar_ListaDeseo(ListaDeseo oListaDeseo)
        {
            try
            {
                _TiendaPruebaContext.ListaDeseos.Remove(oListaDeseo);
                _TiendaPruebaContext.SaveChanges();


                return View(oListaDeseo);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al eliminar la lista deseo  " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        //ListaProductoControllers--------------------------------------------------------------------------------
        public IActionResult IndexListaProducto()
        {
            try
            {
                List<ListaProducto> lista = _TiendaPruebaContext.ListaProducto.Include(c => c.oListaDeseo).Include(c => c.oProducto).ToList(); // poner todas las resenas en una lista, incluyendo usuario y producto
                return View(lista);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al eliminar el index ListaProducto  " + ex.Message);
                return BadRequest(ModelState);

            }
        }



        [HttpGet]
        public IActionResult ListaProducto_Detalle(int idListaProducto, int idProducto)
        {
            try
            {
                ListaProductoVM oListaProductoVM = new ListaProductoVM()
                {
                    oListaProducto = new ListaProducto(),
                    oListaDeListas = _TiendaPruebaContext.ListaDeseos.Select(ListaDeseo => new SelectListItem()
                    {
                        Text = ListaDeseo.IdLista.ToString(),
                        Value = ListaDeseo.IdLista.ToString()
                    }).ToList(),

                    oListaProductoProductos = _TiendaPruebaContext.Productos.Select(Producto => new SelectListItem()
                    {
                        Text = Producto.IdProducto.ToString(),
                        Value = Producto.IdProducto.ToString()
                    }).ToList(),

                };
                var existingListaProducto = _TiendaPruebaContext.ListaProducto
            .FirstOrDefault(cp => cp.IDListaProducto == idListaProducto
                                && cp.IdProducto == idProducto);
                ViewBag.ExistingListaProducto = existingListaProducto;
                if (idListaProducto != 0 && idProducto != 0)
                {

                    oListaProductoVM.oListaProducto = _TiendaPruebaContext.ListaProducto.Find(idListaProducto, idProducto);

                }

                return View(oListaProductoVM);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el get de ListaProducto " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpPost]
        public IActionResult ListaProducto_Detalle(ListaProductoVM oListaProductoVM)
        {
            try
            {
                var ListaProducto = oListaProductoVM.oListaProducto.IDListaProducto;
                var producto = oListaProductoVM.oListaProducto.IdProducto;

                var existingListaProducto = _TiendaPruebaContext.ListaProducto
            .FirstOrDefault(cp => cp.IDListaProducto == ListaProducto
                                && cp.IdProducto == producto);

                if (existingListaProducto == null)
                {

                    _TiendaPruebaContext.ListaProducto.Add(oListaProductoVM.oListaProducto);

                }
                else
                {
                    //_TiendaPruebaContext.CarritoProductos.Update(oCarritoProductoVM.oCarritoProducto);
                    // Actualiza otras propiedades según sea necesario
                    _TiendaPruebaContext.ListaProducto.Update(existingListaProducto); // Importante: Actualiza la entidad existente
                    _TiendaPruebaContext.SaveChanges();
                }


                _TiendaPruebaContext.SaveChanges();

                return RedirectToAction("IndexListaProducto", "Home");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el post de ListaProducto  " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpGet]
        public IActionResult Eliminar_ListaProducto(int idListaProducto, int idProducto)
        {
            try
            {


                ListaProducto oListaProducto = _TiendaPruebaContext.ListaProducto.Include(c => c.oListaDeseo).Include(c => c.oProducto).Where(e => e.IDListaProducto == idListaProducto && e.IdProducto == idProducto).FirstOrDefault();

                return View(oListaProducto);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al eliminar la ListaProducto  " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpPost]
        public IActionResult Eliminar_ListaProducto(ListaProducto oListaProducto)
        {
            try
            {
                _TiendaPruebaContext.ListaProducto.Remove(oListaProducto);
                _TiendaPruebaContext.SaveChanges();


                return View(oListaProducto);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al eliminar la ListProducto  " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        //RolesUsuario----------------------------------------------------------------------------------------------------------
        public IActionResult IndexRolesUsuario()
        {
            try
            {
                List<RolesUsuario> lista = _TiendaPruebaContext.RolesUsuario.Include(c => c.oRole).Include(c => c.oUsuario).ToList(); // poner todas las resenas en una lista, incluyendo usuario y producto
                return View(lista);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el index de RolUsuario" + ex.Message);
                return BadRequest(ModelState);

            }
        }



        [HttpGet]
        public IActionResult RolesUsuario_Detalle(int idRoles, int idUsuario)
        {
            try
            {
                RolesUsuarioVM oRolesUsuarioVM = new RolesUsuarioVM()
                {
                    oRolesUsuario = new RolesUsuario(),
                    oListaRoles = _TiendaPruebaContext.Roles.Select(Role => new SelectListItem()
                    {
                        Text = Role.IdRoles.ToString(),
                        Value = Role.IdRoles.ToString()
                    }).ToList(),

                    oListaUsuarios = _TiendaPruebaContext.Usuarios.Select(Usuario => new SelectListItem()
                    {
                        Text = Usuario.IdUsuario.ToString(),
                        Value = Usuario.IdUsuario.ToString()
                    }).ToList(),

                };
                var existingRolesUsuario = _TiendaPruebaContext.RolesUsuario
            .FirstOrDefault(cp => cp.IdRoles == idRoles
                                && cp.IdUsuario == idUsuario);
                ViewBag.existingRolesUsuario = existingRolesUsuario;
                if (idRoles != 0 && idUsuario != 0)
                {

                    oRolesUsuarioVM.oRolesUsuario = _TiendaPruebaContext.RolesUsuario.Find(idRoles, idUsuario);

                }

                return View(oRolesUsuarioVM);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el get de RolUsuario " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpPost]
        public IActionResult RolesUsuario_Detalle(RolesUsuarioVM oRolesUsuarioVM)
        {
            try
            {
                var Roles = oRolesUsuarioVM.oRolesUsuario.IdRoles;
                var Usuario = oRolesUsuarioVM.oRolesUsuario.IdUsuario;

                var existingRolesUsuario = _TiendaPruebaContext.RolesUsuario
            .FirstOrDefault(cp => cp.IdRoles == Roles
                                && cp.IdUsuario == Usuario);

                if (existingRolesUsuario == null)
                {

                    _TiendaPruebaContext.RolesUsuario.Add(oRolesUsuarioVM.oRolesUsuario);

                }
                else
                {
                    //_TiendaPruebaContext.CarritoProductos.Update(oCarritoProductoVM.oCarritoProducto);
                    // Actualiza otras propiedades según sea necesario
                    _TiendaPruebaContext.RolesUsuario.Update(existingRolesUsuario); // Importante: Actualiza la entidad existente
                    _TiendaPruebaContext.SaveChanges();
                }


                _TiendaPruebaContext.SaveChanges();

                return RedirectToAction("IndexRolesUsuario", "Home");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error en el post de RolUsuario " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpGet]
        public IActionResult Eliminar_RolesUsuario(int idRoles, int idUsuario)
        {
            try
            {


                RolesUsuario oRolesUsuario = _TiendaPruebaContext.RolesUsuario.Include(c => c.oRole).Include(c => c.oUsuario).Where(e => e.IdRoles == idRoles && e.IdUsuario == idUsuario).FirstOrDefault();

                return View(oRolesUsuario);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al eliminar el RolUsuario  " + ex.Message);
                return BadRequest(ModelState);

            }
        }

        [HttpPost]
        public IActionResult Eliminar_RolesUsuario(RolesUsuario oRolesUsuario)
        {
            try
            {
                _TiendaPruebaContext.RolesUsuario.Remove(oRolesUsuario);
                _TiendaPruebaContext.SaveChanges();


                return View(oRolesUsuario);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ModelState.AddModelError("", "Ocurrió un error al eliminar el Rol Usuario  " + ex.Message);
                return BadRequest(ModelState);

            }
        }






    }
}