using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Interface
{
   public interface IOBAllocationService
    {
         bool SaveOBAlocEditHdr(string pstrCmpId, string pstrAlocDocId, string pstrAlocDocDt, string pstrSrNum, string pstrShipDt,
          string pstrCancelDt, string pstrPriceTkt, string pstrCustId, string pstrCustName, string pstrCustOrdrNum, string pstrCustOrdrDt, string pstrNote);
        int getAlocEditTmpRefNum();
        List<clsOBAlocHdr> getOBAlocHdr(string pstrCmpId, string pstrAlocDocId);
        List<clsOBAlocSmry> getOBAlocDtl(string pstrCmpId, string pstrAlocDocId);
        List<ClsOBSRAlocDtl> getSRAlocDtl(string pstrCmpId, string pstrSrNo);
        List<clsOBAlocCtn> getOBAlocCtn(string pstrCmpId, string pstrAlocDocId);
        List<clsOBAlocCtnByLine> getOBAlocCtnByLine(string pstrCmpId, string pstrAlocDocId, int pstrAlocLine);
        string saveAlocCtnEditByLine(string pstrRefNum,string pstrCmpId, string pstrAlocDocId, string pstrAlocDocDt, int pintLineNum, DataTable pdtAlocCtnList);
        List<ClsOBSRAlocDtlByLoc> getSRAlocDtlByLoc(string @pstrRefNum, string pstrCmpId, string pstrSrNo);
        ClsOBAllocation GetOBSRSummary(ClsOBAllocation objOBAllocation);
        bool SaveAloctionByBatch(string pstrCmpId, string pstrRefNum, string pstrSrNo, string pstrUserId);
        DataTable getOBSRAlocStatus(string pstrCmpId, string pstrRefNum, string pstrUserId, ref DataTable dtUserEmail);
         DataTable getOBSRAlocStatus(string pstrCmpId, string pstrRefNum);
         PickLabel fnGetOBPickLabel(string pstrCmpId, string pstrRefNum);
        PickLabel fnGetOBDocForPrint(string pstrCmpId, string pstrRefNum);
  
        PickLabel fnGetOBPickLabelSmry(PickLabel pobjPickLabel);
        PickLabel fnGetOBSRListForBinprint(string pstrCmpId, string pstrSrList);
        PickLabel fnGetOBDocBySrList(string pstrCmpId, string pstrSrList);
    }
}
