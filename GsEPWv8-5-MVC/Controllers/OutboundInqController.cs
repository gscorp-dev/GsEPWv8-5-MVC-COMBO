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
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Web.Script.Serialization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using GsEPWv8_5_MVC.Common;
using GsEPWv8_4_MVC.Common;
using GsEPWv8_4_MVC.ShipWebReference;


#region Change History
// CR_3PL_MVC_OB_2018_0227_001 - Modified by Soniya for set default from date before one year  in filter section
//CR2018-03-08-001 Added By Nithya for Email Function
#endregion Change History
namespace GsEPWv8_5_MVC.Controllers
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
        string l_str_dflt_dt_reqd = string.Empty;
        int SelAlocated = 0;
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
        CustMaster objCustMaster = new CustMaster();
        ICustMasterService objCustMasterService = new CustMasterService();
        DataTable dtOutInq;
        public List<OBLoadUploadDtl> lstOBLoadUploadDtl;
        public List<OBLoadUploadInvalidData> lstOBLoadUploadInvalidData;
        public OBLoadUploadFileInfo objOBLoadUploadFileInfo;
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
                Session["sess_str_doc_type"] = "OB";
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
                    objOutboundInq.so_dtFm = DateTime.Now.AddDays(Common.clsGlobal.DispDateFrom).ToString("MM/dd/yyyy");
                    objOutboundInq.so_dtTo = DateTime.Now.ToString("MM/dd/yyyy");
                    LookUp objLookUp = new LookUp();
                    LookUpService ServiceObject1 = new LookUpService();
                    objLookUp.id = "2";
                    objLookUp.lookuptype = "OUTBOUNDINQUIRY";
                    objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                    objOutboundInq.ListLookUpDtl = objLookUp.ListLookUpDtl;
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
                        if (Session["g_str_p_str_route_dt"] != null)
                        {
                            objOutboundInq.route_dt = Session["g_str_p_str_route_dt"].ToString();
                        }
                      
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
            Session["TEMP_Store"] = String.IsNullOrEmpty(p_str_Store) ? "" : p_str_Store;  
            Session["TEMP_batch_id"] = p_str_batch_id.Trim();
            Session["TEMP_shipdtFm"] = p_str_shipdtFm.Trim();
            Session["TEMP_shipdtTo"] = p_str_shipdtTo.Trim();
            Session["TEMP_STATUS"] = p_str_status.Trim();
            Session["TEMP_cust_name"] = p_str_cust_name.Trim();
            if (p_str_screen_title != null)
            {
                Session["TEMP_SCREEN_ID"] = p_str_screen_title.Trim();
            }

            Session["TEMP_STYLE"] = p_str_Style.Trim();
            Session["TEMP_COLOR"] = p_str_color.Trim();
            Session["TEMP_SIZE"] = p_str_size.Trim();

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

              

                if (System.Configuration.ConfigurationManager.AppSettings["CompanyWebLink"].ToString() == "3plecom.gensoftcorp.com")
                {
                    objOutboundInq.status = "OPEN-ALOC";
                    objOutboundInq = ServiceObject.GetEcomOrderInq(objOutboundInq);
                }
                else
                {
                    if (Session["g_str_p_str_route_dt"] != null)
                    {
                        objOutboundInq.route_dt = Session["g_str_p_str_route_dt"].ToString();
                    }
                    objOutboundInq = ServiceObject.GetOutboundInq(objOutboundInq);
                }

                objOutboundInq.so_num = p_str_so_num;
                ServiceObject.DeleteAUTOALOC(objOutboundInq);
                Mapper.CreateMap<OutboundInq, OutboundInqModel>();
                OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
                 if (System.Configuration.ConfigurationManager.AppSettings["CompanyWebLink"].ToString() == "3plecom.gensoftcorp.com")
                {
                    return PartialView("_EcomOutboundInq", objOutboundInqModel);
                }
                 else
                {
                    return PartialView("_OutboundInq", objOutboundInqModel);
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

        public ActionResult GetOutboundInqDetail(string p_str_cmp_id, string p_str_so_num_frm, string p_str_so_num_To, string p_str_so_dt_frm, string p_str_so_dt_to, string p_str_CustPO, string p_str_AlocId, string p_str_Store, 
            string p_str_batch_id, string p_str_shipdtFm, string p_str_shipdtTo, string p_str_status, string p_str_cust_name, string p_str_screen_title,
            string p_str_Style, string p_str_color, string p_str_size,string p_str_route_dt)
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
                objOutboundInq.screentitle = p_str_screen_title;
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
                objOutboundInq.itm_num = p_str_Style.Trim();
                objOutboundInq.itm_color = p_str_color.Trim();
                objOutboundInq.itm_size = p_str_size.Trim();
                objOutboundInq.route_dt = p_str_route_dt;
                Session["g_str_p_str_route_dt"] = p_str_route_dt;
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
            string p_str_batch_id, string p_str_status, string p_str_shipdtFm, string p_str_shipdtTo, string type, string p_str_Style, string p_str_color, string p_str_size, string p_str_ship_doc_id)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string lstrEmailMsg = string.Empty;
            string lstrEmailSubject = string.Empty;
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
            Folderpath = System.Configuration.ConfigurationManager.AppSettings["tempFilepath"].ToString().Trim();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            EmailAlertHdr objEmailAlertHdr = new EmailAlertHdr();
            clsRptEmail objRptEmail = new clsRptEmail();
            string l_str_email_message = string.Empty;
            bool lblnRptEmailExists = false;
            objCompany.cmp_id = p_str_cmpid;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            l_str_tmp_name = objCompany.LstCmpName[0].cmp_name.ToString().Trim();
            objCustMaster.cust_id = p_str_cmpid;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            string lstrModuleName = string.Empty;
            lstrModuleName = "OUTBOUND";
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }
            objEmail.CmpId = p_str_cmpid;
            objEmail.screenId = ScreenID;
            objEmail.username = objCompany.user_id;
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "OutboundSummary")
                    {

                        strReportName = "rpt_iv_ship_req_GridSummary.rpt";


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
                        lstrEmailSubject = objOutboundInq.cmp_id + "-OB SR Summary Report ";
                        l_str_email_message = "Please find the attached SR Summary Report";

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        if (type == "PDF")
                        {

                            IList<OutboundInq> rptSource = objOutboundInq.LstOutboundInqSummaryRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    rd.SetDataSource(rptSource);
                                    objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                    reportFileName = p_str_cmpid + "-OB-SUMMARY-" + strDateFormat + ".pdf";
                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + reportFileName;
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                    //CR2018-03-07-001 Added By Nithya
                                    Session["RptFileName"] = reportFileName;
                                }
                            }

                        }
                        else if (type == "XLS")
                        {

                            IList<OutboundInq> rptSource = objOutboundInq.LstOutboundInqSummaryRpt.ToList();
                            if (rptSource.Count > 0)
                            {

                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);

                                    rd.SetDataSource(rptSource);
                                    objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim();
                                    rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                    reportFileName = p_str_cmpid + "-OB-SUMMARY-" + strDateFormat + ".xls";
                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + reportFileName;
                                    rd.ExportToDisk(ExportFormatType.ExcelWorkbook, strFileName);

                                    Session["RptFileName"] = reportFileName;
                                }
                            }
                        }
                    }
                    else if (l_str_rpt_selection == "PickTickByStyle")
                    {
                        string RptResult = string.Empty;
                        strReportName = "rpt_iv_pickslip_consolidated_by_style.rpt";


                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmpid.Trim();
                        objOutboundInq.so_numFm = p_str_so_num_frm.Trim();
                        objOutboundInq.so_numTo = p_str_so_num_To.Trim();
                        objOutboundInq.quote_num = p_str_batch_id.Trim();
                        batchId = objOutboundInq.quote_num;
                        objOutboundInq = ServiceObject.OutboundInqPickStyleRpt(objOutboundInq);
                        lstrEmailSubject = objOutboundInq.cmp_id + "- OB Pick ticket by Style Report ";
                        l_str_email_message = "Please find the attached Pick ticket by Style Report";
                        if (type == "PDF")
                        {

                            IList<OutboundInq> rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    rd.SetDataSource(rptSource);
                                    objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim();
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
                                    reportFileName = p_str_cmpid + "-OB-PICK-TKT-BY-STYLE" + strDateFormat + ".pdf";
                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + reportFileName;
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);

                                    Session["RptFileName"] = strFileName;
                                }
                            }

                        }

                        else if (type == "XLS")
                        {

                            IList<OutboundInq> rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
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
                                    reportFileName = p_str_cmpid + "-OB-PICK-TKT-BY-STYLE" + strDateFormat + ".xls";
                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + reportFileName;
                                    rd.ExportToDisk(ExportFormatType.ExcelWorkbook, strFileName);

                                    Session["RptFileName"] = strFileName;
                                }
                            }
                        }
                    }
                    else if (l_str_rpt_selection == "PickTickByLoc")
                    {
                        strReportName = "rpt_iv_pickslip_consolidated_by_loc.rpt";
                        objOutboundInq.cmp_id = p_str_cmpid.Trim();
                        objOutboundInq.so_numFm = p_str_so_num_frm.Trim();
                        objOutboundInq.so_numTo = p_str_so_num_To.Trim();
                        objOutboundInq.quote_num = p_str_batch_id.Trim();
                        batchId = objOutboundInq.quote_num;
                        objOutboundInq = ServiceObject.OutboundInqPickLocationRpt(objOutboundInq);

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        lstrEmailSubject = objOutboundInq.cmp_id + "- OB Pick ticket by Location Report ";
                        l_str_email_message = "Please find the attached Pick ticket by Location Report";

                        if (type == "PDF")
                        {

                            IList<OutboundInq> rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    rd.SetDataSource(rptSource);
                                    objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
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
                                    reportFileName = p_str_cmpid + "-OB-PICK-TKT-BY-LOCATION-" + strDateFormat + ".pdf";
                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + reportFileName;
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);

                                    Session["RptFileName"] = strFileName;
                                }
                            }
                        }

                        else
                        if (type == "XLS")
                        {
                            IList<OutboundInq> rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
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
                                    reportFileName = p_str_cmpid + "-OB-PICK-TKT-BY-LOCATION-" + strDateFormat + ".xls";
                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + reportFileName;
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                    Session["RptFileName"] = strFileName;
                                }
                            }
                        }

                    }
                    else if (l_str_rpt_selection == "GridSummary")
                    {
                        strReportName = "rpt_iv_ob_grd_Summary.rpt";

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
                        lstrEmailSubject = objOutboundInq.cmp_id + "- OB Grid Summary Report ";
                        l_str_email_message = "Please find the Attached Grid Summary Report";
                        if (type == "PDF")
                        {
                            IList<OutboundInq> rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                    reportFileName = p_str_cmpid + "-OB-GRID-SUMMARY-REPORT-" + strDateFormat + ".pdf";
                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + reportFileName;
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);

                                    Session["RptFileName"] = strFileName;
                                }
                            }
                        }

                        else if (type == "XLS")
                        {
                            IList<OutboundInq> rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                    reportFileName = "OB-GRID-SUMMARY-" + strDateFormat + ".xls";
                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + reportFileName;
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                    //CR2018-03-07-001 Added By Nithya
                                    Session["RptFileName"] = strFileName;
                                }
                            }
                        }


                    }
                    else if (l_str_rpt_selection == "SR940Summary")
                    {
                        strReportName = "ShipReq940.rpt";

                        lstrEmailSubject = objOutboundInq.cmp_id + "- OB Summary Report ";
                        l_str_email_message = "Please find the attached OB Summary Report";
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//EDI//" + strReportName;

                        if (type == "PDF")
                        {
                            OB940UploadFile objOB940UploadFile = new OB940UploadFile();
                            OB940UploadFileService ServiceOB940UploadFile = new OB940UploadFileService();
                            objOB940UploadFile.cmp_id = p_str_cmpid.Trim();
                            objOB940UploadFile.file_name = string.Empty;
                            if (p_str_so_dt_frm != null)
                            {
                                if ((p_str_so_dt_frm.Length > 0) && (p_str_so_dt_frm != "undefined")) objOB940UploadFile.upload_dt_from = p_str_so_dt_frm.Trim();
                            }
                            if (p_str_so_dt_frm != null)
                            {
                                if ((p_str_so_dt_to.Length > 0) && (p_str_so_dt_to != "undefined")) objOB940UploadFile.upload_dt_to = p_str_so_dt_to.Trim();
                            }
                            objOB940UploadFile.batch_id = p_str_batch_id.Trim();
                            objOB940UploadFile = ServiceOB940UploadFile.GetOB940UploadDtlRptData(objOB940UploadFile, string.Empty, SelectedID);

                            if (objOB940UploadFile.ListOB940UploadDtlRpt.Count == 0)
                            {

                            }
                            else
                            {
                                int AlocCount = 0;
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    var rptSource1 = objOB940UploadFile.ListOB940UploadDtlRpt.ToList();
                                    if (rptSource1.Count > 0)
                                    {
                                        rd.Load(strRptPath);

                                        AlocCount = objOB940UploadFile.ListOB940UploadDtlRpt.Count();
                                        objCompany.cmp_id = p_str_cmpid.Trim();
                                        objCompany = ServiceObjectCompany.CompanyAddresHdrDtls(objCompany);
                                        objOB940UploadFile.ListCompanyAddresHdrDtls = objCompany.ListCompanyAddresHdrDtls;
                                        rd.SetDataSource(rptSource1);
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



                                        if (AlocCount > 0)
                                        {

                                            // l_str_rptdtl = objOB940UploadFile.cmp_id + "-" + "SR940Summary Report" + "_" + objOutboundInq.so_num;
                                            objEmail.EmailSubject = objOB940UploadFile.cmp_id + "-" + " " + " " + "SR940Summary Report";
                                            objEmail.EmailMessage = "Hi All," + "\n" + "Please find the attached 940 Summary Report for the Batch Id " + p_str_batch_id;


                                            objOB940UploadFile.cust_logo_path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                            rd.SetParameterValue("fml_image_path", objOB940UploadFile.cust_logo_path);
                                            rd.SetParameterValue("fmlReportTitle", "940 Ship Request Upload");
                                            rd.DataDefinition.FormulaFields["fmlCompanyName"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].cmp_name.ToString().Trim() + "'";
                                            rd.DataDefinition.FormulaFields["fmlCompAddress"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].addr_line1.ToString().Trim() + "'";
                                            rd.DataDefinition.FormulaFields["fmlCompCity"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].city.ToString().Trim() + "'";
                                            rd.DataDefinition.FormulaFields["fmlCompstate_id"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].state_id.ToString().Trim() + "'";
                                            rd.DataDefinition.FormulaFields["fmlCompPhone"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].tel.ToString().Trim() + "'";
                                            rd.DataDefinition.FormulaFields["fmlCompFax"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].fax.ToString().Trim() + "'";
                                            rd.DataDefinition.FormulaFields["fmlCompPostCode"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].post_code.ToString().Trim() + "'";
                                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                            reportFileName = objOB940UploadFile.cmp_id + "-SR940SUMMARY-" + strDateFormat + ".pdf";
                                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + reportFileName;
                                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);

                                            Session["RptFileName"] = strFileName;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            string tempFileName = string.Empty;
                            string l_str_file_name = string.Empty;
                            string l_str_so_num = SelectedID;
                            string l_str_process_id = string.Empty;

                            string l_str_quote_num = string.Empty;
                            string l_str_ordr_num = string.Empty;
                            string l_str_store_id = string.Empty;
                            string l_str_ship_dt = string.Empty;
                            string l_str_cust_ordr_num = string.Empty;
                            string l_str_shipto_id = string.Empty;
                            string l_str_dept_id = string.Empty;
                            string l_str_cancel_dt = string.Empty;
                            string l_str_cust_name = string.Empty;
                            string l_str_dc_id = string.Empty;
                            string l_str_st_mail_name = string.Empty;
                            string l_str_so_dt_frm = string.Empty;
                            string l_str_so_dt_to = string.Empty;

                            DataTable dtBill = new DataTable();
                            DateTime dt_ship;
                            DateTime dt_can;

                            OB940UploadFile objOB940UploadFile = new OB940UploadFile();
                            OB940UploadFileService ServiceOB940UploadFile = new OB940UploadFileService();
                            dtBill = ServiceOB940UploadFile.GetOB940UploadDtlRptDataExcel(p_str_cmpid, l_str_file_name, p_str_batch_id, l_str_so_dt_frm, l_str_so_dt_to, l_str_process_id, l_str_so_num);

                            if (!Directory.Exists(strOutputpath))
                            {
                                Directory.CreateDirectory(strOutputpath);
                            }

                            l_str_file_name = "DF_" + p_str_cmpid.ToUpper().ToString().Trim() + "_OB_SR940_SUMMARY_" + strDateFormat + ".xlsx";

                            tempFileName = strOutputpath + l_str_file_name;

                            if (System.IO.File.Exists(tempFileName))
                                System.IO.File.Delete(tempFileName);
                            xls_OB_SR940_Summary mxcel1 = new xls_OB_SR940_Summary(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "OB_SR940_SUMMARY-BY-BATCH.xlsx");

                            var dataRows = dtBill.Rows;
                            DataRow dr = dataRows[0];

                            l_str_quote_num = dr["quote_num"].ToString();
                            l_str_ordr_num = dr["ordr_num"].ToString();
                            l_str_store_id = dr["store_id"].ToString();
                            dt_ship = DateTime.Parse(dr["ship_dt"].ToString());
                            l_str_ship_dt = dt_ship.ToString("MM/dd/yyyy");

                            l_str_so_num = dr["so_num"].ToString();
                            l_str_cust_ordr_num = dr["cust_ordr_num"].ToString();
                            l_str_dept_id = dr["dept_id"].ToString();
                            dt_can = DateTime.Parse(dr["ship_dt"].ToString());
                            l_str_cancel_dt = dt_can.ToString("MM/dd/yyyy");
                            l_str_cust_name = dr["cust_name"].ToString();
                            l_str_dc_id = dr["dc_id"].ToString();
                            l_str_st_mail_name = dr["st_mail_name"].ToString();
                            int l_itn_tot_ctns = 0;
                            decimal l_dec_totcube = 0;
                            decimal l_dec_totwgt = 0;
                            int l_int_tot_orders = 0;

                            mxcel1.PopulateHeader(p_str_cmpid);
                            mxcel1.PopulateData(dtBill, ref l_int_tot_orders, ref l_itn_tot_ctns, ref l_dec_totcube, ref l_dec_totwgt);

                            mxcel1.SaveAs(tempFileName);
                            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                            Session["RptFileName"] = l_str_file_name;
                            reportFileName = l_str_file_name;

                            objEmail = new Email();
                            objEmailService = new EmailService();
                            objEmail.CmpId = p_str_cmpid;
                            objEmail.screenId = "OB Acknowledgement";
                            objEmail.username = Session["UserID"].ToString().Trim();
                            objEmail.Reportselection = "SR940Summary";
                            objEmail = objEmailService.GetSendMailDetails(objEmail);
                            if (objEmail.ListEamilDetail.Count != 0)
                            {

                                objEmail.Attachment = l_str_file_name;
                                objEmail.EmailTo = (objEmail.ListEamilDetail[0].EmailTo.Trim() == null || objEmail.ListEamilDetail[0].EmailTo.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailTo.Trim();
                                objEmail.EmailCC = (objEmail.ListEamilDetail[0].EmailCC.Trim() == null || objEmail.ListEamilDetail[0].EmailCC.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailCC.Trim();

                            }
                            else
                            {
                                objEmail.Attachment = l_str_file_name;
                                objEmail.EmailTo = "";
                                objEmail.EmailCC = "";
                            }
                            objEmail.EmailSubject = p_str_cmpid + "-" + "OB 940 Summary Report ";


                            l_str_email_message = "Hi All, " + "\n" + " Please find the attached OB SUmmary Report" + "\n\n";
                            l_str_email_message = l_str_email_message + " CmpId : " + p_str_cmpid + "\n";
                            l_str_email_message = l_str_email_message + " Total Orders# : " + l_int_tot_orders.ToString() + "\n" + " Total Ctn(s)# : " + l_itn_tot_ctns.ToString() + "\n";
                            l_str_email_message = l_str_email_message + " Total Cube# : " + l_dec_totcube.ToString() + "\n" + " Total Weight# : " + l_dec_totwgt.ToString() + "\n";
                            objEmail.EmailMessage = l_str_email_message;
                            string l_str_email_regards = string.Empty;
                            string l_str_email_footer1 = string.Empty;
                            string l_str_email_footer2 = string.Empty;
                            try
                            {
                                l_str_email_regards = System.Configuration.ConfigurationManager.AppSettings["EmailRegards"].ToString().Trim();
                                l_str_email_footer1 = System.Configuration.ConfigurationManager.AppSettings["EmailFooter1"].ToString().Trim();
                                l_str_email_footer2 = System.Configuration.ConfigurationManager.AppSettings["EmailFooter2"].ToString().Trim();
                            }
                            catch (Exception ex)
                            {
                                l_str_email_regards = "3PL WAREHOUSE";
                                l_str_email_footer1 = "Thank you for your business.";
                                l_str_email_footer2 = "Please Do not reply to this alert mail, the mail box is not monitored. If any question or help, please contact the CSR";
                            }

                            objEmail.EmailMessage = objEmail.EmailMessage + "\n" + "\n" + l_str_email_footer1;
                            objEmail.EmailMessage = objEmail.EmailMessage + "\n" + "\n" + "Regards,";
                            objEmail.EmailMessage = objEmail.EmailMessage + "\n" + l_str_email_regards;
                            objEmail.EmailMessage = objEmail.EmailMessage + "\n" + "\n" + l_str_email_footer2;

                            Mapper.CreateMap<Email, EmailModel>();
                            EmailModel EmailModel1 = Mapper.Map<Email, EmailModel>(objEmail);
                            return PartialView("_Email", EmailModel1);
                        }
                    }
                    else if (l_str_rpt_selection == "BillofLadding")
                    {
                        strReportName = "rpt_iv_bill_of_lading.rpt";

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
                        objOutboundShipInq.cmp_id = p_str_cmpid.Trim();
                        objOutboundShipInq.ship_doc_id = p_str_ship_doc_id.Trim();
                        objOutboundShipInq = OutboundShipInqServiceObject.OutboundShipInqBillofLaddingRpt(objOutboundShipInq);
                        if (objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt.Count > 0)
                        {
                            objOutboundShipInq.so_num = (objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].so_num == null || objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].so_num.Trim() == "" ? string.Empty : objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].so_num.Trim());
                            objOutboundShipInq.shipdate = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt[0].Bol_ShipDt.ToString("dd/MM/yyyy");
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
                                        // rd.ExportToDisk(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        reportFileName = l_str_rptdtl + "_" + DateTime.Now.ToFileTime() + ".pdf";
                                        Session["RptFileName"] = strFileName;
                                    }
                                }
                            }

                        }
                    }

                    else if (l_str_rpt_selection == "GridShipAck")
                    {
                        l_str_rpt_so_num = SelectedID;
                        strReportName = "rpt_iv_ship_request_Ack.rpt";


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
                                l_str_rptdtl = objOutboundInq.cmp_id + "_" + " SR OB Ack" + "_" + objOutboundInq.so_num;
                                objEmail.EmailSubject = objOutboundInq.cmp_id + "-" + " " + " " + "SR OB Ack" + "|" + " " + " " + "SR#: " + " " + objOutboundInq.so_num + "|" + " " + " " + "SR Date : " + objOutboundInq.ship_dt + "|" + " " + " " + "Store ID: " + objOutboundInq.store_id;
                                objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objOutboundInq.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "SR#: " + " " + " " + objOutboundInq.so_num + "\n" + "SR Date: " + " " + " " + objOutboundInq.ship_dt + "\n" + "StoreID: " + objOutboundInq.store_id + "\n" + "Total Cartons Requested: " + objOutboundInq.totalctns + " " + "Ctns" + "\n" + " Total Cube: " + objOutboundInq.totalcube + " " + "Lbs";
                                lstrEmailMsg = objEmail.EmailMessage.Replace("Hi All,", "");
                                lstrEmailSubject = objEmail.EmailSubject;
                            }

                            else if ((objOutboundInq.CustName == "" || objOutboundInq.CustName == null || objOutboundInq.CustName == "-"))

                            {
                                l_str_rptdtl = objOutboundInq.cmp_id + "_" + "OB Ack" + "_" + objOutboundInq.so_num;
                                objEmail.EmailSubject = objOutboundInq.cmp_id + "-" + " " + " " + "SR OB Ack" + "|" + " " + " " + "SR#: " + objOutboundInq.so_num + "|" + " " + " " + "SR Date: " + objOutboundInq.ship_dt + "|" + " " + " " + "Cust PO#: " + objOutboundInq.po_num + "|" + " " + " " + "Store ID: " + objOutboundInq.store_id;
                                objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objOutboundInq.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "SR#: " + " " + " " + objOutboundInq.so_num + "\n" + "SR Date: " + " " + " " + objOutboundInq.ship_dt + "\n" + "Cust PO#: " + objOutboundInq.po_num + "\n" + "StoreID: " + objOutboundInq.store_id + "\n" + "Ref#- " + "\n" + "Total Cartons Requested: " + objOutboundInq.totalctns + " " + "Ctns" + "\n" + " Total Cube: " + objOutboundInq.totalcube + " " + "Lbs";
                                lstrEmailMsg = objEmail.EmailMessage;
                                lstrEmailSubject = objEmail.EmailSubject;
                            }
                            else
                            {
                                l_str_rptdtl = objOutboundInq.cmp_id + "_" + "OB Ack" + "_" + objOutboundInq.so_num;
                                objEmail.EmailSubject = objOutboundInq.cmp_id + "-" + " " + " " + "SR OB Ack" + "|" + " " + " " + "SR#: " + objOutboundInq.so_num + "|" + " " + " " + "SR Date: " + objOutboundInq.ship_dt + "|" + " " + " " + "Cust PO#: " + objOutboundInq.po_num + "|" + " " + " " + "Store: " + objOutboundInq.store_id + "|" + " " + " " + "Ref# -Cust Name : " + objOutboundInq.cust_name;
                                objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objOutboundInq.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "SR#: " + " " + " " + objOutboundInq.so_num + "\n" + "SR Date: " + " " + " " + objOutboundInq.ship_dt + "\n" + "Cust PO#: " + objOutboundInq.po_num + "\n" + "StoreID: " + objOutboundInq.store_id + "\n" + "Ref#- " + "\n" + "CustName : " + objOutboundInq.cust_name + "\n" + "Total Cartons Requested: " + objOutboundInq.totalctns + " " + "Ctns" + "\n" + " Total Cube: " + objOutboundInq.totalcube + " " + "Lbs";
                                lstrEmailMsg = objEmail.EmailMessage;
                                lstrEmailSubject = objEmail.EmailSubject;
                            }

                            if (type == "PDF")
                            {
                                var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                                if (rptSource.Count > 0)
                                {
                                    using (ReportDocument rd = new ReportDocument())
                                    {
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                        AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);
                                        objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                        //strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
                                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                        reportFileName = l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                        Session["RptFileName"] = strFileName;
                                    }
                                }
                            }

                            else
                            if (type == "XLS")
                            {
                                string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                                strDateFormat = strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                string tempFileName = string.Empty;
                                string l_str_file_name = string.Empty;
                                string l_str_ordr_num = string.Empty;
                                string l_str_so_num = string.Empty;
                                string l_str_So_dt = string.Empty;
                                string l_str_cust_name = string.Empty;
                                string l_str_canceldate = string.Empty;
                                string l_str_shipdate = string.Empty;
                                string l_str_quote_num = string.Empty;
                                string l_str_shipto_id = string.Empty;
                                string l_str_cust_ordr_num = string.Empty;

                                DataTable dtBill = new DataTable();
                                var dataRows = dtBill.Rows;

                                dtBill = ServiceObject.OutboundInqShipAckExcel(p_str_cmpid, SelectedID);

                                if (!Directory.Exists(strOutputpath))
                                {
                                    Directory.CreateDirectory(strOutputpath);
                                }

                                l_str_file_name = p_str_cmpid.ToUpper().ToString().Trim() + "-OB-SR-ACK-" + strDateFormat + ".xlsx";

                                tempFileName = strOutputpath + l_str_file_name;

                                if (System.IO.File.Exists(tempFileName))
                                    System.IO.File.Delete(tempFileName);
                                xls_OB_SR_ACK_Excel mxcel1 = new xls_OB_SR_ACK_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "OB_SR_ACK.xlsx");

                                DataRow dr = dtBill.Rows[0];
                                l_str_ordr_num = dr["ordr_num"].ToString();
                                l_str_so_num = dr["so_num"].ToString();
                                l_str_So_dt = dr["So_dt"].ToString();
                                l_str_cust_name = dr["cust_name"].ToString();
                                l_str_canceldate = dr["canceldate"].ToString();
                                l_str_shipdate = dr["shipdate"].ToString();
                                l_str_quote_num = dr["quote_num"].ToString();
                                l_str_shipto_id = dr["shipto_id"].ToString();
                                l_str_cust_ordr_num = dr["cust_ordr_num"].ToString();

                                mxcel1.PopulateHeader(p_str_cmpid, l_str_ordr_num, l_str_so_num, l_str_So_dt, l_str_cust_name, l_str_canceldate, l_str_shipdate, l_str_quote_num, l_str_shipto_id, l_str_cust_ordr_num);
                                mxcel1.PopulateData(dtBill, true);
                                mxcel1.SaveAs(tempFileName);

                                objRptEmail.getEmailAlertDetails(objEmailAlertHdr, p_str_cmpid, "OUTBOUND", "OB-940-ACK", ref lblnRptEmailExists);

                                if (lblnRptEmailExists == false)
                                {
                                    l_str_email_message = "Hi All, " + "\n\n" + " Please find the attached OB 940 Acknowledgement Report" + "\n\n";
                                }

                                else
                                {
                                    l_str_email_message = l_str_email_message + objEmail.EmailMessage + "\n\n";
                                }

                                objEmailAlertHdr.emailMessage = l_str_email_message;
                                objEmailAlertHdr.emailMessage = objEmailAlertHdr.emailMessage + "\n" + objEmailAlertHdr.emailFooter + "\n";
                                objEmailAlertHdr.filePath = strOutputpath;
                                objEmailAlertHdr.fileName = l_str_file_name;
                                objEmailAlertHdr.emailSubject = lstrEmailSubject;
                                EmailAlert objEmailAlert = new EmailAlert();
                                objEmailAlertHdr.cmpId = p_str_cmpid;
                                objEmailAlert.objEmailAlertHdr = objEmailAlertHdr;

                                Mapper.CreateMap<EmailAlert, EmailAlertModel>();
                                EmailAlertModel EmailAlertModel = Mapper.Map<EmailAlert, EmailAlertModel>(objEmailAlert);
                                return PartialView("_EmailAlert", EmailAlertModel);



                            }
                        }
                    }
                    else
                    {
                        Response.Write("<H2>Report not found</H2>");
                    }


                    //clsRptEmail objRptEmail = new clsRptEmail();
                    //bool lblnRptEmailExists = false;

                    string lstrReportId = string.Empty;
                    if (l_str_rpt_selection == "GridShipAck")
                    {
                        l_str_rpt_selection = "OB-940-ACK";
                    }
                    else
                    {
                        l_str_rpt_selection = string.Empty;
                    }

                    objRptEmail.getEmailAlertDetails(objEmailAlertHdr, p_str_cmpid, lstrModuleName, l_str_rpt_selection, ref lblnRptEmailExists);

                    if (lblnRptEmailExists == false)
                    {
                        if (l_str_email_message.Length == 0)
                        {
                            l_str_email_message = "Hi All, " + "\n" + " Please find the attached OB Report" + "\n\n";
                        }
                        else
                        {
                            l_str_email_message = "Hi All, " + "\n" + l_str_email_message;
                        }
                    }

                    else
                    {
                        if (l_str_email_message.Trim().Length == 0)
                        {
                            l_str_email_message = l_str_email_message + objEmail.EmailMessage + "\n";
                        }
                        else
                        {
                            l_str_email_message = "Hi All, " + "\n" + l_str_email_message;
                        }

                    }

                    objEmailAlertHdr.emailMessage = l_str_email_message;
                    objEmailAlertHdr.emailMessage = objEmailAlertHdr.emailMessage + "\n" + objEmailAlertHdr.emailFooter + "\n";
                    string strOutputTempPath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                    objEmailAlertHdr.filePath = strOutputTempPath;
                    objEmailAlertHdr.fileName = reportFileName;
                    objEmailAlertHdr.emailSubject = lstrEmailSubject;
                    EmailAlert objEmailAlert1 = new EmailAlert();
                    objEmailAlertHdr.cmpId = p_str_cmpid;
                    objEmailAlert1.objEmailAlertHdr = objEmailAlertHdr;

                    Mapper.CreateMap<EmailAlert, EmailAlertModel>();
                    EmailAlertModel EmailAlert1Model = Mapper.Map<EmailAlert, EmailAlertModel>(objEmailAlert1);
                    return PartialView("_EmailAlert", EmailAlert1Model);

                    //objEmail.CmpId = p_str_cmpid;
                    //objEmail.screenId = ScreenID;
                    //objEmail.username = objCompany.user_id;
                    //objEmail.Reportselection = l_str_rpt_selection;
                    //objEmail = objEmailService.GetSendMailDetails(objEmail);
                    //if (objEmail.ListEamilDetail.Count != 0)
                    //{

                    //    objEmail.Attachment = reportFileName;
                    //    objEmail.EmailTo = (objEmail.ListEamilDetail[0].EmailTo.Trim() == null || objEmail.ListEamilDetail[0].EmailTo.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailTo.Trim();
                    //    objEmail.EmailCC = (objEmail.ListEamilDetail[0].EmailCC.Trim() == null || objEmail.ListEamilDetail[0].EmailCC.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailCC.Trim();

                    //}
                    //else
                    //{
                    //    objEmail.Attachment = reportFileName;
                    //    objEmail.EmailTo = "";
                    //    objEmail.EmailCC = "";
                    //}
                    //Mapper.CreateMap<Email, EmailModel>();
                    //EmailModel EmailModel = Mapper.Map<Email, EmailModel>(objEmail);
                    //return PartialView("_Email", EmailModel);

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
       string p_str_batch_id, string p_str_status, string p_str_shipdtFm, string p_str_shipdtTo, string type, string p_str_Style, string p_str_color, string p_str_size)
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
                    if (l_str_rpt_selection == "OutboundSummary")
                    {

                        strReportName = "rpt_iv_ship_req_GridSummary.rpt";
                        OutboundInq objOutboundInq = new OutboundInq();
                        OutboundInqService ServiceObject = new OutboundInqService();

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
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objOutboundInq.LstOutboundInqSummaryRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }

                        else
                        if (type == "Excel")
                        {
                            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                            string strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
                            string tempFileName = string.Empty;
                            string l_str_file_name = string.Empty;

                            DataTable dtBill = new DataTable();

                            dtBill = ServiceObject.OutboundInqSummaryExcel(p_str_cmpid, p_str_so_num_frm, p_str_so_num_To, p_str_so_dt_frm, p_str_so_dt_to, p_str_CustPO, p_str_AlocId, p_str_status, p_str_Store, p_str_batch_id, p_str_shipdtFm, p_str_shipdtTo, p_str_Style, p_str_color, p_str_size);

                            if (!Directory.Exists(strOutputpath))
                            {
                                Directory.CreateDirectory(strOutputpath);
                            }

                            l_str_file_name = "DF_" + p_str_cmpid.ToUpper().ToString().Trim() + "_OB_SR_SUMMARY_" + strDateFormat + ".xlsx";

                            tempFileName = strOutputpath + l_str_file_name;

                            if (System.IO.File.Exists(tempFileName))
                                System.IO.File.Delete(tempFileName);
                            xls_OB_SR_Summary_Excel mxcel = new xls_OB_SR_Summary_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "OB_SR_SUMMARY.xlsx");

                            mxcel.PopulateHeader(p_str_cmpid);
                            mxcel.PopulateData(dtBill, true);
                            mxcel.SaveAs(tempFileName);
                            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                            return File(fs, "application / xlsx", l_str_file_name);
                            /*
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
                        */


                        }
                    }
                    else if (l_str_rpt_selection == "PickTickByStyle")
                    {
                        string RptResult = string.Empty;
                        strReportName = "rpt_iv_pickslip_consolidated_by_style.rpt";
                        OutboundInq objOutboundInq = new OutboundInq();
                        OutboundInqService ServiceObject = new OutboundInqService();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmpid.Trim();
                        objOutboundInq.so_numFm = p_str_so_num_frm.Trim();
                        objOutboundInq.so_numTo = p_str_so_num_To.Trim();
                        objOutboundInq.quote_num = p_str_batch_id.Trim();
                        batchId = objOutboundInq.quote_num;

                        if (type == "PDF")
                        {
                            objOutboundInq = ServiceObject.OutboundInqPickStyleRpt(objOutboundInq);
                            var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
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
                            }
                        }

                        else
                        if (type == "Excel")
                        {

                            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                            string strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
                            string tempFileName = string.Empty;
                            string l_str_file_name = string.Empty;
                            string l_str_batch_id = string.Empty;

                            DataTable dtBill = new DataTable();


                            dtBill = ServiceObject.OutboundInqPickByStyle(p_str_cmpid, p_str_so_num_frm, p_str_so_num_To, p_str_batch_id);

                            if (!Directory.Exists(strOutputpath))
                            {
                                Directory.CreateDirectory(strOutputpath);
                            }

                            l_str_file_name = "DF_" + p_str_cmpid.ToUpper().ToString().Trim() + "_OB_PICK_TKT_BY_STYLE_" + strDateFormat + ".xlsx";

                            tempFileName = strOutputpath + l_str_file_name;

                            if (System.IO.File.Exists(tempFileName))
                                System.IO.File.Delete(tempFileName);
                            xls_OB_Pick_Ticket_By_Style mxcel = new xls_OB_Pick_Ticket_By_Style(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "OB_PICK_TKT_STYLE.xlsx");

                            mxcel.PopulateHeader(p_str_cmpid, p_str_batch_id);
                            mxcel.PopulateData(dtBill, true);
                            mxcel.SaveAs(tempFileName);
                            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                            return File(fs, "application / xlsx", l_str_file_name);

                            /*
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


                        */
                        }
                    }
                    else if (l_str_rpt_selection == "PickTickByLoc")
                    {
                        strReportName = "rpt_iv_pickslip_consolidated_by_loc.rpt";
                        OutboundInq objOutboundInq = new OutboundInq();
                        OutboundInqService ServiceObject = new OutboundInqService();

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmpid.Trim();
                        objOutboundInq.so_numFm = p_str_so_num_frm.Trim();
                        objOutboundInq.so_numTo = p_str_so_num_To.Trim();
                        objOutboundInq.quote_num = p_str_batch_id.Trim();
                        batchId = objOutboundInq.quote_num;

                        if (type == "PDF")
                        {
                            objOutboundInq = ServiceObject.OutboundInqPickLocationRpt(objOutboundInq);
                            var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
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
                            }
                        }
                        else
                        if (type == "Excel")
                        {
                            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                            string strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
                            string tempFileName = string.Empty;
                            string l_str_file_name = string.Empty;
                            string l_str_batch_id = string.Empty;


                            DataTable dtBill = new DataTable();


                            dtBill = ServiceObject.OutboundInqPickByLocation(p_str_cmpid, p_str_so_num_frm, p_str_so_num_To, p_str_batch_id);

                            if (!Directory.Exists(strOutputpath))
                            {
                                Directory.CreateDirectory(strOutputpath);
                            }

                            l_str_file_name = "DF_" + p_str_cmpid.ToUpper().ToString().Trim() + "_OB_PICK_TKT_BY_LOCATION_" + strDateFormat + ".xlsx";

                            tempFileName = strOutputpath + l_str_file_name;

                            if (System.IO.File.Exists(tempFileName))
                                System.IO.File.Delete(tempFileName);
                            xls_OB_Pick_Ticket_By_Location mxcel = new xls_OB_Pick_Ticket_By_Location(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "OB_PICK_TKT_LOC.xlsx");

                            mxcel.PopulateHeader(p_str_cmpid, p_str_batch_id);
                            mxcel.PopulateData(dtBill, true);
                            mxcel.SaveAs(tempFileName);
                            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                            return File(fs, "application / xlsx", l_str_file_name);
                            /*
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

                        */

                        }
                    }
                    else if (l_str_rpt_selection == "GridSummary")
                    {
                        strReportName = "rpt_iv_ob_grd_Summary.rpt";
                        OutboundInq objOutboundInq = new OutboundInq();
                        OutboundInqService ServiceObject = new OutboundInqService();
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
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }

                        else
                        if (type == "Excel")
                        {
                            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                            string strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
                            string tempFileName = string.Empty;
                            string l_str_file_name = string.Empty;

                            DataTable dtBill = new DataTable();

                            dtBill = ServiceObject.OutboundInqGridSummaryExcel(p_str_cmpid, p_str_AlocId, p_str_so_num_frm, p_str_so_num_To, p_str_so_dt_frm, p_str_so_dt_to, p_str_CustPO, p_str_status, p_str_Store, p_str_batch_id, p_str_shipdtFm, p_str_shipdtTo, p_str_Style, p_str_color, p_str_size);

                            if (!Directory.Exists(strOutputpath))
                            {
                                Directory.CreateDirectory(strOutputpath);
                            }

                            l_str_file_name = "DF_" + p_str_cmpid.ToUpper().ToString().Trim() + "_OB_GRID_SUMMARY_" + strDateFormat + ".xlsx";

                            tempFileName = strOutputpath + l_str_file_name;

                            if (System.IO.File.Exists(tempFileName))
                                System.IO.File.Delete(tempFileName);
                            xls_OB_Grid_Summary mxcel = new xls_OB_Grid_Summary(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "OB_GRID_SUMMARY.xlsx");

                            mxcel.PopulateHeader(p_str_cmpid);
                            mxcel.PopulateData(dtBill, true);
                            mxcel.SaveAs(tempFileName);
                            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                            return File(fs, "application / xlsx", l_str_file_name);

                        }


                    }
                    else if (l_str_rpt_selection == "SR940Summary")
                    {

                        OB940UploadFile objOB940UploadFile = new OB940UploadFile();
                        OB940UploadFileService ServiceOB940UploadFile = new OB940UploadFileService();
                        Company objCompany = new Company();
                        CompanyService ServiceObjectCompany = new CompanyService();

                        List<eComSR940> li = new List<eComSR940>();

                        DataTable dtRpt = new DataTable();
                        DataSet dsRpt = new DataSet();
                        objCustMaster.cust_id = p_str_cmpid;
                        objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
                        if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
                        {
                            objCustMaster.ListGetCustLogo[0].cust_logo = "";
                        }

                        if (type == "PDF")
                        {
                            strReportName = "ShipReq940.rpt";

                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//EDI//" + strReportName;
                            objOB940UploadFile.cmp_id = p_str_cmpid.Trim();

                            objOB940UploadFile.file_name = string.Empty;
                            if ((p_str_so_dt_frm.Length > 0) && (p_str_so_dt_frm != "undefined")) objOB940UploadFile.upload_dt_from = p_str_so_dt_frm.Trim();
                            if ((p_str_so_dt_to.Length > 0) && (p_str_so_dt_to != "undefined")) objOB940UploadFile.upload_dt_to = p_str_so_dt_to.Trim();
                            string l_str_so_num = string.Empty;
                            if ((SelectedID.Length > 0) && (SelectedID != "undefined")) l_str_so_num = SelectedID;

                            objOB940UploadFile.batch_id = p_str_batch_id.Trim();


                            objOB940UploadFile = ServiceOB940UploadFile.GetOB940UploadDtlRptData(objOB940UploadFile, string.Empty, l_str_so_num);

                            if (objOB940UploadFile.ListOB940UploadDtlRpt.Count == 0)
                            {

                            }
                            else
                            {
                                var rptSource1 = objOB940UploadFile.ListOB940UploadDtlRpt.ToList();
                                if (rptSource1.Count > 0)
                                {
                                    using (ReportDocument rd = new ReportDocument())
                                    {
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                        AlocCount = objOB940UploadFile.ListOB940UploadDtlRpt.Count();
                                        objCompany.cmp_id = p_str_cmpid.Trim();
                                        objCompany = ServiceObjectCompany.CompanyAddresHdrDtls(objCompany);
                                        objOB940UploadFile.ListCompanyAddresHdrDtls = objCompany.ListCompanyAddresHdrDtls;
                                        if (rptSource1 != null && rptSource1.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource1);
                                        objOB940UploadFile.cust_logo_path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objOB940UploadFile.cust_logo_path);
                                        rd.SetParameterValue("fmlReportTitle", "940 Ship Request Upload");
                                        rd.DataDefinition.FormulaFields["fmlCompanyName"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].cmp_name.ToString().Trim() + "'";
                                        rd.DataDefinition.FormulaFields["fmlCompAddress"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].addr_line1.ToString().Trim() + "'";
                                        rd.DataDefinition.FormulaFields["fmlCompCity"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].city.ToString().Trim() + "'";
                                        rd.DataDefinition.FormulaFields["fmlCompstate_id"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].state_id.ToString().Trim() + "'";
                                        rd.DataDefinition.FormulaFields["fmlCompPhone"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].tel.ToString().Trim() + "'";
                                        rd.DataDefinition.FormulaFields["fmlCompFax"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].fax.ToString().Trim() + "'";
                                        rd.DataDefinition.FormulaFields["fmlCompPostCode"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].post_code.ToString().Trim() + "'";
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "940ShipRequestUpload");
                                    }
                                }
                            }


                        }
                        else
                            if (type == "Excel")
                        {

                            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                            string strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
                            string tempFileName = string.Empty;
                            string l_str_file_name = string.Empty;
                            string l_str_so_num = SelectedID;
                            string l_str_process_id = string.Empty;

                            string l_str_quote_num = string.Empty;
                            string l_str_ordr_num = string.Empty;
                            string l_str_store_id = string.Empty;
                            string l_str_ship_dt = string.Empty;
                            string l_str_cust_ordr_num = string.Empty;
                            string l_str_shipto_id = string.Empty;
                            string l_str_dept_id = string.Empty;
                            string l_str_cancel_dt = string.Empty;
                            string l_str_cust_name = string.Empty;
                            string l_str_dc_id = string.Empty;
                            string l_str_st_mail_name = string.Empty;
                            string l_str_so_dt_frm = string.Empty;
                            string l_str_so_dt_to = string.Empty;

                            DataTable dtBill = new DataTable();
                            DateTime dt_ship;
                            DateTime dt_can;


                            dtBill = ServiceOB940UploadFile.GetOB940UploadDtlRptDataExcel(p_str_cmpid, l_str_file_name, p_str_batch_id, l_str_so_dt_frm, l_str_so_dt_to, l_str_process_id, l_str_so_num);

                            if (!Directory.Exists(strOutputpath))
                            {
                                Directory.CreateDirectory(strOutputpath);
                            }

                            l_str_file_name = "DF_" + p_str_cmpid.ToUpper().ToString().Trim() + "_OB_SR940_SUMMARY_" + strDateFormat + ".xlsx";

                            tempFileName = strOutputpath + l_str_file_name;

                            if (System.IO.File.Exists(tempFileName))
                                System.IO.File.Delete(tempFileName);
                            xls_OB_SR940_Summary mxcel1 = new xls_OB_SR940_Summary(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "OB_SR940_SUMMARY-BY-BATCH.xlsx");

                            var dataRows = dtBill.Rows;
                            DataRow dr = dataRows[0];

                            l_str_quote_num = dr["quote_num"].ToString();
                            l_str_ordr_num = dr["ordr_num"].ToString();
                            l_str_store_id = dr["store_id"].ToString();
                            dt_ship = DateTime.Parse(dr["ship_dt"].ToString());
                            l_str_ship_dt = dt_ship.ToString("MM/dd/yyyy");

                            l_str_so_num = dr["so_num"].ToString();
                            l_str_cust_ordr_num = dr["cust_ordr_num"].ToString();
                            l_str_dept_id = dr["dept_id"].ToString();
                            dt_can = DateTime.Parse(dr["ship_dt"].ToString());
                            l_str_cancel_dt = dt_can.ToString("MM/dd/yyyy");
                            l_str_cust_name = dr["cust_name"].ToString();
                            l_str_dc_id = dr["dc_id"].ToString();
                            l_str_st_mail_name = dr["st_mail_name"].ToString();


                            //mxcel1.PopulateHeader(p_str_cmpid, l_str_ordr_num, l_str_quote_num, l_str_store_id, l_str_ship_dt, l_str_so_num, l_str_cust_ordr_num, l_str_dept_id, l_str_cancel_dt, l_str_cust_name, l_str_dc_id, l_str_st_mail_name);
                            //mxcel1.PopulateData(dtBill, true);
                            int l_itn_tot_ctns = 0;
                            decimal l_dec_totcube = 0;
                            decimal l_dec_totwgt = 0;
                            int l_int_tot_orders = 0;

                            mxcel1.PopulateHeader(p_str_cmpid);
                            mxcel1.PopulateData(dtBill, ref l_int_tot_orders, ref l_itn_tot_ctns, ref l_dec_totcube, ref l_dec_totwgt);
                            mxcel1.SaveAs(tempFileName);
                            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                            return File(fs, "application / xlsx", l_str_file_name);

                        }

                    }
                    else if (l_str_rpt_selection == "GridShipAck")
                    {
                        l_str_rpt_so_num = SelectedID;
                        strReportName = "rpt_iv_ship_request_Ack.rpt";
                        OutboundInq objOutboundInq = new OutboundInq();
                        OutboundInqService ServiceObject = new OutboundInqService();

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmpid.Trim();
                        objOutboundInq.Sonum = SelectedID.Trim();

                        if (type == "PDF")
                        {
                            objOutboundInq = ServiceObject.OutboundInqShipAck(objOutboundInq);
                            var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }

                        }

                        else
                        if (type == "Excel")
                        {
                            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                            string strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
                            string tempFileName = string.Empty;
                            string l_str_file_name = string.Empty;
                            string l_str_ordr_num = string.Empty;
                            string l_str_so_num = string.Empty;
                            string l_str_So_dt = string.Empty;
                            string l_str_cust_name = string.Empty;
                            string l_str_canceldate = string.Empty;
                            string l_str_shipdate = string.Empty;
                            string l_str_quote_num = string.Empty;
                            string l_str_shipto_id = string.Empty;
                            string l_str_cust_ordr_num = string.Empty;

                            DataTable dtBill = new DataTable();


                            dtBill = ServiceObject.OutboundInqShipAckExcel(p_str_cmpid, SelectedID);

                            if (!Directory.Exists(strOutputpath))
                            {
                                Directory.CreateDirectory(strOutputpath);
                            }

                            l_str_file_name = "DF_" + p_str_cmpid.ToUpper().ToString().Trim() + "_OB_SR_ACK_" + strDateFormat + ".xlsx";

                            tempFileName = strOutputpath + l_str_file_name;

                            if (System.IO.File.Exists(tempFileName))
                                System.IO.File.Delete(tempFileName);
                            xls_OB_SR_ACK_Excel mxcel1 = new xls_OB_SR_ACK_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "OB_SR_ACK.xlsx");

                            var dataRows = dtBill.Rows;
                            DataRow dr = dataRows[0];

                            l_str_ordr_num = dr["ordr_num"].ToString();
                            l_str_so_num = dr["so_num"].ToString();
                            l_str_So_dt = dr["So_dt"].ToString();
                            l_str_cust_name = dr["cust_name"].ToString();
                            l_str_canceldate = dr["canceldate"].ToString();
                            l_str_shipdate = dr["shipdate"].ToString();
                            l_str_quote_num = dr["quote_num"].ToString();
                            l_str_shipto_id = dr["shipto_id"].ToString();
                            l_str_cust_ordr_num = dr["cust_ordr_num"].ToString();

                            mxcel1.PopulateHeader(p_str_cmpid, l_str_ordr_num, l_str_so_num, l_str_So_dt, l_str_cust_name, l_str_canceldate, l_str_shipdate, l_str_quote_num, l_str_shipto_id, l_str_cust_ordr_num);
                            mxcel1.PopulateData(dtBill, true);
                            mxcel1.SaveAs(tempFileName);
                            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                            return File(fs, "application / xlsx", l_str_file_name);

                            /*
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
                            */
                        }
                    }
                    //CR-2018-04-24-001 Added BY Nithya
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
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }

                        else
                        if (type == "Excel")
                        {

                            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                            string strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
                            string tempFileName = string.Empty;
                            string l_str_file_name = string.Empty;
                            string l_str_ship_doc_id = string.Empty;
                            string l_str_Bol_ShipDt = string.Empty;
                            string l_str_so_num = string.Empty;
                            string l_str_quote_num = string.Empty;
                            string l_str_cust_name = string.Empty;
                            string l_str_cust_ordr_num = string.Empty;
                            string l_str_ship_to_name = string.Empty;
                            DateTime dt;


                            DataTable dtBill = new DataTable();

                            dtBill = ServiceObject.OutboundShipInqBillofLaddingExcelTemplate(p_str_cmpid, SelectedID);

                            if (!Directory.Exists(strOutputpath))
                            {
                                Directory.CreateDirectory(strOutputpath);
                            }

                            l_str_file_name = "DF_" + p_str_cmpid.ToUpper().ToString().Trim() + "_OB_BL_OF_LADDING_" + strDateFormat + ".xlsx";

                            tempFileName = strOutputpath + l_str_file_name;

                            if (System.IO.File.Exists(tempFileName))
                                System.IO.File.Delete(tempFileName);
                            xls_OB_Bill_Of_Ladding_Excel mxcel = new xls_OB_Bill_Of_Ladding_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "OB_BL_OF_LADDING.xlsx");
                            var dataRows = dtBill.Rows;
                            DataRow dr = dataRows[0];

                            l_str_ship_doc_id = dr["ship_doc_id"].ToString();
                            dt = DateTime.Parse(dr["Bol_ShipDt"].ToString());
                            l_str_Bol_ShipDt = dt.ToString("MM/dd/yyyy");
                            l_str_so_num = dr["so_num"].ToString();
                            l_str_quote_num = dr["quote_num"].ToString();
                            l_str_cust_name = dr["cust_name"].ToString();
                            l_str_cust_ordr_num = dr["cust_ordr_num"].ToString();
                            l_str_ship_to_name = dr["ship_to_name"].ToString();


                            mxcel.PopulateHeader(p_str_cmpid, l_str_ship_doc_id, l_str_Bol_ShipDt, l_str_so_num, l_str_quote_num, l_str_cust_name, l_str_cust_ordr_num, l_str_ship_to_name);
                            mxcel.PopulateData(dtBill, true);
                            mxcel.SaveAs(tempFileName);
                            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                            return File(fs, "application / xlsx", l_str_file_name);

                            /*
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
                       */
                        }
                    }


                    else if (l_str_rpt_selection == "OBBOLConfirmation")
                    {
                        strReportName = "rpt_ob_ship_bol_conf.rpt";
                        OBGetBOLConf objOBGetBOLConf = new OBGetBOLConf();
                        OutboundInqService ServiceObject = new OutboundInqService();

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOBGetBOLConf.cmp_id = p_str_cmpid;
                        objOBGetBOLConf.ship_doc_id = SelectedID;

                        objOBGetBOLConf = ServiceObject.GetOBBOLConfDtlRptData(objOBGetBOLConf, p_str_cmpid, p_str_so_num_frm, p_str_so_num_To, p_str_shipdtFm, p_str_shipdtTo, p_str_batch_id);
                        //CR20180623-001

                        if (objOBGetBOLConf.ListOBBOLConfRpt.Count > 0)
                        {


                        }
                        else
                        {
                            msg = "No Record Found";
                            jsonErrorCode = "-2";
                            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);
                        }
                        //END
                        if (type == "PDF")
                        {
                            var rptSource = objOBGetBOLConf.ListOBBOLConfRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);

                                    rd.SetDataSource(rptSource);
                                    Company objCompany = new Company();
                                    CompanyService ServiceObjectCompany = new CompanyService();
                                    objCustMaster.cust_id = p_str_cmpid;
                                    objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
                                    if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
                                    {
                                        objCustMaster.ListGetCustLogo[0].cust_logo = "";
                                    }
                                    rd.SetParameterValue("fml_image_path",System.Web.HttpContext.Current.Server.MapPath("~/") +  objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim());
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }

                        else
                        if (type == "Excel")
                        {
                            //objOBGetBOLConf = ServiceObject.OutboundShipInqBillofLaddingExcel(objOBGetBOLConf);

                            //List<OB_SHIP_BOL_Excel> li = new List<OB_SHIP_BOL_Excel>();
                            //for (int i = 0; i < objOBGetBOLConf.LstOutboundShipInqBillofLaddingRpt.Count; i++)
                            //{

                            //    OB_SHIP_BOL_Excel objOBInquiryExcel = new OB_SHIP_BOL_Excel();
                            //    objOBInquiryExcel.LineNo = objOBGetBOLConf.LstOutboundShipInqBillofLaddingRpt[i].line_num;
                            //    objOBInquiryExcel.Style = objOBGetBOLConf.LstOutboundShipInqBillofLaddingRpt[i].itm_num;
                            //    objOBInquiryExcel.Color = objOBGetBOLConf.LstOutboundShipInqBillofLaddingRpt[i].itm_color;
                            //    objOBInquiryExcel.Size = objOBGetBOLConf.LstOutboundShipInqBillofLaddingRpt[i].itm_size;
                            //    objOBInquiryExcel.Ppk = objOBGetBOLConf.LstOutboundShipInqBillofLaddingRpt[i].ctn_qty;
                            //    objOBInquiryExcel.Ctns = objOBGetBOLConf.LstOutboundShipInqBillofLaddingRpt[i].ctn_cnt;
                            //    objOBInquiryExcel.ShipQty = objOBGetBOLConf.LstOutboundShipInqBillofLaddingRpt[i].line_qty;
                            //    objOBInquiryExcel.Uom = objOBGetBOLConf.LstOutboundShipInqBillofLaddingRpt[i].pick_uom;


                            //    li.Add(objOBInquiryExcel);
                            //}

                            //GridView gv = new GridView();
                            //gv.DataSource = li;
                            //gv.DataBind();
                            //Session["OB_BOL"] = gv;
                            //return new DownloadFileActionResult((GridView)Session["OB_BOL"], "OB_BOL" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
                        }
                    }

                    //CR20180721-001 Added By Nithya
                    else if (l_str_rpt_selection == "SRPackingSlip")
                    {
                        strReportName = "rpt_iv_packslip_tpw.rpt";
                        OutboundShipInq objOutboundShipInq = new OutboundShipInq();
                        OutboundShipInqService ServiceObject = new OutboundShipInqService();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundShipInq.cmp_id = p_str_cmpid;
                        objOutboundShipInq.ship_doc_id = SelectedID;
                        objOutboundShipInq = ServiceObject.OutboundShipInqpackSlipRpt(objOutboundShipInq);
                        //objOutboundShipInq = ServiceObject.GetTotCubesRpt(objOutboundShipInq);
                        //if (objOutboundShipInq.LstPalletCount.Count > 0)
                        //{
                        //    objOutboundShipInq.TotCube = objOutboundShipInq.LstPalletCount[0].TotCube;
                        //}
                        //else
                        //{
                        //    objOutboundShipInq.TotCube = 0;
                        //}
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
                                    objOutboundShipInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim();//CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objOutboundShipInq.Image_Path);
                                    //rd.SetParameterValue("SumOfCubes", objOutboundShipInq.TotCube);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
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
            objOutboundInq.po_num = (objOutboundInq.LstOutboundInqpickstyleRpt[0].po_num == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].po_num == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].po_num);
            objOutboundInq.pick_no = (objOutboundInq.LstOutboundInqpickstyleRpt[0].pick_no == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].pick_no == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].pick_no);
            objOutboundInq.ref_no = (objOutboundInq.LstOutboundInqpickstyleRpt[0].ref_no == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ref_no == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].ref_no);

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
            objOutboundInq.ShipDt = (objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipDt == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipDt == "" ? string.Empty : Convert.ToDateTime(objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipDt).ToString("MM/dd/yyyy"));
            objOutboundInq.CancelDt = (objOutboundInq.LstOutboundInqpickstyleRpt[0].CancelDt == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].CancelDt == "" ? string.Empty : Convert.ToDateTime(objOutboundInq.LstOutboundInqpickstyleRpt[0].CancelDt).ToString("MM/dd/yyyy"));
            objOutboundInq.fob = (objOutboundInq.LstOutboundInqpickstyleRpt[0].fob == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].fob == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].fob);
            objOutboundInq.store_id = (objOutboundInq.LstOutboundInqpickstyleRpt[0].store_id == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].store_id == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].store_id);
            objOutboundInq.DCID = (objOutboundInq.LstOutboundInqpickstyleRpt[0].DCID == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].DCID == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].DCID);
            objOutboundInq.ShipToAttn = (objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToAttn == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToAttn == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToAttn);
            objOutboundInq.ShipToEmail = (objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToEmail == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToEmail == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToEmail);
            objOutboundInq.ShipToAddr1 = (objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToAddr1 == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToAddr1 == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToAddr1);
            objOutboundInq.ShipToAddr2 = (objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToAddr2 == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToAddr2 == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToAddr2);
            objOutboundInq.ShipToCity = (objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToCity == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToCity == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToCity);
            objOutboundInq.ShipToState = (objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToState == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToState == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToState);
            objOutboundInq.ShipToZipCode = (objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToZipCode == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToZipCode == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToZipCode);
            objOutboundInq.ShipToCountry = (objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToCountry == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToCountry == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToCountry);
            objOutboundInq.note = (objOutboundInq.LstOutboundInqpickstyleRpt[0].note == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].note == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].note);
            objOutboundInq.ShipToID = (objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToID == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToID == "" ? string.Empty : objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToID);
            objOutboundInq.ListSOTracking = ServiceObject.fnGetSoTracking(cmp_id, Id, string.Empty).ListSOTracking;
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
            objCustMaster.cust_id = cmp_id;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }
            try
            {
                if (isValid)
                {
                    strReportName = "rpt_iv_ship_request_Ack.rpt";
                    OutboundInq objOutboundInq = new OutboundInq();
                    OutboundInqService ServiceObject = new OutboundInqService();

                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                    objOutboundInq.cmp_id = p_str_cmpid;
                    objOutboundInq.Sonum = p_str_ShipReq_id;
                    objOutboundInq = ServiceObject.OutboundInqShipAck(objOutboundInq);
                    var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                    if (rptSource.Count > 0)
                    {
                        using (ReportDocument rd = new ReportDocument())
                        {
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);

                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
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
            objOutboundInq.status = "O";
            Session["tempcmpid"] = objOutboundInq.cmp_id;
            Session["tempStatus"] = objOutboundInq.status;
            objOutboundInq.so_dt = DateTime.Now.ToString("MM-dd-yyyy");
            objOutboundInq.screentitle = p_str_screen_title;
            objOutboundInq.ShipDt = DateTime.Now.AddDays(1).ToString("MM-dd-yyyy");
            objOutboundInq.CancelDt = DateTime.Now.AddDays(2).ToString("MM-dd-yyyy");
            objOutboundInq.cust_id = "";
            objOutboundInq.dc_id = "1";
            objOutboundInq.ref_no = "-";
            objOutboundInq.AuthId = "-";
            objOutboundInq.ShipVia = "-";
            objOutboundInq.Note = "-";
            objOutboundInq.CustPO = "";
            objOutboundInq.shipchrg = "0";
            objOutboundInq.CustPO = "-";
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objOutboundInq.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objOutboundInq = ServiceObject.GetIbSRIdDetail(objOutboundInq);
            objOutboundInq.Sonum = objOutboundInq.SRNUm;
            l_str_ibdocid = objOutboundInq.SRNUm;
            objOutboundInq.Sonum = l_str_ibdocid;
            Session["tempSonum"] = objOutboundInq.Sonum;
            objOutboundInq.Sonum = objOutboundInq.Sonum;
            objOutboundInq.Batchid = objOutboundInq.Sonum;
            ServiceObject.DeleteTempshipEntrytable(objOutboundInq);
            string l_str_DftWhs = string.Empty;
            objOutboundInq = ServiceObject.GetDftWhs(objOutboundInq);
            if (objOutboundInq.ListPickdtl.Count > 0)
            {
                l_str_DftWhs = objOutboundInq.ListPickdtl[0].dft_whs.Trim();
            }
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
            if (objOutboundInq.ListPick.Count == 0)
            {
                l_str_DftWhs = "WHS";
                return Json(l_str_DftWhs, JsonRequestBehavior.AllowGet);
            }
            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objOutboundInq.ListCntryPick = objPick.ListCntryPick;
            objOutboundInq.country = "USA";
            objPick.Cntry_Id = objOutboundInq.country;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objOutboundInq.ListStatePick = objPick.ListStatePick;
            objOutboundInq.country = "USA";
            objPick = ServiceObjectPick.GetPickUom(objPick);
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
            objOutboundInq = ServiceObject.LoadCustDtls(objOutboundInq);
            if (objOutboundInq.LstFiledtl.Count > 0)
            {
                objOutboundInq.aloc_by = objOutboundInq.LstFiledtl[0].aloc_by;
                objOutboundInq.Stk_Chk_Reqt = objOutboundInq.LstFiledtl[0].Stk_Chk_Reqt;
                if (objOutboundInq.Stk_Chk_Reqt == null)
                {
                    objOutboundInq.Stk_Chk_Reqt = "";
                }
                else
                {
                    objOutboundInq.Stk_Chk_Reqt = objOutboundInq.Stk_Chk_Reqt.Trim();
                }
            }

            objOutboundInq = ServiceObject.LoadCustConfig(objOutboundInq);

            string lstrFormName = string.Empty;

            if (String.IsNullOrEmpty(objOutboundInq.objCustConfig.ecom_recv_by_bin) == true || objOutboundInq.objCustConfig.ecom_recv_by_bin == "0" || objOutboundInq.objCustConfig.ecom_recv_by_bin == "N")
            {
                lstrFormName = "_ShipReqEntry";
            }
            else
            {
                lstrFormName = "_EcomOrderEntry";
            }
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);



            return PartialView(lstrFormName, objOutboundInqModel);
        }

        public ActionResult GetOBSRSummaryForLoadEntry(string p_str_cmp_id, string p_str_batch_num, string p_str_so_num_from, string p_str_so_num_to, string p_str_so_dt_from, string p_str_so_dt_to, string p_str_load_number)
        {
            string l_str_fm_dt = string.Empty;
            DateTime date = DateTime.Now;
            l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");

            OBSRLoadInquiry objOBSRLoadInquiry = new OBSRLoadInquiry();
            OBSRLoadEntryService ServiceObject = new OBSRLoadEntryService();

            CompanyService ServiceObjectCompany = new CompanyService();
            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = p_str_cmp_id;

            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objOBSRLoadInquiry.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objOBSRLoadInquiry.ListStatePick = objPick.ListStatePick;

            objOBSRLoadInquiry.cmp_id = p_str_cmp_id;
            objOBSRLoadInquiry.batch_num = p_str_batch_num;
            objOBSRLoadInquiry.so_num_from = p_str_so_num_from;
            objOBSRLoadInquiry.so_num_to = p_str_so_num_to;
            objOBSRLoadInquiry.so_dt_from = p_str_so_dt_from;
            objOBSRLoadInquiry.so_dt_to = p_str_so_dt_to;
            objOBSRLoadInquiry.load_number = p_str_load_number;

            objOBSRLoadInquiry = ServiceObject.GetOBSRSummaryForLoadEntry(objOBSRLoadInquiry);
            if (objOBSRLoadInquiry.ListOBSRShipTo.Count > 0)
            {
                objOBSRLoadInquiry.st_mail_name = objOBSRLoadInquiry.ListOBSRShipTo[0].st_mail_name;
                objOBSRLoadInquiry.st_addr_line1 = objOBSRLoadInquiry.ListOBSRShipTo[0].st_addr_line1;
                objOBSRLoadInquiry.st_addr_line2 = objOBSRLoadInquiry.ListOBSRShipTo[0].st_addr_line2;
                objOBSRLoadInquiry.st_city = objOBSRLoadInquiry.ListOBSRShipTo[0].st_city;
                objOBSRLoadInquiry.st_state_id = objOBSRLoadInquiry.ListOBSRShipTo[0].st_state_id;
                objOBSRLoadInquiry.st_cntry_id = objOBSRLoadInquiry.ListOBSRShipTo[0].st_cntry_id;
                objOBSRLoadInquiry.st_post_code = objOBSRLoadInquiry.ListOBSRShipTo[0].st_post_code;
            }
            int l_int_grant_tot_ctns = 0;
            decimal l_int_grant_tot_weight = 0;
            decimal l_int_grant_tot_cube = 0;
            for (int i = 0; i < objOBSRLoadInquiry.ListOBGetSRSummary.Count; i++)
            {
                l_int_grant_tot_ctns = l_int_grant_tot_ctns + objOBSRLoadInquiry.ListOBGetSRSummary[i].tot_ctns;
                l_int_grant_tot_weight = l_int_grant_tot_weight + objOBSRLoadInquiry.ListOBGetSRSummary[i].tot_weight;
                l_int_grant_tot_cube = l_int_grant_tot_cube + objOBSRLoadInquiry.ListOBGetSRSummary[i].tot_cube;
            }
            objOBSRLoadInquiry.grant_tot_ctns = l_int_grant_tot_ctns;
            objOBSRLoadInquiry.grant_tot_weight = l_int_grant_tot_weight;
            objOBSRLoadInquiry.grant_tot_cube = l_int_grant_tot_cube;

            Company objCompany = new Company();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objOBSRLoadInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objCompany.cmp_id = p_str_cmp_id;


            Mapper.CreateMap<OBSRLoadInquiry, OBSRLoadInquiryModel>();
            OBSRLoadInquiryModel objOBSRLoadInquiryModel = Mapper.Map<OBSRLoadInquiry, OBSRLoadInquiryModel>(objOBSRLoadInquiry);
            return PartialView("_LoadEntryGrid", objOBSRLoadInquiryModel);
        }


        public ActionResult GetOBSRSummary(string p_str_cmp_id, string p_str_batch_num, string p_str_so_num_from, string p_str_so_num_to, string p_str_so_dt_from, string p_str_so_dt_to, string p_str_load_number)
        {
            string l_str_fm_dt = string.Empty;
            DateTime date = DateTime.Now;
            l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");

            OBSRLoadInquiry objOBSRLoadInquiry = new OBSRLoadInquiry();
            OBSRLoadEntryService ServiceObject = new OBSRLoadEntryService();

            CompanyService ServiceObjectCompany = new CompanyService();
            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = p_str_cmp_id;

            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objOBSRLoadInquiry.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objOBSRLoadInquiry.ListStatePick = objPick.ListStatePick;

            objOBSRLoadInquiry.cmp_id = p_str_cmp_id;
            objOBSRLoadInquiry.batch_num = p_str_batch_num;
            objOBSRLoadInquiry.so_num_from = p_str_so_num_from;
            objOBSRLoadInquiry.so_num_to = p_str_so_num_to;
            objOBSRLoadInquiry.so_dt_from = p_str_so_dt_from;
            objOBSRLoadInquiry.so_dt_to = p_str_so_dt_to;
            objOBSRLoadInquiry.load_number = p_str_load_number;

            objOBSRLoadInquiry = ServiceObject.GetOBSRSummary(objOBSRLoadInquiry);
            objOBSRLoadInquiry = ServiceObject.getOBLoadHdrByBatch(objOBSRLoadInquiry);
            if (objOBSRLoadInquiry.ListOBSRLoadEntryHdr != null && objOBSRLoadInquiry.ListOBSRLoadEntryHdr.Count > 0)
            {
                objOBSRLoadInquiry.batch_num = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].batch_num;
                objOBSRLoadInquiry.load_number = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].load_number;
                objOBSRLoadInquiry.load_approve_dt = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].load_approve_dt;
                objOBSRLoadInquiry.bol_number = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].bol_number;
                objOBSRLoadInquiry.spcl_inst = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].spcl_inst;

                objOBSRLoadInquiry.load_pick_dt = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].load_pick_dt;
                objOBSRLoadInquiry.carrier_name = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].carrier_name;
                objOBSRLoadInquiry.trailer_num = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].trailer_num;
                objOBSRLoadInquiry.seal_num = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].seal_num;
                objOBSRLoadInquiry.spcl_inst = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].spcl_inst;

                objOBSRLoadInquiry.shipto_id = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].shipto_id;
                objOBSRLoadInquiry.st_mail_name = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_mail_name;
                objOBSRLoadInquiry.st_addr_line1 = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_addr_line1;
                objOBSRLoadInquiry.st_addr_line2 = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_addr_line2;
                objOBSRLoadInquiry.st_city = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_city;
                objOBSRLoadInquiry.st_cntry_id = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_cntry_id;
                objOBSRLoadInquiry.st_state_id = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_state_id;
                objOBSRLoadInquiry.st_post_code = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_post_code;
            }
            else if (objOBSRLoadInquiry.ListOBSRShipTo.Count > 0)
            {
                objOBSRLoadInquiry.st_mail_name = objOBSRLoadInquiry.ListOBSRShipTo[0].st_mail_name;
                objOBSRLoadInquiry.st_addr_line1 = objOBSRLoadInquiry.ListOBSRShipTo[0].st_addr_line1;
                objOBSRLoadInquiry.st_addr_line2 = objOBSRLoadInquiry.ListOBSRShipTo[0].st_addr_line2;
                objOBSRLoadInquiry.st_city = objOBSRLoadInquiry.ListOBSRShipTo[0].st_city;
                objOBSRLoadInquiry.st_state_id = objOBSRLoadInquiry.ListOBSRShipTo[0].st_state_id;
                objOBSRLoadInquiry.st_cntry_id = objOBSRLoadInquiry.ListOBSRShipTo[0].st_cntry_id;
                objOBSRLoadInquiry.st_post_code = objOBSRLoadInquiry.ListOBSRShipTo[0].st_post_code;
            }
            int l_int_grant_tot_ctns = 0;
            decimal l_int_grant_tot_weight = 0;
            decimal l_int_grant_tot_cube = 0;
            for (int i = 0; i < objOBSRLoadInquiry.ListOBGetSRSummary.Count; i++)
            {
                l_int_grant_tot_ctns = l_int_grant_tot_ctns + objOBSRLoadInquiry.ListOBGetSRSummary[i].tot_ctns;
                l_int_grant_tot_weight = l_int_grant_tot_weight + objOBSRLoadInquiry.ListOBGetSRSummary[i].tot_weight;
                l_int_grant_tot_cube = l_int_grant_tot_cube + objOBSRLoadInquiry.ListOBGetSRSummary[i].tot_cube;
            }
            objOBSRLoadInquiry.grant_tot_ctns = l_int_grant_tot_ctns;
            objOBSRLoadInquiry.grant_tot_weight = l_int_grant_tot_weight;
            objOBSRLoadInquiry.grant_tot_cube = l_int_grant_tot_cube;

            objOBSRLoadInquiry.tot_ctns = l_int_grant_tot_ctns;
            objOBSRLoadInquiry.tot_weight = l_int_grant_tot_weight;
            objOBSRLoadInquiry.tot_cube = l_int_grant_tot_cube;

            Company objCompany = new Company();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objOBSRLoadInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objCompany.cmp_id = p_str_cmp_id;


            Mapper.CreateMap<OBSRLoadInquiry, OBSRLoadInquiryModel>();
            OBSRLoadInquiryModel objOBSRLoadInquiryModel = Mapper.Map<OBSRLoadInquiry, OBSRLoadInquiryModel>(objOBSRLoadInquiry);
            return PartialView("_BatchBOLEntryGrid", objOBSRLoadInquiryModel);
        }
        public ActionResult LoadEntry(string p_str_single_sr, string p_str_cmp_id, string p_str_batch_num, string p_str_so_num_from, string p_str_so_num_to,
            string p_str_so_dt_from, string p_str_so_dt_to, string p_str_load_number, string p_str_scn_id)
        {

            OBSRLoadInquiry objOBSRLoadInquiry = new OBSRLoadInquiry();
            OBSRLoadEntryService ServiceObject = new OBSRLoadEntryService();
            CompanyService ServiceObjectCompany = new CompanyService();

            string l_str_load_doc_id = string.Empty;
            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = p_str_cmp_id;

            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objOBSRLoadInquiry.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objOBSRLoadInquiry.ListStatePick = objPick.ListStatePick;

            objOBSRLoadInquiry.cmp_id = p_str_cmp_id;
            objOBSRLoadInquiry.batch_num = p_str_batch_num;
            objOBSRLoadInquiry.so_num_from = p_str_so_num_from;
            objOBSRLoadInquiry.so_num_to = p_str_so_num_to;
            objOBSRLoadInquiry.so_dt_from = p_str_so_dt_from;
            objOBSRLoadInquiry.so_dt_to = p_str_so_dt_to;
            objOBSRLoadInquiry.load_number = p_str_load_number;
            objOBSRLoadInquiry.single_sr = p_str_single_sr;

            if (p_str_single_sr == "Y")
            {
                l_str_load_doc_id = ServiceObject.GetOBLoadDocIdBySR(p_str_cmp_id, p_str_so_num_from);
            }

            if (l_str_load_doc_id.Length > 0)
            {
                objOBSRLoadInquiry.load_doc_id = l_str_load_doc_id;
                if (p_str_single_sr == "Y")
                {
                    objOBSRLoadInquiry = ServiceObject.GetLoadEntry(objOBSRLoadInquiry);
                }
                else
                {
                    objOBSRLoadInquiry.so_num_from = string.Empty;
                    objOBSRLoadInquiry = ServiceObject.GetLoadEntry(objOBSRLoadInquiry);
                    objOBSRLoadInquiry.so_num_from = p_str_so_num_from;
                }
                objOBSRLoadInquiry.batch_num = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].batch_num;
                objOBSRLoadInquiry.load_number = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].load_number;
                objOBSRLoadInquiry.load_approve_dt = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].load_approve_dt;
                objOBSRLoadInquiry.bol_number = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].bol_number;
                objOBSRLoadInquiry.spcl_inst = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].spcl_inst;

                objOBSRLoadInquiry.load_pick_dt = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].load_pick_dt;
                objOBSRLoadInquiry.carrier_name = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].carrier_name;
                objOBSRLoadInquiry.trailer_num = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].trailer_num;
                objOBSRLoadInquiry.seal_num = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].seal_num;
                objOBSRLoadInquiry.spcl_inst = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].spcl_inst;

                objOBSRLoadInquiry.shipto_id = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].shipto_id;
                objOBSRLoadInquiry.st_mail_name = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_mail_name;
                objOBSRLoadInquiry.st_addr_line1 = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_addr_line1;
                objOBSRLoadInquiry.st_addr_line2 = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_addr_line2;
                objOBSRLoadInquiry.st_city = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_city;
                objOBSRLoadInquiry.st_cntry_id = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_cntry_id;
                objOBSRLoadInquiry.st_state_id = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_state_id;
                objOBSRLoadInquiry.st_post_code = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_post_code;

            }
            else if (p_str_batch_num.Length > 0 || p_str_so_num_from.Length > 0)
            {
                objOBSRLoadInquiry = ServiceObject.GetOBSRSummaryForLoadEntry(objOBSRLoadInquiry);
                if (objOBSRLoadInquiry.ListOBSRShipTo.Count > 0)
                {
                    objOBSRLoadInquiry.st_mail_name = objOBSRLoadInquiry.ListOBSRShipTo[0].st_mail_name;
                    objOBSRLoadInquiry.st_addr_line1 = objOBSRLoadInquiry.ListOBSRShipTo[0].st_addr_line1;
                    objOBSRLoadInquiry.st_addr_line2 = objOBSRLoadInquiry.ListOBSRShipTo[0].st_addr_line2;
                    objOBSRLoadInquiry.st_city = objOBSRLoadInquiry.ListOBSRShipTo[0].st_city;
                    objOBSRLoadInquiry.st_cntry_id = objOBSRLoadInquiry.ListOBSRShipTo[0].st_cntry_id;
                    objOBSRLoadInquiry.st_state_id = objOBSRLoadInquiry.ListOBSRShipTo[0].st_state_id;
                    objOBSRLoadInquiry.st_post_code = objOBSRLoadInquiry.ListOBSRShipTo[0].st_post_code;

                    objOBSRLoadInquiry.load_doc_id = Convert.ToString(ServiceObject.GetOBLoadDocId());
                }
            }
            else
            {
                objOBSRLoadInquiry.load_doc_id = Convert.ToString(ServiceObject.GetOBLoadDocId());
            }


            Company objCompany = new Company();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objOBSRLoadInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objCompany.cmp_id = p_str_cmp_id;
            Mapper.CreateMap<OBSRLoadInquiry, OBSRLoadInquiryModel>();
            OBSRLoadInquiryModel objOBSRLoadInquiryModel = Mapper.Map<OBSRLoadInquiry, OBSRLoadInquiryModel>(objOBSRLoadInquiry);
            return PartialView("_LoadEntry", objOBSRLoadInquiryModel);
        }

        public ActionResult BatchBOLEntry(string p_str_single_sr, string p_str_cmp_id, string p_str_batch_num, string p_str_so_num_from, string p_str_so_num_to, string p_str_so_dt_from, string p_str_so_dt_to, string p_str_load_number, string p_str_scn_id, string p_str_print_summary)
        {

            OBSRLoadInquiry objOBSRLoadInquiry = new OBSRLoadInquiry();
            OBSRLoadEntryService ServiceObject = new OBSRLoadEntryService();
            CompanyService ServiceObjectCompany = new CompanyService();
            string l_str_load_doc_id = string.Empty;
            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = p_str_cmp_id;

            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objOBSRLoadInquiry.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objOBSRLoadInquiry.ListStatePick = objPick.ListStatePick;

            objOBSRLoadInquiry.cmp_id = p_str_cmp_id;
            objOBSRLoadInquiry.batch_num = p_str_batch_num;
            objOBSRLoadInquiry.so_num_from = p_str_so_num_from;
            objOBSRLoadInquiry.so_num_to = p_str_so_num_to;
            objOBSRLoadInquiry.so_dt_from = p_str_so_dt_from;
            objOBSRLoadInquiry.so_dt_to = p_str_so_dt_to;
            objOBSRLoadInquiry.load_number = p_str_load_number;
            objOBSRLoadInquiry.single_sr = p_str_single_sr;
            objOBSRLoadInquiry.print_summary = p_str_print_summary;

            if (p_str_print_summary == "S")
            {
                l_str_load_doc_id = ServiceObject.GetOBLoadDocIdBySR(p_str_cmp_id, p_str_so_num_from);
            }
            if (l_str_load_doc_id.Length > 0)
            {
                objOBSRLoadInquiry.load_doc_id = l_str_load_doc_id;
                if (p_str_print_summary == "S")
                {
                    objOBSRLoadInquiry = ServiceObject.GetLoadEntry(objOBSRLoadInquiry);
                }
                else
                {
                    objOBSRLoadInquiry.so_num_from = string.Empty;
                    objOBSRLoadInquiry = ServiceObject.GetLoadEntry(objOBSRLoadInquiry);
                    objOBSRLoadInquiry.so_num_from = p_str_so_num_from;
                }
                objOBSRLoadInquiry.batch_num = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].batch_num;
                objOBSRLoadInquiry.load_number = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].load_number;
                objOBSRLoadInquiry.load_approve_dt = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].load_approve_dt;
                objOBSRLoadInquiry.bol_number = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].bol_number;
                objOBSRLoadInquiry.spcl_inst = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].spcl_inst;

                objOBSRLoadInquiry.load_pick_dt = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].load_pick_dt;
                objOBSRLoadInquiry.carrier_name = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].carrier_name;
                objOBSRLoadInquiry.trailer_num = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].trailer_num;
                objOBSRLoadInquiry.seal_num = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].seal_num;
                objOBSRLoadInquiry.spcl_inst = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].spcl_inst;

                objOBSRLoadInquiry.shipto_id = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].shipto_id;
                objOBSRLoadInquiry.st_mail_name = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_mail_name;
                objOBSRLoadInquiry.st_addr_line1 = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_addr_line1;
                objOBSRLoadInquiry.st_addr_line2 = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_addr_line2;
                objOBSRLoadInquiry.st_city = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_city;
                objOBSRLoadInquiry.st_cntry_id = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_cntry_id;
                objOBSRLoadInquiry.st_state_id = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_state_id;
                objOBSRLoadInquiry.st_post_code = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_post_code;

            }
            else if (p_str_batch_num.Length > 0 || p_str_so_num_from.Length > 0)
            {
                objOBSRLoadInquiry = ServiceObject.GetOBSRSummary(objOBSRLoadInquiry);
                if (objOBSRLoadInquiry.ListOBSRShipTo.Count > 0)
                {
                    objOBSRLoadInquiry.st_mail_name = objOBSRLoadInquiry.ListOBSRShipTo[0].st_mail_name;
                    objOBSRLoadInquiry.st_addr_line1 = objOBSRLoadInquiry.ListOBSRShipTo[0].st_addr_line1;
                    objOBSRLoadInquiry.st_addr_line2 = objOBSRLoadInquiry.ListOBSRShipTo[0].st_addr_line2;
                    objOBSRLoadInquiry.st_city = objOBSRLoadInquiry.ListOBSRShipTo[0].st_city;
                    objOBSRLoadInquiry.st_cntry_id = objOBSRLoadInquiry.ListOBSRShipTo[0].st_cntry_id;
                    objOBSRLoadInquiry.st_state_id = objOBSRLoadInquiry.ListOBSRShipTo[0].st_state_id;
                    objOBSRLoadInquiry.st_post_code = objOBSRLoadInquiry.ListOBSRShipTo[0].st_post_code;
                }
            }
            objOBSRLoadInquiry.load_doc_id = Convert.ToString(ServiceObject.GetOBLoadDocId());
            Company objCompany = new Company();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objOBSRLoadInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objCompany.cmp_id = p_str_cmp_id;
            Mapper.CreateMap<OBSRLoadInquiry, OBSRLoadInquiryModel>();
            OBSRLoadInquiryModel objOBSRLoadInquiryModel = Mapper.Map<OBSRLoadInquiry, OBSRLoadInquiryModel>(objOBSRLoadInquiry);
            return PartialView("_BatchBOLEntry", objOBSRLoadInquiryModel);
        }

        [HttpPost]
        public JsonResult SaveLoadEntry(string p_str_cmp_id, string p_str_load_doc_id, string p_str_load_number, string p_str_load_approve_dt, string p_str_load_pick_dt, string p_str_so_list, List<OBSRLoadEntryHdr> objOBSRLoadEntryHdr)
        {
            string l_str_error_code = string.Empty;
            string[] strSoList;
            string l_str_load_number = string.Empty;
            DataTable dtSoList = new DataTable();

            l_str_load_number = p_str_load_number;
            dtSoList.Columns.Add("cmp_id", typeof(string));
            dtSoList.Columns.Add("load_doc_id", typeof(string));
            dtSoList.Columns.Add("load_number", typeof(string));
            dtSoList.Columns.Add("load_approve_dt", typeof(DateTime));
            dtSoList.Columns.Add("so_num", typeof(string));
            dtSoList.Columns.Add("load_pick_dt", typeof(DateTime));


            if (p_str_so_list.Length > 0)
            {
                strSoList = p_str_so_list.Split(',');

                for (int i = 0; i < strSoList.Length; i++)
                {
                    DataRow dtSoRow = dtSoList.NewRow();
                    dtSoRow[0] = p_str_cmp_id;
                    dtSoRow[1] = p_str_load_doc_id;
                    if (l_str_load_number == string.Empty)
                    {
                        l_str_load_number = "99-" + strSoList[0].ToString();
                    }

                    dtSoRow[2] = l_str_load_number;
                    if (string.IsNullOrEmpty(p_str_load_approve_dt))
                    {

                        dtSoRow[3] = DBNull.Value;

                    }
                    else
                    {
                        dtSoRow[3] = DateTime.Parse(p_str_load_approve_dt);
                    }
                    dtSoRow[4] = strSoList[i].ToString();
                    if (string.IsNullOrEmpty(p_str_load_pick_dt))
                    {

                        dtSoRow[5] = DBNull.Value;

                    }
                    else
                    {
                        dtSoRow[5] = DateTime.Parse(p_str_load_pick_dt);
                    }


                    dtSoList.Rows.Add(dtSoRow);
                }
            }
            else
            {
                l_str_error_code = "No SO# selected";
                return Json(l_str_error_code, JsonRequestBehavior.AllowGet);
            }
            OBSRLoadInquiry objOBSRLoadInquiry = new OBSRLoadInquiry();
            OBSRLoadEntryService ServiceObject = new OBSRLoadEntryService();
            string user_id = string.Empty;
            user_id = Session["UserID"].ToString();
            if (ServiceObject.SaveOBLoadEntry(p_str_cmp_id, l_str_load_number, p_str_so_list, objOBSRLoadEntryHdr, user_id, dtSoList) == true)
            {

            }

            Mapper.CreateMap<OBSRLoadInquiry, OBSRLoadInquiryModel>();
            OBSRLoadInquiryModel objOBSRLoadInquiryModel = Mapper.Map<OBSRLoadInquiry, OBSRLoadInquiryModel>(objOBSRLoadInquiry);
            objOBSRLoadEntryHdr.Clear();

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveTempBOLBatchEntry(string p_str_cmp_id, string p_str_load_doc_id, string p_str_load_number, string p_str_load_approve_dt, string p_str_load_pick_dt,  List<OBSRLoadEntryHdr> objOBSRLoadEntryHdr
            , List<OBBOLDtl> ListOBBOLDtl)
        {
            string l_str_error_code = string.Empty;
            string l_str_load_number = string.Empty;

            Session["pintOrdrCount"] = ListOBBOLDtl.Count;

            DataTable dtSoList = new DataTable();

            l_str_load_number = p_str_load_number;
            dtSoList.Columns.Add("cmp_id", typeof(string));
            dtSoList.Columns.Add("load_doc_id", typeof(string));
            dtSoList.Columns.Add("load_number", typeof(string));
            dtSoList.Columns.Add("load_approve_dt", typeof(DateTime));

            dtSoList.Columns.Add("so_num", typeof(string));
            dtSoList.Columns.Add("load_pick_dt", typeof(DateTime));
            dtSoList.Columns.Add("cust_ordr_num", typeof(string));
            dtSoList.Columns.Add("tot_weight", typeof(string));
            dtSoList.Columns.Add("add_info", typeof(string));
            dtSoList.Columns.Add("tot_ctns", typeof(string));


            if (ListOBBOLDtl != null)
            {
                //strSoList = string.Empty;

                for (int i = 0; i < ListOBBOLDtl.Count; i++)
                {
                    DataRow dtSoRow = dtSoList.NewRow();
                    dtSoRow[0] = p_str_cmp_id;
                    dtSoRow[1] = p_str_load_doc_id;
                    if (l_str_load_number == string.Empty)
                    {
                        l_str_load_number = "99-" + ListOBBOLDtl[i].so_num;
                    }

                    dtSoRow[2] = l_str_load_number;
                    if (string.IsNullOrEmpty(p_str_load_approve_dt))
                    {

                        dtSoRow[3] = DBNull.Value;

                    }
                    else
                    {
                        dtSoRow[3] = DateTime.Parse(p_str_load_approve_dt);
                    }
                    dtSoRow[4] = ListOBBOLDtl[i].so_num;
                    if (string.IsNullOrEmpty(p_str_load_pick_dt))
                    {

                        dtSoRow[5] = DBNull.Value;

                    }
                    else
                    {
                        dtSoRow[5] = DateTime.Parse(p_str_load_pick_dt);
                    }

                    dtSoRow[6] = ListOBBOLDtl[i].cust_ordr_num;
                    dtSoRow[7] = ListOBBOLDtl[i].tot_weight;
                    dtSoRow[8] = ListOBBOLDtl[i].add_info;
                    dtSoRow[9] = ListOBBOLDtl[i].tot_ctns;

                    dtSoList.Rows.Add(dtSoRow);
                }
            }
            else
            {
                l_str_error_code = "No SO# selected";
                return Json(l_str_error_code, JsonRequestBehavior.AllowGet);
            }
            OBSRLoadInquiry objOBSRLoadInquiry = new OBSRLoadInquiry();
            OBSRLoadEntryService ServiceObject = new OBSRLoadEntryService();
            string user_id = string.Empty;
            user_id = Session["UserID"].ToString();

            if (ServiceObject.fnSaveMasterBol(p_str_cmp_id, l_str_load_number, objOBSRLoadEntryHdr, user_id, dtSoList) == true)
            {

            }

            Mapper.CreateMap<OBSRLoadInquiry, OBSRLoadInquiryModel>();
            OBSRLoadInquiryModel objOBSRLoadInquiryModel = Mapper.Map<OBSRLoadInquiry, OBSRLoadInquiryModel>(objOBSRLoadInquiry);
            objOBSRLoadEntryHdr.Clear();

            return Json("", JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteLoadEntry(string p_str_cmp_id, string p_str_load_doc_id, string p_str_load_number)
        {
            string l_str_error_code = string.Empty;

            OBSRLoadInquiry objOBSRLoadInquiry = new OBSRLoadInquiry();
            OBSRLoadEntryService ServiceObject = new OBSRLoadEntryService();
            string user_id = string.Empty;
            user_id = Session["UserID"].ToString();
            if (ServiceObject.DeleteLoadEntry(p_str_cmp_id, p_str_load_doc_id, p_str_load_number) == true)
            {

            }

            Mapper.CreateMap<OBSRLoadInquiry, OBSRLoadInquiryModel>();
            OBSRLoadInquiryModel objOBSRLoadInquiryModel = Mapper.Map<OBSRLoadInquiry, OBSRLoadInquiryModel>(objOBSRLoadInquiry);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult SRLoadDelete(string p_str_cmp_id, string p_str_load_doc_id, string p_str_load_number)
        {
            OBSRLoadInquiry objOBSRLoadInquiry = new OBSRLoadInquiry();
            OBSRLoadEntryService ServiceObject = new OBSRLoadEntryService();
            CompanyService ServiceObjectCompany = new CompanyService();

            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = p_str_cmp_id;

            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objOBSRLoadInquiry.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objOBSRLoadInquiry.ListStatePick = objPick.ListStatePick;

            objOBSRLoadInquiry.cmp_id = p_str_cmp_id;
            objOBSRLoadInquiry.load_number = p_str_load_number;
            objOBSRLoadInquiry.load_doc_id = p_str_load_doc_id;

            if (p_str_load_number.Length > 0 || p_str_load_doc_id.Length > 0)
            {
                objOBSRLoadInquiry = ServiceObject.GetLoadEntry(objOBSRLoadInquiry);

            }

            Company objCompany = new Company();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objOBSRLoadInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objCompany.cmp_id = p_str_cmp_id;
            Mapper.CreateMap<OBSRLoadInquiry, OBSRLoadInquiryModel>();
            OBSRLoadInquiryModel objOBSRLoadInquiryModel = Mapper.Map<OBSRLoadInquiry, OBSRLoadInquiryModel>(objOBSRLoadInquiry);
            return PartialView("_LoadEntryDelete", objOBSRLoadInquiryModel);
        }
        public ActionResult ViewLoadEntry(string p_str_cmp_id, string p_str_load_doc_id, string p_str_load_number, string p_str_so_num)
        {
            OBSRLoadInquiry objOBSRLoadInquiry = new OBSRLoadInquiry();
            OBSRLoadEntryService ServiceObject = new OBSRLoadEntryService();
            CompanyService ServiceObjectCompany = new CompanyService();

            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = p_str_cmp_id;

            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objOBSRLoadInquiry.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objOBSRLoadInquiry.ListStatePick = objPick.ListStatePick;

            objOBSRLoadInquiry.cmp_id = p_str_cmp_id;
            objOBSRLoadInquiry.load_number = p_str_load_number;


            if (p_str_load_number.Length > 0 || p_str_load_doc_id.Length > 0)
            {
                objOBSRLoadInquiry = ServiceObject.GetLoadEntry(objOBSRLoadInquiry);
                if (objOBSRLoadInquiry.ListOBSRLoadEntryHdr.Count > 0)
                {
                    objOBSRLoadInquiry.st_mail_name = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_mail_name;
                    objOBSRLoadInquiry.st_addr_line1 = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_addr_line1;
                    objOBSRLoadInquiry.st_addr_line2 = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_addr_line2;
                    objOBSRLoadInquiry.st_city = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_city;
                    objOBSRLoadInquiry.st_cntry_id = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_cntry_id;
                    objOBSRLoadInquiry.st_state_id = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_state_id;
                    objOBSRLoadInquiry.st_post_code = objOBSRLoadInquiry.ListOBSRLoadEntryHdr[0].st_post_code;
                }
            }

            Company objCompany = new Company();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objOBSRLoadInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objCompany.cmp_id = p_str_cmp_id;
            Mapper.CreateMap<OBSRLoadInquiry, OBSRLoadInquiryModel>();
            OBSRLoadInquiryModel objOBSRLoadInquiryModel = Mapper.Map<OBSRLoadInquiry, OBSRLoadInquiryModel>(objOBSRLoadInquiry);
            return PartialView("_LoadEntryView", objOBSRLoadInquiryModel);
        }

        public ActionResult LoadEntryBatch(string p_str_cmp_id)
        {

            string l_str_cmp_id = string.Empty;
            OBLoadUploadFile objOBLoadUploadFile = new OBLoadUploadFile();
            OBLoadUploadFileInfo objOBLoadUploadFileInfo = new OBLoadUploadFileInfo();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objOBLoadUploadFile.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
            objOBLoadUploadFile.user_id = Session["UserID"].ToString().Trim();
            objOBLoadUploadFile.objOBLoadUploadFileInfo = objOBLoadUploadFileInfo;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objOBLoadUploadFile.ListCompany = objCompany.ListCompanyPickDtl;
            Session["lstOBLoadUploadDtl"] = "";
            Session["lstOBLoadUploadInvalidData"] = "";
            Session["objOBLoadUploadFileInfo"] = "";
            Session["l_str_error_msg"] = "";
            Mapper.CreateMap<OBLoadUploadFile, OBLoadUploadFileModel>();
            OBLoadUploadFileModel objOBLoadUploadFileModel = Mapper.Map<OBLoadUploadFile, OBLoadUploadFileModel>(objOBLoadUploadFile);
            return PartialView("_LoadEntryBatch", objOBLoadUploadFileModel);
        }

        public FileResult LoadEntryTemplatedownload()
        {
            return File("~\\templates\\OB_SF201_LOADNUM_UPLOAD_with_COMMENTS.xlsx", "text/xlsx", string.Format("OB_SF201_LOADNUM_UPLOAD_with_COMMENTS-{0}.xlsx", DateTime.Now.ToString("yyyyMMdd-HHmmss")));
        }

        public ActionResult LoadEntryBatchClearAll(string p_str_cmp_id)
        {
            OBLoadUploadFile objOBLoadUploadFile = new OBLoadUploadFile();
            OBLoadUploadFileInfo objOBLoadUploadFileInfo = new OBLoadUploadFileInfo();
            Session["lstOBLoadUploadDtl"] = "";
            Session["lstOBLoadUploadInvalidData"] = "";
            Session["objOBLoadUploadFileInfo"] = "";
            Session["l_str_error_msg"] = "";
            objOBLoadUploadFile.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
            objOBLoadUploadFile.user_id = Session["UserID"].ToString().Trim();
            objOBLoadUploadFile.objOBLoadUploadFileInfo = objOBLoadUploadFileInfo;
            Mapper.CreateMap<OBLoadUploadFile, OBLoadUploadFileModel>();
            OBLoadUploadFileModel objOBLoadUploadFileModel = Mapper.Map<OBLoadUploadFile, OBLoadUploadFileModel>(objOBLoadUploadFile);
            return PartialView("_LoadEntryBatch", objOBLoadUploadFileModel);

        }

        public ActionResult CheckLoadEntryFileExists(string p_str_cmp_id, string p_str_file_name)
        {
            OBLoadEntryUploadFileService ServiceOBLoadEntryUploadFile = new OBLoadEntryUploadFileService();
            bool l_bl_file_exist = false;
            l_bl_file_exist = ServiceOBLoadEntryUploadFile.CheckLoadEntryFileExists(p_str_cmp_id, p_str_file_name);
            return Json(l_bl_file_exist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadLoadEntryFile(string l_str_cmp_id)
        {
            OBLoadUploadFile objOBLoadUploadFile = new OBLoadUploadFile();
            OBLoadEntryUploadFileService ServiceOBLoadEntryUploadFile = new OBLoadEntryUploadFileService();
            Session["lstOBLoadUploadDtl"] = "";
            Session["lstOBLoadUploadInvalidData"] = "";
            Session["objOBLoadUploadFileInfo"] = "";
            Session["l_str_error_msg"] = "";
            ViewBag.l_int_error_count = "0";


            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase FileUpload = files[i];
                        string filename;
                        string l_str_error_msg = string.Empty;
                        string BatchResult = string.Empty;


                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = FileUpload.FileName.Split(new char[] { '\\' });
                            filename = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            filename = FileUpload.FileName;
                        }


                        if (FileUpload != null)
                        {
                            if (FileUpload.ContentLength > 0)
                            {
                                //Workout our file path
                                string l_str_file_name = Path.GetFileName(FileUpload.FileName);
                                string l_str_file_path = Path.Combine(Server.MapPath("~/uploads"), l_str_file_name);
                                //Try and upload
                                try
                                {
                                    FileUpload.SaveAs(l_str_file_path);
                                    string l_str_ext = Path.GetExtension(l_str_file_name);
                                    if (l_str_ext.ToUpper() != ".CSV")
                                    {
                                        objOBLoadUploadFile.error_mode = true;
                                        objOBLoadUploadFile.error_desc = "Invalid File Format";
                                        return Json(objOBLoadUploadFile, JsonRequestBehavior.AllowGet);
                                    }

                                    Get_Load_Entry_CSV_List_Data(l_str_cmp_id, l_str_file_path, l_str_file_name, ".CSV", ref l_str_error_msg);

                                    if (l_str_error_msg != "")
                                    {
                                        objOBLoadUploadFile.error_mode = true;
                                        objOBLoadUploadFile.error_desc = l_str_error_msg;

                                    }
                                    objOBLoadUploadFile.ListOBLoadUploadDtl = lstOBLoadUploadDtl;
                                    objOBLoadUploadFile.ListOBLoadUploadInvalidData = lstOBLoadUploadInvalidData;
                                    ViewBag.l_int_error_count = lstOBLoadUploadInvalidData.Count;

                                    Mapper.CreateMap<OBLoadUploadFile, OBLoadUploadFileModel>();
                                    OBLoadUploadFileModel OBLoadUploadFileModel = Mapper.Map<OBLoadUploadFile, OBLoadUploadFileModel>(objOBLoadUploadFile);
                                    ////return Json(objOBLoadUploadFile, JsonRequestBehavior.AllowGet);
                                    return PartialView("_LoadEntryBatchGrid", OBLoadUploadFileModel);

                                }
                                catch (Exception ex)
                                {

                                    objOBLoadUploadFile.error_mode = true;
                                    objOBLoadUploadFile.error_desc = ex.InnerException.ToString();
                                    return Json(objOBLoadUploadFile, JsonRequestBehavior.AllowGet);
                                }

                            }

                        }


                        else
                        {
                            //Catch errors
                            objOBLoadUploadFile.error_mode = true;
                            objOBLoadUploadFile.error_desc = "Please select a file";
                            return Json(objOBLoadUploadFile, JsonRequestBehavior.AllowGet);
                        }



                    }
                }
                catch (Exception ex)
                {
                    objOBLoadUploadFile.error_mode = true;
                    objOBLoadUploadFile.error_desc = ex.Message;
                    return Json(objOBLoadUploadFile, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                objOBLoadUploadFile.error_mode = true;
                objOBLoadUploadFile.error_desc = "No files selected.";
                return Json(objOBLoadUploadFile, JsonRequestBehavior.AllowGet);
            }
            Mapper.CreateMap<OBLoadUploadFile, OBLoadUploadFileModel>();
            OBLoadUploadFileModel objOBLoadUploadFileModel = Mapper.Map<OBLoadUploadFile, OBLoadUploadFileModel>(objOBLoadUploadFile);
            return PartialView("_LoadEntryBatchGrid", objOBLoadUploadFileModel);
        }

        enum GrdLoad : int
        {
            SLF201 = 0,
            CMP_ID = 1,
            BATCH_ID = 2,
            CUST_PO = 3,
            DEPT = 4,
            STORE = 5,
            LOADNUM = 6,
            LOAD_APPROVE_DT = 7,
            CARRIER = 8,
            CARRIER_PICK_DT = 9,
            CUBE = 10,
            WGT = 11,
        }

        private void Get_Load_Entry_CSV_List_Data(string p_str_cmp_id, string p_str_file_path, string p_str_file_name, string p_str_file_extn, ref string l_str_error_msg)
        {


            lstOBLoadUploadDtl = new List<OBLoadUploadDtl>();
            lstOBLoadUploadInvalidData = new List<OBLoadUploadInvalidData>();
            OBLoadEntryUploadFileService ServiceOBLoadEntryUploadFile = new OBLoadEntryUploadFileService();
            l_str_error_msg = string.Empty;
            string l_str_error_desc = string.Empty;
            string l_str_hdr_data = string.Empty;
            string l_str_file_name = string.Empty;
            string l_str_upload_ref_num = string.Empty;
            int l_int_no_of_lines = 0;
            string l_str_cntr_id = string.Empty;
            int l_int_line_num = 0;
            int l_int_line_without_data = 0;
            try
            {
                l_str_file_name = System.IO.Path.GetFileNameWithoutExtension(p_str_file_path);
                if (p_str_file_extn.ToUpper().Equals(".CSV"))
                {
                    List<string> lst_file_line_content = new List<string>(System.IO.File.ReadAllLines(p_str_file_path));
                    int l_int_blank_line = lst_file_line_content.FindIndex(x => x.Trim().Length == 0);
                    l_int_line_without_data = lst_file_line_content.FindIndex(x => x.Trim().Contains(",,,,,,,"));

                    if (l_int_blank_line != -1)
                    {
                        while (lst_file_line_content.Count > l_int_blank_line)
                        {
                            lst_file_line_content.RemoveAt(lst_file_line_content.Count - 1);
                        }
                    }

                    if (l_int_line_without_data != -1)
                    {
                        while (lst_file_line_content.Count > l_int_line_without_data)
                        {
                            lst_file_line_content.RemoveAt(lst_file_line_content.Count - 1);
                        }
                    }


                    if (lst_file_line_content.Count() > 0)
                    {

                        l_str_upload_ref_num = Convert.ToString(ServiceOBLoadEntryUploadFile.GetOBLoadUploadRefNum(p_str_cmp_id));
                        l_int_no_of_lines = lst_file_line_content.Count();
                        objOBLoadUploadFileInfo = new OBLoadUploadFileInfo();
                        objOBLoadUploadFileInfo.cmp_id = p_str_cmp_id;
                        objOBLoadUploadFileInfo.file_name = p_str_file_name;
                        objOBLoadUploadFileInfo.upload_by = Session["UserID"].ToString().Trim();
                        objOBLoadUploadFileInfo.upload_date_time = DateTime.Now;
                        objOBLoadUploadFileInfo.no_of_lines = l_int_no_of_lines;
                        objOBLoadUploadFileInfo.status = "PEND";
                        objOBLoadUploadFileInfo.upload_ref_num = l_str_upload_ref_num;

                        Session["objOBLoadUploadFileInfo"] = objOBLoadUploadFileInfo;

                        int l_int_cur_line = 0;
                        List<string> lst_csv_data = new List<string>();
                        using (var file_reader = new CsvFileReader(p_str_file_path))
                        {
                            while (file_reader.ReadRow(lst_csv_data))
                            {
                                if (lst_csv_data[(int)GrdLoad.SLF201].ToUpper().Equals("SLF201") && l_int_cur_line < l_int_no_of_lines)
                                {

                                    l_int_cur_line = l_int_cur_line + 1;
                                    if (l_int_cur_line == 1)
                                    {
                                        continue;
                                    }
                                    OBLoadUploadDtl objOBLoadUploadDtl = new OBLoadUploadDtl();
                                    int l_str_length = lst_csv_data.Count;

                                    bool bool_is_valied = false;
                                    if ((l_str_length == 12)) bool_is_valied = true;
                                    if (bool_is_valied == false)
                                    {
                                        l_str_error_desc = "Line  " + l_int_cur_line.ToString() + " contains " + (l_str_length).ToString() + " fields It should be 12 Please refer the Link 'IB 943 Upload Sample for sample' available in this page ";
                                        OBLoadUploadInvalidData objOBLoadUploadInvalidData = new OBLoadUploadInvalidData();
                                        objOBLoadUploadInvalidData.batch_num = objOBLoadUploadDtl.batch_num;
                                        objOBLoadUploadInvalidData.line_num = objOBLoadUploadDtl.dtl_line;
                                        objOBLoadUploadInvalidData.error_desc = l_str_error_desc;
                                        objOBLoadUploadInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOBLoadUploadInvalidData.Add(objOBLoadUploadInvalidData);
                                        continue;
                                    }


                                    if (lst_csv_data[(int)GrdLoad.CMP_ID].Trim().Length > 10 || lst_csv_data[(int)GrdLoad.CMP_ID].Trim().Length <= 2)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + "- Company Id (2nd column) Length should be between 3 to 10 ";
                                        OBLoadUploadInvalidData objOBLoadUploadInvalidData = new OBLoadUploadInvalidData();
                                        objOBLoadUploadInvalidData.batch_num = objOBLoadUploadDtl.batch_num;
                                        objOBLoadUploadInvalidData.line_num = objOBLoadUploadDtl.dtl_line;
                                        objOBLoadUploadInvalidData.error_desc = l_str_error_desc;
                                        objOBLoadUploadInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOBLoadUploadInvalidData.Add(objOBLoadUploadInvalidData);
                                        continue;
                                    }

                                    if (lst_csv_data[(int)GrdLoad.CMP_ID] != p_str_cmp_id)
                                    {
                                        l_str_error_desc = "Line : " + l_int_cur_line.ToString() + " contains Invalid Company Id ";
                                        OBLoadUploadInvalidData objOBLoadUploadInvalidData = new OBLoadUploadInvalidData();
                                        objOBLoadUploadInvalidData.batch_num = objOBLoadUploadDtl.batch_num;
                                        objOBLoadUploadInvalidData.line_num = objOBLoadUploadDtl.dtl_line;
                                        objOBLoadUploadInvalidData.error_desc = l_str_error_desc;
                                        objOBLoadUploadInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOBLoadUploadInvalidData.Add(objOBLoadUploadInvalidData);
                                        continue;
                                    }


                                    if (lst_csv_data[(int)GrdLoad.BATCH_ID].Trim().Length > 10)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + "- Batch Id length should be maximum of 10 ";
                                        OBLoadUploadInvalidData objOBLoadUploadInvalidData = new OBLoadUploadInvalidData();
                                        objOBLoadUploadInvalidData.batch_num = objOBLoadUploadDtl.batch_num;
                                        objOBLoadUploadInvalidData.line_num = objOBLoadUploadDtl.dtl_line;
                                        objOBLoadUploadInvalidData.error_desc = l_str_error_desc;
                                        objOBLoadUploadInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOBLoadUploadInvalidData.Add(objOBLoadUploadInvalidData);
                                        continue;
                                    }

                                    else if (lst_csv_data[(int)GrdLoad.BATCH_ID].Trim().Length == 0)
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + "- Batch Id Should Not be blank ";
                                        OBLoadUploadInvalidData objOBLoadUploadInvalidData = new OBLoadUploadInvalidData();
                                        objOBLoadUploadInvalidData.batch_num = objOBLoadUploadDtl.batch_num;
                                        objOBLoadUploadInvalidData.line_num = objOBLoadUploadDtl.dtl_line;
                                        objOBLoadUploadInvalidData.error_desc = l_str_error_desc;
                                        objOBLoadUploadInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOBLoadUploadInvalidData.Add(objOBLoadUploadInvalidData);
                                        continue;
                                    }

                                    if (lst_csv_data[(int)GrdLoad.CUST_PO].Trim().Length > 20)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Cust PO Number should be maximum of 20 ";
                                        OBLoadUploadInvalidData objOBLoadUploadInvalidData = new OBLoadUploadInvalidData();
                                        objOBLoadUploadInvalidData.batch_num = objOBLoadUploadDtl.batch_num;
                                        objOBLoadUploadInvalidData.line_num = objOBLoadUploadDtl.dtl_line;
                                        objOBLoadUploadInvalidData.error_desc = l_str_error_desc;
                                        objOBLoadUploadInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOBLoadUploadInvalidData.Add(objOBLoadUploadInvalidData);
                                        continue;
                                    }

                                    else if (lst_csv_data[(int)GrdLoad.CUST_PO].Trim().Length == 0)
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + "- Cust PO Should Not be blank ";
                                        OBLoadUploadInvalidData objOBLoadUploadInvalidData = new OBLoadUploadInvalidData();
                                        objOBLoadUploadInvalidData.batch_num = objOBLoadUploadDtl.batch_num;
                                        objOBLoadUploadInvalidData.line_num = objOBLoadUploadDtl.dtl_line;
                                        objOBLoadUploadInvalidData.error_desc = l_str_error_desc;
                                        objOBLoadUploadInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOBLoadUploadInvalidData.Add(objOBLoadUploadInvalidData);
                                        continue;
                                    }


                                    if (lst_csv_data[(int)GrdLoad.DEPT].Trim().Length > 10)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Dept# should be maximum of 10 ";
                                        OBLoadUploadInvalidData objOBLoadUploadInvalidData = new OBLoadUploadInvalidData();
                                        objOBLoadUploadInvalidData.batch_num = objOBLoadUploadDtl.batch_num;
                                        objOBLoadUploadInvalidData.line_num = objOBLoadUploadDtl.dtl_line;
                                        objOBLoadUploadInvalidData.error_desc = l_str_error_desc;
                                        objOBLoadUploadInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOBLoadUploadInvalidData.Add(objOBLoadUploadInvalidData);
                                        continue;
                                    }
                                    if (lst_csv_data[(int)GrdLoad.STORE].Trim().Length > 10)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Store should be maximum of 10 ";
                                        OBLoadUploadInvalidData objOBLoadUploadInvalidData = new OBLoadUploadInvalidData();
                                        objOBLoadUploadInvalidData.batch_num = objOBLoadUploadDtl.batch_num;
                                        objOBLoadUploadInvalidData.line_num = objOBLoadUploadDtl.dtl_line;
                                        objOBLoadUploadInvalidData.error_desc = l_str_error_desc;
                                        objOBLoadUploadInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOBLoadUploadInvalidData.Add(objOBLoadUploadInvalidData);
                                        continue;
                                    }


                                    if (lst_csv_data[(int)GrdLoad.LOADNUM].Trim().Length > 20)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Load Number should be maximum of 20 ";
                                        OBLoadUploadInvalidData objOBLoadUploadInvalidData = new OBLoadUploadInvalidData();
                                        objOBLoadUploadInvalidData.batch_num = objOBLoadUploadDtl.batch_num;
                                        objOBLoadUploadInvalidData.line_num = objOBLoadUploadDtl.dtl_line;
                                        objOBLoadUploadInvalidData.error_desc = l_str_error_desc;
                                        objOBLoadUploadInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOBLoadUploadInvalidData.Add(objOBLoadUploadInvalidData);
                                        continue;
                                    }

                                    else if (lst_csv_data[(int)GrdLoad.LOADNUM].Trim().Length == 0)
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + "- Load Number Should Not be blank ";
                                        OBLoadUploadInvalidData objOBLoadUploadInvalidData = new OBLoadUploadInvalidData();
                                        objOBLoadUploadInvalidData.batch_num = objOBLoadUploadDtl.batch_num;
                                        objOBLoadUploadInvalidData.line_num = objOBLoadUploadDtl.dtl_line;
                                        objOBLoadUploadInvalidData.error_desc = l_str_error_desc;
                                        objOBLoadUploadInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOBLoadUploadInvalidData.Add(objOBLoadUploadInvalidData);
                                        continue;
                                    }

                                    if (lst_csv_data[(int)GrdLoad.LOAD_APPROVE_DT].Trim().Length != 10)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - LOAD-Approve-Dt should be Of 10 ";
                                        OBLoadUploadInvalidData objOBLoadUploadInvalidData = new OBLoadUploadInvalidData();
                                        objOBLoadUploadInvalidData.batch_num = objOBLoadUploadDtl.batch_num;
                                        objOBLoadUploadInvalidData.line_num = objOBLoadUploadDtl.dtl_line;
                                        objOBLoadUploadInvalidData.error_desc = l_str_error_desc;
                                        objOBLoadUploadInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOBLoadUploadInvalidData.Add(objOBLoadUploadInvalidData);
                                        continue;
                                    }

                                    else if (lst_csv_data[(int)GrdLoad.LOAD_APPROVE_DT].Trim().Length == 0)
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + "- Load Approve Date Should Not be blank ";
                                        OBLoadUploadInvalidData objOBLoadUploadInvalidData = new OBLoadUploadInvalidData();
                                        objOBLoadUploadInvalidData.batch_num = objOBLoadUploadDtl.batch_num;
                                        objOBLoadUploadInvalidData.line_num = objOBLoadUploadDtl.dtl_line;
                                        objOBLoadUploadInvalidData.error_desc = l_str_error_desc;
                                        objOBLoadUploadInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOBLoadUploadInvalidData.Add(objOBLoadUploadInvalidData);
                                        continue;
                                    }



                                    if (lst_csv_data[(int)GrdLoad.CARRIER].Length > 20)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - CARRIER length should be maximum of 20 ";

                                        OBLoadUploadInvalidData objOBLoadUploadInvalidData = new OBLoadUploadInvalidData();
                                        objOBLoadUploadInvalidData.batch_num = objOBLoadUploadDtl.batch_num;
                                        objOBLoadUploadInvalidData.line_num = objOBLoadUploadDtl.dtl_line;
                                        objOBLoadUploadInvalidData.error_desc = l_str_error_desc;
                                        objOBLoadUploadInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOBLoadUploadInvalidData.Add(objOBLoadUploadInvalidData);
                                        continue;
                                    }


                                    if (lst_csv_data[(int)GrdLoad.CARRIER_PICK_DT].Length > 20)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - CARRIER PICK DT length should be of 10 ";

                                        OBLoadUploadInvalidData objOBLoadUploadInvalidData = new OBLoadUploadInvalidData();
                                        objOBLoadUploadInvalidData.batch_num = objOBLoadUploadDtl.batch_num;
                                        objOBLoadUploadInvalidData.line_num = objOBLoadUploadDtl.dtl_line;
                                        objOBLoadUploadInvalidData.error_desc = l_str_error_desc;
                                        objOBLoadUploadInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOBLoadUploadInvalidData.Add(objOBLoadUploadInvalidData);
                                        continue;
                                    }


                                    l_int_line_num = l_int_line_num + 1;
                                    objOBLoadUploadDtl.dtl_line = l_int_line_num;
                                    objOBLoadUploadDtl.cmp_id = lst_csv_data[(int)GrdLoad.CMP_ID].Trim().ToUpper();
                                    objOBLoadUploadDtl.batch_num = lst_csv_data[(int)GrdLoad.BATCH_ID].Trim().ToUpper();
                                    objOBLoadUploadDtl.cust_po = lst_csv_data[(int)GrdLoad.CUST_PO].Trim().ToUpper();
                                    objOBLoadUploadDtl.dept_id = lst_csv_data[(int)GrdLoad.DEPT].Trim().ToUpper();
                                    objOBLoadUploadDtl.store_id = lst_csv_data[(int)GrdLoad.STORE].Trim().ToUpper();

                                    objOBLoadUploadDtl.proc_status = "PEND";
                                    objOBLoadUploadDtl.load_number = lst_csv_data[(int)GrdLoad.LOADNUM].Trim().ToUpper();

                                    if (CheckDate(lst_csv_data[(int)GrdLoad.LOAD_APPROVE_DT]))
                                    {
                                        try
                                        {
                                            objOBLoadUploadDtl.load_approve_dt = lst_csv_data[(int)GrdLoad.LOAD_APPROVE_DT].Trim();
                                        }
                                        catch
                                        {
                                            objOBLoadUploadDtl.load_approve_dt = DateTime.Now.ToString("MM/dd/yyyy");
                                        }
                                    }

                                    else
                                    {
                                        objOBLoadUploadDtl.load_approve_dt = DateTime.Now.ToString("MM/dd/yyyy");

                                    }


                                    objOBLoadUploadDtl.carrier_name = lst_csv_data[(int)GrdLoad.CARRIER].Trim().ToUpper();

                                    if (CheckDate(lst_csv_data[(int)GrdLoad.CARRIER_PICK_DT]))
                                    {
                                        try
                                        {
                                            objOBLoadUploadDtl.load_pick_dt = lst_csv_data[(int)GrdLoad.CARRIER_PICK_DT].Trim().ToUpper();
                                        }
                                        catch
                                        {
                                            objOBLoadUploadDtl.load_pick_dt = DateTime.Now.ToString("MM/dd/yyyy");
                                        }
                                    }

                                    else
                                    {
                                        objOBLoadUploadDtl.load_pick_dt = DateTime.Now.ToString("MM/dd/yyyy");

                                    }


                                    try
                                    {
                                        objOBLoadUploadDtl.tot_weight = Convert.ToDecimal(lst_csv_data[(int)GrdLoad.WGT].Trim());
                                    }
                                    catch
                                    {
                                        objOBLoadUploadDtl.tot_weight = 0;
                                    }
                                    try
                                    {
                                        objOBLoadUploadDtl.tot_cube = Convert.ToDecimal(lst_csv_data[(int)GrdLoad.CUBE].Trim());
                                    }
                                    catch
                                    {
                                        objOBLoadUploadDtl.tot_cube = 0;

                                    }
                                    objOBLoadUploadDtl.tot_palet = 0;

                                    objOBLoadUploadDtl.upload_ref_num = l_str_upload_ref_num;
                                    string l_str_so_num = string.Empty;
                                    l_str_so_num = ServiceOBLoadEntryUploadFile.CheckBatchCustPOExists(p_str_cmp_id,
                                        objOBLoadUploadDtl.batch_num, objOBLoadUploadDtl.cust_po, objOBLoadUploadDtl.dept_id, objOBLoadUploadDtl.store_id);

                                    if (l_str_so_num == string.Empty)
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " SR Number Not found for the Batch/Cust PO/Dept/Store ";

                                        OBLoadUploadInvalidData objOBLoadUploadInvalidData = new OBLoadUploadInvalidData();
                                        objOBLoadUploadInvalidData.batch_num = objOBLoadUploadDtl.batch_num;
                                        objOBLoadUploadInvalidData.line_num = objOBLoadUploadDtl.dtl_line;
                                        objOBLoadUploadInvalidData.error_desc = l_str_error_desc;
                                        objOBLoadUploadInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOBLoadUploadInvalidData.Add(objOBLoadUploadInvalidData);
                                    }
                                    else if (l_str_so_num == "DUPLICATE")
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " Multiple SR Number Exists for the Batch/Cust PO/Dept/Store ";

                                        OBLoadUploadInvalidData objOBLoadUploadInvalidData = new OBLoadUploadInvalidData();
                                        objOBLoadUploadInvalidData.batch_num = objOBLoadUploadDtl.batch_num;
                                        objOBLoadUploadInvalidData.line_num = objOBLoadUploadDtl.dtl_line;
                                        objOBLoadUploadInvalidData.error_desc = l_str_error_desc;
                                        objOBLoadUploadInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOBLoadUploadInvalidData.Add(objOBLoadUploadInvalidData);
                                    }

                                    else
                                    {
                                        objOBLoadUploadDtl.maker = Session["UserID"].ToString().Trim();
                                        objOBLoadUploadDtl.maker_dt = DateTime.Now.ToString("MM/dd/yyyy");
                                        objOBLoadUploadDtl.so_num = l_str_so_num;
                                        lstOBLoadUploadDtl.Add(objOBLoadUploadDtl);
                                    }


                                }

                                else
                                {
                                    if (l_int_cur_line < l_int_no_of_lines)
                                        l_str_error_msg = "Invalid File Format";
                                    continue;
                                }
                            }

                        }



                        Session["lstOBLoadUploadDtl"] = lstOBLoadUploadDtl;
                        Session["lstOBLoadUploadInvalidData"] = lstOBLoadUploadInvalidData;
                        // Session["objOBLoadUploadFileInfo"] = "";



                        if (lstOBLoadUploadInvalidData.Count > 0)
                        {

                            l_str_error_msg = "ERROR";
                            Session["l_str_error_msg"] = "ERROR";
                        }

                    }
                    else
                    {
                        l_str_error_msg = "Invalid File Format";
                    }
                }
            }
            catch (Exception ex)
            {
                l_str_error_msg = ex.InnerException.ToString();
            }
        }

        [HttpPost]
        public JsonResult SaveLoadEntryBatch(string p_str_cmp_id, string p_str_file_name)
        {
            string l_str_error_code = string.Empty;
            DataTable dt_ob_load_batch_dtl = new DataTable();
            DataTable dt_ob_load_file_info = new DataTable();
            OBLoadUploadFile objOBLoadUploadFile = new OBLoadUploadFile();
            OBLoadEntryUploadFileService ServiceOBLoadEntryUploadFile = new OBLoadEntryUploadFileService();
            string user_id = string.Empty;
            user_id = Session["UserID"].ToString();

            List<OBLoadUploadDtl> lstOBLoadUploadDtl = new List<OBLoadUploadDtl>();
            lstOBLoadUploadDtl = Session["lstOBLoadUploadDtl"] as List<OBLoadUploadDtl>;
            dt_ob_load_batch_dtl = Utility.ConvertListToDataTable(lstOBLoadUploadDtl);

            OBLoadUploadFileInfo objOBLoadUploadFileInfo = new OBLoadUploadFileInfo();
            objOBLoadUploadFileInfo = Session["objOBLoadUploadFileInfo"] as OBLoadUploadFileInfo;
            objOBLoadUploadFileInfo.file_name = p_str_file_name;
            objOBLoadUploadFileInfo.upload_by = user_id;
            objOBLoadUploadFileInfo.no_of_lines = lstOBLoadUploadDtl.Count;
            objOBLoadUploadFileInfo.status = "PEND";

            dt_ob_load_file_info = Utility.ObjectToDataTable(objOBLoadUploadFileInfo);


            if (ServiceOBLoadEntryUploadFile.SaveOBLoadEntryBatch(p_str_cmp_id, dt_ob_load_file_info, dt_ob_load_batch_dtl, user_id) == true)
            {
                Session["lstOBLoadUploadDtl"] = "";
                Session["lstOBLoadUploadInvalidData"] = "";
                Session["objOBLoadUploadFileInfo"] = "";
                Session["l_str_error_msg"] = "";
                ViewBag.l_int_error_count = "0";
                objOBLoadUploadFile.error_desc = "NO";
            }
            else
            {
                Session["l_str_error_msg"] = "Error";
            }

            string path = Path.Combine(Server.MapPath("~/uploads"), p_str_file_name);
            string path2 = Path.Combine(Server.MapPath("~/uploads/zProcessed940"), p_str_file_name);
            DirectoryInfo dirInfo = new DirectoryInfo(path2);
            if (!System.IO.File.Exists(path2))
            {
                System.IO.File.Move(path, path2);
            }
            else
            {
                string l_str_FileNameOnly = p_str_file_name.Substring(0, p_str_file_name.LastIndexOf("."));
                string path3 = Path.Combine(Server.MapPath("~/uploads/zProcessed940"), l_str_FileNameOnly + "-" + DateTime.Now.ToString("yyyyMMddTHHmmss") + ".csv");
                System.IO.File.Move(path, path3);
            }


            lstOBLoadUploadDtl.Clear();
            Mapper.CreateMap<OBLoadUploadFile, OBLoadUploadFileModel>();
            OBLoadUploadFileModel objOBLoadUploadFileModel = Mapper.Map<OBLoadUploadFile, OBLoadUploadFileModel>(objOBLoadUploadFile);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        protected bool CheckDate(String date)

        {

            try

            {

                DateTime dt = DateTime.Parse(date);

                return true;

            }
            catch

            {

                return false;

            }

        }
        public ActionResult LoadEntryBOLRpt(string p_str_cmp_id, string p_str_load_doc_id, string p_str_load_number)
        {
            string strReportName = string.Empty;
            string jsonErrorCode = string.Empty;
            string msg = string.Empty;
            try
            {
                strReportName = "rpt_ob_sr_bol_conf_by_load_number.rpt";
                OBGetSRBOLConfRpt objOBGetBOLConf = new OBGetSRBOLConfRpt();
                OBSRLoadEntryService ServiceObject = new OBSRLoadEntryService();

                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                objOBGetBOLConf.cmp_id = p_str_cmp_id;
                objOBGetBOLConf.load_number = p_str_load_number;

                objOBGetBOLConf = ServiceObject.GetOBSRBOLConfRptByLoadNumber(objOBGetBOLConf, p_str_cmp_id, p_str_load_doc_id, p_str_load_number);
                //CR20180623-001

                if (objOBGetBOLConf.ListOBGetSRBOLConfRpt.Count > 0)
                {


                }
                else
                {

                    Response.Write("<H2>Report not found</H2>");
                }


                var rptSource = objOBGetBOLConf.ListOBGetSRBOLConfRpt.ToList();
                if (rptSource.Count > 0)
                {
                    using (ReportDocument rd = new ReportDocument())
                    {

                        rd.Load(strRptPath);

                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        rd.SetParameterValue("param_cmp_name", System.Configuration.ConfigurationManager.AppSettings["rptCmpName"].ToString().Trim());
                        rd.SetParameterValue("param_cmp_address", System.Configuration.ConfigurationManager.AppSettings["rptCmpAddress"].ToString().Trim());
                        rd.SetParameterValue("param_cmp_city_state_zip", System.Configuration.ConfigurationManager.AppSettings["rptCmpCityStateZzip"].ToString().Trim());

                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
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



        private static int TotalPageCount(string file)
        {
            using (StreamReader sr = new StreamReader(System.IO.File.OpenRead(file)))
            {
                Regex regex = new Regex(@"/Type\s*/Page[^s]");
                MatchCollection matches = regex.Matches(sr.ReadToEnd());

                return matches.Count;
            }
        }
        public ActionResult BOLBatchRpt(string p_str_cmp_id, string p_str_load_doc_id, string p_str_is_same_ship_to, string p_str_print_summary, string p_str_incld_batch_print)
        {
            string strReportName = string.Empty;
            string jsonErrorCode = string.Empty;
            string msg = string.Empty;
            OBSRLoadEntryService ServiceObject = new OBSRLoadEntryService();
            int pintOrdrCount = (int)Session["pintOrdrCount"];
            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
            string l_str_dt = string.Empty;
            string l_str_file_1 = string.Empty;
            string l_str_file_2 = string.Empty;
            string l_str_file_3 = string.Empty;
            string l_str_bol_file_name = string.Empty;
            l_str_dt = DateTime.Now.ToString("yyyyMMddHHssmm");

            try
            {
                if (p_str_print_summary == "S")
                {


                    strReportName = "rpt_bol_by_batch.rpt";
                    OBGetSRBOLConfRpt objOBGetBOLConf = new OBGetSRBOLConfRpt();
                   

                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                    objOBGetBOLConf.cmp_id = p_str_cmp_id;

                    objOBGetBOLConf = ServiceObject.GetOBBOLByBatch(objOBGetBOLConf, p_str_cmp_id, p_str_load_doc_id, p_str_is_same_ship_to);
                    if (objOBGetBOLConf.ListOBGetSRBOLConfRpt.Count > 0)
                    {
                    }
                    else
                    {
                        Response.Write("<H2>Report not found</H2>");
                    }
                    var rptSource = objOBGetBOLConf.ListOBGetSRBOLConfRpt.ToList();
                    if (rptSource.Count > 0)
                    {
                        using (ReportDocument rd = new ReportDocument())
                        {

                            rd.Load(strRptPath);

                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            rd.SetParameterValue("param_cmp_name", System.Configuration.ConfigurationManager.AppSettings["rptCmpName"].ToString().Trim());
                            rd.SetParameterValue("param_cmp_address", System.Configuration.ConfigurationManager.AppSettings["rptCmpAddress"].ToString().Trim());
                            rd.SetParameterValue("param_cmp_city_state_zip", System.Configuration.ConfigurationManager.AppSettings["rptCmpCityStateZzip"].ToString().Trim());

                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                    }
                }

                else
                { 
                if (pintOrdrCount <=6)
                {

                    strReportName = "rpt_ob_sr_bol_conf_by_load_number.rpt";
                    OBGetSRBOLConfRpt objOBGetBOLConf = new OBGetSRBOLConfRpt();

                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                    objOBGetBOLConf.cmp_id = p_str_cmp_id;

                    objOBGetBOLConf = ServiceObject.GetOBBOLByBatch(objOBGetBOLConf, p_str_cmp_id, p_str_load_doc_id, p_str_is_same_ship_to);
                    if (objOBGetBOLConf.ListOBGetSRBOLConfRpt.Count > 0)
                    {
                    }
                    else
                    {
                        Response.Write("<H2>Report not found</H2>");
                    }
                    var rptSource = objOBGetBOLConf.ListOBGetSRBOLConfRpt.ToList();
                    if (rptSource.Count > 0)
                    {
                        using (ReportDocument rd = new ReportDocument())
                        {

                            rd.Load(strRptPath);

                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            rd.SetParameterValue("param_cmp_name", System.Configuration.ConfigurationManager.AppSettings["rptCmpName"].ToString().Trim());
                            rd.SetParameterValue("param_cmp_address", System.Configuration.ConfigurationManager.AppSettings["rptCmpAddress"].ToString().Trim());
                            rd.SetParameterValue("param_cmp_city_state_zip", System.Configuration.ConfigurationManager.AppSettings["rptCmpCityStateZzip"].ToString().Trim());
                            if ((p_str_incld_batch_print == "Y") && (p_str_print_summary != "S") && (pintOrdrCount > 1))
                            {
                                l_str_file_1 = strOutputpath + p_str_load_doc_id + "1" + l_str_dt + ".pdf";
                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, l_str_file_1);
                            }
                            else
                            {
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            }
                                
                        }
                    }
                }
                else
                {
                    strReportName = "rpt_bol_with_summary_1.rpt";
                    string strReportName2 = string.Empty;
                    strReportName2 = "rpt_bol_with_summary_2.rpt";
                    OBGetSRBOLConfRpt objOBGetBOLConf = new OBGetSRBOLConfRpt();

                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                    string strRptPath2 = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName2;
                    objOBGetBOLConf.cmp_id = p_str_cmp_id;

                    objOBGetBOLConf = ServiceObject.GetOBBOLByBatch(objOBGetBOLConf, p_str_cmp_id, p_str_load_doc_id, p_str_is_same_ship_to);
                    if (objOBGetBOLConf.ListOBGetSRBOLConfRpt.Count > 0)
                    {

                    }
                    else
                    {
                        Response.Write("<H2>Report not found</H2>");
                    }
                    var rptSource = objOBGetBOLConf.ListOBGetSRBOLConfRpt.ToList();
         
                    if (rptSource.Count > 0)
                    {
                        using (ReportDocument rd = new ReportDocument())
                        {

                            rd.Load(strRptPath);

                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            rd.SetParameterValue("param_cmp_name", System.Configuration.ConfigurationManager.AppSettings["rptCmpName"].ToString().Trim());
                            rd.SetParameterValue("param_cmp_address", System.Configuration.ConfigurationManager.AppSettings["rptCmpAddress"].ToString().Trim());
                            rd.SetParameterValue("param_cmp_city_state_zip", System.Configuration.ConfigurationManager.AppSettings["rptCmpCityStateZzip"].ToString().Trim());
                            rd.SetParameterValue("parmBOLDate", 0);
                            rd.SetParameterValue("parmTotalWeight", 0);
                            rd.SetParameterValue("parmTotalCtns", 0);
                            l_str_file_1 = strOutputpath + p_str_load_doc_id + "1" + l_str_dt + ".pdf";
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, l_str_file_1);
                        }


                        using (ReportDocument rd = new ReportDocument())
                        {

                            rd.Load(strRptPath2);

                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            rd.SetParameterValue("param_cmp_name", System.Configuration.ConfigurationManager.AppSettings["rptCmpName"].ToString().Trim());
                            rd.SetParameterValue("param_cmp_address", System.Configuration.ConfigurationManager.AppSettings["rptCmpAddress"].ToString().Trim());
                            rd.SetParameterValue("param_cmp_city_state_zip", System.Configuration.ConfigurationManager.AppSettings["rptCmpCityStateZzip"].ToString().Trim());
                            rd.SetParameterValue("parmBOLDate", 0);
                            rd.SetParameterValue("parmTotalWeight", 0);
                            rd.SetParameterValue("parmTotalCtns", 0);
                            l_str_file_2 = strOutputpath + p_str_load_doc_id + "2" + l_str_dt + ".pdf";
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, l_str_file_2);
                        }
                       
                        }
                    }
                    if ((p_str_incld_batch_print == "Y") && (p_str_print_summary != "S") && (pintOrdrCount > 1))
                        {
                            strReportName = "rpt_bol_by_batch.rpt";
                            OBGetSRBOLConfRpt objOBGetBOLConfBatch = new OBGetSRBOLConfRpt();


                            string strRptPath1 = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                            objOBGetBOLConfBatch.cmp_id = p_str_cmp_id;

                            objOBGetBOLConfBatch = ServiceObject.GetOBBOLByBatch(objOBGetBOLConfBatch, p_str_cmp_id, p_str_load_doc_id, p_str_is_same_ship_to);
                            if (objOBGetBOLConfBatch.ListOBGetSRBOLConfRpt.Count > 0)
                            {
                            }
                            else
                            {
                                Response.Write("<H2>Report not found</H2>");
                            }
                            var rptSource1 = objOBGetBOLConfBatch.ListOBGetSRBOLConfRpt.ToList();
                            if (rptSource1.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {

                                    rd.Load(strRptPath1);

                                    if (rptSource1 != null && rptSource1.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource1);
                                    rd.SetParameterValue("param_cmp_name", System.Configuration.ConfigurationManager.AppSettings["rptCmpName"].ToString().Trim());
                                    rd.SetParameterValue("param_cmp_address", System.Configuration.ConfigurationManager.AppSettings["rptCmpAddress"].ToString().Trim());
                                    rd.SetParameterValue("param_cmp_city_state_zip", System.Configuration.ConfigurationManager.AppSettings["rptCmpCityStateZzip"].ToString().Trim());
                                    if (l_str_file_2.Length >0)
                                    {
                                        l_str_file_3 = strOutputpath + p_str_load_doc_id + "3" + l_str_dt + ".pdf";
                                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, l_str_file_3);
                                    }
                                    else
                                    {
                                        l_str_file_2 = strOutputpath + p_str_load_doc_id + "2" + l_str_dt + ".pdf";
                                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, l_str_file_2);
                                    }
                                   
                                  
                                }
                            }
                        }

                    l_str_bol_file_name = strOutputpath + p_str_load_doc_id + "-" + l_str_dt + ".pdf";
                   
                    MergePDF(l_str_file_1, l_str_file_2, l_str_file_3, l_str_bol_file_name);
                        return File(l_str_bol_file_name, "application/pdf");
                
                }
            }

            catch (Exception ex)
            {
                msg = ex.Message;
                jsonErrorCode = "-2";
            }

            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);
        }

        private static void MergePDF(string File1, string File2, string File3, string outFileName)
        {
            string[] fileArray;

          
            if (File3.Length == 0)
            {
                fileArray = new string[3];
                fileArray[0] = File1;
                fileArray[1] = File2;
            }
            else
            {
                fileArray = new string[4];
                fileArray[0] = File1;
                fileArray[1] = File2;
                fileArray[2] = File3;
            }
               

            PdfReader reader = null;
            Document sourceDocument = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage;
            string outputPdfPath = outFileName;

            sourceDocument = new Document();
            pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));

            //output file Open  
            sourceDocument.Open();


            //files list wise Loop  
            for (int f = 0; f < fileArray.Length - 1; f++)
            {
              
                    int pages = TotalPageCount(fileArray[f]);

                    reader = new PdfReader(fileArray[f]);
                    //Add pages in new file  
                    for (int i = 1; i <= pages; i++)
                    {
                        importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                        pdfCopyProvider.AddPage(importedPage);
                    }
                         

                reader.Close();
            }
            //save the output file  
            sourceDocument.Close();

        }
        private static void ConvertImageToPDF(string pstrFolderName, string pdfFileName)
        {
            iTextSharp.text.Rectangle pgSize = new iTextSharp.text.Rectangle(800, 1200);
            Document doc = new Document(pgSize, 0, 0, 0, 0);
            try

            {

                DirectoryInfo di = new DirectoryInfo(pstrFolderName);
              ///  var orderedFiles = Directory.GetFiles("*.gif").Select(f => new FileInfo(f)).OrderBy(f => f.CreationTime);
                FileInfo[] Images = di.GetFiles("*.gif");
                int i = 0;
                PdfWriter.GetInstance(doc, new FileStream(pdfFileName, FileMode.Create));
                doc.Open();
                
                for (i = 0; i < Images.Length; i++)
                {
                    string gifFileName = Images[i].FullName;
                    iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(gifFileName);
                    gif.RotationDegrees = -90f;
                    doc.Add(gif);
                    if (i < Images.Length - 1)
                    {
                        doc.NewPage();
                    }
                }


                //string gifFileName;

                //iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(gifFileName);
                //gif.RotationDegrees = -90f;


                //PdfWriter.GetInstance(doc, new FileStream(pdfFileName, FileMode.Create));

                //doc.Open();
                //doc.Add(gif);

            }

            catch (Exception ex)

            {
            }

            finally

            {

                doc.Close();

            }
        }
        public ActionResult GetAlocPostByBatch(string p_str_cmp_id, string p_str_batch_num, string p_str_load_number,
            string p_str_so_num_from, string p_str_so_num_to, string p_str_so_dt_from, string p_str_so_dt_to)
        {
            OBAlocPostInquiry objOBAlocPostInquiry = new OBAlocPostInquiry();
            OutboundInqService ServiceObject = new OutboundInqService();
            CompanyService ServiceObjectCompany = new CompanyService();
            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = p_str_cmp_id;
            objOBAlocPostInquiry.cmp_id = p_str_cmp_id;
            objOBAlocPostInquiry.batch_num = p_str_batch_num;
            objOBAlocPostInquiry.load_number = p_str_load_number;
            objOBAlocPostInquiry.so_num_from = p_str_so_num_from;
            objOBAlocPostInquiry.so_num_to = p_str_so_num_to;
            objOBAlocPostInquiry.so_dt_from = p_str_so_dt_from;
            objOBAlocPostInquiry.so_dt_to = p_str_so_dt_to;
            objOBAlocPostInquiry = ServiceObject.GetOBAlocOpenList(objOBAlocPostInquiry);
            Company objCompany = new Company();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objOBAlocPostInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objCompany.cmp_id = p_str_cmp_id;
            Mapper.CreateMap<OBAlocPostInquiry, OBAlocPostInquiryModel>();
            OBAlocPostInquiryModel objOBAlocPostInquiryModel = Mapper.Map<OBAlocPostInquiry, OBAlocPostInquiryModel>(objOBAlocPostInquiry);
            return PartialView("_AlocPostByBatch", objOBAlocPostInquiryModel);
        }

        public ActionResult GetAlocPostByBatchSearch(string p_str_cmp_id, string p_str_batch_num, string p_str_load_number,
           string p_str_so_num_from, string p_str_so_num_to, string p_str_so_dt_from, string p_str_so_dt_to)
        {

            OBAlocPostInquiry objOBAlocPostInquiry = new OBAlocPostInquiry();
            OutboundInqService ServiceObject = new OutboundInqService();
            CompanyService ServiceObjectCompany = new CompanyService();
            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = p_str_cmp_id;
            objOBAlocPostInquiry.cmp_id = p_str_cmp_id;
            objOBAlocPostInquiry.batch_num = p_str_batch_num;
            objOBAlocPostInquiry.load_number = p_str_load_number;
            objOBAlocPostInquiry.so_num_from = p_str_so_num_from;
            objOBAlocPostInquiry.so_num_to = p_str_so_num_to;
            objOBAlocPostInquiry.so_dt_from = p_str_so_dt_from;
            objOBAlocPostInquiry.so_dt_to = p_str_so_dt_to;
            objOBAlocPostInquiry = ServiceObject.GetOBAlocOpenList(objOBAlocPostInquiry);
            Company objCompany = new Company();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objOBAlocPostInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objCompany.cmp_id = p_str_cmp_id;
            Mapper.CreateMap<OBAlocPostInquiry, OBAlocPostInquiryModel>();
            OBAlocPostInquiryModel objOBAlocPostInquiryModel = Mapper.Map<OBAlocPostInquiry, OBAlocPostInquiryModel>(objOBAlocPostInquiry);
            return PartialView("_AlocPostByBatchGrid", objOBAlocPostInquiryModel);
        }
        public JsonResult PostAlocPostByBatch(string p_str_cmp_id, string p_str_sel_aloc_doc_id_list, string p_str_aloc_post_dt)
        {
            string l_str_error_code = string.Empty;
            string[] strAlocList;
            string l_str_load_number = string.Empty;
            DataTable dtAlocList = new DataTable();
            OBAlocPostInquiry objOBAlocPostInquiry = new OBAlocPostInquiry();
            OutboundInqService ServiceObject = new OutboundInqService();
            int l_int_aloc_tmp_ref_no = 0;

            l_int_aloc_tmp_ref_no = ServiceObject.GetAlocPostRefNo();
            dtAlocList.Columns.Add("req_num", typeof(string));
            dtAlocList.Columns.Add("cmp_id", typeof(string));
            dtAlocList.Columns.Add("aloc_doc_id", typeof(string));

            dtAlocList.Columns.Add("aloc_post_dt", typeof(DateTime));
            dtAlocList.Columns.Add("post_status", typeof(string));
            dtAlocList.Columns.Add("maker", typeof(string));
            dtAlocList.Columns.Add("maker_dt", typeof(DateTime));

            if (p_str_sel_aloc_doc_id_list.Length > 0)
            {
                strAlocList = p_str_sel_aloc_doc_id_list.Split(',');

                for (int i = 0; i < strAlocList.Length; i++)
                {

                    if (ServiceObject.fnGenerateIBfromOB(p_str_cmp_id, string.Empty, strAlocList[i].ToString()) == false)
                    {

                    }
                    DataRow dtSoRow = dtAlocList.NewRow();
                    dtSoRow[0] = l_int_aloc_tmp_ref_no;
                    dtSoRow[1] = p_str_cmp_id;
                    dtSoRow[2] = strAlocList[i].ToString();
                    dtSoRow[3] = p_str_aloc_post_dt.ToString();
                    dtSoRow[4] = "P";
                    dtSoRow[5] = Session["UserID"].ToString().Trim();
                    dtSoRow[6] = DateTime.Now.ToString("MM/dd/yyyy");


                    dtAlocList.Rows.Add(dtSoRow);
                }
            }
            else
            {
                l_str_error_code = "No SO# selected";
                return Json(l_str_error_code, JsonRequestBehavior.AllowGet);
            }

            //string user_id = string.Empty;
            //user_id = Session["UserID"].ToString();
            if (ServiceObject.SaveOBBulkAlocPost(p_str_cmp_id, l_int_aloc_tmp_ref_no.ToString(), dtAlocList) == true)
            {

            }

            //Mapper.CreateMap<OBSRLoadInquiry, OBSRLoadInquiryModel>();
            //OBSRLoadInquiryModel objOBSRLoadInquiryModel = Mapper.Map<OBSRLoadInquiry, OBSRLoadInquiryModel>(objOBSRLoadInquiry);
            //objOBSRLoadEntryHdr.Clear();

            return Json("", JsonRequestBehavior.AllowGet);
        }


        public JsonResult On_ChangeShipToDetails(string id, string p_str_cmp_id)
        {
            try
            {
                OutboundInq objOutboundInq = new OutboundInq();
                OutboundInqService ServiceObject = new OutboundInqService();
                Pick objPick = new Pick();
                PickService ServiceObjectPick = new PickService();
                //objPick.cmp_id = Session["tempcmpid"].ToString().Trim();
                objPick.cmp_id = p_str_cmp_id;
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
        public JsonResult On_ChangeCountryDetails(string id, string p_str_cmp_id)
        {
            try
            {
                OutboundInq objOutboundInq = new OutboundInq();
                OutboundInqService ServiceObject = new OutboundInqService();
                Pick objPick = new Pick();
                PickService ServiceObjectPick = new PickService();
                //objPick.cmp_id = Session["tempcmpid"].ToString().Trim();
                objPick.cmp_id = p_str_cmp_id;
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
        public FileResult SampleTemplatedownload()
        {
            return File("~\\templates\\OB_DETAIL_UPLOAD_TEMPLATE_WITH_SAMPLE.xlsx", "text/xls", string.Format("OB_DETAIL_UPLOAD_TEMPLATE_WITH_SAMPLE-{0}.xlsx", DateTime.Now.ToString("yyyyMMdd-HHmmss")));
        }
        public JsonResult CheckOBCustPOExists(string pstrCmpId, string pstrCustOrdrNum)
        {
            int lintCheckPoExists = 0;
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            lintCheckPoExists = ServiceObject.fnCheckOBCustPOExists(pstrCmpId, pstrCustOrdrNum);
           
            return Json(lintCheckPoExists, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadAvailQty(string p_str_cmp_id, string p_str_Itmcode, string p_str_style, string p_str_color, string p_str_size, string p_str_vendpo)
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
            objOutboundInq.po_num = p_str_vendpo;
            objOutboundInq = ServiceObject.OutboundGETALOCSORTSTMT(objOutboundInq);
            if (objOutboundInq.LstItmxCustdtl.Count > 0)
            {
                objOutboundInq.aloc_sort_stmt = objOutboundInq.LstItmxCustdtl[0].aloc_sort_stmt;
                strAlocSortStmt = objOutboundInq.aloc_sort_stmt;
                objOutboundInq.aloc_by = objOutboundInq.LstItmxCustdtl[0].aloc_by;
                strAlocBy = objOutboundInq.aloc_by;
            }
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
                objOutboundInq.itm_code = objOutboundInq.LstItmxCustdtl[0].itm_code;
                objOutboundInq.LstItmxCustdtl[0].avail_qty = Convert.ToDecimal(avlqty);
                objOutboundInq.length = objOutboundInq.LstItmxCustdtl[0].length;
                objOutboundInq.width = objOutboundInq.LstItmxCustdtl[0].width;
                objOutboundInq.height = objOutboundInq.LstItmxCustdtl[0].depth;
                objOutboundInq.cube = objOutboundInq.LstItmxCustdtl[0].cube;
                objOutboundInq.weight = objOutboundInq.LstItmxCustdtl[0].wgt;
                objOutboundInq.ppk = objOutboundInq.LstItmxCustdtl[0].ctn_qty;
                p_str_Itmcode = objOutboundInq.LstItmxCustdtl[0].itm_code;
                GetAvailQty(ref AvailQty, p_str_Itmcode, p_str_cmp_id, p_str_vendpo); 
                objOutboundInq.LstItmxCustdtl[0].avail_qty = AvailQty;
                objOutboundInq.avail_qty = objOutboundInq.LstItmxCustdtl[0].avail_qty; 
            }
            return Json(objOutboundInq.LstItmxCustdtl, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadAvailEcomQty(string p_str_cmp_id, string p_str_Itmcode, string p_str_style, string p_str_color, string p_str_size, string p_str_vendpo,string ecom_recv_by_bin)
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
            objOutboundInq.po_num = p_str_vendpo;
            objOutboundInq = ServiceObject.OutboundGETALOCSORTSTMT(objOutboundInq);
            if (objOutboundInq.LstItmxCustdtl.Count > 0)
            {
                objOutboundInq.aloc_sort_stmt = objOutboundInq.LstItmxCustdtl[0].aloc_sort_stmt;
                strAlocSortStmt = objOutboundInq.aloc_sort_stmt;
                objOutboundInq.aloc_by = objOutboundInq.LstItmxCustdtl[0].aloc_by;
                strAlocBy = objOutboundInq.aloc_by;
            }
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

                if (String.IsNullOrEmpty(ecom_recv_by_bin) || ecom_recv_by_bin == "0")
            {
                objOutboundInq = ServiceObject.Getitmlist(objOutboundInq);
            }
            else
            {
                objOutboundInq = ServiceObject.GetEcomitmlist(objOutboundInq);
            }

          

            if (objOutboundInq.LstItmxCustdtl.Count > 0)
            {
                objOutboundInq.itm_code = objOutboundInq.LstItmxCustdtl[0].itm_code;
                objOutboundInq.LstItmxCustdtl[0].avail_qty = Convert.ToDecimal(avlqty);
                objOutboundInq.length = objOutboundInq.LstItmxCustdtl[0].length;
                objOutboundInq.width = objOutboundInq.LstItmxCustdtl[0].width;
                objOutboundInq.height = objOutboundInq.LstItmxCustdtl[0].depth;
                objOutboundInq.cube = objOutboundInq.LstItmxCustdtl[0].cube;
                objOutboundInq.weight = objOutboundInq.LstItmxCustdtl[0].wgt;
                objOutboundInq.ppk = objOutboundInq.LstItmxCustdtl[0].ctn_qty;
                p_str_Itmcode = objOutboundInq.LstItmxCustdtl[0].itm_code;
                GetAvailQty(ref AvailQty, p_str_Itmcode, p_str_cmp_id, p_str_vendpo);
                objOutboundInq.LstItmxCustdtl[0].avail_qty = AvailQty;
                objOutboundInq.avail_qty = objOutboundInq.LstItmxCustdtl[0].avail_qty;
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
            if (objOutboundInq.LstOutboundInqAlocGridLoadDtls.Count > 0)
            {
                Session["l_str_so_num"] = objOutboundInq.LstOutboundInqAlocGridLoadDtls[0].so_num;
            }
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

            if (ServiceObject.fnGenerateIBfromOB(p_str_cmp_id, Session["l_str_so_num"].ToString(), p_str_alocdocid) == false)
            {

            }
            //CR20180813-001 Added By Nithya
            objOutboundInq.cmp_id = p_str_cmp_id;
            objOutboundInq.Sonum = Session["l_str_so_num"].ToString();
            objOutboundInq.aloc_doc_id = p_str_alocdocid;
            objOutboundInq.shipdocid = "";
            objOutboundInq.mode = "ALOC-POST";
            objOutboundInq.maker = Session["UserID"].ToString().Trim();
            objOutboundInq.makerdt = DateTime.Now.ToString("MM/dd/yyyy");
            objOutboundInq.Auditcomment = "Posted";
            objOutboundInq = ServiceObject.Add_To_proc_save_audit_trail(objOutboundInq);
            //END
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
            if (p_str_shipdocId == "" || p_str_shipdocId == null)
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
            if (objOutboundInq.LstAlocUnPostGridLoadDtls.Count > 0)
            {
                Session["l_str_so_num"] = objOutboundInq.LstAlocUnPostGridLoadDtls[0].so_num;
            }
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

                ServiceObject.SaveAlocUnPost(objOutboundInq);
                objOutboundInq.cmp_id = p_str_cmp_id;
                objOutboundInq.Sonum = Session["l_str_so_num"].ToString();
                objOutboundInq.aloc_doc_id = p_str_alocdocid;
                objOutboundInq.shipdocid = "";
                objOutboundInq.mode = "A-UNPOST";
                objOutboundInq.maker = Session["UserID"].ToString().Trim();
                objOutboundInq.makerdt = DateTime.Now.ToString("MM/dd/yyyy");
                objOutboundInq.Auditcomment = "Un Posted";
                objOutboundInq = ServiceObject.Add_To_proc_save_audit_trail(objOutboundInq);
                //END
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
        public ActionResult SaveSoTracking (string pstrCmpId, string pstrSoNum, string pstrTrackNum, string pstrTrackNumType, string pstrMode, string pstrTrackStatus)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            try
            {
                objOutboundInq = ServiceObject.fnSaveSoTracking(pstrMode, pstrCmpId, pstrSoNum, pstrTrackNumType, pstrTrackNum, pstrTrackStatus, DateTime.Now.ToString("MM/dd/yyyy"), "Add");
                objOutboundInq.View_Flag = pstrMode;
                Mapper.CreateMap<OutboundInq, OutboundInqModel>();
                OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
                return PartialView("_ShipRequestTrack", objOutboundInqModel);
            }
            catch (Exception ex)
            {
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            finally
            {

            }

        }
        public ActionResult InsertTemptable(string p_str_cmp_id, string p_str_Sonum, int p_str_LineNum, string p_str_STATUS, string p_str_Itmdtl,
            string p_str_itm_color, string p_str_itm_size, string p_str_itm_name, decimal p_str_avlqty, decimal p_str_ord_qty,
            decimal p_str_ppk, decimal p_str_ctn, string p_str_qty_uom, decimal p_str_length,
            decimal p_str_width, decimal p_str_height, decimal p_str_weight, string p_str_cube, string p_str_vendpo,
            string p_str_note, string p_str_cust_id, string p_str_itm_code, string p_str_Mode, string p_str_aloc_by, string l_str_temp_stk_ref_num)
        {
            try
            {
                OutboundInq objOutboundInq = new OutboundInq();
                OutboundInqService ServiceObject = new OutboundInqService();
                objOutboundInq.LineNum = p_str_LineNum;
                objOutboundInq.cmp_id = p_str_cmp_id;
                objOutboundInq.Sonum = p_str_Sonum;
                objOutboundInq.itm_code = p_str_itm_code;
                objOutboundInq.aloc_by = p_str_aloc_by;
                objOutboundInq.po_num = p_str_vendpo;
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
                    objOutboundInq.aloc_by = p_str_aloc_by;

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
                    objOutboundInq.temp_stk_ref_num = l_str_temp_stk_ref_num;
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

        public ActionResult SaveShipReqEntry(string p_str_cmp_id, string p_str_cust_id, string p_str_batch_id, string p_str_Sonum, string p_str_pricetkt, string p_str_so_dt,
           string p_str_ShipDt, string p_str_CancelDt, string p_str_refno, string p_str_freight_id, string p_str_status, string p_str_Type, string p_str_AuthId
            , string p_str_fob, string p_str_ShipVia, string p_str_shipchrg, string p_str_CustPO, string p_str_CustOrderdt, string p_str_dept_id
            , string p_str_store_id, string p_str_Note, string p_str_shipto_id, string p_str_Attn, string p_str_dc_id, string p_str_Mailname, string p_str_Addr1
            , string p_str_Addr2, string p_str_city, string p_str_state, string p_str_zipcode, string p_str_country, string p_str_radio, string p_str_pick_no, string p_str_ref_no)
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
            objOutboundInq.Batchid = p_str_batch_id;
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
            objOutboundInq.pick_no = p_str_pick_no;
            objOutboundInq.ref_no = p_str_ref_no;
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
                objOutboundInq = ServiceObject.CheckShipToidExist(objOutboundInq);
                if (objOutboundInq.LstFiledtl.Count == 0)
                {
                    ServiceObject.GetShipToAddressSave(objOutboundInq);
                }
            }
            ServiceObject.Add_To_proc_save_so_hdr_excel(objOutboundInq);
            ServiceObject.Add_To_proc_save_so_addr_excel(objOutboundInq);

            //CR20180813-001 Added By Nithya
            objOutboundInq.cmp_id = p_str_cmp_id;
            objOutboundInq.Sonum = p_str_Sonum;
            objOutboundInq.aloc_doc_id = "";
            objOutboundInq.shipdocid = "";
            objOutboundInq.mode = "SR-INUT";
            objOutboundInq.maker = Session["UserID"].ToString().Trim();
            objOutboundInq.makerdt = DateTime.Now.ToString("MM/dd/yyyy");
            objOutboundInq.Auditcomment = "Added new outbound entry";
            objOutboundInq = ServiceObject.Add_To_proc_save_audit_trail(objOutboundInq);
            //END

            objOutboundInq = ServiceObject.LoadCustConfig(objOutboundInq);

            string lstrFormName = string.Empty;

            if (String.IsNullOrEmpty(objOutboundInq.objCustConfig.ecom_recv_by_bin) ==true || objOutboundInq.objCustConfig.ecom_recv_by_bin == "0" || objOutboundInq.objCustConfig.ecom_recv_by_bin == "N")
            {
                lstrFormName = "_ShipReqEntry";
            }
            else
            {
                lstrFormName = "_EcomOrderEntry";
            }

            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            //return PartialView("_ShipReqEntry", objOutboundInqModel);
            return PartialView(lstrFormName, objOutboundInqModel);
        }
        public ActionResult SaveShipReqEntryEdit(string p_str_cmp_id, string p_str_cust_id, string p_str_batch_id, string p_str_Sonum, string p_str_pricetkt, string p_str_so_dt,
        string p_str_ShipDt, string p_str_CancelDt, string p_str_refno, string p_str_freight_id, string p_str_status, string p_str_Type, string p_str_AuthId
         , string p_str_fob, string p_str_ShipVia, string p_str_shipchrg, string p_str_CustPO, string p_str_CustOrderdt, string p_str_dept_id
         , string p_str_store_id, string p_str_Note, string p_str_shipto_id, string p_str_Attn, string p_str_dc_id, string p_str_Mailname, string p_str_Addr1
         , string p_str_Addr2, string p_str_city, string p_str_state, string p_str_zipcode, string p_str_country, string p_str_radio, string p_str_quote_num, string p_str_pick_no, string p_str_ref_no)
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
            objOutboundInq.Batchid = p_str_batch_id;
            objOutboundInq.pick_no = p_str_pick_no;
            objOutboundInq.ref_no = p_str_ref_no;
            objOutboundInq = ServiceObject.GetOutSaleId(objOutboundInq);
            objOutboundInq.out_sale_id = (objOutboundInq.out_sale_id == null || objOutboundInq.ListGetOutSaleId[0].out_sale_id.Trim() == "") ? objOutboundInq.out_sale_id : null;
            //objOutboundInq.ListGetOutSaleId[0].out_sale_id;
            if (p_str_radio == "GridShippermanant")
            {
                objOutboundInq = ServiceObject.CheckShipToidExist(objOutboundInq);
                if (objOutboundInq.LstFiledtl.Count == 0)
                {
                    ServiceObject.GetShipToAddressSave(objOutboundInq);
                }
            }
            ServiceObject.Update_To_proc_save_so_hdr(objOutboundInq);
            ServiceObject.Update_To_proc_save_so_addr(objOutboundInq);

            //CR20180813-001 Added By Nithya
            objOutboundInq.cmp_id = p_str_cmp_id;
            objOutboundInq.Sonum = p_str_Sonum;
            objOutboundInq.aloc_doc_id = "";
            objOutboundInq.shipdocid = "";
            objOutboundInq.mode = "SR-MODIFY";
            objOutboundInq.maker = Session["UserID"].ToString().Trim();
            objOutboundInq.makerdt = DateTime.Now.ToString("MM/dd/yyyy");
            objOutboundInq.Auditcomment = "Modified outbound Entry";
            objOutboundInq = ServiceObject.Add_To_proc_save_audit_trail(objOutboundInq);
            //END
            objOutboundInq = ServiceObject.LoadCustConfig(objOutboundInq);

            string lstrFormName = string.Empty;

            if (String.IsNullOrEmpty(objOutboundInq.objCustConfig.ecom_recv_by_bin) == true || objOutboundInq.objCustConfig.ecom_recv_by_bin == "0" || objOutboundInq.objCustConfig.ecom_recv_by_bin == "N")
            {
                lstrFormName = "_ShipReqEntry";
            }
            else
            {
                lstrFormName = "_EcomOrderEntry";
            }

            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            //return PartialView("_ShipReqEntry", objOutboundInqModel);
            return PartialView(lstrFormName, objOutboundInqModel);
        }
        public ActionResult SRDeleteGridData(string p_str_so_num, string p_str_cmp_id, string p_str_itm_code, string p_str_LineNum, string p_str_shpiEntry, string l_str_temp_stk_ref_num)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.cmp_id = p_str_cmp_id;
            objOutboundInq.Sonum = p_str_so_num;
            objOutboundInq.itm_code = p_str_itm_code;
            objOutboundInq.LineNum = Convert.ToInt32(p_str_LineNum);
            objOutboundInq.ShipEntry = p_str_shpiEntry;
            objOutboundInq.temp_stk_ref_num = l_str_temp_stk_ref_num;
            objOutboundInq = ServiceObject.GetGridDeleteData(objOutboundInq);

            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_ShipEntryGrid", objOutboundInqModel);
        }

        public JsonResult SREditDisplayGridToTextbox(string p_str_so_num, string p_str_cmp_id, string p_str_itm_code, string p_str_LineNum, bool l_bool_is_in_edit, string l_str_po_number)//CR_MVC_3PL_0320-001 Added By Nithya
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
            if (Session["l_str_session_aloc_by"] != null)
            {
                objOutboundInq.aloc_by = Session["l_str_session_aloc_by"].ToString();
            }
            else
            {
                objOutboundInq.aloc_by = Session["UserID"].ToString().Trim();
            }
            objOutboundInq.po_num = l_str_po_number;
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
                objOutboundInq.temp_stk_ref_num = string.Empty;
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
                                if (ext == ".csv")
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
                            if (objOutboundInq.ListItmStyledtl.Count > 0)
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
                            if (objOutboundInq.LstItmxCustdtl.Count > 0)
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
                                objInboundInquiry.weight = 2;
                                objInboundInquiry.length = 2;
                                objInboundInquiry.width = 2;
                                objInboundInquiry.height = 2;
                                objInboundInquiry.cube = Convert.ToDecimal(Cube);
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
                            if (objOutboundInq.qty_uom == "" || objOutboundInq.qty_uom == null)
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
                            objOutboundInq.temp_stk_ref_num = string.Empty;
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
            //objPick = ServiceObjectPick.GetShipToPick(objPick);
            //objOutboundInq.ListShipToPick = objPick.ListShipToPick;
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
            objOutboundInq.pick_no = objOutboundInq.lstobjOutboundInq[0].pick_no;
            objOutboundInq.ref_no = objOutboundInq.lstobjOutboundInq[0].ref_no;
            objOutboundInq.temp_stk_ref_num = string.Empty;
            objOutboundInq = ServiceObject.GetViewDetailgrid(objOutboundInq);
            objOutboundInq.ListSOTracking = ServiceObject.fnGetSoTracking(cmpid, so_num, string.Empty).ListSOTracking;
            objOutboundInq.View_Flag = "V";
            objOutboundInq = ServiceObject.LoadCustConfig(objOutboundInq);

            string lstrFormName = string.Empty;

            if (String.IsNullOrEmpty(objOutboundInq.objCustConfig.ecom_recv_by_bin) == true || objOutboundInq.objCustConfig.ecom_recv_by_bin == "0" || objOutboundInq.objCustConfig.ecom_recv_by_bin == "N")
            {
                lstrFormName = "_ShipReqEntry";
            }
            else
            {
                lstrFormName = "_EcomOrderEntry";
            }

            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundShipModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            //return PartialView("_ShipReqEntry", objOutboundShipModel);
            return PartialView(lstrFormName, objOutboundShipModel);
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
            //objPick = ServiceObjectPick.GetShipToPick(objPick);
            //objOutboundInq.ListShipToPick = objPick.ListShipToPick;
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
            objOutboundInq.ship_hdr_notes = objOutboundInq.lstobjOutboundInq[0].note;
            objOutboundInq.pick_no = objOutboundInq.lstobjOutboundInq[0].pick_no;
            objOutboundInq.ref_no = objOutboundInq.lstobjOutboundInq[0].ref_no;
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
            objOutboundInq.temp_stk_ref_num = string.Empty;
            objOutboundInq = ServiceObject.GetViewDetailgrid(objOutboundInq);
            objOutboundInq.View_Flag = "D";
            objOutboundInq.ListSOTracking = ServiceObject.fnGetSoTracking(cmpid, so_num, string.Empty).ListSOTracking;
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundShipModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            objOutboundInq = ServiceObject.LoadCustConfig(objOutboundInq);

            string lstrFormName = string.Empty;

            if (String.IsNullOrEmpty(objOutboundInq.objCustConfig.ecom_recv_by_bin) == true || objOutboundInq.objCustConfig.ecom_recv_by_bin == "0" || objOutboundInq.objCustConfig.ecom_recv_by_bin == "N")
            {
                lstrFormName = "_ShipReqEntry";
            }
            else
            {
                lstrFormName = "_EcomOrderEntry";
            }
            //return PartialView("_ShipReqEntry", objOutboundShipModel);
            return PartialView(lstrFormName, objOutboundShipModel);
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
        public ActionResult Edit(string cmpid, string so_num, string Batchid)
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
            objPick = ServiceObjectPick.GetPickUom(objPick);
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
            if (objOutboundInq.id == null || objOutboundInq.id == string.Empty)
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
            objOutboundInq.cust_id = objOutboundInq.lstobjOutboundInq[0].cust_name;//CR20180829-001 Added  By Nithya
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
            objOutboundInq.status = (objOutboundInq.lstobjOutboundInq[0].status == null || objOutboundInq.lstobjOutboundInq[0].status == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].status);
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
            objOutboundInq.shipchrg = (objOutboundInq.lstobjOutboundInq[0].sh_chg == null || objOutboundInq.lstobjOutboundInq[0].sh_chg == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].sh_chg);
            objOutboundInq.quote_num = (objOutboundInq.lstobjOutboundInq[0].quote_num == null || objOutboundInq.lstobjOutboundInq[0].quote_num == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].quote_num);
            objOutboundInq.pick_no = (objOutboundInq.lstobjOutboundInq[0].pick_no == null || objOutboundInq.lstobjOutboundInq[0].pick_no == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].pick_no);
            objOutboundInq.ref_no = (objOutboundInq.lstobjOutboundInq[0].ref_no == null || objOutboundInq.lstobjOutboundInq[0].ref_no == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].ref_no);
            if (objOutboundInq.shipchrg == "0.0000")
            {
                objOutboundInq.shipchrg = "0.00";
            }
            else
            {
                objOutboundInq.shipchrg = objOutboundInq.shipchrg;
            }
            objOutboundInq.CustPO = (objOutboundInq.lstobjOutboundInq[0].cust_ordr_num == null || objOutboundInq.lstobjOutboundInq[0].cust_ordr_num == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].cust_ordr_num);
            objOutboundInq.CustOrderdt = (objOutboundInq.lstobjOutboundInq[0].cust_ordr_dt == null || objOutboundInq.lstobjOutboundInq[0].cust_ordr_dt == "" ? string.Empty : Convert.ToDateTime(objOutboundInq.lstobjOutboundInq[0].cust_ordr_dt).ToString("MM/dd/yyyy"));
            if (objOutboundInq.CustOrderdt == "01/01/1900")
            {
                objOutboundInq.CustOrderdt = "";
            }
            else
            {
                objOutboundInq.CustOrderdt = objOutboundInq.CustOrderdt;
            }
            objOutboundInq.dept_id = (objOutboundInq.lstobjOutboundInq[0].dept_id == null || objOutboundInq.lstobjOutboundInq[0].dept_id == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].dept_id);
            objOutboundInq.store_id = (objOutboundInq.lstobjOutboundInq[0].store_id == null || objOutboundInq.lstobjOutboundInq[0].store_id == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].store_id);
            objOutboundInq.ship_hdr_notes = (objOutboundInq.lstobjOutboundInq[0].note == null || objOutboundInq.lstobjOutboundInq[0].note == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].note);
            objOutboundInq = ServiceObject.GetViewAddrDetail(objOutboundInq);
            objOutboundInq.shipto_id = (objOutboundInq.lstobjOutboundInq[0].shipto_id == null || objOutboundInq.lstobjOutboundInq[0].shipto_id == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].shipto_id);
            objOutboundInq.Attn = (objOutboundInq.lstobjOutboundInq[0].sl_attn == null || objOutboundInq.lstobjOutboundInq[0].sl_attn == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].sl_attn);
            objOutboundInq.dc_id = (objOutboundInq.lstobjOutboundInq[0].dc_id == null || objOutboundInq.lstobjOutboundInq[0].dc_id == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].dc_id);
            objOutboundInq.Mailname = (objOutboundInq.lstobjOutboundInq[0].sl_mail_name == null || objOutboundInq.lstobjOutboundInq[0].sl_mail_name == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].sl_mail_name);
            objOutboundInq.Addr1 = (objOutboundInq.lstobjOutboundInq[0].sl_addr_line1 == null || objOutboundInq.lstobjOutboundInq[0].sl_addr_line1 == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].sl_addr_line1);
            objOutboundInq.Addr2 = (objOutboundInq.lstobjOutboundInq[0].sl_addr_line2 == null || objOutboundInq.lstobjOutboundInq[0].sl_addr_line2 == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].sl_addr_line2);
            objOutboundInq.city = (objOutboundInq.lstobjOutboundInq[0].sl_city == null || objOutboundInq.lstobjOutboundInq[0].sl_city == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].sl_city);
            objOutboundInq.state = (objOutboundInq.lstobjOutboundInq[0].sl_state_id == null || objOutboundInq.lstobjOutboundInq[0].sl_state_id == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].sl_state_id);
            objOutboundInq.zipcode = (objOutboundInq.lstobjOutboundInq[0].sl_post_code == null || objOutboundInq.lstobjOutboundInq[0].sl_post_code == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].sl_post_code);
            objOutboundInq.country = (objOutboundInq.lstobjOutboundInq[0].sl_cntry_id == null || objOutboundInq.lstobjOutboundInq[0].sl_cntry_id == "" ? string.Empty : objOutboundInq.lstobjOutboundInq[0].sl_cntry_id);
            OBAllocationService oBAllocationService = new OBAllocationService();
            objOutboundInq.lstOBSRAlocDtl = oBAllocationService.getSRAlocDtl(cmpid, so_num);
            if (objOutboundInq.lstOBSRAlocDtl.Count > 0)
                TempData["obSrAlocRefNum"] = objOutboundInq.lstOBSRAlocDtl[0].ref_num;
            ViewBag.IsBackOrder = "N";
            bool lstrBoExists = false;
            foreach (ClsOBSRAlocDtl rec in objOutboundInq.lstOBSRAlocDtl)
            {
                if (rec.bo_qty > 0)
                {
                    ViewBag.IsBackOrder = "Y";
                    objOutboundInq.temp_stk_ref_num = objOutboundInq.lstOBSRAlocDtl[0].ref_num;
                    lstrBoExists = true;
                    break;
                }

            }

            if (lstrBoExists == true)
            {
                objOutboundInq.bo_flag = "Y";
            }
            else
            {
                objOutboundInq.bo_flag = "N";
                objOutboundInq.temp_stk_ref_num = string.Empty;
            }
            objOutboundInq = ServiceObject.GetViewDetailgrid(objOutboundInq);

            objOutboundInq = ServiceObject.GetShipReqEditDtl(objOutboundInq);
            //CR-180330-001 Added By Nithya
            objOutboundInq = ServiceObject.GetSRGridRowCount(objOutboundInq);
            l_int_line_count = objOutboundInq.ListCheckExistStyle[0].SRRowcount;
            objOutboundInq.LineNum = l_int_line_count + 1;
            objPick.Cntry_Id = objOutboundInq.country;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objOutboundInq.ListStatePick = objPick.ListStatePick;
            objOutboundInq.View_Flag = "M";
            objOutboundInq.quote_num = Batchid;
            objOutboundInq = ServiceObject.LoadCustDtls(objOutboundInq);

            objOutboundInq.ListSOTracking = ServiceObject.fnGetSoTracking(cmpid, so_num, string.Empty).ListSOTracking;

            if (objOutboundInq.LstFiledtl.Count > 0)
            {
                objOutboundInq.aloc_by = objOutboundInq.LstFiledtl[0].aloc_by;
                Session["l_str_session_aloc_by"] = objOutboundInq.aloc_by;
            }

            objOutboundInq = ServiceObject.LoadCustConfig(objOutboundInq);

            string lstrFormName = string.Empty;

            if (String.IsNullOrEmpty(objOutboundInq.objCustConfig.ecom_recv_by_bin) == true || objOutboundInq.objCustConfig.ecom_recv_by_bin == "0" || objOutboundInq.objCustConfig.ecom_recv_by_bin == "N")
            {
                lstrFormName = "_ShipReqEntry";
            }
            else
            {
                lstrFormName = "_EcomOrderEntry";
            }

            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundShipModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            //return PartialView("_ShipReqEntry", objOutboundShipModel);
            return PartialView(lstrFormName, objOutboundShipModel);
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
        //CR20180828-001 Added by Nithya
        public JsonResult ItemXGetshiptoDtl(string term, string cmp_id, string cust_id)
        {
            OutboundInqService ServiceObject = new OutboundInqService();
            var List = ServiceObject.ItemXGetshiptoDetails(term.Trim(), cmp_id, cust_id).LstItmxCustdtl.Select(x => new
            {
                label = x.shipto_id,
                value = x.shipto_id,
                shipto_id = x.shipto_id,
                cust_id = x.cust_id,
                attn = x.attn,
                DC_id = x.DC_id,
                mail_name = x.mail_name,
                addr_line1 = x.addr_line1,
                addr_line2 = x.addr_line2,
                city = x.city,
                state_id = x.state_id,
                post_code = x.post_code,
                cntry_id = x.cntry_id
            }).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PickCustDtl(string term, string cmp_id)
        {
            OutboundInqService ServiceObject = new OutboundInqService();
            var List = ServiceObject.GetCustDtl(term.Trim(), cmp_id).LstItmxCustdtl.Select(x => new { label = x.Custdtl, value = x.cust_id, cust_id = x.cust_id, Custname = x.Custname }).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        //END
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
            objOutboundInq.so_num = Id;
            objOutboundInq.cmp_id = cmp_id;
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
                objOutboundInq.po_num = objOutboundInq.lstShipAlocdtl[j].po_num;
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

            Session["l_str_session_aloc_by"] = objOutboundInq.aloc_by;
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
                objOutboundInq.po_num = objOutboundInq.LstItmxCustdtl[k].po_num;
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
                objOutboundInq.balance_qty = Avail - objOutboundInq.due_qty;//CR20180730-001 Added By Nithya
                objOutboundInq.loc_id = "";
                objOutboundInq.ctn_qty = 0;
                ServiceObject.InsertTempAlocSummary(objOutboundInq);
                objOutboundInq = ServiceObject.Update_Tbl_Temp_So_Auto_Aloc_BackOrdervalue(objOutboundInq);
                objOutboundInq.itm_num = objOutboundInq.LstItmxCustdtl[k].itm_num;
                objOutboundInq.itm_color = objOutboundInq.LstItmxCustdtl[k].itm_color;
                objOutboundInq.itm_size = objOutboundInq.LstItmxCustdtl[k].itm_size;
                objOutboundInq.itm_code = objOutboundInq.LstItmxCustdtl[k].itm_code;
                objOutboundInq.whs_id = Whsid;
                objOutboundInq.po_num = objOutboundInq.po_num;//CR20181129
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
            string Bak_OdrQty = string.Empty;

            string tmpStep = string.Empty;
            string tmpStatus = string.Empty;
            string LocId = string.Empty;
            string New_Pkg_Id = string.Empty;
            string palet_id = string.Empty;
            string SptCtnMsg = string.Empty;
            string Line_Num = string.Empty;
            string OldPkgId = string.Empty;
            string WhsID = string.Empty;
            string l_str_doc_pkg_id = string.Empty;
            int dueqty = 0;
            int LineNum = 0;
            string l_str_sr_pick_ref_no = string.Empty;

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
            objOutboundInq.so_num = p_str_Alocshiprqfm;
            l_str_sr_pick_ref_no = ServiceObject.GetSRPickRefNumber(p_str_cmp_id, p_str_Alocshiprqfm);
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
                  //  ServiceObject.Moveto_TrnHdr(objOutboundInq);

                }
                objOutboundInq = ServiceObject.OutboundGETTEMPALOCSUMMARY(objOutboundInq);
                objOutboundInq = ServiceObject.OutboundGETTEMPLIST(objOutboundInq);
                for (int m = 0; m < objOutboundInq.LstAlocSummary.Count(); m++)
                {
                    updateSodtl(m, p_str_cmp_id, p_str_Alocshiprqfm);
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
                    Move_to_aloc_dtl(m, p_str_cmp_id, p_str_Alocdocid, p_str_Alocshiprqfm);
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
                    Move_to_aloc_ctn(m, p_str_cmp_id, p_str_Alocdocid, p_str_Alocshiprqfm);
                    Update_Tbl_iv_itm_trn_in(m, p_str_cmp_id, p_str_Alocdocid, p_str_AlocOrdrno, p_str_Alocshiptoname, p_str_Alocshiprqfm);
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
                objOutboundInq.aloc_doc_id = p_str_Alocdocid;
                ServiceObject.Change_SOHdr_Status_atAdd(objOutboundInq);
                //CR20180906-001 Added By Nithya
                ServiceObject.Change_LotDtl_Status_atAdd(objOutboundInq);
                //END
                objOutboundInq.l_str_aloc_aloc_dtls = true;
                //CR20180813-001 Added By Nithya
                objOutboundInq.cmp_id = p_str_cmp_id;
                objOutboundInq.Sonum = p_str_Alocshiprqfm;
                objOutboundInq.aloc_doc_id = p_str_Alocdocid;
                objOutboundInq.shipdocid = "";
                objOutboundInq.mode = "ALOC";
                objOutboundInq.maker = Session["UserID"].ToString().Trim();
                objOutboundInq.makerdt = DateTime.Now.ToString("MM/dd/yyyy");
                objOutboundInq.Auditcomment = "Allocated";
                objOutboundInq = ServiceObject.Add_To_proc_save_audit_trail(objOutboundInq);
                ServiceObject.DeleteAUTOALOC(objOutboundInq);
                //END
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
                    strReportName = "rpt_iv_vas.rpt";

                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//VasInquiry//" + strReportName;
                    objVasInquiry.cmp_id = p_str_cmp_id;
                    objVasInquiry.ship_doc_id = p_str_vasid;
                    objVasInquiry = ServiceObject.GetVasPostDetails(objVasInquiry);
                    var rptSource = objVasInquiry.ListVasInquiry.ToList();
                    if (rptSource.Count > 0)
                    {
                        using (ReportDocument rd = new ReportDocument())
                        {
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objVasInquiry.ListVasInquiry.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objVasInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim();
                            rd.SetParameterValue("fml_image_path", objVasInquiry.Image_Path);

                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
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
        public ActionResult ShowAlocSaveReport(string p_str_cmpid, string p_str_Alocdocid, string p_str_Sonum)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            OutboundInq objInbound = new OutboundInq();
            OutboundInqService objService = new OutboundInqService();
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
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                    objOutboundInq.cmp_id = p_str_cmpid;
                    objOutboundInq.AlocdocId = p_str_Alocdocid;
                    objOutboundInq = ServiceObject.Get_AlocSaveRpt(objOutboundInq);
                    var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                    if (rptSource.Count > 0)
                    {
                        using (ReportDocument rd = new ReportDocument())
                        {
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);

                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Out Ticket Report");
                        }
                    }
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
                    strReportName = "rpt_iv_packslip_tpw.rpt";
                    OutboundShipInq objOutboundShipInq = new OutboundShipInq();
                    OutboundShipInqService ServiceObject = new OutboundShipInqService();
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                    objOutboundShipInq.cmp_id = p_str_cmpid;
                    objOutboundShipInq.ship_doc_id = Session["l_str_bol_num"].ToString();
                    objOutboundShipInq = ServiceObject.OutboundShipInqpackSlipRpt(objOutboundShipInq);
                    //objOutboundShipInq = ServiceObject.GetTotCubesRpt(objOutboundShipInq);
                    //if (objOutboundShipInq.LstPalletCount.Count > 0)
                    //{
                    //    objOutboundShipInq.TotCube = objOutboundShipInq.LstPalletCount[0].TotCube;
                    //}
                    //else
                    //{
                    //    objOutboundShipInq.TotCube = 0;
                    //}
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
                            objOutboundShipInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundShipInq.Image_Path);
                            //rd.SetParameterValue("SumOfCubes", objOutboundShipInq.TotCube);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
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
                objOutboundInq.po_num = objOutboundInq.lstShipAlocdtl[i].po_num;
                objOutboundInq = ServiceObject.OutboundGETAVILQTY(objOutboundInq);
                objOutboundInq.avail_qty = objOutboundInq.LstAvailqty[0].pkg_avail_cnt;


                objOutboundInq.avail_qty = Convert.ToInt32(objOutboundInq.aloc_qty) + Convert.ToInt32(objOutboundInq.LstAvailqty[0].pkg_avail_cnt);

                objOutboundInq.balance_qty = Convert.ToInt32(objOutboundInq.aloc_qty) + Convert.ToInt32(objOutboundInq.LstAvailqty[0].pkg_avail_cnt) - objOutboundInq.due_qty;

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
                objOutboundInq.balance_qty = Avail - objOutboundInq.due_qty;//CR20180730-001 Added By Nithya
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
        public ActionResult AlocSaveDelete(string p_str_cmp_id, string p_str_aloc_id, string p_str_so_num, string p_str_new_loc_id)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.cmp_id = p_str_cmp_id;
            objOutboundInq.AlocdocId = p_str_aloc_id;
            objOutboundInq.so_num = p_str_so_num;
            objOutboundInq = ServiceObject.Del_Alloc_Upd_SO(objOutboundInq);
            //if (p_str_new_loc_id == "BTS")
            //{
            ServiceObject.delAlocAndUpdtLoc(p_str_cmp_id, p_str_aloc_id, p_str_so_num, p_str_new_loc_id);
            //}
            //else
            //{
            //    objOutboundInq = ServiceObject.Del_data_to_itm_trn_in(objOutboundInq);
            //}

            //objOutboundInq = ServiceObject.OutboundGETTEMPALOCDTL(objOutboundInq);
            //for (int i = 0; i < objOutboundInq.LstAlocDtl.Count(); i++)
            //{
            //    int UpDtQty = 0;
            //    UpDtQty = objOutboundInq.LstAlocDtl[i].Aloc;
            //    if (UpDtQty > 0)
            //    {
            //        objOutboundInq.cmp_id = p_str_cmp_id;
            //        objOutboundInq.itm_code = objOutboundInq.LstAlocDtl[i].itm_code;
            //        objOutboundInq.whs_id = objOutboundInq.LstAlocDtl[i].whs_id;
            //        objOutboundInq.lot_id = objOutboundInq.LstAlocDtl[i].lot_id;
            //        objOutboundInq.rcvd_dt = objOutboundInq.LstAlocDtl[i].rcvd_dt;
            //        objOutboundInq.loc_id = objOutboundInq.LstAlocDtl[i].loc_id;
            //        objOutboundInq.palet_id = objOutboundInq.LstAlocDtl[i].Palet_id;
            //        objOutboundInq.aloc_qty = UpDtQty;
            //        objOutboundInq.process_id = "";
            //        objOutboundInq = ServiceObject.Add_To_Trn_Hdr(objOutboundInq);
            //        if (p_str_new_loc_id == "BTS")
            //        {
            //            objOutboundInq.loc_id = "BTS";
            //                   objOutboundInq = ServiceObject.addInvTransHdr(objOutboundInq);
            //        }
            //    }
            //}
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
                //  objOutboundInq.balance_qty = Avail - objOutboundInq.due_qty;//CR20180730-001 Added By Nithya


                objOutboundInq.po_num = objOutboundInq.lstShipAlocdtl[i].po_num;
                objOutboundInq = ServiceObject.OutboundGETAVILQTY(objOutboundInq);

                objOutboundInq.avail_qty = Convert.ToInt32(objOutboundInq.aloc_qty) + Convert.ToInt32(objOutboundInq.LstAvailqty[0].pkg_avail_cnt);
                objOutboundInq.balance_qty = Convert.ToInt32(objOutboundInq.aloc_qty) + Convert.ToInt32(objOutboundInq.LstAvailqty[0].pkg_avail_cnt) - objOutboundInq.due_qty;


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
        public ActionResult EditAutoAloc(string p_str_cmp_id, string p_str_Alocdocid, string SelectdID, string p_str_Sonum, string p_str_so_line)
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
            objOutboundInq.Soline = Convert.ToInt16(p_str_so_line);
            objOutboundInq = ServiceObject.GetSelectedgridValueList(objOutboundInq);
            DtlLine = objOutboundInq.LstManualAloc[0].Soline;
            objOutboundInq.po_num = objOutboundInq.LstManualAloc[0].po_num;
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


            // objOutboundInq.aloc_by = Session["l_str_session_aloc_by"].ToString();
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
            string l_str_sr_pick_ref_no = string.Empty;
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
            objOutboundInq.so_num = p_str_Alocshiprqfm;
            l_str_sr_pick_ref_no = ServiceObject.GetSRPickRefNumber(p_str_cmp_id, p_str_Alocshiprqfm);
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
                  //  ServiceObject.Moveto_TrnHdr(objOutboundInq);
                }
                objOutboundInq = ServiceObject.OutboundGETTEMPALOCSUMMARY(objOutboundInq);
                objOutboundInq = ServiceObject.OutboundGETTEMPLIST(objOutboundInq);


                for (int m = 0; m < objOutboundInq.LstAlocSummary.Count(); m++)
                {
                    updateSodtl(m, p_str_cmp_id, p_str_Alocshiprqfm);
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
                    Move_to_aloc_dtl(m, p_str_cmp_id, p_str_Alocdocid, p_str_Alocshiprqfm);
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
                    Move_to_aloc_ctn(m, p_str_cmp_id, p_str_Alocdocid, p_str_Alocshiprqfm);
                    Update_Tbl_iv_itm_trn_in(m, p_str_cmp_id, p_str_Alocdocid, p_str_AlocOrdrno, p_str_Alocshiptoname, p_str_Alocshiprqfm);
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
        public ActionResult AddEditAutoAloc(string p_str_cmp_id, string p_str_Alocdocid, string SelectdID, string p_str_Sonum, string p_str_so_line)
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
            int balanceqty = p_str_Reqqty - l_str_Alocated;
            objOutboundInq.LstManualAloc = InvoiceLi;
            Session["GridInvoiceList"] = objOutboundInq.LstManualAloc;
            //Session["lblAlocated"] = 0;
            List<OutboundInq> GETInvoiceList = new List<OutboundInq>();
            GETInvoiceList = Session["GridInvoiceList"] as List<OutboundInq>;
            return Json(new { loc_id = l_str_loc_id, avail_qty = l_str_avail_qty, pkg_qty = l_str_pkg_qty, Aloc = l_str_Aloc, back_ordr_qty = l_str_back_ordr_qty, LineNum = l_str_LineNum, SelAloc = getdata7, alocated = l_str_Alocated, linenum = linenum, balanceqty = balanceqty }, JsonRequestBehavior.AllowGet);
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
            int balanceqty = p_str_Reqqty - l_str_Alocated;
            objOutboundInq.LstManualAloc = InvoiceLi;
            Session["GridInvoiceList"] = objOutboundInq.LstManualAloc;
            //Session["lblAlocated"] = 0;
            List<OutboundInq> GETInvoiceList = new List<OutboundInq>();
            GETInvoiceList = Session["GridInvoiceList"] as List<OutboundInq>;
            return Json(new { loc_id = l_str_loc_id, avail_qty = l_str_avail_qty, pkg_qty = l_str_pkg_qty, Aloc = l_str_Aloc, back_ordr_qty = l_str_back_ordr_qty, LineNum = l_str_LineNum, SelAloc = getdata7, alocated = l_str_Alocated, linenum = linenum, balanceqty = balanceqty }, JsonRequestBehavior.AllowGet);
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
                GSoNum = Session["Sonum"].ToString();
                objOutboundInq.so_num = GSoNum;
                objOutboundInq = ServiceObject.OutboundGETTEMPALOCDTL(objOutboundInq);
                AllocationLineNum = Session["lineNum"].ToString();
                line = Convert.ToInt32(AllocationLineNum);
                Itm_Code1 = GETInvoiceLi[0].itm_code;
                Itm_Num1 = GETInvoiceLi[0].itm_num;
                Itm_Color1 = GETInvoiceLi[0].itm_color;
                Itm_Size1 = GETInvoiceLi[0].itm_size;
                l_int_dtlLine = Session["DtlLine"].ToString();
                l_int_dueLine = Session["DueLine"].ToString();

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
            OutboundInq.Sonum = ibdocid;
            OutboundInq.Container = cntrid;
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel OutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(OutboundInq);
            return PartialView("_OutboundInquiryImportFile", OutboundInqModel);
        }
        public ActionResult OutboundInquiryFileDocument(string cmp_id, string quotenum, string Sonum, string datefrom, string dateto, string screentitle)
        {
            string name = string.Empty;
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.CompID = cmp_id;
            objOutboundInq.cmp_id = cmp_id;
            objOutboundInq.Sonum = Sonum;
            objOutboundInq.quote_num = quotenum;
            objOutboundInq.DocumentdateFrom = datefrom;
            objOutboundInq.DocumentdateTo = dateto;
            objOutboundInq.screentitle = screentitle;
            objOutboundInq.doctype = "OUTBOUND";

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
            directoryPath = Path.Combine(directoryPath, Sonum);
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
                    objOutboundInq.Filename = name;
                    long size = flInfo.Length;
                    lstrAlocList = lstrAlocList + objOutboundInq.Filename + ",";
                }
                lstrAlocList = lstrAlocList.Remove(lstrAlocList.Length - 1, 1);
                lstrAlocList = lstrAlocList + "";
                objOutboundInq.Filename = lstrAlocList;
            }
            else
            {
                foreach (FileInfo flInfo in dir1.GetFiles())
                {
                    name = flInfo.Name;
                    objOutboundInq.Filename = name;
                    long size = flInfo.Length;
                }

            }
            objOutboundInq = ServiceObject.GetTempFiledtl(objOutboundInq);
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel OutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_OutboundInquiryImportFile", OutboundInqModel);
        }
        public ActionResult ShowvasdtlReport(string p_str_cmpid, string p_str_status, string p_str_Shipping_id)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
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
                    strReportName = "rpt_iv_vas.rpt";
                    VasInquiry objVasInquiry = new VasInquiry();
                    IVasInquiryService ServiceObject = new VasInquiryService();
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//VasInquiry//" + strReportName;
                    objVasInquiry.cmp_id = p_str_cmpid;
                    objVasInquiry.ship_doc_id = p_str_Shipping_id;
                    objVasInquiry = ServiceObject.GetVasPostDetails(objVasInquiry);
                    var rptSource = objVasInquiry.ListVasInquiry.ToList();
                    if (rptSource.Count > 0)
                    {
                        using (ReportDocument rd = new ReportDocument())
                        {
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objVasInquiry.ListVasInquiry.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objVasInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim();
                            rd.SetParameterValue("fml_image_path", objVasInquiry.Image_Path);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
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
        //CR_3PL_MVC_BL_2018_0226_001 Modified By Ravi 26-02-2017
        [HttpPost]
        public ActionResult GridUploadFiles(string CompID, string ibdocid, string comment, string P_STR_Container, string p_str_sub_doc_type)
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
                                string fileNameOnly = Path.GetFileNameWithoutExtension(FileUpload.FileName).Replace("#", ""); ;
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
        public JsonResult AlocSummaryCount(string p_str_so_num)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.so_num = p_str_so_num;
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
                    OutboundInq objOutboundInq = new OutboundInq();
                    OutboundInqService ServiceObject = new OutboundInqService();

                    strReportName = "rpt_mvc_so_bo.rpt";
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                    objOutboundInq.cmp_id = p_str_cmpid;
                    objOutboundInq.so_num = p_str_Sonum;
                    objOutboundInq = ServiceObject.Get_AlocBackOrderRptList(objOutboundInq);
                    var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                    if (rptSource.Count > 0)
                    {
                        using (ReportDocument rd = new ReportDocument())
                        {
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
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
        private void updateSodtl(int m, string Cmp_id, string str_so_num)
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
            objOutboundInq.so_num = str_so_num;
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
        private void Move_to_aloc_dtl(int J, string Cmp_id, string p_str_Alocdocid, string str_so_num)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            string Item_code, Item_num, Item_Color, Item_Size = string.Empty;
            int Line_Num = 0;
            int I = 0;
            objOutboundInq.cmp_id = Cmp_id;
            objOutboundInq.so_num = str_so_num;
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
                objOutboundInq.itm_code = objOutboundInq.LstItmxCustdtl[J].itm_code;  //Item_code
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
                if (objOutboundInq.aloc_qty == 0)
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
        private void Move_to_aloc_ctn(int K, string Cmp_id, string p_str_Alocdocid, string str_so_num)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            string Itm_Code, Lot_Id, Rcvd_Dt, Loc_Id, Aloc_Qty = string.Empty;
            string Item_Code, Item_num, Item_Color, Item_Size = string.Empty;
            int Line_Num = 0;
            int Alo, CTN = 0;
            int PrevLine = 1;
            int NewCtnQty = 0;
            objOutboundInq.cmp_id = Cmp_id;
            objOutboundInq.so_num = str_so_num;
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
        private void Update_Tbl_iv_itm_trn_in(int K, string Cmp_id, string p_str_Alocdocid, string p_str_AlocOrdrno, string p_str_Alocshiptoname, string str_so_num)
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
            objOutboundInq.cmp_id = Cmp_id;
            objOutboundInq.so_num = str_so_num;
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
         (Item_num == objOutboundInq.LstAlocDtl[i].itm_num) && (Item_Color == objOutboundInq.LstAlocDtl[i].itm_color) &&
         (Item_Size == objOutboundInq.LstAlocDtl[i].itm_size))
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
                            // objOutboundInq.lbl_id = objOutboundInq.ListCheckExistStyle[0].lbl_id;
                            objOutboundInq.lbl_id = str_so_num;
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
                        objOutboundInq.lbl_id = str_so_num;
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
                        objOutboundInq.aloc_by = Session["l_str_session_aloc_by"].ToString();
                        objOutboundInq.lbl_id = str_so_num;
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
            objOutboundInq.palet_id = palet_id;
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
        private void GetAvailQty(ref int AvailQty, string ItemCode, string cmp_id, string p_str_vendpo)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            IOutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.cmp_id = cmp_id;
            objOutboundInq.itm_code = ItemCode;
            objOutboundInq.po_num = p_str_vendpo;
            objOutboundInq = ServiceObject.OutboundGETALOCSORTSTMT(objOutboundInq);
            if (objOutboundInq.LstItmxCustdtl.Count > 0)
            {
                objOutboundInq.aloc_sort_stmt = objOutboundInq.LstItmxCustdtl[0].aloc_sort_stmt;
                strAlocSortStmt = objOutboundInq.aloc_sort_stmt;
                objOutboundInq.aloc_by = objOutboundInq.LstItmxCustdtl[0].aloc_by;
                strAlocBy = objOutboundInq.aloc_by;
            }
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
            Folderpath = System.Configuration.ConfigurationManager.AppSettings["tempFilepath"].ToString().Trim();
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
                    OutboundInq objOutboundInq = new OutboundInq();
                    OutboundInqService ServiceObject = new OutboundInqService();
                    objOutboundInq.cmp_id = p_str_cmpid;
                    objOutboundInq.so_num = p_str_Sonum;
                    objOutboundInq = ServiceObject.BackOrderRpt(objOutboundInq);
                    if (objOutboundInq.LstAlocDtl.Count > 0)
                    {
                        strReportName = "rpt_mvc_so_bo.rpt";
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmpid;
                        objOutboundInq.so_num = p_str_Sonum;
                        objOutboundInq = ServiceObject.Get_AlocBackOrderRptList(objOutboundInq);
                        var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//ALOC_ENTRY_" + strDateFormat + ".pdf";
                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                reportFileName = "Outbound ALOC_ENTRY_" + strDateFormat + ".pdf";
                                Session["RptFileName"] = strFileName;
                            }
                        }
                        //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                    }
                    else
                    {
                        strReportName = "rpt_iv_pickslip_direct.rpt";
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmpid;
                        objOutboundInq.AlocdocId = p_str_Alocdocid;
                        objOutboundInq = ServiceObject.Get_AlocSaveRpt(objOutboundInq);
                        var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//ALOC_ENTRY_" + strDateFormat + ".pdf";
                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                reportFileName = "Outbound ALOC_ENTRY_" + strDateFormat + ".pdf";
                                Session["RptFileName"] = strFileName;
                            }
                        }


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
                        //objOutboundShipInq = ServiceObject.GetTotCubesRpt(objOutboundShipInq);//CR20180601-001 Added bY Nithya
                        //if (objOutboundShipInq.LstPalletCount.Count > 0)
                        //{
                        //    objOutboundShipInq.TotCube = objOutboundShipInq.LstPalletCount[0].TotCube;
                        //}
                        //else
                        //{
                        //    objOutboundShipInq.TotCube = 0;
                        //}
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
                                objOutboundShipInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objOutboundShipInq.Image_Path);
                                //rd.SetParameterValue("SumOfCubes", objOutboundShipInq.TotCube);
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
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                objOutboundShipInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objOutboundShipInq.Image_Path);
                                rd.SetParameterValue("TotCube", objOutboundShipInq.TotCube);
                                rd.SetParameterValue("TotCarton", objOutboundShipInq.TotCtns);
                                rd.SetParameterValue("TotWgt", objOutboundShipInq.TotWgt);
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
        //END
        //CR-20180522-001 Added By Nithya
        public ActionResult StockVerify(string p_str_cmp_id, string p_str_screen_title, string p_str_Sonum, string p_str_batchId, string P_str_SoNumFm, string P_str_SoNumTo, string P_str_screen_MOde)
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
            DataColumn colponum = new DataColumn("po_num", typeof(string));

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
            dtOutInq.Columns.Add(colponum);

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
                objOutboundInq.po_num = objOutboundInq.LstStkverifyList[i].po_num;//CR20181128

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
                dtOutInq.Rows[lintCount][colponum] = objOutboundInq.po_num;

                OutboundInq objOutboundInqDtltemp = new OutboundInq();
                objOutboundInqDtltemp.cmp_id = objOutboundInq.cmp_id;
                objOutboundInqDtltemp.LineNum = LineNum;
                objOutboundInqDtltemp.itm_code = objOutboundInq.itm_code;
                objOutboundInqDtltemp.Style = objOutboundInq.Style;
                objOutboundInqDtltemp.Color = objOutboundInq.Color;
                objOutboundInqDtltemp.Size = objOutboundInq.Size;
                objOutboundInqDtltemp.whs_id = objOutboundInq.whs_id;
                objOutboundInqDtltemp.po_num = objOutboundInq.po_num;
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
                objOutboundInq = ServiceObject.OutboundGETALOCSORTSTMT(objOutboundInq);
                if (objOutboundInq.LstItmxCustdtl.Count > 0)
                {
                    objOutboundInq.aloc_sort_stmt = objOutboundInq.LstItmxCustdtl[0].aloc_sort_stmt;
                    strAlocSortStmt = objOutboundInq.aloc_sort_stmt;
                    objOutboundInq.aloc_by = objOutboundInq.LstItmxCustdtl[0].aloc_by;
                    strAlocBy = objOutboundInq.aloc_by;
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
            objOutboundInq.ScreenMode = P_str_screen_MOde;
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundShipModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_Outbound_StockVerify", objOutboundShipModel);
        }


        public ActionResult UPSLabelPrint(string p_str_cmp_id, string pstrWhsId, string p_str_screen_title, string p_str_so_num, string p_str_accout_id)
        {
            string jsonErrorCode = "0";
            string l_str_error_message = string.Empty;

            try
            {
                UPSShipment objUPSShipment = new UPSShipment();

                objUPSShipment.cmp_id = p_str_cmp_id;
                objUPSShipment.so_num = p_str_so_num;
                UPSShipmentService objUPSShipmentService = new UPSShipmentService();

                objUPSShipment = objUPSShipmentService.GetUPSShipperDetails(p_str_cmp_id, "", p_str_so_num, Session["UserID"].ToString().Trim(), "3847WY");

                if (objUPSShipment.UPSGsSecurity.Username != null)
                {
                    objUPSShipment.cmp_id = p_str_cmp_id;
                    objUPSShipment.so_num = p_str_so_num;
                    objUPSShipment.service_code = "03";
                    Mapper.CreateMap<UPSShipment, UPSShipmentModel>();
                    UPSShipmentModel objUPSShipmentModel = Mapper.Map<UPSShipment, UPSShipmentModel>(objUPSShipment);
                    return PartialView("_UPSLabelPrint", objUPSShipmentModel);
                }
                else

                {

                    return Json(new { result = jsonErrorCode, err = "Ship Request Details Not found" }, JsonRequestBehavior.AllowGet);
                }

            }

            catch (Exception ex)
            {
                l_str_error_message = ex.Message;
                jsonErrorCode = "-2";
            }

            return Json(new { result = jsonErrorCode, err = l_str_error_message }, JsonRequestBehavior.AllowGet);


        }

        public ActionResult UPSGenerateLabels(UPSGsSecurity objUPSGsSecurity, UPSShipFrom objUPSShipFrom, UPSShipTo objUPSShipTo,
            UPSShipper objUPSShipper, UPSSoldTo objUPSSoldTo, shipOdrerDtl[] lstShipOdrerDtl,
            string ServiceCode, string SRNum,
            string InvoiceNumber, string InvoiceDate, string PurchaseOrderNumber, string TermsOfShipment, string ReasonForExport, string Comments)
        {

            string strFileName = string.Empty;
            try
            {

                string lstrUPSLabelPath = ConfigurationManager.AppSettings["UPSLabelpath"].ToString();
                lstrUPSLabelPath = Path.Combine(lstrUPSLabelPath, SRNum);
                ShipService shpSvc = new ShipService();
                ShipmentRequest shipmentRequest = new ShipmentRequest();
                UPSSecurity upss = new UPSSecurity();
                UPSSecurityServiceAccessToken upssSvcAccessToken = new UPSSecurityServiceAccessToken();
                upssSvcAccessToken.AccessLicenseNumber = objUPSGsSecurity.AccessLicenseNumber;
                upss.ServiceAccessToken = upssSvcAccessToken;
                UPSSecurityUsernameToken upssUsrNameToken = new UPSSecurityUsernameToken();
                upssUsrNameToken.Username = objUPSGsSecurity.Username;
                upssUsrNameToken.Password = objUPSGsSecurity.Password;
                upss.UsernameToken = upssUsrNameToken;
                shpSvc.UPSSecurityValue = upss;

                RequestType request = new RequestType();
                String[] requestOption = { "nonvalidate" };
                request.RequestOption = requestOption;
                shipmentRequest.Request = request;
                ShipmentType shipment = new ShipmentType();
                shipment.Description = "CDC Shipper Request";

                ShipperType shipper = new ShipperType();
                shipper.ShipperNumber = objUPSShipper.ShipperNumber;
                PaymentInfoType paymentInfo = new PaymentInfoType();
                ShipmentChargeType shpmentCharge = new ShipmentChargeType();
                BillShipperType billShipper = new BillShipperType();
                billShipper.AccountNumber = objUPSShipper.ShipperNumber;
                shpmentCharge.BillShipper = billShipper;
                shpmentCharge.Type = "01";
                ShipmentChargeType[] shpmentChargeArray = { shpmentCharge };
                paymentInfo.ShipmentCharge = shpmentChargeArray;
                shipment.PaymentInformation = paymentInfo;
                ShipAddressType shipperAddress = new ShipAddressType();

                String[] addressLine = { objUPSShipper.ShiperAddress.AddressLine1, objUPSShipper.ShiperAddress.AddressLine2 };
                String[] addressLine01 = { objUPSShipper.ShiperAddress.AddressLine1 };
                if (objUPSShipper.ShiperAddress.AddressLine2 == null)
                {
                    shipperAddress.AddressLine = addressLine01;
                }
                else
                {
                    shipperAddress.AddressLine = addressLine;
                }

                shipper.Address = shipperAddress;
                shipperAddress.City = String.IsNullOrEmpty(objUPSShipper.ShiperAddress.City) ? string.Empty : objUPSShipper.ShiperAddress.City;
                shipperAddress.PostalCode = String.IsNullOrEmpty(objUPSShipper.ShiperAddress.PostalCode) ? string.Empty : objUPSShipper.ShiperAddress.PostalCode.Replace("-", string.Empty);
                shipperAddress.StateProvinceCode = String.IsNullOrEmpty(objUPSShipper.ShiperAddress.StateProvinceCode) ? string.Empty : objUPSShipper.ShiperAddress.StateProvinceCode;
                shipperAddress.CountryCode = objUPSShipper.ShiperAddress.CountryCode;

                shipper.Name = String.IsNullOrEmpty(objUPSShipper.ShiperName) ? string.Empty : objUPSShipper.ShiperName;
                shipper.AttentionName = String.IsNullOrEmpty(objUPSShipper.ShiperAttentionName) ? string.Empty : objUPSShipper.ShiperAttentionName;
                ShipPhoneType shipperPhone = new ShipPhoneType();
                shipperPhone.Number = String.IsNullOrEmpty(objUPSShipper.ShiperPhone) ? string.Empty : objUPSShipper.ShiperPhone.Replace("-", string.Empty);
                shipper.Phone = shipperPhone;
                shipment.Shipper = shipper;

                ShipFromType shipFrom = new ShipFromType();
                ShipAddressType shipFromAddress = new ShipAddressType();


                String[] shipFromAddressLine = { objUPSShipFrom.ShipFromAddress.AddressLine1, String.IsNullOrEmpty(objUPSShipFrom.ShipFromAddress.AddressLine2) ? string.Empty : objUPSShipFrom.ShipFromAddress.AddressLine2 };
                String[] shipFromAddressLine01 = { objUPSShipFrom.ShipFromAddress.AddressLine1 };

                if (objUPSShipFrom.ShipFromAddress.AddressLine2 == null)
                {
                    shipFromAddress.AddressLine = shipFromAddressLine01;
                }
                else
                {
                    shipFromAddress.AddressLine = shipFromAddressLine;
                }
                shipFromAddress.City = String.IsNullOrEmpty(objUPSShipFrom.ShipFromAddress.City) ? string.Empty : objUPSShipFrom.ShipFromAddress.City;
                shipFromAddress.PostalCode = String.IsNullOrEmpty(objUPSShipFrom.ShipFromAddress.PostalCode) ? string.Empty : objUPSShipFrom.ShipFromAddress.PostalCode.Replace("-", string.Empty);
                shipFromAddress.StateProvinceCode = String.IsNullOrEmpty(objUPSShipFrom.ShipFromAddress.StateProvinceCode) ? string.Empty : objUPSShipFrom.ShipFromAddress.StateProvinceCode;
                shipFromAddress.CountryCode = String.IsNullOrEmpty(objUPSShipFrom.ShipFromAddress.CountryCode) ? string.Empty : objUPSShipFrom.ShipFromAddress.CountryCode;
                shipFrom.Address = shipFromAddress;
                shipFrom.AttentionName = String.IsNullOrEmpty(objUPSShipFrom.ShipFromAttentionName) ? string.Empty : objUPSShipFrom.ShipFromAttentionName;
                shipFrom.Name = String.IsNullOrEmpty(objUPSShipFrom.ShipFromName) ? string.Empty : objUPSShipFrom.ShipFromName;
                shipment.ShipFrom = shipFrom;


                ShipToType shipTo = new ShipToType();
                ShipToAddressType shipToAddress = new ShipToAddressType();
                String[] shipToAddressLine = { objUPSShipTo.ShipToAddress.AddressLine1, objUPSShipTo.ShipToAddress.AddressLine2 };
                String[] shipToAddressLine01 = { objUPSShipTo.ShipToAddress.AddressLine1 };
                if (objUPSShipTo.ShipToAddress.AddressLine2 == null)
                {
                    shipToAddress.AddressLine = shipToAddressLine01;
                }
                else
                {
                    shipToAddress.AddressLine = shipToAddressLine;
                }


                shipToAddress.CountryCode = String.IsNullOrEmpty(objUPSShipTo.ShipToAddress.CountryCode) ? string.Empty : objUPSShipTo.ShipToAddress.CountryCode;
                shipToAddress.City = String.IsNullOrEmpty(objUPSShipTo.ShipToAddress.City) ? string.Empty : objUPSShipTo.ShipToAddress.City;
                shipToAddress.StateProvinceCode = String.IsNullOrEmpty(objUPSShipTo.ShipToAddress.StateProvinceCode) ? string.Empty : objUPSShipTo.ShipToAddress.StateProvinceCode;
                shipToAddress.PostalCode = String.IsNullOrEmpty(objUPSShipTo.ShipToAddress.PostalCode) ? string.Empty : objUPSShipTo.ShipToAddress.PostalCode.Replace("-", string.Empty);

                shipTo.Address = shipToAddress;
                shipTo.AttentionName = String.IsNullOrEmpty(objUPSShipTo.ShipToAttentionName) ? string.Empty : objUPSShipTo.ShipToAttentionName;
                shipTo.Name = String.IsNullOrEmpty(objUPSShipTo.ShipToName) ? string.Empty : objUPSShipTo.ShipToName;

                ShipPhoneType shipToPhone = new ShipPhoneType();
                shipToPhone.Number = String.IsNullOrEmpty(objUPSShipTo.ShipToPhone) ? string.Empty : objUPSShipTo.ShipToPhone.Replace("-", string.Empty);
                shipTo.Phone = shipToPhone;
                shipment.ShipTo = shipTo;


                ServiceType service = new ServiceType();
                service.Code = ServiceCode;
                shipment.Service = service;

                ShipmentTypeShipmentServiceOptions shpServiceOptions = new ShipmentTypeShipmentServiceOptions();

                /** **** International Forms ***** */
                InternationalFormType internationalForms = new InternationalFormType();

                /** **** Commercial Invoice ***** */
                String[] formTypeList = { "01" };
                internationalForms.FormType = formTypeList;

                ContactType contacts = new ContactType();

                SoldToType soldTo = new SoldToType();
                soldTo.Option = "1";

                soldTo.AttentionName = String.IsNullOrEmpty(objUPSSoldTo.SoldToAttentionName) ? string.Empty : objUPSSoldTo.SoldToAttentionName;
                soldTo.Name = String.IsNullOrEmpty(objUPSSoldTo.SoldToName) ? string.Empty : objUPSSoldTo.SoldToName;

                PhoneType soldToPhone = new PhoneType();
                soldToPhone.Number = objUPSSoldTo.SoldToPhone;
                soldToPhone.Number = String.IsNullOrEmpty(objUPSSoldTo.SoldToPhone) ? string.Empty : objUPSSoldTo.SoldToPhone.Replace("-", string.Empty);
                soldToPhone.Extension = String.Empty;
                soldTo.Phone = soldToPhone;
                AddressType soldToAddress = new AddressType();

                String[] soldToAddressLine = { objUPSSoldTo.SoldToAddress.AddressLine1, objUPSSoldTo.SoldToAddress.AddressLine2 };
                String[] soldToAddressLine01 = { objUPSShipTo.ShipToAddress.AddressLine1 };
                if (objUPSShipTo.ShipToAddress.AddressLine2 == null)
                {
                    soldToAddress.AddressLine = soldToAddressLine01;
                }
                else
                {
                    soldToAddress.AddressLine = soldToAddressLine;
                }

                soldToAddress.City = String.IsNullOrEmpty(objUPSSoldTo.SoldToAddress.City) ? string.Empty : objUPSSoldTo.SoldToAddress.City;
                soldToAddress.StateProvinceCode = String.IsNullOrEmpty(objUPSSoldTo.SoldToAddress.StateProvinceCode) ? string.Empty : objUPSSoldTo.SoldToAddress.StateProvinceCode;
                soldToAddress.PostalCode = String.IsNullOrEmpty(objUPSSoldTo.SoldToAddress.PostalCode) ? string.Empty : objUPSSoldTo.SoldToAddress.PostalCode.Replace("-", string.Empty);
                soldToAddress.CountryCode = String.IsNullOrEmpty(objUPSSoldTo.SoldToAddress.CountryCode) ? string.Empty : objUPSSoldTo.SoldToAddress.CountryCode;

                soldTo.Address = soldToAddress;
                contacts.SoldTo = soldTo;

                internationalForms.Contacts = contacts;
                int totCtns = 0;
                foreach (shipOdrerDtl objshipOdrerDtl in lstShipOdrerDtl)
                {
                    totCtns = totCtns + objshipOdrerDtl.ordr_ctns;
                }
                    ProductType[] productList = new ProductType[totCtns];
                int i = 0;
                foreach (shipOdrerDtl objshipOdrerDtl in lstShipOdrerDtl)
                {
                    for (int c = 0; c< objshipOdrerDtl.ordr_ctns; c++)
                    {
                        ProductType product1 = new ProductType();
                        String[] description = { objshipOdrerDtl.itm_name };
                        product1.Description = description;
                        product1.CommodityCode = objshipOdrerDtl.itm_num;
                        product1.OriginCountryCode = "US";
                        GsEPWv8_4_MVC.ShipWebReference.UnitType unit = new GsEPWv8_4_MVC.ShipWebReference.UnitType();
                        unit.Number = 1.ToString();
                        unit.Value = objshipOdrerDtl.sell_price.ToString("0.##");
                        UnitOfMeasurementType uomProduct = new UnitOfMeasurementType();
                        uomProduct.Code = "BOX";
                        uomProduct.Description = "BOX";
                        unit.UnitOfMeasurement = uomProduct;
                        product1.Unit = unit;
                        ProductWeightType productWeight = new ProductWeightType();
                        productWeight.Weight = objshipOdrerDtl.wgt.ToString();
                        UnitOfMeasurementType uomForWeight = new UnitOfMeasurementType();
                        uomForWeight.Code = "LBS";
                        uomForWeight.Description = "LBS";
                        productWeight.UnitOfMeasurement = uomForWeight;
                        product1.ProductWeight = productWeight;
                        productList[i] = product1;
                        i += 1;
                    }
                   


                }
                internationalForms.Product = productList;


                internationalForms.InvoiceNumber = String.IsNullOrEmpty(InvoiceNumber) ? string.Empty : InvoiceNumber;

                DateTime dtInvoiceDate;
                try
                {
                    if (DateTime.TryParse(InvoiceDate, out dtInvoiceDate))
                    {
                        internationalForms.InvoiceDate = String.IsNullOrEmpty(InvoiceDate) ? string.Empty : dtInvoiceDate.ToString("yyyyMMdd");
                    }
                    else
                    {
                        internationalForms.InvoiceDate = string.Empty;
                    }
                }
                catch (Exception)
                {
                    internationalForms.InvoiceDate = string.Empty;

                }


                internationalForms.PurchaseOrderNumber = String.IsNullOrEmpty(PurchaseOrderNumber) ? string.Empty : PurchaseOrderNumber;
                internationalForms.TermsOfShipment = String.IsNullOrEmpty(TermsOfShipment) ? string.Empty : TermsOfShipment;
                internationalForms.ReasonForExport = String.IsNullOrEmpty(TermsOfShipment) ? "ECOM Orders" : TermsOfShipment;
                internationalForms.Comments = String.IsNullOrEmpty(TermsOfShipment) ? string.Empty : Comments;

                IFChargesType discount = new IFChargesType();

                discount.MonetaryValue = "0";
                internationalForms.Discount = discount;
                IFChargesType freight = new IFChargesType();
                freight.MonetaryValue = "0";
                internationalForms.FreightCharges = freight;
                IFChargesType insurance = new IFChargesType();
                insurance.MonetaryValue = "0";
                internationalForms.InsuranceCharges = insurance;
                OtherChargesType otherCharges = new OtherChargesType();
                otherCharges.MonetaryValue = "0";
                otherCharges.Description = "Misc";
                internationalForms.OtherCharges = otherCharges;
                internationalForms.CurrencyCode = "USD";


                shpServiceOptions.InternationalForms = internationalForms;
                shipment.ShipmentServiceOptions = shpServiceOptions;

                PackageType[] pkgArray = new PackageType[totCtns];
                int j = 0;
                foreach (shipOdrerDtl objshipOdrerDtl in lstShipOdrerDtl)
                {
                    for (int c = 0; c < objshipOdrerDtl.ordr_ctns; c++)
                    {

                        PackageType package = new PackageType();
                        PackageWeightType packageWeight = new PackageWeightType();
                        packageWeight.Weight = objshipOdrerDtl.wgt.ToString();
                        ShipUnitOfMeasurementType uom = new ShipUnitOfMeasurementType();
                        uom.Code = "LBS";
                        packageWeight.UnitOfMeasurement = uom;
                        package.PackageWeight = packageWeight;
                        PackagingType packType = new PackagingType();
                        packType.Code = "02";
                        package.Packaging = packType;
                        pkgArray[j] = package;
                        j += 1;
                    }

                }
                shipment.Package = pkgArray;

                LabelSpecificationType labelSpec = new LabelSpecificationType();
                LabelStockSizeType labelStockSize = new LabelStockSizeType();
                labelStockSize.Height = "6";
                labelStockSize.Width = "4";
                labelSpec.LabelStockSize = labelStockSize;
                LabelImageFormatType labelImageFormat = new LabelImageFormatType();
                labelImageFormat.Code = "GIF";
                labelSpec.LabelImageFormat = labelImageFormat;
                shipmentRequest.LabelSpecification = labelSpec;
                shipmentRequest.Shipment = shipment;
                Console.WriteLine(shipmentRequest);
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11; //This line will ensure the latest security protocol for consuming the web service call.
                ShipmentResponse shipmentResponse = shpSvc.ProcessShipment(shipmentRequest);

                int k = 0;
                string lstrUPSLabelFullPath = Path.Combine(lstrUPSLabelPath, shipmentResponse.ShipmentResults.ShipmentIdentificationNumber.ToString());
                for (k = 0; k < shipmentResponse.ShipmentResults.PackageResults.Length; k++)
                {
                    string graphicImage = shipmentResponse.ShipmentResults.PackageResults[k].ShippingLabel.GraphicImage;
                    byte[] labelBuffer = Convert.FromBase64String(graphicImage);
                    string fileName = string.Format("{0}label{1}.gif",k.ToString("#000"), shipmentResponse.ShipmentResults.ShipmentIdentificationNumber.ToString());
                    string returnFileName = fileName;
                    if (!System.IO.Directory.Exists(lstrUPSLabelFullPath)) System.IO.Directory.CreateDirectory(lstrUPSLabelFullPath);
                    string pathToCheck = Path.Combine(lstrUPSLabelFullPath, returnFileName);
                    // Should just delete the old file (code copied from another project)  
                    if (System.IO.File.Exists(pathToCheck))
                    {
                        int counter = 1;
                        while (System.IO.File.Exists(pathToCheck))
                        {
                            returnFileName = counter.ToString() + fileName;
                            pathToCheck = Path.Combine(lstrUPSLabelFullPath, returnFileName);
                            counter++;
                        }
                    }
                    System.IO.FileStream LabelFile = new System.IO.FileStream(pathToCheck, System.IO.FileMode.Create);
                    LabelFile.Write(labelBuffer, 0, labelBuffer.Length);
                    LabelFile.Close();
                   
                }
                strFileName = Path.Combine(lstrUPSLabelFullPath, shipmentResponse.ShipmentResults.ShipmentIdentificationNumber.ToString());
                strFileName = string.Format("{0}.pdf", strFileName);
                ConvertImageToPDF(lstrUPSLabelFullPath, strFileName);


            }
            catch (Exception Ex)
            {
                return Json("ERROR-" + ((System.Xml.XmlElement)((System.Web.Services.Protocols.SoapException)Ex).Detail).InnerText, JsonRequestBehavior.AllowGet);

            }
            finally
            {

            }

            return Json(strFileName, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OBCtnLblPrnt(string p_str_cmp_id, string p_str_screen_title, string p_str_so_num)
        {
            string jsonErrorCode = "0";
            string l_str_error_message = string.Empty;
            try
            {
                OBCtnLblPrnt objOBCtnLblPrnt = new OBCtnLblPrnt();

                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                objOBCtnLblPrnt.cmp_id = p_str_cmp_id;
                objOBCtnLblPrnt.so_num = p_str_so_num;
                LabelPrintService objLabelPrintService = new LabelPrintService();
                objOBCtnLblPrnt = objLabelPrintService.GetOBLabelPrintDetails(objOBCtnLblPrnt);

                if (objOBCtnLblPrnt.LstCtnLabelPrint.Count > 0)
                {


                    LookUp objLookUp = new LookUp();
                    LookUpService ServiceObjects = new LookUpService();
                    objLookUp.id = "307";
                    objLookUp.lookuptype = "LBL-FORMAT";
                    objLookUp = ServiceObjects.GetLookUpValue(objLookUp);
                    objOBCtnLblPrnt.ListLblFormat = objLookUp.ListLookUpDtl;
                    objOBCtnLblPrnt.LstCtnLabelPrint[0].lbl_format = objOBCtnLblPrnt.ListLblFormat[0].name;
                    objLookUp = new LookUp();
                    objLookUp.id = "306";
                    objLookUp.lookuptype = "LBL-LAYOUT";
                    objLookUp = ServiceObjects.GetLookUpValue(objLookUp);
                    objOBCtnLblPrnt.ListLblLayout = objLookUp.ListLookUpDtl;
                    objOBCtnLblPrnt.LstCtnLabelPrint[0].lbl_layout = objOBCtnLblPrnt.ListLblLayout[0].name;

                    objOBCtnLblPrnt.LstCtnLabelPrint[0].ship_from = objOBCtnLblPrnt.LstCtnLabelPrint[0].ship_from;
                    objOBCtnLblPrnt.LstCtnLabelPrint[0].ship_from_addr1 = "   " + objOBCtnLblPrnt.LstCtnLabelPrint[0].ship_from_addr1;
                    objOBCtnLblPrnt.LstCtnLabelPrint[0].ship_from_addr2 = "   " + objOBCtnLblPrnt.LstCtnLabelPrint[0].ship_from_addr2;

                    objOBCtnLblPrnt.LstCtnLabelPrint[0].ship_to = objOBCtnLblPrnt.LstCtnLabelPrint[0].st_mail_name;
                    objOBCtnLblPrnt.LstCtnLabelPrint[0].ship_to_addr1 = objOBCtnLblPrnt.LstCtnLabelPrint[0].st_addr_line1;
                    objOBCtnLblPrnt.LstCtnLabelPrint[0].ship_to_addr2 = objOBCtnLblPrnt.LstCtnLabelPrint[0].st_city + " " + objOBCtnLblPrnt.LstCtnLabelPrint[0].st_state_id + " " + objOBCtnLblPrnt.LstCtnLabelPrint[0].st_post_code;
                    Mapper.CreateMap<OBCtnLblPrnt, OBCtnLblPrntModel>();
                    OBCtnLblPrntModel OBCtnLblPrntModel = Mapper.Map<OBCtnLblPrnt, OBCtnLblPrntModel>(objOBCtnLblPrnt);
                    return PartialView("_OBCtnLblPrnt", OBCtnLblPrntModel);
                }
                else

                {

                    return Json(new { result = jsonErrorCode, err = "Ship Request Details Not found" }, JsonRequestBehavior.AllowGet);
                }

            }

            catch (Exception ex)
            {
                l_str_error_message = ex.Message;
                jsonErrorCode = "-2";
            }

            return Json(new { result = jsonErrorCode, err = l_str_error_message }, JsonRequestBehavior.AllowGet);


        }

        public ActionResult GenerateCtnLabels(OBCtnLblPrnt objOBCtnLblPrnt)
        {
            const int leftMargin = 10;
            const int rightMargin = 0;
            const int topMargin = 0;
            const int bottomMargin = 0;
            const int myWidth = 150;
            const int myHeight = 100;


            int l_int_ctns = Convert.ToInt16(objOBCtnLblPrnt.tot_ctns);
            iTextSharp.text.Rectangle pgSize = new iTextSharp.text.Rectangle(myWidth, myHeight);


            string strPDFPath = System.Configuration.ConfigurationManager.AppSettings["Labelpath"].ToString().Trim();
            iTextSharp.text.Font mainFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font infoFont1 = FontFactory.GetFont("Arial", 6.5f, Font.NORMAL, BaseColor.BLACK);

            iTextSharp.text.Font mainFont2 = FontFactory.GetFont("Arial", 15, Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font infoFont2 = FontFactory.GetFont("Arial", 13f, Font.NORMAL, BaseColor.BLACK);

            string strFileName = string.Empty;
            try
            {
             
                strFileName = strPDFPath + "\\" + objOBCtnLblPrnt.cmp_id.Trim() + "-" + objOBCtnLblPrnt.so_num.Trim() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";

                if( (objOBCtnLblPrnt.lbl_format == "THERMAL") )
                {
                     mainFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK);
                    infoFont1 = FontFactory.GetFont("Arial", 6.5f, Font.NORMAL, BaseColor.BLACK);

                        mainFont2 = FontFactory.GetFont("Arial", 15, Font.NORMAL, BaseColor.BLACK);
                     infoFont2 = FontFactory.GetFont("Arial", 13f, Font.NORMAL, BaseColor.BLACK);

                    Document doc = new iTextSharp.text.Document(pgSize, leftMargin, rightMargin, topMargin, bottomMargin);
                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(strFileName, FileMode.Create));
                    doc.Open();

                    if ((objOBCtnLblPrnt.lbl_layout == "GENERAL"))
                    {


                        for (int i = 1; i <= l_int_ctns; i++)
                        {
                            PdfPTable modelInfoTable = new PdfPTable(1);

                            // modelInfoTable.TotalWidth = 100;
                            modelInfoTable.WidthPercentage = 100;
                            modelInfoTable.HorizontalAlignment = Element.ALIGN_CENTER;
                            modelInfoTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                            modelInfoTable.DefaultCell.Padding = 0;
                            modelInfoTable.DefaultCell.UseAscender = false;
                            modelInfoTable.DefaultCell.UseDescender = false;


                            Phrase ShipFromPhrase = new Phrase();
                            Chunk mChunkShipFromName = new Chunk(objOBCtnLblPrnt.ship_from.ToString().Trim() + " " + objOBCtnLblPrnt.ship_from_cmp.ToString().Trim() + "\n", mainFont);
                            ShipFromPhrase.Add(mChunkShipFromName);

                            PdfPCell ShipFromCell = new PdfPCell(ShipFromPhrase)
                            {
                                BorderWidth = 0
                            };
                            ShipFromCell.BorderWidth = 0;

                            ShipFromCell.PaddingLeft = 0;
                            ShipFromCell.PaddingRight = 0;
                            ShipFromCell.PaddingTop = 2;
                            ShipFromCell.PaddingBottom = 1;
                            ShipFromCell.SetLeading(0f, 1.2f);
                            ShipFromCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            modelInfoTable.AddCell(ShipFromCell);

                            Phrase ShipFromAddr1 = new Phrase();
                            Chunk mChunkShipFromAddr1 = new Chunk(objOBCtnLblPrnt.ship_from_addr1.ToString().Trim() + "\n", infoFont1);
                            ShipFromAddr1.Add(mChunkShipFromAddr1);
                            ShipFromCell = new PdfPCell(ShipFromAddr1);
                            ShipFromCell.PaddingLeft = 0;
                            ShipFromCell.PaddingRight = 0;
                            ShipFromCell.PaddingTop = 1;
                            ShipFromCell.PaddingBottom = 0;
                            ShipFromCell.SetLeading(0f, 1.0f);
                            ShipFromCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            modelInfoTable.AddCell(ShipFromCell);


                            iTextSharp.text.Font lineFont = FontFactory.GetFont("Kalinga", 8, new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8")));

                            Phrase LinePhrase = new Phrase();
                            Chunk lineChunk = new Chunk("______________________________", lineFont);
                            LinePhrase.Add(lineChunk);
                            modelInfoTable.AddCell(LinePhrase);


                            ShipFromPhrase = new Phrase();
                            Chunk mChunkBlank = new Chunk(string.Empty + "\n", mainFont);
                            ShipFromPhrase.Add(mChunkBlank);

                            modelInfoTable.AddCell(ShipFromPhrase);
                            ShipFromPhrase = new Phrase();
                            Chunk mChunkShiToName = new Chunk(objOBCtnLblPrnt.ship_to.ToString().Trim() + "\n", mainFont);
                            ShipFromPhrase.Add(mChunkShiToName);
                            modelInfoTable.AddCell(ShipFromPhrase);

                            // ShipToPhrase.Add(new Chunk(Environment.NewLine));

                            Phrase ShipToAddr1Phrase = new Phrase();
                            Chunk mChunkShipToAddr1 = new Chunk(objOBCtnLblPrnt.ship_to_addr1.ToString().Trim() + "\n", infoFont1);
                            ShipToAddr1Phrase.Add(mChunkShipToAddr1);
                            PdfPCell ShipToAddr1Cell = new PdfPCell(ShipToAddr1Phrase);

                            ShipToAddr1Cell.PaddingLeft = 0;
                            ShipToAddr1Cell.PaddingRight = 0;
                            ShipToAddr1Cell.PaddingTop = 0;
                            ShipToAddr1Cell.PaddingBottom = 5;

                            ShipToAddr1Cell.SetLeading(0f, 1.5f);
                            ShipToAddr1Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            modelInfoTable.AddCell(ShipToAddr1Cell);


                            Phrase ShipToAddr2Phrase = new Phrase();
                            Chunk mChunkShipToAddr2 = new Chunk(objOBCtnLblPrnt.ship_to_addr2.ToString().Trim() + "\n", infoFont1);
                            ShipToAddr2Phrase.Add(mChunkShipToAddr2);
                            PdfPCell ShipToAddr2Cell = new PdfPCell(ShipToAddr2Phrase);

                            ShipToAddr2Cell.PaddingLeft = 0;
                            ShipToAddr2Cell.PaddingRight = 0;
                            ShipToAddr2Cell.PaddingTop = 0;
                            ShipToAddr2Cell.PaddingBottom = 5;


                            // ShipToAddr2Cell.Padding = 0;
                            ShipToAddr2Cell.SetLeading(0f, .7f);
                            ShipToAddr2Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            modelInfoTable.AddCell(ShipToAddr2Cell);


                            Phrase BlankPhrase = new Phrase();
                            Chunk mChunkBlank1 = new Chunk(string.Empty + "\n", mainFont);
                            BlankPhrase.Add(mChunkBlank1);
                            modelInfoTable.AddCell(BlankPhrase);

                            ShipFromPhrase = new Phrase();
                            Chunk OrdrNumChunk = new Chunk("PO#: " + objOBCtnLblPrnt.cust_ordr_num.ToString().Trim() + "\n", mainFont);
                            ShipFromPhrase.Add(OrdrNumChunk);
                            modelInfoTable.AddCell(ShipFromPhrase);







                            Phrase ctnPhrase = new Phrase();
                            Chunk mChunkCtn = new Chunk("Ord#:" + objOBCtnLblPrnt.ordr_num.ToString().Trim() + "\t \t \t     " + i.ToString().Trim() + " OF " + l_int_ctns.ToString(), mainFont);
                            ctnPhrase.Add(mChunkCtn);
                            PdfPCell ctnCell = new PdfPCell(ctnPhrase);
                            ctnCell.PaddingLeft = 0;
                            ctnCell.PaddingRight = 0;
                            ctnCell.PaddingTop = 1;
                            ctnCell.PaddingBottom = 2;
                            ctnCell.SetLeading(0f, 1.2f);

                            //// ctnCell.Padding = 0;

                            ctnCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //  ctnCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            modelInfoTable.AddCell(ctnCell);


                            doc.Add(modelInfoTable);
                            doc.NewPage();
                        }
                    }


                    else if (((objOBCtnLblPrnt.lbl_layout == "TJX")) || ((objOBCtnLblPrnt.lbl_layout == "WALMART")))
                    {
                        int lintItmCount = objOBCtnLblPrnt.ListlblCtnStyle.Count;

                        mainFont = FontFactory.GetFont("Arial", 7, Font.NORMAL, BaseColor.BLACK);
                        infoFont1 = FontFactory.GetFont("Arial", 6f, Font.NORMAL, BaseColor.BLACK);

                        mainFont2 = FontFactory.GetFont("Arial", 6, Font.BOLD, BaseColor.BLACK);
                        iTextSharp.text.Font mainFont3 = FontFactory.GetFont("Arial",4, Font.BOLD, BaseColor.BLACK);
                        infoFont2 = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK);
                        int lintCount = 0;
                        for (int j = 0;j < lintItmCount; j++)
                        {
                            int lintItmCtns = objOBCtnLblPrnt.ListlblCtnStyle[j].ordr_ctns;

                        for (int i = 1; i <= lintItmCtns; i++)
                        {
                                lintCount += 1;

                            PdfPTable modelInfoTable = new PdfPTable(1);
                            modelInfoTable.WidthPercentage = 100;
                            modelInfoTable.HorizontalAlignment = Element.ALIGN_CENTER;
                            modelInfoTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                            modelInfoTable.DefaultCell.Padding = 0;
                            modelInfoTable.DefaultCell.UseAscender = false;
                            modelInfoTable.DefaultCell.UseDescender = false;


                            Phrase ShipFromPhrase = new Phrase();
                            Chunk mChunkShipFromName = new Chunk(objOBCtnLblPrnt.ship_from.ToString().Trim() + "\n", infoFont1);
                            ShipFromPhrase.Add(mChunkShipFromName);

                                PdfPCell ShipFromCell = new PdfPCell(ShipFromPhrase)
                                {
                                    BorderWidth = 0
                                };
                                ShipFromCell.BorderWidth = 0;

                                ShipFromCell.PaddingLeft = 0;
                                ShipFromCell.PaddingRight = 0;
                                ShipFromCell.PaddingTop = 2;
                                ShipFromCell.PaddingBottom = 1;
                                ShipFromCell.SetLeading(0f, 1.2f);
                                ShipFromCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                modelInfoTable.AddCell(ShipFromCell);


                                Phrase ShipFromCmpPhrase = new Phrase();
                                Chunk mChunkShipFromCmpName = new Chunk(objOBCtnLblPrnt.ship_from_cmp.ToString().Trim() + "\n", infoFont1);
                                ShipFromCmpPhrase.Add(mChunkShipFromCmpName);
                                modelInfoTable.AddCell(ShipFromCmpPhrase);
 
                            Phrase ShipFromAddr1 = new Phrase();
                            Chunk mChunkShipFromAddr1 = new Chunk(objOBCtnLblPrnt.ship_from_addr1.ToString().Trim() + "\n", infoFont1);
                            ShipFromAddr1.Add(mChunkShipFromAddr1);
                                modelInfoTable.AddCell(ShipFromAddr1);


                                iTextSharp.text.Font lineFont = FontFactory.GetFont("Kalinga", 5, new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8")));

                                Phrase LinePhrase = new Phrase();
                                Chunk lineChunk = new Chunk("------------------------------------------------------------------------------------", mainFont3);
                                LinePhrase.Add(lineChunk);
                                modelInfoTable.AddCell(LinePhrase);

                            //    iTextSharp.text.Font lineFont = FontFactory.GetFont("Kalinga", 5, new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8")));

                            //    Phrase LinePhrase = new Phrase();
                            //    Chunk lineChunk = new Chunk("_________________________________", mainFont);
                            //    LinePhrase.Add(lineChunk);

                            //    ShipFromCell = new PdfPCell(LinePhrase)
                            //    {
                            //        BorderWidth = 0
                                   
                            //};
                            //    ShipFromCell.FixedHeight = 30.0f;
                            //    //ShipFromCell.BorderWidth = 0;

                            //    //ShipFromCell.PaddingLeft = 0;
                            //    //ShipFromCell.PaddingRight = 0;
                            //    //ShipFromCell.PaddingTop = 0;
                            //    //ShipFromCell.PaddingBottom = 0;
                            //    ShipFromCell.HorizontalAlignment = Element.ALIGN_TOP;
                            //    //ShipFromCell.SetLeading(0f, 1.2f);
                            //    //ShipFromCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //    modelInfoTable.AddCell(LinePhrase);





                                if (String.IsNullOrEmpty(objOBCtnLblPrnt.store_id))
                                {

                                    ShipFromPhrase = new Phrase();
                                    Chunk mChunkShiToName = new Chunk(objOBCtnLblPrnt.ship_to.ToString().Trim() + "\n", mainFont2);
                                    ShipFromPhrase.Add(mChunkShiToName);
                                    modelInfoTable.AddCell(ShipFromPhrase);

                                }
                                else
                                {
                                    ShipFromPhrase = new Phrase();
                                    Chunk mChunkShiToName = new Chunk(objOBCtnLblPrnt.ship_to.ToString().Trim() + "\t (DC# " + objOBCtnLblPrnt.store_id.ToString().Trim() + ")\n", mainFont2);
                                    ShipFromPhrase.Add(mChunkShiToName);
                                    modelInfoTable.AddCell(ShipFromPhrase);
                                }


                                // ShipToPhrase.Add(new Chunk(Environment.NewLine));

                                Phrase ShipToAddr1Phrase = new Phrase();
                                Chunk mChunkShipToAddr1 = new Chunk( objOBCtnLblPrnt.ship_to_addr1.ToString().Trim() + "\n", infoFont1);
                                ShipToAddr1Phrase.Add(mChunkShipToAddr1);
                                modelInfoTable.AddCell(ShipToAddr1Phrase);

                                Phrase ShipToAddr2Phrase = new Phrase();
                                Chunk mChunkShipToAddr2 = new Chunk(objOBCtnLblPrnt.ship_to_addr2.ToString().Trim() + "\n", infoFont1);
                                ShipToAddr2Phrase.Add(mChunkShipToAddr2);
                                modelInfoTable.AddCell(ShipToAddr2Phrase);


                                modelInfoTable.AddCell(LinePhrase);


                                if (String.IsNullOrEmpty(objOBCtnLblPrnt.store_id))
                                {

                                    ShipFromPhrase = new Phrase();
                                    Chunk OrdrNumChunk = new Chunk("PO#: " + objOBCtnLblPrnt.cust_ordr_num.ToString().Trim(), mainFont2);
                                    ShipFromPhrase.Add(OrdrNumChunk);
                                    modelInfoTable.AddCell(ShipFromPhrase);

                                }
                                else
                                {
                                    ShipFromPhrase = new Phrase();
                                    Chunk OrdrNumChunk = new Chunk("PO#: " + objOBCtnLblPrnt.cust_ordr_num.ToString().Trim(), mainFont2);
                                    ShipFromPhrase.Add(OrdrNumChunk);
                                    modelInfoTable.AddCell(ShipFromPhrase);

                                    ShipFromPhrase = new Phrase();
                                    OrdrNumChunk = new Chunk("DEPT#: " + objOBCtnLblPrnt.dept_id.ToString().Trim(), infoFont1);
                                    ShipFromPhrase.Add(OrdrNumChunk);
                                    modelInfoTable.AddCell(ShipFromPhrase);
                                }



                                ShipFromPhrase = new Phrase();
                                Chunk itmNumChunk = new Chunk("STYLE#: " + objOBCtnLblPrnt.ListlblCtnStyle[j].itm_num.ToString() + "       TOTAL UNITs: " + objOBCtnLblPrnt.ListlblCtnStyle[j].itm_qty.ToString(), mainFont2);
                                ShipFromPhrase.Add(itmNumChunk);
                                modelInfoTable.AddCell(ShipFromPhrase);

                                string lstrStoreready = objOBCtnLblPrnt.store_ready == true ? "YES" : "NO";
                                string lstrPreTickted = objOBCtnLblPrnt.pre_ticketed == true ? "YES" : "NO";

                                Phrase StoreReadyPhrase = new Phrase();
                                Chunk StoreReadChunk = new Chunk("Store Ready: " + lstrStoreready + "\t  Pretickted: " + lstrPreTickted, infoFont1);
                                StoreReadyPhrase.Add(StoreReadChunk);
                                modelInfoTable.AddCell(StoreReadyPhrase);

                                Phrase CntryOfOrginTicktedPhrase = new Phrase();
                                Chunk CntryOfOrginChunk = new Chunk("Country Of Orgin " + objOBCtnLblPrnt.country_of_orgin.Trim(), infoFont1);
                                CntryOfOrginTicktedPhrase.Add(CntryOfOrginChunk);
                                modelInfoTable.AddCell(CntryOfOrginTicktedPhrase);

                                Phrase CtnNoPhrase = new Phrase();
                                Chunk CtnNoChunk = new Chunk( "              CTN#: " + lintCount.ToString().Trim() + " OF " + l_int_ctns.ToString(), mainFont2);
                                CtnNoPhrase.Add(CtnNoChunk);
                                modelInfoTable.AddCell(CtnNoPhrase);


                                doc.Add(modelInfoTable);
                            doc.NewPage();
                        }
                    }

                    }
                    doc.Close();
                }

                else if( (objOBCtnLblPrnt.lbl_format == "LASER") ||   (objOBCtnLblPrnt.lbl_format == "CSV"))
                {
                    mainFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK);
                    infoFont1 = FontFactory.GetFont("Arial", 6.5f, Font.NORMAL, BaseColor.BLACK);

                    mainFont2 = FontFactory.GetFont("Arial", 15, Font.NORMAL, BaseColor.BLACK);
                    infoFont2 = FontFactory.GetFont("Arial", 13f, Font.NORMAL, BaseColor.BLACK);
                    Document pdfDoc = new Document(PageSize.A4, 25, 25, 40, 40);

                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(strFileName, FileMode.Create));
                    pdfDoc.Open();

                    for (int i = 1; i <= l_int_ctns; i++)
                    {
                        PdfPTable modelInfoTableMain = new PdfPTable(2);

                        modelInfoTableMain.WidthPercentage = 100;
                        modelInfoTableMain.HorizontalAlignment = 0;
                        modelInfoTableMain.SpacingBefore = 50f; ;
                        modelInfoTableMain.SpacingAfter = 50f;


                        PdfPTable modelInfoTable = new PdfPTable(1);
                        modelInfoTable.WidthPercentage = 100;
                        modelInfoTable.HorizontalAlignment = Element.ALIGN_CENTER;
                        modelInfoTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                        modelInfoTable.DefaultCell.Padding = 0;
                        modelInfoTable.DefaultCell.UseAscender = false;
                        modelInfoTable.DefaultCell.UseDescender = false;


                        Phrase ShipFromPhrase = new Phrase();
                        Chunk mChunkShipFromName = new Chunk(objOBCtnLblPrnt.ship_from.ToString().Trim() + "\n", mainFont2);
                        ShipFromPhrase.Add(mChunkShipFromName);

                        PdfPCell ShipFromCell = new PdfPCell(ShipFromPhrase);

                        ShipFromCell.BorderWidth = 0;
                        ShipFromCell.PaddingLeft = 0;
                        ShipFromCell.PaddingRight = 0;
                        ShipFromCell.PaddingTop = 2;
                        ShipFromCell.PaddingBottom = 1;
                        ShipFromCell.SetLeading(0f, 1.2f);
                        ShipFromCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        modelInfoTable.AddCell(ShipFromCell);

                        Phrase ShipFromAddr1 = new Phrase();
                        Chunk mChunkShipFromAddr1 = new Chunk(objOBCtnLblPrnt.ship_from_addr1.ToString().Trim() + "\n", infoFont2);
                        ShipFromAddr1.Add(mChunkShipFromAddr1);
                        ShipFromCell = new PdfPCell(ShipFromAddr1);
                        ShipFromCell.PaddingLeft = 0;
                        ShipFromCell.PaddingRight = 0;
                        ShipFromCell.PaddingTop = 2;
                        ShipFromCell.PaddingBottom = 0;
                        ShipFromCell.SetLeading(0f, 1.0f);
                        ShipFromCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        modelInfoTable.AddCell(ShipFromCell);


                        iTextSharp.text.Font lineFont = FontFactory.GetFont("Kalinga", 12, new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8")));

                        Phrase LinePhrase = new Phrase();
                        Chunk lineChunk = new Chunk("______________________________", lineFont);
                        LinePhrase.Add(lineChunk);
                        PdfPCell cellBlankLine = new PdfPCell(LinePhrase);
                        cellBlankLine.PaddingLeft = 0;
                        cellBlankLine.PaddingRight = 0;
                        cellBlankLine.PaddingTop = 2;
                        cellBlankLine.PaddingBottom = 10;
                        cellBlankLine.SetLeading(0f, 1.0f);
                        cellBlankLine.Border = iTextSharp.text.Rectangle.NO_BORDER;

                        modelInfoTable.AddCell(cellBlankLine);


                        //ShipFromPhrase = new Phrase();
                        //Chunk mChunkBlank = new Chunk(string.Empty + "\n", mainFont2);
                        //ShipFromPhrase.Add(mChunkBlank);
                        //modelInfoTable.AddCell(ShipFromPhrase);


                        Phrase ShipToPhrase = new Phrase();
                        Chunk mChunkShiToName = new Chunk(objOBCtnLblPrnt.ship_to.ToString().Trim() + "\n", mainFont2);
                        ShipToPhrase.Add(mChunkShiToName);

                        PdfPCell cellShipTo = new PdfPCell(ShipToPhrase);
                        cellShipTo.PaddingLeft = 0;
                        cellShipTo.PaddingRight = 0;
                        cellShipTo.PaddingTop = 2;
                        cellShipTo.PaddingBottom = 0;
                        cellShipTo.SetLeading(0f, 1.0f);
                        cellShipTo.Border = iTextSharp.text.Rectangle.NO_BORDER;


                        modelInfoTable.AddCell(cellShipTo);

                        // ShipToPhrase.Add(new Chunk(Environment.NewLine));

                        Phrase ShipToAddr1Phrase = new Phrase();
                        Chunk mChunkShipToAddr1 = new Chunk(objOBCtnLblPrnt.ship_to_addr1.ToString().Trim() + "\n", infoFont2);
                        ShipToAddr1Phrase.Add(mChunkShipToAddr1);
                        PdfPCell ShipToAddr1Cell = new PdfPCell(ShipToAddr1Phrase);

                        ShipToAddr1Cell.PaddingLeft = 0;
                        ShipToAddr1Cell.PaddingRight = 0;
                        ShipToAddr1Cell.PaddingTop = 0;
                        ShipToAddr1Cell.PaddingBottom = 5;

                        ShipToAddr1Cell.SetLeading(0f, 1.5f);
                        ShipToAddr1Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        modelInfoTable.AddCell(ShipToAddr1Cell);


                        Phrase ShipToAddr2Phrase = new Phrase();
                        Chunk mChunkShipToAddr2 = new Chunk(objOBCtnLblPrnt.ship_to_addr2.ToString().Trim() + "\n", infoFont2);
                        ShipToAddr2Phrase.Add(mChunkShipToAddr2);
                        PdfPCell ShipToAddr2Cell = new PdfPCell(ShipToAddr2Phrase);

                        ShipToAddr2Cell.PaddingLeft = 0;
                        ShipToAddr2Cell.PaddingRight = 0;
                        ShipToAddr2Cell.PaddingTop = 0;
                        ShipToAddr2Cell.PaddingBottom = 5;


                        // ShipToAddr2Cell.Padding = 0;
                        ShipToAddr2Cell.SetLeading(0f, .7f);
                        ShipToAddr2Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        modelInfoTable.AddCell(ShipToAddr2Cell);


                        Phrase BlankPhrase = new Phrase();
                        Chunk mChunkBlank1 = new Chunk(string.Empty + "\n", mainFont2);
                        BlankPhrase.Add(mChunkBlank1);
                        modelInfoTable.AddCell(BlankPhrase);

                        Phrase POPhrase = new Phrase();
                        Chunk OrdrNumChunk = new Chunk("PO#: " + objOBCtnLblPrnt.cust_ordr_num.ToString().Trim() + "\n", mainFont2);
                        POPhrase.Add(OrdrNumChunk);
                        PdfPCell cellPONum = new PdfPCell(POPhrase);
                        cellPONum.PaddingLeft = 0;
                        cellPONum.PaddingRight = 0;
                        cellPONum.PaddingTop = 0;
                        cellPONum.PaddingBottom = 5;
                        cellPONum.SetLeading(0f, .7f);
                        cellPONum.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        modelInfoTable.AddCell(cellPONum);


                        Phrase ctnPhrase = new Phrase();
                        Chunk mChunkCtn = new Chunk("Ord#:" + objOBCtnLblPrnt.ordr_num.ToString().Trim() + "\t \t \t     " + i.ToString().Trim() + " OF " + l_int_ctns.ToString(), mainFont2);
                        ctnPhrase.Add(mChunkCtn);
                        PdfPCell ctnCell = new PdfPCell(ctnPhrase);
                        ctnCell.PaddingLeft = 0;
                        ctnCell.PaddingRight = 0;
                        ctnCell.PaddingTop = 1;
                        ctnCell.PaddingBottom = 2;
                        ctnCell.SetLeading(0f, 1.2f);

                        //// ctnCell.Padding = 0;

                        ctnCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //  ctnCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        modelInfoTable.AddCell(ctnCell);


                        PdfPCell cellAddress = new PdfPCell(modelInfoTable);
                        cellAddress.Border = 0;
                        cellAddress.Border = iTextSharp.text.Rectangle.NO_BORDER;


                        modelInfoTableMain.AddCell(cellAddress);
                        pdfDoc.Add(modelInfoTableMain);

                        if (i < l_int_ctns)
                        {
                            i = i + 1;


                            PdfPTable modelInfoTable2 = new PdfPTable(1);

                            // modelInfoTable2.TotalWidth = 100;
                            modelInfoTable2.WidthPercentage = 100;
                            modelInfoTable2.HorizontalAlignment = Element.ALIGN_CENTER;
                            modelInfoTable2.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                            modelInfoTable2.DefaultCell.Padding = 0;
                            modelInfoTable2.DefaultCell.UseAscender = false;
                            modelInfoTable2.DefaultCell.UseDescender = false;


                            Phrase ShipFromPhrase2 = new Phrase();
                            Chunk mChunkShipFromName2 = new Chunk(objOBCtnLblPrnt.ship_from.ToString().Trim() + "\n", mainFont2);
                            ShipFromPhrase2.Add(mChunkShipFromName2);

                            PdfPCell ShipFromCell2 = new PdfPCell(ShipFromPhrase2);

                            ShipFromCell2.BorderWidth = 0;
                            ShipFromCell2.PaddingLeft = 25;
                            ShipFromCell2.PaddingRight = 0;
                            ShipFromCell2.PaddingTop = 2;
                            ShipFromCell2.PaddingBottom = 1;
                            ShipFromCell2.SetLeading(0f, 1.2f);
                            ShipFromCell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            modelInfoTable2.AddCell(ShipFromCell2);


                            Phrase PhraseShipFromAddr12 = new Phrase();
                            Chunk mChunkShipFromAddr12 = new Chunk(objOBCtnLblPrnt.ship_from_addr1.ToString().Trim() + "\n", infoFont2);
                            PhraseShipFromAddr12.Add(mChunkShipFromAddr12);
                            PdfPCell ShipFromAddrCell2 = new PdfPCell(PhraseShipFromAddr12);
                            ShipFromAddrCell2.PaddingLeft = 25;
                            ShipFromAddrCell2.PaddingRight = 0;
                            ShipFromAddrCell2.PaddingTop = 2;
                            ShipFromAddrCell2.PaddingBottom = 0;
                            ShipFromAddrCell2.SetLeading(0f, 1.0f);
                            ShipFromAddrCell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            modelInfoTable2.AddCell(ShipFromAddrCell2);




                            Phrase LinePhrase2 = new Phrase();
                            Chunk lineChunk2 = new Chunk("______________________________", lineFont);
                            LinePhrase2.Add(lineChunk2);
                            PdfPCell cellBlankLine2 = new PdfPCell(LinePhrase2);
                            cellBlankLine2.PaddingLeft = 25;
                            cellBlankLine2.PaddingRight = 0;
                            cellBlankLine2.PaddingTop = 2;
                            cellBlankLine2.PaddingBottom = 10;
                            cellBlankLine2.SetLeading(0f, 1.0f);
                            cellBlankLine2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            modelInfoTable2.AddCell(cellBlankLine2);



                            Phrase ShipToPhrase2 = new Phrase();
                            Chunk mChunkShiToName2 = new Chunk(objOBCtnLblPrnt.ship_to.ToString().Trim() + "\n", mainFont2);
                            ShipToPhrase2.Add(mChunkShiToName2);
                            PdfPCell ShipToCell2 = new PdfPCell(ShipToPhrase2);
                            ShipToCell2.PaddingLeft = 25;
                            ShipToCell2.PaddingRight = 0;
                            ShipToCell2.PaddingTop = 2;
                            ShipToCell2.PaddingBottom = 0;

                            ShipToCell2.SetLeading(0f, 1.0f);
                            ShipToCell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            modelInfoTable2.AddCell(ShipToCell2);



                            Phrase ShipToAddr1Phrase2 = new Phrase();
                            Chunk mChunkShipToAddr12 = new Chunk(objOBCtnLblPrnt.ship_to_addr1.ToString().Trim() + "\n", infoFont2);
                            ShipToAddr1Phrase2.Add(mChunkShipToAddr12);
                            PdfPCell ShipToAddr1Cell2 = new PdfPCell(ShipToAddr1Phrase2);

                            ShipToAddr1Cell2.PaddingLeft = 25;
                            ShipToAddr1Cell2.PaddingRight = 0;
                            ShipToAddr1Cell2.PaddingTop = 0;
                            ShipToAddr1Cell2.PaddingBottom = 5;

                            ShipToAddr1Cell2.SetLeading(0f, 1.5f);
                            ShipToAddr1Cell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            modelInfoTable2.AddCell(ShipToAddr1Cell2);


                            Phrase ShipToAddr2Phrase2 = new Phrase();
                            Chunk mChunkShipToAddr22 = new Chunk(objOBCtnLblPrnt.ship_to_addr2.ToString().Trim() + "\n", infoFont2);
                            ShipToAddr2Phrase2.Add(mChunkShipToAddr22);
                            PdfPCell ShipToAddr2Cell2 = new PdfPCell(ShipToAddr2Phrase2);

                            ShipToAddr2Cell2.PaddingLeft = 25;
                            ShipToAddr2Cell2.PaddingRight = 0;
                            ShipToAddr2Cell2.PaddingTop = 0;
                            ShipToAddr2Cell2.PaddingBottom = 5;


                            // ShipToAddr2Cell.Padding = 0;
                            ShipToAddr2Cell2.SetLeading(0f, .7f);
                            ShipToAddr2Cell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            modelInfoTable2.AddCell(ShipToAddr2Cell2);


                            Phrase BlankPhrase2 = new Phrase();
                            Chunk mChunkBlank12 = new Chunk(string.Empty + "\n", mainFont2);
                            BlankPhrase2.Add(mChunkBlank12);
                            modelInfoTable2.AddCell(BlankPhrase2);

                            Phrase POPhrase2 = new Phrase();
                            Chunk OrdrNumChunk2 = new Chunk("PO#: " + objOBCtnLblPrnt.cust_ordr_num.ToString().Trim() + "\n", mainFont2);
                            POPhrase2.Add(OrdrNumChunk2);
                            PdfPCell cellPONum2 = new PdfPCell(POPhrase2);

                            cellPONum2.PaddingLeft = 25;
                            cellPONum2.PaddingRight = 0;
                            cellPONum2.PaddingTop = 0;
                            cellPONum2.PaddingBottom = 5;
                            cellPONum2.SetLeading(0f, .7f);
                            cellPONum2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            modelInfoTable2.AddCell(cellPONum2);





                            Phrase ctnPhrase2 = new Phrase();
                            Chunk mChunkCtn2 = new Chunk("Ord#:" + objOBCtnLblPrnt.ordr_num.ToString().Trim() + "\t \t \t     " + i.ToString().Trim() + " OF " + l_int_ctns.ToString(), mainFont2);
                            ctnPhrase2.Add(mChunkCtn2);
                            PdfPCell ctnCell2 = new PdfPCell(ctnPhrase2);

                            ctnCell2.PaddingLeft = 25;
                            ctnCell2.PaddingRight = 0;
                            ctnCell2.PaddingTop = 1;
                            ctnCell2.PaddingBottom = 5;
                            ctnCell2.SetLeading(0f, 1.2f);

                            //// ctnCell.Padding = 0;

                            ctnCell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //  ctnCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            modelInfoTable2.AddCell(ctnCell2);




                            PdfPCell cellAddress1 = new PdfPCell(modelInfoTable2);
                            cellAddress1.Border = 0;
                            cellAddress1.Border = iTextSharp.text.Rectangle.NO_BORDER;

                            //modelInfoTableMain.WidthPercentage = 100;
                            //modelInfoTableMain.HorizontalAlignment = 0;
                            //modelInfoTableMain.SpacingBefore = 50f;
                            //modelInfoTableMain.SpacingAfter = 50f;

                            modelInfoTableMain.AddCell(cellAddress1);
                            pdfDoc.Add(modelInfoTableMain);

                        }
                        else
                        {
                            PdfPTable modelInfoTable2 = new PdfPTable(1);
                            PdfPCell cellAddress1 = new PdfPCell(modelInfoTable2);
                            cellAddress1.Border = 0;
                            cellAddress1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            modelInfoTableMain.AddCell(cellAddress1);
                            pdfDoc.Add(modelInfoTableMain);

                        }


                        if ((i % 6) == 0)
                        {
                            pdfDoc.NewPage();
                        }
                    }
                    pdfDoc.Close();
                }
            }
            catch (Exception Ex)
            {
                return Json(Ex.InnerException.ToString(), JsonRequestBehavior.AllowGet);

            }
            finally
            {

            }

            return Json(strFileName, JsonRequestBehavior.AllowGet);
        }


        public ActionResult OBstdBOL(string p_str_cmp_id, string p_str_batch_id, string p_str_so_num, string p_str_so_num_from, string p_str_so_num_to,
            string p_str_so_dt_from, string p_str_so_dt_to, string p_str_screen_title, string export_type)
        {
            string strReportName = string.Empty;
            string jsonErrorCode = string.Empty;
            string msg = string.Empty;
            try
            {
                strReportName = "rpt_ob_sr_bol_conf.rpt";
                OBGetSRBOLConfRpt objOBGetBOLConf = new OBGetSRBOLConfRpt();
                OutboundInqService ServiceObject = new OutboundInqService();

                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                objOBGetBOLConf.cmp_id = p_str_cmp_id;


                objOBGetBOLConf = ServiceObject.GetOBSRBOLConfRpt(objOBGetBOLConf, p_str_cmp_id, p_str_batch_id, p_str_so_num, p_str_so_num_from, p_str_so_num_to, p_str_so_dt_from, p_str_so_dt_to);
                //CR20180623-001

                if (objOBGetBOLConf.ListOBGetSRBOLConfRpt.Count > 0)
                {


                }
                else
                {
                    //msg = "No Record Found";
                    //jsonErrorCode = "-2";
                    //return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);
                    Response.Write("<H2>Report not found</H2>");
                }
                //END
                if (export_type == "PDF")
                {
                    var rptSource = objOBGetBOLConf.ListOBGetSRBOLConfRpt.ToList();
                    if (rptSource.Count > 0)
                    {
                        using (ReportDocument rd = new ReportDocument())
                        {
                            rd.Load(strRptPath);

                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            rd.SetParameterValue("param_cmp_name", System.Configuration.ConfigurationManager.AppSettings["rptCmpName"].ToString().Trim());
                            rd.SetParameterValue("param_cmp_address", System.Configuration.ConfigurationManager.AppSettings["rptCmpAddress"].ToString().Trim());
                            rd.SetParameterValue("param_cmp_city_state_zip", System.Configuration.ConfigurationManager.AppSettings["rptCmpCityStateZzip"].ToString().Trim());

                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
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


        public ActionResult GenerateOBstdBOL(OBCtnLblPrnt objOBCtnLblPrnt)
        {
            const int leftMargin = 10;
            const int rightMargin = 0;
            const int topMargin = 0;
            const int bottomMargin = 0;
            const int myWidth = 150;
            const int myHeight = 100;
            int l_int_ctns = Convert.ToInt16(objOBCtnLblPrnt.tot_ctns);
            iTextSharp.text.Rectangle pgSize = new iTextSharp.text.Rectangle(myWidth, myHeight);


            string strPDFPath = System.Configuration.ConfigurationManager.AppSettings["Labelpath"].ToString().Trim();
            iTextSharp.text.Font mainFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font infoFont1 = FontFactory.GetFont("Arial", 6.5f, Font.NORMAL, BaseColor.BLACK);

            iTextSharp.text.Font mainFont2 = FontFactory.GetFont("Arial", 15, Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font infoFont2 = FontFactory.GetFont("Arial", 13f, Font.NORMAL, BaseColor.BLACK);

            string strFileName = string.Empty;
            try
            {

                strFileName = strPDFPath + "\\" + objOBCtnLblPrnt.cmp_id.Trim() + "-" + objOBCtnLblPrnt.so_num.Trim() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";

                if (objOBCtnLblPrnt.lbl_per_page == "SigleLbl")
                {
                    Document doc = new iTextSharp.text.Document(pgSize, leftMargin, rightMargin, topMargin, bottomMargin);
                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(strFileName, FileMode.Create));
                    doc.Open();
                    for (int i = 1; i <= l_int_ctns; i++)
                    {
                        PdfPTable modelInfoTable = new PdfPTable(1);

                        // modelInfoTable.TotalWidth = 100;
                        modelInfoTable.WidthPercentage = 100;
                        modelInfoTable.HorizontalAlignment = Element.ALIGN_CENTER;
                        modelInfoTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                        modelInfoTable.DefaultCell.Padding = 0;
                        modelInfoTable.DefaultCell.UseAscender = false;
                        modelInfoTable.DefaultCell.UseDescender = false;


                        Phrase ShipFromPhrase = new Phrase();
                        Chunk mChunkShipFromName = new Chunk(objOBCtnLblPrnt.ship_from.ToString().Trim() + "\n", mainFont);
                        ShipFromPhrase.Add(mChunkShipFromName);

                        PdfPCell ShipFromCell = new PdfPCell(ShipFromPhrase)
                        {
                            BorderWidth = 0
                        };
                        ShipFromCell.BorderWidth = 0;

                        ShipFromCell.PaddingLeft = 0;
                        ShipFromCell.PaddingRight = 0;
                        ShipFromCell.PaddingTop = 2;
                        ShipFromCell.PaddingBottom = 1;
                        ShipFromCell.SetLeading(0f, 1.2f);
                        ShipFromCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        modelInfoTable.AddCell(ShipFromCell);

                        Phrase ShipFromAddr1 = new Phrase();
                        Chunk mChunkShipFromAddr1 = new Chunk(objOBCtnLblPrnt.ship_from_addr1.ToString().Trim() + "\n", infoFont1);
                        ShipFromAddr1.Add(mChunkShipFromAddr1);
                        ShipFromCell = new PdfPCell(ShipFromAddr1);
                        ShipFromCell.PaddingLeft = 0;
                        ShipFromCell.PaddingRight = 0;
                        ShipFromCell.PaddingTop = 1;
                        ShipFromCell.PaddingBottom = 0;
                        ShipFromCell.SetLeading(0f, 1.0f);
                        ShipFromCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        modelInfoTable.AddCell(ShipFromCell);


                        iTextSharp.text.Font lineFont = FontFactory.GetFont("Kalinga", 8, new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8")));

                        Phrase LinePhrase = new Phrase();
                        Chunk lineChunk = new Chunk("______________________________", lineFont);
                        LinePhrase.Add(lineChunk);
                        modelInfoTable.AddCell(LinePhrase);


                        ShipFromPhrase = new Phrase();
                        Chunk mChunkBlank = new Chunk(string.Empty + "\n", mainFont);
                        ShipFromPhrase.Add(mChunkBlank);

                        modelInfoTable.AddCell(ShipFromPhrase);
                        ShipFromPhrase = new Phrase();
                        Chunk mChunkShiToName = new Chunk(objOBCtnLblPrnt.ship_to.ToString().Trim() + "\n", mainFont);
                        ShipFromPhrase.Add(mChunkShiToName);
                        modelInfoTable.AddCell(ShipFromPhrase);

                        // ShipToPhrase.Add(new Chunk(Environment.NewLine));

                        Phrase ShipToAddr1Phrase = new Phrase();
                        Chunk mChunkShipToAddr1 = new Chunk(objOBCtnLblPrnt.ship_to_addr1.ToString().Trim() + "\n", infoFont1);
                        ShipToAddr1Phrase.Add(mChunkShipToAddr1);
                        PdfPCell ShipToAddr1Cell = new PdfPCell(ShipToAddr1Phrase);

                        ShipToAddr1Cell.PaddingLeft = 0;
                        ShipToAddr1Cell.PaddingRight = 0;
                        ShipToAddr1Cell.PaddingTop = 0;
                        ShipToAddr1Cell.PaddingBottom = 5;

                        ShipToAddr1Cell.SetLeading(0f, 1.5f);
                        ShipToAddr1Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        modelInfoTable.AddCell(ShipToAddr1Cell);


                        Phrase ShipToAddr2Phrase = new Phrase();
                        Chunk mChunkShipToAddr2 = new Chunk(objOBCtnLblPrnt.ship_to_addr2.ToString().Trim() + "\n", infoFont1);
                        ShipToAddr2Phrase.Add(mChunkShipToAddr2);
                        PdfPCell ShipToAddr2Cell = new PdfPCell(ShipToAddr2Phrase);

                        ShipToAddr2Cell.PaddingLeft = 0;
                        ShipToAddr2Cell.PaddingRight = 0;
                        ShipToAddr2Cell.PaddingTop = 0;
                        ShipToAddr2Cell.PaddingBottom = 5;


                        // ShipToAddr2Cell.Padding = 0;
                        ShipToAddr2Cell.SetLeading(0f, .7f);
                        ShipToAddr2Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        modelInfoTable.AddCell(ShipToAddr2Cell);


                        Phrase BlankPhrase = new Phrase();
                        Chunk mChunkBlank1 = new Chunk(string.Empty + "\n", mainFont);
                        BlankPhrase.Add(mChunkBlank1);
                        modelInfoTable.AddCell(BlankPhrase);

                        ShipFromPhrase = new Phrase();
                        Chunk OrdrNumChunk = new Chunk("PO#: " + objOBCtnLblPrnt.cust_ordr_num.ToString().Trim() + "\n", mainFont);
                        ShipFromPhrase.Add(OrdrNumChunk);
                        modelInfoTable.AddCell(ShipFromPhrase);


                        Phrase ctnPhrase = new Phrase();
                        Chunk mChunkCtn = new Chunk("Ord#:" + objOBCtnLblPrnt.ordr_num.ToString().Trim() + "\t \t \t     " + i.ToString().Trim() + " OF " + l_int_ctns.ToString(), mainFont);
                        ctnPhrase.Add(mChunkCtn);
                        PdfPCell ctnCell = new PdfPCell(ctnPhrase);
                        ctnCell.PaddingLeft = 0;
                        ctnCell.PaddingRight = 0;
                        ctnCell.PaddingTop = 1;
                        ctnCell.PaddingBottom = 2;
                        ctnCell.SetLeading(0f, 1.2f);

                        //// ctnCell.Padding = 0;

                        ctnCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //  ctnCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        modelInfoTable.AddCell(ctnCell);


                        doc.Add(modelInfoTable);
                        doc.NewPage();
                    }
                    doc.Close();
                }

                else
                {
                    Document pdfDoc = new Document(PageSize.A4, 25, 25, 40, 40);

                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(strFileName, FileMode.Create));
                    pdfDoc.Open();

                    for (int i = 1; i <= l_int_ctns; i++)
                    {
                        PdfPTable modelInfoTableMain = new PdfPTable(2);

                        modelInfoTableMain.WidthPercentage = 100;
                        modelInfoTableMain.HorizontalAlignment = 0;
                        modelInfoTableMain.SpacingBefore = 50f; ;
                        modelInfoTableMain.SpacingAfter = 50f;


                        PdfPTable modelInfoTable = new PdfPTable(1);
                        modelInfoTable.WidthPercentage = 100;
                        modelInfoTable.HorizontalAlignment = Element.ALIGN_CENTER;
                        modelInfoTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                        modelInfoTable.DefaultCell.Padding = 0;
                        modelInfoTable.DefaultCell.UseAscender = false;
                        modelInfoTable.DefaultCell.UseDescender = false;


                        Phrase ShipFromPhrase = new Phrase();
                        Chunk mChunkShipFromName = new Chunk(objOBCtnLblPrnt.ship_from.ToString().Trim() + "\n", mainFont2);
                        ShipFromPhrase.Add(mChunkShipFromName);

                        PdfPCell ShipFromCell = new PdfPCell(ShipFromPhrase);

                        ShipFromCell.BorderWidth = 0;
                        ShipFromCell.PaddingLeft = 0;
                        ShipFromCell.PaddingRight = 0;
                        ShipFromCell.PaddingTop = 2;
                        ShipFromCell.PaddingBottom = 1;
                        ShipFromCell.SetLeading(0f, 1.2f);
                        ShipFromCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        modelInfoTable.AddCell(ShipFromCell);

                        Phrase ShipFromAddr1 = new Phrase();
                        Chunk mChunkShipFromAddr1 = new Chunk(objOBCtnLblPrnt.ship_from_addr1.ToString().Trim() + "\n", infoFont2);
                        ShipFromAddr1.Add(mChunkShipFromAddr1);
                        ShipFromCell = new PdfPCell(ShipFromAddr1);
                        ShipFromCell.PaddingLeft = 0;
                        ShipFromCell.PaddingRight = 0;
                        ShipFromCell.PaddingTop = 2;
                        ShipFromCell.PaddingBottom = 0;
                        ShipFromCell.SetLeading(0f, 1.0f);
                        ShipFromCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        modelInfoTable.AddCell(ShipFromCell);


                        iTextSharp.text.Font lineFont = FontFactory.GetFont("Kalinga", 12, new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8")));

                        Phrase LinePhrase = new Phrase();
                        Chunk lineChunk = new Chunk("______________________________", lineFont);
                        LinePhrase.Add(lineChunk);
                        PdfPCell cellBlankLine = new PdfPCell(LinePhrase);
                        cellBlankLine.PaddingLeft = 0;
                        cellBlankLine.PaddingRight = 0;
                        cellBlankLine.PaddingTop = 2;
                        cellBlankLine.PaddingBottom = 10;
                        cellBlankLine.SetLeading(0f, 1.0f);
                        cellBlankLine.Border = iTextSharp.text.Rectangle.NO_BORDER;

                        modelInfoTable.AddCell(cellBlankLine);


                        //ShipFromPhrase = new Phrase();
                        //Chunk mChunkBlank = new Chunk(string.Empty + "\n", mainFont2);
                        //ShipFromPhrase.Add(mChunkBlank);
                        //modelInfoTable.AddCell(ShipFromPhrase);


                        Phrase ShipToPhrase = new Phrase();
                        Chunk mChunkShiToName = new Chunk(objOBCtnLblPrnt.ship_to.ToString().Trim() + "\n", mainFont2);
                        ShipToPhrase.Add(mChunkShiToName);

                        PdfPCell cellShipTo = new PdfPCell(ShipToPhrase);
                        cellShipTo.PaddingLeft = 0;
                        cellShipTo.PaddingRight = 0;
                        cellShipTo.PaddingTop = 2;
                        cellShipTo.PaddingBottom = 0;
                        cellShipTo.SetLeading(0f, 1.0f);
                        cellShipTo.Border = iTextSharp.text.Rectangle.NO_BORDER;


                        modelInfoTable.AddCell(cellShipTo);

                        // ShipToPhrase.Add(new Chunk(Environment.NewLine));

                        Phrase ShipToAddr1Phrase = new Phrase();
                        Chunk mChunkShipToAddr1 = new Chunk(objOBCtnLblPrnt.ship_to_addr1.ToString().Trim() + "\n", infoFont2);
                        ShipToAddr1Phrase.Add(mChunkShipToAddr1);
                        PdfPCell ShipToAddr1Cell = new PdfPCell(ShipToAddr1Phrase);

                        ShipToAddr1Cell.PaddingLeft = 0;
                        ShipToAddr1Cell.PaddingRight = 0;
                        ShipToAddr1Cell.PaddingTop = 0;
                        ShipToAddr1Cell.PaddingBottom = 5;

                        ShipToAddr1Cell.SetLeading(0f, 1.5f);
                        ShipToAddr1Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        modelInfoTable.AddCell(ShipToAddr1Cell);


                        Phrase ShipToAddr2Phrase = new Phrase();
                        Chunk mChunkShipToAddr2 = new Chunk(objOBCtnLblPrnt.ship_to_addr2.ToString().Trim() + "\n", infoFont2);
                        ShipToAddr2Phrase.Add(mChunkShipToAddr2);
                        PdfPCell ShipToAddr2Cell = new PdfPCell(ShipToAddr2Phrase);

                        ShipToAddr2Cell.PaddingLeft = 0;
                        ShipToAddr2Cell.PaddingRight = 0;
                        ShipToAddr2Cell.PaddingTop = 0;
                        ShipToAddr2Cell.PaddingBottom = 5;


                        // ShipToAddr2Cell.Padding = 0;
                        ShipToAddr2Cell.SetLeading(0f, .7f);
                        ShipToAddr2Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        modelInfoTable.AddCell(ShipToAddr2Cell);


                        Phrase BlankPhrase = new Phrase();
                        Chunk mChunkBlank1 = new Chunk(string.Empty + "\n", mainFont2);
                        BlankPhrase.Add(mChunkBlank1);
                        modelInfoTable.AddCell(BlankPhrase);

                        Phrase POPhrase = new Phrase();
                        Chunk OrdrNumChunk = new Chunk("PO#: " + objOBCtnLblPrnt.cust_ordr_num.ToString().Trim() + "\n", mainFont2);
                        POPhrase.Add(OrdrNumChunk);
                        PdfPCell cellPONum = new PdfPCell(POPhrase);
                        cellPONum.PaddingLeft = 0;
                        cellPONum.PaddingRight = 0;
                        cellPONum.PaddingTop = 0;
                        cellPONum.PaddingBottom = 5;
                        cellPONum.SetLeading(0f, .7f);
                        cellPONum.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        modelInfoTable.AddCell(cellPONum);


                        Phrase ctnPhrase = new Phrase();
                        Chunk mChunkCtn = new Chunk("Ord#:" + objOBCtnLblPrnt.ordr_num.ToString().Trim() + "\t \t \t     " + i.ToString().Trim() + " OF " + l_int_ctns.ToString(), mainFont2);
                        ctnPhrase.Add(mChunkCtn);
                        PdfPCell ctnCell = new PdfPCell(ctnPhrase);
                        ctnCell.PaddingLeft = 0;
                        ctnCell.PaddingRight = 0;
                        ctnCell.PaddingTop = 1;
                        ctnCell.PaddingBottom = 2;
                        ctnCell.SetLeading(0f, 1.2f);

                        //// ctnCell.Padding = 0;

                        ctnCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //  ctnCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        modelInfoTable.AddCell(ctnCell);


                        PdfPCell cellAddress = new PdfPCell(modelInfoTable);
                        cellAddress.Border = 0;
                        cellAddress.Border = iTextSharp.text.Rectangle.NO_BORDER;


                        modelInfoTableMain.AddCell(cellAddress);
                        pdfDoc.Add(modelInfoTableMain);

                        if (i < l_int_ctns)
                        {
                            i = i + 1;


                            PdfPTable modelInfoTable2 = new PdfPTable(1);

                            // modelInfoTable2.TotalWidth = 100;
                            modelInfoTable2.WidthPercentage = 100;
                            modelInfoTable2.HorizontalAlignment = Element.ALIGN_CENTER;
                            modelInfoTable2.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                            modelInfoTable2.DefaultCell.Padding = 0;
                            modelInfoTable2.DefaultCell.UseAscender = false;
                            modelInfoTable2.DefaultCell.UseDescender = false;


                            Phrase ShipFromPhrase2 = new Phrase();
                            Chunk mChunkShipFromName2 = new Chunk(objOBCtnLblPrnt.ship_from.ToString().Trim() + "\n", mainFont2);
                            ShipFromPhrase2.Add(mChunkShipFromName2);

                            PdfPCell ShipFromCell2 = new PdfPCell(ShipFromPhrase2);

                            ShipFromCell2.BorderWidth = 0;
                            ShipFromCell2.PaddingLeft = 25;
                            ShipFromCell2.PaddingRight = 0;
                            ShipFromCell2.PaddingTop = 2;
                            ShipFromCell2.PaddingBottom = 1;
                            ShipFromCell2.SetLeading(0f, 1.2f);
                            ShipFromCell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            modelInfoTable2.AddCell(ShipFromCell2);


                            Phrase PhraseShipFromAddr12 = new Phrase();
                            Chunk mChunkShipFromAddr12 = new Chunk(objOBCtnLblPrnt.ship_from_addr1.ToString().Trim() + "\n", infoFont2);
                            PhraseShipFromAddr12.Add(mChunkShipFromAddr12);
                            PdfPCell ShipFromAddrCell2 = new PdfPCell(PhraseShipFromAddr12);
                            ShipFromAddrCell2.PaddingLeft = 25;
                            ShipFromAddrCell2.PaddingRight = 0;
                            ShipFromAddrCell2.PaddingTop = 2;
                            ShipFromAddrCell2.PaddingBottom = 0;
                            ShipFromAddrCell2.SetLeading(0f, 1.0f);
                            ShipFromAddrCell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            modelInfoTable2.AddCell(ShipFromAddrCell2);




                            Phrase LinePhrase2 = new Phrase();
                            Chunk lineChunk2 = new Chunk("______________________________", lineFont);
                            LinePhrase2.Add(lineChunk2);
                            PdfPCell cellBlankLine2 = new PdfPCell(LinePhrase2);
                            cellBlankLine2.PaddingLeft = 25;
                            cellBlankLine2.PaddingRight = 0;
                            cellBlankLine2.PaddingTop = 2;
                            cellBlankLine2.PaddingBottom = 10;
                            cellBlankLine2.SetLeading(0f, 1.0f);
                            cellBlankLine2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            modelInfoTable2.AddCell(cellBlankLine2);



                            Phrase ShipToPhrase2 = new Phrase();
                            Chunk mChunkShiToName2 = new Chunk(objOBCtnLblPrnt.ship_to.ToString().Trim() + "\n", mainFont2);
                            ShipToPhrase2.Add(mChunkShiToName2);
                            PdfPCell ShipToCell2 = new PdfPCell(ShipToPhrase2);
                            ShipToCell2.PaddingLeft = 25;
                            ShipToCell2.PaddingRight = 0;
                            ShipToCell2.PaddingTop = 2;
                            ShipToCell2.PaddingBottom = 0;

                            ShipToCell2.SetLeading(0f, 1.0f);
                            ShipToCell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            modelInfoTable2.AddCell(ShipToCell2);



                            Phrase ShipToAddr1Phrase2 = new Phrase();
                            Chunk mChunkShipToAddr12 = new Chunk(objOBCtnLblPrnt.ship_to_addr1.ToString().Trim() + "\n", infoFont2);
                            ShipToAddr1Phrase2.Add(mChunkShipToAddr12);
                            PdfPCell ShipToAddr1Cell2 = new PdfPCell(ShipToAddr1Phrase2);

                            ShipToAddr1Cell2.PaddingLeft = 25;
                            ShipToAddr1Cell2.PaddingRight = 0;
                            ShipToAddr1Cell2.PaddingTop = 0;
                            ShipToAddr1Cell2.PaddingBottom = 5;

                            ShipToAddr1Cell2.SetLeading(0f, 1.5f);
                            ShipToAddr1Cell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            modelInfoTable2.AddCell(ShipToAddr1Cell2);


                            Phrase ShipToAddr2Phrase2 = new Phrase();
                            Chunk mChunkShipToAddr22 = new Chunk(objOBCtnLblPrnt.ship_to_addr2.ToString().Trim() + "\n", infoFont2);
                            ShipToAddr2Phrase2.Add(mChunkShipToAddr22);
                            PdfPCell ShipToAddr2Cell2 = new PdfPCell(ShipToAddr2Phrase2);

                            ShipToAddr2Cell2.PaddingLeft = 25;
                            ShipToAddr2Cell2.PaddingRight = 0;
                            ShipToAddr2Cell2.PaddingTop = 0;
                            ShipToAddr2Cell2.PaddingBottom = 5;


                            // ShipToAddr2Cell.Padding = 0;
                            ShipToAddr2Cell2.SetLeading(0f, .7f);
                            ShipToAddr2Cell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            modelInfoTable2.AddCell(ShipToAddr2Cell2);


                            Phrase BlankPhrase2 = new Phrase();
                            Chunk mChunkBlank12 = new Chunk(string.Empty + "\n", mainFont2);
                            BlankPhrase2.Add(mChunkBlank12);
                            modelInfoTable2.AddCell(BlankPhrase2);

                            Phrase POPhrase2 = new Phrase();
                            Chunk OrdrNumChunk2 = new Chunk("PO#: " + objOBCtnLblPrnt.cust_ordr_num.ToString().Trim() + "\n", mainFont2);
                            POPhrase2.Add(OrdrNumChunk2);
                            PdfPCell cellPONum2 = new PdfPCell(POPhrase2);

                            cellPONum2.PaddingLeft = 25;
                            cellPONum2.PaddingRight = 0;
                            cellPONum2.PaddingTop = 0;
                            cellPONum2.PaddingBottom = 5;
                            cellPONum2.SetLeading(0f, .7f);
                            cellPONum2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            modelInfoTable2.AddCell(cellPONum2);





                            Phrase ctnPhrase2 = new Phrase();
                            Chunk mChunkCtn2 = new Chunk("Ord#:" + objOBCtnLblPrnt.ordr_num.ToString().Trim() + "\t \t \t     " + i.ToString().Trim() + " OF " + l_int_ctns.ToString(), mainFont2);
                            ctnPhrase2.Add(mChunkCtn2);
                            PdfPCell ctnCell2 = new PdfPCell(ctnPhrase2);

                            ctnCell2.PaddingLeft = 25;
                            ctnCell2.PaddingRight = 0;
                            ctnCell2.PaddingTop = 1;
                            ctnCell2.PaddingBottom = 5;
                            ctnCell2.SetLeading(0f, 1.2f);

                            //// ctnCell.Padding = 0;

                            ctnCell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //  ctnCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            modelInfoTable2.AddCell(ctnCell2);




                            PdfPCell cellAddress1 = new PdfPCell(modelInfoTable2);
                            cellAddress1.Border = 0;
                            cellAddress1.Border = iTextSharp.text.Rectangle.NO_BORDER;

                            //modelInfoTableMain.WidthPercentage = 100;
                            //modelInfoTableMain.HorizontalAlignment = 0;
                            //modelInfoTableMain.SpacingBefore = 50f;
                            //modelInfoTableMain.SpacingAfter = 50f;

                            modelInfoTableMain.AddCell(cellAddress1);
                            pdfDoc.Add(modelInfoTableMain);

                        }
                        else
                        {
                            PdfPTable modelInfoTable2 = new PdfPTable(1);
                            PdfPCell cellAddress1 = new PdfPCell(modelInfoTable2);
                            cellAddress1.Border = 0;
                            cellAddress1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            modelInfoTableMain.AddCell(cellAddress1);
                            pdfDoc.Add(modelInfoTableMain);

                        }


                        if ((i % 6) == 0)
                        {
                            pdfDoc.NewPage();
                        }
                    }
                    pdfDoc.Close();
                }
            }
            catch (Exception Ex)
            {
                return Json(Ex.InnerException.ToString(), JsonRequestBehavior.AllowGet);

            }
            finally
            {

            }

            return Json(strFileName, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ShowStockVerifyExcel(string p_str_cmpid, string p_str_ShipReq_id, string pstrRptId)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

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
                    if (pstrRptId == "RptStkBySr")
                    {


                        OutboundInq objOutboundInq = new OutboundInq();
                        OutboundInqService ServiceObject = new OutboundInqService();
                        List<OutboundInq> StockverifyLi = new List<OutboundInq>();
                        StockverifyLi = Session["GridStockverifyList"] as List<OutboundInq>;
                        objOutboundInq.LstStockverify = StockverifyLi;
                        List<OB_StockverifyExcel> li = new List<OB_StockverifyExcel>();
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
                            objOBInquiryExcel.PONum = objOutboundInq.LstStockverify[i].po_num;
                            li.Add(objOBInquiryExcel);
                        }
                        GridView gv = new GridView();
                        gv.DataSource = li;
                        gv.DataBind();
                        Session["OB_STKVERIFY"] = gv;
                        return new DownloadFileActionResult((GridView)Session["OB_STKVERIFY"], "SR-" + p_str_ShipReq_id + "-STK-" + strDateFormat + ".xls");
                    }
                    else if (pstrRptId == "RptBackOrdr")
                    {
                        OutboundInq objOutboundInq = new OutboundInq();
                        OBAllocationService oBAllocationService = new OBAllocationService();
                        objOutboundInq.lstOBSRAlocDtl = oBAllocationService.getSRAlocDtl(p_str_cmpid, p_str_ShipReq_id);
                        List<OBStockverifyBOExcel> li = new List<OBStockverifyBOExcel>();

                        foreach (ClsOBSRAlocDtl rec in objOutboundInq.lstOBSRAlocDtl)
                        {
                            if (rec.bo_qty > 0)
                            {
                                OBStockverifyBOExcel objOBInquiryExcel = new OBStockverifyBOExcel();
                                objOBInquiryExcel.WhsId = rec.whs_id;
                                objOBInquiryExcel.SRNumber = rec.sr_num;
                                objOBInquiryExcel.LineNumber = rec.line_num;
                                objOBInquiryExcel.Style = rec.itm_num;
                                objOBInquiryExcel.Color = rec.itm_color;
                                objOBInquiryExcel.Size = rec.itm_size;
                                objOBInquiryExcel.ItemName = rec.itm_name;
                                objOBInquiryExcel.DueQty = rec.due_qty;
                                objOBInquiryExcel.AvailQty = rec.avail_qty;
                                objOBInquiryExcel.CurrentAlocQty = rec.aloc_qty;
                                objOBInquiryExcel.BackOrderQty = rec.bo_qty;
                                objOBInquiryExcel.BalanceQty = rec.bal_qty;
                                li.Add(objOBInquiryExcel);
                            }

                        }

                        GridView gv = new GridView();
                        gv.DataSource = li;
                        gv.DataBind();
                        Session["OB_STKVERIFY"] = gv;
                        return new DownloadFileActionResult((GridView)Session["OB_STKVERIFY"], "SR-BO-" + p_str_ShipReq_id + "-NO-STK-" + strDateFormat + ".xls");

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
            if (p_str_Sonum.Contains(","))
            {
                int ResultCount = 2;
                return Json(ResultCount, JsonRequestBehavior.AllowGet);
            }
            else
            {
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

        }
        public ActionResult ShowConsolidatedReport(string p_str_cmp_id, string p_str_Sonum, string P_str_SoNumFm, string P_str_SoNumTo, string Type)
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
                    if (Type == "Style")
                    {
                        string RptResult = string.Empty;
                        strReportName = "rpt_iv_pickslip_consolidated_by_style.rpt";
                        OutboundInq objOutboundInq = new OutboundInq();
                        OutboundInqService ServiceObject = new OutboundInqService();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmp_id.Trim();
                        objOutboundInq.so_numFm = P_str_SoNumFm.Trim();
                        objOutboundInq.so_numTo = P_str_SoNumTo.Trim();
                        objOutboundInq.quote_num = p_str_Sonum.Trim();
                        batchId = objOutboundInq.quote_num;
                        objOutboundInq = ServiceObject.OutboundInqPickStyleRpt(objOutboundInq);
                        var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
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
                        }
                    }
                    else
                    {
                        strReportName = "rpt_iv_pickslip_consolidated_by_loc.rpt";
                        OutboundInq objOutboundInq = new OutboundInq();
                        OutboundInqService ServiceObject = new OutboundInqService();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundInq.cmp_id = p_str_cmp_id.Trim();
                        objOutboundInq.so_numFm = P_str_SoNumFm.Trim();
                        objOutboundInq.so_numTo = P_str_SoNumTo.Trim();
                        objOutboundInq.quote_num = p_str_Sonum.Trim();
                        batchId = objOutboundInq.quote_num;
                        objOutboundInq = ServiceObject.OutboundInqPickLocationRpt(objOutboundInq);

                        var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
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
        public ActionResult SaveStockBackOrderAuotoAdd(string p_str_cmp_id, string p_str_Sonum)
        {
            string ETADate = string.Empty;
            int tmpDtlLine = 0;
            string tmpLotId = string.Empty;
            string tmpDate = string.Empty;
            string l_str_Refno = string.Empty;
            //   string l_str_sr_pick_ref_no = string.Empty;
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objOutboundInq.cmp_id = p_str_cmp_id.Trim();
            objOutboundInq.Sonum = p_str_Sonum.Trim();
            //  l_str_sr_pick_ref_no = ServiceObject.GetSRPickRefNumber(p_str_cmp_id, p_str_Sonum);

            objOutboundInq = ServiceObject.GetpaletId(objOutboundInq);
            objOutboundInq.palet_id = objOutboundInq.palet_id;
            //objOutboundInq.lbl_id = l_str_sr_pick_ref_no;

            objOutboundInq.lbl_id = p_str_Sonum;
            objOutboundInq = ServiceObject.Get_IbdocId(objOutboundInq);
            if (objOutboundInq.ListRMADocId.Count > 0)
            {
                objOutboundInq.ib_doc_id = objOutboundInq.ListRMADocId[0].ct_value;
            }
            objOutboundInq = ServiceObject.Get_LotId(objOutboundInq);
            if (objOutboundInq.LstLotId.Count > 0)
            {
                objOutboundInq.lot_id = objOutboundInq.LstLotId[0].ct_value;
                tmpLotId = "9999" + objOutboundInq.lot_id;
            }
            objOutboundInq = ServiceObject.Get_StockList(objOutboundInq);
            for (int i = 0; i < objOutboundInq.LstStockverify.Count; i++)
            {
                objOutboundInq.dtl_line = tmpDtlLine + 1;
                objOutboundInq.CtnLn = 1;
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
                objOutboundInq.Note = "B/O Auto Received for SR#" + objOutboundInq.LstStockverify[i].so_num.Trim();
                objOutboundInq.refno = "SR#" + objOutboundInq.LstStockverify[i].so_num.Trim();
                l_str_Refno = objOutboundInq.refno;
                objOutboundInq.ppk = objOutboundInq.LstStockverify[i].back_ordr_qty;
                objOutboundInq.length = objOutboundInq.LstStockverify[i].length;
                objOutboundInq.width = objOutboundInq.LstStockverify[i].width;
                objOutboundInq.depth = objOutboundInq.LstStockverify[i].depth;
                objOutboundInq.wgt = objOutboundInq.LstStockverify[i].wgt;
                objOutboundInq.cube = objOutboundInq.LstStockverify[i].cube;
                objOutboundInq.user_id = Session["UserID"].ToString().Trim();
                objOutboundInq.doc_date = DateTime.Now.ToString("MM/dd/yyyy");
                tmpDate = objOutboundInq.doc_date;
                objOutboundInq.rcvd_notes = "B/O Auto Add For." + objOutboundInq.refno;
                objOutboundInq.io_rate_id = "INOUT-1";
                objOutboundInq.st_rate_id = "STRG-1";
                objOutboundInq.loc_id = "FLOOR";
                if (objOutboundInq.back_ordr_qty > 0)
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
                    if (objOutboundInq.LstLotId.Count > 0)
                    {
                        objOutboundInq.lot_id = objOutboundInq.LstLotId[0].lot_id;
                        tmpLotId = "9999" + objOutboundInq.lot_id;
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
            if (p_str_batchId == "")
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
            DataColumn colponum = new DataColumn("po_num", typeof(string));
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
            dtOutInq.Columns.Add(colponum);

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
                objOutboundInq.po_num = objOutboundInq.LstStkverifyList[i].po_num;//CR20181128
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
                dtOutInq.Rows[lintCount][colponum] = objOutboundInq.po_num;
                OutboundInq objOutboundInqDtltemp = new OutboundInq();
                objOutboundInqDtltemp.cmp_id = objOutboundInq.cmp_id;
                objOutboundInqDtltemp.LineNum = LineNum;
                objOutboundInqDtltemp.itm_code = objOutboundInq.itm_code;
                objOutboundInqDtltemp.Style = objOutboundInq.Style;
                objOutboundInqDtltemp.Color = objOutboundInq.Color;
                objOutboundInqDtltemp.Size = objOutboundInq.Size;
                objOutboundInqDtltemp.whs_id = objOutboundInq.whs_id;
                objOutboundInqDtltemp.po_num = objOutboundInq.po_num;

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
                objOutboundInq = ServiceObject.OutboundGETALOCSORTSTMT(objOutboundInq);
                if (objOutboundInq.LstItmxCustdtl.Count > 0)
                {
                    objOutboundInq.aloc_sort_stmt = objOutboundInq.LstItmxCustdtl[0].aloc_sort_stmt;
                    strAlocSortStmt = objOutboundInq.aloc_sort_stmt;
                    objOutboundInq.aloc_by = objOutboundInq.LstItmxCustdtl[0].aloc_by;
                    strAlocBy = objOutboundInq.aloc_by;
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
            DataColumn colponum = new DataColumn("po_num", typeof(string));

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
            dtOutInq.Columns.Add(colponum);
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
                objOutboundInq.po_num = objOutboundInq.LstStkverifyList[i].po_num;

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
                dtOutInq.Rows[lintCount][colponum] = objOutboundInq.po_num;
                OutboundInq objOutboundInqDtltemp = new OutboundInq();
                objOutboundInqDtltemp.cmp_id = objOutboundInq.cmp_id;
                objOutboundInqDtltemp.LineNum = LineNum;
                objOutboundInqDtltemp.itm_code = objOutboundInq.itm_code;
                objOutboundInqDtltemp.Style = objOutboundInq.Style;
                objOutboundInqDtltemp.Color = objOutboundInq.Color;
                objOutboundInqDtltemp.Size = objOutboundInq.Size;
                objOutboundInqDtltemp.whs_id = objOutboundInq.whs_id;
                objOutboundInqDtltemp.po_num = objOutboundInq.po_num;
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
                objOutboundInq = ServiceObject.OutboundGETALOCSORTSTMT(objOutboundInq);
                if (objOutboundInq.LstItmxCustdtl.Count > 0)
                {
                    objOutboundInq.aloc_sort_stmt = objOutboundInq.LstItmxCustdtl[0].aloc_sort_stmt;
                    strAlocSortStmt = objOutboundInq.aloc_sort_stmt;
                    objOutboundInq.aloc_by = objOutboundInq.LstItmxCustdtl[0].aloc_by;
                    strAlocBy = objOutboundInq.aloc_by;
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

                if (objOutboundInq.LstStockverify[j].back_ordr_qty != 0)
                {
                    //int l_int_BackOrder_count = 0;
                    //l_int_BackOrder_count = objOutboundInq.Back_Order_Qty;
                    Back_Order_Qty = Back_Order_Qty + 1;
                }
                //if (objOutboundInq.avail_qty == 0)//CR-20180626-001 added by nithya
                //{
                //    decimal l_int_avail_qty_count = 0;
                //    l_int_avail_qty_count = objOutboundInq.avail_qty;
                //    Avail_Qty = Avail_Qty + 1;
                //}
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
        public ActionResult BackOrderRptCount(string p_str_cmpid, string p_str_Sonum)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            try
            {
                objOutboundInq.cmp_id = p_str_cmpid;
                objOutboundInq.so_num = p_str_Sonum;
                objOutboundInq = ServiceObject.Get_AlocBackOrderCount(objOutboundInq);
                int alocqty = objOutboundInq.LstOutboundInqpickstyleRpt[0].back_ordr_qty;
                if (alocqty > 0)
                {
                    int ResultCount = 1;
                    return Json(new { data = ResultCount }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    int ResultCount = 0;
                    return Json(new { data = ResultCount }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception Ex)
            {
                msg = Ex.Message;
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
                    OutboundInq objOutboundInq = new OutboundInq();
                    OutboundInqService ServiceObject = new OutboundInqService();

                    strReportName = "rpt_mvc_so_bo.rpt";
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                    objOutboundInq.cmp_id = p_str_cmpid;
                    objOutboundInq.so_num = p_str_Sonum;
                    objOutboundInq = ServiceObject.Get_AlocBackOrderCount(objOutboundInq);
                    int alocqty = objOutboundInq.LstOutboundInqpickstyleRpt[0].back_ordr_qty;
                    if (alocqty > 0)
                    {
                        objOutboundInq = ServiceObject.Get_AlocBackOrderRptList(objOutboundInq);
                        var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "BackOrderReport");
                            }
                        }
                    }
                    else
                    {
                        Response.Write("<H2>Report not found</H2>");
                    }
                }
                else
                {
                    jsonErrorCode = "-1";
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
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
            objOutboundInq.cmp_id = p_str_cmpid.Trim();
            objOutboundInq.CompID = p_str_cmpid.Trim();
            objOutboundInq.quote_num = p_str_batchId.Trim();
            Session["batchId"] = p_str_batchId;
            objOutboundInq.so_numFm = P_str_SoNumFm.Trim();
            objOutboundInq.so_numTo = P_str_SoNumTo.Trim();
            objCustMaster.cust_id = p_str_cmpid;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
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
            DataColumn colpo_num = new DataColumn("po_num", typeof(string));


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
            dtOutInq.Columns.Add(colpo_num);

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
                objOutboundInq.po_num = objOutboundInq.LstStkverifyList[i].po_num;
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
                dtOutInq.Rows[lintCount][colpo_num] = objOutboundInq.po_num.ToString();
                OutboundInq objOutboundInqDtltemp = new OutboundInq();
                objOutboundInqDtltemp.cmp_id = objOutboundInq.cmp_id;
                objOutboundInqDtltemp.LineNum = LineNum;
                objOutboundInqDtltemp.itm_code = objOutboundInq.itm_code;
                objOutboundInqDtltemp.itm_num = objOutboundInq.Style;
                objOutboundInqDtltemp.itm_color = objOutboundInq.Color;
                objOutboundInqDtltemp.itm_size = objOutboundInq.Size;
                objOutboundInqDtltemp.whs_id = objOutboundInq.whs_id;
                objOutboundInqDtltemp.Sonum = objOutboundInq.Sonum;
                objOutboundInqDtltemp.po_num = objOutboundInq.po_num;
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
                objOutboundInq = ServiceObject.OutboundGETALOCSORTSTMT(objOutboundInq);
                if (objOutboundInq.LstItmxCustdtl.Count > 0)
                {
                    objOutboundInq.aloc_sort_stmt = objOutboundInq.LstItmxCustdtl[0].aloc_sort_stmt;
                    strAlocSortStmt = objOutboundInq.aloc_sort_stmt;
                    objOutboundInq.aloc_by = objOutboundInq.LstItmxCustdtl[0].aloc_by;
                    strAlocBy = objOutboundInq.aloc_by;
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
                    if (rptSource.Count > 0)
                    {
                        using (ReportDocument rd = new ReportDocument())
                        {
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.LstStkverifyList.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                            batchId = "Batch Id :" + batchId + "";
                            //if (!string.IsNullOrEmpty(p_str_batchId))//CR20180721
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
        public ActionResult ShowEmailstkReport(string p_str_cmpid, string p_str_Sonum, string p_str_batchId, string P_str_SoNumFm, string P_str_SoNumTo, string type)
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
            Folderpath = System.Configuration.ConfigurationManager.AppSettings["tempFilepath"].ToString().Trim();

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

            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
            objOutboundInq.cmp_id = p_str_cmpid.Trim();
            objOutboundInq.CompID = p_str_cmpid.Trim();
            objOutboundInq.quote_num = p_str_batchId.Trim();
            Session["batchId"] = p_str_batchId;
            objOutboundInq.so_numFm = P_str_SoNumFm.Trim();
            objOutboundInq.so_numTo = P_str_SoNumTo.Trim();
            dtOutInq = new DataTable();
            List<OutboundInq> li = new List<OutboundInq>();
            objCustMaster.cust_id = p_str_cmpid;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }
            objEmail.CmpId = p_str_cmpid;
            objEmail.screenId = ScreenID;
            objEmail.username = objCompany.user_id;
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
            DataColumn colpo_num = new DataColumn("po_num", typeof(string));

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
            dtOutInq.Columns.Add(colpo_num);
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
                objOutboundInq.po_num = objOutboundInq.LstStkverifyList[i].po_num;

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
                dtOutInq.Rows[lintCount][colpo_num] = objOutboundInq.po_num.ToString();
                OutboundInq objOutboundInqDtltemp = new OutboundInq();
                objOutboundInqDtltemp.cmp_id = objOutboundInq.cmp_id;
                objOutboundInqDtltemp.LineNum = LineNum;
                objOutboundInqDtltemp.itm_code = objOutboundInq.itm_code;
                objOutboundInqDtltemp.itm_num = objOutboundInq.Style;
                objOutboundInqDtltemp.itm_color = objOutboundInq.Color;
                objOutboundInqDtltemp.itm_size = objOutboundInq.Size;
                objOutboundInqDtltemp.whs_id = objOutboundInq.whs_id;
                objOutboundInqDtltemp.Sonum = objOutboundInq.Sonum;
                objOutboundInqDtltemp.po_num = objOutboundInq.po_num;

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
                objOutboundInq = ServiceObject.OutboundGETALOCSORTSTMT(objOutboundInq);
                if (objOutboundInq.LstItmxCustdtl.Count > 0)
                {
                    objOutboundInq.aloc_sort_stmt = objOutboundInq.LstItmxCustdtl[0].aloc_sort_stmt;
                    strAlocSortStmt = objOutboundInq.aloc_sort_stmt;
                    objOutboundInq.aloc_by = objOutboundInq.LstItmxCustdtl[0].aloc_by;
                    strAlocBy = objOutboundInq.aloc_by;
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
                objOutboundInq.store_id = (objOutboundInq.LstStkverifyList[0].whs_id == null || objOutboundInq.LstStkverifyList[0].whs_id.Trim() == "" ? string.Empty : objOutboundInq.LstStkverifyList[0].whs_id.Trim());
                objOutboundInq.CustName = (objOutboundInq.LstStkverifyList[0].cust_name == null || objOutboundInq.LstStkverifyList[0].cust_name.Trim() == "" ? string.Empty : objOutboundInq.LstStkverifyList[0].cust_name.Trim());
                if (P_str_SoNumFm == "" || P_str_SoNumTo == "")
                {
                    l_str_rptdtl = objOutboundInq.cmp_id + "_" + "OB STK VERIFICATION" + "_" + objOutboundInq.so_num;
                    objEmail.EmailSubject = objOutboundInq.cmp_id + "-" + " " + " " + "OB STK VERIFICATION" + "|" + " " + " " + "SR#: " + objOutboundInq.so_num + "|" + " " + " " + "Store: " + objOutboundInq.store_id + "|" + " " + " " + "Ref# -Cust Name : " + objOutboundInq.cust_name;
                    objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objOutboundInq.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "SR#: " + " " + " " + objOutboundInq.so_num + "\n" + "StoreID: " + objOutboundInq.store_id + "\n" + "Ref#- " + "\n" + "CustName : " + objOutboundInq.cust_name + "\n" + "Total Cartons Requested: " + objOutboundInq.totalctns + " " + "Ctns" + "\n" + " Total Cube: " + objOutboundInq.totalcube + " " + "Lbs" + "\n" + "Order Qty Requested: " + objOutboundInq.OrderQty + "\n" + " Aloc Qty: " + objOutboundInq.Aloc_Qty + "\n " + "BackOrder Qty: " + objOutboundInq.Back_Order_Qty;
                }
                else
                {
                    l_str_rptdtl = objOutboundInq.cmp_id + "_" + "OB STK VERIFICATION" + "_" + objOutboundInq.so_num;
                    objEmail.EmailSubject = objOutboundInq.cmp_id + "-" + " " + " " + "OB STK VERIFICATION" + "|" + " " + " " + "SR# Frm:" + P_str_SoNumFm + "|" + " " + " " + "SR# To:" + P_str_SoNumTo + "|" + " " + " " + "Store: " + objOutboundInq.store_id + "|" + " " + " " + "Ref# -Cust Name : " + objOutboundInq.cust_name;
                    objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objOutboundInq.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "SR# Frm:" + " " + " " + P_str_SoNumFm + "\n" + "SR# To: " + " " + " " + P_str_SoNumTo + "\n" + "StoreID: " + objOutboundInq.store_id + "\n" + "Ref#- " + "\n" + "CustName : " + objOutboundInq.cust_name + "\n" + "Total Cartons Requested: " + objOutboundInq.totalctns + " " + "Ctns" + "\n" + " Total Cube: " + objOutboundInq.totalcube + " " + "Lbs" + "\n" + "Order Qty Requested: " + objOutboundInq.OrderQty + "\n" + " Aloc Qty: " + objOutboundInq.Aloc_Qty + "\n " + "BackOrder Qty: " + objOutboundInq.Back_Order_Qty;
                }
                try
                {
                    if (isValid)
                    {
                        if (type == "PDF")
                        {
                            var rptSource = objOutboundInq.LstStkverifyList.ToList();

                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;

                                    AlocCount = objOutboundInq.LstStkverifyList.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                                    batchId = "Batch Id :" + batchId + "";
                                    //if (!string.IsNullOrEmpty(p_str_batchId))//CR20170721001 Added By Nithya
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
                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                    reportFileName = l_str_rptdtl + "_" + objOutboundInq.quote_num + strDateFormat + ".pdf";
                                    Session["RptFileName"] = strFileName;
                                }
                            }

                        }
                        //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "StockVerification");
                        else
                        {
                            var rptSource = objOutboundInq.LstStkverifyList.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objOutboundInq.LstStkverifyList.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                                    //if (!string.IsNullOrEmpty(p_str_batchId))
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
                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//Outbound_StockVerification__" + strDateFormat + ".xls";
                                    rd.ExportToDisk(ExportFormatType.ExcelWorkbook, strFileName);
                                    reportFileName = "Stock_Verification_" + strDateFormat + ".xls";//CR2018-03-07-001 Added By Nithya
                                    Session["RptFileName"] = strFileName;
                                }
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
        public ActionResult ShowoutboundReport(string SelectedID, string p_str_cmp_id, string p_str_radio, string type)
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
            objCustMaster.cust_id = p_str_cmp_id;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            if (objCustMaster.ListGetCustLogo.Count == 0)
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


                        objOutboundShipInq.cmp_id = p_str_cmp_id;
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
                        objOutboundShipInq.cmp_id = p_str_cmp_id;
                        objOutboundShipInq.ship_doc_id = SelectedID;
                        objOutboundShipInq = ServiceObject.OutboundShipInqpackSlipRpt(objOutboundShipInq);
                        //objOutboundShipInq = ServiceObject.GetTotCubesRpt(objOutboundShipInq);
                        //if (objOutboundShipInq.LstPalletCount.Count > 0)
                        //{
                        //    objOutboundShipInq.TotCube = objOutboundShipInq.LstPalletCount[0].TotCube;
                        //}
                        //else
                        //{
                        //    objOutboundShipInq.TotCube = 0;
                        //}
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
                                    //rd.SetParameterValue("SumOfCubes", objOutboundShipInq.TotCube);
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
        public ActionResult VAsShowReport(string p_str_cmp_id, string p_str_radio, string p_str_vas_id_fm, string p_str_vas_id_to, string p_str_vas_date_fm, string p_str_vas_date_to, string p_str_so_num, string p_str_Status, string SelectedID, string type)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            objCustMaster.cust_id = p_str_cmp_id;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null || objCustMaster.ListGetCustLogo[0].cust_logo == "")
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }
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
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//VasInquiry//" + strReportName;
                        objVasInquiry.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
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
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objVasInquiry.ListVasInquiry.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objVasInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objVasInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Vas Grid Summary");
                                }
                            }
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
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//VasInquiry//" + strReportName;
                        objVasInquiry.cmp_id = p_str_cmp_id;
                        objVasInquiry.ship_doc_id = SelectedID;
                        objVasInquiry = ServiceObject.GetVasPostDetails(objVasInquiry);

                        if (type == "PDF")
                        {
                            var rptSource = objVasInquiry.ListVasInquiry.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objVasInquiry.ListVasInquiry.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objVasInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim();
                                    rd.SetParameterValue("fml_image_path", objVasInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Vas Post Report");
                                }
                            }
                        }

                        else if (type == "Excel")
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
        public ActionResult GenerateShowReport(string p_str_cmp_id, string p_str_bill_doc_id, string p_str_bill_doc_type, string p_str_rpt_status)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            string l_str_status = string.Empty;
            string l_str_rpt_bill_type = string.Empty;
            string l_str_rpt_bill_inout_type = string.Empty;
            string l_str_rpt_instrg_req = string.Empty;
            string l_str_rpt_bill_doc_type = string.Empty;
            string l_str_rpt_bill_status = string.Empty;
            try
            {
                l_str_rpt_selection = p_str_rpt_status;
                l_str_rpt_bill_doc_type = p_str_bill_doc_type.Trim();
                BillingInquiry objBillingInquiry = new BillingInquiry();
                BillingInquiryService ServiceObject = new BillingInquiryService();
                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                objBillingInquiry = ServiceObject.GetBillingBillingType(objBillingInquiry);
                l_str_rpt_bill_type = objBillingInquiry.ListBillingType[0].bill_type;
                objBillingInquiry = ServiceObject.GetBillingInoutType(objBillingInquiry);
                l_str_rpt_bill_inout_type = objBillingInquiry.ListBillingInoutType[0].bill_inout_type;
                l_str_rpt_instrg_req = objBillingInquiry.ListBillingType[0].init_strg_rt_req;

                if (l_str_rpt_bill_doc_type == "STRG")
                {
                    if (l_str_rpt_bill_type.Trim() == "Carton")

                    {
                        if (l_str_rpt_instrg_req.Trim() == "Y")
                        {
                            strReportName = "rpt_st_bill_doc.rpt";
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                            objBillingInquiry = ServiceObject.GetBillingBillDocSTRGRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);


                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }
                        else
                        {
                            strReportName = "rpt_st_bill_doc.rpt";
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                            objBillingInquiry = ServiceObject.GetBillingBillDocSTRGRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }

                    }
                    if (l_str_rpt_bill_type.Trim() == "Cube")

                    {
                        if (l_str_rpt_instrg_req.Trim() == "Y")
                        {
                            strReportName = "rpt_st_bill_doc_bycube.rpt";
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                            objBillingInquiry = ServiceObject.GetBillingBillDocCubeSTRGRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }
                        else
                        {
                            strReportName = "rpt_st_bill_doc_bycube.rpt";
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                            objBillingInquiry = ServiceObject.GetBillingBillDocCubeSTRGRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }

                    }
                    if (l_str_rpt_bill_type.Trim() == "Pallet")

                    {
                        strReportName = "rpt_strg_bill_by_pallet.rpt";
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                        objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();

                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        objBillingInquiry = ServiceObject.GetBillingBillDocPalletSTRGRpt(objBillingInquiry);
                        var rptSource = objBillingInquiry.ListGenBillingStrgByPalletRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objBillingInquiry.ListGenBillingStrgByPalletRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                objBillingInquiry.itm_num = "STRG-2";
                                objBillingInquiry = ServiceObject.GetSecondSTRGRate(objBillingInquiry);
                                if (objBillingInquiry.sec_strg_rate == "1")
                                {
                                    rd.SetParameterValue("fml_rep_itm_num", objBillingInquiry.ListGetSecondSTRGRate[0].itm_num);
                                    rd.SetParameterValue("fml_rep_list_price", objBillingInquiry.ListGetSecondSTRGRate[0].list_price);
                                    rd.SetParameterValue("fml_rep_price_uom", objBillingInquiry.ListGetSecondSTRGRate[0].price_uom);
                                }
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            }
                        }


                    }
                    if (l_str_rpt_bill_type.Trim() == "Location")

                    {
                        strReportName = "rpt_st_bill_doc_location.rpt";
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                        objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                        objBillingInquiry.bill_doc_id = p_str_bill_doc_id.Trim();
                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        objBillingInquiry = ServiceObject.STRGBillLocationRpt(objBillingInquiry);
                        var rptSource = objBillingInquiry.ListGetSTRGBillByLocRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objBillingInquiry.ListGetSTRGBillByLocRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            }
                        }


                    }
                    if (l_str_rpt_bill_type.Trim() == "Pcs")

                    {
                        strReportName = "rpt_st_bill_doc_Pcs_item.rpt";

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                        objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                        objBillingInquiry.bill_doc_id = p_str_bill_doc_id.Trim();
                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        objBillingInquiry = ServiceObject.STRGBillPcsRpt(objBillingInquiry);
                        var rptSource = objBillingInquiry.ListGetSTRGBillByPcsRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objBillingInquiry.ListGetSTRGBillByPcsRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            }
                        }


                    }
                }
                if (l_str_rpt_bill_doc_type == "INOUT")
                {

                    if (l_str_rpt_bill_inout_type.Trim() == "Carton")

                    {
                        if (l_str_rpt_instrg_req.Trim() == "Y")
                        {
                            strReportName = "rpt_inout_bill_doc_with_initstrg.rpt";

                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                            objBillingInquiry = ServiceObject.GetBillingBillInoutCartonInstrgRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListBillingInoutCartonInstrgRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objBillingInquiry.ListBillingInoutCartonInstrgRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }
                        else
                        {
                            strReportName = "rpt_inout_bill_doc.rpt";
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                            objBillingInquiry = ServiceObject.GetBillingBillInoutCartonRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListBillingInoutCartonRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objBillingInquiry.ListBillingInoutCartonRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                    rd.SetParameterValue("InoutType", l_str_rpt_bill_inout_type.ToUpper());//CR20180809
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }

                    }
                    if (l_str_rpt_bill_inout_type.Trim() == "Cube")

                    {
                        if (l_str_rpt_instrg_req.Trim() == "Y")
                        {
                            strReportName = "rpt_inout_bill_doc_bycube_with_initstrg.rpt";
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                            objBillingInquiry = ServiceObject.GetBillingBillInoutCubeInstrgRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListBillingInoutCubeInstrgRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objBillingInquiry.ListBillingInoutCubeInstrgRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }
                        else
                        {
                            strReportName = "rpt_inout_bill_doc_bycube.rpt";
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                            objBillingInquiry = ServiceObject.GetBillingBillInoutCubeRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListBillingInoutCubeRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objBillingInquiry.ListBillingInoutCubeRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }

                    }
                    if (l_str_rpt_bill_inout_type.Trim() == "Container")

                    {
                        strReportName = "rpt_inout_bill_by_Container.rpt";
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                        objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                        objBillingInquiry = ServiceObject.GetBillingBillDocContainerInoutRpt(objBillingInquiry);
                        var rptSource = objBillingInquiry.ListGenBillingInoutByContainerRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objBillingInquiry.ListGenBillingInoutByContainerRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            }
                        }


                    }

                }
                if (l_str_rpt_bill_doc_type == "NORM")
                {
                    Company objCompany = new Company();
                    CompanyService ServiceObjectCompany = new CompanyService();
                    objCompany.cust_of_cmp_id = "";
                    objCompany.cmp_id = objBillingInquiry.cmp_id;
                    objCompany = ServiceObjectCompany.GetCustOfCompName(objCompany);
                    objBillingInquiry.LstCustOfCmpName = objCompany.LstCustOfCmpName;
                    if (objBillingInquiry.LstCustOfCmpName.Count() == 0)
                    {
                        objBillingInquiry.cust_of_cmpid = string.Empty;
                    }
                    else
                    {
                        objBillingInquiry.cust_of_cmpid = objBillingInquiry.LstCustOfCmpName[0].cust_of_cmpid;
                    }
                    string l_str_cmp_id = string.Empty;
                    l_str_cmp_id = objBillingInquiry.cust_of_cmpid.Trim();
                    if (l_str_cmp_id == "FHNJ")
                    {
                        if (objCompany.cmp_id.Trim() == "SJOE")
                        {


                            strReportName = "rpt_va_bill_doc_FH_NJ.rpt";
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0421_001 
                            objBillingInquiry = ServiceObject.GetBillingBillDocVASRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListBillingDocVASRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objBillingInquiry.ListBillingDocVASRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }
                        else
                        {
                            strReportName = "rpt_va_bill_doc.rpt";
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0421_001 
                            objBillingInquiry = ServiceObject.GetBillingBillDocVASRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListBillingDocVASRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objBillingInquiry.ListBillingDocVASRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);

                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }


                        }
                    }
                    else
                    {
                        strReportName = "rpt_va_bill_doc.rpt";
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                        objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0421_001 
                        objBillingInquiry = ServiceObject.GetBillingBillDocVASRpt(objBillingInquiry);
                        var rptSource = objBillingInquiry.ListBillingDocVASRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objBillingInquiry.ListBillingDocVASRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);

                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                                rd.SetParameterValue("fml_rpt_bill_title", "(VAS BILL)");
                                rd.SetParameterValue("fml_rpt_param_bill_num", "Bill#");
                                rd.SetParameterValue("fml_rpt_param_bill_date", "Bill Dt");
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            }
                        }

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
        public ActionResult uploaddoc(string cmpid, string path)

        {

            string docPath = path;
            string paths = path;
            DirectoryInfo dir = new System.IO.DirectoryInfo(docPath);
            return Json(paths, JsonRequestBehavior.AllowGet);

        }
        //CR-20180719-001 Added By Nithya
        public ActionResult GetDocumentUploadCancel(string CompID, string ibdocid, string comment)
        {
            string name = string.Empty;
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            objInboundInquiry.cmp_id = CompID;
            objInboundInquiry.doctype = "OUTBOUND";
            objInboundInquiry.Uploadby = Session["UserID"].ToString().Trim();
            objInboundInquiry.Comments = comment;
            string path = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
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
            DirectoryInfo dir = new DirectoryInfo(directoryPath);
            int count = 0;
            count = dir.GetFiles().Length;
            foreach (FileInfo flInfo in dir.GetFiles())
            {
                if (Directory.Exists(directoryPath))
                {
                    Directory.Delete(directoryPath, true);
                }
            }

            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryDocModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_InboundInquiryImportFile", InboundInquiryDocModel);
        }
        public ActionResult Uploaddelete(string cmp_id, string Filename, string Filepath, string uplddt, string ibdocid)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            objInboundInquiry.cmp_id = cmp_id;
            objInboundInquiry.file_name = Filename;
            objInboundInquiry.file_path = Filepath;
            objInboundInquiry.docUploaddt = uplddt;
            objInboundInquiry.ibdocid = ibdocid;
            objInboundInquiry = objService.Getuploaddelete(objInboundInquiry);
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
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(Path.Combine(directoryPath));
            }
            directoryPath = Path.Combine(directoryPath, Filename);
            System.IO.File.Delete(directoryPath);
            DirectoryInfo dir = new DirectoryInfo(directoryPath);

            objInboundInquiry = objService.GetTempFiledtl(objInboundInquiry);
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_DocumentUploadGrid", InboundInquiryModel);
        }
        public FileStreamResult DocumentUpload(string path, string ext, string filename)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            if (ext == "pdf" || ext == "PDF")
            {
                return File(fs, "application/pdf");
            }
            if (ext == "doc" || ext == "DOC")
            {
                return File(fs, "application/doc");
            }
            if (ext == "xls" || ext == "XLS")
            {
                return File(fs, "application/xls");
            }
            if (ext == "xlsx" || ext == "XLSX")
            {
                return File(fs, "application/xlsx");
            }

            return File(fs, "application/csv");
        }

        public ActionResult CountryChange(string countryid)
        {
            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            OutboundInq objOutboundInq = new OutboundInq();
            objPick.Cntry_Id = countryid.Trim();
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objOutboundInq.ListStatePick = objPick.ListStatePick;
            var serializer = new JavaScriptSerializer() { MaxJsonLength = 86753090 };
            serializer.Serialize(objOutboundInq);
            return new JsonResult()
            {
                Data = objOutboundInq,
                MaxJsonLength = 86753090
            };

        }
        public ActionResult LoadStyleList(string cmp_id, string itm_num, string itm_color, string itm_size, string act)
        {
            OutboundInqService ServiceObject = new OutboundInqService();
            OutboundInq objOutboundInq = new OutboundInq();
            objOutboundInq.cmp_id = cmp_id;
            objOutboundInq.itm_num = itm_num;
            objOutboundInq.itm_color = itm_color;
            objOutboundInq.itm_size = itm_size;
            objOutboundInq = ServiceObject.GetStyleDetails(objOutboundInq);
            objOutboundInq = ServiceObject.Getitmlist(objOutboundInq);
            if (objOutboundInq.LstItmxCustdtl.Count > 0)
            {
                objOutboundInq.length = objOutboundInq.LstItmxCustdtl[0].length;
                objOutboundInq.width = objOutboundInq.LstItmxCustdtl[0].width;
                objOutboundInq.height = objOutboundInq.LstItmxCustdtl[0].depth;
                objOutboundInq.cube = objOutboundInq.LstItmxCustdtl[0].cube;
                objOutboundInq.weight = objOutboundInq.LstItmxCustdtl[0].wgt;
                objOutboundInq.ppk = objOutboundInq.LstItmxCustdtl[0].ctn_qty;
            }
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            if (act == "Main")
            {
                return PartialView("_LoadStyleList", objOutboundInqModel);
            }
            else
            {
                return PartialView("_LoadStyleListGrid", objOutboundInqModel);
            }

        }

        public ActionResult GetOBDocExcpList(string p_str_cmp_id)
        {
            OutboundInqService ServiceObject = new OutboundInqService();
            OBDocExcp objOBDocExcp = new OBDocExcp();
            objOBDocExcp = ServiceObject.GetOBDocExcpList(objOBDocExcp, p_str_cmp_id);
            GridView gv = new GridView();
            gv.DataSource = objOBDocExcp.ListOBDocExcp;
            gv.DataBind();
            Session["ob_doc_excp_list"] = gv;
            return new DownloadFileActionResult((GridView)Session["ob_doc_excp_list"], p_str_cmp_id + "-OB-DOC-EXCP-" + DateTime.Now.ToString("yyyyMMddHHssmm") + ".xls");

        }


    }


}
