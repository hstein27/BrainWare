using System;

namespace Web.Infrastructure
{
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    public class Database : IDisposable//HS - implement dispose pattern for closing connection automatically
    {
        private readonly SqlConnection _connection;

        public Database()
        {
            _connection = new SqlConnection("Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=BrainWAre;Integrated Security=SSPI;AttachDBFilename=C:\\Users\\hstei\\Source\\Repos\\BrainWare\\Web\\App_Data\\BrainWare.mdf");

            _connection.Open();
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
            return sqlQuery.ExecuteReader();
        }

        //HS - unused method. Would delete completely but left here commented out to show I saw this.
        //public int ExecuteNonQuery(string query)
        //{
        //    var sqlQuery = new SqlCommand(query, _connection);

        //    return sqlQuery.ExecuteNonQuery();
        //}

        //HS - close connection on Dispose if opened
        public void Dispose()
        {
            if(_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}