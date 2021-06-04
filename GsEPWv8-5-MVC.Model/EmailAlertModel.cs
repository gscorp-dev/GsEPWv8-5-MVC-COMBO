using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
    public class EmailAlertModel
    {
        public EmailCommon objEmailCommon { get; set; }
        public EmailAlertHdr objEmailAlertHdr { get; set; }
        public IList<EmailAlertHdr> lstEmailAlertHdr { get; set; }
        public EmailAlertDtl objEmailAlertDtl { get; set; }
        public string formControl { get; set; }
    }
}
