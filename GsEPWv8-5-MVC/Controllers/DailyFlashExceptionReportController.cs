using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GsEPWv8_5_MVC.Controllers
{
    public class DailyFlashExceptionReportController : Controller
    {
        // GET: DailyFlashExceptionReport
        public bool l_str_include_entry_dtls;
        public string prefixtotaltime = string.Empty;
        public string suffixtotaltime = string.Empty;
        public string totaltime = string.Empty;
        public string ScreenID = "ExceptionReports";
        public string Folderpath = string.Empty;
        public string l_str_rptdtl = string.Empty;
        public string strDateFormat = string.Empty;
        public string strFileName = string.Empty;
        public string reportFileName = string.Empty;
        DailyFlashExceptionReport ObjDailyFlashExceptionReport = new DailyFlashExceptionReport();
        DailyFlashExceptionReportService ServiceObject = new DailyFlashExceptionReportService();
        Company objCompany = new Company();
        CompanyService ServiceObjectCompany = new CompanyService();
        ExceptionReports objExceptionReport = new ExceptionReports();
        ExceptionReportsService ServiceObjects = new ExceptionReportsService();
        Email objEmail = new Email();
        EmailService objEmailService = new EmailService();
        MailConfig objMailConfig = new MailConfig();
        MailConfigService ServiceMailConfig = new MailConfigService();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DailyFlashExceptionReport(string FullFillType, string cmp)
        {
            string l_str_cmp_id = string.Empty;
            string l_str_fm_dt = string.Empty;
            string l_str_Dflt_Dt_Reqd = string.Empty;

            try
            {
                ObjDailyFlashExceptionReport.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                ObjDailyFlashExceptionReport.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                Session["g_str_Search_flag"] = "False";
             
                if (ObjDailyFlashExceptionReport.cmp_id == null || ObjDailyFlashExceptionReport.cmp_id == string.Empty)
                {
                    ObjDailyFlashExceptionReport.cmp_id = Session["g_str_cmp_id"].ToString().Trim();

                }
                else
                {
                    objCompany.cmp_id = Session["g_str_cmp_id"].ToString().Trim();

                }
               
                if (FullFillType == null)
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    ObjDailyFlashExceptionReport.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    ObjDailyFlashExceptionReport.cmp_id = cmp;

                }
                else if (FullFillType != null)
                {

                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    ObjDailyFlashExceptionReport.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    ObjDailyFlashExceptionReport.cmp_id = cmp;
                 
                }
                ObjDailyFlashExceptionReport.scn_id = "ExceptionReports";
                ObjDailyFlashExceptionReport.rpt_id = "";
                ObjDailyFlashExceptionReport.is_daily_flash = "Yes";
                ObjDailyFlashExceptionReport = ServiceObject.GetExceptionReportName(ObjDailyFlashExceptionReport);
                ObjDailyFlashExceptionReport.cmp_id = "";
                ObjDailyFlashExceptionReport.rpt_id = "";
                ObjDailyFlashExceptionReport.email_to = "";
                ObjDailyFlashExceptionReport.email_cc = "";
                ObjDailyFlashExceptionReport.email_msg = "";
                ObjDailyFlashExceptionReport = ServiceObject.GetExceptionReportDetails(ObjDailyFlashExceptionReport);
                Mapper.CreateMap<DailyFlashExceptionReport, DailyFlashExceptionReportModel>();
                DailyFlashExceptionReportModel ObjDailyFlashExceptionReportModel = Mapper.Map<DailyFlashExceptionReport, DailyFlashExceptionReportModel>(ObjDailyFlashExceptionReport);
                return View(ObjDailyFlashExceptionReportModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
           
        }

        public  ActionResult GetDailyFlashExceptionReportInquiryDetails(string p_str_cmp_id,string p_str_rpt_id,string p_str_email_to,string p_str_email_cc,string p_str_status,string p_str_email_msg)    
        {
            ObjDailyFlashExceptionReport.cmp_id = p_str_cmp_id;
            ObjDailyFlashExceptionReport.rpt_id = p_str_rpt_id;
            ObjDailyFlashExceptionReport.email_to = p_str_email_to;
            ObjDailyFlashExceptionReport.email_cc = p_str_email_cc;
            ObjDailyFlashExceptionReport.Status = p_str_status;
            ObjDailyFlashExceptionReport.email_msg = p_str_email_msg;
            ObjDailyFlashExceptionReport = ServiceObject.GetExceptionReportDetails(ObjDailyFlashExceptionReport);
            Mapper.CreateMap<DailyFlashExceptionReport, DailyFlashExceptionReportModel>();
            DailyFlashExceptionReportModel ObjDailyFlashExceptionReportModel = Mapper.Map<DailyFlashExceptionReport, DailyFlashExceptionReportModel>(ObjDailyFlashExceptionReport);
            return PartialView("_DailyFlashExceptionReport",ObjDailyFlashExceptionReportModel);
           
        }
        public ActionResult NewExceptionReportConfig(string p_str_cmp_id)
        {
            ObjDailyFlashExceptionReport.cmp_id = p_str_cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            ObjDailyFlashExceptionReport.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            ObjDailyFlashExceptionReport.cmp_id = p_str_cmp_id;
            ObjDailyFlashExceptionReport.scn_id = "ExceptionReports";
            ObjDailyFlashExceptionReport.rpt_id = "";
            ObjDailyFlashExceptionReport.is_daily_flash = "Yes";
            ObjDailyFlashExceptionReport = ServiceObject.GetExceptionReportName(ObjDailyFlashExceptionReport);
            ObjDailyFlashExceptionReport.action = "1";
            Mapper.CreateMap<DailyFlashExceptionReport, DailyFlashExceptionReportModel>();
            DailyFlashExceptionReportModel ObjDailyFlashExceptionReportModel = Mapper.Map<DailyFlashExceptionReport, DailyFlashExceptionReportModel>(ObjDailyFlashExceptionReport);
            return PartialView("_ExceptionReportConfig", ObjDailyFlashExceptionReportModel);
        }
        public ActionResult DailyFlashExceptionReportDetailEditView(string p_str_cmp_id, string p_str_rpt_id, string p_str_mail_to, string p_str_mail_cc, string p_str_content, string p_str_status, string p_str_action, string p_str_RunningDays, string p_str_Time)
        {
            ObjDailyFlashExceptionReport.cmp_id = p_str_cmp_id;
            ObjDailyFlashExceptionReport.rpt_id = p_str_rpt_id;
            ObjDailyFlashExceptionReport.rpt_run_days = p_str_RunningDays;
            totaltime = p_str_Time;
            prefixtotaltime = totaltime.Substring(0, 2);
            suffixtotaltime = totaltime.Substring(3);
            ObjDailyFlashExceptionReport.Time = prefixtotaltime;
            ObjDailyFlashExceptionReport.Timemin = suffixtotaltime;
            ObjDailyFlashExceptionReport.email_to = p_str_mail_to;
            ObjDailyFlashExceptionReport.email_cc = p_str_mail_cc;
            ObjDailyFlashExceptionReport.email_msg = p_str_content;
            ObjDailyFlashExceptionReport.Status = p_str_status;
            ObjDailyFlashExceptionReport.action = p_str_action;
            ObjDailyFlashExceptionReport.scn_id = "ExceptionReports";
            ObjDailyFlashExceptionReport.is_daily_flash = "Yes";
            ObjDailyFlashExceptionReport = ServiceObject.GetExceptionReportName(ObjDailyFlashExceptionReport);
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            ObjDailyFlashExceptionReport.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            var listDaysList = new List<DaysDetailList>()
            {
                new DaysDetailList {  DaysID="MON", Days = "MON"},
                new DaysDetailList {  DaysID="TUE", Days = "TUE"},
                new DaysDetailList {  DaysID="WED", Days = "WED"},
                new DaysDetailList {  DaysID="THU", Days = "THU"},
                new DaysDetailList {  DaysID="FRI", Days = "FRI"},
                new DaysDetailList {  DaysID="SAT", Days = "SAT"},
                new DaysDetailList {  DaysID="SUN", Days = "SUN"}
            };
            ObjDailyFlashExceptionReport.ListDays = listDaysList.ToList();
            ObjDailyFlashExceptionReport.SelectedDaysID = (ObjDailyFlashExceptionReport.rpt_run_days != null) ? ObjDailyFlashExceptionReport.rpt_run_days.Split(';').ToList() : null;
            ObjDailyFlashExceptionReport.cmp_id = p_str_cmp_id;
            ObjDailyFlashExceptionReport.rpt_name = p_str_rpt_id;
            Mapper.CreateMap<DailyFlashExceptionReport, DailyFlashExceptionReportModel>();
            DailyFlashExceptionReportModel ObjDailyFlashExceptionReportModel = Mapper.Map<DailyFlashExceptionReport, DailyFlashExceptionReportModel>(ObjDailyFlashExceptionReport);
            return PartialView("_ExceptionReportConfig", ObjDailyFlashExceptionReportModel);
        }

        public ActionResult SaveExceptionReportConfig(string p_str_cmp_id, string p_str_rpt_id, string p_str_mail_to, string p_str_mail_cc, string p_str_content, string p_str_status,string p_str_action,string p_str_RunningDays,string p_str_Time)
        {
            ObjDailyFlashExceptionReport.cmp_id = p_str_cmp_id;
            ObjDailyFlashExceptionReport.rpt_id = p_str_rpt_id;
            ObjDailyFlashExceptionReport.rpt_run_days = p_str_RunningDays;
            ObjDailyFlashExceptionReport.rpt_run_time = p_str_Time;
            ObjDailyFlashExceptionReport.email_to = p_str_mail_to;
            ObjDailyFlashExceptionReport.email_cc = p_str_mail_cc;
            ObjDailyFlashExceptionReport.email_msg = p_str_content;
            ObjDailyFlashExceptionReport.dflt_frmt = "PDF";
            ObjDailyFlashExceptionReport.Status = p_str_status;
            ObjDailyFlashExceptionReport.action = p_str_action;
            ObjDailyFlashExceptionReport.scn_id = "ExceptionReports";
            ObjDailyFlashExceptionReport.is_daily_flash = "Yes";
            ObjDailyFlashExceptionReport = ServiceObject.GetExceptionReportName(ObjDailyFlashExceptionReport);
            if (ObjDailyFlashExceptionReport.ListReportName.Count > 0)
            {
                ObjDailyFlashExceptionReport.rpt_name = ObjDailyFlashExceptionReport.ListReportName[0].rpt_name;
                ObjDailyFlashExceptionReport.rpt_description = ObjDailyFlashExceptionReport.ListReportName[0].rpt_description;
            }
            else
            {
                ObjDailyFlashExceptionReport.rpt_name ="";
                ObjDailyFlashExceptionReport.rpt_description ="";
            }
            if (ObjDailyFlashExceptionReport.action == "1")
            {
                ObjDailyFlashExceptionReport = ServiceObject.GetExistingJobDetails(ObjDailyFlashExceptionReport);
               
                ObjDailyFlashExceptionReport.ExistingRecords = ObjDailyFlashExceptionReport.ListExceptionRptDetails.Count();

                if (ObjDailyFlashExceptionReport.ExistingRecords == 0)
                {
                    ObjDailyFlashExceptionReport = ServiceObject.InsertExceptionReportDetails(ObjDailyFlashExceptionReport);
                    ObjDailyFlashExceptionReport = ServiceObject.InsertExceptionReportMailConfigDetails(ObjDailyFlashExceptionReport);
                    l_str_include_entry_dtls = true;

                }
                else
                {
                    l_str_include_entry_dtls = false;
                }
            }
            else
            {
                ObjDailyFlashExceptionReport = ServiceObject.InsertExceptionReportDetails(ObjDailyFlashExceptionReport);
                ObjDailyFlashExceptionReport = ServiceObject.InsertExceptionReportMailConfigDetails(ObjDailyFlashExceptionReport);
                l_str_include_entry_dtls = true;
            }
           
            return Json(l_str_include_entry_dtls, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmailShowReport(string p_str_cmp_id, string p_str_rpt_id, string p_str_SR_dtFm, string p_str_SR_dtTo,string p_str_itm_code, string type)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            string l_str_tmp_name = string.Empty; //CR - 3PL_MVC_IB_2018_0219_008
            string l_str_status = string.Empty;
            l_str_rpt_selection = p_str_rpt_id;
            objCompany.cmp_id = p_str_cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            objExceptionReport.LstCmpName = objCompany.LstCmpName;
            if (objExceptionReport.LstCmpName.Count > 0)
            {
                l_str_tmp_name = objExceptionReport.LstCmpName[0].cmp_name.ToString().Trim();
            }
            else
            {
                l_str_tmp_name = "";
            }
            objEmail.CmpId = p_str_cmp_id;
            objEmail.screenId = ScreenID;
            objEmail.username = objCompany.user_id;
            ObjDailyFlashExceptionReport.scn_id = "ExceptionReports";
            ObjDailyFlashExceptionReport.is_daily_flash = "Yes";
            ObjDailyFlashExceptionReport.rpt_id = p_str_rpt_id;
            ObjDailyFlashExceptionReport = ServiceObject.GetExceptionReportName(ObjDailyFlashExceptionReport);
            if (ObjDailyFlashExceptionReport.ListReportName.Count > 0)
            {
                ObjDailyFlashExceptionReport.rpt_description = ObjDailyFlashExceptionReport.ListReportName[0].rpt_description;
            }
            else
            {
                ObjDailyFlashExceptionReport.rpt_description = "";
            }

            try
            {
                if (isValid)
                {
                    if (p_str_rpt_id == "Rpt_OB_Exception")
                    {
                        strReportName = "rpt_Outbound_Exception.rpt";

            
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//ExceptionReports//" + strReportName;
                        Folderpath = System.Configuration.ConfigurationManager.AppSettings["tempFilepath"].ToString().Trim();
                        objExceptionReport.cmp_id = p_str_cmp_id;
                        objEmail.CmpId = p_str_cmp_id;
                        objEmail.screenId = ScreenID;
                        objEmail.Reportselection = ObjDailyFlashExceptionReport.rpt_description;
                        objExceptionReport.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
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
                           
                            if (objExceptionReport.cmp_id==""|| objExceptionReport.cmp_id==null)
                            {
                                objExceptionReport.cmp_id = "ALL";
                            }
                                l_str_rptdtl = objExceptionReport.cmp_id + "_" + "Exception Report Summary";
                            objEmail.EmailSubject = objExceptionReport.cmp_id + "-" + "Exception Report Summary" + "|" + " " + " " + objExceptionReport.SO_DT;
                            objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objExceptionReport.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "SO Date: " + " " + " " + objExceptionReport.SO_DT;
                        }
                        var rptSource = objExceptionReport.LstExceptionRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);

                                int AlocCount = 0;
                                AlocCount = objExceptionReport.LstExceptionRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                rd.SetParameterValue("fml_image_path", objExceptionReport.Image_Path);
                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            }
                        }
                        reportFileName = l_str_rptdtl + "_" + strDateFormat + ".pdf";
                        Session["RptFileName"] = strFileName;
                    }


                    if (p_str_rpt_id == "Rpt_IB_Exception")
                    {
                        strReportName = "rpt_Inbound_Exception.rpt";

              
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//ExceptionReports//" + strReportName;
                        Folderpath = System.Configuration.ConfigurationManager.AppSettings["tempFilepath"].ToString().Trim();
                        objExceptionReport.cmp_id = p_str_cmp_id;
                        objEmail.CmpId = p_str_cmp_id;
                        objEmail.screenId = ScreenID;
                        objEmail.Reportselection = ObjDailyFlashExceptionReport.rpt_description;
                        objEmail = objEmailService.GetSendMailDetails(objEmail);
                        if (objEmail.ListEamilDetail.Count != 0)
                        {
                            objEmail.EmailMessageContent = (objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == null || objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailMessageContent.Trim();
                        }
                        objExceptionReport.cmp_id = p_str_cmp_id;
                        objExceptionReport.RcvdDtFm = p_str_SR_dtFm;
                        objExceptionReport.RcvdDtTo = p_str_SR_dtTo;
                        objExceptionReport.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                        objExceptionReport = ServiceObjects.GetIBExceptionReportsInquiryDetailsRpt(objExceptionReport);
                        if (objExceptionReport.LstIBExceptionRpt.Count > 0)
                        {
                            objExceptionReport.RCVD_DT = objExceptionReport.LstIBExceptionRpt[0].RCVD_DT;
                            if (objExceptionReport.cmp_id == "" || objExceptionReport.cmp_id == null)
                            {
                                objExceptionReport.cmp_id = "ALL";
                            }
                            l_str_rptdtl = objExceptionReport.cmp_id + "_" + "Exception Report Summary";
                            objEmail.EmailSubject = objExceptionReport.cmp_id + "-" + "Exception Report Summary" + "|" + " " + " " + objExceptionReport.RCVD_DT;
                            objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objExceptionReport.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "RCVD Date: " + " " + " " + objExceptionReport.RCVD_DT;
                        }
                        var rptSource = objExceptionReport.LstIBExceptionRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);

                                int AlocCount = 0;
                                AlocCount = objExceptionReport.LstIBExceptionRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                rd.SetParameterValue("fml_image_path", objExceptionReport.Image_Path);
                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            }
                        }
                        reportFileName = l_str_rptdtl + "_" + strDateFormat + ".pdf";
                        Session["RptFileName"] = strFileName;
                    }


                    if (p_str_rpt_id == "Rpt_IV_Exception")
                    {
                        strReportName = "rpt_Inventory_Exception.rpt";


                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//ExceptionReports//" + strReportName;
                        Folderpath = System.Configuration.ConfigurationManager.AppSettings["tempFilepath"].ToString().Trim();
                        objExceptionReport.cmp_id = p_str_cmp_id;
                        objEmail.CmpId = p_str_cmp_id;
                        objEmail.screenId = ScreenID;
                        objEmail.Reportselection = ObjDailyFlashExceptionReport.rpt_description;
                        objEmail = objEmailService.GetSendMailDetails(objEmail);
                        if (objEmail.ListEamilDetail.Count != 0)
                        {
                            objEmail.EmailMessageContent = (objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == null || objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailMessageContent.Trim();
                        }
                        objExceptionReport.cmp_id = p_str_cmp_id;
                        objExceptionReport.ITM_CODE = p_str_itm_code;
                        objExceptionReport.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); 

                        objExceptionReport = ServiceObjects.GetIVExceptionReportsInqdtlRpt(objExceptionReport);
                        if (objExceptionReport.LstIVExceptionRpt.Count > 0)
                        {
                            objExceptionReport.ALOC_QTY = objExceptionReport.LstIVExceptionRpt[0].ALOC_QTY;
                            objExceptionReport.RCVD_QTY = objExceptionReport.LstIVExceptionRpt[0].RCVD_QTY;
                            objExceptionReport.AVAIL_QTY_IN = objExceptionReport.LstIVExceptionRpt[0].AVAIL_QTY_IN;
                            objExceptionReport.ALOC_QTY_OUT = objExceptionReport.LstIVExceptionRpt[0].ALOC_QTY_OUT;
                            if (objExceptionReport.cmp_id == "" || objExceptionReport.cmp_id == null)
                            {
                                objExceptionReport.cmp_id = "ALL";
                            }
                            l_str_rptdtl = objExceptionReport.cmp_id + "_" + "InventoryException Report Summary";
                            objEmail.EmailSubject = objExceptionReport.cmp_id + "-" + "InventoryException Report Summary" + "|" + " " + " " + objExceptionReport.RCVD_QTY;
                            objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objExceptionReport.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "RCVD Qty: " + " " + " " + objExceptionReport.RCVD_QTY;
                        }
                        
                        var rptSource = objExceptionReport.LstIVExceptionRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);

                                int AlocCount = 0;
                                AlocCount = objExceptionReport.LstIVExceptionRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                rd.SetParameterValue("fml_image_path", objExceptionReport.Image_Path);
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
                objEmail.CmpId = p_str_cmp_id;
                if (objEmail.CmpId == null)
                {
                    objEmail.CmpId = "";
                }
                objEmail.Attachment = reportFileName;

                objEmail.screenId = ScreenID;
                objEmail.username = objCompany.user_id;
                objEmail.Reportselection = ObjDailyFlashExceptionReport.rpt_description;
                objEmail = objEmailService.GetSendMailDetails(objEmail);
                if (objEmail.ListEamilDetail.Count != 0)
                {

                    objEmail.Attachment = reportFileName;
                    objEmail.EmailTo = (objEmail.ListEamilDetail[0].EmailTo.Trim() == null || objEmail.ListEamilDetail[0].EmailTo.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailTo.Trim();
                    objEmail.EmailCC = (objEmail.ListEamilDetail[0].EmailCC.Trim() == null || objEmail.ListEamilDetail[0].EmailCC.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailCC.Trim();
                 


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
        public ActionResult LoadCompanyList(string p_str_cmp_id, string p_str_selected_cmp_list)
        {
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            ObjDailyFlashExceptionReport.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            if (p_str_selected_cmp_list != null)
            {
                ObjDailyFlashExceptionReport.selected_company = p_str_selected_cmp_list;
            }
            Mapper.CreateMap<DailyFlashExceptionReport, DailyFlashExceptionReportModel>();
            DailyFlashExceptionReportModel ObjDailyFlashExceptionReportModel = Mapper.Map<DailyFlashExceptionReport, DailyFlashExceptionReportModel>(ObjDailyFlashExceptionReport);
            return PartialView("_LoadCompanyList", ObjDailyFlashExceptionReportModel);
        }

    }
}