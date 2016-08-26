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
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (DataBroker db = new DataBroker(connectionString))
            {
                List<SqlParameter> sqlParams = new List<SqlParameter>();

                sqlParams.Add(new SqlParameter { ParameterName = "@id", Value = "1234" });

                DataTable dt = db.GetDataTable("Select * from foo", sqlParams, CommandType.Text);
            }

    


        }
    }
}
