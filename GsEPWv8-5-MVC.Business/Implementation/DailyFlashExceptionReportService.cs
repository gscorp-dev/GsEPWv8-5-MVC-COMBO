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
    public class DailyFlashExceptionReportService: IDailyFlashExceptionReportService
    {
        DailyFlashExceptionReportRepository objDailyFlashExceptionReportRepository = new DailyFlashExceptionReportRepository();
        public DailyFlashExceptionReport GetExceptionReportName(DailyFlashExceptionReport objDailyFlashExceptionReport)
        {
            return objDailyFlashExceptionReportRepository.GetExceptionReportName(objDailyFlashExceptionReport);
        }
        public DailyFlashExceptionReport GetExceptionReportDetails(DailyFlashExceptionReport objDailyFlashExceptionReport)
        {
            return objDailyFlashExceptionReportRepository.GetExceptionReportDetails(objDailyFlashExceptionReport);
        }
        public DailyFlashExceptionReport InsertExceptionReportDetails(DailyFlashExceptionReport objDailyFlashExceptionReport)
        {
            return objDailyFlashExceptionReportRepository.InsertExceptionReportDetails(objDailyFlashExceptionReport);
        }
        public DailyFlashExceptionReport InsertExceptionReportMailConfigDetails(DailyFlashExceptionReport objDailyFlashExceptionReport)
        {
            return objDailyFlashExceptionReportRepository.InsertExceptionReportMailConfigDetails(objDailyFlashExceptionReport);
        }
        public DailyFlashExceptionReport GetExistingJobDetails(DailyFlashExceptionReport objDailyFlashExceptionReport)
        {
            return objDailyFlashExceptionReportRepository.GetExistingJobDetails(objDailyFlashExceptionReport);
        }
    }
    }

