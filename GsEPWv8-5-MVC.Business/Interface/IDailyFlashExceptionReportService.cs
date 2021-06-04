using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface IDailyFlashExceptionReportService
    {
        DailyFlashExceptionReport GetExceptionReportName(DailyFlashExceptionReport objDailyFlashExceptionReport);
        DailyFlashExceptionReport GetExceptionReportDetails(DailyFlashExceptionReport objDailyFlashExceptionReport);
        DailyFlashExceptionReport InsertExceptionReportDetails(DailyFlashExceptionReport objDailyFlashExceptionReport);
        DailyFlashExceptionReport InsertExceptionReportMailConfigDetails(DailyFlashExceptionReport objDailyFlashExceptionReport);
        DailyFlashExceptionReport GetExistingJobDetails(DailyFlashExceptionReport objDailyFlashExceptionReport);

    }
}
