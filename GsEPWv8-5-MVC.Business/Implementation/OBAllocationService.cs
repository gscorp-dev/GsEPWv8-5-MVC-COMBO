using GsEPWv8_5_MVC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Interface;
using GsEPWv8_5_MVC.Data.Implementation;
using System.Data;

namespace GsEPWv8_5_MVC.Business.Implementation
{

    public class OBAllocationService : IOBAllocationService
    {
        IOBAllocationRepository objOBAllocationRepository = new OBAllocationRepository();

        public bool SaveOBAlocEditHdr(string pstrCmpId, string pstrAlocDocId, string pstrAlocDocDt,string pstrSrNum, string pstrShipDt,
          string pstrCancelDt, string pstrPriceTkt, string pstrCustId, string pstrCustName, string pstrCustOrdrNum, string pstrCustOrdrDt,string pstrNote)
        {
            return objOBAllocationRepository.SaveOBAlocEditHdr(pstrCmpId, pstrAlocDocId, pstrAlocDocDt, pstrSrNum, pstrShipDt, pstrCancelDt, pstrPriceTkt,
                pstrCustId, pstrCustName, pstrCustOrdrNum, pstrCustOrdrDt, pstrNote);
        }
        public int getAlocEditTmpRefNum()
        {
            return objOBAllocationRepository.getAlocEditTmpRefNum();
        }
        public List<clsOBAlocHdr> getOBAlocHdr(string pstrCmpId, string pstrAlocDocId)
        {
            return objOBAllocationRepository.getOBAlocHdr(pstrCmpId, pstrAlocDocId);

        }
        public List<clsOBAlocSmry> getOBAlocDtl(string pstrCmpId, string pstrAlocDocId)
        {
            return objOBAllocationRepository.getOBAlocDtl(pstrCmpId, pstrAlocDocId);

        }
        public List<clsOBAlocCtn> getOBAlocCtn(string pstrCmpId, string pstrAlocDocId)
        {
            return objOBAllocationRepository.getOBAlocCtn(pstrCmpId, pstrAlocDocId);

        }

        public List<clsOBAlocCtnByLine> getOBAlocCtnByLine(string pstrCmpId, string pstrAlocDocId, int pstrAlocLine)
        {
            return objOBAllocationRepository.getOBAlocCtnByLine(pstrCmpId, pstrAlocDocId, pstrAlocLine);
        }
        public string saveAlocCtnEditByLine(string pstrRefNum, string pstrCmpId, string pstrAlocDocId, string pstrAlocDocDt, int pintLineNum, DataTable pdtAlocCtnList)
        {
            return objOBAllocationRepository.saveAlocCtnEditByLine(pstrRefNum, pstrCmpId,  pstrAlocDocId,   pstrAlocDocDt, pintLineNum,  pdtAlocCtnList);
        }
        public List<ClsOBSRAlocDtl> getSRAlocDtl(string pstrCmpId, string pstrSrNo)
        {
          return  objOBAllocationRepository.getSRAlocDtl(pstrCmpId, pstrSrNo);
        }
        public List<ClsOBSRAlocDtlByLoc> getSRAlocDtlByLoc(string pstrRefNum, string pstrCmpId, string pstrSrNo)
        {
            return objOBAllocationRepository.getSRAlocDtlByLoc(pstrRefNum,pstrCmpId, pstrSrNo);
        }
        public bool SaveAloctionByBatch(string pstrCmpId, string pstrRefNum, string pstrSrNo, string pstrUserId)
        {
            return objOBAllocationRepository.SaveAloctionByBatch(pstrCmpId, pstrRefNum, pstrSrNo, pstrUserId);
        }

        public ClsOBAllocation GetOBSRSummary(ClsOBAllocation objOBAllocation)
        {
            return objOBAllocationRepository.GetOBSRSummary(objOBAllocation);
        }
       public DataTable getOBSRAlocStatus(string pstrCmpId, string pstrRefNum, string pstrUserId, ref DataTable dtUserEmail)
        {
            return objOBAllocationRepository.getOBSRAlocStatus(pstrCmpId, pstrRefNum, pstrUserId, ref dtUserEmail);
        }

        public DataTable getOBSRAlocStatus(string pstrCmpId, string pstrRefNum)
        {
            return objOBAllocationRepository.getOBSRAlocStatus(pstrCmpId, pstrRefNum);
        }
        public PickLabel fnGetOBPickLabel(string pstrCmpId, string pstrRefNum)
        {
            return objOBAllocationRepository.fnGetOBPickLabel(pstrCmpId, pstrRefNum);
        }

        public PickLabel fnGetOBDocForPrint(string pstrCmpId, string pstrRefNum)
        {
            return objOBAllocationRepository.fnGetOBDocForPrint(pstrCmpId, pstrRefNum);
        }
  
        public PickLabel fnGetOBPickLabelSmry(PickLabel pobjPickLabel)
        {
            return objOBAllocationRepository.fnGetOBPickLabelSmry(pobjPickLabel);

        }
        public PickLabel fnGetOBSRListForBinprint(string pstrCmpId, string pstrSrList)
        {
            return objOBAllocationRepository.fnGetOBSRListForBinprint(pstrCmpId, pstrSrList);
        }
        public PickLabel fnGetOBDocBySrList(string pstrCmpId, string pstrSrList)
        {
            return objOBAllocationRepository.fnGetOBDocBySrList(pstrCmpId, pstrSrList);
        }
    }
}
