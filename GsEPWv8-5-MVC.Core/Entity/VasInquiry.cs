using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class VAS_Grid_SummaryExcel
    {
        //public string Cust_ID { get; set; }
        public string VasID { get; set; }
        public string VasDate { get; set; }
        //public string TotAmount { get; set; }
        public string status { get; set; }
        public string Note { get; set; }
        //public string so_num { get; set; }
        //public string quote_num { get; set; }
        //public string cmp_name { get; set; }
        //public string city { get; set; }
        //public string state_id { get; set; }
        //public string post_code { get; set; }
        //public string addr_line1 { get; set; }
        //public string fax { get; set; }
        //public string tel { get; set; }

    }
    public class VAS_POST_DTL_Excel
    {
        //public string custname { get; set; }
        //public string cust_addr1 { get; set; }
        //public string cust_addr2 { get; set; }
        //public string cust_city { get; set; }
        //public string cust_state { get; set; }
        //public string cust_postcode { get; set; }
        //public string custtel { get; set; }

        //public string custfax { get; set; }

        public string ship_to { get; set; }
        public string ship_doc_id { get; set; }
        public DateTime ShipDt { get; set; }
        public string whs_id { get; set; }
        public string po_num { get; set; }
        public int dtl_line { get; set; }
        public string so_itm_num { get; set; }
        public string catg { get; set; }
        public string so_num { get; set; }
        public string itm_name { get; set; }
        public int ship_qty { get; set; }
        public decimal ship_itm_price { get; set; }
        public string cust_name { get; set; }
        public string notes { get; set; }
        //public string cmp_name { get; set; }
        //public string addr_line1 { get; set; }
        //public string city { get; set; }
        //public string state_id { get; set; }
        //public string post_code { get; set; }
        //public string tel { get; set; }

    }
    public class VasInquiryDtl : BasicEntity
    {
        public string is_company_user { get; set; }
        public string is_cmp_user { get; set; }
        public string cust_id { get; set; }
        public string vas_id_fm { get; set; }
        public string vas_id_to { get; set; }
        public string vas_date_fm { get; set; }
        public string vas_date_to { get; set; }
        public string sr_num { get; set; }
        public string txtstatus { get; set; }
        public string cmp_id { get; set; }
        public string ship_to_id { get; set; }
        public string vas_user_id { get; set; }
        public string ship_price_uom { get; set; }
        public string cust_itm { get; set; }
        public string cust_ordr_num { get; set; }
        public string cust_ordr_dt { get; set; }
        public string shipto_id { get; set; }
        public string custname { get; set; }
        public string cust_addr1 { get; set; }
        public string cust_addr2 { get; set; }
        public string cust_city { get; set; }
        public string cust_state { get; set; }
        public string cust_postcode { get; set; }
        public string custtel { get; set; }
        public string cell { get; set; }
        public string custfax { get; set; }
        public string ship_to { get; set; }
        public string ship_doc_id { get; set; }
        public DateTime ShipDt { get; set; }
        public string whs_id { get; set; }
        public string po_num { get; set; }
        public int dtl_line { get; set; }
        public string so_itm_num { get; set; }
        public string catg { get; set; }
        public string so_num { get; set; }
        public string itm_name { get; set; }
        public int ship_qty { get; set; }
        public decimal ship_itm_price { get; set; }
        public string cust_name { get; set; }
        public string cmp_name { get; set; }
        public string addr_line1 { get; set; }
        public string city { get; set; }
        public string state_id { get; set; }
        public string post_code { get; set; }
        public string fax { get; set; }
        public string tel { get; set; }

        public string Cust_ID { get; set; }
        public string user_id { get; set; }
        public string VasID { get; set; }
        public string VasDate { get; set; }
        public decimal TotAmount { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string quote_num { get; set; }
        public string status { get; set; }
        public string ship_type { get; set; }
        public int so_dtl_line { get; set; }
        public string cust_itm_name { get; set; }
        public string so_itm_color { get; set; }
        public string Shipamt { get; set; }
        public string ship_Uom { get; set; }
        public string ship_dt { get; set; }
        public string notes { get; set; }
        public string note { get; set; }
        public string fob { get; set; }
        public string cust_po_num { get; set; }
        public string cust_po_dt { get; set; }
        public string vas_id { get; set; }
        public string type { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public decimal qty { get; set; }
        public decimal list_price { get; set; }
        public decimal amt { get; set; }
        public string price_uom { get; set; }
        public string Rate { get; set; }
        public decimal Amt { get; set; }
        public string View_Flag { get; set; }
        public string dft_whs { get; set; }
        public string vas_bill_status { get; set; }
        public string bill_doc_id { get; set; }
        public string cust_of_cmpid { get; set; }
        public string screentitle { get; set; }
        public string tmp_cmp_id { get; set; }
        public string Image_Path { get; set; }
        public string IS3RDUSER { get; set; }
        public string VasCount { get; set; }
        public string Check { get; set; }
        public int line_num { get; set; }   //CR - 3PL-MVC-VAS-20180405 Added by Soniya
        public decimal Amount { get; set; }   //CR - 3PL-MVC-VAS-20180505 Added by Soniya
        public string cust_cust_name { get; set; }
        public string hdr_note { get; set; }

        public int upload_file_count { get; set; }

    }
    public class VasInquiry : VasInquiryDtl
    {
        //List Fetch Details
        public IList<VasInquiryDtl> ListVasInquiry { get; set; }
        public IList<VasInquiryDtl> ListVasEntryDtl { get; set; }
        public IList<Company> LstCustOfCmpName { get; set; }
        public IList<VasInquiryDtl> ListLoadSrDetails { get; set; }
        public IList<VasInquiryDtl> ListVasId { get; set; }
        public IList<VasInquiryDtl> ListVasEntryGridDtl { get; set; }
        public IList<VasInquiryDtl> ListUpdateTempVasEntryGridDtl { get; set; }
        public IList<VasInquiryDtl> ListVasEntryTempGridDtl { get; set; }
        public IList<VasInquiryDtl> ListVasUserId { get; set; }
        public IList<Pick> ListPick { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<Company> ListwhsPickDtl { get; set; }
        public IList<VasInquiryDtl> ListPickdtl { get; set; }
        public IList<LookUp> ListLookUpDtl { get; set; }
        public IList<VasInquiryDtl> LstSR { get; set; }
        public IList<VasInquiryDtl> ListReUpdateTempVasEntryGridDtl { get; set; }

        public string vasRateId { get; set; }
        public IList<VasRateDetails> ListVasRateIdDetails { get; set; }
    }

    public class VasRateDetails
    {
        public string rate_id { get; set; }
        public string rate_desc { get; set; }
    }
}
