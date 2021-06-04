using AutoMapper;
using GsEPWv8_4_MVC.Business.Implementation;
using GsEPWv8_4_MVC.Business.Interface;
using GsEPWv8_4_MVC.Core.Entity;
using GsEPWv8_4_MVC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GsEPWv8_4_MVC.Common;

namespace GsEPWv8_4_MVC.Controllers
{
    public class StockChangeController : Controller
    {
        // GET: StockChange
        //public ActionResult Index()
        //{
        //    return View();
        //}
        DataTable dtItemMove;
        public ActionResult StockChange(string FullFillType, string cmp)
        {

            string l_str_cmp_id = string.Empty;
            try
            {
                StockChange objStockChange = new StockChange();
                IStockChangeService ServiceObject = new StockChangeService();
                objStockChange.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
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
                objStockChange.cmp_id = cmp.Trim();
                LookUp objLookUp = new LookUp();
                LookUpService ServiceObject1 = new LookUpService();
                objLookUp.id = "5";
                objLookUp.lookuptype = "INVENTORYINQ";
                objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                objStockChange.ListLookUpDtl = objLookUp.ListLookUpDtl;
                objCompany = ServiceObjectCompany.GetFullFillCompanyDetails(objCompany);
                //objStockChange.DateFrom = "DashBoard";
                //20180428 aDDED By NITHYA dEFAULT WHSID
                //objStockChange = ServiceObject.GetDftWhs(objStockChange);
                //string l_str_DftWhs = objStockChange.LstWhsDetails[0].dft_whs.Trim();
                //if (l_str_DftWhs != "" || l_str_DftWhs != null)
                //{
                //    objStockChange.whs_id = l_str_DftWhs;
                //}
                //objCompany.cust_cmp_id = cmp;
                //objCompany.whs_id = "";
                //objCompany = ServiceObjectCompany.GetWhsIdDetails(objCompany);
                //objStockChange.ListwhsPickDtl = objCompany.ListwhsPickDtl;
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
        public ActionResult GetStockChangeInqDetails(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_itm_num, string p_str_itm_color,
            string p_str_itm_size, string p_str_itm_name, string p_str_search_text, string p_str_cont_id, string p_str_lot_id, string p_str_loc_id, string p_str_whs_id, string p_str_Date_fm, string p_str_Date_To, string p_str_po_num)
        {
            try
            {
                StockChange objStockChange = new StockChange();
                StockChangeService objService = new StockChangeService();
                objStockChange.cmp_id = p_str_cmp_id.Trim();
                objStockChange.ib_doc_id = p_str_ib_doc_id.Trim();
                objStockChange.itm_num = p_str_itm_num.Trim();
                objStockChange.itm_color = p_str_itm_color.Trim();
                objStockChange.itm_size = p_str_itm_size.Trim();
                objStockChange.status = p_str_itm_name.Trim();
                objStockChange.search_text = p_str_search_text.Trim();
                objStockChange.cont_id = p_str_cont_id.Trim();
                objStockChange.lot_id = p_str_lot_id.Trim();
                objStockChange.loc_id = p_str_loc_id.Trim();
                objStockChange.whs_id = p_str_whs_id;
                objStockChange.rcvd_dt_fm = p_str_Date_fm.Trim();
                objStockChange.rcvd_dt_to = p_str_Date_To.Trim();
                objStockChange.po_num = p_str_po_num.Trim();
                objStockChange = objService.GetStockChangeDetails(objStockChange);
                Mapper.CreateMap<StockChange, StockChangeModel>();
                StockChangeModel StockChangeModel = Mapper.Map<StockChange, StockChangeModel>(objStockChange);
                return PartialView("_StockChange", StockChangeModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public ActionResult ItemMove(string p_str_cmpid, string p_str_itm_code, string p_str_paletid, string p_str_ponum, string p_str_LocId, string p_str_style, string p_str_Color, string p_str_Size)
        {
            int LineNum = 0;
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

                //dtItemMove = new DataTable();
                //List<StockChange> li = new List<StockChange>();
                ////2: Initialize a object of type DataRow
                //DataRow drStkItemMove;
                ////3: Initialize enough objects of type DataColumns
                //DataColumn colCmpId = new DataColumn("cmp_id", typeof(string));
                //DataColumn colLineNum = new DataColumn("LineNum", typeof(string));
                //DataColumn colIbDocId = new DataColumn("ib_doc_id", typeof(string));
                //DataColumn colContId = new DataColumn("cont_id", typeof(string));
                //DataColumn colitm_code = new DataColumn("itm_code", typeof(string));
                //DataColumn colitm_num = new DataColumn("itm_num", typeof(string));
                //DataColumn colitm_color = new DataColumn("itm_color", typeof(string));
                //DataColumn colitm_size = new DataColumn("itm_size", typeof(string));
                //DataColumn collot_id = new DataColumn("lot_id", typeof(string));
                //DataColumn colpo_num = new DataColumn("po_num", typeof(string));
                //DataColumn colpkg_id = new DataColumn("pkg_id", typeof(string));
                //DataColumn colwhs_id = new DataColumn("whs_id", typeof(string));
                //DataColumn colfrm_loc = new DataColumn("loc_id", typeof(string));
                //DataColumn colto_loc = new DataColumn("to_loc", typeof(string));
                //DataColumn colpkg_qty = new DataColumn("pkg_qty", typeof(int));
                //DataColumn colitm_qty = new DataColumn("itm_qty", typeof(int));
                //DataColumn colrcvd_dt = new DataColumn("rcvd_dt", typeof(string));
                //DataColumn colChk = new DataColumn("colChk", typeof(string));//2018-03-22-001 Added By Soniya

                //int lintCount = 0;

                ////4: Adding DataColumns to DataTable dt
                //dtItemMove.Columns.Add(colCmpId);
                //dtItemMove.Columns.Add(colLineNum);
                //dtItemMove.Columns.Add(colIbDocId);
                //dtItemMove.Columns.Add(colContId);
                //dtItemMove.Columns.Add(colitm_code);
                //dtItemMove.Columns.Add(colitm_num);
                //dtItemMove.Columns.Add(colitm_color);
                //dtItemMove.Columns.Add(colitm_size);
                //dtItemMove.Columns.Add(collot_id);
                //dtItemMove.Columns.Add(colpo_num);
                //dtItemMove.Columns.Add(colpkg_id);
                //dtItemMove.Columns.Add(colwhs_id);
                //dtItemMove.Columns.Add(colfrm_loc);
                //dtItemMove.Columns.Add(colto_loc);
                //dtItemMove.Columns.Add(colpkg_qty);
                //dtItemMove.Columns.Add(colitm_qty);
                //dtItemMove.Columns.Add(colrcvd_dt);
                //dtItemMove.Columns.Add(colChk);//2018-03-21-001 Added By Soniya
                //objStockChange = objService.GetItemMoveGridLoadItem(objStockChange);
                //for (int i = 0; i < objStockChange.ListGetItemMoveDetails.Count(); i++)
                //{
                //    objStockChange.cmp_id = objStockChange.ListGetItemMoveDetails[i].cmp_id;
                //    objStockChange.ib_doc_id = objStockChange.ListGetItemMoveDetails[i].ib_doc_id;
                //    objStockChange.cont_id = objStockChange.ListGetItemMoveDetails[i].cont_id;
                //    objStockChange.itm_num = objStockChange.ListGetItemMoveDetails[i].itm_num;
                //    objStockChange.itm_code = objStockChange.ListGetItemMoveDetails[i].itm_code;
                //    objStockChange.itm_color = objStockChange.ListGetItemMoveDetails[i].itm_color;
                //    objStockChange.itm_size = objStockChange.ListGetItemMoveDetails[i].itm_size;
                //    objStockChange.lot_id = objStockChange.ListGetItemMoveDetails[i].lot_id;
                //    objStockChange.po_num = objStockChange.ListGetItemMoveDetails[i].po_num;
                //    objStockChange.pkg_id = objStockChange.ListGetItemMoveDetails[i].pkg_id;
                //    objStockChange.whs_id = objStockChange.ListGetItemMoveDetails[i].whs_id;
                //    objStockChange.frm_loc = objStockChange.ListGetItemMoveDetails[i].loc_id;
                //    objStockChange.to_loc = "";
                //    objStockChange.pkg_qty = objStockChange.ListGetItemMoveDetails[i].pkg_qty;
                //    objStockChange.itm_qty = objStockChange.ListGetItemMoveDetails[i].itm_qty;
                //    objStockChange.rcvd_dt = objStockChange.ListGetItemMoveDetails[i].rcvd_dt;

                //    LineNum = LineNum + 1;

                //    drStkItemMove = dtItemMove.NewRow();

                //    dtItemMove.Rows.Add(drStkItemMove);
                //    dtItemMove.Rows[lintCount][colCmpId] = objStockChange.cmp_id.ToString();
                //    dtItemMove.Rows[lintCount][colLineNum] = LineNum;
                //    dtItemMove.Rows[lintCount][colIbDocId] = objStockChange.ib_doc_id.ToString();
                //    dtItemMove.Rows[lintCount][colContId] = objStockChange.cont_id.ToString();
                //    dtItemMove.Rows[lintCount][colitm_code] = objStockChange.itm_code.ToString();
                //    dtItemMove.Rows[lintCount][colitm_num] = objStockChange.itm_num.ToString();
                //    dtItemMove.Rows[lintCount][colitm_color] = objStockChange.itm_color.ToString();
                //    dtItemMove.Rows[lintCount][colitm_size] = objStockChange.itm_size.ToString();
                //    dtItemMove.Rows[lintCount][collot_id] = objStockChange.lot_id.ToString();
                //    dtItemMove.Rows[lintCount][colpo_num] = objStockChange.po_num.ToString();
                //    dtItemMove.Rows[lintCount][colpkg_id] = objStockChange.pkg_id.ToString();
                //    dtItemMove.Rows[lintCount][colwhs_id] = objStockChange.whs_id.ToString();
                //    dtItemMove.Rows[lintCount][colfrm_loc] = objStockChange.frm_loc.ToString();
                //    dtItemMove.Rows[lintCount][colto_loc] = objStockChange.to_loc.ToString();
                //    dtItemMove.Rows[lintCount][colpkg_qty] = objStockChange.pkg_qty.ToString();
                //    dtItemMove.Rows[lintCount][colitm_qty] = objStockChange.itm_qty.ToString();
                //    dtItemMove.Rows[lintCount][colrcvd_dt] = objStockChange.rcvd_dt.ToString();
                //    dtItemMove.Rows[lintCount][colChk] = "false";
                //    StockChange objStockChangetemp = new StockChange();
                //    objStockChangetemp.cmp_id = objStockChange.cmp_id;
                //    objStockChangetemp.LineNum = LineNum;
                //    objStockChangetemp.ib_doc_id = objStockChange.ib_doc_id.Trim();
                //    objStockChangetemp.cont_id = objStockChange.cont_id;
                //    objStockChangetemp.itm_num = objStockChange.itm_num;
                //    objStockChangetemp.itm_code = objStockChange.itm_code;
                //    objStockChangetemp.itm_color = objStockChange.itm_color;
                //    objStockChangetemp.itm_size = objStockChange.itm_size;
                //    objStockChangetemp.lot_id = objStockChange.lot_id;
                //    objStockChangetemp.po_num = objStockChange.po_num;
                //    objStockChangetemp.pkg_id = objStockChange.pkg_id;
                //    objStockChangetemp.whs_id = objStockChange.whs_id;
                //    objStockChangetemp.frm_loc = objStockChange.frm_loc;
                //    objStockChangetemp.to_loc = objStockChange.to_loc;
                //    objStockChangetemp.pkg_qty = objStockChange.pkg_qty;
                //    objStockChangetemp.itm_qty = objStockChange.itm_qty;
                //    objStockChangetemp.rcvd_dt = objStockChange.rcvd_dt;
                //    objStockChangetemp.colChk = "false";
                //    li.Add(objStockChangetemp);
                //    lintCount++;
                //}
                //objStockChange.ListGetItemMoveDetails = li;
                //Session["GridItemMove"] = objStockChange.ListGetItemMoveDetails;
                //objStockChange.ib_doc_id = objStockChange.ListGetItemMoveDetails[0].ib_doc_id;
                //objStockChange.lot_id = objStockChange.ListGetItemMoveDetails[0].lot_id;
                ////objStockChange.date = objStockChange.ListGetItemMoveDetails[0].rcvd_dt;
                //objStockChange.date = Convert.ToDateTime(objStockChange.ListGetItemMoveDetails[0].rcvd_dt).ToString("MM/dd/yyyy");
                //objStockChange.whs_id = objStockChange.ListGetItemMoveDetails[0].whs_id;
                //objStockChange.frm_loc = objStockChange.ListGetItemMoveDetails[0].frm_loc;
                //objStockChange.ib_doc_id = objStockChange.ListGetItemMoveDetails[0].ib_doc_id;
                //objStockChange.lot_num = p_str_paletid;
                //objStockChange.loc_id = p_str_LocId.Trim();
                //objStockChange = objService.GetItemMoveTotQty(objStockChange);
                //objStockChange.tot_ctn = Convert.ToInt32(objStockChange.ListGetItemMoveTotQty[0].TotCtns);
                //objStockChange.tot_qty = Convert.ToInt32(objStockChange.ListGetItemMoveTotQty[0].TotQty);
            }
            Mapper.CreateMap<StockChange, StockChangeModel>();
            StockChangeModel StockChangeModel = Mapper.Map<StockChange, StockChangeModel>(objStockChange);
            return PartialView("_ItemMove", StockChangeModel);
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
        //public ActionResult GetGridToLocClear(int p_str_line_num, string p_str_loc_id)
        //{
        //    try
        //    {
        //        StockChange objStockChange = new StockChange();
        //        StockChangeService objService = new StockChangeService();
        //        int l_str_RowCount = 0;
        //        objStockChange.loc_id = p_str_loc_id;
        //        objStockChange.LineNum = Convert.ToInt16(p_str_line_num);
        //        objStockChange.row_ctn = Convert.ToInt16(p_str_line_num);
        //        List<StockChange> GETItemMoveLi = new List<StockChange>();
        //        GETItemMoveLi = Session["GridItemMove"] as List<StockChange>;
        //        for (int j = 0; j < GETItemMoveLi.Count(); j++)
        //        {

        //            GETItemMoveLi.Where(p => p.LineNum == p_str_line_num).Select(u =>
        //            {
        //                u.to_loc = p_str_loc_id; u.colChk = "false";
        //                return u;
        //            }).ToList();

        //        }
        //        objStockChange.ListGetItemMoveDetails = GETItemMoveLi;
        //        for (int k = 0; k < objStockChange.ListGetItemMoveDetails.Count(); k++)
        //        {
        //            if (objStockChange.ListGetItemMoveDetails[k].colChk == "false")
        //            {
        //                l_str_RowCount = l_str_RowCount + 1;
        //            }
        //        }
        //        objStockChange.row_ctn = l_str_RowCount;
        //        objStockChange.move_ctns = l_str_RowCount;
        //        Session["GridStkItmMove"] = objStockChange.ListGetItemMoveDetails;
        //        Mapper.CreateMap<StockChange, StockChangeModel>();
        //        StockChangeModel StockChangeModel = Mapper.Map<StockChange, StockChangeModel>(objStockChange);
        //        return PartialView("_GridItemMove", StockChangeModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }


        //}
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
        public ActionResult SaveItemMove(string p_str_cmp_id, string p_str_ibdocid, string p_str_ibdocdt, string p_str_lot_no, string p_str_lot_id, string p_str_whs_id, string p_str_frm_loc, string p_str_loc_id,
            string p_str_tot_ctn, string p_str_tot_qty, string p_str_row_ctn, string p_str_move_ctns, string p_str_move_qty, string var_name, string var_name1)
        {
            string l_str_ibdocid;
            string SelPkgIds = string.Empty;
            string ItemCode = string.Empty;
            int ItmQty = 0;
            StockChange objStockChange = new StockChange();
            StockChangeService objService = new StockChangeService();
            int l_str_RowCount = 0;
            objStockChange.cmp_id = p_str_cmp_id;
            //objStockChange.ib_doc_id = p_str_ibdocid;
            objStockChange.adj_doc_id = clsGlobal.AdjDocId;
            objStockChange = objService.SaveStkMove(objStockChange);
            clsGlobal.AdjDocId = string.Empty;
            //List<StockChange> GETItemMoveLi = new List<StockChange>();
            //GETItemMoveLi = Session["GridStkItmMove"] as List<StockChange>;
            //objStockChange.ListGetItemMoveDetails = GETItemMoveLi;
            //if (GETItemMoveLi == null)
            //{
            //    GETItemMoveLi = Session["GridStkTotalItmMove"] as List<StockChange>;
            //    objStockChange.ListGetItemMoveDetails = GETItemMoveLi;
            //}
            //for (int k = 0; k < objStockChange.ListGetItemMoveDetails.Count(); k++)
            //{
            //    if (objStockChange.ListGetItemMoveDetails[k].colChk == "true")
            //    {
            //        l_str_RowCount = l_str_RowCount + 1;
            //    }
            //}
            //objStockChange.row_ctn = l_str_RowCount;
            //objStockChange.move_ctns = l_str_RowCount;
            //p_str_move_ctns = Convert.ToString(objStockChange.move_ctns);
            //p_str_row_ctn = Convert.ToString(objStockChange.row_ctn);
            //objStockChange = objService.GetAdjustDocID(objStockChange);
            //objStockChange.adj_doc_id = objStockChange.adj_doc_id;
            //if (var_name == "TotalMove" || p_str_move_ctns == p_str_tot_ctn)
            //{
            //    GETItemMoveLi = Session["GridStkTotalItmMove"] as List<StockChange>;
            //    objStockChange.ListGetItemMoveDetails = GETItemMoveLi;
            //    objStockChange = objService.GetPalletId(objStockChange);
            //    objStockChange.palet_id = objStockChange.palet_id;
            //    for (int i = 0; i < objStockChange.ListGetItemMoveDetails.Count(); i++)
            //    {
            //        if (objStockChange.ListGetItemMoveDetails[i].colChk == "true")
            //        {
            //            objStockChange.itm_code = objStockChange.ListGetItemMoveDetails[i].itm_code;
            //            ItemCode = objStockChange.itm_code;
            //            objStockChange.itm_qty = objStockChange.ListGetItemMoveDetails[i].itm_qty;
            //            ItmQty = objStockChange.itm_qty;
            //            objStockChange.cmp_id = objStockChange.ListGetItemMoveDetails[i].cmp_id;
            //            objStockChange.lot_id = objStockChange.ListGetItemMoveDetails[i].lot_id;
            //            objStockChange.whs_id = objStockChange.ListGetItemMoveDetails[i].whs_id;
            //            objStockChange.rcvd_dt = objStockChange.ListGetItemMoveDetails[i].rcvd_dt;
            //            objStockChange.pkg_id = objStockChange.ListGetItemMoveDetails[i].pkg_id;

            //            objStockChange.loc_id = p_str_loc_id;

            //            objStockChange = objService.Ck_Loc_Itm_Avail(objStockChange);
            //            if (objStockChange.cnt == 0)
            //            {
            //                objStockChange = objService.Validate_ItmCode(objStockChange);
            //                if (objStockChange.ListCheckLotStatus.Count > 0)
            //                {
            //                    objStockChange.itm_num = objStockChange.ListCheckLotStatus[0].itm_num;
            //                    objStockChange.itm_color = objStockChange.ListCheckLotStatus[0].itm_color;
            //                    objStockChange.itm_size = objStockChange.ListCheckLotStatus[0].itm_size;
            //                    objStockChange.itm_name = objStockChange.ListCheckLotStatus[0].itm_name;
            //                    objStockChange.kit_itm = objStockChange.ListCheckLotStatus[0].kit_itm;
            //                }
            //                objStockChange.optid = "D";
            //                objStockChange = objService.Add_Iv_Itm_Trn_Hdr(objStockChange);
            //                objStockChange.loc_id = p_str_frm_loc;
            //                objStockChange = objService.Update_Rcvd_Qty(objStockChange);
            //            }
            //            else
            //            {

            //                objStockChange.optid = "A";
            //                objStockChange.loc_id = p_str_loc_id;
            //                objStockChange = objService.Update_Rcvd_Qty(objStockChange);
            //                objStockChange.optid = "D";
            //                objStockChange.lot_num = p_str_lot_no;
            //                objStockChange.frm_loc = p_str_frm_loc;
            //                objStockChange.to_loc = p_str_loc_id;
            //                objStockChange.loc_id = p_str_frm_loc;
            //                objStockChange = objService.Update_Rcvd_Qty(objStockChange);
            //            }
            //            objStockChange.lot_num = p_str_lot_no;
            //            objStockChange.frm_loc = p_str_frm_loc;
            //            objStockChange.to_loc = p_str_loc_id;
            //            objStockChange = objService.UPD_TRNIN_TRNHST_PART_TPW(objStockChange);

            //        }

            //    }
            //    objStockChange = objService.UPD_LOTDTL_STK_XFER_PART_TPW(objStockChange);
            //    //SelPkgIds
            //}
            //else
            //{
            //    if (p_str_move_ctns == "0")
            //    {
            //        return Json("", JsonRequestBehavior.AllowGet);
            //    }
            //    objStockChange = objService.GetPalletId(objStockChange);
            //    objStockChange.palet_id = objStockChange.palet_id;
            //    for (int i = 0; i < objStockChange.ListGetItemMoveDetails.Count(); i++)
            //    {
            //        if (objStockChange.ListGetItemMoveDetails[i].colChk == "true")
            //        {
            //            objStockChange.itm_code = objStockChange.ListGetItemMoveDetails[i].itm_code;
            //            ItemCode = objStockChange.itm_code;
            //            objStockChange.itm_qty = objStockChange.ListGetItemMoveDetails[i].itm_qty;
            //            ItmQty = objStockChange.itm_qty;
            //            objStockChange.cmp_id = objStockChange.ListGetItemMoveDetails[i].cmp_id;
            //            objStockChange.lot_id = objStockChange.ListGetItemMoveDetails[i].lot_id;
            //            objStockChange.whs_id = objStockChange.ListGetItemMoveDetails[i].whs_id;
            //            objStockChange.rcvd_dt = objStockChange.ListGetItemMoveDetails[i].rcvd_dt;
            //            objStockChange.pkg_id = objStockChange.ListGetItemMoveDetails[i].pkg_id;
            //            objStockChange.loc_id = p_str_loc_id;
            //            objStockChange = objService.Ck_Loc_Itm_Avail(objStockChange);
            //            if (objStockChange.cnt == 0)
            //            {
            //                objStockChange = objService.Validate_ItmCode(objStockChange);
            //                if (objStockChange.ListCheckLotStatus.Count > 0)
            //                {
            //                    objStockChange.itm_num = objStockChange.ListCheckLotStatus[0].itm_num;
            //                    objStockChange.itm_color = objStockChange.ListCheckLotStatus[0].itm_color;
            //                    objStockChange.itm_size = objStockChange.ListCheckLotStatus[0].itm_size;
            //                    objStockChange.itm_name = objStockChange.ListCheckLotStatus[0].itm_name;
            //                    objStockChange.kit_itm = objStockChange.ListCheckLotStatus[0].kit_itm;
            //                }
            //                objStockChange.optid = "D";
            //                objStockChange.loc_id = p_str_loc_id;
            //                objStockChange = objService.Add_Iv_Itm_Trn_Hdr(objStockChange);
            //                objStockChange.loc_id = p_str_frm_loc;
            //                objStockChange = objService.Update_Rcvd_Qty(objStockChange);
            //            }
            //            else
            //            {

            //                objStockChange.optid = "A";
            //                objStockChange.loc_id = p_str_loc_id;
            //                objStockChange = objService.Update_Rcvd_Qty(objStockChange);
            //                objStockChange.optid = "D";
            //                objStockChange.lot_num = p_str_lot_no;
            //                objStockChange.frm_loc = p_str_frm_loc;
            //                objStockChange.to_loc = p_str_loc_id;
            //                objStockChange.loc_id = p_str_frm_loc;
            //                objStockChange = objService.Update_Rcvd_Qty(objStockChange);
            //            }
            //            objStockChange.lot_num = p_str_lot_no;
            //            objStockChange.frm_loc = p_str_frm_loc;
            //            objStockChange.to_loc = p_str_loc_id;
            //            objStockChange = objService.UPD_TRNIN_TRNHST_PART_TPW(objStockChange);

            //        }
            //    }
            //    objStockChange = objService.UPD_LOTDTL_STK_XFER_PART_TPW(objStockChange);
            //}
            return Json("1", JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdItemTempDetails(string cmpId, string ibdocId, string rcvdate, string paletId, string lot_id, string frm_loc, string WhsId,
            string itm_num, string itm_color, string itm_size, int MoveQty, int MoveCtn, string Itemcode,int l_str_ppk,int tot_ctns, int tot_qty,string to_loc,
            string po_num,string cont_id)
        {
            StockChange objStockChange = new StockChange();
            StockChangeService objService = new StockChangeService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            if(frm_loc.Trim()==to_loc.Trim())
            {
                int Tolocation = 1;
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
            objStockChange.pkg_qty = l_str_ppk;
            objStockChange.tot_ctns = tot_ctns;
            objStockChange.to_loc = to_loc.Trim();
            objStockChange.move_ctns = MoveCtn;
            objStockChange.userId = Session["UserID"].ToString().Trim();
            objStockChange = objService.InsertTempStkMove(objStockChange);
            //Mapper.CreateMap<StockChange, StockChangeModel>();
            //StockChangeModel StockChangeModel = Mapper.Map<StockChange, StockChangeModel>(objStockChange);
            return Json(JsonRequestBehavior.AllowGet);
        }
        public JsonResult ItemXGetLocDtl(string term, string cmp_id)
        {
            StockChangeService ServiceObject = new StockChangeService();
            var List = ServiceObject.ItemXGetLocDetails(term.Trim(), cmp_id).LstItmxlocdtl.Select(x => new { label = x.loc_id, value = x.loc_id}).ToList();
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
        public JsonResult SameLocID( string frm_loc,string to_loc)
        {
            StockChange objStockChange = new StockChange();
            StockChangeService objService = new StockChangeService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            if(frm_loc.Trim()== to_loc.Trim())
            {                
                return Json(to_loc, JsonRequestBehavior.AllowGet);
            }                 
            return Json(JsonRequestBehavior.AllowGet);
        }
        
    }
}