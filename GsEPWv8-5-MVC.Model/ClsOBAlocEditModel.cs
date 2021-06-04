using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
   public class ClsOBAlocEditModel
    {
        private string _cmp_id;
        private string _aloc_doc_id;
        private string _sr_no;
        private string _action_flag;
        private clsOBAlocHdr _objOBAlocHdr;
        private clsOBAlocDtl _objOBAlocDtl;
        private clsOBAlocCtn _objOBAlocCtn;
        private clsAlocCtnByLoc _objAlocCtnByLoc;
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string aloc_doc_id { get { return _aloc_doc_id; } set { _aloc_doc_id = value; } }
        public string action_flag { get { return _action_flag; } set { _action_flag = value; } }
        public string sr_no { get { return _sr_no; } set { _sr_no = value; } }
        public clsOBAlocHdr objOBAlocHdr { get { return _objOBAlocHdr; } set { _objOBAlocHdr = value; } }
        public clsOBAlocDtl objBAlocDtl { get { return _objOBAlocDtl; } set { _objOBAlocDtl = value; } }
        public clsOBAlocCtn objOBAlocCtn { get { return _objOBAlocCtn; } set { _objOBAlocCtn = value; } }
        public clsAlocCtnByLoc objAlocCtnByLoc { get { return _objAlocCtnByLoc; } set { _objAlocCtnByLoc = value; } }
        public IList<clsOBAlocHdr> lstOBAlocHdr { get; set; }
        public List<clsOBAlocSmry> lstOBAlocDtl { get; set; }
        public List<clsOBAlocCtn> lstOBAlocCtn { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public List<clsOBAlocSmry> lstOBAlocSmry { get; set; }
        public IList<clsOBAlocCtnByLine> lstOBAlocCtnByLine { get; set; }
    }
}
