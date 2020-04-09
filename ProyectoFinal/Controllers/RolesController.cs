using ProyectoFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class RolesController : Controller
    {
        // GET: Role
        [Authorize(Roles = "Admin, Superior")]
        public ActionResult Index()
            {
                try
                {
                    AlmacenContexto db = new AlmacenContexto();

                    return View(db.AspNetUserRoles.ToList());
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", "Error" + ex.Message);
                    return View();
                }
            }
        [Authorize(Roles = "Admin")]
        public ActionResult Agregar(AspNetUserRoles ur)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                using (AlmacenContexto db = new AlmacenContexto())
                {

                    db.AspNetUserRoles.Add(ur);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al Agregar Rol", ex);

                return View();
            }



        }
        [Authorize(Roles = "Admin")]
        public ActionResult Eliminar(string Id,string Id2)
        {
            using (var db = new AlmacenContexto())
            {
                AspNetUserRoles aspur = db.AspNetUserRoles.Find(Id,Id2);
                db.AspNetUserRoles.Remove(aspur);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Editar(string id,string id2)
        {
            using (var db = new AlmacenContexto())
            {
                try
                {
                    AspNetUserRoles aspur = db.AspNetUserRoles.Find(id,id2);
                    return View(aspur);
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
        public ActionResult Editar(AspNetUserRoles aspu)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                using (var db = new AlmacenContexto())
                {
                    AspNetUserRoles aspnet = db.AspNetUserRoles.Find( aspu.UserId,aspu.RoleId);

                    aspnet.RoleId = aspu.RoleId;
                    aspnet.RoleName = aspu.RoleName;
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
    }
    }
