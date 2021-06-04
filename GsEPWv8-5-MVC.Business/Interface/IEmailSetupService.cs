using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;

namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface IEmailSetupService
    {
        EmailSetup fnGetEmailList(string pstrCmpId, string pstrUserType);
        EmailSetup fnGetEmailCommon(string pstrCmpId);
        Email fnGetCmpEmailList(string pstrCmpId);
        Email fnGetCustEmailList(string pstrCmpId);
        EmailSetup GetEmailReportDetails(string pstrCmpId, string pstrModuleName, string pstrRptId);
        EmailAlert getEmailAlertDetails(string pstrCmpId, string pstrModuleName, string pstrRptId);
    }
}
