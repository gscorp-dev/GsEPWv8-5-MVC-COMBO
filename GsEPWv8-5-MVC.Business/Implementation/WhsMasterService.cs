using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using GsEPWv8_5_MVC.Data.Interface;
namespace GsEPWv8_5_MVC.Business.Implementation
{
    public class WhsMasterService : IWhsMasterService

    {

        IWhsMasterRepository objRepository = new WhsMasterRepository();      
       public WhsMaster GetWhsMasterDetails(WhsMaster objWhsMaster)
        {
            return objRepository.GetWhsMasterDetails(objWhsMaster);
        }
        public void SaveWhsMasterHdr(WhsMaster objWhsMaster)
        {
            objRepository.SaveWhsMasterHdr(objWhsMaster);
        }

        public WhsMaster CheckWhsIDInUse(WhsMaster objWhsMaster)
        {
            return objRepository.CheckWhsIDInUse(objWhsMaster);
        }
        public WhsMaster CheckWhsIDIsExist(WhsMaster objWhsMaster)
        {
            return objRepository.CheckWhsIDIsExist(objWhsMaster);
        }
        public WhsMaster GetPickCompanyDetails(WhsMaster objWhsMaster)
        {
            return objRepository.GetPickCompanyDetails(objWhsMaster);
        }
       public WhsMaster DefltWhsCannotDelete(WhsMaster objWhsMaster)
        {
            return objRepository.DefltWhsCannotDelete(objWhsMaster);
        }
    }
}
