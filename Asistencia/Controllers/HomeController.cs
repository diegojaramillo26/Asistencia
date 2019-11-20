using Asistencia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asistencia.Controllers
{
    public class HomeController : Controller
    {
        private dbAppsEntities db = new dbAppsEntities();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "strNombre,strCargo,strSeccion,binFirma")] Asistencia.Models.Asistente asistente)
        {
            if (ModelState.IsValid)
            {
                asistente.intIdEvento = int.Parse((string)this.RouteData.Values["id"]);
                db.Asistentes.Add(asistente);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            return View(asistente);
            //try
            //{
            //    // TODO: Add insert logic here
            //    if (ModelState.IsValid)
            //    {
            //        db.Asistentes.Add(asistente);
            //        db.SaveChanges();
            //        return RedirectToAction("Index");
            //    }

            //}
            //catch
            //{
            //    return View(asistente);
            //}
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Home/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Home/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
