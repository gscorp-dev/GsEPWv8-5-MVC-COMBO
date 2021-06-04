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
using static GsEPWv8_5_MVC.Core.Entity.ETAInquiry;

namespace GsEPWv8_5_MVC.Controllers
{
    public class ETAInquiryController : Controller
    {
        // GET: ETAInquiry

        public string ScreenID = "ETAInquiry";
        public string Folderpath = string.Empty;
        public string l_str_rptdtl = string.Empty;
        public string strDateFormat = string.Empty;
        public string strFileName = string.Empty;
        public string reportFileName = string.Empty;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ETAInquiry(string FullFillType, string cmp, string status, string DateFm, string DateTo, string screentitle)
        {
            string l_str_cmp_id = string.Empty;
            string l_str_fm_dt = string.Empty;
            string l_str_Dflt_Dt_Reqd = string.Empty;
            try
            {
                ETAInquiry objETAInquiry = new ETAInquiry();
                ETAInquiryService ServiceObject = new ETAInquiryService();
                objETAInquiry.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                objETAInquiry.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                Session["g_str_Search_flag"] = "False";
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                if (objETAInquiry.cmp_id == null || objETAInquiry.cmp_id == string.Empty)
                {
                    objETAInquiry.cmp_id = Session["g_str_cmp_id"].ToString().Trim();

                }
                else
                {
                    objCompany.cmp_id = Session["g_str_cmp_id"].ToString().Trim();

                }
                objETAInquiry.screentitle = screentitle;
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
                    objETAInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    objETAInquiry.ETA_dt_Fm = DateTime.Now.AddDays(Common.clsGlobal.DispDateFrom).ToString("MM/dd/yyyy") ;
                    objETAInquiry.ETA_dt_To = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else if (FullFillType != null)
                {

                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objETAInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    objETAInquiry.cmp_id = cmp;
                    objETAInquiry.ETA_dt_Fm = DateTime.Now.AddDays(Common.clsGlobal.DispDateFrom).ToString("MM/dd/yyyy") ;
                    objETAInquiry.ETA_dt_To = DateTime.Now.ToString("MM/dd/yyyy");

                    if (l_str_Dflt_Dt_Reqd == "Y")
                    {
                        objETAInquiry = ServiceObject.GetInboundETAInquiryDetails(objETAInquiry);
                    }
                    objCompany.cmp_id = cmp;
                    objCompany = ServiceObjectCompany.GetFullFillCompanyDetails(objCompany);
                }
                Mapper.CreateMap<ETAInquiry, ETAInquiryModel>();
                ETAInquiryModel ETAInquirysModel = Mapper.Map<ETAInquiry, ETAInquiryModel>(objETAInquiry);
                return View(ETAInquirysModel);
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
            ETAInquiry objETAInquiry = new ETAInquiry();
            ETAInquiryService ServiceObject = new ETAInquiryService();
            string l_str_tmp_cmp_id = string.Empty;
            Session["g_str_cmp_id"] = p_str_cmp_id;
            l_str_tmp_cmp_id = Session["g_str_cmp_id"].ToString().Trim();
            return Json(l_str_tmp_cmp_id, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSearchETAInquiry(string p_str_cmp_id, string p_str_ETA_dt_Fr, string p_str_ETA_dt_To)
        {
            try
            {
                string l_str_is_another_usr = string.Empty;
                ETAInquiry objETAInquiry = new ETAInquiry();
                ETAInquiryService ServiceObject = new ETAInquiryService();
                objETAInquiry.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                objETAInquiry.user_id = Session["UserID"].ToString().Trim();
                Session["g_str_Search_flag"] = "True";
                objETAInquiry.cmp_id = p_str_cmp_id.Trim();
                objETAInquiry.ETA_dt_Fm = p_str_ETA_dt_Fr;
                objETAInquiry.ETA_dt_To = p_str_ETA_dt_To;
                objETAInquiry = ServiceObject.GetInboundETAInquiryDetails(objETAInquiry);
                Mapper.CreateMap<ETAInquiry, ETAInquiryModel>();
                ETAInquiryModel ETAInquirysModel = Mapper.Map<ETAInquiry, ETAInquiryModel>(objETAInquiry);
                return PartialView("_ETAInquiry", ETAInquirysModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ShowSummaryReport(string p_str_radio, string p_str_cmp_id, string p_str_ETA_dt_Fr, string p_str_ETA_dt_To, string type)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            string l_str_tmp_name = string.Empty;
            l_str_rpt_selection = p_str_radio;
            string l_str_status = string.Empty;
            ETAInquiry objETAInq = new ETAInquiry();
            ETAInquiryService ServiceObject = new ETAInquiryService();
            CustMaster objCustMaster = new CustMaster();
            CustMasterService objCustMasterService = new CustMasterService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cmp_id = p_str_cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            objETAInq.LstCmpName = objCompany.LstCmpName;
            l_str_tmp_name = objETAInq.LstCmpName[0].cmp_name.ToString().Trim();
            int l_int_totctn = 0;
            int l_int_qty =0;
            decimal l_dec_wgt =0;
            decimal l_dec_cube = 0;

            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "EtaSummayRpt")
                    {
                        strReportName = "rpt_ETAinbound_grid_summary.rpt";
                        ETAInquiry objETAInquiry = new ETAInquiry();
                        ETAInquiryService ServiceObjects = new ETAInquiryService();
                     
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//InboundETAReports//" + strReportName;
                        objETAInquiry.cmp_id = p_str_cmp_id;
                        objETAInquiry.ETA_dt_Fm = p_str_ETA_dt_Fr;
                        objETAInquiry.ETA_dt_To = p_str_ETA_dt_To;
                        objETAInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                        objETAInquiry = ServiceObjects.GetInboundETAInquiryDetailsRpt(objETAInquiry);
                        for(int j = 0; j < objETAInquiry.LstIBETAInqdetail.Count(); j++)
                        {
                            var WGT_amt = objETAInquiry.LstIBETAInqdetail[j].TOT_WGT; 
                            var CUBE_amt = objETAInquiry.LstIBETAInqdetail[j].TOT_CUBE; 
                            var CTN_amt = objETAInquiry.LstIBETAInqdetail[j].TOT_CTN;
                            var QTY_amt = objETAInquiry.LstIBETAInqdetail[j].TOT_QTY;
                            l_int_totctn = l_int_totctn + CTN_amt;
                            l_int_qty = l_int_qty + QTY_amt;
                            decimal TOTWGTAMT = WGT_amt;
                            decimal TOTCUBEAMT = CUBE_amt;
                            if (TOTWGTAMT > 0)
                            {
                                l_dec_wgt = l_dec_wgt + TOTWGTAMT;
                            }
                            if (TOTCUBEAMT > 0)
                            {
                                l_dec_cube = l_dec_cube + TOTCUBEAMT;
                            }
                        }
                        var rptSource = objETAInquiry.LstIBETAInqdetail.ToList();
                       
                        if (type == "PDF")
                        {
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    rd.SetDataSource(rptSource);
                                    rd.SetParameterValue("TotCtn", l_int_totctn);
                                    rd.SetParameterValue("TotQty", l_int_qty);
                                    rd.SetParameterValue("TotCube", l_dec_cube.ToString("0.000"));
                                    rd.SetParameterValue("TotWgt", l_dec_wgt.ToString("0.000"));
                                    rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                    rd.SetParameterValue("fml_image_path", objETAInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                }
                            }
                        }
                        
                        else if (type == "Excel")
                        {
                            List<ETAInqRptSummaryExcel> li = new List<ETAInqRptSummaryExcel>();
                            for (int i = 0; i < objETAInquiry.LstIBETAInqdetail.Count; i++)
                            {

                                ETAInqRptSummaryExcel objETAInquiryExcel = new ETAInqRptSummaryExcel();
                                objETAInquiryExcel.CMP_ID = objETAInquiry.LstIBETAInqdetail[i].CMP_ID;
                                objETAInquiryExcel.IB_DOC_ID = objETAInquiry.LstIBETAInqdetail[i].IB_DOC_ID;
                                objETAInquiryExcel.IB_DOC_DT = objETAInquiry.LstIBETAInqdetail[i].IB_DOC_DT;
                                objETAInquiryExcel.STATUS = objETAInquiry.LstIBETAInqdetail[i].STATUS;
                                objETAInquiryExcel.ETA_DT = objETAInquiry.LstIBETAInqdetail[i].ETA_DT;
                                objETAInquiryExcel.CNTR_ID = objETAInquiry.LstIBETAInqdetail[i].CNTR_ID;
                                objETAInquiryExcel.REQ_NUM = objETAInquiry.LstIBETAInqdetail[i].REQ_NUM;
                                objETAInquiryExcel.VEND_NAME = objETAInquiry.LstIBETAInqdetail[i].VEND_NAME;
                                objETAInquiryExcel.NOTE = objETAInquiry.LstIBETAInqdetail[i].NOTE;
                                objETAInquiryExcel.TOT_CTN = objETAInquiry.LstIBETAInqdetail[i].TOT_CTN;
                                objETAInquiryExcel.TOT_QTY = objETAInquiry.LstIBETAInqdetail[i].TOT_QTY;
                                objETAInquiryExcel.TOT_WGT = objETAInquiry.LstIBETAInqdetail[i].TOT_WGT;
                                objETAInquiryExcel.TOT_CUBE = objETAInquiry.LstIBETAInqdetail[i].TOT_CUBE;
                                li.Add(objETAInquiryExcel);
                            }
                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["ETAInq_Rpt"] = gv;
                            return new DownloadFileActionResult((GridView)Session["ETAInq_Rpt"], "ETAInquiry Report" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
                        }
                    }
                    else
                    {
                        strReportName = "rpt_ib_doc_entry_by_multiple_docs_ack.rpt";
                        ETAInquiry objETAInquiry = new ETAInquiry();
                        ETAInquiryService ServiceObjects = new ETAInquiryService();

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//InboundETAReports//" + strReportName;
                        objETAInquiry.cmp_id = p_str_cmp_id;
                        objETAInquiry.ETA_dt_Fm = p_str_ETA_dt_Fr;
                        objETAInquiry.ETA_dt_To = p_str_ETA_dt_To;
                        objETAInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                        objETAInquiry = ServiceObjects.GetInboundETAInquiryRpt(objETAInquiry);
                        for (int j = 0; j < objETAInquiry.LstIBETAInqdetail.Count(); j++)
                        {
                            var WGT_amt = objETAInquiry.LstIBETAInqdetail[j].TOT_WGT;
                            var CUBE_amt = objETAInquiry.LstIBETAInqdetail[j].TOT_CUBE;
                            var CTN_amt = objETAInquiry.LstIBETAInqdetail[j].TOT_CTN;
                            var QTY_amt = objETAInquiry.LstIBETAInqdetail[j].TOT_QTY;
                            l_int_totctn = l_int_totctn + CTN_amt;
                            l_int_qty = l_int_qty + QTY_amt;
                            decimal TOTWGTAMT = WGT_amt;
                            decimal TOTCUBEAMT = CUBE_amt;
                            if (TOTWGTAMT > 0)
                            {
                                l_dec_wgt = l_dec_wgt + TOTWGTAMT;
                            }
                            if (TOTCUBEAMT > 0)
                            {
                                l_dec_cube = l_dec_cube + TOTCUBEAMT;
                            }
                        }
                        var rptSource = objETAInquiry.LstIBETAInqdetail.ToList();
                        
                        if (type == "PDF")
                        {
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objETAInquiry.LstIBETAInqdetail.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    rd.SetParameterValue("TotCtn", l_int_totctn);
                                    rd.SetParameterValue("TotQty", l_int_qty);
                                    rd.SetParameterValue("TotCube", l_dec_cube.ToString("0.000"));
                                    rd.SetParameterValue("TotWgt", l_dec_wgt.ToString("0.000"));
                                    rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                    rd.SetParameterValue("fml_image_path", objETAInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                }
                            }
                        }
                       
                        else if (type == "Excel")
                        {
                            List<ETAInqRptDetailExcel> li = new List<ETAInqRptDetailExcel>();
                            for (int i = 0; i < objETAInquiry.LstIBETAInqdetail.Count; i++)
                            {

                                ETAInqRptDetailExcel objETAInquiryExcel = new ETAInqRptDetailExcel();
                                objETAInquiryExcel.CMP_ID = objETAInquiry.LstIBETAInqdetail[i].CMP_ID;
                                objETAInquiryExcel.IB_DOC_ID = objETAInquiry.LstIBETAInqdetail[i].IB_DOC_ID;
                                objETAInquiryExcel.IB_DOC_DT = objETAInquiry.LstIBETAInqdetail[i].IB_DOC_DT;
                                objETAInquiryExcel.STATUS = objETAInquiry.LstIBETAInqdetail[i].STATUS;
                                objETAInquiryExcel.ETA_DT = objETAInquiry.LstIBETAInqdetail[i].ETA_DT;
                                objETAInquiryExcel.CNTR_ID = objETAInquiry.LstIBETAInqdetail[i].CNTR_ID;
                                objETAInquiryExcel.REQ_NUM = objETAInquiry.LstIBETAInqdetail[i].REQ_NUM;
                                objETAInquiryExcel.VEND_NAME = objETAInquiry.LstIBETAInqdetail[i].VEND_NAME;
                                objETAInquiryExcel.NOTE = objETAInquiry.LstIBETAInqdetail[i].NOTE;
                                objETAInquiryExcel.TOT_CTN = objETAInquiry.LstIBETAInqdetail[i].TOT_CTN;
                                objETAInquiryExcel.TOT_QTY = objETAInquiry.LstIBETAInqdetail[i].TOT_QTY;
                                objETAInquiryExcel.TOT_WGT = objETAInquiry.LstIBETAInqdetail[i].TOT_WGT;
                                objETAInquiryExcel.TOT_CUBE = objETAInquiry.LstIBETAInqdetail[i].TOT_CUBE;
                                objETAInquiryExcel.DTL_LINE = objETAInquiry.LstIBETAInqdetail[i].DTL_LINE;
                                objETAInquiryExcel.CTN_LINE = objETAInquiry.LstIBETAInqdetail[i].CTN_LINE;
                                objETAInquiryExcel.ITM_NUM = objETAInquiry.LstIBETAInqdetail[i].ITM_NUM;
                                objETAInquiryExcel.ITM_COLOR = objETAInquiry.LstIBETAInqdetail[i].ITM_COLOR;
                                objETAInquiryExcel.ITM_SIZE = objETAInquiry.LstIBETAInqdetail[i].ITM_SIZE;
                                objETAInquiryExcel.ITM_NAME = objETAInquiry.LstIBETAInqdetail[i].ITM_NAME;
                                objETAInquiryExcel.LENGTH = objETAInquiry.LstIBETAInqdetail[i].LENGTH;
                                objETAInquiryExcel.WIDTH = objETAInquiry.LstIBETAInqdetail[i].WIDTH;
                                objETAInquiryExcel.DEPTH = objETAInquiry.LstIBETAInqdetail[i].DEPTH;
                                objETAInquiryExcel.PO_NUM = objETAInquiry.LstIBETAInqdetail[i].PO_NUM;


                                li.Add(objETAInquiryExcel);
                            }
                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["ETAInq_DetailRpt"] = gv;
                            return new DownloadFileActionResult((GridView)Session["ETAInq_DetailRpt"], "ETAInquiry Detail Report" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
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

        public ActionResult EmailShowReport(string p_str_radio, string p_str_cmp_id, string p_str_ETA_dt_Fr, string p_str_ETA_dt_To, string type)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("mm/dd/yyyy");
            string strToDate = DateTime.Now.ToString("mm/dd/yyyy");
            string l_str_rpt_selection = string.Empty;
            string l_str_tmp_name = string.Empty;
            l_str_rpt_selection = p_str_radio;
            int l_int_totctn = 0;
            int l_int_qty = 0;
            decimal l_dec_wgt = 0;
            decimal l_dec_cube = 0;
            CustMaster objCustMaster = new CustMaster();
            CustMasterService objCustMasterService = new CustMasterService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            ETAInquiry objETAInquiry = new ETAInquiry();
            ETAInquiryService ServiceObjects = new ETAInquiryService();
            Email objEmail = new Email();
            EmailService objEmailService = new EmailService();
            objCompany.cmp_id = p_str_cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            objETAInquiry.LstCmpName = objCompany.LstCmpName;
            l_str_tmp_name = objETAInquiry.LstCmpName[0].cmp_name.ToString().Trim();
            objEmail.CmpId = p_str_cmp_id;
            objEmail.screenId = ScreenID;
            objEmail.username = objCompany.user_id;
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "EtaSummayRpt")
                    {
                        strReportName = "rpt_ETAinbound_grid_summary.rpt";

                      
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//InboundETAReports//" + strReportName;
                        Folderpath = System.Configuration.ConfigurationManager.AppSettings["tempFilepath"].ToString().Trim();
                        objETAInquiry.cmp_id = p_str_cmp_id;
                        objEmail.CmpId = p_str_cmp_id;
                        objEmail.screenId = ScreenID;
                        objEmail.Reportselection = l_str_rpt_selection;
                        objEmail = objEmailService.GetSendMailDetails(objEmail);
                        if (objEmail.ListEamilDetail.Count != 0)
                        {
                            objEmail.EmailMessageContent = (objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == null || objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailMessageContent.Trim();
                        }
                        objETAInquiry.cmp_id = p_str_cmp_id;
                        objETAInquiry.ETA_dt_Fm = p_str_ETA_dt_Fr;
                        objETAInquiry.ETA_dt_To = p_str_ETA_dt_To;
                        objETAInquiry = ServiceObjects.GetInboundETAInquiryDetailsRpt(objETAInquiry);
                        for (int j = 0; j < objETAInquiry.LstIBETAInqdetail.Count(); j++)
                        {
                            var WGT_amt = objETAInquiry.LstIBETAInqdetail[j].TOT_WGT;
                            var CUBE_amt = objETAInquiry.LstIBETAInqdetail[j].TOT_CUBE;
                            var CTN_amt = objETAInquiry.LstIBETAInqdetail[j].TOT_CTN;
                            var QTY_amt = objETAInquiry.LstIBETAInqdetail[j].TOT_QTY;
                            l_int_totctn = l_int_totctn + CTN_amt;
                            l_int_qty = l_int_qty + QTY_amt;
                            decimal TOTWGTAMT = WGT_amt;
                            decimal TOTCUBEAMT = CUBE_amt;
                            if (TOTWGTAMT > 0)
                            {
                                l_dec_wgt = l_dec_wgt + TOTWGTAMT;
                            }
                            if (TOTCUBEAMT > 0)
                            {
                                l_dec_cube = l_dec_cube + TOTCUBEAMT;
                            }
                        }
                        if (objETAInquiry.LstIBETAInqdetail.Count > 0)
                        {
                            objETAInquiry.ETA_DT = objETAInquiry.LstIBETAInqdetail[0].ETA_DT;
                            l_str_rptdtl = objETAInquiry.cmp_id + "_" + "ETAInquiry  Summary Report";
                            objEmail.EmailSubject = objETAInquiry.cmp_id + "-" + "ETAInquiry Summary Report" + "|" + " " + " " + objETAInquiry.ETA_DT;
                            objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objETAInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "ETA Date: " + " " + " " + objETAInquiry.ETA_DT;
                        }
                        var rptSource = objETAInquiry.LstIBETAInqdetail.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objETAInquiry.LstIBETAInqdetail.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                rd.SetParameterValue("TotCtn", l_int_totctn);
                                rd.SetParameterValue("TotQty", l_int_qty);
                                rd.SetParameterValue("TotCube", l_dec_cube.ToString("0.000"));
                                rd.SetParameterValue("TotWgt", l_dec_wgt.ToString("0.000"));
                                rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                rd.SetParameterValue("fml_image_path", objETAInquiry.Image_Path);
                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            }
                        }
                        reportFileName = l_str_rptdtl + "_" + strDateFormat + ".pdf";
                        Session["RptFileName"] = strFileName;
                    }
                    else
                    {
                        strReportName = "rpt_ib_doc_entry_by_multiple_docs_ack.rpt";                        
  
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//InboundETAReports//" + strReportName;
                        objETAInquiry.cmp_id = p_str_cmp_id;
                        objETAInquiry.ETA_dt_Fm = p_str_ETA_dt_Fr;
                        objETAInquiry.ETA_dt_To = p_str_ETA_dt_To;
                        objETAInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                        objETAInquiry = ServiceObjects.GetInboundETAInquiryRpt(objETAInquiry);
                        for (int j = 0; j < objETAInquiry.LstIBETAInqdetail.Count(); j++)
                        {
                            var WGT_amt = objETAInquiry.LstIBETAInqdetail[j].TOT_WGT;
                            var CUBE_amt = objETAInquiry.LstIBETAInqdetail[j].TOT_CUBE;
                            var CTN_amt = objETAInquiry.LstIBETAInqdetail[j].TOT_CTN;
                            var QTY_amt = objETAInquiry.LstIBETAInqdetail[j].TOT_QTY;
                            l_int_totctn = l_int_totctn + CTN_amt;
                            l_int_qty = l_int_qty + QTY_amt;
                            decimal TOTWGTAMT = WGT_amt;
                            decimal TOTCUBEAMT = CUBE_amt;
                            if (TOTWGTAMT > 0)
                            {
                                l_dec_wgt = l_dec_wgt + TOTWGTAMT;
                            }
                            if (TOTCUBEAMT > 0)
                            {
                                l_dec_cube = l_dec_cube + TOTCUBEAMT;
                            }
                        }
                        var rptSource = objETAInquiry.LstIBETAInqdetail.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objETAInquiry.LstIBETAInqdetail.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);

                                rd.SetParameterValue("TotCtn", l_int_totctn);
                                rd.SetParameterValue("TotQty", l_int_qty);
                                rd.SetParameterValue("TotCube", l_dec_cube.ToString("0.000"));
                                rd.SetParameterValue("TotWgt", l_dec_wgt.ToString("0.000"));
                                rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                rd.SetParameterValue("fml_image_path", objETAInquiry.Image_Path);
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