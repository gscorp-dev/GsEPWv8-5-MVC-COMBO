using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
    public class StockAsOfDateService : IStockAsOfDateService
    {
        StockAsOfDateRepository objRepository = new StockAsOfDateRepository();
        public StockAsOfDate GetStockAsOfDateDetails(StockAsOfDate objStockAsOfDate)
        {
            return objRepository.GetStockAsOfDateDetails(objStockAsOfDate);
        }
        public StockAsOfDate GetStockAsOfDateDetailsRpt(StockAsOfDate objStockAsOfDate)//CR-3PL-MVC-180322-001 Added By NIthya
        {
            return objRepository.GetStockAsOfDateDetailsRpt(objStockAsOfDate);
        }
        public StockAsOfDate GetStockAsOfDateDetailsRptExcel(StockAsOfDate objStockAsOfDate)
        {
            return objRepository.GetStockAsOfDateDetailsRptExcel(objStockAsOfDate);
        }

        public StockAsOfDate GetTotalCottonStockAsOfDateDetails(StockAsOfDate objStockAsOfDate)
        {
            return objRepository.GetTotalCottonStockAsOfDateDetails(objStockAsOfDate);
        }
        public DataTable getInvAsOfDateByStyle(StockAsOfDate objStockAsOfDate)
        {
            return objRepository.getInvAsOfDateByStyle(objStockAsOfDate);
        }
        public DataTable getInvAsOfDateByStyleSummary(StockAsOfDate objStockAsOfDate)
        {
            return objRepository.getInvAsOfDateByStyleSummary(objStockAsOfDate);
        }
    }
}
