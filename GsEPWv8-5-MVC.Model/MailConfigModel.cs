using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
    public class MailConfigModel
    {

        public string cmp_id { get; set; }
        public string user_id { get; set; }
        public string rptname { get; set; }
        public string Status { get; set; }
        public string scn_id { get; set; }
        public string RptCode { get; set; }
        public string rpt_id { get; set; }
        public string rpt_name { get; set; }
        public string Mode { get; set; }
        public string EmailSubj { get; set; }
        public string EmailTo { get; set; }
        public string EmailCC { get; set; }
        public string EmailBCC { get; set; }
        public string emailsendername { get; set; }
        public string Reportselection { get; set; }
        public string emailmessage { get; set; }
        public string emailbody { get; set; }
        public int TotalRecords { get; set; }
        public int TotalCount { get; set; }
        public string repname { get; set; }
        public string econtent { get; set; }
        public string email { get; set; }
        public string rpt_description { get; set; }
        public string selectedEmail { get; set; }
        public string action { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<MailConfig> LstMailConfig { get; set; }
        public IList<MailConfig> LstSaveMailConfig { get; set; }
        public IList<MailConfig> LstSavedMailCount { get; set; }
        public IList<MailConfig> LstMail { get; set; }
        public IList<MailConfig> LstMailConfigReports { get; set; }
        public IList<MailConfig> LstMailConfigReportID { get; set; }
        public IList<MailConfig> LstUsersMail { get; set; }

    }
}
