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
            Categorium oCategoria = _TiendaPruebaContext.Categoria.FirstOrDefault();

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



    }
}