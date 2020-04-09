using ProyectoFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class ProductosController : Controller
    {
        // GET: Productos
        
        public ActionResult Index()
        {
            try
            {
                AlmacenContexto db = new AlmacenContexto();

                return View(db.productos.ToList());
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Error" + ex.Message);
                return View();
            }
        }
        [Authorize(Roles = "Admin, Superior")]
        public ActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(productos p)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                using (AlmacenContexto db = new AlmacenContexto())
                {

                    db.productos.Add(p);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al agregar Producto", ex);

                return View();
            }



        }
        [Authorize(Roles = "Admin, Superior")]
        public ActionResult Editar(int id)
        {
            using (var db = new AlmacenContexto())
            {
                try
                {
                    productos prod = db.productos.Find(id);
                    return View(prod);
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("Error", ex);

                    return View();
                }
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(productos p)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                using (var db = new AlmacenContexto())
                {
                    productos prod = db.productos.Find(p.codigo_producto);
                    prod.nombre_producto = p.nombre_producto;
                    prod.costo = p.costo;
                    prod.descripcion = p.descripcion;
                    prod.fecha_vencimiento = p.fecha_vencimiento;
                    prod.udm = p.udm;
                    prod.costo = p.costo;
                    prod.existencia = p.existencia;
                    prod.estado = p.estado;
                    
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {

                ModelState.AddModelError("Error", ex);

                return View();
            }

        }


        public ActionResult Detalles(int id)
        {
            try
            {
                using (var db = new AlmacenContexto())
                {
                    productos prod = db.productos.Find(id);
                    return View(prod);
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("Error", ex);

                return View();
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                using (var db = new AlmacenContexto())
                {
                    productos prod = db.productos.Find(id);
                    db.productos.Remove(prod);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }
            
        }
    }
}
  