using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
    public class UserAccountService : IUserAccountService
    {
        UserAccountRepository objRepository = new UserAccountRepository();
        public UserAccount GetUserDetails(UserAccount objUserAccount)
        {
            return objRepository.GetUserDetails(objUserAccount);
        }
        public UserAccount GetUserPermCompanyDetails(UserAccount objUserAccount)
        {
            return objRepository.GetUserPermCompanyDetails(objUserAccount);
        }
        public UserAccount GetDefaultCompanyDetails(UserAccount objUserAccount)
        {
            return objRepository.GetDefaultCompanyDetails(objUserAccount);
        }
        public UserAccount GetGridScnNameDetails(UserAccount objUserAccount)
        {
            return objRepository.GetGridScnNameDetails(objUserAccount);
        }
        public UserAccount GetGridScnPermDetails(UserAccount objUserAccount)
        {
            return objRepository.GetGridScnPermDetails(objUserAccount);
        }
        public UserAccount CheckExistUserId(UserAccount objUserAccount)
        {
            return objRepository.CheckExistUserId(objUserAccount);
        }
        public void SaveUserDtls(UserAccount objUserAccount)
        {
            objRepository.SaveUserDtls(objUserAccount);
        }
        public void UpdateUserDtls(UserAccount objUserAccount)
        {
            objRepository.UpdateUserDtls(objUserAccount);
        }
        public void SaveUserPerm(UserAccount objUserAccount)
        {
            objRepository.SaveUserPerm(objUserAccount);
        }
        public UserAccount SaveCmpPerm(UserAccount objUserAccount)
        {
            return objRepository.SaveCmpPerm(objUserAccount);
        }
        public void DeleteCmpPerm(UserAccount objUserAccount)
        {
            objRepository.DeleteCmpPerm(objUserAccount);
        }
        public void DeleteUserDtls(UserAccount objUserAccount)
        {
            objRepository.DeleteUserDtls(objUserAccount);
        }
        public UserAccount GetDfltCmpId(UserAccount objUserAccount)
        {
            return objRepository.GetDfltCmpId(objUserAccount);
        }
        public UserAccount GetGridotherScnPermDetails(UserAccount objUserAccount)
        {
            return objRepository.GetGridotherScnPermDetails(objUserAccount);
        }

    }
}
