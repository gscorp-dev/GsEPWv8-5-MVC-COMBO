using AutoMapper;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GsEPWv8_5_MVC.Controllers
{
    public class DataUpdateController : Controller
    {
       
        public ActionResult Index()
        {
            return View();
        }
      
        public ActionResult DataUpdate(string cmp)
        {
            DataUpdate ObjDataUpdate = new DataUpdate();
            DataUpdateService ServiceObject = new DataUpdateService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            if (cmp == null || cmp == string.Empty)
            {
                if (Session["g_str_cmp_id"].ToString() != null && Session["g_str_cmp_id"].ToString() != "")
                {
                    ObjDataUpdate.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                }
            }
            else
            {
                ObjDataUpdate.cmp_id = cmp.Trim();
            }
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            ObjDataUpdate.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            Mapper.CreateMap<DataUpdate, DataUpdateModel>();
            DataUpdateModel objDataUpdateModel = Mapper.Map<DataUpdate, DataUpdateModel>(ObjDataUpdate);
            return View(objDataUpdateModel);
        }

        public ActionResult CheckIBDocIDExist(string l_str_cmp_id,string l_str_ib_doc_id)
        {
            DataUpdate ObjDataUpdate = new DataUpdate();
            DataUpdateService ServiceObject = new DataUpdateService();
            ObjDataUpdate.cmp_id = l_str_cmp_id;
            ObjDataUpdate.ib_doc_id = l_str_ib_doc_id;
            ObjDataUpdate = ServiceObject.CheckIbDocIdExist(ObjDataUpdate);
            if (ObjDataUpdate.ListCheckIBDocIDExist.Count > 0)
            {
                ObjDataUpdate.ib_doc_rcvd_dt = ObjDataUpdate.ListCheckIBDocIDExist[0].rcvd_dt.ToString();
            }
            else
            {
                ObjDataUpdate.ib_doc_rcvd_dt = string.Empty;
            }
          
            return Json(ObjDataUpdate.ib_doc_rcvd_dt, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckItemExist(string l_str_cmp_id, string l_str_style, string l_str_color, string l_str_size)
        {
            DataUpdate ObjDataUpdate = new DataUpdate();
            DataUpdateService ServiceObject = new DataUpdateService();
            ObjDataUpdate.cmp_id = l_str_cmp_id;
            ObjDataUpdate.itm_num = l_str_style;
            ObjDataUpdate.itm_color = l_str_color;
            ObjDataUpdate.itm_size = l_str_size;
            ObjDataUpdate = ServiceObject.CheckExistItem(ObjDataUpdate);
            return Json(ObjDataUpdate.ListCheckItmExist.Count, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateIBDocRcvdDate(string l_str_cmp_id, string l_str_ib_doc_id, string l_str_old_rcvd_dt,string l_str_new_rcvd_dt)
        {
            DataUpdate ObjDataUpdate = new DataUpdate();
            DataUpdateService ServiceObject = new DataUpdateService();
            ObjDataUpdate.cmp_id = l_str_cmp_id;
            ObjDataUpdate.ib_doc_id = l_str_ib_doc_id;
            ObjDataUpdate.ib_doc_rcvd_dt = l_str_old_rcvd_dt;
            ObjDataUpdate.ib_doc_new_rcvd_dt = l_str_new_rcvd_dt;
            ObjDataUpdate = ServiceObject.UpdateIBDocRcvdDate(ObjDataUpdate);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemDetails(string term, string cmp_id)
        {
            DataUpdate ObjDataUpdate = new DataUpdate();
            DataUpdateService ServiceObject = new DataUpdateService();
            var List = ServiceObject.GetItemDetails(term, cmp_id).LstItmdtl.Select(x => new {label = x.Itmdtl, value = x.itm_num, itm_num = x.itm_num, itm_color = x.itm_color, itm_size = x.itm_size, itm_name = x.itm_name}).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadItemList(string l_str_cmp_id,string l_str_style, string l_str_color, string l_str_size)
        {
            DataUpdate ObjDataUpdate = new DataUpdate();
            DataUpdateService ServiceObject = new DataUpdateService();
            ObjDataUpdate.cmp_id = l_str_cmp_id;
            ObjDataUpdate.itm_num = l_str_style;
            ObjDataUpdate.itm_color = l_str_color;
            ObjDataUpdate.itm_size = l_str_size;
            ObjDataUpdate = ServiceObject.GetItemList(ObjDataUpdate);
            if (ObjDataUpdate.LstItm.Count > 0)
            {
                ObjDataUpdate.length = ObjDataUpdate.LstItm[0].length;
                ObjDataUpdate.width = ObjDataUpdate.LstItm[0].width;
                ObjDataUpdate.depth = ObjDataUpdate.LstItm[0].depth;
                ObjDataUpdate.cube = ObjDataUpdate.LstItm[0].cube;
                ObjDataUpdate.wgt = ObjDataUpdate.LstItm[0].wgt;
            }
            return Json(ObjDataUpdate.LstItm, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateItemDetails(string l_str_cmp_id, string l_str_style, string l_str_color, string l_str_size,string l_str_length,string l_str_width,string l_str_depth, string l_str_cube, string l_str_wgt)
        {
            DataUpdate ObjDataUpdate = new DataUpdate();
            DataUpdateService ServiceObject = new DataUpdateService();
            ObjDataUpdate.cmp_id = l_str_cmp_id;
            ObjDataUpdate.itm_num = l_str_style;
            ObjDataUpdate.itm_color = l_str_color;
            ObjDataUpdate.itm_size = l_str_size;
            ObjDataUpdate.length = Convert.ToDecimal(l_str_length);
            ObjDataUpdate.width = Convert.ToDecimal(l_str_width);
            ObjDataUpdate.depth = Convert.ToDecimal(l_str_depth);
            ObjDataUpdate.cube = Convert.ToDecimal(l_str_cube);
            ObjDataUpdate.wgt = Convert.ToDecimal(l_str_wgt);
            ObjDataUpdate = ServiceObject.UpdateItemDetails(ObjDataUpdate);
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}