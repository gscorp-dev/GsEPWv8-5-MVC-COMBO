using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
    public class DataUpdateModel
    {
        public string ib_doc_id { get; set; }
        public string cmp_id { get; set; }
        public string cmp_name { get; set; }
        public string ib_doc_rcvd_dt { get; set; }
        public string ib_doc_new_rcvd_dt { get; set; }
        public string Itmdtl { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string itm_name { get; set; }
        public decimal length { get; set; }
        public decimal width { get; set; }
        public decimal depth { get; set; }
        public decimal cube { get; set; }
        public decimal wgt { get; set; }
        public string rcvd_dt { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<DataUpdate> ListCheckIBDocIDExist { get; set; }
        public IList<DataUpdate> ListCheckItmExist { get; set; }
        public IList<DataUpdate> LstItmdtl { get; set; }
        public IList<DataUpdate> LstItm { get; set; }
    }
}
