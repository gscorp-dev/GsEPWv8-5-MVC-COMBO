using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class Priceuom : BasicEntity
    { 
    public string uom_id { get; set; }
    public string uom_desc { get; set; }
    public string uom_type { get; set; }
    }
    public class Price : Priceuom
    { 
    public IList<Priceuom> ListPriceuom { get; set; }
   
    }
}

