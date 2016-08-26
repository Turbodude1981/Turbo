using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data; 

namespace Compliance.DataAccess
{
    public class DataBroker : IDisposable
    {
        
        public string ConnectionString {
            get
            {
                return m_ConnectionString;
            }
            set { }

        }
        private string m_ConnectionString;
        private SqlConnection m_Connection; 
        
        public DataBroker(string connectionString)
        {

            m_ConnectionString = connectionString;
            m_Connection = new SqlConnection(m_ConnectionString);
            _OpenConnection();
        }

        public T GetScalar<T>(string sql, List<SqlParameter> sqlParams = null, CommandType cmdType = CommandType.Text)
        {
            SqlCommand cmd = _CreateCommand(sql, sqlParams, cmdType);
            object scalarVal = cmd.ExecuteScalar();

            return (T) scalarVal;

        }

        public SqlDataReader GetDataReader(string sql, List<SqlParameter> sqlParams = null, CommandType cmdType = CommandType.Text)
        {
            SqlCommand cmd = _CreateCommand(sql, sqlParams, cmdType);
            return cmd.ExecuteReader();

        }

        private SqlCommand _CreateCommand(string sql, List<SqlParameter> sqlParams, CommandType cmdType)
        {
            SqlCommand cmd = m_Connection.CreateCommand();

            cmd.CommandText = sql;
            cmd.CommandType = cmdType;

            if (sqlParams != null)
            {
                sqlParams.ForEach(p =>
                {
                    cmd.Parameters.Add( new SqlParameter { ParameterName = p.ParameterName, Value = p.Value });
                });
            }

            return cmd;
        }

        public DataTable GetDataTable(string queryTextOrSPName, List<SqlParameter> sqlParams = null, CommandType cmdType = CommandType.Text)
        {
            SqlCommand cmd = _CreateCommand(queryTextOrSPName, sqlParams, cmdType);
            DataTable dt = new DataTable();
            dt.Load(GetDataReader(queryTextOrSPName, sqlParams, cmdType));
            return dt;
        }

        public void ExecuteQuery(string sql, List<SqlParameter> sqlParams = null, CommandType cmdType = CommandType.Text)
        {
            SqlCommand cmd = _CreateCommand(sql, sqlParams, cmdType);
            cmd.ExecuteNonQuery();
        }

        #region Private 
        private void _OpenConnection()
        {
            if (m_Connection.State != ConnectionState.Open)
            {
                m_Connection.Open();
            }
            
        }

        
        #endregion


        #region IDisposable impl
        public void Dispose()
        {
            if (m_Connection.State == ConnectionState.Open)
            {
                m_Connection.Close();
            }

            
            m_Connection = null;
        }
        #endregion
    }
}
