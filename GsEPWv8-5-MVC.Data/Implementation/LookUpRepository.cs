using Dapper;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Interface;
using GsEPWv8_5_MVC.Data.Implementation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  GsEPWv8_5_MVC.Data.Implementation
{
    public class LookUpRepository : ILookUpRepository
    {
        public LookUp GetLookUpValue (LookUp objLookUp)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    LookUp objOrderLifeCycleCategory = new LookUp();

                    const string storedProcedure2 = "proc_get_mvcweb_ma_lookup";
                    IEnumerable<LookUp> ListLookUp = connection.Query<LookUp>(storedProcedure2,
                        new
                        {
                            @id = objLookUp.id,
                            @lookuptype = objLookUp.lookuptype,
                            
                        },
                        commandType: CommandType.StoredProcedure);
                    objLookUp.ListLookUpDtl = ListLookUp.ToList();

                    return objLookUp;
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
