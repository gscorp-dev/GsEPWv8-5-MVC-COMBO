using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface IDataUpdateService
    {
        DataUpdate CheckIbDocIdExist(DataUpdate ObjDataUpdate);
        DataUpdate UpdateIBDocRcvdDate(DataUpdate ObjDataUpdate);
        DataUpdate GetItemDetails(string term, string cmp_id);
        DataUpdate UpdateItemDetails(DataUpdate ObjDataUpdate);
        DataUpdate GetItemList(DataUpdate ObjDataUpdate);
        DataUpdate CheckExistItem(DataUpdate ObjDataUpdate);

    }
}
