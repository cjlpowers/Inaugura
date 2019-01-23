using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Inaugura.Data
{
    /// <summary>
    /// A base class for SQL Data stores
    /// </summary>
    public abstract class SqlDataStore
    {
        #region Variables
        private string mConnectionString;
        #endregion

        #region Properties
        /// <summary>
        /// The database connection string
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return this.mConnectionString;
            }
            set
            {
                this.mConnectionString = value;
            }
        }
        #endregion

        #region Methods
        protected SqlDataStore(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// Establishes a connection to a database
        /// </summary>
        /// <param name="dbConnectionString">The database connection string</param>
        /// <returns>The opend connection to the database</returns>
        /// <exception cref="ConnectionException">An exception thrown when a connection attempt fails</exception>
        public static SqlConnection EstablishConnection(string dbConnectionString)
        {
            try
            {
                SqlConnection connection = new SqlConnection(dbConnectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                throw new ConnectionException("Could not extablish connection with database", ex);
            }
        }

        /// <summary>
        /// Gets a result table from a sql command
        /// </summary>		
        /// <param name="cmd">The command object</param>
        /// <returns>The datatable object</returns>
        static protected System.Data.DataTable GetResult(SqlCommand cmd)
        {
            using (SqlDataAdapter adaptor = new SqlDataAdapter(cmd))
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                adaptor.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// Gets all results from a sql command
        /// </summary>		
        /// <param name="cmd">The command object</param>
        /// <returns>The dataset object</returns>
        static protected System.Data.DataSet GetResults(SqlCommand cmd)
        {
            using (SqlDataAdapter adaptor = new SqlDataAdapter(cmd))
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                adaptor.Fill(ds);
                return ds;
            }
        }
        #endregion
    }
}
