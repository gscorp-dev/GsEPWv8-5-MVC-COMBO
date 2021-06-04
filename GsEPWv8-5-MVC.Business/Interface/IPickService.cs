using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface IPickService
    {
        Pick GetWhsPick(Pick objPick);
        Pick GetWhsPickDetails(string term, string cmp_id);
        Pick GetCountryPick(Pick objPick);
        Pick GetStatePick(Pick objPick);
        Pick GetShipToPick(Pick objPick);
        Pick GetPickExsistShipTo(Pick objPick);
        Pick GetPickUom(Pick objPick);
        Pick GetPickItemCode(Pick objPick);



    }
}
