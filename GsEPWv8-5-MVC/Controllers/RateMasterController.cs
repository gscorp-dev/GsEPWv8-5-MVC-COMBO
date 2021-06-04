using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace GsEPWv8_5_MVC.Controllers
{
    #region Change History
    // CR#                         Modified By   Date         Description
    //CR_3PL_MVC_BL_2018_0220_001  Ravikumar     201-0220    To Fix the load Category based On the Type
    //CR20180825-001 Added By Nithya For Check RateId IS  IN USE
    #endregion Change History
    public class RateMasterController : Controller
    {
        public string ScreenID = "RateMaster Inquiry";
        public string l_str_rptdtl = string.Empty;
        public string strDateFormat= string.Empty;
        public string strFileName= string.Empty;
        public string reportFileName= string.Empty;
        public string Folderpath= string.Empty;
        public string l_str_tmp_name = string.Empty;
        public string p_str_report_selection_name = string.Empty;
        public bool l_str_include_entry_dtls;
        Email objEmail = new Email();
        EmailService objEmailService = new EmailService();
        Company objCompany = new Company();
        CompanyService ServiceObjectCompany = new CompanyService();
        LookUp objLookUp = new LookUp();
        LookUpService ServiceobjLookUp = new LookUpService();
        Price objPrice = new Price();
        PriceuomService ServiceObjectPrice = new PriceuomService();
        public ActionResult RateMaster(string FullFillType, string cmp)
        {

            string l_str_cmp_id = string.Empty;
            string l_str_tmp_cmp_id = string.Empty;
            string l_str_scn_id = string.Empty;  
            string l_str_success = string.Empty;                    
            try
            {
                RateMaster objRateMaster = new RateMaster();
                IRateMasterService ServiceObject = new RateMasterService();
                Session["g_str_Search_flag"] = "False";
                if (Session["g_str_cmp_id"].ToString() != null && Session["g_str_cmp_id"].ToString() != string.Empty)
                {
                    objRateMaster.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                    
                }
                else
                {
                    objRateMaster.cmp_id = string.Empty;

                }
                if (FullFillType == null)
                {
                    if (Session["UserID"].ToString() != null)
                    { 
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    }
                    else
                    {
                        objCompany.user_id = string.Empty;
                    }
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany, cmp);
                    objRateMaster.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;

                }

                objLookUp.id = "7";
                objLookUp.lookuptype = "RATEMASTER";
                objLookUp = ServiceobjLookUp.GetLookUpValue(objLookUp);
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
            objRateMaster.cmp_id = (p_str_cmp_id== null)?string.Empty: p_str_cmp_id.Trim();
            objRateMaster.type = (p_str_type == null) ? string.Empty : p_str_type.Trim();
            objRateMaster.Rate_Id_Fm = (p_str_Rate_Id_Fm == null) ? string.Empty : p_str_Rate_Id_Fm.Trim();
            objRateMaster.Rate_Id_To = (p_str_Rate_Id_To == null) ? string.Empty : p_str_Rate_Id_To.Trim();
            TempData["cmp_id"] = objRateMaster.cmp_id;
            TempData["type"] = objRateMaster.type;
            TempData["Rate_Id_Fm"] = objRateMaster.itm_num;
            TempData["Rate_Id_To"] = objRateMaster.itm_num;
            Session["g_str_Search_flag"] = "True";
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
        public ActionResult ShowReport(string p_str_cmpid, string p_str_Rate_Type, string p_str_Rate_Id_Fm, string p_str_Rate_Id_To,string p_str_report_selection_name)
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

                    if (p_str_report_selection_name == "RateMasterSummary")
                    { 
                    strReportName = "rpt_ma_rate_eds.rpt";
                    RateMaster objRateMaster = new RateMaster();
                    IRateMasterService ServiceObject = new RateMasterService();
              
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//RateMaster//" + strReportName;
                    objRateMaster.cmp_id = (p_str_cmpid == null) ? string.Empty : p_str_cmpid.Trim();
                    objRateMaster.type = (p_str_Rate_Type == null) ? string.Empty : p_str_Rate_Type.Trim();
                    objRateMaster.itm_num = (p_str_Rate_Id_Fm == null) ? string.Empty : p_str_Rate_Id_Fm.Trim();
                    objRateMaster.itm_num = (p_str_Rate_Id_To == null) ? string.Empty : p_str_Rate_Id_To.Trim();
                    objRateMaster = ServiceObject.GetRateMasterRptDetails(objRateMaster);
                    var rptSource = objRateMaster.ListRateMasterRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objRateMaster.ListRateMasterRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            }
                        }
                }
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
        public ActionResult EmailShowReport(string p_str_cmpid, string p_str_Rate_Type, string p_str_Rate_Id_Fm, string p_str_Rate_Id_To, string p_str_report_selection_name)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("mm/dd/yyyy");
            string strToDate = DateTime.Now.ToString("mm/dd/yyyy");
            objCompany.cmp_id = p_str_cmpid;
            if (Session["UserID"].ToString() != null)
            {
                objCompany.user_id = Session["UserID"].ToString().Trim();
            }
            else
            {
                objCompany.user_id = string.Empty;
            }
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            RateMaster objRateMaster = new RateMaster();
            IRateMasterService ServiceObject = new RateMasterService();
            objRateMaster.LstCmpName = objCompany.LstCmpName;
            if(objRateMaster.LstCmpName.Count>0)
            {
                l_str_tmp_name = objRateMaster.LstCmpName[0].cmp_name.ToString().Trim();
            }
            objEmail.CmpId = (p_str_cmpid==null)?string.Empty: p_str_cmpid.Trim();
            objEmail.screenId = (ScreenID == null) ? string.Empty : ScreenID.Trim();
            objEmail.username = (objCompany.user_id == null) ? string.Empty : objCompany.user_id.Trim();
            try
            {
                if (isValid)
                {
                    if (p_str_report_selection_name == "RateMasterSummary")
                    {
                        strReportName = "rpt_ma_rate_eds.rpt";

           
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//RateMaster//" + strReportName;
                        Folderpath = System.Configuration.ConfigurationManager.AppSettings["tempFilepath"].ToString().Trim();
                        objRateMaster.cmp_id = (p_str_cmpid == null) ? string.Empty : p_str_cmpid.Trim();
                        objEmail.CmpId = (p_str_cmpid == null) ? string.Empty : p_str_cmpid.Trim();
                        objEmail.screenId = (ScreenID == null) ? string.Empty : ScreenID.Trim();
                        objEmail.Reportselection = p_str_report_selection_name;
                        objEmail = objEmailService.GetSendMailDetails(objEmail);
                        if (objEmail.ListEamilDetail.Count != 0)
                        {
                            objEmail.EmailMessageContent = (objEmail.ListEamilDetail[0].EmailMessageContent== null || objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailMessageContent.Trim();
                        }
                        objRateMaster.type = (p_str_Rate_Type== null)?string.Empty: p_str_Rate_Type.Trim();
                        objRateMaster.itm_num = (p_str_Rate_Id_Fm == null) ? string.Empty : p_str_Rate_Id_Fm.Trim();
                        objRateMaster.itm_num = (p_str_Rate_Id_To == null) ? string.Empty : p_str_Rate_Id_To.Trim();
                        objRateMaster = ServiceObject.GetRateMasterRptDetails(objRateMaster);
                        if (objRateMaster.ListRateMasterRpt.Count > 0)
                        {
                            objRateMaster.itm_num = (objRateMaster.ListRateMasterRpt[0].itm_num == null || objRateMaster.ListRateMasterRpt[0].itm_num.Trim() == "" ? string.Empty : objRateMaster.ListRateMasterRpt[0].itm_num.Trim());
                            objRateMaster.itm_name = (objRateMaster.ListRateMasterRpt[0].itm_name == null || objRateMaster.ListRateMasterRpt[0].itm_name.Trim() == "" ? string.Empty : objRateMaster.ListRateMasterRpt[0].itm_name.Trim());
                            objRateMaster.last_so_dt = (objRateMaster.ListRateMasterRpt[0].last_so_dt == null || objRateMaster.ListRateMasterRpt[0].last_so_dt.Trim() == "" ? string.Empty : objRateMaster.ListRateMasterRpt[0].last_so_dt.Trim());
                            objRateMaster.price_uom= (objRateMaster.ListRateMasterRpt[0].price_uom == null || objRateMaster.ListRateMasterRpt[0].price_uom.Trim() == "" ? string.Empty : objRateMaster.ListRateMasterRpt[0].price_uom.Trim());
                            if (p_str_Rate_Type == "" || p_str_Rate_Type == null || p_str_Rate_Type == "-")
                            {
                                l_str_rptdtl = objRateMaster.cmp_id + "_" + "RateMaster Summary";
                                objEmail.EmailSubject = objRateMaster.cmp_id + "-" + "RateMaster Summary"+"|"+" "+" " + objRateMaster.price_uom;
                                objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objRateMaster.cmp_id + "-" + " " + " " + l_str_tmp_name+"\n" + "Price UOM: " + " " + " " + objRateMaster.price_uom;
                            }
                            else
                            {
                                l_str_rptdtl = objRateMaster.cmp_id + "_" + "RateMaster Summary" + "_" + objRateMaster.type;
                                objEmail.EmailSubject = objRateMaster.cmp_id + "-" + " " + "RateMaster Summary"+"|"+" "+ objRateMaster.price_uom+"|"+" "+ " "+ objRateMaster.type;
                                objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objRateMaster.cmp_id + "-" + " " + " " + l_str_tmp_name+"\n"+ "Price UOM: " + " " + " " + objRateMaster.price_uom+"\n"+"Type: " +" "+" " + objRateMaster.type;
                            }
                        }
                        var rptSource = objRateMaster.ListRateMasterRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objRateMaster.ListRateMasterRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            }
                        }
                        reportFileName = l_str_rptdtl + "_" + strDateFormat + ".pdf";
                        Session["RptFileName"] = strFileName;
                    }
                }

                else
                {
                    Response.Write("<H2>Report not found</H2>");
                }
                objEmail.CmpId = p_str_cmpid;
                if (objEmail.CmpId == null)
                {
                    objEmail.CmpId = "";
                }
                objEmail.Attachment = reportFileName;

                objEmail.screenId = (ScreenID== null)?string.Empty: ScreenID.Trim();
                objEmail.username = objCompany.user_id;
                objEmail.Reportselection = p_str_report_selection_name;
                objEmail = objEmailService.GetSendMailDetails(objEmail);
                if (objEmail.ListEamilDetail.Count != 0)
                {

                    objEmail.Attachment = reportFileName;
                    objEmail.EmailTo = (objEmail.ListEamilDetail[0].EmailTo == null || objEmail.ListEamilDetail[0].EmailTo.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailTo.Trim();
                    objEmail.EmailCC = (objEmail.ListEamilDetail[0].EmailCC== null || objEmail.ListEamilDetail[0].EmailCC.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailCC.Trim();
                    objEmail.EmailMessageContent = (objEmail.ListEamilDetail[0].EmailMessageContent== null || objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailMessageContent.Trim();


                }
                else
                {
                    objEmail.Attachment = reportFileName;
                    objEmail.EmailTo = "";
                    objEmail.EmailCC = "";

                }
                // CR_3PL_2018_0210_002 - End
                Mapper.CreateMap<Email, EmailModel>();
                EmailModel EmailModel = Mapper.Map<Email, EmailModel>(objEmail);

                return PartialView("_Email", EmailModel);
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
            objRateMaster.cmp_id = (cmpid== null)?string.Empty: cmpid.Trim();
            objRateMaster.itm_num = (Itmnum == null) ? string.Empty : Itmnum.Trim();
            objRateMaster = serviceobject.GetRateMasterViewDetails(objRateMaster);
            if (objRateMaster.ListRateMasterViewDtl.Count > 0)
            {
                objRateMaster.catg = objRateMaster.ListRateMasterViewDtl[0].catg;
                objRateMaster.type = objRateMaster.ListRateMasterViewDtl[0].type;
                objRateMaster.status = objRateMaster.ListRateMasterViewDtl[0].status;
                objRateMaster.itm_name = objRateMaster.ListRateMasterViewDtl[0].itm_name;
                objRateMaster.list_price = Math.Round(Convert.ToDecimal(objRateMaster.ListRateMasterViewDtl[0].list_price), 2);
                objRateMaster.price_uom = objRateMaster.ListRateMasterViewDtl[0].price_uom;
                objRateMaster.last_so_dt = objRateMaster.ListRateMasterViewDtl[0].last_so_dt;
            }
           // objRateMaster = serviceobject.GetRateMasterViewDetails(objRateMaster);
            Mapper.CreateMap<RateMaster, RateMasterModel>();
            RateMasterModel objRateMastermodel = Mapper.Map<RateMaster, RateMasterModel>(objRateMaster);
            return PartialView("_RateView", objRateMastermodel);
        }

        public ActionResult Edit(string cmpid, string Itmnum, string staus, string Type, string catg,string uom)
        {
            string l_str_ibs_unit = string.Empty;
            bool l_bln_is_auto_ibs = false;
            RateMaster objRateMaster = new RateMaster();
            RateMasterService serviceobject = new RateMasterService();
            objRateMaster.cmp_id = (cmpid == null) ? string.Empty : cmpid.Trim();
            objRateMaster.itm_num = (Itmnum == null) ? string.Empty : Itmnum.Trim();
            objRateMaster.price_uom =(uom== null)?string.Empty: uom.Trim();
            objRateMaster = serviceobject.GetRateMasterViewDetails(objRateMaster);
            l_bln_is_auto_ibs = objRateMaster.ListRateMasterViewDtl[0].is_auto_ibs;
            l_str_ibs_unit = objRateMaster.ListRateMasterViewDtl[0].ibs_unit;
            objRateMaster.catg = (catg == null) ? string.Empty : catg.Trim();
            objLookUp.id = "8";
            objLookUp.lookuptype = "VAS";
            objLookUp = ServiceobjLookUp.GetLookUpValue(objLookUp);
            objRateMaster.ListLookUpCategoryDtl = objLookUp.ListLookUpDtl;
            objPrice.uom_type = "Quantity";
            objPrice = ServiceObjectPrice.GetPriceuomDetails(objPrice);
            objRateMaster.ListPriceuom = objPrice.ListPriceuom;
            objRateMaster.type =(Type== null)?string.Empty: Type.Trim();
            objLookUp.id = "7";
            objLookUp.lookuptype = "RATEMASTER";
            objLookUp = ServiceobjLookUp.GetLookUpValue(objLookUp);
            objRateMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
            objRateMaster.status =(staus== null)?string.Empty: staus.Trim();
            objLookUp.id = "11";
            objLookUp.lookuptype = "RATESTATUS";
            objLookUp = ServiceobjLookUp.GetLookUpValue(objLookUp);
            objRateMaster.ListLookUpStatusDtl = objLookUp.ListLookUpDtl;
            if (objRateMaster.ListLookUpStatusDtl.Count>0)
            {
                objRateMaster.itm_name = objRateMaster.ListRateMasterViewDtl[0].itm_name;
                objRateMaster.list_price = Math.Round(Convert.ToDecimal(objRateMaster.ListRateMasterViewDtl[0].list_price), 2);
                objRateMaster.last_so_dt = objRateMaster.ListRateMasterViewDtl[0].last_so_dt;
            }

            objLookUp.id = "300";
            objLookUp.lookuptype = "IBSUNIT";
            objLookUp = ServiceobjLookUp.GetLookUpValue(objLookUp);
            objRateMaster.ListIBSUnit = objLookUp.ListLookUpDtl;

            objRateMaster = serviceobject.GetRateMasterViewDetails(objRateMaster);
            objLookUp.lookuptype = (Type == null) ? string.Empty : Type.Trim();
            objLookUp = serviceobject.GetRateMasterCategory(objLookUp);
            objRateMaster.ListLookUpCategoryDtl = objLookUp.ListLookUpCategoryDtl;
            objRateMaster.ibs_unit = l_str_ibs_unit;
            objRateMaster.is_auto_ibs = l_bln_is_auto_ibs;
         
            Mapper.CreateMap<RateMaster, RateMasterModel>();
            RateMasterModel objRateMastermodel = Mapper.Map<RateMaster, RateMasterModel>(objRateMaster);
            return PartialView("_RateEdit", objRateMastermodel);
        }
        public ActionResult UpdateRateMaster(string p_str_cmp_id, string p_str_itm_num, string p_str_type, string p_str_catg, string p_str_status, 
            string p_str_itm_name, string p_str_list_price, string p_str_price_uom, string p_str_last_so_dt, bool p_bln_is_auto_ibs, string p_str_ibs_unit)
        {
            RateMaster objRateMaster = new RateMaster();
            RateMasterService serviceobject = new RateMasterService();
            objRateMaster.cmp_id = (p_str_cmp_id==null)?string.Empty: p_str_cmp_id.Trim();
            objRateMaster.itm_num = (p_str_itm_num==null)?string.Empty: p_str_itm_num.Trim();
            objRateMaster.type = (p_str_type== null)?string.Empty: p_str_type.Trim();
            objRateMaster.catg = (p_str_catg== null)?string.Empty: p_str_catg.Trim();
            objRateMaster.status = (p_str_status== null)?string.Empty: p_str_status.Trim();
            objRateMaster.itm_name = (p_str_itm_name== null)?string.Empty: p_str_itm_name.Trim();
            objRateMaster.list_price =Convert.ToDecimal( p_str_list_price);
            objRateMaster.price_uom = (p_str_price_uom == null) ? string.Empty : p_str_price_uom.Trim();
            objRateMaster.last_so_dt = (p_str_last_so_dt == null) ? string.Empty : p_str_last_so_dt.Trim();
            objRateMaster.opt_id = "M";
            objRateMaster.is_auto_ibs = p_bln_is_auto_ibs;
            objRateMaster.ibs_unit = p_str_ibs_unit;
            
            serviceobject.RateMasterCreateUpdate(objRateMaster);
            l_str_include_entry_dtls = true;
            Mapper.CreateMap<RateMaster, RateMasterModel>();
            RateMasterModel objRateMastermodel = Mapper.Map<RateMaster, RateMasterModel>(objRateMaster);

            return Json(l_str_include_entry_dtls, JsonRequestBehavior.AllowGet);
    }
        public ActionResult InsertRateMaster(string p_str_cmp_id, string p_str_itm_num, string p_str_type, string p_str_catg, string p_str_status, string p_str_itm_name,
            decimal p_str_list_price, string p_str_price_uom, string p_str_last_so_dt, bool p_bln_is_auto_ibs, string p_str_ibs_unit)
        {
            RateMaster objRateMaster = new RateMaster();
            RateMasterService serviceobject = new RateMasterService();
            objRateMaster.cmp_id = (p_str_cmp_id==null)?string.Empty: p_str_cmp_id.Trim();
            objRateMaster.itm_num = (p_str_itm_num == null) ? string.Empty : p_str_itm_num.Trim();
            objRateMaster.type = (p_str_type== null)?string.Empty: p_str_type.Trim();
            objRateMaster.catg = (p_str_catg== null)?string.Empty: p_str_catg.Trim();
            objRateMaster.status = (p_str_status== null)?string.Empty: p_str_status.Trim();
            objRateMaster.itm_name =(p_str_itm_name== null)?string.Empty: p_str_itm_name.Trim();
            objRateMaster.list_price = p_str_list_price;
            objRateMaster.price_uom = (p_str_price_uom== null)?string.Empty: p_str_price_uom.Trim();
            objRateMaster.last_so_dt = (p_str_last_so_dt== null)?string.Empty: p_str_last_so_dt.Trim();
            objRateMaster.is_auto_ibs = p_bln_is_auto_ibs;
            objRateMaster.ibs_unit = p_str_ibs_unit;
            serviceobject.RateMasterCreateUpdate(objRateMaster);
            Mapper.CreateMap<RateMaster, RateMasterModel>();
            RateMasterModel objRateMastermodel = Mapper.Map<RateMaster, RateMasterModel>(objRateMaster);
            return View("~/Views/RateMaster/RateMaster.cshtml", objRateMastermodel);
        }
        public ActionResult NewRateMaster(string p_str_cmp_id, string p_str_itm_num, string p_str_type, string p_str_catg, string p_str_status, string p_str_itm_name, 
            decimal p_str_list_price, string p_str_price_uom, string p_str_last_so_dt,bool p_bln_is_auto_ibs, string p_str_ibs_unit)
        {
          
            RateMaster objRateMaster = new RateMaster();
            int l_str_RateId = 1;
            RateMasterService serviceobject = new RateMasterService();
            objRateMaster.cmp_id = (p_str_cmp_id == null) ? string.Empty : p_str_cmp_id.Trim();
            objRateMaster.itm_num = (p_str_itm_num == null) ? string.Empty : p_str_itm_num.Trim();
            objRateMaster.type = (p_str_type == null) ? string.Empty : p_str_type.Trim();
            objRateMaster.catg = (p_str_catg == null) ? string.Empty : p_str_catg.Trim();
            objRateMaster.status = (p_str_status == null) ? string.Empty : p_str_status.Trim();
            objRateMaster.itm_name = (p_str_itm_name == null) ? string.Empty : p_str_itm_name.Trim();
            objRateMaster.list_price = p_str_list_price;
            objRateMaster.price_uom = (p_str_price_uom == null) ? string.Empty : p_str_price_uom.Trim();
            objRateMaster.last_so_dt = (p_str_last_so_dt == null) ? string.Empty : p_str_last_so_dt.Trim();
            objRateMaster.opt_id = "A";
            objRateMaster.is_auto_ibs = p_bln_is_auto_ibs;
            objRateMaster.ibs_unit = p_str_ibs_unit;
            //CR-180421-001 Added By Nithya
            objRateMaster = serviceobject.ExistRate(objRateMaster);
            if (objRateMaster.LstRateId.Count > 0)
            {
                l_str_RateId = 0;
                return Json(l_str_RateId, JsonRequestBehavior.AllowGet);
            }
           
            serviceobject.RateMasterCreateUpdate(objRateMaster);           
            Mapper.CreateMap<RateMaster, RateMasterModel>();
            RateMasterModel objRateMastermodel = Mapper.Map<RateMaster, RateMasterModel>(objRateMaster);
            return Json(l_str_RateId, JsonRequestBehavior.AllowGet);
          //  return View("~/Views/RateMaster/RateMaster.cshtml", objRateMastermodel);


        }

        public ActionResult Add(string cmpid, string Itmnum)
        {
            RateMaster objRateMaster = new RateMaster();
            RateMasterService serviceobject = new RateMasterService();
            objRateMaster.cmp_id = (cmpid== null)?string.Empty: cmpid.Trim();
            if(Session["UserID"].ToString()!=null)
            {
                objCompany.user_id = Session["UserID"].ToString().Trim();
            }
            else
            {
                objCompany.user_id = string.Empty;
            }
            objRateMaster.last_so_dt = DateTime.Now.ToString("MM/dd/yyyy");
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany, cmpid);
            objRateMaster.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objLookUp.id = "7";
            objLookUp.lookuptype = "RATEMASTER";
            objLookUp = ServiceobjLookUp.GetLookUpValue(objLookUp);
            objRateMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
            objLookUp.id = "8";
            objLookUp.lookuptype = "VAS";
            objLookUp = ServiceobjLookUp.GetLookUpValue(objLookUp);
            objRateMaster.ListLookUpCategoryDtl = objLookUp.ListLookUpDtl;
            objLookUp.id = "11";
            objLookUp.lookuptype = "RATESTATUS";
            objLookUp = ServiceobjLookUp.GetLookUpValue(objLookUp);
            objRateMaster.ListLookUpStatusDtl = objLookUp.ListLookUpDtl;

            objLookUp.id = "300";
            objLookUp.lookuptype = "IBSUNIT";
            objLookUp = ServiceobjLookUp.GetLookUpValue(objLookUp);
            objRateMaster.ListIBSUnit = objLookUp.ListLookUpDtl;

            objRateMaster.price_uom = "EA";
            objPrice.uom_type = "Quantity";
            objPrice = ServiceObjectPrice.GetPriceuomDetails(objPrice);
            objRateMaster.ListPriceuom = objPrice.ListPriceuom;

            Mapper.CreateMap<RateMaster, RateMasterModel>();
            RateMasterModel objRateMastermodel = Mapper.Map<RateMaster, RateMasterModel>(objRateMaster);
            return PartialView("_RateAdd", objRateMastermodel);
        }
        [HttpPost]
        public ActionResult CreateUpdateRateMaster(string p_str_cmp_id, string p_str_catg, string p_str_itm_number, string p_str_list_price, string p_str_last_so_dt, string p_str_status, string p_str_price_uom)
        {
            RateMaster objRateMaster = new RateMaster();
            IRateMasterService ServiceObject = new RateMasterService();
            objRateMaster.cmp_id = (p_str_cmp_id == null) ? string.Empty : p_str_cmp_id.Trim();
            objRateMaster.catg = (p_str_catg == null) ? string.Empty : p_str_catg.Trim();
            objRateMaster.itm_num = (p_str_itm_number == null) ? string.Empty : p_str_itm_number.Trim();
            objRateMaster.last_so_dt = (p_str_last_so_dt == null) ? string.Empty : p_str_last_so_dt.Trim();
            objRateMaster.status = (p_str_status == null) ? string.Empty : p_str_status.Trim();
            objRateMaster.price_uom = (p_str_price_uom == null) ? string.Empty : p_str_price_uom.Trim();
            ServiceObject.RateMasterCreateUpdate(objRateMaster);
            Mapper.CreateMap<RateMaster, RateMasterModel>();
            RateMasterModel objRateMasterModel = Mapper.Map<RateMaster, RateMasterModel>(objRateMaster);
            return View("~/Views/RateMaster/RateMaster.cshtml", objRateMasterModel);
        }
        public ActionResult DeleteRateMaster(string Itmnum, string cmpid,string Type)
        {

            RateMaster objRateMaster = new RateMaster();
            IRateMasterService ServiceObject = new RateMasterService();
            int ResultCount;
            objRateMaster.itm_num = (Itmnum == null) ? string.Empty : Itmnum.Trim();
            objRateMaster.cmp_id = (cmpid == null) ? string.Empty : cmpid.Trim();
            objRateMaster.type = (Type== null)?string.Empty: Type.Trim();
            objRateMaster = ServiceObject.CHECK_RATEID_IS_IN_USE(objRateMaster);
            if(objRateMaster.LstRateId.Count>0)
            {
                ResultCount = 2;
                return Json(ResultCount, JsonRequestBehavior.AllowGet);
            }
            //End
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
        [HttpPost]
        public ActionResult TypeChange(string id)
        {
            LookUp objRateMaster = new LookUp();
            IRateMasterService ServiceObject = new RateMasterService();
            objRateMaster.lookuptype = (id== null)?string.Empty: id.Trim();
            objRateMaster = ServiceObject.GetRateMasterCategory(objRateMaster);
            var serializer = new JavaScriptSerializer() { MaxJsonLength = 86753090 };
            // Perform your serialization
            serializer.Serialize(objRateMaster);
            return new JsonResult()
            {
                Data = objRateMaster,
                MaxJsonLength = 86753090
            };
           

        }
        public JsonResult MASTER_INQ_HDR_DATA(string p_str_cmp_id, string p_str_type, string p_str_Rate_Id_Fm, string p_str_Rate_Id_To)
        {
            RateMaster objRateMaster = new RateMaster();
            IRateMasterService ServiceObject = new RateMasterService();
            Session["g_str_cmp_id"] = (p_str_cmp_id == null) ? string.Empty : p_str_cmp_id.Trim();
            Session["TEMP_CMP_ID"] = (p_str_cmp_id == null) ? string.Empty : p_str_cmp_id.Trim();
            Session["TEMP_RATE_TYPE"] = (p_str_type == null) ? string.Empty : p_str_type.Trim();
            Session["TEMP_RATEID_FM"] = (p_str_Rate_Id_Fm == null) ? string.Empty : p_str_Rate_Id_Fm.Trim();
            Session["TEMP_RATEID_TO"] = (p_str_Rate_Id_To == null) ? string.Empty : p_str_Rate_Id_To.Trim();
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
                l_str_search_flag = Session["g_str_Search_flag"].ToString();
                objRateMaster.is_company_user = Session["IsCompanyUser"].ToString();
                if (l_str_search_flag == "True")
                {
                    objRateMaster.cmp_id = Session["TEMP_CMP_ID"].ToString().Trim();
                    objRateMaster.type = Session["TEMP_RATE_TYPE"].ToString().Trim();
                    objRateMaster.Rate_Id_Fm = Session["TEMP_RATEID_FM"].ToString().Trim();
                    objRateMaster.Rate_Id_To = Session["TEMP_RATEID_TO"].ToString().Trim();

                }
                else
                {
                    objRateMaster.cmp_id = (p_str_cmp_id == null) ? string.Empty : p_str_cmp_id.Trim();
                    objRateMaster.type = (p_str_type == null) ? string.Empty : p_str_type.Trim();
                    objRateMaster.itm_num = (p_str_rate_id_frm == null) ? string.Empty : p_str_rate_id_frm.Trim();


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


        public ActionResult RateDetailReport( string ReportType, string ReportId, string p_str_cmp_id,  string p_str_rate_type, string p_str_rate_id_fm, string p_str_rate_id_to)
        {
            try
            {
                string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                string strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
                string l_str_file_name = string.Empty;
                DataTable dtBill = new DataTable();
                string tempFileName = string.Empty;
                if (ReportType == "EXCEL")
                {
                    if (ReportId == "RateMasterSummary")
                    {
                        IRateMasterService ServiceObject = new RateMasterService();
                        dtBill = ServiceObject.GetRateDtlListExcel(p_str_cmp_id, p_str_rate_type, p_str_rate_id_fm, p_str_rate_id_to);

                        if (!Directory.Exists(strOutputpath))
                        {
                            Directory.CreateDirectory(strOutputpath);
                        }

                        l_str_file_name = "DF_" + p_str_cmp_id.ToUpper().ToString().Trim() + "_RATE_MASTER_SMRY_RPT_" + strDateFormat + ".xlsx";

                        tempFileName = strOutputpath + l_str_file_name;

                        if (System.IO.File.Exists(tempFileName))
                            System.IO.File.Delete(tempFileName);
                        xls_Rate_Master_Summary_Excel mxcel1 = new xls_Rate_Master_Summary_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "RATE_MASTER_SMRY_RPT.xlsx");
                        var dataRows = dtBill.Rows;
                        DataRow dr = dataRows[0];
                        mxcel1.PopulateHeader(p_str_cmp_id);
                        mxcel1.PopulateData(dtBill, true);
                        mxcel1.SaveAs(tempFileName);
                        FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                        return File(fs, "application / xlsx", l_str_file_name);
                        /*
                        IRateMasterService ServiceObject = new RateMasterService();
                        RateDtl objRateDtl = new RateDtl();
                        objRateDtl = ServiceObject.GetRateDtlList(objRateDtl, p_str_cmp_id, p_str_rate_type, p_str_rate_id_fm, p_str_rate_id_to);
                        GridView gv = new GridView();
                        gv.DataSource = objRateDtl.ListRateDtl;
                        gv.DataBind();
                        Session["RateList"] = gv;
                        return new DownloadFileActionResult((GridView)Session["RateList"], p_str_cmp_id + "-RATE-LIST-" + DateTime.Now.ToString("yyyyMMddHHssmm") + ".xls");
                        */
                    }
                    else
                    {
                        return Json(string.Empty, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(string.Empty, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception EX)
            {
                return Json(EX.InnerException.ToString(), JsonRequestBehavior.AllowGet);
            }
            finally
            {

            }


        }

    }
}





