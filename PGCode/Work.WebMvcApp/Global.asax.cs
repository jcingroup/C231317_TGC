using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using System.Web.Optimization;
using DotWeb.CommSetup;

namespace DotWeb.AppStart
{
    public class MvcApplication : System.Web.HttpApplication
    {
        String VarCookie = CommWebSetup.WebCookiesId;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("en-US")
            //{
            //    ContextCondition = (
            //    Context => Context.Request.UserLanguages != null && Context.Request.UserLanguages[0].Contains("en"))
            //});
        }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            //HttpCookie WebLang = Request.Cookies[VarCookie + ".Lang"];

            //if (WebLang == null)
            //{
            //    //強制預設語系
            //    WebLang = new HttpCookie(VarCookie + ".Lang", "zh-TW");

            //    //if (Request.UserLanguages.Length > 0)
            //    //    WebLang = new HttpCookie(VarCookie + ".Lang", Request.UserLanguages[0]);
            //    //else
            //    //    WebLang = new HttpCookie(VarCookie + ".Lang", System.Threading.Thread.CurrentThread.CurrentCulture.Name);

            //    Response.Cookies.Add(WebLang);
            //}

            //if (WebLang != null)
            //{
            //    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(WebLang.Value);
            //    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(WebLang.Value);
            //}
        }
    }
}