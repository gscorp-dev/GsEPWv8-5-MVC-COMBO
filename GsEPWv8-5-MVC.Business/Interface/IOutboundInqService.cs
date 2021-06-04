﻿using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface IOutboundInqService
    {
        OutboundInq GetEcomitmlist(OutboundInq objOutboundInq);
        OutboundInq LoadCustConfig(OutboundInq objOutboundInq);
        OBAlocPostInquiry GetEcomOBAlocOpenList(string pstrCmpId, string pstrBatchId);
        OutboundInq fnSaveSoTracking(string pstrMode, string pstrCmpId, string pstrSoNum, string pstrTrackNumType, string pstrTrackNum, string pstrTrackStatus, string pstrTrackDate, string pstrProcessId);
         OutboundInq fnGetSoTracking(string pstrCmpId, string pstrSoNum, string pstrTrackNum);
        OutboundInq GetOutboundInq(OutboundInq objOutboundInq);
        OutboundInq GetEcomOrderInq(OutboundInq objOutboundInq);
        bool fnEcomSaveBatchPrint(clsEComPrintOrders objEComPrintOrders);
        OutboundInq fnEcomBatchPrint(string pstrCmpId, string pstrBatchId);
        OutboundInq InsertTempFileDocument(OutboundInq objOutboundInq);
        OutboundInq GetStyleDetails(OutboundInq objOutboundInq);
        OutboundInq GetTempFiledtl(OutboundInq objOutboundInq);
        OutboundInq OutboundInqShipAckCtnvalues(OutboundInq objOutboundInq);
        OutboundInq Get_AlocBackOrderCount(OutboundInq objOutboundInq);
        OutboundInq OutboundInqSUmmaryRpt(OutboundInq objOutboundInq);
        OutboundInq OutboundInqPickStyleRpt(OutboundInq objOutboundInq);
        OutboundInq OutboundInqPickLocationRpt(OutboundInq objOutboundInq);
        OutboundInq OutboundInqGridSummary(OutboundInq objOutboundInq);
        OutboundInq OutboundInqShipAck(OutboundInq objOutboundInq);
        OutboundInq GetUnPickQty(OutboundInq objOutboundInq);
        OutboundInq GetShipReqEditDtl(OutboundInq objOutboundInq);
        void UpdateTrnHdr(OutboundInq objOutboundInq);
        void InsertShipDtl(OutboundInq objOutboundInq);
        void SaveAlocUnPost(OutboundInq objOutboundInq);
        OutboundInq CheckAllocpost(OutboundInq objOutboundInq);
        OutboundInq OutboundShipInqhdr(OutboundInq objOutboundInq);
        OutboundInq OutboundShipInqdtl(OutboundInq objOutboundInq);
        OutboundInq GetIbSRIdDetail(OutboundInq objOutboundInq);
        OutboundInq GetOutSaleId(OutboundInq objOutboundInq);
        OutboundInq GetAlocUnPostGridLoadItem(OutboundInq objOutboundInq);
        OutboundInq GetCustId(OutboundInq objOutboundInq);
        OutboundInq GetShipToAddress(OutboundInq objOutboundInq);
        void InsertShipHdr(OutboundInq objOutboundInq);
        OutboundInq ItemXGetitmDetails(string term, string cmp_id);
        void InsertTemptableValue(OutboundInq objOutboundInq);
        void DeleteTempshipEntry(OutboundInq objOutboundInq);
        OutboundInq GetGridList(OutboundInq objOutboundInq);
        OutboundInq GetCheckExistStyle(OutboundInq objOutboundInq);
        OutboundInq GetAlocGridLoadItem(OutboundInq objOutboundInq);
        OutboundInq GetPickGridLoadItem(OutboundInq objOutboundInq);
        OutboundInq GetAlocType(OutboundInq objOutboundInq);
        void GetAlocPostDirect(OutboundInq objOutboundInq);
        void GetAlocPost(OutboundInq objOutboundInq);
        void UpdateStatusInAlocHdr(OutboundInq objOutboundInq);
        void UpdateStatusInAlocDtl(OutboundInq objOutboundInq);
        OutboundInq GetShipNum(OutboundInq objOutboundInq);
        OutboundInq GetSRIdDtl(OutboundInq objOutboundInq);
        OutboundInq LoadAvailQty(OutboundInq objOutboundInq);
        OutboundInq GetPickStyleDetails(OutboundInq objOutboundInq);
        OutboundInq GetShipReqEntryTempGridDtl(OutboundInq objOutboundInq);
        void Add_To_proc_save_so_dtl_excel(OutboundInq objOutboundInq);
        void Add_To_proc_save_so_dtl_due_excel(OutboundInq objOutboundInq);
        void Add_To_proc_save_so_hdr_excel(OutboundInq objOutboundInq);
        void Add_To_proc_save_so_addr_excel(OutboundInq objOutboundInq);
        OutboundInq Getkitflag(OutboundInq objOutboundInq);
        OutboundInq GetCustDtl(OutboundInq objOutboundInq);
        OutboundInq Getitmlist(OutboundInq objOutboundInq);
        OutboundInq GetCSVList(OutboundInq objOutboundInq);
        void OutboundInqTempDelete(OutboundInq objOutboundInq);
        OutboundInq GetViewDetail(OutboundInq objOutboundInq);
        OutboundInq GetViewDetailgrid(OutboundInq objOutboundInq);
        OutboundInq GetViewAddrDetail(OutboundInq objOutboundInq);
        void DeleteShipEntry(OutboundInq objOutboundInq);
        void DeleteTempshipEntrytable(OutboundInq objOutboundInq);
        OutboundInq GetGridEditData(OutboundInq objOutboundInq);
        OutboundInq GetGridDeleteData(OutboundInq objOutboundInq);
        OutboundInq GetShipToAddressSave(OutboundInq objOutboundInq);
        OutboundInq GetItmNameDetails(OutboundInq objOutboundInq);
        OutboundInq OutboundShipAloc(OutboundInq objOutboundInq);
        OutboundInq SoNumFrom_Validation(OutboundInq objOutboundInq);
        OutboundInq OutboundSoldtoId(OutboundInq objOutboundInq);
        OutboundInq GetObalocIdDetail(OutboundInq objOutboundInq);
        OutboundInq OutboundSelectionhdr(OutboundInq objOutboundInq);    
        void InsertTempAUTOALOC(OutboundInq objOutboundInq);
        OutboundInq OutboundGETITMNAME(OutboundInq objOutboundInq);
        OutboundInq OutboundGETALOCSORTSTMT(OutboundInq objOutboundInq);
        OutboundInq OutboundGETAVILQTY(OutboundInq objOutboundInq);
        OutboundInq OutboundGETTEMPLIST(OutboundInq objOutboundInq);
        void DeleteAUTOALOC(OutboundInq objOutboundInq);
        void InsertTempAlocdtl(OutboundInq objOutboundInq);
        void InsertTempAlocSummary(OutboundInq objOutboundInq);
        OutboundInq OutboundGETTEMPALOCSUMMARY(OutboundInq objOutboundInq);
        OutboundInq OutboundGETALOCDTL(OutboundInq objOutboundInq);
        OutboundInq OutboundGETTEMPALOCDTL(OutboundInq objOutboundInq);
        void Update_aloc_num(OutboundInq objOutboundInq);
        OutboundInq Move_to_aloc_hdr(OutboundInq objOutboundInq);
        OutboundInq Move_to_aloc_dtl(OutboundInq objOutboundInq);
        OutboundInq Move_to_aloc_ctn(OutboundInq objOutboundInq);
        void Moveto_TrnHdr(OutboundInq objOutboundInq);
        OutboundInq Update_so_dtl(OutboundInq objOutboundInq);
        void Move_To_Grd_Bad_Itm(OutboundInq objOutboundInq);
        void Update_Tbl_iv_itm_trn_in(OutboundInq objOutboundInq);
        void Change_SOHdr_Status_atAdd(OutboundInq objOutboundInq);
        OutboundInq Get_Newqty(OutboundInq objOutboundInq);
        OutboundInq update_alocctn(OutboundInq objOutboundInq);
        OutboundInq GetSRGridRowCount(OutboundInq objOutboundInq);
        OutboundInq Get_PkgID(OutboundInq objOutboundInq);
        OutboundInq Get_itmtrninList(OutboundInq objOutboundInq);
        void Aloc_SpltCtn_Upd_Itm_Trn_in_direc(OutboundInq objOutboundInq);
        void Aloc_Upd_data_to_itm_trn_in_direc(OutboundInq objOutboundInq);
        void Aloc_into_Itm_Trn_hst_by_itm_del_direc(OutboundInq objOutboundInq);
        void Aloc_into_Itm_Trn_hst_by_itm_direc(OutboundInq objOutboundInq);
        OutboundInq Get_AlocSaveRpt(OutboundInq objOutboundInq);
        OutboundInq GetDftWhs(OutboundInq objOutboundInq);
        OutboundInq GetalochdrList(OutboundInq objOutboundInq);
        OutboundInq GetalocdtlList(OutboundInq objOutboundInq);
        OutboundInq GetalocctnList(OutboundInq objOutboundInq);
        OutboundInq GetalocdueList(OutboundInq objOutboundInq);
        OutboundInq Del_Alloc_Upd_SO(OutboundInq objOutboundInq);
        OutboundInq Del_data_to_itm_trn_in(OutboundInq objOutboundInq);
        OutboundInq Add_To_Trn_Hdr(OutboundInq objOutboundInq);
        OutboundInq BackOrderRpt(OutboundInq objOutboundInq);
        OutboundInq Get_AlocBackOrderRptList(OutboundInq objOutboundInq);
        OutboundInq GetaloceditmanualList(OutboundInq objOutboundInq);
        OutboundInq GetSelectedgridValue(OutboundInq objOutboundInq);
        OutboundInq GetALOCEditList(OutboundInq objOutboundInq);
        OutboundInq SP_Aloc_del_Itm_trn_hst(OutboundInq objOutboundInq);
        OutboundInq Sp_Aloc_Mod_Daloc_iv_itm_trn_in(OutboundInq objOutboundInq);
        OutboundInq GetAddaloceditmanualList(OutboundInq objOutboundInq);
        OutboundInq GetSelectedgridValueList(OutboundInq objOutboundInq);
        OutboundInq UpdateTemp_Alloc_Summary(OutboundInq objOutboundInq);
        OutboundInq Update_Tbl_Temp_So_Auto_Aloc(OutboundInq objOutboundInq);
        OutboundInq DeleteTemp_Alloc_Summary(OutboundInq objOutboundInq);
        void DelSo_dtl_Due_Table(OutboundInq objOutboundInq);//CR_MVC_3PL_20180315-001 Added By NIthya
        void Update_To_proc_save_so_hdr(OutboundInq objOutboundInq);
        void Update_To_proc_save_so_addr(OutboundInq objOutboundInq);
        OutboundInq GetItemCode(OutboundInq objOutboundInq);
        //CR20180504-001 Added By Nithya
        OutboundInq GetPickQty(OutboundInq objOutboundInq);
        void UpdateAvailQtyTrnHdr(OutboundInq objOutboundInq);
        //END
        //CR-20180522-001 Added By Nithya    
        OutboundInq CheckOpenOrderExist(OutboundInq objOutboundInq);
        OutboundInq ShowStockVerifyRpt(OutboundInq objOutboundInq);
        OutboundInq GetItemName(OutboundInq objOutboundInq);
        OutboundInq IsOpenAlocationExists(OutboundInq objOutboundInq);
        //CR-20180529-001 Added By Nithya   
        OutboundInq Validate_LotId(OutboundInq objOutboundInq);
        OutboundInq Validate_IbdocId(OutboundInq objOutboundInq);
        OutboundInq ExistLoc(OutboundInq objOutboundInq);
        void Insert_loc_hdr(OutboundInq objOutboundInq);
        void Save_Lot_hdr(OutboundInq objOutboundInq);
        void Save_Lot_dtl(OutboundInq objOutboundInq);
        void Save_Lot_ctn(OutboundInq objOutboundInq);
        void Save_iv_Itm_trn_in(OutboundInq objOutboundInq);
        void Save_iv_Itm_trn_hst(OutboundInq objOutboundInq);
        void Update_trn_hdr(OutboundInq objOutboundInq);
        void Add_To_proc_save_doc_hdr(OutboundInq objOutboundInq);
        void Add_To_proc_save_doc_dtl(OutboundInq objOutboundInq);
        void Add_To_proc_save_doc_ctn(OutboundInq objOutboundInq);
        void Insert_StockVerify(OutboundInq objOutboundInq);
        void Delete_StockVerify(OutboundInq objOutboundInq);
        OutboundInq Get_StockList(OutboundInq objOutboundInq);
        OutboundInq Get_IbdocId(OutboundInq objOutboundInq);
        OutboundInq Get_LotId(OutboundInq objOutboundInq);
        OutboundInq GetPaletIdValidation(OutboundInq objOutboundInq);
        OutboundInq GetpaletId(OutboundInq objOutboundInq);
        void Change_dochdr_Status(OutboundInq objOutboundInq);
        OutboundInq OutboundGETTEMPLISTvalue(OutboundInq objOutboundInq);
        OutboundInq Update_Tbl_Temp_So_Auto_Aloc_BackOrdervalue(OutboundInq objOutboundInq);
        OutboundInq GETSONUMLIST(OutboundInq objOutboundInq);
        OutboundInq StockverifyRpt(OutboundInq objOutboundInq);
        OutboundInq GetEcomSR940Rpt(OutboundInq objOutboundInq);
        OutboundInq GetStockVerifyRptTotalCottonRecords(OutboundInq objOutboundInq);
        OutboundInq Add_To_proc_save_audit_trail(OutboundInq objOutboundInq);
        bool fnGenerateIBfromOB(string pstrCmpId, string pstrSoNum, string pstrAlocDocId);
        OutboundInq ItemXGetshiptoDetails(string term, string cmp_id,string cust_id);
        OutboundInq CheckShipToidExist(OutboundInq objOutboundInq);
        OutboundInq GetCustDtl(string term, string cmp_id);
        void Change_LotDtl_Status_atAdd(OutboundInq objOutboundInq);
        OutboundInq LoadCustDtls(OutboundInq objOutboundInq);
        OBGetBOLConf GetOBBOLConfDtlRptData(OBGetBOLConf objOBBOLConfDtlRptData, string p_str_cmp_id, string p_str_so_num_from, string p_str_so_num_to, string p_str_ship_dt_from, string p_str_ship_dt_to, string p_str_batch_id);
        OBGetSRBOLConfRpt GetOBSRBOLConfRpt(OBGetSRBOLConfRpt objOBSRBOLConfRptData, string p_str_cmp_id, string p_str_batch_id, string p_str_so_num, string p_str_so_num_from, string p_str_so_num_to, string p_str_so_dt_from, string p_str_so_dt_to);
        string GetSRPickRefNumber(string p_str_cmp_id, string p_str_so_num);
         OBDocExcp GetOBDocExcpList(OBDocExcp objOBDocExcp, string p_str_cmp_id);
        OBAlocPostInquiry GetOBAlocOpenList(OBAlocPostInquiry objOBAlocPostInquiry);
        int GetAlocPostRefNo();
        DataTable OutboundInqShipAckExcel(string l_str_cmp_id, string l_str_so_num);
        DataTable OutboundInqGridSummaryExcel(string l_str_cmp_id, string l_str_AlocId, string l_str_so_num_frm, string l_str_so_num_To, string l_str_so_dt_frm, string l_str_so_dt_to, string l_str_CustPO, string l_str_status, string l_str_Store, string l_str_batch_id, string l_str_shipdtFm, string l_str_shipdtTo, string l_str_Style, string l_str_color, string l_str_size);
        DataTable OutboundInqPickByStyle(string l_str_cmp_id, string l_str_so_num_frm, string l_str_so_num_To, string l_str_batch_id);
        DataTable OutboundInqPickByLocation(string l_str_cmp_id, string l_str_so_num_frm, string l_str_so_num_To, string l_str_batch_id);
        DataTable OutboundInqSummaryExcel(string l_str_cmp_id, string l_str_so_NumFm, string l_str_so_NumTo, string l_str_so_DtFm, string l_str_so_DtTo, string l_str_CustPo, string l_str_AlocNo, string l_str_status, string l_str_store, string l_str_batchId, string l_str_ShipdtFm, string l_str_ShipdtTo, string l_str_Style, string l_str_Color, string l_str_Size);
        bool delAlocAndUpdtLoc(string p_str_cmp_id, string p_str_aloc_doc_id, string p_str_sr_num, string p_str_new_loc_id);
        OutboundInq addInvTransHdr(OutboundInq objOutboundInq);
        int fnCheckOBCustPOExists(string pstrCmpId, string pstrCustOrdrNum);

    }
}
