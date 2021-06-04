using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Interface
{
    public interface IOBLoadEntryUploadFileRepository
    {
        bool CheckLoadEntryFileExists(string p_str_cmp_id, string p_str_file_name);
        string CheckBatchCustPOExists(string p_str_cmp_id, string p_str_batch_num, string p_str_cust_po, string p_str_dept_id, string p_str_store_id);
        int GetOBLoadUploadRefNum(string p_str_cmp_id);
        bool SaveOBLoadEntryBatch(string p_str_cmp_id, DataTable dt_ob_load_file_info, DataTable dt_ob_load_batch_dtl, string user_id);
    }
}
