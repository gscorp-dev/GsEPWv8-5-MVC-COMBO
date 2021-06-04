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
    public class PickService: IPickService
    {
        public Pick GetWhsPick(Pick objPick)
        {
            return objRepository.GetWhsPick(objPick);
        }
        PickRepository objRepository = new PickRepository();
        public Pick GetWhsPickDetails(string term, string cmp_id)
        {
            return objRepository.GetWhsPickDetails(term, cmp_id);
        }
        public Pick GetCountryPick(Pick objPick)
        {
            return objRepository.GetCountryPick( objPick);
        }
        public Pick GetStatePick(Pick objPick)
        {
            return objRepository.GetStatePick( objPick);
        }
        public Pick GetShipToPick(Pick objPick)
        {
            return objRepository.GetShipToPick(objPick);
        }
        public Pick GetPickExsistShipTo(Pick objPick)
        {
            return objRepository.GetPickExsistShipTo(objPick);
        }
        public Pick GetPickUom(Pick objPick)
        {
            return objRepository.GetPickUom(objPick);
        }
        public Pick GetPickItemCode(Pick objPick)
        {
            return objRepository.GetPickItemCode(objPick);
        }
    }
}
