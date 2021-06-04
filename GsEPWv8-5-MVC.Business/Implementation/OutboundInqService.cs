using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
    public class OutboundInqService : IOutboundInqService
    {
        IOutboundInqRepository objRepository = new OutboundInqRepository();

        public OutboundInq GetEcomitmlist(OutboundInq objOutboundInq)
        {
            return objRepository.GetEcomitmlist(objOutboundInq);
        }
        public OutboundInq LoadCustConfig(OutboundInq objOutboundInq)
        {
            return objRepository.LoadCustConfig(objOutboundInq);
        }
        public OBAlocPostInquiry GetEcomOBAlocOpenList(string pstrCmpId, string pstrBatchId)
        {
            return objRepository.GetEcomOBAlocOpenList(pstrCmpId, pstrBatchId);
        }
        public OutboundInq fnSaveSoTracking(string pstrMode, string pstrCmpId, string pstrSoNum, string pstrTrackNumType, string pstrTrackNum, string pstrTrackStatus, string pstrTrackDate, string pstrProcessId)
        {
            return objRepository.fnSaveSoTracking( pstrMode,  pstrCmpId,  pstrSoNum,  pstrTrackNumType,  pstrTrackNum,  pstrTrackStatus,  pstrTrackDate,  pstrProcessId);
        }

         public OutboundInq fnGetSoTracking(string pstrCmpId, string pstrSoNum, string pstrTrackNum)
        {
            return objRepository.fnGetSoTracking( pstrCmpId, pstrSoNum,  pstrTrackNum);
        }
        public OutboundInq GetOutboundInq(OutboundInq objOutboundInq)
        {
            return objRepository.GetOutboundInq(objOutboundInq);
        }

        public OutboundInq GetEcomOrderInq(OutboundInq objOutboundInq)
        {
            return objRepository.GetEcomOrderInq(objOutboundInq);
        }

        public bool fnEcomSaveBatchPrint(clsEComPrintOrders objEComPrintOrders)
        {
            return objRepository.fnEcomSaveBatchPrint(objEComPrintOrders);
        }
        public OutboundInq fnEcomBatchPrint(string pstrCmpId, string pstrBatchId)
        {
            return objRepository.fnEcomBatchPrint(pstrCmpId, pstrBatchId);
        }

        public OutboundInq GetStyleDetails(OutboundInq objOutboundInq)
        {
            return objRepository.GetStyleDetails(objOutboundInq);
        }
        public OutboundInq GetTempFiledtl(OutboundInq objOutboundInq)
        {
            return objRepository.GetTempFiledtl(objOutboundInq);
        }
        public OutboundInq Get_AlocBackOrderCount(OutboundInq objOutboundInq)
        {
            return objRepository.Get_AlocBackOrderCount(objOutboundInq);
        }
        public OutboundInq InsertTempFileDocument(OutboundInq objOutboundInq)
        {
            return objRepository.InsertTempFileDocument(objOutboundInq);
        }
        public OutboundInq OutboundInqShipAckCtnvalues(OutboundInq objOutboundInq)
        {
            return objRepository.OutboundInqShipAckCtnvalues(objOutboundInq);
        }
        public OutboundInq OutboundInqSUmmaryRpt(OutboundInq objOutboundInq)
        {
            return objRepository.OutboundInqSUmmaryRpt(objOutboundInq);
        }
        public OutboundInq GetUnPickQty(OutboundInq objOutboundInq)
        {
            return objRepository.GetUnPickQty(objOutboundInq);
        }
        public OutboundInq GetCustId(OutboundInq objOutboundInq)
        {
            return objRepository.GetCustId(objOutboundInq);
        }
        public OutboundInq GetAlocUnPostGridLoadItem(OutboundInq objOutboundInq)
        {
            return objRepository.GetAlocUnPostGridLoadItem(objOutboundInq);
        }
        public OutboundInq GetShipToAddress(OutboundInq objOutboundInq)
        {
            return objRepository.GetShipToAddress(objOutboundInq);
        }
        public OutboundInq CheckAllocpost(OutboundInq objOutboundInq)
        {
            return objRepository.CheckAllocpost(objOutboundInq);
        }
        public void SaveAlocUnPost(OutboundInq objOutboundInq)
        {
            objRepository.SaveAlocUnPost(objOutboundInq);
        }
        public void InsertShipHdr(OutboundInq objOutboundInq)
        {
            objRepository.InsertShipHdr(objOutboundInq);
        }
        public void UpdateTrnHdr(OutboundInq objOutboundInq)
        {
            objRepository.UpdateTrnHdr(objOutboundInq);
        }
        public void InsertShipDtl(OutboundInq objOutboundInq)
        {
            objRepository.InsertShipDtl(objOutboundInq);
        }

        public OutboundInq GetOutSaleId(OutboundInq objOutboundInq)
        {
            return objRepository.GetOutSaleId(objOutboundInq);
        }
        public OutboundInq OutboundInqPickStyleRpt(OutboundInq objOutboundInq)
        {
            return objRepository.OutboundInqPickStyleRpt(objOutboundInq);
        }
        public OutboundInq OutboundInqPickLocationRpt(OutboundInq objOutboundInq)
        {
            return objRepository.OutboundInqPickLocationRpt(objOutboundInq);
        }
        public OutboundInq GetCheckExistStyle(OutboundInq objOutboundInq)
        {
            return objRepository.GetCheckExistStyle(objOutboundInq);
        }
        public OutboundInq GetAlocGridLoadItem(OutboundInq objOutboundInq)
        {
            return objRepository.GetAlocGridLoadItem(objOutboundInq);
        }
        public OutboundInq GetPickGridLoadItem(OutboundInq objOutboundInq)
        {
            return objRepository.GetPickGridLoadItem(objOutboundInq);
        }
        public OutboundInq GetShipNum(OutboundInq objOutboundInq)
        {
            return objRepository.GetShipNum(objOutboundInq);
        }
        public OutboundInq GetAlocType(OutboundInq objOutboundInq)
        {
            return objRepository.GetAlocType(objOutboundInq);
        }
        public void GetAlocPostDirect(OutboundInq objOutboundInq)
        {
            objRepository.GetAlocPostDirect(objOutboundInq);
        }
        public void GetAlocPost(OutboundInq objOutboundInq)
        {
            objRepository.GetAlocPost(objOutboundInq);
        }
        public void UpdateStatusInAlocHdr(OutboundInq objOutboundInq)
        {
            objRepository.UpdateStatusInAlocHdr(objOutboundInq);
        }
        public void UpdateStatusInAlocDtl(OutboundInq objOutboundInq)
        {
            objRepository.UpdateStatusInAlocDtl(objOutboundInq);
        }
        public OutboundInq OutboundInqGridSummary(OutboundInq objOutboundInq)
        {
            return objRepository.OutboundInqGridSummary(objOutboundInq);
        }
        public OutboundInq OutboundInqShipAck(OutboundInq objOutboundInq)
        {
            return objRepository.OutboundInqShipAck(objOutboundInq);
        }
        public OutboundInq OutboundShipInqhdr(OutboundInq objOutboundInq)
        {
            return objRepository.OutboundShipInqhdr(objOutboundInq);
        }
        public OutboundInq OutboundShipInqdtl(OutboundInq objOutboundInq)
        {
            return objRepository.OutboundShipInqdtl(objOutboundInq);
        }
        public OutboundInq GetIbSRIdDetail(OutboundInq objOutboundInq)
        {
            return objRepository.GetIbSRIdDetail(objOutboundInq);
        }
        public OutboundInq ItemXGetitmDetails(string term, string cmp_id)
        {
            return objRepository.ItemXGetitmDetails(term, cmp_id);
        }
        public void InsertTemptableValue(OutboundInq objOutboundInq)
        {
            objRepository.InsertTemptableValue(objOutboundInq);
        }
        public void DeleteTempshipEntry(OutboundInq objOutboundInq)
        {
            objRepository.DeleteTempshipEntry(objOutboundInq);
        }
        public OutboundInq GetShipReqEditDtl(OutboundInq objOutboundInq)
        {
            return objRepository.GetShipReqEditDtl(objOutboundInq);
        }

        public OutboundInq GetGridList(OutboundInq objOutboundInq)
        {
            return objRepository.GetGridList(objOutboundInq);
        }
        public OutboundInq GetSRIdDtl(OutboundInq objOutboundInq)
        {
            return objRepository.GetSRIdDtl(objOutboundInq);
        }
        public OutboundInq LoadAvailQty(OutboundInq objOutboundInq)
        {
            return objRepository.LoadAvailQty(objOutboundInq);
        }
        public OutboundInq GetPickStyleDetails(OutboundInq objOutboundInq)
        {
            return objRepository.GetPickStyleDetails(objOutboundInq);
        }
        public OutboundInq GetShipReqEntryTempGridDtl(OutboundInq objOutboundInq)
        {
            return objRepository.GetShipReqEntryTempGridDtl(objOutboundInq);
        }
        public void Add_To_proc_save_so_dtl_excel(OutboundInq objOutboundInq)
        {
            objRepository.Add_To_proc_save_so_dtl_excel(objOutboundInq);
        }
        public void Add_To_proc_save_so_dtl_due_excel(OutboundInq objOutboundInq)
        {
            objRepository.Add_To_proc_save_so_dtl_due_excel(objOutboundInq);
        }
        public void Add_To_proc_save_so_hdr_excel(OutboundInq objOutboundInq)
        {
            objRepository.Add_To_proc_save_so_hdr_excel(objOutboundInq);
        }
        public void Add_To_proc_save_so_addr_excel(OutboundInq objOutboundInq)
        {
            objRepository.Add_To_proc_save_so_addr_excel(objOutboundInq);
        }
        public OutboundInq Getkitflag(OutboundInq objOutboundInq)
        {
            return objRepository.Getkitflag(objOutboundInq);
        }
        public OutboundInq GetCustDtl(OutboundInq objOutboundInq)
        {
            return objRepository.GetCustDtl(objOutboundInq);
        }
        public OutboundInq Getitmlist(OutboundInq objOutboundInq)
        {
            return objRepository.Getitmlist(objOutboundInq);
        }
        public OutboundInq GetCSVList(OutboundInq objOutboundInq)
        {
            return objRepository.GetCSVList(objOutboundInq);
        }
        public void OutboundInqTempDelete(OutboundInq objOutboundInq)
        {
            objRepository.OutboundInqTempDelete(objOutboundInq);
        }
        public OutboundInq GetViewDetail(OutboundInq objOutboundInq)
        {
            return objRepository.GetViewDetail(objOutboundInq);
        }
        public OutboundInq GetViewDetailgrid(OutboundInq objOutboundInq)
        {
            return objRepository.GetViewDetailgrid(objOutboundInq);
        }
        public OutboundInq GetViewAddrDetail(OutboundInq objOutboundInq)
        {
            return objRepository.GetViewAddrDetail(objOutboundInq);
        }
        public void DeleteShipEntry(OutboundInq objOutboundInq)
        {
            objRepository.DeleteShipEntry(objOutboundInq);
        }
        public void DeleteTempshipEntrytable(OutboundInq objOutboundInq)
        {
            objRepository.DeleteTempshipEntrytable(objOutboundInq);
        }
        public OutboundInq GetGridEditData(OutboundInq objOutboundInq)
        {
            return objRepository.GetGridEditData(objOutboundInq);
        }
        public OutboundInq GetCheckExistGridData(OutboundInq objOutboundInq)
        {
            return objRepository.GetCheckExistGridData(objOutboundInq);
        }
        public OutboundInq GetGridDeleteData(OutboundInq objOutboundInq)
        {
            return objRepository.GetGridDeleteData(objOutboundInq);
        }
        public OutboundInq GetShipToAddressSave(OutboundInq objOutboundInq)
        {
            return objRepository.GetShipToAddressSave(objOutboundInq);
        }
        public OutboundInq GetItmNameDetails(OutboundInq objOutboundInq)
        {
            return objRepository.GetItmNameDetails(objOutboundInq);
        }
        public OutboundInq OutboundShipAloc(OutboundInq objOutboundInq)
        {
            return objRepository.OutboundShipAloc(objOutboundInq);
        }
        public OutboundInq SoNumFrom_Validation(OutboundInq objOutboundInq)
        {
            return objRepository.SoNumFrom_Validation(objOutboundInq);
        }
        public OutboundInq OutboundSoldtoId(OutboundInq objOutboundInq)
        {
            return objRepository.OutboundSoldtoId(objOutboundInq);
        }
        public OutboundInq GetObalocIdDetail(OutboundInq objOutboundInq)
        {
            return objRepository.GetObalocIdDetail(objOutboundInq);
        }
        public OutboundInq OutboundSelectionhdr(OutboundInq objOutboundInq)
        {
            return objRepository.OutboundSelectionhdr(objOutboundInq);
        }
        public void InsertTempAUTOALOC(OutboundInq objOutboundInq)
        {
            objRepository.InsertTempAUTOALOC(objOutboundInq);
        }
        public OutboundInq OutboundGETITMNAME(OutboundInq objOutboundInq)
        {
            return objRepository.OutboundGETITMNAME(objOutboundInq);
        }
        public OutboundInq OutboundGETALOCSORTSTMT(OutboundInq objOutboundInq)
        {
            return objRepository.OutboundGETALOCSORTSTMT(objOutboundInq);
        }
        public OutboundInq OutboundGETAVILQTY(OutboundInq objOutboundInq)
        {
            return objRepository.OutboundGETAVILQTY(objOutboundInq);
        }
        public OutboundInq OutboundGETTEMPLIST(OutboundInq objOutboundInq)
        {
            return objRepository.OutboundGETTEMPLIST(objOutboundInq);
        }
        public void DeleteAUTOALOC(OutboundInq objOutboundInq)
        {
            objRepository.DeleteAUTOALOC(objOutboundInq);
        }
        public void InsertTempAlocdtl(OutboundInq objOutboundInq)
        {
            objRepository.InsertTempAlocdtl(objOutboundInq);
        }
        public void InsertTempAlocSummary(OutboundInq objOutboundInq)
        {
            objRepository.InsertTempAlocSummary(objOutboundInq);
        }
        public OutboundInq OutboundGETTEMPALOCSUMMARY(OutboundInq objOutboundInq)
        {
            return objRepository.OutboundGETTEMPALOCSUMMARY(objOutboundInq);
        }
        public OutboundInq OutboundGETALOCDTL(OutboundInq objOutboundInq)
        {
            return objRepository.OutboundGETALOCDTL(objOutboundInq);
        }
        public OutboundInq OutboundGETTEMPALOCDTL(OutboundInq objOutboundInq)
        {
            return objRepository.OutboundGETTEMPALOCDTL(objOutboundInq);
        }

        public void Update_aloc_num(OutboundInq objOutboundInq)
        {
            objRepository.Update_aloc_num(objOutboundInq);
        }
        public OutboundInq Move_to_aloc_hdr(OutboundInq objOutboundInq)
        {
            return objRepository.Move_to_aloc_hdr(objOutboundInq);
        }
        public OutboundInq Move_to_aloc_dtl(OutboundInq objOutboundInq)
        {
            return objRepository.Move_to_aloc_dtl(objOutboundInq);
        }
        public OutboundInq Move_to_aloc_ctn(OutboundInq objOutboundInq)
        {
            return objRepository.Move_to_aloc_ctn(objOutboundInq);
        }
        public void Moveto_TrnHdr(OutboundInq objOutboundInq)
        {
            objRepository.Moveto_TrnHdr(objOutboundInq);
        }
        public OutboundInq Update_so_dtl(OutboundInq objOutboundInq)
        {
            return objRepository.Update_so_dtl(objOutboundInq);
        }
        public void Move_To_Grd_Bad_Itm(OutboundInq objOutboundInq)
        {
            objRepository.Move_To_Grd_Bad_Itm(objOutboundInq);
        }
        public void Update_Tbl_iv_itm_trn_in(OutboundInq objOutboundInq)
        {
            objRepository.Update_Tbl_iv_itm_trn_in(objOutboundInq);
        }
        public void Change_SOHdr_Status_atAdd(OutboundInq objOutboundInq)
        {
            objRepository.Change_SOHdr_Status_atAdd(objOutboundInq);
        }
        public OutboundInq Get_Newqty(OutboundInq objOutboundInq)
        {
            return objRepository.Get_Newqty(objOutboundInq);
        }
        public OutboundInq update_alocctn(OutboundInq objOutboundInq)
        {
            return objRepository.update_alocctn(objOutboundInq);
        }
        public OutboundInq GetSRGridRowCount(OutboundInq objOutboundInq)
        {
            return objRepository.GetSRGridRowCount(objOutboundInq);
        }
        public OutboundInq Get_PkgID(OutboundInq objOutboundInq)
        {
            return objRepository.Get_PkgID(objOutboundInq);
        }

        public OutboundInq Get_itmtrninList(OutboundInq objOutboundInq)
        {
            return objRepository.Get_itmtrninList(objOutboundInq);
        }
        public void Aloc_SpltCtn_Upd_Itm_Trn_in_direc(OutboundInq objOutboundInq)
        {
            objRepository.Aloc_SpltCtn_Upd_Itm_Trn_in_direc(objOutboundInq);
        }
        public void Aloc_Upd_data_to_itm_trn_in_direc(OutboundInq objOutboundInq)
        {
            objRepository.Aloc_Upd_data_to_itm_trn_in_direc(objOutboundInq);
        }
        public void Aloc_into_Itm_Trn_hst_by_itm_del_direc(OutboundInq objOutboundInq)
        {
            objRepository.Aloc_into_Itm_Trn_hst_by_itm_del_direc(objOutboundInq);
        }
        public void Aloc_into_Itm_Trn_hst_by_itm_direc(OutboundInq objOutboundInq)
        {
            objRepository.Aloc_into_Itm_Trn_hst_by_itm_direc(objOutboundInq);
        }
        public OutboundInq Get_AlocSaveRpt(OutboundInq objOutboundInq)
        {
            return objRepository.Get_AlocSaveRpt(objOutboundInq);
        }
        public OutboundInq GetDftWhs(OutboundInq objOutboundInq)
        {
            return objRepository.GetDftWhs(objOutboundInq);
        }
        public OutboundInq GetalochdrList(OutboundInq objOutboundInq)
        {
            return objRepository.GetalochdrList(objOutboundInq);
        }
        public OutboundInq GetalocdtlList(OutboundInq objOutboundInq)
        {
            return objRepository.GetalocdtlList(objOutboundInq);
        }
        public OutboundInq GetalocctnList(OutboundInq objOutboundInq)
        {
            return objRepository.GetalocctnList(objOutboundInq);
        }
        public OutboundInq GetalocdueList(OutboundInq objOutboundInq)
        {
            return objRepository.GetalocdueList(objOutboundInq);
        }
        public OutboundInq Del_Alloc_Upd_SO(OutboundInq objOutboundInq)
        {
            return objRepository.Del_Alloc_Upd_SO(objOutboundInq);
        }
        public OutboundInq Del_data_to_itm_trn_in(OutboundInq objOutboundInq)
        {
            return objRepository.Del_data_to_itm_trn_in(objOutboundInq);
        }
        public OutboundInq Add_To_Trn_Hdr(OutboundInq objOutboundInq)
        {
            return objRepository.Add_To_Trn_Hdr(objOutboundInq);
        }
        public OutboundInq BackOrderRpt(OutboundInq objOutboundInq)
        {
            return objRepository.BackOrderRpt(objOutboundInq);
        }
        public OutboundInq Get_AlocBackOrderRptList(OutboundInq objOutboundInq)
        {
            return objRepository.Get_AlocBackOrderRptList(objOutboundInq);
        }
        public OutboundInq GetaloceditmanualList(OutboundInq objOutboundInq)
        {
            return objRepository.GetaloceditmanualList(objOutboundInq);
        }
        public OutboundInq GetSelectedgridValue(OutboundInq objOutboundInq)
        {
            return objRepository.GetSelectedgridValue(objOutboundInq);
        }
        public OutboundInq GetALOCEditList(OutboundInq objOutboundInq)
        {
            return objRepository.GetALOCEditList(objOutboundInq);
        }
        public OutboundInq SP_Aloc_del_Itm_trn_hst(OutboundInq objOutboundInq)
        {
            return objRepository.SP_Aloc_del_Itm_trn_hst(objOutboundInq);
        }
        public OutboundInq Sp_Aloc_Mod_Daloc_iv_itm_trn_in(OutboundInq objOutboundInq)
        {
            return objRepository.Sp_Aloc_Mod_Daloc_iv_itm_trn_in(objOutboundInq);
        }
        public OutboundInq GetAddaloceditmanualList(OutboundInq objOutboundInq)
        {
            return objRepository.GetAddaloceditmanualList(objOutboundInq);
        }
        public OutboundInq GetSelectedgridValueList(OutboundInq objOutboundInq)
        {
            return objRepository.GetSelectedgridValueList(objOutboundInq);
        }
        public OutboundInq UpdateTemp_Alloc_Summary(OutboundInq objOutboundInq)
        {
            return objRepository.UpdateTemp_Alloc_Summary(objOutboundInq);
        }
        public OutboundInq Update_Tbl_Temp_So_Auto_Aloc(OutboundInq objOutboundInq)
        {
            return objRepository.Update_Tbl_Temp_So_Auto_Aloc(objOutboundInq);
        }
        public OutboundInq DeleteTemp_Alloc_Summary(OutboundInq objOutboundInq)
        {
            return objRepository.DeleteTemp_Alloc_Summary(objOutboundInq);
        }
        public void DelSo_dtl_Due_Table(OutboundInq objOutboundInq)//CR_MVC_3PL_20180315-001 Added By NIthya
        {
            objRepository.DelSo_dtl_Due_Table(objOutboundInq);
        }
        public void Update_To_proc_save_so_hdr(OutboundInq objOutboundInq)
        {
            objRepository.Update_To_proc_save_so_hdr(objOutboundInq);
        }
        public void Update_To_proc_save_so_addr(OutboundInq objOutboundInq)
        {
            objRepository.Update_To_proc_save_so_addr(objOutboundInq);
        }
        public OutboundInq GetItemCode(OutboundInq objOutboundInq)
        {
            return objRepository.GetItemCode(objOutboundInq);
        }
        //CR20180504-001 Added By Nithya
        public OutboundInq GetPickQty(OutboundInq objOutboundInq)
        {
            return objRepository.GetPickQty(objOutboundInq);
        }
        public void UpdateAvailQtyTrnHdr(OutboundInq objOutboundInq)
        {
            objRepository.UpdateAvailQtyTrnHdr(objOutboundInq);
        }
        //END
        //CR-20180522-001 Added By Nithya    
        public OutboundInq CheckOpenOrderExist(OutboundInq objOutboundInq)
        {
            return objRepository.CheckOpenOrderExist(objOutboundInq);
        }
        public OutboundInq ShowStockVerifyRpt(OutboundInq objOutboundInq)
        {
            return objRepository.ShowStockVerifyRpt(objOutboundInq);
        }
        public OutboundInq GetItemName(OutboundInq objOutboundInq)
        {
            return objRepository.GetItemName(objOutboundInq);
        }
        public OutboundInq IsOpenAlocationExists(OutboundInq objOutboundInq)
        {
            return objRepository.IsOpenAlocationExists(objOutboundInq);
        }
        //CR-20180529-001 Added By Nithya       
        public OutboundInq Validate_LotId(OutboundInq objOutboundInq)
        {
            return objRepository.Validate_LotId(objOutboundInq);
        }
        public OutboundInq Validate_IbdocId(OutboundInq objOutboundInq)
        {
            return objRepository.Validate_IbdocId(objOutboundInq);
        }
        public OutboundInq ExistLoc(OutboundInq objOutboundInq)
        {
            return objRepository.ExistLoc(objOutboundInq);
        }
        public void Insert_loc_hdr(OutboundInq objOutboundInq)
        {
            objRepository.Insert_loc_hdr(objOutboundInq);
        }
        public void Save_Lot_hdr(OutboundInq objOutboundInq)
        {
            objRepository.Save_Lot_hdr(objOutboundInq);
        }
        public void Save_Lot_dtl(OutboundInq objOutboundInq)
        {
            objRepository.Save_Lot_dtl(objOutboundInq);
        }
        public void Save_Lot_ctn(OutboundInq objOutboundInq)
        {
            objRepository.Save_Lot_ctn(objOutboundInq);
        }
        public void Save_iv_Itm_trn_in(OutboundInq objOutboundInq)
        {
            objRepository.Save_iv_Itm_trn_in(objOutboundInq);
        }
        public void Save_iv_Itm_trn_hst(OutboundInq objOutboundInq)
        {
            objRepository.Save_iv_Itm_trn_hst(objOutboundInq);
        }
        public void Update_trn_hdr(OutboundInq objOutboundInq)
        {
            objRepository.Update_trn_hdr(objOutboundInq);
        }
        public void Add_To_proc_save_doc_hdr(OutboundInq objOutboundInq)
        {
            objRepository.Add_To_proc_save_doc_hdr(objOutboundInq);
        }
        public void Add_To_proc_save_doc_dtl(OutboundInq objOutboundInq)
        {
            objRepository.Add_To_proc_save_doc_dtl(objOutboundInq);
        }
        public void Add_To_proc_save_doc_ctn(OutboundInq objOutboundInq)
        {
            objRepository.Add_To_proc_save_doc_ctn(objOutboundInq);
        }
        public void Insert_StockVerify(OutboundInq objOutboundInq)
        {
            objRepository.Insert_StockVerify(objOutboundInq);
        }
        public void Delete_StockVerify(OutboundInq objOutboundInq)
        {
            objRepository.Delete_StockVerify(objOutboundInq);
        }
        public OutboundInq Get_StockList(OutboundInq objOutboundInq)
        {
            return objRepository.Get_StockList(objOutboundInq);
        }
        public OutboundInq Get_IbdocId(OutboundInq objOutboundInq)
        {
            return objRepository.Get_IbdocId(objOutboundInq);
        }
        public OutboundInq Get_LotId(OutboundInq objOutboundInq)
        {
            return objRepository.Get_LotId(objOutboundInq);
        }
        public OutboundInq GetPaletIdValidation(OutboundInq objOutboundInq)
        {
            return objRepository.GetPaletIdValidation(objOutboundInq);
        }
        public OutboundInq GetpaletId(OutboundInq objOutboundInq)
        {
            return objRepository.GetpaletId(objOutboundInq);
        }
        public void Change_dochdr_Status(OutboundInq objOutboundInq)
        {
            objRepository.Change_dochdr_Status(objOutboundInq);
        }
        public OutboundInq OutboundGETTEMPLISTvalue(OutboundInq objOutboundInq)
        {
            return objRepository.OutboundGETTEMPLISTvalue(objOutboundInq);
        }
        public OutboundInq Update_Tbl_Temp_So_Auto_Aloc_BackOrdervalue(OutboundInq objOutboundInq)
        {
            return objRepository.Update_Tbl_Temp_So_Auto_Aloc_BackOrdervalue(objOutboundInq);
        }
        public OutboundInq GETSONUMLIST(OutboundInq objOutboundInq)
        {
            return objRepository.GETSONUMLIST(objOutboundInq);
        }
        public OutboundInq StockverifyRpt(OutboundInq objOutboundInq)
        {
            return objRepository.StockverifyRpt(objOutboundInq);
        }
        public OutboundInq GetEcomSR940Rpt(OutboundInq objOutboundInq)
        {
            return objRepository.GetEcomSR940Rpt(objOutboundInq);
        }
        public OutboundInq GetStockVerifyRptTotalCottonRecords(OutboundInq objOutboundInq)
        {
            return objRepository.GetStockVerifyRptTotalCottonRecords(objOutboundInq);
        }
        public OutboundInq Add_To_proc_save_audit_trail(OutboundInq objOutboundInq)
        {
            return objRepository.Add_To_proc_save_audit_trail(objOutboundInq);
        }
        public bool fnGenerateIBfromOB(string pstrCmpId, string pstrSoNum, string pstrAlocDocId)
        {
            return objRepository.fnGenerateIBfromOB(pstrCmpId, pstrSoNum, pstrAlocDocId);
        }
        public OutboundInq ItemXGetshiptoDetails(string term, string cmp_id, string cust_id)
        {
            return objRepository.ItemXGetshiptoDetails(term, cmp_id, cust_id);
        }
        public OutboundInq CheckShipToidExist(OutboundInq objOutboundInq)
        {
            return objRepository.CheckShipToidExist(objOutboundInq);
        }
        public OutboundInq GetCustDtl(string term, string cmp_id)
        {
            return objRepository.GetCustDtl(term, cmp_id);
        }
        public void Change_LotDtl_Status_atAdd(OutboundInq objOutboundInq)
        {
            objRepository.Change_LotDtl_Status_atAdd(objOutboundInq);
        }
        public OutboundInq LoadCustDtls(OutboundInq objOutboundInq)
        {
            return objRepository.LoadCustDtls(objOutboundInq);
        }

        public OBGetBOLConf GetOBBOLConfDtlRptData(OBGetBOLConf objOBBOLConfDtlRptData, string p_str_cmp_id, string p_str_so_num_from, string p_str_so_num_to, string p_str_ship_dt_from, string p_str_ship_dt_to, string p_str_batch_id)
        {
            return objRepository.GetOBBOLConfDtlRptData( objOBBOLConfDtlRptData,  p_str_cmp_id, p_str_so_num_from, p_str_so_num_to,  p_str_ship_dt_from,  p_str_ship_dt_to,  p_str_batch_id);
        }

        public OBGetSRBOLConfRpt GetOBSRBOLConfRpt(OBGetSRBOLConfRpt objOBSRBOLConfRptData, string p_str_cmp_id, string p_str_batch_id, string p_str_so_num, string p_str_so_num_from, string p_str_so_num_to, string p_str_so_dt_from, string p_str_so_dt_to)
        {
            return objRepository.GetOBSRBOLConfRpt( objOBSRBOLConfRptData,  p_str_cmp_id, p_str_batch_id,  p_str_so_num, p_str_so_num_from,  p_str_so_num_to,  p_str_so_dt_from,  p_str_so_dt_to);
        }

        public string GetSRPickRefNumber(string p_str_cmp_id, string p_str_so_num)
        {
            return objRepository.GetSRPickRefNumber(p_str_cmp_id, p_str_so_num);
        }

        public OBDocExcp GetOBDocExcpList(OBDocExcp objOBDocExcp, string p_str_cmp_id)
        {
            return objRepository.GetOBDocExcpList( objOBDocExcp,  p_str_cmp_id);
        }

       public OBAlocPostInquiry GetOBAlocOpenList(OBAlocPostInquiry objOBAlocPostInquiry)
        {
            return objRepository.GetOBAlocOpenList( objOBAlocPostInquiry);
        }
        public int GetAlocPostRefNo()
        {
            return objRepository.GetAlocPostRefNo();
        }

        public bool SaveOBBulkAlocPost(string p_str_cmp_id, string p_str_aloc_tmp_ref_no, DataTable dtAlocList)
        {
            return objRepository.SaveOBBulkAlocPost(p_str_cmp_id, p_str_aloc_tmp_ref_no,  dtAlocList);
        }

        public DataTable OutboundInqShipAckExcel(string l_str_cmp_id, string l_str_so_num)
        {
            return objRepository.OutboundInqShipAckExcel(l_str_cmp_id, l_str_so_num);
        }
        public DataTable OutboundInqGridSummaryExcel(string l_str_cmp_id, string l_str_AlocId, string l_str_so_num_frm, string l_str_so_num_To, string l_str_so_dt_frm, string l_str_so_dt_to, string l_str_CustPO, string l_str_status, string l_str_Store, string l_str_batch_id, string l_str_shipdtFm, string l_str_shipdtTo, string l_str_Style, string l_str_color, string l_str_size)
        {
            return objRepository.OutboundInqGridSummaryExcel(l_str_cmp_id, l_str_AlocId, l_str_so_num_frm, l_str_so_num_To, l_str_so_dt_frm, l_str_so_dt_to, l_str_CustPO, l_str_status, l_str_Store, l_str_batch_id, l_str_shipdtFm, l_str_shipdtTo, l_str_Style, l_str_color, l_str_size);
        }

        public DataTable OutboundInqPickByStyle(string l_str_cmp_id, string l_str_so_num_frm, string l_str_so_num_To, string l_str_batch_id)
        {
            return objRepository.OutboundInqPickByStyle(l_str_cmp_id, l_str_so_num_frm, l_str_so_num_To, l_str_batch_id);
        }

        public DataTable OutboundInqPickByLocation(string l_str_cmp_id, string l_str_so_num_frm, string l_str_so_num_To, string l_str_batch_id)
        {
            return objRepository.OutboundInqPickByLocation(l_str_cmp_id, l_str_so_num_frm, l_str_so_num_To, l_str_batch_id);
        }

        public DataTable OutboundInqSummaryExcel(string l_str_cmp_id, string l_str_so_NumFm, string l_str_so_NumTo, string l_str_so_DtFm, string l_str_so_DtTo, string l_str_CustPo, string l_str_AlocNo, string l_str_status, string l_str_store, string l_str_batchId, string l_str_ShipdtFm, string l_str_ShipdtTo, string l_str_Style, string l_str_Color, string l_str_Size)
        {
            return objRepository.OutboundInqSummaryExcel(l_str_cmp_id, l_str_so_NumFm, l_str_so_NumTo, l_str_so_DtFm, l_str_so_DtTo, l_str_CustPo, l_str_AlocNo, l_str_status, l_str_store, l_str_batchId, l_str_ShipdtFm, l_str_ShipdtTo, l_str_Style, l_str_Color, l_str_Size);
        }
        public bool delAlocAndUpdtLoc(string p_str_cmp_id, string p_str_aloc_doc_id, string p_str_sr_num, string p_str_new_loc_id)
        {
            return objRepository.delAlocAndUpdtLoc( p_str_cmp_id,  p_str_aloc_doc_id,  p_str_sr_num,  p_str_new_loc_id);
        }
        public OutboundInq addInvTransHdr(OutboundInq objOutboundInq)
        {
            return objRepository.addInvTransHdr(objOutboundInq);
        }

        public int fnCheckOBCustPOExists(string pstrCmpId, string pstrCustOrdrNum)
        {
            return objRepository.fnCheckOBCustPOExists(pstrCmpId, pstrCustOrdrNum);
        }

    }
}
