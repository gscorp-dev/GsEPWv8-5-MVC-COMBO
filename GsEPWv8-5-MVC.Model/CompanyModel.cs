using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//CR-3PL_MVC_IB_2018_0219_008 - Display company name instead of Comp Id in all the Reports
//CR - 3PL_MVC_IB_2018_0219_004
namespace GsEPWv8_5_MVC.Model
{
    public class CompanydtlModel : BasicEntity
    {
        public string cmp_id { get; set; }
        public string cmp_name { get; set; }
        public string user_id { get; set; }
        public string whs_id { get; set; }
        public string loc_id { get; set; }
        public string whs_name { get; set; }
        public string cust_cmp_id { get; set; }
        public string cust_of_cmp_id { get; set; }
        public string cust_of_cmp_name { get; set; }
        public string cust_of_cmpid { get; set; }
        public string cust_of_cmpname { get; set; }
public string cust_name { get; set; }
        public string Allow_New_item { get; set; }
        public string item_pick { get; set; }
        public string bill_type { get; set; }
        public string bill_inout_type { get; set; }
        public string group_id { get; set; }
        public string group_name { get; set; }

        public string addr_line1 { get; set; }
        public string city { get; set; }
        public string state_id { get; set; }
        public string post_code { get; set; }
        public string fax { get; set; }
        public string tel { get; set; }
        public string Recv_non_doc_itm { get; set; }

    }
    public class CompanyModel : CompanydtlModel
    {
        public IList<Company> ListCompanyDtl { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<Company> ListFullFillCompanyPickDtl { get; set; }
        public IList<Company> ListCustofCompanyPickDtl { get; set; }
        public IList<Company> ListwhsPickDtl { get; set; }
        public IList<Company> ListLocPickDtl { get; set; }
  public IList<Company> Lstcmpdtl { get; set; }
        public IList<Company> Lstwhsdtl { get; set; }
        public IList<Company> Lstcustdtl { get; set; }
        public IList<Company> ListUserType { get; set; }

        
        public IList<Company> ListGetCustConfigDtls { get; set; }
        public IList<Company> LstCmpName { get; set; } //CR - 3PL_MVC_IB_2018_0219_008
        public IList<Company> LstCustOfCmpName { get; set; } //CR - 3PL_MVC_IB_2018_0219_004
        public IList<Company> ListCompanyAddresHdrDtls { get; set; }


    }
}
