using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Interface
{
     interface IUPSShipmentRepository
    {
        UPSShipment GetUPSShipperDetails(string pstrCmpId, string pstrWhsId, string pstrSoNum,  string pstrUserName, string pstrAccountId);
    }
}
