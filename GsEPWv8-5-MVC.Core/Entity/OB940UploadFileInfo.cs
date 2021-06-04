using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class OB940UploadFileInfo
    {
        #region Constructors  
        public OB940UploadFileInfo()
        {

        }
        #endregion
        #region Private Fields  
        private string _cmp_id;
        private string _file_name;
        private string _upload_ref_num;
        private string _status;
        private string _upload_by;
        private DateTime _upload_date_time;
        private int _no_of_lines;
        private string _comments;
        #endregion
        #region Public Properties  
        public string cmp_id { get { return _cmp_id; } set { _cmp_id = value; } }
       
        public string file_name { get { return _file_name; } set { _file_name = value; } }
        public string upload_ref_num { get { return _upload_ref_num; } set { _upload_ref_num = value; } }
        public string status { get { return _status; } set { _status = value; } }
        public string upload_by { get { return _upload_by; } set { _upload_by = value; } }
        public DateTime upload_date_time { get { return _upload_date_time; } set { _upload_date_time = value; } }
        public int no_of_lines { get { return _no_of_lines; } set { _no_of_lines = value; } }
        public string comments { get { return _comments; } set { _comments = value; } }
        #endregion
    }

}
