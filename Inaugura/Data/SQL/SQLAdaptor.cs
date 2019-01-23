using System;
using System.Data;

using System.Data.SqlClient;

namespace Inaugura.Data
{
	/// <summary>
	/// Inaugura MSSQL Adaptor 
	/// </summary>
	public class SQLAdaptor : Inaugura.Data.IDataAdaptor
	{	
		#region Member Variables
		private ZoneStore mZoneStore;
		private string mConnectionString = "";
		#endregion

		#region Properties
		#region IDataAdaptor Members
		public IZoneStore ZoneStore
		{
			get
			{
				return this.mZoneStore;
			}
		}
		#endregion

		public string ConnectionString
		{
			get
			{
				return this.mConnectionString;
			}
            set
            {
                this.mConnectionString = value;                
                this.mZoneStore.ConnectionString = value;
            }
		}        
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="connectionString">The connection string to the database containging the Inaugura specific tables</param>
		public SQLAdaptor(string connectionString)
		{
            this.mConnectionString = connectionString;
            this.mZoneStore = new ZoneStore(this, connectionString);
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
	}
}
