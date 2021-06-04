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
using System.Web.UI.WebControls;

namespace GsEPWv8_5_MVC.Controllers
{
    #region Change History
    // CR_3PL_MVC_OB_2018_0227_001 - Modified by Soniya for set default from date before one year  in filter section
    #endregion Change History
    public class OutboundShipInqController : Controller
    {
        // GET: OutboundShipInq
        public string Folderpath = string.Empty;
        public string EmailSub = string.Empty;
        public string EmailMsg = string.Empty;
        public string l_str_rptdtl = string.Empty;
        public string l_str_tmp_name = string.Empty;
        public string ScreenID = "Outbound Inquiry";
        CustMaster objCustMaster = new CustMaster();
        ICustMasterService objCustMasterService = new CustMasterService();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OutboundShipInq(string FullFillType, string cmp, string status, string DateFm, string DateTo, string screentitle)
        {
            string l_str_cmp_id = string.Empty;
            string l_str_fm_dt = string.Empty;
            string l_str_Dflt_Dt_Reqd = string.Empty;
            try
            {
                OutboundShipInq objOutboundShipInq = new OutboundShipInq();
                OutboundShipInqService ServiceObject = new OutboundShipInqService();
                objOutboundShipInq.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                objOutboundShipInq.is_company_user = Session["IsCompanyUser"].ToString().Trim(); //CR_3PL_MVC _2018_0316_005
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                if (objOutboundShipInq.cmp_id == null || objOutboundShipInq.cmp_id == string.Empty)
                {
                    objOutboundShipInq.cmp_id = Session["g_str_cmp_id"].ToString().Trim();

                }
                else
                {
                    objCompany.cmp_id = Session["g_str_cmp_id"].ToString().Trim();

                }
                objOutboundShipInq.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                //  CR_3PL_MVC_COMMON_2018_0324_001
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
                    objOutboundShipInq.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    DateTime date = DateTime.Now.AddMonths(-12);
                    l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");      // CR_3PL_MVC_OB_2018_0227_001 - Modified by Soniya
                    objOutboundShipInq.Ship_dt_Fm = DateTime.Now.AddDays(Common.clsGlobal.DispDateFrom).ToString("MM/dd/yyyy");
                    objOutboundShipInq.Ship_dt_To = DateTime.Now.ToString("MM/dd/yyyy");

                }
                else if (FullFillType != null)
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objOutboundShipInq.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    objOutboundShipInq.cmp_id = cmp;
                    objOutboundShipInq.ship_doc_id_Fm = "";
                    objOutboundShipInq.ship_doc_id_To = "";
                    objOutboundShipInq.Ship_dt_Fm = DateTime.Now.AddDays(Common.clsGlobal.DispDateFrom).ToString("MM/dd/yyyy") ;
                    objOutboundShipInq.Ship_dt_To = DateTime.Now.ToString("MM/dd/yyyy");
                    objOutboundShipInq.cust_id = "";
                    objOutboundShipInq.aloc_doc_id = "";
                    objOutboundShipInq.ship_to = "";
                    objOutboundShipInq.ship_via_name = "";
                    objOutboundShipInq.whs_id = "";
                    status.Trim();
                    if (status == "POST")
                    {
                        objOutboundShipInq.status = "";
                    }
                    else
                    {
                        objOutboundShipInq.status = "";
                    }
                    if (l_str_Dflt_Dt_Reqd == "Y")
                    {
                        objOutboundShipInq = ServiceObject.GetOutboundShipInq(objOutboundShipInq);
                    }
                    objCompany.cmp_id = cmp;
                    objCompany = ServiceObjectCompany.GetFullFillCompanyDetails(objCompany);
                    //objOutboundShipInq.ListCompanyPickDtl = objCompany.ListFullFillCompanyPickDtl;
                    status.Trim();
                    if (status == "POST")
                    {
                        objOutboundShipInq.status = "POST";
                    }
                    else
                    {
                        objOutboundShipInq.status = "";
                    }
                }
                LookUp objLookUp = new LookUp();
                LookUpService ServiceObject1 = new LookUpService();
                objLookUp.id = "3";
                objLookUp.lookuptype = "OUTBOUNDSHIPINQUIRY";
                objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                objOutboundShipInq.ListLookUpDtl = objLookUp.ListLookUpDtl;
                Mapper.CreateMap<OutboundShipInq, OutboundShipInqModel>();
                OutboundShipInqModel objOutboundShipInqModel = Mapper.Map<OutboundShipInq, OutboundShipInqModel>(objOutboundShipInq);
                return View(objOutboundShipInqModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public ActionResult OutboundShipInqDetail(string p_str_cmp_id, string p_str_ship_docId_Fm, string p_str_ship_docId_To, string p_str_ship_dt_frm, string p_str_ship_dt_to, string p_str_CustId, string p_str_AlocId, string p_str_Shipto, string p_str_ship_via_name, string p_str_Whsid, string p_str_status)
        {
            try
            {

                OutboundShipInq objOutboundShipInq = new OutboundShipInq();
                OutboundShipInqService ServiceObject = new OutboundShipInqService();
                //objOutboundShipInq.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
                //p_str_cmp_id = objOutboundShipInq.cmp_id;  
                objOutboundShipInq.is_company_user = Session["IsCompanyUser"].ToString().Trim(); //CR_3PL_MVC _2018_0316_005
                Session["g_str_Search_flag"] = "True";//CR_3PL_MVC _2018_0413_001 Added By Nithya
                objOutboundShipInq.cmp_id = p_str_cmp_id.Trim();
                objOutboundShipInq.ship_doc_id_Fm = p_str_ship_docId_Fm.Trim();
                objOutboundShipInq.ship_doc_id_To = p_str_ship_docId_To.Trim();
                objOutboundShipInq.Ship_dt_Fm = p_str_ship_dt_frm.Trim();
                objOutboundShipInq.Ship_dt_To = p_str_ship_dt_to.Trim();
                objOutboundShipInq.cust_id = p_str_CustId.Trim();
                objOutboundShipInq.aloc_doc_id = p_str_AlocId.Trim();
                objOutboundShipInq.ship_to = p_str_Shipto.Trim();
                objOutboundShipInq.ship_via_name = p_str_ship_via_name.Trim();
                objOutboundShipInq.whs_id = p_str_Whsid.Trim();
                objOutboundShipInq.status = p_str_status.Trim();
                objOutboundShipInq = ServiceObject.GetOutboundShipInq(objOutboundShipInq);
                Mapper.CreateMap<OutboundShipInq, OutboundShipInqModel>();
                OutboundShipInqModel objOutboundShipInqModel = Mapper.Map<OutboundShipInq, OutboundShipInqModel>(objOutboundShipInq);
                return PartialView("_OutboundShipInq", objOutboundShipInqModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public JsonResult EShipGetCustDetails(string term, string cmpid)
        {
            IOutboundShipInqService ServiceObject = new OutboundShipInqService();
            OutboundShipInq objOutboundShipInq = new OutboundShipInq();
            objOutboundShipInq.cmp_id = cmpid.Trim();
            var List = ServiceObject.OutboundShipInqGetCustDetails(term, cmpid.Trim()).OutboundShipInqGetCustDetails.Select(x => new { label = x.custdtl, value = x.custdtl, cust_id = x.cust_id }).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EShipGetShipToDetailss(string term, string cmpid)
        {
            IOutboundShipInqService ServiceObject = new OutboundShipInqService();
            var List = ServiceObject.OutboundShipInqGetShipToDetails(term, cmpid.Trim()).LstOutboundShipInqGetShipToDetails.Select(x => new { label = x.shiptodtl, value = x.shiptodtl, shipto_id = x.shipto_id }).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EShipGetShipFromDetailss(string term, string cmpid)
        {
            IOutboundShipInqService ServiceObject = new OutboundShipInqService();
            var List = ServiceObject.OutboundShipInqGetShipFromDetails(term, cmpid.Trim()).LstOutboundShipInqGetShipFromDetails.Select(x => new { label = x.whsdtl, value = x.whsdtl, whs_id = x.whs_id }).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetShipReqRptDtls(string SelectedID, string p_str_cmp_id, string p_str_radio, string p_str_ship_docId_Fm, string p_str_ship_docId_To, string p_str_ship_dt_frm, string p_str_ship_dt_to, string p_str_CustId, string p_str_AlocId, string p_str_Shipto, string p_str_ship_via_name, string p_str_Whsid, string p_str_status)
        {
            try
            {
                OutboundShipInq objOutboundShipInq = new OutboundShipInq();
                OutboundShipInqService ServiceObject = new OutboundShipInqService();
                objOutboundShipInq.cmp_id = p_str_cmp_id;
                objOutboundShipInq.ship_doc_id = SelectedID;
                TempData["shipdocId"] = SelectedID;
                TempData["aloc_doc_id"] = p_str_AlocId;
                TempData["ReportSelection"] = p_str_radio;
                TempData["shipdocIdFm"] = p_str_ship_docId_Fm;
                TempData["shipdocIdTo"] = p_str_ship_docId_To;
                TempData["ship_DtFm"] = p_str_ship_dt_frm;
                TempData["ship_DtTo"] = p_str_ship_dt_to;
                TempData["Custid"] = p_str_CustId;
                TempData["status"] = p_str_status;
                TempData["Shipto"] = p_str_Shipto;
                TempData["Shipvianame"] = p_str_ship_via_name;
                TempData["Whsid"] = p_str_Whsid;
                TempData["cmp_id"] = p_str_cmp_id;

                return Json(SelectedID, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ActionResult GetShipInqRpt(string SelectedID, string p_str_cmpid, string p_str_radio)
        {
            try
            {
                OutboundShipInq objOutboundShipInq = new OutboundShipInq();
                OutboundShipInqService ServiceObject = new OutboundShipInqService();
                objOutboundShipInq.cmp_id = p_str_cmpid;
                objOutboundShipInq.ship_doc_id = SelectedID;
                //  objOrderLifeCycle = ServiceObject.GetEShipGetTrackId(objEShip);
                TempData["shipdocId"] = SelectedID;
                TempData["ReportSelection"] = p_str_radio;
                TempData["cmp_id"] = p_str_cmpid;
                return Json(SelectedID, JsonRequestBehavior.AllowGet);
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
        public ActionResult EmailShowReport(string SelectedID, string p_str_cmp_id, string p_str_radio, string p_str_ship_docId_Fm, string p_str_ship_docId_To, string p_str_ship_dt_frm, string p_str_ship_dt_to, string p_str_CustId, string p_str_AlocId, string p_str_Shipto, string p_str_ship_via_name, string p_str_status, string p_str_Whsid, string type)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string batchId = string.Empty;
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            string l_str_rpt_so_num = string.Empty;
            string reportFileName = string.Empty;
            l_str_rpt_selection = p_str_radio;
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            Email objEmail = new Email();
            EmailService objEmailService = new EmailService();
            objCompany.cmp_id = p_str_cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            l_str_tmp_name = objCompany.LstCmpName[0].cmp_name.ToString().Trim();
            Folderpath = System.Configuration.ConfigurationManager.AppSettings["tempFilepath"].ToString().Trim();
            objCustMaster.cust_id = p_str_cmp_id;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "GridSummary")
                    {

                        strReportName = "rpt_Shipping_Summary.rpt";
                        OutboundShipInq objOutboundShipInq = new OutboundShipInq();
                        OutboundShipInqService ServiceObject = new OutboundShipInqService();

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundShipInq.cmp_id = p_str_cmp_id;
                        objOutboundShipInq.aloc_doc_id = p_str_AlocId;
                        objOutboundShipInq.ship_doc_id_Fm = p_str_ship_docId_Fm;
                        objOutboundShipInq.ship_doc_id_To = p_str_ship_docId_To;
                        objOutboundShipInq.Ship_dt_Fm = p_str_ship_dt_frm;
                        objOutboundShipInq.Ship_dt_To = p_str_ship_dt_to;
                        objOutboundShipInq.cust_id = p_str_CustId;
                        objOutboundShipInq.status = p_str_status;
                        objOutboundShipInq.shipto_id = p_str_Shipto;
                        objOutboundShipInq.ship_via_name = p_str_ship_via_name;
                        objOutboundShipInq.whs_id = p_str_Whsid;


                        objOutboundShipInq = ServiceObject.OutboundShipInqSummaryRpt(objOutboundShipInq);
                        EmailSub = "Shipping post Inquiry Report for" + " " + " " + objOutboundShipInq.cmp_id;
                        EmailMsg = "Shipping post Inquiry Report hasbeen Attached for the Process";
                        if (type == "PDF")
                        {
                            string strDateFormat = string.Empty;
                            string strFileName = string.Empty;
                            var rptSource = objOutboundShipInq.LstOutboundShipInqdetail.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objOutboundShipInq.LstOutboundShipInqdetail.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objOutboundShipInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objOutboundShipInq.Image_Path);
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//Outbound SHIPPING_POST_Report" + strDateFormat + ".pdf";
                                    // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                }
                            }
                            // rd.ExportToDisk(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                            reportFileName = "Outbound SHIPPING_POST_Report" + DateTime.Now.ToFileTime() + ".pdf";
                            Session["RptFileName"] = strFileName;

                        }
                        
                    }
                    else if (l_str_rpt_selection == "Outboundpackslip")
                    {

                        strReportName = "rpt_iv_packslip_tpw.rpt";
                        OutboundShipInq objOutboundShipInq = new OutboundShipInq();
                        OutboundShipInqService ServiceObject = new OutboundShipInqService();

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundShipInq.cmp_id = p_str_cmp_id;
                        objOutboundShipInq.ship_doc_id = SelectedID;
                        objOutboundShipInq = ServiceObject.OutboundShipInqpackSlipRpt(objOutboundShipInq);
                        objOutboundShipInq = ServiceObject.GetTotCubesRpt(objOutboundShipInq);//CR20180601-001 Added bY Nithya
                        if (objOutboundShipInq.LstPalletCount.Count > 0)
                        {
                            objOutboundShipInq.TotCube = objOutboundShipInq.LstPalletCount[0].TotCube;
                        }
                        else
                        {
                            objOutboundShipInq.TotCube = 0;
                        }
                        if (type == "PDF")
                        {
                            string strDateFormat = string.Empty;
                            string strFileName = string.Empty;
                            var rptSource = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objOutboundShipInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objOutboundShipInq.Image_Path);
                                    rd.SetParameterValue("SumOfCubes", objOutboundShipInq.TotCube);
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm"); //string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));

                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//Outbound SHIPPING_POST_Rpeort" + strDateFormat + ".pdf";
                                    // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                }
                            }
                            // rd.ExportToDisk(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                            reportFileName = "Outbound SHIPPING_POST_Rpeort" + DateTime.Now.ToFileTime() + ".pdf";
                            Session["RptFileName"] = strFileName;
                        }
                       
                    }
                    else if (l_str_rpt_selection == "BillofLadding")
                    {
                        strReportName = "rpt_iv_bill_of_lading.rpt";
                        string strDateFormat = string.Empty;
                        string strFileName = string.Empty;
                        OutboundShipInq objOutboundShipInq = new OutboundShipInq();
                        OutboundShipInqService ServiceObject = new OutboundShipInqService();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objEmail.Reportselection = l_str_rpt_selection;
                        objEmail = objEmailService.GetSendMailDetails(objEmail);
                        if (objEmail.ListEamilDetail.Count != 0)
                        {
                            objEmail.EmailMessageContent = (objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == null || objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailMessageContent.Trim();
                        }
                        else
                        {
                            objEmail.EmailMessageContent = "";
                        }
                        objOutboundShipInq.cmp_id = p_str_cmp_id.Trim();
                        objOutboundShipInq.ship_doc_id = SelectedID;
                        objOutboundShipInq = ServiceObject.OutboundShipInqBillofLaddingRpt(objOutboundShipInq);
                        if (objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt.Count > 0)
                        {
                            objOutboundShipInq.so_num = (objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].so_num == null || objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].so_num.Trim() == "" ? string.Empty : objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].so_num.Trim());
                            objOutboundShipInq.shipdate = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].Bol_ShipDt.ToString("dd/MM/yyyy");
                            objOutboundShipInq.po_num = (objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].cust_ordr_num == null || objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].cust_ordr_num.Trim() == "" ? string.Empty : objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].cust_ordr_num.Trim());
                            objOutboundShipInq.whs_id = (objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].whs_id == null || objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].whs_id.Trim() == "" ? string.Empty : objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].whs_id.Trim());
                            objOutboundShipInq.ship_to = (objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].ship_to == null || objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].ship_to.Trim() == "" ? string.Empty : objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].ship_to.Trim());
                            objOutboundShipInq = ServiceObject.GETRptValue(objOutboundShipInq);
                            if (objOutboundShipInq.LstshipbillOfladding.Count > 0)
                            {
                                objOutboundShipInq.TotCube = objOutboundShipInq.LstshipbillOfladding[0].TotCube;
                                objOutboundShipInq.TotCtns = objOutboundShipInq.LstshipbillOfladding[0].TotCtns;
                                objOutboundShipInq.TotWgt = objOutboundShipInq.LstshipbillOfladding[0].TotWgt;


                            }
                            else
                            {
                                objOutboundShipInq.TotCube = 0;
                                objOutboundShipInq.TotCtns = 0;
                                objOutboundShipInq.TotWgt = 0;
                            }
                            if ((objOutboundShipInq.po_num == "" || objOutboundShipInq.po_num == null || objOutboundShipInq.po_num == "-") && (objOutboundShipInq.whs_id == "" || objOutboundShipInq.whs_id == null || objOutboundShipInq.whs_id == "-") && (objOutboundShipInq.ship_to == "" || objOutboundShipInq.ship_to == null || objOutboundShipInq.ship_to == "-"))
                            {
                                l_str_rptdtl = objOutboundShipInq.cmp_id + "_" + "BillofLadding " + "_" + objOutboundShipInq.so_num;
                                objEmail.EmailSubject = objOutboundShipInq.cmp_id + "-" + " " + " " + "BillofLadding" + "|" + " " + " " + "ShipDoc#: " + " " + objOutboundShipInq.so_num + "|" + " " + " " + "Shipped Date: " + objOutboundShipInq.shipdate;
                                objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objOutboundShipInq.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "ShipDoc#: " + " " + " " + objOutboundShipInq.so_num + "\n" + "Shipped Date: " + " " + " " + objOutboundShipInq.shipdate + "\n" + "Ref#- " + "\n" + "Total Cartons Requested: " + objOutboundShipInq.TotCtns + " " + "Ctns" + "\n" + " Total Cube: " + objOutboundShipInq.TotCube + " " + "Lbs";
                            }

                            else if ((objOutboundShipInq.whs_id == "" || objOutboundShipInq.whs_id == null || objOutboundShipInq.whs_id == "-") && (objOutboundShipInq.ship_to == "" || objOutboundShipInq.ship_to == null || objOutboundShipInq.ship_to == "-"))

                            {
                                l_str_rptdtl = objOutboundShipInq.cmp_id + "_" + "BillofLadding" + "_" + objOutboundShipInq.so_num;
                                objEmail.EmailSubject = objOutboundShipInq.cmp_id + "-" + " " + " " + "BillofLadding" + "|" + " " + " " + "ShipDoc#: " + objOutboundShipInq.so_num + "|" + " " + " " + "Ship Date: " + objOutboundShipInq.shipdate + "|" + " " + " " + "Cust PO#: " + objOutboundShipInq.po_num;
                                objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objOutboundShipInq.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "SR#: " + " " + " " + objOutboundShipInq.so_num + "\n" + "Shipped Date: " + " " + " " + objOutboundShipInq.shipdate + "\n" + "Cust PO#: " + objOutboundShipInq.po_num + "\n" + "Ref#- " + "\n" + "Total Cartons Requested: " + objOutboundShipInq.TotCtns + " " + "Ctns" + "\n" + " Total Cube: " + objOutboundShipInq.TotCube + " " + "Lbs";

                            }
                            else if ((objOutboundShipInq.ship_to == "" || objOutboundShipInq.ship_to == null || objOutboundShipInq.ship_to == "-"))
                            {
                                l_str_rptdtl = objOutboundShipInq.cmp_id + "_" + "BillofLadding" + "_" + objOutboundShipInq.so_num;
                                objEmail.EmailSubject = objOutboundShipInq.cmp_id + "-" + " " + " " + "BillofLadding" + "|" + " " + " " + "ShipDoc#: " + objOutboundShipInq.so_num + "|" + " " + " " + "Ship Date:  " + objOutboundShipInq.shipdate + "|" + " " + " " + "Cust PO#: " + objOutboundShipInq.po_num + "|" + " " + " " + "StoreID: " + objOutboundShipInq.whs_id + "|" + " " + " " + "Ref# -Cust Name : " + objOutboundShipInq.cust_name;
                                objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objOutboundShipInq.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "ShipDoc#: " + " " + " " + objOutboundShipInq.so_num + "\n" + "Shipped Date: " + " " + " " + objOutboundShipInq.shipdate + "\n" + "Cust PO#: " + objOutboundShipInq.po_num + "\n" + "StoreID: " + objOutboundShipInq.whs_id + "\n" + "Ref#- " + "\n" + "CustName : " + objOutboundShipInq.cust_name + "\n" + "Total Cartons Requested: " + objOutboundShipInq.TotCtns + " " + "Ctns" + "\n" + " Total Cube: " + objOutboundShipInq.TotCube + " " + "Lbs";
                            }
                            else
                            {
                                l_str_rptdtl = objOutboundShipInq.cmp_id + "_" + "BillofLadding" + "_" + objOutboundShipInq.so_num;
                                objEmail.EmailSubject = objOutboundShipInq.cmp_id + "-" + " " + " " + "BillofLadding" + "|" + " " + " " + "ShipDoc#: " + objOutboundShipInq.so_num + "|" + " " + " " + "Ship Date:  " + objOutboundShipInq.shipdate + "|" + " " + " " + "Cust PO#: " + objOutboundShipInq.po_num + "|" + " " + " " + "StoreID: " + objOutboundShipInq.whs_id + "|" + " " + " " + "Ref# -Cust Name : " + objOutboundShipInq.cust_name;
                                objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objOutboundShipInq.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "ShipDoc#: " + " " + " " + objOutboundShipInq.so_num + "\n" + "Shipped Date: " + " " + " " + objOutboundShipInq.shipdate + "\n" + "Shipped To: " + "\n" + "Cust PO#: " + objOutboundShipInq.po_num + "\n" + "StoreID: " + objOutboundShipInq.whs_id + "\n" + "Ref#- " + "\n" + "CustName : " + objOutboundShipInq.cust_name + "\n" + "Total Cartons Requested: " + objOutboundShipInq.TotCtns + " " + "Ctns" + "\n" + " Total Cube: " + objOutboundShipInq.TotCube + " " + "Lbs";
                            }
                            //END
                            if (type == "PDF")
                            {
                                var rptSource = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt.ToList();
                                if (rptSource.Count > 0)
                                {
                                    using (ReportDocument rd = new ReportDocument())
                                    {
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                        AlocCount = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);
                                        objOutboundShipInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objOutboundShipInq.Image_Path);
                                        rd.SetParameterValue("TotCube", objOutboundShipInq.TotCube);
                                        rd.SetParameterValue("TotCarton", objOutboundShipInq.TotCtns);
                                        rd.SetParameterValue("TotWgt", objOutboundShipInq.TotWgt);
                                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");  //string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));

                                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                        // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                    }
                                }
                                // rd.ExportToDisk(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                reportFileName = l_str_rptdtl + "_" + DateTime.Now.ToFileTime() + ".pdf";
                                Session["RptFileName"] = strFileName;
                            }


                        }
                    }
                    else
                    {
                        Response.Write("<H2>Report not found</H2>");
                    }
                    objEmail.CmpId = p_str_cmp_id;
                    objEmail.screenId = ScreenID;
                    objEmail.username = objCompany.user_id;
                    objEmail.Reportselection = l_str_rpt_selection;
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
                    //CR2018-03-15-001 End
                    Mapper.CreateMap<Email, EmailModel>();
                    EmailModel EmailModel = Mapper.Map<Email, EmailModel>(objEmail);

                    return PartialView("_Email", EmailModel);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                jsonErrorCode = "-2";
            }

            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ShowReport(string SelectedID, string p_str_cmpid, string p_str_radio, string p_str_ship_docId_Fm, string p_str_ship_docId_To, string p_str_ship_dt_frm, string p_str_ship_dt_to, string p_str_CustId, string p_str_AlocId, string p_str_Shipto, string p_str_ship_via_name, string p_str_status, string p_str_Whsid, string type)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string batchId = string.Empty;
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            string l_str_rpt_so_num = string.Empty;
            objCustMaster.cust_id = p_str_cmpid;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }
            l_str_rpt_selection = p_str_radio;
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "GridSummary")
                    {

                        strReportName = "rpt_Shipping_Summary.rpt";
                        OutboundShipInq objOutboundShipInq = new OutboundShipInq();
                        OutboundShipInqService ServiceObject = new OutboundShipInqService();

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;


                        objOutboundShipInq.cmp_id = p_str_cmpid;
                        objOutboundShipInq.aloc_doc_id = p_str_AlocId;
                        objOutboundShipInq.ship_doc_id_Fm = p_str_ship_docId_Fm;
                        objOutboundShipInq.ship_doc_id_To = p_str_ship_docId_To;
                        objOutboundShipInq.Ship_dt_Fm = p_str_ship_dt_frm;
                        objOutboundShipInq.Ship_dt_To = p_str_ship_dt_to;
                        objOutboundShipInq.cust_id = p_str_CustId;
                        objOutboundShipInq.status = p_str_status;
                        objOutboundShipInq.shipto_id = p_str_Shipto;
                        objOutboundShipInq.ship_via_name = p_str_ship_via_name;
                        objOutboundShipInq.whs_id = p_str_Whsid;
                        objOutboundShipInq = ServiceObject.OutboundShipInqSummaryRpt(objOutboundShipInq);

                        if (type == "PDF")
                        {
                            var rptSource = objOutboundShipInq.LstOutboundShipInqdetail.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objOutboundShipInq.LstOutboundShipInqdetail.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objOutboundShipInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objOutboundShipInq.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "ShipPostGridSummaryReport");
                                }
                            }
                        }
                        
                        else
                        if (type == "Excel")
                        {

                            List<OB_SHIP_Grid_SMRYExcel> li = new List<OB_SHIP_Grid_SMRYExcel>();
                            for (int i = 0; i < objOutboundShipInq.LstOutboundShipInqdetail.Count; i++)
                            {

                                OB_SHIP_Grid_SMRYExcel objOBInquiryExcel = new OB_SHIP_Grid_SMRYExcel();
                                objOBInquiryExcel.ship_doc_id = objOutboundShipInq.LstOutboundShipInqdetail[i].ship_doc_id;
                                objOBInquiryExcel.ShipDt = objOutboundShipInq.LstOutboundShipInqdetail[i].OB_Ship_dt;
                                objOBInquiryExcel.status = objOutboundShipInq.LstOutboundShipInqdetail[i].status;

                                objOBInquiryExcel.WhsId = objOutboundShipInq.LstOutboundShipInqdetail[i].WhsId;
                                objOBInquiryExcel.CustId = objOutboundShipInq.LstOutboundShipInqdetail[i].CustId;
                                objOBInquiryExcel.ShipTo = objOutboundShipInq.LstOutboundShipInqdetail[i].ShipTo;
                                objOBInquiryExcel.ShipViaName = objOutboundShipInq.LstOutboundShipInqdetail[i].ShipViaName;

                                objOBInquiryExcel.AlocDocId = objOutboundShipInq.LstOutboundShipInqdetail[i].AlocDocId;
                                objOBInquiryExcel.Ship_post_dt = objOutboundShipInq.LstOutboundShipInqdetail[i].Ship_post_dt;

                                li.Add(objOBInquiryExcel);
                            }

                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["OB_SHIP_GRID_SMRY"] = gv;
                            return new DownloadFileActionResult((GridView)Session["OB_SHIP_GRID_SMRY"], "OB_SHIP_GRID_SMRY" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



                        }

                    }
                    else if (l_str_rpt_selection == "Outboundpackslip")
                    {

                        strReportName = "rpt_iv_packslip_tpw.rpt";
                        OutboundShipInq objOutboundShipInq = new OutboundShipInq();
                        OutboundShipInqService ServiceObject = new OutboundShipInqService();

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundShipInq.cmp_id = p_str_cmpid;
                        objOutboundShipInq.ship_doc_id = SelectedID;
                        objOutboundShipInq = ServiceObject.OutboundShipInqpackSlipRpt(objOutboundShipInq);
                        objOutboundShipInq = ServiceObject.GetTotCubesRpt(objOutboundShipInq);
                        if (objOutboundShipInq.LstPalletCount.Count > 0)
                        {
                            objOutboundShipInq.TotCube = objOutboundShipInq.LstPalletCount[0].TotCube;
                        }
                        else
                        {
                            objOutboundShipInq.TotCube = 0;
                        }
                        if (type == "PDF")
                        {
                            var rptSource = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objOutboundShipInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objOutboundShipInq.Image_Path);
                                    rd.SetParameterValue("SumOfCubes", objOutboundShipInq.TotCube);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "ShipPostPackingSlipReport");
                                }
                            }
                        }
                       
                        else
                        if (type == "Excel")
                        {

                            List<OB_SHIP_PACKING_SLIPExcel> li = new List<OB_SHIP_PACKING_SLIPExcel>();
                            for (int i = 0; i < objOutboundShipInq.LstOutboundShipInqpackingSlipRpt.Count; i++)
                            {

                                OB_SHIP_PACKING_SLIPExcel objOBInquiryExcel = new OB_SHIP_PACKING_SLIPExcel();
                                objOBInquiryExcel.ShipDocID = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].ship_doc_id;
                                objOBInquiryExcel.ShipDocDate = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].ship_dt;
                                objOBInquiryExcel.SRNo = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].so_num;

                                objOBInquiryExcel.CustPO = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].cust_ord;
                                objOBInquiryExcel.WhsId = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].whs_id;
                                objOBInquiryExcel.CancelDt = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].cancel_dt;

                                objOBInquiryExcel.StoreId = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].cust_store;
                                objOBInquiryExcel.DeptID = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].cust_dept;
                                objOBInquiryExcel.LineNo = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].line_num;


                                objOBInquiryExcel.Style = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].itm_num;
                                objOBInquiryExcel.Color = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].itm_color;
                                objOBInquiryExcel.Size = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].itm_size;
                                objOBInquiryExcel.Description = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].itm_name;
                                //objOBInquiryExcel.Ppk = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].itm_qty;
                                objOBInquiryExcel.Ppk = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].ctn_cnt;
                                objOBInquiryExcel.ItemQty = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].itm_qty;

                                //objOBInquiryExcel.ctn_line = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].ctn_line;
                                objOBInquiryExcel.Ctns = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].ctn_cnt;
                                objOBInquiryExcel.Qty = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].ctn_qty;
                                objOBInquiryExcel.UOM = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].qtyUOM;

                                //objOBInquiryExcel.line_qty = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].line_qty;
                                //objOBInquiryExcel.aloc_doc_id = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].aloc_doc_id;
                                //objOBInquiryExcel.pick_qty = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].pick_qty;
                                //objOBInquiryExcel.pick_ctns = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].pick_ctns;
                                //objOBInquiryExcel.shipto_id = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].shipto_id;
                                //objOBInquiryExcel.SoldToId = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].SoldToId;
                                li.Add(objOBInquiryExcel);
                            }

                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["OB_PACKING_SLIP"] = gv;
                            return new DownloadFileActionResult((GridView)Session["OB_PACKING_SLIP"], "OB_PACKING_SLIP" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



                        }

                    }
                    else if (l_str_rpt_selection == "BillofLadding")
                    {
                        strReportName = "rpt_iv_bill_of_lading.rpt";
                        OutboundShipInq objOutboundShipInq = new OutboundShipInq();
                        OutboundShipInqService ServiceObject = new OutboundShipInqService();

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundShipInq.cmp_id = p_str_cmpid;
                        objOutboundShipInq.ship_doc_id = SelectedID;
                        objOutboundShipInq = ServiceObject.OutboundShipInqBillofLaddingRpt(objOutboundShipInq);
                        //CR20180623-001
                        objOutboundShipInq = ServiceObject.GETRptValue(objOutboundShipInq);
                        if (objOutboundShipInq.LstshipbillOfladding.Count > 0)
                        {
                            objOutboundShipInq.TotCube = objOutboundShipInq.LstshipbillOfladding[0].TotCube;
                            objOutboundShipInq.TotCtns = objOutboundShipInq.LstshipbillOfladding[0].TotCtns;
                            objOutboundShipInq.TotWgt = objOutboundShipInq.LstshipbillOfladding[0].TotWgt;

                        }
                        else
                        {
                            objOutboundShipInq.TotCube = 0;
                            objOutboundShipInq.TotCtns = 0;
                            objOutboundShipInq.TotWgt = 0;
                        }
                        //END
                        if (type == "PDF")
                        {
                            var rptSource = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objOutboundShipInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objOutboundShipInq.Image_Path);
                                    rd.SetParameterValue("TotCube", objOutboundShipInq.TotCube);
                                    rd.SetParameterValue("TotCarton", objOutboundShipInq.TotCtns);
                                    rd.SetParameterValue("TotWgt", objOutboundShipInq.TotWgt);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "ShipPostBillOfLaddingReport");
                                }
                            }

                        }
                       
                        else
                        if (type == "Excel")
                        {
                            objOutboundShipInq = ServiceObject.OutboundShipInqBillofLaddingExcel(objOutboundShipInq);

                            List<OB_SHIP_BOL_Excel> li = new List<OB_SHIP_BOL_Excel>();
                            for (int i = 0; i < objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt.Count; i++)
                            {

                                OB_SHIP_BOL_Excel objOBInquiryExcel = new OB_SHIP_BOL_Excel();
                                objOBInquiryExcel.LineNo = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].line_num;
                                objOBInquiryExcel.Style = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].itm_num;
                                objOBInquiryExcel.Color = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].itm_color;
                                objOBInquiryExcel.Size = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].itm_size;
                                objOBInquiryExcel.Ppk = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].ctn_qty;
                                objOBInquiryExcel.Ctns = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].ctn_cnt;
                                objOBInquiryExcel.ShipQty = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].line_qty;
                                objOBInquiryExcel.Uom = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].pick_uom;


                                li.Add(objOBInquiryExcel);
                            }
                            //List<OB_SHIP_BOLExcel> li = new List<OB_SHIP_BOLExcel>();
                            //for (int i = 0; i < objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt.Count; i++)
                            //{

                            //    OB_SHIP_BOLExcel objOBInquiryExcel = new OB_SHIP_BOLExcel();
                            //    objOBInquiryExcel.aloc_doc_id = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].aloc_doc_id;
                            //    objOBInquiryExcel.so_itm_num = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].so_itm_num;
                            //    objOBInquiryExcel.ctn_cnt = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].ctn_cnt;
                            //    objOBInquiryExcel.line_qty = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].line_qty;
                            //    objOBInquiryExcel.ShipDt = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].OB_Ship_dt;
                            //    objOBInquiryExcel.ship_doc_id = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].ship_doc_id;
                            //    objOBInquiryExcel.so_num = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].so_num;
                            //    objOBInquiryExcel.cust_ord = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].cust_ord;

                            //    objOBInquiryExcel.pick_uom = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].pick_uom;
                            //    objOBInquiryExcel.pick_qty = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].pick_qty;
                            //    objOBInquiryExcel.line_num = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].line_num;
                            //    objOBInquiryExcel.itm_num = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].itm_num;
                            //    objOBInquiryExcel.itm_color = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].itm_color;
                            //    objOBInquiryExcel.itm_size = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].itm_size;
                            //    objOBInquiryExcel.ctn_line = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].ctn_line;
                            //    objOBInquiryExcel.ctn_qty = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].ctn_qty;

                            //    //objOBInquiryExcel.cmp_id = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[i].cmp_id;
                            //    objOBInquiryExcel.cust_ordr_num = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].cust_ordr_num.ToString();
                            //    objOBInquiryExcel.ship_to_name = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[i].ship_to_name;

                            //    li.Add(objOBInquiryExcel);
                            //}

                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["OB_BOL"] = gv;
                            return new DownloadFileActionResult((GridView)Session["OB_BOL"], "OB_BOL" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



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
        public ActionResult RefreshReport()
        {
            return PartialView("_ReportsDisplay");
        }
        public ActionResult ReportView()
        {
            return View();
        }
        public ActionResult ShippingDetail(string shipdocid, string cmp_id)
        {
            OutboundShipInq objOutboundShipInq = new OutboundShipInq();
            OutboundShipInqService ServiceObject = new OutboundShipInqService();
            objOutboundShipInq.cmp_id = cmp_id;
            objOutboundShipInq.ship_doc_id = shipdocid;
            objOutboundShipInq = ServiceObject.OutboundShipInqhdr(objOutboundShipInq);
            objOutboundShipInq.status = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[0].status.Trim();
            objOutboundShipInq.whs_id = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[0].whs_id.Trim();
            objOutboundShipInq.ship_type = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[0].ship_type.Trim();
            objOutboundShipInq.status = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[0].status.Trim();
            if (objOutboundShipInq.status.Trim() == "O")
            {
                objOutboundShipInq.status = "OPEN";
            }
            else if (objOutboundShipInq.status.Trim() == "P")
            {
                objOutboundShipInq.status = "POST";
            }
            objOutboundShipInq.notes = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[0].notes.Trim();
            objOutboundShipInq.Ship_dt = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt[0].Ship_dt;
            objOutboundShipInq = ServiceObject.OutboundShipInqdtl(objOutboundShipInq);
            Mapper.CreateMap<OutboundShipInq, OutboundShipInqModel>();
            OutboundShipInqModel objOutboundShipInqModel = Mapper.Map<OutboundShipInq, OutboundShipInqModel>(objOutboundShipInq);
            return PartialView("_ShippingDetail", objOutboundShipInqModel);
        }

        public ActionResult ShipPost(string CmpId, string ShipDocId, string Shipdt, string screentitle,string p_cust_ordr_num, string p_ordr_num, string p_cust_name)
        {
            decimal l_str_pallet_wgt = 0;
            decimal l_str_pallet_cube = 0;
            string l_str_pallet_note = "Item(s)/SKU:";
            string l_str_prev_itm = string.Empty;
            OutboundShipInq objOutboundShipInq = new OutboundShipInq();
            OutboundShipInqService ServiceObject = new OutboundShipInqService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objOutboundShipInq.cmp_id = CmpId;
            objOutboundShipInq.ship_doc_id = ShipDocId;
            objOutboundShipInq.screentitle = screentitle;
            
            objOutboundShipInq.ship_post_dt = DateTime.Now.ToString("MM/dd/yyyy");
            objOutboundShipInq.ship_ready_dt = Shipdt;
            objOutboundShipInq = ServiceObject.GEtStrgBillTYpe(objOutboundShipInq);  
            objOutboundShipInq.bill_type = objOutboundShipInq.ListStrgBillType[0].bill_type;  
            objOutboundShipInq.bill_inout_type = objOutboundShipInq.ListStrgBillType[0].bill_inout_type;  
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany.cust_cmp_id = CmpId;
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objOutboundShipInq.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objOutboundShipInq = ServiceObject.GetShipPostGrid(objOutboundShipInq);
            for (int i = 0; i < objOutboundShipInq.ListShipPost.Count(); i++)
            {
                if (l_str_prev_itm != objOutboundShipInq.ListShipPost[i].itm_num)
                {
                    l_str_pallet_note = l_str_pallet_note + objOutboundShipInq.ListShipPost[i].itm_num + ",";
                }
                l_str_prev_itm = objOutboundShipInq.ListShipPost[i].itm_num;

            }
            l_str_pallet_note = l_str_pallet_note.Remove(l_str_pallet_note.Length - 1);
            objOutboundShipInq.PALLET_NOTE = l_str_pallet_note;
            ServiceObject.GetPoNum(objOutboundShipInq);
            objOutboundShipInq = ServiceObject.GetContId(objOutboundShipInq);
            if (objOutboundShipInq.ListGETPONUM.Count > 0)
            {
                objOutboundShipInq.PONUM = (objOutboundShipInq.ListGETPONUM[0].PONUM == null || objOutboundShipInq.ListGETPONUM[0].PONUM == string.Empty ? string.Empty : objOutboundShipInq.ListGETPONUM[0].PONUM.Trim());

            }
            else
            {
                objOutboundShipInq.PONUM = "-";
            }
            objOutboundShipInq.po_num = p_ordr_num;
            objOutboundShipInq.cust_po_num = p_cust_ordr_num;
            objOutboundShipInq.cust_name = p_cust_name;

            //Added By Ravi -> Introducing Linq Concepts         
            var p_str_get_wgt = objOutboundShipInq.ListShipPost.GroupBy(x => x.pkg_id).Select(x => new { Count = x.Sum(y => y.wgt) });
            var Wgt = p_str_get_wgt.ToList();
            if (Wgt.Count() == 0)
            {
                objOutboundShipInq.PALLET_WEIGHT = 0;
                objOutboundShipInq.PALLET_CUBE = 0;
            }
            else
            {
                l_str_pallet_wgt = Convert.ToDecimal(Wgt[0].Count);
                objOutboundShipInq.PALLET_WEIGHT = Convert.ToDecimal(l_str_pallet_wgt);
                var p_str_get_cube = objOutboundShipInq.ListShipPost.GroupBy(x => x.pkg_id).Select(x => new { Count = x.Sum(y => y.cube) });
                var Cube = p_str_get_cube.ToList();
                l_str_pallet_cube = Convert.ToDecimal(Cube[0].Count);
                objOutboundShipInq.PALLET_CUBE = Convert.ToDecimal(l_str_pallet_cube);
            }
            //END
            Mapper.CreateMap<OutboundShipInq, OutboundShipInqModel>();
            OutboundShipInqModel objOutboundShipInqModel = Mapper.Map<OutboundShipInq, OutboundShipInqModel>(objOutboundShipInq);
            return PartialView("_ShippingPost", objOutboundShipInqModel);
        }
        public ActionResult SaveShippingPost(string p_str_cmp_id, string p_str_shipdocid, string p_str_ship_dt, string p_str_ship_post_dt, string p_str_ship_via_name, string p_str_cont_id,
            string p_str_seal_num, string p_str_picked_by, string p_str_track_num, string p_str_ship_note, string P_STR_CNTR_PALLET, string P_STR_PALLET_CNTR_ID, string P_STR_PALLET_WEIGHT,
            string P_STR_PALLET_CUBE, string P_STR_PONUM, string P_STR_PALLET_NOTE, string P_STR_BILL_TYPE)
        {

            OutboundShipInq objOutboundShipInq = new OutboundShipInq();
            OutboundShipInqService ServiceObject = new OutboundShipInqService();
            objOutboundShipInq.cmp_id = p_str_cmp_id;
            objOutboundShipInq.ship_doc_id = p_str_shipdocid;
            objOutboundShipInq.ship_ready_dt = p_str_ship_dt;
            objOutboundShipInq.ship_post_dt = p_str_ship_post_dt;
            objOutboundShipInq.ship_via_name = p_str_ship_via_name;
            objOutboundShipInq.cont_id = p_str_cont_id;
            objOutboundShipInq.seal_num = p_str_seal_num;
            objOutboundShipInq.picked_by = p_str_picked_by;
            objOutboundShipInq.track_num = p_str_track_num;
            objOutboundShipInq.ship_note = p_str_ship_note;
            objOutboundShipInq.bill_type = P_STR_BILL_TYPE;     // CR_3PL_MVC_BL_2018_0312_003 Added By SONIYA
            objOutboundShipInq = ServiceObject.GetCheckShipPost(objOutboundShipInq);
            if (objOutboundShipInq.ListCheckShipPost.Count == 0)
            {
                ServiceObject.SaveShipPost(objOutboundShipInq);
                ServiceObject.InsertShipPost(objOutboundShipInq);
                //CR20180813-001 Added By Nithya
                objOutboundShipInq = ServiceObject.Get_SoNo(objOutboundShipInq);
                if (objOutboundShipInq.LstshipbillOfladding.Count > 0)
                {
                    objOutboundShipInq.Sonum = objOutboundShipInq.LstshipbillOfladding[0].so_num;
                    objOutboundShipInq.aloc_doc_id = objOutboundShipInq.LstshipbillOfladding[0].aloc_doc_id;
                }
                objOutboundShipInq.cmp_id = p_str_cmp_id;
                objOutboundShipInq.Sonum = objOutboundShipInq.Sonum;
                objOutboundShipInq.aloc_doc_id = objOutboundShipInq.aloc_doc_id;
                objOutboundShipInq.ship_doc_id = p_str_shipdocid;
                objOutboundShipInq.mode = "SHIP-POST";
                objOutboundShipInq.maker = Session["UserID"].ToString().Trim();
                objOutboundShipInq.makerdt = DateTime.Now.ToString("MM/dd/yyyy");
                objOutboundShipInq.Auditcomment = "Shipped";
                objOutboundShipInq = ServiceObject.Add_To_proc_save_audit_trail(objOutboundShipInq);
                //END
            }
            else
            {
                return Json(objOutboundShipInq.ListCheckShipPost.Count, JsonRequestBehavior.AllowGet);
            }
            if (objOutboundShipInq.bill_type == "Pallet")            // CR_3PL_MVC_BL_2018_0312_003 Added By SONIYA
            {
                objOutboundShipInq.CNTR_PALLET = P_STR_CNTR_PALLET;
                objOutboundShipInq.CNTR_ID = P_STR_PALLET_CNTR_ID;                          // CR_3PL_MVC_IB_2018_0303_001 Added by Soniya
                objOutboundShipInq.PALLET_WEIGHT = Convert.ToDecimal(P_STR_PALLET_WEIGHT);
                objOutboundShipInq.PALLET_CUBE = Convert.ToDecimal(P_STR_PALLET_CUBE);
                objOutboundShipInq.PONUM = P_STR_PONUM;
                objOutboundShipInq.PALLET_NOTE = P_STR_PALLET_NOTE;
                objOutboundShipInq.Shipdt = p_str_ship_dt;//CR2018-03-08-001 Added By nithya
                ServiceObject.InsertPalletShipDtl(objOutboundShipInq);
                //CR20180813-001 Added By Nithya
                objOutboundShipInq = ServiceObject.Get_SoNo(objOutboundShipInq);
                if (objOutboundShipInq.LstshipbillOfladding.Count > 0)
                {
                    objOutboundShipInq.Sonum = objOutboundShipInq.LstshipbillOfladding[0].so_num;
                    objOutboundShipInq.aloc_doc_id = objOutboundShipInq.LstshipbillOfladding[0].aloc_doc_id;
                }
                objOutboundShipInq.cmp_id = p_str_cmp_id;
                objOutboundShipInq.Sonum = objOutboundShipInq.Sonum;
                objOutboundShipInq.aloc_doc_id = objOutboundShipInq.aloc_doc_id;
                objOutboundShipInq.ship_doc_id = p_str_shipdocid;
                objOutboundShipInq.mode = "SHIP-POST";
                objOutboundShipInq.maker = Session["UserID"].ToString().Trim();
                objOutboundShipInq.makerdt = DateTime.Now.ToString("MM/dd/yyyy");
                objOutboundShipInq.Auditcomment = "Shipped";
                objOutboundShipInq = ServiceObject.Add_To_proc_save_audit_trail(objOutboundShipInq);
                //END
            }
            Mapper.CreateMap<OutboundShipInq, OutboundShipInqModel>();
            OutboundShipInqModel objOutboundShipInqModel = Mapper.Map<OutboundShipInq, OutboundShipInqModel>(objOutboundShipInq);
            return PartialView("~/Views/OutboundShipInq/OutboundShipInq.cshtml", objOutboundShipInqModel);
        }
        public ActionResult ShowdtlshipReport(string p_str_cmpid, string p_str_status, string p_str_Shipping_id)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string cmp_id = p_str_cmpid;
            string ib_doc_id = p_str_Shipping_id;
            string l_str_status = p_str_status;
            OutboundShipInq objOutbound = new OutboundShipInq();
            OutboundShipInqService SerObject = new OutboundShipInqService();
            objCustMaster.cust_id = p_str_cmpid;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }
            try
            {
                if (isValid)
                {
                    if (p_str_status == "OPEN")
                    {
                        strReportName = "rpt_iv_packslip_tpw.rpt";
                        OutboundShipInq objOutboundShipInq = new OutboundShipInq();
                        OutboundShipInqService ServiceObject = new OutboundShipInqService();
    
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundShipInq.cmp_id = p_str_cmpid;
                        objOutboundShipInq.ship_doc_id = p_str_Shipping_id;
                        objOutboundShipInq = ServiceObject.OutboundShipInqpackSlipRpt(objOutboundShipInq);
                        objOutboundShipInq = ServiceObject.GetTotCubesRpt(objOutboundShipInq);
                        if (objOutboundShipInq.LstPalletCount.Count > 0)
                        {
                            objOutboundShipInq.TotCube = objOutboundShipInq.LstPalletCount[0].TotCube;
                        }
                        else
                        {
                            objOutboundShipInq.TotCube = 0;
                        }
                        var rptSource = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                objOutboundShipInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objOutboundShipInq.Image_Path);
                                rd.SetParameterValue("SumOfCubes", objOutboundShipInq.TotCube);
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            }
                        }
                    }
                    if (l_str_status == "POST")
                    {
                        strReportName = "rpt_iv_bill_of_lading.rpt";
                        OutboundShipInq objOutboundShipInq = new OutboundShipInq();
                        OutboundShipInqService ServiceObject = new OutboundShipInqService();

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundShipInq.cmp_id = p_str_cmpid;
                        objOutboundShipInq.ship_doc_id = p_str_Shipping_id;

                        objOutboundShipInq = ServiceObject.OutboundShipInqBillofLaddingRpt(objOutboundShipInq);
                        var rptSource = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                objOutboundShipInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objOutboundShipInq.Image_Path);
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
        //CR_3PL_MVC _2018_0413_001 Added By Nithya
        public JsonResult OB_INQ_HDR_DATA(string p_str_cmp_id, string p_str_ship_docId_Fm, string p_str_ship_docId_To, string p_str_ship_dt_frm, string p_str_ship_dt_to, string p_str_CustId, string p_str_AlocId, string p_str_Shipto, string p_str_ship_via_name, string p_str_Whsid, string p_str_status, string p_str_screentitle)
        {
            OutboundShipInq objOutboundShipInq = new OutboundShipInq();
            OutboundShipInqService ServiceObject = new OutboundShipInqService();

            Session["g_str_cmp_id"] = p_str_cmp_id.Trim();
            Session["TEMP_CMP_ID"] = p_str_cmp_id.Trim();
            Session["ship_doc_id_Fm"] = p_str_ship_docId_Fm.Trim();
            Session["ship_doc_id_To"] = p_str_ship_docId_To.Trim();
            Session["Ship_dt_Fm"] = p_str_ship_dt_frm.Trim();
            Session["Ship_dt_To"] = p_str_ship_dt_to.Trim();
            Session["cust_id"] = p_str_CustId.Trim();
            Session["aloc_doc_id"] = p_str_AlocId.Trim();
            Session["ship_to"] = p_str_Shipto.Trim();
            Session["ship_via_name"] = p_str_ship_via_name.Trim();
            Session["whs_id"] = p_str_Whsid.Trim();
            Session["status"] = p_str_status.Trim();
            Session["Screentitle"] = p_str_screentitle.Trim();
            objOutboundShipInq = ServiceObject.GetOutboundShipInq(objOutboundShipInq);
            return Json(objOutboundShipInq.LstOutboundShipInqdetail, JsonRequestBehavior.AllowGet);
        }
        //CR_3PL_MVC _2018_0413_001 Added By Nithya
        public ActionResult outboundinquiryShipdtl(string p_str_cmp_id, string p_str_Shipdocid)
        {
            try
            {

                OutboundShipInq objOutboundShipInq = new OutboundShipInq();
                OutboundShipInqService ServiceObject = new OutboundShipInqService();
                string l_str_search_flag = string.Empty;
                string l_str_is_another_usr = string.Empty;

                l_str_is_another_usr = Session["IS3RDUSER"].ToString();
                objOutboundShipInq.IS3RDUSER = l_str_is_another_usr.Trim();
                l_str_search_flag = Session["g_str_Search_flag"].ToString().Trim();
                objOutboundShipInq.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                if (l_str_search_flag == "True")
                {
                    objOutboundShipInq.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                    objOutboundShipInq.cmp_id = p_str_cmp_id.Trim();
                    objOutboundShipInq.ship_doc_id_Fm = Session["ship_doc_id_Fm"].ToString().Trim();
                    objOutboundShipInq.ship_doc_id_To = Session["ship_doc_id_To"].ToString().Trim();
                    objOutboundShipInq.Ship_dt_Fm = Session["Ship_dt_Fm"].ToString();
                    objOutboundShipInq.Ship_dt_To = Session["Ship_dt_To"].ToString();
                    objOutboundShipInq.cust_id = Session["cust_id"].ToString();
                    objOutboundShipInq.aloc_doc_id = Session["aloc_doc_id"].ToString();
                    objOutboundShipInq.ship_to = Session["ship_to"].ToString();
                    objOutboundShipInq.ship_via_name = Session["ship_via_name"].ToString();
                    objOutboundShipInq.whs_id = Session["whs_id"].ToString();
                    objOutboundShipInq.status = Session["status"].ToString();
                    objOutboundShipInq.screentitle = Session["Screentitle"].ToString();


                }
                else
                {
                    objOutboundShipInq.cmp_id = p_str_cmp_id;
                    objOutboundShipInq.ship_doc_id_Fm = p_str_Shipdocid.Trim();
                    objOutboundShipInq.ship_doc_id_To = p_str_Shipdocid.Trim();
                    objOutboundShipInq.screentitle = Session["Screentitle"].ToString().Trim();
                }
                objOutboundShipInq = ServiceObject.GetOutboundShipInq(objOutboundShipInq);
                Mapper.CreateMap<OutboundShipInq, OutboundShipInqModel>();
                OutboundShipInqModel objOutboundShipInqModel = Mapper.Map<OutboundShipInq, OutboundShipInqModel>(objOutboundShipInq);
                return PartialView("_OutboundShipInq", objOutboundShipInqModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult GetOutboundShipInquiryFromDashboard(string p_str_cmp_id)
        {
            try
            {
                string l_str_is_3rd_usr = string.Empty;
                OutboundShipInq objOutboundShipInq = new OutboundShipInq();
                OutboundShipInqService ServiceObject = new OutboundShipInqService();
                l_str_is_3rd_usr = Session["IS3RDUSER"].ToString();
                objOutboundShipInq.IS3RDUSER = l_str_is_3rd_usr.Trim();
                objOutboundShipInq.cmp_id = p_str_cmp_id;
                objOutboundShipInq.ship_doc_id_Fm = "";
                objOutboundShipInq.ship_doc_id_To = "";
                objOutboundShipInq.Ship_dt_Fm = "";
                objOutboundShipInq.Ship_dt_To = "";
                objOutboundShipInq.cust_id = "";
                objOutboundShipInq.aloc_doc_id = "";
                objOutboundShipInq.ship_to = "";
                objOutboundShipInq.ship_via_name = "";
                objOutboundShipInq.whs_id = "";
                objOutboundShipInq = ServiceObject.GetOutboundShipInq(objOutboundShipInq);
                Mapper.CreateMap<OutboundShipInq, OutboundShipInqModel>();
                OutboundShipInqModel objOutboundShipInqModel = Mapper.Map<OutboundShipInq, OutboundShipInqModel>(objOutboundShipInq);
                return PartialView("_OutboundShipInq", objOutboundShipInqModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //CR-180425-001 Added By Nithya
        public ActionResult ShipUnPost(string CmpId, string ShipDocId, string Shipdt, string screentitle, string ShipPostdt)
        {
            OutboundShipInq objOutboundShipInq = new OutboundShipInq();
            OutboundShipInqService ServiceObject = new OutboundShipInqService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany.cust_cmp_id = CmpId;
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objOutboundShipInq.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objOutboundShipInq.cmp_id = CmpId;
            objOutboundShipInq.ship_doc_id = ShipDocId;
            objOutboundShipInq.screentitle = screentitle;
            objOutboundShipInq.ship_post_dt = ShipPostdt;
            objOutboundShipInq.ship_ready_dt = Shipdt;
            objOutboundShipInq = ServiceObject.GetShipUnPostGrid(objOutboundShipInq);
            Mapper.CreateMap<OutboundShipInq, OutboundShipInqModel>();
            OutboundShipInqModel objOutboundShipInqModel = Mapper.Map<OutboundShipInq, OutboundShipInqModel>(objOutboundShipInq);
            return PartialView("_ShippingUnpost", objOutboundShipInqModel);
        }
        public ActionResult SaveShippingUnPost(string p_str_cmp_id, string p_str_shipdocid)
        {
            OutboundShipInq objOutboundShipInq = new OutboundShipInq();
            OutboundShipInqService ServiceObject = new OutboundShipInqService();
            objOutboundShipInq.cmp_id = p_str_cmp_id;
            objOutboundShipInq.ship_doc_id = p_str_shipdocid;
            objOutboundShipInq = ServiceObject.Get_SoNo(objOutboundShipInq);
            if (objOutboundShipInq.LstshipbillOfladding.Count > 0)
            {
                objOutboundShipInq.Sonum = objOutboundShipInq.LstshipbillOfladding[0].so_num;
                objOutboundShipInq.aloc_doc_id = objOutboundShipInq.LstshipbillOfladding[0].aloc_doc_id;
            }
            ServiceObject.SaveShipUnPost(objOutboundShipInq);          
            objOutboundShipInq.cmp_id = p_str_cmp_id;
            objOutboundShipInq.Sonum = objOutboundShipInq.Sonum;
            objOutboundShipInq.aloc_doc_id = objOutboundShipInq.aloc_doc_id;
            objOutboundShipInq.ship_doc_id = p_str_shipdocid;
            objOutboundShipInq.mode = "SHIP-UPOST";
            objOutboundShipInq.maker = Session["UserID"].ToString().Trim();
            objOutboundShipInq.makerdt = DateTime.Now.ToString("MM/dd/yyyy");
            objOutboundShipInq.Auditcomment = "Shipped Unpost";
            objOutboundShipInq = ServiceObject.Add_To_proc_save_audit_trail(objOutboundShipInq);
            Mapper.CreateMap<OutboundShipInq, OutboundShipInqModel>();
            OutboundShipInqModel objOutboundShipInqModel = Mapper.Map<OutboundShipInq, OutboundShipInqModel>(objOutboundShipInq);
            return PartialView("~/Views/OutboundShipInq/OutboundShipInq.cshtml", objOutboundShipInqModel);
        }
    }
}