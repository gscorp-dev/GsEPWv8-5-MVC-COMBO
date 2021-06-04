using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using static GsEPWv8_5_MVC.Core.Entity.ExceptionReports;

namespace GsEPWv8_5_MVC.Controllers
{
    public class ExceptionReportsController : Controller
    {
        // GET: ExceptionReports
        public string ScreenID = "Exception Report";
        public string Folderpath = string.Empty;
        public string l_str_rptdtl = string.Empty;
        public string strDateFormat = string.Empty;
        public string strFileName = string.Empty;
        public string reportFileName = string.Empty;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ExceptionReports(string FullFillType, string cmp, string status, string DateFm, string DateTo, string p_str_scn_id)
        {
            string l_str_cmp_id = string.Empty;
            string l_str_fm_dt = string.Empty;
            string l_str_Dflt_Dt_Reqd = string.Empty;

            try
            {
                ExceptionReports objExceptionReports = new ExceptionReports();
                ExceptionReportsService ServiceObject = new ExceptionReportsService();
                objExceptionReports.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                objExceptionReports.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                Session["g_str_Search_flag"] = "False";
                //  CR_3PL_MVC_COMMON_2018_0324_001
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                if (objExceptionReports.cmp_id == null || objExceptionReports.cmp_id == string.Empty)
                {
                    objExceptionReports.cmp_id = Session["g_str_cmp_id"].ToString().Trim();

                }
                else
                {
                    objCompany.cmp_id = Session["g_str_cmp_id"].ToString().Trim();

                }
                objExceptionReports.screentitle = p_str_scn_id;
                l_str_Dflt_Dt_Reqd = Session["DFLT_DT_REQD"].ToString().Trim();
                if (l_str_Dflt_Dt_Reqd == "N")
                {
                    DateFm = "";
                    DateTo = "";
                }
                if (FullFillType == null)
                {
                    //objCompany.cmp_id = l_str_cmp_id;
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objExceptionReports.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    DateTime date = DateTime.Now.AddMonths(-12);
                    l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");
                    objExceptionReports.SRDtFm = DateTime.Now.AddDays(Common.clsGlobal.DispDateFrom).ToString("MM/dd/yyyy") ;
                    objExceptionReports.SRDtTo = DateTime.Now.ToString("MM/dd/yyyy");
                    objExceptionReports.RcvdDtFm = DateTime.Now.AddDays(Common.clsGlobal.DispDateFrom).ToString("MM/dd/yyyy") ;
                    objExceptionReports.RcvdDtTo = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else if (FullFillType != null)
                {

                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objExceptionReports.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    objExceptionReports.cmp_id = cmp;
                    objExceptionReports.ibcmp_id = cmp;
                    objExceptionReports.obcmp_id = cmp;
                    objExceptionReports.ivcmp_id = cmp;
                    objExceptionReports.SRDtFm = DateTime.Now.AddDays(Common.clsGlobal.DispDateFrom).ToString("MM/dd/yyyy"); ;
                    objExceptionReports.SRDtTo = DateTime.Now.ToString("MM/dd/yyyy");
                    objExceptionReports.RcvdDtFm = DateTime.Now.AddDays(Common.clsGlobal.DispDateFrom).ToString("MM/dd/yyyy"); ;
                    objExceptionReports.RcvdDtTo = DateTime.Now.ToString("MM/dd/yyyy");
                    if (l_str_Dflt_Dt_Reqd == "Y")
                    {
                        objExceptionReports = ServiceObject.GetExceptionReportsInquiryDetails(objExceptionReports);
                    }
                    objCompany.cmp_id = cmp;
                    objCompany = ServiceObjectCompany.GetFullFillCompanyDetails(objCompany);
                }
                Mapper.CreateMap<ExceptionReports, ExceptionReportsModel>();
                ExceptionReportsModel ExceptionReportModel = Mapper.Map<ExceptionReports, ExceptionReportsModel>(objExceptionReports);
                return View(ExceptionReportModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ActionResult CmpIdOnChange(string p_str_cmp_id)
        {
            ExceptionReports objExceptionReports = new ExceptionReports();
            ExceptionReportsService ServiceObject = new ExceptionReportsService();
            string l_str_tmp_cmp_id = string.Empty;
            Session["g_str_cmp_id"] = p_str_cmp_id;
            l_str_tmp_cmp_id = Session["g_str_cmp_id"].ToString().Trim();
            return Json(l_str_tmp_cmp_id, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSearchExceptionReportInquiry(string p_str_cmp_id, string p_str_SR_dt_Fr, string p_str_SR_dt_To)
        {
            try
            {
                string l_str_is_another_usr = string.Empty;
                ExceptionReports objExceptionReports = new ExceptionReports();
                ExceptionReportsService ServiceObject = new ExceptionReportsService();
                objExceptionReports.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                objExceptionReports.user_id = Session["UserID"].ToString().Trim();
                Session["g_str_Search_flag"] = "True";
                objExceptionReports.cmp_id = p_str_cmp_id.Trim();
                objExceptionReports.SRDtFm = p_str_SR_dt_Fr;
                objExceptionReports.SRDtTo = p_str_SR_dt_To;
                objExceptionReports = ServiceObject.GetExceptionReportsInquiryDetails(objExceptionReports);
                Mapper.CreateMap<ExceptionReports, ExceptionReportsModel>();
                ExceptionReportsModel ExceptionReportModel = Mapper.Map<ExceptionReports, ExceptionReportsModel>(objExceptionReports);
                return PartialView("_ExceptionReports", ExceptionReportModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult ShowSummaryReport(string p_str_radio, string p_str_cmp_id, string p_str_SR_dtFm, string p_str_SR_dtTo, string type)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            string l_str_tmp_name = string.Empty; //CR - 3PL_MVC_IB_2018_0219_008
            l_str_rpt_selection = p_str_radio;
            string l_str_status = string.Empty;
            ExceptionReports objExceptionReports = new ExceptionReports();
            ExceptionReportsService ServiceObject = new ExceptionReportsService();
            CustMaster objCustMaster = new CustMaster();
            CustMasterService objCustMasterService = new CustMasterService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cmp_id = p_str_cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            objExceptionReports.LstCmpName = objCompany.LstCmpName;
            if (objExceptionReports.LstCmpName.Count > 0)
            {
                l_str_tmp_name = objExceptionReports.LstCmpName[0].cmp_name.ToString().Trim();
            }
            else
            {
                l_str_tmp_name = "";
            }
            //objCustMaster.cust_id = p_str_cmp_id;
            //objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            //if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            //{
            //    objCustMaster.ListGetCustLogo[0].cust_logo = "";
            //}
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "ExceptionRpt")
                    {
                        strReportName = "rpt_Outbound_Exception.rpt";
                        ExceptionReports objExceptionReport = new ExceptionReports();
                        ExceptionReportsService ServiceObjects = new ExceptionReportsService();
                      
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//ExceptionReports//" + strReportName;

                        objExceptionReport.cmp_id = p_str_cmp_id;
                        objExceptionReport.SRDtFm = p_str_SR_dtFm;
                        objExceptionReport.SRDtTo = p_str_SR_dtTo;
                        objExceptionReport.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                        objExceptionReport = ServiceObjects.GetExceptionReportsInquiryDetailsRpt(objExceptionReport);
                        var rptSource = objExceptionReport.LstExceptionRpt.ToList();
                       
                        int AlocCount = 0;
                        AlocCount = objExceptionReport.LstExceptionRpt.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")

                           
                        if (type == "PDF")
                        {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    rd.SetDataSource(rptSource);
                                    rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                    rd.SetParameterValue("fml_image_path", objExceptionReport.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                }
                            }
                      
                        else if (type == "Excel")
                        {
                            List<OutboundExceptionRptSummaryExcel> li = new List<OutboundExceptionRptSummaryExcel>();
                            for (int i = 0; i < objExceptionReport.LstExceptionRpt.Count; i++)
                            {

                                OutboundExceptionRptSummaryExcel objExceptionInquiryExcel = new OutboundExceptionRptSummaryExcel();
                                objExceptionInquiryExcel.CMP_ID = objExceptionReport.LstExceptionRpt[i].CMP_ID;
                                objExceptionInquiryExcel.SR_NUM = objExceptionReport.LstExceptionRpt[i].SO_NUM;
                                objExceptionInquiryExcel.SR_DT = objExceptionReport.LstExceptionRpt[i].SO_DT;
                                objExceptionInquiryExcel.ALOC_STATUS = objExceptionReport.LstExceptionRpt[i].ALOC_STATUS;
                                objExceptionInquiryExcel.STYLE = objExceptionReport.LstExceptionRpt[i].ITM_NUM;
                                objExceptionInquiryExcel.COLOR = objExceptionReport.LstExceptionRpt[i].ITM_COLOR;
                                objExceptionInquiryExcel.SIZE = objExceptionReport.LstExceptionRpt[i].ITM_SIZE;
                                objExceptionInquiryExcel.SO_QTY = objExceptionReport.LstExceptionRpt[i].SO_QTY;
                                objExceptionInquiryExcel.ALOC_DOC_ID = objExceptionReport.LstExceptionRpt[i].ALOC_DOC_ID;
                                objExceptionInquiryExcel.ALOC_QTY = objExceptionReport.LstExceptionRpt[i].ALOC_QTY;
                                objExceptionInquiryExcel.SHIP_DOC_ID = objExceptionReport.LstExceptionRpt[i].SHIP_DOC_ID;
                                objExceptionInquiryExcel.SHIP_QTY = objExceptionReport.LstExceptionRpt[i].SHIP_QTY;
                                objExceptionInquiryExcel.IN_QTY = objExceptionReport.LstExceptionRpt[i].IN_QTY;
                                objExceptionInquiryExcel.OUT_QTY = objExceptionReport.LstExceptionRpt[i].OUT_QTY;
                                objExceptionInquiryExcel.HST_QTY = objExceptionReport.LstExceptionRpt[i].HST_QTY;
                                li.Add(objExceptionInquiryExcel);
                            }
                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["OBException_Rpt"] = gv;
                            return new DownloadFileActionResult((GridView)Session["OBException_Rpt"], "Outbound Exception_Rpt" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
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

        public ActionResult EmailShowReport(string p_str_radio, string p_str_cmp_id, string p_str_SR_dtFm, string p_str_SR_dtTo, string type)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("mm/dd/yyyy");
            string strToDate = DateTime.Now.ToString("mm/dd/yyyy");
            string l_str_rpt_selection = string.Empty;
            string l_str_tmp_name = string.Empty; //CR - 3PL_MVC_IB_2018_0219_008
            l_str_rpt_selection = p_str_radio;
            CustMaster objCustMaster = new CustMaster();
            CustMasterService objCustMasterService = new CustMasterService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            ExceptionReports objExceptionReport = new ExceptionReports();
            ExceptionReportsService ServiceObjects = new ExceptionReportsService();
            Email objEmail = new Email();
            EmailService objEmailService = new EmailService();
            objCompany.cmp_id = p_str_cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            objExceptionReport.LstCmpName = objCompany.LstCmpName;
            l_str_tmp_name = objExceptionReport.LstCmpName[0].cmp_name.ToString().Trim();
            objEmail.CmpId = p_str_cmp_id;
            objEmail.screenId = ScreenID;
            objEmail.username = objCompany.user_id;
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "ExceptionRpt")
                    {
                        strReportName = "rpt_Outbound_Exception.rpt";

                       
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//ExceptionReports//" + strReportName;
                        Folderpath = System.Configuration.ConfigurationManager.AppSettings["tempFilepath"].ToString().Trim();
                        objExceptionReport.cmp_id = p_str_cmp_id;
                        objEmail.CmpId = p_str_cmp_id;
                        objEmail.screenId = ScreenID;
                        objEmail.Reportselection = l_str_rpt_selection;
                        objEmail = objEmailService.GetSendMailDetails(objEmail);
                        if (objEmail.ListEamilDetail.Count != 0)
                        {
                            objEmail.EmailMessageContent = (objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == null || objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailMessageContent.Trim();
                        }
                        objExceptionReport.cmp_id = p_str_cmp_id;
                        objExceptionReport.SRDtFm = p_str_SR_dtFm;
                        objExceptionReport.SRDtTo = p_str_SR_dtTo;
                        objExceptionReport = ServiceObjects.GetExceptionReportsInquiryDetailsRpt(objExceptionReport);
                        if (objExceptionReport.LstExceptionRpt.Count > 0)
                        {
                            objExceptionReport.SO_DT = objExceptionReport.LstExceptionRpt[0].SO_DT;
                            l_str_rptdtl = objExceptionReport.cmp_id + "_" + "Exception Report Summary";
                            objEmail.EmailSubject = objExceptionReport.cmp_id + "-" + "Exception Report Summary" + "|" + " " + " " + objExceptionReport.SO_DT;
                            objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objExceptionReport.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "SO Date: " + " " + " " + objExceptionReport.SO_DT;
                        }
                        var rptSource = objExceptionReport.LstExceptionRpt.ToList();
                        using (ReportDocument rd = new ReportDocument())
                        { 
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objExceptionReport.LstExceptionRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                        }
                        reportFileName = l_str_rptdtl + "_" + strDateFormat + ".pdf";
                        Session["RptFileName"] = strFileName;
                    }
                }

                else
                {
                    Response.Write("<H2>Report not found</H2>");
                }
                objEmail.CmpId = p_str_cmp_id;
                if (objEmail.CmpId == null)
                {
                    objEmail.CmpId = "";
                }
                objEmail.Attachment = reportFileName;

                objEmail.screenId = ScreenID;
                objEmail.username = objCompany.user_id;
                objEmail.Reportselection = l_str_rpt_selection;
                objEmail = objEmailService.GetSendMailDetails(objEmail);
                if (objEmail.ListEamilDetail.Count != 0)
                {

                    objEmail.Attachment = reportFileName;
                    objEmail.EmailTo = (objEmail.ListEamilDetail[0].EmailTo.Trim() == null || objEmail.ListEamilDetail[0].EmailTo.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailTo.Trim();
                    objEmail.EmailCC = (objEmail.ListEamilDetail[0].EmailCC.Trim() == null || objEmail.ListEamilDetail[0].EmailCC.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailCC.Trim();
                    objEmail.EmailMessageContent = (objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == null || objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailMessageContent.Trim();


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
        public ActionResult GetSearchIBExceptionReportInquiry(string p_str_cmp_id, string p_str_Rcvd_dt_Fr, string p_str_Rcvd_dt_To)
        {
            try
            {
                string l_str_is_another_usr = string.Empty;
                ExceptionReports objExceptionReports = new ExceptionReports();
                ExceptionReportsService ServiceObject = new ExceptionReportsService();
                objExceptionReports.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                objExceptionReports.user_id = Session["UserID"].ToString().Trim();
                Session["g_str_Search_flag"] = "True";
                objExceptionReports.cmp_id = p_str_cmp_id.Trim();
                objExceptionReports.RcvdDtFm = p_str_Rcvd_dt_Fr;
                objExceptionReports.RcvdDtTo = p_str_Rcvd_dt_To;
                objExceptionReports = ServiceObject.GetIBExceptionReportsInquiryDetails(objExceptionReports);
                Mapper.CreateMap<ExceptionReports, ExceptionReportsModel>();
                ExceptionReportsModel ExceptionReportModel = Mapper.Map<ExceptionReports, ExceptionReportsModel>(objExceptionReports);
                return PartialView("_IBExceptionReports", ExceptionReportModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult InboundSummaryReport(string p_str_radio, string p_str_cmp_id, string p_str_Rcvd_dt_Fr, string p_str_Rcvd_dt_To, string type)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            string l_str_tmp_name = string.Empty; //CR - 3PL_MVC_IB_2018_0219_008
            l_str_rpt_selection = p_str_radio;
            string l_str_status = string.Empty;
            ExceptionReports objExceptionReports = new ExceptionReports();
            ExceptionReportsService ServiceObject = new ExceptionReportsService();
            CustMaster objCustMaster = new CustMaster();
            CustMasterService objCustMasterService = new CustMasterService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cmp_id = p_str_cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            objExceptionReports.LstCmpName = objCompany.LstCmpName;
            if (objExceptionReports.LstCmpName.Count > 0)
            {
                l_str_tmp_name = objExceptionReports.LstCmpName[0].cmp_name.ToString().Trim();
            }
            else
            {
                l_str_tmp_name = "";
            }
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "IBExceptionRpt")
                    {
                        strReportName = "rpt_Inbound_Exception.rpt";
                        ExceptionReports objExceptionReport = new ExceptionReports();
                        ExceptionReportsService ServiceObjects = new ExceptionReportsService();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//ExceptionReports//" + strReportName;

                        objExceptionReport.cmp_id = p_str_cmp_id;
                        objExceptionReport.RcvdDtFm = p_str_Rcvd_dt_Fr;
                        objExceptionReport.RcvdDtTo = p_str_Rcvd_dt_To;
                        objExceptionReport.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                        objExceptionReport = ServiceObjects.GetIBExceptionReportsInquiryDetailsRpt(objExceptionReport);
                        var rptSource = objExceptionReport.LstIBExceptionRpt.ToList();
                        
                        int AlocCount = 0;
                        AlocCount = objExceptionReport.LstIBExceptionRpt.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                          
                        if (type == "PDF")
                        {
                            using (ReportDocument rd = new ReportDocument())
                            { 
                                rd.Load(strRptPath);
                                rd.SetDataSource(rptSource);
                                rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                rd.SetParameterValue("fml_image_path", objExceptionReport.Image_Path);
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                            }
                        }
                      
                        else if (type == "Excel")
                        {
                            List<inboundExceptionRptSummaryExcel> li = new List<inboundExceptionRptSummaryExcel>();
                            for (int i = 0; i < objExceptionReport.LstIBExceptionRpt.Count; i++)
                            {

                                inboundExceptionRptSummaryExcel objExceptionInquiryExcel = new inboundExceptionRptSummaryExcel();
                                objExceptionInquiryExcel.CMP_ID = objExceptionReport.LstExceptionRpt[i].CMP_ID;
                                objExceptionInquiryExcel.IB_DOC_ID = objExceptionReport.LstExceptionRpt[i].IB_DOC_ID;
                                objExceptionInquiryExcel.LOT_STATUS = objExceptionReport.LstExceptionRpt[i].LOT_STATUS;
                                objExceptionInquiryExcel.LOT_ID = objExceptionReport.LstExceptionRpt[i].LOT_ID;
                                objExceptionInquiryExcel.RCVD_DT = objExceptionReport.LstExceptionRpt[i].RCVD_DT;
                                objExceptionInquiryExcel.STYLE = objExceptionReport.LstExceptionRpt[i].ITM_NUM;
                                objExceptionInquiryExcel.COLOR = objExceptionReport.LstExceptionRpt[i].ITM_COLOR;
                                objExceptionInquiryExcel.SIZE = objExceptionReport.LstExceptionRpt[i].ITM_SIZE;
                                objExceptionInquiryExcel.LOT_QTY = objExceptionReport.LstExceptionRpt[i].LOT_QTY;
                                objExceptionInquiryExcel.RCVD_QTY = objExceptionReport.LstExceptionRpt[i].RCVD_QTY;
                                objExceptionInquiryExcel.IN_QTY = objExceptionReport.LstExceptionRpt[i].IN_QTY;
                                objExceptionInquiryExcel.OUT_QTY = objExceptionReport.LstExceptionRpt[i].OUT_QTY;
                                objExceptionInquiryExcel.HST_QTY = objExceptionReport.LstExceptionRpt[i].HST_QTY;
                                li.Add(objExceptionInquiryExcel);
                            }
                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["IBException_Rpt"] = gv;
                            return new DownloadFileActionResult((GridView)Session["IBException_Rpt"], "Inbound Exception_Rpt" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
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
        public ActionResult IBEmailShowReport(string p_str_radio, string p_str_cmp_id, string p_str_Rcvd_dt_Fr, string p_str_Rcvd_dt_To, string type)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("mm/dd/yyyy");
            string strToDate = DateTime.Now.ToString("mm/dd/yyyy");
            string l_str_rpt_selection = string.Empty;
            string l_str_tmp_name = string.Empty; //CR - 3PL_MVC_IB_2018_0219_008
            l_str_rpt_selection = p_str_radio;
            CustMaster objCustMaster = new CustMaster();
            CustMasterService objCustMasterService = new CustMasterService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            ExceptionReports objExceptionReport = new ExceptionReports();
            ExceptionReportsService ServiceObjects = new ExceptionReportsService();
            Email objEmail = new Email();
            EmailService objEmailService = new EmailService();
            objCompany.cmp_id = p_str_cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            objExceptionReport.LstCmpName = objCompany.LstCmpName;
            l_str_tmp_name = objExceptionReport.LstCmpName[0].cmp_name.ToString().Trim();
            objEmail.CmpId = p_str_cmp_id;
            objEmail.screenId = ScreenID;
            objEmail.username = objCompany.user_id;
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "IBExceptionRpt")
                    {
                        strReportName = "rpt_Inbound_Exception.rpt";

                        
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//ExceptionReports//" + strReportName;
                        Folderpath = System.Configuration.ConfigurationManager.AppSettings["tempFilepath"].ToString().Trim();
                        objExceptionReport.cmp_id = p_str_cmp_id;
                        objEmail.CmpId = p_str_cmp_id;
                        objEmail.screenId = ScreenID;
                        objEmail.Reportselection = l_str_rpt_selection;
                        objEmail = objEmailService.GetSendMailDetails(objEmail);
                        if (objEmail.ListEamilDetail.Count != 0)
                        {
                            objEmail.EmailMessageContent = (objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == null || objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailMessageContent.Trim();
                        }
                        objExceptionReport.cmp_id = p_str_cmp_id;
                        objExceptionReport.RcvdDtFm = p_str_Rcvd_dt_Fr;
                        objExceptionReport.RcvdDtTo = p_str_Rcvd_dt_To;
                        objExceptionReport.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                        objExceptionReport = ServiceObjects.GetIBExceptionReportsInquiryDetailsRpt(objExceptionReport);
                        if (objExceptionReport.LstIBExceptionRpt.Count > 0)
                        {
                            objExceptionReport.RCVD_DT = objExceptionReport.LstIBExceptionRpt[0].RCVD_DT;
                            l_str_rptdtl = objExceptionReport.cmp_id + "_" + "Exception Report Summary";
                            objEmail.EmailSubject = objExceptionReport.cmp_id + "-" + "Exception Report Summary" + "|" + " " + " " + objExceptionReport.RCVD_DT;
                            objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objExceptionReport.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "RCVD Date: " + " " + " " + objExceptionReport.RCVD_DT;
                        }
                        var rptSource = objExceptionReport.LstIBExceptionRpt.ToList();
                        using (ReportDocument rd = new ReportDocument())
                        { 
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objExceptionReport.LstIBExceptionRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                        }
                        reportFileName = l_str_rptdtl + "_" + strDateFormat + ".pdf";
                        Session["RptFileName"] = strFileName;
                    }
                }

                else
                {
                    Response.Write("<H2>Report not found</H2>");
                }
                objEmail.CmpId = p_str_cmp_id;
                if (objEmail.CmpId == null)
                {
                    objEmail.CmpId = "";
                }
                objEmail.Attachment = reportFileName;

                objEmail.screenId = ScreenID;
                objEmail.username = objCompany.user_id;
                objEmail.Reportselection = l_str_rpt_selection;
                objEmail = objEmailService.GetSendMailDetails(objEmail);
                if (objEmail.ListEamilDetail.Count != 0)
                {

                    objEmail.Attachment = reportFileName;
                    objEmail.EmailTo = (objEmail.ListEamilDetail[0].EmailTo.Trim() == null || objEmail.ListEamilDetail[0].EmailTo.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailTo.Trim();
                    objEmail.EmailCC = (objEmail.ListEamilDetail[0].EmailCC.Trim() == null || objEmail.ListEamilDetail[0].EmailCC.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailCC.Trim();
                    objEmail.EmailMessageContent = (objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == null || objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailMessageContent.Trim();


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
        public JsonResult ItemXGetitmDtl(string term, string cmp_id)
        {
            ExceptionReportsService ServiceObject = new ExceptionReportsService();
            var List = ServiceObject.ItemXGetitmDetails(term, cmp_id.Trim()).LstItmDtl.Select(x => new { label = x.Itmdtl, value = x.ITM_NUM, ITM_NUM = x.ITM_NUM, ITM_COLOR = x.ITM_COLOR, ITM_SIZE = x.ITM_SIZE, ITM_NAME = x.ITM_NAME, ITM_CODE = x.ITM_CODE }).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSearchIVExceptionReportInquiry(string p_str_cmp_id, string p_str_itm_code)
        {
            try
            {
                string l_str_is_another_usr = string.Empty;
                ExceptionReports objExceptionReports = new ExceptionReports();
                ExceptionReportsService ServiceObject = new ExceptionReportsService();
                objExceptionReports.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                objExceptionReports.user_id = Session["UserID"].ToString().Trim();
                Session["g_str_Search_flag"] = "True";
                objExceptionReports.cmp_id = p_str_cmp_id.Trim();
                objExceptionReports.ITM_CODE = p_str_itm_code;
                objExceptionReports = ServiceObject.GetIVExceptionReportsInqdtl(objExceptionReports);
                Mapper.CreateMap<ExceptionReports, ExceptionReportsModel>();
                ExceptionReportsModel ExceptionReportModel = Mapper.Map<ExceptionReports, ExceptionReportsModel>(objExceptionReports);
                return PartialView("_IVExceptionReports", ExceptionReportModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult InventorySummaryReport(string p_str_radio, string p_str_cmp_id, string p_str_itm_code, string type)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            string l_str_tmp_name = string.Empty; //CR - 3PL_MVC_IB_2018_0219_008
            l_str_rpt_selection = p_str_radio;
            string l_str_status = string.Empty;
            ExceptionReports objExceptionReports = new ExceptionReports();
            ExceptionReportsService ServiceObject = new ExceptionReportsService();
            CustMaster objCustMaster = new CustMaster();
            CustMasterService objCustMasterService = new CustMasterService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cmp_id = p_str_cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            objExceptionReports.LstCmpName = objCompany.LstCmpName;
            if (objExceptionReports.LstCmpName.Count > 0)
            {
                l_str_tmp_name = objExceptionReports.LstCmpName[0].cmp_name.ToString().Trim();
            }
            else
            {
                l_str_tmp_name = "";
            }
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "IVExceptionRpt")
                    {
                        strReportName = "rpt_Inventory_Exception.rpt";
                        ExceptionReports objExceptionReport = new ExceptionReports();
                        ExceptionReportsService ServiceObjects = new ExceptionReportsService();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//ExceptionReports//" + strReportName;

                        objExceptionReport.cmp_id = p_str_cmp_id;
                        objExceptionReport.ITM_CODE = p_str_itm_code;
                        objExceptionReport.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                        objExceptionReport = ServiceObjects.GetIVExceptionReportsInqdtlRpt(objExceptionReport);
                        var rptSource = objExceptionReport.LstIVExceptionRpt.ToList();
                       
                        int AlocCount = 0;
                        AlocCount = objExceptionReport.LstIVExceptionRpt.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                        { 
                           
                        if (type == "PDF")
                        {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    rd.SetDataSource(rptSource);
                                    rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                    rd.SetParameterValue("fml_image_path", objExceptionReport.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                }
                            }
                       
                        else if (type == "Excel")
                        {
                            List<inventoryExceptionRptSummaryExcel> li = new List<inventoryExceptionRptSummaryExcel>();
                            for (int i = 0; i < objExceptionReport.LstIVExceptionRpt.Count; i++)
                            {

                                inventoryExceptionRptSummaryExcel objExceptionInquiryExcel = new inventoryExceptionRptSummaryExcel();
                                objExceptionInquiryExcel.CMP_ID = objExceptionReport.LstIVExceptionRpt[i].CMP_ID;
                                objExceptionInquiryExcel.ITM_CODE = objExceptionReport.LstIVExceptionRpt[i].ITM_CODE;
                                objExceptionInquiryExcel.ITM_NUM = objExceptionReport.LstIVExceptionRpt[i].ITM_NUM;
                                objExceptionInquiryExcel.ITM_COLOR = objExceptionReport.LstIVExceptionRpt[i].ITM_COLOR;
                                objExceptionInquiryExcel.ITM_SIZE = objExceptionReport.LstIVExceptionRpt[i].ITM_SIZE;
                                objExceptionInquiryExcel.RCVD_QTY = objExceptionReport.LstIVExceptionRpt[i].RCVD_QTY;
                                objExceptionInquiryExcel.ALOC_QTY = objExceptionReport.LstIVExceptionRpt[i].ALOC_QTY;
                                objExceptionInquiryExcel.ALOC_QTY_OUT = objExceptionReport.LstIVExceptionRpt[i].ALOC_QTY_OUT;
                                objExceptionInquiryExcel.AVAIL_QTY_IN = objExceptionReport.LstIVExceptionRpt[i].AVAIL_QTY_IN;
                                li.Add(objExceptionInquiryExcel);
                            }
                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["IVException_Rpt"] = gv;
                            return new DownloadFileActionResult((GridView)Session["IVException_Rpt"], "Inventory Exception_Rpt" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
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
        public ActionResult IVEmailShowReport(string p_str_radio, string p_str_cmp_id, string p_str_itm_code, string type)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("mm/dd/yyyy");
            string strToDate = DateTime.Now.ToString("mm/dd/yyyy");
            string l_str_rpt_selection = string.Empty;
            string l_str_tmp_name = string.Empty; //CR - 3PL_MVC_IB_2018_0219_008
            l_str_rpt_selection = p_str_radio;
            CustMaster objCustMaster = new CustMaster();
            CustMasterService objCustMasterService = new CustMasterService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            ExceptionReports objExceptionReport = new ExceptionReports();
            ExceptionReportsService ServiceObjects = new ExceptionReportsService();
            Email objEmail = new Email();
            EmailService objEmailService = new EmailService();
            objCompany.cmp_id = p_str_cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            objExceptionReport.LstCmpName = objCompany.LstCmpName;
            l_str_tmp_name = objExceptionReport.LstCmpName[0].cmp_name.ToString().Trim();
            objEmail.CmpId = p_str_cmp_id;
            objEmail.screenId = ScreenID;
            objEmail.username = objCompany.user_id;
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "IVExceptionRpt")
                    {
                        strReportName = "rpt_Inventory_Exception.rpt";

                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//ExceptionReports//" + strReportName;
                        Folderpath = System.Configuration.ConfigurationManager.AppSettings["tempFilepath"].ToString().Trim();
                        objExceptionReport.cmp_id = p_str_cmp_id;
                        objEmail.CmpId = p_str_cmp_id;
                        objEmail.screenId = ScreenID;
                        objEmail.Reportselection = l_str_rpt_selection;
                        objEmail = objEmailService.GetSendMailDetails(objEmail);
                        if (objEmail.ListEamilDetail.Count != 0)
                        {
                            objEmail.EmailMessageContent = (objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == null || objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailMessageContent.Trim();
                        }
                        objExceptionReport.cmp_id = p_str_cmp_id;
                        objExceptionReport.ITM_CODE = p_str_itm_code;
                        objExceptionReport.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                        objExceptionReport = ServiceObjects.GetIVExceptionReportsInqdtlRpt(objExceptionReport);
                        if (objExceptionReport.LstIVExceptionRpt.Count > 0)
                        {
                            objExceptionReport.ALOC_QTY = objExceptionReport.LstIVExceptionRpt[0].ALOC_QTY;
                            objExceptionReport.RCVD_QTY = objExceptionReport.LstIVExceptionRpt[0].RCVD_QTY;
                            objExceptionReport.AVAIL_QTY_IN = objExceptionReport.LstIVExceptionRpt[0].AVAIL_QTY_IN;
                            objExceptionReport.ALOC_QTY_OUT = objExceptionReport.LstIVExceptionRpt[0].ALOC_QTY_OUT;
                            l_str_rptdtl = objExceptionReport.cmp_id + "_" + "Exception Report Summary";
                            objEmail.EmailSubject = objExceptionReport.cmp_id + "-" + "Exception Report Summary" + "|" + " " + " " + objExceptionReport.RCVD_QTY;
                            objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objExceptionReport.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "RCVD Qty: " + " " + " " + objExceptionReport.RCVD_QTY;
                        }
                        var rptSource = objExceptionReport.LstIVExceptionRpt.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objExceptionReport.LstIVExceptionRpt.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                        reportFileName = l_str_rptdtl + "_" + strDateFormat + ".pdf";
                        Session["RptFileName"] = strFileName;
                    }
                }

                else
                {
                    Response.Write("<H2>Report not found</H2>");
                }
                objEmail.CmpId = p_str_cmp_id;
                if (objEmail.CmpId == null)
                {
                    objEmail.CmpId = "";
                }
                objEmail.Attachment = reportFileName;

                objEmail.screenId = ScreenID;
                objEmail.username = objCompany.user_id;
                objEmail.Reportselection = l_str_rpt_selection;
                objEmail = objEmailService.GetSendMailDetails(objEmail);
                if (objEmail.ListEamilDetail.Count != 0)
                {

                    objEmail.Attachment = reportFileName;
                    objEmail.EmailTo = (objEmail.ListEamilDetail[0].EmailTo.Trim() == null || objEmail.ListEamilDetail[0].EmailTo.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailTo.Trim();
                    objEmail.EmailCC = (objEmail.ListEamilDetail[0].EmailCC.Trim() == null || objEmail.ListEamilDetail[0].EmailCC.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailCC.Trim();
                    objEmail.EmailMessageContent = (objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == null || objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailMessageContent.Trim();


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
    }
}