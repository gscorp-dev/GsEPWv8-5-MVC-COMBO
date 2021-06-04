using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface IExceptionReportsService
    {
        ExceptionReports GetExceptionReportsInquiryDetails(ExceptionReports objExceptionReports);
        ExceptionReports GetExceptionReportsInquiryDetailsRpt(ExceptionReports objExceptionReports);
        ExceptionReports GetIBExceptionReportsInquiryDetails(ExceptionReports objExceptionReports);
        ExceptionReports GetIBExceptionReportsInquiryDetailsRpt(ExceptionReports objExceptionReports);
        ExceptionReports ItemXGetitmDetails(string term, string cmp_id);
        ExceptionReports GetIVExceptionReportsInqdtl(ExceptionReports objExceptionReports);
        ExceptionReports GetIVExceptionReportsInqdtlRpt(ExceptionReports objExceptionReports);
        ExceptionReports GetIBExceptionReport(ExceptionReports objExceptionReports);
        ExceptionReports GetOBExceptionReport(ExceptionReports objExceptionReports);
        ExceptionReports GetIVExceptionReport(ExceptionReports objExceptionReports);
    }
}
