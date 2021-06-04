using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
    public class CompanyMasterDtlModel : BasicEntity
    {
        public string cust_of_cmp_id { get; set; }
        public string dflt_whs_id { get; set; }
        public string user_type { get; set; }
        public string status { get; set; }
        public string remit_addr_line1 { get; set; }
        public string cmp_id { get; set; }
        public string cmp_name { get; set; }     
        public string remit_attn { get; set; }
        public string start_dt { get; set; }
        public string last_chg_dt { get; set; }            
        public string contact { get; set; }
        public string Tel { get; set; }
        public string cell { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string web { get; set; }
        public string group_id { get; set; }      
        public string attn { get; set; }
        public string addr1 { get; set; }
        public string addr2 { get; set; }
        public string city { get; set; }
        public string state_id { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string remit_addr_line2 { get; set; }
        public string Remit_City { get; set; }
        public string remit_state_id { get; set; }
        public string Remit_Post_Code { get; set; }
        public string remit_cntry_id { get; set; }
        public string remit_tel { get; set; }
        public string remit_fax { get; set; }
        public string remit_email { get; set; }
        public string remit_web { get; set; }
        public string post_code { get; set; }
        public string cntry_id { get; set; }
        public string tel { get; set; }
        public string remit_city { get; set; }
        public string remit_post_code { get; set; }
        public string addr_line1 { get; set; }
        public string addr_line2 { get; set; }
        public string cust_of_cmpid { get; set; }
        public string MasterCount { get; set; }
        public string is_company_user { get; set; }
        public string action_type { get; set; }
        public string process_id { get; set; }
        public string whs_name { get; set; }
        public string View_Flag { get; set; }

    }
        public class CompanyMasterModel : CompanyMasterDtlModel
    {
        public IList<Company> LstCustOfCmpName { get; set; }
        public IList<CompanyMasterDtl> ListCmpDetails { get; set; }
        public IList<CompanyMasterDtl>  ListCmpHdrDetails { get; set; }
        public IList<CompanyMasterDtl> ListAllCmpDetails { get; set; }

        public IList<Pick> ListCntryPick { get; set; }
        public IList<Pick> ListStatePick { get; set; }
        public IList<CompanyMasterDtl> ListCheckExistCmpId { get; set; }        
        public IList<Company> ListCustofCompanyPickDtl { get; set; }
        public IList<LookUp> ListLookUpDtl { get; set; }
        public IList<CompanyMasterDtl> ListCheckCustDtl { get; set; }
        public IList<Pick> ListAddStatePick { get; set; }
        public IList<Pick> ListEditStatePick { get; set; }
        public IList<Pick> ListAddRemitStatePick { get; set; }
        public IList<Pick> ListEditRemitStatePick { get; set; }
        public IList<CompanyMasterDtl> ListCheckExistwhsId { get; set; }


    }
}
