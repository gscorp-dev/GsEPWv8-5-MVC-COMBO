using AutoMapper;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Common;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace GsEPWv8_5_MVC.Controllers
{
    #region Change History
    //CR_3PL_MVC_BL_2018_08_11  Anandan K      To Craete Item Master Page
    #endregion Change History
    public class ItemMasterController : Controller
    {
        public string ScreenID = "ItemMaster Inquiry";
        int l_str_RateId = 0;

        //post method for filter
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ItemMaster(string FullFillType, string cmp)
        {
            string l_str_cmp_id = string.Empty;
            string l_str_tmp_cmp_id = string.Empty;
            try
            {
                Core.Entity.ItemMaster objItemMaster = new Core.Entity.ItemMaster();
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                IItemMasterService ServiceObject = new ItemMasterService();
                Session["g_str_Search_flag"] = "False";
              //  objItemMaster.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                if (Session["g_str_cmp_id"] == null || Session["g_str_cmp_id"].ToString().Trim() == string.Empty)
                {
                    objItemMaster.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
                    Session["g_str_cmp_id"] = objItemMaster.cmp_id;
                    //l_str_cmp_id = Session["dflt_cmp_id"].ToString();

                }
                else
                {
                    objCompany.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                }
                // END CR_3PL_MVC_COMMON_2018_0326_001
                if (FullFillType == null)
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objItemMaster.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                }
                LookUp objLookUp = new LookUp();
                LookUpService ServiceObject2 = new LookUpService();
                objLookUp.id = "7";
                objLookUp.lookuptype = "ITEMMASTER";
                objLookUp = ServiceObject2.GetLookUpValue(objLookUp);
                objItemMaster.ListLookUpDtl = objLookUp.ListLookUpDtl;
                objItemMaster.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                Mapper.CreateMap<Core.Entity.ItemMaster, ItemMasterModel>();
                ItemMasterModel objItemMasterModel = Mapper.Map<Core.Entity.ItemMaster, ItemMasterModel>(objItemMaster);
                return View(objItemMasterModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ActionResult GetItemMasterDtls(string p_str_cmp_id, string p_str_itm_style, string p_str_itm_Color, string p_str_itm_Size, string p_str_itm_Descr)
        {
            ItemMaster objItemMaster = new ItemMaster();
            IItemMasterService ServiceObject = new ItemMasterService();
            objItemMaster.cmp_id = p_str_cmp_id;
            objItemMaster.itm_num = p_str_itm_style;
            objItemMaster.itm_color = p_str_itm_Color!=null? p_str_itm_Color:"";
            objItemMaster.itm_size = p_str_itm_Size;
            objItemMaster.itm_name = p_str_itm_Descr;
            TempData["cmp_id"] = objItemMaster.cmp_id;
            TempData["p_str_itm_style"] = objItemMaster.itm_num;
            TempData["p_str_itm_Color"] = objItemMaster.itm_color;
            TempData["p_str_itm_Size"] = objItemMaster.itm_size;
            TempData["p_str_itm_Descr"] = objItemMaster.itm_name;
            Session["g_str_Search_flag"] = "True";
            objItemMaster = ServiceObject.GetItemMasterDetails(objItemMaster);
            var model = objItemMaster.ListItemMaster;
            GridView gv = new GridView();
            gv.DataSource = model;
            gv.DataBind();
            Session["Rates"] = gv;
            Mapper.CreateMap<ItemMaster, ItemMasterModel>();
            ItemMasterModel objItemMasterModel = Mapper.Map<ItemMaster, ItemMasterModel>(objItemMaster);
            return PartialView("_ItemMaster", objItemMasterModel);
        }
        public ActionResult Add(string cmpid)
        {
         //   ItemMasterModel objItemMaster = new ItemMasterModel();
            ItemMaster objItemMaster = new ItemMaster();
            ItemMasterService serviceobject = new ItemMasterService();

            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            Price objPrice = new Price();
            PriceuomService ServiceObject2 = new PriceuomService();
            objItemMaster.cmp_id = cmpid;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objItemMaster.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            LookUp objLookUp = new LookUp();
            LookUpService ServiceObject1 = new LookUpService();
            objLookUp.id = "11";
            objLookUp.lookuptype = "RATESTATUS";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objItemMaster.ListLookUpStatusDtl = objLookUp.ListLookUpDtl;
            objItemMaster.ListBintatus = objLookUp.ListLookUpDtl;
            objItemMaster.ListPkgType = serviceobject.fnGetItemConfig("PKG_TYPE").ListItemConfig;
            objItemMaster.ListPkgPoly = serviceobject.fnGetItemConfig("PKG-POLY").ListItemConfig;
            objItemMaster.ListPkgBox = serviceobject.fnGetItemConfig("PKG-BOX").ListItemConfig;
            objItemMaster.ListPkgSrvc = serviceobject.fnGetItemPkgServiceg(cmpid,string.Empty).ListPkgSrvc;

            objItemMaster = serviceobject.GetItmId(objItemMaster);
            objItemMaster.itm_code = objItemMaster.itm_code;
            Session["sesItmCode"]= objItemMaster.itm_code;

             objLookUp = new LookUp();
            LookUpService ServiceObjects = new LookUpService();
            objLookUp.id = "104";
            objLookUp.lookuptype = "BIN_SIZE";
            objLookUp = ServiceObjects.GetLookUpValue(objLookUp);
            objItemMaster.ListLookUpBinType = objLookUp.ListLookUpDtl;
 
            PickService ServiceObjectPick = new PickService();
            Pick objPick = new Pick();
            ServiceObjectPick = new PickService();
            objPick.cmp_id = String.Empty;
            objPick = ServiceObjectPick.GetPickUom(objPick);

            objItemMaster.ListQtyUoM = objPick.ListExistShipToAddrsPick;


            objPick.cmp_id = cmpid;
            objPick.Whs_id = "";
            objPick.Whs_name = "";
            objPick = ServiceObjectPick.GetWhsPick(objPick);
            objItemMaster.ListWhs = objPick.ListPick;
            clsBinMater objBinMater = new clsBinMater();
            objBinMater.bin_type = "Mid";
            objBinMater.bin_length = 25.00M;
            objBinMater.bin_width = 19.00M;
            objBinMater.bin_height = 49.00M;
            objBinMater.bin_cube = 13.50M;
            if (objItemMaster.ListWhs.Count > 0)
            {
                objBinMater.whs_id = objItemMaster.ListWhs[0].Whs_id.Trim();
                objItemMaster.objBinMater = objBinMater;

            }
            Mapper.CreateMap<Core.Entity.ItemMaster, ItemMasterModel>();
            ItemMasterModel objItemMasterModel = Mapper.Map<Core.Entity.ItemMaster, ItemMasterModel>(objItemMaster);
            return PartialView("_ItemNewEntry", objItemMasterModel);
        }

        public ActionResult Edit(string cmpid, string staus, string ItmCode,string length, string Width, string Weight, string Size, string Colour,string p_str_bin_id)
        {
            ItemMaster objItemMaster = new ItemMaster();
            ItemMasterService serviceobject = new ItemMasterService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            IItemMasterService ServiceObject = new ItemMasterService();
            LookUp objLookUp = new LookUp();
            LookUpService ServiceObject1 = new LookUpService();
            Price objPrice = new Price();
            PriceuomService ServiceObject2 = new PriceuomService();
            //  objItemMaster.cmp_id = cmpid.Trim();
            objItemMaster.cmp_id = cmpid;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objItemMaster.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objItemMaster.itm_code = ItmCode.Trim();
            objItemMaster.Length = float.Parse(length);
            objItemMaster = serviceobject.GetItemMasterViewDetails(objItemMaster);
            objLookUp.id = "8";
            objLookUp.lookuptype = "VAS";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objItemMaster.ListLookUpCategoryDtl = objLookUp.ListLookUpDtl;
            objItemMaster.cmp_id = objItemMaster.ListItemMasterViewDtl[0].cmp_id;
            objItemMaster.itm_num = objItemMaster.ListItemMasterViewDtl[0].itm_num;
            objItemMaster.catg_id = objItemMaster.ListItemMasterViewDtl[0].catg_id;
            objItemMaster.Status = objItemMaster.ListItemMasterViewDtl[0].Status;
            objItemMaster.itm_name = objItemMaster.ListItemMasterViewDtl[0].itm_name;
            objItemMaster.itm_size = Size;
            objItemMaster.itm_color = Colour;
            objItemMaster.itm_code = objItemMaster.ListItemMasterViewDtl[0].itm_code;
            objItemMaster.Length = objItemMaster.ListItemMasterViewDtl[0].Length;
            objItemMaster.Cube = objItemMaster.ListItemMasterViewDtl[0].Cube;
            objItemMaster.Depth = objItemMaster.ListItemMasterViewDtl[0].Depth;
            objItemMaster.group_id = objItemMaster.ListItemMasterViewDtl[0].group_id;
            objItemMaster.Class = objItemMaster.ListItemMasterViewDtl[0].Class;
            objItemMaster.wgt = objItemMaster.ListItemMasterViewDtl[0].wgt;
            objItemMaster.Width = float.Parse(Width);
            objItemMaster.Opt_id = 2;
            objItemMaster.Status = staus.Trim();
            objLookUp.id = "11";
            objLookUp.lookuptype = "RATESTATUS";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objItemMaster.ListLookUpStatusDtl = objLookUp.ListLookUpDtl;
            objItemMaster.ListBintatus = objLookUp.ListLookUpDtl;
            objItemMaster.itm_name = objItemMaster.ListItemMasterViewDtl[0].itm_name;

            objItemMaster.sku_id = objItemMaster.ListItemMasterViewDtl[0].sku_id;
            objItemMaster.list_price = objItemMaster.ListItemMasterViewDtl[0].list_price;
            objItemMaster.ctn_qty = objItemMaster.ListItemMasterViewDtl[0].ctn_qty;
            objItemMaster.is_inner_pack = objItemMaster.ListItemMasterViewDtl[0].is_inner_pack;
            objItemMaster.inner_pack_length = objItemMaster.ListItemMasterViewDtl[0].inner_pack_length;
            objItemMaster.inner_pack_width = objItemMaster.ListItemMasterViewDtl[0].inner_pack_width;
            objItemMaster.inner_pack_depth = objItemMaster.ListItemMasterViewDtl[0].inner_pack_depth;
            objItemMaster.inner_pack_cube = objItemMaster.ListItemMasterViewDtl[0].inner_pack_cube;
            objItemMaster.inner_pack_wgt = objItemMaster.ListItemMasterViewDtl[0].inner_pack_wgt;
            objItemMaster.inner_pack_ctn_qty = objItemMaster.ListItemMasterViewDtl[0].inner_pack_ctn_qty;
            objItemMaster.pkg_type = objItemMaster.ListItemMasterViewDtl[0].pkg_type;
            objItemMaster.pkg_size = objItemMaster.ListItemMasterViewDtl[0].pkg_size;

            objItemMaster.pce_length = objItemMaster.ListItemMasterViewDtl[0].pce_length;
            objItemMaster.pce_width = objItemMaster.ListItemMasterViewDtl[0].pce_width;
            objItemMaster.pce_depth = objItemMaster.ListItemMasterViewDtl[0].pce_depth;
            objItemMaster.pce_cube = objItemMaster.ListItemMasterViewDtl[0].pce_cube;
            objItemMaster.pce_wgt = objItemMaster.ListItemMasterViewDtl[0].pce_wgt;

            objItemMaster.image_name = objItemMaster.ListItemMasterViewDtl[0].image_name != null ? objItemMaster.ListItemMasterViewDtl[0].image_name : string.Empty;
            if (objItemMaster.image_name.Length > 0)
            {
                objItemMaster.image_name = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["StyleImagePath"].ToString().Trim(), objItemMaster.image_name);
            }

            objItemMaster.ListPkgType = serviceobject.fnGetItemConfig("PKG_TYPE").ListItemConfig;
            objItemMaster.ListPkgPoly = serviceobject.fnGetItemConfig("PKG-POLY").ListItemConfig;
            objItemMaster.ListPkgBox = serviceobject.fnGetItemConfig("PKG-BOX").ListItemConfig;
            objItemMaster.ListPkgSrvc = serviceobject.fnGetItemPkgServiceg(cmpid, ItmCode).ListPkgSrvc;

            objItemMaster = serviceobject.GetItemMasterViewDetails(objItemMaster);
            objLookUp = serviceobject.GetItemMasterCategory(objLookUp);
            objItemMaster.ListLookUpCategoryDtl = objLookUp.ListLookUpCategoryDtl;
            objItemMaster.ListItemStock = serviceobject.getItemStock(objItemMaster.cmp_id, objItemMaster.itm_num, objItemMaster.itm_color, objItemMaster.itm_size,"OPEN");


            objLookUp = new LookUp();
            LookUpService ServiceObjects = new LookUpService();
            objLookUp.id = "104";
            objLookUp.lookuptype = "BIN_SIZE";
            objLookUp = ServiceObjects.GetLookUpValue(objLookUp);
            objItemMaster.ListLookUpBinType = objLookUp.ListLookUpDtl;

            PickService ServiceObjectPick = new PickService();
            Pick objPick = new Pick();
            ServiceObjectPick = new PickService();
            objPick.cmp_id = String.Empty;
            objPick = ServiceObjectPick.GetPickUom(objPick);

            objItemMaster.ListQtyUoM = objPick.ListExistShipToAddrsPick;


            objPick.cmp_id = cmpid;
            objPick.Whs_id = "";
            objPick.Whs_name = "";
            objPick = ServiceObjectPick.GetWhsPick(objPick);
            objItemMaster.ListWhs = objPick.ListPick;
            clsBinMater objBinMater = new clsBinMater();
          
         

            if (String.IsNullOrEmpty(p_str_bin_id))
                {
                objBinMater.bin_type = "Mid";
                objBinMater.bin_length = 25.00M;
                objBinMater.bin_width = 19.00M;
                objBinMater.bin_height = 49.00M;
                objBinMater.bin_cube = 13.50M;
                if (objItemMaster.ListWhs.Count > 0)
                {
                    objBinMater.whs_id = objItemMaster.ListWhs[0].Whs_id.Trim();
                    objItemMaster.objBinMater = objBinMater;

                }
            }
            else
            {
                BinMaster objBinMaster = new BinMaster();
                IBinMasterService ServiceObjectBin = new BinMasterService();
                objBinMaster.objBinMater = ServiceObjectBin.fnGetBinMaster(cmpid, p_str_bin_id).ListBinMasterinqury[0];
                objItemMaster.objBinMater = objBinMaster.objBinMater;
            }
           


            Mapper.CreateMap<ItemMaster, ItemMasterModel>();
            ItemMasterModel objItemMastermodel = Mapper.Map<ItemMaster, ItemMasterModel>(objItemMaster);
            return PartialView("_ItemMasterEdit", objItemMastermodel);
        }
        public ActionResult fnGetItemStock(string pstrCmpid, string pstrItmCode, string pstrStkStaus)
        {
            ItemMaster objItemMaster = new ItemMaster();
            ItemMasterService serviceobject = new ItemMasterService();
            objItemMaster.cmp_id = pstrCmpid;
            objItemMaster.itm_code = pstrItmCode;
            objItemMaster = serviceobject.GetItemMasterViewDetails(objItemMaster);

            objItemMaster.ListItemStock = serviceobject.getItemStock(objItemMaster.cmp_id, objItemMaster.ListItemMasterViewDtl[0].itm_num, objItemMaster.ListItemMasterViewDtl[0].itm_color, objItemMaster.ListItemMasterViewDtl[0].itm_size, pstrStkStaus);
            Mapper.CreateMap<ItemMaster, ItemMasterModel>();
            ItemMasterModel objItemMastermodel = Mapper.Map<ItemMaster, ItemMasterModel>(objItemMaster);
            return PartialView("_ItemMasterStk", objItemMastermodel);
        }


            public JsonResult MASTER_INQ_HDR_DATA(string p_str_cmp_id, string p_str_itm_style, string p_str_itm_Color, string p_str_itm_Size, string p_str_itm_Descr)
        {
            ItemMaster objItemMaster = new ItemMaster();
            IItemMasterService ServiceObject = new ItemMasterService();
            Session["g_str_cmp_id"] = p_str_cmp_id;
            Session["TEMP_CMP_ID"] = p_str_cmp_id;
            Session["TEMP_itm_style"] = p_str_itm_style;
            Session["TEMP_itm_Color"] = p_str_itm_Color;
            Session["TEMP_itm_Size"] = p_str_itm_Size;
            Session["TEMP_itm_Descr"] = p_str_itm_Descr;
            return Json(objItemMaster.MasterCount, JsonRequestBehavior.AllowGet);

        }
        public ActionResult NewItemMaster(string p_str_cmp_id,
            string p_str_itm_num, string p_str_catg, string p_str_itm_code,string p_str_itm_color, string p_str_status, string p_str_itm_name, 
            string p_str_itm_size, string p_str_group_id, string p_str_Length,
            string p_str_Width, string p_str_Depth, decimal p_str_Cube, string p_str_Weight, bool KitItem, bool NonInventory,
            string sku_id, decimal list_price, int ctn_qty, bool is_inner_pack,
            decimal inner_pack_length, decimal inner_pack_width, decimal inner_pack_depth, decimal inner_pack_cube,
            decimal inner_pack_wgt, int inner_pack_ctn_qty, string img_name, string p_str_pkg_type, string p_str_pkg_size, string pkg_service,
            decimal pdecPcsLength, decimal pdecPcsWidth , decimal pdecPcsDepth, decimal pdecPcsCube, decimal pdecPcsWgt,  clsBinMater objBinMaster)
        {
            ItemMaster objItemMaster = new ItemMaster();
            ItemMasterService serviceobject = new ItemMasterService();
            objItemMaster.cmp_id = p_str_cmp_id.Trim();
           // objItemMaster.Str_cmp_id = p_Str_cmp_ids.Trim();
            objItemMaster.itm_num = p_str_itm_num.Trim();
            objItemMaster.catg_id = p_str_catg.Trim();
            objItemMaster.Status = p_str_status.Trim();
            objItemMaster.itm_name = p_str_itm_name.Trim();
            objItemMaster.itm_size = p_str_itm_size.Trim();
            objItemMaster.itm_color = p_str_itm_color.Trim();


            if (String.IsNullOrEmpty(objBinMaster.bin_id))
            {

            }
            else
            {

                IBinMasterService ServiceObject = new BinMasterService();

                int lintBinCount = ServiceObject.fnCheckBinMasterExists(p_str_cmp_id, objBinMaster.bin_id);
                if (lintBinCount > 0)
                {
                    return Json(5, JsonRequestBehavior.AllowGet);
                }
            }

                if (p_str_itm_code == string.Empty)
            {
                p_str_itm_code = Session["sesItmCode"].ToString();
            }
            

            objItemMaster.itm_code = p_str_itm_code;
            objItemMaster.group_id = p_str_group_id.Trim();
            objItemMaster.Length = float.Parse(p_str_Length.Trim());
            objItemMaster.Width = float.Parse(p_str_Width.Trim());
            objItemMaster.Depth = float.Parse(p_str_Depth.Trim());
            objItemMaster.Cube = p_str_Cube;
            objItemMaster.wgt = float.Parse(p_str_Weight.Trim());
            objItemMaster.KitItem= KitItem;
           if(objItemMaster.Kit_Itm==true)
            {
                objItemMaster.itm_size = "-";
                objItemMaster.itm_color = "-";
            }
            objItemMaster.NonInventorysItem = NonInventory;
            objItemMaster.Opt_id = 1;
            l_str_RateId = 1;
            //CR-180421-001 Added By Nithya
            objItemMaster = serviceobject.ExistItem(objItemMaster);
            if (objItemMaster.LstItemId.Count > 0)
            {
                 l_str_RateId = 0;             
            }
            objItemMaster.sku_id = sku_id;
            objItemMaster.list_price = list_price;
            objItemMaster.ctn_qty = ctn_qty;
            objItemMaster.is_inner_pack = is_inner_pack;
            objItemMaster.inner_pack_length = inner_pack_length;
            objItemMaster.inner_pack_width = inner_pack_width;
            objItemMaster.inner_pack_depth = inner_pack_depth;
            objItemMaster.inner_pack_cube = inner_pack_cube;
            objItemMaster.inner_pack_wgt = inner_pack_wgt;
            objItemMaster.inner_pack_ctn_qty = inner_pack_ctn_qty;
            objItemMaster.pkg_type = p_str_pkg_type;
            objItemMaster.pkg_size = p_str_pkg_size;


            objItemMaster.pce_length = pdecPcsLength;
            objItemMaster.pce_width = pdecPcsWidth;
            objItemMaster.pce_depth = pdecPcsDepth;
            objItemMaster.pce_cube = pdecPcsCube;
            objItemMaster.pce_wgt = pdecPcsWgt;


            objItemMaster.image_name = img_name;
            if (Session["uploadImageName"] != null)
            {
                objItemMaster.image_name= SaveImage(p_str_cmp_id.Trim(), p_str_itm_code);
            }

            if ((pkg_service == null) || (pkg_service == string.Empty))
            {
                serviceobject.SaveSaveItemPkgServicer("D", p_str_cmp_id, p_str_itm_code, string.Empty);
            }
            else
            {
                serviceobject.SaveSaveItemPkgServicer("A", p_str_cmp_id, p_str_itm_code, pkg_service);
            }

            serviceobject.ItemMasterHeaderCreateUpdate(objItemMaster);
            serviceobject.ItemMasterCreateUpdate(objItemMaster);

            if (String.IsNullOrEmpty(objBinMaster.bin_id) )
            {

            }
            else
            {

                IBinMasterService ServiceObject = new BinMasterService();

                int lintBinCount = ServiceObject.fnCheckBinMasterExists(p_str_cmp_id, objBinMaster.bin_id);
                if (lintBinCount > 0)
                {
                    return Json(5, JsonRequestBehavior.AllowGet);
                }


            LocationMaster objLocationMaster = new LocationMaster();
            ILocationMasterService ObjService = new LocationMasterService();
            objLocationMaster.cmp_id = objBinMaster.cmp_id;
            objLocationMaster.whs_id = objBinMaster.whs_id;
            objLocationMaster.loc_id = objBinMaster.bin_loc;


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

                if (String.IsNullOrEmpty(objBinMaster.bin_id))
                {
                  
                }
                   else
                {
                    objBinMaster.itm_code = objItemMaster.itm_code;
                    objBinMaster.user_id = Session["UserID"].ToString().Trim();
                    objBinMaster.bin_dt = System.DateTime.Now.ToShortDateString();
                    objBinMaster.process_id = "A";
                    ServiceObject.SaveBinMaster(objBinMaster, "A");
                }


            }
            Session["uploadImageName"] = null;


            return Json(l_str_RateId, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ItemMasterEdit(string p_str_cmp_id,
            string p_str_itm_num, string p_str_catg, string p_str_itm_code, string p_str_itm_color, string p_str_status, string p_str_itm_name,
            string p_str_itm_size, string p_str_group_id, string p_str_Length,
            string p_str_Width, string p_str_Depth, decimal p_str_Cube, string p_str_Weight, bool KitItem, bool NonInventory,
            string sku_id, decimal list_price, int ctn_qty, bool is_inner_pack,
            decimal inner_pack_length, decimal inner_pack_width, decimal inner_pack_depth, decimal inner_pack_cube,
            decimal inner_pack_wgt, int inner_pack_ctn_qty, string txt_img_name,string p_str_pkg_type, string p_str_pkg_size, string pkg_service,
            decimal pdecPcsLength, decimal pdecPcsWidth, decimal pdecPcsDepth, decimal pdecPcsCube, decimal pdecPcsWgt,
            List<ItemStockDimUpdate> ListItemStockDimUpdate, clsBinMater objBinMaster, bool delete_bin)
        {

            ItemMaster objItemMaster = new ItemMaster();
            ItemMasterService serviceobject = new ItemMasterService();
            objItemMaster.cmp_id = p_str_cmp_id.Trim();
            // objItemMaster.Str_cmp_id = p_Str_cmp_ids.Trim();
            objItemMaster.itm_num = p_str_itm_num.Trim();
            objItemMaster.catg_id = p_str_catg.Trim();
            objItemMaster.Status = p_str_status.Trim();
            objItemMaster.itm_name = p_str_itm_name.Trim();
            objItemMaster.itm_size = p_str_itm_size.Trim();
            objItemMaster.itm_color = p_str_itm_color.Trim();
            objItemMaster.itm_code = p_str_itm_code;
            objItemMaster.group_id = p_str_group_id.Trim();
            objItemMaster.Length = float.Parse(p_str_Length.Trim());
            objItemMaster.Width = float.Parse(p_str_Width.Trim());
            objItemMaster.Depth = float.Parse(p_str_Depth.Trim());
            objItemMaster.wgt = float.Parse(p_str_Weight.Trim());
            objItemMaster.Cube = p_str_Cube;
            objItemMaster.KitItem = KitItem;
            if (objItemMaster.Kit_Itm == true)
            {
                objItemMaster.itm_size = "-";
                objItemMaster.itm_color = "-";
            }
            objItemMaster.NonInventorysItem = NonInventory;
            objItemMaster.Opt_id = 2;
            l_str_RateId = 1;

            objItemMaster.sku_id = sku_id;
            objItemMaster.list_price = list_price;
            objItemMaster.ctn_qty = ctn_qty;
            objItemMaster.is_inner_pack = is_inner_pack;
            objItemMaster.inner_pack_length = inner_pack_length;
            objItemMaster.inner_pack_width = inner_pack_width;
            objItemMaster.inner_pack_depth = inner_pack_depth;
            objItemMaster.inner_pack_cube = inner_pack_cube;
            objItemMaster.inner_pack_wgt = inner_pack_wgt;
            objItemMaster.inner_pack_ctn_qty = inner_pack_ctn_qty;
            objItemMaster.image_name = txt_img_name;
            objItemMaster.pkg_type = p_str_pkg_type;
            objItemMaster.pkg_size = p_str_pkg_size;

            objItemMaster.pce_length = pdecPcsLength;
            objItemMaster.pce_width = pdecPcsWidth;
            objItemMaster.pce_depth = pdecPcsDepth;
            objItemMaster.pce_cube = pdecPcsCube;
            objItemMaster.pce_wgt = pdecPcsWgt;

            if (Session["uploadImageName"] != null)
            {
                objItemMaster.image_name = SaveImage(p_str_cmp_id.Trim(), p_str_itm_code);
            }
            if ((pkg_service == null) || (pkg_service == string.Empty))
            {
                serviceobject.SaveSaveItemPkgServicer("D", p_str_cmp_id, p_str_itm_code, string.Empty);
            }
            else
            {
                serviceobject.SaveSaveItemPkgServicer("M", p_str_cmp_id, p_str_itm_code, pkg_service);
            }
            Session["uploadImageName"] = null;
            serviceobject.ItemMasterHeaderCreateUpdate(objItemMaster);
            serviceobject.ItemMasterCreateUpdate(objItemMaster);
            if (ListItemStockDimUpdate != null)
            { 
            DataTable ldtInvMergeCtns;
            ldtInvMergeCtns = new DataTable();
            ldtInvMergeCtns = Utility.ConvertListToDataTable(ListItemStockDimUpdate);
                serviceobject.SaveDimUpdate(p_str_cmp_id, ldtInvMergeCtns);
            }




            if (String.IsNullOrEmpty(objBinMaster.bin_id))
            {

            }
            else
            {
                string pstrMode = string.Empty;
                IBinMasterService ServiceObject = new BinMasterService();

                int lintBinCount = ServiceObject.fnCheckBinMasterExists(p_str_cmp_id, objBinMaster.bin_id);
                if (lintBinCount > 0)
                {
                    pstrMode = "M";
                }
                else
                {
                    pstrMode = "A";
                }


                LocationMaster objLocationMaster = new LocationMaster();
                ILocationMasterService ObjService = new LocationMasterService();
                objLocationMaster.cmp_id = objBinMaster.cmp_id;
                objLocationMaster.whs_id = objBinMaster.whs_id;
                objLocationMaster.loc_id = objBinMaster.bin_loc;


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


            }


            return Json(l_str_RateId, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemView(string cmpid, string staus, string ItmCode, string length, string Width, string Weight, string p_str_bin_id)
        {
            ItemMaster objItemMaster = new ItemMaster();
            ItemMasterService serviceobject = new ItemMasterService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            IItemMasterService ServiceObject = new ItemMasterService();
            LookUp objLookUp = new LookUp();
            LookUpService ServiceObject1 = new LookUpService();
            Price objPrice = new Price();
            PriceuomService ServiceObject2 = new PriceuomService();
            //  objItemMaster.cmp_id = cmpid.Trim();
            objItemMaster.cmp_id = cmpid;
            objItemMaster.cmp_id = cmpid;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objItemMaster.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objItemMaster.itm_code = ItmCode.Trim();
            objItemMaster.Length = float.Parse(length);
            objItemMaster = serviceobject.GetItemMasterViewDetails(objItemMaster);
            objLookUp.id = "8";
            objLookUp.lookuptype = "VAS";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objItemMaster.ListLookUpCategoryDtl = objLookUp.ListLookUpDtl;
            objItemMaster.cmp_id = objItemMaster.ListItemMasterViewDtl[0].cmp_id;
            objItemMaster.itm_num = objItemMaster.ListItemMasterViewDtl[0].itm_num;
            objItemMaster.catg_id = objItemMaster.ListItemMasterViewDtl[0].catg_id;
            objItemMaster.Status = objItemMaster.ListItemMasterViewDtl[0].Status;
            objItemMaster.itm_name = objItemMaster.ListItemMasterViewDtl[0].itm_name;
            objItemMaster.itm_size = objItemMaster.ListItemMasterViewDtl[0].itm_size;
            objItemMaster.itm_color = objItemMaster.ListItemMasterViewDtl[0].itm_color;
            objItemMaster.itm_code = objItemMaster.ListItemMasterViewDtl[0].itm_code;
            objItemMaster.Status = objItemMaster.ListItemMasterViewDtl[0].Status;
            objItemMaster.Length = objItemMaster.ListItemMasterViewDtl[0].Length;
            objItemMaster.Cube = objItemMaster.ListItemMasterViewDtl[0].Cube;
            objItemMaster.Depth = objItemMaster.ListItemMasterViewDtl[0].Depth;

            objItemMaster.pce_length = objItemMaster.ListItemMasterViewDtl[0].pce_length; 
            objItemMaster.pce_width = objItemMaster.ListItemMasterViewDtl[0].pce_width; 
            objItemMaster.pce_depth = objItemMaster.ListItemMasterViewDtl[0].pce_depth; 
            objItemMaster.pce_cube = objItemMaster.ListItemMasterViewDtl[0].pce_cube; 
            objItemMaster.pce_wgt = objItemMaster.ListItemMasterViewDtl[0].pce_wgt; 

            objItemMaster.group_id = objItemMaster.ListItemMasterViewDtl[0].group_id;
            objItemMaster.Class = objItemMaster.ListItemMasterViewDtl[0].Class;
            objItemMaster.Weight = float.Parse(Weight);
            objItemMaster.Width = float.Parse(Width);
            objItemMaster.wgt = objItemMaster.ListItemMasterViewDtl[0].wgt;
            objItemMaster.Opt_id = 2;
            objItemMaster.Status = staus.Trim();
            objLookUp.id = "11";
            objLookUp.lookuptype = "RATESTATUS";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objItemMaster.ListLookUpStatusDtl = objLookUp.ListLookUpDtl;
            objItemMaster.itm_name = objItemMaster.ListItemMasterViewDtl[0].itm_name;
            objItemMaster.sku_id = objItemMaster.ListItemMasterViewDtl[0].sku_id;
            objItemMaster.list_price = objItemMaster.ListItemMasterViewDtl[0].list_price;
            objItemMaster.ctn_qty = objItemMaster.ListItemMasterViewDtl[0].ctn_qty;
            objItemMaster.is_inner_pack = objItemMaster.ListItemMasterViewDtl[0].is_inner_pack;
            objItemMaster.inner_pack_length = objItemMaster.ListItemMasterViewDtl[0].inner_pack_length;
            objItemMaster.inner_pack_width = objItemMaster.ListItemMasterViewDtl[0].inner_pack_width;
            objItemMaster.inner_pack_depth = objItemMaster.ListItemMasterViewDtl[0].inner_pack_depth;
            objItemMaster.inner_pack_cube = objItemMaster.ListItemMasterViewDtl[0].inner_pack_cube;
            objItemMaster.inner_pack_wgt = objItemMaster.ListItemMasterViewDtl[0].inner_pack_wgt;
            objItemMaster.inner_pack_ctn_qty = objItemMaster.ListItemMasterViewDtl[0].inner_pack_ctn_qty;
            objItemMaster.pkg_type = objItemMaster.ListItemMasterViewDtl[0].pkg_type;
            objItemMaster.pkg_size = objItemMaster.ListItemMasterViewDtl[0].pkg_size;
            objItemMaster.image_name = objItemMaster.ListItemMasterViewDtl[0].image_name;
            objItemMaster.image_name = objItemMaster.ListItemMasterViewDtl[0].image_name != null ? objItemMaster.ListItemMasterViewDtl[0].image_name : string.Empty;
            if (objItemMaster.image_name.Length > 0)
            {
                objItemMaster.image_name = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["StyleImagePath"].ToString().Trim(), objItemMaster.image_name);
            }
            objItemMaster.ListPkgType = serviceobject.fnGetItemConfig("PKG_TYPE").ListItemConfig;
            objItemMaster.ListPkgPoly = serviceobject.fnGetItemConfig("PKG-POLY").ListItemConfig;
            objItemMaster.ListPkgBox = serviceobject.fnGetItemConfig("PKG-BOX").ListItemConfig;
            objItemMaster.ListPkgSrvc = serviceobject.fnGetItemPkgServiceg(cmpid, ItmCode).ListPkgSrvc;

            objItemMaster = serviceobject.GetItemMasterViewDetails(objItemMaster);
            objLookUp = serviceobject.GetItemMasterCategory(objLookUp);
            objItemMaster.ListLookUpCategoryDtl = objLookUp.ListLookUpCategoryDtl;
            objItemMaster.ListItemStock= serviceobject.getItemStock(objItemMaster.cmp_id ,objItemMaster.itm_num, objItemMaster.itm_color, objItemMaster.itm_size,"OPEN");

            objLookUp = new LookUp();
            LookUpService ServiceObjects = new LookUpService();
            objLookUp.id = "104";
            objLookUp.lookuptype = "BIN_SIZE";
            objLookUp = ServiceObjects.GetLookUpValue(objLookUp);
            objItemMaster.ListLookUpBinType = objLookUp.ListLookUpDtl;

            PickService ServiceObjectPick = new PickService();
            Pick objPick = new Pick();
            ServiceObjectPick = new PickService();
            objPick.cmp_id = String.Empty;
            objPick = ServiceObjectPick.GetPickUom(objPick);

            objItemMaster.ListQtyUoM = objPick.ListExistShipToAddrsPick;


            objPick.cmp_id = cmpid;
            objPick.Whs_id = "";
            objPick.Whs_name = "";
            objPick = ServiceObjectPick.GetWhsPick(objPick);
            objItemMaster.ListWhs = objPick.ListPick;


           

            if (String.IsNullOrEmpty(p_str_bin_id))
            {
                clsBinMater objBinMater = new clsBinMater();
                IBinMasterService ServiceObjectBin = new BinMasterService();
              
                objItemMaster.objBinMater = objBinMater;

            }
            else
            {
                BinMaster objBinMaster = new BinMaster();
                IBinMasterService ServiceObjectBin = new BinMasterService();
                objBinMaster.objBinMater = ServiceObjectBin.fnGetBinMaster(cmpid, p_str_bin_id).ListBinMasterinqury[0];
                objItemMaster.objBinMater = objBinMaster.objBinMater;
            }


            Mapper.CreateMap<ItemMaster, ItemMasterModel>();
            ItemMasterModel objItemMastermodel = Mapper.Map<ItemMaster, ItemMasterModel>(objItemMaster);
            return PartialView("_ItemMasterView", objItemMastermodel);
        }

        public ActionResult ItemDelete(string cmpid, string staus, string ItmCode, string length, string Width, string Weight)
        {
            ItemMaster objItemMaster = new ItemMaster();
            ItemMasterService serviceobject = new ItemMasterService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            IItemMasterService ServiceObject = new ItemMasterService();
            LookUp objLookUp = new LookUp();
            LookUpService ServiceObject1 = new LookUpService();
            Price objPrice = new Price();
            PriceuomService ServiceObject2 = new PriceuomService();
            //  objItemMaster.cmp_id = cmpid.Trim();
            objItemMaster.cmp_id = cmpid;
            objItemMaster.cmp_id = cmpid;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objItemMaster.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objItemMaster.itm_code = ItmCode.Trim();
            objItemMaster.Length = float.Parse(length);
            objItemMaster = serviceobject.GetItemMasterViewDetails(objItemMaster);
            objLookUp.id = "8";
            objLookUp.lookuptype = "VAS";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objItemMaster.ListLookUpCategoryDtl = objLookUp.ListLookUpDtl;
            objItemMaster.cmp_id = objItemMaster.ListItemMasterViewDtl[0].cmp_id;
            objItemMaster.itm_num = objItemMaster.ListItemMasterViewDtl[0].itm_num;
            objItemMaster.catg_id = objItemMaster.ListItemMasterViewDtl[0].catg_id;
            objItemMaster.Status = objItemMaster.ListItemMasterViewDtl[0].Status;
            objItemMaster.itm_name = objItemMaster.ListItemMasterViewDtl[0].itm_name;
            objItemMaster.itm_size = objItemMaster.ListItemMasterViewDtl[0].itm_size;
            objItemMaster.itm_color = objItemMaster.ListItemMasterViewDtl[0].itm_color;
            objItemMaster.itm_code = objItemMaster.ListItemMasterViewDtl[0].itm_code;
            objItemMaster.Status = objItemMaster.ListItemMasterViewDtl[0].Status;
            objItemMaster.Length = objItemMaster.ListItemMasterViewDtl[0].Length;
            objItemMaster.Cube = objItemMaster.ListItemMasterViewDtl[0].Cube;
            objItemMaster.Depth = objItemMaster.ListItemMasterViewDtl[0].Depth;
            objItemMaster.group_id = objItemMaster.ListItemMasterViewDtl[0].group_id;
            objItemMaster.Class = objItemMaster.ListItemMasterViewDtl[0].Class; 
            objItemMaster.Weight = float.Parse(Weight);
            objItemMaster.Width = float.Parse(Width);
            objItemMaster.wgt = objItemMaster.ListItemMasterViewDtl[0].wgt;
            objItemMaster.Opt_id = 3;
            objItemMaster.Status = staus.Trim();
            objLookUp.id = "11";
            objLookUp.lookuptype = "RATESTATUS";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
            objItemMaster.ListLookUpStatusDtl = objLookUp.ListLookUpDtl;
            objItemMaster.itm_name = objItemMaster.ListItemMasterViewDtl[0].itm_name;
            objItemMaster = serviceobject.GetItemMasterViewDetails(objItemMaster);
            objLookUp = serviceobject.GetItemMasterCategory(objLookUp);
            objItemMaster.ListLookUpCategoryDtl = objLookUp.ListLookUpCategoryDtl;
            objItemMaster.pkg_type = objItemMaster.ListItemMasterViewDtl[0].pkg_type;
            objItemMaster.pkg_size = objItemMaster.ListItemMasterViewDtl[0].pkg_size;
            objItemMaster.ListPkgType = serviceobject.fnGetItemConfig("PKG_TYPE").ListItemConfig;
            objItemMaster.ListPkgPoly = serviceobject.fnGetItemConfig("PKG-POLY").ListItemConfig;
            objItemMaster.ListPkgBox = serviceobject.fnGetItemConfig("PKG-BOX").ListItemConfig;
            objItemMaster.ListPkgSrvc = serviceobject.fnGetItemPkgServiceg(cmpid, ItmCode).ListPkgSrvc;
            Mapper.CreateMap<ItemMaster, ItemMasterModel>();
            ItemMasterModel objItemMastermodel = Mapper.Map<ItemMaster, ItemMasterModel>(objItemMaster);
            return PartialView("_ItemMasterDelete", objItemMastermodel);
        }

        public ActionResult ItemMasterDelete(string p_str_cmp_id,string p_str_itm_code)
        {

            ItemMaster objItemMaster = new ItemMaster();
            ItemMasterService serviceobject = new ItemMasterService();
            objItemMaster.cmp_id = p_str_cmp_id.Trim();
            objItemMaster.itm_code = p_str_itm_code;
            objItemMaster.Opt_id = 3;
            objItemMaster = serviceobject.CheckAndDeleteItm(objItemMaster);
            if(objItemMaster.LstItemId.Count>0)
            {
                l_str_RateId = 0;
                return Json(l_str_RateId, JsonRequestBehavior.AllowGet);
            }
            else
            {
                serviceobject.ItemMasterdelete(objItemMaster);
            }         
            l_str_RateId = 1;
            return Json(l_str_RateId, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemMasterExcelReport(string ReportId, string p_str_cmp_id, string ReportType)
        {
            try
            {
                if (ReportType == "EXCEL")
                {
                    string tempFileName = string.Empty;
                    string l_str_file_name = string.Empty;
                    string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                    string strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
                    ItemMasterService serviceobject = new ItemMasterService();
                    ItemdtlReport objItemdtlReport = new ItemdtlReport();
                    DataTable dtBill = new DataTable();

                    if (ReportId == "ItemDetails")
                    {
                        dtBill = serviceobject.GetItemDtlListExcelRpt(p_str_cmp_id);

                        if (!Directory.Exists(strOutputpath))
                        {
                            Directory.CreateDirectory(strOutputpath);
                        }

                        l_str_file_name = "DF_" + p_str_cmp_id.ToUpper().ToString().Trim() + "_ITEM_DETAILS_" + strDateFormat + ".xlsx";

                        tempFileName = strOutputpath + l_str_file_name;

                        if (System.IO.File.Exists(tempFileName))
                            System.IO.File.Delete(tempFileName);
                        xls_Item_Details_Excel mxcel  = new xls_Item_Details_Excel(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "ITEM_DETAILS_RPT.xlsx");
                        var dataRows = dtBill.Rows;
                        DataRow dr = dataRows[0];

                        mxcel.PopulateHeader(p_str_cmp_id);
                        mxcel.PopulateData(dtBill, true);
                        mxcel.SaveAs(tempFileName);
                        FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                        return File(fs, "application / xlsx", l_str_file_name);
                    }

                    else if (ReportId == "ItemDimCmpRpt")
                    {
                      

                        dtBill = serviceobject.GetItemAndStockDimRpt(p_str_cmp_id);

                        if (!Directory.Exists(strOutputpath))
                        {
                            Directory.CreateDirectory(strOutputpath);
                        }

                        l_str_file_name = "DF_" + p_str_cmp_id.ToUpper().ToString().Trim() + "-ITEM-AND-STOCK-DIMS-COMP-RPT" + strDateFormat + ".xlsx";

                        tempFileName = strOutputpath + l_str_file_name;

                        if (System.IO.File.Exists(tempFileName))
                            System.IO.File.Delete(tempFileName);
                        xls_Item_and_Stock_Dim_Comp mxcel = new xls_Item_and_Stock_Dim_Comp(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "ITEM_STK_DIM_COMP_RPT.xlsx");
                        var dataRows = dtBill.Rows;
                        DataRow dr = dataRows[0];

                        mxcel.PopulateHeader(p_str_cmp_id);
                        mxcel.PopulateData(dtBill, true);
                        mxcel.SaveAs(tempFileName);
                        FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                        return File(fs, "application / xlsx", l_str_file_name);
                    }
                    else
                    {
                        return Json(string.Empty, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(string.Empty, JsonRequestBehavior.AllowGet);
                }
            }
            
            catch (Exception EX)
            {
                return Json(EX.InnerException.ToString(), JsonRequestBehavior.AllowGet);
            }
           finally
            {

            }


        }

        public ActionResult IncludeImage(string p_str_cmp_id, string p_str_itm_code, 
        string p_str_img_file_name,  string p_str_img_type, string p_str_img_order, string p_str_img_desc)
        {
            itmImage objItmImage = new itmImage();
            ItemMaster objItemMaster = new ItemMaster();
            objItmImage.cmp_id = p_str_cmp_id;
            objItmImage.itm_code = p_str_itm_code;

            objItmImage.img_type = p_str_img_type;
            objItmImage.img_order = Convert.ToInt32(p_str_img_order);
            objItmImage.img_desc = p_str_img_desc;
            string fileName = Path.GetFileName(p_str_img_file_name);
            string directoryPath = Path.Combine(Server.MapPath("~/Images/zfolderimage"), p_str_cmp_id);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(Path.Combine(directoryPath));
            }
            string newName = System.IO.Path.GetFileNameWithoutExtension(fileName);
            newName = newName + ".jpg";
            string temppath = Path.Combine(directoryPath, newName);
            objItmImage.img_path = temppath;
           
           // objStyleInquiry = ServiceObject.InsertStyleHeaderandDetailsValuesTemptable(objStyleInquiry);
            Mapper.CreateMap<ItemMaster, ItemMasterModel>();
            ItemMasterModel objItemMasterModel = Mapper.Map<ItemMaster, ItemMasterModel>(objItemMaster);
            return PartialView("_GrdItmImage", objItemMasterModel);

        }

        public ActionResult UploadFiles(string pstrCmpId, string pstrItmNum, string pstrItmCode)
        {
            int l_str_ResultCount = 0;
            string l_str_tmp_directoryPath = string.Empty;
            string l_str_tmp_file_name = string.Empty;
            string l_str_tmp_full_file_path = string.Empty;
            string lstrItemName = string.Empty;
            lstrItemName = pstrCmpId + "-" + pstrItmCode ;
            
                lstrItemName = lstrItemName + "-" + pstrItmNum.Trim();
           

            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase FileUpload = files[0];

                    string l_str_error_msg = string.Empty;
                    if (FileUpload != null)
                    {
                        if (FileUpload.ContentLength > 0)
                        {
                            //Workout our file path
                            l_str_tmp_file_name = Path.GetFileName(FileUpload.FileName);
                            lstrItemName = lstrItemName + Path.GetExtension(FileUpload.FileName);
                            Session["uploadImageName"] = lstrItemName;
                           
                            l_str_tmp_directoryPath = Path.Combine(Server.MapPath("~/Images/zfolderimage"), pstrCmpId);

                            if (!Directory.Exists(l_str_tmp_directoryPath))
                            {
                                Directory.CreateDirectory(Path.Combine(l_str_tmp_directoryPath));
                            }

                            l_str_tmp_full_file_path = Path.Combine(l_str_tmp_directoryPath, lstrItemName);
                            try
                            {
                                l_str_ResultCount = 1;

                                FileUpload.SaveAs(l_str_tmp_full_file_path);
                            }
                            catch (Exception ex)
                            {
                               
                            }

                        }

                    }
                    else
                    {
                        return Json(l_str_ResultCount, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception ex)
                {
                    return Json(l_str_ResultCount, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                return Json(l_str_ResultCount, JsonRequestBehavior.AllowGet);
            }
            return Json(l_str_ResultCount, JsonRequestBehavior.AllowGet);
        }

        public string SaveImage(string pstrCmpId, string pstrItmCode)
        {
            string strSourceFolder = Path.Combine(Server.MapPath("~/Images/zfolderimage"));
            strSourceFolder = Path.Combine(strSourceFolder, pstrCmpId, "IMAGE");
            string lstrImageName = Session["uploadImageName"].ToString();
            System.IO.FileInfo fileImageName = new FileInfo(Path.Combine(strSourceFolder, lstrImageName));

            if (fileImageName.Exists)
            {

                string imagePath =  System.Configuration.ConfigurationManager.AppSettings["StyleImagePath"].ToString().Trim();
                string strDestinationFolder = Path.Combine(imagePath, pstrCmpId, "IMAGE") + "/";

                if (!Directory.Exists(strDestinationFolder))
                {
                    Directory.CreateDirectory(Path.Combine(strDestinationFolder));
                }
                strDestinationFolder = Path.Combine(strDestinationFolder, pstrItmCode.Substring(0, 4));

                if (!Directory.Exists(strDestinationFolder))
                { 
                    Directory.CreateDirectory(strDestinationFolder);
                }
                fileImageName.CopyTo(Path.Combine(strDestinationFolder, lstrImageName.Trim()), true);

                Directory.Delete(strSourceFolder, true);

                string newName = System.IO.Path.GetFileNameWithoutExtension(lstrImageName);
                string lstrDestinationFolder =  Path.Combine( pstrCmpId, pstrItmCode.Substring(0,4)).ToString().Replace(@"\", @"/") + "/";
                return Path.Combine(lstrDestinationFolder, lstrImageName);

            }
            else
            {

                return lstrImageName;
            }
        }
    }
}