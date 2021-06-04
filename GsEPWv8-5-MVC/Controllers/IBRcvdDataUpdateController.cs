using AutoMapper;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Common;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace GsEPWv8_5_MVC.Controllers
{
    public class IBRcvdDataUpdateController : Controller
    {
        // GET: IBRcvdDataUpdate
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IBRcvdDataUpdate(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_cntr_id)
        {
            string l_str_cmp_id = string.Empty;
            string l_str_tmp_cmp_id = string.Empty;


            IBRcvdDataUpdateService ServiceIBRcvdDataUpdate = new IBRcvdDataUpdateService();

            IBRcvdDataUpdate objIBRcvdDataUpdateHdr = new IBRcvdDataUpdate();
            IB943UploadFileInfo objIB943UploadFileInfo = new IB943UploadFileInfo();

            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();

            if (p_str_cmp_id == null || p_str_cmp_id == string.Empty)
            {
                l_str_tmp_cmp_id = Session["g_str_cmp_id"].ToString();
                l_str_cmp_id = Session["dflt_cmp_id"].ToString();

                if (l_str_tmp_cmp_id != "")
                {
                    objIBRcvdDataUpdateHdr.cmp_id = l_str_tmp_cmp_id;
                    l_str_cmp_id = l_str_tmp_cmp_id;
                }
                else if (l_str_cmp_id != null)
                {
                    objIBRcvdDataUpdateHdr.cmp_id = l_str_cmp_id.Trim();

                }
                else
                {
                    objIBRcvdDataUpdateHdr.cmp_id = "";
                    l_str_cmp_id = "";

                }

            }
            else
            {
                objIBRcvdDataUpdateHdr.cmp_id = p_str_cmp_id;
                l_str_cmp_id = p_str_cmp_id;
            }

            objIBRcvdDataUpdateHdr.cmp_id = l_str_cmp_id;
            objIBRcvdDataUpdateHdr.ib_doc_id = p_str_ib_doc_id;
            objIBRcvdDataUpdateHdr.cntr_id = p_str_cntr_id;

            objCompany.user_id = Session["UserID"].ToString().Trim();

            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objIBRcvdDataUpdateHdr.ListCompany = objCompany.ListCompanyPickDtl;

            Mapper.CreateMap<IBRcvdDataUpdate, IBRcvdDataUpdateModel>();
            IBRcvdDataUpdateModel objIBRcvdDataUpdateModel = Mapper.Map<IBRcvdDataUpdate, IBRcvdDataUpdateModel>(objIBRcvdDataUpdateHdr);
            return View(objIBRcvdDataUpdateModel);
        }

     
        public JsonResult SaveIBRcvdData( IBRcvdDataUpdateHdr objRcvdDataUpdateHdr, List<IBRcvdDataUpdateDtl> ItemDetails, string p_str_save_hdr, string p_str_cntr_type, bool p_bln_excld_bill)
        {
            string l_str_cmp_id = string.Empty;
            string l_str_ib_doc_id = string.Empty;
            string l_str_rcvd_dt = string.Empty;
            int l_int_doc_dt_ck = 0;
            int ResultCount = 0;
            DataTable dtRcvdDtCanUpdate;
            l_str_cmp_id = objRcvdDataUpdateHdr.cmp_id;
            l_str_ib_doc_id = objRcvdDataUpdateHdr.ib_doc_id;
            l_str_rcvd_dt = objRcvdDataUpdateHdr.rcvd_dt;
      
            IBRcvdDataUpdateService ServiceIBRcvdDataUpdate = new IBRcvdDataUpdateService();
            DataTable p_dt_tbl_ib_rcvd_updt_hdr = new DataTable();
            DataTable p_dt_tbl_ib_rcvd_updt_dtl = new DataTable();
            dtRcvdDtCanUpdate = new DataTable();
            dtRcvdDtCanUpdate = ServiceIBRcvdDataUpdate.GetIBCheckRcvdDtCanUpdate(l_str_cmp_id, l_str_ib_doc_id, l_str_rcvd_dt);
            if (dtRcvdDtCanUpdate.Rows.Count>0)
            {
               // l_int_inv_rec_cnt = Convert.ToInt16( dtRcvdDtCanUpdate.Rows[0]["l_int_rec_cnt"]);
                l_int_doc_dt_ck = Convert.ToInt16(dtRcvdDtCanUpdate.Rows[0]["l_int_doc_dt_ck"]);

                //if (l_int_inv_rec_cnt > 0 && l_int_doc_dt_ck >0)
                //{
                //    ResultCount = 1;
                //    return Json(ResultCount, JsonRequestBehavior.AllowGet);
                //}
                //else if (l_int_inv_rec_cnt > 0)
                //{
                //    ResultCount = 2;
                //    return Json(ResultCount, JsonRequestBehavior.AllowGet);
                //}
                //else
                 if (l_int_doc_dt_ck > 0)
                {
                    ResultCount = 3;
                    return Json(ResultCount, JsonRequestBehavior.AllowGet);
                }
            }

            p_dt_tbl_ib_rcvd_updt_hdr = Utility.ObjectToDataTable(objRcvdDataUpdateHdr);
            p_dt_tbl_ib_rcvd_updt_dtl = Utility.ConvertListToDataTable(ItemDetails);
            ServiceIBRcvdDataUpdate.SaveIBRcvdData(l_str_cmp_id, p_dt_tbl_ib_rcvd_updt_hdr, p_dt_tbl_ib_rcvd_updt_dtl, p_str_save_hdr, p_str_cntr_type, p_bln_excld_bill);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetIBRcvdDataUpdate(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_cntr_id)
        {
            string l_str_cmp_id = string.Empty;
            string l_str_tmp_cmp_id = string.Empty;
            int ResultCount = 0;
            IBRcvdDataUpdateService ServiceIBRcvdDataUpdate = new IBRcvdDataUpdateService();
            IBRcvdDataUpdate objIBRcvdDataUpdateHdr = new IBRcvdDataUpdate();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();

            if (p_str_cmp_id == null || p_str_cmp_id == string.Empty)
            {
                l_str_tmp_cmp_id = Session["g_str_cmp_id"].ToString();
                l_str_cmp_id = Session["dflt_cmp_id"].ToString();

                if (l_str_tmp_cmp_id != "")
                {
                    objIBRcvdDataUpdateHdr.cmp_id = l_str_tmp_cmp_id;
                    l_str_cmp_id = l_str_tmp_cmp_id;
                }
                else if (l_str_cmp_id != null)
                {
                    objIBRcvdDataUpdateHdr.cmp_id = l_str_cmp_id.Trim();

                }
                else
                {
                    objIBRcvdDataUpdateHdr.cmp_id = "";
                    l_str_cmp_id = "";

                }

            }
            else
            {
                objIBRcvdDataUpdateHdr.cmp_id = p_str_cmp_id;
                l_str_cmp_id = p_str_cmp_id;
            }

            objIBRcvdDataUpdateHdr.cmp_id = l_str_cmp_id;
            objIBRcvdDataUpdateHdr.ib_doc_id = p_str_ib_doc_id;
            objIBRcvdDataUpdateHdr.cntr_id = p_str_cntr_id;
            
            objIBRcvdDataUpdateHdr = ServiceIBRcvdDataUpdate.GetRcvdHdr(objIBRcvdDataUpdateHdr);
            if (objIBRcvdDataUpdateHdr.GetRcvdHdr.Count > 0)
            { 
                objIBRcvdDataUpdateHdr = ServiceIBRcvdDataUpdate.ListDocItemList(objIBRcvdDataUpdateHdr);
            
            objCompany.user_id = Session["UserID"].ToString().Trim();

            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objIBRcvdDataUpdateHdr.ListCompany = objCompany.ListCompanyPickDtl;

                LookUp objLookUp = new LookUp();
                LookUpService ServiceObject1 = new LookUpService();
                objLookUp.id = "103";
                objLookUp.lookuptype = "CNTR_SIZE";
                objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                objIBRcvdDataUpdateHdr.ListContainerType = objLookUp.ListLookUpDtl;
                if (objIBRcvdDataUpdateHdr.GetRcvdHdr[0].cntr_type != null)
                {
                    objIBRcvdDataUpdateHdr.cntr_type = objIBRcvdDataUpdateHdr.GetRcvdHdr[0].cntr_type.ToString();
                }
                objIBRcvdDataUpdateHdr.excld_bill = objIBRcvdDataUpdateHdr.GetRcvdHdr[0].excld_bill;


                Mapper.CreateMap<IBRcvdDataUpdate, IBRcvdDataUpdateModel>();
            IBRcvdDataUpdateModel objIBRcvdDataUpdateModel = Mapper.Map<IBRcvdDataUpdate, IBRcvdDataUpdateModel>(objIBRcvdDataUpdateHdr);
            return PartialView("_GetIBRcvdDataUpdate", objIBRcvdDataUpdateModel);
            }
            else
            {
                ResultCount = 2;
                return Json(ResultCount, JsonRequestBehavior.AllowGet);
            }
        }
    }
}