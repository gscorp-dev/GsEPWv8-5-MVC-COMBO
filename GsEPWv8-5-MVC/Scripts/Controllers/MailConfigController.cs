using AutoMapper;
using GsEPWv8_4_MVC.Business.Implementation;
using GsEPWv8_4_MVC.Business.Interface;
using GsEPWv8_4_MVC.Core.Entity;
using GsEPWv8_4_MVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GsEPWv8_4_MVC.Controllers
{
    public class MailConfigController : Controller
    {
        MailConfig objMailConfig = new MailConfig();
        MailConfigModel objDailyFlashConfigModel = new MailConfigModel();
        IMailConfigService ServiceObject = new MailConfigService();
        Company objCompany = new Company();
        CompanyService ServiceObjectCompany = new CompanyService();
        public bool l_str_include_entry_dtls;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MailConfig()
        {
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objMailConfig.UserId = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objMailConfig.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objMailConfig.cmpid = Session["UserID"].ToString().Trim();
            Mapper.CreateMap<MailConfig, MailConfigModel>();
            MailConfigModel objMailConfigModel = Mapper.Map<MailConfig, MailConfigModel>(objMailConfig);
            return View(objMailConfigModel);

        }
        public ActionResult MailConfigChange(string compid, string screenid)
        {
            objMailConfig.cmpid = compid.Trim();
            objMailConfig.scn_id = screenid.Trim();
            objMailConfig = ServiceObject.GetReportDetails(objMailConfig);
            var serializer = new JavaScriptSerializer() { MaxJsonLength = 86753090 };

            // Perform your serialization
            serializer.Serialize(objMailConfig);
            return new JsonResult()
            {
                Data = objMailConfig,
                MaxJsonLength = 86753090
            };

        }
     
        public ActionResult SaveConfigDetails(string compid, string screenid,string Reportid, string mode, string mailto, string mailcc, string mailBcc, string content)
        {
            objMailConfig.cmpid = compid.Trim();
            objMailConfig = ServiceObject.GetMailCount(objMailConfig);
            objMailConfig.TotalRecords = objMailConfig.LstSavedMailCount.Count();
            objMailConfig.scn_id = screenid.Trim();
            objMailConfig.rpt_id = Reportid.Trim();
            objMailConfig.Mode = mode.Trim();
            objMailConfig.EmailTo = mailto.Trim();
            objMailConfig.EmailCC = mailcc.Trim();
            objMailConfig.EmailBCC = mailBcc.Trim();
            objMailConfig.emailbody = content.Trim();
            if (Session["UserID"]==null)
            {
                objMailConfig.UserId = "";
            }
            objMailConfig.UserId = Session["UserID"].ToString().Trim();
            objMailConfig = ServiceObject.GetReportDetails(objMailConfig);
            objMailConfig.rpt_name = objMailConfig.LstMailConfig[0].rpt_name;
            objMailConfig = ServiceObject.SaveMailConfigDetails(objMailConfig);
            objMailConfig.TotalCount= objMailConfig.LstSaveMailConfig.Count();
            if (objMailConfig.TotalCount > objMailConfig.TotalRecords)
            {
                l_str_include_entry_dtls = true;
            }
            else
            {
                l_str_include_entry_dtls = false;
            }
            return Json(l_str_include_entry_dtls, JsonRequestBehavior.AllowGet);

        }
    }
}
