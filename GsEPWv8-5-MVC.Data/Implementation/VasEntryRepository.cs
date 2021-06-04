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
    public class VasEntryRepository : IVasEntryRepository
    {

        public VasEntry GetVasEntryDetails(VasEntry objVasEntry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "proc_get_mvc_doc_inquiry_dtls";
                    IEnumerable<VasEntry> Listsample = connection.Query<VasEntry>(storedProcedure2,
             new
             {

                 @cust_id = objVasEntry.cust_id,
                 @vas_id = objVasEntry.vas_id,
                 @vas_date = objVasEntry.vas_date,
                 @status = objVasEntry.status,
                 @sr_num = objVasEntry.sr_num,
                 @fob = objVasEntry.fob,
                 @ship_to_id = objVasEntry.ship_to_id,
                 @cust_po = objVasEntry.cust_po,
                 @cust_po_dt = objVasEntry.cust_po_dt,
                 @header_note = objVasEntry.header_note,
                 @item_num = objVasEntry.item_num,
                 @item_desc = objVasEntry.item_desc,
                 @rate = objVasEntry.rate,
                 @amount = objVasEntry.amount,
                 @sts = objVasEntry.sts,
                 @note = objVasEntry.note,
                 @uom = objVasEntry.uom,


             },
            commandType: CommandType.StoredProcedure);
                    objVasEntry.LstInquiry = Listsample.ToList();

                    return objVasEntry;
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
