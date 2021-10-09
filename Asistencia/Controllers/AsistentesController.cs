using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Asistencia.Models;

namespace Asistencia.Controllers
{
    [Authorize]
    public class AsistentesController : Controller
    {
        private dbAppsEntities db = new dbAppsEntities();

        // GET: Asistentes
        [Authorize(Roles = "Administrador,Creador")]
        public ActionResult Index(int? id)
        {
           //var asistentes = db.Asistentes.Include(b => b.Evento);
            var asistentes = (from c in db.Asistentes.Include("Evento")
                         where c.intIdEvento == id
                         select c).ToList();
            return View(asistentes);
        }

        // GET: Asistentes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asistente asistente = db.Asistentes.Find(id);
           
            if (asistente == null)
            {
                return HttpNotFound();
            }
            return View(asistente);
        }

        // GET: Asistentes/Create
        [AllowAnonymous]
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            else
            {
                Evento evento = db.Eventos.Find(int.Parse((string)this.RouteData.Values["id"]));
                if (evento.bitEstado != false)
                {
                    ViewBag.NombreEvento = evento.strTema;
                    return View();
                }
                else
                {
                    //TempData["Message"] = "El evento no se encuentra activo";
                    //return RedirectToAction("Index", "Eventos");
                    ViewBag.NombreEvento = evento.strTema;
                    return View("Inactive");
                    //return Content("<script language='javascript' type='text/javascript'>alert('El evento no se encuentra activo');</script>");
                   
                    //return Content("<script language='javascript' type='text/javascript'>Swal.fire('El evento no se encuentra activo');</script>");
                }
            }


        }

        // POST: Asistentes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IntIdAsistente,strNombre,strCargo,intIdEvento,strSeccion,binFirma,strDocumento")] Asistente asistente)
        {
            if (ModelState.IsValid)
            {
                asistente.intIdEvento = int.Parse((string)this.RouteData.Values["id"]);
                if (!db.Asistentes.Any(a => a.strDocumento == asistente.strDocumento && a.intIdEvento == asistente.intIdEvento))
                {
                    db.Asistentes.Add(asistente);
                    db.SaveChanges();
                    TempData["Message"] = "El registro se guardó correctamente"; 
                }else
                {
                    TempData["Message"] = $"Ya existe un registro con el documento {asistente.strDocumento}, no se guardó el formulario";
                }
                return RedirectToAction("Create");
          
            }

            return View(asistente);
        }

        // GET: Asistentes/Edit/5
        [Authorize(Roles = "Administrador,Creador")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asistente asistente = db.Asistentes.Find(id);
            if (asistente == null)
            {
                return HttpNotFound();
            }
            return View(asistente);
        }

        // POST: Asistentes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Creador")]
        public ActionResult Edit([Bind(Include = "IntIdAsistente,strNombre,strCargo,intIdEvento,strSeccion,strDocumento")] Asistente asistente)
        {
            if (ModelState.IsValid)
            {
                var currentAsistante = db.Asistentes.Find(asistente.IntIdAsistente);
                db.Entry(currentAsistante).Property("strDocumento").CurrentValue= asistente.strDocumento;
                db.Entry(currentAsistante).Property("strNombre").CurrentValue = asistente.strNombre;
                db.Entry(currentAsistante).Property("strCargo").CurrentValue = asistente.strCargo;
                db.Entry(currentAsistante).Property("strSeccion").CurrentValue = asistente.strSeccion;
                db.SaveChanges();
                return RedirectToAction("Index",new {id = asistente.intIdEvento });
            }
            return View(asistente);
        }

        // GET: Asistentes/Delete/5
        [Authorize(Roles = "Administrador,Creador")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asistente asistente = db.Asistentes.Find(id);
            if (asistente == null)
            {
                return HttpNotFound();
            }
            return View(asistente);
        }

        // POST: Asistentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador,Creador")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Asistente asistente = db.Asistentes.Find(id);
            db.Asistentes.Remove(asistente);
            db.SaveChanges();
            return RedirectToAction("Index", "Eventos");
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
