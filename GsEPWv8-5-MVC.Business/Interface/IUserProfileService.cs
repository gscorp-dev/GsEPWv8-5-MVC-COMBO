using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface IUserProfileService
    {
        UserProfile CheckExistUserID(UserProfile ObjUserProfile);
        UserProfile UpdateUserAccPwd(UserProfile ObjUserProfile);
    }
}
