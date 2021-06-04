using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class UserAccountDtl : BasicEntity
    {
        public string user_id { get; set; }
        public string SelectedCompany { get; set; }
        public string first_name { get; set; }
        public string Status { get; set; }
        public string Exp { get; set; }
        public int Count { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string tel { get; set; }
        public string user_fst_name { get; set; }
        public string user_lst_name { get; set; }
        public string cmp_id { get; set; }
        public string cmp_name { get; set; }
        public string scn_name { get; set; }
        public string scn_view { get; set; }
        public string scn_add { get; set; }
        public string scn_mod { get; set; }
        public string scn_del { get; set; }
        public string scn_post { get; set; }
        public string scn_caption { get; set; }
        public string password { get; set; }
        public string cfm_password { get; set; }
        public string usr_type { get; set; }
        public string group_id { get; set; }
        public string group_name { get; set; }
        public string cmp_perm1 { get; set; }
        public string scn_id { get; set; }
        public string pm_perm { get; set; }
        public string colChk { get; set; }
        public string colCheck { get; set; }
        public string scn_unpost { get; set; }

        public int LineNum { get; set; }
        public string View_Flag { get; set; }
        public string user_type { get; set; }
        public string passwd { get; set; }
        public string dflt_cust_id { get; set; }

        public string MasterCount { get; set; }


    }
    public class UserAccount : UserAccountDtl
    {
        public IList<UserAccountDtl> ListUserDetails { get; set; }
        public IList<UserAccountDtl> LstCmpPermDtls { get; set; }
        public IList<UserAccountDtl> LstCmpPerm { get; set; }
        public IList<Company> ListUserType { get; set; }
        public IList<UserAccountDtl> ListLoadUserDetails { get; set; }
        public IList<UserAccountDtl> LstScnPermDtls { get; set; }
        public IList<UserAccountDtl> ListScnPermDtls { get; set; }
        public IList<UserAccountDtl> ListCmpDtls { get; set; }
        public IList<UserAccountDtl> ListGetUserCmpDetails { get; set; }
        public IList<UserAccountDtl> LstModCmpPermDtls { get; set; }
        public IList<UserAccountDtl> ListGetDfltCmpId { get; set; }

        public IList<UserAccountDtl> LstModScnPermDtls { get; set; }
        public IList<UserAccountDtl> ListCheckUserIdExist { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<UserAccountDtl> ListScnDtls { get; set; }
        public IList<UserAccountDtl> ListCompanyPermissiondtl { get; set; }
        public IList<UserAccountDtl> ListDfltScnDtls { get; set; }
        public IList<UserAccountDtl> ListOtherScnPerm { get; set; }
        public IList<UserAccountDtl> ListOtherScnPermMod { get; set; }
    }
}
