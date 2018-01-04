using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class TicketsController : Controller
    {
        private TicketDBContext db = new TicketDBContext();
        private MenuDBContext dbMenu = new MenuDBContext();

        // GET: Tickets
        public ActionResult Index()
        {
            return View(db.Menus.ToList());
        }

        // Being named id is important because of my routing settings. It expects a paramter named ID, not some custom more fitting name 
        public ActionResult Confirmation(int? id)
        {
            if(OrderNumber.paid == false)
            {
                return HttpNotFound();
            }
            OrderNumber.tNum = (int)id;
            var a = db.Menus.ToList();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            foreach (Ticket b in a)
            {
                b.TableNumber = (int)id;
                if(b.Status == "ordering")
                    b.Status = "Preparing";

                db.SaveChanges();
            }

            return View(db.Menus.ToList());
        }

        public ActionResult StaffPage()
        {
            return View(db.Menus.ToList());
        }

        [HttpPost]
        public JsonResult AjaxMethod(string id)
        {
            var a = db.Menus.ToList();
            foreach (Ticket b in a)
            {
                b.Status = id;

                db.SaveChanges();
            }

            return Json(a[0]);
        }

        [HttpPost]
        public JsonResult UpdateStatus()
        {
            var a = db.Menus.ToList();
            return Json(a[0]);
        }

        [HttpPost]
        public ActionResult AddToOrder(int ID)
        {
            var menu = dbMenu.Menus.ToList();
            var tick = db.Menus.ToList();

            Ticket t = new Ticket();

            foreach (Menu b in menu)
            {

                if (b.ID == ID)
                {
                    try
                    {
                        t.ID = b.ID;
                        t.Name = b.Name;
                        t.Description = b.Descritpion;
                        t.Price = b.Price;
                        t.Quantity = 1;
                        t.Category = b.Category;
                        t.TableNumber = 0;
                        t.Status = "ordering";
                        db.Menus.Add(t);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        // Need to remove the ticket we added above because it already exists. If we don't remove then save, it will crash with dup ID key
                        db.Menus.Remove(t);
                        Ticket addQuan = db.Menus.Find(ID);
                        addQuan.Quantity = (addQuan.Quantity + 1);
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Debug.Print("ERROR ~~~~~ ::::: " + ex);
                        }
                    }
                    break;
                }
            }

            return Json(t.Name.Trim());
        }

        // GET: Tickets/Details/X
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Menus.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Carts/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.basketID = id;

            return View();
        }


        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Description,Price,Quantity,Category")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.TableNumber = 0;
                ticket.Status = "ordering";
                db.Menus.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Menus.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,Price,Quantity,Category")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Menus.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Menus.Find(id);
            db.Menus.Remove(ticket);
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
