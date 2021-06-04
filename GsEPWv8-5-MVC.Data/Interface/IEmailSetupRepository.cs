using System.Data;
using GsEPWv8_5_MVC.Core.Entity;
using System.Collections.Generic;

namespace GsEPWv8_5_MVC.Data.Interface
{
    public interface IEmailSetupRepository
    {
        EmailSetup fnGetEmailList(string pstrCmpId,string pstrUserType);
        EmailSetup fnGetEmailCommon(string pstrCmpId);
        bool fnSaveEmailList(string pstrCmpId, List<clsRptEmailDtl> lstRptEmailDt, EmailCommon objEmailCommon);
        Email fnGetCmpEmailList(string pstrCmpId);
        Email fnGetCustEmailList(string pstrCmpId);
        EmailSetup GetEmailReportDetails(string pstrCmpId, string pstrModuleName, string pstrRptId);
         EmailAlert getEmailAlertDetails(string pstrCmpId, string pstrModuleName, string pstrRptId);
    }
}
