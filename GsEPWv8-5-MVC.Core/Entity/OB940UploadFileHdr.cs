using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class OB940UploadFileHdr
    {
        #region Constructors  
        public OB940UploadFileHdr()
        {

        }
        #endregion
        #region Private Fields  
        private string _upload_ref_num;
        private string _cmp_id;
        private string _ref_num;
        private string _batch_id;
        private string _cust_id;
        private int _dtl_count;
        private string _ordr_num;
        private string _cust_ordr_num;
        private int _rel_id;
        private DateTime _sr_dt;
        private DateTime _start_dt;
        private DateTime _cancel_dt;
        private string _dept_id;
        private string _store_id;
        private string _st_name;
        private string _st_addr_line1;
        private string _st_addr_line2;
        private string _st_city;
        private string _st_state_id;
        private string _st_post_code;
        private string _st_cntry_id;
        private string _hdr_note;
        private string _shipvia_id;
        private string _pick_no;
        private string _ref_no;

      
        #endregion
        #region Public Properties  
        public string upload_ref_num { get { return _upload_ref_num; } set { _upload_ref_num = value; } }
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string ref_num { get { return _ref_num; } set { _ref_num = value; } }
        public string batch_id { get { return _batch_id; } set { _batch_id = value; } }
        public string cust_id { get { return _cust_id; } set { _cust_id = value; } }
        public int dtl_count { get { return _dtl_count; } set { _dtl_count = value; } }
        public string ordr_num { get { return _ordr_num; } set { _ordr_num = value; } }
        public string cust_ordr_num { get { return _cust_ordr_num; } set { _cust_ordr_num = value; } }
        public int rel_id { get { return _rel_id; } set { _rel_id = value; } }
        public DateTime sr_dt { get { return _sr_dt; } set { _sr_dt = value; } }
        public DateTime start_dt { get { return _start_dt; } set { _start_dt = value; } }
        public DateTime cancel_dt { get { return _cancel_dt; } set { _cancel_dt = value; } }
        public string dept_id { get { return _dept_id; } set { _dept_id = value; } }
        public string store_id { get { return _store_id; } set { _store_id = value; } }
        public string st_name { get { return _st_name; } set { _st_name = value; } }
        public string st_addr_line1 { get { return _st_addr_line1; } set { _st_addr_line1 = value; } }
        public string st_addr_line2 { get { return _st_addr_line2; } set { _st_addr_line2 = value; } }
        public string st_city { get { return _st_city; } set { _st_city = value; } }
        public string st_state_id { get { return _st_state_id; } set { _st_state_id = value; } }
        public string st_post_code { get { return _st_post_code; } set { _st_post_code = value; } }
        public string st_cntry_id { get { return _st_cntry_id; } set { _st_cntry_id = value; } }
        public string hdr_note { get { return _hdr_note; } set { _hdr_note = value; } }
        public string shipvia_id { get { return _shipvia_id; } set { _shipvia_id = value; } }
        public string pick_no { get { return _pick_no; } set { _pick_no = value; } }
        public string ref_no { get { return _ref_no; } set { _ref_no = value; } }
        

        #endregion
    }


}
