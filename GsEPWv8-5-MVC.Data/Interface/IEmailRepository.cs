using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GsEPWv8_5_MVC.Core.Entity;
namespace GsEPWv8_5_MVC.Data.Interface
{
  public   interface IEmailRepository
    {
        Email GetSendMailDetails(Email objSendMail);
        Email GetMailDetails(Email objSendMail);
    }
}
