using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class ClsMaEcomLink
    {
        private int _link_id;
        private string _link_name;
        private string _web_address;
        public  int link_id  { get { return _link_id; } set { _link_id = value; } }
        public string link_name { get { return _link_name; } set { _link_name = value; } }
        public string web_address { get { return _web_address; } set { _web_address = value; } }
    }
    public class ClsCustEcomLinkHdr : ClsMaEcomLink
    {
        private string _cmp_id;
        private string _cust_id;
        private string _user_account;
        private string _user_pwd;
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string cust_id { get { return _cust_id; } set { _cust_id = value; } }
        public string user_account { get { return _user_account; } set { _user_account = value; } }
        public string user_pwd { get { return _user_pwd; } set { _user_pwd = value; } }
    }

    public class ClsEcomLink :ClsCustEcomLinkHdr
    {
  
        public IList<ClsCustEcomLinkHdr> lstCustEcomLinkHdr { get; set; }
        public ClsCustEcomLinkHdr objCustEcomLinkHdr { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
    }
}
