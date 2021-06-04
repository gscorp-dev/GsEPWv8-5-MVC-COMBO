using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
    public class LookUpDtlModel : BasicEntity
    {
        public string id { get; set; }
        public string lookuptype { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string default_yn { get; set; }
        public string status_id { get; set; }
    }
    public class LookUpModel : LookUpDtlModel
    {
        public IList<LookUp> ListLookUpDtl { get; set; }
        public IList<LookUp> ListLookUpCategoryDtl { get; set; }
        public IList<LookUp> ListLookUpStatusDtl { get; set; }
    }
}
