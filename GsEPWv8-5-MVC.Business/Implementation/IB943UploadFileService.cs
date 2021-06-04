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
    public class IB943UploadFileService: IIB943UploadFileService
    {
        IB943UploadFileRepository objeComIBCntrUploadRepository = new IB943UploadFileRepository();

        public bool Check943UploadFileExists(string p_str_cmp_id, string p_str_file_name)
        {
            return objeComIBCntrUploadRepository.Check943UploadFileExists(p_str_cmp_id, p_str_file_name);
        }


        public int Get943UploadRefNum(string p_str_cmp_id)
        {
            return objeComIBCntrUploadRepository.Get943UploadRefNum(p_str_cmp_id);
        }
        public bool CheckCntrIdExists(string p_str_cmp_id, string p_str_ref_num)
        {
            return objeComIBCntrUploadRepository.CheckCntrIdExists(p_str_cmp_id, p_str_ref_num);
        }

        public string SaveIB943UploadFile(string p_str_cmp_id, DataTable p_dt__Ib_943_upload_file_info, DataTable p_dt__Ib_943_upload_file_hdr, DataTable p_dt__Ib_943_upload_file_dtl)
        {
            return objeComIBCntrUploadRepository.SaveIB943UploadFile(p_str_cmp_id, p_dt__Ib_943_upload_file_info, p_dt__Ib_943_upload_file_hdr, p_dt__Ib_943_upload_file_dtl);
        }

        public string MoveIB943UploadToIBDocTables(string p_str_cmp_id, string p_str_upload_ref_num)

        {
            return objeComIBCntrUploadRepository.MoveIB943UploadToIBDocTables(p_str_cmp_id, p_str_upload_ref_num);

        }
       public IB943UploadFile Send943EmailAckReport(string p_str_cmp_id, string p_str_file_name, string p_str_upload_ref_num, IB943UploadFile objIB943UploadFile)     
        {
            return objeComIBCntrUploadRepository.Send943EmailAckReport(p_str_cmp_id, p_str_file_name, p_str_upload_ref_num,objIB943UploadFile);
        }

        public DataTable GetIB940UploadFileSummary(string p_str_cmp_id, string p_str_file_name, string p_str_upload_ref_num)
        {
            return objeComIBCntrUploadRepository.GetIB940UploadFileSummary(p_str_cmp_id, p_str_file_name, p_str_upload_ref_num);

        }

        public DataTable fnGetIBDocListBy943(string pstrCmpId, string pstrUploadRefNo)

        {
            return objeComIBCntrUploadRepository.fnGetIBDocListBy943(pstrCmpId, pstrUploadRefNo);
        }

}
}
