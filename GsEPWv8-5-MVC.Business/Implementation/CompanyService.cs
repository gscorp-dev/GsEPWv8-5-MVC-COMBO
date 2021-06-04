using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//CR-3PL_MVC_IB_2018_0219_008 - Display company name instead of Comp Id in all the Reports

namespace GsEPWv8_5_MVC.Business.Implementation
{
    public class CompanyService : ICompanyService
    {
        public Company GetCompanyDetails(Company objICompanyService)
        {
            return objRepository.GetCompanyDetails(objICompanyService);
        }
        CompanyRepository objRepository = new CompanyRepository();
       public Company GetPickCompanyDetails(Company objCompany)
        {
         return objRepository.GetPickCompanyDetails(objCompany);
        }
        public Company GetFullFillCompanyDetails(Company objCompany)
        {
            return objRepository.GetFullFillCompanyDetails(objCompany);
        }
        public Company GetCustOfCompanyDetails(Company objCompany)
        {
            return objRepository.GetCustOfCompanyDetails(objCompany);
        }
        public Company GetWhsIdDetails(Company objCompany)
        {
            return objRepository.GetWhsIdDetails(objCompany);
        }
        public Company GetLocIdDetails(Company objCompany)
        {
            return objRepository.GetLocIdDetails(objCompany);
        }
        public Company GetCustConfigDtls(Company objInboundInquiry)
        {
            return objRepository.GetCustConfigDtls(objInboundInquiry);
        }
        public Company Validate_Company(Company objCompany)
        {
            return objRepository.Validate_Company(objCompany);
        }
        public Company Validate_Cmp_whs(Company objCompany)
        {
            return objRepository.Validate_Cmp_whs(objCompany);
        }
        public Company Validate_Customer(Company objCompany)
        {
            return objRepository.Validate_Customer(objCompany);
        }
        //CR - 3PL_MVC_IB_2018_0219_008
        public Company GetCompName(Company objCompany) 
        {
            return objRepository.GetCompName(objCompany);
        }
        //CR - 3PL_MVC_IB_2018_0219_008
        //CR - 3PL_MVC_IB_2018_0219_004
        public Company GetCustOfCompName(Company objCompany)
        {
            return objRepository.GetCustOfCompName(objCompany);
        }
        //CR - 3PL_MVC_IB_2018_0219_004
        public Company LoadUserType(Company objCompany)
        {
            return objRepository.LoadUserType(objCompany);
        }
        public Company CompanyAddresHdrDtls(Company objCompany)
        {
            return objRepository.CompanyAddresHdrDtls(objCompany);
        }

        public Company GetPickCompanyDetails(Company objCompany, string pstrCmpId)
        {
            return objRepository.GetPickCompanyDetails(objCompany, pstrCmpId);
        }
    }

}
