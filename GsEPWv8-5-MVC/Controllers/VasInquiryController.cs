using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Common;
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
    public class VasInquiryController : Controller
    {

        public string EmailSub = string.Empty;
        public string Folderpath = string.Empty;
        public string EmailMsg = string.Empty;
        public string strDateFormat = string.Empty;
        public string strFileName = string.Empty;
        public string reportFileName = string.Empty;
        public string l_str_rptdtl = string.Empty;
        public string l_str_tmp_name = string.Empty;
        public string ScreenID = "vas Inquiry";
        Email objEmail = new Email();
        IEmailService objEmailService = new EmailService();
        VasInquiry objVasInquiry = new VasInquiry();
        IVasInquiryService ServiceObject = new VasInquiryService();
        Company objCompany = new Company();
        CompanyService ServiceObjectCompany = new CompanyService();
        CustMaster objCustMaster = new CustMaster();
        ICustMasterService objCustMasterService = new CustMasterService();
        public ActionResult VasInquiry(string FullFillType, string cmp, string status, string p_str_scn_id)
        {
            string l_str_cmp_id = string.Empty;
            string l_str_fm_dt = string.Empty;
            string l_str_dflt_dt_reqd = string.Empty;   
            string l_str_is_3rd_usr = string.Empty;   


            try
            {

                Session["g_str_Search_flag"] = "False";
                objVasInquiry.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                objVasInquiry.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                l_str_is_3rd_usr = Session["IS3RDUSER"].ToString();
                objVasInquiry.IS3RDUSER = l_str_is_3rd_usr.Trim();
                objVasInquiry.screentitle = p_str_scn_id;
                if (objVasInquiry.cmp_id == null || objVasInquiry.cmp_id == string.Empty)
                {
                    objVasInquiry.cmp_id = Session["g_str_cmp_id"].ToString().Trim();

                }
                else
                {
                    objCompany.cmp_id = Session["g_str_cmp_id"].ToString().Trim();

                }
                l_str_dflt_dt_reqd = Session["DFLT_DT_REQD"].ToString().Trim();
                objVasInquiry.vas_date_fm = DateTime.Now.AddDays(Common.clsGlobal.DispDateFrom).ToString("MM/dd/yyyy"); ;
                objVasInquiry.vas_date_to = DateTime.Now.ToString("MM/dd/yyyy");

                if (FullFillType == null)
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objVasInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;


                }
                if (FullFillType != null)
                {
                    if (status == "OPEN")
                    {
                        objVasInquiry.Status = "OPEN";
                    }
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objVasInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    objVasInquiry.cmp_id = cmp;
                    objVasInquiry.vas_id_fm = "";
                    objVasInquiry.vas_id_to = "";
                    objVasInquiry.so_num = "";
                    if (l_str_dflt_dt_reqd == "Y")
                    {
                        objVasInquiry = ServiceObject.GetVasInquiryDetails(objVasInquiry);
                    }

                    objCompany = ServiceObjectCompany.GetFullFillCompanyDetails(objCompany);
                }
                LookUp objLookUp = new LookUp();
                LookUpService ServiceObject1 = new LookUpService();
                objLookUp.id = "6";
                objLookUp.lookuptype = "VASINQ";
                objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                objVasInquiry.ListLookUpDtl = objLookUp.ListLookUpDtl;
                objVasInquiry.ListVasRateIdDetails = ServiceObject.fnGetVasRateIdDetails(cmp).ListVasRateIdDetails;
                Mapper.CreateMap<VasInquiry, VasInquiryModel>();
                VasInquiryModel objVasInquiryModel = Mapper.Map<VasInquiry, VasInquiryModel>(objVasInquiry);
                return View(objVasInquiryModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        //public ActionResult CmpIdOnChange(string p_str_cmp_id)
        //{
        //    StockInquiryDtl objStackInquiry = new StockInquiryDtl();
        //    IStockInquiryService ServiceObject = new StockInquiryService();
        //    string l_str_tmp_cmp_id = string.Empty;
        //    Session["g_str_cmp_id"] = p_str_cmp_id;
        //    l_str_tmp_cmp_id = Session["g_str_cmp_id"].ToString().Trim();
        //    return Json(l_str_tmp_cmp_id, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult CmpIdOnChange(string p_str_cmp_id)
        {
            
            string l_str_tmp_cmp_id = string.Empty;
            Session["g_str_cmp_id"] = p_str_cmp_id;
            l_str_tmp_cmp_id = Session["g_str_cmp_id"].ToString().Trim();

            objVasInquiry.ListVasRateIdDetails = ServiceObject.fnGetVasRateIdDetails(p_str_cmp_id).ListVasRateIdDetails;
            Mapper.CreateMap<VasInquiry, VasInquiryModel>();
            VasInquiryModel objVasInquiryModel = Mapper.Map<VasInquiry, VasInquiryModel>(objVasInquiry);
            return PartialView("_VasRateIdDtl", objVasInquiryModel);
         }

        [HttpGet]
        public ActionResult GetVasGridDetails(string p_str_cmp_id, string p_str_vas_id_fm, string p_str_vas_id_to, string p_str_ship_vas_date_fm, 
            string p_str_vas_date_to, string p_str_so_num, string p_str_Status, string p_str_screentitle, string pstrVASRateId)
        {
            VasInquiry objVasInquiry = new VasInquiry();
            IVasInquiryService ServiceObject = new VasInquiryService();
            objVasInquiry.cmp_id = p_str_cmp_id;
            if (Session["UserID"].ToString().Trim() == null)
            {
                objVasInquiry.user_id = "";
            }
            objVasInquiry.user_id = Session["UserID"].ToString().Trim();

            objVasInquiry.is_company_user = Session["IsCompanyUser"].ToString();
            objVasInquiry.vas_id_fm = p_str_vas_id_fm;
            objVasInquiry.vas_id_to = p_str_vas_id_to;
            objVasInquiry.vas_date_fm = p_str_ship_vas_date_fm;
            objVasInquiry.vas_date_to = p_str_vas_date_to;
            objVasInquiry.so_num = p_str_so_num;
            objVasInquiry.Status = p_str_Status;
            objVasInquiry.screentitle = p_str_screentitle;
            objVasInquiry.vasRateId = pstrVASRateId.Trim();
            TempData["cmp_id"] = objVasInquiry.cmp_id;
            TempData["vas_id_fm"] = objVasInquiry.vas_id_fm;
            TempData["vas_id_to"] = objVasInquiry.vas_id_to;
            TempData["vas_date_fm"] = objVasInquiry.vas_date_fm;
            TempData["vas_date_to"] = objVasInquiry.vas_date_to;
            TempData["so_num"] = objVasInquiry.so_num;
            TempData["Status"] = objVasInquiry.Status;
            Session["g_str_Search_flag"] = "True";
            objVasInquiry = ServiceObject.GetVasInquiryDetails(objVasInquiry);

            Mapper.CreateMap<VasInquiry, VasInquiryModel>();
            VasInquiryModel objVasInquiryModel = Mapper.Map<VasInquiry, VasInquiryModel>(objVasInquiry);
            return PartialView("_VasInquirygrid", objVasInquiryModel);
        }

        public ActionResult GetVasGridSummary(string SelectedID, string p_str_cmp_id, string p_str_radio, string p_str_vas_id_fm, string p_str_vas_id_to, string p_str_vas_date_fm, string p_str_vas_date_to, string p_str_so_num, string p_str_Status)
        {
            try
            {
                VasInquiry objVasInquiry = new VasInquiry();
                IVasInquiryService ServiceObject = new VasInquiryService();
                objVasInquiry.cmp_id = p_str_cmp_id;
                objVasInquiry.ship_doc_id = SelectedID;
                TempData["cmp_id"] = p_str_cmp_id;
                TempData["vas_id_fm"] = p_str_vas_id_fm;
                TempData["vas_id_to"] = p_str_vas_id_to;
                TempData["vas_date_fm"] = p_str_vas_date_fm;
                TempData["vas_date_to"] = p_str_vas_date_to;
                TempData["so_num"] = p_str_so_num;
                TempData["Status"] = p_str_Status;
                TempData["ReportSelection"] = p_str_radio;
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
        public JsonResult VAS_ID_INQ_HDR_DATA(string p_str_cmp_id, string p_str_vas_id_fm, string p_str_vas_id_to, string p_str_vas_date_fm, string p_str_vas_date_to, string p_str_so_num, string p_str_Status, string p_str_Screen)
        {
            VasInquiry objVasInquiry = new VasInquiry();
            IVasInquiryService ServiceObject = new VasInquiryService();
            Session["g_str_cmp_id"] = p_str_cmp_id.Trim();
            Session["TEMP_VAS_CMP_ID"] = p_str_cmp_id.Trim();
            Session["TEMP_VAS_ID_FM"] = p_str_vas_id_fm.Trim();
            Session["TEMP_VAS_ID_TO"] = p_str_vas_id_to.Trim();
            Session["TEMP_VAS_DT_FM"] = p_str_vas_date_fm.Trim();
            Session["TEMP_VAS_DT_TO"] = p_str_vas_date_to.Trim();
            Session["TEMP_VAS_SO_NUM"] = p_str_so_num.Trim();
            Session["TEMP_VAS_TYPE"] = p_str_Status.Trim();
            Session["TEMP_VAS_SCREEN"] = p_str_Screen.Trim();
            return Json(objVasInquiry, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetVasGridINQ(string p_str_cmp_id, string p_str_vas_id)
        {
            //string l_str_tot_bill_amt;

            VasInquiry objVasInquiry = new VasInquiry();
            IVasInquiryService ServiceObject = new VasInquiryService();
            string l_str_search_flag = string.Empty;
            string l_str_is_another_usr = string.Empty;
            l_str_is_another_usr = Session["IS3RDUSER"].ToString();
            objVasInquiry.IS3RDUSER = l_str_is_another_usr.Trim();
            l_str_search_flag = Session["g_str_Search_flag"].ToString().Trim();
            if (Session["UserID"].ToString().Trim() == null)
            {
                objVasInquiry.user_id = "";
            }
            objVasInquiry.user_id = Session["UserID"].ToString().Trim();
            objVasInquiry.is_company_user = Session["IsCompanyUser"].ToString();

            if (l_str_search_flag == "True")
            {
                objVasInquiry.cmp_id = p_str_cmp_id;
                objVasInquiry.vas_id_fm = p_str_vas_id;
                objVasInquiry.vas_id_fm = Session["TEMP_VAS_ID_FM"].ToString().Trim();
                objVasInquiry.vas_id_to = Session["TEMP_VAS_ID_TO"].ToString().Trim();
                objVasInquiry.vas_date_fm = Session["TEMP_VAS_DT_FM"].ToString().Trim();
                objVasInquiry.vas_date_to = Session["TEMP_VAS_DT_TO"].ToString().Trim();
                objVasInquiry.so_num = Session["TEMP_VAS_SO_NUM"].ToString().Trim();
                objVasInquiry.Status = Session["TEMP_VAS_TYPE"].ToString().Trim();
                objVasInquiry.screentitle = Session["TEMP_VAS_SCREEN"].ToString().Trim();

                
            }
            else
            {
                objVasInquiry.cmp_id = p_str_cmp_id;
                objVasInquiry.vas_id_fm = p_str_vas_id;
                Session["g_str_Search_flag"] = "False";

            }
            objVasInquiry.screentitle = Session["TEMP_VAS_SCREEN"].ToString().Trim();


            if (objVasInquiry.vas_date_fm == null || objVasInquiry.vas_date_fm == "" || objVasInquiry.vas_date_to == null || objVasInquiry.vas_date_to == "")
            {
                objVasInquiry.vas_date_fm = DateTime.Now.AddDays(Common.clsGlobal.DispDateFrom).ToString("MM/dd/yyyy");
                objVasInquiry.vas_date_to = DateTime.Now.ToString("MM/dd/yyyy");
            }
            objVasInquiry = ServiceObject.GetVasInquiryDetails(objVasInquiry);
            //objVasInquiry.ListVasInquiry.
            Mapper.CreateMap<VasInquiry, VasInquiryModel>();
            VasInquiryModel objVasInquiryModel = Mapper.Map<VasInquiry, VasInquiryModel>(objVasInquiry);

            return PartialView("_VasInquirygrid", objVasInquiryModel);
        }
        public JsonResult VAS_INQ_HDR_DATA(string p_str_cmp_id, string p_str_vas_id_fm, string p_str_vas_id_to, string p_str_ship_vas_date_fm,
     string p_str_vas_date_to, string p_str_so_num, string p_str_Status)
        {
            VasInquiry objVasInquiry = new VasInquiry();
            IVasInquiryService ServiceObject = new VasInquiryService();
            Session["g_str_cmp_id"] = p_str_cmp_id.Trim();
            Session["TEMP_CMP_ID"] = p_str_cmp_id.Trim();
            Session["TEMP_VASID_FRM"] = p_str_vas_id_fm.Trim();
            Session["TEMP_VASID_TO"] = p_str_vas_id_to.Trim();
            Session["TEMP_VASDT_FRM"] = p_str_ship_vas_date_fm.Trim();
            Session["TEMP_VASDT_TO"] = p_str_vas_date_to.Trim();
            Session["TEMP_SO_NUM"] = p_str_so_num.Trim();
            Session["TEMP_STATUS"] = p_str_Status.Trim();
            return Json(objVasInquiry.VasCount, JsonRequestBehavior.AllowGet);

        }
        //CR - 3PL-MVC-VAS-20180505 Added by Soniya
        public ActionResult VasInquiryDtl(string p_str_cmp_id, string p_str_vas_id)
        {
            try
            {

                VasInquiry objVasInquiry = new VasInquiry();
                IVasInquiryService ServiceObject = new VasInquiryService();
                string l_str_search_flag = string.Empty;
                string l_str_is_another_usr = string.Empty;
                l_str_search_flag = Session["g_str_Search_flag"].ToString().Trim();
                if (Session["UserID"].ToString().Trim() == null)
                {
                    objVasInquiry.user_id = "";
                }
                objVasInquiry.user_id = Session["UserID"].ToString().Trim();
                //objVasInquiry = ServiceObject.CheckCompanyUser(objVasInquiry);
                //{
                objVasInquiry.is_company_user = Session["IsCompanyUser"].ToString();///objVasInquiry.ListVasInquiry[0].is_cmp_user;
                //}
                if (l_str_search_flag == "True")
                {
                    if (Session["UserID"].ToString().Trim() == null)
                    {
                        objVasInquiry.user_id = "";
                    }
                    objVasInquiry.user_id = Session["UserID"].ToString().Trim();
                    //objVasInquiry = ServiceObject.CheckCompanyUser(objVasInquiry);
                    //{
                    objVasInquiry.is_company_user = Session["IsCompanyUser"].ToString(); //objVasInquiry.ListVasInquiry[0].is_cmp_user;
                                                                                         // }
                    objVasInquiry.cmp_id = Session["TEMP_CMP_ID"].ToString().Trim();
                    objVasInquiry.vas_id_fm = Session["TEMP_VASID_FRM"].ToString().Trim();//CR-180421-001 Added By Nithya
                    objVasInquiry.vas_id_to = Session["TEMP_VASID_TO"].ToString().Trim();
                    objVasInquiry.vas_date_fm = Session["TEMP_VASDT_FRM"].ToString().Trim();
                    objVasInquiry.vas_date_to = Session["TEMP_VASDT_TO"].ToString().Trim();//CR-180421-001 Added By Nithya
                    objVasInquiry.so_num = Session["TEMP_SO_NUM"].ToString().Trim();
                    objVasInquiry.Status = Session["TEMP_STATUS"].ToString().Trim();

                }
                else
                {
                    objVasInquiry.cmp_id = p_str_cmp_id;
                    objVasInquiry.vas_id_fm = p_str_vas_id;
                }

                objVasInquiry = ServiceObject.GetVasInquiryDetails(objVasInquiry);
                Mapper.CreateMap<VasInquiry, VasInquiryModel>();
                VasInquiryModel objVasInquirymodel = Mapper.Map<VasInquiry, VasInquiryModel>(objVasInquiry);
                return PartialView("_VasInquirygrid", objVasInquirymodel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult TruncateVasEntry()
        {
            VasInquiry objVasInquiry = new VasInquiry();
            IVasInquiryService ServiceObject = new VasInquiryService();
            ServiceObject.TruncateTempVasEntry(objVasInquiry);
            Mapper.CreateMap<VasInquiry, VasInquiryModel>();
            VasInquiryModel objVasInquirymodel = Mapper.Map<VasInquiry, VasInquiryModel>(objVasInquiry);
            return View("~/Views/VasInquiry/VasInquiry.cshtml", objVasInquirymodel);
        }
        public ActionResult GetVasInqRpt(string SelectedID, string p_str_cmpid, string p_str_radio)
        {
            try
            {
                OutboundShipInq objOutboundShipInq = new OutboundShipInq();
                OutboundShipInqService ServiceObject = new OutboundShipInqService();
                objOutboundShipInq.cmp_id = p_str_cmpid;
                objOutboundShipInq.ship_doc_id = SelectedID;
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
        public ActionResult EmailShowReport(string p_str_cmp_id, string p_str_radio, string p_str_vas_id_fm, string p_str_vas_id_to, string p_str_vas_date_fm, string p_str_vas_date_to, string p_str_so_num, string p_str_Status, string SelectdID, string type)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            l_str_rpt_selection = p_str_radio;
            string reportFileName = string.Empty;   //CR2018-03-15-001 Added By Soniya
            Folderpath = System.Configuration.ConfigurationManager.AppSettings["tempFilepath"].ToString().Trim();
            objCompany.cmp_id = p_str_cmp_id.ToString().Trim();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            objCompany.LstCmpName = objCompany.LstCmpName;
            l_str_tmp_name = objCompany.LstCmpName[0].cmp_name.ToString().Trim();
            objEmail.CmpId = p_str_cmp_id;
            objEmail.screenId = ScreenID;
            objEmail.username = objCompany.user_id;
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
                        EmailSub = "Vas Inquiry Report for" + " " + " " + objVasInquiry.cmp_id;
                        EmailMsg = "Vas Inquiry Report hasbeen Attached for the Process";

                        if (type == "PDF")
                        {
                            string strDateFormat = string.Empty;
                            string strFileName = string.Empty;
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
                                    objVasInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objVasInquiry.Image_Path);
                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//Vas Inquiry_" + strDateFormat + ".pdf";
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                }
                            }
                            reportFileName = "Vas Inquiry Report " + DateTime.Now.ToFileTime() + ".pdf";
                            Session["RptFileName"] = strFileName;
                        }
                        //else if (type == "Word")
                        //{
                        //    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        //}
                        //else
                        if (type == "Excel")
                        {

                            List<VAS_Grid_SummaryExcel> li = new List<VAS_Grid_SummaryExcel>();
                            for (int i = 0; i < objVasInquiry.ListVasInquiry.Count; i++)
                            {

                                VAS_Grid_SummaryExcel objOBInquiryExcel = new VAS_Grid_SummaryExcel();
                                objOBInquiryExcel.VasID = objVasInquiry.ListVasInquiry[i].VasID;
                                objOBInquiryExcel.VasDate = objVasInquiry.ListVasInquiry[i].VasDate;
                                objOBInquiryExcel.status = objVasInquiry.ListVasInquiry[i].status;
                                objOBInquiryExcel.Note = objVasInquiry.ListVasInquiry[i].Note;
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
                        string strDateFormat = string.Empty;
                        string strFileName = string.Empty;
                        strReportName = "rpt_iv_vas.rpt";
                        VasInquiry objVasInquiry = new VasInquiry();
                        IVasInquiryService ServiceObject = new VasInquiryService();
                       
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//VasInquiry//" + strReportName;
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
                        objVasInquiry.cmp_id = p_str_cmp_id;
                        objVasInquiry.ship_doc_id = SelectdID;
                        objVasInquiry = ServiceObject.GetVasInquiryPostRptCtnValues(objVasInquiry);
                        if (objVasInquiry.ListVasInquiry.Count > 0)
                        {
                            objVasInquiry.ship_qty = objVasInquiry.ListVasInquiry[0].ship_qty;
                            objVasInquiry.ship_itm_price = objVasInquiry.ListVasInquiry[0].ship_itm_price;
                        }
                        objVasInquiry = ServiceObject.GetVasPostDetails(objVasInquiry);
                        objVasInquiry.so_num = (objVasInquiry.ListVasInquiry[0].so_num == null || objVasInquiry.ListVasInquiry[0].so_num.Trim() == "" ? string.Empty : objVasInquiry.ListVasInquiry[0].so_num.Trim());
                        objVasInquiry.ship_dt = objVasInquiry.ListVasInquiry[0].ShipDt.ToString("dd/MM/yyyy");
                        objVasInquiry.whs_id = (objVasInquiry.ListVasInquiry[0].whs_id == null || objVasInquiry.ListVasInquiry[0].whs_id.Trim() == "" ? string.Empty : objVasInquiry.ListVasInquiry[0].whs_id.Trim());
                        objVasInquiry.cust_po_num = (objVasInquiry.ListVasInquiry[0].cust_ordr_num == null || objVasInquiry.ListVasInquiry[0].cust_ordr_num.Trim() == "" ? string.Empty : objVasInquiry.ListVasInquiry[0].cust_ordr_num.Trim());
                        objVasInquiry.bill_doc_id = (objVasInquiry.ListVasInquiry[0].bill_doc_id == null || objVasInquiry.ListVasInquiry[0].bill_doc_id.Trim() == "" ? string.Empty : objVasInquiry.ListVasInquiry[0].bill_doc_id.Trim());
                        objVasInquiry.cust_cust_name = (objVasInquiry.ListVasInquiry[0].cust_cust_name == null || objVasInquiry.ListVasInquiry[0].cust_cust_name.Trim() == "" ? string.Empty : objVasInquiry.ListVasInquiry[0].cust_cust_name.Trim());
                        if (((objVasInquiry.cust_cust_name == "" || objVasInquiry.cust_cust_name == null || objVasInquiry.cust_cust_name == "-") && (objVasInquiry.cust_po_num == "" || objVasInquiry.cust_po_num == null || objVasInquiry.cust_po_num == "-") && (objVasInquiry.bill_doc_id == "" || objVasInquiry.bill_doc_id == null || objVasInquiry.bill_doc_id == "-")))
                        {
                            l_str_rptdtl = objVasInquiry.cmp_id + "_" + "VAS POST" + "_" + objVasInquiry.ship_doc_id;
                            objEmail.EmailSubject = objVasInquiry.cmp_id + "-" + " " + " " + "VAS POST" + "|" + " " + " " + "IB#: " + " " + objVasInquiry.ship_doc_id + "|" + " " + " " + "Ship Date: " + objVasInquiry.ship_dt + "|" + " " + " " + "Store ID: " + objVasInquiry.whs_id;
                            objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objVasInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Ship Doc Id#: " + " " + " " + objVasInquiry.ship_doc_id + "\n" + "Ship Date: " + " " + objVasInquiry.ship_dt + "\n" + "Store ID: " + objVasInquiry.whs_id + "\n" + "Ship Qty: " + objVasInquiry.ship_qty + "\n" + "Ship Item Price: " + objVasInquiry.ship_itm_price;

                        }
                        else if ((objVasInquiry.cust_po_num == "" || objVasInquiry.cust_po_num == null || objVasInquiry.cust_po_num == "-") && (objVasInquiry.bill_doc_id == "" || objVasInquiry.bill_doc_id == null || objVasInquiry.bill_doc_id == "-"))
                        {
                            l_str_rptdtl = objVasInquiry.cmp_id + "_" + "VAS POST" + "_" + objVasInquiry.ship_doc_id;
                            objEmail.EmailSubject = objVasInquiry.cmp_id + "-" + " " + " " + "VAS POST" + "|" + " " + " " + "IB#: " + " " + objVasInquiry.ship_doc_id + "|" + " " + " " + "Ship Date: " + objVasInquiry.ship_dt + "|" + " " + " " + "Store ID: " + objVasInquiry.whs_id;
                            objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objVasInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Ship Doc Id#: " + " " + " " + objVasInquiry.ship_doc_id + "\n" + "Ship Date: " + " " + objVasInquiry.ship_dt + "\n" + "Store ID: " + objVasInquiry.whs_id + "\n" + "Ship Qty: " + objVasInquiry.ship_qty + "\n" + "Ship Item Price: " + objVasInquiry.ship_itm_price + "\n" + "Custname: " + objVasInquiry.cust_cust_name;
                        }
                        else if ((objVasInquiry.cust_cust_name == "" || objVasInquiry.cust_cust_name == null || objVasInquiry.cust_cust_name == "-") && (objVasInquiry.bill_doc_id == "" || objVasInquiry.bill_doc_id == null || objVasInquiry.bill_doc_id == "-"))
                        {
                            l_str_rptdtl = objVasInquiry.cmp_id + "_" + "VAS POST" + "_" + objVasInquiry.ship_doc_id;
                            objEmail.EmailSubject = objVasInquiry.cmp_id + "-" + " " + " " + "VAS POST" + "|" + " " + " " + "IB#: " + " " + objVasInquiry.ship_doc_id + "|" + " " + " " + "Ship Date: " + objVasInquiry.ship_dt + "|" + " " + " " + "Store ID: " + objVasInquiry.whs_id;
                            objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objVasInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Ship Doc Id#: " + " " + " " + objVasInquiry.ship_doc_id + "\n" + "Ship Date: " + " " + objVasInquiry.ship_dt + "\n" + "Store ID: " + objVasInquiry.whs_id + "\n" + "Store ID: " + objVasInquiry.whs_id + "\n" + "Ship Qty: " + objVasInquiry.ship_qty + "\n" + "Ship Item Price: " + objVasInquiry.ship_itm_price + "\n" + "CustPO#: " + objVasInquiry.cust_po_num;
                        }
                        else if ((objVasInquiry.cust_cust_name == "" || objVasInquiry.cust_cust_name == null || objVasInquiry.cust_cust_name == "-") && (objVasInquiry.cust_po_num == "" || objVasInquiry.cust_po_num == null || objVasInquiry.cust_po_num == "-"))
                        {
                            l_str_rptdtl = objVasInquiry.cmp_id + "_" + "VAS POST" + "_" + objVasInquiry.ship_doc_id;
                            objEmail.EmailSubject = objVasInquiry.cmp_id + "-" + " " + " " + "VAS POST" + "|" + " " + " " + "IB#: " + " " + objVasInquiry.ship_doc_id + "|" + " " + " " + "Ship Date: " + objVasInquiry.ship_dt + "|" + " " + " " + "Store ID: " + objVasInquiry.whs_id;
                            objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objVasInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Ship Doc Id#: " + " " + " " + objVasInquiry.ship_doc_id + "\n" + "Ship Date: " + " " + objVasInquiry.ship_dt + "\n" + "Store ID: " + objVasInquiry.whs_id + "\n" + "Ship Qty: " + objVasInquiry.ship_qty + "\n" + "Ship Item Price: " + objVasInquiry.ship_itm_price + "\n" + "BillDocID#: " + objVasInquiry.bill_doc_id;
                        }
                        else
                        {
                            l_str_rptdtl = objVasInquiry.cmp_id + "_" + "VAS POST" + "_" + objVasInquiry.ship_doc_id;
                            objEmail.EmailSubject = objVasInquiry.cmp_id + "-" + " " + " " + "VAS POST" + "|" + " " + " " + "IB#: " + " " + objVasInquiry.ship_doc_id + "|" + " " + " " + "Ship Date: " + objVasInquiry.ship_dt + "|" + " " + " " + "Store ID: " + objVasInquiry.whs_id;
                            objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objVasInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Ship Doc Id#: " + " " + " " + objVasInquiry.ship_doc_id + "\n" + "Ship Date: " + " " + objVasInquiry.ship_dt + "\n" + "Store ID: " + objVasInquiry.whs_id + "\n" + "Ship Qty: " + objVasInquiry.ship_qty + "\n" + "Ship Item Price: " + objVasInquiry.ship_itm_price + "\n" + "BillDocID#: " + objVasInquiry.bill_doc_id;
                        }
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
                                    objVasInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objVasInquiry.Image_Path);
                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + strDateFormat + ".pdf";
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                }
                            }
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

        public ActionResult ShowReport(string p_str_cmp_id, string p_str_radio, string p_str_vas_id_fm, string p_str_vas_id_to, string p_str_vas_date_fm, string p_str_vas_date_to, string p_str_so_num, string p_str_Status, string SelectedID, string type)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
            string strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
            string l_str_file_name = string.Empty;
            DataTable dtBill = new DataTable();
            string tempFileName = string.Empty;

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
                        objCustMaster.cust_id = p_str_cmp_id;
                        objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
                        if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
                        {
                            objCustMaster.ListGetCustLogo[0].cust_logo = "";
                        }

                        objVasInquiry.cmp_id = p_str_cmp_id;
                        objVasInquiry.vas_id_fm = p_str_vas_id_fm;
                        objVasInquiry.vas_id_to = p_str_vas_id_to;
                        objVasInquiry.vas_date_fm = p_str_vas_date_fm;
                        objVasInquiry.vas_date_to = p_str_vas_date_to;
                        objVasInquiry.so_num = p_str_so_num;
                        objVasInquiry.Status = p_str_Status;

                        if (type == "PDF")
                        {
                            objVasInquiry = ServiceObject.GetVasInquiryDetailsRpt(objVasInquiry);
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
                                    //objVasInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    objVasInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo; //CR20180811-001 Added By Nithya
                                    rd.SetParameterValue("fml_image_path", objVasInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Vas Grid Summary");
                                }
                            }

                        }

                        else
                        if (type == "Excel")
                        {

                            dtBill = ServiceObject.GetVasInquiryDetailsRptExcel(p_str_cmp_id, p_str_vas_id_fm, p_str_vas_id_to, p_str_vas_date_fm, p_str_vas_date_to, p_str_so_num, p_str_Status);

                            if (!Directory.Exists(strOutputpath))
                            {
                                Directory.CreateDirectory(strOutputpath);
                            }

                            l_str_file_name = "DF_" + p_str_cmp_id.ToUpper().ToString().Trim() + "_VAS_SMRY_RPT_" + strDateFormat + ".xlsx";

                            tempFileName = strOutputpath + l_str_file_name;

                            if (System.IO.File.Exists(tempFileName))
                                System.IO.File.Delete(tempFileName);
                            xls_VAS_Summary_Excel mxcel1 = new xls_VAS_Summary_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "VAS_SMRY_RPT.xlsx");
                            var dataRows = dtBill.Rows;
                            DataRow dr = dataRows[0];
                            mxcel1.PopulateHeader(p_str_cmp_id);
                            mxcel1.PopulateData(dtBill, true);
                            mxcel1.SaveAs(tempFileName);
                            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                            return File(fs, "application / xlsx", l_str_file_name);
                           

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
                        objCustMaster.cust_id = p_str_cmp_id;
                        objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
                        if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
                        {
                            objCustMaster.ListGetCustLogo[0].cust_logo = "";
                        }
                        //END
                        if (type == "PDF")
                        {
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
                                    //objVasInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    objVasInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim();
                                    rd.SetParameterValue("fml_image_path", objVasInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Vas Post Report");
                                }
                            }
                        }
                        
                        else
                        if (type == "Excel")
                        {
                            string l_str_cust_name = String.Empty;
                            string l_str_whs_id = String.Empty;
                            string l_str_ship_doc_id = String.Empty;
                            string l_str_ShipDt = String.Empty;
                            string l_str_po_num = String.Empty;
                            string l_str_ship_to = String.Empty;
                            dtBill = ServiceObject.GetVasPostDetailsExcel(p_str_cmp_id, SelectedID);
                            var dataRows = dtBill.Rows;
                            var dataColumns = dtBill.Columns;
                            if (!Directory.Exists(strOutputpath))
                            {
                                Directory.CreateDirectory(strOutputpath);
                            }

                            l_str_file_name = "DF_" + p_str_cmp_id.ToUpper().ToString().Trim() + "_VAS_POST_RPT_" + strDateFormat + ".xlsx";

                            tempFileName = strOutputpath + l_str_file_name;

                            if (System.IO.File.Exists(tempFileName))
                                System.IO.File.Delete(tempFileName);
                            xls_VAS_Post_Excel mxcel1 = new xls_VAS_Post_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "VAS_POST_RPT.xlsx");
                            DataRow dr = dataRows[0];
                            l_str_cust_name = dr["cust_name"].ToString();
                            l_str_whs_id = dr["whs_id"].ToString();
                            l_str_ship_doc_id = dr["ship_doc_id"].ToString();
                            l_str_ShipDt = dr["ShipDt"].ToString();
                            l_str_po_num = dr["po_num"].ToString();
                            l_str_ship_to = dr["ship_to"].ToString();
                            mxcel1.PopulateHeader(p_str_cmp_id, l_str_cust_name, l_str_whs_id, l_str_ship_doc_id, l_str_ShipDt, l_str_po_num, l_str_ship_to);
                            mxcel1.PopulateData(dtBill, true);
                            mxcel1.SaveAs(tempFileName);
                            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                            return File(fs, "application / xlsx", l_str_file_name);
                           
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

        public ActionResult VasDetail(string VasId, string cmp_id)
        {
            var status = string.Empty;
            VasInquiry objVasInquiry = new VasInquiry();
            IVasInquiryService ServiceObject = new VasInquiryService();
            objVasInquiry.cmp_id = cmp_id;
            objVasInquiry.ship_doc_id = VasId;
            objVasInquiry = ServiceObject.GetVashdr(objVasInquiry);
            objVasInquiry.Status = objVasInquiry.ListVasInquiry[0].Status.Trim();
            status = objVasInquiry.Status;
            if (status == "P")
            {
                objVasInquiry.Status = "POST";
            }
            else
            {
                objVasInquiry.Status = "OPEN";
            }
            objVasInquiry.whs_id = objVasInquiry.ListVasInquiry[0].whs_id;
            if (objVasInquiry.ListVasInquiry[0].whs_id == null || objVasInquiry.ListVasInquiry[0].whs_id == "")
            {
                objVasInquiry.whs_id = "";
            }
            else
            {
                objVasInquiry.whs_id = objVasInquiry.ListVasInquiry[0].whs_id;
            }
            //objVasInquiry.whs_id = objVasInquiry.ListVasInquiry[0].whs_id.Trim();
            objVasInquiry.ship_type = objVasInquiry.ListVasInquiry[0].ship_type;
            objVasInquiry.ship_to = objVasInquiry.ListVasInquiry[0].ship_to;
            if (objVasInquiry.ListVasInquiry[0].notes == null || objVasInquiry.ListVasInquiry[0].notes == "")
            {
                objVasInquiry.notes = "";
            }
            else
            {
                objVasInquiry.notes = objVasInquiry.ListVasInquiry[0].notes.Trim();
            }
            //objVasInquiry.notes = (objVasInquiry.ListVasInquiry[0].notes.Trim() == null || objVasInquiry.ListVasInquiry[0].notes.Trim() == "") ? "" : objVasInquiry.ListVasInquiry[0].notes.Trim();
            objVasInquiry.ship_dt = objVasInquiry.ListVasInquiry[0].ship_dt;
            objVasInquiry.po_num = objVasInquiry.ListVasInquiry[0].po_num;
            objVasInquiry = ServiceObject.GetVasdtl(objVasInquiry);
            Mapper.CreateMap<VasInquiry, VasInquiryModel>();
            VasInquiryModel objVasInquiryModel = Mapper.Map<VasInquiry, VasInquiryModel>(objVasInquiry);
            return PartialView("_VasDetail", objVasInquiryModel);
        }

        public ActionResult VasSRDetail(string SRId, string cmp_id, string BatchId)
        {
            var status = string.Empty;

            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.CompID = cmp_id;
            objOutboundInq.ShipReqID = SRId;
            objOutboundInq.QuoteNum = BatchId;
            objOutboundInq = ServiceObject.OutboundShipInqhdr(objOutboundInq);
            if (objOutboundInq.LstOutboundInqpickstyleRpt.Count() == 0)
            {

                return Json(objOutboundInq.l_int_sr_detail, JsonRequestBehavior.AllowGet);
            }
            else
            {
                objOutboundInq.ShipReqID = objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipReqID.Trim();
                objOutboundInq.ShipReqDt = objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipReqDt.Trim();
                objOutboundInq.OrdType = objOutboundInq.LstOutboundInqpickstyleRpt[0].OrdType.Trim();
                objOutboundInq.status = objOutboundInq.LstOutboundInqpickstyleRpt[0].status.Trim();
                if (status == "P")
                {
                    objOutboundInq.status = "POST";
                }
                else
                {
                    objOutboundInq.status = "OPEN";
                }
                objOutboundInq.step = objOutboundInq.LstOutboundInqpickstyleRpt[0].step.Trim();
                objOutboundInq.cust_id = objOutboundInq.LstOutboundInqpickstyleRpt[0].cust_id.Trim();
                objOutboundInq.OrdNum = objOutboundInq.LstOutboundInqpickstyleRpt[0].OrdNum.Trim();
                objOutboundInq.CustPO = objOutboundInq.LstOutboundInqpickstyleRpt[0].CustPO.Trim();
                objOutboundInq.DeptID = objOutboundInq.LstOutboundInqpickstyleRpt[0].DeptID.Trim();
                objOutboundInq.ShipVia = objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipVia.Trim();
                objOutboundInq.FreightID = objOutboundInq.LstOutboundInqpickstyleRpt[0].FreightID.Trim();
                objOutboundInq.ShipDt = objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipDt.Trim();
                objOutboundInq.CancelDt = objOutboundInq.LstOutboundInqpickstyleRpt[0].CancelDt.Trim();
                objOutboundInq.fob = objOutboundInq.LstOutboundInqpickstyleRpt[0].fob.Trim();
                objOutboundInq.store_id = objOutboundInq.LstOutboundInqpickstyleRpt[0].store_id.Trim();

                objOutboundInq.DCID = (objOutboundInq.LstOutboundInqpickstyleRpt[0].DCID == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].DCID == "") ? objOutboundInq.LstOutboundInqpickstyleRpt[0].DCID.Trim() : null;
                objOutboundInq.ShipToAttn = objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToAttn.Trim();
                objOutboundInq.ShipToEmail = objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToEmail.Trim();
                objOutboundInq.ShipToAddr1 = objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToAddr1.Trim();
                objOutboundInq.ShipToAddr2 = objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToAddr2.Trim();
                objOutboundInq.ShipToCity = objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToCity.Trim();
                objOutboundInq.ShipToState = objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToState.Trim();
                //objOutboundInq.ShipToZipCode = objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToZipCode.Trim();
                objOutboundInq.ShipToZipCode = (objOutboundInq.ShipToZipCode == null || objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToZipCode.Trim() == "") ? objOutboundInq.ShipToZipCode : null;
                objOutboundInq.ShipToCountry = objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToCountry.Trim();
                objOutboundInq.note = objOutboundInq.LstOutboundInqpickstyleRpt[0].note.Trim();
                objOutboundInq.ShipToID = objOutboundInq.LstOutboundInqpickstyleRpt[0].ShipToID.Trim();
                objOutboundInq = ServiceObject.OutboundShipInqdtl(objOutboundInq);
            }
            Mapper.CreateMap<OutboundInq, OutboundInqModel>();
            OutboundInqModel objOutboundInqModel = Mapper.Map<OutboundInq, OutboundInqModel>(objOutboundInq);
            return PartialView("_ShipReqDetail", objOutboundInqModel);
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
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//VasInquiry//" + strReportName;
                    objVasInquiry.cmp_id = p_str_cmpid;
                    objVasInquiry.ship_doc_id = p_str_Shipping_id;
                    objVasInquiry = ServiceObject.GetVasPostDetails(objVasInquiry);
                    //CR20180811-001 Added By Nithya
                    objCustMaster.cust_id = p_str_cmpid;
                    objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
                    if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
                    {
                        objCustMaster.ListGetCustLogo[0].cust_logo = "";
                    }
                    //END
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
                           // objVasInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo; //CR20180811-001 Added By Nithya
                          
                            rd.SetParameterValue("fml_image_path", objVasInquiry.Image_Path);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Vas Post Report");
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
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                    objOutboundInq.cmp_id = p_str_cmpid;
                    objOutboundInq.Sonum = p_str_ShipReq_id;
                    objOutboundInq = ServiceObject.OutboundInqShipAck(objOutboundInq);
                    var rptSource = objOutboundInq.LstOutboundInqpickstyleRpt.ToList();
                    if (rptSource.Count > 0)
                    {
                        using (ReportDocument rd = new ReportDocument())
                        {
                        
                            //CR20180811-001 Added By Nithya
                            objCustMaster.cust_id = p_str_cmpid;
                            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
                            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
                            {
                                objCustMaster.ListGetCustLogo[0].cust_logo = "";
                            }
                            //END
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objOutboundInq.LstOutboundInqpickstyleRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objOutboundInq.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim();
                            rd.SetParameterValue("fml_image_path", objOutboundInq.Image_Path);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "SR ACK  Report");
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
        public ActionResult Add(string cmpid, string Sonum, string p_str_scn_id)
        {
            string l_str_vasid;
            //string l_str_whsid;   
            string l_str_vas_user_id;
            VasInquiry objVasInquiry = new VasInquiry();
            IVasInquiryService ServiceObject = new VasInquiryService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objVasInquiry.cmp_id = cmpid;
            objVasInquiry.so_num = Sonum;
            objVasInquiry.screentitle = p_str_scn_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objVasInquiry.Status = "O";
            objVasInquiry.ship_dt = DateTime.Now.ToString("MM/dd/yyyy");
            objVasInquiry.cust_po_dt = DateTime.Now.ToString("MM/dd/yyyy");
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objVasInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objVasInquiry = ServiceObject.GetVasIdDetail(objVasInquiry);
            objVasInquiry.ship_doc_id = objVasInquiry.vas_id;
            l_str_vasid = objVasInquiry.vas_id;
            objVasInquiry.ship_doc_id = l_str_vasid;
            objVasInquiry = ServiceObject.GetVasUserId(objVasInquiry);
            ServiceObject.TruncateTempVasEntry(objVasInquiry);
            objVasInquiry.vas_user_id = objVasInquiry.vas_user_id;
            l_str_vas_user_id = objVasInquiry.vas_user_id;
            objVasInquiry.vas_user_id = l_str_vas_user_id;
            objVasInquiry = ServiceObject.GetDftWhs(objVasInquiry);
            string l_str_DftWhs = objVasInquiry.ListPickdtl[0].dft_whs.Trim();
            if (l_str_DftWhs != "" || l_str_DftWhs != null)
            {
                objVasInquiry.fob = objVasInquiry.ListPickdtl[0].dft_whs.Trim();
                objVasInquiry.whs_id = objVasInquiry.ListPickdtl[0].dft_whs.Trim();

            }
            objVasInquiry.cmp_id = cmpid;
            objVasInquiry.so_num = Sonum;
            objVasInquiry = ServiceObject.LoadSrDetails(objVasInquiry);
            if (objVasInquiry.ListLoadSrDetails.Count > 0)
            {
                objVasInquiry.ship_to_id = (objVasInquiry.ListLoadSrDetails[0].shipto_id == null || objVasInquiry.ListLoadSrDetails[0].shipto_id.Trim() == "") ? null : objVasInquiry.ListLoadSrDetails[0].shipto_id;
                objVasInquiry.cust_po_num = (objVasInquiry.ListLoadSrDetails[0].cust_ordr_num == null || objVasInquiry.ListLoadSrDetails[0].cust_ordr_num.Trim() == "") ? null : objVasInquiry.ListLoadSrDetails[0].cust_ordr_num;
                if (objVasInquiry.ship_to_id == null)
                {
                    objVasInquiry.ship_to_id = "-";
                }
                else
                {
                    objVasInquiry.ship_to_id = (objVasInquiry.ListLoadSrDetails[0].shipto_id == null || objVasInquiry.ListLoadSrDetails[0].shipto_id.Trim() == "") ? null : objVasInquiry.ListLoadSrDetails[0].shipto_id;
                }
                if (objVasInquiry.cust_po_num == null)
                {
                    objVasInquiry.cust_po_num = "-";
                }
                else
                {
                    objVasInquiry.cust_po_num = (objVasInquiry.ListLoadSrDetails[0].cust_ordr_num == null || objVasInquiry.ListLoadSrDetails[0].cust_ordr_num.Trim() == "") ? null : objVasInquiry.ListLoadSrDetails[0].cust_ordr_num;
                }
                if (p_str_scn_id == "OBInquiry")
                {
                    objVasInquiry.Note = "Ship To:" + objVasInquiry.ship_to_id + " " + " " + " " + "Cust PO#:" + objVasInquiry.cust_po_num;
                }
                else
                {
                    objVasInquiry.Note = (objVasInquiry.ListLoadSrDetails[0].note == null || objVasInquiry.ListLoadSrDetails[0].note.Trim() == "") ? null : objVasInquiry.ListLoadSrDetails[0].note;
                }
            }
            //objCompany.cust_cmp_id = cmpid;
            //objCompany.whs_id = "";           
            //objCompany = ServiceObjectCompany.GetWhsIdDetails(objCompany);
            //objVasInquiry.ListwhsPickDtl = objCompany.ListwhsPickDtl;
            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = cmpid;
            objPick.Whs_id = "";
            objPick.Whs_name = "";
            objPick = ServiceObjectPick.GetWhsPick(objPick);
            objVasInquiry.ListPick = objPick.ListPick;
            objVasInquiry = ServiceObject.GetVasEntryDtl(objVasInquiry);
            for (int i = 0; i < objVasInquiry.ListVasEntryGridDtl.Count(); i++)
            {
                objVasInquiry.dtl_line = i + 1;
                objVasInquiry.itm_num = objVasInquiry.ListVasEntryGridDtl[i].itm_num.Trim();
                objVasInquiry.itm_color = "-";
                objVasInquiry.itm_name = objVasInquiry.ListVasEntryGridDtl[i].itm_name.Trim();
                objVasInquiry.qty = objVasInquiry.ListVasEntryGridDtl[i].qty;
                objVasInquiry.list_price = objVasInquiry.ListVasEntryGridDtl[i].list_price;
                objVasInquiry.amt = objVasInquiry.ListVasEntryGridDtl[i].amt;
                objVasInquiry.Status = "O";
                objVasInquiry.notes = "";
                objVasInquiry.price_uom = objVasInquiry.ListVasEntryGridDtl[i].price_uom.Trim();
                objVasInquiry.cmp_id = cmpid;
                objVasInquiry.ship_doc_id = l_str_vasid;
                objVasInquiry.vas_user_id = l_str_vas_user_id;
                ServiceObject.VasInsert(objVasInquiry);
            }
            objVasInquiry = ServiceObject.GetVasEntryTempGridDtl(objVasInquiry);
            //objVasInquiry = ServiceObject.TruncateTempVasEntry(objVasInquiry);
            Mapper.CreateMap<VasInquiry, VasInquiryModel>();
            VasInquiryModel objVasInquirymodel = Mapper.Map<VasInquiry, VasInquiryModel>(objVasInquiry);
            return PartialView("_VasEntry", objVasInquirymodel);
        }
        [HttpPost]
        public JsonResult LoadSRDetails(string p_str_cmp_id, string p_str_sonum, string p_str_whs_id)//CR-20180514-001 added by nithya
        {
            VasInquiry objVasInquiry = new VasInquiry();
            IVasInquiryService ServiceObject = new VasInquiryService();
            objVasInquiry.cmp_id = p_str_cmp_id;
            objVasInquiry.so_num = p_str_sonum;
            objVasInquiry = ServiceObject.LoadSrDetails(objVasInquiry);
            if (objVasInquiry.ListLoadSrDetails.Count > 0)
            {
                objVasInquiry.whs_id = (objVasInquiry.ListLoadSrDetails[0].fob.Trim() == null || objVasInquiry.ListLoadSrDetails[0].fob.Trim() == "") ? null : objVasInquiry.ListLoadSrDetails[0].fob;
                objVasInquiry.ship_to_id = (objVasInquiry.ListLoadSrDetails[0].shipto_id == null || objVasInquiry.ListLoadSrDetails[0].shipto_id.Trim() == "") ? null : objVasInquiry.ListLoadSrDetails[0].shipto_id;
                objVasInquiry.cust_po_num = (objVasInquiry.ListLoadSrDetails[0].cust_ordr_num == null || objVasInquiry.ListLoadSrDetails[0].cust_ordr_num.Trim() == "") ? null : objVasInquiry.ListLoadSrDetails[0].cust_ordr_num;
                objVasInquiry.notes = (objVasInquiry.ListLoadSrDetails[0].note == null || objVasInquiry.ListLoadSrDetails[0].note.Trim() == "") ? null : objVasInquiry.ListLoadSrDetails[0].note;
            }
            if (objVasInquiry.whs_id == null || objVasInquiry.whs_id == "")
            {
                objVasInquiry.whs_id = p_str_whs_id;
            }
            return Json(new { DATA = objVasInquiry.whs_id, DATA1 = objVasInquiry.ship_to_id, DATA2 = objVasInquiry.cust_po_num, DATA3 = objVasInquiry.notes }, JsonRequestBehavior.AllowGet);//CR-20180514-001 added by nithya

        }
        //public ActionResult SaveVasEntry(string p_str_cmp_id, string p_str_vas_id, string p_str_vas_dt, string p_str_so_num, string p_str_whs_id,
        //    string p_str_ship_to_id, string p_str_cust_po_num, string p_str_cust_po_dt, string p_str_note)
        //{
        //    VasInquiry objVasInquiry = new VasInquiry();
        //    IVasInquiryService ServiceObject = new VasInquiryService();
        //    objVasInquiry.cmp_id = p_str_cmp_id;
        //    objVasInquiry.vas_id = p_str_vas_id;
        //    objVasInquiry.so_num = p_str_so_num;//CR20180719-001 Added by Nithya

        //    int l_dec_lineNum = 0;
        //    decimal lstrRate = 0;
        //    decimal lstrAmt = 0;
        //    bool l_str_flag = false;
        //    objVasInquiry = ServiceObject.GetVasEntryTempGridDtl(objVasInquiry);
        //    objVasInquiry = ServiceObject.GetQtyCount(objVasInquiry);
        //    if(objVasInquiry.ListVasEntryDtl.Count==0)
        //    {
        //        return Json(l_str_flag, JsonRequestBehavior.AllowGet);
        //    }
        //    // ChkAndSave(p_str_cmp_id, p_str_vas_id, ref l_str_flag);                   //CR - 3PL-MVC-VAS-201800825 Added by Nithya
        //    //if (l_str_flag == false)
        //    //{
        //    //    return Json(l_str_flag, JsonRequestBehavior.AllowGet);
        //    //}
        //    else
        //    {
        //        for (int i = 0; i < objVasInquiry.ListVasEntryTempGridDtl.Count(); i++)
        //        {
        //            //lstrRate = objVasInquiry.ListVasEntryTempGridDtl[i].qty;
        //            //lstrAmt = objVasInquiry.ListVasEntryTempGridDtl[i].Amount;
        //            //if (lstrRate == 0)
        //            //{

        //            //    l_str_flag = "false";
        //            //    break;
        //            //}
        //            if (objVasInquiry.ListVasEntryTempGridDtl[i].qty > 0 && objVasInquiry.ListVasEntryTempGridDtl[i].list_price > 0)
        //            {
        //                l_dec_lineNum = l_dec_lineNum + 1;
        //                objVasInquiry.dtl_line = l_dec_lineNum;
        //                objVasInquiry.so_num = p_str_so_num;
        //                objVasInquiry.cmp_id = objVasInquiry.ListVasEntryTempGridDtl[i].cmp_id.Trim();
        //                objVasInquiry.ship_doc_id = objVasInquiry.ListVasEntryTempGridDtl[i].vas_id.Trim();
        //                objVasInquiry.Status = objVasInquiry.ListVasEntryTempGridDtl[i].status.Trim();
        //                objVasInquiry.itm_num = objVasInquiry.ListVasEntryTempGridDtl[i].itm_num.Trim();
        //                objVasInquiry.itm_name = objVasInquiry.ListVasEntryTempGridDtl[i].itm_name.Trim();
        //                objVasInquiry.qty = objVasInquiry.ListVasEntryTempGridDtl[i].qty;
        //                objVasInquiry.list_price = objVasInquiry.ListVasEntryTempGridDtl[i].list_price;
        //                objVasInquiry.price_uom = objVasInquiry.ListVasEntryTempGridDtl[i].price_uom.Trim();
        //                objVasInquiry.Note = objVasInquiry.ListVasEntryTempGridDtl[i].note.Trim();
        //                ServiceObject.SaveVasEntryDtl(objVasInquiry);
        //                //ServiceObject.UpdateVasRateHdr(objVasInquiry);//CR-180421-001
        //            }

        //        }

        //        //objVasInquiry = ServiceObject.TruncateTempVasEntry(objVasInquiry);
        //        objVasInquiry.cmp_id = p_str_cmp_id;
        //        objVasInquiry.ship_doc_id = p_str_vas_id;
        //        objVasInquiry.ship_dt = p_str_vas_dt;
        //        objVasInquiry.Status = "O";
        //        objVasInquiry.so_num = p_str_so_num;
        //        objVasInquiry.whs_id = p_str_whs_id;
        //        objVasInquiry.ship_to_id = p_str_ship_to_id;
        //        objVasInquiry.cust_po_num = p_str_cust_po_num;
        //        objVasInquiry.cust_po_dt = p_str_cust_po_dt;
        //        objVasInquiry.notes = p_str_note;
        //        ServiceObject.SaveVasEntryHdr(objVasInquiry);
        //        ServiceObject.TruncateTempVasEntry(objVasInquiry);
        //        Mapper.CreateMap<VasInquiry, VasInquiryModel>();
        //        VasInquiryModel objVasInquirymodel = Mapper.Map<VasInquiry, VasInquiryModel>(objVasInquiry);
        //        return View("~/Views/VasInquiry/VasInquiry.cshtml", objVasInquirymodel);
        //    }
        //}
        [HttpPost]
        public JsonResult SaveVasEntry(List<VasInquiryDtlNew> ItemDetails, List<VasHdrNew> VasHdr)
        {

            int l_dec_lineNum = 0;
            string p_str_cmp_id = string.Empty;
            string p_str_vas_id = string.Empty;
            string p_str_vas_dt = string.Empty;
            string p_str_so_num = string.Empty;
            string p_str_whs_id = string.Empty;
            string p_str_ship_to_id = string.Empty;
            string p_str_cust_po_num = string.Empty;
            string p_str_cust_po_dt = string.Empty;
            string p_str_note = string.Empty;
            foreach (VasHdrNew Itemhdr in VasHdr)
            {
                p_str_cmp_id = (Itemhdr.cmp_id == null ? string.Empty : Itemhdr.cmp_id);
                p_str_vas_id = (Itemhdr.vas_id == null ? string.Empty : Itemhdr.vas_id);
                p_str_vas_dt = (Itemhdr.vas_dt == null ? string.Empty : Itemhdr.vas_dt);
                p_str_so_num = (Itemhdr.so_num == null ? string.Empty : Itemhdr.so_num);
                p_str_whs_id = (Itemhdr.whs_id == null ? string.Empty : Itemhdr.whs_id);
                p_str_ship_to_id = (Itemhdr.ship_to_id == null ? string.Empty : Itemhdr.ship_to_id);
                p_str_cust_po_num = (Itemhdr.cust_po_num == null ? string.Empty : Itemhdr.cust_po_num);
                p_str_cust_po_dt = (Itemhdr.cust_po_dt == null ? string.Empty : Itemhdr.cust_po_dt);
                p_str_note = (Itemhdr.note == null ? string.Empty : Itemhdr.note);
            }
            if (ItemDetails == null)
            {
                ItemDetails.Clear();
                VasHdr.Clear();
                int l_str_Count = 1;
                return Json(l_str_Count, JsonRequestBehavior.AllowGet);
            }
            foreach (VasInquiryDtlNew Item in ItemDetails)
            {
                l_dec_lineNum = l_dec_lineNum + 1;
                objVasInquiry.dtl_line = l_dec_lineNum;
                objVasInquiry.so_num = p_str_so_num;
                objVasInquiry.cmp_id = p_str_cmp_id;
                objVasInquiry.ship_doc_id = p_str_vas_id;
                objVasInquiry.Status = "O";
                objVasInquiry.itm_num = Item.itm_num;
                objVasInquiry.itm_name = Item.itm_name;
                objVasInquiry.qty = Item.qty;
                objVasInquiry.list_price = Item.list_price;
                objVasInquiry.price_uom = Item.price_uom;
                objVasInquiry.Note = Item.notes;
                ServiceObject.SaveVasEntryDtl(objVasInquiry);
            }
            objVasInquiry.cmp_id = p_str_cmp_id;
            objVasInquiry.ship_doc_id = p_str_vas_id;
            objVasInquiry.ship_dt = p_str_vas_dt;
            objVasInquiry.Status = "O";
            objVasInquiry.so_num = p_str_so_num;
            objVasInquiry.whs_id = p_str_whs_id;
            objVasInquiry.ship_to_id = p_str_ship_to_id;
            objVasInquiry.cust_po_num = p_str_cust_po_num;
            objVasInquiry.cust_po_dt = p_str_cust_po_dt;
            objVasInquiry.notes = p_str_note;
            ServiceObject.SaveVasEntryHdr(objVasInquiry);
            ServiceObject.TruncateTempVasEntry(objVasInquiry);
            ItemDetails.Clear();
            VasHdr.Clear();
            return Json("", JsonRequestBehavior.AllowGet);
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

                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//VasInquiry//" + strReportName;
                    objVasInquiry.cmp_id = p_str_cmp_id;
                    objVasInquiry.ship_doc_id = p_str_vasid;
                    objVasInquiry = ServiceObject.GetVasPostDetails(objVasInquiry);
                    var rptSource = objVasInquiry.ListVasInquiry.ToList();
                    if (rptSource.Count > 0)
                    {
                        using (ReportDocument rd = new ReportDocument())
                        {
                            //CR20180811-001 Added By Nithya
                            objCustMaster.cust_id = p_str_cmp_id;
                            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
                            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
                            {
                                objCustMaster.ListGetCustLogo[0].cust_logo = "";
                            }
                            //END
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objVasInquiry.ListVasInquiry.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            //objVasInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            objVasInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim();
                            rd.SetParameterValue("fml_image_path", objVasInquiry.Image_Path);//CR2018-04-28-001 Added By Nithya
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
        //public ActionResult UpdateVasEntry(string p_str_cmp_id, string p_str_vas_id, string p_str_vas_dt, string p_str_so_num, string p_str_whs_id,
        //    string p_str_ship_to_id, string p_str_cust_po_num, string p_str_cust_po_dt, string p_str_note)
        //{
        //    VasInquiry objVasInquiry = new VasInquiry();
        //    IVasInquiryService ServiceObject = new VasInquiryService();
        //    objVasInquiry.cmp_id = p_str_cmp_id;
        //    objVasInquiry.vas_id = p_str_vas_id;
        //    int l_dec_lineNum = 0;
        //    decimal lstrRate = 0;
        //    decimal lstrAmt = 0;
        //    bool l_str_flag = false;
        //    objVasInquiry = ServiceObject.GetVasEntryTempGridDtl(objVasInquiry);
        //    ServiceObject.DeleteVasedit(objVasInquiry);
        //    // ChkAndSave(p_str_cmp_id, p_str_vas_id, ref l_str_flag); //CR - 3PL-MVC-VAS-201800825 Added by Nithya
        //    //if (l_str_flag == false)
        //    //{
        //    //    return Json(l_str_flag, JsonRequestBehavior.AllowGet);

        //    //}
        //    objVasInquiry = ServiceObject.GetQtyCount(objVasInquiry);
        //    if (objVasInquiry.ListVasEntryDtl.Count == 0)
        //    {
        //        return Json(l_str_flag, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        for (int i = 0; i < objVasInquiry.ListVasEntryTempGridDtl.Count(); i++)
        //        {

        //            if (objVasInquiry.ListVasEntryTempGridDtl[i].qty > 0 && objVasInquiry.ListVasEntryTempGridDtl[i].list_price > 0)
        //            {
        //                l_dec_lineNum = l_dec_lineNum + 1;
        //                objVasInquiry.dtl_line = l_dec_lineNum;
        //                objVasInquiry.so_num = p_str_so_num;
        //                objVasInquiry.cmp_id = objVasInquiry.ListVasEntryTempGridDtl[i].cmp_id.Trim();
        //                objVasInquiry.ship_doc_id = objVasInquiry.ListVasEntryTempGridDtl[i].vas_id.Trim();
        //                objVasInquiry.Status = objVasInquiry.ListVasEntryTempGridDtl[i].status.Trim();
        //                objVasInquiry.itm_num = objVasInquiry.ListVasEntryTempGridDtl[i].itm_num.Trim();
        //                objVasInquiry.itm_name = objVasInquiry.ListVasEntryTempGridDtl[i].itm_name.Trim();
        //                objVasInquiry.qty = objVasInquiry.ListVasEntryTempGridDtl[i].qty;
        //                objVasInquiry.list_price = objVasInquiry.ListVasEntryTempGridDtl[i].list_price;
        //                objVasInquiry.price_uom = objVasInquiry.ListVasEntryTempGridDtl[i].price_uom.Trim();
        //                objVasInquiry.Note = objVasInquiry.ListVasEntryTempGridDtl[i].note.Trim();
        //                ServiceObject.SaveVasEntryDtl(objVasInquiry);
        //                //ServiceObject.UpdateVasRateHdr(objVasInquiry);CR-180421-001
        //            }

        //        }
        //        //objVasInquiry = ServiceObject.TruncateTempVasEntry(objVasInquiry);
        //        objVasInquiry.cmp_id = p_str_cmp_id;
        //        objVasInquiry.ship_doc_id = p_str_vas_id;
        //        objVasInquiry.ship_dt = p_str_vas_dt;
        //        objVasInquiry.Status = "O";
        //        objVasInquiry.so_num = p_str_so_num;
        //        objVasInquiry.fob = p_str_whs_id;
        //        objVasInquiry.ship_to_id = p_str_ship_to_id;
        //        objVasInquiry.cust_po_num = p_str_cust_po_num;
        //        objVasInquiry.cust_po_dt = p_str_cust_po_dt;
        //        objVasInquiry.notes = p_str_note;
        //        ServiceObject.UpdateVasEntryHdr(objVasInquiry);
        //        ServiceObject.TruncateTempVasEntry(objVasInquiry);
        //        Mapper.CreateMap<VasInquiry, VasInquiryModel>();
        //        VasInquiryModel objVasInquirymodel = Mapper.Map<VasInquiry, VasInquiryModel>(objVasInquiry);
        //        return View("~/Views/VasInquiry/VasInquiry.cshtml", objVasInquirymodel);
        //    }
        //}
        [HttpPost]
        public JsonResult UpdateVasEntry(List<VasInquiryDtlNew> ItemDetails, List<VasHdrNew> VasHdr)
        {
            bool l_str_flag = false;
            int l_dec_lineNum = 0;
            int l_int_qty = 0;
            string p_str_cmp_id = string.Empty;
            string p_str_vas_id = string.Empty;
            string p_str_vas_dt = string.Empty;
            string p_str_so_num = string.Empty;
            string p_str_whs_id = string.Empty;
            string p_str_ship_to_id = string.Empty;
            string p_str_cust_po_num = string.Empty;
            string p_str_cust_po_dt = string.Empty;
            string p_str_note = string.Empty;
            foreach (VasHdrNew Itemhdr in VasHdr)
            {
                p_str_cmp_id = (Itemhdr.cmp_id == null ? string.Empty : Itemhdr.cmp_id);
                p_str_vas_id = (Itemhdr.vas_id == null ? string.Empty : Itemhdr.vas_id);
                p_str_vas_dt = (Itemhdr.vas_dt == null ? string.Empty : Itemhdr.vas_dt);
                p_str_so_num = (Itemhdr.so_num == null ? string.Empty : Itemhdr.so_num);
                p_str_whs_id = (Itemhdr.whs_id == null ? string.Empty : Itemhdr.whs_id);
                p_str_ship_to_id = (Itemhdr.ship_to_id == null ? string.Empty : Itemhdr.ship_to_id);
                p_str_cust_po_num = (Itemhdr.cust_po_num == null ? string.Empty : Itemhdr.cust_po_num);
                p_str_cust_po_dt = (Itemhdr.cust_po_dt == null ? string.Empty : Itemhdr.cust_po_dt);
                p_str_note = (Itemhdr.note == null ? string.Empty : Itemhdr.note);
            }
            VasInquiry objVasInquiry = new VasInquiry();
            IVasInquiryService ServiceObject = new VasInquiryService();
            objVasInquiry.cmp_id = p_str_cmp_id;
            objVasInquiry.vas_id = p_str_vas_id;
            ServiceObject.DeleteVasedit(objVasInquiry);
            if (ItemDetails == null)
            {
                ItemDetails.Clear();
                VasHdr.Clear();
                int l_str_Count = 1;
                return Json(l_str_Count, JsonRequestBehavior.AllowGet);
            }
            foreach (VasInquiryDtlNew Item in ItemDetails)
            {
                l_dec_lineNum = l_dec_lineNum + 1;
                objVasInquiry.dtl_line = l_dec_lineNum;
                objVasInquiry.so_num = p_str_so_num;
                objVasInquiry.cmp_id = p_str_cmp_id;
                objVasInquiry.ship_doc_id = p_str_vas_id;
                objVasInquiry.Status = "O";
                objVasInquiry.itm_num = Item.itm_num;
                objVasInquiry.itm_name = Item.itm_name;
                objVasInquiry.qty = Item.qty;
                objVasInquiry.list_price = Item.list_price;
                objVasInquiry.price_uom = Item.price_uom;
                objVasInquiry.Note = Item.notes;
                ServiceObject.SaveVasEntryDtl(objVasInquiry);
            }
            objVasInquiry.cmp_id = p_str_cmp_id;
            objVasInquiry.ship_doc_id = p_str_vas_id;
            objVasInquiry.ship_dt = p_str_vas_dt;
            objVasInquiry.Status = "O";
            objVasInquiry.so_num = p_str_so_num;
            objVasInquiry.whs_id = p_str_whs_id;
            objVasInquiry.ship_to_id = p_str_ship_to_id;
            objVasInquiry.cust_po_num = p_str_cust_po_num;
            objVasInquiry.cust_po_dt = p_str_cust_po_dt;
            objVasInquiry.notes = p_str_note;
            ServiceObject.UpdateVasEntryHdr(objVasInquiry);
            ServiceObject.TruncateTempVasEntry(objVasInquiry);
            ItemDetails.Clear();
            VasHdr.Clear();
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(string VasId, string CmpId, string p_str_scn_id, string p_str_whs_id)
        {
            string l_str_vas_user_id;
            VasInquiry objVasInquiry = new VasInquiry();
            IVasInquiryService ServiceObject = new VasInquiryService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objVasInquiry.vas_id = VasId;
            objVasInquiry.cmp_id = CmpId;
            objVasInquiry.screentitle = p_str_scn_id;
            objVasInquiry = ServiceObject.GetVasEntryhdr(objVasInquiry);
            objVasInquiry.cmp_id = objVasInquiry.ListVasInquiry[0].cmp_id.Trim();
            objVasInquiry.ship_dt = objVasInquiry.ListVasInquiry[0].ship_dt.Trim();
            objVasInquiry.Status = objVasInquiry.ListVasInquiry[0].status.Trim();
            objVasInquiry.so_num = objVasInquiry.ListVasInquiry[0].so_num;
            if (objVasInquiry.ListVasInquiry[0].whs_id == null || objVasInquiry.ListVasInquiry[0].whs_id == "")
            {
                objVasInquiry.whs_id = "";
            }
            else
            {
                objVasInquiry.whs_id = (objVasInquiry.ListVasInquiry[0].whs_id.Trim() == null || objVasInquiry.ListVasInquiry[0].whs_id.Trim() == "") ? "" : objVasInquiry.ListVasInquiry[0].whs_id.Trim();
            }

            objCompany.cust_cmp_id = CmpId;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objVasInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objCompany.whs_id = objVasInquiry.whs_id;
            objCompany = ServiceObjectCompany.GetWhsIdDetails(objCompany);
            objVasInquiry.ListwhsPickDtl = objCompany.ListwhsPickDtl;

            objVasInquiry.ship_to_id = objVasInquiry.ListVasInquiry[0].ship_to;
            objVasInquiry.cust_po_num = objVasInquiry.ListVasInquiry[0].po_num;
            objVasInquiry.cust_po_dt = objVasInquiry.ListVasInquiry[0].ship_dt;
            objVasInquiry.vas_user_id = objVasInquiry.vas_user_id;
            l_str_vas_user_id = objVasInquiry.vas_user_id;
            objVasInquiry.vas_user_id = l_str_vas_user_id;
            if (objVasInquiry.ListVasInquiry[0].notes == null || objVasInquiry.ListVasInquiry[0].notes == "")
            {
                objVasInquiry.Note = "";
            }
            else
            {
                objVasInquiry.Note = objVasInquiry.ListVasInquiry[0].notes.Trim();
            }
            objVasInquiry = ServiceObject.GetVasEntryGrid(objVasInquiry);

            for (int i = 0; i < objVasInquiry.ListVasEntryGridDtl.Count(); i++)
            {

                objVasInquiry.dtl_line = i + 1;
                objVasInquiry.itm_num = objVasInquiry.ListVasEntryGridDtl[i].cust_itm.Trim();
                objVasInquiry.itm_color = "-";
                objVasInquiry.itm_name = objVasInquiry.ListVasEntryGridDtl[i].cust_itm_name;
                objVasInquiry.qty = objVasInquiry.ListVasEntryGridDtl[i].ship_qty;
                objVasInquiry.list_price = objVasInquiry.ListVasEntryGridDtl[i].ship_itm_price;
                objVasInquiry.amt = objVasInquiry.ListVasEntryGridDtl[i].Amt;
                objVasInquiry.status = objVasInquiry.ListVasEntryGridDtl[i].status;
                objVasInquiry.notes = objVasInquiry.ListVasEntryGridDtl[i].notes;
                objVasInquiry.price_uom = objVasInquiry.ListVasEntryGridDtl[i].ship_price_uom;
                objVasInquiry.cmp_id = objVasInquiry.ListVasEntryGridDtl[i].cmp_id;
                objVasInquiry.ship_doc_id = VasId;
                objVasInquiry.vas_user_id = l_str_vas_user_id;
                ServiceObject.VasInsert(objVasInquiry);

            }
            objVasInquiry = ServiceObject.GetVasEntryTempGridDtl(objVasInquiry);
            if (objVasInquiry.screentitle == "VasInquiry")
            {
                objVasInquiry.whs_id = p_str_whs_id;
                //objVasInquiry.ListwhsPickDtl = p_str_whs_id;
            }
            if (objVasInquiry.screentitle == "OBInquiry")
            {
                objVasInquiry.whs_id = objVasInquiry.ListVasInquiry[0].whs_id;
            }
            objVasInquiry.cmp_id = CmpId;
            Mapper.CreateMap<VasInquiry, VasInquiryModel>();
            VasInquiryModel objVasInquirymodel = Mapper.Map<VasInquiry, VasInquiryModel>(objVasInquiry);
            return PartialView("_VasEdit", objVasInquirymodel);
        }

        public ActionResult Delete(string p_str_vas_id, string cmp_id, string p_str_vas_bill_status, string p_str_scn_id)
        {

            VasInquiry objVasInquiry = new VasInquiry();
            IVasInquiryService ServiceObject = new VasInquiryService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objVasInquiry.cmp_id = cmp_id;
            objVasInquiry.vas_id = p_str_vas_id;
            objVasInquiry.vas_bill_status = p_str_vas_bill_status;
            objVasInquiry.screentitle = p_str_scn_id;
            objVasInquiry = ServiceObject.GetVasEntryhdr(objVasInquiry);
            objVasInquiry.ship_dt = objVasInquiry.ListVasInquiry[0].ship_dt.Trim();
            objVasInquiry.Status = objVasInquiry.ListVasInquiry[0].status.Trim();
            objVasInquiry.so_num = objVasInquiry.ListVasInquiry[0].so_num;
            if (objVasInquiry.ListVasInquiry[0].whs_id == null || objVasInquiry.ListVasInquiry[0].whs_id == "")
            {
                objVasInquiry.whs_id = "";
            }
            else
            {
                objVasInquiry.whs_id = (objVasInquiry.ListVasInquiry[0].whs_id.Trim() == null || objVasInquiry.ListVasInquiry[0].whs_id.Trim() == "") ? "" : objVasInquiry.ListVasInquiry[0].whs_id.Trim();

            }
            objVasInquiry.ListwhsPickDtl = objCompany.ListwhsPickDtl;
            objVasInquiry.ship_to_id = objVasInquiry.ListVasInquiry[0].ship_to;
            objVasInquiry.cust_po_num = objVasInquiry.ListVasInquiry[0].po_num;
            objVasInquiry.cust_po_dt = objVasInquiry.ListVasInquiry[0].ship_dt;
            if (objVasInquiry.ListVasInquiry[0].notes == null || objVasInquiry.ListVasInquiry[0].notes == "")
            {
                objVasInquiry.Note = "";
            }
            else
            {
                objVasInquiry.Note = objVasInquiry.ListVasInquiry[0].notes.Trim();
            }
            objVasInquiry = ServiceObject.GetVasEntryGriddtl(objVasInquiry);
            for (int i = 0; i < objVasInquiry.ListVasEntryDtl.Count(); i++)
            {

                objVasInquiry.dtl_line = objVasInquiry.ListVasEntryDtl[i].dtl_line;
                objVasInquiry.itm_num = (objVasInquiry.ListVasEntryDtl[i].cust_itm == null || objVasInquiry.ListVasEntryDtl[i].cust_itm == "" ? string.Empty : objVasInquiry.ListVasEntryDtl[i].cust_itm);
                objVasInquiry.itm_name = (objVasInquiry.ListVasEntryDtl[i].cust_itm_name == null || objVasInquiry.ListVasEntryDtl[i].cust_itm_name == "" ? string.Empty : objVasInquiry.ListVasEntryDtl[i].cust_itm_name);
                objVasInquiry.qty = objVasInquiry.ListVasEntryDtl[i].ship_qty;
                objVasInquiry.list_price = objVasInquiry.ListVasEntryDtl[i].ship_itm_price;
                objVasInquiry.amt = objVasInquiry.ListVasEntryDtl[i].Amt;
                objVasInquiry.Status = (objVasInquiry.ListVasEntryDtl[i].status.Trim() == null || objVasInquiry.ListVasEntryDtl[i].status.Trim() == "" ? string.Empty : objVasInquiry.ListVasEntryDtl[i].status.Trim());
                objVasInquiry.notes = (objVasInquiry.ListVasEntryDtl[i].notes == null || objVasInquiry.ListVasEntryDtl[i].notes == "" ? string.Empty : objVasInquiry.ListVasEntryDtl[i].notes);
                objVasInquiry.price_uom = objVasInquiry.ListVasEntryDtl[i].ship_price_uom;
            }
            objVasInquiry.View_Flag = "D";
            Mapper.CreateMap<VasInquiry, VasInquiryModel>();
            VasInquiryModel objVasInquirymodel = Mapper.Map<VasInquiry, VasInquiryModel>(objVasInquiry);
            return PartialView("_VasDelete", objVasInquirymodel);
        }
        public ActionResult Post(string VasId, string cmp_id, string p_str_scn_id)
        {

            VasInquiry objVasInquiry = new VasInquiry();
            IVasInquiryService ServiceObject = new VasInquiryService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objVasInquiry.cmp_id = cmp_id;
            objVasInquiry.vas_id = VasId;
            objVasInquiry.screentitle = p_str_scn_id;
            objVasInquiry = ServiceObject.GetVasEntryhdr(objVasInquiry);
            objVasInquiry.ship_dt = objVasInquiry.ListVasInquiry[0].ship_dt.Trim();
            objVasInquiry.Status = objVasInquiry.ListVasInquiry[0].status.Trim();
            if (objVasInquiry.ListVasInquiry[0].so_num == null)
            {
                objVasInquiry.so_num = string.Empty;
            }
            else
            {
                objVasInquiry.so_num = objVasInquiry.ListVasInquiry[0].so_num.Trim();
            }
            if (objVasInquiry.ListVasInquiry[0].whs_id == null || objVasInquiry.ListVasInquiry[0].whs_id == "")
            {
                objVasInquiry.whs_id = "";
            }
            else
            {
                objVasInquiry.whs_id = objVasInquiry.ListVasInquiry[0].whs_id.Trim();
            }
            //objVasInquiry.whs_id = (objVasInquiry.ListVasInquiry[0].whs_id.Trim() == null || objVasInquiry.ListVasInquiry[0].whs_id.Trim() == "") ? "" : objVasInquiry.ListVasInquiry[0].whs_id.Trim();           
            objVasInquiry.ListwhsPickDtl = objCompany.ListwhsPickDtl;
            objVasInquiry.ship_to_id = (objVasInquiry.ListVasInquiry[0].ship_to == null || objVasInquiry.ListVasInquiry[0].ship_to == "") ? "" : objVasInquiry.ListVasInquiry[0].ship_to.Trim();
            objVasInquiry.cust_po_num = (objVasInquiry.ListVasInquiry[0].po_num == null || objVasInquiry.ListVasInquiry[0].po_num == "") ? "" : objVasInquiry.ListVasInquiry[0].po_num.Trim();
            objVasInquiry.cust_po_dt = objVasInquiry.ListVasInquiry[0].ship_dt.Trim();
            if (objVasInquiry.ListVasInquiry[0].notes == null || objVasInquiry.ListVasInquiry[0].notes == "")
            {
                objVasInquiry.Note = "";
            }
            else
            {
                objVasInquiry.Note = objVasInquiry.ListVasInquiry[0].notes.Trim();
            }
            objVasInquiry = ServiceObject.GetVasEntryGriddtl(objVasInquiry);
            for (int i = 0; i < objVasInquiry.ListVasEntryDtl.Count(); i++)
            {

                objVasInquiry.dtl_line = objVasInquiry.ListVasEntryDtl[i].dtl_line;
                objVasInquiry.itm_num = objVasInquiry.ListVasEntryDtl[i].cust_itm;
                objVasInquiry.itm_name = objVasInquiry.ListVasEntryDtl[i].cust_itm_name;
                objVasInquiry.qty = objVasInquiry.ListVasEntryDtl[i].ship_qty;
                objVasInquiry.list_price = objVasInquiry.ListVasEntryDtl[i].ship_itm_price;
                objVasInquiry.amt = objVasInquiry.ListVasEntryDtl[i].Amt;
                objVasInquiry.Status = objVasInquiry.ListVasEntryDtl[i].status;
                objVasInquiry.notes = objVasInquiry.ListVasEntryDtl[i].notes;
                objVasInquiry.price_uom = objVasInquiry.ListVasEntryDtl[i].ship_price_uom;
            }
            objVasInquiry.View_Flag = "P";
            Mapper.CreateMap<VasInquiry, VasInquiryModel>();
            VasInquiryModel objVasInquirymodel = Mapper.Map<VasInquiry, VasInquiryModel>(objVasInquiry);
            return PartialView("_VasDelete", objVasInquirymodel);
        }
        public ActionResult UnPost(string p_str_vas_id, string p_str_cmp_id, string p_str_bill_doc_id, string p_str_scn_id)
        {

            VasInquiry objVasInquiry = new VasInquiry();
            IVasInquiryService ServiceObject = new VasInquiryService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objVasInquiry.cmp_id = p_str_cmp_id;
            objVasInquiry.vas_id = p_str_vas_id;
            objVasInquiry.bill_doc_id = p_str_bill_doc_id;
            objVasInquiry.screentitle = p_str_scn_id;
            objVasInquiry = ServiceObject.GetVasEntryhdr(objVasInquiry);
            objVasInquiry.ship_dt = objVasInquiry.ListVasInquiry[0].ship_dt.Trim();
            objVasInquiry.Status = objVasInquiry.ListVasInquiry[0].status.Trim();
            objVasInquiry.so_num = objVasInquiry.ListVasInquiry[0].so_num.Trim();
            if (objVasInquiry.ListVasInquiry[0].whs_id == null || objVasInquiry.ListVasInquiry[0].whs_id == "")
            {
                objVasInquiry.whs_id = "";
            }
            else
            {
                objVasInquiry.whs_id = objVasInquiry.ListVasInquiry[0].whs_id.Trim();
            }
            //objVasInquiry.whs_id = (objVasInquiry.ListVasInquiry[0].whs_id.Trim() == null || objVasInquiry.ListVasInquiry[0].whs_id.Trim() == "") ? "" : objVasInquiry.ListVasInquiry[0].whs_id.Trim();           
            objVasInquiry.ListwhsPickDtl = objCompany.ListwhsPickDtl;
            objVasInquiry.ship_to_id = objVasInquiry.ListVasInquiry[0].ship_to;
            objVasInquiry.cust_po_num = objVasInquiry.ListVasInquiry[0].po_num;
            objVasInquiry.cust_po_dt = objVasInquiry.ListVasInquiry[0].ship_dt;
            if (objVasInquiry.ListVasInquiry[0].notes == null || objVasInquiry.ListVasInquiry[0].notes == "")
            {
                objVasInquiry.Note = "";
            }
            else
            {
                objVasInquiry.Note = objVasInquiry.ListVasInquiry[0].notes;
            }
            objVasInquiry = ServiceObject.GetVasEntryGriddtl(objVasInquiry);
            for (int i = 0; i < objVasInquiry.ListVasEntryDtl.Count(); i++)
            {

                objVasInquiry.dtl_line = objVasInquiry.ListVasEntryDtl[i].dtl_line;
                objVasInquiry.itm_num = objVasInquiry.ListVasEntryDtl[i].cust_itm;
                objVasInquiry.itm_name = objVasInquiry.ListVasEntryDtl[i].cust_itm_name;
                objVasInquiry.qty = objVasInquiry.ListVasEntryDtl[i].ship_qty;
                objVasInquiry.list_price = objVasInquiry.ListVasEntryDtl[i].ship_itm_price;
                objVasInquiry.amt = objVasInquiry.ListVasEntryDtl[i].Amt;
                objVasInquiry.Status = objVasInquiry.ListVasEntryDtl[i].status;
                objVasInquiry.notes = objVasInquiry.ListVasEntryDtl[i].notes;
                objVasInquiry.price_uom = objVasInquiry.ListVasEntryDtl[i].ship_price_uom;
            }
            objVasInquiry.View_Flag = "UP";
            Mapper.CreateMap<VasInquiry, VasInquiryModel>();
            VasInquiryModel objVasInquirymodel = Mapper.Map<VasInquiry, VasInquiryModel>(objVasInquiry);
            return PartialView("_VasDelete", objVasInquirymodel);
        }
        public ActionResult VasDelete(string p_str_cmp_id, string p_str_vasid)
        {
            VasInquiry objVasInquiry = new VasInquiry();
            IVasInquiryService ServiceObject = new VasInquiryService();
            objVasInquiry.cmp_id = p_str_cmp_id;
            objVasInquiry.vas_id = p_str_vasid;
            ServiceObject.DeleteVasEntry(objVasInquiry);
            Mapper.CreateMap<VasInquiry, VasInquiryModel>();
            VasInquiryModel objVasInquirymodel = Mapper.Map<VasInquiry, VasInquiryModel>(objVasInquiry);
            int resultcount;
            resultcount = 1;
            return Json(resultcount, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VasPost(string p_str_cmp_id, string p_str_vasid)
        {
            VasInquiry objVasInquiry = new VasInquiry();
            IVasInquiryService ServiceObject = new VasInquiryService();
            objVasInquiry.cmp_id = p_str_cmp_id;
            objVasInquiry.vas_id = p_str_vasid;
            ServiceObject.GetVasPost(objVasInquiry);
            Mapper.CreateMap<VasInquiry, VasInquiryModel>();
            VasInquiryModel objVasInquirymodel = Mapper.Map<VasInquiry, VasInquiryModel>(objVasInquiry);
            return View("~/Views/VasInquiry/VasInquiry.cshtml", objVasInquirymodel);
        }
        public ActionResult VasUnPost(string p_str_cmp_id, string p_str_vasid, string p_str_bill_doc_id)
        {
            VasInquiry objVasInquiry = new VasInquiry();
            IVasInquiryService ServiceObject = new VasInquiryService();
            objVasInquiry.cmp_id = p_str_cmp_id;
            objVasInquiry.vas_id = p_str_vasid;
            objVasInquiry.bill_doc_id = p_str_bill_doc_id;
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cust_of_cmp_id = "";
            objCompany.cmp_id = p_str_cmp_id;
            objCompany = ServiceObjectCompany.GetCustOfCompName(objCompany);
            objVasInquiry.LstCustOfCmpName = objCompany.LstCustOfCmpName;
            objVasInquiry.cust_of_cmpid = objVasInquiry.LstCustOfCmpName[0].cust_of_cmpid;
            ServiceObject.GetVasUnPost(objVasInquiry);
            Mapper.CreateMap<VasInquiry, VasInquiryModel>();
            VasInquiryModel objVasInquirymodel = Mapper.Map<VasInquiry, VasInquiryModel>(objVasInquiry);
            return View("~/Views/VasInquiry/VasInquiry.cshtml", objVasInquirymodel);
        }
        public JsonResult UpdRateTempDetails(int p_str_qty, decimal p_str_rate, decimal p_str_amt, string p_str_itmnum, string p_str_note)
        {
            VasInquiry objVasInquiry = new VasInquiry();
            IVasInquiryService ServiceObject = new VasInquiryService();
            objVasInquiry.itm_num = p_str_itmnum;
            if (p_str_qty == 0)
            {
                objVasInquiry.qty = 0;
            }
            else
            {
                objVasInquiry.qty = p_str_qty;
            }

            objVasInquiry.list_price = p_str_rate;
            objVasInquiry.amt = p_str_amt;
            objVasInquiry.note = p_str_note;

            if (p_str_note == "")
            {
                objVasInquiry.note = "-";
            }
            else
            {
                objVasInquiry.note = p_str_note;
            }
            objVasInquiry = ServiceObject.GetUpdateTempVasEntryDtl(objVasInquiry);

            Mapper.CreateMap<VasInquiry, VasInquiryModel>();
            VasInquiryModel objVasInquirymodel = Mapper.Map<VasInquiry, VasInquiryModel>(objVasInquiry);
            //return PartialView("_VasEntry", objVasInquirymodel);
            return Json(JsonRequestBehavior.AllowGet);
        }
        //CR - 3PL-MVC-VAS-20180405 Added by Soniya
        public JsonResult ReUpdateTempVasDetails(int p_str_qty, decimal p_str_rate, decimal p_str_amt, string p_str_itmnum, string p_str_note, int p_str_RowNo, string VasId)
        {
            VasInquiry objVasInquiry = new VasInquiry();
            IVasInquiryService ServiceObject = new VasInquiryService();
            objVasInquiry.itm_num = p_str_itmnum;
            objVasInquiry.qty = p_str_qty;
            objVasInquiry.list_price = p_str_rate;
            objVasInquiry.amt = p_str_amt;
            //objVasInquiry.note = p_str_note;
            objVasInquiry.note = p_str_note;
            objVasInquiry.line_num = p_str_RowNo;
            objVasInquiry.vas_id = VasId;
            objVasInquiry = ServiceObject.GetUpdateTempVasEntryDtl(objVasInquiry);//Added
                                                                                  //objVasInquiry = ServiceObject.GetReUpdateTempVasEntryDtl(objVasInquiry);//Commented

            //Mapper.CreateMap<VasInquiry, VasInquiryModel>();
            //VasInquiryModel objVasInquirymodel = Mapper.Map<VasInquiry, VasInquiryModel>(objVasInquiry);
            //return PartialView("_VasEntry", objVasInquirymodel);
            return Json("", JsonRequestBehavior.AllowGet);
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
            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();

            try
            {
                l_str_rpt_selection = p_str_rpt_status;
                l_str_rpt_bill_doc_type = p_str_bill_doc_type.Trim();

                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                objBillingInquiry = ServiceObject.GetBillingBillingType(objBillingInquiry);
                l_str_rpt_bill_type = objBillingInquiry.ListBillingType[0].bill_type;
                objBillingInquiry = ServiceObject.GetBillingInoutType(objBillingInquiry);
                l_str_rpt_bill_inout_type = objBillingInquiry.ListBillingInoutType[0].bill_inout_type;
                l_str_rpt_instrg_req = objBillingInquiry.ListBillingType[0].init_strg_rt_req;



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
                        //strReportName = "rpt_va_bill_doc_FH_NJ.rpt";
                        if (objCompany.cmp_id.Trim() == "SJOE")
                        {
                            strReportName = "rpt_va_bill_doc_FH_NJ.rpt";

                        }
                        else
                        {
                            strReportName = "rpt_va_bill_doc.rpt";

                        }
                    }
                    else
                    {
                        strReportName = "rpt_va_bill_doc.rpt";
                    }

                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                    objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                    objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
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
                            if (l_str_cmp_id == "FHNJ")
                            {
                                if (objCompany.cmp_id.Trim() == "SJOE")
                                {

                                }
                                else
                                {
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                                }
                            }
                            else
                            {
                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                            }
                            rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                            rd.SetParameterValue("fml_rpt_bill_title", "(VAS BILL)");
                            rd.SetParameterValue("fml_rpt_param_bill_num", "Bill#");
                            rd.SetParameterValue("fml_rpt_param_bill_date", "Bill Dt");
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
        public ActionResult EmailShowVASReport(string p_str_cmp_id, string p_str_bill_doc_id, string p_str_bill_doc_type, string p_str_rpt_status)
        {

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
            objCompany.cmp_id = p_str_cmp_id.ToString().Trim();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            objCompany.LstCmpName = objCompany.LstCmpName;
            l_str_tmp_name = objCompany.LstCmpName[0].cmp_name.ToString().Trim();
            objEmail.CmpId = p_str_cmp_id;
            objEmail.screenId = ScreenID;
            objEmail.username = objCompany.user_id;
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
                        //strReportName = "rpt_va_bill_doc_FH_NJ.rpt";
                        if (objCompany.cmp_id.Trim() == "SJOE")
                        {
                            strReportName = "rpt_va_bill_doc_FH_NJ.rpt";

                        }
                        else
                        {
                            strReportName = "rpt_va_bill_doc.rpt";

                        }
                    }
                    else
                    {
                        strReportName = "rpt_va_bill_doc.rpt";
                    }

                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                    objEmail.Reportselection = l_str_rpt_bill_doc_type;
                    objEmail = objEmailService.GetSendMailDetails(objEmail);
                    if (objEmail.ListEamilDetail.Count != 0)
                    {
                        objEmail.EmailMessageContent = (objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == null || objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailMessageContent.Trim();
                    }
                    else
                    {
                        objEmail.EmailMessageContent = "";
                    }
                    objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                    objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                    objBillingInquiry = ServiceObject.GetBillingBillDocVASRpt(objBillingInquiry);
                    var rptSource = objBillingInquiry.ListBillingDocVASRpt.ToList();
                    if (rptSource.Count > 0)
                    {
                        using (ReportDocument rd = new ReportDocument())
                        {
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objBillingInquiry.ListBillingDocVASRpt.Count();
                            objBillingInquiry = ServiceObject.GetBillingBillamountInoutCartonRpt(objBillingInquiry);
                            objBillingInquiry.bill_amt = objBillingInquiry.ListBillingamountInoutCartonRpt[0].bill_amt;
                            objBillingInquiry.Bill_doc_dt_Fr = objBillingInquiry.ListBillingDocVASRpt[0].ship_dt.ToShortDateString();
                            objBillingInquiry.bill_doc_id = objBillingInquiry.ListBillingDocVASRpt[0].bill_doc_id;
                            objBillingInquiry.ship_doc_id = objBillingInquiry.ListBillingDocVASRpt[0].ship_doc_id;
                            l_str_rptdtl = objBillingInquiry.cmp_id + "_" + "VAS BILL" + "_" + objBillingInquiry.bill_doc_id + "_" + objBillingInquiry.bill_doc_id;
                            objEmail.EmailSubject = objBillingInquiry.cmp_id + "-" + "VAS BILL " + "|" + " " + "Invoice#: " + objBillingInquiry.bill_doc_id + "|" + " " + "Invoice:  " + "$" + objBillingInquiry.bill_amt + "|" + " " + "VAS ID#: " + objBillingInquiry.ship_doc_id + "|" + " " + "VAS Date: " + objBillingInquiry.Bill_doc_dt_Fr;
                            objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Invoice#: " + " " + " " + objBillingInquiry.bill_doc_id + "\n" + "Invoice: " + "$" + objBillingInquiry.bill_amt + "\n" + "VAS ID#: " + " " + " " + objBillingInquiry.ship_doc_id + "\n" + "VAS Date: " + " " + " " + objBillingInquiry.Bill_doc_dt_Fr;
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            if (l_str_cmp_id == "FHNJ")
                            {
                                if (objCompany.cmp_id.Trim() == "SJOE")
                                {

                                }
                                else
                                {
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                                }
                            }
                            else
                            {
                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                            }
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                            // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                        }
                    }
                    reportFileName = l_str_rptdtl + "_" + strDateFormat + ".pdf";//CR2018-03-15-001 Added By Soniya
                    Session["RptFileName"] = strFileName;
                }
                objEmail.CmpId = p_str_cmp_id;
                objEmail.screenId = ScreenID;
                objEmail.username = objCompany.user_id;
                objEmail.Reportselection = l_str_rpt_bill_doc_type;
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

            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShowvasdtlExcelReport(string p_str_cmpid, string p_str_status, string p_str_Shipping_id)
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
                    VasInquiry objVasInquiry = new VasInquiry();
                    IVasInquiryService ServiceObject = new VasInquiryService();
                    objVasInquiry.cmp_id = p_str_cmpid;
                    objVasInquiry.ship_doc_id = p_str_Shipping_id;
                    objVasInquiry = ServiceObject.GetVasPostDetails(objVasInquiry);
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
        //CR - 3PL-MVC-VAS-20180505 Added by Soniya

        private void ChkAndSave(string p_str_cmp_id, string p_str_vas_id, ref bool l_str_flag)
        {
            VasInquiry objVasInquiry = new VasInquiry();
            IVasInquiryService ServiceObject = new VasInquiryService();
            objVasInquiry.cmp_id = p_str_cmp_id;
            objVasInquiry.vas_id = p_str_vas_id;
            decimal lstrRate = 0;
            decimal lstrAmt = 0;
            objVasInquiry = ServiceObject.GetVasEntryTempGridDtl(objVasInquiry);
            for (int m = 0; m < objVasInquiry.ListVasEntryTempGridDtl.Count(); m++)
            {
                lstrRate = objVasInquiry.ListVasEntryTempGridDtl[m].qty;
                lstrAmt = objVasInquiry.ListVasEntryTempGridDtl[m].Amount;
                if (lstrRate != 0)
                {
                    l_str_flag = true;
                    break;
                }
                l_str_flag = false;
            }
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
                                    objVasInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objVasInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Vas Post Report");
                                }
                            }
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


        public ActionResult VASUploadDocs(string cmpid, string path)

        {

            string docPath = path;
            string paths = path;
            DirectoryInfo dir = new System.IO.DirectoryInfo(docPath);
            return Json(paths, JsonRequestBehavior.AllowGet);

        }
        //CR-20180719-001 Added By Nithya
        public ActionResult GetDocumentUploadCancel(string CompID, string pstrVASDocId, string comment)
        {
            string name = string.Empty;
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            objInboundInquiry.cmp_id = CompID;
            objInboundInquiry.doctype = "VAS";
            objInboundInquiry.Uploadby = Session["UserID"].ToString().Trim();
            objInboundInquiry.Comments = comment;
            string path = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
            string directoryPath = Path.Combine((path), CompID);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(Path.Combine(directoryPath));
            }
            directoryPath = Path.Combine(directoryPath, "VAS");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(Path.Combine(directoryPath));
            }
            directoryPath = Path.Combine(directoryPath, pstrVASDocId);
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
            return PartialView("_VASInquiryImportFile", InboundInquiryDocModel);
        }
        public ActionResult VASDocDelete(string cmp_id, string Filename, string Filepath, string uplddt, string pstrVASDocId)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            objInboundInquiry.cmp_id = cmp_id;
            objInboundInquiry.file_name = Filename;
            objInboundInquiry.file_path = Filepath;
            objInboundInquiry.docUploaddt = uplddt;
            objInboundInquiry.ibdocid = pstrVASDocId;
            objInboundInquiry = objService.Getuploaddelete(objInboundInquiry);
            string path = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
            string directoryPath = Path.Combine((path), cmp_id);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(Path.Combine(directoryPath));
            }
            directoryPath = Path.Combine(directoryPath, "VAS");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(Path.Combine(directoryPath));
            }
            directoryPath = Path.Combine(directoryPath, pstrVASDocId);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(Path.Combine(directoryPath));
            }
            directoryPath = Path.Combine(directoryPath, Filename);
            System.IO.File.Delete(directoryPath);
            DirectoryInfo dir = new DirectoryInfo(directoryPath);
            objInboundInquiry.doctype = "VAS";
           objInboundInquiry = objService.GetTempFiledtl(objInboundInquiry);
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_DocumentUploadGridVAS", InboundInquiryModel);
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

        public FileStreamResult ViewDocument(string path, string ext, string filename)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            if (ext == "pdf" || ext == "PDF")
            {
                return File(fs, "application/pdf");
            }
            if (ext == "doc" || ext == "DOC")
            {
                return File(fs, "application/doc", filename);
            }
            if (ext == "xls" || ext == "XLS")
            {
                return File(fs, "application/xls", filename);
            }
            if (ext == "xlsx" || ext == "XLSX")
            {
                return File(fs, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
            }
            if (ext == "docx" || ext == "DOCX")
            {
                return File(fs, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", filename);
            }

            if (ext == "jpg" || ext == "jpeg")
            {
                return File(fs, "image/jpeg", filename);
            }
            return File(fs, "application/csv", filename);



        }

        public ActionResult GetDocumentFullFileName(string cmpid, string path)

        {
            try
            {
                string l_str_config_doc_path = string.Empty;
                string l_str_full_file_path = string.Empty;
                l_str_config_doc_path = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
                l_str_full_file_path = Path.Combine((l_str_config_doc_path), path);
                return Json(l_str_full_file_path, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }


        }

    }
}