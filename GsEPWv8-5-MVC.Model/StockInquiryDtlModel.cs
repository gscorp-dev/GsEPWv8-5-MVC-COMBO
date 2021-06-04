using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
    public class StockInquiryDtlModel
    {
        public string is_company_user { get; set; }
        public string cmp_id { get; set; }
        public string kit_id { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string itm_name { get; set; }
        public string lot_id { get; set; }
        public string cont_id { get; set; }
        public string loc_id { get; set; }
        public string Audcmp_id { get; set; }
        public string Audkit_id { get; set; }
        public string Auditm_num { get; set; }
        public string Auditm_color { get; set; }
        public string Auditm_size { get; set; }
        public string Auditm_name { get; set; }
        public string Audlot_id { get; set; }
        public string Audcont_id { get; set; }
        public string Audloc_id { get; set; }
        public string Audwhs_id { get; set; }
        public string Audib_doc_id { get; set; }
        public string AudDate_fm { get; set; }
        public string AudDate_to { get; set; }
        public string Audgrn_id { get; set; }
        public string Audstatus { get; set; }
        public string Audpo_num { get; set; }

        
        public string whs_id { get; set; }
        public int AvlCtn { get; set; }
        public decimal pkgQty { get; set; }
        public string palet_id { get; set; }
       
        public decimal itm_qty { get; set; }
        public int AvlQty { get; set; }
        public int tot_qty { get; set; }
        public string pkg_type { get; set; }
        public string po_num { get; set; }
        public string ib_doc_id { get; set; }
        public string ob_doc_id { get; set; }
        public int Ctns { get; set; }
        public string Qty { get; set; }     
        public string Ref_Num { get; set; }
        public string Date_fm { get; set; }
        public string Date_to { get; set; }     
        public string tran_type { get; set; }          
        public int AlocCtn { get; set; }
        public int Alocqty { get; set; }
        private string _Rcvd_Qty = "0";
        public string Rcvd_Qty { get { return _Rcvd_Qty; } set { _Rcvd_Qty = value; } }
        private string _Rcvd_Ctns = "0";
        public string Rcvd_Ctns { get { return _Rcvd_Ctns; } set { _Rcvd_Ctns = value; } }
        private string _Ship_Qty = "0";
        public string Ship_Qty { get { return _Ship_Qty; } set { _Ship_Qty = value; } }
        private string _Ship_Ctns = "0";
        public string Ship_Ctns { get { return _Ship_Ctns; } set { _Ship_Ctns = value; } }
        private string _Adj_Qty = "0";
        public string Adj_Qty { get { return _Adj_Qty; } set { _Adj_Qty = value; } }
        private string _Adj_Ctns = "0";
        public string Adj_Ctns { get { return _Adj_Ctns; } set { _Adj_Ctns = value; } }
        public string Status { get; set; }
        public string Ppk { get; set; }
        public string Itm_Qty { get; set; }
        public string doc_date { get; set; }
        public string grn_id { get; set; }
        public string cmp_name { get; set; }
        public string city { get; set; }
        public string state_id { get; set; }
        public string post_code { get; set; }
        public string addr_line1 { get; set; }
        public string tel { get; set; }
        public string fax { get; set; }
        public string item_code { get; set; }
        public string pkg_id { get; set; }
        public string status { get; set; }
        public string doc_notes { get; set; }
        public decimal cube { get; set; }
        public string Selection1 { get; set; }
        public string Selection2 { get; set; }
        public string ordr_num { get; set; }
        public string cust_ordr_num { get; set; }
        public string vend_name { get; set; }
        public string fmto_name { get; set; }
        public string doc_id { get; set; }
        public int  SHIP_CTN { get; set; }
        public int ADJUST_CTN { get; set; }
        public int RCVD_CTN { get; set; }
        public int RCVD { get; set; }
        public int SHIP { get; set; }
        public int TOT_BALANCE { get; set; }
        public int TOT_CTN { get; set; }
        public int Avail_cnt { get; set; }
        public int Avail_qty { get; set; }
        public string radio { get; set; }
        public decimal Itmqty { get; set; }
        public string ItmCode { get; set; }
        public string Style { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string DocDate { get; set; }
        public string TranType { get; set; }
        public string Locid { get; set; }
        public string docId { get; set; }
        public string LotId { get; set; }
        public int pkg_qty { get; set; }
        public int AlocQty { get; set; }
        public string locid { get; set; }
        public string ponum { get; set; }
        public decimal pkgqty { get; set; }
        public decimal itmqty { get; set; }
        public string pkgid { get; set; }
        public int Packgqty { get; set; }
        public string LocaId { get; set; }
        public string rcvd_dt { get; set; }
        public string Itmdtl { get; set; }
        public string p_str_cmpid { get; set; }
        public string AuditItmdtl { get; set; }

        public string Item_Name { get; set; }
        public string Whs { get; set; }
        public string Loc { get; set; }
        public string Po_Num { get; set; }
        public string In_Out { get; set; }

        public int Pcs { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Item_Cube { get; set; }
        public decimal Weight { get; set; }
        public string Notes { get; set; }
        public string Strg { get; set; }

        public string Date { get; set; }
        public string supp_cust { get; set; }
        public string Doc_ID { get; set; }
        public decimal Cube { get; set; }
        public string Item_name { get; set; }
        public string p_str_company { get; set; }
        public string CompID { get; set; }
        public string WareHouse { get; set; }
        public string Description { get; set; }
        public int STK_QTY { get; set; }
        public int AVAIL_QTY { get; set; }
        public int WIP_QTY { get; set; }
        public int ALLOC_QTY { get; set; }
        public int Itmqty_rpt { get; set; }
        public int Itmqty_smry { get; set; }
        public int stk_adj_plus { get; set; }
        public int stk_adj_minus { get; set; }
        public string CompName { get; set; }
        public string tmp_cmp_id { get; set; }
        public decimal itm_cube { get; set; }       //CR_3PL_MVC_STK_2018_0223_001 Added by Soniya

        public string Image_Path { get; set; }//CR_3PL_MVC_BL_2018_0425_001 
        public string sr_no { get; set; }
        public int upload_file_count { get; set; }


        public IList<StockInquiryDtl> ListStockInquiryGrid { get; set; }

        public IList<StockInquiryDtl> ListStockInquiry { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<StockInquiryDtl> ListStockInquiryaudit { get; set; }
        public IList<StockInquiryDtl> ListStockInquiryRpt { get; set; }
        public IList<StockInquiryDtl> ListStockSummRpt { get; set; }
        public IList<StockInquiryDtl> ListStockLocRpt { get; set; }
        public IList<StockInquiryDtl> ListStockLocSummRpt { get; set; }
        public IList<StockInquiryDtl> ListAudRpt { get; set; }
        public IList<StockInquiryDtl> ListAudSummRpt { get; set; }
        public IList<StockInquiryDtl> LstStockInquirystock { get; set; }
        public IList<StockInquiryDtl> LstItmxCustdtl { get; set; }
        public IList<LookUp> ListLookUpDtl { get; set; }
        public IList<StockInquiryDtl> LstStockInquiryGridDtl { get; set; }
        public IList<StockInquiryDtl> LstStockInquiryStyleDtl { get; set; }
        public IList<StockInquiryDtl> LstStockInquiryStyleSmry { get; set; }
        public IList<StockInquiryDtl> LstStockInquiryLocDtl { get; set; }
        public IList<StockInquiryDtl> LstStockInquiryLocSmry { get; set; }
        public IList<StockInquiryDtl> LstAuditWithIsm { get; set; }
        public IList<StockInquiryDtl> LstAuditWithOutIsm { get; set; }
        public IList<StockInquiryDtl> LstAuditOnlyIsm { get; set; }
        public IList<AuditItemDtl> LstAuditItemDtl { get; set; }

    }

}
