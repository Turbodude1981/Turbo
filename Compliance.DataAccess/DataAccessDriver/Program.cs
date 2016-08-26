using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compliance.DataAccess;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessDriver
{
    class Program
    {

        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;

            DataTable dt1 = _GetDataTableByQuery(connectionString);
            DataTable dt2 = _GetDataTableBySP(connectionString);

            SqlDataReader reader1 = _GetDataReaderBySP(connectionString);
            SqlDataReader reader2 = _GetDataReaderByQuery(connectionString);

            int id = _GetScalar(connectionString);
             
          

    


        }

        private static DataTable _GetDataTableByQuery(string connectionString)
        {
            DataTable dt;
            using (DataBroker db = new DataBroker(connectionString))
            {
               

                dt = db.GetDataTable("Select * from [dbo].[things]" );


            }

            return dt;
        }

        private static DataTable _GetDataTableBySP(string connectionString)
        {
            DataTable dt;
            using (DataBroker db = new DataBroker(connectionString))
            {
                List<SqlParameter> sqlParams = new List<SqlParameter>();

                sqlParams.Add(new SqlParameter { ParameterName = "@thingId", Value = "1" });

                dt = db.GetDataTable("[dbo].[usp_GetThings]", sqlParams, CommandType.StoredProcedure);


            }

            return dt;
        }


        private static SqlDataReader _GetDataReaderByQuery(string connectionString)
        {
            SqlDataReader dr;
            using (DataBroker db = new DataBroker(connectionString))
            {


                dr = db.GetDataReader("Select * from [dbo].[things]");


            }

            return dr;
        }

        private static SqlDataReader _GetDataReaderBySP(string connectionString)
        {
            SqlDataReader reader;
            using (DataBroker db = new DataBroker(connectionString))
            {
                List<SqlParameter> sqlParams = new List<SqlParameter>();

                sqlParams.Add(new SqlParameter { ParameterName = "@thingId", Value = "1" });

               reader = db.GetDataReader("[dbo].[usp_GetThings]", sqlParams, CommandType.StoredProcedure);


            }

            return reader;
        }

        private static int _GetScalar(string connectionString)
        {
            int id; 
            using (DataBroker db = new DataBroker(connectionString))
            {
               
                id = db.GetScalar<int>("SELECT MAX(id) FROM [dbo].[Things]");


            }

            return id;
        }
    }
}
