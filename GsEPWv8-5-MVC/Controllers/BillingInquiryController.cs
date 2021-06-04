using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

#region Change History
// CR#                         Modified By   Date         Description
//CR_3PL_MVC_BL_2018_0210_001  Ravikumar     201-0210     To Fix the Storage Bill issue
//CR-3PL_MVC_IB_2018_0219_004  Added By Meera for add Inout bill by container
//CR-3PL_MVC_BL_2018_0224_001 Added By Nithya for add Storage bill by Pallet
//  CR_3PL_MVC_BL_2018_0226_001 MEERA 2018-02-26  Add Starage Bill By Pallet Report
// CR_3PL_MVC_BL_2018_0227_001 - Commented by Soniya for remove default date in filter section
//CR-3PL_MVC_IB_2018-03-10-001 Added By Nithya For Inbound inq BilldocID Details Not Showing
//CR-2018-05-21-001 Added By Nithya for Storage by pcs 
#endregion Change History

namespace GsEPWv8_5_MVC.Controllers
{
    public class BillingInquiryController : Controller
    {
        // GET: BillingInquiry
        decimal l_int_bill_amount = 0;
        public string EmailSub = string.Empty;
        public string EmailMsg = string.Empty;
        public string ScreenID = "Billing Inquiry";
        public string l_str_rptdtl = string.Empty;
        public string l_str_tmp_name = string.Empty;
        public string Folderpath = string.Empty;
        Email objEmail = new Email();
        EmailService objEmailService = new EmailService();
        BillingInquiry objBillingInquiry = new BillingInquiry();
        BillingInquiryService ServiceObject = new BillingInquiryService();
        Company objCompany = new Company();
        CompanyService ServiceObjectCompany = new CompanyService();
        public ActionResult BillingInquiry(string FullFillType, string cmp, string status, string DateFm, string DateTo, string p_str_scn_id)
        {
            string l_str_cmp_id = string.Empty;
            string l_str_fm_dt = string.Empty;
            string l_str_Dflt_Dt_Reqd = string.Empty;
            try
            {
                BillingInquiry objBillingInquiry = new BillingInquiry();
                BillingInquiryService ServiceObject = new BillingInquiryService();
                objBillingInquiry.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                objBillingInquiry.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                Session["g_str_Search_flag"] = "False";
                //CR_3PL_MVC_COMMON_2018_0324_001
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                if (objBillingInquiry.cmp_id == null || objBillingInquiry.cmp_id == string.Empty)
                {
                    objBillingInquiry.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                }
                else
                {
                    objCompany.cmp_id = Session["g_str_cmp_id"].ToString().Trim();

                }
                objBillingInquiry.screentitle = p_str_scn_id;
                l_str_Dflt_Dt_Reqd = Session["DFLT_DT_REQD"].ToString().Trim();
                if (l_str_Dflt_Dt_Reqd == "N")
                {
                    DateFm = "";
                    DateTo = "";
                }
                objBillingInquiry.Bill_doc_dt_Fr = DateTime.Now.AddDays(Common.clsGlobal.DispDateFrom).ToString("MM/dd/yyyy"); ;
                objBillingInquiry.Bill_doc_dt_To = DateTime.Now.ToString("MM/dd/yyyy");
                if (FullFillType == null)
                {
                    //objCompany.cmp_id = l_str_cmp_id;
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objBillingInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    DateTime date = DateTime.Now.AddMonths(-12);
                    l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");

                }
                else if (FullFillType != null)
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objBillingInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    objBillingInquiry.cmp_id = cmp;
                    objBillingInquiry.Bill_doc_id = "";
                    objBillingInquiry.Bill_type = "";

                    objBillingInquiry.Bill_doc_dt_Fr = DateTime.Now.AddDays(Common.clsGlobal.DispDateFrom).ToString("MM/dd/yyyy"); ;
                    objBillingInquiry.Bill_doc_dt_To = DateTime.Now.ToString("MM/dd/yyyy");

                    if (l_str_Dflt_Dt_Reqd == "Y")
                    {
                        objBillingInquiry = ServiceObject.GetBillingInquiryDetails(objBillingInquiry);
                    }
                    objCompany.cmp_id = cmp;
                    objCompany = ServiceObjectCompany.GetFullFillCompanyDetails(objCompany);
                }
                LookUp objLookUp = new LookUp();
                LookUpService ServiceObject1 = new LookUpService();
                objLookUp.id = "4";
                objLookUp.lookuptype = "BILLINGINQUIRY";
                objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                objBillingInquiry.ListLookUpDtl = objLookUp.ListLookUpDtl;
                Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
                BillingInquiryModel BillingInquiryModel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);

                return View(BillingInquiryModel);
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
            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();
            string l_str_tmp_cmp_id = string.Empty;
            Session["g_str_cmp_id"] = p_str_cmp_id;// CR_3PL_MVC_COMMON_2018_0326_001
            l_str_tmp_cmp_id = Session["g_str_cmp_id"].ToString().Trim();
            return Json(l_str_tmp_cmp_id, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerateStrgBill(string cmpid, string datefrom, string dateto)
        {
            string l_str_fm_dt = string.Empty;
            DateTime date = DateTime.Now;
            l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");
            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();
            objBillingInquiry.cmp_id = cmpid;
            objBillingInquiry.bill_print_dt = DateTime.Now;
            objBillingInquiry.bill_as_of_date = DateTime.Now.ToString("MM/dd/yyyy");
            objBillingInquiry.bill_pd_fm = l_str_fm_dt;
            objBillingInquiry.DocumentdateFrom = datefrom; //CR 20180308_01 Added by murugan
            objBillingInquiry.DocumentdateTo = dateto; //CR 20180308_01 Added by murugan
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cust_of_cmp_id = "";
            //CR-3PL_MVC_BL_2018_0224_001 
            objCompany.cmp_id = cmpid;
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
            objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
            objBillingInquiry.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objBillingInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objCompany.cmp_id = cmpid;
            objCompany = ServiceObjectCompany.GetFullFillCompanyDetails(objCompany);
            Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
            BillingInquiryModel objVasInquirymodel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);
            return PartialView("_GenerateSTRGBill", objVasInquirymodel);
        }
        public ActionResult GenerateVASBill(string cmpid, string p_str_scn_id)
        {
            string l_str_fm_dt = string.Empty;
            DateTime date = DateTime.Now;
            l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");
            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();
            objBillingInquiry.cmp_id = cmpid;
            objBillingInquiry.screentitle = p_str_scn_id;
            objBillingInquiry.bill_print_dt = DateTime.Now;
            objBillingInquiry.vas_bill_pd_to = DateTime.Now.ToString("MM/dd/yyyy");
            objBillingInquiry.vas_bill_pd_fm = l_str_fm_dt;
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cust_of_cmp_id = "";
            objCompany.cmp_id = cmpid;
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
            //objBillingInquiry.cust_of_cmpid = objBillingInquiry.LstCustOfCmpName[0].cust_of_cmpid;
            objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
            objBillingInquiry.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objBillingInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;

            Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
            BillingInquiryModel objVasInquirymodel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);
            return PartialView("_GenerateVASBill", objVasInquirymodel);
        }
        public ActionResult LoadGenerateVASBillDetail(string p_str_cmpid, string p_str_vasid, string p_str_vasdt, string p_str_screentitle)
        {
            string l_str_fm_dt = string.Empty;
            DateTime date = DateTime.Now;
            l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");
            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();
            objBillingInquiry.cmp_id = p_str_cmpid;
            objBillingInquiry.ship_doc_id = p_str_vasid;
            objBillingInquiry.ship_dt = Convert.ToDateTime(p_str_vasdt);
            objBillingInquiry.bill_print_dt = DateTime.Now;
            objBillingInquiry.bill_as_of_date = DateTime.Now.ToString("MM/dd/yyyy");
            objBillingInquiry.vas_bill_pd_to = DateTime.Now.ToString("MM/dd/yyyy");
            objBillingInquiry.vas_bill_pd_fm = l_str_fm_dt;
            objBillingInquiry.screentitle = p_str_screentitle;
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cust_of_cmp_id = "";
            objCompany.cmp_id = p_str_cmpid;
            objCompany = ServiceObjectCompany.GetCustOfCompName(objCompany);
            objBillingInquiry.LstCustOfCmpName = objCompany.LstCustOfCmpName;
            objBillingInquiry.cust_of_cmpid = objBillingInquiry.LstCustOfCmpName[0].cust_of_cmpid;
            objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
            objBillingInquiry.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objBillingInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
            BillingInquiryModel objVasInquirymodel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);
            return PartialView("_GenerateVASBill", objVasInquirymodel);
        }
        public ActionResult LoadGenerateInoutBillDetails(string cmpid, string p_str_screen_title, string p_str_Bill_doc_id)
        {
            string l_str_fm_dt = string.Empty;
            DateTime date = DateTime.Now;
            l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");
            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();
            objBillingInquiry.cmp_id = cmpid;
            objBillingInquiry.bill_doc_id = p_str_Bill_doc_id;
            objBillingInquiry.bill_print_dt = DateTime.Now;
            objBillingInquiry.screentitle = p_str_screen_title;
            objBillingInquiry.inout_bill_pd_fm = l_str_fm_dt;
            objBillingInquiry.inout_bill_pd_to = DateTime.Now.ToString("MM/dd/yyyy");
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cust_of_cmp_id = "";
            //CR-3PL_MVC_IB_2018_0219_004 
            objCompany.cmp_id = cmpid;
            objCompany = ServiceObjectCompany.GetCustOfCompName(objCompany);
            objBillingInquiry.LstCustOfCmpName = objCompany.LstCustOfCmpName;
            objBillingInquiry.cust_of_cmpid = objBillingInquiry.LstCustOfCmpName[0].cust_of_cmpid;
            objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
            objBillingInquiry.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objBillingInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;

            Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
            BillingInquiryModel objVasInquirymodel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);
            return PartialView("_GenerateInOutBill", objVasInquirymodel);
        }
        public ActionResult LoadGenerateConsolidateInoutBillDetails(string cmpid, string p_str_screen_title)
        {
            string l_str_fm_dt = string.Empty;
            DateTime date = DateTime.Now;
            string l_str_rpt_bill_type = string.Empty;
            l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");
            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();
            objBillingInquiry.cmp_id = cmpid;
            objBillingInquiry.screentitle = p_str_screen_title;
            objBillingInquiry.inout_bill_pd_fm = l_str_fm_dt;
            objBillingInquiry.inout_bill_pd_to = DateTime.Now.ToString("MM/dd/yyyy");
            objBillingInquiry.bill_as_of_date = DateTime.Now.ToString("MM/dd/yyyy");
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cust_of_cmp_id = "";
            //CR-3PL_MVC_IB_2018_0219_004 
            objCompany.cmp_id = cmpid;
            objCompany = ServiceObjectCompany.GetCustOfCompName(objCompany);
            objBillingInquiry.LstCustOfCmpName = objCompany.LstCustOfCmpName;
            objBillingInquiry.cust_of_cmpid = objBillingInquiry.LstCustOfCmpName[0].cust_of_cmpid;
            objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
            objBillingInquiry.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objBillingInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objBillingInquiry.cmp_id = cmpid;
            objBillingInquiry = ServiceObject.GetBillingInoutType(objBillingInquiry);
            l_str_rpt_bill_type = objBillingInquiry.ListBillingInoutType[0].bill_inout_type;
            objBillingInquiry.RateType = l_str_rpt_bill_type;
            Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
            BillingInquiryModel objVasInquirymodel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);
            return PartialView("_GenerateConsolidateInOutBill", objVasInquirymodel);
        }
        public ActionResult LoadGenerateConsolidateVasDetails(string cmpid, string p_str_screen_title)
        {
            string l_str_fm_dt = string.Empty;
            DateTime date = DateTime.Now;
            l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");
            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();
            objBillingInquiry.cmp_id = cmpid;
            objBillingInquiry.screentitle = p_str_screen_title;
            objBillingInquiry.vas_bill_pd_fm = l_str_fm_dt;
            objBillingInquiry.vas_bill_pd_to = DateTime.Now.ToString("MM/dd/yyyy");
            objBillingInquiry.bill_as_of_date = DateTime.Now.ToString("MM/dd/yyyy");
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cust_of_cmp_id = "";
            //CR-3PL_MVC_IB_2018_0219_004 
            objCompany.cmp_id = cmpid;
            objCompany = ServiceObjectCompany.GetCustOfCompName(objCompany);
            objBillingInquiry.LstCustOfCmpName = objCompany.LstCustOfCmpName;
            objBillingInquiry.cust_of_cmpid = objBillingInquiry.LstCustOfCmpName[0].cust_of_cmpid;
            objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
            objBillingInquiry.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objBillingInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
            BillingInquiryModel objVasInquirymodel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);
            return PartialView("_GenerateConsolidateVasBill", objVasInquirymodel);
        }
        public ActionResult LoadGenerateConsolidateStorageDetails(string cmpid, string p_str_screen_title)
        {
            string l_str_fm_dt = string.Empty;
            DateTime date = DateTime.Now;
            string l_str_rpt_bill_type = string.Empty;
            l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");
            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();
            objBillingInquiry.cmp_id = cmpid;
            objBillingInquiry.screentitle = p_str_screen_title;
            objBillingInquiry.bill_print_dt = Convert.ToDateTime(l_str_fm_dt);
            objBillingInquiry.bill_pd_fm = DateTime.Now.ToString("MM/dd/yyyy");
            objBillingInquiry.bill_as_of_date = DateTime.Now.ToString("MM/dd/yyyy");
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cust_of_cmp_id = "";
            //CR-3PL_MVC_IB_2018_0219_004 
            objCompany.cmp_id = cmpid;
            objCompany = ServiceObjectCompany.GetCustOfCompName(objCompany);
            objBillingInquiry.LstCustOfCmpName = objCompany.LstCustOfCmpName;
            objBillingInquiry.cust_of_cmpid = objBillingInquiry.LstCustOfCmpName[0].cust_of_cmpid;
            objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
            objBillingInquiry.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objBillingInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objBillingInquiry.cmp_id = cmpid;
            objBillingInquiry = ServiceObject.GetBillingBillingType(objBillingInquiry);
            l_str_rpt_bill_type = objBillingInquiry.ListBillingType[0].bill_type;
            objBillingInquiry.RateType = l_str_rpt_bill_type;
            Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
            BillingInquiryModel objVasInquirymodel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);
            return PartialView("_GenerateConsolidateStorageBill", objVasInquirymodel);
        }
        public ActionResult GenerateIBInoutBillDetails(string p_str_cmpid, string p_str_ib_doc_id, string p_str_screen_title)
        {
            string l_str_fm_dt = string.Empty;
            DateTime date = DateTime.Now;
            DateTime l_dt_rcvd_dt = DateTime.Now;
            l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");
            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();
            objBillingInquiry.cmp_id = p_str_cmpid;
            objBillingInquiry.ib_doc_id = p_str_ib_doc_id;
            objBillingInquiry.bill_print_dt = DateTime.Now;
            objBillingInquiry.bill_as_of_date = DateTime.Now.ToString("MM/dd/yyyy");
            objBillingInquiry.screentitle = p_str_screen_title;
            //objBillingInquiry = ServiceObject.GetDocRcvdDate(objBillingInquiry);
            //l_dt_rcvd_dt = objBillingInquiry.LstDocRcvdDt[0].rcvd_dt;
            //objBillingInquiry.inout_bill_pd_fm = l_dt_rcvd_dt.ToString("MM/dd/yyyy");
            //objBillingInquiry.inout_bill_pd_to = l_dt_rcvd_dt.ToString("MM/dd/yyyy");
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cust_of_cmp_id = "";
            objCompany.cmp_id = p_str_cmpid;
            objCompany = ServiceObjectCompany.GetCustOfCompName(objCompany);
            objBillingInquiry.LstCustOfCmpName = objCompany.LstCustOfCmpName;
            objBillingInquiry.cust_of_cmpid = objBillingInquiry.LstCustOfCmpName[0].cust_of_cmpid;

            objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
            objBillingInquiry.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objBillingInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objBillingInquiry.cust_id = p_str_cmpid;
            objBillingInquiry.cmp_id = p_str_cmpid;
            objBillingInquiry.ib_doc_id = p_str_ib_doc_id;

            Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
            BillingInquiryModel objVasInquirymodel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);
            return PartialView("_GenerateInOutBill", objVasInquirymodel);
        }
        public ActionResult LoadGenerateVASBillDetails(string cmpid)
        {
            string l_str_fm_dt = string.Empty;
            DateTime date = DateTime.Now;
            l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");
            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();
            objBillingInquiry.cmp_id = cmpid;
            objBillingInquiry.bill_print_dt = DateTime.Now;
            objBillingInquiry.bill_as_of_date = DateTime.Now.ToString("MM/dd/yyyy");
            objBillingInquiry.inout_bill_pd_fm = l_str_fm_dt;
            objBillingInquiry.inout_bill_pd_to = DateTime.Now.ToString("MM/dd/yyyy");
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cust_of_cmp_id = "";
            objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
            objBillingInquiry.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objBillingInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;

            Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
            BillingInquiryModel objVasInquirymodel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);
            return PartialView("_GenerateInOutBill", objVasInquirymodel);
        }

        public ActionResult GetSearchStrgBillInqDetails(string p_str_cmp_id, string p_str_cust_id, string p_str_print_dt, string p_str_bill_pd_fm, string p_str_as_of_date)
        {
            try
            {
                string l_str_bill_amount = string.Empty;
                BillingInquiry objBillingInquiry = new BillingInquiry();
                BillingInquiryService ServiceObject = new BillingInquiryService();
                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry.cust_id = p_str_cust_id;

                objBillingInquiry.bill_pd_fm = p_str_bill_pd_fm;
                objBillingInquiry.bill_pd_to = p_str_as_of_date;
                objBillingInquiry.bill_type = "STRG";
                objBillingInquiry = ServiceObject.GetBillingStrgRcvdDetails(objBillingInquiry); //CR-3PL_MVC_IB_2018-03-10-001 Added By Nithya
                for (int i = 0; i < objBillingInquiry.ListBillRcvdDetails.Count(); i++)
                {

                    l_int_bill_amount = l_int_bill_amount + Convert.ToDecimal(objBillingInquiry.ListBillRcvdDetails[i].bill_amt);
                    objBillingInquiry.tot_inv_strg_amnt = Convert.ToString(l_int_bill_amount);
                }
                Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
                BillingInquiryModel BillingInquiryModel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);

                return PartialView("_GenerateBillGrid", BillingInquiryModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //CR-3PL_MVC_IB_2018_0219_004 
        public ActionResult GetSearchInOutBillInqDetails(string p_str_cmp_id, string p_str_cust_id, string p_str_print_dt, string p_str_bill_pd_fm, string p_str_bill_pd_to, string p_str_ib_doc_id)
        {
            try
            {
                string l_str_bill_amount = string.Empty;
                BillingInquiry objBillingInquiry = new BillingInquiry();
                BillingInquiryService ServiceObject = new BillingInquiryService();
                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry.cust_id = p_str_cust_id;

                DateTime l_dt_rcvd_dt;

                objBillingInquiry.bill_pd_fm = p_str_bill_pd_fm;
                objBillingInquiry.bill_pd_to = p_str_bill_pd_to;
                objBillingInquiry.bill_type = "INOUT";
                if (p_str_ib_doc_id == "" || p_str_ib_doc_id == null)
                {
                    objBillingInquiry = ServiceObject.CheckInoutBillDocIdExisting(objBillingInquiry);
                    objBillingInquiry = ServiceObject.GetBillingRcvdDetails(objBillingInquiry);
                    for (int i = 0; i < objBillingInquiry.ListBillRcvdDetails.Count(); i++)
                    {

                        l_int_bill_amount = l_int_bill_amount + Convert.ToDecimal(objBillingInquiry.ListBillRcvdDetails[i].bill_amt);
                        objBillingInquiry.tot_inv_strg_amnt = Convert.ToString(l_int_bill_amount);
                    }
                }
                else
                {
                    objBillingInquiry.ib_doc_id = p_str_ib_doc_id;
                    objBillingInquiry = ServiceObject.CheckExsistBLDocIDFromLotHdr(objBillingInquiry);
                  
                    if (objBillingInquiry.ListCheckExistingInOutBillDocId.Count > 0)
                    {
                        objBillingInquiry.bill_doc_id = objBillingInquiry.ListCheckExistingInOutBillDocId[0].bill_doc_id.ToString();
                        objBillingInquiry = ServiceObject.GetDocRcvdDate(objBillingInquiry);
                        l_dt_rcvd_dt = objBillingInquiry.LstDocRcvdDt[0].rcvd_dt;

                        objBillingInquiry.bill_pd_fm = l_dt_rcvd_dt.ToString("MM/dd/yyyy");
                        objBillingInquiry.bill_pd_to = l_dt_rcvd_dt.ToString("MM/dd/yyyy");
                        objBillingInquiry = ServiceObject.GetBillingInoutRcvdDetails(objBillingInquiry);//CR-3PL_MVC_IB_2018-03-10-001 Added By Nithya
                        for (int i = 0; i < objBillingInquiry.ListBillRcvdDetails.Count(); i++)
                        {

                            l_int_bill_amount = l_int_bill_amount + Convert.ToDecimal(objBillingInquiry.ListBillRcvdDetails[i].bill_amt);
                            objBillingInquiry.tot_inv_strg_amnt = Convert.ToString(l_int_bill_amount);
                        }
                    }
                       
                     

                }


                Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
                BillingInquiryModel BillingInquiryModel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);

                return PartialView("_GrdGenerateInOutBill", BillingInquiryModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetSearchVASBillInqDtls(string p_str_cmpid, string p_str_cust_id, string p_str_print_dt, string p_str_vas_bill_pd_fm, string p_str_vas_bill_pd_to, string p_str_vas_id, string p_str_vas_dt, string p_str_screentitle)
        {
            try
            {
                DateTime l_dt_bill_vas_dt = DateTime.Now;
                string l_str_bill_amount = string.Empty;
                BillingInquiry objBillingInquiry = new BillingInquiry();
                BillingInquiryService ServiceObject = new BillingInquiryService();
                objBillingInquiry.cmp_id = p_str_cmpid;
                objBillingInquiry.cust_id = p_str_cust_id;
                objBillingInquiry.ship_doc_id = p_str_vas_id;
                objBillingInquiry.bill_pd_fm = p_str_vas_bill_pd_fm;
                objBillingInquiry.bill_pd_to = p_str_vas_bill_pd_to;
                objBillingInquiry.bill_type = "NORM";
                objBillingInquiry.ship_dt = Convert.ToDateTime(p_str_vas_dt);
                l_dt_bill_vas_dt = objBillingInquiry.ship_dt;
                string l_str_fm_dt = string.Empty;
                DateTime date = DateTime.Now;
                l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");

                objBillingInquiry.screentitle = p_str_screentitle;
                objBillingInquiry.bill_print_dt = DateTime.Now;
                objBillingInquiry.vas_bill_pd_to = DateTime.Now.ToString("MM/dd/yyyy");
                objBillingInquiry.vas_bill_pd_fm = l_str_fm_dt;
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                objCompany.cust_of_cmp_id = "";
                objCompany.cmp_id = p_str_cmpid;
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
                //objBillingInquiry.cust_of_cmpid = objBillingInquiry.LstCustOfCmpName[0].cust_of_cmpid;
                objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
                objBillingInquiry.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
                objCompany.user_id = Session["UserID"].ToString().Trim();
                objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                objBillingInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;


                objBillingInquiry.vas_bill_pd_fm = p_str_vas_dt;
                objBillingInquiry.vas_bill_pd_to = p_str_vas_dt;
                objBillingInquiry.bill_doc_id = p_str_vas_bill_pd_fm.Trim();
                objBillingInquiry = ServiceObject.GetBillingVasBillDetails(objBillingInquiry);
                for (int i = 0; i < objBillingInquiry.ListBillRcvdDetails.Count(); i++)
                {
                    l_int_bill_amount = l_int_bill_amount + Convert.ToDecimal(objBillingInquiry.ListBillRcvdDetails[i].bill_amt);
                    objBillingInquiry.tot_inv_strg_amnt = Convert.ToString(l_int_bill_amount);
                }

                Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
                BillingInquiryModel BillingInquiryModel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);

                return PartialView("_GenerateVASBill", BillingInquiryModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult GetSearchVASBillInqDetails(string p_str_cmp_id, string p_str_cust_id, string p_str_print_dt, string p_str_vas_bill_pd_fm, string p_str_vas_bill_pd_to, string p_str_vas_id, string p_str_vas_dt, string p_str_screentitle)
        {
            try
            {
                DateTime l_dt_bill_vas_dt = DateTime.Now;
                string l_str_bill_amount = string.Empty;
                BillingInquiry objBillingInquiry = new BillingInquiry();
                BillingInquiryService ServiceObject = new BillingInquiryService();
                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry.cust_id = p_str_cust_id;
                objBillingInquiry.ship_doc_id = p_str_vas_id;
                objBillingInquiry.bill_pd_fm = p_str_vas_bill_pd_fm;
                objBillingInquiry.bill_pd_to = p_str_vas_bill_pd_to;
                objBillingInquiry.bill_type = "NORM";
                objBillingInquiry.ship_dt = Convert.ToDateTime(p_str_vas_dt);
                l_dt_bill_vas_dt = objBillingInquiry.ship_dt;
                if (p_str_vas_id == "" || p_str_vas_id == null)
                {
                    objBillingInquiry = ServiceObject.CheckVASBillDocIdExisting(objBillingInquiry);
                    objBillingInquiry = ServiceObject.GetBillingRcvdDetails(objBillingInquiry);
                    for (int i = 0; i < objBillingInquiry.ListBillRcvdDetails.Count(); i++)
                    {

                        l_int_bill_amount = l_int_bill_amount + Convert.ToDecimal(objBillingInquiry.ListBillRcvdDetails[i].bill_amt);
                        objBillingInquiry.tot_inv_strg_amnt = Convert.ToString(l_int_bill_amount);
                    }
                }
                else
                {
                    objBillingInquiry.ship_doc_id = p_str_vas_id;
                    objBillingInquiry = ServiceObject.CheckExsistBLDocIDFromVasHdr(objBillingInquiry);
                    if (objBillingInquiry.Check_existing_bill_doc_id == "" || objBillingInquiry.Check_existing_bill_doc_id == null)
                    {
                        objBillingInquiry.bill_doc_id = "";
                    }
                    else
                    {
                        objBillingInquiry.bill_doc_id = objBillingInquiry.ListCheckExistingVasBillDocId[0].bill_doc_id.ToString();

                        objBillingInquiry.vas_bill_pd_fm = l_dt_bill_vas_dt.ToString("MM/dd/yyyy");
                        objBillingInquiry.vas_bill_pd_to = l_dt_bill_vas_dt.ToString("MM/dd/yyyy");
                        objBillingInquiry.cmp_id = p_str_cust_id;
                        objBillingInquiry = ServiceObject.GetBillingVasBillDetails(objBillingInquiry);
                        for (int i = 0; i < objBillingInquiry.ListBillRcvdDetails.Count(); i++)
                        {

                            l_int_bill_amount = l_int_bill_amount + Convert.ToDecimal(objBillingInquiry.ListBillRcvdDetails[i].bill_amt);
                            objBillingInquiry.tot_inv_strg_amnt = Convert.ToString(l_int_bill_amount);
                        }
                    }
                }
                Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
                BillingInquiryModel BillingInquiryModel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);

                return PartialView("_GrdGenerateVASBill", BillingInquiryModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public ActionResult SaveGenerateStrgBillDetails(string p_str_cmp_id, string p_str_cust_id, string p_str_print_dt, string p_str_as_of_date, string p_str_bill_pd_fm, string p_str_bill_doc_id)
        {
            try
            {
                string l_str_bill_doc_status = string.Empty;
                string l_str_bill_doc_id = string.Empty;
                string l_str_rpt_bill_type = string.Empty;
                BillingInquiry objBillingInquiry = new BillingInquiry();
                BillingInquiryService ServiceObject = new BillingInquiryService();
                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry.cust_id = p_str_cust_id;
                objBillingInquiry.bill_doc_id = p_str_bill_doc_id.Trim();
                //CR_3PL_MVC_BL_2018_0210_001 - Commented and modified by Ravikumar
                // objBillingInquiry.bill_print_dt = DateTime.Parse(p_str_print_dt);
                objBillingInquiry.print_bill_date = p_str_print_dt;
                // CR_3PL_2018_0210_001 - End
                objBillingInquiry.bill_as_of_date = p_str_as_of_date;
                objBillingInquiry.bill_pd_fm = p_str_bill_pd_fm;
                //objBillingInquiry.inout_bill_pd_to = l_dt_rcvd_dt.ToString("MM/dd/yyyy");
                objBillingInquiry = ServiceObject.CheckSTRGBillDocIdExisting(objBillingInquiry);

                //l_str_bill_doc_id = objBillingInquiry.ListCheckExistingSTRGBillDocId[0].bill_doc_id.ToString().Trim();
                objBillingInquiry.cmp_id = p_str_cust_id;
                objBillingInquiry = ServiceObject.GetBillingBillingType(objBillingInquiry);
                l_str_rpt_bill_type = objBillingInquiry.ListBillingType[0].bill_type;
                objBillingInquiry.cmp_id = p_str_cmp_id;
                if (objBillingInquiry.Check_existing_bill_doc_id == "")
                {
                    if (l_str_rpt_bill_type == "Carton")
                    {
                        // objBillingInquiry = ServiceObject.GenerateSTRGBill(objBillingInquiry);//CR-3PL_MVC_BL_2018_00312_001
                        objBillingInquiry = ServiceObject.GenerateSTRGBillCarton(objBillingInquiry);

                    }
                    else if (l_str_rpt_bill_type == "Cube")
                    {
                        // objBillingInquiry = ServiceObject.GenerateSTRGBillByCube(objBillingInquiry);//CR-3PL_MVC_BL_2018_00312_001
                        objBillingInquiry = ServiceObject.GenerateSTRGBillCube(objBillingInquiry);
                    }
                    else if (l_str_rpt_bill_type == "Pallet")

                    {
                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                        objBillingInquiry = ServiceObject.GenerateSTRGBillByPallet(objBillingInquiry);//CR-3PL_MVC_BL_2018_0224_001

                    }
                    else if (l_str_rpt_bill_type == "Location")

                    {
                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                        objBillingInquiry = ServiceObject.GenerateSTRGBillByLoc(objBillingInquiry);//CR-3PL_MVC_BL_2018_0224_001

                    }

                    else if (l_str_rpt_bill_type == "Pcs")

                    {
                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                        objBillingInquiry = ServiceObject.GenerateSTRGBillByPcs(objBillingInquiry);//CR-2018-05-21-001 Added By Nithya

                    }
                    objBillingInquiry = ServiceObject.CheckSTRGBillDocIdExisting(objBillingInquiry);
                    if (objBillingInquiry.ListCheckExistingSTRGBillDocId.Count == 0)
                    {
                        objBillingInquiry.Check_existing_bill_doc_id = "";
                        objBillingInquiry.Success = "N";
                        return Json(objBillingInquiry.Success, JsonRequestBehavior.AllowGet);
                    }

                    else
                    {
                        l_str_bill_doc_id = objBillingInquiry.ListCheckExistingSTRGBillDocId[0].bill_doc_id.ToString().Trim();
                    }
                    objBillingInquiry.bill_doc_id = l_str_bill_doc_id;
                    objBillingInquiry.Success = "O";
                }
                else
                {
                    l_str_bill_doc_status = objBillingInquiry.ListCheckExistingSTRGBillDocId[0].bill_doc_id_status.ToString().Trim();
                    l_str_bill_doc_id = objBillingInquiry.ListCheckExistingSTRGBillDocId[0].bill_doc_id.ToString().Trim();
                    if (l_str_bill_doc_status == "P")
                    {
                        objBillingInquiry.Success = "P";
                    }
                    else if (l_str_bill_doc_status == "O")
                    {
                        objBillingInquiry.Success = l_str_bill_doc_id;
                        objBillingInquiry.bill_doc_id = l_str_bill_doc_id;
                        ////objBillingInquiry = ServiceObject.DeleteExistingSTRGBillDocIdData(objBillingInquiry);
                        ////objBillingInquiry = ServiceObject.GenerateSTRGBill(objBillingInquiry);
                        ////objBillingInquiry.Success = "'" + objBillingInquiry.ship_doc_id + "' Posted Successfully ";
                    }
                }


                return Json(new { data1 = objBillingInquiry.Success, data2 = objBillingInquiry.bill_doc_id }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        //CR-3PL_MVC_IB_2018_0219_004 
        public ActionResult SaveGenerateInoutBillDetails(string p_str_cmp_id, string p_str_cust_id, string p_str_bill_as_of_dt, String p_str_print_dt, string p_str_inout_bill_pd_fm, string p_str_inout_bill_pd_to, string p_str_ib_doc_id)
        {
            try
            {
                string l_str_bill_doc_status = string.Empty;
                string l_str_bill_doc_id = string.Empty;
                string l_str_rpt_bill_type = string.Empty;
                string l_str_init_strg_rate = string.Empty;
                DateTime l_dt_rcvd_dt = DateTime.Now;

                BillingInquiry objBillingInquiry = new BillingInquiry();
                BillingInquiryService ServiceObject = new BillingInquiryService();
                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry.cust_id = p_str_cust_id;
                objBillingInquiry.ib_doc_id = p_str_ib_doc_id;
                //objBillingInquiry.bill_print_dt = DateTime.Parse(p_str_print_dt);
                //CR_3PL_MVC_BL_2018_0210_001 - Commented and modified by Ravikumar
                // objBillingInquiry.bill_print_dt = DateTime.Parse(p_str_print_dt);
                //l_dt_rcvd_dt = objBillingInquiry.LstDocRcvdDt[0].rcvd_dt;


                objBillingInquiry.print_bill_date = p_str_print_dt;
                objBillingInquiry.bill_as_of_date = p_str_bill_as_of_dt;
                // CR_3PL_2018_0210_001 - End            
                objBillingInquiry.inout_bill_pd_fm = p_str_inout_bill_pd_fm;
                objBillingInquiry.inout_bill_pd_to = p_str_inout_bill_pd_to;
                if (p_str_ib_doc_id == "" || p_str_ib_doc_id == null)
                {
                    objBillingInquiry = ServiceObject.CheckInoutBillDocIdExisting(objBillingInquiry);

                }
                else
                {
                    objBillingInquiry = ServiceObject.CheckExsistBLDocIDFromLotHdr(objBillingInquiry);
                    if (objBillingInquiry.Check_existing_bill_doc_id == "" || objBillingInquiry.Check_existing_bill_doc_id == null)
                    {
                        objBillingInquiry.Check_existing_bill_doc_id = "";
                    }
                    else
                    {
                        objBillingInquiry.Check_existing_bill_doc_id = objBillingInquiry.ListCheckExistingInOutBillDocId[0].bill_doc_id.ToString();
                    }
                    objBillingInquiry = ServiceObject.GetDocRcvdDate(objBillingInquiry);
                    l_dt_rcvd_dt = objBillingInquiry.LstDocRcvdDt[0].rcvd_dt;

                    objBillingInquiry.inout_bill_pd_fm = l_dt_rcvd_dt.ToString("MM/dd/yyyy");
                    objBillingInquiry.inout_bill_pd_to = l_dt_rcvd_dt.ToString("MM/dd/yyyy");
                    objBillingInquiry.bill_as_of_date = l_dt_rcvd_dt.ToString("MM/dd/yyyy");
                }
                //l_str_bill_doc_id = objBillingInquiry.ListCheckExistingInOutBillDocId[0].bill_doc_id.ToString().Trim();
                objBillingInquiry.cmp_id = p_str_cust_id;
                objBillingInquiry = ServiceObject.GetBillingInoutType(objBillingInquiry);
                l_str_rpt_bill_type = objBillingInquiry.ListBillingInoutType[0].bill_inout_type;
                l_str_init_strg_rate = objBillingInquiry.ListBillingInoutType[0].init_strg_rt_req;
                if (l_str_init_strg_rate == "Y")
                {
                    objBillingInquiry.init_strg_rt_req = "Y";
                }
                else
                {
                    objBillingInquiry.init_strg_rt_req = "";
                }
                objBillingInquiry.cmp_id = p_str_cmp_id;
                if (objBillingInquiry.Check_existing_bill_doc_id == "" || objBillingInquiry.Check_existing_bill_doc_id == null)
                {
                    if (p_str_ib_doc_id == "" || p_str_ib_doc_id == null)
                    {
                        if (l_str_rpt_bill_type == "Carton")
                        {
                            objBillingInquiry.BillFor = "Carton";
                            objBillingInquiry = ServiceObject.GenerateInOutBillByCarton(objBillingInquiry);
                            //objBillingInquiry = ServiceObject.CheckInoutBillDocIdExisting(objBillingInquiry);
                            l_str_bill_doc_id = objBillingInquiry.ListSaveSTRGBillDetails[0].bill_doc_id.ToString().Trim();
                            objBillingInquiry.Success = objBillingInquiry.ListSaveSTRGBillDetails[0].status.ToString().Trim();
                        }
                        else if (l_str_rpt_bill_type == "Cube")
                        {
                            objBillingInquiry.BillFor = "Cube";

                            objBillingInquiry = ServiceObject.GenerateInOutBillByCube(objBillingInquiry);
                            //objBillingInquiry = ServiceObject.CheckInoutBillDocIdExisting(objBillingInquiry);
                            l_str_bill_doc_id = objBillingInquiry.ListSaveSTRGBillDetails[0].bill_doc_id.ToString().Trim();
                            objBillingInquiry.Success = objBillingInquiry.ListSaveSTRGBillDetails[0].status.ToString().Trim();

                        }
                        //   CR - 20180217
                        else if (l_str_rpt_bill_type == "Container")

                        {
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                            objBillingInquiry.BillFor = "Container";

                            objBillingInquiry = ServiceObject.GenerateInoutBillByContainer(objBillingInquiry);
                            l_str_bill_doc_id = objBillingInquiry.ListGenBillingInoutByContainer[0].bill_doc_id.ToString().Trim();
                            objBillingInquiry.Success = objBillingInquiry.ListGenBillingInoutByContainer[0].status.ToString().Trim();
                            objBillingInquiry = ServiceObject.CheckExsistBLDocID(objBillingInquiry);
                        }


                    }
                    else
                    {
                        if (l_str_rpt_bill_type == "Carton")
                        {
                            objBillingInquiry.BillFor = "Carton";
                            objBillingInquiry = ServiceObject.GenerateInOutBillByCarton(objBillingInquiry);
                            //objBillingInquiry = ServiceObject.CheckExsistBLDocIDFromLotHdr(objBillingInquiry);
                            l_str_bill_doc_id = objBillingInquiry.ListSaveSTRGBillDetails[0].bill_doc_id.ToString().Trim();
                            objBillingInquiry.Success = objBillingInquiry.ListSaveSTRGBillDetails[0].status.ToString().Trim();

                        }
                        else if (l_str_rpt_bill_type == "Cube")
                        {
                            objBillingInquiry.BillFor = "Cube";

                            objBillingInquiry = ServiceObject.GenerateInOutBillByCube(objBillingInquiry);
                            //objBillingInquiry = ServiceObject.CheckExsistBLDocIDFromLotHdr(objBillingInquiry);
                            l_str_bill_doc_id = objBillingInquiry.ListSaveSTRGBillDetails[0].bill_doc_id.ToString().Trim();
                            objBillingInquiry.Success = objBillingInquiry.ListSaveSTRGBillDetails[0].status.ToString().Trim();
                        }
                        //   CR - 20180217
                        else if (l_str_rpt_bill_type == "Container")

                        {
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                            objBillingInquiry.BillFor = "Container";

                            objBillingInquiry = ServiceObject.GenerateInoutBillByContainer(objBillingInquiry);
                            //objBillingInquiry = ServiceObject.CheckExsistBLDocID(objBillingInquiry);
                            l_str_bill_doc_id = objBillingInquiry.ListGenBillingInoutByContainer[0].bill_doc_id.ToString().Trim();
                            objBillingInquiry.Success = objBillingInquiry.ListGenBillingInoutByContainer[0].status.ToString().Trim();
                        }


                    }
                    //if (objBillingInquiry.ListCheckExistingInOutBillDocId.Count == 0)
                    //{
                    //    objBillingInquiry.Check_existing_bill_doc_id = "";
                    //}
                    //else
                    //{
                    //    l_str_bill_doc_id = objBillingInquiry.ListCheckExistingInOutBillDocId[0].bill_doc_id.ToString().Trim();
                    //}

                    objBillingInquiry.bill_doc_id = l_str_bill_doc_id;
                    //objBillingInquiry.Success = "O";
                }
                else
                {
                    //l_str_bill_doc_status = objBillingInquiry.ListCheckExistingInOutBillDocId[0].bill_doc_id_status.ToString().Trim();
                    //l_str_bill_doc_id = objBillingInquiry.ListCheckExistingInOutBillDocId[0].bill_doc_id.ToString().Trim();

                    //if (l_str_bill_doc_status == "P")
                    //{
                    //    objBillingInquiry.Success = "P";
                    //}
                    //else if (l_str_bill_doc_status == "O")
                    //{
                    //objBillingInquiry.Success = l_str_bill_doc_id;
                    //objBillingInquiry.bill_doc_id = l_str_bill_doc_id;
                    ////objBillingInquiry = ServiceObject.DeleteExistingSTRGBillDocIdData(objBillingInquiry);
                    ////objBillingInquiry = ServiceObject.GenerateSTRGBill(objBillingInquiry);
                    ////objBillingInquiry.Success = "'" + objBillingInquiry.ship_doc_id + "' Posted Successfully ";
                    //}
                    objBillingInquiry.Success = "NA";
                    objBillingInquiry.bill_doc_id = objBillingInquiry.ListCheckExistingInOutBillDocId[0].bill_doc_id;
                }


                return Json(new { data1 = objBillingInquiry.Success, data2 = objBillingInquiry.bill_doc_id }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public ActionResult SaveGenerateVASBillDetails(string p_str_cmp_id, string p_str_cust_id, string p_str_bill_as_of_dt, String p_str_print_dt, string p_str_vas_bill_pd_fm, string p_str_vas_bill_pd_to, string p_str_vas_dt, string p_str_vas_id)
        {
            try
            {
                DateTime l_dt_bill_vas_dt = DateTime.Now;
                string l_str_bill_doc_status = string.Empty;
                string l_str_bill_doc_id = string.Empty;
                string l_str_rpt_bill_type = string.Empty;
                BillingInquiry objBillingInquiry = new BillingInquiry();
                BillingInquiryService ServiceObject = new BillingInquiryService();
                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry.cust_id = p_str_cust_id;
                objBillingInquiry.ship_doc_id = p_str_vas_id;
                // objBillingInquiry.bill_print_dt = DateTime.Parse(p_str_print_dt);
                objBillingInquiry.print_bill_date = p_str_print_dt;//CR-180421-001 Added By Nithya
                objBillingInquiry.vas_bill_pd_fm = p_str_vas_bill_pd_fm;
                objBillingInquiry.vas_bill_pd_to = p_str_vas_bill_pd_to;
                objBillingInquiry.ship_dt = Convert.ToDateTime(p_str_vas_dt);
                objBillingInquiry.bill_as_of_dt = p_str_bill_as_of_dt;
                l_dt_bill_vas_dt = objBillingInquiry.ship_dt;
                if (p_str_vas_id == "" || p_str_vas_id == null)
                {
                    objBillingInquiry = ServiceObject.CheckVASBillDocIdExisting(objBillingInquiry);
                }
                else
                {
                    objBillingInquiry = ServiceObject.CheckExsistBLDocIDFromVasHdr(objBillingInquiry);
                    if (objBillingInquiry.Check_existing_bill_doc_id == "" || objBillingInquiry.Check_existing_bill_doc_id == null)
                    {
                        objBillingInquiry.Check_existing_bill_doc_id = "";
                    }
                    else
                    {
                        objBillingInquiry.Check_existing_bill_doc_id = objBillingInquiry.ListCheckExistingVasBillDocId[0].bill_doc_id.ToString();
                    }


                    objBillingInquiry.vas_bill_pd_fm = l_dt_bill_vas_dt.ToString("MM/dd/yyyy");
                    objBillingInquiry.vas_bill_pd_to = l_dt_bill_vas_dt.ToString("MM/dd/yyyy");
                    objBillingInquiry.bill_as_of_dt = l_dt_bill_vas_dt.ToString("MM/dd/yyyy");
                }
                //objBillingInquiry.cmp_id = p_str_cust_id;
                //objBillingInquiry = ServiceObject.GetBillingInoutType(objBillingInquiry);
                //l_str_rpt_bill_type = objBillingInquiry.ListBillingInoutType[0].bill_inout_type;
                objBillingInquiry.cmp_id = p_str_cmp_id;
                if (objBillingInquiry.Check_existing_bill_doc_id == "")
                {

                    objBillingInquiry = ServiceObject.GenerateVASBill(objBillingInquiry);
                    //objBillingInquiry = ServiceObject.CheckVASBillDocIdExisting(objBillingInquiry);
                    l_str_bill_doc_id = objBillingInquiry.ListSaveVASBillDetails[0].bill_doc_id.ToString().Trim();
                    //objBillingInquiry.bill_doc_id = l_str_bill_doc_id;
                    objBillingInquiry.Success = objBillingInquiry.ListSaveVASBillDetails[0].bill_doc_id_status.ToString().Trim();
                    objBillingInquiry.bill_doc_id = l_str_bill_doc_id;
                    //objBillingInquiry.Success = "O";
                }
                else
                {
                    //l_str_bill_doc_status = objBillingInquiry.ListCheckExistingVASBillDocId[0].bill_doc_id_status.ToString().Trim();
                    //l_str_bill_doc_id = objBillingInquiry.ListCheckExistingVASBillDocId[0].bill_doc_id.ToString().Trim();
                    //if (l_str_bill_doc_status == "P")
                    //{
                    //    objBillingInquiry.Success = "P";
                    //}
                    //else if (l_str_bill_doc_status == "O")
                    //{
                    //    objBillingInquiry.Success = l_str_bill_doc_id;
                    //    objBillingInquiry.bill_doc_id = l_str_bill_doc_id;
                    //    ////objBillingInquiry = ServiceObject.DeleteExistingSTRGBillDocIdData(objBillingInquiry);
                    //    ////objBillingInquiry = ServiceObject.GenerateSTRGBill(objBillingInquiry);
                    //    ////objBillingInquiry.Success = "'" + objBillingInquiry.ship_doc_id + "' Posted Successfully ";
                    //}
                    objBillingInquiry.Success = "NA";
                }

                return Json(new { data1 = objBillingInquiry.Success, data2 = objBillingInquiry.bill_doc_id }, JsonRequestBehavior.AllowGet);

                //return Json(objBillingInquiry.Success, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }

        public ActionResult SaveStrgBillIDDetails(string p_str_cmp_id, string p_str_cust_id, string p_str_print_dt, string p_str_as_of_date, string p_str_bill_doc_id)
        {
            try
            {
                string l_str_bill_doc_status = string.Empty;
                string l_str_bill_doc_id = string.Empty;
                string l_str_rpt_bill_type = string.Empty;

                BillingInquiry objBillingInquiry = new BillingInquiry();
                BillingInquiryService ServiceObject = new BillingInquiryService();
                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry.cust_id = p_str_cust_id;
                //CR_3PL_MVC_BL_2018_0210_001 - Commented and modified by Ravikumar
                // objBillingInquiry.bill_print_dt = DateTime.Parse(p_str_print_dt);
                objBillingInquiry.print_bill_date = p_str_print_dt;
                // CR_3PL_2018_0210_001 - End
                objBillingInquiry.bill_as_of_date = p_str_as_of_date;
                objBillingInquiry.bill_doc_id = p_str_bill_doc_id;
                objBillingInquiry.cmp_id = p_str_cust_id;
                objBillingInquiry = ServiceObject.GetBillingBillingType(objBillingInquiry);
                l_str_rpt_bill_type = objBillingInquiry.ListBillingType[0].bill_type;
                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry = ServiceObject.CheckSTRGBillDocIdExisting(objBillingInquiry);

                l_str_bill_doc_status = objBillingInquiry.ListCheckExistingSTRGBillDocId[0].bill_doc_id_status.ToString().Trim();
                l_str_bill_doc_id = objBillingInquiry.ListCheckExistingSTRGBillDocId[0].bill_doc_id.ToString().Trim();
                if (l_str_bill_doc_status == "P")
                {
                    objBillingInquiry.Success = "P";
                }
                else if (l_str_bill_doc_status == "O")
                {
                    objBillingInquiry = ServiceObject.DeleteExistingSTRGBillDocIdData(objBillingInquiry);
                    if (l_str_rpt_bill_type == "Carton")
                    {
                        // objBillingInquiry = ServiceObject.GenerateSTRGBill(objBillingInquiry);//CR-3PL_MVC_BL_2018_00312_001
                        objBillingInquiry = ServiceObject.GenerateSTRGBillCarton(objBillingInquiry);
                    }
                    else if (l_str_rpt_bill_type == "Cube")
                    {
                        //objBillingInquiry = ServiceObject.GenerateSTRGBillByCube(objBillingInquiry);//CR-3PL_MVC_BL_2018_00312_001
                        objBillingInquiry = ServiceObject.GenerateSTRGBillCube(objBillingInquiry);
                    }
                    else if (l_str_rpt_bill_type == "Pallet")
                    {
                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                        objBillingInquiry = ServiceObject.GenerateSTRGBillByPallet(objBillingInquiry);//CR-3PL_MVC_BL_2018_0224_001

                    }
                    objBillingInquiry.Success = objBillingInquiry.bill_doc_id;
                }
                return Json(objBillingInquiry.Success, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        //CR-3PL_MVC_IB_2018_0219_004 
        public ActionResult SaveInOutBillIDDetails(string p_str_cmp_id, string p_str_cust_id, String p_str_print_dt, string p_str_inout_bill_pd_fm, string p_str_inout_bill_pd_to, string p_str_bill_doc_id, string p_str_ib_doc_id)
        {
            try
            {
                string l_str_bill_doc_status = string.Empty;
                string l_str_bill_doc_id = string.Empty;
                string l_str_rpt_bill_type = string.Empty;
                string l_str_init_strg_rate = string.Empty;
                DateTime l_dt_rcvd_dt;

                BillingInquiry objBillingInquiry = new BillingInquiry();
                BillingInquiryService ServiceObject = new BillingInquiryService();
                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry.cust_id = p_str_cust_id;
                objBillingInquiry.bill_print_dt = DateTime.Parse(p_str_print_dt);
                objBillingInquiry.ib_doc_id = p_str_ib_doc_id;


                objBillingInquiry.cmp_id = p_str_cust_id;
                objBillingInquiry = ServiceObject.GetBillingInoutType(objBillingInquiry);
                l_str_rpt_bill_type = objBillingInquiry.ListBillingInoutType[0].bill_inout_type;
                l_str_init_strg_rate = objBillingInquiry.ListBillingInoutType[0].init_strg_rt_req;

                //objBillingInquiry.cmp_id = p_str_cmp_id;
                //objBillingInquiry = ServiceObject.CheckInoutBillDocIdExisting(objBillingInquiry);
                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry.cust_id = p_str_cust_id;
                if (p_str_ib_doc_id == "" || p_str_ib_doc_id == null)
                {
                    objBillingInquiry = ServiceObject.CheckInoutBillDocIdExisting(objBillingInquiry);
                    objBillingInquiry.inout_bill_pd_fm = p_str_inout_bill_pd_fm;
                    objBillingInquiry.inout_bill_pd_to = p_str_inout_bill_pd_to;
                }
                else
                {
                    //objBillingInquiry = ServiceObject.CheckExsistBLDocID(objBillingInquiry);
                    objBillingInquiry = ServiceObject.CheckExsistBLDocIDFromLotHdr(objBillingInquiry);

                    objBillingInquiry.bill_doc_id = objBillingInquiry.ListCheckExistingInOutBillDocId[0].bill_doc_id.ToString();
                    objBillingInquiry = ServiceObject.GetDocRcvdDate(objBillingInquiry);
                    l_dt_rcvd_dt = objBillingInquiry.LstDocRcvdDt[0].rcvd_dt;

                    objBillingInquiry.inout_bill_pd_fm = l_dt_rcvd_dt.ToString("MM/dd/yyyy");
                    objBillingInquiry.inout_bill_pd_to = l_dt_rcvd_dt.ToString("MM/dd/yyyy");
                }
                l_str_bill_doc_status = objBillingInquiry.ListCheckExistingInOutBillDocId[0].bill_doc_id_status.ToString().Trim();
                l_str_bill_doc_id = objBillingInquiry.ListCheckExistingInOutBillDocId[0].bill_doc_id.ToString().Trim();
                if (l_str_init_strg_rate == "Y")
                {
                    objBillingInquiry.init_strg_rt_req = "Y";
                }
                else
                {
                    objBillingInquiry.init_strg_rt_req = "";
                }
                if (l_str_bill_doc_status == "P")
                {
                    objBillingInquiry.Success = "P";
                }
                else if (l_str_bill_doc_status == "O")
                {
                    objBillingInquiry.bill_doc_id = l_str_bill_doc_id;
                    objBillingInquiry = ServiceObject.DeleteExistingSTRGBillDocIdData(objBillingInquiry);
                    if (l_str_rpt_bill_type == "Carton")
                    {
                        objBillingInquiry.BillFor = "Carton";
                        objBillingInquiry = ServiceObject.GenerateInOutBillByCarton(objBillingInquiry);
                    }
                    else if (l_str_rpt_bill_type == "Cube")
                    {
                        objBillingInquiry.BillFor = "Cube";
                        objBillingInquiry = ServiceObject.GenerateInOutBillByCube(objBillingInquiry);
                    }
                    else if (l_str_rpt_bill_type == "Container")
                    {
                        objBillingInquiry.BillFor = "Container";
                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                        objBillingInquiry = ServiceObject.GenerateInoutBillByContainer(objBillingInquiry);
                    }
                    objBillingInquiry.Success = objBillingInquiry.bill_doc_id;
                }
                return Json(objBillingInquiry.Success, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }

        public ActionResult SaveVASBillIDDetails(string p_str_cmp_id, string p_str_cust_id, String p_str_print_dt, string p_str_vas_bill_pd_fm, string p_str_vas_bill_pd_to, string p_str_bill_doc_id)
        {
            try
            {
                string l_str_bill_doc_status = string.Empty;
                string l_str_bill_doc_id = string.Empty;
                //string l_str_rpt_bill_type = string.Empty;

                BillingInquiry objBillingInquiry = new BillingInquiry();
                BillingInquiryService ServiceObject = new BillingInquiryService();
                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry.cust_id = p_str_cust_id;
                objBillingInquiry.bill_print_dt = DateTime.Parse(p_str_print_dt);
                objBillingInquiry.vas_bill_pd_fm = p_str_vas_bill_pd_fm;
                objBillingInquiry.vas_bill_pd_to = p_str_vas_bill_pd_to;
                objBillingInquiry.cmp_id = p_str_cust_id;
                objBillingInquiry = ServiceObject.GetBillingInoutType(objBillingInquiry);
                //l_str_rpt_bill_type = objBillingInquiry.ListBillingInoutType[0].bill_inout_type;

                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry = ServiceObject.CheckVASBillDocIdExisting(objBillingInquiry);

                l_str_bill_doc_status = objBillingInquiry.ListCheckExistingVASBillDocId[0].bill_doc_id_status.ToString().Trim();
                l_str_bill_doc_id = objBillingInquiry.ListCheckExistingVASBillDocId[0].bill_doc_id.ToString().Trim();

                if (l_str_bill_doc_status == "P")
                {
                    objBillingInquiry.Success = "P";
                }
                else if (l_str_bill_doc_status == "O")
                {
                    objBillingInquiry.bill_doc_id = l_str_bill_doc_id;
                    objBillingInquiry = ServiceObject.DeleteExistingSTRGBillDocIdData(objBillingInquiry);

                    objBillingInquiry = ServiceObject.GenerateVASBill(objBillingInquiry);

                    objBillingInquiry.Success = objBillingInquiry.bill_doc_id;
                }
                return Json(objBillingInquiry.Success, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public JsonResult BILL_INQ_HDR_DATA(string p_str_cmp_id, string p_str_Bill_doc_id, string p_str_Bill_type, string p_str_doc_dt_Fr, string p_str_doc_dt_To)
        {
            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();
            Session["g_str_cmp_id"] = p_str_cmp_id.Trim();
            Session["TEMP_BL_CMP_ID"] = p_str_cmp_id.Trim();
            Session["TEMP_BL_DOC_ID"] = p_str_Bill_doc_id.Trim();
            Session["TEMP_BL_DT_FM"] = p_str_doc_dt_Fr.Trim();
            Session["TEMP_BL_DT_TO"] = p_str_doc_dt_To.Trim();
            Session["TEMP_BL_TYPE"] = p_str_Bill_type.Trim();
            return Json(objBillingInquiry, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BillingInquiryDtl(string p_str_cmp_id, string p_str_Bill_doc_id)
        {
            try
            {

                BillingInquiry objBillingInquiry = new BillingInquiry();
                BillingInquiryService ServiceObject = new BillingInquiryService();
                string l_str_search_flag = string.Empty;
                string l_str_is_another_usr = string.Empty;

                l_str_is_another_usr = Session["IS3RDUSER"].ToString();
                objBillingInquiry.IS3RDUSER = l_str_is_another_usr.Trim();
                l_str_search_flag = Session["g_str_Search_flag"].ToString().Trim();
                objBillingInquiry.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                if (l_str_search_flag == "True")
                {
                    objBillingInquiry.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                    objBillingInquiry.cmp_id = Session["TEMP_BL_CMP_ID"].ToString().Trim();
                    objBillingInquiry.Bill_doc_id = Session["TEMP_BL_DOC_ID"].ToString().Trim();
                    objBillingInquiry.Bill_doc_dt_Fr = Session["TEMP_BL_DT_FM"].ToString().Trim();
                    objBillingInquiry.Bill_doc_dt_To = Session["TEMP_BL_DT_TO"].ToString().Trim();
                    objBillingInquiry.Bill_type = Session["TEMP_BL_TYPE"].ToString().Trim();
                    objBillingInquiry = ServiceObject.GetBillingInquiryDetails(objBillingInquiry);

                }
                else
                {
                    objBillingInquiry.cmp_id = p_str_cmp_id;
                    if (p_str_Bill_doc_id != "")
                    {
                        objBillingInquiry.Bill_doc_id = p_str_Bill_doc_id;
                        objBillingInquiry = ServiceObject.GetBillingInquiryDetails(objBillingInquiry);
                    }
                    else
                    {
                        objBillingInquiry.Bill_doc_id = p_str_Bill_doc_id;
                        objBillingInquiry = ServiceObject.GetBillingInquiryDetails(objBillingInquiry);
                    }

                }

                Session["g_str_Search_flag"] = "False";
                Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
                BillingInquiryModel BillingInquiryModel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);
                return PartialView("_BillingInquiry", BillingInquiryModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult GetSearchInboundInquiry(string p_str_cmp_id, string p_str_Bill_doc_id, string p_str_Bill_type, string p_str_doc_dt_Fr, string p_str_doc_dt_To)
        {
            try
            {
                BillingInquiry objBillingInquiry = new BillingInquiry();
                BillingInquiryService ServiceObject = new BillingInquiryService();
                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                Session["g_str_Search_flag"] = "True";
                objBillingInquiry.Bill_doc_id = p_str_Bill_doc_id;
                objBillingInquiry.Bill_type = p_str_Bill_type;
                objBillingInquiry.Bill_doc_dt_Fr = p_str_doc_dt_Fr;
                objBillingInquiry.Bill_doc_dt_To = p_str_doc_dt_To;
                objBillingInquiry = ServiceObject.GetBillingInquiryDetails(objBillingInquiry);
                Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
                BillingInquiryModel BillingInquiryModel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);
                return PartialView("_BillingInquiry", BillingInquiryModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult GetBillingGridSummaryRpt(string p_str_radio, string p_str_cmp_id, string p_str_Bill_doc_id, string p_str_Bill_type, string p_str_doc_dt_Fr, string p_str_doc_dt_To)
        {
            try
            {
                BillingInquiry objBillingInquiry = new BillingInquiry();
                BillingInquiryService ServiceObject = new BillingInquiryService();

                //  objOrderLifeCycle = ServiceObject.GetEShipGetTrackId(objEShip);
                TempData["p_str_cmp_id"] = p_str_cmp_id;
                TempData["p_str_Bill_doc_id"] = p_str_Bill_doc_id;
                TempData["p_str_Bill_type"] = p_str_Bill_type;
                TempData["p_str_doc_dt_Fr"] = p_str_doc_dt_Fr;
                TempData["p_str_doc_dt_To"] = p_str_doc_dt_To;

                TempData["ReportSelection"] = p_str_radio;

                return Json(p_str_cmp_id, JsonRequestBehavior.AllowGet);


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
        public ActionResult GetRptDtls(string SelectedIDs, string p_str_cmp_id, string p_str_radio, string p_str_Bill_type)

        {
            try
            {
                InboundInquiry objInboundInquiry = new InboundInquiry();
                InboundInquiryService ServiceObject = new InboundInquiryService();
                objInboundInquiry.cmp_id = p_str_cmp_id;
                objInboundInquiry.ib_doc_id = SelectedIDs;
                //  objOrderLifeCycle = ServiceObject.GetEShipGetTrackId(objEShip);
                TempData["cmpid"] = p_str_cmp_id;
                TempData["bill_doc_id"] = SelectedIDs;
                TempData["ReportSelection"] = p_str_radio;
                TempData["Rpt_Bill_type"] = p_str_Bill_type;
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
                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");

                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }

                    //}
                    if (l_str_rpt_bill_type.Trim() == "Cube")

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
                                rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");

                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        //}

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
                        //if (l_str_rpt_instrg_req.Trim() == "Y")
                        //{
                        //    strReportName = "rpt_inout_bill_doc_with_initstrg.rpt";
                        //    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        //    objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                        //    objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                        //    objBillingInquiry = ServiceObject.GetBillingBillInoutCartonInstrgRpt(objBillingInquiry);
                        //    var rptSource = objBillingInquiry.ListBillingInoutCartonInstrgRpt.ToList();
                        //    if (rptSource.Count > 0)
                        //    {
                        //        using (ReportDocument rd = new ReportDocument())
                        //        {
                        //            rd.Load(strRptPath);
                        //            int AlocCount = 0;
                        //            AlocCount = objBillingInquiry.ListBillingInoutCartonInstrgRpt.Count();
                        //            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                        //                rd.SetDataSource(rptSource);
                        //            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        //            rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                        //            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        //        }
                        //    }
                        //}
                        //else
                        //{
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

                                rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                                rd.SetParameterValue("fml_rpt_bill_title", "(INOUT BILL BY CARTON)");
                                rd.SetParameterValue("fml_rpt_param_bill_num", "Bill#");
                                rd.SetParameterValue("fml_rpt_param_bill_date", "Bill Dt");
                                DataTable dtCtns = new DataTable();
                                int lintTotCtns = 0;
                                int lintTotIBSQty = 0;
                                dtCtns = ServiceObject.fnGetCtnsByBillDoc(p_str_bill_doc_id);
                                if (dtCtns.Rows.Count > 0)
                                {
                                    lintTotCtns = Convert.ToInt32(dtCtns.Rows[0]["TotCtns"]);
                                    lintTotIBSQty = Convert.ToInt32(dtCtns.Rows[0]["TotIBSQty"]);
                                }
                                rd.SetParameterValue("fmlTotCtns", lintTotCtns);
                                rd.SetParameterValue("fmlTotIBSQty", lintTotIBSQty);

                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            }
                        }
                        //}

                    }
                    if (l_str_rpt_bill_inout_type.Trim() == "Cube")

                    {
                        //if (l_str_rpt_instrg_req.Trim() == "Y")
                        //{
                        //    strReportName = "rpt_inout_bill_doc_bycube_with_initstrg.rpt";
                        //    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        //    objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                        //    objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                        //    objBillingInquiry = ServiceObject.GetBillingBillInoutCubeInstrgRpt(objBillingInquiry);
                        //    var rptSource = objBillingInquiry.ListBillingInoutCubeInstrgRpt.ToList();
                        //    if (rptSource.Count > 0)
                        //    {
                        //        using (ReportDocument rd = new ReportDocument())
                        //        {
                        //            rd.Load(strRptPath);
                        //            int AlocCount = 0;
                        //            AlocCount = objBillingInquiry.ListBillingInoutCubeInstrgRpt.Count();
                        //            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                        //                rd.SetDataSource(rptSource);
                        //            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        //            rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                        //            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        //        }
                        //    }
                        //}
                        //else
                        //{
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
                                rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                                rd.SetParameterValue("fml_rpt_bill_title", "(INBOUND BILL BY CUBE)");
                                rd.SetParameterValue("fml_rpt_param_bill_num", "Bill#");
                                rd.SetParameterValue("fml_rpt_param_bill_date", "Bill Dt");
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            }
                        }
                        //}

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
        public ActionResult CheckStatus(string SelectedID, string p_str_cmp_id, string p_str_radio, string p_str_Bill_doc_id, string p_str_Bill_type, string p_str_doc_dt_Fr, string p_str_doc_dt_To)
        {
            string l_str_status = string.Empty;
            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();
            objBillingInquiry.cmp_id = p_str_cmp_id;
            objBillingInquiry.Bill_doc_id = SelectedID;
            objBillingInquiry = ServiceObject.GetBillingInvStaus(objBillingInquiry);
            l_str_status = objBillingInquiry.ListBillingInvStatus[0].InvoiceStatus;
            return Json(l_str_status, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShowReport(string p_str_radio, string p_str_cmp_id, string p_str_Bill_doc_id, string p_str_Bill_type, string p_str_doc_dt_Fr, string p_str_doc_dt_To, string SelectdID, string type)

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
            decimal l_dec_tot_amnt = 0;
            decimal l_dec_list_price = 0;
            int l_int_tot_qty = 0;
            decimal l_int_tot_ship_qty = 0;
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

                        #region l_str_rpt_bill_doc_type == "STRG"
                        if (l_str_rpt_bill_doc_type == "STRG")
                        {
                            if (l_str_rpt_bill_type.Trim() == "Carton")

                            {

                                    if (type == "PDF")
                                    {
                                        strReportName = "rpt_st_bill_doc.rpt";
                                    }
                                    else
                                    {
                                        strReportName = "rpt_st_bill_doc_Excel.rpt";
                                    }

                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry = ServiceObject.GetBillingBillDocSTRGRpt(objBillingInquiry);
                                    var rptSource = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.ToList();

                                    if (type == "PDF")
                                    {
                                        if (rptSource.Count > 0)
                                        {
                                            using (ReportDocument rd = new ReportDocument())
                                            {
                                                rd.Load(strRptPath);
                                                rd.SetDataSource(rptSource);
                                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                            rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG Carton");
                                            }
                                        }
                                    }

                                    else if (type == "XLS")
                                    {
                                        List<BILLING_STRG_BILLDOC_CRTN_EXCEL> li = new List<BILLING_STRG_BILLDOC_CRTN_EXCEL>();
                                        for (int i = 0; i < objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.Count; i++)
                                        {

                                            BILLING_STRG_BILLDOC_CRTN_EXCEL objOBInquiryExcel = new BILLING_STRG_BILLDOC_CRTN_EXCEL();
                                            objOBInquiryExcel.LineNo = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].dtl_line;
                                            objOBInquiryExcel.Desc = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].itm_name;
                                            objOBInquiryExcel.Style = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].so_itm_num;
                                            objOBInquiryExcel.Color = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].so_itm_color;
                                            objOBInquiryExcel.Size = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].so_itm_size;
                                            objOBInquiryExcel.Ctns = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].ship_ctns;
                                            objOBInquiryExcel.Rate = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].so_itm_price;
                                            objOBInquiryExcel.Amount = (objOBInquiryExcel.Ctns) * (objOBInquiryExcel.Rate);

                                            li.Add(objOBInquiryExcel);
                                        }

                                        GridView gv = new GridView();
                                        gv.DataSource = li;
                                        gv.DataBind();
                                        Session["BILL_DOC"] = gv;
                                        return new DownloadFileActionResult((GridView)Session["BILL_DOC"], "BILL_DOC" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
                                    }
                                    else
                                    {
                                        Response.Write("Please select the mode!");
                                    }
                                //}

                            }

                            if (l_str_rpt_bill_type.Trim() == "Cube")

                            {

                                    if (type == "PDF")
                                    {
                                        strReportName = "rpt_st_bill_doc_bycube.rpt";
                                    }
                                    else
                                    {
                                        strReportName = "rpt_st_bill_doc_bycube_Excel.rpt";
                                    }
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;

                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 


                                    if (type == "PDF")
                                    {
                                        objBillingInquiry = ServiceObject.GetBillingBillDocCubeSTRGRpt(objBillingInquiry);
                                        var rptSource = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.ToList();
                                        if (rptSource.Count > 0)
                                        {
                                            using (ReportDocument rd = new ReportDocument())
                                            {
                                                rd.Load(strRptPath);

                                                rd.SetDataSource(rptSource);
                                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                            rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG Cube");
                                            }
                                        }
                                    }

                                    else if (type == "XLS")
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
                                    else
                                    {
                                        Response.Write("Please select the mode!");
                                    }

                                //}

                            }
                            //  CR_3PL_MVC_BL_2018_0226_001 – Add Starage Bill By Pallet Report
                            if (l_str_rpt_bill_type.Trim() == "Pallet")
                            {


                                if (type == "PDF")
                                {
                                    strReportName = "rpt_strg_bill_by_pallet.rpt";
                                }
                                else
                                {
                                    strReportName = "rpt_strg_bill_by_pallet_Excel.rpt";
                                }
                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                                objBillingInquiry.Bill_doc_id = SelectdID.Trim();
                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                objBillingInquiry = ServiceObject.GetBillingBillDocPalletSTRGRpt(objBillingInquiry);
                                var rptSource = objBillingInquiry.ListGenBillingStrgByPalletRpt.ToList();

                                objBillingInquiry.itm_num = "STRG-2";
                                objBillingInquiry = ServiceObject.GetSecondSTRGRate(objBillingInquiry);


                                if (type == "PDF")
                                {
                                    if (rptSource.Count > 0)
                                    {
                                        using (ReportDocument rd = new ReportDocument())
                                        {
                                            rd.Load(strRptPath);
                                            rd.SetDataSource(rptSource);
                                            if (objBillingInquiry.sec_strg_rate == "1")
                                            {
                                                rd.SetParameterValue("fml_rep_itm_num", objBillingInquiry.ListGetSecondSTRGRate[0].itm_num);
                                                rd.SetParameterValue("fml_rep_list_price", objBillingInquiry.ListGetSecondSTRGRate[0].list_price);
                                                rd.SetParameterValue("fml_rep_price_uom", objBillingInquiry.ListGetSecondSTRGRate[0].price_uom);
                                            }
                                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG Pallet");
                                        }
                                    }
                                }

                                else if (type == "XLS")
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
                                else
                                {
                                    Response.Write("Please select the mode!");
                                }
                            }
                            if (l_str_rpt_bill_type.Trim() == "Location")
                            {

                                if (type == "PDF")
                                {
                                    strReportName = "rpt_st_bill_doc_location.rpt";
                                }
                                else
                                {
                                    strReportName = "rpt_st_bill_doc_location_Excel.rpt";
                                }

                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                                objBillingInquiry.Bill_doc_id = SelectdID.Trim();
                                objBillingInquiry.bill_doc_id = SelectdID.Trim();
                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 


                                if (type == "PDF")
                                {
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
                                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG Location");
                                        }
                                    }
                                }

                                else if (type == "XLS")
                                {

                                    objBillingInquiry = ServiceObject.STRGBillLocationRpt(objBillingInquiry);
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
                                else
                                {
                                    Response.Write("Please select the mode!");
                                }
                            }

                            if (l_str_rpt_bill_type.Trim() == "Pcs")
                            {
                                if (type == "PDF")
                                {
                                    strReportName = "rpt_st_bill_doc_Pcs_item.rpt";
                                }
                                else
                                {
                                    strReportName = "rpt_st_bill_doc_Pcs_item_Excel.rpt";
                                }
                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                                objBillingInquiry.Bill_doc_id = SelectdID.Trim();
                                objBillingInquiry.bill_doc_id = SelectdID.Trim();
                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                                if (type == "PDF")
                                {
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
                                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG Pcs");
                                        }
                                    }
                                }
                                else

                                {
                                    objBillingInquiry = ServiceObject.STRGBillPcsRpt(objBillingInquiry);
                                    List<BILLING_STRG_BILLDOC_PCS_EXCEL> li = new List<BILLING_STRG_BILLDOC_PCS_EXCEL>();
                                    for (int i = 0; i < objBillingInquiry.ListGetSTRGBillByPcsRpt.Count; i++)
                                    {

                                        BILLING_STRG_BILLDOC_PCS_EXCEL objOBInquiryExcel = new BILLING_STRG_BILLDOC_PCS_EXCEL();
                                        objOBInquiryExcel.BillDocId = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].bill_doc_id;
                                        objOBInquiryExcel.Line = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].dtl_line;
                                        objOBInquiryExcel.Desc = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].ship_itm_name;
                                        objOBInquiryExcel.Style = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].so_itm_num;
                                        objOBInquiryExcel.Color = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].so_itm_color;
                                        objOBInquiryExcel.Size = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].so_itm_size;
                                        objOBInquiryExcel.Qty = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].ship_qty;
                                        objOBInquiryExcel.Rate = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].so_itm_price;
                                        l_dec_list_price = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].so_itm_price;
                                        l_int_tot_ship_qty = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].ship_qty;
                                        l_dec_tot_amnt = l_dec_list_price * l_int_tot_ship_qty;
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
                        #endregion
                        #region l_str_rpt_bill_doc_type == "NORM"
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
                            //string l_str_cmp_id = string.Empty;
                            //l_str_cmp_id = objBillingInquiry.cmp_id;
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
                                if (type == "PDF")
                                {
                                    strReportName = "rpt_va_bill_doc.rpt";
                                }
                                else
                                {
                                    strReportName = "rpt_va_bill_doc_Excel.rpt";
                                }
                            }

                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id;
                            objBillingInquiry.Bill_doc_id = SelectdID;
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0421_001 

                            if (type == "PDF")
                            {
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

                                        if (strReportName == "rpt_va_bill_doc_Excel.rpt")
                                        {
                                            //Functionality - Parameter field to change Invoice/Bill should be handled 
                                        }
                                        else
                                        {
                                            rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                                            rd.SetParameterValue("fml_rpt_bill_title", "(VAS BILL)");
                                            rd.SetParameterValue("fml_rpt_param_bill_num", "Bill#");
                                            rd.SetParameterValue("fml_rpt_param_bill_date", "Bill Dt");
                                        }


                                        if (l_str_cmp_id == "FHNJ")
                                        {
                                            if (objCompany.cmp_id.Trim() == "SJOE")
                                            {

                                            }
                                            else
                                            {
                                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                                            }
                                        }
                                        else
                                        {
                                            rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                        }

                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document Vas");
                                    }
                                }
                            }

                            else if (type == "XLS")
                            {
                                objBillingInquiry = ServiceObject.GetBillingBillDocVASRpt(objBillingInquiry);
                                List<BILLING_NORM_BILLDOC_EXCEL> li = new List<BILLING_NORM_BILLDOC_EXCEL>();
                                for (int i = 0; i < objBillingInquiry.ListBillingDocVASRpt.Count; i++)
                                {

                                    BILLING_NORM_BILLDOC_EXCEL objOBInquiryExcel = new BILLING_NORM_BILLDOC_EXCEL();
                                    objOBInquiryExcel.Line = objBillingInquiry.ListBillingDocVASRpt[i].dtl_line;
                                    objOBInquiryExcel.VASId = objBillingInquiry.ListBillingDocVASRpt[i].ship_doc_id;
                                    objOBInquiryExcel.ShipDate = objBillingInquiry.ListBillingDocVASRpt[i].ship_dt.ToString("MM/dd/yyyy");
                                    objOBInquiryExcel.CustOrder = objBillingInquiry.ListBillingDocVASRpt[i].CustOrder;
                                    objOBInquiryExcel.Notes = objBillingInquiry.ListBillingDocVASRpt[i].Notes;
                                    objOBInquiryExcel.VASDesc = objBillingInquiry.ListBillingDocVASRpt[i].ship_itm_num;
                                    objOBInquiryExcel.Ctns = objBillingInquiry.ListBillingDocVASRpt[i].ship_ctns;
                                    objOBInquiryExcel.Rate = objBillingInquiry.ListBillingDocVASRpt[i].so_itm_price;
                                    objOBInquiryExcel.Amount = (objOBInquiryExcel.Ctns) * (objOBInquiryExcel.Rate);
                                    li.Add(objOBInquiryExcel);
                                }

                                GridView gv = new GridView();
                                gv.DataSource = li;
                                gv.DataBind();
                                Session["BILL_DOC"] = gv;
                                return new DownloadFileActionResult((GridView)Session["BILL_DOC"], "BILL_DOC" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
                            }
                            else
                            {
                                Response.Write("Please select the mode!");
                            }
                        }
                        #endregion
                        #region l_str_rpt_bill_doc_type == "INOUT"
                        if (l_str_rpt_bill_doc_type == "INOUT")
                        {

                            if (l_str_rpt_bill_inout_type.Trim() == "Carton")

                            {

                                    if (type == "PDF")
                                    {
                                        strReportName = "rpt_inout_bill_doc.rpt";
                                    }
                                    else
                                    {
                                        strReportName = "rpt_inout_bill_doc_Excel.rpt";
                                    }

                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry = ServiceObject.GetBillingBillInoutCartonRpt(objBillingInquiry);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    var rptSource = objBillingInquiry.ListBillingInoutCartonRpt.ToList();
                             



                                    if (type == "PDF")
                                    {
                                        if (rptSource.Count > 0)
                                        {
                                            using (ReportDocument rd = new ReportDocument())
                                            {
                                                rd.Load(strRptPath);
                                                rd.SetDataSource(rptSource);
                                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                                if (strReportName == "rpt_inout_bill_doc_Excel.rpt")
                                                {
                                                    //Functionality Should be implemented
                                                }
                                                else
                                                {
                                                DataTable dtCtns = new DataTable();
                                                int lintTotCtns = 0;
                                                int lintTotIBSQty = 0;
                                                dtCtns = ServiceObject.fnGetCtnsByBillDoc(SelectdID);
                                                if (dtCtns.Rows.Count > 0)
                                                {
                                                    lintTotCtns = Convert.ToInt32(dtCtns.Rows[0]["TotCtns"]);
                                                    lintTotIBSQty = Convert.ToInt32(dtCtns.Rows[0]["TotIBSQty"]);
                                                }
                                                rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                                                    rd.SetParameterValue("fml_rpt_bill_title", "(INOUT BILL BY CARTON)");
                                                    rd.SetParameterValue("fml_rpt_param_bill_num", "Bill#");
                                                    rd.SetParameterValue("fml_rpt_param_bill_date", "Bill Dt");
                                                rd.SetParameterValue("fmlTotCtns", lintTotCtns);
                                                rd.SetParameterValue("fmlTotIBSQty", lintTotIBSQty);
                                            }
                                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document INout Carton");
                                            }
                                        }
                                    }

                                    else if (type == "XLS")
                                    {
                                        objBillingInquiry = ServiceObject.GetInOutBillCarton(objBillingInquiry);
                                        objBillingInquiry = ServiceObject.GetBillingBillInoutCartonRpt(objBillingInquiry);
                                        List<BILLING_INOUT_BILLDOC_CTN_EXCEL> li = new List<BILLING_INOUT_BILLDOC_CTN_EXCEL>();
                                        for (int i = 0; i < objBillingInquiry.ListBillingInoutCartonRpt.Count; i++)
                                        {

                                            BILLING_INOUT_BILLDOC_CTN_EXCEL objOBInquiryExcel = new BILLING_INOUT_BILLDOC_CTN_EXCEL();
                                            objOBInquiryExcel.Line = objBillingInquiry.ListBillingInoutCartonRpt[i].dtl_line;
                                            objOBInquiryExcel.ContId = objBillingInquiry.ListBillingInoutCartonRpt[i].cont_id;
                                            objOBInquiryExcel.LotId = objBillingInquiry.ListBillingInoutCartonRpt[i].lot_id;
                                            objOBInquiryExcel.ServiceId = objBillingInquiry.ListBillingInoutCartonRpt[i].ship_itm_num;
                                            objOBInquiryExcel.ContId = objBillingInquiry.ListBillingInoutCartonRpt[i].cont_id;
                                            objOBInquiryExcel.LotId = objBillingInquiry.ListBillingInoutCartonRpt[i].lot_id;
                                            objOBInquiryExcel.Style = objBillingInquiry.ListBillingInoutCartonRpt[i].so_itm_num;
                                            objOBInquiryExcel.Color = objBillingInquiry.ListBillingInoutCartonRpt[i].so_itm_color;
                                            objOBInquiryExcel.Size = objBillingInquiry.ListBillingInoutCartonRpt[i].so_itm_size;
                                            objOBInquiryExcel.Rate = objBillingInquiry.ListBillingInoutCartonRpt[i].so_itm_price;
                                            objOBInquiryExcel.Ctns = objBillingInquiry.ListBillingInoutCartonRpt[i].ship_ctns;
                                            objOBInquiryExcel.TotalAmount = objBillingInquiry.ListBillingInoutCartonRpt[i].bill_amt;


                                            li.Add(objOBInquiryExcel);
                                        }

                                        GridView gv = new GridView();
                                        gv.DataSource = li;
                                        gv.DataBind();
                                        Session["BILL_DOC"] = gv;
                                        return new DownloadFileActionResult((GridView)Session["BILL_DOC"], "BILL_DOC_" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");

                                    }
                                    else
                                    {
                                        Response.Write("Please select the mode!");
                                    }
                                //}

                            }
                            if (l_str_rpt_bill_inout_type.Trim() == "Cube")

                            {
                                
                                    if (type == "PDF")
                                    {
                                        strReportName = "rpt_inout_bill_doc_bycube.rpt";
                                    }
                                    else
                                    {
                                        strReportName = "rpt_inout_bill_doc_bycube_Excel.rpt";
                                    }
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry = ServiceObject.GetBillingBillInoutCubeRpt(objBillingInquiry);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 


                                    if (type == "PDF")
                                    {
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
                                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                                                if (strReportName == "rpt_inout_bill_doc_bycube_Excel.rpt")
                                                { }
                                                else
                                                {
                                                    rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                                                    rd.SetParameterValue("fml_rpt_bill_title", "(INBOUND BILL BY CUBE)");
                                                    rd.SetParameterValue("fml_rpt_param_bill_num", "Bill#");
                                                    rd.SetParameterValue("fml_rpt_param_bill_date", "Bill Dt");
                                                }
                                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document INout Cube");
                                            }
                                        }
                                    }
                                    else if (type == "XLS")
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

                                    else
                                    {
                                        Response.Write("Please select the mode!");
                                    }
                                //}

                            }
                            // CR - 3PL_MVC_IB_2018_0219_004

                            if (l_str_rpt_bill_inout_type.Trim() == "Container")

                            {
                                if (type == "PDF")
                                {
                                    strReportName = "rpt_inout_bill_by_Container.rpt";
                                }
                                else
                                {
                                    strReportName = "rpt_inout_bill_by_Container_Excel.rpt";
                                }
                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                objBillingInquiry.cmp_id = p_str_cmp_id;
                                objBillingInquiry.Bill_doc_id = SelectdID;
                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                objBillingInquiry = ServiceObject.GetBillingBillDocContainerInoutRpt(objBillingInquiry);
                                var rptSource = objBillingInquiry.ListGenBillingInoutByContainerRpt.ToList();


                                if (type == "PDF")
                                {
                                    if (rptSource.Count > 0)
                                    {
                                        using (ReportDocument rd = new ReportDocument())
                                        {
                                            rd.Load(strRptPath);
                                            rd.SetDataSource(rptSource);
                                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document INout Cntr");
                                        }
                                    }
                                }

                                else if (type == "Excel")
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
                                else
                                {
                                    Response.Write("Please select the mode!");
                                }
                            }
                        }

                    }
                    #endregion

                    if (l_str_rpt_selection == "BillInvoice")
                    {
                        BillingInquiry objBillingInquiry = new BillingInquiry();
                        BillingInquiryService ServiceObject = new BillingInquiryService();
                        objBillingInquiry.cmp_id = p_str_cmp_id;
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
                        objBillingInquiry.Bill_doc_id = SelectdID;
                        objBillingInquiry = ServiceObject.GetBillingBillDocIdType(objBillingInquiry);
                        l_str_rpt_bill_doc_type = objBillingInquiry.ListBillingDocIdType[0].bill_type;
                        objBillingInquiry = ServiceObject.GetBillingBillingType(objBillingInquiry);
                        l_str_rpt_bill_type = objBillingInquiry.ListBillingType[0].bill_type;
                        objBillingInquiry = ServiceObject.GetBillingInoutType(objBillingInquiry);
                        l_str_rpt_bill_inout_type = objBillingInquiry.ListBillingInoutType[0].bill_inout_type;
                        objBillingInquiry = ServiceObject.GetBillingInvStaus(objBillingInquiry);
                        l_str_rpt_bill_status = objBillingInquiry.ListBillingInvStatus[0].InvoiceStatus;
                        if (l_str_rpt_bill_status != "P")
                        {
                            objBillingInquiry.ReportStatus = "Invoice Not Posted";
                        }
                        if (l_str_rpt_bill_status == "P")
                        {
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

                                    }
                                    else
                                    {
                                        strReportName = "rpt_va_bill_doc.rpt";

                                    }
                                }
                                else
                                {
                                    if (type == "PDF")
                                    {
                                        strReportName = "rpt_va_bill_doc.rpt";
                                    }
                                    else
                                    {
                                        strReportName = "rpt_va_bill_doc_Excel.rpt";
                                    }
                                }

                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                objBillingInquiry.cmp_id = p_str_cmp_id;
                                objBillingInquiry.Bill_doc_id = SelectdID;
                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0421_001 

                                if (type == "PDF")
                                {
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

                                            if (strReportName == "rpt_va_bill_doc_Excel.rpt")
                                            {
                                                //Functionality - Parameter field to change Invoice/Bill should be handled 
                                            }
                                            else
                                            {
                                                rd.SetParameterValue("fml_rpt_title", "INVOICE");
                                                rd.SetParameterValue("fml_rpt_bill_title", "(VAS BILL)");
                                                rd.SetParameterValue("fml_rpt_param_bill_num", "Invoice#");
                                                rd.SetParameterValue("fml_rpt_param_bill_date", "Invoice Dt");
                                            }


                                            if (l_str_cmp_id == "FHNJ")
                                            {
                                                if (objCompany.cmp_id.Trim() == "SJOE")
                                                {

                                                }
                                                else
                                                {
                                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                                                }
                                            }
                                            else
                                            {
                                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                            }

                                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Invoice Document Vas");
                                        }
                                    }
                                }

                                else if (type == "XLS")
                                {
                                    objBillingInquiry = ServiceObject.GetBillingBillDocVASRpt(objBillingInquiry);
                                    List<BILLING_NORM_BILLDOC_EXCEL> li = new List<BILLING_NORM_BILLDOC_EXCEL>();
                                    for (int i = 0; i < objBillingInquiry.ListBillingDocVASRpt.Count; i++)
                                    {

                                        BILLING_NORM_BILLDOC_EXCEL objOBInquiryExcel = new BILLING_NORM_BILLDOC_EXCEL();
                                        objOBInquiryExcel.Line = objBillingInquiry.ListBillingDocVASRpt[i].dtl_line;
                                        objOBInquiryExcel.VASId = objBillingInquiry.ListBillingDocVASRpt[i].ship_doc_id;
                                        objOBInquiryExcel.ShipDate = objBillingInquiry.ListBillingDocVASRpt[i].ship_dt.ToString("MM/dd/yyyy");
                                        objOBInquiryExcel.CustOrder = objBillingInquiry.ListBillingDocVASRpt[i].CustOrder;
                                        objOBInquiryExcel.Notes = objBillingInquiry.ListBillingDocVASRpt[i].Notes;
                                        objOBInquiryExcel.VASDesc = objBillingInquiry.ListBillingDocVASRpt[i].ship_itm_num;
                                        objOBInquiryExcel.Ctns = objBillingInquiry.ListBillingDocVASRpt[i].ship_ctns;
                                        objOBInquiryExcel.Rate = objBillingInquiry.ListBillingDocVASRpt[i].so_itm_price;
                                        objOBInquiryExcel.Amount = (objOBInquiryExcel.Ctns) * (objOBInquiryExcel.Rate);
                                        li.Add(objOBInquiryExcel);
                                    }

                                    GridView gv = new GridView();
                                    gv.DataSource = li;
                                    gv.DataBind();
                                    Session["INVOICE_DOC"] = gv;
                                    return new DownloadFileActionResult((GridView)Session["INVOICE_DOC"], "INVOICE_DOC" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
                                }
                                else
                                {
                                    Response.Write("Please select the mode!");
                                }
                            }
                           
                            else if (l_str_rpt_bill_doc_type == "INOUT")
                            {
                                if (l_str_rpt_bill_inout_type.Trim() == "Carton")

                                {

                                    if (type == "PDF")
                                    {
                                        strReportName = "rpt_inout_bill_doc.rpt";
                                    }
                                    else
                                    {
                                        strReportName = "rpt_inout_bill_doc_Excel.rpt";
                                    }

                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry = ServiceObject.GetBillingBillInoutCartonRpt(objBillingInquiry);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    var rptSource = objBillingInquiry.ListBillingInoutCartonRpt.ToList();




                                    if (type == "PDF")
                                    {
                                        if (rptSource.Count > 0)
                                        {
                                            using (ReportDocument rd = new ReportDocument())
                                            {
                                                rd.Load(strRptPath);
                                                rd.SetDataSource(rptSource);
                                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                                if (strReportName == "rpt_inout_bill_doc_Excel.rpt")
                                                {
                                                    //Functionality Should be implemented
                                                }
                                                else
                                                {
                                                    DataTable dtCtns = new DataTable();
                                                    int lintTotCtns = 0;
                                                    int lintTotIBSQty = 0;
                                                    dtCtns = ServiceObject.fnGetCtnsByBillDoc(SelectdID);
                                                    if (dtCtns.Rows.Count > 0)
                                                    {
                                                        lintTotCtns = Convert.ToInt32(dtCtns.Rows[0]["TotCtns"]);
                                                        lintTotIBSQty = Convert.ToInt32(dtCtns.Rows[0]["TotIBSQty"]);
                                                    }
                                                    rd.SetParameterValue("fml_rpt_title", "INVOICE");
                                                    rd.SetParameterValue("fml_rpt_bill_title", "(INOUT BILL BY CARTON)");
                                                    rd.SetParameterValue("fml_rpt_param_bill_num", "Invoice#");
                                                    rd.SetParameterValue("fml_rpt_param_bill_date", "Invoice Dt");
                                                    rd.SetParameterValue("fmlTotCtns", lintTotCtns);
                                                    rd.SetParameterValue("fmlTotIBSQty", lintTotIBSQty);
                                                }
                                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Invoice Document INout Carton");
                                            }
                                        }
                                    }

                                    else if (type == "XLS")
                                    {
                                        objBillingInquiry = ServiceObject.GetInOutBillCarton(objBillingInquiry);
                                        objBillingInquiry = ServiceObject.GetBillingBillInoutCartonRpt(objBillingInquiry);
                                        List<BILLING_INOUT_BILLDOC_CTN_EXCEL> li = new List<BILLING_INOUT_BILLDOC_CTN_EXCEL>();
                                        for (int i = 0; i < objBillingInquiry.ListBillingInoutCartonRpt.Count; i++)
                                        {

                                            BILLING_INOUT_BILLDOC_CTN_EXCEL objOBInquiryExcel = new BILLING_INOUT_BILLDOC_CTN_EXCEL();
                                            objOBInquiryExcel.Line = objBillingInquiry.ListBillingInoutCartonRpt[i].dtl_line;
                                            objOBInquiryExcel.ContId = objBillingInquiry.ListBillingInoutCartonRpt[i].cont_id;
                                            objOBInquiryExcel.LotId = objBillingInquiry.ListBillingInoutCartonRpt[i].lot_id;
                                            objOBInquiryExcel.ServiceId = objBillingInquiry.ListBillingInoutCartonRpt[i].ship_itm_num;
                                            objOBInquiryExcel.ContId = objBillingInquiry.ListBillingInoutCartonRpt[i].cont_id;
                                            objOBInquiryExcel.LotId = objBillingInquiry.ListBillingInoutCartonRpt[i].lot_id;
                                            objOBInquiryExcel.Style = objBillingInquiry.ListBillingInoutCartonRpt[i].so_itm_num;
                                            objOBInquiryExcel.Color = objBillingInquiry.ListBillingInoutCartonRpt[i].so_itm_color;
                                            objOBInquiryExcel.Size = objBillingInquiry.ListBillingInoutCartonRpt[i].so_itm_size;
                                            objOBInquiryExcel.Rate = objBillingInquiry.ListBillingInoutCartonRpt[i].so_itm_price;
                                            objOBInquiryExcel.Ctns = objBillingInquiry.ListBillingInoutCartonRpt[i].ship_ctns;
                                            objOBInquiryExcel.TotalAmount = objBillingInquiry.ListBillingInoutCartonRpt[i].bill_amt;


                                            li.Add(objOBInquiryExcel);
                                        }

                                        GridView gv = new GridView();
                                        gv.DataSource = li;
                                        gv.DataBind();
                                        Session["INVOICE_DOC"] = gv;
                                        return new DownloadFileActionResult((GridView)Session["INVOICE_DOC"], "INVOICE_DOC" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");

                                    }
                                    else
                                    {
                                        Response.Write("Please select the mode!");
                                    }
                                    //}

                                }
                                if (l_str_rpt_bill_inout_type.Trim() == "Cube")

                                {

                                    if (type == "PDF")
                                    {
                                        strReportName = "rpt_inout_bill_doc_bycube.rpt";
                                    }
                                    else
                                    {
                                        strReportName = "rpt_inout_bill_doc_bycube_Excel.rpt";
                                    }
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry = ServiceObject.GetBillingBillInoutCubeRpt(objBillingInquiry);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 


                                    if (type == "PDF")
                                    {
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
                                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                                                if (strReportName == "rpt_inout_bill_doc_bycube_Excel.rpt")
                                                { }
                                                else
                                                {
                                                    rd.SetParameterValue("fml_rpt_title", "INVOICE");
                                                    rd.SetParameterValue("fml_rpt_bill_title", "(INBOUND BILL BY CUBE)");
                                                    rd.SetParameterValue("fml_rpt_param_bill_num", "Invoice#");
                                                    rd.SetParameterValue("fml_rpt_param_bill_date", "Invoice Dt");
                                                }
                                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Invoice Document INout Cube");
                                            }
                                        }
                                    }
                                    else if (type == "XLS")
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
                                        Session["INVOICE_DOC"] = gv;
                                        return new DownloadFileActionResult((GridView)Session["BILL_DOC"], "INVOICE_DOC" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");

                                    }

                                    else
                                    {
                                        Response.Write("Please select the mode!");
                                    }
                                    //}

                                }
                                // CR - 3PL_MVC_IB_2018_0219_004

                                if (l_str_rpt_bill_inout_type.Trim() == "Container")

                                {
                                    if (type == "PDF")
                                    {
                                        strReportName = "rpt_inout_bill_by_Container.rpt";
                                    }
                                    else
                                    {
                                        strReportName = "rpt_inout_bill_by_Container_Excel.rpt";
                                    }
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    objBillingInquiry = ServiceObject.GetBillingBillDocContainerInoutRpt(objBillingInquiry);
                                    var rptSource = objBillingInquiry.ListGenBillingInoutByContainerRpt.ToList();


                                    if (type == "PDF")
                                    {
                                        if (rptSource.Count > 0)
                                        {
                                            using (ReportDocument rd = new ReportDocument())
                                            {
                                                rd.Load(strRptPath);
                                                rd.SetDataSource(rptSource);
                                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document INout Cntr");
                                            }
                                        }
                                    }

                                    else if (type == "Excel")
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
                                        Session["INVOICE_DOC"] = gv;
                                        return new DownloadFileActionResult((GridView)Session["INVOICE_DOC"], "INVOICE_DOC_" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");

                                    }
                                    else
                                    {
                                        Response.Write("Please select the mode!");
                                    }
                                }
                            
                            }

                            else
                            {
                                if (l_str_rpt_bill_doc_type == "STRG")
                                {
                                    if (l_str_rpt_bill_type.Trim() == "Carton")

                                    {

                                        if (type == "PDF")
                                        {
                                            strReportName = "rpt_st_bill_doc.rpt";
                                        }
                                        else
                                        {
                                            strReportName = "rpt_st_bill_doc_Excel.rpt";
                                        }

                                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        objBillingInquiry.cmp_id = p_str_cmp_id;
                                        objBillingInquiry.Bill_doc_id = SelectdID;
                                        objBillingInquiry = ServiceObject.GetBillingBillDocSTRGRpt(objBillingInquiry);
                                        var rptSource = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.ToList();

                                        if (type == "PDF")
                                        {
                                            if (rptSource.Count > 0)
                                            {
                                                using (ReportDocument rd = new ReportDocument())
                                                {
                                                    rd.Load(strRptPath);
                                                    rd.SetDataSource(rptSource);
                                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                                    rd.SetParameterValue("fml_rpt_title", "INVOICE");
                                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG Carton");
                                                }
                                            }
                                        }

                                        else if (type == "XLS")
                                        {
                                            List<BILLING_STRG_BILLDOC_CRTN_EXCEL> li = new List<BILLING_STRG_BILLDOC_CRTN_EXCEL>();
                                            for (int i = 0; i < objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.Count; i++)
                                            {

                                                BILLING_STRG_BILLDOC_CRTN_EXCEL objOBInquiryExcel = new BILLING_STRG_BILLDOC_CRTN_EXCEL();
                                                objOBInquiryExcel.LineNo = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].dtl_line;
                                                objOBInquiryExcel.Desc = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].itm_name;
                                                objOBInquiryExcel.Style = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].so_itm_num;
                                                objOBInquiryExcel.Color = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].so_itm_color;
                                                objOBInquiryExcel.Size = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].so_itm_size;
                                                objOBInquiryExcel.Ctns = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].ship_ctns;
                                                objOBInquiryExcel.Rate = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].so_itm_price;
                                                objOBInquiryExcel.Amount = (objOBInquiryExcel.Ctns) * (objOBInquiryExcel.Rate);

                                                li.Add(objOBInquiryExcel);
                                            }

                                            GridView gv = new GridView();
                                            gv.DataSource = li;
                                            gv.DataBind();
                                            Session["BILL_DOC"] = gv;
                                            return new DownloadFileActionResult((GridView)Session["INVOICE_DOC"], "INVOICE_DOC" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
                                        }
                                        else
                                        {
                                            Response.Write("Please select the mode!");
                                        }
                                        //}

                                    }

                                    if (l_str_rpt_bill_type.Trim() == "Cube")

                                    {

                                        if (type == "PDF")
                                        {
                                            strReportName = "rpt_st_bill_doc_bycube.rpt";
                                        }
                                        else
                                        {
                                            strReportName = "rpt_st_bill_doc_bycube_Excel.rpt";
                                        }
                                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                        objBillingInquiry.cmp_id = p_str_cmp_id;
                                        objBillingInquiry.Bill_doc_id = SelectdID;

                                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 


                                        if (type == "PDF")
                                        {
                                            objBillingInquiry = ServiceObject.GetBillingBillDocCubeSTRGRpt(objBillingInquiry);
                                            var rptSource = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.ToList();
                                            if (rptSource.Count > 0)
                                            {
                                                using (ReportDocument rd = new ReportDocument())
                                                {
                                                    rd.Load(strRptPath);

                                                    rd.SetDataSource(rptSource);
                                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                                    rd.SetParameterValue("fml_rpt_title", "INVOICE");
                                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG Cube");
                                                }
                                            }
                                        }

                                        else if (type == "XLS")
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
                                            Session["INVOICE_DOC"] = gv;
                                            return new DownloadFileActionResult((GridView)Session["INVOICE_DOC"], "INVOICE_DOC" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
                                        }
                                        else
                                        {
                                            Response.Write("Please select the mode!");
                                        }

                                        //}

                                    }
                                    //  CR_3PL_MVC_BL_2018_0226_001 – Add Starage Bill By Pallet Report
                                    if (l_str_rpt_bill_type.Trim() == "Pallet")
                                    {


                                        if (type == "PDF")
                                        {
                                            strReportName = "rpt_strg_bill_by_pallet.rpt";
                                        }
                                        else
                                        {
                                            strReportName = "rpt_strg_bill_by_pallet_Excel.rpt";
                                        }
                                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                        objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                                        objBillingInquiry.Bill_doc_id = SelectdID.Trim();
                                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        objBillingInquiry = ServiceObject.GetBillingBillDocPalletSTRGRpt(objBillingInquiry);
                                        var rptSource = objBillingInquiry.ListGenBillingStrgByPalletRpt.ToList();

                                        objBillingInquiry.itm_num = "STRG-2";
                                        objBillingInquiry = ServiceObject.GetSecondSTRGRate(objBillingInquiry);


                                        if (type == "PDF")
                                        {
                                            if (rptSource.Count > 0)
                                            {
                                                using (ReportDocument rd = new ReportDocument())
                                                {
                                                    rd.Load(strRptPath);
                                                    rd.SetDataSource(rptSource);
                                                    if (objBillingInquiry.sec_strg_rate == "1")
                                                    {
                                                        rd.SetParameterValue("fml_rep_itm_num", objBillingInquiry.ListGetSecondSTRGRate[0].itm_num);
                                                        rd.SetParameterValue("fml_rep_list_price", objBillingInquiry.ListGetSecondSTRGRate[0].list_price);
                                                        rd.SetParameterValue("fml_rep_price_uom", objBillingInquiry.ListGetSecondSTRGRate[0].price_uom);
                                                    }
                                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG Pallet");
                                                }
                                            }
                                        }

                                        else if (type == "XLS")
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
                                            Session["INVOICE_DOC"] = gv;
                                            return new DownloadFileActionResult((GridView)Session["INVOICE_DOC"], "INVOICE_DOC" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");

                                        }
                                        else
                                        {
                                            Response.Write("Please select the mode!");
                                        }
                                    }
                                    if (l_str_rpt_bill_type.Trim() == "Location")
                                    {

                                        if (type == "PDF")
                                        {
                                            strReportName = "rpt_st_bill_doc_location.rpt";
                                        }
                                        else
                                        {
                                            strReportName = "rpt_st_bill_doc_location_Excel.rpt";
                                        }

                                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                        objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                                        objBillingInquiry.Bill_doc_id = SelectdID.Trim();
                                        objBillingInquiry.bill_doc_id = SelectdID.Trim();
                                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 


                                        if (type == "PDF")
                                        {
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
                                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG Location");
                                                }
                                            }
                                        }

                                        else if (type == "XLS")
                                        {

                                            objBillingInquiry = ServiceObject.STRGBillLocationRpt(objBillingInquiry);
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
                                            Session["INVOICE_DOC"] = gv;
                                            return new DownloadFileActionResult((GridView)Session["INVOICE_DOC"], "INVOICE_DOC" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
                                        }
                                        else
                                        {
                                            Response.Write("Please select the mode!");
                                        }
                                    }

                                    if (l_str_rpt_bill_type.Trim() == "Pcs")
                                    {
                                        if (type == "PDF")
                                        {
                                            strReportName = "rpt_st_bill_doc_Pcs_item.rpt";
                                        }
                                        else
                                        {
                                            strReportName = "rpt_st_bill_doc_Pcs_item_Excel.rpt";
                                        }
                                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                        objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                                        objBillingInquiry.Bill_doc_id = SelectdID.Trim();
                                        objBillingInquiry.bill_doc_id = SelectdID.Trim();
                                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                                        if (type == "PDF")
                                        {
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
                                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG Pcs");
                                                }
                                            }
                                        }
                                        else

                                        {
                                            objBillingInquiry = ServiceObject.STRGBillPcsRpt(objBillingInquiry);
                                            List<BILLING_STRG_BILLDOC_PCS_EXCEL> li = new List<BILLING_STRG_BILLDOC_PCS_EXCEL>();
                                            for (int i = 0; i < objBillingInquiry.ListGetSTRGBillByPcsRpt.Count; i++)
                                            {

                                                BILLING_STRG_BILLDOC_PCS_EXCEL objOBInquiryExcel = new BILLING_STRG_BILLDOC_PCS_EXCEL();
                                                objOBInquiryExcel.BillDocId = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].bill_doc_id;
                                                objOBInquiryExcel.Line = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].dtl_line;
                                                objOBInquiryExcel.Desc = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].ship_itm_name;
                                                objOBInquiryExcel.Style = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].so_itm_num;
                                                objOBInquiryExcel.Color = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].so_itm_color;
                                                objOBInquiryExcel.Size = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].so_itm_size;
                                                objOBInquiryExcel.Qty = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].ship_qty;
                                                objOBInquiryExcel.Rate = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].so_itm_price;
                                                l_dec_list_price = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].so_itm_price;
                                                l_int_tot_ship_qty = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].ship_qty;
                                                l_dec_tot_amnt = l_dec_list_price * l_int_tot_ship_qty;
                                                objOBInquiryExcel.Amount = l_dec_tot_amnt;
                                                li.Add(objOBInquiryExcel);
                                            }

                                            GridView gv = new GridView();
                                            gv.DataSource = li;
                                            gv.DataBind();
                                            Session["INVOICE_DOC"] = gv;
                                            return new DownloadFileActionResult((GridView)Session["INVOICE_DOC"], "INVOICE_DOC" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
                                        }
                                    }

                                }

                            }



                        }
                    }
                    if (l_str_rpt_selection == "GridSummary")
                    {
                        if (type == "PDF")
                        {
                            strReportName = "rpt_bill_summary.rpt";
                        }
                        else
                        {
                            strReportName = "rpt_bill_summary_Excel.rpt";
                        }
                        if (type == "PDF")
                        {
                            BillingInquiry objBillingInquiry = new BillingInquiry();
                            BillingInquiryService ServiceObject = new BillingInquiryService();
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id;
                            objBillingInquiry.Bill_doc_id = p_str_Bill_doc_id;
                            objBillingInquiry.bill_type = p_str_Bill_type;
                            objBillingInquiry.Bill_doc_dt_Fr = p_str_doc_dt_Fr;
                            objBillingInquiry.Bill_doc_dt_To = p_str_doc_dt_To;

                            objBillingInquiry = ServiceObject.GetBillingSummaryRpt(objBillingInquiry);
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            var rptSource = objBillingInquiry.ListBillingSummaryRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objBillingInquiry.ListBillingSummaryRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim();
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Grid Summary");
                                }
                            }
                        }
                        else if (type == "Word")
                        {
                            BillingInquiry objBillingInquiry = new BillingInquiry();
                            BillingInquiryService ServiceObject = new BillingInquiryService();
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id;
                            objBillingInquiry.Bill_doc_id = p_str_Bill_doc_id;
                            objBillingInquiry.bill_type = p_str_Bill_type;
                            objBillingInquiry.Bill_doc_dt_Fr = p_str_doc_dt_Fr;
                            objBillingInquiry.Bill_doc_dt_To = p_str_doc_dt_To;

                            objBillingInquiry = ServiceObject.GetBillingSummaryRpt(objBillingInquiry);
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            var rptSource = objBillingInquiry.ListBillingSummaryRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objBillingInquiry.ListBillingSummaryRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim();
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Grid Summary");
                                }
                            }
                        }
                        else if (type == "XLS")
                        {
                            string tempFileName = string.Empty;
                            string l_str_file_name = string.Empty;
                            string strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
                            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                            p_str_Bill_type = "";
                            if (!Directory.Exists(strOutputpath))
                            {
                                Directory.CreateDirectory(strOutputpath);
                            }
                            BillingInquiryService objService = new BillingInquiryService();
                            DataTable dt_con_vas = new DataTable();
                            dt_con_vas = objService.GetGridSummaryBillList(p_str_cmp_id, p_str_Bill_doc_id, p_str_Bill_type, p_str_doc_dt_Fr, p_str_doc_dt_To);

                            l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-BILL - GRID SUMMARY REPORT-" + p_str_Bill_doc_id + strDateFormat + ".xlsx";

                            tempFileName = strOutputpath + l_str_file_name;

                            if (System.IO.File.Exists(tempFileName))
                                System.IO.File.Delete(tempFileName);
                            xls_Grid_Summary mxcel2 = new xls_Grid_Summary(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "BL_GRID_SUMMARY.xlsx");
                            mxcel2.PopulateHeader(p_str_cmp_id, string.Empty);

                            mxcel2.PopulateData(dt_con_vas, true);
                            mxcel2.SaveAs(tempFileName);
                            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                            return File(fs, "application / xlsx", l_str_file_name);
                        }
                        else
                        {
                            Response.Write("Please select the mode!");
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


        public ActionResult EmailShowReport(string p_str_radio, string p_str_cmp_id, string p_str_Bill_doc_id, string p_str_Bill_type, string p_str_doc_dt_Fr, string p_str_doc_dt_To, string SelectdID, string type, string p_str_ib_doc_id)

        {
            string tempFilepath = System.Configuration.ConfigurationManager.AppSettings["tempFilePath"].ToString().Trim();
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
            string reportFileName = string.Empty;
            objCompany.cmp_id = p_str_cmp_id.ToString().Trim();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            objBillingInquiry.LstCmpName = objCompany.LstCmpName;
            l_str_tmp_name = objBillingInquiry.LstCmpName[0].cmp_name.ToString().Trim();
            if (p_str_ib_doc_id != null)
            {
                objBillingInquiry.ib_doc_id = p_str_ib_doc_id.Trim();
            }
            else
            {
                objBillingInquiry.ib_doc_id = "";
            }
            objEmail.CmpId = p_str_cmp_id;
            objEmail.screenId = ScreenID;
            objEmail.username = objCompany.user_id;
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
                                     strReportName = "rpt_st_bill_doc.rpt";

                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry = ServiceObject.GetBillingBillDocSTRGRpt(objBillingInquiry);
                                    EmailSub = objBillingInquiry.cmp_id + " - STORAGE BILL - INV - " + SelectdID;
                                    EmailMsg = "Please find the attached Storage Invoice Bill document" + SelectdID;
                                    if (type == "PDF")
                                    {
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
                                            rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports" + p_str_cmp_id + "-INV-" + SelectdID + "-" + strDateFormat + ".pdf";
                                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                            }
                                        }
                                        reportFileName = p_str_cmp_id + "-INV-" + SelectdID + "-" + strDateFormat + ".pdf";
                                        Session["RptFileName"] = strFileName;
                                    }

                                    else
                                    if (type == "Excel")
                                    {
                                        objBillingInquiry = ServiceObject.GetBillingBillDocSTRGRpt(objBillingInquiry);
                                        List<BILLING_STRG_BILLDOC_CRTN_EXCEL> li = new List<BILLING_STRG_BILLDOC_CRTN_EXCEL>();
                                        for (int i = 0; i < objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.Count; i++)
                                        {

                                            BILLING_STRG_BILLDOC_CRTN_EXCEL objOBInquiryExcel = new BILLING_STRG_BILLDOC_CRTN_EXCEL();
                                            objOBInquiryExcel.LineNo = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].LineNo;
                                            objOBInquiryExcel.Desc = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].Desc;
                                            objOBInquiryExcel.Style = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].Style;
                                            objOBInquiryExcel.Color = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].Color;
                                            objOBInquiryExcel.Size = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].Size;
                                            objOBInquiryExcel.Ctns = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].Ctns;
                                            objOBInquiryExcel.Rate = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].Rate;
                                            objOBInquiryExcel.Amount = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].Amount;

                                            li.Add(objOBInquiryExcel);
                                        }

                                        GridView gv = new GridView();
                                        gv.DataSource = li;
                                        gv.DataBind();
                                        Session["BILL_DOC"] = gv;
                                        return new DownloadFileActionResult((GridView)Session["BILL_DOC"], "BILL_DOC" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



                                    }
                                //}

                            }
                            if (l_str_rpt_bill_type.Trim() == "Cube")

                            {
                                
                                    strReportName = "rpt_st_bill_doc_bycube.rpt";
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry = ServiceObject.GetBillingBillDocCubeSTRGRpt(objBillingInquiry);
                                    objBillingInquiry.Bill_doc_dt_Fr = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt[0].rcvd_dt.ToShortDateString();
                                    objBillingInquiry.bill_doc_id = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt[0].bill_doc_id;
                                    objBillingInquiry = ServiceObject.GetBillingBillamountInoutCartonRpt(objBillingInquiry);
                                    objBillingInquiry.bill_amt = objBillingInquiry.ListBillingamountInoutCartonRpt[0].bill_amt;
                                    objBillingInquiry.ship_ctns = objBillingInquiry.ListBillingamountInoutCartonRpt[0].ship_ctns;
                                    l_str_rptdtl = objBillingInquiry.cmp_id + "-" + "STRG-BILL" + "-" + objBillingInquiry.bill_doc_id;

                                    objEmail.EmailSubject = objBillingInquiry.cmp_id + "-" + "STORAGE BILL " + "|" + " " + "Invoice#: " + objBillingInquiry.bill_doc_id + "|" + " " + "Invoice:  " + "$" + objBillingInquiry.bill_amt;

                                    objEmail.EmailMessage = "Hi All," + "\n\n" + " Please find the attached Storage Billing document" + "\n\n";
                                    objEmail.EmailMessage = objEmail.EmailMessage + "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Bill Type: " + l_str_rpt_bill_doc_type + "\n" + "Invoice#: " + " " + " " + objBillingInquiry.bill_doc_id + "\n" + "Invoice: " + "$" + objBillingInquiry.bill_amt + "\n" + "Total Cartons: " + objBillingInquiry.ship_ctns;

                                    if (type == "PDF")
                                    {
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
                                            rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                                            //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//" + l_str_rptdtl + "-" + strDateFormat + ".pdf";
                                                // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                            }
                                        }

                                        reportFileName = l_str_rptdtl + "-" + strDateFormat + ".pdf";//CR2018-03-15-001 Added By Soniya
                                        Session["RptFileName"] = strFileName;
                                    }

                                    else
                                    if (type == "Excel")
                                    {
                                        objBillingInquiry = ServiceObject.GetBillingBillDocCubeSTRGRpt(objBillingInquiry);
                                        List<BILLING_STRG_BILLDOC_CUBE_EXCEL> li = new List<BILLING_STRG_BILLDOC_CUBE_EXCEL>();
                                        for (int i = 0; i < objBillingInquiry.ListBillingStrgExcel.Count; i++)
                                        {

                                            BILLING_STRG_BILLDOC_CUBE_EXCEL objOBInquiryExcel = new BILLING_STRG_BILLDOC_CUBE_EXCEL();
                                            objOBInquiryExcel.Line = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt[i].Line;
                                            objOBInquiryExcel.Description = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt[i].Description;
                                            objOBInquiryExcel.Style = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt[i].Style;
                                            objOBInquiryExcel.Color = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt[i].Color;
                                            objOBInquiryExcel.Size = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt[i].Size;
                                            objOBInquiryExcel.Ctn = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt[i].Ctn;
                                            objOBInquiryExcel.TotCube = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt[i].TotCube;
                                            objOBInquiryExcel.Rate = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt[i].Rate;
                                            objOBInquiryExcel.Amount = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt[i].Amount;

                                            li.Add(objOBInquiryExcel);
                                        }

                                        GridView gv = new GridView();
                                        gv.DataSource = li;
                                        gv.DataBind();
                                        Session["BILL_DOC"] = gv;
                                        return new DownloadFileActionResult((GridView)Session["BILL_DOC"], "BILL_DOC" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



                                    }

                                //}

                            }
                            if (l_str_rpt_bill_type.Trim() == "Pallet")
                            {


                                strReportName = "rpt_strg_bill_by_pallet.rpt";
                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                                objBillingInquiry.Bill_doc_id = SelectdID.Trim();
                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                objBillingInquiry = ServiceObject.GetBillingBillDocPalletSTRGRpt(objBillingInquiry);
                                objBillingInquiry.Bill_doc_dt_Fr = objBillingInquiry.ListGenBillingStrgByPalletRpt[0].rcvd_dt.ToShortDateString();
                                objBillingInquiry.bill_doc_id = objBillingInquiry.ListGenBillingStrgByPalletRpt[0].bill_doc_id;
                                objBillingInquiry = ServiceObject.GetBillingBillamountInoutCartonRpt(objBillingInquiry);
                                objBillingInquiry.bill_amt = objBillingInquiry.ListBillingamountInoutCartonRpt[0].bill_amt;
                                objBillingInquiry.ship_ctns = objBillingInquiry.ListBillingamountInoutCartonRpt[0].ship_ctns;
                                l_str_rptdtl = objBillingInquiry.cmp_id + "_" + "IN-OUT Bill" + "_" + objBillingInquiry.bill_doc_id + "_" + objBillingInquiry.bill_doc_id;
                                objEmail.EmailSubject = objBillingInquiry.cmp_id + "-" + "IN-OUT Bill " + "|" + " " + "Invoice#: " + objBillingInquiry.bill_doc_id + "|" + " " + "Invoice:  " + "$" + objBillingInquiry.bill_amt + "|" + " " + "IB Doc ID#: " + objBillingInquiry.bill_doc_id + "|" + " " + "Received Date: " + objBillingInquiry.Bill_doc_dt_Fr;
                                objEmail.EmailMessage = "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Bill Type: " + l_str_rpt_bill_doc_type + "\n" + "Invoice#: " + " " + " " + objBillingInquiry.bill_doc_id + "\n" + "Invoice: " + "$" + objBillingInquiry.bill_amt + "\n" + "IB Doc ID#: " + " " + " " + objBillingInquiry.bill_doc_id + "\n" + "Received Date: " + " " + " " + objBillingInquiry.Bill_doc_dt_Fr + "\n" + "Total Cartons: " + objBillingInquiry.ship_ctns;

                                if (type == "PDF")
                                {
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
                                            // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//IV_DOC_INQ_" + strDateFormat + ".pdf";
                                            // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                        }
                                    }
                                    reportFileName = l_str_rptdtl + strDateFormat + ".pdf";//CR2018-03-15-001 Added By Soniya
                                    Session["RptFileName"] = strFileName;
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
                            //string l_str_cmp_id = string.Empty;
                            //l_str_cmp_id = objBillingInquiry.cmp_id;
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

                                }
                                else
                                {
                                    strReportName = "rpt_va_bill_doc.rpt";

                                }
                                //strReportName = "rpt_va_bill_doc_FH_NJ.rpt";
                            }
                            else
                            {
                                if (type == "PDF")
                                {
                                    strReportName = "rpt_va_bill_doc.rpt";
                                }
                                else
                                {
                                    strReportName = "rpt_va_bill_doc_Excel.rpt";
                                }
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
                            objBillingInquiry.cmp_id = p_str_cmp_id;
                            objBillingInquiry.Bill_doc_id = SelectdID;
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0421_001 
                            objBillingInquiry = ServiceObject.GetBillingBillDocVASRpt(objBillingInquiry);
                            objBillingInquiry.Bill_doc_dt_Fr = objBillingInquiry.ListBillingDocVASRpt[0].rcvd_dt.ToShortDateString();
                            objBillingInquiry.bill_doc_id = objBillingInquiry.ListBillingDocVASRpt[0].bill_doc_id;
                            objBillingInquiry = ServiceObject.GetBillingBillamountInoutCartonRpt(objBillingInquiry);
                            objBillingInquiry.bill_amt = objBillingInquiry.ListBillingamountInoutCartonRpt[0].bill_amt;
                            objBillingInquiry.ship_ctns = objBillingInquiry.ListBillingamountInoutCartonRpt[0].ship_ctns;
                            l_str_rptdtl = objBillingInquiry.cmp_id + "_" + "VAS BILL" + "_" + objBillingInquiry.bill_doc_id;
                            objEmail.EmailSubject = objBillingInquiry.cmp_id + "-" + "VAS BILL" + "|" + " " + "Invoice#: " + objBillingInquiry.bill_doc_id + "|" + " " + "Invoice Amount:  " + "$" + objBillingInquiry.bill_amt;
                            objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Bill Type: " + l_str_rpt_bill_doc_type + "\n" + "Invoice#: " + " " + " " + objBillingInquiry.bill_doc_id + "\n" + "Invoice Amount: " + "$" + objBillingInquiry.bill_amt;

                            if (type == "PDF" || (type == "XLS"))
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    var rptSource = objBillingInquiry.ListBillingDocVASRpt.ToList();
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
                                        if (type == "PDF")
                                        {
                                            rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                                            rd.SetParameterValue("fml_rpt_bill_title", "(VAS BILL)");
                                            rd.SetParameterValue("fml_rpt_param_bill_num", "Bill#");
                                            rd.SetParameterValue("fml_rpt_param_bill_date", "Bill Dt");
                                        }
                                    }

                                    //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                    if (type == "PDF")
                                    {
                                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                    }
                                    else
                                    {
                                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".xls";
                                        rd.ExportToDisk(ExportFormatType.Excel, strFileName);
                                    }
                                    // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                }
                                if (type == "PDF")
                                {
                                    reportFileName = l_str_rptdtl + strDateFormat + ".pdf";//CR2018-03-15-001 Added By Soniya
                                    Session["RptFileName"] = strFileName;
                                }
                                if (type == "XLS")
                                {
                                    reportFileName = l_str_rptdtl + strDateFormat + ".xls";//CR2018-03-15-001 Added By Soniya
                                    Session["RptFileName"] = strFileName;
                                }

                            }


                        }
                        if (l_str_rpt_bill_doc_type == "INOUT")
                        {
                            if (p_str_ib_doc_id != null)
                            {
                                objBillingInquiry.ib_doc_id = p_str_ib_doc_id.Trim();
                            }
                            else
                            {
                                objBillingInquiry.ib_doc_id = "";
                            }
                            if (l_str_rpt_bill_inout_type.Trim() == "Carton")

                            {
               
                                    strReportName = "rpt_inout_bill_doc.rpt";
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
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry = ServiceObject.GetBillingBillamountInoutCartonRpt(objBillingInquiry);
                                    objBillingInquiry.bill_amt = objBillingInquiry.ListBillingamountInoutCartonRpt[0].bill_amt;
                                    objBillingInquiry.ship_ctns = objBillingInquiry.ListBillingamountInoutCartonRpt[0].ship_ctns;
                                    objBillingInquiry = ServiceObject.GetBillingBillInoutCartonRpt(objBillingInquiry);
                                    objBillingInquiry.Bill_doc_dt_Fr = objBillingInquiry.ListBillingInoutCartonRpt[0].rcvd_dt.ToShortDateString();
                                    objBillingInquiry.bill_doc_id = objBillingInquiry.ListBillingInoutCartonRpt[0].bill_doc_id;
                                    if (objBillingInquiry.ib_doc_id == null || objBillingInquiry.ib_doc_id == "")
                                    {
                                        l_str_rptdtl = objBillingInquiry.cmp_id + "_" + "IN-OUT BILL" + "_" + objBillingInquiry.bill_doc_id;
                                        objEmail.EmailSubject = objBillingInquiry.cmp_id + "-" + "IN-OUT BILL " + "|" + " " + "Invoice#: " + objBillingInquiry.bill_doc_id + "|" + " " + "Invoice:  " + "$" + objBillingInquiry.bill_amt + "|" + " " + "Received Date: " + objBillingInquiry.Bill_doc_dt_Fr;
                                        objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Bill Type: " + l_str_rpt_bill_doc_type + "\n" + "Invoice#: " + " " + " " + objBillingInquiry.bill_doc_id + "\n" + "Invoice: " + "$" + objBillingInquiry.bill_amt + "\n" + "Received Date: " + " " + " " + objBillingInquiry.Bill_doc_dt_Fr + "\n" + "Total Cartons: " + objBillingInquiry.ship_ctns;

                                    }
                                    else
                                    {
                                        l_str_rptdtl = objBillingInquiry.cmp_id + "_" + "IN-OUT BILL" + "_" + objBillingInquiry.bill_doc_id + "_" + objBillingInquiry.ib_doc_id;
                                        objEmail.EmailSubject = objBillingInquiry.cmp_id + "-" + "IN-OUT BILL" + "|" + " " + "Invoice#: " + objBillingInquiry.bill_doc_id + "|" + " " + "Invoice:  " + "$" + objBillingInquiry.bill_amt + "|" + " " + "IB Doc ID#: " + objBillingInquiry.ib_doc_id + "|" + " " + "Received Date: " + objBillingInquiry.Bill_doc_dt_Fr;
                                        objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Bill Type: " + l_str_rpt_bill_doc_type + "\n" + "Invoice#: " + " " + " " + objBillingInquiry.bill_doc_id + "\n" + "Invoice: " + "$" + objBillingInquiry.bill_amt + "\n" + "IB Doc ID#: " + " " + " " + objBillingInquiry.ib_doc_id + "\n" + "Received Date: " + " " + " " + objBillingInquiry.Bill_doc_dt_Fr + "\n" + "Total Cartons: " + objBillingInquiry.ship_ctns;
                                    }
                                    if ((type == "PDF") || (type == "XLS"))
                                    {
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

                                                rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                                                rd.SetParameterValue("fml_rpt_bill_title", "(INOUT BILL BY " + l_str_rpt_bill_inout_type.Trim().ToUpper() + ")");
                                                rd.SetParameterValue("fml_rpt_param_bill_num", "Bill#");
                                                rd.SetParameterValue("fml_rpt_param_bill_date", "Bill Dt");

                                                DataTable dtCtns = new DataTable();
                                                int lintTotCtns = 0;
                                                int lintTotIBSQty = 0;
                                                dtCtns = ServiceObject.fnGetCtnsByBillDoc(SelectdID);
                                                if (dtCtns.Rows.Count > 0)
                                                {
                                                    lintTotCtns = Convert.ToInt32(dtCtns.Rows[0]["TotCtns"]);
                                                    lintTotIBSQty = Convert.ToInt32(dtCtns.Rows[0]["TotIBSQty"]);
                                                }
                                                rd.SetParameterValue("fmlTotCtns", lintTotCtns);
                                                rd.SetParameterValue("fmlTotIBSQty", lintTotIBSQty);

                                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                                if (type == "PDF")
                                                {
                                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                                    reportFileName = l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                                }
                                                else if (type == "XLS")
                                                {
                                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".xls";
                                                    rd.ExportToDisk(ExportFormatType.ExcelRecord, strFileName);
                                                    reportFileName = l_str_rptdtl + "_" + strDateFormat + ".xls";
                                                }
                                            }
                                        }
                                        Session["RptFileName"] = strFileName;
                                    }

                                    else
                                    if (type == "Excel")
                                    {

                                        objBillingInquiry = ServiceObject.GetBillingBillInoutCartonRpt(objBillingInquiry);

                                        List<BILLING_INOUT_BILLDOC_CTN_EXCEL> li = new List<BILLING_INOUT_BILLDOC_CTN_EXCEL>();
                                        for (int i = 0; i < objBillingInquiry.ListBillingInoutCartonRpt.Count; i++)
                                        {

                                            BILLING_INOUT_BILLDOC_CTN_EXCEL objOBInquiryExcel = new BILLING_INOUT_BILLDOC_CTN_EXCEL();
                                            objOBInquiryExcel.Line = objBillingInquiry.ListBillingInoutCartonRpt[i].Line;
                                            objOBInquiryExcel.ServiceId = objBillingInquiry.ListBillingInoutCartonRpt[i].ServiceId;
                                            objOBInquiryExcel.ContId = objBillingInquiry.ListBillingInoutCartonRpt[i].ContId;
                                            objOBInquiryExcel.LotId = objBillingInquiry.ListBillingInoutCartonRpt[i].LotId;
                                            objOBInquiryExcel.Style = objBillingInquiry.ListBillingInoutCartonRpt[i].Style;
                                            objOBInquiryExcel.Color = objBillingInquiry.ListBillingInoutCartonRpt[i].Color;
                                            objOBInquiryExcel.Size = objBillingInquiry.ListBillingInoutCartonRpt[i].Size;
                                            objOBInquiryExcel.Rate = objBillingInquiry.ListBillingInoutCartonRpt[i].Rate;
                                            objOBInquiryExcel.TotalAmount = objBillingInquiry.ListBillingInoutCartonRpt[i].Amount;

                                            li.Add(objOBInquiryExcel);
                                        }

                                        GridView gv = new GridView();
                                        gv.DataSource = li;
                                        gv.DataBind();
                                        Session["BILL_DOC"] = gv;
                                        return new DownloadFileActionResult((GridView)Session["BILL_DOC"], "BILL_DOC_" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");

                                    }
                                //}

                            }
                            if (l_str_rpt_bill_inout_type.Trim() == "Cube")

                            {
                              
                                    strReportName = "rpt_inout_bill_doc_bycube.rpt";
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry = ServiceObject.GetBillingBillInoutCubeRpt(objBillingInquiry);

                                    if (objBillingInquiry.ib_doc_id == null || objBillingInquiry.ib_doc_id == "")
                                    {
                                        l_str_rptdtl = objBillingInquiry.cmp_id + "_" + "IN-OUT BILL" + "-" + SelectdID;
                                        objEmail.EmailSubject = objBillingInquiry.cmp_id + "-" + "IN-OUT BILL " + "|" + " " + "Invoice#: " + SelectdID;
                                        objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Bill Type: " + l_str_rpt_bill_doc_type + "\n" + "Invoice#: " + " " + " " + SelectdID + "\n";

                                    }
                                    else
                                    {
                                        l_str_rptdtl = objBillingInquiry.cmp_id + "_" + "IN-OUT BILL" + "-" + objBillingInquiry.ib_doc_id;
                                        objEmail.EmailSubject = objBillingInquiry.cmp_id + "-" + "IN-OUT BILL" + "|" + " " + "Invoice#: " + SelectdID + "|" + " " + "Invoice:  " + "$" + objBillingInquiry.bill_amt + "|" + " " + "IB Doc ID#: " + objBillingInquiry.ib_doc_id + "|" + " " + "Received Date: " + objBillingInquiry.Bill_doc_dt_Fr;
                                        objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Bill Type: " + SelectdID + "\n" + "Invoice#: " + " " + " " + objBillingInquiry.bill_doc_id + "\n" + "Invoice: " + "$" + objBillingInquiry.bill_amt + "\n" + "IB Doc ID#: " + " " + " " + objBillingInquiry.ib_doc_id + "\n" + "Received Date: " + " " + " " + objBillingInquiry.Bill_doc_dt_Fr + "\n" + "Total Cartons: " + objBillingInquiry.ship_ctns;

                                    }
                                    if (type == "PDF")
                                    {
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
                                                rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                                                rd.SetParameterValue("fml_rpt_bill_title", "(INBOUND BILL BY CUBE)");
                                                rd.SetParameterValue("fml_rpt_param_bill_num", "Bill#");
                                                rd.SetParameterValue("fml_rpt_param_bill_date", "Bill Dt");
                                                //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//" + objBillingInquiry.cmp_id + "-INOUT-BILL-" + SelectdID + "-" + strDateFormat + ".pdf";
                                                // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                            }
                                        }
                                        reportFileName = objBillingInquiry.cmp_id + "-INBOUND-BILL-" + SelectdID + "-" + strDateFormat + ".pdf";
                                        Session["RptFileName"] = strFileName;
                                    }

                                    else
                                    if (type == "XLS")
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
                                //}

                            }
                            if (l_str_rpt_bill_inout_type.Trim() == "Container")

                            {

                                strReportName = "rpt_inout_bill_by_Container.rpt";
                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                objBillingInquiry.cmp_id = p_str_cmp_id;
                                objBillingInquiry.Bill_doc_id = SelectdID;


                                if (type == "PDF")
                                {
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
                                            //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//IV_DOC_INQ_" + strDateFormat + ".pdf";
                                            // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                        }
                                    }
                                    reportFileName = "BL_INQ_" + strDateFormat + ".pdf";//CR2018-03-15-001 Added By Soniya
                                    Session["RptFileName"] = strFileName;
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
                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                        objBillingInquiry.cmp_id = p_str_cmp_id;
                        objBillingInquiry.Bill_doc_id = SelectdID;
                        objBillingInquiry = ServiceObject.GetBillingBillingType(objBillingInquiry);
                        l_str_rpt_bill_type = objBillingInquiry.ListBillingType[0].bill_type;
                        objBillingInquiry = ServiceObject.GetBillingBillDocIdType(objBillingInquiry);
                        l_str_rpt_bill_doc_type = objBillingInquiry.ListBillingDocIdType[0].bill_type;
                        objBillingInquiry = ServiceObject.GetBillingInoutType(objBillingInquiry);
                        l_str_rpt_bill_inout_type = objBillingInquiry.ListBillingInoutType[0].bill_inout_type;

                        objBillingInquiry = ServiceObject.GetBillingInvStaus(objBillingInquiry);
                        l_str_rpt_bill_status = objBillingInquiry.ListBillingInvStatus[0].InvoiceStatus;
                        if (l_str_rpt_bill_status == "P")
                        {
                            if (l_str_rpt_bill_doc_type == "INOUT")
                            {
                                if (p_str_ib_doc_id != null)
                                {
                                    objBillingInquiry.ib_doc_id = p_str_ib_doc_id.Trim();
                                }
                                else
                                {
                                    objBillingInquiry.ib_doc_id = "";
                                }
                                if (l_str_rpt_bill_inout_type.Trim() == "Carton")

                                {

                                    strReportName = "rpt_inout_bill_doc.rpt";
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
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry = ServiceObject.GetBillingBillamountInoutCartonRpt(objBillingInquiry);
                                    objBillingInquiry.bill_amt = objBillingInquiry.ListBillingamountInoutCartonRpt[0].bill_amt;
                                    objBillingInquiry.ship_ctns = objBillingInquiry.ListBillingamountInoutCartonRpt[0].ship_ctns;
                                    objBillingInquiry = ServiceObject.GetBillingBillInoutCartonRpt(objBillingInquiry);
                                    objBillingInquiry.Bill_doc_dt_Fr = objBillingInquiry.ListBillingInoutCartonRpt[0].rcvd_dt.ToShortDateString();
                                    objBillingInquiry.bill_doc_id = objBillingInquiry.ListBillingInoutCartonRpt[0].bill_doc_id;
                                    if (objBillingInquiry.ib_doc_id == null || objBillingInquiry.ib_doc_id == "")
                                    {
                                        l_str_rptdtl = objBillingInquiry.cmp_id + "_" + "IN-OUT BILL" + "_" + objBillingInquiry.bill_doc_id;
                                        objEmail.EmailSubject = objBillingInquiry.cmp_id + "-" + "IN-OUT BILL " + "|" + " " + "Invoice#: " + objBillingInquiry.bill_doc_id + "|" + " " + "Invoice:  " + "$" + objBillingInquiry.bill_amt + "|" + " " + "Received Date: " + objBillingInquiry.Bill_doc_dt_Fr;
                                        objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Bill Type: " + l_str_rpt_bill_doc_type + "\n" + "Invoice#: " + " " + " " + objBillingInquiry.bill_doc_id + "\n" + "Invoice: " + "$" + objBillingInquiry.bill_amt + "\n" + "Received Date: " + " " + " " + objBillingInquiry.Bill_doc_dt_Fr + "\n" + "Total Cartons: " + objBillingInquiry.ship_ctns;

                                    }
                                    else
                                    {
                                        l_str_rptdtl = objBillingInquiry.cmp_id + "_" + "IN-OUT BILL" + "_" + objBillingInquiry.bill_doc_id + "_" + objBillingInquiry.ib_doc_id;
                                        objEmail.EmailSubject = objBillingInquiry.cmp_id + "-" + "IN-OUT BILL" + "|" + " " + "Invoice#: " + objBillingInquiry.bill_doc_id + "|" + " " + "Invoice:  " + "$" + objBillingInquiry.bill_amt + "|" + " " + "IB Doc ID#: " + objBillingInquiry.ib_doc_id + "|" + " " + "Received Date: " + objBillingInquiry.Bill_doc_dt_Fr;
                                        objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Bill Type: " + l_str_rpt_bill_doc_type + "\n" + "Invoice#: " + " " + " " + objBillingInquiry.bill_doc_id + "\n" + "Invoice: " + "$" + objBillingInquiry.bill_amt + "\n" + "IB Doc ID#: " + " " + " " + objBillingInquiry.ib_doc_id + "\n" + "Received Date: " + " " + " " + objBillingInquiry.Bill_doc_dt_Fr + "\n" + "Total Cartons: " + objBillingInquiry.ship_ctns;
                                    }
                                    if ((type == "PDF") || (type == "XLS"))
                                    {
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

                                                rd.SetParameterValue("fml_rpt_title", "INVOICE");
                                                rd.SetParameterValue("fml_rpt_bill_title", "(INOUT BILL BY " + l_str_rpt_bill_inout_type.Trim().ToUpper() + ")");
                                                rd.SetParameterValue("fml_rpt_param_bill_num", "Invoice#");
                                                rd.SetParameterValue("fml_rpt_param_bill_date", "Invoice Dt");

                                                DataTable dtCtns = new DataTable();
                                                int lintTotCtns = 0;
                                                int lintTotIBSQty = 0;
                                                dtCtns = ServiceObject.fnGetCtnsByBillDoc(SelectdID);
                                                if (dtCtns.Rows.Count > 0)
                                                {
                                                    lintTotCtns = Convert.ToInt32(dtCtns.Rows[0]["TotCtns"]);
                                                    lintTotIBSQty = Convert.ToInt32(dtCtns.Rows[0]["TotIBSQty"]);
                                                }
                                                rd.SetParameterValue("fmlTotCtns", lintTotCtns);
                                                rd.SetParameterValue("fmlTotIBSQty", lintTotIBSQty);

                                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                                if (type == "PDF")
                                                {
                                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                                    reportFileName = l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                                }
                                                else if (type == "XLS")
                                                {
                                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".xls";
                                                    rd.ExportToDisk(ExportFormatType.ExcelRecord, strFileName);
                                                    reportFileName = l_str_rptdtl + "_" + strDateFormat + ".xls";
                                                }
                                            }
                                        }
                                        Session["RptFileName"] = strFileName;
                                    }

                                    else
                                    if (type == "Excel")
                                    {

                                        objBillingInquiry = ServiceObject.GetBillingBillInoutCartonRpt(objBillingInquiry);

                                        List<BILLING_INOUT_BILLDOC_CTN_EXCEL> li = new List<BILLING_INOUT_BILLDOC_CTN_EXCEL>();
                                        for (int i = 0; i < objBillingInquiry.ListBillingInoutCartonRpt.Count; i++)
                                        {

                                            BILLING_INOUT_BILLDOC_CTN_EXCEL objOBInquiryExcel = new BILLING_INOUT_BILLDOC_CTN_EXCEL();
                                            objOBInquiryExcel.Line = objBillingInquiry.ListBillingInoutCartonRpt[i].Line;
                                            objOBInquiryExcel.ServiceId = objBillingInquiry.ListBillingInoutCartonRpt[i].ServiceId;
                                            objOBInquiryExcel.ContId = objBillingInquiry.ListBillingInoutCartonRpt[i].ContId;
                                            objOBInquiryExcel.LotId = objBillingInquiry.ListBillingInoutCartonRpt[i].LotId;
                                            objOBInquiryExcel.Style = objBillingInquiry.ListBillingInoutCartonRpt[i].Style;
                                            objOBInquiryExcel.Color = objBillingInquiry.ListBillingInoutCartonRpt[i].Color;
                                            objOBInquiryExcel.Size = objBillingInquiry.ListBillingInoutCartonRpt[i].Size;
                                            objOBInquiryExcel.Rate = objBillingInquiry.ListBillingInoutCartonRpt[i].Rate;
                                            objOBInquiryExcel.TotalAmount = objBillingInquiry.ListBillingInoutCartonRpt[i].Amount;

                                            li.Add(objOBInquiryExcel);
                                        }

                                        GridView gv = new GridView();
                                        gv.DataSource = li;
                                        gv.DataBind();
                                        Session["INVOICE_DOC"] = gv;
                                        return new DownloadFileActionResult((GridView)Session["INVOICE_DOC"], "INVOICE_DOC" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");

                                    }
                                    //}

                                }
                                if (l_str_rpt_bill_inout_type.Trim() == "Cube")

                                {

                                    strReportName = "rpt_inout_bill_doc_bycube.rpt";
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry = ServiceObject.GetBillingBillInoutCubeRpt(objBillingInquiry);

                                    if (objBillingInquiry.ib_doc_id == null || objBillingInquiry.ib_doc_id == "")
                                    {
                                        l_str_rptdtl = objBillingInquiry.cmp_id + "_" + "IN-OUT BILL" + "-" + SelectdID;
                                        objEmail.EmailSubject = objBillingInquiry.cmp_id + "-" + "IN-OUT BILL " + "|" + " " + "Invoice#: " + SelectdID;
                                        objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Bill Type: " + l_str_rpt_bill_doc_type + "\n" + "Invoice#: " + " " + " " + SelectdID + "\n";

                                    }
                                    else
                                    {
                                        l_str_rptdtl = objBillingInquiry.cmp_id + "_" + "IN-OUT BILL" + "-" + objBillingInquiry.ib_doc_id;
                                        objEmail.EmailSubject = objBillingInquiry.cmp_id + "-" + "IN-OUT BILL" + "|" + " " + "Invoice#: " + SelectdID + "|" + " " + "Invoice:  " + "$" + objBillingInquiry.bill_amt + "|" + " " + "IB Doc ID#: " + objBillingInquiry.ib_doc_id + "|" + " " + "Received Date: " + objBillingInquiry.Bill_doc_dt_Fr;
                                        objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Bill Type: " + SelectdID + "\n" + "Invoice#: " + " " + " " + objBillingInquiry.bill_doc_id + "\n" + "Invoice: " + "$" + objBillingInquiry.bill_amt + "\n" + "IB Doc ID#: " + " " + " " + objBillingInquiry.ib_doc_id + "\n" + "Received Date: " + " " + " " + objBillingInquiry.Bill_doc_dt_Fr + "\n" + "Total Cartons: " + objBillingInquiry.ship_ctns;

                                    }
                                    if (type == "PDF")
                                    {
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
                                                rd.SetParameterValue("fml_rpt_title", "INVOICE");
                                                rd.SetParameterValue("fml_rpt_bill_title", "(INBOUND BILL BY CUBE)");
                                                rd.SetParameterValue("fml_rpt_param_bill_num", "Invoice#");
                                                rd.SetParameterValue("fml_rpt_param_bill_date", "Invoice Dt");
                                                //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//" + objBillingInquiry.cmp_id + "-INOUT-BILL-" + SelectdID + "-" + strDateFormat + ".pdf";
                                                // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                            }
                                        }
                                        reportFileName = objBillingInquiry.cmp_id + "-INBOUND-BILL-" + SelectdID + "-" + strDateFormat + ".pdf";
                                        Session["RptFileName"] = strFileName;
                                    }

                                    else
                                    if (type == "XLS")
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
                                        Session["INVOICE_DOC"] = gv;
                                        return new DownloadFileActionResult((GridView)Session["INVOICE_DOC"], "INVOICE_DOC-" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
                                    }
                                    //}

                                }
                                if (l_str_rpt_bill_inout_type.Trim() == "Container")

                                {

                                    strReportName = "rpt_inout_bill_by_Container.rpt";
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;


                                    if (type == "PDF")
                                    {
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
                                                //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "TempReports//IV_DOC_INQ_" + strDateFormat + ".pdf";
                                                // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, l_str_rpt_selection);
                                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                            }
                                        }
                                        reportFileName = "BL_INQ_" + strDateFormat + ".pdf";//CR2018-03-15-001 Added By Soniya
                                        Session["RptFileName"] = strFileName;
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
                                        Session["INVOICE_DOC"] = gv;
                                        return new DownloadFileActionResult((GridView)Session["INVOICE_DOC"], "INVOICE_DOC-" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



                                    }

                                }
                            }
                            else if (l_str_rpt_bill_doc_type == "NORM")
                            {
                                //string l_str_cmp_id = string.Empty;
                                //l_str_cmp_id = objBillingInquiry.cmp_id;
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

                                    }
                                    else
                                    {
                                        strReportName = "rpt_va_bill_doc.rpt";

                                    }
                                    //strReportName = "rpt_va_bill_doc_FH_NJ.rpt";
                                }
                                else
                                {
                                    if (type == "PDF")
                                    {
                                        strReportName = "rpt_va_bill_doc.rpt";
                                    }
                                    else
                                    {
                                        strReportName = "rpt_va_bill_doc_Excel.rpt";
                                    }
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
                                objBillingInquiry.cmp_id = p_str_cmp_id;
                                objBillingInquiry.Bill_doc_id = SelectdID;
                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0421_001 
                                objBillingInquiry = ServiceObject.GetBillingBillDocVASRpt(objBillingInquiry);
                                objBillingInquiry.Bill_doc_dt_Fr = objBillingInquiry.ListBillingDocVASRpt[0].rcvd_dt.ToShortDateString();
                                objBillingInquiry.bill_doc_id = objBillingInquiry.ListBillingDocVASRpt[0].bill_doc_id;
                                objBillingInquiry = ServiceObject.GetBillingBillamountInoutCartonRpt(objBillingInquiry);
                                objBillingInquiry.bill_amt = objBillingInquiry.ListBillingamountInoutCartonRpt[0].bill_amt;
                                objBillingInquiry.ship_ctns = objBillingInquiry.ListBillingamountInoutCartonRpt[0].ship_ctns;
                                l_str_rptdtl = objBillingInquiry.cmp_id + "_" + "VAS INVOICE" + "_" + objBillingInquiry.bill_doc_id;
                                objEmail.EmailSubject = objBillingInquiry.cmp_id + "-" + "VAS INVOICE" + "|" + " " + "Invoice#: " + objBillingInquiry.bill_doc_id + "|" + " " + "Invoice Amount:  " + "$" + objBillingInquiry.bill_amt;
                                objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Bill Type: " + l_str_rpt_bill_doc_type + "\n" + "Invoice#: " + " " + " " + objBillingInquiry.bill_doc_id + "\n" + "Invoice Amount: " + "$" + objBillingInquiry.bill_amt;

                                if (type == "PDF" || (type == "XLS"))
                                {
                                    using (ReportDocument rd = new ReportDocument())
                                    {
                                        var rptSource = objBillingInquiry.ListBillingDocVASRpt.ToList();
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
                                            if (type == "PDF")
                                            {
                                                rd.SetParameterValue("fml_rpt_title", "INVOICE");
                                                rd.SetParameterValue("fml_rpt_bill_title", "(VAS BILL)");
                                                rd.SetParameterValue("fml_rpt_param_bill_num", "Invoice#");
                                                rd.SetParameterValue("fml_rpt_param_bill_date", "Invoice Dt");
                                            }
                                        }

                                        //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                        if (type == "PDF")
                                        {
                                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + tempFilepath + "//INVOICE-" + SelectdID + "-" + strDateFormat + ".pdf";
                                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                        }
                                        else
                                        {
                                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + tempFilepath + "//INVOICE-" + SelectdID + "-"+ strDateFormat + ".xls";
                                            rd.ExportToDisk(ExportFormatType.Excel, strFileName);
                                        }
                                    }
                                    if (type == "PDF")
                                    {
                                        reportFileName = "INVOICE-"+ SelectdID + "-" + strDateFormat + ".pdf";
                                        Session["RptFileName"] = strFileName;
                                    }
                                    if (type == "XLS")
                                    {
                                        reportFileName = "INVOICE-" + SelectdID + "-" + strDateFormat + ".xls";
                                        Session["RptFileName"] = strFileName;
                                    }

                                }


                            }
                            else if (l_str_rpt_bill_doc_type == "STRG")
                            {
                                if (l_str_rpt_bill_type.Trim() == "Carton")
                                {
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry = ServiceObject.GetBillingBillDocSTRGRpt(objBillingInquiry);
                                   
                                    var rptSource = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.ToList();
                                    if (type == "PDF")
                                    {
                                        if (rptSource.Count > 0)
                                        {
                                            using (ReportDocument rd = new ReportDocument())
                                            {
                                                strReportName = "rpt_st_bill_doc.rpt";
                                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                                                rd.Load(strRptPath);
                                                rd.SetDataSource(rptSource);
                                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                                rd.SetParameterValue("fml_rpt_title", "INVOICE");
                                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + tempFilepath + "//-" + p_str_cmp_id + "-INV-" + SelectdID + "-" + strDateFormat + ".pdf";
                                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                                reportFileName = p_str_cmp_id + "-INV-" + SelectdID + "-" + strDateFormat + ".pdf";
                                                Session["RptFileName"] = strFileName;

                                                l_str_rptdtl = objBillingInquiry.cmp_id + "-" + "STRG-BILL" + "-" + SelectdID;

                                                objEmail.EmailSubject = objBillingInquiry.cmp_id + "-" + "STORAGE INVOICE " + "|" + " " + "Invoice#: " + SelectdID;

                                                objEmail.EmailMessage = "Hi All," + "\n\n" + " Please find the attached Storage Invoice document" + "\n\n";
                                                objEmail.EmailMessage = objEmail.EmailMessage + "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Bill Type: " + l_str_rpt_bill_doc_type + "\n" + "Invoice#: " + " " + " " + SelectdID + "\n";


                                            }
                                        }
                                    }

                                    else if (type == "XLS")
                                    {
                                        List<BILLING_STRG_BILLDOC_CRTN_EXCEL> li = new List<BILLING_STRG_BILLDOC_CRTN_EXCEL>();
                                        for (int i = 0; i < objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.Count; i++)
                                        {

                                            BILLING_STRG_BILLDOC_CRTN_EXCEL objOBInquiryExcel = new BILLING_STRG_BILLDOC_CRTN_EXCEL();
                                            objOBInquiryExcel.LineNo = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].dtl_line;
                                            objOBInquiryExcel.Desc = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].itm_name;
                                            objOBInquiryExcel.Style = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].so_itm_num;
                                            objOBInquiryExcel.Color = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].so_itm_color;
                                            objOBInquiryExcel.Size = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].so_itm_size;
                                            objOBInquiryExcel.Ctns = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].ship_ctns;
                                            objOBInquiryExcel.Rate = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].so_itm_price;
                                            objOBInquiryExcel.Amount = (objOBInquiryExcel.Ctns) * (objOBInquiryExcel.Rate);

                                            li.Add(objOBInquiryExcel);
                                        }

                                        GridView gv = new GridView();
                                        gv.DataSource = li;
                                        gv.DataBind();
                                        Session["INVOICE_DOC"] = gv;
                                        return new DownloadFileActionResult((GridView)Session["INVOICE_DOC"], "INVOICE_DOC" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
                                    }
                                    else
                                    {
                                        Response.Write("Please select the mode!");
                                    }
                                    //}

                                }

                                if (l_str_rpt_bill_type.Trim() == "Cube")

                                {
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;

                                    if (type == "PDF")
                                    {
                                        strReportName = "rpt_st_bill_doc_bycube.rpt";
                                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                        objBillingInquiry.cmp_id = p_str_cmp_id;
                                        objBillingInquiry.Bill_doc_id = SelectdID;
                                        objBillingInquiry = ServiceObject.GetBillingBillDocCubeSTRGRpt(objBillingInquiry);
                                        objBillingInquiry.Bill_doc_dt_Fr = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt[0].rcvd_dt.ToShortDateString();
                                        objBillingInquiry.bill_doc_id = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt[0].bill_doc_id;
                                        objBillingInquiry = ServiceObject.GetBillingBillamountInoutCartonRpt(objBillingInquiry);
                                        objBillingInquiry.bill_amt = objBillingInquiry.ListBillingamountInoutCartonRpt[0].bill_amt;
                                        objBillingInquiry.ship_ctns = objBillingInquiry.ListBillingamountInoutCartonRpt[0].ship_ctns;
                                        l_str_rptdtl = objBillingInquiry.cmp_id + "-" + "STRG-INV" + "-" + objBillingInquiry.bill_doc_id;

                                        objEmail.EmailSubject = objBillingInquiry.cmp_id + "-" + "STORAGE Invoice " + "|" + " " + "Invoice#: " + objBillingInquiry.bill_doc_id + "|" + " " + "Invoice:  " + "$" + objBillingInquiry.bill_amt;

                                        objEmail.EmailMessage = "Hi All," + "\n\n" + " Please find the attached Storage Invoice document" + "\n\n";
                                        objEmail.EmailMessage = objEmail.EmailMessage + "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Bill Type: " + l_str_rpt_bill_doc_type + "\n" + "Invoice#: " + " " + " " + objBillingInquiry.bill_doc_id + "\n" + "Invoice: " + "$" + objBillingInquiry.bill_amt + "\n" + "Total Cartons: " + objBillingInquiry.ship_ctns;



                                        objBillingInquiry = ServiceObject.GetBillingBillDocCubeSTRGRpt(objBillingInquiry);
                                        var rptSource = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.ToList();
                                        //objEmail.EmailSubject = objBillingInquiry.cmp_id + "-" + "BILL INVOICE REPORT ";
                                        //objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name;

                                        if (rptSource.Count > 0)
                                        {
                                            using (ReportDocument rd = new ReportDocument())
                                            {
                                                strReportName = "rpt_st_bill_doc_bycube.rpt";
                                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                                //string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                                rd.Load(strRptPath);
                                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                                rd.SetDataSource(rptSource);
                                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                                rd.SetParameterValue("fml_rpt_title", "INVOICE");

                                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/")  +tempFilepath + "//INVOICE-" + p_str_cmp_id + "-INV-" + SelectdID + "-" + strDateFormat + ".pdf";
                                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                                reportFileName = p_str_cmp_id + "-INV-" + SelectdID + "-" + strDateFormat + ".pdf";
                                                Session["RptFileName"] = strFileName;


                                             
                                            }
                                        }
                                    }

                                    else if (type == "XLS")
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
                                        Session["INVOICE_DOC"] = gv;
                                        return new DownloadFileActionResult((GridView)Session["INVOICE_DOC"], "INVOICE_DOC" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
                                    }
                                    else
                                    {
                                        Response.Write("Please select the mode!");
                                    }

                                }

                            }






                        }
                    }

                    if (l_str_rpt_selection == "GridSummary")
                    {
                        strReportName = "rpt_bill_summary.rpt";
                        BillingInquiry objBillingInquiry = new BillingInquiry();
                        BillingInquiryService ServiceObject = new BillingInquiryService();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        objBillingInquiry.cmp_id = p_str_cmp_id;
                        objBillingInquiry.Bill_doc_id = p_str_Bill_doc_id;
                        objBillingInquiry.bill_type = p_str_Bill_type;
                        objBillingInquiry.Bill_doc_dt_Fr = p_str_doc_dt_Fr;
                        objBillingInquiry.Bill_doc_dt_To = p_str_doc_dt_To;
                        objBillingInquiry = ServiceObject.GetBillingSummaryRpt(objBillingInquiry);
                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        objEmail.EmailSubject = objBillingInquiry.cmp_id + "-" + "GRID SUMMARY REPORT " + "|" + " " + "Invoice#: " + objBillingInquiry.bill_doc_id + "|" + " " + "Invoice:  " + "$" + objBillingInquiry.bill_amt + "|" + " " + "Received Date: " + objBillingInquiry.Bill_doc_dt_Fr;
                        objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name;
                        if (type == "PDF")
                        {
                            var rptSource = objBillingInquiry.ListBillingSummaryRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objBillingInquiry.ListBillingSummaryRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + tempFilepath + "//BillingInquiry GridSummary_" + strDateFormat + ".pdf";
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                }
                            }
                            reportFileName = "BillingInquiry GridSummary Report " + DateTime.Now.ToFileTime() + ".pdf";
                            Session["RptFileName"] = strFileName;
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
                    if (objEmail.EmailSubject.Length > 0)
                    {
                        objEmail.EmailSubject = "Bill Invoice Report for" + " " + " " + p_str_cmp_id;
                        objEmail.EmailMessage = "Please find the attached Invoice";
                    }
                }
                else
                {
                    objEmail.Attachment = reportFileName;
                    objEmail.EmailTo = "";
                    objEmail.EmailCC = "";

                }

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
        public ActionResult Billingdtl(string BilldocId, string cmp_id, string Billdocdate, string p_str_viewmode, string datefrom, string dateto, string Type, string IBDocID, string BillType)
        {
            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();
            string l_str_status = string.Empty;
            string l_str_rpt_bill_type = string.Empty;
            string l_str_rpt_bill_inout_type = string.Empty;
            string l_str_rpt_instrg_req = string.Empty;
            string l_str_rpt_bill_doc_type = string.Empty;
            objBillingInquiry.cust_id = cmp_id;
            objBillingInquiry.Bill_doc_id = BilldocId;
            objBillingInquiry.Billdocdt = Billdocdate;
            objBillingInquiry.l_str_viewmode = p_str_viewmode;
            objBillingInquiry.Bill_Type = BillType;
            objBillingInquiry.blType = Type;
            objBillingInquiry.DocumentdateFrom = datefrom;
            objBillingInquiry.DocumentdateTo = dateto;
            if (IBDocID != null)
            {
                objBillingInquiry.ib_doc_id = IBDocID.Trim();
            }
            else
            {
                objBillingInquiry.ib_doc_id = "";
            }
            objBillingInquiry = ServiceObject.GetBillingHdr(objBillingInquiry);
            objBillingInquiry.CUSTId = objBillingInquiry.ListBillingdetail[0].CUSTId.Trim();
            objBillingInquiry.cust_of_cmpid = objBillingInquiry.ListBillingdetail[0].cmp_id.Trim();
            objBillingInquiry.status = objBillingInquiry.ListBillingdetail[0].status.Trim();
            if (objBillingInquiry.status.Trim() == "O")
            {
                objBillingInquiry.status = "OPEN";
            }
            else if (objBillingInquiry.status.Trim() == "P")
            {
                objBillingInquiry.status = "POST";
            }
            objBillingInquiry.bill_type = objBillingInquiry.ListBillingdetail[0].bill_type.Trim();
            objBillingInquiry.billto_id = objBillingInquiry.ListBillingdetail[0].billto_id.Trim();
            if (objBillingInquiry.ListBillingdetail[0].billperiodfr == null)
            {

            }
            else
            {
                objBillingInquiry.billperiodfr = objBillingInquiry.ListBillingdetail[0].billperiodfr.Trim();
            }
            objBillingInquiry.billperiodTo = objBillingInquiry.ListBillingdetail[0].billperiodTo.Trim();
            objBillingInquiry.term_code = objBillingInquiry.ListBillingdetail[0].term_code.Trim();
            objBillingInquiry.ship_via = objBillingInquiry.ListBillingdetail[0].ship_via.Trim();
            objBillingInquiry.proj_id = objBillingInquiry.ListBillingdetail[0].proj_id;
            objBillingInquiry.prod_cost = Math.Round(Convert.ToDecimal(objBillingInquiry.ListBillingdetail[0].prod_cost), 2);  //CR_3PL_MVC_BL_2018_0310_001   Modified by Soniya       
            objBillingInquiry.bill_amt = Math.Round(Convert.ToDecimal(objBillingInquiry.ListBillingdetail[0].bill_amt), 2);     //CR_3PL_MVC_BL_2018_0310_001   Modified by Soniya 
            objBillingInquiry.cmp_id = cmp_id;
            objBillingInquiry = ServiceObject.GetBillingBillingType(objBillingInquiry);
            l_str_rpt_bill_type = objBillingInquiry.ListBillingType[0].bill_type;
            objBillingInquiry = ServiceObject.GetBillingInoutType(objBillingInquiry);
            l_str_rpt_bill_inout_type = objBillingInquiry.ListBillingInoutType[0].bill_inout_type;
            l_str_rpt_instrg_req = objBillingInquiry.ListBillingType[0].init_strg_rt_req;
            objBillingInquiry.cmp_id = cmp_id;
            objBillingInquiry.Bill_doc_id = BilldocId;
            objBillingInquiry = ServiceObject.GetBillingBillDocIdType(objBillingInquiry);
            l_str_rpt_bill_doc_type = objBillingInquiry.ListBillingDocIdType[0].bill_type;
            if (l_str_rpt_bill_doc_type == "STRG")
            {
                if (l_str_rpt_bill_type == "Pallet")
                {
                    objBillingInquiry = ServiceObject.GetSTRGBillRcvdDtlByPallet(objBillingInquiry);
                    objBillingInquiry.Grid_Bill_Type = "PALLET";
                }
                else
                {
                    objBillingInquiry = ServiceObject.GetBillingdtl(objBillingInquiry);
                }
            }
            else
            {
                if (l_str_rpt_bill_inout_type == "Container")
                {
                    objBillingInquiry = ServiceObject.GetSTRGBillRcvdDtlByContainer(objBillingInquiry);
                    objBillingInquiry.Grid_Bill_Type = "CNTR";
                }
                else
                {
                    objBillingInquiry = ServiceObject.GetBillingdtl(objBillingInquiry);

                }
            }
            Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
            BillingInquiryModel BillingInquiryModel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);
            return PartialView("_BillingRcvdDetail", BillingInquiryModel);
        }


        public ActionResult BillRegenerate(string p_str_cmp_id, string p_str_cust_id, string p_str_bill_doc_id, string p_str_bill_doc_dt,
            string p_str_bill_from_dt, string p_str_bill_to_dt, string p_str_bill_type)
        {
            string l_str_bill_amount = string.Empty;
            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();
            objBillingInquiry.cmp_id = p_str_cmp_id;
            objBillingInquiry.cust_id = p_str_cust_id;
            objBillingInquiry.bill_pd_fm = p_str_bill_from_dt;
            objBillingInquiry.bill_pd_to = p_str_bill_to_dt;
            objBillingInquiry.bill_doc_id = p_str_bill_doc_id;
            objBillingInquiry.BillDocId = p_str_bill_doc_id;
            objBillingInquiry.RateType = p_str_bill_type;
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cust_of_cmp_id = "";
            objCompany.cmp_id = p_str_cust_id;
            objCompany = ServiceObjectCompany.GetCustOfCompName(objCompany);
            objBillingInquiry.LstCustOfCmpName = objCompany.LstCustOfCmpName;
            objBillingInquiry.cust_of_cmpid = objBillingInquiry.LstCustOfCmpName[0].cust_of_cmpid;

            objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
            objBillingInquiry.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objBillingInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;



            objBillingInquiry.bill_doc_dt = p_str_bill_doc_dt;
            objBillingInquiry.billdocdt = p_str_bill_doc_dt;
            objBillingInquiry.Billdocdt = p_str_bill_doc_dt;
            objBillingInquiry.Bill_DOC_DT = p_str_bill_doc_dt;

            if (p_str_bill_type == "INOUT")
            {
                objBillingInquiry = ServiceObject.GetRegenerateInoutBillDetails(objBillingInquiry);
                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry.cust_of_cmpid = p_str_cust_id;
                Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
                BillingInquiryModel BillingInquiryModel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);
                return PartialView("_BillRegenerate", BillingInquiryModel);
            }

            else if (p_str_bill_type == "NORM")
            {
                objBillingInquiry.cmp_id = p_str_cust_id;
                objBillingInquiry.bill_as_of_dt = p_str_bill_doc_dt;
                ClsBillVASReBill objBillVASReBill = new ClsBillVASReBill();
                objBillVASReBill.bill_from_dt = p_str_bill_from_dt;
                objBillVASReBill.bill_to_dt = p_str_bill_to_dt;
                objBillVASReBill.re_bill_doc_id = p_str_bill_doc_id;

                objBillVASReBill.lstBillVASList = ServiceObject.getVASBillRegenerate(p_str_cust_id, p_str_bill_doc_id, p_str_bill_from_dt, p_str_bill_to_dt);
                objBillingInquiry.lstBillVASList = objBillVASReBill.lstBillVASList;
                objBillingInquiry.objBillVASReBill = objBillVASReBill;

                objBillingInquiry.cust_of_cmpid = p_str_cmp_id;
                Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
                BillingInquiryModel BillingInquiryModel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);
                return PartialView("_VASBillRegenerate", BillingInquiryModel);

            }
            else
            {
                objBillingInquiry = ServiceObject.GetRegenerateInoutBillDetails(objBillingInquiry);
                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry.cust_of_cmpid = p_str_cust_id;
                Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
                BillingInquiryModel BillingInquiryModel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);
                return PartialView("_BillRegenerate", BillingInquiryModel);
            }

        }

        public ActionResult getVASBillRegenerateDetails(string p_str_cmp_id, string p_str_bill_doc_id, string p_str_bill_from_dt, string p_str_bill_to_dt)
        {
            try
            {
                string l_str_bill_amount = string.Empty;
                BillingInquiry objBillingInquiry = new BillingInquiry();
                BillingInquiryService ServiceObject = new BillingInquiryService();
                ClsBillVASReBill objBillVASReBill = new ClsBillVASReBill();

                objBillVASReBill.lstBillVASList = ServiceObject.getVASBillRegenerate(p_str_cmp_id, p_str_bill_doc_id, p_str_bill_from_dt, p_str_bill_to_dt);
                objBillingInquiry.lstBillVASList = objBillVASReBill.lstBillVASList;
                objBillingInquiry.objBillVASReBill = objBillVASReBill;
                objBillingInquiry = ServiceObject.GetConsolidateVASInqDetails(objBillingInquiry);
                Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
                BillingInquiryModel BillingInquiryModel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);
                return PartialView("_GrdVASBillRegenerate", BillingInquiryModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult BillRegenerateSearch(string p_str_cmp_id, string p_str_cust_id, string p_str_bill_doc_id, string p_str_bill_doc_dt,
            string p_str_bill_from_dt, string p_str_bill_to_dt, string p_str_bill_type)
        {
            string l_str_bill_amount = string.Empty;
            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();
            objBillingInquiry.cmp_id = p_str_cmp_id;
            objBillingInquiry.cust_id = p_str_cust_id;
            objBillingInquiry.bill_pd_fm = p_str_bill_from_dt;
            objBillingInquiry.bill_pd_to = p_str_bill_to_dt;
            objBillingInquiry.bill_doc_id = p_str_bill_doc_id;
            objBillingInquiry.BillDocId = p_str_bill_doc_id;
            objBillingInquiry.RateType = p_str_bill_type;
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cust_of_cmp_id = "";
            objCompany.cmp_id = p_str_cust_id;
            objCompany = ServiceObjectCompany.GetCustOfCompName(objCompany);
            objBillingInquiry.LstCustOfCmpName = objCompany.LstCustOfCmpName;
            objBillingInquiry.cust_of_cmpid = objBillingInquiry.LstCustOfCmpName[0].cust_of_cmpid;

            objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
            objBillingInquiry.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objBillingInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;



            objBillingInquiry.bill_doc_dt = p_str_bill_doc_dt;
            objBillingInquiry.billdocdt = p_str_bill_doc_dt;
            objBillingInquiry.Billdocdt = p_str_bill_doc_dt;
            objBillingInquiry.Bill_DOC_DT = p_str_bill_doc_dt;
            objBillingInquiry = ServiceObject.GetRegenerateInoutBillDetails(objBillingInquiry);
            objBillingInquiry.cmp_id = p_str_cmp_id;
            objBillingInquiry.cust_of_cmpid = p_str_cust_id;
            Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
            BillingInquiryModel BillingInquiryModel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);
            return PartialView("_BillRegenerateGrid", BillingInquiryModel);

        }

        public ActionResult SaveRegenerateInoutBill(string p_str_cust_id, List<clsLotId> plstLotId, string p_str_Bill_type, string p_str_Bill_doc_id, string p_str_bill_as_of_dt, string p_str_doc_dt_Fr, string p_str_doc_dt_To, string p_str_cmp_id, string p_str_print_dt)
        {
            string l_str_rpt_bill_type = string.Empty;
            objBillingInquiry.cmp_id = p_str_cmp_id;
            objBillingInquiry.cust_id = p_str_cust_id;
            objBillingInquiry.bill_type = p_str_Bill_type;
            objBillingInquiry.CUSTId = p_str_cust_id;
            objBillingInquiry.Bill_doc_id = p_str_Bill_doc_id;
            objBillingInquiry = ServiceObject.GetBillDelete(objBillingInquiry);

            objBillingInquiry.temp_bill_doc_id = p_str_Bill_doc_id;

            int l_int_count = 0;
            l_int_count = plstLotId.Count;
            string l_str_lot_id = string.Empty;

            for (int i = 0; i < l_int_count; i++)
            {
                l_str_lot_id = plstLotId[i].lot_id.ToString();
                if (l_str_lot_id == "on")
                {
                }
                else
                {
                    objBillingInquiry.temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id;
                    objBillingInquiry.lot_id = l_str_lot_id;
                    objBillingInquiry = ServiceObject.SaveConsolidateInoutBillDetails(objBillingInquiry);

                }
            }

            objBillingInquiry.bill_as_of_date = p_str_bill_as_of_dt;
            objBillingInquiry.print_bill_date = p_str_print_dt;
            objBillingInquiry.inout_bill_pd_fm = p_str_doc_dt_Fr;
            objBillingInquiry.inout_bill_pd_to = p_str_doc_dt_To;
            objBillingInquiry.ib_doc_id = string.Empty;
            objBillingInquiry.cmp_id = p_str_cust_id;
            objBillingInquiry = ServiceObject.GetBillingBillingType(objBillingInquiry);
            l_str_rpt_bill_type = objBillingInquiry.ListBillingType[0].bill_type;
            objBillingInquiry.BillFor = l_str_rpt_bill_type;
            objBillingInquiry.init_strg_rt_req = string.Empty;
            objBillingInquiry.cmp_id = p_str_cmp_id;
            objBillingInquiry.bill_doc_id = p_str_Bill_doc_id;
            objBillingInquiry = ServiceObject.GenerateInOutBillByCarton(objBillingInquiry);

            objBillingInquiry.bill_doc_id = objBillingInquiry.ListSaveSTRGBillDetails[0].bill_doc_id.ToString().Trim();
            Session["sess_bill_doc_id"] = string.Empty;
            return Json(objBillingInquiry.bill_doc_id, JsonRequestBehavior.AllowGet);
        }


        public ActionResult BillingDelete(string p_str_cmp_id, string p_str_bill_doc_id, string p_str_status, string p_str_cust_id)
        {
            bool l_str_return_value = false;
            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();
            string l_str_status = string.Empty;
            string l_str_rpt_bill_type = string.Empty;
            string l_str_rpt_bill_inout_type = string.Empty;
            string l_str_rpt_instrg_req = string.Empty;
            string l_str_rpt_bill_doc_type = string.Empty;

            objBillingInquiry.cmp_id = p_str_cust_id;
            objBillingInquiry = ServiceObject.GetBillingBillingType(objBillingInquiry);
            l_str_rpt_bill_type = objBillingInquiry.ListBillingType[0].bill_type;
            objBillingInquiry = ServiceObject.GetBillingInoutType(objBillingInquiry);
            l_str_rpt_bill_inout_type = objBillingInquiry.ListBillingInoutType[0].bill_inout_type;
            l_str_rpt_instrg_req = objBillingInquiry.ListBillingType[0].init_strg_rt_req;
            objBillingInquiry.cmp_id = p_str_cust_id;
            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
            objBillingInquiry = ServiceObject.GetBillingBillDocIdType(objBillingInquiry);
            l_str_rpt_bill_doc_type = objBillingInquiry.ListBillingDocIdType[0].bill_type;
            objBillingInquiry.cmp_id = p_str_cmp_id;
            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
            l_str_status = p_str_status;

            objBillingInquiry.CUSTId = p_str_cust_id;
            if (l_str_status == "OPEN" || l_str_status == "VOID")
            {
                if (l_str_rpt_bill_doc_type == "STRG")
                {
                    if (l_str_rpt_bill_type == "Pallet")
                    {
                        objBillingInquiry = ServiceObject.GetBillDeleteByPallet(objBillingInquiry);
                        //objBillingInquiry.Success = "Bill Doc Num: " + p_str_bill_doc_id + "Deleted Successfully";
                        l_str_return_value = true;
                    }
                    else
                    {
                        objBillingInquiry = ServiceObject.GetBillDelete(objBillingInquiry);

                        //objBillingInquiry.Success = "Bill Doc Num: " + p_str_bill_doc_id + "Deleted Successfully";
                        l_str_return_value = true;

                    }

                }
                //CR-2018-05-02-001 Added By Nithya
                else if (l_str_rpt_bill_doc_type == "NORM")
                {
                    objBillingInquiry = ServiceObject.GetVASBillDelete(objBillingInquiry);
                    //objBillingInquiry.Success = "Bill Doc Num: " + p_str_bill_doc_id + "Deleted Successfully";
                    l_str_return_value = true;
                }
                //END
                else
                {
                    if (l_str_rpt_bill_inout_type == "Container")
                    {
                        objBillingInquiry = ServiceObject.GetBillDelete(objBillingInquiry);
                        //objBillingInquiry.Success = "Bill Doc Num: " + p_str_bill_doc_id + "Deleted Successfully";
                        l_str_return_value = true;

                    }
                    else
                    {
                        objBillingInquiry = ServiceObject.GetBillDelete(objBillingInquiry);
                        //objBillingInquiry.Success = "Bill Doc Num: " + p_str_bill_doc_id + "Deleted Successfully";
                        l_str_return_value = true;

                    }
                }
            }
            //else { 
            //objBillingInquiry.Success = "Bill Doc Num: "+ p_str_bill_doc_id + " Already Posted. Cannot Modify";
            //}
            return Json(l_str_return_value, JsonRequestBehavior.AllowGet);
            //Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
            //BillingInquiryModel BillingInquiryModel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);
            //return PartialView("_BillingRcvdDetail", BillingInquiryModel);
        }
        public ActionResult BillingPost(string p_str_cmp_id, string p_str_bill_doc_id, string p_str_status)
        {
            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();
            String l_str_status = String.Empty;
            objBillingInquiry.cmp_id = p_str_cmp_id;
            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
            l_str_status = p_str_status;

            if (l_str_status == "OPEN")
            {
                objBillingInquiry = ServiceObject.GetBillPost(objBillingInquiry);
                objBillingInquiry.Success = "Bill Doc Num: " + p_str_bill_doc_id + "Posted Successfully";
            }
            else
            {
                objBillingInquiry.Success = "Bill Doc Num: " + p_str_bill_doc_id + " Already Posted. Cannot Modify";
            }
            return Json(objBillingInquiry.Success, JsonRequestBehavior.AllowGet);
            //Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
            //BillingInquiryModel BillingInquiryModel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);
            //return PartialView("_BillingRcvdDetail", BillingInquiryModel);
        }
        public ActionResult ShowdtlReports(string p_str_cmpid, string p_str_status, string p_str_bill_doc_id, string p_str_type)
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
                                    if (p_str_type == "PDF")
                                    {
                                        strReportName = "rpt_st_bill_doc.rpt";
                                    }
                                    else
                                    {
                                        strReportName = "rpt_st_bill_doc_Excel.rpt";
                                    }
                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmpid;
                                    objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
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
                                            rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                                            if (p_str_type == "PDF")
                                            {
                                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                            }
                                            else
                                            {
                                                rd.ExportToHttpResponse(ExportFormatType.Excel, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (p_str_type == "PDF")
                                    {
                                        strReportName = "rpt_st_bill_doc.rpt";
                                    }
                                    else
                                    {
                                        strReportName = "rpt_st_bill_doc_Excel.rpt";
                                    }

                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmpid;
                                    objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                                    objBillingInquiry = ServiceObject.GetBillingBillDocSTRGRpt(objBillingInquiry);

                                    if (p_str_type == "PDF")
                                    {
                                        var rptSource = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.ToList();
                                        if (rptSource.Count > 0)
                                        {
                                            using (ReportDocument rd = new ReportDocument())
                                            {

                                                rd.Load(strRptPath);
                                                rd.SetDataSource(rptSource);

                                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                                rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                            }
                                        }
                                    }
                                    else if (p_str_type == "XLS")
                                    {
                                        var rptSource = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.ToList();
                                        if (rptSource.Count > 0)
                                        {
                                            using (ReportDocument rd = new ReportDocument())
                                            {
                                                rd.Load(strRptPath);
                                                rd.SetDataSource(rptSource);

                                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                                rd.ExportToHttpResponse(ExportFormatType.Excel, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        objBillingInquiry.cmp_id = p_str_cmpid;
                                        objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                                        objBillingInquiry = ServiceObject.GetBillingBillDocSTRGRpt(objBillingInquiry);

                                        List<BILLING_INOUT_BILLDOC_CTN_EXCEL> li = new List<BILLING_INOUT_BILLDOC_CTN_EXCEL>();
                                        for (int i = 0; i < objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.Count; i++)
                                        {

                                            BILLING_INOUT_BILLDOC_CTN_EXCEL objOBInquiryExcel = new BILLING_INOUT_BILLDOC_CTN_EXCEL();
                                            objOBInquiryExcel.Line = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].dtl_line;
                                            objOBInquiryExcel.ContId = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].cont_id;
                                            objOBInquiryExcel.LotId = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].lot_id;
                                            objOBInquiryExcel.ServiceId = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].notes;
                                            objOBInquiryExcel.Style = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].so_itm_num;
                                            objOBInquiryExcel.Color = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].so_itm_color;
                                            objOBInquiryExcel.Size = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].so_itm_size;
                                            objOBInquiryExcel.Ctns = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].ship_ctns;
                                            objOBInquiryExcel.Rate = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].so_itm_price;
                                            objOBInquiryExcel.TotalAmount = (objOBInquiryExcel.Ctns) * (objOBInquiryExcel.Rate);


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
                            if (l_str_rpt_bill_type.Trim() == "Cube")

                            {
                                if (l_str_rpt_instrg_req.Trim() == "Y")
                                {
                                    if (p_str_type == "PDF")
                                    {
                                        strReportName = "rpt_st_bill_doc_bycube.rpt";
                                    }
                                    else
                                    {
                                        strReportName = "rpt_st_bill_doc_bycube_Excel.rpt";
                                    }

                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmpid;
                                    objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
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
                                            rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                                            if (p_str_type == "PDF")
                                            {
                                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                            }
                                            else
                                            {
                                                rd.ExportToHttpResponse(ExportFormatType.Excel, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    strReportName = "rpt_st_bill_doc_bycube.rpt";
                                    if (p_str_type == "PDF")
                                    {
                                        strReportName = "rpt_st_bill_doc_bycube.rpt";
                                    }
                                    else
                                    {
                                        strReportName = "rpt_st_bill_doc_bycube_Excel.rpt";
                                    }

                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmpid;
                                    objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
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
                                            rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                        }
                                    }
                                }

                            }
                            //CR-3PL_MVC_BL_2018-03-10-001
                            if (l_str_rpt_bill_type.Trim() == "Pallet")

                            {
                                if (p_str_type == "PDF")
                                {
                                    strReportName = "rpt_strg_bill_by_pallet.rpt";
                                }
                                else
                                {
                                    strReportName = "rpt_strg_bill_by_pallet_Excel.rpt";
                                }

                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                objBillingInquiry.cmp_id = p_str_cmpid.Trim();
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
                                        if (p_str_type == "PDF")
                                        {
                                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                        }
                                        else if (p_str_type == "XLS")
                                        {
                                            rd.ExportToHttpResponse(ExportFormatType.Excel, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                        }
                                    }
                                }


                            }


                            if (l_str_rpt_bill_type.Trim() == "Location")

                            {
                                if (p_str_type == "PDF")
                                {
                                    strReportName = "rpt_st_bill_doc_location.rpt";
                                }
                                else
                                {
                                    strReportName = "rpt_st_bill_doc_location_Excel.rpt";
                                }

                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                objBillingInquiry.cmp_id = p_str_cmpid.Trim();
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
                                        if (p_str_type == "PDF")
                                        {
                                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                        }
                                        else if (p_str_type == "XLS")
                                        {
                                            rd.ExportToHttpResponse(ExportFormatType.Excel, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                        }
                                    }
                                }
                            }
                            if (l_str_rpt_bill_type.Trim() == "Pcs")

                            {
                                if (p_str_type == "PDF")
                                {
                                    strReportName = "rpt_st_bill_doc_Pcs_item.rpt";
                                }
                                else
                                {
                                    strReportName = "rpt_st_bill_doc_Pcs_item_Excel.rpt";
                                }

                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                objBillingInquiry.cmp_id = p_str_cmpid.Trim();
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
                                        if (p_str_type == "PDF")
                                        {
                                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                        }
                                        else if (p_str_type == "XLS")
                                        {
                                            rd.ExportToHttpResponse(ExportFormatType.Excel, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                        }
                                    }
                                }


                            }

                        }
                        if (l_str_rpt_bill_doc_type == "NORM")
                        {
                            //string l_str_cmp_id = string.Empty;
                            //l_str_cmp_id = objBillingInquiry.cmp_id;
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
                                if (p_str_type == "PDF")
                                {
                                    strReportName = "rpt_va_bill_doc.rpt";
                                }
                                else
                                {
                                    strReportName = "rpt_va_bill_doc_Excel.rpt";
                                }
                            }


                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmpid;
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0421_001 
                            objBillingInquiry = ServiceObject.GetBillingBillDocVASRpt(objBillingInquiry);
                            if (p_str_type == "PDF")
                            {

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

                                        if (strReportName == "rpt_va_bill_doc_Excel.rpt")
                                        { }
                                        else
                                        {
                                            rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                                            rd.SetParameterValue("fml_rpt_bill_title", "(VAS BILL)");
                                            rd.SetParameterValue("fml_rpt_param_bill_num", "Bill#");
                                            rd.SetParameterValue("fml_rpt_param_bill_date", "Bill Dt");
                                        }
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                    }
                                }
                            }

                            else if (p_str_type == "XLS")
                            {
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

                                        if (strReportName == "rpt_va_bill_doc_Excel.rpt")
                                        {
                                            rd.ExportToHttpResponse(ExportFormatType.Excel, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                        }
                                        else
                                        {
                                            rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                                            rd.SetParameterValue("fml_rpt_bill_title", "(VAS BILL)");
                                            rd.SetParameterValue("fml_rpt_param_bill_num", "Bill#");
                                            rd.SetParameterValue("fml_rpt_param_bill_date", "Bill Dt");
                                            rd.ExportToHttpResponse(ExportFormatType.Excel, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                        }
                                    }
                                }

                            }
                            else
                            {
                                objBillingInquiry.cmp_id = p_str_cmpid;
                                objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                                objBillingInquiry = ServiceObject.GetBillingBillDocVASRpt(objBillingInquiry);
                                List<BILLING_NORM_BILLDOC_EXCEL> li = new List<BILLING_NORM_BILLDOC_EXCEL>();
                                for (int i = 0; i < objBillingInquiry.ListBillingDocVASRpt.Count; i++)
                                {

                                    BILLING_NORM_BILLDOC_EXCEL objOBInquiryExcel = new BILLING_NORM_BILLDOC_EXCEL();
                                    objOBInquiryExcel.Line = objBillingInquiry.ListBillingDocVASRpt[i].dtl_line;
                                    objOBInquiryExcel.VASId = objBillingInquiry.ListBillingDocVASRpt[i].ship_doc_id;
                                    objOBInquiryExcel.ShipDate = objBillingInquiry.ListBillingDocVASRpt[i].ship_dt.ToString("MM/dd/yyyy");
                                    objOBInquiryExcel.CustOrder = objBillingInquiry.ListBillingDocVASRpt[i].CustOrder;
                                    objOBInquiryExcel.Notes = objBillingInquiry.ListBillingDocVASRpt[i].Notes;
                                    objOBInquiryExcel.VASDesc = objBillingInquiry.ListBillingDocVASRpt[i].ship_itm_num;
                                    objOBInquiryExcel.Ctns = objBillingInquiry.ListBillingDocVASRpt[i].ship_ctns;
                                    objOBInquiryExcel.Rate = objBillingInquiry.ListBillingDocVASRpt[i].so_itm_price;
                                    objOBInquiryExcel.Amount = (objOBInquiryExcel.Ctns) * (objOBInquiryExcel.Rate);
                                    li.Add(objOBInquiryExcel);
                                }
                                GridView gv = new GridView();
                                gv.DataSource = li;
                                gv.DataBind();
                                Session["BILL_DOC"] = gv;
                                return new DownloadFileActionResult((GridView)Session["BILL_DOC"], "BILL_DOC_" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");



                            }
                        }
                        if (l_str_rpt_bill_doc_type == "INOUT")
                        {

                            if (l_str_rpt_bill_inout_type.Trim() == "Carton")

                            {
                                if (l_str_rpt_instrg_req.Trim() == "Y")
                                {
                                    if (p_str_type == "PDF")
                                    {
                                        strReportName = "rpt_inout_bill_doc_with_initstrg.rpt";
                                    }
                                    else
                                    {
                                        strReportName = "rpt_inout_bill_doc_with_initstrg_Excel.rpt";
                                    }

                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry.cmp_id = p_str_cmpid;
                                    objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
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
                                            if (p_str_type == "PDF")
                                            {
                                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                            }
                                            else if (p_str_type == "XLS")
                                            {
                                                rd.ExportToHttpResponse(ExportFormatType.Excel, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (p_str_type == "PDF")
                                    {
                                        strReportName = "rpt_inout_bill_doc.rpt";
                                    }
                                    else
                                    {
                                        strReportName = "rpt_inout_bill_doc_Excel.rpt";
                                    }
                                    if ((p_str_type == "PDF") || (p_str_type == "XLS"))
                                    {

                                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                        objBillingInquiry.cmp_id = p_str_cmpid;
                                        objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
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

                                                if (strReportName == "rpt_inout_bill_doc_Excel.rpt")
                                                {

                                                }
                                                else
                                                {
                                                    rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                                                    rd.SetParameterValue("fml_rpt_bill_title", "(INOUT BILL BY CARTON)");
                                                    rd.SetParameterValue("fml_rpt_param_bill_num", "Bill#");
                                                    rd.SetParameterValue("fml_rpt_param_bill_date", "Bill Dt");
                                                    DataTable dtCtns = new DataTable();
                                                    int lintTotCtns = 0;
                                                    int lintTotIBSQty = 0;
                                                    dtCtns = ServiceObject.fnGetCtnsByBillDoc(p_str_bill_doc_id);
                                                    if (dtCtns.Rows.Count > 0)
                                                    {
                                                        lintTotCtns = Convert.ToInt32(dtCtns.Rows[0]["TotCtns"]);
                                                        lintTotIBSQty = Convert.ToInt32(dtCtns.Rows[0]["TotIBSQty"]);
                                                    }
                                                    rd.SetParameterValue("fmlTotCtns", lintTotCtns);
                                                    rd.SetParameterValue("fmlTotIBSQty", lintTotIBSQty);
                                                }


                                                if (p_str_type == "PDF")
                                                {
                                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                                }
                                                else if (p_str_type == "XLS")
                                                {
                                                    rd.ExportToHttpResponse(ExportFormatType.Excel, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                                }
                                            }
                                        }
                                    }

                                    else
                                    {
                                        objBillingInquiry.cmp_id = p_str_cmpid;
                                        objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                                        objBillingInquiry = ServiceObject.GetBillingBillInoutCartonRpt(objBillingInquiry);

                                        List<BILLING_INOUT_BILLDOC_CTN_EXCEL> li = new List<BILLING_INOUT_BILLDOC_CTN_EXCEL>();
                                        for (int i = 0; i < objBillingInquiry.ListBillingInoutCartonRpt.Count; i++)
                                        {

                                            BILLING_INOUT_BILLDOC_CTN_EXCEL objOBInquiryExcel = new BILLING_INOUT_BILLDOC_CTN_EXCEL();
                                            objOBInquiryExcel.Line = objBillingInquiry.ListBillingInoutCartonRpt[i].dtl_line;
                                            objOBInquiryExcel.ContId = objBillingInquiry.ListBillingInoutCartonRpt[i].cont_id;
                                            objOBInquiryExcel.LotId = objBillingInquiry.ListBillingInoutCartonRpt[i].lot_id;
                                            objOBInquiryExcel.ServiceId = objBillingInquiry.ListBillingInoutCartonRpt[i].so_num;
                                            objOBInquiryExcel.Style = objBillingInquiry.ListBillingInoutCartonRpt[i].so_itm_num;
                                            objOBInquiryExcel.Color = objBillingInquiry.ListBillingInoutCartonRpt[i].so_itm_color;
                                            objOBInquiryExcel.Size = objBillingInquiry.ListBillingInoutCartonRpt[i].so_itm_size;
                                            objOBInquiryExcel.Ctns = objBillingInquiry.ListBillingInoutCartonRpt[i].ship_ctns;
                                            objOBInquiryExcel.Rate = objBillingInquiry.ListBillingInoutCartonRpt[i].so_itm_price;
                                            objOBInquiryExcel.TotalAmount = (objOBInquiryExcel.Ctns) * (objOBInquiryExcel.Rate);


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

                                if (p_str_type == "PDF")
                                {
                                    strReportName = "rpt_inout_bill_doc_bycube.rpt";
                                }
                                else
                                {
                                    strReportName = "rpt_inout_bill_doc_bycube_Excel.rpt";
                                }

                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                objBillingInquiry.cmp_id = p_str_cmpid;
                                objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
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
                                        if (strReportName == "rpt_inout_bill_doc_bycube_Excel.rpt")
                                        {
                                        }
                                        else
                                        {
                                            rd.SetParameterValue("fml_rpt_title", "BILLING DOCUMENT");
                                            rd.SetParameterValue("fml_rpt_bill_title", "(INBOUND BILL BY CUBE)");
                                            rd.SetParameterValue("fml_rpt_param_bill_num", "Bill#");
                                            rd.SetParameterValue("fml_rpt_param_bill_date", "Bill Dt");
                                        }
                                        if (p_str_type == "PDF")
                                        {
                                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                        }
                                        else if (p_str_type == "XLS")
                                        {
                                            rd.ExportToHttpResponse(ExportFormatType.Excel, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                        }
                                    }
                                }


                            }
                            //CR-3PL_MVC_BL_2018-03-10-001
                            if (l_str_rpt_bill_inout_type.Trim() == "Container")

                            {
                                if (p_str_type == "PDF")
                                {
                                    strReportName = "rpt_inout_bill_by_Container.rpt";
                                }
                                else
                                {
                                    strReportName = "rpt_inout_bill_by_Container_Excel.rpt";
                                }

                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                objBillingInquiry.cmp_id = p_str_cmpid.Trim();
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
                                        if (p_str_type == "PDF")
                                        {
                                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                        }
                                        else if (p_str_type == "XLS")
                                        {
                                            rd.ExportToHttpResponse(ExportFormatType.Excel, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                        }
                                    }
                                }

                            }

                        }

                    }
                    if (p_str_status == "POST")
                    {
                        objBillingInquiry.cmp_id = p_str_cmpid;
                        objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                        objBillingInquiry = ServiceObject.GetBillingBillDocIdType(objBillingInquiry);
                        l_str_rpt_bill_doc_type = objBillingInquiry.ListBillingDocIdType[0].bill_type;
                        objBillingInquiry = ServiceObject.GetBillingBillingType(objBillingInquiry);
                        l_str_rpt_bill_type = objBillingInquiry.ListBillingType[0].bill_type;
                        objBillingInquiry = ServiceObject.GetBillingInoutType(objBillingInquiry);
                        l_str_rpt_bill_inout_type = objBillingInquiry.ListBillingInoutType[0].bill_inout_type;
                        if (l_str_rpt_bill_doc_type == "INOUT")
                        {
                            if (l_str_rpt_bill_inout_type.Trim() == "Carton")
                            {
                                if (p_str_type == "PDF")
                                {
                                    strReportName = "rpt_inout_bill_doc.rpt";

                                }
                                else
                                {
                                    strReportName = "rpt_bl_inv_Excel.rpt";
                                }

                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001                            
                                objBillingInquiry = ServiceObject.GetBillingBillInoutCartonRpt(objBillingInquiry);
                                var rptSource = objBillingInquiry.ListBillingInoutCartonRpt.ToList();


                                if ((p_str_type == "PDF") || (p_str_type == "Word") || (p_str_type == "XLS"))
                                {
                                    if (rptSource.Count > 0)
                                    {
                                        using (ReportDocument rd = new ReportDocument())
                                        {
                                            rd.Load(strRptPath);
                                            rd.SetDataSource(rptSource);
                                            rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                                            if (strReportName == "rpt_inout_bill_doc_Excel.rpt")
                                            {

                                            }
                                            else
                                            {

                                                rd.SetParameterValue("fml_rpt_title", "BILL INVOICE");
                                                rd.SetParameterValue("fml_rpt_bill_title", "(INOUT BILL BY CARTON)");
                                                rd.SetParameterValue("fml_rpt_param_bill_num", "Invoice#");
                                                rd.SetParameterValue("fml_rpt_param_bill_date", "Invoice Dt");
                                            }

                                            if (p_str_type == "PDF")
                                            {

                                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Invoice");
                                            }
                                            else if (p_str_type == "Word")
                                            {
                                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Invoice");
                                            }
                                            else if (p_str_type == "XLS")
                                            {
                                                rd.ExportToHttpResponse(ExportFormatType.Excel, System.Web.HttpContext.Current.Response, false, "Billing Invoice");
                                            }
                                        }
                                    }
                                }
                                else

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
                            else if (l_str_rpt_bill_inout_type.Trim() == "Cube")
                            {
                                if ((p_str_type == "PDF") || (p_str_type == "Word") || (p_str_type == "XLS"))
                                {
                                    if (p_str_type == "PDF")
                                    {
                                        strReportName = "rpt_inout_bill_doc_bycube.rpt";
                                    }
                                    else
                                    {
                                        strReportName = "rpt_inout_bill_doc_bycube_Excel.rpt";
                                    }

                                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                    objBillingInquiry = ServiceObject.GetBillingBillInoutCubeRpt(objBillingInquiry);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
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
                                            rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                            if (strReportName == "rpt_inout_bill_doc_bycube_Excel.rpt")
                                            {
                                            }
                                            else
                                            {
                                                rd.SetParameterValue("fml_rpt_title", "BILL INVOICE");
                                                rd.SetParameterValue("fml_rpt_bill_title", "(INBOUND BILL BY CUBE)");
                                                rd.SetParameterValue("fml_rpt_param_bill_num", "Invoice#");
                                                rd.SetParameterValue("fml_rpt_param_bill_date", "Invoice Dt");
                                            }

                                            if (p_str_type == "PDF")
                                            {
                                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                            }
                                            else if (p_str_type == "XLS")
                                            {
                                                rd.ExportToHttpResponse(ExportFormatType.Excel, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                            }
                                        }
                                    }
                                }

                                else
                                {
                                    objBillingInquiry.cmp_id = p_str_cmpid;
                                    objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                                    objBillingInquiry = ServiceObject.GetBillingInvoiceRpt(objBillingInquiry);

                                    List<BILLING_INOUT_BILLDOC_CTN_EXCEL> li = new List<BILLING_INOUT_BILLDOC_CTN_EXCEL>();
                                    for (int i = 0; i < objBillingInquiry.ListBillingInvoiceRpt.Count; i++)
                                    {

                                        BILLING_INOUT_BILLDOC_CTN_EXCEL objOBInquiryExcel = new BILLING_INOUT_BILLDOC_CTN_EXCEL();
                                        objOBInquiryExcel.Line = objBillingInquiry.ListBillingInvoiceRpt[i].dtl_line;
                                        objOBInquiryExcel.ContId = objBillingInquiry.ListBillingInvoiceRpt[i].cont_id;
                                        objOBInquiryExcel.LotId = objBillingInquiry.ListBillingInvoiceRpt[i].lot_id;
                                        objOBInquiryExcel.ServiceId = objBillingInquiry.ListBillingInvoiceRpt[i].notes;
                                        objOBInquiryExcel.Style = objBillingInquiry.ListBillingInvoiceRpt[i].so_itm_num;
                                        objOBInquiryExcel.Color = objBillingInquiry.ListBillingInvoiceRpt[i].so_itm_color;
                                        objOBInquiryExcel.Size = objBillingInquiry.ListBillingInvoiceRpt[i].so_itm_size;
                                        objOBInquiryExcel.Ctns = objBillingInquiry.ListBillingInvoiceRpt[i].ship_ctns;
                                        objOBInquiryExcel.Rate = objBillingInquiry.ListBillingInvoiceRpt[i].so_itm_price;
                                        objOBInquiryExcel.TotalAmount = (objOBInquiryExcel.Ctns) * (objOBInquiryExcel.Rate);


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
                        else if (l_str_rpt_bill_doc_type == "STRG")
                        {
                            if (p_str_type == "PDF")
                            {
                                strReportName = "rpt_bl_inv.rpt";
                            }
                            else
                            {
                                strReportName = "rpt_bl_inv_Excel.rpt";
                            }


                            BillingInquiry objBillingInquiry = new BillingInquiry();
                            BillingInquiryService ServiceObject = new BillingInquiryService();

                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmpid;
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                            objBillingInquiry = ServiceObject.GetInvoiceRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListBillingInvoiceRpt.ToList();


                            //END
                            if ((p_str_type == "PDF") || (p_str_type == "XLS"))
                            {
                                if (rptSource.Count > 0)
                                {
                                    using (ReportDocument rd = new ReportDocument())
                                    {
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                        AlocCount = objBillingInquiry.ListBillingInvoiceRpt.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);
                                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                        //CR20181121-001 Added By Nithya
                                        rd.SetParameterValue("Rpt_title", "STORAGE");
                                        if (p_str_type == "PDF")
                                        {
                                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                        }
                                        else if (p_str_type == "XLS")
                                        {
                                            rd.ExportToHttpResponse(ExportFormatType.Excel, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                        }
                                    }
                                }

                            }
                            else
                            {
                                objBillingInquiry.cmp_id = p_str_cmpid;
                                objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                                objBillingInquiry = ServiceObject.GetBillingInvoiceRpt(objBillingInquiry);

                                List<BILLING_INOUT_BILLDOC_CTN_EXCEL> li = new List<BILLING_INOUT_BILLDOC_CTN_EXCEL>();
                                for (int i = 0; i < objBillingInquiry.ListBillingInvoiceRpt.Count; i++)
                                {

                                    BILLING_INOUT_BILLDOC_CTN_EXCEL objOBInquiryExcel = new BILLING_INOUT_BILLDOC_CTN_EXCEL();
                                    objOBInquiryExcel.Line = objBillingInquiry.ListBillingInvoiceRpt[i].dtl_line;
                                    objOBInquiryExcel.ContId = objBillingInquiry.ListBillingInvoiceRpt[i].cont_id;
                                    objOBInquiryExcel.LotId = objBillingInquiry.ListBillingInvoiceRpt[i].lot_id;
                                    objOBInquiryExcel.ServiceId = objBillingInquiry.ListBillingInvoiceRpt[i].notes;
                                    objOBInquiryExcel.Style = objBillingInquiry.ListBillingInvoiceRpt[i].so_itm_num;
                                    objOBInquiryExcel.Color = objBillingInquiry.ListBillingInvoiceRpt[i].so_itm_color;
                                    objOBInquiryExcel.Size = objBillingInquiry.ListBillingInvoiceRpt[i].so_itm_size;
                                    objOBInquiryExcel.Ctns = objBillingInquiry.ListBillingInvoiceRpt[i].ship_ctns;
                                    objOBInquiryExcel.Rate = objBillingInquiry.ListBillingInvoiceRpt[i].so_itm_price;
                                    objOBInquiryExcel.TotalAmount = (objOBInquiryExcel.Ctns) * (objOBInquiryExcel.Rate);


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
                            if (p_str_type == "PDF")
                            {
                                // strReportName = "rpt_va_bill_inv.rpt";
                                strReportName = "rpt_va_bill_doc.rpt";
                            }
                            else
                            {
                                strReportName = "rpt_va_bill_inv_Excel.rpt";
                            }
                            BillingInquiry objBillingInquiry = new BillingInquiry();
                            BillingInquiryService ServiceObject = new BillingInquiryService();

                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmpid;
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                            objBillingInquiry = ServiceObject.GetBillingBillDocIdType(objBillingInquiry);
                            l_str_rpt_bill_doc_type = objBillingInquiry.ListBillingDocIdType[0].bill_type;
                            //objBillingInquiry = ServiceObject.GetBillingInvoiceRpt(objBillingInquiry);
                            //var rptSource = objBillingInquiry.ListBillingInvoiceRpt.ToList();
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
                            objBillingInquiry = ServiceObject.GetBillingBillDocVASRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListBillingDocVASRpt.ToList();

                            //END
                            if ((p_str_type == "PDF") || (p_str_type == "XLS"))
                            {
                                if (rptSource.Count > 0)
                                {
                                    using (ReportDocument rd = new ReportDocument())
                                    {
                                        rd.Load(strRptPath);

                                        rd.SetDataSource(rptSource);
                                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                        if (strReportName == "rpt_va_bill_inv_Excel.rpt")
                                        { }
                                        else
                                        {
                                            rd.SetParameterValue("fml_rpt_title", "BILL INVOICE");
                                            rd.SetParameterValue("fml_rpt_bill_title", "VALUE ADDED SERVICE");
                                            rd.SetParameterValue("fml_rpt_param_bill_num", "Invoice#");
                                            rd.SetParameterValue("fml_rpt_param_bill_date", "Invoice Dt");
                                        }
                                        if (p_str_type == "PDF")
                                        {
                                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                        }
                                        else if (p_str_type == "XLS")
                                        {
                                            rd.ExportToHttpResponse(ExportFormatType.Excel, System.Web.HttpContext.Current.Response, false, p_str_bill_doc_id);
                                        }
                                    }
                                }
                            }

                            else
                            {
                                objBillingInquiry.cmp_id = p_str_cmpid;
                                objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                                objBillingInquiry = ServiceObject.GetBillingInvoiceRpt(objBillingInquiry);

                                List<BILLING_NORM_BILLDOC_EXCEL> li = new List<BILLING_NORM_BILLDOC_EXCEL>();
                                for (int i = 0; i < objBillingInquiry.ListBillingInvoiceRpt.Count; i++)
                                {

                                    BILLING_NORM_BILLDOC_EXCEL objOBInquiryExcel = new BILLING_NORM_BILLDOC_EXCEL();
                                    objOBInquiryExcel.Line = objBillingInquiry.ListBillingInvoiceRpt[i].dtl_line;
                                    objOBInquiryExcel.VASId = objBillingInquiry.ListBillingInvoiceRpt[i].ship_doc_id;
                                    objOBInquiryExcel.ShipDate = objBillingInquiry.ListBillingInvoiceRpt[i].ship_dt.ToString("MM/dd/yyyy");
                                    objOBInquiryExcel.CustOrder = objBillingInquiry.ListBillingInvoiceRpt[i].CustOrder;
                                    objOBInquiryExcel.Notes = objBillingInquiry.ListBillingInvoiceRpt[i].Notes;
                                    objOBInquiryExcel.VASDesc = objBillingInquiry.ListBillingInvoiceRpt[i].ship_itm_num;
                                    objOBInquiryExcel.Ctns = objBillingInquiry.ListBillingInvoiceRpt[i].ship_ctns;
                                    objOBInquiryExcel.Rate = objBillingInquiry.ListBillingInvoiceRpt[i].so_itm_price;
                                    objOBInquiryExcel.Amount = (objOBInquiryExcel.Ctns) * (objOBInquiryExcel.Rate);
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
        public ActionResult ShowConsolidateReport(string p_str_radio, string p_str_cmp_id, string p_str_Bill_doc_id, string p_str_Bill_type, string p_str_doc_dt_Fr, string p_str_doc_dt_To, string SelectdID, string type)

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
            string p_str_bill_doc_id = SelectdID;
            decimal l_dec_tot_amnt = 0;
            decimal l_dec_list_price = 0;
            int l_int_tot_qty = 0;
            decimal l_int_tot_ship_qty = 0;

            try
            {
                if (isValid)
                {



                    BillingInquiry objBillingInquiry = new BillingInquiry();
                    BillingInquiryService ServiceObject = new BillingInquiryService();
                    if (SelectdID == "" || SelectdID == "undefined")
                    {
                        l_str_rpt_bill_doc_type = p_str_Bill_type;
                    }
                    else
                    {
                        objBillingInquiry.cmp_id = p_str_cmp_id;
                        objBillingInquiry.Bill_doc_id = SelectdID;
                        objBillingInquiry = ServiceObject.GetBillingInoutType(objBillingInquiry);
                        l_str_rpt_bill_inout_type = objBillingInquiry.ListBillingInoutType[0].bill_inout_type;
                        objBillingInquiry.bill_type = "BY " + "-" + l_str_rpt_bill_inout_type;
                        objBillingInquiry = ServiceObject.GetBillingBillDocIdType(objBillingInquiry);
                        l_str_rpt_bill_doc_type = objBillingInquiry.ListBillingDocIdType[0].bill_type;
                    }

                    if (l_str_rpt_bill_doc_type == "INOUT")
                    {
                        if (type == "PDF")
                        {
                            if (l_str_rpt_bill_inout_type.ToUpper() == "CUBE")
                            {
                                strReportName = "rpt_bl_inout_bill_by_cube_consolidated.rpt";
                            }
                            else
                            {
                                strReportName = "rpt_inout_bill_doc_consolidated.rpt";
                            }

                        }
                        else
                        {
                            strReportName = "rpt_inout_bill_doc_consolidated_Excel.rpt";
                        }

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        objBillingInquiry.cmp_id = p_str_cmp_id;
                        if (SelectdID == "" || SelectdID == "undefined")
                        {
                            objBillingInquiry.Bill_doc_id = "";
                        }
                        else
                        {
                            objBillingInquiry.Bill_doc_id = SelectdID;
                        }
                        objBillingInquiry.Bill_type = p_str_Bill_type;
                        objBillingInquiry.Bill_doc_dt_Fr = p_str_doc_dt_Fr;
                        objBillingInquiry.Bill_doc_dt_To = p_str_doc_dt_To;
                        objBillingInquiry = ServiceObject.GetBillingBillConsolidateRpt(objBillingInquiry);
                        var rptSource = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.ToList();

                        //rd.SetParameterValue("fml_rep_type", "INOUT");
                        if (type == "PDF")
                        {
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetDataSource(rptSource);
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                    rd.SetParameterValue("fml_rep_type", objBillingInquiry.bill_type);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Consolidated Inout Bill");
                                }
                            }
                        }

                        else if (type == "XLS")
                        {
                            string tempFileName = string.Empty;
                            string l_str_file_name = string.Empty;
                            string strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
                            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                            if (!Directory.Exists(strOutputpath))
                            {
                                Directory.CreateDirectory(strOutputpath);
                            }
                            BillingInquiryService objService = new BillingInquiryService();
                            DataTable dt_con_vas = new DataTable();
                            dt_con_vas = objService.GetConsolidtedINOUTBillList(p_str_cmp_id, p_str_bill_doc_id, p_str_Bill_type, p_str_doc_dt_Fr, p_str_doc_dt_To);

                            l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-CONSOLIDATED-BILL-INBOUND-" + p_str_bill_doc_id + strDateFormat + ".xlsx";

                            tempFileName = strOutputpath + l_str_file_name;

                            if (System.IO.File.Exists(tempFileName))
                                System.IO.File.Delete(tempFileName);
                            xls_Consalidate_Rpt_By_INOUT mxcel2 = new xls_Consalidate_Rpt_By_INOUT(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "BL_CONSOLIDATED_REPORT_By_INOUT.xlsx");
                            mxcel2.PopulateHeader(p_str_cmp_id, string.Empty);

                            mxcel2.PopulateData(dt_con_vas, true);
                            mxcel2.SaveAs(tempFileName);
                            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                            return File(fs, "application / xlsx", l_str_file_name);
                        }
                        else
                        {
                            Response.Write("Please Select the Mode!");
                        }
                    }
                    else
                    {
                        Company objCompany = new Company();
                        CompanyService ServiceObjectCompany = new CompanyService();
                        objCompany.cust_of_cmp_id = "";
                        objCompany.cmp_id = p_str_cmp_id;
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
                        if (type == "PDF")
                        {
                            strReportName = "rpt_va_bill_doc_consolidate.rpt";
                        }
                        else
                        {
                            strReportName = "rpt_va_bill_doc_consolidate_Excel.rpt";
                        }

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        objBillingInquiry.cmp_id = p_str_cmp_id;
                        if (SelectdID == "" || SelectdID == "undefined")
                        {
                            objBillingInquiry.Bill_doc_id = "";
                        }
                        else
                        {
                            objBillingInquiry.Bill_doc_id = SelectdID;
                        }
                        objBillingInquiry.Bill_type = p_str_Bill_type;
                        objBillingInquiry.Bill_doc_dt_Fr = p_str_doc_dt_Fr;
                        objBillingInquiry.Bill_doc_dt_To = p_str_doc_dt_To;
                        // objBillingInquiry = ServiceObject.GetBillingBillDocVASRpt(objBillingInquiry);


                        if (type == "PDF")
                        {
                            objBillingInquiry = ServiceObject.GetConsolidtedVASBillRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListBillingDocVASRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {

                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                                    AlocCount = objBillingInquiry.ListBillingDocVASRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Consolidated Vas Bill");
                                }
                            }
                        }

                        else if (type == "XLS")
                        {
                            string tempFileName = string.Empty;
                            string l_str_file_name = string.Empty;
                            string strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
                            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                            if (!Directory.Exists(strOutputpath))
                            {
                                Directory.CreateDirectory(strOutputpath);
                            }
                            BillingInquiryService objService = new BillingInquiryService();

                            DataTable dt_con_vas = new DataTable();
                            dt_con_vas = objService.GetConsolidtedVASBillList(p_str_cmp_id, p_str_bill_doc_id, p_str_Bill_type, p_str_doc_dt_Fr, p_str_doc_dt_To);
                            l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-CONSOLIDATED-BILL-VAS-" + p_str_bill_doc_id + strDateFormat + ".xlsx";

                            tempFileName = strOutputpath + l_str_file_name;

                            if (System.IO.File.Exists(tempFileName))
                                System.IO.File.Delete(tempFileName);
                            xls_Consalidate_Rpt_By_VAS mxcel2 = new xls_Consalidate_Rpt_By_VAS(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "BL_CONSOLIDATED_REPORT_By_VAS.xlsx");
                            mxcel2.PopulateHeader(p_str_cmp_id, string.Empty);

                            mxcel2.PopulateData(dt_con_vas, true);
                            mxcel2.SaveAs(tempFileName);
                            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                            return File(fs, "application / xlsx", l_str_file_name);
                        }
                        else
                        {
                            Response.Write("<H2>!!!</H2>");
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

        public ActionResult GetBillDocId(string p_str_cmp_id, string p_str_cust_id, string p_str_ib_doc_id)
        {
            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();
            String l_str_status = String.Empty;
            //objBillingInquiry.cmp_id = p_str_cmp_id;
            objBillingInquiry.ib_doc_id = p_str_ib_doc_id;
            objBillingInquiry.cust_id = p_str_cmp_id;


            objBillingInquiry = ServiceObject.CheckExsistBLDocIDFromLotHdr(objBillingInquiry);
            if (objBillingInquiry.Check_existing_bill_doc_id == "" || objBillingInquiry.Check_existing_bill_doc_id == null)
            {
                objBillingInquiry.bill_doc_id = "";
            }
            else
            {
                objBillingInquiry.bill_doc_id = objBillingInquiry.ListCheckExistingInOutBillDocId[0].bill_doc_id.ToString();
            }
            return Json(objBillingInquiry.bill_doc_id, JsonRequestBehavior.AllowGet);
        }

        //CR-3PL_MVC_IB_2018_0428_001 Added By Nithya
        public JsonResult IB_INQ_HDR_DATA(string p_str_cmp_id, string p_str_Bill_doc_id, string p_str_Bill_type, string p_str_doc_dt_Fr, string p_str_doc_dt_To)
        {
            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();
            Session["g_str_cmp_id"] = p_str_cmp_id.Trim();
            Session["TEMP_CMP_ID"] = p_str_cmp_id.Trim();
            Session["TEMP_BILLDOC_ID"] = p_str_Bill_doc_id.Trim();
            Session["TEMP_BILL_TYPE"] = p_str_Bill_type.Trim();
            Session["TEMP_DOC_DT_FM"] = p_str_doc_dt_Fr.Trim();
            Session["TEMP_DOC_DT_TO"] = p_str_doc_dt_To.Trim();
            return Json(objBillingInquiry.ListBillingdetail, JsonRequestBehavior.AllowGet);

        }
        public ActionResult SaveVASConsolidateBillRegenerate(string p_str_cmp_id, string p_str_ship_doc_id, string p_str_bill_type, string p_str_bill_doc_id, string p_str_bill_as_of_dt,
            string p_str_doc_dt_Fr, string p_str_doc_dt_To, string p_str_bill_cmp_id, string p_str_print_dt)
        {

            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();

            if (p_str_bill_doc_id != null)
            {

                ServiceObject.DeleteVASBill(p_str_bill_cmp_id, p_str_cmp_id, p_str_bill_doc_id);
                string[] str_arry_ship_doc_id = p_str_ship_doc_id.Split(',');
                objBillingInquiry = ServiceObject.GetConsolidateVASBillEntityValue(objBillingInquiry);
                objBillingInquiry.temp_bill_doc_id = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[0].temp_bill_doc_id;
                objBillingInquiry.cust_id = p_str_cmp_id;
                objBillingInquiry.bill_as_of_dt = p_str_bill_as_of_dt;
                objBillingInquiry.print_bill_date = p_str_print_dt;
                objBillingInquiry.vas_bill_pd_fm = p_str_doc_dt_Fr;
                objBillingInquiry.vas_bill_pd_to = p_str_doc_dt_To;
                objBillingInquiry.bill_doc_id = string.Empty;
                objBillingInquiry.cmp_id = p_str_bill_cmp_id;
                objBillingInquiry.bill_type = "NORM";

                foreach (string str_tim_ship_doc_id in str_arry_ship_doc_id)
                {
                    if (str_tim_ship_doc_id == "on") // To exclude the Header checkbox
                    { }
                    else
                    {
                        objBillingInquiry.temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id;
                        objBillingInquiry.ship_doc_id = str_tim_ship_doc_id;
                        objBillingInquiry = ServiceObject.SaveConsolidateBillDetails(objBillingInquiry);
                    }
                }
                objBillingInquiry.bill_doc_id = p_str_bill_doc_id;
            }


            else if (p_str_ship_doc_id == null || p_str_ship_doc_id == "undefined")
            {
                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry.bill_as_of_dt = p_str_bill_as_of_dt;
                objBillingInquiry.bill_pd_fm = p_str_doc_dt_Fr;
                objBillingInquiry.bill_pd_to = p_str_doc_dt_To;
                objBillingInquiry = ServiceObject.GetConsolidateVASInqDetails(objBillingInquiry);
                if (objBillingInquiry.ListBillRcvdDetails.Count == 0)
                {
                    int ResultCount = 1;
                    return Json(ResultCount, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    objBillingInquiry = ServiceObject.GetConsolidateVASBillEntityValue(objBillingInquiry);
                    objBillingInquiry.temp_bill_doc_id = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[0].temp_bill_doc_id;
                    objBillingInquiry.cust_id = p_str_cmp_id;
                    objBillingInquiry.print_bill_date = p_str_print_dt;
                    objBillingInquiry.vas_bill_pd_fm = p_str_doc_dt_Fr;
                    objBillingInquiry.vas_bill_pd_to = p_str_doc_dt_To;
                    objBillingInquiry.bill_doc_id = string.Empty;
                    objBillingInquiry.cmp_id = p_str_bill_cmp_id;
                    objBillingInquiry.bill_type = "NORM";
                    for (int i = 0; i < objBillingInquiry.ListBillRcvdDetails.Count; i++)
                    {
                        objBillingInquiry.temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id;
                        objBillingInquiry.ship_doc_id = objBillingInquiry.ListBillRcvdDetails[i].ship_doc_id;
                        objBillingInquiry = ServiceObject.SaveConsolidateBillDetails(objBillingInquiry);
                    }

                }
            }


            objBillingInquiry = ServiceObject.GenerateVASBill(objBillingInquiry);
            objBillingInquiry.bill_doc_id = objBillingInquiry.ListSaveVASBillDetails[0].bill_doc_id.ToString().Trim();
            Session["sess_bill_doc_id"] = String.Empty;
            return Json(objBillingInquiry.bill_doc_id, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveVASConsolidateSelectedBill(string p_str_cust_id, List<clsVASId> plstShipDocId, string p_str_Bill_type, string p_str_Bill_doc_id, string p_str_bill_as_of_dt, string p_str_doc_dt_Fr, string p_str_doc_dt_To, string p_str_cmp_id, string p_str_print_dt)
        {

            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();

            if (Session["sess_bill_doc_id"] != null)
            {
                objBillingInquiry.bill_doc_id = Session["sess_bill_doc_id"].ToString();
                objBillingInquiry.Bill_doc_id = Session["sess_bill_doc_id"].ToString();
                objBillingInquiry.CUSTId = p_str_cmp_id;
                objBillingInquiry.cmp_id = p_str_cust_id;

                objBillingInquiry = ServiceObject.GetVASBillDelete(objBillingInquiry);
            }

            objBillingInquiry = ServiceObject.GetConsolidateVASBillEntityValue(objBillingInquiry);
            objBillingInquiry.temp_bill_doc_id = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[0].temp_bill_doc_id;
            objBillingInquiry.cust_id = p_str_cmp_id;
            objBillingInquiry.bill_as_of_dt = p_str_bill_as_of_dt;
            objBillingInquiry.print_bill_date = p_str_print_dt;
            objBillingInquiry.vas_bill_pd_fm = p_str_doc_dt_Fr;
            objBillingInquiry.vas_bill_pd_to = p_str_doc_dt_To;
            objBillingInquiry.bill_doc_id = string.Empty;
            objBillingInquiry.cmp_id = p_str_cust_id;
            objBillingInquiry.bill_type = p_str_Bill_type.Trim();

            int l_int_count = 0;
            l_int_count = plstShipDocId.Count;
            string l_str_ship_doc_id = string.Empty;

            for (int i = 0; i < l_int_count; i++)
            {
                l_str_ship_doc_id = plstShipDocId[i].ship_doc_id.ToString();
                if (l_str_ship_doc_id == "on")
                {
                }
                else
                {
                    objBillingInquiry.temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id;
                    objBillingInquiry.ship_doc_id = l_str_ship_doc_id;
                    objBillingInquiry = ServiceObject.SaveConsolidateBillDetails(objBillingInquiry);


                }
            }


            objBillingInquiry.ship_doc_id = string.Empty;

            if (Session["sess_bill_doc_id"] != null)
            {

                objBillingInquiry.bill_doc_id = Session["sess_bill_doc_id"].ToString();
            }
            objBillingInquiry = ServiceObject.GenerateVASBill(objBillingInquiry);
            objBillingInquiry.bill_doc_id = objBillingInquiry.ListSaveVASBillDetails[0].bill_doc_id.ToString().Trim();
            Session["sess_bill_doc_id"] = String.Empty;
            return Json(objBillingInquiry.bill_doc_id, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveVASConsolidateBill(string p_str_cust_id, string p_str_ship_doc_id, string p_str_Bill_type, string p_str_Bill_doc_id, string p_str_bill_as_of_dt, string p_str_doc_dt_Fr, string p_str_doc_dt_To, string p_str_cmp_id, string p_str_print_dt)
        {

            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();

            if (Session["sess_bill_doc_id"] != null)
            {
                objBillingInquiry.bill_doc_id = Session["sess_bill_doc_id"].ToString();
                objBillingInquiry.Bill_doc_id = Session["sess_bill_doc_id"].ToString();
                objBillingInquiry.CUSTId = p_str_cmp_id;
                objBillingInquiry.cmp_id = p_str_cust_id;

                objBillingInquiry = ServiceObject.GetVASBillDelete(objBillingInquiry);
            }


            if (p_str_ship_doc_id == null || p_str_ship_doc_id == "undefined")
            {
                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry.bill_as_of_dt = p_str_bill_as_of_dt;
                objBillingInquiry.bill_pd_fm = p_str_doc_dt_Fr;
                objBillingInquiry.bill_pd_to = p_str_doc_dt_To;
                objBillingInquiry = ServiceObject.GetConsolidateVASInqDetails(objBillingInquiry);
                if (objBillingInquiry.ListBillRcvdDetails.Count == 0)
                {
                    int ResultCount = 1;
                    return Json(ResultCount, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    objBillingInquiry = ServiceObject.GetConsolidateVASBillEntityValue(objBillingInquiry);
                    objBillingInquiry.temp_bill_doc_id = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[0].temp_bill_doc_id;
                    objBillingInquiry.cust_id = p_str_cmp_id;
                    objBillingInquiry.print_bill_date = p_str_print_dt;
                    objBillingInquiry.vas_bill_pd_fm = p_str_doc_dt_Fr;
                    objBillingInquiry.vas_bill_pd_to = p_str_doc_dt_To;
                    objBillingInquiry.bill_doc_id = string.Empty;
                    objBillingInquiry.cmp_id = p_str_cust_id;
                    objBillingInquiry.bill_type = p_str_Bill_type.Trim();
                    for (int i = 0; i < objBillingInquiry.ListBillRcvdDetails.Count; i++)
                    {
                        objBillingInquiry.temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id;
                        objBillingInquiry.ship_doc_id = objBillingInquiry.ListBillRcvdDetails[i].ship_doc_id;
                        objBillingInquiry = ServiceObject.SaveConsolidateBillDetails(objBillingInquiry);
                    }

                }
            }
            else
            {
                string[] str_arry_ship_doc_id = p_str_ship_doc_id.Split(',');
                objBillingInquiry = ServiceObject.GetConsolidateVASBillEntityValue(objBillingInquiry);
                objBillingInquiry.temp_bill_doc_id = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[0].temp_bill_doc_id;
                objBillingInquiry.cust_id = p_str_cmp_id;
                objBillingInquiry.bill_as_of_dt = p_str_bill_as_of_dt;
                objBillingInquiry.print_bill_date = p_str_print_dt;
                objBillingInquiry.vas_bill_pd_fm = p_str_doc_dt_Fr;
                objBillingInquiry.vas_bill_pd_to = p_str_doc_dt_To;
                objBillingInquiry.bill_doc_id = string.Empty;
                objBillingInquiry.cmp_id = p_str_cust_id;
                objBillingInquiry.bill_type = p_str_Bill_type.Trim();

                foreach (string str_tim_ship_doc_id in str_arry_ship_doc_id)
                {
                    if (str_tim_ship_doc_id == "on") // To exclude the Header checkbox
                    { }
                    else
                    {
                        objBillingInquiry.temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id;
                        objBillingInquiry.ship_doc_id = str_tim_ship_doc_id;
                        objBillingInquiry = ServiceObject.SaveConsolidateBillDetails(objBillingInquiry);
                    }
                }
            }
            objBillingInquiry.ship_doc_id = string.Empty;

            if (Session["sess_bill_doc_id"] != null)
            {

                objBillingInquiry.bill_doc_id = Session["sess_bill_doc_id"].ToString();
            }
            objBillingInquiry = ServiceObject.GenerateVASBill(objBillingInquiry);
            objBillingInquiry.bill_doc_id = objBillingInquiry.ListSaveVASBillDetails[0].bill_doc_id.ToString().Trim();
            Session["sess_bill_doc_id"] = String.Empty;
            return Json(objBillingInquiry.bill_doc_id, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSearchConsolidateStorageInqDetails(string p_str_cmp_id, string p_dt_bill_from_dt, string p_dt_bill_to_dt, string p_str_cust_id, string p_str_bill_as_of_dt)
        {
            try
            {

                string l_str_bill_amount = string.Empty;
                BillingInquiry objBillingInquiry = new BillingInquiry();
                BillingInquiryService ServiceObject = new BillingInquiryService();
                objBillingInquiry.cmp_id = p_str_cust_id;
                objBillingInquiry.cust_id = p_str_cmp_id;
                objBillingInquiry.bill_as_of_date = p_str_bill_as_of_dt;
                objBillingInquiry.bill_pd_to = p_dt_bill_to_dt;
                objBillingInquiry = ServiceObject.GetConsolidateStorageBillDetails(objBillingInquiry);

                Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
                BillingInquiryModel BillingInquiryModel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);

                return PartialView("_GrdGenerateConsolidateStorageBill", BillingInquiryModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult SaveStorageConsolidateBill(string p_str_cust_id, string p_str_Bill_doc_id, string p_str_doc_dt_Fr, string p_str_doc_dt_To, string p_str_cmp_id, string p_str_print_dt)
        {
            string l_str_bill_doc_status = string.Empty;
            string l_str_bill_doc_id = string.Empty;
            string l_str_rpt_bill_type = string.Empty;
            objBillingInquiry.cust_id = p_str_cmp_id;
            objBillingInquiry.print_bill_date = p_str_print_dt;
            objBillingInquiry.bill_as_of_date = p_str_doc_dt_To;
            objBillingInquiry.bill_doc_id = string.Empty;
            objBillingInquiry.cust_id = p_str_cust_id;
            objBillingInquiry.cmp_id = p_str_cmp_id;

            objBillingInquiry = ServiceObject.CheckSTRGBillDocIdExisting(objBillingInquiry);
            if (objBillingInquiry.Check_existing_bill_doc_id == "1")
            {
                objBillingInquiry.Bill_doc_id = objBillingInquiry.bill_doc_id;
                objBillingInquiry.CUSTId = p_str_cust_id;
                objBillingInquiry = ServiceObject.GetBillDelete(objBillingInquiry);
            }
            objBillingInquiry.cmp_id = p_str_cust_id;
            objBillingInquiry = ServiceObject.GetBillingBillingType(objBillingInquiry);
            l_str_rpt_bill_type = objBillingInquiry.ListBillingType[0].bill_type;
            objBillingInquiry.cmp_id = p_str_cust_id;
            if (l_str_rpt_bill_type == "Carton")
            {
                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry.cust_id = p_str_cust_id;
                // objBillingInquiry = ServiceObject.GenerateSTRGBill(objBillingInquiry);//CR-3PL_MVC_BL_2018_00312_001                
                objBillingInquiry = ServiceObject.GenerateSTRGBillCarton(objBillingInquiry);
                objBillingInquiry.bill_doc_id = objBillingInquiry.ListSaveSTRGBillDetails[0].bill_doc_id.ToString().Trim();
            }
            else if (l_str_rpt_bill_type == "Cube")
            {
                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry.cust_id = p_str_cust_id;
                //objBillingInquiry = ServiceObject.GenerateSTRGBillByCube(objBillingInquiry);//CR-3PL_MVC_BL_2018_00312_001
                objBillingInquiry = ServiceObject.GenerateSTRGBillCube(objBillingInquiry);
                objBillingInquiry.bill_doc_id = objBillingInquiry.ListSaveSTRGBillDetails[0].bill_doc_id.ToString().Trim();
            }
            else if (l_str_rpt_bill_type == "Pallet")
            {
                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry.cust_id = p_str_cust_id;
                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                objBillingInquiry = ServiceObject.GenerateSTRGBillByPallet(objBillingInquiry);//CR-3PL_MVC_BL_2018_0224_001
                objBillingInquiry.bill_doc_id = objBillingInquiry.ListGenBillingStrgByPallet[0].bill_doc_id.ToString().Trim();
            }
            else if (l_str_rpt_bill_type == "Location")

            {
                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry.cust_id = p_str_cust_id;
                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                objBillingInquiry = ServiceObject.GenerateSTRGBillByLoc(objBillingInquiry);//CR-3PL_MVC_BL_2018_0224_001
                objBillingInquiry.bill_doc_id = objBillingInquiry.ListGetSTRGBillByLoc[0].bill_doc_id.ToString().Trim();
            }
            else if (l_str_rpt_bill_type == "Pcs")
            {
                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry.cust_id = p_str_cust_id;
                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                objBillingInquiry = ServiceObject.GenerateSTRGBillByPcs(objBillingInquiry);//CR-2018-05-21-001 Added By Nithya
                objBillingInquiry.bill_doc_id = objBillingInquiry.ListGetSTRGBillByLoc[0].bill_doc_id.ToString().Trim();
            }
            objBillingInquiry.Success = objBillingInquiry.bill_doc_id;
            //END
            return Json(objBillingInquiry.Success, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSearchConsolidateInoutInqDetails(string p_str_cmp_id, string p_dt_bill_from_dt, string p_dt_bill_to_dt, string p_str_cust_id, string p_str_bill_as_of_dt)
        {
            try
            {
                string l_str_bill_amount = string.Empty;
                BillingInquiry objBillingInquiry = new BillingInquiry();
                BillingInquiryService ServiceObject = new BillingInquiryService();
                objBillingInquiry.cmp_id = p_str_cust_id;
                objBillingInquiry.cust_id = p_str_cmp_id;
                objBillingInquiry.bill_pd_fm = p_dt_bill_from_dt;
                objBillingInquiry.bill_pd_to = p_dt_bill_to_dt;
                objBillingInquiry = ServiceObject.GetConsolidateInoutBillDetails(objBillingInquiry);
                Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
                BillingInquiryModel BillingInquiryModel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);
                return PartialView("_GrdGenerateConsolidateInoutBill", BillingInquiryModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult GetSearchConsolidateVASInqDetails(string p_str_cmp_id, string p_dt_bill_from_dt, string p_dt_bill_to_dt)
        {
            try
            {
                string l_str_bill_amount = string.Empty;
                BillingInquiry objBillingInquiry = new BillingInquiry();
                BillingInquiryService ServiceObject = new BillingInquiryService();
                objBillingInquiry.cmp_id = p_str_cmp_id;
                objBillingInquiry.bill_pd_fm = p_dt_bill_from_dt;
                objBillingInquiry.bill_pd_to = p_dt_bill_to_dt;
                objBillingInquiry = ServiceObject.GetConsolidateVASInqDetails(objBillingInquiry);
                Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
                BillingInquiryModel BillingInquiryModel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);
                return PartialView("_GrdGenerateConsolidateVasBill", BillingInquiryModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult CkBillDocAlreadyExists(string p_str_cust_id, string p_str_Bill_type, string p_str_doc_dt_Fr, string p_str_doc_dt_To)
        {
            BillingInquiryService ServiceObject = new BillingInquiryService();
            string l_str_bill_doc_id = string.Empty;
            l_str_bill_doc_id = ServiceObject.CkBillDocAlreadyExists(p_str_cust_id, p_str_Bill_type, p_str_doc_dt_Fr, p_str_doc_dt_To);
            if (l_str_bill_doc_id.Length > 0)
            {
                int ResultCount = 1;
                Session["sess_bill_doc_id"] = l_str_bill_doc_id;
                return Json(ResultCount, JsonRequestBehavior.AllowGet);
            }
            else
            {
                int ResultCount = 0;
                return Json(ResultCount, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult SaveInoutConsolidateBill(string p_str_cust_id, string p_str_lot_id, string p_str_Bill_type, string p_str_Bill_doc_id, string p_str_bill_as_of_dt, string p_str_doc_dt_Fr, string p_str_doc_dt_To, string p_str_cmp_id, string p_str_print_dt)
        {
            string l_str_rpt_bill_type = string.Empty;
            objBillingInquiry.cmp_id = p_str_cmp_id;
            objBillingInquiry.cust_id = p_str_cust_id;
            objBillingInquiry.bill_type = p_str_Bill_type;
            if (p_str_Bill_doc_id.Trim().Length == 0)
            {
                objBillingInquiry = ServiceObject.GetConsolidateVASBillEntityValue(objBillingInquiry);
                objBillingInquiry.temp_bill_doc_id = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[0].temp_bill_doc_id;
            }
            else
            {

                objBillingInquiry.temp_bill_doc_id = p_str_Bill_doc_id;
            }
            //if (p_str_lot_id == null || p_str_lot_id == "undefined")
            //{
            objBillingInquiry.cmp_id = p_str_cmp_id;
            objBillingInquiry.cust_id = p_str_cust_id;
            objBillingInquiry.bill_as_of_date = p_str_bill_as_of_dt;
            objBillingInquiry.bill_pd_fm = p_str_doc_dt_Fr;
            objBillingInquiry.bill_pd_to = p_str_doc_dt_To;


            if (Session["sess_bill_doc_id"] != null)
            {
                objBillingInquiry.bill_doc_id = Session["sess_bill_doc_id"].ToString();
                objBillingInquiry.Bill_doc_id = Session["sess_bill_doc_id"].ToString();
                objBillingInquiry.CUSTId = p_str_cust_id;

                objBillingInquiry = ServiceObject.GetBillDelete(objBillingInquiry);
            }

            objBillingInquiry = ServiceObject.GetConsolidateInoutBillDetails(objBillingInquiry);



            if (objBillingInquiry.ListSaveSTRGBillDetails.Count == 0)
            {
                int ResultCount = 1;
                return Json(ResultCount, JsonRequestBehavior.AllowGet);
            }
            else
            {
                for (int i = 0; i < objBillingInquiry.ListSaveSTRGBillDetails.Count; i++)
                {
                    objBillingInquiry.temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id;
                    objBillingInquiry.lot_id = objBillingInquiry.ListSaveSTRGBillDetails[i].lot_id;
                    objBillingInquiry = ServiceObject.SaveConsolidateInoutBillDetails(objBillingInquiry);
                }
            }
            //}
            //else
            //{
            //    string[] str_arry_lot_id = p_str_lot_id.Split(',');
            //    foreach (string str_tim_lot_id in str_arry_lot_id)
            //    {
            //        if (str_tim_lot_id == "on") // To exclude the Header checkbox
            //        { }
            //        else
            //        {
            //            objBillingInquiry.temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id;
            //            objBillingInquiry.lot_id = str_tim_lot_id;
            //            objBillingInquiry = ServiceObject.SaveConsolidateInoutBillDetails(objBillingInquiry);

            //        }
            //    }

            //}

            objBillingInquiry.bill_as_of_date = p_str_bill_as_of_dt;
            objBillingInquiry.print_bill_date = p_str_print_dt;
            objBillingInquiry.inout_bill_pd_fm = p_str_doc_dt_Fr;
            objBillingInquiry.inout_bill_pd_to = p_str_doc_dt_To;
            //  objBillingInquiry.bill_doc_id = string.Empty;
            objBillingInquiry.ib_doc_id = string.Empty;
            objBillingInquiry.cmp_id = p_str_cust_id;
            objBillingInquiry = ServiceObject.GetBillingBillingType(objBillingInquiry);
            l_str_rpt_bill_type = objBillingInquiry.ListBillingType[0].bill_type;
            objBillingInquiry.BillFor = l_str_rpt_bill_type;
            objBillingInquiry.init_strg_rt_req = string.Empty;
            objBillingInquiry.cmp_id = p_str_cmp_id;

            //if (Session["sess_bill_doc_id"] != null)
            //{
            //    objBillingInquiry.bill_doc_id = Session["sess_bill_doc_id"].ToString();
            //    objBillingInquiry = ServiceObject.GetBillDelete(objBillingInquiry);
            //}


            objBillingInquiry = ServiceObject.GenerateInOutBillByCarton(objBillingInquiry);

            objBillingInquiry.bill_doc_id = objBillingInquiry.ListSaveSTRGBillDetails[0].bill_doc_id.ToString().Trim();
            Session["sess_bill_doc_id"] = string.Empty;
            return Json(objBillingInquiry.bill_doc_id, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SaveInoutConsolidateSelectedBill(string p_str_cust_id, List<clsLotId> plstLotId, string p_str_Bill_type, string p_str_Bill_doc_id, string p_str_bill_as_of_dt, string p_str_doc_dt_Fr, string p_str_doc_dt_To, string p_str_cmp_id, string p_str_print_dt)
        {
            string l_str_rpt_bill_type = string.Empty;
            objBillingInquiry.cmp_id = p_str_cmp_id;
            objBillingInquiry.cust_id = p_str_cust_id;
            objBillingInquiry.bill_type = p_str_Bill_type;
            if (p_str_Bill_doc_id.Trim().Length == 0)
            {
                objBillingInquiry = ServiceObject.GetConsolidateVASBillEntityValue(objBillingInquiry);
                objBillingInquiry.temp_bill_doc_id = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[0].temp_bill_doc_id;
            }
            else
            {

                objBillingInquiry.temp_bill_doc_id = p_str_Bill_doc_id;
            }

            int l_int_count = 0;
            l_int_count = plstLotId.Count;
            string l_str_lot_id = string.Empty;

            for (int i = 0; i < l_int_count; i++)
            {
                l_str_lot_id = plstLotId[i].lot_id.ToString();
                if (l_str_lot_id == "on")
                {
                }
                else
                {
                    objBillingInquiry.temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id;
                    objBillingInquiry.lot_id = l_str_lot_id;
                    objBillingInquiry = ServiceObject.SaveConsolidateInoutBillDetails(objBillingInquiry);

                }
            }



            objBillingInquiry.bill_as_of_date = p_str_bill_as_of_dt;
            objBillingInquiry.print_bill_date = p_str_print_dt;
            objBillingInquiry.inout_bill_pd_fm = p_str_doc_dt_Fr;
            objBillingInquiry.inout_bill_pd_to = p_str_doc_dt_To;
            //  objBillingInquiry.bill_doc_id = string.Empty;
            objBillingInquiry.ib_doc_id = string.Empty;
            objBillingInquiry.cmp_id = p_str_cust_id;
            objBillingInquiry = ServiceObject.GetBillingBillingType(objBillingInquiry);
            l_str_rpt_bill_type = objBillingInquiry.ListBillingType[0].bill_type;
            objBillingInquiry.BillFor = l_str_rpt_bill_type;
            objBillingInquiry.init_strg_rt_req = string.Empty;
            objBillingInquiry.cmp_id = p_str_cmp_id;

            //if (Session["sess_bill_doc_id"] != null)
            //{
            //    objBillingInquiry.bill_doc_id = Session["sess_bill_doc_id"].ToString();
            //    objBillingInquiry = ServiceObject.GetBillDelete(objBillingInquiry);
            //}


            objBillingInquiry = ServiceObject.GenerateInOutBillByCarton(objBillingInquiry);

            objBillingInquiry.bill_doc_id = objBillingInquiry.ListSaveSTRGBillDetails[0].bill_doc_id.ToString().Trim();
            Session["sess_bill_doc_id"] = string.Empty;
            return Json(objBillingInquiry.bill_doc_id, JsonRequestBehavior.AllowGet);
        }
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
            objInboundInquiry = ServiceObject.GetInboundLotDtl(objInboundInquiry);
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_InboundRcvdLotDetail", InboundInquiryModel);
        }
        public ActionResult VasDetail(string VasId, string cmp_id)
        {
            var status = string.Empty;
            VasInquiry objVasInquiry = new VasInquiry();
            VasInquiryService ServiceObject = new VasInquiryService();
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
            objVasInquiry.whs_id = objVasInquiry.ListVasInquiry[0].whs_id.Trim();
            if (objVasInquiry.ListVasInquiry[0].whs_id == null || objVasInquiry.ListVasInquiry[0].whs_id == "")
            {
                objVasInquiry.whs_id = "";
            }
            else
            {
                objVasInquiry.whs_id = objVasInquiry.ListVasInquiry[0].whs_id.Trim();
            }
            //objVasInquiry.whs_id = objVasInquiry.ListVasInquiry[0].whs_id.Trim();
            objVasInquiry.ship_type = objVasInquiry.ListVasInquiry[0].ship_type.Trim();
            objVasInquiry.ship_to = objVasInquiry.ListVasInquiry[0].ship_to.Trim();
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
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                        objInboundInquiry.cmp_id = cmp_id;
                        objInboundInquiry.ib_doc_id = ib_doc_id;
                        //CR - 3PL_MVC_IB_2018_0910_001
                        objInboundInquiry = ServiceObject.GET_IB_DOC_CUBE_AND_WGT(objInboundInquiry);
                        if (objInboundInquiry.ListTotalCount.Count > 0)
                        {
                            l_int_TotCtn = objInboundInquiry.ListTotalCount[0].TOT_CARTON;
                            l_dec_Totwgt = objInboundInquiry.ListTotalCount[0].TOT_WEIGHT;
                            l_dec_Totcube = objInboundInquiry.ListTotalCount[0].TOTCUBE;
                        }
                        objInboundInquiry = ServiceObject.GetInboundAckRptDetails(objInboundInquiry);
                        var rptSource = objInboundInquiry.ListAckRptDetails.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objInboundInquiry.ListAckRptDetails.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);   //CR - 3PL_MVC_IB_2018_0219_008
                                                                                           //CR - 3PL_MVC_IB_2018_0910_001
                                rd.SetParameterValue("TotCtn", l_int_TotCtn);
                                rd.SetParameterValue("TotWgt", l_dec_Totwgt);
                                rd.SetParameterValue("TotCube", l_dec_Totcube);
                                //end       
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

                            if (objInboundInquiry.ListGETRateID[0].RATEID.Trim() == "CNTR")      //CR_3PL_MVC_BL_2018_0303_001 Added By MEERA 03-03-2018
                            {
                                strReportName = "rpt_ib_doc_recv_post_confrimation_by_container.rpt";

                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                objInboundInquiry.cmp_id = p_str_cmpid;
                                objInboundInquiry.ib_doc_id = ib_doc_id;
                                objInboundInquiry = ServiceObject.GetInboundConfirmationRptDetailsbyContainer(objInboundInquiry);
                                var rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();
                                if (rptSource.Count > 0)
                                {
                                    using (ReportDocument rd = new ReportDocument())
                                    {
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
                                }


                            }
                            else//CR-20180518-001 AddedBy Nithya
                            {
                                strReportName = "rpt_ib_doc_recv_post_confrimation.rpt";

                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                                objInboundInquiry.cmp_id = cmp_id;
                                objInboundInquiry.ib_doc_id = ib_doc_id;
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
                                var rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();
                                if (rptSource.Count > 0)
                                {
                                    using (ReportDocument rd = new ReportDocument())
                                    {
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                        AlocCount = objInboundInquiry.ListConfirmationRptDetails.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);
                                        objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                        rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);    //CR - 3PL_MVC_IB_2018_0219_008

                                        //CR - 3PL_MVC_IB_2018_0910_001
                                        rd.SetParameterValue("TotCtn", l_int_TotCtn);
                                        rd.SetParameterValue("TotWgt", l_dec_Totwgt);
                                        rd.SetParameterValue("TotCube", l_dec_Totcube);
                                        //end   
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                    }
                                }
                            }
                        }


                    }
                    else if (l_str_status == "1-RCVD")
                    {
                        strReportName = "rpt_ib_doc_recv_tallysheet.rpt";
                        InboundInquiry objInboundInquiry = new InboundInquiry();
                        InboundInquiryService ServiceObject = new InboundInquiryService();

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                        objInboundInquiry.cmp_id = cmp_id;
                        objInboundInquiry.ib_doc_id = ib_doc_id;
                        //CR - 3PL_MVC_IB_2018_0910_001
                        objInboundInquiry = ServiceObject.GET_IB_RCVD_DOC_CUBE_AND_WGT(objInboundInquiry);
                        if (objInboundInquiry.ListTotalCount.Count > 0)
                        {
                            l_int_TotCtn = objInboundInquiry.ListTotalCount[0].TOT_CARTON;
                            l_dec_Totwgt = objInboundInquiry.ListTotalCount[0].TOT_WEIGHT;
                            l_dec_Totcube = objInboundInquiry.ListTotalCount[0].TOTCUBE;
                        }
                        objInboundInquiry = ServiceObject.GetInboundTallySheetRptDetails(objInboundInquiry);
                        var rptSource = objInboundInquiry.ListTallySheetRptDetails.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objInboundInquiry.ListTallySheetRptDetails.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);
                                rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);    //CR - 3PL_MVC_IB_2018_0219_008

                                //CR - 3PL_MVC_IB_2018_0910_001
                                rd.SetParameterValue("TotCtn", l_int_TotCtn);
                                rd.SetParameterValue("TotWgt", l_dec_Totwgt);
                                rd.SetParameterValue("TotCube", l_dec_Totcube);
                                //end   
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
                    VasInquiryService ServiceObject = new VasInquiryService();
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
                            objVasInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
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
        public ActionResult GenerateConsolidateShowReport(string p_str_cmp_id, string varname, string p_dt_bill_from_dt, string p_dt_bill_to_dt, string p_str_cust_id, string p_str_bill_as_of_dt, string p_str_lot_id, string Type)
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
                    if (varname == "InvoiceSummary")
                    {
                        if (Type == "PDF")
                        {
                            strReportName = "rpt_Inoutbill_Invoice_summary.rpt";
                        }
                        else
                        {
                            strReportName = "rpt_Inoutbill_Invoice_summary_Excel.rpt";
                        }

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        string l_str_bill_amount = string.Empty;
                        BillingInquiry objBillingInquiry = new BillingInquiry();
                        BillingInquiryService ServiceObject = new BillingInquiryService();
                        objBillingInquiry.cmp_id = p_str_cust_id;
                        objBillingInquiry.cust_id = p_str_cmp_id;
                        objBillingInquiry.bill_pd_fm = p_dt_bill_from_dt;
                        objBillingInquiry.bill_pd_to = p_dt_bill_to_dt;
                        objBillingInquiry = ServiceObject.ConsolidateInoutBillSummaryRpt(objBillingInquiry);
                        var rptSource = objBillingInquiry.ListSaveSTRGBillDetails.ToList();


                        if (Type == "PDF")
                        {
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objBillingInquiry.ListSaveSTRGBillDetails.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Inout Bill Summary Report");
                                }
                            }
                        }


                        else
                        {

                            BILLING_INOUT_BILL_SUMMARY_EXCEL objBillingExcel = new BILLING_INOUT_BILL_SUMMARY_EXCEL();
                            objBillingInquiry.cmp_id = p_str_cust_id;
                            objBillingInquiry.cust_id = p_str_cmp_id;
                            objBillingInquiry.bill_pd_fm = p_dt_bill_from_dt;
                            objBillingInquiry.bill_pd_to = p_dt_bill_to_dt;
                            objBillingInquiry = ServiceObject.ConsolidateInoutBillSummaryRpt(objBillingInquiry);
                            List<BILLING_INOUT_BILL_SUMMARY_EXCEL> li = new List<BILLING_INOUT_BILL_SUMMARY_EXCEL>();
                            for (int i = 0; i < objBillingInquiry.ListSaveSTRGBillDetails.Count; i++)
                            {

                                BILLING_INOUT_BILL_SUMMARY_EXCEL objOBInquiryExcel = new BILLING_INOUT_BILL_SUMMARY_EXCEL();
                                objOBInquiryExcel.IBDOCID = objBillingInquiry.ListSaveSTRGBillDetails[i].ib_doc_id;
                                objOBInquiryExcel.IBDOCDT = objBillingInquiry.ListSaveSTRGBillDetails[i].RcvdDate;
                                objOBInquiryExcel.CNTRNO = objBillingInquiry.ListSaveSTRGBillDetails[i].cont_id;
                                objOBInquiryExcel.LOTID = objBillingInquiry.ListSaveSTRGBillDetails[i].lot_id;
                                objOBInquiryExcel.Cust_Po = objBillingInquiry.ListSaveSTRGBillDetails[i].po_num;
                                objOBInquiryExcel.RateType = objBillingInquiry.ListSaveSTRGBillDetails[i].RateType;
                                objOBInquiryExcel.Ctns = objBillingInquiry.ListSaveSTRGBillDetails[i].TotalCtns;
                                objOBInquiryExcel.Cube = objBillingInquiry.ListSaveSTRGBillDetails[i].TotCube;
                                objOBInquiryExcel.IORate = objBillingInquiry.ListSaveSTRGBillDetails[i].list_price;
                                objOBInquiryExcel.Amount = objBillingInquiry.ListSaveSTRGBillDetails[i].TotAmount;
                                li.Add(objOBInquiryExcel);
                            }
                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["BILL_INOUT_SUMMARY"] = gv;
                            return new DownloadFileActionResult((GridView)Session["BILL_INOUT_SUMMARY"], "BILL_INOUT_SUMMARY" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
                        }
                    }
                    else
                    {
                        if (Type == "PDF")
                        {
                            strReportName = "rpt_Inoutbill_Invoice_detail.rpt";
                        }
                        else
                        {
                            strReportName = "rpt_Inoutbill_Invoice_detail_Excel.rpt";
                        }

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        string l_str_bill_amount = string.Empty;
                        BillingInquiry objBillingInquiry = new BillingInquiry();
                        BillingInquiryService ServiceObject = new BillingInquiryService();
                        objBillingInquiry.cmp_id = p_str_cust_id;
                        objBillingInquiry.cust_id = p_str_cmp_id;
                        objBillingInquiry = ServiceObject.GetConsolidateVASBillEntityValue(objBillingInquiry);
                        objBillingInquiry.temp_bill_doc_id = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[0].temp_bill_doc_id;
                        string[] str_arry_lot_id = p_str_lot_id.Split(',');
                        foreach (string str_tim_lot_id in str_arry_lot_id)
                        {
                            if (str_tim_lot_id == "on") // To exclude the Header checkbox
                            { }
                            else
                            {
                                objBillingInquiry.temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id;
                                objBillingInquiry.lot_id = str_tim_lot_id;
                                objBillingInquiry = ServiceObject.SaveConsolidateInoutBillDetails(objBillingInquiry);
                            }
                        }
                        objBillingInquiry.cmp_id = p_str_cust_id;
                        objBillingInquiry.cust_id = p_str_cmp_id;
                        objBillingInquiry.bill_pd_fm = p_dt_bill_from_dt;
                        objBillingInquiry.bill_pd_to = p_dt_bill_to_dt;

                        if (Type == "PDF")
                        {
                            objBillingInquiry = ServiceObject.ConsolidateInoutBillDetailRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListSaveSTRGBillDetails.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objBillingInquiry.ListSaveSTRGBillDetails.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Inout Bill Summary Report");
                                }
                            }
                        }

                        else
                        {
                            BILLING_INOUT_BILL_DETAIL_EXCEL objBillingExcel = new BILLING_INOUT_BILL_DETAIL_EXCEL();
                            objBillingInquiry.cmp_id = p_str_cust_id;
                            objBillingInquiry.cust_id = p_str_cmp_id;
                            objBillingInquiry = ServiceObject.GetConsolidateVASBillEntityValue(objBillingInquiry);
                            objBillingInquiry.temp_bill_doc_id = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[0].temp_bill_doc_id;
                            str_arry_lot_id = p_str_lot_id.Split(',');
                            foreach (string str_tim_lot_id in str_arry_lot_id)
                            {
                                if (str_tim_lot_id == "on") // To exclude the Header checkbox
                                { }
                                else
                                {
                                    objBillingInquiry.temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id;
                                    objBillingInquiry.lot_id = str_tim_lot_id;
                                    objBillingInquiry = ServiceObject.SaveConsolidateInoutBillDetails(objBillingInquiry);
                                }
                            }
                            objBillingInquiry.cmp_id = p_str_cust_id;
                            objBillingInquiry.cust_id = p_str_cmp_id;
                            objBillingInquiry.bill_pd_fm = p_dt_bill_from_dt;
                            objBillingInquiry.bill_pd_to = p_dt_bill_to_dt;
                            objBillingInquiry = ServiceObject.ConsolidateInoutBillDetailRpt(objBillingInquiry);
                            List<BILLING_INOUT_BILL_DETAIL_EXCEL> li = new List<BILLING_INOUT_BILL_DETAIL_EXCEL>();
                            for (int i = 0; i < objBillingInquiry.ListSaveSTRGBillDetails.Count; i++)
                            {

                                BILLING_INOUT_BILL_DETAIL_EXCEL objOBInquiryExcel = new BILLING_INOUT_BILL_DETAIL_EXCEL();
                                objOBInquiryExcel.IBDOCID = objBillingInquiry.ListSaveSTRGBillDetails[i].ib_doc_id;
                                objOBInquiryExcel.IBDOCDT = Convert.ToDateTime(objBillingInquiry.ListSaveSTRGBillDetails[i].ib_doc_dt);
                                objOBInquiryExcel.CNTRNO = objBillingInquiry.ListSaveSTRGBillDetails[i].cont_id;
                                objOBInquiryExcel.Cust_Po = objBillingInquiry.ListSaveSTRGBillDetails[i].po_num;
                                objOBInquiryExcel.WhsId = objBillingInquiry.ListSaveSTRGBillDetails[i].whs_id;
                                objOBInquiryExcel.RcvdDt = objBillingInquiry.ListSaveSTRGBillDetails[i].palet_dt;
                                objOBInquiryExcel.Style = objBillingInquiry.ListSaveSTRGBillDetails[i].itm_num;
                                objOBInquiryExcel.Color = objBillingInquiry.ListSaveSTRGBillDetails[i].itm_color;
                                objOBInquiryExcel.Size = objBillingInquiry.ListSaveSTRGBillDetails[i].itm_size;
                                objOBInquiryExcel.LOTID = objBillingInquiry.ListSaveSTRGBillDetails[i].lot_id;
                                objOBInquiryExcel.Cube = objBillingInquiry.ListSaveSTRGBillDetails[i].TotCube;
                                objOBInquiryExcel.Wgt = objBillingInquiry.ListSaveSTRGBillDetails[i].wgt;
                                objOBInquiryExcel.LocId = objBillingInquiry.ListSaveSTRGBillDetails[i].loc_id;
                                objOBInquiryExcel.RateType = objBillingInquiry.ListSaveSTRGBillDetails[i].RateType;
                                objOBInquiryExcel.Ctns = objBillingInquiry.ListSaveSTRGBillDetails[i].avail_cnt;
                                objOBInquiryExcel.Ppk = objBillingInquiry.ListSaveSTRGBillDetails[i].itm_qty;
                                objOBInquiryExcel.Pcs = objBillingInquiry.ListSaveSTRGBillDetails[i].avail_qty;
                                li.Add(objOBInquiryExcel);
                            }
                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["BILL_INOUT_DETAIL"] = gv;
                            return new DownloadFileActionResult((GridView)Session["BILL_INOUT_DETAIL"], "BILL_INOUT_DETAIL" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
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
        public ActionResult GenerateConsolidateStrgShowReport(string p_str_cmp_id, string p_dt_bill_from_dt, string p_dt_bill_to_dt, string p_str_cust_id, string p_str_bill_as_of_dt, string Type)
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
                    if (Type == "PDF")
                    {

                        strReportName = "rpt_Strgbill_Invoice_summary.rpt";
                    }
                    else
                    {

                        strReportName = "rpt_Strgbill_Invoice_summary_Excel.rpt";
                    }

                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                    string l_str_bill_amount = string.Empty;
                    BillingInquiry objBillingInquiry = new BillingInquiry();
                    BillingInquiryService ServiceObject = new BillingInquiryService();
                    objBillingInquiry.cmp_id = p_str_cust_id;
                    objBillingInquiry.cust_id = p_str_cmp_id;
                    objBillingInquiry.bill_as_of_date = p_str_bill_as_of_dt;
                    objBillingInquiry.bill_pd_to = p_dt_bill_to_dt;

                    if (Type == "PDF")
                    {
                        objBillingInquiry = ServiceObject.ConsolidateStorageBillSummaryRpt(objBillingInquiry);
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
                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "STRG Bill Summary Report");
                            }
                        }
                    }

                    else
                    {
                        BILLING_STRG_BILL_SUMMARY_EXCEL objBillingExcel = new BILLING_STRG_BILL_SUMMARY_EXCEL();
                        objBillingInquiry.cmp_id = p_str_cust_id;
                        objBillingInquiry.cust_id = p_str_cmp_id;
                        objBillingInquiry.bill_as_of_date = p_str_bill_as_of_dt;
                        objBillingInquiry.bill_pd_to = p_dt_bill_to_dt;
                        objBillingInquiry = ServiceObject.ConsolidateStorageBillSummaryRpt(objBillingInquiry);
                        List<BILLING_STRG_BILL_SUMMARY_EXCEL> li = new List<BILLING_STRG_BILL_SUMMARY_EXCEL>();
                        for (int i = 0; i < objBillingInquiry.ListGetSTRGBillByPcsRpt.Count; i++)
                        {

                            BILLING_STRG_BILL_SUMMARY_EXCEL objOBInquiryExcel = new BILLING_STRG_BILL_SUMMARY_EXCEL();
                            objOBInquiryExcel.Style = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].itm_num;
                            objOBInquiryExcel.Color = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].itm_color;
                            objOBInquiryExcel.Size = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].itm_size;
                            objOBInquiryExcel.PPk = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].itm_qty;
                            objOBInquiryExcel.AvailQty = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].avail_qty;
                            objOBInquiryExcel.LocId = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].loc_id;
                            objOBInquiryExcel.Ctns = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].TotCtns;
                            objOBInquiryExcel.Cube = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].TotCube;
                            objOBInquiryExcel.Wgt = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].TotWeight;
                            objOBInquiryExcel.Rate = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].Rate;
                            objOBInquiryExcel.Amount = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].TotAmount;
                            li.Add(objOBInquiryExcel);
                        }
                        GridView gv = new GridView();
                        gv.DataSource = li;
                        gv.DataBind();
                        Session["BILL_STRG_SUMMARY"] = gv;
                        return new DownloadFileActionResult((GridView)Session["BILL_STRG_SUMMARY"], "BILL_STRG_SUMMARY" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
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
        public ActionResult GenerateConsolidateVasShowReport(string p_str_cmp_id, string var_name, string p_dt_bill_from_dt, string p_dt_bill_to_dt, string p_str_cust_id, string p_str_bill_as_of_dt, string p_str_ship_doc_id, string Type)
        {
            try
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

                        if (var_name == "VasInvoiceSummary")
                        {
                            if (Type == "PDF")
                            {
                                strReportName = "rpt_Vasbill_Invoice_summary.rpt";
                            }
                            else
                            {
                                strReportName = "rpt_Vasbill_Invoice_summary_Excel.rpt";
                            }

                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            string l_str_bill_amount = string.Empty;
                            BillingInquiry objBillingInquiry = new BillingInquiry();
                            BillingInquiryService ServiceObject = new BillingInquiryService();
                            objBillingInquiry.cmp_id = p_str_cmp_id;
                            objBillingInquiry.bill_pd_fm = p_dt_bill_from_dt;
                            objBillingInquiry.bill_pd_to = p_dt_bill_to_dt;

                            if (Type == "PDF")
                            {

                                objBillingInquiry = ServiceObject.GetConsolidateVASSummaryRpt(objBillingInquiry);
                                var rptSource = objBillingInquiry.ListBillRcvdDetails.ToList();
                                if (rptSource.Count > 0)
                                {
                                    using (ReportDocument rd = new ReportDocument())
                                    {
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                        AlocCount = objBillingInquiry.ListBillRcvdDetails.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);
                                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Vas Bill Summary Report");
                                    }
                                }
                            }

                            else
                            {
                                BILLING_VAS_BILL_SUMMARY_EXCEL objBillingExcel = new BILLING_VAS_BILL_SUMMARY_EXCEL();
                                objBillingInquiry.cmp_id = p_str_cmp_id;
                                objBillingInquiry.bill_pd_fm = p_dt_bill_from_dt;
                                objBillingInquiry.bill_pd_to = p_dt_bill_to_dt;
                                objBillingInquiry = ServiceObject.GetConsolidateVASSummaryRpt(objBillingInquiry);
                                List<BILLING_VAS_BILL_SUMMARY_EXCEL> li = new List<BILLING_VAS_BILL_SUMMARY_EXCEL>();
                                for (int i = 0; i < objBillingInquiry.ListBillRcvdDetails.Count; i++)
                                {
                                    BILLING_VAS_BILL_SUMMARY_EXCEL objOBInquiryExcel = new BILLING_VAS_BILL_SUMMARY_EXCEL();
                                    objOBInquiryExcel.VASId = objBillingInquiry.ListBillRcvdDetails[i].ship_doc_id;
                                    objOBInquiryExcel.VasDt = objBillingInquiry.ListBillRcvdDetails[i].ShipDate;
                                    objOBInquiryExcel.WhsId = objBillingInquiry.ListBillRcvdDetails[i].whs_id;
                                    objOBInquiryExcel.Customer = objBillingInquiry.ListBillRcvdDetails[i].cust_id;
                                    objOBInquiryExcel.CustPo = objBillingInquiry.ListBillRcvdDetails[i].po_num;
                                    objOBInquiryExcel.SoNo = objBillingInquiry.ListBillRcvdDetails[i].so_num;
                                    objOBInquiryExcel.BillAmt = objBillingInquiry.ListBillRcvdDetails[i].TotalPrice;
                                    li.Add(objOBInquiryExcel);
                                }
                                GridView gv = new GridView();
                                gv.DataSource = li;
                                gv.DataBind();
                                Session["BILL_VAS_SUMMARY"] = gv;
                                return new DownloadFileActionResult((GridView)Session["BILL_VAS_SUMMARY"], "BILL_VAS_SUMMARY" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
                            }
                        }
                        else
                        {
                            string[] str_arry_ship_doc_id = p_str_ship_doc_id.Split(',');
                            if (Type == "PDF")
                            {
                                strReportName = "rpt_iv_vas_bill_detail.rpt";
                            }
                            else
                            {
                                strReportName = "rpt_iv_vas_bill_detail_Excel.rpt";
                            }

                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            string l_str_bill_amount = string.Empty;
                            BillingInquiry objBillingInquiry = new BillingInquiry();
                            BillingInquiryService ServiceObject = new BillingInquiryService();
                            objBillingInquiry.cmp_id = p_str_cust_id;
                            objBillingInquiry.cust_id = p_str_cmp_id;
                            objBillingInquiry = ServiceObject.GetConsolidateVASBillEntityValue(objBillingInquiry);
                            objBillingInquiry.temp_bill_doc_id = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[0].temp_bill_doc_id;
                            foreach (string str_tim_ship_doc_id in str_arry_ship_doc_id)
                            {
                                if (str_tim_ship_doc_id == "on") // To exclude the Header checkbox
                                { }
                                else
                                {
                                    objBillingInquiry.temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id;
                                    objBillingInquiry.ship_doc_id = str_tim_ship_doc_id;
                                    objBillingInquiry = ServiceObject.SaveConsolidateBillDetails(objBillingInquiry);
                                }
                            }
                            objBillingInquiry.cust_id = p_str_cmp_id;
                            objBillingInquiry.vas_bill_pd_fm = p_dt_bill_from_dt;
                            objBillingInquiry.vas_bill_pd_to = p_dt_bill_to_dt;
                            objBillingInquiry.bill_doc_id = string.Empty;
                            objBillingInquiry.cmp_id = p_str_cust_id;

                            if (Type == "PDF")
                            {
                                objBillingInquiry = ServiceObject.GenerateVASBillDetailRpt(objBillingInquiry);
                                var rptSource = objBillingInquiry.ListSaveVASBillDetails.ToList();
                                if (rptSource.Count > 0)
                                {
                                    using (ReportDocument rd = new ReportDocument())
                                    {
                                        rd.Load(strRptPath);
                                        rd.SetDataSource(rptSource);
                                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Vas Bill Summary Report");
                                    }
                                }
                            }

                            else
                            {
                                str_arry_ship_doc_id = p_str_ship_doc_id.Split(',');
                                BILLING_VAS_BILL_DETAIL_EXCEL objBillingExcel = new BILLING_VAS_BILL_DETAIL_EXCEL();
                                objBillingInquiry.cmp_id = p_str_cust_id;
                                objBillingInquiry.cust_id = p_str_cmp_id;
                                objBillingInquiry = ServiceObject.GetConsolidateVASBillEntityValue(objBillingInquiry);
                                objBillingInquiry.temp_bill_doc_id = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[0].temp_bill_doc_id;
                                foreach (string str_tim_ship_doc_id in str_arry_ship_doc_id)
                                {
                                    if (str_tim_ship_doc_id == "on") // To exclude the Header checkbox
                                    { }
                                    else
                                    {
                                        objBillingInquiry.temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id;
                                        objBillingInquiry.ship_doc_id = str_tim_ship_doc_id;
                                        objBillingInquiry = ServiceObject.SaveConsolidateBillDetails(objBillingInquiry);
                                    }
                                }
                                objBillingInquiry.cust_id = p_str_cmp_id;
                                objBillingInquiry.vas_bill_pd_fm = p_dt_bill_from_dt;
                                objBillingInquiry.vas_bill_pd_to = p_dt_bill_to_dt;
                                objBillingInquiry.bill_doc_id = string.Empty;
                                objBillingInquiry.cmp_id = p_str_cust_id;
                                objBillingInquiry = ServiceObject.GenerateVASBillDetailRpt(objBillingInquiry);
                                List<BILLING_VAS_BILL_DETAIL_EXCEL> li = new List<BILLING_VAS_BILL_DETAIL_EXCEL>();
                                for (int i = 0; i < objBillingInquiry.ListSaveVASBillDetails.Count; i++)
                                {
                                    BILLING_VAS_BILL_DETAIL_EXCEL objOBInquiryExcel = new BILLING_VAS_BILL_DETAIL_EXCEL();
                                    objOBInquiryExcel.VASId = objBillingInquiry.ListSaveVASBillDetails[i].ship_doc_id;
                                    objOBInquiryExcel.VasDt = objBillingInquiry.ListSaveVASBillDetails[i].ShipDt;
                                    objOBInquiryExcel.WhsId = objBillingInquiry.ListSaveVASBillDetails[i].whs_id;
                                    objOBInquiryExcel.Customer = objBillingInquiry.ListSaveVASBillDetails[i].cust_name;
                                    objOBInquiryExcel.CustPo = objBillingInquiry.ListSaveVASBillDetails[i].cust_ordr_num;
                                    objOBInquiryExcel.ShipTo = objBillingInquiry.ListSaveVASBillDetails[i].ship_to;
                                    objOBInquiryExcel.ServiceId = objBillingInquiry.ListSaveVASBillDetails[i].so_itm_num;
                                    objOBInquiryExcel.ServiceDesc = objBillingInquiry.ListSaveVASBillDetails[i].itm_name;
                                    objOBInquiryExcel.RateCatg = objBillingInquiry.ListSaveVASBillDetails[i].catg;
                                    objOBInquiryExcel.Units = objBillingInquiry.ListSaveVASBillDetails[i].ship_qty;
                                    objOBInquiryExcel.Rate = objBillingInquiry.ListSaveVASBillDetails[i].ship_itm_price;
                                    objOBInquiryExcel.Amount = (objOBInquiryExcel.Units * objOBInquiryExcel.Rate);
                                    li.Add(objOBInquiryExcel);
                                }
                                GridView gv = new GridView();
                                gv.DataSource = li;
                                gv.DataBind();
                                Session["BILL_VAS_DETAIL"] = gv;
                                return new DownloadFileActionResult((GridView)Session["BILL_VAS_DETAIL"], "BILL_VAS_DETAIL" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");

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
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult BillinoutSummaryEmailRpt(string p_str_cmp_id, string varname, string p_dt_bill_from_dt, string p_dt_bill_to_dt, string p_str_cust_id, string p_str_bill_as_of_dt, string p_str_lot_id, string Type)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_bill_doc_type = "INOUT";
            string strDateFormat = string.Empty;
            string strFileName = string.Empty;
            string reportFileName = string.Empty;
            decimal l_dec_tot_bill_amnt = 0;
            string tempFilepath = System.Configuration.ConfigurationManager.AppSettings["tempFilePath"].ToString().Trim();
            try
            {
                if (isValid)
                {
                    if (varname == "InvoiceSummary")
                    {
                        if (Type == "PDF")
                        {
                            strReportName = "rpt_Inoutbill_Invoice_summary.rpt";

                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            string l_str_bill_amount = string.Empty;
                            BillingInquiry objBillingInquiry = new BillingInquiry();
                            BillingInquiryService ServiceObject = new BillingInquiryService();
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
                            objBillingInquiry.cmp_id = p_str_cust_id;
                            objBillingInquiry.cust_id = p_str_cmp_id;
                            objBillingInquiry.bill_pd_fm = p_dt_bill_from_dt;
                            objBillingInquiry.bill_pd_to = p_dt_bill_to_dt;
                            objBillingInquiry = ServiceObject.ConsolidateInoutBillSummaryRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListSaveSTRGBillDetails.ToList();
                            //CR201800809-001 Added by Nithya
                            for (int j = 0; j < objBillingInquiry.ListSaveSTRGBillDetails.Count(); j++)
                            {
                                var bill_amt = (objBillingInquiry.ListSaveSTRGBillDetails[j].TotAmount);
                                decimal l_dec_bill_amnt = bill_amt;
                                if (l_dec_bill_amnt > 0)
                                {
                                    l_dec_tot_bill_amnt = l_dec_tot_bill_amnt + l_dec_bill_amnt;
                                }

                            }
                            //END
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                l_str_rptdtl = objBillingInquiry.cmp_id + "_" + "IN-OUT Bill" + "_" + objBillingInquiry.cust_id + "_" + objBillingInquiry.bill_pd_fm + "_" + objBillingInquiry.bill_pd_to;
                                objEmail.EmailSubject = objBillingInquiry.cmp_id + "-" + "IN-OUT BILL " + "|" + " " + "Customer#: " + objBillingInquiry.cust_id + "|" + " " + "PeriodFm:  " + "$" + objBillingInquiry.bill_pd_fm + "|" + " " + "Period To: " + objBillingInquiry.bill_pd_to + "Bill Amt:  " + "$" + l_dec_tot_bill_amnt;
                                objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Bill Type: " + l_str_rpt_bill_doc_type + "\n" + "Customer: " + objBillingInquiry.cust_id + "\n" + "Period fm: " + objBillingInquiry.bill_pd_fm + "\n" + "Period To: " + objBillingInquiry.bill_pd_to + "\n" + "Bill Amt: " + l_dec_tot_bill_amnt;
                                int AlocCount = 0;
                                AlocCount = objBillingInquiry.ListSaveSTRGBillDetails.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + tempFilepath + "//Generate InoutBill GridSummary_" + strDateFormat + ".pdf";
                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            }
                            reportFileName = "Generate InoutBill GridSummary Report " + DateTime.Now.ToFileTime() + ".pdf";
                            Session["RptFileName"] = strFileName;
                            //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Inout Bill Summary Report");
                        }
                        else
                        {
                            BILLING_INOUT_BILL_SUMMARY_EXCEL objBillingExcel = new BILLING_INOUT_BILL_SUMMARY_EXCEL();
                            objBillingInquiry.cmp_id = p_str_cust_id;
                            objBillingInquiry.cust_id = p_str_cmp_id;
                            objBillingInquiry.bill_pd_fm = p_dt_bill_from_dt;
                            objBillingInquiry.bill_pd_to = p_dt_bill_to_dt;
                            objBillingInquiry = ServiceObject.ConsolidateInoutBillSummaryRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListSaveSTRGBillDetails.ToList();
                            List<BILLING_INOUT_BILL_SUMMARY_EXCEL> li = new List<BILLING_INOUT_BILL_SUMMARY_EXCEL>();
                            for (int i = 0; i < objBillingInquiry.ListSaveSTRGBillDetails.Count; i++)
                            {

                                BILLING_INOUT_BILL_SUMMARY_EXCEL objOBInquiryExcel = new BILLING_INOUT_BILL_SUMMARY_EXCEL();
                                objOBInquiryExcel.IBDOCID = objBillingInquiry.ListSaveSTRGBillDetails[i].ib_doc_id;
                                objOBInquiryExcel.IBDOCDT = objBillingInquiry.ListSaveSTRGBillDetails[i].RcvdDate;
                                objOBInquiryExcel.CNTRNO = objBillingInquiry.ListSaveSTRGBillDetails[i].cont_id;
                                objOBInquiryExcel.LOTID = objBillingInquiry.ListSaveSTRGBillDetails[i].lot_id;
                                objOBInquiryExcel.Cust_Po = objBillingInquiry.ListSaveSTRGBillDetails[i].po_num;
                                objOBInquiryExcel.RateType = objBillingInquiry.ListSaveSTRGBillDetails[i].RateType;
                                objOBInquiryExcel.Ctns = objBillingInquiry.ListSaveSTRGBillDetails[i].TotalCtns;
                                objOBInquiryExcel.Cube = objBillingInquiry.ListSaveSTRGBillDetails[i].TotCube;
                                objOBInquiryExcel.IORate = objBillingInquiry.ListSaveSTRGBillDetails[i].list_price;
                                objOBInquiryExcel.Amount = objBillingInquiry.ListSaveSTRGBillDetails[i].TotAmount;
                                li.Add(objOBInquiryExcel);
                            }
                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["BILL_INOUT_SUMMARY"] = gv;
                            return new DownloadFileActionResult((GridView)Session["BILL_INOUT_SUMMARY"], "BILL_INOUT_SUMMARY" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
                        }
                    }
                    else
                    {
                        if (Type == "PDF")
                        {
                            strReportName = "rpt_Inoutbill_Invoice_detail.rpt";

                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            string l_str_bill_amount = string.Empty;
                            BillingInquiry objBillingInquiry = new BillingInquiry();
                            BillingInquiryService ServiceObject = new BillingInquiryService();
                            objBillingInquiry.cmp_id = p_str_cust_id;
                            objBillingInquiry.cust_id = p_str_cmp_id;
                            objBillingInquiry = ServiceObject.GetConsolidateVASBillEntityValue(objBillingInquiry);
                            objBillingInquiry.temp_bill_doc_id = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[0].temp_bill_doc_id;
                            string[] str_arry_lot_id = p_str_lot_id.Split(',');
                            foreach (string str_tim_lot_id in str_arry_lot_id)
                            {
                                if (str_tim_lot_id == "on") // To exclude the Header checkbox
                                { }
                                else
                                {
                                    objBillingInquiry.temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id;
                                    objBillingInquiry.lot_id = str_tim_lot_id;
                                    objBillingInquiry = ServiceObject.SaveConsolidateInoutBillDetails(objBillingInquiry);
                                }
                            }
                            objBillingInquiry.cmp_id = p_str_cust_id;
                            objBillingInquiry.cust_id = p_str_cmp_id;
                            objBillingInquiry.bill_pd_fm = p_dt_bill_from_dt;
                            objBillingInquiry.bill_pd_to = p_dt_bill_to_dt;
                            objBillingInquiry = ServiceObject.ConsolidateInoutBillDetailRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListSaveSTRGBillDetails.ToList();
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
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    l_str_rptdtl = objBillingInquiry.cmp_id;
                                    objEmail.EmailSubject = objBillingInquiry.cmp_id;
                                    objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Bill Type: " + l_str_rpt_bill_doc_type;
                                    AlocCount = objBillingInquiry.ListSaveSTRGBillDetails.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                    //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Inout Bill Summary Report");
                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + tempFilepath + "//Generate InoutBill Detail_" + strDateFormat + ".pdf";
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                }
                            }
                            reportFileName = "Generate InoutBill Detail Report " + DateTime.Now.ToFileTime() + ".pdf";
                            Session["RptFileName"] = strFileName;
                        }
                        else
                        {
                            BILLING_INOUT_BILL_DETAIL_EXCEL objBillingExcel = new BILLING_INOUT_BILL_DETAIL_EXCEL();
                            objBillingInquiry.cmp_id = p_str_cust_id;
                            objBillingInquiry.cust_id = p_str_cmp_id;
                            objBillingInquiry = ServiceObject.GetConsolidateVASBillEntityValue(objBillingInquiry);
                            objBillingInquiry.temp_bill_doc_id = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[0].temp_bill_doc_id;
                            string[] str_arry_lot_id = p_str_lot_id.Split(',');
                            foreach (string str_tim_lot_id in str_arry_lot_id)
                            {
                                if (str_tim_lot_id == "on") // To exclude the Header checkbox
                                { }
                                else
                                {
                                    objBillingInquiry.temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id;
                                    objBillingInquiry.lot_id = str_tim_lot_id;
                                    objBillingInquiry = ServiceObject.SaveConsolidateInoutBillDetails(objBillingInquiry);
                                }
                            }
                            objBillingInquiry.cmp_id = p_str_cust_id;
                            objBillingInquiry.cust_id = p_str_cmp_id;
                            objBillingInquiry.bill_pd_fm = p_dt_bill_from_dt;
                            objBillingInquiry.bill_pd_to = p_dt_bill_to_dt;
                            objBillingInquiry = ServiceObject.ConsolidateInoutBillDetailRpt(objBillingInquiry);
                            List<BILLING_INOUT_BILL_DETAIL_EXCEL> li = new List<BILLING_INOUT_BILL_DETAIL_EXCEL>();
                            for (int i = 0; i < objBillingInquiry.ListSaveSTRGBillDetails.Count; i++)
                            {

                                BILLING_INOUT_BILL_DETAIL_EXCEL objOBInquiryExcel = new BILLING_INOUT_BILL_DETAIL_EXCEL();
                                objOBInquiryExcel.IBDOCID = objBillingInquiry.ListSaveSTRGBillDetails[i].ib_doc_id;
                                objOBInquiryExcel.IBDOCDT = Convert.ToDateTime(objBillingInquiry.ListSaveSTRGBillDetails[i].ib_doc_dt);
                                objOBInquiryExcel.CNTRNO = objBillingInquiry.ListSaveSTRGBillDetails[i].cont_id;
                                objOBInquiryExcel.Cust_Po = objBillingInquiry.ListSaveSTRGBillDetails[i].po_num;
                                objOBInquiryExcel.WhsId = objBillingInquiry.ListSaveSTRGBillDetails[i].whs_id;
                                objOBInquiryExcel.RcvdDt = objBillingInquiry.ListSaveSTRGBillDetails[i].palet_dt;
                                objOBInquiryExcel.Style = objBillingInquiry.ListSaveSTRGBillDetails[i].itm_num;
                                objOBInquiryExcel.Color = objBillingInquiry.ListSaveSTRGBillDetails[i].itm_color;
                                objOBInquiryExcel.Size = objBillingInquiry.ListSaveSTRGBillDetails[i].itm_size;
                                objOBInquiryExcel.LOTID = objBillingInquiry.ListSaveSTRGBillDetails[i].lot_id;
                                objOBInquiryExcel.Cube = objBillingInquiry.ListSaveSTRGBillDetails[i].TotCube;
                                objOBInquiryExcel.Wgt = objBillingInquiry.ListSaveSTRGBillDetails[i].wgt;
                                objOBInquiryExcel.LocId = objBillingInquiry.ListSaveSTRGBillDetails[i].loc_id;
                                objOBInquiryExcel.RateType = objBillingInquiry.ListSaveSTRGBillDetails[i].RateType;
                                objOBInquiryExcel.Ctns = objBillingInquiry.ListSaveSTRGBillDetails[i].avail_cnt;
                                objOBInquiryExcel.Ppk = objBillingInquiry.ListSaveSTRGBillDetails[i].itm_qty;
                                objOBInquiryExcel.Pcs = objBillingInquiry.ListSaveSTRGBillDetails[i].avail_qty;
                                li.Add(objOBInquiryExcel);
                            }
                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["BILL_INOUT_DETAIL"] = gv;
                            return new DownloadFileActionResult((GridView)Session["BILL_INOUT_DETAIL"], "BILL_INOUT_DETAIL" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
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

        //public ActionResult ReGenerateBill(string p_str_cmp_id, string p_str_bill_doc_id)
        //{
        //    string jsonErrorCode = "0";
        //    string l_str_msg = string.Empty;
        //    bool l_bol_sp_status = false;

        //    try
        //    {
        //        BillingInquiryService ServiceObject = new BillingInquiryService();
        //        // l_bol_sp_status = ServiceObject.BillReGenerate(p_str_cmp_id, p_str_bill_doc_id);
        //        if (l_bol_sp_status == false)
        //            jsonErrorCode = "-2";

        //    }
        //    catch (Exception ex)
        //    {
        //        l_str_msg = ex.Message;
        //        jsonErrorCode = "-2";
        //    }

        //    return Json(new { result = jsonErrorCode, err = l_str_msg }, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult BillStrgSummaryEmailRpt(string p_str_cmp_id, string p_dt_bill_from_dt, string p_dt_bill_to_dt, string p_str_cust_id, string p_str_bill_as_of_dt, string Type)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string strDateFormat = string.Empty;
            string strFileName = string.Empty;
            string reportFileName = string.Empty;
            string l_str_rpt_bill_doc_type = "STRG";
            string tempFilepath = System.Configuration.ConfigurationManager.AppSettings["tempFilePath"].ToString().Trim();
            try
            {
                if (isValid)
                {
                    if (Type == "PDF")
                    {

                        strReportName = "rpt_Strgbill_Invoice_summary.rpt";
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        string l_str_bill_amount = string.Empty;
                        BillingInquiry objBillingInquiry = new BillingInquiry();
                        BillingInquiryService ServiceObject = new BillingInquiryService();
                        objBillingInquiry.cmp_id = p_str_cust_id;
                        objBillingInquiry.cust_id = p_str_cmp_id;
                        objBillingInquiry.bill_as_of_date = p_str_bill_as_of_dt;
                        objBillingInquiry.bill_pd_to = p_dt_bill_to_dt;
                        objBillingInquiry = ServiceObject.ConsolidateStorageBillSummaryRpt(objBillingInquiry);
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

                                l_str_rptdtl = objBillingInquiry.cmp_id;
                                objEmail.EmailSubject = objBillingInquiry.cmp_id;
                                objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Bill Type: " + l_str_rpt_bill_doc_type;

                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "STRG Bill Summary Report");
                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + tempFilepath + "//Generate StrgBill GridSummary_" + strDateFormat + ".pdf";
                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            }
                        }
                        reportFileName = "Generate StrgBill GridSummary Report " + DateTime.Now.ToFileTime() + ".pdf";
                        Session["RptFileName"] = strFileName;
                    }
                    else
                    {
                        BILLING_STRG_BILL_SUMMARY_EXCEL objBillingExcel = new BILLING_STRG_BILL_SUMMARY_EXCEL();
                        objBillingInquiry.cmp_id = p_str_cust_id;
                        objBillingInquiry.cust_id = p_str_cmp_id;
                        objBillingInquiry.bill_as_of_date = p_str_bill_as_of_dt;
                        objBillingInquiry.bill_pd_to = p_dt_bill_to_dt;
                        objBillingInquiry = ServiceObject.ConsolidateStorageBillSummaryRpt(objBillingInquiry);
                        List<BILLING_STRG_BILL_SUMMARY_EXCEL> li = new List<BILLING_STRG_BILL_SUMMARY_EXCEL>();
                        for (int i = 0; i < objBillingInquiry.ListGetSTRGBillByPcsRpt.Count; i++)
                        {

                            BILLING_STRG_BILL_SUMMARY_EXCEL objOBInquiryExcel = new BILLING_STRG_BILL_SUMMARY_EXCEL();
                            objOBInquiryExcel.Style = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].itm_num;
                            objOBInquiryExcel.Color = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].itm_color;
                            objOBInquiryExcel.Size = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].itm_size;
                            objOBInquiryExcel.PPk = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].itm_qty;
                            objOBInquiryExcel.AvailQty = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].avail_qty;
                            objOBInquiryExcel.LocId = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].loc_id;
                            objOBInquiryExcel.Ctns = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].TotCtns;
                            objOBInquiryExcel.Cube = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].TotCube;
                            objOBInquiryExcel.Wgt = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].TotWeight;
                            objOBInquiryExcel.Rate = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].Rate;
                            objOBInquiryExcel.Amount = objBillingInquiry.ListGetSTRGBillByPcsRpt[i].TotAmount;
                            li.Add(objOBInquiryExcel);
                        }
                        GridView gv = new GridView();
                        gv.DataSource = li;
                        gv.DataBind();
                        Session["BILL_STRG_SUMMARY"] = gv;
                        return new DownloadFileActionResult((GridView)Session["BILL_STRG_SUMMARY"], "BILL_STRG_SUMMARY" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
                    }
                }
                else
                {
                    Response.Write("<H2>Report not found</H2>");
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
        public ActionResult BillVASSummaryEmailRpt(string p_str_cmp_id, string var_name, string p_dt_bill_from_dt, string p_dt_bill_to_dt, string p_str_cust_id, string p_str_bill_as_of_dt, string p_str_ship_doc_id, string Type)
        {
            try
            {
                bool isValid = true;
                string jsonErrorCode = "0";
                string strReportName = string.Empty;
                string msg = "";
                string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
                string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
                string l_str_rpt_bill_doc_type = "NORM";
                string strDateFormat = string.Empty;
                string strFileName = string.Empty;
                string reportFileName = string.Empty;
                string tempFilepath = System.Configuration.ConfigurationManager.AppSettings["tempFilePath"].ToString().Trim();
                try
                {
                    if (isValid)
                    {

                        if (var_name == "VasInvoiceSummary")
                        {
                            if (Type == "PDF")
                            {
                                strReportName = "rpt_Vasbill_Invoice_summary.rpt";
                            }
                            else
                            {
                                strReportName = "rpt_Vasbill_Invoice_summary_Excel.rpt";
                            }

                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            string l_str_bill_amount = string.Empty;
                            BillingInquiry objBillingInquiry = new BillingInquiry();
                            BillingInquiryService ServiceObject = new BillingInquiryService();
                            objBillingInquiry.cmp_id = p_str_cmp_id;
                            objBillingInquiry.bill_pd_fm = p_dt_bill_from_dt;
                            objBillingInquiry.bill_pd_to = p_dt_bill_to_dt;
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
                            l_str_rptdtl = objBillingInquiry.cmp_id;
                            objEmail.EmailSubject = objBillingInquiry.cmp_id;
                            objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Bill Type: " + l_str_rpt_bill_doc_type;
                            objBillingInquiry = ServiceObject.GetConsolidateVASSummaryRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListBillRcvdDetails.ToList();
                            if ((Type == "XLS") || (Type == "XLS"))
                            {
                                if (rptSource.Count > 0)
                                {
                                    using (ReportDocument rd = new ReportDocument())
                                    {
                                        rd.Load(strRptPath);
                                        rd.SetDataSource(rptSource);
                                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                        if (Type == "PDF")
                                        {
                                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + tempFilepath + "//Generate VasBill GridSummary_" + strDateFormat + ".pdf";
                                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                            reportFileName = "Generate VasBill GridSummary Report " + DateTime.Now.ToFileTime() + ".pdf";
                                            Session["RptFileName"] = strFileName;
                                        }
                                        else if (Type == "XLS")
                                        {
                                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + tempFilepath + "//Generate VasBill GridSummary_" + strDateFormat + ".xls";
                                            rd.ExportToDisk(ExportFormatType.Excel, strFileName);
                                            reportFileName = "Generate VasBill GridSummary Report " + DateTime.Now.ToFileTime() + ".xls";
                                            Session["RptFileName"] = strFileName;
                                        }
                                    }
                                }

                            }
                            else
                            {
                                BILLING_VAS_BILL_SUMMARY_EXCEL objBillingExcel = new BILLING_VAS_BILL_SUMMARY_EXCEL();
                                objBillingInquiry.cmp_id = p_str_cmp_id;
                                objBillingInquiry.bill_pd_fm = p_dt_bill_from_dt;
                                objBillingInquiry.bill_pd_to = p_dt_bill_to_dt;
                                objBillingInquiry = ServiceObject.GetConsolidateVASSummaryRpt(objBillingInquiry);
                                List<BILLING_VAS_BILL_SUMMARY_EXCEL> li = new List<BILLING_VAS_BILL_SUMMARY_EXCEL>();
                                for (int i = 0; i < objBillingInquiry.ListBillRcvdDetails.Count; i++)
                                {
                                    BILLING_VAS_BILL_SUMMARY_EXCEL objOBInquiryExcel = new BILLING_VAS_BILL_SUMMARY_EXCEL();
                                    objOBInquiryExcel.VASId = objBillingInquiry.ListBillRcvdDetails[i].ship_doc_id;
                                    objOBInquiryExcel.VasDt = objBillingInquiry.ListBillRcvdDetails[i].ShipDate;
                                    objOBInquiryExcel.WhsId = objBillingInquiry.ListBillRcvdDetails[i].whs_id;
                                    objOBInquiryExcel.Customer = objBillingInquiry.ListBillRcvdDetails[i].cust_id;
                                    objOBInquiryExcel.CustPo = objBillingInquiry.ListBillRcvdDetails[i].po_num;
                                    objOBInquiryExcel.SoNo = objBillingInquiry.ListBillRcvdDetails[i].so_num;
                                    objOBInquiryExcel.BillAmt = objBillingInquiry.ListBillRcvdDetails[i].TotalPrice;
                                    li.Add(objOBInquiryExcel);
                                }
                                GridView gv = new GridView();
                                gv.DataSource = li;
                                gv.DataBind();
                                Session["BILL_VAS_SUMMARY"] = gv;
                                return new DownloadFileActionResult((GridView)Session["BILL_VAS_SUMMARY"], "BILL_VAS_SUMMARY" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");
                            }
                        }
                        else
                        {
                            if (Type == "PDF")
                            {
                                strReportName = "rpt_iv_vas_bill_detail.rpt";
                            }
                            else
                            {
                                strReportName = "rpt_iv_vas_bill_detail_Excel.rpt";
                            }
                            string[] str_arry_ship_doc_id = p_str_ship_doc_id.Split(',');
                            strReportName = "rpt_iv_vas_bill_detail.rpt";

                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            string l_str_bill_amount = string.Empty;
                            BillingInquiry objBillingInquiry = new BillingInquiry();
                            BillingInquiryService ServiceObject = new BillingInquiryService();
                            objBillingInquiry.cmp_id = p_str_cust_id;
                            objBillingInquiry.cust_id = p_str_cmp_id;
                            objBillingInquiry = ServiceObject.GetConsolidateVASBillEntityValue(objBillingInquiry);
                            objBillingInquiry.temp_bill_doc_id = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[0].temp_bill_doc_id;
                            foreach (string str_tim_ship_doc_id in str_arry_ship_doc_id)
                            {
                                if (str_tim_ship_doc_id == "on") // To exclude the Header checkbox
                                { }
                                else
                                {
                                    objBillingInquiry.temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id;
                                    objBillingInquiry.ship_doc_id = str_tim_ship_doc_id;
                                    objBillingInquiry = ServiceObject.SaveConsolidateBillDetails(objBillingInquiry);
                                }
                            }
                            objBillingInquiry.cust_id = p_str_cmp_id;
                            objBillingInquiry.vas_bill_pd_fm = p_dt_bill_from_dt;
                            objBillingInquiry.vas_bill_pd_to = p_dt_bill_to_dt;
                            objBillingInquiry.bill_doc_id = string.Empty;
                            objBillingInquiry.cmp_id = p_str_cust_id;
                            objBillingInquiry = ServiceObject.GenerateVASBillDetailRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListSaveVASBillDetails.ToList();
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
                            l_str_rptdtl = objBillingInquiry.cmp_id;
                            objEmail.EmailSubject = objBillingInquiry.cmp_id;
                            objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Bill Type: " + l_str_rpt_bill_doc_type;
                            if ((Type == "PDF") || (Type == "XLS"))
                            {
                                if (rptSource.Count > 0)
                                {
                                    using (ReportDocument rd = new ReportDocument())
                                    {
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                        AlocCount = objBillingInquiry.ListSaveVASBillDetails.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);
                                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                        if (Type == "PDF")
                                        {
                                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + tempFilepath + "//VAS-DETAIL-" + strDateFormat + ".pdf";
                                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                            reportFileName = "Generate VasBill Detail Report " + DateTime.Now.ToFileTime() + ".pdf";
                                            Session["RptFileName"] = strFileName;
                                        }
                                        else if (Type == "XLS")
                                        {

                                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + tempFilepath + "//VAS-DETAIL-" + strDateFormat + ".XLS";
                                            rd.ExportToDisk(ExportFormatType.Excel, strFileName);
                                            reportFileName = "Generate VasBill Detail Report " + DateTime.Now.ToFileTime() + ".XLS";
                                            Session["RptFileName"] = strFileName;
                                        }
                                    }
                                }
                            }

                            else
                            {
                                str_arry_ship_doc_id = p_str_ship_doc_id.Split(',');
                                BILLING_VAS_BILL_DETAIL_EXCEL objBillingExcel = new BILLING_VAS_BILL_DETAIL_EXCEL();
                                objBillingInquiry.cmp_id = p_str_cust_id;
                                objBillingInquiry.cust_id = p_str_cmp_id;
                                objBillingInquiry = ServiceObject.GetConsolidateVASBillEntityValue(objBillingInquiry);
                                objBillingInquiry.temp_bill_doc_id = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[0].temp_bill_doc_id;
                                foreach (string str_tim_ship_doc_id in str_arry_ship_doc_id)
                                {
                                    if (str_tim_ship_doc_id == "on") // To exclude the Header checkbox
                                    { }
                                    else
                                    {
                                        objBillingInquiry.temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id;
                                        objBillingInquiry.ship_doc_id = str_tim_ship_doc_id;
                                        objBillingInquiry = ServiceObject.SaveConsolidateBillDetails(objBillingInquiry);
                                    }
                                }
                                objBillingInquiry.cust_id = p_str_cmp_id;
                                objBillingInquiry.vas_bill_pd_fm = p_dt_bill_from_dt;
                                objBillingInquiry.vas_bill_pd_to = p_dt_bill_to_dt;
                                objBillingInquiry.bill_doc_id = string.Empty;
                                objBillingInquiry.cmp_id = p_str_cust_id;
                                objBillingInquiry = ServiceObject.GenerateVASBillDetailRpt(objBillingInquiry);
                                List<BILLING_VAS_BILL_DETAIL_EXCEL> li = new List<BILLING_VAS_BILL_DETAIL_EXCEL>();
                                for (int i = 0; i < objBillingInquiry.ListSaveVASBillDetails.Count; i++)
                                {
                                    BILLING_VAS_BILL_DETAIL_EXCEL objOBInquiryExcel = new BILLING_VAS_BILL_DETAIL_EXCEL();
                                    objOBInquiryExcel.VASId = objBillingInquiry.ListSaveVASBillDetails[i].ship_doc_id;
                                    objOBInquiryExcel.VasDt = objBillingInquiry.ListSaveVASBillDetails[i].ShipDt;
                                    objOBInquiryExcel.WhsId = objBillingInquiry.ListSaveVASBillDetails[i].whs_id;
                                    objOBInquiryExcel.Customer = objBillingInquiry.ListSaveVASBillDetails[i].cust_name;
                                    objOBInquiryExcel.CustPo = objBillingInquiry.ListSaveVASBillDetails[i].cust_ordr_num;
                                    objOBInquiryExcel.ShipTo = objBillingInquiry.ListSaveVASBillDetails[i].ship_to;
                                    objOBInquiryExcel.ServiceId = objBillingInquiry.ListSaveVASBillDetails[i].so_itm_num;
                                    objOBInquiryExcel.ServiceDesc = objBillingInquiry.ListSaveVASBillDetails[i].itm_name;
                                    objOBInquiryExcel.RateCatg = objBillingInquiry.ListSaveVASBillDetails[i].catg;
                                    objOBInquiryExcel.Units = objBillingInquiry.ListSaveVASBillDetails[i].ship_qty;
                                    objOBInquiryExcel.Rate = objBillingInquiry.ListSaveVASBillDetails[i].ship_itm_price;
                                    objOBInquiryExcel.Amount = (objOBInquiryExcel.Units * objOBInquiryExcel.Rate);
                                    li.Add(objOBInquiryExcel);
                                }
                                GridView gv = new GridView();
                                gv.DataSource = li;
                                gv.DataBind();
                                Session["BILL_VAS_DETAIL"] = gv;
                                return new DownloadFileActionResult((GridView)Session["BILL_VAS_DETAIL"], "BILL_VAS_DETAIL" + "" + DateTime.Now.ToString("MM/DD/YYYY") + ".xls");

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
                    objEmail.Reportselection = var_name;
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
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult ShowReportResult(string p_str_radio, string p_str_cmp_id, string p_str_Bill_doc_id, string p_str_Bill_type, string p_str_doc_dt_Fr, string p_str_doc_dt_To, string SelectdID, string type)
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
            string l_str_rpt_bill_type = string.Empty;
            string l_str_rpt_bill_inout_type = string.Empty;
            string l_str_rpt_instrg_req = string.Empty;
            string l_str_rpt_bill_doc_type = string.Empty;
            string l_str_rpt_bill_status = string.Empty;
            decimal l_dec_tot_amnt = 0;
            decimal l_dec_list_price = 0;
            int l_int_tot_qty = 0;
            decimal l_int_tot_ship_qty = 0;
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
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry = ServiceObject.GetBillingBillDocSTRGRpt(objBillingInquiry);
                                    if (objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.Count == 0)
                                    {
                                        int ResultCount = 1;
                                        return Json(ResultCount, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                else
                                {
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry = ServiceObject.GetBillingBillDocSTRGRpt(objBillingInquiry);
                                    if (objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.Count == 0)
                                    {
                                        int ResultCount = 1;
                                        return Json(ResultCount, JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }
                            if (l_str_rpt_bill_type.Trim() == "Cube")
                            {
                                if (l_str_rpt_instrg_req.Trim() == "Y")
                                {
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry = ServiceObject.GetBillingBillDocCubeSTRGRpt(objBillingInquiry);
                                    if (objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.Count == 0)
                                    {
                                        int ResultCount = 1;
                                        return Json(ResultCount, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                else
                                {
                                    objBillingInquiry.cmp_id = p_str_cmp_id;
                                    objBillingInquiry.Bill_doc_id = SelectdID;
                                    objBillingInquiry = ServiceObject.GetBillingBillDocCubeSTRGRpt(objBillingInquiry);
                                    if (objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.Count == 0)
                                    {
                                        int ResultCount = 1;
                                        return Json(ResultCount, JsonRequestBehavior.AllowGet);
                                    }
                                }

                            }
                            if (l_str_rpt_bill_type.Trim() == "Pallet")
                            {
                                objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                                objBillingInquiry.Bill_doc_id = SelectdID.Trim();
                                objBillingInquiry = ServiceObject.GetBillingBillDocPalletSTRGRpt(objBillingInquiry);
                                if (objBillingInquiry.ListGenBillingStrgByPalletRpt.Count == 0)
                                {
                                    int ResultCount = 1;
                                    return Json(ResultCount, JsonRequestBehavior.AllowGet);
                                }
                            }
                            if (l_str_rpt_bill_type.Trim() == "Location")
                            {
                                objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                                objBillingInquiry.Bill_doc_id = SelectdID.Trim();
                                objBillingInquiry.bill_doc_id = SelectdID.Trim();
                                objBillingInquiry = ServiceObject.STRGBillLocationRpt(objBillingInquiry);
                                if (objBillingInquiry.ListGetSTRGBillByLocRpt.Count == 0)
                                {
                                    int ResultCount = 1;
                                    return Json(ResultCount, JsonRequestBehavior.AllowGet);
                                }
                            }
                            if (l_str_rpt_bill_type.Trim() == "Pcs")
                            {
                                objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                                objBillingInquiry.Bill_doc_id = SelectdID.Trim();
                                objBillingInquiry.bill_doc_id = SelectdID.Trim();
                                objBillingInquiry = ServiceObject.STRGBillPcsRpt(objBillingInquiry);
                                if (objBillingInquiry.ListGetSTRGBillByPcsRpt.Count == 0)
                                {
                                    int ResultCount = 1;
                                    return Json(ResultCount, JsonRequestBehavior.AllowGet);
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
                            objBillingInquiry.cmp_id = p_str_cmp_id;
                            objBillingInquiry.Bill_doc_id = SelectdID;
                            objBillingInquiry = ServiceObject.GetBillingBillDocVASRpt(objBillingInquiry);
                            if (objBillingInquiry.ListBillingDocVASRpt.Count == 0)
                            {
                                int ResultCount = 1;
                                return Json(ResultCount, JsonRequestBehavior.AllowGet);
                            }
                        }

                        if (l_str_rpt_bill_doc_type == "INOUT")
                        {

                            if (l_str_rpt_bill_inout_type.Trim() == "Carton")

                            {
                                //if (l_str_rpt_instrg_req.Trim() == "Y")
                                //{
                                //    objBillingInquiry.cmp_id = p_str_cmp_id;
                                //    objBillingInquiry.Bill_doc_id = SelectdID;
                                //    objBillingInquiry = ServiceObject.GetBillingBillInoutCartonInstrgRpt(objBillingInquiry);
                                //    if (objBillingInquiry.ListBillingInoutCartonInstrgRpt.Count == 0)
                                //    {
                                //        int ResultCount = 1;
                                //        return Json(ResultCount, JsonRequestBehavior.AllowGet);
                                //    }
                                //}
                                //else
                                //{
                                objBillingInquiry.cmp_id = p_str_cmp_id;
                                objBillingInquiry.Bill_doc_id = SelectdID;
                                objBillingInquiry = ServiceObject.GetBillingBillInoutCartonRpt(objBillingInquiry);
                                if (objBillingInquiry.ListBillingInoutCartonRpt.Count == 0)
                                {
                                    int ResultCount = 1;
                                    return Json(ResultCount, JsonRequestBehavior.AllowGet);
                                }
                                //}

                            }
                            if (l_str_rpt_bill_inout_type.Trim() == "Cube")

                            {
                                //if (l_str_rpt_instrg_req.Trim() == "Y")
                                //{
                                //    objBillingInquiry.cmp_id = p_str_cmp_id;
                                //    objBillingInquiry.Bill_doc_id = SelectdID;
                                //    objBillingInquiry = ServiceObject.GetBillingBillInoutCubeInstrgRpt(objBillingInquiry);
                                //    if (objBillingInquiry.ListBillingInoutCubeInstrgRpt.Count == 0)
                                //    {
                                //        int ResultCount = 1;
                                //        return Json(ResultCount, JsonRequestBehavior.AllowGet);
                                //    }
                                //}
                                //else
                                //{
                                objBillingInquiry.cmp_id = p_str_cmp_id;
                                objBillingInquiry.Bill_doc_id = SelectdID;
                                objBillingInquiry = ServiceObject.GetBillingBillInoutCubeRpt(objBillingInquiry);
                                if (objBillingInquiry.ListBillingInoutCubeRpt.Count == 0)
                                {
                                    int ResultCount = 1;
                                    return Json(ResultCount, JsonRequestBehavior.AllowGet);
                                }
                                //}
                            }
                            if (l_str_rpt_bill_inout_type.Trim() == "Container")
                            {
                                objBillingInquiry.cmp_id = p_str_cmp_id;
                                objBillingInquiry.Bill_doc_id = SelectdID;
                                objBillingInquiry = ServiceObject.GetBillingBillDocContainerInoutRpt(objBillingInquiry);
                                if (objBillingInquiry.ListGenBillingInoutByContainerRpt.Count == 0)
                                {
                                    int ResultCount = 1;
                                    return Json(ResultCount, JsonRequestBehavior.AllowGet);
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
                        if (l_str_rpt_bill_status != "P")
                        {
                            objBillingInquiry.ReportStatus = "Invoice Not Posted";
                        }
                        if (l_str_rpt_bill_status == "P")
                        {

                            if (l_str_rpt_bill_doc_type == "NORM")
                            {
                                objBillingInquiry.cmp_id = p_str_cmp_id;
                                objBillingInquiry.Bill_doc_id = SelectdID;
                                objBillingInquiry = ServiceObject.GetBillingInvoiceRpt(objBillingInquiry);
                            }
                            else
                            {
                                objBillingInquiry.cmp_id = p_str_cmp_id;
                                objBillingInquiry.Bill_doc_id = SelectdID;
                                objBillingInquiry = ServiceObject.GetInvoiceRpt(objBillingInquiry);
                            }
                            if (objBillingInquiry.ListBillingInvoiceRpt.Count == 0)
                            {
                                int ResultCount = 1;
                                return Json(ResultCount, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }

                    if (l_str_rpt_selection == "GridSummary")
                    {
                        objBillingInquiry.cmp_id = p_str_cmp_id;
                        objBillingInquiry.Bill_doc_id = p_str_Bill_doc_id;
                        objBillingInquiry.bill_type = p_str_Bill_type;
                        objBillingInquiry.Bill_doc_dt_Fr = p_str_doc_dt_Fr;
                        objBillingInquiry.Bill_doc_dt_To = p_str_doc_dt_To;
                        objBillingInquiry = ServiceObject.GetBillingSummaryRpt(objBillingInquiry);
                        if (objBillingInquiry.ListBillingSummaryRpt.Count == 0)
                        {
                            int ResultCount = 1;
                            return Json(ResultCount, JsonRequestBehavior.AllowGet);
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
        public ActionResult ShowConsolidateReportResult(string p_str_radio, string p_str_cmp_id, string p_str_Bill_doc_id, string p_str_Bill_type, string p_str_doc_dt_Fr, string p_str_doc_dt_To, string SelectdID, string type)

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
            decimal l_dec_tot_amnt = 0;
            decimal l_dec_list_price = 0;
            int l_int_tot_qty = 0;
            decimal l_int_tot_ship_qty = 0;
            try
            {
                if (isValid)
                {



                    BillingInquiry objBillingInquiry = new BillingInquiry();
                    BillingInquiryService ServiceObject = new BillingInquiryService();
                    if (SelectdID == "" || SelectdID == "undefined")
                    {
                        l_str_rpt_bill_doc_type = p_str_Bill_type;
                    }
                    else
                    {
                        objBillingInquiry.cmp_id = p_str_cmp_id;
                        objBillingInquiry.Bill_doc_id = SelectdID;
                        objBillingInquiry = ServiceObject.GetBillingBillDocIdType(objBillingInquiry);
                        l_str_rpt_bill_doc_type = objBillingInquiry.ListBillingDocIdType[0].bill_type;
                    }

                    if (l_str_rpt_bill_doc_type == "INOUT")
                    {

                        objBillingInquiry.cmp_id = p_str_cmp_id;
                        if (SelectdID == "" || SelectdID == "undefined")
                        {
                            objBillingInquiry.Bill_doc_id = "";
                        }
                        else
                        {
                            objBillingInquiry.Bill_doc_id = SelectdID;
                        }
                        objBillingInquiry.Bill_type = p_str_Bill_type;
                        //objBillingInquiry.Bill_doc_dt_Fr = p_str_doc_dt_Fr;
                        //objBillingInquiry.Bill_doc_dt_To = p_str_doc_dt_To;
                        objBillingInquiry = ServiceObject.GetBillingBillConsolidateRpt(objBillingInquiry);
                        if (objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.Count == 0)
                        {
                            int ResultCount = 1;
                            return Json(ResultCount, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        Company objCompany = new Company();
                        CompanyService ServiceObjectCompany = new CompanyService();
                        objCompany.cust_of_cmp_id = "";
                        objCompany.cmp_id = p_str_cmp_id;
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
                        objBillingInquiry.cmp_id = p_str_cmp_id;
                        if (SelectdID == "" || SelectdID == "undefined")
                        {
                            objBillingInquiry.Bill_doc_id = "";
                        }
                        else
                        {
                            objBillingInquiry.Bill_doc_id = SelectdID;
                        }
                        objBillingInquiry.Bill_type = p_str_Bill_type;
                        objBillingInquiry.Bill_doc_dt_Fr = p_str_doc_dt_Fr;
                        objBillingInquiry.Bill_doc_dt_To = p_str_doc_dt_To;
                        // objBillingInquiry = ServiceObject.GetBillingBillDocVASRpt(objBillingInquiry);
                        objBillingInquiry = ServiceObject.GetConsolidtedVASBillRpt(objBillingInquiry);
                        if (objBillingInquiry.ListBillingDocVASRpt.Count == 0)
                        {
                            int ResultCount = 1;
                            return Json(ResultCount, JsonRequestBehavior.AllowGet);
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
        public ActionResult ShowConsolidateReportEmail(string p_str_radio, string p_str_cmp_id, string p_str_Bill_doc_id, string p_str_Bill_type, string p_str_doc_dt_Fr, string p_str_doc_dt_To, string SelectdID, string type)

        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/dd/yyyy");
            string strToDate = DateTime.Now.ToString("MM/dd/yyyy");
            string l_str_rpt_selection = string.Empty;
            objCompany.user_id = Session["UserID"].ToString().Trim();

            l_str_rpt_selection = p_str_radio;

            string l_str_status = string.Empty;
            string l_str_rpt_bill_type = string.Empty;
            string l_str_rpt_bill_inout_type = string.Empty;
            string l_str_rpt_instrg_req = string.Empty;
            string l_str_rpt_bill_doc_type = string.Empty;
            string l_str_rpt_bill_status = string.Empty;
            string strDateFormat = string.Empty;
            string strFileName = string.Empty;
            string reportFileName = string.Empty;
            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();
            objCompany.cmp_id = p_str_cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            objBillingInquiry.LstCmpName = objCompany.LstCmpName;
            l_str_tmp_name = objBillingInquiry.LstCmpName[0].cmp_name.ToString().Trim();

            try
            {
                if (isValid)
                {
                    if (SelectdID == "" || SelectdID == "undefined")
                    {
                        l_str_rpt_bill_doc_type = p_str_Bill_type;
                    }
                    else
                    {
                        objBillingInquiry.cmp_id = p_str_cmp_id;
                        objBillingInquiry.Bill_doc_id = SelectdID;
                        objBillingInquiry = ServiceObject.GetBillingInoutType(objBillingInquiry);
                        l_str_rpt_bill_inout_type = objBillingInquiry.ListBillingInoutType[0].bill_inout_type;
                        objBillingInquiry.bill_type = "BY" + "-" + l_str_rpt_bill_inout_type;
                        objBillingInquiry = ServiceObject.GetBillingBillDocIdType(objBillingInquiry);
                        l_str_rpt_bill_doc_type = objBillingInquiry.ListBillingDocIdType[0].bill_type;
                    }

                    if (l_str_rpt_bill_doc_type == "INOUT")
                    {
                        if (type == "PDF")
                        {
                            strReportName = "rpt_inout_bill_doc_consolidated.rpt";
                        }
                        else
                        {
                            strReportName = "rpt_inout_bill_doc_consolidated_Excel.rpt";
                        }

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        objBillingInquiry.cmp_id = p_str_cmp_id;
                        if (SelectdID == "" || SelectdID == "undefined")
                        {
                            objBillingInquiry.Bill_doc_id = "";
                        }
                        else
                        {
                            objBillingInquiry.Bill_doc_id = SelectdID;
                        }
                        objBillingInquiry.Bill_type = p_str_Bill_type;
                        objBillingInquiry.Bill_doc_dt_Fr = p_str_doc_dt_Fr;
                        objBillingInquiry.Bill_doc_dt_To = p_str_doc_dt_To;
                        objBillingInquiry.bill_amt = 0;
                        objBillingInquiry.ship_ctns = 0;
                        objBillingInquiry = ServiceObject.GetBillingBillConsolidateRpt(objBillingInquiry);
                        for (int i = 0; i < objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.Count; i++)
                        {
                            objBillingInquiry.bill_amt = objBillingInquiry.bill_amt + (objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].ship_ctns * objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].so_itm_price);
                            objBillingInquiry.ship_ctns = objBillingInquiry.ship_ctns + objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].ship_ctns;
                            objBillingInquiry.RcvdDate = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt[i].rcvd_dt.ToString("MM/dd/yyyy");
                        }
                        var rptSource = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                                AlocCount = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                rd.SetParameterValue("fml_rep_type", objBillingInquiry.bill_type);
                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                l_str_rptdtl = objBillingInquiry.cmp_id + "_" + "Consolidate IN-OUT Bill";
                                objEmail.EmailSubject = objBillingInquiry.cmp_id + "-" + "Consolidate IN-OUT Bill " + "|" + " " + "Invoice:  " + "$" + objBillingInquiry.bill_amt + "|" + " " + "Received Date: " + objBillingInquiry.RcvdDate;
                                objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Bill Type: " + l_str_rpt_bill_doc_type + "\n" + "Invoice Amnt: " + "$" + objBillingInquiry.bill_amt + "\n" + "Received Date: " + " " + " " + objBillingInquiry.RcvdDate + "\n" + "Total Cartons: " + objBillingInquiry.ship_ctns;

                                if (type == "PDF")
                                {

                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                    reportFileName = l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                }
                                else if (type == "Word")
                                {

                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                    reportFileName = l_str_rptdtl + "_" + strDateFormat + ".docx";
                                }
                                else if (type == "XLS")
                                {

                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".xls";
                                    rd.ExportToDisk(ExportFormatType.Excel, strFileName);
                                    reportFileName = l_str_rptdtl + "_" + strDateFormat + ".xls";
                                }
                            }
                        }
                        Session["RptFileName"] = strFileName;
                    }
                    else
                    {
                        Company objCompany = new Company();
                        CompanyService ServiceObjectCompany = new CompanyService();
                        objCompany.cust_of_cmp_id = "";
                        objCompany.cmp_id = p_str_cmp_id;
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
                        if (type == "PDF")
                        {
                            strReportName = "rpt_va_bill_doc_consolidate.rpt";
                        }
                        else
                        {
                            strReportName = "rpt_va_bill_doc_consolidate_Excel.rpt";
                        }

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        objBillingInquiry.cmp_id = p_str_cmp_id;
                        if (SelectdID == "" || SelectdID == "undefined")
                        {
                            objBillingInquiry.Bill_doc_id = "";
                        }
                        else
                        {
                            objBillingInquiry.Bill_doc_id = SelectdID;
                        }
                        objBillingInquiry.Bill_type = p_str_Bill_type;
                        objBillingInquiry.Bill_doc_dt_Fr = p_str_doc_dt_Fr;
                        objBillingInquiry.Bill_doc_dt_To = p_str_doc_dt_To;
                        objBillingInquiry.bill_amt = 0;
                        objBillingInquiry.ship_ctns = 0;
                        objBillingInquiry = ServiceObject.GetConsolidtedVASBillRpt(objBillingInquiry);
                        for (int i = 0; i < objBillingInquiry.ListBillingDocVASRpt.Count; i++)
                        {
                            objBillingInquiry.bill_amt = objBillingInquiry.bill_amt + (objBillingInquiry.ListBillingDocVASRpt[i].ship_ctns * objBillingInquiry.ListBillingDocVASRpt[i].so_itm_price);
                            objBillingInquiry.ship_ctns = objBillingInquiry.ship_ctns + objBillingInquiry.ListBillingDocVASRpt[i].ship_ctns;
                            objBillingInquiry.RcvdDate = objBillingInquiry.ListBillingDocVASRpt[i].rcvd_dt.ToString("MM/dd/yyyy");
                        }
                        var rptSource = objBillingInquiry.ListBillingDocVASRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                                AlocCount = objBillingInquiry.ListBillingDocVASRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                l_str_rptdtl = objBillingInquiry.cmp_id + "_" + "Consolidate IN-OUT Bill" + "_" + objBillingInquiry.bill_doc_id;
                                objEmail.EmailSubject = objBillingInquiry.cmp_id + "-" + "Consolidate IN-OUT Bill " + "|" + " " + "Invoice#: " + objBillingInquiry.bill_doc_id + "|" + " " + "Invoice:  " + "$" + objBillingInquiry.bill_amt + "|" + " " + "Received Date: " + objBillingInquiry.RcvdDate;
                                objEmail.EmailMessage = "Hi All," + "\n" + objEmail.EmailMessageContent + "\n" + "CmpId: " + " " + " " + objBillingInquiry.cmp_id + "-" + " " + " " + l_str_tmp_name + "\n" + "Bill Type: " + l_str_rpt_bill_doc_type + "\n" + "Invoice Amnt: " + "$" + objBillingInquiry.bill_amt + "\n" + "Received Date: " + " " + " " + objBillingInquiry.RcvdDate + "\n" + "Total Cartons: " + objBillingInquiry.ship_ctns;
                                if (type == "PDF")
                                {

                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                    reportFileName = l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                }
                                else if (type == "Word")
                                {

                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".pdf";
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                    reportFileName = l_str_rptdtl + "_" + strDateFormat + ".docx";
                                }
                                else if (type == "XLS")
                                {

                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "_" + strDateFormat + ".xls";
                                    rd.ExportToDisk(ExportFormatType.Excel, strFileName);
                                    reportFileName = l_str_rptdtl + "_" + strDateFormat + ".xls";
                                }
                            }
                        }
                        Session["RptFileName"] = strFileName;
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

        public FileStreamResult XlsInoutBillByCubeRoundComp(string p_str_cmp_id, string p_str_bill_doc_id)
        {


            string tempFileName = string.Empty;
            string l_str_file_name = string.Empty;
            string strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
            if (!Directory.Exists(strOutputpath))
            {
                Directory.CreateDirectory(strOutputpath);
            }
            BillingInquiryService objService = new BillingInquiryService();
            DataTable dtBill = new DataTable();
            dtBill = objService.GetBillByCubeList(p_str_cmp_id, p_str_bill_doc_id);
            l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-INV-INBOUND-BILL-" + p_str_bill_doc_id + strDateFormat + ".xlsx";

            tempFileName = strOutputpath + l_str_file_name;

            if (System.IO.File.Exists(tempFileName))
                System.IO.File.Delete(tempFileName);
            xls_bl_inout_by_cube_cmpr mxcel1 = new xls_bl_inout_by_cube_cmpr(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "BL_INOUT_BILL_BY_CUBE_ROUNDED.xlsx");
            mxcel1.PopulateHeader(p_str_cmp_id, string.Empty);

            mxcel1.PopulateData(dtBill, true);
            mxcel1.SaveAs(tempFileName);
            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
            return File(fs, "application / xlsx", l_str_file_name);

        }
        public ActionResult XlsInoutBillByCubeRoundCompEmail(string p_str_cmp_id, string p_str_bill_doc_id)

        {
            /*
            Response.Write("<H2>Report Under Construction!</H2>");
            */
            return null;

        }

        public ActionResult ViewXLSBill(string p_str_cmp_id, string p_str_bill_doc_id, string p_str_radio)
        {
            string l_str_rpt_selection = p_str_radio;
            string l_str_bill_type = string.Empty;
            string l_str_bill_by = string.Empty;
            string l_str_strg_bill_by = string.Empty;
            string tempFileName = string.Empty;
            string l_str_file_name = string.Empty;
            string l_str_cust_name = string.Empty;
            string msg = "";
            string jsonErrorCode = "0";
            string strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
            BillingInquiryService objService = new BillingInquiryService();
            DataTable dtBill = new DataTable();
            l_str_bill_type = objService.GetBillType(p_str_bill_doc_id);
            string l_str_bill_date = string.Empty;
            if (!Directory.Exists(strOutputpath))
            {
                Directory.CreateDirectory(strOutputpath);
            }

            l_str_bill_by = objService.GetBillBy(p_str_cmp_id, "INOUT");
            l_str_strg_bill_by = objService.GetBillBy(p_str_cmp_id, "STRG");
            try
            {
                if (l_str_rpt_selection == "BILL-DOC-SMRY")
                {
                    if (l_str_bill_type.ToUpper() == "STRG")
                    {
                        DataTable dtBlHdr = new DataTable();
                        dtBill = objService.fnGetStrgBillByItmSmry(p_str_cmp_id, p_str_bill_doc_id, ref dtBlHdr);
                        if (dtBill.Rows.Count > 1)
                        {
                            l_str_bill_date = dtBill.Rows[0]["bill_doc_dt"].ToString();
                            l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-STRG-BILL-DOCUMENT-" + p_str_bill_doc_id + "-" + strDateFormat + ".xlsx";
                            tempFileName = strOutputpath + l_str_file_name;
                            if (System.IO.File.Exists(tempFileName))
                                System.IO.File.Delete(tempFileName);

                            if (l_str_strg_bill_by.ToUpper() == "CUBE")
                            {
                                xls_bl_strg_bill_by_style_smry mxcel1 = new xls_bl_strg_bill_by_style_smry(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "BL_STRG_BILL_BY_CUBE.xlsx", "CUBE");
                                mxcel1.PopulateHeader(p_str_cmp_id, p_str_bill_doc_id, l_str_bill_date, "CUBE", Math.Round(Convert.ToDecimal(dtBlHdr.Rows[0]["bill_amt"]), 2));
                                mxcel1.PopulateData(dtBill, true);
                                mxcel1.SaveAs(tempFileName);
                            }
                            else if (l_str_strg_bill_by.ToUpper() == "CARTON")
                            {
                                xls_bl_strg_bill_by_style_smry mxcel1 = new xls_bl_strg_bill_by_style_smry(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "BL_STRG_BILL_BY_CTN.xlsx", "CARTON");
                                mxcel1.PopulateHeader(p_str_cmp_id, p_str_bill_doc_id, l_str_bill_date, "CARTON", Math.Round(Convert.ToDecimal(dtBlHdr.Rows[0]["bill_amt"]), 2));
                                mxcel1.PopulateData(dtBill, true);
                                mxcel1.SaveAs(tempFileName);
                            }
                        }
                        FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                        return File(fs, "application / xlsx", l_str_file_name);
                    }
                    else
                    {

                    }
                }

                else if (l_str_rpt_selection == "BillDocument")
                {
                    switch (l_str_bill_type.ToUpper())
                    {
                        case "INOUT":
                            {
                                DataTable dtBlHdr = new DataTable();
                                dtBill = objService.GetBillDocInoutRpt(p_str_cmp_id, p_str_bill_doc_id, ref dtBlHdr);

                                if (dtBill.Rows.Count > 0)
                                {
                                    l_str_bill_date = dtBill.Rows[0]["bill_dt"].ToString();
                                    switch (l_str_bill_by.ToUpper())
                                    {
                                        case "CUBE":
                                            {
                                                l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-INBOUND-BILL-DOCUMENT-" + p_str_bill_doc_id + "-" + strDateFormat + ".xlsx";
                                                tempFileName = strOutputpath + l_str_file_name;
                                                if (System.IO.File.Exists(tempFileName))
                                                    System.IO.File.Delete(tempFileName);
                                                xls_bl_inout_bill_by_cube mxcel1 = new xls_bl_inout_bill_by_cube(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "BL_INOUT_BILL_BY_CUBE.xlsx");
                                                mxcel1.PopulateHeader(p_str_cmp_id, p_str_bill_doc_id, l_str_bill_date, "CUBE", Math.Round(Convert.ToDecimal(dtBlHdr.Rows[0]["bill_amt"]), 2));
                                                mxcel1.PopulateData(dtBill, true);
                                                mxcel1.SaveAs(tempFileName);
                                                break;
                                            }

                                        case "CARTON":
                                            {
                                                l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-INBOUND-BILL-DOCUMENT-" + p_str_bill_doc_id + "-" + strDateFormat + ".xlsx";
                                                tempFileName = strOutputpath + l_str_file_name;
                                                if (System.IO.File.Exists(tempFileName))
                                                    System.IO.File.Delete(tempFileName);
                                                xls_bl_inout_bill_by_ctn mxcel1 = new xls_bl_inout_bill_by_ctn(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "BL_INOUT_BILL_BY_CTN.xlsx");
                                                mxcel1.PopulateHeader(p_str_cmp_id, p_str_bill_doc_id, l_str_bill_date, "CARTON", Math.Round(Convert.ToDecimal(dtBlHdr.Rows[0]["bill_amt"]), 2));
                                                mxcel1.PopulateData(dtBill, true);
                                                mxcel1.SaveAs(tempFileName);
                                                break;
                                            }
                                    }

                                }
                                break;
                            }
                        case "STRG":
                            {
                                DataTable dtBlHdr = new DataTable();
                                dtBill = objService.GetBillDocStrgRpt(p_str_cmp_id, p_str_bill_doc_id, ref dtBlHdr);
                                if (dtBill.Rows.Count > 1)
                                {
                                    l_str_bill_date = dtBill.Rows[0]["bill_doc_dt"].ToString();
                                    l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-STRG-BILL-DOCUMENT-" + p_str_bill_doc_id + "-" + strDateFormat + ".xlsx";
                                    tempFileName = strOutputpath + l_str_file_name;
                                    if (System.IO.File.Exists(tempFileName))
                                        System.IO.File.Delete(tempFileName);

                                    if (l_str_strg_bill_by.ToUpper() == "CUBE")
                                    {
                                        xls_bl_strg_bill mxcel1 = new xls_bl_strg_bill(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "BL_STRG_BILL_BY_CUBE.xlsx", "CUBE");
                                        mxcel1.PopulateHeader(p_str_cmp_id, p_str_bill_doc_id, l_str_bill_date, "CUBE", Math.Round(Convert.ToDecimal(dtBlHdr.Rows[0]["bill_amt"]), 2));
                                        mxcel1.PopulateData(dtBill, true);
                                        mxcel1.SaveAs(tempFileName);
                                    }
                                    else if (l_str_strg_bill_by.ToUpper() == "CARTON")
                                    {
                                        xls_bl_strg_bill mxcel1 = new xls_bl_strg_bill(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "BL_STRG_BILL_BY_CTN.xlsx", "CARTON");
                                        mxcel1.PopulateHeader(p_str_cmp_id, p_str_bill_doc_id, l_str_bill_date, "CARTON", Math.Round(Convert.ToDecimal(dtBlHdr.Rows[0]["bill_amt"]), 2));
                                        mxcel1.PopulateData(dtBill, true);
                                        mxcel1.SaveAs(tempFileName);
                                    }
                                }
                                break;
                            }


                        case "NORM":
                            {
                                DataTable dtBlHdr = new DataTable();
                                dtBill = objService.GetBillDocVASRpt(p_str_cmp_id, p_str_bill_doc_id, ref dtBlHdr);
                                l_str_bill_date = dtBill.Rows[0]["bill_doc_dt"].ToString();
                                l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-VAS-BILL-DOCUMENT-" + p_str_bill_doc_id + "-" + strDateFormat + ".xlsx";
                                tempFileName = strOutputpath + l_str_file_name;
                                if (System.IO.File.Exists(tempFileName))
                                    System.IO.File.Delete(tempFileName);
                                xls_bl_vas_bill mxcel1 = new xls_bl_vas_bill(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "BL_VAS_BILL.xlsx");
                                mxcel1.PopulateHeader(p_str_cmp_id, p_str_bill_doc_id, l_str_bill_date, "VAS", Math.Round(Convert.ToDecimal(dtBlHdr.Rows[0]["bill_amt"]), 2));
                                mxcel1.PopulateData(dtBill, Math.Round(Convert.ToDecimal(dtBlHdr.Rows[0]["bill_amt"]), 2), true);
                                mxcel1.SaveAs(tempFileName);
                                break;
                            }
                    }
                    FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                    return File(fs, "application / xlsx", l_str_file_name);
                }

                else if (l_str_rpt_selection == "BillInvoice")
                {
                    switch (l_str_bill_type.ToUpper())
                    {
                        case "INOUT":
                            {
                                switch (l_str_bill_by.ToUpper())
                                {
                                    case "CUBE":
                                        {
                                            DataTable dtBlHdr = new DataTable();
                                            dtBill = objService.GetBillDocInoutRpt(p_str_cmp_id, p_str_bill_doc_id, ref dtBlHdr);
                                            var dataRows = dtBill.Rows;
                                            var dataColumns = dtBill.Columns;
                                            l_str_bill_date = dtBill.Rows[0]["bill_dt"].ToString();
                                            l_str_cust_name = dtBill.Rows[0]["cust_name"].ToString();
                                            l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-INBOUND-BILL-INVOICE-DOCUMENT-" + p_str_bill_doc_id + "-" + strDateFormat + ".xlsx";
                                            tempFileName = strOutputpath + l_str_file_name;
                                            if (System.IO.File.Exists(tempFileName))
                                                System.IO.File.Delete(tempFileName);
                                            xls_bl_invoice_inout_bill_by_cube mxcel = new xls_bl_invoice_inout_bill_by_cube(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "BL_INVOICE_INOUT_BILL_BY_CUBE.xlsx");
                                            mxcel.PopulateHeader(p_str_cmp_id, p_str_bill_doc_id, l_str_bill_date, "CUBE", Math.Round(Convert.ToDecimal(dtBlHdr.Rows[0]["bill_amt"]), 2), l_str_cust_name);
                                            mxcel.PopulateData(dtBill, true);
                                            mxcel.SaveAs(tempFileName);
                                            break;
                                        }

                                    case "CARTON":
                                        {
                                            dtBill = objService.GetBillInvoiceInoutRpt(p_str_cmp_id, p_str_bill_doc_id);
                                            var dataRows = dtBill.Rows;
                                            var dataColumns = dtBill.Columns;
                                            DataRow dr = dataRows[0];
                                            l_str_bill_date = dr["bill_as_of_date"].ToString();
                                            l_str_cust_name = dr["cust_name"].ToString();
                                            l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-INBOUND-BILL-INVOICE-DOCUMENT-" + p_str_bill_doc_id + "-" + strDateFormat + ".xlsx";
                                            tempFileName = strOutputpath + l_str_file_name;
                                            if (System.IO.File.Exists(tempFileName))
                                                System.IO.File.Delete(tempFileName);
                                            xls_bl_invoice_inout_bill_by_ctn mxcel1 = new xls_bl_invoice_inout_bill_by_ctn(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "BL_INVOICE_INOUT_BILL_BY_CTN.xlsx");
                                            mxcel1.PopulateHeader(p_str_cmp_id, p_str_bill_doc_id, l_str_bill_date, "CARTON", Math.Round(Convert.ToDecimal(dr["bill_amt"]), 2), l_str_cust_name);
                                            mxcel1.PopulateData(dtBill, true);
                                            mxcel1.SaveAs(tempFileName);
                                            break;
                                        }
                                }
                                break;
                            }

                        case "STRG":
                            {

                                dtBill = objService.GetBillInvoiceStorageRpt(p_str_cmp_id, p_str_bill_doc_id);
                                var dataRows = dtBill.Rows;
                                var dataColumns = dtBill.Columns;
                                if (dtBill.Rows.Count > 0)
                                {
                                    DataRow dr = dataRows[0];
                                    l_str_bill_date = dr["bill_dt"].ToString();
                                    l_str_cust_name = dr["cust_name"].ToString();
                                    l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-STRG-BILL-INVOICE-DOCUMENT-" + p_str_bill_doc_id + "-" + strDateFormat + ".xlsx";
                                    tempFileName = strOutputpath + l_str_file_name;
                                    if (System.IO.File.Exists(tempFileName))
                                        System.IO.File.Delete(tempFileName);

                                    if (l_str_strg_bill_by.ToUpper() == "CUBE")
                                    {
                                        xls_bl_invoice_strg_bill mxcel1 = new xls_bl_invoice_strg_bill(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "BL_INVOICE_STRG_BILL_BY_CUBE.xlsx", "CUBE");
                                        mxcel1.PopulateHeader(p_str_cmp_id, p_str_bill_doc_id, l_str_bill_date, "CUBE", l_str_cust_name);
                                        mxcel1.PopulateData(dtBill, true);
                                        mxcel1.SaveAs(tempFileName);
                                    }
                                    else if (l_str_strg_bill_by.ToUpper() == "CARTON")
                                    {
                                        xls_bl_invoice_strg_bill mxcel1 = new xls_bl_invoice_strg_bill(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "BL_INVOICE_STRG_BILL_BY_CTN.xlsx", "CARTON");
                                        mxcel1.PopulateHeader(p_str_cmp_id, p_str_bill_doc_id, l_str_bill_date, "CARTON", l_str_cust_name);
                                        mxcel1.PopulateData(dtBill, true);
                                        mxcel1.SaveAs(tempFileName);
                                    }
                                }
                                break;
                            }


                        case "NORM":
                            {
                                string l_str_bill_pd_fm = string.Empty;
                                string l_str_bill_pd_to = string.Empty;

                                dtBill = objService.GetBillInvoiceVASRpt(p_str_cmp_id, p_str_bill_doc_id);
                                var dataRows = dtBill.Rows;
                                var dataColumns = dtBill.Columns;
                                DataRow dr = dataRows[0];
                                l_str_bill_date = dr["bill_as_of_date"].ToString();
                                l_str_cust_name = dr["cust_name"].ToString();
                                l_str_bill_pd_fm = dr["bill_pd_fm"].ToString();
                                l_str_bill_pd_to = dr["bill_pd_to"].ToString();
                                l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-VAS-INVOICE-BILL-DOCUMENT-" + p_str_bill_doc_id + "-" + strDateFormat + ".xlsx";
                                tempFileName = strOutputpath + l_str_file_name;
                                if (System.IO.File.Exists(tempFileName))
                                    System.IO.File.Delete(tempFileName);
                                xls_bl_invoice_vas_bill mxcel1 = new xls_bl_invoice_vas_bill(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "BL_INVOICE_VAS_BILL.xlsx", "VAS");
                                mxcel1.PopulateHeader(p_str_cmp_id, p_str_bill_doc_id, l_str_bill_date, "VAS", l_str_cust_name, l_str_bill_pd_fm, l_str_bill_pd_to);
                                mxcel1.PopulateData(dtBill, true);
                                mxcel1.SaveAs(tempFileName);
                                break;
                            }
                    }
                    FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                    return File(fs, "application / xlsx", l_str_file_name);
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
