using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
 public  class ClsEcomLinkModel
    {
        private string _cmp_id;
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public IList<ClsCustEcomLinkHdr> lstCustEcomLinkHdr { get; set; }
        public ClsCustEcomLinkHdr objCustEcomLinkHdr { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
    }
}
