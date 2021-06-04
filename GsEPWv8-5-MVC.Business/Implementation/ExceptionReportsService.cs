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
    public class ExceptionReportsService: IExceptionReportsService
    {
        ExceptionReportsRepository objRepository = new ExceptionReportsRepository();
        public ExceptionReports GetExceptionReportsInquiryDetails(ExceptionReports objExceptionReports)
        {
            return objRepository.GetExceptionReportsInquiryDetails(objExceptionReports);
        }
        public ExceptionReports GetExceptionReportsInquiryDetailsRpt(ExceptionReports objExceptionReports)
        {
            return objRepository.GetExceptionReportsInquiryDetailsRpt(objExceptionReports);
        }
        public ExceptionReports GetIBExceptionReportsInquiryDetails(ExceptionReports objExceptionReports)
        {
            return objRepository.GetIBExceptionReportsInquiryDetails(objExceptionReports);
        }
        public ExceptionReports GetIBExceptionReportsInquiryDetailsRpt(ExceptionReports objExceptionReports)
        {
            return objRepository.GetIBExceptionReportsInquiryDetailsRpt(objExceptionReports);
        }
        public ExceptionReports ItemXGetitmDetails(string term, string cmp_id)
        {
            return objRepository.ItemXGetitmDetails(term, cmp_id);
        }
        public ExceptionReports GetIVExceptionReportsInqdtl(ExceptionReports objExceptionReports)
        {
            return objRepository.GetIVExceptionReportsInqdtl(objExceptionReports);
        }
        public ExceptionReports GetIVExceptionReportsInqdtlRpt(ExceptionReports objExceptionReports)
        {
            return objRepository.GetIVExceptionReportsInqdtlRpt(objExceptionReports);
        }      
        public ExceptionReports GetIBExceptionReport(ExceptionReports objExceptionReports)
        {
            return objRepository.GetIBExceptionReport(objExceptionReports);
        }
        public ExceptionReports GetOBExceptionReport(ExceptionReports objExceptionReports)
        {
            return objRepository.GetOBExceptionReport(objExceptionReports);
        }
        public ExceptionReports GetIVExceptionReport(ExceptionReports objExceptionReports)
        {
            return objRepository.GetIVExceptionReport(objExceptionReports);
        }
    }
}
