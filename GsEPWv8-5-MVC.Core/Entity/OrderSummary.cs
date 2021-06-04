using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
 public   class OrderSummary : BasicEntity

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
        public ClsEcomLink objEcomLink { get; set; }
    }
}
