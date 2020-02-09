using System;

namespace Web.Infrastructure
{
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    public class Database : IDisposable//HS - implement dispose pattern for closing connection automatically
    {
        private const string ConnectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=BrainWAre;Integrated Security=SSPI;AttachDBFilename=C:\\Users\\hstei\\Source\\Repos\\BrainWare\\Web\\App_Data\\BrainWare.mdf";
        private readonly SqlConnection _connection;

        public Database()
        {
            _connection = new SqlConnection(ConnectionString);
        }

        //HS - add SQLParameter array for bind variables, default to null, and CommandType, default to text
        public DbDataReader ExecuteReader(string query, CommandType cmdType = CommandType.Text, SqlParameter[] sqlParams = null)
        {
            var sqlQuery = new SqlCommand(query, _connection)
            {
                CommandType = cmdType
            };
            if (sqlParams != null)
            {
                sqlQuery.Parameters.AddRange(sqlParams);
            }
            if(_connection.State != ConnectionState.Open)//HS - only open connection when necessary
            {
                _connection.Open();
            }
            return sqlQuery.ExecuteReader();
        }

        //HS - close connection on Dispose if opened
        public void Dispose()
        {
            if(_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}