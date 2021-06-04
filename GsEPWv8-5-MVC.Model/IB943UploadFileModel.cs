using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
    public class IB943UploadFileHdrModel
    {
        private string _upload_ref_num;
        private string _cmp_id;
        private string _cntr_id;

        private string _ref_num;
        private string _rcvd_via;
        private string _rcvd_from;
        private string _master_bol;
        private string _vessel_no;
        private string _hdr_note;
        private string _hdr_data;
        public string upload_ref_num { get { return _upload_ref_num; } set { _upload_ref_num = value; } }
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string ib_doc_dt { get; set; }
        public string cntr_id { get { return _cntr_id; } set { _cntr_id = value; } }
        public string eta_date { get; set; }
        public string ref_num { get { return _ref_num; } set { _ref_num = value; } }
        public string rcvd_via { get { return _rcvd_via; } set { _rcvd_via = value; } }
        public string rcvd_from { get { return _rcvd_from; } set { _rcvd_from = value; } }
        public string master_bol { get { return _master_bol; } set { _master_bol = value; } }
        public string vessel_no { get { return _vessel_no; } set { _vessel_no = value; } }
        public string hdr_note { get { return _hdr_note; } set { _hdr_note = value; } }
        public string hdr_data { get { return _hdr_data; } set { _hdr_data = value; } }


    }
    public class IB943UploadFileDtlModel
    {
        private string _upload_ref_num = string.Empty;
        private string _cmp_id = string.Empty;
        private string _ib_doc_id;
        private string _cntr_id = string.Empty;
        private int _dtl_line;
        private int _line_num = 0;
        private string _po_num = string.Empty;
        private string _itm_code = string.Empty;
        private string _itm_num = string.Empty;
        private string _itm_color = string.Empty;
        private string _itm_size = string.Empty;
        private string _itm_name = string.Empty;
        private int _ordr_qty;
        private int _ctn_qty;
        private int _ordr_ctn;
        private decimal _ctn_length;
        private decimal _ctn_width;
        private decimal _ctn_height;
        private decimal _ctn_cube;
        private decimal _ctn_wgt;
        private string _loc_id = string.Empty;
        private string _st_rate_id = string.Empty;
        private string _io_rate_id = string.Empty;
        private string _dtl_note = string.Empty;
        private string _header_data = string.Empty;
        private string _error_desc = string.Empty;
        public string upload_ref_num { get { return _upload_ref_num; } set { _upload_ref_num = value; } }
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string ib_doc_id { get { return _ib_doc_id; } set { _ib_doc_id = value; } }
        public string cntr_id { get { return _cntr_id; } set { _cntr_id = value; } }
        public int dtl_line { get { return _dtl_line; } set { _dtl_line = value; } }
        public int line_num { get { return _line_num; } set { _line_num = value; } }
        public string po_num { get { return _po_num; } set { _po_num = value; } }
        public string itm_code { get { return _itm_code; } set { _itm_code = value; } }
        
        public string itm_num { get { return _itm_num; } set { _itm_num = value; } }
        public string itm_color { get { return _itm_color; } set { _itm_color = value; } }
        public string itm_size { get { return _itm_size; } set { _itm_size = value; } }
        public string itm_name { get { return _itm_name; } set { _itm_name = value; } }
        public int ordr_qty { get { return _ordr_qty; } set { _ordr_qty = value; } }
        public int ctn_qty { get { return _ctn_qty; } set { _ctn_qty = value; } }
        public int ordr_ctn { get { return _ordr_ctn; } set { _ordr_ctn = value; } }
        public decimal ctn_length { get { return _ctn_length; } set { _ctn_length = value; } }
        public decimal ctn_width { get { return _ctn_width; } set { _ctn_width = value; } }
        public decimal ctn_height { get { return _ctn_height; } set { _ctn_height = value; } }
        public decimal ctn_cube { get { return _ctn_cube; } set { _ctn_cube = value; } }
        public decimal ctn_wgt { get { return _ctn_wgt; } set { _ctn_wgt = value; } }
        public string loc_id { get { return _loc_id; } set { _loc_id = value; } }
        public string st_rate_id { get { return _st_rate_id; } set { _st_rate_id = value; } }
        public string io_rate_id { get { return _io_rate_id; } set { _io_rate_id = value; } }
        public string dtl_note { get { return _dtl_note; } set { _dtl_note = value; } }
        public string header_data { get { return _header_data; } set { _header_data = value; } }
        public string error_desc { get { return _error_desc; } set { _error_desc = value; } }
        public string factory_id { get; set; }
        public string cust_name { get; set; }
        public string cust_po_num { get; set; }
        public string pick_list { get; set; }
    }
    public class IB943InvalidDataModel
    {
        private string _cntr_id;
        private int _line_num;
        private string _line_data;
        private string _error_desc;
        public string cntr_id { get { return _cntr_id; } set { _cntr_id = value; } }
        public int line_num { get { return _line_num; } set { _line_num = value; } }
        public string line_data { get { return _line_data; } set { _line_data = value; } }
        public string error_desc { get { return _error_desc; } set { _error_desc = value; } }

    }
    public class IB943UploadFileModel
    {

        private string _upload_ref_num = string.Empty;
        private string _cmp_id = string.Empty;

        public string upload_ref_num { get { return _upload_ref_num; } set { _upload_ref_num = value; } }
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string user_id { get; set; }
        public string upload_dt_from { get; set; }
        public string upload_dt_to { get; set; }
        public string file_name { get; set; }
        public bool error_mode { get; set; }
        public string error_desc { get; set; }
        public string cust_logo_path { get; set; }
        public string email_auto_sent { get; set; }
        public int dtl_count { get; set; }
        public IList<IB943UploadFileHdr> ListIB943UploadFileHdr { get; set; }
        public IList<IB943UploadFileDtl> ListIB943UploadFileDtl { get; set; }

        public IB943UploadFileInfo objIB943UploadFileInfo;
        public IList<IB943InvalidData> ListIB943InvalidData { get; set; }
        public IList<Company> ListCompany { get; set; }
        public IList<ItemMasterdtl> ListItemMasterViewDtl { get; set; }
        public IList<IB943UploadAckRpt> ListIB943UploadAckRpt { get; set; }


    }

    public class IB943UploadAckRptSmry
    {
        public string cmp_id { get; set; }
        public string cntr_id { get; set; }
        public string ib_doc_id { get; set; }
        public int tot_ctn { get; set; }
        public int tot_qty { get; set; }
        public decimal tot_wgt { get; set; }
        public decimal tot_cube { get; set; }

    }
    public class IB943UploadAckRpt
    {
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string itm_name { get; set; }
        public string vend_id { get; set; }
        public string req_num { get; set; }
        public string cntr_id { get; set; }
        public string ib_doc_id { get; set; }
        public string ib_doc_dt { get; set; }
        public string city { get; set; }
        public string state_id { get; set; }
        public string post_code { get; set; }
        public string addr_line1 { get; set; }
        public string cmp_name { get; set; }
        public decimal wgt { get; set; }
        public decimal InbCube { get; set; }
        public int dtl_line { get; set; }
        public string po_num { get; set; }
        public int itm_qty { get; set; }
        public int tot_qty { get; set; }
        public string eta_dt { get; set; }
        public int tot_ctn { get; set; }
        public decimal length { get; set; }
        public decimal width { get; set; }
        public decimal depth { get; set; }
        public string rpt_note { get; set; }
        public int ctn_line { get; set; }
        public string cube_excp { get; set; }
        public string dup_itm_excp { get; set; }

    }
}
