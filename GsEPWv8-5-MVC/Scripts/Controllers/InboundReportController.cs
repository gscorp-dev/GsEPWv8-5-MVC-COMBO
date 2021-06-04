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

namespace GsEPWv8_4_MVC.Controllers
{
    #region Change History
   
    #endregion Change History
    public class InboundReportController : Controller
    {
        // GET: InboundReport
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult InboundReport(string FullFillType, string cmp, string status, string DateFm, string DateTo)
        {
            string l_str_cmp_id = string.Empty;
            string l_str_Dflt_Dt_Reqd = string.Empty;
            try
            {
                InboundReport objInboundReport = new InboundReport();
                InboundReportService ServiceObject = new InboundReportService();
                objInboundReport.cmp_id = Session["dflt_cmp_id"].ToString().Trim();

                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                if (objInboundReport.cmp_id != "" && FullFillType == null)
                {
                    //objCompany.cmp_id = l_str_cmp_id;
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objInboundReport.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    l_str_Dflt_Dt_Reqd = Session["DFLT_DT_REQD"].ToString().Trim();
                    if (l_str_Dflt_Dt_Reqd == "N")
                    {
                        DateFm = "";
                        DateTo = "";
                    }
                }
                
                else
                {
                    l_str_Dflt_Dt_Reqd = Session["DFLT_DT_REQD"].ToString().Trim();
                    if (l_str_Dflt_Dt_Reqd == "N")
                    {
                        DateFm = "";
                        DateTo = "";
                    }
                    if (FullFillType == null)
                    {
                        objCompany.user_id = Session["UserID"].ToString().Trim();
                        objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                        objInboundReport.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    }
                    if (FullFillType != null)
                    {
                        objCompany.user_id = Session["UserID"].ToString().Trim();
                        objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                        objInboundReport.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;


                        objInboundReport.cmp_id = cmp;
                        objInboundReport.whs_id ="";
                        objInboundReport.ib_doc_idFm = "";
                        objInboundReport.ib_doc_idTo = "";
                        objInboundReport.RcvdDtFm = DateTime.Now.AddDays(Common.clsGlobal.DispDateFrom).ToString("MM/dd/yyyy"); 
                        objInboundReport.RcvdDtTo = DateTime.Now.ToString("MM/dd/yyyy"); 
                       

                        TempData["p_str_cmp_id"] = cmp;
                        TempData["p_str_whs_id"] = "";
                        TempData["p_str_ib_doc_idFm"] = objInboundReport.ib_doc_idFm;
                        TempData["p_str_ib_doc_idTo"] = objInboundReport.ib_doc_idTo;
                        TempData["p_str_RcvdDtFm"] = objInboundReport.RcvdDtFm;
                        TempData["p_str_RcvdDtTo"] = objInboundReport.RcvdDtTo;
                        if (l_str_Dflt_Dt_Reqd == "Y")
                        {
                            objInboundReport = ServiceObject.GetInboundRpt(objInboundReport);
                        }
                    }
                }                
                //20180428 aDDED By NITHYA dEFAULT WHSID
                objInboundReport = ServiceObject.GetDftWhs(objInboundReport);
                string l_str_DftWhs = objInboundReport.LstWhsDetails[0].dft_whs.Trim();
                if (l_str_DftWhs != "" || l_str_DftWhs != null)
                {
                    objInboundReport.whs_id = l_str_DftWhs;
                }
                objCompany.cust_cmp_id = cmp;
                objCompany.whs_id = "";
                objCompany = ServiceObjectCompany.GetWhsIdDetails(objCompany);
                objInboundReport.ListwhsPickDtl = objCompany.ListwhsPickDtl;
                Mapper.CreateMap<InboundReport, InboundReportModel>();
                InboundReportModel objInboundReportModel = Mapper.Map<InboundReport, InboundReportModel>(objInboundReport);
                return View(objInboundReportModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ActionResult GetInboundReport(string p_str_cmp_id, string p_str_whs_id, string p_str_ib_doc_idFm, string p_str_ib_doc_idTo, string p_str_RcvdDtFm, string p_str_RcvdDtTo)
        {
            try
            {
                InboundReport objInboundReport = new InboundReport();
                InboundReportService ServiceObject = new InboundReportService();
                //objInboundReport.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
                //p_str_cmp_id = objInboundReport.cmp_id;
                objInboundReport.cmp_id = p_str_cmp_id.Trim();
                objInboundReport.whs_id = p_str_whs_id.Trim();
                objInboundReport.ib_doc_idFm = p_str_ib_doc_idFm.Trim();
                objInboundReport.ib_doc_idTo = p_str_ib_doc_idTo.Trim();
                objInboundReport.RcvdDtFm = p_str_RcvdDtFm.Trim();
                objInboundReport.RcvdDtTo = p_str_RcvdDtTo.Trim();
                TempData["p_str_cmp_id"] = p_str_cmp_id;
                TempData["p_str_whs_id"] = p_str_whs_id;
                TempData["p_str_ib_doc_idFm"] = p_str_ib_doc_idFm;
                TempData["p_str_ib_doc_idTo"] = p_str_ib_doc_idTo;
                TempData["p_str_RcvdDtFm"] = p_str_RcvdDtFm;
                TempData["p_str_RcvdDtTo"] = p_str_RcvdDtTo;              
                objInboundReport = ServiceObject.GetInboundRpt(objInboundReport);
                Mapper.CreateMap<InboundReport, InboundReportModel>();
                InboundReportModel objInboundReportModel = Mapper.Map<InboundReport, InboundReportModel>(objInboundReport);
                return PartialView("_InboundReport", objInboundReportModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public JsonResult GetWhsDetailss(string term, string cmpid)
        {
            InboundReportService ServiceObject = new InboundReportService();
            var List = ServiceObject.InboundWhsDetails(term, cmpid.Trim()).LstWhsDetails.Select(x => new { label = x.whsdtl, value = x.whsdtl, whs_id = x.whs_id }).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShowEmailReport(string var_name, string p_str_cmp_id, string p_str_whs_id, string p_str_RcvdDtFm, string p_str_RcvdDtTo, string p_str_ib_doc_idFm, string p_str_ib_doc_idTo, string type)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("mm/dd/yyyy");
            string strToDate = DateTime.Now.ToString("mm/dd/yyyy");
            string l_str_rpt_selection = string.Empty;
            string strDateFormat = string.Empty;
            string strFileName = string.Empty;
            string reportFileName = string.Empty;
            l_str_rpt_selection = var_name;

           
         


           try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "InbndStyle" || l_str_rpt_selection == "InbndDate")
                    {
                        if (l_str_rpt_selection == "InbndStyle")
                        {
                            strReportName = "rpt_ib_recv_rpt_by_style.rpt";
                        }
                        else
                        {
                            strReportName = "rpt_ib_recv_rpt_by_dt.rpt";
                        }
                        InboundReport objInboundReport = new InboundReport();
                        IInboundReportService ServiceObject = new InboundReportService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                        objInboundReport.cmp_id = p_str_cmp_id;
                        objInboundReport.whs_id = p_str_whs_id;
                        objInboundReport.ib_doc_idFm = p_str_ib_doc_idFm;

                        objInboundReport.ib_doc_idTo = p_str_ib_doc_idTo;

                        objInboundReport.RcvdDtFm = p_str_RcvdDtFm;
                        objInboundReport.RcvdDtTo = p_str_RcvdDtTo;
                        var rpt_title1 = "SORT BY STYLE";
                        var rpt_title2 = "SORT BY DATE";
                        var rpt_fm_dt = "Begining";
                        var rpt_to_dt = "Upto Date";
                        if (l_str_rpt_selection == "InbndStyle")
                        {
                            objInboundReport = ServiceObject.GetInboundRptStyle(objInboundReport);

                        }
                        else
                        {
                            objInboundReport = ServiceObject.GetInboundRptDate(objInboundReport);
                        }
                        var rptSource = objInboundReport.LstinboundRpt.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objInboundReport.LstinboundRpt.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        if (p_str_RcvdDtFm == "")
                        {
                            rd.SetParameterValue("fml_fm_dt", rpt_fm_dt);
                        }
                        else
                        {
                            rd.SetParameterValue("fml_fm_dt", p_str_RcvdDtFm.Trim());
                        }
                        if (p_str_RcvdDtTo == "")
                        {
                            rd.SetParameterValue("fml_to_dt", rpt_to_dt);
                        }
                        else
                        {
                            rd.SetParameterValue("fml_to_dt", p_str_RcvdDtTo.Trim());
                        }
                        if (l_str_rpt_selection == "InbndStyle")
                        {
                            rd.SetParameterValue("fml_rpt_title", rpt_title1);
                        }
                        else
                        {
                            rd.SetParameterValue("fml_rpt_title", rpt_title2);
                        }
                        objInboundReport.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0430_001 
                        rd.SetParameterValue("fml_image_path", objInboundReport.Image_Path);
                        if (type == "PDF")
                        {
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            if (l_str_rpt_selection == "InbndStyle")
                            {
                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//IB_RCV_RPT_BY_STYLE" + strDateFormat + ".pdf";


                            }
                            else
                            {
                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//IB_RCV_RPT_BY_DATE" + strDateFormat + ".pdf";
                            }

                            // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            // rd.ExportToDisk(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                            reportFileName = "RECEIVING_REPORTS_" + strDateFormat + ".pdf";//CR2018-03-15-001 Added By Soniya
                            Session["RptFileName"] = strFileName;
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Inbound Reports");
                        }
                        else
                        if (type == "Excel")
                        {

                            if (l_str_rpt_selection == "InbndStyle")
                            {
                                InboundRcvngRptByStyleExcel objInboundReceivingExcel = new InboundRcvngRptByStyleExcel();
                                objInboundReceivingExcel.Container_ID = p_str_cmp_id;
                                objInboundReceivingExcel.WHS = p_str_whs_id;
                                objInboundReceivingExcel.IB_Doc_ID = p_str_ib_doc_idFm;
                                objInboundReceivingExcel.Lot_Id = p_str_ib_doc_idTo;
                                objInboundReceivingExcel.Rcvd_Date = p_str_RcvdDtFm;
                                objInboundReceivingExcel.Loc_Id = p_str_RcvdDtTo;
                                objInboundReceivingExcel = ServiceObject.GetInboundRcvngRptByStyleExcel(objInboundReceivingExcel);
                                var model = objInboundReceivingExcel.ListInboundRcvngRptByStyleExcel.ToList();
                                GridView gv = new GridView();
                                gv.DataSource = model;
                                gv.DataBind();
                                Session["InbndStyle"] = gv;
                                if (Session["InbndStyle"] != null)
                                {
                                    return new DownloadFileActionResult((GridView)Session["InbndStyle"], "InbndStyle-" + DateTime.Now.ToString() + ".xls");
                                }
                            }
                            else
                            {
                                InboundRcvngRptByDateExcel objInboundReceivingdateExcel = new InboundRcvngRptByDateExcel();
                                objInboundReceivingdateExcel.Container_ID = p_str_cmp_id;
                                objInboundReceivingdateExcel.WHS = p_str_whs_id;
                                objInboundReceivingdateExcel.IB_Doc_ID = p_str_ib_doc_idFm;
                                objInboundReceivingdateExcel.Container_ID = p_str_ib_doc_idTo;
                                objInboundReceivingdateExcel.Rcvd_Date = p_str_RcvdDtFm;
                                objInboundReceivingdateExcel.Loc_Id = p_str_RcvdDtTo;
                                objInboundReceivingdateExcel = ServiceObject.GetInboundRcvngRptByDateExcel(objInboundReceivingdateExcel);
                                var model = objInboundReceivingdateExcel.ListInboundRcvngRptByDateExcel.ToList();
                                GridView gv = new GridView();
                                gv.DataSource = model;
                                gv.DataBind();
                                Session["InbndDate"] = gv;
                                if (Session["InbndDate"] != null)
                                {
                                    return new DownloadFileActionResult((GridView)Session["InbndDate"], "InbndDate-" + DateTime.Now.ToString() + ".xls");
                                }
                            }
                            //rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);

                        }
                        //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                    }
                }
                else
                {
                    Response.Write("<H2>Report not found</H2>");
                }
                Email objEmail = new Email();
                objEmail.CmpId = p_str_cmp_id;
                if (l_str_rpt_selection == "InbndStyle")
                {
                    objEmail.EmailSubject = "IB_RCV_RPT_BY_STYLE";

                }
                else
                {
                    objEmail.EmailSubject = "IB_RCV_RPT_BY_DATE";

                }
                EmailService objEmailService = new EmailService();
                objEmail = objEmailService.GetSendMailDetails(objEmail);
                //CR2018-03-15-001 Added By Soniya
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
                //CR2018-03-15-001 End
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

        public ActionResult ShowReport(string var_name, string p_str_cmp_id, string p_str_whs_id, string p_str_RcvdDtFm, string p_str_RcvdDtTo, string p_str_ib_doc_idFm, string p_str_ib_doc_idTo, string type)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("mm/dd/yyyy");
            string strToDate = DateTime.Now.ToString("mm/dd/yyyy");
            string l_str_rpt_selection = string.Empty;

            //string p_str_cmp_id = string.Empty;
            //string p_str_ib_doc_idFm = string.Empty;
            //string p_str_ib_doc_idTo = string.Empty;
            //string p_str_RcvdDtFm = string.Empty;
            //string p_str_RcvdDtTo = string.Empty;
            //string p_str_whs_id = string.Empty;
            l_str_rpt_selection = var_name;        
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "InbndStyle" || l_str_rpt_selection == "InbndDate")
                    {
                        if (l_str_rpt_selection == "InbndStyle")
                        {
                            strReportName = "rpt_ib_recv_rpt_by_style.rpt";
                        }
                        else
                        {
                            strReportName = "rpt_ib_recv_rpt_by_dt.rpt";
                        }
                        InboundReport objInboundReport = new InboundReport();
                        IInboundReportService ServiceObject = new InboundReportService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                        objInboundReport.cmp_id = p_str_cmp_id;
                        objInboundReport.whs_id = p_str_whs_id;
                        objInboundReport.ib_doc_idFm = p_str_ib_doc_idFm;
                      
                        objInboundReport.ib_doc_idTo = p_str_ib_doc_idTo;
                      
                        objInboundReport.RcvdDtFm = p_str_RcvdDtFm;
                        objInboundReport.RcvdDtTo = p_str_RcvdDtTo;
                        var rpt_title1 = "SORT BY STYLE";
                        var rpt_title2 = "SORT BY DATE";
                        var rpt_fm_dt = "Begining";
                        var rpt_to_dt = "Upto Date";
                        if (l_str_rpt_selection == "InbndStyle")
                        {
                            objInboundReport = ServiceObject.GetInboundRptStyle(objInboundReport);
                           
                        }
                        else
                        {
                            objInboundReport = ServiceObject.GetInboundRptDate(objInboundReport);
                        }
                        var rptSource = objInboundReport.LstinboundRpt.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objInboundReport.LstinboundRpt.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        if (p_str_RcvdDtFm == "")
                        {
                            rd.SetParameterValue("fml_fm_dt", rpt_fm_dt);
                        }
                        else
                        {
                            rd.SetParameterValue("fml_fm_dt", p_str_RcvdDtFm.Trim());
                        }
                       if(p_str_RcvdDtTo=="")
                        {
                            rd.SetParameterValue("fml_to_dt", rpt_to_dt);
                        }
                       else
                        {
                            rd.SetParameterValue("fml_to_dt", p_str_RcvdDtTo.Trim());
                        }
                        if (l_str_rpt_selection == "InbndStyle")
                        {
                            rd.SetParameterValue("fml_rpt_title", rpt_title1);
                        }
                         else
                        {
                            rd.SetParameterValue("fml_rpt_title", rpt_title2);
                        }
                        objInboundReport.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0430_001 
                        rd.SetParameterValue("fml_image_path", objInboundReport.Image_Path);
                        if (type == "PDF")
                        {

                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Inbound Reports");
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Inbound Reports");
                        }
                        else 
                        if (type == "Excel")
                        {
                           
                            if (l_str_rpt_selection == "InbndStyle")
                            {
                                InboundRcvngRptByStyleExcel objInboundReceivingExcel = new InboundRcvngRptByStyleExcel();
                                objInboundReceivingExcel.Container_ID = p_str_cmp_id;
                                objInboundReceivingExcel.WHS = p_str_whs_id;
                                objInboundReceivingExcel.IB_Doc_ID = p_str_ib_doc_idFm;  
                                objInboundReceivingExcel.Lot_Id = p_str_ib_doc_idTo;
                                objInboundReceivingExcel.Rcvd_Date = p_str_RcvdDtFm;
                                objInboundReceivingExcel.Loc_Id = p_str_RcvdDtTo;
                                objInboundReceivingExcel = ServiceObject.GetInboundRcvngRptByStyleExcel(objInboundReceivingExcel);
                                var model = objInboundReceivingExcel.ListInboundRcvngRptByStyleExcel.ToList();
                                GridView gv = new GridView();
                                gv.DataSource = model;
                                gv.DataBind();
                                Session["InbndStyle"] = gv;
                                if (Session["InbndStyle"] != null)
                                {
                                    return new DownloadFileActionResult((GridView)Session["InbndStyle"], "InbndStyle-"  + DateTime.Now.ToString() + ".xls");
                                }
                            }
                            else
                            {
                                InboundRcvngRptByDateExcel objInboundReceivingdateExcel = new InboundRcvngRptByDateExcel();
                                objInboundReceivingdateExcel.Container_ID = p_str_cmp_id;
                                objInboundReceivingdateExcel.WHS = p_str_whs_id;
                                objInboundReceivingdateExcel.IB_Doc_ID = p_str_ib_doc_idFm;
                                objInboundReceivingdateExcel.Lot_Id = p_str_ib_doc_idTo;
                                objInboundReceivingdateExcel.Rcvd_Date = p_str_RcvdDtFm;
                                objInboundReceivingdateExcel.Loc_Id = p_str_RcvdDtTo;
                                objInboundReceivingdateExcel = ServiceObject.GetInboundRcvngRptByDateExcel(objInboundReceivingdateExcel);
                                var model = objInboundReceivingdateExcel.ListInboundRcvngRptByDateExcel.ToList();
                                GridView gv = new GridView();
                                gv.DataSource = model;
                                gv.DataBind();
                                Session["InbndDate"] = gv;
                                if (Session["InbndDate"] != null)
                                {
                                    return new DownloadFileActionResult((GridView)Session["InbndDate"], "InbndDate-"  + DateTime.Now.ToString() + ".xls");
                                }
                            }
                            //rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);

                        }
                        //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
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
        public ActionResult RefreshReport()
        {
            return PartialView("_ReportsDisplay");
        }    
        public ActionResult ReportView()
        {
            return View();
        }
    }
}