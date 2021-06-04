using AutoMapper;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Web.Script.Serialization;
#region Change History
//CR_3PL_MVC_BL_2018_0316_001  SONIYA     20180316  TO SET THE DEFAULT CMP ID
#endregion Change History

namespace GsEPWv8_5_MVC.Controllers
{
    public class CustMasterController : Controller
    {

        public ActionResult CountryChange(string countryid)
        {
            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            CustMaster objCustMaster = new CustMaster();
            objPick.Cntry_Id = countryid.Trim();
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objCustMaster.ListStatePick = objPick.ListStatePick;
            var serializer = new JavaScriptSerializer() { MaxJsonLength = 86753090 };
            serializer.Serialize(objCustMaster);
            return new JsonResult()
            {
                Data = objCustMaster,
                MaxJsonLength = 86753090
            };

        }

        //Costomer Inquiry Screen - Load
        public ActionResult CustMaster(string FullFillType, string cmp)
        {
            string l_str_cmp_id = string.Empty;
            string l_str_fm_dt = string.Empty;
            string l_str_tmp_cmp_id = string.Empty;                 // CR_3PL_MVC_COMMON_2018_0326_001
            string l_str_scn_id = string.Empty;
            string l_str_success = string.Empty;
            try
            {
                CustMaster objCustMaster = new CustMaster();
                ICustMasterService ServiceObject = new CustMasterService();
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                Session["g_str_Search_flag"] = "False";
                objCustMaster.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
                objCustMaster.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                if (objCustMaster.cmp_id != "" && FullFillType == null)
                {
                    objCompany.cmp_id = objCustMaster.cmp_id;
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objCustMaster.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    objCompany.cust_of_cmp_id = "";
                    objCompany = ServiceObjectCompany.GetCustOfCompName(objCompany);
                    objCustMaster.LstCustOfCmpName = objCompany.LstCustOfCmpName;
                    if (objCustMaster.LstCustOfCmpName.Count > 0)
                    {
                        objCustMaster.cust_of_cmpid = objCustMaster.LstCustOfCmpName[0].cust_of_cmpid;
                    }
                    objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
                    objCustMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
                    //objCustMaster = ServiceObject.GetCustMasterDetails(objCustMaster);
                    objCustMaster.cmp_id = string.Empty;
                    DateTime date = DateTime.Now;
                    l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");
                    objCustMaster.dt_of_entry = l_str_fm_dt;
                    objCustMaster.last_chg_dt = "";

                    objCustMaster = ServiceObject.GetDftCmpWhs(objCustMaster);
                    if (objCustMaster.ListPickdtl.Count > 0)
                    {
                        objCustMaster.whs_id = objCustMaster.ListPickdtl[0].dft_whs.Trim();
                    }

                    objCompany = ServiceObjectCompany.GetWhsIdDetails(objCompany);
                    objCustMaster.ListwhsPickDtl = objCompany.ListwhsPickDtl;

                }
                else
                {
                    if (FullFillType == null)
                    {
                        objCompany.cmp_id = objCustMaster.cmp_id;
                        objCompany.user_id = Session["UserID"].ToString().Trim();
                        objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                        objCustMaster.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                        objCompany.cust_of_cmp_id = "";
                        objCompany = ServiceObjectCompany.GetCustOfCompName(objCompany);
                        objCustMaster.LstCustOfCmpName = objCompany.LstCustOfCmpName;
                        if (objCustMaster.LstCustOfCmpName.Count > 0)
                        {
                            objCustMaster.cust_of_cmpid = objCustMaster.LstCustOfCmpName[0].cust_of_cmpid;
                        }
                        objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
                        objCustMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
                        //objCustMaster = ServiceObject.GetCustMasterDetails(objCustMaster);
                        DateTime date = DateTime.Now;
                        l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");
                        objCustMaster.dt_of_entry = l_str_fm_dt;
                        objCustMaster.last_chg_dt = "";

                        objCustMaster = ServiceObject.GetDftCmpWhs(objCustMaster);
                        if (objCustMaster.ListPickdtl.Count > 0)
                        {
                            objCustMaster.whs_id = objCustMaster.ListPickdtl[0].dft_whs.Trim();
                        }

                        objCompany = ServiceObjectCompany.GetWhsIdDetails(objCompany);
                        objCustMaster.ListwhsPickDtl = objCompany.ListwhsPickDtl;

                    }
                }
                if (FullFillType != null)
                {
                    //objCustMaster.cmp_id = cmp;
                    objCustMaster.cust_of_cmp_id = "";
                    objCustMaster.status = "";
                    objCustMaster.dt_of_entry = "";
                    objCustMaster.last_chg_dt = "";
                    objCompany.cmp_id = cmp;
                    objCompany = ServiceObjectCompany.GetFullFillCompanyDetails(objCompany);
                    objCustMaster.ListCompanyPickDtl = objCompany.ListFullFillCompanyPickDtl;
                    objCustMaster.dt_of_entry = "";
                    objCustMaster.last_chg_dt = "";
                    objCustMaster.io_rate_id = "INOUT-1";
                    objCustMaster.strg_rate_id = "STRG-1";
                }
                Mapper.CreateMap<CustMaster, CustMasterModel>();
                CustMasterModel objCustMasterModel = Mapper.Map<CustMaster, CustMasterModel>(objCustMaster);
                return View(objCustMasterModel);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        // Add Customer
        public ActionResult CustomerAdd(string p_str_cmp_id)
        {

            CustMaster objCustMaster = new CustMaster();
            ICustMasterService ServiceObject = new CustMasterService();
            objCustMaster.cmp_id = p_str_cmp_id;
            Session["tempcmpid"] = objCustMaster.cmp_id;
            objCustMaster.source = "-";
            objCustMaster.territory = "-";
            objCustMaster.credit_lt = "0.0";
            objCustMaster.term_code = "1";
            objCustMaster.pmt_term = "Payment due  upon receipt of Invoice";
            objCustMaster.cmp_id = p_str_cmp_id;
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();

            objCompany.cust_of_cmp_id = p_str_cmp_id;
            objCustMaster.cust_of_cmpid = p_str_cmp_id;
            objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
            objCustMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
            objCompany.cmp_id = objCustMaster.cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objCustMaster.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;

            objCustMaster = ServiceObject.GetDftCmpWhs(objCustMaster);
            if (objCustMaster.ListPickdtl.Count > 0)
            {
                objCustMaster.fob = objCustMaster.ListPickdtl[0].dft_whs.Trim();
                objCustMaster.whs_id = objCustMaster.ListPickdtl[0].dft_whs.Trim();
            }

            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = p_str_cmp_id;
            objPick.Whs_id = "";
            objPick.Whs_name = "";
            objPick = ServiceObjectPick.GetWhsPick(objPick);
            objCustMaster.ListPick = objPick.ListPick;
            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objCustMaster.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objCustMaster.ListStatePick = objPick.ListStatePick;

            objCustMaster.last_chg_dt = DateTime.Now.ToString("MM/dd/yyyy");
            objCustMaster.dt_of_entry = DateTime.Now.ToString("MM/dd/yyyy");
            LookUp objLookUp = new LookUp();
            LookUpService ServiceObject1 = new LookUpService();
            objLookUp.id = "16";
            objLookUp.lookuptype = "CUSTOMERMASTER";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
            objLookUp.id = "10";
            objLookUp.lookuptype = "STRG";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListSrtg = objLookUp.ListLookUpDtl;
            objLookUp.id = "9";
            objLookUp.lookuptype = "INOUT";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListInout = objLookUp.ListLookUpDtl;
            objLookUp.id = "17";
            objLookUp.lookuptype = "CUSTRCVLOTBY";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListRcvLot = objLookUp.ListLookUpDtl;
            objLookUp.id = "18";
            objLookUp.lookuptype = "CUSTALOCBY";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListAlocBy = objLookUp.ListLookUpDtl;
            objLookUp.id = "19";
            objLookUp.lookuptype = "DocAllowNewItem";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListDocAllowNewItm = objLookUp.ListLookUpDtl;
            objLookUp.id = "20";
            objLookUp.lookuptype = "ItemListBy";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListItmListBy = objLookUp.ListLookUpDtl;
            objLookUp.id = "21";
            objLookUp.lookuptype = "DirecAllowNewItem";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListDirectAllowNewItm = objLookUp.ListLookUpDtl;
            objLookUp.id = "22";
            objLookUp.lookuptype = "IncludeInitStrg";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListIncludeInitStrg = objLookUp.ListLookUpDtl;
            objLookUp.id = "23";
            objLookUp.lookuptype = "DocAutoIncrement";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListAutoIncrement = objLookUp.ListLookUpDtl;
            objLookUp.id = "24";
            objLookUp.lookuptype = "CustCatg";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListCustCatg = objLookUp.ListLookUpDtl;
            objLookUp.id = "25";
            objLookUp.lookuptype = "BillCycle";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListBillCycle = objLookUp.ListLookUpDtl;
            objLookUp.id = "26";
            objLookUp.lookuptype = "CreditType";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListCreditType = objLookUp.ListLookUpDtl;
            objLookUp.id = "27";
            objLookUp.lookuptype = "TaxExempt";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListTaxExempt = objLookUp.ListLookUpDtl;
            objLookUp.id = "102";
            objLookUp.lookuptype = "CUBEROUNDED";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListCubeRounded = objLookUp.ListLookUpDtl;

            objLookUp.id = "301";
            objLookUp.lookuptype = "CUST-TYPE";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListCustType = objLookUp.ListLookUpDtl;
            objCustMaster.cust_type = "B2B";

            CustMaster objCustMasterUser = ServiceObject.fnGetAppUserList(string.Empty, "ADD");

            CustMaster objCustRate = ServiceObject.fnGetCustDfltRate(p_str_cmp_id);
            if (objCustRate.ListCustDfltRate.Count > 0)
            {

                for (int i = 0; i < objCustRate.ListCustDfltRate.Count; i++)
                {
                    if (objCustRate.ListCustDfltRate[i].rate_type == "INOUT")
                    {
                        objCustMaster.io_rate_id = objCustRate.ListCustDfltRate[i].rate_id;
                        objCustMaster.io_rate_name = objCustRate.ListCustDfltRate[i].rate_name;
                        objCustMaster.io_rate_price = objCustRate.ListCustDfltRate[i].rate_price;
                        objCustMaster.inout_bill = objCustRate.ListCustDfltRate[i].inout_bill;
                    }
                    else if (objCustRate.ListCustDfltRate[i].rate_type == "STRG")
                    {
                        objCustMaster.strg_rate_id = objCustRate.ListCustDfltRate[i].rate_id;
                        objCustMaster.strg_rate_name = objCustRate.ListCustDfltRate[i].rate_name;
                        objCustMaster.strg_rate_price = objCustRate.ListCustDfltRate[i].rate_price;
                        objCustMaster.strg_bill = objCustRate.ListCustDfltRate[i].strg_bill;
                    }
                }
                if ((objCustMaster.io_rate_id == null) || (objCustMaster.io_rate_id == string.Empty))
                {
                    objCustMaster.io_rate_id = "INOUT-1";
                    objCustMaster.io_rate_name = "INOUT RATE";
                    objCustMaster.io_rate_price = Convert.ToDecimal( ".5");
                    objCustMaster.inout_bill = "Cube";
                }
                if ((objCustMaster.strg_rate_id == null) || (objCustMaster.strg_rate_id == string.Empty))
                {
                    objCustMaster.strg_rate_id = "STRG-1";
                    objCustMaster.strg_rate_name = "STORAGE RATE";
                    objCustMaster.strg_rate_price = Convert.ToDecimal(".26");
                    objCustMaster.strg_bill ="Cube";
                }
            }
            else
            {
                objCustMaster.io_rate_id = "INOUT-1";
                objCustMaster.io_rate_name = "INOUT RATE";
                objCustMaster.io_rate_price = Convert.ToDecimal(".5");
                objCustMaster.inout_bill = "Cube";
                objCustMaster.strg_rate_id = "STRG-1";
                objCustMaster.strg_rate_name = "STORAGE RATE";
                objCustMaster.strg_rate_price = Convert.ToDecimal(".26");
                objCustMaster.strg_bill = "Cube";
            }


            //objCustMaster.io_rate_id = "INOUT-1";
            //objCustMaster.strg_rate_id = "STRG-1";
           
            objCustMaster.ListAppUsers = objCustMasterUser.ListAppUsers;


            Mapper.CreateMap<CustMaster, CustMasterModel>();

            CustMasterModel objCustMastermodel = Mapper.Map<CustMaster, CustMasterModel>(objCustMaster);
            return PartialView("_CustAdd", objCustMastermodel);
        }
        public ActionResult CustomerCopy(string cmp_id)
        {
            CustMaster objCustMaster = new CustMaster();
            ICustMasterService ServiceObject = new CustMasterService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            LookUp objLookUp = new LookUp();
            objCustMaster.cust_of_cmp_id = Session["tempcmpid"].ToString();
            objCustMaster.cmp_id = cmp_id;
            LookUpService ServiceObject1 = new LookUpService();
            objCompany.cmp_id = objCustMaster.cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objCustMaster.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objCustMaster = ServiceObject.GetCustHdrDetails(objCustMaster);
            objCustMaster = ServiceObject.GetCustDetails(objCustMaster);
            objCustMaster.cust_of_cmp_id = objCustMaster.ListCustHdr[0].cmp_id.Trim();
            objCompany.cust_of_cmp_id = objCustMaster.ListCustHdr[0].cmp_id.Trim();
            objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
            objCustMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
            objCustMaster.cmp_id = objCustMaster.ListCustHdr[0].cust_id.Trim();
            objCustMaster.cmp_name = "";
            objCustMaster.cust_grp_id = objCustMaster.ListCustHdr[0].groupid.Trim();
            objCustMaster.status = (objCustMaster.ListCustHdr[0].status == null || objCustMaster.ListCustHdr[0].status.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].status.Trim();
            objCustMaster.dt_of_entry = (objCustMaster.ListCustHdr[0].start_dt == null || objCustMaster.ListCustHdr[0].start_dt == "" ? string.Empty : Convert.ToDateTime(objCustMaster.ListCustHdr[0].start_dt).ToString("MM/dd/yyyy")).Trim();
            objCustMaster.last_chg_dt = (objCustMaster.ListCustHdr[0].last_chg_dt == null || objCustMaster.ListCustHdr[0].last_chg_dt == "" ? string.Empty : Convert.ToDateTime(objCustMaster.ListCustHdr[0].last_chg_dt).ToString("MM/dd/yyyy"));
            objCustMaster.Tel = (objCustMaster.ListCustHdr[0].tel == null || objCustMaster.ListCustHdr[0].tel.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].tel.Trim();
            objCustMaster.cell = (objCustMaster.ListCustHdr[0].cell == null || objCustMaster.ListCustHdr[0].cell.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].cell.Trim();
            objCustMaster.contact = (objCustMaster.ListCustHdr[0].contact == null || objCustMaster.ListCustHdr[0].contact.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].contact.Trim();
            objCustMaster.email = (objCustMaster.ListCustHdr[0].email == null || objCustMaster.ListCustHdr[0].email.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].email.Trim();
            objCustMaster.fax = (objCustMaster.ListCustHdr[0].fax == null || objCustMaster.ListCustHdr[0].fax.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].fax.Trim();
            objCustMaster.web = (objCustMaster.ListCustHdr[0].web == null || objCustMaster.ListCustHdr[0].web.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].web.Trim();
            objCustMaster.source = (objCustMaster.ListCustHdr[0].source == null || objCustMaster.ListCustHdr[0].source.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].source.Trim();
            objCustMaster.whs_id = (objCustMaster.ListCustHdr[0].dft_whs == null || objCustMaster.ListCustHdr[0].dft_whs.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].dft_whs.Trim();
            objCustMaster.file_name = "";
            objCustMaster.Image_Path = "";
            objCustMaster.mail_name = (objCustMaster.ListCustDtl[0].mail_name == null || objCustMaster.ListCustDtl[0].mail_name.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].mail_name.Trim();
            objCustMaster.attn = (objCustMaster.ListCustDtl[0].attn == null || objCustMaster.ListCustDtl[0].attn.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].attn.Trim();
            objCustMaster.addr1 = (objCustMaster.ListCustDtl[0].addr_line1 == null || objCustMaster.ListCustDtl[0].addr_line1.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].addr_line1.Trim();
            objCustMaster.addr2 = (objCustMaster.ListCustDtl[0].addr_line2 == null || objCustMaster.ListCustDtl[0].addr_line2.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].addr_line2.Trim();
            objCustMaster.city = (objCustMaster.ListCustDtl[0].city == null || objCustMaster.ListCustDtl[0].city.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].city.Trim();
            objCustMaster.state = (objCustMaster.ListCustDtl[0].state_id == null || objCustMaster.ListCustDtl[0].state_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].state_id.Trim();
            objCustMaster.zip = (objCustMaster.ListCustDtl[0].post_code == null || objCustMaster.ListCustDtl[0].post_code.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].post_code.Trim();
            objCustMaster.country = (objCustMaster.ListCustDtl[0].cntry_id == null || objCustMaster.ListCustDtl[0].cntry_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].cntry_id.Trim();
            objCustMaster.frieght_id = (objCustMaster.ListCustDtl[0].freight_id == null || objCustMaster.ListCustDtl[0].freight_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].freight_id.Trim();
            objCustMaster.ship_via_id = (objCustMaster.ListCustDtl[0].ship_via_id == null || objCustMaster.ListCustDtl[0].ship_via_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].ship_via_id.Trim();
            objCustMaster.catg = (objCustMaster.ListCustDtl[0].cust_catg == null || objCustMaster.ListCustDtl[0].cust_catg.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].cust_catg.Trim();
            objCustMaster.bl_cycle = (objCustMaster.ListCustDtl[0].bill_cycle == null || objCustMaster.ListCustDtl[0].bill_cycle.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].bill_cycle.Trim();
            objCustMaster.code = (objCustMaster.ListCustDtl[0].crdt_code == null || objCustMaster.ListCustDtl[0].crdt_code.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_code.Trim();
            objCustMaster.type = (objCustMaster.ListCustDtl[0].crdt_chck == null || objCustMaster.ListCustDtl[0].crdt_chck.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_chck.Trim();
            objCustMaster.msg = (objCustMaster.ListCustDtl[0].crdt_msg == null || objCustMaster.ListCustDtl[0].crdt_msg.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_msg.Trim();
            objCustMaster.credit_lt = (objCustMaster.ListCustDtl[0].crdt_limit == null || objCustMaster.ListCustDtl[0].crdt_limit.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_limit.Trim();
            objCustMaster.disc = (objCustMaster.ListCustDtl[0].disc == null || objCustMaster.ListCustDtl[0].disc.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].disc.Trim();
            objCustMaster.term_code = (objCustMaster.ListCustDtl[0].terms_id == null || objCustMaster.ListCustDtl[0].terms_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].terms_id.Trim();
            objCustMaster.ord_val = (objCustMaster.ListCustDtl[0].ordr_value == null || objCustMaster.ListCustDtl[0].ordr_value.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].ordr_value.Trim();
            objCustMaster.tax_code = (objCustMaster.ListCustDtl[0].tax_exempt_id == null || objCustMaster.ListCustDtl[0].tax_exempt_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].tax_exempt_id.Trim();
            objCustMaster.tax_exempt = (objCustMaster.ListCustDtl[0].tax_exempt == null || objCustMaster.ListCustDtl[0].tax_exempt.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].tax_exempt.Trim();
            objCustMaster.gl_num = (objCustMaster.ListCustDtl[0].gl_num == null || objCustMaster.ListCustDtl[0].gl_num.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].gl_num.Trim();
            objCustMaster.file_name = "";
            objCustMaster = ServiceObject.GetDftWhs(objCustMaster);
            string l_str_DftWhs = objCustMaster.ListPickdtl[0].dft_whs.Trim();
            if (l_str_DftWhs != "" || l_str_DftWhs != null)
            {
                objCustMaster.fob = objCustMaster.ListPickdtl[0].dft_whs.Trim();        //CR-3PL_MVC_BL_2018_0316_001 Added by Soniya      
                objCustMaster.whs_id = objCustMaster.ListPickdtl[0].dft_whs.Trim();
            }
            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = Session["tempcmpid"].ToString(); ;
            objPick.Whs_id = "";
            objPick.Whs_name = "";
            objPick = ServiceObjectPick.GetWhsPick(objPick);
            objCustMaster.ListPick = objPick.ListPick;

            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objCustMaster.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objCustMaster.ListStatePick = objPick.ListStatePick;

            objCustMaster.region = objCustMaster.ListCustHdr[0].regn_id.Trim();
            objCustMaster.territory = objCustMaster.ListCustHdr[0].tery_id.Trim();
            objCustMaster = ServiceObject.GetCustConfigDetails(objCustMaster);

            objLookUp.id = "16";
            objLookUp.lookuptype = "CUSTOMERMASTER";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
            objLookUp.id = "10";
            objLookUp.lookuptype = "STRG";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListSrtg = objLookUp.ListLookUpDtl;

            objLookUp.id = "9";
            objLookUp.lookuptype = "INOUT";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListInout = objLookUp.ListLookUpDtl;

            if (objCustMaster.ListCustConfig.Count > 0)
            {

                objCustMaster.allow_new_item = objCustMaster.ListCustConfig[0].Allow_New_item.Trim();
                objCustMaster.inout_bill = objCustMaster.ListCustConfig[0].bill_inout_type.Trim();
                objCustMaster.bl_free_days = (objCustMaster.ListCustConfig[0].bill_free_days == null) ? "" : objCustMaster.ListCustConfig[0].bill_free_days.Trim();
                objCustMaster.pmt_term = (objCustMaster.ListCustConfig[0].pmt_term == null) ? "" : objCustMaster.ListCustConfig[0].pmt_term.Trim(); //CR20180112-001 Added By Nithya
                objCustMaster.strg_bill = objCustMaster.ListCustConfig[0].bill_type.Trim();
                objCustMaster.recv_lot_by = objCustMaster.ListCustConfig[0].Recv_Itm_Mode.Trim();
                objCustMaster.aloc_by = objCustMaster.ListCustConfig[0].Aloc_by.Trim();
                if (objCustMaster.ListCustConfig[0].Allow_New_item.Trim() == "Y")
                {
                    objCustMaster.allow_new_item = "Yes";
                }
                else
                {
                    objCustMaster.allow_new_item = "No";
                }

                objCustMaster.itemlistby = objCustMaster.ListCustConfig[0].item_pick;
                if (objCustMaster.ListCustConfig[0].item_pick.Trim() == "Document Entry")
                {
                    objCustMaster.itemlistby = "DocEntry";
                }
                else
                {
                    objCustMaster.itemlistby = "ItemMaster";
                }

                if (objCustMaster.ListCustConfig[0].init_strg_rt_req == "Y")
                {
                    objCustMaster.initstrg = "Yes";
                }
                else
                {
                    objCustMaster.initstrg = "No";
                }


                objCustMaster.autoincre = objCustMaster.ListCustConfig[0].Doc_Increment;
                if (objCustMaster.ListCustConfig[0].Doc_Increment == "Y")
                {
                    objCustMaster.autoincre = "Yes";
                }
                else
                {
                    objCustMaster.autoincre = "No";
                }
                //CR20180829-001 Added By Nithya
                objCustMaster.Recv_non_doc_itm = objCustMaster.ListCustConfig[0].Recv_non_doc_itm;
                if (objCustMaster.ListCustConfig[0].Recv_non_doc_itm == "Y")
                {
                    objCustMaster.Recv_non_doc_itm = "Yes";
                }
                else
                {
                    objCustMaster.Recv_non_doc_itm = "No";
                }
                //END
                //CR20180829-001 Added By Nithya
                objCustMaster.Stk_Chk_Reqt = objCustMaster.ListCustConfig[0].Stk_Chk_Reqt;
                if (objCustMaster.ListCustConfig[0].Stk_Chk_Reqt == "Y")
                {
                    objCustMaster.Stk_Chk_Reqt = "Yes";
                }
                else
                {
                    objCustMaster.Stk_Chk_Reqt = "No";
                }
                //CR20180112-001 Added By Nithya
                objCustMaster.cube_auto_calc = objCustMaster.ListCustConfig[0].cube_auto_calc;
                if (objCustMaster.ListCustConfig[0].cube_auto_calc == "Y")
                {
                    objCustMaster.cube_auto_calc = "Yes";
                }
                else
                {
                    objCustMaster.cube_auto_calc = "No";
                }

                objCustMaster.is_bill_by_cube_rounded = objCustMaster.ListCustConfig[0].is_bill_by_cube_rounded;
                objCustMaster.min_inout_cube = Convert.ToDecimal(objCustMaster.ListCustConfig[0].min_inout_cube);
                objCustMaster.min_strg_cube = Convert.ToDecimal(objCustMaster.ListCustConfig[0].min_strg_cube);

                if (objCustMaster.ListCustConfig[0].is_bill_by_cube_rounded == "Y")
                {
                    objCustMaster.is_bill_by_cube_rounded = "Yes";
                }
                else
                {
                    objCustMaster.is_bill_by_cube_rounded = "No";
                }

                //END
            }

            else
            {
                objCustMaster.bl_free_days = string.Empty;
                objCustMaster.strg_bill = string.Empty;
                objCustMaster.inout_bill = string.Empty;
                objCustMaster.allow_new_item = "Yes";
                objCustMaster.initstrg = "No";
                objCustMaster.itemlistby = "ItemMaster";
                objCustMaster.autoincre = "Yes";
                objCustMaster.Recv_non_doc_itm = "No";
                objCustMaster.Stk_Chk_Reqt = "No";
                objCustMaster.cube_auto_calc = "No";
                objCustMaster.is_bill_by_cube_rounded = "No";
                objCustMaster.min_inout_cube = 0;
                objCustMaster.min_strg_cube = 0;
            }
            objLookUp.id = "19";
            objLookUp.lookuptype = "DocAllowNewItem";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListDocAllowNewItm = objLookUp.ListLookUpDtl;

            objLookUp.id = "20";
            objLookUp.lookuptype = "ItemListBy";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListItmListBy = objLookUp.ListLookUpDtl;

            objLookUp.id = "22";
            objLookUp.lookuptype = "IncludeInitStrg";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListIncludeInitStrg = objLookUp.ListLookUpDtl;

            //END            
            objLookUp.id = "23";
            objLookUp.lookuptype = "DocAutoIncrement";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListAutoIncrement = objLookUp.ListLookUpDtl;
            //objCustMaster.recv_lot_by = objCustMaster.ListCustConfig[0].Recv_Itm_Mode;
            objLookUp.id = "17";
            objLookUp.lookuptype = "CUSTRCVLOTBY";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListRcvLot = objLookUp.ListLookUpDtl;
            objLookUp.id = "18";
            objLookUp.lookuptype = "CUSTALOCBY";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListAlocBy = objLookUp.ListLookUpDtl;
            objLookUp.id = "24";
            objLookUp.lookuptype = "CustCatg";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListCustCatg = objLookUp.ListLookUpDtl;
            objLookUp.id = "25";
            objLookUp.lookuptype = "BillCycle";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListBillCycle = objLookUp.ListLookUpDtl;
            objLookUp.id = "26";
            objLookUp.lookuptype = "CreditType";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListCreditType = objLookUp.ListLookUpDtl;
            objLookUp.id = "27";
            objLookUp.lookuptype = "TaxExempt";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListTaxExempt = objLookUp.ListLookUpDtl;

            objLookUp.id = "102";
            objLookUp.lookuptype = "CUBEROUNDED";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListCubeRounded = objLookUp.ListLookUpDtl;
            CustMaster objCustMasterUser = ServiceObject.fnGetAppUserList(string.Empty, "ADD");
            objCustMaster.ListAppUsers = objCustMasterUser.ListAppUsers;
            objCustMaster.io_rate_id = "INOUT-1";
            objCustMaster.strg_rate_id = "STRG-1";
            Mapper.CreateMap<CustMaster, CustMasterModel>();
            CustMasterModel objCustMastermodel = Mapper.Map<CustMaster, CustMasterModel>(objCustMaster);
            return PartialView("_CustAdd", objCustMastermodel);
        }
        // Edit Customer
        public ActionResult CustomerEdit(string cust_of_cmp_id, string cmp_id)
        {
            CustMaster objCustMaster = new CustMaster();
            ICustMasterService ServiceObject = new CustMasterService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            LookUp objLookUp = new LookUp();
            objCustMaster.cust_of_cmp_id = cust_of_cmp_id.TrimEnd();
            objCustMaster.cmp_id = cmp_id.TrimEnd();
            //string l_str_chk_itemlistby = string.Empty;
            //bool l_bool_chk_itemlistby;
            LookUpService ServiceObject1 = new LookUpService();
            objCustMaster = ServiceObject.GetCustHdrDetails(objCustMaster);
            objCustMaster = ServiceObject.GetCustDetails(objCustMaster);
            objCustMaster.cust_of_cmp_id = objCustMaster.ListCustHdr[0].cmp_id.Trim();
            objCompany.cust_of_cmp_id = objCustMaster.ListCustHdr[0].cmp_id.Trim();
            objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
            objCustMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
            objCustMaster.cmp_id = objCustMaster.ListCustHdr[0].cust_id.Trim();
            objCustMaster.cmp_name = objCustMaster.ListCustHdr[0].cust_name.Trim();
            objCustMaster.cust_grp_id = objCustMaster.ListCustHdr[0].groupid.Trim();
            var Result = ServiceObject.GetCustInitial(cmp_id);
            objCustMaster.con_string = Result;
            objCustMaster.status = (objCustMaster.ListCustHdr[0].status == null || objCustMaster.ListCustHdr[0].status.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].status.Trim();
            objCustMaster.dt_of_entry = (objCustMaster.ListCustHdr[0].start_dt == null || objCustMaster.ListCustHdr[0].start_dt == "" ? string.Empty : Convert.ToDateTime(objCustMaster.ListCustHdr[0].start_dt).ToString("MM/dd/yyyy")).Trim();
            objCustMaster.last_chg_dt = (objCustMaster.ListCustHdr[0].last_chg_dt == null || objCustMaster.ListCustHdr[0].last_chg_dt == "" ? string.Empty : Convert.ToDateTime(objCustMaster.ListCustHdr[0].last_chg_dt).ToString("MM/dd/yyyy"));
            objCustMaster.Tel = (objCustMaster.ListCustHdr[0].tel == null || objCustMaster.ListCustHdr[0].tel.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].tel.Trim();
            objCustMaster.cell = (objCustMaster.ListCustHdr[0].cell == null || objCustMaster.ListCustHdr[0].cell.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].cell.Trim();
            objCustMaster.contact = (objCustMaster.ListCustHdr[0].contact == null || objCustMaster.ListCustHdr[0].contact.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].contact.Trim();
            objCustMaster.email = (objCustMaster.ListCustHdr[0].email == null || objCustMaster.ListCustHdr[0].email.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].email.Trim();
            objCustMaster.fax = (objCustMaster.ListCustHdr[0].fax == null || objCustMaster.ListCustHdr[0].fax.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].fax.Trim();
            objCustMaster.web = (objCustMaster.ListCustHdr[0].web == null || objCustMaster.ListCustHdr[0].web.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].web.Trim();
            objCustMaster.source = (objCustMaster.ListCustHdr[0].source == null || objCustMaster.ListCustHdr[0].source.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].source.Trim();
            objCustMaster.whs_id = (objCustMaster.ListCustHdr[0].dft_whs == null || objCustMaster.ListCustHdr[0].dft_whs.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].dft_whs.Trim();
            objCustMaster.mail_name = (objCustMaster.ListCustDtl[0].mail_name == null || objCustMaster.ListCustDtl[0].mail_name.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].mail_name.Trim();
            objCustMaster.attn = (objCustMaster.ListCustDtl[0].attn == null || objCustMaster.ListCustDtl[0].attn.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].attn.Trim();
            objCustMaster.addr1 = (objCustMaster.ListCustDtl[0].addr_line1 == null || objCustMaster.ListCustDtl[0].addr_line1.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].addr_line1.Trim();
            objCustMaster.addr2 = (objCustMaster.ListCustDtl[0].addr_line2 == null || objCustMaster.ListCustDtl[0].addr_line2.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].addr_line2.Trim();
            objCustMaster.city = (objCustMaster.ListCustDtl[0].city == null || objCustMaster.ListCustDtl[0].city.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].city.Trim();
            objCustMaster.state = (objCustMaster.ListCustDtl[0].state_id == null || objCustMaster.ListCustDtl[0].state_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].state_id.Trim();
            objCustMaster.zip = (objCustMaster.ListCustDtl[0].post_code == null || objCustMaster.ListCustDtl[0].post_code.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].post_code.Trim();
            objCustMaster.country = (objCustMaster.ListCustDtl[0].cntry_id == null || objCustMaster.ListCustDtl[0].cntry_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].cntry_id.Trim();
            objCustMaster.frieght_id = (objCustMaster.ListCustDtl[0].freight_id == null || objCustMaster.ListCustDtl[0].freight_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].freight_id.Trim();
            objCustMaster.ship_via_id = (objCustMaster.ListCustDtl[0].ship_via_id == null || objCustMaster.ListCustDtl[0].ship_via_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].ship_via_id.Trim();
            objCustMaster.catg = (objCustMaster.ListCustDtl[0].cust_catg == null || objCustMaster.ListCustDtl[0].cust_catg.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].cust_catg.Trim();
            objCustMaster.bl_cycle = (objCustMaster.ListCustDtl[0].bill_cycle == null || objCustMaster.ListCustDtl[0].bill_cycle.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].bill_cycle.Trim();
            objCustMaster.code = (objCustMaster.ListCustDtl[0].crdt_code == null || objCustMaster.ListCustDtl[0].crdt_code.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_code.Trim();
            objCustMaster.type = (objCustMaster.ListCustDtl[0].crdt_chck == null || objCustMaster.ListCustDtl[0].crdt_chck.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_chck.Trim();
            objCustMaster.msg = (objCustMaster.ListCustDtl[0].crdt_msg == null || objCustMaster.ListCustDtl[0].crdt_msg.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_msg.Trim();
            objCustMaster.credit_lt = (objCustMaster.ListCustDtl[0].crdt_limit == null || objCustMaster.ListCustDtl[0].crdt_limit.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_limit.Trim();
            objCustMaster.disc = (objCustMaster.ListCustDtl[0].disc == null || objCustMaster.ListCustDtl[0].disc.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].disc.Trim();
            objCustMaster.term_code = (objCustMaster.ListCustDtl[0].terms_id == null || objCustMaster.ListCustDtl[0].terms_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].terms_id.Trim();
            objCustMaster.ord_val = (objCustMaster.ListCustDtl[0].ordr_value == null || objCustMaster.ListCustDtl[0].ordr_value.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].ordr_value.Trim();
            objCustMaster.tax_code = (objCustMaster.ListCustDtl[0].tax_exempt_id == null || objCustMaster.ListCustDtl[0].tax_exempt_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].tax_exempt_id.Trim();
            objCustMaster.tax_exempt = (objCustMaster.ListCustDtl[0].tax_exempt == null || objCustMaster.ListCustDtl[0].tax_exempt.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].tax_exempt.Trim();
            objCustMaster.gl_num = (objCustMaster.ListCustDtl[0].gl_num == null || objCustMaster.ListCustDtl[0].gl_num.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].gl_num.Trim();
            objCustMaster.file_name = (objCustMaster.ListCustDtl[0].cust_logo == null || objCustMaster.ListCustDtl[0].cust_logo == "") ? string.Empty : objCustMaster.ListCustDtl[0].cust_logo;//CR20180724-001 Added By Nithya
            //objCustMaster.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.file_name;
            Session["ImagePath"] = objCustMaster.file_name;
            objCustMaster.Image_Path = objCustMaster.file_name;
            objCustMaster = ServiceObject.GetDftWhs(objCustMaster);
            string l_str_DftWhs = objCustMaster.ListPickdtl[0].dft_whs.Trim();
            if (l_str_DftWhs != "" || l_str_DftWhs != null)
            {
                objCustMaster.fob = objCustMaster.ListPickdtl[0].dft_whs.Trim();        //CR-3PL_MVC_BL_2018_0316_001 Added by Soniya      
                objCustMaster.whs_id = objCustMaster.ListPickdtl[0].dft_whs.Trim();

            }
            //objCompany.cust_cmp_id = cmpid;
            //objCompany.whs_id = "";           
            //objCompany = ServiceObjectCompany.GetWhsIdDetails(objCompany);
            //objVasInquiry.ListwhsPickDtl = objCompany.ListwhsPickDtl;
            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = cmp_id;
            objPick.Whs_id = "";
            objPick.Whs_name = "";
            objPick = ServiceObjectPick.GetWhsPick(objPick);
            objCustMaster.ListPick = objPick.ListPick;
            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objCustMaster.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objCustMaster.ListStatePick = objPick.ListStatePick;
            objCustMaster.region = objCustMaster.ListCustHdr[0].regn_id.Trim();
            objCustMaster.territory = objCustMaster.ListCustHdr[0].tery_id.Trim();
            objCustMaster = ServiceObject.GetCustConfigDetails(objCustMaster);


            objLookUp.id = "301";
            objLookUp.lookuptype = "CUST-TYPE";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListCustType = objLookUp.ListLookUpDtl;
      

            if (objCustMaster.ListCustConfig.Count > 0)
            {
                objCustMaster.bl_free_days = (objCustMaster.ListCustConfig[0].bill_free_days == null) ? "" : objCustMaster.ListCustConfig[0].bill_free_days.Trim();
                objCustMaster.pmt_term = (objCustMaster.ListCustConfig[0].pmt_term == null) ? "" : objCustMaster.ListCustConfig[0].pmt_term.Trim(); //CR20180112-001 Added By Nithya
                objCustMaster.strg_bill = objCustMaster.ListCustConfig[0].bill_type;
                objCustMaster.inout_bill = objCustMaster.ListCustConfig[0].bill_inout_type.Trim();
                objCustMaster.allow_new_item = objCustMaster.ListCustConfig[0].Allow_New_item.Trim();
                objCustMaster.recv_lot_by = objCustMaster.ListCustConfig[0].Recv_Itm_Mode.Trim();
                objCustMaster.aloc_by = objCustMaster.ListCustConfig[0].Aloc_by.Trim();
                if (objCustMaster.ListCustConfig[0].Allow_New_item.Trim() == "Y")
                {
                    objCustMaster.allow_new_item = "Yes";
                }
                else
                {
                    objCustMaster.allow_new_item = "No";
                }
                if (objCustMaster.ListCustConfig[0].init_strg_rt_req == "Y")
                {
                    objCustMaster.initstrg = "Yes";
                }
                else
                {
                    objCustMaster.initstrg = "No";
                }
                objCustMaster.itemlistby = objCustMaster.ListCustConfig[0].item_pick;
                if (objCustMaster.ListCustConfig[0].item_pick.Trim() == "Document Entry")
                {
                    objCustMaster.itemlistby = "DocEntry";
                }
                else
                {
                    objCustMaster.itemlistby = "ItemMaster";
                }

                objCustMaster.autoincre = objCustMaster.ListCustConfig[0].Doc_Increment;
                if (objCustMaster.ListCustConfig[0].Doc_Increment == "Y")
                {
                    objCustMaster.autoincre = "Yes";
                }
                else
                {
                    objCustMaster.autoincre = "No";
                }
                //CR20180829-001 Added By Nithya
                objCustMaster.Recv_non_doc_itm = objCustMaster.ListCustConfig[0].Recv_non_doc_itm;
                if (objCustMaster.ListCustConfig[0].Recv_non_doc_itm == "Y")
                {
                    objCustMaster.Recv_non_doc_itm = "Yes";
                }
                else
                {
                    objCustMaster.Recv_non_doc_itm = "No";
                }
                //END  
                //CR20180829-001 Added By Nithya
                objCustMaster.Stk_Chk_Reqt = objCustMaster.ListCustConfig[0].Stk_Chk_Reqt;
                if (objCustMaster.ListCustConfig[0].Stk_Chk_Reqt == "Y")
                {
                    objCustMaster.Stk_Chk_Reqt = "Yes";
                }
                else
                {
                    objCustMaster.Stk_Chk_Reqt = "No";
                }
                //CR20180112-001 Added By Nithya
                objCustMaster.cube_auto_calc = objCustMaster.ListCustConfig[0].cube_auto_calc;
                if (objCustMaster.ListCustConfig[0].cube_auto_calc == "Y")
                {
                    objCustMaster.cube_auto_calc = "Yes";
                }
                else
                {
                    objCustMaster.cube_auto_calc = "No";
                }

                objCustMaster.is_bill_by_cube_rounded = objCustMaster.ListCustConfig[0].is_bill_by_cube_rounded;
                objCustMaster.min_inout_cube = Convert.ToDecimal( objCustMaster.ListCustConfig[0].min_inout_cube);
                objCustMaster.min_strg_cube = Convert.ToDecimal(objCustMaster.ListCustConfig[0].min_strg_cube);

                if (objCustMaster.ListCustConfig[0].is_bill_by_cube_rounded == "Y")
                {
                    objCustMaster.is_bill_by_cube_rounded = "Yes";
                }
                else
                {
                    objCustMaster.is_bill_by_cube_rounded = "No";
                }

                objCustMaster.ecom_recv_by_bin = objCustMaster.ListCustConfig[0].ecom_recv_by_bin;
                objCustMaster.allow_940_new_item = objCustMaster.ListCustConfig[0].allow_940_new_item;
                objCustMaster.cust_type = objCustMaster.ListCustConfig[0].cust_type;

                //END
            }
            else
            {
                objCustMaster.bl_free_days = string.Empty;
                objCustMaster.strg_bill = string.Empty;
                objCustMaster.inout_bill = string.Empty;
                objCustMaster.allow_new_item = "Yes";
                objCustMaster.initstrg = "No";
                objCustMaster.itemlistby = "ItemMaster";
                objCustMaster.autoincre = "Yes";
                objCustMaster.Recv_non_doc_itm = "No";
                objCustMaster.Stk_Chk_Reqt = "No";
                objCustMaster.cube_auto_calc = "No";
                objCustMaster.is_bill_by_cube_rounded = "No";
                objCustMaster.min_inout_cube = 0;
                objCustMaster.min_strg_cube = 0;
                objCustMaster.ecom_recv_by_bin = false;
                objCustMaster.allow_940_new_item = false;
                objCustMaster.cust_type = "B2B";
            }

            objLookUp.id = "16";
            objLookUp.lookuptype = "CUSTOMERMASTER";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
            objLookUp.id = "10";
            objLookUp.lookuptype = "STRG";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListSrtg = objLookUp.ListLookUpDtl;

            objLookUp.id = "9";
            objLookUp.lookuptype = "INOUT";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListInout = objLookUp.ListLookUpDtl;



            objLookUp.id = "19";
            objLookUp.lookuptype = "DocAllowNewItem";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListDocAllowNewItm = objLookUp.ListLookUpDtl;

            objLookUp.id = "20";
            objLookUp.lookuptype = "ItemListBy";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListItmListBy = objLookUp.ListLookUpDtl;
            //if (objCustMaster.itemlistby == "Document Entry")
            //{
            //    objCustMaster.itemlistby = "Yes";
            //} 


            objLookUp.id = "22";
            objLookUp.lookuptype = "IncludeInitStrg";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListIncludeInitStrg = objLookUp.ListLookUpDtl;

            //END              
            objLookUp.id = "23";
            objLookUp.lookuptype = "DocAutoIncrement";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListAutoIncrement = objLookUp.ListLookUpDtl;
            //objCustMaster.recv_lot_by = objCustMaster.ListCustConfig[0].Recv_Itm_Mode;
            objLookUp.id = "17";
            objLookUp.lookuptype = "CUSTRCVLOTBY";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListRcvLot = objLookUp.ListLookUpDtl;
            
            objLookUp.id = "18";
            objLookUp.lookuptype = "CUSTALOCBY";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListAlocBy = objLookUp.ListLookUpDtl;
            objLookUp.id = "24";
            objLookUp.lookuptype = "CustCatg";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListCustCatg = objLookUp.ListLookUpDtl;
            objLookUp.id = "25";
            objLookUp.lookuptype = "BillCycle";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListBillCycle = objLookUp.ListLookUpDtl;
            objLookUp.id = "26";
            objLookUp.lookuptype = "CreditType";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListCreditType = objLookUp.ListLookUpDtl;

            objLookUp.id = "27";
            objLookUp.lookuptype = "TaxExempt";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListTaxExempt = objLookUp.ListLookUpDtl;

            objLookUp.id = "102";
            objLookUp.lookuptype = "CUBEROUNDED";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListCubeRounded = objLookUp.ListLookUpDtl;
            CustMaster objCustRate = ServiceObject.fnGetCustDfltRate(cmp_id);
            if (objCustRate.ListCustDfltRate.Count > 0)
            {

                for (int i = 0; i < objCustRate.ListCustDfltRate.Count; i++)
                {
                    if (objCustRate.ListCustDfltRate[i].rate_type == "INOUT")
                    {
                        objCustMaster.io_rate_id = objCustRate.ListCustDfltRate[i].rate_id;
                        objCustMaster.io_rate_name = objCustRate.ListCustDfltRate[i].rate_name;
                        objCustMaster.io_rate_price = objCustRate.ListCustDfltRate[i].rate_price;
                    }
                    else if (objCustRate.ListCustDfltRate[i].rate_type == "STRG")
                    {
                        objCustMaster.strg_rate_id = objCustRate.ListCustDfltRate[i].rate_id;
                        objCustMaster.strg_rate_name = objCustRate.ListCustDfltRate[i].rate_name;
                        objCustMaster.strg_rate_price = objCustRate.ListCustDfltRate[i].rate_price;
                    }
                }
                if ((objCustMaster.io_rate_id == null) || (objCustMaster.io_rate_id == string.Empty)) objCustMaster.io_rate_id = "INOUT-1";
                if ((objCustMaster.strg_rate_id == null) || (objCustMaster.strg_rate_id == string.Empty)) objCustMaster.strg_rate_id = "STRG-1";
            }
            else
            {
                objCustMaster.io_rate_id = "INOUT-1";
                objCustMaster.strg_rate_id = "STRG-1";
            }
         

            CustMaster objCustMasterUser = ServiceObject.fnGetAppUserList(cmp_id, "MOD");
            objCustMaster.ListAppUsers = objCustMasterUser.ListAppUsers;


            Mapper.CreateMap<CustMaster, CustMasterModel>();
            CustMasterModel objCustMastermodel = Mapper.Map<CustMaster, CustMasterModel>(objCustMaster);
            return PartialView("_CustEdit", objCustMastermodel);
        }

        // View Customer
        public ActionResult CustomerViews(string cust_of_cmp_id, string cmp_id)
        {
            CustMaster objCustMaster = new CustMaster();
            ICustMasterService ServiceObject = new CustMasterService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            LookUp objLookUp = new LookUp();
            objCustMaster.cust_of_cmp_id = cust_of_cmp_id;
            objCustMaster.cmp_id = cmp_id;
            LookUpService ServiceObject1 = new LookUpService();
            objCustMaster = ServiceObject.GetCustHdrDetails(objCustMaster);
            objCustMaster = ServiceObject.GetCustDetails(objCustMaster);
            objCustMaster.cust_of_cmp_id = objCustMaster.ListCustHdr[0].cmp_id.Trim();
            objCompany.cust_of_cmp_id = objCustMaster.ListCustHdr[0].cmp_id.Trim();
            objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
            objCustMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
            objCustMaster.cmp_id = objCustMaster.ListCustHdr[0].cust_id.Trim();
            objCustMaster.cmp_name = objCustMaster.ListCustHdr[0].cust_name.Trim();
            objCustMaster.cust_grp_id = objCustMaster.ListCustHdr[0].groupid.Trim();
            var Result = ServiceObject.GetCustInitial(cmp_id);
            objCustMaster.con_string = Result;
            objCustMaster.status = (objCustMaster.ListCustHdr[0].status == null || objCustMaster.ListCustHdr[0].status.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].status.Trim();
            objCustMaster.dt_of_entry = (objCustMaster.ListCustHdr[0].start_dt == null || objCustMaster.ListCustHdr[0].start_dt == "" ? string.Empty : Convert.ToDateTime(objCustMaster.ListCustHdr[0].start_dt).ToString("MM/dd/yyyy")).Trim();
            objCustMaster.last_chg_dt = (objCustMaster.ListCustHdr[0].last_chg_dt == null || objCustMaster.ListCustHdr[0].last_chg_dt == "" ? string.Empty : Convert.ToDateTime(objCustMaster.ListCustHdr[0].last_chg_dt).ToString("MM/dd/yyyy"));
            objCustMaster.Tel = (objCustMaster.ListCustHdr[0].tel == null || objCustMaster.ListCustHdr[0].tel.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].tel.Trim();
            objCustMaster.cell = (objCustMaster.ListCustHdr[0].cell == null || objCustMaster.ListCustHdr[0].cell.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].cell.Trim();
            objCustMaster.contact = (objCustMaster.ListCustHdr[0].contact == null || objCustMaster.ListCustHdr[0].contact.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].contact.Trim();
            objCustMaster.email = (objCustMaster.ListCustHdr[0].email == null || objCustMaster.ListCustHdr[0].email.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].email.Trim();
            objCustMaster.fax = (objCustMaster.ListCustHdr[0].fax == null || objCustMaster.ListCustHdr[0].fax.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].fax.Trim();
            objCustMaster.web = (objCustMaster.ListCustHdr[0].web == null || objCustMaster.ListCustHdr[0].web.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].web.Trim();
            objCustMaster.source = (objCustMaster.ListCustHdr[0].source == null || objCustMaster.ListCustHdr[0].source.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].source.Trim();
            objCustMaster.whs_id = (objCustMaster.ListCustHdr[0].dft_whs == null || objCustMaster.ListCustHdr[0].dft_whs.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].dft_whs.Trim();
            objCustMaster.file_name = (objCustMaster.ListCustDtl[0].cust_logo == null || objCustMaster.ListCustDtl[0].cust_logo == "") ? string.Empty : objCustMaster.ListCustDtl[0].cust_logo;
            objCustMaster.Image_Path = objCustMaster.file_name;
            objCustMaster.mail_name = (objCustMaster.ListCustDtl[0].mail_name == null || objCustMaster.ListCustDtl[0].mail_name.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].mail_name.Trim();
            objCustMaster.attn = (objCustMaster.ListCustDtl[0].attn == null || objCustMaster.ListCustDtl[0].attn.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].attn.Trim();
            objCustMaster.addr1 = (objCustMaster.ListCustDtl[0].addr_line1 == null || objCustMaster.ListCustDtl[0].addr_line1.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].addr_line1.Trim();
            objCustMaster.addr2 = (objCustMaster.ListCustDtl[0].addr_line2 == null || objCustMaster.ListCustDtl[0].addr_line2.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].addr_line2.Trim();
            objCustMaster.city = (objCustMaster.ListCustDtl[0].city == null || objCustMaster.ListCustDtl[0].city.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].city.Trim();
            objCustMaster.state = (objCustMaster.ListCustDtl[0].state_id == null || objCustMaster.ListCustDtl[0].state_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].state_id.Trim();
            objCustMaster.zip = (objCustMaster.ListCustDtl[0].post_code == null || objCustMaster.ListCustDtl[0].post_code.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].post_code.Trim();
            objCustMaster.country = (objCustMaster.ListCustDtl[0].cntry_id == null || objCustMaster.ListCustDtl[0].cntry_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].cntry_id.Trim();
            objCustMaster.frieght_id = (objCustMaster.ListCustDtl[0].freight_id == null || objCustMaster.ListCustDtl[0].freight_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].freight_id.Trim();
            objCustMaster.ship_via_id = (objCustMaster.ListCustDtl[0].ship_via_id == null || objCustMaster.ListCustDtl[0].ship_via_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].ship_via_id.Trim();
            objCustMaster.catg = (objCustMaster.ListCustDtl[0].cust_catg == null || objCustMaster.ListCustDtl[0].cust_catg.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].cust_catg.Trim();
            objCustMaster.bl_cycle = (objCustMaster.ListCustDtl[0].bill_cycle == null || objCustMaster.ListCustDtl[0].bill_cycle.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].bill_cycle.Trim();
            objCustMaster.code = (objCustMaster.ListCustDtl[0].crdt_code == null || objCustMaster.ListCustDtl[0].crdt_code.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_code.Trim();
            objCustMaster.type = (objCustMaster.ListCustDtl[0].crdt_chck == null || objCustMaster.ListCustDtl[0].crdt_chck.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_chck.Trim();
            objCustMaster.msg = (objCustMaster.ListCustDtl[0].crdt_msg == null || objCustMaster.ListCustDtl[0].crdt_msg.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_msg.Trim();
            objCustMaster.credit_lt = (objCustMaster.ListCustDtl[0].crdt_limit == null || objCustMaster.ListCustDtl[0].crdt_limit.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_limit.Trim();
            objCustMaster.disc = (objCustMaster.ListCustDtl[0].disc == null || objCustMaster.ListCustDtl[0].disc.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].disc.Trim();
            objCustMaster.term_code = (objCustMaster.ListCustDtl[0].terms_id == null || objCustMaster.ListCustDtl[0].terms_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].terms_id.Trim();
            objCustMaster.ord_val = (objCustMaster.ListCustDtl[0].ordr_value == null || objCustMaster.ListCustDtl[0].ordr_value.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].ordr_value.Trim();
            objCustMaster.tax_code = (objCustMaster.ListCustDtl[0].tax_exempt_id == null || objCustMaster.ListCustDtl[0].tax_exempt_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].tax_exempt_id.Trim();
            objCustMaster.tax_exempt = (objCustMaster.ListCustDtl[0].tax_exempt == null || objCustMaster.ListCustDtl[0].tax_exempt.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].tax_exempt.Trim();
            objCustMaster.gl_num = (objCustMaster.ListCustDtl[0].gl_num == null || objCustMaster.ListCustDtl[0].gl_num.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].gl_num.Trim();
            objCustMaster.file_name = (objCustMaster.ListCustDtl[0].cust_logo == null || objCustMaster.ListCustDtl[0].cust_logo.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].cust_logo.Trim();//CR20180724-001 Added By Nithya
            objCustMaster = ServiceObject.GetDftWhs(objCustMaster);
            string l_str_DftWhs = objCustMaster.ListPickdtl[0].dft_whs.Trim();
            if (l_str_DftWhs != "" || l_str_DftWhs != null)
            {
                objCustMaster.fob = objCustMaster.ListPickdtl[0].dft_whs.Trim();        //CR-3PL_MVC_BL_2018_0316_001 Added by Soniya      
                objCustMaster.whs_id = objCustMaster.ListPickdtl[0].dft_whs.Trim();
            }
            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = cmp_id;
            objPick.Whs_id = "";
            objPick.Whs_name = "";
            objPick = ServiceObjectPick.GetWhsPick(objPick);
            objCustMaster.ListPick = objPick.ListPick;
            objCustMaster.region = objCustMaster.ListCustHdr[0].regn_id.Trim();
            objCustMaster.territory = objCustMaster.ListCustHdr[0].tery_id.Trim();
            objCustMaster = ServiceObject.GetCustConfigDetails(objCustMaster);

            objLookUp.id = "16";
            objLookUp.lookuptype = "CUSTOMERMASTER";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
            objLookUp.id = "10";
            objLookUp.lookuptype = "STRG";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListSrtg = objLookUp.ListLookUpDtl;

            objLookUp.id = "9";
            objLookUp.lookuptype = "INOUT";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListInout = objLookUp.ListLookUpDtl;

            objLookUp.id = "301";
            objLookUp.lookuptype = "CUST-TYPE";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListCustType = objLookUp.ListLookUpDtl;


            if (objCustMaster.ListCustConfig.Count > 0)
            {

                objCustMaster.allow_new_item = objCustMaster.ListCustConfig[0].Allow_New_item.Trim();
                objCustMaster.inout_bill = objCustMaster.ListCustConfig[0].bill_inout_type.Trim();
                objCustMaster.bl_free_days = (objCustMaster.ListCustConfig[0].bill_free_days == null) ? "" : objCustMaster.ListCustConfig[0].bill_free_days.Trim();
                objCustMaster.pmt_term = (objCustMaster.ListCustConfig[0].pmt_term == null) ? "" : objCustMaster.ListCustConfig[0].pmt_term.Trim(); //CR20180112-001 Added By Nithya
                objCustMaster.strg_bill = objCustMaster.ListCustConfig[0].bill_type.Trim();
                objCustMaster.recv_lot_by = objCustMaster.ListCustConfig[0].Recv_Itm_Mode.Trim();
                objCustMaster.aloc_by = objCustMaster.ListCustConfig[0].Aloc_by.Trim();
                if (objCustMaster.ListCustConfig[0].Allow_New_item.Trim() == "Y")
                {
                    objCustMaster.allow_new_item = "Yes";
                }
                else
                {
                    objCustMaster.allow_new_item = "No";
                }

                objCustMaster.itemlistby = objCustMaster.ListCustConfig[0].item_pick;
                if (objCustMaster.ListCustConfig[0].item_pick.Trim() == "Document Entry")
                {
                    objCustMaster.itemlistby = "DocEntry";
                }
                else
                {
                    objCustMaster.itemlistby = "ItemMaster";
                }

                if (objCustMaster.ListCustConfig[0].init_strg_rt_req == "Y")
                {
                    objCustMaster.initstrg = "Yes";
                }
                else
                {
                    objCustMaster.initstrg = "No";
                }


                objCustMaster.autoincre = objCustMaster.ListCustConfig[0].Doc_Increment;
                if (objCustMaster.ListCustConfig[0].Doc_Increment == "Y")
                {
                    objCustMaster.autoincre = "Yes";
                }
                else
                {
                    objCustMaster.autoincre = "No";
                }
                //CR20180829-001 Added By Nithya
                objCustMaster.Recv_non_doc_itm = objCustMaster.ListCustConfig[0].Recv_non_doc_itm;
                if (objCustMaster.ListCustConfig[0].Recv_non_doc_itm == "Y")
                {
                    objCustMaster.Recv_non_doc_itm = "Yes";
                }
                else
                {
                    objCustMaster.Recv_non_doc_itm = "No";
                }
                //END
                //CR20180829-001 Added By Nithya
                objCustMaster.Stk_Chk_Reqt = objCustMaster.ListCustConfig[0].Stk_Chk_Reqt;
                if (objCustMaster.ListCustConfig[0].Stk_Chk_Reqt == "Y")
                {
                    objCustMaster.Stk_Chk_Reqt = "Yes";
                }
                else
                {
                    objCustMaster.Stk_Chk_Reqt = "No";
                }
                //CR20180112-001 Added By Nithya
                objCustMaster.cube_auto_calc = objCustMaster.ListCustConfig[0].cube_auto_calc;
                if (objCustMaster.ListCustConfig[0].cube_auto_calc == "Y")
                {
                    objCustMaster.cube_auto_calc = "Yes";
                }
                else
                {
                    objCustMaster.cube_auto_calc = "No";
                }

                objCustMaster.is_bill_by_cube_rounded = objCustMaster.ListCustConfig[0].is_bill_by_cube_rounded;
                objCustMaster.min_inout_cube = Convert.ToDecimal(objCustMaster.ListCustConfig[0].min_inout_cube);
                objCustMaster.min_strg_cube = Convert.ToDecimal(objCustMaster.ListCustConfig[0].min_strg_cube);

                if (objCustMaster.ListCustConfig[0].is_bill_by_cube_rounded == "Y")
                {
                    objCustMaster.is_bill_by_cube_rounded = "Yes";
                }
                else
                {
                    objCustMaster.is_bill_by_cube_rounded = "No";
                }
                objCustMaster.ecom_recv_by_bin = objCustMaster.ListCustConfig[0].ecom_recv_by_bin;
                objCustMaster.allow_940_new_item = objCustMaster.ListCustConfig[0].allow_940_new_item;
                objCustMaster.cust_type = objCustMaster.ListCustConfig[0].cust_type;

                CustMaster objCustRate = ServiceObject.fnGetCustDfltRate(cmp_id);
                if (objCustRate.ListCustDfltRate.Count > 0)
                {

                    for (int i = 0; i < objCustRate.ListCustDfltRate.Count; i++)
                    {
                        if (objCustRate.ListCustDfltRate[i].rate_type == "INOUT")
                        {
                            objCustMaster.io_rate_id = objCustRate.ListCustDfltRate[i].rate_id;
                            objCustMaster.io_rate_name = objCustRate.ListCustDfltRate[i].rate_name;
                            objCustMaster.io_rate_price = objCustRate.ListCustDfltRate[i].rate_price;
                        }
                        else if (objCustRate.ListCustDfltRate[i].rate_type == "STRG")
                        {
                            objCustMaster.strg_rate_id = objCustRate.ListCustDfltRate[i].rate_id;
                            objCustMaster.strg_rate_name = objCustRate.ListCustDfltRate[i].rate_name;
                            objCustMaster.strg_rate_price = objCustRate.ListCustDfltRate[i].rate_price;
                        }
                    }
                }
            }

            else
            {
                objCustMaster.bl_free_days = string.Empty;
                objCustMaster.strg_bill = string.Empty;
                objCustMaster.inout_bill = string.Empty;
                objCustMaster.allow_new_item = "Yes";
                objCustMaster.initstrg = "No";
                objCustMaster.itemlistby = "ItemMaster";
                objCustMaster.autoincre = "Yes";
                objCustMaster.Recv_non_doc_itm = "No";
                objCustMaster.Stk_Chk_Reqt = "No";
                objCustMaster.cube_auto_calc = "No";
                objCustMaster.is_bill_by_cube_rounded = "No";
                objCustMaster.min_inout_cube = 0;
                objCustMaster.min_strg_cube = 0;
                objCustMaster.cust_type = "B2B";
            }
            objLookUp.id = "19";
            objLookUp.lookuptype = "DocAllowNewItem";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListDocAllowNewItm = objLookUp.ListLookUpDtl;

            objLookUp.id = "20";
            objLookUp.lookuptype = "ItemListBy";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListItmListBy = objLookUp.ListLookUpDtl;

            objLookUp.id = "22";
            objLookUp.lookuptype = "IncludeInitStrg";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListIncludeInitStrg = objLookUp.ListLookUpDtl;

            //END            
            objLookUp.id = "23";
            objLookUp.lookuptype = "DocAutoIncrement";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListAutoIncrement = objLookUp.ListLookUpDtl;
            //objCustMaster.recv_lot_by = objCustMaster.ListCustConfig[0].Recv_Itm_Mode;
            objLookUp.id = "17";
            objLookUp.lookuptype = "CUSTRCVLOTBY";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListRcvLot = objLookUp.ListLookUpDtl;
            objLookUp.id = "18";
            objLookUp.lookuptype = "CUSTALOCBY";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListAlocBy = objLookUp.ListLookUpDtl;
            objLookUp.id = "24";
            objLookUp.lookuptype = "CustCatg";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListCustCatg = objLookUp.ListLookUpDtl;
            objLookUp.id = "25";
            objLookUp.lookuptype = "BillCycle";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListBillCycle = objLookUp.ListLookUpDtl;
            objLookUp.id = "26";
            objLookUp.lookuptype = "CreditType";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListCreditType = objLookUp.ListLookUpDtl;
            objLookUp.id = "27";
            objLookUp.lookuptype = "TaxExempt";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListTaxExempt = objLookUp.ListLookUpDtl;

            objLookUp.id = "102";
            objLookUp.lookuptype = "CUBEROUNDED";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListCubeRounded = objLookUp.ListLookUpDtl;

            CustMaster objCustMasterUser = ServiceObject.fnGetAppUserList(cmp_id, "VIEW");
            objCustMaster.ListAppUsers = objCustMasterUser.ListAppUsers;

            Mapper.CreateMap<CustMaster, CustMasterModel>();
            CustMasterModel objCustMastermodel = Mapper.Map<CustMaster, CustMasterModel>(objCustMaster);
            return PartialView("_CustView", objCustMastermodel);
        }
        // Delete Customer
        public ActionResult Delete(string cust_of_cmp_id, string cmp_id)
        {

            CustMaster objCustMaster = new CustMaster();
            ICustMasterService ServiceObject = new CustMasterService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            LookUp objLookUp = new LookUp();
            objCustMaster.cust_of_cmp_id = cust_of_cmp_id;
            objCustMaster.cmp_id = cmp_id;
            //string l_str_chk_itemlistby = string.Empty;
            //bool l_bool_chk_itemlistby;
            LookUpService ServiceObject1 = new LookUpService();
            objCustMaster = ServiceObject.GetCustHdrDetails(objCustMaster);
            objCustMaster = ServiceObject.GetCustDetails(objCustMaster);
            objCustMaster.cust_of_cmp_id = objCustMaster.ListCustHdr[0].cmp_id.Trim();
            objCompany.cust_of_cmp_id = objCustMaster.ListCustHdr[0].cmp_id.Trim();
            objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
            objCustMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
            objCustMaster.cmp_id = objCustMaster.ListCustHdr[0].cust_id.Trim();
            objCustMaster.cmp_name = objCustMaster.ListCustHdr[0].cust_name.Trim();
            objCustMaster.cust_grp_id = objCustMaster.ListCustHdr[0].groupid.Trim();
            var Result = ServiceObject.GetCustInitial(cmp_id);
            objCustMaster.con_string = Result;
            objCustMaster.status = (objCustMaster.ListCustHdr[0].status == null || objCustMaster.ListCustHdr[0].status.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].status.Trim();
            objCustMaster.dt_of_entry = (objCustMaster.ListCustHdr[0].start_dt == null || objCustMaster.ListCustHdr[0].start_dt == "" ? string.Empty : Convert.ToDateTime(objCustMaster.ListCustHdr[0].start_dt).ToString("MM/dd/yyyy")).Trim();
            objCustMaster.last_chg_dt = (objCustMaster.ListCustHdr[0].last_chg_dt == null || objCustMaster.ListCustHdr[0].last_chg_dt == "" ? string.Empty : Convert.ToDateTime(objCustMaster.ListCustHdr[0].last_chg_dt).ToString("MM/dd/yyyy"));
            objCustMaster.Tel = (objCustMaster.ListCustHdr[0].tel == null || objCustMaster.ListCustHdr[0].tel.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].tel.Trim();
            objCustMaster.cell = (objCustMaster.ListCustHdr[0].cell == null || objCustMaster.ListCustHdr[0].cell.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].cell.Trim();
            objCustMaster.contact = (objCustMaster.ListCustHdr[0].contact == null || objCustMaster.ListCustHdr[0].contact.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].contact.Trim();
            objCustMaster.email = (objCustMaster.ListCustHdr[0].email == null || objCustMaster.ListCustHdr[0].email.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].email.Trim();
            objCustMaster.fax = (objCustMaster.ListCustHdr[0].fax == null || objCustMaster.ListCustHdr[0].fax.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].fax.Trim();
            objCustMaster.web = (objCustMaster.ListCustHdr[0].web == null || objCustMaster.ListCustHdr[0].web.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].web.Trim();
            objCustMaster.source = (objCustMaster.ListCustHdr[0].source == null || objCustMaster.ListCustHdr[0].source.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].source.Trim();
            objCustMaster.whs_id = (objCustMaster.ListCustHdr[0].dft_whs == null || objCustMaster.ListCustHdr[0].dft_whs.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].dft_whs.Trim();
            objCustMaster.mail_name = (objCustMaster.ListCustDtl[0].mail_name == null || objCustMaster.ListCustDtl[0].mail_name.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].mail_name.Trim();
            objCustMaster.attn = (objCustMaster.ListCustDtl[0].attn == null || objCustMaster.ListCustDtl[0].attn.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].attn.Trim();
            objCustMaster.addr1 = (objCustMaster.ListCustDtl[0].addr_line1 == null || objCustMaster.ListCustDtl[0].addr_line1.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].addr_line1.Trim();
            objCustMaster.addr2 = (objCustMaster.ListCustDtl[0].addr_line2 == null || objCustMaster.ListCustDtl[0].addr_line2.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].addr_line2.Trim();
            objCustMaster.city = (objCustMaster.ListCustDtl[0].city == null || objCustMaster.ListCustDtl[0].city.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].city.Trim();
            objCustMaster.state = (objCustMaster.ListCustDtl[0].state_id == null || objCustMaster.ListCustDtl[0].state_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].state_id.Trim();
            objCustMaster.zip = (objCustMaster.ListCustDtl[0].post_code == null || objCustMaster.ListCustDtl[0].post_code.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].post_code.Trim();
            objCustMaster.country = (objCustMaster.ListCustDtl[0].cntry_id == null || objCustMaster.ListCustDtl[0].cntry_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].cntry_id.Trim();
            objCustMaster.frieght_id = (objCustMaster.ListCustDtl[0].freight_id == null || objCustMaster.ListCustDtl[0].freight_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].freight_id.Trim();
            objCustMaster.ship_via_id = (objCustMaster.ListCustDtl[0].ship_via_id == null || objCustMaster.ListCustDtl[0].ship_via_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].ship_via_id.Trim();
            objCustMaster.catg = (objCustMaster.ListCustDtl[0].cust_catg == null || objCustMaster.ListCustDtl[0].cust_catg.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].cust_catg.Trim();
            objCustMaster.bl_cycle = (objCustMaster.ListCustDtl[0].bill_cycle == null || objCustMaster.ListCustDtl[0].bill_cycle.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].bill_cycle.Trim();
            objCustMaster.code = (objCustMaster.ListCustDtl[0].crdt_code == null || objCustMaster.ListCustDtl[0].crdt_code.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_code.Trim();
            objCustMaster.type = (objCustMaster.ListCustDtl[0].crdt_chck == null || objCustMaster.ListCustDtl[0].crdt_chck.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_chck.Trim();
            objCustMaster.msg = (objCustMaster.ListCustDtl[0].crdt_msg == null || objCustMaster.ListCustDtl[0].crdt_msg.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_msg.Trim();
            objCustMaster.credit_lt = (objCustMaster.ListCustDtl[0].crdt_limit == null || objCustMaster.ListCustDtl[0].crdt_limit.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_limit.Trim();
            objCustMaster.disc = (objCustMaster.ListCustDtl[0].disc == null || objCustMaster.ListCustDtl[0].disc.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].disc.Trim();
            objCustMaster.term_code = (objCustMaster.ListCustDtl[0].terms_id == null || objCustMaster.ListCustDtl[0].terms_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].terms_id.Trim();
            objCustMaster.ord_val = (objCustMaster.ListCustDtl[0].ordr_value == null || objCustMaster.ListCustDtl[0].ordr_value.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].ordr_value.Trim();
            objCustMaster.tax_code = (objCustMaster.ListCustDtl[0].tax_exempt_id == null || objCustMaster.ListCustDtl[0].tax_exempt_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].tax_exempt_id.Trim();
            objCustMaster.tax_exempt = (objCustMaster.ListCustDtl[0].tax_exempt == null || objCustMaster.ListCustDtl[0].tax_exempt.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].tax_exempt.Trim();
            objCustMaster.gl_num = (objCustMaster.ListCustDtl[0].gl_num == null || objCustMaster.ListCustDtl[0].gl_num.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].gl_num.Trim();
            objCustMaster.file_name = (objCustMaster.ListCustDtl[0].cust_logo == null || objCustMaster.ListCustDtl[0].cust_logo.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].cust_logo.Trim();//CR20180724-001 Added By Nithya
            Session["ImagePath"] = objCustMaster.file_name;
            objCustMaster.Image_Path = objCustMaster.file_name;
            objCustMaster = ServiceObject.GetDftWhs(objCustMaster);
            string l_str_DftWhs = objCustMaster.ListPickdtl[0].dft_whs.Trim();
            if (l_str_DftWhs != "" || l_str_DftWhs != null)
            {
                objCustMaster.fob = objCustMaster.ListPickdtl[0].dft_whs.Trim();        //CR-3PL_MVC_BL_2018_0316_001 Added by Soniya      
                objCustMaster.whs_id = objCustMaster.ListPickdtl[0].dft_whs.Trim();

            }

            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = cmp_id;
            objPick.Whs_id = "";
            objPick.Whs_name = "";
            objPick = ServiceObjectPick.GetWhsPick(objPick);
            objCustMaster.ListPick = objPick.ListPick;
            objCustMaster.region = objCustMaster.ListCustHdr[0].regn_id.Trim();
            objCustMaster.territory = objCustMaster.ListCustHdr[0].tery_id.Trim();
            objCustMaster = ServiceObject.GetCustConfigDetails(objCustMaster);

            objLookUp.id = "16";
            objLookUp.lookuptype = "CUSTOMERMASTER";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
            objLookUp.id = "10";
            objLookUp.lookuptype = "STRG";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListSrtg = objLookUp.ListLookUpDtl;

            objLookUp.id = "9";
            objLookUp.lookuptype = "INOUT";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListInout = objLookUp.ListLookUpDtl;

            objLookUp.id = "301";
            objLookUp.lookuptype = "CUST-TYPE";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListCustType = objLookUp.ListLookUpDtl;


            if (objCustMaster.ListCustConfig.Count > 0)
            {
                objCustMaster.inout_bill = objCustMaster.ListCustConfig[0].bill_inout_type.Trim();

                objCustMaster.bl_free_days = (objCustMaster.ListCustConfig[0].bill_free_days == null) ? "" : objCustMaster.ListCustConfig[0].bill_free_days.Trim();
                objCustMaster.pmt_term = (objCustMaster.ListCustConfig[0].pmt_term == null) ? "" : objCustMaster.ListCustConfig[0].pmt_term.Trim(); //CR20180112-001 Added By Nithya
                objCustMaster.strg_bill = objCustMaster.ListCustConfig[0].bill_type.Trim();
                objCustMaster.allow_new_item = objCustMaster.ListCustConfig[0].Allow_New_item.Trim();
                objCustMaster.recv_lot_by = objCustMaster.ListCustConfig[0].Recv_Itm_Mode.Trim();
                objCustMaster.aloc_by = objCustMaster.ListCustConfig[0].Aloc_by.Trim();
              
                if (objCustMaster.ListCustConfig[0].Allow_New_item.Trim() == "Y")
                {
                    objCustMaster.allow_new_item = "Yes";
                }
                else
                {
                    objCustMaster.allow_new_item = "No";
                }

                objCustMaster.itemlistby = objCustMaster.ListCustConfig[0].item_pick;
                if (objCustMaster.ListCustConfig[0].item_pick.Trim() == "Document Entry")
                {
                    objCustMaster.itemlistby = "DocEntry";
                }
                else
                {
                    objCustMaster.itemlistby = "ItemMaster";
                }

                if (objCustMaster.ListCustConfig[0].init_strg_rt_req == "Y")
                {
                    objCustMaster.initstrg = "Yes";
                }
                else
                {
                    objCustMaster.initstrg = "No";
                }

                objCustMaster.autoincre = objCustMaster.ListCustConfig[0].Doc_Increment;
                if (objCustMaster.ListCustConfig[0].Doc_Increment == "Y")
                {
                    objCustMaster.autoincre = "Yes";
                }
                else
                {
                    objCustMaster.autoincre = "No";
                }
                //CR20180829-001 Added By Nithya
                objCustMaster.Recv_non_doc_itm = objCustMaster.ListCustConfig[0].Recv_non_doc_itm;
                if (objCustMaster.ListCustConfig[0].Recv_non_doc_itm == "Y")
                {
                    objCustMaster.Recv_non_doc_itm = "Yes";
                }
                else
                {
                    objCustMaster.Recv_non_doc_itm = "No";
                }
                //END 
                //CR20180829-001 Added By Nithya
                objCustMaster.Stk_Chk_Reqt = objCustMaster.ListCustConfig[0].Stk_Chk_Reqt;
                if (objCustMaster.ListCustConfig[0].Stk_Chk_Reqt == "Y")
                {
                    objCustMaster.Stk_Chk_Reqt = "Yes";
                }
                else
                {
                    objCustMaster.Stk_Chk_Reqt = "No";
                }
                //CR20180112-001 Added By Nithya
                objCustMaster.cube_auto_calc = objCustMaster.ListCustConfig[0].cube_auto_calc;
                if (objCustMaster.ListCustConfig[0].cube_auto_calc == "Y")
                {
                    objCustMaster.cube_auto_calc = "Yes";
                }
                else
                {
                    objCustMaster.cube_auto_calc = "No";
                }
                //END

                objCustMaster.is_bill_by_cube_rounded = objCustMaster.ListCustConfig[0].is_bill_by_cube_rounded;
                objCustMaster.min_inout_cube = Convert.ToDecimal(objCustMaster.ListCustConfig[0].min_inout_cube);
                objCustMaster.min_strg_cube = Convert.ToDecimal(objCustMaster.ListCustConfig[0].min_strg_cube);
                if (objCustMaster.ListCustConfig[0].is_bill_by_cube_rounded == "Y")
                {
                    objCustMaster.is_bill_by_cube_rounded = "Yes";
                }
                else
                {
                    objCustMaster.is_bill_by_cube_rounded = "No";
                }
                objCustMaster.ecom_recv_by_bin = objCustMaster.ListCustConfig[0].ecom_recv_by_bin;
                objCustMaster.allow_940_new_item = objCustMaster.ListCustConfig[0].allow_940_new_item;
                objCustMaster.cust_type = objCustMaster.ListCustConfig[0].cust_type;
            }
            else
            {

                objCustMaster.bl_free_days = string.Empty;
                objCustMaster.strg_bill = string.Empty;
                objCustMaster.inout_bill = string.Empty;
                objCustMaster.allow_new_item = "Yes";
                objCustMaster.initstrg = "No";
                objCustMaster.itemlistby = "ItemMaster";
                objCustMaster.autoincre = "Yes";
                objCustMaster.Recv_non_doc_itm = "No";
                objCustMaster.Stk_Chk_Reqt = "No";
                objCustMaster.cube_auto_calc = "No";
                objCustMaster.is_bill_by_cube_rounded = "No";
                objCustMaster.min_inout_cube =0;
                objCustMaster.min_strg_cube = 0;
                objCustMaster.cust_type = "B2B";
            }


            objLookUp.id = "19";
            objLookUp.lookuptype = "DocAllowNewItem";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListDocAllowNewItm = objLookUp.ListLookUpDtl;

            objLookUp.id = "20";
            objLookUp.lookuptype = "ItemListBy";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListItmListBy = objLookUp.ListLookUpDtl;

            objLookUp.id = "22";
            objLookUp.lookuptype = "IncludeInitStrg";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListIncludeInitStrg = objLookUp.ListLookUpDtl;

            //END          
            objLookUp.id = "23";
            objLookUp.lookuptype = "DocAutoIncrement";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListAutoIncrement = objLookUp.ListLookUpDtl;
            // objCustMaster.recv_lot_by = objCustMaster.ListCustConfig[0].Recv_Itm_Mode;
            objLookUp.id = "17";
            objLookUp.lookuptype = "CUSTRCVLOTBY";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListRcvLot = objLookUp.ListLookUpDtl;
            objLookUp.id = "18";
            objLookUp.lookuptype = "CUSTALOCBY";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListAlocBy = objLookUp.ListLookUpDtl;
            objLookUp.id = "24";
            objLookUp.lookuptype = "CustCatg";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListCustCatg = objLookUp.ListLookUpDtl;
            objLookUp.id = "25";
            objLookUp.lookuptype = "BillCycle";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListBillCycle = objLookUp.ListLookUpDtl;
            objLookUp.id = "26";
            objLookUp.lookuptype = "CreditType";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListCreditType = objLookUp.ListLookUpDtl;
            objLookUp.id = "27";
            objLookUp.lookuptype = "TaxExempt";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListTaxExempt = objLookUp.ListLookUpDtl;

            objLookUp.id = "102";
            objLookUp.lookuptype = "CUBEROUNDED";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListCubeRounded = objLookUp.ListLookUpDtl;

            Mapper.CreateMap<CustMaster, CustMasterModel>();
            CustMasterModel objCustMastermodel = Mapper.Map<CustMaster, CustMasterModel>(objCustMaster);
            return PartialView("_CustDel", objCustMastermodel);
        }

        // Delete Customer - Save
        public ActionResult CustDelete(string p_str_cmp_id, string p_str_cust_id)
        {
            int resultcount;
            CustMaster objCustMaster = new CustMaster();
            ICustMasterService ServiceObject = new CustMasterService();
            objCustMaster.cust_of_cmp_id = p_str_cmp_id;
            objCustMaster.cmp_id = p_str_cust_id;
            objCustMaster = ServiceObject.CheckCustomerExist(objCustMaster);
            if (objCustMaster.ListGetCustLogo.Count == 0)
            {
                ServiceObject.DeleteCust(objCustMaster);
                //ServiceObject.DeleteCmpHdr(objCustMaster);
                resultcount = 1;
            }
            else
            {
                resultcount = 0;
                return Json(resultcount, JsonRequestBehavior.AllowGet);
            }
            CustMaster objCustMasterUser = ServiceObject.fnGetAppUserList(p_str_cust_id, "ADD");
            Mapper.CreateMap<CustMaster, CustMasterModel>();
            CustMasterModel objCustMastermodel = Mapper.Map<CustMaster, CustMasterModel>(objCustMaster);
            return Json(resultcount, JsonRequestBehavior.AllowGet);
        }

        // Search Customer
        public ActionResult GetCustomerDetails(string p_str_cust_id, string p_str_cmp_id, string p_str_whs_id)
        {
            try
            {
                CustMaster objCustMaster = new CustMaster();
                ICustMasterService ServiceObject = new CustMasterService();
                objCustMaster.user_id = Session["UserID"].ToString().Trim();
                objCustMaster.cust_of_cmp_id = p_str_cust_id.Trim();
                objCustMaster.cmp_id = p_str_cmp_id.Trim();
                objCustMaster.whs_id = p_str_whs_id;
                Session["g_str_Search_flag"] = "True";
                objCustMaster = ServiceObject.GetCustMasterDetails(objCustMaster);
                Mapper.CreateMap<CustMaster, CustMasterModel>();
                CustMasterModel CustMasterModel = Mapper.Map<CustMaster, CustMasterModel>(objCustMaster);
                return PartialView("_CustMaster", CustMasterModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Add Customer Save 
        public ActionResult SaveCustdtls(string p_str_cmp_id, string p_str_cust_id, string p_str_cmp_name, string p_str_whs_id, string p_str_source, string p_str_region, string p_str_territory,
            string p_str_cust_grp_id, string p_str_status, string p_str_contact, string p_str_dt_of_entry,
             string p_str_last_chg_dt, string p_str_tel, string p_str_fax, string p_str_email, string p_str_web, string p_str_cell, string p_str_bl_free_days, string p_str_strg_bill, string p_str_inout_bill, string p_str_allow_new_item,
             string p_str_itm_listby, string p_str_auto_inc, string p_str_rcv_lot_by, string p_str_initstrg, string p_str_aloc_by, string p_str_mail_name, string p_str_attn, string p_str_addr1,
             string p_str_addr2, string p_str_city, string p_str_state, string p_str_zip, string p_str_country, string p_str_frieght_id, string p_str_ship_via_id, string p_str_catg, string p_str_bill_cycle, string p_str_credit_code,
             string p_str_credit_type, string p_str_credit_msg, string p_str_credit_limit, string p_str_discount, string p_str_terms_code, string p_str_order_value, string p_str_tax_code,
             string p_str_tax_exempt, string p_str_gl_num, string p_str_file_name, string p_str_recv_doc_itm, string p_str_stk_chk_reqt,
             string p_str_pmt_term,string p_str_cube_auto_cacl, string p_str_is_bill_by_cube_rounded, String p_str_min_inout_cube, 
             String p_str_min_strg_cube, string p_str_Cust_initial, string lstrUserList,
             string pstrIORateId, string pstrIORateName, string pdecIORatePrice, string pstrStrgRateId, 
             string pstrStrgRateName, string pdecStrgRatePrice, bool p_str_ecom_recv_by_bin, bool p_str_allow_940_new_item, string p_str_cust_type)
        {
            try
            {
                string l_str_lot_count = string.Empty;
                string l_str_docid_count = string.Empty;
                string fullPath = string.Empty;
                string fileName = string.Empty;
                int imgflag = 0;
                int CustInitial = 0;
                CustMaster objCustMaster = new CustMaster();
                ICustMasterService ServiceObject = new CustMasterService();
                if (lstrUserList.Length >0 )
                {

                }
                if (p_str_Cust_initial != null)
                {
                   var Result = ServiceObject.CheckExistCustInitial(p_str_Cust_initial);
                    if (Result != "")
                    {
                        CustInitial = 3;
                        return Json(CustInitial, JsonRequestBehavior.AllowGet);
                    }
                    objCustMaster.con_string = p_str_Cust_initial;
                }
                
                objCustMaster.cust_of_cmp_id = p_str_cmp_id.Trim();
                objCustMaster.cmp_id = p_str_cust_id.Trim();

                objCustMaster = ServiceObject.GetCheckExistCmpId(objCustMaster);
                if (objCustMaster.ListCheckExistCmpId.Count == 0)
                {
                    objCustMaster.user_id = Session["UserID"].ToString().Trim();
                    objCustMaster.cmp_name = p_str_cmp_name.Trim();
                    objCustMaster.contact = p_str_contact.Trim();
                    objCustMaster.cust_grp_id = p_str_cust_grp_id.Trim();
                    objCustMaster.status = p_str_status.Trim();
                    objCustMaster.dt_of_entry = p_str_dt_of_entry;
                    objCustMaster.last_chg_dt = p_str_last_chg_dt.Trim();
                    objCustMaster.Tel = p_str_tel.Trim();
                    objCustMaster.cell = p_str_cell.Trim();
                    objCustMaster.email = p_str_email.Trim();
                    if (p_str_fax == null)
                    {
                        objCustMaster.fax = "";
                    }
                    else
                    {
                        objCustMaster.fax = p_str_fax.Trim();
                    }
                    //CR20180706-001 Added by Nithya
                    if (p_str_file_name == "" || p_str_file_name == null)
                    {
                        objCustMaster.cust_logo = "";
                    }
                    else
                    {
                        objCustMaster.cust_logo = SaveCustLogo(p_str_cust_id, p_str_file_name);
                        string strSourceFolder = Path.Combine(Server.MapPath("~/Images/Customer"));
                        strSourceFolder = Path.Combine(strSourceFolder, p_str_cust_id);
                        System.IO.FileInfo l_str_file_cust_logo = new FileInfo(Path.Combine(strSourceFolder, p_str_file_name.Substring(p_str_file_name.LastIndexOf('/') + 1)));
                        if (!l_str_file_cust_logo.Exists)
                        {
                            imgflag = 2;
                            return Json(imgflag, JsonRequestBehavior.AllowGet);
                        }
                    }

                           
                    objCustMaster.web = p_str_web;
                    objCustMaster.source = p_str_source.Trim();
                    objCustMaster.whs_id = p_str_whs_id.Trim();
                    objCustMaster.whs_name = p_str_whs_id.Trim();
                    objCustMaster.region = p_str_region.Trim();
                    objCustMaster.territory = p_str_territory.Trim();
                    objCustMaster.insaleid = "";
                    objCustMaster.outsaleid = "";
                    ServiceObject.SaveCustMaster(objCustMaster);
                    objCustMaster.mail_name = p_str_mail_name.Trim();
                    objCustMaster.attn = p_str_attn.Trim();
                    objCustMaster.addr1 = p_str_addr1.Trim();
                    objCustMaster.addr2 = p_str_addr2.Trim();
                    objCustMaster.city = p_str_city.Trim();
                    objCustMaster.state = p_str_state.Trim();
                    objCustMaster.zip = p_str_zip;
                    objCustMaster.country = p_str_country.Trim();
                    objCustMaster.frieght_id = p_str_frieght_id.Trim();
                    objCustMaster.ship_via_id = p_str_ship_via_id.Trim();
                    objCustMaster.catg = p_str_catg.Trim();
                    objCustMaster.bl_cycle = p_str_bill_cycle.Trim();
                    objCustMaster.code = p_str_credit_code.Trim();
                    objCustMaster.type = p_str_credit_type.Trim();
                    objCustMaster.msg = p_str_credit_msg.Trim();
                    objCustMaster.credit_lt = p_str_credit_limit.Trim();
                    objCustMaster.disc = p_str_discount.Trim();
                    objCustMaster.term_code = p_str_terms_code;
                    objCustMaster.ord_val = p_str_order_value.Trim();
                    objCustMaster.tax_code = p_str_tax_code.Trim();
                    objCustMaster.tax_exempt = p_str_tax_exempt.Trim();
                    objCustMaster.gl_num = p_str_gl_num.Trim();
                    ServiceObject.SaveCustMasterDtl(objCustMaster);
                    ServiceObject.SaveCmpHdr(objCustMaster);
                    ServiceObject.SaveUserXCmp(objCustMaster, lstrUserList);

                    objCustMaster.io_rate_id = pstrIORateId;
                    objCustMaster.io_rate_name = pstrIORateName;
                    objCustMaster.io_rate_price = Convert.ToDecimal( pdecIORatePrice);

                    objCustMaster.strg_rate_id = pstrStrgRateId;
                    objCustMaster.strg_rate_name = pstrStrgRateName;
                    objCustMaster.strg_rate_price = Convert.ToDecimal(pdecStrgRatePrice);

                   


                    if (p_str_whs_id != null)
                    {
                        ServiceObject.SaveWhsMaster(objCustMaster);
                    }
                    if (p_str_bl_free_days == null)
                    {
                        objCustMaster.bl_free_days = "";
                    }
                    else
                    {
                        objCustMaster.bl_free_days = p_str_bl_free_days.Trim();
                    }
                    objCustMaster.strg_bill = p_str_strg_bill.Trim();
                    objCustMaster.inout_bill = p_str_inout_bill.Trim();

                  

                    // p_str_bl_free_days.Trim();
                    if (objCustMaster.strg_bill == "")
                    {
                        objCustMaster.strg_bill = "Carton";
                    }
                    else
                    {
                        objCustMaster.strg_bill = p_str_strg_bill.Trim();
                    }
                    if (objCustMaster.inout_bill == "")
                    {
                        objCustMaster.inout_bill = "Carton";
                    }
                    else
                    {
                        objCustMaster.inout_bill = p_str_inout_bill.Trim();
                    }

                    ServiceObject.fnAddCustDfltRate(objCustMaster);

                    objCustMaster.allow_new_item = p_str_allow_new_item.Trim();
                    if (p_str_allow_new_item.Trim() == "Yes")
                    {
                        objCustMaster.allow_new_item = "Y";
                    }
                    else
                    {
                        objCustMaster.allow_new_item = "N";
                    }
                    objCustMaster.itemlistby = p_str_itm_listby.Trim();
                    if (p_str_itm_listby.Trim() == "ItemMaster")
                    {
                        objCustMaster.initstrg = "Item Master";
                    }
                    else
                    {
                        objCustMaster.initstrg = "Document Entry";
                    }
                    objCustMaster.autoincre = p_str_auto_inc.Trim();
                    if (p_str_auto_inc.Trim() == "Yes")
                    {
                        objCustMaster.autoincre = "Y";
                    }
                    else
                    {
                        objCustMaster.autoincre = "N";
                    }
                    objCustMaster.recv_lot_by = p_str_rcv_lot_by.Trim();
                    objCustMaster.initstrg = p_str_initstrg.Trim();
                    if (p_str_initstrg.Trim() == "Yes")
                    {
                        objCustMaster.initstrg = "Y";
                    }
                    else
                    {
                        objCustMaster.initstrg = "N";
                    }
                    //CR20180829-001 Added By Nithya
                    objCustMaster.Recv_non_doc_itm = p_str_recv_doc_itm.Trim();
                    if (p_str_recv_doc_itm.Trim() == "Yes")
                    {
                        objCustMaster.Recv_non_doc_itm = "Y";
                    }
                    else
                    {
                        objCustMaster.Recv_non_doc_itm = "N";
                    }
                    //END
                    //CR20180914-001 Added By Nithya
                    objCustMaster.Stk_Chk_Reqt = p_str_stk_chk_reqt.Trim();
                    if (p_str_stk_chk_reqt.Trim() == "Yes")
                    {
                        objCustMaster.Stk_Chk_Reqt = "Y";
                    }
                    else
                    {
                        objCustMaster.Stk_Chk_Reqt = "N";
                    }
                    //END
                    //CR20180914-001 Added By Nithya
                    objCustMaster.cube_auto_calc = p_str_cube_auto_cacl.Trim();
                    if (p_str_cube_auto_cacl.Trim() == "Yes")
                    {
                        objCustMaster.cube_auto_calc = "Y";

                       

                    }
                    else
                    {
                        objCustMaster.cube_auto_calc = "N";
                    }
                    //END

                    if (p_str_is_bill_by_cube_rounded != null)
                    { 
                        if (p_str_is_bill_by_cube_rounded.Trim() == "Yes")
                        {
                            objCustMaster.is_bill_by_cube_rounded = "Y";
                            objCustMaster.min_inout_cube = Convert.ToDecimal( p_str_min_inout_cube);
                            objCustMaster.min_strg_cube = Convert.ToDecimal(p_str_min_strg_cube);
                        }
                        else
                        {
                            objCustMaster.is_bill_by_cube_rounded = "N";
                            objCustMaster.min_inout_cube = 0;
                            objCustMaster.min_strg_cube = 0;
                        }
                    }
                    else
                    {
                        objCustMaster.is_bill_by_cube_rounded = "N";
                        objCustMaster.min_inout_cube = 0;
                        objCustMaster.min_strg_cube = 0;
                    }

                    objCustMaster.aloc_by = p_str_aloc_by.Trim();
                    objCustMaster.ecom_recv_by_bin = p_str_ecom_recv_by_bin;
                    objCustMaster.allow_940_new_item = p_str_allow_940_new_item;
                    objCustMaster.cust_type = p_str_cust_type;



                    objCustMaster.pmt_term = p_str_pmt_term;
                    ServiceObject.SaveCustMasterConfig(objCustMaster);
                    ServiceObject.SaveCustMasterConfigDir(objCustMaster);
                    objCustMaster = ServiceObject.GetTableEntityValueCount(objCustMaster);
                    objCustMaster.entity_count = objCustMaster.ListCheckEntityValue[0].entity_count;
                    l_str_lot_count = objCustMaster.entity_count;
                    if (l_str_lot_count == "0")
                    {
                        objCustMaster.entity_Code = "Lot_id";
                        ServiceObject.InsertTableEntityValueByCust(objCustMaster);
                    }
                    objCustMaster = ServiceObject.GetTableEntityValueCountByRMA_DocId(objCustMaster);
                    objCustMaster.entity_count_doc_id = objCustMaster.ListCheckEntityValueRmaDocId[0].entity_count_doc_id;
                    l_str_docid_count = objCustMaster.entity_count_doc_id;
                    if (l_str_docid_count == "0")
                    {
                        objCustMaster.entity_Code = "rma_doc_id";
                        ServiceObject.InsertTableEntityValueByCust(objCustMaster);
                    }

                    imgflag = 0;
                    return Json(imgflag, JsonRequestBehavior.AllowGet);

                    //Mapper.CreateMap<CustMaster, CustMasterModel>();
                    //CustMasterModel objCustMastermodel = Mapper.Map<CustMaster, CustMasterModel>(objCustMaster);
                    //return View("~/Views/CustMaster/CustMaster.cshtml", objCustMastermodel);
                }
                else
                {
                    return Json(objCustMaster.ListCheckExistCmpId.Count, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                string strException = ex.InnerException.ToString();
                return Json(null);
            }
        }

        public string SaveCustLogo(string p_str_cmp_id, string p_str_file_name)
        {
            string strSourceFolder = Path.Combine(Server.MapPath("~/Images/zfolderimage"));
            strSourceFolder = Path.Combine(strSourceFolder, p_str_cmp_id);
            System.IO.FileInfo l_str_file_cust_logo = new FileInfo(Path.Combine(strSourceFolder, p_str_file_name));

            if (l_str_file_cust_logo.Exists)
            {

                string imagePath = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["CustLogo"].ToString().Trim();
                string strDestinationFolder = Path.Combine(imagePath, p_str_cmp_id) + "/";

                if (Directory.Exists(strDestinationFolder))
                {
                    Directory.Delete(strDestinationFolder, true);
                }
                if (!Directory.Exists(strDestinationFolder))
                {
                    Directory.CreateDirectory(Path.Combine(strDestinationFolder));
                }

                l_str_file_cust_logo.CopyTo(Path.Combine(strDestinationFolder, p_str_file_name.Trim()), true);

                Directory.Delete(strSourceFolder, true);

                string newName = System.IO.Path.GetFileNameWithoutExtension(p_str_file_name);
                string lstrDestinationFolder = "/" + Path.Combine(System.Configuration.ConfigurationManager.AppSettings["CustLogo"].ToString().Trim(), p_str_cmp_id).ToString().Replace(@"\", @"/") + "/";
                return Path.Combine(lstrDestinationFolder, p_str_file_name);

            }
            else
            {

                return p_str_file_name;
            }
        }
        // Edit Customer Save
        public ActionResult UpdateCustDtls(string p_str_cmp_id, string p_str_cust_id, string p_str_cmp_name, string p_str_whs_id, string p_str_source, string p_str_region, string p_str_territory,
         string p_str_cust_grp_id, string p_str_status, string p_str_contact, string p_str_dt_of_entry,
          string p_str_last_chg_dt, string p_str_tel, string p_str_fax, string p_str_email, string p_str_web, string p_str_cell, string p_str_bl_free_days, string p_str_strg_bill, string p_str_inout_bill, string p_str_allow_new_item,
           string p_str_itm_listby, string p_str_auto_inc, string p_str_rcv_lot_by, string p_str_initstrg, string p_str_aloc_by, string p_str_mail_name, string p_str_attn, string p_str_addr1,
           string p_str_addr2, string p_str_city, string p_str_state, string p_str_zip, string p_str_country, string p_str_frieght_id, string p_str_ship_via_id, string p_str_catg, string p_str_bill_cycle, string p_str_credit_code,
           string p_str_credit_type, string p_str_credit_msg, string p_str_credit_limit, string p_str_discount, string p_str_terms_code, string p_str_order_value, string p_str_tax_code, string p_str_tax_exempt,
           string p_str_gl_num, string p_str_file_name, string p_str_recv_doc_itm, string p_str_stk_chk_reqt,
           string p_str_pmt_terms,string p_str_cube_auto_calc, string p_str_is_bill_by_cube_rounded, String p_str_min_inout_cube, 
           String p_str_min_strg_cube, string lstrUserList,
            string pstrIORateId, string pstrIORateName, string pdecIORatePrice, string pstrStrgRateId, string pstrStrgRateName, string pdecStrgRatePrice,
            bool p_str_ecom_recv_by_bin, bool p_str_allow_940_new_item, string p_str_cust_type)
        {
            string l_str_lot_count = string.Empty;
            string l_str_docid_count = string.Empty;
            int imgflag = 0;
            CustMaster objCustMaster = new CustMaster();
            ICustMasterService ServiceObject = new CustMasterService();
            objCustMaster.cust_of_cmp_id = p_str_cmp_id.Trim();
            objCustMaster.cmp_id = p_str_cust_id.Trim();
            objCustMaster.cmp_name = p_str_cmp_name.Trim();
            objCustMaster.cust_grp_id = p_str_cust_grp_id.Trim();
            objCustMaster.status = p_str_status.Trim();
            objCustMaster.dt_of_entry = p_str_dt_of_entry;
            objCustMaster.last_chg_dt = p_str_last_chg_dt.Trim();
            objCustMaster.Tel = p_str_tel.Trim();
            objCustMaster.cell = p_str_cell.Trim();
            objCustMaster.email = p_str_email.Trim();
            objCustMaster.fax = p_str_fax.Trim();
            objCustMaster.contact = p_str_contact.Trim();
            objCustMaster.web = p_str_web;
            objCustMaster.source = p_str_source.Trim();
            objCustMaster.whs_id = p_str_whs_id.Trim();
            objCustMaster.whs_name = p_str_whs_id.Trim();
            objCustMaster.region = p_str_region.Trim();
            objCustMaster.territory = p_str_territory.Trim();
            objCustMaster.insaleid = "";
            objCustMaster.outsaleid = "";
            //CR20180706-001 Added by Nithya
            if (p_str_file_name == "" || p_str_file_name == null || p_str_file_name.Trim() =="0")
            {
                objCustMaster.cust_logo = "";

            }
            else
            {
                objCustMaster.cust_logo = SaveCustLogo(p_str_cust_id, p_str_file_name);
                string strSourceFolder = Path.Combine(Server.MapPath("~/Images/Customer"));
                strSourceFolder = Path.Combine(strSourceFolder, p_str_cust_id);
                System.IO.FileInfo l_str_file_cust_logo = new FileInfo(Path.Combine(strSourceFolder, p_str_file_name.Substring(p_str_file_name.LastIndexOf('/') + 1)));
                if (!l_str_file_cust_logo.Exists)
                {
                    imgflag = 1;
                    return Json(imgflag, JsonRequestBehavior.AllowGet);
                }
            }
            //END
           
            ServiceObject.UpdateCustMaster(objCustMaster);
            ServiceObject.SaveUserXCmp(objCustMaster, lstrUserList);

            objCustMaster.mail_name = p_str_mail_name.Trim();
            objCustMaster.attn = p_str_attn.Trim();
            objCustMaster.addr1 = p_str_addr1.Trim();
            objCustMaster.addr2 = p_str_addr2.Trim();
            objCustMaster.city = p_str_city.Trim();
            objCustMaster.state = p_str_state.Trim();
            objCustMaster.zip = p_str_zip;
            objCustMaster.country = p_str_country.Trim();
            objCustMaster.frieght_id = p_str_frieght_id.Trim();
            objCustMaster.ship_via_id = p_str_ship_via_id.Trim();
            objCustMaster.catg = p_str_catg.Trim();
            objCustMaster.bl_cycle = p_str_bill_cycle.Trim();
            objCustMaster.code = p_str_credit_code.Trim();
            objCustMaster.type = p_str_credit_type.Trim();
            objCustMaster.msg = p_str_credit_msg.Trim();
            objCustMaster.credit_lt = p_str_credit_limit.Trim();
            objCustMaster.disc = p_str_discount.Trim();
            objCustMaster.term_code = p_str_terms_code;
            objCustMaster.ord_val = p_str_order_value.Trim();
            objCustMaster.tax_code = p_str_tax_code.Trim();
            objCustMaster.tax_exempt = p_str_tax_exempt.Trim();
            objCustMaster.gl_num = p_str_gl_num.Trim();

            // objCustMaster.cust_logo = p_str_file_name;
            ServiceObject.UpdateCustMasterDtl(objCustMaster);
            ServiceObject.UpdateCmpHdr(objCustMaster);
            objCustMaster.bl_free_days = p_str_bl_free_days.Trim();
            objCustMaster.strg_bill = p_str_strg_bill.Trim();
            objCustMaster.inout_bill = p_str_inout_bill;
            objCustMaster.allow_new_item = p_str_allow_new_item.Trim();
            if (p_str_allow_new_item.Trim() == "Yes")
            {
                objCustMaster.allow_new_item = "Y";
            }
            else
            {
                objCustMaster.allow_new_item = "N";
            }
            objCustMaster.itemlistby = p_str_itm_listby.Trim();
            if (p_str_itm_listby.Trim() == "DocEntry")
            {
                objCustMaster.itemlistby = "Document Entry";
            }
            else
            {
                objCustMaster.itemlistby = "Item Master";
            }
            objCustMaster.autoincre = p_str_auto_inc.Trim();
            if (p_str_auto_inc.Trim() == "Yes")
            {
                objCustMaster.autoincre = "Y";
            }
            else
            {
                objCustMaster.autoincre = "N";
            }
            objCustMaster.recv_lot_by = p_str_rcv_lot_by.Trim();
            objCustMaster.initstrg = p_str_initstrg.Trim();
            if (p_str_initstrg.Trim() == "Yes")
            {
                objCustMaster.initstrg = "Y";
            }
            else
            {
                objCustMaster.initstrg = "N";
            }
            //CR20180829-001 Added By Nithya
            objCustMaster.Recv_non_doc_itm = p_str_recv_doc_itm.Trim();
            if (p_str_recv_doc_itm.Trim() == "Yes")
            {
                objCustMaster.Recv_non_doc_itm = "Y";
            }
            else
            {
                objCustMaster.Recv_non_doc_itm = "N";
            }
            //END   
            //CR20180914-001 Added By Nithya
            objCustMaster.Stk_Chk_Reqt = p_str_stk_chk_reqt.Trim();
            if (p_str_stk_chk_reqt.Trim() == "Yes")
            {
                objCustMaster.Stk_Chk_Reqt = "Y";
            }
            else
            {
                objCustMaster.Stk_Chk_Reqt = "N";
            }
            //END    
            //CR20180914-001 Added By Nithya
            objCustMaster.cube_auto_calc = p_str_cube_auto_calc.Trim();
            if (p_str_cube_auto_calc.Trim() == "Yes")
            {
                objCustMaster.cube_auto_calc = "Y";
            }
            else
            {
                objCustMaster.cube_auto_calc = "N";
            }

            if (p_str_is_bill_by_cube_rounded.Trim() == "Yes")
            {
                objCustMaster.is_bill_by_cube_rounded = "Y";
                objCustMaster.min_inout_cube = Convert.ToDecimal(p_str_min_inout_cube);
                objCustMaster.min_strg_cube = Convert.ToDecimal(p_str_min_strg_cube);
            }
            else
            {
                objCustMaster.is_bill_by_cube_rounded = "N";
                objCustMaster.min_inout_cube = Convert.ToDecimal(p_str_min_inout_cube);
                objCustMaster.min_strg_cube = Convert.ToDecimal(p_str_min_strg_cube);
            }
            objCustMaster.io_rate_id = pstrIORateId;
            objCustMaster.io_rate_name = pstrIORateName;
            objCustMaster.io_rate_price = Convert.ToDecimal(pdecIORatePrice);

            objCustMaster.strg_rate_id = pstrStrgRateId;
            objCustMaster.strg_rate_name = pstrStrgRateName;
            objCustMaster.strg_rate_price = Convert.ToDecimal(pdecStrgRatePrice);
          //  ServiceObject.fnAddCustDfltRate(objCustMaster);

            //END    

            objCustMaster.aloc_by = p_str_aloc_by.Trim();
            objCustMaster.ecom_recv_by_bin = p_str_ecom_recv_by_bin;
            objCustMaster.allow_940_new_item = p_str_allow_940_new_item;

            objCustMaster.cust_type = p_str_cust_type;

            objCustMaster.pmt_term = p_str_pmt_terms;            
            if (p_str_whs_id != null)
            {
                ServiceObject.SaveWhsMaster(objCustMaster);
            }
            ServiceObject.UpdateCustMasterConfig(objCustMaster);
            ServiceObject.UpdateCustMasterConfigDir(objCustMaster);
            objCustMaster = ServiceObject.GetTableEntityValueCount(objCustMaster);
            objCustMaster.entity_count = objCustMaster.ListCheckEntityValue[0].entity_count;
            l_str_lot_count = objCustMaster.entity_count;
            if (l_str_lot_count == "0")
            {
                objCustMaster.entity_Code = "Lot_id";
               // ServiceObject.InsertTableEntityValueByCust(objCustMaster);
            }
            objCustMaster = ServiceObject.GetTableEntityValueCountByRMA_DocId(objCustMaster);
            objCustMaster.entity_count_doc_id = objCustMaster.ListCheckEntityValueRmaDocId[0].entity_count_doc_id;
            l_str_docid_count = objCustMaster.entity_count_doc_id;
            if (l_str_docid_count == "0")
            {
                objCustMaster.entity_Code = "rma_doc_id";
               // ServiceObject.InsertTableEntityValueByCust(objCustMaster);
            }
            Mapper.CreateMap<CustMaster, CustMasterModel>();
            CustMasterModel objCustMastermodel = Mapper.Map<CustMaster, CustMasterModel>(objCustMaster);
            return View("~/Views/CustMaster/CustMaster.cshtml", objCustMastermodel);

        }
        //  Country Change Event - To display the states based on the selectefd country
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

        // Save Inquiry Section in session To reload
        public JsonResult SaveInqSecInSession(string p_str_cmp_id, string p_str_cust_id)
        {
            CustMaster objCustMaster = new CustMaster();
            ICustMasterService ServiceObject = new CustMasterService();
            Session["TEMP_CUST_ID"] = p_str_cust_id.Trim();
            Session["TEMP_CMP_ID"] = p_str_cmp_id.Trim();
            return Json(objCustMaster.MasterCount, JsonRequestBehavior.AllowGet);

        }

        // Customer - Inquiry Referesh
        public ActionResult CustMasterDtl(string p_str_cmp_id, string p_str_cust_id)
        {
            try
            {

                CustMaster objCustMaster = new CustMaster();
                ICustMasterService ServiceObject = new CustMasterService();
                string l_str_search_flag = string.Empty;
                string l_str_is_another_usr = string.Empty;

                //l_str_is_another_usr = Session["isanother"].ToString();
                //objRateMaster.IsAnotherUser = l_str_is_another_usr.Trim();
                l_str_search_flag = Session["g_str_Search_flag"].ToString().Trim();
                //objCustMaster.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                if (l_str_search_flag == "True")
                {
                    //objRateMaster.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                    objCustMaster.cust_of_cmp_id = Session["TEMP_CMP_ID"].ToString();
                    objCustMaster.cmp_id = Session["TEMP_CUST_ID"].ToString();
                    objCustMaster.user_id = Session["UserID"].ToString();

                }
                else
                {
                    objCustMaster.cust_of_cmp_id = p_str_cmp_id;
                    objCustMaster.cmp_id = p_str_cust_id.Trim();
                    objCustMaster.user_id = Session["UserID"].ToString().Trim();


                }

                objCustMaster = ServiceObject.GetCustMasterDetails(objCustMaster);
                Mapper.CreateMap<CustMaster, CustMasterModel>();
                CustMasterModel objCustMastermodel = Mapper.Map<CustMaster, CustMasterModel>(objCustMaster);
                return PartialView("_CustMaster", objCustMastermodel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // Upload Customer logo
        public ActionResult UploadFiles(string l_str_cmp_id)
        {
            int l_str_ResultCount = 0;
            string l_str_tmp_directoryPath = string.Empty;
            string l_str_tmp_file_name = string.Empty;
            string l_str_tmp_full_file_path = string.Empty;
            CustMaster objCustMaster = new CustMaster();
            ICustMasterService ServiceObject = new CustMasterService();
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase FileUpload = files[0];

                    string l_str_error_msg = string.Empty;
                    if (FileUpload != null)
                    {
                        if (FileUpload.ContentLength > 0)
                        {
                            //Workout our file path
                            l_str_tmp_file_name = Path.GetFileName(FileUpload.FileName);
                            l_str_tmp_directoryPath = Path.Combine(Server.MapPath("~/Images/zfolderimage"), l_str_cmp_id);
                            if (Directory.Exists(l_str_tmp_directoryPath))
                            {
                                Directory.Delete(l_str_tmp_directoryPath, true);

                            }
                            if (!Directory.Exists(l_str_tmp_directoryPath))
                            {
                                Directory.CreateDirectory(Path.Combine(l_str_tmp_directoryPath));
                            }
                            l_str_tmp_full_file_path = Path.Combine(l_str_tmp_directoryPath, l_str_tmp_file_name);
                            try
                            {
                                l_str_ResultCount = 1;

                                FileUpload.SaveAs(l_str_tmp_full_file_path);
                            }
                            catch (Exception ex)
                            {
                                return Json(l_str_ResultCount, JsonRequestBehavior.AllowGet);

                            }

                        }

                    }
                    else
                    {
                        return Json(l_str_ResultCount, JsonRequestBehavior.AllowGet);

                    }





                }
                catch (Exception ex)
                {
                    return Json(l_str_ResultCount, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                return Json(l_str_ResultCount, JsonRequestBehavior.AllowGet);
            }
            return Json(l_str_ResultCount, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowReport(string p_str_cmpid, string p_str_cust_id, string p_str_report_selection_name,string p_str_report_type)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
           
            try
            {
                if (isValid)
                {

                    if (p_str_report_selection_name == "CustomerDetails")
                    {
                        if (p_str_report_type == "PDF")
                        {
                            strReportName = "rpt_ma_cust_dtl.rpt";
                        }
                        else
                        {
                            strReportName = "rpt_ma_cust_dtl_Excel.rpt";
                        }
                        CustMaster objCustMaster = new CustMaster();
                        ICustMasterService ServiceObject = new CustMasterService();
                       
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Master//" + strReportName;
                        objCustMaster.cmp_id = (p_str_cmpid == null) ? string.Empty : p_str_cmpid.Trim();
                        objCustMaster.cust_id = (p_str_cust_id == null) ? string.Empty : p_str_cust_id.Trim();
                        objCustMaster.type = (p_str_report_type == null) ? string.Empty : p_str_report_type.Trim();
                        if(Session["UserID"]!=null)
                        {
                           objCustMaster.user_id = Session["UserID"].ToString().Trim();
                        }
                        objCustMaster = ServiceObject.GetCustMasterRptDetails(objCustMaster);
                        var rptSource = objCustMaster.ListCustDetails.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objCustMaster.ListCustDetails.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                if (p_str_report_type == "PDF")
                                {
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                                else
                                {
                                    rd.ExportToHttpResponse(ExportFormatType.Excel, System.Web.HttpContext.Current.Response, false, "crReport");
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

    }
}