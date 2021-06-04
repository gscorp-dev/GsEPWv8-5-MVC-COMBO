using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Interface
{
    public interface IDailyFlashExceptionReportRepository
    {
        DailyFlashExceptionReport GetExceptionReportName(DailyFlashExceptionReport objDailyFlashExceptionReport);
        DailyFlashExceptionReport GetExceptionReportDetails(DailyFlashExceptionReport objDailyFlashExceptionReport);
        DailyFlashExceptionReport InsertExceptionReportDetails(DailyFlashExceptionReport objDailyFlashExceptionReport);
        DailyFlashExceptionReport InsertExceptionReportMailConfigDetails(DailyFlashExceptionReport objDailyFlashExceptionReport);
        DailyFlashExceptionReport GetExistingJobDetails(DailyFlashExceptionReport objDailyFlashExceptionReport);
    }
}
