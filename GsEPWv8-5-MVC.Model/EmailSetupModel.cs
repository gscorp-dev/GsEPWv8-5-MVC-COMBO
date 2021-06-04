using System;
using System.Collections.Generic;
using GsEPWv8_5_MVC.Core.Entity;

namespace GsEPWv8_5_MVC.Model
{
    public class EmailSetupModel
    {
        #region Constructors  
        public EmailSetupModel() { }
        #endregion
        #region Private Fields  
        private string _cmp_id;
        private clsRptEmailDtl _clsRptEmailDtl;
        #endregion
        #region Public Properties  
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string tmp_cmp_id { get; set; }
        public string screentitle { get; set; }
        public clsRptEmailDtl objRptEmailDtl { get { return _clsRptEmailDtl; } set { _clsRptEmailDtl = value; } }
        public EmailCommon objEmailCommon { get; set; }
        public IList<clsRptEmailDtl> lstRptEmailDtl { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<LookUp> ListRptEmailFormat { get; set; }
        #endregion
    }
}
