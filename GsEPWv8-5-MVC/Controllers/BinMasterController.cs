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

namespace GsEPWv8_5_MVC.Controllers
{
    public class BinMasterController : Controller
    {
        // GET: BinMaster
        public ActionResult BinMasterInquiry(string p_str_cmp_id)
        {
            try
            {
                BinMaster objBinMaster = new BinMaster();
                IBinMasterService ServiceObject = new BinMasterService();
                objBinMaster.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                

                if (objBinMaster.cmp_id == p_str_cmp_id)
                {
                    objBinMaster.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
                }

                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                objCompany.user_id = Session["UserID"].ToString().Trim();
                objCompany.cust_cmp_id = p_str_cmp_id;
                objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                objBinMaster.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;

                Mapper.CreateMap<BinMaster, BinMasterModel>();
                BinMasterModel objBinMasterModel = Mapper.Map<BinMaster, BinMasterModel>(objBinMaster);

                return View(objBinMasterModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public ActionResult BinMasterInquiryGridDtl(string p_str_cmp_id, string p_str_bin_id, string p_str_bin_desc, string p_str_whs_id,
            string p_str_itm_num, string p_str_itm_color, string p_str_itm_size)
        {
            try
            {
                BinMaster objBinMaster = new BinMaster();
                IBinMasterService ServiceObject = new BinMasterService();
                objBinMaster.cmp_id = p_str_cmp_id;
                objBinMaster.bin_id = p_str_bin_id;
                objBinMaster.bin_desc = p_str_bin_desc;
                objBinMaster.whs_id = p_str_itm_num;
                objBinMaster.itm_num = p_str_itm_color;
                objBinMaster.itm_color = p_str_itm_size;
                objBinMaster.itm_color = p_str_itm_size;
                objBinMaster = ServiceObject.GetBinMasterInquiryDetails(objBinMaster);
                Mapper.CreateMap<BinMaster, BinMasterModel>();
                BinMasterModel objBinMasterModel = Mapper.Map<BinMaster, BinMasterModel>(objBinMaster);

                return PartialView("_BinMasterInquiry", objBinMasterModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public ActionResult BinMasterEntry(string p_str_cmp_id )
        {
            try
            {
               
                BinMaster objBinMaster = new BinMaster();
                IBinMasterService ServiceObject = new BinMasterService();
                clsBinMater BinMaterObject = new clsBinMater();
                BinMaterObject.cmp_id = p_str_cmp_id;
                BinMaterObject.cmp_id = p_str_cmp_id.Trim();

                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                objCompany.user_id = Session["UserID"].ToString().Trim();
                objCompany.cust_cmp_id = p_str_cmp_id;
                objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                objBinMaster.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
           
                BinMaterObject.bin_dt = System.DateTime.Now.ToShortDateString();
                objBinMaster.objBinMater = BinMaterObject;

                LookUp objLookUp = new LookUp();
                    LookUpService ServiceObjects = new LookUpService();
                    objLookUp.id = "104";
                    objLookUp.lookuptype = "BIN_SIZE";
                    objLookUp = ServiceObjects.GetLookUpValue(objLookUp);
                    objBinMaster.ListLookUpBinType = objLookUp.ListLookUpDtl;

                objLookUp = new LookUp();
                objLookUp.id = "105";
                objLookUp.lookuptype = "REC_STATUS";
                objLookUp = ServiceObjects.GetLookUpValue(objLookUp);
                objBinMaster.ListStatus = objLookUp.ListLookUpDtl;
              
                objBinMaster.view_flag = "A";

                Pick objPick = new Pick();
                PickService ServiceObjectPick = new PickService();
                objPick.cmp_id = p_str_cmp_id;
                objPick.Whs_id = "";
                objPick.Whs_name = "";
                objPick = ServiceObjectPick.GetWhsPick(objPick);
                objBinMaster.ListWhs = objPick.ListPick;
                if (objBinMaster.ListWhs.Count > 0) objBinMaster.objBinMater.whs_id = objBinMaster.ListWhs[0].Whs_id.Trim();


                objPick = new Pick();
                ServiceObjectPick = new PickService();
                objPick.cmp_id = String.Empty;
                objPick =  ServiceObjectPick.GetPickUom(objPick);

                objBinMaster.ListQtyUoM = objPick.ListExistShipToAddrsPick;

               
                Mapper.CreateMap<BinMaster, BinMasterModel>();
                    BinMasterModel objBinMasterModel = Mapper.Map<BinMaster, BinMasterModel>(objBinMaster);
                return PartialView("_BinMasterEntry", objBinMasterModel);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public JsonResult ItemXGetitmDtl(string term, string cmp_id)
        {
            OutboundInqService ServiceObject = new OutboundInqService();
            var List = ServiceObject.ItemXGetitmDetails(term, cmp_id).LstItmxCustdtl.Select(x => new { label = x.Itmdtl, value = x.itm_num, itm_num = x.itm_num, itm_color = x.itm_color, itm_size = x.itm_size, itm_name = x.itm_name, itm_code = x.itm_code }).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }



        public ActionResult ViewBinMaster(string p_str_cmp_id, string p_str_bin_id, string p_str_mode)
        {
            BinMaster objBinMaster = new BinMaster();
            IBinMasterService ServiceObject = new BinMasterService();
            clsBinMater BinMaterObject = new clsBinMater();
            BinMaterObject.cmp_id = p_str_cmp_id;
            BinMaterObject.cmp_id = p_str_cmp_id.Trim();

            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany.cust_cmp_id = p_str_cmp_id;
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objBinMaster.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;

            BinMaterObject.bin_dt = System.DateTime.Now.ToShortDateString();
            objBinMaster.objBinMater = BinMaterObject;

            LookUp objLookUp = new LookUp();
            LookUpService ServiceObjects = new LookUpService();
            objLookUp.id = "104";
            objLookUp.lookuptype = "BIN_SIZE";
            objLookUp = ServiceObjects.GetLookUpValue(objLookUp);
            objBinMaster.ListLookUpBinType = objLookUp.ListLookUpDtl;

            objLookUp = new LookUp();
            objLookUp.id = "105";
            objLookUp.lookuptype = "REC_STATUS";
            objLookUp = ServiceObjects.GetLookUpValue(objLookUp);
            objBinMaster.ListStatus = objLookUp.ListLookUpDtl;

            objBinMaster.view_flag = p_str_mode;

            Pick objPick = new Pick();
            PickService ServiceObjectPick = new PickService();
            objPick.cmp_id = p_str_cmp_id;
            objPick.Whs_id = "";
            objPick.Whs_name = "";
            objPick = ServiceObjectPick.GetWhsPick(objPick);
            objBinMaster.ListWhs = objPick.ListPick;

            if (objBinMaster.ListWhs.Count > 0) objBinMaster.objBinMater.whs_id = objBinMaster.ListWhs[0].Whs_id.Trim();
            objBinMaster.objBinMater = ServiceObject.fnGetBinMaster(p_str_cmp_id, p_str_bin_id).ListBinMasterinqury[0];

            Mapper.CreateMap<BinMaster, BinMasterModel>();
            BinMasterModel objBinMasterModel = Mapper.Map<BinMaster, BinMasterModel>(objBinMaster);
            return PartialView("_BinMasterEntry", objBinMasterModel);
        }

        public JsonResult fnGetItemPcsDimDtl(string pstrCmpId, string pstrItmNum, string pstrItmColor, string pstrItmSize)
        {
            IBinMasterService ServiceObject = new BinMasterService();
            clsItemPcsDim ItemPcsDim = new clsItemPcsDim();
            try
            {
                ItemPcsDim = ServiceObject.GetBinspGetItemPcsDimDtl(pstrCmpId, pstrItmNum, pstrItmColor, pstrItmSize);
                return Json(ItemPcsDim, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Y", JsonRequestBehavior.AllowGet);
            }
           

        }


        [HttpPost]
        public ActionResult SaveBinMasterEntry(clsBinMater objBinMaster, string pstrMode)
        {
            try
            {
                IBinMasterService ServiceObject = new BinMasterService();
                LocationMaster objLocationMaster = new LocationMaster();
                ILocationMasterService ObjService = new LocationMasterService();
                objLocationMaster.cmp_id = objBinMaster.cmp_id;
                objLocationMaster.whs_id = objBinMaster.whs_id;
                objLocationMaster.loc_id = objBinMaster.bin_loc;

                if (pstrMode == "A")
                {
                 int  lintBinCount = ServiceObject.fnCheckBinMasterExists(objLocationMaster.cmp_id, objBinMaster.bin_id);
                    if (lintBinCount > 0)
                    {
                       return Json("BinExists", JsonRequestBehavior.AllowGet);
                    }


                    int linStyleCount = ServiceObject.fnCheckBinMasterExists(objLocationMaster.cmp_id, objBinMaster.bin_id);
                    if (linStyleCount > 0)
                    {
                        return Json("StyleExists", JsonRequestBehavior.AllowGet);
                    }

                }

                objLocationMaster.option = "3";
                objLocationMaster = ObjService.CHECKLOCIDEXIST(objLocationMaster);
                if (objLocationMaster.ListLocationMasterDetails.Count == 0)
                {
                    objLocationMaster.whs_id = objBinMaster.whs_id;
                    objLocationMaster.loc_id = objBinMaster.bin_loc;
                    objLocationMaster.option = "1";
                    objLocationMaster.loc_desc = objBinMaster.bin_loc;
                    objLocationMaster.status = "A";
                    objLocationMaster.note = "";
                    objLocationMaster.length = 0;
                    objLocationMaster.width = 0;
                    objLocationMaster.depth = 0;
                    objLocationMaster.cube = 0;
                    objLocationMaster.usage = "";
                    objLocationMaster.process_id = "Add";
                    objLocationMaster.loc_type = "BIN";
                    objLocationMaster = ObjService.InsertLocationMasterDetails(objLocationMaster);
                }

                
                objBinMaster.user_id = Session["UserID"].ToString().Trim();
                objBinMaster.bin_dt = System.DateTime.Now.ToShortDateString();
                objBinMaster.process_id = pstrMode;
                ServiceObject.SaveBinMaster(objBinMaster, pstrMode);                             
                return Json("true", JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
    }
}