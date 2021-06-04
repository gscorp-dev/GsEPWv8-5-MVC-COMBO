using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
   
    public class BinMasterModel 
    {
        public string cmp_id { get; set; }
        public string whs_id { get; set; }
        public string view_flag { get; set; }
        public string bin_id { get; set; }
        public string bin_desc { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string itm_name { get; set; }
        public clsBinMater objBinMater { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<LookUp> ListLookUpBinType { get; set; }
        public IList<clsBinMater> ListBinMasterinqury { get; set; }
        public IList<Pick> ListWhs { get; set; }
        public IList<LookUp> ListStatus { get; set; }
        public IList<LookUp> ListLookUpDtls { get; set; }
        public IList<BinMaster> Listbin_id { get; set; }

        public IList<Pick> ListQtyUoM { get; set; }

    }

}