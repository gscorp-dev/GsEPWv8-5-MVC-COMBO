using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//CR-3PL_MVC_IB_2018_0219_004 - Add a new column Bill and it should be visible once the Receiving is posted. By clicking the Bill link system should generate In&Out bill for the specific IB DOC ID and the status of the Bill column should be changed as Bill Posted 

namespace GsEPWv8_5_MVC.Model
{
    public class BillingInquiryModel : BasicEntity
    {
        public string is_company_user { get; set; }
        public string cmp_id { get; set; }
        public string RcvdDate { get; set; }
        public int TotalCtns { get; set; }
        public string RateId { get; set; }
        public string bill_status { get; set; }
        public string temp_bill_doc_id { get; set; }
        public string cmp_addr_line1 { get; set; }
        public string ref_no { get; set; }
        public string whs_id { get; set; }
        public string ship_to { get; set; }
        public string so_num { get; set; }
        public int TotalActivities { get; set; }
        public decimal TotalPrice { get; set; }
        public string cmp_name { get; set; }
        public string CmpID { get; set; }
        public string BillDocId { get; set; }
        public string status { get; set; }
        public string bill_type { get; set; }
        public DateTime Bill_doc_dt { get; set; }
        public string bill_dt { get; set; }
        public string cust_id { get; set; }
        //public string Cust_id { get; set; }
        public string cust_name { get; set; }
        public decimal bill_amt { get; set; }
        public string Bill_doc_id { get; set; }
        //    public string Cust_id { get; set; }
        public string Bill_type { get; set; }
        public string Bill_doc_dt_Fr { get; set; }
        public string Bill_doc_dt_To { get; set; }

        public decimal frgt_cost { get; set; }
        public string city { get; set; }
        public string state_id { get; set; }
        public string DocumentdateFrom { get; set; }
        public string DocumentdateTo { get; set; }
        public string billto_id { get; set; }
        public string sp_id { get; set; }
        public decimal prod_cost { get; set; }
        public decimal tax_pcnt { get; set; }

        public decimal TotWeight { get; set; }
        public string post_code { get; set; }
        public string addr_line1 { get; set; }
        public string remit_addr_line1 { get; set; }

        public string remit_addr_line2 { get; set; }
        public string remit_city { get; set; }
        public string remit_state_id { get; set; }

        public string remit_post_code { get; set; }
        public string fax { get; set; }
        public string tel { get; set; }


        public decimal so_itm_price { get; set; }
        public int ship_ctns { get; set; }

        public int dtl_line { get; set; }
        public string ship_itm_name { get; set; }
        public string bill_pd_fm { get; set; }
        public string bill_pd_to { get; set; }
        public string ship_doc_notes { get; set; }
        public string notes { get; set; }
        public DateTime ship_dt { get; set; }

        public string cust_ord { get; set; }
        public string cust_addr_line1 { get; set; }
        public string cust_city { get; set; }
        public string cust_state_id { get; set; }
        public string cust_post_code { get; set; }
        public string cust_addr_line2 { get; set; }

        public string bill_doc_id { get; set; }

        public string Bill_Type { get; set; }
        public string bill_free_days { get; set; }
        public string init_strg_rt_req { get; set; }
        public string po_num { get; set; }
        public string ship_doc_id { get; set; }
        public int itm_line { get; set; }
        public string remit_attn { get; set; }
        public string so_itm_num { get; set; }
        public int ship_qty { get; set; }
        public string so_itm_color { get; set; }
        public string cont_id { get; set; }
        public DateTime rcvd_dt { get; set; }
        public string lot_id { get; set; }
        public string bill_doc_dt { get; set; }
        public DateTime bill_print_dt { get; set; }
        public string kit_id { get; set; }
        public string so_itm_code { get; set; }
        public string so_itm_size { get; set; }
        public decimal ctn_qty { get; set; }
        public string term_code { get; set; }
        public string ship_itm_num { get; set; }
        public string itm_name { get; set; }
        public string contact { get; set; }
        public string addr_line2 { get; set; }
        public string bill_inout_type { get; set; }
        public string cust_fax { get; set; }
        public float cube { get; set; }
        public string loc_id { get; set; }
        public decimal length { get; set; }
        public decimal width { get; set; }
        public decimal depth { get; set; }
        public string bill_as_of_date { get; set; }
        public string bill_as_of_dt { get; set; }
        public string Bill_DOC_DT { get; set; }
        public string billperiodfr { get; set; }
        public string billperiodTo { get; set; }
        public string ship_via { get; set; }
        public string proj_id { get; set; }

        //public int length { get; set; }
        public int Width { get; set; }
        ////public int depth { get; set; }
        public decimal Cube { get; set; }
        public string CUSTId { get; set; }
        public string Billdocdt { get; set; }
        public string billdocdt { get; set; }
        public decimal CUBE { get; set; }
        public decimal Shipctn { get; set; }
        public decimal Blength { get; set; }
        public decimal Bwidth { get; set; }
        public decimal Bdepth { get; set; }
        public decimal Bcube { get; set; }

        public int Bitmline { get; set; }

        public string InvoiceStatus { get; set; }

        public string cust_of_cmpid { get; set; }
        public string cust_of_cmpname { get; set; }

        //public string Cust_ID { get; set; }
        public string curr_code { get; set; }
        public string Status { get; set; }
        public string Success { get; set; }
        public string bill_doc_id_status { get; set; }
        public string Check_existing_bill_doc_id { get; set; }
        public string tot_inv_strg_amnt { get; set; }
        public string rpt_cust_id { get; set; }
        public DateTime rpt_bill_pd_to { get; set; }
        public DateTime rpt_bill_pd_fm { get; set; }
        public string inout_bill_pd_fm { get; set; }
        public string inout_bill_pd_to { get; set; }
        public string BillFor { get; set; }
        public string vas_bill_pd_fm { get; set; }
        public string vas_bill_pd_to { get; set; }
        public string l_str_viewmode { get; set; }
        public int LineNo { get; set; }
        public string Desc { get; set; }
        public string Style { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public int Ctns { get; set; }

        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public int Line { get; set; }
        public string Description { get; set; }

        public int Ctn { get; set; }

        public decimal TotCube { get; set; }
        public string VASID { get; set; }
        public string ShipDate { get; set; }
        public string CustOrder { get; set; }
        public string ShipDocNotes { get; set; }
        public string Notes { get; set; }
        public string Inv_Notes { get; set; }

        public string VASDesc { get; set; }
        public string VASId { get; set; }
        public string ContID { get; set; }
        public string LotID { get; set; }
        public int NoOfCtn { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal QTYPrice { get; set; }
        public string ServiceId { get; set; }
        public string ContId { get; set; }
        public string LotId { get; set; }

        public string RptStatus { get; set; }
        public string ReportStatus { get; set; }
        public string print_bill_date { get; set; }
        public string ib_doc_id { get; set; } //CR-3PL_MVC_IB_2018_0219_004
        public string rate_id { get; set; }
        public string Image_Path { get; set; }
        public string itm_num { get; set; }
        public decimal list_price { get; set; }
        public string price_uom { get; set; }
        public string sec_strg_rate { get; set; }
        public int strg_days { get; set; }
        public string Grid_Bill_Type { get; set; }
        public string doc_dt { get; set; }                  //CR_3PL_MVC_BL_2018_0308_001 Added by Soniya
        public string tmp_cmp_id { get; set; }
        public string screentitle { get; set; }

        public string rpt_notes { get; set; }
        public string IS3RDUSER { get; set; }
        public string rpt_dtl_notes { get; set; }
        public string so_price_uom { get; set; }
        public string rpt_hdr_notes { get; set; }
        public decimal blso_itm_price { get; set; }
        public int bl_shipctns { get; set; }
        public float blcube { get; set; }
        public string blType { get; set; }
        public string itm_code { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string RateType { get; set; }
        public string st_rate_id { get; set; }
        public int itm_qty { get; set; }
        public int avail_qty { get; set; }
        public int TotCtns { get; set; }
        public decimal TotAmount { get; set; }
        public int wgt { get; set; }
        public int avail_cnt { get; set; }
        public decimal ship_itm_price { get; set; }
        public string cust_addr1 { get; set; }
        public string cust_state { get; set; }
        public string cust_postcode { get; set; }
        public string custtel { get; set; }
        public string custfax { get; set; }
        public string catg { get; set; }
        public DateTime palet_dt { get; set; }
        public DateTime ib_doc_dt { get; set; }

        public string cust_ordr_num { get; set; }
        public string ShipDt { get; set; }
        public string Custaddr_line2 { get; set; }
        public string Custaddr_line1 { get; set; }
        public string Custpost_code { get; set; }
        public string Custstate_id { get; set; }
        public string Custcity { get; set; }
        public string pmt_term { get; set; }
        public string vas_dtl_notes { get; set; }

        public ClsBillVASReBill objBillVASReBill = new ClsBillVASReBill();
        public List<ClsBillVAS> lstBillVASList { get; set; }
        public IList<BillingInquiry> ListBillingInquiry { get; set; }
        public IList<BillingInquiry> ListBillingSummaryRpt { get; set; }
        public IList<BillingInquiry> ListBillingInvoiceRpt { get; set; }
        public IList<BillingInquiry> ListBillingType { get; set; }
        public IList<BillingInquiry> ListBillingInoutType { get; set; }
        public IList<BillingInquiry> ListBillingDocIdType { get; set; }
        public IList<BillingInquiry> ListBillingDocVASRpt { get; set; }
        public IList<BillingInquiry> ListBillingInoutCartonRpt { get; set; }
        public IList<BillingInquiry> ListBillingDocSTRGCartonwithinitRpt { get; set; }
        public IList<BillingInquiry> ListBillingInoutCartonInstrgRpt { get; set; }
        public IList<BillingInquiry> ListBillingInoutCubeInstrgRpt { get; set; }
        public IList<BillingInquiry> ListBillingInoutCubeRpt { get; set; }
        public IList<BillingInquiry> ListBillingInoutItemRpt { get; set; }
        public IList<BillingInquiry> ListBillingInoutLocRpt { get; set; }
        public IList<BillingInquiry> ListBillingdetail { get; set; }
        public IList<BillingInquiry> ListBillingDocSTRGCubewithinitRpt { get; set; }


        public IList<Company> ListCompanyDtl { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<LookUp> ListLookUpDtl { get; set; }
        public IList<Company> ListCustofCompanyPickDtl { get; set; }
        public IList<BillingInquiry> ListBillingInvStatus { get; set; }
        public IList<BillingInquiry> ListLoadSTRGBillDetails { get; set; }
        public IList<BillingInquiry> ListBillRcvdDetails { get; set; }
        public IList<BillingInquiry> ListSaveSTRGBillDetails { get; set; }
        public IList<BillingInquiry> ListCheckExistingSTRGBillDocId { get; set; }
        public IList<BillingInquiry> ListSTRGBillDocStatus { get; set; }
        public IList<BillingInquiry> ListLoadInoutBillDetails { get; set; }
        public IList<BillingInquiry> ListCheckExistingInOutBillDocId { get; set; }
        public IList<BillingInquiry> ListSaveVASBillDetails { get; set; }
        public IList<BillingInquiry> ListCheckExistingVASBillDocId { get; set; }
        public IList<BillingInquiry> ListCheckExistingVasBillDocId { get; set; }
        public IList<BillingInquiry> ListBillingStrgExcel { get; set; }
        public IList<Company> LstCustOfCmpName { get; set; } //CR - 3PL_MVC_IB_2018_0219_004
        public IList<BillingInquiry> LstDocRcvdDt { get; set; } //CR - 3PL_MVC_IB_2018_0219_004
        public IList<BillingInquiry> ListGetSecondSTRGRate { get; set; }
        public IList<BillingInquiry> ListGetSTRGBillByPallet { get; set; }
        public IList<BillingInquiry> ListGetInoutBillByContainer { get; set; }
        public IList<BillingInquiry> ListBillingamountInoutCartonRpt { get; set; }

        public IList<BillingInquiry> ListGetSTRGBillByLoc { get; set; }
        public IList<BillingInquiry> ListGetSTRGBillByLocRpt { get; set; }
        public IList<BillingInquiry> ListGetSTRGBillByPcsRpt { get; set; }
        public IList<BillingInquiry> LstTempBilldetails { get; set; }
        public IList<Company> LstCmpName { get; set; }


    }
}
