using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
    public class DataUpdateService: IDataUpdateService
    {
        DataUpdateRepository ObjDataUpdateRepository = new DataUpdateRepository();
        public DataUpdate CheckIbDocIdExist(DataUpdate ObjDataUpdate)
        {
            return ObjDataUpdateRepository.CheckIbDocIdExist(ObjDataUpdate);
        }
        public DataUpdate UpdateIBDocRcvdDate(DataUpdate ObjDataUpdate)
        {
            return ObjDataUpdateRepository.UpdateIBDocRcvdDate(ObjDataUpdate);
        }
        public DataUpdate GetItemDetails(string term, string cmp_id)
        {
            return ObjDataUpdateRepository.GetItemDetails(term,cmp_id);
        }
        public DataUpdate UpdateItemDetails(DataUpdate ObjDataUpdate)
        {
            return ObjDataUpdateRepository.UpdateItemDetails(ObjDataUpdate);
        }
        public DataUpdate GetItemList(DataUpdate ObjDataUpdate)
        {
            return ObjDataUpdateRepository.GetItemList(ObjDataUpdate);
        }
        public DataUpdate CheckExistItem(DataUpdate ObjDataUpdate)
        {
            return ObjDataUpdateRepository.CheckExistItem(ObjDataUpdate);
        }
    }
}
