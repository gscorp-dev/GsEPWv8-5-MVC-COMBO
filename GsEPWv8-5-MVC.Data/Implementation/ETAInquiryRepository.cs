using Dapper;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Implementation
{
    public class ETAInquiryRepository: IETAInquiryRepository
    {
        public ETAInquiry GetInboundETAInquiryDetails(ETAInquiry objETAInquiry)
        {
            try
            {
                ETAInquiry objCustDtl = new ETAInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_GET_IB_LIST_BY_ETA_DT";
                    IList<ETAInquiry> ListIRFP = connection.Query<ETAInquiry>(SearchCustDtls, new
                    {
                        @P_STR_CMP_ID = objETAInquiry.cmp_id,
                        @P_DATE_ETA_DT_FM = objETAInquiry.ETA_dt_Fm,
                        @P_DATE_ETA_DT_TO = objETAInquiry.ETA_dt_To,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objETAInquiry.LstIBETAInqdetail = ListIRFP.ToList();
                }
                return objETAInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ETAInquiry GetInboundETAInquiryDetailsRpt(ETAInquiry objETAInquiry)
        {
            try
            {
                ETAInquiry objCustDtl = new ETAInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_GET_IB_LIST_BY_ETA_DT_RPT";
                    IList<ETAInquiry> ListIRFP = connection.Query<ETAInquiry>(SearchCustDtls, new
                    {
                        @P_STR_CMP_ID = objETAInquiry.cmp_id,
                        @P_DATE_ETA_DT_FM = objETAInquiry.ETA_dt_Fm,
                        @P_DATE_ETA_DT_TO = objETAInquiry.ETA_dt_To,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objETAInquiry.LstIBETAInqdetail = ListIRFP.ToList();
                }
                return objETAInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ETAInquiry GetInboundETAInquiryRpt(ETAInquiry objETAInquiry)
        {
            try
            {
                ETAInquiry objCustDtl = new ETAInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_GET_IB_LIST_BY_ETA_DT_DETAIL_RPT";
                    IList<ETAInquiry> ListIRFP = connection.Query<ETAInquiry>(SearchCustDtls, new
                    {
                        @P_STR_CMP_ID = objETAInquiry.cmp_id,
                        @P_DATE_ETA_DT_FM = objETAInquiry.ETA_dt_Fm,
                        @P_DATE_ETA_DT_TO = objETAInquiry.ETA_dt_To,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objETAInquiry.LstIBETAInqdetail = ListIRFP.ToList();
                }
                return objETAInquiry;
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
