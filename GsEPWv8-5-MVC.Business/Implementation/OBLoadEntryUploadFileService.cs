using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
    public class OBLoadEntryUploadFileService : IOBLoadEntryUploadFileService
    {
        OBLoadEntryUploadFileRepository objOBLoadEntryUploadFileRepository = new OBLoadEntryUploadFileRepository();

        public bool CheckLoadEntryFileExists(string p_str_cmp_id, string p_str_file_name)
        {
            return objOBLoadEntryUploadFileRepository.CheckLoadEntryFileExists(p_str_cmp_id, p_str_file_name);
        }
        public string CheckBatchCustPOExists(string p_str_cmp_id, string p_str_batch_num, string p_str_cust_po, string p_str_dept_id, string p_str_store_id)
        {
            return objOBLoadEntryUploadFileRepository.CheckBatchCustPOExists( p_str_cmp_id,  p_str_batch_num,  p_str_cust_po,  p_str_dept_id,  p_str_store_id);
        }
        public int GetOBLoadUploadRefNum(string p_str_cmp_id)
        {
            return objOBLoadEntryUploadFileRepository.GetOBLoadUploadRefNum(p_str_cmp_id);
        }
        public bool SaveOBLoadEntryBatch(string p_str_cmp_id, DataTable dt_ob_load_file_info, DataTable dt_ob_load_batch_dtl, string user_id)
        {
            return objOBLoadEntryUploadFileRepository.SaveOBLoadEntryBatch(p_str_cmp_id, dt_ob_load_file_info, dt_ob_load_batch_dtl, user_id);
        }
    }
}
