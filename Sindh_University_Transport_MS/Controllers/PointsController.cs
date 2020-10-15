using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sindh_University_Transport_MS.Models;
using System.IO;

namespace Sindh_University_Transport_MS.Controllers
{
    public class PointsController : Controller
    {
        private Transport_ModelContainer db = new Transport_ModelContainer();

        // GET: Points
        public ActionResult Index()
        {
            var points = db.Points.Include(p => p.Location);
            return View(points.ToList());
        }

        // GET: Points/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Point point = db.Points.Find(id);
            if (point == null)
            {
                return HttpNotFound();
            }
            return View(point);
        }

        // GET: Points/Create
        public ActionResult _Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult _Create(Points_Get point)
        {
            if (ModelState.IsValid)
            {
            
            if (point.P_Photo.ContentLength > 0)
            {
                    string extension = Path.GetExtension(point.P_Photo.FileName);

                    if (extension.ToLower()==".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        string filenmae = Path.GetFileName(point.P_Photo.FileName);

                        string path = Path.Combine(Server.MapPath(" /Photos/"), filenmae);

                        point.P_Photo.SaveAs(path);
                    }
                    else
                    {
                        Response.Write("Please upload image with extention .jpg or .jpeg or .png");
                    }
            }
            else
            {
                Response.Write("error");
            }
            }
            return View();
        }


        public ActionResult Create()
        {
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name");
            return View();
        }

        // POST: Points/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "P_Id,P_Name,P_type,P_Number,P_Photo,P_S_Timing,P_E_Timing,Shift,LocationId")] Points_Get point)
        {
            if (ModelState.IsValid)
            {
                Point points = new Point();
                points.P_Name = point.P_Name;
                points.P_type = point.P_type;
                points.P_Number = point.P_Number;
              

                if(point.P_Photo.ContentLength > 0)
                   {
                    string extension = Path.GetExtension(point.P_Photo.FileName);

                    if(extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                        {
                        string filenmae = Path.GetFileName(point.P_Photo.FileName);

                        string path = Path.Combine(Server.MapPath("~/Photos/"), filenmae);

                        points.P_Photo = @"~/Photos/" + filenmae;
                        point.P_Photo.SaveAs(path);
                        }
                    else
                        {
                        Response.Write("Please upload image with extention .jpg or .jpeg or .png");
                        }
                    }
                else
                {
                    Response.Write("error");
                }
                points.P_S_Timing = point.P_S_Timing;
                points.P_E_Timing = point.P_E_Timing;
                points.Shift = point.Shift;
                points.LocationId = point.LocationId;

                db.Points.Add(points);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", point.LocationId);
            return View(point);
        }

        // GET: Points/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Point point = db.Points.Find(id);

            Points_Get points = new Points_Get();
            points.P_Id = point.P_Id;
            points.P_Name = point.P_Name;
            points.P_type = point.P_type;
            points.P_Number = point.P_Number;
            points.P_S_Timing = point.P_S_Timing;
            points.P_E_Timing = point.P_E_Timing;
            points.Shift = point.Shift;
            points.LocationId = point.LocationId;

            if (point == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", point.LocationId);

            return View(points);
        }

        // POST: Points/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "P_Id,P_Name,P_type,P_Number,P_Photo,P_S_Timing,P_E_Timing,Shift,LocationId")] Points_Get point)
        {
            

            if (ModelState.IsValid)
            {
                Point points = db.Points.FirstOrDefault(x => x.P_Id == point.P_Id);
                Point pt = db.Points.FirstOrDefault(x => x.P_Id == point.P_Id);
                points.P_Name = point.P_Name;
                points.P_type = point.P_type;
                points.P_Number = point.P_Number;


                if (point.P_Photo == null)
                {
                    points.P_Photo = pt.P_Photo;
                }
                else
                {
                    if (point.P_Photo.ContentLength > 0)
                {
                        string extension = Path.GetExtension(point.P_Photo.FileName);
                        string filenmae = Path.GetFileName(point.P_Photo.FileName);

                        if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                        {


                            string path = Path.Combine(Server.MapPath("~/Photos/"), filenmae);

                            points.P_Photo = @"~/Photos/" + filenmae;
                            point.P_Photo.SaveAs(path);



                        }
                        else
                        {
                            Response.Write("Please upload image with extention .jpg or .jpeg or .png");
                        }
                    }
                
                else
                {
                        Response.Write("error");
                 }
                }
                points.P_S_Timing = point.P_S_Timing;
                points.P_E_Timing = point.P_E_Timing;
                points.Shift = point.Shift;
                points.LocationId = point.LocationId;


                db.Entry(points).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", point.LocationId);
            return View(point);
        }



        // GET: Points/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Point point = db.Points.Find(id);

            if (point == null)
            {
                return HttpNotFound();
            }
            return View(point);
        }

        // POST: Points/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Point point = db.Points.Find(id);
            db.Points.Remove(point);
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
