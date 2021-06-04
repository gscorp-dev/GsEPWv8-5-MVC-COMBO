using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class LoginHistoryModel
    {
        public string RemoteIP { get; set; } // this variable get " context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]; (or) context.Request.ServerVariables["REMOTE_ADDR"]; Value"
        public string Latitude { get; set; } // googlemap latitude poit 
        public string Longitude { get; set; } // googlemap longitude poit 
        public string BrowserClientIP { get; set; }
        public string ComputerIP { get; set; } // Client mechin Exat IP Address, But this will be change based on them work grop or them network police or manual change 
        public string ComputerName { get; set; } // Client mechin Exat Name, but this is changable by client
        public string MACAddress { get; set; } // Client mechin Exat Address, this is Unic Address for Every machin.
        public string City { get; set; }
        public string State { get; set; }
        public string ISP { get; set; }
        public string Country { get; set; }
        public string TimeZone { get; set; }
        public string LoginDate { get; set; }
        public string LogoutDate { get; set; }
        public string SessionID { get; set; }
        public bool IsLoginSuccess { get; set; }


    }
    public class Login
    {
        public string CompanyAppName { get; set; }
        public int UserID { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string user_id { get; set; }
        public string passwd { get; set; }
        public string dflt_cmp_id { get; set; }
        public string ismob_user { get; set; }
        public string dflt_cust_id { get; set; }
        public string user_type { get; set; }
        public string Message { get; set; }

        public string Showday { get; set; }
        public string Result { get; set; }
        public string CompanyLogo { get; set; }
        public string is_cmp_user { get; set; }
        public string user_fst_name { get; set; }
        public string user_lst_name { get; set; }
        public string DFLT_DT_REQD { get; set; }
        public string IS3RDUSER { get; set; }
        public string ConnectionName { get; set; }
        public DateTime chg_dt { get; set; }
        public List<Login> LstLoginUserDetails { get; set; }
    }
}