using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_4_MVC.Core.Entity
{

    public class IB943InvalidData
    {
        public string ref_num { get; set; }
        public int line_num { get; set; }
        public string line_data { get; set; }
        public string error_desc { get; set; }


    }

    public class eComIB943Upload
    {
        public string cmp_id { get; set; }
        public int dtl_line { get; set; }
        public int ctn_line { get; set; }
        public int file_count { get; set; }
        public string File_Length { get; set; }
        public int StyleStatus { get; set; }
        public string user_id { get; set; }
        public string StatusDesc { get; set; } 
        public string Itm_Code { get; set; }
        public string HeaderInfo { get; set; }
        public string eta_date { get; set; }
        public string ref_num { get; set; }
        public string rcvd_via { get; set; }
        public string rcvd_from { get; set; }
        public string master_bol { get; set; }
        public string vessel_no { get; set; }
        public string hdr_notes { get; set; }
        public string cntr_id { get; set; }
        public string po_num { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string itm_name { get; set; }
        public int itm_qty { get; set; }
        public int ctn_qty { get; set; }
        public int ctns { get; set; }
        public string loc_id { get; set; }
        public string st_rate_id { get; set; }
        public string io_rate_id { get; set; }
        public decimal ctn_length { get; set; }
        public decimal ctn_width { get; set; }
        public decimal ctn_height { get; set; }
        public decimal ctn_cube { get; set; }
        public decimal ctn_wgt { get; set; }
        public bool error_mode { get; set; }
        public string error_desc { get; set; }
        public string dtl_notes { get; set; }
        public string file_name { get; set; }
        public string ib_cntr_upld_doc_id { get; set; }
        public string entry_dt { get; set; }
        public string vend_id { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<eComIB943Upload> ListeCom940IB943UploadHdr { get; set; }
        public IList<eComIB943Upload> ListeCom940IB943UploadDtl { get; set; }
        public IList<eComIB943Upload> CheckExistStyle { get; set; }
        public IList<eComIB943Upload> ListeCom940IBDtl { get; set; }
        public IList<eComIB943Upload> ListeCom940IBUploadDtlRpt { get; set; }
        public IList<Company> ListCompanyAddresHdrDtls { get; set; }

        public IList<eComIB943Upload> ListAddECom940IBUpload { get; set; }
        public IList<eComIB943Upload> ListCheckExistIBUploadFile { get; set; }
        public IList<eComIB943Upload> ListEcomError { get; set; }
        public IList<eComIB943Upload> ListEcombatchError { get; set; }
        public IList<eComIB943Upload> ListNo_Of_Records { get; set; }
        public IList<eComIB943Upload> ListGetIBDOCID { get; set; }
        public IList<eComIB943Upload> ListGetIBDOCIDTemp { get; set; }
    }
}
