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
    public class CompanyMasterService : ICompanyMasterService
    {
        CompanyMasterRepository objRepository = new CompanyMasterRepository();
        public CompanyMaster GetCmpMasterDetails(CompanyMaster objCompanyMaster)
        {
            return objRepository.GetCmpMasterDetails(objCompanyMaster);
        }
        public CompanyMaster GetCmpHdrDetails(CompanyMaster objCompanyMaster)
        {
            return objRepository.GetCmpHdrDetails(objCompanyMaster);
        }
             
        public void SaveCmpMasterDtls(CompanyMaster objCompanyMaster)
        {
            objRepository.SaveCmpMasterDtls(objCompanyMaster);
        }
        public void UpdateCmpMasterDtls(CompanyMaster objCompanyMaster)
        {
            objRepository.UpdateCmpMasterDtls(objCompanyMaster);
        }
        public void DeleteCmp(CompanyMaster objCompanyMaster)
        {
            objRepository.DeleteCmp(objCompanyMaster);
        }
        public CompanyMaster CheckExistCmpId(CompanyMaster objCompanyMaster)
        {
            return objRepository.CheckExistCmpId(objCompanyMaster);
        }
        public CompanyMaster CheckCustDetails(CompanyMaster objCompanyMaster)
        {
            return objRepository.CheckCustDetails(objCompanyMaster);
        }
        public CompanyMaster CheckExistwhsId(CompanyMaster objCompanyMaster)
        {
            return objRepository.CheckExistwhsId(objCompanyMaster);
        }
    }
}
