using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
    public class EmailSetupService : IEmailSetupService

    {
        IEmailSetupRepository objEmailSetup = new EmailSetupRepository();

       public EmailSetup fnGetEmailList(string pstrCmpId, string pstrUserType)
        {
           return objEmailSetup.fnGetEmailList(pstrCmpId,  pstrUserType);
        }
        public EmailSetup fnGetEmailCommon(string pstrCmpId)
        {
            return objEmailSetup.fnGetEmailCommon(pstrCmpId);
        }

        public bool fnSaveEmailList(string pstrCmpId, List<clsRptEmailDtl> lstRptEmailDt, EmailCommon objEmailCommon)
        {
            return objEmailSetup.fnSaveEmailList(pstrCmpId,  lstRptEmailDt, objEmailCommon);
        }

        public Email fnGetCmpEmailList(string pstrCmpId)
        {
            return objEmailSetup.fnGetCmpEmailList(pstrCmpId);

        }
        public Email fnGetCustEmailList(string pstrCmpId)
        {
            return objEmailSetup.fnGetCustEmailList(pstrCmpId);
        }

        public EmailSetup GetEmailReportDetails(string pstrCmpId, string pstrModuleName, string pstrRptId)
        {
            return objEmailSetup.GetEmailReportDetails(pstrCmpId, pstrModuleName, pstrRptId);
        }
        public EmailAlert getEmailAlertDetails(string pstrCmpId, string pstrModuleName, string pstrRptId)
        {
            return objEmailSetup.getEmailAlertDetails(pstrCmpId, pstrModuleName, pstrRptId);
        }

    }
}
