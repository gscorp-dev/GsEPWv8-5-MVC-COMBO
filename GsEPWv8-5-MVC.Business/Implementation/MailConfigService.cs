using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
    public class MailConfigService: IMailConfigService
    {
        IMailConfigRepository objRepository = new MailConfigRepository();
        public MailConfig GetReportDetails(MailConfig objMailConfig)
        {
            return objRepository.GetReportDetails(objMailConfig);
        }
        public MailConfig SaveMailConfigDetails(MailConfig objMailConfig)
        {
            return objRepository.SaveMailConfigDetails(objMailConfig);
        }
        public MailConfig GetExistingMailCount(MailConfig objMailConfig)
        {
            return objRepository.GetExistingMailCount(objMailConfig);
        }
        public MailConfig GetMailConfigDetails(MailConfig objMailConfig)
        {
            return objRepository.GetMailConfigDetails(objMailConfig);
        }
        public MailConfig GetReportName(MailConfig objMailConfig)
        {
            return objRepository.GetReportName(objMailConfig);
        }
        public MailConfig GetUsersEmailList(MailConfig objMailConfig)
        {
            return objRepository.GetUsersEmailList(objMailConfig);
        }
    }
}
