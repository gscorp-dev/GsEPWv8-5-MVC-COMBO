
using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface  IBinMasterService
    {
        clsItemPcsDim GetBinspGetItemPcsDimDtl(string pstrCmpId, string pstrItmNum, string pstrItmColor, string pstrItmSize);
        BinMaster GetBinMasterInquiryDetails(BinMaster objIBinMasterService);
        bool SaveBinMaster(clsBinMater objBinMater, string pstrMode);

        BinMaster fnGetBinMaster(string pstrCmpId, string pstrBinId);

        int fnCheckBinMasterExists(string pstrCmpId, string pstrBinId);
        int fnCheckBinStyleExists(string pstrCmpId, string pstrItmCode);
        bool fnInsertBinLocByIBDocId(string pstrCmpId, string pstrWhsId, string pstrLocId, string pstrItmCode, string pstrItmName);
    }
}
