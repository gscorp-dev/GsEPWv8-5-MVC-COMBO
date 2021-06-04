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
using System.Web.UI.WebControls;

namespace GsEPWv8_4_MVC.Controllers
{

    public class WhsMasterController : Controller
    {
        public ActionResult WhsMaster(string FullFillType, string cmp)
        {

            string l_str_cmp_id = string.Empty;
            string l_str_tmp_cmp_id = string.Empty;                 // CR_3PL_MVC_COMMON_2018_0326_001

            try
            {
                WhsMaster objWhsMaster = new WhsMaster();
                IWhsMasterService ServiceObject = new WhsMasterService();
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                //objWhsMaster.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
                // CR_3PL_MVC_COMMON_2018_0326_001

                objWhsMaster.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                if (objWhsMaster.cmp_id == null || objWhsMaster.cmp_id == string.Empty)
                {
                    objWhsMaster.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                }
                else
                {
                    objCompany.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                }

                // END CR_3PL_MVC_COMMON_2018_0326_001
              
                if (objWhsMaster.cmp_id != "" && FullFillType == null)
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objWhsMaster.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;

                }
                else
                {
                    if (FullFillType == null)
                    {
                        objCompany.user_id = Session["UserID"].ToString().Trim();
                        objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                        objWhsMaster.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    }
                }
                LookUp objLookUp = new LookUp();
                LookUpService ServiceObject1 = new LookUpService();
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
        public ActionResult GetWhsMasterDetails(string p_str_cmp_id, string p_str_whs_id, string p_str_whs_name)
        {
            WhsMaster objWhsMaster = new WhsMaster();
            IWhsMasterService ServiceObject = new WhsMasterService();
            objWhsMaster.cmp_id = p_str_cmp_id;
            objWhsMaster.whs_id = p_str_whs_id;
            objWhsMaster.whs_name = p_str_whs_name;
            TempData["cmp_id"] = objWhsMaster.cmp_id;
            TempData["whs_id"] = objWhsMaster.whs_id;
            TempData["whs_name"] = objWhsMaster.whs_name;
            objWhsMaster = ServiceObject.GetWhsMasterDetails(objWhsMaster);
            var model = objWhsMaster.ListWhsMaster;
            GridView gv = new GridView();
            gv.DataSource = model;
            gv.DataBind();
            Session["Cars"] = gv;
            Mapper.CreateMap<WhsMaster, WhsMasterModel>();
            WhsMasterModel objWhsMasterModel = Mapper.Map<WhsMaster, WhsMasterModel>(objWhsMaster);
            return PartialView("_WhsMaster", objWhsMasterModel);
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Views(string cmpid, string whsid)
        {
            WhsMaster objWhsMaster = new WhsMaster();
            //WhsMasterService serviceobject = new WhsMasterService();
            //objWhsMaster.cmp_id = cmpid;
            //objWhsMaster.whs_id = whsid;
            //objWhsMaster = serviceobject.GetWhsMasterViewDetails(objWhsMaster);
            //objWhsMaster.catg = objWhsMaster.ListWhsMasterViewDtl[0].catg;
            //objWhsMaster.type = objWhsMaster.ListWhsMasterViewDtl[0].type;
            //objWhsMaster.status = objWhsMaster.ListWhsMasterViewDtl[0].status;
            //objWhsMaster.itm_name = objWhsMaster.ListWhsMasterViewDtl[0].itm_name;
            //objWhsMaster.list_price = objWhsMaster.ListWhsMasterViewDtl[0].list_price;
            //objWhsMaster.price_uom = objWhsMaster.ListWhsMasterViewDtl[0].price_uom;
            //objWhsMaster.last_so_dt = objWhsMaster.ListWhsMasterViewDtl[0].last_so_dt;
            //objWhsMaster = serviceobject.GetWhsMasterViewDetails(objWhsMaster);
            //Mapper.CreateMap<WhsMaster, WhsMasterModel>();
            WhsMasterModel objWhsMastermodel = Mapper.Map<WhsMaster, WhsMasterModel>(objWhsMaster);
            return PartialView("_RateView", objWhsMastermodel);
        }
        public ActionResult Edit(string cmpid, string Itmnum, string staus, string Type, string catg)
        {
            WhsMaster objWhsMaster = new WhsMaster();
            //WhsMasterService serviceobject = new WhsMasterService();
            //LookUp objLookUp = new LookUp();
            //LookUpService ServiceObject1 = new LookUpService();
            //Price objPrice = new Price();
            //PriceuomService ServiceObject2 = new PriceuomService();
            //objWhsMaster.cmp_id = cmpid.Trim();
            //objWhsMaster.itm_num = Itmnum.Trim();

            //objWhsMaster = serviceobject.GetWhsMasterViewDetails(objWhsMaster);
            //objWhsMaster.catg = catg.Trim();
            //objLookUp.id = "8";
            //objLookUp.lookuptype = "VAS";
            //objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            //objWhsMaster.ListLookUpCategoryDtl = objLookUp.ListLookUpDtl;

            //objPrice.uom_type = "Quantity";
            //objPrice = ServiceObject2.GetPriceuomDetails(objPrice);
            //objWhsMaster.ListPriceuom = objPrice.ListPriceuom;


            //objWhsMaster.type = Type.Trim();
            //objLookUp.id = "7";
            //objLookUp.lookuptype = "WhsMaster";
            //objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            //objWhsMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;

            //objWhsMaster.status = staus.Trim();
            //objLookUp.id = "11";
            //objLookUp.lookuptype = "RATESTATUS";
            //objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            //objWhsMaster.ListLookUpStatusDtl = objLookUp.ListLookUpDtl;
            //objWhsMaster.itm_name = objWhsMaster.ListWhsMasterViewDtl[0].itm_name;
            //objWhsMaster.list_price = objWhsMaster.ListWhsMasterViewDtl[0].list_price;
            //objWhsMaster.price_uom = objWhsMaster.ListWhsMasterViewDtl[0].price_uom;
            //objWhsMaster.last_so_dt = objWhsMaster.ListWhsMasterViewDtl[0].last_so_dt;
            //objWhsMaster = serviceobject.GetWhsMasterViewDetails(objWhsMaster);
            //Mapper.CreateMap<WhsMaster, WhsMasterModel>();
            WhsMasterModel objWhsMastermodel = Mapper.Map<WhsMaster, WhsMasterModel>(objWhsMaster);
            return PartialView("_RateEdit", objWhsMastermodel);
        }
        public ActionResult InsertWhsMaster(string p_str_cmp_id, string p_str_whs_id, string p_str_whs_name, string p_str_whs_attn, string p_str_whs_mail_name, string p_str_whs_city, string p_str_whs_StartDate,
            string p_str_whs_lastDate, string p_str_whs_status, string p_str_whs_addr_line1, string p_str_whs_addr_line2, string p_str_whs_state_id, string p_str_whs_post_code, string p_str_whs_cntry_id, string p_str_whs_tel
            , string p_str_whs_cell, string p_str_whs_fax, string p_str_whs_email, string p_str_whs_web,bool chkWhsId)
        {
            WhsMaster objWhsMaster = new WhsMaster();
            WhsMasterService serviceobject = new WhsMasterService();
            objWhsMaster.cmp_id = p_str_cmp_id;
            objWhsMaster.whs_id = p_str_whs_id;
            objWhsMaster = serviceobject.Validate_Cmp_whs(objWhsMaster);
            if (objWhsMaster.LstCheckWhsId.Count() > 0)
            {
                return Json(objWhsMaster.LstCheckWhsId.Count, JsonRequestBehavior.AllowGet);
            }      
            objWhsMaster.whs_name = (p_str_whs_name.Trim() == null) ? "" : p_str_whs_name.Trim();
            objWhsMaster.status = (p_str_whs_status.Trim() == null) ? "" : p_str_whs_status.Trim();
            objWhsMaster.StartDate = (p_str_whs_StartDate == null || p_str_whs_StartDate == "") ? "" : p_str_whs_StartDate;
            objWhsMaster.lastDate = (p_str_whs_lastDate == null || p_str_whs_lastDate == "") ? "" : p_str_whs_lastDate; ;
            objWhsMaster.attn = (p_str_whs_attn.Trim() == null || p_str_whs_attn == "") ? "" : p_str_whs_attn.Trim();
            objWhsMaster.mail_name = (p_str_whs_mail_name.Trim() == null || p_str_whs_mail_name == "") ? "" : p_str_whs_mail_name.Trim(); 
            objWhsMaster.addr_line1 = (p_str_whs_addr_line1.Trim() == null || p_str_whs_addr_line1 == "") ? "" : p_str_whs_addr_line1.Trim();
            objWhsMaster.addr_line2 = (p_str_whs_addr_line2.Trim() == null || p_str_whs_addr_line2 == "") ? "" : p_str_whs_addr_line2.Trim();
            objWhsMaster.city = (p_str_whs_city.Trim() == null || p_str_whs_city == "") ? "" : p_str_whs_city.Trim(); 
            objWhsMaster.state_id = (p_str_whs_state_id.Trim() == null || p_str_whs_state_id == "") ? "" : p_str_whs_state_id.Trim(); 
            objWhsMaster.post_code = (p_str_whs_post_code.Trim() == null || p_str_whs_post_code == "") ? "" : p_str_whs_post_code.Trim(); 
            objWhsMaster.cntry_id = (p_str_whs_cntry_id.Trim() == null || p_str_whs_cntry_id == "") ? "" : p_str_whs_cntry_id.Trim(); 
            objWhsMaster.tel = (p_str_whs_tel.Trim() == null || p_str_whs_tel == "") ? "" : p_str_whs_tel.Trim(); 
            objWhsMaster.cell = (p_str_whs_cell.Trim() == null || p_str_whs_cell == "") ? "" : p_str_whs_cell.Trim(); 
            objWhsMaster.fax = (p_str_whs_fax.Trim() == null || p_str_whs_fax == "") ? "" : p_str_whs_fax.Trim(); 
            objWhsMaster.email = (p_str_whs_email.Trim() == null || p_str_whs_email == "") ? "" : p_str_whs_email.Trim(); 
            objWhsMaster.web = (p_str_whs_web.Trim() == null || p_str_whs_web == "") ? "" : p_str_whs_web.Trim();           
            objWhsMaster.notes = "";
            objWhsMaster.dft_whs = chkWhsId;
            objWhsMaster.process_id = "Add";
            serviceobject.WhsMasterInsert(objWhsMaster);
            Mapper.CreateMap<WhsMaster, WhsMasterModel>();
            WhsMasterModel objWhsMastermodel = Mapper.Map<WhsMaster, WhsMasterModel>(objWhsMaster);
            return View("~/Views/WhsMaster/WhsMaster.cshtml", objWhsMastermodel);
        }
        public ActionResult NewWhsMaster(string p_str_cmp_id, string p_str_itm_num, string p_str_type, string p_str_catg, string p_str_status, string p_str_itm_name, decimal p_str_list_price, string p_str_price_uom, string p_str_last_so_dt)
        {
            WhsMaster objWhsMaster = new WhsMaster();
            //WhsMasterService serviceobject = new WhsMasterService();
            //objWhsMaster.cmp_id = p_str_cmp_id.Trim();
            //objWhsMaster.itm_num = p_str_itm_num.Trim();
            //objWhsMaster.type = p_str_type.Trim();
            //objWhsMaster.catg = p_str_catg.Trim();
            //objWhsMaster.status = p_str_status.Trim();
            //objWhsMaster.itm_name = p_str_itm_name.Trim();
            //objWhsMaster.list_price = p_str_list_price;
            //objWhsMaster.price_uom = p_str_price_uom.Trim();
            //objWhsMaster.last_so_dt = p_str_last_so_dt.Trim();
            //serviceobject.WhsMasterCreateUpdate(objWhsMaster);
            //Mapper.CreateMap<WhsMaster, WhsMasterModel>();
            WhsMasterModel objWhsMastermodel = Mapper.Map<WhsMaster, WhsMasterModel>(objWhsMaster);
            return View("~/Views/WhsMaster/WhsMaster.cshtml", objWhsMastermodel);
        }
        public ActionResult Add(string cmpid, string Itmnum)
        {
            WhsMaster objWhsMaster = new WhsMaster();
            WhsMasterService serviceobject = new WhsMasterService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objWhsMaster.cmp_id = cmpid;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objWhsMaster.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = cmpid;
            objPick = ServiceObjectPick.GetCountryPick(objPick);
            objWhsMaster.ListCntryPick = objPick.ListCntryPick;
            objPick = ServiceObjectPick.GetStatePick(objPick);
            objWhsMaster.ListStatePick = objPick.ListStatePick;
            LookUp objLookUp = new LookUp();
            LookUpService ServiceObject1 = new LookUpService();
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
        [HttpPost]
        public ActionResult UpdateWhsMaster(string p_str_cmp_id, string p_str_catg, string p_str_itm_number, string p_str_list_price, string p_str_last_so_dt, string p_str_status, string p_str_price_uom)
        {
            WhsMaster objWhsMaster = new WhsMaster();
            ////string temp = string.Empty;
            ////if (p_str_itm_number != null)
            ////{
            ////    objWhsMaster.cmp_id = Convert.ToString(p_str_itm_number);
            ////    temp = "Edit";
            ////}
            //IWhsMasterService ServiceObject = new WhsMasterService();

            //objWhsMaster.cmp_id = p_str_cmp_id;
            //objWhsMaster.catg = p_str_catg;
            //objWhsMaster.itm_num = p_str_itm_number;
            ////objWhsMaster.list_price = p_str_list_price;
            //objWhsMaster.last_so_dt = p_str_last_so_dt;
            //objWhsMaster.status = p_str_status;
            //objWhsMaster.price_uom = p_str_price_uom;
            //ServiceObject.WhsMasterUpdate(objWhsMaster);
            //Mapper.CreateMap<WhsMaster, WhsMasterModel>();
            WhsMasterModel objWhsMasterModel = Mapper.Map<WhsMaster, WhsMasterModel>(objWhsMaster);
            ////if (temp != "Edit")
            ////    return View(objWhsMasterModel);
            ////else
            return View("~/Views/WhsMaster/WhsMaster.cshtml", objWhsMasterModel);



        }
        public ActionResult DeleteWhsMaster(string Itmnum, string cmpid)
        {

            WhsMaster objWhsMaster = new WhsMaster();
            IWhsMasterService ServiceObject = new WhsMasterService();
            string ResultCount;
            objWhsMaster.whs_id = Itmnum.Trim();
            objWhsMaster.cmp_id = cmpid.Trim();
            ServiceObject.WhsMasterDelete(objWhsMaster);
            ResultCount = "AD";

            return Json(ResultCount, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Download()
        {
            if (Session["Cars"] != null)
            {
                return new DownloadFileActionResult((GridView)Session["Cars"], "Cars.xls");
            }
            else
            {
                return new JavaScriptResult();
            }
        }

    }
}





