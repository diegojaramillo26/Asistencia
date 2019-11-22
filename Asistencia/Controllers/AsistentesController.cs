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
    public class AsistentesController : Controller
    {
        private dbAppsEntities db = new dbAppsEntities();

        // GET: Asistentes
        public ActionResult Index()
        {
            var asistentes = db.Asistentes.Include(a => a.Evento);
            return View(asistentes.ToList());
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
                    return View();
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('El evento no se encuentra activo');</script>");
                }
            }


        }

        // POST: Asistentes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IntIdAsistente,strNombre,strCargo,intIdEvento,strSeccion,binFirma")] Asistente asistente)
        {
            if (ModelState.IsValid)
            {
                db.Asistentes.Add(asistente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.intIdEvento = new SelectList(db.Eventos, "intIdEvento", "strTema", asistente.intIdEvento);
            return View(asistente);
        }

        // GET: Asistentes/Edit/5
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
            ViewBag.intIdEvento = new SelectList(db.Eventos, "intIdEvento", "strTema", asistente.intIdEvento);
            return View(asistente);
        }

        // POST: Asistentes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IntIdAsistente,strNombre,strCargo,intIdEvento,strSeccion,binFirma")] Asistente asistente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(asistente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.intIdEvento = new SelectList(db.Eventos, "intIdEvento", "strTema", asistente.intIdEvento);
            return View(asistente);
        }

        // GET: Asistentes/Delete/5
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
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Asistente asistente = db.Asistentes.Find(id);
            db.Asistentes.Remove(asistente);
            db.SaveChanges();
            return RedirectToAction("Index");
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
