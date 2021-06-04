using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
   public class StockChangeService : IStockChangeService
    {
        IStockChangeRepository objRepository = new StockChangeRepository();
        public StockChange GetStockChangeDetails(StockChange objStockChange)
        {
            return objRepository.GetStockChangeDetails(objStockChange);
        }

        public StockChange fnGetStockForMoveByIbDocId(StockChange objStockChange)
        {
            return objRepository.fnGetStockForMoveByIbDocId(objStockChange);
        }

        public StockChange GetItemMoveGridLoadItem(StockChange objStockChange)
        {
            return objRepository.GetItemMoveGridLoadItem(objStockChange);
        }
        public StockChange GetItemMoveTotQty(StockChange objStockChange)
        {
            return objRepository.GetItemMoveTotQty(objStockChange);
        }
        public StockChange CheckLotStatus(StockChange objStockChange)
        {
            return objRepository.CheckLotStatus(objStockChange);
        }
        public StockChange GetAdjustDocID(StockChange objStockChange)
        {
            return objRepository.GetAdjustDocID(objStockChange);
        }
        public StockChange GetPalletId(StockChange objStockChange)
        {
            return objRepository.GetPalletId(objStockChange);
        }
        public StockChange Ck_Loc_Itm_Avail(StockChange objStockChange)
        {
            return objRepository.Ck_Loc_Itm_Avail(objStockChange);
        }
        public StockChange Update_Rcvd_Qty(StockChange objStockChange)
        {
            return objRepository.Update_Rcvd_Qty(objStockChange);
        }
        public StockChange Add_Iv_Itm_Trn_Hdr(StockChange objStockChange)
        {
            return objRepository.Add_Iv_Itm_Trn_Hdr(objStockChange);
        }
        public StockChange UPD_TRNIN_TRNHST_PART_TPW(StockChange objStockChange)
        {
            return objRepository.UPD_TRNIN_TRNHST_PART_TPW(objStockChange);
        }
        public StockChange UPD_LOTDTL_STK_XFER_PART_TPW(StockChange objStockChange)
        {
            return objRepository.UPD_LOTDTL_STK_XFER_PART_TPW(objStockChange);
        }
        public StockChange Validate_ItmCode(StockChange objStockChange)
        {
            return objRepository.Validate_ItmCode(objStockChange);
        }
        public StockChange InsertTempStkMove(StockChange objStockChange)
        {
            return objRepository.InsertTempStkMove(objStockChange);
        }
        public StockChange ItemXGetLocDetails(string term, string cmp_id)
        {
            return objRepository.ItemXGetLocDetails(term, cmp_id);
        }
        //public StockChange SaveStkMove(StockChange objStockChange)
        //{
        //    return objRepository.SaveStkMove(objStockChange);
        //}
        public string SaveStkMove(string p_str_cmp_id, string p_str_user_id, DataTable p_dt_item_stock_move)
        {
            return objRepository.SaveStkMove(p_str_cmp_id,  p_str_user_id, p_dt_item_stock_move);
        }
        public StockChange UpdateTempStkMove(StockChange objStockChange)
        {
            return objRepository.UpdateTempStkMove(objStockChange);
        }

       
    }
}
