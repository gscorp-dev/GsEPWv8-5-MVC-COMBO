using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class cls_temp_iv_stk_move
    {

        #region Constructors  
        public cls_temp_iv_stk_move() { }
        #endregion

        #region Private Fields  
        private string _cmp_id;
        private string _adj_doc_id;
        private string _adj_dt;
        private string _itm_code;
        private string _whs_id;
        private string _ib_doc_id;
        private string _rcvd_dt;
        private string _cont_id;
        private string _lot_id;
        private string _palet_id;
        private string _po_num;
        private string _loc_id_from;
        private object _ctn_qty;
        private object _tot_ctns;
        private string _loc_id_to;
        private object _move_ctns;
        private string _user_id;
        private string _process_id;
        #endregion

        #region Public Properties  
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string adj_doc_id { get { return _adj_doc_id; } set { _adj_doc_id = value; } }
        public string adj_dt { get { return _adj_dt; } set { _adj_dt = value; } }
        public string itm_code { get { return _itm_code; } set { _itm_code = value; } }
        public string whs_id { get { return _whs_id; } set { _whs_id = value; } }
        public string ib_doc_id { get { return _ib_doc_id; } set { _ib_doc_id = value; } }
        public string rcvd_dt { get { return _rcvd_dt; } set { _rcvd_dt = value; } }
        public string cont_id { get { return _cont_id; } set { _cont_id = value; } }
        public string lot_id { get { return _lot_id; } set { _lot_id = value; } }
        public string palet_id { get { return _palet_id; } set { _palet_id = value; } }
        public string po_num { get { return _po_num; } set { _po_num = value; } }
        public string loc_id_from { get { return _loc_id_from; } set { _loc_id_from = value; } }
        public object ctn_qty { get { return _ctn_qty; } set { _ctn_qty = value; } }
        public object tot_ctns { get { return _tot_ctns; } set { _tot_ctns = value; } }
        public string loc_id_to { get { return _loc_id_to; } set { _loc_id_to = value; } }
        public object move_ctns { get { return _move_ctns; } set { _move_ctns = value; } }
        public string user_id { get { return _user_id; } set { _user_id = value; } }
        public string process_id { get { return _process_id; } set { _process_id = value; } }
        #endregion

        public List<cls_temp_iv_stk_move>ListItemStockMove;

    }
    }
