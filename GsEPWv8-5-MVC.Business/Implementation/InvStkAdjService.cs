using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using System.Data;

namespace GsEPWv8_5_MVC.Business.Implementation
{
   public class InvStkAdjService : IInvStkAdjService
    {
        InvStkAdjRepository objRepository = new InvStkAdjRepository();
        public bool SaveInvStkAdjByCtns(InvStkAdjAdd objInvStkAdjByCtns)
        {
            return objRepository.SaveInvStkAdjByCtns(objInvStkAdjByCtns);
        }
        public InvStkAdj getStockForAdj(InvStkAdjInquiry objInvStkAdjInquiry)
        {
            return objRepository.getStockForAdj(objInvStkAdjInquiry);
        }

        public bool SaveInvStkAdjTempSingle(InvStkAdjAdd objInvStkAdjAdd)
        {
            return objRepository.SaveInvStkAdjTempSingle(objInvStkAdjAdd);
        }

        public bool CheckPhyCountFileExists(string p_str_cmp_id, string p_str_file_name)
        {
            return objRepository.CheckPhyCountFileExists(p_str_cmp_id, p_str_file_name);
        }

        public int GetInvPhyCountUploadRefNum(string p_str_cmp_id)
        {
            return objRepository.GetInvPhyCountUploadRefNum(p_str_cmp_id);
        }
        public bool SaveMergeCtns(string pstrCmpId, InvMergeHdr ObjInvMergeHdr, DataTable ldtMergeDtl)
        {
            return objRepository.SaveMergeCtns(pstrCmpId, ObjInvMergeHdr, ldtMergeDtl);
        }
    }
}
