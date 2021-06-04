using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{

    public class ClsOBAllocation
    {
        private string _cmpId;
        private string _srNo;
        private string _alocDocId;
        private string _processId;
        private string _actionFlag;
        private ClsOBAllocationHdr _OBAllocationHdr;
        private clsOBAlocInquiryByBatch _OBAlocInquiryByBatch;
        private ClsOBSRAlocDtlByLoc _OBSRAlocDtlByLoc;
        public string cmpId { get { return _cmpId; } set { _cmpId = value; } }
        public string srNo { get { return _srNo; } set { _srNo = value; } }
        public string alocDocId { get { return _alocDocId; } set { _alocDocId = value; } }
        public string processId { get { return _processId; } set { _processId = value; } }
        public string actionFlag { get { return _actionFlag; } set { _actionFlag = value; } }
        public ClsOBAllocationHdr OBAllocationHdr { get { return _OBAllocationHdr; } set { _OBAllocationHdr = value; } }
        public List<ClsOBAllocationDtl> lstOBAllocationDtl { get; set; }
        public List<ClsOBAllocatioCtn> lstOBAllocatioCtn { get; set; }
        public List<ClsOBSRAlocDtl> lstOBSRAlocDtl { get; set; }
        public IList<ClsOBSRAlocDtlByLoc> lstOBSRAlocDtlByLoc { get; set; }
        public clsOBAlocInquiryByBatch OBAlocInquiryByBatch { get { return _OBAlocInquiryByBatch; } set { _OBAlocInquiryByBatch = value; } }
        public IList<OBGetSRSummary> ListOBGetSRSummary { get; set; }
      
        public IList<Company> ListCompanyPickDtl { get; set; }

    }

    public class ClsOBAllocationHdr
    {
     
        private string _cmpId;
        private string _cmpName;
        private string _whsId;
        private string _srNo;
        private string _srDt;
        private string _alocDocId;
        private string _alocDocDt;
        private string _alocStatus;
        private string _alocType;
        private string _shipDt;
        private string _cancelDt;
        private bool _priceTkt;
        private string _custId;
        private string _custName;
        private string _custOrderNum;
        private string _custOrderDt;
        private string _shipToId;
        private string _shipToName;
        private string _cntrId;
        private string _note;
        private string _alocPostDt;
        private string _processId;
        private string _actionFlag;
        public string cmpId { get { return _cmpId; } set { _cmpId = value; } }
        public string cmpName { get { return _cmpName; } set { _cmpName = value; } }
        public string whsId { get { return _whsId; } set { _whsId = value; } }
        public string srNo { get { return _srNo; } set { _srNo = value; } }
        public string srDt { get { return _srDt; } set { _srDt = value; } }
        public string alocDocId { get { return _alocDocId; } set { _alocDocId = value; } }
        public string alocDocDt { get { return _alocDocDt; } set { _alocDocDt = value; } }
        public string alocStatus { get { return _alocStatus; } set { _alocStatus = value; } }
        public string alocType { get { return _alocType; } set { _alocType = value; } }
        public string shipDt { get { return _shipDt; } set { _shipDt = value; } }
        public string cancelDt { get { return _cancelDt; } set { _cancelDt = value; } }
        public bool priceTkt { get { return _priceTkt; } set { _priceTkt = value; } }
        public string custId { get { return _custId; } set { _custId = value; } }
        public string custName { get { return _custName; } set { _custName = value; } }
        
        public string custOrderNum { get { return _custOrderNum; } set { _custOrderNum = value; } }
        public string custOrderDt { get { return _custOrderDt; } set { _custOrderDt = value; } }
        public string shipToId { get { return _shipToId; } set { _shipToId = value; } }
        public string shipToName { get { return _shipToName; } set { _shipToName = value; } }
        public string cntrId { get { return _cntrId; } set { _cntrId = value; } }
        public string Note { get { return _note; } set { _note = value; } }
        public string alocPostDt { get { return _alocPostDt; } set { _alocPostDt = value; } }
        public string processId { get { return _processId; } set { _processId = value; } }
        public string actionFlag { get { return _actionFlag; } set { _actionFlag = value; } }
    }
    public class ClsOBAllocationDtl
    {
        private string _cmpId;
        private string _srNo;
        private string _alocDocId;
        private int _lineNum;
        private string _lineStatus;
        private string _itmCode;
        private string _itmNum;
        private string _itmColor;
        private string _itmSize;
        private Int32 _soDtlLine =1;
        private Int32 _alocCtns = 0;
        private Int32 _alocQty = 0;
        private string _alocUoM ;
        private Int32 _dueQty = 0;
        private Int32 _pickCtns = 0;
        private Int32 _pickQty = 0;
        private string _note;
        private string _processId;
       

        public string cmpId { get { return _cmpId; } set { _cmpId = value; } }
        public string srNo { get { return _srNo; } set { _srNo = value; } }
        public string alocDocId { get { return _alocDocId; } set { _alocDocId = value; } }
        public Int32 lineNum { get { return _lineNum; } set { _lineNum = value; } }
        public string lineStatus { get { return _lineStatus; } set { _lineStatus = value; } }
        public string itmCode { get { return _itmCode; } set { _itmCode = value; } }
        public string itmNum { get { return _itmNum; } set { _itmNum = value; } }
        public string itmColor { get { return _itmColor; } set { _itmColor = value; } }
        public string itmSize { get { return _itmSize; } set { _itmSize = value; } }
        public Int32 soDtlLine { get { return _soDtlLine; } set { _soDtlLine = value; } }
        public Int32 alocCtns { get { return _alocCtns; } set { _alocCtns = value; } }
        public Int32 alocQty { get { return _alocQty; } set { _alocQty = value; } }
        public string alocUoM { get { return _alocUoM; } set { _alocUoM = value; } }
        public Int32 dueQty { get { return _dueQty; } set { _dueQty = value; } }
        public Int32 pickCtns { get { return _pickCtns; } set { _pickCtns = value; } }
        public Int32 pickQty { get { return _pickQty; } set { _pickQty = value; } }
        public string Note { get { return _note; } set { _note = value; } }
        public string processId { get { return _processId; } set { _processId = value; } }

    }
    public class ClsOBAllocatioCtn
    {
        private string _cmpId;
        private string _whsId;
        private string _srNo;
        private string _alocDocId;
        private int _lineNum;
        private int _ctnline;
        private string _itmCode;
        private string _itmNum;
        private string _itmColor;
        private string _itmSize;
        private string _poNum;

        private string _lotId;
        private string _rcvdDt;
        private string _locId;
        private string _paletId;
        private Int32 _ctnQty = 0;
        private Int32 _ctnCnt = 0;
        private Int32 _lineQty = 0;
        private Int32 _locBalQty = 0;
        private string _isSplitCtn;
        private Int32 _splitlQty = 0;
        public string cmpId { get { return _cmpId; } set { _cmpId = value; } }
        public string whsId { get { return _whsId; } set { _whsId = value; } }
        public string srNo { get { return _srNo; } set { _srNo = value; } }
        public string alocDocId { get { return _alocDocId; } set { _alocDocId = value; } }
        public Int32 lineNum { get { return _lineNum; } set { _lineNum = value; } }
        public Int32 ctnline { get { return _ctnline; } set { _ctnline = value; } }
        public string itmCode { get { return _itmCode; } set { _itmCode = value; } }
        public string itmNum { get { return _itmNum; } set { _itmNum = value; } }
        public string itmColor { get { return _itmColor; } set { _itmColor = value; } }
        public string itmSize { get { return _itmSize; } set { _itmSize = value; } }
        public string poNum { get { return _poNum; } set { _poNum = value; } }
        public string lotId { get { return _lotId; } set { _lotId = value; } }
        public string rcvdDt { get { return _rcvdDt; } set { _rcvdDt = value; } }
        public string locId { get { return _locId; } set { _locId = value; } }
        public string paletId { get { return _paletId; } set { _paletId = value; } }
        public Int32 ctnCnt { get { return _ctnCnt; } set { _ctnCnt = value; } }
        public Int32 ctnQty { get { return _ctnQty; } set { _ctnQty = value; } }
 
        public Int32 lineQty { get { return _lineQty; } set { _lineQty = value; } }
        public Int32 locBalQty { get { return _locBalQty; } set { _locBalQty = value; } }
        public string isSplitCtn { get { return _isSplitCtn; } set { _isSplitCtn = value; } }
        public Int32 splitlQty { get { return _splitlQty; } set { _splitlQty = value; } }
    }
    public class ClsOBSRAlocDtlByLoc
    {
        private string _refNum;
        private string _cmpId;
        private string _whsId;
        private string _alocDocId;
        private int _lineNum;
        private string _itmCode;
        private string _itmNum;
        private string _itmColor;
        private string _itmSize;
        private string _poNum;

        private string _lotId;
        private string _rcvdDt;
        private string _locId;
        private string _paletId;
    
        private Int32 _ctnCnt = 0;
        private Int32 _ctnQty = 0;
        private Int32 _lineQty = 0;
        private Int32 _lineTakenQty = 0;
        private Int32 _locBalQty = 0;
        private string _isSplitCtn;
        private Int32 _splitlQty = 0;
        public string ref_num { get { return _refNum; } set { _refNum = value; } }
        public string cmp_id { get { return _cmpId; } set { _cmpId = value; } }
        public string whs_id { get { return _whsId; } set { _whsId = value; } }

        public string aloc_doc_id { get { return _alocDocId; } set { _alocDocId = value; } }
        public Int32 line_num { get { return _lineNum; } set { _lineNum = value; } }
         public string itm_code { get { return _itmCode; } set { _itmCode = value; } }
        public string itm_num { get { return _itmNum; } set { _itmNum = value; } }
        public string itm_color { get { return _itmColor; } set { _itmColor = value; } }
        public string itm_size { get { return _itmSize; } set { _itmSize = value; } }
        public string po_num { get { return _poNum; } set { _poNum = value; } }
        public string lot_id { get { return _lotId; } set { _lotId = value; } }
        public string rcvd_dt { get { return _rcvdDt; } set { _rcvdDt = value; } }
        public string loc_id { get { return _locId; } set { _locId = value; } }
        public string palet_id { get { return _paletId; } set { _paletId = value; } }
        public Int32 ctn_cnt { get { return _ctnCnt; } set { _ctnCnt = value; } }
        public Int32 ctn_qty { get { return _ctnQty; } set { _ctnQty = value; } }
        public Int32 line_qty { get { return _lineQty; } set { _lineQty = value; } }
        public Int32 line_taken_qty { get { return _lineTakenQty; } set { _lineTakenQty = value; } }
        public Int32 loc_bal_qty { get { return _locBalQty; } set { _locBalQty = value; } }
        public string is_split_ctn { get { return _isSplitCtn; } set { _isSplitCtn = value; } }
        public Int32 splitl_qty { get { return _splitlQty; } set { _splitlQty = value; } }
    }
    public class ClsOBSRAlocDtl
    {
        private string _refNum;
        private string _cmpId;
        private string _whsId;
        private string _srNo;
        private string _alocDocId;
        private int _lineNum;
        private string _itmCode;
        private string _itmNum;
        private string _itmColor;
        private string _itmSize;
        private string _itmName;
        private Int32 _dueQty = 0;
        private Int32 _availQty = 0;
        private Int32 _alocQty = 0;
        private Int32 _boQty = 0;
        private Int32 _balQty = 0;
        public string ref_num { get { return _refNum; } set { _refNum = value; } }
        public string cmp_id { get { return _cmpId; } set { _cmpId = value; } }
        public string whs_id { get { return _whsId; } set { _whsId = value; } }
        public string sr_num { get { return _srNo; } set { _srNo = value; } }
        public string aloc_doc_id { get { return _alocDocId; } set { _alocDocId = value; } }
        public Int32 line_num { get { return _lineNum; } set { _lineNum = value; } }
        public string itm_code { get { return _itmCode; } set { _itmCode = value; } }
        public string itm_num { get { return _itmNum; } set { _itmNum = value; } }
        public string itm_color { get { return _itmColor; } set { _itmColor = value; } }
        public string itm_size { get { return _itmSize; } set { _itmSize = value; } }
        public string itm_name { get { return _itmName; } set { _itmName = value; } }
        public Int32 avail_qty { get { return _availQty; } set { _availQty = value; } }
        public Int32 due_qty { get { return _dueQty; } set { _dueQty = value; } }
        public Int32 aloc_qty { get { return _alocQty; } set { _alocQty = value; } }
        public Int32 bo_qty { get { return _boQty; } set { _boQty = value; } }
        public Int32 bal_qty { get { return _balQty; } set { _balQty = value; } }
    }

    public class clsOBAlocInquiryByBatch
    {
        private string _cmpId;
        private string _batchNum;
        private string _srNumFrom;
        private string _srNumTo;
        private string _srDtFrom;
        private string _srDtTo;
        private string _srLoadNum;
        private string _screenId;
        public string cmpId { get { return _cmpId; } set { _cmpId = value; } }
        public string batchNum { get { return _batchNum; } set { _batchNum = value; } }
        public string srNumFrom { get { return _srNumFrom; } set { _srNumFrom = value; } }
        public string srNumTo { get { return _srNumTo; } set { _srNumTo = value; } }
        public string srDtFrom { get { return _srDtFrom; } set { _srDtFrom = value; } }
        public string srDtTo { get { return _srDtTo; } set { _srDtTo = value; } }
        public string srLoadNum { get { return _srLoadNum; } set { _srLoadNum = value; } }
        public string screenId { get { return _screenId; } set { _screenId = value; } }
    }
}