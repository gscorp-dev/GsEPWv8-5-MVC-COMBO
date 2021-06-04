using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class PickLabel
    {
        public string cmp_id { get; set; }
        public string ref_num { get; set; }
        public string batch_num { get; set; }
        public string so_num_from { get; set; }
        public string so_num_to { get; set; }
        public string so_dt_from { get; set; }
        public string so_dt_to { get; set; }
        public List<PickLabelDtl> lstPickLabelDtl { get; set; }
        public List<PickLabelSmry> lstPickLabelSmry { get; set; }
        public List<DocForPrint> lstDocForPrint { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
    }

    public class DocForPrint
    {
        public string cmp_id { get; set; }
        public string so_num { get; set; }
        public string file_type { get; set; }
        public string file_path { get; set; }
        public string upload_file_name { get; set; }
    }
        public class PickLabelDtl
    {
        public string cmp_id { get; set; }
        public string so_num { get; set; }
        public string so_dt { get; set; }
        public string cust_name { get; set; }
        public string CompID { get; set; }
        public string cust_ordr_num { get; set; }
        public string ref_no { get; set; }
        public string ship_dt { get; set; }
        public string whs_id { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string itm_name { get; set; }
        public string upc_code { get; set; }
        public string aloc_qty { get; set; }
        public string loc_id { get; set; }
        public string length { get; set; }
        public string width { get; set; }
        public string depth { get; set; }
        public string cube { get; set; }
        public string wgt { get; set; }
        public string wgt_uom { get; set; }
        public string shipto_id { get; set; }
        public string st_attn { get; set; }
        public string st_addr_line1 { get; set; }
        public string st_addr_line2 { get; set; }
        public string st_city { get; set; }
        public string st_state_id { get; set; }
        public string st_post_code { get; set; }
        public string st_cntry_id { get; set; }

    }
    public class PickLabelSmry
    {
        public string cmp_id { get; set; }
        public string batch_num { get; set; }
        public string so_num { get; set; }
        public string so_dt { get; set; }
        public string ordr_num { get; set; }
        public string ref_no { get; set; }
        public string cust_name { get; set; }
        public string cust_ordr_num { get; set; }
        public string ship_dt { get; set; }
        public string aloc_qty { get; set; }
        public string st_attn { get; set; }
        public string shipto_id { get; set; }
        public string st_addr_line1 { get; set; }
        public string st_addr_line2 { get; set; }
        public string st_city { get; set; }
        public string st_state_id { get; set; }
        public string st_post_code { get; set; }
        public string st_cntry_id { get; set; }

    }
}
