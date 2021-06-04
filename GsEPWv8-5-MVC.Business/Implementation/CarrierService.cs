using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;

namespace GsEPWv8_5_MVC.Business.Implementation
{
    public class CarrierService :ICarrierService
    {
        ICarrierRepository objCarrier = new CarrierRepository();

     public   Carrier fnGetCarrierList(string pstrCarrierId, string pstrCarrierName)
        {
            return objCarrier.fnGetCarrierList(pstrCarrierId, pstrCarrierName);
        }

        public bool fnSaveCarrierDetails(string pstrOption, string pstrCarrierId, CarrierHdr objCarrierHdr)
        {
            return objCarrier.fnSaveCarrierDetails(pstrOption,pstrCarrierId, objCarrierHdr);
        }

    }
}
