using Microsoft.AspNetCore.Mvc;
using ProyectoCore.Models;
using ProyectoCore.Models.ViewModels;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ProyectoCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly TiendaPruebaContext _TiendaPruebaContext;

        public HomeController(TiendaPruebaContext context)
        {
            _TiendaPruebaContext = context;
        }

       

        public IActionResult Index()
        {
            List<Producto> lista = _TiendaPruebaContext.Productos.Include(c => c.oCategorium).ToList(); // poner todos los productos en una lista, inclutendo categoria
            return View(lista);
        }



        [HttpGet]
        public IActionResult Producto_Detalle(int idProducto)
        {
            ProductoVM oProductoVM = new ProductoVM() { 
                oProducto = new Producto(),
                oListaCategoria = _TiendaPruebaContext.Categoria.Select(categoria => new SelectListItem() {
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

        [HttpPost]
        public IActionResult Producto_Detalle(ProductoVM oProductoVM)
        {
          if(oProductoVM.oProducto.IdProducto == 0)
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

        [HttpGet]
        public IActionResult Eliminar(int idProducto)
        {
          Producto oProducto = _TiendaPruebaContext.Productos.Include(c => c.oCategorium).Where(e => e.IdProducto == idProducto).FirstOrDefault();

            return View(oProducto);
        }

        [HttpPost]
        public IActionResult Eliminar(Producto oProducto)
        {
            _TiendaPruebaContext.Productos.Remove(oProducto);
            _TiendaPruebaContext.SaveChanges();


            return View(oProducto);
        }


        // categoria ---------------------------------------------------------------------------------

        public IActionResult IndexCategoria()
        {
            List<Categorium> lista = _TiendaPruebaContext.Categoria.ToList(); // poner todas las categorias en una lista, sin incluir otro atributo
            return View(lista);
        }

        [HttpGet]
        public IActionResult Categoria_Detalle(int idCategoria)
        {
            CategoriaVM oCategoriaVM = new CategoriaVM()
            {
                oCategoria = new Categorium()

            };

            if (idCategoria != 0)
            {
                oCategoriaVM.oCategoria = _TiendaPruebaContext.Categoria.Find(idCategoria);

            }

            return View(oCategoriaVM);
        }

        [HttpPost]
        public IActionResult Categoria_Detalle(CategoriaVM oCategoriaVM)
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

        [HttpGet]
        public IActionResult Eliminar_Categoria(int idCategoria)
        {
            Categorium oCategoria = _TiendaPruebaContext.Categoria.Where(e => e.IdCategoria == idCategoria).FirstOrDefault();

            return View(oCategoria);
        }

        [HttpPost]
        public IActionResult Eliminar_Categoria(Categorium oCategoria)
        {
            _TiendaPruebaContext.Categoria.Remove(oCategoria);
            _TiendaPruebaContext.SaveChanges();


            return View(oCategoria);
        }

        //resena--------------------------------------------------------------------------------

        public IActionResult IndexResena()
        {
            List<Reseña> lista = _TiendaPruebaContext.Reseñas.Include(c => c.oUsuario).Include(c => c.oProducto).ToList(); // poner todas las resenas en una lista, incluyendo usuario y producto
            return View(lista);
        }



        [HttpGet]
        public IActionResult Resena_Detalle(int idResena)
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

        [HttpPost]
        public IActionResult Resena_Detalle(ResenaVM oResenaVM)
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

        [HttpGet]
        public IActionResult Eliminar_Resena(int idResena)
        {
            Reseña oResena = _TiendaPruebaContext.Reseñas.Include(c => c.oUsuario).Include(c => c.oProducto).Where(e => e.IdReseña == idResena).FirstOrDefault();

            return View(oResena);
        }

        [HttpPost]
        public IActionResult Eliminar_Resena(Reseña oReseña)
        {
            _TiendaPruebaContext.Reseñas.Remove(oReseña);
            _TiendaPruebaContext.SaveChanges();


            return View(oReseña);
        }

        //MetodoPago----------------------------------------------------------------------

        public IActionResult IndexMetodoPago()
        {
            List<MetodoPago> lista = _TiendaPruebaContext.MetodoPagos.ToList(); 
            return View(lista);
        }

        [HttpGet]
        public IActionResult MetodoPago_Detalle(int idMetodoPago)
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

        [HttpPost]
        public IActionResult MetodoPago_Detalle(MetodoPagoVM oMetodoPagoVM)
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

        [HttpGet]
        public IActionResult Eliminar_MetodoPago(int idMetodoPago)
        {
            MetodoPago oMetodoPago = _TiendaPruebaContext.MetodoPagos.Where(e => e.IdMetodo == idMetodoPago).FirstOrDefault();

            return View(oMetodoPago);
        }

        [HttpPost]
        public IActionResult Eliminar_MetodoPago(MetodoPago oMetodoPago)
        {
            _TiendaPruebaContext.MetodoPagos.Remove(oMetodoPago);
            _TiendaPruebaContext.SaveChanges();


            return View(oMetodoPago);
        }

        //recibo---------------------------------------------------------------------------------------
        public IActionResult IndexRecibo()
        {
            List<Recibo> lista = _TiendaPruebaContext.Recibos.Include(c => c.oCarrito).Include(c => c.oMetodoPago).ToList(); // poner todas las resenas en una lista, incluyendo usuario y producto
            return View(lista);
        }



        [HttpGet]
        public IActionResult Recibo_Detalle(int idRecibo)
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

        [HttpPost]
        public IActionResult Recibo_Detalle(ReciboVM oReciboVM)
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

        [HttpGet]
        public IActionResult Eliminar_Recibo(int idRecibo)
        {
            Recibo oRecibo = _TiendaPruebaContext.Recibos.Include(c => c.oCarrito).Include(c => c.oMetodoPago).Where(e => e.IdRecibo == idRecibo).FirstOrDefault();

            return View(oRecibo);
        }

        [HttpPost]
        public IActionResult Eliminar_Recibo(Recibo oRecibo)
        {
            _TiendaPruebaContext.Recibos.Remove(oRecibo);
            _TiendaPruebaContext.SaveChanges();


            return View(oRecibo);
        }

        //Role----------------------------------------------------------------------------------------------------

        public IActionResult IndexRole()
        {
            List<Role> lista = _TiendaPruebaContext.Roles.ToList(); // poner todas las categorias en una lista, sin incluir otro atributo
            return View(lista);
        }

        [HttpGet]
        public IActionResult Role_Detalle(int idRole)
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

        [HttpPost]
        public IActionResult Role_Detalle(RoleVM oRoleVM)
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

        [HttpGet]
        public IActionResult Eliminar_Role(int idRole)
        {
            Role oRole = _TiendaPruebaContext.Roles.Where(e => e.IdRoles == idRole).FirstOrDefault();

            return View(oRole);
        }

        [HttpPost]
        public IActionResult Eliminar_Role(Role oRole)
        {
            _TiendaPruebaContext.Roles.Remove(oRole);
            _TiendaPruebaContext.SaveChanges();


            return View(oRole);
        }


        //usuario----------------------------------------------------------------------------------

        public IActionResult IndexUsuario()
        {
            List<Usuario> lista = _TiendaPruebaContext.Usuarios.ToList(); // poner todas las categorias en una lista, sin incluir otro atributo
            return View(lista);
        }

        [HttpGet]
        public IActionResult Usuario_Detalle(int idUsuario)
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

        [HttpPost]
        public IActionResult Usuario_Detalle(UsuarioVM oUsuarioVM)
        {
            if (oUsuarioVM.oUsuario.IdUsuario == 0)
            {
                _TiendaPruebaContext.Usuarios.Add(oUsuarioVM.oUsuario);
            }
            else
            {
                _TiendaPruebaContext.Usuarios.Update(oUsuarioVM.oUsuario);
            }


            _TiendaPruebaContext.SaveChanges();

            return RedirectToAction("IndexUsuario", "Home");
        }

        [HttpGet]
        public IActionResult Eliminar_Usuario(int idUsuario)
        {
            Usuario oUsuario = _TiendaPruebaContext.Usuarios.Where(e => e.IdUsuario == idUsuario).FirstOrDefault();

            return View(oUsuario);
        }

        [HttpPost]
        public IActionResult Eliminar_Usuario(Usuario oUsuario)
        {
            _TiendaPruebaContext.Usuarios.Remove(oUsuario);
            _TiendaPruebaContext.SaveChanges();


            return View(oUsuario);
        }

        //CarritoProducto-----------------------------------------------------------------------------------------

        public IActionResult IndexCarritoProducto()
        {
            List<CarritoProducto> lista = _TiendaPruebaContext.CarritoProductos.Include(c => c.oCarrito).Include(c => c.oProducto).ToList(); // poner todas las resenas en una lista, incluyendo usuario y producto
            return View(lista);
        }



        [HttpGet]
        public IActionResult CarritoProducto_Detalle(int idCarritoProducto)
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
                    Text = Producto.Nombre,
                    Value = Producto.IdProducto.ToString()
                }).ToList(),

            };

            if (idCarritoProducto != 0)
            {
                oCarritoProductoVM.oCarritoProducto = _TiendaPruebaContext.CarritoProductos.Find(idCarritoProducto);

            }

            return View(oCarritoProductoVM);
        }

        [HttpPost]
        public IActionResult CarritoProducto_Detalle(CarritoProductoVM oCarritoProductoVM)
        {
            if (oCarritoProductoVM.oCarritoProducto.IdCarrito == 0)
            {
                _TiendaPruebaContext.CarritoProductos.Add(oCarritoProductoVM.oCarritoProducto);
            }
            else
            {
                _TiendaPruebaContext.CarritoProductos.Update(oCarritoProductoVM.oCarritoProducto);
            }


            _TiendaPruebaContext.SaveChanges();

            return RedirectToAction("IndexCarritoProducto", "Home");
        }

        [HttpGet]
        public IActionResult Eliminar_CarritoProducto(int idCarritoProducto)
        {
            CarritoProducto oCarritoProducto = _TiendaPruebaContext.CarritoProductos.Include(c => c.oCarrito).Include(c => c.oProducto).Where(e => e.IdCarrito == idCarritoProducto).FirstOrDefault();

            return View(oCarritoProducto);
        }

        [HttpPost]
        public IActionResult Eliminar_CarritoProducto(CarritoProducto oCarritoProducto)
        {
            _TiendaPruebaContext.CarritoProductos.Remove(oCarritoProducto);
            _TiendaPruebaContext.SaveChanges();


            return View(oCarritoProducto);
        }

        //Carrito----------------------------------------------------------------------------------------------

        public IActionResult IndexCarrito()
        {
            List<Carrito> lista = _TiendaPruebaContext.Carritos.Include(c => c.oUsuario).ToList(); // poner todos los productos en una lista, inclutendo categoria
            return View(lista);
        }



        [HttpGet]
        public IActionResult Carrito_Detalle(int idCarrito)
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

        [HttpPost]
        public IActionResult Carrito_Detalle(CarritoVM oCarritoVM)
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

        [HttpGet]
        public IActionResult Eliminar_Carrito(int idCarrito)
        {
            Carrito oCarrito = _TiendaPruebaContext.Carritos.Include(c => c.oUsuario).Where(e => e.IdCarrito == idCarrito).FirstOrDefault();

            return View(oCarrito);
        }

        [HttpPost]
        public IActionResult Eliminar_Carrito(Carrito oCarrito)
        {
            _TiendaPruebaContext.Carritos.Remove(oCarrito);
            _TiendaPruebaContext.SaveChanges();


            return View(oCarrito);
        }

        //ListaDeseo---------------------------------------------------------------------------------------------------------------------------------

        public IActionResult IndexListaDeseo()
        {
            List<ListaDeseo> lista = _TiendaPruebaContext.ListaDeseos.Include(c => c.oUsuario).ToList(); // poner todos los productos en una lista, inclutendo categoria
            return View(lista);
        }



        [HttpGet]
        public IActionResult ListaDeseo_Detalle(int idListaDeseo)
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

        [HttpPost]
        public IActionResult ListaDeseo_Detalle(ListaDeseoVM oListaDeseoVM)
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

        [HttpGet]
        public IActionResult Eliminar_ListaDeseo(int idListaDeseo)
        {
            ListaDeseo oListaDeseo = _TiendaPruebaContext.ListaDeseos.Include(c => c.oUsuario).Where(e => e.IdLista == idListaDeseo).FirstOrDefault();

            return View(oListaDeseo);
        }

        [HttpPost]
        public IActionResult Eliminar_ListaDeseo(ListaDeseo oListaDeseo)
        {
            _TiendaPruebaContext.ListaDeseos.Remove(oListaDeseo);
            _TiendaPruebaContext.SaveChanges();


            return View(oListaDeseo);
        }





    }
}