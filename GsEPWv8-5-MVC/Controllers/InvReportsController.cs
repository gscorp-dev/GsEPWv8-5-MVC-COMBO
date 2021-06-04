using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GsEPWv8_4_MVC.Controllers
{
    public class InvReportsController : Controller
    {
       
        [HttpGet]
        public ActionResult InvReports()
        {
            return View();
        }
    }
}