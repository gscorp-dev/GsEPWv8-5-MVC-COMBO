using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class ClsOBAlocEdit
    {
        private string _cmp_id;
        private string _aloc_doc_id;
        private string _sr_no;
        private string _action_flag;
        private clsOBAlocHdr _objOBAlocHdr;
        private clsOBAlocDtl _objOBAlocDtl;
        private clsOBAlocCtn _objOBAlocCtn;
        private clsAlocCtnByLoc _objAlocCtnByLoc;
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string aloc_doc_id { get { return _aloc_doc_id; } set { _aloc_doc_id = value; } }
        public string action_flag { get { return _action_flag; } set { _action_flag = value; } }
        public string sr_no { get { return _sr_no; } set { _sr_no = value; } }
        public clsOBAlocHdr objOBAlocHdr { get { return _objOBAlocHdr; } set { _objOBAlocHdr = value; } }
        public clsOBAlocDtl objBAlocDtl { get { return _objOBAlocDtl; } set { _objOBAlocDtl = value; } }
        public clsOBAlocCtn objOBAlocCtn { get { return _objOBAlocCtn; } set { _objOBAlocCtn = value; } }
        public clsAlocCtnByLoc objAlocCtnByLoc { get { return _objAlocCtnByLoc; } set { _objAlocCtnByLoc = value; } }
        public IList<clsOBAlocHdr> lstOBAlocHdr { get; set; }
        public IList<clsOBAlocDtl> lstOBAlocDtl { get; set; }
        public IList<clsOBAlocCtn> lstOBAlocCtn { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList <clsOBAlocSmry> lstOBAlocSmry { get; set; }
        public IList <clsOBAlocCtnByLine> lstOBAlocCtnByLine { get; set; }


    }

    public class clsOBAlocHdr

    {
        #region Constructors  
        public clsOBAlocHdr() { }
        #endregion
        #region Private Fields  
        private string _cmp_id;
        private string _aloc_doc_id;
        private string _aloc_dt;
        private string _so_num;
        private string _so_dt;
        private string _aloc_type;
        private string _status;
        private string _price_tkt;
        private string _ship_dt;
        private string _cancel_dt;
        private string _cust_id;
        private string _cust_name;
        private string _cust_ordr_num;
        private string _cust_ordr_dt;
        private string _cntr_num;
        private string _so_info;
        private string _whs_id;
        private string _note;
        private string _ship_to_id;
        private string _ship_to_name;
        private string _process_id;
        private string _aloc_post_dt;
        #endregion
        #region Public Properties  
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string aloc_doc_id { get { return _aloc_doc_id; } set { _aloc_doc_id = value; } }
        public string aloc_dt { get { return _aloc_dt; } set { _aloc_dt = value; } }
        public string so_num { get { return _so_num; } set { _so_num = value; } }
        public string so_dt { get { return _so_dt; } set { _so_dt = value; } }
        public string aloc_type { get { return _aloc_type; } set { _aloc_type = value; } }
        public string status { get { return _status; } set { _status = value; } }
        public string price_tkt { get { return _price_tkt; } set { _price_tkt = value; } }
        public string ship_dt { get { return _ship_dt; } set { _ship_dt = value; } }
        public string cancel_dt { get { return _cancel_dt; } set { _cancel_dt = value; } }
        public string cust_id { get { return _cust_id; } set { _cust_id = value; } }
        public string cust_name { get { return _cust_name; } set { _cust_name = value; } }
        public string cust_ordr_num { get { return _cust_ordr_num; } set { _cust_ordr_num = value; } }
        public string cust_ordr_dt { get { return _cust_ordr_dt; } set { _cust_ordr_dt = value; } }
        public string cntr_num { get { return _cntr_num; } set { _cntr_num = value; } }
        public string so_info { get { return _so_info; } set { _so_info = value; } }
        public string whs_id { get { return _whs_id; } set { _whs_id = value; } }
        public string note { get { return _note; } set { _note = value; } }
        public string ship_to_id { get { return _ship_to_id; } set { _ship_to_id = value; } }
        public string ship_to_name { get { return _ship_to_name; } set { _ship_to_name = value; } }
        public string process_id { get { return _process_id; } set { _process_id = value; } }
        public string aloc_post_dt { get { return _aloc_post_dt; } set { _aloc_post_dt = value; } }
        #endregion
    }
   

    public class clsOBAlocDtl
    {
        #region Constructors  
        public clsOBAlocDtl() { }
        #endregion
        #region Private Fields  
        private string _cmp_id;
        private string _aloc_doc_id;
        private int _line_num;
        private int _itm_line;
        private string _itm_code;
        private string _status;
        private string _cust_id;
        private string _so_num;
        private string _so_itm_code;
        private string _so_itm_num;
        private string _so_itm_color;
        private string _so_itm_size;
        private int _so_dtl_line;
        private int _so_due_line;
        private string _ship_to;
        private int _aloc_ctns;
        private int _aloc_qty;
        private int _due_qty;
        private string _aloc_uom;
        private int _pick_ctns;
        private int _pick_qty;
        private string _pick_uom;
        private string _note;
        private string _process_id;
        #endregion
        #region Public Properties  
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string aloc_doc_id { get { return _aloc_doc_id; } set { _aloc_doc_id = value; } }
        public int line_num { get { return _line_num; } set { _line_num = value; } }
        public int itm_line { get { return _itm_line; } set { _itm_line = value; } }
        public string itm_code { get { return _itm_code; } set { _itm_code = value; } }
        public string status { get { return _status; } set { _status = value; } }
        public string cust_id { get { return _cust_id; } set { _cust_id = value; } }
        public string so_num { get { return _so_num; } set { _so_num = value; } }
        public string so_itm_code { get { return _so_itm_code; } set { _so_itm_code = value; } }
        public string so_itm_num { get { return _so_itm_num; } set { _so_itm_num = value; } }
        public string so_itm_color { get { return _so_itm_color; } set { _so_itm_color = value; } }
        public string so_itm_size { get { return _so_itm_size; } set { _so_itm_size = value; } }
        public int so_dtl_line { get { return _so_dtl_line; } set { _so_dtl_line = value; } }
        public int so_due_line { get { return _so_due_line; } set { _so_due_line = value; } }
        public string ship_to { get { return _ship_to; } set { _ship_to = value; } }
        public int aloc_ctns { get { return _aloc_ctns; } set { _aloc_ctns = value; } }
        public int aloc_qty { get { return _aloc_qty; } set { _aloc_qty = value; } }
        public int due_qty { get { return _due_qty; } set { _due_qty = value; } }
        public string aloc_uom { get { return _aloc_uom; } set { _aloc_uom = value; } }
        public int pick_ctns { get { return _pick_ctns; } set { _pick_ctns = value; } }
        public int pick_qty { get { return _pick_qty; } set { _pick_qty = value; } }
        public string pick_uom { get { return _pick_uom; } set { _pick_uom = value; } }
        public string note { get { return _note; } set { _note = value; } }
        public string process_id { get { return _process_id; } set { _process_id = value; } }
        #endregion  }
    }

    public class clsOBAlocCtn
    {
        #region Constructors  
        public clsOBAlocCtn() { }
        #endregion
        #region Private Fields  
        private string _cmp_id;
        private string _aloc_doc_id;
        private int _line_num;
        private int _itm_line;
        private int _ctn_line;
        private string _itm_code;
        private string _itm_num;
        private string _itm_color;
        private string _itm_size;
        private string _so_num;
        private int _so_line_num;
        private int _so_due_line;
        private int _so_itm_line;
        private string _lot_id;
        private string _po_num;
        private DateTime _rcvd_dt;
        private string _whs_id;
        private string _loc_id;
        private string _palet_id;
        private string _pkg_type;
        private int _ctn_qty;
        private int _itm_qty;
        private int _ctn_cnt;
        private int _line_qty;
        private string _qty_uom;
        private int _pick_qty;
        private string _pick_uom;
        private int _loc_bal_qty;
        private string _process_id;
        #endregion
        #region Public Properties  
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string aloc_doc_id { get { return _aloc_doc_id; } set { _aloc_doc_id = value; } }
        public int line_num { get { return _line_num; } set { _line_num = value; } }
        public int itm_line { get { return _itm_line; } set { _itm_line = value; } }
        public int ctn_line { get { return _ctn_line; } set { _ctn_line = value; } }
        public string itm_code { get { return _itm_code; } set { _itm_code = value; } }
        public string itm_num { get { return _itm_num; } set { _itm_num = value; } }
        public string itm_color { get { return _itm_color; } set { _itm_color = value; } }
        public string itm_size { get { return _itm_size; } set { _itm_size = value; } }
        public string so_num { get { return _so_num; } set { _so_num = value; } }
        public int so_line_num { get { return _so_line_num; } set { _so_line_num = value; } }
        public int so_due_line { get { return _so_due_line; } set { _so_due_line = value; } }
        public int so_itm_line { get { return _so_itm_line; } set { _so_itm_line = value; } }
        public string lot_id { get { return _lot_id; } set { _lot_id = value; } }
        public string po_num { get { return _po_num; } set { _po_num = value; } }
        public DateTime rcvd_dt { get { return _rcvd_dt; } set { _rcvd_dt = value; } }
        public string whs_id { get { return _whs_id; } set { _whs_id = value; } }
        public string loc_id { get { return _loc_id; } set { _loc_id = value; } }
        public string palet_id { get { return _palet_id; } set { _palet_id = value; } }
        public string pkg_type { get { return _pkg_type; } set { _pkg_type = value; } }
        public int ctn_qty { get { return _ctn_qty; } set { _ctn_qty = value; } }
        public int itm_qty { get { return _itm_qty; } set { _itm_qty = value; } }
        public int ctn_cnt { get { return _ctn_cnt; } set { _ctn_cnt = value; } }
        public int line_qty { get { return _line_qty; } set { _line_qty = value; } }
        public string qty_uom { get { return _qty_uom; } set { _qty_uom = value; } }
        public int pick_qty { get { return _pick_qty; } set { _pick_qty = value; } }
        public string pick_uom { get { return _pick_uom; } set { _pick_uom = value; } }
        public int loc_bal_qty { get { return _loc_bal_qty; } set { _loc_bal_qty = value; } }
        public string process_id { get { return _process_id; } set { _process_id = value; } }
        #endregion  }

    }
    public class clsOBAlocCtnByLine
    {
        #region Constructors  
        public clsOBAlocCtnByLine() { }
        #endregion
        #region Private Fields  
        private string _cmp_id;
        private string _whs_id;
        private string _aloc_doc_id;
        private int _line_num;
        private string _so_num;
        private int _so_line_num;
        private string _itm_code;
        private string _itm_num;
        private string _itm_color;
        private string _itm_size;
        private string _lot_id;
        private string _rcvd_dt;
        private string _po_num;
        private string _palet_id;
        private string _loc_id;
        private int _tot_line_ctns;
        private int _tot_line_qty;
        private int _itm_qty;
        private int _avail_cnts;
        private int _avail_qty;
        private int _aloc_cnts;
        private int _aloc_qty;
        private int _split_qty;


        #endregion
        #region Public Properties  
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string whs_id { get { return _whs_id; } set { _whs_id = value; } }
        public string aloc_doc_id { get { return _aloc_doc_id; } set { _aloc_doc_id = value; } }
        public int line_num { get { return _line_num; } set { _line_num = value; } }
        public string so_num { get { return _so_num; } set { _so_num = value; } }
        public int so_line_num { get { return _so_line_num; } set { _so_line_num = value; } }
        public string itm_code { get { return _itm_code; } set { _itm_code = value; } }
        public string itm_num { get { return _itm_num; } set { _itm_num = value; } }
        public string itm_color { get { return _itm_color; } set { _itm_color = value; } }
        public string itm_size { get { return _itm_size; } set { _itm_size = value; } }
        public string loc_id { get { return _loc_id; } set { _loc_id = value; } }
        public string lot_id { get { return _lot_id; } set { _lot_id = value; } }
        public string rcvd_dt { get { return _rcvd_dt; } set { _rcvd_dt = value; } }
        public string palet_id { get { return _palet_id; } set { _palet_id = value; } }
        public string po_num { get { return _po_num; } set { _po_num = value; } }
        public int tot_line_ctns { get { return _tot_line_ctns; } set { _tot_line_ctns = value; } }
        public int tot_line_qty { get { return _tot_line_qty; } set { _tot_line_qty = value; } }
        public int itm_qty { get { return _itm_qty; } set { _itm_qty = value; } }
        public int avail_ctns { get { return _avail_cnts; } set { _avail_cnts = value; } }
        public int avail_qty { get { return _avail_qty; } set { _avail_qty = value; } }
        public int aloc_ctns { get { return _aloc_cnts; } set { _aloc_cnts = value; } }
        public int aloc_qty { get { return _aloc_qty; } set { _aloc_qty = value; } }
        public int split_qty { get { return _split_qty; } set { _split_qty = value; } }
        

        #endregion  }

    }
    public class clsOBAlocSmry
    {
        #region Constructors  
        public clsOBAlocSmry() { }
        #endregion
        #region Private Fields  
        private string _cmp_id;
        private string _aloc_doc_id;
        private int _line_num;
        private string _so_num;
        private int _so_dtl_line; 
        private string _status;
        private string _itm_code;
        private string _itm_num;
        private string _itm_color;
        private string _itm_size;
        private string _itm_name;
        private int _due_qty;
        private int _aloc_ctns;
        private int _aloc_qty;
        private int _bo_qty;

        #endregion
        #region Public Properties  
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string aloc_doc_id { get { return _aloc_doc_id; } set { _aloc_doc_id = value; } }
        public int line_num { get { return _line_num; } set { _line_num = value; } }
        public string status { get { return _status; } set { _status = value; } }
        public string so_num { get { return _so_num; } set { _so_num = value; } }
        public int so_dtl_line { get { return _so_dtl_line; } set { _so_dtl_line = value; } }
       
        public string itm_code { get { return _itm_code; } set { _itm_code = value; } }
        public string itm_num { get { return _itm_num; } set { _itm_num = value; } }
        public string itm_color { get { return _itm_color; } set { _itm_color = value; } }
        public string itm_size { get { return _itm_size; } set { _itm_size = value; } }
        public string itm_name { get { return _itm_name; } set { _itm_name = value; } }
        public int due_qty { get { return _due_qty; } set { _due_qty = value; } }
        public int aloc_ctns { get { return _aloc_ctns; } set { _aloc_ctns = value; } }
        public int aloc_qty { get { return _aloc_qty; } set { _aloc_qty = value; } }
        public int bo_qty { get { return _bo_qty; } set { _bo_qty = value; } }

        #endregion  }
    }

    public class clsAlocCtnByLoc
    {
        private string _po_num = string.Empty;
        private int _itm_qty;
        private int _split_qty;
        public string ref_num { get; set; }
        public string cmp_id { get; set; }
        public string whs_id { get; set; }
        public string aloc_doc_id { get; set; }
        public string so_num { get; set; }
        public int line_num { get; set; }
        public int so_line_num { get; set; }
        public int ctn_line { get; set; }
        public string itm_code { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string loc_id { get; set; }
        public string lot_id { get; set; }
        public string rcvd_dt { get; set; }
        public string palet_id { get; set; }
        public string po_num { get { return _po_num; } set { _po_num = value; } }
        public int itm_qty { get { return _itm_qty; } set { _itm_qty = value; } }
        public int old_aloc_ctns { get; set; }
        public int aloc_ctns { get; set; }
        public int split_qty { get { return _split_qty; } set { _split_qty = value; } }



    }

}