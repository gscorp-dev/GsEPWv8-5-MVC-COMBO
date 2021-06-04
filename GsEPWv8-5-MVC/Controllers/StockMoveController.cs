using AutoMapper;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GsEPWv8_5_MVC.Common;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;


namespace GsEPWv8_5_MVC.Controllers
{
    public class StockMoveController : Controller
    {

        public string EmailSub = string.Empty;
        public string EmailMsg = string.Empty;
        public string Folderpath = string.Empty;
        CustMaster objCustMaster = new CustMaster();
        ICustMasterService objCustMasterService = new CustMasterService();

        public ActionResult StockMove(string FullFillType, string cmp, string screentitle)
        {

            string l_str_cmp_id = string.Empty;
            try
            {
                StockChange objStockChange = new StockChange();
                IStockChangeService ServiceObject = new StockChangeService();
                if (cmp == null)
                {
                    objStockChange.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                }
                else
                {
                    objStockChange.cmp_id = cmp;
                }


                clsGlobal.AdjDocId = string.Empty;
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                if (objStockChange.cmp_id != "" && FullFillType == null)
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objStockChange.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;

                }
                else
                {
                    if (FullFillType == null)
                    {
                        objCompany.user_id = Session["UserID"].ToString().Trim();
                        objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                        objStockChange.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    }
                }
                objCompany.user_id = Session["UserID"].ToString().Trim();
                objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                objStockChange.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                objCompany.cust_cmp_id = cmp;
                objCompany = ServiceObjectCompany.GetLocIdDetails(objCompany);
                objStockChange.ListLocPickDtl = objCompany.ListLocPickDtl;
                objStockChange.cmp_id = cmp;
                LookUp objLookUp = new LookUp();
                LookUpService ServiceObject1 = new LookUpService();
                objLookUp.id = "5";
                objLookUp.lookuptype = "INVENTORYINQ";
                objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                objStockChange.ListLookUpDtl = objLookUp.ListLookUpDtl;
                objStockChange.p_str_company = objStockChange.cmp_id;
                objCompany = ServiceObjectCompany.GetFullFillCompanyDetails(objCompany);

                Mapper.CreateMap<StockChange, StockChangeModel>();
                StockChangeModel objStockChangeModel = Mapper.Map<StockChange, StockChangeModel>(objStockChange);
                return View(objStockChangeModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public ActionResult GetStockForMoveByIbDocId(string p_str_cmp_id, string p_str_ib_doc_id,  string p_str_cont_id,  string p_str_refresh)
        {
            try
            {
                StockChange objStockChange = new StockChange();
                StockChangeService objService = new StockChangeService();
                objStockChange.cmp_id = p_str_cmp_id.Trim();

                if (p_str_refresh == "N")
                {
                    objStockChange.ib_doc_id = p_str_ib_doc_id.Trim();
                    objStockChange.cont_id = p_str_cont_id.Trim();
                }
                else
                {
                    objStockChange.ib_doc_id = Session["str_ib_doc_id"].ToString();
                    objStockChange.cont_id = Session["str_cont_id"].ToString();
                }

                Session["g_str_Search_flag"] = "True";

                objStockChange = objService.fnGetStockForMoveByIbDocId(objStockChange);
                Mapper.CreateMap<StockChange, StockChangeModel>();
                StockChangeModel StockChangeModel = Mapper.Map<StockChange, StockChangeModel>(objStockChange);
                return PartialView("_StockMove", StockChangeModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult SaveItemMove(string p_str_cmp_id, List<cls_temp_iv_stk_move> ListItemStockMove)
        {
            string SelPkgIds = string.Empty;
            string ItemCode = string.Empty;
            string l_str_save_status = string.Empty;
            StockChange objStockChange = new StockChange();
            StockChangeService objService = new StockChangeService();
            DataTable dt_item_stock_move = new DataTable();
            dt_item_stock_move = Utility.ConvertListToDataTable(ListItemStockMove);
            l_str_save_status = objService.SaveStkMove(p_str_cmp_id, Session["UserID"].ToString().Trim(), dt_item_stock_move);


            return Json("1", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRefreshGrid(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_cntr_id)
        {
            try
            {

                StockChange objStockChange = new StockChange();
                StockChangeService objService = new StockChangeService();
                objStockChange.cmp_id = p_str_cmp_id;
                objStockChange.ib_doc_id = p_str_ib_doc_id;
                objStockChange.cont_id = p_str_cntr_id;
                objStockChange = objService.fnGetStockForMoveByIbDocId(objStockChange);
                Mapper.CreateMap<StockChange, StockChangeModel>();
                StockChangeModel StockChangeModel = Mapper.Map<StockChange, StockChangeModel>(objStockChange);
                return PartialView("_StockMove", StockChangeModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult DashboardStkChangeItemdtl(string p_str_cmp_id, string Itemcode)
        {
            try
            {
                StockChange objStockChange = new StockChange();
                StockChangeService objService = new StockChangeService();
                objStockChange.cmp_id = p_str_cmp_id;
                objStockChange.itm_code = Itemcode;
                objStockChange = objService.GetItemMoveGridLoadItem(objStockChange);
                Mapper.CreateMap<StockChange, StockChangeModel>();
                StockChangeModel StockChangeModel = Mapper.Map<StockChange, StockChangeModel>(objStockChange);
                return PartialView("_GridDashboardItemMove", StockChangeModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult ItemMove(string p_str_cmpid, string p_str_itm_code, string p_str_paletid, string p_str_ponum, string p_str_LocId,
            string p_str_style, string p_str_Color, string p_str_Size)
        {
            string p_str_success = string.Empty;
            StockChange objStockChange = new StockChange();
            StockChangeService objService = new StockChangeService();
            objStockChange.cmp_id = p_str_cmpid;
            objStockChange.itm_code = p_str_itm_code;
            objStockChange.palet_id = p_str_paletid;
            objStockChange.po_num = p_str_ponum;
            objStockChange.to_loc = p_str_LocId.Trim();
            objStockChange.itm_num = p_str_style.Trim();
            objStockChange.itm_color = p_str_Color.Trim();
            objStockChange.itm_size = p_str_Size.Trim();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cmp_id = p_str_cmpid;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany.cust_cmp_id = p_str_cmpid;
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objStockChange.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objCompany = ServiceObjectCompany.GetLocIdDetails(objCompany);
            objStockChange.ListLocPickDtl = objCompany.ListLocPickDtl;
            objStockChange = objService.CheckLotStatus(objStockChange);
            if (objStockChange.ListCheckLotStatus.Count > 0)
            {
                objStockChange.status = objStockChange.ListCheckLotStatus[0].status;
            }
            if (objStockChange.status == "TEMP")
            {
                p_str_success = "0";
                return Json(p_str_success, JsonRequestBehavior.AllowGet);
            }
            else
            {
                objStockChange = objService.GetItemMoveGridLoadItem(objStockChange);
                if (objStockChange.ListGetItemMoveDetails.Count > 0)
                {
                    objStockChange.ib_doc_id = objStockChange.ListGetItemMoveDetails[0].ib_doc_id;
                    objStockChange.lot_id = objStockChange.ListGetItemMoveDetails[0].lot_id;
                    objStockChange.date = Convert.ToDateTime(objStockChange.ListGetItemMoveDetails[0].rcvd_dt).ToString("MM/dd/yyyy");
                    objStockChange.whs_id = objStockChange.ListGetItemMoveDetails[0].whs_id;
                    objStockChange.frm_loc = objStockChange.ListGetItemMoveDetails[0].loc_id;
                    objStockChange.ib_doc_id = objStockChange.ListGetItemMoveDetails[0].ib_doc_id;
                    objStockChange.lot_num = p_str_paletid;
                }


            }
            Mapper.CreateMap<StockChange, StockChangeModel>();
            StockChangeModel StockChangeModel = Mapper.Map<StockChange, StockChangeModel>(objStockChange);
            return PartialView("_ItemMove", StockChangeModel);
        }

        public ActionResult DashboardItemMove(string FullFillType, string cmp, string screentitle)
        {
            string l_str_cmp_id = string.Empty;
            try
            {
                StockChange objStockChange = new StockChange();
                IStockChangeService ServiceObject = new StockChangeService();

                if (cmp == null)
                {
                    objStockChange.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                }
                else
                {
                    objStockChange.cmp_id = cmp;
                }

                clsGlobal.AdjDocId = string.Empty;
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                if (objStockChange.cmp_id != "" && FullFillType == null)
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objStockChange.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                }
                else
                {
                    if (FullFillType == null)
                    {
                        objCompany.user_id = Session["UserID"].ToString().Trim();
                        objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                        objStockChange.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    }
                }

                objCompany.user_id = Session["UserID"].ToString().Trim();
                objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);

                objStockChange.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                objCompany.cust_cmp_id = cmp;
                objCompany = ServiceObjectCompany.GetLocIdDetails(objCompany);

                objStockChange.ListLocPickDtl = objCompany.ListLocPickDtl;
                objStockChange.cmp_id = cmp;
                LookUp objLookUp = new LookUp();
                LookUpService ServiceObject1 = new LookUpService();
                objLookUp.id = "5";
                objLookUp.lookuptype = "INVENTORYINQ";
                objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                objStockChange.ListLookUpDtl = objLookUp.ListLookUpDtl;
                objCompany = ServiceObjectCompany.GetFullFillCompanyDetails(objCompany);

                Mapper.CreateMap<StockChange, StockChangeModel>();
                StockChangeModel objStockChangeModel = Mapper.Map<StockChange, StockChangeModel>(objStockChange);
                return View(objStockChangeModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public ActionResult DashboardItemMoveGrid(string p_str_cmpid, string p_str_itm_code, string p_str_paletid, string p_str_ponum, string p_str_LocId, string p_str_style, string p_str_Color, string p_str_Size)
        {

            OutboundInq objOutboundInq = new OutboundInq();
            OutboundInqService ServiceObject = new OutboundInqService();
            objOutboundInq.cmp_id = p_str_cmpid;
            objOutboundInq.itm_num = p_str_style;
            objOutboundInq.itm_color = p_str_Color;
            objOutboundInq.itm_size = p_str_Size;
            objOutboundInq = ServiceObject.GetItemCode(objOutboundInq);
            if (objOutboundInq.LstAlocSummary.Count > 0)
            {
                objOutboundInq.ItmCode = objOutboundInq.LstAlocSummary[0].ItmCode;
                objOutboundInq.itm_name = objOutboundInq.LstAlocSummary[0].itm_name;
            }
            p_str_itm_code = objOutboundInq.ItmCode;

            string p_str_success = string.Empty;
            StockChange objStockChange = new StockChange();
            StockChangeService objService = new StockChangeService();
            objStockChange.cmp_id = p_str_cmpid;
            objStockChange.itm_code = p_str_itm_code;
            objStockChange.palet_id = p_str_paletid;
            objStockChange.po_num = p_str_ponum;
            objStockChange.to_loc = p_str_LocId.Trim();
            objStockChange.itm_num = p_str_style.Trim();
            objStockChange.itm_color = p_str_Color.Trim();
            objStockChange.itm_size = p_str_Size.Trim();

            Session["g_str_Search_flag"] = "True";
            objStockChange = objService.GetStockChangeDetails(objStockChange);

            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cmp_id = p_str_cmpid;
            //objOutboundInq.aloc_dt = DateTime.Now.ToString("MM/dd/yyyy");
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany.cust_cmp_id = p_str_cmpid;
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);

            objStockChange.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objCompany = ServiceObjectCompany.GetLocIdDetails(objCompany);

            objStockChange.ListLocPickDtl = objCompany.ListLocPickDtl;
            objStockChange = objService.CheckLotStatus(objStockChange);

            if (objStockChange.ListCheckLotStatus.Count > 0)
            {
                objStockChange.status = objStockChange.ListCheckLotStatus[0].status;
            }
            if (objStockChange.status == "TEMP")
            {
                p_str_success = "0";
                return Json(p_str_success, JsonRequestBehavior.AllowGet);
            }
            else
            {
                objStockChange = objService.GetItemMoveGridLoadItem(objStockChange);
                if (objStockChange.ListGetItemMoveDetails.Count > 0)
                {
                    objStockChange.ib_doc_id = objStockChange.ListGetItemMoveDetails[0].ib_doc_id;
                    objStockChange.lot_id = objStockChange.ListGetItemMoveDetails[0].lot_id;
                    objStockChange.date = Convert.ToDateTime(objStockChange.ListGetItemMoveDetails[0].rcvd_dt).ToString("MM/dd/yyyy");
                    objStockChange.whs_id = objStockChange.ListGetItemMoveDetails[0].whs_id;
                    objStockChange.frm_loc = objStockChange.ListGetItemMoveDetails[0].loc_id;
                    objStockChange.ib_doc_id = objStockChange.ListGetItemMoveDetails[0].ib_doc_id;
                    objStockChange.lot_num = p_str_paletid;
                }
            }
            Mapper.CreateMap<StockChange, StockChangeModel>();
            StockChangeModel StockChangeModel = Mapper.Map<StockChange, StockChangeModel>(objStockChange);
            return PartialView("_DashboardItemMoveGrid", StockChangeModel);
        }

        public ActionResult GetGridToLoc(int p_str_line_num, string p_str_loc_id)
        {
            try
            {
                StockChange objStockChange = new StockChange();
                StockChangeService objService = new StockChangeService();
                int l_str_RowCount = 0;
                objStockChange.loc_id = p_str_loc_id;
                objStockChange.LineNum = Convert.ToInt16(p_str_line_num);
                objStockChange.row_ctn = Convert.ToInt16(p_str_line_num);
                List<StockChange> GETItemMoveLi = new List<StockChange>();
                GETItemMoveLi = Session["GridItemMove"] as List<StockChange>;
                for (int j = 0; j < GETItemMoveLi.Count(); j++)
                {

                    GETItemMoveLi.Where(p => p.LineNum == p_str_line_num).Select(u =>
                    {
                        u.to_loc = p_str_loc_id; u.colChk = "true";
                        return u;
                    }).ToList();

                }
                objStockChange.ListGetItemMoveDetails = GETItemMoveLi;
                for (int k = 0; k < objStockChange.ListGetItemMoveDetails.Count(); k++)
                {
                    if (objStockChange.ListGetItemMoveDetails[k].colChk == "true")
                    {
                        l_str_RowCount = l_str_RowCount + 1;
                    }
                }
                objStockChange.row_ctn = l_str_RowCount;
                objStockChange.move_ctns = l_str_RowCount;
                Session["GridStkItmMove"] = objStockChange.ListGetItemMoveDetails;
                Mapper.CreateMap<StockChange, StockChangeModel>();
                StockChangeModel StockChangeModel = Mapper.Map<StockChange, StockChangeModel>(objStockChange);
                return PartialView("_GridItemMove", StockChangeModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult TotalMoveGridToLoc(string p_str_IBdoc_id, string p_str_loc_id)
        {
            try
            {
                StockChange objStockChange = new StockChange();
                StockChangeService objService = new StockChangeService();
                int l_str_RowCount = 0;
                objStockChange.loc_id = p_str_loc_id;
                List<StockChange> GETItemTotalMoveLi = new List<StockChange>();
                GETItemTotalMoveLi = Session["GridItemMove"] as List<StockChange>;
                for (int j = 0; j < GETItemTotalMoveLi.Count(); j++)
                {

                    GETItemTotalMoveLi.Where(p => p.ib_doc_id == p_str_IBdoc_id.Trim()).Select(u =>
                    {
                        u.to_loc = p_str_loc_id; u.colChk = "true";
                        return u;
                    }).ToList();
                }
                objStockChange.ListGetItemMoveDetails = GETItemTotalMoveLi;
                for (int k = 0; k < objStockChange.ListGetItemMoveDetails.Count(); k++)
                {
                    if (objStockChange.ListGetItemMoveDetails[k].colChk == "true")
                    {
                        l_str_RowCount = l_str_RowCount + 1;
                    }
                }
                objStockChange.row_ctn = l_str_RowCount;
                objStockChange.move_ctns = l_str_RowCount;
                Session["GridStkTotalItmMove"] = objStockChange.ListGetItemMoveDetails;
                Mapper.CreateMap<StockChange, StockChangeModel>();
                StockChangeModel StockChangeModel = Mapper.Map<StockChange, StockChangeModel>(objStockChange);
                return PartialView("_GridItemMove", StockChangeModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult DashboardTotalMoveGridToLoc(string p_str_IBdoc_id, string p_str_loc_id)
        {
            try
            {
                StockChange objStockChange = new StockChange();
                StockChangeService objService = new StockChangeService();
                int l_str_RowCount = 0;
                objStockChange.loc_id = p_str_loc_id;
                List<StockChange> GETItemTotalMoveLi = new List<StockChange>();
                GETItemTotalMoveLi = Session["GridItemMove"] as List<StockChange>;
                for (int j = 0; j < GETItemTotalMoveLi.Count(); j++)
                {

                    GETItemTotalMoveLi.Where(p => p.ib_doc_id == p_str_IBdoc_id.Trim()).Select(u =>
                    {
                        u.to_loc = p_str_loc_id; u.colChk = "true";
                        return u;
                    }).ToList();
                }
                objStockChange.ListGetItemMoveDetails = GETItemTotalMoveLi;
                for (int k = 0; k < objStockChange.ListGetItemMoveDetails.Count(); k++)
                {
                    if (objStockChange.ListGetItemMoveDetails[k].colChk == "true")
                    {
                        l_str_RowCount = l_str_RowCount + 1;
                    }
                }
                objStockChange.row_ctn = l_str_RowCount;
                objStockChange.move_ctns = l_str_RowCount;
                Session["GridStkTotalItmMove"] = objStockChange.ListGetItemMoveDetails;
                Mapper.CreateMap<StockChange, StockChangeModel>();
                StockChangeModel StockChangeModel = Mapper.Map<StockChange, StockChangeModel>(objStockChange);
                return PartialView("_GridDashboardItemMove", StockChangeModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult UncheckAll(string p_str_IBdoc_id)
        {
            try
            {
                StockChange objStockChange = new StockChange();
                StockChangeService objService = new StockChangeService();

                List<StockChange> GETItemUncheckLi = new List<StockChange>();
                GETItemUncheckLi = Session["GridStkTotalItmMove"] as List<StockChange>;
                for (int j = 0; j < GETItemUncheckLi.Count(); j++)
                {

                    GETItemUncheckLi.Where(p => p.ib_doc_id == p_str_IBdoc_id.Trim()).Select(u =>
                    {
                        u.colChk = "false"; u.to_loc = "";
                        return u;
                    }).ToList();
                }
                objStockChange.ListGetItemMoveDetails = GETItemUncheckLi;
                Session["GridItemMove"] = objStockChange.ListGetItemMoveDetails;
                Mapper.CreateMap<StockChange, StockChangeModel>();
                StockChangeModel StockChangeModel = Mapper.Map<StockChange, StockChangeModel>(objStockChange);
                return PartialView("_GridItemMove", StockChangeModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
 
        public ActionResult UpdItemTempDetails(string cmpId, string ibdocId, string rcvdate, string paletId, string lot_id, string frm_loc, string WhsId,
            string itm_num, string itm_color, string itm_size, int MoveQty, int MoveCtn, string Itemcode, int l_str_ppk, int tot_ctns, int tot_qty, string to_loc,
            string po_num, string cont_id)
        {
            StockChange objStockChange = new StockChange();
            StockChangeService objService = new StockChangeService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            if (frm_loc.Trim() == to_loc.Trim())
            {
                int Tolocation = 1;
                return Json(Tolocation, JsonRequestBehavior.AllowGet);
            }
            if ((tot_qty) < (MoveQty))
            {
                int Tolocation = 2;
                return Json(Tolocation, JsonRequestBehavior.AllowGet);
            }
            objStockChange.cmp_id = cmpId.Trim();
            if (clsGlobal.AdjDocId == string.Empty)
            {
                objStockChange = objService.GetAdjustDocID(objStockChange);
                objStockChange.adj_doc_id = objStockChange.adj_doc_id;
                clsGlobal.AdjDocId = objStockChange.adj_doc_id;
            }

            else
            {
                objStockChange.adj_doc_id = clsGlobal.AdjDocId;
            }


            if (WhsId == "") //To get WHSID - If WhsId = ""
            {
                objStockChange.cmp_id = cmpId;
                objStockChange.itm_code = Itemcode;
                objStockChange = objService.GetItemMoveGridLoadItem(objStockChange);
                WhsId = objStockChange.ListGetItemMoveDetails[0].whs_id;
            }

            objStockChange.adj_date = DateTime.Now.ToString("MM/dd/yyyy");
            objStockChange.itm_code = Itemcode.Trim();
            objStockChange.whs_id = WhsId.Trim();
            objStockChange.ib_doc_id = ibdocId.Trim();
            objStockChange.rcvd_dt = rcvdate.Trim();
            objStockChange.cont_id = cont_id.Trim();
            objStockChange.lot_id = lot_id.Trim();
            objStockChange.lot_num = paletId.Trim();
            objStockChange.po_num = po_num.Trim();
            objStockChange.frm_loc = frm_loc.Trim();
            objStockChange.pkg_qty = l_str_ppk;
            objStockChange.tot_ctns = tot_ctns;
            objStockChange.to_loc = to_loc.Trim();
            objStockChange.move_ctns = MoveCtn;
            objStockChange.userId = Session["UserID"].ToString().Trim();
            objStockChange = objService.InsertTempStkMove(objStockChange);
            return Json("3", JsonRequestBehavior.AllowGet);
        }

        public JsonResult ItemXGetLocDtl(string term, string cmp_id)
        {
            StockChangeService ServiceObject = new StockChangeService();
            var List = ServiceObject.ItemXGetLocDetails(term.Trim(), cmp_id).LstItmxlocdtl.Select(x => new { label = x.loc_id, value = x.loc_id }).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReUpdateTempItemDetails(string cmpId, string ibdocId, string rcvdate, string paletId, string lot_id, string frm_loc, string WhsId,
    string itm_num, string itm_color, string itm_size, int MoveQty, int MoveCtn, string Itemcode, int l_str_ppk, int tot_ctns, int tot_qty, string to_loc,
    string po_num, string cont_id)
        {
            StockChange objStockChange = new StockChange();
            StockChangeService objService = new StockChangeService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objStockChange.cmp_id = cmpId.Trim();
            objStockChange = objService.GetAdjustDocID(objStockChange);
            objStockChange.adj_doc_id = objStockChange.adj_doc_id;
            objStockChange.adj_date = DateTime.Now.ToString("MM/dd/yyyy"); ;
            objStockChange.itm_code = Itemcode.Trim();
            objStockChange.whs_id = WhsId.Trim();
            objStockChange.ib_doc_id = ibdocId.Trim();
            objStockChange.rcvd_dt = rcvdate.Trim();
            objStockChange.cont_id = cont_id.Trim();
            objStockChange.lot_id = lot_id.Trim();
            objStockChange.lot_num = paletId.Trim();
            objStockChange.po_num = po_num.Trim();
            objStockChange.frm_loc = frm_loc.Trim();
            objStockChange.tot_qty = tot_qty;
            objStockChange.tot_ctns = tot_ctns;
            objStockChange.to_loc = to_loc.Trim();
            objStockChange.move_ctns = MoveCtn;
            objStockChange.userId = Session["UserID"].ToString().Trim();
            objStockChange = objService.UpdateTempStkMove(objStockChange);
            //objStockChange = objService.SaveStkMove(objStockChange);
            //Mapper.CreateMap<StockChange, StockChangeModel>();
            //StockChangeModel StockChangeModel = Mapper.Map<StockChange, StockChangeModel>(objStockChange);
            return Json(JsonRequestBehavior.AllowGet);
        }
        public JsonResult SameLocID(string frm_loc, string to_loc)
        {
            StockChange objStockChange = new StockChange();
            StockChangeService objService = new StockChangeService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            if (frm_loc.Trim() == to_loc.Trim())
            {
                int ResultCount = 1;
                return Json(ResultCount, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        //CR_3PL_MVC _2018_0811_001 Added By Nithya
        public JsonResult STK_CHG_INQ_HDR_DATA(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_itm_num, string p_str_itm_color,
            string p_str_itm_size, string p_str_itm_name, string p_str_search_text, string p_str_cont_id, string p_str_lot_id, string p_str_loc_id, string p_str_whs_id, string p_str_Date_fm, string p_str_Date_To, string p_str_po_num)
        {
            StockChange objStockChange = new StockChange();
            StockChangeService objService = new StockChangeService();
            Session["str_cmp_id"] = p_str_cmp_id.Trim();
            Session["str_itm_num"] = p_str_itm_num.Trim();
            Session["str_itm_color"] = p_str_itm_color.Trim();
            Session["str_itm_size"] = p_str_itm_size.Trim();
            Session["str_whs_id"] = p_str_whs_id.Trim();

            Session["str_ib_doc_id"] = p_str_ib_doc_id.Trim();
            Session["str_status"] = p_str_itm_name.Trim();
            Session["str_search_text"] = p_str_search_text.Trim();
            Session["str_cont_id"] = p_str_cont_id.Trim();
            Session["str_lot_id"] = p_str_lot_id.Trim();
            Session["str_loc_id"] = p_str_loc_id.Trim();

            Session["str_rcvd_dt_fm"] = p_str_Date_fm.Trim();
            Session["str_rcvd_dt_to"] = p_str_Date_To.Trim();
            Session["str_po_num"] = p_str_po_num.Trim();
            return Json(objStockChange.ListGetStockChangeDetails, JsonRequestBehavior.AllowGet);
        }

     

       

             public EmptyResult CmpIdOnChange(string p_str_cmp_id)
        {
            Session["g_str_cmp_id"] = (p_str_cmp_id == null ? string.Empty : p_str_cmp_id.Trim());
            return null;
        }



    }
}