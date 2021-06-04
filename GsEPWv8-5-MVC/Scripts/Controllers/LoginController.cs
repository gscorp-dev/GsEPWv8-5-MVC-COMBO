using GsEPWv8_4_MVC.Business.Implementation;
using GsEPWv8_4_MVC.Business.Interface;
using GsEPWv8_4_MVC.Core.Entity;
using GsEPWv8_4_MVC.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GsEPWv8_4_MVC.Common;

namespace GsEPWv8_4_MVC.Controllers
{
    public class LoginController : Controller
    {
        HttpCookie objCookie = new HttpCookie("BasicInformation");
        ILoginService objService = new LoginService();
        // GET: Login
        public ActionResult Login()
        {
            LoginModel objModel = new LoginModel();
            ViewBag.chkchecked = "";
            if (Request.Cookies["UserName"] != null && Request.Cookies["Password"] != null)
            {
                objModel.Email = Request.Cookies["UserName"].Value;
                objModel.Password = Request.Cookies["Password"].Value;
                ViewBag.chkchecked = "checked";

                //objModel.Showday = DateTime.Today;
            }
            objModel.CompanyAppName = System.Configuration.ConfigurationManager.AppSettings["AppWelcome"].ToString();
            objModel.CompanyLogo = System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString();

            objModel.Showday = DateTime.Now.ToLongDateString();
            ViewBag.ErrorMessage = "";
            return View(objModel);
        }

        [HttpPost]
        public ActionResult Login(LoginModel objModel, string remember = null)
        {
            if (remember == "checked")
            {
                Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(30);
                Response.Cookies["Password"].Expires = DateTime.Now.AddDays(30);
            }
            else
            {
                Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);
                
            }
            Response.Cookies["UserName"].Value = objModel.Email.Trim();
            Response.Cookies["Password"].Value = objModel.Password.Trim();
            Login objLogin = new Login();
            objLogin.Email = objModel.Email.Trim();
            objLogin.Password = objModel.Password.Trim();
            Login objLoginData = objService.LoginAuthentication(objLogin);
            if (objLoginData != null)
            {
                if (objLoginData.Email == "")
                {
                    ViewBag.ErrorMessage = "Username or password is incorrect";
                    return View(objModel);
                }
                else
                {

                    //COMMENT BY RAVI BELOW

                    Session["UserID"] = objLoginData.user_id;
                    Session["dflt_cmp_id"] = objLoginData.dflt_cmp_id.Trim();
                    //Session["isanother"] = objLoginData.isanother.Trim();
                    HttpCookie objCookie = new HttpCookie("BasicInformation");
                    objCookie["UserID"] = objLoginData.user_id.ToString();
                    objCookie["dflt_cmp_id"] = objLoginData.dflt_cmp_id.ToString().Trim();
                    objCookie["dflt_is_mob_user"] = objLoginData.ismob_user.ToString().Trim();
                    objCookie.Expires = DateTime.Now.AddDays(1);
                
                    Response.Cookies.Add(objCookie);
                   

                    //COMMENT BY RAVI ABOVE
                    if (objCookie["dflt_is_mob_user"] == "Y")
                    {
                        return RedirectToAction("ListEcomBinScanOut", "EcomBinScanOut");
                    }
                    else
                    {
                        return RedirectToAction("ECommDashBoard", "ECommDashBoard");
                    }
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Username or password is incorrect";
                return View(objModel);
            }
        }

        public ActionResult Logout()
        {
            Session["UserID"] = null;
            return RedirectToAction("UserLogin");
        }

        public ActionResult loginNew()
        {
            return View();
        }
        public ActionResult loginView()
        {
            LoginModel objModel = new LoginModel();
            ViewBag.chkchecked = "";
            if (Request.Cookies["UserName"] != null && Request.Cookies["Password"] != null)
            {
                objModel.Email = Request.Cookies["UserName"].Value;
                objModel.Password = Request.Cookies["Password"].Value;
                ViewBag.chkchecked = "checked";
                //objModel.Showday = DateTime.Today;
            }
            objModel.CompanyAppName = System.Configuration.ConfigurationManager.AppSettings["AppWelcome"].ToString();
            objModel.CompanyLogo = System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString();
            objModel.CompanyWebLink = System.Configuration.ConfigurationManager.AppSettings["CompanyWebLink"].ToString();
            objModel.Showday = DateTime.Now.ToLongDateString();
            Session["DisplayDate"] = DateTime.Now.ToLongDateString();
            ViewBag.ErrorMessage = "";
            return View(objModel);
        }
        [HttpPost]
        public ActionResult LoginView(LoginModel objModel, string remember = null)
        {
            if (remember == "checked")
            {
                Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(30);
                Response.Cookies["Password"].Expires = DateTime.Now.AddDays(30);
            }
            else
            {
                Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);

            }
            Response.Cookies["UserName"].Value = objModel.Email.Trim();
            Response.Cookies["Password"].Value = objModel.Password.Trim();
            Login objLogin = new Login();
            objLogin.Email = objModel.Email.Trim();
            objLogin.Password = objModel.Password.Trim();
            Login objLoginData = objService.LoginAuthentication(objLogin);
            if (objLoginData != null)
            {
                if (objLoginData.Email == "")
                {
                    ViewBag.ErrorMessage = "Username or password is incorrect";
                    return View(objModel);
                }
                else
                {


                    Session["UserID"] = objLoginData.user_id;
                    Session["dflt_cmp_id"] = objLoginData.dflt_cust_id.Trim();
                    HttpCookie objCookie = new HttpCookie("BasicInformation");
                    objCookie["UserID"] = objLoginData.user_id.ToString();
                    objCookie["dflt_cmp_id"] = objLoginData.dflt_cust_id.ToString().Trim();
                    objCookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(objCookie);

                    return RedirectToAction("DashBoard", "DashBoard");

                }
            }
            else
            {
                ViewBag.ErrorMessage = "Username or password is incorrect";
                return View(objModel);
            }
        }
        public ActionResult newlogin()
        {
            return View();
        }
        public ActionResult UserLogin()
        {
            LoginModel objModel = new LoginModel();
            ViewBag.chkchecked = "";
            if (Request.Cookies["UserName"] != null && Request.Cookies["Password"] != null)
            {
                objModel.Email = Request.Cookies["UserName"].Value;
                objModel.Password = Request.Cookies["Password"].Value;
                ViewBag.chkchecked = "checked";
                //objModel.Showday = DateTime.Today;
            }
            objModel.CompanyAppName = System.Configuration.ConfigurationManager.AppSettings["AppWelcome"].ToString();
            objModel.CompanyLogo = System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToString();
            objModel.CompanyWebLink = System.Configuration.ConfigurationManager.AppSettings["CompanyWebLink"].ToString();
            objModel.DFLT_DT_REQD = System.Configuration.ConfigurationManager.AppSettings["DFLT_DT_REQD"].ToString();
            Session["DFLT_DT_REQD"] = objModel.DFLT_DT_REQD;
            Session["g_str_cmp_id"] = string.Empty;

            objModel.Showday = DateTime.Now.ToLongDateString();
            Session["DisplayDate"] = DateTime.Now.ToLongDateString();
            ViewBag.ErrorMessage = "";
            return View(objModel);
        }
        [HttpPost]
        public ActionResult UserLogin(LoginModel objModel, string Email, string Password)
        {
            Response.Cookies["UserName"].Value = objModel.Email.Trim();
            Response.Cookies["Password"].Value = objModel.Password.Trim();
            Login objLogin = new Login();
            objLogin.Email = objModel.Email.Trim();
            objLogin.Password = objModel.Password.Trim();
            Login objLoginData = objService.LoginAuthentication(objLogin);
            if (objLoginData != null)
            {
                if (objLoginData.Email == "")
                {
                    ViewBag.ErrorMessage = "Username or password is incorrect";
                    return View(objModel);
                }
                else
                {


                    Session["UserID"] = objLoginData.user_id;
                    if (objLoginData.dflt_cust_id == null)
                    {
                        Session["dflt_cmp_id"] = "";

                    }
                    else
                    {
                        Session["dflt_cmp_id"] = objLoginData.dflt_cust_id.Trim();

                    }
                    if (objLoginData.is_cmp_user == null)
                    {
                        Session["IsCompanyUser"] = "";

                    }
                    else
                    {
                        Session["IsCompanyUser"] = objLoginData.is_cmp_user.Trim();

                    }
                    HttpCookie objCookie = new HttpCookie("BasicInformation");
                    objCookie["UserID"] = objLoginData.user_id.ToString();
                    if (objLoginData.dflt_cust_id == null)
                    {
                        objCookie["dflt_cmp_id"] = "";

                    }
                    else
                    {
                        objCookie["dflt_cmp_id"] = objLoginData.dflt_cust_id.ToString().Trim();
                    }
                    if (objLoginData.is_cmp_user == null)
                    {
                        objCookie["IsCompanyUser"] = "";

                    }
                    else
                    {
                        objCookie["IsCompanyUser"] = objLoginData.is_cmp_user.ToString().Trim();
                    }
                    Session["UserfstName"] = objLoginData.user_fst_name;
                    Session["UserlstName"] = objLoginData.user_lst_name;
                    if (objLoginData.IS3RDUSER == null)
                    {
                        Session["IS3RDUSER"] = "";

                    }
                    else
                    {
                        Session["IS3RDUSER"] = objLoginData.IS3RDUSER;
                    }
                    if (objLoginData.ConnectionName == null)
                    {
                        Session["ConnectionName"] = "";

                    }
                    else
                    {
                        Session["ConnectionName"] = objLoginData.ConnectionName;
                    }

                    try
                    {
                        Common.clsGlobal.DispDateFrom = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["DispDateFrom"].ToString().Trim()) * (-1);
                    }
                    catch
                    {
                        Common.clsGlobal.DispDateFrom = -60;
                    }

                    objCookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(objCookie);

                    ViewBag.ErrorMessage = "Success";
                    return Json(ViewBag.ErrorMessage, JsonRequestBehavior.AllowGet);

                }
            }
            else
            {
                ViewBag.ErrorMessage = "Username or password is incorrect";
                return Json(ViewBag.ErrorMessage, JsonRequestBehavior.AllowGet);
            }
        }

        //Added By RAVI on 06-11-2018
        [HttpGet]
        public ActionResult ExtendSession()
        {
            System.Web.Security.FormsAuthentication.SetAuthCookie(User.Identity.Name, false);
            var data = new { IsSuccess = true };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //END 
    }
}