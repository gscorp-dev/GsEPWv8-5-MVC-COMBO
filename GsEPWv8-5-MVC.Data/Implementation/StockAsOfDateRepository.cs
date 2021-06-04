using Dapper;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Implementation
{
    public class StockAsOfDateRepository : IStockAsOfDateRepository
    {
        public StockAsOfDate GetStockAsOfDateDetails(StockAsOfDate objStockAsOfDate)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "SP_MVC_IV_STOCK_ASOFDATE_INQUIRY_DTL";
                    IEnumerable<StockAsOfDate> ListstockInquiryLIST = connection.Query<StockAsOfDate>(storedProcedure1,
                        new
                        {
                            @P_STR_CMPID   = objStockAsOfDate.cmp_id,
                            @P_STR_As_Of_Dt = objStockAsOfDate.As_Of_Date,
                            @P_STR_Style = objStockAsOfDate.itm_num,
                            @P_STR_Color = objStockAsOfDate.itm_color,
                            @P_STR_Size = objStockAsOfDate.itm_size,
                            @P_STR_Desc = objStockAsOfDate.itm_name,
                            @P_STR_STYLE_SEARCH = objStockAsOfDate.status,
                            @P_STR_IB_Doc_ID = objStockAsOfDate.ib_doc_id,
                            @P_STR_Cont_ID = objStockAsOfDate.cont_id,
                            @P_STR_Lot_ID = objStockAsOfDate.lot_id,
                            @P_STR_Loc_ID = objStockAsOfDate.loc_id,
                            @P_STR_Ref_No = objStockAsOfDate.grn_id,
                            @P_STR_Whs_ID = objStockAsOfDate.whs_id,
                           

                        },
                        commandType: CommandType.StoredProcedure);
                    objStockAsOfDate.ListStockAsOfDateGrid = ListstockInquiryLIST.ToList();
                }

                return objStockAsOfDate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
            }
        }

        public DataTable getInvAsOfDateByStyle(StockAsOfDate objStockAsOfDate)
        {
            DataTable dtInvAsOfDate = new DataTable();

            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("SP_MVC_IV_STOCK_ASOFDATE_INQUIRY_DTL", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@P_STR_CMPID", SqlDbType.VarChar).Value = objStockAsOfDate.cmp_id;
                command.Parameters.Add("@P_STR_As_Of_Dt", SqlDbType.VarChar).Value = objStockAsOfDate.As_Of_Date;
                command.Parameters.Add("@P_STR_Style", SqlDbType.VarChar).Value = objStockAsOfDate.itm_num;
                command.Parameters.Add("@P_STR_Color", SqlDbType.VarChar).Value = objStockAsOfDate.itm_color;
                command.Parameters.Add("@P_STR_Size", SqlDbType.VarChar).Value = objStockAsOfDate.itm_size;
                command.Parameters.Add("@P_STR_Desc", SqlDbType.VarChar).Value = objStockAsOfDate.itm_name;
                command.Parameters.Add("@P_STR_STYLE_SEARCH", SqlDbType.VarChar).Value = objStockAsOfDate.style_stearch;
                command.Parameters.Add("@P_STR_IB_Doc_ID", SqlDbType.VarChar).Value = objStockAsOfDate.ib_doc_id;
                command.Parameters.Add("@P_STR_Cont_ID", SqlDbType.VarChar).Value = objStockAsOfDate.cont_id;
                command.Parameters.Add("@P_STR_Lot_ID", SqlDbType.VarChar).Value = objStockAsOfDate.lot_id;
                command.Parameters.Add("@P_STR_Loc_ID", SqlDbType.VarChar).Value = objStockAsOfDate.loc_id;
                command.Parameters.Add("@P_STR_Ref_No", SqlDbType.VarChar).Value = objStockAsOfDate.grn_id;
                command.Parameters.Add("@P_STR_Whs_ID", SqlDbType.VarChar).Value = objStockAsOfDate.whs_id;
                connection.Open();
                dtInvAsOfDate.Load(command.ExecuteReader());
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return dtInvAsOfDate;
        }
        public DataTable getInvAsOfDateByStyleSummary(StockAsOfDate objStockAsOfDate)
        {
            DataTable dtInvAsOfDate = new DataTable();

            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("spIvStockSummaryAsOfDateRpt", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@P_STR_CMPID", SqlDbType.VarChar).Value = objStockAsOfDate.cmp_id;
                command.Parameters.Add("@P_STR_As_Of_Dt", SqlDbType.VarChar).Value = objStockAsOfDate.As_Of_Date;
                command.Parameters.Add("@P_STR_Style", SqlDbType.VarChar).Value = objStockAsOfDate.itm_num;
                command.Parameters.Add("@P_STR_Color", SqlDbType.VarChar).Value = objStockAsOfDate.itm_color;
                command.Parameters.Add("@P_STR_Size", SqlDbType.VarChar).Value = objStockAsOfDate.itm_size;
                command.Parameters.Add("@P_STR_Desc", SqlDbType.VarChar).Value = objStockAsOfDate.itm_name;
                command.Parameters.Add("@P_STR_STYLE_SEARCH", SqlDbType.VarChar).Value = objStockAsOfDate.style_stearch;
                command.Parameters.Add("@P_STR_IB_Doc_ID", SqlDbType.VarChar).Value = objStockAsOfDate.ib_doc_id;
                command.Parameters.Add("@P_STR_Cont_ID", SqlDbType.VarChar).Value = objStockAsOfDate.cont_id;
                command.Parameters.Add("@P_STR_Lot_ID", SqlDbType.VarChar).Value = objStockAsOfDate.lot_id;
                command.Parameters.Add("@P_STR_Loc_ID", SqlDbType.VarChar).Value = objStockAsOfDate.loc_id;
                command.Parameters.Add("@P_STR_Ref_No", SqlDbType.VarChar).Value = objStockAsOfDate.grn_id;
                command.Parameters.Add("@P_STR_Whs_ID", SqlDbType.VarChar).Value = objStockAsOfDate.whs_id;
                connection.Open();
                dtInvAsOfDate.Load(command.ExecuteReader());

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return dtInvAsOfDate;
        }
        public StockAsOfDate GetStockAsOfDateDetailsRpt(StockAsOfDate objStockAsOfDate)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "SP_MVC_IV_STOCK_ASOFDATE_RPT";
                    IEnumerable<StockAsOfDate> ListstockInquiryLIST = connection.Query<StockAsOfDate>(storedProcedure1,
                        new
                        {
                            @P_STR_CMPID = objStockAsOfDate.cmp_id,
                            @P_STR_As_Of_Dt = objStockAsOfDate.As_Of_Date,
                            @P_STR_Style = objStockAsOfDate.itm_num,
                            @P_STR_Color = objStockAsOfDate.itm_color,
                            @P_STR_Size = objStockAsOfDate.itm_size,
                            @P_STR_Desc = objStockAsOfDate.itm_name,
                            P_STR_STYLE_SEARCH = objStockAsOfDate.style_stearch,
                            @P_STR_IB_Doc_ID = objStockAsOfDate.ib_doc_id,
                            @P_STR_Cont_ID = objStockAsOfDate.cont_id,
                            @P_STR_Lot_ID = objStockAsOfDate.lot_id,
                            @P_STR_Loc_ID = objStockAsOfDate.loc_id,
                            @P_STR_Ref_No = objStockAsOfDate.grn_id,
                            @P_STR_Whs_ID = objStockAsOfDate.whs_id,
                            
                        },
                        commandType: CommandType.StoredProcedure);
                    objStockAsOfDate.LstAsOfDateRpt = ListstockInquiryLIST.ToList();
                }

                return objStockAsOfDate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public StockAsOfDate GetStockAsOfDateDetailsRptExcel(StockAsOfDate objStockAsOfDate)//CR-3PL-MVC-180322-001 Added By NIthya
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "sp_iv_stl_as_of_dt_rpt_excel";
                    IEnumerable<StockAsOfDate> ListstockInquiryLIST = connection.Query<StockAsOfDate>(storedProcedure1,
                        new
                        {
                            @P_STR_CMPID = objStockAsOfDate.cmp_id,
                            @P_STR_Style = objStockAsOfDate.itm_num,
                            @P_STR_Color = objStockAsOfDate.itm_color,
                            @P_STR_Size = objStockAsOfDate.itm_size,
                            @P_STR_Desc = objStockAsOfDate.itm_name,
                            @P_STR_Status = objStockAsOfDate.status,
                            @P_STR_IB_Doc_ID = objStockAsOfDate.ib_doc_id,
                            @P_STR_Cont_ID = objStockAsOfDate.cont_id,
                            @P_STR_Lot_ID = objStockAsOfDate.lot_id,
                            @P_STR_Loc_ID = objStockAsOfDate.loc_id,
                            @P_STR_Ref_No = objStockAsOfDate.grn_id,
                            @P_STR_Whs_ID = objStockAsOfDate.whs_id,
                            @P_STR_As_Of_Dt = objStockAsOfDate.As_Of_Date,
                        },
                        commandType: CommandType.StoredProcedure);
                    objStockAsOfDate.LstAsOfDateRpt = ListstockInquiryLIST.ToList();
                }

                return objStockAsOfDate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public StockAsOfDate GetTotalCottonStockAsOfDateDetails(StockAsOfDate objStockAsOfDate)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "SP_MVC_IV_STOCK_ASOFDATE_RPT_CTN_TOTAL";
                    IEnumerable<StockAsOfDate> ListstockInquiryLIST = connection.Query<StockAsOfDate>(storedProcedure1,
                        new
                        {
                            @P_STR_CMPID = objStockAsOfDate.cmp_id,
                            @P_STR_As_Of_Dt = objStockAsOfDate.As_Of_Date,

                        },
                        commandType: CommandType.StoredProcedure);
                    objStockAsOfDate.ListCottonStockAsOfDateGrid = ListstockInquiryLIST.ToList();
                }

                return objStockAsOfDate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
    }
}
