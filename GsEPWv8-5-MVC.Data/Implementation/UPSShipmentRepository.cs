using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using GsEPWv8_5_MVC.Core.Entity;
using System.Data;
using GsEPWv8_5_MVC.Data.Interface;
using System.Configuration;
using System.Data.SqlClient;

namespace GsEPWv8_5_MVC.Data.Implementation
{
    public class UPSShipmentRepository : IUPSShipmentRepository
    {
        public UPSShipment GetUPSShipperDetails(string pstrCmpId, string pstrWhsId, string pstrSoNum, string pstrUserName, string pstrAccountId)
        {
            UPSShipment objUPSShipment = new UPSShipment();
            System.Data.DataTable tblShipper = null;
            System.Data.DataTable tblShipTo = null;
            System.Data.DataTable tblSoldTo = null;
            System.Data.DataTable tblShipFrom = null;
            System.Data.DataTable tblSrHdr = null;

            System.Data.SqlClient.SqlCommand sqlCommand = null;
            System.Data.SqlClient.SqlConnection sqlConn;
            System.Data.DataSet dataSet = null;
            System.Data.SqlClient.SqlDataAdapter adapter = null;
            UPSAddressType objUPSAddressType = null;
            try
            {
                sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                sqlConn.Open();

                sqlCommand = new System.Data.SqlClient.SqlCommand("spGetUPSAccountDetail", sqlConn);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 0;
                sqlCommand.Parameters.AddWithValue("@pstrCmpId", pstrCmpId);
                sqlCommand.Parameters.AddWithValue("@pstrWhsId", pstrWhsId);
                sqlCommand.Parameters.AddWithValue("@pstrSoNum", pstrSoNum);
                sqlCommand.Parameters.AddWithValue("@pstrUserName", pstrUserName);
                sqlCommand.Parameters.AddWithValue("@pstrAccountId", pstrAccountId);
                adapter = new System.Data.SqlClient.SqlDataAdapter(sqlCommand);
                dataSet = new System.Data.DataSet();
                adapter.Fill(dataSet);

                if (dataSet != null)
                {
                    if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        tblShipper = dataSet.Tables[0];
                        UPSGsSecurity objUPSSecurity = new UPSGsSecurity();

                        objUPSSecurity.Username = tblShipper.Rows[0]["Username"].ToString();
                        objUPSSecurity.Password = tblShipper.Rows[0]["Password"].ToString();
                        objUPSSecurity.AccessLicenseNumber = tblShipper.Rows[0]["AccessLicenseNumber"].ToString();
                        objUPSShipment.UPSGsSecurity = objUPSSecurity;

                        UPSShipper objUPSShipper = new UPSShipper();
                        objUPSShipper.ShipperNumber = tblShipper.Rows[0]["ShipperNumber"].ToString();


                        objUPSShipper.ShiperAttentionName = tblShipper.Rows[0]["ShiperAttentionName"].ToString();
                        objUPSShipper.ShiperName = tblShipper.Rows[0]["ShiperName"].ToString();
                        objUPSShipper.ShiperPhone = tblShipper.Rows[0]["ShiperPhone"].ToString();
                        objUPSAddressType = new UPSAddressType();
                        objUPSAddressType.AddressLine1 = tblShipper.Rows[0]["AddressLine1"].ToString();
                        objUPSAddressType.AddressLine2 = tblShipper.Rows[0]["AddressLine2"].ToString();
                        objUPSAddressType.City = tblShipper.Rows[0]["City"].ToString();
                        objUPSAddressType.StateProvinceCode = tblShipper.Rows[0]["StateProvinceCode"].ToString();

                        objUPSAddressType.CountryCode = tblShipper.Rows[0]["CountryCode"].ToString();
                        objUPSAddressType.PostalCode = tblShipper.Rows[0]["PostalCode"].ToString();

                        objUPSShipper.ShiperAddress = objUPSAddressType;
                        objUPSShipment.Shipper = objUPSShipper;
                    }

                    if (dataSet.Tables.Count > 1 && dataSet.Tables[1].Rows.Count > 0)
                    {
                        tblShipTo = dataSet.Tables[1];

                        UPSShipTo objUPSShipTo = new UPSShipTo();


                        objUPSShipTo.ShipToAttentionName = tblShipTo.Rows[0]["ShipToAttentionName"].ToString();
                        objUPSShipTo.ShipToName = tblShipTo.Rows[0]["ShipToName"].ToString();
                        objUPSAddressType = new UPSAddressType();
                        objUPSAddressType.AddressLine1 = tblShipTo.Rows[0]["AddressLine1"].ToString();
                        objUPSAddressType.AddressLine2 = tblShipTo.Rows[0]["AddressLine2"].ToString();
                        objUPSAddressType.City = tblShipTo.Rows[0]["City"].ToString();
                        objUPSAddressType.StateProvinceCode = tblShipTo.Rows[0]["StateProvinceCode"].ToString();
                        objUPSAddressType.CountryCode = tblShipTo.Rows[0]["CountryCode"].ToString();
                        objUPSAddressType.PostalCode = tblShipTo.Rows[0]["PostalCode"].ToString();
                        objUPSShipTo.ShipToPhone = tblShipTo.Rows[0]["ShipToPhone"].ToString();
                        objUPSShipTo.ShipToAddress = objUPSAddressType;
                        objUPSShipment.UPSShipTo = objUPSShipTo;

                    }
                    if (dataSet.Tables.Count > 2 && dataSet.Tables[2].Rows.Count > 0)
                    {
                        tblSoldTo = dataSet.Tables[2];

                        UPSSoldTo objUPSSoldTo = new UPSSoldTo();
                        objUPSSoldTo.SoldToAttentionName = tblSoldTo.Rows[0]["SoldToAttentionName"].ToString();
                        objUPSSoldTo.SoldToName = tblSoldTo.Rows[0]["SoldToName"].ToString();
                        objUPSAddressType = new UPSAddressType();
                        objUPSAddressType.AddressLine1 = tblSoldTo.Rows[0]["AddressLine1"].ToString();
                        objUPSAddressType.AddressLine2 = tblSoldTo.Rows[0]["AddressLine2"].ToString();
                        objUPSAddressType.City = tblSoldTo.Rows[0]["City"].ToString();
                        objUPSAddressType.StateProvinceCode = tblSoldTo.Rows[0]["StateProvinceCode"].ToString();
                        objUPSAddressType.CountryCode = tblSoldTo.Rows[0]["CountryCode"].ToString();
                        objUPSAddressType.PostalCode = tblSoldTo.Rows[0]["PostalCode"].ToString();
                        objUPSSoldTo.SoldToAddress = objUPSAddressType;
                        objUPSSoldTo.SoldToPhone = tblSoldTo.Rows[0]["SoldToPhone"].ToString();
                        objUPSShipment.UPSSoldTo = objUPSSoldTo;

                    }
                    if (dataSet.Tables.Count > 3 && dataSet.Tables[3].Rows.Count > 0)
                    {
                        tblShipFrom = dataSet.Tables[3];

                        UPSShipFrom objUPSShipFrom = new UPSShipFrom();
                        objUPSShipFrom.ShipFromAttentionName = tblShipFrom.Rows[0]["ShipFromAttentionName"].ToString();
                        objUPSShipFrom.ShipFromName = tblShipFrom.Rows[0]["ShipFromName"].ToString();
                        objUPSAddressType = new UPSAddressType();
                        objUPSAddressType.AddressLine1 = tblShipFrom.Rows[0]["AddressLine1"].ToString();
                        objUPSAddressType.AddressLine2 = tblShipFrom.Rows[0]["AddressLine2"].ToString();
                        objUPSAddressType.City = tblShipFrom.Rows[0]["City"].ToString();
                        objUPSAddressType.StateProvinceCode = tblShipFrom.Rows[0]["StateProvinceCode"].ToString();
                        objUPSAddressType.CountryCode = tblShipFrom.Rows[0]["CountryCode"].ToString();
                        objUPSAddressType.PostalCode = tblShipFrom.Rows[0]["PostalCode"].ToString();
                        objUPSShipFrom.ShipFromAddress = objUPSAddressType;

                        objUPSShipment.UPSShipFrom = objUPSShipFrom;

                    }
                    if (dataSet.Tables.Count > 4 && dataSet.Tables[4].Rows.Count > 0)
                    {
                        tblSrHdr = dataSet.Tables[4];
                        objUPSShipment.InvoiceNumber = tblSrHdr.Rows[0]["InvoiceNumber"].ToString();
                        objUPSShipment.InvoiceDate = tblSrHdr.Rows[0]["InvoiceDate"].ToString();
                        objUPSShipment.PurchaseOrderNumber = tblSrHdr.Rows[0]["PurchaseOrderNumber"].ToString();
                    }


                }

            }


            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }


            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "spGetShipOdrerDtl";
                    IEnumerable<shipOdrerDtl> ListShipOdrerDtl = connection.Query<shipOdrerDtl>(storedProcedure2,
                        new
                        {
                            @pstrCmpId = pstrCmpId,
                            @pstrSoNum = pstrSoNum,
                        },
                        commandType: CommandType.StoredProcedure);
                    objUPSShipment.OrderDetails = ListShipOdrerDtl.ToList();
                }

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "spGetUPSServiceDtl";
                    IEnumerable<UPSServiceType> ListUPSServiceType = connection.Query<UPSServiceType>(storedProcedure2,
                        new
                        {

                        },
                        commandType: CommandType.StoredProcedure);
                    objUPSShipment.UPSServiceTypeList = ListUPSServiceType.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

            return objUPSShipment;
        }
    }
}
