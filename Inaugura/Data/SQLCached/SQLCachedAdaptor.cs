using System;
using System.Data;

using System.Data.SqlClient;

namespace Inaugura.Data
{
	/// <summary>
	/// Inaugura MSSQL Adaptor 
	/// </summary>
	public class SQLCachedAdaptor : Inaugura.Data.IDataAdaptor, IDisposable
	{	
		#region Member Variables
		private CustomerStoreCached mCustomerStore;
		private CompanyStoreCached mCompanyStore;
		private ZoneStoreCached mZoneStore;
		private string mConnectionString;
        private string mCacheDirectory;
        private bool mDisposed = false;
		#endregion

		#region Properties
		#region IDataAdaptor Members
		public ICustomerStore CustomerStore
		{
			get
			{
				return this.mCustomerStore;
			}
		}

		public ICompanyStore CompanyStore
		{
			get
			{
				return this.mCompanyStore;
			}
		}

		public IZoneStore ZoneStore
		{
			get
			{
				return this.mZoneStore;
			}
		}
		#endregion
        /// <summary>
        /// The database connection string
        /// </summary>
		public string ConnectionString
		{
			get
			{
				return this.mConnectionString;
			}
			private set
			{
				this.mConnectionString = value;
			}
		}        

        /// <summary>
        /// The cache directory
        /// </summary>
        public string CacheDirectory
        {
            get
            {
                return this.mCacheDirectory;
            }
            private set
            {
                this.mCacheDirectory = value;
            }
        }
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="connectionString">The connection string to the database containging the Inaugura specific tables</param>
        /// <param name="cacheDirectory">The cache directory</param>
        public SQLCachedAdaptor(string connectionString, string cacheDirectory)
		{            
            this.ConnectionString = connectionString;
            this.CacheDirectory = cacheDirectory;
            this.mCustomerStore = new CustomerStoreCached(this, connectionString, System.IO.Path.Combine(cacheDirectory, "Customers"));
            this.mCompanyStore = new CompanyStoreCached(this, connectionString, System.IO.Path.Combine(cacheDirectory, "Companies"));
            this.mZoneStore = new ZoneStoreCached(this, connectionString, System.IO.Path.Combine(cacheDirectory, "Zones"));
		}

        /// <summary>
        /// Destructor
        /// </summary>
        ~SQLCachedAdaptor()
        {
            this.Dispose(false);
        }

        #region IDisposable Members
        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!this.mDisposed)
            {
                if (disposing)
                {
                    // dispose managed resources
                    this.mCustomerStore.Dispose();
                    this.mCompanyStore.Dispose();
                    this.mZoneStore.Dispose();
                }
                // dispose unmanaged resources
                this.mDisposed = true;
            }
        }

        #endregion
}
}
