using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
    public class UserProfileService
    {
        UserProfileRepository objRepository = new UserProfileRepository();
        public UserProfile CheckExistUserID(UserProfile ObjUserProfile)
        {
            return objRepository.CheckExistUserID(ObjUserProfile);
        }
        public UserProfile UpdateUserAccPwd(UserProfile ObjUserProfile)
        {
            return objRepository.UpdateUserAccPwd(ObjUserProfile);
        }
    }
}
