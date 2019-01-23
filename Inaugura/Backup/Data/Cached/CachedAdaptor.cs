using System;
using System.Data;
using System.Data.SqlClient;

using Inaugura.Data;

namespace Inaugura.RealLeads.Data.Cached
{
	/// <summary>
	/// Data Adaptor for MS SQL Server
	/// </summary>
	public class CachedAdaptor : Inaugura.RealLeads.Data.IRealLeadsDataAdaptor
	{	
		#region Member Variables
        private Inaugura.Caching.Cache mCache;
        Inaugura.RealLeads.Data.IRealLeadsDataAdaptor mData;
        private AddressStore mAddressStore;
        private UserStore mUserStore;
        private ListingStore mListingStore;
		#endregion

		#region Properties
        /// <summary>
        /// The underlying cache object
        /// </summary>
        internal Inaugura.Caching.Cache Cache
        {
            get
            {
                return this.mCache;
            }
        }

        /// <summary>
        /// The underlying data adaptor
        /// </summary>
        internal Inaugura.RealLeads.Data.IRealLeadsDataAdaptor Data
        {
            get
            {
                return this.mData;
            }
        }

		#region IDataAdaptor Members
        public IAdministrationStore AdministrationStore
        {
            get
            {
                return this.Data.AdministrationStore;
            }
        }

        public IAddressStore AddressStore
        {
            get
            {
                return mAddressStore;
            }
        }

        public IUserStore UserStore
        {
            get
            {
                return this.mUserStore;
            }
        }
		
		public IListingStore ListingStore
		{
			get
			{
                return this.mListingStore;
			}
		}

		public IVoiceMailStore VoiceMailStore
		{
			get
			{
                return this.Data.VoiceMailStore;
			}
		}

		public ICallLogStore CallLogStore
		{
			get
			{
                return this.Data.CallLogStore;
			}
		}

        public IWebLogStore WebLogStore
        {
            get
            {
                return this.Data.WebLogStore;
            }
        }

        public IInvoiceStore InvoiceStore
        {
            get
            {
                return this.Data.InvoiceStore;
            }
        }

		#endregion
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">The underlying data object</param>
        /// <param name="cache">The underlying cache object</param>
        public CachedAdaptor(IRealLeadsDataAdaptor data, Inaugura.Caching.Cache cache)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (cache == null)
                throw new ArgumentNullException("cache");

            this.mData = data;
            this.mCache = cache;

            this.mAddressStore = new AddressStore(this);
            this.mUserStore = new UserStore(this);
            this.mListingStore = new ListingStore(this);
		}

        /// <summary>
        /// Establishes a database connection
        /// </summary>
        /// <param name="dbConnectionString">The database connection string</param>
        /// <returns>A database connection</returns>
        /// <exception cref="ConnectionException">Thrown when a connection attempt fails</exception>
		internal static SqlConnection EstablishConnection(string dbConnectionString)
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
