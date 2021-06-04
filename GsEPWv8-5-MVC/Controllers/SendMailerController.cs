using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using GsEPWv8_5_MVC.Model;
using System.Text;
using AutoMapper;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Business.Implementation;

namespace GsEPWv8_5_MVC.Controllers
{
    // CR#                         Modified By   Date         Description
    //3PL_MVC_WC_2018_0219_021      Ravikumar     201-0219    To Fix the Email CC Issue
    public class SendMailerController : Controller
    {
        // GET: SendMailer
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendMail(EmailModel _objModelMail, string EmailTo, string EmailCc, string EmailSubject, string EmailMessage, string EmailAttachment, string FilePath)
        {

            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                string strSMTPHost = string.Empty;
                string strSmtpUserId = string.Empty;
                string strSmtpPassword = string.Empty;
                string l_str_from_email = string.Empty;
                string l_str_from_email_display_name = string.Empty;

                try
                {
                    strSMTPHost = System.Configuration.ConfigurationManager.AppSettings["SMTPHost"].ToString();
                    strSmtpUserId = System.Configuration.ConfigurationManager.AppSettings["SMTPUserId"].ToString();
                    strSmtpPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPUserPwd"].ToString();
                    l_str_from_email = System.Configuration.ConfigurationManager.AppSettings["FromEmailId"].ToString();
                    l_str_from_email_display_name = System.Configuration.ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
                }
                catch
                {
                }

                mail.To.Add(_objModelMail.EmailTo);
                if (_objModelMail.EmailCC != null)
                {
                    mail.CC.Add(_objModelMail.EmailCC);
                }
                // mail.Bcc.Add(_objModelMail.EmailBcc);
                //mail.From = new MailAddress(strSmtpUserId);
                mail.From = new MailAddress(l_str_from_email, l_str_from_email_display_name);
                mail.Subject = _objModelMail.EmailSubject;
                StringBuilder myBuilder = new StringBuilder();
                if (EmailMessage != null && EmailMessage != "")
                {
                    string[] lstr_message = _objModelMail.EmailMessage.Split('\n');

                    for (int j = 0; j < lstr_message.Count(); j++)
                    {

                        myBuilder.Append(lstr_message[j].ToString());
                        myBuilder.Append("<br/>");
                        if (j == lstr_message.Count() - 1)
                        {
                            myBuilder.Append("<br/>");
                            //myBuilder.Append(EmailMessageContent);
                        }
                        mail.Body = myBuilder.ToString();
                    }
                }
                else
                {
                    mail.Body = "";
                }
                // string Body = _objModelMail.EmailMessage;
                mail.IsBodyHtml = true;
                string lstr_file_name = string.Empty;
                if (EmailAttachment.Length > 0 && FilePath.Length>0)
                {
                    lstr_file_name = FilePath + EmailAttachment;
                }
                else
                { 
                    if (Session["RptFileName"] != null)
                    {
                        lstr_file_name = Session["RptFileName"].ToString();
                    }
                }
                if (lstr_file_name.Length > 0)
                {
                    Attachment attach = new Attachment(lstr_file_name);
                    mail.Attachments.Add(attach);
                }

                else
                {

                    String[] str_ary_file_list = (string[])Session["str_ary_file_list"];

                    for (int i = 0; i < str_ary_file_list.Length; i++)
                    {
                        Attachment attach = new Attachment(str_ary_file_list[i]);
                        mail.Attachments.Add(attach);
                    }

                }
                SmtpClient smtp = new SmtpClient();
                smtp.Host = strSMTPHost; //"smtp.gmail.com";
                smtp.Port = 25;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(strSmtpUserId, strSmtpPassword);
                smtp.EnableSsl = false;

                try
                {
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    strSmtpPassword = ex.InnerException.ToString();
                }
                Session["RptFileName"] = null;
                Session["str_ary_file_list"] = null;
                //return Json("Index", _objModelMail);
                return Json("Email", JsonRequestBehavior.AllowGet);
            }
            else
            {
                Session["RptFileName"] = null;
                Session["str_ary_file_list"] = null;
                return View();
            }
        }
        public ActionResult GetEmailToList(string p_str_user_id, string p_str_email_to)
        {
            Email objEmail = new Email();
            EmailSetupService objEmailService = new EmailSetupService();

           // EmailService objEmailService = new EmailService();
            string[] laryEmailToList = p_str_email_to.Split(',');
            string lstrNonCmpEmail = string.Empty;
            if (Session["UserID"].ToString() != null)
            {
                objEmail.user_id = Session["UserID"].ToString().Trim();
            }
            objEmail.CmpId = Session["g_str_cmp_id"].ToString();
            objEmail = objEmailService.fnGetCustEmailList(objEmail.CmpId);
       
            objEmail.EmailTo = (p_str_email_to == null ? string.Empty : p_str_email_to.Trim());
            // objEmail = objEmailService.GetMailDetails(objEmail);

            if (laryEmailToList.Length > 0)
            { 
            foreach (string laryEmailTo in laryEmailToList)
            {
                    if (objEmail.ListGetMail.Select(l => l.email).ToList().Contains(laryEmailTo) == true)
                    {
                        //
                    }
                    else
                    {
                        lstrNonCmpEmail = lstrNonCmpEmail + laryEmailTo + ",";
                    }
                
            }
                objEmail.non_cmp_email = lstrNonCmpEmail;
            }
            
            if (lstrNonCmpEmail.Length > 0)
            {
                objEmail.non_cmp_email = lstrNonCmpEmail.Remove(lstrNonCmpEmail.Length - 1, 1);
            }
            else
            {
                objEmail.non_cmp_email = string.Empty;
            }
            objEmail.Actiontype = "EmailTo";
            Mapper.CreateMap<Email, EmailModel>();
            EmailModel objEmailModel = Mapper.Map<Email, EmailModel>(objEmail);
            return PartialView("_GetEmailList", objEmailModel);
        }
        public ActionResult GetEmailCCList(string p_str_user_id, string p_str_email_cc)
        {
            Email objEmail = new Email();
            EmailSetupService objEmailService = new EmailSetupService();
           // EmailService objEmailService = new EmailService();
            string[] laryEmailToList = p_str_email_cc.Split(',');
            string lstrNonCmpEmail = string.Empty;
            if (Session["UserID"].ToString() != null)
            {
                objEmail.user_id = Session["UserID"].ToString().Trim();
            }
            objEmail.CmpId = Session["g_str_cmp_id"].ToString();

            objEmail = objEmailService.fnGetCmpEmailList(string.Empty);
            objEmail.EmailCC = (p_str_email_cc == null ? string.Empty : p_str_email_cc.Trim());
            //objEmail = objEmailService.GetMailDetails(objEmail);
            if (laryEmailToList.Length > 0)
            {
                foreach (string laryEmailTo in laryEmailToList)
                {
                    if (objEmail.ListGetMail.Select(l => l.email).ToList().Contains(laryEmailTo) == true)
                    {
                        //
                    }
                    else
                    {
                        lstrNonCmpEmail = lstrNonCmpEmail + laryEmailTo + ",";
                    }

                }
                objEmail.non_cmp_email = lstrNonCmpEmail;
            }

            if (lstrNonCmpEmail.Length > 0)
            {
                objEmail.non_cmp_email = lstrNonCmpEmail.Remove(lstrNonCmpEmail.Length - 1, 1);
            }
            else
            {
                objEmail.non_cmp_email = string.Empty;
            }
            objEmail.Actiontype = "EmailCC";
            Mapper.CreateMap<Email, EmailModel>();
            EmailModel objEmailModel = Mapper.Map<Email, EmailModel>(objEmail);
            return PartialView("_GetEmailList", objEmailModel);
        }

        public ActionResult GetCustEmailList(string p_str_user_id, string p_str_email_to)
        {
            Email objEmail = new Email();
            EmailSetupService objEmailService = new EmailSetupService();
            string[] laryEmailToList = p_str_email_to.Split(',');
            string lstrNonCmpEmail = string.Empty;
            if (Session["UserID"].ToString() != null)
            {
                objEmail.user_id = Session["UserID"].ToString().Trim();
            }
            objEmail.CmpId = Session["g_str_cmp_id"].ToString();
            objEmail = objEmailService.fnGetCustEmailList(objEmail.CmpId);

            objEmail.EmailTo = (p_str_email_to == null ? string.Empty : p_str_email_to.Trim());

            if (laryEmailToList.Length > 0)
            {
                foreach (string laryEmailTo in laryEmailToList)
                {
                    if (objEmail.ListGetMail.Select(l => l.email).ToList().Contains(laryEmailTo) == true)
                    {
                        //
                    }
                    else
                    {
                        lstrNonCmpEmail = lstrNonCmpEmail + laryEmailTo + ",";
                    }

                }
                objEmail.non_cmp_email = lstrNonCmpEmail;
            }

            if (lstrNonCmpEmail.Length > 0)
            {
                objEmail.non_cmp_email = lstrNonCmpEmail.Remove(lstrNonCmpEmail.Length - 1, 1);
            }
            else
            {
                objEmail.non_cmp_email = string.Empty;
            }
            objEmail.Actiontype = "custEmailTo";
            Mapper.CreateMap<Email, EmailModel>();
            EmailModel objEmailModel = Mapper.Map<Email, EmailModel>(objEmail);
            return PartialView("_GetEmailAlertList", objEmailModel);
        }
        public ActionResult GetCmpEmailList(string pstrCmpEmailTo)
        {
            Email objEmail = new Email();
            EmailSetupService objEmailService = new EmailSetupService();
            // EmailService objEmailService = new EmailService();
            string[] laryEmailToList = pstrCmpEmailTo.Split(',');
            string lstrNonCmpEmail = string.Empty;
            if (Session["UserID"].ToString() != null)
            {
                objEmail.user_id = Session["UserID"].ToString().Trim();
            }
            objEmail.CmpId = Session["g_str_cmp_id"].ToString();

            objEmail = objEmailService.fnGetCmpEmailList(string.Empty);
            objEmail.EmailCC = (pstrCmpEmailTo == null ? string.Empty : pstrCmpEmailTo.Trim());
            if (laryEmailToList.Length > 0)
            {
                foreach (string laryEmailTo in laryEmailToList)
                {
                    if (objEmail.ListGetMail.Select(l => l.email).ToList().Contains(laryEmailTo) == true)
                    {
                        //
                    }
                    else
                    {
                        lstrNonCmpEmail = lstrNonCmpEmail + laryEmailTo + ",";
                    }

                }
                objEmail.non_cmp_email = lstrNonCmpEmail;
            }

            if (lstrNonCmpEmail.Length > 0)
            {
                objEmail.non_cmp_email = lstrNonCmpEmail.Remove(lstrNonCmpEmail.Length - 1, 1);
            }
            else
            {
                objEmail.non_cmp_email = string.Empty;
            }
            objEmail.Actiontype = "cmpEmailTo";
            Mapper.CreateMap<Email, EmailModel>();
            EmailModel objEmailModel = Mapper.Map<Email, EmailModel>(objEmail);
            return PartialView("_GetEmailAlertList", objEmailModel);
        }

        [HttpPost]
        public ActionResult SendEMailAlert(string custEmailTo, string cmpEmailTo, string emailCC, string emailSubject, string emailMessage, string emailFilePath, string emailFileName)
        {

            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                string strSMTPHost = string.Empty;
                string strSmtpUserId = string.Empty;
                string strSmtpPassword = string.Empty;
                string l_str_from_email = string.Empty;
                string l_str_from_email_display_name = string.Empty;

                try
                {
                    strSMTPHost = System.Configuration.ConfigurationManager.AppSettings["SMTPHost"].ToString();
                    strSmtpUserId = System.Configuration.ConfigurationManager.AppSettings["SMTPUserId"].ToString();
                    strSmtpPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPUserPwd"].ToString();
                    l_str_from_email = System.Configuration.ConfigurationManager.AppSettings["FromEmailId"].ToString();
                    l_str_from_email_display_name = System.Configuration.ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
                }
                catch
                {
                }

                if ((custEmailTo != string.Empty) && (cmpEmailTo != string.Empty))
                {
                    mail.To.Add(custEmailTo + "," + cmpEmailTo);
                }
                else if (custEmailTo != string.Empty)
                {
                    mail.To.Add(custEmailTo);
                }
                else if (cmpEmailTo != string.Empty)
                {
                    mail.To.Add(cmpEmailTo);
                }

                if (emailCC != string.Empty)
                {
                    mail.CC.Add(custEmailTo);
                }

                mail.From = new MailAddress(l_str_from_email, l_str_from_email_display_name);
                mail.Subject = emailSubject;
                StringBuilder myBuilder = new StringBuilder();
                if (emailMessage != null && emailMessage != "")
                {
                    string[] lstr_message = emailMessage.Split('\n');

                    for (int j = 0; j < lstr_message.Count(); j++)
                    {

                        myBuilder.Append(lstr_message[j].ToString());
                        myBuilder.Append("<br/>");
                        if (j == lstr_message.Count() - 1)
                        {
                            myBuilder.Append("<br/>");
                        }
                        mail.Body = myBuilder.ToString();
                    }
                }
                else
                {
                    mail.Body = "";
                }

                mail.IsBodyHtml = true;
                string lstr_file_name = string.Empty;
                if (emailFileName.Length > 0 && emailFilePath.Length > 0)
                {
                    lstr_file_name = emailFilePath + emailFileName;
                }
                else
                {
                    if (Session["RptFileName"] != null)
                    {
                        lstr_file_name = Session["RptFileName"].ToString();
                    }
                }
                if (lstr_file_name.Length > 0)
                {
                    Attachment attach = new Attachment(lstr_file_name);
                    mail.Attachments.Add(attach);
                }

                else
                {

                    String[] str_ary_file_list = (string[])Session["str_ary_file_list"];

                    for (int i = 0; i < str_ary_file_list.Length; i++)
                    {
                        Attachment attach = new Attachment(str_ary_file_list[i]);
                        mail.Attachments.Add(attach);
                    }

                }
                SmtpClient smtp = new SmtpClient();
                smtp.Host = strSMTPHost; //"smtp.gmail.com";
                smtp.Port = 25;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(strSmtpUserId, strSmtpPassword);
                smtp.EnableSsl = false;

                try
                {
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    strSmtpPassword = ex.InnerException.ToString();
                }
                Session["RptFileName"] = null;
                Session["str_ary_file_list"] = null;
                //return Json("Index", _objModelMail);
                return Json("Email", JsonRequestBehavior.AllowGet);
            }
            else
            {
                Session["RptFileName"] = null;
                Session["str_ary_file_list"] = null;
                return View();
            }
        }

        [HttpPost]
        public ActionResult SendEMailWithMultiDocAlert(string custEmailTo, string cmpEmailTo, string emailCC, string emailSubject, string emailMessage, string[] emailFileNameWthPath)
        {

            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                string strSMTPHost = string.Empty;
                string strSmtpUserId = string.Empty;
                string strSmtpPassword = string.Empty;
                string l_str_from_email = string.Empty;
                string l_str_from_email_display_name = string.Empty;

                try
                {
                    strSMTPHost = System.Configuration.ConfigurationManager.AppSettings["SMTPHost"].ToString();
                    strSmtpUserId = System.Configuration.ConfigurationManager.AppSettings["SMTPUserId"].ToString();
                    strSmtpPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPUserPwd"].ToString();
                    l_str_from_email = System.Configuration.ConfigurationManager.AppSettings["FromEmailId"].ToString();
                    l_str_from_email_display_name = System.Configuration.ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
                }
                catch
                {
                }

                if ((custEmailTo != string.Empty) && (cmpEmailTo != string.Empty))
                {
                    mail.To.Add(custEmailTo + "," + cmpEmailTo);
                }
                else if (custEmailTo != string.Empty)
                {
                    mail.To.Add(custEmailTo);
                }
                else if (cmpEmailTo != string.Empty)
                {
                    mail.To.Add(cmpEmailTo);
                }

                if (emailCC != string.Empty)
                {
                    mail.CC.Add(custEmailTo);
                }

                mail.From = new MailAddress(l_str_from_email, l_str_from_email_display_name);
                mail.Subject = emailSubject;
                StringBuilder myBuilder = new StringBuilder();
                if (emailMessage != null && emailMessage != "")
                {
                    string[] lstr_message = emailMessage.Split('\n');

                    for (int j = 0; j < lstr_message.Count(); j++)
                    {

                        myBuilder.Append(lstr_message[j].ToString());
                        myBuilder.Append("<br/>");
                        if (j == lstr_message.Count() - 1)
                        {
                            myBuilder.Append("<br/>");
                        }
                        mail.Body = myBuilder.ToString();
                    }
                }
                else
                {
                    mail.Body = "";
                }

                mail.IsBodyHtml = true;
                string lstr_file_name = string.Empty;
                

                    for (int i = 0; i < emailFileNameWthPath.Length; i++)
                    {
                        Attachment attach = new Attachment(emailFileNameWthPath[i].ToString());
                        mail.Attachments.Add(attach);
                    }

                SmtpClient smtp = new SmtpClient();
                smtp.Host = strSMTPHost; //"smtp.gmail.com";
                smtp.Port = 25;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(strSmtpUserId, strSmtpPassword);
                smtp.EnableSsl = false;

                try
                {
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    strSmtpPassword = ex.InnerException.ToString();
                }
                return Json("Email", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View();
            }
        }


    }
}