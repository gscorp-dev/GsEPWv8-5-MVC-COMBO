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
  public  class ETAInquiryService: IETAInquiryService
    {
        ETAInquiryRepository objRepository = new ETAInquiryRepository();
        public ETAInquiry GetInboundETAInquiryDetails(ETAInquiry objETAInquiry)
        {
            return objRepository.GetInboundETAInquiryDetails(objETAInquiry);
        }
        public ETAInquiry GetInboundETAInquiryDetailsRpt(ETAInquiry objETAInquiry)
        {
            return objRepository.GetInboundETAInquiryDetailsRpt(objETAInquiry);
        }
        public ETAInquiry GetInboundETAInquiryRpt(ETAInquiry objETAInquiry)
        {
            return objRepository.GetInboundETAInquiryRpt(objETAInquiry);
        }
    }
}
