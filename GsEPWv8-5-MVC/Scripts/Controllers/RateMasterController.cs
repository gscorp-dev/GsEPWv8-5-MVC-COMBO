using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GsEPWv8_4_MVC.Business.Implementation;
using GsEPWv8_4_MVC.Business.Interface;
using GsEPWv8_4_MVC.Core.Entity;
using GsEPWv8_4_MVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace GsEPWv8_4_MVC.Controllers
{
    #region Change History
    // CR#                         Modified By   Date         Description
    //CR_3PL_MVC_BL_2018_0220_001  Ravikumar     201-0220    To Fix the load Category based On the Type
    #endregion Change History
    public class RateMasterController : Controller
    {
        public ActionResult RateMaster(string FullFillType, string cmp)
        {

            string l_str_cmp_id = string.Empty;
            string l_str_tmp_cmp_id = string.Empty;
            string l_str_scn_id = string.Empty;  
            string l_str_success = string.Empty;                 // CR_3PL_MVC_COMMON_2018_0326_001            
            try
            {
                RateMaster objRateMaster = new RateMaster();
                IRateMasterService ServiceObject = new RateMasterService();
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
               
                            Session["g_str_Search_flag"] = "False";
                            objRateMaster.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                            if (objRateMaster.cmp_id == null || objRateMaster.cmp_id == string.Empty)
                            {
                                objRateMaster.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
                                //l_str_cmp_id = Session["dflt_cmp_id"].ToString();

                            }
                            else
                            {
                                objCompany.cmp_id = Session["g_str_cmp_id"].ToString().Trim();

                            }
                            // END CR_3PL_MVC_COMMON_2018_0326_001
                            if (FullFillType == null)
                            {
                                objCompany.user_id = Session["UserID"].ToString().Trim();
                                objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                                objRateMaster.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;

                            }
                            LookUp objLookUp = new LookUp();
                            LookUpService ServiceObject2 = new LookUpService();
                            objLookUp.id = "7";
                            objLookUp.lookuptype = "RATEMASTER";
                            objLookUp = ServiceObject2.GetLookUpValue(objLookUp);
                            objRateMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
                            Mapper.CreateMap<RateMaster, RateMasterModel>();
                            RateMasterModel objRateMasterModel = Mapper.Map<RateMaster, RateMasterModel>(objRateMaster);

                            return View(objRateMasterModel);
                       
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        [HttpGet]
        public ActionResult GetRateMasterDtls(string p_str_cmp_id, string p_str_type, string p_str_Rate_Id_Fm, string p_str_Rate_Id_To)
        {
            RateMaster objRateMaster = new RateMaster();
            IRateMasterService ServiceObject = new RateMasterService();
            objRateMaster.cmp_id = p_str_cmp_id;
            objRateMaster.type = p_str_type;
            objRateMaster.Rate_Id_Fm = p_str_Rate_Id_Fm;
            objRateMaster.Rate_Id_To = p_str_Rate_Id_To;
            TempData["cmp_id"] = objRateMaster.cmp_id;
            TempData["type"] = objRateMaster.type;
            TempData["Rate_Id_Fm"] = objRateMaster.itm_num;
            TempData["Rate_Id_To"] = objRateMaster.itm_num;
            Session["g_str_Search_flag"] = "True";////CR-180421-001 Added By Nithya
            //objVasInquiry = ServiceObject.VasInquiryGetCompanyDetails(objVasInquiry);
            objRateMaster = ServiceObject.GetRateMasterDetails(objRateMaster);
            var model = objRateMaster.ListRateMaster;
            GridView gv = new GridView();
            gv.DataSource = model;
            gv.DataBind();
            Session["Rates"] = gv;
            Mapper.CreateMap<RateMaster, RateMasterModel>();
            RateMasterModel objRateMasterModel = Mapper.Map<RateMaster, RateMasterModel>(objRateMaster);
            return PartialView("_RateMaster", objRateMasterModel);
        }
        public ActionResult ShowReport(string p_str_cmpid, string p_str_type, string p_str_Rate_Id_Fm, string p_str_Rate_Id_To)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("mm/dd/yyyy");
            string strToDate = DateTime.Now.ToString("mm/dd/yyyy");
            try
            {
                if (isValid)
                {

                    strReportName = "rpt_ma_rate_eds.rpt";
                    RateMaster objRateMaster = new RateMaster();
                    IRateMasterService ServiceObject = new RateMasterService();
                    ReportDocument rd = new ReportDocument();
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//RateMaster//" + strReportName;
                    objRateMaster.cmp_id = p_str_cmpid;
                    objRateMaster.type = p_str_type;
                    objRateMaster.itm_num = p_str_Rate_Id_Fm;
                    objRateMaster.itm_num = p_str_Rate_Id_To;
                    objRateMaster = ServiceObject.GetRateMasterRptDetails(objRateMaster);
                    var rptSource = objRateMaster.ListRateMasterRpt.ToList();
                    rd.Load(strRptPath);
                    int AlocCount = 0;
                    AlocCount = objRateMaster.ListRateMasterRpt.Count();
                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                        rd.SetDataSource(rptSource);
                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                }
                else
                {
                    Response.Write("<H2>Report not found</H2>");
                }
            }

            catch (Exception ex)
            {
                msg = ex.Message;
                jsonErrorCode = "-2";
            }

            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult RefreshReport()
        {
            return PartialView("_ReportsDisplay");
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ReportView()
        {
            return View();
        }
        public ActionResult Views(string cmpid, string Itmnum)
        {
            RateMaster objRateMaster = new RateMaster();
            RateMasterService serviceobject = new RateMasterService();
            objRateMaster.cmp_id = cmpid;
            objRateMaster.itm_num = Itmnum;
            objRateMaster = serviceobject.GetRateMasterViewDetails(objRateMaster);
            objRateMaster.catg = objRateMaster.ListRateMasterViewDtl[0].catg;
            objRateMaster.type = objRateMaster.ListRateMasterViewDtl[0].type;
            objRateMaster.status = objRateMaster.ListRateMasterViewDtl[0].status;
            objRateMaster.itm_name = objRateMaster.ListRateMasterViewDtl[0].itm_name;
            objRateMaster.list_price = Math.Round(Convert.ToDecimal(objRateMaster.ListRateMasterViewDtl[0].list_price), 2);
            objRateMaster.price_uom = objRateMaster.ListRateMasterViewDtl[0].price_uom;
            objRateMaster.last_so_dt = objRateMaster.ListRateMasterViewDtl[0].last_so_dt;
            objRateMaster = serviceobject.GetRateMasterViewDetails(objRateMaster);
            Mapper.CreateMap<RateMaster, RateMasterModel>();
            RateMasterModel objRateMastermodel = Mapper.Map<RateMaster, RateMasterModel>(objRateMaster);
            return PartialView("_RateView", objRateMastermodel);
        }

        //CR_3PL_MVC_BL_2018_0220_001 Added By Ravi
        public ActionResult Edit(string cmpid, string Itmnum, string staus, string Type, string catg)
        {
            RateMaster objRateMaster = new RateMaster();
            RateMasterService serviceobject = new RateMasterService();
            LookUp objLookUp = new LookUp();
            LookUpService ServiceObject1 = new LookUpService();
            Price objPrice = new Price();
            PriceuomService ServiceObject2 = new PriceuomService();
            objRateMaster.cmp_id = cmpid.Trim();
            objRateMaster.itm_num = Itmnum.Trim();

            objRateMaster = serviceobject.GetRateMasterViewDetails(objRateMaster);
            objRateMaster.catg = catg.Trim();
            objLookUp.id = "8";
            objLookUp.lookuptype = "VAS";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objRateMaster.ListLookUpCategoryDtl = objLookUp.ListLookUpDtl;
            
            objPrice.uom_type = "Quantity";
            objPrice = ServiceObject2.GetPriceuomDetails(objPrice);
            objRateMaster.ListPriceuom = objPrice.ListPriceuom;
            objRateMaster.price_uom = objRateMaster.ListRateMasterViewDtl[0].price_uom;

            objRateMaster.type = Type.Trim();
            objLookUp.id = "7";
            objLookUp.lookuptype = "RATEMASTER";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objRateMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;

            objRateMaster.status = staus.Trim();
            objLookUp.id = "11";
            objLookUp.lookuptype = "RATESTATUS";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objRateMaster.ListLookUpStatusDtl = objLookUp.ListLookUpDtl;
            objRateMaster.itm_name = objRateMaster.ListRateMasterViewDtl[0].itm_name;
            objRateMaster.list_price = Math.Round(Convert.ToDecimal(objRateMaster.ListRateMasterViewDtl[0].list_price), 2);          
            objRateMaster.last_so_dt = objRateMaster.ListRateMasterViewDtl[0].last_so_dt;
            objRateMaster = serviceobject.GetRateMasterViewDetails(objRateMaster);
            objLookUp.lookuptype = Type.Trim();
            objLookUp = serviceobject.GetRateMasterCategory(objLookUp);
            objRateMaster.ListLookUpCategoryDtl = objLookUp.ListLookUpCategoryDtl;
            Mapper.CreateMap<RateMaster, RateMasterModel>();
            RateMasterModel objRateMastermodel = Mapper.Map<RateMaster, RateMasterModel>(objRateMaster);
            return PartialView("_RateEdit", objRateMastermodel);
        }
        //END
        public ActionResult UpdateRateMaster(string p_str_cmp_id, string p_str_itm_num, string p_str_type, string p_str_catg, string p_str_status, string p_str_itm_name, decimal p_str_list_price, string p_str_price_uom, string p_str_last_so_dt)
        {
            RateMaster objRateMaster = new RateMaster();
            RateMasterService serviceobject = new RateMasterService();
            objRateMaster.cmp_id = p_str_cmp_id.Trim();
            objRateMaster.itm_num = p_str_itm_num.Trim();
            objRateMaster.type = p_str_type.Trim();
            objRateMaster.catg = p_str_catg.Trim();
            objRateMaster.status = p_str_status.Trim();
            objRateMaster.itm_name = p_str_itm_name.Trim();
            objRateMaster.list_price = p_str_list_price;
            objRateMaster.price_uom = p_str_price_uom.Trim();
            objRateMaster.last_so_dt = p_str_last_so_dt.Trim();
            objRateMaster.opt_id = "M";
            serviceobject.RateMasterCreateUpdate(objRateMaster);
            Mapper.CreateMap<RateMaster, RateMasterModel>();
            RateMasterModel objRateMastermodel = Mapper.Map<RateMaster, RateMasterModel>(objRateMaster);
            return View("~/Views/RateMaster/RateMaster.cshtml", objRateMastermodel);
        }
        public ActionResult InsertRateMaster(string p_str_cmp_id, string p_str_itm_num, string p_str_type, string p_str_catg, string p_str_status, string p_str_itm_name, decimal p_str_list_price, string p_str_price_uom, string p_str_last_so_dt)
        {
            RateMaster objRateMaster = new RateMaster();
            RateMasterService serviceobject = new RateMasterService();
            objRateMaster.cmp_id = p_str_cmp_id.Trim();
            objRateMaster.itm_num = p_str_itm_num.Trim();
            objRateMaster.type = p_str_type.Trim();
            objRateMaster.catg = p_str_catg.Trim();
            objRateMaster.status = p_str_status.Trim();
            objRateMaster.itm_name = p_str_itm_name.Trim();
            objRateMaster.list_price = p_str_list_price;
            objRateMaster.price_uom = p_str_price_uom.Trim();
            objRateMaster.last_so_dt = p_str_last_so_dt.Trim();
            serviceobject.RateMasterCreateUpdate(objRateMaster);
            Mapper.CreateMap<RateMaster, RateMasterModel>();
            RateMasterModel objRateMastermodel = Mapper.Map<RateMaster, RateMasterModel>(objRateMaster);
            return View("~/Views/RateMaster/RateMaster.cshtml", objRateMastermodel);
        }
        public ActionResult NewRateMaster(string p_str_cmp_id, string p_str_itm_num, string p_str_type, string p_str_catg, string p_str_status, string p_str_itm_name, decimal p_str_list_price, string p_str_price_uom, string p_str_last_so_dt)
        {
          
            RateMaster objRateMaster = new RateMaster();
            RateMasterService serviceobject = new RateMasterService();
            objRateMaster.cmp_id = p_str_cmp_id.Trim();
            objRateMaster.itm_num = p_str_itm_num.Trim();
            objRateMaster.type = p_str_type.Trim();
            objRateMaster.catg = p_str_catg.Trim();
            objRateMaster.status = p_str_status.Trim();
            objRateMaster.itm_name = p_str_itm_name.Trim();
            objRateMaster.list_price = p_str_list_price;
            objRateMaster.price_uom = p_str_price_uom.Trim();
            objRateMaster.last_so_dt = p_str_last_so_dt.Trim();
            objRateMaster.opt_id = "A";
            //CR-180421-001 Added By Nithya
            objRateMaster = serviceobject.ExistRate(objRateMaster);
            if (objRateMaster.LstRateId.Count > 0)
            {
                int l_str_RateId = 0;
                return Json(l_str_RateId, JsonRequestBehavior.AllowGet);
            }
            //END
            serviceobject.RateMasterCreateUpdate(objRateMaster);           
            Mapper.CreateMap<RateMaster, RateMasterModel>();
            RateMasterModel objRateMastermodel = Mapper.Map<RateMaster, RateMasterModel>(objRateMaster);
            return View("~/Views/RateMaster/RateMaster.cshtml", objRateMastermodel);


        }

        public ActionResult Add(string cmpid, string Itmnum)
        {
            RateMaster objRateMaster = new RateMaster();
            RateMasterService serviceobject = new RateMasterService();

            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            Price objPrice = new Price();
            PriceuomService ServiceObject2 = new PriceuomService();
            //objRateMaster.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
            objRateMaster.cmp_id = cmpid;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objRateMaster.last_so_dt = DateTime.Now.ToString("MM/dd/yyyy");
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objRateMaster.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            //objRateMaster.last_so_dt = DateTime.Now.ToString("MM/dd/yyyy");
            LookUp objLookUp = new LookUp();
            LookUpService ServiceObject1 = new LookUpService();
            objLookUp.id = "7";
            objLookUp.lookuptype = "RATEMASTER";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objRateMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
            objLookUp.id = "8";
            objLookUp.lookuptype = "VAS";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objRateMaster.ListLookUpCategoryDtl = objLookUp.ListLookUpDtl;
            objLookUp.id = "11";
            objLookUp.lookuptype = "RATESTATUS";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objRateMaster.ListLookUpStatusDtl = objLookUp.ListLookUpDtl;
            objPrice.uom_type = "Quantity";
            objPrice = ServiceObject2.GetPriceuomDetails(objPrice);
            objRateMaster.ListPriceuom = objPrice.ListPriceuom;

            Mapper.CreateMap<RateMaster, RateMasterModel>();
            RateMasterModel objRateMastermodel = Mapper.Map<RateMaster, RateMasterModel>(objRateMaster);
            return PartialView("_RateAdd", objRateMastermodel);
        }
        [HttpPost]
        public ActionResult CreateUpdateRateMaster(string p_str_cmp_id, string p_str_catg, string p_str_itm_number, string p_str_list_price, string p_str_last_so_dt, string p_str_status, string p_str_price_uom)
        {
            RateMaster objRateMaster = new RateMaster();
            //string temp = string.Empty;
            //if (p_str_itm_number != null)
            //{
            //    objRateMaster.cmp_id = Convert.ToString(p_str_itm_number);
            //    temp = "Edit";
            //}
            IRateMasterService ServiceObject = new RateMasterService();

            objRateMaster.cmp_id = p_str_cmp_id;
            objRateMaster.catg = p_str_catg;
            objRateMaster.itm_num = p_str_itm_number;
            //objRateMaster.list_price = p_str_list_price;
            objRateMaster.last_so_dt = p_str_last_so_dt;
            objRateMaster.status = p_str_status;
            objRateMaster.price_uom = p_str_price_uom;

            ServiceObject.RateMasterCreateUpdate(objRateMaster);
            Mapper.CreateMap<RateMaster, RateMasterModel>();
            RateMasterModel objRateMasterModel = Mapper.Map<RateMaster, RateMasterModel>(objRateMaster);
            return View("~/Views/RateMaster/RateMaster.cshtml", objRateMasterModel);



        }
        public ActionResult DeleteRateMaster(string Itmnum, string cmpid)
        {

            RateMaster objRateMaster = new RateMaster();
            IRateMasterService ServiceObject = new RateMasterService();
            int ResultCount;
            objRateMaster.itm_num = Itmnum.Trim();
            objRateMaster.cmp_id = cmpid.Trim();
            ServiceObject.RateMasterDelete(objRateMaster);
            ResultCount = 1;

            return Json(ResultCount, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Download()
        {
            if (Session["Rates"] != null)
            {
                return new DownloadFileActionResult((GridView)Session["Rates"], "RateMaster.xls");
            }
            else
            {
                return new JavaScriptResult();
            }
        }

        //CR_3PL_MVC_BL_2018_0220_001 Added By Ravi
        [HttpPost]
        public ActionResult TypeChange(string id)
        {
            LookUp objRateMaster = new LookUp();
            IRateMasterService ServiceObject = new RateMasterService();
            objRateMaster.lookuptype = id;
            objRateMaster = ServiceObject.GetRateMasterCategory(objRateMaster);
            var serializer = new JavaScriptSerializer() { MaxJsonLength = 86753090 };
            // Perform your serialization
            serializer.Serialize(objRateMaster);
            return new JsonResult()
            {
                Data = objRateMaster,
                MaxJsonLength = 86753090
            };
            //return Json(objDashBoard, JsonRequestBehavior.AllowGet);

        }
        //END
        public JsonResult MASTER_INQ_HDR_DATA(string p_str_cmp_id, string p_str_type, string p_str_Rate_Id_Fm, string p_str_Rate_Id_To)
        {
            RateMaster objRateMaster = new RateMaster();
            IRateMasterService ServiceObject = new RateMasterService();
            Session["g_str_cmp_id"] = p_str_cmp_id.Trim();
            Session["TEMP_CMP_ID"] = p_str_cmp_id.Trim();
            Session["TEMP_RATE_TYPE"] = p_str_type.Trim();
            Session["TEMP_RATEID_FM"] = p_str_Rate_Id_Fm.Trim();
            Session["TEMP_RATEID_TO"] = p_str_Rate_Id_To.Trim();
            return Json(objRateMaster.MasterCount, JsonRequestBehavior.AllowGet);

        }
        public ActionResult RateMasterDtl(string p_str_cmp_id, string p_str_type,string p_str_rate_id_frm)
        {
            try
            {

                RateMaster objRateMaster = new RateMaster();
                IRateMasterService ServiceObject = new RateMasterService();
                string l_str_search_flag = string.Empty;
                string l_str_is_another_usr = string.Empty;

                //l_str_is_another_usr = Session["isanother"].ToString();
                //objRateMaster.IsAnotherUser = l_str_is_another_usr.Trim();
                l_str_search_flag = Session["g_str_Search_flag"].ToString().Trim();
                objRateMaster.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                if (l_str_search_flag == "True")
                {
                    //objRateMaster.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                    objRateMaster.cmp_id = Session["TEMP_CMP_ID"].ToString().Trim();
                    objRateMaster.type = Session["TEMP_RATE_TYPE"].ToString().Trim();//CR-180421-001 Added By Nithya
                    objRateMaster.Rate_Id_Fm = Session["TEMP_RATEID_FM"].ToString().Trim();
                    objRateMaster.Rate_Id_To = Session["TEMP_RATEID_TO"].ToString().Trim();

                }
                else
                {
                    objRateMaster.cmp_id = p_str_cmp_id;
                    objRateMaster.type = p_str_type;
                    objRateMaster.itm_num = p_str_rate_id_frm;


                }

                objRateMaster = ServiceObject.GetRateMasterDetails(objRateMaster);
                Mapper.CreateMap<RateMaster, RateMasterModel>();
                RateMasterModel objRateMasterModel = Mapper.Map<RateMaster, RateMasterModel>(objRateMaster);
                return PartialView("_RateMaster", objRateMasterModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}





