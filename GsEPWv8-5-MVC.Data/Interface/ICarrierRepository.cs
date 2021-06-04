using GsEPWv8_5_MVC.Core.Entity;
using System.Collections.Generic;

namespace GsEPWv8_5_MVC.Data.Interface
{
    public interface ICarrierRepository
    {
        Carrier fnGetCarrierList(string pstrCarrierId, string pstrCarrierName);
        bool fnSaveCarrierDetails(string pstrOption, string pstrCarrierId, CarrierHdr objCarrierHdr);
    }
}
