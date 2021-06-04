using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Interface
{
    public interface IDashBoardRepository
    {
        DashBoard ListDashBoard(DashBoard objDashBoard);
        DashBoard ListOutBound(DashBoard objDashBoard);
        DashBoard GetOutBoundShipReq(DashBoard objDashBoard);
        DashBoard GetOutBoundAloc(DashBoard objDashBoard);
        DashBoard GetOutBoundShipPost(DashBoard objDashBoard);
        DashBoard ListVas(DashBoard objDashBoard);
        DashBoard ListVasOpen(DashBoard objDashBoard);
        DashBoard ListBill(DashBoard objDashBoard);
        DashBoard ListBillOpen(DashBoard objDashBoard);
        DashBoard GetInboundRcvdData(DashBoard objDashBoard);
        DashBoard GetInboundOpenData(DashBoard objDashBoard);
        DashBoard GetInboundPostData(DashBoard objDashBoard);

    }
}
