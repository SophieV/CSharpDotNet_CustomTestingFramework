using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TestMVC4App.Models
{
    /// <summary>
    /// Database Connection used to know the UPIs to test.
    /// </summary>
    public class DatabaseFacade : IDisposable
    {
        private static string connectionString = @"Server=tcp:le9rmjfn5q.database.windows.net,1433;Database=yfmps-entities-a13;User ID=slamTHEdbNOince@le9rmjfn5q;Password=allQUIETallsWELL104;Encrypt=True;Connection Timeout=30;";
        private static string selectStatement = "SELECT Upi FROM [User]";
        private static SqlConnection conn = new SqlConnection(connectionString);
        SqlCommand queryCommand = new SqlCommand(selectStatement, conn);

        /// <summary>
        /// Establishes the connection to the database and retrieves the list of UPIs.
        /// </summary>
        /// <returns>List of UPIs.</returns>
        public HashSet<int> ConnectToDataSourceAndRetrieveUPIs()
        {
            var upiList = new HashSet<int>();
            conn.Open();
            System.Diagnostics.Debug.WriteLine("Connection state is: " + conn.State.ToString());

            SqlDataReader sdr = queryCommand.ExecuteReader();

            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    upiList.Add(sdr.GetInt32(0));
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No rows found.");
            }

            sdr.Close();

            conn.Close();

            return upiList;
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