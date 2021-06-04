
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
   public  class BinMasterService : IBinMasterService
    {
        BinMasterRepository objRepository = new BinMasterRepository();

        public clsItemPcsDim GetBinspGetItemPcsDimDtl(string pstrCmpId, string pstrItmNum, string pstrItmColor, string pstrItmSize)
        {
         return objRepository.GetBinspGetItemPcsDimDtl(pstrCmpId, pstrItmNum, pstrItmColor, pstrItmSize);
        }
        public BinMaster GetBinMasterInquiryDetails(BinMaster objIBinMasterService)
        {
            return objRepository.GetBinMasterInquiryDetails(objIBinMasterService);
        }


        public bool SaveBinMaster(clsBinMater objBinMater, string pstrMode)
        {
            return objRepository.SaveBinMaster(objBinMater, pstrMode);
        }
        public BinMaster fnGetBinMaster(string pstrCmpId, string pstrBinId)
        {
            return objRepository.fnGetBinMaster(pstrCmpId, pstrBinId);
        }

        public int fnCheckBinMasterExists(string pstrCmpId, string pstrBinId)
        {
            return objRepository.fnCheckBinMasterExists(pstrCmpId, pstrBinId);
        }
        public int fnCheckBinStyleExists(string pstrCmpId, string pstrItmCode)
        {
            return objRepository.fnCheckBinStyleExists(pstrCmpId, pstrItmCode);

        }

        public bool fnInsertBinLocByIBDocId(string pstrCmpId, string pstrWhsId, string pstrLocId, string pstrItmCode, string pstrItmName)
        {
            return objRepository.fnInsertBinLocByIBDocId(pstrCmpId, pstrWhsId, pstrLocId, pstrItmCode, pstrItmName);
        }
    }
}
