using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GsEPWv8_5_MVC.Core.Entity
{
    public class EmailSetup
    {
        #region Constructors  
        public EmailSetup() { }
        #endregion
        #region Private Fields  
        private string _cmp_id;
        private clsRptEmailDtl _clsRptEmailDtl;
        #endregion
        #region Public Properties  
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string tmp_cmp_id { get; set; }
        public string screentitle { get; set; }
        public clsRptEmailDtl objRptEmailDtl  { get { return _clsRptEmailDtl; } set { _clsRptEmailDtl = value; } }
        public EmailCommon objEmailCommon { get; set; }
        public IList<clsRptEmailDtl> lstRptEmailDtl { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<LookUp> ListRptEmailFormat { get; set; }
        #endregion
    }

    public class EmailCommon
    {
  
        public string cmpId { get; set; }
        public string emailFrom { get; set; }
        public string cmpEmailTo { get; set; }
        public string custEmailTo { get; set; }
        public string emailCC { get; set; }
        public string emailMessage { get; set; }
        public string emailFooter { get; set; }

    }
    public class clsRptEmailDtl
    {
        #region Constructors  
        public clsRptEmailDtl() { }
        #endregion
        #region Private Fields  
        private string _cmp_id;
        private string _module_name;
        private string _rpt_id;
        private string _rpt_name;
        private string _is_internal;
          private string _rpt_file_format;
        private string _cmp_email_list;
        private string _cust_email_list;
        private string _email_body;
        private string _is_auto_email;
        #endregion
        #region Public Properties  
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string module_name { get { return _module_name; } set { _module_name = value; } }
        public string rpt_id { get { return _rpt_id; } set { _rpt_id = value; } }
        public string rpt_name { get { return _rpt_name; } set { _rpt_name = value; } }
        public string is_internal { get { return _is_internal; } set { _is_internal = value; } }
         public string rpt_file_format { get { return _rpt_file_format; } set { _rpt_file_format = value; } }
        public string cmp_email_list { get { return _cmp_email_list; } set { _cmp_email_list = value; } }
        public string cust_email_list { get { return _cust_email_list; } set { _cust_email_list = value; } }
        public string email_body { get { return _email_body; } set { _email_body = value; } }
        public string is_auto_email { get { return _is_auto_email; } set { _is_auto_email = value; } }
        #endregion
    }
}

