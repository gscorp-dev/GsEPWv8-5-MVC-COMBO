using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Mvc.Html;
using System.Configuration;
using System.IO;
//CR#
//CR2018-03-10-001 Commented By Nithya for Summary By Style  Report Not Showing 
namespace GsEPWv8_5_MVC.Controllers
{
    public class StockInquiryController : Controller
    {

        int TranTotalQty = 0;
        int TranTotalCtn = 0;

        int ShipTotalQty = 0;
        int ShipTotalCtn = 0;

        int AdjTotalQtyAdd = 0;
        int AdjTotalCtnAdd = 0;

        int AdjTotalQtyMin = 0;
        int AdjTotalCtnMin = 0;

        int AlocTotalQty = 0;
        int AlocTotalCtn = 0;

        int Total = 0;
        int l_int_rcvd_ctn = 0;
        int l_int_rcvd_qty = 0;

        int AvlCtn = 0;
        int AvlQty = 0;

        int AlocQty = 0;
        DataTable dtStockInq;
        public string EmailSub = string.Empty;
        public string EmailMsg = string.Empty;
        public string Folderpath = string.Empty;
        CustMaster objCustMaster = new CustMaster();
        ICustMasterService objCustMasterService = new CustMasterService();


        public ActionResult StockInquiry(string FullFillType, string cmp, string status, string DateFm, string DateTo, string screentitle)
        {

            string l_str_cmp_id = string.Empty;
            try
            {
                StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                IStockInquiryService ServiceObject = new StockInquiryService();
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();

                if (objStackInquiry.cmp_id == null || objStackInquiry.cmp_id == string.Empty)
                {
                    objStackInquiry.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                }
                else
                {
                    objCompany.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                    objStackInquiry.cmp_id = objCompany.cmp_id;
                }

                objStackInquiry.is_company_user = Session["IsCompanyUser"].ToString().Trim();

                if (objStackInquiry.cmp_id != "")
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objStackInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                }
                else
                {
                    if (FullFillType == null)
                    {
                        objCompany.user_id = Session["UserID"].ToString().Trim();
                        objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                        objStackInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    }
                }
                if (FullFillType != null)
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objStackInquiry.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    objStackInquiry.cmp_id = cmp.Trim();
                    objStackInquiry.ib_doc_id = "";
                    objStackInquiry.po_num = "";
                    objStackInquiry.Ref_Num = "";
                    objStackInquiry.cont_id = "";
                    objStackInquiry.lot_id = "";
                    objStackInquiry.loc_id = "";
                    objStackInquiry.whs_id = "";
                    objStackInquiry.Itmdtl = "";
                    objStackInquiry.itm_color = "";
                    objStackInquiry.itm_size = "";
                    objStackInquiry.itm_name = "";
                    objStackInquiry.Date_fm = "";
                    objStackInquiry.Date_to = "";


                }
                LookUp objLookUp = new LookUp();
                LookUpService ServiceObject1 = new LookUpService();
                objLookUp.id = "5";
                objLookUp.lookuptype = "INVENTORYINQ";
                objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                objStackInquiry.ListLookUpDtl = objLookUp.ListLookUpDtl;
                objStackInquiry.p_str_company = objStackInquiry.cmp_id;
                Session["tempAlocQty"] = 0;
                Mapper.CreateMap<StockInquiryDtl, StockInquiryDtlModel>();
                StockInquiryDtlModel objStockInquiryModel = Mapper.Map<StockInquiryDtl, StockInquiryDtlModel>(objStackInquiry);
                return View(objStockInquiryModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        [HttpGet]

        public ActionResult GetStockGridINQDetails(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_Status, string p_str_itm_num, string p_str_itm_color,
            string p_str_itm_size, string p_str_itm_name, string p_str_cont_id, string p_str_lot_id, string p_str_loc_id, string p_str_grn_id, string p_str_whs_id, string p_str_Date_fm, string p_str_Date_To, string p_str_po_num)
        {
            string l_str_stkcmp_id = string.Empty;
            string l_str_stkitm_num = string.Empty;
            string l_str_stkitm_color = string.Empty;
            string l_str_stkitm_size = string.Empty;
            string l_str_stkitm_desc = string.Empty;
            string l_str_stklot_id = string.Empty;
            string l_str_stkpalet_id = string.Empty;
            string l_str_stkcont_id = string.Empty;
            string l_str_stkwhs_id = string.Empty;
            string l_str_stkloc_id = string.Empty;
            string l_str_stkpkg_type = string.Empty;
            string l_str_stkpo_num = string.Empty;
            string l_str_stkibdoc_id = string.Empty;
            string l_str_stkrcvd_dt = string.Empty;
            int l_str_AsrtItm = 0;
            int l_str_stpkg_qty = 0;
            int l_str_stkitm_qty = 0;
            int l_str_stktot_ctn = 0;
            int l_str_stktot_qty = 0;
            int l_str_aloc_ctn = 0;
            int l_str_aloc_qty = 0;
            string l_str_aloccmp_id = string.Empty;
            string l_str_alocitm_num = string.Empty;
            string l_str_alocitm_color = string.Empty;
            string l_str_alocitm_size = string.Empty;
            string l_str_alocibdoc_id = string.Empty;
            string l_str_aloclot_id = string.Empty;
            string l_str_alocpalet_id = string.Empty;
            string l_str_aloccont_id = string.Empty;
            string l_str_alocloc_id = string.Empty;
            string l_str_alocpkg_type = string.Empty;
            string l_str_alocpo_num = string.Empty;
            string l_str_alocwhs = string.Empty;
            int l_str_alocitm_qty = 0;
            int l_str_alocpkg_qty = 0;
            int l_str_count1 = 0;
            int l_str_count2 = 0;
            dtStockInq = new DataTable();
            List<StockInquiryDtl> li = new List<StockInquiryDtl>();
            //2: Initialize a object of type DataRow
            DataRow drStk;

            //3: Initialize enough objects of type DataColumns
            DataColumn colCmpId = new DataColumn("cmp_id", typeof(string));
            DataColumn colib_doc_id = new DataColumn("ib_doc_id", typeof(string));
            DataColumn colrcvd_dt = new DataColumn("rcvd_dt", typeof(string));
            DataColumn colcont_id = new DataColumn("cont_id", typeof(string));
            DataColumn colitm_num = new DataColumn("itm_num", typeof(string));
            DataColumn colitm_color = new DataColumn("itm_color", typeof(string));
            DataColumn colitm_size = new DataColumn("itm_size", typeof(string));
            DataColumn colitm_name = new DataColumn("itm_name", typeof(string));
            DataColumn colloc_id = new DataColumn("loc_id", typeof(string));
            DataColumn collot_id = new DataColumn("lot_id", typeof(string));
            DataColumn colpalet_id = new DataColumn("palet_id", typeof(string));
            DataColumn colpo_num = new DataColumn("po_num", typeof(string));
            DataColumn colTOT_CTN = new DataColumn("TOT_CTN", typeof(int));
            DataColumn colpkg_qty = new DataColumn("pkg_qty", typeof(int));
            DataColumn colwhs_id = new DataColumn("whs_id", typeof(string));
            DataColumn colitm_qty = new DataColumn("itm_qty", typeof(int));
            DataColumn coltot_qty = new DataColumn("tot_qty", typeof(int));
            DataColumn colAlocCtn = new DataColumn("AlocCtn", typeof(int));
            DataColumn colAlocqty = new DataColumn("Alocqty", typeof(int));
            DataColumn colpkg_type = new DataColumn("pkg_type", typeof(string));
            DataColumn colkit_id = new DataColumn("kit_id", typeof(string));
            int lintCount = 0;

            //4: Adding DataColumns to DataTable dt
            dtStockInq.Columns.Add(colCmpId);
            dtStockInq.Columns.Add(colib_doc_id);
            dtStockInq.Columns.Add(colrcvd_dt);
            dtStockInq.Columns.Add(colcont_id);
            dtStockInq.Columns.Add(colitm_num);
            dtStockInq.Columns.Add(colitm_color);
            dtStockInq.Columns.Add(colitm_size);
            dtStockInq.Columns.Add(colitm_name);
            dtStockInq.Columns.Add(colloc_id);
            dtStockInq.Columns.Add(collot_id);
            dtStockInq.Columns.Add(colpalet_id);
            dtStockInq.Columns.Add(colpo_num);
            dtStockInq.Columns.Add(colTOT_CTN);
            dtStockInq.Columns.Add(colpkg_qty);
            dtStockInq.Columns.Add(colwhs_id);
            dtStockInq.Columns.Add(colitm_qty);
            dtStockInq.Columns.Add(coltot_qty);
            dtStockInq.Columns.Add(colAlocCtn);
            dtStockInq.Columns.Add(colAlocqty);
            dtStockInq.Columns.Add(colpkg_type);
            dtStockInq.Columns.Add(colkit_id);

            StockInquiryDtl objStackInquiry = new StockInquiryDtl();
            IStockInquiryService ServiceObject = new StockInquiryService();
            //ServiceObject.TruncateTempStockInquiry(objStackInquiry);
            //objStackInquiry.cmp_id = "OSU";
            objStackInquiry.is_company_user = Session["IsCompanyUser"].ToString().Trim();
            objStackInquiry = ServiceObject.GetStockInquiryDetails(objStackInquiry);
            objStackInquiry = ServiceObject.GetStockInquiryDetailAloc(objStackInquiry);
            l_str_count1 = objStackInquiry.ListStockInquiry.Count();
            l_str_count2 = objStackInquiry.LstStockInquirystock.Count();
            for (int i = 0; i < l_str_count1; i++)
            {
                l_str_stkitm_num = "";
                l_str_stkitm_color = "";
                l_str_stkitm_size = "";
                l_str_stklot_id = "";
                l_str_stkpalet_id = "";
                l_str_stkwhs_id = "";
                l_str_stpkg_qty = 0;
                l_str_stpkg_qty = 0;
                l_str_stktot_ctn = 0;
                l_str_stktot_qty = 0;
                l_str_aloc_ctn = 0;
                l_str_aloc_qty = 0;
                l_str_stkcmp_id = objStackInquiry.ListStockInquiry[i].cmp_id;
                l_str_AsrtItm = Convert.ToInt32(objStackInquiry.ListStockInquiry[i].kit_id);
                l_str_stkitm_num = objStackInquiry.ListStockInquiry[i].itm_num;
                l_str_stkitm_color = objStackInquiry.ListStockInquiry[i].itm_color;
                l_str_stkitm_size = objStackInquiry.ListStockInquiry[i].itm_size;
                l_str_stkitm_desc = objStackInquiry.ListStockInquiry[i].itm_name;
                l_str_stklot_id = objStackInquiry.ListStockInquiry[i].lot_id;
                l_str_stkpalet_id = objStackInquiry.ListStockInquiry[i].palet_id;
                l_str_stkcont_id = objStackInquiry.ListStockInquiry[i].cont_id;
                l_str_stkloc_id = objStackInquiry.ListStockInquiry[i].loc_id;
                l_str_stkwhs_id = objStackInquiry.ListStockInquiry[i].whs_id;
                l_str_stkpkg_type = objStackInquiry.ListStockInquiry[i].pkg_type;
                l_str_stkpo_num = objStackInquiry.ListStockInquiry[i].po_num;
                l_str_stkibdoc_id = objStackInquiry.ListStockInquiry[i].ib_doc_id;
                l_str_stpkg_qty = objStackInquiry.ListStockInquiry[i].pkg_qty;
                l_str_stkitm_qty = Convert.ToInt32(objStackInquiry.ListStockInquiry[i].itm_qty);
                l_str_stktot_qty = objStackInquiry.ListStockInquiry[i].AvlQty;
                l_str_stktot_ctn = objStackInquiry.ListStockInquiry[i].AvlCtn;
                l_str_stkrcvd_dt = objStackInquiry.ListStockInquiry[i].rcvd_dt;

                for (int j = 0; j < l_str_count2; j++)
                {
                    l_str_aloccmp_id = objStackInquiry.LstStockInquirystock[j].cmp_id;
                    l_str_alocitm_num = objStackInquiry.LstStockInquirystock[j].itm_num;
                    l_str_alocitm_color = objStackInquiry.LstStockInquirystock[j].itm_color;
                    l_str_alocitm_size = objStackInquiry.LstStockInquirystock[j].itm_size;
                    l_str_alocibdoc_id = objStackInquiry.LstStockInquirystock[j].ib_doc_id;
                    l_str_aloclot_id = objStackInquiry.LstStockInquirystock[j].lot_id;
                    l_str_alocpalet_id = objStackInquiry.LstStockInquirystock[j].palet_id;
                    l_str_aloccont_id = objStackInquiry.LstStockInquirystock[j].cont_id;
                    l_str_alocloc_id = objStackInquiry.LstStockInquirystock[j].loc_id;
                    l_str_alocpkg_type = objStackInquiry.LstStockInquirystock[j].pkg_type;
                    l_str_alocpo_num = objStackInquiry.LstStockInquirystock[j].po_num;
                    l_str_alocwhs = objStackInquiry.LstStockInquirystock[j].whs_id;
                    l_str_alocitm_qty = Convert.ToInt32(objStackInquiry.LstStockInquirystock[j].itm_qty);
                    l_str_alocpkg_qty = objStackInquiry.LstStockInquirystock[j].pkg_qty;

                    if (l_str_stkcmp_id == l_str_aloccmp_id && l_str_stkpalet_id == l_str_alocpalet_id && l_str_stkpo_num == l_str_alocpo_num && l_str_stklot_id == l_str_aloclot_id && l_str_stkwhs_id == l_str_alocwhs
                    && l_str_stpkg_qty == l_str_alocpkg_qty && l_str_stkitm_num == l_str_alocitm_num && l_str_stkitm_color == l_str_alocitm_color && l_str_stkitm_size == l_str_alocitm_size && l_str_stkloc_id == l_str_alocloc_id && l_str_stkpkg_type == l_str_alocpkg_type)
                    {
                        l_str_aloc_ctn = Convert.ToInt32(objStackInquiry.LstStockInquirystock[j].AlocCtn);
                        l_str_aloc_qty = objStackInquiry.LstStockInquirystock[j].AlocQty;
                        l_str_stktot_ctn = l_str_stktot_ctn - l_str_aloc_ctn;
                        l_str_stktot_qty = l_str_stktot_qty - l_str_aloc_qty;
                        break;
                    }

                }
                if (l_str_stktot_qty <= 0)
                {
                    continue;
                }
                objStackInquiry.cmp_id = l_str_stkcmp_id;
                objStackInquiry.ib_doc_id = l_str_stkibdoc_id;
                objStackInquiry.rcvd_dt = l_str_stkrcvd_dt;
                objStackInquiry.cont_id = l_str_stkcont_id;
                objStackInquiry.itm_num = l_str_stkitm_num;
                objStackInquiry.itm_color = l_str_stkitm_color;
                objStackInquiry.itm_size = l_str_stkitm_size;
                objStackInquiry.itm_name = l_str_stkitm_desc;
                objStackInquiry.loc_id = l_str_stkloc_id;
                objStackInquiry.lot_id = l_str_stklot_id;
                objStackInquiry.palet_id = l_str_stkpalet_id;
                objStackInquiry.po_num = l_str_stkpo_num;
                objStackInquiry.TOT_CTN = l_str_stktot_ctn;
                objStackInquiry.pkg_qty = l_str_stpkg_qty;
                objStackInquiry.whs_id = l_str_stkwhs_id;
                objStackInquiry.itm_qty = l_str_stkitm_qty;
                objStackInquiry.tot_qty = l_str_stktot_qty;
                objStackInquiry.AlocCtn = Convert.ToString(l_str_aloc_ctn);
                objStackInquiry.Alocqty = Convert.ToString(l_str_aloc_qty);
                objStackInquiry.pkg_type = l_str_stkpkg_type;
                objStackInquiry.kit_id = Convert.ToString(l_str_AsrtItm);
                //ServiceObject.InsertTempInventory(objStackInquiry);
                drStk = dtStockInq.NewRow();

                dtStockInq.Rows.Add(drStk);
                dtStockInq.Rows[lintCount][colCmpId] = objStackInquiry.cmp_id.ToString();
                dtStockInq.Rows[lintCount][colib_doc_id] = objStackInquiry.ib_doc_id.ToString();
                dtStockInq.Rows[lintCount][colrcvd_dt] = objStackInquiry.rcvd_dt.ToString();
                dtStockInq.Rows[lintCount][colcont_id] = objStackInquiry.cont_id.ToString();
                dtStockInq.Rows[lintCount][colitm_num] = objStackInquiry.itm_num.ToString();
                dtStockInq.Rows[lintCount][colitm_color] = objStackInquiry.itm_color.ToString();
                dtStockInq.Rows[lintCount][colitm_size] = objStackInquiry.itm_size.ToString();
                dtStockInq.Rows[lintCount][colitm_name] = objStackInquiry.itm_name.ToString();
                dtStockInq.Rows[lintCount][colloc_id] = objStackInquiry.loc_id.ToString();
                dtStockInq.Rows[lintCount][collot_id] = objStackInquiry.lot_id.ToString();
                dtStockInq.Rows[lintCount][colpalet_id] = objStackInquiry.palet_id.ToString();
                dtStockInq.Rows[lintCount][colpo_num] = objStackInquiry.po_num.ToString();
                dtStockInq.Rows[lintCount][colTOT_CTN] = objStackInquiry.TOT_CTN.ToString();
                dtStockInq.Rows[lintCount][colpkg_qty] = objStackInquiry.pkg_qty.ToString();
                dtStockInq.Rows[lintCount][colwhs_id] = objStackInquiry.whs_id.ToString();
                dtStockInq.Rows[lintCount][colitm_qty] = objStackInquiry.itm_qty.ToString();
                dtStockInq.Rows[lintCount][coltot_qty] = objStackInquiry.tot_qty.ToString();
                dtStockInq.Rows[lintCount][colAlocCtn] = objStackInquiry.AlocCtn.ToString();
                dtStockInq.Rows[lintCount][colAlocqty] = objStackInquiry.Alocqty.ToString();
                dtStockInq.Rows[lintCount][colpkg_type] = objStackInquiry.pkg_type.ToString();
                dtStockInq.Rows[lintCount][colkit_id] = objStackInquiry.kit_id.ToString();
                StockInquiryDtl objStockInquiryDtltemp = new StockInquiryDtl();
                objStockInquiryDtltemp.cmp_id = objStackInquiry.cmp_id;
                objStockInquiryDtltemp.ib_doc_id = objStackInquiry.ib_doc_id;
                objStockInquiryDtltemp.rcvd_dt = objStackInquiry.rcvd_dt;
                objStockInquiryDtltemp.cont_id = objStackInquiry.cont_id;
                objStockInquiryDtltemp.itm_num = objStackInquiry.itm_num;
                objStockInquiryDtltemp.itm_color = objStackInquiry.itm_color;
                objStockInquiryDtltemp.itm_size = objStackInquiry.itm_size;
                objStockInquiryDtltemp.itm_name = objStackInquiry.itm_name;
                objStockInquiryDtltemp.loc_id = objStackInquiry.loc_id;
                objStockInquiryDtltemp.lot_id = objStackInquiry.lot_id;
                objStockInquiryDtltemp.palet_id = objStackInquiry.palet_id;
                objStockInquiryDtltemp.po_num = objStackInquiry.po_num;
                objStockInquiryDtltemp.TOT_CTN = objStackInquiry.TOT_CTN;
                objStockInquiryDtltemp.pkg_qty = objStackInquiry.pkg_qty;
                objStockInquiryDtltemp.whs_id = objStackInquiry.whs_id;
                objStockInquiryDtltemp.itm_qty = objStackInquiry.itm_qty;
                objStockInquiryDtltemp.tot_qty = objStackInquiry.tot_qty;
                objStockInquiryDtltemp.AlocCtn = objStackInquiry.AlocCtn;
                objStockInquiryDtltemp.Alocqty = objStackInquiry.Alocqty;
                objStockInquiryDtltemp.pkg_type = objStackInquiry.pkg_type;
                objStockInquiryDtltemp.kit_id = objStackInquiry.kit_id;
                li.Add(objStockInquiryDtltemp);
                lintCount++;
                Session["tempAlocQty"] = 0;
                // objStackInquiry = ServiceObject.GetStockInquiryGridDetails(objStackInquiry);
                AvlCtn = AvlCtn + l_str_stktot_ctn;
                AvlQty = AvlQty + l_str_stktot_qty;
                AlocQty = AlocQty + l_str_aloc_qty;
                Session["tempAlocQty"] = objStackInquiry.AlocQty;
            }
            objStackInquiry.AvlCtn = AvlCtn;
            objStackInquiry.AvlQty = AvlQty;
            objStackInquiry.AlocQty = AlocQty;
            objStackInquiry.ListStockInquiryGrid = li;
            Mapper.CreateMap<StockInquiryDtl, StockInquiryDtlModel>();
            StockInquiryDtlModel objStockInquiryModel = Mapper.Map<StockInquiryDtl, StockInquiryDtlModel>(objStackInquiry);
            return PartialView(objStockInquiryModel);
            //}

        }

        public ActionResult GetStockGridDetails(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_Status, string p_str_itm_num, string p_str_itm_color,
            string p_str_itm_size, string p_str_itm_name, string p_str_cont_id, string p_str_lot_id, string p_str_loc_id, string p_str_grn_id, string p_str_whs_id, string p_str_Date_fm, string p_str_Date_To, string p_str_po_num)
        {
            string l_str_stkcmp_id = string.Empty;
            string l_str_stkitm_num = string.Empty;
            string l_str_stkitm_color = string.Empty;
            string l_str_stkitm_size = string.Empty;
            string l_str_stkitm_desc = string.Empty;
            string l_str_stklot_id = string.Empty;
            string l_str_stkpalet_id = string.Empty;
            string l_str_stkcont_id = string.Empty;
            string l_str_stkwhs_id = string.Empty;
            string l_str_stkloc_id = string.Empty;
            string l_str_stkpkg_type = string.Empty;
            string l_str_stkpo_num = string.Empty;
            string l_str_stkibdoc_id = string.Empty;
            string l_str_stkrcvd_dt = string.Empty;
            int l_str_AsrtItm = 0;
            int l_str_stpkg_qty = 0;
            int l_str_stkitm_qty = 0;
            int l_str_stktot_ctn = 0;
            int l_str_stktot_qty = 0;
            int l_str_aloc_ctn = 0;
            int l_str_aloc_qty = 0;
            string l_str_aloccmp_id = string.Empty;
            string l_str_alocitm_num = string.Empty;
            string l_str_alocitm_color = string.Empty;
            string l_str_alocitm_size = string.Empty;
            string l_str_alocibdoc_id = string.Empty;
            string l_str_aloclot_id = string.Empty;
            string l_str_alocpalet_id = string.Empty;
            string l_str_aloccont_id = string.Empty;
            string l_str_alocloc_id = string.Empty;
            string l_str_alocpkg_type = string.Empty;
            string l_str_alocpo_num = string.Empty;
            string l_str_alocwhs = string.Empty;
            int l_str_alocitm_qty = 0;
            int l_str_alocpkg_qty = 0;
            int l_str_count1 = 0;
            int l_str_count2 = 0;
            dtStockInq = new DataTable();
            List<StockInquiryDtl> li = new List<StockInquiryDtl>();
            //2: Initialize a object of type DataRow
            DataRow drStk;

            //3: Initialize enough objects of type DataColumns
            DataColumn colCmpId = new DataColumn("cmp_id", typeof(string));
            DataColumn colib_doc_id = new DataColumn("ib_doc_id", typeof(string));
            DataColumn colrcvd_dt = new DataColumn("rcvd_dt", typeof(string));
            DataColumn colcont_id = new DataColumn("cont_id", typeof(string));
            DataColumn colitm_num = new DataColumn("itm_num", typeof(string));
            DataColumn colitm_color = new DataColumn("itm_color", typeof(string));
            DataColumn colitm_size = new DataColumn("itm_size", typeof(string));
            DataColumn colitm_name = new DataColumn("itm_name", typeof(string));
            DataColumn colloc_id = new DataColumn("loc_id", typeof(string));
            DataColumn collot_id = new DataColumn("lot_id", typeof(string));
            DataColumn colpalet_id = new DataColumn("palet_id", typeof(string));
            DataColumn colpo_num = new DataColumn("po_num", typeof(string));
            DataColumn colTOT_CTN = new DataColumn("TOT_CTN", typeof(int));
            DataColumn colpkg_qty = new DataColumn("pkg_qty", typeof(int));
            DataColumn colwhs_id = new DataColumn("whs_id", typeof(string));
            DataColumn colitm_qty = new DataColumn("itm_qty", typeof(int));
            DataColumn coltot_qty = new DataColumn("tot_qty", typeof(int));
            DataColumn colAlocCtn = new DataColumn("AlocCtn", typeof(int));
            DataColumn colAlocqty = new DataColumn("Alocqty", typeof(int));
            DataColumn colpkg_type = new DataColumn("pkg_type", typeof(string));
            DataColumn colkit_id = new DataColumn("kit_id", typeof(string));
            int lintCount = 0;

            //4: Adding DataColumns to DataTable dt
            dtStockInq.Columns.Add(colCmpId);
            dtStockInq.Columns.Add(colib_doc_id);
            dtStockInq.Columns.Add(colrcvd_dt);
            dtStockInq.Columns.Add(colcont_id);
            dtStockInq.Columns.Add(colitm_num);
            dtStockInq.Columns.Add(colitm_color);
            dtStockInq.Columns.Add(colitm_size);
            dtStockInq.Columns.Add(colitm_name);
            dtStockInq.Columns.Add(colloc_id);
            dtStockInq.Columns.Add(collot_id);
            dtStockInq.Columns.Add(colpalet_id);
            dtStockInq.Columns.Add(colpo_num);
            dtStockInq.Columns.Add(colTOT_CTN);
            dtStockInq.Columns.Add(colpkg_qty);
            dtStockInq.Columns.Add(colwhs_id);
            dtStockInq.Columns.Add(colitm_qty);
            dtStockInq.Columns.Add(coltot_qty);
            dtStockInq.Columns.Add(colAlocCtn);
            dtStockInq.Columns.Add(colAlocqty);
            dtStockInq.Columns.Add(colpkg_type);
            dtStockInq.Columns.Add(colkit_id);

            StockInquiryDtl objStackInquiry = new StockInquiryDtl();
            IStockInquiryService ServiceObject = new StockInquiryService();
            //ServiceObject.TruncateTempStockInquiry(objStackInquiry);
            objStackInquiry.cmp_id = p_str_cmp_id;
            objStackInquiry.is_company_user = Session["IsCompanyUser"].ToString().Trim();
            objStackInquiry.ib_doc_id = p_str_ib_doc_id;
            objStackInquiry.Status = p_str_Status.Trim();
            objStackInquiry.itm_num = p_str_itm_num.Trim();
            objStackInquiry.itm_color = p_str_itm_color;
            objStackInquiry.itm_size = p_str_itm_size;
            objStackInquiry.itm_name = p_str_itm_name;
            objStackInquiry.cont_id = p_str_cont_id;
            objStackInquiry.loc_id = p_str_loc_id;
            objStackInquiry.lot_id = p_str_lot_id;
            objStackInquiry.grn_id = p_str_grn_id;
            objStackInquiry.whs_id = p_str_whs_id;
            objStackInquiry.Date_fm = p_str_Date_fm;
            objStackInquiry.Date_to = p_str_Date_To;
            objStackInquiry.po_num = p_str_po_num;
            objStackInquiry = ServiceObject.GetStockInquiryDetails(objStackInquiry);
            objStackInquiry = ServiceObject.GetStockInquiryDetailAloc(objStackInquiry);
            l_str_count1 = objStackInquiry.ListStockInquiry.Count();
            l_str_count2 = objStackInquiry.LstStockInquirystock.Count();
            for (int i = 0; i < l_str_count1; i++)
            {
                l_str_stkitm_num = "";
                l_str_stkitm_color = "";
                l_str_stkitm_size = "";
                l_str_stklot_id = "";
                l_str_stkpalet_id = "";
                l_str_stkwhs_id = "";
                l_str_stpkg_qty = 0;
                l_str_stpkg_qty = 0;
                l_str_stktot_ctn = 0;
                l_str_stktot_qty = 0;
                l_str_aloc_ctn = 0;
                l_str_aloc_qty = 0;
                l_str_stkcmp_id = objStackInquiry.ListStockInquiry[i].cmp_id;
                l_str_AsrtItm = Convert.ToInt32(objStackInquiry.ListStockInquiry[i].kit_id);
                l_str_stkitm_num = objStackInquiry.ListStockInquiry[i].itm_num;
                l_str_stkitm_color = objStackInquiry.ListStockInquiry[i].itm_color;
                l_str_stkitm_size = objStackInquiry.ListStockInquiry[i].itm_size;
                l_str_stkitm_desc = objStackInquiry.ListStockInquiry[i].itm_name;
                l_str_stklot_id = objStackInquiry.ListStockInquiry[i].lot_id;
                l_str_stkpalet_id = objStackInquiry.ListStockInquiry[i].palet_id;
                l_str_stkcont_id = objStackInquiry.ListStockInquiry[i].cont_id;
                l_str_stkloc_id = objStackInquiry.ListStockInquiry[i].loc_id;
                l_str_stkwhs_id = objStackInquiry.ListStockInquiry[i].whs_id;
                l_str_stkpkg_type = objStackInquiry.ListStockInquiry[i].pkg_type;
                l_str_stkpo_num = objStackInquiry.ListStockInquiry[i].po_num;
                l_str_stkibdoc_id = objStackInquiry.ListStockInquiry[i].ib_doc_id;
                l_str_stpkg_qty = objStackInquiry.ListStockInquiry[i].pkg_qty;
                l_str_stkitm_qty = Convert.ToInt32(objStackInquiry.ListStockInquiry[i].itm_qty);
                l_str_stktot_qty = objStackInquiry.ListStockInquiry[i].AvlQty;
                l_str_stktot_ctn = objStackInquiry.ListStockInquiry[i].AvlCtn;
                l_str_stkrcvd_dt = objStackInquiry.ListStockInquiry[i].rcvd_dt;

                for (int j = 0; j < l_str_count2; j++)
                {
                    l_str_aloccmp_id = objStackInquiry.LstStockInquirystock[j].cmp_id;
                    l_str_alocitm_num = objStackInquiry.LstStockInquirystock[j].itm_num;
                    l_str_alocitm_color = objStackInquiry.LstStockInquirystock[j].itm_color;
                    l_str_alocitm_size = objStackInquiry.LstStockInquirystock[j].itm_size;
                    l_str_alocibdoc_id = objStackInquiry.LstStockInquirystock[j].ib_doc_id;
                    l_str_aloclot_id = objStackInquiry.LstStockInquirystock[j].lot_id;
                    l_str_alocpalet_id = objStackInquiry.LstStockInquirystock[j].palet_id;
                    l_str_aloccont_id = objStackInquiry.LstStockInquirystock[j].cont_id;
                    l_str_alocloc_id = objStackInquiry.LstStockInquirystock[j].loc_id;
                    l_str_alocpkg_type = objStackInquiry.LstStockInquirystock[j].pkg_type;
                    l_str_alocpo_num = objStackInquiry.LstStockInquirystock[j].po_num;
                    l_str_alocwhs = objStackInquiry.LstStockInquirystock[j].whs_id;
                    l_str_alocitm_qty = Convert.ToInt32(objStackInquiry.LstStockInquirystock[j].itm_qty);
                    l_str_alocpkg_qty = objStackInquiry.LstStockInquirystock[j].pkg_qty;

                    if (l_str_stkcmp_id == l_str_aloccmp_id && l_str_stkpalet_id == l_str_alocpalet_id && l_str_stkpo_num == l_str_alocpo_num && l_str_stklot_id == l_str_aloclot_id && l_str_stkwhs_id == l_str_alocwhs
                    && l_str_stpkg_qty == l_str_alocpkg_qty && l_str_stkitm_num == l_str_alocitm_num && l_str_stkitm_color == l_str_alocitm_color && l_str_stkitm_size == l_str_alocitm_size && l_str_stkloc_id == l_str_alocloc_id && l_str_stkpkg_type == l_str_alocpkg_type)
                    {
                        l_str_aloc_ctn = Convert.ToInt32(objStackInquiry.LstStockInquirystock[j].AlocCtn);
                        l_str_aloc_qty = objStackInquiry.LstStockInquirystock[j].AlocQty;
                        l_str_stktot_ctn = l_str_stktot_ctn - l_str_aloc_ctn;
                        l_str_stktot_qty = l_str_stktot_qty - l_str_aloc_qty;
                        break;
                    }

                }
                if (l_str_stktot_qty <= 0)
                {
                    continue;
                }
                objStackInquiry.cmp_id = l_str_stkcmp_id;
                objStackInquiry.ib_doc_id = l_str_stkibdoc_id;
                objStackInquiry.rcvd_dt = l_str_stkrcvd_dt;
                objStackInquiry.cont_id = l_str_stkcont_id;
                objStackInquiry.itm_num = l_str_stkitm_num;
                objStackInquiry.itm_color = l_str_stkitm_color;
                objStackInquiry.itm_size = l_str_stkitm_size;
                objStackInquiry.itm_name = l_str_stkitm_desc;
                objStackInquiry.loc_id = l_str_stkloc_id;
                objStackInquiry.lot_id = l_str_stklot_id;
                objStackInquiry.palet_id = l_str_stkpalet_id;
                objStackInquiry.po_num = l_str_stkpo_num;
                objStackInquiry.TOT_CTN = l_str_stktot_ctn;
                objStackInquiry.pkg_qty = l_str_stpkg_qty;
                objStackInquiry.whs_id = l_str_stkwhs_id;
                objStackInquiry.itm_qty = l_str_stkitm_qty;
                objStackInquiry.tot_qty = l_str_stktot_qty;
                objStackInquiry.AlocCtn = Convert.ToString(l_str_aloc_ctn);
                objStackInquiry.Alocqty = Convert.ToString(l_str_aloc_qty);
                objStackInquiry.pkg_type = l_str_stkpkg_type;
                objStackInquiry.kit_id = Convert.ToString(l_str_AsrtItm);
                //ServiceObject.InsertTempInventory(objStackInquiry);
                drStk = dtStockInq.NewRow();

                dtStockInq.Rows.Add(drStk);
                dtStockInq.Rows[lintCount][colCmpId] = objStackInquiry.cmp_id.ToString();
                dtStockInq.Rows[lintCount][colib_doc_id] = objStackInquiry.ib_doc_id.ToString();
                dtStockInq.Rows[lintCount][colrcvd_dt] = objStackInquiry.rcvd_dt.ToString();
                dtStockInq.Rows[lintCount][colcont_id] = objStackInquiry.cont_id.ToString();
                dtStockInq.Rows[lintCount][colitm_num] = objStackInquiry.itm_num.ToString();
                dtStockInq.Rows[lintCount][colitm_color] = objStackInquiry.itm_color.ToString();
                dtStockInq.Rows[lintCount][colitm_size] = objStackInquiry.itm_size.ToString();
                dtStockInq.Rows[lintCount][colitm_name] = objStackInquiry.itm_name.ToString();
                dtStockInq.Rows[lintCount][colloc_id] = objStackInquiry.loc_id.ToString();
                dtStockInq.Rows[lintCount][collot_id] = objStackInquiry.lot_id.ToString();
                dtStockInq.Rows[lintCount][colpalet_id] = objStackInquiry.palet_id.ToString();
                dtStockInq.Rows[lintCount][colpo_num] = objStackInquiry.po_num.ToString();
                dtStockInq.Rows[lintCount][colTOT_CTN] = objStackInquiry.TOT_CTN.ToString();
                dtStockInq.Rows[lintCount][colpkg_qty] = objStackInquiry.pkg_qty.ToString();
                dtStockInq.Rows[lintCount][colwhs_id] = objStackInquiry.whs_id.ToString();
                dtStockInq.Rows[lintCount][colitm_qty] = objStackInquiry.itm_qty.ToString();
                dtStockInq.Rows[lintCount][coltot_qty] = objStackInquiry.tot_qty.ToString();
                dtStockInq.Rows[lintCount][colAlocCtn] = objStackInquiry.AlocCtn.ToString();
                dtStockInq.Rows[lintCount][colAlocqty] = objStackInquiry.Alocqty.ToString();
                dtStockInq.Rows[lintCount][colpkg_type] = objStackInquiry.pkg_type.ToString();
                dtStockInq.Rows[lintCount][colkit_id] = objStackInquiry.kit_id.ToString();
                StockInquiryDtl objStockInquiryDtltemp = new StockInquiryDtl();
                objStockInquiryDtltemp.cmp_id = objStackInquiry.cmp_id;
                objStockInquiryDtltemp.ib_doc_id = objStackInquiry.ib_doc_id;
                objStockInquiryDtltemp.rcvd_dt = objStackInquiry.rcvd_dt;
                objStockInquiryDtltemp.cont_id = objStackInquiry.cont_id;
                objStockInquiryDtltemp.itm_num = objStackInquiry.itm_num;
                objStockInquiryDtltemp.itm_color = objStackInquiry.itm_color;
                objStockInquiryDtltemp.itm_size = objStackInquiry.itm_size;
                objStockInquiryDtltemp.itm_name = objStackInquiry.itm_name;
                objStockInquiryDtltemp.loc_id = objStackInquiry.loc_id;
                objStockInquiryDtltemp.lot_id = objStackInquiry.lot_id;
                objStockInquiryDtltemp.palet_id = objStackInquiry.palet_id;
                objStockInquiryDtltemp.po_num = objStackInquiry.po_num;
                objStockInquiryDtltemp.TOT_CTN = objStackInquiry.TOT_CTN;
                objStockInquiryDtltemp.pkg_qty = objStackInquiry.pkg_qty;
                objStockInquiryDtltemp.whs_id = objStackInquiry.whs_id;
                objStockInquiryDtltemp.itm_qty = objStackInquiry.itm_qty;
                objStockInquiryDtltemp.tot_qty = objStackInquiry.tot_qty;
                objStockInquiryDtltemp.AlocCtn = objStackInquiry.AlocCtn;
                objStockInquiryDtltemp.Alocqty = objStackInquiry.Alocqty;
                objStockInquiryDtltemp.pkg_type = objStackInquiry.pkg_type;
                objStockInquiryDtltemp.kit_id = objStackInquiry.kit_id;
                li.Add(objStockInquiryDtltemp);
                lintCount++;
                Session["tempAlocQty"] = 0;
                // objStackInquiry = ServiceObject.GetStockInquiryGridDetails(objStackInquiry);
                AvlCtn = AvlCtn + l_str_stktot_ctn;
                AvlQty = AvlQty + l_str_stktot_qty;
                AlocQty = AlocQty + l_str_aloc_qty;
                Session["tempAlocQty"] = objStackInquiry.AlocQty;
            }
            objStackInquiry.AvlCtn = AvlCtn;
            objStackInquiry.AvlQty = AvlQty;
            objStackInquiry.AlocQty = AlocQty;
            objStackInquiry.ListStockInquiryGrid = li;


            Mapper.CreateMap<StockInquiryDtl, StockInquiryDtlModel>();
            StockInquiryDtlModel objStockInquiryModel = Mapper.Map<StockInquiryDtl, StockInquiryDtlModel>(objStackInquiry);
            return PartialView("_StockInquiry", objStockInquiryModel);
            //}

        }
        public ActionResult GetAuditGridDetails(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_itm_num, string p_str_itm_color,
            string p_str_itm_size, string p_str_itm_name, string p_str_cont_id, string p_str_lot_id, string p_str_loc_id, string p_str_grn_id, string p_str_whs_id, string p_str_po_num)
        {

            StockInquiryDtl objStackInquiry = new StockInquiryDtl();
            IStockInquiryService ServiceObject = new StockInquiryService();
            objStackInquiry.cmp_id = p_str_cmp_id;
            objStackInquiry.ib_doc_id = p_str_ib_doc_id;
            //objStackInquiry.Status = p_str_Status;
            objStackInquiry.itm_num = p_str_itm_num.Trim();
            objStackInquiry.itm_color = p_str_itm_color;
            objStackInquiry.itm_size = p_str_itm_size;
            objStackInquiry.itm_name = p_str_itm_name;
            objStackInquiry.cont_id = p_str_cont_id;
            objStackInquiry.lot_id = p_str_lot_id;
            objStackInquiry.loc_id = p_str_loc_id;
            objStackInquiry.grn_id = p_str_grn_id;
            objStackInquiry.whs_id = p_str_whs_id;
            objStackInquiry.po_num = p_str_po_num.Trim();
            objStackInquiry = ServiceObject.GetAuditTrailDetails(objStackInquiry);
            string tranType = string.Empty;
            Total = 0;
            for (int i = 0; i < objStackInquiry.ListStockInquiryaudit.Count(); i++)
            {
                tranType = objStackInquiry.ListStockInquiryaudit[i].tran_type.Trim();
                tranType = tranType.Substring(tranType.Length - 4);
                if (tranType == "RCVD")
                {

                    l_int_rcvd_qty = l_int_rcvd_qty + objStackInquiry.ListStockInquiryaudit[i].Avail_qty;
                    l_int_rcvd_ctn = l_int_rcvd_ctn + objStackInquiry.ListStockInquiryaudit[i].Avail_cnt;
                    objStackInquiry.Rcvd_Qty = Convert.ToString(l_int_rcvd_qty);
                    objStackInquiry.Rcvd_Ctns = Convert.ToString(l_int_rcvd_ctn);
                    Total = Total + objStackInquiry.ListStockInquiryaudit[i].Avail_qty;
                }
                if (tranType == "ALOC")
                {

                    AlocTotalQty = AlocTotalQty + objStackInquiry.ListStockInquiryaudit[i].Avail_qty;
                    AlocTotalCtn = AlocTotalCtn + objStackInquiry.ListStockInquiryaudit[i].Avail_cnt;
                    objStackInquiry.Alocqty = Convert.ToString(AlocTotalQty);
                    objStackInquiry.AlocCtn = Convert.ToString(AlocTotalCtn);
                    Total = Total - objStackInquiry.ListStockInquiryaudit[i].Avail_qty;

                }
                if (tranType == "SHIP")
                {

                    ShipTotalQty = ShipTotalQty + objStackInquiry.ListStockInquiryaudit[i].Avail_qty;
                    ShipTotalCtn = ShipTotalCtn + objStackInquiry.ListStockInquiryaudit[i].Avail_cnt;
                    objStackInquiry.Ship_Qty = Convert.ToString(ShipTotalQty);
                    objStackInquiry.Ship_Ctns = Convert.ToString(ShipTotalCtn);
                    Total = Total - objStackInquiry.ListStockInquiryaudit[i].Avail_qty;
                }
                if (tranType == "SUNP")
                {

                    ShipTotalQty = ShipTotalQty - objStackInquiry.ListStockInquiryaudit[i].Avail_qty;
                    ShipTotalCtn = ShipTotalCtn - objStackInquiry.ListStockInquiryaudit[i].Avail_cnt;
                    objStackInquiry.Ship_Qty = Convert.ToString(ShipTotalQty * (-1)) ;
                    objStackInquiry.Ship_Ctns = Convert.ToString(ShipTotalCtn * (-1));
                    Total = Total + objStackInquiry.ListStockInquiryaudit[i].Avail_qty;
                }

                if (tranType == "ISA-")
                {
                    AdjTotalQtyMin = AdjTotalQtyMin + objStackInquiry.ListStockInquiryaudit[i].Avail_qty;
                    AdjTotalCtnMin = AdjTotalCtnMin + objStackInquiry.ListStockInquiryaudit[i].Avail_cnt;
                    objStackInquiry.Adj_Qty = Convert.ToString(AdjTotalQtyMin);
                    objStackInquiry.Adj_Ctns = Convert.ToString(AdjTotalCtnMin);
                    objStackInquiry.stk_adj_minus = AdjTotalQtyMin;
                    Total = Total - objStackInquiry.ListStockInquiryaudit[i].Avail_qty;
                }
            }
            objStackInquiry.AvlCtn = (l_int_rcvd_ctn   - ShipTotalCtn - AdjTotalCtnMin - AlocTotalCtn);
            objStackInquiry.AvlQty = (l_int_rcvd_qty  - ShipTotalQty - AdjTotalQtyMin - AlocTotalQty);
            Session["sess_str_doc_type"] = "OB";
            Mapper.CreateMap<StockInquiryDtl, StockInquiryDtlModel>();
            StockInquiryDtlModel objStockInquiryModel = Mapper.Map<StockInquiryDtl, StockInquiryDtlModel>(objStackInquiry);
            return PartialView("_AuditTrail", objStockInquiryModel);
        }
        public ActionResult EmailShowReport(string var_name, string p_str_cmp_id, string p_str_ib_doc_id, string p_str_itm_num, string p_str_itm_color,
           string p_str_itm_size, string p_str_itm_name, string p_str_cont_id, string p_str_lot_id, string p_str_loc_id, string p_str_grn_id, string p_str_whs_id, string p_str_Date_fm,
           string p_str_Date_To, string p_str_po_num, string lblalocqty, string type, string p_str_style_stearch)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            string l_str_rpt_selection1 = string.Empty;
            l_str_rpt_selection = var_name;
            string strDateFormat = string.Empty;  //CR2018 - 03 - 07 - 001 Added By Nithya
            string strFileName = string.Empty;
            string reportFileName = string.Empty;
            Folderpath = System.Configuration.ConfigurationManager.AppSettings["tempFilepath"].ToString().Trim();
            objCustMaster.cust_id = p_str_cmp_id;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }
            //l_str_rpt_selection1 = var_name1;
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "DetailbyStyle")
                    {
                        strReportName = "rpt_iv_stk_by_style_ea.rpt";
                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.Date_fm = p_str_Date_fm;
                        objStackInquiry.Date_to = p_str_Date_To;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.style_stearch = p_str_style_stearch;
                        objStackInquiry = ServiceObject.GetStockRptDetails(objStackInquiry);
                        EmailSub = "StockReport Detail by Style for" + " " + " " + objStackInquiry.cmp_id;
                        EmailMsg = "StockReport Detail by Style hasbeen Attached for the Process";
                        using (ReportDocument rd = new ReportDocument())
                        {
                            if (type == "PDF")
                            {

                                var rptSource = objStackInquiry.ListStockInquiryRpt.ToList();

                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objStackInquiry.ListStockInquiryRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                rd.SetParameterValue("AlocQty", AlocQty);
                                objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");//string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//StockReport_Detail by Style_" + strDateFormat + ".pdf";
                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                reportFileName = "StockReport Detail by Style " + DateTime.Now.ToFileTime() + ".pdf";
                                Session["RptFileName"] = strFileName;
                            }
                          
                            else if (type == "XLS")
                            {
                                //CR2018 - 03 - 08 - 001 Added By Nithya
                                var rptSource = objStackInquiry.ListStockInquiryRpt.ToList();
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objStackInquiry.ListStockInquiryRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                rd.SetParameterValue("AlocQty", AlocQty);
                                objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");//string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
                                strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//INV-DETAIL-BY-STYLE-" + DateTime.Now.ToString("yyyyMMddHHssmm") + ".xls";
                                rd.ExportToDisk(ExportFormatType.ExcelWorkbook, strFileName);
                                reportFileName = p_str_cmp_id.Trim() + "-INV-DETAIL-BY-STYLE-" + DateTime.Now.ToFileTime() + ".xls";
                                Session["RptFileName"] = strFileName;

                            }
                        }
                    }
                

                else if (l_str_rpt_selection == "SummarybyStyle")
                {
                    //strReportName = "rpt_iv_stk_by_style_ea_smry.rpt";
                    strReportName = "stock_inquiry_analysis_loc.rpt";
                    StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                    IStockInquiryService ServiceObject = new StockInquiryService();
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                    objStackInquiry.cmp_id = p_str_cmp_id;
                    objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                    objStackInquiry.itm_num = p_str_itm_num;
                    objStackInquiry.itm_color = p_str_itm_color;
                    objStackInquiry.itm_size = p_str_itm_size;
                    objStackInquiry.itm_name = p_str_itm_name;
                    objStackInquiry.cont_id = p_str_cont_id;
                    objStackInquiry.lot_id = p_str_lot_id;
                    objStackInquiry.loc_id = p_str_loc_id;
                    objStackInquiry.grn_id = p_str_grn_id;
                    objStackInquiry.whs_id = p_str_whs_id;
                    objStackInquiry.Date_fm = p_str_Date_fm;
                    objStackInquiry.Date_to = p_str_Date_To;
                    objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.style_stearch = p_str_style_stearch;
                        objStackInquiry = ServiceObject.GetStockLocSummRptDetails(objStackInquiry);
                    EmailSub = "StockReport Summary by Style for" + " " + " " + objStackInquiry.cmp_id;
                    EmailMsg = "StockReport Summary by Style hasbeen Attached for the Process";
                    using (ReportDocument rd = new ReportDocument())
                    {
                        if (type == "PDF")
                        {
                            var rptSource = objStackInquiry.ListStockLocRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objStackInquiry.ListStockLocRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                            rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//StockReport_Summary by Style_" + strDateFormat + ".pdf";
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            reportFileName = "StockReport Summary by Style" + DateTime.Now.ToFileTime() + ".pdf";
                            Session["RptFileName"] = strFileName;
                        }
                        else if (type == "XLS")
                        {
                            var rptSource = objStackInquiry.ListStockLocRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objStackInquiry.ListStockLocRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                            rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                            //rd.SetParameterValue("AlocQty", AlocQty);
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//StockReport_Summary by Style_" + strDateFormat + ".xls";
                            rd.ExportToDisk(ExportFormatType.ExcelWorkbook, strFileName);
                            reportFileName = "StockReport Summary by Style" + DateTime.Now.ToFileTime() + ".xls";
                            Session["RptFileName"] = strFileName;
                        }
                    }
                }
               else if (l_str_rpt_selection == "DetailbyLoc")
                {
                    strReportName = "rpt_iv_stk_by_loc_ea.rpt";
                    StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                    IStockInquiryService ServiceObject = new StockInquiryService();

                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                    objStackInquiry.cmp_id = p_str_cmp_id;
                    objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                    objStackInquiry.itm_num = p_str_itm_num;
                    objStackInquiry.itm_color = p_str_itm_color;
                    objStackInquiry.itm_size = p_str_itm_size;
                    objStackInquiry.itm_name = p_str_itm_name;
                    objStackInquiry.cont_id = p_str_cont_id;
                    objStackInquiry.lot_id = p_str_lot_id;
                    objStackInquiry.loc_id = p_str_loc_id;
                    objStackInquiry.grn_id = p_str_grn_id;
                    objStackInquiry.whs_id = p_str_whs_id;
                    objStackInquiry.Date_fm = p_str_Date_fm;
                    objStackInquiry.Date_to = p_str_Date_To;
                    objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.style_stearch = p_str_style_stearch;
                        objStackInquiry = ServiceObject.GetStockLocRptDetails(objStackInquiry);
                    EmailSub = "StockReport Detail by Location for" + " " + " " + objStackInquiry.cmp_id;
                    EmailMsg = "StockReport Detail by Location hasbeen Attached for the Process";
                    using (ReportDocument rd = new ReportDocument())
                    {
                        if (type == "PDF")
                        {
                            var rptSource = objStackInquiry.ListStockLocRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objStackInquiry.ListStockLocRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                            rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                            rd.SetParameterValue("AlocQty", AlocQty);
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//StockReport_Detail by Loc_" + strDateFormat + ".pdf";
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            reportFileName = "StockReport Detail by Location" + DateTime.Now.ToFileTime() + ".pdf";
                            Session["RptFileName"] = strFileName;
                        }
                      
                        else  if (type == "XLS")
                        {
                            var rptSource = objStackInquiry.ListStockLocRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objStackInquiry.ListStockLocRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                            rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                            rd.SetParameterValue("AlocQty", AlocQty);
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//StockReport_Detail by Loc_" + strDateFormat + ".xls";
                            rd.ExportToDisk(ExportFormatType.ExcelWorkbook, strFileName);
                            reportFileName = "StockReport Detail by Location" + DateTime.Now.ToFileTime() + ".xls";
                            Session["RptFileName"] = strFileName;
                        }
                    }
                }
               else if (l_str_rpt_selection == "SummarybyLoc")
                {

                    //strReportName = "rpt_iv_stk_by_style_ea_smry.rpt";
                    strReportName = "stock_inquiry_analysis_loc.rpt";
                    StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                    IStockInquiryService ServiceObject = new StockInquiryService();

                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                    objStackInquiry.cmp_id = p_str_cmp_id;
                    objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                    objStackInquiry.itm_num = p_str_itm_num;
                    objStackInquiry.itm_color = p_str_itm_color;
                    objStackInquiry.itm_size = p_str_itm_size;
                    objStackInquiry.itm_name = p_str_itm_name;
                    objStackInquiry.cont_id = p_str_cont_id;
                    objStackInquiry.lot_id = p_str_lot_id;
                    objStackInquiry.loc_id = p_str_loc_id;
                    objStackInquiry.grn_id = p_str_grn_id;
                    objStackInquiry.whs_id = p_str_whs_id;
                    objStackInquiry.Date_fm = p_str_Date_fm;
                    objStackInquiry.Date_to = p_str_Date_To;
                    objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.style_stearch = p_str_style_stearch;
                        objStackInquiry = ServiceObject.GetStockLocSummRptDetails(objStackInquiry);
                    EmailSub = "StockReport Summary by Location for" + " " + " " + objStackInquiry.cmp_id;
                    EmailMsg = "StockReport Summary by Location hasbeen Attached for the Process";
                    using (ReportDocument rd = new ReportDocument())
                    {
                        if (type == "PDF")
                        {
                            var rptSource = objStackInquiry.ListStockLocRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objStackInquiry.ListStockLocRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                            rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//StockReport_Summary by Loc_" + strDateFormat + ".pdf";
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            reportFileName = "StockReport Summary by Location" + DateTime.Now.ToFileTime() + ".pdf";
                            Session["RptFileName"] = strFileName;
                        }
                        
                        else  if (type == "XLS")
                        {
                            var rptSource = objStackInquiry.ListStockLocRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objStackInquiry.ListStockLocRpt.Count();
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                            rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//StockReport_Summary by Loc_" + DateTime.Now.ToString("yyyyMMddHHssmm") + ".xls";
                            rd.ExportToDisk(ExportFormatType.ExcelWorkbook, strFileName);
                            reportFileName = "StockReport Summary by Location" + DateTime.Now.ToFileTime() + ".xls";
                            Session["RptFileName"] = strFileName;

                        }

                    }
                }
                else
                {
                    Response.Write("<H2>Report not found</H2>");
                }
                Email objEmail = new Email();
                objEmail.CmpId = p_str_cmp_id;
                objEmail.EmailSubject = EmailSub;
                objEmail.EmailMessage = EmailMsg;
                EmailService objEmailService = new EmailService();
                objEmail = objEmailService.GetSendMailDetails(objEmail);
                if (objEmail.ListEamilDetail.Count != 0)
                {

                    objEmail.Attachment = reportFileName;
                    objEmail.EmailTo = (objEmail.ListEamilDetail[0].EmailTo.Trim() == null || objEmail.ListEamilDetail[0].EmailTo.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailTo.Trim();
                    objEmail.EmailCC = (objEmail.ListEamilDetail[0].EmailCC.Trim() == null || objEmail.ListEamilDetail[0].EmailCC.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailCC.Trim();

                }
                else
                {
                    objEmail.Attachment = reportFileName;
                    objEmail.EmailTo = "";
                    objEmail.EmailCC = "";
                }
                Mapper.CreateMap<Email, EmailModel>();
                EmailModel EmailModel = Mapper.Map<Email, EmailModel>(objEmail);
                return PartialView("_Email", EmailModel);
            }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                jsonErrorCode = "-2";
            }

            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ShowGridExcelReport(string p_str_grid_name, string p_str_cmp_id)
        {
           return new DownloadFileActionResult((GridView)Session[p_str_grid_name], p_str_grid_name + "" + DateTime.Now.ToString("yyyyMMddHHssmm") + ".xls");
        }
        public ActionResult ShowExcelReport( string p_str_file_name,string p_str_cmp_id)
        {
            string strDateFormat = string.Empty;
            FileStream fs = new FileStream(p_str_file_name, FileMode.Open, FileAccess.Read);
            string l_str_down_load_file_name = string.Empty;
            strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
            l_str_down_load_file_name = p_str_cmp_id + "-" + "IV-" +  strDateFormat + ".xlsx";
            
            return File(fs, "application / xlsx", l_str_down_load_file_name);
        }

        public ActionResult ShowPDFReport(string p_str_file_name, string p_str_cmp_id)
        {
            FileStream fs = new FileStream(p_str_file_name, FileMode.Open, FileAccess.Read);
            return File(fs, "application/pdf");
        }

        public ActionResult ShowReport(string var_name, string p_str_cmp_id, string p_str_ib_doc_id, string p_str_itm_num, string p_str_itm_color,
            string p_str_itm_size, string p_str_itm_name, string p_str_cont_id, string p_str_lot_id, string p_str_loc_id, string p_str_grn_id, string p_str_whs_id,
            string p_str_Date_fm, string p_str_Date_To, string p_str_po_num, string lblalocqty, string type, string p_str_style_stearch)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            string l_str_rpt_selection1 = string.Empty;


            //Added By Buddhika
            string strDateFormat = string.Empty;
            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
            //////
            objCustMaster.cust_id = p_str_cmp_id;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }
            l_str_rpt_selection = var_name;
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "DetailbyStyle")
                    {
                        strReportName = "rpt_iv_stk_by_style_ea.rpt";
                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.Date_fm = p_str_Date_fm;
                        objStackInquiry.Date_to = p_str_Date_To;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.style_stearch = p_str_style_stearch;


                        if (Session["tempAlocQty"] != null)
                        {
                            objStackInquiry.AlocQty = Convert.ToInt32(Session["tempAlocQty"].ToString().Trim());
                        }



                        if (type == "PDF")
                        {
                            objStackInquiry = ServiceObject.GetStockRptDetails(objStackInquiry);
                            var rptSource = objStackInquiry.ListStockInquiryRpt.ToList();

                            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                            string tempFileName = string.Empty;
                            string l_str_file_name = string.Empty;

                            if (!Directory.Exists(strOutputpath))
                            {
                                Directory.CreateDirectory(strOutputpath);
                            }
                            l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-IV-STK-BY-STYLE" + strDateFormat + ".pdf";
                            tempFileName = strOutputpath + l_str_file_name;

                            if (System.IO.File.Exists(tempFileName))
                                System.IO.File.Delete(tempFileName);

                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objStackInquiry.ListStockInquiryRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                    rd.SetParameterValue("AlocQty", objStackInquiry.AlocQty);
                                    rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                    
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, tempFileName);
                                    return Json(tempFileName, JsonRequestBehavior.AllowGet);

                                    
                                }
                            }
                        }


                        else
                        if (type == "Excel")
                        {
 
                            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                           // strDateFormat = string.Concat(DateTime.Now.Year, "-", DateTime.Now.ToString("MM"), "-", DateTime.Now.ToString("dd"));
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            string tempFileName = string.Empty;
                            string l_str_file_name = string.Empty;
                            DataTable dtBill = new DataTable();

                            objStackInquiry = ServiceObject.GetStockInquiryStyleDtlExcel(objStackInquiry);

                            if (!Directory.Exists(strOutputpath))
                            {
                                Directory.CreateDirectory(strOutputpath);
                            }

                            l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() +  "-IV-STYLE-DTL-" + strDateFormat + ".xlsx";

                            tempFileName = strOutputpath + l_str_file_name;

                            if (System.IO.File.Exists(tempFileName))
                                System.IO.File.Delete(tempFileName);
                            xls_IV_Style_Dtl_Excel mxcel1 = new xls_IV_Style_Dtl_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IV_Style_Dtl_Excel.xlsx");

                            mxcel1.PopulateHeader_IV_Style_Dtl_Excel(p_str_cmp_id);
                            mxcel1.PopulateData_IV_Style_Dtl_Excel(objStackInquiry.LstStockInquiryStyleDtl, true);
                            mxcel1.SaveAs(tempFileName);
                            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                            return Json(tempFileName, JsonRequestBehavior.AllowGet);

                            //return File(fs, "application / xlsx", l_str_file_name);


                        }

                    }
                }
                if (l_str_rpt_selection == "SummarybyStyle")
                {
                    strReportName = "stock_inquiry_analysis.rpt";
                    StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                    IStockInquiryService ServiceObject = new StockInquiryService();
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                    objStackInquiry.cmp_id = p_str_cmp_id;
                    objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                    objStackInquiry.itm_num = p_str_itm_num;
                    objStackInquiry.itm_color = p_str_itm_color;
                    objStackInquiry.itm_size = p_str_itm_size;
                    objStackInquiry.itm_name = p_str_itm_name;
                    objStackInquiry.cont_id = p_str_cont_id;
                    objStackInquiry.lot_id = p_str_lot_id;
                    objStackInquiry.loc_id = p_str_loc_id;
                    objStackInquiry.grn_id = p_str_grn_id;
                    objStackInquiry.whs_id = p_str_whs_id;
                    objStackInquiry.Date_fm = p_str_Date_fm;
                    objStackInquiry.Date_to = p_str_Date_To;
                    objStackInquiry.po_num = p_str_po_num;
                    objStackInquiry.style_stearch = p_str_style_stearch;
                    objStackInquiry.AlocQty = Convert.ToInt32(Session["tempAlocQty"].ToString().Trim());

                    objStackInquiry = ServiceObject.GetStockStyleSummRptDetails(objStackInquiry);

                    if (type == "PDF")
                    {
                        var rptSource = objStackInquiry.ListStockLocRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objStackInquiry.ListStockLocRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                                string tempFileName = string.Empty;
                                string l_str_file_name = string.Empty;

                                if (!Directory.Exists(strOutputpath))
                                {
                                    Directory.CreateDirectory(strOutputpath);
                                }
                                l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-IV-STK-SMRY-BY-STYLE" + strDateFormat + ".pdf";
                                tempFileName = strOutputpath + l_str_file_name;

                                if (System.IO.File.Exists(tempFileName))
                                    System.IO.File.Delete(tempFileName);


                                //rd.SetParameterValue("AlocQty", objStackInquiry.AlocQty); CR2018 - 03 - 10 - 001 Commented By Nithya
                                //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                rd.ExportToDisk(ExportFormatType.PortableDocFormat, tempFileName);
                                return Json(tempFileName, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }

                    else
                    if (type == "Excel")
                    {

                        List<StockInquiryStyleSmmryExcel> li = new List<StockInquiryStyleSmmryExcel>();
                        for (int i = 0; i < objStackInquiry.ListStockLocRpt.Count; i++)
                        {

                            StockInquiryStyleSmmryExcel objStackInquiryExcel = new StockInquiryStyleSmmryExcel();
                            objStackInquiryExcel.Style = objStackInquiry.ListStockLocRpt[i].Style;
                            objStackInquiryExcel.Color = objStackInquiry.ListStockLocRpt[i].Color;
                            objStackInquiryExcel.Size = objStackInquiry.ListStockLocRpt[i].Size;
                            objStackInquiryExcel.Description = objStackInquiry.ListStockLocRpt[i].Description;
                            objStackInquiryExcel.WIP = objStackInquiry.ListStockLocRpt[i].WIP_QTY;
                            objStackInquiryExcel.STOCK = objStackInquiry.ListStockLocRpt[i].STK_QTY;
                            objStackInquiryExcel.ALLOCATED = objStackInquiry.ListStockLocRpt[i].ALLOC_QTY;
                            objStackInquiryExcel.AVAILABLE = objStackInquiry.ListStockLocRpt[i].AVAIL_QTY;
                            objStackInquiryExcel.OPEN_ORDER_QTY = objStackInquiry.ListStockLocRpt[i].OPEN_ORDER_QTY;
                            // objStackInquiryExcel.OPEN_TO_SELL_QTY = objStackInquiry.ListStockLocRpt[i].OPEN_TO_SELL_QTY;

                            li.Add(objStackInquiryExcel);
                        }

                        GridView gv = new GridView();
                        gv.DataSource = li;
                        gv.DataBind();
                        Session["IV_STYLE_SMRY"] = gv;
                        return Json("IV_STYLE_SMRY", JsonRequestBehavior.AllowGet);
                        //return new DownloadFileActionResult((GridView)Session["IV_STYLE_SMRY"], "IV_STYLE_SMRY" + "" + DateTime.Now.ToString("yyyyMMddHHssmm") + ".xls");



                    }
                }
                if (l_str_rpt_selection == "DetailbyLoc")
                {
                    strReportName = "rpt_iv_stk_by_loc_ea.rpt";
                    StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                    IStockInquiryService ServiceObject = new StockInquiryService();
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                    objStackInquiry.cmp_id = p_str_cmp_id;
                    objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                    objStackInquiry.itm_num = p_str_itm_num;
                    objStackInquiry.itm_color = p_str_itm_color;
                    objStackInquiry.itm_size = p_str_itm_size;
                    objStackInquiry.itm_name = p_str_itm_name;
                    objStackInquiry.cont_id = p_str_cont_id;
                    objStackInquiry.lot_id = p_str_lot_id;
                    objStackInquiry.loc_id = p_str_loc_id;
                    objStackInquiry.grn_id = p_str_grn_id;
                    objStackInquiry.whs_id = p_str_whs_id;
                    objStackInquiry.Date_fm = p_str_Date_fm;
                    objStackInquiry.Date_to = p_str_Date_To;
                    objStackInquiry.po_num = p_str_po_num;
                    objStackInquiry.style_stearch = p_str_style_stearch;
                    if (Session["tempAlocQty"] != null)
                    {
                        objStackInquiry.AlocQty = Convert.ToInt32(Session["tempAlocQty"].ToString().Trim());
                    }


                    if (type == "PDF")
                    {
                        objStackInquiry = ServiceObject.GetStockLocRptDetails(objStackInquiry);
                        var rptSource = objStackInquiry.ListStockLocRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objStackInquiry.ListStockLocRpt.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                rd.SetParameterValue("AlocQty", objStackInquiry.AlocQty);
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            }
                        }
                    }

                    else
                    if (type == "Excel")
                    {

                      
                        string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                        strDateFormat = string.Concat(DateTime.Now.Year, "-", DateTime.Now.ToString("MM"), "-", DateTime.Now.ToString("dd"));
                        string tempFileName = string.Empty;
                        string l_str_file_name = string.Empty;
                        DataTable dtBill = new DataTable();

                        objStackInquiry = ServiceObject.GetStockInquiryLocDtlExcel(objStackInquiry);

                        if (!Directory.Exists(strOutputpath))
                        {
                            Directory.CreateDirectory(strOutputpath);
                        }

                        l_str_file_name =  p_str_cmp_id.ToUpper().ToString().Trim() + "-IV-STK-DTL-BY-LOC-" + strDateFormat + ".xlsx";

                        tempFileName = strOutputpath + l_str_file_name;

                        if (System.IO.File.Exists(tempFileName))
                            System.IO.File.Delete(tempFileName);
                        xls_IV_Stk_Detail_By_Loc mxcel1 = new xls_IV_Stk_Detail_By_Loc(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IV_Stk_Detail_By_Loc.xlsx");

                        mxcel1.PopulateHeader_IV_Stk_Detail_By_Loc_Excel(p_str_cmp_id);
                        mxcel1.PopulateData_IV_Stk_Detail_By_Loc_Excel(objStackInquiry.LstStockInquiryLocDtl, true);
                        mxcel1.SaveAs(tempFileName);
                        FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                       // return File(fs, "application / xlsx", l_str_file_name);
                        return Json(tempFileName, JsonRequestBehavior.AllowGet);





                    }
                }
                if (l_str_rpt_selection == "SummarybyLoc")
                {
                    //strReportName = "rpt_iv_stk_by_style_ea_smry.rpt";
                    strReportName = "stock_inquiry_analysis_loc.rpt";
                    StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                    IStockInquiryService ServiceObject = new StockInquiryService();

                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                    objStackInquiry.cmp_id = p_str_cmp_id;
                    objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                    objStackInquiry.itm_num = p_str_itm_num;
                    objStackInquiry.itm_color = p_str_itm_color;
                    objStackInquiry.itm_size = p_str_itm_size;
                    objStackInquiry.itm_name = p_str_itm_name;
                    objStackInquiry.cont_id = p_str_cont_id;
                    objStackInquiry.lot_id = p_str_lot_id;
                    objStackInquiry.loc_id = p_str_loc_id;
                    objStackInquiry.grn_id = p_str_grn_id;
                    objStackInquiry.whs_id = p_str_whs_id;
                    objStackInquiry.Date_fm = p_str_Date_fm;
                    objStackInquiry.Date_to = p_str_Date_To;
                    objStackInquiry.po_num = p_str_po_num;
                    objStackInquiry.style_stearch = p_str_style_stearch;
                    objStackInquiry.AlocQty = Convert.ToInt32(Session["tempAlocQty"].ToString().Trim());
                    //objStackInquiry = ServiceObject.GetStockLocSummRptDetails(objStackInquiry);
                    objStackInquiry = ServiceObject.GetStockInquiryLocSmryExcel(objStackInquiry);
                    if (type == "PDF")
                    {
                        var rptSource = objStackInquiry.LstStockInquiryLocSmry.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                                int AlocCount = 0;
                                AlocCount = objStackInquiry.LstStockInquiryLocSmry.Count();
                                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource);
                                objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                //rd.SetParameterValue("AlocQty", objStackInquiry.AlocQty);
                                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                            }
                        }
                    }

                    else
                    if (type == "Excel")
                    {


                        List<StockInquiryLocDtlBySummaryExcel> li = new List<StockInquiryLocDtlBySummaryExcel>();
                        for (int i = 0; i < objStackInquiry.LstStockInquiryLocSmry.Count; i++)
                        {

                            StockInquiryLocDtlBySummaryExcel objStackInquiryExcel = new StockInquiryLocDtlBySummaryExcel();
                            objStackInquiryExcel.Style = objStackInquiry.LstStockInquiryLocSmry[i].Style;
                            objStackInquiryExcel.Color = objStackInquiry.LstStockInquiryLocSmry[i].Color;
                            objStackInquiryExcel.Size = objStackInquiry.LstStockInquiryLocSmry[i].Size;
                            objStackInquiryExcel.Description = objStackInquiry.LstStockInquiryLocSmry[i].Description;
                            objStackInquiryExcel.WIP = objStackInquiry.LstStockInquiryLocSmry[i].WIP_QTY;
                            objStackInquiryExcel.STOCK = objStackInquiry.LstStockInquiryLocSmry[i].STK_QTY;
                            objStackInquiryExcel.ALLOCATED = objStackInquiry.LstStockInquiryLocSmry[i].ALLOC_QTY;
                            objStackInquiryExcel.AVAILABLE = objStackInquiry.LstStockInquiryLocSmry[i].AVAIL_QTY;
                            li.Add(objStackInquiryExcel);
                        }
                        GridView gv = new GridView();
                        gv.DataSource = li;
                        gv.DataBind();
                        Session["IV_LOC_DTL"] = gv;
                        return Json("IV_LOC_DTL", JsonRequestBehavior.AllowGet);
                        // return new DownloadFileActionResult((GridView)Session["IV_LOC_DTL"], "IV_LOC_DTL" + "" + DateTime.Now.ToString("yyyyMMddHHssmm") + ".xls");
                    }
                }
                else
                {
                    Response.Write("<H2>Report not found</H2>");
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                jsonErrorCode = "-2";
            }

            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);

        }


        public ActionResult EmailshowreportAudit(string var_name, string p_str_cmp_id, string p_str_ib_doc_id, string p_str_itm_num, string p_str_itm_color,
  string p_str_itm_size, string p_str_itm_name, string p_str_cont_id, string p_str_lot_id, string p_str_loc_id, string p_str_grn_id, string p_str_whs_id, string p_str_po_num, string type)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            string l_str_rpt_selection1 = string.Empty;
            l_str_rpt_selection = var_name;
            string strDateFormat = string.Empty;
            string strFileName = string.Empty;
            string reportFileName = string.Empty; //CR2018 - 03 - 07 - 001 Added By Nithya
            Folderpath = System.Configuration.ConfigurationManager.AppSettings["tempFilepath"].ToString().Trim();
            objCustMaster.cust_id = p_str_cmp_id;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }
            //l_str_rpt_selection1 = var_name1;
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "DetailWithoutISM")
                    {
                        strReportName = "rpt_iv_stk_aud_ea_not.rpt";
                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.radio = l_str_rpt_selection;//CR2018-03-08-001 Added By Nithya
                        objStackInquiry = ServiceObject.GetAudRptDetails(objStackInquiry);
                        var rptSource = objStackInquiry.ListAudRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                if (type == "PDF")
                                {

                                    rd.Load(strRptPath);

                                    rd.SetDataSource(rptSource);
                                    var rpt_title = "AUDIT  TRAIL  REPORT (Detail)";
                                    var rpt_title1 = "- Without ISM";
                                    var rpttitle = rpt_title + rpt_title1;
                                    objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                    rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                    rd.SetParameterValue("fml_rep_title", rpttitle);
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//IV_STOCK_INQ" + strDateFormat + ".pdf";

                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                    reportFileName = "IV_STK_INQ_" + DateTime.Now.ToFileTime() + ".pdf";
                                    Session["RptFileName"] = strFileName;
                                    //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }

                                else
                        if (type == "XLS")
                                {
                                    rd.Load(strRptPath);

                                    rd.SetDataSource(rptSource);
                                    var rpt_title = "AUDIT  TRAIL  REPORT (Detail)";
                                    var rpt_title1 = "- Without ISM";
                                    var rpttitle = rpt_title + rpt_title1;
                                    rd.SetParameterValue("fml_rep_title", rpttitle);
                                    objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                    rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//IV_STOCK_INQ" + strDateFormat + ".xls";

                                    rd.ExportToDisk(ExportFormatType.ExcelWorkbook, strFileName);
                                    reportFileName = "IV_STK_INQ_" + DateTime.Now.ToFileTime() + ".xls";
                                    Session["RptFileName"] = strFileName;
                                    return Json(strFileName, JsonRequestBehavior.AllowGet);

                                }
                            }
                        }

                    }
                    if (l_str_rpt_selection == "DetailWithISM")
                    {
                        strReportName = "rpt_iv_stk_aud_ea_not.rpt";
                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.radio = l_str_rpt_selection;//CR2018-03-08-001 Added By Nithya
                        objStackInquiry = ServiceObject.GetAudRptDetails(objStackInquiry);
                        var rptSource = objStackInquiry.ListAudRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                if (type == "PDF")
                                {

                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objStackInquiry.ListAudRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    var rpt_title = "AUDIT  TRAIL  REPORT (Detail)";
                                    var rpt_title1 = "-With ISM";
                                    var rpttitle = rpt_title + rpt_title1;
                                    rd.SetParameterValue("fml_rep_title", rpttitle);
                                    objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                    rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//IV_STOCK_INQ" + strDateFormat + ".pdf";
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                    reportFileName = "IV_STK_INQ_" + DateTime.Now.ToFileTime() + ".pdf";
                                    Session["RptFileName"] = strFileName;
                                    //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }

                                else
                        if (type == "XLS")
                                {

                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objStackInquiry.ListAudRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    var rpt_title = "AUDIT  TRAIL  REPORT (Detail)";
                                    var rpt_title1 = "- Without ISM";
                                    var rpttitle = rpt_title + rpt_title1;
                                    rd.SetParameterValue("fml_rep_title", rpttitle);
                                    objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                    rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//IV_STOCK_INQ" + strDateFormat + ".xls";

                                    rd.ExportToDisk(ExportFormatType.ExcelWorkbook, strFileName);
                                    reportFileName = "IV_STK_INQ_" + DateTime.Now.ToFileTime() + ".xls";
                                    Session["RptFileName"] = strFileName;

                                }
                            }
                        }

                    }
                    if (l_str_rpt_selection == "DetailOnlyISM")
                    {
                        strReportName = "rpt_iv_stk_aud_ea_not.rpt";
                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.radio = l_str_rpt_selection;//CR2018-03-08-001 Added By Nithya
                        objStackInquiry = ServiceObject.GetAudRptDetails(objStackInquiry);
                        var rptSource = objStackInquiry.ListAudRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                if (type == "PDF")
                                {

                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objStackInquiry.ListAudRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    var rpt_title = "AUDIT  TRAIL  REPORT (Detail)";
                                    var rpt_title1 = "- ISM Only";
                                    var rpttitle = rpt_title + rpt_title1;
                                    rd.SetParameterValue("fml_rep_title", rpttitle);
                                    objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                    rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//IV_STOCK_INQ" + strDateFormat + ".pdf";

                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                    reportFileName = "IV_STK_INQ_" + DateTime.Now.ToFileTime() + ".pdf";
                                    Session["RptFileName"] = strFileName;
                                    //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }

                                else
                        if (type == "XLS")
                                {

                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objStackInquiry.ListAudRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    var rpt_title = "AUDIT  TRAIL  REPORT (Detail)";
                                    var rpt_title1 = "- Without ISM";
                                    var rpttitle = rpt_title + rpt_title1;
                                    rd.SetParameterValue("fml_rep_title", rpttitle);
                                    objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                    rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//IV_STOCK_INQ" + strDateFormat + ".xls";

                                    rd.ExportToDisk(ExportFormatType.ExcelWorkbook, strFileName);
                                    reportFileName = "IV_STK_INQ_" + DateTime.Now.ToFileTime() + ".xls";
                                    Session["RptFileName"] = strFileName;
                                }
                            }

                        }
                    }
                    if (l_str_rpt_selection == "SummaryWithoutISM")
                    {
                        strReportName = "rpt_iv_stk_aud_ea_smry.rpt";
                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.radio = l_str_rpt_selection;//CR2018-03-08-001 Added By Nithya
                        objStackInquiry = ServiceObject.GetAudRptSummary(objStackInquiry);
                        var rptSource = objStackInquiry.ListAudRpt.ToList();
                        if (type == "PDF")
                        {
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);

                                    rd.SetDataSource(rptSource);
                                    var rpt_title = "AUDIT  TRAIL  REPORT  (Summary)";
                                    var rpt_title1 = "-Without ISM";
                                    var rpttitle = rpt_title + rpt_title1;
                                    rd.SetParameterValue("fml_rep_title", rpttitle);
                                    objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                    rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//IV_STOCK_INQ" + strDateFormat + ".pdf";

                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                }
                            }
                            reportFileName = "IV_STK_INQ_" + DateTime.Now.ToFileTime() + ".pdf";
                            Session["RptFileName"] = strFileName;
                            //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }

                        else
                        if (type == "XLS")
                        {
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objStackInquiry.ListAudRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    var rpt_title = "AUDIT  TRAIL  REPORT (Detail)";
                                    var rpt_title1 = "- Without ISM";
                                    var rpttitle = rpt_title + rpt_title1;
                                    rd.SetParameterValue("fml_rep_title", rpttitle);
                                    objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                    rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//IV_STOCK_INQ" + strDateFormat + ".xls";

                                    rd.ExportToDisk(ExportFormatType.ExcelWorkbook, strFileName);
                                }
                            }
                            reportFileName = "IV_STK_INQ_" + DateTime.Now.ToFileTime() + ".xls";
                            Session["RptFileName"] = strFileName;




                        }

                    }
                    if (l_str_rpt_selection == "SummaryWithISM")
                    {
                        strReportName = "rpt_iv_stk_aud_ea_smry.rpt";
                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.radio = l_str_rpt_selection;//CR2018-03-08-001 Added By Nithya
                        objStackInquiry = ServiceObject.GetAudRptSummary(objStackInquiry);

                        var rptSource = objStackInquiry.ListAudRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                if (type == "PDF")
                                {

                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objStackInquiry.ListAudRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    var rpt_title = "AUDIT  TRAIL  REPORT  (Summary)";
                                    var rpt_title1 = "-With ISM";
                                    var rpttitle = rpt_title + rpt_title1;
                                    rd.SetParameterValue("fml_rep_title", rpttitle);
                                    objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                    rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//IV_STOCK_INQ" + strDateFormat + ".pdf";

                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                    reportFileName = "IV_STK_INQ_" + DateTime.Now.ToFileTime() + ".pdf";
                                    Session["RptFileName"] = strFileName;
                                    //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }

                                else
                        if (type == "XLS")
                                {

                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objStackInquiry.ListAudRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    var rpt_title = "AUDIT  TRAIL  REPORT (Detail)";
                                    var rpt_title1 = "- Without ISM";
                                    var rpttitle = rpt_title + rpt_title1;
                                    rd.SetParameterValue("fml_rep_title", rpttitle);
                                    objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                    rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//IV_STOCK_INQ" + strDateFormat + ".xls";

                                    rd.ExportToDisk(ExportFormatType.ExcelWorkbook, strFileName);
                                    reportFileName = "IV_STK_INQ_" + DateTime.Now.ToFileTime() + ".xls";
                                    Session["RptFileName"] = strFileName;

                                }
                            }
                        }

                    }
                    if (l_str_rpt_selection == "SummaryonlyISM")
                    {
                        strReportName = "rpt_iv_stk_aud_ea_smry.rpt";
                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.radio = l_str_rpt_selection;//CR2018-03-08-001 Added By Nithya
                        objStackInquiry = ServiceObject.GetAudRptSummary(objStackInquiry);
                        var rptSource = objStackInquiry.ListAudRpt.ToList();
                        if (rptSource.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            {
                                if (type == "PDF")
                                {


                                    rd.Load(strRptPath);

                                    rd.SetDataSource(rptSource);
                                    var rpt_title = "AUDIT  TRAIL  REPORT  (Summary)";
                                    var rpt_title1 = "-With ISM";
                                    var rpttitle = rpt_title + rpt_title1;
                                    rd.SetParameterValue("fml_rep_title", rpttitle);
                                    objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                    rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//IV_STOCK_INQ" + strDateFormat + ".pdf";

                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                    reportFileName = "IV_STK_INQ_" + DateTime.Now.ToFileTime() + ".pdf";
                                    Session["RptFileName"] = strFileName;
                                    //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }

                                else
                        if (type == "XLS")
                                {
                                    rd.Load(strRptPath);

                                    rd.SetDataSource(rptSource);
                                    var rpt_title = "AUDIT  TRAIL  REPORT (Detail)";
                                    var rpt_title1 = "- Without ISM";
                                    var rpttitle = rpt_title + rpt_title1;
                                    rd.SetParameterValue("fml_rep_title", rpttitle);
                                    objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                    rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");

                                    strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//IV_STOCK_INQ" + strDateFormat + ".xls";

                                    rd.ExportToDisk(ExportFormatType.ExcelWorkbook, strFileName);
                                    reportFileName = "IV_STK_INQ_" + DateTime.Now.ToFileTime() + ".xls";
                                    Session["RptFileName"] = strFileName;
                                }
                            }

                        }
                    }
                }
                else
                {
                    Response.Write("<H2>Report not found</H2>");
                }

                Email objEmail = new Email();
                objEmail.CmpId = p_str_cmp_id;
                objEmail.EmailSubject = "IV_STK_INQ";
                EmailService objEmailService = new EmailService();
                objEmail = objEmailService.GetSendMailDetails(objEmail);
                if (objEmail.ListEamilDetail.Count != 0)
                {

                    objEmail.Attachment = reportFileName;
                    objEmail.EmailTo = (objEmail.ListEamilDetail[0].EmailTo.Trim() == null || objEmail.ListEamilDetail[0].EmailTo.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailTo.Trim();
                    objEmail.EmailCC = (objEmail.ListEamilDetail[0].EmailCC.Trim() == null || objEmail.ListEamilDetail[0].EmailCC.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailCC.Trim();
                    objEmail.EmailMessage = (objEmail.ListEamilDetail[0].EmailMessage.Trim() == null || objEmail.ListEamilDetail[0].EmailMessage.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailMessage.Trim();

                }
                else
                {
                    objEmail.Attachment = reportFileName;
                    objEmail.EmailTo = "";
                    objEmail.EmailCC = "";
                    objEmail.EmailMessage = "";
                }
                Mapper.CreateMap<Email, EmailModel>();
                EmailModel EmailModel = Mapper.Map<Email, EmailModel>(objEmail);

                return PartialView("_Email", EmailModel);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                jsonErrorCode = "-2";
            }
            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerateAuditItemDtl(string p_str_cmp_id, string p_str_itm_num, string p_str_itm_color, string p_str_itm_size)
        {
            IStockInquiryService ServiceObject = new StockInquiryService();
            StockInquiryDtl objStackInquiry = new StockInquiryDtl();

            objStackInquiry = ServiceObject.GetAuditItemDtl(objStackInquiry, p_str_cmp_id, p_str_itm_num, p_str_itm_color, p_str_itm_size);

            List<AuditItemDtl> LstAuditItemDtl = new List<AuditItemDtl>();
            int l_int_tot_rcvd_qty = 0;
            int l_int_tot_aloc_qty = 0;
            int l_int_tot_sr_qty = 0;
            int l_int_tot_avail_qty = 0;
            AuditItemDtl objStackInquiryExcel;
            for (int i = 0; i < objStackInquiry.LstAuditItemDtl.Count; i++)
            {

                objStackInquiryExcel = new AuditItemDtl();
                objStackInquiryExcel.CmpId = objStackInquiry.LstAuditItemDtl[i].CmpId;
                objStackInquiryExcel.Style = objStackInquiry.LstAuditItemDtl[i].Style;
                objStackInquiryExcel.Color = objStackInquiry.LstAuditItemDtl[i].Color;
                objStackInquiryExcel.Size = objStackInquiry.LstAuditItemDtl[i].Size;
                objStackInquiryExcel.InvStatus = objStackInquiry.LstAuditItemDtl[i].InvStatus;
                if (objStackInquiry.LstAuditItemDtl[i].InvStatus == "RCVD")
                    l_int_tot_rcvd_qty = l_int_tot_rcvd_qty + objStackInquiry.LstAuditItemDtl[i].RcvdQty;
                else if (objStackInquiry.LstAuditItemDtl[i].InvStatus == "OBSR")
                    l_int_tot_sr_qty = l_int_tot_sr_qty + objStackInquiry.LstAuditItemDtl[i].SRQty;
                else
                    l_int_tot_aloc_qty = l_int_tot_aloc_qty + objStackInquiry.LstAuditItemDtl[i].AlocQty;


                objStackInquiryExcel.DocDate = objStackInquiry.LstAuditItemDtl[i].DocDate;
                objStackInquiryExcel.IBDocId = objStackInquiry.LstAuditItemDtl[i].IBDocId;
                objStackInquiryExcel.Container = objStackInquiry.LstAuditItemDtl[i].Container;

                objStackInquiryExcel.RcvdQty = objStackInquiry.LstAuditItemDtl[i].RcvdQty;
                objStackInquiryExcel.OBDocId = objStackInquiry.LstAuditItemDtl[i].OBDocId;
                objStackInquiryExcel.SRQty = objStackInquiry.LstAuditItemDtl[i].SRQty;

                objStackInquiryExcel.AlocDocId = objStackInquiry.LstAuditItemDtl[i].AlocDocId;
                objStackInquiryExcel.RefPickNumber = objStackInquiry.LstAuditItemDtl[i].RefPickNumber;
                objStackInquiryExcel.AlocQty = objStackInquiry.LstAuditItemDtl[i].AlocQty;
                objStackInquiryExcel.BalanceQty = "";
                LstAuditItemDtl.Add(objStackInquiryExcel);
            }
            l_int_tot_avail_qty = l_int_tot_rcvd_qty - l_int_tot_aloc_qty;
            // This for Summary
            objStackInquiryExcel = new AuditItemDtl();
            objStackInquiryExcel.CmpId = objStackInquiry.LstAuditItemDtl[0].CmpId;
            objStackInquiryExcel.Style = "";
            objStackInquiryExcel.Color = "";
            objStackInquiryExcel.Size = "";
            objStackInquiryExcel.InvStatus = "Total :";

            objStackInquiryExcel.BalanceQty = Convert.ToString(l_int_tot_avail_qty);
            objStackInquiryExcel.IBDocId = "";
            objStackInquiryExcel.Container = "";
            objStackInquiryExcel.AlocDocId = "";
            objStackInquiryExcel.RcvdQty = l_int_tot_rcvd_qty;
            objStackInquiryExcel.SRQty = l_int_tot_sr_qty;
            objStackInquiryExcel.OBDocId = "";
            objStackInquiryExcel.RefPickNumber = "";
            objStackInquiryExcel.AlocQty = l_int_tot_aloc_qty;
            LstAuditItemDtl.Add(objStackInquiryExcel);

            GridView gv = new GridView();
            gv.DataSource = LstAuditItemDtl;
            gv.DataBind();
            Session["IV_AUDIT_BY_STYLE"] = gv;
            return Json("IV_AUDIT_BY_STYLE", JsonRequestBehavior.AllowGet);
           // return new DownloadFileActionResult((GridView)Session["IV_AUDIT_BY_STYLE"], "IV_AUDIT_BY_STYLE" + "" + DateTime.Now.ToString("yyyyMMddHHssmm") + ".xls");

        }

        public ActionResult showreportAudit(string var_name, string p_str_cmp_id, string p_str_ib_doc_id, string p_str_itm_num, string p_str_itm_color,
          string p_str_itm_size, string p_str_itm_name, string p_str_cont_id, string p_str_lot_id, string p_str_loc_id, string p_str_grn_id, string p_str_whs_id, string p_str_po_num, string type)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            string l_str_rpt_selection1 = string.Empty;
            l_str_rpt_selection = var_name;
            objCustMaster.cust_id = p_str_cmp_id;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }
            //l_str_rpt_selection1 = var_name1;
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "DetailWithoutISM")
                    {
                        strReportName = "rpt_iv_stk_aud_ea_not.rpt";
                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.radio = l_str_rpt_selection;
                        objStackInquiry = ServiceObject.GetAudRptDetails(objStackInquiry);

                        if (type == "PDF" || type == null)
                        {
                            var rptSource = objStackInquiry.ListAudRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objStackInquiry.ListAudRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    var rpt_title = "AUDIT  TRAIL  REPORT (Detail)";
                                    var rpt_title1 = "- Without ISM";
                                    var rpttitle = rpt_title + rpt_title1;
                                    objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                    rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                    rd.SetParameterValue("fml_rep_title", rpttitle);

                                    string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                                    string tempFileName = string.Empty;
                                    string l_str_file_name = string.Empty;
                                    string strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                    if (!Directory.Exists(strOutputpath))
                                    {
                                        Directory.CreateDirectory(strOutputpath);
                                    }
                                    l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-IV-AUDIT-" + strDateFormat + ".xlsx";
                                    tempFileName = strOutputpath + l_str_file_name;

                                    if (System.IO.File.Exists(tempFileName))
                                        System.IO.File.Delete(tempFileName);
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, tempFileName);
                                    return Json(tempFileName, JsonRequestBehavior.AllowGet);
                                    // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");


                                }
                            }
                        }

                        else
                        if (type == "Excel")
                        {
                            objStackInquiry = ServiceObject.GetAuditWithOutIsmExcel(objStackInquiry);

                            List<StockAuditTrail_WithoutISMExcel> li = new List<StockAuditTrail_WithoutISMExcel>();
                            for (int i = 0; i < objStackInquiry.LstAuditWithOutIsm.Count; i++)
                            {

                                StockAuditTrail_WithoutISMExcel objStackInquiryExcel = new StockAuditTrail_WithoutISMExcel();
                                objStackInquiryExcel.Style = objStackInquiry.LstAuditWithOutIsm[i].Style;
                                objStackInquiryExcel.Color = objStackInquiry.LstAuditWithOutIsm[i].Color;
                                objStackInquiryExcel.Size = objStackInquiry.LstAuditWithOutIsm[i].Size;
                                objStackInquiryExcel.IBDocId = objStackInquiry.LstAuditWithOutIsm[i].IBDocId;
                                objStackInquiryExcel.ContainerId = objStackInquiry.LstAuditWithOutIsm[i].ContainerId;
                                objStackInquiryExcel.LotId = objStackInquiry.LstAuditWithOutIsm[i].lot_id;
                                objStackInquiryExcel.Date = objStackInquiry.LstAuditWithOutIsm[i].Date;
                                objStackInquiryExcel.VendorName = objStackInquiry.LstAuditWithOutIsm[i].VendorName;
                                objStackInquiryExcel.Whs = objStackInquiry.LstAuditWithOutIsm[i].Whs;
                                objStackInquiryExcel.Loc = objStackInquiry.LstAuditWithOutIsm[i].Loc;
                               objStackInquiryExcel.PoNumber = objStackInquiry.LstAuditWithOutIsm[i].PoNumber;
                                objStackInquiryExcel.DocId = objStackInquiry.LstAuditWithOutIsm[i].Doc_ID;
                                objStackInquiryExcel.Ctns = objStackInquiry.LstAuditWithOutIsm[i].Ctns;
                                objStackInquiryExcel.TransType = objStackInquiry.LstAuditWithOutIsm[i].In_Out;
                                objStackInquiryExcel.Pcs = objStackInquiry.LstAuditWithOutIsm[i].Pcs;
                                objStackInquiryExcel.Notes = objStackInquiry.LstAuditWithOutIsm[i].Notes;
                                objStackInquiryExcel.Length = objStackInquiry.LstAuditWithOutIsm[i].Length;
                                objStackInquiryExcel.Width = objStackInquiry.LstAuditWithOutIsm[i].Width;
                                objStackInquiryExcel.Height = objStackInquiry.LstAuditWithOutIsm[i].Height;
                                objStackInquiryExcel.Cube = objStackInquiry.LstAuditWithOutIsm[i].Cube;
                                objStackInquiryExcel.Weight = objStackInquiry.LstAuditWithOutIsm[i].Weight;
                                li.Add(objStackInquiryExcel);
                            }

                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["IV_AUDIT_WITHOUTISM"] = gv;
                            return Json("IV_AUDIT_WITHOUTISM", JsonRequestBehavior.AllowGet);
                          //  return new DownloadFileActionResult((GridView)Session["IV_AUDIT_WITHOUTISM"], "IV_AUDIT_WITHOUTISM" + "" + DateTime.Now.ToString("yyyyMMddHHssmm") + ".xls");



                        }

                    }
                    if (l_str_rpt_selection == "DetailWithISM")
                    {
                        strReportName = "rpt_iv_stk_aud_ea_not.rpt";
                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.radio = l_str_rpt_selection;
                        objStackInquiry = ServiceObject.GetAudRptDetails(objStackInquiry);

                        if (type == "PDF")
                        {
                            var rptSource = objStackInquiry.ListAudRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objStackInquiry.ListAudRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    var rpt_title = "AUDIT  TRAIL  REPORT (Detail)";
                                    var rpt_title1 = "-With ISM";
                                    var rpttitle = rpt_title + rpt_title1;
                                    objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                    rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                    rd.SetParameterValue("fml_rep_title", rpttitle);
                                    string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                                    string tempFileName = string.Empty;
                                    string l_str_file_name = string.Empty;
                                    string strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                    if (!Directory.Exists(strOutputpath))
                                    {
                                        Directory.CreateDirectory(strOutputpath);
                                    }
                                    l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-IV-AUDIT-" + strDateFormat + ".xlsx";
                                    tempFileName = strOutputpath + l_str_file_name;

                                    if (System.IO.File.Exists(tempFileName))
                                        System.IO.File.Delete(tempFileName);
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, tempFileName);
                                    return Json(tempFileName, JsonRequestBehavior.AllowGet);
                                    //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }

                        else
                        if (type == "Excel")
                        {

                            objStackInquiry = ServiceObject.GetAuditWithOutIsmExcel(objStackInquiry);
                            List<StockAuditTrail_WithoutISMExcel> li = new List<StockAuditTrail_WithoutISMExcel>();
                            for (int i = 0; i < objStackInquiry.LstAuditWithOutIsm.Count; i++)
                            {

                                StockAuditTrail_WithoutISMExcel objStackInquiryExcel = new StockAuditTrail_WithoutISMExcel();
                                objStackInquiryExcel.Style = objStackInquiry.LstAuditWithOutIsm[i].Style;
                                objStackInquiryExcel.Color = objStackInquiry.LstAuditWithOutIsm[i].Color;
                                objStackInquiryExcel.Size = objStackInquiry.LstAuditWithOutIsm[i].Size;
                                objStackInquiryExcel.IBDocId = objStackInquiry.LstAuditWithOutIsm[i].IBDocId;
                                objStackInquiryExcel.ContainerId = objStackInquiry.LstAuditWithOutIsm[i].ContainerId;
                                objStackInquiryExcel.LotId = objStackInquiry.LstAuditWithOutIsm[i].lot_id;
                                objStackInquiryExcel.Date = objStackInquiry.LstAuditWithOutIsm[i].Date;
                                objStackInquiryExcel.VendorName = objStackInquiry.LstAuditWithOutIsm[i].VendorName;
                                objStackInquiryExcel.Whs = objStackInquiry.LstAuditWithOutIsm[i].Whs;
                                objStackInquiryExcel.Loc = objStackInquiry.LstAuditWithOutIsm[i].Loc;
                                objStackInquiryExcel.PoNumber = objStackInquiry.LstAuditWithOutIsm[i].PoNumber;
                                objStackInquiryExcel.DocId = objStackInquiry.LstAuditWithOutIsm[i].Doc_ID;
                                objStackInquiryExcel.Ctns = objStackInquiry.LstAuditWithOutIsm[i].Ctns;
                                objStackInquiryExcel.TransType = objStackInquiry.LstAuditWithOutIsm[i].In_Out;
                                objStackInquiryExcel.Pcs = objStackInquiry.LstAuditWithOutIsm[i].Pcs;
                                //objStackInquiryExcel.Length = objStackInquiry.LstAuditWithOutIsm[i].Length;
                                //objStackInquiryExcel.Width = objStackInquiry.LstAuditWithOutIsm[i].Width;
                                //objStackInquiryExcel.Height = objStackInquiry.LstAuditWithOutIsm[i].Height;
                                //objStackInquiryExcel.Cube = objStackInquiry.LstAuditWithOutIsm[i].Cube;
                                //objStackInquiryExcel.Weight = objStackInquiry.LstAuditWithOutIsm[i].Weight;
                                objStackInquiryExcel.Notes = objStackInquiry.LstAuditWithOutIsm[i].Notes;
                                objStackInquiryExcel.Length = objStackInquiry.LstAuditWithOutIsm[i].Length;
                                objStackInquiryExcel.Width = objStackInquiry.LstAuditWithOutIsm[i].Width;
                                objStackInquiryExcel.Height = objStackInquiry.LstAuditWithOutIsm[i].Height;
                                objStackInquiryExcel.Cube = objStackInquiry.LstAuditWithOutIsm[i].Cube;
                                objStackInquiryExcel.Weight = objStackInquiry.LstAuditWithOutIsm[i].Weight;
                                li.Add(objStackInquiryExcel);
                            }

                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["IV_AUDIT_WITHOUTISM"] = gv;
                            return Json("IV_AUDIT_WITHOUTISM", JsonRequestBehavior.AllowGet);
                            //return new DownloadFileActionResult((GridView)Session["IV_AUDIT_WITHOUTISM"], "IV_AUDIT_WITHOUTISM" + "" + DateTime.Now.ToString("yyyyMMddHHssmm") + ".xls");


                        }


                    }
                    if (l_str_rpt_selection == "DetailOnlyISM")
                    {
                        strReportName = "rpt_iv_stk_aud_ea_not.rpt";
                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.radio = l_str_rpt_selection;
                        objStackInquiry = ServiceObject.GetAudRptDetails(objStackInquiry);

                        if (type == "PDF")
                        {

                            var rptSource = objStackInquiry.ListAudRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objStackInquiry.ListAudRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    var rpt_title = "AUDIT  TRAIL  REPORT (Detail)";
                                    var rpt_title1 = "- ISM Only";
                                    var rpttitle = rpt_title + rpt_title1;
                                    objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                    rd.SetParameterValue("fml_rep_title", rpttitle);
                                    rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                    string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                                    string tempFileName = string.Empty;
                                    string l_str_file_name = string.Empty;
                                    string strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                    if (!Directory.Exists(strOutputpath))
                                    {
                                        Directory.CreateDirectory(strOutputpath);
                                    }
                                    l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-IV-AUDIT-" + strDateFormat + ".xlsx";
                                    tempFileName = strOutputpath + l_str_file_name;

                                    if (System.IO.File.Exists(tempFileName))
                                        System.IO.File.Delete(tempFileName);
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, tempFileName);
                                    return Json(tempFileName, JsonRequestBehavior.AllowGet);

                                  //  rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }

                        else
                        if (type == "Excel")
                        {
                            objStackInquiry = ServiceObject.GetAuditOnlyIsmExcel(objStackInquiry);

                            List<StockAuditTrail_Smry_OnlyISMExcel> li = new List<StockAuditTrail_Smry_OnlyISMExcel>();
                            for (int i = 0; i < objStackInquiry.LstAuditOnlyIsm.Count; i++)
                            {

                                StockAuditTrail_Smry_OnlyISMExcel objStackInquiryExcel = new StockAuditTrail_Smry_OnlyISMExcel();
                                objStackInquiryExcel.Style = objStackInquiry.LstAuditOnlyIsm[i].Style;
                                objStackInquiryExcel.Color = objStackInquiry.LstAuditOnlyIsm[i].Color;
                                objStackInquiryExcel.Size = objStackInquiry.LstAuditOnlyIsm[i].Size;
                                //objStackInquiryExcel.Po_Num = objStackInquiry.LstAuditOnlyIsm[i].Po_Num;
                                objStackInquiryExcel.Pcs = objStackInquiry.LstAuditOnlyIsm[i].Pcs;

                                li.Add(objStackInquiryExcel);
                            }

                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["IV_AUDIT_ONLYISM"] = gv;
                            return Json("IV_AUDIT_ONLYISM", JsonRequestBehavior.AllowGet);
                            //   return new DownloadFileActionResult((GridView)Session["IV_AUDIT_ONLYISM"], "IV_AUDIT_ONLYISM" + "" + DateTime.Now.ToString("yyyyMMddHHssmm") + ".xls");



                        }
                    }
                    if (l_str_rpt_selection == "SummaryWithoutISM")
                    {
                        strReportName = "rpt_iv_stk_aud_ea_smry.rpt";
                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.radio = l_str_rpt_selection;
                        objStackInquiry = ServiceObject.GetAudRptSummary(objStackInquiry);

                        if (type == "PDF")
                        {
                            var rptSource = objStackInquiry.ListAudRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objStackInquiry.ListAudRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    var rpt_title = "AUDIT  TRAIL  REPORT  (Summary)";
                                    var rpt_title1 = "-Without ISM";
                                    var rpttitle = rpt_title + rpt_title1;
                                    objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                    rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                    rd.SetParameterValue("fml_rep_title", rpttitle);
                                    string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                                    string tempFileName = string.Empty;
                                    string l_str_file_name = string.Empty;
                                    string strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                    if (!Directory.Exists(strOutputpath))
                                    {
                                        Directory.CreateDirectory(strOutputpath);
                                    }
                                    l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-IV-AUDIT-" + strDateFormat + ".xlsx";
                                    tempFileName = strOutputpath + l_str_file_name;

                                    if (System.IO.File.Exists(tempFileName))
                                        System.IO.File.Delete(tempFileName);
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, tempFileName);
                                    return Json(tempFileName, JsonRequestBehavior.AllowGet);
                                   // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }

                        else
                        if (type == "Excel")
                        {
                            objStackInquiry = ServiceObject.GetAuditWithIsmExcel(objStackInquiry);

                            List<StockAuditTrail_Smry_WithISMExcel> li = new List<StockAuditTrail_Smry_WithISMExcel>();
                            for (int i = 0; i < objStackInquiry.LstAuditWithIsm.Count; i++)
                            {
                                StockAuditTrail_Smry_WithISMExcel objStackInquiryExcel = new StockAuditTrail_Smry_WithISMExcel();
                                objStackInquiryExcel.Style = objStackInquiry.LstAuditWithIsm[i].Style;
                                objStackInquiryExcel.Color = objStackInquiry.LstAuditWithIsm[i].Color;
                                objStackInquiryExcel.Size = objStackInquiry.LstAuditWithIsm[i].Size;
                                objStackInquiryExcel.Pcs = objStackInquiry.LstAuditWithIsm[i].Pcs;
                                li.Add(objStackInquiryExcel);
                            }

                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["IV_AUDIT_WITHOUTISM"] = gv;
                            return Json("IV_AUDIT_WITHOUTISM", JsonRequestBehavior.AllowGet);
                          //  return new DownloadFileActionResult((GridView)Session["IV_AUDIT_WITHISM"], "IV_AUDIT_WITHISM" + "" + DateTime.Now.ToString("yyyyMMddHHssmm") + ".xls");




                        }

                    }
                    if (l_str_rpt_selection == "SummaryWithISM")
                    {
                        strReportName = "rpt_iv_stk_aud_ea_smry.rpt";
                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.radio = l_str_rpt_selection;
                        objStackInquiry = ServiceObject.GetAudRptSummary(objStackInquiry);


                        if (type == "PDF")
                        {
                            var rptSource = objStackInquiry.ListAudRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objStackInquiry.ListAudRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    var rpt_title = "AUDIT  TRAIL  REPORT  (Summary)";
                                    var rpt_title1 = "-With ISM";
                                    var rpttitle = rpt_title + rpt_title1;
                                    objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                    rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                    rd.SetParameterValue("fml_rep_title", rpttitle);
                                    string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                                    string tempFileName = string.Empty;
                                    string l_str_file_name = string.Empty;
                                    string strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                    if (!Directory.Exists(strOutputpath))
                                    {
                                        Directory.CreateDirectory(strOutputpath);
                                    }
                                    l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-IV-AUDIT-" + strDateFormat + ".xlsx";
                                    tempFileName = strOutputpath + l_str_file_name;

                                    if (System.IO.File.Exists(tempFileName))
                                        System.IO.File.Delete(tempFileName);
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, tempFileName);
                                    return Json(tempFileName, JsonRequestBehavior.AllowGet);
                                   // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }

                        else
                        if (type == "Excel")
                        {
                            objStackInquiry = ServiceObject.GetAuditWithIsmExcel(objStackInquiry);

                            List<StockAuditTrail_Smry_WithISMExcel> li = new List<StockAuditTrail_Smry_WithISMExcel>();
                            for (int i = 0; i < objStackInquiry.LstAuditWithIsm.Count; i++)
                            {

                                StockAuditTrail_Smry_WithISMExcel objStackInquiryExcel = new StockAuditTrail_Smry_WithISMExcel();
                                objStackInquiryExcel.Style = objStackInquiry.LstAuditWithIsm[i].Style;
                                objStackInquiryExcel.Color = objStackInquiry.LstAuditWithIsm[i].Color;
                                objStackInquiryExcel.Size = objStackInquiry.LstAuditWithIsm[i].Size;
                                //objStackInquiryExcel.Item_name = objStackInquiry.LstAuditWithIsm[i].Item_name;
                                objStackInquiryExcel.Pcs = objStackInquiry.LstAuditWithIsm[i].Pcs;
                                //objStackInquiryExcel.Length = objStackInquiry.LstAuditWithIsm[i].Length;
                                //objStackInquiryExcel.Width = objStackInquiry.LstAuditWithIsm[i].Width;
                                //objStackInquiryExcel.Height = objStackInquiry.LstAuditWithIsm[i].Height;
                                //objStackInquiryExcel.Cube = objStackInquiry.LstAuditWithIsm[i].Cube;
                                //objStackInquiryExcel.Weight = objStackInquiry.LstAuditWithIsm[i].Weight;
                                li.Add(objStackInquiryExcel);
                            }

                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["IV_AUDIT_WITHISM"] = gv;
                            return Json("IV_AUDIT_WITHISM", JsonRequestBehavior.AllowGet);
                            //return new DownloadFileActionResult((GridView)Session["IV_AUDIT_WITHISM"], "IV_AUDIT_WITHISM" + "" + DateTime.Now.ToString("yyyyMMddHHssmm") + ".xls");



                        }

                    }
                    if (l_str_rpt_selection == "SummaryonlyISM")
                    {
                        strReportName = "rpt_iv_stk_aud_ea_smry.rpt";
                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.radio = l_str_rpt_selection;
                        objStackInquiry = ServiceObject.GetAudRptSummary(objStackInquiry);

                        if (type == "PDF")
                        {
                            var rptSource = objStackInquiry.ListAudRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objStackInquiry.ListAudRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    var rpt_title = "AUDIT  TRAIL  REPORT  (Summary)";
                                    var rpt_title1 = "- ISM Only";
                                    var rpttitle = rpt_title + rpt_title1;
                                    objStackInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0425_001 
                                    rd.SetParameterValue("fml_image_path", objStackInquiry.Image_Path);
                                    rd.SetParameterValue("fml_rep_title", rpttitle);
                                    string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                                    string tempFileName = string.Empty;
                                    string l_str_file_name = string.Empty;
                                    string strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                    if (!Directory.Exists(strOutputpath))
                                    {
                                        Directory.CreateDirectory(strOutputpath);
                                    }
                                    l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-IV-AUDIT-" + strDateFormat + ".xlsx";
                                    tempFileName = strOutputpath + l_str_file_name;

                                    if (System.IO.File.Exists(tempFileName))
                                        System.IO.File.Delete(tempFileName);
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, tempFileName);
                                    return Json(tempFileName, JsonRequestBehavior.AllowGet);
                                   // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }

                        else if (type == "Excel")
                        {
                            objStackInquiry = ServiceObject.GetAuditOnlyIsmExcel(objStackInquiry);

                            List<StockAuditTrail_Smry_OnlyISMExcel> li = new List<StockAuditTrail_Smry_OnlyISMExcel>();
                            for (int i = 0; i < objStackInquiry.LstAuditOnlyIsm.Count; i++)
                            {

                                StockAuditTrail_Smry_OnlyISMExcel objStackInquiryExcel = new StockAuditTrail_Smry_OnlyISMExcel();
                                objStackInquiryExcel.Style = objStackInquiry.LstAuditOnlyIsm[i].Style;
                                objStackInquiryExcel.Color = objStackInquiry.LstAuditOnlyIsm[i].Color;
                                objStackInquiryExcel.Size = objStackInquiry.LstAuditOnlyIsm[i].Size;
                                //objStackInquiryExcel.Po_Num = objStackInquiry.LstAuditOnlyIsm[i].Po_Num;
                                objStackInquiryExcel.Pcs = objStackInquiry.LstAuditOnlyIsm[i].Pcs;

                                li.Add(objStackInquiryExcel);
                            }

                            GridView gv = new GridView();
                            gv.DataSource = li;
                            gv.DataBind();
                            Session["IV_AUDIT_ONLYISM"] = gv;
                            return Json("IV_AUDIT_WITHISM", JsonRequestBehavior.AllowGet);
                           // return new DownloadFileActionResult((GridView)Session["IV_AUDIT_ONLYISM"], "IV_AUDIT_ONLYISM" + "" + DateTime.Now.ToString("yyyyMMddHHssmm") + ".xls");



                        }
                    }
                    else
                    {
                        Response.Write("<H2>Report not found</H2>");
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                jsonErrorCode = "-2";
            }
            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);
        }

        public EmptyResult CmpIdOnChange(string p_str_cmp_id)
        {
            Session["g_str_cmp_id"] = (p_str_cmp_id == null ? string.Empty : p_str_cmp_id.Trim());
            return null;
        }
        public ActionResult RefreshReport()
        {
            return PartialView("_ReportsDisplay");
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ReportView()
        {
            return View();
        }
        public JsonResult ItemXGetitmDtl(string term, string cmp_id)
        {
            IStockInquiryService ServiceObject = new StockInquiryService();
            var List = ServiceObject.ItemXGetitmDetails(term, cmp_id.Trim()).LstItmxCustdtl.Select(x => new { label = x.Itmdtl, value = x.itm_num, itm_num = x.itm_num, itm_color = x.itm_color, itm_size = x.itm_size, itm_name = x.itm_name }).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AuditItemXGetitmDtl(string term, string cmp_id)
        {
            IStockInquiryService ServiceObject = new StockInquiryService();
            var List = ServiceObject.ItemXGetitmDetails(term, cmp_id).LstItmxCustdtl.Select(x => new { label = x.Itmdtl, value = x.itm_num, itm_num = x.itm_num, itm_color = x.itm_color, itm_size = x.itm_size, itm_name = x.itm_name }).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        //CR-180420-001 Added By Nithya
        public JsonResult showreportAuditResult(string var_name, string p_str_cmp_id, string p_str_ib_doc_id, string p_str_itm_num, string p_str_itm_color,
         string p_str_itm_size, string p_str_itm_name, string p_str_cont_id, string p_str_lot_id, string p_str_loc_id, string p_str_grn_id, string p_str_whs_id, string p_str_po_num, string type)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            string l_str_rpt_selection1 = string.Empty;
            l_str_rpt_selection = var_name;
            //l_str_rpt_selection1 = var_name1;
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "DetailWithoutISM")
                    {

                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.radio = l_str_rpt_selection;
                        objStackInquiry = ServiceObject.GetAudRptDetails(objStackInquiry);
                        if (objStackInquiry.ListAudRpt.Count == 0)
                        {
                            int ResultCount = 1;
                            return Json(ResultCount, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (l_str_rpt_selection == "DetailWithISM")
                    {

                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.radio = l_str_rpt_selection;
                        objStackInquiry = ServiceObject.GetAudRptDetails(objStackInquiry);
                        if (objStackInquiry.ListAudRpt.Count == 0)
                        {
                            int ResultCount = 1;
                            return Json(ResultCount, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (l_str_rpt_selection == "DetailOnlyISM")
                    {

                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.radio = l_str_rpt_selection;
                        objStackInquiry = ServiceObject.GetAudRptDetails(objStackInquiry);
                        if (objStackInquiry.ListAudRpt.Count == 0)
                        {
                            int ResultCount = 1;
                            return Json(ResultCount, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (l_str_rpt_selection == "SummaryWithoutISM")
                    {

                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.radio = l_str_rpt_selection;
                        objStackInquiry = ServiceObject.GetAudRptSummary(objStackInquiry);
                        if (objStackInquiry.ListAudRpt.Count == 0)
                        {
                            int ResultCount = 1;
                            return Json(ResultCount, JsonRequestBehavior.AllowGet);
                        }

                    }
                    if (l_str_rpt_selection == "SummaryWithISM")
                    {

                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.radio = l_str_rpt_selection;
                        objStackInquiry = ServiceObject.GetAudRptSummary(objStackInquiry);
                        if (objStackInquiry.ListAudRpt.Count == 0)
                        {
                            int ResultCount = 1;
                            return Json(ResultCount, JsonRequestBehavior.AllowGet);
                        }

                    }
                    if (l_str_rpt_selection == "SummaryonlyISM")
                    {
                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.radio = l_str_rpt_selection;
                        objStackInquiry = ServiceObject.GetAudRptSummary(objStackInquiry);
                        if (objStackInquiry.ListAudRpt.Count == 0)
                        {
                            int ResultCount = 1;
                            return Json(ResultCount, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                jsonErrorCode = "-2";
            }
            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);
        }
        //CR-180425-001 Added By Nithya
        public ActionResult ShowReportResult(string var_name, string p_str_cmp_id, string p_str_ib_doc_id, string p_str_itm_num, string p_str_itm_color,
            string p_str_itm_size, string p_str_itm_name, string p_str_cont_id, string p_str_lot_id, string p_str_loc_id, string p_str_grn_id, string p_str_whs_id, string p_str_Date_fm, string p_str_Date_To,
            string p_str_po_num, string lblalocqty, string type, string p_str_style_stearch)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            string l_str_rpt_selection1 = string.Empty;
            l_str_rpt_selection = var_name;
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "DetailbyStyle")
                    {

                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.Date_fm = p_str_Date_fm;
                        objStackInquiry.Date_to = p_str_Date_To;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.style_stearch = p_str_style_stearch;
                        objStackInquiry.AlocQty = Convert.ToInt32(Session["tempAlocQty"].ToString().Trim());
                        objStackInquiry = ServiceObject.GetStockRptDetails(objStackInquiry);
                        if (objStackInquiry.ListStockInquiryRpt.Count == 0)
                        {
                            int ResultCount = 1;
                            return Json(ResultCount, JsonRequestBehavior.AllowGet);
                        }

                    }
                    if (l_str_rpt_selection == "SummarybyStyle")
                    {

                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.Date_fm = p_str_Date_fm;
                        objStackInquiry.Date_to = p_str_Date_To;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.style_stearch = p_str_style_stearch;
                        objStackInquiry.AlocQty = Convert.ToInt32(Session["tempAlocQty"].ToString().Trim());

                        objStackInquiry = ServiceObject.GetStockStyleSummRptDetails(objStackInquiry);
                        if (objStackInquiry.ListStockLocRpt.Count == 0)
                        {
                            int ResultCount = 1;
                            return Json(ResultCount, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (l_str_rpt_selection == "DetailbyLoc")
                    {
                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.Date_fm = p_str_Date_fm;
                        objStackInquiry.Date_to = p_str_Date_To;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.style_stearch = p_str_style_stearch;
                        objStackInquiry.AlocQty = Convert.ToInt32(Session["tempAlocQty"].ToString().Trim());
                        objStackInquiry.style_stearch = p_str_style_stearch;
                        objStackInquiry = ServiceObject.GetStockLocRptDetails(objStackInquiry);
                        if (objStackInquiry.ListStockLocRpt.Count == 0)
                        {
                            int ResultCount = 1;
                            return Json(ResultCount, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (l_str_rpt_selection == "SummarybyLoc")
                    {
                        StockInquiryDtl objStackInquiry = new StockInquiryDtl();
                        IStockInquiryService ServiceObject = new StockInquiryService();
                        objStackInquiry.cmp_id = p_str_cmp_id;
                        objStackInquiry.ib_doc_id = p_str_ib_doc_id;
                        objStackInquiry.itm_num = p_str_itm_num;
                        objStackInquiry.itm_color = p_str_itm_color;
                        objStackInquiry.itm_size = p_str_itm_size;
                        objStackInquiry.itm_name = p_str_itm_name;
                        objStackInquiry.cont_id = p_str_cont_id;
                        objStackInquiry.lot_id = p_str_lot_id;
                        objStackInquiry.loc_id = p_str_loc_id;
                        objStackInquiry.grn_id = p_str_grn_id;
                        objStackInquiry.whs_id = p_str_whs_id;
                        objStackInquiry.Date_fm = p_str_Date_fm;
                        objStackInquiry.Date_to = p_str_Date_To;
                        objStackInquiry.po_num = p_str_po_num;
                        objStackInquiry.style_stearch = p_str_style_stearch;
                        objStackInquiry.AlocQty = Convert.ToInt32(Session["tempAlocQty"].ToString().Trim());
                        objStackInquiry = ServiceObject.GetStockInquiryLocSmryExcel(objStackInquiry);
                        if (objStackInquiry.LstStockInquiryLocSmry.Count == 0)
                        {
                            int ResultCount = 1;
                            return Json(ResultCount, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                jsonErrorCode = "-2";
            }

            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult ItemXGetLocDtl(string term, string cmp_id)
        {
            StockChangeService ServiceObject = new StockChangeService();
            var List = ServiceObject.ItemXGetLocDetails(term.Trim(), cmp_id).LstItmxlocdtl.Select(x => new { label = x.loc_id, value = x.loc_id }).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }


        public ActionResult XlsIvStkSmryRpt(string var_name, string p_str_cmp_id, string p_str_ib_doc_id, string p_str_itm_num, string p_str_itm_color,
            string p_str_itm_size, string p_str_itm_name, string p_str_cont_id, string p_str_lot_id, string p_str_loc_id, string p_str_grn_id, string p_str_whs_id,
            string p_str_Date_fm, string p_str_Date_To, string p_str_po_num, string lblalocqty, string type, string p_str_style_stearch)
        {


            string tempFileName = string.Empty;
            string l_str_file_name = string.Empty;
            string strDateFormat = String.Empty;
            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
            if (!Directory.Exists(strOutputpath))
            {
                Directory.CreateDirectory(strOutputpath);
            }

            StockInquiryDtl objStackInquiry = new StockInquiryDtl();
            IStockInquiryService ServiceObject = new StockInquiryService();
            objStackInquiry.cmp_id = p_str_cmp_id;
            objStackInquiry.ib_doc_id = p_str_ib_doc_id;
            objStackInquiry.itm_num = p_str_itm_num;
            objStackInquiry.itm_color = p_str_itm_color;
            objStackInquiry.itm_size = p_str_itm_size;
            objStackInquiry.itm_name = p_str_itm_name;
            objStackInquiry.cont_id = p_str_cont_id;
            objStackInquiry.lot_id = p_str_lot_id;
            objStackInquiry.loc_id = p_str_loc_id;
            objStackInquiry.grn_id = p_str_grn_id;
            objStackInquiry.whs_id = p_str_whs_id;
            objStackInquiry.Date_fm = p_str_Date_fm;
            objStackInquiry.Date_to = p_str_Date_To;
            objStackInquiry.po_num = p_str_po_num;
            objStackInquiry.style_stearch = p_str_style_stearch;
            if (var_name == "SummarybyLoc")
            {

                DataTable dtBill = new DataTable();
                dtBill = ServiceObject.GetStockSmryByLocRpt(objStackInquiry);
                strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-INV-STK-SMRY-BY-LOC-" + strDateFormat + ".xlsx";

                tempFileName = strOutputpath + l_str_file_name;

                if (System.IO.File.Exists(tempFileName))
                    System.IO.File.Delete(tempFileName);
                xls_iv_stk_smry_by_loc mxcel1;
                mxcel1 = new xls_iv_stk_smry_by_loc(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "INV-STK-SMRY_BY_LOC.xlsx");
                mxcel1.PopulateHeader(p_str_cmp_id);
               
            

                mxcel1.PopulateData(dtBill, true);
                mxcel1.SaveAs(tempFileName);
                FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                return Json(tempFileName, JsonRequestBehavior.AllowGet);
            }
            else
            { 
            objStackInquiry.AlocQty = Convert.ToInt32(Session["tempAlocQty"].ToString().Trim());

            DataTable dtBill = new DataTable();
                if (var_name == "OpnToSellStkSryRpt")
                {
                   dtBill = ServiceObject.GetStockStyleSummRpt(objStackInquiry);
                }
                else
                {
                    dtBill = ServiceObject.GetStockSmryByStyleRpt(objStackInquiry);
                }
                       
            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
            l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-INV-STK-SMRY-BY-STYLE-" + strDateFormat +  ".xlsx";

            tempFileName = strOutputpath + l_str_file_name;

            if (System.IO.File.Exists(tempFileName))
                System.IO.File.Delete(tempFileName);
                xls_iv_stk_smry_by_style mxcel1;
                
                if (var_name == "OpnToSellStkSryRpt")
                {
                    mxcel1 = new xls_iv_stk_smry_by_style(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "INV-STK-SMRY_BY_STYLE-OPN-SELL.xlsx");
                    mxcel1.PopulateHeader(p_str_cmp_id, "Y");
                    mxcel1.PopulateData(dtBill, true,"Y");
                }
                else
                {
                    mxcel1 = new xls_iv_stk_smry_by_style(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "INV-STK-SMRY_BY_STYLE.xlsx");
                    mxcel1.PopulateHeader(p_str_cmp_id, "N");
                    mxcel1.PopulateData(dtBill, true,"N");
                }
               

           
            mxcel1.SaveAs(tempFileName);
            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
            return Json(tempFileName, JsonRequestBehavior.AllowGet);
            }

        }

    }



}
