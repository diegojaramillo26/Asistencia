using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Asistencia.Models;
using Asistencia.ViewModels;

namespace Asistencia.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private dbAppsEntities db = new dbAppsEntities();

        // GET: Usuarios
        public ActionResult Index()
        {
            return View(db.Usuarios.ToList());
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        private List<SelectListItem> GetRols()
        {
            return db.Rols.Select(r => new SelectListItem { Text = r.Nombre, Value = r.Id.ToString() }).ToList();
        }
        // GET: Usuarios/Create
        public ActionResult Create()
        {
            var usuarioViewModel = new UsuarioViewModel();
            usuarioViewModel.Usuario = new Usuario();
            usuarioViewModel.Roles = GetRols();

            return View(usuarioViewModel);
        }

        // POST: Usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsuarioViewModel model)
        {
            var exists = db.Usuarios.Any(u => u.Email == model.Usuario.Email);
            if (ModelState.IsValid && !exists)
            {
                model.Usuario.Clave = Common.EncryptAes.Encrypt(model.Usuario.Clave);
                foreach (var rol in model.Roles.Where(x => x.Selected == true))
                {
                    var newRol = new Usuario_Rol { Id_rol = int.Parse(rol.Value) };
                    model.Usuario.Usuario_Rol.Add(newRol);
                }

                db.Usuarios.Add(model.Usuario);
                db.SaveChanges();
                TempData["Message"] = "El registro se guardó correctamente";
                return RedirectToAction("Create");
            }

            return View(model);
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioViewModel usuarioViewModel = new UsuarioViewModel();
            usuarioViewModel.Usuario = db.Usuarios.Find(id);
            if (usuarioViewModel.Usuario == null)
            {
                return HttpNotFound();
            }
            usuarioViewModel.Usuario.Clave = Common.EncryptAes.Decrypt(usuarioViewModel.Usuario.Clave);
            usuarioViewModel.Roles = GetRols();
            foreach (var rol in usuarioViewModel.Usuario.Usuario_Rol)
            {
                usuarioViewModel.Roles.Where(x => x.Value == rol.Id_rol.ToString()).FirstOrDefault().Selected = true;
            }
            return View(usuarioViewModel);
        }

        // POST: Usuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsuarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.Attach(model.Usuario);
                model.Usuario.Clave = Common.EncryptAes.Encrypt(model.Usuario.Clave);
                foreach (var rol in model.Roles.Where(x => x.Selected == true))
                {
                    var newRol = new Usuario_Rol { Id_rol = int.Parse(rol.Value), Id_Usuario = model.Usuario.Id };
                    model.Usuario.Usuario_Rol.Add(newRol);
                }

                var roles = db.Usuario_Rol.Where(x => x.Id_Usuario == model.Usuario.Id).ToList();
                db.Usuario_Rol.RemoveRange(roles);

                //db.Entry(model.Usuario.Usuario_Rol).State = EntityState.Modified;
                db.Entry(model.Usuario).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "El registro se guardó correctamente";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            db.Usuario_Rol.RemoveRange(usuario.Usuario_Rol);
            db.Usuarios.Remove(usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult BulkCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BulkCreate(HttpPostedFileBase file)
        {
            if (file.ContentLength == 0) {
                TempData["Message"] = "El archivo está vacio";
                return RedirectToAction("Index");
            }
            else
            {
                using (StreamReader sr = new StreamReader(file.InputStream))
                {
                    string currentLine = sr.ReadLine();
                    int count = 0;
                    while ((currentLine = sr.ReadLine())!= null)
                    {
                        var values = currentLine.Split(',');
                        var email = values[2];
                        var exists = db.Usuarios.Any(u => u.Email == email);
                        if (!exists)
                        {
                            Usuario newUser = new Usuario {
                                Nombres = values[0],
                                Apellidos = values[1],
                                Email = values[2],
                                Clave= Common.EncryptAes.Encrypt(values[3])

                            };
                            foreach (var rol in values[4].Split('-'))
                            {
                                var newRol = new Usuario_Rol { Id_rol = int.Parse(rol) };
                                newUser.Usuario_Rol.Add(newRol);
                            }

                            db.Usuarios.Add(newUser);
                            db.SaveChanges();
                            count += 1;
                        }
                    }
                    TempData["Message"] = $"Se registraron {count} usuarios correctamente";
                    return RedirectToAction("Index");
                }
            }
        } 

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
