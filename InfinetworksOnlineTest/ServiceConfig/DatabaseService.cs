using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace InfinetworksOnlineTest.ServiceConfig
{
    public class DatabaseService<T>
    {
        // Property Database Service MYSQL
        private static MySqlConnection existingConnection;
        private static MySqlConnection ConnectionSql => existingConnection ?? (existingConnection = GetSqlConnection());


        private static MySqlConnection GetSqlConnection()
        {
            MySqlConnection connection = new MySqlConnection(Constant.connectionString);
            connection.Open();

            return connection;
        }

        public async static Task ExecuteNoReturn(string spName, object param = null)
        {
            await ConnectionSql.ExecuteAsync(sql: spName, param: param, commandType: System.Data.CommandType.StoredProcedure, commandTimeout: 30);
            await ConnectionSql.CloseAsync();
        }

        public async static Task ExecuteNoReturn(string queryText, bool status, object param = null)
        {
            if (status == true)
            {
                await ConnectionSql.ExecuteAsync(sql: queryText, param: param, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                await ConnectionSql.CloseAsync();
            }
            else
            {
                await ConnectionSql.CloseAsync();
            }
        }

        public async static Task<T> ExecuteSingleAsync(string spName, object param = null)
        {
            T result = await ConnectionSql.QueryFirstOrDefaultAsync<T>(sql: spName, param: param, commandType: System.Data.CommandType.StoredProcedure);
            ConnectionSql.Close();
            return result;
        }

        public async static Task<IEnumerable<T>> ExecuteAsync(string spName, object param = null)
        {
            // return empty object when query returns no rows
            IEnumerable<T> result = new List<T>();

            result = await existingConnection.QueryAsync<T>(sql: spName, param: param, commandType: System.Data.CommandType.StoredProcedure);
            if (ConnectionSql.State == System.Data.ConnectionState.Open)
            {
                ConnectionSql.Close();
            }
            return result;
        }
    }
}
