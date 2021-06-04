using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//CR-3PL_MVC_IB_2018_0219_008 - Display company name instead of Comp Id in all the Reports

namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface ICompanyService
    {
        Company GetCompanyDetails(Company objLookUp);
        Company GetPickCompanyDetails(Company objCompany);
        Company GetFullFillCompanyDetails(Company objCompany);
        Company GetCustOfCompanyDetails(Company objCompany);
        Company GetWhsIdDetails(Company objCompany);
        Company GetLocIdDetails(Company objCompany);
        Company GetCustConfigDtls(Company objCompany);
Company Validate_Company(Company objCompany);
        Company Validate_Cmp_whs(Company objCompany);
        Company Validate_Customer(Company objCompany);
        Company LoadUserType(Company objCompany);
        Company GetCompName(Company objCompany);//CR - 3PL_MVC_IB_2018_0219_008
        Company GetCustOfCompName(Company objCompany); //CR - 3PL_MVC_IB_2018_0219_004
        Company CompanyAddresHdrDtls(Company objCompanyMaster);
        Company GetPickCompanyDetails(Company objCompany, string pstrCmpId);


    }

}
