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
   public class LabelPrintService : ILabelPrintService
    {
        LabelPrintRepository objRepository = new LabelPrintRepository();
        public OBCtnLblPrnt GetOBLabelPrintDetails(OBCtnLblPrnt objOBCtnLblPrnt)
        {
            return objRepository.GetOBLabelPrintDetails(objOBCtnLblPrnt);
        }
    }
}
