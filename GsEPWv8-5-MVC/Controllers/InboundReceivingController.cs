using AutoMapper;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace GsEPWv8_5_MVC.Controllers
{
    public class InboundReceivingController : Controller
    {
        // GET: InboundReceiving
        public ActionResult GetInboundReceiving(string FullFillType, string cmp)
        {
            string l_str_cmp_id = string.Empty;
            string l_str_fm_dt = string.Empty;
            try
            {
                InboundReceiving objInboundReceiving = new InboundReceiving();
                InboundReceivingService ServiceObject = new InboundReceivingService();
                objInboundReceiving.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
                objInboundReceiving.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                if (objInboundReceiving.cmp_id != "" && FullFillType == null)
                {
                    //objCompany.cmp_id = l_str_cmp_id;
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objInboundReceiving.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    objInboundReceiving.cmp_id = objInboundReceiving.ListCompanyPickDtl[0].cmp_id;
                    //DateTime date = DateTime.Now;
                    //l_str_fm_dt = new DateTime(date.Year, date.Month, 1).ToString("MM/dd/yyyy");          // CR_3PL_MVC_IB_2018_0227_001 - Commented by Soniya
                    //objInboundReceiving.rcv_dt_frm = l_str_fm_dt;
                    //objInboundReceiving.rcv_dt_to = DateTime.Now.ToString("MM/dd/yyyy");

                }
                else
                {
                    if (FullFillType == null)
                    {
                        objCompany.user_id = Session["UserID"].ToString().Trim();
                        objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                        objInboundReceiving.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    }
                }
                if (FullFillType != null)
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objInboundReceiving.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    objInboundReceiving.cmp_id = cmp.Trim();
                    objInboundReceiving.whs_id = "";
                    //DateTime date = DateTime.Now.AddDays(-30);
                    //objInboundReceiving.rcv_dt_frm = Convert.ToDateTime(date).ToString("MM/dd/yyyy");         // CR_3PL_MVC_IB_2018_0227_001 - Commented by Soniya
                    //objInboundReceiving.rcv_dt_to = DateTime.Now.ToString("MM/dd/yyyy");
                    objInboundReceiving.ib_doc_frm = "";
                    objInboundReceiving.ib_doc_to = "";
                    objInboundReceiving.Email = "";                  
                }
                Mapper.CreateMap<InboundReceiving, InboundReceivingModel>();
                InboundReceivingModel objInboundReceivingModel = Mapper.Map<InboundReceiving, InboundReceivingModel>(objInboundReceiving);

                return View(objInboundReceivingModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        [HttpGet]
        public ActionResult GetSummarydtl(string p_str_cmp_id, string p_str_whs_id, string p_str_rcv_dt_frm, string p_str_rcv_dt_to, string p_str_ib_doc_frm, string p_str_ib_doc_to)
        {

            try
            {
                InboundReceiving objInboundReceiving = new InboundReceiving();
                InboundReceivingService objService = new InboundReceivingService();
                objInboundReceiving.cmp_id = p_str_cmp_id;
                objInboundReceiving.whs_id = p_str_whs_id;
                objInboundReceiving.rcv_dt_frm = p_str_rcv_dt_frm;
                objInboundReceiving.rcv_dt_to = p_str_rcv_dt_to;
                objInboundReceiving.ib_doc_frm = p_str_ib_doc_frm;
                objInboundReceiving.ib_doc_to = p_str_ib_doc_to;
                objInboundReceiving = objService.GetInboundReceivingDtl(objInboundReceiving);
                Mapper.CreateMap<InboundReceiving, InboundReceivingModel>();
                InboundReceivingModel InboundReceivingModel = Mapper.Map<InboundReceiving, InboundReceivingModel>(objInboundReceiving);
                return PartialView("_InboundGetSummarydetail", InboundReceivingModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult GetSummaryhdr(string tablerowvalue, string p_str_cmp_id)
        {

            try
            {
                InboundReceiving objInboundReceiving = new InboundReceiving();
                InboundReceivingService objService = new InboundReceivingService();
                objInboundReceiving.cmp_id = p_str_cmp_id;
                objInboundReceiving.ib_doc_id = tablerowvalue;
                objInboundReceiving = objService.GetInboundReceivingHdr(objInboundReceiving);
                Mapper.CreateMap<InboundReceiving, InboundReceivingModel>();
                InboundReceivingModel InboundReceivingModel = Mapper.Map<InboundReceiving, InboundReceivingModel>(objInboundReceiving);
                return PartialView("_InboundGetSummarydetailHdr", InboundReceivingModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public ActionResult Emails(string emp, InboundReceivingModel objInboundReceivingModel)
        {

            try
            {
                InboundReceiving objInboundReceiving = new InboundReceiving();
                InboundReceivingService objService = new InboundReceivingService();
                objInboundReceiving.GmailUsername = "soniya@gensoftcorp.com";
                objInboundReceiving.GmailPassword = "gsInd123";
                objInboundReceiving.GmailHost = "smtpout.secureserver.net";
                objInboundReceiving.GmailPort = 25; // Gmail can use ports 25, 465 & 587; but must be 25 for medium trust environment.
                objInboundReceiving.GmailSSL = false;
                objInboundReceiving.ToEmail = "nithiya@gensoftcorp.com";
                objInboundReceiving.Subject = "Subject";
                objInboundReceiving.Title = "EPW";
                //"Verify your email id";
                objInboundReceiving.Body = "Thank you for Using EPW.<br><br>Dear Sir/Ma'am," + objInboundReceiving.Email + "Thanks for Registering your account.<br><br><br> Your UserName, Password & Activation Code Following Below.<br> UserName : " + objInboundReceiving.Email + ",<br> Password :" + objInboundReceiving.Email + "<br> Activation Code :" + objInboundReceiving.Email + "<br> Login URl : <a href='http://Ravi/saramaxecomm'>WWW.saramaxecomm.com</a>";
                //"Thanks for Registering your account.<br> please verify your email id by clicking the link <br> <a href='youraccount.com/verifycode=12323232'>verify</a>";
                objInboundReceiving.IsHtml = true;

                //un comment Fr mail  Sending
                Send(objInboundReceiving);
                return View("");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void Send(InboundReceiving obInboundReceiving)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = obInboundReceiving.GmailHost;
            smtp.Port = obInboundReceiving.GmailPort;
            smtp.EnableSsl = obInboundReceiving.GmailSSL;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(obInboundReceiving.GmailUsername, obInboundReceiving.GmailPassword);

            using (var message = new MailMessage(obInboundReceiving.GmailUsername, obInboundReceiving.ToEmail))
            {
                message.Subject = obInboundReceiving.Subject;
                message.Body = obInboundReceiving.Body;
                message.IsBodyHtml = obInboundReceiving.IsHtml;
                smtp.Send(message);
            }
        }
        public JsonResult GetWhsDetails(string term, string cmpid)
        {
            InboundReceivingService ServiceObject = new InboundReceivingService();
            var List = ServiceObject.InboundRecvWhsDetails(term, cmpid.Trim()).LstInboundReceivingHdr.Select(x => new { label = x.whsdtl, value = x.whsdtl, whs_id = x.whs_id }).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EmailForm(string cmpid)
        {
            //InboundReceiving objInboundReceiving = new InboundReceiving();
            //InboundReceivingService objService = new InboundReceivingService();
            //Mapper.CreateMap<InboundReceiving, InboundReceivingModel>();
            //InboundReceivingModel InboundReceivingModel = Mapper.Map<InboundReceiving, InboundReceivingModel>(objInboundReceiving);
            return PartialView("_Email");
        }
        public EmptyResult CmpIdOnChange(string p_str_cmp_id)
        {
            Session["g_str_cmp_id"] = (p_str_cmp_id == null ? string.Empty : p_str_cmp_id.Trim());
            return null;
        }
    }
}
    
