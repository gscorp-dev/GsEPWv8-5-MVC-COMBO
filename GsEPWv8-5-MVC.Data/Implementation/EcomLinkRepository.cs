using Dapper;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GsEPWv8_5_MVC.Data.Implementation
{
    public class EcomLinkRepository : IEcomLinkRepository
    {
        public ClsEcomLink fnGetCustEcomLinkDtl(string pstrCmpId)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    ClsEcomLink objEcomLink = new ClsEcomLink();

                    IEnumerable<ClsCustEcomLinkHdr> lstCustEcomLinkHdr = connection.Query<ClsCustEcomLinkHdr>("spGetCustEcomLinkDtl",
                         new
                         {
                             @pstrCompId = pstrCmpId
                         },
                         commandType: CommandType.StoredProcedure);
                    objEcomLink.lstCustEcomLinkHdr = lstCustEcomLinkHdr.ToList();

                    return objEcomLink;
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
    }
}
