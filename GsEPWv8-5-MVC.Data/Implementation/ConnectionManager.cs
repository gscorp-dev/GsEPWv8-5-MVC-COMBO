using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace  GsEPWv8_5_MVC.Data.Implementation
{

    public static class ConnectionManager
    {
        //public static string ConnectionString = WebConfigurationManager.ConnectionStrings["WellnessDB"].ConnectionString;        
        public static IDbConnection OpenConnection()
        {

            string _ConnectionName = string.Empty;

            _ConnectionName = HttpContext.Current.Session["ConnectionName"].ToString();
            //string l_str_cmp_id = HttpContext.Current.Session["g_str_cmp_id"].ToString().Trim();
            //if (l_str_cmp_id == "CIG")
            //{
            //    _ConnectionName = "Saramax";
            //}
          
            //if(_ConnectionName == "Saramax")
            //{
            //    IDbConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["Saramax"].ConnectionString);
            //    connection.Open();
            //    return connection;
            //}
            //else if (_ConnectionName == "FreightHorse")
            //{
            //    IDbConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["FreightHorse"].ConnectionString);
            //    connection.Open();
            //    return connection;
            //} 
            if (_ConnectionName != "")
            {
                IDbConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings[_ConnectionName].ConnectionString);
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
