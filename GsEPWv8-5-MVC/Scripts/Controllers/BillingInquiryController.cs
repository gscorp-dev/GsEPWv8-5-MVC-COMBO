using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GsEPWv8_4_MVC.Business.Implementation;
using GsEPWv8_4_MVC.Core.Entity;
using GsEPWv8_4_MVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace GsEPWv8_4_MVC.Controllers
{
    public class BillingInquiryController : Controller
    {
        // GET: BillingInquiry
        decimal l_int_bill_amount = 0;
        public string EmailSub = string.Empty;
        public string EmailMsg = string.Empty;

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
                //  CR_3PL_MVC_COMMON_2018_0324_001
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
                if ( FullFillType == null)
                {
                    //objCompany.cmp_id = l_str_cmp_id;
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objBillingInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    DateTime date = DateTime.Now.AddMonths(-12);
                    l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");
                    objBillingInquiry.Bill_doc_dt_Fr = l_str_fm_dt;
                    objBillingInquiry.Bill_doc_dt_To = DateTime.Now.ToString("MM/dd/yyyy");
                    objBillingInquiry = ServiceObject.GetBillingInquiryDetails(objBillingInquiry);
                }
                else  if (FullFillType != null)
                {
                   
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objBillingInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    objBillingInquiry.cmp_id = cmp;
                    objBillingInquiry.Bill_doc_id = "";
                    objBillingInquiry.Bill_type = "";
                   
                    objBillingInquiry.Bill_doc_dt_Fr = DateTime.Now.AddDays(Common.clsGlobal.DispDateFrom).ToString("MM/dd/yyyy"); ;
                    objBillingInquiry.Bill_doc_dt_To = DateTime.Now.ToString("MM/dd/yyyy"); ; ; ;
                 
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
            else { 
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
        public ActionResult LoadGenerateVASBillDetail(string p_str_cmpid, string p_str_vasid,string p_str_vasdt, string p_str_screentitle)
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
        public ActionResult LoadGenerateInoutBillDetails(string cmpid,string p_str_screen_title)
        {
            string l_str_fm_dt = string.Empty;
            DateTime date = DateTime.Now;
            l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");
            BillingInquiry objBillingInquiry = new BillingInquiry();
            BillingInquiryService ServiceObject = new BillingInquiryService();
            objBillingInquiry.cmp_id = cmpid;    
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
        //CR-3PL_MVC_IB_2018_0219_004 
        public ActionResult GenerateIBInoutBillDetails(string p_str_cmpid , string p_str_ib_doc_id, string p_str_screen_title)
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
              
                DateTime l_dt_rcvd_dt ;

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
                    if (objBillingInquiry.Check_existing_bill_doc_id == "" || objBillingInquiry.Check_existing_bill_doc_id == null)
                    {
                        objBillingInquiry.bill_doc_id = "";
                    }
                    else
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
        public ActionResult GetSearchVASBillInqDetails(string p_str_cmp_id, string p_str_cust_id, string p_str_print_dt, string p_str_vas_bill_pd_fm, string p_str_vas_bill_pd_to, string p_str_vas_id,string p_str_vas_dt ,string p_str_screentitle)
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
        public ActionResult SaveGenerateStrgBillDetails(string p_str_cmp_id, string p_str_cust_id, string p_str_print_dt, string p_str_as_of_date,string p_str_bill_pd_fm)
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
                    if(l_str_rpt_bill_type == "Carton")
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

               
                return Json(new { data1 = objBillingInquiry.Success , data2 = objBillingInquiry.bill_doc_id }, JsonRequestBehavior.AllowGet);
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
        public ActionResult SaveGenerateInoutBillDetails(string p_str_cmp_id, string p_str_cust_id,String p_str_print_dt, string p_str_inout_bill_pd_fm, string p_str_inout_bill_pd_to , string p_str_ib_doc_id)
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
                }
                //l_str_bill_doc_id = objBillingInquiry.ListCheckExistingInOutBillDocId[0].bill_doc_id.ToString().Trim();
                objBillingInquiry.cmp_id = p_str_cust_id;
                objBillingInquiry = ServiceObject.GetBillingInoutType(objBillingInquiry);
                l_str_rpt_bill_type = objBillingInquiry.ListBillingInoutType[0].bill_inout_type;
                l_str_init_strg_rate = objBillingInquiry.ListBillingInoutType[0].init_strg_rt_req;
                if(l_str_init_strg_rate =="Y")
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


                return Json(new {data1= objBillingInquiry.Success , data2= objBillingInquiry.bill_doc_id } , JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public ActionResult SaveGenerateVASBillDetails(string p_str_cmp_id, string p_str_cust_id, String p_str_print_dt, string p_str_vas_bill_pd_fm, string p_str_vas_bill_pd_to,string p_str_vas_dt,string p_str_vas_id)
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

        public ActionResult SaveStrgBillIDDetails(string p_str_cmp_id, string p_str_cust_id, string p_str_print_dt,string p_str_as_of_date , string p_str_bill_doc_id)
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
                DateTime l_dt_rcvd_dt ;

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
                    else {
                        objBillingInquiry.Bill_doc_id = p_str_Bill_doc_id;
                    }
                    //objBillingInquiry.Bill_type = p_str_Bill_type;
                    //objBillingInquiry.Bill_doc_dt_Fr = p_str_doc_dt_Fr;
                    //objBillingInquiry.Bill_doc_dt_To = p_str_doc_dt_To;
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
        public ActionResult GetRptDtls(string SelectedIDs, string p_str_cmp_id, string p_str_radio , string p_str_Bill_type)

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
        public ActionResult GenerateShowReport(string p_str_cmp_id, string p_str_bill_doc_id, string p_str_bill_doc_type , string p_str_rpt_status)
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
            try {
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
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
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
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
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
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
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
                    if (l_str_rpt_bill_type.Trim() == "Location")

                    {
                        strReportName = "rpt_st_bill_doc_location.rpt";
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                        objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                        objBillingInquiry.bill_doc_id = p_str_bill_doc_id.Trim();
                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        objBillingInquiry = ServiceObject.STRGBillLocationRpt(objBillingInquiry);
                        var rptSource = objBillingInquiry.ListGetSTRGBillByLocRpt.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objBillingInquiry.ListGetSTRGBillByLocRpt.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");


                    }
                    if (l_str_rpt_bill_type.Trim() == "Pcs")

                    {
                        strReportName = "rpt_st_bill_doc_Pcs_item.rpt";
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                        objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                        objBillingInquiry.bill_doc_id = p_str_bill_doc_id.Trim();
                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        objBillingInquiry = ServiceObject.STRGBillPcsRpt(objBillingInquiry);
                        var rptSource = objBillingInquiry.ListGetSTRGBillByPcsRpt.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objBillingInquiry.ListGetSTRGBillByPcsRpt.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                        rd.SetDataSource(rptSource);
                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
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
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
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
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
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
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
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
                    if (l_str_rpt_bill_inout_type.Trim() == "Container")

                    {
                            strReportName = "rpt_inout_bill_by_Container.rpt";
                            ReportDocument rd = new ReportDocument();
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id.Trim();
                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                        objBillingInquiry = ServiceObject.GetBillingBillDocContainerInoutRpt(objBillingInquiry);
                            var rptSource = objBillingInquiry.ListGenBillingInoutByContainerRpt.ToList();
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
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                        objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0421_001 
                        objBillingInquiry = ServiceObject.GetBillingBillDocVASRpt(objBillingInquiry);
                        var rptSource = objBillingInquiry.ListBillingDocVASRpt.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objBillingInquiry.ListBillingDocVASRpt.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                        else
                        {
                            strReportName = "rpt_va_bill_doc.rpt";
                            ReportDocument rd = new ReportDocument();
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0421_001 
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
                    }
                    else
                    {
                    strReportName = "rpt_va_bill_doc.rpt";
                        ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                        objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0421_001 
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


                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                jsonErrorCode = "-2";
            }

            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult ShowReport(string SelectdID, string p_str_cmp_id, string p_str_bill_doc_id, string p_str_bill_doc_type, string p_str_rpt_status)
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
            int     l_int_tot_qty = 0;
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
                                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                                        AlocCount = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);
                                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG Carton");
                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG Carton");
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
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
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
                                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG Carton");
                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG Carton");
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
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 


                                    if (type == "PDF")
                                    {
                                        var rptSource = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.ToList();
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                     
                                        AlocCount = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);
                                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG Cube");
                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG  Cube");
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
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 


                                    if (type == "PDF")
                                    {
                                        var rptSource = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.ToList();
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                      
                                        AlocCount = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);
                                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG Cube");
                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG Cube");
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
                          //  CR_3PL_MVC_BL_2018_0226_001 – Add Starage Bill By Pallet Report
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
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG Pallet");
                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG Pallet");
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
                            if (l_str_rpt_bill_type.Trim() == "Location")
                            {

                                //strReportName = "rpt_st_bill_doc_bycube.rpt";
                                //ReportDocument rd = new ReportDocument();
                                //string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                //objBillingInquiry.cmp_id = p_str_cmp_id;
                                //objBillingInquiry.Bill_doc_id = SelectdID;
                                //objBillingInquiry = ServiceObject.GenerateSTRGBillByPallet(objBillingInquiry);



                                strReportName = "rpt_st_bill_doc_location.rpt";
                                ReportDocument rd = new ReportDocument();
                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                                objBillingInquiry.Bill_doc_id = SelectdID.Trim();
                                objBillingInquiry.bill_doc_id = SelectdID.Trim();
                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                objBillingInquiry = ServiceObject.STRGBillLocationRpt(objBillingInquiry);


                                if (type == "PDF")
                                {
                                    var rptSource = objBillingInquiry.ListGetSTRGBillByLocRpt.ToList();
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objBillingInquiry.ListGetSTRGBillByLocRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                               
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG Location");
                                }
                                else if (type == "Word")
                                {
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG Location");
                                }
                                else
                                if (type == "Excel")
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
                            }
                            if (l_str_rpt_bill_type.Trim() == "Pcs")
                            {

                                strReportName = "rpt_st_bill_doc_Pcs_item.rpt";
                                ReportDocument rd = new ReportDocument();
                                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                                objBillingInquiry.cmp_id = p_str_cmp_id.Trim();
                                objBillingInquiry.Bill_doc_id = SelectdID.Trim();
                                objBillingInquiry.bill_doc_id = SelectdID.Trim();
                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                objBillingInquiry = ServiceObject.STRGBillPcsRpt(objBillingInquiry);
                                
                                if (type == "PDF")
                                {
                                    var rptSource = objBillingInquiry.ListGetSTRGBillByPcsRpt.ToList();
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objBillingInquiry.ListGetSTRGBillByPcsRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG Pcs");
                                
                                }
                                else if (type == "Word")
                                {
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document STRG Pcs");
                                }
                                else
                                if (type == "Excel")
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
                            strReportName = "rpt_va_bill_doc.rpt";
                            }                                
                            ReportDocument rd = new ReportDocument();
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id;
                            objBillingInquiry.Bill_doc_id = SelectdID;
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0421_001 
                            objBillingInquiry = ServiceObject.GetBillingBillDocVASRpt(objBillingInquiry);
                           

                            if (type == "PDF")
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
                                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);

                                    }
                                }
                                else
                                {
                                    rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                }
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document Vas");
                            }
                            else if (type == "Word")
                            {
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document Vas");
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
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0421_001 

                                    objBillingInquiry = ServiceObject.GetBillingBillInoutCartonInstrgRpt(objBillingInquiry);
                                   
                                    if (type == "PDF")
                                    {
                                        var rptSource = objBillingInquiry.ListBillingInoutCartonInstrgRpt.ToList();
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                        AlocCount = objBillingInquiry.ListBillingInoutCartonInstrgRpt.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);
                                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document INout Carton");
                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document INout Carton");
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
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                                    if (type == "PDF")
                                    {
                                        var rptSource = objBillingInquiry.ListBillingInoutCartonRpt.ToList();
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                        AlocCount = objBillingInquiry.ListBillingInoutCartonRpt.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);
                                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document INout Carton");
                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document INout Carton");
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
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                                    if (type == "PDF")
                                    {
                                        var rptSource = objBillingInquiry.ListBillingInoutCubeInstrgRpt.ToList();
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                        AlocCount = objBillingInquiry.ListBillingInoutCubeInstrgRpt.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);
                                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document INout Cube");
                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document INout Cube");
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
                                    objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 


                                    if (type == "PDF")
                                    {
                                        var rptSource = objBillingInquiry.ListBillingInoutCubeRpt.ToList();
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                        AlocCount = objBillingInquiry.ListBillingInoutCubeRpt.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);
                                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document INout Cube");
                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document INout Cube");
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
                           // CR - 3PL_MVC_IB_2018_0219_004

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
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document INout Cntr");
                                    }
                                    else if (type == "Word")
                                    {
                                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Document INout Cntr");
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
                            // CR - 3PL_MVC_IB_2018_0219_004






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
                            if (l_str_rpt_bill_status=="P")
                        {
                        strReportName = "rpt_va_bill_inv.rpt";
                            //BillingInquiry objBillingInquiry = new BillingInquiry();
                            //BillingInquiryService ServiceObject = new BillingInquiryService();
                            ReportDocument rd = new ReportDocument();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                        objBillingInquiry.cmp_id = p_str_cmp_id;
                        objBillingInquiry.Bill_doc_id = SelectdID;
                        objBillingInquiry = ServiceObject.GetBillingInvoiceRpt(objBillingInquiry);
                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 

                            if (type == "PDF")
                            {
                                var rptSource = objBillingInquiry.ListBillingInvoiceRpt.ToList();
                                rd.Load(strRptPath);
                                 int AlocCount = 0;
                                AlocCount = objBillingInquiry.ListBillingInvoiceRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Invoice");
                            }
                            else if (type == "Word")
                            {
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Invoice");
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
                        //objBillingInquiry.cmp_id = TempData["p_str_cmp_id"].ToString().Trim();
                        //objBillingInquiry.Bill_doc_id = TempData["p_str_Bill_doc_id"].ToString().Trim();
                        //objBillingInquiry.bill_type = TempData["p_str_Bill_type"].ToString().Trim();
                        //objBillingInquiry.Bill_doc_dt_Fr = TempData["p_str_doc_dt_Fr"].ToString().Trim();
                        //objBillingInquiry.Bill_doc_dt_To = TempData["p_str_doc_dt_To"].ToString().Trim();


 
                        objBillingInquiry = ServiceObject.GetBillingSummaryRpt(objBillingInquiry);
                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 


                        if (type == "PDF")
                        {
                            var rptSource = objBillingInquiry.ListBillingSummaryRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objBillingInquiry.ListBillingSummaryRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim();
                            rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Grid Summary");
                        }
                        else if (type == "Word")
                        {
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Billing Grid Summary");
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
            }
             catch (Exception ex)
            {
                msg = ex.Message;
                jsonErrorCode = "-2";
            }

            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);

        }
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
                                    EmailSub = "BillDocument Report for" + " " + " " + objBillingInquiry.cmp_id;
                                    EmailMsg = "BillDocument Report hasbeen Attached for the Process";

                                    if (type == "PDF")
                                    {
                                        var rptSource = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.ToList();
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                        AlocCount = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);

                                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "3PL_CDC//BillDocument_" + strDateFormat + ".pdf";
                                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                        reportFileName = "BillDocument Report " + DateTime.Now.ToFileTime() + ".pdf";
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
                                    EmailSub = "BillDocument Report for" + " " + " " + objBillingInquiry.cmp_id;
                                    EmailMsg = "BillDocument Report hasbeen Attached for the Process";
                                    if (type == "PDF")
                                    {
                                        var rptSource = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.ToList();
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                        AlocCount = objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);

                                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "3PL_CDC//BillDocument_" + strDateFormat + ".pdf";
                                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                        reportFileName = "BillDocument Report " + DateTime.Now.ToFileTime() + ".pdf";
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
                                    EmailSub = "BillDocument Report for" + " " + " " + objBillingInquiry.cmp_id;
                                    EmailMsg = "BillDocument Report hasbeen Attached for the Process";
                                    if (type == "PDF")
                                    {
                                        var rptSource = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.ToList();
                                        rd.Load(strRptPath);
                                        int AlocCount = 0;
                                        AlocCount = objBillingInquiry.ListBillingDocSTRGCubewithinitRpt.Count();
                                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                            rd.SetDataSource(rptSource);

                                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "3PL_CDC//BillDocument_" + strDateFormat + ".pdf";
                                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                        reportFileName = "BillDocument Report " + DateTime.Now.ToFileTime() + ".pdf";
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

                                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
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
                            strReportName = "rpt_va_bill_doc.rpt";
                            }                           
                            ReportDocument rd = new ReportDocument();
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmp_id;
                            objBillingInquiry.Bill_doc_id = SelectdID;
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0421_001 
                            objBillingInquiry = ServiceObject.GetBillingBillDocVASRpt(objBillingInquiry);


                            if (type == "PDF")
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
                                }

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
                                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
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
                                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
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
                                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
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
                                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                        rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
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
                            EmailSub = "BillInvoice Report for" + " " + " " + objBillingInquiry.cmp_id;
                            EmailMsg = "BillInvoice Report hasbeen Attached for the Process";

                            if (type == "PDF")
                        {
                            var rptSource = objBillingInquiry.ListBillingInvoiceRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objBillingInquiry.ListBillingInvoiceRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                                objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "3PL_CDC//BillInvoice_" + strDateFormat + ".pdf";
                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                reportFileName = "BillInvoice Report " + DateTime.Now.ToFileTime() + ".pdf";
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
                        objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        EmailSub = "BillingInquiry GridSummary Report for" + " " + " " + objBillingInquiry.cmp_id;
                        EmailMsg = "BillingInquiry GridSummary Report hasbeen Attached for the Process";

                        if (type == "PDF")
                    {
                        var rptSource = objBillingInquiry.ListBillingSummaryRpt.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objBillingInquiry.ListBillingSummaryRpt.Count();
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                            rd.SetParameterValue("fml_image_path", objBillingInquiry.Image_Path);
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + "3PL_CDC//BillingInquiry GridSummary_" + strDateFormat + ".pdf";
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            reportFileName = "BillingInquiry GridSummary Report " + DateTime.Now.ToFileTime() + ".pdf";
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
        public ActionResult Billingdtl(string BilldocId, string cmp_id,string Billdocdate, string p_str_viewmode, string datefrom, string dateto,string Type)
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
            objBillingInquiry.blType = Type;/*CR-20180525-001 Added by Nithya*/
            objBillingInquiry.DocumentdateFrom = datefrom; //CR 20180308_01 Added by murugan
            objBillingInquiry.DocumentdateTo = dateto; //CR 20180308_01 Added by murugan
            objBillingInquiry = ServiceObject.GetBillingHdr(objBillingInquiry);
            objBillingInquiry.CUSTId = objBillingInquiry.ListBillingdetail[0].CUSTId.Trim();
            objBillingInquiry.cust_of_cmpid = objBillingInquiry.ListBillingdetail[0].cmp_id.Trim();
            objBillingInquiry.status = objBillingInquiry.ListBillingdetail[0].status.Trim();
            if (objBillingInquiry.status.Trim()=="O")
            {
                objBillingInquiry.status = "OPEN";
            }
            else if (objBillingInquiry.status.Trim()=="P")
                {
                objBillingInquiry.status = "POST";
            }
           
            objBillingInquiry.bill_type = objBillingInquiry.ListBillingdetail[0].bill_type.Trim();
            objBillingInquiry.billto_id = objBillingInquiry.ListBillingdetail[0].billto_id.Trim();
            objBillingInquiry.billperiodfr = objBillingInquiry.ListBillingdetail[0].billperiodfr.Trim();
            objBillingInquiry.billperiodTo = objBillingInquiry.ListBillingdetail[0].billperiodTo.Trim();
            objBillingInquiry.term_code = objBillingInquiry.ListBillingdetail[0].term_code.Trim();
            objBillingInquiry.ship_via = objBillingInquiry.ListBillingdetail[0].ship_via.Trim();
            objBillingInquiry.proj_id = objBillingInquiry.ListBillingdetail[0].proj_id.Trim();
            objBillingInquiry.prod_cost = Math.Round(Convert.ToDecimal(objBillingInquiry.ListBillingdetail[0].prod_cost),2);  //CR_3PL_MVC_BL_2018_0310_001   Modified by Soniya       
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
            if (l_str_status == "OPEN")
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
                            strReportName = "rpt_va_bill_doc.rpt";
                            }
                           
                            ReportDocument rd = new ReportDocument();
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Billing//" + strReportName;
                            objBillingInquiry.cmp_id = p_str_cmpid;
                            objBillingInquiry.Bill_doc_id = p_str_bill_doc_id;
                            objBillingInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString().Trim(); //CR_3PL_MVC_BL_2018_0421_001 
                            objBillingInquiry = ServiceObject.GetBillingBillDocVASRpt(objBillingInquiry);
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
                            }
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
        //END
        //CR-3PL_MVC_IB_2018_0428_001 Added By Nithya
        //public ActionResult BillingInquiryDtl(string p_str_cmp_id, string p_str_billdocid)
        //{
        //    try
        //    {

        //        BillingInquiry objBillingInquiry = new BillingInquiry();
        //        BillingInquiryService ServiceObject = new BillingInquiryService();
        //        string l_str_search_flag = string.Empty;
        //        string l_str_is_another_usr = string.Empty;

        //        l_str_is_another_usr = Session["IS3RDUSER"].ToString();
        //        objBillingInquiry.IS3RDUSER = l_str_is_another_usr.Trim();
        //        l_str_search_flag = Session["g_str_Search_flag"].ToString().Trim();
        //        objBillingInquiry.is_company_user = Session["IsCompanyUser"].ToString().Trim();
        //        if (l_str_search_flag == "True")
        //        {
        //            objBillingInquiry.is_company_user = Session["IsCompanyUser"].ToString().Trim();
        //            objBillingInquiry.cmp_id = Session["TEMP_CMP_ID"].ToString().Trim();
        //            objBillingInquiry.Bill_doc_id = Session["TEMP_BILLDOC_ID"].ToString().Trim();
        //            objBillingInquiry.Bill_type = Session["TEMP_BILL_TYPE"].ToString().Trim();
        //            objBillingInquiry.Bill_doc_dt_Fr = Session["TEMP_DOC_DT_FM"].ToString().Trim();
        //            objBillingInquiry.Bill_doc_dt_To = Session["TEMP_DOC_DT_TO"].ToString().Trim();
                   
        //        }
        //        else
        //        {
        //            objBillingInquiry.cmp_id = p_str_cmp_id;
        //            objBillingInquiry.ib_doc_id = p_str_billdocid;                                 
        //        }
        //        objBillingInquiry = ServiceObject.GetBillingInquiryDetails(objBillingInquiry);
        //        Mapper.CreateMap<BillingInquiry, BillingInquiryModel>();
        //        BillingInquiryModel BillingInquiryModel = Mapper.Map<BillingInquiry, BillingInquiryModel>(objBillingInquiry);
        //        return PartialView("_BillingInquiry", BillingInquiryModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        //END
    }
}