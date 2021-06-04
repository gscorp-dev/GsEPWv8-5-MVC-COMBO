using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class clsEComOrders
    {
        private List<clsEComPrintOrders> EComPrintOrdersField;
        public List<clsEComPrintOrders> EComPrintOrders
        {
            get
            {
                return EComPrintOrdersField;
            }

            set
            {
                EComPrintOrdersField = value;
            }
        }
    }
   public class clsEComPrintOrders
    {
        public string cmp_id { get; set; }
        public string batch_id { get; set; }
        public string batch_status { get; set; }
        public string ordr_rcvd_dt { get; set; }
        public int total_orders { get; set; }
        public int open_orders { get; set; }
        public int alloc_orders { get; set; }
        public int shipped_orders { get; set; }

        public string printed_dt { get; set; }
        public string printed_by { get; set; }
        public string batch_file_name { get; set; }
        public string mode { get; set; }
        public string print_file_name { get; set; }
    }

    public class clsEcomPrintAloc
    {
        public string cmp_id { get; set; }
        public string whs_id { get; set; }
        public string batch_id { get; set; }
        public string loc_id { get; set; }
        public string cust_name { get; set; }
        public string cust_ordr_num { get; set; }
        public string ref_no { get; set; }
        public string so_num { get; set; }
        public string so_dt { get; set; }
        public string aloc_doc_id { get; set; }
        public string ship_dt { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string itm_name { get; set; }
        public string upc_code { get; set; }
        public string aloc_qty { get; set; }
        public string length { get; set; }
        public string width { get; set; }
        public string depth { get; set; }
        public string cube { get; set; }
        public string wgt { get; set; }
     


    }
    public class clsEcomPrintDoc
    {
        public string cmp_id { get; set; }
        public string batch_id { get; set; }
        public string so_num { get; set; }
        public string doc_type { get; set; }
        public string file_path { get; set; }
        public string upload_file_name { get; set; }
    
    }
}
