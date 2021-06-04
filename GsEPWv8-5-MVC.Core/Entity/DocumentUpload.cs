using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class DocumentUpload
    {
        #region Constructors  
        public DocumentUpload() { }

        #endregion
        #region Private Fields  
        private string _cmp_id;
        private string _doc_type;
        private string _doc_id;
        private string _cntr_id;
        private string _orig_file_name;
        private string _upload_file_name;
        private string _file_path;
        private DateTime _upload_dt;
        private string _upload_by;
        private string _comments;
        private string _doc_sub_type;
        #endregion

        #region Public Properties  
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
        public string doc_type { get { return _doc_type; } set { _doc_type = value; } }
        public string doc_id { get { return _doc_id; } set { _doc_id = value; } }
        public string cntr_id { get { return _cntr_id; } set { _cntr_id = value; } }
        public string orig_file_name { get { return _orig_file_name; } set { _orig_file_name = value; } }
        public string upload_file_name { get { return _upload_file_name; } set { _upload_file_name = value; } }
        public string file_path { get { return _file_path; } set { _file_path = value; } }
        public DateTime upload_dt { get { return _upload_dt; } set { _upload_dt = value; } }
        public string upload_by { get { return _upload_by; } set { _upload_by = value; } }
        public string comments { get { return _comments; } set { _comments = value; } }
        public string doc_sub_type { get { return _doc_sub_type; } set { _doc_sub_type = value; } }
        public IList<DocumentUpload> LstDocumentUpload { get; set; }
        public IList<LookUp> ListDocumentsSubType { get; set; }

        #endregion
    }

   
}
