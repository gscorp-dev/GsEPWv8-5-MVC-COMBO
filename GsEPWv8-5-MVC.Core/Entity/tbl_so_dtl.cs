using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class tbl_so_dtl
    {
        public tbl_so_dtl()
        {
        }
        public string CompID { get; set; }
        public string BatchNo { get; set; }
        public string CustID { get; set; }
        public string CustPO { get; set; }
        public Int32 POLine { get; set; }
        public string Style { get; set; }
        public string CustSKU { get; set; }
        public int StyleQty { get; set; }
        public int StyleCarton { get; set; }
        public string StylePPK { get; set; }
        public double StyleCube { get; set; }
        public double StyleWgt { get; set; }
        public string StyleDesc { get; set; }
        public int StyleStatus { get; set; }
        public string StatusDesc { get; set; }
        public string Itm_Code { get; set; }
    }
}
