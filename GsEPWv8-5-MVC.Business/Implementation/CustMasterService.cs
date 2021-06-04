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
    public class CustMasterService : ICustMasterService
    {
        CustMasterRepository objRepository = new CustMasterRepository();
        public CustMaster GetCustMasterDetails(CustMaster objCustMaster)
        {
            return objRepository.GetCustMasterDetails(objCustMaster);
        }        
        public CustMaster GetCustHdrDetails(CustMaster objCustMaster)
        {
            return objRepository.GetCustHdrDetails(objCustMaster);
        }
        public CustMaster GetDftWhs(CustMaster objCustMaster)
        {
            return objRepository.GetDftWhs(objCustMaster);
        }

        public CustMaster GetDftCmpWhs(CustMaster objCustMaster)
        {
            return objRepository.GetDftCmpWhs(objCustMaster);
        }
        public CustMaster GetCustDetails(CustMaster objCustMaster)
        {
            return objRepository.GetCustDetails(objCustMaster);
        }
        public CustMaster fnGetAppUserList(string pstrCmpId, string pstrMode)
        {
            return objRepository.fnGetAppUserList(pstrCmpId, pstrMode);
        }

        public CustMaster GetCustConfigDetails(CustMaster objCustMaster)
        {
            return objRepository.GetCustConfigDetails(objCustMaster);
        }       
        public void SaveCmpHdr(CustMaster objCustMaster)
        {
            objRepository.SaveCmpHdr(objCustMaster);
        }
        public void SaveWhsMaster(CustMaster objCustMaster)
        {
            objRepository.SaveWhsMaster(objCustMaster);
        }
        public void UpdateCustMasterConfigDir(CustMaster objCustMaster)
        {
            objRepository.UpdateCustMasterConfigDir(objCustMaster);
        }
        public void SaveCustMasterConfigDir(CustMaster objCustMaster)
        {
            objRepository.SaveCustMasterConfigDir(objCustMaster);
        }    
        public void UpdateCmpHdr(CustMaster objCustMaster)
        {
            objRepository.UpdateCmpHdr(objCustMaster);
        }
        public void DeleteCmpHdr(CustMaster objCustMaster)
        {
            objRepository.DeleteCmpHdr(objCustMaster);
        }
       

        public void UpdateCustMasterDtl(CustMaster objCustMaster)
        {
            objRepository.UpdateCustMasterDtl(objCustMaster);
        }

        public void SaveCustMasterDtl(CustMaster objCustMaster)
        {
            objRepository.SaveCustMasterDtl(objCustMaster);
        }
        public void SaveCustMaster(CustMaster objCustMaster)
        {
             objRepository.SaveCustMaster(objCustMaster);
        }
        public void SaveCustMasterConfig(CustMaster objCustMaster)
        {
            objRepository.SaveCustMasterConfig(objCustMaster);
        }
        public void UpdateCustMasterConfig(CustMaster objCustMaster)
        {
            objRepository.UpdateCustMasterConfig(objCustMaster);
        }
        public void UpdateCustMaster(CustMaster objCustMaster)
        {
            objRepository.UpdateCustMaster(objCustMaster);
        }
        public void DeleteCust(CustMaster objCustMaster)
        {
            objRepository.DeleteCust(objCustMaster);
        }
        public void SaveUserXCmp(CustMaster objCustMaster, string pstrUserList)
        {
            objRepository.SaveUserXCmp(objCustMaster, pstrUserList);
        }
        public CustMaster GetTableEntityValueCount(CustMaster objCustMaster)
        {
            return objRepository.GetTableEntityValueCount(objCustMaster);
        }
        public void InsertTableEntityValueByCust(CustMaster objCustMaster)
        {
            objRepository.InsertTableEntityValueByCust(objCustMaster);
        }
        public CustMaster GetTableEntityValueCountByRMA_DocId(CustMaster objCustMaster)
        {
            return objRepository.GetTableEntityValueCountByRMA_DocId(objCustMaster);
        }
        public CustMaster GetCheckExistCmpId(CustMaster objCustMaster)
        {
            return objRepository.GetCheckExistCmpId(objCustMaster);
        }

        public CustMaster GetCustomerLogo(CustMaster objCustMaster)
        {
            return objRepository.GetCustomerLogo(objCustMaster);
        }
        public CustMaster CheckCustomerExist(CustMaster objCustMaster)
        {
            return objRepository.CheckCustomerExist(objCustMaster);
        }
       public CustMaster GetCustMasterRptDetails(CustMaster objCustMaster)
        {
            return objRepository.GetCustMasterRptDetails(objCustMaster);
        }
        public string CheckExistCustInitial(string p_str_Cust_initial)
        {
            return objRepository.CheckExistCustInitial(p_str_Cust_initial);
        }

        public string GetCustInitial(string cmp_id)
        {
            return objRepository.GetCustInitial(cmp_id);
        }
        public bool fnAddCustDfltRate(CustMaster objCustMaster)
        {
            return objRepository.fnAddCustDfltRate(objCustMaster);
        }
        public CustMaster fnGetCustDfltRate(string pstrCmpId)
        {
            return objRepository.fnGetCustDfltRate(pstrCmpId);
        }
    }
}
