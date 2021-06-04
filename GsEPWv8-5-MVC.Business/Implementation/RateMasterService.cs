using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
   public class RateMasterService : IRateMasterService
    {
        IRateMasterRepository objRepository = new RateMasterRepository();
        public void RateMasterDelete(RateMaster objRateMaster)
        {
            objRepository.RateMasterDelete(objRateMaster);
        }
        public void RateMasterNew(RateMaster objRateMaster)
        {
            objRepository.RateMasterNew(objRateMaster);
        }
        public void RateMasterCreateUpdate(RateMaster objRateMaster)
        {
            objRepository.RateMasterCreateUpdate(objRateMaster);
        }
        public RateMaster GetRateMasterDetails(RateMaster objRateMaster)
        {
            return objRepository.GetRateMasterDetails(objRateMaster);
        }
        public RateMaster GetRateMasterRptDetails(RateMaster objRateMaster)
        {
            return objRepository.GetRateMasterRptDetails(objRateMaster);
        }
        public RateMaster GetRateMasterViewDetails(RateMaster objRateMaster)
        {
            return objRepository.GetRateMasterViewDetails(objRateMaster);
        }
        //CR_3PL_MVC_BL_2018_0220_001 Added By Ravi
        public LookUp GetRateMasterCategory(LookUp objRateMaster)
        {
            return objRepository.GetRateMasterCategory(objRateMaster);
        }
        //END
        //CR-180421-001 Added By Nithya
        public RateMaster ExistRate(RateMaster objRateMaster)
        {
            return objRepository.ExistRate(objRateMaster);
        }
        //END
        public RateMaster CHECK_RATEID_IS_IN_USE(RateMaster objRateMaster)
        {
            return objRepository.CHECK_RATEID_IS_IN_USE(objRateMaster);
        }
        public RateDtl GetRateDtlList(RateDtl objRateDtl, string p_str_cmp_id, string p_str_rate_type, string p_str_rate_id_fm, string p_str_rate_id_to)
        {
            return objRepository.GetRateDtlList(objRateDtl, p_str_cmp_id, p_str_rate_type, p_str_rate_id_fm, p_str_rate_id_to);
        }
        public DataTable GetRateDtlListExcel(string p_str_cmp_id, string p_str_rate_type, string p_str_rate_id_fm, string p_str_rate_id_to)
        {
            return objRepository.GetRateDtlListExcel(p_str_cmp_id, p_str_rate_type, p_str_rate_id_fm, p_str_rate_id_to);
        }
    }
}
