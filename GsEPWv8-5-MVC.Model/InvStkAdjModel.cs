using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
namespace GsEPWv8_5_MVC.Model
{
    public class InvStkAdjModel
    {
        public string cmp_id { get; set; }
        public bool error_mode { get; set; }
        public string error_desc { get; set; }
        public InvStkAdjInquiry objInvStkAdjInquiry { get; set; }
        public InvAdjUploadFileInfo objInvAdjUploadFileInfo { get; set; }
        public InvPhyCountInvalidData objInvPhyCountInvalidData { get; set; }
        public InvStkAdjAdd objInvStkAdjAdd { get; set; }
        public InvMergeHdr objInvMergeHdr { get; set; }
        public InvMergeLoad objInvMergeLoad { get; set; }
        public IList<InvStkAdjDetail> ListInvStkAdjDetail { get; set; }

        public IList<InvStkAdjAdd> ListInvStkAdjUpload { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<LookUp> ListLookUpDtl { get; set; }
        public IList<LookUp> ListInvAdjType { get; set; }
        public IList<InvPhyCountInvalidData> ListInvPhyCountInvalidData { get; set; }
        public IList<InvMergeCtns> ListInvMergeCtns { get; set; }
    }
}
