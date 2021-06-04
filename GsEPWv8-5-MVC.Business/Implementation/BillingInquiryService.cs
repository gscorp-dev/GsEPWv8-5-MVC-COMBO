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
    public class BillingInquiryService : IBillingInquiryService
    {
        public BillingInquiry GetBillingInquiryDetails(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillingInquiryDetails(objBillingInquiry);
        }
        BillingInquiryRepository objRepository = new BillingInquiryRepository();
        public BillingInquiry GetBillingSummaryRpt(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillingSummaryRpt(objBillingInquiry);
        }
        public BillingInquiry GetBillingBillConsolidateRpt(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillingBillConsolidateRpt(objBillingInquiry);
        }
        public BillingInquiry GetBillingInvoiceRpt(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillingInvoiceRpt(objBillingInquiry);
        }
        public BillingInquiry GetBillingBillingType(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillingBillingType(objBillingInquiry);
        }
        public BillingInquiry GetBillingBillDocIdType(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillingBillDocIdType(objBillingInquiry);
        }
        public BillingInquiry CheckExsistBLDocIDFromVasHdr(BillingInquiry objBillingInquiry)
        {
            return objRepository.CheckExsistBLDocIDFromVasHdr(objBillingInquiry);
        }
        public BillingInquiry GetBillingBillDocVASRpt(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillingBillDocVASRpt(objBillingInquiry);
        }
        public BillingInquiry GetBillingBillInoutCartonRpt(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillingBillInoutCartonRpt(objBillingInquiry);
        }
        public BillingInquiry GetBillingBillInoutCartonInstrgRpt(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillingBillInoutCartonInstrgRpt(objBillingInquiry);
        }
        public BillingInquiry GetBillingBillInoutCubeRpt(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillingBillInoutCubeRpt(objBillingInquiry);
        }
        public BillingInquiry GetBillingBillInoutCubeInstrgRpt(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillingBillInoutCubeInstrgRpt(objBillingInquiry);
        }
        public BillingInquiry GetBillingInoutType(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillingInoutType(objBillingInquiry);
        }
        public BillingInquiry GetBillingBillDocSTRGRpt(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillingBillDocSTRGRpt(objBillingInquiry);
        }

        public BillingInquiry GetBillingBillDocCubeSTRGRpt(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillingBillDocCubeSTRGRpt(objBillingInquiry);
        }
        public BillingInquiry GetBillingHdr(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillingHdr(objBillingInquiry);
        }
        public BillingInquiry GetBillingdtl(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillingdtl(objBillingInquiry);
        }
        public BillingInquiry GetBillingInvStaus(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillingInvStaus(objBillingInquiry);
        }
        public BillingInquiry GetBillingRcvdDetails(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillingRcvdDetails(objBillingInquiry);
        }
        public BillingInquiry CheckSTRGBillDocIdExisting(BillingInquiry objBillingInquiry)
        {
            return objRepository.CheckSTRGBillDocIdExisting(objBillingInquiry);
        }
        public BillingInquiry DeleteExistingSTRGBillDocIdData(BillingInquiry objBillingInquiry)
        {
            return objRepository.DeleteExistingSTRGBillDocIdData(objBillingInquiry);
        }
        public BillingInquiry GenerateSTRGBill(BillingInquiry objBillingInquiry)
        {
            return objRepository.GenerateSTRGBill(objBillingInquiry);
        }
        public BillingInquiry GenerateSTRGBillByCube(BillingInquiry objBillingInquiry)
        {
            return objRepository.GenerateSTRGBillByCube(objBillingInquiry);
        }
        public BillingInquiry CheckInoutBillDocIdExisting(BillingInquiry objBillingInquiry)
        {
            return objRepository.CheckInoutBillDocIdExisting(objBillingInquiry);
        }
        public BillingInquiry GenerateInOutBillByCarton(BillingInquiry objBillingInquiry)
        {
            return objRepository.GenerateInOutBillByCarton(objBillingInquiry);
        }
        public BillingInquiry GenerateInOutBillByCube(BillingInquiry objBillingInquiry)
        {
            return objRepository.GenerateInOutBillByCube(objBillingInquiry);
        }
        public BillingInquiry GenerateVASBill(BillingInquiry objBillingInquiry)
        {
            return objRepository.GenerateVASBill(objBillingInquiry);
        }
        public BillingInquiry CheckVASBillDocIdExisting(BillingInquiry objBillingInquiry)
        {
            return objRepository.CheckVASBillDocIdExisting(objBillingInquiry);
        }
        public BillingInquiry GetBillingVasBillDetails(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillingVasBillDetails(objBillingInquiry);
        }

        public BillingInquiry GetBillDelete(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillDelete(objBillingInquiry);
        }
        public BillingInquiry GetBillPost(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillPost(objBillingInquiry);
        }
        public BillingInquiry GetStrgBillExcel(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetStrgBillExcel(objBillingInquiry);
        }
        public BillingInquiry GetStrgBillCubeExcel(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetStrgBillCubeExcel(objBillingInquiry);
        }
        public BillingInquiry GetNormBillExcel(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetNormBillExcel(objBillingInquiry);
        }
        public BillingInquiry GetInOutBillCube(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetInOutBillCube(objBillingInquiry);
        }
        public BillingInquiry GetInOutBillCarton(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetInOutBillCarton(objBillingInquiry);
        }
        public BillingInquiry GenerateInoutBillByContainer(BillingInquiry objBillingInquiry)
        {
            return objRepository.GenerateInoutBillByContainer(objBillingInquiry);
        }
        public BillingInquiry GenerateSTRGBillByPallet(BillingInquiry objBillingInquiry)
        {
            return objRepository.GenerateSTRGBillByPallet(objBillingInquiry);
        }
        public BillingInquiry GetBillingBillDocPalletSTRGRpt(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillingBillDocPalletSTRGRpt(objBillingInquiry);
        }
        public BillingInquiry GetBillingBillDocContainerInoutRpt(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillingBillDocContainerInoutRpt(objBillingInquiry);
        }
        // CR-3PL_MVC_IB_2018_0219_004
        public BillingInquiry CheckExsistBLDocID(BillingInquiry objBillingInquiry)
        {
            return objRepository.CheckExsistBLDocID(objBillingInquiry);
        }
        // CR-3PL_MVC_IB_2018_0219_004
        // CR-3PL_MVC_IB_2018_0219_004
        public BillingInquiry GetDocRcvdDate(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetDocRcvdDate(objBillingInquiry);
        }
        // CR-3PL_MVC_IB_2018_0219_004
        // CR-3PL_MVC_IB_2018_0219_004
        public BillingInquiry CheckExsistBLDocIDFromLotHdr(BillingInquiry objBillingInquiry)
        {
            return objRepository.CheckExsistBLDocIDFromLotHdr(objBillingInquiry);
        }
        // CR-3PL_MVC_IB_2018_0219_004
        public BillingInquiry GetSecondSTRGRate(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetSecondSTRGRate(objBillingInquiry);
        }
        public BillingInquiry GetBillDeleteByPallet(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillDeleteByPallet(objBillingInquiry);
        }
        public BillingInquiry GetSTRGBillRcvdDtlByPallet(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetSTRGBillRcvdDtlByPallet(objBillingInquiry);
        }
        public BillingInquiry GetSTRGBillRcvdDtlByContainer(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetSTRGBillRcvdDtlByContainer(objBillingInquiry);
        }
        public BillingInquiry GetBillingInoutRcvdDetails(BillingInquiry objBillingInquiry)//CR-3PL_MVC_IB_2018-03-10-001
        {
            return objRepository.GetBillingInoutRcvdDetails(objBillingInquiry);
        }
        public BillingInquiry GetBillingStrgRcvdDetails(BillingInquiry objBillingInquiry)//CR-3PL_MVC_IB_2018-03-10-001
        {
            return objRepository.GetBillingStrgRcvdDetails(objBillingInquiry);
        }
        public BillingInquiry GenerateSTRGBillCarton(BillingInquiry objBillingInquiry)//CR-3PL_MVC_BL_2018_00312_001      
        {
            return objRepository.GenerateSTRGBillCarton(objBillingInquiry);
        }
        public BillingInquiry GenerateSTRGBillCube(BillingInquiry objBillingInquiry)//CR-3PL_MVC_BL_2018_00312_001      
        {
            return objRepository.GenerateSTRGBillCube(objBillingInquiry);
        }
        public BillingInquiry GenerateSTRGBillByLoc(BillingInquiry objBillingInquiry)//CR-3PL_MVC_BL_2018_00312_001      
        {
            return objRepository.GenerateSTRGBillByLoc(objBillingInquiry);
        }
        public BillingInquiry STRGBillLocationRpt(BillingInquiry objBillingInquiry)//CR-3PL_MVC_BL_2018_00312_001      
        {
            return objRepository.STRGBillLocationRpt(objBillingInquiry);
        }
        public BillingInquiry GetVASBillDelete(BillingInquiry objBillingInquiry)   //CR-2018-05-02-001 Added By Nithya 
        {
            return objRepository.GetVASBillDelete(objBillingInquiry);
        }
        public BillingInquiry STRGBillPcsRpt(BillingInquiry objBillingInquiry) //CR-2018-05-21-001 Added By Nithya
        {
            return objRepository.STRGBillPcsRpt(objBillingInquiry);
        }
        public BillingInquiry GenerateSTRGBillByPcs(BillingInquiry objBillingInquiry)//CR-2018-05-21-001 Added By Nithya   
        {
            return objRepository.GenerateSTRGBillByPcs(objBillingInquiry);
        }

        public BillingInquiry GetBillingVasBillInqDetails(BillingInquiry objBillingInquiry)//CR-2018-05-21-001 Added By Nithya   
        {
            return objRepository.GetBillingVasBillInqDetails(objBillingInquiry);
        }
        public BillingInquiry UpdateVASHDR(BillingInquiry objBillingInquiry)//CR-2018-05-21-001 Added By Nithya   
        {
            return objRepository.UpdateVASHDR(objBillingInquiry);
        }
        public BillingInquiry GetBillingiNOUTBillInqDetails(BillingInquiry objBillingInquiry)//CR-2018-05-21-001 Added By Nithya   
        {
            return objRepository.GetBillingiNOUTBillInqDetails(objBillingInquiry);
        }
        public BillingInquiry GetBillingBillamountInoutCartonRpt(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetBillingBillamountInoutCartonRpt(objBillingInquiry);
        }
        public BillingInquiry GetConsolidateVASInqDetails(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetConsolidateVASInqDetails(objBillingInquiry);
        }
        public BillingInquiry GetConsolidateVASBillEntityValue(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetConsolidateVASBillEntityValue(objBillingInquiry);
        }
        public BillingInquiry SaveConsolidateBillDetails(BillingInquiry objBillingInquiry)
        {
            return objRepository.SaveConsolidateBillDetails(objBillingInquiry);
        }
        public BillingInquiry GetConsolidateStorageBillDetails(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetConsolidateStorageBillDetails(objBillingInquiry);
        }
        public BillingInquiry GetConsolidateInoutBillDetails(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetConsolidateInoutBillDetails(objBillingInquiry);
        }

        public BillingInquiry GetRegenerateInoutBillDetails(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetRegenerateInoutBillDetails(objBillingInquiry);
        }
        public BillingInquiry SaveConsolidateInoutBillDetails(BillingInquiry objBillingInquiry)
        {
            return objRepository.SaveConsolidateInoutBillDetails(objBillingInquiry);
        }
        public BillingInquiry ConsolidateInoutBillSummaryRpt(BillingInquiry objBillingInquiry)
        {
            return objRepository.ConsolidateInoutBillSummaryRpt(objBillingInquiry);
        }
        public BillingInquiry ConsolidateInoutBillDetailRpt(BillingInquiry objBillingInquiry)
        {
            return objRepository.ConsolidateInoutBillDetailRpt(objBillingInquiry);
        }
        public BillingInquiry ConsolidateStorageBillSummaryRpt(BillingInquiry objBillingInquiry)
        {
            return objRepository.ConsolidateStorageBillSummaryRpt(objBillingInquiry);
        }
        public BillingInquiry GetConsolidateVASSummaryRpt(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetConsolidateVASSummaryRpt(objBillingInquiry);
        }
        public BillingInquiry GenerateVASBillDetailRpt(BillingInquiry objBillingInquiry)
        {
            return objRepository.GenerateVASBillDetailRpt(objBillingInquiry);
        }
        public BillingInquiry GetConsolidtedVASBillRpt(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetConsolidtedVASBillRpt(objBillingInquiry);
        }
        public BillingInquiry GetInvoiceRpt(BillingInquiry objBillingInquiry)
        {
            return objRepository.GetInvoiceRpt(objBillingInquiry);
        }

        public string CkBillDocAlreadyExists(string p_str_cust_id, string p_str_bill_type, string p_str_bill_from_dt, string p_str_bill_to_dt)
        {
            return objRepository.CkBillDocAlreadyExists(p_str_cust_id, p_str_bill_type, p_str_bill_from_dt, p_str_bill_to_dt);
        }
        public DataTable GetBillByCubeList(string p_str_cmp_id, string p_str_bill_doc_id)
        {
            return objRepository.GetBillByCubeList(p_str_cmp_id, p_str_bill_doc_id);
        }
        public DataTable GetBillDocVASRpt(string p_str_cmp_id, string p_str_bill_doc_id, ref DataTable dtBlHdr)
        {
            return objRepository.GetBillDocVASRpt(p_str_cmp_id, p_str_bill_doc_id, ref dtBlHdr);
        }
        public DataTable GetBillDocInoutRpt(string p_str_cmp_id, string p_str_bill_doc_id, ref DataTable dtBlHdr)
        {
            return objRepository.GetBillDocInoutRpt(p_str_cmp_id, p_str_bill_doc_id, ref dtBlHdr);
        }
        public string GetBillType(string p_str_bill_doc_id)
        {
            return objRepository.GetBillType(p_str_bill_doc_id);
        }

        public string GetBillBy(string p_str_cust_id, string p_str_bill_type)
        {
            return objRepository.GetBillBy( p_str_cust_id,  p_str_bill_type);
        }
        public DataTable GetConsolidtedVASBillList(string p_str_cmp_id, string p_str_bill_doc_id, string p_str_Bill_type, string p_str_doc_dt_Fr, string p_str_doc_dt_To)
        {
            return objRepository.GetConsolidtedVASBillList(p_str_cmp_id, p_str_bill_doc_id, p_str_Bill_type, p_str_doc_dt_Fr, p_str_doc_dt_To);
        }

        public DataTable GetConsolidtedINOUTBillList(string p_str_cmp_id, string p_str_bill_doc_id, string p_str_Bill_type, string p_str_doc_dt_Fr, string p_str_doc_dt_To)
        {
            return objRepository.GetConsolidtedINOUTBillList(p_str_cmp_id, p_str_bill_doc_id, p_str_Bill_type, p_str_doc_dt_Fr, p_str_doc_dt_To);
        }
        public DataTable GetGridSummaryBillList(string p_str_cmp_id, string p_str_bill_doc_id, string p_str_Bill_type, string p_str_doc_dt_Fr, string p_str_doc_dt_To)
        {
            return objRepository.GetGridSummaryBillList(p_str_cmp_id, p_str_bill_doc_id, p_str_Bill_type, p_str_doc_dt_Fr, p_str_doc_dt_To);
        }
        public DataTable GetBillDocStrgRpt(string p_str_cust_id, string p_str_bill_doc_id, ref DataTable dtBlHdr)
        {
            return objRepository.GetBillDocStrgRpt(p_str_cust_id, p_str_bill_doc_id, ref  dtBlHdr);
        }
        public DataTable fnGetStrgBillByItmSmry(string p_str_cust_id, string p_str_bill_doc_id, ref DataTable dtBlHdr)
        {
            return objRepository.fnGetStrgBillByItmSmry(p_str_cust_id, p_str_bill_doc_id, ref dtBlHdr);
        }


        public DataTable GetBillInvoiceInoutRpt(string p_str_cmp_id, string p_str_bill_doc_id)
        {
            return objRepository.GetBillInvoiceInoutRpt(p_str_cmp_id, p_str_bill_doc_id);
        }
        public DataTable GetBillInvoiceStorageRpt(string p_str_cmp_id, string p_str_bill_doc_id)
        {
            return objRepository.GetBillInvoiceStorageRpt(p_str_cmp_id, p_str_bill_doc_id);
        }
        public DataTable GetBillInvoiceVASRpt(string p_str_cmp_id, string p_str_bill_doc_id)
        {
            return objRepository.GetBillInvoiceVASRpt(p_str_cmp_id, p_str_bill_doc_id);
        }
        public List<ClsBillVAS> getVASBillRegenerate(string pstrCmpId, string pstrBillDocId, string pdtBillFromDt, string pdtBillToDt)
        {
            return objRepository.getVASBillRegenerate(pstrCmpId,  pstrBillDocId,  pdtBillFromDt,  pdtBillToDt);
        }
        public bool DeleteVASBill(string p_str_bill_cmp_id, string p_str_cust_id, string p_str_bill_doc_id)
        {
            return objRepository.DeleteVASBill( p_str_bill_cmp_id, p_str_cust_id,  p_str_bill_doc_id);
        }
       public DataTable fnGetCtnsByBillDoc(string pstrBillDocId)
        {
            return objRepository.fnGetCtnsByBillDoc(pstrBillDocId);
        }
    }
}