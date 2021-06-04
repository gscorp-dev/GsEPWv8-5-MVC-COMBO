using System;
using GsEPWv8_5_MVC.Core.Entity;

namespace GsEPWv8_5_MVC.Data.Interface
{
   public interface IEcomLinkRepository
    {
         ClsEcomLink fnGetCustEcomLinkDtl(string pstrCmpId);
    }
}
