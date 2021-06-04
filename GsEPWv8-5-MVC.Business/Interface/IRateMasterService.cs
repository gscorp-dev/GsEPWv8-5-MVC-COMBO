using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface IRateMasterService
    {
        void RateMasterDelete(RateMaster objRateMaster);
        void RateMasterNew(RateMaster objRateMaster);
        void RateMasterCreateUpdate(RateMaster objRateMaster);
        RateMaster GetRateMasterDetails(RateMaster objRateMaster);
        RateMaster GetRateMasterRptDetails(RateMaster objRateMaster);
        RateMaster GetRateMasterViewDetails(RateMaster objRateMaster);
        //CR_3PL_MVC_BL_2018_0220_001 Added By Ravi
        LookUp GetRateMasterCategory(LookUp objRateMaster);
        //END
        RateMaster ExistRate(RateMaster objRateMaster);//CR-180421-001 Added By Nithya
        RateMaster CHECK_RATEID_IS_IN_USE(RateMaster objRateMaster);
        RateDtl GetRateDtlList(RateDtl objRateDtl, string p_str_cmp_id, string p_str_rate_type, string p_str_rate_id_fm, string p_str_rate_id_to);
        DataTable GetRateDtlListExcel(string p_str_cmp_id, string p_str_rate_type, string p_str_rate_id_fm, string p_str_rate_id_to);
    }
}
