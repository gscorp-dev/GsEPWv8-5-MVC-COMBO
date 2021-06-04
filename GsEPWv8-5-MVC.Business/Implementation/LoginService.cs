using  GsEPWv8_5_MVC.Business.Interface;
using  GsEPWv8_5_MVC.Core.Entity;
using  GsEPWv8_5_MVC.Data.Implementation;
using  GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  GsEPWv8_5_MVC.Business.Implementation
{
    public class LoginService : ILoginService
    {
        ILoginRepository objRepository = new LoginRepository();

        public Login LoginAuthentication(Login objLogin)
        {
            return objRepository.LoginAuthentication(objLogin);
        }
        public bool CheckPasswordExpiry(string p_str_user_id)
        {
            return objRepository.CheckPasswordExpiry(p_str_user_id);
        }
    }
}
