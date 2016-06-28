using ProcCore.Business.Logic;
using System;
using System.Linq;
using System.Web.Mvc;

namespace DotWeb.WebApp.Controllers
{
    public class ProductsController : WebFrontController
    {
        public ActionResult Products_new(int? sid, int? page)
        {
            ViewBag.BodyClass = "Products new";
            ViewBag.Cur = new string[] { "", "", "", "", "", "", "", "", "", "", "" };
            String[] Series = { "", "屏風系列", "辦公桌系列", "會議桌系列", "檔案櫃系列", "主管辦公桌系列", "辦公椅系列", "沙發系列", "", "", "其他商品" };
            if (sid == null) { sid = 1; }

            ViewBag.Cur[Convert.ToInt32(sid)] = "current";
            ViewBag.Nur = Convert.ToInt32(sid);
            ViewBag.Series = Series[Convert.ToInt32(sid)];
            a_ProductKind t = new a_ProductKind() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            RunQueryPackage<m_ProductKind> r = t.SearchMaster(new q_ProductKind() { s_IsOpen = true, s_Series = sid, s_IsSecond = false }, 1);

            page = page == null ? 1 : page;

            int totalpage = r.SearchData.Count() / 9;
            int mod = r.SearchData.Count() % 9;
            if (mod != 0)
            {
                totalpage++;
            }
            int getTotalPage = totalpage;
            var getProduct = r.SearchData.Skip(9 * ((int)page - 1)).Take(9);

            ViewBag.nextPage = getTotalPage > page ? page + 1 : page;
            ViewBag.prvePage = page <= 1 ? 1 : page - 1;

            if (r.SearchData.Count() >= 1 && r.SearchData.Count() <= 9)
            {
                ViewBag.totalPage = 1;
            }
            else
            {
                ViewBag.totalPage = getTotalPage;
            }
            ViewBag.page = page;
            ViewBag.sid = sid;


            return View(getProduct.AsEnumerable());
        }
        public ActionResult Products_new2(int? sid, int? kid, int? page)
        {
            ViewBag.BodyClass = "Products new";
            ViewBag.Cur = new string[] { "", "", "", "", "", "", "", "", "", "", "" };
            String[] Series = { "", "屏風系列", "辦公桌系列", "會議桌系列", "檔案櫃系列", "主管辦公桌系列", "辦公椅系列", "沙發系列", "", "", "其他商品" };
            if (sid == null) { sid = 1; }

            ViewBag.Cur[Convert.ToInt32(sid)] = "current";
            ViewBag.Nur = Convert.ToInt32(sid);
            ViewBag.Series = Series[Convert.ToInt32(sid)];
            a_ProductKind t = new a_ProductKind() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            RunQueryPackage<m_ProductKind> r = t.SearchMaster(new q_ProductKind() { s_ID = kid }, 1);
            ViewBag.kind = r.SearchData.First().Name;

            a_ProductData p = new a_ProductData() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            RunQueryPackage<m_ProductData> pr = p.SearchMaster(new q_ProductData() { s_IsOpen = true, s_IsSecond = false, s_Kind = kid }, 1);



            if (r.SearchData.Any())
            {
                int? kind_id = r.SearchData.First().ID;
                var getid = p.SearchMaster(new q_ProductData() { s_Kind = kind_id }, 0);
                if (r.SearchData.Count() == 1 && getid.SearchData.Any())
                { //如果此種類(kid)只有一項就直接跳到product_second3去顯示

                    int id = getid.SearchData.First().ID;
                    Response.Redirect("~/Products/Products_new3/" + id + "?sid=" + sid);
                }
            }

            page = page == null ? 1 : page;

            int totalpage = pr.SearchData.Count() / 9;
            int mod = pr.SearchData.Count() % 9;
            if (mod != 0)
            {
                totalpage++;
            }
            int getTotalPage = totalpage;
            var getProduct = pr.SearchData.Skip(9 * ((int)page - 1)).Take(9);

            ViewBag.nextPage = getTotalPage > page ? page + 1 : page;
            ViewBag.prvePage = page <= 1 ? 1 : page - 1;
            if (r.SearchData.Count() >= 1 && r.SearchData.Count() <= 9)
            {
                ViewBag.totalPage = 1;
            }
            else
            {
                ViewBag.totalPage = getTotalPage;
            }
            ViewBag.page = page;
            ViewBag.sid = sid;


            return View(getProduct.AsEnumerable());
        }
        public ActionResult Products_new3(int id, int? sid)
        {
            a_ProductData b = new a_ProductData() { Connection = getSQLConnection(), logPlamInfo = plamInfo };//資料庫連線,取得登入者資訊
            var a = b.GetDataMaster(id, 0);
            //取得kind的值
            a_ProductKind t = new a_ProductKind() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            RunQueryPackage<m_ProductKind> r = t.SearchMaster(new q_ProductKind() { s_ID = a.SearchData.Kind }, 1);
            var tmp = r.SearchData.FirstOrDefault().Name;
            ViewBag.kind = tmp;//放進viewbag裡

            ViewBag.BodyClass = "Products new";
            ViewBag.Cur = new string[] { "", "", "", "", "", "", "", "", "", "", "" };
            String[] Series = { "", "屏風系列", "辦公桌系列", "會議桌系列", "檔案櫃系列", "主管辦公桌系列", "辦公椅系列", "沙發系列", "", "", "其他商品" };
            if (sid == null) { sid = 1; }

            ViewBag.Cur[Convert.ToInt32(sid)] = "current";
            ViewBag.Nur = Convert.ToInt32(sid);
            ViewBag.Series = Series[Convert.ToInt32(sid)];

            return View(a.SearchData);
        }
        public ActionResult Products_second(int? sid, int? page)
        {
            if (sid == null) { sid = 1; }
            if (Convert.ToInt32(sid) == 0 || Convert.ToInt32(sid) == 8 || Convert.ToInt32(sid) == 9) { return RedirectToAction("Products_second2", "Products", new { sid = sid }); }

            ViewBag.BodyClass = "Products second";
            ViewBag.Cur = new string[] { "", "", "", "", "", "", "", "", "", "", "" };
            String[] Series = { "新進商品", "屏風系列", "辦公桌系列", "會議桌系列", "檔案櫃系列", "主管辦公桌系列", "辦公椅系列", "沙發系列", "特價系列", "規劃樣品展示", "其他商品" };


            ViewBag.Cur[Convert.ToInt32(sid)] = "current";
            ViewBag.Nur = Convert.ToInt32(sid);
            ViewBag.Series = Series[Convert.ToInt32(sid)];
            a_ProductKind t = new a_ProductKind() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            RunQueryPackage<m_ProductKind> r = t.SearchMaster(new q_ProductKind() { s_IsOpen = true, s_Series = sid, s_IsSecond = true }, 1);


            page = page == null ? 1 : page;

            int totalpage = r.SearchData.Count() / 9;
            int mod = r.SearchData.Count() % 9;
            if (mod != 0)
            {
                totalpage++;
            }
            int getTotalPage = totalpage;
            var getProduct = r.SearchData.Skip(9 * ((int)page - 1)).Take(9);

            ViewBag.nextPage = getTotalPage > page ? page + 1 : page;
            ViewBag.prvePage = page <= 1 ? 1 : page - 1;

            if (r.SearchData.Count() >= 1 && r.SearchData.Count() <= 9)
            {
                ViewBag.totalPage = 1;
            }
            else
            {
                ViewBag.totalPage = getTotalPage;
            }
            ViewBag.page = page;
            ViewBag.sid = sid;

            return View(getProduct);
        }
        public ActionResult Products_second2(int? sid, int? kid, int? page)
        {
            ViewBag.BodyClass = "Products second";
            ViewBag.Cur = new string[] { "", "", "", "", "", "", "", "", "", "", "" };
            String[] Series = { "新進商品", "屏風系列", "辦公桌系列", "會議桌系列", "檔案櫃系列", "主管辦公桌系列", "辦公椅系列", "沙發系列", "特價系列", "規劃樣品展示", "其他商品" };
            if (sid == null) { sid = 1; }

            ViewBag.Cur[Convert.ToInt32(sid)] = "current";
            ViewBag.Nur = Convert.ToInt32(sid);
            ViewBag.Series = Series[Convert.ToInt32(sid)];

            a_ProductKind t = new a_ProductKind() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            RunQueryPackage<m_ProductKind> r = t.SearchMaster(new q_ProductKind() { s_ID = kid, s_Series = sid }, 1);
            a_ProductData p = new a_ProductData() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            RunQueryPackage<m_ProductData> pr = new RunQueryPackage<m_ProductData>();
            q_ProductData q = new q_ProductData() { s_IsOpen = true, s_IsSecond = true };

            if (Convert.ToInt32(sid) == 0) { q.s_IsNew = true; ViewBag.kind = "新進商品"; }
            else if (Convert.ToInt32(sid) == 8) { q.s_IsOnSell = true; ViewBag.kind = "特價商品"; }
            else if (Convert.ToInt32(sid) == 9) { q.s_IsDisp = true; ViewBag.kind = "精品展示"; }
            else if (Convert.ToInt32(sid) == 10) { q.s_Kind = kid; ViewBag.kind = "其他商品"; }
            else
            {
                ViewBag.kind = r.SearchData.First().Name;
                q.s_Kind = kid;
            }
            if (r.SearchData.Any())
            {
                int? kind_id = r.SearchData.First().ID;
                var getid = p.SearchMaster(new q_ProductData() { s_Kind = kind_id }, 0);
                if (r.SearchData.Count() == 1 && getid.SearchData.Any())
                { //如果此種類(kid)只有一項就直接跳到product_second3去顯示

                    int id = getid.SearchData.First().ID;
                    Response.Redirect("~/Products/Products_second3/" + id + "?sid=" + sid);
                }
            }


            pr = p.SearchMaster(q, 1);

            page = page == null ? 1 : page;

            int totalpage = pr.SearchData.Count() / 9;
            int mod = pr.SearchData.Count() % 9;
            if (mod != 0)
            {
                totalpage++;
            }
            int getTotalPage = totalpage;
            var getProduct = pr.SearchData.Skip(9 * ((int)page - 1)).Take(9);

            ViewBag.nextPage = getTotalPage > page ? page + 1 : page;
            ViewBag.prvePage = page <= 1 ? 1 : page - 1;
            if (r.SearchData.Count() >= 1 && r.SearchData.Count() <= 9)
            {
                ViewBag.totalPage = 1;
            }
            else
            {
                ViewBag.totalPage = getTotalPage;
            }
            ViewBag.page = page;
            ViewBag.sid = sid;

            return View(getProduct.AsEnumerable());
        }
        public ActionResult Products_second3(int id, int? sid)
        {
            a_ProductData b = new a_ProductData() { Connection = getSQLConnection(), logPlamInfo = plamInfo };//資料庫連線,取得登入者資訊
            var a = b.GetDataMaster(id, 0);
            if (a.SearchData == null) {
                return Redirect("~/error.html");
            }

            //取得kind的值
            a_ProductKind t = new a_ProductKind() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            RunQueryPackage<m_ProductKind> r = t.SearchMaster(new q_ProductKind() { s_ID = a.SearchData.Kind }, 1);
            var tmp = r.SearchData.FirstOrDefault().Name;
            ViewBag.kind = tmp;//放進viewbag裡

            ViewBag.BodyClass = "Products second";
            ViewBag.Cur = new string[] { "", "", "", "", "", "", "", "", "", "", "" };
            String[] Series = { "新進商品", "屏風系列", "辦公桌系列", "會議桌系列", "檔案櫃系列", "主管辦公桌系列", "辦公椅系列", "沙發系列", "特價系列", "規劃樣品展示", "其他商品" };
            if (sid == null) { sid = 1; }

            ViewBag.Cur[Convert.ToInt32(sid)] = "current";
            ViewBag.Nur = Convert.ToInt32(sid);
            ViewBag.Series = Series[Convert.ToInt32(sid)];

            return View(a.SearchData);
        }

    }
}
