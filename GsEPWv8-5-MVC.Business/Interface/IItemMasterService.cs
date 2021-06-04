using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface IItemMasterService
    {
        ItemMaster fnGetItemConfig(string pstrConfigType);
        ItemMaster fnGetItemPkgServiceg(string pstrCmpId, string pstrItmCode);
        bool SaveSaveItemPkgServicer(string pstrMode, string pstrCmpId, string pstrItmCode, string pstrPkgService);
        ItemMaster GetItemMasterDetails(ItemMaster objItemMaster);
        string fnAddNew940Item(ItemMaster objItemMaster);
        ItemMaster ExistItem(ItemMaster objItemMaster);
        ItemMaster GetItemMasterViewDetails(ItemMaster objRateMaster);
        void ItemMasterCreateUpdate(ItemMaster objRateMaster);
        void ItemMasterdelete(ItemMaster objRateMaster);

        void ItemMasterHeaderCreateUpdate(ItemMaster objRateMaster);
        LookUp GetItemMasterCategory(LookUp objRateMaster);
        ItemMaster GetItmId(ItemMaster objRateMaster);
        void ItemMasterKithdr(ItemMaster objItemMaster);
        void ItemMasterKitdtl(ItemMaster objItemMaster);
        ItemMaster CheckAndDeleteItm(ItemMaster objItemMaster);
        ItemdtlReport GetItemDtlList(ItemdtlReport objItemdtlReport, string p_str_cmp_id);
        DataTable GetItemDtlListExcelRpt(string p_str_cmp_id);

        bool SaveDimUpdate(string pstrCmpId, DataTable ldtDimUpdate);
        List<ItemStock> getItemStock(string p_str_cmp_id, string p_str_itm_num, string p_str_itm_color, string p_str_itm_size, string pstrStkStatus);
    }
}
