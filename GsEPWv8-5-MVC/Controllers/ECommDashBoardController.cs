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
    public class ECommDashBoardController : Controller
    {

        [HttpGet]
        public ActionResult ECommDashBoard()
        {

            string l_str_cmp_id = string.Empty;
            string l_str_frm_dt = string.Empty;
            string l_str_to_dt = string.Empty;
            string l_str_tmp_cmp_id = string.Empty;
            string l_str_is_3rd_usr = string.Empty;


            int l_int_total_order = 0;
            string orderstatus;
            try
            {
                Core.Entity.OrderSummary objOrderSummary = new OrderSummary();
                IOrderSummaryService ServiceObject = new OrderSummaryService();
                objOrderSummary.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
                objOrderSummary = ServiceObject.ListOrderSummary(objOrderSummary);
                for (int j = 0; j < objOrderSummary.LstOrderSummary.Count(); j++)
                {
                    orderstatus = objOrderSummary.LstOrderSummary[j].OrderStatus;
                    l_int_total_order = objOrderSummary.LstOrderSummary[j].TotalOrders;
                    if (orderstatus == "TEMP" || orderstatus == "O")
                    {
                        objOrderSummary.ecom_open_orders += l_int_total_order;
                    }

                    if (orderstatus == "A")
                    {
                       objOrderSummary.ecom_aloc_orders = l_int_total_order;
                    }
                    if (orderstatus == "S")
                    {
                       objOrderSummary.ecom_ship_orders = l_int_total_order;
                    }
                    if (orderstatus == "P")
                    {
                       objOrderSummary.ecom_post_orders = l_int_total_order;
                    }

                }
                Session["IsCompanyUser"] = "Y";
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                if (l_str_cmp_id != "")
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objOrderSummary.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    objOrderSummary.cmp_id = l_str_cmp_id;

                }
                else
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objOrderSummary.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    objOrderSummary.cmp_id = objOrderSummary.ListCompanyPickDtl[0].cmp_id;
                    l_str_cmp_id = objOrderSummary.ListCompanyPickDtl[0].cmp_id;
                   
                 
                }
                objOrderSummary.objEcomLink = ServiceObject.fnGetCustEcomLinkDtl(objOrderSummary.cmp_id);
                Mapper.CreateMap<OrderSummary, OrderSummaryModel>();
                OrderSummaryModel objOrderSummaryModel = Mapper.Map<OrderSummary, OrderSummaryModel>(objOrderSummary);
                return View(objOrderSummaryModel);
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

            string l_str_cmp_id = string.Empty;
            string l_str_frm_dt = string.Empty;
            string l_str_to_dt = string.Empty;
            string l_str_tmp_cmp_id = string.Empty;
            string l_str_is_3rd_usr = string.Empty;


            int l_int_temp_order = 0;
            int l_int_total_order = 0;
            string orderstatus;
            try
            {
                l_str_cmp_id = id;
                Core.Entity.OrderSummary objOrderSummary = new OrderSummary();
                IOrderSummaryService ServiceObject = new OrderSummaryService();
                objOrderSummary.cmp_id = id.Trim();
                objOrderSummary = ServiceObject.ListOrderSummary(objOrderSummary);
                for (int j = 0; j < objOrderSummary.LstOrderSummary.Count(); j++)
                {
                    orderstatus = objOrderSummary.LstOrderSummary[j].OrderStatus;
                    l_int_total_order = objOrderSummary.LstOrderSummary[j].TotalOrders;
                    if (orderstatus == "TEMP" || orderstatus == "O")
                    {
                        objOrderSummary.ecom_open_orders += l_int_total_order;
                    }

                    if (orderstatus == "A")
                    {
                        objOrderSummary.ecom_aloc_orders = l_int_total_order;
                    }
                    if (orderstatus == "S")
                    {
                        objOrderSummary.ecom_ship_orders = l_int_total_order;
                    }
                    if (orderstatus == "P")
                    {
                        objOrderSummary.ecom_post_orders = l_int_total_order;
                    }

                }
                Session["IsCompanyUser"] = "Y";
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                if (l_str_cmp_id != "")
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objOrderSummary.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    objOrderSummary.cmp_id = l_str_cmp_id;

                }
                else
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objOrderSummary.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    objOrderSummary.cmp_id = objOrderSummary.ListCompanyPickDtl[0].cmp_id;
                    l_str_cmp_id = objOrderSummary.ListCompanyPickDtl[0].cmp_id;
                }

                Session["g_str_cmp_id"] = l_str_cmp_id;
                Mapper.CreateMap<OrderSummary, OrderSummaryModel>();
                OrderSummaryModel objOrderSummaryModel = Mapper.Map<OrderSummary, OrderSummaryModel>(objOrderSummary);

                return PartialView("_ECommDashBoard", objOrderSummaryModel);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ActionResult DateOnChangeGetModels(string p_str_frm_dt, string p_str_to_dt, string p_str_cmp_Id)
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

                return PartialView("_ECommDashBoard", objDashboardModel);

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