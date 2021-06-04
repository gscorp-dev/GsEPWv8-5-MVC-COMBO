using AutoMapper;
using GsEPWv8_4_MVC.Business.Implementation;
using GsEPWv8_4_MVC.Business.Interface;
using GsEPWv8_4_MVC.Core.Entity;
using GsEPWv8_4_MVC.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

#region Change History
//CR_3PL_MVC_BL_2018_0316_001  SONIYA     20180316  TO SET THE DEFAULT CMP ID
#endregion Change History

namespace GsEPWv8_4_MVC.Controllers
{
    public class CustMasterController : Controller
    {
        // GET: CustMaster
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
              
               
                            if (objCustMaster.cmp_id != "" && FullFillType == null)
                            {

                                objCompany.cmp_id = objCustMaster.cmp_id;
                                objCompany.user_id = Session["UserID"].ToString().Trim();
                                objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                                objCustMaster.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                                objCompany.cust_of_cmp_id = "";
                                objCompany = ServiceObjectCompany.GetCustOfCompName(objCompany);
                                objCustMaster.LstCustOfCmpName = objCompany.LstCustOfCmpName;
                                objCustMaster.cust_of_cmpid = objCustMaster.LstCustOfCmpName[0].cust_of_cmpid;
                                objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
                                objCustMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
                                //objCustMaster = ServiceObject.GetCustMasterDetails(objCustMaster);
                                DateTime date = DateTime.Now;
                                l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");
                                objCustMaster.dt_of_entry = l_str_fm_dt;
                                objCustMaster.last_chg_dt = "";

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
                                    objCustMaster.cust_of_cmpid = objCustMaster.LstCustOfCmpName[0].cust_of_cmpid;
                                    objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
                                    objCustMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
                                    //objCustMaster = ServiceObject.GetCustMasterDetails(objCustMaster);
                                    DateTime date = DateTime.Now;
                                    l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");
                                    objCustMaster.dt_of_entry = l_str_fm_dt;
                                    objCustMaster.last_chg_dt = "";
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
               
        public ActionResult New(string cmpid)
        {

            CustMaster objCustMaster = new CustMaster();
            ICustMasterService ServiceObject = new CustMasterService();
            objCustMaster.cmp_id = cmpid;
            Session["tempcmpid"] = objCustMaster.cmp_id;
            objCustMaster.source = "-";
            objCustMaster.territory = "-";
            objCustMaster.credit_lt = "0.0";
            objCustMaster.term_code = "1";
            objCustMaster.cmp_id = cmpid;
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            //CR-3PL_MVC_BL_2018_0316_001 Added by Soniya      
            objCompany.cust_of_cmp_id = "";            
            objCompany.cmp_id = cmpid;
            objCompany = ServiceObjectCompany.GetCustOfCompName(objCompany);
            objCustMaster.LstCustOfCmpName = objCompany.LstCustOfCmpName;
            objCustMaster.cust_of_cmpid = objCustMaster.LstCustOfCmpName[0].cust_of_cmpid;
            objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
            objCustMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
            //CR-3PL_MVC_BL_2018_0316_001 End
            //objCompany.user_id = Session["UserID"].ToString().Trim();
            //objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            //objCustMaster.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
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
            objPick.cmp_id = cmpid;
            objPick.Whs_id = "";
            objPick.Whs_name = "";
            objPick = ServiceObjectPick.GetWhsPick(objPick);
            objCustMaster.ListPick = objPick.ListPick;
            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objCustMaster.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objCustMaster.ListStatePick = objPick.ListStatePick;
            //objCustMaster.cmp_id = cmpid;
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
            Mapper.CreateMap<CustMaster, CustMasterModel>();
            CustMasterModel objCustMastermodel = Mapper.Map<CustMaster, CustMasterModel>(objCustMaster);
            return PartialView("_AddCustMaster", objCustMastermodel);
        }

        public ActionResult Edit(string cust_of_cmp_id, string cmp_id)
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
            objCustMaster.status = (objCustMaster.ListCustHdr[0].status.Trim() == null || objCustMaster.ListCustHdr[0].status.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].status.Trim();
            objCustMaster.dt_of_entry = (objCustMaster.ListCustHdr[0].start_dt == null || objCustMaster.ListCustHdr[0].start_dt == "" ? string.Empty : Convert.ToDateTime(objCustMaster.ListCustHdr[0].start_dt).ToString("MM/dd/yyyy")).Trim();
            objCustMaster.last_chg_dt = (objCustMaster.ListCustHdr[0].last_chg_dt == null || objCustMaster.ListCustHdr[0].last_chg_dt == "" ? string.Empty : Convert.ToDateTime(objCustMaster.ListCustHdr[0].last_chg_dt).ToString("MM/dd/yyyy")); 
            objCustMaster.Tel = (objCustMaster.ListCustHdr[0].tel.Trim() == null || objCustMaster.ListCustHdr[0].tel.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].tel.Trim();
            objCustMaster.cell = (objCustMaster.ListCustHdr[0].cell.Trim() == null || objCustMaster.ListCustHdr[0].cell.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].cell.Trim();
            objCustMaster.contact = (objCustMaster.ListCustHdr[0].contact.Trim() == null || objCustMaster.ListCustHdr[0].contact.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].contact.Trim();
            objCustMaster.email = (objCustMaster.ListCustHdr[0].email.Trim() == null || objCustMaster.ListCustHdr[0].email.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].email.Trim();
            objCustMaster.fax = (objCustMaster.ListCustHdr[0].fax.Trim() == null || objCustMaster.ListCustHdr[0].fax.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].fax.Trim();
            objCustMaster.web = (objCustMaster.ListCustHdr[0].web.Trim() == null || objCustMaster.ListCustHdr[0].web.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].web.Trim();
            objCustMaster.source = (objCustMaster.ListCustHdr[0].source.Trim() == null || objCustMaster.ListCustHdr[0].source.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].source.Trim();
            objCustMaster.whs_id = (objCustMaster.ListCustHdr[0].dft_whs.Trim() == null || objCustMaster.ListCustHdr[0].dft_whs.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].dft_whs.Trim();
            objCustMaster.mail_name = (objCustMaster.ListCustDtl[0].mail_name.Trim() == null || objCustMaster.ListCustDtl[0].mail_name.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].mail_name.Trim();
            objCustMaster.attn = (objCustMaster.ListCustDtl[0].attn.Trim() == null || objCustMaster.ListCustDtl[0].attn.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].attn.Trim();
            objCustMaster.addr1 = (objCustMaster.ListCustDtl[0].addr_line1.Trim() == null || objCustMaster.ListCustDtl[0].addr_line1.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].addr_line1.Trim();
            objCustMaster.addr2 = (objCustMaster.ListCustDtl[0].addr_line2.Trim() == null || objCustMaster.ListCustDtl[0].addr_line2.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].addr_line2.Trim();
            objCustMaster.city = (objCustMaster.ListCustDtl[0].city.Trim() == null || objCustMaster.ListCustDtl[0].city.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].city.Trim();
            objCustMaster.state = (objCustMaster.ListCustDtl[0].state_id.Trim() == null || objCustMaster.ListCustDtl[0].state_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].state_id.Trim();
            objCustMaster.zip = (objCustMaster.ListCustDtl[0].post_code.Trim() == null || objCustMaster.ListCustDtl[0].post_code.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].post_code.Trim();
            objCustMaster.country = (objCustMaster.ListCustDtl[0].cntry_id.Trim() == null || objCustMaster.ListCustDtl[0].cntry_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].cntry_id.Trim();
            objCustMaster.frieght_id = (objCustMaster.ListCustDtl[0].freight_id.Trim() == null || objCustMaster.ListCustDtl[0].freight_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].freight_id.Trim();
            objCustMaster.ship_via_id = (objCustMaster.ListCustDtl[0].ship_via_id.Trim() == null || objCustMaster.ListCustDtl[0].ship_via_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].ship_via_id.Trim();
            objCustMaster.catg = (objCustMaster.ListCustDtl[0].cust_catg.Trim() == null || objCustMaster.ListCustDtl[0].cust_catg.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].cust_catg.Trim();
            objCustMaster.bl_cycle = (objCustMaster.ListCustDtl[0].bill_cycle.Trim() == null || objCustMaster.ListCustDtl[0].bill_cycle.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].bill_cycle.Trim();
            objCustMaster.code = (objCustMaster.ListCustDtl[0].crdt_code.Trim() == null || objCustMaster.ListCustDtl[0].crdt_code.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_code.Trim();
            objCustMaster.type = (objCustMaster.ListCustDtl[0].crdt_chck.Trim() == null || objCustMaster.ListCustDtl[0].crdt_chck.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_chck.Trim();
            objCustMaster.msg = (objCustMaster.ListCustDtl[0].crdt_msg.Trim() == null || objCustMaster.ListCustDtl[0].crdt_msg.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_msg.Trim();
            objCustMaster.credit_lt = (objCustMaster.ListCustDtl[0].crdt_limit.Trim() == null || objCustMaster.ListCustDtl[0].crdt_limit.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_limit.Trim();
            objCustMaster.disc = (objCustMaster.ListCustDtl[0].disc.Trim() == null || objCustMaster.ListCustDtl[0].disc.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].disc.Trim();
            objCustMaster.term_code = (objCustMaster.ListCustDtl[0].terms_id.Trim() == null || objCustMaster.ListCustDtl[0].terms_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].terms_id.Trim();
            objCustMaster.ord_val = (objCustMaster.ListCustDtl[0].ordr_value.Trim() == null || objCustMaster.ListCustDtl[0].ordr_value.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].ordr_value.Trim();
            objCustMaster.tax_code = (objCustMaster.ListCustDtl[0].tax_exempt_id.Trim() == null || objCustMaster.ListCustDtl[0].tax_exempt_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].tax_exempt_id.Trim();
            objCustMaster.tax_exempt = (objCustMaster.ListCustDtl[0].tax_exempt.Trim() == null || objCustMaster.ListCustDtl[0].tax_exempt.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].tax_exempt.Trim();
            objCustMaster.gl_num = (objCustMaster.ListCustDtl[0].gl_num.Trim() == null || objCustMaster.ListCustDtl[0].gl_num.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].gl_num.Trim();
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
            objCustMaster.bl_free_days = (objCustMaster.ListCustConfig[0].bill_free_days.Trim() == null) ? "" : objCustMaster.ListCustConfig[0].bill_free_days.Trim();
            objCustMaster.strg_bill = objCustMaster.ListCustConfig[0].bill_type.Trim();
            objLookUp.id = "16";
            objLookUp.lookuptype = "CUSTOMERMASTER";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
            objLookUp.id = "10";
            objLookUp.lookuptype = "STRG";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListSrtg = objLookUp.ListLookUpDtl;
            objCustMaster.inout_bill = objCustMaster.ListCustConfig[0].bill_inout_type.Trim();
            objLookUp.id = "9";
            objLookUp.lookuptype = "INOUT";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListInout = objLookUp.ListLookUpDtl;
            objCustMaster.allow_new_item = objCustMaster.ListCustConfig[0].Allow_New_item.Trim();
            if(objCustMaster.ListCustConfig[0].Allow_New_item.Trim() == "Y")
            {
                objCustMaster.allow_new_item = "Yes";
            }
            else
            {
                objCustMaster.allow_new_item = "No";
            }
            objLookUp.id = "19";
            objLookUp.lookuptype = "DocAllowNewItem";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListDocAllowNewItm = objLookUp.ListLookUpDtl;
            objCustMaster.itemlistby = objCustMaster.ListCustConfig[0].item_pick;
            if (objCustMaster.ListCustConfig[0].item_pick.Trim() == "Document Entry")
            {
                objCustMaster.itemlistby = "DocEntry";
            }
            else
            {
                objCustMaster.itemlistby = "ItemMaster";
            }
            objLookUp.id = "20";
            objLookUp.lookuptype = "ItemListBy";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListItmListBy = objLookUp.ListLookUpDtl;
            //if (objCustMaster.itemlistby == "Document Entry")
            //{
            //    objCustMaster.itemlistby = "Yes";
            //} 
          
            if(objCustMaster.ListCustConfig[0].init_strg_rt_req == "Y")
            {
                objCustMaster.initstrg = "Yes";
            }  
            else
            {
                objCustMaster.initstrg = "No";
            }
            objLookUp.id = "22";
            objLookUp.lookuptype = "IncludeInitStrg";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListIncludeInitStrg = objLookUp.ListLookUpDtl;
            objCustMaster.autoincre = objCustMaster.ListCustConfig[0].Doc_Increment;
            if (objCustMaster.ListCustConfig[0].Doc_Increment == "Y")
            {
                objCustMaster.autoincre = "Yes";
            }
            else
            {
                objCustMaster.autoincre = "No";
            }
            objLookUp.id = "23";
            objLookUp.lookuptype = "DocAutoIncrement";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListAutoIncrement = objLookUp.ListLookUpDtl;
            objCustMaster.recv_lot_by = objCustMaster.ListCustConfig[0].Recv_Itm_Mode;
            objLookUp.id = "17";
            objLookUp.lookuptype = "CUSTRCVLOTBY";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListRcvLot = objLookUp.ListLookUpDtl;
            objCustMaster.aloc_by = objCustMaster.ListCustConfig[0].Aloc_by.Trim();
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
            Mapper.CreateMap<CustMaster, CustMasterModel>();
            CustMasterModel objCustMastermodel = Mapper.Map<CustMaster, CustMasterModel>(objCustMaster);
            return PartialView("_CustEdit", objCustMastermodel);
        }
        public ActionResult Views(string cust_of_cmp_id, string cmp_id)
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
            objCustMaster.status = (objCustMaster.ListCustHdr[0].status.Trim() == null || objCustMaster.ListCustHdr[0].status.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].status.Trim();
            objCustMaster.dt_of_entry = (objCustMaster.ListCustHdr[0].start_dt == null || objCustMaster.ListCustHdr[0].start_dt == "" ? string.Empty : Convert.ToDateTime(objCustMaster.ListCustHdr[0].start_dt).ToString("MM/dd/yyyy")).Trim();
            objCustMaster.last_chg_dt = (objCustMaster.ListCustHdr[0].last_chg_dt == null || objCustMaster.ListCustHdr[0].last_chg_dt == "" ? string.Empty : Convert.ToDateTime(objCustMaster.ListCustHdr[0].last_chg_dt).ToString("MM/dd/yyyy"));
            objCustMaster.Tel = (objCustMaster.ListCustHdr[0].tel.Trim() == null || objCustMaster.ListCustHdr[0].tel.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].tel.Trim();
            objCustMaster.cell = (objCustMaster.ListCustHdr[0].cell.Trim() == null || objCustMaster.ListCustHdr[0].cell.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].cell.Trim();
            objCustMaster.contact = (objCustMaster.ListCustHdr[0].contact.Trim() == null || objCustMaster.ListCustHdr[0].contact.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].contact.Trim();
            objCustMaster.email = (objCustMaster.ListCustHdr[0].email.Trim() == null || objCustMaster.ListCustHdr[0].email.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].email.Trim();
            objCustMaster.fax = (objCustMaster.ListCustHdr[0].fax.Trim() == null || objCustMaster.ListCustHdr[0].fax.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].fax.Trim();
            objCustMaster.web = (objCustMaster.ListCustHdr[0].web.Trim() == null || objCustMaster.ListCustHdr[0].web.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].web.Trim();
            objCustMaster.source = (objCustMaster.ListCustHdr[0].source.Trim() == null || objCustMaster.ListCustHdr[0].source.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].source.Trim();
            objCustMaster.whs_id = (objCustMaster.ListCustHdr[0].dft_whs.Trim() == null || objCustMaster.ListCustHdr[0].dft_whs.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].dft_whs.Trim();

            objCustMaster.mail_name = (objCustMaster.ListCustDtl[0].mail_name.Trim() == null || objCustMaster.ListCustDtl[0].mail_name.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].mail_name.Trim();
            objCustMaster.attn = (objCustMaster.ListCustDtl[0].attn.Trim() == null || objCustMaster.ListCustDtl[0].attn.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].attn.Trim();
            objCustMaster.addr1 = (objCustMaster.ListCustDtl[0].addr_line1.Trim() == null || objCustMaster.ListCustDtl[0].addr_line1.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].addr_line1.Trim();
            objCustMaster.addr2 = (objCustMaster.ListCustDtl[0].addr_line2.Trim() == null || objCustMaster.ListCustDtl[0].addr_line2.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].addr_line2.Trim();
            objCustMaster.city = (objCustMaster.ListCustDtl[0].city.Trim() == null || objCustMaster.ListCustDtl[0].city.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].city.Trim();
            objCustMaster.state = (objCustMaster.ListCustDtl[0].state_id.Trim() == null || objCustMaster.ListCustDtl[0].state_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].state_id.Trim();
            objCustMaster.zip = (objCustMaster.ListCustDtl[0].post_code.Trim() == null || objCustMaster.ListCustDtl[0].post_code.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].post_code.Trim();
            objCustMaster.country = (objCustMaster.ListCustDtl[0].cntry_id.Trim() == null || objCustMaster.ListCustDtl[0].cntry_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].cntry_id.Trim();
            objCustMaster.frieght_id = (objCustMaster.ListCustDtl[0].freight_id.Trim() == null || objCustMaster.ListCustDtl[0].freight_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].freight_id.Trim();
            objCustMaster.ship_via_id = (objCustMaster.ListCustDtl[0].ship_via_id.Trim() == null || objCustMaster.ListCustDtl[0].ship_via_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].ship_via_id.Trim();
            objCustMaster.catg = (objCustMaster.ListCustDtl[0].cust_catg.Trim() == null || objCustMaster.ListCustDtl[0].cust_catg.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].cust_catg.Trim();
            objCustMaster.bl_cycle = (objCustMaster.ListCustDtl[0].bill_cycle.Trim() == null || objCustMaster.ListCustDtl[0].bill_cycle.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].bill_cycle.Trim();
            objCustMaster.code = (objCustMaster.ListCustDtl[0].crdt_code.Trim() == null || objCustMaster.ListCustDtl[0].crdt_code.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_code.Trim();
            objCustMaster.type = (objCustMaster.ListCustDtl[0].crdt_chck.Trim() == null || objCustMaster.ListCustDtl[0].crdt_chck.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_chck.Trim();
            objCustMaster.msg = (objCustMaster.ListCustDtl[0].crdt_msg.Trim() == null || objCustMaster.ListCustDtl[0].crdt_msg.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_msg.Trim();
            objCustMaster.credit_lt = (objCustMaster.ListCustDtl[0].crdt_limit.Trim() == null || objCustMaster.ListCustDtl[0].crdt_limit.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_limit.Trim();
            objCustMaster.disc = (objCustMaster.ListCustDtl[0].disc.Trim() == null || objCustMaster.ListCustDtl[0].disc.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].disc.Trim();
            objCustMaster.term_code = (objCustMaster.ListCustDtl[0].terms_id.Trim() == null || objCustMaster.ListCustDtl[0].terms_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].terms_id.Trim();
            objCustMaster.ord_val = (objCustMaster.ListCustDtl[0].ordr_value.Trim() == null || objCustMaster.ListCustDtl[0].ordr_value.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].ordr_value.Trim();
            objCustMaster.tax_code = (objCustMaster.ListCustDtl[0].tax_exempt_id.Trim() == null || objCustMaster.ListCustDtl[0].tax_exempt_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].tax_exempt_id.Trim();
            objCustMaster.tax_exempt = (objCustMaster.ListCustDtl[0].tax_exempt.Trim() == null || objCustMaster.ListCustDtl[0].tax_exempt.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].tax_exempt.Trim();
            objCustMaster.gl_num = (objCustMaster.ListCustDtl[0].gl_num.Trim() == null || objCustMaster.ListCustDtl[0].gl_num.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].gl_num.Trim();
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
            objCustMaster.region = objCustMaster.ListCustHdr[0].regn_id.Trim();
            objCustMaster.territory = objCustMaster.ListCustHdr[0].tery_id.Trim();
            objCustMaster = ServiceObject.GetCustConfigDetails(objCustMaster);
            objCustMaster.bl_free_days = (objCustMaster.ListCustConfig[0].bill_free_days.Trim() == null) ? "" : objCustMaster.ListCustConfig[0].bill_free_days.Trim();
            objCustMaster.strg_bill = objCustMaster.ListCustConfig[0].bill_type.Trim();
            objLookUp.id = "16";
            objLookUp.lookuptype = "CUSTOMERMASTER";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
            objLookUp.id = "10";
            objLookUp.lookuptype = "STRG";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListSrtg = objLookUp.ListLookUpDtl;
            objCustMaster.inout_bill = objCustMaster.ListCustConfig[0].bill_inout_type.Trim();
            objLookUp.id = "9";
            objLookUp.lookuptype = "INOUT";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListInout = objLookUp.ListLookUpDtl;
            objCustMaster.allow_new_item = objCustMaster.ListCustConfig[0].Allow_New_item.Trim();
            if (objCustMaster.ListCustConfig[0].Allow_New_item.Trim() == "Y")
            {
                objCustMaster.allow_new_item = "Yes";
            }
            else
            {
                objCustMaster.allow_new_item = "No";
            }
            objLookUp.id = "19";
            objLookUp.lookuptype = "DocAllowNewItem";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListDocAllowNewItm = objLookUp.ListLookUpDtl;
            objCustMaster.itemlistby = objCustMaster.ListCustConfig[0].item_pick;
            if (objCustMaster.ListCustConfig[0].item_pick.Trim() == "Document Entry")
            {
                objCustMaster.itemlistby = "DocEntry";
            }
            else
            {
                objCustMaster.itemlistby = "ItemMaster";
            }
            objLookUp.id = "20";
            objLookUp.lookuptype = "ItemListBy";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListItmListBy = objLookUp.ListLookUpDtl;
            //if (objCustMaster.itemlistby == "Document Entry")
            //{
            //    objCustMaster.itemlistby = "Yes";
            //} 

            if (objCustMaster.ListCustConfig[0].init_strg_rt_req == "Y")
            {
                objCustMaster.initstrg = "Yes";
            }
            else
            {
                objCustMaster.initstrg = "No";
            }
            objLookUp.id = "22";
            objLookUp.lookuptype = "IncludeInitStrg";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListIncludeInitStrg = objLookUp.ListLookUpDtl;
            objCustMaster.autoincre = objCustMaster.ListCustConfig[0].Doc_Increment;
            if (objCustMaster.ListCustConfig[0].Doc_Increment == "Y")
            {
                objCustMaster.autoincre = "Yes";
            }
            else
            {
                objCustMaster.autoincre = "No";
            }
            objLookUp.id = "23";
            objLookUp.lookuptype = "DocAutoIncrement";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListAutoIncrement = objLookUp.ListLookUpDtl;
            objCustMaster.recv_lot_by = objCustMaster.ListCustConfig[0].Recv_Itm_Mode;
            objLookUp.id = "17";
            objLookUp.lookuptype = "CUSTRCVLOTBY";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListRcvLot = objLookUp.ListLookUpDtl;
            objCustMaster.aloc_by = objCustMaster.ListCustConfig[0].Aloc_by.Trim();
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
            Mapper.CreateMap<CustMaster, CustMasterModel>();
            CustMasterModel objCustMastermodel = Mapper.Map<CustMaster, CustMasterModel>(objCustMaster);
            return PartialView("_CustView", objCustMastermodel);
        }
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
            objCustMaster.status = (objCustMaster.ListCustHdr[0].status.Trim() == null || objCustMaster.ListCustHdr[0].status.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].status.Trim();
            objCustMaster.dt_of_entry = (objCustMaster.ListCustHdr[0].start_dt == null || objCustMaster.ListCustHdr[0].start_dt == "" ? string.Empty : Convert.ToDateTime(objCustMaster.ListCustHdr[0].start_dt).ToString("MM/dd/yyyy")).Trim();
            objCustMaster.last_chg_dt = (objCustMaster.ListCustHdr[0].last_chg_dt == null || objCustMaster.ListCustHdr[0].last_chg_dt == "" ? string.Empty : Convert.ToDateTime(objCustMaster.ListCustHdr[0].last_chg_dt).ToString("MM/dd/yyyy"));
            objCustMaster.Tel = (objCustMaster.ListCustHdr[0].tel.Trim() == null || objCustMaster.ListCustHdr[0].tel.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].tel.Trim();
            objCustMaster.cell = (objCustMaster.ListCustHdr[0].cell.Trim() == null || objCustMaster.ListCustHdr[0].cell.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].cell.Trim();
            objCustMaster.contact = (objCustMaster.ListCustHdr[0].contact.Trim() == null || objCustMaster.ListCustHdr[0].contact.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].contact.Trim();
            objCustMaster.email = (objCustMaster.ListCustHdr[0].email.Trim() == null || objCustMaster.ListCustHdr[0].email.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].email.Trim();
            objCustMaster.fax = (objCustMaster.ListCustHdr[0].fax.Trim() == null || objCustMaster.ListCustHdr[0].fax.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].fax.Trim();
            objCustMaster.web = (objCustMaster.ListCustHdr[0].web.Trim() == null || objCustMaster.ListCustHdr[0].web.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].web.Trim();
            objCustMaster.source = (objCustMaster.ListCustHdr[0].source.Trim() == null || objCustMaster.ListCustHdr[0].source.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].source.Trim();
            objCustMaster.whs_id = (objCustMaster.ListCustHdr[0].dft_whs.Trim() == null || objCustMaster.ListCustHdr[0].dft_whs.Trim() == "") ? string.Empty : objCustMaster.ListCustHdr[0].dft_whs.Trim();
            objCustMaster.mail_name = (objCustMaster.ListCustDtl[0].mail_name.Trim() == null || objCustMaster.ListCustDtl[0].mail_name.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].mail_name.Trim();
            objCustMaster.attn = (objCustMaster.ListCustDtl[0].attn.Trim() == null || objCustMaster.ListCustDtl[0].attn.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].attn.Trim();
            objCustMaster.addr1 = (objCustMaster.ListCustDtl[0].addr_line1.Trim() == null || objCustMaster.ListCustDtl[0].addr_line1.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].addr_line1.Trim();
            objCustMaster.addr2 = (objCustMaster.ListCustDtl[0].addr_line2.Trim() == null || objCustMaster.ListCustDtl[0].addr_line2.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].addr_line2.Trim();
            objCustMaster.city = (objCustMaster.ListCustDtl[0].city.Trim() == null || objCustMaster.ListCustDtl[0].city.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].city.Trim();
            objCustMaster.state = (objCustMaster.ListCustDtl[0].state_id.Trim() == null || objCustMaster.ListCustDtl[0].state_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].state_id.Trim();
            objCustMaster.zip = (objCustMaster.ListCustDtl[0].post_code.Trim() == null || objCustMaster.ListCustDtl[0].post_code.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].post_code.Trim();
            objCustMaster.country = (objCustMaster.ListCustDtl[0].cntry_id.Trim() == null || objCustMaster.ListCustDtl[0].cntry_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].cntry_id.Trim();
            objCustMaster.frieght_id = (objCustMaster.ListCustDtl[0].freight_id.Trim() == null || objCustMaster.ListCustDtl[0].freight_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].freight_id.Trim();
            objCustMaster.ship_via_id = (objCustMaster.ListCustDtl[0].ship_via_id.Trim() == null || objCustMaster.ListCustDtl[0].ship_via_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].ship_via_id.Trim();
            objCustMaster.catg = (objCustMaster.ListCustDtl[0].cust_catg.Trim() == null || objCustMaster.ListCustDtl[0].cust_catg.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].cust_catg.Trim();
            objCustMaster.bl_cycle = (objCustMaster.ListCustDtl[0].bill_cycle.Trim() == null || objCustMaster.ListCustDtl[0].bill_cycle.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].bill_cycle.Trim();
            objCustMaster.code = (objCustMaster.ListCustDtl[0].crdt_code.Trim() == null || objCustMaster.ListCustDtl[0].crdt_code.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_code.Trim();
            objCustMaster.type = (objCustMaster.ListCustDtl[0].crdt_chck.Trim() == null || objCustMaster.ListCustDtl[0].crdt_chck.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_chck.Trim();
            objCustMaster.msg = (objCustMaster.ListCustDtl[0].crdt_msg.Trim() == null || objCustMaster.ListCustDtl[0].crdt_msg.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_msg.Trim();
            objCustMaster.credit_lt = (objCustMaster.ListCustDtl[0].crdt_limit.Trim() == null || objCustMaster.ListCustDtl[0].crdt_limit.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].crdt_limit.Trim();
            objCustMaster.disc = (objCustMaster.ListCustDtl[0].disc.Trim() == null || objCustMaster.ListCustDtl[0].disc.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].disc.Trim();
            objCustMaster.term_code = (objCustMaster.ListCustDtl[0].terms_id.Trim() == null || objCustMaster.ListCustDtl[0].terms_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].terms_id.Trim();
            objCustMaster.ord_val = (objCustMaster.ListCustDtl[0].ordr_value.Trim() == null || objCustMaster.ListCustDtl[0].ordr_value.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].ordr_value.Trim();
            objCustMaster.tax_code = (objCustMaster.ListCustDtl[0].tax_exempt_id.Trim() == null || objCustMaster.ListCustDtl[0].tax_exempt_id.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].tax_exempt_id.Trim();
            objCustMaster.tax_exempt = (objCustMaster.ListCustDtl[0].tax_exempt.Trim() == null || objCustMaster.ListCustDtl[0].tax_exempt.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].tax_exempt.Trim();
            objCustMaster.gl_num = (objCustMaster.ListCustDtl[0].gl_num.Trim() == null || objCustMaster.ListCustDtl[0].gl_num.Trim() == "") ? string.Empty : objCustMaster.ListCustDtl[0].gl_num.Trim();
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
            objCustMaster.region = objCustMaster.ListCustHdr[0].regn_id.Trim();
            objCustMaster.territory = objCustMaster.ListCustHdr[0].tery_id.Trim();
            objCustMaster = ServiceObject.GetCustConfigDetails(objCustMaster);
            objCustMaster.bl_free_days = (objCustMaster.ListCustConfig[0].bill_free_days.Trim() == null) ? "" : objCustMaster.ListCustConfig[0].bill_free_days.Trim();
            objCustMaster.strg_bill = objCustMaster.ListCustConfig[0].bill_type.Trim();
            objLookUp.id = "16";
            objLookUp.lookuptype = "CUSTOMERMASTER";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
            objLookUp.id = "10";
            objLookUp.lookuptype = "STRG";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListSrtg = objLookUp.ListLookUpDtl;
            objCustMaster.inout_bill = objCustMaster.ListCustConfig[0].bill_inout_type.Trim();
            objLookUp.id = "9";
            objLookUp.lookuptype = "INOUT";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListInout = objLookUp.ListLookUpDtl;
            objCustMaster.allow_new_item = objCustMaster.ListCustConfig[0].Allow_New_item.Trim();
            if (objCustMaster.ListCustConfig[0].Allow_New_item.Trim() == "Y")
            {
                objCustMaster.allow_new_item = "Yes";
            }
            else
            {
                objCustMaster.allow_new_item = "No";
            }
            objLookUp.id = "19";
            objLookUp.lookuptype = "DocAllowNewItem";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListDocAllowNewItm = objLookUp.ListLookUpDtl;
            objCustMaster.itemlistby = objCustMaster.ListCustConfig[0].item_pick;
            if (objCustMaster.ListCustConfig[0].item_pick.Trim() == "Document Entry")
            {
                objCustMaster.itemlistby = "DocEntry";
            }
            else
            {
                objCustMaster.itemlistby = "ItemMaster";
            }
            objLookUp.id = "20";
            objLookUp.lookuptype = "ItemListBy";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListItmListBy = objLookUp.ListLookUpDtl;
            //if (objCustMaster.itemlistby == "Document Entry")
            //{
            //    objCustMaster.itemlistby = "Yes";
            //} 

            if (objCustMaster.ListCustConfig[0].init_strg_rt_req == "Y")
            {
                objCustMaster.initstrg = "Yes";
            }
            else
            {
                objCustMaster.initstrg = "No";
            }
            objLookUp.id = "22";
            objLookUp.lookuptype = "IncludeInitStrg";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListIncludeInitStrg = objLookUp.ListLookUpDtl;
            objCustMaster.autoincre = objCustMaster.ListCustConfig[0].Doc_Increment;
            if (objCustMaster.ListCustConfig[0].Doc_Increment == "Y")
            {
                objCustMaster.autoincre = "Yes";
            }
            else
            {
                objCustMaster.autoincre = "No";
            }
            objLookUp.id = "23";
            objLookUp.lookuptype = "DocAutoIncrement";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListAutoIncrement = objLookUp.ListLookUpDtl;
            objCustMaster.recv_lot_by = objCustMaster.ListCustConfig[0].Recv_Itm_Mode;
            objLookUp.id = "17";
            objLookUp.lookuptype = "CUSTRCVLOTBY";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCustMaster.ListRcvLot = objLookUp.ListLookUpDtl;
            objCustMaster.aloc_by = objCustMaster.ListCustConfig[0].Aloc_by.Trim();
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
            Mapper.CreateMap<CustMaster, CustMasterModel>();
            CustMasterModel objCustMastermodel = Mapper.Map<CustMaster, CustMasterModel>(objCustMaster);
            return PartialView("_CustDel", objCustMastermodel);
        }
        public ActionResult CustDelete(string p_str_cmp_id, string p_str_cust_id)
        {
            CustMaster objCustMaster = new CustMaster();
            ICustMasterService ServiceObject = new CustMasterService();
            objCustMaster.cust_of_cmp_id = p_str_cmp_id;
            objCustMaster.cmp_id = p_str_cust_id;
            ServiceObject.DeleteCust(objCustMaster);
            ServiceObject.DeleteCmpHdr(objCustMaster);
            Mapper.CreateMap<CustMaster, CustMasterModel>();
            CustMasterModel objCustMastermodel = Mapper.Map<CustMaster, CustMasterModel>(objCustMaster);
            int resultcount;
            resultcount = 1;
            return Json(resultcount, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCustomerDetails(string p_str_cust_id, string p_str_cmp_id)
         {
            try
            {
                CustMaster objCustMaster = new CustMaster();
                ICustMasterService ServiceObject = new CustMasterService();
                objCustMaster.user_id = Session["UserID"].ToString().Trim();
                objCustMaster.cust_of_cmp_id = p_str_cust_id.Trim();               
                objCustMaster.cmp_id = p_str_cmp_id.Trim();
                Session["g_str_Search_flag"] = "True";////CR-180427-001 Added By Soniya            
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
        public ActionResult UpdateCustDtls(string p_str_cmp_id, string p_str_cust_id, string p_str_cmp_name, string p_str_whs_id, string p_str_source, string p_str_region, string p_str_territory,
           string p_str_cust_grp_id, string p_str_status, string p_str_contact, string p_str_dt_of_entry,
            string p_str_last_chg_dt, string p_str_tel, string p_str_fax, string p_str_email, string p_str_web, string p_str_cell, string p_str_bl_free_days, string p_str_strg_bill, string p_str_inout_bill, string p_str_allow_new_item,
             string p_str_itm_listby, string p_str_auto_inc, string p_str_rcv_lot_by, string p_str_initstrg, string p_str_aloc_by, string p_str_mail_name, string p_str_attn, string p_str_addr1,
             string p_str_addr2, string p_str_city, string p_str_state, string p_str_zip, string p_str_country, string p_str_frieght_id, string p_str_ship_via_id, string p_str_catg, string p_str_bill_cycle, string p_str_credit_code,
             string p_str_credit_type, string p_str_credit_msg, string p_str_credit_limit, string p_str_discount, string p_str_terms_code, string p_str_order_value, string p_str_tax_code, string p_str_tax_exempt, string p_str_gl_num)
        {
            string l_str_lot_count = string.Empty;
            string l_str_docid_count = string.Empty;
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
            objCustMaster.region = p_str_region.Trim();
            objCustMaster.territory = p_str_territory.Trim();
            objCustMaster.insaleid = "";
            objCustMaster.outsaleid = "";
            ServiceObject.UpdateCustMaster(objCustMaster);
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
            ServiceObject.UpdateCustMasterDtl(objCustMaster);
            ServiceObject.UpdateCmpHdr(objCustMaster);            
            objCustMaster.bl_free_days = p_str_bl_free_days.Trim();
            objCustMaster.strg_bill = p_str_strg_bill.Trim();
            objCustMaster.inout_bill = p_str_inout_bill;
            objCustMaster.allow_new_item = p_str_allow_new_item.Trim();
            if(p_str_allow_new_item.Trim() == "Yes")
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
            objCustMaster.aloc_by = p_str_aloc_by.Trim();
            ServiceObject.UpdateCustMasterConfig(objCustMaster);
            ServiceObject.UpdateCustMasterConfigDir(objCustMaster);
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
            Mapper.CreateMap<CustMaster, CustMasterModel>();
            CustMasterModel objCustMastermodel = Mapper.Map<CustMaster, CustMasterModel>(objCustMaster);
            return View("~/Views/CustMaster/CustMaster.cshtml", objCustMastermodel);
        }
        public ActionResult SaveCustdtls(string p_str_cust_id,string p_str_cmp_id, string p_str_cmp_name, string p_str_whs_id, string p_str_source, string p_str_region, string p_str_territory,
            string p_str_cust_grp_id, string p_str_status, string p_str_contact, string p_str_dt_of_entry, 
             string p_str_last_chg_dt, string p_str_tel, string p_str_fax, string p_str_email, string p_str_web,string p_str_cell, string p_str_bl_free_days, string p_str_strg_bill, string p_str_inout_bill, string p_str_allow_new_item,
             string p_str_itm_listby, string p_str_auto_inc, string p_str_rcv_lot_by, string p_str_initstrg, string p_str_aloc_by, string p_str_mail_name, string p_str_attn, string p_str_addr1,
             string p_str_addr2, string p_str_city, string p_str_state, string p_str_zip, string p_str_country, string p_str_frieght_id, string p_str_ship_via_id, string p_str_catg, string p_str_bill_cycle, string p_str_credit_code,
             string p_str_credit_type, string p_str_credit_msg, string p_str_credit_limit, string p_str_discount, string p_str_terms_code, string p_str_order_value, string p_str_tax_code, string p_str_tax_exempt, string p_str_gl_num)
        {
            string l_str_lot_count = string.Empty;
            string l_str_docid_count = string.Empty;
            CustMaster objCustMaster = new CustMaster();
            ICustMasterService ServiceObject = new CustMasterService();           
            objCustMaster.cust_of_cmp_id = p_str_cust_id.Trim();
            objCustMaster.cmp_id = p_str_cmp_id.Trim();
            objCustMaster = ServiceObject.GetCheckExistCmpId(objCustMaster);
            if(objCustMaster.ListCheckExistCmpId.Count == 0)
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

                objCustMaster.web = p_str_web;
                objCustMaster.source = p_str_source.Trim();
                objCustMaster.whs_id = p_str_whs_id.Trim();
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
                ServiceObject.SaveUserXCmp(objCustMaster);
                ServiceObject.SaveWhsMaster(objCustMaster);
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
                objCustMaster.aloc_by = p_str_aloc_by.Trim();
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
                l_str_docid_count  = objCustMaster.entity_count_doc_id;
                if (l_str_docid_count == "0")
                {
                    objCustMaster.entity_Code = "rma_doc_id";
                    ServiceObject.InsertTableEntityValueByCust(objCustMaster);
                }
                Mapper.CreateMap<CustMaster, CustMasterModel>();
                CustMasterModel objCustMastermodel = Mapper.Map<CustMaster, CustMasterModel>(objCustMaster);
                return View("~/Views/CustMaster/CustMaster.cshtml", objCustMastermodel);
            }
            else
            {
                return Json(objCustMaster.ListCheckExistCmpId.Count, JsonRequestBehavior.AllowGet);
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
        public JsonResult CUST_MASTER_INQ_HDR_DATA(string p_str_cmp_id, string p_str_cust_id)
        {
            CustMaster objCustMaster = new CustMaster();
            ICustMasterService ServiceObject = new CustMasterService();           
            Session["TEMP_CUST_ID"] = p_str_cust_id.Trim();
            Session["TEMP_CMP_ID"] = p_str_cmp_id.Trim();       
            return Json(objCustMaster.MasterCount, JsonRequestBehavior.AllowGet);

        }
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
                    objCustMaster.cust_of_cmp_id = Session["TEMP_CMP_ID"].ToString().Trim();
                    objCustMaster.cmp_id = Session["TEMP_CUST_ID"].ToString().Trim();
                    objCustMaster.user_id = Session["UserID"].ToString().Trim();

                }
                else
                {
                    objCustMaster.cust_of_cmp_id = p_str_cmp_id.Trim();
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
    }
}