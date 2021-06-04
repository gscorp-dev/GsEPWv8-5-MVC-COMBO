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
    public class LocationMasterController : Controller
    {
        // GET: LocationMaster
        LocationMaster objLocationMaster = new LocationMaster();
        ILocationMasterService ServiceObject = new LocationMasterService();
        Company objCompany = new Company();
        ICompanyService ServiceObjectCompany = new CompanyService();
        Pick objPick = new Pick();
        PickService ServiceObjectPick = new PickService();
        public bool l_str_include_entry_dtls;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LocationMaster(string FullFillType, string cmp)
        {
            string l_str_cmp_id = string.Empty;
            string l_str_tmp_cmp_id = string.Empty;
            string l_str_scn_id = string.Empty;
            string l_str_success = string.Empty;

            try
            {
                Session["g_str_Search_flag"] = "False";
                objLocationMaster.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                if (objLocationMaster.cmp_id == null || objLocationMaster.cmp_id == string.Empty)
                {
                    objLocationMaster.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
                   

                }
                else
                {
                    objCompany.cmp_id = Session["g_str_cmp_id"].ToString().Trim();

                }
                
                if (FullFillType == null)
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objLocationMaster.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    objPick.cmp_id = objLocationMaster.cmp_id;
                    objPick.Whs_id = "";
                    objPick.Whs_name = "";
                    objPick = ServiceObjectPick.GetWhsPick(objPick);
                    objLocationMaster.ListWareHousePickdtl = objPick.ListPick;
                    objLocationMaster.cmp_id = objLocationMaster.cmp_id;
                    objLocationMaster = ServiceObject.GetLocationMasterDetails(objLocationMaster);

                }
                Mapper.CreateMap<LocationMaster, LocationMasterModel>();
                LocationMasterModel objLocationMasterModel = Mapper.Map<LocationMaster, LocationMasterModel>(objLocationMaster);
                return View(objLocationMasterModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ActionResult CompanyOnChange(string p_str_cmp_id)
        {
            objPick.cmp_id = p_str_cmp_id;
            objPick.Whs_id = "";
            objPick.Whs_name = "";
            objPick = ServiceObjectPick.GetWhsPick(objPick);
            objLocationMaster.ListWareHousePickdtl = objPick.ListPick;
            objLocationMaster.cmp_id = p_str_cmp_id;
            objLocationMaster = ServiceObject.GetLocationMasterDetails(objLocationMaster);
            var serializer = new JavaScriptSerializer() { MaxJsonLength = 86753090 };

            // Perform your serialization
            serializer.Serialize(objLocationMaster);
            return new JsonResult()
            {
                Data = objLocationMaster,
                MaxJsonLength = 86753090
            };

        }
        
        public ActionResult GetLocationMasterDetail(string p_str_cmp_id, string p_str_whs_id,string p_str_loc_id,string p_str_loc_desc)
        {
            
            try
            {
                objLocationMaster.cmp_id =(p_str_cmp_id == null ? "" : p_str_cmp_id.Trim()) ;
                objLocationMaster.whs_id = (p_str_whs_id == null ? "" : p_str_whs_id.Trim());
                objLocationMaster.loc_id = (p_str_loc_id == null ? "" : p_str_loc_id.Trim());
                objLocationMaster.loc_desc = (p_str_loc_desc == null ? "" : p_str_loc_desc.Trim());
                objLocationMaster = ServiceObject.GetLocationMasterDetails(objLocationMaster);
                Mapper.CreateMap<LocationMaster, LocationMasterModel>();
                LocationMasterModel objLocationMasterModel = Mapper.Map<LocationMaster, LocationMasterModel>(objLocationMaster);
                return PartialView("_LocationMaster", objLocationMasterModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ActionResult LocationMasterDetailView(string p_str_cmp_id)
        {

            try
            {
                objCompany.user_id = Session["UserID"].ToString().Trim();
                objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                objLocationMaster.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                objPick.cmp_id = p_str_cmp_id;
                objPick.Whs_id = "";
                objPick.Whs_name = "";
                objPick = ServiceObjectPick.GetWhsPick(objPick);
                objLocationMaster.ListWareHousePickdtl = objPick.ListPick;
                objLocationMaster.state_id = "Insert";
                objLocationMaster.cmp_id = p_str_cmp_id;
                LookUp objLookUp = new LookUp();
                LookUpService ServiceObject1 = new LookUpService();
                objLookUp.id = "302";
                objLookUp.lookuptype = "LOC-TYPE";
                objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                objLocationMaster.ListLocType = objLookUp.ListLookUpDtl;


                Mapper.CreateMap<LocationMaster, LocationMasterModel>();
                LocationMasterModel objLocationMasterModel = Mapper.Map<LocationMaster, LocationMasterModel>(objLocationMaster);
                return PartialView("_LocationMasterDetail", objLocationMasterModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

      
        public ActionResult LocationMasterDetailEditView(string p_str_cmp_id, string p_str_whs_id, string p_str_loc_id, string p_str_loc_desc, string p_str_Status, string p_str_note,
            decimal p_str_length, decimal p_str_width,
            decimal p_str_depth, decimal p_str_cube, string option, string p_str_loc_type)
    {

            try
            {
                objLocationMaster.cmp_id = p_str_cmp_id;
                objLocationMaster.whs_id = p_str_whs_id;
                objLocationMaster.loc_id = p_str_loc_id;
                objLocationMaster.loc_desc = p_str_loc_desc;
                objLocationMaster.status = p_str_Status;
                objLocationMaster.note = p_str_note;
                objLocationMaster.length =p_str_length;
                objLocationMaster.width = p_str_width;
                objLocationMaster.depth = p_str_depth;
                objLocationMaster.cube = p_str_cube;
                objLocationMaster.usage ="Edit";
                objLocationMaster.option = option;
                objLocationMaster.loc_type = p_str_loc_type;
                objCompany.user_id = Session["UserID"].ToString().Trim();
                if (option == "2")
                {
                    objLocationMaster.state_id = "Edit";
                    
                }
                else if (option == "3")
                {
                    objLocationMaster.state_id = "Delete";
                }
                else
                {
                    objLocationMaster.state_id = "View";
                }
                objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                objLocationMaster.ListCompany = objCompany.ListCompanyPickDtl;
                objPick.cmp_id = objLocationMaster.cmp_id;
                objPick.Whs_id = "";
                objPick.Whs_name = "";
                objPick = ServiceObjectPick.GetWhsPick(objPick);
                objLocationMaster.Whs_name = objPick.ListPick[0].Whs_name;
                objLocationMaster.cmp_id = p_str_cmp_id;
                LookUp objLookUp = new LookUp();
                LookUpService ServiceObject1 = new LookUpService();
                objLookUp.id = "302";
                objLookUp.lookuptype = "LOC-TYPE";
                objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                objLocationMaster.ListLocType = objLookUp.ListLookUpDtl;

                objLocationMaster.loc_type = p_str_loc_type;
                Mapper.CreateMap<LocationMaster, LocationMasterModel>();
                LocationMasterModel objLocationMasterModel = Mapper.Map<LocationMaster, LocationMasterModel>(objLocationMaster);
                return PartialView("_LocationMasterDetail", objLocationMasterModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ActionResult AddLocationMasterDetails(string p_str_cmp_id, string p_str_whs_id, string p_str_loc_id, string p_str_loc_desc, string p_str_Status, string p_str_note,
            decimal p_str_length, decimal p_str_width,
            decimal p_str_depth, decimal p_str_cube, string option, string p_str_loc_type)
        {

            objLocationMaster.cmp_id = p_str_cmp_id;
            objLocationMaster.whs_id = p_str_whs_id;
            objLocationMaster.loc_id = p_str_loc_id;
            objLocationMaster.option = "3";
            objLocationMaster.loc_type = p_str_loc_type;
            objLocationMaster = ServiceObject.InsertLocationMasterDetails(objLocationMaster);
            objLocationMaster.totalrecords = objLocationMaster.ListInsertMasterDetails.Count();
            objLocationMaster.loc_desc = p_str_loc_desc;
            objLocationMaster.status = p_str_Status;
            objLocationMaster.note = p_str_note;
            objLocationMaster.length = p_str_length;
            objLocationMaster.width = p_str_width;
            objLocationMaster.depth = p_str_depth;
            objLocationMaster.cube = p_str_cube;
            objLocationMaster.usage = "";
            objLocationMaster.process_id ="Add";
            objLocationMaster.option = option;

            objLocationMaster = ServiceObject.InsertLocationMasterDetails(objLocationMaster);
            objLocationMaster.option = "3";
            objLocationMaster.totalcount = objLocationMaster.ListInsertMasterDetails.Count();
            l_str_include_entry_dtls = true;
            LookUp objLookUp = new LookUp();
            LookUpService ServiceObject1 = new LookUpService();
            objLookUp.id = "302";
            objLookUp.lookuptype = "LOC-TYPE";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objLocationMaster.ListLocType = objLookUp.ListLookUpDtl;
            objLocationMaster.loc_type = p_str_loc_type;
            Mapper.CreateMap<LocationMaster, LocationMasterModel>();
            LocationMasterModel objLocationMasterModel = Mapper.Map<LocationMaster, LocationMasterModel>(objLocationMaster);
            return Json(l_str_include_entry_dtls, JsonRequestBehavior.AllowGet);


        }
        public ActionResult EditLocationMasterDetails(string p_str_cmp_id, string p_str_whs_id, string p_str_loc_id, string p_str_loc_desc, string p_str_Status,
            string p_str_note, decimal p_str_length, decimal p_str_width,
           decimal p_str_depth, decimal p_str_cube, string option, string p_str_loc_type)
        {

            objLocationMaster.cmp_id = p_str_cmp_id;
            objLocationMaster.whs_id = p_str_whs_id;
            objLocationMaster.loc_id = p_str_loc_id;
            objLocationMaster.loc_desc = p_str_loc_desc;
            objLocationMaster.status = p_str_Status;
            objLocationMaster.note = p_str_note;
            objLocationMaster.length = p_str_length;
            objLocationMaster.width = p_str_width;
            objLocationMaster.depth = p_str_depth;
            objLocationMaster.cube =p_str_cube;
            objLocationMaster.usage = "";
            objLocationMaster.process_id ="Edit";
            objLocationMaster.option = option;
            objLocationMaster.loc_type = p_str_loc_type;
            objLocationMaster = ServiceObject.InsertLocationMasterDetails(objLocationMaster);
           
                l_str_include_entry_dtls = true;
            LookUp objLookUp = new LookUp();
            LookUpService ServiceObject1 = new LookUpService();
            objLookUp.id = "302";
            objLookUp.lookuptype = "LOC-TYPE";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objLocationMaster.ListLocType = objLookUp.ListLookUpDtl;
            objLocationMaster.loc_type = p_str_loc_type;

            Mapper.CreateMap<LocationMaster, LocationMasterModel>();
            LocationMasterModel objLocationMasterModel = Mapper.Map<LocationMaster, LocationMasterModel>(objLocationMaster);
            return Json(l_str_include_entry_dtls, JsonRequestBehavior.AllowGet);


        }
        public ActionResult DeleteLocationMasterDetails(string p_str_cmp_id, string p_str_whs_id, string p_str_loc_id, string option)
        {

            objLocationMaster.cmp_id = p_str_cmp_id;
            objLocationMaster.whs_id = p_str_whs_id;
            objLocationMaster.loc_id = p_str_loc_id;
            objLocationMaster.option = "3";
            objLocationMaster = ServiceObject.DeleteLocationMasterDetails(objLocationMaster);
            l_str_include_entry_dtls = true;
            Mapper.CreateMap<LocationMaster, LocationMasterModel>();
            LocationMasterModel objLocationMasterModel = Mapper.Map<LocationMaster, LocationMasterModel>(objLocationMaster);
            return Json(l_str_include_entry_dtls, JsonRequestBehavior.AllowGet);


        }

        public JsonResult PickWhsDtl(string term, string cmp_id)
        {
            objLocationMaster.cmp_id = cmp_id.Trim();
            objLocationMaster.Whs_id = term.Trim();
            objLocationMaster.Whs_name = "";
            var List = ServiceObject.GetWhsPickDetails(term, cmp_id).ListWhsDetails.Select(x => new { label = x.Whs_dtl, value = x.Whs_id, Whs_name = x.Whs_name }).ToList();
           
            return Json(List, JsonRequestBehavior.AllowGet);
        }

    }
}
