using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{

    public class BinMaster
    {
        public string view_flag { get; set; }
        public string cmp_id { get; set; }
        public string whs_id { get; set; }
        public string bin_id { get; set; }
        public string bin_desc { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string itm_name { get; set; }
        public clsBinMater objBinMater { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<LookUp> ListLookUpBinType { get; set; }
        public IList<clsBinMater> ListBinMasterinqury { get; set; }
       public IList<Pick> ListWhs { get; set; }
        public IList<LookUp> ListStatus { get; set; }
        public IList<LookUp> ListLookUpDtls { get; set; }
        public IList<BinMaster> Listbin_id { get; set; }
        public IList<Pick> ListQtyUoM { get; set; }

    }

    public class clsBinMater
    {
        public string cmp_id { get; set; }
        public string whs_id { get; set; }
        public string bin_id { get; set; }
        public string bin_type { get; set; }
        public string bin_loc { get; set; }
        public string bin_desc { get; set; }
        public string status { get; set; }
        public string itm_code { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string itm_name { get; set; }
        public decimal bin_length { get; set; }
        public decimal bin_width { get; set; }
        public decimal bin_height { get; set; }
        public decimal bin_cube { get; set; }
        public decimal bin_wgt { get; set; }
        public string min_qty { get; set; }
        public string max_qty { get; set; }
        public string ordr_qty { get; set; }
        public string qty_uom { get; set; }
        public string price { get; set; }
        public string qpm { get; set; }
        public string lead_time { get; set; }
        public int bin_ppk { get; set; }
        public decimal pce_length { get; set; }
        public decimal pce_width { get; set; }
        public decimal pce_depth { get; set; }
        public decimal pce_cube { get; set; }
        public decimal pce_wgt { get; set; }
        public string bin_dt { get; set; }
        public string user_id { get; set; }
        public string process_id { get; set; }
       
       public clsItemPcsDim ItemPcsDim { get; set; }

    }

    public class clsItemPcsDim
    {
        public decimal pce_length { get; set; }
        public decimal pce_width { get; set; }
        public decimal pce_depth { get; set; }
        public decimal pce_cube { get; set; }
        public decimal pce_wgt { get; set; }
    }

}
