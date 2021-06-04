using AutoMapper;
using GsEPWv8_4_MVC.Business.Implementation;
using GsEPWv8_4_MVC.Business.Interface;
using GsEPWv8_4_MVC.Core.Entity;
using GsEPWv8_4_MVC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GsEPWv8_4_MVC.Controllers
{
    public class UserAccountController : Controller
    {
        // GET: UserAccount
        DataTable dtUserAcct;
        DataTable dtUserAcctPerm;

        public ActionResult UserAccount()
        {
            UserAccount objUserAccount = new UserAccount();
            IUserAccountService ServiceObject = new UserAccountService();
            //objUserAccount = ServiceObject.GetGridPermissionDetails(objUserAccount);
            //objUserAccount = ServiceObject.GetGridScnNameDetails(objUserAccount);
            Mapper.CreateMap<UserAccount, UserAccountModel>();
            UserAccountModel objUserAccountModel = Mapper.Map<UserAccount, UserAccountModel>(objUserAccount);
            return View(objUserAccountModel);
            
        }
        public ActionResult GetUserDetails(string p_str_user_id)
        {
            try
            {
                UserAccount objUserAccount = new UserAccount();
                IUserAccountService ServiceObject = new UserAccountService();
                objUserAccount.user_id = p_str_user_id;
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
        public ActionResult New()
        {
            int LineNum = 0;
            UserAccount objUserAccount = new UserAccount();
            IUserAccountService ServiceObject = new UserAccountService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany = ServiceObjectCompany.LoadUserType(objCompany);
            objUserAccount.ListUserType = objCompany.ListUserType;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objUserAccount.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
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
            objUserAccount = ServiceObject.GetGridPermissionDetails(objUserAccount);
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
            dtUserAcctPerm = new DataTable();
            List<UserAccountDtl> UserPermLi = new List<UserAccountDtl>();
            //2: Initialize a object of type DataRow
            DataRow drScnPerm;
            //3: Initialize enough objects of type DataColumns
            DataColumn colScnNo = new DataColumn("scn_no", typeof(string));
            DataColumn colScnId = new DataColumn("scn_id", typeof(string));
            DataColumn colStatus = new DataColumn("status", typeof(string));
            DataColumn colScnName = new DataColumn("scn_name", typeof(string));
            DataColumn colScnCaption = new DataColumn("scn_caption", typeof(string));
            DataColumn colScnView = new DataColumn("scn_view", typeof(string));
            DataColumn colScnAdd = new DataColumn("scn_add", typeof(string));
            DataColumn colScnMod = new DataColumn("scn_mod", typeof(string));
            DataColumn colScnDel = new DataColumn("scn_del", typeof(string));
            DataColumn colScnPost = new DataColumn("scn_post", typeof(string));
            DataColumn colCheck = new DataColumn("colCheck", typeof(string));
            int lineCount = 0;
            dtUserAcctPerm.Columns.Add(colScnNo);
            dtUserAcctPerm.Columns.Add(colScnId);
            dtUserAcctPerm.Columns.Add(colStatus);
            dtUserAcctPerm.Columns.Add(colScnName);
            dtUserAcctPerm.Columns.Add(colScnCaption);
            dtUserAcctPerm.Columns.Add(colScnView);
            dtUserAcctPerm.Columns.Add(colScnAdd);
            dtUserAcctPerm.Columns.Add(colScnMod);
            dtUserAcctPerm.Columns.Add(colScnDel);
            dtUserAcctPerm.Columns.Add(colScnPost);
            dtUserAcctPerm.Columns.Add(colCheck);
            objUserAccount = ServiceObject.GetGridScnNameDetails(objUserAccount);
            for (int j = 0; j < objUserAccount.ListScnDtls.Count(); j++)
            {
                objUserAccount.scn_caption = objUserAccount.ListScnDtls[j].scn_caption;
                objUserAccount.scn_view = objUserAccount.ListScnDtls[j].scn_view;
                objUserAccount.scn_add = objUserAccount.ListScnDtls[j].scn_add;
                objUserAccount.scn_mod = objUserAccount.ListScnDtls[j].scn_mod;
                objUserAccount.scn_del = objUserAccount.ListScnDtls[j].scn_del;
                objUserAccount.scn_post = objUserAccount.ListScnDtls[j].scn_post;
                LineNum = LineNum + 1;

                drScnPerm = dtUserAcctPerm.NewRow();
                dtUserAcctPerm.Rows.Add(drScnPerm);
                dtUserAcctPerm.Rows[lineCount][colScnCaption] = objUserAccount.scn_caption;
                dtUserAcctPerm.Rows[lineCount][colScnView] = objUserAccount.scn_view;
                dtUserAcctPerm.Rows[lineCount][colScnAdd] = objUserAccount.scn_add;
                dtUserAcctPerm.Rows[lineCount][colScnMod] = objUserAccount.scn_mod;
                dtUserAcctPerm.Rows[lineCount][colScnDel] = objUserAccount.scn_del;
                dtUserAcctPerm.Rows[lineCount][colScnPost] = objUserAccount.scn_post;
                dtUserAcctPerm.Rows[lineCount][colCheck] = "false";
                UserAccount objUserAccountPermDtl = new UserAccount();
                objUserAccountPermDtl.LineNum = LineNum;
                objUserAccountPermDtl.scn_caption = objUserAccount.scn_caption;
                objUserAccountPermDtl.scn_view = objUserAccount.scn_view;
                objUserAccountPermDtl.scn_add = objUserAccount.scn_add;
                objUserAccountPermDtl.scn_mod = objUserAccount.scn_mod;
                objUserAccountPermDtl.scn_del = objUserAccount.scn_del;
                objUserAccountPermDtl.scn_post = objUserAccount.scn_post;
                objUserAccountPermDtl.colCheck = "false";
                UserPermLi.Add(objUserAccountPermDtl);
                lineCount++;

            }
            objUserAccount.LstScnPermDtls = UserPermLi;
            Session["GridScnPerm"] = objUserAccount.LstScnPermDtls;
            objUserAccount.View_Flag = "A";
            Mapper.CreateMap<UserAccount, UserAccountModel>();
            UserAccountModel objUserAccountModel = Mapper.Map<UserAccount, UserAccountModel>(objUserAccount);
            return PartialView("_UserAccountNew", objUserAccountModel);
        }
        public ActionResult UserIdCheckExist(string p_str_userid)
        {
            //string l_str_userid_count = string.Empty;
            UserAccount objUserAccount = new UserAccount();
            IUserAccountService ServiceObject = new UserAccountService();
            objUserAccount.user_id = p_str_userid;
            objUserAccount = ServiceObject.CheckExistUserId(objUserAccount);
             //l_str_userid_count = Convert.ToString(objUserAccount.ListCheckUserIdExist);
            return Json(objUserAccount.ListCheckUserIdExist.Count, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UserAccountCmpPermDtls(int p_str_RowNo, string Checkvalue)
        {
            UserAccount objUserAccount = new UserAccount();
            IUserAccountService ServiceObject = new UserAccountService();
            List<UserAccountDtl> CmpPermLi = new List<UserAccountDtl>();
            CmpPermLi = Session["GridCmpPerm"] as List<UserAccountDtl>;
            var result = from r in CmpPermLi where r.LineNum == p_str_RowNo select new { r.cmp_id, r.cmp_name,r.LineNum };
            var get_result = result.ToList();
          
            var l_str_line_num = get_result[0].LineNum;
            var l_str_chk_value = Checkvalue;            
                for (int j = 0; j < CmpPermLi.Count(); j++)
                {

                    CmpPermLi.Where(p => p.LineNum == p_str_RowNo).Select(u =>
                    {                       
                        u.colChk = Checkvalue;
                        return u;
                    }).ToList();
                }                            
            
            objUserAccount.LstCmpPermDtls = CmpPermLi;
            Session["GridUpdCmpPerm"] = objUserAccount.LstCmpPermDtls;
            //Session["lblAlocated"] = 0;
            List<UserAccountDtl> GETCmpList = new List<UserAccountDtl>();
            GETCmpList = Session["GridUpdCmpPerm"] as List<UserAccountDtl>;
            return Json(objUserAccount.LstCmpPermDtls, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckDefaultCustId(string p_str_cmp_id, string p_str_user_id)
        {
            UserAccount objUserAccount = new UserAccount();
            IUserAccountService ServiceObject = new UserAccountService();
            List<UserAccountDtl> CmpPermLi = new List<UserAccountDtl>();
            CmpPermLi = Session["GridCmpPerm"] as List<UserAccountDtl>;
            //var result = from r in CmpPermLi where r.cmp_id == p_str_cmp_id select new { r.cmp_id, r.colChk };
            //var get_result = result.ToList();
            bool l_str_result = false;
            //var l_str_cmp_id = get_result[0].cmp_id; 
            //var l_str_chk_value = get_result[0].colChk;

          
                for (int i = 0; i < CmpPermLi.Count; i++)
                {
                if(CmpPermLi[i].colChk.Trim() == "true")
                {
                    if (CmpPermLi[i].cmp_id.Trim() == p_str_cmp_id)
                    {
                        l_str_result = true;
                    }
                }

            }
            return Json(l_str_result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Edit(string p_str_user_id)
        {
            int LineNum = 0;
            string l_str_scn_id = string.Empty;
            string l_str_view = string.Empty;
            string l_str_add = string.Empty;
            string l_str_mod = string.Empty;
            string l_str_post = string.Empty;
            string l_str_del = string.Empty;
            string l_str_cmp_id = string.Empty;
            UserAccount objUserAccount = new UserAccount();
            IUserAccountService ServiceObject = new UserAccountService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objUserAccount.user_id = p_str_user_id.Trim();
            //objUserAccount = ServiceObject.LoadUserType(objUserAccount);
            objUserAccount = ServiceObject.LoadUserDtls(objUserAccount);
            objUserAccount.first_name = objUserAccount.ListLoadUserDetails[0].user_fst_name;
            objUserAccount.last_name = objUserAccount.ListLoadUserDetails[0].user_lst_name;
            objUserAccount.usr_type = objUserAccount.ListLoadUserDetails[0].user_type;
            objCompany = ServiceObjectCompany.LoadUserType(objCompany);
            objUserAccount.ListUserType = objCompany.ListUserType;
            objUserAccount.password = objUserAccount.ListLoadUserDetails[0].passwd;
            objUserAccount.cfm_password = objUserAccount.ListLoadUserDetails[0].passwd;
            objUserAccount.email = objUserAccount.ListLoadUserDetails[0].email;
            objUserAccount.tel = objUserAccount.ListLoadUserDetails[0].tel;
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
            objUserAccount = ServiceObject.GetGridPermissionDetails(objUserAccount);
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
            objUserAccount = ServiceObject.GetUserCmpDtls(objUserAccount);
            for (int j = 0; j < objUserAccount.LstCmpPermDtls.Count(); j++)
            {
                objUserAccount.cmp_id = objUserAccount.LstCmpPermDtls[j].cmp_id;
                l_str_cmp_id = objUserAccount.cmp_id;               
                for (int i = 0; i < objUserAccount.ListGetUserCmpDetails.Count(); i++)
                {
                    if (objUserAccount.ListGetUserCmpDetails[i].cmp_id.Trim() == l_str_cmp_id.Trim())
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
            Session["GridModCompPerm"] = objUserAccount.LstModCmpPermDtls;
            dtUserAcctPerm = new DataTable();
            List<UserAccountDtl> UserPermLi = new List<UserAccountDtl>();
            //2: Initialize a object of type DataRow
            DataRow drScnPerm;
            //3: Initialize enough objects of type DataColumns
            DataColumn colScnNo = new DataColumn("scn_no", typeof(string));
            DataColumn colScnId = new DataColumn("scn_id", typeof(string));
            DataColumn colStatus = new DataColumn("status", typeof(string));
            DataColumn colScnName = new DataColumn("scn_name", typeof(string));
            DataColumn colScnCaption = new DataColumn("scn_caption", typeof(string));
            DataColumn colScnView = new DataColumn("scn_view", typeof(string));
            DataColumn colScnAdd = new DataColumn("scn_add", typeof(string));
            DataColumn colScnMod = new DataColumn("scn_mod", typeof(string));
            DataColumn colScnDel = new DataColumn("scn_del", typeof(string));
            DataColumn colScnPost = new DataColumn("scn_post", typeof(string));
            DataColumn colCheck = new DataColumn("colCheck", typeof(string));
            int lineCount = 0;
            dtUserAcctPerm.Columns.Add(colScnNo);
            dtUserAcctPerm.Columns.Add(colScnId);
            dtUserAcctPerm.Columns.Add(colStatus);
            dtUserAcctPerm.Columns.Add(colScnName);
            dtUserAcctPerm.Columns.Add(colScnCaption);
            dtUserAcctPerm.Columns.Add(colScnView);
            dtUserAcctPerm.Columns.Add(colScnAdd);
            dtUserAcctPerm.Columns.Add(colScnMod);
            dtUserAcctPerm.Columns.Add(colScnDel);
            dtUserAcctPerm.Columns.Add(colScnPost);
            dtUserAcctPerm.Columns.Add(colCheck);
            objUserAccount = ServiceObject.GetGridScnNameDetails(objUserAccount);
            for (int j = 0; j < objUserAccount.ListScnDtls.Count(); j++)
            {
                objUserAccount.scn_caption = objUserAccount.ListScnDtls[j].scn_caption;
                objUserAccount.scn_id = objUserAccount.ListScnDtls[j].scn_id;
                objUserAccount.scn_view = objUserAccount.ListScnDtls[j].scn_view;
                objUserAccount.scn_add = objUserAccount.ListScnDtls[j].scn_add;
                objUserAccount.scn_mod = objUserAccount.ListScnDtls[j].scn_mod;
                objUserAccount.scn_del = objUserAccount.ListScnDtls[j].scn_del;
                objUserAccount.scn_post = objUserAccount.ListScnDtls[j].scn_post;
                LineNum = LineNum + 1;

                drScnPerm = dtUserAcctPerm.NewRow();
                dtUserAcctPerm.Rows.Add(drScnPerm);
                dtUserAcctPerm.Rows[lineCount][colScnCaption] = objUserAccount.scn_caption;
                dtUserAcctPerm.Rows[lineCount][colScnId] = objUserAccount.scn_view;
                dtUserAcctPerm.Rows[lineCount][colScnView] = objUserAccount.scn_id;
                dtUserAcctPerm.Rows[lineCount][colScnAdd] = objUserAccount.scn_add;
                dtUserAcctPerm.Rows[lineCount][colScnMod] = objUserAccount.scn_mod;
                dtUserAcctPerm.Rows[lineCount][colScnDel] = objUserAccount.scn_del;
                dtUserAcctPerm.Rows[lineCount][colScnPost] = objUserAccount.scn_post;
                dtUserAcctPerm.Rows[lineCount][colCheck] = "false";
                UserAccount objUserAccountPermDtl = new UserAccount();
                objUserAccountPermDtl.LineNum = LineNum;
                objUserAccountPermDtl.scn_caption = objUserAccount.scn_caption;
                objUserAccountPermDtl.scn_id = objUserAccount.scn_id;
                objUserAccountPermDtl.scn_view = objUserAccount.scn_view;
                objUserAccountPermDtl.scn_add = objUserAccount.scn_add;
                objUserAccountPermDtl.scn_mod = objUserAccount.scn_mod;
                objUserAccountPermDtl.scn_del = objUserAccount.scn_del;
                objUserAccountPermDtl.scn_post = objUserAccount.scn_post;
                objUserAccountPermDtl.colCheck = "false";
                UserPermLi.Add(objUserAccountPermDtl);
                lineCount++;

            }
            objUserAccount.LstScnPermDtls = UserPermLi;
            Session["GridModScnPerm"] = objUserAccount.LstScnPermDtls;
            //objUserAccount = ServiceObject.GetGridScnNameDetails(objUserAccount);
            objUserAccount = ServiceObject.GetGridScnPermDetails(objUserAccount);
            List<UserAccountDtl> ScnPermLi = new List<UserAccountDtl>();
            ScnPermLi = Session["GridModScnPerm"] as List<UserAccountDtl>;
            if (objUserAccount.ListScnDtls.Count() > 0)
            {
                for(int j = 0; j < objUserAccount.ListScnPermDtls.Count(); j++)
                {
                    objUserAccount.scn_id = objUserAccount.ListScnPermDtls[j].scn_id;
                    l_str_scn_id = objUserAccount.scn_id;
                    objUserAccount.pm_view = objUserAccount.ListScnPermDtls[j].pm_view;
                    l_str_view = objUserAccount.pm_view;
                    objUserAccount.pm_add = objUserAccount.ListScnPermDtls[j].pm_add;
                    l_str_add = objUserAccount.pm_add;
                    objUserAccount.pm_mod = objUserAccount.ListScnPermDtls[j].pm_mod;
                    l_str_mod = objUserAccount.pm_mod;
                    objUserAccount.pm_post = objUserAccount.ListScnPermDtls[j].pm_post;
                    l_str_post = objUserAccount.pm_post;
                    objUserAccount.pm_del = objUserAccount.ListScnPermDtls[j].pm_del;
                    l_str_del = objUserAccount.pm_del;
                    
                   
                    for (int i = 0; i < objUserAccount.LstScnPermDtls.Count(); i++)
                    {
                        if (objUserAccount.LstScnPermDtls[i].scn_id.Trim() == l_str_scn_id.Trim())
                        {
                            if(l_str_view == null || l_str_view == "1")
                            {
                                for (int k = 0; k < ScnPermLi.Count(); k++)
                                {

                                    ScnPermLi.Where(p => p.scn_id == l_str_scn_id).Select(u =>
                                    {
                                        u.scn_view = "true";
                                        return u;
                                    }).ToList();
                                }                               
                            }
                            else
                            {
                                for (int k = 0; k < ScnPermLi.Count(); k++)
                                {

                                    ScnPermLi.Where(p => p.scn_id == l_str_scn_id).Select(u =>
                                    {
                                        u.scn_view = "false";
                                        return u;
                                    }).ToList();
                                }
                            }
                            if (l_str_add == null || l_str_add == "1")
                            {
                                for (int k = 0; k < ScnPermLi.Count(); k++)
                                {

                                    ScnPermLi.Where(p => p.scn_id == l_str_scn_id).Select(u =>
                                    {
                                        u.scn_add = "true";
                                        return u;
                                    }).ToList();
                                }
                              
                            }
                            else
                            {
                                for (int k = 0; k < ScnPermLi.Count(); k++)
                                {

                                    ScnPermLi.Where(p => p.scn_id == l_str_scn_id).Select(u =>
                                    {
                                        u.scn_add = "false";
                                        return u;
                                    }).ToList();
                                }
                            }
                            if (l_str_mod == null || l_str_mod == "1")
                            {
                                for (int k = 0; k < ScnPermLi.Count(); k++)
                                {

                                    ScnPermLi.Where(p => p.scn_id == l_str_scn_id).Select(u =>
                                    {
                                        u.scn_mod = "true";
                                        return u;
                                    }).ToList();
                                }
                               
                            }
                            else
                            {
                                for (int k = 0; k < ScnPermLi.Count(); k++)
                                {

                                    ScnPermLi.Where(p => p.scn_id == l_str_scn_id).Select(u =>
                                    {
                                        u.scn_mod = "false";
                                        return u;
                                    }).ToList();
                                }
                            }
                            if (l_str_post == null || l_str_post == "1")
                            {
                                for (int k = 0; k < ScnPermLi.Count(); k++)
                                {

                                    ScnPermLi.Where(p => p.scn_id == l_str_scn_id).Select(u =>
                                    {
                                        u.scn_post = "true";
                                        return u;
                                    }).ToList();
                                }
                                
                            }
                            else
                            {
                                for (int k = 0; k < ScnPermLi.Count(); k++)
                                {

                                    ScnPermLi.Where(p => p.scn_id == l_str_scn_id).Select(u =>
                                    {
                                        u.scn_post = "false";
                                        return u;
                                    }).ToList();
                                }
                            }
                            if (l_str_del == null || l_str_del == "1")
                            {
                                for (int k = 0; k < ScnPermLi.Count(); k++)
                                {

                                    ScnPermLi.Where(p => p.scn_id == l_str_scn_id).Select(u =>
                                    {
                                        u.scn_del = "true";
                                        return u;
                                    }).ToList();
                                }
                            }
                            else
                            {
                                for (int k = 0; k < ScnPermLi.Count(); k++)
                                {

                                    ScnPermLi.Where(p => p.scn_id == l_str_scn_id).Select(u =>
                                    {
                                        u.scn_del = "false";
                                        return u;
                                    }).ToList();
                                }
                            }
                        }
                        
                    }
                    continue;
                }
                

            }
            objUserAccount.LstModScnPermDtls = ScnPermLi;
            Session["GridModScreenPerm"] = objUserAccount.LstModScnPermDtls;
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

        public ActionResult SaveUserdtls(string p_str_user_id, string p_str_fst_name, string p_str_lst_name, string p_str_pwd, string p_str_cfm_pwd, string p_str_user_type, string p_str_email,
          string p_str_tel, string p_str_screen_name, string p_str_view, string p_str_add,
           string p_str_mod, string p_str_del, string p_str_post,string p_str_dflt_cmpid, string p_str_addd, string p_str_modd, string p_str_dele, string p_str_vieww, string p_str_postt)
        {
            UserAccount objUserAccount = new UserAccount();
            IUserAccountService ServiceObject = new UserAccountService();
            objUserAccount.user_id = p_str_user_id.Trim();
            objUserAccount.first_name = p_str_fst_name.Trim();
            objUserAccount.last_name = p_str_lst_name.Trim();
            objUserAccount = ServiceObject.CheckExistUserId(objUserAccount);
            if(objUserAccount.ListCheckUserIdExist.Count== 0)
            {
                objUserAccount = ServiceObject.GetGridScnNameDetails(objUserAccount);
                objUserAccount.ListScnDtls = objUserAccount.ListScnDtls;
                foreach (var ID in p_str_addd.Split(','))
                {
                    objUserAccount.ListScnDtls.Where(p => p.scn_id == ID).Select(u =>
                    {
                        u.Result = "1";
                        return u;
                    }).ToList();
                }
                objUserAccount.ListScnDtls = objUserAccount.ListScnDtls;
                for (int i = 0; i < objUserAccount.ListScnDtls.Count(); i++)
                {
                    objUserAccount.colChk = "add";
                    if (objUserAccount.ListScnDtls[i].Result == "1")
                    {
                        objUserAccount.scn_id = objUserAccount.ListScnDtls[i].scn_id;
                        objUserAccount.pm_add = "1";
                        ServiceObject.SaveUserPerm(objUserAccount);
                    }
                    else
                    {
                        objUserAccount.scn_id = objUserAccount.ListScnDtls[i].scn_id;
                        objUserAccount.pm_add = "0";
                        ServiceObject.SaveUserPerm(objUserAccount);
                    }
                }
                objUserAccount = ServiceObject.GetGridScnNameDetails(objUserAccount);
                objUserAccount.ListScnDtls = objUserAccount.ListScnDtls;
                foreach (var ID in p_str_modd.Split(','))
                {
                    objUserAccount.ListScnDtls.Where(p => p.scn_id == ID).Select(u =>
                    {
                        u.Result = "1";
                        return u;
                    }).ToList();
                }
                objUserAccount.ListScnDtls = objUserAccount.ListScnDtls;
                for (int i = 0; i < objUserAccount.ListScnDtls.Count(); i++)
                {
                    objUserAccount.colChk = "mod";
                    if (objUserAccount.ListScnDtls[i].Result == "1")
                    {
                        objUserAccount.scn_id = objUserAccount.ListScnDtls[i].scn_id;
                        objUserAccount.pm_mod = "1";
                        ServiceObject.SaveUserPerm(objUserAccount);
                    }
                    else
                    {
                        objUserAccount.scn_id = objUserAccount.ListScnDtls[i].scn_id;
                        objUserAccount.pm_mod = "0";
                        ServiceObject.SaveUserPerm(objUserAccount);
                    }
                }
                objUserAccount = ServiceObject.GetGridScnNameDetails(objUserAccount);
                objUserAccount.ListScnDtls = objUserAccount.ListScnDtls;
                foreach (var ID in p_str_dele.Split(','))
                {
                    objUserAccount.ListScnDtls.Where(p => p.scn_id == ID).Select(u =>
                    {
                        u.Result = "1";
                        return u;
                    }).ToList();
                }
                objUserAccount.ListScnDtls = objUserAccount.ListScnDtls;
                for (int i = 0; i < objUserAccount.ListScnDtls.Count(); i++)
                {
                    objUserAccount.colChk = "del";
                    if (objUserAccount.ListScnDtls[i].Result == "1")
                    {
                        objUserAccount.scn_id = objUserAccount.ListScnDtls[i].scn_id;
                        objUserAccount.pm_del = "1";
                        ServiceObject.SaveUserPerm(objUserAccount);
                    }
                    else
                    {
                        objUserAccount.scn_id = objUserAccount.ListScnDtls[i].scn_id;
                        objUserAccount.pm_del = "0";
                        ServiceObject.SaveUserPerm(objUserAccount);
                    }
                }
                objUserAccount = ServiceObject.GetGridScnNameDetails(objUserAccount);
                objUserAccount.ListScnDtls = objUserAccount.ListScnDtls;
                foreach (var ID in p_str_vieww.Split(','))
                {
                    objUserAccount.ListScnDtls.Where(p => p.scn_id == ID).Select(u =>
                    {
                        u.Result = "1";
                        return u;
                    }).ToList();
                }
                objUserAccount.ListScnDtls = objUserAccount.ListScnDtls;
                for (int i = 0; i < objUserAccount.ListScnDtls.Count(); i++)
                {
                    objUserAccount.colChk = "view";
                    if (objUserAccount.ListScnDtls[i].Result == "1")
                    {
                        objUserAccount.scn_id = objUserAccount.ListScnDtls[i].scn_id;
                        objUserAccount.pm_view = "1";
                        ServiceObject.SaveUserPerm(objUserAccount);
                    }
                    else
                    {
                        objUserAccount.scn_id = objUserAccount.ListScnDtls[i].scn_id;
                        objUserAccount.pm_view = "0";
                        ServiceObject.SaveUserPerm(objUserAccount);
                    }
                }
                objUserAccount = ServiceObject.GetGridScnNameDetails(objUserAccount);
                objUserAccount.ListScnDtls = objUserAccount.ListScnDtls;
                foreach (var ID in p_str_postt.Split(','))
                {
                    objUserAccount.ListScnDtls.Where(p => p.scn_id == ID).Select(u =>
                    {
                        u.Result = "1";
                        return u;
                    }).ToList();
                }
                objUserAccount.ListScnDtls = objUserAccount.ListScnDtls;
                for (int i = 0; i < objUserAccount.ListScnDtls.Count(); i++)
                {
                    objUserAccount.colChk = "post";
                    if (objUserAccount.ListScnDtls[i].Result == "1")
                    {
                        objUserAccount.scn_id = objUserAccount.ListScnDtls[i].scn_id;
                        objUserAccount.pm_post = "1";
                        ServiceObject.SaveUserPerm(objUserAccount);
                    }
                    else
                    {
                        objUserAccount.scn_id = objUserAccount.ListScnDtls[i].scn_id;
                        objUserAccount.pm_post = "0";
                        ServiceObject.SaveUserPerm(objUserAccount);
                    }
                }
                if (p_str_lst_name == string.Empty)
                {
                    objUserAccount.last_name = "-";
                }
                else
                {
                    objUserAccount.last_name = p_str_lst_name.Trim();
                }
                objUserAccount.password = p_str_pwd.Trim();
                objUserAccount.cfm_password = p_str_cfm_pwd.Trim();
                objUserAccount.email = p_str_email.Trim();
                if (p_str_email == string.Empty)
                {
                    objUserAccount.email = "-";
                }
                else
                {
                    objUserAccount.email = p_str_email.Trim();
                }
                objUserAccount.tel = p_str_tel.Trim();
                if (p_str_tel == string.Empty)
                {
                    objUserAccount.tel = "-";
                }
                else
                {
                    objUserAccount.tel = p_str_tel.Trim();
                }
                objUserAccount.usr_type = p_str_user_type.Trim();
                if (p_str_user_type == string.Empty)
                {
                    objUserAccount.usr_type = "-";
                }
                else
                {
                    objUserAccount.usr_type = p_str_user_type.Trim();
                }
                objUserAccount.cmp_perm1 = System.Configuration.ConfigurationManager.AppSettings["DefaultCompID"].ToString().Trim();
                objUserAccount.dflt_cust_id = p_str_dflt_cmpid.Trim();
                ServiceObject.SaveUserDtls(objUserAccount);

                objUserAccount.LstCmpPerm = Session["GridUpdCmpPerm"] as List<UserAccountDtl>;
                for (int i = 0; i < objUserAccount.LstCmpPerm.Count; i++)
                {
                    if (objUserAccount.LstCmpPerm[i].colChk == "true")
                    {
                        objUserAccount.user_id = p_str_user_id.Trim();
                        objUserAccount.first_name = p_str_fst_name.Trim();
                        objUserAccount.cmp_id = objUserAccount.LstCmpPerm[i].cmp_id.Trim();
                        objUserAccount.cmp_name = objUserAccount.LstCmpPerm[i].cmp_name.Trim();
                        ServiceObject.SaveCmpPerm(objUserAccount);
                    }
                }
                Mapper.CreateMap<UserAccount, UserAccountModel>();
                UserAccountModel objUserAccountModel = Mapper.Map<UserAccount, UserAccountModel>(objUserAccount);
                return View("~/Views/UserAccount/UserAccount.cshtml", objUserAccountModel);
            }
            else
            {
                return Json(objUserAccount.ListCheckUserIdExist.Count, JsonRequestBehavior.AllowGet);
            }
            
        }
        public ActionResult UpdateUserdtls(string p_str_user_id, string p_str_fst_name, string p_str_lst_name, string p_str_pwd, string p_str_cfm_pwd, string p_str_user_type, string p_str_email,
          string p_str_tel, string p_str_screen_name, string p_str_view, string p_str_add,
           string p_str_mod, string p_str_del, string p_str_post, string p_str_dflt_cmpid, string p_str_addd, string p_str_modd, string p_str_dele, string p_str_vieww, string p_str_postt)
        {
            UserAccount objUserAccount = new UserAccount();
            IUserAccountService ServiceObject = new UserAccountService();
            objUserAccount.user_id = p_str_user_id.Trim();
            objUserAccount.first_name = p_str_fst_name.Trim();
            objUserAccount.last_name = p_str_lst_name.Trim();
            //ServiceObject.DeleteScreenPermDtls(objUserAccount);
            objUserAccount = ServiceObject.GetGridScnNameDetails(objUserAccount);
            objUserAccount.ListScnDtls = objUserAccount.ListScnDtls;
            foreach (var ID in p_str_addd.Split(','))
            {
                objUserAccount.ListScnDtls.Where(p => p.scn_id == ID).Select(u =>
                {
                    u.Result = "1";
                    return u;
                }).ToList();
            }
            objUserAccount.ListScnDtls = objUserAccount.ListScnDtls;
            for (int i = 0; i < objUserAccount.ListScnDtls.Count(); i++)
            {
                objUserAccount.colChk = "add";
                if (objUserAccount.ListScnDtls[i].Result == "1")
                {
                    objUserAccount.scn_id = objUserAccount.ListScnDtls[i].scn_id;
                    objUserAccount.pm_add = "1";
                    ServiceObject.SaveUserPerm(objUserAccount);
                }
                else
                {
                    objUserAccount.scn_id = objUserAccount.ListScnDtls[i].scn_id;
                    objUserAccount.pm_add = "0";
                    ServiceObject.SaveUserPerm(objUserAccount);
                }
            }
            objUserAccount = ServiceObject.GetGridScnNameDetails(objUserAccount);
            objUserAccount.ListScnDtls = objUserAccount.ListScnDtls;
            foreach (var ID in p_str_modd.Split(','))
            {
                objUserAccount.ListScnDtls.Where(p => p.scn_id == ID).Select(u =>
                {
                    u.Result = "1";
                    return u;
                }).ToList();
            }
            objUserAccount.ListScnDtls = objUserAccount.ListScnDtls;
            for (int i = 0; i < objUserAccount.ListScnDtls.Count(); i++)
            {
                objUserAccount.colChk = "mod";
                if (objUserAccount.ListScnDtls[i].Result == "1")
                {
                    objUserAccount.scn_id = objUserAccount.ListScnDtls[i].scn_id;
                    objUserAccount.pm_mod = "1";
                    ServiceObject.SaveUserPerm(objUserAccount);
                }
                else
                {
                    objUserAccount.scn_id = objUserAccount.ListScnDtls[i].scn_id;
                    objUserAccount.pm_mod = "0";
                    ServiceObject.SaveUserPerm(objUserAccount);
                }
            }
            objUserAccount = ServiceObject.GetGridScnNameDetails(objUserAccount);
            objUserAccount.ListScnDtls = objUserAccount.ListScnDtls;
            foreach (var ID in p_str_dele.Split(','))
            {
                objUserAccount.ListScnDtls.Where(p => p.scn_id == ID).Select(u =>
                {
                    u.Result = "1";
                    return u;
                }).ToList();
            }
            objUserAccount.ListScnDtls = objUserAccount.ListScnDtls;
            for (int i = 0; i < objUserAccount.ListScnDtls.Count(); i++)
            {
                objUserAccount.colChk = "del";
                if (objUserAccount.ListScnDtls[i].Result == "1")
                {
                    objUserAccount.scn_id = objUserAccount.ListScnDtls[i].scn_id;
                    objUserAccount.pm_del = "1";
                    ServiceObject.SaveUserPerm(objUserAccount);
                }
                else
                {
                    objUserAccount.scn_id = objUserAccount.ListScnDtls[i].scn_id;
                    objUserAccount.pm_del = "0";
                    ServiceObject.SaveUserPerm(objUserAccount);
                }
            }
            objUserAccount = ServiceObject.GetGridScnNameDetails(objUserAccount);
            objUserAccount.ListScnDtls = objUserAccount.ListScnDtls;
            foreach (var ID in p_str_vieww.Split(','))
            {
                objUserAccount.ListScnDtls.Where(p => p.scn_id == ID).Select(u =>
                {
                    u.Result = "1";
                    return u;
                }).ToList();
            }
            objUserAccount.ListScnDtls = objUserAccount.ListScnDtls;
            for (int i = 0; i < objUserAccount.ListScnDtls.Count(); i++)
            {
                objUserAccount.colChk = "view";
                if (objUserAccount.ListScnDtls[i].Result == "1")
                {
                    objUserAccount.scn_id = objUserAccount.ListScnDtls[i].scn_id;
                    objUserAccount.pm_view = "1";
                    ServiceObject.SaveUserPerm(objUserAccount);
                }
                else
                {
                    objUserAccount.scn_id = objUserAccount.ListScnDtls[i].scn_id;
                    objUserAccount.pm_view = "0";
                    ServiceObject.SaveUserPerm(objUserAccount);
                }
            }
            objUserAccount = ServiceObject.GetGridScnNameDetails(objUserAccount);
            objUserAccount.ListScnDtls = objUserAccount.ListScnDtls;
            foreach (var ID in p_str_postt.Split(','))
            {
                objUserAccount.ListScnDtls.Where(p => p.scn_id == ID).Select(u =>
                {
                    u.Result = "1";
                    return u;
                }).ToList();
            }
            objUserAccount.ListScnDtls = objUserAccount.ListScnDtls;
            for (int i = 0; i < objUserAccount.ListScnDtls.Count(); i++)
            {
                objUserAccount.colChk = "post";
                if (objUserAccount.ListScnDtls[i].Result == "1")
                {
                    objUserAccount.scn_id = objUserAccount.ListScnDtls[i].scn_id;
                    objUserAccount.pm_post = "1";
                    ServiceObject.SaveUserPerm(objUserAccount);
                }
                else
                {
                    objUserAccount.scn_id = objUserAccount.ListScnDtls[i].scn_id;
                    objUserAccount.pm_post = "0";
                    ServiceObject.SaveUserPerm(objUserAccount);
                }
            }
            if (p_str_lst_name == string.Empty)
            {
                objUserAccount.last_name = "-";
            }
            else
            {
                objUserAccount.last_name = p_str_lst_name.Trim();
            }
            objUserAccount.password = p_str_pwd.Trim();
            objUserAccount.cfm_password = p_str_cfm_pwd.Trim();
            objUserAccount.email = p_str_email.Trim();
            if (p_str_email == string.Empty)
            {
                objUserAccount.email = "-";
            }
            else
            {
                objUserAccount.email = p_str_email.Trim();
            }
            objUserAccount.tel = p_str_tel.Trim();
            if (p_str_tel == string.Empty)
            {
                objUserAccount.tel = "-";
            }
            else
            {
                objUserAccount.tel = p_str_tel.Trim();
            }
            objUserAccount.usr_type = p_str_user_type.Trim();
            if (p_str_user_type == string.Empty)
            {
                objUserAccount.usr_type = "-";
            }
            else
            {
                objUserAccount.usr_type = p_str_user_type.Trim();
            }
            objUserAccount.cmp_perm1 = System.Configuration.ConfigurationManager.AppSettings["DefaultCompID"].ToString().Trim();
            objUserAccount.dflt_cust_id = p_str_dflt_cmpid.Trim();
            ServiceObject.UpdateUserDtls(objUserAccount);
            //for (int i = 0; i < objUserAccount.ListScnDtls.Count; i++)
            //{
            //    if (objUserAccount.ListScnDtls[0].scn_view == "True" || objUserAccount.ListScnDtls[0].scn_add == "True" || objUserAccount.ListScnDtls[0].scn_mod == "True" || objUserAccount.ListScnDtls[0].scn_del == "True" || objUserAccount.ListScnDtls[0].scn_post == "True")
            //    {
            //        objUserAccount.user_id = p_str_user_id.Trim();
            //        objUserAccount.scn_id = objUserAccount.ListScnDtls[0].scn_id.Trim();
            //        if (objUserAccount.ListScnDtls[0].scn_view == "True")
            //        {
            //            objUserAccount.pm_view = "1";
            //        }
            //        else
            //        {
            //            objUserAccount.pm_view = "0";
            //        }
            //        if (objUserAccount.ListScnDtls[0].scn_add == "True")
            //        {
            //            objUserAccount.pm_add = "1";
            //        }
            //        else
            //        {
            //            objUserAccount.pm_add = "0";
            //        }
            //        if (objUserAccount.ListScnDtls[0].scn_mod == "True")
            //        {
            //            objUserAccount.pm_mod = "1";
            //        }
            //        else
            //        {
            //            objUserAccount.pm_mod = "0";
            //        }
            //        if (objUserAccount.ListScnDtls[0].scn_del == "True")
            //        {
            //            objUserAccount.pm_del = "1";
            //        }
            //        else
            //        {
            //            objUserAccount.pm_del = "0";
            //        }
            //        if (objUserAccount.ListScnDtls[0].scn_post == "True")
            //        {
            //            objUserAccount.pm_post = "1";
            //        }
            //        else
            //        {
            //            objUserAccount.pm_post = "0";
            //        }
            //    }
            //    ServiceObject.SaveUserPerm(objUserAccount);
            //}
           
            ServiceObject.DeleteCmpPerm(objUserAccount);
            objUserAccount.LstCmpPerm = Session["GridUpdCmpPerm"] as List<UserAccountDtl>;
            for (int i = 0; i < objUserAccount.LstCmpPerm.Count; i++)
            {
                if (objUserAccount.LstCmpPerm[i].colChk == "true")
                {
                    objUserAccount.user_id = p_str_user_id.Trim();
                    objUserAccount.first_name = p_str_fst_name.Trim();
                    objUserAccount.cmp_id = objUserAccount.LstCmpPerm[i].cmp_id.Trim();
                    objUserAccount.cmp_name = objUserAccount.LstCmpPerm[i].cmp_name.Trim();
                    ServiceObject.SaveCmpPerm(objUserAccount);
                }
            }
            Mapper.CreateMap<UserAccount, UserAccountModel>();
            UserAccountModel objUserAccountModel = Mapper.Map<UserAccount, UserAccountModel>(objUserAccount);
            return View("~/Views/UserAccount/UserAccount.cshtml", objUserAccountModel);
        }
        public ActionResult Del(string p_str_user_id)
        {
            int LineNum = 0;
            string l_str_scn_id = string.Empty;
            string l_str_view = string.Empty;
            string l_str_add = string.Empty;
            string l_str_mod = string.Empty;
            string l_str_post = string.Empty;
            string l_str_del = string.Empty;
            string l_str_cmp_id = string.Empty;           
            UserAccount objUserAccount = new UserAccount();
            IUserAccountService ServiceObject = new UserAccountService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objUserAccount.user_id = p_str_user_id.Trim();
            //objUserAccount = ServiceObject.LoadUserType(objUserAccount);
            objUserAccount = ServiceObject.LoadUserDtls(objUserAccount);
            objUserAccount.first_name = objUserAccount.ListLoadUserDetails[0].user_fst_name;
            objUserAccount.last_name = objUserAccount.ListLoadUserDetails[0].user_lst_name;
            objUserAccount.usr_type = objUserAccount.ListLoadUserDetails[0].user_type;
            objCompany = ServiceObjectCompany.LoadUserType(objCompany);
            objUserAccount.ListUserType = objCompany.ListUserType;
            objUserAccount.password = objUserAccount.ListLoadUserDetails[0].passwd;
            objUserAccount.cfm_password = objUserAccount.ListLoadUserDetails[0].passwd;
            objUserAccount.email = objUserAccount.ListLoadUserDetails[0].email;
            objUserAccount.tel = objUserAccount.ListLoadUserDetails[0].tel;
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
            objUserAccount = ServiceObject.GetGridPermissionDetails(objUserAccount);
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
            objUserAccount = ServiceObject.GetUserCmpDtls(objUserAccount);
            for (int j = 0; j < objUserAccount.LstCmpPermDtls.Count(); j++)
            {
                objUserAccount.cmp_id = objUserAccount.LstCmpPermDtls[j].cmp_id;
                l_str_cmp_id = objUserAccount.cmp_id;
                for (int i = 0; i < objUserAccount.ListGetUserCmpDetails.Count(); i++)
                {
                    if (objUserAccount.ListGetUserCmpDetails[i].cmp_id.Trim() == l_str_cmp_id.Trim())
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
            Session["GridModCompPerm"] = objUserAccount.LstModCmpPermDtls;
            dtUserAcctPerm = new DataTable();
            List<UserAccountDtl> UserPermLi = new List<UserAccountDtl>();
            //2: Initialize a object of type DataRow
            DataRow drScnPerm;
            //3: Initialize enough objects of type DataColumns
            DataColumn colScnNo = new DataColumn("scn_no", typeof(string));
            DataColumn colScnId = new DataColumn("scn_id", typeof(string));
            DataColumn colStatus = new DataColumn("status", typeof(string));
            DataColumn colScnName = new DataColumn("scn_name", typeof(string));
            DataColumn colScnCaption = new DataColumn("scn_caption", typeof(string));
            DataColumn colScnView = new DataColumn("scn_view", typeof(string));
            DataColumn colScnAdd = new DataColumn("scn_add", typeof(string));
            DataColumn colScnMod = new DataColumn("scn_mod", typeof(string));
            DataColumn colScnDel = new DataColumn("scn_del", typeof(string));
            DataColumn colScnPost = new DataColumn("scn_post", typeof(string));
            DataColumn colCheck = new DataColumn("colCheck", typeof(string));
            int lineCount = 0;
            dtUserAcctPerm.Columns.Add(colScnNo);
            dtUserAcctPerm.Columns.Add(colScnId);
            dtUserAcctPerm.Columns.Add(colStatus);
            dtUserAcctPerm.Columns.Add(colScnName);
            dtUserAcctPerm.Columns.Add(colScnCaption);
            dtUserAcctPerm.Columns.Add(colScnView);
            dtUserAcctPerm.Columns.Add(colScnAdd);
            dtUserAcctPerm.Columns.Add(colScnMod);
            dtUserAcctPerm.Columns.Add(colScnDel);
            dtUserAcctPerm.Columns.Add(colScnPost);
            dtUserAcctPerm.Columns.Add(colCheck);
            objUserAccount = ServiceObject.GetGridScnNameDetails(objUserAccount);
            for (int j = 0; j < objUserAccount.ListScnDtls.Count(); j++)
            {
                objUserAccount.scn_caption = objUserAccount.ListScnDtls[j].scn_caption;
                objUserAccount.scn_id = objUserAccount.ListScnDtls[j].scn_id;
                objUserAccount.scn_view = objUserAccount.ListScnDtls[j].scn_view;
                objUserAccount.scn_add = objUserAccount.ListScnDtls[j].scn_add;
                objUserAccount.scn_mod = objUserAccount.ListScnDtls[j].scn_mod;
                objUserAccount.scn_del = objUserAccount.ListScnDtls[j].scn_del;
                objUserAccount.scn_post = objUserAccount.ListScnDtls[j].scn_post;
                LineNum = LineNum + 1;

                drScnPerm = dtUserAcctPerm.NewRow();
                dtUserAcctPerm.Rows.Add(drScnPerm);
                dtUserAcctPerm.Rows[lineCount][colScnCaption] = objUserAccount.scn_caption;
                dtUserAcctPerm.Rows[lineCount][colScnId] = objUserAccount.scn_view;
                dtUserAcctPerm.Rows[lineCount][colScnView] = objUserAccount.scn_id;
                dtUserAcctPerm.Rows[lineCount][colScnAdd] = objUserAccount.scn_add;
                dtUserAcctPerm.Rows[lineCount][colScnMod] = objUserAccount.scn_mod;
                dtUserAcctPerm.Rows[lineCount][colScnDel] = objUserAccount.scn_del;
                dtUserAcctPerm.Rows[lineCount][colScnPost] = objUserAccount.scn_post;
                dtUserAcctPerm.Rows[lineCount][colCheck] = "false";
                UserAccount objUserAccountPermDtl = new UserAccount();
                objUserAccountPermDtl.LineNum = LineNum;
                objUserAccountPermDtl.scn_caption = objUserAccount.scn_caption;
                objUserAccountPermDtl.scn_id = objUserAccount.scn_id;
                objUserAccountPermDtl.scn_view = objUserAccount.scn_view;
                objUserAccountPermDtl.scn_add = objUserAccount.scn_add;
                objUserAccountPermDtl.scn_mod = objUserAccount.scn_mod;
                objUserAccountPermDtl.scn_del = objUserAccount.scn_del;
                objUserAccountPermDtl.scn_post = objUserAccount.scn_post;
                objUserAccountPermDtl.colCheck = "false";
                UserPermLi.Add(objUserAccountPermDtl);
                lineCount++;

            }
            objUserAccount.LstScnPermDtls = UserPermLi;
            Session["GridModScnPerm"] = objUserAccount.LstScnPermDtls;
            //objUserAccount = ServiceObject.GetGridScnNameDetails(objUserAccount);
            objUserAccount = ServiceObject.GetGridScnPermDetails(objUserAccount);
            List<UserAccountDtl> ScnPermLi = new List<UserAccountDtl>();
            ScnPermLi = Session["GridModScnPerm"] as List<UserAccountDtl>;
            if (objUserAccount.ListScnDtls.Count() > 0)
            {
                for (int j = 0; j < objUserAccount.ListScnPermDtls.Count(); j++)
                {
                    objUserAccount.scn_id = objUserAccount.ListScnPermDtls[j].scn_id;
                    l_str_scn_id = objUserAccount.scn_id;
                    objUserAccount.pm_view = objUserAccount.ListScnPermDtls[j].pm_view;
                    l_str_view = objUserAccount.pm_view;
                    objUserAccount.pm_add = objUserAccount.ListScnPermDtls[j].pm_add;
                    l_str_add = objUserAccount.pm_add;
                    objUserAccount.pm_mod = objUserAccount.ListScnPermDtls[j].pm_mod;
                    l_str_mod = objUserAccount.pm_mod;
                    objUserAccount.pm_post = objUserAccount.ListScnPermDtls[j].pm_post;
                    l_str_post = objUserAccount.pm_post;
                    objUserAccount.pm_del = objUserAccount.ListScnPermDtls[j].pm_del;
                    l_str_del = objUserAccount.pm_del;


                    for (int i = 0; i < objUserAccount.LstScnPermDtls.Count(); i++)
                    {
                        if (objUserAccount.LstScnPermDtls[i].scn_id.Trim() == l_str_scn_id.Trim())
                        {
                            if (l_str_view == null || l_str_view == "1")
                            {
                                for (int k = 0; k < ScnPermLi.Count(); k++)
                                {

                                    ScnPermLi.Where(p => p.scn_id == l_str_scn_id).Select(u =>
                                    {
                                        u.scn_view = "true";
                                        return u;
                                    }).ToList();
                                }
                            }
                            else
                            {
                                for (int k = 0; k < ScnPermLi.Count(); k++)
                                {

                                    ScnPermLi.Where(p => p.scn_id == l_str_scn_id).Select(u =>
                                    {
                                        u.scn_view = "false";
                                        return u;
                                    }).ToList();
                                }
                            }
                            if (l_str_add == null || l_str_add == "1")
                            {
                                for (int k = 0; k < ScnPermLi.Count(); k++)
                                {

                                    ScnPermLi.Where(p => p.scn_id == l_str_scn_id).Select(u =>
                                    {
                                        u.scn_add = "true";
                                        return u;
                                    }).ToList();
                                }

                            }
                            else
                            {
                                for (int k = 0; k < ScnPermLi.Count(); k++)
                                {

                                    ScnPermLi.Where(p => p.scn_id == l_str_scn_id).Select(u =>
                                    {
                                        u.scn_add = "false";
                                        return u;
                                    }).ToList();
                                }
                            }
                            if (l_str_mod == null || l_str_mod == "1")
                            {
                                for (int k = 0; k < ScnPermLi.Count(); k++)
                                {

                                    ScnPermLi.Where(p => p.scn_id == l_str_scn_id).Select(u =>
                                    {
                                        u.scn_mod = "true";
                                        return u;
                                    }).ToList();
                                }

                            }
                            else
                            {
                                for (int k = 0; k < ScnPermLi.Count(); k++)
                                {

                                    ScnPermLi.Where(p => p.scn_id == l_str_scn_id).Select(u =>
                                    {
                                        u.scn_mod = "false";
                                        return u;
                                    }).ToList();
                                }
                            }
                            if (l_str_post == null || l_str_post == "1")
                            {
                                for (int k = 0; k < ScnPermLi.Count(); k++)
                                {

                                    ScnPermLi.Where(p => p.scn_id == l_str_scn_id).Select(u =>
                                    {
                                        u.scn_post = "true";
                                        return u;
                                    }).ToList();
                                }

                            }
                            else
                            {
                                for (int k = 0; k < ScnPermLi.Count(); k++)
                                {

                                    ScnPermLi.Where(p => p.scn_id == l_str_scn_id).Select(u =>
                                    {
                                        u.scn_post = "false";
                                        return u;
                                    }).ToList();
                                }
                            }
                            if (l_str_del == null || l_str_del == "1")
                            {
                                for (int k = 0; k < ScnPermLi.Count(); k++)
                                {

                                    ScnPermLi.Where(p => p.scn_id == l_str_scn_id).Select(u =>
                                    {
                                        u.scn_del = "true";
                                        return u;
                                    }).ToList();
                                }
                            }
                            else
                            {
                                for (int k = 0; k < ScnPermLi.Count(); k++)
                                {

                                    ScnPermLi.Where(p => p.scn_id == l_str_scn_id).Select(u =>
                                    {
                                        u.scn_del = "false";
                                        return u;
                                    }).ToList();
                                }
                            }
                        }

                    }
                    continue;
                }


            }
            objUserAccount.LstModScnPermDtls = ScnPermLi;
            Session["GridModScreenPerm"] = objUserAccount.LstModScnPermDtls;
            objUserAccount = ServiceObject.GetDfltCmpId(objUserAccount);
            objUserAccount.cmp_id = objUserAccount.ListGetDfltCmpId[0].dflt_cust_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objUserAccount.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objUserAccount.View_Flag = "D";
            Mapper.CreateMap<UserAccount, UserAccountModel>();
            UserAccountModel objUserAccountModel = Mapper.Map<UserAccount, UserAccountModel>(objUserAccount);
            return PartialView("_UserAccountNew", objUserAccountModel);
        }
        public ActionResult DeleteUserAccount(string p_str_user_id)
        {
            string l_str_success=string.Empty;
            UserAccount objUserAccount = new UserAccount();
            IUserAccountService ServiceObject = new UserAccountService();
            objUserAccount.user_id = p_str_user_id.Trim();
            if(p_str_user_id != Session["UserID"].ToString().Trim())
            {
                ServiceObject.DeleteUserDtls(objUserAccount);
                ServiceObject.DeleteCmpPermDtls(objUserAccount);
                //ResultCount = "AD";
                //return Json(ResultCount, JsonRequestBehavior.AllowGet);
                Mapper.CreateMap<UserAccount, UserAccountModel>();
                UserAccountModel objUserAccountModel = Mapper.Map<UserAccount, UserAccountModel>(objUserAccount);
                return View("~/Views/UserAccount/UserAccount.cshtml", objUserAccountModel);
            }
            
            else
            {
                l_str_success = "1";
                return Json(l_str_success, JsonRequestBehavior.AllowGet);
            }
        }

        //public ActionResult UserAccountDtl(string p_str_user_id)
        //{
        //    try
        //    {

        //        UserAccount objUserAccount = new UserAccount();
        //        IUserAccountService ServiceObject = new UserAccountService();
        //        string l_str_search_flag = string.Empty;
        //        string l_str_is_another_usr = string.Empty;

        //        //l_str_is_another_usr = Session["isanother"].ToString();
        //        //objRateMaster.IsAnotherUser = l_str_is_another_usr.Trim();
        //        //l_str_search_flag = Session["g_str_Search_flag"].ToString().Trim();
        //        //objUserAccount.is_company_user = Session["IsCompanyUser"].ToString().Trim();
        //        if (l_str_search_flag == "True")
        //        {
        //            //objRateMaster.is_company_user = Session["IsCompanyUser"].ToString().Trim();
        //            objUserAccount.user_id = Session["TEMP_USR_ID"].ToString().Trim();
        //            objUserAccount.first_name = Session["TEMP_FST_NAME"].ToString().Trim();//CR-180421-001 Added By Nithya
        //            objUserAccount.last_name = Session["TEMP_LST_NAME"].ToString().Trim();                   

        //        }
        //        else
        //        {
        //            objUserAccount.user_id = p_str_user_id;
                  
        //        }

        //        objUserAccount = ServiceObject.GetUserDetails(objUserAccount);
        //        Mapper.CreateMap<UserAccount, UserAccountModel>();
        //        UserAccountModel objUserAccountModel = Mapper.Map<UserAccount, UserAccountModel>(objUserAccount);
        //        return PartialView("_UserAccount", objUserAccountModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        public JsonResult USR_ACCT_HDR_DATA(string p_str_user_id)
        {
            UserAccount objUserAccount = new UserAccount();
            IUserAccountService ServiceObject = new UserAccountService();
            Session["TEMP_USR_ID"] = p_str_user_id.Trim();           
            return Json(objUserAccount.MasterCount, JsonRequestBehavior.AllowGet);

        }

    }
}