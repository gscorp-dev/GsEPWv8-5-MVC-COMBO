using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class CustMasterDtl : BasicEntity
    {
        public string cust_of_cmp_id { get; set; }
        public string cmp_id { get; set; }
        public string comp_id { get; set; }
        public string cmp_name { get; set; }
        public string cust_grp_id { get; set; }
        public string status { get; set; }
        public string source { get; set; }
        public string dt_of_entry { get; set; }
        public string last_chg_dt { get; set; }
        public string date_of_entrys { get; set; }
        public string last_of_entrys { get; set; }
        public string region { get; set; }
        public string territory { get; set; }
        public string whs_id { get; set; }
        public string contact { get; set; }
        public string Tel { get; set; }
        public string cell { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string web { get; set; }
        public string bl_free_days { get; set; }
        public string strg_bill { get; set; }
        public string inout_bill { get; set; }
        public string recv_lot_by { get; set; }
        public string aloc_by { get; set; }
        public string mail_name { get; set; }
        public string attn { get; set; }
        public string addr1 { get; set; }
        public string addr2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string frieght_id { get; set; }
        public string ship_via_id { get; set; }
        public string catg { get; set; }
        public string bl_cycle { get; set; }
        public string code { get; set; }
        public string type { get; set; }
        public string credit_lt { get; set; }
        public string msg { get; set; }
        public string disc { get; set; }
        public string ord_val { get; set; }
        public string tax_code { get; set; }
        public string tax_exempt { get; set; }
        public string gl_num { get; set; }
        public string user_id { get; set; }
        public string term_code { get; set; }
        public string insaleid { get; set; }
        public string outsaleid { get; set; }
        public string cust_id { get; set; }
        public string cust_name { get; set; }
        public string groupid { get; set; }
        public string start_dt { get; set; }
        public string tel { get; set; }
        public string dft_whs { get; set; }
        public string regn_id { get; set; }
        public string tery_id { get; set; }
        public string bill_free_days { get; set; }
        public string bill_type { get; set; }
        public string bill_inout_type { get; set; }
        public string Recv_Itm_Mode { get; set; }
        public string Aloc_by { get; set; }
        public string cmpid { get; set; }
        public string custid { get; set; }
        public string allow_new_item { get; set; }
        public string itemlistby { get; set; }
        public string initstrg { get; set; }
        public string Doc_Increment { get; set; }
        public string autoincre { get; set; }
        public string Allow_New_item { get; set; }
        public string item_pick { get; set; }
        public string init_strg_rt_req { get; set; }
        public string aloc_sort_stmt { get; set; }
        public bool l_bool_itemlistby { get; set; }
        public string cust_of_cmpid { get; set; }
        public string fob { get; set; }
        public string addr_line1 { get; set; }
        public string addr_line2 { get; set; }
        public string state_id { get; set; }
        public string post_code { get; set; }
        public string cntry_id { get; set; }
        public string freight_id { get; set; }
        public string cust_catg { get; set; }
        public string bill_cycle { get; set; }
        public string crdt_code { get; set; }
        public string crdt_chck { get; set; }
        public string crdt_msg { get; set; }
        public string crdt_limit { get; set; }
        public string terms_id { get; set; }
        public string ordr_value { get; set; }
        public string tax_exempt_id { get; set; }
        public string whs_name { get; set; }
        public string entity_Code { get; set; }
        public string entity_count { get; set; }
        public string entity_count_doc_id { get; set; }
        public string MasterCount { get; set; }
        public string is_company_user { get; set; }
        public string cust_logo { get; set; }
        public string file_name { get; set; }
        public string Image_Path { get; set; }
        public string Recv_non_doc_itm { get; set; }
        public string CUSTID { get; set; }
        public string Stk_Chk_Reqt { get; set; }
        public string pmt_term { get; set; }
        public string cube_auto_calc { get; set; }
        public string is_bill_by_cube_rounded { get; set; }
        public decimal min_inout_cube { get; set; }
        public decimal min_strg_cube { get; set; }
        public string cmp_addr_line1 { get; set; }
        public string cmp_addr_line2 { get; set; }
        public string cmp_state_id { get; set; }
        public string cmp_post_code { get; set; }
        public string cmp_tel { get; set; }
        public string cmp_fax { get; set; }
        public string cmp_email { get; set; }
        public string cust_addr_line1 { get; set; }
        public string cust_addr_line2 { get; set; }
        public string cust_state_id { get; set; }
        public string cust_post_code { get; set; }
        public string cust_tel { get; set; }
        public string cust_fax { get; set; }
        public string cust_email { get; set; }
        public string cust_city { get; set; }
        public string cmp_city { get; set; }
        public string con_string { get; set; }


        public string io_rate_id { get; set; }
        public string io_rate_name { get; set; }
        public decimal io_rate_price { get; set; }
        public string strg_rate_id { get; set; }
        public string strg_rate_name { get; set; }
        public decimal strg_rate_price { get; set; }
        public bool ecom_recv_by_bin { get; set; }
        public bool allow_940_new_item { get; set; }
        public string cust_type { get; set; }
        public string is_scan_serial { get; set; }

    }

    public class custDfltRate
    {
        public string cmp_id { get; set; }
        public string rate_type { get; set; }
        public string rate_id { get; set; }
        public string rate_name { get; set; }
        public decimal rate_price { get; set; }
        public string inout_bill { get; set; }
        public string strg_bill { get; set; }

    }
    public class appUsers
    {
        public string user_perm { get; set; }
        public string user_id { get; set; }
        public string user_type { get; set; }
        public string user_fst_name { get; set; }
        public string user_lst_name { get; set; }
        public string email { get; set; }
    }
    public class CustMaster : CustMasterDtl
    {
        public IList<appUsers> ListAppUsers { get; set; }
        public IList<CustMasterDtl> ListCustDetails { get; set; }
        public IList<CustMasterDtl> ListCustHdr { get; set; }
        public IList<CustMasterDtl> ListCustConfig { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<CustMasterDtl> ListPickdtl { get; set; }
        public IList<Pick> ListPick { get; set; }
        public IList<Pick> ListCntryPick { get; set; }
        public IList<Pick> ListStatePick { get; set; }
        public IList<Company> ListCustofCompanyPickDtl { get; set; }
        public IList<Company> LstCustOfCmpName { get; set; }
        public IList<Company> ListwhsPickDtl { get; set; }
        public IList<LookUp> ListLookUpDtl { get; set; }
        public IList<LookUp> ListSrtg { get; set; }
        public IList<LookUp> ListInout { get; set; }
        public IList<LookUp> ListRcvLot { get; set; }
        public IList<LookUp> ListAlocBy { get; set; }
        public IList<LookUp> ListCustCatg { get; set; }
        public IList<CustMasterDtl> ListCustDtl { get; set; }
        public IList<CustMasterDtl> ListCheckEntityValue { get; set; }

        public IList<LookUp> ListBillCycle { get; set; }
        public IList<LookUp> ListCreditType { get; set; }
        public IList<LookUp> ListTaxExempt { get; set; }
        public IList<LookUp> ListDocAllowNewItm { get; set; }
        public IList<LookUp> ListItmListBy { get; set; }
        public IList<LookUp> ListDirectAllowNewItm { get; set; }
        public IList<LookUp> ListIncludeInitStrg { get; set; }
        public IList<LookUp> ListAutoIncrement { get; set; }
        public IList<CustMasterDtl> ListCheckExistCmpId { get; set; }
        public IList<CustMasterDtl> ListCheckEntityValueRmaDocId { get; set; }
        public IList<CustMasterDtl> ListGetCustLogo { get; set; }
        public IList<LookUp> ListCubeRounded { get; set; }
        
        public IList<custDfltRate> ListCustDfltRate { get; set; }
        public IList<LookUp> ListCustType { get; set; }
    }
}
