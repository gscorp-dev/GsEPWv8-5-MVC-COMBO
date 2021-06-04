
using GsEPWv8_5_MVC.Core.Entity;

namespace GsEPWv8_5_MVC.Data.Interface
{
   public interface  IBinMasterRepository
    {
        clsItemPcsDim GetBinspGetItemPcsDimDtl(string pstrCmpId, string pstrItmNum, string pstrItmColor, string pstrItmSize);
        BinMaster GetBinMasterInquiryDetails(BinMaster objIBinMasterService);
        BinMaster fnGetBinMaster(string pstrCmpId, string pstrBinId);
        bool SaveBinMaster(clsBinMater objBinMater, string pstrMode);
        int fnCheckBinMasterExists(string pstrCmpId, string pstrBinId);
        int fnCheckBinStyleExists(string pstrCmpId, string pstrItmCode);
         bool fnInsertBinLocByIBDocId(string pstrCmpId, string pstrWhsId, string pstrLocId, string pstrItmCode, string pstrItmName);

    }
}
