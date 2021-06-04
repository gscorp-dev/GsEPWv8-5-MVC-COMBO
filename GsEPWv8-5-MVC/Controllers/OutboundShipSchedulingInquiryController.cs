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
    public class OutboundShipSchedulingInquiryController : Controller
    {
        OutboundShipSchedulingInquiry objOutboundShipSchedulingInq = new OutboundShipSchedulingInquiry();
        OutboundShipSchedulingInquiryService ServiceObject = new OutboundShipSchedulingInquiryService();
        Company objCompany = new Company();
        CompanyService ServiceObjectCompany = new CompanyService();
        CustMaster objCustMaster = new CustMaster();
        ICustMasterService objCustMasterService = new CustMasterService();
        public bool l_str_include_entry_dtls;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OutboundShipSchedulingInquiry(string FullFillType, string cmp, string status, string DateFm, string DateTo, string screentitle)
        {
            string l_str_cmp_id = string.Empty;
            string l_str_fm_dt = string.Empty;
            string l_str_Dflt_Dt_Reqd = string.Empty;
            try
            {

                objOutboundShipSchedulingInq.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                objOutboundShipSchedulingInq.is_company_user = Session["IsCompanyUser"].ToString().Trim(); //CR_3PL_MVC _2018_0316_005
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                if (objOutboundShipSchedulingInq.cmp_id == null || objOutboundShipSchedulingInq.cmp_id == string.Empty)
                {
                    objOutboundShipSchedulingInq.cmp_id = Session["g_str_cmp_id"].ToString().Trim();

                }
                else
                {
                    objCompany.cmp_id = Session["g_str_cmp_id"].ToString().Trim();

                }
                objOutboundShipSchedulingInq.is_company_user = Session["IsCompanyUser"].ToString().Trim();
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
                    objOutboundShipSchedulingInq.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    DateTime date = DateTime.Now.AddMonths(-12);
                    l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");      // CR_3PL_MVC_OB_2018_0227_001 - Modified by Soniya
                    objOutboundShipSchedulingInq.Ship_dt_Fm = DateTime.Now.AddDays(Common.clsGlobal.DispDateFrom).ToString("MM/dd/yyyy");
                    objOutboundShipSchedulingInq.Ship_dt_To = DateTime.Now.ToString("MM/dd/yyyy");

                }
                else if (FullFillType != null)
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objOutboundShipSchedulingInq.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    objOutboundShipSchedulingInq.cmp_id = cmp;
                    objOutboundShipSchedulingInq.ship_doc_id_Fm = "";
                    objOutboundShipSchedulingInq.ship_doc_id_To = "";
                    objOutboundShipSchedulingInq.Ship_dt_Fm = DateTime.Now.AddDays(Common.clsGlobal.DispDateFrom).ToString("MM/dd/yyyy");
                    objOutboundShipSchedulingInq.Ship_dt_To = DateTime.Now.ToString("MM/dd/yyyy") ;
                    objOutboundShipSchedulingInq.cust_id = "";
                    objOutboundShipSchedulingInq.aloc_doc_id = "";
                    objOutboundShipSchedulingInq.ship_to = "";
                    objOutboundShipSchedulingInq.ship_via_name = "";
                    objOutboundShipSchedulingInq.whs_id = "";
                    status.Trim();
                    if (status == "POST")
                    {
                        objOutboundShipSchedulingInq.status = "";
                    }
                    else
                    {
                        objOutboundShipSchedulingInq.status = "";
                    }
                    if (l_str_Dflt_Dt_Reqd == "Y")
                    {
                        objOutboundShipSchedulingInq = ServiceObject.GetOutboundShipScheduleInq(objOutboundShipSchedulingInq);
                    }
                    objCompany.cmp_id = cmp;
                    objCompany = ServiceObjectCompany.GetFullFillCompanyDetails(objCompany);
                    //objOutboundShipSchedulingInq.ListCompanyPickDtl = objCompany.ListFullFillCompanyPickDtl;
                    status.Trim();
                    if (status == "POST")
                    {
                        objOutboundShipSchedulingInq.status = "POST";
                    }
                    else
                    {
                        objOutboundShipSchedulingInq.status = "";
                    }
                }
                Mapper.CreateMap<OutboundShipSchedulingInquiry, OutboundShipSchedulingInquiryModel>();
                OutboundShipSchedulingInquiryModel objOutboundShipSchedulingInqModel = Mapper.Map<OutboundShipSchedulingInquiry, OutboundShipSchedulingInquiryModel>(objOutboundShipSchedulingInq);
                return View(objOutboundShipSchedulingInqModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ActionResult GetOutboundShipSchedulingInqDetail(string p_str_cmp_id, string p_str_ship_dt_frm, string p_str_ship_dt_to, string p_str_AlocId)
        {
            try
            {
                objOutboundShipSchedulingInq.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                Session["g_str_Search_flag"] = "True";
                objOutboundShipSchedulingInq.cmp_id = p_str_cmp_id.Trim();
                objOutboundShipSchedulingInq.ship_doc_id_Fm = string.Empty;
                objOutboundShipSchedulingInq.ship_doc_id_To = string.Empty;
                objOutboundShipSchedulingInq.Ship_dt_Fm = p_str_ship_dt_frm.Trim();
                objOutboundShipSchedulingInq.Ship_dt_To = p_str_ship_dt_to.Trim();
                objOutboundShipSchedulingInq.cust_id = string.Empty;
                objOutboundShipSchedulingInq.aloc_doc_id = p_str_AlocId.Trim();
                objOutboundShipSchedulingInq.ship_to = string.Empty;
                objOutboundShipSchedulingInq.ship_via_name = string.Empty;
                objOutboundShipSchedulingInq.whs_id = string.Empty;
                objOutboundShipSchedulingInq.status = "POST";
                objOutboundShipSchedulingInq = ServiceObject.GetOutboundShipScheduleInq(objOutboundShipSchedulingInq);
                Mapper.CreateMap<OutboundShipSchedulingInquiry, OutboundShipSchedulingInquiryModel>();
                OutboundShipSchedulingInquiryModel objOutboundShipSchedulingInqModel = Mapper.Map<OutboundShipSchedulingInquiry, OutboundShipSchedulingInquiryModel>(objOutboundShipSchedulingInq);
                return PartialView("_OutboundShipSchedulingInquiry", objOutboundShipSchedulingInqModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public ActionResult NewShipScheduleEntry(string cmp_id, string aloc_doc_id, string status)
        {
            objOutboundShipSchedulingInq.cmp_id = cmp_id;
            objOutboundShipSchedulingInq.aloc_doc_id = aloc_doc_id;
            objOutboundShipSchedulingInq.status = status;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objOutboundShipSchedulingInq.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objOutboundShipSchedulingInq.cmp_id = Session["UserID"].ToString().Trim();
            objOutboundShipSchedulingInq = ServiceObject.GetShipSchedulelEntityValue(objOutboundShipSchedulingInq);
            objOutboundShipSchedulingInq.ship_schdl_doc_id = objOutboundShipSchedulingInq.Lstshipschdldocid[0].ship_schdl_doc_id;
            Mapper.CreateMap<OutboundShipSchedulingInquiry, OutboundShipSchedulingInquiryModel>();
            OutboundShipSchedulingInquiryModel objOutboundShipSchedulingInqModel = Mapper.Map<OutboundShipSchedulingInquiry, OutboundShipSchedulingInquiryModel>(objOutboundShipSchedulingInq);
            return PartialView("_ShipSchedulingAdd", objOutboundShipSchedulingInqModel);

        }

        public ActionResult EditShipScheduleEntry(string cmp_id, string aloc_doc_id, string status, string ship_schdl_doc_id, string ship_car_name, string truck_id, string ship_schdl_date, string ship_notes, string state_id)
        {
            objOutboundShipSchedulingInq.tmp_cmp_id = cmp_id;
            objOutboundShipSchedulingInq.aloc_doc_id = aloc_doc_id;
            objOutboundShipSchedulingInq.status = status;
            objOutboundShipSchedulingInq.ship_schdl_doc_id = ship_schdl_doc_id;
            objOutboundShipSchedulingInq.ship_schdl_date = ship_schdl_date;
            objOutboundShipSchedulingInq.ship_via_name = ship_car_name;
            objOutboundShipSchedulingInq.truck_id = truck_id;
            objOutboundShipSchedulingInq.notes = ship_notes;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objOutboundShipSchedulingInq.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objOutboundShipSchedulingInq.cmp_id = Session["UserID"].ToString().Trim();
            objOutboundShipSchedulingInq.state_id = state_id;
            Mapper.CreateMap<OutboundShipSchedulingInquiry, OutboundShipSchedulingInquiryModel>();
            OutboundShipSchedulingInquiryModel objOutboundShipSchedulingInqModel = Mapper.Map<OutboundShipSchedulingInquiry, OutboundShipSchedulingInquiryModel>(objOutboundShipSchedulingInq);
            return PartialView("_ShipSchedulingEdit", objOutboundShipSchedulingInqModel);

        }
        public ActionResult DeleteShipScheduleEntry(string cmp_id, string aloc_doc_id, string status, string ship_schdl_doc_id, string ship_car_name, string truck_id)
        {
            objOutboundShipSchedulingInq.cmp_id = cmp_id;
            objOutboundShipSchedulingInq.aloc_doc_id = aloc_doc_id;
            objOutboundShipSchedulingInq.status = status;
            objOutboundShipSchedulingInq.ship_schdl_doc_id = ship_schdl_doc_id;
            objOutboundShipSchedulingInq.ship_car_name = ship_car_name;
            objOutboundShipSchedulingInq.truck_id = truck_id;
            Mapper.CreateMap<OutboundShipSchedulingInquiry, OutboundShipSchedulingInquiryModel>();
            OutboundShipSchedulingInquiryModel objOutboundShipSchedulingInqModel = Mapper.Map<OutboundShipSchedulingInquiry, OutboundShipSchedulingInquiryModel>(objOutboundShipSchedulingInq);
            return PartialView("_ShipSchedulingDelete", objOutboundShipSchedulingInqModel);


        }

        public ActionResult AddShipScheduleEntryDetails(string cmp_id, string ship_schdl_doc_id, string shipscheduledate, string shipcarname, string alocid, string truckid, string note, string status, string actiontype)
        {

            objOutboundShipSchedulingInq.cmp_id = cmp_id;
            objOutboundShipSchedulingInq.shipscheduleddate = shipscheduledate;
            objOutboundShipSchedulingInq.shipcarname = shipcarname;
            objOutboundShipSchedulingInq.aloc_doc_id = alocid;
            objOutboundShipSchedulingInq.shipcarid = truckid;
            objOutboundShipSchedulingInq.notes = note;
            objOutboundShipSchedulingInq.status = status;
            objOutboundShipSchedulingInq.ship_schdl_doc_id = ship_schdl_doc_id;
            objOutboundShipSchedulingInq.actiontype = actiontype;
            objOutboundShipSchedulingInq = ServiceObject.SaveShipScheduleDetails(objOutboundShipSchedulingInq);
            objOutboundShipSchedulingInq.PROCESS_ID = objOutboundShipSchedulingInq.LstSaveShipSchdlDetail[0].ship_schdl_doc_id;
            if (objOutboundShipSchedulingInq.PROCESS_ID == objOutboundShipSchedulingInq.ship_schdl_doc_id)
            {
                l_str_include_entry_dtls = true;
            }
            else
            {
                l_str_include_entry_dtls = false;
            }
            Mapper.CreateMap<OutboundShipSchedulingInquiry, OutboundShipSchedulingInquiryModel>();
            OutboundShipSchedulingInquiryModel objOutboundShipSchedulingInqModel = Mapper.Map<OutboundShipSchedulingInquiry, OutboundShipSchedulingInquiryModel>(objOutboundShipSchedulingInq);
            return Json(l_str_include_entry_dtls, JsonRequestBehavior.AllowGet);

        }
        public ActionResult EditShipScheduleEntryDetails(string cmp_id, string ship_schdl_doc_id, string ship_schdl_date, string shipcarname, string alocid, string truckid, string note, string status, string actiontype)

        {

            objOutboundShipSchedulingInq.cmp_id = cmp_id;
            objOutboundShipSchedulingInq.shipscheduleddate = ship_schdl_date;
            objOutboundShipSchedulingInq.shipcarname = shipcarname;
            objOutboundShipSchedulingInq.aloc_doc_id = alocid;
            objOutboundShipSchedulingInq.shipcarid = truckid;
            objOutboundShipSchedulingInq.notes = note;
            objOutboundShipSchedulingInq.status = status;
            objOutboundShipSchedulingInq.ship_schdl_doc_id = ship_schdl_doc_id;
            objOutboundShipSchedulingInq.shipcarname = shipcarname;
            objOutboundShipSchedulingInq.shipcarid = truckid;
            objOutboundShipSchedulingInq.notes = note;
            objOutboundShipSchedulingInq.actiontype = actiontype;
            objOutboundShipSchedulingInq = ServiceObject.SaveShipScheduleDetails(objOutboundShipSchedulingInq);
            if (objOutboundShipSchedulingInq.actiontype == "Edit")
            {
                objOutboundShipSchedulingInq.PROCESS_ID = objOutboundShipSchedulingInq.LstSaveShipSchdlDetail[0].ship_schdl_doc_id;
                if (objOutboundShipSchedulingInq.PROCESS_ID == objOutboundShipSchedulingInq.ship_schdl_doc_id)
                {
                    l_str_include_entry_dtls = true;
                }
                else
                {
                    l_str_include_entry_dtls = false;
                }
            }
            else
            {
                l_str_include_entry_dtls = true;
            }
            Mapper.CreateMap<OutboundShipSchedulingInquiry, OutboundShipSchedulingInquiryModel>();
            OutboundShipSchedulingInquiryModel objOutboundShipSchedulingInqModel = Mapper.Map<OutboundShipSchedulingInquiry, OutboundShipSchedulingInquiryModel>(objOutboundShipSchedulingInq);
            return Json(l_str_include_entry_dtls, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ShowReport(string SelectedID, string p_str_cmp_id, string p_str_radio, string type)
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
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }
            l_str_rpt_selection = p_str_radio;


            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "Outboundpackslip")
                    {

                        strReportName = "rpt_iv_packslip_tpw.rpt";
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                        objOutboundShipSchedulingInq.cmp_id = p_str_cmp_id;
                        objOutboundShipSchedulingInq.ship_doc_id = SelectedID;
                        objOutboundShipSchedulingInq = ServiceObject.OutboundShipInqpackSlipRpt(objOutboundShipSchedulingInq);
                        objOutboundShipSchedulingInq = ServiceObject.GetTotCubesRpt(objOutboundShipSchedulingInq);
                        if (objOutboundShipSchedulingInq.LstPalletCount.Count > 0)
                        {
                            objOutboundShipSchedulingInq.TotCube = objOutboundShipSchedulingInq.LstPalletCount[0].TotCube;
                        }
                        else
                        {
                            objOutboundShipSchedulingInq.TotCube = 0;
                        }
                        if (type == "PDF")
                        {
                            var rptSource = objOutboundShipSchedulingInq.LstOutboundShipInqpackingSlipRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundShipSchedulingInq.LstOutboundShipInqpackingSlipRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundShipSchedulingInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOutboundShipSchedulingInq.Image_Path);
                            rd.SetParameterValue("SumOfCubes", objOutboundShipSchedulingInq.TotCube);
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
                            for (int i = 0; i < objOutboundShipSchedulingInq.LstOutboundShipInqpackingSlipRpt.Count; i++)
                            {

                                OB_SHIP_PACKING_SLIPExcel objOBInquiryExcel = new OB_SHIP_PACKING_SLIPExcel();
                                objOBInquiryExcel.ShipDocID = objOutboundShipSchedulingInq.LstOutboundShipInqpackingSlipRpt[i].ship_doc_id;
                               // objOBInquiryExcel.ShipDocDate = objOutboundShipSchedulingInq.LstOutboundShipInqpackingSlipRpt[i].ship_dt;
                                objOBInquiryExcel.SRNo = objOutboundShipSchedulingInq.LstOutboundShipInqpackingSlipRpt[i].so_num;
                                objOBInquiryExcel.CustPO = objOutboundShipSchedulingInq.LstOutboundShipInqpackingSlipRpt[i].cust_ord;
                                objOBInquiryExcel.WhsId = objOutboundShipSchedulingInq.LstOutboundShipInqpackingSlipRpt[i].whs_id;
                                objOBInquiryExcel.CancelDt = objOutboundShipSchedulingInq.LstOutboundShipInqpackingSlipRpt[i].cancel_dt;
                                objOBInquiryExcel.StoreId = objOutboundShipSchedulingInq.LstOutboundShipInqpackingSlipRpt[i].cust_store;
                                objOBInquiryExcel.DeptID = objOutboundShipSchedulingInq.LstOutboundShipInqpackingSlipRpt[i].cust_dept;
                                objOBInquiryExcel.LineNo = objOutboundShipSchedulingInq.LstOutboundShipInqpackingSlipRpt[i].line_num;
                                objOBInquiryExcel.Style = objOutboundShipSchedulingInq.LstOutboundShipInqpackingSlipRpt[i].itm_num;
                                objOBInquiryExcel.Color = objOutboundShipSchedulingInq.LstOutboundShipInqpackingSlipRpt[i].itm_color;
                                objOBInquiryExcel.Size = objOutboundShipSchedulingInq.LstOutboundShipInqpackingSlipRpt[i].itm_size;
                              //  objOBInquiryExcel.Description = objOutboundShipSchedulingInq.LstOutboundShipInqpackingSlipRpt[i].itm_name;
                                objOBInquiryExcel.Ppk = objOutboundShipSchedulingInq.LstOutboundShipInqpackingSlipRpt[i].ctn_cnt;
                                objOBInquiryExcel.ItemQty = objOutboundShipSchedulingInq.LstOutboundShipInqpackingSlipRpt[i].itm_qty;
                                objOBInquiryExcel.Ctns = objOutboundShipSchedulingInq.LstOutboundShipInqpackingSlipRpt[i].ctn_cnt;
                                objOBInquiryExcel.Qty = objOutboundShipSchedulingInq.LstOutboundShipInqpackingSlipRpt[i].ctn_qty;
                                objOBInquiryExcel.UOM = objOutboundShipSchedulingInq.LstOutboundShipInqpackingSlipRpt[i].qtyUOM;
                                li.Add(objOBInquiryExcel);
                            }

                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["OB_PACKING_SLIP"] = gv;
                            return new DownloadFileActionResult((GridView)Session["OB_PACKING_SLIP"], "OB_PACKING_SLIP" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



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