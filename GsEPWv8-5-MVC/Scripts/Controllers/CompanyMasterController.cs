using AutoMapper;
using GsEPWv8_4_MVC.Business.Implementation;
using GsEPWv8_4_MVC.Business.Interface;
using GsEPWv8_4_MVC.Core.Entity;
using GsEPWv8_4_MVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GsEPWv8_4_MVC.Controllers
{
    public class CompanyMasterController : Controller
    {
        // GET: CompanyMaster
        public ActionResult CompanyMaster(string FullFillType, string cmp)
        {
            string l_str_scn_id = string.Empty;
            string l_str_success = string.Empty;
            try
            {
                CompanyMaster objCompanyMaster = new CompanyMaster();
                ICompanyMasterService ServiceObject = new CompanyMasterService();
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                Session["g_str_Search_flag"] = "False";
                objCompanyMaster.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
                objCompanyMaster.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                objCompany.cust_of_cmp_id = "";
                            objCompany.cmp_id = objCompanyMaster.cmp_id;
                            objCompany = ServiceObjectCompany.GetCustOfCompName(objCompany);
                            objCompanyMaster.LstCustOfCmpName = objCompany.LstCustOfCmpName;
                            objCompanyMaster.cust_of_cmpid = objCompanyMaster.LstCustOfCmpName[0].cust_of_cmpid;
                            objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
                            objCompanyMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
                            //objCompany.cust_of_cmp_id = "";
                            //objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
                            //objCompanyMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
                            //objCompanyMaster = ServiceObject.GetAllCmpGridDetails(objCompanyMaster);
                            LookUp objLookUp = new LookUp();
                            LookUpService ServiceObject2 = new LookUpService();
                            objLookUp.id = "16";
                            objLookUp.lookuptype = "CUSTOMERMASTER";
                            objLookUp = ServiceObject2.GetLookUpValue(objLookUp);
                            objCompanyMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
                            Mapper.CreateMap<CompanyMaster, CompanyMasterModel>();
                            CompanyMasterModel objCompanyMasterModel = Mapper.Map<CompanyMaster, CompanyMasterModel>(objCompanyMaster);
                            return View(objCompanyMasterModel);                        
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ActionResult SaveCmpdtls(string p_str_cmp_id, string p_str_cmp_name, string p_str_start_dt, string p_str_last_chg_dt, string p_str_status, string p_str_addr1, string p_str_addr2,
          string p_str_city, string p_str_state_id, string p_str_zip, string p_str_country,
           string p_str_attn, string p_str_tel, string p_str_cell, string p_str_fax, string p_str_email, string p_str_web, string p_str_remit_attn, string p_str_remit_addr_line1, string p_str_remit_addr_line2, string p_str_Remit_City,
           string p_str_remit_state_id, string p_str_Remit_Post_Code, string p_str_remit_cntry_id, string p_str_remit_tel, string p_str_remit_fax, string p_str_remit_email, string p_str_remit_web)
        {
            int l_int_count = 0;
            CompanyMaster objCompanyMaster = new CompanyMaster();
            ICompanyMasterService ServiceObject = new CompanyMasterService();
            objCompanyMaster.cust_of_cmp_id = p_str_cmp_id.Trim();
            objCompanyMaster.cmp_name = p_str_cmp_name.Trim();
            objCompanyMaster = ServiceObject.CheckExistCmpId(objCompanyMaster);
            if (objCompanyMaster.ListCheckExistCmpId.Count == 0)
            {
                objCompanyMaster.start_dt = p_str_start_dt.Trim();
                objCompanyMaster.last_chg_dt = p_str_last_chg_dt.Trim();
                objCompanyMaster.status = p_str_status.Trim();
                objCompanyMaster.addr1 = p_str_addr1;
                objCompanyMaster.addr2 = p_str_addr2.Trim();
                objCompanyMaster.Tel = p_str_tel.Trim();
                objCompanyMaster.fax = p_str_fax.Trim();
                objCompanyMaster.email = p_str_email.Trim();
                objCompanyMaster.web = p_str_web;
                objCompanyMaster.country = p_str_country.Trim();
                objCompanyMaster.group_id = "Comp";
                objCompanyMaster.state_id = p_str_state_id.Trim();
                objCompanyMaster.zip = p_str_zip;
                objCompanyMaster.city = p_str_city;
                objCompanyMaster.cell = p_str_cell;
                objCompanyMaster.attn = p_str_attn.Trim();
                objCompanyMaster.remit_attn = p_str_remit_attn.Trim();
                objCompanyMaster.remit_addr_line1 = p_str_remit_addr_line1.Trim();
                objCompanyMaster.remit_addr_line2 = p_str_remit_addr_line2.Trim();
                objCompanyMaster.Remit_City = p_str_Remit_City;
                objCompanyMaster.remit_cntry_id = p_str_remit_cntry_id;
                objCompanyMaster.remit_state_id = (p_str_remit_state_id.Trim() == null) ? "" : p_str_remit_state_id.Trim();
                objCompanyMaster.Remit_Post_Code = (p_str_Remit_Post_Code.Trim() == null) ? "" : p_str_Remit_Post_Code.Trim();
                objCompanyMaster.remit_state_id = (p_str_remit_state_id.Trim() == null) ? "" : p_str_remit_state_id.Trim();
                objCompanyMaster.remit_tel = (p_str_remit_tel.Trim() == null) ? "" : p_str_remit_tel.Trim();
                objCompanyMaster.remit_fax = (p_str_remit_fax.Trim() == null) ? "" : p_str_remit_fax.Trim();
                objCompanyMaster.remit_email = (p_str_remit_email.Trim() == null) ? "" : p_str_remit_email.Trim();
                objCompanyMaster.remit_web = (p_str_remit_web.Trim() == null) ? "" : p_str_remit_web.Trim();
                ServiceObject.SaveCmpMasterDtls(objCompanyMaster);
                Mapper.CreateMap<CompanyMaster, CompanyMasterModel>();
                CompanyMasterModel objCompanyMastermodel = Mapper.Map<CompanyMaster, CompanyMasterModel>(objCompanyMaster);
                return View("~/Views/CompanyMaster/CompanyMaster.cshtml", objCompanyMastermodel);
            }
            else
            {
                l_int_count = 0;
                return Json(l_int_count, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Edit(string cust_of_cmp_id)
        {
            CompanyMaster objCompanyMaster = new CompanyMaster();
            ICompanyMasterService ServiceObject = new CompanyMasterService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            LookUp objLookUp = new LookUp();
            objCompanyMaster.cust_of_cmp_id = cust_of_cmp_id;            
            LookUpService ServiceObject1 = new LookUpService();
            objCompanyMaster = ServiceObject.GetCmpHdrDetails(objCompanyMaster);
            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objCompanyMaster.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objCompanyMaster.ListStatePick = objPick.ListStatePick;
            objCompanyMaster.cust_of_cmp_id = objCompanyMaster.ListCmpHdrDetails[0].cmp_id.Trim();
            objCompany.cust_of_cmp_id = objCompanyMaster.ListCmpHdrDetails[0].cmp_id.Trim();
            objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
            objCompanyMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
            objCompanyMaster.start_dt = (objCompanyMaster.ListCmpHdrDetails[0].start_dt.Trim() == null || objCompanyMaster.ListCmpHdrDetails[0].start_dt.Trim() == "" ? string.Empty : Convert.ToDateTime(objCompanyMaster.ListCmpHdrDetails[0].start_dt.Trim()).ToString("MM/dd/yyyy"));
            objCompanyMaster.last_chg_dt = (objCompanyMaster.ListCmpHdrDetails[0].last_chg_dt.Trim() == null || objCompanyMaster.ListCmpHdrDetails[0].last_chg_dt.Trim() == "" ? string.Empty : Convert.ToDateTime(objCompanyMaster.ListCmpHdrDetails[0].last_chg_dt.Trim()).ToString("MM/dd/yyyy"));           
            objCompanyMaster.cmp_name = objCompanyMaster.ListCmpHdrDetails[0].cmp_name.Trim();           
            objCompanyMaster.status = objCompanyMaster.ListCmpHdrDetails[0].status.Trim();
            objLookUp.id = "16";
            objLookUp.lookuptype = "CUSTOMERMASTER";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCompanyMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;           
            objCompanyMaster.attn = (objCompanyMaster.ListCmpHdrDetails[0].attn.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].attn.Trim();            
            objCompanyMaster.addr1 = (objCompanyMaster.ListCmpHdrDetails[0].addr_line1.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].addr_line1.Trim();           
            objCompanyMaster.addr2 = (objCompanyMaster.ListCmpHdrDetails[0].addr_line2.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].addr_line2.Trim();            
            objCompanyMaster.city = (objCompanyMaster.ListCmpHdrDetails[0].city.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].city.Trim();
            objCompanyMaster.state_id = (objCompanyMaster.ListCmpHdrDetails[0].state_id.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].state_id.Trim();
            objCompanyMaster.zip = (objCompanyMaster.ListCmpHdrDetails[0].post_code.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].post_code.Trim();
            objCompanyMaster.country = (objCompanyMaster.ListCmpHdrDetails[0].cntry_id.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].cntry_id.Trim();           
            objCompanyMaster.Tel = (objCompanyMaster.ListCmpHdrDetails[0].tel.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].tel.Trim();           
            objCompanyMaster.fax = (objCompanyMaster.ListCmpHdrDetails[0].fax.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].fax.Trim();           
            objCompanyMaster.web = (objCompanyMaster.ListCmpHdrDetails[0].web.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].web.Trim();            
            objCompanyMaster.email = (objCompanyMaster.ListCmpHdrDetails[0].email.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].email.Trim();            
            objCompanyMaster.remit_attn = (objCompanyMaster.ListCmpHdrDetails[0].remit_attn.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_attn.Trim();           
            objCompanyMaster.remit_addr_line1 = (objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line1.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line1.Trim();            
            objCompanyMaster.remit_addr_line2 = (objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line2.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line2.Trim();          
            objCompanyMaster.Remit_City = (objCompanyMaster.ListCmpHdrDetails[0].remit_city.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_city.Trim();
            objCompanyMaster.remit_state_id = (objCompanyMaster.ListCmpHdrDetails[0].remit_state_id.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_state_id.Trim();           
            objCompanyMaster.Remit_Post_Code = (objCompanyMaster.ListCmpHdrDetails[0].remit_post_code.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_post_code.Trim();            
            objCompanyMaster.remit_cntry_id = (objCompanyMaster.ListCmpHdrDetails[0].remit_cntry_id.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_cntry_id.Trim();            
            objCompanyMaster.remit_tel = (objCompanyMaster.ListCmpHdrDetails[0].remit_tel.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_tel.Trim();           
            objCompanyMaster.remit_fax = (objCompanyMaster.ListCmpHdrDetails[0].remit_fax.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_fax.Trim();                       
            objCompanyMaster.remit_email = (objCompanyMaster.ListCmpHdrDetails[0].remit_email.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_email.Trim();            
            objCompanyMaster.remit_web = (objCompanyMaster.ListCmpHdrDetails[0].remit_web.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_web.Trim();
            Mapper.CreateMap<CompanyMaster, CompanyMasterModel>();
            CompanyMasterModel objCompanyMastermodel = Mapper.Map<CompanyMaster, CompanyMasterModel>(objCompanyMaster);
            return PartialView("_CmpMasterEdit", objCompanyMastermodel);
        }
        public ActionResult Delete(string cust_of_cmp_id)
        {
            CompanyMaster objCompanyMaster = new CompanyMaster();
            ICompanyMasterService ServiceObject = new CompanyMasterService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            LookUp objLookUp = new LookUp();
            objCompanyMaster.cust_of_cmp_id = cust_of_cmp_id;
            LookUpService ServiceObject1 = new LookUpService();
            objCompanyMaster = ServiceObject.GetCmpHdrDetails(objCompanyMaster);
            objCompanyMaster.cust_of_cmp_id = objCompanyMaster.ListCmpHdrDetails[0].cmp_id.Trim();
            objCompany.cust_of_cmp_id = objCompanyMaster.ListCmpHdrDetails[0].cmp_id.Trim();
            objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
            objCompanyMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
            objCompanyMaster.cmp_name = objCompanyMaster.ListCmpHdrDetails[0].cmp_name.Trim();
            objCompanyMaster.start_dt = (objCompanyMaster.ListCmpHdrDetails[0].start_dt.Trim() == null || objCompanyMaster.ListCmpHdrDetails[0].start_dt.Trim() == "" ? string.Empty : Convert.ToDateTime(objCompanyMaster.ListCmpHdrDetails[0].start_dt.Trim()).ToString("MM/dd/yyyy"));            
            objCompanyMaster.last_chg_dt = (objCompanyMaster.ListCmpHdrDetails[0].last_chg_dt.Trim() == null || objCompanyMaster.ListCmpHdrDetails[0].last_chg_dt.Trim() == "" ? string.Empty : Convert.ToDateTime(objCompanyMaster.ListCmpHdrDetails[0].last_chg_dt.Trim()).ToString("MM/dd/yyyy"));
            objCompanyMaster.status = objCompanyMaster.ListCmpHdrDetails[0].status.Trim();
            objLookUp.id = "16";
            objLookUp.lookuptype = "CUSTOMERMASTER";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCompanyMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
            objCompanyMaster.attn = (objCompanyMaster.ListCmpHdrDetails[0].attn.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].attn.Trim();
            objCompanyMaster.addr1 = (objCompanyMaster.ListCmpHdrDetails[0].addr_line1.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].addr_line1.Trim();
            objCompanyMaster.addr2 = (objCompanyMaster.ListCmpHdrDetails[0].addr_line2.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].addr_line2.Trim();
            objCompanyMaster.city = (objCompanyMaster.ListCmpHdrDetails[0].city.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].city.Trim();
            objCompanyMaster.state_id = (objCompanyMaster.ListCmpHdrDetails[0].state_id.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].state_id.Trim();
            objCompanyMaster.zip = (objCompanyMaster.ListCmpHdrDetails[0].post_code.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].post_code.Trim();
            objCompanyMaster.country = (objCompanyMaster.ListCmpHdrDetails[0].cntry_id.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].cntry_id.Trim();
            objCompanyMaster.Tel = (objCompanyMaster.ListCmpHdrDetails[0].tel.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].tel.Trim();
            objCompanyMaster.fax = (objCompanyMaster.ListCmpHdrDetails[0].fax.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].fax.Trim();
            objCompanyMaster.web = (objCompanyMaster.ListCmpHdrDetails[0].web.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].web.Trim();
            objCompanyMaster.email = (objCompanyMaster.ListCmpHdrDetails[0].email.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].email.Trim();
            objCompanyMaster.remit_attn = (objCompanyMaster.ListCmpHdrDetails[0].remit_attn.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_attn.Trim();
            objCompanyMaster.remit_addr_line1 = (objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line1.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line1.Trim();
            objCompanyMaster.remit_addr_line2 = (objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line2.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line2.Trim();
            objCompanyMaster.Remit_City = (objCompanyMaster.ListCmpHdrDetails[0].remit_city.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_city.Trim();
            objCompanyMaster.remit_state_id = (objCompanyMaster.ListCmpHdrDetails[0].remit_state_id.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_state_id.Trim();
            objCompanyMaster.Remit_Post_Code = (objCompanyMaster.ListCmpHdrDetails[0].remit_post_code.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_post_code.Trim();
            objCompanyMaster.remit_cntry_id = (objCompanyMaster.ListCmpHdrDetails[0].remit_cntry_id.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_cntry_id.Trim();
            objCompanyMaster.remit_tel = (objCompanyMaster.ListCmpHdrDetails[0].remit_tel.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_tel.Trim();
            objCompanyMaster.remit_fax = (objCompanyMaster.ListCmpHdrDetails[0].remit_fax.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_fax.Trim();
            objCompanyMaster.remit_email = (objCompanyMaster.ListCmpHdrDetails[0].remit_email.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_email.Trim();
            objCompanyMaster.remit_web = (objCompanyMaster.ListCmpHdrDetails[0].remit_web.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_web.Trim();
            objCompanyMaster.View_Flag = "D";
            Mapper.CreateMap<CompanyMaster, CompanyMasterModel>();
            CompanyMasterModel objCompanyMastermodel = Mapper.Map<CompanyMaster, CompanyMasterModel>(objCompanyMaster);
            return PartialView("_CmpMasterDel", objCompanyMastermodel);
        }
        public ActionResult View(string cust_of_cmp_id)
        {
            CompanyMaster objCompanyMaster = new CompanyMaster();
            ICompanyMasterService ServiceObject = new CompanyMasterService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            LookUp objLookUp = new LookUp();
            objCompanyMaster.cust_of_cmp_id = cust_of_cmp_id;
            LookUpService ServiceObject1 = new LookUpService();
            objCompanyMaster = ServiceObject.GetCmpHdrDetails(objCompanyMaster);
            objCompanyMaster.cust_of_cmp_id = objCompanyMaster.ListCmpHdrDetails[0].cmp_id.Trim();
            objCompany.cust_of_cmp_id = objCompanyMaster.ListCmpHdrDetails[0].cmp_id.Trim();
            objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
            objCompanyMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
            objCompanyMaster.cmp_name = objCompanyMaster.ListCmpHdrDetails[0].cmp_name.Trim();
            objCompanyMaster.start_dt = (objCompanyMaster.ListCmpHdrDetails[0].start_dt.Trim() == null || objCompanyMaster.ListCmpHdrDetails[0].start_dt.Trim() == "" ? string.Empty : Convert.ToDateTime(objCompanyMaster.ListCmpHdrDetails[0].start_dt.Trim()).ToString("MM/dd/yyyy"));
            objCompanyMaster.last_chg_dt = (objCompanyMaster.ListCmpHdrDetails[0].last_chg_dt.Trim() == null || objCompanyMaster.ListCmpHdrDetails[0].last_chg_dt.Trim() == "" ? string.Empty : Convert.ToDateTime(objCompanyMaster.ListCmpHdrDetails[0].last_chg_dt.Trim()).ToString("MM/dd/yyyy"));
            objCompanyMaster.status = objCompanyMaster.ListCmpHdrDetails[0].status.Trim();
            objLookUp.id = "16";
            objLookUp.lookuptype = "CUSTOMERMASTER";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCompanyMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
            objCompanyMaster.attn = (objCompanyMaster.ListCmpHdrDetails[0].attn.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].attn.Trim();
            objCompanyMaster.addr1 = (objCompanyMaster.ListCmpHdrDetails[0].addr_line1.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].addr_line1.Trim();
            objCompanyMaster.addr2 = (objCompanyMaster.ListCmpHdrDetails[0].addr_line2.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].addr_line2.Trim();
            objCompanyMaster.city = (objCompanyMaster.ListCmpHdrDetails[0].city.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].city.Trim();
            objCompanyMaster.state_id = (objCompanyMaster.ListCmpHdrDetails[0].state_id.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].state_id.Trim();
            objCompanyMaster.zip = (objCompanyMaster.ListCmpHdrDetails[0].post_code.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].post_code.Trim();
            objCompanyMaster.country = (objCompanyMaster.ListCmpHdrDetails[0].cntry_id.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].cntry_id.Trim();
            objCompanyMaster.Tel = (objCompanyMaster.ListCmpHdrDetails[0].tel.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].tel.Trim();
            objCompanyMaster.fax = (objCompanyMaster.ListCmpHdrDetails[0].fax.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].fax.Trim();
            objCompanyMaster.web = (objCompanyMaster.ListCmpHdrDetails[0].web.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].web.Trim();
            objCompanyMaster.email = (objCompanyMaster.ListCmpHdrDetails[0].email.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].email.Trim();
            objCompanyMaster.remit_attn = (objCompanyMaster.ListCmpHdrDetails[0].remit_attn.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_attn.Trim();
            objCompanyMaster.remit_addr_line1 = (objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line1.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line1.Trim();
            objCompanyMaster.remit_addr_line2 = (objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line2.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line2.Trim();
            objCompanyMaster.Remit_City = (objCompanyMaster.ListCmpHdrDetails[0].remit_city.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_city.Trim();
            objCompanyMaster.remit_state_id = (objCompanyMaster.ListCmpHdrDetails[0].remit_state_id.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_state_id.Trim();
            objCompanyMaster.Remit_Post_Code = (objCompanyMaster.ListCmpHdrDetails[0].remit_post_code.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_post_code.Trim();
            objCompanyMaster.remit_cntry_id = (objCompanyMaster.ListCmpHdrDetails[0].remit_cntry_id.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_cntry_id.Trim();
            objCompanyMaster.remit_tel = (objCompanyMaster.ListCmpHdrDetails[0].remit_tel.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_tel.Trim();
            objCompanyMaster.remit_fax = (objCompanyMaster.ListCmpHdrDetails[0].remit_fax.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_fax.Trim();
            objCompanyMaster.remit_email = (objCompanyMaster.ListCmpHdrDetails[0].remit_email.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_email.Trim();
            objCompanyMaster.remit_web = (objCompanyMaster.ListCmpHdrDetails[0].remit_web.Trim() == null) ? "" : objCompanyMaster.ListCmpHdrDetails[0].remit_web.Trim();
            objCompanyMaster.View_Flag = "V";
            Mapper.CreateMap<CompanyMaster, CompanyMasterModel>();
            CompanyMasterModel objCompanyMastermodel = Mapper.Map<CompanyMaster, CompanyMasterModel>(objCompanyMaster);
            return PartialView("_CmpMasterDel", objCompanyMastermodel);
        }
        public ActionResult UpdateCmpDtls(string p_str_cmp_id, string p_str_cmp_name, string p_str_start_dt, string p_str_last_chg_dt, string p_str_status, string p_str_addr1, string p_str_addr2,
          string p_str_city, string p_str_state_id, string p_str_zip, string p_str_country,
           string p_str_attn, string p_str_tel,string p_str_fax, string p_str_email, string p_str_web, string p_str_remit_attn, string p_str_remit_addr_line1, string p_str_remit_addr_line2, string p_str_Remit_City,
           string p_str_remit_state_id, string p_str_Remit_Post_Code, string p_str_remit_cntry_id, string p_str_remit_tel, string p_str_remit_fax, string p_str_remit_email, string p_str_remit_web)
        {
            CompanyMaster objCompanyMaster = new CompanyMaster();
            ICompanyMasterService ServiceObject = new CompanyMasterService();
            objCompanyMaster.cust_of_cmp_id = p_str_cmp_id.Trim();
            objCompanyMaster.cmp_name = p_str_cmp_name.Trim();
            objCompanyMaster.start_dt = p_str_start_dt.Trim();
            objCompanyMaster.last_chg_dt = p_str_last_chg_dt.Trim();
            objCompanyMaster.status = p_str_status.Trim();
            objCompanyMaster.addr1 = p_str_addr1;
            objCompanyMaster.addr2 = p_str_addr2.Trim();
            objCompanyMaster.Tel = p_str_tel.Trim();
            objCompanyMaster.fax = p_str_fax.Trim();
            objCompanyMaster.email = p_str_email.Trim();
            objCompanyMaster.web = p_str_web;
            objCompanyMaster.country = p_str_country.Trim();
            objCompanyMaster.group_id = "Comp";
            objCompanyMaster.state_id = p_str_state_id.Trim();
            objCompanyMaster.zip = p_str_zip;
            objCompanyMaster.city = p_str_city;
            objCompanyMaster.attn = p_str_attn.Trim();
            objCompanyMaster.remit_attn = p_str_remit_attn.Trim();
            objCompanyMaster.remit_addr_line1 = p_str_remit_addr_line1.Trim();
            objCompanyMaster.remit_addr_line2 = p_str_remit_addr_line2.Trim();
            objCompanyMaster.Remit_City = p_str_Remit_City;
            objCompanyMaster.remit_cntry_id = p_str_remit_cntry_id;
            objCompanyMaster.remit_state_id = (p_str_remit_state_id.Trim() == null) ? "" : p_str_remit_state_id.Trim();
            objCompanyMaster.Remit_Post_Code = (p_str_Remit_Post_Code.Trim() == null) ? "" : p_str_Remit_Post_Code.Trim();
            objCompanyMaster.remit_state_id = (p_str_remit_state_id.Trim() == null) ? "" : p_str_remit_state_id.Trim();
            objCompanyMaster.remit_tel = (p_str_remit_tel.Trim() == null) ? "" : p_str_remit_tel.Trim();
            objCompanyMaster.remit_fax = (p_str_remit_fax.Trim() == null) ? "" : p_str_remit_fax.Trim();
            objCompanyMaster.remit_email = (p_str_remit_email.Trim() == null) ? "" : p_str_remit_email.Trim();
            objCompanyMaster.remit_web = (p_str_remit_web.Trim() == null) ? "" : p_str_remit_web.Trim();
            ServiceObject.UpdateCmpMasterDtls(objCompanyMaster);
            Mapper.CreateMap<CompanyMaster, CompanyMasterModel>();
            CompanyMasterModel objCompanyMastermodel = Mapper.Map<CompanyMaster, CompanyMasterModel>(objCompanyMaster);
            return View("~/Views/CompanyMaster/CompanyMaster.cshtml", objCompanyMastermodel);
        }
        public ActionResult GetCmpDetails(string p_str_cmp_id)
        {
            try
            {
                CompanyMaster objCompanyMaster = new CompanyMaster();
                ICompanyMasterService ServiceObject = new CompanyMasterService();
                objCompanyMaster.cust_of_cmp_id = p_str_cmp_id.Trim();
                Session["g_str_Search_flag"] = "True";////CR-180427-001 Added By Soniya
                objCompanyMaster.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                objCompanyMaster = ServiceObject.GetCmpMasterDetails(objCompanyMaster);
                Mapper.CreateMap<CompanyMaster, CompanyMasterModel>();
                CompanyMasterModel objCompanyMastermodel = Mapper.Map<CompanyMaster, CompanyMasterModel>(objCompanyMaster);
                return PartialView("_CompanyMaster", objCompanyMastermodel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult CmpDelete(string p_str_cmp_id)
        {
            CompanyMaster objCompanyMaster = new CompanyMaster();
            ICompanyMasterService ServiceObject = new CompanyMasterService();
            objCompanyMaster.cust_of_cmp_id = p_str_cmp_id;
            ServiceObject.DeleteCmp(objCompanyMaster);
            Mapper.CreateMap<CompanyMaster, CompanyMasterModel>();
            CompanyMasterModel objCompanyMastermodel = Mapper.Map<CompanyMaster, CompanyMasterModel>(objCompanyMaster);
            int resultcount;
            resultcount = 1;
            return Json(resultcount, JsonRequestBehavior.AllowGet);
        }
        public ActionResult New(string cmpid)
        {

            CompanyMaster objCompanyMaster = new CompanyMaster();
            ICompanyMasterService ServiceObject = new CompanyMasterService();          
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cust_of_cmp_id = "";
            objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
            objCompanyMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
            objCompanyMaster.last_chg_dt = DateTime.Now.ToString("MM/dd/yyyy");
            objCompanyMaster.start_dt = DateTime.Now.ToString("MM/dd/yyyy");
            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = cmpid;
            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objCompanyMaster.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objCompanyMaster.ListStatePick = objPick.ListStatePick;
            LookUp objLookUp = new LookUp();
            LookUpService ServiceObject1 = new LookUpService();
            objLookUp.id = "16";
            objLookUp.lookuptype = "CUSTOMERMASTER";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objCompanyMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
            Mapper.CreateMap<CompanyMaster, CompanyMasterModel>();
            CompanyMasterModel objCompanyMastermodel = Mapper.Map<CompanyMaster, CompanyMasterModel>(objCompanyMaster);
            return PartialView("_AddCompany", objCompanyMastermodel);
        }

        public JsonResult Cmp_MASTER_INQ_HDR_DATA(string p_str_cmp_id)
        {
            CompanyMaster objCompanyMaster = new CompanyMaster();
            ICompanyMasterService ServiceObject = new CompanyMasterService();
            Session["TEMP_CMP_ID"] = p_str_cmp_id.Trim();
            return Json(objCompanyMaster.MasterCount, JsonRequestBehavior.AllowGet);

        }
        public ActionResult CmpMasterDtl(string p_str_cmp_id)
        {
            try
            {

                CompanyMaster objCompanyMaster = new CompanyMaster();
                ICompanyMasterService ServiceObject = new CompanyMasterService();
                string l_str_search_flag = string.Empty;
                string l_str_is_another_usr = string.Empty;

                //l_str_is_another_usr = Session["isanother"].ToString();
                //objRateMaster.IsAnotherUser = l_str_is_another_usr.Trim();
                l_str_search_flag = Session["g_str_Search_flag"].ToString().Trim();
                objCompanyMaster.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                //objCustMaster.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                if (l_str_search_flag == "True")
                {
                    //objRateMaster.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                    objCompanyMaster.cust_of_cmp_id = Session["TEMP_CMP_ID"].ToString().Trim();
                    objCompanyMaster.is_company_user = Session["IsCompanyUser"].ToString().Trim();

                }
                else
                {
                    objCompanyMaster.cust_of_cmp_id = p_str_cmp_id.Trim();                   
                }

                objCompanyMaster = ServiceObject.GetCmpMasterDetails(objCompanyMaster);
                Mapper.CreateMap<CompanyMaster, CompanyMasterModel>();
                CompanyMasterModel objCompanyMastermodel = Mapper.Map<CompanyMaster, CompanyMasterModel>(objCompanyMaster);
                return PartialView("_CompanyMaster", objCompanyMastermodel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}