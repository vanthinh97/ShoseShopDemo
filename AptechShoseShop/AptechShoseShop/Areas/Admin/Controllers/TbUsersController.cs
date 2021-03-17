using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AptechShoseShop.Models.Entites;

namespace AptechShoseShop.Areas.Admin.Controllers
{
    public class TbUsersController : Controller
    {
        private AptechShoseShopDbContext db = new AptechShoseShopDbContext();

        // GET: Admin/TbUsers
        public ActionResult Index()
        {
            var tbUsers = db.TbUsers.Include(t => t.Status);
            return View(tbUsers.ToList());
        }

        // GET: Admin/TbUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TbUser tbUser = db.TbUsers.Find(id);
            if (tbUser == null)
            {
                return HttpNotFound();
            }
            return View(tbUser);
        }

        // GET: Admin/TbUsers/Create
        public ActionResult Create()
        {
            ViewBag.StatusId = new SelectList(db.StatusUsers, "Id", "StatusName");
            return View();
        }

        // POST: Admin/TbUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FullName,Gender,Email,Address,Password,StatusId,CreatedDate,Avatar")] TbUser tbUser)
        {
            if (ModelState.IsValid)
            {
                db.TbUsers.Add(tbUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StatusId = new SelectList(db.StatusUsers, "Id", "StatusName", tbUser.StatusId);
            return View(tbUser);
        }

        // GET: Admin/TbUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TbUser tbUser = db.TbUsers.Find(id);
            if (tbUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.StatusId = new SelectList(db.StatusUsers, "Id", "StatusName", tbUser.StatusId);
            return View(tbUser);
        }

        // POST: Admin/TbUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FullName,Gender,Email,Address,Password,StatusId,CreatedDate,Avatar")] TbUser tbUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StatusId = new SelectList(db.StatusUsers, "Id", "StatusName", tbUser.StatusId);
            return View(tbUser);
        }

        // GET: Admin/TbUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TbUser tbUser = db.TbUsers.Find(id);
            if (tbUser == null)
            {
                return HttpNotFound();
            }
            return View(tbUser);
        }

        // POST: Admin/TbUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TbUser tbUser = db.TbUsers.Find(id);
            db.TbUsers.Remove(tbUser);
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
