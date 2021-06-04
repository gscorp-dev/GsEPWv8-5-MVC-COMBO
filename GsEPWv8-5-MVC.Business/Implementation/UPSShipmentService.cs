using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;

namespace GsEPWv8_5_MVC.Business.Implementation
{
 
    public class UPSShipmentService : IUPSShipmentService
    {
        UPSShipmentRepository objRepository = new Data.Implementation.UPSShipmentRepository();
        public UPSShipment GetUPSShipperDetails(string pstrCmpId, string pstrWhsId, string pstrSoNum, string pstrUserName, string pstrAccountId)
        {
            return objRepository.GetUPSShipperDetails(pstrCmpId, pstrWhsId, pstrSoNum, pstrUserName, pstrAccountId);

        }
    }
}
