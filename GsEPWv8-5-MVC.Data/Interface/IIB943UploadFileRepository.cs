using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Interface
{
    public interface IIB943UploadFileRepository
    {

        bool Check943UploadFileExists(string p_str_cmp_id, string p_str_file_name);
        int Get943UploadRefNum(string p_str_cmp_id);
        bool CheckCntrIdExists(string p_str_cmp_id, string p_str_cntr_id);
        string SaveIB943UploadFile(string p_str_cmp_id, DataTable p_dt__Ib_943_upload_file_info, DataTable p_dt__Ib_943_upload_file_hdr, DataTable p_dt__Ib_943_upload_file_dtl);
        string MoveIB943UploadToIBDocTables(string p_str_cmp_id, string p_str_upload_ref_num);
        IB943UploadFile Send943EmailAckReport(string p_str_cmp_id, string p_str_file_name, string p_str_upload_ref_num, IB943UploadFile objIB943UploadFile);
         DataTable GetIB940UploadFileSummary(string p_str_cmp_id, string p_str_file_name, string p_str_upload_ref_num);
        DataTable fnGetIBDocListBy943(string pstrCmpId, string pstrUploadRefNo);

    }
}
