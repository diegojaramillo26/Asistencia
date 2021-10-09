using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Asistencia.Models;
using QRCoder;

namespace Asistencia.Controllers
{
    [Authorize]
    public class EventosController : Controller
    {
        private dbAppsEntities db = new dbAppsEntities();

        // GET: Eventos
        public ActionResult Index()
        {
            return View(db.Eventos.OrderByDescending(e => e.dtmFecha).ToList());
        }

        // GET: Eventos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evento evento = db.Eventos.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            return View(evento);
        }

        // GET: Link
        public ActionResult Link(string txtQRCode)
        {
            string link = txtQRCode.Replace(ConfigurationManager.AppSettings["ServerName"], 
                                            ConfigurationManager.AppSettings["ServerName"] + ":" + ConfigurationManager.AppSettings["ServerPort"]);
            ViewBag.txtQRCode = link;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            //System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
            //imgBarCode.Height = 150;
            //imgBarCode.Width = 150;
            using (Bitmap bitMap = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ViewBag.imageBytes = ms.ToArray();
                    //imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                }
            }
            return View();
        }

        // GET: Eventos/Create
        [Authorize(Roles = "Administrador,Creador")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Eventos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Creador")]
        public ActionResult Create([Bind(Include = "intIdEvento,strTema,strFacilitador,strLugar,dtmFecha,dtmDuracion")] Evento evento)
        {
            if (ModelState.IsValid)
            {
                //Get the current claims principal
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                // Get the claims values
                var userId = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                   .Select(c => c.Value).SingleOrDefault();
                evento.intCreador = int.Parse(userId);
                evento.bitEstado = true;
                db.Eventos.Add(evento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(evento);
        }

        // GET: Eventos/Edit/5
        [Authorize(Roles = "Administrador,Creador")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evento evento = db.Eventos.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            return View(evento);
        }

        // POST: Eventos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Creador")]
        public ActionResult Edit([Bind(Include = "intIdEvento,strTema,strFacilitador,strLugar,dtmFecha,dtmDuracion,bitEstado")] Evento evento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(evento).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "El registro se actualizó correctamente";
                return RedirectToAction("Edit", evento.intIdEvento);
            }
            return View(evento);
        }

        // GET: Eventos/Delete/5
        [Authorize(Roles = "Administrador,Creador")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evento evento = db.Eventos.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            return View(evento);
        }

        // POST: Eventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador,Creador")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Evento evento = db.Eventos.Find(id);
            db.Eventos.Remove(evento);
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
