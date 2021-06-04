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

//CR-20180217 Added By Meera for add  Storage bill by Pallet
//CR-3PL_MVC_IB_2018_0219_004 - Add a new column Bill and it should be visible once the Receiving is posted. By clicking the Bill link system should generate In&Out bill for the specific IB DOC ID and the status of the Bill column should be changed as Bill Posted 

namespace GsEPWv8_5_MVC.Data.Implementation
{
    public class BillingInquiryRepository : IBillingInquiryRepository
    {
        public BillingInquiry GetBillingInquiryDetails(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    var cmp_id = objBillingInquiry.cmp_id;
                    var l_str_Bill_type = objBillingInquiry.Bill_type;
                    var tmp_str_Bill_type = String.Empty;
                    if (l_str_Bill_type == "ALL")
                    {
                        tmp_str_Bill_type = "";
                    }
                    else
                    {
                        tmp_str_Bill_type = l_str_Bill_type;
                    }

                    BillingInquiry objOrderLifeCycleCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_billing_inq";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Cmp_id = "",
                            @Cust_id = objBillingInquiry.cmp_id,
                            @Bill_doc_id = objBillingInquiry.Bill_doc_id,
                            @Bill_type = tmp_str_Bill_type,
                            @Bill_doc_dt_Fr = objBillingInquiry.Bill_doc_dt_Fr,
                            @Bill_doc_dt_To = objBillingInquiry.Bill_doc_dt_To,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingInquiry = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetBillingSummaryRpt(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    var cmp_id = objBillingInquiry.cmp_id;
                    var l_str_Bill_type = objBillingInquiry.bill_type;
                    var tmp_str_Bill_type = String.Empty;
                    if (l_str_Bill_type == "ALL")
                    {
                        tmp_str_Bill_type = "";
                    }
                    else
                    {
                        tmp_str_Bill_type = l_str_Bill_type;
                    }

                    BillingInquiry objOrderLifeCycleCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_billing_grid_summary_rpt";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Cmp_id = "",
                            @Cust_id = objBillingInquiry.cmp_id,
                            @Bill_doc_id = objBillingInquiry.Bill_doc_id,
                            @Bill_type = tmp_str_Bill_type,
                            @Bill_doc_dt_Fr = objBillingInquiry.Bill_doc_dt_Fr,
                            @Bill_doc_dt_To = objBillingInquiry.Bill_doc_dt_To,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingSummaryRpt = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetBillingInvoiceRpt(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_billing_grid_invoice_rpt";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Cust_id = objBillingInquiry.cmp_id,
                            @Bill_doc_id = objBillingInquiry.Bill_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingInvoiceRpt = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetBillingBillingType(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_get_bill_type";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Cust_id = objBillingInquiry.cmp_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingType = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetBillingInoutType(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_get_bill_inout_type";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Cust_id = objBillingInquiry.cmp_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingInoutType = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetBillingBillDocIdType(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();
                    string l_str_bill_doc_id = string.Empty;
                    l_str_bill_doc_id = objBillingInquiry.Bill_doc_id;

                    const string storedProcedure2 = "proc_get_mvcweb_get_bill_doc_id_type";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Cust_id = objBillingInquiry.cmp_id,
                            @bill_doc_id = l_str_bill_doc_id
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingDocIdType = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetBillingBillDocVASRpt(BillingInquiry objBillingInquiry)
        {
            try
            {
                string l_str_cmp_id = string.Empty;
                l_str_cmp_id = objBillingInquiry.cust_of_cmpid.Trim();
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();
                    if (l_str_cmp_id == "FHNJ" && objBillingInquiry.cmp_id.Trim() == "SJOE")
                    {



                        const string storedProcedure2 = "SP_MVC_BL_DOC_NORM_RPT_FOR_FH_NJ";
                        IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                            new
                            {
                                @Cust_id = objBillingInquiry.cmp_id,
                                @Bill_doc_id = objBillingInquiry.Bill_doc_id,
                                @Image_path = objBillingInquiry.Image_Path


                            },
                            commandType: CommandType.StoredProcedure);
                        objBillingInquiry.ListBillingDocVASRpt = ListBilling.ToList();

                        //}
                        //else
                        //{
                        //    const string storedProcedure2 = "proc_get_mvcweb_billing_doc_vas_norm_rpt";
                        //    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        //        new
                        //        {
                        //            @Cust_id = objBillingInquiry.cmp_id,
                        //            @Bill_doc_id = objBillingInquiry.Bill_doc_id,

                        //        },
                        //        commandType: CommandType.StoredProcedure);
                        //    objBillingInquiry.ListBillingDocVASRpt = ListBilling.ToList();

                        //}
                    }
                    else
                    {
                        const string storedProcedure2 = "proc_get_mvcweb_billing_doc_vas_norm_rpt";
                        IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                            new
                            {
                                @Cust_id = objBillingInquiry.cmp_id,
                                @Bill_doc_id = objBillingInquiry.Bill_doc_id,

                            },
                            commandType: CommandType.StoredProcedure);
                        objBillingInquiry.ListBillingDocVASRpt = ListBilling.ToList();
                    }


                    return objBillingInquiry;
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
        public BillingInquiry GetBillingBillInoutCartonRpt(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_billing_inout_bill_doc_rpt";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Cust_id = objBillingInquiry.cmp_id,
                            @Bill_doc_id = objBillingInquiry.Bill_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingInoutCartonRpt = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetBillingBillamountInoutCartonRpt(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_billamount_inout_bill_doc_rpt";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Cust_id = objBillingInquiry.cmp_id,
                            @Bill_doc_id = objBillingInquiry.Bill_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingamountInoutCartonRpt = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetBillingBillInoutCartonInstrgRpt(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_billing_inout_bill_doc_with_initstrg_rpt";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Cust_id = objBillingInquiry.cmp_id,
                            @Bill_doc_id = objBillingInquiry.Bill_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingInoutCartonInstrgRpt = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetBillingBillInoutCubeInstrgRpt(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_billing_inout_bill_doc_bycube_with_initstrg_rpt";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Cust_id = objBillingInquiry.cmp_id,
                            @Bill_doc_id = objBillingInquiry.Bill_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingInoutCubeInstrgRpt = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetBillingBillInoutCubeRpt(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_billing_inout_bill_doc_bycube_rpt";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Cust_id = objBillingInquiry.cmp_id,
                            @Bill_doc_id = objBillingInquiry.Bill_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingInoutCubeRpt = ListBilling.ToList();

                    return objBillingInquiry;
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

        public BillingInquiry GetBillingBillDocSTRGRpt(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_billing_doc_Storage_carton_withintl_strg_rpt";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Cust_id = objBillingInquiry.cmp_id,
                            @Bill_doc_id = objBillingInquiry.Bill_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetBillingHdr(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_webmvc_Billing_Receive_hdr";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objBillingInquiry.cust_id,
                            @billdocId = objBillingInquiry.Bill_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingdetail = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetBillingBillDocCubeSTRGRpt(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_billing_doc_Storage_cube_withintl_strg_rpt";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Cust_id = objBillingInquiry.cmp_id,
                            @Bill_doc_id = objBillingInquiry.Bill_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingDocSTRGCubewithinitRpt = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetBillingdtl(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_webmvc_Billing_receive_details";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objBillingInquiry.cust_id,
                            @billdoc_id = objBillingInquiry.Bill_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingdetail = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetBillingInvStaus(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_mvcget_bill_status";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @cust_id = objBillingInquiry.cmp_id,
                            @Bill_doc_id = objBillingInquiry.Bill_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingInvStatus = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetBillingRcvdDetails(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_strg_grid_dtls";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objBillingInquiry.cmp_id,
                            @cust_id = objBillingInquiry.cust_id,
                            // @bill_pd_fm = objBillingInquiry.bill_pd_fm.ToString(),
                            @bill_as_of_date = objBillingInquiry.bill_pd_to.ToString(),
                            @bill_type = objBillingInquiry.bill_type,
                            @bill_doc_id = objBillingInquiry.bill_doc_id //CR-3PL_MVC_IB_2018_0219_004 

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillRcvdDetails = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetBillingVasBillDetails(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_vas_bill_dtls";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objBillingInquiry.cmp_id,//CR20180718-001 Added By Nithya
                            //@cust_id = objBillingInquiry.cust_id,
                            //@bill_pd_fm = objBillingInquiry.bill_pd_fm.ToString(),
                            //@bill_as_of_date = objBillingInquiry.bill_pd_to.ToString(),
                            //@bill_type = objBillingInquiry.bill_type,
                            @bill_doc_id = objBillingInquiry.bill_doc_id //CR-3PL_MVC_IB_2018_0219_004 

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillRcvdDetails = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GenerateSTRGBill(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_generate_storage_bill";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id,
                            @p_str_cust_id = objBillingInquiry.cust_id,
                            @p_dt_bill_print_dt = objBillingInquiry.print_bill_date,
                            @p_str_bill_as_of_date = objBillingInquiry.bill_as_of_date,
                            @p_str_bill_doc_id = objBillingInquiry.bill_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListSaveSTRGBillDetails = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GenerateSTRGBillByCube(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_generate_storage_bill_Cube";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id,
                            @p_str_cust_id = objBillingInquiry.cust_id,
                            @p_dt_bill_print_dt = objBillingInquiry.print_bill_date,
                            @p_str_bill_as_of_date = objBillingInquiry.bill_as_of_date,
                            @p_str_bill_doc_id = objBillingInquiry.bill_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListSaveSTRGBillDetails = ListBilling.ToList();

                    return objBillingInquiry;
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

        public BillingInquiry CheckSTRGBillDocIdExisting(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_Check_Existing_bill_doc_id";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id,
                            @p_str_cust_id = objBillingInquiry.cust_id,
                            @p_dt_bill_print_dt = objBillingInquiry.bill_pd_fm,
                            @p_str_bill_as_of_date = objBillingInquiry.bill_as_of_date,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListCheckExistingSTRGBillDocId = ListBilling.ToList();
                    if (objBillingInquiry.ListCheckExistingSTRGBillDocId.Count == 0)
                    {
                        objBillingInquiry.Check_existing_bill_doc_id = "";
                    }
                    //else
                    //{
                    //    objBillingInquiry.Check_existing_bill_doc_id = "1";
                    //}
                    return objBillingInquiry;
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
        public BillingInquiry DeleteExistingSTRGBillDocIdData(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_delete_Existing_bill_doc_id_data";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id,
                            @p_str_cust_id = objBillingInquiry.cust_id,
                            @p_bill_doc_id = objBillingInquiry.bill_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListSaveSTRGBillDetails = ListBilling.ToList();

                    return objBillingInquiry;
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
        //CR-3PL_MVC_IB_2018_0219_004 
        public BillingInquiry CheckInoutBillDocIdExisting(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_Check_Existing_Inout_bill_doc_id";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id.Trim(),
                            @p_str_cust_id = objBillingInquiry.cust_id,
                            @p_str_bill_pd_fm = objBillingInquiry.inout_bill_pd_fm,
                            @p_str_bill_pd_to = objBillingInquiry.inout_bill_pd_to
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListCheckExistingInOutBillDocId = ListBilling.ToList();
                    if (objBillingInquiry.ListCheckExistingInOutBillDocId.Count == 0)
                    {
                        objBillingInquiry.Check_existing_bill_doc_id = "";
                    }
                    else
                    {
                        objBillingInquiry.Check_existing_bill_doc_id = "1";
                    }
                    return objBillingInquiry;
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
        //CR-3PL_MVC_IB_2018_0219_004
        public BillingInquiry GenerateInOutBillByCarton(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "Sp_generate_inout_bill";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id,
                            @p_str_cust_id = objBillingInquiry.cust_id,
                            @p_str_bill_as_of_dt = objBillingInquiry.bill_as_of_date,
                            @p_str_print_dt = objBillingInquiry.print_bill_date,
                            @p_str_palet_dt_fm = objBillingInquiry.inout_bill_pd_fm,
                            @p_str_palet_dt_to = objBillingInquiry.inout_bill_pd_to,
                            @p_str_bill_doc_id = objBillingInquiry.bill_doc_id,
                            @p_str_InitStrgRtReq = objBillingInquiry.init_strg_rt_req,
                            @p_str_BillFor = objBillingInquiry.BillFor,
                            @p_str_ib_doc_id = objBillingInquiry.ib_doc_id,
                            @p_temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListSaveSTRGBillDetails = ListBilling.ToList();

                    return objBillingInquiry;
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

        public BillingInquiry GenerateInOutBillByCube(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "Sp_generate_inout_bill";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id,
                            @p_str_cust_id = objBillingInquiry.cust_id,
                            @p_str_bill_as_of_dt = objBillingInquiry.bill_as_of_date,
                            @p_str_print_dt = objBillingInquiry.print_bill_date,
                            @p_str_palet_dt_fm = objBillingInquiry.inout_bill_pd_fm,
                            @p_str_palet_dt_to = objBillingInquiry.inout_bill_pd_to,
                            @p_str_bill_doc_id = objBillingInquiry.bill_doc_id,
                            @p_str_InitStrgRtReq = objBillingInquiry.init_strg_rt_req,
                            @p_str_BillFor = objBillingInquiry.BillFor,
                            @p_str_ib_doc_id = objBillingInquiry.ib_doc_id,
                            @p_temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListSaveSTRGBillDetails = ListBilling.ToList();

                    return objBillingInquiry;
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
        //CR-3PL_MVC_IB_2018_0219_004
        public BillingInquiry CheckVASBillDocIdExisting(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_check_existing_vas_bill_doc_id";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id.Trim(),
                            @p_str_cust_id = objBillingInquiry.cust_id,
                            @p_str_bill_pd_fm = objBillingInquiry.vas_bill_pd_fm,
                            @p_str_bill_pd_to = objBillingInquiry.vas_bill_pd_to
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListCheckExistingVASBillDocId = ListBilling.ToList();
                    if (objBillingInquiry.ListCheckExistingVASBillDocId.Count == 0)
                    {
                        objBillingInquiry.Check_existing_bill_doc_id = "";
                    }
                    return objBillingInquiry;
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

        public BillingInquiry GenerateVASBill(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();
                    //@p_str_bill_doc_id = objBillingInquiry.bill_doc_id,
                    const string storedProcedure2 = "proc_get_mvcweb_generate_vas_bill";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id,
                            @p_str_cust_id = objBillingInquiry.cust_id,
                            @p_str_bill_as_of_dt = objBillingInquiry.bill_as_of_dt,
                            @p_dt_bill_print_dt = objBillingInquiry.print_bill_date,//CR-180421-001 Added By Nithya
                            @p_str_bill_date_fm = objBillingInquiry.vas_bill_pd_fm,
                            @p_str_bill_date_to = objBillingInquiry.vas_bill_pd_to,

                            @p_str_bill_doc_id = objBillingInquiry.bill_doc_id,
                            @p_str_ship_doc_id = objBillingInquiry.ship_doc_id,
                            @p_temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListSaveVASBillDetails = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetBillDelete(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_DELETE_BILL";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objBillingInquiry.cmp_id,
                            @bill_doc_id = objBillingInquiry.Bill_doc_id,
                            @p_str_cust_id = objBillingInquiry.CUSTId
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListSaveVASBillDetails = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetBillPost(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "sp_tbl_bl_hdr_post";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objBillingInquiry.cmp_id,
                            @bill_doc_id = objBillingInquiry.Bill_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListSaveVASBillDetails = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetStrgBillExcel(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_bl_storage_bl_rpt";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Invnum = objBillingInquiry.Bill_doc_id,
                            @CmpId = "FHC",

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingStrgExcel = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetStrgBillCubeExcel(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_bl_strg_doc_cube_rpt";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Invnum = objBillingInquiry.Bill_doc_id,
                            @CmpId = "FHC",

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingStrgExcel = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetNormBillExcel(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_bl_doc_nor_excel";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Invnum = objBillingInquiry.Bill_doc_id,
                            @CmpId = "FHC",

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingStrgExcel = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetInOutBillCube(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_bl_Inout_cart_Doc_excel";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Invnum = objBillingInquiry.Bill_doc_id

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingStrgExcel = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetInOutBillCarton(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_bl_Inout_carton_doc_excel";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Invnum = objBillingInquiry.Bill_doc_id,
                            @CmpId = "FHC",

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingStrgExcel = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GenerateSTRGBillByPallet(BillingInquiry objBillingInquiry)// CR-20180217 -Pallet

        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_GENERATE_STRG_BILL_BY_PALLET";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id,
                            @p_str_cust_id = objBillingInquiry.cust_id,
                            @p_str_print_dt = objBillingInquiry.print_bill_date,
                            @p_str_bill_dt_fm = objBillingInquiry.print_bill_date,
                            @p_str_bill_dt_to = objBillingInquiry.bill_as_of_date,
                            @p_str_bill_doc_id = objBillingInquiry.bill_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListGenBillingStrgByPallet = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GenerateInoutBillByContainer(BillingInquiry objBillingInquiry)// CR-20180217-Container
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_GENERATE_IO_BILL_BY_CNTR";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id.Trim(),
                            @p_str_cust_id = objBillingInquiry.cust_id,
                            @p_str_bill_as_of_dt = objBillingInquiry.bill_as_of_date,
                            @p_str_print_dt = objBillingInquiry.print_bill_date,
                            @p_str_rcvd_dt_fm = objBillingInquiry.inout_bill_pd_fm,
                            @p_str_rcvd_dt_to = objBillingInquiry.inout_bill_pd_to,
                            @p_str_bill_doc_id = objBillingInquiry.bill_doc_id,
                            @p_str_ib_doc_id = objBillingInquiry.ib_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListGenBillingInoutByContainer = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry CheckExsistBLDocID(BillingInquiry objBillingInquiry)// CR-3PL_MVC_IB_2018_0219_004
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_CHECK_EXIST_DOC_ID";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id.Trim(),
                            @p_str_cust_id = objBillingInquiry.cust_id,
                            @p_str_ib_doc_id = objBillingInquiry.ib_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListCheckExistingInOutBillDocId = ListBilling.ToList();
                    if (objBillingInquiry.ListCheckExistingInOutBillDocId.Count == 0)
                    {
                        objBillingInquiry.Check_existing_bill_doc_id = "";
                    }
                    else
                    {
                        objBillingInquiry.Check_existing_bill_doc_id = "1";
                    }

                    return objBillingInquiry;
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

        public BillingInquiry GetBillingBillDocPalletSTRGRpt(BillingInquiry objBillingInquiry)// CR-20180217-Container
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_GET_STRG_BILL_BY_PALLET_RPT";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Cust_id = objBillingInquiry.cmp_id,
                            @Bill_doc_id = objBillingInquiry.Bill_doc_id,
                            @Image_path = objBillingInquiry.Image_Path,
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListGenBillingStrgByPalletRpt = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetBillingBillDocContainerInoutRpt(BillingInquiry objBillingInquiry)// CR-20180217-Container
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_GET_INOUT_BILL_BY_CNTR_RPT";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Cust_id = objBillingInquiry.cmp_id,
                            @Bill_doc_id = objBillingInquiry.Bill_doc_id,
                            @Image_path = objBillingInquiry.Image_Path,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListGenBillingInoutByContainerRpt = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetDocRcvdDate(BillingInquiry objBillingInquiry)// CR-3PL_MVC_IB_2018_0219_004
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_GET_RCVD_DT";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objBillingInquiry.cust_id,
                            @ib_doc_id = objBillingInquiry.ib_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.LstDocRcvdDt = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry CheckExsistBLDocIDFromLotHdr(BillingInquiry objBillingInquiry)// CR-3PL_MVC_IB_2018_0219_004
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_CHECK_EXIST_BILL_DOC_ID_FROM_LOT_HDR";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cust_id,
                            @p_str_ib_doc_id = objBillingInquiry.ib_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListCheckExistingInOutBillDocId = ListBilling.ToList();
                    if (objBillingInquiry.ListCheckExistingInOutBillDocId.Count == 0)
                    {
                        objBillingInquiry.Check_existing_bill_doc_id = "";
                    }
                    else
                    {
                        if (objBillingInquiry.ListCheckExistingInOutBillDocId[0].bill_doc_id == null)
                        {
                            objBillingInquiry.Check_existing_bill_doc_id = "";
                        }
                        else
                        {
                            objBillingInquiry.Check_existing_bill_doc_id = "1";
                        }
                    }

                    return objBillingInquiry;
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
        public BillingInquiry CheckExsistBLDocIDFromVasHdr(BillingInquiry objBillingInquiry)// CR-3PL_MVC_IB_2018_0219_004
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_CHECK_EXIST_BILL_DOC_ID_FROM_VAS_HDR";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cust_id,
                            @p_str_ship_doc_id = objBillingInquiry.ship_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListCheckExistingVasBillDocId = ListBilling.ToList();
                    if (objBillingInquiry.ListCheckExistingVasBillDocId.Count == 0)
                    {
                        objBillingInquiry.Check_existing_bill_doc_id = "";
                    }
                    else
                    {
                        objBillingInquiry.Check_existing_bill_doc_id = "1";
                    }

                    return objBillingInquiry;
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
        public BillingInquiry GetSecondSTRGRate(BillingInquiry objBillingInquiry)     //CR_3PL_MVC_BL_2018_0303_001 Added By MEERA 03-03-2018
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_GET_SECOND_STRG_RATE";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objBillingInquiry.cmp_id,
                            @P_STR_ITM_NUM = objBillingInquiry.itm_num,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListGetSecondSTRGRate = ListBilling.ToList();
                    if (objBillingInquiry.ListGetSecondSTRGRate.Count == 0)
                    {
                        objBillingInquiry.sec_strg_rate = "";
                    }
                    else
                    {
                        objBillingInquiry.sec_strg_rate = "1";
                    }

                    return objBillingInquiry;
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
        public BillingInquiry GetBillDeleteByPallet(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_DELETE_BILL_BY_PALLET";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objBillingInquiry.cmp_id,
                            @bill_doc_id = objBillingInquiry.Bill_doc_id,
                            @p_str_cust_id = objBillingInquiry.CUSTId
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListSaveVASBillDetails = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetSTRGBillRcvdDtlByPallet(BillingInquiry objBillingInquiry)// CR-20180217-Container
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_GET_STRG_BILL_DETAILS_BY_PALLET";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Cust_id = objBillingInquiry.cmp_id,
                            @Bill_doc_id = objBillingInquiry.Bill_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListGetSTRGBillByPallet = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetSTRGBillRcvdDtlByContainer(BillingInquiry objBillingInquiry)// CR-20180217-Container
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_GET_INOUT_BILL_DETAILS_BY_CNTR";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Cust_id = objBillingInquiry.cmp_id,
                            @Bill_doc_id = objBillingInquiry.Bill_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListGetInoutBillByContainer = ListBilling.ToList();

                    return objBillingInquiry;
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
        //CR-3PL_MVC_IB_2018-03-10-001
        public BillingInquiry GetBillingInoutRcvdDetails(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_Inout_grid_dtls";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objBillingInquiry.cmp_id,
                            @cust_id = objBillingInquiry.cust_id,
                            //@bill_pd_fm = objBillingInquiry.bill_pd_fm.ToString(),
                            //@bill_as_of_date = objBillingInquiry.bill_pd_to.ToString(),
                            @bill_type = objBillingInquiry.bill_type,
                            @bill_doc_id = objBillingInquiry.bill_doc_id

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillRcvdDetails = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetBillingStrgRcvdDetails(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_GET_IV_STRG_GRID_DETAILS";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objBillingInquiry.cmp_id,
                            @cust_id = objBillingInquiry.cust_id,
                            @bill_as_of_date = objBillingInquiry.bill_pd_to.ToString(),
                            @bill_type = objBillingInquiry.bill_type,
                            @bill_doc_id = objBillingInquiry.bill_doc_id

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillRcvdDetails = ListBilling.ToList();

                    return objBillingInquiry;
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
        //CR-3PL_MVC_BL_2018_00312_001      
        public BillingInquiry GenerateSTRGBillCarton(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_GENERATE_STRG_BILL_BY_CARTON";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id,
                            @p_str_cust_id = objBillingInquiry.cust_id,
                            @p_dt_bill_print_dt = objBillingInquiry.print_bill_date,
                            @p_str_bill_as_of_date = objBillingInquiry.bill_as_of_date,
                            @p_str_bill_doc_id = objBillingInquiry.bill_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListSaveSTRGBillDetails = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GenerateSTRGBillCube(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_GENERATE_STRG_BILL_BY_CUBE";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id,
                            @p_str_cust_id = objBillingInquiry.cust_id,
                            @p_dt_bill_print_dt = objBillingInquiry.print_bill_date,
                            @p_str_bill_as_of_date = objBillingInquiry.bill_as_of_date,
                            @p_str_bill_doc_id = objBillingInquiry.bill_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListSaveSTRGBillDetails = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GenerateSTRGBillByLoc(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_GENERATE_STRG_BILL_BY_LOC";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id,
                            @p_str_cust_id = objBillingInquiry.cust_id,
                            @p_dt_bill_print_dt = objBillingInquiry.print_bill_date,
                            @p_str_bill_as_of_date = objBillingInquiry.bill_as_of_date,
                            @p_str_bill_doc_id = objBillingInquiry.bill_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListGetSTRGBillByLoc = ListBilling.ToList();

                    return objBillingInquiry;
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

        public BillingInquiry STRGBillLocationRpt(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_STRG_BILL_DOC_BU_LOC_RPT";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_CmpId = objBillingInquiry.cmp_id,
                            @p_str_Invnum = objBillingInquiry.bill_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListGetSTRGBillByLocRpt = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetVASBillDelete(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_DELETE_VAS_BILL";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objBillingInquiry.cmp_id,
                            @bill_doc_id = objBillingInquiry.Bill_doc_id,
                            @p_str_cust_id = objBillingInquiry.CUSTId
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListSaveVASBillDetails = ListBilling.ToList();

                    return objBillingInquiry;
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

        public bool DeleteVASBill(string p_str_bill_cmp_id,string p_str_cust_id, string p_str_bill_doc_id)
        {
            bool blnStatus = false;
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    BillingInquiry objBillingCategory = new BillingInquiry();
                    const string storedProcedure2 = "spDeleteVASBill";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_bill_cmp_id = p_str_bill_cmp_id,
                            @p_str_cust_id = p_str_cust_id,
                            @p_str_bill_doc_id = p_str_bill_doc_id
                        },
                        commandType: CommandType.StoredProcedure);
                    blnStatus = true;
                }
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

        //CR-2018-05-21-001 Added By Nithya
        public BillingInquiry STRGBillPcsRpt(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_STRG_BILL_DOC_BU_PCS_RPT";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_CmpId = objBillingInquiry.cmp_id,
                            @p_str_Invnum = objBillingInquiry.bill_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListGetSTRGBillByPcsRpt = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GenerateSTRGBillByPcs(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_GENERATE_STRG_BILL_BY_PCS";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id,
                            @p_str_cust_id = objBillingInquiry.cust_id,
                            @p_dt_bill_print_dt = objBillingInquiry.print_bill_date,
                            @p_str_bill_as_of_date = objBillingInquiry.bill_as_of_date,
                            @p_str_bill_doc_id = objBillingInquiry.bill_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListGetSTRGBillByLoc = ListBilling.ToList();

                    return objBillingInquiry;
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
        //END
        public BillingInquiry GetBillingVasBillInqDetails(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_GET_VAS_INQ";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objBillingInquiry.cmp_id,
                            @bill_sts = objBillingInquiry.bill_status, //CR-3PL_MVC_IB_2018_0219_004 
                            @bill_dt_fm = objBillingInquiry.vas_bill_pd_fm,
                            @bill_dt_to = objBillingInquiry.vas_bill_pd_to
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillRcvdDetails = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry UpdateVASHDR(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_UPDATE_VAS_HDR";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objBillingInquiry.cust_id,
                            @bill_doc_id = objBillingInquiry.bill_doc_id,
                            @ship_doc_id = objBillingInquiry.ship_doc_id //CR-3PL_MVC_IB_2018_0219_004 

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillRcvdDetails = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetBillingiNOUTBillInqDetails(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_GET_INOUT_INQUIRY";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objBillingInquiry.cmp_id,
                            @bill_sts = objBillingInquiry.bill_status, //CR-3PL_MVC_IB_2018_0219_004 
                            @ib_doc_dt_fm = objBillingInquiry.bill_pd_fm,
                            @ib_doc_dt_to = objBillingInquiry.bill_pd_to
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillRcvdDetails = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetConsolidateVASInqDetails(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_FETCH_OPEN_BILLS";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id,
                            @p_dt_bill_from_dt = objBillingInquiry.bill_pd_fm,
                            @p_dt_bill_to_dt = objBillingInquiry.bill_pd_to
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillRcvdDetails = ListBilling.ToList();

                    return objBillingInquiry;
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

        public List<ClsBillVAS> getVASBillRegenerate(string pstrCmpId,  string pstrBillDocId, string pdtBillFromDt, string pdtBillToDt)
        {
            List<ClsBillVAS> lstVASBillList = new List<ClsBillVAS>();
            try
            {
                using (IDbConnection sqlConn = ConnectionManager.OpenConnection())
                {
                    List<ClsBillVAS> lstBillVASBill = sqlConn.Query<ClsBillVAS>("spGetVASBillRegenerate", new
                    {
                        @pstrCmpId = pstrCmpId,
                        @pstrBillDocId = pstrBillDocId,
                        @pdtBillFromDt = pdtBillFromDt,
                        @pdtBillToDt = pdtBillToDt
                    }, commandType: CommandType.StoredProcedure).ToList();
                    lstVASBillList = lstBillVASBill.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return lstVASBillList;
        }

        public BillingInquiry getVASBillRegenerate(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_FETCH_OPEN_BILLS";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id,
                            @p_dt_bill_from_dt = objBillingInquiry.bill_pd_fm,
                            @p_dt_bill_to_dt = objBillingInquiry.bill_pd_to
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillRcvdDetails = ListBilling.ToList();

                    return objBillingInquiry;
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

        public BillingInquiry GetBillingBillConsolidateRpt(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_billing_Consolidated_rpt";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Cust_id = objBillingInquiry.cmp_id,
                            @Bill_doc_id = objBillingInquiry.Bill_doc_id,
                            @Bill_type = objBillingInquiry.Bill_type,
                            @Bill_doc_dt_Fr = objBillingInquiry.Bill_doc_dt_Fr,
                            @Bill_doc_dt_To = objBillingInquiry.Bill_doc_dt_To,


                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetConsolidateVASBillEntityValue(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_TEMP_BILL_DOC_ID";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @temp_bill_doc_id = ""
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingDocSTRGCartonwithinitRpt = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry SaveConsolidateBillDetails(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_INSERT_BL_TEMP_BILL_DOC_ID";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id,
                            @cmp_id = objBillingInquiry.cust_id,
                            @bill_type = objBillingInquiry.bill_type,
                            @ship_doc_id = objBillingInquiry.ship_doc_id
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.LstTempBilldetails = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry SaveConsolidateInoutBillDetails(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_INSERT_BL_TEMP_BILL_LOT_ID";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id,
                            @cmp_id = objBillingInquiry.cust_id,
                            @bill_type = objBillingInquiry.bill_type,
                            @lot_id = objBillingInquiry.lot_id
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.LstTempBilldetails = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetConsolidateStorageBillDetails(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_FETCH_OPEN_STRG_BILLS";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id,
                            @p_str_cust_id = objBillingInquiry.cust_id,
                            @p_dt_bill_as_of_dt = objBillingInquiry.bill_as_of_date,
                            @p_dt_bill_to_dt = objBillingInquiry.bill_pd_to
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListGetSTRGBillByPcsRpt = ListBilling.ToList();

                    return objBillingInquiry;
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

        public string CkBillDocAlreadyExists(string p_str_cust_id, string p_str_bill_type, string p_str_bill_from_dt, string p_str_bill_to_dt)
        {
            string p_str_bill_doc_id = string.Empty;

            try
            {
                int l_int_cntr_id_count = 0;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_bl_bill_doc_already_exists", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cust_id", SqlDbType.VarChar).Value = p_str_cust_id;
                command.Parameters.Add("@p_str_bill_type", SqlDbType.VarChar).Value = p_str_bill_type;
                command.Parameters.Add("@p_str_bill_from_dt", SqlDbType.VarChar).Value = p_str_bill_from_dt;
                command.Parameters.Add("@p_str_bill_to_dt", SqlDbType.VarChar).Value = p_str_bill_to_dt;
                connection.Open();
                if (command.ExecuteScalar() == null)
                {
                    return string.Empty;

                }
                else
                {
                    p_str_bill_doc_id = command.ExecuteScalar().ToString();
                    return p_str_bill_doc_id;

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


        public BillingInquiry GetConsolidateInoutBillDetails(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_FETCH_OPEN_INOUT_BILLS";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id,
                            @p_str_cust_id = objBillingInquiry.cust_id,
                            @p_dt_bill_from_dt = objBillingInquiry.bill_pd_fm,
                            @p_dt_bill_to_dt = objBillingInquiry.bill_pd_to
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListSaveSTRGBillDetails = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetRegenerateInoutBillDetails(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "sp_bl_get_regenerate_inout_bill_dtl";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id,
                            @p_str_cust_id = objBillingInquiry.cust_id,
                            @p_dt_bill_from_dt = objBillingInquiry.bill_pd_fm,
                            @p_dt_bill_to_dt = objBillingInquiry.bill_pd_to,
                            @p_str_bill_doc_id = objBillingInquiry.BillDocId
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListSaveSTRGBillDetails = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry ConsolidateInoutBillSummaryRpt(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_FETCH_OPEN_INOUT_BILLS_SUMMARY_RPT";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id,
                            @p_str_cust_id = objBillingInquiry.cust_id,
                            @p_dt_bill_from_dt = objBillingInquiry.bill_pd_fm,
                            @p_dt_bill_to_dt = objBillingInquiry.bill_pd_to
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListSaveSTRGBillDetails = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry ConsolidateInoutBillDetailRpt(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_FETCH_OPEN_INOUT_BILLS_DETAIL_RPT";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id,
                            @p_str_cust_id = objBillingInquiry.cust_id,
                            @p_dt_bill_from_dt = objBillingInquiry.bill_pd_fm,
                            @p_dt_bill_to_dt = objBillingInquiry.bill_pd_to,
                            @p_temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListSaveSTRGBillDetails = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry ConsolidateStorageBillSummaryRpt(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_FETCH_OPEN_STRG_BILL_SUMMARY_RPT";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id,
                            @p_str_cust_id = objBillingInquiry.cust_id,
                            @p_dt_bill_as_of_dt = objBillingInquiry.bill_as_of_date,
                            @p_dt_bill_to_dt = objBillingInquiry.bill_pd_to
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListGetSTRGBillByPcsRpt = ListBilling.ToList();

                    return objBillingInquiry;
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

        public BillingInquiry GetConsolidateVASSummaryRpt(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_MVC_BL_FETCH_OPEN_BILL_SUMMARY_RPT";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id,
                            @p_dt_bill_from_dt = objBillingInquiry.bill_pd_fm,
                            @p_dt_bill_to_dt = objBillingInquiry.bill_pd_to
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillRcvdDetails = ListBilling.ToList();
                    return objBillingInquiry;
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
        public BillingInquiry GenerateVASBillDetailRpt(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();
                    const string storedProcedure2 = "SP_MVC_BL_FETCH_OPEN_VAS_BILL_DETAIL_RPT";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objBillingInquiry.cmp_id,
                            @p_str_cust_id = objBillingInquiry.cust_id,
                            @p_temp_bill_doc_id = objBillingInquiry.temp_bill_doc_id
                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListSaveVASBillDetails = ListBilling.ToList();

                    return objBillingInquiry;
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
        public BillingInquiry GetConsolidtedVASBillRpt(BillingInquiry objBillingInquiry)
        {
            try
            {
                string l_str_cmp_id = string.Empty;
                l_str_cmp_id = objBillingInquiry.cust_of_cmpid.Trim();
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();
                    if (l_str_cmp_id == "FHNJ" && objBillingInquiry.cmp_id.Trim() == "SJOE")
                    {
                        const string storedProcedure2 = "SP_MVC_BL_DOC_NORM_RPT_FOR_FH_NJ";
                        IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                            new
                            {
                                @Cust_id = objBillingInquiry.cmp_id,
                                @Bill_doc_id = objBillingInquiry.Bill_doc_id,
                                @Image_path = objBillingInquiry.Image_Path


                            },
                            commandType: CommandType.StoredProcedure);
                        objBillingInquiry.ListBillingDocVASRpt = ListBilling.ToList();
                    }
                    else
                    {
                        const string storedProcedure2 = "SP_MVC_GET_CONSOLIDATED_VAS_BILL_RPT";
                        IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                            new
                            {
                                @Cust_id = objBillingInquiry.cmp_id,
                                @Bill_doc_id = objBillingInquiry.Bill_doc_id,
                                @Bill_type = objBillingInquiry.Bill_type,
                                @Bill_doc_dt_Fr = objBillingInquiry.Bill_doc_dt_Fr,
                                @Bill_doc_dt_To = objBillingInquiry.Bill_doc_dt_To,

                            },
                            commandType: CommandType.StoredProcedure);
                        objBillingInquiry.ListBillingDocVASRpt = ListBilling.ToList();
                    }


                    return objBillingInquiry;
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

        public BillingInquiry GetInvoiceRpt(BillingInquiry objBillingInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    BillingInquiry objBillingCategory = new BillingInquiry();

                    const string storedProcedure2 = "SP_BL_billing_invoice_rpt";
                    IEnumerable<BillingInquiry> ListBilling = connection.Query<BillingInquiry>(storedProcedure2,
                        new
                        {
                            @Cust_id = objBillingInquiry.cmp_id,
                            @Bill_doc_id = objBillingInquiry.Bill_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objBillingInquiry.ListBillingInvoiceRpt = ListBilling.ToList();

                    return objBillingInquiry;
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

        public DataTable GetBillByCubeList(string p_str_cmp_id, string p_str_bill_doc_id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_bl_inout_bill_by_cube_cmpr", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@p_str_bill_doc_id", SqlDbType.VarChar).Value = p_str_bill_doc_id;
                connection.Open();
                DataTable dtBill = new DataTable();
                dtBill.Load(command.ExecuteReader());
                return dtBill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }


        public DataTable GetConsolidtedVASBillList(string p_str_cmp_id, string p_str_bill_doc_id, string p_str_Bill_type, string p_str_doc_dt_Fr, string p_str_doc_dt_To)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("proc_get_mvcweb_billing_Consolidated_rpt", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@Cust_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@Bill_doc_id", SqlDbType.VarChar).Value = p_str_bill_doc_id;
                command.Parameters.Add("@Bill_type", SqlDbType.VarChar).Value = p_str_Bill_type;
                command.Parameters.Add("@Bill_doc_dt_Fr", SqlDbType.VarChar).Value = p_str_doc_dt_Fr;
                command.Parameters.Add("@Bill_doc_dt_To", SqlDbType.VarChar).Value = p_str_doc_dt_To;
                connection.Open();
                DataTable dtBill = new DataTable();
                dtBill.Load(command.ExecuteReader());
                return dtBill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public DataTable GetConsolidtedINOUTBillList(string p_str_cmp_id, string p_str_bill_doc_id, string p_str_Bill_type, string p_str_doc_dt_Fr, string p_str_doc_dt_To)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("proc_get_mvcweb_billing_Consolidated_rpt", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@Cust_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@Bill_doc_id", SqlDbType.VarChar).Value = p_str_bill_doc_id;
                command.Parameters.Add("@Bill_type", SqlDbType.VarChar).Value = p_str_Bill_type;
                command.Parameters.Add("@Bill_doc_dt_Fr", SqlDbType.VarChar).Value = p_str_doc_dt_Fr;
                command.Parameters.Add("@Bill_doc_dt_To", SqlDbType.VarChar).Value = p_str_doc_dt_To;
                connection.Open();
                DataTable dtBill = new DataTable();
                dtBill.Load(command.ExecuteReader());
                return dtBill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public DataTable GetGridSummaryBillList(string p_str_cmp_id, string p_str_bill_doc_id, string p_str_Bill_type, string p_str_doc_dt_Fr, string p_str_doc_dt_To)
        {
            try
            {
                    var cmp_id = p_str_cmp_id;
                    var l_str_Bill_type = p_str_Bill_type;
                    var tmp_str_Cmp_id = "";
                    var tmp_str_Bill_type = "";

                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                
                command = new SqlCommand("proc_get_mvcweb_billing_grid_summary_rpt", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@Cmp_id", SqlDbType.VarChar).Value = tmp_str_Cmp_id;
                command.Parameters.Add("@Cust_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@Bill_doc_id", SqlDbType.VarChar).Value = p_str_bill_doc_id;
                command.Parameters.Add("@Bill_type", SqlDbType.VarChar).Value = tmp_str_Bill_type;
                command.Parameters.Add("@Bill_doc_dt_Fr", SqlDbType.VarChar).Value = p_str_doc_dt_Fr;
                command.Parameters.Add("@Bill_doc_dt_To", SqlDbType.VarChar).Value = p_str_doc_dt_To;
                connection.Open();
                DataTable dtBill = new DataTable();
                dtBill.Load(command.ExecuteReader());
                return dtBill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public DataTable GetBillDocVASRpt(string p_str_cmp_id, string p_str_bill_doc_id, ref DataTable dtBlHdr)
        {
            try
            {
                System.Data.DataSet dataSet = null;
                System.Data.SqlClient.SqlDataAdapter adapter = null;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_bl_get_vas_bill", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cust_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@p_str_bill_doc_id", SqlDbType.VarChar).Value = p_str_bill_doc_id;
                connection.Open();
                DataTable dtBill = new DataTable();
                adapter = new System.Data.SqlClient.SqlDataAdapter(command);
                dataSet = new System.Data.DataSet();
                adapter.Fill(dataSet);
                if (dataSet != null)
                {
                    if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        dtBill = dataSet.Tables[0];
                        dtBlHdr = dataSet.Tables[1];
                    }
                }
                return dtBill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public string GetBillType(string p_str_bill_doc_id)
        {

            try
            {
                string p_str_bill_type = string.Empty;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_bl_get_bill_type", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_bill_doc_id", SqlDbType.VarChar).Value = p_str_bill_doc_id;

                connection.Open();
                p_str_bill_type = Convert.ToString(command.ExecuteScalar());

                if (p_str_bill_type != string.Empty)
                {
                    return p_str_bill_type;
                }
                else
                {
                    return p_str_bill_type;
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


        public string GetBillBy(string p_str_cust_id, string p_str_bill_type)
        {

            try
            {
                string p_str_bill_by = string.Empty;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_get_bill_by_cust", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cust_id", SqlDbType.VarChar).Value = p_str_cust_id;
                command.Parameters.Add("@p_str_bill_type", SqlDbType.VarChar).Value = p_str_bill_type;

                connection.Open();
                p_str_bill_by = Convert.ToString(command.ExecuteScalar());

                if (p_str_bill_type != string.Empty)
                {
                    return p_str_bill_by;
                }
                else
                {
                    return string.Empty;
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

        public DataTable GetBillDocInoutRpt(string p_str_cmp_id, string p_str_bill_doc_id, ref DataTable dtBlHdr)
        {
            try
            {
                System.Data.DataSet dataSet = null;
                System.Data.SqlClient.SqlDataAdapter adapter = null;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("proc_get_mvcweb_billing_inout_bill_doc_bycube_rpt", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@Cust_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@Bill_doc_id", SqlDbType.VarChar).Value = p_str_bill_doc_id;
                connection.Open();
                DataTable dtBill = new DataTable();
                adapter = new System.Data.SqlClient.SqlDataAdapter(command);
                dataSet = new System.Data.DataSet();
                adapter.Fill(dataSet);

                if (dataSet != null)
                {
                    if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        dtBill = dataSet.Tables[0];
                        dtBlHdr = dataSet.Tables[1];
                    }
                }
                return dtBill;
            }
            finally
            {

            }
        }


        public DataTable GetBillDocStrgRpt(string p_str_cust_id, string p_str_bill_doc_id, ref DataTable dtBlHdr)
        {
            try
            {
                System.Data.DataSet dataSet = null;
                System.Data.SqlClient.SqlDataAdapter adapter = null;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                DataTable dtBill = new DataTable();
                command = new SqlCommand("sp_bl_get_strg_bill_by_cube", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cust_id", SqlDbType.VarChar).Value = p_str_cust_id;
                command.Parameters.Add("@p_str_bill_doc_id", SqlDbType.VarChar).Value = p_str_bill_doc_id;
                connection.Open();
                adapter = new System.Data.SqlClient.SqlDataAdapter(command);
                dataSet = new System.Data.DataSet();
                adapter.Fill(dataSet);

                if (dataSet != null)
                {
                    if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        dtBill = dataSet.Tables[0];
                        dtBlHdr = dataSet.Tables[1];
                    }
                }
                return dtBill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public DataTable fnGetStrgBillByItmSmry(string p_str_cust_id, string p_str_bill_doc_id, ref DataTable dtBlHdr)
        {
            try
            {
                System.Data.DataSet dataSet = null;
                System.Data.SqlClient.SqlDataAdapter adapter = null;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                DataTable dtBill = new DataTable();
                command = new SqlCommand("spGetStrgBillByItmSmry", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cust_id", SqlDbType.VarChar).Value = p_str_cust_id;
                command.Parameters.Add("@p_str_bill_doc_id", SqlDbType.VarChar).Value = p_str_bill_doc_id;
                connection.Open();
                adapter = new System.Data.SqlClient.SqlDataAdapter(command);
                dataSet = new System.Data.DataSet();
                adapter.Fill(dataSet);

                if (dataSet != null)
                {
                    if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        dtBill = dataSet.Tables[0];
                        dtBlHdr = dataSet.Tables[1];
                    }
                }
                return dtBill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
     
        public DataTable GetBillInvoiceInoutRpt(string p_str_cmp_id, string p_str_bill_doc_id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("proc_get_mvcweb_billing_inout_bill_doc_rpt", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@cust_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@bill_doc_id", SqlDbType.VarChar).Value = p_str_bill_doc_id;
                connection.Open();
                DataTable dtBill = new DataTable();
                dtBill.Load(command.ExecuteReader());
                return dtBill; 
            }
            finally
            {

            }
        }
        public DataTable GetBillInvoiceStorageRpt(string p_str_cmp_id, string p_str_bill_doc_id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();

                command = new SqlCommand("SP_BL_billing_invoice_rpt", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@Cust_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@Bill_doc_id", SqlDbType.VarChar).Value = p_str_bill_doc_id;
                connection.Open();
                DataTable dtBill = new DataTable();
                dtBill.Load(command.ExecuteReader());
                return dtBill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public DataTable GetBillInvoiceVASRpt(string p_str_cmp_id, string p_str_bill_doc_id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();

                command = new SqlCommand("proc_get_mvcweb_billing_doc_vas_norm_rpt", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@Cust_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@Bill_doc_id", SqlDbType.VarChar).Value = p_str_bill_doc_id;
                connection.Open();
                DataTable dtBill = new DataTable();
                dtBill.Load(command.ExecuteReader());
                return dtBill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public DataTable fnGetCtnsByBillDoc( string pstrBillDocId)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();

                command = new SqlCommand("spGetCtnsByBillDoc", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@pstrBillDocId", SqlDbType.VarChar).Value = pstrBillDocId;
                connection.Open();
                DataTable dtCtns = new DataTable();
                dtCtns.Load(command.ExecuteReader());
                return dtCtns;
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
