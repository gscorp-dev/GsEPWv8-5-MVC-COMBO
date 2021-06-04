using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace  GsEPWv8_4_MVC.Data.Implementation
{

    public static class ConnectionManager
    {
        //public static string ConnectionString = WebConfigurationManager.ConnectionStrings["WellnessDB"].ConnectionString;        
        public static IDbConnection OpenConnection()
        {

            string _users = string.Empty;

            _users = HttpContext.Current.Session["isanother"].ToString();
            if(_users == "yes")
            {
                IDbConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["GenSoftConnection1"].ConnectionString);
                connection.Open();
                return connection;
            }
            else
            {
                IDbConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["GenSoftConnection"].ConnectionString);
                connection.Open();
                return connection;
            }

          
            
         
          
        }

        //public static IDbConnection OpenConnection1()
        //{
        //    IDbConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["CFME"].ConnectionString);
        //    connection.Open();
        //    return connection;
        //}
    }
}
