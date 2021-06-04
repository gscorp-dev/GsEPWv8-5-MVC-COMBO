using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
    public class PriceuomService : IPriceuomService
    {
        public Price GetPriceuomDetails(Price objIPriceuomService)
        {
            return objRepository.GetPriceuomDetails(objIPriceuomService);
        }
        PriceuomRepository objRepository = new PriceuomRepository();
    }
}
