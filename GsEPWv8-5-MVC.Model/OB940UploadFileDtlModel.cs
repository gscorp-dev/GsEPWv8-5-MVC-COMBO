using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
    public class OB940UploadFileDtlModel
    {
        #region Constructors  
        public OB940UploadFileDtlModel() { }
        #endregion
        #region Private Fields  
        private string _upload_ref_num;
        private string _cmp_id;
        private string _ref_num;
        private string _batch_id;
        private string _cust_id;
        private int _line_num;
        private string _cust_po_num;
        private int _pick_line_num;
        private string _itm_code;
        private string _itm_num;
        private string _itm_color;
        private string _itm_size;
        private string _itm_name;
        private string _cust_sku;
        private Int32 _ordr_qty = 0;
        private Int32 _ordr_ctns = 0;
        private Int32 _ctn_qty =0;
        private double _length = 0;
        private double _width = 0;
        private double _depth =0;
        private double _wgt = 0;
        private double _cube = 0;
        private string _header_data = string.Empty;
        private string _error_desc = string.Empty;
        #endregion
        #region Public Properties  
        public string upload_ref_num { get { return _upload_ref_num; } set { _upload_ref_num = value; } }
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string ref_num { get { return _ref_num; } set { _ref_num = value; } }
        public string batch_id { get { return _batch_id; } set { _batch_id = value; } }
        public string cust_id { get { return _cust_id; } set { _cust_id = value; } }
        public int line_num { get { return _line_num; } set { _line_num = value; } }
        public string cust_po_num { get { return _cust_po_num; } set { _cust_po_num = value; } }
        public int pick_line_num { get { return _pick_line_num; } set { _pick_line_num = value; } }
        public string itm_code { get { return _itm_code; } set { _itm_code = value; } }
        public string itm_num { get { return _itm_num; } set { _itm_num = value; } }
        public string itm_color { get { return _itm_color; } set { _itm_color = value; } }
        public string itm_size { get { return _itm_size; } set { _itm_size = value; } }
        public string itm_name { get { return _itm_name; } set { _itm_name = value; } }
        public string cust_sku { get { return _cust_sku; } set { _cust_sku = value; } }
        public Int32 ordr_qty { get { return _ordr_qty; } set { _ordr_qty = value; } }
        public Int32 ordr_ctns { get { return _ordr_ctns; } set { _ordr_ctns = value; } }
        public Int32 ctn_qty { get { return _ctn_qty; } set { _ctn_qty = value; } }
        public double length { get { return _length; } set { _length = value; } }
        public double width { get { return _width; } set { _width = value; } }
        public double depth { get { return _depth; } set { _depth = value; } }
        public double wgt { get { return _wgt; } set { _wgt = value; } }
        public double cube { get { return _cube; } set { _cube = value; } }
        public string header_data { get { return _header_data; } set { _header_data = value; } }
        public string error_desc { get { return _error_desc; } set { _error_desc = value; } }

        #endregion
    }
}
