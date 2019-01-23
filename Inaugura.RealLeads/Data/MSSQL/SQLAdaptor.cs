using System;
using System.Data;
using System.Data.SqlClient;

using Inaugura.Data;

namespace Inaugura.RealLeads.Data
{
	/// <summary>
	/// Data Adaptor for MS SQL Server
	/// </summary>
	public class SQLAdaptor : Inaugura.RealLeads.Data.IRealLeadsDataAdaptor
	{	
		#region Member Variables
        private AdministrationStore mAdministrationStore;
        private AddressStore mAddressStore;
        private UserStore mUserStore;
		private ListingStore mListingStore;
		private VoiceMailStore mVoiceMailStore;
		private CallLogStore mCallLogStore;
        private WebLogStore mWebLogStore;
        private InvoiceStore mInvoiceStore;
		private string mConnectionString;
		#endregion

		#region Properties
		#region IDataAdaptor Members
        public IAdministrationStore AdministrationStore
        {
            get
            {
                return this.mAdministrationStore;
            }
        }

        public IAddressStore AddressStore
        {
            get
            {
                return this.mAddressStore;
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
				return this.mVoiceMailStore;
			}
		}

		public ICallLogStore CallLogStore
		{
			get
			{
				return this.mCallLogStore;
			}
		}

        public IWebLogStore WebLogStore
        {
            get
            {
                return this.mWebLogStore;
            }
        }

        public IInvoiceStore InvoiceStore
        {
            get
            {
                return this.mInvoiceStore;
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
		#endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">The database connection string</param>
        /// <param name="fileDirectory">A directory used to store listing files</param>
        public SQLAdaptor(string connectionString, string fileDirectory)
        {
            this.ConnectionString = connectionString;
            this.mAdministrationStore = new AdministrationStore(this, connectionString);
            this.mAddressStore = new AddressStore(this, connectionString);
            this.mUserStore = new UserStore(this, connectionString);
			this.mListingStore = new ListingStore(this, this.ConnectionString, fileDirectory);
            this.mVoiceMailStore = new VoiceMailStore(this, this.ConnectionString);
			this.mCallLogStore = new CallLogStore(this, this.ConnectionString);
            this.mWebLogStore = new WebLogStore(this, this.ConnectionString);
            this.mInvoiceStore = new InvoiceStore(this, this.ConnectionString);
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
