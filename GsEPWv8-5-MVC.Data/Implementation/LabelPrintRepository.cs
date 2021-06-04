using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using GsEPWv8_5_MVC.Core.Entity;
using System.Data;
using GsEPWv8_5_MVC.Data.Interface;


namespace GsEPWv8_5_MVC.Data.Implementation
{
    public class LabelPrintRepository : ILabelPrintRepository
    {

     public   OBCtnLblPrnt GetOBLabelPrintDetails(OBCtnLblPrnt objOBCtnLblPrnt)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

            
                    const string storedProcedure2 = "sp_ob_get_ctn_lbl_prnt";
                    IEnumerable<OBCtnLblPrnt> LstCtnLabelPrint = connection.Query<OBCtnLblPrnt>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objOBCtnLblPrnt.cmp_id,
                            @p_str_so_num = objOBCtnLblPrnt.so_num,

                        },
                        commandType: CommandType.StoredProcedure);
                    objOBCtnLblPrnt.LstCtnLabelPrint = LstCtnLabelPrint.ToList();

                    const string spName = "spOBGetLblPrintStyleList";
                    IEnumerable<lblCtnStyle> ListlblCtnStyle = connection.Query<lblCtnStyle>(spName,
                        new
                        {
                            @p_str_cmp_id = objOBCtnLblPrnt.cmp_id,
                            @p_str_so_num = objOBCtnLblPrnt.so_num,

                        },
                        commandType: CommandType.StoredProcedure);
                    objOBCtnLblPrnt.ListlblCtnStyle = ListlblCtnStyle.ToList();

                    return objOBCtnLblPrnt;
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
