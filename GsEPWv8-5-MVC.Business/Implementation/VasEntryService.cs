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
    public class VasEntryService : IVasEntryService
    {
        VasEntryRepository objRepository = new VasEntryRepository();
        public VasEntry GetVasEntryDetails(VasEntry objVasEntry)
        {
            return objRepository.GetVasEntryDetails(objVasEntry);
        }
       

      
    }
}
