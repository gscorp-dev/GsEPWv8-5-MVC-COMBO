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
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace GsEPWv8_4_MVC.Controllers
{

    #region Change History
    // CR#                         Modified By   Date         Description
    //CR_3PL_MVC_BL_2018_0210_002  RAVIKUMAR     201-0213     TO FIX THE DYNAMIC CONTENT FAILED FOR EMAIL
    //CR-3PL_MVC_IB_2018_0219_008 - MEERA        201-0219     Display company name instead of Comp Id in all the Reports
    //CR-3PL_MVC_IB_2018_0219_006 - NITHYA       201-0219     INOUT AND STORAGE RATE NOT LOADED IN THE COMBO BOX
    //CR_3PL_MVC_BL_2018_0221_001   RAVIKUMAR     201-0221     TO FIX THE IUT FINDINGS IN RECEIVING UNPOST
    //CR-3PL_MVC_IB_2018_0219_004 - MEERA        2018-02-20   Add a new column Bill and it should be visible once the Receiving is posted. By clicking the Bill link system should generate In&Out bill for the specific IB DOC ID and the status of the Bill column should be changed as Bill Posted 
    // CR_3PL_MVC_IB_2018_0223_001 - SONIYA        2018-02-23   Report for Inout type as 'Container'
    // CR_3PL_MVC_IB_2018_0227_001 - Modified by Soniya for set default from date before one year  in filter section
    //CR_3PL_MVC_BL_2018_0303_001 - MEERA 03-03-2018 Add Container Notes .
    //CR_3PL_MVC_BL_2018_0305_001 - MEERA 03-05-2018 Add Item Code Related Changes.
    //CR-3PL_MVC_IB_2018_0312_001 - MEERA 03-12-2018 Add .
    //CR_3PL_MVC_BL_2018_0315_001 Added By MEERA 03-03-2018 Container Related Change
    //CR 03-24-2018-001 added by Nithya For Edit Flag
    // CR_3PL_MVC_IB_DOC_RECV_2018_0324_001 Added By MEERA 03-24-2018 PPK is not loaded 
    //CR180328-001 Added by Nithya For Dou u want to receive SAme PPk,style.po_num in Doc Entry Screen
    #endregion Change History

    public class InboundInquiryController : Controller
    {
        private string storageRate = "";
        private string inoutRate = "";
        public string EmailSub = string.Empty;
        public string EmailMsg = string.Empty;
        public string Folderpath = string.Empty;
        public string ScreenID = "Inbound Inquiry";

        StringBuilder myBuilder = new StringBuilder();
        public List<string> Body = new List<string>();
        Email objEmail = new Email();
        EmailService objEmailService = new EmailService();
        public ActionResult InboundInquiry(string FullFillType, string cmp, string status, string DateFm, string DateTo, string type, string screentitle) //CR_MVC_3PL_0317-001 Added By Nithya
        {
            string l_str_cmp_id = string.Empty;
            string l_str_status = string.Empty;
            string l_str_fm_dt = string.Empty;
            string l_str_Dflt_Dt_Reqd = string.Empty;
            string l_str_is_3rd_usr = string.Empty;


            try
            {

                InboundInquiry objInboundInquiry = new InboundInquiry();
                InboundInquiryService ServiceObject = new InboundInquiryService();
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                Session["g_str_Search_flag"] = "False";
                objInboundInquiry.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                l_str_is_3rd_usr = Session["IS3RDUSER"].ToString();
                objInboundInquiry.IS3RDUSER = l_str_is_3rd_usr.Trim();
                if (objInboundInquiry.cmp_id == null || objInboundInquiry.cmp_id == string.Empty)
                {
                    objInboundInquiry.cmp_id = Session["g_str_cmp_id"].ToString().Trim();

                }
                else
                {
                    objCompany.cmp_id = Session["g_str_cmp_id"].ToString().Trim();

                }
                objInboundInquiry.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                Session["screentitle"] = 0;
                objInboundInquiry.screentitle = screentitle;//CR_MVC_3PL_0317-001 Added By Nithya
                Session["screentitle"] = objInboundInquiry.screentitle;//CR_MVC_3PL_0317-001 Added By Nithya
                Session["l_bool_edit_flag"] = false;
                objInboundInquiry.l_bool_edit_flag = Session["l_bool_edit_flag"].ToString();
                //  CR_3PL_MVC_COMMON_2018_0324_001
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
                    objInboundInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    //DateTime date = DateTime.Now.AddDays(-365);
                    //l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");      // CR_3PL_MVC_IB_2018_0227_001 - Modified by Soniya
                    objInboundInquiry.ib_doc_dt_fm = DateTime.Now.AddDays(Common.clsGlobal.DispDateFrom).ToString("MM/dd/yyyy");
                    objInboundInquiry.ib_doc_dt_to = DateTime.Now.ToString("MM/dd/yyyy");

                    objInboundInquiry.status = "ALL";
                    LookUp objLookUp = new LookUp();
                    LookUpService ServiceObject1 = new LookUpService();
                    objLookUp.id = "1";
                    objLookUp.lookuptype = "DOCINQUIRY";
                    objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                    objInboundInquiry.ListLookUpDtl = objLookUp.ListLookUpDtl;
                    objInboundInquiry.cmp_id = cmp.Trim();
                    objInboundInquiry.cntr_id = "";
                    objInboundInquiry.status = "";
                    objInboundInquiry.ib_doc_id = DateFm;
                    objInboundInquiry.ib_doc_id_to = DateFm;
                    objInboundInquiry.req_num = "";
                    //objInboundInquiry.ib_doc_dt_fm = "";
                    //objInboundInquiry.ib_doc_dt_to = "";
                    objInboundInquiry.eta_dt_fm = "";
                    objInboundInquiry.eta_dt_to = "";
                    objInboundInquiry.screentitle = screentitle;
                    objInboundInquiry = ServiceObject.GetInboundInquiryDetails(objInboundInquiry);

                }

                else if (FullFillType != null)
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objInboundInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    objInboundInquiry.cmp_id = cmp.Trim();
                    objInboundInquiry.cntr_id = "";
                    objInboundInquiry.status = "";
                    objInboundInquiry.ib_doc_id = "";
                    objInboundInquiry.ib_doc_id_to = "";
                    objInboundInquiry.req_num = "";
                    objInboundInquiry.ib_doc_dt_fm = DateTime.Now.AddDays(Common.clsGlobal.DispDateFrom).ToString("MM/dd/yyyy");
                    objInboundInquiry.ib_doc_dt_to = DateTime.Now.ToString("MM/dd/yyyy");
                    //objInboundInquiry.ib_doc_dt_fm = DateFm;
                    //objInboundInquiry.ib_doc_dt_to = DateTo;
                    objInboundInquiry.eta_dt_fm = "";
                    objInboundInquiry.eta_dt_to = "";
                    objInboundInquiry.screentitle = screentitle;//CR_MVC_3PL_0317-001 Added By Nithya
                    if (status == "OPEN")
                    {
                        objInboundInquiry.status = "OPEN";
                        LookUp objLookUp = new LookUp();
                        LookUpService ServiceObject1 = new LookUpService();
                        objLookUp.id = "1";
                        objLookUp.lookuptype = "DOCOPEN";
                        objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                        objInboundInquiry.ListLookUpDtl = objLookUp.ListLookUpDtl;
                    }
                    if (status == "POST")
                    {
                        objInboundInquiry.status = "POST";
                        LookUp objLookUp = new LookUp();
                        LookUpService ServiceObject1 = new LookUpService();
                        objLookUp.id = "1";
                        objLookUp.lookuptype = "DOCPOST";
                        objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                        objInboundInquiry.ListLookUpDtl = objLookUp.ListLookUpDtl;
                    }
                    if (status == "ALL")
                    {
                        objInboundInquiry.status = "ALL";
                        LookUp objLookUp = new LookUp();
                        LookUpService ServiceObject1 = new LookUpService();
                        objLookUp.id = "1";
                        objLookUp.lookuptype = "DOCINQUIRY";
                        objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                        objInboundInquiry.ListLookUpDtl = objLookUp.ListLookUpDtl;
                    }
                    else if (status == "1-RCVD")
                    {
                        objInboundInquiry.status = "1-RCVD";
                        LookUp objLookUp = new LookUp();
                        LookUpService ServiceObject1 = new LookUpService();
                        objLookUp.id = "1";
                        objLookUp.lookuptype = "DOCRCVD";
                        objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                        objInboundInquiry.ListLookUpDtl = objLookUp.ListLookUpDtl;
                    }
                    if (l_str_Dflt_Dt_Reqd == "Y")
                    {
                        objInboundInquiry = ServiceObject.GetInboundInquiryDetails(objInboundInquiry);
                    }

                    objCompany = ServiceObjectCompany.GetFullFillCompanyDetails(objCompany);
                    objInboundInquiry.DateFrom = "DashBoard";


                }
                Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
                InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
                return View(InboundInquiryModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ActionResult RecevLoadDelete(string p_str_CmpId, string p_str_ib_doc_id, string p_str_cntr_id, string datefrom, string dateto)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objInboundInquiry.DocumentdateFrom = datefrom;
            objInboundInquiry.DocumentdateTo = dateto;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objInboundInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objInboundInquiry.cmp_id = p_str_CmpId;
            objInboundInquiry.ib_doc_id = p_str_ib_doc_id;
            objInboundInquiry.cntr_id = p_str_cntr_id;
            objInboundInquiry = objService.GetLoadReceivingdelete(objInboundInquiry);

            objInboundInquiry.View_Flag = "D";

            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_ReceivingDelete", InboundInquiryModel);
        }
        public ActionResult ReceivingDelete(string p_str_cmp_id, string p_str_ib_doc_id)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.ib_doc_id = p_str_ib_doc_id;
            objInboundInquiry = objService.GetReceivingdelete(objInboundInquiry);
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            int resultcount;
            resultcount = 1;
            return Json(resultcount, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(string CmpId, string id, string datefrom, string dateto)
        {

            string l_str_ib_doc_dt = string.Empty;
            string l_str_orderdt = string.Empty;
            string l_str_eta_dt = string.Empty;

            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objInboundInquiry.cmp_id = CmpId;
            objInboundInquiry.ib_doc_id = id;
            objInboundInquiry.DocumentdateFrom = datefrom; //CR 2202018_01 Added by murugan
            objInboundInquiry.DocumentdateTo = dateto; //CR 2202018_01 Added by murugan
            objInboundInquiry = objService.GetDocHdr(objInboundInquiry);
            objInboundInquiry.cmp_id = objInboundInquiry.ListDocHdr[0].cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objInboundInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;

            l_str_ib_doc_dt = Convert.ToDateTime(objInboundInquiry.ListDocHdr[0].ib_doc_dt).ToString("MM/dd/yyyy");
            l_str_orderdt = Convert.ToDateTime(objInboundInquiry.ListDocHdr[0].ordr_dt).ToString("MM/dd/yyyy");
            l_str_eta_dt = Convert.ToDateTime(objInboundInquiry.ListDocHdr[0].eta_dt).ToString("MM/dd/yyyy");


            objInboundInquiry.ib_doc_id = objInboundInquiry.ListDocHdr[0].ib_doc_id;
            objInboundInquiry.ib_doc_dt = l_str_ib_doc_dt;
            objInboundInquiry.status = objInboundInquiry.ListDocHdr[0].status;
            objInboundInquiry.cont_id = objInboundInquiry.ListDocHdr[0].cntr_id;
            objInboundInquiry.eta_dt = l_str_eta_dt;
            objInboundInquiry.refno = objInboundInquiry.ListDocHdr[0].req_num;
            objInboundInquiry.orderdt = l_str_orderdt;
            objInboundInquiry.recvdvia = objInboundInquiry.ListDocHdr[0].shipvia_id;
            objInboundInquiry.recvd_fm = objInboundInquiry.ListDocHdr[0].vend_name;
            objInboundInquiry.bol = objInboundInquiry.ListDocHdr[0].master_bol;
            objInboundInquiry.vessel_num = objInboundInquiry.ListDocHdr[0].vessel_no;
            objInboundInquiry.Note = objInboundInquiry.ListDocHdr[0].Note;
            objInboundInquiry = objService.GetDocDtl(objInboundInquiry);
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_DocEntryDel", InboundInquiryModel);
        }
        public ActionResult DocEdit(string CmpId, string id, string datefrom, string dateto)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            string l_str_ib_doc_dt = string.Empty;
            string l_str_orderdt = string.Empty;
            string l_str_rate = string.Empty;
            int l_int_line_count = 0;
            objInboundInquiry.DocumentdateFrom = datefrom; //CR 2202018_01 Added by murugan
            objInboundInquiry.DocumentdateTo = dateto; //CR 2202018_01 Added by murugan
            string l_str_eta_dt = string.Empty;
            InboundInquiryService objService = new InboundInquiryService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objInboundInquiry.cmp_id = CmpId;
            objInboundInquiry.ib_doc_id = id;
            objInboundInquiry.scn_id = "INBOUND";
            objInboundInquiry.status = "OPEN";
            objInboundInquiry.opt = "A";
            objInboundInquiry.user_id = Session["UserID"].ToString().Trim();
            objInboundInquiry = objService.GetdocEditCount(objInboundInquiry);
            if (objInboundInquiry.LstItmExist.Count > 0)
            {
                objInboundInquiry.editmode = objInboundInquiry.LstItmExist[0].editmode;
            }
            if (objInboundInquiry.editmode != 0)
            {
                int RowCount = 1;
                return Json(RowCount, JsonRequestBehavior.AllowGet);
            }
            objInboundInquiry = objService.GetDeleteTempData(objInboundInquiry);
            objInboundInquiry.ib_doc_dt_fm = datefrom;
            objInboundInquiry.ib_doc_dt_to = dateto;
            objInboundInquiry.cmp_id = CmpId;
            objInboundInquiry.ib_doc_id = id;
            objInboundInquiry.status = "OPEN";
            objInboundInquiry.scn_id = "INBOUND";
            objInboundInquiry.lock_dt = DateTime.Now;
            objInboundInquiry.lock_by = Session["UserID"].ToString().Trim();
            objCompany.cust_cmp_id = CmpId;
            objInboundInquiry = objService.InsertdocEditEntry(objInboundInquiry);
            objInboundInquiry = objService.GetDocHdr(objInboundInquiry);
            objInboundInquiry.cmp_id = objInboundInquiry.ListDocHdr[0].cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objInboundInquiry.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objInboundInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            l_str_ib_doc_dt = Convert.ToDateTime(objInboundInquiry.ListDocHdr[0].ib_doc_dt).ToString("MM/dd/yyyy");
            l_str_orderdt = Convert.ToDateTime(objInboundInquiry.ListDocHdr[0].ordr_dt).ToString("MM/dd/yyyy");
            l_str_eta_dt = Convert.ToDateTime(objInboundInquiry.ListDocHdr[0].eta_dt).ToString("MM/dd/yyyy");
            if (l_str_eta_dt == "01/01/0001")//CR20180511
            {
                l_str_eta_dt = "";
            }
            objInboundInquiry.ib_doc_id = objInboundInquiry.ListDocHdr[0].ib_doc_id;
            objInboundInquiry.ib_doc_dt = l_str_ib_doc_dt;
            objInboundInquiry.status = objInboundInquiry.ListDocHdr[0].status;
            objInboundInquiry.cont_id = objInboundInquiry.ListDocHdr[0].cntr_id;
            objInboundInquiry.eta_dt = l_str_eta_dt;
            objInboundInquiry.refno = objInboundInquiry.ListDocHdr[0].req_num;
            objInboundInquiry.orderdt = l_str_orderdt;
            objInboundInquiry.recvdvia = objInboundInquiry.ListDocHdr[0].shipvia_id;
            objInboundInquiry.recvd_fm = objInboundInquiry.ListDocHdr[0].vend_name;
            objInboundInquiry.bol = objInboundInquiry.ListDocHdr[0].master_bol;
            objInboundInquiry.vessel_num = objInboundInquiry.ListDocHdr[0].vessel_no;
            objCompany = ServiceObjectCompany.GetLocIdDetails(objCompany);
            objInboundInquiry.ListLocPickDtl = objCompany.ListLocPickDtl;
            objInboundInquiry = objService.GEtStrgBillTYpe(objInboundInquiry);
            objInboundInquiry.bill_type = objInboundInquiry.ListStrgBillType[0].bill_type;
            objInboundInquiry.bill_inout_type = objInboundInquiry.ListStrgBillType[0].bill_inout_type;
            objInboundInquiry = objService.LoadStrgId(objInboundInquiry);
            //if (objInboundInquiry.LstStrgIddtl.Count() == 0)
            //{
            //    List<InboundInquiry> li = new List<InboundInquiry>();
            //    objInboundInquiry.strg_rate = "STRG-1";
            //    li.Add(objInboundInquiry);
            //    objInboundInquiry.LstStrgIddtl = li;
            //}


            //objInboundInquiry = objService.LoadInoutId(objInboundInquiry);


            //if (objInboundInquiry.LstInoutIddtl.Count() == 0)
            //{
            //    List<InboundInquiry> lis = new List<InboundInquiry>();
            //    objInboundInquiry.inout_rate = "INOUT-1";
            //    lis.Add(objInboundInquiry);

            //    objInboundInquiry.LstInoutIddtl = lis;
            //}
            //objInboundInquiry = objService.LoadStrgId(objInboundInquiry);
            objInboundInquiry.loc_id = "FLOOR";

            if (objInboundInquiry.LstStrgIddtl.Count() == 0)
            {

                l_str_rate = "STRG";
                return Json(l_str_rate, JsonRequestBehavior.AllowGet);
            }

            objInboundInquiry = objService.LoadInoutId(objInboundInquiry);


            if (objInboundInquiry.LstInoutIddtl.Count() == 0)
            {

                l_str_rate = "INOUT";
                return Json(l_str_rate, JsonRequestBehavior.AllowGet);
            }
            objInboundInquiry = objService.GetDocDtl(objInboundInquiry);

            objInboundInquiry = objService.GetDocEditDtl(objInboundInquiry);

            objInboundInquiry.hdr_note = objInboundInquiry.ListDocHdr[0].note;
            objInboundInquiry = objService.GetDocEntryCount(objInboundInquiry);
            l_int_line_count = objInboundInquiry.ListGetDocEntryCount[0].DocCount;
            objInboundInquiry.line_count = l_int_line_count + 1;
            objInboundInquiry.LineNum = l_int_line_count + 1;
            objInboundInquiry.View_Flag = "M";//CR2018-05-07-001 Added By Nithya
            Session["l_bool_edit_flag"] = false;
            objInboundInquiry.l_bool_edit_flag = Session["l_bool_edit_flag"].ToString();
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_DocEntryEdit", InboundInquiryModel);
        }
        public ActionResult Views(string CmpId, string id)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            String l_str_ib_doc_dt;
            String l_str_orderdt;
            String l_str_eta_dt;
            objInboundInquiry.cmp_id = CmpId;
            objInboundInquiry.ib_doc_id = id;
            objInboundInquiry = objService.GetDocHdr(objInboundInquiry);
            objInboundInquiry.cmp_id = objInboundInquiry.ListDocHdr[0].cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objInboundInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objInboundInquiry.ib_doc_id = objInboundInquiry.ListDocHdr[0].ib_doc_id;

            l_str_ib_doc_dt = Convert.ToDateTime(objInboundInquiry.ListDocHdr[0].ib_doc_dt).ToString("MM/dd/yyyy");
            l_str_orderdt = Convert.ToDateTime(objInboundInquiry.ListDocHdr[0].ordr_dt).ToString("MM/dd/yyyy");
            objInboundInquiry.ib_doc_dt = l_str_ib_doc_dt;
            objInboundInquiry.status = objInboundInquiry.ListDocHdr[0].status;
            objInboundInquiry.cont_id = objInboundInquiry.ListDocHdr[0].cntr_id;
            l_str_eta_dt = Convert.ToDateTime(objInboundInquiry.ListDocHdr[0].eta_dt).ToString("MM/dd/yyyy");
            objInboundInquiry.eta_dt = l_str_eta_dt;
            objInboundInquiry.refno = objInboundInquiry.ListDocHdr[0].req_num;
            objInboundInquiry.orderdt = l_str_orderdt;
            objInboundInquiry.recvdvia = objInboundInquiry.ListDocHdr[0].shipvia_id;
            objInboundInquiry.recvd_fm = objInboundInquiry.ListDocHdr[0].vend_name;
            objInboundInquiry.bol = objInboundInquiry.ListDocHdr[0].master_bol;
            objInboundInquiry.vessel_num = objInboundInquiry.ListDocHdr[0].vessel_no;
            objInboundInquiry.note = objInboundInquiry.ListDocHdr[0].note;
            objInboundInquiry = objService.GetDocDtl(objInboundInquiry);
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_DocEntryView", InboundInquiryModel);
        }
        public JsonResult LoadStyledtl(string p_str_cmp_id)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry = objService.GetPickStyleDetails(objInboundInquiry);
            objInboundInquiry.itm_num = objInboundInquiry.itm_num;
            objInboundInquiry.itm_color = objInboundInquiry.itm_color;
            objInboundInquiry.itm_size = objInboundInquiry.itm_size;
            return Json(objInboundInquiry.ListItmStyledtl, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInboundInquiryFromDashboard(string p_str_cmp_id)
        {
            try
            {
                string l_str_is_3rd_usr = string.Empty;

                InboundInquiry objInboundInquiry = new InboundInquiry();
                InboundInquiryService objService = new InboundInquiryService();
                l_str_is_3rd_usr = Session["IS3RDUSER"].ToString();
                objInboundInquiry.IS3RDUSER = l_str_is_3rd_usr.Trim();
                objInboundInquiry.cmp_id = p_str_cmp_id;
                objInboundInquiry.cntr_id = "";
                objInboundInquiry.status = "";
                objInboundInquiry.ib_doc_id = "";
                objInboundInquiry.req_num = "";
                objInboundInquiry.ib_doc_dt_fm = DateTime.Now.ToString("MM/DD/YYYY");
                objInboundInquiry.ib_doc_dt_to = DateTime.Now.ToString("MM/DD/YYYY");
                objInboundInquiry.eta_dt_fm = "";
                objInboundInquiry.eta_dt_to = "";
                objInboundInquiry = objService.GetInboundInquiryDetails(objInboundInquiry);
                Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
                InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
                return PartialView("_InboundInquiry", InboundInquiryModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult IncludeDocDetails(string p_str_cmp_id, string p_str_ibdocid, string p_str_line_num, string p_str_style, string p_str_color, string p_str_size, string p_str_desc, string p_str_Itmcode,
            string p_str_ord_qty, int p_str_ppk, int p_str_ctns, string p_str_po_num, decimal p_str_length, decimal p_str_width, decimal p_str_height,
            decimal p_str_weight, decimal p_str_cube, string p_str_loc_id, string p_str_strg_rate, string p_str_inout_rate, string p_str_note, int p_int_ctn_line)
        {
            try
            {
                string l_str_itmcode;
                int l_str_doc_entry_id;
                int l_int_ordr_qty1;
                int l_int_ctn_qty1;
                int l_int_tmp_ordr_qty;
                l_int_tmp_ordr_qty = Convert.ToInt32(p_str_ord_qty);
                l_int_ordr_qty1 = l_int_tmp_ordr_qty - ((l_int_tmp_ordr_qty) % (p_str_ppk));
                l_int_ctn_qty1 = l_int_tmp_ordr_qty % (p_str_ppk);

                InboundInquiry objInboundInquiry = new InboundInquiry();
                InboundInquiryService objService = new InboundInquiryService();
                objInboundInquiry.cmp_id = p_str_cmp_id;
                objInboundInquiry.ib_doc_id = p_str_ibdocid;
                objInboundInquiry.line_num = Convert.ToInt32(p_str_line_num);
                objInboundInquiry.itm_code = p_str_Itmcode;
                objInboundInquiry.ctn_line = p_int_ctn_line;
                objInboundInquiry.ppk = p_str_ppk;
                objInboundInquiry.po_num = p_str_po_num;
                objInboundInquiry = objService.CheckItemExist(objInboundInquiry);//CR180328-001 Added by Nithya
                                                                                 //if (objInboundInquiry.LstItmExist.Count == 0)
                                                                                 //{

                //if (p_str_Itmcode == "")
                //{
                //    //objInboundInquiry = objService.GetItmId(objInboundInquiry);
                //    //objInboundInquiry.itmid = objInboundInquiry.itm_code;
                //    //l_str_itmcode = objInboundInquiry.itm_code;
                //    objInboundInquiry.itm_code = "NEW"; //CR_3PL_MVC_BL_2018_0305_001 Added By Meera
                //}
                //else
                //{
                //    objInboundInquiry.itm_code = p_str_Itmcode;
                //}

                //CR-3PL_MVC_IB_2018_0410_001 Added By Nithya
                if (p_str_Itmcode == "")
                {
                    objInboundInquiry.itm_num = p_str_style;
                    objInboundInquiry.itm_color = p_str_color;
                    objInboundInquiry.itm_size = p_str_size;
                    objInboundInquiry = objService.Validate_Itm(objInboundInquiry);
                    if (objInboundInquiry.LstItmExist.Count > 0)
                    {
                        objInboundInquiry.itm_code = objInboundInquiry.LstItmExist[0].itm_code;
                        p_str_Itmcode = objInboundInquiry.itm_code;
                    }

                }
                //end
                //CR_3PL_MVC_BL_2018_0306_001 Added By Meera
                if (p_str_Itmcode == "")
                {
                    objInboundInquiry.itm_num = p_str_style;
                    objInboundInquiry.itm_color = p_str_color;
                    objInboundInquiry.itm_size = p_str_size;
                    objInboundInquiry.itm_name = p_str_desc;
                    objInboundInquiry.ctn_qty = p_str_ppk; //p_str_ctns;//CR20180531-001 Added By Nithya
                    objInboundInquiry.weight = p_str_length;
                    objInboundInquiry.length = p_str_width;
                    objInboundInquiry.width = p_str_height;
                    objInboundInquiry.height = p_str_weight;
                    objInboundInquiry.cube = p_str_cube;

                    objInboundInquiry = objService.GetItmId(objInboundInquiry);
                    objInboundInquiry.itmid = objInboundInquiry.itm_code;
                    l_str_itmcode = objInboundInquiry.itm_code;
                    objInboundInquiry.itm_code = l_str_itmcode;
                    objInboundInquiry.flag = "A";

                    objService.Add_Style_To_Itm_dtl(objInboundInquiry);
                    objService.Add_Style_To_Itm_hdr(objInboundInquiry);
                }
                else
                {
                    objInboundInquiry.itm_num = p_str_style;
                    objInboundInquiry.itm_color = p_str_color;
                    objInboundInquiry.itm_size = p_str_size;
                    objInboundInquiry.itm_name = p_str_desc;
                    objInboundInquiry.ctn_qty = p_str_ppk; //p_str_ctns;//CR20180531-001 Added By Nithya
                    objInboundInquiry.weight = p_str_length;
                    objInboundInquiry.length = p_str_width;
                    objInboundInquiry.width = p_str_height;
                    objInboundInquiry.height = p_str_weight;
                    objInboundInquiry.cube = p_str_cube;
                    objInboundInquiry.flag = "M";
                    objInboundInquiry.itm_code = p_str_Itmcode;
                    objService.Add_Style_To_Itm_dtl(objInboundInquiry);

                }

                //CR_3PL_MVC_BL_2018_0306_001 Added By Meera

                objInboundInquiry = objService.GetCheckExistGridData(objInboundInquiry);
                objInboundInquiry = objService.GetDocEntryId(objInboundInquiry);
                objInboundInquiry.doc_entry_id = objInboundInquiry.doc_entry_id;
                l_str_doc_entry_id = objInboundInquiry.doc_entry_id;
                objInboundInquiry.doc_entry_id = l_str_doc_entry_id;
                objInboundInquiry.itm_num = p_str_style;
                objInboundInquiry.itm_color = p_str_color;
                objInboundInquiry.itm_size = p_str_size;
                objInboundInquiry.itm_name = p_str_desc;
                //  objInboundInquiry.ord_qty = Convert.ToDecimal(p_str_ord_qty);
                //  objInboundInquiry.docuppk = p_str_ppk;
                //  objInboundInquiry.ctn = p_str_ctns;
                objInboundInquiry.po_num = p_str_po_num;
                objInboundInquiry.doclength = p_str_length;
                objInboundInquiry.docwidth = p_str_width;
                objInboundInquiry.docheight = p_str_height;
                objInboundInquiry.docweight = p_str_weight;
                objInboundInquiry.doccube = p_str_cube;
                objInboundInquiry.loc_id = p_str_loc_id;
                objInboundInquiry.strg_rate = p_str_strg_rate;
                objInboundInquiry.inout_rate = p_str_inout_rate;
                if (p_str_note == "")
                {
                    objInboundInquiry.note = "-";
                }
                else
                {
                    objInboundInquiry.note = p_str_note;
                }
                if (l_int_ctn_qty1 > 0)
                {
                    objInboundInquiry.ord_qty = Convert.ToDecimal(p_str_ord_qty) - l_int_ctn_qty1;
                    objInboundInquiry.docuppk = p_str_ppk;
                    objInboundInquiry.ctn = p_str_ctns - 1;
                    objInboundInquiry.ctn_line = 1;

                    objService.InsertTempDocEntryDetails(objInboundInquiry);
                    objInboundInquiry.ord_qty = l_int_ctn_qty1;
                    objInboundInquiry.docuppk = l_int_ctn_qty1;
                    objInboundInquiry.ctn = 1;
                    objInboundInquiry.ctn_line = 2;

                    objService.InsertTempDocEntryDetails(objInboundInquiry);

                }
                else
                {
                    objInboundInquiry.ctn_line = 1;
                    objInboundInquiry.ord_qty = Convert.ToDecimal(p_str_ord_qty);
                    objInboundInquiry.docuppk = p_str_ppk;
                    objInboundInquiry.ctn = p_str_ctns;
                    objService.InsertTempDocEntryDetails(objInboundInquiry);

                }
                objInboundInquiry.cmp_id = p_str_cmp_id;
                objInboundInquiry.ib_doc_id = p_str_ibdocid;
                objInboundInquiry = objService.GetDocumentEntryTempGridDtl(objInboundInquiry);

                objInboundInquiry = objService.GetDocEntryCount(objInboundInquiry);
                Session["l_bool_edit_flag"] = "false";

                Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
                InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
                return PartialView("_DocEntryGrid", InboundInquiryModel);
                //}
                //    else
                //    {
                //        return Json(objInboundInquiry.LstItmExist.Count, JsonRequestBehavior.AllowGet);//CR180328-001 Added by Nithya
                //    }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult IncludeEditDocDetails(string p_str_cmp_id, string p_str_ibdocid, string p_str_line_num, string p_str_style, string p_str_color, string p_str_size, string p_str_desc,
            string p_str_Itmcode, string p_str_ord_qty, int p_str_ppk, int p_str_ctns, string p_str_po_num, decimal p_str_length, decimal p_str_width, decimal p_str_height,
           decimal p_str_weight, decimal p_str_cube, string p_str_loc_id, string p_str_strg_rate, string p_str_inout_rate, string p_str_note, int p_int_ctn_line)
        {
            try
            {
                string l_str_itmcode;
                int l_str_doc_entry_id;
                int l_int_ordr_qty1;
                int l_int_ctn_qty1;
                int l_int_tmp_ordr_qty;

                l_int_tmp_ordr_qty = Convert.ToInt32(p_str_ord_qty);
                l_int_ordr_qty1 = l_int_tmp_ordr_qty - ((l_int_tmp_ordr_qty) % (p_str_ppk));
                l_int_ctn_qty1 = l_int_tmp_ordr_qty % (p_str_ppk);
                InboundInquiry objInboundInquiry = new InboundInquiry();
                InboundInquiryService objService = new InboundInquiryService();
                objInboundInquiry.cmp_id = p_str_cmp_id;
                objInboundInquiry.ib_doc_id = p_str_ibdocid;
                objInboundInquiry.line_num = Convert.ToInt32(p_str_line_num);
                objInboundInquiry.itm_code = p_str_Itmcode;
                objInboundInquiry.ctn_line = p_int_ctn_line;
                objInboundInquiry.ppk = p_str_ppk;
                objInboundInquiry.po_num = p_str_po_num;
                objInboundInquiry = objService.CheckItemExist(objInboundInquiry);//CR180328-001 Added by Nithya
                                                                                 //if (objInboundInquiry.LstItmExist.Count == 0)
                                                                                 //{
                                                                                 //objInboundInquiry.l_bool_edit_flag = Session["l_bool_edit_flag"].ToString();//CR_MVC_3PL_0317-001 Added By Nithya l_bool_is_in_edit
                                                                                 //CR_MVC_3PL_0317-001 Commented By Nithya
                                                                                 //if (p_str_Itmcode == "")
                                                                                 //{
                                                                                 //    objInboundInquiry = objService.GetItmId(objInboundInquiry);
                                                                                 //    objInboundInquiry.itmid = objInboundInquiry.itm_code;
                                                                                 //    l_str_itmcode = objInboundInquiry.itm_code;
                                                                                 //    objInboundInquiry.itm_code = l_str_itmcode;
                                                                                 //}
                                                                                 //else
                                                                                 //{
                                                                                 //    objInboundInquiry.itm_code = p_str_Itmcode;
                                                                                 //}               
                                                                                 //CR-3PL_MVC_IB_2018_0410_001 Added By Nithya
                if (p_str_Itmcode == "")
                {
                    objInboundInquiry.itm_num = p_str_style;
                    objInboundInquiry.itm_color = p_str_color;
                    objInboundInquiry.itm_size = p_str_size;
                    objInboundInquiry = objService.Validate_Itm(objInboundInquiry);
                    //objInboundInquiry.itm_code = objInboundInquiry.LstItmExist[0].itm_code;
                    if (objInboundInquiry.LstItmExist.Count > 0)
                    {
                        objInboundInquiry.itm_code = objInboundInquiry.LstItmExist[0].itm_code;
                        p_str_Itmcode = objInboundInquiry.itm_code;
                    }
                }
                //end
                //CR_MVC_3PL_0317-001 Added By Nithya
                if (p_str_Itmcode == "")
                {
                    objInboundInquiry.itm_num = p_str_style;
                    objInboundInquiry.itm_color = p_str_color;
                    objInboundInquiry.itm_size = p_str_size;
                    objInboundInquiry.itm_name = p_str_desc;
                    objInboundInquiry.ctn_qty = p_str_ctns;
                    objInboundInquiry.weight = p_str_length;
                    objInboundInquiry.length = p_str_width;
                    objInboundInquiry.width = p_str_height;
                    objInboundInquiry.height = p_str_weight;
                    objInboundInquiry.cube = p_str_cube;

                    objInboundInquiry = objService.GetItmId(objInboundInquiry);
                    objInboundInquiry.itmid = objInboundInquiry.itm_code;
                    l_str_itmcode = objInboundInquiry.itm_code;
                    objInboundInquiry.itm_code = l_str_itmcode;
                    objInboundInquiry.flag = "A";

                    objService.Add_Style_To_Itm_dtl(objInboundInquiry);
                    objService.Add_Style_To_Itm_hdr(objInboundInquiry);
                }
                else
                {
                    objInboundInquiry.itm_num = p_str_style;
                    objInboundInquiry.itm_color = p_str_color;
                    objInboundInquiry.itm_size = p_str_size;
                    objInboundInquiry.itm_name = p_str_desc;
                    objInboundInquiry.ctn_qty = p_str_ctns;
                    objInboundInquiry.weight = p_str_length;
                    objInboundInquiry.length = p_str_width;
                    objInboundInquiry.width = p_str_height;
                    objInboundInquiry.height = p_str_weight;
                    objInboundInquiry.cube = p_str_cube;
                    objInboundInquiry.flag = "M";
                    objInboundInquiry.itm_code = p_str_Itmcode;
                }
                //End
                objInboundInquiry = objService.GetCheckExistGridData(objInboundInquiry);
                objInboundInquiry = objService.GetDocEntryId(objInboundInquiry);
                objInboundInquiry.doc_entry_id = objInboundInquiry.doc_entry_id;
                l_str_doc_entry_id = objInboundInquiry.doc_entry_id;
                objInboundInquiry.doc_entry_id = l_str_doc_entry_id;
                objInboundInquiry.itm_num = p_str_style;
                objInboundInquiry.itm_color = p_str_color;
                objInboundInquiry.itm_size = p_str_size;
                objInboundInquiry.itm_name = p_str_desc;
                objInboundInquiry.ord_qty = Convert.ToDecimal(p_str_ord_qty);
                objInboundInquiry.docuppk = p_str_ppk;
                objInboundInquiry.ctn = p_str_ctns;
                objInboundInquiry.po_num = p_str_po_num;
                objInboundInquiry.doclength = p_str_length;
                objInboundInquiry.docwidth = p_str_width;
                objInboundInquiry.docheight = p_str_height;
                objInboundInquiry.docweight = p_str_weight;
                objInboundInquiry.doccube = p_str_cube;
                objInboundInquiry.loc_id = p_str_loc_id;
                objInboundInquiry.strg_rate = p_str_strg_rate;
                objInboundInquiry.inout_rate = p_str_inout_rate;
                if (p_str_note == "")
                {
                    objInboundInquiry.note = "-";
                }
                else
                {
                    objInboundInquiry.note = p_str_note;
                }
                if (l_int_ctn_qty1 > 0)
                {
                    objInboundInquiry.ord_qty = Convert.ToDecimal(p_str_ord_qty) - l_int_ctn_qty1;
                    objInboundInquiry.docuppk = p_str_ppk;
                    objInboundInquiry.ctn = p_str_ctns - 1;
                    objInboundInquiry.ctn_line = 1;

                    objService.InsertTempDocEntryDetails(objInboundInquiry);
                    objInboundInquiry.ord_qty = l_int_ctn_qty1;
                    objInboundInquiry.docuppk = l_int_ctn_qty1;
                    objInboundInquiry.ctn = 1;
                    objInboundInquiry.ctn_line = 2;

                    objService.InsertTempDocEntryDetails(objInboundInquiry);

                }
                else
                {
                    objInboundInquiry.ctn_line = 1;
                    objInboundInquiry.ord_qty = Convert.ToDecimal(p_str_ord_qty);
                    objInboundInquiry.docuppk = p_str_ppk;
                    objInboundInquiry.ctn = p_str_ctns;
                    objService.InsertTempDocEntryDetails(objInboundInquiry);

                }
                objInboundInquiry.cmp_id = p_str_cmp_id;
                objInboundInquiry.ib_doc_id = p_str_ibdocid;
                //objService.InsertTempDocEntryDetails(objInboundInquiry);
                objInboundInquiry = objService.GetDocumentEntryTempGridDtl(objInboundInquiry);
                objInboundInquiry = objService.GetDocEntryCount(objInboundInquiry);
                Session["l_bool_edit_flag"] = "false";

                Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
                InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
                return PartialView("_DocEntryEditGrid", InboundInquiryModel);
                //}
                //else
                //{
                //    return Json(objInboundInquiry.LstItmExist.Count, JsonRequestBehavior.AllowGet);//CR180328-001 Added by Nithya
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public JsonResult LoadAvailQtys(string p_str_cmp_id, string p_str_style, string p_str_color, string p_str_size, string p_str_Itmcode)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.itm_code = p_str_Itmcode;
            objInboundInquiry.itm_num = p_str_style;
            objInboundInquiry.itm_color = p_str_color;
            objInboundInquiry.itm_size = p_str_size;
            //objInboundInquiry = objService.LoadAvailQty(objInboundInquiry);
            //objInboundInquiry.avlqty = objInboundInquiry.LstItmxCustdtl[0].avail_qty;
            //avlqty = Convert.ToString(objInboundInquiry.avlqty);
            objInboundInquiry = objService.Getitmlist(objInboundInquiry);
            if (objInboundInquiry.LstItmxCustdtl.Count() == 0)
            {
                objInboundInquiry.ordr_qty = 0;
                objInboundInquiry.length = 0;
                objInboundInquiry.width = 0;
                objInboundInquiry.height = 0;
                objInboundInquiry.cube = 0;
                objInboundInquiry.weight = 0;
                objInboundInquiry.ppk = 0;
                //objInboundInquiry.itm_code = "";
                //objInboundInquiry.length =  (objInboundInquiry.LstItmxCustdtl[0].length == null || objInboundInquiry.LstItmxCustdtl[0].length == 0 ? 0 : objInboundInquiry.LstItmxCustdtl[0].length);
                //objInboundInquiry.width = (objInboundInquiry.LstItmxCustdtl[0].width == null || objInboundInquiry.LstItmxCustdtl[0].width == 0 ? 0 : objInboundInquiry.LstItmxCustdtl[0].width); 
                //objInboundInquiry.height = (objInboundInquiry.LstItmxCustdtl[0].depth == null || objInboundInquiry.LstItmxCustdtl[0].depth == 0 ? 0 : objInboundInquiry.LstItmxCustdtl[0].depth);
                //objInboundInquiry.cube = (objInboundInquiry.LstItmxCustdtl[0].cube == null || objInboundInquiry.LstItmxCustdtl[0].cube == 0 ? 0 : objInboundInquiry.LstItmxCustdtl[0].cube); 
                //objInboundInquiry.weight = (objInboundInquiry.LstItmxCustdtl[0].wgt == null || objInboundInquiry.LstItmxCustdtl[0].wgt == 0 ? 0 : objInboundInquiry.LstItmxCustdtl[0].wgt); 
                //objInboundInquiry.ppk = (objInboundInquiry.LstItmxCustdtl[0].ctn_qty == null || objInboundInquiry.LstItmxCustdtl[0].ctn_qty == 0 ? 0 : objInboundInquiry.LstItmxCustdtl[0].ctn_qty); 
                //objInboundInquiry.itm_code = (objInboundInquiry.LstItmxCustdtl[0].itm_code == null || objInboundInquiry.LstItmxCustdtl[0].itm_code == string.Empty ? string.Empty : objInboundInquiry.LstItmxCustdtl[0].itm_code.Trim());
                return Json("Y", JsonRequestBehavior.AllowGet);

            }
            else
            {
                //objInboundInquiry.LstItmxCustdtl[0].avail_qty = Convert.ToDecimal(avlqty);
                objInboundInquiry.length = objInboundInquiry.LstItmxCustdtl[0].length;
                objInboundInquiry.width = objInboundInquiry.LstItmxCustdtl[0].width;
                objInboundInquiry.height = objInboundInquiry.LstItmxCustdtl[0].depth;
                objInboundInquiry.cube = objInboundInquiry.LstItmxCustdtl[0].cube;
                objInboundInquiry.weight = objInboundInquiry.LstItmxCustdtl[0].wgt;
                objInboundInquiry.ppk = objInboundInquiry.LstItmxCustdtl[0].ctn_qty;
                objInboundInquiry.itm_code = objInboundInquiry.LstItmxCustdtl[0].itm_code;
                //objInboundInquiry.avlqty = objInboundInquiry.LstItmxCustdtl[0].avail_qty;     
                return Json(objInboundInquiry.LstItmxCustdtl, JsonRequestBehavior.AllowGet);

            }


        }
        public JsonResult LoadAvailQty(string p_str_cmp_id, string p_str_style, string p_str_color, string p_str_size, string p_str_Itmcode)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.itm_code = p_str_Itmcode;
            objInboundInquiry.itm_num = p_str_style;
            objInboundInquiry.itm_color = p_str_color;
            objInboundInquiry.itm_size = p_str_size;
            //objInboundInquiry = objService.LoadAvailQty(objInboundInquiry);
            //objInboundInquiry.avlqty = objInboundInquiry.LstItmxCustdtl[0].avail_qty;
            //avlqty = Convert.ToString(objInboundInquiry.avlqty);
            objInboundInquiry = objService.Getitmlist(objInboundInquiry);
            if (objInboundInquiry.LstItmxCustdtl.Count() == 0)
            {
                objInboundInquiry.ordr_qty = 0;
                objInboundInquiry.length = 0;
                objInboundInquiry.width = 0;
                objInboundInquiry.height = 0;
                objInboundInquiry.cube = 0;
                objInboundInquiry.weight = 0;
                objInboundInquiry.ppk = 0;
                //objInboundInquiry.itm_code = "";
                //objInboundInquiry.length =  (objInboundInquiry.LstItmxCustdtl[0].length == null || objInboundInquiry.LstItmxCustdtl[0].length == 0 ? 0 : objInboundInquiry.LstItmxCustdtl[0].length);
                //objInboundInquiry.width = (objInboundInquiry.LstItmxCustdtl[0].width == null || objInboundInquiry.LstItmxCustdtl[0].width == 0 ? 0 : objInboundInquiry.LstItmxCustdtl[0].width); 
                //objInboundInquiry.height = (objInboundInquiry.LstItmxCustdtl[0].depth == null || objInboundInquiry.LstItmxCustdtl[0].depth == 0 ? 0 : objInboundInquiry.LstItmxCustdtl[0].depth);
                //objInboundInquiry.cube = (objInboundInquiry.LstItmxCustdtl[0].cube == null || objInboundInquiry.LstItmxCustdtl[0].cube == 0 ? 0 : objInboundInquiry.LstItmxCustdtl[0].cube); 
                //objInboundInquiry.weight = (objInboundInquiry.LstItmxCustdtl[0].wgt == null || objInboundInquiry.LstItmxCustdtl[0].wgt == 0 ? 0 : objInboundInquiry.LstItmxCustdtl[0].wgt); 
                //objInboundInquiry.ppk = (objInboundInquiry.LstItmxCustdtl[0].ctn_qty == null || objInboundInquiry.LstItmxCustdtl[0].ctn_qty == 0 ? 0 : objInboundInquiry.LstItmxCustdtl[0].ctn_qty); 
                //objInboundInquiry.itm_code = (objInboundInquiry.LstItmxCustdtl[0].itm_code == null || objInboundInquiry.LstItmxCustdtl[0].itm_code == string.Empty ? string.Empty : objInboundInquiry.LstItmxCustdtl[0].itm_code.Trim());
                return Json("Y", JsonRequestBehavior.AllowGet);

            }
            else
            {
                //objInboundInquiry.LstItmxCustdtl[0].avail_qty = Convert.ToDecimal(avlqty);
                objInboundInquiry.length = objInboundInquiry.LstItmxCustdtl[0].length;
                objInboundInquiry.width = objInboundInquiry.LstItmxCustdtl[0].width;
                objInboundInquiry.height = objInboundInquiry.LstItmxCustdtl[0].depth;
                objInboundInquiry.cube = objInboundInquiry.LstItmxCustdtl[0].cube;
                objInboundInquiry.weight = objInboundInquiry.LstItmxCustdtl[0].wgt;
                objInboundInquiry.ppk = objInboundInquiry.LstItmxCustdtl[0].ctn_qty;
                objInboundInquiry.itm_code = objInboundInquiry.LstItmxCustdtl[0].itm_code;
                //objInboundInquiry.avlqty = objInboundInquiry.LstItmxCustdtl[0].avail_qty;     
                return Json(objInboundInquiry.LstItmxCustdtl, JsonRequestBehavior.AllowGet);

            }


        }
        public JsonResult DocEntryEditDisplayGridToTextbox(string P_Str_CmpId, string P_Str_IbdocId, string P_Str_LineNum, string P_Str_ItmCode, string P_str_DocEntry_Id)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();

            objInboundInquiry.cmp_id = P_Str_CmpId;
            objInboundInquiry.ib_doc_id = P_Str_IbdocId;
            objInboundInquiry.line_num = Convert.ToInt32(P_Str_LineNum);
            objInboundInquiry.itm_code = P_Str_ItmCode;
            Session["tempItmCode"] = objInboundInquiry.itm_code;

            objInboundInquiry.doc_entry_id = Convert.ToInt32(P_str_DocEntry_Id);
            objInboundInquiry = objService.GetGridEditData(objInboundInquiry);
            objInboundInquiry.ListGridEditData[0].inout_rate = (objInboundInquiry.ListGridEditData[0].inout_rate.ToString().Trim()); //CR-3PL_MVC_IB_2018_0312_003
            objInboundInquiry.ListGridEditData[0].strg_rate = (objInboundInquiry.ListGridEditData[0].strg_rate.ToString().Trim()); //CR-3PL_MVC_IB_2018_0312_003
            return Json(objInboundInquiry.ListGridEditData, JsonRequestBehavior.AllowGet);

        }
        public JsonResult EdtiDocEntryGridEditDisplayGridToTextbox(string P_Str_CmpId, string P_Str_IbdocId, string P_Str_LineNum, string P_Str_ItmCode, string P_str_DocEntry_Id)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();

            objInboundInquiry.cmp_id = P_Str_CmpId;
            objInboundInquiry.ib_doc_id = P_Str_IbdocId;
            objInboundInquiry.line_num = Convert.ToInt32(P_Str_LineNum);
            objInboundInquiry.line_count = Convert.ToInt32(P_Str_LineNum);//CR20180507
            objInboundInquiry.LineNum = Convert.ToInt32(P_Str_LineNum);
            objInboundInquiry.itm_code = P_Str_ItmCode;
            Session["tempItmCode"] = objInboundInquiry.itm_code;

            objInboundInquiry.doc_entry_id = Convert.ToInt32(P_str_DocEntry_Id);
            objInboundInquiry = objService.GetEditGridData(objInboundInquiry);
            objInboundInquiry.ListGridEditData[0].inout_rate = (objInboundInquiry.ListGridEditData[0].inout_rate.ToString().Trim());
            objInboundInquiry.ListGridEditData[0].strg_rate = (objInboundInquiry.ListGridEditData[0].strg_rate.ToString().Trim());
            //   objInboundInquiry.ListGridEditData[0].l_bool_edit_flag = Session["l_bool_edit_flag"].ToString();
            return Json(objInboundInquiry.ListGridEditData, JsonRequestBehavior.AllowGet);

        }
        public ActionResult TruncateTempDocEntry()
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            objService.TruncateTempDocEntry(objInboundInquiry);
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("~/Views/InboundInquiry/InboundInquiry.cshtml", InboundInquiryModel);
        }
        public ActionResult DocEntryDeleteGridData(string P_Str_CmpId, string P_Str_IbdocId, string P_Str_LineNum, string P_Str_ItmCode, string P_str_DocEntry_Id)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            objInboundInquiry.cmp_id = P_Str_CmpId;
            objInboundInquiry.line_num = Convert.ToInt32(P_Str_LineNum);
            objInboundInquiry.ib_doc_id = P_Str_IbdocId;
            objInboundInquiry.doc_entry_id = Convert.ToInt32(P_str_DocEntry_Id);
            objInboundInquiry.itm_code = P_Str_ItmCode;
            objInboundInquiry.doc_entry_id = Convert.ToInt32(P_str_DocEntry_Id);
            objInboundInquiry = objService.GetGridDeleteData(objInboundInquiry);
            objInboundInquiry = objService.GetDocEntryCount(objInboundInquiry);
            objInboundInquiry.DocEntryCount = objInboundInquiry.ListGetDocEntryCount[0].DocCount;
            objInboundInquiry.GrdFlag = "D";

            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_DocEntryGrid", InboundInquiryModel);
        }
        public ActionResult DocEntryEditDeleteGridData(string P_Str_CmpId, string P_Str_IbdocId, string P_Str_LineNum, string P_Str_ItmCode, string P_str_DocEntry_Id)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            objInboundInquiry.cmp_id = P_Str_CmpId;
            objInboundInquiry.line_num = Convert.ToInt32(P_Str_LineNum);
            objInboundInquiry.ib_doc_id = P_Str_IbdocId;
            objInboundInquiry.doc_entry_id = Convert.ToInt32(P_str_DocEntry_Id);
            objInboundInquiry.itm_code = P_Str_ItmCode;
            objInboundInquiry.doc_entry_id = Convert.ToInt32(P_str_DocEntry_Id);
            objInboundInquiry = objService.GetGridDeleteData(objInboundInquiry);
            objInboundInquiry = objService.GetDocEntryCount(objInboundInquiry);
            objInboundInquiry.DocEntryCount = objInboundInquiry.ListGetDocEntryCount[0].DocCount;
            objInboundInquiry.GrdFlag = "D";

            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_DocEntryEditGrid", InboundInquiryModel);
        }
        public JsonResult DocEntryCount(string P_Str_CmpId, string P_Str_IbdocId)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            objInboundInquiry.cmp_id = P_Str_CmpId;
            objInboundInquiry.ib_doc_id = P_Str_IbdocId;

            objInboundInquiry = objService.GetDocEntryCount(objInboundInquiry);
            objInboundInquiry.DocEntryCount = objInboundInquiry.ListGetDocEntryCount[0].DocCount;
            return Json(objInboundInquiry.DocEntryCount, JsonRequestBehavior.AllowGet);

        }
        public JsonResult IB_INQ_HDR_DATA(string p_str_cmp_id, string p_str_cntr_id, string p_str_status, string p_str_ib_doc_id, string p_str_ib_doc_id_to, string p_str_ref_no, string p_str_doc_dtFm, string p_str_doc_dtTo, string p_str_eta_dtFm, string p_str_eta_dtTo, string p_str_rcvd_frm, string p_str_screen_title,
            string p_str_itm_num, string p_str_itm_color, string p_str_itm_size, string p_str_itm_desc, string p_str_itm_type)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            Session["g_str_cmp_id"] = p_str_cmp_id.Trim();
            Session["TEMP_CMP_ID"] = p_str_cmp_id.Trim();
            Session["TEMP_CNTR_ID"] = p_str_cntr_id.Trim();
            Session["TEMP_STATUS"] = p_str_status.Trim();
            Session["TEMP_IB_DOC_ID_FM"] = p_str_ib_doc_id.Trim();
            Session["TEMP_IB_DOC_ID_TO"] = p_str_ib_doc_id_to.Trim();
            Session["TEMP_REF_NO"] = p_str_ref_no.Trim();
            Session["TEMP_DOC_DT_FM"] = p_str_doc_dtFm.Trim();
            Session["TEMP_DOC_DT_TO"] = p_str_doc_dtTo.Trim();
            Session["TEMP_ETA_DT_FM"] = p_str_eta_dtFm.Trim();
            Session["TEMP_ETA_DT_TO"] = p_str_eta_dtTo.Trim();
            Session["TEMP_IB_RCVD_FM"] = p_str_rcvd_frm.Trim();
            Session["TEMP_SCREEN_ID"] = p_str_screen_title.Trim();
            Session["TEMP_STYLE"] = p_str_itm_num.Trim();
            Session["TEMP_COLOR"] = p_str_itm_color.Trim();
            Session["TEMP_SIZE"] = p_str_itm_size.Trim();
            Session["TEMP_DESC"] = p_str_itm_desc.Trim();
            Session["TEMP_STATUS"] = p_str_itm_type.Trim();
            //objInboundInquiry = objService.GetInboundInquiryDetails(objInboundInquiry);
            //objInboundInquiry.DocEntryCount = objInboundInquiry.ListGetDocEntryCount[0].DocCount;
            return Json(objInboundInquiry.DocEntryCount, JsonRequestBehavior.AllowGet);

        }
        public ActionResult InboundInquiryDtl(string p_str_cmp_id, string p_str_ibdocid)
        {
            try
            {

                InboundInquiry objInboundInquiry = new InboundInquiry();
                InboundInquiryService objService = new InboundInquiryService();
                string l_str_search_flag = string.Empty;
                string l_str_is_another_usr = string.Empty;

                l_str_is_another_usr = Session["IS3RDUSER"].ToString();
                objInboundInquiry.IS3RDUSER = l_str_is_another_usr.Trim();
                l_str_search_flag = Session["g_str_Search_flag"].ToString().Trim();
                objInboundInquiry.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                objInboundInquiry.user_id = Session["UserID"].ToString().Trim();
                if (l_str_search_flag == "True")
                {
                    objInboundInquiry.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                    objInboundInquiry.cmp_id = Session["TEMP_CMP_ID"].ToString().Trim();
                    objInboundInquiry.cntr_id = Session["TEMP_CNTR_ID"].ToString().Trim();
                    objInboundInquiry.status = Session["TEMP_STATUS"].ToString().Trim();
                    objInboundInquiry.ib_doc_id = Session["TEMP_CMP_ID"].ToString().Trim();
                    objInboundInquiry.ib_doc_id_fm = Session["TEMP_IB_DOC_ID_FM"].ToString().Trim();
                    objInboundInquiry.ib_doc_id_to = Session["TEMP_IB_DOC_ID_TO"].ToString().Trim();
                    objInboundInquiry.req_num = Session["TEMP_REF_NO"].ToString().Trim();
                    objInboundInquiry.ib_doc_dt_fm = Session["TEMP_DOC_DT_FM"].ToString().Trim();
                    objInboundInquiry.ib_doc_dt_to = Session["TEMP_DOC_DT_TO"].ToString().Trim();
                    objInboundInquiry.eta_dt_fm = Session["TEMP_ETA_DT_FM"].ToString().Trim();
                    objInboundInquiry.eta_dt_to = Session["TEMP_ETA_DT_TO"].ToString().Trim();
                    objInboundInquiry.vend_name = Session["TEMP_IB_RCVD_FM"].ToString().Trim();
                    objInboundInquiry.screentitle = Session["TEMP_SCREEN_ID"].ToString().Trim();
                    objInboundInquiry.itm_num = Session["TEMP_STYLE"].ToString().Trim();
                    objInboundInquiry.itm_color = Session["TEMP_COLOR"].ToString().Trim();
                    objInboundInquiry.itm_size = Session["TEMP_SIZE"].ToString().Trim();
                    objInboundInquiry.itm_name = Session["TEMP_DESC"].ToString().Trim();
                    objInboundInquiry.itm_type = Session["TEMP_STATUS"].ToString().Trim();
                }
                else
                {
                    objInboundInquiry.cmp_id = p_str_cmp_id;
                    objInboundInquiry.ib_doc_id = p_str_ibdocid;
                    objInboundInquiry.ib_doc_id_fm = p_str_ibdocid.Trim();
                    objInboundInquiry.ib_doc_id_to = p_str_ibdocid.Trim();
                    objInboundInquiry.screentitle = Session["TEMP_SCREEN_ID"].ToString().Trim();
                }

                objInboundInquiry = objService.GetInboundInquiryDetails(objInboundInquiry);
                Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
                InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
                return PartialView("_InboundInquiry", InboundInquiryModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult GetSearchInboundInquiry(string p_str_cmp_id, string p_str_cntr_id, string p_str_status, string p_str_ib_doc_id, string p_str_ib_doc_id_to, string p_str_ref_no, string p_str_doc_dtFm, string p_str_doc_dtTo, string p_str_eta_dtFm,
            string p_str_eta_dtTo, string p_str_rcvd_frm, string p_str_itm_num, string p_str_itm_color, string p_str_itm_size, string p_str_itm_desc, string p_str_itm_type)
        {
            try
            {
                string l_str_is_another_usr = string.Empty;


                InboundInquiry objInboundInquiry = new InboundInquiry();
                InboundInquiryService objService = new InboundInquiryService();
                l_str_is_another_usr = Session["IS3RDUSER"].ToString();
                objInboundInquiry.IS3RDUSER = l_str_is_another_usr.Trim();
                objInboundInquiry.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                objInboundInquiry.user_id = Session["UserID"].ToString().Trim();
                Session["g_str_Search_flag"] = "True";
                objInboundInquiry.cmp_id = p_str_cmp_id.Trim();
                objInboundInquiry.cntr_id = p_str_cntr_id.Trim();//CR 02-21-2018-00 added by murugan
                objInboundInquiry.status = p_str_status.Trim();
                objInboundInquiry.ib_doc_id = p_str_ib_doc_id.Trim();
                objInboundInquiry.ib_doc_id_fm = p_str_ib_doc_id.Trim();
                objInboundInquiry.ib_doc_id_to = p_str_ib_doc_id_to.Trim();
                objInboundInquiry.req_num = p_str_ref_no.Trim();
                objInboundInquiry.ib_doc_dt_fm = p_str_doc_dtFm.Trim();//CR 02-21-2018-04 added by murugan
                objInboundInquiry.ib_doc_dt_to = p_str_doc_dtTo.Trim();//CR 02-21-2018-04 added by murugan
                objInboundInquiry.eta_dt_fm = p_str_eta_dtFm.Trim();//CR 02-21-2018-01 added by murugan
                objInboundInquiry.eta_dt_to = p_str_eta_dtTo.Trim();//CR 02-21-2018-01 added by murugan
                objInboundInquiry.vend_name = p_str_rcvd_frm.Trim();

                objInboundInquiry.itm_num = p_str_itm_num.Trim();
                objInboundInquiry.itm_color = p_str_itm_color.Trim();
                objInboundInquiry.itm_size = p_str_itm_size.Trim();
                objInboundInquiry.itm_name = p_str_itm_desc.Trim();
                objInboundInquiry.itm_type = p_str_itm_type.Trim();



                objInboundInquiry = objService.GetInboundInquiryDetails(objInboundInquiry);
                Session["l_bool_edit_flag"] = false;//CR 03-24-2018-001 added by Nithya
                Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
                InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
                return PartialView("_InboundInquiry", InboundInquiryModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public ActionResult ShowReport(string SelectdID, string p_str_radio, string p_str_cmpid, string type)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string l_str_inout_type = string.Empty;
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            l_str_rpt_selection = p_str_radio;
            string l_str_status = string.Empty;
            string l_str_tmp_name = string.Empty; //CR - 3PL_MVC_IB_2018_0219_008
            InboundInquiry objInbound = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            objInbound.cmp_id = p_str_cmpid;
            objInbound.ib_doc_id = SelectdID;
            objInbound = objService.GetInboundStatus(objInbound);
            //CR - 3PL_MVC_IB_2018_0219_008
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cmp_id = p_str_cmpid;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            objInbound.LstCmpName = objCompany.LstCmpName;
            l_str_tmp_name = objInbound.LstCmpName[0].cmp_name.ToString().Trim();

            //CR - 3PL_MVC_IB_2018_0219_008
            //changed
            l_str_status = (objInbound.ListInboundStatusRptDetails[0].STATUS == null || objInbound.ListInboundStatusRptDetails[0].STATUS == "" ? string.Empty : objInbound.ListInboundStatusRptDetails[0].STATUS);
            l_str_status = l_str_status.Trim();
            try
            {
                if (isValid)
                {

                    if (l_str_rpt_selection == "Acknowledgement")
                    {
                        strReportName = "rpt_ib_doc_entry_ack.rpt";
                        InboundInquiry objInboundInquiry = new InboundInquiry();
                        InboundInquiryService ServiceObject = new InboundInquiryService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                        objInboundInquiry.cmp_id = p_str_cmpid;
                        objInboundInquiry.ib_doc_id = SelectdID;


                        objInboundInquiry = ServiceObject.GetInboundAckRptDetails(objInboundInquiry);
                        var rptSource = objInboundInquiry.ListAckRptDetails.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objInboundInquiry.ListAckRptDetails.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);  //CR - 3PL_MVC_IB_2018_0219_008
                        //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        if (type == "PDF")
                        {
                            objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.WordForWindows, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                        }
                        else if (type == "Excel")
                        {
                            InboundACKExcel objInboundInquiryExcel = new InboundACKExcel();
                            objInboundInquiryExcel.PONo = p_str_cmpid;
                            objInboundInquiryExcel.Style = SelectdID;
                            objInboundInquiryExcel = ServiceObject.GetInboundAckExcel(objInboundInquiryExcel);
                            var model = objInboundInquiryExcel.ListInboundAckExcelDetails.ToList();
                            GridView gv = new GridView();
                            gv.DataSource = model;
                            gv.DataBind();
                            Session["IB_ACK"] = gv;
                            if (Session["IB_ACK"] != null)
                            {
                                return new DownloadFileActionResult((GridView)Session["IB_ACK"], "IB_ACK-" + SelectdID.Trim() + DateTime.Now.ToString() + ".xls");
                            }
                            //rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);

                        }
                    }

                    if (l_str_rpt_selection == "WorkSheet")
                    {
                        strReportName = "rpt_ib_doc_entry_recv_worksheet.rpt";
                        InboundInquiry objInboundInquiry = new InboundInquiry();
                        InboundInquiryService ServiceObject = new InboundInquiryService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                        objInboundInquiry.cmp_id = p_str_cmpid;
                        objInboundInquiry.ib_doc_id = SelectdID;

                        objInboundInquiry = ServiceObject.GetInboundWorkSheetRptDetails(objInboundInquiry);
                        var rptSource = objInboundInquiry.ListWorkSheetRptDetails.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objInboundInquiry.ListWorkSheetRptDetails.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name); //CR - 3PL_MVC_IB_2018_0219_008
                        if (type == "PDF")
                        {
                            objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);

                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.WordForWindows, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                        }
                        else if (type == "Excel")
                        {

                            InboundWorkSheetExcel objInboundInquiryWorkSheetExcel = new InboundWorkSheetExcel();
                            objInboundInquiryWorkSheetExcel.PONo = p_str_cmpid;
                            objInboundInquiryWorkSheetExcel.Style = SelectdID;
                            objInboundInquiryWorkSheetExcel = ServiceObject.GetInboundWorkSheetExcel(objInboundInquiryWorkSheetExcel);
                            var model = objInboundInquiryWorkSheetExcel.ListInboundWorkSheetExcelDetails.ToList();
                            GridView gv = new GridView();
                            gv.DataSource = model;
                            gv.DataBind();
                            Session["IB_WORKSHEET"] = gv;
                            if (Session["IB_WORKSHEET"] != null)
                            {
                                return new DownloadFileActionResult((GridView)Session["IB_WORKSHEET"], "IB_WORKSHEET-" + SelectdID.Trim() + DateTime.Now.ToString() + ".xls");
                            }
                            //rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                        }

                    }
                    if (l_str_rpt_selection == "Confirmation")
                    {
                        if (l_str_status == "POST")
                        {
                            InboundInquiry objInboundInquiry = new InboundInquiry();
                            InboundInquiryService ServiceObject = new InboundInquiryService();
                            objInboundInquiry.cmp_id = p_str_cmpid;
                            objInboundInquiry.ib_doc_id = SelectdID;
                            objInboundInquiry = ServiceObject.GEtStrgBillTYpe(objInboundInquiry);
                            objInboundInquiry.bill_type = objInboundInquiry.ListStrgBillType[0].bill_type;
                            objInboundInquiry.bill_inout_type = objInboundInquiry.ListStrgBillType[0].bill_inout_type;
                            objInboundInquiry.CNTR_CHECK = "RATE_ID";
                            ServiceObject.GetContainerandRateID(objInboundInquiry);
                            l_str_inout_type = objInboundInquiry.check_inout_type;
                            if (l_str_inout_type != "")
                            {
                                if (objInboundInquiry.ListGETRateID[0].RATEID.Trim() == "CNTR")      //CR_3PL_MVC_BL_2018_0303_001 Added By MEERA 03-03-2018
                                {
                                    strReportName = "rpt_ib_doc_recv_post_confrimation_by_container.rpt";
                                    ReportDocument rd = new ReportDocument();
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                    objInboundInquiry.cmp_id = p_str_cmpid;
                                    objInboundInquiry.ib_doc_id = SelectdID;
                                    objInboundInquiry = ServiceObject.GetInboundConfirmationRptDetailsbyContainer(objInboundInquiry);
                                    var rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objInboundInquiry.ListConfirmationRptDetails.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name); //CR - 3PL_MVC_IB_2018_0219_008
                                                                                             // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                    if (type == "PDF")
                                    {
                                        objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);

                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.WordForWindows, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                    }
                                    else if (type == "Excel")
                                    {
                                        //rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        InboundConfirmationExcel objInboundInquiryCnfrmExcel = new InboundConfirmationExcel();
                                        objInboundInquiryCnfrmExcel.PoNum = p_str_cmpid;
                                        objInboundInquiryCnfrmExcel.Style = SelectdID;
                                        objInboundInquiryCnfrmExcel = ServiceObject.GetInboundConfimExcel(objInboundInquiryCnfrmExcel);
                                        var model = objInboundInquiryCnfrmExcel.ListInboundConfrmExcelDetails.ToList();
                                        GridView gv = new GridView();
                                        gv.DataSource = model;
                                        gv.DataBind();
                                        Session["IB_CNFIRM"] = gv;
                                        if (Session["IB_CNFIRM"] != null)
                                        {
                                            return new DownloadFileActionResult((GridView)Session["IB_CNFIRM"], "IB_CNFIRM-" + SelectdID.Trim() + DateTime.Now.ToString() + ".xls");
                                        }
                                    }
                                }
                                else
                                {
                                    objInboundInquiry.cmp_id = p_str_cmpid;
                                    objInboundInquiry.ib_doc_id = SelectdID;
                                    strReportName = "rpt_ib_doc_recv_post_confrimation.rpt";
                                    ReportDocument rd = new ReportDocument();
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                    objInboundInquiry = ServiceObject.GetInboundConfirmationRptDetails(objInboundInquiry);
                                    var rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objInboundInquiry.ListConfirmationRptDetails.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name); //CR - 3PL_MVC_IB_2018_0219_008
                                                                                             // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                    if (type == "PDF")
                                    {
                                        objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.WordForWindows, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                    }
                                    else if (type == "Excel")
                                    {
                                        //rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        InboundConfirmationExcel objInboundInquiryCnfrmExcel = new InboundConfirmationExcel();
                                        objInboundInquiryCnfrmExcel.PoNum = p_str_cmpid;
                                        objInboundInquiryCnfrmExcel.Style = SelectdID;
                                        objInboundInquiryCnfrmExcel = ServiceObject.GetInboundConfimExcel(objInboundInquiryCnfrmExcel);
                                        var model = objInboundInquiryCnfrmExcel.ListInboundConfrmExcelDetails.ToList();
                                        GridView gv = new GridView();
                                        gv.DataSource = model;
                                        gv.DataBind();
                                        Session["IB_CNFIRM"] = gv;
                                        if (Session["IB_CNFIRM"] != null)
                                        {
                                            return new DownloadFileActionResult((GridView)Session["IB_CNFIRM"], "IB_CNFIRM-" + SelectdID.Trim() + DateTime.Now.ToString() + ".xls");
                                        }
                                    }
                                }
                            }
                            {
                                objInboundInquiry.cmp_id = p_str_cmpid;
                                objInboundInquiry.ib_doc_id = SelectdID;
                                strReportName = "rpt_ib_doc_recv_post_confrimation.rpt";
                                ReportDocument rd = new ReportDocument();
                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                objInboundInquiry = ServiceObject.GetInboundConfirmationRptDetails(objInboundInquiry);
                                var rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objInboundInquiry.ListConfirmationRptDetails.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name); //CR - 3PL_MVC_IB_2018_0219_008
                                                                                         // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                if (type == "PDF")
                                {
                                    objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                }
                                else if (type == "Word")
                                {
                                    rd.ExportToHttpResponse(ExportFormatType.WordForWindows, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                }
                                else if (type == "Excel")
                                {
                                    //rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                    InboundConfirmationExcel objInboundInquiryCnfrmExcel = new InboundConfirmationExcel();
                                    objInboundInquiryCnfrmExcel.PoNum = p_str_cmpid;
                                    objInboundInquiryCnfrmExcel.Style = SelectdID;
                                    objInboundInquiryCnfrmExcel = ServiceObject.GetInboundConfimExcel(objInboundInquiryCnfrmExcel);
                                    var model = objInboundInquiryCnfrmExcel.ListInboundConfrmExcelDetails.ToList();
                                    GridView gv = new GridView();
                                    gv.DataSource = model;
                                    gv.DataBind();
                                    Session["IB_CNFIRM"] = gv;
                                    if (Session["IB_CNFIRM"] != null)
                                    {
                                        return new DownloadFileActionResult((GridView)Session["IB_CNFIRM"], "IB_CNFIRM-" + SelectdID.Trim() + DateTime.Now.ToString() + ".xls");
                                    }
                                }
                            }

                        }
                    }
                    if (l_str_rpt_selection == "TallySheet")
                    {

                        if (l_str_status == "POST")
                        {
                            InboundInquiry objInboundInquiry = new InboundInquiry();
                            InboundInquiryService ServiceObject = new InboundInquiryService();
                            objInboundInquiry.cmp_id = p_str_cmpid;
                            objInboundInquiry.ib_doc_id = SelectdID;
                            // CR_3PL_MVC_IB_2018_0223_001 Added by Soniya  
                            objInboundInquiry = ServiceObject.GEtStrgBillTYpe(objInboundInquiry);
                            objInboundInquiry.bill_type = objInboundInquiry.ListStrgBillType[0].bill_type;
                            objInboundInquiry.bill_inout_type = objInboundInquiry.ListStrgBillType[0].bill_inout_type;
                            objInboundInquiry.CNTR_CHECK = "RATE_ID";
                            ServiceObject.GetContainerandRateID(objInboundInquiry);
                            l_str_inout_type = objInboundInquiry.check_inout_type;
                            if (l_str_inout_type != "")
                            {
                                if (objInboundInquiry.ListGETRateID[0].RATEID == "CNTR")
                                {
                                    strReportName = "rpt_ib_doc_recv_post_confrimation_by_container.rpt";
                                    ReportDocument rd = new ReportDocument();
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                    objInboundInquiry.cmp_id = p_str_cmpid;
                                    objInboundInquiry.ib_doc_id = SelectdID;
                                    objInboundInquiry = ServiceObject.GetInboundConfirmationRptDetailsbyContainer(objInboundInquiry);
                                    var rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objInboundInquiry.ListConfirmationRptDetails.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name); //CR - 3PL_MVC_IB_2018_0219_008
                                                                                             // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                    if (type == "PDF")
                                    {
                                        objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.WordForWindows, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                    }
                                    else if (type == "Excel")
                                    {
                                        //rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        InboundConfirmationExcel objInboundInquiryCnfrmExcel = new InboundConfirmationExcel();
                                        objInboundInquiryCnfrmExcel.PoNum = p_str_cmpid;
                                        objInboundInquiryCnfrmExcel.Style = SelectdID;
                                        objInboundInquiryCnfrmExcel = ServiceObject.GetInboundConfimExcel(objInboundInquiryCnfrmExcel);
                                        var model = objInboundInquiryCnfrmExcel.ListInboundConfrmExcelDetails.ToList();
                                        GridView gv = new GridView();
                                        gv.DataSource = model;
                                        gv.DataBind();
                                        Session["IB_CNFIRM"] = gv;
                                        if (Session["IB_CNFIRM"] != null)
                                        {
                                            return new DownloadFileActionResult((GridView)Session["IB_CNFIRM"], "IB_CNFIRM-" + SelectdID.Trim() + DateTime.Now.ToString() + ".xls");
                                        }
                                    }
                                }
                                else
                                {
                                    objInboundInquiry.cmp_id = p_str_cmpid;
                                    objInboundInquiry.ib_doc_id = SelectdID;
                                    strReportName = "rpt_ib_doc_recv_post_confrimation.rpt";
                                    ReportDocument rd = new ReportDocument();
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                    objInboundInquiry = ServiceObject.GetInboundConfirmationRptDetails(objInboundInquiry);
                                    var rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objInboundInquiry.ListConfirmationRptDetails.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name); //CR - 3PL_MVC_IB_2018_0219_008
                                                                                             // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                    if (type == "PDF")
                                    {
                                        objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.WordForWindows, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                    }
                                    else if (type == "Excel")
                                    {
                                        //rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        InboundConfirmationExcel objInboundInquiryCnfrmExcel = new InboundConfirmationExcel();
                                        objInboundInquiryCnfrmExcel.PoNum = p_str_cmpid;
                                        objInboundInquiryCnfrmExcel.Style = SelectdID;
                                        objInboundInquiryCnfrmExcel = ServiceObject.GetInboundConfimExcel(objInboundInquiryCnfrmExcel);
                                        var model = objInboundInquiryCnfrmExcel.ListInboundConfrmExcelDetails.ToList();
                                        GridView gv = new GridView();
                                        gv.DataSource = model;
                                        gv.DataBind();
                                        Session["IB_CNFIRM"] = gv;
                                        if (Session["IB_CNFIRM"] != null)
                                        {
                                            return new DownloadFileActionResult((GridView)Session["IB_CNFIRM"], "IB_CNFIRM-" + SelectdID.Trim() + DateTime.Now.ToString() + ".xls");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                objInboundInquiry.cmp_id = p_str_cmpid;
                                objInboundInquiry.ib_doc_id = SelectdID;
                                strReportName = "rpt_ib_doc_recv_post_confrimation.rpt";
                                ReportDocument rd = new ReportDocument();
                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                objInboundInquiry = ServiceObject.GetInboundConfirmationRptDetails(objInboundInquiry);
                                var rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objInboundInquiry.ListConfirmationRptDetails.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name); //CR - 3PL_MVC_IB_2018_0219_008
                                                                                         // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                if (type == "PDF")
                                {
                                    objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                }
                                else if (type == "Word")
                                {
                                    rd.ExportToHttpResponse(ExportFormatType.WordForWindows, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                }
                                else if (type == "Excel")
                                {
                                    //rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                    InboundConfirmationExcel objInboundInquiryCnfrmExcel = new InboundConfirmationExcel();
                                    objInboundInquiryCnfrmExcel.PoNum = p_str_cmpid;
                                    objInboundInquiryCnfrmExcel.Style = SelectdID;
                                    objInboundInquiryCnfrmExcel = ServiceObject.GetInboundConfimExcel(objInboundInquiryCnfrmExcel);
                                    var model = objInboundInquiryCnfrmExcel.ListInboundConfrmExcelDetails.ToList();
                                    GridView gv = new GridView();
                                    gv.DataSource = model;
                                    gv.DataBind();
                                    Session["IB_CNFIRM"] = gv;
                                    if (Session["IB_CNFIRM"] != null)
                                    {
                                        return new DownloadFileActionResult((GridView)Session["IB_CNFIRM"], "IB_CNFIRM-" + SelectdID.Trim() + DateTime.Now.ToString() + ".xls");
                                    }
                                }
                            }
                            // CR_3PL_MVC_IB_2018_0223_001 End
                        }
                        else if (l_str_status == "1-RCVD")
                        {
                            strReportName = "rpt_ib_doc_recv_tallysheet.rpt";
                            InboundInquiry objInboundInquiry = new InboundInquiry();
                            InboundInquiryService ServiceObject = new InboundInquiryService();
                            ReportDocument rd = new ReportDocument();
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                            objInboundInquiry.cmp_id = p_str_cmpid;
                            objInboundInquiry.ib_doc_id = SelectdID;

                            objInboundInquiry = ServiceObject.GetInboundTallySheetRptDetails(objInboundInquiry);
                            var rptSource = objInboundInquiry.ListTallySheetRptDetails.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objInboundInquiry.ListTallySheetRptDetails.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name); //CR - 3PL_MVC_IB_2018_0219_008
                            //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            if (type == "PDF")
                            {
                                objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                            }
                            else if (type == "Word")
                            {
                                rd.ExportToHttpResponse(ExportFormatType.WordForWindows, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                            }
                            else if (type == "Excel")
                            {
                                InboundTallySheetExcel objInboundInquiryTallySheetExcel = new InboundTallySheetExcel();
                                objInboundInquiryTallySheetExcel.PoNum = p_str_cmpid;
                                objInboundInquiryTallySheetExcel.Style = SelectdID;
                                objInboundInquiryTallySheetExcel = ServiceObject.GetInboundTallySheetExcel(objInboundInquiryTallySheetExcel);
                                var model = objInboundInquiryTallySheetExcel.ListInboundTallySheetExcelDetails.ToList();
                                GridView gv = new GridView();
                                gv.DataSource = model;
                                gv.DataBind();
                                Session["IB_TALLY"] = gv;
                                if (Session["IB_TALLY"] != null)
                                {
                                    return new DownloadFileActionResult((GridView)Session["IB_TALLY"], "IB_TALLY-" + SelectdID.Trim() + DateTime.Now.ToString() + ".xls");
                                }
                            }
                        }

                    }
                    if (l_str_rpt_selection == "GridSummary")
                    {
                        strReportName = "rpt_inbound_grid_summary.rpt";
                        InboundInquiry objInboundInquiry = new InboundInquiry();
                        InboundInquiryService ServiceObject = new InboundInquiryService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;

                        objInboundInquiry.cmp_id = p_str_cmpid;
                        objInboundInquiry.cntr_id = TempData["p_str_cntr_id"].ToString().Trim();
                        objInboundInquiry.status = TempData["p_str_status"].ToString().Trim();
                        objInboundInquiry.ib_doc_id = SelectdID;
                        objInboundInquiry.req_num = TempData["p_str_ref_no"].ToString().Trim();
                        objInboundInquiry.ib_doc_dt_fm = TempData["p_str_doc_dtFm"].ToString().Trim();
                        objInboundInquiry.ib_doc_dt_to = TempData["p_str_doc_dtTo"].ToString().Trim();
                        objInboundInquiry.eta_dt_fm = TempData["p_str_eta_dtFm"].ToString().Trim();
                        objInboundInquiry.eta_dt_to = TempData["p_str_eta_dtTo"].ToString().Trim();
                        objInboundInquiry = ServiceObject.GetInboundGridSummaryDetails(objInboundInquiry);
                        var rptSource = objInboundInquiry.ListGridSummaryRptDetails.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objInboundInquiry.ListGridSummaryRptDetails.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name); //CR - 3PL_MVC_IB_2018_0219_008
                        //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        if (type == "PDF")
                        {
                            objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.WordForWindows, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                        }
                        else if (type == "Excel")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                        }
                    }
                    else if (l_str_rpt_selection == "ShippingDetail")
                    {

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
        public ActionResult ShowSummaryReport(string p_str_ib_doc_id, string p_str_ib_doc_id_to, string p_str_radio, string p_str_cmp_id, string p_str_cntr_id, string p_str_status, string p_str_ref_no, string p_str_doc_dtFm,
            string p_str_doc_dtTo, string p_str_eta_dtFm, string p_str_eta_dtTo, string p_str_itm_num, string p_str_itm_color, string p_str_itm_size,
            string type)
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
            InboundInquiry objInbound = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            objInbound.cmp_id = p_str_cmp_id;
            objInbound.ib_doc_id = p_str_ib_doc_id;
            //objInbound = objService.GetInboundStatus(objInbound);
            //l_str_status = objInbound.ListInboundStatusRptDetails[0].STATUS;
            //CR - 3PL_MVC_IB_2018_0219_008
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cmp_id = p_str_cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            objInbound.LstCmpName = objCompany.LstCmpName;

            l_str_tmp_name = objInbound.LstCmpName[0].cmp_name.ToString().Trim();

            //CR - 3PL_MVC_IB_2018_0219_008
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "GridSummary")
                    {
                        strReportName = "rpt_inbound_grid_summary.rpt";
                        InboundInquiry objInboundInquiry = new InboundInquiry();
                        InboundInquiryService ServiceObject = new InboundInquiryService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;

                        objInboundInquiry.cmp_id = p_str_cmp_id;
                        objInboundInquiry.cntr_id = p_str_cntr_id;
                        objInboundInquiry.status = p_str_status;
                        objInboundInquiry.ib_doc_id = p_str_ib_doc_id;
                        objInboundInquiry.ib_doc_id_fm = p_str_ib_doc_id;
                        objInboundInquiry.ib_doc_id_to = p_str_ib_doc_id_to;
                        objInboundInquiry.req_num = p_str_ref_no;
                        objInboundInquiry.ib_doc_dt_fm = p_str_doc_dtFm;
                        objInboundInquiry.ib_doc_dt_to = p_str_doc_dtTo;
                        objInboundInquiry.eta_dt_fm = p_str_eta_dtFm;
                        objInboundInquiry.eta_dt_to = p_str_eta_dtTo;
                        objInboundInquiry.itm_num = p_str_itm_num;
                        objInboundInquiry.itm_color = p_str_itm_color;
                        objInboundInquiry.itm_size = p_str_itm_size;

                        objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                        objInboundInquiry = ServiceObject.GetInboundGridSummaryDetails(objInboundInquiry);
                        var rptSource = objInboundInquiry.ListGridSummaryRptDetails.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objInboundInquiry.ListGridSummaryRptDetails.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);

                        // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        if (type == "PDF")
                        {
                            rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name); //CR - 3PL_MVC_IB_2018_0219_008
                            rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);

                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);

                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.WordForWindows, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                        }
                        else if (type == "Excel")
                        {
                            //rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);

                            List<InboundGridSummaryExcel> li = new List<InboundGridSummaryExcel>();
                            for (int i = 0; i < objInboundInquiry.ListGridSummaryRptDetails.Count; i++)
                            {

                                InboundGridSummaryExcel objIBInquiryExcel = new InboundGridSummaryExcel();
                                objIBInquiryExcel.IBDocId = objInboundInquiry.ListGridSummaryRptDetails[i].ib_doc_id;
                                objIBInquiryExcel.DocDt = objInboundInquiry.ListGridSummaryRptDetails[i].ib_doc_dt;
                                objIBInquiryExcel.Status = objInboundInquiry.ListGridSummaryRptDetails[i].rptstatus;
                                objIBInquiryExcel.OrderType = objInboundInquiry.ListGridSummaryRptDetails[i].ordr_type;
                                objIBInquiryExcel.Container = objInboundInquiry.ListGridSummaryRptDetails[i].cntr_id;
                                objIBInquiryExcel.LotId = objInboundInquiry.ListGridSummaryRptDetails[i].lot_id;
                                objIBInquiryExcel.RefNo = objInboundInquiry.ListGridSummaryRptDetails[i].req_num;
                                objIBInquiryExcel.RcvdFrom = objInboundInquiry.ListGridSummaryRptDetails[i].vend_name;
                                objIBInquiryExcel.ETADt = objInboundInquiry.ListGridSummaryRptDetails[i].eta_dt;
                                objIBInquiryExcel.Note = objInboundInquiry.ListGridSummaryRptDetails[i].Note;
                                objIBInquiryExcel.IOBillStatus = objInboundInquiry.ListGridSummaryRptDetails[i].inout_bill_status;
                                li.Add(objIBInquiryExcel);
                            }

                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["IB_DOC_INQ"] = gv;
                            return new DownloadFileActionResult((GridView)Session["IB_DOC_INQ"], "IB_DOC_INQ" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
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
        public ActionResult Edit(string Id, string cmp_id)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();

            objInboundInquiry.CompID = cmp_id;
            objInboundInquiry.ib_doc_id = Id;
            objInboundInquiry.LineNum = 1;
            objInboundInquiry = ServiceObject.GetInboundHdrDtl(objInboundInquiry);
            objInboundInquiry.Container = (objInboundInquiry.ListAckRptDetails[0].Container == null || objInboundInquiry.ListAckRptDetails[0].Container == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].Container.Trim());
            objInboundInquiry.status = (objInboundInquiry.ListAckRptDetails[0].status == null || objInboundInquiry.ListAckRptDetails[0].status == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].status.Trim());
            objInboundInquiry.InboundRcvdDt = (objInboundInquiry.ListAckRptDetails[0].InboundRcvdDt == null || objInboundInquiry.ListAckRptDetails[0].InboundRcvdDt == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].InboundRcvdDt.Trim());
            //objInboundInquiry.vend_id = objInboundInquiry.ListAckRptDetails[0].vend_id.Trim();

            objInboundInquiry.vend_id = (objInboundInquiry.ListAckRptDetails[0].vend_id == null || objInboundInquiry.ListAckRptDetails[0].vend_id == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].vend_id.Trim());
            objInboundInquiry.vend_name = (objInboundInquiry.ListAckRptDetails[0].vend_name == null || objInboundInquiry.ListAckRptDetails[0].vend_name == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].vend_name.Trim());
            objInboundInquiry.FOB = (objInboundInquiry.ListAckRptDetails[0].FOB == null || objInboundInquiry.ListAckRptDetails[0].FOB == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].FOB.Trim());
            objInboundInquiry.refno = (objInboundInquiry.ListAckRptDetails[0].refno == null || objInboundInquiry.ListAckRptDetails[0].refno == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].refno.Trim());

            objInboundInquiry.ibdocid = Id;
            objInboundInquiry = ServiceObject.GetDocEntryId(objInboundInquiry);
            objInboundInquiry.doc_entry_id = objInboundInquiry.doc_entry_id;
            objInboundInquiry = ServiceObject.GetInboundDtl(objInboundInquiry);
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_InboundRcvdDetail", InboundInquiryModel);
        }
        //CR_3PL_MVC_BL_2018_0226_001 Modified By Ravi 26-02-2018
        public ActionResult LotDetail(string Id, string cmp_id, string ibdocid)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            objInboundInquiry.CompID = cmp_id;
            objInboundInquiry.lot_id = Id;
            objInboundInquiry.ib_doc_id = ibdocid;
            objInboundInquiry = ServiceObject.GetInboundHdrDtl(objInboundInquiry);
            objInboundInquiry.Container = (objInboundInquiry.ListAckRptDetails[0].Container == null || objInboundInquiry.ListAckRptDetails[0].Container == "") ? string.Empty : objInboundInquiry.ListAckRptDetails[0].Container.Trim();
            objInboundInquiry.status = (objInboundInquiry.ListAckRptDetails[0].status == null || objInboundInquiry.ListAckRptDetails[0].status == "") ? string.Empty : objInboundInquiry.ListAckRptDetails[0].status.Trim();
            objInboundInquiry.InboundRcvdDt = (objInboundInquiry.ListAckRptDetails[0].InboundRcvdDt == null || objInboundInquiry.ListAckRptDetails[0].InboundRcvdDt == "") ? string.Empty : objInboundInquiry.ListAckRptDetails[0].InboundRcvdDt.Trim();
            objInboundInquiry.vend_id = (objInboundInquiry.ListAckRptDetails[0].vend_id == null || objInboundInquiry.ListAckRptDetails[0].vend_id == "") ? string.Empty : objInboundInquiry.ListAckRptDetails[0].vend_id.Trim();
            objInboundInquiry.vend_name = (objInboundInquiry.ListAckRptDetails[0].vend_name == null || objInboundInquiry.ListAckRptDetails[0].vend_name == "") ? string.Empty : objInboundInquiry.ListAckRptDetails[0].vend_name.Trim();
            objInboundInquiry.FOB = (objInboundInquiry.ListAckRptDetails[0].FOB == null || objInboundInquiry.ListAckRptDetails[0].FOB == "") ? string.Empty : objInboundInquiry.ListAckRptDetails[0].FOB.Trim();
            objInboundInquiry.refno = (objInboundInquiry.ListAckRptDetails[0].refno == null || objInboundInquiry.ListAckRptDetails[0].refno == "") ? string.Empty : objInboundInquiry.ListAckRptDetails[0].refno.Trim();
            objInboundInquiry.ib_doc_id = (objInboundInquiry.ListAckRptDetails[0].ib_doc_id == null || objInboundInquiry.ListAckRptDetails[0].ib_doc_id == "") ? string.Empty : objInboundInquiry.ListAckRptDetails[0].ib_doc_id.Trim();
            //objInboundInquiry.status = objInboundInquiry.ListAckRptDetails[0].status;
            //objInboundInquiry.InboundRcvdDt = objInboundInquiry.ListAckRptDetails[0].InboundRcvdDt.Trim();
            //objInboundInquiry.vend_id = objInboundInquiry.ListAckRptDetails[0].vend_id.Trim();
            //objInboundInquiry.vend_name = objInboundInquiry.ListAckRptDetails[0].vend_name.Trim();
            //objInboundInquiry.FOB = objInboundInquiry.ListAckRptDetails[0].FOB.Trim();
            //objInboundInquiry.refno = objInboundInquiry.ListAckRptDetails[0].refno.Trim();
            //objInboundInquiry.ib_doc_id = objInboundInquiry.ListAckRptDetails[0].ib_doc_id.Trim();
            objInboundInquiry = ServiceObject.GetInboundLotDtl(objInboundInquiry);
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_InboundRcvdLotDetail", InboundInquiryModel);
        }
        //End
        public ActionResult Add(string cmpid, string datefrom, string dateto, string p_str_screen_title)
        {
            string l_str_ibdocid;
            string l_str_DftWhs = string.Empty;
            string l_str_rate = string.Empty;
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objInboundInquiry.cmp_id = cmpid;
            objInboundInquiry.DocumentdateFrom = datefrom;
            objInboundInquiry.DocumentdateTo = dateto;
            objInboundInquiry.screentitle = p_str_screen_title;
            objCompany.cust_cmp_id = cmpid;
            objCompany.cmp_id = cmpid;
            objCompany.whs_id = "";
            //objInboundInquiry.screentitle = Session["screentitle"].ToString();//CR_MVC_3PL_0317-001 Added By Nithya
            objInboundInquiry = ServiceObject.GetDftWhs(objInboundInquiry);
            if (objInboundInquiry.ListPickdtl.Count > 0)
            {
                l_str_DftWhs = (objInboundInquiry.ListPickdtl[0].dft_whs.Trim() == null || objInboundInquiry.ListPickdtl[0].dft_whs.Trim() == string.Empty) ? string.Empty : objInboundInquiry.ListPickdtl[0].dft_whs.Trim();
            }

            if (l_str_DftWhs == "" || l_str_DftWhs == null)
            {
                //objCompany = ServiceObjectCompany.GetCustOfCompName(objCompany);
                //objCompany.cust_cmp_id = objCompany.LstCustOfCmpName[0].cust_of_cmpid;
                objCompany.cust_cmp_id = cmpid;
                objCompany = ServiceObjectCompany.GetWhsIdDetails(objCompany);

                if (objCompany.ListwhsPickDtl.Count() != 0)
                {
                    objInboundInquiry.whs_id = objCompany.ListwhsPickDtl[0].whs_id;
                }
            }
            else
            {

                objInboundInquiry.whs_id = l_str_DftWhs;

            }
            //CR-180418-001 Added by Nithya
            if (objInboundInquiry.whs_id == null || objInboundInquiry.whs_id == "")
            {
                objInboundInquiry.whs_id = "-";
            }
            objInboundInquiry = ServiceObject.AddLocId(objInboundInquiry);
            objInboundInquiry = ServiceObject.LoadCustConfig(objInboundInquiry);
            objInboundInquiry = ServiceObject.LoadCustConfigRcvdItmMode(objInboundInquiry);
            if (objInboundInquiry.ListCustConfigRcvdItmModeDetails.Count() != 0)
            {
                if (objInboundInquiry.ListCustConfigRcvdItmModeDetails[0].Recv_Itm_Mode == "CUST LOT ID")
                {
                    objInboundInquiry.lot_id = "-";
                }
            }
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany.cust_cmp_id = cmpid;
            objInboundInquiry.status = "OPEN";
            Session["tempStatus"] = objInboundInquiry.status;
            objInboundInquiry.itm_color = "-";
            objInboundInquiry.itm_size = "-";

            objCompany = ServiceObjectCompany.GetLocIdDetails(objCompany);
            objInboundInquiry.ListLocPickDtl = objCompany.ListLocPickDtl;
            objInboundInquiry.LineNum = 1;
            objInboundInquiry = ServiceObject.GEtStrgBillTYpe(objInboundInquiry);
            if (objInboundInquiry.ListStrgBillType.Count > 0)
            {
                objInboundInquiry.bill_type = objInboundInquiry.ListStrgBillType[0].bill_type;
                objInboundInquiry.bill_inout_type = objInboundInquiry.ListStrgBillType[0].bill_inout_type;
            }
            else
            {
                objInboundInquiry.bill_type = string.Empty;
            }
            // objInboundInquiry.bill_inout_type = objInboundInquiry.ListStrgBillType[0].bill_inout_type;
            objInboundInquiry.strg_rate = "STRG-1";
            objInboundInquiry.loc_id = "FLOOR";
            objInboundInquiry = ServiceObject.LoadStrgId(objInboundInquiry);


            if (objInboundInquiry.LstStrgIddtl.Count() == 0)
            {
                //objInboundInquiry.LstStrgIddtl.Add();
                //List<InboundInquiry> li = new List<InboundInquiry>();
                //objInboundInquiry.strg_rate = "STRG-1";
                //li.Add(objInboundInquiry);
                //objInboundInquiry.LstStrgIddtl = li;
                l_str_rate = "STRG";
                return Json(l_str_rate, JsonRequestBehavior.AllowGet);
            }

            objInboundInquiry = ServiceObject.LoadInoutId(objInboundInquiry);


            if (objInboundInquiry.LstInoutIddtl.Count() == 0)
            {
                //List<InboundInquiry> lis = new List<InboundInquiry>();
                //objInboundInquiry.inout_rate = "INOUT-1";
                //objInboundInquiry.description1 = "INOUT-1";
                //lis.Add(objInboundInquiry);
                //objInboundInquiry.LstInoutIddtl = lis;
                // objInboundInquiry.LstInoutIddtl[0].inout_rate = "INOUT-1";
                l_str_rate = "INOUT";
                return Json(l_str_rate, JsonRequestBehavior.AllowGet);
            }
            objInboundInquiry.ib_doc_dt = DateTime.Now.ToString("MM/dd/yyyy");
            objInboundInquiry.orderdt = DateTime.Now.ToString("MM/dd/yyyy");
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objInboundInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objInboundInquiry = ServiceObject.GetIbDocIdDetail(objInboundInquiry);
            objInboundInquiry.ib_doc_id = objInboundInquiry.ibdocid;
            l_str_ibdocid = objInboundInquiry.ibdocid;
            objInboundInquiry.ib_doc_id = l_str_ibdocid;
            Session["tempIbdocId"] = objInboundInquiry.ib_doc_id;
            objInboundInquiry.LineNum = 1;
            objInboundInquiry.cmp_id = cmpid;
            objInboundInquiry.View_Flag = "A";//CR2018-05-07-001 Added By Nithya
            objInboundInquiry = ServiceObject.GetPickStyleDetails(objInboundInquiry);
            objInboundInquiry.ListItmStyledtl = objInboundInquiry.ListItmStyledtl;
            ServiceObject.GetDeleteTempData(objInboundInquiry);

            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_DocEntry", InboundInquiryModel);
        }
        public ActionResult ReceivingEntry(string CmpId, string id, string Cont_id, string datefrom, string dateto)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            string l_str_lot_id = string.Empty;
            string l_str_rcvd_itm_mode = string.Empty;
            DateTime l_str_rcd_dt;
            objInboundInquiry.DocumentdateFrom = datefrom;
            objInboundInquiry.DocumentdateTo = dateto;
            InboundInquiryService ServiceObject = new InboundInquiryService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objInboundInquiry.cmp_id = CmpId;
            objInboundInquiry.ibdocid = id;
            objInboundInquiry.ib_doc_id = id;
            objInboundInquiry.cntr_id = Cont_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany.cust_cmp_id = CmpId;
            objInboundInquiry.rcvd_from = "-";
            objInboundInquiry.loc_id = "FLOOR";
            objInboundInquiry.palet_id = "NEW";

            objInboundInquiry = ServiceObject.GetIBRecvDeleteTempData(objInboundInquiry); //CR_3PL_MVC_IB_2018_0317_001 
            //objInboundInquiry = ServiceObject.GetPalletId(objInboundInquiry);
            objInboundInquiry = ServiceObject.GetInsertTblIbDocDtlTmp(objInboundInquiry);
            objCompany = ServiceObjectCompany.GetCustConfigDtls(objCompany);
            objInboundInquiry.ListGetCustConfigDtls = objCompany.ListGetCustConfigDtls;

            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objInboundInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            if (objInboundInquiry.ListGetCustConfigDtls[0].Allow_New_item == "Y")
            {
                objInboundInquiry = ServiceObject.GetPickStyleDetails(objInboundInquiry);
                objInboundInquiry.ListItmStyledtl = objInboundInquiry.ListItmStyledtl;
            }

            objInboundInquiry = ServiceObject.GetRcvngHdr(objInboundInquiry);
            objInboundInquiry.cmp_id = objInboundInquiry.ListRcvgHdr[0].cmp_id;
            objInboundInquiry.ibdocid = objInboundInquiry.ListRcvgHdr[0].ib_doc_id;
            objInboundInquiry.cont_id = objInboundInquiry.ListRcvgHdr[0].cntr_id;
            objInboundInquiry.seal_num = objInboundInquiry.ListRcvgHdr[0].freight_id;
            l_str_rcd_dt = Convert.ToDateTime(objInboundInquiry.ListRcvgHdr[0].ib_doc_dt);
            objInboundInquiry.ib_doc_dt = l_str_rcd_dt.ToString("MM/dd/yyyy");
            //objInboundInquiry.ib_doc_dt = objInboundInquiry.ListRcvgHdr[0].ib_doc_dt;
            objInboundInquiry.vend_id = objInboundInquiry.ListRcvgHdr[0].shipvia_id;
            objInboundInquiry.refno = objInboundInquiry.ListRcvgHdr[0].req_num;
            objInboundInquiry.recvdvia = objInboundInquiry.ListRcvgHdr[0].vend_id;
            objInboundInquiry.rcvd_from = objInboundInquiry.ListRcvgHdr[0].vend_id; ;


            objCompany = ServiceObjectCompany.GetLocIdDetails(objCompany);
            objInboundInquiry.ListLocPickDtl = objCompany.ListLocPickDtl;
            //objInboundInquiry.loc_id = "FLOOR";        
            //20180217 aDDED By NITHYA dEFAULT WHSID
            objInboundInquiry = ServiceObject.GetDftWhs(objInboundInquiry);
            string l_str_DftWhs = objInboundInquiry.ListPickdtl[0].dft_whs.Trim();
            if (l_str_DftWhs != "" || l_str_DftWhs != null)
            {
                objInboundInquiry.whs_id = l_str_DftWhs;
            }
            objCompany.cust_cmp_id = CmpId;
            objCompany.whs_id = "";
            objCompany = ServiceObjectCompany.GetWhsIdDetails(objCompany);
            objInboundInquiry.ListwhsPickDtl = objCompany.ListwhsPickDtl;
            objInboundInquiry.bill_type = objInboundInquiry.ListGetCustConfigDtls[0].bill_type;
            objInboundInquiry.bill_inout_type = objInboundInquiry.ListGetCustConfigDtls[0].bill_inout_type;

            objInboundInquiry = ServiceObject.LoadStrgId(objInboundInquiry);
            for (int i = 0; i < objInboundInquiry.LstStrgIddtl.Count(); i++)
            {
                objInboundInquiry.strg_rate = "STRG-1";
                objInboundInquiry.strg_rate = objInboundInquiry.LstStrgIddtl[i].strg_rate;
            }
            if (objInboundInquiry.LstStrgIddtl.Count() == 0)
            {
                //objInboundInquiry.LstStrgIddtl.Add();
                //List<InboundInquiry> li = new List<InboundInquiry>();
                objInboundInquiry.LstStrgIddtl[0].strg_rate = "STRG-1"; //CR-3PL_MVC_IB_2018_0219_006 -ADDED BY NITHYA INOUT AND STORAGE RATE NOT LOADED IN THE COMBO BOX
                //objInboundInquiry.description = "STRG-1";
                //li.Add(objInboundInquiry);
                //objInboundInquiry.LstStrgIddtl = li;
            }
            objInboundInquiry.inout_rate = "INOUT-1";
            objInboundInquiry = ServiceObject.LoadInoutId(objInboundInquiry);
            // objInboundInquiry.inout_rate = objInboundInquiry.LstInoutIddtl[i].inout_rate;

            //for (int i = 0; i < objInboundInquiry.LstInoutIddtl.Count(); i++)
            //{


            //}
            if (objInboundInquiry.LstInoutIddtl.Count() == 0)
            {
                //List<InboundInquiry> lis = new List<InboundInquiry>();
                //objInboundInquiry.name1 = "INOUT-1";
                // objInboundInquiry.LstInoutIddtl[0].inout_rate = "INOUT-1";//CR-3PL_MVC_IB_2018_0219_006 -ADDED BY NITHYA INOUT AND STORAGE RATE NOT LOADED IN THE COMBO BOX
                //lis.Add(objInboundInquiry);

                //objInboundInquiry.LstInoutIddtl = lis;
                // objInboundInquiry.LstInoutIddtl[0].inout_rate = "INOUT-1";
                List<InboundInquiry> lis = new List<InboundInquiry>();
                objInboundInquiry.inout_rate = "INOUT-1";
                objInboundInquiry.LstInoutIddtl = lis;
            }
            objInboundInquiry = ServiceObject.GetRcvngGridDtl(objInboundInquiry);
            //Ravikumar 02-22-2018
            if (objInboundInquiry.ListRcvgDtl.Count() != 0)
            {
                l_str_lot_id = (objInboundInquiry.ListRcvgDtl[0].lot_id == null || objInboundInquiry.ListRcvgDtl[0].lot_id == "" ? string.Empty : objInboundInquiry.ListRcvgDtl[0].lot_id);
            }

            //End

            objInboundInquiry = ServiceObject.GetInsertTblIbDocRecvDtlTemp(objInboundInquiry);
            objInboundInquiry = ServiceObject.GetRcvdDtlCount(objInboundInquiry);
            objInboundInquiry.recv_dtl_count = objInboundInquiry.GetRcvdDtlCount[0].recv_dtl_count;
            objInboundInquiry.cmp_id = CmpId;
            objInboundInquiry.ib_doc_id = id;
            objInboundInquiry.lot_id = "";
            objInboundInquiry = ServiceObject.GetRcvdEntryCountDtl(objInboundInquiry);
            objInboundInquiry.recvcount = objInboundInquiry.LstRcvdEntryCountDtl[0].recvcount;
            objInboundInquiry = ServiceObject.GetCustRcvdItemMode(objInboundInquiry);
            l_str_rcvd_itm_mode = objInboundInquiry.ListGetCustConfigRcvdItemMode[0].recv_itm_mode;
            if (l_str_rcvd_itm_mode == "CUST LOT ID")
            {
                if (l_str_lot_id != "")
                {
                    objInboundInquiry.lot_id = l_str_lot_id;
                }
                else
                {
                    objInboundInquiry.lot_id = "NEW";
                }
            }
            else
            {
                objInboundInquiry.lot_id = "NEW";
            }
            objInboundInquiry.View_Flag = "A";

            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_ReceivingEntry", InboundInquiryModel);
        }
        public ActionResult ReceivingPost(string CmpId, string id, string Cont_id, string LotId, string datefrom, string dateto)
        {
            //string l_str_rpt_bill_type = string.Empty;
            //string l_str_rpt_bill_inout_type = string.Empty;
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            string l_str_item_sku_note = "Items(s)/SKU: ";      //CR_3PL_MVC_BL_2018_0303_001 Added By MEERA 03-03-2018
            objInboundInquiry.DocumentdateFrom = datefrom; //CR 2202018_01 Added by murugan
            objInboundInquiry.DocumentdateTo = dateto; //CR 2202018_01 Added by murugan
            objInboundInquiry.cmp_id = CmpId;
            objInboundInquiry.ibdocid = id;
            objInboundInquiry.cntr_id = Cont_id;
            objInboundInquiry.lot_id = LotId;

            //objInboundInquiry = ServiceObject.GEtStrgBillTYpe(objInboundInquiry);
            //objInboundInquiry.bill_type = objInboundInquiry.ListStrgBillType[0].bill_type;
            //objInboundInquiry.bill_inout_type = objInboundInquiry.ListStrgBillType[0].bill_inout_type;
            objInboundInquiry = ServiceObject.GetRcvngPostDtls(objInboundInquiry);
            objInboundInquiry = ServiceObject.LoadAvailDtl(objInboundInquiry);
            objInboundInquiry = ServiceObject.LoadLotItem(objInboundInquiry);

            TempData["TempListDetail"] = objInboundInquiry.ListRcvgPost;
            objInboundInquiry = ServiceObject.GEtStrgBillTYpe(objInboundInquiry);
            objInboundInquiry.bill_type = objInboundInquiry.ListStrgBillType[0].bill_type;
            objInboundInquiry.bill_inout_type = objInboundInquiry.ListStrgBillType[0].bill_inout_type;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany.cust_cmp_id = CmpId;
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objInboundInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            //CR_3PL_MVC_BL_2018_0303_001 Added By MEERA 03-03-2018
            if (CmpId == "SJOE")
            {
                for (int i = 0; i < objInboundInquiry.ListLotItem.Count(); i++)
                {
                    l_str_item_sku_note = l_str_item_sku_note + objInboundInquiry.ListRcvgPost[i].itm_num + ",";
                }
                l_str_item_sku_note = l_str_item_sku_note.Remove(l_str_item_sku_note.Length - 1);
                objInboundInquiry.CNTR_NOTE = l_str_item_sku_note;
            }
            else
            {
                objInboundInquiry.CNTR_NOTE = "-";
            }

            //End CR_3PL_MVC_BL_2018_0303_001 Added By MEERA 03-03-2018
            //Condition Added By Ravi 17-02-2018  
            if (objInboundInquiry.ListRcvgPost.Count != 0)
            {
                objInboundInquiry.cmp_id = objInboundInquiry.ListRcvgPost[0].cmp_id.Trim();
                objInboundInquiry.ibdocid = objInboundInquiry.ListRcvgPost[0].ib_doc_id;
                objInboundInquiry.cont_id = objInboundInquiry.ListRcvgPost[0].cntr_id;
                objInboundInquiry.ctns = objInboundInquiry.ListAvailDtl[0].CtnQty;
                objInboundInquiry.tot_qty = objInboundInquiry.ListAvailDtl[0].TotQty;

            }
            objInboundInquiry.View_Flag = "P";
            //CR_3PL_MVC_BL_2018_0224_001 Added By Ravi 24-02-2018
            objInboundInquiry.CNTR_CHECK = "CNTR_ID";
            ServiceObject.GetContainerandRateID(objInboundInquiry);
            objInboundInquiry.CNTR_CHECK = "PO_NUM";
            ServiceObject.GetContainerandRateID(objInboundInquiry);
            if (objInboundInquiry.ListGETContainerID.Count() != 0)
            {
                objInboundInquiry.CONTAINERID = objInboundInquiry.ListGETContainerID[0].CONTAINERID;
            }
            objInboundInquiry.CNTR_CHECK = "STRG_ID";
            ServiceObject.GetContainerandRateID(objInboundInquiry);
            //END
            objInboundInquiry.CNTR_CHECK = "RATE_ID";
            ServiceObject.GetContainerandRateID(objInboundInquiry);
            objInboundInquiry.CNTR_CHECK = "GET_WGTCUBE";
            ServiceObject.GetContainerandRateID(objInboundInquiry);
            if (objInboundInquiry.ListGetWgtCubeValue.Count() != 0)
            {
                objInboundInquiry.CNTR_WEIGHT = objInboundInquiry.ListGetWgtCubeValue[0].CNTR_WEIGHT;
                objInboundInquiry.CNTR_CUBE = objInboundInquiry.ListGetWgtCubeValue[0].CNTR_CUBE;
            }
            //END
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_DocReceivingPost", InboundInquiryModel);
        }
        public ActionResult SaveReceivingPost(string p_str_cmp_id, string p_str_ibdocid, string P_STR_CONTAINERID, string P_STR_CNTR_PALLET, string P_STR_CNTR_WEIGHT
     , string P_STR_CNTR_CUBE, string P_STR_RATEID, string P_STR_CNTR_NOTE, string P_STR_CNTR_PO_NUM, string P_STR_CNTR_ST_RATE_ID, string P_STR_BILL_TYPE)
        {
            string l_str_ibdocid;



            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.ibdocid = p_str_ibdocid;
            objInboundInquiry.bill_inout_type = P_STR_BILL_TYPE;
            objInboundInquiry.ListRcvgPost = TempData["TempListDetail"] as List<InboundInquiry>;
            l_str_ibdocid = objInboundInquiry.ibdocid.Substring(0, 4).ToString().Trim();
            if (l_str_ibdocid.Equals("9999"))
            {

                for (int i = 0; i < objInboundInquiry.ListRcvgPost.Count(); i++)
                {
                    objInboundInquiry.cmp_id = objInboundInquiry.ListRcvgPost[i].cmp_id;
                    objInboundInquiry.ibdocid = objInboundInquiry.ListRcvgPost[i].ib_doc_id;
                    objInboundInquiry.lot_id = objInboundInquiry.ListRcvgPost[i].lot_id;
                    objInboundInquiry.palet_id = objInboundInquiry.ListRcvgPost[i].palet_id;
                    objInboundInquiry.loc_id = objInboundInquiry.ListRcvgPost[i].loc_id;
                    objInboundInquiry = ServiceObject.CanPost(objInboundInquiry);
                    if (objInboundInquiry.ListCanPost[0].tran_status.Trim() == "ORIG")
                    {
                        ServiceObject.ReceivingPost9999Dtls(objInboundInquiry);
                    }
                }

            }

            ServiceObject.ReceivingPostDtls(objInboundInquiry);
            if (objInboundInquiry.bill_inout_type == "Container")            // CR_3PL_MVC_BL_2018_0312_002 Added By SONIYA
            {
                objInboundInquiry.CONTAINERID = P_STR_CONTAINERID;
                objInboundInquiry.RATEID = P_STR_RATEID;
                objInboundInquiry.CNTR_PALLET = P_STR_CNTR_PALLET;
                objInboundInquiry.CNTR_WEIGHT = P_STR_CNTR_WEIGHT;
                objInboundInquiry.CNTR_CUBE = P_STR_CNTR_CUBE;
                objInboundInquiry.CNTR_NOTE = P_STR_CNTR_NOTE;
                //CR_3PL_MVC_BL_2018_0224_001 Added By Ravi 24-02-2018
                objInboundInquiry.po_num = P_STR_CNTR_PO_NUM;
                objInboundInquiry.st_rate_id = P_STR_CNTR_ST_RATE_ID;
                //END
                ServiceObject.InsertCONTAINERRecvDetails(objInboundInquiry);
            }
            else
            {
                objInboundInquiry.cmp_id = p_str_cmp_id;
                objInboundInquiry.ib_doc_id = p_str_ibdocid;
                ServiceObject.Update_Lot_Bill_Status(objInboundInquiry);
            }
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("~/Views/InboundInquiry/InboundInquiry.cshtml", InboundInquiryModel);
        }
        public ActionResult ReceivingUnPost(string CmpId, string id, string Cont_id, string LotId, string datefrom, string dateto)
        {
            int l_int_tot_ctn = 0;
            int l_int_tot_qty = 0;
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objInboundInquiry.DocumentdateFrom = datefrom;
            objInboundInquiry.DocumentdateTo = dateto;
            objInboundInquiry.cmp_id = CmpId;
            objInboundInquiry.ibdocid = id;
            objInboundInquiry.cntr_id = Cont_id;
            objInboundInquiry.lot_id = LotId;
            objInboundInquiry = ServiceObject.GetRcvngUnPostDtls(objInboundInquiry);

            if (objInboundInquiry.ListRcvgUnPost.Count != 0)
            {
                for (int i = 0; i < objInboundInquiry.ListRcvgUnPost.Count(); i++)
                {
                    //objInboundInquiry.cmp_id = objInboundInquiry.ListRcvgUnPost[0].cmp_id.Trim();
                    //objInboundInquiry.ibdocid = objInboundInquiry.ListRcvgUnPost[0].ib_doc_id;
                    //objInboundInquiry.cont_id = objInboundInquiry.ListRcvgUnPost[0].cntr_id;
                    objInboundInquiry.ctns = objInboundInquiry.ListRcvgUnPost[i].tot_pkg;
                    objInboundInquiry.tot_qty = objInboundInquiry.ListRcvgUnPost[i].tot_qty;
                    l_int_tot_ctn = l_int_tot_ctn + objInboundInquiry.ctns;
                    l_int_tot_qty = l_int_tot_qty + objInboundInquiry.tot_qty;
                }
            }
            objInboundInquiry.ctns = l_int_tot_ctn;
            objInboundInquiry.tot_qty = l_int_tot_qty;
            objInboundInquiry = ServiceObject.LoadLotItem(objInboundInquiry);
            //TempData["TempListDetail"] = objInboundInquiry.ListRcvgPost;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany.cust_cmp_id = CmpId;
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objInboundInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objInboundInquiry.View_Flag = "UP";
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_DocReceivingPost", InboundInquiryModel);
        }
        public ActionResult SaveReceivingUnPost(string p_str_cmp_id, string p_str_ibdocid, string p_str_lotid)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.ibdocid = p_str_ibdocid;
            objInboundInquiry.lot_id = p_str_lotid;
            ServiceObject.DocReceivingUnPost(objInboundInquiry);
            //CR_3PL_MVC_BL_2018_0221_001 Added By Ravi 21-02-2018
            ServiceObject.Del_rcv_dtl(objInboundInquiry);
            //END                
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("~/Views/InboundInquiry/InboundInquiry.cshtml", InboundInquiryModel);
        }
        public ActionResult ShowDocRcvngUnPostReport(string p_str_cmpid, string p_str_lotid)
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
                    strReportName = "rpt_iv_doc_tally_sheet_direct_tpw_hst_temp_rcevd_dtl.rpt";
                    InboundInquiry objInboundInquiry = new InboundInquiry();
                    InboundInquiryService ServiceObject = new InboundInquiryService();
                    ReportDocument rd = new ReportDocument();
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                    objInboundInquiry.cmp_id = p_str_cmpid;
                    objInboundInquiry.lot_id = p_str_lotid;
                    objInboundInquiry = ServiceObject.DocTallySheetRpt(objInboundInquiry);
                    var rptSource = objInboundInquiry.ListDocTallySheetRpt.ToList();
                    rd.Load(strRptPath);
                    int AlocCount = 0;
                    AlocCount = objInboundInquiry.ListDocTallySheetRpt.Count();
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
        public FileResult downloadFile()
        {
            return File("~\\uploads\\IB_CONTAINER_UPLOAD.csv", "text/csv", string.Format("Sample-{0}.csv", DateTime.Now.ToString("yyyyMMdd-HHmmss")));
        }
        public JsonResult ItemXGetitmDtl(string term, string cmp_id)
        {
            OutboundInqService ServiceObject = new OutboundInqService();
            var List = ServiceObject.ItemXGetitmDetails(term, cmp_id).LstItmxCustdtl.Select(x => new { label = x.Itmdtl, value = x.itm_num, itm_num = x.itm_num, itm_color = x.itm_color, itm_size = x.itm_size, itm_name = x.itm_name, itm_code = x.itm_code }).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ItemXGetIBinqitmDtl(string term, string cmp_id)
        {
            InboundInquiryService ServiceObject = new InboundInquiryService();
            var List = ServiceObject.ItemXGetIBitmDetails(term, cmp_id).LstItmxCustdtl.Select(x => new { label = x.Itmdtl, value = x.itm_num, itm_num = x.itm_num, itm_color = x.itm_color, itm_size = x.itm_size, itm_name = x.itm_name, itm_code = x.itm_code }).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveDocEntryEdit(string p_str_cmp_id, string p_str_ibdocid, string p_str_ibdocdt, string p_str_status, string p_str_cont_id,
     string p_str_eta_dt, string p_str_refno, string p_str_ordrdt, string p_str_rcvdvia, string p_str_rcvdfm, string p_str_bol, string p_str_vessel_num
      , string p_str_note)
        {
            bool l_str_include_entry_dtls = false;
            int l_int_prev_ctn_line = 0;
            int l_int_prev_next_line = 0;
            //int l_int_prev_ordr_ctn = 0;
            //int l_int_next_ordr_ctn = 0;
            //int l_int_prev_ordr_qty = 0;
            //int l_int_next_ordr_qty = 0;
            //int dtlline = 0;
            string l_str_itmcode = string.Empty;  //CR_3PL_MVC_BL_2018_0305_001
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            Session["tempIbdocId"] = objInboundInquiry.ib_doc_id;
            objInboundInquiry.ib_doc_id = p_str_ibdocid;
            if (p_str_cont_id == "")
            {
                objInboundInquiry.cont_id = "-";
            }
            if (p_str_refno == "")
            {
                objInboundInquiry.refno = "-";
            }
            if (p_str_rcvdvia == "")
            {
                objInboundInquiry.recvdvia = "-";
            }
            if (p_str_vessel_num == "")
            {
                objInboundInquiry.vessel_num = "-";
            }
            if (p_str_rcvdfm == "")
            {
                objInboundInquiry.recvd_fm = "-";
            }
            if (p_str_bol == "")
            {
                objInboundInquiry.bol = "-";
            }
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.ib_doc_id = p_str_ibdocid;
            objInboundInquiry = ServiceObject.GetDocuEntryTempGridDtl(objInboundInquiry);
            if (objInboundInquiry.ListDocdtl.Count() == 0)
            {
                l_str_include_entry_dtls = false;
            }
            else
            {
                objInboundInquiry.cmp_id = p_str_cmp_id;
                objInboundInquiry.ib_doc_id = p_str_ibdocid;
                ServiceObject.Del_doc_Dtl(objInboundInquiry);
                ServiceObject.GetDeleteDocCtn(objInboundInquiry);
                for (int i = 0; i < objInboundInquiry.ListDocdtl.Count(); i++)
                {
                    objInboundInquiry.cmp_id = p_str_cmp_id;
                    objInboundInquiry.ib_doc_id = p_str_ibdocid;
                    objInboundInquiry.LineNum = objInboundInquiry.ListDocdtl[i].line_num;
                    objInboundInquiry.itm_num = objInboundInquiry.ListDocdtl[i].itm_num;
                    objInboundInquiry.itm_color = objInboundInquiry.ListDocdtl[i].itm_color;
                    objInboundInquiry.itm_size = objInboundInquiry.ListDocdtl[i].itm_size;
                    objInboundInquiry.itm_name = objInboundInquiry.ListDocdtl[i].itm_name;
                    objInboundInquiry.itm_code = objInboundInquiry.ListDocdtl[i].itm_code;
                    objInboundInquiry.length = objInboundInquiry.ListDocdtl[i].length;
                    objInboundInquiry.width = objInboundInquiry.ListDocdtl[i].width;
                    objInboundInquiry.height = objInboundInquiry.ListDocdtl[i].height;
                    objInboundInquiry.weight = objInboundInquiry.ListDocdtl[i].weight;
                    objInboundInquiry.cube = objInboundInquiry.ListDocdtl[i].cube;
                    objInboundInquiry.ctn_qty = objInboundInquiry.ListDocdtl[i].ppk;
                    objInboundInquiry = ServiceObject.CheckItmHdr(objInboundInquiry);

                    if (objInboundInquiry.ListStyle.Count() == 0)
                    {
                        //CR_3PL_MVC_BL_2018_0305_001
                        objInboundInquiry = ServiceObject.GetItmId(objInboundInquiry);
                        objInboundInquiry.itmid = objInboundInquiry.itm_code;
                        l_str_itmcode = objInboundInquiry.itm_code;
                        objInboundInquiry.itm_code = l_str_itmcode;
                        //CR_3PL_MVC_BL_2018_0305_001
                        objInboundInquiry.flag = "A";

                        ServiceObject.Add_Style_To_Itm_dtl(objInboundInquiry);
                        ServiceObject.Add_Style_To_Itm_hdr(objInboundInquiry);
                    }
                    else
                    {
                        objInboundInquiry.itm_num = objInboundInquiry.ListDocdtl[i].itm_num;
                        objInboundInquiry.itm_color = objInboundInquiry.ListDocdtl[i].itm_color;
                        objInboundInquiry.itm_size = objInboundInquiry.ListDocdtl[i].itm_size;
                        objInboundInquiry.itm_name = objInboundInquiry.ListDocdtl[i].itm_name;
                        objInboundInquiry.itm_code = objInboundInquiry.ListDocdtl[i].itm_code;
                    }
                    objInboundInquiry.ord_qty = objInboundInquiry.ListDocdtl[i].ordr_qty;
                    objInboundInquiry.docuppk = objInboundInquiry.ListDocdtl[i].ppk;
                    objInboundInquiry.ctn_qty = objInboundInquiry.ListDocdtl[i].ctns;
                    objInboundInquiry.ctn_line = objInboundInquiry.ListDocdtl[i].ctn_line;
                    objInboundInquiry.po_num = objInboundInquiry.ListDocdtl[i].po_num;
                    objInboundInquiry.loc_id = objInboundInquiry.ListDocdtl[i].loc_id;
                    objInboundInquiry.strg_rate = objInboundInquiry.ListDocdtl[i].strg_rate;
                    objInboundInquiry.inout_rate = objInboundInquiry.ListDocdtl[i].inout_rate;
                    objInboundInquiry.note = objInboundInquiry.ListDocdtl[i].note;
                    objInboundInquiry.length = objInboundInquiry.ListDocdtl[i].length;
                    objInboundInquiry.height = objInboundInquiry.ListDocdtl[i].height;
                    objInboundInquiry.cube = objInboundInquiry.ListDocdtl[i].cube;
                    objInboundInquiry.width = objInboundInquiry.ListDocdtl[i].width;
                    objInboundInquiry.weight = objInboundInquiry.ListDocdtl[i].weight;

                    l_int_prev_next_line = objInboundInquiry.ListDocdtl[i].line_num;

                    if (l_int_prev_ctn_line != l_int_prev_next_line)
                    {

                        ServiceObject.Add_To_proc_save_doc_dtl(objInboundInquiry);
                    }
                    else
                    {
                        ServiceObject.UpdateTblIbDocDtl(objInboundInquiry);
                    }
                    ServiceObject.Add_To_proc_save_doc_ctn(objInboundInquiry);

                    l_int_prev_ctn_line = l_int_prev_next_line;

                    //ServiceObject.Add_To_proc_save_doc_dtl(objInboundInquiry);
                    //ServiceObject.Add_To_proc_save_doc_ctn(objInboundInquiry);
                }
                if (p_str_eta_dt == "")
                {
                    objInboundInquiry.eta_dt = "";
                }
                else
                {
                    objInboundInquiry.eta_dt = p_str_eta_dt;
                }
                if (p_str_cont_id == "")
                {
                    objInboundInquiry.cont_id = "-";
                }
                else
                {
                    objInboundInquiry.cont_id = p_str_cont_id;
                }
                if (p_str_refno == "")
                {
                    objInboundInquiry.refno = "-";
                }
                else
                {
                    objInboundInquiry.refno = p_str_refno;
                }
                if (p_str_rcvdvia == "")
                {
                    objInboundInquiry.recvdvia = "-";
                }
                else
                {
                    objInboundInquiry.recvdvia = p_str_rcvdvia;
                }
                if (p_str_vessel_num == "")
                {
                    objInboundInquiry.vessel_num = "-";
                }
                else
                {
                    objInboundInquiry.vessel_num = p_str_vessel_num;
                }
                if (p_str_rcvdfm == "")
                {
                    objInboundInquiry.recvd_fm = "-";
                }
                else
                {
                    objInboundInquiry.recvd_fm = p_str_rcvdfm;
                }
                if (p_str_bol == "")
                {
                    objInboundInquiry.bol = "-";
                }
                else
                {
                    objInboundInquiry.bol = p_str_bol;
                }
                if (p_str_note == "")
                {
                    objInboundInquiry.Note = "Manual Doc:";
                }
                else
                {
                    objInboundInquiry.Note = p_str_note;
                }
                objInboundInquiry.cmp_id = p_str_cmp_id;
                objInboundInquiry.ib_doc_id = p_str_ibdocid;
                objInboundInquiry.ib_doc_dt = p_str_ibdocdt;
                objInboundInquiry.status = p_str_status;
                //objInboundInquiry.cont_id = p_str_cont_id;
                objInboundInquiry.eta_dt = p_str_eta_dt;
                //objInboundInquiry.refno = p_str_refno;
                objInboundInquiry.orderdt = p_str_ordrdt;
                //objInboundInquiry.recvd_fm = p_str_rcvdfm;
                //objInboundInquiry.recvdvia = p_str_rcvdvia;
                //objInboundInquiry.bol = p_str_bol;
                //objInboundInquiry.vessel_num = p_str_vessel_num;
                //objInboundInquiry.Note = p_str_note;
                ServiceObject.UpdateTblIbDocHdr(objInboundInquiry);
                ServiceObject.GetDeleteTempData(objInboundInquiry);

                l_str_include_entry_dtls = true;

            }
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            if (objInboundInquiry.SucessStatus == "0")
            {
                return PartialView("~/Views/InboundInquiry/InboundInquiry.cshtml", InboundInquiryModel);
            }
            else
            {
                return Json(l_str_include_entry_dtls, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SaveDocEntry(string p_str_cmp_id, string p_str_ibdocid, string p_str_ibdocdt, string p_str_status, string p_str_cont_id,
       string p_str_eta_dt, string p_str_refno, string p_str_ordrdt, string p_str_rcvdvia, string p_str_rcvdfm, string p_str_bol, string p_str_vessel_num
        , string p_str_note)
        {
            bool l_str_include_entry_dtls = false;
            int l_int_prev_ctn_line = 0;
            int l_int_prev_next_line = 0;
            //int l_int_prev_ordr_ctn = 0;
            //int l_int_next_ordr_ctn = 0;
            //int l_int_prev_ordr_qty = 0;
            //int l_int_next_ordr_qty = 0;
            string l_str_itmcode = string.Empty;  //CR_3PL_MVC_BL_2018_0305_001


            //int dtlline = 0;
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            Session["tempIbdocId"] = objInboundInquiry.ib_doc_id;
            objInboundInquiry.ib_doc_id = p_str_ibdocid;
            if (p_str_cont_id == "")
            {
                objInboundInquiry.cont_id = "-";
            }
            if (p_str_refno == "")
            {
                objInboundInquiry.refno = "-";
            }
            if (p_str_rcvdvia == "")
            {
                objInboundInquiry.recvdvia = "-";
            }
            if (p_str_vessel_num == "")
            {
                objInboundInquiry.vessel_num = "-";
            }
            if (p_str_rcvdfm == "")
            {
                objInboundInquiry.recvd_fm = "-";
            }
            if (p_str_bol == "")
            {
                objInboundInquiry.bol = "-";
            }
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.ib_doc_id = p_str_ibdocid;
            objInboundInquiry = ServiceObject.GetDocuEntryTempGridDtl(objInboundInquiry);
            if (objInboundInquiry.ListDocdtl.Count() == 0)
            {
                l_str_include_entry_dtls = false;
            }
            else
            {
                objInboundInquiry.cmp_id = p_str_cmp_id;
                objInboundInquiry.ib_doc_id = p_str_ibdocid;
                ServiceObject.Del_doc_Dtl(objInboundInquiry);
                ServiceObject.GetDeleteDocCtn(objInboundInquiry);
                for (int i = 0; i < objInboundInquiry.ListDocdtl.Count(); i++)
                {
                    objInboundInquiry.cmp_id = p_str_cmp_id;
                    objInboundInquiry.ib_doc_id = p_str_ibdocid;
                    objInboundInquiry.LineNum = objInboundInquiry.ListDocdtl[i].line_num;
                    objInboundInquiry.itm_num = objInboundInquiry.ListDocdtl[i].itm_num;
                    objInboundInquiry.itm_color = objInboundInquiry.ListDocdtl[i].itm_color;
                    objInboundInquiry.itm_size = objInboundInquiry.ListDocdtl[i].itm_size;
                    objInboundInquiry.itm_name = objInboundInquiry.ListDocdtl[i].itm_name;
                    objInboundInquiry.itm_code = objInboundInquiry.ListDocdtl[i].itm_code;
                    objInboundInquiry.length = objInboundInquiry.ListDocdtl[i].length;
                    objInboundInquiry.width = objInboundInquiry.ListDocdtl[i].width;
                    objInboundInquiry.height = objInboundInquiry.ListDocdtl[i].height;
                    objInboundInquiry.weight = objInboundInquiry.ListDocdtl[i].weight;
                    objInboundInquiry.cube = objInboundInquiry.ListDocdtl[i].cube;
                    objInboundInquiry.ctn_qty = objInboundInquiry.ListDocdtl[i].ppk;
                    objInboundInquiry.ctn_line = objInboundInquiry.ListDocdtl[i].ctn_line;
                    objInboundInquiry = ServiceObject.CheckItmHdr(objInboundInquiry);

                    if (objInboundInquiry.ListStyle.Count() == 0)
                    {
                        //objInboundInquiry = ServiceObject.GetItmId(objInboundInquiry);
                        //objInboundInquiry.itm_code = objInboundInquiry.Itm_Code;
                        //l_str_itmcode = objInboundInquiry.Itm_Code;
                        //objInboundInquiry.itm_code = l_str_itmcode;
                        //CR_3PL_MVC_BL_2018_0305_001
                        objInboundInquiry = ServiceObject.GetItmId(objInboundInquiry);
                        objInboundInquiry.itmid = objInboundInquiry.itm_code;
                        l_str_itmcode = objInboundInquiry.itm_code;
                        objInboundInquiry.itm_code = l_str_itmcode;
                        //CR_3PL_MVC_BL_2018_0305_001
                        objInboundInquiry.flag = "A";

                        ServiceObject.Add_Style_To_Itm_dtl(objInboundInquiry);
                        ServiceObject.Add_Style_To_Itm_hdr(objInboundInquiry);
                    }
                    else
                    {
                        objInboundInquiry.itm_num = objInboundInquiry.ListDocdtl[i].itm_num;
                        objInboundInquiry.itm_color = objInboundInquiry.ListDocdtl[i].itm_color;
                        objInboundInquiry.itm_size = objInboundInquiry.ListDocdtl[i].itm_size;
                        objInboundInquiry.itm_name = objInboundInquiry.ListDocdtl[i].itm_name;
                        objInboundInquiry.itm_code = objInboundInquiry.ListDocdtl[i].itm_code;
                    }
                    objInboundInquiry.ord_qty = objInboundInquiry.ListDocdtl[i].ordr_qty;
                    objInboundInquiry.docuppk = objInboundInquiry.ListDocdtl[i].ppk;
                    objInboundInquiry.ctn_qty = objInboundInquiry.ListDocdtl[i].ctns;
                    objInboundInquiry.po_num = objInboundInquiry.ListDocdtl[i].po_num;
                    objInboundInquiry.loc_id = objInboundInquiry.ListDocdtl[i].loc_id;
                    objInboundInquiry.strg_rate = objInboundInquiry.ListDocdtl[i].strg_rate;
                    objInboundInquiry.inout_rate = objInboundInquiry.ListDocdtl[i].inout_rate; ;
                    objInboundInquiry.note = objInboundInquiry.ListDocdtl[i].note;
                    objInboundInquiry.length = objInboundInquiry.ListDocdtl[i].length;
                    objInboundInquiry.height = objInboundInquiry.ListDocdtl[i].height;
                    objInboundInquiry.cube = objInboundInquiry.ListDocdtl[i].cube;
                    objInboundInquiry.width = objInboundInquiry.ListDocdtl[i].width;
                    objInboundInquiry.weight = objInboundInquiry.ListDocdtl[i].weight;
                    l_int_prev_next_line = objInboundInquiry.ListDocdtl[i].line_num;

                    if (l_int_prev_ctn_line != l_int_prev_next_line)
                    {

                        ServiceObject.Add_To_proc_save_doc_dtl(objInboundInquiry);
                    }
                    else
                    {
                        ServiceObject.UpdateTblIbDocDtl(objInboundInquiry);

                    }
                    ServiceObject.Add_To_proc_save_doc_ctn(objInboundInquiry);

                    l_int_prev_ctn_line = l_int_prev_next_line;

                }
                if (p_str_cont_id == "")
                {
                    objInboundInquiry.cont_id = "-";
                }
                else
                {
                    objInboundInquiry.cont_id = p_str_cont_id;
                }
                if (p_str_refno == "")
                {
                    objInboundInquiry.refno = "-";
                }
                else
                {
                    objInboundInquiry.refno = p_str_refno;
                }
                if (p_str_rcvdvia == "")
                {
                    objInboundInquiry.recvdvia = "-";
                }
                else
                {
                    objInboundInquiry.recvdvia = p_str_rcvdvia;
                }
                if (p_str_vessel_num == "")
                {
                    objInboundInquiry.vessel_num = "-";
                }
                else
                {
                    objInboundInquiry.vessel_num = p_str_vessel_num;
                }
                if (p_str_rcvdfm == "")
                {
                    objInboundInquiry.recvd_fm = "-";
                }
                else
                {
                    objInboundInquiry.recvd_fm = p_str_rcvdfm;
                }
                if (p_str_bol == "")
                {
                    objInboundInquiry.bol = "-";
                }
                else
                {
                    objInboundInquiry.bol = p_str_bol;
                }
                if (p_str_note == "")
                {
                    objInboundInquiry.Note = "Manual Doc:";
                }
                else
                {
                    objInboundInquiry.Note = p_str_note;
                }
                objInboundInquiry.cmp_id = p_str_cmp_id;
                objInboundInquiry.ib_doc_id = p_str_ibdocid;
                objInboundInquiry.ib_doc_dt = p_str_ibdocdt;
                objInboundInquiry.status = p_str_status;
                //objInboundInquiry.cont_id = p_str_cont_id;
                objInboundInquiry.eta_dt = p_str_eta_dt;
                //objInboundInquiry.refno = p_str_refno;
                objInboundInquiry.orderdt = p_str_ordrdt;
                //objInboundInquiry.recvd_fm = p_str_rcvdfm;
                //objInboundInquiry.recvdvia = p_str_rcvdvia;
                //objInboundInquiry.bol = p_str_bol;
                //objInboundInquiry.vessel_num = p_str_vessel_num;
                //objInboundInquiry.Note = p_str_note;
                ServiceObject.Add_To_proc_save_doc_hdr(objInboundInquiry);
                ServiceObject.GetDeleteTempData(objInboundInquiry);

                l_str_include_entry_dtls = true;

            }
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            if (objInboundInquiry.SucessStatus == "0")
            {
                return PartialView("~/Views/InboundInquiry/InboundInquiry.cshtml", InboundInquiryModel);
            }
            else
            {
                return Json(l_str_include_entry_dtls, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult SaveRecvDocEntry(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_rcvd_dt, string p_str_rcvd_from, string p_str_refno,
       string p_str_vend_id, string p_str_whs_id, string p_str_cont_id, string p_str_seal_num, string p_str_palet_id, string p_str_lot_id)
        {
            int ResultCount = 0;

            string l_str_rcvd_itm_mode = string.Empty;

            //int dtlline = 0;
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            Session["tempIbdocId"] = objInboundInquiry.ib_doc_id;
            objInboundInquiry.ib_doc_id = p_str_ib_doc_id;
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.ib_doc_id = p_str_ib_doc_id;
            objInboundInquiry.ib_doc_dt = p_str_rcvd_dt;
            objInboundInquiry.rcvd_from = p_str_rcvd_from;

            //objInboundInquiry.refno = p_str_refno;
            objInboundInquiry.vend_id = p_str_vend_id;

            objInboundInquiry.whs_id = p_str_whs_id;
            objInboundInquiry.cont_id = p_str_cont_id;
            objInboundInquiry.refno = p_str_refno;
            //objInboundInquiry.seal_num = p_str_seal_num;
            objInboundInquiry.palet_dt = p_str_palet_id;
            //objInboundInquiry.lot_id = p_str_lot_id;
            objInboundInquiry.seal_num = p_str_seal_num;
            if (p_str_cont_id == "")
            {
                objInboundInquiry.cont_id = "-";
            }
            if (p_str_refno == "")
            {
                objInboundInquiry.refno = "-";
            }
            if (p_str_rcvd_from == "")
            {
                objInboundInquiry.recvdvia = "-";
            }
            if (p_str_whs_id == "")
            {
                objInboundInquiry.whs_id = "-";
            }
            if (p_str_seal_num == "")
            {
                objInboundInquiry.seal_num = "-";
            }
            //if (p_str_palet_id == "")
            //{
            //    objInboundInquiry.palet_id = "-";
            //}
            if (p_str_lot_id == "NEW")
            {
                objInboundInquiry.lot_id = "";
            }
            else
            {
                objInboundInquiry.lot_id = p_str_lot_id;
            }
            if (p_str_palet_id == "NEW")
            {
                objInboundInquiry.palet_id = "";
            }
            objInboundInquiry = ServiceObject.LoadCustConfigRcvdItmMode(objInboundInquiry);
            if (objInboundInquiry.ListCustConfigRcvdItmModeDetails.Count() != 0)
            {
                l_str_rcvd_itm_mode = objInboundInquiry.ListCustConfigRcvdItmModeDetails[0].Recv_Itm_Mode;
            }

            objInboundInquiry = ServiceObject.GetRecvTempTableCount(objInboundInquiry);

            //   objInboundInquiry = ServiceObject.GetPalletId(objInboundInquiry);
            if (objInboundInquiry.rcvd_dtl_count == 0)
            {
                return Json(ResultCount, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (l_str_rcvd_itm_mode == "PO")
                {
                    objInboundInquiry = ServiceObject.GetPalletId(objInboundInquiry);

                    objInboundInquiry = ServiceObject.GetDocRecvEntrySaveByPo(objInboundInquiry);
                    objInboundInquiry = ServiceObject.GetUpdateRcvdStatus(objInboundInquiry);

                }
                else if (l_str_rcvd_itm_mode == "ITEM")
                {
                    objInboundInquiry = ServiceObject.GetPalletId(objInboundInquiry);

                    objInboundInquiry = ServiceObject.GetDocRecvEntrySaveByItem(objInboundInquiry);
                    objInboundInquiry = ServiceObject.GetUpdateRcvdStatus(objInboundInquiry);

                }
                else if (l_str_rcvd_itm_mode == "CUST LOT ID")
                {
                    objInboundInquiry.lot_id = p_str_lot_id;
                    objInboundInquiry = ServiceObject.GetPalletId(objInboundInquiry);

                    objInboundInquiry = ServiceObject.GetDocRecvEntrySaveByLotID(objInboundInquiry);
                    objInboundInquiry = ServiceObject.GetUpdateRcvdStatus(objInboundInquiry);
                }
                else
                {
                    objInboundInquiry = ServiceObject.GetPalletId(objInboundInquiry);

                    objInboundInquiry = ServiceObject.GetDocRecvEntrySave(objInboundInquiry);
                    objInboundInquiry = ServiceObject.GetUpdateRcvdStatus(objInboundInquiry);
                }
            }

            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            //if (objInboundInquiry.SucessStatus == "0")
            //{
            return PartialView("~/Views/InboundInquiry/InboundInquiry.cshtml", InboundInquiryModel);
            //}
            //else
            //{
            //    return Json(ResultCount, JsonRequestBehavior.AllowGet);
            //}
        }
        [HttpPost]
        public ActionResult btnDocEdit(string p_str_cmp_id, string p_str_ibdocid, string p_str_ibdocdt, string p_str_status, string p_str_cont_id,
        string p_str_eta_dt, string p_str_refno, string p_str_ordrdt, string p_str_rcvdvia, string p_str_rcvdfm, string p_str_bol, string p_str_vessel_num
         , string p_str_note)
        {
            string l_str_itmcode;
            //int dtlline = 0;
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.ib_doc_id = p_str_ibdocid;
            objInboundInquiry.ib_doc_dt = p_str_ibdocdt;
            objInboundInquiry.status = p_str_status;
            objInboundInquiry.cont_id = p_str_cont_id;
            objInboundInquiry.eta_dt = p_str_eta_dt;
            objInboundInquiry.refno = p_str_refno;
            objInboundInquiry.orderdt = p_str_ordrdt;
            objInboundInquiry.recvd_fm = p_str_rcvdfm;
            objInboundInquiry.recvdvia = p_str_rcvdvia;
            objInboundInquiry.bol = p_str_bol;
            objInboundInquiry.vessel_num = p_str_vessel_num;
            objInboundInquiry.Note = p_str_note;
            ServiceObject.Update_doc_hdr(objInboundInquiry);
            ServiceObject.Del_doc_Dtl(objInboundInquiry);
            objInboundInquiry = ServiceObject.GetDocuEntryTempGridDtl(objInboundInquiry);

            for (int i = 0; i < objInboundInquiry.ListDocdtl.Count(); i++)
            {
                objInboundInquiry.cmp_id = p_str_cmp_id;
                objInboundInquiry.ib_doc_id = p_str_ibdocid;
                objInboundInquiry.LineNum = objInboundInquiry.ListDocdtl[i].line_num;
                objInboundInquiry.itm_num = objInboundInquiry.ListDocdtl[i].itm_num;
                objInboundInquiry.itm_color = objInboundInquiry.ListDocdtl[i].itm_color;
                objInboundInquiry.itm_size = objInboundInquiry.ListDocdtl[i].itm_size;
                objInboundInquiry.itm_name = objInboundInquiry.ListDocdtl[i].itm_name;
                objInboundInquiry = ServiceObject.GetItmId(objInboundInquiry);
                objInboundInquiry.itm_code = objInboundInquiry.Itm_Code;
                l_str_itmcode = objInboundInquiry.Itm_Code;
                objInboundInquiry.itm_code = l_str_itmcode;
                objInboundInquiry = ServiceObject.CheckItmHdr(objInboundInquiry);
                if (objInboundInquiry.ListStyle.Count() == 0)
                {
                    objInboundInquiry.flag = "A";

                    ServiceObject.Add_Style_To_Itm_dtl(objInboundInquiry);
                    ServiceObject.Add_Style_To_Itm_hdr(objInboundInquiry);
                }
                else
                {
                    objInboundInquiry.itm_num = objInboundInquiry.ListDocdtl[i].itm_num;
                    objInboundInquiry.itm_color = objInboundInquiry.ListDocdtl[i].itm_color;
                    objInboundInquiry.itm_size = objInboundInquiry.ListDocdtl[i].itm_size;
                    objInboundInquiry.itm_name = objInboundInquiry.ListDocdtl[i].itm_name;
                }
                objInboundInquiry.ord_qty = objInboundInquiry.ListDocdtl[i].ordr_qty;
                objInboundInquiry.docuppk = objInboundInquiry.ListDocdtl[i].ppk;
                objInboundInquiry.ctn_qty = objInboundInquiry.ListDocdtl[i].ctns;
                objInboundInquiry.po_num = objInboundInquiry.ListDocdtl[i].po_num;
                objInboundInquiry.loc_id = objInboundInquiry.ListDocdtl[i].loc_id;
                objInboundInquiry.strg_rate = objInboundInquiry.ListDocdtl[i].strg_rate;
                objInboundInquiry.inout_rate = objInboundInquiry.ListDocdtl[i].inout_rate; ;
                objInboundInquiry.note = objInboundInquiry.ListDocdtl[i].note;
                objInboundInquiry.length = objInboundInquiry.ListDocdtl[i].length;
                objInboundInquiry.height = objInboundInquiry.ListDocdtl[i].height;
                objInboundInquiry.cube = objInboundInquiry.ListDocdtl[i].cube;
                objInboundInquiry.width = objInboundInquiry.ListDocdtl[i].width;
                objInboundInquiry.weight = objInboundInquiry.ListDocdtl[i].weight;
                ServiceObject.Add_To_proc_save_doc_dtl(objInboundInquiry);
                ServiceObject.Add_To_proc_save_doc_ctn(objInboundInquiry);


            }

            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("~/Views/InboundInquiry/InboundInquiry.cshtml", InboundInquiryModel);
        }

        private static DataTable ProcessCSV(string fileName)
        {
            InboundInquiryModel objInboundInquiryModel = new InboundInquiryModel();
            IInboundInquiryService ServiceObject = new InboundInquiryService();

            InboundInquiry objInboundInquiry = new InboundInquiry();
            IInboundInquiryService ServiceObjects = new InboundInquiryService();
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            //ServiceObject.TruncateTempDocEntry(objInboundInquiry);     //CR_3PL_MVC_BL_2018_0306_001 
            ServiceObject.TruncateTempDocUpload(objInboundInquiry);
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
                    sqlBulkCopy.DestinationTableName = "dbo.temp_Ib_doc_upload";

                    //[OPTIONAL]: Map the Excel columns with that of the database table
                    sqlBulkCopy.ColumnMappings.Add("Column1", "cmp_id");
                    sqlBulkCopy.ColumnMappings.Add("Column2", "cntr_id");
                    sqlBulkCopy.ColumnMappings.Add("Column3", "dtl_line");
                    sqlBulkCopy.ColumnMappings.Add("Column4", "po_num");
                    sqlBulkCopy.ColumnMappings.Add("Column5", "itm_num");
                    sqlBulkCopy.ColumnMappings.Add("Column6", "itm_color");
                    sqlBulkCopy.ColumnMappings.Add("Column7", "itm_size");
                    sqlBulkCopy.ColumnMappings.Add("Column8", "itm_name");
                    sqlBulkCopy.ColumnMappings.Add("Column9", "ordr_qty");
                    sqlBulkCopy.ColumnMappings.Add("Column10", "ctn_qty");
                    sqlBulkCopy.ColumnMappings.Add("Column11", "ordr_ctn");
                    sqlBulkCopy.ColumnMappings.Add("Column12", "loc_id");
                    sqlBulkCopy.ColumnMappings.Add("Column13", "st_rate_id");
                    sqlBulkCopy.ColumnMappings.Add("Column14", "io_rate_id");
                    sqlBulkCopy.ColumnMappings.Add("Column15", "ctn_len");
                    sqlBulkCopy.ColumnMappings.Add("Column16", "ctn_width");
                    sqlBulkCopy.ColumnMappings.Add("Column17", "ctn_hgt");
                    sqlBulkCopy.ColumnMappings.Add("Column18", "ctn_cube");
                    sqlBulkCopy.ColumnMappings.Add("Column19", "ctn_wgt");
                    sqlBulkCopy.ColumnMappings.Add("Column20", "note");
                    conn.Open();
                    sqlBulkCopy.WriteToServer(dt);
                    Feedback = "Upload complete";
                    conn.Close();
                }
            }

            return Feedback;
        }
        public ActionResult UploadFiles(string l_str_new_cmp_id, string l_str_new_cont_id)
        {
            string l_str_cont_id = string.Empty; /*CR - 3PL_MVC_IB_2018 - 03 - 13 - 003*/
            string l_str_itmcode = string.Empty;
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

                        InboundInquiry objInboundInquiry = new InboundInquiry();
                        IInboundInquiryService ServiceObject = new InboundInquiryService();

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

                        InboundInquiry objInboundInqs = new InboundInquiry();
                        IInboundInquiryService ServiceObjects = new InboundInquiryService();
                        Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
                        InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);

                        ServiceObject.GetCSVList(objInboundInquiry);
                        for (int m = 0; m < objInboundInquiry.lstobjInboundInq.Count(); m++)
                        {

                            objInboundInquiry.line_num = Convert.ToInt32(objInboundInquiry.lstobjInboundInq[m].dtl_line);
                            objInboundInquiry.cmp_id = objInboundInquiry.lstobjInboundInq[m].cmp_id.Trim();
                            objInboundInquiry.ib_doc_id = Session["tempIbdocId"].ToString().Trim();
                            objInboundInquiry = ServiceObject.GetDocEntryId(objInboundInquiry);
                            objInboundInquiry.doc_entry_id = objInboundInquiry.ListDocId[0].doc_entry_id;
                            objInboundInquiry.ctn_line = 1; //CR-3PL_MVC_IB_2018_0312_003
                            objInboundInquiry.status = Session["tempStatus"].ToString().Trim();
                            objInboundInquiry.itm_num = objInboundInquiry.lstobjInboundInq[m].itm_num.Trim();
                            objInboundInquiry.itm_color = objInboundInquiry.lstobjInboundInq[m].itm_color.Trim();
                            objInboundInquiry.itm_size = objInboundInquiry.lstobjInboundInq[m].itm_size.Trim();
                            objInboundInquiry.itm_name = objInboundInquiry.lstobjInboundInq[m].itm_name.Trim();
                            objInboundInquiry.doclength = objInboundInquiry.lstobjInboundInq[m].ctn_len;//CR-3PL_MVC_IB_2018_0413_001
                            objInboundInquiry.docwidth = objInboundInquiry.lstobjInboundInq[m].ctn_width;
                            objInboundInquiry.docheight = objInboundInquiry.lstobjInboundInq[m].ctn_hgt;
                            objInboundInquiry.docweight = objInboundInquiry.lstobjInboundInq[m].ctn_wgt;
                            objInboundInquiry.doccube = objInboundInquiry.lstobjInboundInq[m].ctn_cube;
                            objInboundInquiry.length = objInboundInquiry.lstobjInboundInq[m].ctn_len;//CR-3PL_MVC_IB_2018_0413_001
                            objInboundInquiry.width = objInboundInquiry.lstobjInboundInq[m].ctn_width;
                            objInboundInquiry.height = objInboundInquiry.lstobjInboundInq[m].ctn_hgt;
                            objInboundInquiry.weight = objInboundInquiry.lstobjInboundInq[m].ctn_wgt;
                            objInboundInquiry.cube = objInboundInquiry.lstobjInboundInq[m].ctn_cube;
                            objInboundInquiry = ServiceObject.CheckItmHdr(objInboundInquiry);
                            if (objInboundInquiry.Check_exist_itm_count == "1")
                            {
                                objInboundInquiry.itm_code = objInboundInquiry.ListStyle[0].itm_code.Trim();
                                objInboundInquiry = ServiceObject.CheckItmDimension(objInboundInquiry);
                                objInboundInquiry.length = objInboundInquiry.lstobjInboundInq[m].length;
                                objInboundInquiry.width = objInboundInquiry.lstobjInboundInq[m].width;
                                objInboundInquiry.height = objInboundInquiry.lstobjInboundInq[m].depth;
                                objInboundInquiry.weight = objInboundInquiry.lstobjInboundInq[m].wgt;
                                //objInboundInquiry.itm_num = objInboundInquiry.lstobjInboundInq[m].itm_num;
                                //objInboundInquiry.itm_color = objInboundInquiry.lstobjInboundInq[m].itm_color;
                                //objInboundInquiry.itm_size = objInboundInquiry.lstobjInboundInq[m].itm_size;
                                //objInboundInquiry.itm_name = objInboundInquiry.lstobjInboundInq[m].itm_name;
                                objInboundInquiry.itm_code = objInboundInquiry.ListStyle[0].itm_code.Trim();
                            }
                            else
                            {
                                objInboundInquiry = ServiceObject.GetItmId(objInboundInquiry);
                                objInboundInquiry.itmid = objInboundInquiry.itm_code;
                                l_str_itmcode = objInboundInquiry.itm_code;
                                objInboundInquiry.itm_code = l_str_itmcode;
                                objInboundInquiry.flag = "A";

                                ServiceObject.Add_Style_To_Itm_dtl(objInboundInquiry);
                                ServiceObject.Add_Style_To_Itm_hdr(objInboundInquiry);
                            }

                            //if (objInboundInquiry.ListStyle.Count() == 0)
                            //{
                            //    ServiceObject.Add_Style_To_Itm_dtl(objInboundInquiry);
                            //    ServiceObject.Add_Style_To_Itm_hdr(objInboundInquiry);
                            //}
                            //else
                            //{
                            //    objInboundInquiry.itm_num = objInboundInquiry.lstobjInboundInq[m].itm_num;
                            //    objInboundInquiry.itm_color = objInboundInquiry.lstobjInboundInq[m].itm_color;
                            //    objInboundInquiry.itm_size = objInboundInquiry.lstobjInboundInq[m].itm_size;
                            //    objInboundInquiry.itm_name = objInboundInquiry.lstobjInboundInq[m].itm_name;
                            //    objInboundInquiry.itm_code = objInboundInquiry.ListStyle[0].itm_code.Trim();
                            //}
                            if (objInboundInquiry.lstobjInboundInq[m].itm_name == "-")
                            {
                                objInboundInquiry.itm_name = "-";
                            }
                            objInboundInquiry.docuppk = objInboundInquiry.lstobjInboundInq[m].ctn_qty;
                            objInboundInquiry.ord_qty = objInboundInquiry.lstobjInboundInq[m].ordr_qty;
                            objInboundInquiry.po_num = objInboundInquiry.lstobjInboundInq[m].po_num;
                            objInboundInquiry.ctn = objInboundInquiry.lstobjInboundInq[m].ordr_ctn;
                            objInboundInquiry.loc_id = objInboundInquiry.lstobjInboundInq[m].loc_id;
                            objInboundInquiry.strg_rate = objInboundInquiry.lstobjInboundInq[m].st_rate_id;
                            objInboundInquiry.inout_rate = objInboundInquiry.lstobjInboundInq[m].io_rate_id;
                            objInboundInquiry.doclength = objInboundInquiry.lstobjInboundInq[m].ctn_len;
                            objInboundInquiry.docwidth = objInboundInquiry.lstobjInboundInq[m].ctn_width;
                            objInboundInquiry.docheight = objInboundInquiry.lstobjInboundInq[m].ctn_hgt;
                            objInboundInquiry.docweight = objInboundInquiry.lstobjInboundInq[m].ctn_wgt;
                            objInboundInquiry.doccube = objInboundInquiry.lstobjInboundInq[m].ctn_cube;
                            objInboundInquiry.note = objInboundInquiry.lstobjInboundInq[m].note;
                            objInboundInquiry.cont_id = objInboundInquiry.lstobjInboundInq[m].cntr_id;
                            /*CR - 3PL_MVC_IB_2018 - 03 - 13 - 003*/
                            l_str_cont_id = objInboundInquiry.cont_id.Trim();
                            if (l_str_new_cont_id != l_str_cont_id)
                            {
                                return Json(new { data1 = "N", data2 = objInboundInquiry.line_num }, JsonRequestBehavior.AllowGet);
                            }
                            /*CR - 3PL_MVC_IB_2018 - 03 - 13 - 003*/
                            objInboundInquiry.itm_qty = objInboundInquiry.lstobjInboundInq[m].ctn_qty;
                            ServiceObject.InsertTempDocEntryDetails(objInboundInquiry);

                        }

                        objInboundInquiry = ServiceObject.GetDocumentEntryTempGridDtl(objInboundInquiry);
                        Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
                        InboundInquiryModel InboundInquiryDocModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
                        return PartialView("_DocEntryGrid", InboundInquiryDocModel);
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
        public ActionResult DocDelete(string p_str_cmp_id, string p_str_ibdocid)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.ib_doc_id = p_str_ibdocid;
            ServiceObject.DeleteDocEntry(objInboundInquiry);
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            int resultcount;
            resultcount = 1;
            //CR2018021601 Added By Ravi 16-02-2018
            string path = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
            string directoryPath = Path.Combine((path), p_str_cmp_id);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(Path.Combine(directoryPath));
            }
            directoryPath = Path.Combine(directoryPath, "INBOUND");
            DirectoryInfo dir = new DirectoryInfo(directoryPath);
            directoryPath = Path.Combine(directoryPath, p_str_ibdocid);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(Path.Combine(directoryPath));
            }
            var directoryCount = dir.GetDirectories();

            if (directoryCount.Length > 0)
            {

                DirectoryInfo dir1 = new DirectoryInfo(directoryPath);
                foreach (FileInfo flInfo in dir1.GetFiles())
                {
                    if (Directory.Exists(directoryPath))
                    {
                        Directory.Delete(directoryPath, true);
                    }
                }
            }




            //CR2018021601 End
            return Json(resultcount, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShowdtlReport(string p_str_cmpid, string p_str_status, string p_str_ib_doc_id)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string cmp_id = p_str_cmpid;
            string ib_doc_id = p_str_ib_doc_id;
            string l_str_status = p_str_status;
            string l_str_tmp_name = string.Empty;   //CR - 3PL_MVC_IB_2018_0219_008
            string l_str_inout_type = string.Empty;
            InboundInquiry objInbound = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            //CR - 3PL_MVC_IB_2018_0219_008
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cmp_id = p_str_cmpid;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            objInbound.LstCmpName = objCompany.LstCmpName;
            l_str_tmp_name = objInbound.LstCmpName[0].cmp_name.ToString().Trim();
            //CR - 3PL_MVC_IB_2018_0219_008
            try
            {
                if (isValid)
                {
                    if (p_str_status == "OPEN")
                    {
                        strReportName = "rpt_ib_doc_entry_ack.rpt";
                        InboundInquiry objInboundInquiry = new InboundInquiry();
                        InboundInquiryService ServiceObject = new InboundInquiryService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                        objInboundInquiry.cmp_id = cmp_id;
                        objInboundInquiry.ib_doc_id = ib_doc_id;

                        objInboundInquiry = ServiceObject.GetInboundAckRptDetails(objInboundInquiry);
                        var rptSource = objInboundInquiry.ListAckRptDetails.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objInboundInquiry.ListAckRptDetails.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                        rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);   //CR - 3PL_MVC_IB_2018_0219_008
                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                    }
                    if (l_str_status == "POST")
                    {
                        InboundInquiry objInboundInquiry = new InboundInquiry();
                        InboundInquiryService ServiceObject = new InboundInquiryService();
                        objInboundInquiry.cmp_id = p_str_cmpid;
                        objInboundInquiry.ib_doc_id = ib_doc_id;
                        objInboundInquiry = ServiceObject.GEtStrgBillTYpe(objInboundInquiry);
                        objInboundInquiry.bill_type = objInboundInquiry.ListStrgBillType[0].bill_type;
                        objInboundInquiry.bill_inout_type = objInboundInquiry.ListStrgBillType[0].bill_inout_type;
                        objInboundInquiry.CNTR_CHECK = "RATE_ID";
                        ServiceObject.GetContainerandRateID(objInboundInquiry);
                        l_str_inout_type = objInboundInquiry.check_inout_type;
                        if (l_str_inout_type != "")
                        {

                            if (objInboundInquiry.ListGETRateID[0].RATEID.Trim() == "CNTR")      //CR_3PL_MVC_BL_2018_0303_001 Added By MEERA 03-03-2018
                            {
                                strReportName = "rpt_ib_doc_recv_post_confrimation_by_container.rpt";
                                ReportDocument rd = new ReportDocument();
                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                objInboundInquiry.cmp_id = p_str_cmpid;
                                objInboundInquiry.ib_doc_id = ib_doc_id;
                                objInboundInquiry = ServiceObject.GetInboundConfirmationRptDetailsbyContainer(objInboundInquiry);
                                var rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objInboundInquiry.ListConfirmationRptDetails.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);    //CR - 3PL_MVC_IB_2018_0219_008
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");


                            }
                            else//CR-20180518-001 AddedBy Nithya
                            {
                                strReportName = "rpt_ib_doc_recv_post_confrimation.rpt";

                                ReportDocument rd = new ReportDocument();
                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                objInboundInquiry.cmp_id = cmp_id;
                                objInboundInquiry.ib_doc_id = ib_doc_id;

                                objInboundInquiry = ServiceObject.GetInboundConfirmationRptDetails(objInboundInquiry);
                                var rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objInboundInquiry.ListConfirmationRptDetails.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);    //CR - 3PL_MVC_IB_2018_0219_008
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            }
                        }


                    }
                    else if (l_str_status == "1-RCVD")
                    {
                        strReportName = "rpt_ib_doc_recv_tallysheet.rpt";
                        InboundInquiry objInboundInquiry = new InboundInquiry();
                        InboundInquiryService ServiceObject = new InboundInquiryService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                        objInboundInquiry.cmp_id = cmp_id;
                        objInboundInquiry.ib_doc_id = ib_doc_id;

                        objInboundInquiry = ServiceObject.GetInboundTallySheetRptDetails(objInboundInquiry);
                        var rptSource = objInboundInquiry.ListTallySheetRptDetails.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objInboundInquiry.ListTallySheetRptDetails.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                        rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);    //CR - 3PL_MVC_IB_2018_0219_008
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

        public ActionResult ShowSummaryEmailRpt(string p_str_ib_doc_id, string p_str_ib_doc_id_to, string p_str_radio, string p_str_cmp_id, string p_str_cntr_id, string p_str_status, string p_str_ref_no, string p_str_doc_dtFm, string p_str_doc_dtTo, string p_str_eta_dtFm, string p_str_eta_dtTo, string p_str_itm_num, string p_str_itm_color, string p_str_itm_size,
            string type)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            string strFileName = string.Empty;
            string strDateFormat = string.Empty;
            string reportFileName = string.Empty;
            bool isValid = true;
            //string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string l_str_tmp_name = string.Empty; //CR - 3PL_MVC_IB_2018_0219_008
            //string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            l_str_rpt_selection = p_str_radio;
            string l_str_status = string.Empty;
            string l_str_rptdtl = string.Empty;
            Folderpath = System.Configuration.ConfigurationManager.AppSettings["DefaultFolderPath"].ToString().Trim();
            InboundInquiry objInbound = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            objInbound.cmp_id = p_str_cmp_id;
            objInbound.ib_doc_id = p_str_ib_doc_id;
            //CR - 3PL_MVC_IB_2018_0219_008
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cmp_id = p_str_cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            objInbound.LstCmpName = objCompany.LstCmpName;
            l_str_tmp_name = objInbound.LstCmpName[0].cmp_name.ToString().Trim();
            //CR - 3PL_MVC_IB_2018_0219_008
            //objInbound = objService.GetInboundStatus(objInbound);
            //l_str_status = objInbound.ListInboundStatusRptDetails[0].STATUS;

            if (isValid)
            {


                if (l_str_rpt_selection == "GridSummary")
                {
                    strReportName = "rpt_inbound_grid_summary.rpt";
                    ReportDocument rd = new ReportDocument();
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                    objInboundInquiry.cmp_id = p_str_cmp_id;
                    objInboundInquiry.cntr_id = p_str_cntr_id;
                    objInboundInquiry.status = p_str_status;
                    objInboundInquiry.ib_doc_id = p_str_ib_doc_id;
                    objInboundInquiry.ib_doc_id_fm = p_str_ib_doc_id;
                    objInboundInquiry.ib_doc_id_to = p_str_ib_doc_id_to;
                    objInboundInquiry.req_num = p_str_ref_no;
                    objInboundInquiry.ib_doc_dt_fm = p_str_doc_dtFm;
                    objInboundInquiry.ib_doc_dt_to = p_str_doc_dtTo;
                    objInboundInquiry.eta_dt_fm = p_str_eta_dtFm;
                    objInboundInquiry.eta_dt_to = p_str_eta_dtTo;

                    objInboundInquiry.itm_num = p_str_itm_num;
                    objInboundInquiry.itm_color = p_str_itm_color;
                    objInboundInquiry.itm_size = p_str_itm_size;
                    objInboundInquiry = objService.GetInboundGridSummaryDetails(objInboundInquiry);
                    if (objInboundInquiry.ListGridSummaryRptDetails.Count > 0)
                    {
                        objInboundInquiry.cmp_id = (objInboundInquiry.ListGridSummaryRptDetails[0].cmp_id == null || objInboundInquiry.ListGridSummaryRptDetails[0].cmp_id.Trim() == "" ? string.Empty : objInboundInquiry.ListGridSummaryRptDetails[0].cmp_id.Trim());
                        objInboundInquiry.ibdocid = (objInboundInquiry.ListGridSummaryRptDetails[0].ib_doc_id == null || objInboundInquiry.ListGridSummaryRptDetails[0].ib_doc_id.Trim() == "" ? string.Empty : objInboundInquiry.ListGridSummaryRptDetails[0].ib_doc_id.Trim());
                        objInboundInquiry.ib_doc_dt = (objInboundInquiry.ListGridSummaryRptDetails[0].ib_doc_dt == null || objInboundInquiry.ListGridSummaryRptDetails[0].ib_doc_dt.Trim() == "" ? string.Empty : objInboundInquiry.ListGridSummaryRptDetails[0].ib_doc_dt.Trim());
                        objInboundInquiry.cntr_id = (objInboundInquiry.ListGridSummaryRptDetails[0].cntr_id == null || objInboundInquiry.ListGridSummaryRptDetails[0].cntr_id.Trim() == "" ? string.Empty : objInboundInquiry.ListGridSummaryRptDetails[0].cntr_id.Trim());
                        objInboundInquiry.eta_dt = (objInboundInquiry.ListGridSummaryRptDetails[0].eta_dt == null || objInboundInquiry.ListGridSummaryRptDetails[0].eta_dt.Trim() == "" ? string.Empty : objInboundInquiry.ListGridSummaryRptDetails[0].eta_dt.Trim());
                        if ((objInboundInquiry.cntr_id == "") && (objInboundInquiry.status == "") && (objInboundInquiry.ib_doc_id_fm == "") && (objInboundInquiry.ib_doc_id_to == "") && (objInboundInquiry.ib_doc_dt_fm == "") && (objInboundInquiry.ib_doc_dt_fm == "") && (objInboundInquiry.eta_dt_fm == "") && (objInboundInquiry.eta_dt_to == ""))
                        {
                            l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB GridSummary";
                            objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "IB GridSummary";
                            objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id;

                        }
                        else if ((objInboundInquiry.status == "") && (objInboundInquiry.ib_doc_id_fm == "") && (objInboundInquiry.ib_doc_id_to == "") && (objInboundInquiry.ib_doc_dt_fm == "") && (objInboundInquiry.ib_doc_dt_fm == "") && (objInboundInquiry.eta_dt_fm == "") && (objInboundInquiry.eta_dt_to == ""))
                        {
                            l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB GridSummary" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                            objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "IB GridSummary" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                            objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "Ref# -" + "\n" + "Received From:" + objInboundInquiry.recvd_fm + "\n" + "ETA Date:" + objInboundInquiry.eta_dt;

                        }
                        else if ((objInboundInquiry.ib_doc_id_fm == "") && (objInboundInquiry.ib_doc_id_to == "") && (objInboundInquiry.ib_doc_dt_fm == "") && (objInboundInquiry.ib_doc_dt_fm == "") && (objInboundInquiry.eta_dt_fm == "") && (objInboundInquiry.eta_dt_to == ""))
                        {
                            l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB GridSummary" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                            objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "IB GridSummary" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                            objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "Ref# -" + "\n" + "Received From:" + objInboundInquiry.recvd_fm + "\n" + "ETA Date:" + objInboundInquiry.eta_dt;

                        }
                        else if ((objInboundInquiry.ib_doc_id_to == "") && (objInboundInquiry.ib_doc_dt_fm == "") && (objInboundInquiry.ib_doc_dt_fm == "") && (objInboundInquiry.eta_dt_fm == "") && (objInboundInquiry.eta_dt_to == ""))
                        {
                            l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB GridSummary" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                            objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "IB GridSummary" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                            objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "Ref# -" + "\n" + "Received From:" + objInboundInquiry.recvd_fm + "\n" + "ETA Date:" + objInboundInquiry.eta_dt;

                        }
                        else if ((objInboundInquiry.ib_doc_dt_fm == "") && (objInboundInquiry.ib_doc_dt_fm == "") && (objInboundInquiry.eta_dt_fm == "") && (objInboundInquiry.eta_dt_to == ""))
                        {
                            l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB GridSummary" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                            objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "IB GridSummary" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                            objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "Ref# -" + "\n" + "Received From:" + objInboundInquiry.recvd_fm + "\n" + "ETA Date:" + objInboundInquiry.eta_dt;

                        }
                        else if ((objInboundInquiry.ib_doc_dt_fm == "") && (objInboundInquiry.eta_dt_fm == "") && (objInboundInquiry.eta_dt_to == ""))
                        {
                            l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB GridSummary" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                            objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "IB GridSummary" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                            objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "Ref# -" + "\n" + "Received From:" + objInboundInquiry.recvd_fm + "\n" + "ETA Date:" + objInboundInquiry.eta_dt;

                        }
                        else if ((objInboundInquiry.eta_dt_fm == "") && (objInboundInquiry.eta_dt_to == ""))
                        {
                            l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB GridSummary" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                            objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "IB GridSummary" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                            objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "Ref# -" + "\n" + "Received From:" + objInboundInquiry.recvd_fm + "\n" + "ETA Date:" + objInboundInquiry.eta_dt;

                        }
                        else if ((objInboundInquiry.eta_dt_to == ""))
                        {
                            l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB GridSummary" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                            objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "IB GridSummary" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                            objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "Ref# -" + "\n" + "Received From:" + objInboundInquiry.recvd_fm + "\n" + "ETA Date:" + objInboundInquiry.eta_dt;

                        }
                        else
                        {
                            l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB GridSummary" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                            objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "IB GridSummary" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                            objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "Ref# -" + "\n" + "Received From:" + objInboundInquiry.recvd_fm + "\n" + "ETA Date:" + objInboundInquiry.eta_dt;
                        }
                    }

                    var rptSource = objInboundInquiry.ListGridSummaryRptDetails.ToList();
                    rd.Load(strRptPath);
                    int AlocCount = 0;
                    AlocCount = objInboundInquiry.ListGridSummaryRptDetails.Count();
                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                        rd.SetDataSource(rptSource);
                    rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name); //CR - 3PL_MVC_IB_2018_0219_008
                                                                             // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                                                             // string strDateFormat = string.Empty;
                                                                             // string strFileName = string.Empty;
                    if (type == "PDF")
                    {
                        Random filerand = new Random();
                        int iyear = DateTime.Now.Year;
                        iyear = filerand.Next(1000000, 9999999);
                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                        objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                        // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                        reportFileName = l_str_rptdtl + DateTime.Now.ToFileTime() + ".pdf";
                        Session["RptFileName"] = strFileName;

                    }
                    else if (type == "Word")
                    {
                        rd.ExportToHttpResponse(ExportFormatType.WordForWindows, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                    }
                    else if (type == "Excel")
                    {
                        //rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);

                    }
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
        public ActionResult ShowEmailReport(string SelectdID, string p_str_radio, string p_str_cmpid, string type)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            l_str_rpt_selection = p_str_radio;
            string l_str_status = string.Empty;
            string l_str_tmp_name = string.Empty;//CR - 3PL_MVC_IB_2018_0219_008
            InboundInquiry objInbound = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            objInbound.cmp_id = p_str_cmpid;
            objInbound.ib_doc_id = SelectdID;
            objInbound = objService.GetInboundStatus(objInbound);
            l_str_status = objInbound.ListInboundStatusRptDetails[0].STATUS.Trim();
            string strDateFormat = string.Empty;
            string strFileName = string.Empty;
            string reportFileName = string.Empty;
            string l_str_inout_type = string.Empty;
            string l_str_rptdtl = string.Empty;

            Folderpath = System.Configuration.ConfigurationManager.AppSettings["DefaultFolderPath"].ToString().Trim();
            //CR - 3PL_MVC_IB_2018_0219_008
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cmp_id = p_str_cmpid;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            objInbound.LstCmpName = objCompany.LstCmpName;
            l_str_tmp_name = objInbound.LstCmpName[0].cmp_name.ToString().Trim();
            //CR - 3PL_MVC_IB_2018_0219_008
            try
            {
                if (isValid)
                {

                    if (l_str_rpt_selection == "Acknowledgement")
                    {
                        strReportName = "rpt_ib_doc_entry_ack.rpt";
                        InboundInquiry objInboundInquiry = new InboundInquiry();
                        InboundInquiryService ServiceObject = new InboundInquiryService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                        objInboundInquiry.cmp_id = p_str_cmpid;
                        objInboundInquiry.ib_doc_id = SelectdID;


                        objInboundInquiry = ServiceObject.GetInboundAckRptDetails(objInboundInquiry);

                        if (objInboundInquiry.ListAckRptDetails.Count > 0)
                        {
                            objInboundInquiry.ibdocid = (objInboundInquiry.ListAckRptDetails[0].ib_doc_id == null || objInboundInquiry.ListAckRptDetails[0].ib_doc_id.Trim() == "" ? string.Empty : objInboundInquiry.ListAckRptDetails[0].ib_doc_id.Trim());
                            objInboundInquiry.cntr_id = (objInboundInquiry.ListAckRptDetails[0].cntr_id == null || objInboundInquiry.ListAckRptDetails[0].cntr_id.Trim() == "" ? string.Empty : objInboundInquiry.ListAckRptDetails[0].cntr_id.Trim());
                            objInboundInquiry.ib_doc_dt = (objInboundInquiry.ListAckRptDetails[0].ib_doc_dt == null || objInboundInquiry.ListAckRptDetails[0].ib_doc_dt.Trim() == "" ? string.Empty : objInboundInquiry.ListAckRptDetails[0].ib_doc_dt.Trim());
                            objInboundInquiry.eta_dt = (objInboundInquiry.ListAckRptDetails[0].eta_dt == null || objInboundInquiry.ListAckRptDetails[0].eta_dt.Trim() == "" ? string.Empty : objInboundInquiry.ListAckRptDetails[0].eta_dt.Trim());
                            objInboundInquiry.recvd_fm = (objInboundInquiry.ListAckRptDetails[0].vend_name == null || objInboundInquiry.ListAckRptDetails[0].vend_name.Trim() == "" ? string.Empty : objInboundInquiry.ListAckRptDetails[0].vend_name.Trim());
                            objInboundInquiry.req_num = (objInboundInquiry.ListAckRptDetails[0].req_num == null || objInboundInquiry.ListAckRptDetails[0].req_num.Trim() == "" ? string.Empty : objInboundInquiry.ListAckRptDetails[0].req_num.Trim());
                            if ((objInboundInquiry.cntr_id == "" || objInboundInquiry.cntr_id == null || objInboundInquiry.cntr_id == "-") && (objInboundInquiry.eta_dt == "" || objInboundInquiry.eta_dt == null || objInboundInquiry.eta_dt == "-") && (objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                            {
                                l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound Ack" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + " " + " " + "Inbound Ack" + "|" + " " + " " + "IB#: " + " " + objInboundInquiry.ibdocid + "|" + " " + " " + "IB Date: " + objInboundInquiry.ib_doc_dt;
                                objEmail.EmailMessage = "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + " " + " " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + " " + objInboundInquiry.ib_doc_dt;

                            }
                            else if ((objInboundInquiry.eta_dt == "" || objInboundInquiry.eta_dt == null || objInboundInquiry.eta_dt == "-") && (objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                            {
                                l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound Ack" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + " " + " " + "Inbound Ack" + "|" + " " + " " + "IB#: " + objInboundInquiry.ibdocid + "|" + " " + " " + "IB_Dt: " + objInboundInquiry.ib_doc_dt + "|" + " " + " " + "CNTR#: " + objInboundInquiry.cntr_id;
                                objEmail.EmailMessage = "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + " " + " " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + " " + " " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#: " + " " + " " + objInboundInquiry.cntr_id;

                            }
                            else if ((objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                            {
                                l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound Ack" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + " " + " " + "Inbound Ack" + "|" + " " + " " + "IB#: " + objInboundInquiry.ibdocid + "|" + " " + " " + "IB_Dt: " + objInboundInquiry.ib_doc_dt + "|" + " " + " " + "CNTR#: " + objInboundInquiry.cntr_id;
                                objEmail.EmailMessage = "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "ETA Date:" + " " + " " + objInboundInquiry.eta_dt;

                            }
                            else if ((objInboundInquiry.req_num == "" || objInboundInquiry.req_num == null || objInboundInquiry.req_num == "-"))
                            {
                                l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound Ack" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + " " + " " + "Inbound Ack" + "|" + " " + " " + "IB#: " + objInboundInquiry.ibdocid + "|" + " " + " " + "IB_Dt: " + objInboundInquiry.ib_doc_dt + "|" + " " + " " + "CNTR#: " + objInboundInquiry.cntr_id;
                                objEmail.EmailMessage = "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + " " + " " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + " " + " " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#: " + " " + " " + objInboundInquiry.cntr_id + "\n" + "Received From: " + " " + " " + objInboundInquiry.recvd_fm + "\n" + "ETA Date: " + " " + " " + objInboundInquiry.eta_dt;


                            }
                            else
                            {
                                l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound Ack" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "Inbound Ack" + "|" + " " + "IB#: " + objInboundInquiry.ibdocid + "|" + " " + "IB_Dt: " + objInboundInquiry.ib_doc_dt + "|" + " " + "CNTR#: " + objInboundInquiry.cntr_id;
                                objEmail.EmailMessage = "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + " " + " " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + " " + " " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#: " + " " + " " + objInboundInquiry.cntr_id + "\n" + "Ref#: " + " " + " " + objInboundInquiry.req_num + "\n" + "Received From: " + " " + " " + objInboundInquiry.recvd_fm + "\n" + "ETA Date: " + " " + " " + objInboundInquiry.eta_dt;
                            }
                        }

                        var rptSource = objInboundInquiry.ListAckRptDetails.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objInboundInquiry.ListAckRptDetails.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);//CR - 3PL_MVC_IB_2018_0219_008
                                                                                //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                                                                // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");

                        if (type == "PDF")
                        {
                            objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                            // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            // rd.ExportToDisk(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                            reportFileName = l_str_rptdtl + strDateFormat + ".pdf";
                            Session["RptFileName"] = strFileName;


                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.WordForWindows, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                        }
                        else if (type == "Excel")
                        {
                            //rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);

                        }
                    }
                    if (l_str_rpt_selection == "TallySheet")
                    {

                        if (l_str_status == "POST")
                        {
                            strReportName = "rpt_ib_doc_recv_post_confrimation.rpt";
                            InboundInquiry objInboundInquiry = new InboundInquiry();
                            InboundInquiryService ServiceObject = new InboundInquiryService();
                            ReportDocument rd = new ReportDocument();
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                            objInboundInquiry.cmp_id = p_str_cmpid;
                            objInboundInquiry.ib_doc_id = SelectdID;
                            objInboundInquiry = ServiceObject.GetTotal(objInboundInquiry);
                            objInboundInquiry.tot_ctn = objInboundInquiry.ListTotalCount[0].tot_ctn;
                            objInboundInquiry.tot_cube = objInboundInquiry.ListTotalCount[0].tot_cube;
                            objInboundInquiry = ServiceObject.GetInboundConfirmationRptDetails(objInboundInquiry);
                            if (objInboundInquiry.ListConfirmationRptDetails.Count > 0)
                            {
                                objInboundInquiry.ibdocid = (objInboundInquiry.ListConfirmationRptDetails[0].ib_doc_id == null || objInboundInquiry.ListConfirmationRptDetails[0].ib_doc_id.Trim() == "" ? string.Empty : objInboundInquiry.ListConfirmationRptDetails[0].ib_doc_id.Trim());
                                objInboundInquiry.cntr_id = (objInboundInquiry.ListConfirmationRptDetails[0].cont_id == null || objInboundInquiry.ListConfirmationRptDetails[0].cont_id.Trim() == "" ? string.Empty : objInboundInquiry.ListConfirmationRptDetails[0].cont_id.Trim());
                                objInboundInquiry.ib_doc_dt = (objInboundInquiry.ListConfirmationRptDetails[0].ib_doc_dt == null || objInboundInquiry.ListConfirmationRptDetails[0].ib_doc_dt.Trim() == "" ? string.Empty : objInboundInquiry.ListConfirmationRptDetails[0].ib_doc_dt.Trim());
                                objInboundInquiry.eta_dt = (objInboundInquiry.ListConfirmationRptDetails[0].eta_dt == null || objInboundInquiry.ListConfirmationRptDetails[0].eta_dt.Trim() == "" ? string.Empty : objInboundInquiry.ListConfirmationRptDetails[0].eta_dt.Trim());

                                if ((objInboundInquiry.cntr_id == "" || objInboundInquiry.cntr_id == null || objInboundInquiry.cntr_id == "-") && (objInboundInquiry.eta_dt == "" || objInboundInquiry.eta_dt == null || objInboundInquiry.eta_dt == "-") && (objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                                {
                                    l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound TallySheet" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                    objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + " " + " " + "Inbound TallySheet" + "|" + " " + " " + "IB#: " + " " + objInboundInquiry.ibdocid + "|" + " " + " " + "IB Date: " + objInboundInquiry.ib_doc_dt;
                                    objEmail.EmailMessage = "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + " " + " " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + " " + objInboundInquiry.ib_doc_dt + "\n" + "Total Cartons Received: " + " " + " " + objInboundInquiry.tot_ctn + " " + "Ctns" + "\n" + "Total Cube: " + " " + " " + objInboundInquiry.tot_cube + " " + "Lbs";

                                }
                                else if ((objInboundInquiry.eta_dt == "" || objInboundInquiry.eta_dt == null || objInboundInquiry.eta_dt == "-") && (objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                                {
                                    l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound TallySheet" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                    objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + " " + " " + "Inbound TallySheet" + "|" + " " + " " + "IB#: " + objInboundInquiry.ibdocid + "|" + " " + " " + "IB_Dt: " + objInboundInquiry.ib_doc_dt + "|" + " " + " " + "CNTR#: " + objInboundInquiry.cntr_id;
                                    objEmail.EmailMessage = "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + " " + " " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + " " + " " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#: " + " " + " " + objInboundInquiry.cntr_id + "\n" + "Total Cartons Received: " + " " + " " + objInboundInquiry.tot_ctn + " " + "Ctns" + "\n" + "Total Cube: " + " " + " " + objInboundInquiry.tot_cube + " " + "Lbs";

                                }
                                else if ((objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                                {
                                    l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound TallySheet" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                    objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + " " + " " + "Inbound TallySheet" + "|" + " " + " " + "IB#: " + objInboundInquiry.ibdocid + "|" + " " + " " + "IB_Dt: " + objInboundInquiry.ib_doc_dt + "|" + " " + " " + "CNTR#: " + objInboundInquiry.cntr_id;
                                    objEmail.EmailMessage = "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "ETA Date:" + " " + " " + objInboundInquiry.eta_dt;

                                }
                                else if ((objInboundInquiry.req_num == "" || objInboundInquiry.req_num == null || objInboundInquiry.req_num == "-"))
                                {
                                    l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound TallySheet" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                    objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + " " + " " + "Inbound TallySheet" + "|" + " " + " " + "IB#: " + objInboundInquiry.ibdocid + "|" + " " + " " + "IB_Dt: " + objInboundInquiry.ib_doc_dt + "|" + " " + " " + "CNTR#: " + objInboundInquiry.cntr_id;
                                    objEmail.EmailMessage = "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + " " + " " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + " " + " " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#: " + " " + " " + objInboundInquiry.cntr_id + "\n" + "Received From: " + " " + " " + objInboundInquiry.recvd_fm + "\n" + "ETA Date: " + " " + " " + objInboundInquiry.eta_dt + "\n" + "Total Cartons Received: " + " " + " " + objInboundInquiry.tot_ctn + " " + "Ctns" + "\n" + "Total Cube: " + " " + " " + objInboundInquiry.tot_cube + " " + "Lbs";


                                }
                                else
                                {
                                    l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound TallySheet" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                    objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "Inbound TallySheet" + "|" + " " + "IB#: " + objInboundInquiry.ibdocid + "|" + " " + "IB_Dt: " + objInboundInquiry.ib_doc_dt + "|" + " " + "CNTR#: " + objInboundInquiry.cntr_id;
                                    objEmail.EmailMessage = "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + " " + " " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + " " + " " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#: " + " " + " " + objInboundInquiry.cntr_id + "\n" + "Ref#: " + " " + " " + objInboundInquiry.req_num + "\n" + "Received From: " + " " + " " + objInboundInquiry.recvd_fm + "\n" + "ETA Date: " + " " + " " + objInboundInquiry.eta_dt + "\n" + "Total Cartons Received: " + " " + " " + objInboundInquiry.tot_ctn + " " + "Ctns" + "\n" + "Total Cube: " + " " + " " + objInboundInquiry.tot_cube + " " + "Lbs";
                                }

                            }
                            var rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objInboundInquiry.ListConfirmationRptDetails.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);//CR - 3PL_MVC_IB_2018_0219_008
                                                                                    // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            if (type == "PDF")
                            {
                                objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//Inbound Doc.Receiving Post Confirmation(TallySheet)_" + strDateFormat + ".pdf";
                                // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                // rd.ExportToDisk(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                reportFileName = l_str_rptdtl + strDateFormat + ".pdf";
                                Session["RptFileName"] = strFileName;


                            }
                            else if (type == "Word")
                            {
                                rd.ExportToHttpResponse(ExportFormatType.WordForWindows, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                            }
                            else if (type == "Excel")
                            {
                                //rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);

                            }

                        }
                        else if (l_str_status == "1-RCVD")
                        {
                            strReportName = "rpt_ib_doc_recv_tallysheet.rpt";
                            InboundInquiry objInboundInquiry = new InboundInquiry();
                            InboundInquiryService ServiceObject = new InboundInquiryService();
                            ReportDocument rd = new ReportDocument();
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                            objInboundInquiry.cmp_id = p_str_cmpid;
                            objInboundInquiry.ib_doc_id = SelectdID;

                            objInboundInquiry = ServiceObject.GetInboundTallySheetRptDetails(objInboundInquiry);

                            if (objInboundInquiry.ListTallySheetRptDetails.Count > 0)
                            {
                                objInboundInquiry.ibdocid = (objInboundInquiry.ListTallySheetRptDetails[0].ib_doc_id == null || objInboundInquiry.ListTallySheetRptDetails[0].ib_doc_id.Trim() == "" ? string.Empty : objInboundInquiry.ListTallySheetRptDetails[0].ib_doc_id.Trim());
                                objInboundInquiry.cntr_id = (objInboundInquiry.ListTallySheetRptDetails[0].cntr_id == null || objInboundInquiry.ListTallySheetRptDetails[0].cntr_id.Trim() == "" ? string.Empty : objInboundInquiry.ListTallySheetRptDetails[0].cntr_id.Trim());
                                objInboundInquiry.ib_doc_dt = (objInboundInquiry.ListTallySheetRptDetails[0].ib_doc_dt == null || objInboundInquiry.ListTallySheetRptDetails[0].ib_doc_dt.Trim() == "" ? string.Empty : objInboundInquiry.ListTallySheetRptDetails[0].ib_doc_dt.Trim());
                                objInboundInquiry.eta_dt = (objInboundInquiry.ListTallySheetRptDetails[0].eta_dt == null || objInboundInquiry.ListTallySheetRptDetails[0].eta_dt.Trim() == "" ? string.Empty : objInboundInquiry.ListTallySheetRptDetails[0].eta_dt.Trim());
                                objInboundInquiry.tot_ctn = int.Parse(objInboundInquiry.ListTallySheetRptDetails[0].pkg_id);
                                objInboundInquiry.tot_cube = objInboundInquiry.ListTallySheetRptDetails[0].cube;
                                if ((objInboundInquiry.cntr_id == "" || objInboundInquiry.cntr_id == null || objInboundInquiry.cntr_id == "-") && (objInboundInquiry.eta_dt == "" || objInboundInquiry.eta_dt == null || objInboundInquiry.eta_dt == "-") && (objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                                {
                                    l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound Tally Sheet" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                    objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "Inbound  Tally Sheet" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt;
                                    objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "-" + l_str_tmp_name + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "Total Cube:" + objInboundInquiry.tot_cube + "Lbs";

                                }
                                else if ((objInboundInquiry.eta_dt == "" || objInboundInquiry.eta_dt == null || objInboundInquiry.eta_dt == "-") && (objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                                {
                                    l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound  Tally Sheet" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                    objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "Inbound  Tally Sheet" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                                    objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "-" + l_str_tmp_name + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "Total Cartons Received:" + "Ctns" + objInboundInquiry.tot_ctn + "\n" + "Total Cube:" + objInboundInquiry.tot_cube + "Lbs";

                                }
                                else if ((objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                                {
                                    l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound  Tally Sheet" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                    objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "Inbound  Tally Sheet" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                                    objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "-" + l_str_tmp_name + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "ETA Date:" + objInboundInquiry.eta_dt + "\n" + "Total Cartons Received:" + "Ctns" + objInboundInquiry.tot_ctn + "\n" + "Total Cube:" + objInboundInquiry.tot_cube + "Lbs";

                                }
                                else if ((objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                                {

                                    l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound  Tally Sheet" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                    objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "Inbound  Tally Sheet" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                                    objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "-" + l_str_tmp_name + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "Ref# -" + "\n" + "Received From:" + objInboundInquiry.recvd_fm + "\n" + "Total Cartons Received:" + "Ctns" + objInboundInquiry.tot_ctn + "\n" + "Total Cube:" + objInboundInquiry.tot_cube + "Lbs";

                                }
                                else
                                {
                                    l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound  Tally Sheet" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                    objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "Inbound  Tally Sheet" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                                    objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "-" + l_str_tmp_name + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "Ref# -" + objInboundInquiry.req_num + "\n" + "Received From:" + objInboundInquiry.recvd_fm + "\n" + "ETA Date:" + objInboundInquiry.eta_dt + "\n" + "Total Cartons Received:" + "Ctns" + objInboundInquiry.tot_ctn + "\n" + "Total Cube:" + objInboundInquiry.tot_cube + "Lbs";

                                }
                            }
                            var rptSource = objInboundInquiry.ListTallySheetRptDetails.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objInboundInquiry.ListTallySheetRptDetails.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);//CR - 3PL_MVC_IB_2018_0219_008
                                                                                    // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");

                            if (type == "PDF")
                            {
                                objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                // rd.ExportToDisk(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                reportFileName = l_str_rptdtl + strDateFormat + ".pdf";
                                Session["RptFileName"] = strFileName;


                            }
                            else if (type == "Word")
                            {
                                rd.ExportToHttpResponse(ExportFormatType.WordForWindows, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                            }
                            else if (type == "Excel")
                            {
                                //rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);

                            }
                        }

                    }

                    if (l_str_rpt_selection == "WorkSheet")
                    {
                        strReportName = "rpt_ib_doc_entry_recv_worksheet.rpt";
                        InboundInquiry objInboundInquiry = new InboundInquiry();
                        InboundInquiryService ServiceObject = new InboundInquiryService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                        objInboundInquiry.cmp_id = p_str_cmpid;
                        objInboundInquiry.ib_doc_id = SelectdID;

                        objInboundInquiry = ServiceObject.GetInboundWorkSheetRptDetails(objInboundInquiry);

                        if (objInboundInquiry.ListWorkSheetRptDetails.Count > 0)
                        {
                            objInboundInquiry.ibdocid = (objInboundInquiry.ListWorkSheetRptDetails[0].ib_doc_id == null || objInboundInquiry.ListWorkSheetRptDetails[0].ib_doc_id.Trim() == "" ? string.Empty : objInboundInquiry.ListWorkSheetRptDetails[0].ib_doc_id.Trim());
                            objInboundInquiry.cntr_id = (objInboundInquiry.ListWorkSheetRptDetails[0].cntr_id == null || objInboundInquiry.ListWorkSheetRptDetails[0].cntr_id.Trim() == "" ? string.Empty : objInboundInquiry.ListWorkSheetRptDetails[0].cntr_id.Trim());
                            objInboundInquiry.ib_doc_dt = (objInboundInquiry.ListWorkSheetRptDetails[0].ib_doc_dt == null || objInboundInquiry.ListWorkSheetRptDetails[0].ib_doc_dt.Trim() == "" ? string.Empty : objInboundInquiry.ListWorkSheetRptDetails[0].ib_doc_dt.Trim());
                            objInboundInquiry.eta_dt = (objInboundInquiry.ListWorkSheetRptDetails[0].eta_dt == null || objInboundInquiry.ListWorkSheetRptDetails[0].eta_dt.Trim() == "" ? string.Empty : objInboundInquiry.ListWorkSheetRptDetails[0].eta_dt.Trim());
                            objInboundInquiry.recvd_fm = (objInboundInquiry.ListWorkSheetRptDetails[0].vend_name == null || objInboundInquiry.ListWorkSheetRptDetails[0].vend_name.Trim() == "" ? string.Empty : objInboundInquiry.ListWorkSheetRptDetails[0].vend_name.Trim());
                            objInboundInquiry.req_num = (objInboundInquiry.ListWorkSheetRptDetails[0].req_num == null || objInboundInquiry.ListWorkSheetRptDetails[0].req_num.Trim() == "" ? string.Empty : objInboundInquiry.ListWorkSheetRptDetails[0].req_num.Trim());
                            objInboundInquiry.tot_ctn = objInboundInquiry.ListWorkSheetRptDetails[0].tot_ctn;
                            objInboundInquiry.tot_cube = objInboundInquiry.ListWorkSheetRptDetails[0].workCUBE;
                            if ((objInboundInquiry.cntr_id == "" || objInboundInquiry.cntr_id == null || objInboundInquiry.cntr_id == "-") && (objInboundInquiry.eta_dt == "" || objInboundInquiry.eta_dt == null || objInboundInquiry.eta_dt == "-") && (objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                            {
                                l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound Work Sheet" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "Inbound Work Sheet" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt;
                                objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "-" + l_str_tmp_name + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt;

                            }
                            else if ((objInboundInquiry.eta_dt == "" || objInboundInquiry.eta_dt == null || objInboundInquiry.eta_dt == "-") && (objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                            {
                                l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound Work Sheet" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "Inbound Work Sheet" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                                objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "-" + l_str_tmp_name + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id;

                            }
                            else if ((objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                            {
                                l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound Work Sheet" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "Inbound Work Sheet" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                                objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "-" + l_str_tmp_name + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "ETA Date:" + objInboundInquiry.eta_dt;

                            }
                            else if ((objInboundInquiry.req_num == "" || objInboundInquiry.req_num == null || objInboundInquiry.req_num == "-"))
                            {
                                l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound Work Sheet" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "Inbound Work Sheet" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                                objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "-" + l_str_tmp_name + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "Received From:" + objInboundInquiry.recvd_fm + "\n" + "ETA Date:" + objInboundInquiry.eta_dt;

                            }
                            else
                            {
                                l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound Work Sheet" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "Inbound Work Sheet" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                                objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "-" + l_str_tmp_name + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "Ref# -" + objInboundInquiry.req_num + "\n" + "Received From:" + objInboundInquiry.recvd_fm + "\n" + "ETA Date:" + objInboundInquiry.eta_dt;

                            }
                        }
                        var rptSource = objInboundInquiry.ListWorkSheetRptDetails.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objInboundInquiry.ListWorkSheetRptDetails.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name); //CR - 3PL_MVC_IB_2018_0219_008

                        if (type == "PDF")
                        {
                            objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                            // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            // rd.ExportToDisk(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                            reportFileName = l_str_rptdtl + strDateFormat + ".pdf";
                            Session["RptFileName"] = strFileName;

                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.WordForWindows, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                        }
                        else if (type == "Excel")
                        {
                            //rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);

                        }

                    }
                    if (l_str_rpt_selection == "Confirmation")
                    {
                        if (l_str_status == "POST")
                        {
                            InboundInquiry objInboundInquiry = new InboundInquiry();
                            InboundInquiryService ServiceObject = new InboundInquiryService();
                            objInboundInquiry.cmp_id = p_str_cmpid;
                            objInboundInquiry.ib_doc_id = SelectdID;
                            objInboundInquiry = ServiceObject.GEtStrgBillTYpe(objInboundInquiry);
                            objInboundInquiry.bill_type = objInboundInquiry.ListStrgBillType[0].bill_type;
                            objInboundInquiry.bill_inout_type = objInboundInquiry.ListStrgBillType[0].bill_inout_type;
                            objInboundInquiry.CNTR_CHECK = "RATE_ID";
                            ServiceObject.GetContainerandRateID(objInboundInquiry);
                            l_str_inout_type = objInboundInquiry.check_inout_type;
                            if (l_str_inout_type != "")
                            {

                                if (objInboundInquiry.ListGETRateID[0].RATEID.Trim() == "CNTR")      //CR_3PL_MVC_BL_2018_0303_001 Added By MEERA 03-03-2018
                                {
                                    strReportName = "rpt_ib_doc_recv_post_confrimation_by_container.rpt";
                                    ReportDocument rd = new ReportDocument();
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                    objInboundInquiry.cmp_id = p_str_cmpid;
                                    objInboundInquiry.ib_doc_id = SelectdID;
                                    objInboundInquiry = ServiceObject.GetInboundConfirmationRptDetailsbyContainer(objInboundInquiry);
                                    if (objInboundInquiry.ListConfirmationRptDetails.Count > 0)
                                    {
                                        objInboundInquiry.ibdocid = (objInboundInquiry.ListConfirmationRptDetails[0].ib_doc_id == null || objInboundInquiry.ListConfirmationRptDetails[0].ib_doc_id.Trim() == "" ? string.Empty : objInboundInquiry.ListConfirmationRptDetails[0].ib_doc_id.Trim());
                                        objInboundInquiry.cntr_id = (objInboundInquiry.ListConfirmationRptDetails[0].cntr_id == null || objInboundInquiry.ListConfirmationRptDetails[0].cntr_id.Trim() == "" ? string.Empty : objInboundInquiry.ListConfirmationRptDetails[0].cntr_id.Trim());
                                        objInboundInquiry.eta_dt = (objInboundInquiry.ListConfirmationRptDetails[0].eta_dt == null || objInboundInquiry.ListConfirmationRptDetails[0].eta_dt.Trim() == "" ? string.Empty : objInboundInquiry.ListConfirmationRptDetails[0].eta_dt.Trim());
                                        objInboundInquiry.ib_doc_dt = (objInboundInquiry.ListConfirmationRptDetails[0].ib_doc_dt == null || objInboundInquiry.ListConfirmationRptDetails[0].ib_doc_dt.Trim() == "" ? string.Empty : objInboundInquiry.ListConfirmationRptDetails[0].ib_doc_dt.Trim());
                                        objInboundInquiry.cntr_id = (objInboundInquiry.ListConfirmationRptDetails[0].cntr_id == null || objInboundInquiry.ListConfirmationRptDetails[0].cntr_id.Trim() == "" ? string.Empty : objInboundInquiry.ListConfirmationRptDetails[0].cntr_id.Trim());
                                        objInboundInquiry.eta_dt = (objInboundInquiry.ListConfirmationRptDetails[0].eta_dt == null || objInboundInquiry.ListConfirmationRptDetails[0].eta_dt.Trim() == "" ? string.Empty : objInboundInquiry.ListConfirmationRptDetails[0].eta_dt.Trim());
                                        if ((objInboundInquiry.cntr_id == "" || objInboundInquiry.cntr_id == null || objInboundInquiry.cntr_id == "-") && (objInboundInquiry.eta_dt == "" || objInboundInquiry.eta_dt == null || objInboundInquiry.eta_dt == "-") && (objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                                        {
                                            l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound Confirmation" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                            objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "Inbound Confirmation" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt;
                                            objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "-" + l_str_tmp_name + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt;

                                        }
                                        else if ((objInboundInquiry.eta_dt == "" || objInboundInquiry.eta_dt == null || objInboundInquiry.eta_dt == "-") && (objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                                        {
                                            l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound Confirmation" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                            objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "Inbound Confirmation" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                                            objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "-" + l_str_tmp_name + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id;

                                        }
                                        else if ((objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                                        {
                                            l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound Confirmation" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                            objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "Inbound Confirmation" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                                            objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "-" + l_str_tmp_name + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "ETA Date:" + objInboundInquiry.eta_dt;

                                        }
                                        else if ((objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                                        {

                                            l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound Confirmation" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                            objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "Inbound Confirmation" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                                            objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "-" + l_str_tmp_name + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "Ref# -" + "\n" + "Received From:" + objInboundInquiry.recvd_fm;

                                        }
                                        else
                                        {
                                            l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound Confirmation" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                            objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "Inbound Confirmation" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                                            objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "-" + l_str_tmp_name + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "Ref# -" + objInboundInquiry.req_num + "\n" + "Received From:" + objInboundInquiry.recvd_fm + "\n" + "ETA Date:" + objInboundInquiry.eta_dt;

                                        }
                                    }

                                    var rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objInboundInquiry.ListConfirmationRptDetails.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name); //CR - 3PL_MVC_IB_2018_0219_008
                                                                                             // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                    if (type == "PDF")
                                    {
                                        objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                        // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                        // rd.ExportToDisk(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        reportFileName = l_str_rptdtl + strDateFormat + ".pdf";
                                        Session["RptFileName"] = strFileName;

                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.WordForWindows, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                    }
                                    else if (type == "Excel")
                                    {
                                        //rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        InboundConfirmationExcel objInboundInquiryCnfrmExcel = new InboundConfirmationExcel();
                                        objInboundInquiryCnfrmExcel.PoNum = p_str_cmpid;
                                        objInboundInquiryCnfrmExcel.Style = SelectdID;
                                        objInboundInquiryCnfrmExcel = ServiceObject.GetInboundConfimExcel(objInboundInquiryCnfrmExcel);
                                        var model = objInboundInquiryCnfrmExcel.ListInboundConfrmExcelDetails.ToList();
                                        GridView gv = new GridView();
                                        gv.DataSource = model;
                                        gv.DataBind();
                                        Session["IB_CNFIRM"] = gv;
                                        if (Session["IB_CNFIRM"] != null)
                                        {
                                            return new DownloadFileActionResult((GridView)Session["IB_CNFIRM"], "IB_CNFIRM-" + SelectdID.Trim() + DateTime.Now.ToString() + ".xls");
                                        }
                                    }

                                }

                                else
                                {
                                    objInboundInquiry.cmp_id = p_str_cmpid;
                                    objInboundInquiry.ib_doc_id = SelectdID;
                                    strReportName = "rpt_ib_doc_recv_post_confrimation.rpt";

                                    ReportDocument rd = new ReportDocument();
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                    objInboundInquiry.cmp_id = p_str_cmpid;
                                    objInboundInquiry.ib_doc_id = SelectdID;

                                    objInboundInquiry = ServiceObject.GetInboundConfirmationRptDetails(objInboundInquiry);

                                    if (objInboundInquiry.ListConfirmationRptDetails.Count > 0)
                                    {
                                        objInboundInquiry.ibdocid = (objInboundInquiry.ListConfirmationRptDetails[0].ib_doc_id == null || objInboundInquiry.ListConfirmationRptDetails[0].ib_doc_id.Trim() == "" ? string.Empty : objInboundInquiry.ListConfirmationRptDetails[0].ib_doc_id.Trim());
                                        objInboundInquiry.cntr_id = (objInboundInquiry.ListConfirmationRptDetails[0].cntr_id == null || objInboundInquiry.ListConfirmationRptDetails[0].cntr_id.Trim() == "" ? string.Empty : objInboundInquiry.ListConfirmationRptDetails[0].cntr_id.Trim());
                                        objInboundInquiry.eta_dt = (objInboundInquiry.ListConfirmationRptDetails[0].eta_dt == null || objInboundInquiry.ListConfirmationRptDetails[0].eta_dt.Trim() == "" ? string.Empty : objInboundInquiry.ListConfirmationRptDetails[0].eta_dt.Trim());
                                        objInboundInquiry.ib_doc_dt = (objInboundInquiry.ListConfirmationRptDetails[0].ib_doc_dt == null || objInboundInquiry.ListConfirmationRptDetails[0].ib_doc_dt.Trim() == "" ? string.Empty : objInboundInquiry.ListConfirmationRptDetails[0].ib_doc_dt.Trim());
                                        objInboundInquiry.cntr_id = (objInboundInquiry.ListConfirmationRptDetails[0].cntr_id == null || objInboundInquiry.ListConfirmationRptDetails[0].cntr_id.Trim() == "" ? string.Empty : objInboundInquiry.ListConfirmationRptDetails[0].cntr_id.Trim());
                                        objInboundInquiry.eta_dt = (objInboundInquiry.ListConfirmationRptDetails[0].eta_dt == null || objInboundInquiry.ListConfirmationRptDetails[0].eta_dt.Trim() == "" ? string.Empty : objInboundInquiry.ListConfirmationRptDetails[0].eta_dt.Trim());
                                        if ((objInboundInquiry.cntr_id == "" || objInboundInquiry.cntr_id == null || objInboundInquiry.cntr_id == "-") && (objInboundInquiry.eta_dt == "" || objInboundInquiry.eta_dt == null || objInboundInquiry.eta_dt == "-") && (objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                                        {
                                            l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound Confirmation" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                            objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "Inbound Confirmation" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt;
                                            objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "-" + l_str_tmp_name + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt;

                                        }
                                        else if ((objInboundInquiry.eta_dt == "" || objInboundInquiry.eta_dt == null || objInboundInquiry.eta_dt == "-") && (objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                                        {
                                            l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound Confirmation" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                            objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "Inbound Confirmation" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                                            objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "-" + l_str_tmp_name + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id;

                                        }
                                        else if ((objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                                        {
                                            l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound Confirmation" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                            objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "Inbound Work Sheet" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                                            objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "-" + l_str_tmp_name + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "ETA Date:" + objInboundInquiry.eta_dt;

                                        }
                                        else if ((objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                                        {

                                            l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound Confirmation" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                            objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "Inbound Confirmation" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                                            objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "-" + l_str_tmp_name + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "Received From:" + objInboundInquiry.recvd_fm;

                                        }
                                        else
                                        {
                                            l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "Inbound Confirmation" + "_" + objInboundInquiry.ibdocid + "_" + objInboundInquiry.ib_doc_dt;
                                            objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "Inbound Confirmation" + "|" + "IB#:" + objInboundInquiry.ibdocid + "|" + "IB Date: " + objInboundInquiry.ib_doc_dt + "|" + "CNTR#:" + objInboundInquiry.cntr_id;
                                            objEmail.EmailMessage = "CmpId :" + objInboundInquiry.cmp_id + "-" + l_str_tmp_name + "\n" + "IB Doc Id# " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "Ref# -" + objInboundInquiry.req_num + "\n" + "Received From:" + objInboundInquiry.recvd_fm + "\n" + "ETA Date:" + objInboundInquiry.eta_dt;

                                        }
                                    }
                                    var rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objInboundInquiry.ListConfirmationRptDetails.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);//CR - 3PL_MVC_IB_2018_0219_008
                                                                                            // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                    if (type == "PDF")
                                    {
                                        objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                        // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                        // rd.ExportToDisk(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        reportFileName = l_str_rptdtl + strDateFormat + ".pdf";
                                        Session["RptFileName"] = strFileName;


                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.WordForWindows, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                    }
                                    else if (type == "Excel")
                                    {
                                        //rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);

                                    }
                                }
                            }
                        }
                    }


                    if (l_str_rpt_selection == "GridSummary")
                    {
                        strReportName = "rpt_inbound_grid_summary.rpt";
                        InboundInquiry objInboundInquiry = new InboundInquiry();
                        InboundInquiryService ServiceObject = new InboundInquiryService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;

                        objInboundInquiry.cmp_id = p_str_cmpid;
                        objInboundInquiry.cntr_id = TempData["p_str_cntr_id"].ToString().Trim();
                        objInboundInquiry.status = TempData["p_str_status"].ToString().Trim();
                        objInboundInquiry.ib_doc_id = SelectdID;
                        objInboundInquiry.req_num = TempData["p_str_ref_no"].ToString().Trim();
                        objInboundInquiry.ib_doc_dt_fm = TempData["p_str_doc_dtFm"].ToString().Trim();
                        objInboundInquiry.ib_doc_dt_to = TempData["p_str_doc_dtTo"].ToString().Trim();
                        objInboundInquiry.eta_dt_fm = TempData["p_str_eta_dtFm"].ToString().Trim();
                        objInboundInquiry.eta_dt_to = TempData["p_str_eta_dtTo"].ToString().Trim();
                        objInboundInquiry = ServiceObject.GetInboundGridSummaryDetails(objInboundInquiry);

                        if (objInboundInquiry.ListGridSummaryRptDetails.Count > 0)
                        {
                            objInboundInquiry.ibdocid = (objInboundInquiry.ListGridSummaryRptDetails[0].ib_doc_id == null || objInboundInquiry.ListGridSummaryRptDetails[0].ib_doc_id.Trim() == "" ? string.Empty : objInboundInquiry.ListGridSummaryRptDetails[0].ib_doc_id.Trim());
                            objInboundInquiry.cntr_id = (objInboundInquiry.ListGridSummaryRptDetails[0].cntr_id == null || objInboundInquiry.ListGridSummaryRptDetails[0].cntr_id.Trim() == "" ? string.Empty : objInboundInquiry.ListGridSummaryRptDetails[0].cntr_id.Trim());
                            objInboundInquiry.eta_dt = (objInboundInquiry.ListGridSummaryRptDetails[0].eta_dt == null || objInboundInquiry.ListGridSummaryRptDetails[0].eta_dt.Trim() == "" ? string.Empty : objInboundInquiry.ListGridSummaryRptDetails[0].eta_dt.Trim());
                            if ((objInboundInquiry.cntr_id == "" || objInboundInquiry.cntr_id == null || objInboundInquiry.cntr_id == "-") && (objInboundInquiry.eta_dt == "" || objInboundInquiry.eta_dt == null))
                            {
                                EmailSub = p_str_cmpid + "/" + "GRID_SUMMARY" + "_" + "IB#:" + objInboundInquiry.ibdocid;
                                l_str_rptdtl = p_str_cmpid + "_" + "GRID_SUMMARY" + "_" + "IB_" + objInboundInquiry.ibdocid;

                            }
                            else
                            {
                                l_str_rptdtl = p_str_cmpid + "_" + "GRID_SUMMARY" + "_" + "IB_" + objInboundInquiry.ibdocid + "_" + "CNTR_" + objInboundInquiry.cntr_id + "_" + "ETA_DT_" + objInboundInquiry.eta_dt;
                                EmailSub = p_str_cmpid + "/" + "GRID_SUMMARY" + "_" + "IB#:" + objInboundInquiry.ibdocid + "_" + "CNTR#:" + objInboundInquiry.cntr_id + "_" + "ETA_DT:" + objInboundInquiry.eta_dt;
                            }
                            if ((objInboundInquiry.cntr_id == "" || objInboundInquiry.cntr_id == null || objInboundInquiry.cntr_id == "-") && (objInboundInquiry.eta_dt == "" || objInboundInquiry.eta_dt == null))
                            {
                                EmailMsg = p_str_cmpid + "/" + "GRID_SUMMARY" + ";" + "IB#:" + objInboundInquiry.ibdocid;
                            }
                            else
                            {
                                EmailMsg = p_str_cmpid + "/" + "GRID_SUMMARY" + ";" + "IB#:" + objInboundInquiry.ibdocid + ";" + "CNTR#:" + objInboundInquiry.cntr_id + ";" + "ETA_DT:" + objInboundInquiry.eta_dt;
                            }
                        }
                        var rptSource = objInboundInquiry.ListGridSummaryRptDetails.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objInboundInquiry.ListGridSummaryRptDetails.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);//CR - 3PL_MVC_IB_2018_0219_008
                        //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        if (type == "PDF")
                        {
                            objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);

                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.WordForWindows, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                        }
                        else if (type == "Excel")
                        {
                            //rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                        }
                    }


                }
                else
                {
                    Response.Write("<H2>Report not found</H2>");
                }
                //CR_3PL_MVC_BL_2018_0210_002 - Commented and modified by Ravikumar
                //Email objEmail = new Email();
                objEmail.CmpId = p_str_cmpid;
                objEmail.screenId = ScreenID;
                objEmail.username = objCompany.user_id;
                objEmail.Reportselection = l_str_rpt_selection;
                //EmailService objEmailService = new EmailService();
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
                //CR_3PL_MVC_BL_2018_0210_002 - Above
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
        public ActionResult Getcheckedvalue(string p_str_cmp_id)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry = ServiceObject.IsRMAChecked(objInboundInquiry);
            return Json(objInboundInquiry.ListRMADocId, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CmpIdOnChange(string p_str_cmp_id)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            string l_str_tmp_cmp_id = string.Empty;
            Session["g_str_cmp_id"] = p_str_cmp_id;// CR_3PL_MVC_COMMON_2018_0326_001
            l_str_tmp_cmp_id = Session["g_str_cmp_id"].ToString().Trim();
            return Json(l_str_tmp_cmp_id, JsonRequestBehavior.AllowGet);
        }
        //CR-3PL_MVC_IB_2018_0312_001 
        public ActionResult GetDocStatus(string p_str_cmp_id, string p_str_ib_doc_id)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.ib_doc_id = p_str_ib_doc_id;
            objInboundInquiry = ServiceObject.GetInboundStatus(objInboundInquiry);
            objInboundInquiry.STATUS = objInboundInquiry.ListInboundStatusRptDetails[0].STATUS;
            return Json(objInboundInquiry.STATUS.Trim(), JsonRequestBehavior.AllowGet);
        }
        //End CR-3PL_MVC_IB_2018_0312_001 

        //public ActionResult DownloadExcelForProductForSummary(string p_str_ib_doc_id, string p_str_radio, string p_str_cmp_id, string p_str_cntr_id, string p_str_status, string p_str_ref_no, string p_str_doc_dtFm, string p_str_doc_dtTo, string p_str_eta_dtFm, string p_str_eta_dtTo)
        //{
        //    //Data can be passed with hidden input elements.
        //    string contextIdStr = Request.Form["hdnContextId"];
        //    int contextId = 0;
        //    Int32.TryParse(contextIdStr, out contextId);

        //    //Call to get Excel byte array.
        //    var excelBytes = GetExcelBytesForProduct(contextId);

        //    //Set file name.
        //    var fileName = string.Format("Products-{0}-{1}.xlsx", contextId, DateTime.Now.ToString("MMddyyyyHHmmssfff"));

        //    //Return file with the type and name. 
        //    //ContentType "application/vnd.ms-excel" does not work well for browsers other than IE.
        //    return excelBytes != null ? File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName) : null;
        //}
        //public ActionResult DownloadExcelForProduct(string p_str_cmpid, string p_str_status, string p_str_ib_doc_id)
        //{
        //    //Data can be passed with hidden input elements.
        //    string contextIdStr = Request.Form["hdnContextId"];
        //    int contextId = 0;
        //    Int32.TryParse(contextIdStr, out contextId);

        //    //Call to get Excel byte array.
        //    var excelBytes = GetExcelBytesForProduct(contextId);

        //    //Set file name.
        //    var fileName = string.Format("Products-{0}-{1}.xlsx", contextId, DateTime.Now.ToString("MMddyyyyHHmmssfff"));

        //    //Return file with the type and name. 
        //    //ContentType "application/vnd.ms-excel" does not work well for browsers other than IE.
        //    return excelBytes != null ? File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName) : null;
        //}
        //private byte[] GetExcelBytesForProduct(int contextId)
        //{


        //    ExcelInboundAck objInboundInquiry1 = new ExcelInboundAck();
        //    InboundInquiryService ServiceObject = new InboundInquiryService();
        //    //contextId can be used for getting particular parts or user-specific data.
        //    //It's not used for demo here.  
        //    ServiceObject.GetExcelInboundAck(objInboundInquiry1);
        //    var dataList = objInboundInquiry1.ListAckRptDetails;

        //    //Comma delimited string for outputting final column names with added suffixes if needed.
        //    //ToExcelBytes() will search and replace final column names.
        //    var colFixes = "Unit Price ($)";

        //    //Using custom sheet name, not default model object name.
        //    var sheetName = "Product List";

        //    return dataList.ToExcelBytes<ExcelInboundAck>(columnFixes: colFixes, sheetName: sheetName);
        //}

        //protected void ExportExcel(object sender, EventArgs e)
        //{
        //    //Get the GridView Data from database.
        //    DataTable dt = GetData();

        //    //Set DataTable Name which will be the name of Excel Sheet.
        //    dt.TableName = "GridView_Data";

        //    //Create a New Workbook.
        //    using (XLWorkbook wb = new XLWorkbook())
        //    {
        //        //Add the DataTable as Excel Worksheet.
        //        wb.Worksheets.Add(dt);

        //        using (MemoryStream memoryStream = new MemoryStream())
        //        {
        //            //Save the Excel Workbook to MemoryStream.
        //            wb.SaveAs(memoryStream);

        //            //Convert MemoryStream to Byte array.
        //            byte[] bytes = memoryStream.ToArray();
        //            memoryStream.Close();

        //            //Send Email with Excel attachment.
        //            using (MailMessage mm = new MailMessage("sender@gmail.com", "recipient@gmail.com"))
        //            {
        //                mm.Subject = "GridView Exported Excel";
        //                mm.Body = "GridView Exported Excel Attachment";

        //                //Add Byte array as Attachment.
        //                mm.Attachments.Add(new Attachment(new MemoryStream(bytes), "GridView.xlsx"));
        //                mm.IsBodyHtml = true;
        //                SmtpClient smtp = new SmtpClient();
        //                smtp.Host = "smtp.gmail.com";
        //                smtp.EnableSsl = true;
        //                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential();
        //                credentials.UserName = "sender@gmail.com";
        //                credentials.Password = "<password>";
        //                smtp.UseDefaultCredentials = true;
        //                smtp.Credentials = credentials;
        //                smtp.Port = 587;
        //                smtp.Send(mm);
        //            }
        //        }
        //    }
        //}

        public ActionResult InboundInquiryFileDocument(string cmp_id, string ibdocid, string cntrid, string datefrom, string dateto)
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
            objInboundInquiry.doctype = "INBOUND";
            string path = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
            string directoryPath = Path.Combine((path), cmp_id);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(Path.Combine(directoryPath));
            }
            directoryPath = Path.Combine(directoryPath, "INBOUND");
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
        public ActionResult Inbounddocupld(string cmp_id, string ibdocid, string cntrid, string datefrom, string dateto, string p_str_screentitle)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            objInboundInquiry.CompID = cmp_id;
            objInboundInquiry.cmp_id = cmp_id;
            objInboundInquiry.ibdocid = ibdocid;
            objInboundInquiry.Container = cntrid;
            objInboundInquiry.DocumentdateFrom = datefrom;
            objInboundInquiry.DocumentdateTo = dateto;
            objInboundInquiry.screentitle = p_str_screentitle;
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryDocModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_InboundInquiryImportFile", InboundInquiryDocModel);
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
                                directoryPath = Path.Combine(directoryPath, "INBOUND");
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
                    objInboundInquiry.doctype = "INBOUND";
                    objInboundInquiry.Uploadby = Session["UserID"].ToString().Trim();
                    objInboundInquiry.Comments = comment;
                    objInboundInquiry.UPLOAD_FILE = tempfilename;
                    string path1 = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
                    string directoryPath1 = Path.Combine((path1), CompID);
                    if (!Directory.Exists(directoryPath1))
                    {
                        Directory.CreateDirectory(Path.Combine(directoryPath1));
                    }
                    directoryPath1 = Path.Combine(directoryPath1, "INBOUND");
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
                    objInboundInquiry.doctype = "INBOUND";
                    string path2 = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
                    string directoryPath2 = Path.Combine((path2), CompID);
                    if (!Directory.Exists(directoryPath1))
                    {
                        Directory.CreateDirectory(Path.Combine(directoryPath2));
                    }
                    directoryPath2 = Path.Combine(directoryPath2, "INBOUND");
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
        public ActionResult GetDocumentUpload(string CompID, string ibdocid, string comment)
        {
            string name = string.Empty;
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            objInboundInquiry.cmp_id = CompID;
            objInboundInquiry.doctype = "INBOUND";
            objInboundInquiry.Uploadby = Session["UserID"].ToString().Trim();
            objInboundInquiry.Comments = comment;
            string path = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
            string directoryPath = Path.Combine((path), CompID);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(Path.Combine(directoryPath));
            }
            directoryPath = Path.Combine(directoryPath, "INBOUND");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(Path.Combine(directoryPath));
            }
            directoryPath = Path.Combine(directoryPath, ibdocid);

            DirectoryInfo dir = new DirectoryInfo(directoryPath);
            foreach (FileInfo flInfo in dir.GetFiles())
            {
                name = flInfo.Name;
                objInboundInquiry.Filename = name;
                long size = flInfo.Length;
                DateTime creationTime = flInfo.CreationTime;
                objInboundInquiry.Uploaddt = creationTime;
                objInboundInquiry.filepath = directoryPath;
                path = Path.Combine(directoryPath, name);
                if (!Directory.Exists(path))
                {
                    objInboundInquiry.Filename = name;
                    ServiceObject.InsertTempFileDocument(objInboundInquiry);
                }
            }
            objInboundInquiry.cmp_id = CompID;
            objInboundInquiry.doctype = "INBOUND";
            path = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
            directoryPath = Path.Combine((path), CompID);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(Path.Combine(directoryPath));
            }
            directoryPath = Path.Combine(directoryPath, "INBOUND");
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
            return PartialView("_DocumentUploadGrid", InboundInquiryDocModel);
        }
        public ActionResult uploaddoc(string cmpid, string path)

        {
            //string paths = path;                     
            //string FILENAME = filepath + '\\' + filename;
            string docPath = path;
            string paths = path;
            DirectoryInfo dir = new System.IO.DirectoryInfo(docPath);
            //int count = 0;
            //if(Directory.Exists(docPath))
            //                    {
            //    count = dir.GetFiles().Length;
            //}
            //if(count>0)
            //{
            return Json(paths, JsonRequestBehavior.AllowGet);
            // }
            //else
            // {
            //     return Json("File Not Found!");
            // }           
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
        public ActionResult GetDocumentUploadCancel(string CompID, string ibdocid, string comment)
        {
            string name = string.Empty;
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            objInboundInquiry.cmp_id = CompID;
            objInboundInquiry.doctype = "INBOUND";
            objInboundInquiry.Uploadby = Session["UserID"].ToString().Trim();
            objInboundInquiry.Comments = comment;
            string path = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
            string directoryPath = Path.Combine((path), CompID);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(Path.Combine(directoryPath));
            }
            directoryPath = Path.Combine(directoryPath, "INBOUND");
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
            directoryPath = Path.Combine(directoryPath, "INBOUND");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(Path.Combine(directoryPath));
            }
            //CR2018021602 Added By Ravi 16-02-2018
            directoryPath = Path.Combine(directoryPath, ibdocid);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(Path.Combine(directoryPath));
            }
            directoryPath = Path.Combine(directoryPath, Filename);
            //CR2018021602 End
            System.IO.File.Delete(directoryPath);
            DirectoryInfo dir = new DirectoryInfo(directoryPath);
            //int count = 0;
            //count = dir.GetFiles().Length;
            //foreach (FileInfo flInfo in dir.GetFiles())
            //{
            //    if (Directory.Exists(directoryPath))
            //    {
            //        Directory.Delete(directoryPath, true);
            //    }
            //}


            objInboundInquiry = objService.GetTempFiledtl(objInboundInquiry);
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_DocumentUploadGrid", InboundInquiryModel);
        }
        //public ActionResult DocReceivingEditCancel(string p_str_cmp_id, string p_str_ib_doc_id, int p_str_dtl_line, int p_str_ctn_line, string p_str_itm_code)
        //{
        public ActionResult DocReceivingEditCancel(string p_str_cmp_id, string p_str_ib_doc_id, int p_str_dtl_line, int p_str_ctn_line, string p_str_itm_code)
        {
            string name = string.Empty;
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.ib_doc_id = p_str_ib_doc_id;
            objInboundInquiry.line_num = p_str_dtl_line;
            objInboundInquiry.ctn_line = p_str_ctn_line;
            objInboundInquiry.itm_code = p_str_itm_code;

            objInboundInquiry = ServiceObject.GetRecvdtlGrid(objInboundInquiry);

            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_ReceivingEntryGrid", InboundInquiryModel);
        }
        public ActionResult ReceivingEntryEdit(string CmpId, string id, string Cont_id, string LotId, string datefrom, string dateto)
        {
            string l_str_lot_id = string.Empty;
            string l_str_rcvd_itm_mode = string.Empty;
            string l_str_transtatus = string.Empty;
            string l_str_locId = string.Empty;
            DateTime l_str_rcd_dt;
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objInboundInquiry.cmp_id = CmpId;
            objInboundInquiry.ibdocid = id;
            objInboundInquiry.ib_doc_id = id;
            objInboundInquiry.cntr_id = Cont_id;
            objInboundInquiry.DocumentdateFrom = datefrom;
            objInboundInquiry.DocumentdateTo = dateto;
            ServiceObject.GetIBRecvDeleteTempData(objInboundInquiry);    //CR_3PL_MVC_BL_2018_0313_001 
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objInboundInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objCompany.cust_cmp_id = CmpId;
            objInboundInquiry.lot_id = LotId;
            objInboundInquiry.cmp_id = CmpId;
            objInboundInquiry.ib_doc_id = id;
            objInboundInquiry.loc_id = "FLOOR";
            objInboundInquiry = ServiceObject.GetDftWhs(objInboundInquiry);
            string l_str_DftWhs = objInboundInquiry.ListPickdtl[0].dft_whs.Trim();
            if (l_str_DftWhs != "" || l_str_DftWhs != null)
            {
                objInboundInquiry.whs_id = l_str_DftWhs;
            }
            objInboundInquiry = ServiceObject.GetCntrValidation(objInboundInquiry);

            if (objInboundInquiry.ListPaletId.Count > 0)
            {
                objInboundInquiry.palet_id = objInboundInquiry.ListPaletId[0].palet_id;
            }
            else
            {

            }
            objInboundInquiry = ServiceObject.GetLotIDValidation(objInboundInquiry);
            if (objInboundInquiry.ListPaletId.Count > 0)
            {
                objInboundInquiry.palet_id = objInboundInquiry.ListPaletId[0].palet_id;
            }
            else
            {

            }
            objInboundInquiry = ServiceObject.GetPaletIdValidation(objInboundInquiry);
            if (objInboundInquiry.ListPaletId.Count > 0)
            {
                objInboundInquiry.palet_id = objInboundInquiry.ListPaletId[0].palet_id;
            }
            else
            {

            }
            objInboundInquiry = ServiceObject.Gettranstaus(objInboundInquiry);
            objInboundInquiry.tran_status = objInboundInquiry.ListRcvgDtl[0].tran_status;
            l_str_transtatus = objInboundInquiry.tran_status;
            if (l_str_transtatus != "ORIG")
            {

            }
            objInboundInquiry = ServiceObject.Getlotdtltext(objInboundInquiry);
            objInboundInquiry.cmp_id = objInboundInquiry.ListDocHdr[0].cmp_id;
            //objInboundInquiry.ib_doc_id = objInboundInquiry.ListDocHdr[0].ib_doc_id;
            //objInboundInquiry.lot_id = objInboundInquiry.ListDocHdr[0].lot_id;
            //objInboundInquiry.palet_id = objInboundInquiry.ListDocHdr[0].palet_id;
            objInboundInquiry.whs_id = objInboundInquiry.ListDocHdr[0].whs_id.Trim();
            //objInboundInquiry.loc_id = objInboundInquiry.ListDocHdr[0].loc_id.Trim();
            l_str_locId = objInboundInquiry.loc_id;
            objInboundInquiry.cont_id = objInboundInquiry.ListDocHdr[0].cont_id;
            objInboundInquiry.pkg_type = objInboundInquiry.ListDocHdr[0].pkg_type;
            objInboundInquiry.rate_id = objInboundInquiry.ListDocHdr[0].rate_id;
            objInboundInquiry.notes = objInboundInquiry.ListDocHdr[0].notes;
            l_str_rcd_dt = Convert.ToDateTime(objInboundInquiry.ListDocHdr[0].palet_dt);
            objInboundInquiry.ib_doc_dt = l_str_rcd_dt.ToString("MM/dd/yyyy");
            objCompany = ServiceObjectCompany.GetLocIdDetails(objCompany);
            objInboundInquiry.ListLocPickDtl = objCompany.ListLocPickDtl;
            objCompany = ServiceObjectCompany.GetCustConfigDtls(objCompany);
            objInboundInquiry.ListGetCustConfigDtls = objCompany.ListGetCustConfigDtls;
            objInboundInquiry.bill_type = objInboundInquiry.ListGetCustConfigDtls[0].bill_type;
            objInboundInquiry.bill_inout_type = objInboundInquiry.ListGetCustConfigDtls[0].bill_inout_type;
            objCompany.cust_cmp_id = CmpId;
            objCompany.whs_id = "";
            objCompany = ServiceObjectCompany.GetWhsIdDetails(objCompany);
            objInboundInquiry.ListwhsPickDtl = objCompany.ListwhsPickDtl;
            objInboundInquiry = ServiceObject.LoadStrgId(objInboundInquiry);
            for (int i = 0; i < objInboundInquiry.LstStrgIddtl.Count(); i++)
            {
                objInboundInquiry.strg_rate = "STRG-1";
                objInboundInquiry.strg_rate = objInboundInquiry.LstStrgIddtl[i].strg_rate.Trim();
            }
            if (objInboundInquiry.LstStrgIddtl.Count() == 0)
            {
                objInboundInquiry.strg_rate = "STRG-1";
            }
            objInboundInquiry = ServiceObject.LoadInoutId(objInboundInquiry);
            for (int i = 0; i < objInboundInquiry.LstInoutIddtl.Count(); i++)
            {
                objInboundInquiry.inout_rate = "INOUT-1";
                objInboundInquiry.inout_rate = objInboundInquiry.LstInoutIddtl[i].inout_rate;
            }
            if (objInboundInquiry.LstInoutIddtl.Count() == 0)
            {
                objInboundInquiry.inout_rate = "INOUT-1";
            }
            objInboundInquiry = ServiceObject.GetLotHdr(objInboundInquiry);
            if (objInboundInquiry.ListDocHdr.Count() == 0)
            {
                objInboundInquiry.rcvd_from = "-";
                objInboundInquiry.refno = "-";
                objInboundInquiry.seal_num = "-";
                objInboundInquiry.vend_id = "-";
            }
            else
            {
                objInboundInquiry.rcvd_from = objInboundInquiry.ListDocHdr[0].vend_name;
                objInboundInquiry.refno = objInboundInquiry.ListDocHdr[0].po_num;
                objInboundInquiry.seal_num = objInboundInquiry.ListDocHdr[0].seal_num;
                objInboundInquiry.vend_id = objInboundInquiry.ListDocHdr[0].rcvd_via;
            }
            objInboundInquiry = ServiceObject.GetGridlotdtl(objInboundInquiry);
            objInboundInquiry = ServiceObject.InsertTblIbDocRecvDtlTemp(objInboundInquiry);
            objInboundInquiry = ServiceObject.GetRecvdtlGrid(objInboundInquiry);
            objInboundInquiry = ServiceObject.GetRcvdEntryCountDtl(objInboundInquiry);
            objInboundInquiry.recvcount = objInboundInquiry.LstRcvdEntryCountDtl[0].recvcount;
            objInboundInquiry.View_Flag = "M";
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_ReceivingEntry", InboundInquiryModel);
        }

        public ActionResult ReceivingEntryEditSave(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_rcvd_dt, string p_str_rcvd_from, string p_str_refno,
       string p_str_vend_id, string p_str_whs_id, string p_str_cont_id, string p_str_seal_num, string p_str_palet_id, string p_str_lot_id, string p_str_loc_id)
        {
            string l_str_lot_id = string.Empty;
            string l_str_rcvd_itm_mode = string.Empty;
            string l_str_transtatus = string.Empty;
            string l_str_locId = string.Empty;
            string l_str_Kit_type = string.Empty;
            int l_str_KitQty = 0;
            decimal StRate = 0;
            decimal IORate = 0;
            int ResultCount = 0;
            DateTime l_str_rcd_dt;
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.ibdocid = p_str_ib_doc_id;
            objInboundInquiry.ib_doc_id = p_str_ib_doc_id;
            objInboundInquiry.lot_id = p_str_lot_id;
            objInboundInquiry.palet_id = p_str_palet_id;
            objInboundInquiry.whs_id = p_str_whs_id;
            objInboundInquiry.loc_id = p_str_loc_id;
            objInboundInquiry.cont_id = p_str_cont_id;
            objInboundInquiry.cntr_id = p_str_cont_id;
            objInboundInquiry.ib_doc_dt = p_str_rcvd_dt;
            objInboundInquiry.recvd_fm = p_str_rcvd_from;
            objInboundInquiry.refno = p_str_refno;
            objInboundInquiry = ServiceObject.GetRecvdtlGrid(objInboundInquiry);


            //   objInboundInquiry = ServiceObject.GetPalletId(objInboundInquiry);
            if (objInboundInquiry.ListRcvgDtl.Count() == 0)
            {
                return Json(ResultCount, JsonRequestBehavior.AllowGet);
            }
            for (int i = 0; i < objInboundInquiry.ListRcvgDtl.Count(); i++)
            {
                objInboundInquiry.itm_code = objInboundInquiry.ListRcvgDtl[i].itm_code;
                objInboundInquiry = ServiceObject.Del_Doc_qty_Mod(objInboundInquiry);
            }
            objInboundInquiry = ServiceObject.Del_iv_itm_trn(objInboundInquiry);
            for (int j = 0; j < objInboundInquiry.ListRcvgDtl.Count(); j++)
            {
                objInboundInquiry.itm_code = objInboundInquiry.ListRcvgDtl[j].itm_code;
                objInboundInquiry.rcvd_qty = objInboundInquiry.ListRcvgDtl[j].tot_qty;
                objInboundInquiry.ctn_qty = objInboundInquiry.ListRcvgDtl[j].tot_ctn;
                objInboundInquiry.dtl_line = objInboundInquiry.ListRcvgDtl[j].dtl_line;
                objInboundInquiry.ctn_line = objInboundInquiry.ListRcvgDtl[j].ctn_line;
                objInboundInquiry = ServiceObject.Update_Doc_tbl(objInboundInquiry);
                objInboundInquiry = ServiceObject.Update_Doc_ctn(objInboundInquiry);
                objInboundInquiry = ServiceObject.GetKitQty(objInboundInquiry);
                if (objInboundInquiry.ListPickdtl.Count != 0)
                {
                    l_str_Kit_type = objInboundInquiry.ListPickdtl[0].kit_itm;
                    l_str_KitQty = objInboundInquiry.ListPickdtl[0].tot_qty;
                }
                if (l_str_Kit_type == "N")
                {
                    l_str_KitQty = objInboundInquiry.ListPickdtl[j].ctn_qty;
                }
                objInboundInquiry.itm_num = objInboundInquiry.ListRcvgDtl[j].itm_num;
                objInboundInquiry.itm_color = objInboundInquiry.ListRcvgDtl[j].itm_color;
                objInboundInquiry.itm_size = objInboundInquiry.ListRcvgDtl[j].itm_size;
                objInboundInquiry.pkg_type = l_str_Kit_type;
                objInboundInquiry.itm_code = objInboundInquiry.ListRcvgDtl[j].itm_code;
                objInboundInquiry.kit_qty = l_str_KitQty;
                objInboundInquiry.st_rate_id = objInboundInquiry.ListRcvgDtl[j].st_rate_id;
                objInboundInquiry.io_rate_id = objInboundInquiry.ListRcvgDtl[j].io_rate_id;
                objInboundInquiry.po_num = objInboundInquiry.ListRcvgDtl[j].po_num;
                objInboundInquiry.length = objInboundInquiry.ListRcvgDtl[j].length;
                objInboundInquiry.width = objInboundInquiry.ListRcvgDtl[j].width;
                objInboundInquiry.depth = objInboundInquiry.ListRcvgDtl[j].depth;
                objInboundInquiry.wgt = objInboundInquiry.ListRcvgDtl[j].wgt;
                objInboundInquiry.cube = objInboundInquiry.ListRcvgDtl[j].cube;
                objInboundInquiry.itm_name = objInboundInquiry.ListRcvgDtl[j].itm_name;
                objInboundInquiry.ppk = objInboundInquiry.ListRcvgDtl[j].ctn_qty;
                objInboundInquiry.ctns = objInboundInquiry.ListRcvgDtl[j].tot_ctn;
                objInboundInquiry.dtl_line = objInboundInquiry.ListRcvgDtl[j].dtl_line;
                objInboundInquiry = ServiceObject.CkSTRate(objInboundInquiry);
                if (objInboundInquiry.ListPickdtl.Count != 0)
                {
                    StRate = objInboundInquiry.ListPickdtl[0].list_price;
                }
                else
                {
                    StRate = 0;
                }
                objInboundInquiry = ServiceObject.CkIORate(objInboundInquiry);
                if (objInboundInquiry.ListDocEntryDtl.Count != 0)
                {
                    IORate = objInboundInquiry.ListDocEntryDtl[0].list_price;
                }
                else
                {
                    IORate = 0;
                }
                objInboundInquiry.strg_rate = Convert.ToString(StRate);
                objInboundInquiry.rate_id = Convert.ToString(IORate);

                objInboundInquiry = ServiceObject.Add_To_Itm_Trn_in_CtnQty(objInboundInquiry);
                objInboundInquiry = ServiceObject.GetrecvEditTotQty(objInboundInquiry);
                if (objInboundInquiry.ListDocdtl.Count != 0)
                {
                    objInboundInquiry.ordr_qty = objInboundInquiry.ListDocdtl[0].ordr_qty;
                    objInboundInquiry.ctn_qty = objInboundInquiry.ListDocdtl[0].tot_ctn;

                }
                else
                {
                    objInboundInquiry.ordr_qty = 0;
                }
                objInboundInquiry = ServiceObject.Add_Iv_Lot_Dtl(objInboundInquiry);
                objInboundInquiry = ServiceObject.Add_tbl_iv_lot_ctn(objInboundInquiry);

            }
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("~/Views/InboundInquiry/InboundInquiry.cshtml", InboundInquiryModel);
        }

        public JsonResult DocrecvEntryEditDisplayGridToTextbox(string P_Str_CmpId, string P_Str_itmNum, string P_Str_Color, string P_Str_Size, string P_str_ibdocId,
            int P_str_LineNum, string P_str_itmCode, string P_str_ctn_line)
        {
            int OrderQty = 0;
            int l_int_rcvd_qty = 0;
            int l_int_temp_rcvd_qty = 0;
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            objInboundInquiry.cmp_id = P_Str_CmpId;
            objInboundInquiry.ib_doc_id = P_str_ibdocId;
            objInboundInquiry.line_num = P_str_LineNum;
            objInboundInquiry.itm_code = P_str_itmCode;
            objInboundInquiry.ibdocid = P_str_ibdocId;
            objInboundInquiry.ctn_line = Convert.ToInt32(P_str_ctn_line);
            objInboundInquiry.itm_num = P_Str_itmNum;
            objInboundInquiry.itm_color = P_Str_Color;
            objInboundInquiry.itm_size = P_Str_Size;
            Session["tempItmCode"] = objInboundInquiry.itm_code;
            objInboundInquiry = objService.SumTotqty(objInboundInquiry);


            objInboundInquiry = objService.GetGridRecvEditData(objInboundInquiry);
            if (objInboundInquiry.ListRcvgDtl.Count != 0)
            {
                objInboundInquiry.ordr_qty = objInboundInquiry.ListRcvgDtl[0].ordr_qty;
                OrderQty = objInboundInquiry.ordr_qty;
                //objInboundInquiry.ListDocRecvEntryDtl[0].ordr_qty = objInboundInquiry.ordr_qty;
            }
            else
            {
                objInboundInquiry.ordr_qty = 0;
            }
            if (objInboundInquiry.ListDocRecvEntryDtl.Count != 0)
            {
                objInboundInquiry.loc_id = (objInboundInquiry.ListDocRecvEntryDtl[0].loc_id == null || objInboundInquiry.ListDocRecvEntryDtl[0].loc_id.Trim() == "") ? "FLOOR" : objInboundInquiry.ListDocRecvEntryDtl[0].loc_id.Trim();
                objInboundInquiry.io_rate_id = (objInboundInquiry.ListDocRecvEntryDtl[0].io_rate_id == null || objInboundInquiry.ListDocRecvEntryDtl[0].io_rate_id.Trim() == "") ? "INOUT-1" : objInboundInquiry.ListDocRecvEntryDtl[0].io_rate_id.Trim();
                objInboundInquiry.st_rate_id = (objInboundInquiry.ListDocRecvEntryDtl[0].st_rate_id == null || objInboundInquiry.ListDocRecvEntryDtl[0].st_rate_id.Trim() == "") ? "STRG-1" : objInboundInquiry.ListDocRecvEntryDtl[0].st_rate_id.Trim();
                objInboundInquiry.itm_num = objInboundInquiry.ListDocRecvEntryDtl[0].itm_num.Trim();
                objInboundInquiry.itm_color = objInboundInquiry.ListDocRecvEntryDtl[0].itm_color.Trim();
                objInboundInquiry.itm_size = objInboundInquiry.ListDocRecvEntryDtl[0].itm_size.Trim();
                objInboundInquiry.itm_name = objInboundInquiry.ListDocRecvEntryDtl[0].itm_name.Trim();
                objInboundInquiry.length = objInboundInquiry.ListDocRecvEntryDtl[0].length;
                objInboundInquiry.width = objInboundInquiry.ListDocRecvEntryDtl[0].width;
                objInboundInquiry.depth = objInboundInquiry.ListDocRecvEntryDtl[0].depth;
                objInboundInquiry.wgt = objInboundInquiry.ListDocRecvEntryDtl[0].wgt;
                objInboundInquiry.cube = objInboundInquiry.ListDocRecvEntryDtl[0].cube;
                objInboundInquiry.dtl_line = objInboundInquiry.ListDocRecvEntryDtl[0].dtl_line;
                objInboundInquiry.ctn_line = objInboundInquiry.ListDocRecvEntryDtl[0].ctn_line;
                objInboundInquiry.tot_qty = objInboundInquiry.ListDocRecvEntryDtl[0].tot_qty;
                objInboundInquiry.tot_ctn = objInboundInquiry.ListDocRecvEntryDtl[0].tot_ctn;
                objInboundInquiry.ctn_qty = objInboundInquiry.ListDocRecvEntryDtl[0].ctn_qty;
                objInboundInquiry.po_num = objInboundInquiry.ListDocRecvEntryDtl[0].po_num.Trim();
                objInboundInquiry.itm_code = objInboundInquiry.ListDocRecvEntryDtl[0].itm_code.Trim();
                objInboundInquiry.ib_doc_id = objInboundInquiry.ListDocRecvEntryDtl[0].ib_doc_id.Trim();
            }
            objInboundInquiry = objService.GetItemRcvdQty(objInboundInquiry);
            if (objInboundInquiry.ListItemRcvdQty.Count != 0)
            {
                l_int_rcvd_qty = objInboundInquiry.ListItemRcvdQty[0].rcvd_qty;
                //l_int_temp_rcvd_qty = l_int_rcvd_qty - objInboundInquiry.tot_ctn;
            }

            List<InboundInquiry> LISTJSONGETVALUE = new List<InboundInquiry>();
            List<InboundInquiry> LISTJSONSETVALUE = new List<InboundInquiry>();
            objInboundInquiry.loc_id = objInboundInquiry.loc_id.Trim();
            objInboundInquiry.io_rate_id = objInboundInquiry.io_rate_id.Trim();
            objInboundInquiry.st_rate_id = objInboundInquiry.st_rate_id.Trim();
            objInboundInquiry.itm_num = objInboundInquiry.itm_num.Trim();
            objInboundInquiry.itm_color = objInboundInquiry.itm_color.Trim();
            objInboundInquiry.itm_size = objInboundInquiry.itm_size.Trim();
            objInboundInquiry.itm_name = objInboundInquiry.itm_name.Trim();
            objInboundInquiry.length = objInboundInquiry.length;
            objInboundInquiry.width = objInboundInquiry.width;
            objInboundInquiry.depth = objInboundInquiry.depth;
            objInboundInquiry.wgt = objInboundInquiry.wgt;
            objInboundInquiry.cube = objInboundInquiry.cube;
            objInboundInquiry.dtl_line = objInboundInquiry.dtl_line;
            objInboundInquiry.ctn_line = objInboundInquiry.ctn_line;
            objInboundInquiry.tot_qty = objInboundInquiry.ordr_qty;
            objInboundInquiry.tot_ctn = objInboundInquiry.tot_ctn;
            objInboundInquiry.ctn_qty = objInboundInquiry.ctn_qty;
            objInboundInquiry.po_num = objInboundInquiry.po_num.Trim();
            objInboundInquiry.itm_code = objInboundInquiry.itm_code.Trim();
            objInboundInquiry.ib_doc_id = objInboundInquiry.ib_doc_id.Trim();
            objInboundInquiry.ordr_qty = OrderQty;
            objInboundInquiry.rcvd_qty = l_int_rcvd_qty;
            //  objInboundInquiry.balance = OrderQty - l_int_rcvd_qty;
            objInboundInquiry.balance = objInboundInquiry.tot_ctn * objInboundInquiry.ctn_qty;

            // objInboundInquiry.recving_qty = OrderQty - l_int_rcvd_qty;
            objInboundInquiry.recving_qty = objInboundInquiry.tot_ctn * objInboundInquiry.ctn_qty;

            objInboundInquiry.tot_qty = objInboundInquiry.tot_ctn * objInboundInquiry.ctn_qty;
            // CR_3PL_MVC_IB_DOC_RECV_2018_0324_001
            //if (OrderQty > (l_int_rcvd_qty + objInboundInquiry.tot_qty))
            //{
            //    objInboundInquiry.tot_qty = objInboundInquiry.balance;
            //    objInboundInquiry.tot_ctn = objInboundInquiry.balance;
            //    objInboundInquiry.ctn_qty = 1;
            //}
            // End CR_3PL_MVC_IB_DOC_RECV_2018_0324_001
            //objInboundInquiry.ppk = objInboundInquiry.tot_ctn * objInboundInquiry.ctn_qty;
            LISTJSONGETVALUE.Add(objInboundInquiry);
            LISTJSONSETVALUE = LISTJSONGETVALUE;

            //objInboundInquiry.loc_id = objInboundInquiry.ListDocRecvEntryDtl[0].loc_id.Trim();
            //objInboundInquiry.io_rate_id = objInboundInquiry.ListDocRecvEntryDtl[0].io_rate_id.Trim();
            //objInboundInquiry.st_rate_id = objInboundInquiry.ListDocRecvEntryDtl[0].st_rate_id.Trim();
            //objInboundInquiry.ListDocRecvEntryDtl[1].ordr_qty = objInboundInquiry.ordr_qty;
            //objInboundInquiry.ListDocRecvEntryDtl[0].loc_id = objInboundInquiry.loc_id.Trim();
            //objInboundInquiry.ListDocRecvEntryDtl[0].io_rate_id = objInboundInquiry.io_rate_id.Trim();
            //objInboundInquiry.ListDocRecvEntryDtl[0].st_rate_id = objInboundInquiry.st_rate_id.Trim();
            return Json(LISTJSONSETVALUE, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReceivingEntryView(string CmpId, string id, string Cont_id, string LotId)
        {
            string l_str_lot_id = string.Empty;
            string l_str_rcvd_itm_mode = string.Empty;
            string l_str_transtatus = string.Empty;
            string l_str_locId = string.Empty;
            DateTime l_str_rcd_dt;
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objInboundInquiry.cmp_id = CmpId;
            objInboundInquiry.ibdocid = id;
            objInboundInquiry.ib_doc_id = id;
            objInboundInquiry.cntr_id = Cont_id;
            ServiceObject.GetIBRecvDeleteTempData(objInboundInquiry);     //CR_3PL_MVC_BL_2018_0306_001 
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objInboundInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objCompany.cust_cmp_id = CmpId;
            objInboundInquiry.lot_id = LotId;
            objInboundInquiry.cmp_id = CmpId;
            objInboundInquiry.ib_doc_id = id;
            objInboundInquiry.loc_id = "FLOOR";
            objInboundInquiry = ServiceObject.GetDftWhs(objInboundInquiry);
            string l_str_DftWhs = objInboundInquiry.ListPickdtl[0].dft_whs.Trim();
            if (l_str_DftWhs != "" || l_str_DftWhs != null)
            {
                objInboundInquiry.whs_id = l_str_DftWhs;
            }
            objInboundInquiry = ServiceObject.GetCntrValidation(objInboundInquiry);

            if (objInboundInquiry.ListPaletId.Count > 0)
            {
                objInboundInquiry.palet_id = objInboundInquiry.ListPaletId[0].palet_id;
            }
            else
            {

            }
            objInboundInquiry = ServiceObject.GetLotIDValidation(objInboundInquiry);
            if (objInboundInquiry.ListPaletId.Count > 0)
            {
                objInboundInquiry.palet_id = objInboundInquiry.ListPaletId[0].palet_id;
            }
            else
            {

            }
            objInboundInquiry = ServiceObject.GetPaletIdValidation(objInboundInquiry);
            if (objInboundInquiry.ListPaletId.Count > 0)
            {
                objInboundInquiry.palet_id = objInboundInquiry.ListPaletId[0].palet_id;
            }
            else
            {

            }
            objInboundInquiry = ServiceObject.Gettranstaus(objInboundInquiry);
            objInboundInquiry.tran_status = objInboundInquiry.ListRcvgDtl[0].tran_status;
            l_str_transtatus = objInboundInquiry.tran_status;
            if (l_str_transtatus != "ORIG")
            {

            }
            objInboundInquiry = ServiceObject.Getlotdtltext(objInboundInquiry);
            objInboundInquiry.cmp_id = objInboundInquiry.ListDocHdr[0].cmp_id;
            //objInboundInquiry.ib_doc_id = objInboundInquiry.ListDocHdr[0].ib_doc_id;
            //objInboundInquiry.lot_id = objInboundInquiry.ListDocHdr[0].lot_id;
            //objInboundInquiry.palet_id = objInboundInquiry.ListDocHdr[0].palet_id;
            objInboundInquiry.whs_id = objInboundInquiry.ListDocHdr[0].whs_id.Trim();
            objInboundInquiry.loc_id = objInboundInquiry.ListDocHdr[0].loc_id.Trim();
            l_str_locId = objInboundInquiry.loc_id;
            objInboundInquiry.cont_id = objInboundInquiry.ListDocHdr[0].cont_id;
            objInboundInquiry.pkg_type = objInboundInquiry.ListDocHdr[0].pkg_type;
            objInboundInquiry.rate_id = objInboundInquiry.ListDocHdr[0].rate_id;
            objInboundInquiry.notes = objInboundInquiry.ListDocHdr[0].notes;
            l_str_rcd_dt = Convert.ToDateTime(objInboundInquiry.ListDocHdr[0].palet_dt);
            objInboundInquiry.ib_doc_dt = l_str_rcd_dt.ToString("MM/dd/yyyy");
            objCompany = ServiceObjectCompany.GetLocIdDetails(objCompany);
            objInboundInquiry.ListLocPickDtl = objCompany.ListLocPickDtl;
            objCompany = ServiceObjectCompany.GetCustConfigDtls(objCompany);
            objInboundInquiry.ListGetCustConfigDtls = objCompany.ListGetCustConfigDtls;
            objInboundInquiry.bill_type = objInboundInquiry.ListGetCustConfigDtls[0].bill_type;
            objInboundInquiry.bill_inout_type = objInboundInquiry.ListGetCustConfigDtls[0].bill_inout_type;
            objCompany.cust_cmp_id = CmpId;
            objCompany.whs_id = "";
            objCompany = ServiceObjectCompany.GetWhsIdDetails(objCompany);
            objInboundInquiry.ListwhsPickDtl = objCompany.ListwhsPickDtl;
            objInboundInquiry = ServiceObject.LoadStrgId(objInboundInquiry);
            for (int i = 0; i < objInboundInquiry.LstStrgIddtl.Count(); i++)
            {
                objInboundInquiry.strg_rate = "STRG-1";
                objInboundInquiry.strg_rate = objInboundInquiry.LstStrgIddtl[i].strg_rate;
            }
            if (objInboundInquiry.LstStrgIddtl.Count() == 0)
            {
                objInboundInquiry.strg_rate = "STRG-1";
            }
            objInboundInquiry = ServiceObject.LoadInoutId(objInboundInquiry);
            for (int i = 0; i < objInboundInquiry.LstInoutIddtl.Count(); i++)
            {
                objInboundInquiry.inout_rate = "INOUT-1";
                objInboundInquiry.inout_rate = objInboundInquiry.LstInoutIddtl[i].inout_rate;
            }
            if (objInboundInquiry.LstInoutIddtl.Count() == 0)
            {
                objInboundInquiry.inout_rate = "INOUT-1";
            }
            objInboundInquiry = ServiceObject.GetLotHdr(objInboundInquiry);
            if (objInboundInquiry.ListDocHdr.Count() == 0)
            {
                objInboundInquiry.rcvd_from = "-";
                objInboundInquiry.refno = "-";
                objInboundInquiry.seal_num = "-";
                objInboundInquiry.vend_id = "-";
            }
            else
            {
                objInboundInquiry.rcvd_from = objInboundInquiry.ListDocHdr[0].vend_name;
                objInboundInquiry.refno = objInboundInquiry.ListDocHdr[0].po_num;
                objInboundInquiry.seal_num = objInboundInquiry.ListDocHdr[0].seal_num;
                objInboundInquiry.vend_id = objInboundInquiry.ListDocHdr[0].rcvd_via;
            }
            objInboundInquiry = ServiceObject.GetGridlotdtl(objInboundInquiry);
            objInboundInquiry = ServiceObject.InsertTblIbDocRecvDtlTemp(objInboundInquiry);
            objInboundInquiry = ServiceObject.GetRecvdtlGrid(objInboundInquiry);
            objInboundInquiry = ServiceObject.GetRcvdEntryCountDtl(objInboundInquiry);//CR2018615
            objInboundInquiry.recvcount = objInboundInquiry.LstRcvdEntryCountDtl[0].recvcount;
            objInboundInquiry.View_Flag = "V";
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_ReceivingEntry", InboundInquiryModel);
        }
        public ActionResult IncludeRecvDetails(string p_str_cmp_id, string p_str_ibdocid, string p_str_line_num, string p_str_ctn_line, string p_str_style, string p_str_color, string p_str_size,
            string p_str_desc, string p_str_Itmcode, int p_str_ord_qty, int p_str_ppk, int p_str_ctns, string p_str_po_num, decimal p_str_length, decimal p_str_width,
            decimal p_str_height, decimal p_str_weight, decimal p_str_cube, int p_str_tot_qty, string p_str_strg_rate, string p_str_inout_rate, string p_str_loc_id, string p_str_lot_id, string p_str_palet_id)
        {
            int l_int_ordr_qty1;
            int l_int_ctn_qty1;
            int l_int_tmp_ordr_qty;
            int l_int_rcvd_qty = 0;
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.ibdocid = p_str_ibdocid;
            objInboundInquiry.ib_doc_id = p_str_ibdocid;
            objInboundInquiry.line_num = Convert.ToInt32(p_str_line_num);
            objInboundInquiry.ctn_line = Convert.ToInt32(p_str_ctn_line);
            objInboundInquiry.itm_line = Convert.ToInt32(p_str_ctn_line);
            objInboundInquiry.itm_num = p_str_style;
            objInboundInquiry.itm_color = p_str_color;
            objInboundInquiry.itm_size = p_str_size;
            objInboundInquiry.itm_name = p_str_desc;
            objInboundInquiry.tot_qty = p_str_ord_qty;
            objInboundInquiry.ctn_qty = p_str_ppk;
            objInboundInquiry.tot_ctn = p_str_ctns;
            objInboundInquiry.po_num = p_str_po_num;
            objInboundInquiry.length = p_str_length;
            objInboundInquiry.width = p_str_width;
            objInboundInquiry.depth = p_str_height;
            objInboundInquiry.cube = p_str_cube;
            objInboundInquiry.tot_qty = p_str_tot_qty;
            objInboundInquiry.st_rate_id = p_str_strg_rate;
            objInboundInquiry.io_rate_id = p_str_inout_rate;
            objInboundInquiry.loc_id = p_str_loc_id;
            objInboundInquiry.lot_id = p_str_lot_id;
            objInboundInquiry.palet_id = p_str_palet_id;
            objInboundInquiry.itm_code = p_str_Itmcode;
            objInboundInquiry.wgt = p_str_weight;
            l_int_tmp_ordr_qty = p_str_tot_qty;
            l_int_ordr_qty1 = l_int_tmp_ordr_qty - ((l_int_tmp_ordr_qty) % (p_str_ppk));
            l_int_ctn_qty1 = l_int_tmp_ordr_qty % (p_str_ppk);
            objInboundInquiry = ServiceObject.LoadCustConfig(objInboundInquiry);
            if (objInboundInquiry.ListCustConfigDetails.Count() != 0)
            {
                objInboundInquiry.Allow_New_item = objInboundInquiry.ListCustConfigDetails[0].Allow_New_item;
            }

            if (objInboundInquiry.Allow_New_item == "Y")
            {
                objInboundInquiry = ServiceObject.GetItmCode(objInboundInquiry);
                if (objInboundInquiry.ListDocRecvEntryDtl.Count != 0)
                {
                    objInboundInquiry.itm_code = objInboundInquiry.ListDocRecvEntryDtl[0].itm_code;
                }
                else
                {
                    int l_str_Count = 0;
                    return Json(l_str_Count, JsonRequestBehavior.AllowGet);
                }
                objInboundInquiry = ServiceObject.GetItmName(objInboundInquiry);
                if (objInboundInquiry.ListPickdtl.Count() != 0)
                {
                    objInboundInquiry = ServiceObject.UpdtItmDtl(objInboundInquiry);
                }
                //objInboundInquiry = ServiceObject.InsertTblIbDocRecvDtlTemp(objInboundInquiry);
                objInboundInquiry = ServiceObject.GetCheckExistGridDataRecvEntry(objInboundInquiry);
                objInboundInquiry = ServiceObject.InsertRecvEntryTemptable(objInboundInquiry);
                objInboundInquiry = ServiceObject.GetItemRcvdQty(objInboundInquiry);
                //objInboundInquiry = ServiceObject.GetRecvdtlGrid(objInboundInquiry);
                if (objInboundInquiry.ListItemRcvdQty.Count != 0)
                {
                    l_int_rcvd_qty = objInboundInquiry.ListItemRcvdQty[0].rcvd_qty;
                }
                objInboundInquiry.balance = p_str_ord_qty - (p_str_tot_qty + l_int_rcvd_qty);
                TempData["balance"] = objInboundInquiry.balance;
                objInboundInquiry.rcvd_qty = p_str_tot_qty + l_int_rcvd_qty;
                TempData["rcvd_qty"] = objInboundInquiry.rcvd_qty;
                objInboundInquiry.ordr_qty = p_str_ord_qty;
                TempData["ordr_qty"] = objInboundInquiry.ordr_qty;
                objInboundInquiry = ServiceObject.GetCtnLineNo(objInboundInquiry);
                //objInboundInquiry = ServiceObject.GetRecvdtlGrid(objInboundInquiry);
                if (objInboundInquiry.ListCtnLine.Count != 0)
                {
                    TempData["ctn_line"] = objInboundInquiry.ListCtnLine[0].ctn_line;
                }
                TempData["dtl_line"] = p_str_line_num;
                objInboundInquiry = ServiceObject.GetRecvdtlGrid(objInboundInquiry);
                objInboundInquiry.palet_id = p_str_palet_id;
                objInboundInquiry = ServiceObject.GetRecvEntryCount(objInboundInquiry);

            }
            else
            {
                objInboundInquiry = ServiceObject.GetItmCode(objInboundInquiry);
                objInboundInquiry.itm_code = objInboundInquiry.ListDocRecvEntryDtl[0].itm_code;
                objInboundInquiry = ServiceObject.GetItmName(objInboundInquiry);
                if (objInboundInquiry.ListPickdtl.Count() != 0)
                {
                    objInboundInquiry = ServiceObject.UpdtItmDtl(objInboundInquiry);
                }
                //objInboundInquiry = ServiceObject.InsertTblIbDocRecvDtlTemp(objInboundInquiry);
                objInboundInquiry = ServiceObject.GetCheckExistGridDataRecvEntry(objInboundInquiry);

                objInboundInquiry = ServiceObject.InsertRecvEntryTemptable(objInboundInquiry);
                objInboundInquiry = ServiceObject.GetItemRcvdQty(objInboundInquiry);
                //objInboundInquiry = ServiceObject.GetRecvdtlGrid(objInboundInquiry);
                if (objInboundInquiry.ListItemRcvdQty.Count != 0)
                {
                    l_int_rcvd_qty = objInboundInquiry.ListItemRcvdQty[0].rcvd_qty;
                }
                objInboundInquiry.balance = p_str_ord_qty - (p_str_tot_qty + l_int_rcvd_qty);
                TempData["balance"] = objInboundInquiry.balance;
                objInboundInquiry.rcvd_qty = p_str_tot_qty + l_int_rcvd_qty;
                TempData["rcvd_qty"] = objInboundInquiry.rcvd_qty;
                objInboundInquiry.ordr_qty = p_str_ord_qty;
                TempData["ordr_qty"] = objInboundInquiry.ordr_qty;
                objInboundInquiry = ServiceObject.GetCtnLineNo(objInboundInquiry);
                //objInboundInquiry = ServiceObject.GetRecvdtlGrid(objInboundInquiry);
                if (objInboundInquiry.ListCtnLine.Count != 0)
                {
                    TempData["ctn_line"] = objInboundInquiry.ListCtnLine[0].ctn_line;
                }
                TempData["dtl_line"] = p_str_line_num;
                objInboundInquiry = ServiceObject.GetRecvdtlGrid(objInboundInquiry);
                objInboundInquiry.palet_id = p_str_palet_id;
                objInboundInquiry = ServiceObject.GetRecvEntryCount(objInboundInquiry);
            }
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.ib_doc_id = p_str_ibdocid;
            objInboundInquiry.lot_id = p_str_lot_id;
            objInboundInquiry = ServiceObject.GetRcvdEntryCountDtl(objInboundInquiry);
            objInboundInquiry.recvcount = objInboundInquiry.LstRcvdEntryCountDtl[0].recvcount;
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_ReceivingEntryGrid", InboundInquiryModel);
        }
        public JsonResult DocRecvEntrydtlCount(string P_Str_CmpId, string P_Str_IbdocId)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            objInboundInquiry.cmp_id = P_Str_CmpId;

            objInboundInquiry.ib_doc_id = P_Str_IbdocId;
            objInboundInquiry.lot_id = "";
            objInboundInquiry = objService.GetRcvdEntryCountDtl(objInboundInquiry);
            objInboundInquiry.recvcount = objInboundInquiry.LstRcvdEntryCountDtl[0].recvcount;
            return Json(objInboundInquiry.recvcount, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTempPassingData(string P_Str_CmpId, string P_Str_IbdocId)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            objInboundInquiry.balance = Convert.ToInt32(TempData["balance"]);
            objInboundInquiry.rcvd_qty = Convert.ToInt32(TempData["rcvd_qty"]);
            objInboundInquiry.ordr_qty = Convert.ToInt32(TempData["ordr_qty"]);
            objInboundInquiry.ctn_line = Convert.ToInt32(TempData["ctn_line"]);
            objInboundInquiry.dtl_line = Convert.ToInt32(TempData["dtl_line"]);

            return Json(objInboundInquiry, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DocRecvEntryCount(string P_Str_CmpId, string P_Str_IbdocId)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            objInboundInquiry.cmp_id = P_Str_CmpId;
            objInboundInquiry.ib_doc_id = P_Str_IbdocId;
            objInboundInquiry = objService.GetRecvEntryCount(objInboundInquiry);
            objInboundInquiry.DocEntryCount = objInboundInquiry.ListGetDocEntryCount[0].DocCount;
            objInboundInquiry = objService.GetRecvdtlGrid(objInboundInquiry);
            return Json(objInboundInquiry.DocEntryCount, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DocRecvEntryDeleteGridData(string P_Str_CmpId, string P_Str_IbdocId, string P_Str_LineNum, string P_Str_ItmCode, string P_Str_ctnLine)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            objInboundInquiry.cmp_id = P_Str_CmpId;
            objInboundInquiry.line_num = Convert.ToInt32(P_Str_LineNum);
            objInboundInquiry.ib_doc_id = P_Str_IbdocId;
            objInboundInquiry.itm_code = P_Str_ItmCode;
            objInboundInquiry.ctn_line = Convert.ToInt32(P_Str_ctnLine);
            objInboundInquiry = objService.GetRecvEntryGridDeleteData(objInboundInquiry);
            objInboundInquiry = objService.GetRecvEntryCount(objInboundInquiry);
            objInboundInquiry.DocEntryCount = objInboundInquiry.ListGetDocEntryCount[0].DocCount;
            objInboundInquiry = objService.GetRecvdtlGrid(objInboundInquiry);
            objInboundInquiry.GrdFlag = "D";
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_ReceivingEntryGrid", InboundInquiryModel);
        }
        //CR - 3PL_MVC_IB_2018_0219_008
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
                            ReportDocument rd = new ReportDocument();
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                            objBillingInquiry = ServiceObject.GetBillingBillDocSTRGRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else
                        {
                            strReportName = "rpt_st_bill_doc.rpt";
                            ReportDocument rd = new ReportDocument();
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                            objBillingInquiry = ServiceObject.GetBillingBillDocSTRGRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }

                    }
                    if (l_str_rpt_bill_type.Trim() == "Cube")

                    {
                        if (l_str_rpt_instrg_req.Trim() == "Y")
                        {
                            strReportName = "rpt_st_bill_doc_bycube.rpt";
                            ReportDocument rd = new ReportDocument();
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                            objBillingInquiry = ServiceObject.GetBillingBillDocCubeSTRGRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else
                        {
                            strReportName = "rpt_st_bill_doc_bycube.rpt";
                            ReportDocument rd = new ReportDocument();
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                            objBillingInquiry = ServiceObject.GetBillingBillDocCubeSTRGRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }

                    }
                    if (l_str_rpt_bill_type.Trim() == "Pallet")

                    {

                        strReportName = "rpt_strg_bill_by_pallet.rpt";
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                        objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        objBillingInquiry = ServiceObject.GetBillingBillDocPalletSTRGRpt(objBillingInquiry);
                        var rptSource = objBillingInquiry.ListGenBillingStrgByPalletRpt.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objBillingInquiry.ListGenBillingStrgByPalletRpt.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");


                    }
                }
                if (l_str_rpt_bill_doc_type == "INOUT")
                {

                    if (l_str_rpt_bill_inout_type.Trim() == "Carton")

                    {
                        if (l_str_rpt_instrg_req.Trim() == "Y")
                        {
                            strReportName = "rpt_inout_bill_doc_with_initstrg.rpt";
                            ReportDocument rd = new ReportDocument();
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                            objBillingInquiry = ServiceObject.GetBillingBillInoutCartonInstrgRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListBillingInoutCartonInstrgRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objBillingInquiry.ListBillingInoutCartonInstrgRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else
                        {
                            strReportName = "rpt_inout_bill_doc.rpt";
                            ReportDocument rd = new ReportDocument();
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                            objBillingInquiry = ServiceObject.GetBillingBillInoutCartonRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListBillingInoutCartonRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objBillingInquiry.ListBillingInoutCartonRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }

                    }
                    if (l_str_rpt_bill_inout_type.Trim() == "Cube")

                    {
                        if (l_str_rpt_instrg_req.Trim() == "Y")
                        {
                            strReportName = "rpt_inout_bill_doc_bycube_with_initstrg.rpt";
                            ReportDocument rd = new ReportDocument();
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                            objBillingInquiry = ServiceObject.GetBillingBillInoutCubeInstrgRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListBillingInoutCubeInstrgRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objBillingInquiry.ListBillingInoutCubeInstrgRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else
                        {
                            strReportName = "rpt_inout_bill_doc_bycube.rpt";
                            ReportDocument rd = new ReportDocument();
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                            objBillingInquiry = ServiceObject.GetBillingBillInoutCubeRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListBillingInoutCubeRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objBillingInquiry.ListBillingInoutCubeRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }

                    }
                    if (l_str_rpt_bill_inout_type.Trim() == "Container")

                    {
                        strReportName = "rpt_inout_bill_by_Container.rpt";
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                        objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        objBillingInquiry = ServiceObject.GetBillingBillDocContainerInoutRpt(objBillingInquiry);
                        var rptSource = objBillingInquiry.ListGenBillingInoutByContainerRpt.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objBillingInquiry.ListGenBillingInoutByContainerRpt.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");


                    }

                }
                if (l_str_rpt_bill_doc_type == "NORM")
                {
                    strReportName = "rpt_va_bill_doc.rpt";
                    ReportDocument rd = new ReportDocument();
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                    objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                    objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                    objBillingInquiry = ServiceObject.GetBillingBillDocVASRpt(objBillingInquiry);
                    var rptSource = objBillingInquiry.ListBillingDocVASRpt.ToList();
                    rd.Load(strRptPath);
                    int AlocCount = 0;
                    AlocCount = objBillingInquiry.ListBillingDocVASRpt.Count();
                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                        rd.SetDataSource(rptSource);
                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                jsonErrorCode = "-2";
            }

            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);
        }
        //CR - 3PL_MVC_IB_2018_0219_008

        //CR_MVC_3PL_0317-001 Added By Nithya
        public ActionResult CancelEditDocDetails(string p_str_cmp_id, string p_str_ibdocid)
        {
            try
            {

                InboundInquiry objInboundInquiry = new InboundInquiry();
                InboundInquiryService objService = new InboundInquiryService();
                objInboundInquiry.cmp_id = p_str_cmp_id;
                objInboundInquiry.ib_doc_id = p_str_ibdocid;

                objInboundInquiry = objService.GetDocumentEntryTempGridDtl(objInboundInquiry);
                objInboundInquiry = objService.GetDocEntryCount(objInboundInquiry);
                Session["l_bool_edit_flag"] = "false";
                objInboundInquiry.l_bool_edit_flag = Session["l_bool_edit_flag"].ToString();//CR_MVC_3PL_0317-001 Added By Nithya l_bool_is_in_edit  
                Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
                InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
                return PartialView("_DocEntryEditGrid", InboundInquiryModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult ShowdtlReports(string p_str_cmpid, string p_str_status, string p_str_bill_doc_id)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string cmp_id = p_str_cmpid;
            string l_str_status = string.Empty;
            string l_str_rpt_bill_type = string.Empty;
            string l_str_rpt_bill_inout_type = string.Empty;
            string l_str_rpt_instrg_req = string.Empty;
            string l_str_rpt_bill_doc_type = string.Empty;

            try
            {
                if (isValid)
                {
                    if (p_str_status == "OPEN")

                    {
                        BillingInquiry objBillingInquiry = new BillingInquiry();
                        BillingInquiryService ServiceObject = new BillingInquiryService();
                        objBillingInquiry.cmp_id = p_str_cmpid;
                        objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                        objBillingInquiry = ServiceObject.GetBillingBillingType(objBillingInquiry);
                        l_str_rpt_bill_type = objBillingInquiry.ListBillingType[0].bill_type;
                        objBillingInquiry = ServiceObject.GetBillingInoutType(objBillingInquiry);
                        l_str_rpt_bill_inout_type = objBillingInquiry.ListBillingInoutType[0].bill_inout_type;
                        l_str_rpt_instrg_req = objBillingInquiry.ListBillingType[0].init_strg_rt_req;
                        objBillingInquiry = ServiceObject.GetBillingBillDocIdType(objBillingInquiry);
                        objBillingInquiry.cmp_id = p_str_cmpid;
                        objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                        l_str_rpt_bill_doc_type = objBillingInquiry.ListBillingDocIdType[0].bill_type;

                        if (l_str_rpt_bill_doc_type == "STRG")
                        {
                            if (l_str_rpt_bill_type.Trim() == "Carton")

                            {
                                if (l_str_rpt_instrg_req.Trim() == "Y")
                                {
                                    strReportName = "rpt_st_bill_doc.rpt";
                                    ReportDocument rd = new ReportDocument();
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmpid;
                                    objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                                    objBillingInquiry = ServiceObject.GetBillingBillDocSTRGRpt(objBillingInquiry);
                                    var rptSource = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.ToList();
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);

                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                                else
                                {
                                    strReportName = "rpt_st_bill_doc.rpt";
                                    ReportDocument rd = new ReportDocument();
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmpid;
                                    objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                                    objBillingInquiry = ServiceObject.GetBillingBillDocSTRGRpt(objBillingInquiry);
                                    var rptSource = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.ToList();
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
                            if (l_str_rpt_bill_type.Trim() == "Cube")

                            {
                                if (l_str_rpt_instrg_req.Trim() == "Y")
                                {
                                    strReportName = "rpt_st_bill_doc_bycube.rpt";
                                    ReportDocument rd = new ReportDocument();
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmpid;
                                    objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                                    objBillingInquiry = ServiceObject.GetBillingBillDocCubeSTRGRpt(objBillingInquiry);
                                    var rptSource = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.ToList();
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    //objBillingInquiry = ServiceObject.GetBillingBillDocPalletSTRGRpt(objBillingInquiry);
                                    //objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                                else
                                {
                                    strReportName = "rpt_st_bill_doc_bycube.rpt";
                                    ReportDocument rd = new ReportDocument();
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmpid;
                                    objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                                    objBillingInquiry = ServiceObject.GetBillingBillDocCubeSTRGRpt(objBillingInquiry);
                                    var rptSource = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.ToList();
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
                            //CR-3PL_MVC_BL_2018-03-10-001
                            if (l_str_rpt_bill_type.Trim() == "Pallet")

                            {
                                strReportName = "rpt_strg_bill_by_pallet.rpt";
                                ReportDocument rd = new ReportDocument();
                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                objBillingInquiry.cmp_id = p_str_cmpid.Trim();
                                objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();

                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                objBillingInquiry = ServiceObject.GetBillingBillDocPalletSTRGRpt(objBillingInquiry);
                                var rptSource = objBillingInquiry.ListGenBillingStrgByPalletRpt.ToList();
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
                        if (l_str_rpt_bill_doc_type == "NORM")
                        {
                            strReportName = "rpt_va_bill_doc.rpt";
                            ReportDocument rd = new ReportDocument();
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmpid;
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                            objBillingInquiry = ServiceObject.GetBillingBillDocVASRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListBillingDocVASRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objBillingInquiry.ListBillingDocVASRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        if (l_str_rpt_bill_doc_type == "INOUT")
                        {

                            if (l_str_rpt_bill_inout_type.Trim() == "Carton")

                            {
                                if (l_str_rpt_instrg_req.Trim() == "Y")
                                {
                                    strReportName = "rpt_inout_bill_doc_with_initstrg.rpt";
                                    ReportDocument rd = new ReportDocument();
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmpid;
                                    objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                                    objBillingInquiry = ServiceObject.GetBillingBillInoutCartonInstrgRpt(objBillingInquiry);
                                    var rptSource = objBillingInquiry.ListBillingInoutCartonInstrgRpt.ToList();
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objBillingInquiry.ListBillingInoutCartonInstrgRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                                else
                                {
                                    strReportName = "rpt_inout_bill_doc.rpt";
                                    ReportDocument rd = new ReportDocument();
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmpid;
                                    objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                                    objBillingInquiry = ServiceObject.GetBillingBillInoutCartonRpt(objBillingInquiry);
                                    var rptSource = objBillingInquiry.ListBillingInoutCartonRpt.ToList();
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objBillingInquiry.ListBillingInoutCartonRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }

                            }
                            if (l_str_rpt_bill_inout_type.Trim() == "Cube")

                            {
                                if (l_str_rpt_instrg_req.Trim() == "Y")
                                {
                                    strReportName = "rpt_inout_bill_doc_bycube_with_initstrg.rpt";
                                    ReportDocument rd = new ReportDocument();
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmpid;
                                    objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                                    objBillingInquiry = ServiceObject.GetBillingBillInoutCubeInstrgRpt(objBillingInquiry);
                                    var rptSource = objBillingInquiry.ListBillingInoutCubeInstrgRpt.ToList();
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objBillingInquiry.ListBillingInoutCubeInstrgRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                                else
                                {
                                    strReportName = "rpt_inout_bill_doc_bycube.rpt";
                                    ReportDocument rd = new ReportDocument();
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmpid;
                                    objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                                    objBillingInquiry = ServiceObject.GetBillingBillInoutCubeRpt(objBillingInquiry);
                                    var rptSource = objBillingInquiry.ListBillingInoutCubeRpt.ToList();
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
                            //CR-3PL_MVC_BL_2018-03-10-001
                            if (l_str_rpt_bill_inout_type.Trim() == "Container")

                            {
                                strReportName = "rpt_inout_bill_by_Container.rpt";
                                ReportDocument rd = new ReportDocument();
                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                objBillingInquiry.cmp_id = p_str_cmpid.Trim();
                                objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                                objBillingInquiry = ServiceObject.GetBillingBillDocContainerInoutRpt(objBillingInquiry);
                                var rptSource = objBillingInquiry.ListGenBillingInoutByContainerRpt.ToList();
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objBillingInquiry.ListGenBillingInoutByContainerRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);

                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");


                            }

                        }

                    }
                    if (p_str_status == "POST")
                    {
                        strReportName = "rpt_va_bill_inv.rpt";
                        BillingInquiry objBillingInquiry = new BillingInquiry();
                        BillingInquiryService ServiceObject = new BillingInquiryService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        objBillingInquiry.cmp_id = p_str_cmpid;
                        objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                        objBillingInquiry = ServiceObject.GetBillingInvoiceRpt(objBillingInquiry);
                        var rptSource = objBillingInquiry.ListBillingInvoiceRpt.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objBillingInquiry.ListBillingInvoiceRpt.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
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

        //CR - 3PL_MVC_IB_2018_0219_008
        public ActionResult EmailShowReport(string p_str_radio, string p_str_cmp_id, string p_str_Bill_doc_id, string p_str_Bill_type, string p_str_doc_dt_Fr, string p_str_doc_dt_To, string SelectdID, string type)

        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");

            string l_str_rpt_selection = string.Empty;
            //l_str_rpt_selection = TempData["ReportSelection"].ToString().Trim();

            l_str_rpt_selection = p_str_radio;

            string l_str_status = string.Empty;
            string l_str_rpt_bill_type = string.Empty;
            string l_str_rpt_bill_inout_type = string.Empty;
            string l_str_rpt_instrg_req = string.Empty;
            string l_str_rpt_bill_doc_type = string.Empty;
            string l_str_rpt_bill_status = string.Empty;
            string strDateFormat = string.Empty;
            string strFileName = string.Empty;
            decimal l_dec_tot_amnt = 0;
            decimal l_dec_list_price = 0;
            int l_int_tot_qty = 0;
            string reportFileName = string.Empty;       //CR2018-03-15-001 Added By Soniya
            try
            {
                if (isValid)
                {


                    if (l_str_rpt_selection == "BillDocument")

                    {
                        BillingInquiry objBillingInquiry = new BillingInquiry();
                        BillingInquiryService ServiceObject = new BillingInquiryService();
                        objBillingInquiry.cmp_id = p_str_cmp_id;
                        objBillingInquiry.Bill_doc_id = SelectdID;
                        objBillingInquiry = ServiceObject.GetBillingInvStaus(objBillingInquiry);
                        l_str_rpt_bill_status = objBillingInquiry.ListBillingInvStatus[0].InvoiceStatus;

                        objBillingInquiry.cmp_id = p_str_cmp_id;
                        objBillingInquiry.Bill_doc_id = SelectdID;
                        objBillingInquiry = ServiceObject.GetBillingBillingType(objBillingInquiry);
                        l_str_rpt_bill_type = objBillingInquiry.ListBillingType[0].bill_type;
                        objBillingInquiry = ServiceObject.GetBillingInoutType(objBillingInquiry);
                        l_str_rpt_bill_inout_type = objBillingInquiry.ListBillingInoutType[0].bill_inout_type;
                        l_str_rpt_instrg_req = objBillingInquiry.ListBillingType[0].init_strg_rt_req;
                        objBillingInquiry = ServiceObject.GetBillingBillDocIdType(objBillingInquiry);
                        objBillingInquiry.cmp_id = p_str_cmp_id;
                        objBillingInquiry.Bill_doc_id = SelectdID;
                        l_str_rpt_bill_doc_type = objBillingInquiry.ListBillingDocIdType[0].bill_type;

                        if (l_str_rpt_bill_doc_type == "STRG")
                        {
                            if (l_str_rpt_bill_type.Trim() == "Carton")

                            {
                                if (l_str_rpt_instrg_req.Trim() == "Y")
                                {
                                    strReportName = "rpt_st_bill_doc.rpt";
                                    ReportDocument rd = new ReportDocument();
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry = ServiceObject.GetBillingBillDocSTRGRpt(objBillingInquiry);


                                    if (type == "PDF")
                                    {
                                        var rptSource = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.ToList();
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                        AlocCount = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);
                                        //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//IV_DOC_INQ_" + strDateFormat + ".pdf";
                                        // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                        // rd.ExportToDisk(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        reportFileName = "BL_INQ_" + strDateFormat + ".pdf";//CR2018-03-15-001 Added By Soniya
                                        Session["RptFileName"] = strFileName;
                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                    }
                                    else
                                    if (type == "Excel")
                                    {
                                        objBillingInquiry = ServiceObject.GetStrgBillExcel(objBillingInquiry);
                                        List<BILLING_STRG_BILLDOC_CRTN_EXCEL> li = new List<BILLING_STRG_BILLDOC_CRTN_EXCEL>();
                                        for (int i = 0; i < objBillingInquiry.ListBillingStrgExcel.Count; i++)
                                        {

                                            BILLING_STRG_BILLDOC_CRTN_EXCEL objOBInquiryExcel = new BILLING_STRG_BILLDOC_CRTN_EXCEL();
                                            objOBInquiryExcel.LineNo = objBillingInquiry.ListBillingStrgExcel[i].LineNo;
                                            objOBInquiryExcel.Desc = objBillingInquiry.ListBillingStrgExcel[i].Desc;
                                            objOBInquiryExcel.Style = objBillingInquiry.ListBillingStrgExcel[i].Style;
                                            objOBInquiryExcel.Color = objBillingInquiry.ListBillingStrgExcel[i].Color;
                                            objOBInquiryExcel.Size = objBillingInquiry.ListBillingStrgExcel[i].Size;
                                            objOBInquiryExcel.Ctns = objBillingInquiry.ListBillingStrgExcel[i].Ctns;
                                            objOBInquiryExcel.Rate = objBillingInquiry.ListBillingStrgExcel[i].Rate;
                                            objOBInquiryExcel.Amount = objBillingInquiry.ListBillingStrgExcel[i].Amount;

                                            li.Add(objOBInquiryExcel);
                                        }

                                        GridView gv = new GridView();
                                        gv.DataSource = li;
                                        gv.DataBind();
                                        Session["BILL_DOC"] = gv;
                                        return new DownloadFileActionResult((GridView)Session["BILL_DOC"], "BILL_DOC" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



                                    }

                                }
                                else
                                {
                                    strReportName = "rpt_st_bill_doc.rpt";
                                    ReportDocument rd = new ReportDocument();
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry = ServiceObject.GetBillingBillDocSTRGRpt(objBillingInquiry);

                                    if (type == "PDF")
                                    {
                                        var rptSource = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.ToList();
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                        AlocCount = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);
                                        //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//IV_DOC_INQ_" + strDateFormat + ".pdf";
                                        // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                        // rd.ExportToDisk(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        reportFileName = "BL_INQ_" + strDateFormat + ".pdf";//CR2018-03-15-001 Added By Soniya
                                        Session["RptFileName"] = strFileName;
                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                    }
                                    else
                                    if (type == "Excel")
                                    {
                                        objBillingInquiry = ServiceObject.GetStrgBillExcel(objBillingInquiry);
                                        List<BILLING_STRG_BILLDOC_CRTN_EXCEL> li = new List<BILLING_STRG_BILLDOC_CRTN_EXCEL>();
                                        for (int i = 0; i < objBillingInquiry.ListBillingStrgExcel.Count; i++)
                                        {

                                            BILLING_STRG_BILLDOC_CRTN_EXCEL objOBInquiryExcel = new BILLING_STRG_BILLDOC_CRTN_EXCEL();
                                            objOBInquiryExcel.LineNo = objBillingInquiry.ListBillingStrgExcel[i].LineNo;
                                            objOBInquiryExcel.Desc = objBillingInquiry.ListBillingStrgExcel[i].Desc;
                                            objOBInquiryExcel.Style = objBillingInquiry.ListBillingStrgExcel[i].Style;
                                            objOBInquiryExcel.Color = objBillingInquiry.ListBillingStrgExcel[i].Color;
                                            objOBInquiryExcel.Size = objBillingInquiry.ListBillingStrgExcel[i].Size;
                                            objOBInquiryExcel.Ctns = objBillingInquiry.ListBillingStrgExcel[i].Ctns;
                                            objOBInquiryExcel.Rate = objBillingInquiry.ListBillingStrgExcel[i].Rate;
                                            objOBInquiryExcel.Amount = objBillingInquiry.ListBillingStrgExcel[i].Amount;

                                            li.Add(objOBInquiryExcel);
                                        }

                                        GridView gv = new GridView();
                                        gv.DataSource = li;
                                        gv.DataBind();
                                        Session["BILL_DOC"] = gv;
                                        return new DownloadFileActionResult((GridView)Session["BILL_DOC"], "BILL_DOC" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



                                    }
                                }

                            }
                            if (l_str_rpt_bill_type.Trim() == "Cube")

                            {
                                if (l_str_rpt_instrg_req.Trim() == "Y")
                                {
                                    strReportName = "rpt_st_bill_doc_bycube.rpt";
                                    ReportDocument rd = new ReportDocument();
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry = ServiceObject.GetBillingBillDocCubeSTRGRpt(objBillingInquiry);

                                    if (type == "PDF")
                                    {
                                        var rptSource = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.ToList();
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                        AlocCount = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);
                                        //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//IV_DOC_INQ_" + strDateFormat + ".pdf";
                                        // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                        reportFileName = "BL_INQ_" + strDateFormat + ".pdf";//CR2018-03-15-001 Added By Soniya
                                        Session["RptFileName"] = strFileName;
                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                    }
                                    else
                                    if (type == "Excel")
                                    {
                                        objBillingInquiry = ServiceObject.GetStrgBillCubeExcel(objBillingInquiry);
                                        List<BILLING_STRG_BILLDOC_CUBE_EXCEL> li = new List<BILLING_STRG_BILLDOC_CUBE_EXCEL>();
                                        for (int i = 0; i < objBillingInquiry.ListBillingStrgExcel.Count; i++)
                                        {

                                            BILLING_STRG_BILLDOC_CUBE_EXCEL objOBInquiryExcel = new BILLING_STRG_BILLDOC_CUBE_EXCEL();
                                            objOBInquiryExcel.Line = objBillingInquiry.ListBillingStrgExcel[i].Line;
                                            objOBInquiryExcel.Description = objBillingInquiry.ListBillingStrgExcel[i].Description;
                                            objOBInquiryExcel.Style = objBillingInquiry.ListBillingStrgExcel[i].Style;
                                            objOBInquiryExcel.Color = objBillingInquiry.ListBillingStrgExcel[i].Color;
                                            objOBInquiryExcel.Size = objBillingInquiry.ListBillingStrgExcel[i].Size;
                                            objOBInquiryExcel.Ctn = objBillingInquiry.ListBillingStrgExcel[i].Ctn;
                                            objOBInquiryExcel.TotCube = objBillingInquiry.ListBillingStrgExcel[i].TotCube;
                                            objOBInquiryExcel.Rate = objBillingInquiry.ListBillingStrgExcel[i].Rate;
                                            objOBInquiryExcel.Amount = objBillingInquiry.ListBillingStrgExcel[i].Amount;

                                            li.Add(objOBInquiryExcel);
                                        }

                                        GridView gv = new GridView();
                                        gv.DataSource = li;
                                        gv.DataBind();
                                        Session["BILL_DOC"] = gv;
                                        return new DownloadFileActionResult((GridView)Session["BILL_DOC"], "BILL_DOC" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



                                    }
                                }
                                else
                                {
                                    strReportName = "rpt_st_bill_doc_bycube.rpt";
                                    ReportDocument rd = new ReportDocument();
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry = ServiceObject.GetBillingBillDocCubeSTRGRpt(objBillingInquiry);


                                    if (type == "PDF")
                                    {
                                        var rptSource = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.ToList();
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                        AlocCount = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);
                                        //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//IV_DOC_INQ_" + strDateFormat + ".pdf";
                                        // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                        reportFileName = "BL_INQ_" + strDateFormat + ".pdf";//CR2018-03-15-001 Added By Soniya
                                        Session["RptFileName"] = strFileName;
                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                    }
                                    else
                                    if (type == "Excel")
                                    {
                                        objBillingInquiry = ServiceObject.GetStrgBillCubeExcel(objBillingInquiry);
                                        List<BILLING_STRG_BILLDOC_CUBE_EXCEL> li = new List<BILLING_STRG_BILLDOC_CUBE_EXCEL>();
                                        for (int i = 0; i < objBillingInquiry.ListBillingStrgExcel.Count; i++)
                                        {

                                            BILLING_STRG_BILLDOC_CUBE_EXCEL objOBInquiryExcel = new BILLING_STRG_BILLDOC_CUBE_EXCEL();
                                            objOBInquiryExcel.Line = objBillingInquiry.ListBillingStrgExcel[i].Line;
                                            objOBInquiryExcel.Description = objBillingInquiry.ListBillingStrgExcel[i].Description;
                                            objOBInquiryExcel.Style = objBillingInquiry.ListBillingStrgExcel[i].Style;
                                            objOBInquiryExcel.Color = objBillingInquiry.ListBillingStrgExcel[i].Color;
                                            objOBInquiryExcel.Size = objBillingInquiry.ListBillingStrgExcel[i].Size;
                                            objOBInquiryExcel.Ctn = objBillingInquiry.ListBillingStrgExcel[i].Ctn;
                                            objOBInquiryExcel.TotCube = objBillingInquiry.ListBillingStrgExcel[i].TotCube;
                                            objOBInquiryExcel.Rate = objBillingInquiry.ListBillingStrgExcel[i].Rate;
                                            objOBInquiryExcel.Amount = objBillingInquiry.ListBillingStrgExcel[i].Amount;

                                            li.Add(objOBInquiryExcel);
                                        }

                                        GridView gv = new GridView();
                                        gv.DataSource = li;
                                        gv.DataBind();
                                        Session["BILL_DOC"] = gv;
                                        return new DownloadFileActionResult((GridView)Session["BILL_DOC"], "BILL_DOC" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



                                    }

                                }

                            }
                            if (l_str_rpt_bill_type.Trim() == "Pallet")
                            {

                                //strReportName = "rpt_st_bill_doc_bycube.rpt";
                                //ReportDocument rd = new ReportDocument();
                                //string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                //objBillingInquiry.cmp_id = p_str_cmp_id;
                                //objBillingInquiry.Bill_doc_id = SelectdID;
                                //objBillingInquiry = ServiceObject.GenerateSTRGBillByPallet(objBillingInquiry);



                                strReportName = "rpt_strg_bill_by_pallet.rpt";
                                ReportDocument rd = new ReportDocument();
                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                                objBillingInquiry.Bill_doc_id = SelectdID.Trim();
                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                objBillingInquiry = ServiceObject.GetBillingBillDocPalletSTRGRpt(objBillingInquiry);


                                if (type == "PDF")
                                {
                                    var rptSource = objBillingInquiry.ListGenBillingStrgByPalletRpt.ToList();
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
                                    // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//IV_DOC_INQ_" + strDateFormat + ".pdf";
                                    // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                    reportFileName = "BL_INQ_" + strDateFormat + ".pdf";//CR2018-03-15-001 Added By Soniya
                                    Session["RptFileName"] = strFileName;
                                }
                                else if (type == "Word")
                                {
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                                else
                                if (type == "Excel")
                                {
                                    objBillingInquiry = ServiceObject.GetBillingBillDocPalletSTRGRpt(objBillingInquiry);
                                    List<BILLING_STRG_BILLDOC_PALLET_EXCEL> li = new List<BILLING_STRG_BILLDOC_PALLET_EXCEL>();
                                    for (int i = 0; i < objBillingInquiry.ListGenBillingStrgByPalletRpt.Count; i++)
                                    {

                                        BILLING_STRG_BILLDOC_PALLET_EXCEL objOBInquiryExcel = new BILLING_STRG_BILLDOC_PALLET_EXCEL();
                                        objOBInquiryExcel.Line = objBillingInquiry.ListGenBillingStrgByPalletRpt[i].dtl_line;
                                        objOBInquiryExcel.IBDocId = objBillingInquiry.ListGenBillingStrgByPalletRpt[i].doc_id;
                                        objOBInquiryExcel.DocDt = objBillingInquiry.ListGenBillingStrgByPalletRpt[i].doc_dt;
                                        objOBInquiryExcel.BillDocId = objBillingInquiry.ListGenBillingStrgByPalletRpt[i].bill_doc_id;
                                        objOBInquiryExcel.PoNum = objBillingInquiry.ListGenBillingStrgByPalletRpt[i].po_num;
                                        objOBInquiryExcel.Qty = objBillingInquiry.ListGenBillingStrgByPalletRpt[i].no_of_pallets;
                                        objOBInquiryExcel.Rate = objBillingInquiry.ListGenBillingStrgByPalletRpt[i].rate_price;
                                        objOBInquiryExcel.RateType = objBillingInquiry.ListGenBillingStrgByPalletRpt[i].rate_id;


                                        l_dec_list_price = objBillingInquiry.ListGenBillingStrgByPalletRpt[i].rate_price;
                                        l_int_tot_qty = objBillingInquiry.ListGenBillingStrgByPalletRpt[i].no_of_pallets;
                                        l_dec_tot_amnt = l_dec_list_price * l_int_tot_qty;
                                        objOBInquiryExcel.Amount = l_dec_tot_amnt;
                                        li.Add(objOBInquiryExcel);
                                    }

                                    GridView gv = new GridView();
                                    gv.DataSource = li;
                                    gv.DataBind();
                                    Session["BILL_DOC"] = gv;
                                    return new DownloadFileActionResult((GridView)Session["BILL_DOC"], "BILL_DOC" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



                                }
                            }

                        }
                        if (l_str_rpt_bill_doc_type == "NORM")
                        {
                            strReportName = "rpt_va_bill_doc.rpt";
                            ReportDocument rd = new ReportDocument();
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id;
                            objBillingInquiry.Bill_doc_id = SelectdID;
                            objBillingInquiry = ServiceObject.GetBillingBillDocVASRpt(objBillingInquiry);


                            if (type == "PDF")
                            {
                                var rptSource = objBillingInquiry.ListBillingDocVASRpt.ToList();
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objBillingInquiry.ListBillingDocVASRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//IV_DOC_INQ_" + strDateFormat + ".pdf";
                                // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                reportFileName = "BL_INQ_" + strDateFormat + ".pdf";//CR2018-03-15-001 Added By Soniya
                                Session["RptFileName"] = strFileName;
                            }
                            else if (type == "Word")
                            {
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            }
                            else
                            if (type == "Excel")
                            {
                                objBillingInquiry = ServiceObject.GetNormBillExcel(objBillingInquiry);
                                List<BILLING_NORM_BILLDOC_EXCEL> li = new List<BILLING_NORM_BILLDOC_EXCEL>();
                                for (int i = 0; i < objBillingInquiry.ListBillingStrgExcel.Count; i++)
                                {

                                    BILLING_NORM_BILLDOC_EXCEL objOBInquiryExcel = new BILLING_NORM_BILLDOC_EXCEL();
                                    objOBInquiryExcel.Line = objBillingInquiry.ListBillingStrgExcel[i].Line;
                                    objOBInquiryExcel.VASId = objBillingInquiry.ListBillingStrgExcel[i].VASId;
                                    objOBInquiryExcel.ShipDate = objBillingInquiry.ListBillingStrgExcel[i].ShipDate;
                                    objOBInquiryExcel.CustOrder = objBillingInquiry.ListBillingStrgExcel[i].CustOrder;
                                    objOBInquiryExcel.ShipDocNotes = objBillingInquiry.ListBillingStrgExcel[i].ShipDocNotes;
                                    objOBInquiryExcel.Notes = objBillingInquiry.ListBillingStrgExcel[i].Notes;
                                    objOBInquiryExcel.VASDesc = objBillingInquiry.ListBillingStrgExcel[i].VASDesc;
                                    objOBInquiryExcel.Ctns = objBillingInquiry.ListBillingStrgExcel[i].Ctns;
                                    objOBInquiryExcel.Rate = objBillingInquiry.ListBillingStrgExcel[i].Rate;
                                    objOBInquiryExcel.Amount = objBillingInquiry.ListBillingStrgExcel[i].Amount;
                                    li.Add(objOBInquiryExcel);
                                }

                                GridView gv = new GridView();
                                gv.DataSource = li;
                                gv.DataBind();
                                Session["BILL_DOC"] = gv;
                                return new DownloadFileActionResult((GridView)Session["BILL_DOC"], "BILL_DOC" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



                            }
                        }
                        if (l_str_rpt_bill_doc_type == "INOUT")
                        {

                            if (l_str_rpt_bill_inout_type.Trim() == "Carton")

                            {
                                if (l_str_rpt_instrg_req.Trim() == "Y")
                                {
                                    strReportName = "rpt_inout_bill_doc_with_initstrg.rpt";
                                    ReportDocument rd = new ReportDocument();
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;

                                    objBillingInquiry = ServiceObject.GetBillingBillInoutCartonInstrgRpt(objBillingInquiry);

                                    if (type == "PDF")
                                    {
                                        var rptSource = objBillingInquiry.ListBillingInoutCartonInstrgRpt.ToList();
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                        AlocCount = objBillingInquiry.ListBillingInoutCartonInstrgRpt.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);
                                        //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//IV_DOC_INQ_" + strDateFormat + ".pdf";
                                        // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                        reportFileName = "BL_INQ_" + strDateFormat + ".pdf";//CR2018-03-15-001 Added By Soniya
                                        Session["RptFileName"] = strFileName;
                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                    }
                                    else
                                    if (type == "Excel")
                                    {
                                        objBillingInquiry = ServiceObject.GetInOutBillCarton(objBillingInquiry);

                                        List<BILLING_INOUT_BILLDOC_CTN_EXCEL> li = new List<BILLING_INOUT_BILLDOC_CTN_EXCEL>();
                                        for (int i = 0; i < objBillingInquiry.ListBillingStrgExcel.Count; i++)
                                        {

                                            BILLING_INOUT_BILLDOC_CTN_EXCEL objOBInquiryExcel = new BILLING_INOUT_BILLDOC_CTN_EXCEL();
                                            objOBInquiryExcel.Line = objBillingInquiry.ListBillingStrgExcel[i].Line;
                                            objOBInquiryExcel.ServiceId = objBillingInquiry.ListBillingStrgExcel[i].ServiceId;
                                            objOBInquiryExcel.ContId = objBillingInquiry.ListBillingStrgExcel[i].ContId;
                                            objOBInquiryExcel.LotId = objBillingInquiry.ListBillingStrgExcel[i].LotId;
                                            objOBInquiryExcel.Style = objBillingInquiry.ListBillingStrgExcel[i].Style;
                                            objOBInquiryExcel.Color = objBillingInquiry.ListBillingStrgExcel[i].Color;
                                            objOBInquiryExcel.Size = objBillingInquiry.ListBillingStrgExcel[i].Size;
                                            objOBInquiryExcel.Rate = objBillingInquiry.ListBillingStrgExcel[i].Rate;
                                            objOBInquiryExcel.Amount = objBillingInquiry.ListBillingStrgExcel[i].Amount;


                                            li.Add(objOBInquiryExcel);
                                        }

                                        GridView gv = new GridView();
                                        gv.DataSource = li;
                                        gv.DataBind();
                                        Session["BILL_DOC"] = gv;
                                        return new DownloadFileActionResult((GridView)Session["BILL_DOC"], "BILL_DOC_" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



                                    }
                                }
                                else
                                {
                                    strReportName = "rpt_inout_bill_doc.rpt";
                                    ReportDocument rd = new ReportDocument();
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry = ServiceObject.GetBillingBillInoutCartonRpt(objBillingInquiry);

                                    if (type == "PDF")
                                    {
                                        var rptSource = objBillingInquiry.ListBillingInoutCartonRpt.ToList();
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                        AlocCount = objBillingInquiry.ListBillingInoutCartonRpt.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);
                                        //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//IV_DOC_INQ_" + strDateFormat + ".pdf";
                                        // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                        reportFileName = "BL_INQ_" + strDateFormat + ".pdf";//CR2018-03-15-001 Added By Soniya
                                        Session["RptFileName"] = strFileName;
                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                    }
                                    else
                                    if (type == "Excel")
                                    {

                                        objBillingInquiry = ServiceObject.GetInOutBillCarton(objBillingInquiry);

                                        List<BILLING_INOUT_BILLDOC_CTN_EXCEL> li = new List<BILLING_INOUT_BILLDOC_CTN_EXCEL>();
                                        for (int i = 0; i < objBillingInquiry.ListBillingStrgExcel.Count; i++)
                                        {

                                            BILLING_INOUT_BILLDOC_CTN_EXCEL objOBInquiryExcel = new BILLING_INOUT_BILLDOC_CTN_EXCEL();
                                            objOBInquiryExcel.Line = objBillingInquiry.ListBillingStrgExcel[i].Line;
                                            objOBInquiryExcel.ServiceId = objBillingInquiry.ListBillingStrgExcel[i].ServiceId;
                                            objOBInquiryExcel.ContId = objBillingInquiry.ListBillingStrgExcel[i].ContId;
                                            objOBInquiryExcel.LotId = objBillingInquiry.ListBillingStrgExcel[i].LotId;
                                            objOBInquiryExcel.Style = objBillingInquiry.ListBillingStrgExcel[i].Style;
                                            objOBInquiryExcel.Color = objBillingInquiry.ListBillingStrgExcel[i].Color;
                                            objOBInquiryExcel.Size = objBillingInquiry.ListBillingStrgExcel[i].Size;
                                            objOBInquiryExcel.Rate = objBillingInquiry.ListBillingStrgExcel[i].Rate;
                                            objOBInquiryExcel.Amount = objBillingInquiry.ListBillingStrgExcel[i].Amount;


                                            li.Add(objOBInquiryExcel);
                                        }

                                        GridView gv = new GridView();
                                        gv.DataSource = li;
                                        gv.DataBind();
                                        Session["BILL_DOC"] = gv;
                                        return new DownloadFileActionResult((GridView)Session["BILL_DOC"], "BILL_DOC_" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");




                                    }
                                }

                            }
                            if (l_str_rpt_bill_inout_type.Trim() == "Cube")

                            {
                                if (l_str_rpt_instrg_req.Trim() == "Y")
                                {
                                    strReportName = "rpt_inout_bill_doc_bycube_with_initstrg.rpt";
                                    ReportDocument rd = new ReportDocument();
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry = ServiceObject.GetBillingBillInoutCubeInstrgRpt(objBillingInquiry);

                                    if (type == "PDF")
                                    {
                                        var rptSource = objBillingInquiry.ListBillingInoutCubeInstrgRpt.ToList();
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                        AlocCount = objBillingInquiry.ListBillingInoutCubeInstrgRpt.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);
                                        //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//IV_DOC_INQ_" + strDateFormat + ".pdf";
                                        // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                        reportFileName = "BL_INQ_" + strDateFormat + ".pdf";//CR2018-03-15-001 Added By Soniya
                                        Session["RptFileName"] = strFileName;
                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                    }
                                    else
                                    if (type == "Excel")
                                    {
                                        objBillingInquiry = ServiceObject.GetInOutBillCube(objBillingInquiry);
                                        List<BILLING_INOUT_BILLDOC_CUBE_EXCEL> li = new List<BILLING_INOUT_BILLDOC_CUBE_EXCEL>();
                                        for (int i = 0; i < objBillingInquiry.ListBillingStrgExcel.Count; i++)
                                        {

                                            BILLING_INOUT_BILLDOC_CUBE_EXCEL objOBInquiryExcel = new BILLING_INOUT_BILLDOC_CUBE_EXCEL();
                                            objOBInquiryExcel.Line = objBillingInquiry.ListBillingStrgExcel[i].Line;
                                            objOBInquiryExcel.Description = objBillingInquiry.ListBillingStrgExcel[i].Description;
                                            objOBInquiryExcel.ContID = objBillingInquiry.ListBillingStrgExcel[i].ContID;
                                            objOBInquiryExcel.LotID = objBillingInquiry.ListBillingStrgExcel[i].LotID;
                                            objOBInquiryExcel.Style = objBillingInquiry.ListBillingStrgExcel[i].Style;
                                            objOBInquiryExcel.Color = objBillingInquiry.ListBillingStrgExcel[i].Color;
                                            objOBInquiryExcel.Size = objBillingInquiry.ListBillingStrgExcel[i].Size;
                                            objOBInquiryExcel.NoOfCtn = objBillingInquiry.ListBillingStrgExcel[i].NoOfCtn;
                                            objOBInquiryExcel.ItemPrice = objBillingInquiry.ListBillingStrgExcel[i].ItemPrice;
                                            objOBInquiryExcel.QTYPrice = objBillingInquiry.ListBillingStrgExcel[i].QTYPrice;


                                            li.Add(objOBInquiryExcel);
                                        }

                                        GridView gv = new GridView();
                                        gv.DataSource = li;
                                        gv.DataBind();
                                        Session["BILL_DOC"] = gv;
                                        return new DownloadFileActionResult((GridView)Session["BILL_DOC"], "BILL_DOC_" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



                                    }
                                }
                                else
                                {
                                    strReportName = "rpt_inout_bill_doc_bycube.rpt";
                                    ReportDocument rd = new ReportDocument();
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry = ServiceObject.GetBillingBillInoutCubeRpt(objBillingInquiry);


                                    if (type == "PDF")
                                    {
                                        var rptSource = objBillingInquiry.ListBillingInoutCubeRpt.ToList();
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                        AlocCount = objBillingInquiry.ListBillingInoutCubeRpt.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);
                                        //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//IV_DOC_INQ_" + strDateFormat + ".pdf";
                                        // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                        reportFileName = "BL_INQ_" + strDateFormat + ".pdf";//CR2018-03-15-001 Added By Soniya
                                        Session["RptFileName"] = strFileName;
                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                    }
                                    else
                                    if (type == "Excel")
                                    {

                                        objBillingInquiry = ServiceObject.GetInOutBillCube(objBillingInquiry);
                                        List<BILLING_INOUT_BILLDOC_CUBE_EXCEL> li = new List<BILLING_INOUT_BILLDOC_CUBE_EXCEL>();
                                        for (int i = 0; i < objBillingInquiry.ListBillingStrgExcel.Count; i++)
                                        {

                                            BILLING_INOUT_BILLDOC_CUBE_EXCEL objOBInquiryExcel = new BILLING_INOUT_BILLDOC_CUBE_EXCEL();
                                            objOBInquiryExcel.Line = objBillingInquiry.ListBillingStrgExcel[i].Line;
                                            objOBInquiryExcel.Description = objBillingInquiry.ListBillingStrgExcel[i].Description;
                                            objOBInquiryExcel.ContID = objBillingInquiry.ListBillingStrgExcel[i].ContID;
                                            objOBInquiryExcel.LotID = objBillingInquiry.ListBillingStrgExcel[i].LotID;
                                            objOBInquiryExcel.Style = objBillingInquiry.ListBillingStrgExcel[i].Style;
                                            objOBInquiryExcel.Color = objBillingInquiry.ListBillingStrgExcel[i].Color;
                                            objOBInquiryExcel.Size = objBillingInquiry.ListBillingStrgExcel[i].Size;
                                            objOBInquiryExcel.NoOfCtn = objBillingInquiry.ListBillingStrgExcel[i].NoOfCtn;
                                            objOBInquiryExcel.ItemPrice = objBillingInquiry.ListBillingStrgExcel[i].ItemPrice;
                                            objOBInquiryExcel.QTYPrice = objBillingInquiry.ListBillingStrgExcel[i].QTYPrice;


                                            li.Add(objOBInquiryExcel);
                                        }

                                        GridView gv = new GridView();
                                        gv.DataSource = li;
                                        gv.DataBind();
                                        Session["BILL_DOC"] = gv;
                                        return new DownloadFileActionResult((GridView)Session["BILL_DOC"], "BILL_DOC_" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
                                    }
                                }

                            }
                            if (l_str_rpt_bill_inout_type.Trim() == "Container")

                            {

                                strReportName = "rpt_inout_bill_by_Container.rpt";
                                ReportDocument rd = new ReportDocument();
                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                objBillingInquiry.cmp_id = p_str_cmp_id;
                                objBillingInquiry.Bill_doc_id = SelectdID;
                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                objBillingInquiry = ServiceObject.GetBillingBillDocContainerInoutRpt(objBillingInquiry);

                                if (type == "PDF")
                                {
                                    var rptSource = objBillingInquiry.ListGenBillingInoutByContainerRpt.ToList();
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objBillingInquiry.ListGenBillingInoutByContainerRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//IV_DOC_INQ_" + strDateFormat + ".pdf";
                                    // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                    reportFileName = "BL_INQ_" + strDateFormat + ".pdf";//CR2018-03-15-001 Added By Soniya
                                    Session["RptFileName"] = strFileName;
                                }
                                else if (type == "Word")
                                {
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                                else
                                if (type == "Excel")
                                {
                                    objBillingInquiry = ServiceObject.GetBillingBillDocContainerInoutRpt(objBillingInquiry);
                                    List<BILLING_INOUT_BILLDOC_CONTAINER_EXCEL> li = new List<BILLING_INOUT_BILLDOC_CONTAINER_EXCEL>();
                                    for (int i = 0; i < objBillingInquiry.ListGenBillingInoutByContainerRpt.Count; i++)
                                    {

                                        BILLING_INOUT_BILLDOC_CONTAINER_EXCEL objOBInquiryExcel = new BILLING_INOUT_BILLDOC_CONTAINER_EXCEL();
                                        objOBInquiryExcel.Line = objBillingInquiry.ListGenBillingInoutByContainerRpt[i].dtl_line;
                                        objOBInquiryExcel.IBDocId = objBillingInquiry.ListGenBillingInoutByContainerRpt[i].doc_id;
                                        objOBInquiryExcel.DocDt = objBillingInquiry.ListGenBillingInoutByContainerRpt[i].doc_dt;
                                        objOBInquiryExcel.BillDocId = objBillingInquiry.ListGenBillingInoutByContainerRpt[i].bill_doc_id;
                                        objOBInquiryExcel.CntrId = objBillingInquiry.ListGenBillingInoutByContainerRpt[i].cntr_id;
                                        objOBInquiryExcel.Rate = objBillingInquiry.ListGenBillingInoutByContainerRpt[i].tot_wgt;
                                        objOBInquiryExcel.TotWgt = objBillingInquiry.ListGenBillingInoutByContainerRpt[i].tot_cube;
                                        objOBInquiryExcel.TotCube = objBillingInquiry.ListGenBillingInoutByContainerRpt[i].tot_cube;
                                        objOBInquiryExcel.Note = objBillingInquiry.ListGenBillingInoutByContainerRpt[i].note;
                                        objOBInquiryExcel.Amount = objBillingInquiry.ListGenBillingInoutByContainerRpt[i].rate_price;
                                        //objOBInquiryExcel.QTYPrice = objBillingInquiry.ListBillingStrgExcel[i].bill_pd_to;

                                        li.Add(objOBInquiryExcel);
                                    }

                                    GridView gv = new GridView();
                                    gv.DataSource = li;
                                    gv.DataBind();
                                    Session["BILL_DOC"] = gv;
                                    return new DownloadFileActionResult((GridView)Session["BILL_DOC"], "BILL_DOC_" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



                                }

                            }
                        }

                    }


                    if (l_str_rpt_selection == "BillInvoice")
                    {
                        BillingInquiry objBillingInquiry = new BillingInquiry();
                        BillingInquiryService ServiceObject = new BillingInquiryService();
                        objBillingInquiry.cmp_id = p_str_cmp_id;
                        objBillingInquiry.Bill_doc_id = SelectdID;
                        objBillingInquiry = ServiceObject.GetBillingInvStaus(objBillingInquiry);
                        l_str_rpt_bill_status = objBillingInquiry.ListBillingInvStatus[0].InvoiceStatus;
                        if (l_str_rpt_bill_status == "P")
                        {
                            strReportName = "rpt_va_bill_inv.rpt";
                            //BillingInquiry objBillingInquiry = new BillingInquiry();
                            //BillingInquiryService ServiceObject = new BillingInquiryService();
                            ReportDocument rd = new ReportDocument();
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id;
                            objBillingInquiry.Bill_doc_id = SelectdID;
                            objBillingInquiry = ServiceObject.GetBillingInvoiceRpt(objBillingInquiry);

                            if (type == "PDF")
                            {
                                var rptSource = objBillingInquiry.ListBillingInvoiceRpt.ToList();
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objBillingInquiry.ListBillingInvoiceRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//IV_DOC_INQ_" + strDateFormat + ".pdf";
                                // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                reportFileName = "BL_INQ_" + strDateFormat + ".pdf";//CR2018-03-15-001 Added By Soniya
                                Session["RptFileName"] = strFileName;
                            }
                            else if (type == "Word")
                            {
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            }
                            else
                            if (type == "Excel")
                            {

                                List<BILLING_INV_EXCEL> li = new List<BILLING_INV_EXCEL>();
                                for (int i = 0; i < objBillingInquiry.ListBillingInvoiceRpt.Count; i++)
                                {

                                    BILLING_INV_EXCEL objOBInquiryExcel = new BILLING_INV_EXCEL();
                                    objOBInquiryExcel.bill_doc_id = objBillingInquiry.ListBillingInvoiceRpt[i].bill_doc_id;
                                    objOBInquiryExcel.so_itm_price = objBillingInquiry.ListBillingInvoiceRpt[i].so_itm_price;
                                    objOBInquiryExcel.ship_ctns = objBillingInquiry.ListBillingInvoiceRpt[i].ship_ctns;
                                    objOBInquiryExcel.dtl_line = objBillingInquiry.ListBillingInvoiceRpt[i].dtl_line;
                                    objOBInquiryExcel.ship_itm_name = objBillingInquiry.ListBillingInvoiceRpt[i].ship_itm_name;
                                    objOBInquiryExcel.bill_pd_fm = objBillingInquiry.ListBillingInvoiceRpt[i].bill_pd_fm;
                                    objOBInquiryExcel.bill_pd_to = objBillingInquiry.ListBillingInvoiceRpt[i].bill_pd_to;

                                    objOBInquiryExcel.ship_doc_notes = objBillingInquiry.ListBillingInvoiceRpt[i].ship_doc_notes;
                                    objOBInquiryExcel.notes = objBillingInquiry.ListBillingInvoiceRpt[i].notes;
                                    objOBInquiryExcel.ship_dt = objBillingInquiry.ListBillingInvoiceRpt[i].ship_dt;
                                    objOBInquiryExcel.cust_ord = objBillingInquiry.ListBillingInvoiceRpt[i].cust_ord;

                                    li.Add(objOBInquiryExcel);
                                }

                                GridView gv = new GridView();
                                gv.DataSource = li;
                                gv.DataBind();
                                Session["BILL_INV"] = gv;
                                return new DownloadFileActionResult((GridView)Session["BILL_INV"], "BILL_INV" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



                            }
                        }
                    }

                    if (l_str_rpt_selection == "GridSummary")
                    {
                        strReportName = "rpt_bill_summary.rpt";
                        BillingInquiry objBillingInquiry = new BillingInquiry();
                        BillingInquiryService ServiceObject = new BillingInquiryService();
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        objBillingInquiry.cmp_id = p_str_cmp_id;
                        objBillingInquiry.Bill_doc_id = p_str_Bill_doc_id;
                        objBillingInquiry.bill_type = p_str_Bill_type;
                        objBillingInquiry.Bill_doc_dt_Fr = p_str_doc_dt_Fr;
                        objBillingInquiry.Bill_doc_dt_To = p_str_doc_dt_To;
                        objBillingInquiry = ServiceObject.GetBillingSummaryRpt(objBillingInquiry);


                        if (type == "PDF")
                        {
                            var rptSource = objBillingInquiry.ListBillingSummaryRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objBillingInquiry.ListBillingSummaryRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//IV_DOC_INQ_" + strDateFormat + ".pdf";
                            // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            reportFileName = "BL_INQ_" + strDateFormat + ".pdf";//CR2018-03-15-001 Added By Soniya
                            Session["RptFileName"] = strFileName;
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else
                        if (type == "Excel")
                        {

                            List<BILLING_Grid_SMRY_EXCEL> li = new List<BILLING_Grid_SMRY_EXCEL>();
                            for (int i = 0; i < objBillingInquiry.ListBillingSummaryRpt.Count; i++)
                            {

                                BILLING_Grid_SMRY_EXCEL objOBInquiryExcel = new BILLING_Grid_SMRY_EXCEL();
                                objOBInquiryExcel.bill_doc_id = objBillingInquiry.ListBillingSummaryRpt[i].bill_doc_id;
                                objOBInquiryExcel.billdocdt = objBillingInquiry.ListBillingSummaryRpt[i].billdocdt;
                                objOBInquiryExcel.RptStatus = objBillingInquiry.ListBillingSummaryRpt[i].RptStatus;
                                objOBInquiryExcel.Bill_Type = objBillingInquiry.ListBillingSummaryRpt[i].Bill_Type;
                                objOBInquiryExcel.prod_cost = objBillingInquiry.ListBillingSummaryRpt[i].prod_cost;
                                objOBInquiryExcel.frgt_cost = objBillingInquiry.ListBillingSummaryRpt[i].frgt_cost;
                                objOBInquiryExcel.bill_amt = objBillingInquiry.ListBillingSummaryRpt[i].bill_amt;


                                li.Add(objOBInquiryExcel);
                            }

                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["BILL_GRID_SMRY"] = gv;
                            return new DownloadFileActionResult((GridView)Session["BILL_GRID_SMRY"], "BILL_GRID_SMRY" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



                        }
                    }
                    else if (l_str_rpt_selection == "ShippingDetail")
                    {

                    }

                }

                else
                {
                    Response.Write("<H2>Report not found</H2>");
                }
                Email objEmail = new Email();
                objEmail.CmpId = p_str_cmp_id;
                objEmail.EmailSubject = "BL_INQ";
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
        //CR180328-001 Added by Nithya
        public ActionResult IncludeDocDetailEntry(string p_str_cmp_id, string p_str_ibdocid, string p_str_line_num, string p_str_style, string p_str_color, string p_str_size, string p_str_desc, string p_str_Itmcode,
           string p_str_ord_qty, int p_str_ppk, int p_str_ctns, string p_str_po_num, decimal p_str_length, decimal p_str_width, decimal p_str_height,
           decimal p_str_weight, decimal p_str_cube, string p_str_loc_id, string p_str_strg_rate, string p_str_inout_rate, string p_str_note)
        {
            try
            {
                string l_str_itmcode;
                int l_str_doc_entry_id;
                int l_int_ordr_qty1;
                int l_int_ctn_qty1;
                int l_int_tmp_ordr_qty;
                l_int_tmp_ordr_qty = Convert.ToInt32(p_str_ord_qty);
                l_int_ordr_qty1 = l_int_tmp_ordr_qty - ((l_int_tmp_ordr_qty) % (p_str_ppk));
                l_int_ctn_qty1 = l_int_tmp_ordr_qty % (p_str_ppk);

                InboundInquiry objInboundInquiry = new InboundInquiry();
                InboundInquiryService objService = new InboundInquiryService();
                objInboundInquiry.cmp_id = p_str_cmp_id;
                objInboundInquiry.ib_doc_id = p_str_ibdocid;
                objInboundInquiry.line_num = Convert.ToInt32(p_str_line_num);
                objInboundInquiry.itm_code = p_str_Itmcode;
                //CR-3PL_MVC_IB_2018_0410_001 Added By Nithya
                if (p_str_Itmcode == "")
                {
                    objInboundInquiry.itm_num = p_str_style;
                    objInboundInquiry.itm_color = p_str_color;
                    objInboundInquiry.itm_size = p_str_size;
                    objInboundInquiry = objService.Validate_Itm(objInboundInquiry);
                    if (objInboundInquiry.LstItmExist.Count > 0)
                    {
                        objInboundInquiry.itm_code = objInboundInquiry.LstItmExist[0].itm_code;
                        p_str_Itmcode = objInboundInquiry.itm_code;
                    }

                }
                //end
                if (p_str_Itmcode == "")
                {
                    objInboundInquiry.itm_num = p_str_style;
                    objInboundInquiry.itm_color = p_str_color;
                    objInboundInquiry.itm_size = p_str_size;
                    objInboundInquiry.itm_name = p_str_desc;
                    objInboundInquiry.ctn_qty = p_str_ctns;
                    objInboundInquiry.weight = p_str_length;
                    objInboundInquiry.length = p_str_width;
                    objInboundInquiry.width = p_str_height;
                    objInboundInquiry.height = p_str_weight;
                    objInboundInquiry.cube = p_str_cube;

                    objInboundInquiry = objService.GetItmId(objInboundInquiry);
                    objInboundInquiry.itmid = objInboundInquiry.itm_code;
                    l_str_itmcode = objInboundInquiry.itm_code;
                    objInboundInquiry.itm_code = l_str_itmcode;
                    objInboundInquiry.flag = "A";
                    objService.Add_Style_To_Itm_dtl(objInboundInquiry);
                    objService.Add_Style_To_Itm_hdr(objInboundInquiry);
                }
                else
                {
                    objInboundInquiry.itm_code = p_str_Itmcode;
                    objInboundInquiry.itm_num = p_str_style;
                    objInboundInquiry.itm_color = p_str_color;
                    objInboundInquiry.itm_size = p_str_size;
                    objInboundInquiry.itm_name = p_str_desc;
                    objInboundInquiry.ctn_qty = p_str_ctns;
                    objInboundInquiry.weight = p_str_length;
                    objInboundInquiry.length = p_str_width;
                    objInboundInquiry.width = p_str_height;
                    objInboundInquiry.height = p_str_weight;
                    objInboundInquiry.cube = p_str_cube;
                    objInboundInquiry.flag = "M";
                    objService.Add_Style_To_Itm_dtl(objInboundInquiry);
                }

                //CR_3PL_MVC_BL_2018_0306_001 Added By Meera

                objInboundInquiry = objService.GetCheckExistGridData(objInboundInquiry);
                objInboundInquiry = objService.GetDocEntryId(objInboundInquiry);
                objInboundInquiry.doc_entry_id = objInboundInquiry.doc_entry_id;
                l_str_doc_entry_id = objInboundInquiry.doc_entry_id;
                objInboundInquiry.doc_entry_id = l_str_doc_entry_id;
                objInboundInquiry.itm_num = p_str_style;
                objInboundInquiry.itm_color = p_str_color;
                objInboundInquiry.itm_size = p_str_size;
                objInboundInquiry.itm_name = p_str_desc;
                //  objInboundInquiry.ord_qty = Convert.ToDecimal(p_str_ord_qty);
                //  objInboundInquiry.docuppk = p_str_ppk;
                //  objInboundInquiry.ctn = p_str_ctns;
                objInboundInquiry.po_num = p_str_po_num;
                objInboundInquiry.doclength = p_str_length;
                objInboundInquiry.docwidth = p_str_width;
                objInboundInquiry.docheight = p_str_height;
                objInboundInquiry.docweight = p_str_weight;
                objInboundInquiry.doccube = p_str_cube;
                objInboundInquiry.loc_id = p_str_loc_id;
                objInboundInquiry.strg_rate = p_str_strg_rate;
                objInboundInquiry.inout_rate = p_str_inout_rate;
                if (p_str_note == "")
                {
                    objInboundInquiry.note = "-";
                }
                else
                {
                    objInboundInquiry.note = p_str_note;
                }
                if (l_int_ctn_qty1 > 0)
                {
                    objInboundInquiry.ord_qty = Convert.ToDecimal(p_str_ord_qty) - l_int_ctn_qty1;
                    objInboundInquiry.docuppk = p_str_ppk;
                    objInboundInquiry.ctn = p_str_ctns - 1;
                    objInboundInquiry.ctn_line = 1;

                    objService.InsertTempDocEntryDetails(objInboundInquiry);
                    objInboundInquiry.ord_qty = l_int_ctn_qty1;
                    objInboundInquiry.docuppk = l_int_ctn_qty1;
                    objInboundInquiry.ctn = 1;
                    objInboundInquiry.ctn_line = 2;

                    objService.InsertTempDocEntryDetails(objInboundInquiry);

                }
                else
                {
                    objInboundInquiry.ctn_line = 1;
                    objInboundInquiry.ord_qty = Convert.ToDecimal(p_str_ord_qty);
                    objInboundInquiry.docuppk = p_str_ppk;
                    objInboundInquiry.ctn = p_str_ctns;
                    objService.InsertTempDocEntryDetails(objInboundInquiry);

                }
                objInboundInquiry.cmp_id = p_str_cmp_id;
                objInboundInquiry.ib_doc_id = p_str_ibdocid;
                objInboundInquiry = objService.GetDocumentEntryTempGridDtl(objInboundInquiry);

                objInboundInquiry = objService.GetDocEntryCount(objInboundInquiry);


                Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
                InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
                return PartialView("_DocEntryGrid", InboundInquiryModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult CheckExistContainerIDS(string p_str_cmp_id, string p_str_cntr_id)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            string l_str_result = string.Empty;
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.cntr_id = p_str_cntr_id;
            objInboundInquiry = objService.GEtStrgBillTYpe(objInboundInquiry);
            objInboundInquiry.bill_inout_type = objInboundInquiry.ListStrgBillType[0].bill_inout_type;
            if (objInboundInquiry.bill_inout_type == "Container")
            {
                objInboundInquiry = objService.Check_Exist_Container_Id(objInboundInquiry);
                if (objInboundInquiry.ListCheckExistContainerId.Count() != 0)
                {
                    if (objInboundInquiry.ListCheckExistContainerId[0].cntr_id.Trim() == p_str_cntr_id)
                    {
                        l_str_result = "True";
                    }
                }
            }
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);

            return Json(l_str_result, JsonRequestBehavior.AllowGet);
        }
        //CR20180710-001 Added By Nithya
        public ActionResult InboundEdit(string p_str_cmp_id, string p_str_ibdocid)
        {
            try
            {

                InboundInquiry objInboundInquiry = new InboundInquiry();
                InboundInquiryService objService = new InboundInquiryService();
                string l_str_search_flag = string.Empty;
                string l_str_is_another_usr = string.Empty;
                l_str_is_another_usr = Session["IS3RDUSER"].ToString();
                objInboundInquiry.IS3RDUSER = l_str_is_another_usr.Trim();
                l_str_search_flag = Session["g_str_Search_flag"].ToString().Trim();
                objInboundInquiry.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                objInboundInquiry.user_id = Session["UserID"].ToString().Trim();
                objInboundInquiry.cmp_id = p_str_cmp_id;
                objInboundInquiry.ib_doc_id = p_str_ibdocid;
                objInboundInquiry.scn_id = "INBOUND";
                objInboundInquiry.status = "OPEN";
                objInboundInquiry.opt = "B";
                objInboundInquiry = objService.GetdocEditCount(objInboundInquiry);
                return Json(objInboundInquiry.LstItmExist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
