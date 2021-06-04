using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
    public class GetUniqueNumbersModel : BasicEntity
    {
        public string Unique { get; set; }
        public string ct_value { get; set; }

        public string new_ship_doc_id { get; set; }

        public string new_process_id { get; set; }
    }
    public class GetCompanyNameModel : GetUniqueNumbersModel
    {
        public string cmp_id { get; set; }

        public string cmp_name { get; set; }
    }
    public class GetEcomBinScanOutDetailModel : GetCompanyNameModel
    {
        public string aloc_doc_id { get; set; }

        public string AlocNo { get; set; }
        public string status { get; set; }
    }
    public class GetEcomBinScanOutHeaderModel : GetEcomBinScanOutDetailModel
    {
        public string so_num { get; set; }

        public string SoNum { get; set; }
        public DateTime so_dt { get; set; }

        public DateTime SoDt { get; set; }
        public string cust_ordr_num { get; set; }

        public string Custpo { get; set; }

        public string AllocateNumber { get; set; }

        public string SoDate { get; set; }

        public string SoPo { get; set; }

        public string CustPorder { get; set; }
        public string ScanBin { get; set; }
        public string ScanUPC { get; set; }
        public string loc_id { get; set; }

        public string upc_code { get; set; }

        public string so_itm_num { get; set; }
        public string ship_dt { get; set; }
        public string so_itm_size { get; set; }
        public string aloc_qty { get; set; }
        public string pick_qty { get; set; }
        public string l_str_ship_dt { get; set; }
        public string line_num { get; set; }
        public string itm_code { get; set; }
        public string fob { get; set; }
        public string FullfillType { get; set; }


    }
    public class EcomBinScanOutModel : GetEcomBinScanOutHeaderModel
    {
        //Basic Entity Details
        public string LoginUserID { get; set; }
        public string LoginUserName { get; set; }
        public int LoginUserGroupID { get; set; }
        //List Fetch Details
        public IList<GetEcomBinScanOutHeader> Checkbinupc { get; set; }
        public IList<GetEcomBinScanOutDetail> Getstatus { get; set; }
        public IList<GetUniqueNumbers> GetIdentityNumbers { get; set; }
        public IList<EcomBinScanOut> GetListEcomBinScanOutGridHeader { get; set; }
        public IList<GetEcomBinScanOutHeader> GetListEcomBinScanOutHeader { get; set; }
        public IList<GetCompanyName> GetCompanyNameDetails { get; set; }
        public IList<EcomBinScanOut> GetListEcomBinScanOutSHeader { get; set; }
        public IList<DoubleEcomBinScanOutModel> LstDoubleEcomBinScanOutModel { get; set; }
    }

}
