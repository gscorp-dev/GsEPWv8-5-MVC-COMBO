using GsEPWv8_4_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_4_MVC.Data.Interface
{
    public interface IeComIB943UploadRepository
    {

        bool Check943UploadFileExists(string p_str_cmp_id, string p_str_file_name);
        int Get943UploadRefNum(string p_str_cmp_id);

        void InsertIB943UploadTempHdrtblDetails(eComIB943Upload objeComIB943Upload);
        void InsertIB943UploadTempDtltblDetails(eComIB943Upload objeComIB943Upload);
        void InsertIB943UploadDocHdrtblDetails(eComIB943Upload objeComIB943Upload);
        void InsertIB943UploadDocDtltblDetails(eComIB943Upload objeComIB943Upload);
        void InsertDetailstoCtntable(eComIB943Upload objeComIB943Upload);
        void InsertDetailstoAuditTrail(eComIB943Upload objeComIB943Upload);
        eComIB943Upload CheckIBUploadedStyleExist(eComIB943Upload objeComIB943Upload);
        eComIB943Upload GetIB943UploadTempHdrtblDetails(eComIB943Upload objeComIB943Upload);
        eComIB943Upload GetIB943UploadTempDtltblDetails(eComIB943Upload objeComIB943Upload);
        eComIB943Upload DeleteTempTable(eComIB943Upload eComIB943Upload);
        eComIB943Upload AddECom943UploadFileNAme(eComIB943Upload objeComIB943Upload);
        void UpdateTblIbDocDtl(eComIB943Upload objeComIB943Upload);
        void InsertIB943SingleLineUploadTempDtltblDetails(eComIB943Upload objeComIB943Upload);
        eComIB943Upload GetIB943SingleLineUploadTempDtltblDetails(eComIB943Upload objeComIB943Upload);
        void DeleteSingleLineCntrUploadTempTable(eComIB943Upload eComIB943Upload);

    }
}
