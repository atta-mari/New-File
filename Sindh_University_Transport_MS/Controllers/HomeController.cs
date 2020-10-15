using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sindh_University_Transport_MS.Models;
using PagedList;
using PagedList.Mvc;

namespace Sindh_University_Transport_MS.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        Transport_ModelContainer db = new Transport_ModelContainer();
        public ActionResult Index(string search,int? page)
        { 
            if (page != null )
            {
                Session["page"] = page;
            }
            Session["search"] = "";
            if (search != "")
            {
                Session["search"] = search;
            }
            ViewBag.PointsData = db.Points.ToList();
            return View(db.Cities.ToList());
        }

        public ActionResult AllPoints()
        {
            int? page=1;
            string search = "";
            if (Session["page"] != null)
            {
                page = int.Parse(Session["page"].ToString());
            }
            if (Session["search"] != null)
            {
                search = Session["search"].ToString();
            }
            if (search != "")
            {
                var data = db.Points.Where(x => x.Location.Name.StartsWith(search) || x.Location.Name == null).ToList().ToPagedList(page ?? 1, 2);

                return View(data);
            }
            else
            {
             
                var data = db.Points.ToList().ToPagedList(page ?? 1, 2);

                return View(data);
            }
            var datas = db.Points.ToList().ToPagedList(page ?? 1, 2);

            return View(datas);

        }



        public ActionResult Areas(int? id)
        {
            if (id != null)
            {
                Session["id"] = id;
            }
            
            return View(db.Areas.Where(A=>A.CityId==id).ToList());
        }

        public ActionResult AllPointsCities()
        {
            
            int? id=null;
            if (Session["id"] != null)
            {
                id = int.Parse(Session["id"].ToString());
            }

            var data = db.Points.Where(x => x.LocationId == x.Location.Id
            && x.Location.AreaID == x.Location.Area.Id
            && x.Location.Area.CityId == x.Location.Area.City.Id
            && x.Location.Area.City.Id == x.Location.Area.City.Id
            && x.Location.Area.City.Id==id

            );

            return View(data.ToList());

        }

        public ActionResult Locations(int? id)
        {
            if (id != null)
            {
                Session["Areaid"] = id;
            }

            Area ar = db.Areas.FirstOrDefault(x=>x.Id==id);
            ViewBag.id = ar.CityId;
            return View(db.Locations.Where(L =>L.AreaID==id).ToList());
        }

        public ActionResult AllPointsAreas()
        {

            int? id = null;
            if (Session["Areaid"] != null)
            {
                id = int.Parse(Session["Areaid"].ToString());
            }

            var data = db.Points.Where(x => x.LocationId == x.Location.Id
            && x.Location.AreaID == x.Location.Area.Id
            && x.Location.Area.CityId == x.Location.Area.City.Id
            && x.Location.Area.Id== id

            );

            return View(data.ToList());

        }


        public ActionResult Points(int? id)
        {
            if (id != null)
            {
                Session["Pointsid"] = id;
            }


            Location loc = db.Locations.Find(id);
            ViewBag.id = loc.AreaID;
            return View();
        }


        public ActionResult AllPointsPoints()
        {

            int? id = null;
            if (Session["Pointsid"] != null)
            {
                id = int.Parse(Session["Pointsid"].ToString());
            }

            var data = db.Points.Where(x => x.LocationId ==  id);

            return View(data.ToList());

        }

        public ActionResult AboutUS()
        {
            return View();
        }


        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Login_Admin log)
        {
            if (ModelState.IsValid)
            {
                bool check = db.Admins.Any(x => x.Email == log.Email && x.Password == log.Password);

                if (check)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.msg = "email or password is incorrect";
                    return View();
                }
            }
            return View();
        }
    }
}