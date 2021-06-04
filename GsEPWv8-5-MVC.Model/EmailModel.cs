using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GsEPWv8_5_MVC.Model
{
   public  class EmailDtlModel
    {
        private string _EmailCC = string.Empty;
        public string CmpId { get; set; }
        public string EmailSubject { get; set; }
        public string EmailTo { get; set; }
        //public string EmailCC { get; set; }
        public string EmailBcc { get; set; }
        public string EmailMessage { get; set; }
        public string Attachment { get; set; }
        public string EmailMessageContent { get; set; }
        public string user_fst_name { get; set; }
        public string user_lst_name { get; set; }
        public string email { get; set; }
        public string user_id { get; set; }
        public string Actiontype { get; set; }
        public string FilePath { get; set; }
        public string rpt_file_format { get; set; }
        public string is_auto_email { get; set; }
        public string non_cmp_email { get; set; }
        public string EmailCC
        {
            get
            {
                return _EmailCC;
            }

            set
            {
                _EmailCC = value;
            }
        }
    }

    public class EmailModel : EmailDtlModel
    {
        public IList<Email> ListEamilDetail { get; set; }
        public IList<Email> ListGetMail { get; set; }
        public string Message { get; set; }
        public string formControl { get; set; }

    }

}
