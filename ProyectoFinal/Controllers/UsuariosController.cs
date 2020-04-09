using ProyectoFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class UsuariosController : Controller
    {
        // GET: Usuarios
        public ActionResult Index()
        {
            try
            {
                AlmacenContexto db = new AlmacenContexto();

                return View(db.AspNetUsers.ToList());
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Error" + ex.Message);
                return View();
            }
        }

        [Authorize(Roles = "Admin, Superior")]
        public ActionResult Editar(string Id)
        {
            using (var db = new AlmacenContexto())
            {
                try
                {
                    AspNetUsers asp = db.AspNetUsers.Find(Id);
                    return View(asp);
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
        public ActionResult Editar(AspNetUsers u)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                using (var db = new AlmacenContexto())
                {
                    AspNetUsers asp = db.AspNetUsers.Find(u.Id);

                    asp.Email = u.Email;
                    asp.PhoneNumber = u.PhoneNumber;
                    asp.UserName = u.UserName;
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
        public ActionResult Detalles(string id)
        {
            try
            {
                using (var db = new AlmacenContexto())
                {
                    AspNetUsers asu = db.AspNetUsers.Find(id);
                    return View(asu);
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("Error", ex);

                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Eliminar(string Id)
        {
            using (var db = new AlmacenContexto())
            {
                AspNetUsers asp = db.AspNetUsers.Find(Id);
                db.AspNetUsers.Remove(asp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        public static string NombreUsuario(string Id)
        {
            using (var db = new AlmacenContexto())
            {
                return db.AspNetUsers.Find(Id).UserName;
            }
        }

    }
}