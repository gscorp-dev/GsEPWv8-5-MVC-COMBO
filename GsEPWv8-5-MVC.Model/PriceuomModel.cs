using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
    public class PriceuomModel  : BasicEntity
    {
        public string uom_id { get; set; }
        public string uom_desc { get; set; }
        public string uom_type { get; set; }
    }
    public class PriceModel : PriceuomModel
    {
        public IList<Priceuom> ListPriceuom { get; set; }


    }
}
