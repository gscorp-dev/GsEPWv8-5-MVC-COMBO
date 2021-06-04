using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Interface
{
    public interface ILocationMasterRepository
    {
         LocationMaster GetLocationMasterDetails(LocationMaster objLocationMaster);
        LocationMaster InsertLocationMasterDetails(LocationMaster objLocationMaster);
        LocationMaster DeleteLocationMasterDetails(LocationMaster objLocationMaster);
        LocationMaster GetWhsPickDetails(string term, string cmp_id);
        LocationMaster CHECKLOCIDEXIST(LocationMaster objLocationMaster);

    }
}
