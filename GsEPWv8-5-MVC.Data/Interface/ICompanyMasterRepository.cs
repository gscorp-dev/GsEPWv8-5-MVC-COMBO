using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Interface
{
    public interface ICompanyMasterRepository
    {
        CompanyMaster GetCmpMasterDetails(CompanyMaster objCompanyMaster);
        void SaveCmpMasterDtls(CompanyMaster objCompanyMaster);
        CompanyMaster GetCmpHdrDetails(CompanyMaster objCompanyMaster);
        CompanyMaster CheckExistCmpId(CompanyMaster objCompanyMaster);
        void UpdateCmpMasterDtls(CompanyMaster objCompanyMaster);
        void DeleteCmp(CompanyMaster objCompanyMaster);
        CompanyMaster CheckCustDetails(CompanyMaster objCompanyMaster);
        CompanyMaster CheckExistwhsId(CompanyMaster objCompanyMaster);

    }
}
