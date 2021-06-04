using  GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  GsEPWv8_5_MVC.Data.Interface
{
    public interface ILookUpRepository
    {
        LookUp GetLookUpValue(LookUp objLookUp);
    }
}
