using GsEPWv8_5_MVC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;

namespace GsEPWv8_5_MVC.Business.Implementation
{
    public class LocationMasterService : ILocationMasterService
    {
        LocationMasterRepository objLocationMasterRepository = new LocationMasterRepository();
        public LocationMaster GetLocationMasterDetails(LocationMaster objLocationMaster)
        {
            return objLocationMasterRepository.GetLocationMasterDetails(objLocationMaster);
        }

        public LocationMaster InsertLocationMasterDetails(LocationMaster objLocationMaster)
        {
            return objLocationMasterRepository.InsertLocationMasterDetails(objLocationMaster);
        }

        public LocationMaster DeleteLocationMasterDetails(LocationMaster objLocationMaster)
        {
            return objLocationMasterRepository.DeleteLocationMasterDetails(objLocationMaster);
        }
        public LocationMaster GetWhsPickDetails(string term, string cmp_id)
        {
            return objLocationMasterRepository.GetWhsPickDetails(term, cmp_id);
        }
        public LocationMaster CHECKLOCIDEXIST(LocationMaster objLocationMaster)
        {
            return objLocationMasterRepository.CHECKLOCIDEXIST(objLocationMaster);
        }
    }
}
