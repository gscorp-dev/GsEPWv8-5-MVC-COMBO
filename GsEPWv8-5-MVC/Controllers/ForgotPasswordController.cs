using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Core;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GsEPWv8_5_MVC.Controllers
{
    public class ForgotPasswordController : Controller
    {
        public bool l_str_chk_details = false;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ForgotPassword()
        {
            ForgotPasswordModel ObjForgotPasswordModel = new ForgotPasswordModel();
            ObjForgotPasswordModel.CompanyLogo = System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString();
            ObjForgotPasswordModel.CompanyWebLink = System.Configuration.ConfigurationManager.AppSettings["CompanyWebLink"].ToString();
            ObjForgotPasswordModel.CompanyAppName = System.Configuration.ConfigurationManager.AppSettings["AppWelcome"].ToString();
            ObjForgotPasswordModel.Show_day = DateTime.Now.ToLongDateString();
            return View(ObjForgotPasswordModel);
        }
        public ActionResult CheckUserIDExist(string p_str_user_id, string p_str_email_id)
        {
           
            ForgotPassword ObjForgotPassword = new ForgotPassword();
            ForgotPasswordService ServiceObject = new ForgotPasswordService();
            ObjForgotPassword.user_id = (p_str_user_id==null?string.Empty: p_str_user_id.Trim());
            ObjForgotPassword.email_id = (p_str_email_id == null ? string.Empty : p_str_email_id.Trim());
            ObjForgotPassword = ServiceObject.CheckUserIDExist(ObjForgotPassword);
            if(ObjForgotPassword.ListCheckUserIDExist.Count>0)
            {
                if((ObjForgotPassword.user_id == ObjForgotPassword.ListCheckUserIDExist[0].user_id) && (ObjForgotPassword.email_id == ObjForgotPassword.ListCheckUserIDExist[0].email_id))
                {
                    l_str_chk_details = true;
                }
            }
            return Json(l_str_chk_details, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SendMail(string p_str_user_id, string p_str_email_id)
        {
            ForgotPassword ObjForgotPassword = new ForgotPassword();
            ForgotPasswordService ServiceObject = new ForgotPasswordService();
            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                string strSMTPHost = string.Empty;
                string strSmtpUserId = string.Empty;
                string strSmtpPassword = string.Empty;
                string EmailMessage = string.Empty;

                try
                {
                    strSMTPHost = System.Configuration.ConfigurationManager.AppSettings["SMTPHost"].ToString();
                    strSmtpUserId = System.Configuration.ConfigurationManager.AppSettings["SMTPUserId"].ToString();
                    strSmtpPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPUserPwd"].ToString();
                }
                catch
                {
                }
                if(p_str_email_id!=null && p_str_email_id!=string.Empty)
                {
                    mail.To.Add(p_str_email_id.Trim());
                }
            
                mail.From = new MailAddress(strSmtpUserId);
                Random randomnumber = new Random();
                var randompassword = randomnumber.Next(0, 1000000).ToString("D10");
                if(randompassword!=null && randompassword!=string.Empty)
                {
                    ObjForgotPassword.user_id = (p_str_user_id == null) ? string.Empty : p_str_user_id.Trim();
                    ObjForgotPassword.new_pwd = (randompassword== null)? string.Empty : randompassword.Trim();
                    ObjForgotPassword = ServiceObject.UpdateUserAccPwd(ObjForgotPassword);
                }
                mail.Subject = "LoginCredentials Changed";
                EmailMessage = "Hi, "+"\n"+"\n"+ "Your LoginCredentials  hasbeen Changed" + "\n"+"User ID : "+ ObjForgotPassword.user_id + "\n"+"Password :"+ randompassword;
                StringBuilder myBuilder = new StringBuilder();
                if (EmailMessage != null && EmailMessage != "")
                {
                    string[] lstr_message =EmailMessage.Split('\n');

                    for (int j = 0; j < lstr_message.Count(); j++)
                    {

                        myBuilder.Append(lstr_message[j].ToString());
                        myBuilder.Append("<br/>");
                        if (j == lstr_message.Count() - 1)
                        {
                            myBuilder.Append("<br/>");
                            myBuilder.Append("<br/>");
                            myBuilder.Append("<br/>");
                            myBuilder.Append("<br/>");
                            myBuilder.Append("Thanks & Regards, ");
                            myBuilder.Append("<br/>");
                            myBuilder.Append("GensoftCorp");
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
                SmtpClient smtp = new SmtpClient();
                smtp.Host = strSMTPHost; 
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