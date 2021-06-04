using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GsEPWv8_5_MVC.Core.Entity;
using System.Data;
using GsEPWv8_5_MVC.Data.Interface;
using System.Configuration;
using System.Data.SqlClient;

namespace GsEPWv8_5_MVC.Data.Implementation
{
    public class CarrierRepository : ICarrierRepository
    {
        public Carrier fnGetCarrierList(string pstrCarrierId, string pstrCarrierName)
        {

            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    Carrier objCarrier = new Carrier();
                    const string storedProcedure2 = "spGetCarrierList";
                    IEnumerable<CarrierHdr> ListCarrierList = connection.Query<CarrierHdr>(storedProcedure2,
                        new
                        {
                            @pstrCarrierId = pstrCarrierId,
                            @pstrCarrierName = pstrCarrierName
                        },
                        commandType: CommandType.StoredProcedure);
                    objCarrier.lstCarrierList = ListCarrierList.ToList();

                    return objCarrier;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public bool fnSaveCarrierDetails(string pstrOption, string pstrCarrierId, CarrierHdr objCarrierHdr)

        {
            bool blnStatus = false;
            try {
                string connString = ConfigurationManager.ConnectionStrings["GensoftConnection"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "spSaveCarrer";
                        cmd.Parameters.Add("@pstrOption", SqlDbType.VarChar).Value = pstrOption;
                        cmd.Parameters.Add("@pstrCarrierId", SqlDbType.VarChar).Value = pstrCarrierId;
                        cmd.Parameters.Add("@pstrCarrierScacCode", SqlDbType.VarChar).Value = objCarrierHdr.carrier_scac_code;
                        cmd.Parameters.Add("@pstrCarrierName", SqlDbType.VarChar).Value = objCarrierHdr.carrier_name;
                        cmd.Parameters.Add("@pstrContactName", SqlDbType.VarChar).Value = objCarrierHdr.contact_name;
                        cmd.Parameters.Add("@pstrContactOfficeNum", SqlDbType.VarChar).Value = objCarrierHdr.contact_office_num;
                        cmd.Parameters.Add("@pstrContactCellNum", SqlDbType.VarChar).Value = objCarrierHdr.contact_cell_num;
                        cmd.Parameters.Add("@pstrContactEmail", SqlDbType.VarChar).Value = objCarrierHdr.contact_email;
                        cmd.Parameters.Add("@pstrCarrierAlertEmail", SqlDbType.VarChar).Value = objCarrierHdr.carrier_alert_email;
                        cmd.Parameters.Add("@pstrEnteredDt", SqlDbType.VarChar).Value = objCarrierHdr.entered_dt;
                        cmd.Parameters.Add("@pstrEnteredBy", SqlDbType.VarChar).Value = objCarrierHdr.entered_by;
                        cmd.Parameters.Add("@pstrUpdatedDt", SqlDbType.VarChar).Value = objCarrierHdr.updated_dt;
                        cmd.Parameters.Add("@pstrUpdatedBy", SqlDbType.VarChar).Value = objCarrierHdr.updated_by;
                        cmd.Parameters.Add("@pstrProcessId", SqlDbType.VarChar).Value = objCarrierHdr.process_id;
                        cmd.Connection = conn;
                        cmd.ExecuteNonQuery();
                    }
                }
                blnStatus = true;
            }
            catch (Exception ex)
            {
                blnStatus = false;
                throw ex;
            }
            finally
            {

            }
            return blnStatus;
        }
     }
    }


