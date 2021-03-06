﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

using ProcCore.WebCore;
using ProcCore.JqueryHelp.JQGridScript;
using ProcCore.JqueryHelp;
using ProcCore.NetExtension;
using ProcCore.Business.Logic;
using ProcCore.Business.Logic.TablesDescription;
using ProcCore.ReturnAjaxResult;
using Newtonsoft.Json;

namespace DotWeb.Areas.Sys_Base.Controllers
{
    public class PowerUnitController : BaseController
    {
        ProgData Tab;
        a_PowerUnit ac;

        public PowerUnitController()
        {
            Tab = new ProgData();
        }

        public RedirectResult Index()
        {
            return Redirect(Url.Action("ListGrid"));
        }

        public ActionResult ListGrid(q_ProgData sh)
        {
            HandleRequest HRq = new HandleRequest();  //記錄QueryString
            HRq.encodeURIComponent = true;

            ac = new a_PowerUnit() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            ViewBag.Caption = GetSystemInfo().prog_name;
            CreatGridInitScript();
            HandleCollectDataToOptions();

            return View("EditData");
        }

        protected void CreatGridInitScript()
        {
            List<String> n = new List<string>();
            List<jqGrid.colObject> m = new List<jqGrid.colObject>();

            //n.Add("程式名稱");
            m.Add(new jqGrid.colObject() { name = "prog", width = "180", sortable = false });

            PowerManagement pCollect = new PowerManagement(0);

            foreach (Power p in pCollect.Powers)
            {
                n.Add(p.name.ToString());
                m.Add(new jqGrid.colObject()
                {
                    name = p.name.ToString(),
                    align = "center",
                    resizable = false,
                    formatter = new funcMethodModule() { funcName = "$.fn.MakeCheckOBJ" }
                });
            }

            //ViewData["array_ColNames"] = n.ToArray();
            ViewData["array_ColModel"] = m.ToArray();
        }

        protected void HandleCollectDataToOptions()
        {
            var a = ac.CollectKeyValueData_Unit().Where(x => !x.Key.Contains("1")).ToDictionary(x => x.Key, y => y.Value);
            ViewBag.Unit_Option = MakeCollectDataToOptions(a, true);
        }

        [HttpPost]
        public String ajax_UpdateUnitData(m_PowerUnit md)
        {
            ReturnAjaxInfo rAjaxResult = new ReturnAjaxInfo();

            a_PowerUnit ac = new a_PowerUnit() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            RunUpdateEnd r = ac.UpdateMaster(md, LoginUserId.ToString());

            if (!r.Result)
                rAjaxResult.message = r.Message;
            return JsonConvert.SerializeObject(rAjaxResult, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }

        [HttpPost]
        public String ajax_MasterGridData(q_PowerUnit sh)
        {
            a_PowerUnit ac = new a_PowerUnit() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            var r = ac.SearchMaster(sh, LoginUserId);

            HandleResultAjaxFiles(r, Resources.Res.NoMessage);

            int page = (sh.page == null ? 1 : sh.page.CInt()); //int.Parse(getPage);

            int startRecord = PageCount.PageInfo(page, this.DefPageSize, r.Count);

            JQGridDataObject dataObject = new JQGridDataObject();
            List<RowElement> setRowElement = new List<RowElement>();


            foreach (var v in r.SearchData)
            {
                RowElement re = new RowElement();

                re.id = v.progid.ToString();
                re.cell = new String[8];
                re.cell[0] = v.progname;

                for (int i = 0; i < v.Powers.Count; i++)
                    re.cell[i + 1] = JsonConvert.SerializeObject(v.Powers[i], new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

                setRowElement.Add(re);
            }

            dataObject.rows = setRowElement.ToArray();
            dataObject.total = PageCount.TotalPage;
            dataObject.page = PageCount.Page;
            dataObject.records = PageCount.RecordCount;
            return JsonConvert.SerializeObject(dataObject, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}
