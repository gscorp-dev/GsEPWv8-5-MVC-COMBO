using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
    public class DashBoardService: IDashBoardService
    {
        IDashBoardRepository objRepository = new DashBoardRepository();
        public DashBoard ListDashBoard(DashBoard objDashBoard)
        {
            return objRepository.ListDashBoard(objDashBoard);
        }
        public DashBoard ListOutBound(DashBoard objDashBoard)
        {
            return objRepository.ListOutBound(objDashBoard);
        }
        public DashBoard ListVas(DashBoard objDashBoard)
        {
            return objRepository.ListVas(objDashBoard);
        }
        public DashBoard ListVasOpen(DashBoard objDashBoard)
        {
            return objRepository.ListVasOpen(objDashBoard);
        }
        public DashBoard ListBill(DashBoard objDashBoard)
        {
            return objRepository.ListBill(objDashBoard);
        }
        public DashBoard ListBillOpen(DashBoard objDashBoard)
        {
            return objRepository.ListBillOpen(objDashBoard);
        }
        public DashBoard GetOutBoundShipReq(DashBoard objDashBoard)
        {
            return objRepository.GetOutBoundShipReq(objDashBoard);
        }
        public DashBoard GetOutBoundAloc(DashBoard objDashBoard)
        {
            return objRepository.GetOutBoundAloc(objDashBoard);
        }
        public DashBoard GetOutBoundShipPost(DashBoard objDashBoard)
        {
            return objRepository.GetOutBoundShipPost(objDashBoard);
        }
        public DashBoard GetInboundRcvdData(DashBoard objDashBoard)
        {
            return objRepository.GetInboundRcvdData(objDashBoard);
        }
        public DashBoard GetInboundOpenData(DashBoard objDashBoard)
        {
            return objRepository.GetInboundOpenData(objDashBoard);
        }
        public DashBoard GetInboundPostData(DashBoard objDashBoard)
        {
            return objRepository.GetInboundPostData(objDashBoard);
        }
    }
}
