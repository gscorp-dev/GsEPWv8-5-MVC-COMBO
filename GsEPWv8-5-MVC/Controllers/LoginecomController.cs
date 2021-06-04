using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Common;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GsEPWv8_5_MVC.Controllers
{
    public class LoginecomController : Controller
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
            objModel.Showday = DateTime.Now.ToLongDateString();
            Session["DisplayDate"] = DateTime.Now.ToLongDateString();
            if (objLoginData != null)
            {
                if (objLoginData.Email == "")
                {
                    ViewBag.ErrorMessage = "Username or password is incorrect";
                    return View(objModel);
                }
                else
                {
                    //this.Session["SessionID"] = System.Web.HttpContext.Current.Session.SessionID;

                    //COMMENT BY RAVI BELOW

                    Session["UserID"] = objLoginData.user_id;

                    Session["UserfstName"] = objLoginData.user_fst_name;
                    Session["UserlstName"] = objLoginData.user_lst_name;

                    //Session["UserName"] = objLoginData.Email;
                    Session["dflt_cmp_id"] = objLoginData.dflt_cmp_id.Trim();
                    HttpCookie objCookie = new HttpCookie("BasicInformation");
                    objCookie["UserID"] = objLoginData.user_id.ToString();
                    //objCookie["UserName"] = objLoginData.Email.ToString();//dflt_cmp_id
                    objCookie["dflt_cmp_id"] = objLoginData.dflt_cmp_id.ToString().Trim();
                    objCookie["dflt_is_mob_user"] = objLoginData.ismob_user.ToString().Trim();
                    objCookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(objCookie);

                    //COMMENT BY RAVI ABOVE
                    if(objCookie["dflt_is_mob_user"] =="Y")
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
            return RedirectToAction("loginView");
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
            objModel.Showday = DateTime.Now.ToLongDateString();
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
            objModel.Showday = DateTime.Now.ToLongDateString();
            Session["DisplayDate"] = DateTime.Now.ToLongDateString();
            Session["g_str_cmp_id"] = string.Empty;

            if (objLoginData != null)
            {
                if (objLoginData.Email == "")
                {
                    ViewBag.ErrorMessage = "Username or password is incorrect";
                    return View(objModel);
                }
                else
                {
                    Session["UserfstName"] = objLoginData.user_fst_name;
                    Session["UserlstName"] = objLoginData.user_lst_name;
                    Session["UserID"] = objLoginData.user_id;
                    Session["dflt_cmp_id"] = objLoginData.dflt_cust_id.Trim();
                    HttpCookie objCookie = new HttpCookie("BasicInformation");
                    objCookie["UserID"] = objLoginData.user_id.ToString();
                    objCookie["dflt_cmp_id"] = objLoginData.dflt_cust_id.ToString().Trim();
                    objCookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(objCookie);
                    // Adding for Combo
                    Session["g_str_cmp_id"] = objLoginData.dflt_cust_id.ToString().Trim();
                    // 
                    if (objLoginData.ConnectionName == null)
                    {
                        Session["ConnectionName"] = "";
                    }
                    else
                    {
                        Session["ConnectionName"] = objLoginData.ConnectionName;
                    }

                    if (objLoginData.is_cmp_user == null)
                    {
                        Session["IsCompanyUser"] = "";
                        clsGlobal.IsCompanyUser = "";

                    }
                    else
                    {
                        Session["IsCompanyUser"] = objLoginData.is_cmp_user.Trim();
                        clsGlobal.IsCompanyUser = objLoginData.is_cmp_user.Trim();
                    }
                    
                    if (objLoginData.user_type == null)
                    {
                        Session["UserType"] = "Customer";
                    }
                    else
                    {
                        Session["UserType"] = objLoginData.user_type.Trim();
                    }
                    if (objLoginData.IS3RDUSER == null)
                    {
                        Session["IS3RDUSER"] = "";

                    }
                    else
                    {
                        Session["IS3RDUSER"] = objLoginData.IS3RDUSER;
                    }

                    try
                    {
                        Common.clsGlobal.DispDateFrom = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["DispDateFromEcom"].ToString().Trim()) * (-1);

                    }
                    catch
                    {
                        Common.clsGlobal.DispDateFrom = -30;
                    }

                    objModel.DFLT_DT_REQD = System.Configuration.ConfigurationManager.AppSettings["DFLT_DT_REQD"].ToString();
                    Session["DFLT_DT_REQD"] = objModel.DFLT_DT_REQD;
                    objModel.CompanyWebLink = System.Configuration.ConfigurationManager.AppSettings["CompanyWebLink"].ToString();
                    if (objModel.CompanyWebLink == "3plpro.gensoftcorp.com")
                    {
                        return RedirectToAction("DashBoard", "DashBoard");
                    }
                    else if (objModel.CompanyWebLink == "3plecom.gensoftcorp.com")
                    {
                        return RedirectToAction("EcommDashBoard", "ECommDashBoard");
                    }
                    else
                    {
                        return null;
                    }

          
            }
            }
            else
            {
                ViewBag.ErrorMessage = "Username or password is incorrect";
                return View(objModel);
            }
        }

    }
}