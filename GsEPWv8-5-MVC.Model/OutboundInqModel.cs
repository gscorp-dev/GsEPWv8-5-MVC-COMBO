using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//CR#

namespace GsEPWv8_5_MVC.Model
{
    public class OutboundInqModel : BasicEntity
    {
        public CustConfig objCustConfig { get; set; }
        public string quote_num { get; set; }
        public string b_quote_num { get; set; }
        public string Comments { get; set; }
        public string file_path { get; set; }
        public int totalctns { get; set; }
        public string ship_hdr_notes { get; set; }
        public decimal totalcube { get; set; }
        public DateTime OBShipdt { get; set; }
       public string doctype { get; set; }
        public DateTime Uploaddt { get; set; }
        public decimal ordr_cube { get; set; }
        public string Uploadby { get; set; }
        public string so_num { get; set; }
        public string filepath { get; set; }
        public string Batchid { get; set; }
        public string shipdocid { get; set; }
        public string UPLOAD_FILE { get; set; }
        public string aloc_doc_id { get; set; }
        public string cmp_id { get; set; }
        public string status { get; set; }
        public string step { get; set; }
        public string Filename { get; set; }
        public string so_dt { get; set; }
        public string cust_id { get; set; }
        public string user_id { get; set; }
        public int Itmline { get; set; }
        public string ordr_num { get; set; }
        public string cust_name { get; set; }
        public string cust_ordr_num { get; set; }
        public string store_id { get; set; }
        public string so_numFm { get; set; }
        public string so_numTo { get; set; }
        public string ShipdtFm { get; set; }
        public string ShipdtTo { get; set; }
        public string so_dtFm { get; set; }
        public string route_dt { get; set; }
        public string so_dtTo { get; set; }
        public string Sonum { get; set; }
        public string VasId { get; set; }
        public string SoDt { get; set; }
        public string CustId { get; set; }
        public string CustOrderNo { get; set; }
        public string StoreId { get; set; }
        public string AlocdocId { get; set; }
        public string cmp_name { get; set; }
        public string city { get; set; }
        public string state_id { get; set; }
        public string post_code { get; set; }
        public string addr_line1 { get; set; }
        public string tel { get; set; }
        public string fax { get; set; }
        public int line_num { get; set; }
        public int ctn_qty { get; set; }
        public decimal cube { get; set; }
        public decimal pick_cube { get; set; }
        public decimal aloc_cube { get; set; }
        public string note { get; set; }
        public int ctn_cnt { get; set; }
        public string loc_id { get; set; }
        public string itm_name { get; set; }
        public string itm_code { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string so_itm_num { get; set; }
        public string itm_size { get; set; }
        public string cancel_dt { get; set; }
        public string ship_dt { get; set; }
        public string shipto_id { get; set; }
        public string shipvia_id { get; set; }
        public string freight_id { get; set; }
        public string fob { get; set; }
        public string dept_id { get; set; }
        public string terms_id { get; set; }
        public string dc_id { get; set; }
        public string due_dt { get; set; }
        public string lot_id { get; set; }
        public int itm_line { get; set; }
        public int ordr_ctns { get; set; }
        public string qty_uom { get; set; }
        public int ordr_qty { get; set; }
        public int back_ordr_qty { get; set; }
        public int ship_qty { get; set; }
        public int total_qty { get; set; }
        public string po_num { get; set; }
        public string CompID { get; set; }
        public string ShipReqID { get; set; }
        public string Status { get; set; }
        public string Step { get; set; }
        public string ShipReqDt { get; set; }
        public string OrdType { get; set; }
        public string CustID { get; set; }
        public string CustName { get; set; }
        public string OrdNum { get; set; }
        public string CustPO { get; set; }
        public string Container { get; set; }
        public string QuoteNum { get; set; }
        public string DeptID { get; set; }
        public string StoreID { get; set; }
        public string FOB { get; set; }
        public string Note { get; set; }
        public string DCID { get; set; }
        public string ShipToID { get; set; }
        public string ShipToAttn { get; set; }
        public string ShipToEmail { get; set; }
        public string ShipToAddr1 { get; set; }
        public string ShipToAddr2 { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToState { get; set; }
        public string ShipToZipCode { get; set; }
        public string ShipToCountry { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public int LineNum { get; set; }
        public string Style { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public decimal OrdQty { get; set; }
        public int OrdCtns { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public string Cube { get; set; }
        public string QtyUOM { get; set; }
        public string ShipVia { get; set; }
        public string ShipID { get; set; }
        public string ShipDt { get; set; }
        public string CancelDt { get; set; }
        public string FreightID { get; set; }
        public string Cust_id { get; set; }
        public string STATUS { get; set; }
        public string STEP { get; set; }
        public string STOREID { get; set; }
        public string CUSTID { get; set; }
        public string So_dt { get; set; }
        public string RptResult { get; set; }
        public string ShipreqDt { get; set; }
        public string pricetkt { get; set; }
        public string Type { get; set; }
        public string refno { get; set; }
        public string AuthId { get; set; }
        public string shipchrg { get; set; }
        public string CustOrderdt { get; set; }
        public string Itmdtl { get; set; }
        public string file_name { get; set; }
        public decimal ppk { get; set; }
        public decimal ord_qty { get; set; }
        public decimal ctn { get; set; }
        public decimal length { get; set; }
        public decimal width { get; set; }
        public decimal height { get; set; }
        public decimal weight { get; set; }
        public string vendpo { get; set; }
        public string SRNUm { get; set; }
        public string Attn { get; set; }
        public string Mailname { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string country { get; set; }
        public string zipcode { get; set; }
        public string state { get; set; }
        public string City { get; set; }
        public decimal avlqty { get; set; }
        public string id { get; set; }
        public string Errdesc { get; set; }
        public decimal wgt { get; set; }
        public decimal itm_cube { get; set; }
        public decimal line_cube { get; set; }
        public decimal line_wgt { get; set; }
        public decimal depth { get; set; }
        public decimal list_price { get; set; }
        public string list_uom { get; set; }
        public decimal sell_price { get; set; }
        public string sell_uom { get; set; }
        public string pack_id { get; set; }
        public string comm_pcnt { get; set; }
        public int itm_qty { get; set; }
        public decimal aloc_qty { get; set; }
        public string FullfillType { get; set; }
        public string recv_source { get; set; }
        public string kit_itm { get; set; }
        public string cust_itm_num { get; set; }
        public string cust_itm_color { get; set; }
        public string cust_itm_desc { get; set; }
        public string price_uom { get; set; }
        public decimal dtlsize { get; set; }
        public string avail_dt { get; set; }
        public string aloc_dt { get; set; }
        public string pick_dt { get; set; }
        public string load_dt { get; set; }
        public decimal ordr_cost { get; set; }
        public string style { get; set; }
        public string color { get; set; }
        public string size { get; set; }
        public string Name { get; set; }
        public int orderqty { get; set; }
        public string Notes { get; set; }
        public string ponum { get; set; }
        public string p_str_cmpid { get; set; }
        public int rowno { get; set; }
        public decimal shipppk { get; set; }
        public string ShipStyle { get; set; }
        public string shipcolor { get; set; }
        public string Shipsize { get; set; }
        public string shipname { get; set; }
        public string shipponum { get; set; }
        public decimal shipqty { get; set; }
        public decimal shipwgt { get; set; }
        public decimal shipctn { get; set; }
        public decimal shipdepth { get; set; }
        public string shipqtyuom { get; set; }
        public string shiplstuom { get; set; }
        public string price_tkt { get; set; }
        public string ordr_type { get; set; }
        public string sh_chg { get; set; }
        public string Dept_ID { get; set; }
        public string Cust_ordr_dt { get; set; }
        public string Cancel_dt { get; set; }
        public string Ship_dt { get; set; }
        public string in_sale_id { get; set; }
        public string soldto_id { get; set; }
        public string sl_attn { get; set; }
        public string sl_mail_name { get; set; }
        public string sl_addr_line1 { get; set; }
        public string sl_addr_line2 { get; set; }
        public string sl_city { get; set; }
        public string sl_state_id { get; set; }
        public string sl_post_code { get; set; }
        public string sl_cntry_id { get; set; }
        public decimal avail_qty { get; set; }
        public decimal shiplength { get; set; }
        public decimal shipwidth { get; set; }
        public decimal shipheight { get; set; }
        public decimal shipweight { get; set; }
        public decimal shipcube { get; set; }

        public string State_ID { get; set; }
        public string State_Name { get; set; }
        public string Cntry_Id { get; set; }
        public string Cntry_Name { get; set; }
        public string attn { get; set; }
        public string DC_id { get; set; }
        public string mail_name { get; set; }
        public string addr_line2 { get; set; }
        public string cntry_id { get; set; }
        public string Countrydtl { get; set; }
        public string statedtl { get; set; }
        public string ShipEntry { get; set; }
        public string uom_id { get; set; }
        public string uom_desc { get; set; }
        public string Check_existing_uom { get; set; }

        public string p_str_Mode { get; set; }
        public string Check_itm_code { get; set; }
        public string is_company_user { get; set; }
 public string out_sale_id { get; set; }
        public string ship_doc_id { get; set; }
        public string View_Flag { get; set; }
 public string so_itm_color { get; set; }
        public string so_itm_size { get; set; }

        public int pick_qty { get; set; }
        public string process_id { get; set; }
        public string pkg_id { get; set; }
        public int kit_qty { get; set; }
        public string pkg_type { get; set; }
        public string aloc_type { get; set; }
        public string ship_to_name { get; set; }
        public string cntr_num { get; set; }
        public string ship_to_id { get; set; }
        public string ShipDocNum { get; set; }
        public string bol_num { get; set; }
        public string Compname { get; set; }
        public string ShipReqIDFm { get; set; }
        public string ShipReqIDTo { get; set; }
        public string DeliverydtTo { get; set; }
        public string DeliverydtFm { get; set; }
        public string whsid { get; set; }
        public string whsname { get; set; }
        public string lblshiptoid { get; set; }
        public string orderNo { get; set; }
        public string cust_ordr_dt { get; set; }
        public int due_line { get; set; }
        public int due_qty { get; set; }
        public int dtl_line { get; set; }
        public string whs_id { get; set; }
        public string aloc_sort_stmt { get; set; }
        public string aloc_by { get; set; }
        public int pkg_avail_cnt { get; set; }
        public int reqQty { get; set; }
        public int aloc_line { get; set; }
        public int Soline { get; set; }
        public int DueLine { get; set; }
        public int Line { get; set; }
        public DateTime rcvd_dt { get; set; }
        public int pkg_qty { get; set; }
        public int ctn_line { get; set; }
        public string Palet_id { get; set; }
        public int Aloc { get; set; }
        public int ItmLn { get; set; }
        public int CtnLn { get; set; }
        public int AlcLn { get; set; }
        public int Bal { get; set; }
        public int ReturnValue { get; set; }
        public string so_itm_code { get; set; }
        public string Recvddt { get; set; }
        public int lineqty { get; set; }
        public string kit_type { get; set; }
        public string kit_id { get; set; }
        public string palet_id { get; set; }
        public string cont_id { get; set; }
        public string bill_status { get; set; }
        public string st_rate_id { get; set; }
        public string io_rate_id { get; set; }
        public string doc_id { get; set; }
        public string doc_date { get; set; }
        public string doc_notes { get; set; }
        public string group_id { get; set; }
        public string fmto_name { get; set; }
        public string lbl_id { get; set; }
        public string grn_id { get; set; }
        public string ib_doc_id { get; set; }
        public int soline { get; set; }
        public int dueline { get; set; }
        public int SRRowcount { get; set; }
        public string doc_pkg_id { get; set; }
        public string new_pkg_qty { get; set; }
        public bool l_str_aloc_aloc_dtls { get; set; }
        public int l_int_sr_detail { get; set; }
        public string SoNotes { get; set; }
        public int line_qty { get; set; }
        public int loc_bal_qty { get; set; }
        public DateTime canceldt { get; set; }
        public DateTime Shippingdt { get; set; }
        public DateTime AlcDate { get; set; }
        public string alocdptid { get; set; }
        public string alocstateid { get; set; }
        public string alocpostcode { get; set; }
        public string aloccity { get; set; }
        public string alocaddrline1 { get; set; }
        public string dft_whs { get; set; }
        public DateTime canceldate { get; set; }
        public DateTime shipdate { get; set; }
        public string deptid { get; set; }
        public string dcid { get; set; }
        public int selalocated { get; set; }
        public int alocated { get; set; }
        public int available { get; set; }
        public int que_qty { get; set; }
        public int pkgqty { get; set; }
        public int Selaloc { get; set; }
        public string locid { get; set; }
        public int pkg_cnt { get; set; }
        public int Avail { get; set; }
        public string whs_name { get; set; }
        public string billto_id { get; set; }
        public string colChk { get; set; }
        public string so_hdr_note { get; set; }
        public string screentitle { get; set; }//CR_MVC_3PL_0317-001 Added By Nithya
        public string l_bool_edit_flag { get; set; }//CR_MVC_3PL_0320-001 Added By Nithya
        public int l_int_Aloc_Summary_Count { get; set; }//CR_MVC_3PL_0326-001 Added By Nithya
        public int l_int_Aloc_BackOrder_Count { get; set; }//CR_MVC_3PL_0327-001 Added By Nithya
public string tmp_cmp_id { get; set; }
        public string ItmCode { get; set; }
        public string IS3RDUSER { get; set; }
        public string Ship_Post_Dt { get; set; }
        public string ObDcId { get; set; }

        public string Image_Path { get; set; }


        public string cmp_city { get; set; }
        public string cmp_state_id { get; set; }

        public string cmp_post_code { get; set; }
        public int balance_qty { get; set; }//CR-20180522-001 Added By Nithya
        public int OrderQty { get; set; }//CR-20180522-001 Added By Nithya
        public int Back_Order_Qty { get; set; }//CR-20180522-001 Added By Nithya
        public int Aloc_Qty { get; set; }//CR-20180522-001 Added By Nithya
        public int backorderCount { get; set; }//CR-20180522-001 Added By Nithya
        public int TotRecs { get; set; }

        public string strg_rate { get; set; }
        public string inout_rate { get; set; }
        public string rcvd_notes { get; set; }
        public string ct_value { get; set; }
        public string obItmdtl { get; set; }
        public string obItmcolor { get; set; }
        public string obItmsize { get; set; }
        public string obitmname { get; set; }
        public string ScreenMode { get; set; }
        public string Shipstatus { get; set; }
        public string Vasstatus { get; set; }
        public string vas_bill_status { get; set; }
        public DateTime shippost_dt { get; set; }
        public string bill_doc_id { get; set; }
        public string VasDate { get; set; }
        public string DocumentdateFrom { get; set; }
        public string DocumentdateTo { get; set; }
        public string Custdtl { get; set; }
        public string Custname { get; set; }
	public string ship_city{get;set;}
	public string ship_state{get;set;}
        public string Stk_Chk_Reqt { get; set; }

        public string st_mail_name { get; set; }
        public string st_addr_line1 { get; set; }
        public string st_addr_line2 { get; set; }
        public string st_city { get; set; }
        public string st_state_id { get; set; }
        public string st_post_code { get; set; }
        public string st_cntry_id { get; set; }
        public string load_doc_id { get; set; }
        public string load_number { get; set; }
        public string load_approve_dt { get; set; }
        public string load_pick_dt { get; set; }

        public string pick_no { get; set; }
        public string ref_no { get; set; }
        public string temp_stk_ref_num { get; set; }
        public string bo_flag { get; set; }
        public int doc_label { get; set; }
        public int doc_pack { get; set; }
        public string track_num { get; set; }
        public SOTracking objSOTracking { get; set; }
        public IList<SOTracking> ListSOTracking { get; set; }
        public IList<OutboundInq> LstOutboundInqdetail { get; set; }
        public IList<LookUp> ListLookUpDtl { get; set; }
        public IList<Company> ListCompanyDtl { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<OutboundInq> LstOutboundInqSummaryRpt { get; set; }
        public IList<OutboundInq> LstOutboundInqpickstyleRpt { get; set; }
        public IList<OB_SR_ACKExcel> LstOutboundInqpickstyleRept { get; set; }

        public IList<OutboundInq> LstItmxCustdtl { get; set; }
        public IList<OutboundInq> ListSRDocId { get; set; }
  public IList<OutboundInq> ListGetOutSaleId { get; set; }
   public IList<OutboundInq> LstGetAlocType { get; set; }
        public IList<LookUp> Listtype { get; set; }
        public IList<LookUp> Listpricetkt { get; set; }
        public IList<OutboundInq> ListCustId { get; set; }
        public IList<OutboundInq> ListShipToAddress { get; set; }
        public IList<LookUp> Listfrieghtid { get; set; }
        public IList<OutboundInq> ListItmStyledtl { get; set; }
        public IList<OutboundInq> lstobjOutboundInq { get; set; }
 public IList<OutboundInq>  LstOutboundInqAlocGridLoadDtls { get; set; }
        public IList<OutboundInq> LstOutboundInqPickGridLoadDtls { get; set; }
        public IList<OutboundInq> LstOutboundUnPickQty { get; set; }
        public IList<OutboundInq> LstCheckAllocPost { get; set; }
        public IList<OutboundInq>  ListShipDocNum { get; set; }
        public IList<Pick> ListPick { get; set; }
        public IList<OutboundInq> ListPickdtl { get; set; }
        public IList<OutboundInq> LstAlocUnPostGridLoadDtls { get; set; }
        public IList<Pick> ListCntryPick { get; set; }
        public IList<Pick> ListStatePick { get; set; }
        public IList<Pick> ListShipToPick { get; set; }
        public IList<OutboundInq> ListCheckExistStyle { get; set; }
        public IList<Pick> ListExistShipToAddrsPick { get; set; }
        public IList<Pick> ListUomPick { get; set; }
        public IList<OutboundInq> ListGridEditData { get; set; }
        public IList<Pick> ListCheckItemCode { get; set; }
        public IList<OutboundInq> lstShiptoAddrsSave { get; set; }
        public IList<OutboundInq> lstShipAlocdtl { get; set; }
        public IList<OutboundInq> ListLoadShipReqEditDtl { get; set; }
        public IList<Company> Lstcmpdtl { get; set; }
        public IList<Company> Lstwhsdtl { get; set; }
        public IList<Company> Lstcustdtl { get; set; }
        public IList<Company> LstCmpName { get; set; }
        public IList<OutboundInq> LstAlocdocid { get; set; }
        public IList<OutboundInq> LstAvailqty { get; set; }
        public IList<OutboundInq> LstAlocSummary { get; set; }
        public IList<OutboundInq> LstAlocDtl { get; set; }
        public IList<OutboundInq> Lstpkgid { get; set; }
        public IList<OutboundInq> LstManualAloc { get; set; }
        public IList<OutboundInq> LstFiledtl { get; set; }
        public IList<OutboundInq> LstOutboundPickQty { get; set; } //CR20180504-001 Added By Nithya
        public IList<OutboundInq> LstStockverify { get; set; }//CR-20180522-001 Added By Nithya
        public IList<OutboundInq> LstStkverifyList { get; set; }//CR-20180522-001 Added By Nithya
        public IList<OutboundInq> LstConsolidatedRptByStyle { get; set; }//CR-20180522-001 Added By Nithya
        public IList<OutboundInq> LstConsolidatedRptByLoc { get; set; }//CR-20180522-001 Added By Nithya
        public IList<OutboundInq> LstLocId { get; set; }//CR-20180529-001 Added By Nithya
        public IList<OutboundInq> LstLotId { get; set; }//CR-20180529-001 Added By Nithya
        public IList<OutboundInq> LstIbdocId { get; set; }//CR-20180529-001 Added By Nithya
        public IList<OutboundInq> ListRMADocId { get; set; }
        public IList<OutboundInq> ListPaletId { get; set; }
        public IList<Company> ListCompanyAddresHdrDtls { get; set; }
        public IList<OutboundInq> ListeCom940SRUploadDtlRpt { get; set; }
        public IList<OutboundInq> LstStkverifyListTotal { get; set; }
        public IList<OutboundInq> LstItmDtls { get; set; }
        public IList<OutboundInq> LstItmdueQty { get; set; }

        public int upload_file_count { get; set; }
        public ob_so_hdr2 so_hdr2 { get; set; }
        public List<ClsOBSRAlocDtl> lstOBSRAlocDtl { get; set; }
        private List<clsEComPrintOrders> EComPrintOrdersField;
        private List<clsEComPrintOrders> EComProcOrdersField;
        public List<clsEComPrintOrders> ListEComPrintOrders
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
        public List<clsEComPrintOrders> ListEComProcOrders
        {
            get
            {
                return EComProcOrdersField;
            }

            set
            {
                EComProcOrdersField = value;
            }
        }

        private List<clsEcomPrintAloc> EcomPrintAlocField;
        private List<clsEcomPrintDoc> EcomPrintDocField;
        public List<clsEcomPrintAloc> ListEcomPrintAloc
        {
            get
            {
                return EcomPrintAlocField;
            }

            set
            {
                EcomPrintAlocField = value;
            }
        }
        public List<clsEcomPrintDoc> ListEcomPrintDoc
        {
            get
            {
                return EcomPrintDocField;
            }

            set
            {
                EcomPrintDocField = value;
            }
        }
    }

    public class ob_so_hdr2
    {
        public string cmp_id { get; set; }
        public string so_num { get; set; }
        public string pick_no { get; set; }
        public string ref_no { get; set; }
        public string load_no { get; set; }
        public string load_approve_dt { get; set; }
        public string load_pick_dt { get; set; }
        public string carrier_name { get; set; }
    }
    public class OBGetBOLConfModel
    {
        public string cmp_id { get; set; }
        public string cmp_name { get; set; }
        public string addr_line1 { get; set; }
        public string addr_line2 { get; set; }
        public string city { get; set; }
        public string state_id { get; set; }
        public string post_code { get; set; }
        public string tel { get; set; }
        public string fax { get; set; }
        public string so_num { get; set; }
        public string quote_num { get; set; }
        public string cust_name { get; set; }
        public string cust_ordr_num { get; set; }
        public string aloc_doc_id { get; set; }
        public string ship_dt { get; set; }
        public string ship_doc_id { get; set; }
        public string picked_by { get; set; }
        public string ship_via_name { get; set; }
        public string ship_post_dt { get; set; }
        public string shipto_id { get; set; }
        public string st_mail_name { get; set; }
        public string st_addr_line1 { get; set; }
        public string st_addr_line2 { get; set; }
        public string st_city { get; set; }
        public string st_state_id { get; set; }
        public string st_post_code { get; set; }
        public string st_cntry_id { get; set; }
        public string line_num { get; set; }
        public string ctn_line { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string po_num { get; set; }
        public string ctn_qty { get; set; }
        public int ctn_cnt { get; set; }
        public int line_qty { get; set; }
        public string pick_uom { get; set; }

        public IList<OBGetBOLConf> ListOBBOLConfRpt { get; set; }

    }

    public class OBGetSRBOLConfRptModel
    {
        public string load_doc_id { get; set; }
        public string cmp_id { get; set; }
        public string cmp_name { get; set; }
        public string addr_line1 { get; set; }
        public string addr_line2 { get; set; }
        public string city { get; set; }
        public string state_id { get; set; }
        public string post_code { get; set; }
        public string tel { get; set; }
        public string fax { get; set; }
        public string so_num { get; set; }
        public string quote_num { get; set; }
        public string cust_name { get; set; }
     
        public string cust_ordr_num { get; set; }
        public string ordr_num { get; set; }
        public string so_dt { get; set; }
        public string shipto_id { get; set; }
        public string st_mail_name { get; set; }
        public string st_addr_line1 { get; set; }
        public string st_addr_line2 { get; set; }
        public string st_city { get; set; }
        public string st_state_id { get; set; }
        public string st_post_code { get; set; }
        public string st_cntry_id { get; set; }
        public int tot_ctns { get; set; }
        public decimal tot_weight { get; set; }
        public decimal tot_cube { get; set; }
        public string load_number { get; set; }
        public string bol_number { get; set; }
        public string spcl_inst { get; set; }
        public string carrier_name { get; set; }
        public string trailer_num { get; set; }
        public string seal_num { get; set; }
        public int load_ctns { get; set; }
        public decimal load_cube { get; set; }
        public decimal load_weight { get; set; }

        public IList<OBGetSRBOLConfRpt> ListOBGetSRBOLConfRpt { get; set; }

    }


    public class OBSRLoadInquiryModel
    {
        
        private string _cmp_id;
        private string _load_doc_id;
        private string _batch_num;
        private string _so_num_from;
        private string _so_num_to;
        private string _so_dt_from;
        private string _so_dt_to;
        private string _load_number;
        private string _load_approve_dt;
        private string _bol_number;
        private string _spcl_inst;
        private string _load_pick_dt;
        private string _carrier_name;
        private string _trailer_num;
        private string _seal_num;
        private string _shipto_id;
        private string _st_mail_name;
        private string _st_addr_line1;
        private string _st_addr_line2;
        private string _st_city;
        private string _st_state_id;
        private string _st_post_code;
        private string _st_cntry_id;
        private string _load_note;
        private int _tot_ctns;
        private decimal _tot_cube;
        private decimal _tot_weight;
        private decimal _grant_tot_weight;
        private int _grant_tot_ctns;
        private decimal _grant_tot_cube;
        private int _tot_palet;
        private string _maker;
        private string _maker_dt;
        private string _single_sr;
        private string _print_summary;
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string load_doc_id { get { return _load_doc_id; } set { _load_doc_id = value; } }
        public string batch_num { get { return _batch_num; } set { _batch_num = value; } }
        public string so_num_from { get { return _so_num_from; } set { _so_num_from = value; } }
        public string so_num_to { get { return _so_num_to; } set { _so_num_to = value; } }
        public string so_dt_from { get { return _so_dt_from; } set { _so_dt_from = value; } }
        public string so_dt_to { get { return _so_dt_to; } set { _so_dt_to = value; } }
        public string load_number { get { return _load_number; } set { _load_number = value; } }
        public string load_approve_dt { get { return _load_approve_dt; } set { _load_approve_dt = value; } }
        public string bol_number { get { return _bol_number; } set { _bol_number = value; } }
        public string spcl_inst { get { return _spcl_inst; } set { _spcl_inst = value; } }
        public string load_pick_dt { get { return _load_pick_dt; } set { _load_pick_dt = value; } }
        public string shipto_id { get { return _shipto_id; } set { _shipto_id = value; } }
        public string carrier_name { get { return _carrier_name; } set { _carrier_name = value; } }
        public string trailer_num { get { return _trailer_num; } set { _trailer_num = value; } }
        public string seal_num { get { return _seal_num; } set { _seal_num = value; } }

        public string st_mail_name { get { return _st_mail_name; } set { _st_mail_name = value; } }
        public string st_addr_line1 { get { return _st_addr_line1; } set { _st_addr_line1 = value; } }
        public string st_addr_line2 { get { return _st_addr_line2; } set { _st_addr_line2 = value; } }
        public string st_city { get { return _st_city; } set { _st_city = value; } }
        public string st_state_id { get { return _st_state_id; } set { _st_state_id = value; } }
        public string st_post_code { get { return _st_post_code; } set { _st_post_code = value; } }
        public string st_cntry_id { get { return _st_cntry_id; } set { _st_cntry_id = value; } }
        public string load_note { get { return _load_note; } set { _load_note = value; } }
        public int tot_ctns { get { return _tot_ctns; } set { _tot_ctns = value; } }
        public decimal tot_cube { get { return _tot_cube; } set { _tot_cube = value; } }
        public decimal tot_weight { get { return _tot_weight; } set { _tot_weight = value; } }
        public decimal grant_tot_cube { get { return _grant_tot_cube; } set { _grant_tot_cube = value; } }
        public int tot_palet { get { return _tot_palet; } set { _tot_palet = value; } }
        public string maker { get { return _maker; } set { _maker = value; } }
        public string maker_dt { get { return _maker_dt; } set { _maker_dt = value; } }
        public string single_sr { get { return _single_sr; } set { _single_sr = value; } }
        public string print_summary { get { return _print_summary; } set { _print_summary = value; } }
        public decimal grant_tot_weight { get { return _grant_tot_weight; } set { _grant_tot_weight = value; } }
        public int grant_tot_ctns { get { return _grant_tot_ctns; } set { _grant_tot_ctns = value; } }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<OBGetSRSummary> ListOBGetSRSummary { get; set; }
        public IList<OBBOLDtl> ListOBBOLDtl { get; set; }
        public IList<OBSRShipTo> ListOBSRShipTo { get; set; }
        public IList<OBSRLoadEntryDtl> ListOBSRLoadEntryDtl { get; set; }
        public IList<OBSRLoadEntryHdr> ListOBSRLoadEntryHdr { get; set; }
        public IList<Pick> ListCntryPick { get; set; }
        public IList<Pick> ListStatePick { get; set; }
        public IList<OBDocExcp> ListOBDocExcp { get; set; }
      
    }


    public class OBAlocPostInquiryModel
    {
        private string _cmp_id;
        private string _batch_num;
        private string _load_number;
        private string _so_num_from;
        private string _so_num_to;
        private string _so_dt_from;
        private string _so_dt_to;
        private string _aloc_post_dt;


        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string batch_num { get { return _batch_num; } set { _batch_num = value; } }
        public string load_number { get { return _load_number; } set { _load_number = value; } }
        public string so_num_from { get { return _so_num_from; } set { _so_num_from = value; } }
        public string so_num_to { get { return _so_num_to; } set { _so_num_to = value; } }
        public string so_dt_from { get { return _so_dt_from; } set { _so_dt_from = value; } }
        public string so_dt_to { get { return _so_dt_to; } set { _so_dt_to = value; } }
        public string aloc_post_dt { get { return _aloc_post_dt; } set { _aloc_post_dt = value; } }



        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<OBGetAlocSummary> ListOBGetAlocOpenSummary { get; set; }
        public IList<OBAlocPostByBatchDtl> ListOBAlocPostByBatchDtl { get; set; }
    }

    public class OBCtnLblPrntModel
    {
        #region Constructors  
        public OBCtnLblPrntModel() { }

        #endregion
        #region Private Fields  
        private string _cmp_id;
        private string _so_num;
        private string _ordr_num;
        private string _cust_ordr_num;
        private string _st_mail_name;
        private string _st_addr_line1;
        private string _st_city;
        private string _st_state_id;
        private string _st_post_code;
        private string _cmp_name;
        private string _addr_line1;
        private string _city;
        private string _state_id;
        private string _post_code;
        private string _ship_from;
        private string _ship_from_cmp;
        private string _ship_from_addr1;
        private string _ship_from_addr2;
        private string _ship_to;
        private string _ship_to_addr1;
        private string _ship_to_addr2;
        private int _tot_ctns;
        private string _lbl_per_page;
        private string _lbl_layout;
        private string _lbl_format;
        private string _store_id;
        private string _dept_id;
        #endregion

        #region Public Properties  
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string so_num { get { return _so_num; } set { _so_num = value; } }
        public string ordr_num { get { return _ordr_num; } set { _ordr_num = value; } }
        public string cust_ordr_num { get { return _cust_ordr_num; } set { _cust_ordr_num = value; } }
        public string st_mail_name { get { return _st_mail_name; } set { _st_mail_name = value; } }
        public string st_addr_line1 { get { return _st_addr_line1; } set { _st_addr_line1 = value; } }
        public string st_city { get { return _st_city; } set { _st_city = value; } }
        public string st_state_id { get { return _st_state_id; } set { _st_state_id = value; } }
        public string st_post_code { get { return _st_post_code; } set { _st_post_code = value; } }
        public string cmp_name { get { return _cmp_name; } set { _cmp_name = value; } }
        public string addr_line1 { get { return _addr_line1; } set { _addr_line1 = value; } }
        public string city { get { return _city; } set { _city = value; } }
        public string state_id { get { return _state_id; } set { _state_id = value; } }
        public string post_code { get { return _post_code; } set { _post_code = value; } }
        public string ship_from { get { return _ship_from; } set { _ship_from = value; } }
        public string ship_from_cmp { get { return _ship_from_cmp; } set { _ship_from_cmp = value; } }
        public string ship_from_addr1 { get { return _ship_from_addr1; } set { _ship_from_addr1 = value; } }
        public string ship_from_addr2 { get { return _ship_from_addr2; } set { _ship_from_addr2 = value; } }
        public string ship_to { get { return _ship_to; } set { _ship_to = value; } }
        public string ship_to_addr1 { get { return _ship_to_addr1; } set { _ship_to_addr1 = value; } }
        public string ship_to_addr2 { get { return _ship_to_addr2; } set { _ship_to_addr2 = value; } }
        public int tot_ctns { get { return _tot_ctns; } set { _tot_ctns = value; } }
        public string lbl_per_page { get { return _lbl_per_page; } set { _lbl_per_page = value; } }
        public string lbl_layout { get { return _lbl_layout; } set { _lbl_layout = value; } }
        public string lbl_format { get { return _lbl_format; } set { _lbl_format = value; } }
        public IList<OBCtnLblPrnt> LstCtnLabelPrint { get; set; }
        public string store_id { get { return _store_id; } set { _store_id = value; } }
        public string dept_id { get { return _dept_id; } set { _dept_id = value; } }
        public bool store_ready { get; set; }
        public bool pre_ticketed { get; set; }
        public string country_of_orgin { get; set; }
       
        public IList<LookUp> ListLblLayout { get; set; }
        public IList<LookUp> ListLblFormat { get; set; }
        public IList<lblCtnStyle> ListlblCtnStyle { get; set; }

        #endregion
    }

    public class UPSShipmentModel
    {

        public UPSShipmentModel() { }


        private string _cmp_id;
        private string _cmp_name;
        private string _so_num;
        private string _ordr_num;
        private string _PurchaseOrderNumberField;
        private string _InvoiceNumberField;
        private string _InvoiceDateField;
        private string _TermsOfShipmentField;
        private string _ReasonForExportField;
        private string _CommentsField;
        public string service_code { get; set; }
        private List<shipOdrerDtl> OrderDetailsField;
        private List<UPSServiceType> UPSServiceTypeListField;
        private UPSGsSecurity UPSSecurityField;
        private UPSShipper UPSShipperField;
        private UPSShipTo UPSShipToField;
        private UPSShipFrom UPSShipFromField;
        private UPSSoldTo UPSSoldToField;

        private UPSproductType shipProductTypeField;
        private UPSInternationalFormType InternationalFormTypeField;

        private UPSPackageType PackageTypeField;

        private string labelHeightField;
        private string labelWidthField;
        private string labelImageFormatField;
        private List<UPSServiceType> UPSServiceTypeField;

        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string cmp_name { get { return _cmp_name; } set { _cmp_name = value; } }
        public string so_num { get { return _so_num; } set { _so_num = value; } }
        public string ordr_num { get { return _ordr_num; } set { _ordr_num = value; } }
        

        public UPSGsSecurity UPSGsSecurity
        {
            get
            {
                return UPSSecurityField;
            }

            set
            {
                UPSSecurityField = value;
            }
        }
        public UPSShipper Shipper
        {
            get
            {
                return UPSShipperField;
            }

            set
            {
                UPSShipperField = value;
            }
        }

        public UPSShipTo UPSShipTo
        {
            get
            {
                return UPSShipToField;
            }

            set
            {
                UPSShipToField = value;
            }
        }

        public UPSShipFrom UPSShipFrom
        {
            get
            {
                return UPSShipFromField;
            }

            set
            {
                UPSShipFromField = value;
            }
        }

        public UPSSoldTo UPSSoldTo
        {
            get
            {
                return UPSSoldToField;
            }

            set
            {
                UPSSoldToField = value;
            }
        }

        public UPSproductType ShipProductType
        {
            get
            {
                return shipProductTypeField;
            }

            set
            {
                shipProductTypeField = value;
            }
        }

        public UPSInternationalFormType InternationalFormType
        {
            get
            {
                return InternationalFormTypeField;
            }

            set
            {
                InternationalFormTypeField = value;
            }
        }

        public UPSPackageType PackageType
        {
            get
            {
                return PackageTypeField;
            }

            set
            {
                PackageTypeField = value;
            }
        }

        public string LabelHeight
        {
            get
            {
                return labelHeightField;
            }

            set
            {
                labelHeightField = value;
            }
        }

        public string LabelWidth
        {
            get
            {
                return labelWidthField;
            }

            set
            {
                labelWidthField = value;
            }
        }

        public string LabelImageFormat
        {
            get
            {
                return labelImageFormatField;
            }

            set
            {
                labelImageFormatField = value;
            }
        }

        public List<shipOdrerDtl> OrderDetails
        {
            get
            {
                return OrderDetailsField;
            }

            set
            {
                OrderDetailsField = value;
            }
        }

        public string PurchaseOrderNumber
        {
            get
            {
                return _PurchaseOrderNumberField;
            }

            set
            {
                _PurchaseOrderNumberField = value;
            }
        }

        public string InvoiceNumber
        {
            get
            {
                return _InvoiceNumberField;
            }

            set
            {
                _InvoiceNumberField = value;
            }
        }

        public string InvoiceDate
        {
            get
            {
                return _InvoiceDateField;
            }

            set
            {
                _InvoiceDateField = value;
            }
        }

        public string TermsOfShipment
        {
            get
            {
                return _TermsOfShipmentField;
            }

            set
            {
                _TermsOfShipmentField = value;
            }
        }

        public string ReasonForExport
        {
            get
            {
                return _ReasonForExportField;
            }

            set
            {
                _ReasonForExportField = value;
            }
        }

        public string Comments
        {
            get
            {
                return _CommentsField;
            }

            set
            {
                _CommentsField = value;
            }
        }
        public List<UPSServiceType> UPSServiceTypeList
        {
            get
            {
                return UPSServiceTypeListField;
            }

            set
            {
                UPSServiceTypeListField = value;
            }
        }
    }


}
