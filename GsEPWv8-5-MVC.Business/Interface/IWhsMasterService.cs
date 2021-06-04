using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GsEPWv8_5_MVC.Core.Entity;

namespace GsEPWv8_5_MVC.Business.Interface
{
   public  interface IWhsMasterService
    {
        void SaveWhsMasterHdr(WhsMaster objWhsMaster);
        WhsMaster GetWhsMasterDetails(WhsMaster objWhsMaster);
        WhsMaster CheckWhsIDIsExist(WhsMaster objWhsMaster);
        WhsMaster CheckWhsIDInUse(WhsMaster objWhsMaster);
        WhsMaster GetPickCompanyDetails(WhsMaster objWhsMaster);
        WhsMaster DefltWhsCannotDelete(WhsMaster objWhsMaster);
    }
}
