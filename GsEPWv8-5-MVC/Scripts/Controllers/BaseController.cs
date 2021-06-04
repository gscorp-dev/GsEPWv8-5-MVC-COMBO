using  GsEPWv8_4_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace  GsEPWv8_4_MVC.Controllers
{
    public class BaseController : Controller
    {

        public BasicEntity ObjBasicEntity = new BasicEntity();

        // GET: Base
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public BaseController()
        {
            BasicInformation(ObjBasicEntity);
        }

        public void BasicInformation(BasicEntity ObjBasicEntity)
        {
            HttpCookie objCookie = System.Web.HttpContext.Current.Request.Cookies["BasicInformation"];
            if (objCookie != null)
            {
                ObjBasicEntity.UserID = Convert.ToInt16(objCookie["iUserID"]);
                ObjBasicEntity.UserName = objCookie["vUserName"];
                ObjBasicEntity.UserGroupID = Convert.ToInt16(objCookie["iUserGroupID"]);
                ObjBasicEntity.UserRole = objCookie["vUserRole"];
            }
        }

    }
}