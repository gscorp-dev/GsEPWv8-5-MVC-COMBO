using AutoMapper;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GsEPWv8_5_MVC.Controllers
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
            objMailConfig.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objMailConfig.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objMailConfig.cmp_id =null;
            objMailConfig.scn_id = null;
            objMailConfig = ServiceObject.GetReportDetails(objMailConfig);
            objMailConfig.LstMailConfigReports = objMailConfig.LstMailConfig;
            Mapper.CreateMap<MailConfig, MailConfigModel>();
            MailConfigModel objMailConfigModel = Mapper.Map<MailConfig, MailConfigModel>(objMailConfig);
            return View(objMailConfigModel);

        }
        public ActionResult MailConfigChange(string compid, string screenid)
        {
            objMailConfig.cmp_id = compid.Trim();
            objMailConfig.scn_id = screenid.Trim();
            objMailConfig = ServiceObject.GetReportDetails(objMailConfig);
            objMailConfig.LstMailConfigReports = objMailConfig.LstMailConfig;
            var serializer = new JavaScriptSerializer() { MaxJsonLength = 86753090 };

            // Perform your serialization
            serializer.Serialize(objMailConfig);
            return new JsonResult()
            {
                Data = objMailConfig,
                MaxJsonLength = 86753090
            };

        }
        public ActionResult NewMailConfigEntry(string compid)
        {
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objMailConfig.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objMailConfig.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objMailConfig.cmp_id = null;
            objMailConfig.scn_id = null;
            objMailConfig = ServiceObject.GetReportDetails(objMailConfig);
            objMailConfig.LstMailConfigReports = objMailConfig.LstMailConfig;
            objMailConfig.cmp_id = compid;
            Mapper.CreateMap<MailConfig, MailConfigModel>();
            MailConfigModel objMailConfigModel = Mapper.Map<MailConfig, MailConfigModel>(objMailConfig);
            return PartialView("_SaveMailConfig", objMailConfigModel);
        }
        public ActionResult LoadMailList(string compid,string SelectedMailList)
        {
            objMailConfig.cmp_id = compid.Trim();
            objMailConfig = ServiceObject.GetUsersEmailList(objMailConfig);
            if (SelectedMailList != null)
            {
                objMailConfig.selectedEmail = SelectedMailList;
            }
            Mapper.CreateMap<MailConfig, MailConfigModel>();
            MailConfigModel objMailConfigModel = Mapper.Map<MailConfig, MailConfigModel>(objMailConfig);
            return PartialView("_LoadEmailList", objMailConfigModel);
        }

        public ActionResult SaveConfigDetails(string compid, string screenid,string Reportid, string mailto, string mailcc,string content, string status)
        {
            objMailConfig.cmp_id = compid.Trim();
            objMailConfig.scn_id = screenid.Trim();
            objMailConfig.rpt_id = Reportid.Trim();
            objMailConfig = ServiceObject.GetReportName(objMailConfig);
            if (objMailConfig.LstMailConfigReportID.Count > 0)
            {
                objMailConfig.rpt_name = objMailConfig.LstMailConfigReportID[0].rpt_name.Trim();
                objMailConfig.rpt_description = objMailConfig.LstMailConfigReportID[0].rpt_description.Trim();
            }
            objMailConfig.EmailTo = mailto.Trim();
            objMailConfig.EmailCC = mailcc.Trim();
            objMailConfig.emailbody = content.Trim();
            objMailConfig.Status = status.Trim();
            objMailConfig = ServiceObject.GetExistingMailCount(objMailConfig);
            objMailConfig.TotalRecords = objMailConfig.LstSavedMailCount.Count();
            if (objMailConfig.TotalRecords == 0)
            {
                objMailConfig = ServiceObject.SaveMailConfigDetails(objMailConfig);
                l_str_include_entry_dtls = true;
            }
          else
            {
                l_str_include_entry_dtls = false;
            }
            return Json(l_str_include_entry_dtls, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetMailConfigDetails(string compid, string screenid, string Reportid, string mailto, string mailcc)
        {
            objMailConfig.cmp_id = compid.Trim();
            objMailConfig.scn_id = screenid.Trim();
            objMailConfig.rpt_id = Reportid.Trim();
            objMailConfig.EmailTo = mailto.Trim();
            objMailConfig.EmailCC = mailcc.Trim();
            objMailConfig = ServiceObject.GetReportName(objMailConfig);
            if (objMailConfig.LstMailConfigReportID.Count > 0)
            {
                objMailConfig.rpt_name = objMailConfig.LstMailConfigReportID[0].rpt_name.Trim();
               
            }
            else
            {
                objMailConfig.rpt_name = string.Empty;
            }
            objMailConfig = ServiceObject.GetMailConfigDetails(objMailConfig);
            Mapper.CreateMap<MailConfig, MailConfigModel>();
            MailConfigModel objMailConfigModel = Mapper.Map<MailConfig, MailConfigModel>(objMailConfig);
            return PartialView("_MailConfig", objMailConfigModel);
        }
    }
}
