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
    // CR_3PL_MVC_COMMON_2018_0326_001 Added By MEERA Company Id Related Change 
    public class DashBoardController : Controller
    {
        // GET: DashBoard
        public ActionResult DashBoard(string id)
        {
            try
           {
                string l_str_cmp_id = string.Empty;
                string l_str_frm_dt = string.Empty;
                string l_str_to_dt = string.Empty;
                string l_str_tmp_cmp_id = string.Empty;
                string l_str_is_3rd_usr = string.Empty;
                DashBoard objDashBoard = new DashBoard();
                IDashBoardService ServiceObject = new DashBoardService();
                l_str_is_3rd_usr = Session["IS3RDUSER"].ToString();
                objDashBoard.IS3RDUSER = l_str_is_3rd_usr.Trim();
                if (id == null || id== string.Empty)
                {
                    l_str_tmp_cmp_id = Session["g_str_cmp_id"].ToString();
                    l_str_cmp_id = Session["dflt_cmp_id"].ToString();
                   
                    if (l_str_tmp_cmp_id != "")
                    {
                        objDashBoard.cmp_id = l_str_tmp_cmp_id;
                    }
                    else if (l_str_cmp_id != null)
                {
                        objDashBoard.cmp_id = l_str_cmp_id.Trim();

                    }
                    else
                    {
                        objDashBoard.cmp_id = "";

                    }
                   
                }
                else {
                        objDashBoard.cmp_id = id;

                    }                    
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                if (objDashBoard.cmp_id != "")
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objDashBoard.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                   
                }
                else
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    //objDashBoard.cmp_id = id;
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objDashBoard.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                }
               
                DateTime date = DateTime.Now.AddMonths(-12);
                l_str_frm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");      // CR_3PL_MVC_OB_2018_0227_001 - Modified by Soniya
                objDashBoard.frm_dt = l_str_frm_dt;
                objDashBoard.to_dt = DateTime.Now.ToString("MM/dd/yyyy");
                objDashBoard = ServiceObject.ListDashBoard(objDashBoard);
                objDashBoard = ServiceObject.GetInboundRcvdData(objDashBoard);
                objDashBoard = ServiceObject.GetInboundOpenData(objDashBoard);
                objDashBoard = ServiceObject.GetInboundPostData(objDashBoard);
                objDashBoard = ServiceObject.ListOutBound(objDashBoard);
                objDashBoard = ServiceObject.ListVas(objDashBoard);
               
                objDashBoard = ServiceObject.ListBill(objDashBoard);
                objDashBoard = ServiceObject.ListVasOpen(objDashBoard);
                objDashBoard = ServiceObject.ListBillOpen(objDashBoard);
                objDashBoard = ServiceObject.GetOutBoundShipReq(objDashBoard);
                objDashBoard = ServiceObject.GetOutBoundAloc(objDashBoard);
                objDashBoard = ServiceObject.GetOutBoundShipPost(objDashBoard);
                 Session["g_str_cmp_id"]= objDashBoard.cmp_id;//CR-20180421-001
                Mapper.CreateMap<DashBoard, DashBoardModel>();
                DashBoardModel objDashBoardModel = Mapper.Map<DashBoard, DashBoardModel>(objDashBoard);
                return View(objDashBoardModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ActionResult GetModels(string id)
        {
            try
            {
                string l_str_frm_dt = string.Empty;
                string l_str_to_dt = string.Empty;
                string l_str_is_another_usr = string.Empty;
                DashBoard objDashBoard = new DashBoard();
                IDashBoardService ServiceObject = new DashBoardService();
                objDashBoard.cmp_id = id.Trim();
                objDashBoard.cmp_id = id.Trim();
                l_str_is_another_usr = Session["IS3RDUSER"].ToString();
                objDashBoard.IS3RDUSER = l_str_is_another_usr.Trim();
                DateTime date = DateTime.Now.AddDays(-30);
                l_str_frm_dt = Convert.ToDateTime(date).ToString("MM/dd/yyyy");
                objDashBoard.frm_dt = l_str_frm_dt;
                l_str_to_dt = DateTime.Now.ToString("MM/dd/yyyy");
                // CR_3PL_MVC_COMMON_2018_0326_001
                Session["g_str_cmp_id"] = id;
                // END CR_3PL_MVC_COMMON_2018_0326_001
                objDashBoard.to_dt = l_str_to_dt;
                objDashBoard = ServiceObject.ListDashBoard(objDashBoard);
                objDashBoard = ServiceObject.GetInboundRcvdData(objDashBoard);
                objDashBoard = ServiceObject.GetInboundOpenData(objDashBoard);
                objDashBoard = ServiceObject.GetInboundPostData(objDashBoard);
                objDashBoard = ServiceObject.ListOutBound(objDashBoard);
                objDashBoard = ServiceObject.ListVas(objDashBoard);
                objDashBoard = ServiceObject.ListBill(objDashBoard);
                objDashBoard = ServiceObject.ListVasOpen(objDashBoard);
                objDashBoard = ServiceObject.ListBillOpen(objDashBoard);
                objDashBoard = ServiceObject.GetOutBoundShipReq(objDashBoard);
                objDashBoard = ServiceObject.GetOutBoundAloc(objDashBoard);
                objDashBoard = ServiceObject.GetOutBoundShipPost(objDashBoard);
                Mapper.CreateMap<DashBoard, DashBoardModel>();
                DashBoardModel objDashboardModel = Mapper.Map<DashBoard, DashBoardModel>(objDashBoard);

                return PartialView("_Dashboard", objDashboardModel);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ActionResult DateOnChangeGetModels(string p_str_frm_dt, string p_str_to_dt ,string p_str_cmp_Id)
        {
            try
            {
                string l_str_frm_dt = string.Empty;
                string l_str_to_dt = string.Empty;
                string l_str_is_another_usr = string.Empty;
                DashBoard objDashBoard = new DashBoard();
                IDashBoardService ServiceObject = new DashBoardService();
                objDashBoard.cmp_id = p_str_cmp_Id.Trim();
                DateTime date = DateTime.Now.AddDays(-30);
                l_str_frm_dt = Convert.ToDateTime(date).ToString("MM/dd/yyyy");
                l_str_is_another_usr = Session["IS3RDUSER"].ToString();
                objDashBoard.IS3RDUSER = l_str_is_another_usr.Trim();
                l_str_to_dt = DateTime.Now.ToString("MM/dd/yyyy");
                objDashBoard.frm_dt = p_str_frm_dt;
                objDashBoard.to_dt = p_str_to_dt;

              
                objDashBoard = ServiceObject.ListDashBoard(objDashBoard);
                objDashBoard = ServiceObject.GetInboundRcvdData(objDashBoard);
                objDashBoard = ServiceObject.GetInboundOpenData(objDashBoard);
                objDashBoard = ServiceObject.GetInboundPostData(objDashBoard);
                objDashBoard = ServiceObject.ListOutBound(objDashBoard);
                objDashBoard = ServiceObject.ListVas(objDashBoard);
                objDashBoard = ServiceObject.ListBill(objDashBoard);
                objDashBoard = ServiceObject.ListVasOpen(objDashBoard);
                objDashBoard = ServiceObject.ListBillOpen(objDashBoard);
                objDashBoard = ServiceObject.GetOutBoundShipReq(objDashBoard);
                objDashBoard = ServiceObject.GetOutBoundAloc(objDashBoard);
                objDashBoard = ServiceObject.GetOutBoundShipPost(objDashBoard);
                Mapper.CreateMap<DashBoard, DashBoardModel>();
                DashBoardModel objDashboardModel = Mapper.Map<DashBoard, DashBoardModel>(objDashBoard);

                return PartialView("_Dashboard", objDashboardModel);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public ActionResult GetInboundInquiryFromDashboard(string p_str_cmp_id)
        {
            try
            {
                InboundInquiry objInboundInquiry = new InboundInquiry();
                InboundInquiryService objService = new InboundInquiryService();
                objInboundInquiry.cmp_id = p_str_cmp_id;
                objInboundInquiry.cntr_id = "";
                objInboundInquiry.status = "";
                objInboundInquiry.ib_doc_id = "";
                objInboundInquiry.req_num = "";
                objInboundInquiry.ib_doc_dt_fm = DateTime.Now.ToString("MM/DD/YYYY");
                objInboundInquiry.ib_doc_dt_to = DateTime.Now.ToString("MM/DD/YYYY");
                objInboundInquiry.eta_dt_fm = "";
                objInboundInquiry.eta_dt_to = "";
                objInboundInquiry = objService.GetInboundInquiryDetails(objInboundInquiry);
                Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
                InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
                return PartialView("_InboundInquiry", InboundInquiryModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}