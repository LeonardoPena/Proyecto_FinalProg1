using ProyectoFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class DespachoController : Controller
    {
        // GET: Despacho
        [Authorize(Roles = "Admin, Recepcion, Superior")]
        public ActionResult Index()
        {
            try
            {
                AlmacenContexto db = new AlmacenContexto();

                return View(db.despacho.ToList());
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
        public ActionResult Agregar(despacho d)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                using (AlmacenContexto db = new AlmacenContexto())
                {
                    productos p = db.productos.Find(d.idproducto);

                    if (d.accion== "Entrada") {

                        p.existencia += d.cant_producto ;
                    
                    }else if (d.accion == "Salida")
                    {
                        if (p.existencia>d.cant_producto)
                        {
                            p.existencia -= d.cant_producto;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }

                    db.despacho.Add(d);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al agregar", ex);

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
                    despacho des = db.despacho.Find(id);
                    return View(des);
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
        public ActionResult Editar(despacho d)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                using (var db = new AlmacenContexto())
                {
                    despacho de = db.despacho.Find(d.id);

                    productos p = db.productos.Find(d.idproducto);
                    productos pd = db.productos.Find(d.idproducto);

                    if(de.idproducto == d.idproducto)
                    {
                        if (de.accion == "Entrada" && d.accion == "Entrada")
                        {
                            if (de.cant_producto > d.cant_producto)
                            {
                                p.existencia -= de.cant_producto - d.cant_producto;
                            }
                            else if (de.cant_producto < d.cant_producto)
                            {
                                p.existencia += d.cant_producto - de.cant_producto;
                            }
                            else if (de.cant_producto == d.cant_producto)
                            {

                            }
                        }
                        else if(de.accion== "Entrada"&& d.accion == "Salida")
                        {
                            if (p.existencia >= (de.cant_producto-d.cant_producto))
                            {
                                p.existencia -= de.cant_producto + d.cant_producto;
                            }
                        }
                        else if (de.accion == "Salida" && d.accion == "Salida")
                        {
                            if (de.cant_producto>d.cant_producto)
                            {
                                p.existencia += de.cant_producto - d.cant_producto;
                            }
                            else if (de.cant_producto < d.cant_producto)
                            {
                                if (p.existencia>=d.cant_producto)
                                {
                                    p.existencia -= d.cant_producto - de.cant_producto;
                                }
                                else
                                {
                                    throw new Exception();
                                }
                            }else if(de.cant_producto == d.cant_producto)
                            {
                                
                            }
                        }
                        else if (de.accion == "Salida" && d.accion == "Entrada")
                        {
                            p.existencia += de.cant_producto + d.cant_producto;
                        }

                    }
                    else if (de.idproducto != d.idproducto)
                    {

                        if (de.accion == "Entrada" && d.accion == "Entrada")
                        {
                            pd.existencia -= de.cant_producto;
                            p.existencia += d.cant_producto;
                        }
                        else if (de.accion == "Entrada" && d.accion == "Salida")
                        {
                            pd.existencia -= de.cant_producto;
                            p.existencia -= d.cant_producto;
                        }
                        else if (de.accion == "Salida" && d.accion == "Salida")
                        {
                            pd.existencia += de.cant_producto;
                            p.existencia -= d.cant_producto;
                        }
                        else if (de.accion == "Salida" && d.accion == "Entrada")
                        {
                            pd.existencia += de.cant_producto;
                            p.existencia += d.cant_producto;
                        }
                    } 



                    despacho des  = db.despacho.Find(d.id);
                    des.accion = d.accion;
                    des.fecha = d.fecha;
                    des.cant_producto = d.cant_producto;
                    des.detalle = d.detalle;
                    des.contacto = d.contacto;
                    des.idcliente = d.idcliente;
                    des.idproducto = d.idproducto;
                    
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
        [Authorize(Roles = "Admin, Recepcion, Superior")]
        public ActionResult Detalles(int id)
        {
            try
            {
                using (var db = new AlmacenContexto())
                {
                    despacho des = db.despacho.Find(id);
                    return View(des);
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
            
            using (var db = new AlmacenContexto())
            {
                despacho des = db.despacho.Find(id);
                productos p = db.productos.Find(des.idproducto);
                
                if (des.accion == "Entrada")
                {

                    p.existencia -= des.cant_producto;

                }
                else if (des.accion == "Salida")
                {
                   
                        p.existencia += des.cant_producto;
                 }
                
                db.despacho.Remove(des);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
               
            
        }
        public ActionResult ListClient()
        {
            using(var db= new AlmacenContexto())
            {
                return PartialView(db.clientes.ToList());
            }
        }
        public ActionResult ListpProd()
        {
            using (var db = new AlmacenContexto())
            {
                return PartialView(db.productos.ToList());
            }
        }
    }
}