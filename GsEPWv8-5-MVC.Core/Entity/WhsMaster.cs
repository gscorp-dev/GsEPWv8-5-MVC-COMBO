using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class WhsMasterDtl
    {

        #region Constructors
        public WhsMasterDtl()
        {
        }
        #endregion
        #region Private Fields
        private string _cmp_id;
        private string _cmp_name;
        private string _whs_id;
        private string _whs_name;
        private string _status;
        private DateTime _start_dt;
        private DateTime _last_chg_dt;
        private string _attn;
        private string _mail_name;
        private string _addr_line1;
        private string _addr_line2;
        private string _city;
        private string _state_id;
        private string _post_code;
        private string _cntry_id;
        private string _tel;
        private string _cell;
        private string _fax;
        private string _email;
        private string _web;
        private DateTime _fm_dt;
        private DateTime _to_dt;
        private string _notes;
        private bool _dft_whs;
        private string _process_id;
        private string _View_Flag;
        private string _StartDate;
        private string _lastDate;
        private string _action_type;
        private int _row_count;
        private string _user_id;
        #endregion
        #region Public Properties
        public string cmp_id
        {
            get { return _cmp_id; }
            set { _cmp_id = value; }
        }
        public string cmp_name
        {
            get { return _cmp_name; }
            set { _cmp_name = value; }
        }
        public string whs_id
        {
            get { return _whs_id; }
            set { _whs_id = value; }
        }
        public string whs_name
        {
            get { return _whs_name; }
            set { _whs_name = value; }
        }
        public string status
        {
            get { return _status; }
            set { _status = value; }
        }
        public DateTime start_dt
        {
            get { return _start_dt; }
            set { _start_dt = value; }
        }
        public DateTime last_chg_dt
        {
            get { return _last_chg_dt; }
            set { _last_chg_dt = value; }
        }
        public string attn
        {
            get { return _attn; }
            set { _attn = value; }
        }
        public string mail_name
        {
            get { return _mail_name; }
            set { _mail_name = value; }
        }
        public string addr_line1
        {
            get { return _addr_line1; }
            set { _addr_line1 = value; }
        }
        public string addr_line2
        {
            get { return _addr_line2; }
            set { _addr_line2 = value; }
        }
        public string city
        {
            get { return _city; }
            set { _city = value; }
        }
        public string state_id
        {
            get { return _state_id; }
            set { _state_id = value; }
        }
        public string post_code
        {
            get { return _post_code; }
            set { _post_code = value; }
        }
        public string cntry_id
        {
            get { return _cntry_id; }
            set { _cntry_id = value; }
        }
        public string tel
        {
            get { return _tel; }
            set { _tel = value; }
        }
        public string cell
        {
            get { return _cell; }
            set { _cell = value; }
        }
        public string fax
        {
            get { return _fax; }
            set { _fax = value; }
        }
        public string email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string web
        {
            get { return _web; }
            set { _web = value; }
        }
        public DateTime fm_dt
        {
            get { return _fm_dt; }
            set { _fm_dt = value; }
        }
        public DateTime to_dt
        {
            get { return _to_dt; }
            set { _to_dt = value; }
        }
        public string notes
        {
            get { return _notes; }
            set { _notes = value; }
        }
        public bool dft_whs
        {
            get { return _dft_whs; }
            set { _dft_whs = value; }
        }
        public string process_id
        {
            get { return _process_id; }
            set { _process_id = value; }
        }
        public string View_Flag
        {
            get { return _View_Flag; }
            set { _View_Flag = value; }
        }
        public string StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; }
        }
        public string lastDate
        {
            get { return _lastDate; }
            set { _lastDate = value; }
        }
        public string action_type
        {
            get { return _action_type; }
            set { _action_type = value; }
        }
        public int row_count
        {
            get { return _row_count; }
            set { _row_count = value; }
        }
        public string user_id
        {
            get { return _user_id; }
            set { _user_id = value; }
        }
        #endregion
    }


    public class WhsMaster : WhsMasterDtl
    {
        //List Fetch Details
        public IList<WhsMasterDtl> ListWhsMaster { get; set; }
        public IList<WhsMasterDtl> ListWhsMasterViewDtl { get; set; }
        public IList<WhsMasterDtl> LstCheckWhsId { get; set; }
        public IList<WhsMasterDtl> LstCheckWhsIdnotdel { get; set; }
        public IList<LookUp> ListLookUpDtl { get; set; }
        public IList<WhsMasterDtl> ListCompanyPickDtl { get; set; }
       public IList<LookUp> ListLookUpStatusDtl { get; set; }
        public IList<Pick> ListCntryPick { get; set; }
        public IList<Pick> ListStatePick { get; set; }
        public IList<Pick> ListAddStatePick { get; set; }
        public IList<Pick> ListEditStatePick { get; set; }
    }
}
