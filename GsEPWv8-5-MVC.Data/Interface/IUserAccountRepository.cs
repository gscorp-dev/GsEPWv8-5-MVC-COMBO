using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Interface
{
    public interface IUserAccountRepository
    {      
        UserAccount GetUserDetails(UserAccount objUserAccount);
        UserAccount CheckExistUserId(UserAccount objUserAccount);
        UserAccount GetDefaultCompanyDetails(UserAccount objUserAccount);
        UserAccount GetUserPermCompanyDetails(UserAccount objUserAccount);
        UserAccount GetGridScnNameDetails(UserAccount objUserAccount);
        UserAccount GetGridScnPermDetails(UserAccount objUserAccount);
       
        void SaveUserDtls(UserAccount objUserAccount);
        void UpdateUserDtls(UserAccount objUserAccount);        
        void SaveUserPerm(UserAccount objUserAccount);
        UserAccount SaveCmpPerm(UserAccount objUserAccount);
        void DeleteCmpPerm(UserAccount objUserAccount);
        void DeleteUserDtls(UserAccount objUserAccount);
        UserAccount GetDfltCmpId(UserAccount objUserAccount);
        UserAccount GetGridotherScnPermDetails(UserAccount objUserAccount);





    }
}
