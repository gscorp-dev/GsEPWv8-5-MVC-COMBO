using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GsEPWv8_5_MVC.Core.Entity
{
    public class RateMasterDtl : BasicEntity
    {
        public string uom_id { get; set; }
        public string uom_desc { get; set; }
        public string uom_type { get; set; }
        public string cmp_id { get; set; }
        public string cmp_name { get; set; }
        public string type { get; set; }
        public string catg { get; set; }
        public string itm_num { get; set; }
        public string itm_number { get; set; }

        public string status { get; set; }
       
        public decimal list_price { get; set; }
        public string price_uom { get; set; }
        public string last_so_dt { get; set; }
        public string editable { get; set; }
        public string Rate_Type { get; set; }
        public string Catg { get; set; }
        public string Rate_Id { get; set; }
        public string Rate_Desc { get; set; }
        public string Status { get; set; }

        public string p_itm_num { get; set; }
        public string p_type { get; set; }
        public string p_itm_name { get; set; }


        public string itm_name { get; set; }
        public string Rate_Id_Fm { get; set; }
        public string Rate_Id_To { get; set; }
        public string tel { get; set; }
        public string fax { get; set; }
        public string city { get; set; }
        public string state_id { get; set; }
        public string post_code { get; set; }
        public string addr_line1 { get; set; }
        public string MenuID { get; set; }
        public string ProcessID { get; set; }
        public string MasterCount { get; set; }
        public string is_company_user { get; set; }
          public string opt_id { get; set; }

        //CR_3PL_MVC_BL_2018_0220_001 Added By Ravi
        public string lookuptype { get; set; }
        public bool is_auto_ibs { get; set; }
        public string ibs_unit { get; set; }
        //END
    }
    public class RateDtl
    {
        public string CmpId { get; set; }
        public string RateId { get; set; }
        public string RateDesc { get; set; }
        public string ListPrice { get; set; }
        public string PriceUom { get; set; }
        public string LastChangedDate { get; set; }
        public IList<RateDtl> ListRateDtl { get; set; }
        
    }

    public class RateMaster : RateMasterDtl
    {
        //List Fetch Details
        public IList<RateMasterDtl> ListRateMaster { get; set; }
        public IList<Company> LstCmpName { get; set; }
        public IList<RateMasterDtl> ListRateMasterRpt { get; set; }
        public IList<RateMasterDtl> ListRateMasterViewDtl { get; set; }
        public IList<LookUp> ListLookUpDtl { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<LookUp> ListLookUpCategoryDtl { get; set; }
        public IList<LookUp> ListLookUpStatusDtl { get; set; }
        public IList<Priceuom> ListPriceuom { get; set; }
        public IList<RateMasterDtl> LstRateId { get; set; }

        public IList<LookUp> ListIBSUnit { get; set; }



    }

}
