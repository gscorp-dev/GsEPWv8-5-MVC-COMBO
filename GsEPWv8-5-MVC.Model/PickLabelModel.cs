using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
    public class PickLabelModel
    {
        public string cmp_id { get; set; }
        public string ref_num { get; set; }
        public string batch_num { get; set; }
        public string so_num_from { get; set; }
        public string so_num_to { get; set; }
        public string so_dt_from { get; set; }
        public string so_dt_to { get; set; }
        public List<PickLabelDtl> lstPickLabelDtl { get; set; }
        public List<PickLabelSmry> lstPickLabelSmry { get; set; }
        public List<DocForPrint> lstDocForPrint { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
    }
}
