using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class StockAsOfDate
    {
        public string is_company_user { get; set; }
        public string cmp_id { get; set; }
        public string kit_id { get; set; }
        public string itm_num { get; set; }
        public float totcube { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string itm_name { get; set; }
        public string lot_id { get; set; }
        public string cont_id { get; set; }
        public string loc_id { get; set; }
        public string st_rate_id { get; set; }
        public string kit_type { get; set; }
        public decimal length { get; set; }
        public decimal width { get; set; }
        public decimal depth { get; set; }
        public decimal wgt { get; set; }
        public decimal cube { get; set; }
        public int avail_cnt { get; set; }
        public int avail_qty { get; set; }
        public string itm_code { get; set; }
        public string p_str_company { get; set; }
        public string p_str_cmpid { get; set; }
        public string ib_doc_id { get; set; }
        public string po_num { get; set; }
        public string grn_id { get; set; }
        public string whs_id { get; set; }
        public string Itmdtl { get; set; }
        public string status { get; set; }
        public string As_Of_Date { get; set; }
        public DateTime rcvd_dt { get; set; }
        public int pkg_qty { get; set; }
        public string palet_id { get; set; }
        public int Ctns { get; set; }
        public string cmp_name { get; set; }//CR-3PL-MVC-180322-001 Added By NIthya
        public string addr_line1 { get; set; }
        public string addr_line2 { get; set; }
        public string tel { get; set; }
        public string fax { get; set; }
        public string state_id { get; set; }
        public string post_code { get; set; }
        public string city { get; set; }
        public string Image_Path { get; set; }
     
        public string style_stearch { get; set; }

        public class StockAsOfdateDetailExcel
        {
            public string IBdocId { get; set; }
            public string ContainerId { get; set; }
            public DateTime RcvdDt { get; set; }
            public string Style { get; set; }
            public string Color { get; set; }
            public string Size { get; set; }
            public string Description { get; set; }
            public string WhsId { get; set; }
            public string LocId { get; set; }         
            public int AvailCtn { get; set; }
            public int PPk { get; set; }
            public int AvailQty { get; set; }            
        }

        public class StockAsOfdateDetailRptExcel
        {
            public string CmpId { get; set; }
            public string WhsId { get; set; }
            public DateTime RcvdDt { get; set; }
            public string IBDocId { get; set; }
            public string ContainerId { get; set; }
          
            public string Style { get; set; }
            public string Color { get; set; }
            public string Size { get; set; }
            public string Description { get; set; }
         
            public string LocId { get; set; }
            public int AvailCtn { get; set; }
            public int PPk { get; set; }
            public int AvailQty { get; set; }
            public decimal Length { get; set; }
            public decimal Width { get; set; }
            public decimal Depth { get; set; }
            public decimal Cube { get; set; }
            public decimal Weight { get; set; }
        

        }
        public class StockAsOfdateSummaryExcel
        {           
            public string Style { get; set; }
            public string Color { get; set; }
            public string Size { get; set; }
            public string Description { get; set; }          
            public int AvailQty { get; set; }    
        }
        public IList<StockAsOfDate> ListStockAsOfDateGrid { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<LookUp> ListLookUpDtl { get; set; }
        public IList<Company> LstCmpName { get; set; }
        public IList<StockAsOfDate> LstAsOfDateRpt { get; set; }
        public IList<StockAsOfDate> ListCottonStockAsOfDateGrid { get; set; }

    }
}
