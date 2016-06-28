using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ProcCore;
using ProcCore.WebCore;
using ProcCore.NetExtension;
using ProcCore.Business.Logic;
using ProcCore.Business.Logic.TablesDescription;
using ProcCore.ReturnAjaxResult;
using ProcCore.JqueryHelp.JQGridScript;
using DotWeb.CommSetup;
using Newtonsoft.Json;
using ProcCore.JqueryHelp.ztreeHelp;

namespace DotWeb.Areas.Sys_Active.Controllers
{
    public class ProductKindController : BaseAction<m_ProductKind, a_ProductKind, q_ProductKind, ProductKind>
    {
        #region Action and function section

        public RedirectResult Index()
        {
            return Redirect(Url.Action("ListGrid"));
        }
        public override ActionResult ListGrid(q_ProductKind sh)
        {
            operationMode = OperationMode.EnterList;
            HandleRequest HRq = new HandleRequest(); //記錄QueryString            
            HRq.encodeURIComponent = true;
            HRq.Remove("page");

            ViewBag.Page = QueryGridPage();
            ViewBag.Caption = GetSystemInfo().prog_name;
            ViewBag.AppendQuertString = HRq.ToQueryString();
            HRq = null;

            return View("ListData", sh);
        }
        public override ActionResult EditMasterNewData()
        {
            operationMode = OperationMode.EditInsert;
            HandleCollectDataToOptions();
            ViewBag.Caption = GetSystemInfo().prog_name;
            HandleRequest HRq = new HandleRequest();  //記錄QueryString
            HRq.Remove("Id"); //不需記ID
            ViewBag.QueryString = HRq.ToQueryString();
            HRq = null;
            ac = new a_ProductKind() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            return View("EditData", new m_ProductKind()
            {
                ID = ac.GetIDX(), 
                EditType = EditModeType.Insert,
                IsOpen =true
            });
        }
        public override ActionResult EditMasterDataByID(int id)
        {
            operationMode = OperationMode.EditModify;
            ac = new a_ProductKind() { Connection = getSQLConnection(), logPlamInfo = plamInfo }; ;

            RunOneDataEnd<m_ProductKind> HResult = ac.GetDataMaster(id, LoginUserId);
            md = HResult.SearchData;
            md.EditType = EditModeType.Modify;
            HandleResultCheck(HResult);
            HandleCollectDataToOptions();

            ViewBag.Caption = GetSystemInfo().prog_name;

            HandleRequest HRq = new HandleRequest();  //記錄QueryString
            HRq.Remove("id"); //不需記ID
            ViewBag.QueryString = HRq.ToQueryString();
            HRq = null;

            return View("EditData", md);
        }
        protected override void HandleCollectDataToOptions()
        {
            ItemsManage i_Item = new ItemsManage() { Connection = getSQLConnection(), logPlamInfo = this.plamInfo };
            ViewBag.mainkind_Option = MakeCollectDataToOptions(i_Item.i_MainKind(LoginUserId), false);
            ViewBag.subkind_Option = MakeCollectDataToOptions(new Dictionary<String, String>(), false);
        }
        #endregion

        #region ajax call section
        [HttpPost]
        [ValidateInput(false)]
        public String ajax_MasterUpdata(m_ProductKind md)
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            String returnPicturePath = String.Empty;

            ac = new a_ProductKind() { Connection = getSQLConnection(), logPlamInfo = plamInfo }; ;

            if (md.EditType == EditModeType.Insert)
            {   //新增
                RunInsertEnd HResult = this.ac.InsertMaster(md, LoginUserId);
                rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Insert_Success);
                rAjaxResult.id = HResult.InsertId;
            }
            else
            {
                //修改
                RunEnd HResult = this.ac.UpdateMaster(md, LoginUserId);
                rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Update_Success);
                rAjaxResult.id = md.ID;
            }
            return JsonConvert.SerializeObject(rAjaxResult, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }

        [HttpPost]
        public override String ajax_MasterDeleteData(String id)
        {
            String returnString = string.Empty;

            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            ac = new a_ProductKind() { Connection = getSQLConnection(), logPlamInfo = plamInfo }; ;

            RunDeleteEnd HResult = ac.DeleteMaster(id.Split(',').CInt(), LoginUserId);
            rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Delete_Success);
            return JsonConvert.SerializeObject(rAjaxResult, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }

        [HttpGet]
        public override String ajax_MasterGridData(q_ProductKind queryObj)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料
            ac = new a_ProductKind() { Connection = getSQLConnection(), logPlamInfo = plamInfo }; ;

            RunQueryPackage<m_ProductKind> HResult = ac.SearchMaster(queryObj, LoginUserId);
            HandleResultCheck(HResult);
            #endregion
            #region 設定 Page物件 頁數 總筆數 每頁筆數
            int page = (queryObj.page == null ? 1 : queryObj.page.CInt());
            int startRecord = PageCount.PageInfo(page, this.DefPageSize, HResult.Count);
            #endregion
            #region 每行及每個欄位資料組成
            List<RowElement> setRowElement = new List<RowElement>();
            var Modules = HResult.SearchData.Skip(startRecord).Take(this.DefPageSize);
            var Series =new String[] { "", "屏風系列", "辦公桌系列", "會議桌系列", "檔案櫃系列", "主管辦公桌系列", "辦公椅系列", "沙發系列","","","其他商品" };
            foreach (m_ProductKind md in Modules)
            {
                List<String> setFields = new List<String>(6);

                setFields.Add(md.ID.ToString());
                setFields.Add(Series[md.Series]);
                setFields.Add(md.Name);
                setFields.Add(md.IsOpen.BooleanValue(BooleanSheet.yn));
                setFields.Add(md.IsSecond.BooleanValue("二手","全新"));
                setFields.Add(md.Sort.ToString());
                

                setRowElement.Add(new RowElement() { id = md.ID.ToString(), cell = setFields.ToArray() });
            }
            #endregion
            #region 回傳JSON資料

            return JsonConvert.SerializeObject(new JQGridDataObject()
            {
                rows = setRowElement.ToArray(),
                total = PageCount.TotalPage,
                page = PageCount.Page,
                records = PageCount.RecordCount
            }, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            #endregion
        }

        #endregion

        #region ajax file upload handle
        [HttpPost]
        [ValidateInput(false)]
        public String ajax_UploadFine(int Id, String FilesKind, String hd_FileUp_EL)
        {
            //hd_FileUp_EL
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();

            #region
            String tpl_File = String.Empty;
            try
            {
                //判斷是否為圖片檔
                if (!IMGExtDef.Any(x => x == hd_FileUp_EL.GetFileExt()))
                {
                    HandFineSave(hd_FileUp_EL, Id, new FilesUpScope(), FilesKind, false);
                    rAjaxResult.result = true;
                    rAjaxResult.success = true;
                    rAjaxResult.FileName = hd_FileUp_EL.GetFileName();
                }
                else
                {
                    if (FilesKind == "ContexImg")
                    {
                        HandImageSave(hd_FileUp_EL, Id, ImageFileUpParm.ProductContext, FilesKind);
                        rAjaxResult.result = true;
                        rAjaxResult.success = true;
                        rAjaxResult.FileName = hd_FileUp_EL.GetFileName();
                    }

                    if (FilesKind == "ListIcon")
                    {
                        HandImageSave(hd_FileUp_EL, Id, ImageFileUpParm.NewsBasic, FilesKind);
                        rAjaxResult.result = true;
                        rAjaxResult.success = true;
                        rAjaxResult.FileName = hd_FileUp_EL.GetFileName();
                    }
                }
            }
            catch (LogicError ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.success = false;
                rAjaxResult.error = GetRecMessage(ex.Message);
            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.success = false;
                rAjaxResult.error = ex.Message;
            }
            #endregion
            return JsonConvert.SerializeObject(rAjaxResult, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }

        [HttpPost]
        [ValidateInput(false)]
        public String ajax_ListFiles(int Id, String FileKind)
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            rAjaxResult.filesObject = ListSysFiles(Id, FileKind);
            rAjaxResult.result = true;
            return JsonConvert.SerializeObject(rAjaxResult, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }

        [HttpPost]
        [ValidateInput(false)]
        public String ajax_DeleteFiles(int Id, String FileKind, String FileName)
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            DeleteSysFile(Id, FileKind, FileName, ImageFileUpParm.NewsBasic);
            rAjaxResult.result = true;
            return JsonConvert.SerializeObject(rAjaxResult, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
        #endregion
    }
}
