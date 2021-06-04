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
using System.Web.UI.WebControls;
using static GsEPWv8_4_MVC.Core.Entity.StockAsOfDate;

namespace GsEPWv8_4_MVC.Controllers
{
    public class StockAsOfDateController : Controller
    {
        // GET: StockAsOfDate
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult StockAsOfDate(string FullFillType, string cmp, string status, string DateFm, string DateTo)
        {
            string l_str_cmp_id = string.Empty;
            try
            {
                StockAsOfDate objStockAsOfDate = new StockAsOfDate();
                IStockAsOfDateService ServiceObject = new StockAsOfDateService();
                objStockAsOfDate.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
                objStockAsOfDate.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                if (objStockAsOfDate.cmp_id != "")
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objStockAsOfDate.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                }
                else
                {
                    if (FullFillType == null)
                    {
                        objCompany.user_id = Session["UserID"].ToString().Trim();
                        objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                        objStockAsOfDate.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    }
                }
                if (FullFillType != null)
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objStockAsOfDate.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    objStockAsOfDate.cmp_id = cmp.Trim();
                    //objStockAsOfDate.ib_doc_id = "";
                    //objStockAsOfDate.po_num = "";
                    //objStockAsOfDate.Ref_Num = "";
                    objStockAsOfDate.cont_id = "";
                    objStockAsOfDate.lot_id = "";
                    objStockAsOfDate.loc_id = "";
                    //objStockAsOfDate.whs_id = "";
                    //objStockAsOfDate.Itmdtl = "";
                    objStockAsOfDate.itm_color = "";
                    objStockAsOfDate.itm_size = "";
                    objStockAsOfDate.itm_name = "";

                }
                LookUp objLookUp = new LookUp();
                LookUpService ServiceObject1 = new LookUpService();
                objLookUp.id = "5";
                objLookUp.lookuptype = "INVENTORYINQ";
                objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                objStockAsOfDate.ListLookUpDtl = objLookUp.ListLookUpDtl;
                objStockAsOfDate.p_str_company = objStockAsOfDate.cmp_id;
                Mapper.CreateMap<StockAsOfDate, StockAsOfDateModel>();
                StockAsOfDateModel objStockAsOfDateModel = Mapper.Map<StockAsOfDate, StockAsOfDateModel>(objStockAsOfDate);
                return View(objStockAsOfDateModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ActionResult GetStockASOfDateDetails(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_Status, string p_str_itm_num, string p_str_itm_color,
            string p_str_itm_size, string p_str_itm_name, string p_str_cont_id, string p_str_loc_id, string p_str_whs_id, string p_str_As_Of_Date, string p_str_radio)
        {
            try
            {
                StockAsOfDate objStockAsOfDate = new StockAsOfDate();
                IStockAsOfDateService ServiceObject = new StockAsOfDateService();
                objStockAsOfDate.cmp_id = p_str_cmp_id;
                objStockAsOfDate.ib_doc_id = p_str_ib_doc_id;
                objStockAsOfDate.itm_num = p_str_itm_num;
                objStockAsOfDate.itm_color = p_str_itm_color;
                objStockAsOfDate.itm_size = p_str_itm_size;
                objStockAsOfDate.status = p_str_Status;
                objStockAsOfDate.itm_name = p_str_itm_name;
                objStockAsOfDate.cont_id = p_str_cont_id;
                objStockAsOfDate.loc_id = p_str_loc_id;
                objStockAsOfDate.whs_id = p_str_whs_id;
                objStockAsOfDate.As_Of_Date = p_str_As_Of_Date;
                objStockAsOfDate = ServiceObject.GetStockAsOfDateDetails(objStockAsOfDate);
                if (p_str_radio.Trim() == "SummaryStyle")
                {
                    Mapper.CreateMap<StockAsOfDate, StockAsOfDateModel>();
                    StockAsOfDateModel objStockAsOfDateModel = Mapper.Map<StockAsOfDate, StockAsOfDateModel>(objStockAsOfDate);
                    return PartialView("_StockAsOfdateSummary", objStockAsOfDateModel);
                }
                else
                {
                    Mapper.CreateMap<StockAsOfDate, StockAsOfDateModel>();
                    StockAsOfDateModel objStockAsOfDateModel = Mapper.Map<StockAsOfDate, StockAsOfDateModel>(objStockAsOfDate);
                    return PartialView("_StockAsOfDate", objStockAsOfDateModel);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult ShowReport(string var_name, string p_str_cmp_id, string p_str_ib_doc_id,  string p_str_itm_num, string p_str_itm_color,
          string p_str_itm_size, string p_str_itm_name, string p_str_cont_id, string p_str_loc_id, string p_str_whs_id, string p_str_As_Of_Date, string p_str_Status, string type)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            string l_str_rpt_selection1 = string.Empty;
            l_str_rpt_selection = var_name;
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "DetailbyStyle")
                    {
                        strReportName = "rpt_inventory_AsOf_date_Detail_Rpt.rpt";
                        StockAsOfDate objStockAsOfDate = new StockAsOfDate();
                        IStockAsOfDateService ServiceObject = new StockAsOfDateService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                        objStockAsOfDate.cmp_id = p_str_cmp_id;
                        objStockAsOfDate.ib_doc_id = p_str_ib_doc_id;
                        objStockAsOfDate.itm_num = p_str_itm_num;
                        objStockAsOfDate.itm_color = p_str_itm_color;
                        objStockAsOfDate.itm_size = p_str_itm_size;
                        objStockAsOfDate.status = p_str_Status;
                        objStockAsOfDate.itm_name = p_str_itm_name;
                        objStockAsOfDate.cont_id = p_str_cont_id;
                        objStockAsOfDate.loc_id = p_str_loc_id;
                        objStockAsOfDate.whs_id = p_str_whs_id;
                        objStockAsOfDate.As_Of_Date = p_str_As_Of_Date;
                        objStockAsOfDate = ServiceObject.GetStockAsOfDateDetailsRpt(objStockAsOfDate);
                        if (type == "PDF")
                        {
                            var rptSource = objStockAsOfDate.LstAsOfDateRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objStockAsOfDate.LstAsOfDateRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objStockAsOfDate.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objStockAsOfDate.Image_Path);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else
                        if (type == "Excel")
                        {
                            objStockAsOfDate = ServiceObject.GetStockAsOfDateDetailsRpt(objStockAsOfDate);

                            List<StockAsOfdateDetailExcel> li = new List<StockAsOfdateDetailExcel>();
                            for (int i = 0; i < objStockAsOfDate.LstAsOfDateRpt.Count; i++)
                            {
                                StockAsOfdateDetailExcel objAsOfdateDetailExcel = new StockAsOfdateDetailExcel();
                                objAsOfdateDetailExcel.IBdocId = objStockAsOfDate.LstAsOfDateRpt[i].ib_doc_id;
                                objAsOfdateDetailExcel.RcvdDt = objStockAsOfDate.LstAsOfDateRpt[i].rcvd_dt;
                                objAsOfdateDetailExcel.ContainerId = objStockAsOfDate.LstAsOfDateRpt[i].cont_id;
                                objAsOfdateDetailExcel.Style = objStockAsOfDate.LstAsOfDateRpt[i].itm_num;
                                objAsOfdateDetailExcel.Color = objStockAsOfDate.LstAsOfDateRpt[i].itm_color;
                                objAsOfdateDetailExcel.Size = objStockAsOfDate.LstAsOfDateRpt[i].itm_size;
                                objAsOfdateDetailExcel.Description = objStockAsOfDate.LstAsOfDateRpt[i].itm_name;
                                objAsOfdateDetailExcel.WhsId = objStockAsOfDate.LstAsOfDateRpt[i].whs_id;
                                objAsOfdateDetailExcel.LocId = objStockAsOfDate.LstAsOfDateRpt[i].loc_id;
                                objAsOfdateDetailExcel.AvailCtn = objStockAsOfDate.LstAsOfDateRpt[i].Ctns;
                                objAsOfdateDetailExcel.PPk = objStockAsOfDate.LstAsOfDateRpt[i].pkg_qty;
                                objAsOfdateDetailExcel.AvailQty = objStockAsOfDate.LstAsOfDateRpt[i].avail_qty;                               
                                li.Add(objAsOfdateDetailExcel);
                            }
                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["IV_STYLE_DTL"] = gv;
                            return new DownloadFileActionResult((GridView)Session["IV_STYLE_DTL"], "IV_STYLE_DTL" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
                        }
                    }

                    if (l_str_rpt_selection == "SummarybyStyle")
                    {
                        strReportName = "rpt_inventory_AsOf_date_Summary_Rpt.rpt";
                        StockAsOfDate objStockAsOfDate = new StockAsOfDate();
                        IStockAsOfDateService ServiceObject = new StockAsOfDateService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                        objStockAsOfDate.cmp_id = p_str_cmp_id;
                        objStockAsOfDate.ib_doc_id = p_str_ib_doc_id;
                        objStockAsOfDate.itm_num = p_str_itm_num;
                        objStockAsOfDate.itm_color = p_str_itm_color;
                        objStockAsOfDate.itm_size = p_str_itm_size;
                        objStockAsOfDate.status = p_str_Status;
                        objStockAsOfDate.itm_name = p_str_itm_name;
                        objStockAsOfDate.cont_id = p_str_cont_id;
                        objStockAsOfDate.loc_id = p_str_loc_id;
                        objStockAsOfDate.whs_id = p_str_whs_id;
                        objStockAsOfDate.As_Of_Date = p_str_As_Of_Date;
                        objStockAsOfDate = ServiceObject.GetStockAsOfDateDetailsRpt(objStockAsOfDate);//CR-3PL-MVC-180322-001 Added By NIthya
                        if (type == "PDF")
                        {
                            var rptSource = objStockAsOfDate.LstAsOfDateRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objStockAsOfDate.LstAsOfDateRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objStockAsOfDate.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objStockAsOfDate.Image_Path);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else
                        if (type == "Excel")
                        {
                            objStockAsOfDate = ServiceObject.GetStockAsOfDateDetailsRpt(objStockAsOfDate);

                            List<StockAsOfdateSummaryExcel> li = new List<StockAsOfdateSummaryExcel>();
                            for (int i = 0; i < objStockAsOfDate.LstAsOfDateRpt.Count; i++)
                            {

                                StockAsOfdateSummaryExcel objAsOfdateSummaryExcel = new StockAsOfdateSummaryExcel();
                                objAsOfdateSummaryExcel.Style = objStockAsOfDate.LstAsOfDateRpt[i].itm_num;
                                objAsOfdateSummaryExcel.Color = objStockAsOfDate.LstAsOfDateRpt[i].itm_color;
                                objAsOfdateSummaryExcel.Size = objStockAsOfDate.LstAsOfDateRpt[i].itm_size;
                                objAsOfdateSummaryExcel.Description = objStockAsOfDate.LstAsOfDateRpt[i].itm_name;
                                objAsOfdateSummaryExcel.AvailQty = objStockAsOfDate.LstAsOfDateRpt[i].avail_qty;                               
                                li.Add(objAsOfdateSummaryExcel);
                            }
                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["IV_STYLE_SMRY"] = gv;
                            return new DownloadFileActionResult((GridView)Session["IV_STYLE_SMRY"], "IV_STYLE_SMRY" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
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
        public ActionResult EmailShowReport(string var_name, string p_str_cmp_id, string p_str_ib_doc_id,  string p_str_itm_num, string p_str_itm_color,
        string p_str_itm_size, string p_str_itm_name, string p_str_cont_id, string p_str_loc_id, string p_str_whs_id, string p_str_As_Of_Date, string p_str_Status, string type)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            string l_str_rpt_selection1 = string.Empty;
            string strDateFormat = string.Empty;
            string strFileName = string.Empty;
            string reportFileName = string.Empty;
            l_str_rpt_selection = var_name;
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "DetailbyStyle")
                    {
                        strReportName = "rpt_inventory_AsOf_date_Detail_Rpt.rpt";
                        StockAsOfDate objStockAsOfDate = new StockAsOfDate();
                        IStockAsOfDateService ServiceObject = new StockAsOfDateService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                        objStockAsOfDate.cmp_id = p_str_cmp_id;
                        objStockAsOfDate.ib_doc_id = p_str_ib_doc_id;
                        objStockAsOfDate.itm_num = p_str_itm_num;
                        objStockAsOfDate.itm_color = p_str_itm_color;
                        objStockAsOfDate.itm_size = p_str_itm_size;
                        objStockAsOfDate.status = p_str_Status;
                        objStockAsOfDate.itm_name = p_str_itm_name;
                        objStockAsOfDate.cont_id = p_str_cont_id;
                        objStockAsOfDate.loc_id = p_str_loc_id;
                        objStockAsOfDate.whs_id = p_str_whs_id;
                        objStockAsOfDate.As_Of_Date = p_str_As_Of_Date;
                        objStockAsOfDate = ServiceObject.GetStockAsOfDateDetailsRpt(objStockAsOfDate);
                        if (type == "PDF")
                        {
                            var rptSource = objStockAsOfDate.LstAsOfDateRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objStockAsOfDate.LstAsOfDateRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//AS_OF_DATE_INQ_" + strDateFormat + ".pdf";
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);                         
                            reportFileName = "AS_OF_DATE_INQ_" + strDateFormat + ".pdf";
                            Session["RptFileName"] = strFileName;
                            // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else
                        if (type == "Excel")
                        {
                            objStockAsOfDate = ServiceObject.GetStockAsOfDateDetailsRpt(objStockAsOfDate);

                            List<StockAsOfdateDetailExcel> li = new List<StockAsOfdateDetailExcel>();
                            for (int i = 0; i < objStockAsOfDate.LstAsOfDateRpt.Count; i++)
                            {
                                StockAsOfdateDetailExcel objAsOfdateDetailExcel = new StockAsOfdateDetailExcel();
                                objAsOfdateDetailExcel.IBdocId = objStockAsOfDate.LstAsOfDateRpt[i].ib_doc_id;
                                objAsOfdateDetailExcel.RcvdDt = objStockAsOfDate.LstAsOfDateRpt[i].rcvd_dt;
                                objAsOfdateDetailExcel.ContainerId = objStockAsOfDate.LstAsOfDateRpt[i].cont_id;
                                objAsOfdateDetailExcel.Style = objStockAsOfDate.LstAsOfDateRpt[i].itm_num;
                                objAsOfdateDetailExcel.Color = objStockAsOfDate.LstAsOfDateRpt[i].itm_color;
                                objAsOfdateDetailExcel.Size = objStockAsOfDate.LstAsOfDateRpt[i].itm_size;
                                objAsOfdateDetailExcel.Description = objStockAsOfDate.LstAsOfDateRpt[i].itm_name;
                                objAsOfdateDetailExcel.WhsId = objStockAsOfDate.LstAsOfDateRpt[i].whs_id;
                                objAsOfdateDetailExcel.LocId = objStockAsOfDate.LstAsOfDateRpt[i].loc_id;
                                objAsOfdateDetailExcel.AvailCtn = objStockAsOfDate.LstAsOfDateRpt[i].Ctns;
                                objAsOfdateDetailExcel.PPk = objStockAsOfDate.LstAsOfDateRpt[i].pkg_qty;
                                objAsOfdateDetailExcel.AvailQty = objStockAsOfDate.LstAsOfDateRpt[i].avail_qty;
                                li.Add(objAsOfdateDetailExcel);
                            }
                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["IV_STYLE_DTL"] = gv;
                            return new DownloadFileActionResult((GridView)Session["IV_STYLE_DTL"], "IV_STYLE_DTL" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
                        }
                    }

                    if (l_str_rpt_selection == "SummarybyStyle")
                    {
                        strReportName = "rpt_inventory_AsOf_date_Summary_Rpt.rpt";
                        StockAsOfDate objStockAsOfDate = new StockAsOfDate();
                        IStockAsOfDateService ServiceObject = new StockAsOfDateService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                        objStockAsOfDate.cmp_id = p_str_cmp_id;
                        objStockAsOfDate.ib_doc_id = p_str_ib_doc_id;
                        objStockAsOfDate.itm_num = p_str_itm_num;
                        objStockAsOfDate.itm_color = p_str_itm_color;
                        objStockAsOfDate.itm_size = p_str_itm_size;
                        objStockAsOfDate.status = p_str_Status;
                        objStockAsOfDate.itm_name = p_str_itm_name;
                        objStockAsOfDate.cont_id = p_str_cont_id;
                        objStockAsOfDate.loc_id = p_str_loc_id;
                        objStockAsOfDate.whs_id = p_str_whs_id;
                        objStockAsOfDate.As_Of_Date = p_str_As_Of_Date;
                        objStockAsOfDate = ServiceObject.GetStockAsOfDateDetailsRpt(objStockAsOfDate);//CR-3PL-MVC-180322-001 Added By NIthya
                        if (type == "PDF")
                        {
                            var rptSource = objStockAsOfDate.LstAsOfDateRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objStockAsOfDate.LstAsOfDateRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//AS_OF_DATE_INQ_" + strDateFormat + ".pdf";
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            reportFileName = "AS_OF_DATE_INQ_" + strDateFormat + ".pdf";
                            Session["RptFileName"] = strFileName;
                            //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else
                        if (type == "Excel")
                        {
                            objStockAsOfDate = ServiceObject.GetStockAsOfDateDetailsRpt(objStockAsOfDate);

                            List<StockAsOfdateSummaryExcel> li = new List<StockAsOfdateSummaryExcel>();
                            for (int i = 0; i < objStockAsOfDate.LstAsOfDateRpt.Count; i++)
                            {

                                StockAsOfdateSummaryExcel objAsOfdateSummaryExcel = new StockAsOfdateSummaryExcel();
                                objAsOfdateSummaryExcel.Style = objStockAsOfDate.LstAsOfDateRpt[i].itm_num;
                                objAsOfdateSummaryExcel.Color = objStockAsOfDate.LstAsOfDateRpt[i].itm_color;
                                objAsOfdateSummaryExcel.Size = objStockAsOfDate.LstAsOfDateRpt[i].itm_size;
                                objAsOfdateSummaryExcel.Description = objStockAsOfDate.LstAsOfDateRpt[i].itm_name;
                                objAsOfdateSummaryExcel.AvailQty = objStockAsOfDate.LstAsOfDateRpt[i].avail_qty;
                                li.Add(objAsOfdateSummaryExcel);
                            }
                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["IV_STYLE_SMRY"] = gv;
                            return new DownloadFileActionResult((GridView)Session["IV_STYLE_SMRY"], "IV_STYLE_SMRY" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
                        }
                    }
                }
                else
                {
                    Response.Write("<H2>Report not found</H2>");
                }
                Email objEmail = new Email();
                objEmail.CmpId = p_str_cmp_id;
                objEmail.EmailSubject = "IV_AS_OF_DATE_INQ_";
                //objEmail.Attachments = reportFileName;
                EmailService objEmailService = new EmailService();
                objEmail = objEmailService.GetSendMailDetails(objEmail);
                if (objEmail.ListEamilDetail.Count != 0)
                {

                    objEmail.Attachment = reportFileName;
                    objEmail.EmailTo = (objEmail.ListEamilDetail[0].EmailTo.Trim() == null || objEmail.ListEamilDetail[0].EmailTo.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailTo.Trim();
                    objEmail.EmailCC = (objEmail.ListEamilDetail[0].EmailCC.Trim() == null || objEmail.ListEamilDetail[0].EmailCC.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailCC.Trim();
                    objEmail.EmailMessage = (objEmail.ListEamilDetail[0].EmailMessage.Trim() == null || objEmail.ListEamilDetail[0].EmailMessage.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailMessage.Trim();

                }
                else
                {
                    objEmail.Attachment = reportFileName;
                    objEmail.EmailTo = "";
                    objEmail.EmailCC = "";
                    objEmail.EmailMessage = "";




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
    }
}