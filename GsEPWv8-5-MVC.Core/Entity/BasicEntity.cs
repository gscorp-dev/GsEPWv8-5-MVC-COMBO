using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  GsEPWv8_5_MVC.Core.Entity
{
    public class Menu
    {
        public string vModule { get; set; }
        public string vMenu { get; set; }
        public string vSubMenu { get; set; }
        public string vUrl { get; set; }
        public IList<Menu> LstMenu { get; set; }
    }
    public class BasicEntity
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int UserGroupID { get; set; }
        public string UserGroup { get; set; }
        public int UserRoleID { get; set; }
        public string UserRole { get; set; }

        //Paging
        public string RecordsPerPage { get; set; }
        public string CurrentPageNo { get; set; }
        public string RowCount { get; set; }
        public string TotalRecordCount { get; set; }
        public string OrderBy { get; set; }
        public string PageFrom { get; set; }
        public int RowNumber { get; set; }

        public string Result { get; set; }
        public string ResultMessage { get; set; }
    }
}
