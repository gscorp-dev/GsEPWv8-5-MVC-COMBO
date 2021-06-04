using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class IBRcvdDataUpdate
    {
        public string cmp_id;
        public string ib_doc_id;
        public string ref_no;
        public string cntr_id;
        public string cntr_type;
        public string rcvd_dt;
        public string doc_status_changed;
        public bool excld_bill;

        public IList<IBRcvdDataUpdate> GetRcvdHdr { get; set; }
        public IList<IBRcvdDataUpdateDtl> ListDocItemList { get; set; }
        public IList<Company> ListCompany { get; set; }
        public IList<LookUp> ListContainerType { get; set; }
    }

    public class IBRcvdDataUpdateHdr
    {
        public string cmp_id { get; set; }
        public string ib_doc_id { get; set; }
        public string cntr_id { get; set; }
        public string ref_no { get; set; }
        public string rcvd_dt { get; set; }
        //public string cntr_type { get; set; }
        //public bool excld_bill { get; set; }
    }

    public class IBRcvdDataUpdateDtl
    {
        public string cmp_id { get; set; }
        public string ib_doc_id { get; set; }
        public string itm_code { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string itm_name { get; set; }
        public decimal length { get; set; }
        public decimal width { get; set; }
        public decimal depth { get; set; }
        public decimal cube { get; set; }
        public decimal wgt { get; set; }
    }



}
