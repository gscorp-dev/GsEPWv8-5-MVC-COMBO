using System.Collections.Generic;
using GsEPWv8_5_MVC.Core.Entity;


namespace GsEPWv8_5_MVC.Model
{
   public class CarrierModel
    {
        private string _carrier_id;
        private string _carrier_name;
        public string cmp_id { get; set; }
        public string tmp_cmp_id { get; set; }
        public string screentitle { get; set; }
        public string option { get; set; }
        public string carrier_id { get { return _carrier_id; } set { _carrier_id = value; } }
        public string carrier_name { get { return _carrier_name; } set { _carrier_name = value; } }
        public CarrierHdr objCarrierHdr;
        public IList<CarrierHdr> lstCarrierList { get; set; }
    }
}
