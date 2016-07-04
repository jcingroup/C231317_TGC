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
    public class ProductDataController : BaseAction<m_ProductData, a_ProductData, q_ProductData, ProductData>
    {

        #region Action and function section
        public RedirectResult Index()
        {
            return Redirect(Url.Action("ListGrid"));
        }
        public override ActionResult ListGrid(q_ProductData sh)
        {
            #region GetKinds
            //gg
            a_ProductKind c = new a_ProductKind() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            RunQueryPackage<m_ProductKind> hResult = c.SearchMaster(new q_ProductKind(), LoginUserId);
            HandleResultCheck(hResult);
            List<SelectListItem> All_Kinds = new List<SelectListItem>();
            List<SelectListItem> New_Kinds = new List<SelectListItem>();
            List<SelectListItem> Second_Kinds = new List<SelectListItem>();
            Second_Kinds.Add(new SelectListItem() { Text = "不分類", Value = null });
            New_Kinds.Add(new SelectListItem() { Text = "不分類", Value = null });
            foreach (var item in hResult.SearchData)
            {
                All_Kinds.Add(new SelectListItem() { Text = item.Name, Value = item.ID.ToString() });
                if (item.IsSecond == true)
                    Second_Kinds.Add(new SelectListItem() { Text = item.Name, Value = item.ID.ToString() });
                else
                    New_Kinds.Add(new SelectListItem() { Text = item.Name, Value = item.ID.ToString() });
            }
            ViewBag.NewKind_Option = New_Kinds;
            ViewBag.SecondKind_Option = Second_Kinds;
            #endregion

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
            #region GetKinds
            //gg
            a_ProductKind c = new a_ProductKind() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            RunQueryPackage<m_ProductKind> hResult = c.SearchMaster(new q_ProductKind(), LoginUserId);
            HandleResultCheck(hResult);
            List<SelectListItem> All_Kinds = new List<SelectListItem>();
            List<SelectListItem> New_Kinds = new List<SelectListItem>();
            List<SelectListItem> Second_Kinds = new List<SelectListItem>();
            foreach (var item in hResult.SearchData.Where(x => x.Series == 12))//預設為 新進商品 SID=12
            {
                All_Kinds.Add(new SelectListItem() { Text = item.Name, Value = item.ID.ToString() });
                if (item.IsSecond == true)
                    Second_Kinds.Add(new SelectListItem() { Text = item.Name, Value = item.ID.ToString() });
                else
                    New_Kinds.Add(new SelectListItem() { Text = item.Name, Value = item.ID.ToString() });
            }
            ViewBag.NewKind_Option = New_Kinds;
            ViewBag.SecondKind_Option = Second_Kinds;
            ViewBag.AllKind_Option = All_Kinds;
            #endregion

            operationMode = OperationMode.EditInsert;
            HandleCollectDataToOptions();
            ViewBag.Caption = GetSystemInfo().prog_name;
            HandleRequest HRq = new HandleRequest();  //記錄QueryString
            HRq.Remove("Id"); //不需記ID
            ViewBag.QueryString = HRq.ToQueryString();
            HRq = null;
            ac = new a_ProductData() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            return View("EditData", new m_ProductData()
            {
                ID = ac.GetIDX(),
                IsSecond = true,
                EditType = EditModeType.Insert,
                IsOpen = true
            });
        }
        public override ActionResult EditMasterDataByID(int id)
        {


            #region GetKinds
            //gg
            a_ProductKind c = new a_ProductKind() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            RunQueryPackage<m_ProductKind> hResult = c.SearchMaster(new q_ProductKind(), LoginUserId);
            HandleResultCheck(hResult);
            List<SelectListItem> New_Kinds = new List<SelectListItem>();
            List<SelectListItem> Second_Kinds = new List<SelectListItem>();
            #endregion

            operationMode = OperationMode.EditModify;
            ac = new a_ProductData() { Connection = getSQLConnection(), logPlamInfo = plamInfo };

            RunOneDataEnd<m_ProductData> HResult = ac.GetDataMaster(id, LoginUserId);
            md = HResult.SearchData;

            int sid = hResult.SearchData.Where(m_ProductData => m_ProductData.ID == md.Kind).First().Series;
            foreach (var item in hResult.SearchData.Where(x => x.Series == sid))
            {
                if (item.IsSecond == true)
                    Second_Kinds.Add(new SelectListItem() { Text = item.Name, Value = item.ID.ToString() });
                else
                    New_Kinds.Add(new SelectListItem() { Text = item.Name, Value = item.ID.ToString() });
            }
            ViewBag.NewKind_Option = New_Kinds;
            ViewBag.SecondKind_Option = Second_Kinds;

            md.Series = sid;
            md.EditType = EditModeType.Modify;
            HandleResultCheck(HResult);
            HandleCollectDataToOptions();

            ViewBag.Caption = GetSystemInfo().prog_name;

            HandleRequest HRq = new HandleRequest();  //記錄QueryString
            HRq.Remove("id"); //不需記ID
            ViewBag.QueryString = HRq.ToQueryString();
            ViewBag.id = id;
            HRq = null;

            return View("EditData", md);
        }

        public string getOption(int sid, bool is_second)
        {
            #region GetKinds
            //gg
            a_ProductKind c = new a_ProductKind() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            RunQueryPackage<m_ProductKind> hResult = c.SearchMaster(new q_ProductKind(), LoginUserId);
            HandleResultCheck(hResult);
            List<SelectListItem> New_Kinds = new List<SelectListItem>();
            List<SelectListItem> Second_Kinds = new List<SelectListItem>();
            #endregion

            foreach (var item in hResult.SearchData.Where(x => x.Series == sid))
            {
                if (item.IsSecond == true)
                    Second_Kinds.Add(new SelectListItem() { Text = item.Name, Value = item.ID.ToString() });
                else
                    New_Kinds.Add(new SelectListItem() { Text = item.Name, Value = item.ID.ToString() });
            }

            string getjson = string.Empty;
            if (is_second)
                getjson = JsonConvert.SerializeObject(Second_Kinds);
            else
                getjson = JsonConvert.SerializeObject(New_Kinds);


            return getjson;

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
        public String ajax_MasterUpdata(m_ProductData md)
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            String returnPicturePath = String.Empty;

            ac = new a_ProductData() { Connection = getSQLConnection(), logPlamInfo = plamInfo }; ;

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
            ac = new a_ProductData() { Connection = getSQLConnection(), logPlamInfo = plamInfo }; ;

            RunDeleteEnd HResult = ac.DeleteMaster(id.Split(',').CInt(), LoginUserId);
            rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Delete_Success);
            return JsonConvert.SerializeObject(rAjaxResult, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }

        [HttpGet]
        public override String ajax_MasterGridData(q_ProductData queryObj)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料
            ac = new a_ProductData() { Connection = getSQLConnection(), logPlamInfo = plamInfo }; ;

            RunQueryPackage<m_ProductData> HResult = ac.SearchMaster(queryObj, LoginUserId);
            HandleResultCheck(HResult);
            #endregion
            #region 設定 Page物件 頁數 總筆數 每頁筆數
            int page = (queryObj.page == null ? 1 : queryObj.page.CInt());
            int startRecord = PageCount.PageInfo(page, this.DefPageSize, HResult.Count);
            #endregion
            #region 每行及每個欄位資料組成
            List<RowElement> setRowElement = new List<RowElement>();
            var Modules = HResult.SearchData.Skip(startRecord).Take(this.DefPageSize);

            a_ProductKind c = new a_ProductKind() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            RunQueryPackage<m_ProductKind> hResult = c.SearchMaster(new q_ProductKind(), LoginUserId);
            var Series = new String[] { "", "屏風系列", "辦公桌系列", "會議桌系列", "檔案櫃系列", "主管辦公桌系列", "辦公椅系列", "沙發系列", "", "", "其他商品", "", "新進商品", "特價商品" };
            foreach (m_ProductData md in Modules)
            {
                List<String> setFields = new List<String>(10);
                //JQGrid Col
                int sid = 0;
                if (hResult.SearchData.AsQueryable().Where(m_ProductData => m_ProductData.ID == md.Kind).Any())
                    sid = hResult.SearchData.AsQueryable().Where(m_ProductData => m_ProductData.ID == md.Kind).First().Series;

                setFields.Add(md.ID.ToString());
                setFields.Add(md.Name);
                setFields.Add(md.Price);
                setFields.Add(md.IsOpen.BooleanValue(BooleanSheet.yn));
                setFields.Add(Series[sid]);
                setFields.Add(hResult.SearchData.AsQueryable().Where(m_ProductData => m_ProductData.ID == md.Kind).First().Name);
                setFields.Add(md.IsSecond.BooleanValue("二手", "全新"));
                setFields.Add(md.IsOnSell.BooleanValue(BooleanSheet.yn));
                setFields.Add(md.IsDisp.BooleanValue(BooleanSheet.yn));
                setFields.Add(md.IsNew.BooleanValue(BooleanSheet.yn));
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

        //MasterTreeNode
        //[HttpPost]
        //public String ajax_MasterTreeData(Int32? id)
        //{
        //    if (id == null)
        //        id = 0;

        //    #region 連接BusinessLogicLibary資料庫並取得資料
        //    a_ProductMainKind ac = new a_ProductMainKind() { Connection = getSQLConnection(), logPlamInfo = this.plamInfo };
        //    RunQueryPackage<m_ProductMainKind> HResult = ac.SearchMaster(new q_ProductMainKind() { s_id_parent = id }, LoginUnitId);
        //    HandleResultCheck(HResult);

        //    var Modules = HResult.SearchData;
        //    List<treeNode> tN = new List<treeNode>();
        //    foreach (m_ProductMainKind md in Modules)
        //    {
        //        treeNode dataObj = new treeNode()
        //        {
        //            id = md.id,
        //            name = md.name,
        //            pId = md.id_parent,
        //            isParent = md.is_folder
        //        };
        //        tN.Add(dataObj);
        //    }
        //    #endregion

        //    #region 回傳JSON資料
        //    return JsonConvert.SerializeObject(tN.ToArray(), new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        //    #endregion
        //}

        //[HttpPost]
        //public String ajax_Exist_SN(String sn)
        //{
        //    #region 連接BusinessLogicLibary資料庫並取得資料
        //    ac = new a_Product();
        //    ac.Connection = this.getSQLConnection();

        //    return JsonConvert.SerializeObject(ac.Exists_SN(sn, LoginUserId), new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        //    #endregion
        //}
        #endregion

        #region ajax file upload handle
        [HttpPost]
        [ValidateInput(false)]
        public string ajax_UploadFine(int Id, String FilesKind, String hd_FileUp_EL)
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

                    if (FilesKind == "ListMore")
                    {
                        HandImageSave(hd_FileUp_EL, Id, ImageFileUpParm.ProductMore, FilesKind);
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
        public string ajax_ListFiles(int Id, String FileKind)
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            rAjaxResult.filesObject = ListSysFiles(Id, FileKind);
            rAjaxResult.result = true;
            return JsonConvert.SerializeObject(rAjaxResult, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }

        [HttpPost]
        [ValidateInput(false)]
        public string ajax_DeleteFiles(int Id, String FileKind, String FileName)
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            DeleteSysFile(Id, FileKind, FileName, ImageFileUpParm.NewsBasic);
            rAjaxResult.result = true;
            return JsonConvert.SerializeObject(rAjaxResult, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
        #endregion
    }
}
