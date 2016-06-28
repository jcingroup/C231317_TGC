using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotWeb.WebApp.Controllers
{
    public class InfoController : WebFrontController
    {
        //
        // GET: /Info/

        public ActionResult Info()
        {
            ViewBag.BodyClass = "Info";
            ViewBag.Cur = new string[] { "", "", "", "", "", "", "", "", "", "", "" };
            return View();
        }
        public ActionResult Info2()
        {
            ViewBag.BodyClass = "Info";
            ViewBag.Cur = new string[] { "", "", "", "", "", "", "", "", "", "", "" };
            return View();
        }
        public ActionResult Info3()
        {
            ViewBag.BodyClass = "Info";
            ViewBag.Cur = new string[] { "", "", "", "", "", "", "", "", "", "", "" };
            return View();
        }
        public ActionResult Info4()
        {
            ViewBag.BodyClass = "Info";
            ViewBag.Cur = new string[] { "", "", "", "", "", "", "", "", "", "", "" };
            return View();
        }
        public ActionResult Info5()
        {
            ViewBag.BodyClass = "Info";
            ViewBag.Cur = new string[] { "", "", "", "", "", "", "", "", "", "", "" };
            return View();
        }
        public ActionResult Info6()
        {
            ViewBag.BodyClass = "Info";
            ViewBag.Cur = new string[] { "", "", "", "", "", "", "", "", "", "", "" };
            return View();
        }
        public ActionResult Info7()
        {
            ViewBag.BodyClass = "Info";
            ViewBag.Cur = new string[] { "", "", "", "", "", "", "", "", "", "", "" };
            return View();
        }

    }
}
