using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Interface
{
    public interface  IDocumentUploadRepository
    {

        EmailAlertHdr fnGetAttachments(string pstrCmpId, string pstrDocId, string pstrDocType);
        DocumentUpload GetOBDocumentByType(string pstrCmpId, string pstrSoNum, string pstrDocSubType);
        DocumentUpload GetDocumntUpload(DocumentUpload objDocumentUpload);
        DocumentUpload DeleteDocumntUpload(DocumentUpload objDocumentUpload, string p_str_del_all);
        void SaveDocumentUpload(DocumentUpload objDocumentUpload);
    }
}
