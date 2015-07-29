using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TestMVC4App.Models
{
    /// <summary>
    /// Database Connection used to know the OldIds to test.
    /// </summary>
    public class DatabaseFacade : IDisposable
    {
        private SqlConnection conn;
        private static string selectStatement = "SELECT OldId FROM [User]";
        private SqlCommand queryCommand;

        /// <summary>
        /// Establishes the connection to the database and retrieves the list of OldIds.
        /// </summary>
        /// <returns>List of OldIds.</returns>
        public HashSet<int> ConnectToDataSourceAndRetrieveOldIds(string version)
        {
            string connectionString = @"Server=tcp:yyyy.database.windows.net,1433;Database=xxxx-"+version+";User ID=wwwwwww;Encrypt=True;Connection Timeout=30;";
            conn = new SqlConnection(connectionString);
            queryCommand = new SqlCommand(selectStatement, conn);

            var OldIdList = new HashSet<int>();
            conn.Open();
            System.Diagnostics.Debug.WriteLine("Connection state is: " + conn.State.ToString());

            SqlDataReader sdr = queryCommand.ExecuteReader();

            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    OldIdList.Add(sdr.GetInt32(0));
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No rows found.");
            }

            sdr.Close();

            conn.Close();

            return OldIdList;
        }

        /// <summary>
        /// Releases Connection and Query objects.
        /// </summary>
        public void Dispose()
        {
            if (conn != null)
            {
                conn.Dispose();
                conn = null;
            }

            if (queryCommand != null)
            {
                queryCommand.Dispose();
                queryCommand = null;
            }
        }
    }
}