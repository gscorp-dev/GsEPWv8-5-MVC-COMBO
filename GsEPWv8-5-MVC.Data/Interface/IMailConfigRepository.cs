using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Interface
{
    public interface IMailConfigRepository
    {
         MailConfig GetReportDetails(MailConfig objMailConfig);
         MailConfig SaveMailConfigDetails(MailConfig objMailConfig);
        MailConfig GetExistingMailCount(MailConfig objMailConfig);
        MailConfig GetMailConfigDetails(MailConfig objMailConfig);
    
        MailConfig GetReportName(MailConfig objMailConfig);
        MailConfig GetUsersEmailList(MailConfig objMailConfig);
    }
}
