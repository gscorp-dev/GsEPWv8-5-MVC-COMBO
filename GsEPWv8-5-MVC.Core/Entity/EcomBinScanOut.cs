using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class GetUniqueNumbers : BasicEntity
    {
        public string Unique { get; set; }
        public string ct_value { get; set; }

        public string new_ship_doc_id { get; set; }

        public string new_process_id { get; set; }
    }
    public class GetCompanyName : GetUniqueNumbers
    {
        public string cmp_id { get; set; }

        public string cmp_name { get; set; }
    }
    public class GetEcomBinScanOutDetail : GetCompanyName
    {
        public string aloc_doc_id { get; set; }

        public string AlocNo { get; set; }

        public string status { get; set; }
    }
    public class GetEcomBinScanOutHeader : GetEcomBinScanOutDetail
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
    public class EcomBinScanOut : GetEcomBinScanOutHeader
    {
        //Basic Entity Details
        public int LoginUserID { get; set; }
        public string LoginUserName { get; set; }
        public int LoginUserGroupID { get; set; }

        public IList<GetEcomBinScanOutHeader> Checkbinupc { get; set; }
        public IList<GetEcomBinScanOutDetail> Getstatus { get; set; }
        //List Fetch Details
        public IList<GetUniqueNumbers> GetIdentityNumbers { get; set; }
        public IList<GetEcomBinScanOutHeader> GetListEcomBinScanOutHeader { get; set; }
        public IList<GetCompanyName> GetCompanyNameDetails{ get; set; }

        public IList<EcomBinScanOut> GetListEcomBinScanOutSHeader { get; set; }

        public IList<EcomBinScanOut> GetListEcomBinScanOutGridHeader { get; set; }
        public IList<DoubleEcomBinScanOutModel> LstDoubleEcomBinScanOutModel { get; set; }
        
        public DataTable dtDoubleEcomBinScanOutModel { get; set; }
    }

    public class DoubleEcomBinScanOutModel
    {
        public string loc_id { get; set; }

        public string upc_code { get; set; }

        public string so_itm_num { get; set; }

        public string so_itm_size { get; set; }
        public string aloc_qty { get; set; }
        public string pick_qty { get; set; }
    }
}
