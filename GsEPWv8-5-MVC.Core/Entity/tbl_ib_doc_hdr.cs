using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class tbl_ib_doc_hdr
    {
      
        public string cmp_id { get; set; }
        public string cntr_id { get; set; }
        public string HeaderInfo { get; set; }
        public string eta_date { get; set; }
        public string ref_num { get; set; }
        public string rcvd_via { get; set; }
        public string rcvd_from { get; set; }
        public string master_bol { get; set; }
        public string vessel_no { get; set; }
        public string hdr_notes { get; set; }
      
    }
}
