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
    public class OutboundShipRptController : Controller
    {
        // GET: OutboundShipRpt
        int TotalQty = 0;
        int TotalCtn = 0;
        CustMaster objCustMaster = new CustMaster();
        ICustMasterService objCustMasterService = new CustMasterService();
        public ActionResult OutboundShipRpt(string FullFillType, string cmp, string status, string DateFm, string DateTo, string screentitle)
        {
            string l_str_cmp_id = string.Empty;
            string l_str_Dflt_Dt_Reqd = string.Empty;
            try
            {
                OutboundShipRpt objOutboundShipRpt = new OutboundShipRpt();
                OutboundShipRptService ServiceObject = new OutboundShipRptService();
                objOutboundShipRpt.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
                objOutboundShipRpt.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                objOutboundShipRpt.ship_dt_fm = DateTime.Now.AddDays(Common.clsGlobal.DispDateFrom).ToString("MM/dd/yyyy");
                objOutboundShipRpt.ship_dt_to = DateTime.Now.ToString("MM/dd/yyyy");
                if (objOutboundShipRpt.cmp_id != "" && FullFillType == null)
                {


                    //objCompany.cmp_id = l_str_cmp_id;
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objOutboundShipRpt.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
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
                        objOutboundShipRpt.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                        
                    }
                    if (FullFillType != null)
                    {
                        objCompany.user_id = Session["UserID"].ToString().Trim();
                        objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                        objOutboundShipRpt.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                        objOutboundShipRpt.cmp_id = cmp;                     
                        objOutboundShipRpt.whs_id = "";
                        objOutboundShipRpt.cust_ord = "";                       
                        objOutboundShipRpt.ship_doc_id = "";
                        objOutboundShipRpt.ship_via_name = "";
                        objOutboundShipRpt.itm_num = "";
                        objOutboundShipRpt.itm_color = "";
                        objOutboundShipRpt.itm_size = "";
                        objOutboundShipRpt.Status = "";
                        if (l_str_Dflt_Dt_Reqd == "Y")
                        {
                            objOutboundShipRpt = ServiceObject.GetOutboundShipRptInquiryDetails(objOutboundShipRpt);
                       
                        for (int i = 0; i < objOutboundShipRpt.LstOutboundShipRptInqdetail.Count(); i++)
                        {
                            TotalCtn = TotalCtn + objOutboundShipRpt.LstOutboundShipRptInqdetail[i].Ctns;
                            objOutboundShipRpt.TotalCtn = TotalCtn;
                            TotalQty = TotalQty + objOutboundShipRpt.LstOutboundShipRptInqdetail[i].Pcs;
                            objOutboundShipRpt.TotalQty = TotalQty;
                        }
                        }

                    }
                }               
                LookUp objLookUp = new LookUp();
                LookUpService ServiceObject1 = new LookUpService();
                objLookUp.id = "5";
                objLookUp.lookuptype = "INVENTORYINQ";
                objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                objOutboundShipRpt.ListLookUpDtl = objLookUp.ListLookUpDtl;
                //20180428 aDDED By NITHYA dEFAULT WHSID
                objOutboundShipRpt = ServiceObject.GetDftWhs(objOutboundShipRpt);
                string l_str_DftWhs = objOutboundShipRpt.LstWhsDetails[0].dft_whs.Trim();
                if (l_str_DftWhs != "" || l_str_DftWhs != null)
                {
                    objOutboundShipRpt.whs_id = l_str_DftWhs;
                }
                objCompany.cust_cmp_id = cmp;
                objCompany.whs_id = "";
                objCompany = ServiceObjectCompany.GetWhsIdDetails(objCompany);
                objOutboundShipRpt.ListwhsPickDtl = objCompany.ListwhsPickDtl;
                Mapper.CreateMap<OutboundShipRpt, OutboundShipRptModel>();
                OutboundShipRptModel objOutboundShipRptModel = Mapper.Map<OutboundShipRpt, OutboundShipRptModel>(objOutboundShipRpt);
                return View(objOutboundShipRptModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ActionResult OutboundShipRptInqDetail(string p_str_cmp_id, string p_str_whs_id, string p_str_cust_po, string p_str_ship_dt_fm, string p_str_ship_dt_to, string p_str_BoL, string p_str_Carrier, string p_str_Style, string p_str_Color, string p_str_Size, string p_str_status)
        {
            try
            {
                OutboundShipRpt objOutboundShipRpt = new OutboundShipRpt();
                OutboundShipRptService ServiceObject = new OutboundShipRptService();             
                objOutboundShipRpt.cmp_id = p_str_cmp_id.Trim();
                objOutboundShipRpt.whs_id = p_str_whs_id.Trim();
                objOutboundShipRpt.cust_ord = p_str_cust_po.Trim();
                objOutboundShipRpt.ship_dt_fm = p_str_ship_dt_fm.Trim();
                objOutboundShipRpt.ship_dt_to = p_str_ship_dt_to.Trim();
                objOutboundShipRpt.ship_doc_id = p_str_BoL.Trim();
                objOutboundShipRpt.ship_via_name = p_str_Carrier.Trim();
                objOutboundShipRpt.itm_num = p_str_Style.Trim();
                objOutboundShipRpt.itm_color = p_str_Color.Trim();
                objOutboundShipRpt.itm_size = p_str_Size.Trim();
                objOutboundShipRpt.Status = p_str_status.Trim();
                objOutboundShipRpt = ServiceObject.GetOutboundShipRptInquiryDetails(objOutboundShipRpt);
                for (int i = 0; i < objOutboundShipRpt.LstOutboundShipRptInqdetail.Count(); i++)
                {
                    TotalCtn= TotalCtn + objOutboundShipRpt.LstOutboundShipRptInqdetail[i].Ctns;
                    objOutboundShipRpt.TotalCtn = TotalCtn;
                    TotalQty = TotalQty + objOutboundShipRpt.LstOutboundShipRptInqdetail[i].Pcs;
                    objOutboundShipRpt.TotalQty = TotalQty;
                }

                Mapper.CreateMap<OutboundShipInq, OutboundShipInqModel>();
                OutboundShipRptModel objOutboundShipRptModel = Mapper.Map<OutboundShipRpt, OutboundShipRptModel>(objOutboundShipRpt);
                return PartialView("_OutboundShipRpt", objOutboundShipRptModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public ActionResult EmailShowReport(string var_name, string p_str_cmp_id, string p_str_whs_id, string p_str_cust_po, string p_str_ship_dt_fm, string p_str_ship_dt_to, string p_str_BoL, string p_str_Carrier, string p_str_Style, string p_str_Color, string p_str_Size, string type)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("mm/dd/yyyy");
            string strToDate = DateTime.Now.ToString("mm/dd/yyyy");
            string l_str_rpt_selection = string.Empty;
            string reportFileName = string.Empty;
            objCustMaster.cust_id = p_str_cmp_id;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }
            l_str_rpt_selection = var_name;
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "OutbndDate" || l_str_rpt_selection == "OutbndStyle")
                    {
                        if (l_str_rpt_selection == "OutbndDate")
                        {
                            strReportName = "rpt_iv_ship_dt.rpt";
                        }
                        else
                        {
                            strReportName = "rpt_iv_ship_itm.rpt";
                        }
                        OutboundShipRpt objOutboundShipRpt = new OutboundShipRpt();
                        OutboundShipRptService ServiceObject = new OutboundShipRptService();
                     
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundShipRpt.cmp_id = p_str_cmp_id;
                        objOutboundShipRpt.whs_id = p_str_whs_id;
                        objOutboundShipRpt.cust_ord = p_str_cust_po;
                        objOutboundShipRpt.ship_dt_fm = p_str_ship_dt_fm;
                        objOutboundShipRpt.ship_dt_to = p_str_ship_dt_to;
                        objOutboundShipRpt.ship_doc_id = p_str_BoL;
                        objOutboundShipRpt.ship_via_name = p_str_Carrier;
                        objOutboundShipRpt.itm_num = p_str_Style;
                        objOutboundShipRpt.itm_color = p_str_Color;
                        objOutboundShipRpt.itm_size = p_str_Size;
                        var rpt_fm_dt = "Begining";
                        var rpt_to_dt = "Upto Date";
                        if (l_str_rpt_selection == "OutbndDate")
                        {
                            objOutboundShipRpt = ServiceObject.GetOutboundShipRptbydateDetails(objOutboundShipRpt);

                        }
                        else
                        {
                            objOutboundShipRpt = ServiceObject.GetOutboundShipRptbystyleDetails(objOutboundShipRpt);
                        }

                        if (type == "PDF")
                        {
                            string strDateFormat = string.Empty;
                            string strFileName = string.Empty;
                            var rptSource = objOutboundShipRpt.LstOutboundShipRptbyDatedetail.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objOutboundShipRpt.LstOutboundShipRptbyDatedetail.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    if (p_str_ship_dt_fm == "")
                                    {
                                        rd.SetParameterValue("fml_fm_dt", rpt_fm_dt);
                                    }
                                    else
                                    {
                                        rd.SetParameterValue("fml_fm_dt", p_str_ship_dt_fm);
                                    }
                                    if (p_str_ship_dt_to == "")
                                    {
                                        rd.SetParameterValue("fml_to_dt", rpt_to_dt);
                                    }
                                    else
                                    {
                                        rd.SetParameterValue("fml_to_dt", p_str_ship_dt_to);
                                    }
                                    objOutboundShipRpt.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0430_001 
                                    rd.SetParameterValue("fml_image_path", objOutboundShipRpt.Image_Path);
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//IV_OUTSHIP_RPT" + strDateFormat + ".pdf";

                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                }
                            }
                            reportFileName = "SHIPPING_REPORTS_" + DateTime.Now.ToFileTime() + ".pdf";
                            Session["RptFileName"] = strFileName;

                        }
                       
                    }
                }
                else
                {
                    Response.Write("<H2>Report not found</H2>");
                }
                Email objEmail = new Email();
                objEmail.CmpId = p_str_cmp_id;
                objEmail.EmailSubject = "IV_OUTSHIP_RPT";
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

        public ActionResult ShowReport(string var_name, string p_str_cmp_id, string p_str_whs_id, string p_str_cust_po, string p_str_ship_dt_fm, string p_str_ship_dt_to, string p_str_BoL, string p_str_Carrier, string p_str_Style, string p_str_Color, string p_str_Size , string type)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("mm/dd/yyyy");
            string strToDate = DateTime.Now.ToString("mm/dd/yyyy");
            string l_str_rpt_selection = string.Empty;
            objCustMaster.cust_id = p_str_cmp_id;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }
            l_str_rpt_selection = var_name;
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "OutbndDate" || l_str_rpt_selection == "OutbndStyle")
                    {
                        if (l_str_rpt_selection == "OutbndDate")
                        {
                            strReportName = "rpt_iv_ship_dt.rpt";
                        }
                        else
                        {
                            strReportName = "rpt_iv_ship_itm.rpt";
                        }
                        OutboundShipRpt objOutboundShipRpt = new OutboundShipRpt();
                        OutboundShipRptService ServiceObject = new OutboundShipRptService();

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundShipRpt.cmp_id = p_str_cmp_id;
                        objOutboundShipRpt.whs_id = p_str_whs_id;
                        objOutboundShipRpt.cust_ord = p_str_cust_po;
                        objOutboundShipRpt.ship_dt_fm = p_str_ship_dt_fm;
                        objOutboundShipRpt.ship_dt_to = p_str_ship_dt_to;
                        objOutboundShipRpt.ship_doc_id =p_str_BoL;
                        objOutboundShipRpt.ship_via_name = p_str_Carrier;
                        objOutboundShipRpt.itm_num = p_str_Style;
                        objOutboundShipRpt.itm_color = p_str_Color;
                        objOutboundShipRpt.itm_size = p_str_Size;  
                        var rpt_fm_dt = "Begining";
                        var rpt_to_dt = "Upto Date";
                        if (l_str_rpt_selection == "OutbndDate")
                        {
                            objOutboundShipRpt = ServiceObject.GetOutboundShipRptbydateDetails(objOutboundShipRpt);

                        }
                        else
                        {
                            objOutboundShipRpt = ServiceObject.GetOutboundShipRptbystyleDetails(objOutboundShipRpt);
                        }
                       
                        if (type == "PDF")
                        {
                            var rptSource = objOutboundShipRpt.LstOutboundShipRptbyDatedetail.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objOutboundShipRpt.LstOutboundShipRptbyDatedetail.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    if (p_str_ship_dt_fm == "")
                                    {
                                        rd.SetParameterValue("fml_fm_dt", rpt_fm_dt);
                                    }
                                    else
                                    {
                                        rd.SetParameterValue("fml_fm_dt", p_str_ship_dt_fm);
                                    }
                                    if (p_str_ship_dt_to == "")
                                    {
                                        rd.SetParameterValue("fml_to_dt", rpt_to_dt);
                                    }
                                    else
                                    {
                                        rd.SetParameterValue("fml_to_dt", p_str_ship_dt_to);
                                    }
                                    objOutboundShipRpt.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0430_001 
                                    rd.SetParameterValue("fml_image_path", objOutboundShipRpt.Image_Path);

                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "ShippingReport");
                                }
                            }
                        }
                       
                        else
                        if (type == "Excel")
                        {
                            if (l_str_rpt_selection == "OutbndDate")
                            {
                            List<OB_SHIP_RPT_Excel> li = new List<OB_SHIP_RPT_Excel>();
                            for (int i = 0; i < objOutboundShipRpt.LstOutboundShipRptbyDatedetail.Count; i++)
                            {
                               
                                    OB_SHIP_RPT_Excel objOBInquiryExcel = new OB_SHIP_RPT_Excel();
                                    objOBInquiryExcel.CmpId = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].cmp_id;
                                    objOBInquiryExcel.CustomerName = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].ship_to_name;
                                    objOBInquiryExcel.SRNumber = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].so_num;
                                    objOBInquiryExcel.Bol = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].ship_doc_id;
                                    objOBInquiryExcel.WhsId = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].whs_id;
                                    objOBInquiryExcel.ShipDate = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].ship_dt;
                                    objOBInquiryExcel.RefNum = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].ref_num;

                                    objOBInquiryExcel.CustPO = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].note;
                                    objOBInquiryExcel.Style = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].itm_num;
                                    objOBInquiryExcel.Color = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].itm_color;
                                    objOBInquiryExcel.Size = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].itm_size;
                                    objOBInquiryExcel.Ctns = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].ctn_cnt;
                                    objOBInquiryExcel.Pcs = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].line_qty;
                                    //objOBInquiryExcel.ship_qty = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].ship_qty;
                                    objOBInquiryExcel.CustPO = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].cust_ordr_num;
                                    objOBInquiryExcel.Cube = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].ctn_cube;
                                    objOBInquiryExcel.Weight = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].ctn_wgt;


                                    li.Add(objOBInquiryExcel);
                                }
                                GridView gv = new GridView();
                                gv.DataSource = li;
                                gv.DataBind();
                                Session["OB_SHIP_BYDATE"] = gv;

                                return new DownloadFileActionResult((GridView)Session["OB_SHIP_BYDATE"], "OB_SHIP_BYDATE" + "" + DateTime.Now.ToString("yyyyMMddHHssmm") + ".xls");
                            }
                            else                                
                            {
                                List<OB_SHIP_RPT_STYLE_Excel> li = new List<OB_SHIP_RPT_STYLE_Excel>();
                                for (int i = 0; i < objOutboundShipRpt.LstOutboundShipRptbyDatedetail.Count; i++)
                                {
                                    OB_SHIP_RPT_STYLE_Excel objOBInquiryStyleExcel = new OB_SHIP_RPT_STYLE_Excel();
                                    objOBInquiryStyleExcel.CmpId = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].cmp_id;
                                    objOBInquiryStyleExcel.WhsId = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].whs_id;
                                    objOBInquiryStyleExcel.ShipDate = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].ship_dt;
                                    objOBInquiryStyleExcel.BOL = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].ship_doc_id;
                                    objOBInquiryStyleExcel.SRNumber = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].so_num;
                                    objOBInquiryStyleExcel.CustomerName = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].ship_to_name;
                                    objOBInquiryStyleExcel.RefNum = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].ref_num;
                      
                                    objOBInquiryStyleExcel.StyleName = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].itm_name;
                                    objOBInquiryStyleExcel.Style = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].itm_num;
                                    objOBInquiryStyleExcel.Color = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].itm_color;
                                    objOBInquiryStyleExcel.Size = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].itm_size;
                                    objOBInquiryStyleExcel.Ctns = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].ctn_cnt;
                                    objOBInquiryStyleExcel.Pcs = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].line_qty;

                                    objOBInquiryStyleExcel.Cube = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].ctn_cube;
                                    objOBInquiryStyleExcel.Weight = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].ctn_wgt;

                                    //objOBInquiryStyleExcel.ship_qty = objOutboundShipRpt.LstOutboundShipRptbyDatedetail[i].ship_qty;//CR - 3PL_MVC-IB-20180405
                                    li.Add(objOBInquiryStyleExcel);
                                }
                                GridView gv = new GridView();
                                gv.DataSource = li;
                                gv.DataBind();
                                Session["OB_SHIP_BYSTYLE"] = gv;
                                return new DownloadFileActionResult((GridView)Session["OB_SHIP_BYSTYLE"], "OB_SHIP_BYSTYLE" + "" + DateTime.Now.ToString("yyyyMMddHHssmm") + ".xls");
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
        public ActionResult RefreshReport()
        {
            return PartialView("_ReportsDisplay");
        }
        public ActionResult ReportView()
        {
            return View();
        }
        public JsonResult ItemXGetitmDtl(string term, string cmp_id)
        {
            OutboundShipRptService ServiceObject = new OutboundShipRptService();
            var List = ServiceObject.ItemXGetitmDetails(term, cmp_id).LstItmxCustdtl.Select(x => new { label = x.Itmdtl, value = x.itm_num, itm_num = x.itm_num, itm_color = x.itm_color, itm_size = x.itm_size, itm_name = x.itm_name }).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
    }
  }
