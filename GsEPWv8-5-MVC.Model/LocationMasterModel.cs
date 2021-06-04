using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
    public class LocationMasterModel
    {
        public string cmp_id { get; set; }
        public string whs_id { get; set; }
        public string Whs_id { get; set; }
        public string Whs_dtl { get; set; }
        public string loc_id { get; set; }
        public string loc_desc { get; set; }
        public string Whs_name { get; set; }
        public string status { get; set; }
        public string note { get; set; }
        public decimal length { get; set; }
        public decimal width { get; set; }
        public decimal depth { get; set; }
        public decimal cube { get; set; }
        public string process_id { get; set; }
        public string usage { get; set; }
        public string option { get; set; }
        public int totalcount { get; set; }
        public int totalrecords { get; set; }
        public string state_id { get; set; }
        public string tmp_cmp_id { get; set; }
        public string loc_type { get; set; }
        public IList<LocationMaster> ListLocationMasterDetails { get; set; }
        public IList<LocationMaster> ListDeleteMasterDetails { get; set; }
        public IList<Pick> ListPick { get; set; }
        public IList<LocationMaster> ListWhsDetails { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<LocationMaster> ListInsertMasterDetails { get; set; }
        public IList<Company> ListCompany { get; set; }
        public IList<Pick> ListWareHousePickdtl { get; set; }
        public IList<LookUp> ListLocType { get; set; }


    }
}
