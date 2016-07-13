using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProcCore.Business.Logic;
using ProcCore.WebCore;
using ProcCore.NetExtension;
using ProcCore.Business.Logic.TablesDescription;
using ProcCore.ReturnAjaxResult;
using DotWeb.CommSetup;
using Newtonsoft.Json;

namespace DotWeb.WebApp.Controllers
{
    public class IndexController : WebFrontController
    {
        public ActionResult Index()
        {
            ViewBag.IsFirstPage = true;
            ViewBag.BodyClass = "index";
            ViewBag.Cur = new string[] { "", "", "", "", "", "", "", "", "", "", "" };
            #region getSid Int
            a_ProductKind c = new a_ProductKind() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            RunQueryPackage<m_ProductKind> hResult = c.SearchMaster(new q_ProductKind() { s_Series = 12 }, 1);//get 新進商品
            int[] s_kinds = hResult.SearchData.Select(x => x.ID).ToArray();
            #endregion
            a_ProductData t = new a_ProductData() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            RunQueryPackage<m_ProductData> r = t.SearchMaster(new q_ProductData() { s_IsSecond = true, s_IsOpen = true, s_Kinds = s_kinds }, 1);
            return View(r.SearchData.AsEnumerable());
        }
    }


    public class PageHome
    {
        public m_Message[] messages { get; set; }
        public m_Product[] products { get; set; }
    }
}
