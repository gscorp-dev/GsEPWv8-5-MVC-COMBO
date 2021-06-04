using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Interface
{
    public interface IPickRepository
    {
        Pick GetWhsPick(Pick objPick);
        Pick GetWhsPickDetails(string term, string cmp_id);
        Pick GetCountryPick(Pick objPick);
        Pick GetStatePick(Pick objPick);
        Pick GetPickExsistShipTo(Pick objPick);
        Pick GetPickUom(Pick objPick);

    }
}
