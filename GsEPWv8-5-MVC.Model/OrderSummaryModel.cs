using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
    public class OrderSummaryModel : BasicEntity

    {
        public string cmp_id { get; set; }

        public string OrderStatus { get; set; }

        public Int32 TotalOrders { get; set; }
        public Int32 ecom_temp_orders { get; set; }
        public Int32 ecom_open_orders { get; set; }
        public Int32 ecom_aloc_orders { get; set; }
        public Int32 ecom_ship_orders { get; set; }
        public Int32 ecom_post_orders { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<OrderSummary> LstOrderSummary { get; set; }
        public string p_str_cmp_id { get; set; }
        public string frm_dt { get; set; }
        public string to_dt { get; set; }
        public string p_str_frm_dt { get; set; }
        public string p_str_to_dt { get; set; }
        public ClsEcomLink objEcomLink { get; set; }
    }
}
