using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GsEPWv8_4_MVC.Business.Implementation;
using GsEPWv8_4_MVC.Business.Interface;
using GsEPWv8_4_MVC.Core.Entity;
using GsEPWv8_4_MVC.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Reflection;
//CR#                               NAME    Date        Desc
//CR_3PL_MVC_OB_2018_0220_001      NITHYA  2018-02-20  To Implement the Allocation Edit  Outbound module
//CR-3PL_MVC-OB-20180405-001 Added By Nithya To Implement the Aloc Unpost  Outbound module
#region Change History
// CR_3PL_MVC_OB_2018_0227_001 - Modified by Soniya for set default from date before one year  in filter section
//CR2018-03-08-001 Added By Nithya for Email Function
#endregion Change History
namespace GsEPWv8_4_MVC.Controllers
{
    public class OutboundInqController : Controller
    {
        string Feedbackresult = string.Empty;
        decimal ordrcost = 0;
        string Status = string.Empty;
        string Whsid = string.Empty;
        string l_str_ITM_NUM = string.Empty;
        string l_str_ITM_COLOR = string.Empty;
        string l_str_ITM_SIZE = string.Empty;
        string l_str_WHSID = string.Empty;
        string l_str_ITM_CODE = string.Empty;
        string l_str_LOCID = string.Empty;
        int l_str_PKGQTY = 0;
        string l_str_PONUM = string.Empty;
        string l_str_PALETID = string.Empty;
        int Avail = 0;
        int Order_Qty = 0;
        int AvailQty = 0;
        int BackOrder_Qty = 0;
        int Alloc_pkg, grddtl_AlocdQty, Allocation_Line_Num = 0;
        string AlocLine = string.Empty;
        int PkgQty, AllocatedQty = 0;
        int BackOrder_Pkg = 0;
        string strAlocSortStmt = string.Empty;
        string strAlocBy = string.Empty;
        int Aloc_line, ctn_line = 0;
        int Aloc_ctn_qty = 0;
        string l_str_RCVDDT = string.Empty;
        int linenum = 0;
        int dueline = 0;
        string custid = string.Empty;
        string l_str_LOTID = string.Empty;
        int SelCtnCnt, SelQty, ToAlocQty, BalQty, ReqQty, grdAlocQty = 0;
        decimal l_str_AVAILQTY = 0;
        int l_str_dueqty = 0;
        int l_str_AlocQty = 0;
        string l_str_bol_num = string.Empty;
        string GSoNum = string.Empty;
        string l_int_dtlLine = string.Empty;
        string l_int_dueLine = string.Empty;
        string l_str_dflt_dt_reqd = string.Empty;       // CR_3PL_MVC_Vas_2018_0324_001 - Added by Soniya
        int SelAlocated = 0;       // CR_3PL_MVC_Vas_2018_0324_001 - Added by Soniya
        public string EmailSub = string.Empty;
        public string EmailMsg = string.Empty;
        public string Folderpath = string.Empty;
        public string l_str_rptdtl = string.Empty;
        public string l_str_tmp_name = string.Empty;
        public string ScreenID = "Outbound Inquiry";
        Email objEmail = new Email();
        EmailService objEmailService = new EmailService();
        OutboundShipInq objOutboundShipInq = new OutboundShipInq();
        OutboundShipInqService OutboundShipInqServiceObject = new OutboundShipInqService();
        
        DataTable dtOutInq;
        [HttpGet]
        public ActionResult OutboundInq(string FullFillType, string cmp, string status, string DateFm, string DateTo, string screentitle)//CR_MVC_3PL_0317-001 Added By Nithya
        {

            string l_str_cmp_id = string.Empty;
            string l_str_fm_dt = string.Empty;
            string l_str_Dflt_Dt_Reqd = string.Empty;
            string l_str_is_3rd_usr = string.Empty;
            try
            {
                OutboundInq objOutboundInq = new OutboundInq();
                OutboundInqService ServiceObject = new OutboundInqService();
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                l_str_is_3rd_usr = Session["IS3RDUSER"].ToString();
                objOutboundInq.IS3RDUSER = l_str_is_3rd_usr.Trim();
                Session["g_str_Search_flag"] = "False";
                Session["GridInvoiceList"] = null;
                Session["GridInvoice"] = null;
                objOutboundInq.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                if (objOutboundInq.cmp_id == null || objOutboundInq.cmp_id == string.Empty)
                {
                    objOutboundInq.cmp_id = Session["g_str_cmp_id"].ToString().Trim();

                }
                else
                {
                    objCompany.cmp_id = Session["g_str_cmp_id"].ToString().Trim();

                }
                l_str_Dflt_Dt_Reqd = Session["DFLT_DT_REQD"].ToString().Trim();
                if (l_str_Dflt_Dt_Reqd == "N")
                {
                    DateFm = "";
                    DateTo = "";
                }
                objOutboundInq.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                objOutboundInq.screentitle = screentitle;//CR_MVC_3PL_0317-001 Added By Nithya
                Session["screentitle"] = objOutboundInq.screentitle;//CR_MVC_3PL_0317-001 Added By Nithya
                Session["l_bool_edit_flag"] = false;
                Session["lblAlocated"] = 0;
                objOutboundInq.l_bool_edit_flag = Session["l_bool_edit_flag"].ToString();
                ServiceObject.DeleteTempshipEntrytable(objOutboundInq);
                ServiceObject.DeleteAUTOALOC(objOutboundInq);

                if (FullFillType == null)
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objOutboundInq.user_id = objCompany.user_id;
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objOutboundInq.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    objOutboundInq.screentitle = screentitle;//CR_MVC_3PL_0317-001 Added By Nithya
                    DateTime date = DateTime.Now.AddMonths(-12);
                    l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");      // CR_3PL_MVC_OB_2018_0227_001 - Modified by Soniya
                    objOutboundInq.so_dtFm = l_str_fm_dt;
                    objOutboundInq.so_dtTo = DateTime.Now.ToString("MM/dd/yyyy");
                    LookUp objLookUp = new LookUp();
                    LookUpService ServiceObject1 = new LookUpService();
                    objLookUp.id = "2";
                    objLookUp.lookuptype = "OUTBOUNDINQUIRY";
                    objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                    objOutboundInq.ListLookUpDtl = objLookUp.ListLookUpDtl;
                    objOutboundInq = ServiceObject.GetOutboundInq(objOutboundInq);
                }

                else if (FullFillType != null)
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objOutboundInq.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    objOutboundInq.cmp_id = cmp;
                    objOutboundInq.so_numFm = "";
                    objOutboundInq.so_numTo = "";
                    objOutboundInq.screentitle = screentitle;//CR_MVC_3PL_0317-001 Added By Nithya
                    objOutboundInq.so_dtFm = DateTime.Now.AddDays(Common.clsGlobal.DispDateFrom).ToString("MM/dd/yyyy"); 
                    objOutboundInq.so_dtTo = DateTime.Now.ToString("MM/dd/yyyy");
                    objOutboundInq.cust_ordr_num = "";
                    objOutboundInq.aloc_doc_id = "";
                    objOutboundInq.store_id = "";
                    objOutboundInq.quote_num = "";
                    objOutboundInq.ShipdtFm = "";
                    objOutboundInq.ShipdtTo = "";
                    status.Trim();
                    if (status == "ALOC")
                    {
                        objOutboundInq.status = "ALOC";
                        LookUp objLookUp = new LookUp();
                        LookUpService ServiceObject1 = new LookUpService();
                        objLookUp.id = "2";
                        objLookUp.lookuptype = "OBALOC";
                        objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                        objOutboundInq.ListLookUpDtl = objLookUp.ListLookUpDtl;
                    }
                    else if (status == "OPEN")
                    {
                        objOutboundInq.status = "OPEN";
                        LookUp objLookUp = new LookUp();
                        LookUpService ServiceObject1 = new LookUpService();
                        objLookUp.id = "2";
                        objLookUp.lookuptype = "OBOPEN";
                        objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                        objOutboundInq.ListLookUpDtl = objLookUp.ListLookUpDtl;
                    }
                    else
                    {
                        objOutboundInq.status = "ALL";
                        LookUp objLookUp = new LookUp();
                        LookUpService ServiceObject1 = new LookUpService();
                        objLookUp.id = "2";
                        objLookUp.lookuptype = "OUTBOUNDINQUIRY";
                        objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                        objOutboundInq.ListLookUpDtl = objLookUp.ListLookUpDtl;
                    }
                    if (l_str_Dflt_Dt_Reqd == "Y")
                    {
                        objOutboundInq = ServiceObject.GetOutboundInq(objOutboundInq);
                    }
                    objCompany.cmp_id = cmp;
                    objCompany = ServiceObjectCompany.GetFullFillCompanyDetails(objCompany);
                    //objOutboundInq.ListCompanyPickDtl = objCompany.ListFullFillCompanyPickDtl;
                    if (status == "ALOC")
                    {
                        objOutboundInq.status = "ALOC";
                        LookUp objLookUp = new LookUp();
                        LookUpService ServiceObject1 = new LookUpService();
                        objLookUp.id = "2";
                        objLookUp.lookuptype = "OBALOC";
                        objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                        objOutboundInq.ListLookUpDtl = objLookUp.ListLookUpDtl;
                    }
                    else if (status == "OPEN")
                    {
                        objOutboundInq.status = "OPEN";
                        LookUp objLookUp = new LookUp();
                        LookUpService ServiceObject1 = new LookUpService();
                        objLookUp.id = "2";
                        objLookUp.lookuptype = "OBOPEN";
                        objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                        objOutboundInq.ListLookUpDtl = objLookUp.ListLookUpDtl;
                    }
                    else
                    {
                        objOutboundInq.status = "ALL";
                        LookUp objLookUp = new LookUp();
                        LookUpService ServiceObject1 = new LookUpService();
                        objLookUp.id = "2";
                        objLookUp.lookuptype = "OUTBOUNDINQUIRY";
                        objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                        objOutboundInq.ListLookUpDtl = objLookUp.ListLookUpDtl;
                    }
                    //objOutboundInq.so_dtFm = DateFm;
                    //objOutboundInq.so_dtTo = DateTime.Now.ToString("MM/dd/yyyy");
                }

                Mapper.CreateMap<OutboundInq, OutboundInqModel>();
                OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
                return View(objOutboundInqModel);
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
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            string l_str_tmp_cmp_id = string.Empty;
            Session["g_str_cmp_id"] = p_str_cmp_id;// CR_3PL_MVC_COMMON_2018_0326_001
            l_str_tmp_cmp_id = Session["g_str_cmp_id"].ToString().Trim();
            return Json(l_str_tmp_cmp_id, JsonRequestBehavior.AllowGet);
        }
        public JsonResult OB_INQ_HDR_DATA(string p_str_cmp_id, string p_str_so_num_frm, string p_str_so_num_To, string p_str_so_dt_frm, string p_str_so_dt_to, string p_str_CustPO, string p_str_AlocId, string p_str_Store,
            string p_str_batch_id, string p_str_shipdtFm, string p_str_shipdtTo, string p_str_status, string p_str_cust_name, string p_str_screen_title, string p_str_Style, string p_str_color, string p_str_size)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            Session["g_str_cmp_id"] = p_str_cmp_id.Trim();
            Session["TEMP_CMP_ID"] = p_str_cmp_id.Trim();
            Session["TEMP_IB_so_num_frm"] = p_str_so_num_frm.Trim();
            Session["TEMP_IB_so_num_To"] = p_str_so_num_To.Trim();
            Session["TEMP_so_dt_frm"] = p_str_so_dt_frm.Trim();
            Session["TEMP_so_dt_to"] = p_str_so_dt_to.Trim();
            Session["TEMP_CustPO"] = p_str_CustPO.Trim();
            Session["TEMP_AlocId"] = p_str_AlocId.Trim();
            Session["TEMP_Store"] = p_str_Store.Trim();
            Session["TEMP_batch_id"] = p_str_batch_id.Trim();
            Session["TEMP_shipdtFm"] = p_str_shipdtFm.Trim();
            Session["TEMP_shipdtTo"] = p_str_shipdtTo.Trim();
            Session["TEMP_STATUS"] = p_str_status.Trim();
            Session["TEMP_cust_name"] = p_str_cust_name.Trim();
            Session["TEMP_SCREEN_ID"] = p_str_screen_title.Trim();
            Session["TEMP_STYLE"] = p_str_Style.Trim();
            Session["TEMP_COLOR"] = p_str_color.Trim();
            Session["TEMP_SIZE"] = p_str_size.Trim();

            //objInboundInquiry = objService.GetInboundInquiryDetails(objInboundInquiry);
            //objInboundInquiry.DocEntryCount = objInboundInquiry.ListGetDocEntryCount[0].DocCount;
            return Json(objOutboundInq, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetOutboundInqDtl(string p_str_cmp_id, string p_str_so_num, string p_str_AlocdocId)
        {
            try
            {
                OutboundInq objOutboundInq = new OutboundInq();
                OutboundInqService ServiceObject = new OutboundInqService();

                string l_str_search_flag = string.Empty;
                string l_str_is_3rd_usr = string.Empty;
                l_str_is_3rd_usr = Session["IS3RDUSER"].ToString();
                objOutboundInq.IS3RDUSER = l_str_is_3rd_usr.Trim();
                l_str_search_flag = Session["g_str_Search_flag"].ToString().Trim();
                objOutboundInq.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                if (l_str_search_flag == "True")
                {
                    objOutboundInq.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                    objOutboundInq.cmp_id = p_str_cmp_id.Trim();
                    objOutboundInq.so_numFm = Session["TEMP_IB_so_num_frm"].ToString().Trim();
                    objOutboundInq.so_numTo = Session["TEMP_IB_so_num_To"].ToString().Trim();
                    objOutboundInq.so_dtFm = Session["TEMP_so_dt_frm"].ToString().Trim();
                    objOutboundInq.so_dtTo = Session["TEMP_so_dt_to"].ToString().Trim();
                    objOutboundInq.cust_ordr_num = Session["TEMP_CustPO"].ToString().Trim();
                    objOutboundInq.aloc_doc_id = Session["TEMP_AlocId"].ToString().Trim();
                    objOutboundInq.store_id = Session["TEMP_Store"].ToString().Trim();
                    objOutboundInq.quote_num = Session["TEMP_batch_id"].ToString().Trim();
                    objOutboundInq.ShipdtFm = Session["TEMP_shipdtFm"].ToString().Trim();
                    objOutboundInq.ShipdtTo = Session["TEMP_shipdtTo"].ToString().Trim();
                    objOutboundInq.status = Session["TEMP_STATUS"].ToString().Trim();
                    objOutboundInq.cust_name = Session["TEMP_cust_name"].ToString().Trim();
                    objOutboundInq.screentitle = Session["TEMP_SCREEN_ID"].ToString().Trim();
                    objOutboundInq.itm_num = Session["TEMP_STYLE"].ToString().Trim();//CR_MVC_3PL_20180605-001 Added By NIthya
                    objOutboundInq.itm_color = Session["TEMP_COLOR"].ToString().Trim();
                    objOutboundInq.itm_size = Session["TEMP_SIZE"].ToString().Trim();                    
                }
                else
                {
                    objOutboundInq.cmp_id = p_str_cmp_id.Trim();
                    objOutboundInq.so_numFm = p_str_so_num.Trim();
                    objOutboundInq.so_numTo = p_str_so_num.Trim();
                    objOutboundInq.aloc_doc_id = p_str_AlocdocId;
                    objOutboundInq.screentitle = Session["TEMP_SCREEN_ID"].ToString().Trim();
                }

                objOutboundInq = ServiceObject.GetOutboundInq(objOutboundInq);
                Mapper.CreateMap<OutboundInq, OutboundInqModel>();
                OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
                return PartialView("_OutboundInq", objOutboundInqModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public ActionResult GetOutboundInqDetail(string p_str_cmp_id, string p_str_so_num_frm, string p_str_so_num_To, string p_str_so_dt_frm, string p_str_so_dt_to, string p_str_CustPO, string p_str_AlocId, string p_str_Store, string p_str_batch_id, string p_str_shipdtFm, string p_str_shipdtTo, string p_str_status, string p_str_cust_name, string p_str_screen_title,
            string p_str_Style,string p_str_color,string p_str_size)
        {
            try
            {
                string l_str_is_3rd_usr = string.Empty;
                OutboundInq objOutboundInq = new OutboundInq();
                OutboundInqService ServiceObject = new OutboundInqService();
                l_str_is_3rd_usr = Session["IS3RDUSER"].ToString();
                objOutboundInq.IS3RDUSER = l_str_is_3rd_usr.Trim();
                objOutboundInq.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                Session["g_str_Search_flag"] = "True";
                objOutboundInq.screentitle = p_str_screen_title;//CR_MVC_3PL_20180411-001 Added By NIthya
                objOutboundInq.cmp_id = p_str_cmp_id.Trim();
                objOutboundInq.so_numFm = p_str_so_num_frm.Trim();
                objOutboundInq.so_numTo = p_str_so_num_To.Trim();
                objOutboundInq.so_dtFm = p_str_so_dt_frm.Trim();
                objOutboundInq.so_dtTo = p_str_so_dt_to.Trim();
                objOutboundInq.cust_ordr_num = p_str_CustPO.Trim();
                objOutboundInq.aloc_doc_id = p_str_AlocId.Trim();
                objOutboundInq.store_id = p_str_Store.Trim();
                objOutboundInq.quote_num = p_str_batch_id.Trim();
                objOutboundInq.ShipdtFm = p_str_shipdtFm.Trim();
                objOutboundInq.ShipdtTo = p_str_shipdtTo.Trim();
                objOutboundInq.status = p_str_status.Trim();
                objOutboundInq.cust_name = p_str_cust_name.Trim();
                //CR_MVC_3PL_20180605-001 Added By NIthya
                objOutboundInq.itm_num = p_str_Style.Trim();
                objOutboundInq.itm_color = p_str_color.Trim();
                objOutboundInq.itm_size = p_str_size.Trim();
                //End
                objOutboundInq = ServiceObject.GetOutboundInq(objOutboundInq);
                Mapper.CreateMap<OutboundInq, OutboundInqModel>();
                OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
                return PartialView("_OutboundInq", objOutboundInqModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public ActionResult GetShipReqRptDtls(string SelectedIDs, string p_str_cmpid, string p_str_radio, string p_str_so_num_frm, string p_str_so_num_To, string p_str_so_dt_frm, string p_str_so_dt_to, string p_str_CustPO, string p_str_AlocId, string p_str_Store, string p_str_batch_id, string p_str_status, string p_str_shipdtFm, string p_str_shipdtTo)
        {
            try
            {
                OutboundInq objOutboundInq = new OutboundInq();
                OutboundInqService ServiceObject = new OutboundInqService();
                objOutboundInq.cmp_id = p_str_cmpid;
                objOutboundInq.Sonum = SelectedIDs;
                //  objOrderLifeCycle = ServiceObject.GetEShipGetTrackId(objEShip);
                TempData["sonum"] = SelectedIDs;
                TempData["aloc_doc_id"] = p_str_AlocId;
                TempData["ReportSelection"] = p_str_radio;
                TempData["so_NumFm"] = p_str_so_num_frm;
                TempData["so_NumTo"] = p_str_so_num_To;
                TempData["so_DtFm"] = p_str_so_dt_frm;
                TempData["so_DtTo"] = p_str_so_dt_to;
                TempData["CustPo"] = p_str_CustPO;
                TempData["status"] = p_str_status;
                TempData["storeid"] = p_str_Store;
                TempData["batchId"] = p_str_batch_id;
                TempData["ShipdtFm"] = p_str_shipdtFm;
                TempData["ShipdtTo"] = p_str_shipdtTo;
                TempData["cmp_id"] = p_str_cmpid;
                return Json(SelectedIDs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ActionResult GetShipReqRpt(string SelectedID, string p_str_cmpid, string p_str_radio, string p_str_so_num_frm, string p_str_so_num_To, string p_str_batch_id)
        {
            try
            {
                int ResultCount = 0;
                string RptResult = string.Empty;
                OutboundInq objOutboundInq = new OutboundInq();
                OutboundInqService ServiceObject = new OutboundInqService();
                objOutboundInq.cmp_id = p_str_cmpid;
                objOutboundInq.Sonum = SelectedID;
                //  objOrderLifeCycle = ServiceObject.GetEShipGetTrackId(objEShip);
                TempData["sonum"] = SelectedID;
                TempData["ReportSelection"] = p_str_radio;
                TempData["so_NumFm"] = p_str_so_num_frm;
                TempData["so_NumTo"] = p_str_so_num_To;
                TempData["batchId"] = p_str_batch_id;
                TempData["cmp_id"] = p_str_cmpid;
                objOutboundInq.cmp_id = p_str_cmpid;
                objOutboundInq.so_numFm = SelectedID;
                objOutboundInq.so_numTo = SelectedID;
                objOutboundInq.quote_num = p_str_batch_id;
                if (p_str_radio == "PickTickByStyle")
                {
                    objOutboundInq = ServiceObject.IsOpenAlocationExists(objOutboundInq);
                    if (objOutboundInq.LstStockverify.Count > 0)
                    {
                        objOutboundInq.TotRecs = objOutboundInq.LstStockverify[0].TotRecs;
                        ResultCount = 1;
                    }
                    else
                    {
                        objOutboundInq.TotRecs = 0;
                    }
                    if (objOutboundInq.TotRecs == 0)
                    {
                         ResultCount = 0;                        
                    }
                }
                else
                {
                    objOutboundInq = ServiceObject.IsOpenAlocationExists(objOutboundInq);
                    if (objOutboundInq.LstStockverify.Count > 0)
                    {
                        objOutboundInq.TotRecs = objOutboundInq.LstStockverify[0].TotRecs;
                        ResultCount = 1;
                    }
                    else
                    {
                        objOutboundInq.TotRecs = 0;
                    }
                    if (objOutboundInq.TotRecs == 0)
                    {
                         ResultCount = 0;
                        
                    }
                }
                return Json(ResultCount, JsonRequestBehavior.AllowGet);
                //if (objOutboundInq.RptResult == "empty")
                //{
                //    SelectedID = "0";
                //    return Json(SelectedID, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //    return Json(SelectedID, JsonRequestBehavior.AllowGet);
                //}
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
        //CR2018-03-08-001 Added By Nithya
        public ActionResult ShowEmailReport(string var_name, string SelectedID, string p_str_cmpid, string p_str_so_num_frm, string p_str_so_num_To, string p_str_so_dt_frm, string p_str_so_dt_to, string p_str_CustPO, string p_str_AlocId, string p_str_Store,
            string p_str_batch_id, string p_str_status, string p_str_shipdtFm, string p_str_shipdtTo, string type, string p_str_Style, string p_str_color, string p_str_size,string p_str_ship_doc_id)
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
            string strDateFormat = string.Empty;
            string strFileName = string.Empty;
            string reportFileName = string.Empty;//CR2018-03-07-001 Added By Nithya
            l_str_rpt_selection = var_name;
            Folderpath= System.Configuration.ConfigurationManager.AppSettings["DefaultFolderPath"].ToString().Trim();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            objCompany.cmp_id = p_str_cmpid;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            l_str_tmp_name = objCompany.LstCmpName[0].cmp_name.ToString().Trim();
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "OutboundSummary")
                    {

                        strReportName = "rpt_iv_ship_req_GridSummary.rpt";

                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmpid.Trim();
                        objOutboundInq.aloc_doc_id = p_str_AlocId.Trim();
                        objOutboundInq.so_numFm = p_str_so_num_frm.Trim();
                        objOutboundInq.so_numTo = p_str_so_num_To.Trim();
                        objOutboundInq.so_dtFm = p_str_so_dt_frm.Trim();
                        objOutboundInq.so_dtTo = p_str_so_dt_to.Trim();
                        objOutboundInq.cust_ordr_num = p_str_CustPO.Trim();
                        objOutboundInq.status = p_str_status.Trim();
                        objOutboundInq.store_id = p_str_Store.Trim();
                        objOutboundInq.quote_num = p_str_batch_id.Trim();
                        objOutboundInq.ShipdtFm = p_str_shipdtFm.Trim();
                        objOutboundInq.ShipdtTo = p_str_shipdtTo.Trim();
                        objOutboundInq.itm_num = p_str_Style.Trim();
                        objOutboundInq.itm_color = p_str_color.Trim();
                        objOutboundInq.itm_size = p_str_size.Trim();//CR_MVC_3PL_20180605-001 Added By NIthya
                        objOutboundInq = ServiceObject.OutboundInqSUmmaryRpt(objOutboundInq);
                        EmailSub = "Outbound ShipReq.Entry GridSummary Report for" + " " + " " + objOutboundInq.cmp_id;
                        EmailMsg = "Outbound ShipReq.Entry GridSummary Report hasbeen Attached for the Process";
                        if (type == "PDF")
                        {
                            var rptSource = objOutboundInq.LstOutboundInqSummaryRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.LstOutboundInqSummaryRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//Outbound_ShipReq.Entry_GridSummary__" + strDateFormat + ".pdf";
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            reportFileName = "Inbound_GridSummary Report_" + strDateFormat + ".pdf";//CR2018-03-07-001 Added By Nithya
                            Session["RptFileName"] = strFileName;
                          
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else
                        if (type == "XLS")
                        {
                            var rptSource = objOutboundInq.LstOutboundInqSummaryRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.LstOutboundInqSummaryRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//Outbound_ShipReq.Entry_GridSummary__" + strDateFormat + ".xls";
                            rd.ExportToDisk(ExportFormatType.ExcelWorkbook, strFileName);
                            reportFileName = "Outbound_GridSummary Report_" + strDateFormat + ".xls";//CR2018-03-07-001 Added By Nithya
                            Session["RptFileName"] = strFileName;
                        }
                    }
                    else if (l_str_rpt_selection == "PickTickByStyle")
                    {
                        string RptResult = string.Empty;
                        strReportName = "rpt_iv_pickslip_consolidated_by_style.rpt";

                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmpid.Trim();
                        objOutboundInq.so_numFm = p_str_so_num_frm.Trim();
                        objOutboundInq.so_numTo = p_str_so_num_To.Trim();
                        objOutboundInq.quote_num = p_str_batch_id.Trim();
                        batchId = objOutboundInq.quote_num;
                        objOutboundInq = ServiceObject.OutboundInqPickStyleRpt(objOutboundInq);
                        EmailSub = "Outbound PickSlip Consolidated by Style Report for" + " " + " " + objOutboundInq.cmp_id;
                        EmailMsg = "Outbound PickSlip Consolidated by Style  Report hasbeen Attached for the Process";
                        if (type == "PDF")
                        {

                            var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                            batchId = "Batch Id :" + batchId + "";
                            string batchIdFm = "Ship Request From :" + p_str_so_num_frm + "  Ship Request To : " + p_str_so_num_To;
                            if (batchId != string.Empty)
                            {
                                if (!string.IsNullOrEmpty(batchId))
                                    rd.SetParameterValue("fml_rep_selectionBy", batchId);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(batchIdFm))
                                    rd.SetParameterValue("fml_rep_selectionBy", batchIdFm);
                            }
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath +"//Outbound_PickTickByStyle__" + strDateFormat + ".pdf";
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            reportFileName = "Outbound PickSlip Consolidated by Style  Report_" + strDateFormat + ".pdf";//CR2018-03-07-001 Added By Nithya
                            Session["RptFileName"] = strFileName;


                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else
                        if (type == "XLS")
                        {
                            var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);

                            batchId = "Batch Id :" + batchId + "";
                            string batchIdFm = "Ship Request From :" + p_str_so_num_frm + "  Ship Request To : " + p_str_so_num_To;
                            if (batchId != string.Empty)
                            {
                                if (!string.IsNullOrEmpty(batchId))
                                    rd.SetParameterValue("fml_rep_selectionBy", batchId);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(batchIdFm))
                                    rd.SetParameterValue("fml_rep_selectionBy", batchIdFm);
                            }
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//Outbound_PickTickByStyle__" + strDateFormat + ".xls";
                            rd.ExportToDisk(ExportFormatType.ExcelWorkbook, strFileName);
                            reportFileName = "Outbound PickSlip Consolidated by Style  Report_" + strDateFormat + ".xls";//CR2018-03-07-001 Added By Nithya
                            Session["RptFileName"] = strFileName;              
                        }
                    }
                    else if (l_str_rpt_selection == "PickTickByLoc")
                    {
                        strReportName = "rpt_iv_pickslip_consolidated_by_loc.rpt";

                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmpid.Trim();
                        objOutboundInq.so_numFm = p_str_so_num_frm.Trim();
                        objOutboundInq.so_numTo = p_str_so_num_To.Trim();
                        objOutboundInq.quote_num = p_str_batch_id.Trim();
                        batchId = objOutboundInq.quote_num;
                        objOutboundInq = ServiceObject.OutboundInqPickLocationRpt(objOutboundInq);
                        EmailSub = "Outbound PickSlip Consolidated by Location Report for" + " " + " " + objOutboundInq.cmp_id;
                        EmailMsg = "Outbound PickSlip Consolidated by Location  Report hasbeen Attached for the Process";
                        if (type == "PDF")
                        {

                            var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                            batchId = "Batch Id :" + batchId + "";
                            string batchIdFm = "Ship Request From :" + p_str_so_num_frm + "  Ship Request To : " + p_str_so_num_To;
                            if (batchId != string.Empty)
                            {
                                if (!string.IsNullOrEmpty(batchId))
                                    rd.SetParameterValue("fml_rep_selectionBy", batchId);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(batchId))
                                    rd.SetParameterValue("fml_rep_selectionBy", batchIdFm);
                            }
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//Outbound_PickTickByLoc__" + strDateFormat + ".pdf";
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            reportFileName = "Outbound PickSlip Consolidated by Location Report_" + strDateFormat + ".pdf";//CR2018-03-07-001 Added By Nithya
                            Session["RptFileName"] = strFileName;
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else
                        if (type == "XLS")
                        {
                            var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            batchId = "Batch Id :" + batchId + "";
                            string batchIdFm = "Ship Request From :" + p_str_so_num_frm + "  Ship Request To : " + p_str_so_num_To;
                            if (batchId != string.Empty)
                            {
                                if (!string.IsNullOrEmpty(batchId))
                                    rd.SetParameterValue("fml_rep_selectionBy", batchId);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(batchId))
                                    rd.SetParameterValue("fml_rep_selectionBy", batchIdFm);
                            }
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//Outbound_PickTickByLoc__" + strDateFormat + ".xls";
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            reportFileName = "Outbound PickSlip Consolidated by Location Report_" + strDateFormat + ".xls";//CR2018-03-07-001 Added By Nithya
                            Session["RptFileName"] = strFileName;
                        }
                    }
                    else if (l_str_rpt_selection == "GridSummary")
                    {
                        strReportName = "rpt_iv_ob_grd_Summary.rpt";

                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmpid.Trim();
                        objOutboundInq.aloc_doc_id = p_str_AlocId.Trim();
                        objOutboundInq.so_numFm = p_str_so_num_frm;
                        objOutboundInq.so_numTo = p_str_so_num_To;
                        objOutboundInq.so_dtFm = p_str_so_dt_frm;
                        objOutboundInq.so_dtTo = p_str_so_dt_to;
                        objOutboundInq.cust_ordr_num = p_str_CustPO;
                        objOutboundInq.status = p_str_status;
                        objOutboundInq.store_id = p_str_Store;
                        objOutboundInq.quote_num = p_str_batch_id;
                        objOutboundInq.ShipdtFm = p_str_shipdtFm;
                        objOutboundInq.ShipdtTo = p_str_shipdtTo;
                        batchId = objOutboundInq.quote_num;
                        objOutboundInq = ServiceObject.OutboundInqGridSummary(objOutboundInq);
                        EmailSub = "Outbound Grid Summary Report for" + " " + " " + objOutboundInq.cmp_id;
                        EmailMsg = "Outbound Grid Summary Report hasbeen Attached for the Process";
                        if (type == "PDF")
                        {
                            var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//Outbound_Grid Summary_" + strDateFormat + ".pdf";
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            reportFileName = "Outbound Grid Summary Report_" + strDateFormat + ".xls";//CR2018-03-07-001 Added By Nithya
                            Session["RptFileName"] = strFileName;
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else
                        if (type == "XLS")
                        {
                            var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//Outbound_Grid Summary_" + strDateFormat + ".xls";
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            reportFileName = "Outbound Grid Summary Report_" + strDateFormat + ".xls";//CR2018-03-07-001 Added By Nithya
                            Session["RptFileName"] = strFileName;
                        }


                    }
                    else if (l_str_rpt_selection == "SR940Summary")
                    {
                        strReportName = "ShipReq940.rpt";
                      
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//EDI//" + strReportName;
                        if (type == "PDF")
                        {
                            objOutboundInq.cmp_id = p_str_cmpid.Trim();
                            objOutboundInq.so_numFm = p_str_so_num_frm.Trim();
                            objOutboundInq.so_numTo = p_str_so_num_To.Trim();
                            objOutboundInq.quote_num = p_str_batch_id.Trim();
                            objOutboundInq = ServiceObject.GetEcomSR940Rpt(objOutboundInq);           
                            var rptSource = objOutboundInq.ListeCom940SRUploadDtlRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.ListeCom940SRUploadDtlRpt.Count();
                            if (objOutboundInq.ListeCom940SRUploadDtlRpt.Count > 0)
                            {
                                objOutboundInq.so_num = (objOutboundInq.ListeCom940SRUploadDtlRpt[0].so_num == null || objOutboundInq.ListeCom940SRUploadDtlRpt[0].so_num.Trim() == "" ? string.Empty : objOutboundInq.ListeCom940SRUploadDtlRpt[0].so_num.Trim());
                                objOutboundInq.ship_dt = objOutboundInq.ListeCom940SRUploadDtlRpt[0].ship_dt;
                                DateTime date = Convert.ToDateTime(objOutboundInq.ship_dt);
                                objOutboundInq.ship_dt = (date.ToString("dd/MM/yyyy"));
                                objOutboundInq.po_num = (objOutboundInq.ListeCom940SRUploadDtlRpt[0].cust_ordr_num == null || objOutboundInq.ListeCom940SRUploadDtlRpt[0].cust_ordr_num.Trim() == "" ? string.Empty : objOutboundInq.ListeCom940SRUploadDtlRpt[0].cust_ordr_num.Trim());
                                objOutboundInq.store_id = (objOutboundInq.ListeCom940SRUploadDtlRpt[0].store_id == null || objOutboundInq.ListeCom940SRUploadDtlRpt[0].store_id.Trim() == "" ? string.Empty : objOutboundInq.ListeCom940SRUploadDtlRpt[0].store_id.Trim());
                                objOutboundInq.CustName = (objOutboundInq.ListeCom940SRUploadDtlRpt[0].cust_name == null || objOutboundInq.ListeCom940SRUploadDtlRpt[0].cust_name.Trim() == "" ? string.Empty : objOutboundInq.ListeCom940SRUploadDtlRpt[0].cust_name.Trim());


                                if ((objOutboundInq.po_num == "" || objOutboundInq.po_num == null || objOutboundInq.po_num == "-") && (objOutboundInq.CustName == "" || objOutboundInq.CustName == null || objOutboundInq.CustName == "-") && (objOutboundInq.so_numFm == null || objOutboundInq.so_numFm == "") && (objOutboundInq.so_numTo == null || objOutboundInq.so_numTo == ""))
                                {
                                    l_str_rptdtl = objOutboundInq.cmp_id + "_" + "SR940Summary Report" + "_" + objOutboundInq.so_num + "_" + objOutboundInq.ship_dt;
                                    objEmail.EmailSubject = objOutboundInq.cmp_id + "-" + " " + " " + "SR940Summary Report" + "|" + " " + " " + "SR#: " + " " + objOutboundInq.so_num + "|" + " " + " " + "SR Date : " + objOutboundInq.ship_dt + "|" + " " + " " + "Store ID: " + objOutboundInq.store_id;
                                    objEmail.EmailMessage = "CmpId: " + " " + " " + objOutboundInq.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "SR#: " + " " + " " + objOutboundInq.so_num + "\n" + "SR Date: " + " " + " " + objOutboundInq.ship_dt + "\n" + "StoreID: " + objOutboundInq.store_id;
                                }
                                else if ((objOutboundInq.CustName == "" || objOutboundInq.CustName == null || objOutboundInq.CustName == "-") && (objOutboundInq.so_numFm == null || objOutboundInq.so_numFm == "") && (objOutboundInq.so_numTo == null || objOutboundInq.so_numTo == ""))

                                {
                                    l_str_rptdtl = objOutboundInq.cmp_id + "_" + "SR940Summary Report" + "_" + objOutboundInq.so_num + "_" + objOutboundInq.ship_dt;
                                    objEmail.EmailSubject = objOutboundInq.cmp_id + "-" + " " + " " + "SR940Summary Report" + "|" + " " + " " + "SR#: " + objOutboundInq.so_num + "|" + " " + " " + "SR Date: " + objOutboundInq.ship_dt + "|" + " " + " " + "Cust PO#: " + objOutboundInq.po_num + "|" + " " + " " + "Store ID: " + objOutboundInq.store_id;
                                    objEmail.EmailMessage = "CmpId: " + " " + " " + objOutboundInq.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "SR#: " + " " + " " + objOutboundInq.so_num + "\n" + "SR Date: " + " " + " " + objOutboundInq.ship_dt + "\n" + "Cust PO#: " + objOutboundInq.po_num + "\n" + "StoreID: " + objOutboundInq.store_id;

                                }
                                else if ((objOutboundInq.so_numFm == null || objOutboundInq.so_numFm == "") && (objOutboundInq.so_numTo == null || objOutboundInq.so_numTo == ""))
                                {
                                    l_str_rptdtl = objOutboundInq.cmp_id + "_" + "SR940Summary Report" + "_" + objOutboundInq.so_num + "_" + objOutboundInq.ship_dt;
                                    objEmail.EmailSubject = objOutboundInq.cmp_id + "-" + " " + " " + "SR940Summary Report" + "|" + " " + " " + "SR#: " + objOutboundInq.so_num + "|" + " " + " " + "SR Date: " + objOutboundInq.ship_dt + "|" + " " + " " + "Cust PO#: " + objOutboundInq.po_num + "|" + " " + " " + "Store: " + objOutboundInq.store_id + "|" + " " + " " + "Ref# -Cust Name : " + objOutboundInq.CustName;
                                    objEmail.EmailMessage = "CmpId: " + " " + " " + objOutboundInq.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "SR#: " + " " + " " + objOutboundInq.so_num + "\n" + "SR Date: " + " " + " " + objOutboundInq.ship_dt + "\n" + "Cust PO#: " + objOutboundInq.po_num + "\n" + "StoreID: " + objOutboundInq.store_id + "\n" + "Ref#- " + "\n" + "CustName : " + objOutboundInq.CustName;
                                }
                                else
                                {
                                    l_str_rptdtl = objOutboundInq.cmp_id + "_" + "SR940Summary Report" + "_" + objOutboundInq.so_num + "_" + objOutboundInq.ship_dt;
                                    objEmail.EmailSubject = objOutboundInq.cmp_id + "-" + " " + " " + "SR940Summary Report" + "|" + " " + " " + "SR#: " + objOutboundInq.so_num + "|" + " " + " " + "SR Date: " + objOutboundInq.ship_dt + "|" + " " + " " + "SR# Fm: " + objOutboundInq.so_numFm + "|" + " " + " " + "SR# To: " + objOutboundInq.so_numTo + "|" + " " + " " + "Cust PO#: " + "|" + " " + " " + objOutboundInq.po_num + "|" + " " + " " + "Store: " + objOutboundInq.store_id + "|" + " " + " " + "Ref# -Cust Name : " + objOutboundInq.CustName;
                                    objEmail.EmailMessage = "CmpId: " + " " + " " + objOutboundInq.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "SR#: " + " " + " " + objOutboundInq.so_num + "\n" + "SR Date: " + " " + " " + objOutboundInq.ship_dt + "\n" + "SR# Fm: " + objOutboundInq.so_numFm + "\n" + "SR# To: " + objOutboundInq.so_numTo;

                                }
                            objCompany.cmp_id = p_str_cmpid.Trim();
                            objCompany = ServiceObjectCompany.CompanyAddresHdrDtls(objCompany);
                            objOutboundInq.ListCompanyAddresHdrDtls = objCompany.ListCompanyAddresHdrDtls;
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                            rd.SetParameterValue("fmlReportTitle", "940 Ship Request Upload");
                            rd.DataDefinition.FormulaFields["fmlCompanyName"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].cmp_name.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompAddress"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].addr_line1.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompCity"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].city.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompstate_id"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].state_id.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompPhone"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].tel.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompFax"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].fax.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompPostCode"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].post_code.ToString().Trim() + "'";
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//Ecomm SR940Summary__" + strDateFormat + ".pdf";
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                reportFileName = l_str_rptdtl + strDateFormat + ".pdf";//CR2018-03-07-001 Added By Nithya
                            Session["RptFileName"] = strFileName;
                        }
                    }
                    }
                    else if (l_str_rpt_selection == "BillofLadding")
                    {
                        strReportName = "rpt_iv_bill_of_lading.rpt";
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundShipInq.cmp_id = p_str_cmpid.Trim();
                        objOutboundShipInq.ship_doc_id = p_str_ship_doc_id.Trim();
                        objOutboundShipInq = OutboundShipInqServiceObject.OutboundShipInqBillofLaddingRpt(objOutboundShipInq);
                        if (objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt.Count > 0)
                        {
                            objOutboundShipInq.so_num = (objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].so_num == null || objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].so_num.Trim() == "" ? string.Empty : objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].so_num.Trim());
                            objOutboundShipInq.shipdate = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].Bol_ShipDt.ToString("dd/MM/yyyy"); ;
                            objOutboundShipInq.po_num = (objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].cust_ordr_num == null || objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].cust_ordr_num.Trim() == "" ? string.Empty : objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].cust_ordr_num.Trim());
                            objOutboundShipInq.whs_id = (objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].whs_id == null || objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].whs_id.Trim() == "" ? string.Empty : objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].whs_id.Trim());
                            objOutboundShipInq.ship_to = (objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].ship_to == null || objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].ship_to.Trim() == "" ? string.Empty : objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].ship_to.Trim());
                            objOutboundShipInq = OutboundShipInqServiceObject.GETRptValue(objOutboundShipInq);
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
                            if ((objOutboundShipInq.po_num == "" || objOutboundShipInq.po_num == null || objOutboundShipInq.whs_id == "-") && (objOutboundShipInq.whs_id == "" || objOutboundShipInq.whs_id == null || objOutboundShipInq.whs_id == "-") && (objOutboundShipInq.ship_to == "" || objOutboundShipInq.ship_to == null || objOutboundShipInq.ship_to == "-"))
                            {
                                l_str_rptdtl = objOutboundShipInq.cmp_id + "_" + "BillofLadding " + "_" + objOutboundShipInq.so_num + "_" + objOutboundShipInq.shipdate;
                                objEmail.EmailSubject = objOutboundShipInq.cmp_id + "-" + " " + " " + "BillofLadding" + "|" + " " + " " + "ShipDoc#: " + " " + objOutboundShipInq.so_num + "|" + " " + " " + "Ship Date : " + objOutboundShipInq.shipdate;
                                objEmail.EmailMessage = "CmpId: " + " " + " " + objOutboundShipInq.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "ShipDoc#: " + " " + " " + objOutboundShipInq.so_num + "\n" + "SR Date: " + " " + " " + objOutboundShipInq.shipdate + "\n" + "StoreID: " + objOutboundShipInq.Bol_ShipDt + "\n" + "Ref#- " + "\n" + "Total Cartons Requested: " + objOutboundShipInq.TotCtns + " " + "Ctns" + "\n" + " Total Cube: " + objOutboundShipInq.TotCube + " " + "Lbs";
                            }

                            else if ((objOutboundShipInq.ship_to == "" || objOutboundShipInq.ship_to == null || objOutboundShipInq.ship_to == "-"))

                            {
                                l_str_rptdtl = objOutboundShipInq.cmp_id + "_" + "BillofLadding" + "_" + objOutboundShipInq.so_num + "_" + objOutboundShipInq.shipdate;
                                objEmail.EmailSubject = objOutboundShipInq.cmp_id + "-" + " " + " " + "BillofLadding" + "|" + " " + " " + "ShipDoc#: " + objOutboundShipInq.so_num + "|" + " " + " " + "Ship Date: " + objOutboundShipInq.shipdate + "|" + " " + " " + "Cust PO#: " + objOutboundShipInq.po_num + "|" + " " + " " + "Store ID: " + objOutboundShipInq.whs_id;
                                objEmail.EmailMessage = "CmpId: " + " " + " " + objOutboundShipInq.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "SR#: " + " " + " " + objOutboundShipInq.so_num + "\n" + "Shipped Date: " + " " + " " + objOutboundShipInq.shipdate + "\n" + "Cust PO#: " + objOutboundShipInq.po_num + "\n" + "StoreID: " + objOutboundShipInq.whs_id + "\n" + "Ref#- " + "\n" + "Total Cartons Requested: " + objOutboundShipInq.TotCtns + " " + "Ctns" + "\n" + " Total Cube: " + objOutboundShipInq.TotCube + " " + "Lbs";

                            }
                            else
                            {
                                l_str_rptdtl = objOutboundShipInq.cmp_id + "_" + "BillofLadding" + "_" + objOutboundShipInq.so_num + "_" + objOutboundShipInq.shipdate;
                                objEmail.EmailSubject = objOutboundShipInq.cmp_id + "-" + " " + " " + "BillofLadding" + "|" + " " + " " + "ShipDoc#: " + objOutboundShipInq.so_num + "|" + " " + " " + "Ship Date:  " + objOutboundShipInq.shipdate + "|" + " " + " " + "Cust PO#: " + objOutboundShipInq.po_num + "|" + " " + " " + "StoreID: " + objOutboundShipInq.whs_id + "|" + " " + " " + "Ref# -Cust Name : " + objOutboundShipInq.cust_name;
                                objEmail.EmailMessage = "CmpId: " + " " + " " + objOutboundShipInq.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "ShipDoc#: " + " " + " " + objOutboundShipInq.so_num + "\n" + "Shipped Date: " + " " + " " + objOutboundShipInq.shipdate + "\n" + "Cust PO#: " + objOutboundShipInq.po_num + "\n" + "StoreID: " + objOutboundShipInq.whs_id + "\n" + "Ref#- " + "\n" + "CustName : " + objOutboundShipInq.cust_name + "\n" + "Total Cartons Requested: " + objOutboundShipInq.TotCtns + " " + "Ctns" + "\n" + " Total Cube: " + objOutboundShipInq.TotCube + " " + "Lbs";
                            }
                            //END
                            if (type == "PDF")
                            {
                                var rptSource = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt.ToList();
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                objOutboundShipInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objOutboundShipInq.Image_Path);
                                rd.SetParameterValue("TotCube", objOutboundShipInq.TotCube);
                                rd.SetParameterValue("TotCarton", objOutboundShipInq.TotCtns);
                                rd.SetParameterValue("TotWgt", objOutboundShipInq.TotWgt);
                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");  //string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));

                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//SHIPPING_POST_Bill Of ladding Report" + strDateFormat + ".pdf";
                                // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                // rd.ExportToDisk(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                reportFileName = l_str_rptdtl + DateTime.Now.ToFileTime() + ".pdf";
                                Session["RptFileName"] = strFileName;
                            }

                        }
                    }

                    else if (l_str_rpt_selection == "GridShipAck")
                    {
                        l_str_rpt_so_num = SelectedID;
                        strReportName = "rpt_iv_ship_request_Ack.rpt";
                      
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmpid.Trim();
                        objOutboundInq.Sonum = SelectedID.Trim();
                        objOutboundInq = ServiceObject.OutboundInqShipAckCtnvalues(objOutboundInq);
                        objOutboundInq.totalctns = objOutboundInq.LstOutboundInqpickstyleRpt[0].ordr_ctns;
                        objOutboundInq.totalcube = objOutboundInq.LstOutboundInqpickstyleRpt[0].ordr_cube;
                        objOutboundInq = ServiceObject.OutboundInqShipAck(objOutboundInq);
                         if (objOutboundInq.LstOutboundInqpickstyleRpt.Count > 0)
                        {
                            objOutboundInq.so_num = (objOutboundInq.LstOutboundInqpickstyleRpt[0].so_num == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].so_num.Trim() == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].so_num.Trim());
                            objOutboundInq.ship_dt = objOutboundInq.LstOutboundInqpickstyleRpt[0].shipdate.ToString("dd/MM/yyyy");
                            objOutboundInq.po_num = (objOutboundInq.LstOutboundInqpickstyleRpt[0].cust_ordr_num == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].cust_ordr_num.Trim() == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].cust_ordr_num.Trim());
                            objOutboundInq.store_id = (objOutboundInq.LstOutboundInqpickstyleRpt[0].store_id == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].store_id.Trim() == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].store_id.Trim());
                            objOutboundInq.CustName = (objOutboundInq.LstOutboundInqpickstyleRpt[0].cust_name == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].cust_name.Trim() == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].cust_name.Trim());


                            if ((objOutboundInq.po_num == "" || objOutboundInq.po_num == null || objOutboundInq.po_num == "-") && (objOutboundInq.CustName == "" || objOutboundInq.CustName == null || objOutboundInq.CustName == "-"))
                            {
                                l_str_rptdtl = objOutboundInq.cmp_id + "_" + "Outbound Ack" + "_" + objOutboundInq.so_num + "_" + objOutboundInq.ship_dt;
                                objEmail.EmailSubject = objOutboundInq.cmp_id + "-" + " " + " " + "SR Acknowledgement" + "|" + " " + " " + "SR#: " + " " + objOutboundInq.so_num + "|" + " " + " " + "SR Date : " + objOutboundInq.ship_dt + "|" + " " + " " + "Store ID: " + objOutboundInq.store_id;
                                objEmail.EmailMessage = "CmpId: " + " " + " " + objOutboundInq.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "SR#: " + " " + " " + objOutboundInq.so_num + "\n" + "SR Date: " + " " + " " + objOutboundInq.ship_dt + "\n" + "StoreID: " + objOutboundInq.store_id + "\n" + "Total Cartons Requested: " + objOutboundInq.totalctns + " " + "Ctns" + "\n" + " Total Cube: " + objOutboundInq.totalcube + " " + "Lbs";
                            }

                            else if ((objOutboundInq.CustName == "" || objOutboundInq.CustName == null || objOutboundInq.CustName == "-"))

                            {
                                l_str_rptdtl = objOutboundInq.cmp_id + "_" + "Outbound Ack" + "_" + objOutboundInq.so_num + "_" + objOutboundInq.ship_dt;
                                objEmail.EmailSubject = objOutboundInq.cmp_id + "-" + " " + " " + "Outbound Ack" + "|" + " " + " " + "SR#: " + objOutboundInq.so_num + "|" + " " + " " + "SR Date: " + objOutboundInq.ship_dt + "|" + " " + " " + "Cust PO#: " + objOutboundInq.po_num + "|" + " " + " " + "Store ID: " + objOutboundInq.store_id;
                                objEmail.EmailMessage = "CmpId: " + " " + " " + objOutboundInq.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "SR#: " + " " + " " + objOutboundInq.so_num + "\n" + "SR Date: " + " " + " " + objOutboundInq.ship_dt + "\n" + "Cust PO#: " + objOutboundInq.po_num + "\n" + "StoreID: " + objOutboundInq.store_id + "\n" + "Ref#- " + "\n" + "Total Cartons Requested: " + objOutboundInq.totalctns + " " + "Ctns" + "\n" + " Total Cube: " + objOutboundInq.totalcube + " " + "Lbs";

                            }
                            else
                            {
                                l_str_rptdtl = objOutboundInq.cmp_id + "_" + "Outbound Ack" + "_" + objOutboundInq.so_num + "_" + objOutboundInq.ship_dt;
                                objEmail.EmailSubject = objOutboundInq.cmp_id + "-" + " " + " " + "Outbound Ack" + "|" + " " + " " + "SR#: " + objOutboundInq.so_num + "|" + " " + " " + "SR Date: " + objOutboundInq.ship_dt + "|" + " " + " " + "Cust PO#: " + objOutboundInq.po_num + "|" + " " + " " + "Store: " + objOutboundInq.store_id + "|" + " " + " " + "Ref# -Cust Name : " + objOutboundInq.cust_name;
                                objEmail.EmailMessage = "CmpId: " + " " + " " + objOutboundInq.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "SR#: " + " " + " " + objOutboundInq.so_num + "\n" + "SR Date: " + " " + " " + objOutboundInq.ship_dt + "\n" + "Cust PO#: " + objOutboundInq.po_num + "\n" + "StoreID: " + objOutboundInq.store_id + "\n" + "Ref#- " + "\n" + "CustName : " + objOutboundInq.cust_name + "\n" + "Total Cartons Requested: " + objOutboundInq.totalctns + " " + "Ctns" + "\n" + " Total Cube: " + objOutboundInq.totalcube + " " + "Lbs";
                            }

                        if (type == "PDF")
                        {
                            var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            //strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath +"//Outbound_Ship Req.Acknowledgement_" + strDateFormat + ".pdf";
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                reportFileName = l_str_rptdtl + strDateFormat + ".pdf";
                            Session["RptFileName"] = strFileName;
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else
                        if (type == "XLS")
                        {
                            var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//SR_INQ_" + strDateFormat + ".xls";
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            reportFileName = "Outbound_Ship Req.Acknowledgement Report_" + strDateFormat + ".xls";//CR2018-03-07-001 Added By Nithya
                            Session["RptFileName"] = strFileName;        
                    }
                }
}
                else
                {
                    Response.Write("<H2>Report not found</H2>");
                }
                objEmail.CmpId = p_str_cmpid;
                    objEmail.screenId = ScreenID;
                    objEmail.username = objCompany.user_id;
                    objEmail.Reportselection = l_str_rpt_selection;
                EmailService objEmailService = new EmailService();
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
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                jsonErrorCode = "-2";
            }

            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ShowReport(string var_name, string SelectedID, string p_str_cmpid, string p_str_so_num_frm, string p_str_so_num_To, string p_str_so_dt_frm, string p_str_so_dt_to, string p_str_CustPO, string p_str_AlocId, string p_str_Store,
            string p_str_batch_id, string p_str_status, string p_str_shipdtFm, string p_str_shipdtTo, string type,string p_str_Style, string p_str_color, string p_str_size)
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
            l_str_rpt_selection = var_name;
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "OutboundSummary")
                    {

                        strReportName = "rpt_iv_ship_req_GridSummary.rpt";
                        OutboundInq objOutboundInq = new OutboundInq();
                        OutboundInqService ServiceObject = new OutboundInqService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmpid.Trim();
                        objOutboundInq.aloc_doc_id = p_str_AlocId.Trim();
                        objOutboundInq.so_numFm = p_str_so_num_frm.Trim();
                        objOutboundInq.so_numTo = p_str_so_num_To.Trim();
                        objOutboundInq.so_dtFm = p_str_so_dt_frm.Trim();
                        objOutboundInq.so_dtTo = p_str_so_dt_to.Trim();
                        objOutboundInq.cust_ordr_num = p_str_CustPO.Trim();
                        objOutboundInq.status = p_str_status.Trim();
                        objOutboundInq.store_id = p_str_Store.Trim();
                        objOutboundInq.quote_num = p_str_batch_id.Trim();
                        objOutboundInq.ShipdtFm = p_str_shipdtFm.Trim();
                        objOutboundInq.ShipdtTo = p_str_shipdtTo.Trim();
                        objOutboundInq.itm_num = p_str_Style.Trim();
                        objOutboundInq.itm_color = p_str_color.Trim();
                        objOutboundInq.itm_size = p_str_size.Trim();

                        objOutboundInq = ServiceObject.OutboundInqSUmmaryRpt(objOutboundInq);

                        if (type == "PDF")
                        {
                            var rptSource = objOutboundInq.LstOutboundInqSummaryRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.LstOutboundInqSummaryRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else
                        if (type == "Excel")
                        {

                            List<OB_SummaryExcel> li = new List<OB_SummaryExcel>();
                            for (int i = 0; i < objOutboundInq.LstOutboundInqSummaryRpt.Count; i++)
                            {

                                OB_SummaryExcel objOBInquiryExcel = new OB_SummaryExcel();
                                objOBInquiryExcel.Sonum = objOutboundInq.LstOutboundInqSummaryRpt[i].Sonum;
                                objOBInquiryExcel.STATUS = objOutboundInq.LstOutboundInqSummaryRpt[i].STATUS;
                                objOBInquiryExcel.STEP = objOutboundInq.LstOutboundInqSummaryRpt[i].STEP;
                                objOBInquiryExcel.SODt = objOutboundInq.LstOutboundInqSummaryRpt[i].ShipreqDt;
                                objOBInquiryExcel.CUSTID = objOutboundInq.LstOutboundInqSummaryRpt[i].CUSTID;
                                objOBInquiryExcel.CustOrderNo = objOutboundInq.LstOutboundInqSummaryRpt[i].CustOrderNo;
                                objOBInquiryExcel.STOREID = objOutboundInq.LstOutboundInqSummaryRpt[i].STOREID;
                                objOBInquiryExcel.AlocdocId = objOutboundInq.LstOutboundInqSummaryRpt[i].AlocdocId;
                                //objOBInquiryExcel.cmp_name = objOutboundInq.LstOutboundInqSummaryRpt[i].cmp_name;
                                //objOBInquiryExcel.city = objOutboundInq.LstOutboundInqSummaryRpt[i].city;
                                //objOBInquiryExcel.state_id = objOutboundInq.LstOutboundInqSummaryRpt[i].state_id;
                                //objOBInquiryExcel.post_code = objOutboundInq.LstOutboundInqSummaryRpt[i].post_code;
                                //objOBInquiryExcel.addr_line1 = objOutboundInq.LstOutboundInqSummaryRpt[i].addr_line1;
                                //objOBInquiryExcel.tel = objOutboundInq.LstOutboundInqSummaryRpt[i].tel;
                                //objOBInquiryExcel.fax = objOutboundInq.LstOutboundInqSummaryRpt[i].fax;

                                li.Add(objOBInquiryExcel);
                            }

                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["OB_SMRY"] = gv;
                            return new DownloadFileActionResult((GridView)Session["OB_SMRY"], "OB_SMRY" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



                        }
                    }
                    else if (l_str_rpt_selection == "PickTickByStyle")
                    {
                        string RptResult = string.Empty;
                        strReportName = "rpt_iv_pickslip_consolidated_by_style.rpt";
                        OutboundInq objOutboundInq = new OutboundInq();
                        OutboundInqService ServiceObject = new OutboundInqService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmpid.Trim();
                        objOutboundInq.so_numFm = p_str_so_num_frm.Trim();
                        objOutboundInq.so_numTo = p_str_so_num_To.Trim();
                        objOutboundInq.quote_num = p_str_batch_id.Trim();
                        batchId = objOutboundInq.quote_num;
                        objOutboundInq = ServiceObject.OutboundInqPickStyleRpt(objOutboundInq);

                        if (type == "PDF")
                        {
                            var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                            batchId = "Batch Id :" + batchId + "";
                            string batchIdFm = "Ship Request From :" + p_str_so_num_frm + "  Ship Request To : " + p_str_so_num_To;
                            if (batchId != string.Empty)
                            {
                                if (!string.IsNullOrEmpty(batchId))
                                    rd.SetParameterValue("fml_rep_selectionBy", batchId);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(batchIdFm))
                                    rd.SetParameterValue("fml_rep_selectionBy", batchIdFm);
                            }

                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else
                        if (type == "Excel")
                        {

                            List<OB_SR_PickByLocExcel> li = new List<OB_SR_PickByLocExcel>();
                            for (int i = 0; i < objOutboundInq.LstOutboundInqpickstyleRpt.Count; i++)
                            {

                                OB_SR_PickByLocExcel objOBInquiryExcel = new OB_SR_PickByLocExcel();
                                objOBInquiryExcel.Style = objOutboundInq.LstOutboundInqpickstyleRpt[i].itm_num;
                                objOBInquiryExcel.Color = objOutboundInq.LstOutboundInqpickstyleRpt[i].itm_color;
                                objOBInquiryExcel.Size = objOutboundInq.LstOutboundInqpickstyleRpt[i].itm_size;
                                objOBInquiryExcel.ItemName = objOutboundInq.LstOutboundInqpickstyleRpt[i].itm_name;
                                objOBInquiryExcel.Loc = objOutboundInq.LstOutboundInqpickstyleRpt[i].loc_id;
                                objOBInquiryExcel.Ctns = objOutboundInq.LstOutboundInqpickstyleRpt[i].ctn_cnt;

                                objOBInquiryExcel.Ppk = objOutboundInq.LstOutboundInqpickstyleRpt[i].ctn_cnt;
                                objOBInquiryExcel.LnQty = objOutboundInq.LstOutboundInqpickstyleRpt[i].ctn_cnt * objOutboundInq.LstOutboundInqpickstyleRpt[i].ctn_qty;




                                li.Add(objOBInquiryExcel);
                            }

                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["OB_PICK_BYSTYLE"] = gv;
                            return new DownloadFileActionResult((GridView)Session["OB_PICK_BYSTYLE"], "OB_PICK_BYSTYLE" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



                        }
                    }
                    else if (l_str_rpt_selection == "PickTickByLoc")
                    {
                        strReportName = "rpt_iv_pickslip_consolidated_by_loc.rpt";
                        OutboundInq objOutboundInq = new OutboundInq();
                        OutboundInqService ServiceObject = new OutboundInqService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmpid.Trim();
                        objOutboundInq.so_numFm = p_str_so_num_frm.Trim();
                        objOutboundInq.so_numTo = p_str_so_num_To.Trim();
                        objOutboundInq.quote_num = p_str_batch_id.Trim();
                        batchId = objOutboundInq.quote_num;
                        objOutboundInq = ServiceObject.OutboundInqPickLocationRpt(objOutboundInq);

                        if (type == "PDF")
                        {
                            var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                            batchId = "Batch Id :" + batchId + "";
                            string batchIdFm = "Ship Request From :" + p_str_so_num_frm + "  Ship Request To : " + p_str_so_num_To;
                            if (batchId != string.Empty)
                            {
                                if (!string.IsNullOrEmpty(batchId))
                                    rd.SetParameterValue("fml_rep_selectionBy", batchId);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(batchId))
                                    rd.SetParameterValue("fml_rep_selectionBy", batchIdFm);
                            }
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else
                        if (type == "Excel")
                        {

                            List<OB_SR_PickByLocExcel> li = new List<OB_SR_PickByLocExcel>();
                            for (int i = 0; i < objOutboundInq.LstOutboundInqpickstyleRpt.Count; i++)
                            {

                                OB_SR_PickByLocExcel objOBInquiryExcel = new OB_SR_PickByLocExcel();
                                objOBInquiryExcel.Loc = objOutboundInq.LstOutboundInqpickstyleRpt[i].loc_id;
                                objOBInquiryExcel.Style = objOutboundInq.LstOutboundInqpickstyleRpt[i].itm_num;
                                objOBInquiryExcel.Color = objOutboundInq.LstOutboundInqpickstyleRpt[i].itm_color;
                                objOBInquiryExcel.Size = objOutboundInq.LstOutboundInqpickstyleRpt[i].itm_size;
                                objOBInquiryExcel.ItemName = objOutboundInq.LstOutboundInqpickstyleRpt[i].itm_name;

                                objOBInquiryExcel.Ctns = objOutboundInq.LstOutboundInqpickstyleRpt[i].ctn_cnt;

                                objOBInquiryExcel.Ppk = objOutboundInq.LstOutboundInqpickstyleRpt[i].ctn_cnt;
                                objOBInquiryExcel.LnQty = objOutboundInq.LstOutboundInqpickstyleRpt[i].ctn_cnt * objOutboundInq.LstOutboundInqpickstyleRpt[i].ctn_qty;

                                li.Add(objOBInquiryExcel);
                            }

                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["OB_PICK_BYLOC"] = gv;
                            return new DownloadFileActionResult((GridView)Session["OB_PICK_BYLOC"], "OB_PICK_BYLOC" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



                        }
                    }
                    else if (l_str_rpt_selection == "GridSummary")
                    {
                        strReportName = "rpt_iv_ob_grd_Summary.rpt";
                        OutboundInq objOutboundInq = new OutboundInq();
                        OutboundInqService ServiceObject = new OutboundInqService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmpid.Trim();
                        objOutboundInq.aloc_doc_id = p_str_AlocId.Trim();
                        objOutboundInq.so_numFm = p_str_so_num_frm;
                        objOutboundInq.so_numTo = p_str_so_num_To;
                        objOutboundInq.so_dtFm = p_str_so_dt_frm;
                        objOutboundInq.so_dtTo = p_str_so_dt_to;
                        objOutboundInq.cust_ordr_num = p_str_CustPO;
                        objOutboundInq.status = p_str_status;
                        objOutboundInq.store_id = p_str_Store;
                        objOutboundInq.quote_num = p_str_batch_id;
                        objOutboundInq.ShipdtFm = p_str_shipdtFm;
                        objOutboundInq.ShipdtTo = p_str_shipdtTo;
                        objOutboundInq.itm_num = p_str_Style.Trim();
                        objOutboundInq.itm_color = p_str_color.Trim();
                        objOutboundInq.itm_size = p_str_size.Trim();
                        batchId = objOutboundInq.quote_num;
                        objOutboundInq = ServiceObject.OutboundInqGridSummary(objOutboundInq);

                        if (type == "PDF")
                        {
                            var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else
                        if (type == "Excel")
                        {

                            List<OB_Grid_SummaryExcel> li = new List<OB_Grid_SummaryExcel>();
                            for (int i = 0; i < objOutboundInq.LstOutboundInqpickstyleRpt.Count; i++)
                            {

                                OB_Grid_SummaryExcel objOBInquiryExcel = new OB_Grid_SummaryExcel();
                                //objOBInquiryExcel.cmp_id = objOutboundInq.LstOutboundInqpickstyleRpt[i].cmp_id;
                                objOBInquiryExcel.BatchId = objOutboundInq.LstOutboundInqpickstyleRpt[i].quote_num;
                                objOBInquiryExcel.SRNo = objOutboundInq.LstOutboundInqpickstyleRpt[i].so_num;
                                //
                                //objOBInquiryExcel.cust_ordr_num = objOutboundInq.LstOutboundInqpickstyleRpt[i].cust_ordr_num;
                                objOBInquiryExcel.SRDate = objOutboundInq.LstOutboundInqpickstyleRpt[i].ShipreqDt;
                                objOBInquiryExcel.AlocDocId = objOutboundInq.LstOutboundInqpickstyleRpt[i].aloc_doc_id;
                                objOBInquiryExcel.AlocDt = objOutboundInq.LstOutboundInqpickstyleRpt[i].aloc_dt;
                                objOBInquiryExcel.VasId = objOutboundInq.LstOutboundInqpickstyleRpt[i].VasId;
                                objOBInquiryExcel.ShipDocID = objOutboundInq.LstOutboundInqpickstyleRpt[i].ship_doc_id;
                                objOBInquiryExcel.ShipDt = objOutboundInq.LstOutboundInqpickstyleRpt[i].Ship_Post_Dt;
                                objOBInquiryExcel.CustPo = objOutboundInq.LstOutboundInqpickstyleRpt[i].cust_ordr_num;
                                objOBInquiryExcel.Status = objOutboundInq.LstOutboundInqpickstyleRpt[i].STATUS;
                                objOBInquiryExcel.RefNo = objOutboundInq.LstOutboundInqpickstyleRpt[i].ordr_num;
                                objOBInquiryExcel.StoreId = objOutboundInq.LstOutboundInqpickstyleRpt[i].store_id;
                                objOBInquiryExcel.CustName = objOutboundInq.LstOutboundInqpickstyleRpt[i].cust_name;
                                //objOBInquiryExcel.post_code = objOutboundInq.LstOutboundInqpickstyleRpt[i].post_code;
                                //objOBInquiryExcel.city = objOutboundInq.LstOutboundInqpickstyleRpt[i].city;
                                //objOBInquiryExcel.fax = objOutboundInq.LstOutboundInqpickstyleRpt[i].fax;
                                //objOBInquiryExcel.tel = objOutboundInq.LstOutboundInqpickstyleRpt[i].tel;
                                //objOBInquiryExcel.addr_line1 = objOutboundInq.LstOutboundInqpickstyleRpt[i].addr_line1;
                                li.Add(objOBInquiryExcel);
                            }

                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["OB_GRID_SMRY"] = gv;
                            return new DownloadFileActionResult((GridView)Session["OB_GRID_SMRY"], "OB_GRID_SMRY" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



                        }


                    }
                    else if (l_str_rpt_selection == "SR940Summary")
                    {
                        strReportName = "ShipReq940.rpt";
                        OutboundInq objOutboundInq = new OutboundInq();
                        OutboundInqService ServiceObject = new OutboundInqService();
                        Company objCompany = new Company();
                        CompanyService ServiceObjectCompany = new CompanyService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//EDI//" + strReportName;                       
                        if (type == "PDF")
                        {
                            objOutboundInq.cmp_id = p_str_cmpid.Trim();
                            objOutboundInq.so_numFm = p_str_so_num_frm.Trim();
                            objOutboundInq.so_numTo = p_str_so_num_To.Trim();
                            objOutboundInq.quote_num = p_str_batch_id.Trim();
                            objOutboundInq = ServiceObject.GetEcomSR940Rpt(objOutboundInq);
                            var rptSource = objOutboundInq.ListeCom940SRUploadDtlRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.ListeCom940SRUploadDtlRpt.Count();
                            objCompany.cmp_id = p_str_cmpid.Trim();
                            objCompany = ServiceObjectCompany.CompanyAddresHdrDtls(objCompany);
                            objOutboundInq.ListCompanyAddresHdrDtls = objCompany.ListCompanyAddresHdrDtls;
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                            rd.SetParameterValue("fmlReportTitle", "940 Ship Request Upload");
                            rd.DataDefinition.FormulaFields["fmlCompanyName"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].cmp_name.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompAddress"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].addr_line1.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompCity"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].city.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompstate_id"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].state_id.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompPhone"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].tel.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompFax"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].fax.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompPostCode"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].post_code.ToString().Trim() + "'";
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "940ShipRequestUpload");
                        }
                    }
                    else if (l_str_rpt_selection == "GridShipAck")
                    {
                        l_str_rpt_so_num = SelectedID;
                        strReportName = "rpt_iv_ship_request_Ack.rpt";
                        OutboundInq objOutboundInq = new OutboundInq();
                        OutboundInqService ServiceObject = new OutboundInqService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmpid.Trim();
                        objOutboundInq.Sonum = SelectedID.Trim();
                        objOutboundInq = ServiceObject.OutboundInqShipAck(objOutboundInq);


                        if (type == "PDF")
                        {
                            var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else
                        if (type == "Excel")
                        {

                            List<OB_SR_ACKExcel> li = new List<OB_SR_ACKExcel>();
                            for (int i = 0; i < objOutboundInq.LstOutboundInqpickstyleRpt.Count; i++)
                            {

                                OB_SR_ACKExcel objOBInquiryExcel = new OB_SR_ACKExcel();


                                //objOBInquiryExcel.so_num = objOBInquiryExcel.so_num = objOutboundInq.LstOutboundInqpickstyleRpt[i].so_num;
                                //objOBInquiryExcel.cancel_dt = objOutboundInq.LstOutboundInqpickstyleRpt[i].cancel_dt;
                                //objOBInquiryExcel.ship_dt = objOutboundInq.LstOutboundInqpickstyleRpt[i].ship_dt;
                                //objOBInquiryExcel.shipto_id = objOutboundInq.LstOutboundInqpickstyleRpt[i].shipto_id;
                                objOBInquiryExcel.shipvia_id = objOutboundInq.LstOutboundInqpickstyleRpt[i].shipvia_id;
                                objOBInquiryExcel.freight_id = objOutboundInq.LstOutboundInqpickstyleRpt[i].freight_id;
                                objOBInquiryExcel.FOB = objOutboundInq.LstOutboundInqpickstyleRpt[i].FOB;
                                objOBInquiryExcel.dept_id = objOutboundInq.LstOutboundInqpickstyleRpt[i].deptid;
                                objOBInquiryExcel.store_id = objOutboundInq.LstOutboundInqpickstyleRpt[i].store_id;
                                objOBInquiryExcel.DCId = objOutboundInq.LstOutboundInqpickstyleRpt[i].ObDcId;
                                objOBInquiryExcel.Terms = objOutboundInq.LstOutboundInqpickstyleRpt[i].terms_id;
                                //objOBInquiryExcel.due_dt = objOutboundInq.LstOutboundInqpickstyleRpt[i].due_dt;
                                //objOBInquiryExcel.status = objOutboundInq.LstOutboundInqpickstyleRpt[i].status;
                                //objOBInquiryExcel.lot_id = objOutboundInq.LstOutboundInqpickstyleRpt[i].lot_id;
                                objOBInquiryExcel.lineNum = objOutboundInq.LstOutboundInqpickstyleRpt[i].line_num;
                                //objOBInquiryExcel.itm_line = objOutboundInq.LstOutboundInqpickstyleRpt[i].itm_line;
                                //objOBInquiryExcel.ordr_ctns = objOutboundInq.LstOutboundInqpickstyleRpt[i].ordr_ctns;
                                objOBInquiryExcel.Style = objOutboundInq.LstOutboundInqpickstyleRpt[i].itm_num;
                                objOBInquiryExcel.Color = objOutboundInq.LstOutboundInqpickstyleRpt[i].itm_color;
                                objOBInquiryExcel.Size = objOutboundInq.LstOutboundInqpickstyleRpt[i].itm_size;
                                objOBInquiryExcel.Description = objOutboundInq.LstOutboundInqpickstyleRpt[i].itm_name;

                                objOBInquiryExcel.OrderQty = objOutboundInq.LstOutboundInqpickstyleRpt[i].ordr_qty;
                                //objOBInquiryExcel.back_ordr_qty = objOutboundInq.LstOutboundInqpickstyleRpt[i].back_ordr_qty;
                                //objOBInquiryExcel.ship_qty = objOutboundInq.LstOutboundInqpickstyleRpt[i].ship_qty;

                                objOBInquiryExcel.Ppk = objOutboundInq.LstOutboundInqpickstyleRpt[i].ctn_qty;
                                //objOBInquiryExcel.itm_qty = objOutboundInq.LstOutboundInqpickstyleRpt[i].itm_qty;
                                //objOBInquiryExcel.total_qty = objOutboundInq.LstOutboundInqpickstyleRpt[i].total_qty;
                                //objOBInquiryExcel.cust_ordr_num = objOutboundInq.LstOutboundInqpickstyleRpt[i].cust_ordr_num;
                                //objOBInquiryExcel.So_dt = objOutboundInq.LstOutboundInqpickstyleRpt[i].So_dt;
                                //objOBInquiryExcel.ordr_num = objOutboundInq.LstOutboundInqpickstyleRpt[i].ordr_num;
                                objOBInquiryExcel.Uom = objOutboundInq.LstOutboundInqpickstyleRpt[i].qty_uom;

                                objOBInquiryExcel.po_num = objOutboundInq.LstOutboundInqpickstyleRpt[i].po_num;
                                objOBInquiryExcel.Note = objOutboundInq.LstOutboundInqpickstyleRpt[i].Note;

                                //objOBInquiryExcel.cmp_name = objOutboundInq.LstOutboundInqpickstyleRpt[i].cmp_name;
                                //objOBInquiryExcel.state_id = objOutboundInq.LstOutboundInqpickstyleRpt[i].state_id;
                                //objOBInquiryExcel.post_code = objOutboundInq.LstOutboundInqpickstyleRpt[i].post_code;
                                //objOBInquiryExcel.city = objOutboundInq.LstOutboundInqpickstyleRpt[i].city;
                                //objOBInquiryExcel.fax = objOutboundInq.LstOutboundInqpickstyleRpt[i].fax;
                                //objOBInquiryExcel.tel = objOutboundInq.LstOutboundInqpickstyleRpt[i].tel;
                                //objOBInquiryExcel.addr_line1 = objOutboundInq.LstOutboundInqpickstyleRpt[i].addr_line1;
                                //objOBInquiryExcel.itm_name = objOutboundInq.LstOutboundInqpickstyleRpt[i].itm_name;

                                li.Add(objOBInquiryExcel);
                            }

                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["OB_GRID_SMRY"] = gv;
                            return new DownloadFileActionResult((GridView)Session["OB_GRID_SMRY"], "OB_GRID_SMRY" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



                        }
                    }
                    //CR-2018-04-24-001 Added BY Nithya
                    else if (l_str_rpt_selection == "BillofLadding")
                    {
                        strReportName = "rpt_iv_bill_of_lading.rpt";
                        OutboundShipInq objOutboundShipInq = new OutboundShipInq();
                        OutboundShipInqService ServiceObject = new OutboundShipInqService();
                        ReportDocument rd = new ReportDocument();
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
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundShipInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundShipInq.Image_Path);
                            rd.SetParameterValue("TotCube", objOutboundShipInq.TotCube);
                            rd.SetParameterValue("TotCarton", objOutboundShipInq.TotCtns);
                            rd.SetParameterValue("TotWgt", objOutboundShipInq.TotWgt);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
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
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ReportView()
        {
            return View();
        }
        public ActionResult ShipReqDetail(string Id, string cmp_id, string batchId)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.CompID = cmp_id;
            objOutboundInq.ShipReqID = Id;
            objOutboundInq.QuoteNum = batchId;
            objOutboundInq = ServiceObject.OutboundShipInqhdr(objOutboundInq);
            objOutboundInq.ShipReqID = (objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipReqID == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipReqID == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipReqID); 
            objOutboundInq.ShipReqDt = (objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipReqDt == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipReqDt == "" ? string.Empty : Convert.ToDateTime(objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipReqDt).ToString("MM/dd/yyyy"));
            objOutboundInq.OrdType = (objOutboundInq.LstOutboundInqpickstyleRpt[0].OrdType == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].OrdType == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].OrdType);
            objOutboundInq.status = (objOutboundInq.LstOutboundInqpickstyleRpt[0].status == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].status == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].status);
            if (objOutboundInq.status.Trim() == "O")
            {
                objOutboundInq.status = "OPEN";
            }
            else if (objOutboundInq.status.Trim() == "P")
            {
                objOutboundInq.status = "POST";
            }
            else if (objOutboundInq.status.Trim() == "A")
            {
                objOutboundInq.status = "ALOC";
            }
            else if (objOutboundInq.status.Trim() == "S")
            {
                objOutboundInq.status = "SHIP";
            }
            else if (objOutboundInq.status.Trim() == "B")
            {
                objOutboundInq.status = "B/O";
            }
            objOutboundInq.step = (objOutboundInq.LstOutboundInqpickstyleRpt[0].step == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].step == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].step); 
            objOutboundInq.cust_id = (objOutboundInq.LstOutboundInqpickstyleRpt[0].cust_id == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].cust_id == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].cust_id); 
            objOutboundInq.OrdNum = (objOutboundInq.LstOutboundInqpickstyleRpt[0].OrdNum == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].OrdNum == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].OrdNum);
            objOutboundInq.CustPO = (objOutboundInq.LstOutboundInqpickstyleRpt[0].CustPO == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].CustPO == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].CustPO); 
            objOutboundInq.DeptID = (objOutboundInq.LstOutboundInqpickstyleRpt[0].DeptID == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].DeptID == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].DeptID); 
            objOutboundInq.ShipVia = (objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipVia == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipVia == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipVia);
            objOutboundInq.FreightID = (objOutboundInq.LstOutboundInqpickstyleRpt[0].FreightID == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].FreightID == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].FreightID); 
            objOutboundInq.ShipDt =  (objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipDt == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipDt == "" ? string.Empty : Convert.ToDateTime(objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipDt).ToString("MM/dd/yyyy"));
            objOutboundInq.CancelDt = (objOutboundInq.LstOutboundInqpickstyleRpt[0].CancelDt == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].CancelDt == "" ? string.Empty : Convert.ToDateTime(objOutboundInq.LstOutboundInqpickstyleRpt[0].CancelDt).ToString("MM/dd/yyyy"));
            objOutboundInq.fob =  (objOutboundInq.LstOutboundInqpickstyleRpt[0].fob == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].fob == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].fob);
            objOutboundInq.store_id =  (objOutboundInq.LstOutboundInqpickstyleRpt[0].store_id == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].store_id == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].store_id);
            objOutboundInq.DCID =  (objOutboundInq.LstOutboundInqpickstyleRpt[0].DCID == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].DCID == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].DCID);
            objOutboundInq.ShipToAttn =  (objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToAttn == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToAttn == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToAttn);
            objOutboundInq.ShipToEmail = (objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToEmail == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToEmail == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToEmail);
            objOutboundInq.ShipToAddr1 =  (objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToAddr1 == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToAddr1 == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToAddr1);
            objOutboundInq.ShipToAddr2 =  (objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToAddr2 == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToAddr2 == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToAddr2);
            objOutboundInq.ShipToCity =  (objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToCity == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToCity == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToCity);
            objOutboundInq.ShipToState =  (objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToState == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToState == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToState);
            objOutboundInq.ShipToZipCode =  (objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToZipCode == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToZipCode == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToZipCode);
            objOutboundInq.ShipToCountry = (objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToCountry == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToCountry == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToCountry);
            objOutboundInq.note =  (objOutboundInq.LstOutboundInqpickstyleRpt[0].note == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].note == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].note); 
            objOutboundInq.ShipToID = (objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToID == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToID == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToID);
            objOutboundInq = ServiceObject.OutboundShipInqdtl(objOutboundInq);
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_ShipReqDetail", objOutboundInqModel);
        }


        public ActionResult ShowdtlReport(string p_str_cmpid, string p_str_ShipReq_id)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string cmp_id = p_str_cmpid;
            string ib_doc_id = p_str_ShipReq_id;
            OutboundInq objInbound = new OutboundInq();
            OutboundInqService objService = new OutboundInqService();
            try
            {
                if (isValid)
                {
                    strReportName = "rpt_iv_ship_request_Ack.rpt";
                    OutboundInq objOutboundInq = new OutboundInq();
                    OutboundInqService ServiceObject = new OutboundInqService();
                    ReportDocument rd = new ReportDocument();
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                    objOutboundInq.cmp_id = p_str_cmpid;
                    objOutboundInq.Sonum = p_str_ShipReq_id;
                    objOutboundInq = ServiceObject.OutboundInqShipAck(objOutboundInq);
                    var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                    rd.Load(strRptPath);
                    int AlocCount = 0;
                    AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                        rd.SetDataSource(rptSource);
                    objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                    rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);

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
        public ActionResult ShipToAdress(string cmpid)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.cmp_id = cmpid;
            objOutboundInq.status = "O";
            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = cmpid;
            objPick.Whs_id = "";
            objPick.Whs_name = "";

            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objOutboundInq.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objOutboundInq.ListStatePick = objPick.ListStatePick;

            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_ShipToAddrs", objOutboundInqModel);
        }

        public ActionResult ShipEntry(string cmpid, string p_str_screen_title)
        {
            string l_str_ibdocid;
            string l_str_uom;
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objOutboundInq.cmp_id = cmpid;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            //objOutboundInq.screentitle = Session["screentitle"].ToString();//CR_MVC_3PL_0317-001 Added By Nithya
            objOutboundInq.status = "O";
            Session["tempcmpid"] = objOutboundInq.cmp_id;
            Session["tempStatus"] = objOutboundInq.status;
            objOutboundInq.so_dt = DateTime.Now.ToString("MM-dd-yyyy");
            objOutboundInq.screentitle = p_str_screen_title;
            objOutboundInq.ShipDt = DateTime.Now.AddDays(1).ToString("MM-dd-yyyy");
            objOutboundInq.CancelDt = DateTime.Now.AddDays(2).ToString("MM-dd-yyyy");
            objOutboundInq.cust_id = "-";
            objOutboundInq.dc_id = "1";
            objOutboundInq.refno = "-";
            objOutboundInq.AuthId = "-";
            objOutboundInq.ShipVia = "-";
            objOutboundInq.Note = "-";
            objOutboundInq.CustPO = "";
            objOutboundInq.shipchrg = "0";
            objOutboundInq.shipto_id = "-";
            objOutboundInq.Attn = "-";
            objOutboundInq.Mailname = "-";
            objOutboundInq.Addr1 = "-";
            objOutboundInq.Addr2 = "-";
            objOutboundInq.city = "-";
            objOutboundInq.zipcode = "-";
            objOutboundInq.dept_id = "1";
            objOutboundInq.store_id = "1";
            objOutboundInq.CustPO = "-";
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objOutboundInq.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objOutboundInq = ServiceObject.GetIbSRIdDetail(objOutboundInq);
            objOutboundInq.Sonum = objOutboundInq.SRNUm;
            l_str_ibdocid = objOutboundInq.SRNUm;
            objOutboundInq.Sonum = l_str_ibdocid;
            Session["tempSonum"] = objOutboundInq.Sonum;
            objOutboundInq.Sonum = objOutboundInq.Sonum;//CR-180402-001 Commented By Nithya
            ServiceObject.DeleteTempshipEntrytable(objOutboundInq);
            objOutboundInq = ServiceObject.GetDftWhs(objOutboundInq);
            string l_str_DftWhs = objOutboundInq.ListPickdtl[0].dft_whs.Trim();
            if (l_str_DftWhs != "" || l_str_DftWhs != null)
            {
                objOutboundInq.fob = objOutboundInq.ListPickdtl[0].dft_whs.Trim();
                objOutboundInq.whs_id = objOutboundInq.ListPickdtl[0].dft_whs.Trim();

            }
            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = cmpid;
            objPick.Whs_id = "";
            objPick.Whs_name = "";
            objPick = ServiceObjectPick.GetWhsPick(objPick);
            objOutboundInq.ListPick = objPick.ListPick;
            //objOutboundInq.fob = objOutboundInq.ListPick[0].Whs_id.Trim();
            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objOutboundInq.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objOutboundInq.ListStatePick = objPick.ListStatePick;
            objPick = ServiceObjectPick.GetShipToPick(objPick);
            objOutboundInq.ListShipToPick = objPick.ListShipToPick;
            objPick = ServiceObjectPick.GetPickUom(objPick);
            //objOutboundInq.ListUomPick = objPick.ListUomPick;
            l_str_uom = objOutboundInq.Check_existing_uom;

            if (l_str_uom != null)
            {
                objOutboundInq.ListUomPick = objPick.ListUomPick;
                l_str_uom = objOutboundInq.ListUomPick[0].qty_uom.Trim();

            }
            else
            {
                List<Pick> li = new List<Pick>();
                objPick.qty_uom = "EA";
                li.Add(objPick);
                objOutboundInq.ListUomPick = li;
            }

            LookUp objLookUp = new LookUp();
            LookUpService ServiceObject1 = new LookUpService();
            objLookUp.id = "12";
            objLookUp.lookuptype = "OUTBOUNDSTATUS";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objOutboundInq.ListLookUpDtl = objLookUp.ListLookUpDtl;
            objLookUp.id = "13";
            objLookUp.lookuptype = "OUTBOUNDPRICETKT";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objOutboundInq.Listpricetkt = objLookUp.ListLookUpDtl;
            objLookUp.id = "14";
            objLookUp.lookuptype = "OUTBOUNDTYPE";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objOutboundInq.Listtype = objLookUp.ListLookUpDtl;
            objLookUp.id = "15";
            objLookUp.lookuptype = "OUTBOUNDFRIEGHTID";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objOutboundInq.Listfrieghtid = objLookUp.ListLookUpDtl;
            objOutboundInq.LineNum = 1;
            objOutboundInq.cmp_id = cmpid;
            objOutboundInq = ServiceObject.GetPickStyleDetails(objOutboundInq);
            objOutboundInq.ListItmStyledtl = objOutboundInq.ListItmStyledtl;
            objOutboundInq.View_Flag = "A";
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_ShipReqEntry", objOutboundInqModel);
        }
        public JsonResult On_ChangeShipToDetails(string id)
        {
            try
            {
                OutboundInq objOutboundInq = new OutboundInq();
                OutboundInqService ServiceObject = new OutboundInqService();
                Pick objPick = new Pick();
                PickService ServiceObjectPick = new PickService();
                objPick.cmp_id = Session["tempcmpid"].ToString().Trim();
                objPick.shipto_id = id.Trim();
                objPick = ServiceObjectPick.GetPickExsistShipTo(objPick);
                objOutboundInq.ListExistShipToAddrsPick = objPick.ListExistShipToAddrsPick;
                return Json(objOutboundInq.ListExistShipToAddrsPick, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public JsonResult On_ChangeCountryDetails(string id)
        {
            try
            {
                OutboundInq objOutboundInq = new OutboundInq();
                OutboundInqService ServiceObject = new OutboundInqService();
                Pick objPick = new Pick();
                PickService ServiceObjectPick = new PickService();
                objPick.cmp_id = Session["tempcmpid"].ToString().Trim();
                objPick.Cntry_Id = id.Trim();
                objPick = ServiceObjectPick.GetStatePick(objPick);
                objOutboundInq.ListCntryPick = objPick.ListCntryPick;
                return Json(objOutboundInq.ListCntryPick, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public FileResult downloadFile()
        {
            return File("~\\uploads\\OSU_SR_Template.csv", "text/csv", string.Format("Sample-{0}.csv", DateTime.Now.ToString("yyyyMMdd-HHmmss")));
        }
        public JsonResult LoadAvailQty(string p_str_cmp_id, string p_str_Itmcode, string p_str_style, string p_str_color, string p_str_size)
        {
            string avlqty;
            int AvailQty = 0;
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.cmp_id = p_str_cmp_id;
            objOutboundInq.itm_code = p_str_Itmcode;
            objOutboundInq.itm_num = p_str_style;
            objOutboundInq.itm_color = p_str_color;
            objOutboundInq.itm_size = p_str_size;
            objOutboundInq = ServiceObject.LoadAvailQty(objOutboundInq);
            if (objOutboundInq.LstItmxCustdtl.Count == 0)
            {
                objOutboundInq.avlqty = 0;
            }
            else
            {
                objOutboundInq.avlqty = objOutboundInq.LstItmxCustdtl[0].avail_qty;

            }

            avlqty = Convert.ToString(objOutboundInq.avlqty);
            objOutboundInq = ServiceObject.Getitmlist(objOutboundInq);
            if (objOutboundInq.LstItmxCustdtl.Count > 0)
            {
                objOutboundInq.itm_code = objOutboundInq.LstItmxCustdtl[0].itm_code;//CR_MVC_3PL_20180411-001 Added By NIthya
                objOutboundInq.LstItmxCustdtl[0].avail_qty = Convert.ToDecimal(avlqty);
                objOutboundInq.length = objOutboundInq.LstItmxCustdtl[0].length;
                objOutboundInq.width = objOutboundInq.LstItmxCustdtl[0].width;
                objOutboundInq.height = objOutboundInq.LstItmxCustdtl[0].depth;
                objOutboundInq.cube = objOutboundInq.LstItmxCustdtl[0].cube;
                objOutboundInq.weight = objOutboundInq.LstItmxCustdtl[0].wgt;
                objOutboundInq.ppk = objOutboundInq.LstItmxCustdtl[0].ctn_qty;
                p_str_Itmcode = objOutboundInq.LstItmxCustdtl[0].itm_code;
                GetAvailQty(ref AvailQty, p_str_Itmcode, p_str_cmp_id);//CR_MVC_3PL_20180411-001 Added By NIthya  
                objOutboundInq.LstItmxCustdtl[0].avail_qty = AvailQty;
                objOutboundInq.avail_qty = objOutboundInq.LstItmxCustdtl[0].avail_qty;//CR_MVC_3PL_20180411-001 Added By NIthya   
            }
            return Json(objOutboundInq.LstItmxCustdtl, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShipReqPost(string CmpId, string AlocDocId, string p_str_AlocDt)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objOutboundInq.cmp_id = CmpId;
            objOutboundInq.aloc_doc_id = AlocDocId;
            objOutboundInq.aloc_dt = p_str_AlocDt;//CR2018-03-07-001 Added By Nithya
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany.cust_cmp_id = CmpId;
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objOutboundInq.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objOutboundInq = ServiceObject.GetAlocGridLoadItem(objOutboundInq);
            objOutboundInq = ServiceObject.GetPickGridLoadItem(objOutboundInq);
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_AlocPost", objOutboundInqModel);
        }
        public ActionResult SaveAlocPost(string p_str_cmp_id, string p_str_alocdocid, string p_str_aloc_dt)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.cmp_id = p_str_cmp_id;
            objOutboundInq.aloc_doc_id = p_str_alocdocid;
            objOutboundInq.aloc_dt = p_str_aloc_dt;
            objOutboundInq = ServiceObject.GetShipNum(objOutboundInq);
            objOutboundInq.bol_num = objOutboundInq.ShipDocNum;
            l_str_bol_num = objOutboundInq.ShipDocNum;
            Session["l_str_bol_num"] = l_str_bol_num;
            objOutboundInq.bol_num = l_str_bol_num;
            objOutboundInq = ServiceObject.GetAlocType(objOutboundInq);
            objOutboundInq.aloc_type = (objOutboundInq.LstGetAlocType[0].aloc_type == null || objOutboundInq.LstGetAlocType[0].aloc_type.Trim() == "" ? string.Empty : objOutboundInq.LstGetAlocType[0].aloc_type.Trim());
            objOutboundInq.ship_to_name = (objOutboundInq.LstGetAlocType[0].ship_to_name == null || objOutboundInq.LstGetAlocType[0].ship_to_name.Trim() == "" ? string.Empty : objOutboundInq.LstGetAlocType[0].ship_to_name.Trim()); 
            objOutboundInq.cntr_num = (objOutboundInq.LstGetAlocType[0].cntr_num == null || objOutboundInq.LstGetAlocType[0].cntr_num.Trim() == "" ? string.Empty : objOutboundInq.LstGetAlocType[0].cntr_num.Trim());
            objOutboundInq.ship_to_id = (objOutboundInq.LstGetAlocType[0].ship_to_id == null || objOutboundInq.LstGetAlocType[0].ship_to_id.Trim() == "" ? string.Empty : objOutboundInq.LstGetAlocType[0].ship_to_id.Trim());
            if (objOutboundInq.aloc_type == "Direct")
            {
                ServiceObject.GetAlocPostDirect(objOutboundInq);
            }
            else
            {
                ServiceObject.GetAlocPost(objOutboundInq);
            }
            ServiceObject.UpdateStatusInAlocHdr(objOutboundInq);
            ServiceObject.UpdateStatusInAlocDtl(objOutboundInq);
            objOutboundInq = ServiceObject.GetCustId(objOutboundInq);
            objOutboundInq.cust_id = (objOutboundInq.ListCustId[0].cust_id == null || objOutboundInq.ListCustId[0].cust_id.Trim() == "" ? string.Empty : objOutboundInq.ListCustId[0].cust_id.Trim());
            objOutboundInq.ship_to_id = (objOutboundInq.ListCustId[0].ship_to_id == null || objOutboundInq.ListCustId[0].ship_to_id.Trim() == "" ? string.Empty : objOutboundInq.ListCustId[0].ship_to_id.Trim());
            objOutboundInq.whs_id = (objOutboundInq.ListCustId[0].whs_id == null || objOutboundInq.ListCustId[0].whs_id.Trim() == "" ? string.Empty : objOutboundInq.ListCustId[0].whs_id.Trim());
            objOutboundInq.note = "Ship Confirmation On:" + objOutboundInq.aloc_dt.Trim();
            objOutboundInq = ServiceObject.GetShipToAddress(objOutboundInq);
            if (objOutboundInq.ListShipToAddress.Count() == 0)
            {
                objOutboundInq.mail_name = "";
                objOutboundInq.addr_line1 = "";
                objOutboundInq.addr_line2 = "";
                objOutboundInq.city = "";
                objOutboundInq.state_id = "";
                objOutboundInq.post_code = "";
                objOutboundInq.cntry_id = "";
            }
            else
            {
                //(objOutboundInq.lstobjOutboundInq[0].so_dt == null || objOutboundInq.lstobjOutboundInq[0].so_dt == "" ? string.Empty : Convert.ToDateTime(objOutboundInq.lstobjOutboundInq[0].so_dt).ToString("MM/dd/yyyy"));
                objOutboundInq.mail_name = (objOutboundInq.ListShipToAddress[0].mail_name == null || objOutboundInq.ListShipToAddress[0].mail_name.Trim() == "" ? string.Empty : objOutboundInq.ListShipToAddress[0].mail_name);
                objOutboundInq.addr_line1 = (objOutboundInq.ListShipToAddress[0].addr_line1 == null || objOutboundInq.ListShipToAddress[0].addr_line1.Trim() == "" ? string.Empty : objOutboundInq.ListShipToAddress[0].addr_line1);
                objOutboundInq.addr_line2 = (objOutboundInq.ListShipToAddress[0].addr_line2 == null || objOutboundInq.ListShipToAddress[0].addr_line2.Trim() == "" ? string.Empty : objOutboundInq.ListShipToAddress[0].addr_line2.Trim());
                objOutboundInq.city = (objOutboundInq.ListShipToAddress[0].city == null || objOutboundInq.ListShipToAddress[0].city.Trim() == "" ? string.Empty : objOutboundInq.ListShipToAddress[0].city);
                objOutboundInq.state_id = (objOutboundInq.ListShipToAddress[0].state_id == null || objOutboundInq.ListShipToAddress[0].state_id.Trim() == "" ? string.Empty : objOutboundInq.ListShipToAddress[0].state_id);
                objOutboundInq.post_code = (objOutboundInq.ListShipToAddress[0].post_code == null || objOutboundInq.ListShipToAddress[0].post_code.Trim() == "" ? string.Empty : objOutboundInq.ListShipToAddress[0].post_code);
                objOutboundInq.cntry_id = (objOutboundInq.ListShipToAddress[0].cntry_id == null || objOutboundInq.ListShipToAddress[0].cntry_id.Trim() == "" ? string.Empty : objOutboundInq.ListShipToAddress[0].cntry_id);
            }

            ServiceObject.InsertShipHdr(objOutboundInq);
            objOutboundInq = ServiceObject.GetUnPickQty(objOutboundInq);
            for (int i = 0; i < objOutboundInq.LstOutboundUnPickQty.Count(); i++)
            {
                objOutboundInq.itm_qty = objOutboundInq.LstOutboundUnPickQty[i].itm_qty;
                objOutboundInq.itm_code = objOutboundInq.LstOutboundUnPickQty[i].itm_code.Trim();
                objOutboundInq.whs_id = (objOutboundInq.LstOutboundUnPickQty[i].whs_id.Trim() == null || objOutboundInq.LstOutboundUnPickQty[i].whs_id.Trim() == "") ? "" : objOutboundInq.LstOutboundUnPickQty[i].whs_id.Trim();
                objOutboundInq.lot_id = (objOutboundInq.LstOutboundUnPickQty[i].lot_id.Trim() == null || objOutboundInq.LstOutboundUnPickQty[i].lot_id.Trim() == "") ? "" : objOutboundInq.LstOutboundUnPickQty[i].lot_id.Trim();
                objOutboundInq.loc_id = (objOutboundInq.LstOutboundUnPickQty[i].loc_id.Trim() == null || objOutboundInq.LstOutboundUnPickQty[i].loc_id.Trim() == "") ? "" : objOutboundInq.LstOutboundUnPickQty[i].loc_id.Trim();
                objOutboundInq.rcvd_dt = objOutboundInq.LstOutboundUnPickQty[0].rcvd_dt;
                ServiceObject.UpdateTrnHdr(objOutboundInq);
            }
            ServiceObject.InsertShipDtl(objOutboundInq);
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("~/Views/OutboundInq/OutboundInq.cshtml", objOutboundInqModel);
        }
        public ActionResult ShipRequestUnPost(string CmpId, string AlocDocId, string p_str_shipdocId)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objOutboundInq.cmp_id = CmpId;
            objOutboundInq.aloc_doc_id = AlocDocId;
            //objOutboundInq.aloc_dt = DateTime.Now.ToString("MM/dd/yyyy");
            objCompany.user_id = Session["UserID"].ToString().Trim();
            if(p_str_shipdocId==""|| p_str_shipdocId ==null)
            {
                objOutboundInq.bol_num = Session["l_str_bol_num"].ToString(); //CR-3PL_MVC-OB-20180405-001 Added By Nithya
            }
            else
            {
                objOutboundInq.bol_num = p_str_shipdocId;
            }          
            objCompany.cust_cmp_id = CmpId;
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objOutboundInq.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objOutboundInq = ServiceObject.GetAlocUnPostGridLoadItem(objOutboundInq);
            //objOutboundInq = ServiceObject.GetPickGridLoadItem(objOutboundInq);
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_AlocUnPost", objOutboundInqModel);
        }
        public ActionResult SaveAlocUnPost(string p_str_cmp_id, string p_str_alocdocid, string p_str_bolnum)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.cmp_id = p_str_cmp_id;
            objOutboundInq.aloc_doc_id = p_str_alocdocid;
            objOutboundInq.bol_num = p_str_bolnum;
            objOutboundInq = ServiceObject.CheckAllocpost(objOutboundInq);
            if (objOutboundInq.LstCheckAllocPost.Count() == 0)
            {
                return Json(objOutboundInq.LstCheckAllocPost.Count, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //CR - 3PL_MVC-IB-20180405 Added By Nithya
               objOutboundInq = ServiceObject.GetPickQty(objOutboundInq);


                //for (int i = 0; i < objOutboundInq.LstOutboundPickQty.Count(); i++)
                //{
                //    objOutboundInq.itm_qty = objOutboundInq.LstOutboundPickQty[i].total_qty;
                //    objOutboundInq.itm_code = objOutboundInq.LstOutboundPickQty[i].itm_code.Trim();
                //    objOutboundInq.whs_id = (objOutboundInq.LstOutboundPickQty[i].whs_id.Trim() == null || objOutboundInq.LstOutboundPickQty[i].whs_id.Trim() == "") ? "" : objOutboundInq.LstOutboundPickQty[i].whs_id.Trim();
                //    objOutboundInq.lot_id = (objOutboundInq.LstOutboundPickQty[i].lot_id.Trim() == null || objOutboundInq.LstOutboundPickQty[i].lot_id.Trim() == "") ? "" : objOutboundInq.LstOutboundPickQty[i].lot_id.Trim();
                //    objOutboundInq.loc_id = (objOutboundInq.LstOutboundPickQty[i].loc_id.Trim() == null || objOutboundInq.LstOutboundPickQty[i].loc_id.Trim() == "") ? "" : objOutboundInq.LstOutboundPickQty[i].loc_id.Trim();
                //    objOutboundInq.Recvddt = Convert.ToString(objOutboundInq.LstOutboundPickQty[i].rcvd_dt.ToString("MM/dd/yyyy"));
                //    objOutboundInq.palet_id = objOutboundInq.LstOutboundPickQty[i].palet_id;
                //    objOutboundInq.pkg_qty = objOutboundInq.LstOutboundPickQty[i].itm_qty;
                //    objOutboundInq.itm_num = objOutboundInq.LstOutboundPickQty[i].itm_num;
                //    objOutboundInq.itm_color = objOutboundInq.LstOutboundPickQty[i].itm_color;
                //    objOutboundInq.itm_size = objOutboundInq.LstOutboundPickQty[i].itm_size;
                //    objOutboundInq.pkg_type = objOutboundInq.LstOutboundPickQty[i].pkg_type;
                //    ServiceObject.UpdateAvailQtyTrnHdr(objOutboundInq);
                //}
                //END
                ServiceObject.SaveAlocUnPost(objOutboundInq);
            }
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("~/Views/OutboundInq/OutboundInq.cshtml", objOutboundInqModel);
        }
        public ActionResult TruncateTempShipReqEntry()
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.cmp_id = "";
            objOutboundInq.Sonum = "";
            ServiceObject.DeleteTempshipEntrytable(objOutboundInq);
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return View("~/Views/OutboundInq/OutboundInq.cshtml", objOutboundInqModel);

        }
        public ActionResult InsertTemptable(string p_str_cmp_id, string p_str_Sonum, int p_str_LineNum, string p_str_STATUS, string p_str_Itmdtl, string p_str_itm_color, string p_str_itm_size, string p_str_itm_name, decimal p_str_avlqty, decimal p_str_ord_qty, decimal p_str_ppk, decimal p_str_ctn, string p_str_qty_uom, decimal p_str_length,
          decimal p_str_width, decimal p_str_height, decimal p_str_weight, string p_str_cube, string p_str_vendpo, string p_str_note, string p_str_cust_id, string p_str_itm_code, string p_str_Mode)
        {
            try
            {
                OutboundInq objOutboundInq = new OutboundInq();
                OutboundInqService ServiceObject = new OutboundInqService();
                objOutboundInq.LineNum = p_str_LineNum;
                objOutboundInq.cmp_id = p_str_cmp_id;
                objOutboundInq.Sonum = p_str_Sonum;
                objOutboundInq.itm_code = p_str_itm_code;
                objOutboundInq = ServiceObject.GetCheckExistStyle(objOutboundInq);
                if (objOutboundInq.ListCheckExistStyle.Count == 0)
                {
                    objOutboundInq = ServiceObject.GetCheckExistGridData(objOutboundInq);
                    objOutboundInq = ServiceObject.GetSRIdDtl(objOutboundInq);
                    objOutboundInq.id = objOutboundInq.ListSRDocId[0].id;
                    objOutboundInq.STATUS = p_str_STATUS.Trim();
                    objOutboundInq.itm_num = p_str_Itmdtl.Trim();
                    objOutboundInq.itm_color = p_str_itm_color.Trim();
                    objOutboundInq.itm_size = p_str_itm_size.Trim();
                    objOutboundInq.itm_name = p_str_itm_name.Trim();
                    objOutboundInq.avlqty = p_str_avlqty;
                    objOutboundInq.OrdQty = p_str_ord_qty;
                    objOutboundInq.ppk = p_str_ppk;
                    objOutboundInq.ctn = p_str_ctn;
                    objOutboundInq.po_num = p_str_vendpo;
                    objOutboundInq.length = p_str_length;
                    objOutboundInq.width = p_str_width;
                    objOutboundInq.height = p_str_height;
                    objOutboundInq.weight = p_str_weight;
                    objOutboundInq.Cube = p_str_cube;
                    objOutboundInq.vendpo = p_str_vendpo.Trim();
                    objOutboundInq.Note = p_str_note.Trim();
                    objOutboundInq.lot_id = "";
                    objOutboundInq.pack_id = "PF";
                    objOutboundInq.comm_pcnt = "5";
                    objOutboundInq.cust_id = p_str_cust_id;

                    objOutboundInq = ServiceObject.Getkitflag(objOutboundInq);
                    objOutboundInq.kit_itm = objOutboundInq.ListItmStyledtl[0].kit_itm.Trim();
                    objOutboundInq.itm_code = objOutboundInq.ListItmStyledtl[0].itm_code;
                    objOutboundInq = ServiceObject.GetCustDtl(objOutboundInq);
                    if (objOutboundInq.LstItmxCustdtl.Count == 0)
                    {
                        objOutboundInq.cust_itm_num = "-";
                        objOutboundInq.cust_itm_color = "-";
                        objOutboundInq.cust_itm_desc = "-";
                    }
                    else
                    {
                        objOutboundInq.cust_itm_num = objOutboundInq.LstItmxCustdtl[0].cust_itm_num;
                        objOutboundInq.cust_itm_color = objOutboundInq.LstItmxCustdtl[0].cust_itm_color;
                        objOutboundInq.cust_itm_desc = objOutboundInq.LstItmxCustdtl[0].cust_itm_desc;
                    }

                    objOutboundInq = ServiceObject.Getitmlist(objOutboundInq);
                    objOutboundInq.wgt = objOutboundInq.LstItmxCustdtl[0].wgt;
                    objOutboundInq.cube = objOutboundInq.LstItmxCustdtl[0].cube;
                    objOutboundInq.list_price = objOutboundInq.LstItmxCustdtl[0].list_price;
                    objOutboundInq.qty_uom = objOutboundInq.LstItmxCustdtl[0].qty_uom;
                    if (objOutboundInq.qty_uom == "" || objOutboundInq.qty_uom == null)
                    {
                        objOutboundInq.qty_uom = "EA";
                    }
                    objOutboundInq.price_uom = objOutboundInq.LstItmxCustdtl[0].price_uom;
                    if (objOutboundInq.price_uom == "" || objOutboundInq.price_uom == null)
                    {
                        objOutboundInq.price_uom = "EA";
                    }
                    objOutboundInq.dtlsize = (p_str_length * p_str_width * p_str_height);
                    ordrcost = p_str_ord_qty + objOutboundInq.LstItmxCustdtl[0].list_price;
                    objOutboundInq.Errdesc = "-";
                    ServiceObject.InsertTemptableValue(objOutboundInq);
                    objOutboundInq = ServiceObject.GetGridList(objOutboundInq);
                    Mapper.CreateMap<OutboundInq, OutboundInqModel>();
                    OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
                    return PartialView("_ShipEntryGrid", objOutboundInqModel);
                }
                else
                {
                    return Json(objOutboundInq.ListCheckExistStyle.Count, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public JsonResult CheckItemdtl(string p_str_cmp_id, string p_str_style, string p_str_color, string p_str_size)
        {

            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = p_str_cmp_id;
            objPick.itm_num = p_str_style;
            objPick.itm_color = p_str_color;
            objPick.itm_size = p_str_size;
            objPick = ServiceObjectPick.GetPickItemCode(objPick);
            objOutboundInq.ListCheckItemCode = objPick.ListCheckItemCode;


            return Json(objOutboundInq.ListCheckItemCode, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadStyledtl(string p_str_cmp_id)
        {

            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.cmp_id = p_str_cmp_id;
            objOutboundInq = ServiceObject.GetPickStyleDetails(objOutboundInq);
            objOutboundInq.itm_num = objOutboundInq.itm_num;
            objOutboundInq.itm_color = objOutboundInq.itm_color;
            objOutboundInq.itm_size = objOutboundInq.itm_size;
            return Json(objOutboundInq.ListItmStyledtl, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveShipReqEntry(string p_str_cmp_id, string p_str_cust_id, string p_str_Sonum, string p_str_pricetkt, string p_str_so_dt,
           string p_str_ShipDt, string p_str_CancelDt, string p_str_refno, string p_str_freight_id, string p_str_status, string p_str_Type, string p_str_AuthId
            , string p_str_fob, string p_str_ShipVia, string p_str_shipchrg, string p_str_CustPO, string p_str_CustOrderdt, string p_str_dept_id
            , string p_str_store_id, string p_str_Note, string p_str_shipto_id, string p_str_Attn, string p_str_dc_id, string p_str_Mailname, string p_str_Addr1
            , string p_str_Addr2, string p_str_city, string p_str_state, string p_str_zipcode, string p_str_country, string p_str_radio)
        {
            int totorder = 0;
            int OrderQty = 0;
            decimal listprice = 0;
            int dtlline = 0;
            string l_str_List_uom = String.Empty;
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objOutboundInq.cmp_id = p_str_cmp_id;//CR2018-03-08-001 Added By Nithya
            objOutboundInq.Sonum = p_str_Sonum;
            objOutboundInq = ServiceObject.GetShipReqEntryTempGridDtl(objOutboundInq);
            if (objOutboundInq.ListItmStyledtl.Count == 0)//CR_MVC_3PL_20180411-001 Added By NIthya
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            for (int i = 0; i < objOutboundInq.ListItmStyledtl.Count(); i++)
            {
                dtlline = i + 1;
                objOutboundInq.cmp_id = p_str_cmp_id;
                objOutboundInq.Sonum = p_str_Sonum;
                objOutboundInq.line_num = objOutboundInq.ListItmStyledtl[i].line_num;
                objOutboundInq.itm_line = dtlline;
                objOutboundInq.po_num = objOutboundInq.ListItmStyledtl[i].po_num;
                objOutboundInq.itm_code = objOutboundInq.ListItmStyledtl[i].itm_code;
                objOutboundInq.itm_num = objOutboundInq.ListItmStyledtl[i].itm_num;
                objOutboundInq.itm_color = objOutboundInq.ListItmStyledtl[i].itm_color;
                objOutboundInq.itm_size = objOutboundInq.ListItmStyledtl[i].itm_size;
                objOutboundInq.ordr_qty = objOutboundInq.ListItmStyledtl[i].ordr_qty;
                OrderQty = objOutboundInq.ordr_qty;
                totorder = totorder + OrderQty;
                objOutboundInq.ordr_ctns = objOutboundInq.ListItmStyledtl[i].ordr_ctns;
                objOutboundInq.qty_uom = objOutboundInq.ListItmStyledtl[i].qty_uom;
                objOutboundInq.list_price = objOutboundInq.ListItmStyledtl[i].list_price;
                listprice = objOutboundInq.list_price;
                objOutboundInq.list_uom = objOutboundInq.ListItmStyledtl[i].list_uom;
                objOutboundInq.sell_price = objOutboundInq.ListItmStyledtl[i].list_price;
                l_str_List_uom = objOutboundInq.ListItmStyledtl[i].list_uom;
                if (l_str_List_uom != null)
                {
                    objOutboundInq.sell_uom = objOutboundInq.ListItmStyledtl[i].list_uom.Trim();

                }
                else
                {
                    objOutboundInq.sell_uom = "";
                }
                objOutboundInq.pack_id = objOutboundInq.ListItmStyledtl[i].pack_id;
                objOutboundInq.comm_pcnt = "5";
                objOutboundInq.cube = objOutboundInq.ListItmStyledtl[i].cube;
                objOutboundInq.Size = objOutboundInq.ListItmStyledtl[i].Size;
                objOutboundInq.wgt = objOutboundInq.ListItmStyledtl[i].wgt;
                objOutboundInq.itm_qty = objOutboundInq.ListItmStyledtl[i].ctn_qty;
                objOutboundInq.ctn_qty = objOutboundInq.ListItmStyledtl[i].ctn_qty;
                objOutboundInq.aloc_qty = 0;
                objOutboundInq.ship_qty = 0;
                objOutboundInq.back_ordr_qty = 0;
                objOutboundInq.dept_id = p_str_dept_id;
                objOutboundInq.store_id = p_str_store_id;
                objOutboundInq.ShipDt = p_str_ShipDt;
                objOutboundInq.CancelDt = p_str_CancelDt;
                objOutboundInq.length = objOutboundInq.ListItmStyledtl[i].length;
                objOutboundInq.width = objOutboundInq.ListItmStyledtl[i].width;
                objOutboundInq.depth = objOutboundInq.ListItmStyledtl[i].depth;
                objOutboundInq.po_num = objOutboundInq.ListItmStyledtl[i].po_num;
                objOutboundInq.note = objOutboundInq.ListItmStyledtl[i].note;
                objOutboundInq.due_dt = DateTime.Now.ToString("MM/dd/yyyy");
                objOutboundInq.avail_dt = DateTime.Now.AddDays(-5).ToString("MM-dd-yyyy");
                objOutboundInq.aloc_dt = DateTime.Now.AddDays(-4).ToString("MM-dd-yyyy");
                objOutboundInq.pick_dt = DateTime.Now.AddDays(-3).ToString("MM-dd-yyyy");
                objOutboundInq.load_dt = DateTime.Now.AddDays(-2).ToString("MM-dd-yyyy");
                objOutboundInq.kit_itm = objOutboundInq.ListItmStyledtl[i].kit_itm;
                objOutboundInq.cust_itm_num = objOutboundInq.ListItmStyledtl[i].cust_itm_num;
                objOutboundInq.cust_itm_color = objOutboundInq.ListItmStyledtl[i].cust_itm_color;
                objOutboundInq.cust_itm_desc = objOutboundInq.ListItmStyledtl[i].cust_itm_desc;
                objOutboundInq.fob = p_str_fob;
                ServiceObject.Add_To_proc_save_so_dtl_excel(objOutboundInq);
                ServiceObject.Add_To_proc_save_so_dtl_due_excel(objOutboundInq);
            }
            objOutboundInq.cmp_id = p_str_cmp_id;
            objOutboundInq.Sonum = p_str_Sonum;
            objOutboundInq.pricetkt = p_str_pricetkt;
            objOutboundInq.So_dt = p_str_so_dt;
            objOutboundInq.OrdType = p_str_Type;
            objOutboundInq.cust_id = p_str_cust_id;
            //CR-180402-001 Added By Nithya
            if (objOutboundInq.cust_id == "" || objOutboundInq.cust_id == null)
            {
                objOutboundInq.cust_id = "-";
            }
            objOutboundInq.CustName = "-";
            objOutboundInq.ordr_num = p_str_refno;
            objOutboundInq.CustPO = p_str_CustPO;
            objOutboundInq.CustOrderdt = p_str_CustOrderdt;
            objOutboundInq.AuthId = p_str_AuthId;
            objOutboundInq.dept_id = p_str_dept_id;
            objOutboundInq.store_id = p_str_store_id;
            objOutboundInq.shipvia_id = p_str_ShipVia;
            objOutboundInq.freight_id = p_str_freight_id;
            objOutboundInq.shipchrg = p_str_shipchrg;
            objOutboundInq.ShipDt = p_str_ShipDt;
            objOutboundInq.CancelDt = p_str_CancelDt;
            objOutboundInq.note = p_str_Note;
            objOutboundInq.total_qty = totorder;
            objOutboundInq.dc_id = p_str_dc_id;
            objOutboundInq.shipto_id = p_str_shipto_id;
            objOutboundInq.Attn = p_str_Attn;
            objOutboundInq.Mailname = p_str_Mailname;
            objOutboundInq.Addr1 = p_str_Addr1;
            objOutboundInq.Addr2 = p_str_Addr2;
            objOutboundInq.ShipToZipCode = p_str_zipcode;
            objOutboundInq.ShipToCity = p_str_city;
            objOutboundInq.ShipToState = p_str_state;
            objOutboundInq.ShipToCountry = p_str_country;
            objOutboundInq.fob = p_str_fob;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            //objOutboundInq.itm_line = l_str_itm_line;
            //objOutboundInq.Itmline = l_str_itm_line+1;
            objOutboundInq.user_id = objCompany.user_id;
            objOutboundInq.quote_num = p_str_Sonum;
            objOutboundInq = ServiceObject.GetOutSaleId(objOutboundInq);
            objOutboundInq.out_sale_id = (objOutboundInq.out_sale_id == null || objOutboundInq.ListGetOutSaleId[0].out_sale_id.Trim() == "") ? objOutboundInq.out_sale_id : null;
            //objOutboundInq.ListGetOutSaleId[0].out_sale_id;
            if (p_str_radio == "GridShippermanant")
            {
                ServiceObject.GetShipToAddressSave(objOutboundInq);
            }
            ServiceObject.Add_To_proc_save_so_hdr_excel(objOutboundInq);
            ServiceObject.Add_To_proc_save_so_addr_excel(objOutboundInq);
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_ShipReqEntry", objOutboundInqModel);
        }
        public ActionResult SaveShipReqEntryEdit(string p_str_cmp_id, string p_str_cust_id, string p_str_Sonum, string p_str_pricetkt, string p_str_so_dt,
        string p_str_ShipDt, string p_str_CancelDt, string p_str_refno, string p_str_freight_id, string p_str_status, string p_str_Type, string p_str_AuthId
         , string p_str_fob, string p_str_ShipVia, string p_str_shipchrg, string p_str_CustPO, string p_str_CustOrderdt, string p_str_dept_id
         , string p_str_store_id, string p_str_Note, string p_str_shipto_id, string p_str_Attn, string p_str_dc_id, string p_str_Mailname, string p_str_Addr1
         , string p_str_Addr2, string p_str_city, string p_str_state, string p_str_zipcode, string p_str_country, string p_str_radio,string p_str_quote_num)
        {
            int totorder = 0;
            int OrderQty = 0;
            decimal listprice = 0;
            int dtlline = 0;
            string l_str_List_uom = String.Empty;
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objOutboundInq.cmp_id = p_str_cmp_id;//CR2018-03-08-001 Added By Nithya
            objOutboundInq.Sonum = p_str_Sonum;
            objOutboundInq = ServiceObject.GetShipReqEntryTempGridDtl(objOutboundInq);
            if (objOutboundInq.ListItmStyledtl.Count == 0)//CR_MVC_3PL_20180411-001 Added By NIthya
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            ServiceObject.DelSo_dtl_Due_Table(objOutboundInq);//CR_MVC_3PL_20180315-001 Added By NIthya          
            for (int i = 0; i < objOutboundInq.ListItmStyledtl.Count(); i++)
            {
                dtlline = i + 1;
                objOutboundInq.cmp_id = p_str_cmp_id;
                objOutboundInq.Sonum = p_str_Sonum;
                objOutboundInq.line_num = objOutboundInq.ListItmStyledtl[i].line_num;
                objOutboundInq.itm_line = dtlline;
                objOutboundInq.po_num = objOutboundInq.ListItmStyledtl[i].po_num;
                objOutboundInq.itm_code = objOutboundInq.ListItmStyledtl[i].itm_code;
                objOutboundInq.itm_num = objOutboundInq.ListItmStyledtl[i].itm_num;
                objOutboundInq.itm_color = objOutboundInq.ListItmStyledtl[i].itm_color;
                objOutboundInq.itm_size = objOutboundInq.ListItmStyledtl[i].itm_size;
                objOutboundInq.ordr_qty = objOutboundInq.ListItmStyledtl[i].ordr_qty;
                OrderQty = objOutboundInq.ordr_qty;
                totorder = totorder + OrderQty;
                objOutboundInq.ordr_ctns = objOutboundInq.ListItmStyledtl[i].ordr_ctns;
                objOutboundInq.qty_uom = objOutboundInq.ListItmStyledtl[i].qty_uom;
                objOutboundInq.list_price = objOutboundInq.ListItmStyledtl[i].list_price;
                listprice = objOutboundInq.list_price;
                objOutboundInq.list_uom = objOutboundInq.ListItmStyledtl[i].list_uom;
                objOutboundInq.sell_price = objOutboundInq.ListItmStyledtl[i].list_price;
                l_str_List_uom = objOutboundInq.ListItmStyledtl[i].list_uom;
                if (l_str_List_uom != null)
                {
                    objOutboundInq.sell_uom = objOutboundInq.ListItmStyledtl[i].list_uom.Trim();

                }
                else
                {
                    objOutboundInq.sell_uom = "";
                }
                objOutboundInq.pack_id = objOutboundInq.ListItmStyledtl[i].pack_id;
                objOutboundInq.comm_pcnt = "5";
                objOutboundInq.cube = objOutboundInq.ListItmStyledtl[i].cube;
                objOutboundInq.Size = objOutboundInq.ListItmStyledtl[i].Size;
                objOutboundInq.wgt = objOutboundInq.ListItmStyledtl[i].wgt;
                objOutboundInq.itm_qty = objOutboundInq.ListItmStyledtl[i].ctn_qty;
                objOutboundInq.ctn_qty = objOutboundInq.ListItmStyledtl[i].ctn_qty;
                objOutboundInq.aloc_qty = 0;
                objOutboundInq.ship_qty = 0;
                objOutboundInq.back_ordr_qty = 0;
                objOutboundInq.dept_id = p_str_dept_id;
                objOutboundInq.store_id = p_str_store_id;
                objOutboundInq.ShipDt = p_str_ShipDt;
                objOutboundInq.CancelDt = p_str_CancelDt;
                objOutboundInq.length = objOutboundInq.ListItmStyledtl[i].length;
                objOutboundInq.width = objOutboundInq.ListItmStyledtl[i].width;
                objOutboundInq.depth = objOutboundInq.ListItmStyledtl[i].depth;
                objOutboundInq.po_num = objOutboundInq.ListItmStyledtl[i].po_num;
                objOutboundInq.note = objOutboundInq.ListItmStyledtl[i].note;
                objOutboundInq.due_dt = DateTime.Now.ToString("MM/dd/yyyy");
                objOutboundInq.avail_dt = DateTime.Now.AddDays(-5).ToString("MM-dd-yyyy");
                objOutboundInq.aloc_dt = DateTime.Now.AddDays(-4).ToString("MM-dd-yyyy");
                objOutboundInq.pick_dt = DateTime.Now.AddDays(-3).ToString("MM-dd-yyyy");
                objOutboundInq.load_dt = DateTime.Now.AddDays(-2).ToString("MM-dd-yyyy");
                objOutboundInq.kit_itm = objOutboundInq.ListItmStyledtl[i].kit_itm;
                objOutboundInq.cust_itm_num = objOutboundInq.ListItmStyledtl[i].cust_itm_num;
                objOutboundInq.cust_itm_color = objOutboundInq.ListItmStyledtl[i].cust_itm_color;
                objOutboundInq.cust_itm_desc = objOutboundInq.ListItmStyledtl[i].cust_itm_desc;
                objOutboundInq.fob = p_str_fob;
                ServiceObject.Add_To_proc_save_so_dtl_excel(objOutboundInq);
                ServiceObject.Add_To_proc_save_so_dtl_due_excel(objOutboundInq);
                //ServiceObject.Update_To_proc_save_so_dtl_due(objOutboundInq);
            }
            objOutboundInq.cmp_id = p_str_cmp_id;
            objOutboundInq.Sonum = p_str_Sonum;
            objOutboundInq.pricetkt = p_str_pricetkt;
            objOutboundInq.So_dt = p_str_so_dt;
            objOutboundInq.OrdType = p_str_Type;
            objOutboundInq.cust_id = p_str_cust_id;
            //CR-180402-001 Added By Nithya
            if (objOutboundInq.cust_id == "" || objOutboundInq.cust_id == null)
            {
                objOutboundInq.cust_id = "-";
            }
            objOutboundInq.CustName = "-";
            objOutboundInq.ordr_num = p_str_refno;
            objOutboundInq.CustPO = p_str_CustPO;
            objOutboundInq.CustOrderdt = p_str_CustOrderdt;
            objOutboundInq.AuthId = p_str_AuthId;
            objOutboundInq.dept_id = p_str_dept_id;
            objOutboundInq.store_id = p_str_store_id;
            objOutboundInq.shipvia_id = p_str_ShipVia;
            objOutboundInq.freight_id = p_str_freight_id;
            objOutboundInq.shipchrg = p_str_shipchrg;
            objOutboundInq.ShipDt = p_str_ShipDt;
            objOutboundInq.CancelDt = p_str_CancelDt;
            objOutboundInq.note = p_str_Note;
            objOutboundInq.total_qty = totorder;
            objOutboundInq.dc_id = p_str_dc_id;
            objOutboundInq.shipto_id = p_str_shipto_id;
            objOutboundInq.Attn = p_str_Attn;
            objOutboundInq.Mailname = p_str_Mailname;
            objOutboundInq.Addr1 = p_str_Addr1;
            objOutboundInq.Addr2 = p_str_Addr2;
            objOutboundInq.ShipToZipCode = p_str_zipcode;
            objOutboundInq.ShipToCity = p_str_city;
            objOutboundInq.ShipToState = p_str_state;
            objOutboundInq.ShipToCountry = p_str_country;
            objOutboundInq.fob = p_str_fob;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            //objOutboundInq.itm_line = l_str_itm_line;
            //objOutboundInq.Itmline = l_str_itm_line+1;
            objOutboundInq.user_id = objCompany.user_id;
            objOutboundInq.quote_num = p_str_quote_num;      
            objOutboundInq = ServiceObject.GetOutSaleId(objOutboundInq);
            objOutboundInq.out_sale_id = (objOutboundInq.out_sale_id == null || objOutboundInq.ListGetOutSaleId[0].out_sale_id.Trim() == "") ? objOutboundInq.out_sale_id : null;
            //objOutboundInq.ListGetOutSaleId[0].out_sale_id;
            if (p_str_radio == "GridShippermanant")
            {
                ServiceObject.GetShipToAddressSave(objOutboundInq);
            }
            ServiceObject.Update_To_proc_save_so_hdr(objOutboundInq);
            ServiceObject.Update_To_proc_save_so_addr(objOutboundInq);
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_ShipReqEntry", objOutboundInqModel);
        }
        public ActionResult SRDeleteGridData(string p_str_so_num, string p_str_cmp_id, string p_str_itm_code, string p_str_LineNum, string p_str_shpiEntry)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.cmp_id = p_str_cmp_id;
            objOutboundInq.Sonum = p_str_so_num;
            objOutboundInq.itm_code = p_str_itm_code;
            objOutboundInq.LineNum = Convert.ToInt32(p_str_LineNum);
            objOutboundInq.ShipEntry = p_str_shpiEntry;
            objOutboundInq = ServiceObject.GetGridDeleteData(objOutboundInq);
            //Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            //OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            //objEShip.ctn_cube = l_dec_cube;
            //OutboundInqModel.[0].ctn_cube = OutboundInqModel.ctn_cube;
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_ShipEntryGrid", objOutboundInqModel);
        }

        public JsonResult SREditDisplayGridToTextbox(string p_str_so_num, string p_str_cmp_id, string p_str_itm_code, string p_str_LineNum, bool l_bool_is_in_edit)//CR_MVC_3PL_0320-001 Added By Nithya
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            Session["l_bool_edit_flag"] = 0;//CR_MVC_3PL_0317-001 Added By Nithya
            objOutboundInq.cmp_id = p_str_cmp_id;
            objOutboundInq.so_num = p_str_so_num;
            objOutboundInq.itm_code = p_str_itm_code;
            objOutboundInq.LineNum = Convert.ToInt32(p_str_LineNum);
            //objOutboundInq.ShipEntry = p_str_shpiEntry;
            objOutboundInq = ServiceObject.GetGridEditData(objOutboundInq);
            //objPick = ServiceObjectPick.GetPickUom(objPick);
            //objOutboundInq.qty_uom = objOutboundInq.ListGridEditData[0].qty_uom.ToString().Trim();
            //objPick.ListUomPick[0].qty_uom = (objOutboundInq.ListUomPick[0].qty_uom.ToString().Trim());
            objOutboundInq = ServiceObject.LoadAvailQty(objOutboundInq);
            ServiceObject.DeleteTempshipEntry(objOutboundInq);
            Session["l_bool_edit_flag"] = l_bool_is_in_edit;
            objOutboundInq.l_bool_edit_flag = Session["l_bool_edit_flag"].ToString();//CR_MVC_3PL_0320-001 Added By Nithya
            // objOutboundInq.avail_qty = objOutboundInq.LstItmxCustdtl[0].avail_qty;
            //Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            //OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            //objEShip.ctn_cube = l_dec_cube;
            //OutboundInqModel.[0].ctn_cube = OutboundInqModel.ctn_cube;
            //return Json(new
            //{
            //    objOutboundInq.ListGridEditData,
            //    objOutboundInq.LstItmxCustdtl
            //}, JsonRequestBehavior.AllowGet);
            return Json(objOutboundInq, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]

        public ActionResult OutboundShipEntry(HttpPostedFileBase FileUpload)
        {
            OutboundInqModel objOutboundInqModel = new OutboundInqModel();
            IOutboundInqService ServiceObject = new OutboundInqService();

            // Set up DataTable place holder
            DataTable dt = new DataTable();

            //check we have a file
            if (FileUpload != null)
            {
                if (FileUpload.ContentLength > 0)
                {
                    //Workout our file path
                    string fileName = Path.GetFileName(FileUpload.FileName);
                    string path = Path.Combine(Server.MapPath("~/uploads"), fileName);

                    //Try and upload
                    try
                    {
                        FileUpload.SaveAs(path);
                        //Process the CSV file and capture the results to our DataTable place holder
                        dt = ProcessCSV(path);

                        //Process the DataTable and capture the results to our SQL Bulk copy
                        ViewData["Feedback"] = ProcessBulkCopy(dt);
                    }
                    catch (Exception ex)
                    {
                        //Catch errors
                        ViewData["Feedback"] = ex.Message;
                    }
                }
            }
            else
            {
                //Catch errors
                ViewData["Feedback"] = "Please select a file";
            }

            //Tidy up
            dt.Dispose();

            OutboundInq objOutboundInqs = new OutboundInq();
            IOutboundInqService ServiceObjects = new OutboundInqService();
            Mapper.CreateMap<OutboundInqModel, OutboundInq>();
            OutboundInq objOutboundInq = Mapper.Map<OutboundInqModel, OutboundInq>(objOutboundInqModel);

            ServiceObject.GetCSVList(objOutboundInq);
            for (int m = 0; m < objOutboundInq.lstobjOutboundInq.Count(); m++)
            {
                int linenum = 0;
                linenum = m + 1;
                decimal orderQty, ShipEntryPPk = 0;
                objOutboundInq.LineNum = objOutboundInq.lstobjOutboundInq[m].rowno; ;
                objOutboundInq.cmp_id = Session["tempcmpid"].ToString().Trim();
                objOutboundInq.Sonum = Session["tempSonum"].ToString().Trim();
                objOutboundInq = ServiceObject.GetSRIdDtl(objOutboundInq);
                objOutboundInq.id = objOutboundInq.ListSRDocId[0].id;
                objOutboundInq.STATUS = Session["tempStatus"].ToString().Trim();
                objOutboundInq.itm_num = objOutboundInq.lstobjOutboundInq[m].style.Trim();
                objOutboundInq.itm_color = objOutboundInq.lstobjOutboundInq[m].color.Trim();
                objOutboundInq.itm_size = objOutboundInq.lstobjOutboundInq[m].size.Trim();
                objOutboundInq.Note = objOutboundInq.lstobjOutboundInq[m].Notes.Trim();
                objOutboundInq.itm_name = objOutboundInq.lstobjOutboundInq[m].Name.Trim();
                if (objOutboundInq.itm_name == "")
                {
                    objOutboundInq.itm_name = "";
                }

                objOutboundInq.OrdQty = objOutboundInq.lstobjOutboundInq[m].orderqty;
                orderQty = objOutboundInq.OrdQty;
                objOutboundInq.ppk = objOutboundInq.lstobjOutboundInq[m].shipppk;
                ShipEntryPPk = objOutboundInq.ppk;
                objOutboundInq.note = objOutboundInq.lstobjOutboundInq[m].Notes;
                objOutboundInq.po_num = objOutboundInq.lstobjOutboundInq[m].ponum;

                if (objOutboundInq.lstobjOutboundInq.Count >= 2)
                {
                    objOutboundInq.OrdQty = (objOutboundInq.ctn * ShipEntryPPk);
                }
                else
                {
                    objOutboundInq.ctn = Math.Ceiling((orderQty) / (ShipEntryPPk));
                }
                // objOutboundInq.OrdQty = (orderQty * ShipEntryPPk);        
                objOutboundInq.ctn = Math.Ceiling((orderQty) / (ShipEntryPPk));
                objOutboundInq.pack_id = "PF";
                objOutboundInq.comm_pcnt = "5";
                objOutboundInq.cust_id = "-";
                objOutboundInq = ServiceObject.Getkitflag(objOutboundInq);
                objOutboundInq.kit_itm = objOutboundInq.ListItmStyledtl[0].kit_itm.Trim();
                objOutboundInq.itm_code = objOutboundInq.ListItmStyledtl[0].itm_code;
                objOutboundInq = ServiceObject.GetCustDtl(objOutboundInq);
                if (objOutboundInq.LstItmxCustdtl.Count == 0)
                {
                    objOutboundInq.cust_itm_num = "-";
                    objOutboundInq.cust_itm_color = "-";
                    objOutboundInq.cust_itm_desc = "-";
                }
                else
                {
                    objOutboundInq.cust_itm_num = objOutboundInq.LstItmxCustdtl[0].cust_itm_num;
                    objOutboundInq.cust_itm_color = objOutboundInq.LstItmxCustdtl[0].cust_itm_color;
                    objOutboundInq.cust_itm_desc = objOutboundInq.LstItmxCustdtl[0].cust_itm_desc;
                }

                objOutboundInq = ServiceObject.Getitmlist(objOutboundInq);
                objOutboundInq.weight = objOutboundInq.LstItmxCustdtl[0].wgt;
                objOutboundInq.Cube = Convert.ToString(objOutboundInq.LstItmxCustdtl[0].cube);
                objOutboundInq.length = objOutboundInq.LstItmxCustdtl[0].length;
                objOutboundInq.width = objOutboundInq.LstItmxCustdtl[0].width;
                objOutboundInq.height = objOutboundInq.LstItmxCustdtl[0].depth;
                objOutboundInq.list_price = objOutboundInq.LstItmxCustdtl[0].list_price;
                objOutboundInq.qty_uom = objOutboundInq.LstItmxCustdtl[0].qty_uom;
                if (objOutboundInq.qty_uom == "")
                {
                    objOutboundInq.qty_uom = "EA";
                }
                objOutboundInq.price_uom = objOutboundInq.LstItmxCustdtl[0].price_uom;
                if (objOutboundInq.price_uom == "")
                {
                    objOutboundInq.price_uom = "EA";
                }
                objOutboundInq.dtlsize = (objOutboundInq.length * objOutboundInq.width * objOutboundInq.height);
                //ordrcost = p_str_ord_qty + objOutboundInq.LstItmxCustdtl[0].list_price;
                objOutboundInq.Errdesc = "-";
                objOutboundInq.lot_id = "";
                ServiceObject.InsertTemptableValue(objOutboundInq);

            }
            objOutboundInq = ServiceObject.GetGridList(objOutboundInq);
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundShipModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_ShipEntryGrid", objOutboundShipModel);
        }
        private static DataTable ProcessCSV(string fileName)
        {
            OutboundInqModel objOutboundInqModel = new OutboundInqModel();
            IOutboundInqService ServiceObject = new OutboundInqService();

            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObjects = new OutboundInqService();
            Mapper.CreateMap<OutboundInqModel, OutboundInq>();
            OutboundInq objOutboundInqs = Mapper.Map<OutboundInqModel, OutboundInq>(objOutboundInqModel);

            ServiceObject.OutboundInqTempDelete(objOutboundInqs);
            //Set up our variables
            string Feedback = string.Empty;
            string line = string.Empty;
            string[] strArray;
            DataTable dt = new DataTable();
            DataRow row;
            // work out where we should split on comma, but not in a sentence
            Regex r = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            //Set the filename in to our stream
            StreamReader sr = new StreamReader(fileName);

            //Read the first line and split the string at , with our regular expression in to an array
            line = sr.ReadLine();
            strArray = r.Split(line);

            //For each item in the new split array, dynamically builds our Data columns. Save us having to worry about it.
            Array.ForEach(strArray, s => dt.Columns.Add(new DataColumn()));

            //Read each line in the CVS file until it’s empty
            while ((line = sr.ReadLine()) != null)
            {
                row = dt.NewRow();

                //add our current value to our data row
                row.ItemArray = r.Split(line);
                dt.Rows.Add(row);
            }
            //Tidy Streameader up
            sr.Dispose();
            //return a the new DataTable
            return dt;
        }
        private static String ProcessBulkCopy(DataTable dt)
        {
            string Feedback = string.Empty;
            string connString = ConfigurationManager.ConnectionStrings["GenSoftConnection"].ConnectionString;

            //make our connection and dispose at the end
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(conn))
                {
                    //Set the database table name.
                    sqlBulkCopy.DestinationTableName = "dbo.temp_ShipRequestUpload";

                    //[OPTIONAL]: Map the Excel columns with that of the database table
                    sqlBulkCopy.ColumnMappings.Add("Column1", "style");
                    sqlBulkCopy.ColumnMappings.Add("Column2", "color");
                    sqlBulkCopy.ColumnMappings.Add("Column3", "size");
                    sqlBulkCopy.ColumnMappings.Add("Column4", "Name");
                    sqlBulkCopy.ColumnMappings.Add("Column5", "ponum");
                    sqlBulkCopy.ColumnMappings.Add("Column6", "orderqty");
                    sqlBulkCopy.ColumnMappings.Add("Column7", "ppk");
                    sqlBulkCopy.ColumnMappings.Add("Column8", "Notes");
                    conn.Open();
                    sqlBulkCopy.WriteToServer(dt);
                    //var result = (from p in dt.AsEnumerable()
                    //              group p by p["ppk"]
                    //        into r
                    //              select new
                    //              {
                    //                  ID = r.Key,
                    //                  Percentage = r.Sum((s) => decimal.Parse(s["ppk"].ToString()))
                    //              }).ToList();
                    var customers = from customer in dt.AsEnumerable()
                                    where customer.Field<string>("Column7") == "1"
                                    select new
                                    {
                                        CustomerId = customer.Field<string>("Column1"),
                                        Name = customer.Field<string>("Column2"),
                                        Country = customer.Field<string>("Column3")
                                    };
                    if (customers.Count() == 0)
                    {
                        //TempData[TempDataKeys.Message] = "Some message";
                    }
                    else
                    {
                        Feedback = "Upload complete";
                        conn.Close();
                    }

                }
            }

            return Feedback;
        }
        [HttpPost]
        public ActionResult UserRegistration(OutboundInqModel myformdata)
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadFiles()
        {
            string l_str_itmcode;
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase FileUpload = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = FileUpload.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = FileUpload.FileName;
                        }

                        OutboundInqModel objOutboundInqModel = new OutboundInqModel();
                        IOutboundInqService ServiceObject = new OutboundInqService();

                        // Set up DataTable place holder
                        DataTable dt = new DataTable();

                        //check we have a file
                        if (FileUpload != null)
                        {
                            if (FileUpload.ContentLength > 0)
                            {
                                //Workout our file path
                                string fileName = Path.GetFileName(FileUpload.FileName);
                                string path = Path.Combine(Server.MapPath("~/uploads"), fileName);
                                var ext = Path.GetExtension(fileName);
                                //Try and upload
                                if(ext==".csv")
                                {                            
                                try
                                {
                                    FileUpload.SaveAs(path);
                                    //Process the CSV file and capture the results to our DataTable place holder
                                    dt = ProcessCSV(path);

                                    //Process the DataTable and capture the results to our SQL Bulk copy
                                    ViewData["Feedback"] = ProcessBulkCopy(dt);
                                }
                                     
                                catch (Exception ex)
                                {
                                    //Catch errors
                                    ViewData["Feedback"] = ex.Message;
                                }
                                }
                                else
                                {
                                    int ResultCount = 1;
                                    return Json(ResultCount, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                        else
                        {
                            //Catch errors
                            ViewData["Feedback"] = "Please select a file";
                        }

                        //Tidy up
                        dt.Dispose();

                        OutboundInq objOutboundInqs = new OutboundInq();
                        IOutboundInqService ServiceObjects = new OutboundInqService();
                        Mapper.CreateMap<OutboundInqModel, OutboundInq>();
                        OutboundInq objOutboundInq = Mapper.Map<OutboundInqModel, OutboundInq>(objOutboundInqModel);

                        ServiceObject.GetCSVList(objOutboundInq);
                        for (int m = 0; m < objOutboundInq.lstobjOutboundInq.Count(); m++)
                        {
                            int linenum = 0;
                            linenum = m + 1;
                            decimal orderQty, ShipEntryPPk = 0;
                            objOutboundInq.LineNum = objOutboundInq.lstobjOutboundInq[m].rowno;
                            objOutboundInq.cmp_id = Session["tempcmpid"].ToString().Trim();
                            objOutboundInq.Sonum = Session["tempSonum"].ToString().Trim();
                            objOutboundInq = ServiceObject.GetSRIdDtl(objOutboundInq);
                            objOutboundInq.id = objOutboundInq.ListSRDocId[0].id;
                            objOutboundInq.STATUS = Session["tempStatus"].ToString().Trim();
                            objOutboundInq.itm_num = objOutboundInq.lstobjOutboundInq[m].style.Trim();
                            objOutboundInq.itm_color = objOutboundInq.lstobjOutboundInq[m].color.Trim();
                            objOutboundInq.itm_size = objOutboundInq.lstobjOutboundInq[m].size.Trim();
                            objOutboundInq.Note = objOutboundInq.lstobjOutboundInq[m].Notes.Trim();
                            objOutboundInq.itm_name = objOutboundInq.lstobjOutboundInq[m].Name.Trim();
                            if (objOutboundInq.itm_name == "")
                            {
                                objOutboundInq.itm_name = "";
                            }

                            objOutboundInq.OrdQty = objOutboundInq.lstobjOutboundInq[m].orderqty;
                            orderQty = objOutboundInq.OrdQty;
                            objOutboundInq.ppk = objOutboundInq.lstobjOutboundInq[m].ppk;
                            ShipEntryPPk = objOutboundInq.ppk;
                            objOutboundInq.note = objOutboundInq.lstobjOutboundInq[m].Notes;
                            objOutboundInq.po_num = objOutboundInq.lstobjOutboundInq[m].ponum;
                            objOutboundInq.ctn = Math.Ceiling((orderQty) / (ShipEntryPPk));
                            objOutboundInq.ctn = Math.Ceiling((orderQty) / (ShipEntryPPk));
                            objOutboundInq.pack_id = "PF";
                            objOutboundInq.comm_pcnt = "5";
                            objOutboundInq.cust_id = "-";
                            objOutboundInq = ServiceObject.Getkitflag(objOutboundInq);
                            if(objOutboundInq.ListItmStyledtl.Count>0)
                            {
                                objOutboundInq.kit_itm = objOutboundInq.ListItmStyledtl[0].kit_itm.Trim();
                                objOutboundInq.itm_code = objOutboundInq.ListItmStyledtl[0].itm_code;
                            }
                            else
                            {
                                objOutboundInq.kit_itm = "-";
                            }                           
                            objOutboundInq = ServiceObject.GetCustDtl(objOutboundInq);
                            if (objOutboundInq.LstItmxCustdtl.Count == 0)
                            {
                                objOutboundInq.cust_itm_num = "-";
                                objOutboundInq.cust_itm_color = "-";
                                objOutboundInq.cust_itm_desc = "-";
                            }
                            else
                            {
                                objOutboundInq.cust_itm_num = objOutboundInq.LstItmxCustdtl[0].cust_itm_num;
                                objOutboundInq.cust_itm_color = objOutboundInq.LstItmxCustdtl[0].cust_itm_color;
                                objOutboundInq.cust_itm_desc = objOutboundInq.LstItmxCustdtl[0].cust_itm_desc;
                            }
                            objOutboundInq = ServiceObject.Getitmlist(objOutboundInq);
                            if(objOutboundInq.LstItmxCustdtl.Count>0)
                            {
                                objOutboundInq.weight = objOutboundInq.LstItmxCustdtl[0].wgt;
                                objOutboundInq.Cube = Convert.ToString(objOutboundInq.LstItmxCustdtl[0].cube);
                                objOutboundInq.length = objOutboundInq.LstItmxCustdtl[0].length;
                                objOutboundInq.width = objOutboundInq.LstItmxCustdtl[0].width;
                                objOutboundInq.height = objOutboundInq.LstItmxCustdtl[0].depth;
                                objOutboundInq.list_price = objOutboundInq.LstItmxCustdtl[0].list_price;
                                objOutboundInq.qty_uom = objOutboundInq.LstItmxCustdtl[0].qty_uom;
                                objOutboundInq.price_uom = objOutboundInq.LstItmxCustdtl[0].price_uom;
                            }
                            else
                            {
                                InboundInquiry objInboundInquiry = new InboundInquiry();
                                InboundInquiryService objService = new InboundInquiryService();
                                string Cube = "0.005";
                                objInboundInquiry.cmp_id = objOutboundInq.cmp_id;
                                objInboundInquiry.itm_num = objOutboundInq.itm_num; 
                                objInboundInquiry.itm_color = objOutboundInq.itm_color;
                                objInboundInquiry.itm_size = objOutboundInq.itm_size;
                                objInboundInquiry.itm_name = objOutboundInq.itm_name;
                                objInboundInquiry.ctn_qty = Convert.ToInt32(objOutboundInq.ppk); //p_str_ctns;//CR20180531-001 Added By Nithya
                                objInboundInquiry.weight =2;
                                objInboundInquiry.length = 2;
                                objInboundInquiry.width = 2;
                                objInboundInquiry.height = 2;
                                objInboundInquiry.cube =Convert.ToDecimal(Cube);
                                objInboundInquiry = objService.GetItmId(objInboundInquiry);
                                objInboundInquiry.itmid = objInboundInquiry.itm_code;
                                l_str_itmcode = objInboundInquiry.itm_code;
                                objInboundInquiry.itm_code = l_str_itmcode;
                                objInboundInquiry.flag = "A";
                                objService.Add_Style_To_Itm_dtl(objInboundInquiry);
                                objService.Add_Style_To_Itm_hdr(objInboundInquiry);

                                objOutboundInq = ServiceObject.Getitmlist(objOutboundInq);
                                objOutboundInq.itm_code = l_str_itmcode;
                                objOutboundInq.weight = objOutboundInq.LstItmxCustdtl[0].wgt;
                                objOutboundInq.Cube = Convert.ToString(objOutboundInq.LstItmxCustdtl[0].cube);
                                objOutboundInq.length = objOutboundInq.LstItmxCustdtl[0].length;
                                objOutboundInq.width = objOutboundInq.LstItmxCustdtl[0].width;
                                objOutboundInq.height = objOutboundInq.LstItmxCustdtl[0].depth;
                                objOutboundInq.list_price = objOutboundInq.LstItmxCustdtl[0].list_price;
                                objOutboundInq.qty_uom = objOutboundInq.LstItmxCustdtl[0].qty_uom;
                                objOutboundInq.price_uom = objOutboundInq.LstItmxCustdtl[0].price_uom;
                            }            
                            if (objOutboundInq.qty_uom == "" || objOutboundInq.qty_uom ==null)
                            {
                                objOutboundInq.qty_uom = "EA";
                            }                           
                            if (objOutboundInq.price_uom == "" || objOutboundInq.price_uom == null)
                            {
                                objOutboundInq.price_uom = "EA";
                            }
                            objOutboundInq.dtlsize = (objOutboundInq.length * objOutboundInq.width * objOutboundInq.height);
                            //ordrcost = p_str_ord_qty + objOutboundInq.LstItmxCustdtl[0].list_price;
                            objOutboundInq.Errdesc = "-";
                            objOutboundInq.lot_id = "";
                            ServiceObject.InsertTemptableValue(objOutboundInq);

                        }
                        objOutboundInq = ServiceObject.GetGridList(objOutboundInq);
                        Mapper.CreateMap<OutboundInq, OutboundInqModel>();
                        OutboundInqModel objOutboundShipModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
                        return PartialView("_ShipEntryGrid", objOutboundShipModel);
                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        public ActionResult Views(string cmpid, string so_num)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.cmp_id = cmpid;
            objOutboundInq.so_num = so_num;
            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = cmpid;
            objPick.Whs_id = "";
            objPick.Whs_name = "";
            objPick = ServiceObjectPick.GetWhsPick(objPick);
            objOutboundInq.Sonum = so_num;
            ServiceObject.DeleteTempshipEntrytable(objOutboundInq);//CR-180402-001 Added By Nithya
            objOutboundInq.ListPick = objPick.ListPick;
            objOutboundInq.fob = objOutboundInq.ListPick[0].Whs_id.Trim();
            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objOutboundInq.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objOutboundInq.ListStatePick = objPick.ListStatePick;
            objPick = ServiceObjectPick.GetShipToPick(objPick);
            objOutboundInq.ListShipToPick = objPick.ListShipToPick;
            objPick = ServiceObjectPick.GetPickUom(objPick);
            //objOutboundInq.ListUomPick = objPick.ListUomPick;
            LookUp objLookUp = new LookUp();
            LookUpService ServiceObject1 = new LookUpService();
            objLookUp.id = "12";
            objLookUp.lookuptype = "OUTBOUNDSTATUS";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objOutboundInq.ListLookUpDtl = objLookUp.ListLookUpDtl;
            objLookUp.id = "13";
            objLookUp.lookuptype = "OUTBOUNDPRICETKT";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objOutboundInq.Listpricetkt = objLookUp.ListLookUpDtl;
            objLookUp.id = "14";
            objLookUp.lookuptype = "OUTBOUNDTYPE";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objOutboundInq.Listtype = objLookUp.ListLookUpDtl;
            objLookUp.id = "15";
            objLookUp.lookuptype = "OUTBOUNDFRIEGHTID";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objOutboundInq.Listfrieghtid = objLookUp.ListLookUpDtl;
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objOutboundInq.cmp_id = cmpid;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objOutboundInq.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objOutboundInq = ServiceObject.GetViewDetail(objOutboundInq);
            objOutboundInq.cmp_id = objOutboundInq.lstobjOutboundInq[0].cmp_id;
            objOutboundInq.cust_id = objOutboundInq.lstobjOutboundInq[0].cust_id;
            objOutboundInq.Sonum = objOutboundInq.lstobjOutboundInq[0].so_num;
            objOutboundInq.pricetkt = objOutboundInq.lstobjOutboundInq[0].price_tkt;
            objOutboundInq.so_dt = (objOutboundInq.lstobjOutboundInq[0].so_dt == null || objOutboundInq.lstobjOutboundInq[0].so_dt == "" ? string.Empty : Convert.ToDateTime(objOutboundInq.lstobjOutboundInq[0].so_dt).ToString("MM/dd/yyyy"));
            objOutboundInq.ShipDt = (objOutboundInq.lstobjOutboundInq[0].ship_dt == null || objOutboundInq.lstobjOutboundInq[0].ship_dt == "" ? string.Empty : Convert.ToDateTime(objOutboundInq.lstobjOutboundInq[0].ship_dt).ToString("MM/dd/yyyy"));
            objOutboundInq.CancelDt = (objOutboundInq.lstobjOutboundInq[0].cancel_dt == null || objOutboundInq.lstobjOutboundInq[0].cancel_dt == "" ? string.Empty : Convert.ToDateTime(objOutboundInq.lstobjOutboundInq[0].cancel_dt).ToString("MM/dd/yyyy"));
            objOutboundInq.refno = objOutboundInq.lstobjOutboundInq[0].ordr_num;
            objOutboundInq.freight_id = objOutboundInq.lstobjOutboundInq[0].freight_id;
            objOutboundInq.status = objOutboundInq.lstobjOutboundInq[0].status;
            objOutboundInq.Type = objOutboundInq.lstobjOutboundInq[0].ordr_type;
            objOutboundInq.AuthId = objOutboundInq.lstobjOutboundInq[0].in_sale_id;
            objOutboundInq.fob = objOutboundInq.lstobjOutboundInq[0].fob;
            objOutboundInq.ShipVia = objOutboundInq.lstobjOutboundInq[0].shipvia_id;
            objOutboundInq.shipchrg = objOutboundInq.lstobjOutboundInq[0].sh_chg;
            objOutboundInq.CustPO = objOutboundInq.lstobjOutboundInq[0].cust_ordr_num;
            objOutboundInq.CustOrderdt = (objOutboundInq.lstobjOutboundInq[0].cust_ordr_dt == null || objOutboundInq.lstobjOutboundInq[0].cust_ordr_dt == "" ? string.Empty : Convert.ToDateTime(objOutboundInq.lstobjOutboundInq[0].cust_ordr_dt).ToString("MM/dd/yyyy"));
            objOutboundInq.dept_id = objOutboundInq.lstobjOutboundInq[0].dept_id;
            objOutboundInq.store_id = objOutboundInq.lstobjOutboundInq[0].store_id;
            objOutboundInq.Note = objOutboundInq.lstobjOutboundInq[0].note;
            objOutboundInq = ServiceObject.GetViewAddrDetail(objOutboundInq);
            objOutboundInq.shipto_id = objOutboundInq.lstobjOutboundInq[0].shipto_id;
            objOutboundInq.Attn = objOutboundInq.lstobjOutboundInq[0].sl_attn;
            objOutboundInq.dc_id = objOutboundInq.lstobjOutboundInq[0].dc_id;
            objOutboundInq.Mailname = objOutboundInq.lstobjOutboundInq[0].sl_mail_name;
            objOutboundInq.Addr1 = objOutboundInq.lstobjOutboundInq[0].sl_addr_line1;
            objOutboundInq.Addr2 = objOutboundInq.lstobjOutboundInq[0].sl_addr_line2;
            objOutboundInq.city = objOutboundInq.lstobjOutboundInq[0].sl_city;
            objOutboundInq.state = objOutboundInq.lstobjOutboundInq[0].sl_state_id;
            objOutboundInq.zipcode = objOutboundInq.lstobjOutboundInq[0].sl_post_code;
            objOutboundInq.country = objOutboundInq.lstobjOutboundInq[0].sl_cntry_id;

            objOutboundInq = ServiceObject.GetViewDetailgrid(objOutboundInq);
            objOutboundInq.View_Flag = "V";
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundShipModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_ShipReqEntry", objOutboundShipModel);
        }
        public ActionResult Delete(string cmpid, string so_num)
        {

            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.cmp_id = cmpid;
            objOutboundInq.so_num = so_num;
            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = cmpid;
            objPick.Whs_id = "";
            objPick.Whs_name = "";
            objOutboundInq.cmp_id = cmpid;
            objOutboundInq.Sonum = so_num;
            ServiceObject.DeleteTempshipEntrytable(objOutboundInq);//CR-180402-001 Added By Nithya
            objPick = ServiceObjectPick.GetWhsPick(objPick);
            objOutboundInq.ListPick = objPick.ListPick;
            objOutboundInq.fob = objOutboundInq.ListPick[0].Whs_id.Trim();
            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objOutboundInq.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objOutboundInq.ListStatePick = objPick.ListStatePick;
            objPick = ServiceObjectPick.GetShipToPick(objPick);
            objOutboundInq.ListShipToPick = objPick.ListShipToPick;
            objPick = ServiceObjectPick.GetPickUom(objPick);
            //objOutboundInq.ListUomPick = objPick.ListUomPick;
            LookUp objLookUp = new LookUp();
            LookUpService ServiceObject1 = new LookUpService();
            objLookUp.id = "12";
            objLookUp.lookuptype = "OUTBOUNDSTATUS";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objOutboundInq.ListLookUpDtl = objLookUp.ListLookUpDtl;
            objLookUp.id = "13";
            objLookUp.lookuptype = "OUTBOUNDPRICETKT";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objOutboundInq.Listpricetkt = objLookUp.ListLookUpDtl;
            objLookUp.id = "14";
            objLookUp.lookuptype = "OUTBOUNDTYPE";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objOutboundInq.Listtype = objLookUp.ListLookUpDtl;
            objLookUp.id = "15";
            objLookUp.lookuptype = "OUTBOUNDFRIEGHTID";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objOutboundInq.Listfrieghtid = objLookUp.ListLookUpDtl;
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objOutboundInq.cmp_id = cmpid;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objOutboundInq.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objOutboundInq = ServiceObject.GetViewDetail(objOutboundInq);
            objOutboundInq.cmp_id = objOutboundInq.lstobjOutboundInq[0].cmp_id;
            objOutboundInq.cust_id = objOutboundInq.lstobjOutboundInq[0].cust_id;
            objOutboundInq.Sonum = objOutboundInq.lstobjOutboundInq[0].so_num;
            objOutboundInq.pricetkt = objOutboundInq.lstobjOutboundInq[0].price_tkt;
            objOutboundInq.so_dt = (objOutboundInq.lstobjOutboundInq[0].so_dt == null || objOutboundInq.lstobjOutboundInq[0].so_dt == "" ? string.Empty : Convert.ToDateTime(objOutboundInq.lstobjOutboundInq[0].so_dt).ToString("MM/dd/yyyy"));
            objOutboundInq.ShipDt = (objOutboundInq.lstobjOutboundInq[0].ship_dt == null || objOutboundInq.lstobjOutboundInq[0].ship_dt == "" ? string.Empty : Convert.ToDateTime(objOutboundInq.lstobjOutboundInq[0].ship_dt).ToString("MM/dd/yyyy"));
            objOutboundInq.CancelDt = (objOutboundInq.lstobjOutboundInq[0].cancel_dt == null || objOutboundInq.lstobjOutboundInq[0].cancel_dt == "" ? string.Empty : Convert.ToDateTime(objOutboundInq.lstobjOutboundInq[0].cancel_dt).ToString("MM/dd/yyyy"));
            objOutboundInq.refno = objOutboundInq.lstobjOutboundInq[0].ordr_num;
            objOutboundInq.freight_id = objOutboundInq.lstobjOutboundInq[0].freight_id;
            objOutboundInq.status = objOutboundInq.lstobjOutboundInq[0].status;
            objOutboundInq.Type = objOutboundInq.lstobjOutboundInq[0].ordr_type;
            objOutboundInq.AuthId = objOutboundInq.lstobjOutboundInq[0].in_sale_id;
            objOutboundInq.fob = objOutboundInq.lstobjOutboundInq[0].fob;
            objOutboundInq.ShipVia = objOutboundInq.lstobjOutboundInq[0].shipvia_id;
            objOutboundInq.shipchrg = objOutboundInq.lstobjOutboundInq[0].sh_chg;
            objOutboundInq.CustPO = objOutboundInq.lstobjOutboundInq[0].cust_ordr_num;
            objOutboundInq.CustOrderdt = (objOutboundInq.lstobjOutboundInq[0].cust_ordr_dt == null || objOutboundInq.lstobjOutboundInq[0].cust_ordr_dt == "" ? string.Empty : Convert.ToDateTime(objOutboundInq.lstobjOutboundInq[0].cust_ordr_dt).ToString("MM/dd/yyyy"));
            objOutboundInq.dept_id = objOutboundInq.lstobjOutboundInq[0].dept_id;
            objOutboundInq.store_id = objOutboundInq.lstobjOutboundInq[0].store_id;
            objOutboundInq.Note = objOutboundInq.lstobjOutboundInq[0].note;
            objOutboundInq = ServiceObject.GetViewAddrDetail(objOutboundInq);
            objOutboundInq.shipto_id = objOutboundInq.lstobjOutboundInq[0].shipto_id;
            objOutboundInq.Attn = objOutboundInq.lstobjOutboundInq[0].sl_attn;
            objOutboundInq.dc_id = objOutboundInq.lstobjOutboundInq[0].dc_id;
            objOutboundInq.Mailname = objOutboundInq.lstobjOutboundInq[0].sl_mail_name;
            objOutboundInq.Addr1 = objOutboundInq.lstobjOutboundInq[0].sl_addr_line1;
            objOutboundInq.Addr2 = objOutboundInq.lstobjOutboundInq[0].sl_addr_line2;
            objOutboundInq.city = objOutboundInq.lstobjOutboundInq[0].sl_city;
            objOutboundInq.state = objOutboundInq.lstobjOutboundInq[0].sl_state_id;
            objOutboundInq.zipcode = objOutboundInq.lstobjOutboundInq[0].sl_post_code;
            objOutboundInq.country = objOutboundInq.lstobjOutboundInq[0].sl_cntry_id;
            objOutboundInq = ServiceObject.GetViewDetailgrid(objOutboundInq);
            objOutboundInq.View_Flag = "D";
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundShipModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_ShipReqEntry", objOutboundShipModel);
        }
        public ActionResult ShipreqDelete(string p_str_cmp_id, string p_str_Sonum)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.cmp_id = p_str_cmp_id;
            objOutboundInq.so_num = p_str_Sonum;
            ServiceObject.DeleteShipEntry(objOutboundInq);
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundShipModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            int resultcount;
            resultcount = 1;
            return Json(resultcount, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(string cmpid, string so_num,string Batchid)
        {
            string l_str_uom = string.Empty;
            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = cmpid;
            objPick.Whs_id = "";
            objPick.Whs_name = "";
            int l_int_line_count = 0;//CR-180330-001 Added By Nithya
            objOutboundInq.cmp_id = cmpid;
            objOutboundInq.Sonum = so_num;
            ServiceObject.DeleteTempshipEntrytable(objOutboundInq);//CR-180402-001 Added By Nithya
            objPick = ServiceObjectPick.GetWhsPick(objPick);
            objOutboundInq.ListPick = objPick.ListPick;
            objOutboundInq.fob = objOutboundInq.ListPick[0].Whs_id.Trim();
            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objOutboundInq.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objOutboundInq.ListStatePick = objPick.ListStatePick;
            objPick = ServiceObjectPick.GetShipToPick(objPick);
            objOutboundInq.ListShipToPick = objPick.ListShipToPick;
            objPick = ServiceObjectPick.GetPickUom(objPick);
            //objOutboundInq.ListUomPick = objPick.ListUomPick;           
            l_str_uom = objOutboundInq.Check_existing_uom;

            if (l_str_uom != null)
            {
                objOutboundInq.ListUomPick = objPick.ListUomPick;
                l_str_uom = objOutboundInq.ListUomPick[0].qty_uom.Trim();

            }
            else
            {
                List<Pick> li = new List<Pick>();
                objPick.qty_uom = "EA";
                li.Add(objPick);
                objOutboundInq.ListUomPick = li;
            }
            objOutboundInq.cmp_id = cmpid;
            objOutboundInq.so_num = so_num;
            objOutboundInq = ServiceObject.GetSRIdDtl(objOutboundInq);
            objOutboundInq.id = objOutboundInq.ListSRDocId[0].id;
            if (objOutboundInq.id == null || objOutboundInq.id ==string.Empty)
            {
                objOutboundInq.id = "0";
            }
            objOutboundInq = ServiceObject.GetViewDetail(objOutboundInq);
            objOutboundInq.cmp_id = objOutboundInq.lstobjOutboundInq[0].cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objOutboundInq.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            LookUp objLookUp = new LookUp();
            LookUpService ServiceObject1 = new LookUpService();
            objOutboundInq.cust_id = objOutboundInq.lstobjOutboundInq[0].cust_id;
            objOutboundInq.Sonum = objOutboundInq.lstobjOutboundInq[0].so_num;
            objOutboundInq.pricetkt = objOutboundInq.lstobjOutboundInq[0].price_tkt;
            objLookUp.id = "13";
            objLookUp.lookuptype = "OUTBOUNDPRICETKT";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objOutboundInq.Listpricetkt = objLookUp.ListLookUpDtl;
            objOutboundInq.so_dt = (objOutboundInq.lstobjOutboundInq[0].so_dt == null || objOutboundInq.lstobjOutboundInq[0].so_dt == "" ? string.Empty : Convert.ToDateTime(objOutboundInq.lstobjOutboundInq[0].so_dt).ToString("MM/dd/yyyy"));
            objOutboundInq.ShipDt = (objOutboundInq.lstobjOutboundInq[0].ship_dt == null || objOutboundInq.lstobjOutboundInq[0].ship_dt == "" ? string.Empty : Convert.ToDateTime(objOutboundInq.lstobjOutboundInq[0].ship_dt).ToString("MM/dd/yyyy"));
            objOutboundInq.CancelDt = (objOutboundInq.lstobjOutboundInq[0].cancel_dt == null || objOutboundInq.lstobjOutboundInq[0].cancel_dt == "" ? string.Empty : Convert.ToDateTime(objOutboundInq.lstobjOutboundInq[0].cancel_dt).ToString("MM/dd/yyyy"));
            objOutboundInq.refno = (objOutboundInq.lstobjOutboundInq[0].ordr_num == null || objOutboundInq.lstobjOutboundInq[0].ordr_num == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].ordr_num);
            objOutboundInq.freight_id = (objOutboundInq.lstobjOutboundInq[0].freight_id == null || objOutboundInq.lstobjOutboundInq[0].freight_id == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].freight_id); 
            objLookUp.id = "15";
            objLookUp.lookuptype = "OUTBOUNDFRIEGHTID";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objOutboundInq.Listfrieghtid = objLookUp.ListLookUpDtl;
            objOutboundInq.status =( objOutboundInq.lstobjOutboundInq[0].status == null || objOutboundInq.lstobjOutboundInq[0].status == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].status);
            objLookUp.id = "12";
            objLookUp.lookuptype = "OUTBOUNDSTATUS";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objOutboundInq.ListLookUpDtl = objLookUp.ListLookUpDtl;
            objOutboundInq.Type = (objOutboundInq.lstobjOutboundInq[0].ordr_type == null || objOutboundInq.lstobjOutboundInq[0].ordr_type == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].ordr_type); 
            objLookUp.id = "14";
            objLookUp.lookuptype = "OUTBOUNDTYPE";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objOutboundInq.Listtype = objLookUp.ListLookUpDtl;
            objOutboundInq.AuthId = (objOutboundInq.lstobjOutboundInq[0].in_sale_id == null || objOutboundInq.lstobjOutboundInq[0].in_sale_id == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].in_sale_id);
            objOutboundInq.fob = (objOutboundInq.lstobjOutboundInq[0].fob == null || objOutboundInq.lstobjOutboundInq[0].fob == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].fob); 
            objOutboundInq.ShipVia = (objOutboundInq.lstobjOutboundInq[0].shipvia_id == null || objOutboundInq.lstobjOutboundInq[0].shipvia_id == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].shipvia_id);
            objOutboundInq.shipchrg =( objOutboundInq.lstobjOutboundInq[0].sh_chg == null || objOutboundInq.lstobjOutboundInq[0].sh_chg == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].sh_chg);
            objOutboundInq.quote_num = (objOutboundInq.lstobjOutboundInq[0].quote_num == null || objOutboundInq.lstobjOutboundInq[0].quote_num == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].quote_num);
            if (objOutboundInq.shipchrg== "0.0000")
            {
                objOutboundInq.shipchrg ="0.00";
            }
            else
            {
                objOutboundInq.shipchrg = objOutboundInq.shipchrg;
            }
            objOutboundInq.CustPO = (objOutboundInq.lstobjOutboundInq[0].cust_ordr_num == null || objOutboundInq.lstobjOutboundInq[0].cust_ordr_num == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].cust_ordr_num); 
            objOutboundInq.CustOrderdt = (objOutboundInq.lstobjOutboundInq[0].cust_ordr_dt == null || objOutboundInq.lstobjOutboundInq[0].cust_ordr_dt == "" ? string.Empty : Convert.ToDateTime(objOutboundInq.lstobjOutboundInq[0].cust_ordr_dt).ToString("MM/dd/yyyy"));
            if (objOutboundInq.CustOrderdt== "01/01/1900")
            {
                objOutboundInq.CustOrderdt = "";
            }
            else
            {
                objOutboundInq.CustOrderdt = objOutboundInq.CustOrderdt;
            }
            objOutboundInq.dept_id = (objOutboundInq.lstobjOutboundInq[0].dept_id == null || objOutboundInq.lstobjOutboundInq[0].dept_id == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].dept_id);
            objOutboundInq.store_id = (objOutboundInq.lstobjOutboundInq[0].store_id == null || objOutboundInq.lstobjOutboundInq[0].store_id == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].store_id);
            objOutboundInq.Note =( objOutboundInq.lstobjOutboundInq[0].note == null || objOutboundInq.lstobjOutboundInq[0].note == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].note);
            objOutboundInq = ServiceObject.GetViewAddrDetail(objOutboundInq);
            objOutboundInq.shipto_id = (objOutboundInq.lstobjOutboundInq[0].shipto_id == null || objOutboundInq.lstobjOutboundInq[0].shipto_id == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].shipto_id); 
            objOutboundInq.Attn = (objOutboundInq.lstobjOutboundInq[0].sl_attn == null || objOutboundInq.lstobjOutboundInq[0].sl_attn == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].sl_attn);
            objOutboundInq.dc_id = (objOutboundInq.lstobjOutboundInq[0].dc_id== null || objOutboundInq.lstobjOutboundInq[0].dc_id == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].dc_id);
            objOutboundInq.Mailname = (objOutboundInq.lstobjOutboundInq[0].sl_mail_name == null || objOutboundInq.lstobjOutboundInq[0].sl_mail_name == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].sl_mail_name);
            objOutboundInq.Addr1 =( objOutboundInq.lstobjOutboundInq[0].sl_addr_line1 == null || objOutboundInq.lstobjOutboundInq[0].sl_addr_line1 == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].sl_addr_line1);
            objOutboundInq.Addr2 = (objOutboundInq.lstobjOutboundInq[0].sl_addr_line2 == null || objOutboundInq.lstobjOutboundInq[0].sl_addr_line2 == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].sl_addr_line2);
            objOutboundInq.city = (objOutboundInq.lstobjOutboundInq[0].sl_city == null || objOutboundInq.lstobjOutboundInq[0].sl_city == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].sl_city);
            objOutboundInq.state = (objOutboundInq.lstobjOutboundInq[0].sl_state_id == null || objOutboundInq.lstobjOutboundInq[0].sl_state_id == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].sl_state_id);
            objOutboundInq.zipcode =( objOutboundInq.lstobjOutboundInq[0].sl_post_code == null || objOutboundInq.lstobjOutboundInq[0].sl_post_code == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].sl_post_code);
            objOutboundInq.country = (objOutboundInq.lstobjOutboundInq[0].sl_cntry_id == null || objOutboundInq.lstobjOutboundInq[0].sl_cntry_id == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].sl_cntry_id);
            objOutboundInq = ServiceObject.GetViewDetailgrid(objOutboundInq);
            objOutboundInq = ServiceObject.GetShipReqEditDtl(objOutboundInq);
            //CR-180330-001 Added By Nithya
            objOutboundInq = ServiceObject.GetSRGridRowCount(objOutboundInq);
            l_int_line_count = objOutboundInq.ListCheckExistStyle[0].SRRowcount;
            objOutboundInq.LineNum = l_int_line_count + 1;
            //END
            objOutboundInq.View_Flag = "M";
            objOutboundInq.quote_num = Batchid;
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundShipModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_ShipReqEntry", objOutboundShipModel);
        }
        //public ActionResult LoadctnDtl(string cmpid, string so_num)
        //{
        //}
        public JsonResult PickWhsDtl(string term, string cmp_id)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            Pick objPick = new Pick();
            PickService ServiceObject1 = new PickService();
            objPick.cmp_id = cmp_id.Trim();
            objPick.Whs_id = term.Trim();
            objPick.Whs_name = "";
            //objPick = ServiceObject1.GetWhsPick(objPick);

            var List = ServiceObject1.GetWhsPickDetails(term, cmp_id).ListPick.Select(x => new { label = x.Whs_id, value = x.Whs_id, Whs_name = x.Whs_name }).ToList();
            objOutboundInq.ListPick = objPick.ListPick;

            return Json(List, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult ItemXGetitmDtl(string term, string cmp_id)
        //{
        //    IStockInquiryService ServiceObject = new StockInquiryService();
        //    var List = ServiceObject.ItemXGetitmDetails(term, cmp_id).LstItmxCustdtl.Select(x => new { label = x.Itmdtl, value = x.itm_num, itm_num = x.itm_num, itm_color = x.itm_color, itm_size = x.itm_size, itm_name = x.itm_name }).ToList();
        //    return Json(List, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult ItemXGetitmDtl(string term, string cmp_id)
        {

            OutboundInqService ServiceObject = new OutboundInqService();
            var List = ServiceObject.ItemXGetitmDetails(term.Trim(), cmp_id).LstItmxCustdtl.Select(x => new { label = x.Itmdtl, value = x.itm_num, itm_num = x.itm_num, itm_color = x.itm_color, itm_size = x.itm_size, itm_name = x.itm_name, itm_code = x.itm_code }).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSRGridRowCount(string p_str_cmp_id, string p_str_so_num)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.cmp_id = p_str_cmp_id;
            objOutboundInq.so_num = p_str_so_num;
            objOutboundInq = ServiceObject.GetSRGridRowCount(objOutboundInq);
            objOutboundInq.SRRowcount = objOutboundInq.ListCheckExistStyle[0].SRRowcount;
            return Json(objOutboundInq.SRRowcount, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShipReqAllocation(string Id, string cmp_id, string p_str_batch_id)
        {

            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.Sonum = Id;
            objOutboundInq.aloc_dt = DateTime.Now.ToString("MM/dd/yyyy");
            ServiceObject.DeleteAUTOALOC(objOutboundInq);
            objOutboundInq = ServiceObject.OutboundShipAloc(objOutboundInq);
            for (int i = 0; i < objOutboundInq.lstShipAlocdtl.Count(); i++)
            {
                objOutboundInq.CompID = cmp_id;
                objOutboundInq.Compname = "";
                objOutboundInq.ShipReqIDFm = Id;
                objOutboundInq.ShipReqIDTo = Id;
                objOutboundInq.whsid = objOutboundInq.lstShipAlocdtl[i].fob;
                Whsid = objOutboundInq.whsid;
                objOutboundInq.Cust_id = objOutboundInq.lstShipAlocdtl[i].cust_id;
                custid = objOutboundInq.Cust_id;
                objOutboundInq.CustName = objOutboundInq.lstShipAlocdtl[i].cust_name;
                objOutboundInq.orderNo = objOutboundInq.lstShipAlocdtl[i].ordr_num;
                objOutboundInq.shipto_id = objOutboundInq.lstShipAlocdtl[i].shipto_id;
                objOutboundInq.lblshiptoid = objOutboundInq.lstShipAlocdtl[i].shipto_id;
                objOutboundInq.CustPO = objOutboundInq.lstShipAlocdtl[i].cust_ordr_num;
                objOutboundInq.CustOrderdt = Convert.ToString(objOutboundInq.lstShipAlocdtl[i].cust_ordr_dt);
                if (objOutboundInq.CustOrderdt == "" || objOutboundInq.CustOrderdt == null)
                {
                    objOutboundInq.CustOrderdt = "";
                    objOutboundInq.ShipDt = "";
                    objOutboundInq.CancelDt = "";
                }
                else
                {
                    objOutboundInq.CustOrderdt = Convert.ToString(objOutboundInq.lstShipAlocdtl[i].cust_ordr_dt);
                    objOutboundInq.ShipDt = Convert.ToString(objOutboundInq.lstShipAlocdtl[i].ship_dt);
                    objOutboundInq.CancelDt = Convert.ToString(objOutboundInq.lstShipAlocdtl[i].cancel_dt);
                }
            }
            objOutboundInq = ServiceObject.GetObalocIdDetail(objOutboundInq);
            objOutboundInq.AlocdocId = objOutboundInq.AlocdocId;
            objOutboundInq = ServiceObject.SoNumFrom_Validation(objOutboundInq);
            if (objOutboundInq.lstShipAlocdtl.Count > 0)
            {
                objOutboundInq.status = objOutboundInq.lstShipAlocdtl[0].status;

            }
            if (objOutboundInq.status != "O")
            {
                var l_str_Staus = "A";
                return Json(l_str_Staus, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                objCompany.user_id = Session["UserID"].ToString().Trim();
                objCompany.cmp_id = cmp_id;
                objCompany.whs_id = Whsid;
                objCompany.cust_cmp_id = custid;
                objCompany = ServiceObjectCompany.Validate_Company(objCompany);
                objOutboundInq.Lstcmpdtl = objCompany.Lstcmpdtl;
                if (objOutboundInq.Lstcmpdtl.Count > 0)
                {
                    objOutboundInq.Compname = objOutboundInq.Lstcmpdtl[0].cmp_name;
                }
                else
                {
                    objOutboundInq.Compname = "";
                }
                objCompany = ServiceObjectCompany.Validate_Cmp_whs(objCompany);
                objOutboundInq.Lstwhsdtl = objCompany.Lstwhsdtl;
                if (objOutboundInq.Lstwhsdtl.Count > 0)
                {
                    objOutboundInq.whsname = objOutboundInq.Lstwhsdtl[0].whs_name;
                }
                else
                {
                    objOutboundInq.whsname = "";
                }
                objCompany = ServiceObjectCompany.Validate_Customer(objCompany);
                objOutboundInq.Lstcustdtl = objCompany.Lstcustdtl;
                if (objOutboundInq.Lstcustdtl.Count > 0)
                {
                    objOutboundInq.cust_name = objOutboundInq.Lstcustdtl[0].cust_name;
                }
                else
                {
                    objOutboundInq.cust_name = "";
                }
            }
            objOutboundInq = ServiceObject.OutboundSoldtoId(objOutboundInq);
            if (objOutboundInq.lstShipAlocdtl.Count > 0)
            {
                objOutboundInq.lblshiptoid = objOutboundInq.lstShipAlocdtl[0].soldto_id;
            }
            objOutboundInq = ServiceObject.OutboundSelectionhdr(objOutboundInq);
            Aloc_line = 0;
            for (int j = 0; j < objOutboundInq.lstShipAlocdtl.Count(); j++)
            {
                Aloc_line = Aloc_line + 1;
                objOutboundInq.cmp_id = objOutboundInq.lstShipAlocdtl[j].cmp_id;
                objOutboundInq.so_num = objOutboundInq.lstShipAlocdtl[j].so_num;
                objOutboundInq.line_num = objOutboundInq.lstShipAlocdtl[j].line_num;
                objOutboundInq.itm_code = objOutboundInq.lstShipAlocdtl[j].itm_code;
                objOutboundInq.qty_uom = objOutboundInq.lstShipAlocdtl[j].qty_uom;
                objOutboundInq.aloc_qty = objOutboundInq.lstShipAlocdtl[j].aloc_qty;
                objOutboundInq.due_qty = objOutboundInq.lstShipAlocdtl[j].due_qty;                   
                objOutboundInq.back_ordr_qty = objOutboundInq.lstShipAlocdtl[j].back_ordr_qty;
                objOutboundInq.dtl_line = objOutboundInq.lstShipAlocdtl[j].dtl_line;
                objOutboundInq.due_line = objOutboundInq.lstShipAlocdtl[j].due_line;
                objOutboundInq.itm_num = objOutboundInq.lstShipAlocdtl[j].itm_num;
                objOutboundInq.itm_color = objOutboundInq.lstShipAlocdtl[j].itm_color;
                objOutboundInq.itm_size = objOutboundInq.lstShipAlocdtl[j].itm_size;
                objOutboundInq.store_id = objOutboundInq.lstShipAlocdtl[j].store_id;
                objOutboundInq.whs_id = objOutboundInq.lstShipAlocdtl[j].whs_id;
                objOutboundInq.cust_id = objOutboundInq.lstShipAlocdtl[j].cust_id;
                objOutboundInq.shipto_id = objOutboundInq.lstShipAlocdtl[0].shipto_id;
                objOutboundInq.dc_id = objOutboundInq.lstShipAlocdtl[j].dc_id;
                objOutboundInq.status = objOutboundInq.lstShipAlocdtl[j].status;
                objOutboundInq = ServiceObject.OutboundGETITMNAME(objOutboundInq);
                objOutboundInq.itm_name = objOutboundInq.LstItmxCustdtl[0].itm_name;
                objOutboundInq.lstShipAlocdtl[j].itm_name = objOutboundInq.itm_name;
                objOutboundInq.lstShipAlocdtl[j].aloc_line = Aloc_line;
                ServiceObject.InsertTempAUTOALOC(objOutboundInq);
            }
            objOutboundInq = ServiceObject.OutboundGETALOCSORTSTMT(objOutboundInq);
            if (objOutboundInq.LstItmxCustdtl.Count > 0)
            {
                objOutboundInq.aloc_sort_stmt = objOutboundInq.LstItmxCustdtl[0].aloc_sort_stmt;
                strAlocSortStmt = objOutboundInq.aloc_sort_stmt;
                objOutboundInq.aloc_by = objOutboundInq.LstItmxCustdtl[0].aloc_by;
                strAlocBy = objOutboundInq.aloc_by;
            }
            objOutboundInq.aloc_sort_stmt = strAlocSortStmt;
            objOutboundInq.aloc_by = strAlocBy;
            objOutboundInq = ServiceObject.OutboundGETTEMPLIST(objOutboundInq);
            Aloc_line = 0;
            Allocation_Line_Num = 0;
            grddtl_AlocdQty = 0;
            for (int k = 0; k < objOutboundInq.LstItmxCustdtl.Count(); k++)
            {
                objOutboundInq.cmp_id = cmp_id;
                objOutboundInq.itm_num = objOutboundInq.LstItmxCustdtl[k].itm_num;
                objOutboundInq.itm_color = objOutboundInq.LstItmxCustdtl[k].itm_color;
                objOutboundInq.itm_size = objOutboundInq.LstItmxCustdtl[k].itm_size;
                objOutboundInq.whs_id = objOutboundInq.LstItmxCustdtl[k].whs_id;
                objOutboundInq.itm_code = objOutboundInq.LstItmxCustdtl[k].itm_code;
                objOutboundInq.due_qty = objOutboundInq.LstItmxCustdtl[k].due_qty;
                l_str_dueqty = objOutboundInq.due_qty;
                objOutboundInq = ServiceObject.OutboundGETAVILQTY(objOutboundInq);
                objOutboundInq.avail_qty = objOutboundInq.LstAvailqty[0].pkg_avail_cnt;

                Avail = Convert.ToInt32(objOutboundInq.avail_qty) - grddtl_AlocdQty;
                Aloc_line = Aloc_line + 1;
                ctn_line = 0;
                ctn_line = ctn_line + 1;
                if (Avail < 0)
                {
                    Avail = 0;
                }
                if (objOutboundInq.LstItmxCustdtl[k].reqQty > 0)
                {
                    Order_Qty = objOutboundInq.LstItmxCustdtl[k].reqQty;
                }
                else
                {
                    Order_Qty = objOutboundInq.LstItmxCustdtl[k].due_qty;
                }
                if (Avail >= Order_Qty)
                {
                    objOutboundInq.LstItmxCustdtl[k].aloc_qty = (Order_Qty);
                    objOutboundInq.LstItmxCustdtl[k].reqQty = Convert.ToInt32(objOutboundInq.LstItmxCustdtl[k].aloc_qty);

                    objOutboundInq.LstItmxCustdtl[k].line_num = Aloc_line;
                    AvailQty = (Avail) - (Order_Qty);

                    BackOrder_Qty = (objOutboundInq.LstItmxCustdtl[k].due_qty) - (Order_Qty);
                    if (BackOrder_Qty < 0)
                    {
                        BackOrder_Qty = 0;
                    }
                    objOutboundInq.LstItmxCustdtl[k].back_ordr_qty = BackOrder_Qty;
                }
                else
                {
                    if (Avail > 0)
                    {
                        objOutboundInq.LstItmxCustdtl[k].aloc_qty = Avail;
                        objOutboundInq.LstItmxCustdtl[k].reqQty = Convert.ToInt32(objOutboundInq.LstItmxCustdtl[k].aloc_qty);
                        objOutboundInq.LstItmxCustdtl[k].line_num = Aloc_line;
                        AvailQty = Avail;
                        BackOrder_Qty = (objOutboundInq.LstItmxCustdtl[k].due_qty) - (Avail);
                        if (BackOrder_Qty < 0)
                        {
                            BackOrder_Qty = 0;
                        }
                        objOutboundInq.LstItmxCustdtl[k].back_ordr_qty = BackOrder_Qty;
                    }
                    else
                    {
                        objOutboundInq.LstItmxCustdtl[k].aloc_qty = 0;
                        objOutboundInq.LstItmxCustdtl[k].reqQty = 0;
                        objOutboundInq.LstItmxCustdtl[k].line_num = Aloc_line;
                        AvailQty = 0;
                        BackOrder_Qty = (objOutboundInq.LstItmxCustdtl[k].due_qty);
                        objOutboundInq.LstItmxCustdtl[k].back_ordr_qty = BackOrder_Qty;
                    }
                }
                objOutboundInq.itm_num = objOutboundInq.LstItmxCustdtl[k].itm_num;
                l_str_ITM_NUM = objOutboundInq.itm_num;
                objOutboundInq.itm_color = objOutboundInq.LstItmxCustdtl[k].itm_color;
                l_str_ITM_COLOR = objOutboundInq.itm_color;
                objOutboundInq.itm_size = objOutboundInq.LstItmxCustdtl[k].itm_size;
                l_str_ITM_SIZE = objOutboundInq.itm_size;
                objOutboundInq.due_qty = objOutboundInq.LstItmxCustdtl[k].due_qty;
                objOutboundInq.aloc_qty = objOutboundInq.LstItmxCustdtl[k].aloc_qty;
                l_str_AlocQty = Convert.ToInt32(objOutboundInq.aloc_qty);
                objOutboundInq.line_num = objOutboundInq.LstItmxCustdtl[k].line_num;
                linenum = objOutboundInq.line_num;
                objOutboundInq.due_line = objOutboundInq.LstItmxCustdtl[k].due_line;//20180119
                dueline = objOutboundInq.due_line;
                objOutboundInq.itm_code = objOutboundInq.LstItmxCustdtl[k].itm_code;
                l_str_ITM_CODE = objOutboundInq.itm_code;
                objOutboundInq.so_num = Id;
                objOutboundInq.whs_id = Whsid;
                objOutboundInq.avail_qty = Avail;
                objOutboundInq.aloc_line = Aloc_line;
                objOutboundInq.back_ordr_qty = BackOrder_Qty;
                objOutboundInq.loc_id = "";
                objOutboundInq.ctn_qty = 0;
                ServiceObject.InsertTempAlocSummary(objOutboundInq);
                objOutboundInq = ServiceObject.Update_Tbl_Temp_So_Auto_Aloc_BackOrdervalue(objOutboundInq);
                objOutboundInq.itm_num = objOutboundInq.LstItmxCustdtl[k].itm_num;
                objOutboundInq.itm_color = objOutboundInq.LstItmxCustdtl[k].itm_color;
                objOutboundInq.itm_size = objOutboundInq.LstItmxCustdtl[k].itm_size;
                objOutboundInq.itm_code = objOutboundInq.LstItmxCustdtl[k].itm_code;
                objOutboundInq.whs_id = Whsid;
                objOutboundInq = ServiceObject.OutboundGETALOCDTL(objOutboundInq);
                SelQty = 0;
                BalQty = 0;
                ReqQty = 0;
                ctn_line = 1;
                ToAlocQty = Order_Qty;
                grdAlocQty = 0;
                for (int l = 0; l < objOutboundInq.LstAlocSummary.Count(); l++)
                {
                    grdAlocQty = 0;
                    objOutboundInq.loc_id = objOutboundInq.LstAlocSummary[l].loc_id;
                    l_str_LOCID = objOutboundInq.loc_id;
                    objOutboundInq.pkg_qty = objOutboundInq.LstAlocSummary[l].pkg_qty;
                    l_str_PKGQTY = objOutboundInq.pkg_qty;
                    objOutboundInq.Palet_id = objOutboundInq.LstAlocSummary[l].palet_id;
                    l_str_PALETID = objOutboundInq.Palet_id;
                    objOutboundInq.po_num = objOutboundInq.LstAlocSummary[l].po_num;
                    l_str_PONUM = objOutboundInq.po_num;
                    objOutboundInq.rcvd_dt = objOutboundInq.LstAlocSummary[l].rcvd_dt;
                    l_str_RCVDDT = Convert.ToString(objOutboundInq.rcvd_dt);
                    objOutboundInq.lot_id = objOutboundInq.LstAlocSummary[l].lot_id;
                    l_str_LOTID = objOutboundInq.lot_id;
                    objOutboundInq.avail_qty = objOutboundInq.LstAlocSummary[l].avail_qty;
                    l_str_AVAILQTY = objOutboundInq.avail_qty;
                    SelQty = Convert.ToInt32(objOutboundInq.LstAlocSummary[l].avail_qty) - grdAlocQty;
                    if (SelQty > 0)
                    {
                        if (ToAlocQty > SelQty)
                        {
                            BalQty = 0;
                            objOutboundInq.aloc_line = Aloc_line;
                            objOutboundInq.ctn_line = ctn_line;
                            objOutboundInq.line_num = linenum;
                            objOutboundInq.due_line = dueline;
                            objOutboundInq.itm_num = l_str_ITM_NUM;
                            objOutboundInq.itm_color = l_str_ITM_COLOR;
                            objOutboundInq.itm_size = l_str_ITM_SIZE;
                            objOutboundInq.itm_code = l_str_ITM_CODE;
                            objOutboundInq.whs_id = Whsid;
                            objOutboundInq.loc_id = l_str_LOCID;
                            objOutboundInq.pkg_qty = l_str_PKGQTY;
                            objOutboundInq.Palet_id = l_str_PALETID;
                            objOutboundInq.po_num = l_str_PONUM;
                            objOutboundInq.lot_id = l_str_LOTID;
                            objOutboundInq.rcvd_dt = Convert.ToDateTime(l_str_RCVDDT);
                            objOutboundInq.back_ordr_qty = BalQty;
                            objOutboundInq.aloc_qty = SelQty;
                            objOutboundInq.avail_qty = l_str_AVAILQTY;
                            ServiceObject.InsertTempAlocdtl(objOutboundInq);
                            ctn_line = ctn_line + 1;
                            ToAlocQty = ToAlocQty - SelQty;
                        }
                        else
                        {
                            BalQty = SelQty - ToAlocQty;
                            objOutboundInq.aloc_line = Aloc_line;
                            objOutboundInq.ctn_line = ctn_line;
                            objOutboundInq.line_num = linenum;
                            objOutboundInq.due_line = dueline;
                            objOutboundInq.itm_num = l_str_ITM_NUM;
                            objOutboundInq.itm_color = l_str_ITM_COLOR;
                            objOutboundInq.itm_size = l_str_ITM_SIZE;
                            objOutboundInq.itm_code = l_str_ITM_CODE;
                            objOutboundInq.whs_id = Whsid;
                            objOutboundInq.loc_id = l_str_LOCID;
                            objOutboundInq.pkg_qty = l_str_PKGQTY;
                            objOutboundInq.Palet_id = l_str_PALETID;
                            objOutboundInq.po_num = l_str_PONUM;
                            objOutboundInq.lot_id = l_str_LOTID;
                            objOutboundInq.rcvd_dt = Convert.ToDateTime(l_str_RCVDDT);
                            objOutboundInq.back_ordr_qty = BalQty;
                            objOutboundInq.aloc_qty = ToAlocQty;
                            objOutboundInq.avail_qty = l_str_AVAILQTY;
                            ServiceObject.InsertTempAlocdtl(objOutboundInq);
                            ctn_line = ctn_line + 1;
                            ToAlocQty = 0;
                            break;
                        }
                    }
                }
                objOutboundInq = ServiceObject.OutboundGETTEMPALOCSUMMARY(objOutboundInq);              
                objOutboundInq.l_int_Aloc_Summary_Count = objOutboundInq.LstAlocSummary.Count();
                objOutboundInq = ServiceObject.OutboundGETTEMPALOCDTL(objOutboundInq);
                objOutboundInq = ServiceObject.OutboundGETTEMPLISTvalue(objOutboundInq);                
                objOutboundInq.View_Flag = "A";
                objOutboundInq.quote_num = p_str_batch_id;
                //int tmpTotAlocQty = 0;
                //for (int m = 0; m== objOutboundInq.LstAlocSummary.Count() - 1;)
                //{
                //    if ((objOutboundInq.LstAlocSummary[m].AlcLn) == Aloc_line)
                //    {
                //        tmpTotAlocQty = tmpTotAlocQty + (objOutboundInq.LstAlocSummary[m].Aloc);
                //    }
                //}
                //if (tmpTotAlocQty != l_str_AlocQty)
                //{
                //    l_str_AlocQty = tmpTotAlocQty;
                //    BackOrder_Qty = l_str_dueqty - l_str_AlocQty;
                //    for (int n = 0; n == objOutboundInq.LstAlocDtl.Count() - 1;)
                //    {
                //        objOutboundInq.LstAlocSummary[n].Aloc = tmpTotAlocQty;
                //        objOutboundInq.LstAlocSummary[n].Bal = BackOrder_Qty;

                //    }
                //}
            }
            //if (objOutboundInq.l_int_Aloc_Summary_Count > 0)
            //{
            //    return Json(new { data1 = "N", data2 = objOutboundInq.l_int_Aloc_Summary_Count }, JsonRequestBehavior.AllowGet);
            //}
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_OutboundShipReqAloc", objOutboundInqModel);
        }

        public ActionResult SaveAlocEntry(string p_str_cmp_id, string p_str_Alocdocid, string p_str_Alocdt, string p_str_Alocshiprqfm, string p_str_Alocdeldtfm,
            string p_str_Alocpricetkt, string p_str_Aloccustpo, string p_str_Alocwhsid, string p_str_Aloccustid,
             string p_str_Alocshiptoid, string p_str_Alocshiptoname, string p_str_Alocshipdt, string p_str_AlocCoustordrdt, string p_str_AlocOrdrno, string p_str_canceldt)
        {
            string Itm_Code, Lot_Id, Rcvd_Dt, Loc_Id, Aloc_Qty = string.Empty;
            string Item_Code = string.Empty;
            string Item_num = string.Empty;
            string Item_Color = string.Empty;
            string Item_Size = string.Empty;
            string So_Num = string.Empty;
            int Alloc_Qty = 0;
            int BO_Qty = 0;
            int Dtl_Line = 0;
            int Due_Line = 0;
            int SdtRec_Aloc_qty = 0;
            int Aloc_qty = 0;
            string Bak_OdrQty = string.Empty;

            int PrevLine = 1;
            int NewCtnQty = 0;
            string tmpStep = string.Empty;
            string tmpStatus = string.Empty;
            int Alo, CTN = 0;
            string Whsid, LocId = string.Empty;
            string New_Pkg_Id = string.Empty;
            string palet_id = string.Empty;
            int Ppk = 0;
            int UpdtPkg = 0;
            string SptCtnMsg = string.Empty;
            int NewQty = 0;
            int ctn_line = 0;
            string Line_Num = string.Empty;
            string OldPkgId = string.Empty;
            string WhsID = string.Empty;
            string ItmCode, ItmNum, ItmColor, ItmSize, ItmName, tmp_po, l_str_doc_pkg_id = string.Empty;
            int AlocQty, dueqty = 0;
            int LineNum = 0;
            int N = 0;
            decimal Availqty = 0;
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.cmp_id = p_str_cmp_id;
            objOutboundInq.AlocdocId = p_str_Alocdocid;
            objOutboundInq.aloc_dt = p_str_Alocdt;
            objOutboundInq.price_tkt = p_str_Alocpricetkt;
            objOutboundInq.ShipDt = p_str_Alocshipdt;
            objOutboundInq.CancelDt = p_str_canceldt;
            objOutboundInq.cust_id = p_str_Aloccustid;
            objOutboundInq.CustOrderNo = p_str_Aloccustpo;
            objOutboundInq.CustOrderdt = p_str_AlocCoustordrdt;
            objOutboundInq.orderNo = p_str_AlocOrdrno;
            objOutboundInq.whs_id = p_str_Alocwhsid;
            objOutboundInq.note = "";
            objOutboundInq.ship_to_id = p_str_Alocshiptoid;
            objOutboundInq.ship_to_name = p_str_Alocshiptoname;
            objOutboundInq.process_id = "";
            objOutboundInq = ServiceObject.Move_to_aloc_hdr(objOutboundInq);
            objOutboundInq = ServiceObject.OutboundGETTEMPALOCDTL(objOutboundInq);
            if (objOutboundInq.LstAlocDtl.Count() == 0)
            {
                objOutboundInq.l_str_aloc_aloc_dtls = false;
            }
            else
            {
                for (int l = 0; l < objOutboundInq.LstAlocDtl.Count(); l++)
                {
                    Itm_Code = objOutboundInq.LstAlocDtl[l].itm_code;
                    Loc_Id = objOutboundInq.LstAlocDtl[l].loc_id;
                    Rcvd_Dt = Convert.ToString(objOutboundInq.LstAlocDtl[l].rcvd_dt);
                    Lot_Id = objOutboundInq.LstAlocDtl[l].lot_id;
                    Aloc_Qty = Convert.ToString(objOutboundInq.LstAlocDtl[l].Aloc);
                    objOutboundInq.cmp_id = p_str_cmp_id;
                    objOutboundInq.itm_code = Itm_Code;
                    objOutboundInq.whs_id = p_str_Alocwhsid;
                    objOutboundInq.lot_id = Lot_Id;
                    objOutboundInq.rcvd_dt = Convert.ToDateTime(Rcvd_Dt);
                    objOutboundInq.loc_id = Loc_Id;
                    objOutboundInq.avail_qty = Convert.ToDecimal(Aloc_Qty);
                    objOutboundInq.process_id = "";
                    ServiceObject.Moveto_TrnHdr(objOutboundInq);
                }
                objOutboundInq = ServiceObject.OutboundGETTEMPALOCSUMMARY(objOutboundInq);
                objOutboundInq = ServiceObject.OutboundGETTEMPLIST(objOutboundInq);
                for (int m = 0; m < objOutboundInq.LstAlocSummary.Count(); m++)
                {
                    updateSodtl(m, p_str_cmp_id);
                    if (objOutboundInq.ReturnValue == 1)
                    {
                        objOutboundInq.aloc_line = objOutboundInq.LstAlocSummary[m].Line;
                        objOutboundInq.whs_id = objOutboundInq.LstAlocSummary[m].whs_id;
                        objOutboundInq.itm_code = objOutboundInq.LstAlocSummary[m].so_num;
                        objOutboundInq.so_num = objOutboundInq.LstAlocSummary[m].itm_code;
                        objOutboundInq.itm_num = objOutboundInq.LstAlocSummary[m].itm_num;
                        objOutboundInq.itm_color = objOutboundInq.LstAlocSummary[m].itm_color;
                        objOutboundInq.itm_size = objOutboundInq.LstAlocSummary[m].itm_size;
                        objOutboundInq.due_qty = objOutboundInq.LstAlocSummary[m].due_qty;
                        objOutboundInq.aloc_qty = objOutboundInq.LstAlocSummary[m].aloc_qty;
                        objOutboundInq.avail_qty = objOutboundInq.LstAlocSummary[m].avail_qty;
                        objOutboundInq.back_ordr_qty = objOutboundInq.LstAlocSummary[m].back_ordr_qty;
                        objOutboundInq.line_num = objOutboundInq.LstAlocSummary[m].Soline;
                        objOutboundInq.due_line = objOutboundInq.LstAlocSummary[m].DueLine;
                        ServiceObject.Move_To_Grd_Bad_Itm(objOutboundInq);
                        objOutboundInq.ReturnValue = 0;
                        continue;
                    }
                    Alloc_Qty = Convert.ToInt32(objOutboundInq.LstAlocSummary[m].aloc_qty);
                    if (Alloc_Qty == 0)
                    {
                        continue;
                    }
                    Move_to_aloc_dtl(m, p_str_cmp_id, p_str_Alocdocid);
                    if (objOutboundInq.ReturnValue == 1)
                    {
                        objOutboundInq.aloc_line = LineNum;
                        objOutboundInq.whs_id = WhsID;
                        objOutboundInq.itm_code = So_Num;
                        objOutboundInq.so_num = Item_Code;
                        objOutboundInq.itm_num = Item_num;
                        objOutboundInq.itm_color = Item_Color;
                        objOutboundInq.itm_size = Item_Size;
                        objOutboundInq.due_qty = dueqty;
                        objOutboundInq.aloc_qty = Alloc_Qty;
                        objOutboundInq.avail_qty = Availqty;
                        objOutboundInq.back_ordr_qty = BO_Qty;
                        objOutboundInq.line_num = Dtl_Line;
                        objOutboundInq.due_line = Due_Line;
                        ServiceObject.Move_To_Grd_Bad_Itm(objOutboundInq);
                        objOutboundInq.ReturnValue = 0;
                        continue;
                    }
                    Move_to_aloc_ctn(m, p_str_cmp_id, p_str_Alocdocid);
                    Update_Tbl_iv_itm_trn_in(m, p_str_cmp_id, p_str_Alocdocid, p_str_AlocOrdrno, p_str_Alocshiptoname);
                }
                objOutboundInq = ServiceObject.OutboundGETTEMPALOCSUMMARY(objOutboundInq);
                for (int p = 0; p < objOutboundInq.LstAlocSummary.Count(); p++)
                {
                    tmpStep = "ALOC";
                    tmpStatus = "A";
                    if (objOutboundInq.LstAlocSummary[p].back_ordr_qty > 0)
                    {
                        tmpStep = "B/O";
                        tmpStatus = "B";
                        break;
                    }
                }
                objOutboundInq.cmp_id = p_str_cmp_id;
                objOutboundInq.so_num = p_str_Alocshiprqfm;
                objOutboundInq.step = tmpStep;
                objOutboundInq.status = tmpStatus;
                ServiceObject.Change_SOHdr_Status_atAdd(objOutboundInq);
                objOutboundInq.l_str_aloc_aloc_dtls = true;
            }
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return Json(objOutboundInq.l_str_aloc_aloc_dtls, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShowVasReport(string p_str_cmp_id, string p_str_vasid)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            VasInquiry objVasInquiry = new VasInquiry();
            IVasInquiryService ServiceObject = new VasInquiryService();
            try
            {
                if (isValid)
                {
                    strReportName = "rpt_iv_vas.rpt";

                    ReportDocument rd = new ReportDocument();
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//VasInquiry//" + strReportName;
                    objVasInquiry.cmp_id = p_str_cmp_id;
                    objVasInquiry.ship_doc_id = p_str_vasid;
                    objVasInquiry = ServiceObject.GetVasPostDetails(objVasInquiry);
                    var rptSource = objVasInquiry.ListVasInquiry.ToList();
                    rd.Load(strRptPath);
                    int AlocCount = 0;
                    AlocCount = objVasInquiry.ListVasInquiry.Count();
                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                        rd.SetDataSource(rptSource);
                    objVasInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                    rd.SetParameterValue("fml_image_path", objVasInquiry.Image_Path);

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
        public ActionResult ShowAlocSaveReport(string p_str_cmpid, string p_str_Alocdocid, string p_str_Sonum)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            OutboundInq objInbound = new OutboundInq();
            OutboundInqService objService = new OutboundInqService();
            try
            {
                if (isValid)
                {
                    OutboundInq objOutboundInq = new OutboundInq();
                    OutboundInqService ServiceObject = new OutboundInqService();
                    //objOutboundInq.cmp_id = p_str_cmpid;
                    //objOutboundInq.so_num = p_str_Sonum;
                    //objOutboundInq = ServiceObject.BackOrderRpt(objOutboundInq);
                    //if (objOutboundInq.LstAlocDtl.Count > 0)
                    //{
                        //strReportName = "rpt_mvc_so_bo.rpt";
                        //ReportDocument rd = new ReportDocument();
                        //string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        //objOutboundInq.cmp_id = p_str_cmpid;
                        //objOutboundInq.so_num = p_str_Sonum;
                        //objOutboundInq = ServiceObject.Get_AlocBackOrderRptList(objOutboundInq);
                        //var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                        //rd.Load(strRptPath);
                        //int AlocCount = 0;
                        //AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                        //if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                        //    rd.SetDataSource(rptSource);
                        //objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        //rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);

                    //    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                    //}
                    //else
                    //{
                        strReportName = "rpt_iv_pickslip_direct.rpt";
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmpid;
                        objOutboundInq.AlocdocId = p_str_Alocdocid;
                        objOutboundInq = ServiceObject.Get_AlocSaveRpt(objOutboundInq);
                        var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                        rd.SetDataSource(rptSource);
                        objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);

                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Out Ticket Report");
                    //}
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
        public ActionResult ShowAlocPostReport(string p_str_cmpid)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            try
            {
                if (isValid)
                {
                    strReportName = "rpt_iv_packslip_tpw.rpt";
                    OutboundShipInq objOutboundShipInq = new OutboundShipInq();
                    OutboundShipInqService ServiceObject = new OutboundShipInqService();
                    ReportDocument rd = new ReportDocument();
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                    objOutboundShipInq.cmp_id = p_str_cmpid;
                    objOutboundShipInq.ship_doc_id = Session["l_str_bol_num"].ToString();
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
                    rd.Load(strRptPath);
                    int AlocCount = 0;
                    AlocCount = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt.Count();
                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                        rd.SetDataSource(rptSource);
                    objOutboundShipInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                    rd.SetParameterValue("fml_image_path", objOutboundShipInq.Image_Path);
                    rd.SetParameterValue("SumOfCubes", objOutboundShipInq.TotCube);
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
        public ActionResult AlocViews(string cmpid, string aloc_num, string so_num, string batch_num)
        {

            int l_int_linenum = 0;
            string l_str_itmnum = string.Empty;
            string l_str_itmcolor = string.Empty;
            string l_str_itmsize = string.Empty;
            string l_str_itmcode = string.Empty;
            string l_str_wshid = string.Empty;
            int dueline = 0;
            int dtlline = 0;
            int ordrqty = 0;
            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objOutboundInq.cmp_id = cmpid;
            objOutboundInq.CompID = cmpid;
            objOutboundInq.aloc_doc_id = aloc_num;
            objOutboundInq.so_num = so_num;
            objOutboundInq = ServiceObject.OutboundGETALOCSORTSTMT(objOutboundInq);
            ServiceObject.DeleteAUTOALOC(objOutboundInq);
            if (objOutboundInq.LstItmxCustdtl.Count > 0)
            {
                objOutboundInq.aloc_sort_stmt = objOutboundInq.LstItmxCustdtl[0].aloc_sort_stmt;
                strAlocSortStmt = objOutboundInq.aloc_sort_stmt;
                objOutboundInq.aloc_by = objOutboundInq.LstItmxCustdtl[0].aloc_by;
                strAlocBy = objOutboundInq.aloc_by;
            }
            objOutboundInq.aloc_sort_stmt = strAlocSortStmt;
            objOutboundInq.aloc_by = strAlocBy;
            objOutboundInq = ServiceObject.GetalochdrList(objOutboundInq);
            objOutboundInq.whsid = objOutboundInq.LstAlocSummary[0].whs_id;
            l_str_wshid = objOutboundInq.whsid;
            objOutboundInq.ShipReqIDFm = so_num;
            objOutboundInq.ShipReqIDTo = so_num;
            objOutboundInq.AlocdocId = aloc_num;
            objOutboundInq.pricetkt = objOutboundInq.LstAlocSummary[0].price_tkt;
            objOutboundInq.CustPO = objOutboundInq.LstAlocSummary[0].cust_ordr_num;
            objCompany.cmp_id = cmpid;
            objCompany.whs_id = objOutboundInq.whsid;
            objCompany = ServiceObjectCompany.Validate_Cmp_whs(objCompany);
            objOutboundInq.Lstwhsdtl = objCompany.Lstwhsdtl;
            if (objOutboundInq.Lstwhsdtl.Count > 0)
            {
                objOutboundInq.whsname = objOutboundInq.Lstwhsdtl[0].whs_name;
            }
            else
            {
                objOutboundInq.whsname = "";
            }
            objCompany.cust_cmp_id = cmpid;
            objCompany.cmp_id = cmpid;
            objCompany = ServiceObjectCompany.Validate_Customer(objCompany);
            objOutboundInq.Lstcustdtl = objCompany.Lstcustdtl;
            if (objOutboundInq.Lstcustdtl.Count > 0)
            {
                objOutboundInq.CustName = objOutboundInq.Lstcustdtl[0].cust_name;
            }
            else
            {
                objOutboundInq.CustName = "";
            }
            objOutboundInq.shipto_id = objOutboundInq.LstAlocSummary[0].ship_to_id;
            objOutboundInq.lblshiptoid = objOutboundInq.LstAlocSummary[0].ship_to_name;
            objOutboundInq.orderNo = objOutboundInq.LstAlocSummary[0].cntr_num;
            objOutboundInq.aloc_dt = objOutboundInq.LstAlocSummary[0].aloc_dt;
            DateTime Alocdt = Convert.ToDateTime(objOutboundInq.aloc_dt);
            objOutboundInq.aloc_dt = Convert.ToString(Alocdt.ToString("MM/dd/yyyy"));
            objOutboundInq = ServiceObject.GetalocdtlList(objOutboundInq);
            for (int i = 0; i < objOutboundInq.lstShipAlocdtl.Count(); i++)
            {

                objOutboundInq.aloc_line = objOutboundInq.lstShipAlocdtl[i].line_num;
                l_int_linenum = objOutboundInq.aloc_line;
                objOutboundInq.whs_id = l_str_wshid;
                l_str_wshid = objOutboundInq.whs_id;
                objOutboundInq.itm_code = objOutboundInq.lstShipAlocdtl[i].itm_code;
                l_str_itmcode = objOutboundInq.itm_code;
                objOutboundInq.so_num = objOutboundInq.lstShipAlocdtl[i].so_num;
                objOutboundInq.itm_num = objOutboundInq.lstShipAlocdtl[i].itm_num;
                l_str_itmnum = objOutboundInq.itm_num;
                objOutboundInq.itm_color = objOutboundInq.lstShipAlocdtl[i].itm_color;
                l_str_itmcolor = objOutboundInq.itm_color;
                objOutboundInq.itm_size = objOutboundInq.lstShipAlocdtl[i].itm_size;
                l_str_itmsize = objOutboundInq.itm_size;
                objOutboundInq.aloc_qty = objOutboundInq.lstShipAlocdtl[i].aloc_qty;
                objOutboundInq.avail_qty = 0;
                objOutboundInq.back_ordr_qty = objOutboundInq.lstShipAlocdtl[i].back_ordr_qty;
                objOutboundInq.line_num = objOutboundInq.lstShipAlocdtl[i].dtl_line;
                objOutboundInq.dtl_line = objOutboundInq.lstShipAlocdtl[i].dtl_line;
                dtlline = objOutboundInq.line_num;
                objOutboundInq.due_line = objOutboundInq.lstShipAlocdtl[i].due_line;
                dueline = objOutboundInq.due_line;
                objOutboundInq = ServiceObject.GetalocdueList(objOutboundInq);
                objOutboundInq.due_qty = objOutboundInq.LstAvailqty[0].due_qty;
                objOutboundInq.aloc_qty = objOutboundInq.LstAvailqty[0].aloc_qty;//CR_MVC_3PL_20180411-001 Added By NIthya
                objOutboundInq.back_ordr_qty = objOutboundInq.LstAvailqty[0].back_ordr_qty;
                ordrqty = objOutboundInq.due_qty;
                objOutboundInq.lstShipAlocdtl[i].due_qty = ordrqty;
                objOutboundInq = ServiceObject.OutboundGETITMNAME(objOutboundInq);
                objOutboundInq.itm_name = objOutboundInq.LstItmxCustdtl[0].itm_name;
                ServiceObject.InsertTempAlocSummary(objOutboundInq);
                objOutboundInq = ServiceObject.GetalocctnList(objOutboundInq);
                for (int j = 0; j < objOutboundInq.LstAlocDtl.Count(); j++)
                {
                    objOutboundInq.aloc_line = l_int_linenum;
                    objOutboundInq.ctn_line = objOutboundInq.LstAlocDtl[j].ctn_line;
                    objOutboundInq.so_num = so_num;
                    objOutboundInq.itm_code = l_str_itmcode;
                    objOutboundInq.itm_num = l_str_itmnum;
                    objOutboundInq.itm_color = l_str_itmcolor;
                    objOutboundInq.itm_size = l_str_itmsize;
                    objOutboundInq.whs_id = objOutboundInq.LstAlocDtl[j].whs_id;
                    objOutboundInq.loc_id = objOutboundInq.LstAlocDtl[j].loc_id;
                    objOutboundInq.rcvd_dt = objOutboundInq.LstAlocDtl[j].rcvd_dt;
                    objOutboundInq.due_qty = ordrqty;
                    objOutboundInq.pkg_qty = objOutboundInq.LstAlocDtl[j].ctn_qty;
                    objOutboundInq.avail_qty = 0;
                    objOutboundInq.aloc_qty = objOutboundInq.LstAlocDtl[j].line_qty;
                    objOutboundInq.back_ordr_qty = 0;
                    objOutboundInq.lot_id = objOutboundInq.LstAlocDtl[j].lot_id;
                    objOutboundInq.Palet_id = objOutboundInq.LstAlocDtl[j].palet_id;
                    objOutboundInq.line_num = dtlline;
                    objOutboundInq.due_line = dueline;
                    objOutboundInq.po_num = objOutboundInq.LstAlocDtl[j].po_num;
                    ServiceObject.InsertTempAlocdtl(objOutboundInq);
                }
            }
            objOutboundInq = ServiceObject.OutboundGETTEMPALOCSUMMARY(objOutboundInq);
            objOutboundInq = ServiceObject.OutboundGETTEMPALOCDTL(objOutboundInq);
            objOutboundInq.View_Flag = "V";
            objOutboundInq.quote_num = batch_num;
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundShipModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_OutboundShipReqAloc", objOutboundShipModel);
        }
        public ActionResult AlocDelete(string cmpid, string aloc_num, string so_num, string batch_num)
        {
            int l_int_linenum = 0;
            string l_str_itmnum = string.Empty;
            string l_str_itmcolor = string.Empty;
            string l_str_itmsize = string.Empty;
            string l_str_itmcode = string.Empty;
            string l_str_wshid = string.Empty;
            int dueline = 0;
            int dtlline = 0;
            int ordrqty = 0;
            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objOutboundInq.cmp_id = cmpid;
            objOutboundInq.CompID = cmpid;
            objOutboundInq.aloc_doc_id = aloc_num;
            objOutboundInq.so_num = so_num;
            objOutboundInq = ServiceObject.OutboundGETALOCSORTSTMT(objOutboundInq);
            ServiceObject.DeleteAUTOALOC(objOutboundInq);
            if (objOutboundInq.LstItmxCustdtl.Count > 0)
            {
                objOutboundInq.aloc_sort_stmt = objOutboundInq.LstItmxCustdtl[0].aloc_sort_stmt;
                strAlocSortStmt = objOutboundInq.aloc_sort_stmt;
                objOutboundInq.aloc_by = objOutboundInq.LstItmxCustdtl[0].aloc_by;
                strAlocBy = objOutboundInq.aloc_by;
            }
            objOutboundInq.aloc_sort_stmt = strAlocSortStmt;
            objOutboundInq.aloc_by = strAlocBy;
            objOutboundInq = ServiceObject.GetalochdrList(objOutboundInq);
            objOutboundInq.whsid = objOutboundInq.LstAlocSummary[0].whs_id;
            l_str_wshid = objOutboundInq.whsid;
            objOutboundInq.ShipReqIDFm = so_num;
            objOutboundInq.ShipReqIDTo = so_num;
            objOutboundInq.AlocdocId = aloc_num;
            objOutboundInq.pricetkt = objOutboundInq.LstAlocSummary[0].price_tkt;
            objOutboundInq.CustPO = objOutboundInq.LstAlocSummary[0].cust_ordr_num;
            objCompany.cmp_id = cmpid;
            objCompany.whs_id = objOutboundInq.whsid;
            objCompany = ServiceObjectCompany.Validate_Cmp_whs(objCompany);
            objOutboundInq.Lstwhsdtl = objCompany.Lstwhsdtl;
            if (objOutboundInq.Lstwhsdtl.Count > 0)
            {
                objOutboundInq.whsname = objOutboundInq.Lstwhsdtl[0].whs_name;
            }
            else
            {
                objOutboundInq.whsname = "";
            }
            objCompany.cust_cmp_id = cmpid;
            objCompany.cmp_id = cmpid;
            objCompany = ServiceObjectCompany.Validate_Customer(objCompany);
            objOutboundInq.Lstcustdtl = objCompany.Lstcustdtl;
            if (objOutboundInq.Lstcustdtl.Count > 0)
            {
                objOutboundInq.CustName = objOutboundInq.Lstcustdtl[0].cust_name;
            }
            else
            {
                objOutboundInq.CustName = "";
            }
            objOutboundInq.shipto_id = objOutboundInq.LstAlocSummary[0].ship_to_id;
            objOutboundInq.lblshiptoid = objOutboundInq.LstAlocSummary[0].ship_to_name;
            objOutboundInq.orderNo = objOutboundInq.LstAlocSummary[0].cntr_num;
            objOutboundInq.aloc_dt = objOutboundInq.LstAlocSummary[0].aloc_dt;
            DateTime Alocdt = Convert.ToDateTime(objOutboundInq.aloc_dt);
            objOutboundInq.aloc_dt = Convert.ToString(Alocdt.ToString("MM/dd/yyyy"));
            objOutboundInq = ServiceObject.GetalocdtlList(objOutboundInq);
            for (int i = 0; i < objOutboundInq.lstShipAlocdtl.Count(); i++)
            {

                objOutboundInq.aloc_line = objOutboundInq.lstShipAlocdtl[i].line_num;
                l_int_linenum = objOutboundInq.aloc_line;
                objOutboundInq.whs_id = l_str_wshid;
                //l_str_wshid = objOutboundInq.whs_id;
                objOutboundInq.itm_code = objOutboundInq.lstShipAlocdtl[i].itm_code;
                l_str_itmcode = objOutboundInq.itm_code;
                objOutboundInq.so_num = objOutboundInq.lstShipAlocdtl[i].so_num;
                objOutboundInq.itm_num = objOutboundInq.lstShipAlocdtl[i].itm_num;
                l_str_itmnum = objOutboundInq.itm_num;
                objOutboundInq.itm_color = objOutboundInq.lstShipAlocdtl[i].itm_color;
                l_str_itmcolor = objOutboundInq.itm_color;
                objOutboundInq.itm_size = objOutboundInq.lstShipAlocdtl[i].itm_size;
                l_str_itmsize = objOutboundInq.itm_size;
                objOutboundInq.aloc_qty = objOutboundInq.lstShipAlocdtl[i].aloc_qty;
                objOutboundInq.avail_qty = 0;
                objOutboundInq.back_ordr_qty = objOutboundInq.lstShipAlocdtl[i].back_ordr_qty;
                objOutboundInq.line_num = objOutboundInq.lstShipAlocdtl[i].dtl_line;
                objOutboundInq.dtl_line = objOutboundInq.lstShipAlocdtl[i].dtl_line;
                dtlline = objOutboundInq.line_num;
                objOutboundInq.due_line = objOutboundInq.lstShipAlocdtl[i].due_line;
                dueline = objOutboundInq.due_line;
                objOutboundInq = ServiceObject.GetalocdueList(objOutboundInq);
                objOutboundInq.due_qty = objOutboundInq.LstAvailqty[0].due_qty;
                objOutboundInq.aloc_qty = objOutboundInq.LstAvailqty[0].aloc_qty;//CR_MVC_3PL_20180411-001 Added By NIthya
                objOutboundInq.back_ordr_qty = objOutboundInq.LstAvailqty[0].back_ordr_qty;
                ordrqty = objOutboundInq.due_qty;
                objOutboundInq.lstShipAlocdtl[i].due_qty = ordrqty;
                objOutboundInq = ServiceObject.OutboundGETITMNAME(objOutboundInq);
                objOutboundInq.itm_name = objOutboundInq.LstItmxCustdtl[0].itm_name;
                ServiceObject.InsertTempAlocSummary(objOutboundInq);
                objOutboundInq = ServiceObject.GetalocctnList(objOutboundInq);
                for (int j = 0; j < objOutboundInq.LstAlocDtl.Count(); j++)
                {
                    objOutboundInq.aloc_line = l_int_linenum;
                    objOutboundInq.ctn_line = objOutboundInq.LstAlocDtl[j].ctn_line;
                    objOutboundInq.so_num = so_num;
                    objOutboundInq.itm_code = l_str_itmcode;
                    objOutboundInq.itm_num = l_str_itmnum;
                    objOutboundInq.itm_color = l_str_itmcolor;
                    objOutboundInq.itm_size = l_str_itmsize;
                    objOutboundInq.whs_id = objOutboundInq.LstAlocDtl[j].whs_id;
                    objOutboundInq.loc_id = objOutboundInq.LstAlocDtl[j].loc_id;
                    objOutboundInq.rcvd_dt = objOutboundInq.LstAlocDtl[j].rcvd_dt;
                    objOutboundInq.due_qty = ordrqty;
                    objOutboundInq.pkg_qty = objOutboundInq.LstAlocDtl[j].ctn_qty;
                    objOutboundInq.avail_qty = 0;
                    objOutboundInq.aloc_qty = objOutboundInq.LstAlocDtl[j].line_qty;
                    objOutboundInq.back_ordr_qty = 0;
                    objOutboundInq.lot_id = objOutboundInq.LstAlocDtl[j].lot_id;
                    objOutboundInq.Palet_id = objOutboundInq.LstAlocDtl[j].palet_id;
                    objOutboundInq.line_num = dtlline;
                    objOutboundInq.due_line = dueline;
                    objOutboundInq.po_num = objOutboundInq.LstAlocDtl[j].po_num;
                    ServiceObject.InsertTempAlocdtl(objOutboundInq);
                }
            }
            objOutboundInq = ServiceObject.OutboundGETTEMPALOCSUMMARY(objOutboundInq);
            objOutboundInq = ServiceObject.OutboundGETTEMPALOCDTL(objOutboundInq);
            objOutboundInq.View_Flag = "D";
            objOutboundInq.quote_num = batch_num;
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundShipModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_OutboundShipReqAloc", objOutboundShipModel);
        }
        public ActionResult AlocSaveDelete(string p_str_cmp_id, string p_str_aloc_id, string p_str_so_num)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.cmp_id = p_str_cmp_id;
            objOutboundInq.AlocdocId = p_str_aloc_id;
            objOutboundInq.so_num = p_str_so_num;
            objOutboundInq = ServiceObject.Del_Alloc_Upd_SO(objOutboundInq);
            objOutboundInq = ServiceObject.Del_data_to_itm_trn_in(objOutboundInq);
            objOutboundInq = ServiceObject.OutboundGETTEMPALOCDTL(objOutboundInq);
            for (int i = 0; i < objOutboundInq.LstAlocDtl.Count(); i++)
            {
                int UpDtQty = 0;
                UpDtQty = objOutboundInq.LstAlocDtl[i].Aloc;
                if (UpDtQty > 0)
                {
                    objOutboundInq.cmp_id = p_str_cmp_id;
                    objOutboundInq.itm_code = objOutboundInq.LstAlocDtl[i].itm_code;
                    objOutboundInq.whs_id = objOutboundInq.LstAlocDtl[i].whs_id;
                    objOutboundInq.lot_id = objOutboundInq.LstAlocDtl[i].lot_id;
                    objOutboundInq.rcvd_dt = objOutboundInq.LstAlocDtl[i].rcvd_dt;
                    objOutboundInq.loc_id = objOutboundInq.LstAlocDtl[i].loc_id;
                    objOutboundInq.aloc_qty = UpDtQty;
                    objOutboundInq.process_id = "";
                    objOutboundInq = ServiceObject.Add_To_Trn_Hdr(objOutboundInq);
                }
            }
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundShipModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            int resultcount;
            resultcount = 1;
            return Json(resultcount, JsonRequestBehavior.AllowGet);
        }
        //CR_3PL_MVC_OB_2018_0220_001
        public ActionResult AlocEdit(string cmpid, string aloc_num, string so_num, string batch_num)
        {
            int l_int_linenum = 0;
            string l_str_itmnum = string.Empty;
            string l_str_itmcolor = string.Empty;
            string l_str_itmsize = string.Empty;
            string l_str_itmcode = string.Empty;
            string l_str_wshid = string.Empty;
            int dueline = 0;
            int dtlline = 0;
            int ordrqty = 0;
            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objOutboundInq.cmp_id = cmpid;
            objOutboundInq.CompID = cmpid;
            objOutboundInq.aloc_doc_id = aloc_num;
            objOutboundInq.so_num = so_num;
            objOutboundInq = ServiceObject.OutboundGETALOCSORTSTMT(objOutboundInq);
            ServiceObject.DeleteAUTOALOC(objOutboundInq);
            if (objOutboundInq.LstItmxCustdtl.Count > 0)
            {
                objOutboundInq.aloc_sort_stmt = objOutboundInq.LstItmxCustdtl[0].aloc_sort_stmt;
                strAlocSortStmt = objOutboundInq.aloc_sort_stmt;
                objOutboundInq.aloc_by = objOutboundInq.LstItmxCustdtl[0].aloc_by;
                strAlocBy = objOutboundInq.aloc_by;
            }
            objOutboundInq.aloc_sort_stmt = strAlocSortStmt;
            objOutboundInq.aloc_by = strAlocBy;
            objOutboundInq = ServiceObject.GetalochdrList(objOutboundInq);
            objOutboundInq.whsid = objOutboundInq.LstAlocSummary[0].whs_id;
            l_str_wshid = objOutboundInq.whsid;
            objOutboundInq.ShipReqIDFm = so_num;
            objOutboundInq.ShipReqIDTo = so_num;
            objOutboundInq.AlocdocId = aloc_num;
            objOutboundInq.pricetkt = objOutboundInq.LstAlocSummary[0].price_tkt;
            objOutboundInq.CustPO = objOutboundInq.LstAlocSummary[0].cust_ordr_num;
            objCompany.cmp_id = cmpid;
            objCompany.whs_id = objOutboundInq.whsid;
            objCompany = ServiceObjectCompany.Validate_Cmp_whs(objCompany);
            objOutboundInq.Lstwhsdtl = objCompany.Lstwhsdtl;
            if (objOutboundInq.Lstwhsdtl.Count > 0)
            {
                objOutboundInq.whsname = objOutboundInq.Lstwhsdtl[0].whs_name;
            }
            else
            {
                objOutboundInq.whsname = "";
            }
            objCompany.cust_cmp_id = cmpid;
            objCompany.cmp_id = cmpid;
            objCompany = ServiceObjectCompany.Validate_Customer(objCompany);
            objOutboundInq.Lstcustdtl = objCompany.Lstcustdtl;
            if (objOutboundInq.Lstcustdtl.Count > 0)
            {
                objOutboundInq.CustName = objOutboundInq.Lstcustdtl[0].cust_name;
            }
            else
            {
                objOutboundInq.CustName = "";
            }
            objOutboundInq.shipto_id = objOutboundInq.LstAlocSummary[0].ship_to_id;
            objOutboundInq.lblshiptoid = objOutboundInq.LstAlocSummary[0].ship_to_name;
            objOutboundInq.orderNo = objOutboundInq.LstAlocSummary[0].cntr_num;
            objOutboundInq.aloc_dt = objOutboundInq.LstAlocSummary[0].aloc_dt;
            DateTime Alocdt = Convert.ToDateTime(objOutboundInq.aloc_dt);
            objOutboundInq.aloc_dt = Convert.ToString(Alocdt.ToString("MM/dd/yyyy"));
            objOutboundInq = ServiceObject.GetalocdtlList(objOutboundInq);
            for (int i = 0; i < objOutboundInq.lstShipAlocdtl.Count(); i++)
            {
                objOutboundInq.aloc_line = objOutboundInq.lstShipAlocdtl[i].line_num;
                l_int_linenum = objOutboundInq.aloc_line;
                objOutboundInq.whs_id = l_str_wshid;
                l_str_wshid = objOutboundInq.whs_id;
                objOutboundInq.itm_code = objOutboundInq.lstShipAlocdtl[i].itm_code;
                l_str_itmcode = objOutboundInq.itm_code;
                objOutboundInq.so_num = objOutboundInq.lstShipAlocdtl[i].so_num;
                objOutboundInq.itm_num = objOutboundInq.lstShipAlocdtl[i].itm_num;
                l_str_itmnum = objOutboundInq.itm_num;
                objOutboundInq.itm_color = objOutboundInq.lstShipAlocdtl[i].itm_color;
                l_str_itmcolor = objOutboundInq.itm_color;
                objOutboundInq.itm_size = objOutboundInq.lstShipAlocdtl[i].itm_size;
                l_str_itmsize = objOutboundInq.itm_size;
                objOutboundInq.aloc_qty = objOutboundInq.lstShipAlocdtl[i].aloc_qty;
                objOutboundInq.avail_qty = 0;
                objOutboundInq.back_ordr_qty = objOutboundInq.lstShipAlocdtl[i].back_ordr_qty;
                objOutboundInq.line_num = objOutboundInq.lstShipAlocdtl[i].dtl_line;
                objOutboundInq.dtl_line = objOutboundInq.lstShipAlocdtl[i].dtl_line;
                dtlline = objOutboundInq.line_num;
                objOutboundInq.due_line = objOutboundInq.lstShipAlocdtl[i].due_line;
                dueline = objOutboundInq.due_line;
                objOutboundInq = ServiceObject.GetalocdueList(objOutboundInq);
                objOutboundInq.due_qty = objOutboundInq.LstAvailqty[0].due_qty;
                objOutboundInq.aloc_qty = objOutboundInq.LstAvailqty[0].aloc_qty;//CR_MVC_3PL_20180411-001 Added By NIthya
                objOutboundInq.back_ordr_qty = objOutboundInq.LstAvailqty[0].back_ordr_qty;
                ordrqty = objOutboundInq.due_qty;
                objOutboundInq.lstShipAlocdtl[i].due_qty = ordrqty;
                objOutboundInq = ServiceObject.OutboundGETITMNAME(objOutboundInq);
                objOutboundInq.itm_name = objOutboundInq.LstItmxCustdtl[0].itm_name;
                ServiceObject.InsertTempAlocSummary(objOutboundInq);
                objOutboundInq = ServiceObject.GetalocctnList(objOutboundInq);
                for (int j = 0; j < objOutboundInq.LstAlocDtl.Count(); j++)
                {
                    objOutboundInq.aloc_line = l_int_linenum;
                    objOutboundInq.ctn_line = objOutboundInq.LstAlocDtl[j].ctn_line;
                    objOutboundInq.so_num = so_num;
                    objOutboundInq.itm_code = l_str_itmcode;
                    objOutboundInq.itm_num = l_str_itmnum;
                    objOutboundInq.itm_color = l_str_itmcolor;
                    objOutboundInq.itm_size = l_str_itmsize;
                    objOutboundInq.whs_id = objOutboundInq.LstAlocDtl[j].whs_id;
                    objOutboundInq.loc_id = objOutboundInq.LstAlocDtl[j].loc_id;
                    objOutboundInq.rcvd_dt = objOutboundInq.LstAlocDtl[j].rcvd_dt;
                    objOutboundInq.due_qty = ordrqty;
                    objOutboundInq.pkg_qty = objOutboundInq.LstAlocDtl[j].ctn_qty;
                    objOutboundInq.avail_qty = 0;
                    objOutboundInq.aloc_qty = objOutboundInq.LstAlocDtl[j].line_qty;
                    objOutboundInq.back_ordr_qty = 0;
                    objOutboundInq.lot_id = objOutboundInq.LstAlocDtl[j].lot_id;
                    objOutboundInq.Palet_id = objOutboundInq.LstAlocDtl[j].palet_id;
                    objOutboundInq.line_num = dtlline;
                    objOutboundInq.due_line = dueline;
                    objOutboundInq.po_num = objOutboundInq.LstAlocDtl[j].po_num;
                    ServiceObject.InsertTempAlocdtl(objOutboundInq);
                }
            }
            objOutboundInq = ServiceObject.OutboundGETTEMPALOCSUMMARY(objOutboundInq);
            objOutboundInq = ServiceObject.OutboundGETTEMPALOCDTL(objOutboundInq);
            objOutboundInq.View_Flag = "M";
            objOutboundInq.quote_num = batch_num;
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundShipModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_OutboundShipReqAloc", objOutboundShipModel);
        }
        public ActionResult EditAutoAloc(string p_str_cmp_id, string p_str_Alocdocid, string SelectdID, string p_str_Sonum)
        {
            int l_int_linenum = 0;
            string ItmCode, ItmNum, ItmColor, ItmSize, WhsId = string.Empty;
            decimal TotAvailQty = 0;
            decimal TotAlocQty = 0;
            int DtlLine = 0;
            int DueLine = 0;
            int OdrQty = 0;
            int LineNum = 0;
            string l_str_wshid = string.Empty;
            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objOutboundInq.cmp_id = p_str_cmp_id;
            objOutboundInq.CompID = p_str_cmp_id;
            objOutboundInq.aloc_doc_id = p_str_Alocdocid;
            objOutboundInq.itm_code = SelectdID;
            objOutboundInq.so_num = p_str_Sonum;
            objOutboundInq = ServiceObject.OutboundGETALOCSORTSTMT(objOutboundInq);
            if (objOutboundInq.LstItmxCustdtl.Count > 0)
            {
                objOutboundInq.aloc_sort_stmt = objOutboundInq.LstItmxCustdtl[0].aloc_sort_stmt;
                strAlocSortStmt = objOutboundInq.aloc_sort_stmt;
                objOutboundInq.aloc_by = objOutboundInq.LstItmxCustdtl[0].aloc_by;
                strAlocBy = objOutboundInq.aloc_by;
            }
            objOutboundInq.aloc_sort_stmt = strAlocSortStmt;
            objOutboundInq.aloc_by = strAlocBy;
            objOutboundInq = ServiceObject.GetSelectedgridValueList(objOutboundInq);
            DtlLine = objOutboundInq.LstManualAloc[0].Soline;
            Session["DtlLine"] = DtlLine;
            DueLine = objOutboundInq.LstManualAloc[0].DueLine;
            Session["DueLine"] = DueLine;
            Allocation_Line_Num = objOutboundInq.LstManualAloc[0].Line;
            GSoNum = objOutboundInq.LstManualAloc[0].so_num;
            Session["Sonum"] = GSoNum;
            ItmCode = objOutboundInq.LstManualAloc[0].itm_code;
            ItmNum = objOutboundInq.LstManualAloc[0].itm_num;
            ItmColor = objOutboundInq.LstManualAloc[0].itm_color;
            ItmSize = objOutboundInq.LstManualAloc[0].itm_size;
            WhsId = objOutboundInq.LstManualAloc[0].whs_id;
            OdrQty = objOutboundInq.LstManualAloc[0].due_qty;
            objOutboundInq.aloc_doc_id = p_str_Alocdocid;
            objOutboundInq.itm_num = ItmNum;
            objOutboundInq.itm_color = ItmColor;
            objOutboundInq.itm_size = ItmSize;
            objOutboundInq.whs_id = WhsId;
            objOutboundInq.style = ItmNum;
            objOutboundInq.color = ItmColor;
            objOutboundInq.size = ItmSize;
            objOutboundInq.whsid = WhsId;
            dtOutInq = new DataTable();
            List<OutboundInq> li = new List<OutboundInq>();
            //2: Initialize a object of type DataRow
            DataRow drOBAloc;


            //3: Initialize enough objects of type DataColumns
            DataColumn colCmpId = new DataColumn("cmp_id", typeof(string));
            DataColumn colLineNum = new DataColumn("LineNum", typeof(string));
            DataColumn colitm_code = new DataColumn("itm_code", typeof(string));
            DataColumn colitm_num = new DataColumn("itm_num", typeof(string));
            DataColumn colitm_color = new DataColumn("itm_color", typeof(string));
            DataColumn colitm_size = new DataColumn("itm_size", typeof(string));
            DataColumn collot_id = new DataColumn("lot_id", typeof(string));
            DataColumn colloc_id = new DataColumn("loc_id", typeof(string));
            DataColumn colrcvd_dt = new DataColumn("rcvd_dt", typeof(string));
            DataColumn colpkg_qty = new DataColumn("pkg_qty", typeof(int));
            DataColumn colpkg_cnt = new DataColumn("pkg_cnt", typeof(int));
            DataColumn colavail_qty = new DataColumn("avail_qty", typeof(decimal));
            DataColumn colpalet_id = new DataColumn("palet_id", typeof(string));
            DataColumn colpo_num = new DataColumn("po_num", typeof(string));
            DataColumn colreqqty = new DataColumn("reqQty", typeof(string));
            DataColumn colalocated = new DataColumn("alocated", typeof(string));
            DataColumn colAloc = new DataColumn("Aloc", typeof(string));
            DataColumn colChk = new DataColumn("colChk", typeof(string));//2018-03-13-001 Added By NIthya


            int lintCount = 0;

            //4: Adding DataColumns to DataTable dt
            dtOutInq.Columns.Add(colCmpId);
            dtOutInq.Columns.Add(colLineNum);
            dtOutInq.Columns.Add(colitm_code);
            dtOutInq.Columns.Add(colitm_num);
            dtOutInq.Columns.Add(colitm_color);
            dtOutInq.Columns.Add(colitm_size);
            dtOutInq.Columns.Add(collot_id);
            dtOutInq.Columns.Add(colloc_id);
            dtOutInq.Columns.Add(colrcvd_dt);
            dtOutInq.Columns.Add(colpkg_qty);
            dtOutInq.Columns.Add(colpkg_cnt);
            dtOutInq.Columns.Add(colavail_qty);
            dtOutInq.Columns.Add(colpalet_id);
            dtOutInq.Columns.Add(colpo_num);
            dtOutInq.Columns.Add(colreqqty);
            dtOutInq.Columns.Add(colalocated);
            dtOutInq.Columns.Add(colAloc);
            dtOutInq.Columns.Add(colChk);//2018-03-13-001 Added By NIthya



            objOutboundInq = ServiceObject.GetaloceditmanualList(objOutboundInq);
            objOutboundInq = ServiceObject.OutboundGETTEMPALOCDTL(objOutboundInq);
            for (int i = 0; i < objOutboundInq.LstManualAloc.Count(); i++)
            {
                string ItmCode1, ItmNum1, ItmColor1, ItmSize1, PkgQty1, LocId1, PaletId1, lstrPONum1 = string.Empty;
                string ItmCode2, ItmNum2, ItmColor2, ItmSize2, PkgQty2, LocId2, PaletId2, lstrPONum2 = string.Empty;
                int AlocQty = 0;
                int PPk = 0;
                decimal avail_qty = 0;
                ItmCode1 = objOutboundInq.LstManualAloc[i].itm_code.Trim();
                ItmNum1 = objOutboundInq.LstManualAloc[i].itm_num.Trim();
                ItmColor1 = objOutboundInq.LstManualAloc[i].itm_color.Trim();
                ItmSize1 = objOutboundInq.LstManualAloc[i].itm_size.Trim();
                PkgQty1 = Convert.ToString(objOutboundInq.LstManualAloc[i].pkg_qty).Trim();
                LocId1 = objOutboundInq.LstManualAloc[i].loc_id.Trim();
                objOutboundInq.loc_id = LocId1.Trim();
                objOutboundInq.lot_id = objOutboundInq.LstManualAloc[i].lot_id.Trim();
                PaletId1 = objOutboundInq.LstManualAloc[i].palet_id.Trim();
                objOutboundInq.palet_id = PaletId1;
                lstrPONum1 = objOutboundInq.LstManualAloc[i].po_num.Trim();
                objOutboundInq.rcvd_dt = objOutboundInq.LstManualAloc[i].rcvd_dt;
                objOutboundInq.pkg_qty = objOutboundInq.LstManualAloc[i].pkg_qty;
                objOutboundInq.pkg_cnt = objOutboundInq.LstManualAloc[i].pkg_cnt;
                objOutboundInq.avail_qty = objOutboundInq.LstManualAloc[i].avail_qty;
                objOutboundInq.po_num = lstrPONum1;
                objOutboundInq.reqQty = OdrQty;
                //objOutboundInq.alocated = 0;


                LineNum = LineNum + 1;
                //ServiceObject.InsertTempInventory(objStackInquiry);
                drOBAloc = dtOutInq.NewRow();

                dtOutInq.Rows.Add(drOBAloc);
                dtOutInq.Rows[lintCount][colCmpId] = objOutboundInq.cmp_id.ToString();
                dtOutInq.Rows[lintCount][colLineNum] = LineNum;
                dtOutInq.Rows[lintCount][colitm_code] = objOutboundInq.itm_code.ToString();
                dtOutInq.Rows[lintCount][colitm_num] = objOutboundInq.itm_num.ToString();
                dtOutInq.Rows[lintCount][colitm_color] = objOutboundInq.itm_color.ToString();
                dtOutInq.Rows[lintCount][colitm_size] = objOutboundInq.itm_size.ToString();
                dtOutInq.Rows[lintCount][collot_id] = objOutboundInq.lot_id.ToString();
                dtOutInq.Rows[lintCount][colloc_id] = objOutboundInq.loc_id.ToString();
                dtOutInq.Rows[lintCount][colrcvd_dt] = objOutboundInq.rcvd_dt.ToString();
                dtOutInq.Rows[lintCount][colpkg_qty] = objOutboundInq.pkg_qty.ToString();
                dtOutInq.Rows[lintCount][colpkg_cnt] = objOutboundInq.pkg_cnt.ToString();
                dtOutInq.Rows[lintCount][colavail_qty] = objOutboundInq.avail_qty.ToString();
                dtOutInq.Rows[lintCount][colpalet_id] = objOutboundInq.palet_id.ToString();
                dtOutInq.Rows[lintCount][colpo_num] = objOutboundInq.po_num.ToString();
                dtOutInq.Rows[lintCount][colreqqty] = objOutboundInq.reqQty.ToString();
                OutboundInq objOutboundInqDtltemp = new OutboundInq();
                objOutboundInqDtltemp.cmp_id = objOutboundInq.cmp_id;
                objOutboundInqDtltemp.LineNum = LineNum;
                objOutboundInqDtltemp.itm_code = objOutboundInq.itm_code;
                objOutboundInqDtltemp.itm_num = objOutboundInq.itm_num;
                objOutboundInqDtltemp.itm_color = objOutboundInq.itm_color;
                objOutboundInqDtltemp.itm_size = objOutboundInq.itm_size;
                objOutboundInqDtltemp.lot_id = objOutboundInq.lot_id;
                objOutboundInqDtltemp.loc_id = objOutboundInq.loc_id;
                objOutboundInqDtltemp.rcvd_dt = objOutboundInq.rcvd_dt;
                objOutboundInqDtltemp.pkg_qty = objOutboundInq.pkg_qty;
                objOutboundInqDtltemp.pkg_cnt = objOutboundInq.pkg_cnt;
                objOutboundInqDtltemp.avail_qty = objOutboundInq.avail_qty;
                avail_qty = objOutboundInqDtltemp.avail_qty;
                objOutboundInqDtltemp.palet_id = objOutboundInq.palet_id;
                objOutboundInqDtltemp.po_num = objOutboundInq.po_num;
                objOutboundInqDtltemp.reqQty = objOutboundInq.reqQty;

                objOutboundInqDtltemp.back_ordr_qty = Convert.ToInt32(objOutboundInq.avail_qty);
                for (int j = 0; j < objOutboundInq.LstAlocDtl.Count(); j++)
                {
                    ItmCode2 = objOutboundInq.LstAlocDtl[j].itm_code.Trim();
                    ItmNum2 = objOutboundInq.LstAlocDtl[j].itm_num.Trim();
                    ItmColor2 = objOutboundInq.LstAlocDtl[j].itm_color.Trim();
                    ItmSize2 = objOutboundInq.LstAlocDtl[j].itm_size.Trim();
                    PkgQty2 = Convert.ToString(objOutboundInq.LstAlocDtl[j].pkg_qty).Trim();
                    LocId2 = objOutboundInq.LstAlocDtl[j].loc_id.Trim();
                    PaletId2 = objOutboundInq.LstAlocDtl[j].Palet_id.Trim();
                    lstrPONum2 = objOutboundInq.LstAlocDtl[j].po_num.Trim();
                    AlocQty = objOutboundInq.LstAlocDtl[j].Aloc;
                    PPk = objOutboundInq.LstAlocDtl[j].pkg_qty;
                    if (ItmCode1 == ItmCode2 && ItmNum1 == ItmNum2 && ItmColor1 == ItmColor2 && ItmSize1 == ItmSize2 && PkgQty1 == PkgQty2 && LocId1 == LocId2 && PaletId1 == PaletId2 && lstrPONum1 == lstrPONum2)
                    {
                        objOutboundInqDtltemp.back_ordr_qty = 0;
                        objOutboundInqDtltemp.back_ordr_qty = Convert.ToInt32(avail_qty) - objOutboundInq.LstAlocDtl[j].Aloc;
                        //objOutboundInqDtltemp.back_ordr_qty =Convert.ToInt32(avail_qty);
                        int tmpPkgCnt = 0;
                        tmpPkgCnt = (AlocQty / PPk);
                        if (AlocQty % PPk != 0)
                        {
                            tmpPkgCnt = tmpPkgCnt + 1;
                        }
                        if (Allocation_Line_Num == objOutboundInq.LstAlocDtl[j].AlcLn)
                        {
                            objOutboundInq.LstManualAloc[i].Aloc = objOutboundInq.LstManualAloc[i].Aloc + objOutboundInq.LstAlocDtl[j].Aloc;
                            objOutboundInqDtltemp.Aloc = objOutboundInq.LstManualAloc[i].Aloc;
                            objOutboundInqDtltemp.colChk = "True";//2018-03-13-001 Added By NIthya

                        }
                    }
                }
                TotAvailQty = TotAvailQty + objOutboundInqDtltemp.avail_qty;
                if (objOutboundInqDtltemp.Aloc > 0)
                {
                    TotAlocQty = TotAlocQty + objOutboundInqDtltemp.Aloc;
                    objOutboundInqDtltemp.alocated = Convert.ToInt32(TotAlocQty);
                }
                li.Add(objOutboundInqDtltemp);
                lintCount++;
            }

            objOutboundInq.que_qty = OdrQty;
            objOutboundInq.available = Convert.ToInt32(TotAvailQty);
            objOutboundInq.reqQty = OdrQty;
            objOutboundInq.alocated = Convert.ToInt32(TotAlocQty);
            Session["lblAlocated"] = 0;
            Session["lblAlocated"] = objOutboundInq.alocated;//2018-03-13-001 Added By NIthya
            objOutboundInq.LstManualAloc = li;
            Session["GridInvoice"] = objOutboundInq.LstManualAloc;
            objOutboundInq.View_Flag = "M";
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundShipModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("__ManualAloc", objOutboundShipModel);
        }
        public ActionResult GetSummarydtl(int tablerowvalue, string p_str_cmp_id, string p_str_so_num, string p_str_itm_code)
        {

            try
            {
                OutboundInq objOutboundInq = new OutboundInq();
                IOutboundInqService ServiceObject = new OutboundInqService();
                objOutboundInq.cmp_id = p_str_cmp_id;
                objOutboundInq.line_num = tablerowvalue;
                Session["lineNum"] = tablerowvalue;
                objOutboundInq.so_num = p_str_so_num;
                objOutboundInq.itm_code = p_str_itm_code;
                objOutboundInq = ServiceObject.GetSelectedgridValue(objOutboundInq);
                Mapper.CreateMap<OutboundInq, OutboundInqModel>();
                OutboundInqModel objOutboundShipModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
                return PartialView("_AlocDetails", objOutboundShipModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult SaveEditAlocEntry(string p_str_cmp_id, string p_str_Alocdocid, string p_str_Alocdt, string p_str_Alocshiprqfm, string p_str_Alocdeldtfm,
         string p_str_Alocpricetkt, string p_str_Aloccustpo, string p_str_Alocwhsid, string p_str_Aloccustid,
          string p_str_Alocshiptoid, string p_str_Alocshiptoname, string p_str_Alocshipdt, string p_str_AlocCoustordrdt, string p_str_AlocOrdrno, string p_str_canceldt)
        {

            string Itm_Code, Lot_Id, Rcvd_Dt, Loc_Id, Aloc_Qty = string.Empty;
            string Item_Code = string.Empty;
            string Item_num = string.Empty;
            string Item_Color = string.Empty;
            string Item_Size = string.Empty;
            string So_Num = string.Empty;
            int Alloc_Qty = 0;
            int BO_Qty = 0;
            int Dtl_Line = 0;
            int Due_Line = 0;
            int SdtRec_Aloc_qty = 0;
            int Aloc_qty = 0;
            string Bak_OdrQty = string.Empty;
            int PrevLine = 1;
            int NewCtnQty = 0;
            string tmpStep = string.Empty;
            string tmpStatus = string.Empty;
            int Alo, CTN = 0;
            string Whsid, LocId = string.Empty;
            string New_Pkg_Id = string.Empty;
            string palet_id = string.Empty;
            int Ppk = 0;
            int UpdtPkg = 0;
            string SptCtnMsg = string.Empty;
            int NewQty = 0;
            int ctn_line = 0;
            string Line_Num = string.Empty;
            string OldPkgId = string.Empty;
            string WhsID = string.Empty;
            string ItmCode, ItmNum, ItmColor, ItmSize, ItmName, tmp_po, l_str_doc_pkg_id = string.Empty;
            int AlocQty, dueqty = 0;
            int LineNum = 0;
            int N = 0;
            decimal Availqty = 0;
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.cmp_id = p_str_cmp_id;
            objOutboundInq.AlocdocId = p_str_Alocdocid;
            objOutboundInq.aloc_dt = p_str_Alocdt;
            objOutboundInq.price_tkt = p_str_Alocpricetkt;
            objOutboundInq.ShipDt = p_str_Alocshipdt;
            objOutboundInq.CancelDt = p_str_canceldt;
            objOutboundInq.cust_id = p_str_Aloccustid;
            objOutboundInq.CustOrderNo = p_str_Aloccustpo;
            objOutboundInq.CustOrderdt = p_str_AlocCoustordrdt;
            objOutboundInq.orderNo = p_str_AlocOrdrno;
            objOutboundInq.whs_id = p_str_Alocwhsid;
            objOutboundInq.note = "";
            objOutboundInq.ship_to_id = p_str_Alocshiptoid;
            objOutboundInq.ship_to_name = p_str_Alocshiptoname;
            objOutboundInq.process_id = "";
            objOutboundInq = ServiceObject.GetALOCEditList(objOutboundInq);
            for (int i = 0; i < objOutboundInq.LstAlocDtl.Count(); i++)
            {
                objOutboundInq.cmp_id = p_str_cmp_id;
                objOutboundInq.itm_code = objOutboundInq.LstAlocDtl[i].itm_code;
                objOutboundInq.whs_id = objOutboundInq.LstAlocDtl[i].whs_id;
                objOutboundInq.lot_id = objOutboundInq.LstAlocDtl[i].lot_id;
                objOutboundInq.rcvd_dt = objOutboundInq.LstAlocDtl[i].rcvd_dt;
                objOutboundInq.loc_id = objOutboundInq.LstAlocDtl[i].loc_id;
                objOutboundInq.aloc_qty = objOutboundInq.LstAlocDtl[i].itm_qty;
                objOutboundInq.process_id = "";
                objOutboundInq = ServiceObject.Add_To_Trn_Hdr(objOutboundInq);
            }
            objOutboundInq = ServiceObject.Sp_Aloc_Mod_Daloc_iv_itm_trn_in(objOutboundInq);
            objOutboundInq = ServiceObject.Del_Alloc_Upd_SO(objOutboundInq);
            objOutboundInq = ServiceObject.Move_to_aloc_hdr(objOutboundInq);
            objOutboundInq = ServiceObject.OutboundGETTEMPALOCDTL(objOutboundInq);
            if (objOutboundInq.LstAlocDtl.Count() == 0)
            {
                objOutboundInq.l_str_aloc_aloc_dtls = false;
            }
            else
            {
                for (int l = 0; l < objOutboundInq.LstAlocDtl.Count(); l++)
                {
                    Itm_Code = objOutboundInq.LstAlocDtl[l].itm_code;
                    Loc_Id = objOutboundInq.LstAlocDtl[l].loc_id;
                    Rcvd_Dt = Convert.ToString(objOutboundInq.LstAlocDtl[l].rcvd_dt);
                    Lot_Id = objOutboundInq.LstAlocDtl[l].lot_id;
                    Aloc_Qty = Convert.ToString(objOutboundInq.LstAlocDtl[l].Aloc);
                    objOutboundInq.cmp_id = p_str_cmp_id;
                    objOutboundInq.itm_code = Itm_Code;
                    objOutboundInq.whs_id = p_str_Alocwhsid;
                    objOutboundInq.lot_id = Lot_Id;
                    objOutboundInq.rcvd_dt = Convert.ToDateTime(Rcvd_Dt);
                    objOutboundInq.loc_id = Loc_Id;
                    objOutboundInq.avail_qty = Convert.ToDecimal(Aloc_Qty);
                    objOutboundInq.process_id = "";
                    ServiceObject.Moveto_TrnHdr(objOutboundInq);
                }
                objOutboundInq = ServiceObject.OutboundGETTEMPALOCSUMMARY(objOutboundInq);
                objOutboundInq = ServiceObject.OutboundGETTEMPLIST(objOutboundInq);
                for (int m = 0; m < objOutboundInq.LstAlocSummary.Count(); m++)
                {
                    updateSodtl(m, p_str_cmp_id);
                    if (objOutboundInq.ReturnValue == 1)
                    {
                        objOutboundInq.aloc_line = objOutboundInq.LstAlocSummary[m].Line;
                        objOutboundInq.whs_id = objOutboundInq.LstAlocSummary[m].whs_id;
                        objOutboundInq.itm_code = objOutboundInq.LstAlocSummary[m].so_num;
                        objOutboundInq.so_num = objOutboundInq.LstAlocSummary[m].itm_code;
                        objOutboundInq.itm_num = objOutboundInq.LstAlocSummary[m].itm_num;
                        objOutboundInq.itm_color = objOutboundInq.LstAlocSummary[m].itm_color;
                        objOutboundInq.itm_size = objOutboundInq.LstAlocSummary[m].itm_size;
                        objOutboundInq.due_qty = objOutboundInq.LstAlocSummary[m].due_qty;
                        objOutboundInq.aloc_qty = objOutboundInq.LstAlocSummary[m].aloc_qty;
                        objOutboundInq.avail_qty = objOutboundInq.LstAlocSummary[m].avail_qty;
                        objOutboundInq.back_ordr_qty = objOutboundInq.LstAlocSummary[m].back_ordr_qty;
                        objOutboundInq.line_num = objOutboundInq.LstAlocSummary[m].Soline;
                        objOutboundInq.due_line = objOutboundInq.LstAlocSummary[m].DueLine;
                        ServiceObject.Move_To_Grd_Bad_Itm(objOutboundInq);
                        objOutboundInq.ReturnValue = 0;
                        continue;
                    }
                    Alloc_Qty = Convert.ToInt32(objOutboundInq.LstAlocSummary[m].aloc_qty);
                    if (Alloc_Qty == 0)
                    {
                        continue;
                    }
                    Move_to_aloc_dtl(m, p_str_cmp_id, p_str_Alocdocid);
                    if (objOutboundInq.ReturnValue == 1)
                    {
                        objOutboundInq.aloc_line = LineNum;
                        objOutboundInq.whs_id = WhsID;
                        objOutboundInq.itm_code = So_Num;
                        objOutboundInq.so_num = Item_Code;
                        objOutboundInq.itm_num = Item_num;
                        objOutboundInq.itm_color = Item_Color;
                        objOutboundInq.itm_size = Item_Size;
                        objOutboundInq.due_qty = dueqty;
                        objOutboundInq.aloc_qty = Alloc_Qty;
                        objOutboundInq.avail_qty = Availqty;
                        objOutboundInq.back_ordr_qty = BO_Qty;
                        objOutboundInq.line_num = Dtl_Line;
                        objOutboundInq.due_line = Due_Line;
                        ServiceObject.Move_To_Grd_Bad_Itm(objOutboundInq);
                        objOutboundInq.ReturnValue = 0;
                        continue;
                    }
                    Move_to_aloc_ctn(m, p_str_cmp_id, p_str_Alocdocid);
                    Update_Tbl_iv_itm_trn_in(m, p_str_cmp_id, p_str_Alocdocid, p_str_AlocOrdrno, p_str_Alocshiptoname);
                }
                objOutboundInq = ServiceObject.OutboundGETTEMPALOCSUMMARY(objOutboundInq);
                for (int p = 0; p < objOutboundInq.LstAlocSummary.Count(); p++)
                {
                    tmpStep = "ALOC";
                    tmpStatus = "A";
                    if (objOutboundInq.LstAlocSummary[p].back_ordr_qty > 0)
                    {
                        tmpStep = "B/O";
                        tmpStatus = "B";
                        break;
                    }
                }
                objOutboundInq.cmp_id = p_str_cmp_id;
                objOutboundInq.so_num = p_str_Alocshiprqfm;
                objOutboundInq.step = tmpStep;
                objOutboundInq.status = tmpStatus;
                ServiceObject.Change_SOHdr_Status_atAdd(objOutboundInq);
                objOutboundInq.l_str_aloc_aloc_dtls = true;
            }
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return Json(objOutboundInq.l_str_aloc_aloc_dtls, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddEditAutoAloc(string p_str_cmp_id, string p_str_Alocdocid, string SelectdID, string p_str_Sonum)
        {
            int l_int_linenum = 0;
            string ItmCode, ItmNum, ItmColor, ItmSize, WhsId, GSoNum = string.Empty;
            decimal TotAvailQty = 0;
            decimal TotAlocQty = 0;
            int DtlLine = 0;
            int DueLine = 0;
            int OdrQty = 0;
            int LineNum = 0;
            string l_str_wshid = string.Empty;
            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objOutboundInq.cmp_id = p_str_cmp_id;
            objOutboundInq.CompID = p_str_cmp_id;
            objOutboundInq.aloc_doc_id = p_str_Alocdocid;
            objOutboundInq.so_num = p_str_Sonum;
            objOutboundInq.itm_code = SelectdID;
            objOutboundInq = ServiceObject.OutboundGETALOCSORTSTMT(objOutboundInq);
            if (objOutboundInq.LstItmxCustdtl.Count > 0)
            {
                objOutboundInq.aloc_sort_stmt = objOutboundInq.LstItmxCustdtl[0].aloc_sort_stmt;
                strAlocSortStmt = objOutboundInq.aloc_sort_stmt;
                objOutboundInq.aloc_by = objOutboundInq.LstItmxCustdtl[0].aloc_by;
                strAlocBy = objOutboundInq.aloc_by;
            }
            objOutboundInq.aloc_sort_stmt = strAlocSortStmt;
            objOutboundInq.aloc_by = strAlocBy;
            objOutboundInq = ServiceObject.GetSelectedgridValueList(objOutboundInq);
            DtlLine = objOutboundInq.LstManualAloc[0].Soline;
            Session["DtlLine"] = DtlLine;
            DueLine = objOutboundInq.LstManualAloc[0].DueLine;
            Session["DueLine"] = DueLine;
            Allocation_Line_Num = objOutboundInq.LstManualAloc[0].Line;
            GSoNum = objOutboundInq.LstManualAloc[0].so_num;
            Session["Sonum"] = GSoNum;
            ItmCode = objOutboundInq.LstManualAloc[0].itm_code;
            ItmNum = objOutboundInq.LstManualAloc[0].itm_num;
            ItmColor = objOutboundInq.LstManualAloc[0].itm_color;
            ItmSize = objOutboundInq.LstManualAloc[0].itm_size;
            WhsId = objOutboundInq.LstManualAloc[0].whs_id;
            OdrQty = objOutboundInq.LstManualAloc[0].due_qty;
            objOutboundInq.aloc_doc_id = p_str_Alocdocid;
            objOutboundInq.itm_num = ItmNum;
            objOutboundInq.itm_color = ItmColor;
            objOutboundInq.itm_size = ItmSize;
            objOutboundInq.whs_id = WhsId;
            dtOutInq = new DataTable();
            List<OutboundInq> li = new List<OutboundInq>();
            //2: Initialize a object of type DataRow
            DataRow drOBAloc;


            //3: Initialize enough objects of type DataColumns
            DataColumn colCmpId = new DataColumn("cmp_id", typeof(string));
            DataColumn colLineNum = new DataColumn("LineNum", typeof(string));
            DataColumn colitm_code = new DataColumn("itm_code", typeof(string));
            DataColumn colitm_num = new DataColumn("itm_num", typeof(string));
            DataColumn colitm_color = new DataColumn("itm_color", typeof(string));
            DataColumn colitm_size = new DataColumn("itm_size", typeof(string));
            DataColumn collot_id = new DataColumn("lot_id", typeof(string));
            DataColumn colloc_id = new DataColumn("loc_id", typeof(string));
            DataColumn colrcvd_dt = new DataColumn("rcvd_dt", typeof(string));
            DataColumn colpkg_qty = new DataColumn("pkg_qty", typeof(int));
            DataColumn colpkg_cnt = new DataColumn("pkg_cnt", typeof(int));
            DataColumn colavail_qty = new DataColumn("avail_qty", typeof(decimal));
            DataColumn colpalet_id = new DataColumn("palet_id", typeof(string));
            DataColumn colpo_num = new DataColumn("po_num", typeof(string));
            DataColumn colreqqty = new DataColumn("reqQty", typeof(string));
            DataColumn colalocated = new DataColumn("alocated", typeof(string));
            DataColumn colAloc = new DataColumn("Aloc", typeof(string));
            DataColumn colChk = new DataColumn("colChk", typeof(string));



            int lintCount = 0;

            //4: Adding DataColumns to DataTable dt
            dtOutInq.Columns.Add(colCmpId);
            dtOutInq.Columns.Add(colLineNum);
            dtOutInq.Columns.Add(colitm_code);
            dtOutInq.Columns.Add(colitm_num);
            dtOutInq.Columns.Add(colitm_color);
            dtOutInq.Columns.Add(colitm_size);
            dtOutInq.Columns.Add(collot_id);
            dtOutInq.Columns.Add(colloc_id);
            dtOutInq.Columns.Add(colrcvd_dt);
            dtOutInq.Columns.Add(colpkg_qty);
            dtOutInq.Columns.Add(colpkg_cnt);
            dtOutInq.Columns.Add(colavail_qty);
            dtOutInq.Columns.Add(colpalet_id);
            dtOutInq.Columns.Add(colpo_num);
            dtOutInq.Columns.Add(colreqqty);
            dtOutInq.Columns.Add(colalocated);
            dtOutInq.Columns.Add(colAloc);
            objOutboundInq = ServiceObject.GetAddaloceditmanualList(objOutboundInq);
            objOutboundInq = ServiceObject.OutboundGETTEMPALOCDTL(objOutboundInq);
            for (int i = 0; i < objOutboundInq.LstManualAloc.Count(); i++)
            {
                string ItmCode1, ItmNum1, ItmColor1, ItmSize1, PkgQty1, LocId1, PaletId1, lstrPONum1 = string.Empty;
                string ItmCode2, ItmNum2, ItmColor2, ItmSize2, PkgQty2, LocId2, PaletId2, lstrPONum2 = string.Empty;
                int AlocQty = 0;
                int PPk = 0;
                decimal avail_qty = 0;
                ItmCode1 = objOutboundInq.LstManualAloc[i].itm_code.Trim();
                ItmNum1 = objOutboundInq.LstManualAloc[i].itm_num.Trim();
                ItmColor1 = objOutboundInq.LstManualAloc[i].itm_color.Trim();
                ItmSize1 = objOutboundInq.LstManualAloc[i].itm_size.Trim();
                PkgQty1 = Convert.ToString(objOutboundInq.LstManualAloc[i].pkg_qty).Trim();
                LocId1 = objOutboundInq.LstManualAloc[i].loc_id.Trim();
                objOutboundInq.loc_id = LocId1.Trim();
                objOutboundInq.lot_id = objOutboundInq.LstManualAloc[i].lot_id.Trim();
                PaletId1 = objOutboundInq.LstManualAloc[i].palet_id.Trim();
                objOutboundInq.palet_id = PaletId1;
                lstrPONum1 = objOutboundInq.LstManualAloc[i].po_num.Trim();
                objOutboundInq.rcvd_dt = objOutboundInq.LstManualAloc[i].rcvd_dt;
                objOutboundInq.pkg_qty = objOutboundInq.LstManualAloc[i].pkg_qty;
                objOutboundInq.pkg_cnt = objOutboundInq.LstManualAloc[i].pkg_cnt;
                objOutboundInq.avail_qty = objOutboundInq.LstManualAloc[i].avail_qty;
                objOutboundInq.po_num = lstrPONum1;
                objOutboundInq.reqQty = OdrQty;
                LineNum = LineNum + 1;
                drOBAloc = dtOutInq.NewRow();
                dtOutInq.Rows.Add(drOBAloc);
                dtOutInq.Rows[lintCount][colCmpId] = objOutboundInq.cmp_id.ToString();
                dtOutInq.Rows[lintCount][colLineNum] = LineNum;
                dtOutInq.Rows[lintCount][colitm_code] = objOutboundInq.itm_code.ToString();
                dtOutInq.Rows[lintCount][colitm_num] = objOutboundInq.itm_num.ToString();
                dtOutInq.Rows[lintCount][colitm_color] = objOutboundInq.itm_color.ToString();
                dtOutInq.Rows[lintCount][colitm_size] = objOutboundInq.itm_size.ToString();
                dtOutInq.Rows[lintCount][collot_id] = objOutboundInq.lot_id.ToString();
                dtOutInq.Rows[lintCount][colloc_id] = objOutboundInq.loc_id.ToString();
                dtOutInq.Rows[lintCount][colrcvd_dt] = objOutboundInq.rcvd_dt.ToString();
                dtOutInq.Rows[lintCount][colpkg_qty] = objOutboundInq.pkg_qty.ToString();
                dtOutInq.Rows[lintCount][colpkg_cnt] = objOutboundInq.pkg_cnt.ToString();
                dtOutInq.Rows[lintCount][colavail_qty] = objOutboundInq.avail_qty.ToString();
                dtOutInq.Rows[lintCount][colpalet_id] = objOutboundInq.palet_id.ToString();
                dtOutInq.Rows[lintCount][colpo_num] = objOutboundInq.po_num.ToString();
                dtOutInq.Rows[lintCount][colreqqty] = objOutboundInq.reqQty.ToString();

                OutboundInq objOutboundInqDtltemp = new OutboundInq();
                objOutboundInqDtltemp.cmp_id = objOutboundInq.cmp_id;
                objOutboundInqDtltemp.LineNum = LineNum;
                objOutboundInqDtltemp.itm_code = objOutboundInq.itm_code;
                objOutboundInqDtltemp.itm_num = objOutboundInq.itm_num;
                objOutboundInqDtltemp.itm_color = objOutboundInq.itm_color;
                objOutboundInqDtltemp.itm_size = objOutboundInq.itm_size;
                objOutboundInqDtltemp.lot_id = objOutboundInq.lot_id;
                objOutboundInqDtltemp.loc_id = objOutboundInq.loc_id;
                objOutboundInqDtltemp.rcvd_dt = objOutboundInq.rcvd_dt;
                objOutboundInqDtltemp.pkg_qty = objOutboundInq.pkg_qty;
                objOutboundInqDtltemp.pkg_cnt = objOutboundInq.pkg_cnt;
                objOutboundInqDtltemp.avail_qty = objOutboundInq.avail_qty;
                avail_qty = objOutboundInqDtltemp.avail_qty;
                objOutboundInqDtltemp.back_ordr_qty = Convert.ToInt32(objOutboundInq.avail_qty);
                objOutboundInqDtltemp.palet_id = objOutboundInq.palet_id;
                objOutboundInqDtltemp.po_num = objOutboundInq.po_num;
                objOutboundInqDtltemp.reqQty = objOutboundInq.reqQty;
                for (int j = 0; j < objOutboundInq.LstAlocDtl.Count(); j++)
                {
                    ItmCode2 = objOutboundInq.LstAlocDtl[j].itm_code.Trim();
                    ItmNum2 = objOutboundInq.LstAlocDtl[j].itm_num.Trim();
                    ItmColor2 = objOutboundInq.LstAlocDtl[j].itm_color.Trim();
                    ItmSize2 = objOutboundInq.LstAlocDtl[j].itm_size.Trim();
                    PkgQty2 = Convert.ToString(objOutboundInq.LstAlocDtl[j].pkg_qty).Trim();
                    LocId2 = objOutboundInq.LstAlocDtl[j].loc_id.Trim();
                    PaletId2 = objOutboundInq.LstAlocDtl[j].Palet_id.Trim();
                    lstrPONum2 = objOutboundInq.LstAlocDtl[j].po_num.Trim();
                    AlocQty = objOutboundInq.LstAlocDtl[j].Aloc;
                    PPk = objOutboundInq.LstAlocDtl[j].pkg_qty;
                    if (ItmCode1 == ItmCode2 && ItmNum1 == ItmNum2 && ItmColor1 == ItmColor2 && ItmSize1 == ItmSize2 && PkgQty1 == PkgQty2 && LocId1 == LocId2 && PaletId1 == PaletId2 && lstrPONum1 == lstrPONum2)
                    {
                        objOutboundInqDtltemp.back_ordr_qty = 0;
                        objOutboundInqDtltemp.back_ordr_qty = Convert.ToInt32(avail_qty) - objOutboundInq.LstAlocDtl[j].Aloc;

                        int tmpPkgCnt = 0;
                        tmpPkgCnt = (AlocQty / PPk);
                        if (AlocQty % PPk != 0)
                        {
                            tmpPkgCnt = tmpPkgCnt + 1;
                        }
                        if (Allocation_Line_Num == objOutboundInq.LstAlocDtl[j].AlcLn)
                        {
                            objOutboundInq.LstManualAloc[i].Aloc = objOutboundInq.LstManualAloc[i].Aloc + objOutboundInq.LstAlocDtl[j].Aloc;
                            objOutboundInqDtltemp.Aloc = objOutboundInq.LstManualAloc[i].Aloc;
                            objOutboundInqDtltemp.colChk = "True";//2018-03-13-001 Added By NIthya

                        }
                    }
                }
                TotAvailQty = TotAvailQty + objOutboundInqDtltemp.avail_qty;
                if (objOutboundInqDtltemp.Aloc > 0)
                {
                    TotAlocQty = TotAlocQty + objOutboundInqDtltemp.Aloc;
                    objOutboundInqDtltemp.alocated = Convert.ToInt32(TotAlocQty);

                }
                li.Add(objOutboundInqDtltemp);
                lintCount++;
            }

            objOutboundInq.que_qty = OdrQty;
            objOutboundInq.available = Convert.ToInt32(TotAvailQty);
            objOutboundInq.reqQty = OdrQty;
            objOutboundInq.alocated = Convert.ToInt32(TotAlocQty);
            Session["lblAlocated"] = 0;
            Session["lblAlocated"] = objOutboundInq.alocated;//2018-03-13-001 Added By NIthya
            objOutboundInq.LstManualAloc = li;
            Session["GridInvoice"] = objOutboundInq.LstManualAloc;
            objOutboundInq.View_Flag = "A";
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundShipModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("__ManualAloc", objOutboundShipModel);
        }
        public JsonResult DocAlocEditDisplayGridToTextbox(int p_str_RowNo, string UnCheckvalue, string Checkvalue, int p_str_Reqqty, int p_str_AlocatedQty, string firstColumn, int Alocated)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            List<OutboundInq> InvoiceLi = new List<OutboundInq>();
            InvoiceLi = Session["GridInvoice"] as List<OutboundInq>;
            var p_str_get_avail_qty = InvoiceLi.GroupBy(x => x.itm_num).Select(x => new { Count = x.Sum(y => y.Aloc) });
            var p_str_get_availqty = p_str_get_avail_qty.ToList();
            var l_str_get_availqty = Convert.ToDecimal(p_str_get_availqty[0].Count);
            var result = from r in InvoiceLi where r.LineNum == p_str_RowNo select new { r.loc_id, r.avail_qty, r.pkg_qty, r.Aloc, r.back_ordr_qty, r.LineNum, r.reqQty, r.alocated };
            var get_result = result.ToList();
            var l_str_loc_id = get_result[0].loc_id;
            var l_str_avail_qty = get_result[0].avail_qty;
            var l_str_pkg_qty = get_result[0].pkg_qty;
            var l_str_Aloc = get_result[0].Aloc;
            var l_str_back_ordr_qty = get_result[0].back_ordr_qty;
            var l_str_LineNum = get_result[0].LineNum;
            var l_str_reqQty = get_result[0].reqQty;
            var getdata10 = get_result[0].alocated;
            int linenum = p_str_RowNo;
            //int l_str_Alocated =Convert.ToInt32(Session["lblAlocated"].ToString());
            int l_str_Alocated = Alocated;
            int getdata7 = 0;
            getdata7 = l_str_Aloc;
            int totAloc_qty = 0;
            totAloc_qty = Convert.ToInt32(l_str_get_availqty);
            if (firstColumn == "True")
            {
                for (int j = 0; j < InvoiceLi.Count(); j++)
                {

                    InvoiceLi.Where(p => p.LineNum == p_str_RowNo).Select(u =>
                    {
                        u.colChk = "false";
                        return u;
                    }).ToList();
                }
                if (UnCheckvalue == "true")
                {
                    UnCheckvalue = "false";
                }
                else
                {
                    UnCheckvalue = "true";
                }

                if (UnCheckvalue == "false")
                {
                    if (p_str_Reqqty <= totAloc_qty)
                    {
                        for (int j = 0; j < InvoiceLi.Count(); j++)
                        {

                            InvoiceLi.Where(p => p.LineNum == p_str_RowNo).Select(u =>
                            {
                                u.colChk = "false";
                                return u;
                            }).ToList();
                        }
                        int Result = 1;
                        return Json(Result, JsonRequestBehavior.AllowGet);
                    }
                    if (p_str_AlocatedQty > 0)
                    {
                        l_str_Alocated = Alocated - l_str_Aloc;
                        l_str_back_ordr_qty = l_str_back_ordr_qty + l_str_Aloc;
                        l_str_Aloc = 0;
                        getdata7 = l_str_Aloc;
                        for (int j = 0; j < InvoiceLi.Count(); j++)
                        {

                            InvoiceLi.Where(p => p.LineNum == p_str_RowNo).Select(u =>
                            {
                                u.back_ordr_qty = l_str_back_ordr_qty; u.Aloc = l_str_Aloc;
                                return u;
                            }).ToList();
                        }
                    }
                    if (l_str_avail_qty >= (l_str_reqQty - totAloc_qty))
                    {
                        l_str_Alocated = l_str_Alocated + (l_str_reqQty - totAloc_qty);
                        l_str_back_ordr_qty = l_str_back_ordr_qty - (l_str_reqQty - totAloc_qty);
                        l_str_Aloc = l_str_reqQty - totAloc_qty;
                        for (int j = 0; j < InvoiceLi.Count(); j++)
                        {

                            InvoiceLi.Where(p => p.LineNum == p_str_RowNo).Select(u =>
                            {
                                u.back_ordr_qty = l_str_back_ordr_qty; u.Aloc = l_str_Aloc;
                                return u;
                            }).ToList();
                        }

                    }
                    else
                    {
                        l_str_Alocated = (l_str_Alocated + l_str_back_ordr_qty);
                        l_str_Aloc = l_str_back_ordr_qty;
                        l_str_back_ordr_qty = 0;
                        for (int j = 0; j < InvoiceLi.Count(); j++)
                        {

                            InvoiceLi.Where(p => p.LineNum == p_str_RowNo).Select(u =>
                            {
                                u.back_ordr_qty = l_str_back_ordr_qty; u.Aloc = l_str_Aloc;
                                return u;
                            }).ToList();
                        }
                    }
                }
                else
                {
                    l_str_Alocated = Alocated - l_str_Aloc;
                    l_str_back_ordr_qty = l_str_back_ordr_qty + l_str_Aloc;
                    l_str_Aloc = 0;
                    for (int j = 0; j < InvoiceLi.Count(); j++)
                    {

                        InvoiceLi.Where(p => p.LineNum == p_str_RowNo).Select(u =>
                        {
                            u.back_ordr_qty = l_str_back_ordr_qty; u.Aloc = l_str_Aloc;
                            return u;
                        }).ToList();
                    }
                }
            }
            else
            {
                for (int j = 0; j < InvoiceLi.Count(); j++)
                {

                    InvoiceLi.Where(p => p.LineNum == p_str_RowNo).Select(u =>
                    {
                        u.colChk = "True";
                        return u;
                    }).ToList();
                }
                if (UnCheckvalue == "true")
                {
                    UnCheckvalue = "false";
                }
                else
                {
                    UnCheckvalue = "true";
                }
                if (UnCheckvalue == "false")
                {
                    if (p_str_Reqqty <= totAloc_qty)
                    {
                        for (int j = 0; j < InvoiceLi.Count(); j++)
                        {

                            InvoiceLi.Where(p => p.LineNum == p_str_RowNo).Select(u =>
                            {
                                u.colChk = "false";
                                return u;
                            }).ToList();
                        }
                        int Result = 1;
                        return Json(Result, JsonRequestBehavior.AllowGet);
                    }
                    if (p_str_AlocatedQty > 0)
                    {
                        l_str_Alocated = Alocated - l_str_Aloc;
                        l_str_back_ordr_qty = l_str_back_ordr_qty + l_str_Aloc;
                        l_str_Aloc = 0;
                        getdata7 = l_str_Aloc;
                        for (int j = 0; j < InvoiceLi.Count(); j++)
                        {

                            InvoiceLi.Where(p => p.LineNum == p_str_RowNo).Select(u =>
                            {
                                u.back_ordr_qty = l_str_back_ordr_qty; u.Aloc = l_str_Aloc;
                                return u;
                            }).ToList();
                        }
                    }
                    if (l_str_avail_qty >= (l_str_reqQty - totAloc_qty))
                    {
                        l_str_Alocated = l_str_Alocated + (l_str_reqQty - totAloc_qty);
                        l_str_back_ordr_qty = l_str_back_ordr_qty - (l_str_reqQty - totAloc_qty);
                        l_str_Aloc = l_str_reqQty - totAloc_qty;
                        for (int j = 0; j < InvoiceLi.Count(); j++)
                        {

                            InvoiceLi.Where(p => p.LineNum == p_str_RowNo).Select(u =>
                            {
                                u.back_ordr_qty = l_str_back_ordr_qty; u.Aloc = l_str_Aloc;
                                return u;
                            }).ToList();
                        }

                    }
                    else
                    {
                        l_str_Alocated = (l_str_Alocated + l_str_back_ordr_qty);
                        l_str_Aloc = l_str_back_ordr_qty;
                        l_str_back_ordr_qty = 0;
                        for (int j = 0; j < InvoiceLi.Count(); j++)
                        {

                            InvoiceLi.Where(p => p.LineNum == p_str_RowNo).Select(u =>
                            {
                                u.back_ordr_qty = l_str_back_ordr_qty; u.Aloc = l_str_Aloc;
                                return u;
                            }).ToList();
                        }
                    }
                }
                else
                {
                    l_str_Alocated = Alocated - l_str_Aloc;
                    l_str_back_ordr_qty = l_str_back_ordr_qty + l_str_Aloc;
                    l_str_Aloc = 0;
                    for (int j = 0; j < InvoiceLi.Count(); j++)
                    {

                        InvoiceLi.Where(p => p.LineNum == p_str_RowNo).Select(u =>
                        {
                            u.back_ordr_qty = l_str_back_ordr_qty; u.Aloc = l_str_Aloc;
                            return u;
                        }).ToList();
                    }
                }
                //l_str_Alocated = Alocated - l_str_Aloc;
                //l_str_back_ordr_qty = l_str_back_ordr_qty + l_str_Aloc;
                //l_str_Aloc = 0;
                //getdata7 = l_str_Aloc;
                //for (int j = 0; j < InvoiceLi.Count(); j++)
                //{
                //    InvoiceLi.Where(p => p.LineNum == p_str_RowNo).Select(u =>
                //    {
                //        u.back_ordr_qty = l_str_back_ordr_qty; u.Aloc = l_str_Aloc;
                //        return u;
                //    }).ToList();
                //}
            }
            objOutboundInq.LstManualAloc = InvoiceLi;
            Session["GridInvoiceList"] = objOutboundInq.LstManualAloc;
            //Session["lblAlocated"] = 0;
            List<OutboundInq> GETInvoiceList = new List<OutboundInq>();
            GETInvoiceList = Session["GridInvoiceList"] as List<OutboundInq>;
            return Json(new { loc_id = l_str_loc_id, avail_qty = l_str_avail_qty, pkg_qty = l_str_pkg_qty, Aloc = l_str_Aloc, back_ordr_qty = l_str_back_ordr_qty, LineNum = l_str_LineNum, SelAloc = getdata7, alocated = l_str_Alocated, linenum = linenum }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DocAlocStyleEditDisplayGridToTextbox(int p_str_RowNo, string UnCheckvalue, string Checkvalue, int p_str_Reqqty, int p_str_AlocatedQty, string firstColumn, int Alocated)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            List<OutboundInq> InvoiceLi = new List<OutboundInq>();
            InvoiceLi = Session["GridInvoice"] as List<OutboundInq>;
            // InvoiceLi = Session["GridOpenInvoiceList"] as List<OutboundInq>;
            var p_str_get_avail_qty = InvoiceLi.GroupBy(x => x.itm_num).Select(x => new { Count = x.Sum(y => y.Aloc) });
            var p_str_get_availqty = p_str_get_avail_qty.ToList();
            var l_str_get_availqty = Convert.ToDecimal(p_str_get_availqty[0].Count);
            var result = from r in InvoiceLi where r.LineNum == p_str_RowNo select new { r.loc_id, r.avail_qty, r.pkg_qty, r.Aloc, r.back_ordr_qty, r.LineNum, r.reqQty, r.alocated };
            var get_result = result.ToList();
            var l_str_loc_id = get_result[0].loc_id;
            var l_str_avail_qty = get_result[0].avail_qty;
            var l_str_pkg_qty = get_result[0].pkg_qty;
            var l_str_Aloc = get_result[0].Aloc;
            var l_str_back_ordr_qty = get_result[0].back_ordr_qty;
            var l_str_LineNum = get_result[0].LineNum;
            var l_str_reqQty = get_result[0].reqQty;
            var getdata10 = get_result[0].alocated;
            int linenum = p_str_RowNo;
            //int l_str_Alocated =Convert.ToInt32(Session["lblAlocated"].ToString());
            int l_str_Alocated = SelAlocated;

            //int l_str_Alocate =Convert.ToInt32(Session["SelAlocated"].ToString());
            int getdata7 = 0;
            getdata7 = l_str_Aloc;
            int totAloc_qty = 0;
            // l_str_Aloc = Convert.ToInt32(l_str_get_availqty);
            totAloc_qty = Convert.ToInt32(l_str_get_availqty);
            if (firstColumn == "True")
            {
                for (int j = 0; j < InvoiceLi.Count(); j++)
                {

                    InvoiceLi.Where(p => p.LineNum == p_str_RowNo).Select(u =>
                    {
                        u.colChk = "false";
                        return u;
                    }).ToList();
                }
            }
            else
            {
                for (int j = 0; j < InvoiceLi.Count(); j++)
                {

                    InvoiceLi.Where(p => p.LineNum == p_str_RowNo).Select(u =>
                    {
                        u.colChk = "true";
                        return u;
                    }).ToList();
                }
            }
            l_str_Alocated = Alocated - l_str_Aloc;
            l_str_back_ordr_qty = l_str_back_ordr_qty + l_str_Aloc;
            l_str_Aloc = 0;
            for (int j = 0; j < InvoiceLi.Count(); j++)
            {

                InvoiceLi.Where(p => p.LineNum == p_str_RowNo).Select(u =>
                {
                    u.back_ordr_qty = l_str_back_ordr_qty; u.Aloc = l_str_Aloc;
                    return u;
                }).ToList();
            }
            objOutboundInq.LstManualAloc = InvoiceLi;
            Session["GridInvoiceList"] = objOutboundInq.LstManualAloc;
            //Session["lblAlocated"] = 0;
            List<OutboundInq> GETInvoiceList = new List<OutboundInq>();
            GETInvoiceList = Session["GridInvoiceList"] as List<OutboundInq>;
            return Json(new { loc_id = l_str_loc_id, avail_qty = l_str_avail_qty, pkg_qty = l_str_pkg_qty, Aloc = l_str_Aloc, back_ordr_qty = l_str_back_ordr_qty, LineNum = l_str_LineNum, SelAloc = getdata7, alocated = l_str_Alocated, linenum = linenum }, JsonRequestBehavior.AllowGet);
        }
        //CR180404-001 Added By Nithya
        public ActionResult IncludeManualAloc(int p_str_line_num, int p_str_Alocselalocated, int p_str_AlocreqQty, int Alocated)
        {
            int Sel_AlocQty;
            int totAloc_Qty;
            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            Company objCompany = new Company();
            List<OutboundInq> GETInvoiceLi = new List<OutboundInq>();
            GETInvoiceLi = Session["GridInvoiceList"] as List<OutboundInq>;
            if (GETInvoiceLi == null)
            {
                GETInvoiceLi = Session["GridInvoice"] as List<OutboundInq>;
                objOutboundInq.LstManualAloc = GETInvoiceLi;
                var result1 = from r in GETInvoiceLi where r.colChk == "True" select new { r.loc_id, r.avail_qty, r.pkg_qty, r.Aloc, r.back_ordr_qty, r.LineNum, r.reqQty, r.alocated };
                var get_result1 = result1.ToList();
                p_str_line_num = get_result1[0].LineNum;
            }
            if (p_str_line_num == 0)
            {
                var result2 = from r in GETInvoiceLi where r.colChk == "True" select new { r.loc_id, r.avail_qty, r.pkg_qty, r.Aloc, r.back_ordr_qty, r.LineNum, r.reqQty, r.alocated };
                var get_result2 = result2.ToList();
                p_str_line_num = get_result2[0].LineNum;
            }
            var p_str_get_avail_qty = GETInvoiceLi.GroupBy(x => x.itm_num).Select(x => new { Count = x.Sum(y => y.Aloc) });
            var p_str_get_availqty = p_str_get_avail_qty.ToList();
            var l_str_get_availqty = Convert.ToDecimal(p_str_get_availqty[0].Count);
            var result = from r in GETInvoiceLi where r.LineNum == p_str_line_num select new { r.loc_id, r.avail_qty, r.pkg_qty, r.Aloc, r.back_ordr_qty, r.LineNum, r.reqQty, r.alocated };
            var get_result = result.ToList();
            var l_str_loc_id = get_result[0].loc_id;
            var l_str_avail_qty = get_result[0].avail_qty;
            var l_str_pkg_qty = get_result[0].pkg_qty;
            var l_str_Aloc = get_result[0].Aloc;
            var l_str_back_ordr_qty = get_result[0].back_ordr_qty;
            var l_str_LineNum = get_result[0].LineNum;
            var l_str_reqQty = get_result[0].reqQty;
            int l_str_AlocatedQty = 0;
            int lintCount = 0;
            SelAlocated = p_str_Alocselalocated;
            int alocqty = (p_str_Alocselalocated % l_str_pkg_qty);
            Sel_AlocQty = l_str_Aloc;
            if (p_str_Alocselalocated == 0)
            {

                objOutboundInq.LstManualAloc = GETInvoiceLi;
                Session["GridOpenInvoiceList"] = objOutboundInq.LstManualAloc;
                List<OutboundInq> GETInvoiceList1 = new List<OutboundInq>();
                GETInvoiceList1 = Session["GridOpenInvoiceList"] as List<OutboundInq>;
                var P_str_get_Aloc = GETInvoiceList1.GroupBy(x => x.itm_num).Select(x => new { Count = x.Sum(y => y.Aloc) });
                var P_Str_get_Alocqty = P_str_get_Aloc.ToList();
                var l_Str_get_Alocqty = Convert.ToDecimal(P_Str_get_Alocqty[0].Count);
                var SelAlocate1 = l_Str_get_Alocqty;
                Session["SelAlocated"] = SelAlocate1;
                Mapper.CreateMap<OutboundInq, OutboundInqModel>();
                OutboundInqModel objOutboundShipReqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
                return PartialView("_AlocManualAloc", objOutboundShipReqModel);
            }
            else
            {
                if (p_str_AlocreqQty == 0)
                {
                    int Result = 1;
                    Session["GridOpenInvoiceList"] = GETInvoiceLi;
                    return Json(Result, JsonRequestBehavior.AllowGet);
                }
                //if (Sel_AlocQty != 0)
                //{
                //    int Result = 2;
                //    return Json(Result, JsonRequestBehavior.AllowGet);
                //}
                if (alocqty != 0)
                {
                    int Result = 4;
                    Session["GridOpenInvoiceList"] = GETInvoiceLi;
                    return Json(Result, JsonRequestBehavior.AllowGet);
                    
                }
                if (p_str_Alocselalocated > l_str_avail_qty)
                {
                    int Result = 5;
                    Session["GridOpenInvoiceList"] = GETInvoiceLi;
                    return Json(Result, JsonRequestBehavior.AllowGet);
                }
                if ((p_str_AlocreqQty - (l_str_get_availqty - Sel_AlocQty)) < p_str_Alocselalocated)
                {
                    int Result = 3;
                    Session["GridOpenInvoiceList"] = GETInvoiceLi;
                    return Json(Result, JsonRequestBehavior.AllowGet);
                }                            
                if (p_str_AlocreqQty <= p_str_Alocselalocated)
                {
                    l_str_back_ordr_qty = Convert.ToInt32(l_str_avail_qty - p_str_Alocselalocated);
                    l_str_Aloc = p_str_Alocselalocated;
                    totAloc_Qty = 0;
                    for (int j = 0; j < GETInvoiceLi.Count(); j++)
                    {

                        GETInvoiceLi.Where(p => p.LineNum == p_str_line_num).Select(u =>
                         {
                             u.back_ordr_qty = l_str_back_ordr_qty; u.Aloc = l_str_Aloc;
                             return u;
                         }).ToList();
                    }

                    l_str_AlocatedQty = Convert.ToInt32(l_str_get_availqty);
                }
                else
                {
                    l_str_back_ordr_qty = Convert.ToInt32(l_str_avail_qty - p_str_Alocselalocated);
                    l_str_Aloc = p_str_Alocselalocated;
                    for (int j = 0; j < GETInvoiceLi.Count(); j++)
                    {
                        GETInvoiceLi.Where(p => p.LineNum == p_str_line_num).Select(u =>
                         {
                             u.back_ordr_qty = l_str_back_ordr_qty; u.Aloc = l_str_Aloc;
                             return u;
                         }).ToList();
                    }
                    totAloc_Qty = 0;
                    l_str_AlocatedQty = Convert.ToInt32(l_str_get_availqty);
                }
            }
            objOutboundInq.LstManualAloc = GETInvoiceLi;
            Session["GridOpenInvoiceList"] = objOutboundInq.LstManualAloc;
            //CR-180406-001 Added By Nithya
            List<OutboundInq> GETInvoiceList = new List<OutboundInq>();
            GETInvoiceList = Session["GridOpenInvoiceList"] as List<OutboundInq>;
            var AlocQty = from r in GETInvoiceList where r.LineNum == p_str_line_num select new { r.loc_id, r.avail_qty, r.pkg_qty, r.Aloc, r.back_ordr_qty, r.LineNum, r.reqQty, r.alocated };
            var get_getAlocQty = AlocQty.ToList();
            var l_str_AlocQty = get_getAlocQty[0].Aloc;
            if (l_str_AlocQty > 0)
            {
                for (int j = 0; j < GETInvoiceList.Count(); j++)
                {

                    GETInvoiceList.Where(p => p.LineNum == p_str_line_num).Select(u =>
                    {
                        u.colChk = "True";
                        return u;
                    }).ToList();
                }
            }
            objOutboundInq.LstManualAloc = GETInvoiceList;
            Session["GridOpenInvoiceList"] = objOutboundInq.LstManualAloc;
            //END
            //var p_str_get_Aloc = GETInvoiceList.GroupBy(x => x.itm_num).Select(x => new { Count = x.Sum(y => y.Aloc) });
            //var p_str_get_Alocqty = p_str_get_Aloc.ToList();
            //var SelAlocate = Convert.ToDecimal(p_str_get_Alocqty[0].Count);
            //Session["SelAlocated"] = SelAlocate;
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundShipModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_AlocManualAloc", objOutboundShipModel);

        }
        public ActionResult IncludeManualAlocEdit(int p_str_line_num, int p_str_Alocselalocated, int p_str_AlocreqQty, int Alocated)
        {
            int Sel_AlocQty;
            int totAloc_Qty;
            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            Company objCompany = new Company();
            List<OutboundInq> GETInvoiceLi = new List<OutboundInq>();
            GETInvoiceLi = Session["GridInvoiceList"] as List<OutboundInq>;
            if (GETInvoiceLi == null)
            {
                GETInvoiceLi = Session["GridInvoice"] as List<OutboundInq>;
                objOutboundInq.LstManualAloc = GETInvoiceLi;
            }
            if (p_str_line_num == 0)
            {
                var result2 = from r in GETInvoiceLi where r.colChk == "True" select new { r.loc_id, r.avail_qty, r.pkg_qty, r.Aloc, r.back_ordr_qty, r.LineNum, r.reqQty, r.alocated };
                var get_result2 = result2.ToList();
                p_str_line_num = get_result2[0].LineNum;
            }
            var p_str_get_avail_qty = GETInvoiceLi.GroupBy(x => x.itm_num).Select(x => new { Count = x.Sum(y => y.Aloc) });
            var p_str_get_availqty = p_str_get_avail_qty.ToList();
            var l_str_get_availqty = Convert.ToDecimal(p_str_get_availqty[0].Count);
            var result = from r in GETInvoiceLi where r.LineNum == p_str_line_num select new { r.loc_id, r.avail_qty, r.pkg_qty, r.Aloc, r.back_ordr_qty, r.LineNum, r.reqQty, r.alocated };
            var get_result = result.ToList();
            var l_str_loc_id = get_result[0].loc_id;
            var l_str_avail_qty = get_result[0].avail_qty;
            var l_str_pkg_qty = get_result[0].pkg_qty;
            var l_str_Aloc = get_result[0].Aloc;
            var l_str_back_ordr_qty = get_result[0].back_ordr_qty;
            var l_str_LineNum = get_result[0].LineNum;
            var l_str_reqQty = get_result[0].reqQty;
            int l_str_AlocatedQty = 0;
            int lintCount = 0;
            int alocqty = (p_str_Alocselalocated % l_str_pkg_qty);
            Sel_AlocQty = l_str_Aloc;
            int l_int_avail_qty = Convert.ToInt32(l_str_avail_qty);
            if (p_str_Alocselalocated > (l_int_avail_qty))
            {
                int Result = 5;
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            if ((p_str_AlocreqQty - (l_str_get_availqty - Sel_AlocQty)) < p_str_Alocselalocated)
            {
                int Result = 3;
                Session["GridOpenInvoiceList"] = GETInvoiceLi;
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            if (p_str_AlocreqQty <= p_str_Alocselalocated)
            {
                l_str_back_ordr_qty = Convert.ToInt32(l_str_avail_qty - p_str_Alocselalocated);
                l_str_Aloc = p_str_Alocselalocated;
                totAloc_Qty = 0;
                for (int j = 0; j < GETInvoiceLi.Count(); j++)
                {

                    GETInvoiceLi.Where(p => p.LineNum == p_str_line_num).Select(u =>
                    {
                        u.back_ordr_qty = l_str_back_ordr_qty; u.Aloc = l_str_Aloc;
                        return u;
                    }).ToList();
                }

                l_str_AlocatedQty = Convert.ToInt32(l_str_get_availqty);
            }
            else
            {
                l_str_back_ordr_qty = Convert.ToInt32(l_str_avail_qty - p_str_Alocselalocated);
                l_str_Aloc = p_str_Alocselalocated;
                for (int j = 0; j < GETInvoiceLi.Count(); j++)
                {
                    GETInvoiceLi.Where(p => p.LineNum == p_str_line_num).Select(u =>
                    {
                        u.back_ordr_qty = l_str_back_ordr_qty; u.Aloc = l_str_Aloc;
                        return u;
                    }).ToList();
                }
                totAloc_Qty = 0;
                l_str_AlocatedQty = Convert.ToInt32(l_str_get_availqty);
            }
            objOutboundInq.LstManualAloc = GETInvoiceLi;
            Session["GridOpenInvoiceList"] = objOutboundInq.LstManualAloc;
            //CR-180406-001 Added By Nithya
            List<OutboundInq> GETInvoiceList = new List<OutboundInq>();
            GETInvoiceList = Session["GridOpenInvoiceList"] as List<OutboundInq>;
            var AlocQty = from r in GETInvoiceList where r.LineNum == p_str_line_num select new { r.loc_id, r.avail_qty, r.pkg_qty, r.Aloc, r.back_ordr_qty, r.LineNum, r.reqQty, r.alocated };
            var get_getAlocQty = AlocQty.ToList();
            var l_str_AlocQty = get_getAlocQty[0].Aloc;
            if (l_str_AlocQty > 0)
            {
                for (int j = 0; j < GETInvoiceList.Count(); j++)
                {

                    GETInvoiceList.Where(p => p.LineNum == p_str_line_num).Select(u =>
                    {
                        u.colChk = "True";
                        return u;
                    }).ToList();
                }
            }
            objOutboundInq.LstManualAloc = GETInvoiceList;
            Session["GridOpenInvoiceList"] = objOutboundInq.LstManualAloc;
            //END
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundShipModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_AlocManualAloc", objOutboundShipModel);
        }

        public ActionResult IncludeManualAvailQty(int p_str_line_num, int p_str_Alocselalocated, int p_str_AlocreqQty, int Alocated)
        {
            int Sel_AlocQty;
            int totAloc_Qty;
            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            Company objCompany = new Company();
            List<OutboundInq> GETInvoiceLi = new List<OutboundInq>();
            GETInvoiceLi = Session["GridInvoiceList"] as List<OutboundInq>;
            if (GETInvoiceLi == null)
            {
                GETInvoiceLi = Session["GridInvoice"] as List<OutboundInq>;
                objOutboundInq.LstManualAloc = GETInvoiceLi;
            }
            if (p_str_line_num == 0)
            {
                var result2 = from r in GETInvoiceLi where r.colChk == "True" select new { r.loc_id, r.avail_qty, r.pkg_qty, r.Aloc, r.back_ordr_qty, r.LineNum, r.reqQty, r.alocated };
                var get_result2 = result2.ToList();
                p_str_line_num = get_result2[0].LineNum;
            }
            var p_str_get_avail_qty = GETInvoiceLi.GroupBy(x => x.itm_num).Select(x => new { Count = x.Sum(y => y.Aloc) });
            var p_str_get_availqty = p_str_get_avail_qty.ToList();
            var l_str_get_availqty = Convert.ToDecimal(p_str_get_availqty[0].Count);
            var result = from r in GETInvoiceLi where r.LineNum == p_str_line_num select new { r.loc_id, r.avail_qty, r.pkg_qty, r.Aloc, r.back_ordr_qty, r.LineNum, r.reqQty, r.alocated };
            var get_result = result.ToList();
            var l_str_loc_id = get_result[0].loc_id;
            var l_str_avail_qty = get_result[0].avail_qty;
            var l_str_pkg_qty = get_result[0].pkg_qty;
            var l_str_Aloc = get_result[0].Aloc;
            var l_str_back_ordr_qty = get_result[0].back_ordr_qty;
            var l_str_LineNum = get_result[0].LineNum;
            var l_str_reqQty = get_result[0].reqQty;
            int l_str_AlocatedQty = 0;
            int lintCount = 0;
            int alocqty = (p_str_Alocselalocated % l_str_pkg_qty);
            Sel_AlocQty = l_str_Aloc;
            if (p_str_AlocreqQty <= p_str_Alocselalocated)
            {
                l_str_back_ordr_qty = Convert.ToInt32(l_str_avail_qty - p_str_Alocselalocated);
                l_str_Aloc = p_str_Alocselalocated;
                totAloc_Qty = 0;
                for (int j = 0; j < GETInvoiceLi.Count(); j++)
                {

                    GETInvoiceLi.Where(p => p.LineNum == p_str_line_num).Select(u =>
                    {
                        u.back_ordr_qty = l_str_back_ordr_qty; u.Aloc = l_str_Aloc;
                        return u;
                    }).ToList();
                }

                l_str_AlocatedQty = Convert.ToInt32(l_str_get_availqty);
            }
            else
            {
                l_str_back_ordr_qty = Convert.ToInt32(l_str_avail_qty - p_str_Alocselalocated);
                l_str_Aloc = p_str_Alocselalocated;
                for (int j = 0; j < GETInvoiceLi.Count(); j++)
                {
                    GETInvoiceLi.Where(p => p.LineNum == p_str_line_num).Select(u =>
                    {
                        u.back_ordr_qty = l_str_back_ordr_qty; u.Aloc = l_str_Aloc;
                        return u;
                    }).ToList();
                }
                totAloc_Qty = 0;
                l_str_AlocatedQty = Convert.ToInt32(l_str_get_availqty);
            }
            objOutboundInq.LstManualAloc = GETInvoiceLi;
            Session["GridOpenInvoiceList"] = objOutboundInq.LstManualAloc;
            //CR-180406-001 Added By Nithya
            List<OutboundInq> GETInvoiceList = new List<OutboundInq>();
            GETInvoiceList = Session["GridOpenInvoiceList"] as List<OutboundInq>;
            var AlocQty = from r in GETInvoiceList where r.LineNum == p_str_line_num select new { r.loc_id, r.avail_qty, r.pkg_qty, r.Aloc, r.back_ordr_qty, r.LineNum, r.reqQty, r.alocated };
            var get_getAlocQty = AlocQty.ToList();
            var l_str_AlocQty = get_getAlocQty[0].Aloc;
            if (l_str_AlocQty > 0)
            {
                for (int j = 0; j < GETInvoiceList.Count(); j++)
                {

                    GETInvoiceList.Where(p => p.LineNum == p_str_line_num).Select(u =>
                    {
                        u.colChk = "True";
                        return u;
                    }).ToList();
                }
            }
            objOutboundInq.LstManualAloc = GETInvoiceList;
            Session["GridOpenInvoiceList"] = objOutboundInq.LstManualAloc;
            //END
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundShipModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_AlocManualAloc", objOutboundShipModel);
        }
        //End
        public ActionResult ShowManualAloc()
        {

            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            List<OutboundInq> GETInvoiceLi = new List<OutboundInq>();
            GETInvoiceLi = Session["GridInvoiceList"] as List<OutboundInq>;
            objOutboundInq.LstManualAloc = GETInvoiceLi;
            Session["GridOpenInvoiceList"] = null;
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundShipModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_AlocManualAloc", objOutboundShipModel);
        }

        public ActionResult MAnualAlocSave(string p_str_cmp_id, string Whsid, int dueqty, int Availqty, string View_Flag)
        {
            string Itm_Code1 = string.Empty;
            string Ctn_Qty1, Aloc_Ctns1, Aloc_Qty1, ItmCtn_Qty2, Aloc_Ctns2, Aloc_Qty2 = string.Empty;
            string Itm_Num1 = string.Empty;
            string Itm_Color1 = string.Empty;
            string Itm_Size1 = string.Empty;
            string Itm_Code2, Itm_Num2, Itm_Color2, Itm_Size2 = string.Empty;
            int DtlLine1 = 0;
            int DueLine1 = 0;
            int DtlLine = 0;
            int DueLine = 0;
            int tmpAlocLineNum = 0;
            int Pkg_cnt = 0;
            string WhsId, LocId, Pkg_Type = string.Empty;
            decimal AvailQty = 0;
            int TotAlocQty = 0;
            int TotBalQty = 0;
            int Pos = 0;
            int PPK = 0;
            int PCS = 0;
            int BalQty = 0;
            int ctn_line = 0;
            int AlocQty = 0;
            int tmpAlocDueqty = 0;
            int line = 0;
            string palet_id, lot_id, tmp_po, Itm_Code = string.Empty;
            DateTime RcvdDt;
            string AllocationLineNum = string.Empty;
            string chkdone = string.Empty;
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            List<OutboundInq> GETInvoiceLi = new List<OutboundInq>();
            GETInvoiceLi = Session["GridOpenInvoiceList"] as List<OutboundInq>;
            if (GETInvoiceLi == null)
            {

                GETInvoiceLi = Session["GridInvoiceList"] as List<OutboundInq>;
                objOutboundInq.LstManualAloc = GETInvoiceLi;
                Session["GridInvoiceList"] = objOutboundInq.LstManualAloc;
                if (GETInvoiceLi == null)
                {
                    GETInvoiceLi = Session["GridInvoice"] as List<OutboundInq>;
                    objOutboundInq.LstManualAloc = GETInvoiceLi;
                }
                if (GETInvoiceLi == null)
                {
                    int Result = 1;  
                    return Json(Result, JsonRequestBehavior.AllowGet);
                }
                objOutboundInq.cmp_id = p_str_cmp_id;
                objOutboundInq = ServiceObject.OutboundGETTEMPALOCDTL(objOutboundInq);
                AllocationLineNum = Session["lineNum"].ToString();
                line = Convert.ToInt32(AllocationLineNum);
                Itm_Code1 = GETInvoiceLi[0].itm_code;
                Itm_Num1 = GETInvoiceLi[0].itm_num;
                Itm_Color1 = GETInvoiceLi[0].itm_color;
                Itm_Size1 = GETInvoiceLi[0].itm_size;
                l_int_dtlLine = Session["DtlLine"].ToString();
                l_int_dueLine = Session["DueLine"].ToString();
                GSoNum = Session["Sonum"].ToString();
                DtlLine = Convert.ToInt32(l_int_dtlLine);
                DueLine = Convert.ToInt32(l_int_dueLine);
                for (int l = 0; l < objOutboundInq.LstAlocDtl.Count(); l++)
                {
                    Allocation_Line_Num = objOutboundInq.LstAlocDtl[l].AlcLn;
                    Itm_Code2 = objOutboundInq.LstAlocDtl[l].itm_code;
                    Itm_Num2 = objOutboundInq.LstAlocDtl[l].itm_num;
                    Itm_Color2 = objOutboundInq.LstAlocDtl[l].itm_color;
                    Itm_Size2 = objOutboundInq.LstAlocDtl[l].itm_size;
                    DtlLine1 = objOutboundInq.LstAlocDtl[l].soline;
                    DueLine1 = objOutboundInq.LstAlocDtl[l].due_line;
                    tmpAlocLineNum = objOutboundInq.LstAlocDtl[l].AlcLn;
                    if (Itm_Code1 == Itm_Code2 && Itm_Num1 == Itm_Num2 && Itm_Color1 == Itm_Color2 && Itm_Size1 == Itm_Size2 && DtlLine == DtlLine1 && DueLine == DueLine1 && tmpAlocLineNum == Allocation_Line_Num)
                    {
                        objOutboundInq.aloc_line = Allocation_Line_Num;
                        objOutboundInq.ctn_line = ctn_line;
                        objOutboundInq.so_num = GSoNum;
                        objOutboundInq.itm_code = Itm_Code2;
                        objOutboundInq = ServiceObject.DeleteTemp_Alloc_Summary(objOutboundInq);
                        // objOutboundInq.LstAlocDtl.RemoveAt(l);
                    }
                }
                for (int j = 0; j < GETInvoiceLi.Count(); j++)
                {
                    Itm_Code = GETInvoiceLi[j].itm_code;
                    LocId = GETInvoiceLi[j].loc_id;
                    RcvdDt = GETInvoiceLi[j].rcvd_dt;
                    PPK = GETInvoiceLi[j].pkg_qty;
                    PCS = GETInvoiceLi[j].pkg_cnt;
                    AlocQty = GETInvoiceLi[j].Aloc;
                    BalQty = GETInvoiceLi[j].back_ordr_qty;
                    lot_id = GETInvoiceLi[j].lot_id;
                    palet_id = GETInvoiceLi[j].palet_id;
                    tmp_po = GETInvoiceLi[j].po_num;
                    AvailQty = GETInvoiceLi[j].avail_qty;
                    if (AlocQty > 0)
                    {
                        ctn_line = ctn_line + 1;

                        int AvlQtyTmp = 0;
                        if (View_Flag == "A")
                        {
                            AvlQtyTmp = ((PPK * PCS) + AlocQty + PPK - (AlocQty % PPK));
                            objOutboundInq.View_Flag = "A";//CR180404-001 Added By Nithya
                        }
                        else
                        {
                            AvlQtyTmp = (PPK * PCS);
                            objOutboundInq.View_Flag = "M";//CR180404-001 Added By Nithya
                        }
                        objOutboundInq.aloc_line = Convert.ToInt32(AllocationLineNum);
                        objOutboundInq.ctn_line = ctn_line;
                        objOutboundInq.line_num = DtlLine;
                        objOutboundInq.so_num = GSoNum;
                        objOutboundInq.due_line = DueLine;
                        objOutboundInq.itm_num = Itm_Num1;
                        objOutboundInq.due_qty = dueqty;
                        objOutboundInq.itm_color = Itm_Color1;
                        objOutboundInq.itm_size = Itm_Size1;
                        objOutboundInq.itm_code = Itm_Code1;
                        objOutboundInq.whs_id = Whsid;
                        objOutboundInq.loc_id = LocId;
                        objOutboundInq.pkg_qty = PPK;
                        objOutboundInq.Palet_id = palet_id;
                        objOutboundInq.po_num = tmp_po;
                        objOutboundInq.lot_id = lot_id;
                        objOutboundInq.rcvd_dt = RcvdDt;
                        objOutboundInq.back_ordr_qty = BalQty;
                        objOutboundInq.aloc_qty = AlocQty;
                        objOutboundInq.avail_qty = AvailQty;
                        ServiceObject.InsertTempAlocdtl(objOutboundInq);
                        TotAlocQty = TotAlocQty + AlocQty;
                        TotBalQty = TotBalQty + BalQty;
                    }
                }
                objOutboundInq = ServiceObject.OutboundGETTEMPALOCSUMMARY(objOutboundInq);
                for (int k = 0; k < objOutboundInq.LstAlocSummary.Count(); k++)
                {
                    Itm_Code2 = objOutboundInq.LstAlocSummary[k].itm_code;
                    Itm_Num2 = objOutboundInq.LstAlocSummary[k].itm_num;
                    Itm_Color2 = objOutboundInq.LstAlocSummary[k].itm_color;
                    Itm_Size2 = objOutboundInq.LstAlocSummary[k].itm_size;
                    DtlLine1 = objOutboundInq.LstAlocSummary[k].Soline;
                    DueLine1 = objOutboundInq.LstAlocSummary[k].DueLine;
                    tmpAlocLineNum = objOutboundInq.LstAlocSummary[k].Line;
                    tmpAlocDueqty = objOutboundInq.LstAlocSummary[k].due_qty;
                    objOutboundInq.Line = objOutboundInq.LstAlocSummary[k].Line;
                    objOutboundInq.so_num = objOutboundInq.LstAlocSummary[k].so_num;
                    objOutboundInq.itm_code = objOutboundInq.LstAlocSummary[k].itm_code;
                    if (Itm_Code1 == Itm_Code2 && Itm_Num1 == Itm_Num2 && Itm_Color1 == Itm_Color2 && Itm_Size1 == Itm_Size2 && DtlLine == DtlLine1 && DueLine == DueLine1 && tmpAlocLineNum == line)
                    {
                        objOutboundInq.aloc_qty = TotAlocQty;
                        objOutboundInq.avail_qty = Availqty;
                        objOutboundInq.back_ordr_qty = tmpAlocDueqty - TotAlocQty;
                        objOutboundInq = ServiceObject.UpdateTemp_Alloc_Summary(objOutboundInq);
                    }
                }
                objOutboundInq = ServiceObject.OutboundGETTEMPLIST(objOutboundInq);
                for (int m = 0; m < objOutboundInq.LstItmxCustdtl.Count(); m++)
                {
                    string itm_code = string.Empty;
                    string itm_num = string.Empty;
                    string itm_color = string.Empty;
                    string itm_size = string.Empty;
                    int itm_line = 0;
                    int dueQty = 0;
                    itm_code = objOutboundInq.LstItmxCustdtl[m].itm_code;
                    itm_num = objOutboundInq.LstItmxCustdtl[m].itm_num;
                    itm_color = objOutboundInq.LstItmxCustdtl[m].itm_color;
                    itm_size = objOutboundInq.LstItmxCustdtl[m].itm_size;
                    itm_line = objOutboundInq.LstItmxCustdtl[m].line_num;
                    dueQty = objOutboundInq.LstItmxCustdtl[m].due_qty;
                    if (itm_code == Itm_Code1 && itm_num == Itm_Num1 && itm_color == Itm_Color1 && itm_size == Itm_Size1 && itm_line == line)
                    {
                        objOutboundInq.aloc_qty = TotAlocQty;
                        objOutboundInq.due_qty = dueQty;
                        if (dueQty < TotAlocQty)
                        {
                            objOutboundInq.back_ordr_qty = 0;
                        }
                        else
                        {
                            objOutboundInq.back_ordr_qty = dueQty - TotAlocQty;
                        }
                        objOutboundInq = ServiceObject.Update_Tbl_Temp_So_Auto_Aloc(objOutboundInq);
                        break;
                    }
                }    
                objOutboundInq = ServiceObject.OutboundGETTEMPALOCDTL(objOutboundInq);
                objOutboundInq = ServiceObject.OutboundGETTEMPALOCSUMMARY(objOutboundInq);
                Session["GridInvoiceList"] = null;//CR-180409
                Session["GridInvoice"] = null;               
                Mapper.CreateMap<OutboundInq, OutboundInqModel>();
                OutboundInqModel objOutboundInqAlocModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
                return PartialView("_AlocSummaryDetail", objOutboundInqAlocModel);
            }
            else
            {
                objOutboundInq.cmp_id = p_str_cmp_id;
                objOutboundInq = ServiceObject.OutboundGETTEMPALOCDTL(objOutboundInq);
                AllocationLineNum = Session["lineNum"].ToString();
                line = Convert.ToInt32(AllocationLineNum);
                Itm_Code1 = GETInvoiceLi[0].itm_code;
                Itm_Num1 = GETInvoiceLi[0].itm_num;
                Itm_Color1 = GETInvoiceLi[0].itm_color;
                Itm_Size1 = GETInvoiceLi[0].itm_size;
                l_int_dtlLine = Session["DtlLine"].ToString();
                l_int_dueLine = Session["DueLine"].ToString();
                GSoNum = Session["Sonum"].ToString();
                DtlLine = Convert.ToInt32(l_int_dtlLine);
                DueLine = Convert.ToInt32(l_int_dueLine);
                for (int l = 0; l < objOutboundInq.LstAlocDtl.Count(); l++)
                {
                    Allocation_Line_Num = objOutboundInq.LstAlocDtl[l].AlcLn;
                    Itm_Code2 = objOutboundInq.LstAlocDtl[l].itm_code;
                    Itm_Num2 = objOutboundInq.LstAlocDtl[l].itm_num;
                    Itm_Color2 = objOutboundInq.LstAlocDtl[l].itm_color;
                    Itm_Size2 = objOutboundInq.LstAlocDtl[l].itm_size;
                    DtlLine1 = objOutboundInq.LstAlocDtl[l].soline;
                    DueLine1 = objOutboundInq.LstAlocDtl[l].due_line;
                    tmpAlocLineNum = objOutboundInq.LstAlocDtl[l].AlcLn;
                    if (Itm_Code1 == Itm_Code2 && Itm_Num1 == Itm_Num2 && Itm_Color1 == Itm_Color2 && Itm_Size1 == Itm_Size2 && DtlLine == DtlLine1 && DueLine == DueLine1 && tmpAlocLineNum == Allocation_Line_Num)
                    {
                        objOutboundInq.aloc_line = Allocation_Line_Num;
                        objOutboundInq.ctn_line = ctn_line;
                        objOutboundInq.so_num = GSoNum;
                        objOutboundInq.itm_code = Itm_Code2;
                        objOutboundInq = ServiceObject.DeleteTemp_Alloc_Summary(objOutboundInq);
                        // objOutboundInq.LstAlocDtl.RemoveAt(l);
                    }
                }
                for (int j = 0; j < GETInvoiceLi.Count(); j++)
                {
                    Itm_Code = GETInvoiceLi[j].itm_code;
                    LocId = GETInvoiceLi[j].loc_id;
                    RcvdDt = GETInvoiceLi[j].rcvd_dt;
                    PPK = GETInvoiceLi[j].pkg_qty;
                    PCS = GETInvoiceLi[j].pkg_cnt;
                    AlocQty = GETInvoiceLi[j].Aloc;
                    BalQty = GETInvoiceLi[j].back_ordr_qty;
                    lot_id = GETInvoiceLi[j].lot_id;
                    palet_id = GETInvoiceLi[j].palet_id;
                    tmp_po = GETInvoiceLi[j].po_num;
                    AvailQty = GETInvoiceLi[j].avail_qty;
                    if (AlocQty > 0)
                    {
                        ctn_line = ctn_line + 1;

                        int AvlQtyTmp = 0;
                        if (View_Flag == "A")
                        {
                            AvlQtyTmp = ((PPK * PCS) + AlocQty + PPK - (AlocQty % PPK));
                            objOutboundInq.View_Flag = "A";//CR180404-001 Added By Nithya
                        }
                        else
                        {
                            AvlQtyTmp = (PPK * PCS);
                            objOutboundInq.View_Flag = "M";//CR180404-001 Added By Nithya
                        }
                        objOutboundInq.aloc_line = Convert.ToInt32(AllocationLineNum);
                        objOutboundInq.ctn_line = ctn_line;
                        objOutboundInq.line_num = DtlLine;
                        objOutboundInq.so_num = GSoNum;
                        objOutboundInq.due_line = DueLine;
                        objOutboundInq.itm_num = Itm_Num1;
                        objOutboundInq.due_qty = dueqty;
                        objOutboundInq.itm_color = Itm_Color1;
                        objOutboundInq.itm_size = Itm_Size1;
                        objOutboundInq.itm_code = Itm_Code1;
                        objOutboundInq.whs_id = Whsid;
                        objOutboundInq.loc_id = LocId;
                        objOutboundInq.pkg_qty = PPK;
                        objOutboundInq.Palet_id = palet_id;
                        objOutboundInq.po_num = tmp_po;
                        objOutboundInq.lot_id = lot_id;
                        objOutboundInq.rcvd_dt = RcvdDt;
                        objOutboundInq.back_ordr_qty = BalQty;
                        objOutboundInq.aloc_qty = AlocQty;
                        objOutboundInq.avail_qty = AvailQty;
                        ServiceObject.InsertTempAlocdtl(objOutboundInq);
                        TotAlocQty = TotAlocQty + AlocQty;
                        TotBalQty = TotBalQty + BalQty;
                    }
                }
                objOutboundInq = ServiceObject.OutboundGETTEMPALOCSUMMARY(objOutboundInq);
                for (int k = 0; k < objOutboundInq.LstAlocSummary.Count(); k++)
                {
                    Itm_Code2 = objOutboundInq.LstAlocSummary[k].itm_code;
                    Itm_Num2 = objOutboundInq.LstAlocSummary[k].itm_num;
                    Itm_Color2 = objOutboundInq.LstAlocSummary[k].itm_color;
                    Itm_Size2 = objOutboundInq.LstAlocSummary[k].itm_size;
                    DtlLine1 = objOutboundInq.LstAlocSummary[k].Soline;
                    DueLine1 = objOutboundInq.LstAlocSummary[k].DueLine;
                    tmpAlocLineNum = objOutboundInq.LstAlocSummary[k].Line;
                    tmpAlocDueqty = objOutboundInq.LstAlocSummary[k].due_qty;
                    objOutboundInq.Line = objOutboundInq.LstAlocSummary[k].Line;
                    objOutboundInq.so_num = objOutboundInq.LstAlocSummary[k].so_num;
                    objOutboundInq.itm_code = objOutboundInq.LstAlocSummary[k].itm_code;
                    if (Itm_Code1 == Itm_Code2 && Itm_Num1 == Itm_Num2 && Itm_Color1 == Itm_Color2 && Itm_Size1 == Itm_Size2 && DtlLine == DtlLine1 && DueLine == DueLine1 && tmpAlocLineNum == line)
                    {
                        objOutboundInq.aloc_qty = TotAlocQty;
                        objOutboundInq.avail_qty = Availqty;
                        objOutboundInq.back_ordr_qty = tmpAlocDueqty - TotAlocQty;
                        objOutboundInq = ServiceObject.UpdateTemp_Alloc_Summary(objOutboundInq);
                    }
                }
                objOutboundInq = ServiceObject.OutboundGETTEMPLIST(objOutboundInq);
                for (int m = 0; m < objOutboundInq.LstItmxCustdtl.Count(); m++)
                {
                    string itm_code = string.Empty;
                    string itm_num = string.Empty;
                    string itm_color = string.Empty;
                    string itm_size = string.Empty;
                    int itm_line = 0;
                    int dueQty = 0;
                    itm_code = objOutboundInq.LstItmxCustdtl[m].itm_code;
                    itm_num = objOutboundInq.LstItmxCustdtl[m].itm_num;
                    itm_color = objOutboundInq.LstItmxCustdtl[m].itm_color;
                    itm_size = objOutboundInq.LstItmxCustdtl[m].itm_size;
                    itm_line = objOutboundInq.LstItmxCustdtl[m].line_num;
                    dueQty = objOutboundInq.LstItmxCustdtl[m].due_qty;
                    if (itm_code == Itm_Code1 && itm_num == Itm_Num1 && itm_color == Itm_Color1 && itm_size == Itm_Size1 && itm_line == line)
                    {
                        objOutboundInq.aloc_qty = TotAlocQty;
                        objOutboundInq.due_qty = dueQty;
                        if (dueQty < TotAlocQty)
                        {
                            objOutboundInq.back_ordr_qty = 0;
                        }
                        else
                        {
                            objOutboundInq.back_ordr_qty = dueQty - TotAlocQty;
                        }
                        objOutboundInq = ServiceObject.Update_Tbl_Temp_So_Auto_Aloc(objOutboundInq);
                        break;
                    }
                }
                objOutboundInq = ServiceObject.OutboundGETTEMPALOCDTL(objOutboundInq);
                objOutboundInq = ServiceObject.OutboundGETTEMPALOCSUMMARY(objOutboundInq);
                Session["GridInvoiceList"] = null;//CR-180409
                Session["GridInvoice"] = null;               
                Mapper.CreateMap<OutboundInq, OutboundInqModel>();
                OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
                return PartialView("_AlocSummaryDetail", objOutboundInqModel);
            }
        }
        public ActionResult Outbounddocupld(string cmp_id, string ibdocid, string cntrid, string datefrom, string dateto)
        {
            OutboundInq OutboundInq = new OutboundInq();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            OutboundInq.CompID = cmp_id;
            OutboundInq.cmp_id = cmp_id;
            OutboundInq.b_quote_num = ibdocid;
            OutboundInq.Container = cntrid;
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel OutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(OutboundInq);
            return PartialView("_OutboundInquiryImportFile", OutboundInqModel);
        }
        public ActionResult OutboundInquiryFileDocument(string cmp_id, string ibdocid, string cntrid, string datefrom, string dateto)
        {
            string name = string.Empty;
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            objInboundInquiry.CompID = cmp_id;
            objInboundInquiry.cmp_id = cmp_id;
            objInboundInquiry.ibdocid = ibdocid;
            objInboundInquiry.Container = cntrid;
            objInboundInquiry.DocumentdateFrom = datefrom;
            objInboundInquiry.DocumentdateTo = dateto;
            objInboundInquiry.doctype = "OUTBOUND";
            string path = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
            string directoryPath = Path.Combine((path), cmp_id);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(Path.Combine(directoryPath));
            }
            directoryPath = Path.Combine(directoryPath, "OUTBOUND");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(Path.Combine(directoryPath));
            }
            directoryPath = Path.Combine(directoryPath, ibdocid);
            DirectoryInfo dir1 = new DirectoryInfo(directoryPath);
            int count = 0;
            count = dir1.GetFiles().Length;
            if (count > 1)
            {
                string lstrAlocList;
                lstrAlocList = ("");
                foreach (FileInfo flInfo in dir1.GetFiles())
                {
                    name = flInfo.Name;
                    objInboundInquiry.Filename = name;
                    long size = flInfo.Length;
                    lstrAlocList = lstrAlocList + objInboundInquiry.Filename + ",";
                }
                lstrAlocList = lstrAlocList.Remove(lstrAlocList.Length - 1, 1);
                lstrAlocList = lstrAlocList + "";
                objInboundInquiry.Filename = lstrAlocList;
            }
            else
            {
                foreach (FileInfo flInfo in dir1.GetFiles())
                {
                    name = flInfo.Name;
                    objInboundInquiry.Filename = name;
                    long size = flInfo.Length;
                }

            }
            objInboundInquiry = ServiceObject.GetTempFiledtl(objInboundInquiry);
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryDocModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_InboundInquiryImportFile", InboundInquiryDocModel);
        }
        public ActionResult ShowvasdtlReport(string p_str_cmpid, string p_str_status, string p_str_Shipping_id)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            try
            {
                if (isValid)
                {

                    strReportName = "rpt_iv_vas.rpt";
                    VasInquiry objVasInquiry = new VasInquiry();
                    IVasInquiryService ServiceObject = new VasInquiryService();
                    ReportDocument rd = new ReportDocument();
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//VasInquiry//" + strReportName;
                    objVasInquiry.cmp_id = p_str_cmpid;
                    objVasInquiry.ship_doc_id = p_str_Shipping_id;
                    objVasInquiry = ServiceObject.GetVasPostDetails(objVasInquiry);
                    var rptSource = objVasInquiry.ListVasInquiry.ToList();
                    rd.Load(strRptPath);
                    int AlocCount = 0;
                    AlocCount = objVasInquiry.ListVasInquiry.Count();
                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                    rd.SetDataSource(rptSource); 
                    objVasInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                    rd.SetParameterValue("fml_image_path", objVasInquiry.Image_Path);
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
        //CR_3PL_MVC_BL_2018_0226_001 Modified By Ravi 26-02-2017
        [HttpPost]
        public ActionResult GridUploadFiles(string CompID, string ibdocid, string comment, string P_STR_Container)
        {
            string name = string.Empty;
            string tempfilename = string.Empty;
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            //ServiceObject2.StyleInquiryTempDelete(objStyleInquiry2);
            Session["CompanyID"] = CompID;
            objInboundInquiry.ibdocid = ibdocid;
            Session["Lstibdocid"] = ibdocid;
            Session["AssignValue"] = "InsertedValue";
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {

                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        string filename = Path.GetFileName(Request.Files[i].FileName);


                        HttpPostedFileBase FileUpload = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = FileUpload.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = FileUpload.FileName;
                            //string extension = Path.GetExtension(fname);
                            //fname.Replace(extension, ".csv");
                            //fname = Path.ChangeExtension(fname, ".csv");
                        }

                        InboundInquiryModel objStyleInquiryModel = new InboundInquiryModel();
                        InboundInquiryService ServiceObject1 = new InboundInquiryService();

                        // Set up DataTable place holder
                        DataTable dt1 = new DataTable();

                        //check we have a file
                        if (FileUpload != null)
                        {
                            if (FileUpload.ContentLength > 0)
                            {
                                //Workout our file path
                                //Added By Ravi 26-03-2017
                                string fileNameOnly = Path.GetFileNameWithoutExtension(FileUpload.FileName);
                                //END
                                string fileName = Path.GetFileName(FileUpload.FileName);
                                string extension = System.IO.Path.GetExtension(fileName).ToLower();
                                tempfilename = string.Format(fileNameOnly + "-" + CompID + "-" + ibdocid + "-" + P_STR_Container + extension);
                                // For Getting file Extension

                                string query = null;
                                string connString = "";
                                string[] validFileTypes = { ".xls", ".xlsx", ".csv" };
                                string path = string.Empty;
                                //string path1 = string.Empty;
                                //string path2 = string.Empty;
                                path = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
                                string directoryPath = Path.Combine((path), CompID);
                                if (!Directory.Exists(directoryPath))
                                {
                                    Directory.CreateDirectory(Path.Combine(directoryPath));
                                }
                                directoryPath = Path.Combine(directoryPath, "OUTBOUND");
                                if (!Directory.Exists(directoryPath))
                                {
                                    Directory.CreateDirectory(Path.Combine(directoryPath));
                                }
                                directoryPath = Path.Combine(directoryPath, ibdocid);
                                if (!Directory.Exists(directoryPath))
                                {
                                    Directory.CreateDirectory(Path.Combine(directoryPath));
                                }
                                DirectoryInfo dir2 = new DirectoryInfo(directoryPath);
                                int counts;
                                counts = dir2.GetFiles().Length;

                                int getcount = counts + 1;
                                tempfilename = string.Format(fileNameOnly + "-" + CompID + "-" + ibdocid + "-" + P_STR_Container + "000" + getcount + extension);

                                path = Path.Combine(directoryPath, fileName);
                                FileUpload.SaveAs(path);
                            }
                        }
                        else
                        {
                            //Catch errors
                            ViewData["Feedback"] = "Please select a file";
                        }
                        dt1.Dispose();
                    }
                    objInboundInquiry.cmp_id = CompID;
                    objInboundInquiry.doctype = "OUTBOUND";
                    objInboundInquiry.Uploadby = Session["UserID"].ToString().Trim();
                    objInboundInquiry.Comments = comment;
                    objInboundInquiry.UPLOAD_FILE = tempfilename;
                    string path1 = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
                    string directoryPath1 = Path.Combine((path1), CompID);
                    if (!Directory.Exists(directoryPath1))
                    {
                        Directory.CreateDirectory(Path.Combine(directoryPath1));
                    }
                    directoryPath1 = Path.Combine(directoryPath1, "OUTBOUND");
                    if (!Directory.Exists(directoryPath1))
                    {
                        Directory.CreateDirectory(Path.Combine(directoryPath1));
                    }
                    directoryPath1 = Path.Combine(directoryPath1, ibdocid);

                    DirectoryInfo dir = new DirectoryInfo(directoryPath1);
                    foreach (FileInfo flInfo in dir.GetFiles())
                    {
                        name = flInfo.Name;
                        objInboundInquiry.Filename = name;
                        long size = flInfo.Length;
                        DateTime creationTime = flInfo.CreationTime;
                        objInboundInquiry.Uploaddt = creationTime;
                        objInboundInquiry.filepath = directoryPath1;
                        path1 = Path.Combine(directoryPath1, name);
                        if (!Directory.Exists(path1))
                        {
                            objInboundInquiry.Filename = name;
                            ServiceObject.InsertTempFileDocument(objInboundInquiry);
                        }
                    }
                    objInboundInquiry.cmp_id = CompID;
                    objInboundInquiry.doctype = "OUTBOUND";
                    string path2 = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
                    string directoryPath2 = Path.Combine((path2), CompID);
                    if (!Directory.Exists(directoryPath1))
                    {
                        Directory.CreateDirectory(Path.Combine(directoryPath2));
                    }
                    directoryPath2 = Path.Combine(directoryPath2, "OUTBOUND");
                    if (!Directory.Exists(directoryPath2))
                    {
                        Directory.CreateDirectory(Path.Combine(directoryPath2));
                    }
                    directoryPath2 = Path.Combine(directoryPath2, ibdocid);
                    DirectoryInfo dir1 = new DirectoryInfo(directoryPath2);
                    int count = 0;
                    count = dir1.GetFiles().Length;
                    if (count > 1)
                    {
                        string lstrAlocList;
                        lstrAlocList = ("");
                        foreach (FileInfo flInfo in dir1.GetFiles())
                        {
                            name = flInfo.Name;
                            objInboundInquiry.Filename = name;
                            long size = flInfo.Length;
                            lstrAlocList = lstrAlocList + objInboundInquiry.Filename + ",";
                        }
                        lstrAlocList = lstrAlocList.Remove(lstrAlocList.Length - 1, 1);
                        lstrAlocList = lstrAlocList + "";
                        objInboundInquiry.Filename = lstrAlocList;
                    }
                    else
                    {
                        foreach (FileInfo flInfo in dir1.GetFiles())
                        {
                            name = flInfo.Name;
                            objInboundInquiry.Filename = name;
                            long size = flInfo.Length;
                        }
                    }
                    objInboundInquiry = ServiceObject.GetTempFiledtl(objInboundInquiry);
                    Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
                    InboundInquiryModel InboundInquiryDocModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
                    return PartialView("_DocumentUploadGrid", InboundInquiryDocModel);
                    //return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        //END
        public JsonResult AlocSummaryCount()
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq = ServiceObject.OutboundGETTEMPALOCSUMMARY(objOutboundInq);
            objOutboundInq.l_int_Aloc_Summary_Count = objOutboundInq.LstAlocSummary.Count();
            return Json(objOutboundInq.l_int_Aloc_Summary_Count, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShowAlocBackOrderReport(string p_str_cmpid, string p_str_Alocdocid, string p_str_Sonum)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            OutboundInq objInbound = new OutboundInq();
            OutboundInqService objService = new OutboundInqService();
            try
            {
                if (isValid)
                {
                    OutboundInq objOutboundInq = new OutboundInq();
                    OutboundInqService ServiceObject = new OutboundInqService();

                    strReportName = "rpt_mvc_so_bo.rpt";
                    ReportDocument rd = new ReportDocument();
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                    objOutboundInq.cmp_id = p_str_cmpid;
                    objOutboundInq.so_num = p_str_Sonum;
                    objOutboundInq = ServiceObject.Get_AlocBackOrderRptList(objOutboundInq);
                    var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                    rd.Load(strRptPath);
                    int AlocCount = 0;
                    AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
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
        public JsonResult GetBackOrderCount(string p_str_cmpid, string p_str_Sonum)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.cmp_id = p_str_cmpid;
            objOutboundInq.so_num = p_str_Sonum;
            objOutboundInq = ServiceObject.BackOrderRpt(objOutboundInq);
            objOutboundInq.l_int_Aloc_BackOrder_Count = objOutboundInq.LstAlocDtl.Count();
            return Json(objOutboundInq.l_int_Aloc_BackOrder_Count, JsonRequestBehavior.AllowGet);
        }
        //CR-180330-001 Added By Nithya
        public JsonResult GetItemCode(string l_str_cmp_id, string l_str_Itmdtl, string l_str_itm_color, string l_str_itm_size)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.cmp_id = l_str_cmp_id;
            objOutboundInq.itm_num = l_str_Itmdtl;
            objOutboundInq.itm_color = l_str_itm_color;
            objOutboundInq.itm_size = l_str_itm_size;
            objOutboundInq = ServiceObject.GetItemCode(objOutboundInq);
            if (objOutboundInq.LstAlocSummary.Count > 0)
            {
                objOutboundInq.ItmCode = objOutboundInq.LstAlocSummary[0].ItmCode;
                objOutboundInq.itm_name = objOutboundInq.LstAlocSummary[0].itm_name;
                return Json(new { data1 = "Y", data2 = objOutboundInq.ItmCode, data3 = objOutboundInq.itm_name }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data1 = "N", data2 = objOutboundInq.ItmCode, data3 = objOutboundInq.itm_name }, JsonRequestBehavior.AllowGet);
            //return Json(objOutboundInq.ItmCode, JsonRequestBehavior.AllowGet);
        }
        //END  
        private void updateSodtl(int m, string Cmp_id)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            string Itm_Code, Lot_Id, Rcvd_Dt, Loc_Id, Aloc_Qty = string.Empty;
            string Item_Code = string.Empty;
            string Item_num = string.Empty;
            string Item_Color = string.Empty;
            string Item_Size = string.Empty;
            string So_Num = string.Empty;
            int Alloc_Qty = 0;
            int BO_Qty = 0;
            int Dtl_Line = 0;
            int Due_Line = 0;
            string Bak_OdrQty = string.Empty;
            decimal Availqty = 0;
            int AlocQty, dueqty = 0;
            int LineNum = 0;
            string WhsID = string.Empty;
            objOutboundInq = ServiceObject.OutboundGETTEMPALOCSUMMARY(objOutboundInq);
            Item_Code = objOutboundInq.LstAlocSummary[m].itm_code;
            Item_num = objOutboundInq.LstAlocSummary[m].itm_num;
            Item_Color = objOutboundInq.LstAlocSummary[m].itm_color;
            Item_Size = objOutboundInq.LstAlocSummary[m].itm_size;
            So_Num = objOutboundInq.LstAlocSummary[m].so_num;
            Alloc_Qty = Convert.ToInt32(objOutboundInq.LstAlocSummary[m].aloc_qty);
            BO_Qty = objOutboundInq.LstAlocSummary[m].back_ordr_qty;
            Dtl_Line = objOutboundInq.LstAlocSummary[m].Soline;
            Due_Line = objOutboundInq.LstAlocSummary[m].DueLine;
            LineNum = objOutboundInq.LstAlocSummary[m].Line;
            WhsID = objOutboundInq.LstAlocSummary[m].whs_id;
            Availqty = Convert.ToInt32(objOutboundInq.LstAlocSummary[m].avail_qty);
            dueqty = objOutboundInq.LstAlocSummary[m].due_qty;
            if (BO_Qty > 0)
            {
                objOutboundInq.step = "B/O";
                objOutboundInq.status = "B";
            }
            else if (Alloc_Qty > 0)
            {
                objOutboundInq.step = "ALOC";
                objOutboundInq.status = "A";
            }
            else
            {
                objOutboundInq.step = "ENTRY";
                objOutboundInq.status = "O";
            }
            objOutboundInq.cmp_id = Cmp_id;
            objOutboundInq.Sonum = So_Num;
            objOutboundInq.itm_code = Item_Code;
            objOutboundInq.itm_num = Item_num;
            objOutboundInq.line_num = Dtl_Line;
            objOutboundInq.due_line = Due_Line;
            objOutboundInq.aloc_qty = Alloc_Qty;
            objOutboundInq.back_ordr_qty = BO_Qty;
            objOutboundInq = ServiceObject.Update_so_dtl(objOutboundInq);
            objOutboundInq.ReturnValue = objOutboundInq.ReturnValue;
        }
        private void Move_to_aloc_dtl(int J, string Cmp_id, string p_str_Alocdocid)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            string Item_code, Item_num, Item_Color, Item_Size = string.Empty;
            int Line_Num = 0;
            int I = 0;
            objOutboundInq = ServiceObject.OutboundGETTEMPALOCSUMMARY(objOutboundInq);
            objOutboundInq = ServiceObject.OutboundGETTEMPALOCDTL(objOutboundInq);

            Item_code = objOutboundInq.LstAlocSummary[J].itm_code;
            Item_num = objOutboundInq.LstAlocSummary[J].itm_num;
            Item_Color = objOutboundInq.LstAlocSummary[J].itm_color;
            Item_Size = objOutboundInq.LstAlocSummary[J].itm_size;
            Line_Num = objOutboundInq.LstAlocSummary[J].Line;
            objOutboundInq = ServiceObject.OutboundGETTEMPLIST(objOutboundInq);
            int AlocLineNo = objOutboundInq.LstItmxCustdtl[J].line_num;
            if (AlocLineNo != 0)
            {
                objOutboundInq.cmp_id = Cmp_id;
                objOutboundInq.AlocdocId = p_str_Alocdocid;
                objOutboundInq.line_num = AlocLineNo;
                objOutboundInq.itm_code = Item_code;
                objOutboundInq.so_num = objOutboundInq.LstItmxCustdtl[J].so_num;
                objOutboundInq.cust_id = objOutboundInq.LstItmxCustdtl[J].cust_id;
                objOutboundInq.so_itm_code = objOutboundInq.LstItmxCustdtl[J].itm_code;
                objOutboundInq.itm_num = objOutboundInq.LstItmxCustdtl[J].itm_num;
                objOutboundInq.itm_color = objOutboundInq.LstItmxCustdtl[J].itm_color;
                objOutboundInq.itm_size = objOutboundInq.LstItmxCustdtl[J].itm_size;
                objOutboundInq.dtl_line = objOutboundInq.LstItmxCustdtl[J].dtl_line;
                objOutboundInq.due_line = objOutboundInq.LstItmxCustdtl[J].due_line;
                objOutboundInq.ship_to_id = objOutboundInq.LstItmxCustdtl[J].shipto_id;
                objOutboundInq.aloc_qty = objOutboundInq.LstItmxCustdtl[J].aloc_qty;
                if (objOutboundInq.aloc_qty==0)
                {
                    objOutboundInq.aloc_qty = objOutboundInq.LstItmxCustdtl[J].due_qty;
                }
                else
                {
                    objOutboundInq.aloc_qty = objOutboundInq.LstItmxCustdtl[J].aloc_qty;
                }
                objOutboundInq.due_qty = objOutboundInq.LstItmxCustdtl[J].due_qty;
                objOutboundInq.qty_uom = objOutboundInq.LstItmxCustdtl[J].qty_uom;
                objOutboundInq.note = objOutboundInq.LstItmxCustdtl[J].note;
                objOutboundInq = ServiceObject.Move_to_aloc_dtl(objOutboundInq);
                objOutboundInq.ReturnValue = objOutboundInq.ReturnValue;
            }
        }
        private void Move_to_aloc_ctn(int K, string Cmp_id, string p_str_Alocdocid)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            string Itm_Code, Lot_Id, Rcvd_Dt, Loc_Id, Aloc_Qty = string.Empty;
            string Item_Code, Item_num, Item_Color, Item_Size = string.Empty;
            int Line_Num = 0;
            int Alo, CTN = 0;
            int PrevLine = 1;
            int NewCtnQty = 0;
            objOutboundInq = ServiceObject.OutboundGETTEMPALOCSUMMARY(objOutboundInq);
            objOutboundInq = ServiceObject.OutboundGETTEMPALOCDTL(objOutboundInq);
            int LnNum = objOutboundInq.LstAlocSummary[K].Line;
            Item_Code = objOutboundInq.LstAlocSummary[K].itm_code;
            Item_num = objOutboundInq.LstAlocSummary[K].itm_num;
            Item_Color = objOutboundInq.LstAlocSummary[K].itm_color;
            Item_Size = objOutboundInq.LstAlocSummary[K].itm_size;
            int J = 0;
            for (int i = 0; i < objOutboundInq.LstAlocDtl.Count(); i++)
            {
                if ((LnNum == objOutboundInq.LstAlocDtl[i].AlcLn) && (Item_Code == objOutboundInq.LstAlocDtl[i].itm_code) &&
         (Item_num == objOutboundInq.LstAlocDtl[i].itm_num) && (Item_Color == objOutboundInq.LstAlocDtl[i].itm_color) && (Item_Size == objOutboundInq.LstAlocDtl[i].itm_size))
                {
                    Itm_Code = objOutboundInq.LstAlocDtl[i].itm_code;
                    Loc_Id = objOutboundInq.LstAlocDtl[i].loc_id;
                    Rcvd_Dt = Convert.ToString(objOutboundInq.LstAlocDtl[i].rcvd_dt);
                    Lot_Id = objOutboundInq.LstAlocDtl[i].lot_id;
                    Aloc_Qty = Convert.ToString(objOutboundInq.LstAlocDtl[i].Aloc);
                    Alo = objOutboundInq.LstAlocDtl[i].Aloc;
                    CTN = objOutboundInq.LstAlocDtl[i].pkg_qty;
                    NewCtnQty = (Alo % CTN);
                    PrevLine = objOutboundInq.LstAlocDtl[i].AlcLn;
                    objOutboundInq.cmp_id = Cmp_id;
                    objOutboundInq.AlocdocId = p_str_Alocdocid;
                    objOutboundInq.line_num = objOutboundInq.LstAlocDtl[i].AlcLn;
                    if (PrevLine == objOutboundInq.LstAlocDtl[i].AlcLn)
                    {
                        objOutboundInq.ctn_line = J + 1;
                        J = J + 1;
                    }
                    else
                    {
                        PrevLine = objOutboundInq.LstAlocDtl[i].AlcLn;
                        J = 1;
                        objOutboundInq.ctn_line = 1;
                    }
                    objOutboundInq.itm_code = objOutboundInq.LstAlocDtl[i].itm_code;
                    objOutboundInq.itm_num = objOutboundInq.LstAlocDtl[i].itm_num;
                    objOutboundInq.itm_color = objOutboundInq.LstAlocDtl[i].itm_color;
                    objOutboundInq.itm_size = objOutboundInq.LstAlocDtl[i].itm_size;
                    objOutboundInq.so_num = objOutboundInq.LstAlocDtl[i].so_num;
                    objOutboundInq.Soline = objOutboundInq.LstAlocDtl[i].soline;
                    objOutboundInq.due_line = objOutboundInq.LstAlocDtl[i].due_line;
                    objOutboundInq.lot_id = objOutboundInq.LstAlocDtl[i].lot_id;
                    objOutboundInq.po_num = objOutboundInq.LstAlocDtl[i].po_num;
                    objOutboundInq.Recvddt = Convert.ToString(objOutboundInq.LstAlocDtl[i].rcvd_dt);
                    objOutboundInq.whs_id = objOutboundInq.LstAlocDtl[i].whs_id;
                    objOutboundInq.loc_id = objOutboundInq.LstAlocDtl[i].loc_id;
                    objOutboundInq.Palet_id = objOutboundInq.LstAlocDtl[i].Palet_id;
                    objOutboundInq.ctn_qty = CTN;
                    objOutboundInq.ctn_cnt = (Alo / CTN);
                    if (NewCtnQty == 0)
                    {
                        objOutboundInq.lineqty = Alo;
                    }
                    else
                    {
                        objOutboundInq.lineqty = Alo - NewCtnQty;
                    }
                    objOutboundInq.Bal = objOutboundInq.LstAlocDtl[i].Bal;
                    objOutboundInq = ServiceObject.Move_to_aloc_ctn(objOutboundInq);
                    if (NewCtnQty > 0)
                    {
                        objOutboundInq.ctn_cnt = NewCtnQty;
                        objOutboundInq = ServiceObject.Get_Newqty(objOutboundInq);
                        if (objOutboundInq.ListCheckExistStyle.Count > 0)
                        {
                            objOutboundInq.ctn_cnt = NewCtnQty;
                            objOutboundInq = ServiceObject.update_alocctn(objOutboundInq);
                        }
                        else
                        {
                            objOutboundInq.cmp_id = Cmp_id;
                            objOutboundInq.AlocdocId = p_str_Alocdocid;
                            objOutboundInq.line_num = objOutboundInq.LstAlocDtl[i].AlcLn;
                            objOutboundInq.ctn_line = J + 1;
                            J = J + 1;
                            objOutboundInq.itm_code = objOutboundInq.LstAlocDtl[i].itm_code;
                            objOutboundInq.itm_num = objOutboundInq.LstAlocDtl[i].itm_num;
                            objOutboundInq.itm_color = objOutboundInq.LstAlocDtl[i].itm_color;
                            objOutboundInq.itm_size = objOutboundInq.LstAlocDtl[i].itm_size;
                            objOutboundInq.so_num = objOutboundInq.LstAlocDtl[i].so_num;
                            objOutboundInq.Soline = objOutboundInq.LstAlocDtl[i].soline;
                            objOutboundInq.due_line = objOutboundInq.LstAlocDtl[i].due_line;
                            objOutboundInq.lot_id = objOutboundInq.LstAlocDtl[i].lot_id;
                            objOutboundInq.po_num = objOutboundInq.LstAlocDtl[i].po_num;
                            objOutboundInq.Recvddt = Convert.ToString(objOutboundInq.LstAlocDtl[i].rcvd_dt);
                            objOutboundInq.whs_id = objOutboundInq.LstAlocDtl[i].whs_id;
                            objOutboundInq.loc_id = objOutboundInq.LstAlocDtl[i].loc_id;
                            objOutboundInq.Palet_id = objOutboundInq.LstAlocDtl[i].Palet_id;
                            objOutboundInq.ctn_qty = NewCtnQty;
                            objOutboundInq.ctn_cnt = 1;
                            objOutboundInq.lineqty = NewCtnQty;
                            objOutboundInq.Bal = 0;
                            objOutboundInq = ServiceObject.Move_to_aloc_ctn(objOutboundInq);
                        }
                    }
                }
            }
        }
        private void Update_Tbl_iv_itm_trn_in(int K, string Cmp_id, string p_str_Alocdocid, string p_str_AlocOrdrno, string p_str_Alocshiptoname)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            string Itm_Code, Lot_Id, Rcvd_Dt, Loc_Id, Aloc_Qty = string.Empty;
            string Item_Code, Item_num, Item_Color, Item_Size = string.Empty;
            int Line_Num = 0;
            int Alo, CTN = 0;
            int PrevLine = 1;
            int NewCtnQty = 0;
            int Ppk = 0;
            int UpdtPkg = 0;
            string ItmCode, ItmNum, ItmColor, ItmSize, ItmName, tmp_po, l_str_doc_pkg_id = string.Empty;
            int AlocQty, dueqty = 0;
            int LineNum = 0;
            int N = 0;
            decimal Availqty = 0;
            string Whsid, LocId = string.Empty;
            int NewQty = 0;
            string palet_id = string.Empty;
            string OldPkgId = string.Empty;
            string ProcessId = string.Empty;
            objOutboundInq = ServiceObject.OutboundGETTEMPALOCSUMMARY(objOutboundInq);
            objOutboundInq = ServiceObject.OutboundGETTEMPALOCDTL(objOutboundInq);
            int LnNum = objOutboundInq.LstAlocSummary[K].Line;
            Item_Code = objOutboundInq.LstAlocSummary[K].itm_code;
            Item_num = objOutboundInq.LstAlocSummary[K].itm_num;
            Item_Color = objOutboundInq.LstAlocSummary[K].itm_color;
            Item_Size = objOutboundInq.LstAlocSummary[K].itm_size;
            int J = 0;
            for (int i = 0; i < objOutboundInq.LstAlocDtl.Count(); i++)
            {
                if ((LnNum == objOutboundInq.LstAlocDtl[i].AlcLn) && (Item_Code == objOutboundInq.LstAlocDtl[i].itm_code) &&
         (Item_num == objOutboundInq.LstAlocDtl[i].itm_num) && (Item_Color == objOutboundInq.LstAlocDtl[i].itm_color) && (Item_Size == objOutboundInq.LstAlocDtl[i].itm_size))
                {
                    Itm_Code = objOutboundInq.LstAlocDtl[i].itm_code;
                    Loc_Id = objOutboundInq.LstAlocDtl[i].loc_id;
                    Rcvd_Dt = Convert.ToString(objOutboundInq.LstAlocDtl[i].rcvd_dt);
                    Lot_Id = objOutboundInq.LstAlocDtl[i].lot_id;
                    Aloc_Qty = Convert.ToString(objOutboundInq.LstAlocDtl[i].Aloc);
                    Alo = objOutboundInq.LstAlocDtl[i].Aloc;
                    CTN = objOutboundInq.LstAlocDtl[i].pkg_qty;
                    NewCtnQty = (Alo % CTN);
                    PrevLine = objOutboundInq.LstAlocDtl[i].AlcLn;
                    objOutboundInq.cmp_id = Cmp_id;
                    objOutboundInq.AlocdocId = p_str_Alocdocid;
                    objOutboundInq.line_num = objOutboundInq.LstAlocDtl[i].AlcLn;
                    Ppk = objOutboundInq.LstAlocDtl[i].pkg_qty;
                    AlocQty = objOutboundInq.LstAlocDtl[i].Aloc;
                    Whsid = objOutboundInq.LstAlocDtl[i].whs_id;
                    LocId = objOutboundInq.LstAlocDtl[i].loc_id;
                    palet_id = objOutboundInq.LstAlocDtl[i].Palet_id;
                    ItmCode = objOutboundInq.LstAlocDtl[i].itm_code;
                    ItmNum = objOutboundInq.LstAlocDtl[i].itm_num;
                    ItmColor = objOutboundInq.LstAlocDtl[i].itm_color;
                    ItmSize = objOutboundInq.LstAlocDtl[i].itm_size;
                    tmp_po = objOutboundInq.LstAlocDtl[i].po_num;
                    ctn_line = ctn_line + 1;
                    int AlocLineNo = objOutboundInq.LstAlocDtl[i].AlcLn;
                    NewQty = (AlocQty % Ppk);
                    BalQty = Ppk - NewQty;
                    UpdtPkg = (AlocQty / Ppk);
                    if (NewQty != 0)//CR18-03-26-001 Added by Nithya
                                    //if (NewQty > 0)
                    {
                        objOutboundInq.cmp_id = Cmp_id;
                        objOutboundInq.itm_code = ItmCode;
                        objOutboundInq.whs_id = Whsid;
                        objOutboundInq.loc_id = LocId;
                        objOutboundInq.Palet_id = palet_id;
                        objOutboundInq.pkg_qty = Ppk;
                        objOutboundInq.po_num = tmp_po;
                        objOutboundInq = ServiceObject.Get_itmtrninList(objOutboundInq);
                        //for (K = 0; J < objOutboundInq.ListCheckExistStyle.Count(); J++)
                        if (objOutboundInq.ListCheckExistStyle.Count() > 0)
                        {
                            objOutboundInq = ServiceObject.Get_PkgID(objOutboundInq);
                            objOutboundInq.doc_pkg_id = objOutboundInq.doc_pkg_id;
                            l_str_doc_pkg_id = objOutboundInq.doc_pkg_id;
                            OldPkgId = objOutboundInq.ListCheckExistStyle[0].pkg_id;
                            objOutboundInq.pkg_id = l_str_doc_pkg_id;
                            objOutboundInq.cmp_id = Cmp_id;
                            objOutboundInq.itm_code = objOutboundInq.ListCheckExistStyle[0].itm_code;//CR18-04-23-001 Added by Nithya
                            objOutboundInq.itm_num = objOutboundInq.ListCheckExistStyle[0].itm_num;
                            objOutboundInq.itm_color = objOutboundInq.ListCheckExistStyle[0].itm_color;
                            objOutboundInq.itm_size = objOutboundInq.ListCheckExistStyle[0].itm_size;
                            objOutboundInq.kit_type = objOutboundInq.ListCheckExistStyle[0].kit_type;
                            objOutboundInq.kit_id = objOutboundInq.ListCheckExistStyle[0].kit_id;
                            objOutboundInq.kit_qty = objOutboundInq.ListCheckExistStyle[0].kit_qty;
                            objOutboundInq.lot_id = objOutboundInq.ListCheckExistStyle[0].lot_id;
                            objOutboundInq.palet_id = objOutboundInq.ListCheckExistStyle[0].palet_id;
                            objOutboundInq.cont_id = objOutboundInq.ListCheckExistStyle[0].cont_id;
                            objOutboundInq.rcvd_dt = objOutboundInq.ListCheckExistStyle[0].rcvd_dt;
                            objOutboundInq.bill_status = objOutboundInq.ListCheckExistStyle[0].bill_status;
                            objOutboundInq.io_rate_id = objOutboundInq.ListCheckExistStyle[0].io_rate_id;
                            objOutboundInq.st_rate_id = objOutboundInq.ListCheckExistStyle[0].st_rate_id;
                            objOutboundInq.doc_id = objOutboundInq.ListCheckExistStyle[0].doc_id;
                            objOutboundInq.doc_date = objOutboundInq.ListCheckExistStyle[0].doc_date;
                            objOutboundInq.doc_notes = objOutboundInq.ListCheckExistStyle[0].doc_notes;
                            objOutboundInq.fmto_name = objOutboundInq.ListCheckExistStyle[0].fmto_name;
                            objOutboundInq.group_id = objOutboundInq.ListCheckExistStyle[0].group_id;
                            objOutboundInq.whs_id = objOutboundInq.ListCheckExistStyle[0].whs_id;
                            objOutboundInq.loc_id = objOutboundInq.ListCheckExistStyle[0].loc_id;
                            objOutboundInq.pkg_type = objOutboundInq.ListCheckExistStyle[0].pkg_type;
                            objOutboundInq.pkg_qty = BalQty;
                            objOutboundInq.itm_qty = BalQty;
                            objOutboundInq.lbl_id = objOutboundInq.ListCheckExistStyle[0].lbl_id;
                            objOutboundInq.grn_id = objOutboundInq.ListCheckExistStyle[0].grn_id;
                            objOutboundInq.po_num = objOutboundInq.ListCheckExistStyle[0].po_num;
                            objOutboundInq.process_id = objOutboundInq.ListCheckExistStyle[0].process_id;
                            ProcessId = objOutboundInq.process_id;
                            objOutboundInq.length = objOutboundInq.ListCheckExistStyle[0].length;
                            objOutboundInq.width = objOutboundInq.ListCheckExistStyle[0].width;
                            objOutboundInq.depth = objOutboundInq.ListCheckExistStyle[0].depth;
                            objOutboundInq.cube = objOutboundInq.ListCheckExistStyle[0].cube;
                            objOutboundInq.wgt = objOutboundInq.ListCheckExistStyle[0].wgt;
                            objOutboundInq.ib_doc_id = objOutboundInq.ListCheckExistStyle[0].ib_doc_id;
                            ServiceObject.Update_Tbl_iv_itm_trn_in(objOutboundInq);

                        }
                        //CR-20180327-001 Added By Nithya
                        objOutboundInq.cmp_id = Cmp_id;
                        objOutboundInq.pkg_id = OldPkgId;
                        objOutboundInq.itm_code = ItmCode;
                        objOutboundInq.itm_num = ItmNum;
                        objOutboundInq.itm_color = ItmColor;
                        objOutboundInq.itm_size = ItmSize;
                        objOutboundInq.whs_id = Whsid;
                        objOutboundInq.loc_id = LocId;
                        objOutboundInq.Palet_id = palet_id;
                        objOutboundInq.doc_id = p_str_Alocdocid;
                        objOutboundInq.po_num = tmp_po;
                        objOutboundInq.pkg_qty = Ppk;
                        objOutboundInq.new_pkg_qty = Convert.ToString(NewQty);
                        objOutboundInq.doc_date = DateTime.Now.ToString("MM/dd/yyyy");
                        ServiceObject.Aloc_SpltCtn_Upd_Itm_Trn_in_direc(objOutboundInq);
                    }
                    if (UpdtPkg > 0)
                    {
                        objOutboundInq.cmp_id = Cmp_id;
                        objOutboundInq.itm_code = ItmCode;
                        objOutboundInq.itm_num = ItmNum;
                        objOutboundInq.itm_color = ItmColor;
                        objOutboundInq.itm_size = ItmSize;
                        objOutboundInq.whs_id = Whsid;
                        objOutboundInq.loc_id = LocId;
                        objOutboundInq.Palet_id = palet_id;
                        objOutboundInq.pkg_qty = Ppk;
                        objOutboundInq.doc_id = p_str_Alocdocid;
                        objOutboundInq.doc_date = DateTime.Now.ToString("MM/dd/yyyy");
                        objOutboundInq.po_num = tmp_po;
                        objOutboundInq.process_id = ProcessId;
                        objOutboundInq.RowCount = Convert.ToString(UpdtPkg);
                        ServiceObject.Aloc_Upd_data_to_itm_trn_in_direc(objOutboundInq);
                    }
                }
            }
            objOutboundInq.cmp_id = Cmp_id;
            objOutboundInq.doc_id = p_str_Alocdocid;
            objOutboundInq.itm_code = Item_Code;
            objOutboundInq.itm_num = Item_num;
            objOutboundInq.itm_color = Item_Color;
            objOutboundInq.itm_size = Item_Size;
            objOutboundInq.shipname = p_str_Alocshiptoname;
            objOutboundInq.orderNo = p_str_AlocOrdrno;
            objOutboundInq.doc_date = DateTime.Now.ToString("MM/dd/yyyy");
            ServiceObject.Aloc_into_Itm_Trn_hst_by_itm_del_direc(objOutboundInq);
            ServiceObject.Aloc_into_Itm_Trn_hst_by_itm_direc(objOutboundInq);
        }
        public JsonResult GetAlocatedQty()
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            List<OutboundInq> GETInvoiceList = new List<OutboundInq>();
            GETInvoiceList = Session["GridOpenInvoiceList"] as List<OutboundInq>;
            var p_str_get_Aloc = GETInvoiceList.GroupBy(x => x.itm_num).Select(x => new { Count = x.Sum(y => y.Aloc) });
            var p_str_get_Alocqty = p_str_get_Aloc.ToList();
            var SelAlocate = Convert.ToDecimal(p_str_get_Alocqty[0].Count);
            Session["SelAlocated"] = SelAlocate;
            return Json(SelAlocate, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ShowManualAlocEdit(int p_str_RowNo)
        {

            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            List<OutboundInq> GETInvoiceLi = new List<OutboundInq>();
            GETInvoiceLi = Session["GridInvoiceList"] as List<OutboundInq>;
            if (GETInvoiceLi == null)
            {
                GETInvoiceLi = Session["GridInvoice"] as List<OutboundInq>;
                objOutboundInq.LstManualAloc = GETInvoiceLi;
                var result1 = from r in GETInvoiceLi where r.LineNum == p_str_RowNo select new { r.loc_id, r.avail_qty, r.pkg_qty, r.Aloc, r.back_ordr_qty, r.LineNum, r.reqQty, r.alocated };
                var get_result1 = result1.ToList();
                p_str_RowNo = get_result1[0].LineNum;
                for (int j = 0; j < GETInvoiceLi.Count(); j++)
                {

                    GETInvoiceLi.Where(p => p.LineNum == p_str_RowNo).Select(u =>
                    {
                        u.colChk = "false";
                        return u;
                    }).ToList();
                }
            }
            else
            {
                GETInvoiceLi = Session["GridInvoice"] as List<OutboundInq>;
                objOutboundInq.LstManualAloc = GETInvoiceLi;
                for (int j = 0; j < GETInvoiceLi.Count(); j++)
                {

                    GETInvoiceLi.Where(p => p.LineNum == p_str_RowNo).Select(u =>
                    {
                        u.colChk = "false";
                        return u;
                    }).ToList();
                }
            }
            objOutboundInq.LstManualAloc = GETInvoiceLi;
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundShipReqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_AlocManualAloc", objOutboundShipReqModel);
        }
        //CR_MVC_3PL_20180411-001 Added By NIthya  
        private void GetAvailQty(ref int AvailQty, string ItemCode, string cmp_id)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.cmp_id = cmp_id;
            objOutboundInq.itm_code = ItemCode;
            objOutboundInq = ServiceObject.LoadAvailQty(objOutboundInq);
            if (objOutboundInq.LstItmxCustdtl.Count == 0)
            {
                objOutboundInq.avlqty = 0;
            }
            else
            {
                objOutboundInq.avlqty = objOutboundInq.LstItmxCustdtl[0].avail_qty;
                AvailQty = Convert.ToInt32(objOutboundInq.avlqty);
            }
        }
        //END
        //CR20180504-001 Added By Nithya
        public ActionResult ShowEmailAlocReport(string p_str_cmpid, string p_str_Alocdocid, string p_str_Sonum)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            OutboundInq objInbound = new OutboundInq();
            OutboundInqService objService = new OutboundInqService();
            string strDateFormat = string.Empty;
            string strFileName = string.Empty;
            string reportFileName = string.Empty;//CR2018-03-07-001 Added By Nithya
            Folderpath = System.Configuration.ConfigurationManager.AppSettings["DefaultFolderPath"].ToString().Trim();
            try
            {
                if (isValid)
                {
                    OutboundInq objOutboundInq = new OutboundInq();
                    OutboundInqService ServiceObject = new OutboundInqService();
                    objOutboundInq.cmp_id = p_str_cmpid;
                    objOutboundInq.so_num = p_str_Sonum;
                    objOutboundInq = ServiceObject.BackOrderRpt(objOutboundInq);
                    if (objOutboundInq.LstAlocDtl.Count > 0)
                    {
                        strReportName = "rpt_mvc_so_bo.rpt";
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmpid;
                        objOutboundInq.so_num = p_str_Sonum;
                        objOutboundInq = ServiceObject.Get_AlocBackOrderRptList(objOutboundInq);
                        var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/")  +Folderpath + "//ALOC_ENTRY_" + strDateFormat + ".pdf";
                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                        reportFileName = "Outbound ALOC_ENTRY_" + strDateFormat + ".pdf";
                        Session["RptFileName"] = strFileName;
                        //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                    }
                    else
                    {
                        strReportName = "rpt_iv_pickslip_direct.rpt";
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmpid;
                        objOutboundInq.AlocdocId = p_str_Alocdocid;
                        objOutboundInq = ServiceObject.Get_AlocSaveRpt(objOutboundInq);
                        var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//ALOC_ENTRY_" + strDateFormat + ".pdf";
                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                        reportFileName = "Outbound ALOC_ENTRY_" + strDateFormat + ".pdf";
                        Session["RptFileName"] = strFileName;


                        //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                    }
                }
                else
                {
                    Response.Write("<H2>Report not found</H2>");
                }
                //CR20180504-001 Added By Nithya
                Email objEmail = new Email();
                objEmail.CmpId = p_str_cmpid;
                objEmail.EmailSubject = "ALOC_ENTRY_";
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
        //END
        //CR-20180518-001 Added by Nithya
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
            try
            {
                if (isValid)
                {
                    if (p_str_status == "OPEN")
                    {
                        strReportName = "rpt_iv_packslip_tpw.rpt";
                        OutboundShipInq objOutboundShipInq = new OutboundShipInq();
                        OutboundShipInqService ServiceObject = new OutboundShipInqService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundShipInq.cmp_id = p_str_cmpid;
                        objOutboundShipInq.ship_doc_id = p_str_Shipping_id;
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
                        var rptSource = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        objOutboundShipInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        rd.SetParameterValue("fml_image_path", objOutboundShipInq.Image_Path);
                         rd.SetParameterValue("SumOfCubes", objOutboundShipInq.TotCube);
                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                    }
                    if (l_str_status == "POST")
                    {
                        strReportName = "rpt_iv_bill_of_lading.rpt";
                        OutboundShipInq objOutboundShipInq = new OutboundShipInq();
                        OutboundShipInqService ServiceObject = new OutboundShipInqService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundShipInq.cmp_id = p_str_cmpid;
                        objOutboundShipInq.ship_doc_id = p_str_Shipping_id;

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
                        var rptSource = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        objOutboundShipInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        rd.SetParameterValue("fml_image_path", objOutboundShipInq.Image_Path);
                        rd.SetParameterValue("TotCube", objOutboundShipInq.TotCube);
                        rd.SetParameterValue("TotCarton", objOutboundShipInq.TotCtns);
                        rd.SetParameterValue("TotWgt", objOutboundShipInq.TotWgt);
                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
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
        //END
        //CR-20180522-001 Added By Nithya
        public ActionResult StockVerify(string p_str_cmp_id, string p_str_screen_title, string p_str_Sonum,string p_str_batchId, string P_str_SoNumFm, string P_str_SoNumTo , string P_str_screen_MOde)
        {
             int LineNum = 0;
            int Back_Order_Qty = 0;
            int intAvailQty = 0;
            string StrwhsId = string.Empty;
            string StrItmCode = string.Empty;
            string StrItmNum = string.Empty;
            string StrItmColor = string.Empty;
            string StrItmSize = string.Empty;
            int intOrderQty = 0;
            int intTotBOLines = 0;
            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objOutboundInq.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objOutboundInq.cmp_id = p_str_cmp_id.Trim();
            objOutboundInq.CompID = p_str_cmp_id.Trim();
            //objOutboundInq.Sonum = p_str_Sonum;
            objOutboundInq.quote_num = p_str_batchId.Trim();
            Session["batchId"] = p_str_batchId;
            objOutboundInq.so_numFm = P_str_SoNumFm.Trim();
            objOutboundInq.so_numTo = P_str_SoNumTo.Trim();
            objOutboundInq.screentitle = p_str_screen_title.Trim();
            ServiceObject.Delete_StockVerify(objOutboundInq);
            objOutboundInq = ServiceObject.CheckOpenOrderExist(objOutboundInq);
            if(objOutboundInq.LstStockverify.Count>0)
            {
                objOutboundInq.TotRecs = objOutboundInq.LstStockverify[0].TotRecs;
            }
            else
            {
                objOutboundInq.TotRecs = 0;
            }        
            if (objOutboundInq.TotRecs == 0)
            {
                int ResultCount = 1;
                return Json(ResultCount, JsonRequestBehavior.AllowGet);
            }
            dtOutInq = new DataTable();
            List<OutboundInq> li = new List<OutboundInq>();
            //2: Initialize a object of type DataRow
            DataRow drOBAloc;
            //3: Initialize enough objects of type DataColumns
            DataColumn colLine = new DataColumn("LineNum", typeof(string));
            DataColumn colCmpId = new DataColumn("cmp_id", typeof(string));
            DataColumn colwhs_id = new DataColumn("whs_id", typeof(string));
            DataColumn colitm_code = new DataColumn("itm_code", typeof(string));
            DataColumn colStyle = new DataColumn("Style", typeof(string));
            DataColumn colColor = new DataColumn("Color", typeof(string));
            DataColumn colSize = new DataColumn("Size", typeof(string));
            DataColumn colitmName = new DataColumn("itm_name", typeof(string));
            DataColumn colOrderQty = new DataColumn("OrderQty", typeof(decimal));
            DataColumn colAloc_Qty = new DataColumn("Aloc_Qty", typeof(decimal));
            DataColumn colBack_Order_Qty = new DataColumn("Back_Order_Qty", typeof(decimal));
            DataColumn colBalanceQty = new DataColumn("balance_qty", typeof(decimal));
            DataColumn colbackorderCount = new DataColumn("backorderCount", typeof(int));
            int lintCount = 0;
            //4: Adding DataColumns to DataTable dt           
            dtOutInq.Columns.Add(colLine);
            dtOutInq.Columns.Add(colCmpId);
            dtOutInq.Columns.Add(colwhs_id);
            dtOutInq.Columns.Add(colitm_code);
            dtOutInq.Columns.Add(colStyle);
            dtOutInq.Columns.Add(colColor);
            dtOutInq.Columns.Add(colSize);
            dtOutInq.Columns.Add(colitmName);
            dtOutInq.Columns.Add(colOrderQty);
            dtOutInq.Columns.Add(colAloc_Qty);
            dtOutInq.Columns.Add(colBack_Order_Qty);
            dtOutInq.Columns.Add(colBalanceQty);
            dtOutInq.Columns.Add(colbackorderCount);
            objOutboundInq = ServiceObject.ShowStockVerifyRpt(objOutboundInq);
            for (int i = 0; i < objOutboundInq.LstStkverifyList.Count(); i++)
            {
                objOutboundInq.cmp_id = objOutboundInq.LstStkverifyList[i].cmp_id;
                objOutboundInq.whs_id = objOutboundInq.LstStkverifyList[i].whs_id;
                StrwhsId = objOutboundInq.whs_id;
                objOutboundInq.itm_code = objOutboundInq.LstStkverifyList[i].itm_code;
                StrItmCode = objOutboundInq.itm_code;
                objOutboundInq.Style = objOutboundInq.LstStkverifyList[i].Style;
                StrItmNum = objOutboundInq.Style;
                objOutboundInq.Color = objOutboundInq.LstStkverifyList[i].Color;
                StrItmColor = objOutboundInq.Color;
                objOutboundInq.Size = objOutboundInq.LstStkverifyList[i].Size;
                StrItmSize = objOutboundInq.Size;
                objOutboundInq.OrderQty = objOutboundInq.LstStkverifyList[i].OrderQty;
                //objOutboundInq.Sonum = objOutboundInq.LstStkverifyList[i].so_num;
                objOutboundInq.Back_Order_Qty = objOutboundInq.LstStkverifyList[i].Back_Order_Qty;
                objOutboundInq.Aloc_Qty = objOutboundInq.LstStkverifyList[i].Aloc_Qty;
                intOrderQty = objOutboundInq.Aloc_Qty;
                Back_Order_Qty = objOutboundInq.Back_Order_Qty;
                LineNum = LineNum + 1;
                drOBAloc = dtOutInq.NewRow();
                dtOutInq.Rows.Add(drOBAloc);
                dtOutInq.Rows[lintCount][colLine] = LineNum;
                dtOutInq.Rows[lintCount][colCmpId] = objOutboundInq.cmp_id.ToString();
                dtOutInq.Rows[lintCount][colwhs_id] = objOutboundInq.whs_id.ToString();
                dtOutInq.Rows[lintCount][colitm_code] = objOutboundInq.itm_code.ToString();
                dtOutInq.Rows[lintCount][colStyle] = objOutboundInq.Style.ToString();
                dtOutInq.Rows[lintCount][colColor] = objOutboundInq.Color.ToString();
                dtOutInq.Rows[lintCount][colSize] = objOutboundInq.Size.ToString();
                dtOutInq.Rows[lintCount][colOrderQty] = objOutboundInq.OrderQty.ToString();
                OutboundInq objOutboundInqDtltemp = new OutboundInq();
                objOutboundInqDtltemp.cmp_id = objOutboundInq.cmp_id;
                objOutboundInqDtltemp.LineNum = LineNum;
                objOutboundInqDtltemp.itm_code = objOutboundInq.itm_code;
                objOutboundInqDtltemp.Style = objOutboundInq.Style;
                objOutboundInqDtltemp.Color = objOutboundInq.Color;
                objOutboundInqDtltemp.Size = objOutboundInq.Size;
                objOutboundInqDtltemp.whs_id = objOutboundInq.whs_id;
                if (Back_Order_Qty > 0)
                {
                    objOutboundInqDtltemp.OrderQty = objOutboundInq.Back_Order_Qty;
                    objOutboundInq.OrderQty = objOutboundInqDtltemp.OrderQty;
                }
                else
                {
                    objOutboundInqDtltemp.OrderQty = objOutboundInq.OrderQty;
                    objOutboundInq.OrderQty = objOutboundInqDtltemp.OrderQty;
                }
                objOutboundInq.cmp_id = p_str_cmp_id;
                objOutboundInq.itm_code = StrItmCode;
                objOutboundInq.itm_num = StrItmNum;
                objOutboundInq.itm_color = StrItmColor;
                objOutboundInq.itm_size = StrItmSize;
                objOutboundInq.whs_id = StrwhsId;
                objOutboundInq = ServiceObject.GetItemName(objOutboundInq);
                if (objOutboundInq.LstOutboundPickQty.Count > 0)
                {
                    objOutboundInqDtltemp.itm_name = objOutboundInq.LstOutboundPickQty[0].itm_name;
                    objOutboundInq.itm_name = objOutboundInqDtltemp.itm_name;
                }
                else
                {
                    objOutboundInqDtltemp.itm_name = "-";
                    objOutboundInq.itm_name = "-";
                }
                objOutboundInq = ServiceObject.OutboundGETAVILQTY(objOutboundInq);
                objOutboundInq.avail_qty = objOutboundInq.LstAvailqty[0].pkg_avail_cnt;
                objOutboundInqDtltemp.avail_qty = objOutboundInq.avail_qty;
                intAvailQty = Convert.ToInt32(objOutboundInqDtltemp.avail_qty);
                intOrderQty = objOutboundInqDtltemp.OrderQty;
                if ((intAvailQty - intOrderQty) >= 0)
                {
                    objOutboundInqDtltemp.back_ordr_qty = 0;
                    objOutboundInq.Back_Order_Qty = 0;
                    objOutboundInqDtltemp.balance_qty = intAvailQty - intOrderQty;
                    objOutboundInq.balance_qty = objOutboundInqDtltemp.balance_qty;
                }
                else
                {
                    objOutboundInqDtltemp.back_ordr_qty = intOrderQty - intAvailQty;
                    objOutboundInq.Back_Order_Qty = objOutboundInqDtltemp.back_ordr_qty;
                    objOutboundInqDtltemp.balance_qty = 0;
                    objOutboundInq.balance_qty = 0;
                    intTotBOLines = intTotBOLines + 1;
                    objOutboundInqDtltemp.backorderCount = intTotBOLines;

                }
                if (P_str_SoNumFm == "" || P_str_SoNumFm ==null)
                {
                    if(p_str_batchId=="" || p_str_batchId == null)
                    {
                        objOutboundInq = ServiceObject.GETSONUMLIST(objOutboundInq);
                        objOutboundInq.Sonum = objOutboundInq.lstShipAlocdtl[0].so_num;
                    }
                    else
                    {
                        objOutboundInq.Sonum = p_str_batchId;
                    }
                }
                else
                {
                    objOutboundInq.Sonum = P_str_SoNumFm;
                    objOutboundInq.so_num = P_str_SoNumFm;
                }                
                ServiceObject.Insert_StockVerify(objOutboundInq);
                li.Add(objOutboundInqDtltemp);
                lintCount++;
            }
            objOutboundInq.backorderCount = intTotBOLines;
            objOutboundInq.LstStockverify = li;
            Session["GridStockverifyList"] = objOutboundInq.LstStockverify;
            objOutboundInq.ScreenMode = P_str_screen_MOde; 
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundShipModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_Outbound_StockVerify", objOutboundShipModel);
        }
        public ActionResult ShowStockVerifyExcel(string p_str_cmpid, string p_str_ShipReq_id)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string cmp_id = p_str_cmpid.Trim();
            string ib_doc_id = p_str_ShipReq_id.Trim();
            OutboundInq objInbound = new OutboundInq();
            OutboundInqService objService = new OutboundInqService();
            try
            {
                if (isValid)
                {
                    OutboundInq objOutboundInq = new OutboundInq();
                    OutboundInqService ServiceObject = new OutboundInqService();
                    List<OutboundInq> StockverifyLi = new List<OutboundInq>();
                    StockverifyLi = Session["GridStockverifyList"] as List<OutboundInq>;
                    objOutboundInq.LstStockverify = StockverifyLi;
                    List <OB_StockverifyExcel> li = new List<OB_StockverifyExcel>();
                    for (int i = 0; i < objOutboundInq.LstStockverify.Count; i++)
                    {

                        OB_StockverifyExcel objOBInquiryExcel = new OB_StockverifyExcel();
                        objOBInquiryExcel.whs_id = objOutboundInq.LstStockverify[i].whs_id;
                        objOBInquiryExcel.Style = objOutboundInq.LstStockverify[i].Style;
                        objOBInquiryExcel.Color = objOutboundInq.LstStockverify[i].Color;
                        objOBInquiryExcel.Size = objOutboundInq.LstStockverify[i].Size;
                        objOBInquiryExcel.ItemName = objOutboundInq.LstStockverify[i].itm_name;
                        objOBInquiryExcel.DueQty = objOutboundInq.LstStockverify[i].OrderQty;
                        objOBInquiryExcel.AvailQty = Convert.ToInt32(objOutboundInq.LstStockverify[i].avail_qty);
                        objOBInquiryExcel.BackOrder_Qty = objOutboundInq.LstStockverify[i].back_ordr_qty;
                        objOBInquiryExcel.balanceQty = objOutboundInq.LstStockverify[i].balance_qty;                       
                        li.Add(objOBInquiryExcel);
                    }
                    GridView gv = new GridView();
                    gv.DataSource = li;
                    gv.DataBind();
                    Session["OB_STKVERIFY"] = gv;
                    return new DownloadFileActionResult((GridView)Session["OB_STKVERIFY"], "OB_STKVERIFY" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
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
        public ActionResult ConsolidatedRptByStyleAndLoc(string p_str_cmp_id, string p_str_screen_title, string p_str_Sonum, string P_str_SoNumFm, string P_str_SoNumTo)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objOutboundInq.cmp_id = p_str_cmp_id;
            objOutboundInq.CompID = p_str_cmp_id;
            objOutboundInq.Sonum = p_str_Sonum.Trim();
            objOutboundInq.quote_num = p_str_Sonum.Trim();
            objOutboundInq.so_numFm = P_str_SoNumFm.Trim();
            objOutboundInq.so_numTo = P_str_SoNumTo.Trim();
            objOutboundInq.screentitle = p_str_screen_title;
            objOutboundInq = ServiceObject.IsOpenAlocationExists(objOutboundInq);
            if (objOutboundInq.LstStockverify.Count > 0)
            {
                objOutboundInq.TotRecs = objOutboundInq.LstStockverify[0].TotRecs;
            }
            else
            {
                objOutboundInq.TotRecs = 0;
            }          
            if (objOutboundInq.TotRecs == 0)
            {
                int ResultCount = 1;
                return Json(ResultCount, JsonRequestBehavior.AllowGet);
            }
            else
            {
                int ResultCount = 2;
                return Json(ResultCount, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ShowConsolidatedReport(string p_str_cmp_id, string p_str_Sonum,string P_str_SoNumFm,string P_str_SoNumTo,string Type)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string cmp_id = p_str_cmp_id;
            string ib_doc_id = p_str_Sonum;
            string batchId = string.Empty;
            string AlocDocId = string.Empty;
            string So_num = string.Empty;
            OutboundInq objInbound = new OutboundInq();
            OutboundInqService objService = new OutboundInqService();
            try
            {
                if (isValid)
                {
                    if(Type== "Style")
                    {
                        string RptResult = string.Empty;
                        strReportName = "rpt_iv_pickslip_consolidated_by_style.rpt";
                        OutboundInq objOutboundInq = new OutboundInq();
                        OutboundInqService ServiceObject = new OutboundInqService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmp_id.Trim();
                        objOutboundInq.so_numFm = P_str_SoNumFm.Trim();
                        objOutboundInq.so_numTo = P_str_SoNumTo.Trim();
                        objOutboundInq.quote_num = p_str_Sonum.Trim();
                        batchId = objOutboundInq.quote_num;
                        objOutboundInq = ServiceObject.OutboundInqPickStyleRpt(objOutboundInq);
                            var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                            batchId = "Batch Id :" + batchId + "";
                            string batchIdFm = "Ship Request From :" + P_str_SoNumFm + "  Ship Request To : " + P_str_SoNumTo;
                            if (batchId != string.Empty)
                            {
                                if (!string.IsNullOrEmpty(batchId))
                                    rd.SetParameterValue("fml_rep_selectionBy", batchId);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(batchIdFm))
                                    rd.SetParameterValue("fml_rep_selectionBy", batchIdFm);
                            }

                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                    }
                    else
                    {
                        strReportName = "rpt_iv_pickslip_consolidated_by_loc.rpt";
                        OutboundInq objOutboundInq = new OutboundInq();
                        OutboundInqService ServiceObject = new OutboundInqService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmp_id.Trim();
                        objOutboundInq.so_numFm = P_str_SoNumFm.Trim();
                        objOutboundInq.so_numTo = P_str_SoNumTo.Trim();
                        objOutboundInq.quote_num = p_str_Sonum.Trim();
                        batchId = objOutboundInq.quote_num;
                        objOutboundInq = ServiceObject.OutboundInqPickLocationRpt(objOutboundInq);
                        //for(int i=0; i< objOutboundInq.LstOutboundInqpickstyleRpt.Count();i++)
                        //{
                        //    AlocDocId = objOutboundInq.LstOutboundInqpickstyleRpt[i].aloc_doc_id;
                        //    lstrAlocList = lstrAlocList + AlocDocId + "','";
                        //    So_num = objOutboundInq.LstOutboundInqpickstyleRpt[i].so_num;
                        //    lstrSonumList = lstrSonumList + So_num + "','";
                        //}                                                                    
                        //lstrAlocList = lstrAlocList.Remove(lstrAlocList.Length - 2, 2);
                        //lstrAlocList = lstrAlocList + "]";
                        //lstrSonumList = lstrSonumList.Remove(lstrSonumList.Length - 2, 2);
                        //lstrSonumList = lstrSonumList + "]";
                        var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                            batchId = "Batch Id :" + batchId + "";
                            string batchIdFm = "Ship Request From :" + P_str_SoNumFm + "  Ship Request To : " + p_str_Sonum;
                            if (batchId != string.Empty)
                            {
                                if (!string.IsNullOrEmpty(batchId))
                                    rd.SetParameterValue("fml_rep_selectionBy", batchId);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(batchId))
                                    rd.SetParameterValue("fml_rep_selectionBy", batchIdFm);
                            }
                            //rd.SetParameterValue("AlocDocId", lstrAlocList);
                            //rd.SetParameterValue("SoNum", lstrSonumList);
                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
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
        public ActionResult SaveStockBackOrderAuotoAdd(string p_str_cmp_id, string p_str_Sonum)
        {
            string ETADate = string.Empty;
            int tmpDtlLine = 0;
            int tmpItmLine = 0;
            string tmpLotId = string.Empty;
            string tmpDate = string.Empty;
            string l_str_Refno = string.Empty;
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.user_id = Session["UserID"].ToString().Trim();          
            objOutboundInq.cmp_id = p_str_cmp_id.Trim();
            objOutboundInq.Sonum = p_str_Sonum.Trim();
            objOutboundInq = ServiceObject.GetpaletId(objOutboundInq);
            objOutboundInq.palet_id = objOutboundInq.palet_id;
            objOutboundInq = ServiceObject.Get_IbdocId(objOutboundInq);
            if (objOutboundInq.ListRMADocId.Count > 0)
            {
                objOutboundInq.ib_doc_id = objOutboundInq.ListRMADocId[0].ct_value;
            }
            objOutboundInq = ServiceObject.Get_LotId(objOutboundInq);
            if (objOutboundInq.LstLotId.Count > 0)
            {
                objOutboundInq.lot_id = objOutboundInq.LstLotId[0].ct_value;
                tmpLotId = "9999"+objOutboundInq.lot_id;
            }
            objOutboundInq = ServiceObject.Get_StockList(objOutboundInq); 
            for (int i = 0; i < objOutboundInq.LstStockverify.Count; i++)
            {
                objOutboundInq.dtl_line = tmpDtlLine + 1;
                objOutboundInq.CtnLn =1;
                objOutboundInq.whs_id = objOutboundInq.LstStockverify[i].whs_id;
                objOutboundInq.itm_num = objOutboundInq.LstStockverify[i].itm_num;
                objOutboundInq.itm_color = objOutboundInq.LstStockverify[i].itm_color;
                objOutboundInq.itm_size = objOutboundInq.LstStockverify[i].itm_size;
                objOutboundInq.itm_name = objOutboundInq.LstStockverify[i].itm_name;
                objOutboundInq.OrderQty = objOutboundInq.LstStockverify[i].back_ordr_qty;
                objOutboundInq.itm_code = objOutboundInq.LstStockverify[i].itm_code;
                objOutboundInq.avail_qty = Convert.ToInt32(objOutboundInq.LstStockverify[i].avail_qty);
                objOutboundInq.back_ordr_qty = objOutboundInq.LstStockverify[i].back_ordr_qty;
                objOutboundInq.balance_qty = objOutboundInq.LstStockverify[i].balance_qty;
                objOutboundInq.Note= "B/O Auto Received for SR#" + objOutboundInq.LstStockverify[i].so_num.Trim();
                objOutboundInq.refno = "SR#" + objOutboundInq.LstStockverify[i].so_num.Trim();
                l_str_Refno = objOutboundInq.refno;
                objOutboundInq.ppk = objOutboundInq.LstStockverify[i].back_ordr_qty;
                objOutboundInq.length = objOutboundInq.LstStockverify[i].length;
                objOutboundInq.width = objOutboundInq.LstStockverify[i].width;
                objOutboundInq.depth = objOutboundInq.LstStockverify[i]. depth;
                objOutboundInq.wgt = objOutboundInq.LstStockverify[i].wgt;
                objOutboundInq.cube = objOutboundInq.LstStockverify[i].cube;
                objOutboundInq.user_id = Session["UserID"].ToString().Trim();
                objOutboundInq.doc_date = DateTime.Now.ToString("MM/dd/yyyy");
                tmpDate = objOutboundInq.doc_date;
                objOutboundInq.rcvd_notes = "B/O Auto Add For." + objOutboundInq.refno;
                objOutboundInq.io_rate_id = "INOUT-1";
                objOutboundInq.st_rate_id = "STRG-1";                            
                objOutboundInq.loc_id = "FLOOR";
               if(objOutboundInq.back_ordr_qty>0)
                {              
                objOutboundInq = ServiceObject.GetPaletIdValidation(objOutboundInq);
                if (objOutboundInq.ListPaletId.Count > 0)
                {
                    objOutboundInq.palet_id = objOutboundInq.ListPaletId[0].palet_id;
                }              
                objOutboundInq = ServiceObject.Validate_IbdocId(objOutboundInq);
                if (objOutboundInq.LstIbdocId.Count > 0)
                {
                    objOutboundInq.ib_doc_id = objOutboundInq.LstIbdocId[0].ib_doc_id;                    
                }
                else
                {
                    ServiceObject.Add_To_proc_save_doc_hdr(objOutboundInq);
                }               
                ServiceObject.Add_To_proc_save_doc_dtl(objOutboundInq);
                tmpDtlLine = objOutboundInq.dtl_line;
                ServiceObject.Add_To_proc_save_doc_ctn(objOutboundInq);
                objOutboundInq = ServiceObject.ExistLoc(objOutboundInq);              
                if (objOutboundInq.LstLocId.Count > 0)
                {
                    objOutboundInq.loc_id = objOutboundInq.LstLocId[0].loc_id;                                   
                }
                else
                {
                    ServiceObject.Insert_loc_hdr(objOutboundInq);
                }               
                objOutboundInq = ServiceObject.Validate_LotId(objOutboundInq);
                if(objOutboundInq.LstLotId.Count>0)
                {
                    objOutboundInq.lot_id = objOutboundInq.LstLotId[0].lot_id;
                    tmpLotId = "9999"+objOutboundInq.lot_id;
                }
                else
                {
                    ServiceObject.Save_Lot_hdr(objOutboundInq);
                }
                objOutboundInq.doc_notes = "B/O Auto Add For the SR#:" + p_str_Sonum.Trim() + " - On Date:" + tmpDate;
                objOutboundInq = ServiceObject.Get_PkgID(objOutboundInq);
                objOutboundInq.doc_pkg_id = objOutboundInq.doc_pkg_id;
                objOutboundInq.pkg_id = objOutboundInq.doc_pkg_id;
                ServiceObject.Save_Lot_dtl(objOutboundInq);
                ServiceObject.Save_Lot_ctn(objOutboundInq);
                ServiceObject.Save_iv_Itm_trn_in(objOutboundInq);
                ServiceObject.Save_iv_Itm_trn_hst(objOutboundInq);  
                ServiceObject.Change_dochdr_Status(objOutboundInq);
                }
            }
            objOutboundInq.refno = l_str_Refno;
            ServiceObject.Update_trn_hdr(objOutboundInq);
            return Json("1", JsonRequestBehavior.AllowGet);
        }
        public ActionResult StockVerifyGrid(string p_str_cmp_id, string p_str_screen_title, string p_str_Sonum, string p_str_batchId, string P_str_SoNumFm, string P_str_SoNumTo, string P_str_screen_MOde)
        {
            int LineNum = 0;
            int Back_Order_Qty = 0;
            int intAvailQty = 0;
            string StrwhsId = string.Empty;
            string StrItmCode = string.Empty;
            string StrItmNum = string.Empty;
            string StrItmColor = string.Empty;
            string StrItmSize = string.Empty;
            int intOrderQty = 0;
            int intTotBOLines = 0;
            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objOutboundInq.cmp_id = p_str_cmp_id.Trim();
            objOutboundInq.CompID = p_str_cmp_id.Trim();
            objOutboundInq.Sonum = p_str_Sonum.Trim();
            objOutboundInq.so_numFm = P_str_SoNumFm.Trim();
            objOutboundInq.so_numTo = P_str_SoNumTo.Trim();
            if(p_str_batchId=="")
            {
                objOutboundInq.quote_num = Session["batchId"].ToString();
            }
            else
            {
                objOutboundInq.quote_num = p_str_batchId.Trim();
                Session["batchId"] = p_str_batchId.Trim();
            }
            objOutboundInq.screentitle = p_str_screen_title.Trim();
            ServiceObject.Delete_StockVerify(objOutboundInq);
            objOutboundInq = ServiceObject.CheckOpenOrderExist(objOutboundInq);
            if (objOutboundInq.LstStockverify.Count > 0)
            {
                objOutboundInq.TotRecs = objOutboundInq.LstStockverify[0].TotRecs;
            }
            else
            {
                objOutboundInq.TotRecs = 0;
            }
            if (objOutboundInq.TotRecs == 0)
            {
                int ResultCount = 1;
                return Json(ResultCount, JsonRequestBehavior.AllowGet);
            }
            dtOutInq = new DataTable();
            List<OutboundInq> li = new List<OutboundInq>();
            //2: Initialize a object of type DataRow
            DataRow drOBAloc;
            //3: Initialize enough objects of type DataColumns
            DataColumn colLine = new DataColumn("LineNum", typeof(string));
            DataColumn colCmpId = new DataColumn("cmp_id", typeof(string));
            DataColumn colwhs_id = new DataColumn("whs_id", typeof(string));
            DataColumn colitm_code = new DataColumn("itm_code", typeof(string));
            DataColumn colStyle = new DataColumn("Style", typeof(string));
            DataColumn colColor = new DataColumn("Color", typeof(string));
            DataColumn colSize = new DataColumn("Size", typeof(string));
            DataColumn colitmName = new DataColumn("itm_name", typeof(string));
            DataColumn colOrderQty = new DataColumn("OrderQty", typeof(decimal));
            DataColumn colAloc_Qty = new DataColumn("Aloc_Qty", typeof(decimal));
            DataColumn colBack_Order_Qty = new DataColumn("Back_Order_Qty", typeof(decimal));
            DataColumn colBalanceQty = new DataColumn("balance_qty", typeof(decimal));
            DataColumn colbackorderCount = new DataColumn("backorderCount", typeof(int));
            int lintCount = 0;
            //4: Adding DataColumns to DataTable dt           
            dtOutInq.Columns.Add(colLine);
            dtOutInq.Columns.Add(colCmpId);
            dtOutInq.Columns.Add(colwhs_id);
            dtOutInq.Columns.Add(colitm_code);
            dtOutInq.Columns.Add(colStyle);
            dtOutInq.Columns.Add(colColor);
            dtOutInq.Columns.Add(colSize);
            dtOutInq.Columns.Add(colitmName);
            dtOutInq.Columns.Add(colOrderQty);
            dtOutInq.Columns.Add(colAloc_Qty);
            dtOutInq.Columns.Add(colBack_Order_Qty);
            dtOutInq.Columns.Add(colBalanceQty);
            dtOutInq.Columns.Add(colbackorderCount);
            objOutboundInq = ServiceObject.ShowStockVerifyRpt(objOutboundInq);
            for (int i = 0; i < objOutboundInq.LstStkverifyList.Count(); i++)
            {
                objOutboundInq.cmp_id = objOutboundInq.LstStkverifyList[i].cmp_id;
                objOutboundInq.whs_id = objOutboundInq.LstStkverifyList[i].whs_id;
                StrwhsId = objOutboundInq.whs_id;
                objOutboundInq.itm_code = objOutboundInq.LstStkverifyList[i].itm_code;
                StrItmCode = objOutboundInq.itm_code;
                objOutboundInq.Style = objOutboundInq.LstStkverifyList[i].Style;
                StrItmNum = objOutboundInq.Style;
                objOutboundInq.Color = objOutboundInq.LstStkverifyList[i].Color;
                StrItmColor = objOutboundInq.Color;
                objOutboundInq.Size = objOutboundInq.LstStkverifyList[i].Size;
                StrItmSize = objOutboundInq.Size;
                objOutboundInq.OrderQty = objOutboundInq.LstStkverifyList[i].OrderQty;
                objOutboundInq.Back_Order_Qty = objOutboundInq.LstStkverifyList[i].Back_Order_Qty;
                objOutboundInq.Aloc_Qty = objOutboundInq.LstStkverifyList[i].Aloc_Qty;
                intOrderQty = objOutboundInq.Aloc_Qty;
                Back_Order_Qty = objOutboundInq.Back_Order_Qty;
                LineNum = LineNum + 1;
                drOBAloc = dtOutInq.NewRow();
                dtOutInq.Rows.Add(drOBAloc);
                dtOutInq.Rows[lintCount][colLine] = LineNum;
                dtOutInq.Rows[lintCount][colCmpId] = objOutboundInq.cmp_id.ToString();
                dtOutInq.Rows[lintCount][colwhs_id] = objOutboundInq.whs_id.ToString();
                dtOutInq.Rows[lintCount][colitm_code] = objOutboundInq.itm_code.ToString();
                dtOutInq.Rows[lintCount][colStyle] = objOutboundInq.Style.ToString();
                dtOutInq.Rows[lintCount][colColor] = objOutboundInq.Color.ToString();
                dtOutInq.Rows[lintCount][colSize] = objOutboundInq.Size.ToString();
                dtOutInq.Rows[lintCount][colOrderQty] = objOutboundInq.OrderQty.ToString();
                OutboundInq objOutboundInqDtltemp = new OutboundInq();
                objOutboundInqDtltemp.cmp_id = objOutboundInq.cmp_id;
                objOutboundInqDtltemp.LineNum = LineNum;
                objOutboundInqDtltemp.itm_code = objOutboundInq.itm_code;
                objOutboundInqDtltemp.Style = objOutboundInq.Style;
                objOutboundInqDtltemp.Color = objOutboundInq.Color;
                objOutboundInqDtltemp.Size = objOutboundInq.Size;
                objOutboundInqDtltemp.whs_id = objOutboundInq.whs_id;
                if (Back_Order_Qty > 0)
                {
                    objOutboundInqDtltemp.OrderQty = objOutboundInq.Back_Order_Qty;
                    objOutboundInq.OrderQty = objOutboundInqDtltemp.OrderQty;
                }
                else
                {
                    objOutboundInqDtltemp.OrderQty = objOutboundInq.OrderQty;
                    objOutboundInq.OrderQty = objOutboundInqDtltemp.OrderQty;
                }
                objOutboundInq.cmp_id = p_str_cmp_id;
                objOutboundInq.itm_code = StrItmCode;
                objOutboundInq.itm_num = StrItmNum;
                objOutboundInq.itm_color = StrItmColor;
                objOutboundInq.itm_size = StrItmSize;
                objOutboundInq.whs_id = StrwhsId;
                objOutboundInq = ServiceObject.GetItemName(objOutboundInq);
                if (objOutboundInq.LstOutboundPickQty.Count > 0)
                {
                    objOutboundInqDtltemp.itm_name = objOutboundInq.LstOutboundPickQty[0].itm_name;
                    objOutboundInq.itm_name = objOutboundInqDtltemp.itm_name;
                }
                else
                {
                    objOutboundInqDtltemp.itm_name = "-";
                    objOutboundInq.itm_name = "-";
                }
                objOutboundInq = ServiceObject.OutboundGETAVILQTY(objOutboundInq);
                objOutboundInq.avail_qty = objOutboundInq.LstAvailqty[0].pkg_avail_cnt;
                objOutboundInqDtltemp.avail_qty = objOutboundInq.avail_qty;
                intAvailQty = Convert.ToInt32(objOutboundInqDtltemp.avail_qty);
                intOrderQty = objOutboundInqDtltemp.OrderQty;
                if ((intAvailQty - intOrderQty) >= 0)
                {
                    objOutboundInqDtltemp.back_ordr_qty = 0;
                    objOutboundInq.Back_Order_Qty = 0;
                    objOutboundInqDtltemp.balance_qty = intAvailQty - intOrderQty;
                    objOutboundInq.balance_qty = objOutboundInqDtltemp.balance_qty;
                }
                else
                {
                    objOutboundInqDtltemp.back_ordr_qty = intOrderQty - intAvailQty;
                    objOutboundInq.Back_Order_Qty = objOutboundInqDtltemp.back_ordr_qty;
                    objOutboundInqDtltemp.balance_qty = 0;
                    objOutboundInq.balance_qty = 0;
                    intTotBOLines = intTotBOLines + 1;
                    objOutboundInqDtltemp.backorderCount = intTotBOLines;

                }
                if (P_str_SoNumFm == "" || P_str_SoNumFm == null)
                {
                    if (p_str_batchId == "" || p_str_batchId == null)
                    {
                        objOutboundInq = ServiceObject.GETSONUMLIST(objOutboundInq);
                        objOutboundInq.Sonum = objOutboundInq.lstShipAlocdtl[0].so_num;
                    }
                    else
                    {
                        objOutboundInq.Sonum = p_str_batchId;
                    }
                }
                else
                {
                    objOutboundInq.Sonum = P_str_SoNumFm;
                    objOutboundInq.so_num = P_str_SoNumFm;
                }
                ServiceObject.Insert_StockVerify(objOutboundInq);
                li.Add(objOutboundInqDtltemp);
                lintCount++;
            }
            objOutboundInq.backorderCount = intTotBOLines;
            objOutboundInq.LstStockverify = li;
            Session["GridStockverifyList"] = objOutboundInq.LstStockverify;
            objOutboundInq.ScreenMode = P_str_screen_MOde.Trim();
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundShipModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_Stock_Verify_Grid", objOutboundShipModel);
        }
        public ActionResult AlocStockVerify(string p_str_cmp_id, string p_str_screen_title, string p_str_Sonum, string p_str_batchId, string P_str_SoNumFm, string P_str_SoNumTo)
        {
            int LineNum = 0;
            int Back_Order_Qty = 0;
            int intAvailQty = 0;
            string StrwhsId = string.Empty;
            string StrItmCode = string.Empty;
            string StrItmNum = string.Empty;
            string StrItmColor = string.Empty;
            string StrItmSize = string.Empty;
            int intOrderQty = 0;
            int intTotBOLines = 0;
            int Avail_Qty = 0;
            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objOutboundInq.cmp_id = p_str_cmp_id.Trim();
            objOutboundInq.CompID = p_str_cmp_id.Trim();
            objOutboundInq.Sonum = p_str_Sonum.Trim();
            if (p_str_batchId == "")
            {
                objOutboundInq.quote_num = Session["batchId"].ToString();
            }
            else
            {
                objOutboundInq.quote_num = p_str_batchId;
                Session["batchId"] = p_str_batchId;
            }
            //objOutboundInq.quote_num = p_str_batchId;
            objOutboundInq.so_numFm = P_str_SoNumFm.Trim();
            objOutboundInq.so_numTo = P_str_SoNumTo.Trim();
            objOutboundInq.screentitle = p_str_screen_title.Trim();
            ServiceObject.Delete_StockVerify(objOutboundInq);
            objOutboundInq = ServiceObject.CheckOpenOrderExist(objOutboundInq);
            if (objOutboundInq.LstStockverify.Count > 0)
            {
                objOutboundInq.TotRecs = objOutboundInq.LstStockverify[0].TotRecs;
            }
            else
            {
                objOutboundInq.TotRecs = 0;
            }
            if (objOutboundInq.TotRecs == 0)
            {
                int ResultCount = 1;
                return Json(ResultCount, JsonRequestBehavior.AllowGet);
            }
            dtOutInq = new DataTable();
            List<OutboundInq> li = new List<OutboundInq>();
            //2: Initialize a object of type DataRow
            DataRow drOBAloc;
            //3: Initialize enough objects of type DataColumns
            DataColumn colLine = new DataColumn("LineNum", typeof(string));
            DataColumn colCmpId = new DataColumn("cmp_id", typeof(string));
            DataColumn colwhs_id = new DataColumn("whs_id", typeof(string));
            DataColumn colitm_code = new DataColumn("itm_code", typeof(string));
            DataColumn colStyle = new DataColumn("Style", typeof(string));
            DataColumn colColor = new DataColumn("Color", typeof(string));
            DataColumn colSize = new DataColumn("Size", typeof(string));
            DataColumn colitmName = new DataColumn("itm_name", typeof(string));
            DataColumn colOrderQty = new DataColumn("OrderQty", typeof(decimal));
            DataColumn colAloc_Qty = new DataColumn("Aloc_Qty", typeof(decimal));
            DataColumn colBack_Order_Qty = new DataColumn("Back_Order_Qty", typeof(decimal));
            DataColumn colBalanceQty = new DataColumn("balance_qty", typeof(decimal));
            DataColumn colbackorderCount = new DataColumn("backorderCount", typeof(int));
            int lintCount = 0;
            //4: Adding DataColumns to DataTable dt           
            dtOutInq.Columns.Add(colLine);
            dtOutInq.Columns.Add(colCmpId);
            dtOutInq.Columns.Add(colwhs_id);
            dtOutInq.Columns.Add(colitm_code);
            dtOutInq.Columns.Add(colStyle);
            dtOutInq.Columns.Add(colColor);
            dtOutInq.Columns.Add(colSize);
            dtOutInq.Columns.Add(colitmName);
            dtOutInq.Columns.Add(colOrderQty);
            dtOutInq.Columns.Add(colAloc_Qty);
            dtOutInq.Columns.Add(colBack_Order_Qty);
            dtOutInq.Columns.Add(colBalanceQty);
            dtOutInq.Columns.Add(colbackorderCount);
            objOutboundInq = ServiceObject.ShowStockVerifyRpt(objOutboundInq);
            for (int i = 0; i < objOutboundInq.LstStkverifyList.Count(); i++)
            {
                objOutboundInq.cmp_id = objOutboundInq.LstStkverifyList[i].cmp_id;
                objOutboundInq.whs_id = objOutboundInq.LstStkverifyList[i].whs_id;
                StrwhsId = objOutboundInq.whs_id;
                objOutboundInq.itm_code = objOutboundInq.LstStkverifyList[i].itm_code;
                StrItmCode = objOutboundInq.itm_code;
                objOutboundInq.Style = objOutboundInq.LstStkverifyList[i].Style;
                StrItmNum = objOutboundInq.Style;
                objOutboundInq.Color = objOutboundInq.LstStkverifyList[i].Color;
                StrItmColor = objOutboundInq.Color;
                objOutboundInq.Size = objOutboundInq.LstStkverifyList[i].Size;
                StrItmSize = objOutboundInq.Size;
                objOutboundInq.OrderQty = objOutboundInq.LstStkverifyList[i].OrderQty;
                objOutboundInq.Back_Order_Qty = objOutboundInq.LstStkverifyList[i].Back_Order_Qty;
                objOutboundInq.Aloc_Qty = objOutboundInq.LstStkverifyList[i].Aloc_Qty;
                intOrderQty = objOutboundInq.Aloc_Qty;
                Back_Order_Qty = objOutboundInq.Back_Order_Qty;
                LineNum = LineNum + 1;
                drOBAloc = dtOutInq.NewRow();
                dtOutInq.Rows.Add(drOBAloc);
                dtOutInq.Rows[lintCount][colLine] = LineNum;
                dtOutInq.Rows[lintCount][colCmpId] = objOutboundInq.cmp_id.ToString();
                dtOutInq.Rows[lintCount][colwhs_id] = objOutboundInq.whs_id.ToString();
                dtOutInq.Rows[lintCount][colitm_code] = objOutboundInq.itm_code.ToString();
                dtOutInq.Rows[lintCount][colStyle] = objOutboundInq.Style.ToString();
                dtOutInq.Rows[lintCount][colColor] = objOutboundInq.Color.ToString();
                dtOutInq.Rows[lintCount][colSize] = objOutboundInq.Size.ToString();
                dtOutInq.Rows[lintCount][colOrderQty] = objOutboundInq.OrderQty.ToString();
                OutboundInq objOutboundInqDtltemp = new OutboundInq();
                objOutboundInqDtltemp.cmp_id = objOutboundInq.cmp_id;
                objOutboundInqDtltemp.LineNum = LineNum;
                objOutboundInqDtltemp.itm_code = objOutboundInq.itm_code;
                objOutboundInqDtltemp.Style = objOutboundInq.Style;
                objOutboundInqDtltemp.Color = objOutboundInq.Color;
                objOutboundInqDtltemp.Size = objOutboundInq.Size;
                objOutboundInqDtltemp.whs_id = objOutboundInq.whs_id;
                if (Back_Order_Qty > 0)
                {
                    objOutboundInqDtltemp.OrderQty = objOutboundInq.Back_Order_Qty;
                    objOutboundInq.OrderQty = objOutboundInqDtltemp.OrderQty;
                }
                else
                {
                    objOutboundInqDtltemp.OrderQty = objOutboundInq.OrderQty;
                    objOutboundInq.OrderQty = objOutboundInqDtltemp.OrderQty;
                }
                objOutboundInq.cmp_id = p_str_cmp_id;
                objOutboundInq.itm_code = StrItmCode;
                objOutboundInq.itm_num = StrItmNum;
                objOutboundInq.itm_color = StrItmColor;
                objOutboundInq.itm_size = StrItmSize;
                objOutboundInq.whs_id = StrwhsId;
                objOutboundInq = ServiceObject.GetItemName(objOutboundInq);
                if (objOutboundInq.LstOutboundPickQty.Count > 0)
                {
                    objOutboundInqDtltemp.itm_name = objOutboundInq.LstOutboundPickQty[0].itm_name;
                    objOutboundInq.itm_name = objOutboundInqDtltemp.itm_name;
                }
                else
                {
                    objOutboundInqDtltemp.itm_name = "-";
                    objOutboundInq.itm_name = "-";
                }
                objOutboundInq = ServiceObject.OutboundGETAVILQTY(objOutboundInq);
                objOutboundInq.avail_qty = objOutboundInq.LstAvailqty[0].pkg_avail_cnt;
                objOutboundInqDtltemp.avail_qty = objOutboundInq.avail_qty;
                intAvailQty = Convert.ToInt32(objOutboundInqDtltemp.avail_qty);
                intOrderQty = objOutboundInqDtltemp.OrderQty;
                if ((intAvailQty - intOrderQty) >= 0)
                {
                    objOutboundInqDtltemp.back_ordr_qty = 0;
                    objOutboundInq.Back_Order_Qty = 0;
                    objOutboundInqDtltemp.balance_qty = intAvailQty - intOrderQty;
                    objOutboundInq.balance_qty = objOutboundInqDtltemp.balance_qty;
                }
                else
                {
                    objOutboundInqDtltemp.back_ordr_qty = intOrderQty - intAvailQty;
                    objOutboundInq.Back_Order_Qty = objOutboundInqDtltemp.back_ordr_qty;
                    objOutboundInqDtltemp.balance_qty = 0;
                    objOutboundInq.balance_qty = 0;
                    intTotBOLines = intTotBOLines + 1;
                    objOutboundInqDtltemp.backorderCount = intTotBOLines;

                }
                if (P_str_SoNumFm == "" || P_str_SoNumFm == null)
                {
                    if (p_str_batchId == "" || p_str_batchId == null)
                    {
                        objOutboundInq = ServiceObject.GETSONUMLIST(objOutboundInq);
                        objOutboundInq.Sonum = objOutboundInq.lstShipAlocdtl[0].so_num;
                    }
                    else
                    {
                        objOutboundInq.Sonum = p_str_batchId;
                    }
                }
                else
                {
                    objOutboundInq.Sonum = P_str_SoNumFm;
                    objOutboundInq.so_num = P_str_SoNumFm;
                }
                ServiceObject.Insert_StockVerify(objOutboundInq);
                li.Add(objOutboundInqDtltemp);
                lintCount++;
            }
            objOutboundInq.backorderCount = intTotBOLines;
            objOutboundInq.LstStockverify = li;
            Session["GridStockverifyList"] = objOutboundInq.LstStockverify;
            //objOutboundInq.ScreenMode = P_str_screen_MOde;
            for (int j = 0; j < objOutboundInq.LstStockverify.Count(); j++)
            {
                //objOutboundInq.Back_Order_Qty= objOutboundInq.LstStockverify[j].back_ordr_qty;
                //Back_Order_Qty = objOutboundInq.Back_Order_Qty;

                if (objOutboundInq.Back_Order_Qty != 0)
                {
                    int l_int_BackOrder_count = 0;
                    l_int_BackOrder_count = objOutboundInq.Back_Order_Qty;
                    Back_Order_Qty = Back_Order_Qty + 1;                   
                }
                if (objOutboundInq.avail_qty == 0)//CR-20180626-001 added by nithya
                {
                    decimal l_int_avail_qty_count = 0;
                    l_int_avail_qty_count = objOutboundInq.avail_qty;
                    Avail_Qty = Avail_Qty + 1;
                }
            }
            //if(Avail_Qty > 0)
            //{
            //    int ResultCount = 4;
            //    return Json(ResultCount, JsonRequestBehavior.AllowGet);
            //}
            if (Back_Order_Qty > 0)
            {               
                int ResultCount = 3;
                return Json(ResultCount, JsonRequestBehavior.AllowGet);
            }
            else
            {
                int ResultCount = 2;
                return Json(ResultCount, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CheckBackOrder(string p_str_cmpid, string p_str_Alocdocid, string p_str_Sonum)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            OutboundInq objInbound = new OutboundInq();
            OutboundInqService objService = new OutboundInqService();
            try
            {
                if (isValid)
                {
                    OutboundInq objOutboundInq = new OutboundInq();
                    OutboundInqService ServiceObject = new OutboundInqService();
                    objOutboundInq.cmp_id = p_str_cmpid;
                    objOutboundInq.so_num = p_str_Sonum;
                    objOutboundInq = ServiceObject.BackOrderRpt(objOutboundInq);
                    if (objOutboundInq.LstAlocDtl.Count > 0)
                    {
                        int ResultCount = 1;
                        return Json(ResultCount, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        int ResultCount = 2;
                        return Json(ResultCount, JsonRequestBehavior.AllowGet);
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
        public ActionResult BackOrderRpt(string p_str_cmpid, string p_str_Alocdocid, string p_str_Sonum)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            OutboundInq objInbound = new OutboundInq();
            OutboundInqService objService = new OutboundInqService();
            try
            {
                if (isValid)
                {
                    OutboundInq objOutboundInq = new OutboundInq();
                    OutboundInqService ServiceObject = new OutboundInqService();
        
                        strReportName = "rpt_mvc_so_bo.rpt";
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmpid;
                        objOutboundInq.so_num = p_str_Sonum;
                        objOutboundInq = ServiceObject.Get_AlocBackOrderRptList(objOutboundInq);
                        var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                        rd.SetDataSource(rptSource);
                        objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "BackOrderReport");                  
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
        //CR20180609-001 Added By Nithya
        public ActionResult OutboundAlocOrders(string p_str_cmp_id)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.CompID = p_str_cmp_id;           
            objOutboundInq = ServiceObject.OutboundGETTEMPLISTvalue(objOutboundInq);
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel OutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("AlocOrderDetails", OutboundInqModel);
        }
        //END

        public ActionResult ShowStockverifyPDF(string p_str_cmpid, string p_str_Sonum, string p_str_batchId, string P_str_SoNumFm, string P_str_SoNumTo)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string batchId = string.Empty;
            int LineNum = 0;
            int Back_Order_Qty = 0;
            int intAvailQty = 0;
            string StrwhsId = string.Empty;
            string StrItmCode = string.Empty;
            string StrItmNum = string.Empty;
            string StrItmColor = string.Empty;
            string StrItmSize = string.Empty;
            int intOrderQty = 0;
            int intTotBOLines = 0;
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            strReportName = "rpt_iv_stockverify.rpt";
            ReportDocument rd = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
            objOutboundInq.cmp_id = p_str_cmpid.Trim();
            objOutboundInq.CompID = p_str_cmpid.Trim();
            objOutboundInq.quote_num = p_str_batchId.Trim();
            Session["batchId"] = p_str_batchId;
            objOutboundInq.so_numFm = P_str_SoNumFm.Trim();
            objOutboundInq.so_numTo = P_str_SoNumTo.Trim();
            dtOutInq = new DataTable();
            List<OutboundInq> li = new List<OutboundInq>();
            //2: Initialize a object of type DataRow
            DataRow drOBAloc;
            //3: Initialize enough objects of type DataColumns
            DataColumn colLine = new DataColumn("LineNum", typeof(string));
            DataColumn colCmpId = new DataColumn("cmp_id", typeof(string));
            DataColumn colwhs_id = new DataColumn("whs_id", typeof(string));
            DataColumn colitm_code = new DataColumn("itm_code", typeof(string));
            DataColumn colStyle = new DataColumn("itm_num", typeof(string));
            DataColumn colColor = new DataColumn("itm_color", typeof(string));
            DataColumn colSize = new DataColumn("itm_size", typeof(string));
            DataColumn colitmName = new DataColumn("itm_name", typeof(string));
            DataColumn colOrderQty = new DataColumn("OrderQty", typeof(decimal));
            DataColumn colAloc_Qty = new DataColumn("Aloc_Qty", typeof(decimal));
            DataColumn colBack_Order_Qty = new DataColumn("Back_Order_Qty", typeof(decimal));
            DataColumn colBalanceQty = new DataColumn("balance_qty", typeof(decimal));
            DataColumn colbackorderCount = new DataColumn("backorderCount", typeof(int));
            DataColumn colso_num = new DataColumn("so_num", typeof(string));

            int lintCount = 0;
            //4: Adding DataColumns to DataTable dt           
            dtOutInq.Columns.Add(colLine);
            dtOutInq.Columns.Add(colCmpId);
            dtOutInq.Columns.Add(colwhs_id);
            dtOutInq.Columns.Add(colitm_code);
            dtOutInq.Columns.Add(colStyle);
            dtOutInq.Columns.Add(colColor);
            dtOutInq.Columns.Add(colSize);
            dtOutInq.Columns.Add(colitmName);
            dtOutInq.Columns.Add(colOrderQty);
            dtOutInq.Columns.Add(colAloc_Qty);
            dtOutInq.Columns.Add(colBack_Order_Qty);
            dtOutInq.Columns.Add(colBalanceQty);
            dtOutInq.Columns.Add(colbackorderCount);
            dtOutInq.Columns.Add(colso_num);            
            objOutboundInq = ServiceObject.StockverifyRpt(objOutboundInq);
            for (int i = 0; i < objOutboundInq.LstStkverifyList.Count(); i++)
            {
                objOutboundInq.cmp_id = objOutboundInq.LstStkverifyList[i].cmp_id;
                objOutboundInq.whs_id = objOutboundInq.LstStkverifyList[i].whs_id;
                StrwhsId = objOutboundInq.whs_id;
                objOutboundInq.itm_code = objOutboundInq.LstStkverifyList[i].itm_code;
                StrItmCode = objOutboundInq.itm_code;
                objOutboundInq.Style = objOutboundInq.LstStkverifyList[i].Style;
                StrItmNum = objOutboundInq.Style;
                objOutboundInq.Color = objOutboundInq.LstStkverifyList[i].Color;
                StrItmColor = objOutboundInq.Color;
                objOutboundInq.Size = objOutboundInq.LstStkverifyList[i].Size;
                StrItmSize = objOutboundInq.Size;
                objOutboundInq.OrderQty = objOutboundInq.LstStkverifyList[i].OrderQty;
                objOutboundInq.Sonum = objOutboundInq.LstStkverifyList[i].so_num;
                objOutboundInq.Back_Order_Qty = objOutboundInq.LstStkverifyList[i].Back_Order_Qty;
                objOutboundInq.Aloc_Qty = objOutboundInq.LstStkverifyList[i].Aloc_Qty;
                intOrderQty = objOutboundInq.Aloc_Qty;
                Back_Order_Qty = objOutboundInq.Back_Order_Qty;
                LineNum = LineNum + 1;
                drOBAloc = dtOutInq.NewRow();
                dtOutInq.Rows.Add(drOBAloc);
                dtOutInq.Rows[lintCount][colLine] = LineNum;
                dtOutInq.Rows[lintCount][colCmpId] = objOutboundInq.cmp_id.ToString();
                dtOutInq.Rows[lintCount][colwhs_id] = objOutboundInq.whs_id.ToString();
                dtOutInq.Rows[lintCount][colitm_code] = objOutboundInq.itm_code.ToString();
                dtOutInq.Rows[lintCount][colStyle] = objOutboundInq.Style.ToString();
                dtOutInq.Rows[lintCount][colColor] = objOutboundInq.Color.ToString();
                dtOutInq.Rows[lintCount][colSize] = objOutboundInq.Size.ToString();
                dtOutInq.Rows[lintCount][colOrderQty] = objOutboundInq.OrderQty.ToString();
                dtOutInq.Rows[lintCount][colso_num] = objOutboundInq.Sonum.ToString();
                OutboundInq objOutboundInqDtltemp = new OutboundInq();
                objOutboundInqDtltemp.cmp_id = objOutboundInq.cmp_id;
                objOutboundInqDtltemp.LineNum = LineNum;
                objOutboundInqDtltemp.itm_code = objOutboundInq.itm_code;
                objOutboundInqDtltemp.itm_num = objOutboundInq.Style;
                objOutboundInqDtltemp.itm_color = objOutboundInq.Color;
                objOutboundInqDtltemp.itm_size = objOutboundInq.Size;
                objOutboundInqDtltemp.whs_id = objOutboundInq.whs_id;
                objOutboundInqDtltemp.Sonum = objOutboundInq.Sonum;

                if (Back_Order_Qty > 0)
                {
                    objOutboundInqDtltemp.OrderQty = objOutboundInq.Back_Order_Qty;
                    objOutboundInq.OrderQty = objOutboundInqDtltemp.OrderQty;
                }
                else
                {
                    objOutboundInqDtltemp.OrderQty = objOutboundInq.OrderQty;
                    objOutboundInq.OrderQty = objOutboundInqDtltemp.OrderQty;
                }
                objOutboundInq.cmp_id = p_str_cmpid;
                objOutboundInq.itm_code = StrItmCode;
                objOutboundInq.itm_num = StrItmNum;
                objOutboundInq.itm_color = StrItmColor;
                objOutboundInq.itm_size = StrItmSize;
                objOutboundInq.whs_id = StrwhsId;
                objOutboundInq = ServiceObject.GetItemName(objOutboundInq);
                if (objOutboundInq.LstOutboundPickQty.Count > 0)
                {
                    objOutboundInqDtltemp.itm_name = objOutboundInq.LstOutboundPickQty[0].itm_name;
                    objOutboundInq.itm_name = objOutboundInqDtltemp.itm_name;
                }
                else
                {
                    objOutboundInqDtltemp.itm_name = "-";
                    objOutboundInq.itm_name = "-";
                }
                objOutboundInq = ServiceObject.OutboundGETAVILQTY(objOutboundInq);
                objOutboundInq.avail_qty = objOutboundInq.LstAvailqty[0].pkg_avail_cnt;
                objOutboundInqDtltemp.avail_qty = objOutboundInq.avail_qty;
                intAvailQty = Convert.ToInt32(objOutboundInqDtltemp.avail_qty);
                intOrderQty = objOutboundInqDtltemp.OrderQty;
                if ((intAvailQty - intOrderQty) >= 0)
                {
                    objOutboundInqDtltemp.back_ordr_qty = 0;
                    objOutboundInq.Back_Order_Qty = 0;
                    objOutboundInqDtltemp.balance_qty = intAvailQty - intOrderQty;
                    objOutboundInq.balance_qty = objOutboundInqDtltemp.balance_qty;
                }
                else
                {
                    objOutboundInqDtltemp.back_ordr_qty = intOrderQty - intAvailQty;
                    objOutboundInq.Back_Order_Qty = objOutboundInqDtltemp.back_ordr_qty;
                    objOutboundInqDtltemp.balance_qty = 0;
                    objOutboundInq.balance_qty = 0;
                    intTotBOLines = intTotBOLines + 1;
                    objOutboundInqDtltemp.backorderCount = intTotBOLines;

                }
                if (P_str_SoNumFm == "" || P_str_SoNumFm == null)
                {
                    if (p_str_batchId == "" || p_str_batchId == null)
                    {
                        objOutboundInq = ServiceObject.GETSONUMLIST(objOutboundInq);
                        objOutboundInq.Sonum = objOutboundInq.lstShipAlocdtl[0].so_num;
                    }
                    else
                    {
                        objOutboundInq.Sonum = p_str_batchId;
                    }
                }
                else
                {
                    objOutboundInq.Sonum = P_str_SoNumFm;
                    objOutboundInq.so_num = P_str_SoNumFm;
                }
                li.Add(objOutboundInqDtltemp);
                lintCount++;
            }
            objOutboundInq.backorderCount = intTotBOLines;
            objOutboundInq.LstStkverifyList = li;
            Session["GridStockverifyList"] = objOutboundInq.LstStkverifyList;
            objCompany.cmp_id = p_str_cmpid.Trim();
            objCompany = ServiceObjectCompany.CompanyAddresHdrDtls(objCompany);
            objOutboundInq.ListCompanyAddresHdrDtls = objCompany.ListCompanyAddresHdrDtls;
            try
            {
                if (isValid)
                {

                    var rptSource = objOutboundInq.LstStkverifyList.ToList();
                    rd.Load(strRptPath);
                    int AlocCount = 0;
                    AlocCount = objOutboundInq.LstStkverifyList.Count();
                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                        rd.SetDataSource(rptSource);
                    objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                    rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                    batchId = "Batch Id :" + batchId + "";
                        if (!string.IsNullOrEmpty(p_str_batchId))
                            rd.SetParameterValue("fml_rep_selectionBy", p_str_batchId);
      
                        //if (!string.IsNullOrEmpty(P_str_SoNumFm))
                            rd.SetParameterValue("SoNumFm", P_str_SoNumFm);
                        //if (!string.IsNullOrEmpty(P_str_SoNumTo))
                            rd.SetParameterValue("SoNumTo", P_str_SoNumTo);
                    rd.DataDefinition.FormulaFields["fmlCompanyName"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].cmp_name.ToString().Trim() + "'";
                    rd.DataDefinition.FormulaFields["fmlCompAddress"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].addr_line1.ToString().Trim() + "'";
                    rd.DataDefinition.FormulaFields["fmlCompCity"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].city.ToString().Trim() + "'";
                    rd.DataDefinition.FormulaFields["fmlCompstate_id"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].state_id.ToString().Trim() + "'";
                    rd.DataDefinition.FormulaFields["fmlCompPhone"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].tel.ToString().Trim() + "'";
                    rd.DataDefinition.FormulaFields["fmlCompFax"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].fax.ToString().Trim() + "'";
                    rd.DataDefinition.FormulaFields["fmlCompPostCode"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].post_code.ToString().Trim() + "'";
                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "StockVerification");
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
        public ActionResult ShowEmailstkReport(string p_str_cmpid, string p_str_Sonum, string p_str_batchId, string P_str_SoNumFm, string P_str_SoNumTo,string type)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string batchId = string.Empty;
            int LineNum = 0;
            int Back_Order_Qty = 0;
            int intAvailQty = 0;
            string StrwhsId = string.Empty;
            string StrItmCode = string.Empty;
            string StrItmNum = string.Empty;
            string StrItmColor = string.Empty;
            string StrItmSize = string.Empty;
            int intOrderQty = 0;
            int intTotBOLines = 0;
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            string l_str_rpt_so_num = string.Empty;
            string strDateFormat = string.Empty;
            string strFileName = string.Empty;
            string reportFileName = string.Empty;
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            Folderpath = System.Configuration.ConfigurationManager.AppSettings["DefaultFolderPath"].ToString().Trim();
         
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            objCompany.cmp_id = p_str_cmpid;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            l_str_tmp_name = objCompany.LstCmpName[0].cmp_name.ToString().Trim();
            l_str_rpt_selection = "Stock Verify";
            strReportName = "rpt_iv_stockverify.rpt";
            ReportDocument rd = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
            objOutboundInq.cmp_id = p_str_cmpid.Trim();
            objOutboundInq.CompID = p_str_cmpid.Trim();
            objOutboundInq.quote_num = p_str_batchId.Trim();
            Session["batchId"] = p_str_batchId;
            objOutboundInq.so_numFm = P_str_SoNumFm.Trim();
            objOutboundInq.so_numTo = P_str_SoNumTo.Trim();
            dtOutInq = new DataTable();
            List<OutboundInq> li = new List<OutboundInq>();
           
            //2: Initialize a object of type DataRow
            DataRow drOBAloc;
            //3: Initialize enough objects of type DataColumns
            DataColumn colLine = new DataColumn("LineNum", typeof(string));
            DataColumn colCmpId = new DataColumn("cmp_id", typeof(string));
            DataColumn colwhs_id = new DataColumn("whs_id", typeof(string));
            DataColumn colitm_code = new DataColumn("itm_code", typeof(string));
            DataColumn colStyle = new DataColumn("itm_num", typeof(string));
            DataColumn colColor = new DataColumn("itm_color", typeof(string));
            DataColumn colSize = new DataColumn("itm_size", typeof(string));
            DataColumn colitmName = new DataColumn("itm_name", typeof(string));
            DataColumn colOrderQty = new DataColumn("OrderQty", typeof(decimal));
            DataColumn colAloc_Qty = new DataColumn("Aloc_Qty", typeof(decimal));
            DataColumn colBack_Order_Qty = new DataColumn("Back_Order_Qty", typeof(decimal));
            DataColumn colBalanceQty = new DataColumn("balance_qty", typeof(decimal));
            DataColumn colbackorderCount = new DataColumn("backorderCount", typeof(int));
            DataColumn colso_num = new DataColumn("so_num", typeof(string));
              int lintCount = 0;
            //4: Adding DataColumns to DataTable dt           
            dtOutInq.Columns.Add(colLine);
            dtOutInq.Columns.Add(colCmpId);
            dtOutInq.Columns.Add(colwhs_id);
            dtOutInq.Columns.Add(colitm_code);
            dtOutInq.Columns.Add(colStyle);
            dtOutInq.Columns.Add(colColor);
            dtOutInq.Columns.Add(colSize);
            dtOutInq.Columns.Add(colitmName);
            dtOutInq.Columns.Add(colOrderQty);
            dtOutInq.Columns.Add(colAloc_Qty);
            dtOutInq.Columns.Add(colBack_Order_Qty);
            dtOutInq.Columns.Add(colBalanceQty);
            dtOutInq.Columns.Add(colbackorderCount);
            dtOutInq.Columns.Add(colso_num);
            objOutboundInq = ServiceObject.StockverifyRpt(objOutboundInq);
            for (int i = 0; i < objOutboundInq.LstStkverifyList.Count(); i++)
            {
                objOutboundInq.cmp_id = objOutboundInq.LstStkverifyList[i].cmp_id;
                objOutboundInq.whs_id = objOutboundInq.LstStkverifyList[i].whs_id;
                StrwhsId = objOutboundInq.whs_id;
                objOutboundInq.itm_code = objOutboundInq.LstStkverifyList[i].itm_code;
                StrItmCode = objOutboundInq.itm_code;
                objOutboundInq.Style = objOutboundInq.LstStkverifyList[i].Style;
                StrItmNum = objOutboundInq.Style;
                objOutboundInq.Color = objOutboundInq.LstStkverifyList[i].Color;
                StrItmColor = objOutboundInq.Color;
                objOutboundInq.Size = objOutboundInq.LstStkverifyList[i].Size;
                StrItmSize = objOutboundInq.Size;
                objOutboundInq.OrderQty = objOutboundInq.LstStkverifyList[i].OrderQty;
                objOutboundInq.Sonum = objOutboundInq.LstStkverifyList[i].so_num;
                objOutboundInq.Back_Order_Qty = objOutboundInq.LstStkverifyList[i].Back_Order_Qty;
                objOutboundInq.Aloc_Qty = objOutboundInq.LstStkverifyList[i].Aloc_Qty;
                intOrderQty = objOutboundInq.Aloc_Qty;
                Back_Order_Qty = objOutboundInq.Back_Order_Qty;
                LineNum = LineNum + 1;
                drOBAloc = dtOutInq.NewRow();
                dtOutInq.Rows.Add(drOBAloc);
                dtOutInq.Rows[lintCount][colLine] = LineNum;
                dtOutInq.Rows[lintCount][colCmpId] = objOutboundInq.cmp_id.ToString();
                dtOutInq.Rows[lintCount][colwhs_id] = objOutboundInq.whs_id.ToString();
                dtOutInq.Rows[lintCount][colitm_code] = objOutboundInq.itm_code.ToString();
                dtOutInq.Rows[lintCount][colStyle] = objOutboundInq.Style.ToString();
                dtOutInq.Rows[lintCount][colColor] = objOutboundInq.Color.ToString();
                dtOutInq.Rows[lintCount][colSize] = objOutboundInq.Size.ToString();
                dtOutInq.Rows[lintCount][colOrderQty] = objOutboundInq.OrderQty.ToString();
                dtOutInq.Rows[lintCount][colso_num] = objOutboundInq.Sonum.ToString();
                OutboundInq objOutboundInqDtltemp = new OutboundInq();
                objOutboundInqDtltemp.cmp_id = objOutboundInq.cmp_id;
                objOutboundInqDtltemp.LineNum = LineNum;
                objOutboundInqDtltemp.itm_code = objOutboundInq.itm_code;
                objOutboundInqDtltemp.itm_num = objOutboundInq.Style;
                objOutboundInqDtltemp.itm_color = objOutboundInq.Color;
                objOutboundInqDtltemp.itm_size = objOutboundInq.Size;
                objOutboundInqDtltemp.whs_id = objOutboundInq.whs_id;
                objOutboundInqDtltemp.Sonum = objOutboundInq.Sonum;

                if (Back_Order_Qty > 0)
                {
                    objOutboundInqDtltemp.OrderQty = objOutboundInq.Back_Order_Qty;
                    objOutboundInq.OrderQty = objOutboundInqDtltemp.OrderQty;
                }
                else
                {
                    objOutboundInqDtltemp.OrderQty = objOutboundInq.OrderQty;
                    objOutboundInq.OrderQty = objOutboundInqDtltemp.OrderQty;
                }
                objOutboundInq.cmp_id = p_str_cmpid;
                objOutboundInq.itm_code = StrItmCode;
                objOutboundInq.itm_num = StrItmNum;
                objOutboundInq.itm_color = StrItmColor;
                objOutboundInq.itm_size = StrItmSize;
                objOutboundInq.whs_id = StrwhsId;
                objOutboundInq = ServiceObject.GetItemName(objOutboundInq);
                if (objOutboundInq.LstOutboundPickQty.Count > 0)
                {
                    objOutboundInqDtltemp.itm_name = objOutboundInq.LstOutboundPickQty[0].itm_name;
                    objOutboundInq.itm_name = objOutboundInqDtltemp.itm_name;
                }
                else
                {
                    objOutboundInqDtltemp.itm_name = "-";
                    objOutboundInq.itm_name = "-";
                }
                objOutboundInq = ServiceObject.OutboundGETAVILQTY(objOutboundInq);
                objOutboundInq.avail_qty = objOutboundInq.LstAvailqty[0].pkg_avail_cnt;
                objOutboundInqDtltemp.avail_qty = objOutboundInq.avail_qty;
                intAvailQty = Convert.ToInt32(objOutboundInqDtltemp.avail_qty);
                intOrderQty = objOutboundInqDtltemp.OrderQty;
                if ((intAvailQty - intOrderQty) >= 0)
                {
                    objOutboundInqDtltemp.back_ordr_qty = 0;
                    objOutboundInq.Back_Order_Qty = 0;
                    objOutboundInqDtltemp.balance_qty = intAvailQty - intOrderQty;
                    objOutboundInq.balance_qty = objOutboundInqDtltemp.balance_qty;
                }
                else
                {
                    objOutboundInqDtltemp.back_ordr_qty = intOrderQty - intAvailQty;
                    objOutboundInq.Back_Order_Qty = objOutboundInqDtltemp.back_ordr_qty;
                    objOutboundInqDtltemp.balance_qty = 0;
                    objOutboundInq.balance_qty = 0;
                    intTotBOLines = intTotBOLines + 1;
                    objOutboundInqDtltemp.backorderCount = intTotBOLines;

                }
                if (P_str_SoNumFm == "" || P_str_SoNumFm == null)
                {
                    if (p_str_batchId == "" || p_str_batchId == null)
                    {
                        objOutboundInq = ServiceObject.GETSONUMLIST(objOutboundInq);
                        objOutboundInq.Sonum = objOutboundInq.lstShipAlocdtl[0].so_num;
                    }
                    else
                    {
                        objOutboundInq.Sonum = p_str_batchId;
                    }
                }
                else
                {
                    objOutboundInq.Sonum = P_str_SoNumFm;
                    objOutboundInq.so_num = P_str_SoNumFm;
                }
                li.Add(objOutboundInqDtltemp);
                lintCount++;
            }
            objOutboundInq.backorderCount = intTotBOLines;
            objOutboundInq.LstStkverifyList = li;
            Session["GridStockverifyList"] = objOutboundInq.LstStkverifyList;
            objCompany.cmp_id = p_str_cmpid.Trim();
            objCompany = ServiceObjectCompany.CompanyAddresHdrDtls(objCompany);
            objOutboundInq.ListCompanyAddresHdrDtls = objCompany.ListCompanyAddresHdrDtls;
          objOutboundInq = ServiceObject.GetStockVerifyRptTotalCottonRecords(objOutboundInq);
            if (objOutboundInq.LstStkverifyListTotal.Count > 0)
            {
                objOutboundInq.totalctns = objOutboundInq.LstStkverifyListTotal[0].totalctns;
                objOutboundInq.totalcube = objOutboundInq.LstStkverifyListTotal[0].totalcube;
            }
            else

            {
                objOutboundInq.totalctns = 0;
                objOutboundInq.totalcube = 0;
            }
            objOutboundInq = ServiceObject.StockverifyRpt(objOutboundInq);
            if (objOutboundInq.LstStkverifyList.Count > 0)
            {
                objOutboundInq.so_num = (objOutboundInq.LstStkverifyList[0].so_num == null || objOutboundInq.LstStkverifyList[0].so_num.Trim() == "" ? string.Empty : objOutboundInq.LstStkverifyList[0].so_num.Trim());
                objOutboundInq.ship_dt = (objOutboundInq.LstStkverifyList[0].so_dt);
                DateTime date =Convert.ToDateTime (objOutboundInq.ship_dt);
                objOutboundInq.ship_dt= (date.ToString("dd/MM/yyyy"));
                objOutboundInq.store_id = (objOutboundInq.LstStkverifyList[0].whs_id == null || objOutboundInq.LstStkverifyList[0].whs_id.Trim() == "" ? string.Empty : objOutboundInq.LstStkverifyList[0].whs_id.Trim());
                objOutboundInq.CustName = (objOutboundInq.LstStkverifyList[0].cust_name == null || objOutboundInq.LstStkverifyList[0].cust_name.Trim() == "" ? string.Empty : objOutboundInq.LstStkverifyList[0].cust_name.Trim());
                objOutboundInq.OrderQty = objOutboundInq.LstStkverifyList[0].OrderQty;
                objOutboundInq.Aloc_Qty = objOutboundInq.LstStkverifyList[0].Aloc_Qty;
                objOutboundInq.Back_Order_Qty = objOutboundInq.LstStkverifyList[0].Back_Order_Qty;


                if ((objOutboundInq.CustName == "" || objOutboundInq.CustName == null || objOutboundInq.CustName == "-"))
                {
                    l_str_rptdtl = objOutboundInq.cmp_id + "_" + "Stock Verification" + "_" + objOutboundInq.so_num + "_" + objOutboundInq.ship_dt;
                    objEmail.EmailSubject = objOutboundInq.cmp_id + "-" + " " + " " + "Stock Verification" + "|" + " " + " " + "SR#: " + objOutboundInq.so_num + "|" + " " + " " + "SR Date: " + objOutboundInq.ship_dt + "|" + " " + " " + "Store: " + objOutboundInq.store_id + "|" + " " + " ";
                    objEmail.EmailMessage = "CmpId: " + " " + " " + objOutboundInq.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "SR#: " + " " + " " + objOutboundInq.so_num + "\n" + "SR Date: " + " " + " " + objOutboundInq.ship_dt + "\n" + "StoreID: " + objOutboundInq.store_id + "\n" + "Total Cartons Requested: " + objOutboundInq.totalctns + " " + "Ctns" + "\n" + " Total Cube: " + objOutboundInq.totalcube + " " + "Lbs" + "\n" + "Order Qty Requested: " + objOutboundInq.OrderQty + "\n" + " Aloc Qty: " + objOutboundInq.Aloc_Qty + "\n " + "BackOrder Qty: " + objOutboundInq.Back_Order_Qty;
                }
                else
                {
                    l_str_rptdtl = objOutboundInq.cmp_id + "_" + "Stock Verification" + "_" + objOutboundInq.so_num + "_" + objOutboundInq.ship_dt;
                    objEmail.EmailSubject = objOutboundInq.cmp_id + "-" + " " + " " + "Stock Verification" + "|" + " " + " " + "SR#: " + objOutboundInq.so_num + "|" + " " + " " + "SR Date: " + objOutboundInq.ship_dt + "|" + " " + " " + "Store: " + objOutboundInq.store_id + "|" + " " + " " + "Ref# -Cust Name : " + objOutboundInq.cust_name;
                    objEmail.EmailMessage = "CmpId: " + " " + " " + objOutboundInq.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "SR#: " + " " + " " + objOutboundInq.so_num + "\n" + "SR Date: " + " " + " " + objOutboundInq.shipdate + "\n" + "StoreID: " + objOutboundInq.store_id + "\n" + "Ref#- " + "\n" + "CustName : " + objOutboundInq.cust_name + "\n" + "Total Cartons Requested: " + objOutboundInq.totalctns + " " + "Ctns" + "\n" + " Total Cube: " + objOutboundInq.totalcube + " " + "Lbs" + "\n" + "Order Qty Requested: " + objOutboundInq.OrderQty + "\n" + " Aloc Qty: " + objOutboundInq.Aloc_Qty + "\n " + "BackOrder Qty: " + objOutboundInq.Back_Order_Qty;
                }

            try
            {
                if (isValid)
                {
                    if (type == "PDF")
                    {
                        var rptSource = objOutboundInq.LstStkverifyList.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objOutboundInq.LstStkverifyList.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                        batchId = "Batch Id :" + batchId + "";
                        if (!string.IsNullOrEmpty(p_str_batchId))
                            rd.SetParameterValue("fml_rep_selectionBy", p_str_batchId);

                        //if (!string.IsNullOrEmpty(P_str_SoNumFm))
                        rd.SetParameterValue("SoNumFm", P_str_SoNumFm);
                        //if (!string.IsNullOrEmpty(P_str_SoNumTo))
                        rd.SetParameterValue("SoNumTo", P_str_SoNumTo);
                        rd.DataDefinition.FormulaFields["fmlCompanyName"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].cmp_name.ToString().Trim() + "'";
                        rd.DataDefinition.FormulaFields["fmlCompAddress"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].addr_line1.ToString().Trim() + "'";
                        rd.DataDefinition.FormulaFields["fmlCompCity"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].city.ToString().Trim() + "'";
                        rd.DataDefinition.FormulaFields["fmlCompstate_id"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].state_id.ToString().Trim() + "'";
                        rd.DataDefinition.FormulaFields["fmlCompPhone"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].tel.ToString().Trim() + "'";
                        rd.DataDefinition.FormulaFields["fmlCompFax"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].fax.ToString().Trim() + "'";
                        rd.DataDefinition.FormulaFields["fmlCompPostCode"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].post_code.ToString().Trim() + "'";
                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath+"//Outbound_StockVerification__" + strDateFormat + ".pdf";
                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                        reportFileName = "Stock_Verification_" + strDateFormat + " _" + objOutboundInq.quote_num + ".pdf";
                        Session["RptFileName"] = strFileName;

                    }                  
                    //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "StockVerification");
                        else
                        {
                        var rptSource = objOutboundInq.LstStkverifyList.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objOutboundInq.LstStkverifyList.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                        if (!string.IsNullOrEmpty(p_str_batchId))
                            rd.SetParameterValue("fml_rep_selectionBy", p_str_batchId);

                        //if (!string.IsNullOrEmpty(P_str_SoNumFm))
                        rd.SetParameterValue("SoNumFm", P_str_SoNumFm);
                        //if (!string.IsNullOrEmpty(P_str_SoNumTo))
                        rd.SetParameterValue("SoNumTo", P_str_SoNumTo);
                        rd.DataDefinition.FormulaFields["fmlCompanyName"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].cmp_name.ToString().Trim() + "'";
                        rd.DataDefinition.FormulaFields["fmlCompAddress"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].addr_line1.ToString().Trim() + "'";
                        rd.DataDefinition.FormulaFields["fmlCompCity"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].city.ToString().Trim() + "'";
                        rd.DataDefinition.FormulaFields["fmlCompstate_id"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].state_id.ToString().Trim() + "'";
                        rd.DataDefinition.FormulaFields["fmlCompPhone"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].tel.ToString().Trim() + "'";
                        rd.DataDefinition.FormulaFields["fmlCompFax"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].fax.ToString().Trim() + "'";
                        rd.DataDefinition.FormulaFields["fmlCompPostCode"].Text = "'" + objOutboundInq.ListCompanyAddresHdrDtls[0].post_code.ToString().Trim() + "'";
                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath +"//Outbound_StockVerification__" + strDateFormat + ".xls";
                        rd.ExportToDisk(ExportFormatType.ExcelWorkbook, strFileName);
                        reportFileName = "Stock_Verification_" + strDateFormat + ".xls";//CR2018-03-07-001 Added By Nithya
                        Session["RptFileName"] = strFileName;
                    }
                }


                else
                {
                    Response.Write("<H2>Report not found</H2>");
                }
           
                objEmail.CmpId = p_str_cmpid;
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
            }

            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);
            
        }
        //CR2018-0629-001 Added bY Nithya
        public ActionResult ShowoutboundReport(string SelectedID, string p_str_cmp_id, string p_str_radio, string p_str_ship_docId_Fm, string p_str_ship_docId_To, string p_str_ship_dt_frm, string p_str_ship_dt_to, string p_str_CustId, string p_str_AlocId, string p_str_Shipto, string p_str_ship_via_name, string p_str_status, string p_str_Whsid, string type)
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
                        ReportDocument rd = new ReportDocument();
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

                        if (type == "PDF")
                        {
                            var rptSource = objOutboundShipInq.LstOutboundShipInqdetail.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundShipInq.LstOutboundShipInqdetail.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundShipInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundShipInq.Image_Path);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "ShipPostGridSummaryReport");
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "ShipPostGridSummaryReport");
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
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundShipInq.cmp_id = p_str_cmp_id;
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
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundShipInq.LstOutboundShipInqpackingSlipRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundShipInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundShipInq.Image_Path);
                            rd.SetParameterValue("SumOfCubes", objOutboundShipInq.TotCube);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "ShipPostPackingSlipReport");
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "ShipPostPackingSlipReport");
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
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundShipInq.cmp_id = p_str_cmp_id;
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
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundShipInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundShipInq.Image_Path);
                            rd.SetParameterValue("TotCube", objOutboundShipInq.TotCube);
                            rd.SetParameterValue("TotCarton", objOutboundShipInq.TotCtns);
                            rd.SetParameterValue("TotWgt", objOutboundShipInq.TotWgt);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "ShipPostBillOfLaddingReport");
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "ShipPostBillOfLaddingReport");
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
        public ActionResult VAsShowReport(string p_str_cmp_id, string p_str_radio, string p_str_vas_id_fm, string p_str_vas_id_to, string p_str_vas_date_fm, string p_str_vas_date_to, string p_str_so_num, string p_str_Status, string SelectedID, string type)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            //string p_str_cmp_id = string.Empty;
            //string p_str_vas_id_fm = string.Empty;
            //string p_str_vas_id_to = string.Empty;
            //string p_str_vas_date_fm = string.Empty;
            //string p_str_vas_date_to = string.Empty;
            //string p_str_so_num = string.Empty;
            //string p_str_Status = string.Empty;
            //l_str_rpt_selection = TempData["ReportSelection"].ToString();
            l_str_rpt_selection = p_str_radio;
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "Vas Inquiry Report")
                    {
                        strReportName = "rpt_iv_vas_details.rpt";
                        VasInquiry objVasInquiry = new VasInquiry();
                        IVasInquiryService ServiceObject = new VasInquiryService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//VasInquiry//" + strReportName;
                        objVasInquiry.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
                        //objVasInquiry.aloc_doc_id =  lstrAlocList ;
                        //objVasInquiry.cmp_id = TempData["cmp_id"].ToString();
                        //objVasInquiry.vas_id_fm = TempData["vas_id_fm"].ToString();
                        //objVasInquiry.vas_id_to = TempData["vas_id_to"].ToString();
                        //objVasInquiry.vas_date_fm = TempData["vas_date_fm"].ToString();
                        //objVasInquiry.vas_date_to = TempData["vas_date_to"].ToString();
                        //objVasInquiry.so_num = TempData["so_num"].ToString();
                        //objVasInquiry.Status = TempData["Status"].ToString();
                        objVasInquiry.cmp_id = p_str_cmp_id;
                        objVasInquiry.vas_id_fm = p_str_vas_id_fm;
                        objVasInquiry.vas_id_to = p_str_vas_id_to;
                        objVasInquiry.vas_date_fm = p_str_vas_date_fm;
                        objVasInquiry.vas_date_to = p_str_vas_date_to;
                        objVasInquiry.so_num = p_str_so_num;
                        objVasInquiry.Status = p_str_Status;


                        objVasInquiry = ServiceObject.GetVasInquiryDetailsRpt(objVasInquiry);


                        if (type == "PDF")
                        {
                            var rptSource = objVasInquiry.ListVasInquiry.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objVasInquiry.ListVasInquiry.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objVasInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objVasInquiry.Image_Path);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Vas Grid Summary");
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Vas Grid Summary");
                        }
                        else
                        if (type == "Excel")
                        {

                            List<VAS_Grid_SummaryExcel> li = new List<VAS_Grid_SummaryExcel>();
                            for (int i = 0; i < objVasInquiry.ListVasInquiry.Count; i++)
                            {

                                VAS_Grid_SummaryExcel objOBInquiryExcel = new VAS_Grid_SummaryExcel();
                                objOBInquiryExcel.VasID = objVasInquiry.ListVasInquiry[i].VasID;
                                objOBInquiryExcel.VasDate = objVasInquiry.ListVasInquiry[i].VasDate;
                                objOBInquiryExcel.status = objVasInquiry.ListVasInquiry[i].status;
                                objOBInquiryExcel.Note = objVasInquiry.ListVasInquiry[i].notes;


                                li.Add(objOBInquiryExcel);
                            }

                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["VAS_GRID_SMRY"] = gv;
                            return new DownloadFileActionResult((GridView)Session["VAS_GRID_SMRY"], "VAS_GRID_SMRY" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



                        }
                    }
                    else if (l_str_rpt_selection == "Vas Post Report")
                    {
                        strReportName = "rpt_iv_vas.rpt";
                        VasInquiry objVasInquiry = new VasInquiry();
                        IVasInquiryService ServiceObject = new VasInquiryService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//VasInquiry//" + strReportName;
                        objVasInquiry.cmp_id = p_str_cmp_id;
                        objVasInquiry.ship_doc_id = SelectedID;
                        objVasInquiry = ServiceObject.GetVasPostDetails(objVasInquiry);

                        if (type == "PDF")
                        {
                            var rptSource = objVasInquiry.ListVasInquiry.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objVasInquiry.ListVasInquiry.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objVasInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objVasInquiry.Image_Path);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Vas Post Report");
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Vas Post Report");
                        }
                        else
                        if (type == "Excel")
                        {

                            List<VAS_POST_DTL_Excel> li = new List<VAS_POST_DTL_Excel>();
                            for (int i = 0; i < objVasInquiry.ListVasInquiry.Count; i++)
                            {

                                VAS_POST_DTL_Excel objOBInquiryExcel = new VAS_POST_DTL_Excel();
                                objOBInquiryExcel.ship_to = objVasInquiry.ListVasInquiry[i].ship_to;
                                objOBInquiryExcel.ship_doc_id = objVasInquiry.ListVasInquiry[i].ship_doc_id;
                                objOBInquiryExcel.ShipDt = objVasInquiry.ListVasInquiry[i].ShipDt;
                                objOBInquiryExcel.whs_id = objVasInquiry.ListVasInquiry[i].whs_id;
                                objOBInquiryExcel.po_num = objVasInquiry.ListVasInquiry[i].po_num;
                                objOBInquiryExcel.dtl_line = objVasInquiry.ListVasInquiry[i].dtl_line;
                                objOBInquiryExcel.so_itm_num = objVasInquiry.ListVasInquiry[i].so_itm_num;
                                objOBInquiryExcel.catg = objVasInquiry.ListVasInquiry[i].catg;
                                objOBInquiryExcel.so_num = objVasInquiry.ListVasInquiry[i].so_num;
                                objOBInquiryExcel.itm_name = objVasInquiry.ListVasInquiry[i].itm_name;
                                objOBInquiryExcel.ship_qty = objVasInquiry.ListVasInquiry[i].ship_qty;
                                objOBInquiryExcel.ship_itm_price = objVasInquiry.ListVasInquiry[i].ship_itm_price;
                                objOBInquiryExcel.cust_name = objVasInquiry.ListVasInquiry[i].cust_name;
                                objOBInquiryExcel.notes = objVasInquiry.ListVasInquiry[i].notes;

                                li.Add(objOBInquiryExcel);
                            }

                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["VAS_GRID_SMRY"] = gv;
                            return new DownloadFileActionResult((GridView)Session["VAS_GRID_SMRY"], "VAS_GRID_SMRY" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



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

    }
}
