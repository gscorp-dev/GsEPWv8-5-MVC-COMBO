using GsEPWv8_5_MVC.Core.Entity;
using System.Collections.Generic;
namespace GsEPWv8_5_MVC.Model
{
    public class OBAllocationModel
    {
        private string _cmpId;
        private string _srNo;
        private string _alocDocId;
        private string _processId;
        private string _actionFlag;
        private ClsOBAllocationHdr _OBAllocationHdr;
        private clsOBAlocInquiryByBatch _OBAlocInquiryByBatch;
        public string cmpId { get { return _cmpId; } set { _cmpId = value; } }
        public string srNo { get { return _srNo; } set { _srNo = value; } }
        public string alocDocId { get { return _alocDocId; } set { _alocDocId = value; } }
        public string processId { get { return _processId; } set { _processId = value; } }
        public string actionFlag { get { return _actionFlag; } set { _actionFlag = value; } }
        public ClsOBAllocationHdr OBAllocationHdr { get { return _OBAllocationHdr; } set { _OBAllocationHdr = value; } }
        public List<ClsOBAllocationDtl> lstOBAllocationDtl { get; set; }
        public List< ClsOBAllocatioCtn > lstOBAllocatioCtn { get; set; }
        public List<ClsOBSRAlocDtl> lstOBSRAlocDtl { get; set; }
        public clsOBAlocInquiryByBatch OBAlocInquiryByBatch { get { return _OBAlocInquiryByBatch; } set { _OBAlocInquiryByBatch = value; } }
        public IList<OBGetSRSummary> ListOBGetSRSummary { get; set; }
        public IList<ClsOBSRAlocDtlByLoc> lstOBSRAlocDtlByLoc { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }

    }
}
