using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
   public  class InvStkAdj
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
    public class InvStkAdjInquiry
    {
        public string cmp_id { get; set; }
        public string whs_id { get; set; }
        public string itm_code { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string itm_name { get; set; }
        public string rcvd_from_dt { get; set; }
        public string rcvd_to_dt { get; set; }
        public string ib_doc_id { get; set; }
        public string cont_id { get; set; }
        public string lot_id { get; set; }
        public string po_num { get; set; }
        public string palet_id { get; set; }
        public string loc_id { get; set; }
        public string ref_no { get; set; }
        public string status { get; set; }

    }
    public class InvStkAdjDetail
    {
        public string cmp_id { get; set; }
        public string whs_id { get; set; }
        public string itm_code { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string itm_name { get; set; }
        public string rcvd_dt { get; set; }
        public string ib_doc_id { get; set; }
        public string cont_id { get; set; }
        public string lot_id { get; set; }
        public string po_num { get; set; }
        public string palet_id { get; set; }
        public string loc_id { get; set; }
        public int avail_ctn { get; set; }
        public int itm_qty { get; set; }
        public int avail_qty { get; set; }
        public int merge_ctns { get; set; }
        public IList<InvStkAdjDetail> ListInvStkAdjDetail { get; set; }
    }

    public class InvMergeLoad
    {
        public string cmp_id { get; set; }
        public int merge_ctns { get; set; }
        public int merge_ppk { get; set; }
        public int merge_qty { get; set; }
        public int sel_qty { get; set; }
        public int reqd_qty { get; set; }
        public string merge_loc_id { get; set; }
        public string merge_note { get; set; }
        public int merge_odd_ppk { get; set; }

    }
    public class InvMergeHdr
    {
        public string cmp_id { get; set; }
        public string merge_doc_id { get; set; }
        public string itm_code { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public int merge_ctns { get; set; }
        public int merge_ppk { get; set; }
        public string merge_loc_id { get; set; }
        public string merge_note { get; set; }

        public int merge_odd_ppk { get; set; }

    }

    public class InvMergeCtns
    {
       
        public string cmp_id { get; set; }
        public string whs_id { get; set; }
        public string merge_doc_id { get; set; }
        public int line_num { get; set; }
        public string itm_code { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string ib_doc_id { get; set; }
        public string lot_id { get; set; }
        public string loc_id { get; set; }
        public string po_num { get; set; }
        public string palet_id { get; set; }
        public int itm_qty { get; set; }
        public int merge_ctns { get; set; }

    }

    public class InvAdjUploadFileInfo
    {
       
        private string _cmp_id;
        private string _file_name;
        private string _upload_ref_num;
        private string _status;
        private string _upload_by;
        private DateTime _upload_date_time;
        private int _no_of_lines;
        private string _comments;
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }

        public string file_name { get { return _file_name; } set { _file_name = value; } }
        public string upload_ref_num { get { return _upload_ref_num; } set { _upload_ref_num = value; } }
        public string status { get { return _status; } set { _status = value; } }
        public string upload_by { get { return _upload_by; } set { _upload_by = value; } }
        public DateTime upload_date_time { get { return _upload_date_time; } set { _upload_date_time = value; } }
        public int no_of_lines { get { return _no_of_lines; } set { _no_of_lines = value; } }
        public string comments { get { return _comments; } set { _comments = value; } }

    }
    public class InvPhyCountInvalidData
    {
        private int _line_num;
        private string _line_data;
        private string _error_desc;
        public int line_num { get { return _line_num; } set { _line_num = value; } }
        public string line_data { get { return _line_data; } set { _line_data = value; } }
        public string error_desc { get { return _error_desc; } set { _error_desc = value; } }


    }
    public class InvStkAdjAdd
    {
        private string _error_desc = string.Empty;
        private string _itm_code = string.Empty;
        private string _itm_num = string.Empty;
        private string _itm_color = string.Empty;
        private string _itm_size = string.Empty;
        private string _itm_name = string.Empty;
        private string _po_num = string.Empty;
        private string _lot_id = string.Empty;
        private string _cur_loc_id = string.Empty;
        private string _new_loc_id = string.Empty;
        private string _adj_note = string.Empty;
        private string _adj_reason = string.Empty;
        
        public string cmp_id { get; set; }
        public string ref_num { get; set; }
        public int dtl_line { get; set; }
        public string itm_code { get { return _itm_code; } set { _itm_code = value; } }

        public string itm_num { get { return _itm_num; } set { _itm_num = value; } }
        public string itm_color { get { return _itm_color; } set { _itm_color = value; } }
        public string itm_size { get { return _itm_size; } set { _itm_size = value; } }
        public string itm_name { get { return _itm_name; } set { _itm_name = value; } }
        public string lot_id { get { return _lot_id; } set { _lot_id = value; } }
        public string po_num { get { return _po_num; } set { _po_num = value; } }
        public string cur_loc_id { get { return _cur_loc_id; } set { _cur_loc_id = value; } }
        public int cur_avail_ctn { get; set; }
        public int cur_itm_qty { get; set; }
        public int cur_avail_qty { get; set; }
        public string action_flag { get; set; }
        public string new_loc_id { get { return _new_loc_id; } set { _new_loc_id = value; } }
        public int new_avail_ctn { get; set; }
        public int adj_ctns { get; set; }
        public int new_itm_qty { get; set; }
        public int new_avail_qty { get; set; }
        public string adj_note { get { return _adj_note; } set { _adj_note = value; } }
        public string adj_reason { get { return _adj_reason; } set { _adj_reason = value; } }
        public string user_id { get; set; }
        public int split_qty { get; set; }
        public int new_split_ctn_qty { get; set; }
        public int adj_cur_ctns { get; set; }
        public int bal_avail_ctns { get; set; }
        public string error_desc { get { return _error_desc; } set { _error_desc = value; } }
    }
    }
