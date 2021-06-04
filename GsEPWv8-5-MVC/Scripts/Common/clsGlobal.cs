using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GsEPWv8_4_MVC.Common
{
    public static class clsGlobal
    {
        static int _DispDateFrom;

        static String _AdjDocId;

        public static int DispDateFrom
        {
            get
            {
                return _DispDateFrom;
            }

            set
            {
                _DispDateFrom = value;
            }
        }

        public static string AdjDocId
        {
            get
            {
                return _AdjDocId;
            }

            set
            {
                _AdjDocId = value;
            }
        }
    }
}