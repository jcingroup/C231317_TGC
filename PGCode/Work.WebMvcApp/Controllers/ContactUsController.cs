using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Reflection;
using System.Collections.Specialized;
using ProcCore.Business.Logic;
using ProcCore.WebCore;
using ProcCore.NetExtension;
using ProcCore.ReturnAjaxResult;
using DotWeb.CommSetup;
using Newtonsoft.Json;

namespace DotWeb.WebApp.Controllers
{
    public class ContactUsController : WebFrontController
    {
        //
        // GET: /ContactUs/

        public ActionResult ContactUs()
        {
            ViewBag.BodyClass = "ContactUs";
            ViewBag.Cur = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            return View();
        }
        
        public string ajax_sendmail(mailContext t)
        {
            try
            {
                mailContext ap = t;
                ViewResult resultView = View("mail_apply", ap);

                StringResult sr = new StringResult();
                sr.ViewName = resultView.ViewName;
                sr.MasterName = resultView.MasterName;
                sr.ViewData = resultView.ViewData;
                sr.TempData = resultView.TempData;
                sr.ExecuteResult(this.ControllerContext);

                MailMessage message = new MailMessage();

                message.From = new MailAddress(t.t9, t.t2);
                //message.To.Add(new MailAddress("top04244@yahoo.com.tw"));
                message.To.Add(new MailAddress("top04244@yahoo.com.tw", "禾吉辰"));
                message.Bcc.Add(new MailAddress("jerryissky@gmail.com", "Jerry"));
                //message.CC.Add(new MailAddress("k302296@gmail.com", "Jason"));
                //message.CC.Add(new MailAddress("lei.chieh@gmail.com", "Stacey"));

                message.IsBodyHtml = true;
                message.BodyEncoding = System.Text.Encoding.UTF8;//E-mail編碼
                message.Subject = "禾吉辰網站聯絡來信 由 :"+ t.t2;
                message.Body = sr.ToHtmlString;

                String MailServer = System.Configuration.ConfigurationManager.AppSettings["MailServer"];
                SmtpClient smtpClient = new SmtpClient(MailServer, 25); //設定E-mail Server和port
                //SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                //top04244@yahoo.com.tw

                //smtpClient.Credentials = new System.Net.NetworkCredential("jerryissky@gmail.com", "");
                //smtpClient.EnableSsl = true;

                smtpClient.Send(message);

                return JsonConvert.SerializeObject(true, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "發生錯誤";
            }
        }
    }
}
