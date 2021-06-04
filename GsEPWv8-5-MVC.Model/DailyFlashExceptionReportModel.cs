using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
    public class DaysDetailListModel : DailyFlashExceptionReportModel
    {
        public string DaysID { get; set; }
        public string Days { get; set; }
    }
    public class DailyFlashExceptionReportModel
    {
        public string cmp_id { get; set; }
        public string cmp_name { get; set; }
        public string rpt_id { get; set; }
        public string rpt_description { get; set; }
        public string rpt_name { get; set; }
        public string is_company_user { get; set; }
        public string scn_id { get; set; }
        public string is_daily_flash { get; set; }
        public string Status { get; set; }
        public string rpt_run_time { get; set; }
        public string dflt_frmt { get; set; }
        public string Time { get; set; }
        public string Timemin { get; set; }
        public string action { get; set; }
        public string rpt_run_days { get; set; }
        public int ExistingRecords { get; set; }
        public int NewRecords { get; set; }
        public string rpt_status { get; set; }
        public string email_to { get; set; }
        public string email_cc { get; set; }
        public string email_msg { get; set; }
        public string selected_email { get; set; }
        public string selected_company { get; set; }
        public IList<DailyFlashExceptionReport> ListReportName { get; set; }
        public IList<DailyFlashExceptionReport> ListExceptionRptDetails { get; set; }
        public IList<DailyFlashExceptionReport> ListSaveExceptionRptDetails { get; set; }
        public IList<Company> ListCompanyPickDtl { get;set;}
        public IList<DaysDetailList> ListDays { get; set; }
        public IEnumerable<string> SelectedDaysID { get; set; }
    }
}
