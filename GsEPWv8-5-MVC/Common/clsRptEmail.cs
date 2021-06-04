using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GsEPWv8_4_MVC.Common
{
    public class clsRptEmail
    {
        public void getEmailAlertDetails(EmailAlertHdr objEmailAlertHdr, string pstrCmpId, string pstrModuleName, string pstrRptId, ref bool lblnRptEmailExists)
        {
            string l_str_email_regards = string.Empty;
            string l_str_email_footer1 = string.Empty;
            string l_str_email_footer2 = string.Empty;
            EmailAlert objEmailAlert = new EmailAlert();
            EmailSetupService ServiceObject = new EmailSetupService();


            objEmailAlert = ServiceObject.getEmailAlertDetails(pstrCmpId, pstrModuleName, pstrRptId);

            if (objEmailAlert.lstEmailAlertHdr.Count != 0)
            {
                objEmailAlertHdr.rptFileFormat = objEmailAlert.lstEmailAlertHdr[0].rptFileFormat;
                objEmailAlertHdr.isAutoEmail = objEmailAlert.lstEmailAlertHdr[0].isAutoEmail;
                lblnRptEmailExists = true;
                if (objEmailAlert.lstEmailAlertHdr[0].cmpEmailTo != null)
                {
                    objEmailAlertHdr.cmpEmailTo = objEmailAlert.lstEmailAlertHdr[0].cmpEmailTo.Trim();
                }

                if (objEmailAlert.lstEmailAlertHdr[0].custEmailTo != null)
                {
                    objEmailAlertHdr.custEmailTo = objEmailAlert.lstEmailAlertHdr[0].custEmailTo.Trim();
                }

               
                if (objEmailAlert.lstEmailAlertHdr[0].emailMessage == null)
                {
                    //objEmail.EmailMessage = pstrEmailMessage;
                }
                else
                {
                    objEmailAlertHdr.emailMessage = objEmailAlert.lstEmailAlertHdr[0].emailMessage;
                }
                if (objEmailAlert.lstEmailAlertHdr[0].emailCC == null)
                {
                    
                }
                else
                {
                    objEmailAlertHdr.emailCC = objEmailAlert.lstEmailAlertHdr[0].emailCC;
                }
            }
            else
            {
                // objEmail.EmailMessage = pstrEmailMessage;
                objEmailAlertHdr.cmpEmailTo = "";
                objEmailAlertHdr.custEmailTo = "";
                objEmailAlertHdr.emailCC = "";
                objEmailAlertHdr.emailMessage = "";
            }

            try
            {
                l_str_email_regards = System.Configuration.ConfigurationManager.AppSettings["EmailRegards"].ToString().Trim();
                l_str_email_footer1 = System.Configuration.ConfigurationManager.AppSettings["EmailFooter1"].ToString().Trim();
                l_str_email_footer2 = System.Configuration.ConfigurationManager.AppSettings["EmailFooter2"].ToString().Trim();
            }
            catch (Exception ex)
            {
                l_str_email_regards = "3PL WAREHOUSE";
                l_str_email_footer1 = "Thank you for your business.";
                l_str_email_footer2 = "Please Do not reply to this alert mail, the mail box is not monitored. If any question or help, please contact the CSR";
            }
            objEmailAlertHdr.emailFooter = objEmailAlertHdr.emailFooter + "\n" + "\n" + l_str_email_footer1;
            objEmailAlertHdr.emailFooter = objEmailAlertHdr.emailFooter + "\n" + "\n" + "Regards,";
            objEmailAlertHdr.emailFooter = objEmailAlertHdr.emailFooter + "\n" + l_str_email_regards;
            objEmailAlertHdr.emailFooter = objEmailAlertHdr.emailFooter + "\n" + "\n" + l_str_email_footer2;


            //return objEmail;
        }
        public void getEmailDetails(Email objEmail, string pstrCmpId, string pstrModuleName, string pstrRptId, ref bool lblnRptEmailExists)
        {
            string l_str_email_regards = string.Empty;
            string l_str_email_footer1 = string.Empty;
            string l_str_email_footer2 = string.Empty;
            EmailSetup objEmailSetup = new EmailSetup();
            EmailSetupService ServiceObject = new EmailSetupService();
            

            objEmailSetup = ServiceObject.GetEmailReportDetails(pstrCmpId, pstrModuleName, pstrRptId);
            
            if (objEmailSetup.lstRptEmailDtl.Count != 0)
            {
                objEmail.rpt_file_format = objEmailSetup.lstRptEmailDtl[0].rpt_file_format;
                objEmail.is_auto_email = objEmailSetup.lstRptEmailDtl[0].is_auto_email;
                lblnRptEmailExists = true;
                if (objEmailSetup.lstRptEmailDtl[0].cmp_email_list != null)
                {
                    objEmail.EmailTo = objEmailSetup.lstRptEmailDtl[0].cmp_email_list.Trim();
                }
                if (objEmailSetup.lstRptEmailDtl[0].cust_email_list != null)
                {
                    if (objEmail.EmailTo != null)
                    {
                        if (objEmail.EmailTo.Length> 0)
                        {
                            objEmail.EmailTo = objEmail.EmailTo + "," + objEmailSetup.lstRptEmailDtl[0].cust_email_list.Trim();
                        }
                        else
                        {
                            objEmail.EmailTo = objEmailSetup.lstRptEmailDtl[0].cust_email_list.Trim();
                        }
                    }
                    else
                    {
                        objEmail.EmailTo = objEmailSetup.lstRptEmailDtl[0].cust_email_list.Trim();
                    }
                }

               // objEmail.EmailTo = (objEmailSetup.lstRptEmailDtl[0].cmp_email_list.Trim() == null || objEmail.ListEamilDetail[0].EmailTo.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailTo.Trim();
               // objEmail.EmailCC = (objEmail.ListEamilDetail[0].EmailCC.Trim() == null || objEmail.ListEamilDetail[0].EmailCC.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailCC.Trim();
                if (objEmailSetup.lstRptEmailDtl[0].email_body == null)
                {
                    //objEmail.EmailMessage = pstrEmailMessage;
                }
                else
                {
                    objEmail.EmailMessage = objEmailSetup.lstRptEmailDtl[0].email_body;
                }
            }
            else
            {
               // objEmail.EmailMessage = pstrEmailMessage;
                objEmail.EmailTo = "";
                objEmail.EmailCC = "";
            }
            //Email objEmail = new Email();
            try
            {
                l_str_email_regards = System.Configuration.ConfigurationManager.AppSettings["EmailRegards"].ToString().Trim();
                l_str_email_footer1 = System.Configuration.ConfigurationManager.AppSettings["EmailFooter1"].ToString().Trim();
                l_str_email_footer2 = System.Configuration.ConfigurationManager.AppSettings["EmailFooter2"].ToString().Trim();
            }
            catch (Exception ex)
            {
                l_str_email_regards = "3PL WAREHOUSE";
                l_str_email_footer1 = "Thank you for your business.";
                l_str_email_footer2 = "Please Do not reply to this alert mail, the mail box is not monitored. If any question or help, please contact the CSR";
            }
            objEmail.EmailFooter = objEmail.EmailFooter + "\n" + "\n" + l_str_email_footer1;
            objEmail.EmailFooter = objEmail.EmailFooter + "\n" + "\n" + "Regards,";
            objEmail.EmailFooter = objEmail.EmailFooter + "\n" + l_str_email_regards;
            objEmail.EmailFooter = objEmail.EmailFooter + "\n" + "\n" + l_str_email_footer2;


            //return objEmail;
        }
    }
}