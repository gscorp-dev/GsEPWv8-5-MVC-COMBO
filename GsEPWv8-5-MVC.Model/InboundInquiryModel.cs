using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//CR-3PL_MVC_IB_2018_0219_004 - Add a new column Bill and it should be visible once the Receiving is posted. By clicking the Bill link system should generate In&Out bill for the specific IB DOC ID and the status of the Bill column should be changed as Bill Posted 

namespace GsEPWv8_5_MVC.Model
{
    public class InboundGetInquiryDtlModel : BasicEntity
    {
        //Basic Entity Details
        public string is_company_user { get; set; }
        public string action_type { get; set; }
        public string ibs_status { get; set; }
        public string user_type { get; set; }
        public string cmp_id { get; set; }
        public string p_str_cmpid { get; set; }
        public string cmp_name { get; set; }
        public string cntr_id { get; set; }
        public string cntr_type { get; set; }
        public string status { get; set; }
        public string ib_doc_id { get; set; }
        public string doc_id { get; set; }
        public string grn_id { get; set; }
        public int kit_qty { get; set; }
        public string ib_doc_id_fm { get; set; }
        public string ib_doc_id_to { get; set; }
        public string seal_num { get; set; }
        public int balance { get; set; }
        public string freight_id { get; set; }
        public string req_num { get; set; }
        public string ib_doc_dt_fm { get; set; }
        public string ib_doc_dt_to { get; set; }

        public string ib_rcvd_dt_fm { get; set; }
        public string ib_rcvd_dt_to { get; set; }

        public string file_name { get; set; }
        public string ib_doc_dt { get; set; }
        public string eta_dt_fm { get; set; }
        public string eta_dt_to { get; set; }
        public string eta_dt { get; set; }
        public string orderdt { get; set; }
        public int inc_qty { get; set; }
        public int inc_ctn { get; set; }
        public int doc_entry_id { get; set; }
        public string palet_dt { get; set; }
        public string recvdvia { get; set; }
        public string recvd_fm { get; set; }
        public string bol { get; set; }
        public string vessel_num { get; set; }
        public decimal ord_qty { get; set; }
        public decimal height { get; set; }
        public decimal weight { get; set; }
        public decimal docppk { get; set; }
        public string lot_id { get; set; }
        public bool Isrma { get; set; }
        public string palet_id { get; set; }
        public string vend_name { get; set; }
        public string post_status { get; set; }
        public string po_num { get; set; }
        public string ordr_type { get; set; }
        public string note { get; set; }
        public string Rcvd_note { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string itm_name { get; set; }
        public string vend_id { get; set; }
        public string city { get; set; }
        public string state_id { get; set; }
        public string post_code { get; set; }
        public string addr_line1 { get; set; }
        public decimal wgt { get; set; }
        public int docuppk { get; set; }
        public decimal cube { get; set; }
        public int dtl_line { get; set; }
        public int itm_qty { get; set; }
        public int tot_qty { get; set; }
        public int tot_ctn { get; set; }
        public decimal length { get; set; }
        public decimal width { get; set; }
        public decimal depth { get; set; }
        public int ctn_line { get; set; }
        public string tel { get; set; }
        public string fax { get; set; }
        public int ctn_qty { get; set; }
        public string cont_id { get; set; }
        public string hdr_po_num { get; set; }
        public string whs_id { get; set; }
        public string Itmdtl { get; set; }
        public string pkg_id { get; set; }
        public string tran_type { get; set; }
        public int pkg_qty { get; set; }
        public string itm_code { get; set; }
        public string CompID { get; set; }
        public string Itm_Code { get; set; }
        public string Container { get; set; }
        public string InboundRcvdDt { get; set; }
        public string refno { get; set; }
        public string FOB { get; set; }
        public int LineNum { get; set; }
        public int CtnLine { get; set; }
        public string Status { get; set; }
        public string ecom_recv_by_bin { get; set; }
        public string cube_auto_calc { get; set; }
        //public string Style { get; set; }
        //public string Color { get; set; }
        //public string Size { get; set; }
        public int ppk { get; set; }
        public int ctn { get; set; }
        public int TotalQty { get; set; }
        public string Note { get; set; }
        public string ibdocid { get; set; }
        public string STATUS { get; set; }
        public decimal CUBE { get; set; }
        public string loc_id { get; set; }
        public string rptstatus { get; set; }
        public string strg_rate { get; set; }
        public string inout_rate { get; set; }
        public int rcvd_qty { get; set; }
        public decimal InbCube { get; set; }
        public decimal rptCUBE { get; set; }
        public decimal tallyCUBE { get; set; }
        public decimal workCUBE { get; set; }
        public string DateFrom { get; set; }
        public decimal doclength { get; set; }
        public decimal docwidth { get; set; }
        public decimal docheight { get; set; }
        public decimal docweight { get; set; }
        public decimal doccube { get; set; }
        public int avlqty { get; set; }
        public int avail_qty { get; set; }
        public int ordr_qty { get; set; }
        public int ctns { get; set; }
        public string Style { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public decimal Cube { get; set; }
        public int line_num { get; set; }
        public string ordr_dt { get; set; }
        public string shipvia_id { get; set; }
        public string master_bol { get; set; }
        public string vessel_no { get; set; }
        public int ordr_ctn { get; set; }
        public string itmid { get; set; }
        public decimal ctn_cube { get; set; }
        public string st_rate_id { get; set; }
        public string io_rate_id { get; set; }
        public decimal ctn_len { get; set; }
        public int itm_line { get; set; }
        public decimal ctn_width { get; set; }
        public decimal ctn_hgt { get; set; }
        public decimal ctn_wgt { get; set; }
        public string ct_value { get; set; }
        public int DocEntry { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string name1 { get; set; }
        public string description1 { get; set; }
        public string Recv_Itm_Mode { get; set; }
        public string rcvd_from { get; set; }

        public string Allow_New_item { get; set; }
        public string item_pick { get; set; }
        public string bill_type { get; set; }
        public string bill_inout_type { get; set; }
        public int CtnQty { get; set; }
        public int TotQty { get; set; }
        public string tran_status { get; set; }
        public string recv_itm_mode { get; set; }
        public string SucessStatus { get; set; }
        public string Comments { get; set; }
        public int rcvd_dtl_count { get; set; }
        public string rcvd_dt { get; set; }
        public string Filename { get; set; }
        public DateTime Uploaddt { get; set; }
        public string Uploadby { get; set; }
        public string doctype { get; set; }
        public string filepath { get; set; }
        public string file_path { get; set; }
        public int DocEntryCount { get; set; }
        public int DocCount { get; set; }

        public string GrdFlag { get; set; }
        public string docUploaddt { get; set; }
        public string View_Flag { get; set; }
        public string rpt_note { get; set; }

        public int line_count { get; set; }
        public string dtl_note { get; set; }
        public string hdr_note { get; set; }
        public string pkg_type { get; set; }
        public string rate_id { get; set; }
        public string notes { get; set; }
        public string rcvd_via { get; set; }
        public string dft_whs { get; set; }
        public string kit_itm { get; set; }
        public decimal list_price { get; set; }
        public string process_id { get; set; }
        //Added By Ravi 17-02-2018
        public string CONTAINERID { get; set; }
        public string CNTR_PALLET { get; set; }
        public string CNTR_WEIGHT { get; set; }
        public string CNTR_CUBE { get; set; }
        public string CNTR_NOTE { get; set; }
        public string RATEID { get; set; }
        public string CNTR_CHECK { get; set; }
        public string RESULT_STATUS { get; set; }
        public string PROCESS_ID { get; set; }
        public int no_of_pallets { get; set; }
        public decimal tot_wgt { get; set; }
        public decimal tot_cube { get; set; }
        public string DocumentdateFrom { get; set; }
        public string DocumentdateTo { get; set; }
        public string CntrNote { get; set; }
        public string check_inout_type { get; set; }
        public string inout_bill_status { get; set; } //CR-3PL_MVC_IB_2018_0219_004

        //END
        //CR_3PL_MVC_BL_2018_0226_001 ADDED BY RAVI 
        public string UPLOAD_FILE { get; set; }
        public string bill_doc_id { get; set; }
        public int recv_dtl_count { get; set; }  // CR-3PL_MVC_IB_2018_0312_002

        public string rpt_itemcode { get; set; }
        public string rpt_itm_num { get; set; }
        public string rpt_itm_color { get; set; }
        public string rpt_itm_size { get; set; }
        public decimal rpt_wgt { get; set; }
        public decimal rpt_length { get; set; }
        public decimal rpt_width { get; set; }
        public decimal rpt_depth { get; set; }
        public string Check_exist_itm_count { get; set; }
        public string screentitle { get; set; }//CR_MVC_3PL_0317-001 Added By Nithya
        public string l_bool_edit_flag { get; set; }//CR_MVC_3PL_0317-001 Added By Nithya
        public int recving_qty { get; set; } //CR_3PL_MVC_IB_2018_0317_001 
        public int tmp_recving_qty { get; set; }
        public string tmp_cmp_id { get; set; }
        public string IS3RDUSER { get; set; }
        public string Image_Path { get; set; }

        public string rpt_req_no { get; set; }

        public string flag { get; set; }
        public string itm_type { get; set; }


        public string ib_Itmdtl { get; set; }
        public string ib_itm_size { get; set; }
        public string ib_itm_color { get; set; }
        public string ib_itm_name { get; set; }
        public int recvcount { get; set; }
        public int tot_pkg { get; set; }
        public string scn_id { get; set; }
        public string lock_by { get; set; }
        public DateTime lock_dt { get; set; }
        public string user_id { get; set; }

        public int editmode { get; set; }
        public string opt { get; set; }
        public int int_ib_doc_in_use { get; set; }
        public string l_bool_flag { get; set; }
        public string Recv_non_doc_itm { get; set; }
        //End
        public int TOT_CARTON { get; set; }
        public decimal TOT_WEIGHT { get; set; }
        public decimal TOTCUBE { get; set; }
        public string aloc_by { get; set; }
        public decimal srvc_price { get; set; }
        public string srvc_uom { get; set; }
        public int srvc_qty { get; set; }
        public decimal amt { get; set; }
        public string ibs_user_id { get; set; }
        public string srvc_id { get; set; }
        public string srvc_name { get; set; }
        public string price_uom { get; set; }
        public string ibs_doc_id { get; set; }
        public int upload_file_count { get; set; }
        
        public DateTime doc_dt_for_order_by { get; set; }
    
        public Int64 total_ctns { get; set; }
        public decimal total_cube { get; set; }
        public string rcvd_cube_in_range { get; set; }
        public Int64 dup_itm_count { get; set; }
        public Int64 less_cube_count { get; set; }
        public string cube_excp { get; set; }
        public string dup_itm_excp { get; set; }
        public string ib_load_dt { get; set; }
        public string factory_id { get; set; }
        public string vend_po_num { get; set; }
        public string cust_name { get; set; }
        public string cust_po_num { get; set; }
        public string pick_list { get; set; }

    }
    public class ib_ibs_dtl
    {
        public string cmp_id { get; set; }
        public string ib_doc_id { get; set; }
        public int dtl_line { get; set; }
        public string status { get; set; }
        public string srvc_id { get; set; }
        public string srvc_name { get; set; }
        public decimal srvc_price { get; set; }
        public string srvc_uom { get; set; }
        public int srvc_qty { get; set; }
        public string notes { get; set; }
        public decimal srvc_line_amount { get; set; }
        public string ibs_doc_id { get; set; }


    }
    public class InboundInquiryModel : InboundGetInquiryDtlModel
    {
        //List Fetch Details
        public IList<InboundInquiry> ListInboundDocInquiry { get; set; }
        public IList<InboundInquiry> ListItmStyledtl { get; set; }
        public IList<InboundInquiry> ListIbDocId { get; set; }
        public IList<InboundInquiry> ListDocuEntrydtl { get; set; }
        public IList<InboundInquiry> LstStrgIddtl { get; set; }
        public IList<InboundInquiry> ListRcvgUnPost { get; set; }
        public IList<InboundInquiry> ListRMADocId { get; set; }
        public IList<InboundInquiry> ListLotItem { get; set; }
        public IList<InboundInquiry> ListGetItmdtl { get; set; }
        public IList<InboundInquiry> LstInoutIddtl { get; set; }
        public IList<InboundInquiry> ListStyle { get; set; }
        public IList<InboundInquiry> ListItmId { get; set; }
        public IList<InboundInquiry> ListGetItmhdr { get; set; }
        public IList<InboundInquiry> ListDocTallySheetRpt { get; set; }
        public IList<InboundInquiry> ListGridEditData { get; set; }
        public IList<InboundInquiry> ListRcvgPost { get; set; }
        public IList<InboundInquiry> ListAvailDtl { get; set; }
        public IList<InboundInquiry> lstobjInboundInq { get; set; }
        public IList<InboundInquiry> ListDocHdr { get; set; }
        public IList<InboundInquiry> ListDocDtl { get; set; }
        public IList<LookUp> ListLookUpDtl { get; set; }
        public IList<Company> ListCompanyDtl { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<Company> ListLocPickDtl { get; set; }
        public IList<InboundInquiry> ListCanPost { get; set; }
        public IList<InboundInquiry> LstItmxCustdtl { get; set; }
        public IList<InboundInquiry> ListCustConfigDetails { get; set; }
        public IList<InboundInquiry> ListCustConfigRcvdItmModeDetails { get; set; }
        public IList<InboundInquiry> ListDocdtl { get; set; }
        public IList<InboundInquiry> ListRcvgHdr { get; set; }
        public IList<InboundInquiry> ListDimension { get; set; }
        public IList<Company> ListwhsPickDtl { get; set; }
        public IList<InboundInquiry> ListRcvgDtl { get; set; }
        public IList<InboundInquiry> ListAckRptDetails { get; set; }
        public IList<InboundInquiry> ListWorkSheetRptDetails { get; set; }
        public IList<InboundInquiry> ListDocId { get; set; }
        public IList<InboundInquiry> ListGridSummaryRptDetails { get; set; }
        public IList<InboundInquiry> ListDocEntryDtl { get; set; }
        public IList<InboundInquiry> ListTallySheetRptDetails { get; set; }
        public IList<InboundInquiry> ListConfirmationRptDetails { get; set; }
        public IList<InboundInquiry> ListInboundStatusRptDetails { get; set; }
        public IList<InboundInquiry> ListStrgBillType { get; set; }
        public IList<InboundInquiry> ListPaletId { get; set; }
        public IList<InboundInquiry> ListLotId { get; set; }
        public IList<InboundInquiry> ListInsertTblIbDocDtlTmp { get; set; }
        public IList<Company> ListGetCustConfigDtls { get; set; }
        public IList<InboundInquiry> ListGetCustConfigRcvdItemMode { get; set; }
        public IList<InboundInquiry> ListRecvdDetails { get; set; }
        public IList<InboundInquiry> LstFiledtl { get; set; }
        public IList<InboundInquiry> ListSaveDocRecvdDetails { get; set; }
        public IList<InboundInquiry> ListGetRecvdDetailsCount { get; set; }
        public IList<InboundInquiry> ListGetDocEntryCount { get; set; }
        public IList<InboundInquiry> ListLoadReceivingDelDtl { get; set; }
        public IList<InboundInquiry> ListLoadDocEditDtl { get; set; }
        public IList<InboundInquiry> ListPickdtl { get; set; }
        public IList<InboundInquiry> ListDocRecvEntryDtl { get; set; }
        //Added By Ravi 17-02-2018
        public IList<InboundInquiry> ListGETContainerID { get; set; }
        public IList<InboundInquiry> ListGETRateID { get; set; }
        public IList<InboundInquiry> GETRESULTVALUE { get; set; }
        public IList<InboundInquiry> ListGetWgtCubeValue { get; set; }
        //CR_3PL_MVC_BL_2018_0224_001 Added By Ravi 24-02-2018
        public IList<InboundInquiry> ListGetPONUM { get; set; }
        public IList<InboundInquiry> ListGetSTRG_ID { get; set; }
        //END
        public IList<Company> LstCmpName { get; set; }//CR - 3PL_MVC_IB_2018_0219_008

        //END  
        //  CR-3PL_MVC_IB_2018_0312_002
        public IList<InboundInquiry> GetRcvdDtlCount { get; set; }
        //END CR-3PL_MVC_IB_2018_0312_002 
        public IList<InboundInquiry> ListItemRcvdQty { get; set; }
        public IList<InboundInquiry> ListCtnLine { get; set; }
        public IList<InboundInquiry> ListCheckExistContainerId { get; set; }
        public IList<InboundInquiry> LstItmExist { get; set; }
        public IList<InboundInquiry> LstRcvdEntryCountDtl { get; set; }
        public IList<InboundInquiry> LstMail { get; set; }
        public IList<InboundInquiry> ListTotalCount { get; set; }
        public IList<InboundInquiry> List_ID_DOC_IN_USE { get; set; }
        public IList<InboundInquiry> List_ibs_dtl { get; set; }
        public IList<InboundInquiry> List_get_ibs_dtl { get; set; }
        public IList<InboundInquiry> List_get_Ibs_Doc_ID { get; set; }
        public IList<LookUp> ListContainerType { get; set; }




    }
}
