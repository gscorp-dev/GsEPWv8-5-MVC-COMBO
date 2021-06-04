using AutoMapper;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GsEPWv8_5_MVC.Controllers
{
    public class CompanyMasterController : Controller
    {
       
        Pick objPick = new Pick();
        PickService ServiceObjectPick = new PickService();
        Company objCompany = new Company();
        CompanyService ServiceObjectCompany = new CompanyService();
        LookUp objLookUp = new LookUp();
        LookUpService ServiceObjectLookUp = new LookUpService();
      
        public ActionResult CompanyMaster(string FullFillType, string cmp)
        {
            string l_str_scn_id = string.Empty;
            string l_str_success = string.Empty;
            try
            {
                CompanyMaster objCompanyMaster = new CompanyMaster();
                ICompanyMasterService ServiceObject = new CompanyMasterService();
                Session["g_str_Search_flag"] = "False";
                if(Session["g_str_cmp_id"].ToString()!=null && Session["g_str_cmp_id"].ToString()!=string.Empty)
                {
                    objCompanyMaster.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                }
                else
                {
                    objCompanyMaster.cmp_id = string.Empty;
                }
                if (Session["IsCompanyUser"].ToString() != null && Session["IsCompanyUser"].ToString() != string.Empty)
                {
                    objCompanyMaster.is_company_user = Session["IsCompanyUser"].ToString().Trim();
                }
                else
                {
                    objCompanyMaster.is_company_user = string.Empty;
                }
            
                objCompany.cust_of_cmp_id =string.Empty;
                objCompany.cmp_id = objCompanyMaster.cmp_id;
                objCompany = ServiceObjectCompany.GetCustOfCompName(objCompany);
                objCompanyMaster.LstCustOfCmpName = objCompany.LstCustOfCmpName;
                if (objCompanyMaster.LstCustOfCmpName.Count > 0)
                {
                    objCompanyMaster.cust_of_cmpid = (objCompanyMaster.LstCustOfCmpName[0].cust_of_cmpid== null)?string.Empty: objCompanyMaster.LstCustOfCmpName[0].cust_of_cmpid.Trim();
                }
                else
                {
                    objCompanyMaster.cust_of_cmpid = string.Empty;
                }
                objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
                objCompanyMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
                objLookUp.id = "16";
                objLookUp.lookuptype = "CUSTOMERMASTER";
                objLookUp = ServiceObjectLookUp.GetLookUpValue(objLookUp);
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
           string p_str_remit_state_id, string p_str_Remit_Post_Code, string p_str_remit_cntry_id, string p_str_remit_tel, string p_str_remit_fax, string p_str_remit_email, string p_str_remit_web,string p_str_dflt_whs_id)
        {
            int l_int_count = 0;
            CompanyMaster objCompanyMaster = new CompanyMaster();
            ICompanyMasterService ServiceObject = new CompanyMasterService();
            objCompanyMaster.cust_of_cmp_id = (p_str_cmp_id==null)?string.Empty: p_str_cmp_id.Trim();
            objCompanyMaster.cmp_name = (p_str_cmp_name== null)?string.Empty: p_str_cmp_name.Trim();
            objCompanyMaster = ServiceObject.CheckExistCmpId(objCompanyMaster);
            if (objCompanyMaster.ListCheckExistCmpId.Count == 0)
            {
                objCompanyMaster.start_dt = (p_str_start_dt == null) ? string.Empty : p_str_start_dt.Trim();
                objCompanyMaster.last_chg_dt = (p_str_last_chg_dt == null) ? string.Empty : p_str_start_dt.Trim();
                objCompanyMaster.status = (p_str_status == null) ? string.Empty : p_str_status.Trim();
                objCompanyMaster.addr1 = (p_str_addr1 == null) ? string.Empty : p_str_addr1.Trim();
                objCompanyMaster.addr2 = (p_str_addr2 == null) ? string.Empty : p_str_addr2.Trim();
                objCompanyMaster.Tel = (p_str_tel == null) ? string.Empty : p_str_tel.Trim();
                objCompanyMaster.fax = (p_str_fax == null) ? string.Empty : p_str_fax.Trim();
                objCompanyMaster.email = (p_str_email == null) ? string.Empty : p_str_email.Trim();
                objCompanyMaster.web = (p_str_web == null) ? string.Empty : p_str_web.Trim();
                objCompanyMaster.country = (p_str_country == null) ? string.Empty : p_str_country.Trim();
                objCompanyMaster.group_id = "Comp";
                objCompanyMaster.state_id = (p_str_state_id == null) ? string.Empty : p_str_state_id.Trim();
                objCompanyMaster.zip = (p_str_zip == null) ? string.Empty : p_str_zip.Trim();
                objCompanyMaster.city = (p_str_city == null) ? string.Empty : p_str_city.Trim();
                objCompanyMaster.cell = (p_str_cell == null) ? string.Empty : p_str_cell.Trim();
                objCompanyMaster.attn = (p_str_attn == null) ? string.Empty : p_str_attn.Trim();
                objCompanyMaster.remit_attn = (p_str_remit_attn == null) ? string.Empty : p_str_remit_attn.Trim();
                objCompanyMaster.remit_addr_line1 = (p_str_remit_addr_line1 == null) ? string.Empty : p_str_remit_addr_line1.Trim();
                objCompanyMaster.remit_addr_line2 = (p_str_remit_addr_line2 == null) ? string.Empty : p_str_remit_addr_line2.Trim();
                objCompanyMaster.Remit_City = (p_str_Remit_City == null) ? string.Empty : p_str_Remit_City.Trim();
                objCompanyMaster.remit_cntry_id = (p_str_remit_cntry_id == null) ? string.Empty : p_str_remit_cntry_id.Trim();
                objCompanyMaster.remit_state_id = (p_str_remit_state_id == null) ? string.Empty : p_str_remit_state_id.Trim();
                objCompanyMaster.Remit_Post_Code = (p_str_Remit_Post_Code == null) ? string.Empty : p_str_Remit_Post_Code.Trim();
                objCompanyMaster.remit_state_id = (p_str_remit_state_id == null) ? string.Empty : p_str_remit_state_id.Trim();
                objCompanyMaster.remit_tel = (p_str_remit_tel == null) ? string.Empty : p_str_remit_tel.Trim();
                objCompanyMaster.remit_fax = (p_str_remit_fax == null) ? string.Empty : p_str_remit_fax.Trim();
                objCompanyMaster.remit_email = (p_str_remit_email == null) ? string.Empty : p_str_remit_email.Trim();
                objCompanyMaster.remit_web = (p_str_remit_web == null) ? string.Empty : p_str_remit_web.Trim();
                objCompanyMaster.dflt_whs_id = (p_str_dflt_whs_id == null) ? string.Empty : p_str_dflt_whs_id.Trim();
                objCompanyMaster.whs_name = (p_str_dflt_whs_id == null) ? string.Empty : p_str_dflt_whs_id.Trim();
                objCompanyMaster.action_type = "1";
                ServiceObject.SaveCmpMasterDtls(objCompanyMaster);
                Mapper.CreateMap<CompanyMaster, CompanyMasterModel>();
                CompanyMasterModel objCompanyMastermodel = Mapper.Map<CompanyMaster, CompanyMasterModel>(objCompanyMaster);
                return View("~/Views/CompanyMaster/CompanyMaster.cshtml", objCompanyMastermodel);
            }
            else
            {
                l_int_count = 1;
                return Json(l_int_count, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult CompanyEdit(string p_str_cust_of_cmp_id)
        {
            CompanyMaster objCompanyMaster = new CompanyMaster();
            ICompanyMasterService ServiceObject = new CompanyMasterService();
            objCompanyMaster.cust_of_cmp_id = (p_str_cust_of_cmp_id== null)?string.Empty: p_str_cust_of_cmp_id.Trim();
            if (Session["dflt_cmp_id"].ToString() != null && Session["dflt_cmp_id"].ToString() != string.Empty)
            {
                objPick.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
            }
            else
            {
                objPick.cmp_id = string.Empty;
            }
            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objCompanyMaster.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objCompanyMaster.ListStatePick = objPick.ListStatePick;
            objCompanyMaster = ServiceObject.GetCmpHdrDetails(objCompanyMaster);
            if (objCompanyMaster.ListCmpHdrDetails.Count > 0)
            {
                objCompanyMaster.cust_of_cmp_id = (objCompanyMaster.ListCmpHdrDetails[0].cmp_id == null ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].cmp_id.Trim());
                objCompany.cust_of_cmp_id = (objCompanyMaster.ListCmpHdrDetails[0].cmp_id == null ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].cmp_id.Trim());
                objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
                objCompanyMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
                objCompanyMaster.start_dt = (objCompanyMaster.ListCmpHdrDetails[0].start_dt == null || objCompanyMaster.ListCmpHdrDetails[0].start_dt.Trim() == string.Empty ? string.Empty : Convert.ToDateTime(objCompanyMaster.ListCmpHdrDetails[0].start_dt.Trim()).ToString("MM/dd/yyyy"));
                objCompanyMaster.last_chg_dt = (objCompanyMaster.ListCmpHdrDetails[0].last_chg_dt == null || objCompanyMaster.ListCmpHdrDetails[0].last_chg_dt.Trim() == "" ? string.Empty : Convert.ToDateTime(objCompanyMaster.ListCmpHdrDetails[0].last_chg_dt.Trim()).ToString("MM/dd/yyyy"));
                objCompanyMaster.cmp_name = (objCompanyMaster.ListCmpHdrDetails[0].cmp_name == null ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].cmp_name.Trim());
                objCompanyMaster.status = (objCompanyMaster.ListCmpHdrDetails[0].status == null ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].status.Trim());
                objLookUp.id = "16";
                objLookUp.lookuptype = "CUSTOMERMASTER";
                objLookUp = ServiceObjectLookUp.GetLookUpValue(objLookUp);
                objCompanyMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
                objCompanyMaster.attn = (objCompanyMaster.ListCmpHdrDetails[0].attn == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].attn.Trim();
                objCompanyMaster.addr1 = (objCompanyMaster.ListCmpHdrDetails[0].addr_line1 == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].addr_line1.Trim();
                objCompanyMaster.addr2 = (objCompanyMaster.ListCmpHdrDetails[0].addr_line2 == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].addr_line2.Trim();
                objCompanyMaster.city = (objCompanyMaster.ListCmpHdrDetails[0].city == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].city.Trim();
                objCompanyMaster.state_id = (objCompanyMaster.ListCmpHdrDetails[0] == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].state_id.Trim();
                objCompanyMaster.zip = (objCompanyMaster.ListCmpHdrDetails[0].post_code == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].post_code.Trim();
                objCompanyMaster.country = (objCompanyMaster.ListCmpHdrDetails[0].cntry_id == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].cntry_id.Trim();
                objCompanyMaster.Tel = (objCompanyMaster.ListCmpHdrDetails[0].tel == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].tel.Trim();
                objCompanyMaster.fax = (objCompanyMaster.ListCmpHdrDetails[0].fax == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].fax.Trim();
                objCompanyMaster.web = (objCompanyMaster.ListCmpHdrDetails[0].web == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].web.Trim();
                objCompanyMaster.email = (objCompanyMaster.ListCmpHdrDetails[0].email == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].email.Trim();
                objCompanyMaster.remit_attn = (objCompanyMaster.ListCmpHdrDetails[0].remit_attn == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_attn.Trim();
                objCompanyMaster.remit_addr_line1 = (objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line1 == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line1.Trim();
                objCompanyMaster.remit_addr_line2 = (objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line2 == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line2.Trim();
                objCompanyMaster.Remit_City = (objCompanyMaster.ListCmpHdrDetails[0].remit_city == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_city.Trim();
                objCompanyMaster.remit_state_id = (objCompanyMaster.ListCmpHdrDetails[0].remit_state_id == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_state_id.Trim();
                objCompanyMaster.Remit_Post_Code = (objCompanyMaster.ListCmpHdrDetails[0].remit_post_code == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_post_code.Trim();
                objCompanyMaster.remit_cntry_id = (objCompanyMaster.ListCmpHdrDetails[0].remit_cntry_id == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_cntry_id.Trim();
                objCompanyMaster.remit_tel = (objCompanyMaster.ListCmpHdrDetails[0].remit_tel == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_tel.Trim();
                objCompanyMaster.remit_fax = (objCompanyMaster.ListCmpHdrDetails[0].remit_fax == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_fax.Trim();
                objCompanyMaster.remit_email = (objCompanyMaster.ListCmpHdrDetails[0].remit_email == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_email.Trim();
                objCompanyMaster.remit_web = (objCompanyMaster.ListCmpHdrDetails[0].remit_web == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_web.Trim();
                objCompanyMaster.dflt_whs_id = (objCompanyMaster.ListCmpHdrDetails[0].dflt_whs_id == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].dflt_whs_id.Trim();
            }
            Mapper.CreateMap<CompanyMaster, CompanyMasterModel>();
            CompanyMasterModel objCompanyMastermodel = Mapper.Map<CompanyMaster, CompanyMasterModel>(objCompanyMaster);
            return PartialView("_CmpMasterEdit", objCompanyMastermodel);
        }
        public ActionResult CompanyDelete(string p_str_cust_of_cmp_id)
        {
            CompanyMaster objCompanyMaster = new CompanyMaster();
            ICompanyMasterService ServiceObject = new CompanyMasterService();
            objCompanyMaster.cust_of_cmp_id = (p_str_cust_of_cmp_id== null)?string.Empty: p_str_cust_of_cmp_id.Trim();
            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objCompanyMaster.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objCompanyMaster.ListStatePick = objPick.ListStatePick;
            objCompanyMaster = ServiceObject.GetCmpHdrDetails(objCompanyMaster);
            if (objCompanyMaster.ListCmpHdrDetails.Count > 0)
            {
                objCompanyMaster.cust_of_cmp_id = (objCompanyMaster.ListCmpHdrDetails[0].cmp_id == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].cmp_id.Trim();
                objCompany.cust_of_cmp_id = (objCompanyMaster.ListCmpHdrDetails[0].cmp_id == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].cmp_id.Trim();
                objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
                objCompanyMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
                objCompanyMaster.cmp_name = (objCompanyMaster.ListCmpHdrDetails[0].cmp_name == null ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].cmp_name.Trim());
                objCompanyMaster.start_dt = (objCompanyMaster.ListCmpHdrDetails[0].start_dt == null || objCompanyMaster.ListCmpHdrDetails[0].start_dt.Trim() == string.Empty ? string.Empty : Convert.ToDateTime(objCompanyMaster.ListCmpHdrDetails[0].start_dt.Trim()).ToString("MM/dd/yyyy"));
                objCompanyMaster.last_chg_dt = (objCompanyMaster.ListCmpHdrDetails[0].last_chg_dt == null || objCompanyMaster.ListCmpHdrDetails[0].last_chg_dt.Trim() == string.Empty ? string.Empty : Convert.ToDateTime(objCompanyMaster.ListCmpHdrDetails[0].last_chg_dt.Trim()).ToString("MM/dd/yyyy"));
                objCompanyMaster.status = (objCompanyMaster.ListCmpHdrDetails[0].status == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].status.Trim();
                objLookUp.id = "16";
                objLookUp.lookuptype = "CUSTOMERMASTER";
                objLookUp = ServiceObjectLookUp.GetLookUpValue(objLookUp);
                objCompanyMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
                objCompanyMaster.attn = (objCompanyMaster.ListCmpHdrDetails[0].attn == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].attn.Trim();
                objCompanyMaster.addr1 = (objCompanyMaster.ListCmpHdrDetails[0].addr_line1 == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].addr_line1.Trim();
                objCompanyMaster.addr2 = (objCompanyMaster.ListCmpHdrDetails[0].addr_line2 == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].addr_line2.Trim();
                objCompanyMaster.city = (objCompanyMaster.ListCmpHdrDetails[0].city == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].city.Trim();
                objCompanyMaster.state_id = (objCompanyMaster.ListCmpHdrDetails[0].state_id == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].state_id.Trim();
                objCompanyMaster.zip = (objCompanyMaster.ListCmpHdrDetails[0].post_code == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].post_code.Trim();
                objCompanyMaster.country = (objCompanyMaster.ListCmpHdrDetails[0].cntry_id == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].cntry_id.Trim();
                objCompanyMaster.Tel = (objCompanyMaster.ListCmpHdrDetails[0].tel == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].tel.Trim();
                objCompanyMaster.fax = (objCompanyMaster.ListCmpHdrDetails[0].fax == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].fax.Trim();
                objCompanyMaster.web = (objCompanyMaster.ListCmpHdrDetails[0].web == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].web.Trim();
                objCompanyMaster.email = (objCompanyMaster.ListCmpHdrDetails[0].email == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].email.Trim();
                objCompanyMaster.remit_attn = (objCompanyMaster.ListCmpHdrDetails[0].remit_attn == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_attn.Trim();
                objCompanyMaster.remit_addr_line1 = (objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line1 == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line1.Trim();
                objCompanyMaster.remit_addr_line2 = (objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line2 == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line2.Trim();
                objCompanyMaster.Remit_City = (objCompanyMaster.ListCmpHdrDetails[0].remit_city == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_city.Trim();
                objCompanyMaster.remit_state_id = (objCompanyMaster.ListCmpHdrDetails[0].remit_state_id == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_state_id.Trim();
                objCompanyMaster.Remit_Post_Code = (objCompanyMaster.ListCmpHdrDetails[0].remit_post_code == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_post_code.Trim();
                objCompanyMaster.remit_cntry_id = (objCompanyMaster.ListCmpHdrDetails[0].remit_cntry_id == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_cntry_id.Trim();
                objCompanyMaster.remit_tel = (objCompanyMaster.ListCmpHdrDetails[0].remit_tel == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_tel.Trim();
                objCompanyMaster.remit_fax = (objCompanyMaster.ListCmpHdrDetails[0].remit_fax == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_fax.Trim();
                objCompanyMaster.remit_email = (objCompanyMaster.ListCmpHdrDetails[0].remit_email == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_email.Trim();
                objCompanyMaster.remit_web = (objCompanyMaster.ListCmpHdrDetails[0].remit_web == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_web.Trim();
                objCompanyMaster.dflt_whs_id = (objCompanyMaster.ListCmpHdrDetails[0].dflt_whs_id == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].dflt_whs_id.Trim();
            }
            objCompanyMaster.View_Flag = "D";
            Mapper.CreateMap<CompanyMaster, CompanyMasterModel>();
            CompanyMasterModel objCompanyMastermodel = Mapper.Map<CompanyMaster, CompanyMasterModel>(objCompanyMaster);
            return PartialView("_CmpMasterDel", objCompanyMastermodel);
        }
        public ActionResult CompanyView(string p_str_cust_of_cmp_id)
        {
            CompanyMaster objCompanyMaster = new CompanyMaster();
            ICompanyMasterService ServiceObject = new CompanyMasterService();
            objCompanyMaster.cust_of_cmp_id = (p_str_cust_of_cmp_id== null)?string.Empty: p_str_cust_of_cmp_id.Trim();
            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objCompanyMaster.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objCompanyMaster.ListStatePick = objPick.ListStatePick;
            objCompanyMaster = ServiceObject.GetCmpHdrDetails(objCompanyMaster);
            if (objCompanyMaster.ListCmpHdrDetails.Count > 0)
            {

                objCompanyMaster.cmp_name = (objCompanyMaster.ListCmpHdrDetails[0].cmp_name == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].cmp_name.Trim();
                objCompanyMaster.cust_of_cmp_id = (objCompanyMaster.ListCmpHdrDetails[0].cmp_id == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].cmp_id.Trim();
                objCompany.cust_of_cmp_id = (objCompanyMaster.ListCmpHdrDetails[0].cmp_id == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].cmp_id.Trim();
                objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
                objCompanyMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
                objCompanyMaster.start_dt = (objCompanyMaster.ListCmpHdrDetails[0].start_dt == null || objCompanyMaster.ListCmpHdrDetails[0].start_dt.Trim() == string.Empty ? string.Empty : Convert.ToDateTime(objCompanyMaster.ListCmpHdrDetails[0].start_dt.Trim()).ToString("MM/dd/yyyy"));
                objCompanyMaster.last_chg_dt = (objCompanyMaster.ListCmpHdrDetails[0].last_chg_dt == null || objCompanyMaster.ListCmpHdrDetails[0].last_chg_dt.Trim() == string.Empty ? string.Empty : Convert.ToDateTime(objCompanyMaster.ListCmpHdrDetails[0].last_chg_dt.Trim()).ToString("MM/dd/yyyy"));
                objCompanyMaster.status = objCompanyMaster.ListCmpHdrDetails[0].status.Trim();
                objLookUp.id = "16";
                objLookUp.lookuptype = "CUSTOMERMASTER";
                objLookUp = ServiceObjectLookUp.GetLookUpValue(objLookUp);
                objCompanyMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
                objCompanyMaster.attn = (objCompanyMaster.ListCmpHdrDetails[0].attn == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].attn.Trim();
                objCompanyMaster.addr1 = (objCompanyMaster.ListCmpHdrDetails[0].addr_line1 == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].addr_line1.Trim();
                objCompanyMaster.addr2 = (objCompanyMaster.ListCmpHdrDetails[0].addr_line2 == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].addr_line2.Trim();
                objCompanyMaster.city = (objCompanyMaster.ListCmpHdrDetails[0].city == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].city.Trim();
                objCompanyMaster.state_id = (objCompanyMaster.ListCmpHdrDetails[0].state_id == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].state_id.Trim();
                objCompanyMaster.zip = (objCompanyMaster.ListCmpHdrDetails[0].post_code == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].post_code.Trim();
                objCompanyMaster.country = (objCompanyMaster.ListCmpHdrDetails[0].cntry_id == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].cntry_id.Trim();
                objCompanyMaster.Tel = (objCompanyMaster.ListCmpHdrDetails[0].tel == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].tel.Trim();
                objCompanyMaster.fax = (objCompanyMaster.ListCmpHdrDetails[0].fax == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].fax.Trim();
                objCompanyMaster.web = (objCompanyMaster.ListCmpHdrDetails[0].web == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].web.Trim();
                objCompanyMaster.email = (objCompanyMaster.ListCmpHdrDetails[0].email == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].email.Trim();
                objCompanyMaster.remit_attn = (objCompanyMaster.ListCmpHdrDetails[0].remit_attn == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_attn.Trim();
                objCompanyMaster.remit_addr_line1 = (objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line1 == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line1.Trim();
                objCompanyMaster.remit_addr_line2 = (objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line2 == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_addr_line2.Trim();
                objCompanyMaster.Remit_City = (objCompanyMaster.ListCmpHdrDetails[0].remit_city == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_city.Trim();
                objCompanyMaster.remit_state_id = (objCompanyMaster.ListCmpHdrDetails[0].remit_state_id == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_state_id.Trim();
                objCompanyMaster.Remit_Post_Code = (objCompanyMaster.ListCmpHdrDetails[0].remit_post_code == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_post_code.Trim();
                objCompanyMaster.remit_cntry_id = (objCompanyMaster.ListCmpHdrDetails[0].remit_cntry_id == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_cntry_id.Trim();
                objCompanyMaster.remit_tel = (objCompanyMaster.ListCmpHdrDetails[0].remit_tel == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_tel.Trim();
                objCompanyMaster.remit_fax = (objCompanyMaster.ListCmpHdrDetails[0].remit_fax == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_fax.Trim();
                objCompanyMaster.remit_email = (objCompanyMaster.ListCmpHdrDetails[0].remit_email == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_email.Trim();
                objCompanyMaster.remit_web = (objCompanyMaster.ListCmpHdrDetails[0].remit_web == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].remit_web.Trim();
                objCompanyMaster.dflt_whs_id = (objCompanyMaster.ListCmpHdrDetails[0].dflt_whs_id == null) ? string.Empty : objCompanyMaster.ListCmpHdrDetails[0].dflt_whs_id.Trim();
            }
            objCompanyMaster.View_Flag = "V";
            Mapper.CreateMap<CompanyMaster, CompanyMasterModel>();
            CompanyMasterModel objCompanyMastermodel = Mapper.Map<CompanyMaster, CompanyMasterModel>(objCompanyMaster);
            return PartialView("_CmpMasterDel", objCompanyMastermodel);
        }
        public ActionResult UpdateCmpDtls(string p_str_cmp_id, string p_str_cmp_name, string p_str_start_dt, string p_str_last_chg_dt, string p_str_status, string p_str_addr1, string p_str_addr2,
          string p_str_city, string p_str_state_id, string p_str_zip, string p_str_country,
           string p_str_attn, string p_str_tel,string p_str_fax, string p_str_email, string p_str_web, string p_str_remit_attn, string p_str_remit_addr_line1, string p_str_remit_addr_line2, string p_str_Remit_City,
           string p_str_remit_state_id, string p_str_Remit_Post_Code, string p_str_remit_cntry_id, string p_str_remit_tel, string p_str_remit_fax, string p_str_remit_email, string p_str_remit_web, string p_str_dflt_whs_id)
        {
           // int l_int_count = 0;
            CompanyMaster objCompanyMaster = new CompanyMaster();
            ICompanyMasterService ServiceObject = new CompanyMasterService();
            objCompanyMaster.cust_of_cmp_id = (p_str_cmp_id==null)?string.Empty:p_str_cmp_id.Trim();
            objCompanyMaster.cmp_name = (p_str_cmp_name== null)?string.Empty: p_str_cmp_name.Trim();
            objCompanyMaster.start_dt = (p_str_start_dt == null) ? string.Empty : p_str_start_dt.Trim();
            objCompanyMaster.last_chg_dt = (p_str_last_chg_dt== null)?string.Empty: p_str_last_chg_dt.Trim();
            objCompanyMaster.status = (p_str_status== null)?string.Empty: p_str_status.Trim();
            objCompanyMaster.addr1 = (p_str_addr1 == null) ? string.Empty : p_str_addr1.Trim();
            objCompanyMaster.addr2 = (p_str_addr2== null)?string.Empty: p_str_addr2.Trim();
            objCompanyMaster.Tel = (p_str_tel== null)?string.Empty: p_str_tel.Trim();
            objCompanyMaster.fax = (p_str_fax== null)?string.Empty: p_str_fax.Trim();
            objCompanyMaster.email = (p_str_email== null)?string.Empty: p_str_email.Trim();
            objCompanyMaster.web = (p_str_web== null)?string.Empty: p_str_web.Trim();
            objCompanyMaster.country = (p_str_country== null)?string.Empty: p_str_country.Trim();
            objCompanyMaster.group_id = "Comp";
            objCompanyMaster.state_id = (p_str_state_id== null)?string.Empty: p_str_state_id.Trim();
            objCompanyMaster.zip = (p_str_zip== null)?string.Empty: p_str_zip.Trim();
            objCompanyMaster.city = (p_str_city== null)?string.Empty: p_str_city.Trim();
            objCompanyMaster.attn = (p_str_attn== null)?string.Empty: p_str_attn.Trim();
            objCompanyMaster.remit_attn = (p_str_remit_attn== null)?string.Empty: p_str_remit_attn.Trim();
            objCompanyMaster.remit_addr_line1 = (p_str_remit_addr_line1== null)?string.Empty: p_str_remit_addr_line1.Trim();
            objCompanyMaster.remit_addr_line2 = (p_str_remit_addr_line2== null)?string.Empty: p_str_remit_addr_line2.Trim();
            objCompanyMaster.Remit_City = (p_str_Remit_City== null)?string.Empty: p_str_Remit_City.Trim();
            objCompanyMaster.remit_cntry_id = (p_str_remit_cntry_id == null) ? string.Empty : p_str_remit_cntry_id.Trim();
            objCompanyMaster.remit_state_id = (p_str_remit_state_id== null) ? string.Empty : p_str_remit_state_id.Trim();
            objCompanyMaster.Remit_Post_Code = (p_str_Remit_Post_Code== null) ? string.Empty : p_str_Remit_Post_Code.Trim();
            objCompanyMaster.remit_tel = (p_str_remit_tel== null) ? string.Empty : p_str_remit_tel.Trim();
            objCompanyMaster.remit_fax = (p_str_remit_fax == null) ? string.Empty : p_str_remit_fax.Trim();
            objCompanyMaster.remit_email = (p_str_remit_email== null) ? string.Empty : p_str_remit_email.Trim();
            objCompanyMaster.remit_web = (p_str_remit_web== null) ? string.Empty : p_str_remit_web.Trim();
            objCompanyMaster.dflt_whs_id = (p_str_dflt_whs_id== null) ? string.Empty : p_str_dflt_whs_id.Trim();
            objCompanyMaster.action_type = "2";
            //objCompanyMaster = ServiceObject.CheckExistwhsId(objCompanyMaster);
            //{
            //    if (objCompanyMaster.ListCheckExistwhsId.Count == 0)
            //    {
            //        
            //    }
            //    else
            //    {
            //        l_int_count = 1;
            //        return Json(l_int_count, JsonRequestBehavior.AllowGet);
            //    }

            //}
            ServiceObject.UpdateCmpMasterDtls(objCompanyMaster);
            Mapper.CreateMap<CompanyMaster, CompanyMasterModel>();
            CompanyMasterModel objCompanyMastermodel = Mapper.Map<CompanyMaster, CompanyMasterModel>(objCompanyMaster);
            return View("~/Views/CompanyMaster/CompanyMaster.cshtml", objCompanyMastermodel);
        }


        public ActionResult GetCompanyDetails(string p_str_cmp_id)
        {
            try
            {
                CompanyMaster objCompanyMaster = new CompanyMaster();
                ICompanyMasterService ServiceObject = new CompanyMasterService();
                objCompanyMaster.cust_of_cmp_id = (p_str_cmp_id== null)?string.Empty: p_str_cmp_id.Trim();
                Session["g_str_Search_flag"] = "True";
                objCompanyMaster.is_company_user = Session["IsCompanyUser"].ToString();
                objCompanyMaster = ServiceObject.GetCmpMasterDetails(objCompanyMaster);
                objCompany.cust_of_cmp_id = string.Empty;
                objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
                objCompanyMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
                Mapper.CreateMap<CompanyMaster, CompanyMasterModel>();
                CompanyMasterModel objCompanyMastermodel = Mapper.Map<CompanyMaster, CompanyMasterModel>(objCompanyMaster);
                return PartialView("_CompanyMaster", objCompanyMastermodel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult DeleteCmpDtls(string p_str_cmp_id)
        {
            int result = 0;
            CompanyMaster objCompanyMaster = new CompanyMaster();
            ICompanyMasterService ServiceObject = new CompanyMasterService();
            objCompanyMaster.cust_of_cmp_id = (p_str_cmp_id== null)?string.Empty: p_str_cmp_id.Trim();
            objCompanyMaster = ServiceObject.CheckCustDetails(objCompanyMaster);
            if(objCompanyMaster.ListCheckCustDtl.Count==0)
            {
                result =1;
                ServiceObject.DeleteCmp(objCompanyMaster);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result = 0;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
           
            
        }
        public ActionResult CompanyAdd(string p_str_cmp_id)
        {

            CompanyMaster objCompanyMaster = new CompanyMaster();
            ICompanyMasterService ServiceObject = new CompanyMasterService();          
            objCompany.cust_of_cmp_id = string.Empty;
            objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
            objCompanyMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
            objCompanyMaster.last_chg_dt = DateTime.Now.ToString("MM/dd/yyyy");
            objCompanyMaster.start_dt = DateTime.Now.ToString("MM/dd/yyyy");
            objPick.cmp_id = (p_str_cmp_id== null)?string.Empty: p_str_cmp_id.Trim();
            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objCompanyMaster.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objCompanyMaster.ListStatePick = objPick.ListStatePick;
            objLookUp.id = "16";
            objLookUp.lookuptype = "CUSTOMERMASTER";
            objLookUp = ServiceObjectLookUp.GetLookUpValue(objLookUp);
            objCompanyMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
            Mapper.CreateMap<CompanyMaster, CompanyMasterModel>();
            CompanyMasterModel objCompanyMastermodel = Mapper.Map<CompanyMaster, CompanyMasterModel>(objCompanyMaster);
            return PartialView("_AddCompany", objCompanyMastermodel);
        }

        public JsonResult Company_MASTER_INQ_HDR_DATA(string p_str_cmp_id)
        {
            CompanyMaster objCompanyMaster = new CompanyMaster();
            ICompanyMasterService ServiceObject = new CompanyMasterService();
            Session["TEMP_CMP_ID"] = (p_str_cmp_id== null)?string.Empty: p_str_cmp_id.Trim();
            return Json(objCompanyMaster.MasterCount, JsonRequestBehavior.AllowGet);

        }
        public ActionResult CompanyMasterDetail(string p_str_cmp_id)
        {
            try
            {

                CompanyMaster objCompanyMaster = new CompanyMaster();
                ICompanyMasterService ServiceObject = new CompanyMasterService();
                string l_str_search_flag = string.Empty;
                string l_str_is_another_usr = string.Empty;
                if(Session["g_str_Search_flag"].ToString()!=null && Session["g_str_Search_flag"].ToString()!=string.Empty)
                {
                    l_str_search_flag = Session["g_str_Search_flag"].ToString().Trim();
                }
                else
                {
                    l_str_search_flag = "False";
                }
                objCompanyMaster.is_company_user = Session["IsCompanyUser"].ToString();
                if (l_str_search_flag == "True")
                {
                    objCompanyMaster.cust_of_cmp_id = Session["TEMP_CMP_ID"].ToString().Trim();
                    objCompanyMaster.is_company_user = Session["IsCompanyUser"].ToString().Trim();

                }
                else
                {
                    objCompanyMaster.cust_of_cmp_id = (p_str_cmp_id==null)?string.Empty : p_str_cmp_id.Trim();                   
                }

                objCompanyMaster = ServiceObject.GetCmpMasterDetails(objCompanyMaster);
                objCompany.cust_of_cmp_id = string.Empty;
                objCompany = ServiceObjectCompany.GetCustOfCompanyDetails(objCompany);
                objCompanyMaster.ListCustofCompanyPickDtl = objCompany.ListCustofCompanyPickDtl;
                Mapper.CreateMap<CompanyMaster, CompanyMasterModel>();
                CompanyMasterModel objCompanyMastermodel = Mapper.Map<CompanyMaster, CompanyMasterModel>(objCompanyMaster);
                return PartialView("_CompanyMaster", objCompanyMastermodel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult CountryChange(string p_str_cntry_id)
        {
            CompanyMaster objCompanyMaster = new CompanyMaster();
            objPick.Cntry_Id = (p_str_cntry_id==null)?string.Empty: p_str_cntry_id.Trim();
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objCompanyMaster.ListAddStatePick = objPick.ListStatePick;
            objCompanyMaster.ListEditStatePick = objPick.ListStatePick;
            objCompanyMaster.ListAddRemitStatePick = objPick.ListStatePick;
            objCompanyMaster.ListEditRemitStatePick = objPick.ListStatePick;
            var serializer = new JavaScriptSerializer() { MaxJsonLength = 86753090 };
            serializer.Serialize(objCompanyMaster);
            return new JsonResult()
            {
                Data = objCompanyMaster,
                MaxJsonLength = 86753090
            };

        }
    }
}