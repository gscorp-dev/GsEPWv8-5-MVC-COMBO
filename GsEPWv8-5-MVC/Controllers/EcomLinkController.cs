using AutoMapper;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GsEPWv8_4_MVC.Controllers
{
    public class EcomLinkController : Controller
    {
        // GET: EcomLink
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EcomGetAPI(string pstrCmpId)
        {

            string lstrCarrierId = string.Empty;
            string lstrCarrierName = string.Empty;

            try
            {
                ClsEcomLink objClsMaEcomLink = new ClsEcomLink();
                EcomLinkService srEcomLinkService = new EcomLinkService();
                objClsMaEcomLink.cmp_id = pstrCmpId;
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();

                objCompany.cmp_id = pstrCmpId;
                objCompany.user_id = Session["UserID"].ToString().Trim();
                objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                objClsMaEcomLink.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;

                //   objCarrier.lstCarrierList = srvcObjCarrier.fnGetCarrierList(lstrCarrierId, lstrCarrierName).lstCarrierList;
                Mapper.CreateMap<ClsEcomLink, ClsEcomLinkModel>();
                ClsEcomLinkModel EcomLinkModel = Mapper.Map<ClsEcomLink, ClsEcomLinkModel>(objClsMaEcomLink);
                EcomLinkModel.cmp_id = pstrCmpId;
                return View(EcomLinkModel);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }


        }

        public ActionResult EcomItemUpload(string pstrCmpId)
        {
            ClsEcomLink objClsMaEcomLink = new ClsEcomLink();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();

            objCompany.cmp_id = pstrCmpId;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objClsMaEcomLink.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            Mapper.CreateMap<ClsEcomLink, ClsEcomLinkModel>();
            ClsEcomLinkModel EcomLinkModel = Mapper.Map<ClsEcomLink, ClsEcomLinkModel>(objClsMaEcomLink);
            EcomLinkModel.cmp_id = pstrCmpId;
            return PartialView("_EcomItemUpload", EcomLinkModel);
            
        }

        public ActionResult EcomInventoryUpload(string pstrCmpId)
        {
            ClsEcomLink objClsMaEcomLink = new ClsEcomLink();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();

            objCompany.cmp_id = pstrCmpId;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objClsMaEcomLink.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            Mapper.CreateMap<ClsEcomLink, ClsEcomLinkModel>();
            ClsEcomLinkModel EcomLinkModel = Mapper.Map<ClsEcomLink, ClsEcomLinkModel>(objClsMaEcomLink);
            EcomLinkModel.cmp_id = pstrCmpId;
            return PartialView("_EcomItemUpload", EcomLinkModel);
        }

    }
}