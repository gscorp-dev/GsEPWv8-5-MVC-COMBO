using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GsEPWv8_5_MVC.Core.Entity
{
    public class Carrier
    {
        private string _carrier_id;
        private string _carrier_name;
        public string tmp_cmp_id { get; set; }
        public string cmp_id { get; set; }
        public string screentitle { get; set; }
        public string option { get; set; }
        public string carrier_id { get { return _carrier_id; } set { _carrier_id = value; } }
        public string carrier_name { get { return _carrier_name; } set { _carrier_name = value; } }
        public CarrierHdr objCarrierHdr;
        public IList<CarrierHdr> lstCarrierList { get; set; }
    }

    public class CarrierHdr
    {
        #region Private Fields  
        private string _carrier_id;
        private string _carrier_scac_code;
        private string _carrier_name;
        private string _contact_name;
        private string _contact_office_num;
        private string _contact_cell_num;
        private string _contact_email;
        private string _carrier_alert_email;
        private string _entered_dt;
        private string _entered_by;
        private string _updated_dt;
        private string _updated_by;
        private string _process_id;
        #endregion
        #region Public Properties  
        public string carrier_id { get { return _carrier_id; } set { _carrier_id = value; } }
        public string carrier_scac_code { get { return _carrier_scac_code; } set { _carrier_scac_code = value; } }
        public string carrier_name { get { return _carrier_name; } set { _carrier_name = value; } }
        public string contact_name { get { return _contact_name; } set { _contact_name = value; } }
        public string contact_office_num { get { return _contact_office_num; } set { _contact_office_num = value; } }
        public string contact_cell_num { get { return _contact_cell_num; } set { _contact_cell_num = value; } }
        public string contact_email { get { return _contact_email; } set { _contact_email = value; } }
        public string carrier_alert_email { get { return _carrier_alert_email; } set { _carrier_alert_email = value; } }
        public string entered_dt { get { return _entered_dt; } set { _entered_dt = value; } }
        public string entered_by { get { return _entered_by; } set { _entered_by = value; } }
        public string updated_dt { get { return _updated_dt; } set { _updated_dt = value; } }
        public string updated_by { get { return _updated_by; } set { _updated_by = value; } }
        public string process_id { get { return _process_id; } set { _process_id = value; } }
        #endregion

    }
}
