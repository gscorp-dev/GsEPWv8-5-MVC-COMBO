using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;

namespace GsEPWv8_5_MVC.Data.Interface
{
    public interface   IInvStkAdjRepository
    {
        bool SaveInvStkAdjByCtns(InvStkAdjAdd objInvStkAdjByCtns);
        InvStkAdj getStockForAdj(InvStkAdjInquiry objInvStkAdjInquiry);
        bool SaveInvStkAdjTempSingle(InvStkAdjAdd objInvStkAdjAdd);
         bool CheckPhyCountFileExists(string p_str_cmp_id, string p_str_file_name);
         int GetInvPhyCountUploadRefNum(string p_str_cmp_id);
        bool SaveMergeCtns(string pstrCmpId, InvMergeHdr ObjInvMergeHdr, DataTable ldtMergeDtl);
    }
}
