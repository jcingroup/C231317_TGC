using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

using ProcCore.Business.Logic;

namespace DotWeb._Code.Ashx
{
    public class AjaxGetProductKind : BaseIHttpHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            this.c = context;
            vmJsonResult r_json_data = null;
            a_ProductKind LBase = new a_ProductKind();

            try
            {
                LBase.Connection = getSQLConnection();
                r_json_data = new vmJsonResult() { result = true, data = LBase.SearchMaster(new q_ProductKind() {}, 0) };
                context.Response.Write(JsonConvert.SerializeObject(r_json_data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
            }
            catch (Exception ex)
            {
                r_json_data = new vmJsonResult() { result = false, message = ex.Message + ":" + ex.StackTrace };
                context.Response.Write(JsonConvert.SerializeObject(r_json_data));
            }
        }
    }
}