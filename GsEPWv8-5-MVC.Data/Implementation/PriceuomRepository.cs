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
    public class PriceuomRepository : IPriceuomRepository
    {
        public Price GetPriceuomDetails(Price objPriceuom)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    LookUp objOrderLifeCycleCategory = new LookUp();


                    const string storedProcedure2 = "proc_get_mvcweb_validate_price_uom";
                    IEnumerable<Priceuom> Listprice = connection.Query<Priceuom>(storedProcedure2,
                        new
                        {
                            @uom_type = objPriceuom.uom_type

                        },
                        commandType: CommandType.StoredProcedure);
                    objPriceuom.ListPriceuom = Listprice.ToList();

                    return objPriceuom;
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
