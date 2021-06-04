using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface IBillingInquiryService
    {
        BillingInquiry GetBillingInquiryDetails(BillingInquiry objBillingInquiry);
        BillingInquiry GetConsolidateVASBillEntityValue(BillingInquiry objBillingInquiry);
        BillingInquiry GetConsolidateInoutBillDetails(BillingInquiry objBillingInquiry);
        BillingInquiry GetRegenerateInoutBillDetails(BillingInquiry objBillingInquiry);
        BillingInquiry SaveConsolidateBillDetails(BillingInquiry objBillingInquiry);
        BillingInquiry GetBillingSummaryRpt(BillingInquiry objLogin);
        BillingInquiry GetBillingInvoiceRpt(BillingInquiry objLogin);
        BillingInquiry GetBillingBillConsolidateRpt(BillingInquiry objLogin);
        BillingInquiry GetBillingBillingType(BillingInquiry objLogin);
        BillingInquiry GetBillingBillDocIdType(BillingInquiry objLogin);
        BillingInquiry GetBillingBillDocVASRpt(BillingInquiry objLogin);
        BillingInquiry GetBillingBillInoutCartonRpt(BillingInquiry objLogin);
        BillingInquiry GetBillingBillInoutCartonInstrgRpt(BillingInquiry objLogin);
        BillingInquiry GetBillingBillInoutCubeRpt(BillingInquiry objLogin);
        BillingInquiry GetBillingBillInoutCubeInstrgRpt(BillingInquiry objLogin);
        BillingInquiry GetBillingInoutType(BillingInquiry objLogin);
        BillingInquiry GetBillingBillDocSTRGRpt(BillingInquiry objLogin);
        BillingInquiry GetBillingBillDocCubeSTRGRpt(BillingInquiry objLogin);
        BillingInquiry GetBillingHdr(BillingInquiry objLogin);
        BillingInquiry GetBillingVasBillDetails(BillingInquiry objLogin);
        BillingInquiry GetBillingdtl(BillingInquiry objLogin);    
        BillingInquiry GetBillingInvStaus(BillingInquiry objLogin);
        BillingInquiry GetBillingRcvdDetails(BillingInquiry objLogin);
        BillingInquiry GenerateSTRGBill(BillingInquiry objLogin);
        BillingInquiry GenerateSTRGBillByCube(BillingInquiry objLogin);
        BillingInquiry CheckSTRGBillDocIdExisting(BillingInquiry objLogin);
        BillingInquiry CheckExsistBLDocIDFromVasHdr(BillingInquiry objLogin);
        BillingInquiry DeleteExistingSTRGBillDocIdData(BillingInquiry objLogin);
        BillingInquiry CheckInoutBillDocIdExisting(BillingInquiry objLogin);
        BillingInquiry GenerateInOutBillByCarton(BillingInquiry objLogin);
        BillingInquiry GenerateInOutBillByCube(BillingInquiry objLogin);
        BillingInquiry GenerateVASBill(BillingInquiry objLogin);
        BillingInquiry CheckVASBillDocIdExisting(BillingInquiry objLogin);
        BillingInquiry GetBillDelete(BillingInquiry objLogin);
        BillingInquiry GetBillPost(BillingInquiry objLogin);
        BillingInquiry GetStrgBillExcel(BillingInquiry objLogin);
        BillingInquiry GetStrgBillCubeExcel(BillingInquiry objLogin);
        BillingInquiry GetNormBillExcel(BillingInquiry objLogin);
        BillingInquiry GetInOutBillCube(BillingInquiry objLogin);
        BillingInquiry GetInOutBillCarton(BillingInquiry objLogin);
        BillingInquiry GenerateInoutBillByContainer(BillingInquiry objLogin);
        BillingInquiry GenerateSTRGBillByPallet(BillingInquiry objLogin);
        BillingInquiry GetBillingBillDocPalletSTRGRpt(BillingInquiry objLogin);
        BillingInquiry GetBillingBillDocContainerInoutRpt(BillingInquiry objLogin);
        BillingInquiry CheckExsistBLDocID(BillingInquiry objLogin);  // CR-3PL_MVC_IB_2018_0219_004
        BillingInquiry GetDocRcvdDate(BillingInquiry objLogin);  // CR-3PL_MVC_IB_2018_0219_004
        BillingInquiry CheckExsistBLDocIDFromLotHdr(BillingInquiry objLogin);  // CR-3PL_MVC_IB_2018_0219_004
        BillingInquiry GetSecondSTRGRate(BillingInquiry objLogin);  // CR-3PL_MVC_IB_2018_0219_004
        BillingInquiry GetBillDeleteByPallet(BillingInquiry objLogin);  // CR-3PL_MVC_IB_2018_0307_001
        BillingInquiry GetSTRGBillRcvdDtlByPallet(BillingInquiry objLogin);
        BillingInquiry GetSTRGBillRcvdDtlByContainer(BillingInquiry objLogin);
        BillingInquiry GetBillingInoutRcvdDetails(BillingInquiry objLogin);//CR-3PL_MVC_IB_2018-03-10-001
        BillingInquiry GetBillingStrgRcvdDetails(BillingInquiry objLogin);//CR-3PL_MVC_IB_2018-03-10-001
        BillingInquiry GenerateSTRGBillCarton(BillingInquiry objLogin);//CR-3PL_MVC_BL_2018_00312_001      
        BillingInquiry GenerateSTRGBillCube(BillingInquiry objLogin);//CR-3PL_MVC_BL_2018_00312_001         
        BillingInquiry GenerateSTRGBillByLoc(BillingInquiry objLogin);
        BillingInquiry STRGBillLocationRpt(BillingInquiry objLogin);
        BillingInquiry GetVASBillDelete(BillingInquiry objLogin); //CR-2018-05-02-001 Added By Nithya
        BillingInquiry STRGBillPcsRpt(BillingInquiry objLogin);//CR-2018-05-21-001 Added By Nithya
        BillingInquiry GenerateSTRGBillByPcs(BillingInquiry objLogin);//CR-2018-05-21-001 Added By Nithya
        BillingInquiry GetBillingVasBillInqDetails(BillingInquiry objLogin);
        BillingInquiry UpdateVASHDR(BillingInquiry objLogin);
        BillingInquiry GetBillingiNOUTBillInqDetails(BillingInquiry objLogin);
        BillingInquiry GetBillingBillamountInoutCartonRpt(BillingInquiry objLogin);
        BillingInquiry GetConsolidateVASInqDetails(BillingInquiry objLogin);
        BillingInquiry GetConsolidateStorageBillDetails(BillingInquiry objLogin);
        BillingInquiry SaveConsolidateInoutBillDetails(BillingInquiry objLogin);
        BillingInquiry ConsolidateInoutBillSummaryRpt(BillingInquiry objLogin);
        BillingInquiry ConsolidateInoutBillDetailRpt(BillingInquiry objLogin);
        BillingInquiry ConsolidateStorageBillSummaryRpt(BillingInquiry objLogin);
        BillingInquiry GetConsolidateVASSummaryRpt(BillingInquiry objLogin);
        BillingInquiry GenerateVASBillDetailRpt(BillingInquiry objLogin);
        BillingInquiry GetConsolidtedVASBillRpt(BillingInquiry objLogin);
        BillingInquiry GetInvoiceRpt(BillingInquiry objLogin);
        string CkBillDocAlreadyExists(string p_str_cust_id, string p_str_bill_type, string p_str_bill_from_dt, string p_str_bill_to_dt);
        DataTable GetBillByCubeList(string p_str_cmp_id, string p_str_bill_doc_id);
        string GetBillType(string p_str_bill_doc_id);
        string GetBillBy(string p_str_cust_id, string p_str_bill_type);
        DataTable GetGridSummaryBillList(string p_str_cmp_id, string p_str_bill_doc_id, string p_str_Bill_type, string p_str_doc_dt_Fr, string p_str_doc_dt_To);
        DataTable GetConsolidtedINOUTBillList(string p_str_cmp_id, string p_str_bill_doc_id, string p_str_Bill_type, string p_str_doc_dt_Fr, string p_str_doc_dt_To);
        DataTable GetBillDocInoutRpt(string p_str_cmp_id, string p_str_bill_doc_id, ref DataTable dtBlHdr);
        DataTable GetBillDocStrgRpt(string p_str_cust_id, string p_str_bill_doc_id, ref DataTable dtBlHdr);
        DataTable fnGetStrgBillByItmSmry(string p_str_cust_id, string p_str_bill_doc_id, ref DataTable dtBlHdr);
        DataTable GetBillDocVASRpt(string p_str_cmp_id, string p_str_bill_doc_id, ref DataTable dtBlHdr);
        DataTable GetConsolidtedVASBillList(string p_str_cmp_id, string p_str_bill_doc_id, string p_str_Bill_type, string p_str_doc_dt_Fr, string p_str_doc_dt_To);
        DataTable GetBillInvoiceInoutRpt(string p_str_cmp_id, string p_str_bill_doc_id);
        DataTable GetBillInvoiceStorageRpt(string p_str_cmp_id, string p_str_bill_doc_id);
        DataTable GetBillInvoiceVASRpt(string p_str_cmp_id, string p_str_bill_doc_id);
        List<ClsBillVAS> getVASBillRegenerate(string pstrCmpId, string pstrBillDocId, string pdtBillFromDt, string pdtBillToDt);
        bool DeleteVASBill(string p_str_bill_cmp_id, string p_str_cust_id, string p_str_bill_doc_id);
        DataTable fnGetCtnsByBillDoc(string pstrBillDocId);
    }
}
