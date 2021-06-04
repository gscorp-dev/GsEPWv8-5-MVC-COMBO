using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class clsItemConfig
    {
        public string config_type { get; set; }
        public string config_name { get; set; }
        public string config_desc { get; set; }
        public string default_yn { get; set; }
        public string status_id { get; set; }
        public string disp_ordr { get; set; }
    }

    public class clsItemPkgService
    {
        public bool pkg_service_exists { get; set; }
        public string pkg_service { get; set; }
    }
    public class ItemMasterdtl : BasicEntity
    {
        public string cmp_id { get; set; }
        public string itm_num { get; set; }
        public string itm_code { get; set; }
        public string itm_size { get; set; }
        public string itm_name { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public float Weight { get; set; }
        public decimal Cube { get; set; }

        public string Status { get; set; }
        public float Depth { get; set; }
        public string Str_cmp_id { get; set; }
        public string MasterCount { get; set; }
        public string group_id { get; set; }
        public bool Kit_Itm { get; set; }
        public string catg_id { get; set; }
        public int Opt_id { get; set; }
        public float wgt { get; set; }
        public string Class { get; set; }
        public bool KitItem { get; set; }
        public string itm_color { get; set; }

        public string itmid { get; set; }
        public bool NonInventorysItem { get; set; }
       
        public string sku_id { get; set; }
        public decimal list_price { get; set; }
        public int ctn_qty { get; set; }
        public bool is_inner_pack { get; set; }
        public int inner_pack_ctn_qty { get; set; }
        public decimal inner_pack_length { get; set; }
        public decimal inner_pack_width { get; set; }
        public decimal inner_pack_depth { get; set; }
        public decimal inner_pack_cube { get; set; }
        public decimal inner_pack_wgt { get; set; }
        public string image_name { get; set; }
        public string pkg_type { get; set; }
        public string pkg_size { get; set; }
        public string pkg_service { get; set; }

        public string bin_id { get; set; }
        public string bin_type { get; set; }
        public decimal bin_length { get; set; }
        public decimal bin_width { get; set; }
        public decimal bin_height { get; set; }
        public decimal bin_cube { get; set; }
        public decimal bin_wgt { get; set; }
        public int bin_ppk { get; set; }


        public decimal pce_length { get; set; }
        public decimal pce_width { get; set; }
        public decimal pce_depth { get; set; }
        public decimal pce_cube { get; set; }
        public decimal pce_wgt { get; set; }
        public bool bln_delete_bin { get; set; }


        public IList<clsItemPkgService> ListItemPkgSrvc { get; set; }
        clsItemConfig objItemConfig { get; set; }



    }


    public class ItemStock
    {
        public string cmp_id { get; set; }
        public string whs_id { get; set; }
        public string itm_code { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string itm_name { get; set; }
        public string ib_doc_id { get; set; }
        public string rcvd_dt { get; set; }
        public string cont_id { get; set; }
        public string lot_id { get; set; }
        public string po_num { get; set; }
        public string loc_id { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public float Depth { get; set; }
        public decimal Cube { get; set; }
        public decimal Weight { get; set; }
        public int avail_ctn { get; set; }
        public int itm_qty { get; set; }
        public int avail_qty { get; set; }
    }

    public class ItemStockDimUpdate
    {
        public string ref_num { get; set; }
        public string cmp_id { get; set; }
        public string ib_doc_id { get; set; }
        public string itm_code { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public decimal length { get; set; }
        public decimal width { get; set; }
        public decimal depth { get; set; }
        public decimal cube { get; set; }
        public decimal wgt { get; set; }
        public string rec_status { get; set; }

    }

    public class ItemMaster : ItemMasterdtl
    {
        //List Fetch Details
        public IList<ItemMaster> ListItemMaster { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<LookUp> ListLookUpStatusDtl { get; set; }
        public IList<LookUp> ListLookUpCategoryDtl { get; set; }
        public IList<ItemMasterdtl> ListItemMasterViewDtl { get; set; }
        public IList<LookUp> ListLookUpDtl { get; set; }
        public IList<ItemMaster> LstItemId { get; set; }
         public IList<ItemMaster> ListItmId { get; set; }

        public IList<ItemStock> ListItemStock { get; set; }
        public IList<ItemStockDimUpdate> ListItemStockDimUpdate { get; set; }
        public IList<itmImage> ListitmImage { get; set; }
        public itmImage objItmImage { get; set; }

        public IList<clsItemConfig> ListItemConfig { get; set; }
        public IList<clsItemConfig> ListPkgType { get; set; }
        public IList<clsItemConfig> ListPkgPoly { get; set; }
        public IList<clsItemConfig> ListPkgBox { get; set; }
        public IList<clsItemPkgService> ListPkgSrvc { get; set; }
        public clsBinMater objBinMater { get; set; }
        public IList<LookUp> ListLookUpBinType { get; set; }
        public IList<LookUp> ListBintatus { get; set; }
        public IList<Pick> ListQtyUoM { get; set; }
        public IList<Pick> ListWhs { get; set; }

    }
    public class itmImage
    {
        public string cmp_id { get; set; }
        public string itm_code { get; set; }
        public int img_order { get; set; }
        public string img_type { get; set; }
        public string img_file_name { get; set; }
        public string img_path { get; set; }
        public string img_name { get; set; }
        public string img_id { get; set; }
        public int line_num { get; set; }
        public string img_desc { get; set; }
    }


    public class ItemdtlReport 
    {
        public string CmpId { get; set; }
        public string Style { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string ItemName { get; set; }
        public string Status { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public float Depth { get; set; }
        public decimal Cube { get; set; }
        public decimal Weight { get; set; }

   
        public IList<ItemdtlReport> ListItemdtl { get; set; }


    }

}
