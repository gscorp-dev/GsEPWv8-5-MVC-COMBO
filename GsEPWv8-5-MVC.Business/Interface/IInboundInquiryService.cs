using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface IInboundInquiryService
    {
        string fnCheckCheckBinRefered(string pstrCmpId, string pstrBinId, string pstrItmNum, string pstrItmColor, string pstrItmSize, string pstrItmCode);
        InboundInquiry GetInboundInquiryDetails(InboundInquiry objInboundInquiry);
        InboundInquiry GetIbDocIdDetail(InboundInquiry objInboundInquiry);
        InboundInquiry GetInboundAckRptDetails(InboundInquiry objInboundInquiry);
        InboundInquiry GetInboundWorkSheetRptDetails(InboundInquiry objInboundInquiry);
        void InsertTempDocEntryDetails(InboundInquiry objInboundInquiry);
        void Add_To_proc_save_doc_hdr(InboundInquiry objInboundInquiry);
        void Add_To_proc_save_doc_dtl(InboundInquiry objInboundInquiry);
        void Add_To_proc_save_doc_ctn(InboundInquiry objInboundInquiry);
        void DeleteDocEntry(InboundInquiry objInboundInquiry);
        void Add_Style_To_Itm_dtl(InboundInquiry objInboundInquiry);
        void Add_Style_To_Itm_hdr(InboundInquiry objInboundInquiry);
        void Update_doc_hdr(InboundInquiry objInboundInquiry);
        void Del_doc_Dtl(InboundInquiry objInboundInquiry);
        void TruncateTempDocUpload(InboundInquiry objInboundInquiry);
        void ReceivingPostDtls(InboundInquiry objInboundInquiry);
        void DocReceivingUnPost(InboundInquiry objInboundInquiry);
        void ReceivingPost9999Dtls(InboundInquiry objInboundInquiry);
        InboundInquiry GetRcvngUnPostDtls(InboundInquiry objInboundInquiry);
        InboundInquiry DocTallySheetRpt(InboundInquiry objInboundInquiry);
        InboundInquiry LoadLotItem(InboundInquiry objInboundInquiry);
        InboundInquiry CanPost(InboundInquiry objInboundInquiry);
        InboundInquiry GetInboundConfirmationRptDetailsbyContainer(InboundInquiry objInboundInquiry);
        InboundInquiry GetRcvngGridDtl(InboundInquiry objInboundInquiry);
        InboundInquiry GetRcvngPostDtls(InboundInquiry objInboundInquiry);
        InboundInquiry LoadAvailDtl(InboundInquiry objInboundInquiry);
        InboundInquiry GetCSVList(InboundInquiry objInboundInquiry);
        InboundInquiry GetRcvngHdr(InboundInquiry objInboundInquiry);
        InboundInquiry GetCheckExistGridData(InboundInquiry objInboundInquiry);
        InboundInquiry GetGridDeleteData(InboundInquiry objInboundInquiry);
        InboundInquiry CheckItmHdr(InboundInquiry objInboundInquiry);
        InboundInquiry GetItemHdr(InboundInquiry objInboundInquiry);
        InboundInquiry GetItmId(InboundInquiry objInboundInquiry);
        InboundInquiry GetPickStyleDetails(InboundInquiry objInboundInquiry);
        InboundInquiry GetDocEntryId(InboundInquiry objInboundInquiry);
        InboundInquiry GetDocumentEntryTempGridDtl(InboundInquiry objInboundInquiry);
        InboundInquiry ItemXGetitmDetails(string term, string cmp_id);
        InboundInquiry GetDocHdr(InboundInquiry objInboundInquiry);
        InboundInquiry IsRMAChecked(InboundInquiry objInboundInquiry);
        InboundInquiry GetDocDtl(InboundInquiry objInboundInquiry);
        InboundInquiry GetGridEditData(InboundInquiry objInboundInquiry);
        InboundInquiry GetDocuEntryTempGridDtl(InboundInquiry objInboundInquiry);
        InboundInquiry GetInboundTallySheetRptDetails(InboundInquiry objInboundInquiry);
        InboundInquiry GetInboundConfirmationRptDetails(InboundInquiry objInboundInquiry);
        InboundInquiry GetInboundGridSummaryDetails(InboundInquiry objInboundInquiry);
        InboundInquiry GetInboundStatus(InboundInquiry objInboundInquiry);
        InboundInquiry GetInboundHdrDtl(InboundInquiry objInboundInquiry);
        InboundInquiry GetInboundDtl(InboundInquiry objInboundInquiry);
        InboundInquiry GetInboundLotDtl(InboundInquiry objInboundInquiry);
        InboundInquiry CheckItmDimension(InboundInquiry objInboundInquiry);
        InboundInquiry LoadStrgId(InboundInquiry objInboundInquiry);
        void TruncateTempDocEntry(InboundInquiry objInboundInquiry);
        InboundInquiry LoadInoutId(InboundInquiry objInboundInquiry);
        InboundInquiry LoadCustConfig(InboundInquiry objInboundInquiry);
        InboundInquiry LoadCustConfigRcvdItmMode(InboundInquiry objInboundInquiry);
        InboundInquiry Getitmlist(InboundInquiry objInboundInquiry);
        InboundACKExcel GetInboundAckExcel(InboundACKExcel objInboundInquiry);
        InboundWorkSheetExcel GetInboundWorkSheetExcel(InboundWorkSheetExcel objInboundInquiry);
        InboundTallySheetExcel GetInboundTallySheetExcel(InboundTallySheetExcel objInboundInquiry);

        InboundConfirmationExcel GetInboundConfimExcel(InboundConfirmationExcel objInboundInquiry);

        InboundInquiry GetInboundLotHdr(InboundInquiry objInboundInquiry);
        InboundInquiry GEtStrgBillTYpe(InboundInquiry objInboundInquiry);
        InboundInquiry GetPalletId(InboundInquiry objInboundInquiry);
        InboundInquiry GetLotId(InboundInquiry objInboundInquiry);
        InboundInquiry GetInsertTblIbDocDtlTmp(InboundInquiry objInboundInquiry);
        InboundInquiry GetCustRcvdItemMode(InboundInquiry objInboundInquiry);
        InboundInquiry GetInsertTblIbDocRecvDtlTemp(InboundInquiry objInboundInquiry);
        void InsertTempFileDocument(InboundInquiry objInboundInquiry);
        InboundInquiry GetTempFiledtl(InboundInquiry objInboundInquiry);
        InboundInquiry GetDocRecvEntrySave(InboundInquiry objInboundInquiry);
        InboundInquiry GetRecvTempTableCount(InboundInquiry objInboundInquiry);
        InboundInquiry GetDocEntryCount(InboundInquiry objInboundInquiry);
        InboundInquiry Getuploaddelete(InboundInquiry objInboundInquiry);
        InboundInquiry GetReceivingdelete(InboundInquiry objInboundInquiry);
        InboundInquiry GetLoadReceivingdelete(InboundInquiry objInboundInquiry);
        InboundInquiry GetUpdateRcvdStatus(InboundInquiry objInboundInquiry);
        InboundInquiry GetDocRecvEntrySaveByItem(InboundInquiry objInboundInquiry);
        InboundInquiry GetDocRecvEntrySaveByLotID(InboundInquiry objInboundInquiry);
        InboundInquiry GetDocRecvEntrySaveByPo(InboundInquiry objInboundInquiry);
        InboundInquiry GetDocEditDtl(InboundInquiry objInboundInquiry);
        InboundInquiry GetEditGridData(InboundInquiry objInboundInquiry);
        InboundInquiry GetDeleteTempData(InboundInquiry objInboundInquiry);
        InboundInquiry UpdateTblIbDocHdr(InboundInquiry objInboundInquiry);
        InboundInquiry GetDeleteDocCtn(InboundInquiry objInboundInquiry);
        InboundInquiry UpdateTblIbDocDtl(InboundInquiry objInboundInquiry);
        InboundInquiry GetPaletIdValidation(InboundInquiry objInboundInquiry);
        InboundInquiry Gettranstaus(InboundInquiry objInboundInquiry);
        InboundInquiry Getlotdtltext(InboundInquiry objInboundInquiry);
        InboundInquiry GetLotHdr(InboundInquiry objInboundInquiry);
        InboundInquiry GetGridlotdtl(InboundInquiry objInboundInquiry);
        InboundInquiry GetDftWhs(InboundInquiry objInboundInquiry);
        InboundInquiry Del_Doc_qty_Mod(InboundInquiry objInboundInquiry);
        InboundInquiry Del_iv_itm_trn(InboundInquiry objInboundInquiry);
        InboundInquiry Update_Doc_tbl(InboundInquiry objInboundInquiry);
        InboundInquiry Update_Doc_ctn(InboundInquiry objInboundInquiry);
        InboundInquiry Add_To_Itm_Trn_in_CtnQty(InboundInquiry objInboundInquiry);
        InboundInquiry Add_Iv_Lot_Dtl(InboundInquiry objInboundInquiry);
        InboundInquiry Add_tbl_iv_lot_ctn(InboundInquiry objInboundInquiry);
        InboundInquiry GetKitQty(InboundInquiry objInboundInquiry);
        InboundInquiry CkIORate(InboundInquiry objInboundInquiry);
        InboundInquiry CkSTRate(InboundInquiry objInboundInquiry);
        InboundInquiry GetGridRecvEditData(InboundInquiry objInboundInquiry);
        InboundInquiry InsertTblIbDocRecvDtlTemp(InboundInquiry objInboundInquiry);
        InboundInquiry GetItmName(InboundInquiry objInboundInquiry);
        InboundInquiry GetItmCode(InboundInquiry objInboundInquiry);
        InboundInquiry UpdtItmDtl(InboundInquiry objInboundInquiry);
        InboundInquiry GetCheckExistGridDataRecvEntry(InboundInquiry objInboundInquiry);
        InboundInquiry InsertRecvEntryTemptable(InboundInquiry objInboundInquiry);
        InboundInquiry GetRecvEntryCount(InboundInquiry objInboundInquiry);
        InboundInquiry GetRecvdtlGrid(InboundInquiry objInboundInquiry);
        InboundInquiry GetRecvEntryGridDeleteData(InboundInquiry objInboundInquiry);
        InboundInquiry SumTotqty(InboundInquiry objInboundInquiry);
        InboundInquiry GetLotIDValidation(InboundInquiry objInboundInquiry);
        InboundInquiry GetCntrValidation(InboundInquiry objInboundInquiry);

        //Added By Ravi 17-02-2018
        InboundInquiry GetContainerandRateID(InboundInquiry objInboundInquiry);
        InboundInquiry InsertCONTAINERRecvDetails(InboundInquiry objInboundInquiry);
        //END  
        //CR_3PL_MVC_BL_2018_0221_001 Added By Ravi 21-02-2018
        void Del_rcv_dtl(InboundInquiry objInboundInquiry);
        //END
        //CR_3PL_MVC_IB_2018_0228_001 Added By Ravi 21-02-2018
        InboundInquiry AddLocId(InboundInquiry objInboundInquiry);
        //END
        //  CR-3PL_MVC_IB_2018_0312_002
        InboundInquiry GetRcvdDtlCount(InboundInquiry objInboundInquiry);
        //END CR-3PL_MVC_IB_2018_0312_002 

        InboundInquiry GetIBRecvDeleteTempData(InboundInquiry objInboundInquiry);       //  CR-3PL_MVC_IB_2018_0313_002
        InboundInquiry GetItemRcvdQty(InboundInquiry objInboundInquiry);
        InboundInquiry GetCtnLineNo(InboundInquiry objInboundInquiry);
        InboundInquiry Update_Lot_Bill_Status(InboundInquiry objInboundInquiry);
        InboundInquiry Check_Exist_Container_Id(InboundInquiry objInboundInquiry);
        InboundInquiry CheckItemExist(InboundInquiry objInboundInquiry);
        InboundInquiry Validate_Itm(InboundInquiry objInboundInquiry);  //  CR-3PL_MVC_IB_2018_0410_001 Added By Nithya
        InboundInquiry ItemXGetIBitmDetails(string term, string cmp_id);
        InboundInquiry GetRcvdEntryCountDtl(InboundInquiry objInboundInquiry);  //  CR-3PL_MVC_IB_2018_0410_001 Added By Nithya
        InboundInquiry InsertdocEditEntry(InboundInquiry objInboundInquiry);
        InboundInquiry GetdocEditCount(InboundInquiry objInboundInquiry);
        InboundInquiry GetrecvEditTotQty(InboundInquiry objInboundInquiry);
        InboundInquiry Add_To_proc_save_audit_trail(InboundInquiry objInboundInquiry);//  CR-3PL_MVC_IB_2018_0813_001 Added By Nithya
        InboundInquiry CHECKDOCDATE(InboundInquiry objInboundInquiry);
        InboundInquiry GET_IB_DOC_CUBE_AND_WGT(InboundInquiry objInboundInquiry);
        InboundInquiry GET_IB_RCVD_DOC_CUBE_AND_WGT(InboundInquiry objInboundInquiry);
        InboundInquiry GET_IBS_DTL_FROM_RATE_MASTER(InboundInquiry objInboundInquiry);
        void INSERT_IBS_DTL_TEMP_TBL(InboundInquiry objInboundInquiry);
        InboundInquiry GET_IBS_DTL_TEMP_TBL(InboundInquiry objInboundInquiry);
        void DELETE_IBS_DTL_TEMP_TBL(InboundInquiry objInboundInquiry);
        void INSERT_IBS_DTL(InboundInquiry objInboundInquiry);
        void UPDATE_IBS_DTL(InboundInquiry objInboundInquiry);
        void DELETE_IBS_DTL(InboundInquiry objInboundInquiry);
        InboundInquiry GET_IBS_DTL(InboundInquiry objInboundInquiry);
        InboundInquiry GET_IBS_DOC_ID(InboundInquiry objInboundInquiry);
        IBDocExcp GetIBDocExcpList(IBDocExcp objIBDocExcp, string p_str_cmp_id);
        DataTable GetIBDocExceptionList(string p_str_cmp_id);
        DataTable getXlsIBStkSmryByLocList(string p_str_cmp_id, string p_str_ib_doc_id);
        DataTable GetInboundExceptionList(String l_str_cmp_id);
        bool DeleteIOBillByIbDocId(string p_str_cmp_id, string p_str_bill_doc_id, string p_str_lot_id);
        DataTable GetIBCheckIbDocValidateItem(string p_str_cmp_id, string p_str_ib_doc_id);
        DataTable GetIBCheckIbDocRcvdCubeCheck(string p_str_cmp_id, string p_str_ib_doc_id);
        bool UpdateLotContainer(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_cntr_id, string p_str_cntr_type, string p_str_rcvd_dt,string p_str_ib_load_dt);
        DataTable GetInboundAckExcelTemplate(string l_str_cmp_id, string l_str_doc_id);
        DataTable fnIBGetInboundAckRpt(string pstrCmpdId, string pstrFileName, string pstrUploadRefNum);
        DataTable GetInboundGridSummaryExcelTemplate(string l_str_cmp_id, string l_str_doc_id, string l_str_cntr_id, string l_str_status, string l_str_ib_doc_dt_fm, string l_str_ib_doc_dt_to, string l_str_req_num, string l_str_eta_dt_fm, string l_str_eta_dt_to, string l_str_itm_num, string l_str_itm_color, string l_str_itm_size);
        DataTable GetInboundWorkSheetExcelTemplate(string l_str_cmp_id, string l_str_ib_doc_id);
        DataTable GetInboundContainerExcelTemplate(string l_str_cmp_id, string l_str_ib_doc_id, string l_str_rate_id);
        bool AddAutoSpecialIBSEntry(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_cntr_id);
        bool AddAutoIBSEntry(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_cntr_id);
        string checkIBDocInUse(string p_str_cmp_id, string p_str_ib_doc_id);
        bool checkIBDocRcvd(string pstrCmpId, string pstrIbDocId);
        string fnIBRecvDtlEditTempSave(string pstrCmpId, DataTable dtIBRrecvDtlEditTemp);

        void InsertScanInDetails(InboundInquiry objInboundInquiry);
        List<ItemScanIN> getScanInDetailsByItemCode(string cmpId, string itm_code, string itm_serial_num);

    }
}
