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
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using GsEPWv8_5_MVC.Common;
using System.Globalization;
using System.IO;
using GsEPWv8_4_MVC.Common;

namespace GsEPWv8_5_MVC.Controllers
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
    //CR20180813-001 Added By Nithya For insert Audit trail table
    //CR20180825-001 Added By Nithya For Allow New LocId 
    //CR - 3PL_MVC_IB_2018_0910_001 BY NITHYA For Add TotCtn,totWgt,TotCube Values in Inbound All Reports
    #endregion Change History

    public class InboundInquiryController : Controller
    {
        public string EmailSub = string.Empty;
        public string EmailMsg = string.Empty;
        public string Folderpath = string.Empty;
        public string ScreenID = "Inbound Inquiry";
        public string imgpath = string.Empty;
        public string strlogopath = string.Empty;
        Email objEmail = new Email();
        EmailService objEmailService = new EmailService();
        CustMaster objCustMaster = new CustMaster();
        ICustMasterService objCustMasterService = new CustMasterService();
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
                Session["sess_str_doc_type"] = "IB";
                clsGlobal.DocItemCode = string.Empty;
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
                    objInboundInquiry.cmp_id = objCompany.cmp_id;
                }
                objInboundInquiry.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                Session["screentitle"] = 0;
                objInboundInquiry.screentitle = screentitle;
                Session["screentitle"] = objInboundInquiry.screentitle;
                Session["l_bool_edit_flag"] = false;
                objInboundInquiry.l_bool_edit_flag = Session["l_bool_edit_flag"].ToString();

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
                    objInboundInquiry.ib_doc_dt_fm = DateTime.Now.AddDays(Common.clsGlobal.DispDateFrom).ToString("MM/dd/yyyy");
                    objInboundInquiry.ib_doc_dt_to = DateTime.Now.ToString("MM/dd/yyyy");
                    objInboundInquiry.status = "ALL";
                    LookUp objLookUp = new LookUp();
                    LookUpService ServiceObject1 = new LookUpService();
                    objLookUp.id = "1";
                    objLookUp.lookuptype = "DOCINQUIRY";
                    objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                    objInboundInquiry.ListLookUpDtl = objLookUp.ListLookUpDtl;
                    objInboundInquiry.cmp_id = objInboundInquiry.cmp_id;
                    objInboundInquiry.cntr_id = "";
                    objInboundInquiry.status = "";
                    objInboundInquiry.ib_doc_id = DateFm;
                    objInboundInquiry.ib_doc_id_to = DateFm;
                    objInboundInquiry.req_num = "";
                    objInboundInquiry.eta_dt_fm = "";
                    objInboundInquiry.eta_dt_to = "";
                    objInboundInquiry.screentitle = screentitle;

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
                    objInboundInquiry.eta_dt_fm = "";
                    objInboundInquiry.eta_dt_to = "";
                    objInboundInquiry.screentitle = screentitle;
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
            //objInboundInquiry = objService.GetdocEditCount(objInboundInquiry);
            //if (objInboundInquiry.LstItmExist.Count > 0)
            //{
            //    objInboundInquiry.editmode = objInboundInquiry.LstItmExist[0].editmode;
            //}
            //if (objInboundInquiry.editmode != 0)
            //{
            //    int RowCount = 1;
            //    return Json(RowCount, JsonRequestBehavior.AllowGet);
            //}
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
            //objInboundInquiry = objService.InsertdocEditEntry(objInboundInquiry);
            objInboundInquiry = objService.GetDocHdr(objInboundInquiry);
            objInboundInquiry.cmp_id = objInboundInquiry.ListDocHdr[0].cmp_id.Trim();
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
            objInboundInquiry = objService.LoadCustConfig(objInboundInquiry);
            if (objInboundInquiry.ListCustConfigDetails.Count > 0)
            {
                objInboundInquiry.Recv_Itm_Mode = objInboundInquiry.ListCustConfigDetails[0].Recv_Itm_Mode;
                objInboundInquiry.aloc_by = objInboundInquiry.ListCustConfigDetails[0].aloc_by;
                objInboundInquiry.ecom_recv_by_bin = objInboundInquiry.ListCustConfigDetails[0].ecom_recv_by_bin;
                objInboundInquiry.cube_auto_calc = objInboundInquiry.ListCustConfigDetails[0].cube_auto_calc;
            }
          
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
            decimal p_str_weight, decimal p_str_cube, string p_str_loc_id, string p_str_strg_rate, string p_str_inout_rate, string p_str_note, int p_int_ctn_line,
            string p_str_factory_id, string p_str_cust_name, string p_str_cust_po_num, string p_str_pick_list)
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
                objInboundInquiry.factory_id = p_str_factory_id;
                objInboundInquiry.cust_name = p_str_cust_name;
                objInboundInquiry.cust_po_num = p_str_cust_po_num;
                objInboundInquiry.pick_list = p_str_pick_list;
                objInboundInquiry = objService.LoadCustConfig(objInboundInquiry);
                if (objInboundInquiry.ListCustConfigDetails[0].ecom_recv_by_bin == "1")
                {
                    if (p_str_loc_id != "FLOOR")
                    {
                        string BinLocRefered = objService.fnCheckCheckBinRefered(p_str_cmp_id, p_str_loc_id, p_str_style, p_str_color, p_str_size, p_str_Itmcode);
                        if (BinLocRefered != "NO")
                        {
                            return Json(2, JsonRequestBehavior.AllowGet);
                        }
                    }

                }


                if (objInboundInquiry.ListCustConfigDetails.Count() != 0)
                {
                    objInboundInquiry.Allow_New_item = objInboundInquiry.ListCustConfigDetails[0].Allow_New_item;
                    objInboundInquiry.Recv_non_doc_itm = objInboundInquiry.ListCustConfigDetails[0].Recv_non_doc_itm;

                }
                //CR20180825-001 Added By Nithya
                objInboundInquiry = objService.GetDftWhs(objInboundInquiry);
                string l_str_DftWhs = (objInboundInquiry.ListPickdtl[0].dft_whs == null || objInboundInquiry.ListPickdtl[0].dft_whs == "" ? string.Empty : objInboundInquiry.ListPickdtl[0].dft_whs);
                if (l_str_DftWhs != "" || l_str_DftWhs != null)
                {
                    objInboundInquiry.whs_id = l_str_DftWhs;
                }
                else
                {
                    l_str_DftWhs = "";
                }
                LocationMaster objLocationMaster = new LocationMaster();
                ILocationMasterService ServiceObject = new LocationMasterService();
                objLocationMaster.cmp_id = p_str_cmp_id;
                objLocationMaster.whs_id = l_str_DftWhs;
                objLocationMaster.loc_id = p_str_loc_id;
                objLocationMaster.loc_desc = "";
                //objLocationMaster.option = "3";
                objLocationMaster = ServiceObject.CHECKLOCIDEXIST(objLocationMaster);
                if (objLocationMaster.ListLocationMasterDetails.Count == 0)
                {
                    objLocationMaster.whs_id = l_str_DftWhs;
                    objLocationMaster.loc_id = p_str_loc_id;
                    objLocationMaster.option = "1";
                    objLocationMaster.loc_desc = p_str_loc_id;
                    objLocationMaster.status = "";
                    objLocationMaster.note = p_str_note;
                    objLocationMaster.length = p_str_length;
                    objLocationMaster.width = p_str_width;
                    objLocationMaster.depth = p_str_height;
                    objLocationMaster.cube = p_str_cube;
                    objLocationMaster.usage = "";
                    objLocationMaster.process_id = "Add";
                    objLocationMaster.loc_type = "BIN";
                    objLocationMaster = ServiceObject.InsertLocationMasterDetails(objLocationMaster);
                }

                //END
                objInboundInquiry = objService.CheckItemExist(objInboundInquiry);
                if (objInboundInquiry.Allow_New_item == "Y")
                {
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

                    else

                    {

                        // If the Leave event is not triggred then the item code should be taken again.


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
                        objInboundInquiry.length = p_str_length;
                        objInboundInquiry.width = p_str_width;
                        objInboundInquiry.height = p_str_height;
                        objInboundInquiry.weight = p_str_weight;
                        objInboundInquiry.cube = p_str_cube;

                        objInboundInquiry = objService.GetItmId(objInboundInquiry);
                        objInboundInquiry.itmid = objInboundInquiry.itm_code;
                        l_str_itmcode = objInboundInquiry.itm_code;
                        objInboundInquiry.itm_code = l_str_itmcode;
                        p_str_Itmcode = l_str_itmcode;
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
                        objInboundInquiry.length = p_str_length;
                        objInboundInquiry.width = p_str_width;
                        objInboundInquiry.height = p_str_height;
                        objInboundInquiry.weight = p_str_weight;
                        objInboundInquiry.cube = p_str_cube;
                        objInboundInquiry.flag = "M";
                        objInboundInquiry.itm_code = p_str_Itmcode;
                        objService.Add_Style_To_Itm_dtl(objInboundInquiry);

                    }

                }
                else
                {
                    if (p_str_Itmcode == "")
                    {
                        int l_str_Count = 0;
                        return Json(l_str_Count, JsonRequestBehavior.AllowGet);
                    }
                }
                if (objInboundInquiry.itm_code != clsGlobal.DocItemCode)
                {
                    objInboundInquiry.itm_code = clsGlobal.DocItemCode;
                    objInboundInquiry = objService.GetCheckExistGridData(objInboundInquiry);
                }
                else
                {
                    objInboundInquiry.itm_code = p_str_Itmcode;
                    objInboundInquiry = objService.GetCheckExistGridData(objInboundInquiry);
                }
                clsGlobal.DocItemCode = string.Empty;
                objInboundInquiry.itm_code = p_str_Itmcode;
                //CR_3PL_MVC_BL_2018_0306_001 Added By Meera
                objInboundInquiry = objService.GetDocEntryId(objInboundInquiry);
                objInboundInquiry.doc_entry_id = objInboundInquiry.doc_entry_id;
                l_str_doc_entry_id = objInboundInquiry.doc_entry_id;
                objInboundInquiry.doc_entry_id = l_str_doc_entry_id;
                objInboundInquiry.itm_num = p_str_style;
                objInboundInquiry.itm_color = p_str_color;
                objInboundInquiry.itm_size = p_str_size;
                objInboundInquiry.itm_name = p_str_desc;

                objInboundInquiry.po_num = p_str_po_num;

                objInboundInquiry.factory_id = p_str_factory_id;
                objInboundInquiry.cust_name = p_str_cust_name;
                objInboundInquiry.cust_po_num = p_str_cust_po_num;
                objInboundInquiry.pick_list = p_str_pick_list;

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
                IBinMasterService ServiceBinMaster = new BinMasterService();
                ServiceBinMaster.fnInsertBinLocByIBDocId(p_str_cmp_id, l_str_DftWhs, p_str_loc_id, p_str_Itmcode, p_str_desc);
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
           decimal p_str_weight, decimal p_str_cube, string p_str_loc_id, string p_str_strg_rate, string p_str_inout_rate, string p_str_note, int p_int_ctn_line,
           string p_str_factory_id, string p_str_cust_name, string p_str_cust_po_num, string p_str_pick_list)
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
                objInboundInquiry.factory_id = p_str_factory_id;
                objInboundInquiry.cust_name = p_str_cust_name;
                objInboundInquiry.cust_po_num = p_str_cust_po_num;
                objInboundInquiry.pick_list = p_str_pick_list;
                //CR20180914-001 Added By Nithya
                objInboundInquiry = objService.LoadCustConfig(objInboundInquiry);
                if (objInboundInquiry.ListCustConfigDetails.Count() != 0)
                {
                    objInboundInquiry.Allow_New_item = objInboundInquiry.ListCustConfigDetails[0].Allow_New_item;
                    objInboundInquiry.Recv_non_doc_itm = objInboundInquiry.ListCustConfigDetails[0].Recv_non_doc_itm;

                }

                if (objInboundInquiry.ListCustConfigDetails[0].ecom_recv_by_bin == "1")
                {
                    if (p_str_loc_id != "FLOOR")
                    {
                        string BinLocRefered = objService.fnCheckCheckBinRefered(p_str_cmp_id, p_str_loc_id, p_str_style, p_str_color, p_str_size, p_str_Itmcode);
                        if (BinLocRefered != "NO")
                        {
                            return Json(2, JsonRequestBehavior.AllowGet);
                        }
                    }

                }

                //END
                //CR20180825-001 Added By Nithya
                objInboundInquiry = objService.GetDftWhs(objInboundInquiry);
                string l_str_DftWhs = (objInboundInquiry.ListPickdtl[0].dft_whs == null || objInboundInquiry.ListPickdtl[0].dft_whs == "" ? string.Empty : objInboundInquiry.ListPickdtl[0].dft_whs);
                if (l_str_DftWhs != "" || l_str_DftWhs != null)
                {
                    objInboundInquiry.whs_id = l_str_DftWhs;
                }
                else
                {
                    l_str_DftWhs = "";
                }
                LocationMaster objLocationMaster = new LocationMaster();
                ILocationMasterService ServiceObject = new LocationMasterService();
                objLocationMaster.cmp_id = p_str_cmp_id;
                objLocationMaster.whs_id = l_str_DftWhs;
                objLocationMaster.loc_id = p_str_loc_id;
                // objLocationMaster.option = "3";
                objLocationMaster = ServiceObject.CHECKLOCIDEXIST(objLocationMaster);
                if (objLocationMaster.ListLocationMasterDetails.Count == 0)
                {
                    objLocationMaster.whs_id = l_str_DftWhs;
                    objLocationMaster.loc_id = p_str_loc_id;
                    objLocationMaster.option = "1";
                    objLocationMaster.loc_desc = p_str_loc_id;
                    objLocationMaster.status = "";
                    objLocationMaster.note = p_str_note;
                    objLocationMaster.length = p_str_length;
                    objLocationMaster.width = p_str_width;
                    objLocationMaster.depth = p_str_height;
                    objLocationMaster.cube = p_str_cube;
                    objLocationMaster.usage = "";
                    objLocationMaster.process_id = "Add";
                    objLocationMaster.loc_type = "BIN";
                    objLocationMaster = ServiceObject.InsertLocationMasterDetails(objLocationMaster);
                }
                //END
                objInboundInquiry = objService.CheckItemExist(objInboundInquiry);

                if (objInboundInquiry.Allow_New_item == "Y")
                {
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
                        objInboundInquiry.length = p_str_length;
                        objInboundInquiry.width = p_str_width;
                        objInboundInquiry.height = p_str_height;
                        objInboundInquiry.weight = p_str_weight;
                        objInboundInquiry.cube = p_str_cube;

                        objInboundInquiry = objService.GetItmId(objInboundInquiry);
                        objInboundInquiry.itmid = objInboundInquiry.itm_code;
                        l_str_itmcode = objInboundInquiry.itm_code;
                        objInboundInquiry.itm_code = l_str_itmcode;
                        p_str_Itmcode = l_str_itmcode;
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
                        objInboundInquiry.length = p_str_length;
                        objInboundInquiry.width = p_str_width;
                        objInboundInquiry.height = p_str_height;
                        objInboundInquiry.weight = p_str_weight;
                        objInboundInquiry.cube = p_str_cube;
                        objInboundInquiry.flag = "M";
                        objInboundInquiry.itm_code = p_str_Itmcode;
                        objService.Add_Style_To_Itm_dtl(objInboundInquiry);
                    }
                    //End
                }
                else
                {
                    if (p_str_Itmcode == "")
                    {
                        int l_str_Count = 0;
                        return Json(l_str_Count, JsonRequestBehavior.AllowGet);
                    }
                }
                if (objInboundInquiry.itm_code != clsGlobal.DocItemCode)
                {
                    objInboundInquiry.itm_code = clsGlobal.DocItemCode;
                    objInboundInquiry = objService.GetCheckExistGridData(objInboundInquiry);
                }
                else
                {
                    objInboundInquiry.itm_code = p_str_Itmcode;
                    objInboundInquiry = objService.GetCheckExistGridData(objInboundInquiry);
                }
                clsGlobal.DocItemCode = string.Empty;
                objInboundInquiry.itm_code = p_str_Itmcode;
                //objInboundInquiry = objService.GetCheckExistGridData(objInboundInquiry);
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
                objInboundInquiry.factory_id = p_str_factory_id;
                objInboundInquiry.cust_name = p_str_cust_name;
                objInboundInquiry.cust_po_num = p_str_cust_po_num;
                objInboundInquiry.pick_list = p_str_pick_list;
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

                IBinMasterService ServiceBinMaster = new BinMasterService();
                ServiceBinMaster.fnInsertBinLocByIBDocId(p_str_cmp_id, l_str_DftWhs, p_str_loc_id, p_str_Itmcode, p_str_desc);

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
            if (clsGlobal.DocItemCode == string.Empty)
            {
                clsGlobal.DocItemCode = objInboundInquiry.itm_code;
                Session["tempItmCode"] = objInboundInquiry.itm_code;
            }
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
            Session["tempItmCode"] = objInboundInquiry.itm_code;//CR20180914
            if (clsGlobal.DocItemCode == string.Empty)
            {
                clsGlobal.DocItemCode = objInboundInquiry.itm_code;
                Session["tempItmCode"] = objInboundInquiry.itm_code;
            }
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
            try
            {

           
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
            }
            catch (Exception Ex)
            {
                Session["TEMP_CNTR_ID"] =string.Empty;
                Session["TEMP_STATUS"] = string.Empty;
                Session["TEMP_IB_DOC_ID_FM"] = string.Empty;
                Session["TEMP_IB_DOC_ID_TO"] = string.Empty;
                Session["TEMP_REF_NO"] = string.Empty;
                Session["TEMP_DOC_DT_FM"] = string.Empty;
                Session["TEMP_DOC_DT_TO"] = string.Empty;
                Session["TEMP_ETA_DT_FM"] = string.Empty; ;
                Session["TEMP_ETA_DT_TO"] = string.Empty; ;
                Session["TEMP_IB_RCVD_FM"] = string.Empty;
                Session["TEMP_SCREEN_ID"] = string.Empty;
                Session["TEMP_STYLE"] = string.Empty;
                Session["TEMP_COLOR"] = string.Empty;
                Session["TEMP_SIZE"] = string.Empty;
                Session["TEMP_DESC"] = string.Empty;
                Session["TEMP_STATUS"] = string.Empty;
            }
            finally
            {

            }
            return Json(objInboundInquiry.DocEntryCount, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CHECK_IB_DOC_IN_USE(string p_str_cmp_id, string p_str_ib_doc_id, int p_int_ib_doc_in_use)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();

            InboundInquiryService objService = new InboundInquiryService();
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.ib_doc_id = p_str_ib_doc_id;
            objInboundInquiry = objService.check_ib_doc_in_use(objInboundInquiry);
            p_int_ib_doc_in_use = objInboundInquiry.int_ib_doc_in_use;
            return Json(p_int_ib_doc_in_use, JsonRequestBehavior.AllowGet);
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
        public ActionResult GetSearchInboundInquiry(string p_str_cmp_id, string p_str_cntr_id, string p_str_status, string p_str_ib_doc_id, string p_str_ib_doc_id_to, string p_str_ref_no, string p_str_doc_dtFm,
            string p_str_doc_dtTo, string p_str_rcvd_dt_fm, string p_str_rcvd_dt_to, string p_str_eta_dtFm,
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
                objInboundInquiry.ib_rcvd_dt_fm = p_str_rcvd_dt_fm.Trim();
                objInboundInquiry.ib_rcvd_dt_to = p_str_rcvd_dt_to.Trim();
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
            int l_int_TotCtn = 0;
            decimal l_dec_Totwgt = 0;
            decimal l_dec_Totcube = 0;
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
            objCustMaster.cust_id = p_str_cmpid;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }
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
                        InboundInquiry objInboundInquiry = new InboundInquiry();
                        InboundInquiryService ServiceObject = new InboundInquiryService();
                    
                        if (type == "PDF")
                        {
                            strReportName = "rpt_ib_doc_entry_ack.rpt";
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                            objInboundInquiry.cmp_id = p_str_cmpid;
                            objCustMaster.cmp_id = p_str_cmpid;
                            objInboundInquiry.ib_doc_id = SelectdID;
                            objInboundInquiry = ServiceObject.GET_IB_DOC_CUBE_AND_WGT(objInboundInquiry);
                            if (objInboundInquiry.ListTotalCount.Count > 0)
                            {
                                l_int_TotCtn = objInboundInquiry.ListTotalCount[0].TOT_CARTON;
                                l_dec_Totwgt = objInboundInquiry.ListTotalCount[0].TOT_WEIGHT;
                                l_dec_Totcube = objInboundInquiry.ListTotalCount[0].TOTCUBE;
                            }
                            //end     
                            objInboundInquiry = ServiceObject.GetInboundAckRptDetails(objInboundInquiry);
                            int AlocCount = 0;
                            AlocCount = objInboundInquiry.ListAckRptDetails.Count();
                            if (AlocCount > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    var rptSource = objInboundInquiry.ListAckRptDetails.ToList();
                                    rd.Load(strRptPath);

                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                    rd.SetParameterValue("TotCtn", l_int_TotCtn);
                                    rd.SetParameterValue("TotWgt", l_dec_Totwgt);
                                    rd.SetParameterValue("TotCube", l_dec_Totcube);
                                    objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo;
                                    rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                }
                            }

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

                        }
                    }

                    if (l_str_rpt_selection == "WorkSheet")
                    {
                       
                        InboundInquiry objInboundInquiry = new InboundInquiry();
                        InboundInquiryService ServiceObject = new InboundInquiryService();

                        
                      
                        if (type == "PDF")
                        {
                            strReportName = "rpt_ib_doc_entry_recv_worksheet.rpt";
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                            objInboundInquiry.cmp_id = p_str_cmpid;
                            objInboundInquiry.ib_doc_id = SelectdID;
                            objInboundInquiry = ServiceObject.GET_IB_DOC_CUBE_AND_WGT(objInboundInquiry);
                            if (objInboundInquiry.ListTotalCount.Count > 0)
                            {
                                l_int_TotCtn = objInboundInquiry.ListTotalCount[0].TOT_CARTON;
                                l_dec_Totwgt = objInboundInquiry.ListTotalCount[0].TOT_WEIGHT;
                                l_dec_Totcube = objInboundInquiry.ListTotalCount[0].TOTCUBE;
                            }

                            objInboundInquiry = ServiceObject.GetInboundWorkSheetRptDetails(objInboundInquiry);
                            int AlocCount = 0;
                            AlocCount = objInboundInquiry.ListWorkSheetRptDetails.Count();
                            var rptSource = objInboundInquiry.ListWorkSheetRptDetails.ToList();
                            objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim();

                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);

                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                    rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                    rd.SetParameterValue("TotCtn", l_int_TotCtn);
                                    rd.SetParameterValue("TotWgt", l_dec_Totwgt);
                                    rd.SetParameterValue("TotCube", l_dec_Totcube);
                                    rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                            }
                                
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
                                if (objInboundInquiry.ListGETRateID[0].RATEID.Trim() == "CNTR")      
                                {
                                   
                                                                                             
                                    if (type == "PDF")
                                    {
                                        strReportName = "rpt_ib_doc_recv_post_confrimation_by_container.rpt";
                                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                        objInboundInquiry.cmp_id = p_str_cmpid;
                                        objInboundInquiry.ib_doc_id = SelectdID;
                                        objInboundInquiry = ServiceObject.GetInboundConfirmationRptDetailsbyContainer(objInboundInquiry);
                                        int AlocCount = 0;
                                        AlocCount = objInboundInquiry.ListConfirmationRptDetails.Count();
                                        objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim();

                                        var rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();
                                        using (ReportDocument rd = new ReportDocument())
                                        {
                                            rd.Load(strRptPath);
                                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                                rd.SetDataSource(rptSource);
                                                rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                                rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        }
                                         
                                    }
                                  
                                    else if (type == "Excel")
                                    {
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
                                    
                                    if (type == "PDF")
                                    {
                                        objInboundInquiry.cmp_id = p_str_cmpid;
                                        objInboundInquiry.ib_doc_id = SelectdID;
                                        strReportName = "rpt_ib_doc_recv_post_confrimation.rpt";
                                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                        objInboundInquiry = ServiceObject.GET_IB_RCVD_DOC_CUBE_AND_WGT(objInboundInquiry);
                                        if (objInboundInquiry.ListTotalCount.Count > 0)
                                        {
                                            l_int_TotCtn = objInboundInquiry.ListTotalCount[0].TOT_CARTON;
                                            l_dec_Totwgt = objInboundInquiry.ListTotalCount[0].TOT_WEIGHT;
                                            l_dec_Totcube = objInboundInquiry.ListTotalCount[0].TOTCUBE;
                                        }
                                        objInboundInquiry = ServiceObject.GetInboundConfirmationRptDetails(objInboundInquiry);
                                        IList<InboundInquiry> rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();

                                        if (rptSource.Count > 0)
                                        {
                                            using (ReportDocument rd = new ReportDocument())
                                            {
                                                rd.Load(strRptPath);
                                                rd.SetDataSource(rptSource);
                                                rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                                rd.SetParameterValue("TotCtn", l_int_TotCtn);
                                                rd.SetParameterValue("TotWgt", l_dec_Totwgt);
                                                rd.SetParameterValue("TotCube", l_dec_Totcube);
                                                objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                                rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                            }
                                               
                                        }
                                    }
                                   
                                    else if (type == "Excel")
                                    {

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
                           
                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                objInboundInquiry = ServiceObject.GET_IB_RCVD_DOC_CUBE_AND_WGT(objInboundInquiry);
                                if (objInboundInquiry.ListTotalCount.Count > 0)
                                {
                                    l_int_TotCtn = objInboundInquiry.ListTotalCount[0].TOT_CARTON;
                                    l_dec_Totwgt = objInboundInquiry.ListTotalCount[0].TOT_WEIGHT;
                                    l_dec_Totcube = objInboundInquiry.ListTotalCount[0].TOTCUBE;
                                }
                                objInboundInquiry = ServiceObject.GetInboundConfirmationRptDetails(objInboundInquiry);
                                IList<InboundInquiry> rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();

                                if (type == "PDF")
                                {
                                    if (rptSource.Count > 0)
                                    {
                                        using (ReportDocument rd = new ReportDocument())
                                        {
                                            rd.Load(strRptPath);
                                            rd.SetDataSource(rptSource);
                                            rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                            rd.SetParameterValue("TotCtn", l_int_TotCtn);
                                            rd.SetParameterValue("TotWgt", l_dec_Totwgt);
                                            rd.SetParameterValue("TotCube", l_dec_Totcube);
                                            objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                            rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        }
                                          
                                    }

                                   
                                }
                             else if (type == "Excel")
                                {
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
                                    objInboundInquiry.cmp_id = p_str_cmpid;
                                    objInboundInquiry.ib_doc_id = SelectdID;
                                                   
                                    if (type == "PDF")
                                    {
                                        strReportName = "rpt_ib_doc_recv_post_confrimation_by_container.rpt";
                                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                        objInboundInquiry = ServiceObject.GetInboundConfirmationRptDetailsbyContainer(objInboundInquiry);
                                        IList<InboundInquiry> rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();
                                        if (rptSource.Count > 0)
                                        {
                                            using (ReportDocument rd = new ReportDocument())
                                            { 
                                                rd.Load(strRptPath);
                                                rd.SetDataSource(rptSource);
                                                rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                                objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim();
                                                rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                            }
                                        }
                                      
                                    }

                                    else if (type == "Excel")
                                    {
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
             
                                    if (type == "PDF")
                                    {
                                        objInboundInquiry = ServiceObject.GET_IB_RCVD_DOC_CUBE_AND_WGT(objInboundInquiry);
                                        if (objInboundInquiry.ListTotalCount.Count > 0)
                                        {
                                            l_int_TotCtn = objInboundInquiry.ListTotalCount[0].TOT_CARTON;
                                            l_dec_Totwgt = objInboundInquiry.ListTotalCount[0].TOT_WEIGHT;
                                            l_dec_Totcube = objInboundInquiry.ListTotalCount[0].TOTCUBE;
                                        }
                                        objInboundInquiry = ServiceObject.GetInboundConfirmationRptDetails(objInboundInquiry);
                                        IList<InboundInquiry> rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();

                                        if (rptSource.Count > 0)
                                        { 
                                        strReportName = "rpt_ib_doc_recv_post_confrimation.rpt";
                                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                            using (ReportDocument rd = new ReportDocument())
                                            { 
                                                rd.Load(strRptPath);
                                                rd.SetDataSource(rptSource);
                                                rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                                rd.SetParameterValue("TotCtn", l_int_TotCtn);
                                                rd.SetParameterValue("TotWgt", l_dec_Totwgt);
                                                rd.SetParameterValue("TotCube", l_dec_Totcube);
                                                objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                                rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                            }

                                        }
                                    }
                                   
                                    else if (type == "Excel")
                                    {
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
                                
                                if (type == "PDF")
                                {
                                    strReportName = "rpt_ib_doc_recv_post_confrimation.rpt";
                                    
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                    objInboundInquiry = ServiceObject.GET_IB_RCVD_DOC_CUBE_AND_WGT(objInboundInquiry);
                                    if (objInboundInquiry.ListTotalCount.Count > 0)
                                    {
                                        l_int_TotCtn = objInboundInquiry.ListTotalCount[0].TOT_CARTON;
                                        l_dec_Totwgt = objInboundInquiry.ListTotalCount[0].TOT_WEIGHT;
                                        l_dec_Totcube = objInboundInquiry.ListTotalCount[0].TOTCUBE;
                                    }

                                    objInboundInquiry = ServiceObject.GetInboundConfirmationRptDetails(objInboundInquiry);
                                    IList<InboundInquiry> rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();
                                    if (rptSource.Count > 0)
                                    {
                                        using (ReportDocument rd = new ReportDocument())
                                        { 
                                            rd.Load(strRptPath);
                                            rd.SetDataSource(rptSource);
                                            rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                            rd.SetParameterValue("TotCtn", l_int_TotCtn);
                                            rd.SetParameterValue("TotWgt", l_dec_Totwgt);
                                            rd.SetParameterValue("TotCube", l_dec_Totcube);
                                            objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim();
                                            rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                        }
                                    }
                                }

                                else if (type == "Excel")
                                {
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
                        else if (l_str_status == "1-RCVD")
                        {
                            InboundInquiry objInboundInquiry = new InboundInquiry();
                            InboundInquiryService ServiceObject = new InboundInquiryService();
                            if (type == "PDF")
                            {
                                objInboundInquiry.cmp_id = p_str_cmpid;
                                objInboundInquiry.ib_doc_id = SelectdID;
                                objInboundInquiry = ServiceObject.GET_IB_RCVD_DOC_CUBE_AND_WGT(objInboundInquiry);
                                if (objInboundInquiry.ListTotalCount.Count > 0)
                                {
                                    l_int_TotCtn = objInboundInquiry.ListTotalCount[0].TOT_CARTON;
                                    l_dec_Totwgt = objInboundInquiry.ListTotalCount[0].TOT_WEIGHT;
                                    l_dec_Totcube = objInboundInquiry.ListTotalCount[0].TOTCUBE;
                                }
                                objInboundInquiry = ServiceObject.GetInboundTallySheetRptDetails(objInboundInquiry);
                                IList<InboundInquiry> rptSource = objInboundInquiry.ListTallySheetRptDetails.ToList();
                                if (rptSource.Count > 0)
                                {
                                strReportName = "rpt_ib_doc_recv_tallysheet.rpt";
                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                    using (ReportDocument rd = new ReportDocument())
                                    { 
                                        rd.Load(strRptPath);
                                        rd.SetDataSource(rptSource);
                                        rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                        rd.SetParameterValue("TotCtn", l_int_TotCtn);
                                        rd.SetParameterValue("TotWgt", l_dec_Totwgt);
                                        rd.SetParameterValue("TotCube", l_dec_Totcube);
                                        DataTable dtIBDocValidateItem = ServiceObject.GetIBCheckIbDocRcvdCubeCheck(p_str_cmpid, SelectdID);
                                        if (dtIBDocValidateItem.Rows.Count > 0)
                                        {
                                            if (dtIBDocValidateItem.Rows[0]["dup_itm_count"].ToString() == string.Empty)
                                            {
                                                rd.SetParameterValue("fml_duplicate_item", "No Duplicate");
                                            }
                                            else
                                            {
                                                rd.SetParameterValue("fml_duplicate_item", "DUPLICATE ITEMS RECEIVED");
                                            }

                                            if ((dtIBDocValidateItem.Rows[0]["less_cube_count"].ToString() == string.Empty) || (dtIBDocValidateItem.Rows[0]["less_cube_count"].ToString() == "0"))
                                            {
                                                rd.SetParameterValue("fml_cube_size_issue", "OK");
                                            }
                                            else
                                            {
                                                rd.SetParameterValue("fml_cube_size_issue", "CUBE MISSING!’  ‘MUST GET / MEASURE CUBE and ENTER on RECEIVING!");
                                            }


                                            if (dtIBDocValidateItem.Rows[0]["cntr_cube_mismatch"].ToString() == "N")
                                            {
                                                rd.SetParameterValue("fml_cntr_cube_mismatch", "OK");
                                            }
                                            else if (dtIBDocValidateItem.Rows[0]["cntr_cube_mismatch"].ToString() == "L")
                                            {
                                                rd.SetParameterValue("fml_cntr_cube_mismatch", "RECEIVED CUBE LESS THAN CONTAINER SIZE");
                                            }
                                            else if (dtIBDocValidateItem.Rows[0]["cntr_cube_mismatch"].ToString() == "H")
                                            {
                                                rd.SetParameterValue("fml_cntr_cube_mismatch", "RECEIVED CUBE GREATER THAN CONTAINER SIZE");
                                            }

                                        }
                                        else
                                        {
                                            rd.SetParameterValue("fml_duplicate_item", string.Empty);
                                            rd.SetParameterValue("fml_cube_size_issue", string.Empty);
                                            rd.SetParameterValue("fml_cntr_cube_mismatch", string.Empty);
                                        }
                                objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); 
                                rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                }
                              }
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
                        IList<InboundInquiry> rptSource = objInboundInquiry.ListGridSummaryRptDetails.ToList();

                        if(rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            { 
                                rd.Load(strRptPath);
                                rd.SetDataSource(rptSource);
                                rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name); 
                                if (type == "PDF")
                                {
                                    objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim();
                                    rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                }
                        
                                else if (type == "Excel")
                                {
                                    rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                }
                            }
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

            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cmp_id = p_str_cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            objInbound.LstCmpName = objCompany.LstCmpName;

            l_str_tmp_name = objInbound.LstCmpName[0].cmp_name.ToString().Trim();
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
                        InboundInquiry objInboundInquiry = new InboundInquiry();
                        InboundInquiryService ServiceObject = new InboundInquiryService();
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
                        

                        if (type == "PDF")
                        {
                            
                            objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim();
                            objInboundInquiry = ServiceObject.GetInboundGridSummaryDetails(objInboundInquiry);
                            strReportName = "rpt_inbound_grid_summary.rpt";
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;

                           
                            IList<InboundInquiry> rptSource = objInboundInquiry.ListGridSummaryRptDetails.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    rd.SetDataSource(rptSource);
                                    rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                    rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                }
                                    
                            }
                            else
                            {
                                Response.Write("<H2>Record not found</H2>");
                            }
                        }

                        else if (type == "Excel")
                        {

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
            objInboundInquiry.cmp_id = cmp_id;
            objInboundInquiry = ServiceObject.GetInboundDtl(objInboundInquiry);
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_InboundRcvdDetail", InboundInquiryModel);
        }

        public ActionResult LoadItemCodeDetails(string Id, string cmp_id)
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
            objInboundInquiry.cmp_id = cmp_id;
            objInboundInquiry = ServiceObject.GetInboundDtl(objInboundInquiry);
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_ItemCodeScanDetails", InboundInquiryModel);
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
            objInboundInquiry.cntr_type = (objInboundInquiry.ListAckRptDetails[0].cntr_type == null || objInboundInquiry.ListAckRptDetails[0].cntr_type == "") ? string.Empty : objInboundInquiry.ListAckRptDetails[0].cntr_type.Trim();
            objInboundInquiry.ib_load_dt = (objInboundInquiry.ListAckRptDetails[0].ib_load_dt == null || objInboundInquiry.ListAckRptDetails[0].ib_load_dt == "") ? string.Empty : objInboundInquiry.ListAckRptDetails[0].ib_load_dt.Trim();

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
            string l_str_whsId = string.Empty;
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
            objInboundInquiry = ServiceObject.GetDftWhs(objInboundInquiry);
            if (objInboundInquiry.ListPickdtl.Count > 0)
            {
                l_str_DftWhs = (objInboundInquiry.ListPickdtl[0].dft_whs.Trim() == null || objInboundInquiry.ListPickdtl[0].dft_whs.Trim() == string.Empty) ? string.Empty : objInboundInquiry.ListPickdtl[0].dft_whs.Trim();
            }

            if (l_str_DftWhs == "" || l_str_DftWhs == null)
            {
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
                l_str_whsId = "WHS";
                return Json(l_str_whsId, JsonRequestBehavior.AllowGet);
            }
            objInboundInquiry = ServiceObject.AddLocId(objInboundInquiry);
            objInboundInquiry = ServiceObject.LoadCustConfig(objInboundInquiry);
            if (objInboundInquiry.ListCustConfigDetails.Count > 0)
            {
                objInboundInquiry.Recv_Itm_Mode = objInboundInquiry.ListCustConfigDetails[0].Recv_Itm_Mode;
                objInboundInquiry.aloc_by = objInboundInquiry.ListCustConfigDetails[0].aloc_by;
                objInboundInquiry.ecom_recv_by_bin = objInboundInquiry.ListCustConfigDetails[0].ecom_recv_by_bin;
                objInboundInquiry.cube_auto_calc = objInboundInquiry.ListCustConfigDetails[0].cube_auto_calc;

            }
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
            objInboundInquiry.strg_rate = "STRG-1";
            objInboundInquiry.loc_id = "FLOOR";
            objInboundInquiry = ServiceObject.LoadStrgId(objInboundInquiry);


            if (objInboundInquiry.LstStrgIddtl.Count() == 0)
            {
                l_str_rate = "STRG";
                return Json(l_str_rate, JsonRequestBehavior.AllowGet);
            }

            objInboundInquiry = ServiceObject.LoadInoutId(objInboundInquiry);


            if (objInboundInquiry.LstInoutIddtl.Count() == 0)
            {
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
        public ActionResult ReceivingEntry(string CmpId, string id, string pstrIbDocDate, string Cont_id, string datefrom, string dateto)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            string l_str_lot_id = string.Empty;
            string l_str_rcvd_itm_mode = string.Empty;
            DateTime l_str_rcd_dt;
            bool lblnIBDocRcvd = false;
            objInboundInquiry.DocumentdateFrom = datefrom;
            objInboundInquiry.DocumentdateTo = dateto;
            objInboundInquiry.ib_doc_dt = pstrIbDocDate;
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
            objInboundInquiry.loc_id = "FLOOR";//CR20180825-001 Commented By Nithya
            objInboundInquiry.palet_id = "NEW";
            lblnIBDocRcvd= ServiceObject.checkIBDocRcvd(CmpId, id);
            if (lblnIBDocRcvd == true)
            {
                return Json("IB DOC ALREADY RCVD", JsonRequestBehavior.AllowGet);
            }
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
            //CR20180829-001 
            if (objInboundInquiry.ListGetCustConfigDtls[0].Allow_New_item == "Y")
            {
                objInboundInquiry.Allow_New_item = "Y";
            }
            else
            {
                objInboundInquiry.Allow_New_item = "N";
            }
            if (objInboundInquiry.ListGetCustConfigDtls[0].Recv_non_doc_itm == "Y")
            {
                objInboundInquiry.Recv_non_doc_itm = "Y";
            }
            else
            {
                objInboundInquiry.Recv_non_doc_itm = "N";
            }
            //END
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
            string l_str_DftWhs = string.Empty;
            objInboundInquiry = ServiceObject.GetDftWhs(objInboundInquiry);
            if (objInboundInquiry.ListPickdtl.Count > 0)
            {
                l_str_DftWhs = objInboundInquiry.ListPickdtl[0].dft_whs.Trim();
            }
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

            if (objInboundInquiry.LstStrgIddtl.Count() == 0)
            {
                return Json("STRG-RATE-NOT-FOUND", JsonRequestBehavior.AllowGet);
            }

           

            for (int i = 0; i < objInboundInquiry.LstStrgIddtl.Count(); i++)
            {
                objInboundInquiry.strg_rate = "STRG-1";
                objInboundInquiry.strg_rate = objInboundInquiry.LstStrgIddtl[i].strg_rate;
            }
            
            objInboundInquiry.inout_rate = "INOUT-1";
            objInboundInquiry = ServiceObject.LoadInoutId(objInboundInquiry);
            if (objInboundInquiry.LstInoutIddtl.Count() == 0)
            {
                return Json("INOUT-RATE-NOT-FOUND", JsonRequestBehavior.AllowGet);
            }
            if (objInboundInquiry.LstInoutIddtl.Count() == 0)
            {
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
            //CR20180904-001
            objInboundInquiry.dtl_line = objInboundInquiry.recv_dtl_count + 1;
            //END
            objInboundInquiry.cmp_id = CmpId;
            objInboundInquiry.ib_doc_id = id;
            objInboundInquiry.lot_id = "";

            objInboundInquiry = ServiceObject.GetRcvdEntryCountDtl(objInboundInquiry);
            objInboundInquiry.recvcount = objInboundInquiry.LstRcvdEntryCountDtl[0].recvcount;
            objInboundInquiry = ServiceObject.LoadCustConfig(objInboundInquiry);
            if (objInboundInquiry.ListCustConfigDetails.Count > 0)
            {
                objInboundInquiry.Recv_Itm_Mode = objInboundInquiry.ListCustConfigDetails[0].Recv_Itm_Mode;
                objInboundInquiry.aloc_by = objInboundInquiry.ListCustConfigDetails[0].aloc_by;
                objInboundInquiry.ecom_recv_by_bin = objInboundInquiry.ListCustConfigDetails[0].ecom_recv_by_bin;
            }
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

            LookUp objLookUp = new LookUp();
            LookUpService ServiceObject1 = new LookUpService();
            objLookUp.id = "103";
            objLookUp.lookuptype = "CNTR_SIZE";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);

            LookUp objLookUpDflt = new LookUp();

            objLookUpDflt.id = "103";
            objLookUpDflt.lookuptype = "CNTR_SIZE";
            objLookUpDflt.name = "--Select CNTR Type--";
            objLookUpDflt.description = "--Select CNTR Type--";
            objLookUp.ListLookUpDtl.Add(objLookUpDflt);

            objInboundInquiry.ListContainerType = objLookUp.ListLookUpDtl;
            objInboundInquiry.cntr_type = "--Select CNTR Type--";
            DataTable dtIBDocValidateItem = ServiceObject.GetIBCheckIbDocValidateItem(CmpId, id);
            if (dtIBDocValidateItem.Rows.Count > 0)
            {
                if (dtIBDocValidateItem.Rows[0]["dup_itm_count"].ToString() == string.Empty)
                {
                    objInboundInquiry.dup_itm_count = 0;
                }
                else
                {
                    objInboundInquiry.dup_itm_count = Convert.ToInt64(dtIBDocValidateItem.Rows[0]["dup_itm_count"].ToString());
                }

                if (dtIBDocValidateItem.Rows[0]["less_cube_count"].ToString() == string.Empty)
                {
                    objInboundInquiry.less_cube_count = 0;
                }
                else
                {
                    objInboundInquiry.less_cube_count = Convert.ToInt64(dtIBDocValidateItem.Rows[0]["less_cube_count"].ToString());
                }


            }
            else
            {
                objInboundInquiry.dup_itm_count = 0;
                objInboundInquiry.less_cube_count = 0;
            }
         
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_ReceivingEntry", InboundInquiryModel);
        }

        public ActionResult ReceivingDimEdit(string pstrCmpId, string pstrIBDocId)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            string l_str_rcvd_itm_mode = string.Empty;
            InboundInquiryService ServiceObject = new InboundInquiryService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objInboundInquiry.cmp_id = pstrCmpId;
            objInboundInquiry.ibdocid = pstrIBDocId;
            objInboundInquiry.ib_doc_id = pstrIBDocId;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany.cust_cmp_id = pstrCmpId;
            objCompany = ServiceObjectCompany.GetLocIdDetails(objCompany);
            objInboundInquiry.ListLocPickDtl = objCompany.ListLocPickDtl;

            objInboundInquiry = ServiceObject.GetRecvdtlGrid(objInboundInquiry);
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_ReceivingDimEdit", InboundInquiryModel);
        }

        public JsonResult SaveReceivingDimEdit( string pstrCmpId, string pstrIBDocId, List<IBRrecvDtlEditTemp> ItemDetails)
        {

            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            DataTable dtIBRrecvDtlEditTemp = new DataTable();
            dtIBRrecvDtlEditTemp = Utility.ConvertListToDataTable(ItemDetails);
            ServiceObject.fnIBRecvDtlEditTempSave(pstrCmpId, dtIBRrecvDtlEditTemp);

            objInboundInquiry = ServiceObject.GetRecvdtlGrid(objInboundInquiry);
           
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return Json(0, JsonRequestBehavior.AllowGet);

            //return PartialView("_ReceivingEntryGrid", InboundInquiryModel);

      
        }


        public ActionResult IncludeReceivingDimEdit(string pstrCmpId, string pstrIBDocId)
        {

            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            objInboundInquiry.cmp_id = pstrCmpId;
            objInboundInquiry.ib_doc_id = pstrIBDocId;
            objInboundInquiry = ServiceObject.GetRecvdtlGrid(objInboundInquiry);

            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
          return PartialView("_ReceivingEntryGrid", InboundInquiryModel);


        }

        public ActionResult ReceivingPost(string CmpId, string id, string Cont_id, string LotId, string datefrom, string dateto, string rcvd_dt)
        {
      
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            string l_str_item_sku_note = "Items(s)/SKU: ";      
            objInboundInquiry.DocumentdateFrom = datefrom; 
            objInboundInquiry.DocumentdateTo = dateto; 
            objInboundInquiry.cmp_id = CmpId;
            objInboundInquiry.ibdocid = id;
            objInboundInquiry.cntr_id = Cont_id;
            objInboundInquiry.lot_id = LotId;

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

    
            if (objInboundInquiry.ListRcvgPost.Count != 0)
            {
                objInboundInquiry.cmp_id = objInboundInquiry.ListRcvgPost[0].cmp_id.Trim();
                objInboundInquiry.ibdocid = objInboundInquiry.ListRcvgPost[0].ib_doc_id;
                objInboundInquiry.cont_id = objInboundInquiry.ListRcvgPost[0].cntr_id;
                objInboundInquiry.ctns = objInboundInquiry.ListAvailDtl[0].CtnQty;
                objInboundInquiry.tot_qty = objInboundInquiry.ListAvailDtl[0].TotQty;

            }
            objInboundInquiry.View_Flag = "P";

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
            objInboundInquiry.rcvd_dt = rcvd_dt;
            objInboundInquiry.ib_doc_dt = string.Empty;
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_DocReceivingPost", InboundInquiryModel);
        }
        public ActionResult SaveReceivingPost(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_cntr_id, string P_STR_CNTR_PALLET, string P_STR_CNTR_WEIGHT
     , string P_STR_CNTR_CUBE, string P_STR_RATEID, string P_STR_CNTR_NOTE, string P_STR_CNTR_PO_NUM, string P_STR_CNTR_ST_RATE_ID, string P_STR_BILL_TYPE)
        {
            string l_str_ibdocid;
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.ibdocid = p_str_ib_doc_id;
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
                    //CR20180813-001 Added By Nithya
                    objInboundInquiry.cmp_id = p_str_cmp_id;
                    objInboundInquiry.ib_doc_id = p_str_ib_doc_id;
                    objInboundInquiry.mode = "POST";
                    objInboundInquiry.maker = Session["UserID"].ToString().Trim();
                    objInboundInquiry.makerdt = DateTime.Now.ToString("MM/dd/yyyy");
                    objInboundInquiry.Auditcomment = "Posted";
                    ServiceObject.Add_To_proc_save_audit_trail(objInboundInquiry);
                    //END
                }

            }

            ServiceObject.ReceivingPostDtls(objInboundInquiry);
            bool is_add_auto_ibs_entry = false;
            is_add_auto_ibs_entry = ServiceObject.AddAutoSpecialIBSEntry(p_str_cmp_id, p_str_ib_doc_id, p_str_cntr_id);
            is_add_auto_ibs_entry = ServiceObject.AddAutoIBSEntry(p_str_cmp_id, p_str_ib_doc_id, p_str_cntr_id);
            

            if (objInboundInquiry.bill_inout_type == "Container")            // CR_3PL_MVC_BL_2018_0312_002 Added By SONIYA
            {
                objInboundInquiry.CONTAINERID = p_str_cntr_id;
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
                //CR20180813-001 Added By Nithya
                objInboundInquiry.cmp_id = p_str_cmp_id;
                objInboundInquiry.ib_doc_id = p_str_ib_doc_id;
                objInboundInquiry.mode = "POST";
                objInboundInquiry.maker = Session["UserID"].ToString().Trim();
                objInboundInquiry.makerdt = DateTime.Now.ToString("MM/dd/yyyy");
                objInboundInquiry.Auditcomment = "Posted";
                ServiceObject.Add_To_proc_save_audit_trail(objInboundInquiry);
                //END
            }
            else
            {
                objInboundInquiry.cmp_id = p_str_cmp_id;
                objInboundInquiry.ib_doc_id = p_str_ib_doc_id;
                ServiceObject.Update_Lot_Bill_Status(objInboundInquiry);
                //CR20180813-001 Added By Nithya
                objInboundInquiry.cmp_id = p_str_cmp_id;
                objInboundInquiry.ib_doc_id = p_str_ib_doc_id;
                objInboundInquiry.mode = "POST";
                objInboundInquiry.maker = Session["UserID"].ToString().Trim();
                objInboundInquiry.makerdt = DateTime.Now.ToString("MM/dd/yyyy");
                objInboundInquiry.Auditcomment = "Posted";
                ServiceObject.Add_To_proc_save_audit_trail(objInboundInquiry);
                //END
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
            //CR20180813-001 Added By Nithya
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.ib_doc_id = p_str_ibdocid;
            objInboundInquiry.mode = "UNPOST";
            objInboundInquiry.maker = Session["UserID"].ToString().Trim();
            objInboundInquiry.makerdt = DateTime.Now.ToString("MM/dd/yyyy");
            objInboundInquiry.Auditcomment = "Un Posted";
            ServiceObject.Add_To_proc_save_audit_trail(objInboundInquiry);
            //END
            //CR_3PL_MVC_BL_2018_0221_001 Added By Ravi 21-02-2018
            ServiceObject.Del_rcv_dtl(objInboundInquiry);
            //END                
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("~/Views/InboundInquiry/InboundInquiry.cshtml", InboundInquiryModel);
        }
        public ActionResult ShowDocRcvngUnPostReport(string p_str_cmpid, string p_str_lotid, string p_str_ibdocid)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string l_str_tmp_name = string.Empty;
            string msg = "";
            int l_int_TotCtn = 0;
            decimal l_dec_Totwgt = 0;
            decimal l_dec_Totcube = 0;
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cmp_id = p_str_cmpid;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            objInboundInquiry.LstCmpName = objCompany.LstCmpName;

            l_str_tmp_name = objInboundInquiry.LstCmpName[0].cmp_name.ToString().Trim();
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

                    objInboundInquiry.cmp_id = p_str_cmpid;
                    objInboundInquiry.ib_doc_id = p_str_ibdocid;
                    objInboundInquiry = ServiceObject.GET_IB_RCVD_DOC_CUBE_AND_WGT(objInboundInquiry);
                    if (objInboundInquiry.ListTotalCount.Count > 0)
                    {
                        l_int_TotCtn = objInboundInquiry.ListTotalCount[0].TOT_CARTON;
                        l_dec_Totwgt = objInboundInquiry.ListTotalCount[0].TOT_WEIGHT;
                        l_dec_Totcube = objInboundInquiry.ListTotalCount[0].TOTCUBE;
                    }

                    objInboundInquiry = ServiceObject.GetInboundTallySheetRptDetails(objInboundInquiry);
                    IList<InboundInquiry> rptSource = objInboundInquiry.ListTallySheetRptDetails.ToList();
                    if (rptSource.Count > 0)
                    { 
                    strReportName = "rpt_ib_doc_recv_tallysheet.rpt";
                        using (ReportDocument rd = new ReportDocument())
                        { 
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                            rd.Load(strRptPath);
                            rd.SetDataSource(rptSource);
                            rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name); 
                            rd.SetParameterValue("TotCtn", l_int_TotCtn);
                            rd.SetParameterValue("TotWgt", l_dec_Totwgt);
                            rd.SetParameterValue("TotCube", l_dec_Totcube);
                            rd.SetParameterValue("fml_duplicate_item", string.Empty);
                            rd.SetParameterValue("fml_cube_size_issue", string.Empty);
                            rd.SetParameterValue("fml_cntr_cube_mismatch", string.Empty);
                            objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); 
                            rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "UNPOST REPORT");
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
        public FileResult SampleTemplatedownload()
        {
            return File("~\\templates\\IB_DETAIL_UPLOAD_TEMPLATE_WITH_SAMPLE.xlsx", "text/xlsx", string.Format("IB_DETAIL_UPLOAD_TEMPLATE_WITH_SAMPLE-{0}.xlsx", DateTime.Now.ToString("yyyyMMdd-HHmmss")));
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
                    objInboundInquiry.factory_id = objInboundInquiry.ListDocdtl[i].factory_id;
                    objInboundInquiry.cust_name = objInboundInquiry.ListDocdtl[i].cust_name;
                    objInboundInquiry.cust_po_num = objInboundInquiry.ListDocdtl[i].cust_po_num;
                    objInboundInquiry.pick_list = objInboundInquiry.ListDocdtl[i].pick_list;
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
                //CR20180813-001 Added By Nithya
                objInboundInquiry.cmp_id = p_str_cmp_id;
                objInboundInquiry.ib_doc_id = p_str_ibdocid;
                objInboundInquiry.mode = "MODIFY";
                objInboundInquiry.maker = Session["UserID"].ToString().Trim();
                objInboundInquiry.makerdt = DateTime.Now.ToString("MM/dd/yyyy");
                objInboundInquiry.Auditcomment = "Modified Inbound Entry";
                ServiceObject.Add_To_proc_save_audit_trail(objInboundInquiry);
                //END
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
                    objInboundInquiry.factory_id = objInboundInquiry.ListDocdtl[i].factory_id;
                    objInboundInquiry.cust_name = objInboundInquiry.ListDocdtl[i].cust_name;
                    objInboundInquiry.cust_po_num = objInboundInquiry.ListDocdtl[i].cust_po_num;
                    objInboundInquiry.pick_list = objInboundInquiry.ListDocdtl[i].pick_list;
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

                //CR20180813-001 Added By Nithya
                objInboundInquiry.cmp_id = p_str_cmp_id;
                objInboundInquiry.ib_doc_id = p_str_ibdocid;
                objInboundInquiry.mode = "INPUT";
                objInboundInquiry.maker = Session["UserID"].ToString().Trim();
                objInboundInquiry.makerdt = DateTime.Now.ToString("MM/dd/yyyy");
                objInboundInquiry.Auditcomment = "Added new Inbound entry";
                ServiceObject.Add_To_proc_save_audit_trail(objInboundInquiry);
                //END
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
       string p_str_vend_id, string p_str_whs_id, string p_str_cont_id, string p_str_seal_num, string p_str_palet_id, string p_str_lot_id, string p_str_cntr_type, string p_str_ib_load_dt)
        {
            int ResultCount = 0;

            string l_str_rcvd_itm_mode = string.Empty;
            DateTime l_dt_IbdocDt;
            DateTime l_dt_rcvddocDt;
            //int dtlline = 0;
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            Session["tempIbdocId"] = objInboundInquiry.ib_doc_id;
            objInboundInquiry.ib_doc_id = p_str_ib_doc_id;
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.ib_doc_id = p_str_ib_doc_id;
            bool lblnIBDocRcvd = false;
            lblnIBDocRcvd = ServiceObject.checkIBDocRcvd(p_str_cmp_id, p_str_ib_doc_id);
            if (lblnIBDocRcvd == true)
            {
                ResultCount = 3;
                return Json(ResultCount, JsonRequestBehavior.AllowGet);
            }

            //CR20180910
            l_dt_rcvddocDt = Convert.ToDateTime(p_str_rcvd_dt);
            objInboundInquiry = ServiceObject.CHECKDOCDATE(objInboundInquiry);
            //if (objInboundInquiry.ListTotalCount.Count > 0)
            //{
            //    ResultCount = 2;
            //    return Json(ResultCount, JsonRequestBehavior.AllowGet);
            //}
            l_dt_IbdocDt = Convert.ToDateTime(objInboundInquiry.ib_doc_dt);
            if (l_dt_IbdocDt <= l_dt_rcvddocDt)
            {
                objInboundInquiry.ib_doc_dt = p_str_rcvd_dt;
            }
            else
            {
                ResultCount = 2;
                return Json(ResultCount, JsonRequestBehavior.AllowGet);
            }
            //END
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
            objInboundInquiry.cntr_type = p_str_cntr_type;
            objInboundInquiry.ib_load_dt = p_str_ib_load_dt;
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
                ResultCount = 1;
            }
            //CR20180813-001 Added By Nithya
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.ib_doc_id = p_str_ib_doc_id;
            objInboundInquiry.mode = "RCVD";
            objInboundInquiry.maker = Session["UserID"].ToString().Trim();
            objInboundInquiry.makerdt = DateTime.Now.ToString("MM/dd/yyyy");
            objInboundInquiry.Auditcomment = "Received";
            objInboundInquiry.cntr_type = p_str_cntr_type;
           
            ServiceObject.Add_To_proc_save_audit_trail(objInboundInquiry);
            //END
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return Json(ResultCount, JsonRequestBehavior.AllowGet);
            //if (objInboundInquiry.SucessStatus == "0")
            //{
            // return PartialView("~/Views/InboundInquiry/InboundInquiry.cshtml", InboundInquiryModel);
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
                objInboundInquiry.factory_id = objInboundInquiry.ListDocdtl[i].factory_id;
                objInboundInquiry.cust_name = objInboundInquiry.ListDocdtl[i].cust_name;
                objInboundInquiry.cust_po_num = objInboundInquiry.ListDocdtl[i].cust_po_num;
                objInboundInquiry.pick_list = objInboundInquiry.ListDocdtl[i].pick_list;
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
                            objInboundInquiry.factory_id = string.Empty;
                            objInboundInquiry.cust_name = string.Empty;
                            objInboundInquiry.cust_po_num = string.Empty;
                            objInboundInquiry.pick_list = string.Empty;
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
            string l_str_config_file_path = string.Empty;
            string l_str_folder_full_path = string.Empty;
            string l_str_sub_folder = string.Empty;
            string l_str_folder_path = string.Empty;
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.ib_doc_id = p_str_ibdocid;

            l_str_config_file_path = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
            l_str_sub_folder = p_str_ibdocid.Substring(0, 3);
            l_str_folder_path = p_str_cmp_id.Trim() + "\\" + "INBOUND" + "\\" + p_str_ibdocid.Trim() + "\\" + l_str_sub_folder;

            l_str_folder_full_path = Path.Combine(l_str_config_file_path, l_str_folder_path);
            ServiceObject.DeleteDocEntry(objInboundInquiry);
            if (Directory.Exists(l_str_folder_full_path))
            {
                System.IO.Directory.Delete(l_str_folder_full_path);
            }

            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            int resultcount;
            resultcount = 1;
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
            int l_int_TotCtn = 0;
            decimal l_dec_Totwgt = 0;
            decimal l_dec_Totcube = 0;
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
                        strReportName = "rpt_ib_doc_entry_ack.rpt";
                        InboundInquiry objInboundInquiry = new InboundInquiry();
                        InboundInquiryService ServiceObject = new InboundInquiryService();
                        
                        objInboundInquiry.cmp_id = cmp_id;
                        objInboundInquiry.ib_doc_id = ib_doc_id;
                        objInboundInquiry = ServiceObject.GET_IB_DOC_CUBE_AND_WGT(objInboundInquiry);
                        if (objInboundInquiry.ListTotalCount.Count > 0)
                        {
                            l_int_TotCtn = objInboundInquiry.ListTotalCount[0].TOT_CARTON;
                            l_dec_Totwgt = objInboundInquiry.ListTotalCount[0].TOT_WEIGHT;
                            l_dec_Totcube = objInboundInquiry.ListTotalCount[0].TOTCUBE;
                        }
               
                        objInboundInquiry = ServiceObject.GetInboundAckRptDetails(objInboundInquiry);
                        IList<InboundInquiry> rptSource = objInboundInquiry.ListAckRptDetails.ToList();

                        if (rptSource.Count > 0)
                        {
                           
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                            using (ReportDocument rd = new ReportDocument())
                            { 

                                rd.Load(strRptPath);
                                rd.SetDataSource(rptSource);
                                objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); 
                                rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                rd.SetParameterValue("TotCtn", l_int_TotCtn);
                                rd.SetParameterValue("TotWgt", l_dec_Totwgt);
                                rd.SetParameterValue("TotCube", l_dec_Totcube);
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            }
                        }
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

                            if (objInboundInquiry.ListGETRateID[0].RATEID.Trim() == "CNTR")     
                            {
                                objInboundInquiry.cmp_id = p_str_cmpid;
                                objInboundInquiry.ib_doc_id = ib_doc_id;
                                objInboundInquiry = ServiceObject.GetInboundConfirmationRptDetailsbyContainer(objInboundInquiry);
                                objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim();
                                IList<InboundInquiry> rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();
                            
                                if (rptSource.Count > 0)
                                { 
                                    using (ReportDocument rd = new ReportDocument())
                                    {
                                        strReportName = "rpt_ib_doc_recv_post_confrimation_by_container.rpt";
                                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                        rd.Load(strRptPath);
                                        rd.SetDataSource(rptSource);
                                        rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                        rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                    }
                                }
                            }
                            else
                            {
                               
                                objInboundInquiry.cmp_id = cmp_id;
                                objInboundInquiry.ib_doc_id = ib_doc_id;
                                objInboundInquiry = ServiceObject.GET_IB_RCVD_DOC_CUBE_AND_WGT(objInboundInquiry);
                                if (objInboundInquiry.ListTotalCount.Count > 0)
                                {
                                    l_int_TotCtn = objInboundInquiry.ListTotalCount[0].TOT_CARTON;
                                    l_dec_Totwgt = objInboundInquiry.ListTotalCount[0].TOT_WEIGHT;
                                    l_dec_Totcube = objInboundInquiry.ListTotalCount[0].TOTCUBE;
                                }
 
                                objInboundInquiry = ServiceObject.GetInboundConfirmationRptDetails(objInboundInquiry);
                                IList<InboundInquiry> rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();
                                if (rptSource.Count> 0)
                                {
                                    using (ReportDocument rd = new ReportDocument())
                                    { 

                                        strReportName = "rpt_ib_doc_recv_post_confrimation.rpt";
                                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                        rd.Load(strRptPath);
                                        rd.SetDataSource(rptSource);
                                        objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim();  
                                        rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                        rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);    
                                        rd.SetParameterValue("TotCtn", l_int_TotCtn);
                                        rd.SetParameterValue("TotWgt", l_dec_Totwgt);
                                        rd.SetParameterValue("TotCube", l_dec_Totcube);
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                    }
                                }
                            }
                        }
                        else
                        {
                            objInboundInquiry.cmp_id = cmp_id;
                            objInboundInquiry.ib_doc_id = ib_doc_id;
                            objInboundInquiry = ServiceObject.GET_IB_RCVD_DOC_CUBE_AND_WGT(objInboundInquiry);
                            if (objInboundInquiry.ListTotalCount.Count > 0)
                            {
                                l_int_TotCtn = objInboundInquiry.ListTotalCount[0].TOT_CARTON;
                                l_dec_Totwgt = objInboundInquiry.ListTotalCount[0].TOT_WEIGHT;
                                l_dec_Totcube = objInboundInquiry.ListTotalCount[0].TOTCUBE;
                            }
                            objInboundInquiry = ServiceObject.GetInboundConfirmationRptDetails(objInboundInquiry);
                            IList<InboundInquiry> rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();

                        if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    strReportName = "rpt_ib_doc_recv_post_confrimation.rpt";
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                    rd.Load(strRptPath);
                                    rd.SetDataSource(rptSource);
                                    objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); 
                                    rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                    rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                    rd.SetParameterValue("TotCtn", l_int_TotCtn);
                                    rd.SetParameterValue("TotWgt", l_dec_Totwgt);
                                    rd.SetParameterValue("TotCube", l_dec_Totcube);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }


                    }
                    else if (l_str_status == "1-RCVD")
                    {
                        strReportName = "rpt_ib_doc_recv_tallysheet.rpt";

                        InboundInquiry objInboundInquiry = new InboundInquiry();
                        InboundInquiryService ServiceObject = new InboundInquiryService();
                       

                        objInboundInquiry.cmp_id = cmp_id;
                        objInboundInquiry.ib_doc_id = ib_doc_id;
                        objInboundInquiry = ServiceObject.GET_IB_RCVD_DOC_CUBE_AND_WGT(objInboundInquiry);
                        if (objInboundInquiry.ListTotalCount.Count > 0)
                        {
                            l_int_TotCtn = objInboundInquiry.ListTotalCount[0].TOT_CARTON;
                            l_dec_Totwgt = objInboundInquiry.ListTotalCount[0].TOT_WEIGHT;
                            l_dec_Totcube = objInboundInquiry.ListTotalCount[0].TOTCUBE;
                        }
    
                        objInboundInquiry = ServiceObject.GetInboundTallySheetRptDetails(objInboundInquiry);
                        IList<InboundInquiry> rptSource = objInboundInquiry.ListTallySheetRptDetails.ToList();

                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            { 
                                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                rd.Load(strRptPath);
                                rd.SetDataSource(rptSource);
                                objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); 
                                rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);    
                                rd.SetParameterValue("TotCtn", l_int_TotCtn);
                                rd.SetParameterValue("TotWgt", l_dec_Totwgt);
                                rd.SetParameterValue("TotCube", l_dec_Totcube);
                                DataTable dtIBDocValidateItem = ServiceObject.GetIBCheckIbDocRcvdCubeCheck(p_str_cmpid, p_str_ib_doc_id);
                                if (dtIBDocValidateItem.Rows.Count > 0)
                                {
                                    if ((dtIBDocValidateItem.Rows[0]["dup_itm_count"].ToString() == string.Empty ) || (dtIBDocValidateItem.Rows[0]["dup_itm_count"].ToString() == "0"))
                                    {
                                        rd.SetParameterValue("fml_duplicate_item", "NO Duplicate");
                                    }
                                    else
                                    {
                                        rd.SetParameterValue("fml_duplicate_item", "DUPLICATE ITEMS RECEIVED");
                                    }

                                    if ((dtIBDocValidateItem.Rows[0]["less_cube_count"].ToString() == string.Empty) || (dtIBDocValidateItem.Rows[0]["less_cube_count"].ToString() == "0"))
                                    {
                                        rd.SetParameterValue("fml_cube_size_issue", "OK");
                                    }
                                    else
                                    {
                                        rd.SetParameterValue("fml_cube_size_issue", "CUBE MISSING!’  ‘MUST GET / MEASURE CUBE and ENTER on RECEIVING!");
                                    }

                                    if (dtIBDocValidateItem.Rows[0]["cntr_cube_mismatch"].ToString() == "N")
                                    {
                                        rd.SetParameterValue("fml_cntr_cube_mismatch", "OK");
                                    }
                                    else if (dtIBDocValidateItem.Rows[0]["cntr_cube_mismatch"].ToString() == "L")
                                    {
                                        rd.SetParameterValue("fml_cntr_cube_mismatch", "RECEIVED CUBE LESS THAN CONTAINER SIZE");
                                    }
                                    else if (dtIBDocValidateItem.Rows[0]["cntr_cube_mismatch"].ToString() == "H")
                                    {
                                        rd.SetParameterValue("fml_cntr_cube_mismatch", "RECEIVED CUBE GREATER THAN CONTAINER SIZE");
                                    }

                                }
                                else
                                {
                                    rd.SetParameterValue("fml_duplicate_item", string.Empty);
                                    rd.SetParameterValue("fml_cube_size_issue", string.Empty);
                                    rd.SetParameterValue("fml_cntr_cube_mismatch", string.Empty);
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
            Folderpath = System.Configuration.ConfigurationManager.AppSettings["tempFilepath"].ToString().Trim();
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
            objCustMaster.cust_id = p_str_cmp_id;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }

            if (isValid)
            {


                if (l_str_rpt_selection == "GridSummary")
                {
                   
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

                 

                    if (type == "PDF")
                    {
                        using (ReportDocument rd = new ReportDocument())
                        { 
                            strReportName = "rpt_inbound_grid_summary.rpt";
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                            IList<InboundInquiry> rptSource = objInboundInquiry.ListGridSummaryRptDetails.ToList();
                            rd.Load(strRptPath);
                            rd.SetDataSource(rptSource);
                            rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                            Random filerand = new Random();
                            int iyear = DateTime.Now.Year;
                            iyear = filerand.Next(1000000, 9999999);
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                            objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); 
                            rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);

                        }
                        reportFileName = l_str_rptdtl + DateTime.Now.ToFileTime() + ".pdf";
                        Session["RptFileName"] = strFileName;

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
            int l_int_TotCtn = 0;
            decimal l_dec_Totwgt = 0;
            decimal l_dec_Totcube = 0;
            Folderpath = System.Configuration.ConfigurationManager.AppSettings["tempFilepath"].ToString().Trim();
            //CR - 3PL_MVC_IB_2018_0219_008
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cmp_id = p_str_cmpid;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            objInbound.LstCmpName = objCompany.LstCmpName;
            l_str_tmp_name = objInbound.LstCmpName[0].cmp_name.ToString().Trim();
            objCustMaster.cust_id = p_str_cmpid;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
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

                    if (l_str_rpt_selection == "Acknowledgement")
                    {
                       
                        InboundInquiry objInboundInquiry = new InboundInquiry();
                        InboundInquiryService ServiceObject = new InboundInquiryService();

                        objInboundInquiry.cmp_id = p_str_cmpid;
                        objInboundInquiry.ib_doc_id = SelectdID;

                        objInboundInquiry = ServiceObject.GET_IB_DOC_CUBE_AND_WGT(objInboundInquiry);
                        if (objInboundInquiry.ListTotalCount.Count > 0)
                        {
                            l_int_TotCtn = objInboundInquiry.ListTotalCount[0].TOT_CARTON;
                            l_dec_Totwgt = objInboundInquiry.ListTotalCount[0].TOT_WEIGHT;
                            l_dec_Totcube = objInboundInquiry.ListTotalCount[0].TOTCUBE;
                        }
    
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
                                l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB ACK" + "_" + "IB DOC#" + "_" + objInboundInquiry.ibdocid;
                                objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + " " + " " + "IB ACK" + "|" + " " + " " + "IB DOC#: " + " " + objInboundInquiry.ibdocid + "|" + " " + " " + "IB Date: " + objInboundInquiry.ib_doc_dt;
                                objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + " " + " " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + " " + objInboundInquiry.ib_doc_dt;

                            }
                            else if ((objInboundInquiry.eta_dt == "" || objInboundInquiry.eta_dt == null || objInboundInquiry.eta_dt == "-") && (objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                            {
                                l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB ACK" + "_" + "IB DOC#" + "_" + objInboundInquiry.ibdocid;
                                objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + " " + " " + "IB ACK" + "|" + " " + " " + "IB DOC#: " + objInboundInquiry.ibdocid + "|" + " " + " " + "IB_Dt: " + objInboundInquiry.ib_doc_dt + "|" + " " + " " + "CNTR#: " + objInboundInquiry.cntr_id;
                                objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + " " + " " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + " " + " " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#: " + " " + " " + objInboundInquiry.cntr_id;

                            }
                            else if ((objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                            {
                                l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB ACK" + "_" + "IB DOC#" + "_" + objInboundInquiry.ibdocid;
                                objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + " " + " " + "IB ACK" + "|" + " " + " " + "IB DOC#: " + objInboundInquiry.ibdocid + "|" + " " + " " + "IB_Dt: " + objInboundInquiry.ib_doc_dt + "|" + " " + " " + "CNTR#: " + objInboundInquiry.cntr_id;
                                objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "ETA Date:" + " " + " " + objInboundInquiry.eta_dt;

                            }
                            else if ((objInboundInquiry.req_num == "" || objInboundInquiry.req_num == null || objInboundInquiry.req_num == "-"))
                            {
                                l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB ACK" + "_" + "IB DOC#" + "_" + objInboundInquiry.ibdocid;
                                objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + " " + " " + "IB ACK" + "|" + " " + " " + "IB DOC#: " + objInboundInquiry.ibdocid + "|" + " " + " " + "IB_Dt: " + objInboundInquiry.ib_doc_dt + "|" + " " + " " + "CNTR#: " + objInboundInquiry.cntr_id;
                                objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + " " + " " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + " " + " " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#: " + " " + " " + objInboundInquiry.cntr_id + "\n" + "Received From: " + " " + " " + objInboundInquiry.recvd_fm + "\n" + "ETA Date: " + " " + " " + objInboundInquiry.eta_dt;


                            }
                            else
                            {
                                l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB ACK" + "_" + "IB DOC#" + "_" + objInboundInquiry.ibdocid;
                                objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "IB ACK" + "|" + " " + "IB DOC#: " + objInboundInquiry.ibdocid + "|" + " " + "IB_Dt: " + objInboundInquiry.ib_doc_dt + "|" + " " + "CNTR#: " + objInboundInquiry.cntr_id;
                                objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + " " + " " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + " " + " " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#: " + " " + " " + objInboundInquiry.cntr_id + "\n" + "Ref#: " + " " + " " + objInboundInquiry.req_num + "\n" + "Received From: " + " " + " " + objInboundInquiry.recvd_fm + "\n" + "ETA Date: " + " " + " " + objInboundInquiry.eta_dt;
                            }
                        }

                        IList<InboundInquiry> rptSource = objInboundInquiry.ListAckRptDetails.ToList();

                       
                       
                        if (type == "PDF")
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                strReportName = "rpt_ib_doc_entry_ack.rpt";
                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                rd.Load(strRptPath);
                                rd.SetDataSource(rptSource);
                                rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                rd.SetParameterValue("TotCtn", l_int_TotCtn);
                                rd.SetParameterValue("TotWgt", l_dec_Totwgt);
                                rd.SetParameterValue("TotCube", l_dec_Totcube);
                                objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo;
                                rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                reportFileName = l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                Session["RptFileName"] = strFileName;
                            }
                        }

                    }
                    if (l_str_rpt_selection == "TallySheet")
                    {

                        if (l_str_status == "POST")
                        {
                           
                            InboundInquiry objInboundInquiry = new InboundInquiry();
                            InboundInquiryService ServiceObject = new InboundInquiryService();

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
                            objInboundInquiry.cmp_id = p_str_cmpid;
                            objInboundInquiry.ib_doc_id = SelectdID;
                            //CR - 3PL_MVC_IB_2018_0910_001
                            objInboundInquiry = ServiceObject.GET_IB_RCVD_DOC_CUBE_AND_WGT(objInboundInquiry);
                            if (objInboundInquiry.ListTotalCount.Count > 0)
                            {
                                l_int_TotCtn = objInboundInquiry.ListTotalCount[0].TOT_CARTON;
                                l_dec_Totwgt = objInboundInquiry.ListTotalCount[0].TOT_WEIGHT;
                                l_dec_Totcube = objInboundInquiry.ListTotalCount[0].TOTCUBE;
                            }
                            //end  

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
                                    l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB TALLYSHEET" + "_" + "IB DOC#" + "_" + objInboundInquiry.ibdocid;
                                    objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + " " + " " + "IB TALLYSHEET" + "|" + " " + " " + "IB#: " + " " + objInboundInquiry.ibdocid + "|" + " " + " " + "IB Date: " + objInboundInquiry.ib_doc_dt;
                                    objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + " " + " " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + " " + objInboundInquiry.ib_doc_dt + "\n" + "Total Cartons Received: " + " " + " " + objInboundInquiry.tot_ctn + " " + "Ctns" + "\n" + "Total Cube: " + " " + " " + objInboundInquiry.tot_cube + " " + "Lbs";

                                }
                                else if ((objInboundInquiry.eta_dt == "" || objInboundInquiry.eta_dt == null || objInboundInquiry.eta_dt == "-") && (objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                                {
                                    l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB TALLYSHEET" + "_" + "IB DOC#" + "_" + objInboundInquiry.ibdocid;
                                    objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + " " + " " + "IB TALLYSHEET" + "|" + " " + " " + "IB#: " + objInboundInquiry.ibdocid + "|" + " " + " " + "IB_Dt: " + objInboundInquiry.ib_doc_dt + "|" + " " + " " + "CNTR#: " + objInboundInquiry.cntr_id;
                                    objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + " " + " " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + " " + " " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#: " + " " + " " + objInboundInquiry.cntr_id + "\n" + "Total Cartons Received: " + " " + " " + objInboundInquiry.tot_ctn + " " + "Ctns" + "\n" + "Total Cube: " + " " + " " + objInboundInquiry.tot_cube + " " + "Lbs";

                                }
                                else if ((objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                                {
                                    l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB TALLYSHEET" + "_" + "IB DOC#" + "_" + objInboundInquiry.ibdocid;
                                    objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + " " + " " + "IB TALLYSHEET" + "|" + " " + " " + "IB#: " + objInboundInquiry.ibdocid + "|" + " " + " " + "IB_Dt: " + objInboundInquiry.ib_doc_dt + "|" + " " + " " + "CNTR#: " + objInboundInquiry.cntr_id;
                                    objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "ETA Date:" + " " + " " + objInboundInquiry.eta_dt;

                                }
                                else if ((objInboundInquiry.req_num == "" || objInboundInquiry.req_num == null || objInboundInquiry.req_num == "-"))
                                {
                                    l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB TALLYSHEET" + "_" + "IB DOC#" + "_" + objInboundInquiry.ibdocid;
                                    objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + " " + " " + "IB TALLYSHEET" + "|" + " " + " " + "IB#: " + objInboundInquiry.ibdocid + "|" + " " + " " + "IB_Dt: " + objInboundInquiry.ib_doc_dt + "|" + " " + " " + "CNTR#: " + objInboundInquiry.cntr_id;
                                    objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + " " + " " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + " " + " " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#: " + " " + " " + objInboundInquiry.cntr_id + "\n" + "Received From: " + " " + " " + objInboundInquiry.recvd_fm + "\n" + "ETA Date: " + " " + " " + objInboundInquiry.eta_dt + "\n" + "Total Cartons Received: " + " " + " " + objInboundInquiry.tot_ctn + " " + "Ctns" + "\n" + "Total Cube: " + " " + " " + objInboundInquiry.tot_cube + " " + "Lbs";


                                }
                                else
                                {
                                    l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB TALLYSHEET" + "_" + "IB DOC#" + "_" + objInboundInquiry.ibdocid;
                                    objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "IB TALLYSHEET" + "|" + " " + "IB#: " + objInboundInquiry.ibdocid + "|" + " " + "IB_Dt: " + objInboundInquiry.ib_doc_dt + "|" + " " + "CNTR#: " + objInboundInquiry.cntr_id;
                                    objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + " " + " " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + " " + " " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#: " + " " + " " + objInboundInquiry.cntr_id + "\n" + "Ref#: " + " " + " " + objInboundInquiry.req_num + "\n" + "Received From: " + " " + " " + objInboundInquiry.recvd_fm + "\n" + "ETA Date: " + " " + " " + objInboundInquiry.eta_dt + "\n" + "Total Cartons Received: " + " " + " " + objInboundInquiry.tot_ctn + " " + "Ctns" + "\n" + "Total Cube: " + " " + " " + objInboundInquiry.tot_cube + " " + "Lbs";
                                }

                            }
                            IList<InboundInquiry> rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();
                            if (rptSource.Count > 0)
                            { 
                                using (ReportDocument rd = new ReportDocument())
                                { 
                                    strReportName = "rpt_ib_doc_recv_post_confrimation.rpt";
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                    rd.Load(strRptPath);
                                    rd.SetDataSource(rptSource);
                                    rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                                                                    
                                    rd.SetParameterValue("TotCtn", l_int_TotCtn);
                                    rd.SetParameterValue("TotWgt", l_dec_Totwgt);
                                    rd.SetParameterValue("TotCube", l_dec_Totcube);
                                                 
                                    if (type == "PDF")
                                    {
                                        objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo; 
                                        rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                        reportFileName = l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                        Session["RptFileName"] = strFileName;
                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.WordForWindows, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                    }
                                }
                            }

                        }
                        else if (l_str_status == "1-RCVD")
                        {
                            
                            InboundInquiry objInboundInquiry = new InboundInquiry();
                            InboundInquiryService ServiceObject = new InboundInquiryService();

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
                            objInboundInquiry.cmp_id = p_str_cmpid;
                            objInboundInquiry.ib_doc_id = SelectdID;
                            //CR - 3PL_MVC_IB_2018_0910_001
                            objInboundInquiry = ServiceObject.GET_IB_RCVD_DOC_CUBE_AND_WGT(objInboundInquiry);
                            if (objInboundInquiry.ListTotalCount.Count > 0)
                            {
                                l_int_TotCtn = objInboundInquiry.ListTotalCount[0].TOT_CARTON;
                                l_dec_Totwgt = objInboundInquiry.ListTotalCount[0].TOT_WEIGHT;
                                l_dec_Totcube = objInboundInquiry.ListTotalCount[0].TOTCUBE;
                            }
                            //end  
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
                                    l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB TALLYSHEET" + "_" + "IB DOC#" + "_" + objInboundInquiry.ibdocid;
                                    objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + " " + " " + "IB TALLYSHEET" + "|" + " " + " " + "IB#: " + " " + objInboundInquiry.ibdocid + "|" + " " + " " + "IB Date: " + objInboundInquiry.ib_doc_dt;
                                    objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + " " + " " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + " " + objInboundInquiry.ib_doc_dt + "\n" + "Total Cartons Received: " + " " + " " + objInboundInquiry.tot_ctn + " " + "Ctns" + "\n" + "Total Cube: " + " " + " " + objInboundInquiry.tot_cube + " " + "Lbs";

                                }
                                else if ((objInboundInquiry.eta_dt == "" || objInboundInquiry.eta_dt == null || objInboundInquiry.eta_dt == "-") && (objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                                {
                                    l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB TALLYSHEET" + "_" + "IB DOC#" + "_" + objInboundInquiry.ibdocid;
                                    objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + " " + " " + "IB TALLYSHEET" + "|" + " " + " " + "IB#: " + objInboundInquiry.ibdocid + "|" + " " + " " + "IB_Dt: " + objInboundInquiry.ib_doc_dt + "|" + " " + " " + "CNTR#: " + objInboundInquiry.cntr_id;
                                    objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + " " + " " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + " " + " " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#: " + " " + " " + objInboundInquiry.cntr_id + "\n" + "Total Cartons Received: " + " " + " " + objInboundInquiry.tot_ctn + " " + "Ctns" + "\n" + "Total Cube: " + " " + " " + objInboundInquiry.tot_cube + " " + "Lbs";

                                }
                                else if ((objInboundInquiry.recvd_fm == "" || objInboundInquiry.recvd_fm == null || objInboundInquiry.recvd_fm == "-") && (objInboundInquiry.req_num != "" || objInboundInquiry.req_num != null || objInboundInquiry.req_num != "-"))
                                {
                                    l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB TALLYSHEET" + "_" + "IB DOC#" + "_" + objInboundInquiry.ibdocid;
                                    objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + " " + " " + "IB TALLYSHEET" + "|" + " " + " " + "IB#: " + objInboundInquiry.ibdocid + "|" + " " + " " + "IB_Dt: " + objInboundInquiry.ib_doc_dt + "|" + " " + " " + "CNTR#: " + objInboundInquiry.cntr_id;
                                    objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#:" + objInboundInquiry.cntr_id + "\n" + "ETA Date:" + " " + " " + objInboundInquiry.eta_dt;

                                }
                                else if ((objInboundInquiry.req_num == "" || objInboundInquiry.req_num == null || objInboundInquiry.req_num == "-"))
                                {
                                    l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB TALLYSHEET" + "_" + "IB DOC#" + "_" + objInboundInquiry.ibdocid;
                                    objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + " " + " " + "IB TALLYSHEET" + "|" + " " + " " + "IB#: " + objInboundInquiry.ibdocid + "|" + " " + " " + "IB_Dt: " + objInboundInquiry.ib_doc_dt + "|" + " " + " " + "CNTR#: " + objInboundInquiry.cntr_id;
                                    objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + " " + " " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + " " + " " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#: " + " " + " " + objInboundInquiry.cntr_id + "\n" + "Received From: " + " " + " " + objInboundInquiry.recvd_fm + "\n" + "ETA Date: " + " " + " " + objInboundInquiry.eta_dt + "\n" + "Total Cartons Received: " + " " + " " + objInboundInquiry.tot_ctn + " " + "Ctns" + "\n" + "Total Cube: " + " " + " " + objInboundInquiry.tot_cube + " " + "Lbs";


                                }
                                else
                                {
                                    l_str_rptdtl = objInboundInquiry.cmp_id + "_" + "IB TALLYSHEET" + "_" + "IB DOC#" + "_" + objInboundInquiry.ibdocid;
                                    objEmail.EmailSubject = objInboundInquiry.cmp_id + "-" + "IB TALLYSHEET" + "|" + " " + "IB#: " + objInboundInquiry.ibdocid + "|" + " " + "IB_Dt: " + objInboundInquiry.ib_doc_dt + "|" + " " + "CNTR#: " + objInboundInquiry.cntr_id;
                                    objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + objInboundInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "IB Doc Id#: " + " " + " " + objInboundInquiry.ibdocid + "\n" + "IB Date: " + " " + " " + objInboundInquiry.ib_doc_dt + "\n" + "CNTR#: " + " " + " " + objInboundInquiry.cntr_id + "\n" + "Ref#: " + " " + " " + objInboundInquiry.req_num + "\n" + "Received From: " + " " + " " + objInboundInquiry.recvd_fm + "\n" + "ETA Date: " + " " + " " + objInboundInquiry.eta_dt + "\n" + "Total Cartons Received: " + " " + " " + objInboundInquiry.tot_ctn + " " + "Ctns" + "\n" + "Total Cube: " + " " + " " + objInboundInquiry.tot_cube + " " + "Lbs";
                                }
                            }
                            var rptSource = objInboundInquiry.ListTallySheetRptDetails.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                { 
                                    strReportName = "rpt_ib_doc_recv_tallysheet.rpt";
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                    rd.Load(strRptPath);
                                     rd.SetDataSource(rptSource);
                                    rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                    rd.SetParameterValue("TotCtn", l_int_TotCtn);
                                    rd.SetParameterValue("TotWgt", l_dec_Totwgt);
                                    rd.SetParameterValue("TotCube", l_dec_Totcube);
                     
                                    rd.SetParameterValue("fml_duplicate_item", string.Empty);
                                    rd.SetParameterValue("fml_cube_size_issue", string.Empty);
                                    rd.SetParameterValue("fml_cntr_cube_mismatch", string.Empty);

                                    if (type == "PDF")
                                    {
                                        objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo; //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                        reportFileName = l_str_rptdtl + strDateFormat + ".pdf";
                                        Session["RptFileName"] = strFileName;
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
                            }
                        }

                    }

                    if (l_str_rpt_selection == "WorkSheet")
                    {
                        
                        InboundInquiry objInboundInquiry = new InboundInquiry();
                        InboundInquiryService ServiceObject = new InboundInquiryService();
                        
                        objInboundInquiry.cmp_id = p_str_cmpid;
                        objInboundInquiry.ib_doc_id = SelectdID;
                        //CR - 3PL_MVC_IB_2018_0910_001
                        objInboundInquiry = ServiceObject.GET_IB_DOC_CUBE_AND_WGT(objInboundInquiry);
                        if (objInboundInquiry.ListTotalCount.Count > 0)
                        {
                            l_int_TotCtn = objInboundInquiry.ListTotalCount[0].TOT_CARTON;
                            l_dec_Totwgt = objInboundInquiry.ListTotalCount[0].TOT_WEIGHT;
                            l_dec_Totcube = objInboundInquiry.ListTotalCount[0].TOTCUBE;
                        }
                        //end  
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
                        IList<InboundInquiry> rptSource = objInboundInquiry.ListWorkSheetRptDetails.ToList();
                        if(rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            { 
                                strReportName = "rpt_ib_doc_entry_recv_worksheet.rpt";
                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                rd.Load(strRptPath);
                                rd.SetDataSource(rptSource);
                                rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name); 
                                rd.SetParameterValue("TotCtn", l_int_TotCtn);
                                rd.SetParameterValue("TotWgt", l_dec_Totwgt);
                                rd.SetParameterValue("TotCube", l_dec_Totcube);
                                objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                if (type == "PDF")
                                {
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                    reportFileName = l_str_rptdtl + strDateFormat + ".pdf";
                                    Session["RptFileName"] = strFileName;

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

                                if (objInboundInquiry.ListGETRateID[0].RATEID.Trim() == "CNTR")      
                                {
                                    
                                    
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

                                    

                                    if (type == "PDF")
                                    {
                                        IList<InboundInquiry> rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();
                                        if (rptSource.Count > 0)
                                        {
                                            using (ReportDocument rd = new ReportDocument())
                                            { 
                                                strReportName = "rpt_ib_doc_recv_post_confrimation_by_container.rpt";
                                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                                rd.Load(strRptPath);
                                                rd.SetDataSource(rptSource);
                                                rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                                objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                                rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                                reportFileName = l_str_rptdtl + strDateFormat + ".pdf";
                                                Session["RptFileName"] = strFileName;
                                            }
                                        }
                                    }

                                    else if (type == "Excel")
                                    {
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
                                    objInboundInquiry.cmp_id = p_str_cmpid;
                                    objInboundInquiry.ib_doc_id = SelectdID;
                                    objInboundInquiry = ServiceObject.GET_IB_RCVD_DOC_CUBE_AND_WGT(objInboundInquiry);
                                    if (objInboundInquiry.ListTotalCount.Count > 0)
                                    {
                                        l_int_TotCtn = objInboundInquiry.ListTotalCount[0].TOT_CARTON;
                                        l_dec_Totwgt = objInboundInquiry.ListTotalCount[0].TOT_WEIGHT;
                                        l_dec_Totcube = objInboundInquiry.ListTotalCount[0].TOTCUBE;
                                    }
                                    //end  
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
                                    IList<InboundInquiry> rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();
                                    if (rptSource.Count > 0)
                                    {
                                        using (ReportDocument rd = new ReportDocument())
                                        { 
                                            strReportName = "rpt_ib_doc_recv_post_confrimation.rpt";
                                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                            rd.Load(strRptPath);
                                            rd.SetDataSource(rptSource);
                                            rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                                            rd.SetParameterValue("TotCtn", l_int_TotCtn);
                                            rd.SetParameterValue("TotWgt", l_dec_Totwgt);
                                            rd.SetParameterValue("TotCube", l_dec_Totcube);
                                            objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                            rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                                              
                                            if (type == "PDF")
                                            {
                                               strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                                reportFileName = l_str_rptdtl + strDateFormat + ".pdf";
                                                Session["RptFileName"] = strFileName;
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
                                    }
                                }
                            }
                            else
                            {
                                objInboundInquiry.cmp_id = p_str_cmpid;
                                objInboundInquiry.ib_doc_id = SelectdID;
                                

                              
                                objInboundInquiry.cmp_id = p_str_cmpid;
                                objInboundInquiry.ib_doc_id = SelectdID;
                                //CR - 3PL_MVC_IB_2018_0910_001
                                objInboundInquiry = ServiceObject.GET_IB_RCVD_DOC_CUBE_AND_WGT(objInboundInquiry);
                                if (objInboundInquiry.ListTotalCount.Count > 0)
                                {
                                    l_int_TotCtn = objInboundInquiry.ListTotalCount[0].TOT_CARTON;
                                    l_dec_Totwgt = objInboundInquiry.ListTotalCount[0].TOT_WEIGHT;
                                    l_dec_Totcube = objInboundInquiry.ListTotalCount[0].TOTCUBE;
                                }
                                //end  
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
                                IList<InboundInquiry> rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();
                                if(rptSource.Count > 0)
                                {
                                    using (ReportDocument rd = new ReportDocument())
                                    { 
                                        strReportName = "rpt_ib_doc_recv_post_confrimation.rpt";
                                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                        rd.Load(strRptPath);
                                        rd.SetDataSource(rptSource);
                                        rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);//CR - 3PL_MVC_IB_2018_0219_008
                                        //CR - 3PL_MVC_IB_2018_0910_001
                                        rd.SetParameterValue("TotCtn", l_int_TotCtn);
                                        rd.SetParameterValue("TotWgt", l_dec_Totwgt);
                                        rd.SetParameterValue("TotCube", l_dec_Totcube);
                                            //end              
                                            objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                            rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                            if (type == "PDF")
                                            {
                               
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
                                                rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);

                                            }
                                    }
                                }
                            }
                        }
                    }
                    if (l_str_rpt_selection == "GridSummary")
                    {
                        
                        InboundInquiry objInboundInquiry = new InboundInquiry();
                        InboundInquiryService ServiceObject = new InboundInquiryService();


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
                        IList<InboundInquiry> rptSource = objInboundInquiry.ListGridSummaryRptDetails.ToList();
                        if(rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            { 
                                strReportName = "rpt_inbound_grid_summary.rpt";
                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                rd.Load(strRptPath);
                        
                                rd.SetDataSource(rptSource);
                                rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);//CR - 3PL_MVC_IB_2018_0219_008
                                objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                if (type == "PDF")
                                {
                         
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
            //CR20180904
            objInboundInquiry = ServiceObject.LoadCustConfig(objInboundInquiry);
            if (objInboundInquiry.ListCustConfigDetails.Count() != 0)
            {
                objInboundInquiry.Allow_New_item = objInboundInquiry.ListCustConfigDetails[0].Allow_New_item;
                objInboundInquiry.Recv_non_doc_itm = objInboundInquiry.ListCustConfigDetails[0].Recv_non_doc_itm;
                objInboundInquiry.Recv_Itm_Mode = objInboundInquiry.ListCustConfigDetails[0].Recv_Itm_Mode;
                objInboundInquiry.aloc_by = objInboundInquiry.ListCustConfigDetails[0].aloc_by;
            }
            //END
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
            objInboundInquiry = ServiceObject.GetRcvngHdr(objInboundInquiry);
            l_str_rcd_dt = Convert.ToDateTime(objInboundInquiry.ListRcvgHdr[0].ib_doc_dt);
            objInboundInquiry.rcvd_dt = l_str_rcd_dt.ToString("MM/dd/yyyy");
            objInboundInquiry.vend_id = objInboundInquiry.ListRcvgHdr[0].shipvia_id;
            objInboundInquiry.refno = objInboundInquiry.ListRcvgHdr[0].req_num;
            objInboundInquiry.recvdvia = objInboundInquiry.ListRcvgHdr[0].vend_id;
            objInboundInquiry.rcvd_from = objInboundInquiry.ListRcvgHdr[0].vend_id;
            objInboundInquiry.ib_doc_dt = l_str_rcd_dt.ToString("MM/dd/yyyy");

            objInboundInquiry = ServiceObject.Getlotdtltext(objInboundInquiry);
            objInboundInquiry.cmp_id = objInboundInquiry.ListDocHdr[0].cmp_id;
       
            l_str_locId = objInboundInquiry.loc_id;
            objInboundInquiry.cont_id = objInboundInquiry.ListDocHdr[0].cont_id;
            objInboundInquiry.pkg_type = objInboundInquiry.ListDocHdr[0].pkg_type;
            objInboundInquiry.rate_id = objInboundInquiry.ListDocHdr[0].rate_id;
            objInboundInquiry.notes = objInboundInquiry.ListDocHdr[0].notes;
            //l_str_rcd_dt = Convert.ToDateTime(objInboundInquiry.ListDocHdr[0].palet_dt);
            //objInboundInquiry.ib_doc_dt = l_str_rcd_dt.ToString("MM/dd/yyyy");
            objCompany = ServiceObjectCompany.GetLocIdDetails(objCompany);
            objInboundInquiry.ListLocPickDtl = objCompany.ListLocPickDtl;
            objCompany = ServiceObjectCompany.GetCustConfigDtls(objCompany);
            objInboundInquiry.ListGetCustConfigDtls = objCompany.ListGetCustConfigDtls;
            objInboundInquiry.bill_type = objInboundInquiry.ListGetCustConfigDtls[0].bill_type;
            objInboundInquiry.bill_inout_type = objInboundInquiry.ListGetCustConfigDtls[0].bill_inout_type;
            objCompany.cust_cmp_id = CmpId;
            objCompany.whs_id = objInboundInquiry.whs_id;//CR20180807-001 Added By Nithya
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
              
                LookUp objLookUp = new LookUp();
                LookUpService ServiceObject1 = new LookUpService();
                objLookUp.id = "103";
                objLookUp.lookuptype = "CNTR_SIZE";
                objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                objInboundInquiry.ListContainerType = objLookUp.ListLookUpDtl;
                objInboundInquiry.cntr_type = objInboundInquiry.ListDocHdr[0].cntr_type;
                if (objInboundInquiry.ListDocHdr[0].ib_load_dt != null)
                {
                    objInboundInquiry.ib_load_dt = Convert.ToDateTime(objInboundInquiry.ListDocHdr[0].ib_load_dt).ToString("MM/dd/yyyy");
                }
              
                l_str_rcd_dt = Convert.ToDateTime(objInboundInquiry.ListDocHdr[0].rcvd_dt);
                objInboundInquiry.rcvd_dt = l_str_rcd_dt.ToString("MM/dd/yyyy");
            }
           
            objInboundInquiry = ServiceObject.GetGridlotdtl(objInboundInquiry);
            objInboundInquiry = ServiceObject.InsertTblIbDocRecvDtlTemp(objInboundInquiry);
            objInboundInquiry = ServiceObject.GetRecvdtlGrid(objInboundInquiry);
            objInboundInquiry = ServiceObject.GetRcvdEntryCountDtl(objInboundInquiry);
            objInboundInquiry.recvcount = objInboundInquiry.LstRcvdEntryCountDtl[0].recvcount;
            //CR20180904
            objInboundInquiry.dtl_line = objInboundInquiry.recvcount + 1;
            //END
            objInboundInquiry.View_Flag = "M";

           


            DataTable dtIBDocValidateItem = ServiceObject.GetIBCheckIbDocRcvdCubeCheck(CmpId, id);
            if (dtIBDocValidateItem.Rows.Count > 0)
            {
                if (dtIBDocValidateItem.Rows[0]["dup_itm_count"].ToString() == string.Empty)
                {
                    objInboundInquiry.dup_itm_count = 0;
                }
                else
                {
                    objInboundInquiry.dup_itm_count = Convert.ToInt64(dtIBDocValidateItem.Rows[0]["dup_itm_count"].ToString());
                }

                if (dtIBDocValidateItem.Rows[0]["less_cube_count"].ToString() == string.Empty)
                {
                    objInboundInquiry.less_cube_count = 0;
                }
                else
                {
                    objInboundInquiry.less_cube_count = Convert.ToInt64(dtIBDocValidateItem.Rows[0]["less_cube_count"].ToString());
                }


            }
            else
            {
                objInboundInquiry.dup_itm_count = 0;
                objInboundInquiry.less_cube_count = 0;
            }


            objInboundInquiry.cmp_id = CmpId;

            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_ReceivingEntry", InboundInquiryModel);
        }
        public ActionResult ReceivingEntryEditSave(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_rcvd_dt, string p_str_rcvd_from, string p_str_refno,
       string p_str_vend_id, string p_str_whs_id, string p_str_cont_id, string p_str_seal_num, string p_str_palet_id, string p_str_lot_id,
       string p_str_loc_id, string p_str_cntr_type, string p_str_ib_load_dt)
        {
            string l_str_lot_id = string.Empty;
            string l_str_rcvd_itm_mode = string.Empty;
            string l_str_transtatus = string.Empty;
            string l_str_locId = string.Empty;
            string l_str_Kit_type = string.Empty;
            DateTime l_dt_IbdocDt;
            DateTime l_dt_rcvddocDt;
            int l_str_KitQty = 0;
            decimal StRate = 0;
            decimal IORate = 0;
            int ResultCount = 0;
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
            //CR20180910
            l_dt_rcvddocDt = Convert.ToDateTime(p_str_rcvd_dt);
            objInboundInquiry = ServiceObject.CHECKDOCDATE(objInboundInquiry);
            if (objInboundInquiry.ListTotalCount.Count > 0)
            {
                objInboundInquiry.ib_doc_dt = objInboundInquiry.ListTotalCount[0].ib_doc_dt;
            }
            l_dt_IbdocDt = Convert.ToDateTime(objInboundInquiry.ib_doc_dt);
            if (l_dt_IbdocDt <= l_dt_rcvddocDt)
            {
                objInboundInquiry.ib_doc_dt = p_str_rcvd_dt;
            }
            else
            {
                ResultCount = 2;
                return Json(ResultCount, JsonRequestBehavior.AllowGet);
            }
            //END
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
                objInboundInquiry.factory_id = objInboundInquiry.ListRcvgDtl[j].factory_id;
                objInboundInquiry.cust_name = objInboundInquiry.ListRcvgDtl[j].cust_name;
                objInboundInquiry.cust_po_num = objInboundInquiry.ListRcvgDtl[j].cust_po_num;
                objInboundInquiry.pick_list = objInboundInquiry.ListRcvgDtl[j].pick_list;
                objInboundInquiry.length = objInboundInquiry.ListRcvgDtl[j].length;
                objInboundInquiry.width = objInboundInquiry.ListRcvgDtl[j].width;
                objInboundInquiry.depth = objInboundInquiry.ListRcvgDtl[j].depth;
                objInboundInquiry.wgt = objInboundInquiry.ListRcvgDtl[j].wgt;
                objInboundInquiry.cube = objInboundInquiry.ListRcvgDtl[j].cube;
                objInboundInquiry.itm_name = objInboundInquiry.ListRcvgDtl[j].itm_name;
                objInboundInquiry.ppk = objInboundInquiry.ListRcvgDtl[j].ctn_qty;
                objInboundInquiry.ctns = objInboundInquiry.ListRcvgDtl[j].tot_ctn;
                objInboundInquiry.dtl_line = objInboundInquiry.ListRcvgDtl[j].dtl_line;
                objInboundInquiry.loc_id = objInboundInquiry.ListRcvgDtl[j].loc_id;//CR20180901-001
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
                ResultCount = 1;
            }

            bool bl_uploadCntr = ServiceObject.UpdateLotContainer(p_str_cmp_id, p_str_ib_doc_id, p_str_cont_id, p_str_cntr_type, p_str_rcvd_dt, p_str_ib_load_dt);
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return Json(ResultCount, JsonRequestBehavior.AllowGet);
            // return PartialView("~/Views/InboundInquiry/InboundInquiry.cshtml", InboundInquiryModel);
        }
        public JsonResult DocrecvEntryEditDisplayGridToTextbox(string P_Str_CmpId, string P_Str_itmNum, string P_Str_Color, string P_Str_Size, string P_str_ibdocId,
            int P_str_LineNum, string P_str_itmCode, string P_str_ctn_line)
        {
            int OrderQty = 0;
            int l_int_rcvd_qty = 0;
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

                objInboundInquiry.factory_id = (objInboundInquiry.ListDocRecvEntryDtl[0].factory_id == null || objInboundInquiry.ListDocRecvEntryDtl[0].factory_id.Trim() == string.Empty) ? string.Empty : objInboundInquiry.ListDocRecvEntryDtl[0].factory_id.Trim();
                objInboundInquiry.cust_name = (objInboundInquiry.ListDocRecvEntryDtl[0].cust_name == null || objInboundInquiry.ListDocRecvEntryDtl[0].cust_name.Trim() == string.Empty) ? string.Empty : objInboundInquiry.ListDocRecvEntryDtl[0].cust_name.Trim();
                objInboundInquiry.cust_po_num = (objInboundInquiry.ListDocRecvEntryDtl[0].cust_po_num == null || objInboundInquiry.ListDocRecvEntryDtl[0].cust_po_num.Trim() == string.Empty) ? string.Empty : objInboundInquiry.ListDocRecvEntryDtl[0].cust_po_num.Trim();
                objInboundInquiry.pick_list = (objInboundInquiry.ListDocRecvEntryDtl[0].pick_list == null || objInboundInquiry.ListDocRecvEntryDtl[0].pick_list.Trim() == string.Empty) ? string.Empty : objInboundInquiry.ListDocRecvEntryDtl[0].pick_list.Trim();

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
            objInboundInquiry.factory_id = objInboundInquiry.factory_id.Trim();
            objInboundInquiry.cust_name = objInboundInquiry.cust_name.Trim();
            objInboundInquiry.cust_po_num = objInboundInquiry.cust_po_num.Trim();
            objInboundInquiry.pick_list = objInboundInquiry.pick_list.Trim();
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
            objInboundInquiry = ServiceObject.GetRcvngHdr(objInboundInquiry);
            l_str_rcd_dt = Convert.ToDateTime(objInboundInquiry.ListRcvgHdr[0].ib_doc_dt);
            objInboundInquiry.ib_doc_dt = l_str_rcd_dt.ToString("MM/dd/yyyy");
            objInboundInquiry.vend_id = objInboundInquiry.ListRcvgHdr[0].shipvia_id;
            objInboundInquiry.refno = objInboundInquiry.ListRcvgHdr[0].req_num;
            objInboundInquiry.recvdvia = objInboundInquiry.ListRcvgHdr[0].vend_id;
            objInboundInquiry.rcvd_from = objInboundInquiry.ListRcvgHdr[0].vend_id;
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
            //l_str_rcd_dt = Convert.ToDateTime(objInboundInquiry.ListDocHdr[0].palet_dt);
            //objInboundInquiry.ib_doc_dt = l_str_rcd_dt.ToString("MM/dd/yyyy");
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
            decimal p_str_height, decimal p_str_weight, decimal p_str_cube, int p_str_tot_qty, string p_str_strg_rate, string p_str_inout_rate, string p_str_loc_id,
            string p_str_lot_id, string p_str_palet_id, int p_int_recdqty, int RcvdQty, string p_str_factory_id, string p_str_cust_name, string p_str_cust_po_num, string p_str_pick_list)
        {
            int l_int_ordr_qty1;
            int l_int_ctn_qty1;
            int l_int_tmp_ordr_qty;
            int l_int_rcvd_qty = 0;
            string l_str_itmcode = string.Empty;
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
            objInboundInquiry.factory_id = p_str_factory_id;
            objInboundInquiry.cust_name = p_str_cust_name;
            objInboundInquiry.cust_po_num = p_str_cust_po_num;
            objInboundInquiry.pick_list = p_str_pick_list;
            objInboundInquiry.length = p_str_length;
            objInboundInquiry.width = p_str_width;
            objInboundInquiry.depth = p_str_height;
            objInboundInquiry.cube = p_str_cube;
            objInboundInquiry.tot_qty = p_str_tot_qty;
            objInboundInquiry.st_rate_id = p_str_strg_rate;
            objInboundInquiry.io_rate_id = p_str_inout_rate;
            objInboundInquiry.loc_id = p_str_loc_id;
            //CR20180829-001
            objInboundInquiry = ServiceObject.GetDftWhs(objInboundInquiry);
            string l_str_DftWhs = (objInboundInquiry.ListPickdtl[0].dft_whs == null || objInboundInquiry.ListPickdtl[0].dft_whs == "" ? string.Empty : objInboundInquiry.ListPickdtl[0].dft_whs);
            if (l_str_DftWhs != "" || l_str_DftWhs != null)
            {
                objInboundInquiry.whs_id = l_str_DftWhs;
            }
            else
            {
                l_str_DftWhs = "";
            }
            LocationMaster objLocationMaster = new LocationMaster();
            ILocationMasterService ObjService = new LocationMasterService();
            objLocationMaster.cmp_id = p_str_cmp_id;
            objLocationMaster.whs_id = l_str_DftWhs;
            objLocationMaster.loc_id = p_str_loc_id;
            objLocationMaster.option = "3";
            objLocationMaster = ObjService.CHECKLOCIDEXIST(objLocationMaster);
            if (objLocationMaster.ListLocationMasterDetails.Count == 0)
            {
                objLocationMaster.whs_id = l_str_DftWhs;
                objLocationMaster.loc_id = p_str_loc_id;
                objLocationMaster.option = "1";
                objLocationMaster.loc_desc = p_str_loc_id;
                objLocationMaster.status = "";
                objLocationMaster.note = "";
                objLocationMaster.length = p_str_length;
                objLocationMaster.width = p_str_width;
                objLocationMaster.depth = p_str_height;
                objLocationMaster.cube = p_str_cube;
                objLocationMaster.usage = "";
                objLocationMaster.process_id = "Add";
                objLocationMaster.loc_type = "BIN";
                objLocationMaster = ObjService.InsertLocationMasterDetails(objLocationMaster);
            }
            //END

            objInboundInquiry.lot_id = p_str_lot_id;
            objInboundInquiry.palet_id = p_str_palet_id;
            objInboundInquiry.itm_code = p_str_Itmcode;
            objInboundInquiry.wgt = p_str_weight;
            l_int_tmp_ordr_qty = p_str_tot_qty;
            l_int_ordr_qty1 = l_int_tmp_ordr_qty - ((l_int_tmp_ordr_qty) % (p_str_ppk));
            l_int_ctn_qty1 = (l_int_tmp_ordr_qty % (p_str_ppk));
            objInboundInquiry = ServiceObject.LoadCustConfig(objInboundInquiry);
            if (objInboundInquiry.ListCustConfigDetails.Count() != 0)
            {
                objInboundInquiry.Allow_New_item = objInboundInquiry.ListCustConfigDetails[0].Allow_New_item;
                objInboundInquiry.Recv_non_doc_itm = objInboundInquiry.ListCustConfigDetails[0].Recv_non_doc_itm;
                objInboundInquiry.aloc_by = objInboundInquiry.ListCustConfigDetails[0].aloc_by;

            }


            if (objInboundInquiry.Recv_non_doc_itm == "Y")
            {
                if (objInboundInquiry.Allow_New_item == "Y")
                {
                    //CR20180904-001 Added By Nithya
                    if (p_str_Itmcode == "")
                    {
                        objInboundInquiry.itm_num = p_str_style;
                        objInboundInquiry.itm_color = p_str_color;
                        objInboundInquiry.itm_size = p_str_size;
                        objInboundInquiry = ServiceObject.Validate_Itm(objInboundInquiry);
                        if (objInboundInquiry.LstItmExist.Count > 0)
                        {
                            objInboundInquiry.itm_code = objInboundInquiry.LstItmExist[0].itm_code;
                            p_str_Itmcode = objInboundInquiry.itm_code;
                        }
                    }
                    if (p_str_Itmcode == "")
                    {
                        objInboundInquiry.itm_num = p_str_style;
                        objInboundInquiry.itm_color = p_str_color;
                        objInboundInquiry.itm_size = p_str_size;
                        objInboundInquiry.itm_name = p_str_desc;
                        objInboundInquiry.ctn_qty = p_str_ppk; //p_str_ctns;//CR20180531-001 Added By Nithya
                        objInboundInquiry.length = p_str_length;
                        objInboundInquiry.width = p_str_width;
                        objInboundInquiry.height = p_str_height;
                        objInboundInquiry.weight = p_str_weight;
                        objInboundInquiry.cube = p_str_cube;
                        objInboundInquiry = ServiceObject.GetItmId(objInboundInquiry);
                        objInboundInquiry.itmid = objInboundInquiry.itm_code;
                        l_str_itmcode = objInboundInquiry.itm_code;
                        objInboundInquiry.itm_code = l_str_itmcode;
                        objInboundInquiry.flag = "A";
                        ServiceObject.Add_Style_To_Itm_dtl(objInboundInquiry);
                        ServiceObject.Add_Style_To_Itm_hdr(objInboundInquiry);
                    }
                    else
                    {
                        objInboundInquiry.itm_num = p_str_style;
                        objInboundInquiry.itm_color = p_str_color;
                        objInboundInquiry.itm_size = p_str_size;
                        objInboundInquiry.itm_name = p_str_desc;
                        objInboundInquiry.ctn_qty = p_str_ppk; //p_str_ctns;//CR20180531-001 Added By Nithya
                        objInboundInquiry.length = p_str_length;
                        objInboundInquiry.width = p_str_width;
                        objInboundInquiry.height = p_str_height;
                        objInboundInquiry.weight = p_str_weight;
                        objInboundInquiry.cube = p_str_cube;
                        objInboundInquiry.flag = "M";
                        objInboundInquiry.itm_code = p_str_Itmcode;
                        ServiceObject.Add_Style_To_Itm_dtl(objInboundInquiry);
                    }
                }
                else
                {
                    if (p_str_Itmcode == "")
                    {
                        int l_str_Count = 0;
                        return Json(l_str_Count, JsonRequestBehavior.AllowGet);
                    }
                }

                IBinMasterService ServiceBinMaster = new BinMasterService();
                ServiceBinMaster.fnInsertBinLocByIBDocId(p_str_cmp_id, l_str_DftWhs, p_str_loc_id, p_str_Itmcode, p_str_desc);

                objInboundInquiry = ServiceObject.Validate_Itm(objInboundInquiry);
                if (objInboundInquiry.LstItmExist.Count > 0)
                {
                    objInboundInquiry.itm_code = objInboundInquiry.LstItmExist[0].itm_code;
                    p_str_Itmcode = objInboundInquiry.itm_code;
                }
                objInboundInquiry = ServiceObject.GetItmName(objInboundInquiry);
                if (objInboundInquiry.ListPickdtl.Count() != 0)
                {
                    objInboundInquiry = ServiceObject.UpdtItmDtl(objInboundInquiry);
                }
                objInboundInquiry = ServiceObject.GetCheckExistGridDataRecvEntry(objInboundInquiry);
                objInboundInquiry = ServiceObject.InsertRecvEntryTemptable(objInboundInquiry);
                objInboundInquiry = ServiceObject.GetItemRcvdQty(objInboundInquiry);
                if (objInboundInquiry.ListItemRcvdQty.Count != 0)
                {
                    l_int_rcvd_qty = objInboundInquiry.ListItemRcvdQty[0].rcvd_qty;
                }
                //objInboundInquiry.balance = p_str_ord_qty - (p_str_tot_qty + l_int_rcvd_qty);CR20180901
                objInboundInquiry.balance = p_int_recdqty - (p_str_tot_qty);
                TempData["balance"] = objInboundInquiry.balance;
                //objInboundInquiry.tmp_recving_qty = p_str_tot_qty + l_int_rcvd_qty;
                objInboundInquiry.rcvd_qty = RcvdQty + p_str_tot_qty;
                TempData["rcvd_qty"] = objInboundInquiry.rcvd_qty;
                objInboundInquiry.ordr_qty = p_str_ord_qty;
                TempData["ordr_qty"] = objInboundInquiry.ordr_qty;
                objInboundInquiry = ServiceObject.GetCtnLineNo(objInboundInquiry);
                if (objInboundInquiry.ListCtnLine.Count != 0)
                {
                    TempData["ctn_line"] = objInboundInquiry.ListCtnLine[0].ctn_line;
                }
                TempData["dtl_line"] = p_str_line_num;
            }
            else
            {
                //objInboundInquiry = ServiceObject.GetItmCode(objInboundInquiry);
                //objInboundInquiry.itm_code = objInboundInquiry.ListDocRecvEntryDtl[0].itm_code;
                //p_str_Itmcode = objInboundInquiry.itm_code;
                // objInboundInquiry.itm_code = p_str_Itmcode;
                objInboundInquiry = ServiceObject.GetItmName(objInboundInquiry);
                if (objInboundInquiry.ListPickdtl.Count() != 0)
                {
                    objInboundInquiry = ServiceObject.UpdtItmDtl(objInboundInquiry);
                }
                objInboundInquiry = ServiceObject.GetCheckExistGridDataRecvEntry(objInboundInquiry);

                objInboundInquiry = ServiceObject.InsertRecvEntryTemptable(objInboundInquiry);
                objInboundInquiry = ServiceObject.GetItemRcvdQty(objInboundInquiry);
                //objInboundInquiry = ServiceObject.GetRecvdtlGrid(objInboundInquiry);
                if (objInboundInquiry.ListItemRcvdQty.Count != 0)
                {
                    l_int_rcvd_qty = objInboundInquiry.ListItemRcvdQty[0].rcvd_qty;
                }
                //objInboundInquiry.balance = p_str_ord_qty - (p_str_tot_qty + l_int_rcvd_qty);CR20180901
                objInboundInquiry.balance = p_int_recdqty - (p_str_tot_qty);
                TempData["balance"] = objInboundInquiry.balance;
                //objInboundInquiry.tmp_recving_qty = p_str_tot_qty + l_int_rcvd_qty;
                objInboundInquiry.rcvd_qty = RcvdQty + p_str_tot_qty;
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
            }
            //END  
            objInboundInquiry = ServiceObject.GetRecvdtlGrid(objInboundInquiry);
            objInboundInquiry.palet_id = p_str_palet_id;
            objInboundInquiry = ServiceObject.GetRecvEntryCount(objInboundInquiry);
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

        public JsonResult CheckBinRefered(string pstrCmpId, string pstrBinLocId, string pstrItmNum, string pstrItmColor, string pstrItmSize, string pstrItmCode)
        {
            InboundInquiryService objService = new InboundInquiryService();
           
            string BinLocRefered=  objService.fnCheckCheckBinRefered(pstrCmpId, pstrBinLocId, pstrItmNum, pstrItmColor, pstrItmSize, pstrItmCode);

            return Json(BinLocRefered, JsonRequestBehavior.AllowGet);
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
            //objInboundInquiry.DocEntryCount = objInboundInquiry.ListGetDocEntryCount[0].DocCount;
            objInboundInquiry.DocEntryCount = objInboundInquiry.ListGetDocEntryCount[0].recvcount;//CR20180910
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
            objCustMaster.cust_id = p_str_cmp_id;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }
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
                            IList<BillingInquiry> rptSource = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.ToList();
                            if (rptSource.Count > 0)
                            { 
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo; //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }
                        else
                        {
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                            objBillingInquiry = ServiceObject.GetBillingBillDocSTRGRpt(objBillingInquiry);
                            IList<BillingInquiry> rptSource = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.ToList();
                            if(rptSource.Count > 0)
                            { 
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    strReportName = "rpt_st_bill_doc.rpt";
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                
                                    rd.Load(strRptPath);
                                     rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
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
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                            objBillingInquiry = ServiceObject.GetBillingBillDocCubeSTRGRpt(objBillingInquiry);
                            IList<BillingInquiry> rptSource = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.ToList();
                            if (rptSource.Count >0)
                            { 
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    strReportName = "rpt_st_bill_doc_bycube.rpt";
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    rd.Load(strRptPath);
                                     rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }
                        else
                        {
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                            objBillingInquiry = ServiceObject.GetBillingBillDocCubeSTRGRpt(objBillingInquiry);
                            IList<BillingInquiry> rptSource = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.ToList();
                            using (ReportDocument rd = new ReportDocument())
                            {
                                strReportName = "rpt_st_bill_doc_bycube.rpt";
                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                                rd.Load(strRptPath);
                                rd.SetDataSource(rptSource);
                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            }
                        }

                    }
                    if (l_str_rpt_bill_type.Trim() == "Pallet")

                    {
                        objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                        objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        objBillingInquiry = ServiceObject.GetBillingBillDocPalletSTRGRpt(objBillingInquiry);
                        IList<BillingInquiry> rptSource = objBillingInquiry.ListGenBillingStrgByPalletRpt.ToList();
                        if(rptSource.Count >0)
                        { 
                            using (ReportDocument rd = new ReportDocument())
                            { 
                                strReportName = "rpt_strg_bill_by_pallet.rpt";
                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                rd.Load(strRptPath);
                                rd.SetDataSource(rptSource);
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
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                            objBillingInquiry = ServiceObject.GetBillingBillInoutCartonInstrgRpt(objBillingInquiry);
                            IList<BillingInquiry> rptSource = objBillingInquiry.ListBillingInoutCartonInstrgRpt.ToList();
                            if (rptSource.Count > 0)
                            { 
                                using (ReportDocument rd = new ReportDocument())
                                { 
                                    strReportName = "rpt_inout_bill_doc_with_initstrg.rpt";
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                              
                                    rd.Load(strRptPath);
                                    rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }
                        else
                        {
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                            objBillingInquiry = ServiceObject.GetBillingBillInoutCartonRpt(objBillingInquiry);
                            IList<BillingInquiry> rptSource = objBillingInquiry.ListBillingInoutCartonRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                { 
                                    strReportName = "rpt_inout_bill_doc.rpt";
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                           
                                    rd.Load(strRptPath);
                                    rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                    rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                                    rd.SetParameterValue("fml_rpt_bill_title", "(INOUT BILL BY CARTON)");
                                    rd.SetParameterValue("fml_rpt_param_bill_num", "Bill#");
                                    rd.SetParameterValue("fml_rpt_param_bill_date", "Bill Dt");

                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }

                    }
                    if (l_str_rpt_bill_inout_type.Trim() == "Cube")

                    {
                        if (l_str_rpt_instrg_req.Trim() == "Y")
                        {
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                            objBillingInquiry = ServiceObject.GetBillingBillInoutCubeInstrgRpt(objBillingInquiry);
                            IList<BillingInquiry> rptSource = objBillingInquiry.ListBillingInoutCubeInstrgRpt.ToList();
                            if (rptSource.Count >0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                { 
                                    strReportName = "rpt_inout_bill_doc_bycube_with_initstrg.rpt";
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    rd.Load(strRptPath);
                                    rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }
                        else
                        {
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                            objBillingInquiry = ServiceObject.GetBillingBillInoutCubeRpt(objBillingInquiry);
                            IList<BillingInquiry> rptSource = objBillingInquiry.ListBillingInoutCubeRpt.ToList();
                            if (rptSource.Count > 0)
                            {

                                using (ReportDocument rd = new ReportDocument())
                                { 
                                    strReportName = "rpt_inout_bill_doc_bycube.rpt";
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    rd.Load(strRptPath);
                                    rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                    rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                                    rd.SetParameterValue("fml_rpt_bill_title", "(INOUT BILL BY CUBE)");
                                    rd.SetParameterValue("fml_rpt_param_bill_num", "Bill#");
                                    rd.SetParameterValue("fml_rpt_param_bill_date", "Bill Dt");
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }

                    }
                    if (l_str_rpt_bill_inout_type.Trim() == "Container")

                    {

                        objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                        objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        objBillingInquiry = ServiceObject.GetBillingBillDocContainerInoutRpt(objBillingInquiry);
                        IList<BillingInquiry> rptSource = objBillingInquiry.ListGenBillingInoutByContainerRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            { 
                                strReportName = "rpt_inout_bill_by_Container.rpt";
                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                rd.Load(strRptPath);
                                rd.SetDataSource(rptSource);
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            }
                        }

                    }

                }
                if (l_str_rpt_bill_doc_type == "NORM")
                {
                    objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                    objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                    objBillingInquiry = ServiceObject.GetBillingBillDocVASRpt(objBillingInquiry);
                    IList<BillingInquiry> rptSource = objBillingInquiry.ListBillingDocVASRpt.ToList();
                    if(rptSource.Count > 0)
                    {
                        using (ReportDocument rd = new ReportDocument())
                        { 
                            strReportName = "rpt_va_bill_doc.rpt";
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            rd.Load(strRptPath);
                            rd.SetDataSource(rptSource);
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
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
        public ActionResult IncludeDocDetailEntry(string p_str_cmp_id, string p_str_ibdocid, string p_str_line_num, string p_str_style, string p_str_color, string p_str_size, string p_str_desc, string p_str_Itmcode,
           string p_str_ord_qty, int p_str_ppk, int p_str_ctns, string p_str_po_num, decimal p_str_length, decimal p_str_width, decimal p_str_height,
           decimal p_str_weight, decimal p_str_cube, string p_str_loc_id, string p_str_strg_rate, string p_str_inout_rate, string p_str_note, string p_int_ctn_line,
           string p_str_factory_id, string p_str_cust_name, string p_str_cust_po_num, string p_str_pick_list)
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
                    objInboundInquiry.length = p_str_length;
                    objInboundInquiry.width = p_str_width;
                    objInboundInquiry.height = p_str_height;
                    objInboundInquiry.weight = p_str_weight;
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
                    objInboundInquiry.length = p_str_length;
                    objInboundInquiry.width = p_str_width;
                    objInboundInquiry.height = p_str_height;
                    objInboundInquiry.weight = p_str_weight;
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
                objInboundInquiry.factory_id = p_str_factory_id;
                objInboundInquiry.cust_name = p_str_cust_name;
                objInboundInquiry.cust_po_num = p_str_cust_po_num;
                objInboundInquiry.pick_list = p_str_pick_list;
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
            //objInboundInquiry = objService.GEtStrgBillTYpe(objInboundInquiry);
            //objInboundInquiry.bill_inout_type = objInboundInquiry.ListStrgBillType[0].bill_inout_type;
            //if (objInboundInquiry.bill_inout_type == "Container")
            //{
                objInboundInquiry = objService.Check_Exist_Container_Id(objInboundInquiry);
                if (objInboundInquiry.ListCheckExistContainerId.Count() != 0)
                {
                    if (objInboundInquiry.ListCheckExistContainerId[0].cntr_id.Trim() == p_str_cntr_id)
                    {
                        l_str_result = "True";
                    }
                }
            //}
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
        public JsonResult ItemXGetLocDtl(string term, string cmp_id)
        {
            StockChangeService ServiceObject = new StockChangeService();
            var List = ServiceObject.ItemXGetLocDetails(term.Trim(), cmp_id).LstItmxlocdtl.Select(x => new { label = x.loc_id, value = x.loc_id }).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        // Add IbsEntry Window
        public ActionResult IbsEntryAdd(string p_str_cmp_id, string p_str_ib_docid, string p_str_status, string p_str_ib_doc_dt, string p_str_user_id)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.ib_doc_id = p_str_ib_docid;
            objInboundInquiry.status = p_str_status;
            if (Session["UserID"].ToString() != null)
            {
                objCompany.user_id = Session["UserID"].ToString().Trim();
            }
            objInboundInquiry.Status = "O";
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objInboundInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objInboundInquiry = objService.GET_IBS_DOC_ID(objInboundInquiry);
            if (objInboundInquiry.List_get_Ibs_Doc_ID.Count > 0 && objInboundInquiry.List_get_Ibs_Doc_ID != null)
            {
                objInboundInquiry.ibs_doc_id = objInboundInquiry.List_get_Ibs_Doc_ID[0].ibs_doc_id;
            }
            objInboundInquiry = objService.GET_IBS_DTL_FROM_RATE_MASTER(objInboundInquiry);
            for (int i = 0; i < objInboundInquiry.List_ibs_dtl.Count(); i++)
            {
                objInboundInquiry.dtl_line = i + 1;
                objInboundInquiry.itm_num = (objInboundInquiry.List_ibs_dtl[i].itm_num == null ? string.Empty : objInboundInquiry.List_ibs_dtl[i].itm_num.Trim());
                objInboundInquiry.itm_name = (objInboundInquiry.List_ibs_dtl[i].itm_name == null ? string.Empty : objInboundInquiry.List_ibs_dtl[i].itm_name.Trim());
                objInboundInquiry.amt = objInboundInquiry.List_ibs_dtl[i].amt;
                objInboundInquiry.Status = "O";
                objInboundInquiry.notes = "";
                objInboundInquiry.srvc_price = objInboundInquiry.List_ibs_dtl[i].list_price;
                objInboundInquiry.srvc_uom = (objInboundInquiry.List_ibs_dtl[i].price_uom == null ? string.Empty : objInboundInquiry.List_ibs_dtl[i].price_uom.Trim());
                objInboundInquiry.ib_doc_id = (p_str_ib_docid == null ? string.Empty : p_str_ib_docid.Trim());
                objInboundInquiry.ib_doc_dt = (p_str_ib_doc_dt == null ? string.Empty : p_str_ib_doc_dt.Trim());
                objInboundInquiry.cmp_id = (p_str_cmp_id == null ? string.Empty : p_str_cmp_id.Trim());
                objInboundInquiry.ibs_user_id = (p_str_user_id == null ? string.Empty : p_str_user_id.Trim());
                objInboundInquiry.ibs_doc_id = objInboundInquiry.ibs_doc_id;
                objService.INSERT_IBS_DTL_TEMP_TBL(objInboundInquiry);
            }
            objInboundInquiry = objService.GET_IBS_DTL_TEMP_TBL(objInboundInquiry);
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_AddIBSEntry", InboundInquiryModel);
        }

        // Edit,Del,View IbsEntry Window
        public ActionResult IbsEntryEdit(string p_str_cmp_id, string p_str_ib_docid, string p_str_status, string p_str_ib_doc_dt, string p_str_ibs_doc_id, string p_str_user_id, string p_str_action_type)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.ib_doc_id = p_str_ib_docid;
            objInboundInquiry.ibs_doc_id = p_str_ibs_doc_id;
            objInboundInquiry.status = p_str_status;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objInboundInquiry.Status = "O";
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objInboundInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objInboundInquiry = objService.GET_IBS_DTL(objInboundInquiry);
            for (int i = 0; i < objInboundInquiry.List_get_ibs_dtl.Count(); i++)
            {
                objInboundInquiry.dtl_line = i + 1;
                objInboundInquiry.itm_num = (objInboundInquiry.List_get_ibs_dtl[i].srvc_id == null ? string.Empty : objInboundInquiry.List_get_ibs_dtl[i].srvc_id.Trim());
                objInboundInquiry.itm_name = (objInboundInquiry.List_get_ibs_dtl[i].srvc_name == null ? string.Empty : objInboundInquiry.List_get_ibs_dtl[i].srvc_name.Trim());
                objInboundInquiry.srvc_qty = objInboundInquiry.List_get_ibs_dtl[i].srvc_qty;
                objInboundInquiry.amt = objInboundInquiry.List_get_ibs_dtl[i].srvc_line_amount;
                objInboundInquiry.Status = "O";
                objInboundInquiry.notes = (objInboundInquiry.List_get_ibs_dtl[i].notes == null ? string.Empty : objInboundInquiry.List_get_ibs_dtl[i].notes.Trim());
                objInboundInquiry.srvc_price = objInboundInquiry.List_get_ibs_dtl[i].srvc_price;
                objInboundInquiry.srvc_uom = (objInboundInquiry.List_get_ibs_dtl[i].srvc_uom == null ? string.Empty : objInboundInquiry.List_get_ibs_dtl[i].srvc_uom.Trim());
                objInboundInquiry.ib_doc_id = (p_str_ib_docid == null ? string.Empty : p_str_ib_docid.Trim());
                objInboundInquiry.ib_doc_dt = (p_str_ib_doc_dt == null ? string.Empty : p_str_ib_doc_dt.Trim());
                objInboundInquiry.cmp_id = (p_str_cmp_id == null ? string.Empty : p_str_cmp_id.Trim());
                objInboundInquiry.ibs_user_id = (p_str_user_id == null ? string.Empty : p_str_user_id.Trim());
                objInboundInquiry.ibs_doc_id = (p_str_ibs_doc_id == null ? string.Empty : p_str_ibs_doc_id.Trim());
                objService.INSERT_IBS_DTL_TEMP_TBL(objInboundInquiry);

            }
            objInboundInquiry = objService.GET_IBS_DTL_TEMP_TBL(objInboundInquiry);
            objInboundInquiry.action_type = p_str_action_type;
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_EditIBSEntry", InboundInquiryModel);
        }

        public ActionResult ViewContainer(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_cntr_id)
        {
            return PartialView("_ViewContainer", null);
        }


        // Add IbsEntry
        public JsonResult SaveIBSEntryDtls(List<ib_ibs_dtl> ItemDetails)
        {

            int l_dec_lineNum = 0;
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            if (ItemDetails == null)
            {
                ItemDetails.Clear();
                int l_str_Count = 1;
                return Json(l_str_Count, JsonRequestBehavior.AllowGet);
            }
            foreach (ib_ibs_dtl Item in ItemDetails)
            {
                l_dec_lineNum = l_dec_lineNum + 1;
                objInboundInquiry.dtl_line = l_dec_lineNum;
                objInboundInquiry.ib_doc_id = Item.ib_doc_id;
                objInboundInquiry.ibs_doc_id = Item.ibs_doc_id;
                objInboundInquiry.cmp_id = Item.cmp_id;
                objInboundInquiry.Status = "O";
                objInboundInquiry.itm_num = Item.srvc_id;
                objInboundInquiry.itm_name = Item.srvc_name;
                objInboundInquiry.srvc_qty = Item.srvc_qty;
                objInboundInquiry.srvc_price = Item.srvc_price;
                objInboundInquiry.srvc_uom = Item.srvc_uom;
                objInboundInquiry.Note = Item.notes;
                objService.INSERT_IBS_DTL(objInboundInquiry);
                objService.DELETE_IBS_DTL_TEMP_TBL(objInboundInquiry);
            }
            ItemDetails.Clear();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        // Edit IbsEntry 
        public JsonResult UpdateIBSEntryDtls(List<ib_ibs_dtl> ItemDetails)
        {

            int l_dec_lineNum = 0;
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            if (ItemDetails == null)
            {
                ItemDetails.Clear();
                int l_str_Count = 1;
                return Json(l_str_Count, JsonRequestBehavior.AllowGet);
            }
            foreach (ib_ibs_dtl Item in ItemDetails)
            {
                objInboundInquiry.ib_doc_id = Item.ib_doc_id;
                objInboundInquiry.cmp_id = Item.cmp_id;
                objInboundInquiry.ibs_doc_id = Item.ibs_doc_id;
                objService.DELETE_IBS_DTL(objInboundInquiry);
            }
            foreach (ib_ibs_dtl Item in ItemDetails)
            {
                l_dec_lineNum = l_dec_lineNum + 1;
                objInboundInquiry.dtl_line = l_dec_lineNum;
                objInboundInquiry.ib_doc_id = Item.ib_doc_id;
                objInboundInquiry.ibs_doc_id = Item.ibs_doc_id;
                objInboundInquiry.cmp_id = Item.cmp_id;
                objInboundInquiry.Status = "O";
                objInboundInquiry.itm_num = Item.srvc_id;
                objInboundInquiry.itm_name = Item.srvc_name;
                objInboundInquiry.srvc_qty = Item.srvc_qty;
                objInboundInquiry.srvc_price = Item.srvc_price;
                objInboundInquiry.srvc_uom = Item.srvc_uom;
                objInboundInquiry.Note = Item.notes;
                objService.INSERT_IBS_DTL(objInboundInquiry);
                objService.DELETE_IBS_DTL_TEMP_TBL(objInboundInquiry);
            }
            ItemDetails.Clear();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        // Delete IbsEntry
        public ActionResult DeleteIbsEntryDtls(string p_str_cmp_id, string p_str_ibdocid, string p_str_ibsdocid)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService objService = new InboundInquiryService();
            objInboundInquiry.cmp_id = (p_str_cmp_id == null ? string.Empty : p_str_cmp_id.Trim());
            objInboundInquiry.ib_doc_id = (p_str_ibdocid == null ? string.Empty : p_str_ibdocid.Trim());
            objInboundInquiry.ibs_doc_id = (p_str_ibsdocid == null ? string.Empty : p_str_ibsdocid.Trim());
            try
            {
                objService.DELETE_IBS_DTL_TEMP_TBL(objInboundInquiry);
                objService.DELETE_IBS_DTL(objInboundInquiry);
                return Json("Deleted", JsonRequestBehavior.AllowGet);
            }
            catch (SqlException Ex)
            {
                return Json("Not Deleted", JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult ShowReportByIBCntrConfirmation(string p_str_cmp_id, string p_str_cntr_id, string p_str_doc_dtFm, string p_str_doc_dtTo, string p_str_rpt_type)
        {

            string jsonErrorCode = "0";
            string l_str_rpt_selection = string.Empty;
            string l_str_img_path = string.Empty;
            string l_str_msg = string.Empty;
            string strRptPath = string.Empty;
            try
            {
                objCustMaster.cust_id = p_str_cmp_id;
                objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
                if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
                {
                    objCustMaster.ListGetCustLogo[0].cust_logo = string.Empty;
                }
                IBRcvdRptByCntrDtl objIBRcvdRptByCntrDtl = new IBRcvdRptByCntrDtl();
                InboundReportService ServiceObject = new InboundReportService();
                objIBRcvdRptByCntrDtl.cmp_id = p_str_cmp_id;
                objIBRcvdRptByCntrDtl.cntr_id = p_str_cntr_id;

                objIBRcvdRptByCntrDtl.rcvd_dt_from = p_str_doc_dtFm;
                objIBRcvdRptByCntrDtl.rcvd_dt_to = p_str_doc_dtTo;
                objIBRcvdRptByCntrDtl = ServiceObject.GetIBRcvdRptByCntrDtl(objIBRcvdRptByCntrDtl);

                if (objIBRcvdRptByCntrDtl.ListIBRcvdRptByCntr.Count == 0)
                {
                    return Json("No Record Found", JsonRequestBehavior.AllowGet);
                }


                if (p_str_cntr_id.Length > 0)
                {
                    strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//rpt_ib_recv_rpt_by_cntr.rpt";
                }
                else
                {
                    strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//rpt_ib_recv_rpt_by_mlty_cntr.rpt";
                }

                //   strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//rpt_ib_recv_rpt_by_cntr.rpt";
                var rptSource = objIBRcvdRptByCntrDtl.ListIBRcvdRptByCntr.ToList();

               
                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                {
                    using (ReportDocument rd = new ReportDocument())
                    { 
                        rd.Load(strRptPath);
                        rd.SetDataSource(rptSource);
                        if (p_str_cntr_id.Length > 0)
                        {
                            rd.SetParameterValue("fml_rpt_title", "BY CONTAINER - " + p_str_cntr_id);
                            //rd.SetParameterValue("fml_rpt_title", "BY CONTAINER ");
                        }
                        else
                        {
                            rd.SetParameterValue("fml_rpt_title", "BY CONTAINER ");
                        }

                        l_str_img_path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo;
                        rd.SetParameterValue("fml_image_path", l_str_img_path);
                        if (p_str_rpt_type == "PDF")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Inbound Reports");
                        }
                        else if (p_str_rpt_type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Inbound Reports");
                        }
                        else
                        if (p_str_rpt_type == "Excel")
                        {
                        }
                    }
                }


            }

            catch (Exception ex)
            {
                l_str_msg = ex.Message;
                jsonErrorCode = "-2";
            }

            return Json(new { result = jsonErrorCode, err = l_str_msg }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetIBDocExcpList(string p_str_cmp_id)
        {
            InboundInquiryService objService = new InboundInquiryService();
            IBDocExcp objIBDocExcp = new IBDocExcp();
            objIBDocExcp = objService.GetIBDocExcpList(objIBDocExcp, p_str_cmp_id);
            GridView gv = new GridView();
            gv.DataSource = objIBDocExcp.ListIBDocExcp;
            gv.DataBind();
            Session["ib_doc_excp_list"] = gv;
            return new DownloadFileActionResult((GridView)Session["ib_doc_excp_list"], p_str_cmp_id + "-IB-DOC-EXCP-" + DateTime.Now.ToString("yyyyMMddHHssmm") + ".xls");

        }

        public ActionResult DeleteIOBillByIbDocId(string p_str_cmp_id, string p_str_bill_doc_id, string p_str_lot_id)
        {
            string jsonErrorCode = "0";
            string l_str_msg = string.Empty;
            bool l_bol_sp_status = false;

            try
            {
                InboundInquiryService objService = new InboundInquiryService();
                l_bol_sp_status = objService.DeleteIOBillByIbDocId(p_str_cmp_id, p_str_bill_doc_id, p_str_lot_id);
                if (l_bol_sp_status == false)
                    jsonErrorCode = "-2";

            }
            catch (Exception ex)
            {
                l_str_msg = ex.Message;
                jsonErrorCode = "-2";
            }

            return Json(new { result = jsonErrorCode, err = l_str_msg }, JsonRequestBehavior.AllowGet);
        }

        public FileStreamResult XlsIBDocExcpFileDownload(string p_str_cmp_id)
        {


            string tempFileName = string.Empty;
            string l_str_file_name = string.Empty;
            string strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") +  ConfigurationManager.AppSettings["tempFilePath"].ToString();
            if (!Directory.Exists(strOutputpath))
            {
                Directory.CreateDirectory(strOutputpath);
            }

            InboundInquiryService objService = new InboundInquiryService();
            DataTable dtIBExcp = new DataTable();
            dtIBExcp = objService.GetIBDocExceptionList(p_str_cmp_id);
            l_str_file_name = "DF_" + p_str_cmp_id.ToUpper().ToString().Trim() + "-INBOUND_EXCP-" + strDateFormat + ".xlsx";

            tempFileName = strOutputpath + l_str_file_name;

            if (System.IO.File.Exists(tempFileName))
                System.IO.File.Delete(tempFileName);
            get_ib_excp_template mxcel1 = new get_ib_excp_template(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "DF_INBOUND_EXCP.xlsx");
            mxcel1.PopulateHeader(p_str_cmp_id);
            mxcel1.PopulateData(dtIBExcp, true);
            mxcel1.SaveAs(tempFileName);
            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
            return File(fs, "application / xlsx", l_str_file_name);

        }


        public FileStreamResult getXlsIBStkSmryByLoc(string p_str_cmp_id, string p_str_ib_doc_id,  string p_str_cntr_id)
        {


            string tempFileName = string.Empty;
            string l_str_file_name = string.Empty;
            string strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
            if (!Directory.Exists(strOutputpath))
            {
                Directory.CreateDirectory(strOutputpath);
            }

            InboundInquiryService objService = new InboundInquiryService();
            DataTable dtIBStkSmry = new DataTable();
            dtIBStkSmry = objService.getXlsIBStkSmryByLocList(p_str_cmp_id, p_str_ib_doc_id);
            l_str_file_name =  p_str_cmp_id.ToUpper().ToString().Trim() + "-" + p_str_ib_doc_id + "-IB-STK-SMRY-BY-LOC-" + strDateFormat + ".xlsx";

            tempFileName = strOutputpath + l_str_file_name;

            if (System.IO.File.Exists(tempFileName))
                System.IO.File.Delete(tempFileName);
            ClsGetXlsIBStkSmryByLoc mxcel1 = new ClsGetXlsIBStkSmryByLoc(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IB_STK_SMRY_BY_LOC.xlsx");
            mxcel1.PopulateHeader(p_str_cmp_id, p_str_ib_doc_id, p_str_cntr_id);
            mxcel1.PopulateData(dtIBStkSmry, true);
            mxcel1.SaveAs(tempFileName);
            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
            return File(fs, "application / xlsx", l_str_file_name);

        }

        public ActionResult ContainerConfRpt(string rptFormat,string rptMode, string p_str_cmp_id, string p_str_whs_id, 
            string p_str_cntr_id, string p_str_rcvd_dt_from, string p_str_rcvd_dt_to)
        {

            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string l_str_rpt_selection = string.Empty;
            string l_str_img_path = string.Empty;
            string lstrEmailMsg = string.Empty;
            string strRptPath = string.Empty;
            string l_str_msg = string.Empty;
            try
            {
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
                
                if (rptFormat == "XLS")
                {

                    string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                    string strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                    string tempFileName = string.Empty;
                    string l_str_file_name = string.Empty;
                    DataTable dtBill = new DataTable();

                    dtBill = ServiceObject.GetInboundRptContainerExcel(p_str_cmp_id, p_str_whs_id, p_str_cntr_id, p_str_rcvd_dt_from, p_str_rcvd_dt_to);

                    if (!Directory.Exists(strOutputpath))
                    {
                        Directory.CreateDirectory(strOutputpath);
                    }

                    l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "_IB_RECV_BY_CNTR_" + strDateFormat + ".xlsx";

                    tempFileName = strOutputpath + l_str_file_name;

                    if (System.IO.File.Exists(tempFileName))
                        System.IO.File.Delete(tempFileName);
                    xls_IB_Recv_By_Container_Excel mxcel1 = new xls_IB_Recv_By_Container_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IB_RECV_RPT_BY_CNTR.xlsx");

                    mxcel1.PopulateHeader(p_str_cmp_id, p_str_rcvd_dt_from, p_str_rcvd_dt_to);
                    mxcel1.PopulateData(dtBill, true);
                    mxcel1.SaveAs(tempFileName);
                    Session["RptFileName"] = tempFileName;
                    if (rptMode == "VIEW")
                    {
                        FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                        return Json(tempFileName, JsonRequestBehavior.AllowGet);
                    }

                    Email objEmail = new Email();
                    objEmail.screenId = "IB-INQUIRY";
                    objEmail.username = string.Empty;
                    objEmail.Reportselection = "CNTR-CONF";
                    objEmail.Attachment = l_str_file_name;
                    lstrEmailMsg = "Hi All," + "\n\n" + " Please Find the attached Container confirmation Report";
                    if (p_str_cntr_id.Length > 0)
                    {
                        objEmail.EmailSubject = p_str_cmp_id + " - Container Confirmation Report - "  + p_str_cntr_id;
                    }
                    else
                    {
                        objEmail.EmailSubject = p_str_cmp_id + " - Container Confirmation Report";
                    }
                    //clsRptEmail objRptEmail = new clsRptEmail();
                    //objRptEmail.getEmailDetails(objEmail, p_str_cmp_id, "INBOUND", "");
                    getEmailDetails(objEmail, lstrEmailMsg);

                    Mapper.CreateMap<Email, EmailModel>();
                    EmailModel EmailModel = Mapper.Map<Email, EmailModel>(objEmail);
                    return PartialView("_Email", EmailModel);

                }


            }

            catch (Exception ex)
            {
                l_str_msg = ex.Message;
                jsonErrorCode = "-2";
            }

            return Json(new { result = jsonErrorCode, err = l_str_msg }, JsonRequestBehavior.AllowGet);

        }

        private void getEmailDetails(Email objEmail,string pstrEmailMessage)
        {
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
            objEmail = objEmailService.GetSendMailDetails(objEmail);
            if (objEmail.ListEamilDetail.Count != 0)
            {

                objEmail.EmailTo = (objEmail.ListEamilDetail[0].EmailTo.Trim() == null || objEmail.ListEamilDetail[0].EmailTo.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailTo.Trim();
                objEmail.EmailCC = (objEmail.ListEamilDetail[0].EmailCC.Trim() == null || objEmail.ListEamilDetail[0].EmailCC.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailCC.Trim();
                if (objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == null)
                {
                    objEmail.EmailMessage = pstrEmailMessage;
                }
                else
                {
                    objEmail.EmailMessage = objEmail.ListEamilDetail[0].EmailMessageContent.Trim();
                }
                
            }
            else
            {
                objEmail.EmailMessage = pstrEmailMessage;
                objEmail.EmailTo = "";
                objEmail.EmailCC = "";
            }
            objEmail.EmailMessage = objEmail.EmailMessage + "\n" + "\n" + l_str_email_footer1;
            objEmail.EmailMessage = objEmail.EmailMessage + "\n" + "\n" + "Regards,";
            objEmail.EmailMessage = objEmail.EmailMessage + "\n" + l_str_email_regards;
            objEmail.EmailMessage = objEmail.EmailMessage + "\n" + "\n" + l_str_email_footer2;

           
           
        }
        public ActionResult ShowExcelReport(string p_str_file_name, string p_str_cmp_id)
        {
            string strDateFormat = string.Empty;
            FileStream fs = new FileStream(p_str_file_name, FileMode.Open, FileAccess.Read);
            string l_str_down_load_file_name = string.Empty;
            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
            l_str_down_load_file_name = p_str_cmp_id + "-" + "IB-" + strDateFormat + ".xlsx";

            return File(fs, "application / xlsx", l_str_down_load_file_name);
        }
        public ActionResult fnGenerateExcelRpts(string rptFormat, string rptMode, string p_str_ib_doc_id, string p_str_ib_doc_id_to, string p_str_radio, string p_str_cmp_id, string p_str_cntr_id, string p_str_status, string p_str_ref_no, string p_str_doc_dtFm,
        string p_str_doc_dtTo, string p_str_eta_dtFm, string p_str_eta_dtTo, string p_str_itm_num, string p_str_itm_color, string p_str_itm_size,
        string type)
        {
            string lstrEmailMsg = string.Empty;
            string lstrEmailSubject = string.Empty;
            InboundInquiryService ServiceObject = new InboundInquiryService();
            string lstrModuleName = "INBOUND";
            string lstrReportId = string.Empty;
            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
            string strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
            InboundACKExcel objInboundInquiryExcel = new InboundACKExcel();
            DataTable dtBill = new DataTable();

            string tempFileName = string.Empty;
            string l_str_file_name = string.Empty;
            string l_str_eta_dt = string.Empty;
            string l_str_inout_type = string.Empty;
            string l_str_whs_id = string.Empty;
            string l_str_palet_dt = string.Empty;
            string l_str_rate_id = string.Empty;

            string l_str_cmp_id = p_str_cmp_id;
            string l_str_doc_id = p_str_ib_doc_id;
            string l_str_ib_doc_dt = p_str_doc_dtTo;
            string l_str_cntr_id = p_str_cntr_id;
            string l_str_req_num = p_str_ref_no;
            string l_str_status = p_str_status;
            string l_str_ib_doc_dt_fm = p_str_doc_dtFm;
            string l_str_ib_doc_dt_to = p_str_doc_dtTo;
            string l_str_eta_dt_fm = p_str_eta_dtFm;
            string l_str_eta_dt_to = p_str_eta_dtTo;
            string l_str_itm_num = p_str_itm_num;
            string l_str_itm_color = p_str_itm_color;
            string l_str_itm_size = p_str_itm_size;

            string l_str_vendor_id = objInboundInquiryExcel.vendor_id;
            string l_str_report_selection = p_str_radio;

            if (l_str_report_selection == "GridSummary")
            {

                dtBill = ServiceObject.GetInboundGridSummaryExcelTemplate(l_str_cmp_id, l_str_doc_id, l_str_cntr_id, l_str_status, l_str_ib_doc_dt_fm, l_str_ib_doc_dt_to, l_str_req_num, l_str_eta_dt_fm, l_str_eta_dt_to, l_str_itm_num, l_str_itm_color, l_str_itm_size);

                if (!Directory.Exists(strOutputpath))
                {
                    Directory.CreateDirectory(strOutputpath);
                }

                l_str_file_name = "DF_" + p_str_cmp_id.ToUpper().ToString().Trim() + "_IB_GRID_SUMMARY_" + strDateFormat + ".xlsx";

                tempFileName = strOutputpath + l_str_file_name;

                if (System.IO.File.Exists(tempFileName))
                    System.IO.File.Delete(tempFileName);

                xls_IB_GridSummary_Excel mxcel = new xls_IB_GridSummary_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IB_GRID_SUMMARY.xlsx");
                mxcel.PopulateHeader(l_str_cmp_id);
                mxcel.PopulateData(dtBill, true);
                mxcel.SaveAs(tempFileName);
                Session["RptFileName"] = tempFileName;
                lstrEmailMsg = "Please find the attached Grid Summary Report";
                lstrEmailSubject = l_str_cmp_id + " - Inbound - Grid Summary Report";
                lstrReportId = "IB-GRD-SMRY";
            }

            else if (l_str_report_selection == "Acknowledgement")
            {

                dtBill = ServiceObject.GetInboundAckExcelTemplate(l_str_cmp_id, l_str_doc_id);

                if (!Directory.Exists(strOutputpath))
                {
                    Directory.CreateDirectory(strOutputpath);
                }

                l_str_file_name = "DF_" + p_str_cmp_id.ToUpper().ToString().Trim() + "_IB_ACK_" + strDateFormat + ".xlsx";

                tempFileName = strOutputpath + l_str_file_name;

                if (System.IO.File.Exists(tempFileName))
                    System.IO.File.Delete(tempFileName);
                xls_IB_Acknowledgement_Excel mxcel1 = new xls_IB_Acknowledgement_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IB_INBOUND_ACK.xlsx");
                var dataRows = dtBill.Rows;
                DataRow dr = dataRows[0];
                l_str_doc_id = dr["ib_doc_id"].ToString();
                l_str_ib_doc_dt = dr["ib_doc_dt"].ToString();
                l_str_cntr_id = dr["cntr_id"].ToString();
                l_str_req_num = dr["req_num"].ToString();
                l_str_eta_dt = dr["eta_dt"].ToString();
                l_str_vendor_id = dr["vend_id"].ToString();
                mxcel1.PopulateHeader(l_str_cmp_id, l_str_doc_id, l_str_ib_doc_dt, l_str_cntr_id, l_str_req_num, l_str_eta_dt, l_str_vendor_id);
                mxcel1.PopulateData(dtBill);
                mxcel1.SaveAs(tempFileName);
                lstrEmailMsg = "Please find the attached Inbound Acknowledgement Report";
                lstrEmailSubject = l_str_cmp_id + " - Inbound - Acknowledgement Report - IB Doc Id - " + l_str_doc_id;
                lstrReportId = "IB-ACK";
            }

            else if (l_str_report_selection == "WorkSheet")
            {
                string l_str_ib_doc_id = l_str_doc_id;
                dtBill = ServiceObject.GetInboundWorkSheetExcelTemplate(l_str_cmp_id, l_str_ib_doc_id);

                if (!Directory.Exists(strOutputpath))
                {
                    Directory.CreateDirectory(strOutputpath);
                }

                l_str_file_name = "DF_" + p_str_cmp_id.ToUpper().ToString().Trim() + "_IB_WORKSHEET_" + strDateFormat + ".xlsx";

                tempFileName = strOutputpath + l_str_file_name;

                if (System.IO.File.Exists(tempFileName))
                    System.IO.File.Delete(tempFileName);
                xls_IB_Worksheet_Excel mxcel1 = new xls_IB_Worksheet_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IB_INBOUND_WORKSHEET.xlsx");
                var dataRows = dtBill.Rows;
                DataRow dr = dataRows[0];
                l_str_doc_id = dr["ib_doc_id"].ToString();
                l_str_ib_doc_dt = dr["ib_doc_dt"].ToString();
                l_str_cntr_id = dr["cntr_id"].ToString();
                l_str_req_num = dr["req_num"].ToString();
                l_str_eta_dt = dr["eta_dt"].ToString();
                l_str_vendor_id = dr["vend_id"].ToString();
                mxcel1.PopulateHeader(l_str_cmp_id, l_str_doc_id, l_str_ib_doc_dt, l_str_cntr_id, l_str_req_num, l_str_eta_dt, l_str_vendor_id);
                mxcel1.PopulateData(dtBill, true);
                mxcel1.SaveAs(tempFileName);
                lstrEmailMsg = "Please find the attached Worksheet Report";
                lstrEmailSubject = l_str_cmp_id + " - Inbound - Worksheet Report";
                lstrReportId = "IB-WRK-SHT";
            }

            else if (l_str_report_selection == "Confirmation")
            {
                InboundInquiry objInboundInquiry = new InboundInquiry();
                InboundInquiryService ServiceObjectForInoutType = new InboundInquiryService();
                objInboundInquiry.cmp_id = l_str_cmp_id;
                objInboundInquiry.ib_doc_id = l_str_doc_id;
                objInboundInquiry = ServiceObjectForInoutType.GEtStrgBillTYpe(objInboundInquiry);
                objInboundInquiry.bill_type = objInboundInquiry.ListStrgBillType[0].bill_type;
                objInboundInquiry.bill_inout_type = objInboundInquiry.ListStrgBillType[0].bill_inout_type;
                objInboundInquiry.CNTR_CHECK = "RATE_ID";
                ServiceObject.GetContainerandRateID(objInboundInquiry);
                l_str_inout_type = objInboundInquiry.check_inout_type;
                l_str_rate_id = objInboundInquiry.ListGETRateID[0].RATEID.Trim();

                if (l_str_inout_type != "")
                {
                    if (l_str_rate_id == "CNTR")
                    {
                        string l_str_ib_doc_id = l_str_doc_id;

                        dtBill = ServiceObject.GetInboundContainerExcelTemplate(l_str_cmp_id, l_str_ib_doc_id, l_str_rate_id);
                        var dataRows = dtBill.Rows;
                        DataRow dr = dataRows[0];
                        l_str_doc_id = dr["ib_doc_id"].ToString();
                        l_str_ib_doc_dt = dr["ib_doc_dt"].ToString();
                        l_str_cntr_id = dr["cntr_id"].ToString();
                        l_str_req_num = dr["hdr_po_num"].ToString();
                        l_str_palet_dt = dr["palet_dt"].ToString();
                        l_str_whs_id = dr["whs_id"].ToString();

                        if (!Directory.Exists(strOutputpath))
                        {
                            Directory.CreateDirectory(strOutputpath);
                        }

                        l_str_file_name = "DF_" + p_str_cmp_id.ToUpper().ToString().Trim() + "_IB_CONFIRMATION_BY_CONTAINER_" + strDateFormat + ".xlsx";

                        tempFileName = strOutputpath + l_str_file_name;

                        if (System.IO.File.Exists(tempFileName))
                            System.IO.File.Delete(tempFileName);
                        xls_IB_Confirmation_Excel mxcel = new xls_IB_Confirmation_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IB_INBOUND_WORKSHEET.xlsx");

                        mxcel.PopulateHeader(l_str_cmp_id, l_str_doc_id, l_str_ib_doc_dt, l_str_cntr_id, l_str_req_num, l_str_palet_dt, l_str_whs_id, l_str_rate_id);
                        mxcel.PopulateData(dtBill, true);
                        mxcel.SaveAs(tempFileName);
                        lstrEmailMsg = "Please find the attached Container confirmation Report";
                        lstrEmailSubject = l_str_cmp_id + " - Inbound - Container confirmation Report";
                    }
                    else
                    {
                        string l_str_ib_doc_id = l_str_doc_id;

                        dtBill = ServiceObject.GetInboundContainerExcelTemplate(l_str_cmp_id, l_str_ib_doc_id, l_str_rate_id);
                        var dataRows = dtBill.Rows;
                        DataRow dr = dataRows[0];
                        l_str_doc_id = dr["ib_doc_id"].ToString();
                        l_str_ib_doc_dt = dr["ib_doc_dt"].ToString();
                        l_str_cntr_id = dr["cont_id"].ToString();
                        l_str_req_num = dr["hdr_po_num"].ToString();
                        l_str_palet_dt = dr["palet_dt"].ToString();
                        l_str_whs_id = dr["whs_id"].ToString();

                        if (!Directory.Exists(strOutputpath))
                        {
                            Directory.CreateDirectory(strOutputpath);
                        }

                        l_str_file_name = "DF_" + p_str_cmp_id.ToUpper().ToString().Trim() + "_IB_CONFIRMATION_" + strDateFormat + ".xlsx";

                        tempFileName = strOutputpath + l_str_file_name;

                        if (System.IO.File.Exists(tempFileName))
                            System.IO.File.Delete(tempFileName);
                        xls_IB_Confirmation_Excel mxcel = new xls_IB_Confirmation_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IB_INBOUND_CONFIRMATION.xlsx");

                        mxcel.PopulateHeader(l_str_cmp_id, l_str_doc_id, l_str_ib_doc_dt, l_str_cntr_id, l_str_req_num, l_str_palet_dt, l_str_whs_id, l_str_rate_id);
                        mxcel.PopulateData(dtBill, true);
                        mxcel.SaveAs(tempFileName);


                        lstrEmailMsg = "Please find the attached Inbound confirmation Report";
                        lstrEmailSubject = l_str_cmp_id + " - Inbound - confirmation Report";
                    }
                    lstrReportId = "IB-CNTR-RCVD-CONF";
                }
            }
            else if (l_str_report_selection == "TallySheet")
            {
                string l_str_ib_doc_id = l_str_doc_id;

                dtBill = ServiceObject.GetInboundContainerExcelTemplate(l_str_cmp_id, l_str_ib_doc_id, l_str_rate_id);
                var dataRows = dtBill.Rows;
                DataRow dr = dataRows[0];
                l_str_doc_id = dr["ib_doc_id"].ToString();
                l_str_ib_doc_dt = dr["ib_doc_dt"].ToString();
                l_str_cntr_id = dr["cont_id"].ToString();
                l_str_req_num = dr["hdr_po_num"].ToString();
                l_str_palet_dt = dr["palet_dt"].ToString();
                l_str_whs_id = dr["whs_id"].ToString();

                if (!Directory.Exists(strOutputpath))
                {
                    Directory.CreateDirectory(strOutputpath);
                }

                l_str_file_name = "DF_" + p_str_cmp_id.ToUpper().ToString().Trim() + "_IB_TALLYSHEET_" + strDateFormat + ".xlsx";

                tempFileName = strOutputpath + l_str_file_name;

                if (System.IO.File.Exists(tempFileName))
                    System.IO.File.Delete(tempFileName);
                xls_IB_TallySheet_Excel mxcel = new xls_IB_TallySheet_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IB_INBOUND_TALLYSHEET.xlsx");

                mxcel.PopulateHeader(l_str_cmp_id, l_str_doc_id, l_str_ib_doc_dt, l_str_cntr_id, l_str_req_num, l_str_palet_dt, l_str_whs_id);
                mxcel.PopulateData(dtBill, true);
                mxcel.SaveAs(tempFileName);
                lstrEmailMsg = "Please find the attached Tally Sheet Report";
                lstrEmailSubject = l_str_cmp_id + " - Inbound - Tally Sheet Report";
                lstrReportId = "IB-TLY-SHT";
            }
            if (rptMode == "VIEW")
            {
                FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                return Json(tempFileName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                EmailAlertHdr objEmailAlertHdr = new EmailAlertHdr();
                clsRptEmail objRptEmail = new clsRptEmail();
                bool lblnRptEmailExists = false;
                objRptEmail.getEmailAlertDetails(objEmailAlertHdr, l_str_cmp_id, lstrModuleName, lstrReportId, ref lblnRptEmailExists);
                string l_str_email_message = string.Empty;
                if (lblnRptEmailExists == false)
                {
                    l_str_email_message = "Hi All, " + "\n" + lstrEmailMsg + "\n";
                }
                else
                {
                    l_str_email_message = "Hi All, " + "\n";
                    l_str_email_message = l_str_email_message + objEmailAlertHdr.emailMessage + "\n\n";
                }
                objEmailAlertHdr.emailMessage = l_str_email_message;
                objEmailAlertHdr.emailMessage = objEmailAlertHdr.emailMessage + "\n" + objEmailAlertHdr.emailFooter + "\n";
                objEmailAlertHdr.filePath = strOutputpath;
                objEmailAlertHdr.fileName = l_str_file_name;
                objEmailAlertHdr.emailSubject = lstrEmailSubject;
                EmailAlert objEmailAlert = new EmailAlert();
                objEmailAlertHdr.cmpId = l_str_cmp_id;
                objEmailAlert.objEmailAlertHdr = objEmailAlertHdr;
              
                Mapper.CreateMap<EmailAlert, EmailAlertModel>();
                EmailAlertModel EmailModel = Mapper.Map<EmailAlert, EmailAlertModel>(objEmailAlert);
            return PartialView("_EmailAlert", EmailModel);
            }

        }
        public ActionResult ShowReport_Excel_Templates(string p_str_ib_doc_id, string p_str_ib_doc_id_to, string p_str_radio, string p_str_cmp_id, string p_str_cntr_id, string p_str_status, string p_str_ref_no, string p_str_doc_dtFm,
            string p_str_doc_dtTo, string p_str_eta_dtFm, string p_str_eta_dtTo, string p_str_itm_num, string p_str_itm_color, string p_str_itm_size,
            string type)
        {

            InboundInquiryService ServiceObject = new InboundInquiryService();

            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
            string strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
            InboundACKExcel objInboundInquiryExcel = new InboundACKExcel();
            DataTable dtBill = new DataTable();

            string tempFileName = string.Empty;
            string l_str_file_name = string.Empty;
            string l_str_eta_dt = string.Empty;
            string l_str_inout_type = string.Empty;
            string l_str_whs_id = string.Empty;
            string l_str_palet_dt = string.Empty;
            string l_str_rate_id = string.Empty;

            string l_str_cmp_id = p_str_cmp_id;
            string l_str_doc_id = p_str_ib_doc_id;
            string l_str_ib_doc_dt = p_str_doc_dtTo;
            string l_str_cntr_id = p_str_cntr_id;
            string l_str_req_num = p_str_ref_no;
            string l_str_status = p_str_status;
            string l_str_ib_doc_dt_fm = p_str_doc_dtFm;
            string l_str_ib_doc_dt_to = p_str_doc_dtTo;
            string l_str_eta_dt_fm = p_str_eta_dtFm;
            string l_str_eta_dt_to = p_str_eta_dtTo;
            string l_str_itm_num = p_str_itm_num;
            string l_str_itm_color = p_str_itm_color;
            string l_str_itm_size = p_str_itm_size;

            string l_str_vendor_id = objInboundInquiryExcel.vendor_id;
            string l_str_report_selection = p_str_radio;

            if (l_str_report_selection == "GridSummary")
            {

                dtBill = ServiceObject.GetInboundGridSummaryExcelTemplate(l_str_cmp_id, l_str_doc_id, l_str_cntr_id, l_str_status, l_str_ib_doc_dt_fm, l_str_ib_doc_dt_to, l_str_req_num, l_str_eta_dt_fm, l_str_eta_dt_to, l_str_itm_num, l_str_itm_color, l_str_itm_size);

                if (!Directory.Exists(strOutputpath))
                {
                    Directory.CreateDirectory(strOutputpath);
                }

                l_str_file_name = "DF_" + p_str_cmp_id.ToUpper().ToString().Trim() + "_IB_GRID_SUMMARY_" + strDateFormat + ".xlsx";

                tempFileName = strOutputpath + l_str_file_name;

                if (System.IO.File.Exists(tempFileName))
                    System.IO.File.Delete(tempFileName);

                xls_IB_GridSummary_Excel mxcel = new xls_IB_GridSummary_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IB_GRID_SUMMARY.xlsx");
                mxcel.PopulateHeader(l_str_cmp_id);
                mxcel.PopulateData(dtBill, true);
                mxcel.SaveAs(tempFileName);
            }

            else if (l_str_report_selection == "Acknowledgement")
            {

                dtBill = ServiceObject.GetInboundAckExcelTemplate(l_str_cmp_id, l_str_doc_id);

                if (!Directory.Exists(strOutputpath))
                {
                    Directory.CreateDirectory(strOutputpath);
                }

                l_str_file_name = "DF_" + p_str_cmp_id.ToUpper().ToString().Trim() + "_IB_ACK_" + strDateFormat + ".xlsx";

                tempFileName = strOutputpath + l_str_file_name;

                if (System.IO.File.Exists(tempFileName))
                    System.IO.File.Delete(tempFileName);
                xls_IB_Acknowledgement_Excel mxcel1 = new xls_IB_Acknowledgement_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IB_INBOUND_ACK.xlsx");
                var dataRows = dtBill.Rows;
                DataRow dr = dataRows[0];
                l_str_doc_id = dr["ib_doc_id"].ToString();
                l_str_ib_doc_dt = dr["ib_doc_dt"].ToString();
                l_str_cntr_id = dr["cntr_id"].ToString();
                l_str_req_num = dr["req_num"].ToString();
                l_str_eta_dt = dr["eta_dt"].ToString();
                l_str_vendor_id = dr["vend_id"].ToString();
                mxcel1.PopulateHeader(l_str_cmp_id, l_str_doc_id, l_str_ib_doc_dt, l_str_cntr_id, l_str_req_num, l_str_eta_dt, l_str_vendor_id);
                mxcel1.PopulateData(dtBill);
                mxcel1.SaveAs(tempFileName);
            }

            else if (l_str_report_selection == "WorkSheet")
            {
                string l_str_ib_doc_id = l_str_doc_id;
                dtBill = ServiceObject.GetInboundWorkSheetExcelTemplate(l_str_cmp_id, l_str_ib_doc_id);

                if (!Directory.Exists(strOutputpath))
                {
                    Directory.CreateDirectory(strOutputpath);
                }

                l_str_file_name = "DF_" + p_str_cmp_id.ToUpper().ToString().Trim() + "_IB_WORKSHEET_" + strDateFormat + ".xlsx";

                tempFileName = strOutputpath + l_str_file_name;

                if (System.IO.File.Exists(tempFileName))
                    System.IO.File.Delete(tempFileName);
                xls_IB_Worksheet_Excel mxcel1 = new xls_IB_Worksheet_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IB_INBOUND_WORKSHEET.xlsx");
                var dataRows = dtBill.Rows;
                DataRow dr = dataRows[0];
                l_str_doc_id = dr["ib_doc_id"].ToString();
                l_str_ib_doc_dt = dr["ib_doc_dt"].ToString();
                l_str_cntr_id = dr["cntr_id"].ToString();
                l_str_req_num = dr["req_num"].ToString();
                l_str_eta_dt = dr["eta_dt"].ToString();
                l_str_vendor_id = dr["vend_id"].ToString();
                mxcel1.PopulateHeader(l_str_cmp_id, l_str_doc_id, l_str_ib_doc_dt, l_str_cntr_id, l_str_req_num, l_str_eta_dt, l_str_vendor_id);
                mxcel1.PopulateData(dtBill, true);
                mxcel1.SaveAs(tempFileName);
            }

            else if (l_str_report_selection == "Confirmation")
            {
                InboundInquiry objInboundInquiry = new InboundInquiry();
                InboundInquiryService ServiceObjectForInoutType = new InboundInquiryService();
                objInboundInquiry.cmp_id = l_str_cmp_id;
                objInboundInquiry.ib_doc_id = l_str_doc_id;
                objInboundInquiry = ServiceObjectForInoutType.GEtStrgBillTYpe(objInboundInquiry);
                objInboundInquiry.bill_type = objInboundInquiry.ListStrgBillType[0].bill_type;
                objInboundInquiry.bill_inout_type = objInboundInquiry.ListStrgBillType[0].bill_inout_type;
                objInboundInquiry.CNTR_CHECK = "RATE_ID";
                ServiceObject.GetContainerandRateID(objInboundInquiry);
                l_str_inout_type = objInboundInquiry.check_inout_type;
                l_str_rate_id = objInboundInquiry.ListGETRateID[0].RATEID.Trim();

                if (l_str_inout_type != "")
                {
                    if (l_str_rate_id == "CNTR")
                    {
                        string l_str_ib_doc_id = l_str_doc_id;

                        dtBill = ServiceObject.GetInboundContainerExcelTemplate(l_str_cmp_id, l_str_ib_doc_id, l_str_rate_id);
                        var dataRows = dtBill.Rows;
                        DataRow dr = dataRows[0];
                        l_str_doc_id = dr["ib_doc_id"].ToString();
                        l_str_ib_doc_dt = dr["ib_doc_dt"].ToString();
                        l_str_cntr_id = dr["cntr_id"].ToString();
                        l_str_req_num = dr["hdr_po_num"].ToString();
                        l_str_palet_dt = dr["palet_dt"].ToString();
                        l_str_whs_id = dr["whs_id"].ToString();

                        if (!Directory.Exists(strOutputpath))
                        {
                            Directory.CreateDirectory(strOutputpath);
                        }

                        l_str_file_name = "DF_" + p_str_cmp_id.ToUpper().ToString().Trim() + "_IB_CONFIRMATION_BY_CONTAINER_" + strDateFormat + ".xlsx";

                        tempFileName = strOutputpath + l_str_file_name;

                        if (System.IO.File.Exists(tempFileName))
                            System.IO.File.Delete(tempFileName);
                        xls_IB_Confirmation_Excel mxcel = new xls_IB_Confirmation_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IB_INBOUND_WORKSHEET.xlsx");

                        mxcel.PopulateHeader(l_str_cmp_id, l_str_doc_id, l_str_ib_doc_dt, l_str_cntr_id, l_str_req_num, l_str_palet_dt, l_str_whs_id, l_str_rate_id);
                        mxcel.PopulateData(dtBill, true);
                        mxcel.SaveAs(tempFileName);
                    }
                    else
                    {
                        string l_str_ib_doc_id = l_str_doc_id;

                        dtBill = ServiceObject.GetInboundContainerExcelTemplate(l_str_cmp_id, l_str_ib_doc_id, l_str_rate_id);
                        var dataRows = dtBill.Rows;
                        DataRow dr = dataRows[0];
                        l_str_doc_id = dr["ib_doc_id"].ToString();
                        l_str_ib_doc_dt = dr["ib_doc_dt"].ToString();
                        l_str_cntr_id = dr["cont_id"].ToString();
                        l_str_req_num = dr["hdr_po_num"].ToString();
                        l_str_palet_dt = dr["palet_dt"].ToString();
                        l_str_whs_id = dr["whs_id"].ToString();

                        if (!Directory.Exists(strOutputpath))
                        {
                            Directory.CreateDirectory(strOutputpath);
                        }

                        l_str_file_name = "DF_" + p_str_cmp_id.ToUpper().ToString().Trim() + "_IB_CONFIRMATION_" + strDateFormat + ".xlsx";

                        tempFileName = strOutputpath + l_str_file_name;

                        if (System.IO.File.Exists(tempFileName))
                            System.IO.File.Delete(tempFileName);
                        xls_IB_Confirmation_Excel mxcel = new xls_IB_Confirmation_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IB_INBOUND_CONFIRMATION.xlsx");

                        mxcel.PopulateHeader(l_str_cmp_id, l_str_doc_id, l_str_ib_doc_dt, l_str_cntr_id, l_str_req_num, l_str_palet_dt, l_str_whs_id, l_str_rate_id);
                        mxcel.PopulateData(dtBill, true);
                        mxcel.SaveAs(tempFileName);
                    }
                }
            }
            else if (l_str_report_selection == "TallySheet")
            {
                string l_str_ib_doc_id = l_str_doc_id;

                dtBill = ServiceObject.GetInboundContainerExcelTemplate(l_str_cmp_id, l_str_ib_doc_id, l_str_rate_id);
                var dataRows = dtBill.Rows;
                DataRow dr = dataRows[0];
                l_str_doc_id = dr["ib_doc_id"].ToString();
                l_str_ib_doc_dt = dr["ib_doc_dt"].ToString();
                l_str_cntr_id = dr["cont_id"].ToString();
                l_str_req_num = dr["hdr_po_num"].ToString();
                l_str_palet_dt = dr["palet_dt"].ToString();
                l_str_whs_id = dr["whs_id"].ToString();

                if (!Directory.Exists(strOutputpath))
                {
                    Directory.CreateDirectory(strOutputpath);
                }

                l_str_file_name = "DF_" + p_str_cmp_id.ToUpper().ToString().Trim() + "_IB_TALLYSHEET_" + strDateFormat + ".xlsx";

                tempFileName = strOutputpath + l_str_file_name;

                if (System.IO.File.Exists(tempFileName))
                    System.IO.File.Delete(tempFileName);
                xls_IB_TallySheet_Excel mxcel = new xls_IB_TallySheet_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IB_INBOUND_TALLYSHEET.xlsx");

                mxcel.PopulateHeader(l_str_cmp_id, l_str_doc_id, l_str_ib_doc_dt, l_str_cntr_id, l_str_req_num, l_str_palet_dt, l_str_whs_id);
                mxcel.PopulateData(dtBill, true);
                mxcel.SaveAs(tempFileName);
            }
            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
            return File(fs, "application / xlsx", l_str_file_name);
        }

        [HttpPost]
        public JsonResult checkIBDocInUse(string p_str_cmp_id, string p_str_ib_doc_id)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService IBService = new InboundInquiryService();

            if (IBService.checkIBDocInUse(p_str_cmp_id, p_str_ib_doc_id) == "Y")
            {
                return Json("Y", JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json("N", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
