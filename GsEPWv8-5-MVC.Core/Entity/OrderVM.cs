using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class OrderVM
    {
        public tbl_so_hdr order { get; set; }
        public List<tbl_so_dtl> orderDetails { get; set; }
    }
}
