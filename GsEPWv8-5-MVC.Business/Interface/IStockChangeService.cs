using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface IStockChangeService
    {
        StockChange GetStockChangeDetails(StockChange objStockChange);
        StockChange fnGetStockForMoveByIbDocId(StockChange objStockChange);
        StockChange GetItemMoveGridLoadItem(StockChange objStockChange);
        StockChange GetItemMoveTotQty(StockChange objStockChange);
        StockChange CheckLotStatus(StockChange objStockChange);
        StockChange GetAdjustDocID(StockChange objStockChange);
        StockChange GetPalletId(StockChange objStockChange);
        StockChange Ck_Loc_Itm_Avail(StockChange objStockChange);
        StockChange Update_Rcvd_Qty(StockChange objStockChange);
        StockChange Add_Iv_Itm_Trn_Hdr(StockChange objStockChange);
        StockChange UPD_TRNIN_TRNHST_PART_TPW(StockChange objStockChange);
        StockChange UPD_LOTDTL_STK_XFER_PART_TPW(StockChange objStockChange);
        StockChange Validate_ItmCode(StockChange objStockChange);
        StockChange InsertTempStkMove(StockChange objStockChange);
        StockChange ItemXGetLocDetails(string term, string cmp_id);
     //   StockChange SaveStkMove(StockChange objStockChange);
        string SaveStkMove(string p_str_cmp_id, string p_str_user_id, DataTable p_dt_item_stock_move);
        StockChange UpdateTempStkMove(StockChange objStockChange);

    }
}
