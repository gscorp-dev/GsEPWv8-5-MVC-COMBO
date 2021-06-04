using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
   public  class IBRcvdDataUpdateModel
    {
        public string cmp_id;
        public string ib_doc_id;
        public string ref_no;
        public string cntr_id;
        public string cntr_type;
        public string rcvd_dt;
        public string doc_status_changed;
        public bool excld_bill;
        public IList<IBRcvdDataUpdate> GetRcvdHdr { get; set; }
        public IList<IBRcvdDataUpdateDtl> ListDocItemList { get; set; }
        public IList<Company> ListCompany { get; set; }
        public IList<LookUp> ListContainerType { get; set; }
    }
   
 
}
