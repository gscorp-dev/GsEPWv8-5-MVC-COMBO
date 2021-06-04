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

namespace GsEPWv8_4_MVC.Controllers
{
    public class CarrierMasterController : Controller
    {
        // GET: CarrierMaster
        public ActionResult CarrierMaster()
        {
      
            string lstrCarrierId = string.Empty;
            string lstrCarrierName = string.Empty;

            try
            {
                Carrier objCarrier = new Carrier();
                ICarrierService srvcObjCarrier = new CarrierService();
                objCarrier.tmp_cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                objCarrier.lstCarrierList = srvcObjCarrier.fnGetCarrierList(lstrCarrierId, lstrCarrierName).lstCarrierList;
                Mapper.CreateMap<Carrier, CarrierModel>();
                CarrierModel CarrierModel = Mapper.Map<Carrier, CarrierModel>(objCarrier);
                return View(CarrierModel);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ActionResult fnGetCarrierDetails(string pstrCarrierId, string pstrCarrierName)
        {

            try
            {
                Carrier objCarrier = new Carrier();
                ICarrierService srvcObjCarrier = new CarrierService();
                objCarrier.tmp_cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                objCarrier.lstCarrierList = srvcObjCarrier.fnGetCarrierList(pstrCarrierId, pstrCarrierName).lstCarrierList;
                Mapper.CreateMap<Carrier, CarrierModel>();
                CarrierModel CarrierModel = Mapper.Map<Carrier, CarrierModel>(objCarrier);

                return PartialView("_CarrierMaster", CarrierModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ActionResult AddCarrierMaster()
        {
            Carrier objCarrier = new Carrier();
            CarrierHdr objCarrierHdr = new CarrierHdr();
            objCarrier.tmp_cmp_id = Session["g_str_cmp_id"].ToString().Trim();
            ICarrierService srvcObjCarrier = new CarrierService();

            if (Session["UserID"].ToString() != null)
            {
                objCarrierHdr.entered_by = Session["UserID"].ToString().Trim();
                objCarrierHdr.updated_by = Session["UserID"].ToString().Trim();
            }
            else
            {
                objCarrierHdr.entered_by = "Admin";
                objCarrierHdr.updated_by = "Admin";
            }
            objCarrierHdr.entered_dt = DateTime.Now.ToString("MM-dd-yyyy");
            objCarrierHdr.updated_dt = DateTime.Now.ToString("MM-dd-yyyy");
            objCarrier.option = "ADD";

            Mapper.CreateMap<Carrier, CarrierModel>();
            CarrierModel objCarrierModel = Mapper.Map<Carrier, CarrierModel>(objCarrier);
            return PartialView("_CarrierEntry", objCarrierModel);
        }

        public ActionResult ViewCarrierMaster(string pstrOption, string pstrCarrierId)
        {
            Carrier objCarrier = new Carrier();

            CarrierHdr objCarrierHdr = new CarrierHdr();
            objCarrier.tmp_cmp_id = Session["g_str_cmp_id"].ToString().Trim();
            ICarrierService srvcObjCarrier = new CarrierService();
            objCarrierHdr = srvcObjCarrier.fnGetCarrierList(pstrCarrierId.Trim(), string.Empty).lstCarrierList[0];
            if (Session["UserID"].ToString() != null)
            {
                objCarrierHdr.updated_by = Session["UserID"].ToString().Trim();
            }
            else
            {
                objCarrierHdr.updated_by = "Admin";
            }
            objCarrierHdr.updated_dt = DateTime.Now.ToString("MM-dd-yyyy");
            objCarrier.option = pstrOption;
            objCarrier.objCarrierHdr = objCarrierHdr;
            Mapper.CreateMap<Carrier, CarrierModel>();
            CarrierModel objCarrierModel = Mapper.Map<Carrier, CarrierModel>(objCarrier);
            return PartialView("_CarrierEntry", objCarrierModel);
        }

        public ActionResult fnSaveCarrierMaster(string pstrOption, string pstrCarrierId, CarrierHdr objCarrierHdr)
        {
            Carrier objCarrier = new Carrier();
            objCarrier.tmp_cmp_id = Session["g_str_cmp_id"].ToString().Trim();
            ICarrierService srvcObjCarrier = new CarrierService();

            if (Session["UserID"].ToString() != null)
            {
                objCarrierHdr.entered_by = Session["UserID"].ToString().Trim();
                objCarrierHdr.updated_by = Session["UserID"].ToString().Trim();
            }
            else
            {
                objCarrierHdr.entered_by = "Admin";
                objCarrierHdr.updated_by = "Admin";
            }
            objCarrierHdr.entered_dt = DateTime.Now.ToString("MM-dd-yyyy");
            objCarrierHdr.updated_dt = DateTime.Now.ToString("MM-dd-yyyy");
            objCarrier.option = "ADD";
            srvcObjCarrier.fnSaveCarrierDetails(pstrOption, pstrCarrierId, objCarrierHdr);
            Mapper.CreateMap<Carrier, CarrierModel>();
            CarrierModel objCarrierModel = Mapper.Map<Carrier, CarrierModel>(objCarrier);
            return Json("true", JsonRequestBehavior.AllowGet);
        }
    }
}