using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Interface
{
    public interface IStockAsOfDateRepository
    {
        StockAsOfDate GetStockAsOfDateDetails(StockAsOfDate objStockAsOfDate);
        StockAsOfDate GetStockAsOfDateDetailsRpt(StockAsOfDate objStockAsOfDate);
        StockAsOfDate GetTotalCottonStockAsOfDateDetails(StockAsOfDate objStockAsOfDate);
        StockAsOfDate GetStockAsOfDateDetailsRptExcel(StockAsOfDate objStockAsOfDate);
        DataTable getInvAsOfDateByStyle(StockAsOfDate objStockAsOfDate);
        DataTable getInvAsOfDateByStyleSummary(StockAsOfDate objStockAsOfDate);
    }
}
