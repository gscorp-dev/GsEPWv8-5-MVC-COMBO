using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
    public class ItemMasterService : IItemMasterService
    {
        IItemMasterRepository objRepository = new ItemMasterRepository();

        public ItemMaster fnGetItemConfig(string pstrConfigType)
        {
            return objRepository.fnGetItemConfig(pstrConfigType);
        }
        public ItemMaster fnGetItemPkgServiceg(string pstrCmpId, string pstrItmCode)
        {
            return objRepository.fnGetItemPkgServiceg(pstrCmpId, pstrItmCode);
        }
        public bool SaveSaveItemPkgServicer(string pstrMode, string pstrCmpId, string pstrItmCode, string pstrPkgService)
        {
            return objRepository.SaveSaveItemPkgServicer(pstrMode, pstrCmpId, pstrItmCode, pstrPkgService);
        }


        public ItemMaster GetItemMasterDetails(ItemMaster objItemMaster)
        {
            return objRepository.GetItemMasterDetails(objItemMaster);
        }

        public string fnAddNew940Item(ItemMaster objItemMaster)
        {
            return objRepository.fnAddNew940Item(objItemMaster);
        }
        public ItemMaster ExistItem(ItemMaster objItemMaster)
        {
            return objRepository.ExistItem(objItemMaster);
        }

        public void ItemMasterCreateUpdate(ItemMaster objItemMaster)
        {
            objRepository.ItemMasterCreateUpdate(objItemMaster);
        }
        public void ItemMasterdelete(ItemMaster objItemMaster)
        {
            objRepository.ItemMasterDeleteItem(objItemMaster);
        }

        public void ItemMasterHeaderCreateUpdate(ItemMaster objItemMaster)
        {
            objRepository.ItemMasterHeaderCreateUpdate(objItemMaster);
        }

        public ItemMaster GetItemMasterViewDetails(ItemMaster objItemMaster)
        {
            return objRepository.GetItemMasterViewDetails(objItemMaster);
        }

        public LookUp GetItemMasterCategory(LookUp objItemMaster)
        {
            return objRepository.GetItemMasterCategory(objItemMaster);
        }
        public ItemMaster GetItmId(ItemMaster objItemMaster)
        {
            return objRepository.GetItmId(objItemMaster);
        }
        public void ItemMasterKithdr(ItemMaster objItemMaster)
        {
            objRepository.ItemMasterKithdr(objItemMaster);
        }
        public void ItemMasterKitdtl(ItemMaster objItemMaster)
        {
            objRepository.ItemMasterKitdtl(objItemMaster);
        }
        public ItemMaster CheckAndDeleteItm(ItemMaster objItemMaster)
        {
            return objRepository.CheckAndDeleteItm(objItemMaster);
        }
        public ItemdtlReport GetItemDtlList(ItemdtlReport objItemdtlReport, string p_str_cmp_id)
        {
            return objRepository.GetItemDtlList(objItemdtlReport, p_str_cmp_id);
        }
        public DataTable GetItemDtlListExcelRpt(string p_str_cmp_id)
        {
            return objRepository.GetItemDtlListExcelRpt(p_str_cmp_id);
        }

        public DataTable GetItemAndStockDimRpt(string p_str_cmp_id)
        {
            return objRepository.GetItemAndStockDimRpt(p_str_cmp_id);
        }

        public bool SaveDimUpdate(string pstrCmpId, DataTable ldtDimUpdate)
        {
        return objRepository.SaveDimUpdate(pstrCmpId,ldtDimUpdate);
        }

        public List<ItemStock> getItemStock(string p_str_cmp_id, string p_str_itm_num, string p_str_itm_color, string p_str_itm_size, string pstrStkStatus)
        {
            return objRepository.getItemStock( p_str_cmp_id,  p_str_itm_num,  p_str_itm_color,  p_str_itm_size, pstrStkStatus);
        }
    }
}
