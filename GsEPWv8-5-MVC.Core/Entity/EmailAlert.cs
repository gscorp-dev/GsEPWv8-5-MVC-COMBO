using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class EmailAlert 
    {
        public EmailCommon objEmailCommon { get; set; }
        public EmailAlertHdr objEmailAlertHdr { get; set; }
        public IList<EmailAlertHdr> lstEmailAlertHdr { get; set; }
        public EmailAlertDtl objEmailAlertDtl { get; set; }
        public string formControl { get; set; }


    }

  

    public class EmailAlertHdr
    {
        public string cmpId { get; set; }
        public string userId { get; set; }
        public string moduleName { get; set; }
        public string rptId { get; set; }
        public string emailFrom { get; set; }
        public string cmpEmailTo { get; set; }
        public string custEmailTo { get; set; }
        public string carrierEmailTo { get; set; }
        public string emailCC { get; set; }
        public string emailBcc { get; set; }
        public string emailSubject { get; set; }
        public string emailMessage { get; set; }
        public string emailFooter { get; set; }
        public string isiInternal { get; set; }
        public string rptFileFormat { get; set; }
        public string isAutoEmail { get; set; }
        public string filePath { get; set; }
        public string fileName { get; set; }
         public IList<AttachDocList> LstAttachDocs { get; set; }

    }

    public class AttachDocList
    {
        public string cmp_id { get; set; }
        public string doc_id { get; set; }
        public string doc_sub_type { get; set; }
        public string file_path { get; set; }
        public string upload_file_name { get; set; }
        public string filePathWithName { get; set; }
    }
    public class EmailAlertDtl
    {
        public string cmpId { get; set; }
        public string alertId { get; set; }
        public string moduleName { get; set; }
        public string docId { get; set; }
        public string cntrId { get; set; }
        public string rptId { get; set; }
        public string carrierId { get; set; }
        public DateTime emailSentDateTime { get; set; }
        public string emailFrom { get; set; }
        public string cmpEmailTo { get; set; }
        public string custEmailTo { get; set; }
        public string carrierEmailTo { get; set; }
        public string emailCC { get; set; }
        public string emailBcc { get; set; }
        public string emailSubject { get; set; }
        public string emailMessage { get; set; }
        public string userId { get; set; }
        public string filePath { get; set; }
        public string fileName { get; set; }

    }
}
