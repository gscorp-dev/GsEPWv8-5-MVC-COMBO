using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class tbl_ib_doc_grid
    {
        public string cmp_id { get; set; }
        public string HeaderInfo { get; set; }
        public int dtl_line { get; set; }
        public int ctn_line { get; set; }
        public string eta_date { get; set; }
        public string ref_num { get; set; }
        public string rcvd_via { get; set; }
        public string rcvd_from { get; set; }
        public string master_bol { get; set; }
        public string vessel_no { get; set; }
        public string hdr_notes { get; set; }
        public string cntr_id { get; set; }
        public string po_num { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string itm_name { get; set; }
        public int itm_qty { get; set; }
        public int ctn_qty { get; set; }
        public int ctns { get; set; }
        public string loc_id { get; set; }
        public string st_rate_id { get; set; }
        public string io_rate_id { get; set; }
        public decimal ctn_length { get; set; }
        public decimal ctn_width { get; set; }
        public decimal ctn_height { get; set; }
        public decimal ctn_cube { get; set; }
        public decimal ctn_wgt { get; set; }
        public string dtl_notes { get; set; }
        public string entry_dt { get; set; }
        public string vend_id { get; set; }
        


    }
}
