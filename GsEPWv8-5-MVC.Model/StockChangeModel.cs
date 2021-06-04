using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
    public class StockChangeModel : BasicEntity
    {
        public string cmp_id { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string Itmdtl { get; set; }
        public string itm_name { get; set; }
        public string itm_code { get; set; }
        public string whs_id { get; set; }
        public string loc_id { get; set; }
        public string ib_doc_id { get; set; }
        public string lot_id { get; set; }
        public string po_num { get; set; }
        public string cont_id { get; set; }
        public string rcvd_dt_fm { get; set; }
        public string rcvd_dt_to { get; set; }
        public string rcvd_dt { get; set; }
        public string search_text { get; set; }

        public string status { get; set; }
        public string kit_id { get; set; }
        public string palet_id { get; set; }
        public string pkg_type { get; set; }
        public int pkg_qty { get; set; }
        public int itm_qty { get; set; }
        public string doc_notes { get; set; }
        public string doc_id { get; set; }
        public int avail_cnt { get; set; }
        public int AvlQty { get; set; }
        public int avail_qty { get; set; }
        public string date { get; set; }
        public string frm_loc { get; set; }
        public string to_loc { get; set; }
        public int row_ctn { get; set; }
        public int move_ctns { get; set; }
        public int tot_ctn { get; set; }
        public int tot_qty { get; set; }
        public int TotCtns { get; set; }
        public int TotQty { get; set; }
        public string lot_num { get; set; }
        public string pkg_id { get; set; }
        public string colChk { get; set; }
        public int LineNum { get; set; }
        public int move_qty { get; set; }
        public string adj_doc_id { get; set; }
        public int cnt { get; set; }
        public string kit_itm { get; set; }
        public string optid { get; set; }
        public int tot_ctns { get; set; }
        public string adj_date { get; set; }
        public string userId { get; set; }
        public string Toloc { get; set; }
        public string ErrorDesc { get; set; }
        public string p_str_company { get; set; }
        public string p_str_cmpid { get; set; }
        public string tmp_cmp_id { get; set; }

        public bool move_all { get; set; }
        public int move_all_qty { get; set; }
        public int move_all_ctns { get; set; }
        public string move_all_to_loc_id { get; set; }

        public IList<StockChangeModel> ListGetStockChangeDetails { get; set; }
        public IList<StockChangeModel>  ListGetItemMoveTotQty { get; set; }
        public IList<StockChangeModel> ListCheckLotStatus { get; set; }
        public IList<LookUp> ListLookUpDtl { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<Company> ListLocPickDtl { get; set; }
        public IList<StockChangeModel> ListGetItemMoveDetails { get; set; }
        public IList<LookUp> ListLookUpCategoryDtl { get; set; }
        public IList<LookUp> ListLookUpStatusDtl { get; set; }
        public IList<StockChange> ListGetAdjustDocID { get; set; }
        public IList<StockChange> ListPaletId { get; set; }
        public IList<StockChange> LstSaveItmMove { get; set; }
        public IList<StockChange> LstItmxlocdtl { get; set; }


        

    }
}
