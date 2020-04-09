using ProyectoFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        
            [Authorize]
        public ActionResult Index()
        {

            try
            {
                AlmacenContexto db = new AlmacenContexto();

                return View(db.clientes.ToList());
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Error" + ex.Message);
                return View();
            }
        }
        [Authorize(Roles = "Admin, Superior, Recepcion")]
        public ActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(clientes c)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                using (AlmacenContexto db = new AlmacenContexto())
                {
                    
                    db.clientes.Add(c);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al agregar Usuario", ex);

                return View();
            }


        
        }
        [Authorize(Roles = "Admin, Superior, Recepcion")]
        public ActionResult Editar(int id)
        {
            using (var db= new AlmacenContexto())
            {
                try
                {
                    clientes client = db.clientes.Find(id);
                    return View(client);
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
        public ActionResult Editar(clientes c)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                using (var db= new AlmacenContexto())
                {
                    clientes cli = db.clientes.Find(c.codigo_cliente);

                    cli.nombre_cliente = c.nombre_cliente;
                    cli.apellido_cliente = c.apellido_cliente;
                    cli.correo_cliente = c.correo_cliente;
                    cli.direcc_cliente = c.direcc_cliente;
                    cli.telef_cliente = c.telef_cliente;
                    cli.tipo = c.tipo;
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
        [Authorize]
        public  ActionResult Detalles(int id)
        {
            try
            {
                using (var db = new AlmacenContexto())
                {
                    clientes client = db.clientes.Find(id);
                    return View(client);
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
                clientes client = db.clientes.Find(id);
                db.clientes.Remove(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}