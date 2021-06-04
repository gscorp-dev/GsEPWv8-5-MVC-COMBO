using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
    public class ForgotPasswordService: IForgotPasswordService
    {
        ForgotPasswordRepository ObjForgotPasswordRepository = new ForgotPasswordRepository();
       public ForgotPassword CheckUserIDExist(ForgotPassword ObjForgotPassword)
        {
            return ObjForgotPasswordRepository.CheckUserIDExist(ObjForgotPassword);
        }
        public ForgotPassword UpdateUserAccPwd(ForgotPassword ObjForgotPassword)
        {
            return ObjForgotPasswordRepository.UpdateUserAccPwd(ObjForgotPassword);
        }
    }
}
