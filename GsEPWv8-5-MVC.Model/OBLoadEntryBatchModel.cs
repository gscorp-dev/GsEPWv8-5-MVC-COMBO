using System;
using System.Collections.Generic;
using GsEPWv8_5_MVC.Core.Entity;


namespace GsEPWv8_5_MVC.Model
{
   public class OBLoadEntryBatchModel
    {
    }
    public class OBLoadUploadDtlModel
    {
        private string _upload_ref_num = string.Empty;
        private string _cmp_id;
        private string _batch_num;
        private int _dtl_line;
        private string _cust_po;
        private string _dept_id;
        private string _store_id;
        private string _so_num;
        private string _load_number;
        private string _load_approve_dt;
        private string _carrier_name;
        private string _load_pick_dt;
        private decimal _tot_cube;
        private decimal _tot_weight;
        private int _tot_palet;
        private string _maker;
        private string _maker_dt;
        private string _proc_status;

        public string upload_ref_num { get { return _upload_ref_num; } set { _upload_ref_num = value; } }
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string batch_num { get { return _batch_num; } set { _batch_num = value; } }
        public int dtl_line { get { return _dtl_line; } set { _dtl_line = value; } }

        public string cust_po { get { return _cust_po; } set { _cust_po = value; } }
        public string dept_id { get { return _dept_id; } set { _dept_id = value; } }
        public string store_id { get { return _store_id; } set { _store_id = value; } }
        public string so_num { get { return _so_num; } set { _so_num = value; } }
        public string load_number { get { return _load_number; } set { _load_number = value; } }
        public string load_approve_dt { get { return _load_approve_dt; } set { _load_approve_dt = value; } }
        public string carrier_name { get { return _carrier_name; } set { _carrier_name = value; } }
        public string load_pick_dt { get { return _load_pick_dt; } set { _load_pick_dt = value; } }
        public decimal tot_cube { get { return _tot_cube; } set { _tot_cube = value; } }
        public decimal tot_weight { get { return _tot_weight; } set { _tot_weight = value; } }
        public int tot_palet { get { return _tot_palet; } set { _tot_palet = value; } }
        public string maker { get { return _maker; } set { _maker = value; } }
        public string maker_dt { get { return _maker_dt; } set { _maker_dt = value; } }
        public string proc_status { get { return _proc_status; } set { _proc_status = value; } }
        
    }

    public class OBLoadUploadFileModel
    {
        private string _cmp_id = string.Empty;
        private string _upload_ref_num = string.Empty;
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string upload_ref_num { get { return _upload_ref_num; } set { _upload_ref_num = value; } }
        public string user_id { get; set; }
        public string upload_dt_from { get; set; }
        public string upload_dt_to { get; set; }
        public string file_name { get; set; }
        public bool error_mode { get; set; }
        public string error_desc { get; set; }
        public int dtl_count { get; set; }
        public IList<OBLoadUploadDtl> ListOBLoadUploadDtl { get; set; }
        public OBLoadUploadFileInfo objOBLoadUploadFileInfo;
        public IList<OBLoadUploadInvalidData> ListOBLoadUploadInvalidData { get; set; }
        public IList<Company> ListCompany { get; set; }
    }

    public class OBLoadUploadFileInfoModel
    {
        #region Constructors  
        public OBLoadUploadFileInfoModel()
        {

        }
        #endregion
        #region Private Fields  
        private string _cmp_id;
        private string _file_name;
        private string _upload_ref_num;
        private string _status;
        private string _upload_by;
        private DateTime _upload_date_time;
        private int _no_of_lines;
        private string _comments;

        #endregion
        #region Public Properties  
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }

        public string file_name { get { return _file_name; } set { _file_name = value; } }
        public string upload_ref_num { get { return _upload_ref_num; } set { _upload_ref_num = value; } }
        public string status { get { return _status; } set { _status = value; } }
        public string upload_by { get { return _upload_by; } set { _upload_by = value; } }
        public DateTime upload_date_time { get { return _upload_date_time; } set { _upload_date_time = value; } }
        public int no_of_lines { get { return _no_of_lines; } set { _no_of_lines = value; } }
        public string comments { get { return _comments; } set { _comments = value; } }

        #endregion
    }

    public class OBLoadUploadInvalidDataModel
    {
        private string _cntr_id;
        private int _line_num;
        private string _line_data;
        private string _error_desc;

        public string cntr_id { get { return _cntr_id; } set { _cntr_id = value; } }
        public int line_num { get { return _line_num; } set { _line_num = value; } }
        public string line_data { get { return _line_data; } set { _line_data = value; } }
        public string error_desc { get { return _error_desc; } set { _error_desc = value; } }


    }
}
