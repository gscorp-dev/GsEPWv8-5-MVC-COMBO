using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GsEPWv8_5_MVC.Controllers
{
    public class UserProfileController : Controller
    {
        // GET: UserProfile
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserProfile()
        {
            UserProfile ObjUserProfile = new UserProfile();
            ObjUserProfile.CompanyLogo = System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString();
            ObjUserProfile.CompanyWebLink = System.Configuration.ConfigurationManager.AppSettings["CompanyWebLink"].ToString();
            ObjUserProfile.CompanyAppName = System.Configuration.ConfigurationManager.AppSettings["AppWelcome"].ToString();
            ObjUserProfile.Show_day = DateTime.Now.ToLongDateString();
            return View();
        }
        public ActionResult UserIDCheckExist(string l_str_old_pwd)
        {
            UserProfile ObjUserProfile = new UserProfile();
            UserProfileService ServiceObject = new UserProfileService();
            if(Session["UserID"].ToString()!=null)
            {
                ObjUserProfile.user_id = Session["UserID"].ToString().Trim();
            }
            else
            {
                ObjUserProfile.user_id =string.Empty;
            }
            ObjUserProfile.old_pwd = l_str_old_pwd;
            ObjUserProfile = ServiceObject.CheckExistUserID(ObjUserProfile);
            return Json(ObjUserProfile.ListCheckUserIdExist.Count, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateUserAccountPassword(string l_str_old_pwd, string l_str_cnfrm_pwd)
        {
            UserProfile ObjUserProfile = new UserProfile();
            UserProfileService ServiceObject = new UserProfileService();
            if (Session["UserID"].ToString() != null)
            {
                ObjUserProfile.user_id = Session["UserID"].ToString().Trim();
            }
            else
            {
                ObjUserProfile.user_id = string.Empty;
            }
            ObjUserProfile.old_pwd = l_str_old_pwd;
            ObjUserProfile.new_pwd = l_str_cnfrm_pwd;
            ObjUserProfile = ServiceObject.UpdateUserAccPwd(ObjUserProfile);
            Session.Abandon();
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}