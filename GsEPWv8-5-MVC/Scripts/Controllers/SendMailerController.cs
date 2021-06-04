using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using GsEPWv8_4_MVC.Model;
using System.Text;

namespace GsEPWv8_4_MVC.Controllers
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
        public ActionResult SendMail(EmailModel _objModelMail, string EmailTo, string EmailCc, string EmailSubject, string EmailMessage,string EmailMessageContent)
        {
            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                string strSMTPHost = string.Empty;
                string strSmtpUserId = string.Empty;
                string strSmtpPassword = string.Empty;
           
                try
                {
                    strSMTPHost = System.Configuration.ConfigurationManager.AppSettings["SMTPHost"].ToString();
                    strSmtpUserId = System.Configuration.ConfigurationManager.AppSettings["SMTPUserId"].ToString();
                    strSmtpPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPUserPwd"].ToString();
                }
                catch {
                }

                mail.To.Add(_objModelMail.EmailTo);
                if (_objModelMail.EmailCC != null)
                {
                    mail.CC.Add(_objModelMail.EmailCC);
                }
                // mail.Bcc.Add(_objModelMail.EmailBcc);
                mail.From = new MailAddress(strSmtpUserId);
                mail.Subject = _objModelMail.EmailSubject;
                StringBuilder myBuilder = new StringBuilder();
                string[] lstr_message = _objModelMail.EmailMessage.Split('\n');

                for (int j = 0; j < lstr_message.Count(); j++)
                {

                    myBuilder.Append(lstr_message[j].ToString());
                    myBuilder.Append("<br/>");
                    if (j == lstr_message.Count() - 1)
                    {
                        myBuilder.Append("<br/>");
                        myBuilder.Append(EmailMessageContent);
                    }
                    mail.Body = myBuilder.ToString();
                }
               // string Body = _objModelMail.EmailMessage;
                mail.IsBodyHtml = true;
                string lstr_file_name  = string.Empty;
                lstr_file_name = Session["RptFileName"].ToString();
                Attachment attach = new Attachment(lstr_file_name);
                mail.Attachments.Add(attach);

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
                catch(Exception ex)
                {
                    strSmtpPassword = ex.InnerException.ToString();
                }
                //return Json("Index", _objModelMail);
                return Json("Email", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View();
            }
        }
        }
}