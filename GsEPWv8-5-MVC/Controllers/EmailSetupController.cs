using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace GsEPWv8_4_MVC.Controllers
{
    public class EmailSetupController : Controller
    {
        // GET: EmailSetup
        public ActionResult EmailSetup()
        {
            string lstrCmpId = string.Empty;
            try
            {
                lstrCmpId = Session["g_str_cmp_id"].ToString().Trim();
                EmailSetup objEmailSetup = new EmailSetup();
                EmailSetupService ServiceObject = new EmailSetupService();
                objEmailSetup.cmp_id = lstrCmpId;
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();

                LookUp objLookUp = new LookUp();
                LookUpService ServiceObject1 = new LookUpService();
                objLookUp.id = "301";
                objLookUp.lookuptype = "RPTEMAILFMT";
                objLookUp = ServiceObject1.GetLookUpValue(objLookUp);

                objEmailSetup.ListRptEmailFormat = objLookUp.ListLookUpDtl;

                objCompany.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                objCompany.user_id = Session["UserID"].ToString().Trim();
                objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                objEmailSetup.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                EmailSetup objEmailSetupFetch = new EmailSetup();
                objEmailSetupFetch = ServiceObject.fnGetEmailList(lstrCmpId, Session["IsCompanyUser"].ToString());
                objEmailSetup.lstRptEmailDtl = ServiceObject.fnGetEmailList(lstrCmpId, Session["IsCompanyUser"].ToString()).lstRptEmailDtl;
                objEmailSetup.objEmailCommon = ServiceObject.fnGetEmailCommon(lstrCmpId).objEmailCommon;
                string lstrEemailRegds = string.Empty;
                string lstrEemailFooter = string.Empty;
                string lstrEemailFooter1 = string.Empty;
                string lstrEemailFooter2 = string.Empty;
                EmailCommon objEmailCommon = new EmailCommon();
                if ((objEmailSetup.objEmailCommon == null) )
                {
                    try
                    {
                        lstrEemailRegds = "Regards," + "\n" + System.Configuration.ConfigurationManager.AppSettings["EmailRegards"].ToString().Trim();
                        lstrEemailFooter1 = System.Configuration.ConfigurationManager.AppSettings["EmailFooter1"].ToString().Trim();
                        lstrEemailFooter2 = System.Configuration.ConfigurationManager.AppSettings["EmailFooter2"].ToString().Trim();
                    }
                    catch (Exception ex)
                    {
                        lstrEemailRegds = "Regards," + "\n" + "3PL WAREHOUSE";
                        lstrEemailFooter1 = "Thank you for your business.";
                        lstrEemailFooter2 = "Please Do not reply to this alert mail, the mail box is not monitored. If any question or help, please contact the CSR";
                    }
                    lstrEemailFooter = lstrEemailRegds;
                    lstrEemailFooter = lstrEemailFooter + "\n\n" + lstrEemailFooter1;
                    lstrEemailFooter = lstrEemailFooter + "\n" + "\n" + lstrEemailFooter2;
                    objEmailCommon.emailFooter = lstrEemailFooter;
                    objEmailSetup.objEmailCommon = objEmailCommon;
                }
                Mapper.CreateMap<EmailSetup, EmailSetupModel>();
                EmailSetupModel EmailSetupModel = Mapper.Map<EmailSetup, EmailSetupModel>(objEmailSetup);
                return View(EmailSetupModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public ActionResult getEmailSetup(string pstrCmpId)
        {
            //string lstrCmpId = string.Empty;
            try
            {
                EmailSetup objEmailSetup = new EmailSetup();
                EmailSetupService ServiceObject = new EmailSetupService();
                //if (pstrCmpId == string.Empty)
                //{
                //    lstrCmpId = Session["g_str_cmp_id"].ToString();
                //}
                //else
                //{
                //    lstrCmpId = pstrCmpId;
                //}
                
                objEmailSetup.cmp_id = pstrCmpId;
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();
                LookUp objLookUp = new LookUp();
                LookUpService ServiceObject1 = new LookUpService();
                objLookUp.id = "301";
                objLookUp.lookuptype = "RPTEMAILFMT";
                objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                objEmailSetup.ListRptEmailFormat = objLookUp.ListLookUpDtl;
                objCompany.cmp_id = pstrCmpId;
                objCompany.user_id = Session["UserID"].ToString().Trim();
                objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                objEmailSetup.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                objEmailSetup.lstRptEmailDtl = ServiceObject.fnGetEmailList(pstrCmpId, Session["IsCompanyUser"].ToString()).lstRptEmailDtl;
                Mapper.CreateMap<EmailSetup, EmailSetupModel>();
                EmailSetupModel EmailSetupModel = Mapper.Map<EmailSetup, EmailSetupModel>(objEmailSetup);
                return PartialView("_EmailSetup", EmailSetupModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }



        public ActionResult CmpIdOnChange(string p_str_cmp_id)
        {
            Session["g_str_cmp_id"] = (p_str_cmp_id == null ? string.Empty : p_str_cmp_id.Trim());
            return Json(p_str_cmp_id, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveReportEmailSetup(string pstrCmpId, List<clsRptEmailDtl> lstRptEmailDt, EmailCommon objEmailCommon)
        {
            bool lblnSaveStatus = false;
            EmailSetup objEmailSetup = new EmailSetup();
            EmailSetupService ServiceObject = new EmailSetupService();
            DataTable dt_item_stock_move = new DataTable();
            objEmailSetup.lstRptEmailDtl = lstRptEmailDt;
            lblnSaveStatus= ServiceObject.fnSaveEmailList(pstrCmpId, lstRptEmailDt, objEmailCommon);
            return Json("1", JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEmailList(string pstrCmpId, string pstrEmail, string pstrFormControl, string pstrEmailType)
        {
            Email objEmail = new Email();
            EmailSetupService objEmailService = new EmailSetupService();

            if (pstrEmailType == "CMP")
            {
                objEmail = objEmailService.fnGetCmpEmailList(pstrCmpId);
            }
            else
            {
                objEmail = objEmailService.fnGetCustEmailList(pstrCmpId);
            }

            if (Session["UserID"].ToString() != null)
            {
                objEmail.user_id = Session["UserID"].ToString().Trim();
            }
            objEmail.CmpId = pstrCmpId;
            objEmail.EmailTo = (pstrEmail == null ? string.Empty : pstrEmail.Trim());
           
           // objEmail = objEmailService.GetMailDetails(objEmail);
            objEmail.Actiontype = "EmailTo";
            objEmail.formControl = pstrFormControl;
            Mapper.CreateMap<Email, EmailModel>();
            EmailModel objEmailModel = Mapper.Map<Email, EmailModel>(objEmail);
            return PartialView("_GetCustEmailList", objEmailModel);
        }
      
    }
}