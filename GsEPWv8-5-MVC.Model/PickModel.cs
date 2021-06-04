using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
    public class PickModel
    {
        public string cmp_id { get; set; }
        public string Whs_id { get; set; }
        public string Whs_name { get; set; }
        public string term { get; set; }
        public string State_ID { get; set; }
        public string State_Name { get; set; }
        public string Cntry_Id { get; set; }
        public string Cntry_Name { get; set; }
        public string shipto_id { get; set; }
        public string cust_id { get; set; }
        public string attn { get; set; }
        public string DC_id { get; set; }
        public string mail_name { get; set; }
        public string addr_line1 { get; set; }
        public string addr_line2 { get; set; }
        public string city { get; set; }
        public string state_id { get; set; }
        public string Countrydtl { get; set; }
        public string statedtl { get; set; }
        public string post_code { get; set; }
        public string cntry_id { get; set; }

        public string uom_id { get; set; }
        public string uom_desc { get; set; }
        public string Check_existing_uom { get; set; }
        public string Check_itm_code { get; set; }
        public string itm_code { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string qty_uom { get; set; }

        public IList<Pick> ListPick { get; set; }
        public IList<Pick> ListCntryPick { get; set; }
        public IList<Pick> ListStatePick { get; set; }
        public IList<Pick> ListShipToPick { get; set; }
        public IList<Pick> ListExistShipToAddrsPick { get; set; }
        public IList<Pick> ListUomPick { get; set; }
        public IList<Pick> ListCheckItemCode { get; set; }

    }
}
