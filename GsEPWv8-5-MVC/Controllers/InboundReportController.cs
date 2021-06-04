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
using System.Web.UI.WebControls;

namespace GsEPWv8_5_MVC.Controllers
{
    #region Change History
   
    #endregion Change History
    public class InboundReportController : Controller
    {
        CustMaster objCustMaster = new CustMaster();
        ICustMasterService objCustMasterService = new CustMasterService();

        // GET: InboundReport
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult InboundReport(string FullFillType, string cmp, string status, string DateFm, string DateTo,string screentitle)
        {
            string l_str_cmp_id = string.Empty;
            string l_str_Dflt_Dt_Reqd = string.Empty;
            try
            {
                InboundReport objInboundReport = new InboundReport();
                InboundReportService ServiceObject = new InboundReportService();
                objInboundReport.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
                objInboundReport.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
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
                    objInboundReport.RcvdDtFm = DateTime.Now.AddDays(Common.clsGlobal.DispDateFrom).ToString("MM/dd/yyyy");
                    objInboundReport.RcvdDtTo = DateTime.Now.ToString("MM/dd/yyyy");
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
                        objInboundReport.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                       


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
                LookUp objLookUp = new LookUp();
                LookUpService ServiceObject1 = new LookUpService();
                objLookUp.id = "5";
                objLookUp.lookuptype = "INVENTORYINQ";
                objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                objInboundReport.ListLookUpDtl = objLookUp.ListLookUpDtl;
                objInboundReport = ServiceObject.GetDftWhs(objInboundReport);
                string l_str_DftWhs = objInboundReport.LstWhsDetails[0].dft_whs.Trim();
                if (l_str_DftWhs != "" || l_str_DftWhs != null)
                {
                    objInboundReport.whs_id = l_str_DftWhs;
                }
                if (FullFillType == null)
                {
                    objCompany.cust_cmp_id = objInboundReport.cmp_id;
                }
                else
                {
                    objCompany.cust_cmp_id = cmp;
                }
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

        public ActionResult CmpIdOnChange(string p_str_cmp_id)
        {
            InboundReport objInboundReport = new InboundReport();
            InboundReportService ServiceObject = new InboundReportService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            string l_str_tmp_cmp_id = string.Empty;
            Session["g_str_cmp_id"] = p_str_cmp_id;
            l_str_tmp_cmp_id = Session["g_str_cmp_id"].ToString().Trim();
            objInboundReport.cmp_id = l_str_tmp_cmp_id;
            objInboundReport = ServiceObject.GetDftWhs(objInboundReport);
            string l_str_DftWhs = objInboundReport.LstWhsDetails[0].dft_whs.Trim();
            if (l_str_DftWhs != "" || l_str_DftWhs != null)
            {
                objInboundReport.whs_id = l_str_DftWhs;
            }
         
            objCompany.whs_id = "";
            objCompany.cust_cmp_id = p_str_cmp_id;
            objCompany = ServiceObjectCompany.GetWhsIdDetails(objCompany);
            objInboundReport.ListwhsPickDtl = objCompany.ListwhsPickDtl;
            Mapper.CreateMap<InboundReport, InboundReportModel>();
            InboundReportModel objInboundReportModel = Mapper.Map<InboundReport, InboundReportModel>(objInboundReport);
            return PartialView("_RefreshWhs", objInboundReportModel);
        }

        public ActionResult GetInboundReport(string p_str_cmp_id, string p_str_whs_id, string p_str_ib_doc_idFm, string p_str_ib_doc_idTo, 
            string p_str_RcvdDtFm, string p_str_RcvdDtTo, string p_str_cntr_id, string p_str_rcvd_status, string p_str_bill_status, string p_str_itm_num, string p_str_itm_color,
         string p_str_itm_size, string p_str_itm_name, string p_str_status)
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
                objInboundReport.cont_id= p_str_cntr_id.Trim();
                objInboundReport.rcvd_status = p_str_rcvd_status;
                objInboundReport.bill_status = p_str_bill_status;
                objInboundReport.itm_num = p_str_itm_num;
                objInboundReport.itm_color = p_str_itm_color;
                objInboundReport.itm_size = p_str_itm_size;
                objInboundReport.itm_name = p_str_itm_name;
                objInboundReport.itm_search_with = p_str_status;

                TempData["p_str_cmp_id"] = p_str_cmp_id;
                TempData["p_str_whs_id"] = p_str_whs_id;
                TempData["p_str_ib_doc_idFm"] = p_str_ib_doc_idFm;
                TempData["p_str_ib_doc_idTo"] = p_str_ib_doc_idTo;
                TempData["p_str_RcvdDtFm"] = p_str_RcvdDtFm;
                TempData["p_str_RcvdDtTo"] = p_str_RcvdDtTo;
                TempData["p_str_cntr_id"] = p_str_cntr_id;
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
        public ActionResult ShowEmailReport(string var_name, string p_str_cmp_id, string p_str_whs_id, string p_str_RcvdDtFm, string p_str_RcvdDtTo,
            string p_str_ib_doc_idFm, string p_str_ib_doc_idTo, string type, string p_str_cntr_id)
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
            string l_str_cmp_id = string.Empty;
            string l_str_whs_id = string.Empty;
            string l_str_IBDocNumFm = string.Empty;
            string l_str_IBDocNumTo = string.Empty;
            string l_InboundRcvdDtFm = string.Empty;
            string l_InboundRcvdDtTo = string.Empty;
            l_str_rpt_selection = var_name;

           try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "InbndStyle" || l_str_rpt_selection == "InbndDate" || l_str_rpt_selection == "IBRcvdByCntr")
                    {
                        InboundReport objInboundReport = new InboundReport();
                        IInboundReportService ServiceObject = new InboundReportService();
                        if (type == "PDF")
                        {
                            if (l_str_rpt_selection == "InbndStyle")
                                {
                                    strReportName = "rpt_ib_recv_rpt_by_style.rpt";
                                }
                            else if (l_str_rpt_selection == "InbndDate")
                            {
                                strReportName = "rpt_ib_recv_rpt_by_dt.rpt";
                            }

                            else if (l_str_rpt_selection == "IBRcvdByCntr")
                            {
                                strReportName = "rpt_ib_recv_rpt_by_cntr.rpt";
                            }

                     
                       
                        objInboundReport.cmp_id = p_str_cmp_id;
                        objInboundReport.whs_id = p_str_whs_id;
                        objInboundReport.ib_doc_idFm = p_str_ib_doc_idFm;

                        objInboundReport.ib_doc_idTo = p_str_ib_doc_idTo;

                        objInboundReport.RcvdDtFm = p_str_RcvdDtFm;
                        objInboundReport.RcvdDtTo = p_str_RcvdDtTo;
                            objInboundReport.cntr_id = p_str_cntr_id;

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
                        IList<InboundReport> rptSource = objInboundReport.LstinboundRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                    { 
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                    rd.Load(strRptPath);
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
                                        rd.SetParameterValue("fml_rpt_title", " SORT BY STYLE");
                                    }
                                    else if (l_str_rpt_selection == "InbndDate")
                                    {
                                        rd.SetParameterValue("fml_rpt_title", " SORT BY DATE");
                                    }
                                    else if (l_str_rpt_selection == "IBRcvdByCntr")
                                    {
                                        rd.SetParameterValue("fml_rpt_title", "BY CONTAINER - ");
                                    }

                                    objCustMaster.cust_id = p_str_cmp_id;
                                    objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
                                    if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
                                    {
                                        objCustMaster.ListGetCustLogo[0].cust_logo = "";
                                    }


                                    objInboundReport.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo;
                                    rd.SetParameterValue("fml_image_path", objInboundReport.Image_Path);

                       
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
                            }
                        }
                       
                        else  if (type == "Excel")
                        {

                            
                             if (l_str_rpt_selection == "IBRcvdByCntr")
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

        public ActionResult ShowReport(string var_name, string p_str_cmp_id, string p_str_whs_id, string p_str_RcvdDtFm, string p_str_RcvdDtTo, string p_str_ib_doc_idFm,
            string p_str_ib_doc_idTo, string type, string  p_str_cntr_id, string p_str_rcvd_status, string p_str_bill_status, string p_str_itm_num, string p_str_itm_color,
         string p_str_itm_size, string p_str_itm_name, string p_str_status)
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
                    if (l_str_rpt_selection == "InbndStyle" || l_str_rpt_selection == "InbndDate" || l_str_rpt_selection == "IBRcvdByCntr" || l_str_rpt_selection == "IBRcvdSummary")
                    {
                        InboundReport objInboundReport = new InboundReport();
                        IInboundReportService ServiceObject = new InboundReportService();
                        if (type == "PDF")
                        {
                            if (l_str_rpt_selection == "InbndStyle")
                            {
                                strReportName = "rpt_ib_recv_rpt_by_style.rpt";
                            }
                            else if (l_str_rpt_selection == "InbndDate")
                            {
                                strReportName = "rpt_ib_recv_rpt_by_dt.rpt";
                            }

                            else if (l_str_rpt_selection == "IBRcvdByCntr")
                            {
                                strReportName = "rpt_ib_recv_rpt_by_cntr.rpt";
                            }


                           
                            objInboundReport.cmp_id = p_str_cmp_id;
                            objInboundReport.whs_id = p_str_whs_id;
                            objInboundReport.ib_doc_idFm = p_str_ib_doc_idFm;
                            objInboundReport.ib_doc_idTo = p_str_ib_doc_idTo;
                            objInboundReport.RcvdDtFm = p_str_RcvdDtFm;
                            objInboundReport.RcvdDtTo = p_str_RcvdDtTo;

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
                            IList<InboundReport> rptSource = objInboundReport.LstinboundRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                { 
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                    rd.Load(strRptPath);
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
                                        rd.SetParameterValue("fml_rpt_title", "SORT BY STYLE");
                                    }
                                    else if (l_str_rpt_selection == "InbndDate")
                                    {
                                        rd.SetParameterValue("fml_rpt_title", "SORT BY DATE");
                                    }

                                    else if (l_str_rpt_selection == "IBRcvdByCntr")
                                    {
                                        rd.SetParameterValue("fml_rpt_title", "BY CONTAINER - ");
                                    }

                                    objCustMaster.cust_id = p_str_cmp_id;
                                    objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
                                    if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
                                    {
                                        objCustMaster.ListGetCustLogo[0].cust_logo = "";
                                    }
                                    objInboundReport.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo;
                                    rd.SetParameterValue("fml_image_path", objInboundReport.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Inbound Reports");
                                }
                            }
                        }

                        else
                        if (type == "Excel")
                        {

                            if (l_str_rpt_selection == "InbndStyle")
                            {
                                string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                                strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
                                string tempFileName = string.Empty;
                                string l_str_file_name = string.Empty;
                                DataTable dtBill = new DataTable();

                                dtBill = ServiceObject.GetInboundRptStyleExcel(p_str_cmp_id, p_str_cntr_id, p_str_whs_id, p_str_ib_doc_idFm, p_str_ib_doc_idTo, p_str_RcvdDtFm, p_str_RcvdDtTo,
                                     p_str_itm_num,  p_str_itm_color, p_str_itm_size,  p_str_itm_name,  p_str_status);

                                if (!Directory.Exists(strOutputpath))
                                {
                                    Directory.CreateDirectory(strOutputpath);
                                }
                                
                                l_str_file_name = "DF_" + p_str_cmp_id.ToUpper().ToString().Trim() + "_IB_RECV_BY_STYLE_" + strDateFormat + ".xlsx";

                                tempFileName = strOutputpath + l_str_file_name;

                                if (System.IO.File.Exists(tempFileName))
                                    System.IO.File.Delete(tempFileName);
                                xls_IB_Recv_By_Style_Excel mxcel1 = new xls_IB_Recv_By_Style_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IB_RECV_RPT_BY_STYLE.xlsx");

                                mxcel1.PopulateHeader(p_str_cmp_id, p_str_RcvdDtFm, p_str_RcvdDtTo);
                                mxcel1.PopulateData(dtBill, true);
                                mxcel1.SaveAs(tempFileName);
                                FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                                return File(fs, "application / xlsx", l_str_file_name);
  
                            }
                            else if (l_str_rpt_selection == "InbndDate")
                            {

                                string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                                strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
                                string tempFileName = string.Empty;
                                string l_str_file_name = string.Empty;
                                DataTable dtBill = new DataTable();

                                dtBill = ServiceObject.GetInboundRptDateExcel(p_str_cmp_id, p_str_cntr_id, p_str_whs_id, p_str_ib_doc_idFm, p_str_ib_doc_idTo, p_str_RcvdDtFm, p_str_RcvdDtTo,
                                     p_str_itm_num, p_str_itm_color, p_str_itm_size, p_str_itm_name, p_str_status);

                                if (!Directory.Exists(strOutputpath))
                                {
                                    Directory.CreateDirectory(strOutputpath);
                                }

                                l_str_file_name = "DF_" + p_str_cmp_id.ToUpper().ToString().Trim() + "_IB_RECV_BY_DATE_" + strDateFormat + ".xlsx";

                                tempFileName = strOutputpath + l_str_file_name;

                                if (System.IO.File.Exists(tempFileName))
                                    System.IO.File.Delete(tempFileName);
                                xls_IB_Recv_By_Date_Excel mxcel1 = new xls_IB_Recv_By_Date_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IB_RECV_RPT_BY_DATE.xlsx");

                                mxcel1.PopulateHeader(p_str_cmp_id, p_str_RcvdDtFm, p_str_RcvdDtTo);
                                mxcel1.PopulateData(dtBill, true);
                                mxcel1.SaveAs(tempFileName);
                                FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                                return File(fs, "application / xlsx", l_str_file_name);

                            }

                            else if (l_str_rpt_selection == "IBRcvdSummary")
                            {

                                string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                                strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
                                string tempFileName = string.Empty;
                                string l_str_file_name = string.Empty;
                                DataTable dtBill = new DataTable();
                                 objInboundReport = new InboundReport();
                                 ServiceObject = new InboundReportService();
                                objInboundReport.cmp_id = p_str_cmp_id;
                                objInboundReport.whs_id = p_str_whs_id;
                                objInboundReport.ib_doc_idFm = p_str_ib_doc_idFm;
                                objInboundReport.ib_doc_idTo = p_str_ib_doc_idTo;
                                objInboundReport.RcvdDtFm = p_str_RcvdDtFm;
                                objInboundReport.RcvdDtTo = p_str_RcvdDtTo;
                                objInboundReport.cont_id = p_str_cntr_id;
                                objInboundReport.rcvd_status = p_str_rcvd_status;
                                objInboundReport.bill_status = p_str_bill_status;
                                dtBill = ServiceObject.GetIBSummaryRpt(objInboundReport);
                              
                                if (!Directory.Exists(strOutputpath))
                                {
                                    Directory.CreateDirectory(strOutputpath);
                                }

                                l_str_file_name = "DF_" + p_str_cmp_id.ToUpper().ToString().Trim() + "_IB_RECV_RPT_BY_SUMMARY_" + strDateFormat + ".xlsx";

                                tempFileName = strOutputpath + l_str_file_name;

                                if (System.IO.File.Exists(tempFileName))
                                    System.IO.File.Delete(tempFileName);
                                xls_ib_rcvd_summary mxcel1 = new xls_ib_rcvd_summary(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IB_RECV_RPT_BY_SUMMARY.xlsx");

                                mxcel1.PopulateHeader(p_str_cmp_id, p_str_RcvdDtFm, p_str_RcvdDtTo, p_str_bill_status);
                                mxcel1.PopulateData(dtBill, true);
                                mxcel1.SaveAs(tempFileName);
                                FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                                return File(fs, "application / xlsx", l_str_file_name);

                            }

                            else if (l_str_rpt_selection == "IBRcvdByCntr")
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
                                    return new DownloadFileActionResult((GridView)Session["InbndDate"], "InbndDate-" + DateTime.Now.ToString("yyyyMMddHHssmm") + ".xls");
                                }
                            }
                        }
                    }

                    else
                    {
                        Response.Write("<H2>Report not found</H2>");
                    }
                }
            }

            catch (Exception ex)
            {
                msg = ex.Message;
                jsonErrorCode = "-2";
            }

            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ShowReportByCntr(string p_str_rpt_type, string p_str_cmp_id, string p_str_whs_id, string p_str_cntr_id, string p_str_rcvd_dt_from, 
            string p_str_rcvd_dt_to, string p_str_RcvdDtFm, string p_str_RcvdDtTo)
        {

            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string l_str_rpt_selection = string.Empty;
            string l_str_img_path = string.Empty;
            string l_str_msg = string.Empty;
            int l_int_cntr_count = 0;
            string strRptPath = string.Empty;
            try
            {
                objCustMaster.cust_id = p_str_cmp_id;
                objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
                if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
                {
                    objCustMaster.ListGetCustLogo[0].cust_logo = "";
                }
               // strReportName = "rpt_ib_recv_rpt_by_cntr.rpt";
                IBRcvdRptByCntrDtl objIBRcvdRptByCntrDtl = new IBRcvdRptByCntrDtl();
                InboundReportService ServiceObject = new InboundReportService();
            
              
                objIBRcvdRptByCntrDtl.cmp_id = p_str_cmp_id;
                objIBRcvdRptByCntrDtl.whs_id = p_str_whs_id;
                objIBRcvdRptByCntrDtl.cntr_id = p_str_cntr_id;
                objIBRcvdRptByCntrDtl.rcvd_dt_from = p_str_rcvd_dt_from;
                objIBRcvdRptByCntrDtl.rcvd_dt_to = p_str_rcvd_dt_to;
                objIBRcvdRptByCntrDtl = ServiceObject.GetIBRcvdRptByCntrDtl(objIBRcvdRptByCntrDtl);

                if (objIBRcvdRptByCntrDtl.ListIBRcvdRptByCntr.Count == 0)
                {
                    return Json("No Record Found", JsonRequestBehavior.AllowGet);
                }
                if (p_str_rpt_type == "PDF")
                {
                    if (p_str_cntr_id.Length>0)
                {
                    strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//rpt_ib_recv_rpt_by_cntr.rpt";
                }
               else
                {
                     strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//rpt_ib_recv_rpt_by_mlty_cntr.rpt";
                }
               
                var rptSource = objIBRcvdRptByCntrDtl.ListIBRcvdRptByCntr.ToList();
               if (rptSource.Count > 0)
                    {
                        using (ReportDocument rd = new ReportDocument())
                        { 
                            rd.Load(strRptPath);
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);

                            if (p_str_cntr_id.Length > 0)
                            {
                                rd.SetParameterValue("fml_rpt_title", "BY CONTAINER - " + p_str_cntr_id);
                            }
                            else
                            {
                                rd.SetParameterValue("fml_rpt_title", "BY CONTAINER ");
                            }
                            l_str_img_path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo;
                            rd.SetParameterValue("fml_image_path", l_str_img_path);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Inbound Reports");
                        }
                    }
                }
                else
                if (p_str_rpt_type == "Excel")
                    {

                    string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                    string strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
                    string tempFileName = string.Empty;
                    string l_str_file_name = string.Empty;
                    DataTable dtBill = new DataTable();

                    dtBill = ServiceObject.GetInboundRptContainerExcel(p_str_cmp_id, p_str_whs_id, p_str_cntr_id, p_str_rcvd_dt_from, p_str_rcvd_dt_to);

                    if (!Directory.Exists(strOutputpath))
                    {
                        Directory.CreateDirectory(strOutputpath);
                    }

                    l_str_file_name = "DF_" + p_str_cmp_id.ToUpper().ToString().Trim() + "_IB_RECV_BY_CNTR_" + strDateFormat + ".xlsx";

                    tempFileName = strOutputpath + l_str_file_name;

                    if (System.IO.File.Exists(tempFileName))
                        System.IO.File.Delete(tempFileName);
                    xls_IB_Recv_By_Container_Excel mxcel1 = new xls_IB_Recv_By_Container_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IB_RECV_RPT_BY_CNTR.xlsx");

                    mxcel1.PopulateHeader(p_str_cmp_id, p_str_RcvdDtFm, p_str_RcvdDtTo);
                    mxcel1.PopulateData(dtBill, true);
                    mxcel1.SaveAs(tempFileName);
                    FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                    return File(fs, "application / xlsx", l_str_file_name);
                   

                }
                   
               
            }

            catch (Exception ex)
            {
                l_str_msg = ex.Message;
                jsonErrorCode = "-2";
            }

            return Json(new { result = jsonErrorCode, err = l_str_msg }, JsonRequestBehavior.AllowGet);

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
 