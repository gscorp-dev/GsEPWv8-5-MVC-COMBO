using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class OB_SHIP_RPT_Excel
    {
        public string ShipDate { get; set; }
        public string CmpId { get; set; }
        public string Bol { get; set; }
        public string CustomerName { get; set; }
        public string SRNumber { get; set; }

    
      
        public string RefNum { get; set; }
        public string CustPO { get; set; }
        public string Style { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string WhsId { get; set; }
        public int Ctns { get; set; }
        public int Pcs { get; set; }
        public decimal Cube { get; set; }
        public decimal Weight { get; set; }
        //CR - 3PL_MVC-IB-20180405 Commented By Nithya
        //public string cmp_name { get; set; }
        //public int ctn_line { get; set; }
        //public int  itm_qty { get; set; }
        //public string ship_dt { get; set; }                    
        //public string note { get; set; }                  
        ////public string addr_line1 { get; set; }
        ////public int city { get; set; }
        ////public string state_id { get; set; }
        ////public int post_code { get; set; }       
        //public int ship_qty { get; set; }             
        //public string fax { get; set; }
        //public string tel { get; set; }
        //END
    }
    public class OB_SHIP_RPT_STYLE_Excel
    {
        public string ShipDate { get; set; }
        public string CmpId { get; set; }
        public string BOL { get; set; }
        public string CustomerName { get; set; }
        public string SRNumber { get; set; }
      
       
        public string RefNum { get; set; }

        public string Style { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string StyleName { get; set; }
        public string WhsId { get; set; }
        public int Ctns { get; set; }
        public int Pcs { get; set; }

        public decimal Cube { get; set; }
        public decimal Weight { get; set; }


        //public int ship_qty { get; set; }    //CR - 3PL_MVC-IB-20180405 Commented By Nithya    
    }
    public class OutboundShipRpt
    {
        public string cmp_id { get; set; }
        public string so_num { get; set; }
        public string cust_ord { get; set; }
        public string ship_via_name { get; set; }
        public string ship_doc_id { get; set; }
        public string ship_dt { get; set; }
        public string ref_num { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string itm_name { get; set; }
        public string Status { get; set; }
        public string whs_id { get; set; }
        public string aloc_doc_id { get; set; }
        public int line_qty { get; set; }
        public int Ctns { get; set; }
        public int Pcs { get; set; }
        public string so_itm_code { get; set; }
        public string ship_dt_fm { get; set; }
        public string ship_dt_to { get; set; }
        public string cmp_name { get; set; }
        public int ctn_line { get; set; }
        public int itm_qty { get; set; }
        public string ship_to_name { get; set; }
        public string note { get; set; }
        public int ctn_cnt { get; set; }
        public string addr_line1 { get; set; }
        public string city { get; set; }
        public string state_id { get; set; }
        public string post_code { get; set; }
        public int ship_qty { get; set; }
        public string cust_ordr_num { get; set; }
        public string fax { get; set; }
        public string tel { get; set; }
        public decimal itm_code { get; set; }
        public int TotalCtn { get; set; }
        public int TotalQty { get; set; }
        public string Itmdtl { get; set; }
        public string p_str_cmpid { get; set; }
        public string dft_whs { get; set; }
        public string Image_Path { get; set; }
        public decimal ctn_cube { get; set; }
        public decimal ctn_wgt { get; set; }

        public IList<OutboundShipRpt> LstOutboundShipRptInqdetail { get; set; }
        public IList<OutboundShipRpt> LstOutboundShipRptbyDatedetail { get; set; }
        public IList<LookUp> ListLookUpDtl { get; set; }
        public IList<Company> ListCompanyDtl { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<OutboundShipRpt> LstItmxCustdtl { get; set; }
        public IList<OutboundShipRpt> LstWhsDetails { get; set; }
        public IList<Company> ListwhsPickDtl { get; set; }
    }
}
