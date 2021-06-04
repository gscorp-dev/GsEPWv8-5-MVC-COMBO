using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class cls_ob_940_sbs
    {

        
        public cls_ob_940_sbs_hdr _objOB940SBShdr;
        public cls_ob_940_sbs_hdr objOB940SBShdr { get { return _objOB940SBShdr; } set { _objOB940SBShdr = value; } }
        public IList<cls_ob_940_sbs_hdr> lstOB940SBSHdr { get; set; }
        public IList<cls_ob_940_sbs_dtl> lstOB940SBSDtl { get; set; }
        public IList<cls_ob_940_sbs_notes> lstOB940SBSNotes { get; set; }
    }
    public class cls_ob_940_sbs_hdr
    {
        private string _cmp_id;
        private string _ref_num;
        private string _po_num;
        private string _release_no;
        private string _po_dt;
        private string _dept_no;
        private string _retailer_po_num;
        private string _req_delivery_dt;
        private string _ship_no_later;
        private string _req_shipment_dt;
        private string _carrier;
        private string _carrier_desc;
        private string _ship_to_loc;
        private string _rec_type;
        private string _po_purpose;
        private string _po_type;
        private string _contact_no;
        private string _ccy;
        private string _ship_status;
        private string _ltr_of_cr;
        private string _vend_id;
        private string _div_id;
        private string _cust_acc;
        private string _cust_ordr_num;
        private string _promo_num;
        private string _tkt_desc;
        private string _other_info;
        private string _frt_terms;
        private string _carier_ser_level;
        private string _pmt_terms_percent;
        private string _pmt_terms_due_date;
        private string _pmt_terms_days_due;
        private string _pmt_terms_net_due_date;
        private string _pmt_terms_net_days_due;
        private string _pmt_terms_desc_amt;
        private string _pmt_term_desc;
        private string _phone_no;
        private string _fax_no;
        private string _email_id;
        private string _chrge_type;
        private string _chrge_srvc;
        private string _chrge_amt;
        private string _chrge_percent;
        private string _chrge_rate;
        private string _chrge_qty;
        private string _chrge_desc;
        private string _ship_to_name;
        private string _ship_to_addr1;
        private string _ship_to_addr2;
        private string _ship_to_city;
        private string _ship_to_state;
        private string _ship_to_zip;
        private string _ship_to_cntry;
        private string _ship_to_contact;
        private string _bill_to_name;
        private string _bill_to_addr1;
        private string _bill_to_addr2;
        private string _bill_to_city;
        private string _bill_to_state;
        private string _bill_to_zip;
        private string _bill_to_cntry;
        private string _bill_to_contact;
        private string _buyer_name;
        private string _buyer_loc;
        private string _buyer_addr1;
        private string _buyer_addr2;
        private string _buyer_city;
        private string _buyer_state;
        private string _buyer_zip;
        private string _buyer_cntry;
        private string _buyer_contact;
        private string _ultimate_loc;
        private string _ship_to_additional_name1;
        private string _ship_to_additional_name2;
        private string _bill_to_additional_name1;
        private string _bill_to_additional_name2;
        private string _buyer_additional_name1;
        private string _buyer_additional_name2;
        private string _notes1;
        private string _notes2;

        private int _line_num;
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string ref_num { get { return _ref_num; } set { _ref_num = value; } }
        public string po_num { get { return _po_num; } set { _po_num = value; } }
        public string release_no { get { return _release_no; } set { _release_no = value; } }
        public string po_dt { get { return _po_dt; } set { _po_dt = value; } }
        public string dept_no { get { return _dept_no; } set { _dept_no = value; } }
        public string retailer_po_num { get { return _retailer_po_num; } set { _retailer_po_num = value; } }
        public string req_delivery_dt { get { return _req_delivery_dt; } set { _req_delivery_dt = value; } }
        public string ship_no_later { get { return _ship_no_later; } set { _ship_no_later = value; } }
        public string req_shipment_dt { get { return _req_shipment_dt; } set { _req_shipment_dt = value; } }
        public string carrier { get { return _carrier; } set { _carrier = value; } }
        public string carrier_desc { get { return _carrier_desc; } set { _carrier_desc = value; } }
        public string ship_to_loc { get { return _ship_to_loc; } set { _ship_to_loc = value; } }
        public string rec_type { get { return _rec_type; } set { _rec_type = value; } }
        public string po_purpose { get { return _po_purpose; } set { _po_purpose = value; } }
        public string po_type { get { return _po_type; } set { _po_type = value; } }
        public string contact_no { get { return _contact_no; } set { _contact_no = value; } }
        public string ccy { get { return _ccy; } set { _ccy = value; } }
        public string ship_status { get { return _ship_status; } set { _ship_status = value; } }
        public string ltr_of_cr { get { return _ltr_of_cr; } set { _ltr_of_cr = value; } }
        public string vend_id { get { return _vend_id; } set { _vend_id = value; } }
        public string div_id { get { return _div_id; } set { _div_id = value; } }
        public string cust_acc { get { return _cust_acc; } set { _cust_acc = value; } }
        public string cust_ordr_num { get { return _cust_ordr_num; } set { _cust_ordr_num = value; } }
        public string promo_num { get { return _promo_num; } set { _promo_num = value; } }
        public string tkt_desc { get { return _tkt_desc; } set { _tkt_desc = value; } }
        public string other_info { get { return _other_info; } set { _other_info = value; } }
        public string frt_terms { get { return _frt_terms; } set { _frt_terms = value; } }
        public string carier_ser_level { get { return _carier_ser_level; } set { _carier_ser_level = value; } }
        public string pmt_terms_percent { get { return _pmt_terms_percent; } set { _pmt_terms_percent = value; } }
        public string pmt_terms_due_date { get { return _pmt_terms_due_date; } set { _pmt_terms_due_date = value; } }
        public string pmt_terms_days_due { get { return _pmt_terms_days_due; } set { _pmt_terms_days_due = value; } }
        public string pmt_terms_net_due_date { get { return _pmt_terms_net_due_date; } set { _pmt_terms_net_due_date = value; } }
        public string pmt_terms_net_days_due { get { return _pmt_terms_net_days_due; } set { _pmt_terms_net_days_due = value; } }
        public string pmt_terms_desc_amt { get { return _pmt_terms_desc_amt; } set { _pmt_terms_desc_amt = value; } }
        public string pmt_term_desc { get { return _pmt_term_desc; } set { _pmt_term_desc = value; } }
        public string phone_no { get { return _phone_no; } set { _phone_no = value; } }
        public string fax_no { get { return _fax_no; } set { _fax_no = value; } }
        public string email_id { get { return _email_id; } set { _email_id = value; } }
        public string chrge_type { get { return _chrge_type; } set { _chrge_type = value; } }
        public string chrge_srvc { get { return _chrge_srvc; } set { _chrge_srvc = value; } }
        public string chrge_amt { get { return _chrge_amt; } set { _chrge_amt = value; } }
        public string chrge_percent { get { return _chrge_percent; } set { _chrge_percent = value; } }
        public string chrge_rate { get { return _chrge_rate; } set { _chrge_rate = value; } }
        public string chrge_qty { get { return _chrge_qty; } set { _chrge_qty = value; } }
        public string chrge_desc { get { return _chrge_desc; } set { _chrge_desc = value; } }
        public string ship_to_name { get { return _ship_to_name; } set { _ship_to_name = value; } }
        public string ship_to_addr1 { get { return _ship_to_addr1; } set { _ship_to_addr1 = value; } }
        public string ship_to_addr2 { get { return _ship_to_addr2; } set { _ship_to_addr2 = value; } }
        public string ship_to_city { get { return _ship_to_city; } set { _ship_to_city = value; } }
        public string ship_to_state { get { return _ship_to_state; } set { _ship_to_state = value; } }
        public string ship_to_zip { get { return _ship_to_zip; } set { _ship_to_zip = value; } }
        public string ship_to_cntry { get { return _ship_to_cntry; } set { _ship_to_cntry = value; } }
        public string ship_to_contact { get { return _ship_to_contact; } set { _ship_to_contact = value; } }
        public string bill_to_name { get { return _bill_to_name; } set { _bill_to_name = value; } }
        public string bill_to_addr1 { get { return _bill_to_addr1; } set { _bill_to_addr1 = value; } }
        public string bill_to_addr2 { get { return _bill_to_addr2; } set { _bill_to_addr2 = value; } }
        public string bill_to_city { get { return _bill_to_city; } set { _bill_to_city = value; } }
        public string bill_to_state { get { return _bill_to_state; } set { _bill_to_state = value; } }
        public string bill_to_zip { get { return _bill_to_zip; } set { _bill_to_zip = value; } }
        public string bill_to_cntry { get { return _bill_to_cntry; } set { _bill_to_cntry = value; } }
        public string bill_to_contact { get { return _bill_to_contact; } set { _bill_to_contact = value; } }
        public string buyer_name { get { return _buyer_name; } set { _buyer_name = value; } }
        public string buyer_loc { get { return _buyer_loc; } set { _buyer_loc = value; } }
        public string buyer_addr1 { get { return _buyer_addr1; } set { _buyer_addr1 = value; } }
        public string buyer_addr2 { get { return _buyer_addr2; } set { _buyer_addr2 = value; } }
        public string buyer_city { get { return _buyer_city; } set { _buyer_city = value; } }
        public string buyer_state { get { return _buyer_state; } set { _buyer_state = value; } }
        public string buyer_zip { get { return _buyer_zip; } set { _buyer_zip = value; } }
        public string buyer_cntry { get { return _buyer_cntry; } set { _buyer_cntry = value; } }
        public string buyer_contact { get { return _buyer_contact; } set { _buyer_contact = value; } }
        public string ultimate_loc { get { return _ultimate_loc; } set { _ultimate_loc = value; } }
        public string ship_to_additional_name1 { get { return _ship_to_additional_name1; } set { _ship_to_additional_name1 = value; } }
        public string ship_to_additional_name2 { get { return _ship_to_additional_name2; } set { _ship_to_additional_name2 = value; } }
        public string bill_to_additional_name1 { get { return _bill_to_additional_name1; } set { _bill_to_additional_name1 = value; } }
        public string bill_to_additional_name2 { get { return _bill_to_additional_name2; } set { _bill_to_additional_name2 = value; } }
        public string buyer_additional_name1 { get { return _buyer_additional_name1; } set { _buyer_additional_name1 = value; } }
        public string buyer_additional_name2 { get { return _buyer_additional_name2; } set { _buyer_additional_name2 = value; } }
        public string notes1 { get { return _notes1; } set { _notes1 = value; } }
        public string notes2 { get { return _notes2; } set { _notes2 = value; } }
 
        public int line_num { get { return _line_num; } set { _line_num = value; } }
        
    }
    public class cls_ob_940_sbs_notes
    {
        private string _cmp_id;
        private string _ref_num;
        private string _po_num;
        private string _comments;
        private int _line_num;
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string ref_num { get { return _ref_num; } set { _ref_num = value; } }
        public string po_num { get { return _po_num; } set { _po_num = value; } }
        public string comments { get { return _comments; } set { _comments = value; } }
        public int line_num { get { return _line_num; } set { _line_num = value; } }
    }
    public class cls_ob_940_sbs_dtl
    {
        private string _cmp_id;
        private string _ref_num;
        private string _po_num;
        private int _po_line_num;
        private int _ordr_qty;
        private string _uom;
        private string _unit_price;
        private string _byer_catalog_or_sku;
        private string _upc_ean;
        private string _itm_num;
        private string _retail_price;
        private string _itm_name;
        private string _itm_color;
        private string _itm_size;
        private string _pack_size;
        private string _pack_size_uom;
        private string _no_of_inner_packs;
        private string _pcs_per_inner_packs;
        private string _store_id;
        private string _qty_per_store;
        private string _sku_num;
        private string _buyer_catlog_num;
        private int _line_num;
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string ref_num { get { return _ref_num; } set { _ref_num = value; } }
        public string po_num { get { return _po_num; } set { _po_num = value; } }

        public int po_line_num { get { return _po_line_num; } set { _po_line_num = value; } }
        public int ordr_qty { get { return _ordr_qty; } set { _ordr_qty = value; } }
        public string uom { get { return _uom; } set { _uom = value; } }
        public string unit_price { get { return _unit_price; } set { _unit_price = value; } }
        public string byer_catalog_or_sku { get { return _byer_catalog_or_sku; } set { _byer_catalog_or_sku = value; } }
        public string upc_ean { get { return _upc_ean; } set { _upc_ean = value; } }
        public string itm_num { get { return _itm_num; } set { _itm_num = value; } }
        public string retail_price { get { return _retail_price; } set { _retail_price = value; } }
        public string itm_name { get { return _itm_name; } set { _itm_name = value; } }
        public string itm_color { get { return _itm_color; } set { _itm_color = value; } }
        public string itm_size { get { return _itm_size; } set { _itm_size = value; } }
        public string pack_size { get { return _pack_size; } set { _pack_size = value; } }
        public string pack_size_uom { get { return _pack_size_uom; } set { _pack_size_uom = value; } }
        public string no_of_inner_packs { get { return _no_of_inner_packs; } set { _no_of_inner_packs = value; } }
        public string pcs_per_inner_packs { get { return _pcs_per_inner_packs; } set { _pcs_per_inner_packs = value; } }
        public string store_id { get { return _store_id; } set { _store_id = value; } }
        public string qty_per_store { get { return _qty_per_store; } set { _qty_per_store = value; } }
        public string sku_num { get { return _sku_num; } set { _sku_num = value; } }
        public string buyer_catlog_num { get { return _buyer_catlog_num; } set { _buyer_catlog_num = value; } }
        public int line_num { get { return _line_num; } set { _line_num = value; } }
      
    }
}