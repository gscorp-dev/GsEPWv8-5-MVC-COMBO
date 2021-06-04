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
using System.Web.UI.WebControls;

namespace GsEPWv8_5_MVC.Controllers
{

    public class WhsMasterController : Controller
    {
        public bool Is_Whs_ID_Exist;
        public bool Is_Whs_ID_In_Use;
        IWhsMasterService ServiceObject = new WhsMasterService();
        LookUp objLookUp = new LookUp();
        LookUpService ServiceObject1 = new LookUpService();
        Pick objPick = new Pick();
        PickService ServiceObjectPick = new PickService();
        public ActionResult WhsMaster(string FullFillType, string cmp)
        {
            WhsMaster objWhsMaster = new WhsMaster();
            string l_str_cmp_id = string.Empty;
            string l_str_tmp_cmp_id = string.Empty;                

            try
            {
                objWhsMaster.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                if (objWhsMaster.cmp_id == null || objWhsMaster.cmp_id == string.Empty)
                {
                    objWhsMaster.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                }
                else
                {
                    objWhsMaster.cmp_id = Session["g_str_cmp_id"].ToString();
                }
              
                if (objWhsMaster.cmp_id != "" && FullFillType == null)
                {
                    if(Session["UserID"].ToString()!=null)
                    {
                        objWhsMaster.user_id = Session["UserID"].ToString().Trim();
                    }
                    else
                    {
                        objWhsMaster.user_id = string.Empty;
                    }
                    objWhsMaster = ServiceObject.GetPickCompanyDetails(objWhsMaster);
                   
                }
                else
                {
                    if (Session["UserID"].ToString() != null)
                    {
                        objWhsMaster.user_id = Session["UserID"].ToString().Trim();
                    }
                    else
                    {
                        objWhsMaster.user_id = string.Empty;
                    }
                    objWhsMaster = ServiceObject.GetPickCompanyDetails(objWhsMaster);
                }
               
                objLookUp.id = "7";
                objLookUp.lookuptype = "WhsMaster";
                objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                objWhsMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
                Mapper.CreateMap<WhsMaster, WhsMasterModel>();
                WhsMasterModel objWhsMasterModel = Mapper.Map<WhsMaster, WhsMasterModel>(objWhsMaster);

                return View(objWhsMasterModel);
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
        public ActionResult GetWhsMasterDetails(string f_str_cmp_id, string f_str_whs_id, string f_str_whs_name)
        {
            WhsMaster objWhsMaster = new WhsMaster();
            objWhsMaster.cmp_id = f_str_cmp_id;
            objWhsMaster.whs_id = f_str_whs_id;
            objWhsMaster.whs_name = f_str_whs_name;
            objWhsMaster = ServiceObject.GetWhsMasterDetails(objWhsMaster);
            Mapper.CreateMap<WhsMaster, WhsMasterModel>();
            WhsMasterModel objWhsMasterModel = Mapper.Map<WhsMaster, WhsMasterModel>(objWhsMaster);
            return PartialView("_WhsMaster", objWhsMasterModel);
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddWhs(string a_str_cmpid)
        {
            WhsMaster objWhsMaster = new WhsMaster();
          
            objWhsMaster.cmp_id = a_str_cmpid;
            if (Session["UserID"].ToString() != null)
            {
                objWhsMaster.user_id = Session["UserID"].ToString().Trim();
            }
            else
            {
                objWhsMaster.user_id = string.Empty;
            }
            objWhsMaster = ServiceObject.GetPickCompanyDetails(objWhsMaster);
            objPick.cmp_id = a_str_cmpid;
            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objWhsMaster.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objWhsMaster.ListStatePick = objPick.ListStatePick;
            objLookUp.id = "16";
            objLookUp.lookuptype = "CUSTOMERMASTER";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objWhsMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
            objWhsMaster.StartDate = DateTime.Now.ToString("MM-dd-yyyy");
            objWhsMaster.lastDate = DateTime.Now.ToString("MM-dd-yyyy");
            objWhsMaster.View_Flag = "A";
            Mapper.CreateMap<WhsMaster, WhsMasterModel>();
            WhsMasterModel objWhsMastermodel = Mapper.Map<WhsMaster, WhsMasterModel>(objWhsMaster);
            return PartialView("_WhsMasterEntry", objWhsMastermodel);
        }
        public ActionResult ViewWhs(string v_str_cmp_id, string v_str_whs_id,string v_str_whs_name)
        {
            WhsMaster objWhsMaster = new WhsMaster();
            objWhsMaster.cmp_id = v_str_cmp_id;
            objWhsMaster.whs_id = v_str_whs_id;
            objWhsMaster.whs_name = v_str_whs_name;
            if (Session["UserID"].ToString() != null)
            {
                objWhsMaster.user_id = Session["UserID"].ToString().Trim();
            }
            else
            {
                objWhsMaster.user_id = string.Empty;
            }
            objWhsMaster = ServiceObject.GetPickCompanyDetails(objWhsMaster);
            objPick.cmp_id = v_str_cmp_id;
            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objWhsMaster.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objWhsMaster.ListStatePick = objPick.ListStatePick;
            objLookUp.id = "16";
            objLookUp.lookuptype = "CUSTOMERMASTER";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objWhsMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
            objWhsMaster = ServiceObject.GetWhsMasterDetails(objWhsMaster);
            if (objWhsMaster.ListWhsMaster.Count>0)
            {
                objWhsMaster.StartDate = objWhsMaster.ListWhsMaster[0].start_dt.ToShortDateString();
                objWhsMaster.lastDate = objWhsMaster.ListWhsMaster[0].last_chg_dt.ToShortDateString();
                objWhsMaster.status = objWhsMaster.ListWhsMaster[0].status;
                objWhsMaster.attn = objWhsMaster.ListWhsMaster[0].attn;
                objWhsMaster.mail_name = objWhsMaster.ListWhsMaster[0].mail_name;
                objWhsMaster.addr_line1 = objWhsMaster.ListWhsMaster[0].addr_line1;
                objWhsMaster.addr_line2 = objWhsMaster.ListWhsMaster[0].addr_line2;
                objWhsMaster.city = objWhsMaster.ListWhsMaster[0].city;
                objWhsMaster.state_id = objWhsMaster.ListWhsMaster[0].state_id;
                objWhsMaster.post_code = objWhsMaster.ListWhsMaster[0].post_code;
                objWhsMaster.cntry_id = objWhsMaster.ListWhsMaster[0].cntry_id;
                objWhsMaster.tel = objWhsMaster.ListWhsMaster[0].tel;
                objWhsMaster.cell = objWhsMaster.ListWhsMaster[0].cell;
                objWhsMaster.fax = objWhsMaster.ListWhsMaster[0].fax;
                objWhsMaster.email = objWhsMaster.ListWhsMaster[0].fax;
                objWhsMaster.web = objWhsMaster.ListWhsMaster[0].web;
                objWhsMaster.dft_whs = objWhsMaster.ListWhsMaster[0].dft_whs;

            }
            objWhsMaster.View_Flag = "V";
            Mapper.CreateMap<WhsMaster, WhsMasterModel>();
            WhsMasterModel objWhsMastermodel = Mapper.Map<WhsMaster, WhsMasterModel>(objWhsMaster);
            return PartialView("_WhsMasterEntry", objWhsMastermodel);
        }
        public ActionResult DeleteWhs(string D_str_cmp_id, string D_str_whs_id, string D_str_whs_name)
        {
            WhsMaster objWhsMaster = new WhsMaster();
            objWhsMaster.cmp_id = D_str_cmp_id;
            objWhsMaster.whs_id = D_str_whs_id;
            objWhsMaster.whs_name = D_str_whs_name;
            if (Session["UserID"].ToString() != null)
            {
                objWhsMaster.user_id = Session["UserID"].ToString().Trim();
            }
            else
            {
                objWhsMaster.user_id = string.Empty;
            }
            objWhsMaster = ServiceObject.GetPickCompanyDetails(objWhsMaster);
            objPick.cmp_id = D_str_cmp_id;
            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objWhsMaster.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objWhsMaster.ListStatePick = objPick.ListStatePick;
            objLookUp.id = "16";
            objLookUp.lookuptype = "CUSTOMERMASTER";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objWhsMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
            objWhsMaster = ServiceObject.GetWhsMasterDetails(objWhsMaster);
            if (objWhsMaster.ListWhsMaster.Count > 0)
            {
                objWhsMaster.StartDate = objWhsMaster.ListWhsMaster[0].start_dt.ToShortDateString();
                objWhsMaster.lastDate = objWhsMaster.ListWhsMaster[0].last_chg_dt.ToShortDateString();
                objWhsMaster.status = objWhsMaster.ListWhsMaster[0].status;
                objWhsMaster.attn = objWhsMaster.ListWhsMaster[0].attn;
                objWhsMaster.mail_name = objWhsMaster.ListWhsMaster[0].mail_name;
                objWhsMaster.addr_line1 = objWhsMaster.ListWhsMaster[0].addr_line1;
                objWhsMaster.addr_line2 = objWhsMaster.ListWhsMaster[0].addr_line2;
                objWhsMaster.city = objWhsMaster.ListWhsMaster[0].city;
                objWhsMaster.state_id = objWhsMaster.ListWhsMaster[0].state_id;
                objWhsMaster.post_code = objWhsMaster.ListWhsMaster[0].post_code;
                objWhsMaster.cntry_id = objWhsMaster.ListWhsMaster[0].cntry_id;
                objWhsMaster.tel = objWhsMaster.ListWhsMaster[0].tel;
                objWhsMaster.cell = objWhsMaster.ListWhsMaster[0].cell;
                objWhsMaster.fax = objWhsMaster.ListWhsMaster[0].fax;
                objWhsMaster.email = objWhsMaster.ListWhsMaster[0].fax;
                objWhsMaster.web = objWhsMaster.ListWhsMaster[0].web;
                objWhsMaster.dft_whs = objWhsMaster.ListWhsMaster[0].dft_whs;

            }
            objWhsMaster.View_Flag = "D";
            Mapper.CreateMap<WhsMaster, WhsMasterModel>();
            WhsMasterModel objWhsMastermodel = Mapper.Map<WhsMaster, WhsMasterModel>(objWhsMaster);
            return PartialView("_WhsMasterEntry", objWhsMastermodel);
        }
        public ActionResult EditWhs(string E_str_cmp_id, string E_str_whs_id, string E_str_whs_name)
        {
            WhsMaster objWhsMaster = new WhsMaster();
            objWhsMaster.cmp_id = E_str_cmp_id;
            objWhsMaster.whs_id = E_str_whs_id;
            objWhsMaster.whs_name = E_str_whs_name;
            if (Session["UserID"].ToString() != null)
            {
                objWhsMaster.user_id = Session["UserID"].ToString().Trim();
            }
            else
            {
                objWhsMaster.user_id = string.Empty;
            }
            objWhsMaster = ServiceObject.GetPickCompanyDetails(objWhsMaster);
            objPick.cmp_id = E_str_cmp_id;
            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objWhsMaster.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objWhsMaster.ListStatePick = objPick.ListStatePick;
            objLookUp.id = "16";
            objLookUp.lookuptype = "CUSTOMERMASTER";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objWhsMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
            objWhsMaster = ServiceObject.GetWhsMasterDetails(objWhsMaster);
            if (objWhsMaster.ListWhsMaster.Count > 0)
            {
                objWhsMaster.StartDate = objWhsMaster.ListWhsMaster[0].start_dt.ToShortDateString();
                objWhsMaster.lastDate = objWhsMaster.ListWhsMaster[0].last_chg_dt.ToShortDateString();
                objWhsMaster.status = objWhsMaster.ListWhsMaster[0].status;
                objWhsMaster.attn = objWhsMaster.ListWhsMaster[0].attn;
                objWhsMaster.mail_name = objWhsMaster.ListWhsMaster[0].mail_name;
                objWhsMaster.addr_line1 = objWhsMaster.ListWhsMaster[0].addr_line1;
                objWhsMaster.addr_line2 = objWhsMaster.ListWhsMaster[0].addr_line2;
                objWhsMaster.city = objWhsMaster.ListWhsMaster[0].city;
                objWhsMaster.state_id = objWhsMaster.ListWhsMaster[0].state_id;
                objWhsMaster.post_code = objWhsMaster.ListWhsMaster[0].post_code;
                objWhsMaster.cntry_id = objWhsMaster.ListWhsMaster[0].cntry_id;
                objWhsMaster.tel = objWhsMaster.ListWhsMaster[0].tel;
                objWhsMaster.cell = objWhsMaster.ListWhsMaster[0].cell;
                objWhsMaster.fax = objWhsMaster.ListWhsMaster[0].fax;
                objWhsMaster.email = objWhsMaster.ListWhsMaster[0].fax;
                objWhsMaster.web = objWhsMaster.ListWhsMaster[0].web;
                objWhsMaster.dft_whs = objWhsMaster.ListWhsMaster[0].dft_whs;

            }
            objWhsMaster.View_Flag = "M";
            Mapper.CreateMap<WhsMaster, WhsMasterModel>();
            WhsMasterModel objWhsMastermodel = Mapper.Map<WhsMaster, WhsMasterModel>(objWhsMaster);
            return PartialView("_WhsMasterEntry", objWhsMastermodel);
        }
        public ActionResult InsertWhsMaster(string I_str_cmp_id, string I_str_whs_id, string I_str_whs_name, string I_str_whs_attn, string I_str_whs_mail_name, string I_str_whs_city, string I_str_whs_StartDate,
                                            string I_str_whs_lastDate, string I_str_whs_status, string I_str_whs_addr_line1, string I_str_whs_addr_line2, string I_str_whs_state_id, string I_str_whs_post_code, string I_str_whs_cntry_id, string I_str_whs_tel,
                                            string I_str_whs_cell, string I_str_whs_fax, string I_str_whs_email, string I_str_whs_web,bool I_str_chk_whs_id,string I_str_action_type)
        {
            WhsMaster objWhsMaster = new WhsMaster();
            objWhsMaster.cmp_id = (I_str_cmp_id==null)?"": I_str_cmp_id.Trim();
            objWhsMaster.whs_id = (I_str_whs_id==null)?"": I_str_whs_id.Trim();
            objWhsMaster.whs_name = (I_str_whs_name.Trim() == null) ? "" : I_str_whs_name.Trim();
            if (I_str_action_type == "1")
            {
                objWhsMaster = ServiceObject.CheckWhsIDIsExist(objWhsMaster);
                if (objWhsMaster.LstCheckWhsId.Count() > 0)
                {
                    Is_Whs_ID_Exist = false;
                    return Json(Is_Whs_ID_Exist, JsonRequestBehavior.AllowGet);
                }
            }
            objWhsMaster.status = (I_str_whs_status == null) ? "" : I_str_whs_status.Trim();
            objWhsMaster.StartDate = (I_str_whs_StartDate == null || I_str_whs_StartDate == "") ? "" : I_str_whs_StartDate;
            objWhsMaster.lastDate = (I_str_whs_lastDate == null || I_str_whs_lastDate == "") ? "" : I_str_whs_lastDate; ;
            objWhsMaster.attn = (I_str_whs_attn== null || I_str_whs_attn == "") ? "" : I_str_whs_attn.Trim();
            objWhsMaster.mail_name = (I_str_whs_mail_name == null || I_str_whs_mail_name == "") ? "" : I_str_whs_mail_name.Trim(); 
            objWhsMaster.addr_line1 = (I_str_whs_addr_line1 == null || I_str_whs_addr_line1 == "") ? "" : I_str_whs_addr_line1.Trim();
            objWhsMaster.addr_line2 = (I_str_whs_addr_line2== null || I_str_whs_addr_line2 == "") ? "" : I_str_whs_addr_line2.Trim();
            objWhsMaster.city = (I_str_whs_city== null || I_str_whs_city == "") ? "" : I_str_whs_city.Trim(); 
            objWhsMaster.state_id = (I_str_whs_state_id == null || I_str_whs_state_id == "") ? "" : I_str_whs_state_id.Trim(); 
            objWhsMaster.post_code = (I_str_whs_post_code == null || I_str_whs_post_code == "") ? "" : I_str_whs_post_code.Trim(); 
            objWhsMaster.cntry_id = (I_str_whs_cntry_id == null || I_str_whs_cntry_id == "") ? "" : I_str_whs_cntry_id.Trim(); 
            objWhsMaster.tel = (I_str_whs_tel == null || I_str_whs_tel == "") ? "" : I_str_whs_tel.Trim(); 
            objWhsMaster.cell = (I_str_whs_cell == null || I_str_whs_cell == "") ? "" : I_str_whs_cell.Trim(); 
            objWhsMaster.fax = (I_str_whs_fax== null || I_str_whs_fax == "") ? "" : I_str_whs_fax.Trim(); 
            objWhsMaster.email = (I_str_whs_email == null || I_str_whs_email == "") ? "" : I_str_whs_email.Trim(); 
            objWhsMaster.web = (I_str_whs_web== null || I_str_whs_web == "") ? "" : I_str_whs_web.Trim();           
            objWhsMaster.notes = string.Empty;
            objWhsMaster.dft_whs = I_str_chk_whs_id;
            if (I_str_action_type == "1")
            {
                objWhsMaster.process_id = "Add";
            }
            else
            {
                objWhsMaster.process_id = "Edit";
            }
            objWhsMaster.action_type = I_str_action_type;
            ServiceObject.SaveWhsMasterHdr(objWhsMaster);
            if (I_str_action_type == "1")
            {
                Is_Whs_ID_Exist = true;
                return Json(Is_Whs_ID_Exist, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
      
        public ActionResult DeleteWhsMaster(string D_str_cmp_id, string D_str_whs_id, string D_str_whs_name,string D_str_action_type)
        {
            WhsMaster objWhsMaster = new WhsMaster();
            objWhsMaster.cmp_id = D_str_cmp_id.Trim();
            objWhsMaster.whs_id = D_str_whs_id.Trim();
            objWhsMaster.whs_name = D_str_whs_name.Trim();
            objWhsMaster = ServiceObject.DefltWhsCannotDelete(objWhsMaster);
            if (objWhsMaster.LstCheckWhsIdnotdel[0].row_count >0)
            {
                int l_str_del_reason =1;
                return Json(l_str_del_reason, JsonRequestBehavior.AllowGet);

            }
            objWhsMaster = ServiceObject.CheckWhsIDInUse(objWhsMaster);
            if (objWhsMaster.LstCheckWhsId[0].row_count==0)
            {
                objWhsMaster.action_type = D_str_action_type;
                ServiceObject.SaveWhsMasterHdr(objWhsMaster);

                int l_str_del_reason = 2;
               // Is_Whs_ID_In_Use = true;
              //  return Json(Is_Whs_ID_In_Use, JsonRequestBehavior.AllowGet);
                return Json(l_str_del_reason, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Is_Whs_ID_In_Use = false;
                int l_str_del_reason =3;
              //  return Json(Is_Whs_ID_In_Use, JsonRequestBehavior.AllowGet);
                return Json(l_str_del_reason, JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult CountryChange(string l_str_cntry_id)
        {
            WhsMaster objWhsMaster = new WhsMaster();
            objPick.Cntry_Id = l_str_cntry_id.Trim();
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objWhsMaster.ListAddStatePick = objPick.ListStatePick;
            objWhsMaster.ListEditStatePick = objPick.ListStatePick;
            var serializer = new JavaScriptSerializer() { MaxJsonLength = 86753090 };
            serializer.Serialize(objWhsMaster);
            return new JsonResult()
            {
                Data = objWhsMaster,
                MaxJsonLength = 86753090
            };

        }

    }
}





