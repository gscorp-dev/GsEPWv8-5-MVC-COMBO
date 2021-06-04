using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GsEPWv8_5_MVC.Common
{
    public static class clsGlobal
    {
        static int _DispDateFrom;

        static String _AdjDocId;
        static String _DocItemCode;
        static string _IsCompanyUser;
        static string _UserType;
        public static IList<Login> _ListUserDetails = new List<Login>();
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
        public static string IsCompanyUser
        {
            get
            {
                return _IsCompanyUser;
            }

            set
            {
                _IsCompanyUser = value;
            }
        }
        public static string UserType
        {
            get
            {
                return _UserType;
            }

            set
            {
                _UserType = value;
            }
        }

        public static string DocItemCode
        {
            get
            {
                return _DocItemCode;
            }

            set
            {
                _DocItemCode = value;
            }
        }
        public static IList<Login> ListUserDetails
        {
            get
            {
                return _ListUserDetails;
            }

            set
            {
                _ListUserDetails = value;
            }
        }

    }
}