using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
    public class InboundInquiryService : IInboundInquiryService
    {
      public  string fnCheckCheckBinRefered(string pstrCmpId, string pstrBinId, string pstrItmNum, string pstrItmColor, string pstrItmSize, string pstrItmCode)
        {
            return objRepository.fnCheckCheckBinRefered(pstrCmpId, pstrBinId, pstrItmNum, pstrItmColor, pstrItmSize, pstrItmCode);
        }
        public InboundInquiry GetInboundInquiryDetails(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetInboundInquiryDetails(objInboundInquiry);
        }
        InboundInquiryRepository objRepository = new InboundInquiryRepository();

        public InboundInquiry GetInboundAckRptDetails(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetInboundAckRptDetails(objInboundInquiry);
        }
        public void InsertTempDocEntryDetails(InboundInquiry objInboundInquiry)
        {
            objRepository.InsertTempDocEntryDetails(objInboundInquiry);
        }
        public void Add_To_proc_save_doc_dtl(InboundInquiry objInboundInquiry)
        {
            objRepository.Add_To_proc_save_doc_dtl(objInboundInquiry);
        }
        public void Add_To_proc_save_doc_ctn(InboundInquiry objInboundInquiry)
        {
            objRepository.Add_To_proc_save_doc_ctn(objInboundInquiry);
        }
        public void TruncateTempDocEntry(InboundInquiry objInboundInquiry)
        {
            objRepository.TruncateTempDocEntry(objInboundInquiry);
        }
        public void TruncateTempDocUpload(InboundInquiry objInboundInquiry)
        {
            objRepository.TruncateTempDocUpload(objInboundInquiry);
        }
        public void Add_Style_To_Itm_dtl(InboundInquiry objInboundInquiry)
        {
            objRepository.Add_Style_To_Itm_dtl(objInboundInquiry);
        }
        public void Add_Style_To_Itm_hdr(InboundInquiry objInboundInquiry)
        {
            objRepository.Add_Style_To_Itm_hdr(objInboundInquiry);
        }
        public void Add_To_proc_save_doc_hdr(InboundInquiry objInboundInquiry)
        {
            objRepository.Add_To_proc_save_doc_hdr(objInboundInquiry);
        }
        public void DeleteDocEntry(InboundInquiry objInboundInquiry)
        {
            objRepository.DeleteDocEntry(objInboundInquiry);
        }
        public void ReceivingPostDtls(InboundInquiry objInboundInquiry)
        {
            objRepository.ReceivingPostDtls(objInboundInquiry);
        }
        public void DocReceivingUnPost(InboundInquiry objInboundInquiry)
        {
            objRepository.DocReceivingUnPost(objInboundInquiry);
        }
        public void ReceivingPost9999Dtls(InboundInquiry objInboundInquiry)
        {
            objRepository.ReceivingPost9999Dtls(objInboundInquiry);
        }
        public void Update_doc_hdr(InboundInquiry objInboundInquiry)
        {
            objRepository.Update_doc_hdr(objInboundInquiry);
        }
        public void Del_doc_Dtl(InboundInquiry objInboundInquiry)
        {
            objRepository.Del_doc_Dtl(objInboundInquiry);
        }
        public InboundInquiry CanPost(InboundInquiry objInboundInquiry)
        {
            return objRepository.CanPost(objInboundInquiry);
        }
        public InboundInquiry DocTallySheetRpt(InboundInquiry objInboundInquiry)
        {
            return objRepository.DocTallySheetRpt(objInboundInquiry);
        }
        public InboundInquiry GetInboundConfirmationRptDetailsbyContainer(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetInboundConfirmationRptDetailsbyContainer(objInboundInquiry);
        }       
        public InboundInquiry GetRcvngUnPostDtls(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetRcvngUnPostDtls(objInboundInquiry);
        }
        public InboundInquiry LoadCustConfigRcvdItmMode(InboundInquiry objInboundInquiry)
        {
            return objRepository.LoadCustConfigRcvdItmMode(objInboundInquiry);
        }
        public InboundInquiry LoadCustConfig(InboundInquiry objInboundInquiry)
        {
            return objRepository.LoadCustConfig(objInboundInquiry);
        }
        public InboundInquiry GetRcvngGridDtl(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetRcvngGridDtl(objInboundInquiry);
        }

        public InboundInquiry GetRcvngHdr(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetRcvngHdr(objInboundInquiry);
        }
        public InboundInquiry GetRcvngPostDtls(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetRcvngPostDtls(objInboundInquiry);
        }
        public InboundInquiry LoadAvailDtl(InboundInquiry objInboundInquiry)
        {
            return objRepository.LoadAvailDtl(objInboundInquiry);
        }
        public InboundInquiry LoadLotItem(InboundInquiry objInboundInquiry)
        {
            return objRepository.LoadLotItem(objInboundInquiry);
        }

        public InboundInquiry GetDocuEntryTempGridDtl(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetDocuEntryTempGridDtl(objInboundInquiry);
        }
        public InboundInquiry GetGridDeleteData(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetGridDeleteData(objInboundInquiry);
        }
        public InboundInquiry GetCheckExistGridData(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetCheckExistGridData(objInboundInquiry);
        }
        public InboundInquiry CheckItmDimension(InboundInquiry objInboundInquiry)
        {
            return objRepository.CheckItmDimension(objInboundInquiry);
        }
        public InboundInquiry GetGridEditData(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetGridEditData(objInboundInquiry);
        }
        public InboundInquiry GetCSVList(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetCSVList(objInboundInquiry);
        }
        public InboundInquiry GetItemHdr(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetItemHdr(objInboundInquiry);
        }

        public InboundInquiry GetItmId(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetItmId(objInboundInquiry);
        }
        public InboundInquiry CheckItmHdr(InboundInquiry objInboundInquiry)
        {
            return objRepository.CheckItmHdr(objInboundInquiry);
        }
        public InboundInquiry GetDocumentEntryTempGridDtl(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetDocumentEntryTempGridDtl(objInboundInquiry);
        }
        public InboundInquiry GetDocHdr(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetDocHdr(objInboundInquiry);
        }
        public InboundInquiry GetDocDtl(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetDocDtl(objInboundInquiry);
        }
        public InboundInquiry ItemXGetitmDetails(string term, string cmp_id)
        {
            return objRepository.ItemXGetitmDetails(term, cmp_id);
        }
        public InboundInquiry GetIbDocIdDetail(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetIbDocIdDetail(objInboundInquiry);
        }
        public InboundInquiry IsRMAChecked(InboundInquiry objInboundInquiry)
        {
            return objRepository.IsRMAChecked(objInboundInquiry);
        }
        public InboundInquiry GetPickStyleDetails(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetPickStyleDetails(objInboundInquiry);
        }
        public InboundInquiry GetDocEntryId(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetDocEntryId(objInboundInquiry);
        }
        public InboundInquiry GetInboundWorkSheetRptDetails(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetInboundWorkSheetRptDetails(objInboundInquiry);
        }
        public InboundInquiry Getitmlist(InboundInquiry objInboundInquiry)
        {
            return objRepository.Getitmlist(objInboundInquiry);
        }
        public InboundInquiry LoadStrgId(InboundInquiry objInboundInquiry)
        {
            return objRepository.LoadStrgId(objInboundInquiry);
        }
        public InboundInquiry LoadInoutId(InboundInquiry objInboundInquiry)
        {
            return objRepository.LoadInoutId(objInboundInquiry);
        }
        public InboundInquiry GetInboundGridSummaryDetails(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetInboundGridSummaryDetails(objInboundInquiry);
        }
        public InboundInquiry GetInboundTallySheetRptDetails(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetInboundTallySheetRptDetails(objInboundInquiry);
        }
        public InboundInquiry GetInboundConfirmationRptDetails(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetInboundConfirmationRptDetails(objInboundInquiry);
        }
        public InboundInquiry GetInboundStatus(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetInboundStatus(objInboundInquiry);
        }
        public InboundInquiry GetInboundHdrDtl(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetInboundHdrDtl(objInboundInquiry);
        }
        public InboundInquiry GetInboundDtl(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetInboundDtl(objInboundInquiry);
        }
        public InboundInquiry GetInboundLotDtl(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetInboundLotDtl(objInboundInquiry);
        }
        public InboundACKExcel GetInboundAckExcel(InboundACKExcel objInboundInquiry)
        {
            return objRepository.GetInboundAckExcel(objInboundInquiry);
        }
        public InboundWorkSheetExcel GetInboundWorkSheetExcel(InboundWorkSheetExcel objInboundInquiry)
        {
            return objRepository.GetInboundWorkSheetExcel(objInboundInquiry);
        }
        public InboundTallySheetExcel GetInboundTallySheetExcel(InboundTallySheetExcel objInboundInquiry)
        {
            return objRepository.GetInboundTallySheetExcel(objInboundInquiry);
        }
        public InboundConfirmationExcel GetInboundConfimExcel(InboundConfirmationExcel objInboundInquiry)
        {
            return objRepository.GetInboundConfimExcel(objInboundInquiry);
        }
        public InboundInquiry GetInboundLotHdr(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetInboundLotHdr(objInboundInquiry);
        }

        public InboundInquiry GEtStrgBillTYpe(InboundInquiry objInboundInquiry)
        {
            return objRepository.GEtStrgBillTYpe(objInboundInquiry);
        }
        public InboundInquiry GetPalletId(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetPalletId(objInboundInquiry);
        }
        public InboundInquiry GetLotId(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetLotId(objInboundInquiry);
        }
        public InboundInquiry GetInsertTblIbDocDtlTmp(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetInsertTblIbDocDtlTmp(objInboundInquiry);
        }
        public InboundInquiry GetCustRcvdItemMode(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetCustRcvdItemMode(objInboundInquiry);
        }
        public InboundInquiry GetInsertTblIbDocRecvDtlTemp(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetInsertTblIbDocRecvDtlTemp(objInboundInquiry);
        }
        public void InsertTempFileDocument(InboundInquiry objInboundInquiry)
        {
            objRepository.InsertTempFileDocument(objInboundInquiry);
        }
        public InboundInquiry GetTempFiledtl(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetTempFiledtl(objInboundInquiry);
        }
        public InboundInquiry GetDocRecvEntrySave(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetDocRecvEntrySave(objInboundInquiry);
        }
        public InboundInquiry GetRecvTempTableCount(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetRecvTempTableCount(objInboundInquiry);
        }
        public InboundInquiry GetDocEntryCount(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetDocEntryCount(objInboundInquiry);
        }
        public InboundInquiry Getuploaddelete(InboundInquiry objInboundInquiry)
        {
            return objRepository.Getuploaddelete(objInboundInquiry);
        }
        public InboundInquiry GetReceivingdelete(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetReceivingdelete(objInboundInquiry);
        }
        public InboundInquiry GetLoadReceivingdelete(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetLoadReceivingdelete(objInboundInquiry);
        }
        public InboundInquiry GetUpdateRcvdStatus(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetUpdateRcvdStatus(objInboundInquiry);
        }
        public InboundInquiry GetDocRecvEntrySaveByItem(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetDocRecvEntrySaveByItem(objInboundInquiry);
        }
        public InboundInquiry GetDocRecvEntrySaveByLotID(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetDocRecvEntrySaveByLotID(objInboundInquiry);
        }
        public InboundInquiry GetDocRecvEntrySaveByPo(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetDocRecvEntrySaveByPo(objInboundInquiry);
        }
        public InboundInquiry GetDocEditDtl(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetDocEditDtl(objInboundInquiry);
        }
        public InboundInquiry GetEditGridData(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetEditGridData(objInboundInquiry);
        }
        public InboundInquiry GetDeleteTempData(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetDeleteTempData(objInboundInquiry);
        }
        public InboundInquiry UpdateTblIbDocHdr(InboundInquiry objInboundInquiry)
        {
            return objRepository.UpdateTblIbDocHdr(objInboundInquiry);
        }
        public InboundInquiry GetDeleteDocCtn(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetDeleteDocCtn(objInboundInquiry);
        }
        public InboundInquiry UpdateTblIbDocDtl(InboundInquiry objInboundInquiry)
        {
            return objRepository.UpdateTblIbDocDtl(objInboundInquiry);
        }
        public InboundInquiry GetPaletIdValidation(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetPaletIdValidation(objInboundInquiry);
        }
        public InboundInquiry Gettranstaus(InboundInquiry objInboundInquiry)
        {
            return objRepository.Gettranstaus(objInboundInquiry);
        }
        public InboundInquiry Getlotdtltext(InboundInquiry objInboundInquiry)
        {
            return objRepository.Getlotdtltext(objInboundInquiry);
        }
        public InboundInquiry GetLotHdr(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetLotHdr(objInboundInquiry);
        }
        public InboundInquiry GetGridlotdtl(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetGridlotdtl(objInboundInquiry);
        }
        public InboundInquiry GetDftWhs(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetDftWhs(objInboundInquiry);
        }

        public InboundInquiry Del_Doc_qty_Mod(InboundInquiry objInboundInquiry)
        {
            return objRepository.Del_Doc_qty_Mod(objInboundInquiry);
        }
        public InboundInquiry Del_iv_itm_trn(InboundInquiry objInboundInquiry)
        {
            return objRepository.Del_iv_itm_trn(objInboundInquiry);
        }
        public InboundInquiry Update_Doc_tbl(InboundInquiry objInboundInquiry)
        {
            return objRepository.Update_Doc_tbl(objInboundInquiry);
        }
        public InboundInquiry Update_Doc_ctn(InboundInquiry objInboundInquiry)
        {
            return objRepository.Update_Doc_ctn(objInboundInquiry);
        }
        public InboundInquiry Add_To_Itm_Trn_in_CtnQty(InboundInquiry objInboundInquiry)
        {
            return objRepository.Add_To_Itm_Trn_in_CtnQty(objInboundInquiry);
        }
        public InboundInquiry Add_Iv_Lot_Dtl(InboundInquiry objInboundInquiry)
        {
            return objRepository.Add_Iv_Lot_Dtl(objInboundInquiry);
        }
        public InboundInquiry Add_tbl_iv_lot_ctn(InboundInquiry objInboundInquiry)
        {
            return objRepository.Add_tbl_iv_lot_ctn(objInboundInquiry);
        }
        public InboundInquiry GetKitQty(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetKitQty(objInboundInquiry);
        }
        public InboundInquiry CkIORate(InboundInquiry objInboundInquiry)
        {
            return objRepository.CkIORate(objInboundInquiry);
        }
        public InboundInquiry CkSTRate(InboundInquiry objInboundInquiry)
        {
            return objRepository.CkSTRate(objInboundInquiry);
        }
        public InboundInquiry GetGridRecvEditData(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetGridRecvEditData(objInboundInquiry);
        }
        public InboundInquiry InsertTblIbDocRecvDtlTemp(InboundInquiry objInboundInquiry)
        {
            return objRepository.InsertTblIbDocRecvDtlTemp(objInboundInquiry);
        }
        public InboundInquiry GetItmName(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetItmName(objInboundInquiry);
        }
        public InboundInquiry GetItmCode(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetItmCode(objInboundInquiry);
        }
        public InboundInquiry UpdtItmDtl(InboundInquiry objInboundInquiry)
        {
            return objRepository.UpdtItmDtl(objInboundInquiry);
        }
        public InboundInquiry GetCheckExistGridDataRecvEntry(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetCheckExistGridDataRecvEntry(objInboundInquiry);
        }
        public InboundInquiry InsertRecvEntryTemptable(InboundInquiry objInboundInquiry)
        {
            return objRepository.InsertRecvEntryTemptable(objInboundInquiry);
        }
        public InboundInquiry GetRecvEntryCount(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetRecvEntryCount(objInboundInquiry);
        }
        public InboundInquiry GetRecvdtlGrid(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetRecvdtlGrid(objInboundInquiry);
        }
        public InboundInquiry GetRecvEntryGridDeleteData(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetRecvEntryGridDeleteData(objInboundInquiry);
        }
        public InboundInquiry SumTotqty(InboundInquiry objInboundInquiry)
        {
            return objRepository.SumTotqty(objInboundInquiry);
        }
        public InboundInquiry GetLotIDValidation(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetLotIDValidation(objInboundInquiry);
        }
        public InboundInquiry GetCntrValidation(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetCntrValidation(objInboundInquiry);
        }
        //Added By Ravi 17-02-2018
        public InboundInquiry GetContainerandRateID(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetContainerandRateID(objInboundInquiry);
        }
        public InboundInquiry InsertCONTAINERRecvDetails(InboundInquiry objInboundInquiry)
        {
            return objRepository.InsertCONTAINERRecvDetails(objInboundInquiry);
        }
        //END  
        //CR_3PL_MVC_BL_2018_0221_001 Added By Ravi 21-02-2018
        public void Del_rcv_dtl(InboundInquiry objInboundInquiry)
        {
            objRepository.Del_rcv_dtl(objInboundInquiry);
        }
        //END

        //CR_3PL_MVC_IB_2018_0228_001 Added By Ravi 21-02-2018
     
        public InboundInquiry AddLocId(InboundInquiry objInboundInquiry)
        {
            return objRepository.AddLocId(objInboundInquiry);
        }
        //END
        //  CR-3PL_MVC_IB_2018_0312_002

        public InboundInquiry GetRcvdDtlCount(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetRcvdDtlCount(objInboundInquiry);
        }
        //END CR-3PL_MVC_IB_2018_0312_002 
        public InboundInquiry GetIBRecvDeleteTempData(InboundInquiry objInboundInquiry)      //  CR-3PL_MVC_IB_2018_0313_002
        {
            return objRepository.GetIBRecvDeleteTempData(objInboundInquiry);
        }
        public InboundInquiry GetItemRcvdQty(InboundInquiry objInboundInquiry)      //  CR-3PL_MVC_IB_2018_0313_002
        {
            return objRepository.GetItemRcvdQty(objInboundInquiry);
        }
        public InboundInquiry GetCtnLineNo(InboundInquiry objInboundInquiry)      //  CR-3PL_MVC_IB_2018_0313_002
        {
            return objRepository.GetCtnLineNo(objInboundInquiry);
        }
        public InboundInquiry Update_Lot_Bill_Status(InboundInquiry objInboundInquiry)      //  CR-3PL_MVC_IB_2018_0313_002
        {
            return objRepository.Update_Lot_Bill_Status(objInboundInquiry);
        }
        public InboundInquiry Check_Exist_Container_Id(InboundInquiry objInboundInquiry)      //  CR-3PL_MVC_IB_2018_0313_002
        {
            return objRepository.Check_Exist_Container_Id(objInboundInquiry);
        }
      public InboundInquiry CheckItemExist(InboundInquiry objInboundInquiry)      //  CR-3PL_MVC_IB_2018_0313_002
        {
            return objRepository.CheckItemExist(objInboundInquiry);
        }
        public InboundInquiry Validate_Itm(InboundInquiry objInboundInquiry)      //  CR-3PL_MVC_IB_2018_0313_002
        {
            return objRepository.Validate_Itm(objInboundInquiry); //  CR-3PL_MVC_IB_2018_0410_001 Added By Nithya
        }

        public InboundInquiry ItemXGetIBitmDetails(string term, string cmp_id)
        {
            return objRepository.ItemXGetIBitmDetails(term, cmp_id);
        }
        public InboundInquiry GetRcvdEntryCountDtl(InboundInquiry objInboundInquiry)      //  CR-3PL_MVC_IB_2018_0313_002
        {
            return objRepository.GetRcvdEntryCountDtl(objInboundInquiry); //  CR-3PL_MVC_IB_2018_0410_001 Added By Nithya
        }
        public InboundInquiry GetTotal(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetTotal(objInboundInquiry); 
        }
        public InboundInquiry InsertdocEditEntry(InboundInquiry objInboundInquiry)
        {
            return objRepository.InsertdocEditEntry(objInboundInquiry);
        }        
        public InboundInquiry GetdocEditCount(InboundInquiry objInboundInquiry)    
        {
            return objRepository.GetdocEditCount(objInboundInquiry); 
        }
        public InboundInquiry GetrecvEditTotQty(InboundInquiry objInboundInquiry)
        {
            return objRepository.GetrecvEditTotQty(objInboundInquiry);
        }

        public InboundInquiry check_ib_doc_in_use(InboundInquiry objInboundInquiry)
        {
            return objRepository.check_ib_doc_in_use(objInboundInquiry);
        }
        public InboundInquiry Add_To_proc_save_audit_trail(InboundInquiry objInboundInquiry)//  CR-3PL_MVC_IB_2018_0813_001 Added By Nithya
        {
            return objRepository.Add_To_proc_save_audit_trail(objInboundInquiry);
        }
        public InboundInquiry CHECKDOCDATE(InboundInquiry objInboundInquiry)//  CR-3PL_MVC_IB_2018_0813_001 Added By Nithya
        {
            return objRepository.CHECKDOCDATE(objInboundInquiry);
        }
        public InboundInquiry GET_IB_DOC_CUBE_AND_WGT(InboundInquiry objInboundInquiry)//  CR-3PL_MVC_IB_2018_0813_001 Added By Nithya
        {
            return objRepository.GET_IB_DOC_CUBE_AND_WGT(objInboundInquiry);
        }
        public InboundInquiry GET_IB_RCVD_DOC_CUBE_AND_WGT(InboundInquiry objInboundInquiry)//  CR-3PL_MVC_IB_2018_0813_001 Added By Nithya
        {
            return objRepository.GET_IB_RCVD_DOC_CUBE_AND_WGT(objInboundInquiry);
        }
        public InboundInquiry GET_IBS_DTL_FROM_RATE_MASTER(InboundInquiry objInboundInquiry)
        {
            return objRepository.GET_IBS_DTL_FROM_RATE_MASTER(objInboundInquiry);
        }
        public void INSERT_IBS_DTL_TEMP_TBL(InboundInquiry objInboundInquiry)
        {
             objRepository.INSERT_IBS_DTL_TEMP_TBL(objInboundInquiry);
        }
        public InboundInquiry GET_IBS_DTL_TEMP_TBL(InboundInquiry objInboundInquiry)
        {
            return objRepository.GET_IBS_DTL_TEMP_TBL(objInboundInquiry);
        }
        public  void DELETE_IBS_DTL_TEMP_TBL(InboundInquiry objInboundInquiry)
        {
             objRepository.DELETE_IBS_DTL_TEMP_TBL(objInboundInquiry);
        }
        public void INSERT_IBS_DTL(InboundInquiry objInboundInquiry)
        {
             objRepository.INSERT_IBS_DTL(objInboundInquiry);
        }
        public void UPDATE_IBS_DTL(InboundInquiry objInboundInquiry)
        {
             objRepository.UPDATE_IBS_DTL(objInboundInquiry);
        }
        public void DELETE_IBS_DTL(InboundInquiry objInboundInquiry)
        {
             objRepository.DELETE_IBS_DTL(objInboundInquiry);
        }
        public InboundInquiry GET_IBS_DTL(InboundInquiry objInboundInquiry)
        {
            return objRepository.GET_IBS_DTL(objInboundInquiry);
        }
        public InboundInquiry GET_IBS_DOC_ID(InboundInquiry objInboundInquiry)
        {
            return objRepository.GET_IBS_DOC_ID(objInboundInquiry);
        }

        public IBDocExcp GetIBDocExcpList(IBDocExcp objIBDocExcp, string p_str_cmp_id)
        {
            return objRepository.GetIBDocExcpList(objIBDocExcp, p_str_cmp_id);
        }

        public DataTable GetIBDocExceptionList(string p_str_cmp_id)
        {
            return objRepository.GetIBDocExceptionList( p_str_cmp_id);
        }

        public DataTable getXlsIBStkSmryByLocList(string p_str_cmp_id,string p_str_ib_doc_id)
        {
            return objRepository.getXlsIBStkSmryByLocList(p_str_cmp_id,  p_str_ib_doc_id);
        }

        public bool DeleteIOBillByIbDocId(string p_str_cmp_id, string p_str_bill_doc_id, string p_str_lot_id)
        { 
            return objRepository.DeleteIOBillByIbDocId(p_str_cmp_id, p_str_bill_doc_id, p_str_lot_id);
        }
        public DataTable GetIBCheckIbDocValidateItem(string p_str_cmp_id, string p_str_ib_doc_id)
        {
            return objRepository.GetIBCheckIbDocValidateItem( p_str_cmp_id,  p_str_ib_doc_id);
        }
        public DataTable GetInboundExceptionList(String l_str_cmp_id)
        {
            return objRepository.GetInboundExceptionList( l_str_cmp_id);
        }

        public bool UpdateLotContainer(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_cntr_id, string p_str_cntr_type, string p_str_rcvd_dt,string p_str_ib_load_dt)
        {

            return objRepository.UpdateLotContainer(p_str_cmp_id, p_str_ib_doc_id, p_str_cntr_id, p_str_cntr_type, p_str_rcvd_dt, p_str_ib_load_dt);
        }

        public DataTable GetIBCheckIbDocRcvdCubeCheck(string p_str_cmp_id, string p_str_ib_doc_id)
        {
            return objRepository.GetIBCheckIbDocRcvdCubeCheck(p_str_cmp_id, p_str_ib_doc_id);
        }

        public DataTable GetInboundAckExcelTemplate(string l_str_cmp_id, string l_str_doc_id)
        {
            return objRepository.GetInboundAckExcelTemplate(l_str_cmp_id, l_str_doc_id);
        }

        public DataTable fnIBGetInboundAckRpt(string pstrCmpdId, string pstrFileName, string pstrUploadRefNum)
        {
            return objRepository.fnIBGetInboundAckRpt(pstrCmpdId, pstrFileName, pstrUploadRefNum);
        }
        public DataTable GetInboundGridSummaryExcelTemplate(string l_str_cmp_id, string l_str_doc_id, string l_str_cntr_id, string l_str_status, string l_str_ib_doc_dt_fm, string l_str_ib_doc_dt_to, string l_str_req_num, string l_str_eta_dt_fm, string l_str_eta_dt_to, string l_str_itm_num, string l_str_itm_color, string l_str_itm_size)
        {
            return objRepository.GetInboundGridSummaryExcelTemplate(l_str_cmp_id, l_str_doc_id, l_str_cntr_id, l_str_status, l_str_ib_doc_dt_fm, l_str_ib_doc_dt_to, l_str_req_num, l_str_eta_dt_fm, l_str_eta_dt_to, l_str_itm_num, l_str_itm_color, l_str_itm_size);
        }
        public DataTable GetInboundWorkSheetExcelTemplate(string l_str_cmp_id, string l_str_ib_doc_id)
        {
            return objRepository.GetInboundWorkSheetExcelTemplate(l_str_cmp_id, l_str_ib_doc_id);
        }
        public DataTable GetInboundContainerExcelTemplate(string l_str_cmp_id, string l_str_ib_doc_id, string l_str_rate_id)
        {
            return objRepository.GetInboundContainerExcelTemplate(l_str_cmp_id, l_str_ib_doc_id, l_str_rate_id);
        }

        public bool AddAutoSpecialIBSEntry(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_cntr_id)
        {
            return objRepository.AddAutoSpecialIBSEntry(p_str_cmp_id, p_str_ib_doc_id, p_str_cntr_id);
        }
        public bool AddAutoIBSEntry(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_cntr_id )
        {
            return objRepository.AddAutoIBSEntry(p_str_cmp_id, p_str_ib_doc_id, p_str_cntr_id);
        }
        public string checkIBDocInUse(string p_str_cmp_id, string p_str_ib_doc_id)
        {
            return objRepository.checkIBDocInUse( p_str_cmp_id,  p_str_ib_doc_id);
        }
        public bool checkIBDocRcvd(string pstrCmpId, string pstrIbDocId)
        {
            return objRepository.checkIBDocRcvd(pstrCmpId, pstrIbDocId);
        }

        public string fnIBRecvDtlEditTempSave(string pstrCmpId, DataTable dtIBRrecvDtlEditTemp)
        {
            return objRepository.fnIBRecvDtlEditTempSave(pstrCmpId, dtIBRrecvDtlEditTemp);
        }

       
    }
}

