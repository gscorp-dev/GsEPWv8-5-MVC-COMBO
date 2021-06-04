using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
   public class ClsBillVAS
    {
        public string cmp_id { get; set; }
        public string cust_of_cmpid { get; set; }
        public string ship_doc_id { get; set; }
        public string ship_dt { get; set; }
        public string vas_status { get; set; }
        public string whs_id { get; set; }
        public string cust_id { get; set; }
        public string po_num { get; set; }
        public string ship_to { get; set; }
        public string so_num { get; set; }
        public string bill_doc_id { get; set; }
        public int total_activities { get; set; }
        public decimal total_price { get; set; }
           
    }

    public class ClsBillVASReBill
    {
        public string cmp_id { get; set; }
        public string cust_of_cmpid { get; set; }
        public string re_bill_doc_id { get; set; }
        public string bill_from_dt { get; set; }
        public string bill_to_dt { get; set; }
        public List<ClsBillVAS> lstBillVASList { get; set; }
      
    }
    }
