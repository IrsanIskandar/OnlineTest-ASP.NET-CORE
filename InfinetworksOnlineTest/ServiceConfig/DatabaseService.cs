using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace InfinetworksOnlineTest.ServiceConfig
{
    public static class DatabaseConnection
    {
        // Property Database Service MYSQL
        public static MySqlConnection existingConnection;
        private static MySqlConnection ConnectionSql => existingConnection ?? (existingConnection = GetSqlConnection());


        public static MySqlConnection GetSqlConnection()
        {
            MySqlConnection connection = new MySqlConnection(Constant.connectionString);
            connection.Open();

            return connection;
        }
    }

    public class DatabaseService<T>
    {
        public async static Task ExecuteNoReturn(string spName, object param = null)
        {
            await DatabaseConnection.GetSqlConnection().ExecuteAsync(sql: spName, param: param, commandType: System.Data.CommandType.StoredProcedure, commandTimeout: 30);
            await DatabaseConnection.GetSqlConnection().CloseAsync();
        }

        public async static Task<bool> ExecuteNoReturn(string queryText, string status = "", object param = null)
        {
            bool result = Convert.ToBoolean(await DatabaseConnection.GetSqlConnection().ExecuteAsync(sql: queryText, param: param, commandType: System.Data.CommandType.Text, commandTimeout: 30));

            if (result == true)
            {
                await DatabaseConnection.GetSqlConnection().CloseAsync();
            }

            return result;
        }

        public async static Task<T> ExecuteSingleAsync(string spName, object param = null)
        {
            T result = await DatabaseConnection.GetSqlConnection().QueryFirstOrDefaultAsync<T>(sql: spName, param: param, commandType: System.Data.CommandType.StoredProcedure);
            DatabaseConnection.GetSqlConnection().Close();
            return result;
        }

        public async static Task<IEnumerable<T>> ExecuteAsync(string spName, object param = null)
        {
            // return empty object when query returns no rows
            IEnumerable<T> result = new List<T>();

            result = await DatabaseConnection.GetSqlConnection().QueryAsync<T>(sql: spName, param: param, commandType: System.Data.CommandType.StoredProcedure);
            if (DatabaseConnection.GetSqlConnection().State == System.Data.ConnectionState.Open)
            {
                DatabaseConnection.GetSqlConnection().Close();
            }
            return result;
        }
    }

    public class DatabaseService
    {
        public async static Task<MySqlCommand> Execute(string spname, MySqlParameter[] parameters = null)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = DatabaseConnection.GetSqlConnection();
            command.CommandText = spname;
            command.CommandTimeout = 30;
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            if(DatabaseConnection.GetSqlConnection().State == System.Data.ConnectionState.Closed)
            {
                await DatabaseConnection.GetSqlConnection().OpenAsync();
            }

            return command;
        }
    }
}
