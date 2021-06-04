using AutoMapper;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GsEPWv8_5_MVC.Controllers
{
    public class UserAccountController : Controller
    {
        // GET: UserAccount
        DataTable dtUserAcct = new DataTable();
        UserAccount objUserAccount = new UserAccount();
        IUserAccountService ServiceObject = new UserAccountService();
        Company objCompany = new Company();
        CompanyService ServiceObjectCompany = new CompanyService();
        public ActionResult UserAccount()
        {
            Mapper.CreateMap<UserAccount, UserAccountModel>();
            UserAccountModel objUserAccountModel = Mapper.Map<UserAccount, UserAccountModel>(objUserAccount);
            return View(objUserAccountModel);

        }
        public ActionResult GetUserDetails(string l_str_user_id, string l_str_user_fst_name, string l_str_user_lst_name)
        {
            try
            {
                objUserAccount.user_id = l_str_user_id;
                objUserAccount.user_fst_name = l_str_user_fst_name;
                objUserAccount.user_lst_name = l_str_user_lst_name;
                objUserAccount = ServiceObject.GetUserDetails(objUserAccount);
                Mapper.CreateMap<UserAccount, UserAccountModel>();
                UserAccountModel objUserAccountModel = Mapper.Map<UserAccount, UserAccountModel>(objUserAccount);
                return PartialView("_UserAccount", objUserAccountModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult AddUser()
        {
            int LineNum = 0;
            objCompany = ServiceObjectCompany.LoadUserType(objCompany);
            objUserAccount.ListUserType = objCompany.ListUserType;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objUserAccount.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            List<UserAccountDtl> li = new List<UserAccountDtl>();
            //2: Initialize a object of type DataRow
            DataRow drCmpPerm;
            //3: Initialize enough objects of type DataColumns
            DataColumn colCmpId = new DataColumn("cmp_id", typeof(string));
            DataColumn colCmpName = new DataColumn("cmp_name", typeof(string));
            DataColumn colChk = new DataColumn("colChk", typeof(string));
            int lintCount = 0;

            //4: Adding DataColumns to DataTable dt
            dtUserAcct.Columns.Add(colCmpId);
            dtUserAcct.Columns.Add(colCmpName);
            dtUserAcct.Columns.Add(colChk);
            objUserAccount = ServiceObject.GetDefaultCompanyDetails(objUserAccount);
            for (int i = 0; i < objUserAccount.ListCmpDtls.Count(); i++)
            {
                objUserAccount.cmp_id = objUserAccount.ListCmpDtls[i].cmp_id;
                objUserAccount.cmp_name = objUserAccount.ListCmpDtls[i].cmp_name;
                LineNum = LineNum + 1;

                drCmpPerm = dtUserAcct.NewRow();
                dtUserAcct.Rows.Add(drCmpPerm);
                dtUserAcct.Rows[lintCount][colCmpId] = objUserAccount.cmp_id.ToString();
                dtUserAcct.Rows[lintCount][colCmpName] = LineNum;
                dtUserAcct.Rows[lintCount][colChk] = "false";
                UserAccount objUserAccountDtl = new UserAccount();
                objUserAccountDtl.LineNum = LineNum;
                objUserAccountDtl.cmp_id = objUserAccount.cmp_id;
                objUserAccountDtl.cmp_name = objUserAccount.cmp_name;
                objUserAccountDtl.colChk = "false";
                li.Add(objUserAccountDtl);
                lintCount++;
            }
            objUserAccount.LstCmpPermDtls = li;
            Session["GridCmpPerm"] = objUserAccount.LstCmpPermDtls;
            objUserAccount.Status = "A";
            objUserAccount = ServiceObject.GetGridScnNameDetails(objUserAccount);
            objUserAccount.View_Flag = "A";
            Mapper.CreateMap<UserAccount, UserAccountModel>();
            UserAccountModel objUserAccountModel = Mapper.Map<UserAccount, UserAccountModel>(objUserAccount);
            return PartialView("_UserAccountNew", objUserAccountModel);
        }
        public ActionResult UserIdCheckExist(string l_str_userid)
        {
            objUserAccount.user_id = l_str_userid;
            objUserAccount = ServiceObject.CheckExistUserId(objUserAccount);
            return Json(objUserAccount.ListCheckUserIdExist.Count, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UserAccountCmpPermDtls(int l_str_Row_No, string l_str_checked_value)
        {
            List<UserAccountDtl> CmpPermLi = new List<UserAccountDtl>();
            CmpPermLi = Session["GridCmpPerm"] as List<UserAccountDtl>;
            if (CmpPermLi != null)
            {
                var result = from r in CmpPermLi where r.LineNum == l_str_Row_No select new { r.cmp_id, r.cmp_name, r.LineNum };
                var get_result = result.ToList();

                var l_str_line_num = get_result[0].LineNum;
                var l_str_chk_value = l_str_checked_value;
                for (int j = 0; j < CmpPermLi.Count(); j++)
                {

                    CmpPermLi.Where(p => p.LineNum == l_str_Row_No).Select(u =>
                    {
                        u.colChk = l_str_checked_value;
                        return u;
                    }).ToList();
                }

                objUserAccount.LstCmpPermDtls = CmpPermLi;
                List<UserAccountDtl> GETCmpList = new List<UserAccountDtl>();
                GETCmpList = Session["GridUpdCmpPerm"] as List<UserAccountDtl>;
                Session["GridUpdCmpPerm"] = objUserAccount.LstCmpPermDtls;
            }
            return Json(objUserAccount.LstCmpPermDtls, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewUser(string l_str_user_id, string l_str_user_type)
        {
            int LineNum = 0;
            string l_str_scn_id = string.Empty;
            string l_str_view = string.Empty;
            string l_str_add = string.Empty;
            string l_str_mod = string.Empty;
            string l_str_post = string.Empty;
            string l_str_unpost = string.Empty;
            string l_str_del = string.Empty;
            string l_str_cmp_id = string.Empty;
            objUserAccount.user_id = l_str_user_id.Trim();
            objUserAccount.Status = "A";
            objUserAccount = ServiceObject.GetUserDetails(objUserAccount);
            objUserAccount.first_name = objUserAccount.ListUserDetails[0].user_fst_name;
            objUserAccount.last_name = objUserAccount.ListUserDetails[0].user_lst_name;
            objUserAccount.usr_type = objUserAccount.ListUserDetails[0].user_type;
            objCompany = ServiceObjectCompany.LoadUserType(objCompany);
            objUserAccount.ListUserType = objCompany.ListUserType;
            objUserAccount.password = objUserAccount.ListUserDetails[0].passwd;
            objUserAccount.cfm_password = objUserAccount.ListUserDetails[0].passwd;
            objUserAccount.email = objUserAccount.ListUserDetails[0].email;
            objUserAccount.tel = objUserAccount.ListUserDetails[0].tel;
            objUserAccount.usr_type = l_str_user_type;
            dtUserAcct = new DataTable();
            List<UserAccountDtl> li = new List<UserAccountDtl>();
            //2: Initialize a object of type DataRow
            DataRow drCmpPerm;
            //3: Initialize enough objects of type DataColumns
            DataColumn colCmpId = new DataColumn("cmp_id", typeof(string));
            DataColumn colCmpName = new DataColumn("cmp_name", typeof(string));
            DataColumn colChk = new DataColumn("colChk", typeof(string));
            int lintCount = 0;

            //4: Adding DataColumns to DataTable dt
            dtUserAcct.Columns.Add(colCmpId);
            dtUserAcct.Columns.Add(colCmpName);
            dtUserAcct.Columns.Add(colChk);
            objUserAccount = ServiceObject.GetDefaultCompanyDetails(objUserAccount);
            for (int i = 0; i < objUserAccount.ListCmpDtls.Count(); i++)
            {
                objUserAccount.cmp_id = objUserAccount.ListCmpDtls[i].cmp_id;
                objUserAccount.cmp_name = objUserAccount.ListCmpDtls[i].cmp_name;
                LineNum = LineNum + 1;

                drCmpPerm = dtUserAcct.NewRow();
                dtUserAcct.Rows.Add(drCmpPerm);
                dtUserAcct.Rows[lintCount][colCmpId] = objUserAccount.cmp_id.ToString();
                dtUserAcct.Rows[lintCount][colCmpName] = LineNum;
                dtUserAcct.Rows[lintCount][colChk] = "false";
                UserAccount objUserAccountDtl = new UserAccount();
                objUserAccountDtl.LineNum = LineNum;
                objUserAccountDtl.cmp_id = objUserAccount.cmp_id;
                objUserAccountDtl.cmp_name = objUserAccount.cmp_name;
                objUserAccountDtl.colChk = "false";
                li.Add(objUserAccountDtl);
                lintCount++;

            }
            objUserAccount.LstCmpPermDtls = li;
            Session["GridCmpPerm"] = objUserAccount.LstCmpPermDtls;
            List<UserAccountDtl> CmpyPermLi = new List<UserAccountDtl>();
            CmpyPermLi = Session["GridCmpPerm"] as List<UserAccountDtl>;
            objUserAccount = ServiceObject.GetUserPermCompanyDetails(objUserAccount);
            for (int j = 0; j < objUserAccount.LstCmpPermDtls.Count(); j++)
            {
                objUserAccount.cmp_id = objUserAccount.LstCmpPermDtls[j].cmp_id;
                l_str_cmp_id = objUserAccount.cmp_id;
                for (int i = 0; i < objUserAccount.ListLoadUserDetails.Count(); i++)
                {
                    if (objUserAccount.ListLoadUserDetails[i].cmp_id.Trim() == l_str_cmp_id.Trim())
                    {
                        for (int k = 0; k < CmpyPermLi.Count(); k++)
                        {

                            CmpyPermLi.Where(p => p.cmp_id == l_str_cmp_id).Select(u =>
                            {
                                u.colChk = "true";
                                return u;
                            }).ToList();
                        }
                    }
                    else
                    {
                        for (int k = 0; k < CmpyPermLi.Count(); k++)
                        {

                            CmpyPermLi.Where(p => p.cmp_id == l_str_scn_id).Select(u =>
                            {
                                u.colChk = "false";
                                return u;
                            }).ToList();
                        }
                    }
                }
                continue;
            }
            objUserAccount.LstModCmpPermDtls = CmpyPermLi;
            Session["GridUpdCmpPerm"] = objUserAccount.LstModCmpPermDtls;
            objUserAccount = ServiceObject.GetDfltCmpId(objUserAccount);
            objUserAccount.cmp_id = objUserAccount.ListGetDfltCmpId[0].dflt_cust_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objUserAccount.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objUserAccount.View_Flag = "V";
            Mapper.CreateMap<UserAccount, UserAccountModel>();
            UserAccountModel objUserAccountModel = Mapper.Map<UserAccount, UserAccountModel>(objUserAccount);
            return PartialView("_UserAccountNew", objUserAccountModel);
        }
        public ActionResult EditUser(string l_str_user_id, string l_str_user_type)
        {
            int LineNum = 0;
            string l_str_scn_id = string.Empty;
            string l_str_view = string.Empty;
            string l_str_add = string.Empty;
            string l_str_mod = string.Empty;
            string l_str_post = string.Empty;
            string l_str_unpost = string.Empty;
            string l_str_del = string.Empty;
            string l_str_cmp_id = string.Empty;
            objUserAccount.user_id = l_str_user_id.Trim();
            objUserAccount.Status = "A";
            objUserAccount = ServiceObject.GetUserDetails(objUserAccount);
            objUserAccount.first_name = objUserAccount.ListUserDetails[0].user_fst_name;
            objUserAccount.last_name = objUserAccount.ListUserDetails[0].user_lst_name;
            objUserAccount.usr_type = objUserAccount.ListUserDetails[0].user_type;
            objCompany = ServiceObjectCompany.LoadUserType(objCompany);
            objUserAccount.ListUserType = objCompany.ListUserType;
            objUserAccount.password = objUserAccount.ListUserDetails[0].passwd;
            objUserAccount.cfm_password = objUserAccount.ListUserDetails[0].passwd;
            objUserAccount.email = objUserAccount.ListUserDetails[0].email;
            objUserAccount.tel = objUserAccount.ListUserDetails[0].tel;
            objUserAccount.usr_type = l_str_user_type;
            dtUserAcct = new DataTable();
            List<UserAccountDtl> li = new List<UserAccountDtl>();
            //2: Initialize a object of type DataRow
            DataRow drCmpPerm;
            //3: Initialize enough objects of type DataColumns
            DataColumn colCmpId = new DataColumn("cmp_id", typeof(string));
            DataColumn colCmpName = new DataColumn("cmp_name", typeof(string));
            DataColumn colChk = new DataColumn("colChk", typeof(string));
            int lintCount = 0;

            //4: Adding DataColumns to DataTable dt
            dtUserAcct.Columns.Add(colCmpId);
            dtUserAcct.Columns.Add(colCmpName);
            dtUserAcct.Columns.Add(colChk);
            objUserAccount = ServiceObject.GetDefaultCompanyDetails(objUserAccount);
            for (int i = 0; i < objUserAccount.ListCmpDtls.Count(); i++)
            {
                objUserAccount.cmp_id = objUserAccount.ListCmpDtls[i].cmp_id;
                objUserAccount.cmp_name = objUserAccount.ListCmpDtls[i].cmp_name;
                LineNum = LineNum + 1;

                drCmpPerm = dtUserAcct.NewRow();
                dtUserAcct.Rows.Add(drCmpPerm);
                dtUserAcct.Rows[lintCount][colCmpId] = objUserAccount.cmp_id.ToString();
                dtUserAcct.Rows[lintCount][colCmpName] = LineNum;
                dtUserAcct.Rows[lintCount][colChk] = "false";
                UserAccount objUserAccountDtl = new UserAccount();
                objUserAccountDtl.LineNum = LineNum;
                objUserAccountDtl.cmp_id = objUserAccount.cmp_id;
                objUserAccountDtl.cmp_name = objUserAccount.cmp_name;
                objUserAccountDtl.colChk = "false";
                li.Add(objUserAccountDtl);
                lintCount++;

            }
            objUserAccount.LstCmpPermDtls = li;
            Session["GridCmpPerm"] = objUserAccount.LstCmpPermDtls;
            List<UserAccountDtl> CmpyPermLi = new List<UserAccountDtl>();
            CmpyPermLi = Session["GridCmpPerm"] as List<UserAccountDtl>;
            objUserAccount = ServiceObject.GetUserPermCompanyDetails(objUserAccount);
            for (int j = 0; j < objUserAccount.LstCmpPermDtls.Count(); j++)
            {
                objUserAccount.cmp_id = objUserAccount.LstCmpPermDtls[j].cmp_id;
                l_str_cmp_id = objUserAccount.cmp_id;
                for (int i = 0; i < objUserAccount.ListLoadUserDetails.Count(); i++)
                {
                    if (objUserAccount.ListLoadUserDetails[i].cmp_id.Trim() == l_str_cmp_id.Trim())
                    {
                        for (int k = 0; k < CmpyPermLi.Count(); k++)
                        {

                            CmpyPermLi.Where(p => p.cmp_id == l_str_cmp_id).Select(u =>
                            {
                                u.colChk = "true";
                                return u;
                            }).ToList();
                        }
                    }
                    else
                    {
                        for (int k = 0; k < CmpyPermLi.Count(); k++)
                        {

                            CmpyPermLi.Where(p => p.cmp_id == l_str_scn_id).Select(u =>
                            {
                                u.colChk = "false";
                                return u;
                            }).ToList();
                        }
                    }
                }
                continue;
            }
            objUserAccount.LstModCmpPermDtls = CmpyPermLi;
            Session["GridUpdCmpPerm"] = objUserAccount.LstModCmpPermDtls;
            objUserAccount = ServiceObject.GetDfltCmpId(objUserAccount);
            objUserAccount.cmp_id = objUserAccount.ListGetDfltCmpId[0].dflt_cust_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objUserAccount.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objUserAccount.View_Flag = "M";
            Mapper.CreateMap<UserAccount, UserAccountModel>();
            UserAccountModel objUserAccountModel = Mapper.Map<UserAccount, UserAccountModel>(objUserAccount);
            return PartialView("_UserAccountNew", objUserAccountModel);
        }
        [HttpPost]
        public JsonResult SaveUserDetails(List<UserAccountHdr> UserAccHdr)
        {
            bool l_bol_dflt_cust_selected = false;
            foreach (UserAccountHdr UserHdr in UserAccHdr)
            {
                objUserAccount.user_id = (UserHdr.user_id == null ? string.Empty : UserHdr.user_id);
                objUserAccount.first_name = (UserHdr.user_fst_name == null ? string.Empty : UserHdr.user_fst_name);
                objUserAccount.last_name = (UserHdr.user_lst_name == null ? string.Empty : UserHdr.user_lst_name);
                objUserAccount.password = (UserHdr.password == null ? string.Empty : UserHdr.password);
                objUserAccount.usr_type = (UserHdr.usr_type == null ? string.Empty : UserHdr.usr_type);
                objUserAccount.email = (UserHdr.email == null ? string.Empty : UserHdr.email);
                objUserAccount.tel = (UserHdr.tel == null ? string.Empty : UserHdr.tel);
                objUserAccount.cmp_perm1 = System.Configuration.ConfigurationManager.AppSettings["DefaultCompID"].ToString().Trim();
                objUserAccount.dflt_cust_id = (UserHdr.dflt_cust_id == null ? string.Empty : UserHdr.dflt_cust_id);
                if (UserHdr.dflt_cust_id == null)
                {
                    l_bol_dflt_cust_selected = true;
                }
                else
                {
                    objUserAccount.LstCmpPerm = Session["GridUpdCmpPerm"] as List<UserAccountDtl>;
                    if (objUserAccount.LstCmpPerm != null)
                    { 
                        for (int i = 0; i < objUserAccount.LstCmpPerm.Count; i++)
                        {
                            if (objUserAccount.LstCmpPerm[i].colChk == "true")
                            {
                                if (UserHdr.dflt_cust_id.ToString().Trim() == objUserAccount.LstCmpPerm[i].cmp_id.Trim())
                                {
                                    l_bol_dflt_cust_selected = true;
                                    break;
                                }

                            }
                        }
                }
                }
            }
                if (l_bol_dflt_cust_selected == true)
                {
                    ServiceObject.SaveUserDtls(objUserAccount);


                    objUserAccount.LstCmpPerm = Session["GridUpdCmpPerm"] as List<UserAccountDtl>;
                    if (objUserAccount.LstCmpPerm != null)
                    {
                        for (int i = 0; i < objUserAccount.LstCmpPerm.Count; i++)
                        {
                            if (objUserAccount.LstCmpPerm[i].colChk == "true")
                            {
                                objUserAccount.cmp_id = objUserAccount.LstCmpPerm[i].cmp_id.Trim();
                                objUserAccount.cmp_name = objUserAccount.LstCmpPerm[i].cmp_name.Trim();
                                ServiceObject.SaveCmpPerm(objUserAccount);
                            }
                        }
                    }
                }
            
            UserAccHdr.Clear();
            return Json(l_bol_dflt_cust_selected, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateUserDtls(List<UserAccountHdr> UserAccHdr)
        {
            bool l_bol_dflt_cust_selected = false;
            foreach (UserAccountHdr UserHdr in UserAccHdr)
            {
                objUserAccount.user_id = (UserHdr.user_id == null ? string.Empty : UserHdr.user_id);
                objUserAccount.first_name = (UserHdr.user_fst_name == null ? string.Empty : UserHdr.user_fst_name);
                objUserAccount.last_name = (UserHdr.user_lst_name == null ? string.Empty : UserHdr.user_lst_name);
                objUserAccount.password = (UserHdr.password == null ? string.Empty : UserHdr.password);
                objUserAccount.usr_type = (UserHdr.usr_type == null ? string.Empty : UserHdr.usr_type);
                objUserAccount.email = (UserHdr.email == null ? string.Empty : UserHdr.email);
                objUserAccount.tel = (UserHdr.tel == null ? string.Empty : UserHdr.tel);
                objUserAccount.cmp_perm1 = System.Configuration.ConfigurationManager.AppSettings["DefaultCompID"].ToString().Trim();
                objUserAccount.dflt_cust_id = (UserHdr.dflt_cust_id == null ? string.Empty : UserHdr.dflt_cust_id);
                
                if (UserHdr.dflt_cust_id == null)
                {
                    l_bol_dflt_cust_selected = true;
                }
                else
                { 
                objUserAccount.LstCmpPerm = Session["GridUpdCmpPerm"] as List<UserAccountDtl>;
                    if (objUserAccount.LstCmpPerm != null)
                    {
                        for (int i = 0; i < objUserAccount.LstCmpPerm.Count; i++)
                        {
                            if (objUserAccount.LstCmpPerm[i].colChk == "true")
                            {
                                if (UserHdr.dflt_cust_id.ToString().Trim() == objUserAccount.LstCmpPerm[i].cmp_id.Trim())
                                {
                                    l_bol_dflt_cust_selected = true;
                                    break;
                                }

                            }
                        }
                    }
                }

               
               
            }

            if (l_bol_dflt_cust_selected == true)
            {
                ServiceObject.UpdateUserDtls(objUserAccount);
                if (Session["GridUpdCmpPerm"] != null)
                {
                    ServiceObject.DeleteCmpPerm(objUserAccount);

                    objUserAccount.LstCmpPerm = Session["GridUpdCmpPerm"] as List<UserAccountDtl>;
                    for (int i = 0; i < objUserAccount.LstCmpPerm.Count; i++)
                    {
                        if (objUserAccount.LstCmpPerm[i].colChk == "true")
                        {
                            objUserAccount.cmp_id = objUserAccount.LstCmpPerm[i].cmp_id.Trim();
                            objUserAccount.cmp_name = objUserAccount.LstCmpPerm[i].cmp_name.Trim();
                            ServiceObject.SaveCmpPerm(objUserAccount);
                        }
                    }
                }
            }
            UserAccHdr.Clear();
            return Json(l_bol_dflt_cust_selected, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteUser(string l_str_user_id)
        {
            int LineNum = 0;
            string l_str_scn_id = string.Empty;
            string l_str_view = string.Empty;
            string l_str_add = string.Empty;
            string l_str_mod = string.Empty;
            string l_str_post = string.Empty;
            string l_str_unpost = string.Empty;
            string l_str_del = string.Empty;
            string l_str_cmp_id = string.Empty;
            UserAccount objUserAccount = new UserAccount();
            IUserAccountService ServiceObject = new UserAccountService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objUserAccount.user_id = l_str_user_id.Trim();
            objUserAccount.Status = "A";
            objUserAccount = ServiceObject.GetUserDetails(objUserAccount);
            objUserAccount.first_name = objUserAccount.ListUserDetails[0].user_fst_name;
            objUserAccount.last_name = objUserAccount.ListUserDetails[0].user_lst_name;
            objUserAccount.usr_type = objUserAccount.ListUserDetails[0].user_type;
            objCompany = ServiceObjectCompany.LoadUserType(objCompany);
            objUserAccount.ListUserType = objCompany.ListUserType;
            objUserAccount.password = objUserAccount.ListUserDetails[0].passwd;
            objUserAccount.cfm_password = objUserAccount.ListUserDetails[0].passwd;
            objUserAccount.email = objUserAccount.ListUserDetails[0].email;
            objUserAccount.tel = objUserAccount.ListUserDetails[0].tel;
            objUserAccount.usr_type = objUserAccount.ListUserDetails[0].user_type;
            dtUserAcct = new DataTable();
            List<UserAccountDtl> li = new List<UserAccountDtl>();
            //2: Initialize a object of type DataRow
            DataRow drCmpPerm;
            //3: Initialize enough objects of type DataColumns
            DataColumn colCmpId = new DataColumn("cmp_id", typeof(string));
            DataColumn colCmpName = new DataColumn("cmp_name", typeof(string));
            DataColumn colChk = new DataColumn("colChk", typeof(string));
            int lintCount = 0;

            //4: Adding DataColumns to DataTable dt
            dtUserAcct.Columns.Add(colCmpId);
            dtUserAcct.Columns.Add(colCmpName);
            dtUserAcct.Columns.Add(colChk);
            objUserAccount = ServiceObject.GetDefaultCompanyDetails(objUserAccount);
            for (int i = 0; i < objUserAccount.ListCmpDtls.Count(); i++)
            {
                objUserAccount.cmp_id = objUserAccount.ListCmpDtls[i].cmp_id;
                objUserAccount.cmp_name = objUserAccount.ListCmpDtls[i].cmp_name;
                LineNum = LineNum + 1;

                drCmpPerm = dtUserAcct.NewRow();
                dtUserAcct.Rows.Add(drCmpPerm);
                dtUserAcct.Rows[lintCount][colCmpId] = objUserAccount.cmp_id.ToString();
                dtUserAcct.Rows[lintCount][colCmpName] = LineNum;
                dtUserAcct.Rows[lintCount][colChk] = "false";
                UserAccount objUserAccountDtl = new UserAccount();
                objUserAccountDtl.LineNum = LineNum;
                objUserAccountDtl.cmp_id = objUserAccount.cmp_id;
                objUserAccountDtl.cmp_name = objUserAccount.cmp_name;
                objUserAccountDtl.colChk = "false";
                li.Add(objUserAccountDtl);
                lintCount++;

            }
            objUserAccount.LstCmpPermDtls = li;
            Session["GridModCmpPerm"] = objUserAccount.LstCmpPermDtls;
            List<UserAccountDtl> CmpyPermLi = new List<UserAccountDtl>();
            CmpyPermLi = Session["GridModCmpPerm"] as List<UserAccountDtl>;
            objUserAccount = ServiceObject.GetUserPermCompanyDetails(objUserAccount);
            for (int j = 0; j < objUserAccount.ListLoadUserDetails.Count(); j++)
            {
                objUserAccount.cmp_id = objUserAccount.ListLoadUserDetails[j].cmp_id;
                l_str_cmp_id = objUserAccount.cmp_id;
                for (int i = 0; i < objUserAccount.ListLoadUserDetails.Count(); i++)
                {
                    if (objUserAccount.ListLoadUserDetails[i].cmp_id.Trim() == l_str_cmp_id.Trim())
                    {
                        for (int k = 0; k < CmpyPermLi.Count(); k++)
                        {

                            CmpyPermLi.Where(p => p.cmp_id == l_str_cmp_id).Select(u =>
                            {
                                u.colChk = "true";
                                return u;
                            }).ToList();
                        }
                    }
                    else
                    {
                        for (int k = 0; k < CmpyPermLi.Count(); k++)
                        {

                            CmpyPermLi.Where(p => p.cmp_id == l_str_scn_id).Select(u =>
                            {
                                u.colChk = "false";
                                return u;
                            }).ToList();
                        }
                    }
                }
                continue;
            }
            objUserAccount.LstModCmpPermDtls = CmpyPermLi;
            Session["GridUpdCmpPerm"] = objUserAccount.LstModCmpPermDtls;
            objUserAccount = ServiceObject.GetDfltCmpId(objUserAccount);
            objUserAccount.cmp_id = objUserAccount.ListGetDfltCmpId[0].dflt_cust_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objUserAccount.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objUserAccount.Status = "B";
            objUserAccount.View_Flag = "D";
            Mapper.CreateMap<UserAccount, UserAccountModel>();
            UserAccountModel objUserAccountModel = Mapper.Map<UserAccount, UserAccountModel>(objUserAccount);
            return PartialView("_UserAccountNew", objUserAccountModel);
        }
        public ActionResult DeleteUserDtls(string l_str_user_id)
        {
            int l_str_success = 0;
            objUserAccount.user_id = l_str_user_id.Trim();
            if (l_str_user_id != Session["UserID"].ToString().Trim())
            {
                ServiceObject.DeleteUserDtls(objUserAccount);
                ServiceObject.DeleteCmpPerm(objUserAccount);
                return Json(l_str_success, JsonRequestBehavior.AllowGet);
            }

            else
            {
                l_str_success = 1;
                return Json(l_str_success, JsonRequestBehavior.AllowGet);
            }
        }


    }
}