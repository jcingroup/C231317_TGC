﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotWeb.WebApp.Controllers
{
    public class AboutUsController : WebFrontController
    {
        //
        // GET: /AboutUs/

        public ActionResult AboutUs()
        {
            ViewBag.BodyClass = "AboutUs";
            ViewBag.Cur = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            return View();
        }

    }
}
