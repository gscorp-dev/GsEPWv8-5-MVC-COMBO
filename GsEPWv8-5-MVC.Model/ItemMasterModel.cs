using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
    public class ItemMasterDtlModel : BasicEntity
    {
        public string cmp_id { get; set; }
        public string itm_num { get; set; }
        public string itm_code { get; set; }
        public string itm_color { get; set; }

        public string itm_size { get; set; }
        public string itm_name { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public decimal Cube { get; set; }
        public float Depth { get; set; }
        public float Weight { get; set; }
        public float wgt { get; set; }
        public string Str_cmp_id { get; set; }
        public string group_id { get; set; }
        public bool Kit_Itm { get; set; }
        public string catg_id { get; set; }
        public string Class { get; set; }
        public string status { get; set; }
        public int Opt_id { get; set; }
        public bool KitItem { get; set; }

        public string itmid { get; set; }
        public bool NonInventorysItem { get; set; }
        public string sku_id { get; set; }
        public decimal list_price { get; set; }
        public string image_name { get; set; }
        public int ctn_qty { get; set; }
        public bool is_inner_pack { get; set; }
        public int inner_pack_ctn_qty { get; set; }
        public decimal inner_pack_length { get; set; }
        public decimal inner_pack_width { get; set; }
        public decimal inner_pack_depth { get; set; }
        public decimal inner_pack_cube { get; set; }
        public decimal inner_pack_wgt { get; set; }

        public string pkg_type { get; set; }
        public string pkg_size { get; set; }
        public string pkg_service { get; set; }
        public decimal pce_length { get; set; }
        public decimal pce_width { get; set; }
        public decimal pce_depth { get; set; }
        public decimal pce_cube { get; set; }
        public decimal pce_wgt { get; set; }
        public bool bln_delete_bin { get; set; }

        public IList<clsItemPkgService> ListItemPkgSrvc { get; set; }
        clsItemConfig objItemConfig { get; set; }
       
    }

    public class ItemMasterModel : ItemMasterDtlModel
    {
        //List Fetch Details
        public IList<ItemMaster> ListItemMaster { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<LookUp> ListLookUpStatusDtl { get; set; }
        public IList<LookUp> ListLookUpCategoryDtl { get; set; }
        public IList<ItemMasterdtl> ListItemMasterViewDtl { get; set; }
        public IList<LookUp> ListLookUpDtl { get; set; }

        public IList<ItemMaster> LstItemId { get; set; }
        public IList<ItemStock> ListItemStock { get; set; }
        public itmImage objItmImage { get; set; }
        public IList<itmImage> ListitmImage { get; set; }
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
}
