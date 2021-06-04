using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;

namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface ICarrierService
    {
        Carrier fnGetCarrierList(string pstrCarrierId, string pstrCarrierName);
        bool fnSaveCarrierDetails(string pstrOption, string pstrCarrierId, CarrierHdr objCarrierHdr);
    }
}
