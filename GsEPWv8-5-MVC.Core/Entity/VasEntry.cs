using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class VasEntryDtl : BasicEntity
    {
        public string cust_id { get; set; }
        public string vas_id { get; set; }
        public string vas_date { get; set; }
        public string status { get; set; }
        public string sr_num { get; set; }
        public string fob { get; set; }
        public string ship_to_id { get; set; }
        public string cust_po { get; set; }
        public string cust_po_dt { get; set; }
        public string header_note { get; set; }
        public string item_num { get; set; }
        public string item_desc { get; set; }
        public string qty { get; set; }
        public string rate { get; set; }
        public string amount { get; set; }
        public string sts { get; set; }
        public string note { get; set; }
        public string uom { get; set; }
    }
    public class VasEntry : VasEntryDtl
    {
        //List Fetch Details
        public IList<VasEntry> ListVasEntry { get; set; }
        public List<VasEntry> LstInquiry { get; set; }
    }
}
