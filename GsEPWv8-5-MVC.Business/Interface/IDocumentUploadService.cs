using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface IDocumentUploadService
    {
        DocumentUpload GetOBDocumentByType(string pstrCmpId, string pstrSoNum, string pstrDocSubType);
        DocumentUpload GetDocumntUpload(DocumentUpload objDocumentUpload);
        DocumentUpload DeleteDocumntUpload(DocumentUpload objDocumentUpload, string p_str_del_all);
        void SaveDocumentUpload(DocumentUpload objDocumentUpload);
    }
}
