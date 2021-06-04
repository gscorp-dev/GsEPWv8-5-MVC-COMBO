using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface IStockAsOfDateService
    {
        StockAsOfDate GetStockAsOfDateDetails(StockAsOfDate objStockAsOfDate);
        StockAsOfDate GetStockAsOfDateDetailsRpt(StockAsOfDate objStockAsOfDate);//CR-3PL-MVC-180322-001 Added By NIthya
        StockAsOfDate GetTotalCottonStockAsOfDateDetails(StockAsOfDate objStockAsOfDate);
        StockAsOfDate GetStockAsOfDateDetailsRptExcel(StockAsOfDate objStockAsOfDate);
        DataTable getInvAsOfDateByStyle(StockAsOfDate objStockAsOfDate);
        DataTable getInvAsOfDateByStyleSummary(StockAsOfDate objStockAsOfDate);
    }
}
