using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject2
{
    public class tbl_ma_user
    {
        public tbl_ma_user()
        {
        }

        private int _user_id;
        public int user_id
        {
            get { return _user_id; }
            set { _user_id = value; }
        }

        private string _username;
        public string username
        {
            get { return _username; }
            set { _username = value; }
        }

        private string _password;
        public string password
        {
            get { return _password; }
            set { _password = value; }
        }

        private string _fname;
        public string fname
        {
            get { return _fname; }
            set { _fname = value; }
        }

        private string _lname;
        public string lname
        {
            get { return _lname; }
            set { _lname = value; }
        }

        private string _mname;
        public string mname
        {
            get { return _mname; }
            set { _mname = value; }
        }

        private string _fullname;
        public string fullname
        {
            get { return _fullname; }
            set { _fullname = value; }
        }

        private string _ssn;
        public string ssn
        {
            get { return _ssn; }
            set { _ssn = value; }
        }

        private string _emailadd;
        public string emailadd
        {
            get { return _emailadd; }
            set { _emailadd = value; }
        }

        private string _telnum;
        public string telnum
        {
            get { return _telnum; }
            set { _telnum = value; }
        }

        private string _mobilenum;
        public string mobilenum
        {
            get { return _mobilenum; }
            set { _mobilenum = value; }
        }

        private string _address1;
        public string address1
        {
            get { return _address1; }
            set { _address1 = value; }
        }

        private string _address2;
        public string address2
        {
            get { return _address2; }
            set { _address2 = value; }
        }

        private string _city;
        public string city
        {
            get { return _city; }
            set { _city = value; }
        }

        private string _state;
        public string state
        {
            get { return _state; }
            set { _state = value; }
        }

        private string _zipcode;
        public string zipcode
        {
            get { return _zipcode; }
            set { _zipcode = value; }
        }

        private string _country;
        public string country
        {
            get { return _country; }
            set { _country = value; }
        }

        private string _fulladdress;
        public string fulladdress
        {
            get { return _fulladdress; }
            set { _fulladdress = value; }
        }

        private int _supervisor_id;
        public int supervisor_id
        {
            get { return _supervisor_id; }
            set { _supervisor_id = value; }
        }

        private string _supervisor_name;
        public string supervisor_name
        {
            get { return _supervisor_name; }
            set { _supervisor_name = value; }
        }

        private byte[] _photo;
        public byte[] photo
        {
            get { return _photo; }
            set { _photo = value; }
        }

        private string _photo_filename;
        public string photo_filename
        {
            get { return _photo_filename; }
            set { _photo_filename = value; }
        }

        private string _photo_fullfilename;
        public string photo_fullfilename
        {
            get { return _photo_fullfilename; }
            set { _photo_fullfilename = value; }
        }

        private byte[] _fingerprint;
        public byte[] fingerprint
        {
            get { return _fingerprint; }
            set { _fingerprint = value; }
        }

        private string _fingerprint_filename;
        public string fingerprint_filename
        {
            get { return _fingerprint_filename; }
            set { _fingerprint_filename = value; }
        }


        private byte[] _signature;
        public byte[] signature
        {
            get { return _signature; }
            set { _signature = value; }
        }

        private string _signature_filename;
        public string signature_filename
        {
            get { return _signature_filename; }
            set { _signature_filename = value; }
        }

        private string _initial_text;
        public string initial_text
        {
            get { return _initial_text; }
            set { _initial_text = value; }
        }

        private byte[] _initial;
        public byte[] initial
        {
            get { return _initial; }
            set { _initial = value; }
        }

        private string _initial_filename;
        public string initial_filename
        {
            get { return _initial_filename; }
            set { _initial_filename = value; }
        }

        private int _usertype_id;
        public int usertype_id
        {
            get { return _usertype_id; }
            set { _usertype_id = value; }
        }

        private string _usertype_name;
        public string usertype_name
        {
            get { return _usertype_name; }
            set { _usertype_name = value; }
        }

        private int _aidetype_id;
        public int aidetype_id
        {
            get { return _aidetype_id; }
            set { _aidetype_id = value; }
        }

        private string _aidetype_name;
        public string aidetype_name
        {
            get { return _aidetype_name; }
            set { _aidetype_name = value; }
        }

        private int _status_id;
        public int status_id
        {
            get { return _status_id; }
            set { _status_id = value; }
        }

        private string _status_name;
        public string status_name
        {
            get { return _status_name; }
            set { _status_name = value; }
        }

        private string _notes;
        public string notes
        {
            get { return _notes; }
            set { _notes = value; }
        }

        private int _cmpid;
        public int cmpid
        {
            get { return _cmpid; }
            set { _cmpid = value; }
        }

        private string _cmpname;
        public string cmpname
        {
            get { return _cmpname; }
            set { _cmpname = value; }
        }

        private string _userpath;
        public string userpath
        {
            get { return _userpath; }
            set { _userpath = value; }
        }

        private string _documentpath;
        public string documentpath
        {
            get { return _documentpath; }
            set { _documentpath = value; }
        }

        private string _defaulthomepage;
        public string defaulthomepage
        {
            get { return _defaulthomepage; }
            set { _defaulthomepage = value; }
        }

        private bool _allowdatalink;
        public bool allowdatalink
        {
            get { return _allowdatalink; }
            set { _allowdatalink = value; }
        }

        private string _salesmanallowed;
        public string salesmanallowed
        {
            get {
                if (_salesmanallowed != null)
                    return _salesmanallowed;
                else
                    return "";
                }
            set { _salesmanallowed = value; }
        }

        private bool _UseCompLogo;
        public bool UseCompLogo
        {
            get { return _UseCompLogo; }
            set { _UseCompLogo = value; }
        }

        private string _compwebsite;
        public string compwebsite
        {
            get { return _compwebsite; }
            set { _compwebsite = value; }
        }

        private string _compid;
        public string compid
        {
            get { return _compid; }
            set { _compid = value; }
        }

        private string _compname;
        public string compname
        {
            get { return _compname; }
            set { _compname = value; }
        }

        private string _comp_logo;
        public string comp_logo
        {
            get { return _comp_logo; }
            set { _comp_logo = value; }
        }

        private string _compemailaddress;
        public string compemailaddress
        {
            get { return _compemailaddress; }
            set { _compemailaddress = value; }
        }

        private string _complogo_height;
        public string complogo_height
        {
            get { return _complogo_height; }
            set { _complogo_height = value; }
        }

        private string _complogo_width;
        public string complogo_width
        {
            get { return _complogo_width; }
            set { _complogo_width = value; }
        }

        private string _comptelnum1;
        public string comptelnum1
        {
            get { return _comptelnum1; }
            set { _comptelnum1 = value; }
        }

        private string _comptelnum2;
        public string comptelnum2
        {
            get { return _comptelnum2; }
            set { _comptelnum2 = value; }
        }

        private string _compfaxnum1;
        public string compfaxnum1
        {
            get { return _compfaxnum1; }
            set { _compfaxnum1 = value; }
        }

        private string _compfaxnum2;
        public string compfaxnum2
        {
            get { return _compfaxnum2; }
            set { _compfaxnum2 = value; }
        }

        private string _compfulladdress;
        public string compfulladdress
        {
            get { return _compfulladdress; }
            set { _compfulladdress = value; }
        }

        private string _compaddress1;
        public string compaddress1
        {
            get { return _compaddress1; }
            set { _compaddress1 = value; }
        }

        private string _compaddress2;
        public string compaddress2
        {
            get { return _compaddress2; }
            set { _compaddress2 = value; }
        }

        private string _compcity;
        public string compcity
        {
            get { return _compcity; }
            set { _compcity = value; }
        }

        private string _compstate;
        public string compstate
        {
            get { return _compstate; }
            set { _compstate = value; }
        }

        private string _compzipcode;
        public string compzipcode
        {
            get { return _compzipcode; }
            set { _compzipcode = value; }
        }

        private string _compcountry;
        public string compcountry
        {
            get { return _compcountry; }
            set { _compcountry = value; }
        }

        private DateTime _datecreated;
        public DateTime datecreated
        {
            get { return _datecreated; }
            set { _datecreated = value; }
        }

        private string _usercreated;
        public string usercreated
        {
            get { return _usercreated; }
            set { _usercreated = value; }
        }

        private DateTime _datemodified;
        public DateTime datemodified
        {
            get { return _datemodified; }
            set { _datemodified = value; }
        }

        private string _usermodified;
        public string usermodified
        {
            get { return _usermodified; }
            set { _usermodified = value; }
        }

        private string _MenuOptSelected;
        public string MenuOptSelected
        {
            get { return _MenuOptSelected; }
            set { _MenuOptSelected = value; }
        }

        private bool _MenuChanged;
        public bool MenuChanged
        {
            get { return _MenuChanged; }
            set { _MenuChanged = value; }
        }
        public string DefaultDivision { get; set; }
        public string prodcontrol { get; set; }
        public string user_acct_id { get; set; }
    }
}
