using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Data
{
    /// <summary>
    ///  A hybrid Customer Store which includes caching functionality
    /// </summary>
    public class CompanyStoreCached : CompanyStore, IDisposable
    {
        #region Variables
        private Caching.Cache<Company> mCompanyCache;
        private bool mDisposed = false;
        #endregion

        #region Properties
        /// <summary>
        /// The company cache
        /// </summary>
        protected Caching.Cache<Company> CompanyCache
        {
            get
            {
                return this.mCompanyCache;
            }
            private set
            {
                this.mCompanyCache = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="adaptor">The data adaptor</param>
        /// <param name="connectionString">The database connection string</param>
        /// <param name="cacheDirectory">The cache directory</param>
        public CompanyStoreCached(IDataAdaptor adaptor, string connectionString, string cacheDirectory)
            : base(adaptor, connectionString)
        {
            this.CompanyCache = new Inaugura.Data.Caching.Cache<Company>(cacheDirectory);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~CompanyStoreCached()
        {
            this.Dispose(false);
        }

        public override Company GetCompany(string id)
        {
            try
            {
                Company company = base.GetCompany(id);
                if (company != null)
                    this.CompanyCache.Store(company);
                return company;
            }
            catch (Inaugura.Data.DataException exception)
            {
                Inaugura.Log.AddException(exception);
                // try and get the company from the cache
                Company company =  this.CompanyCache.Retrieve(id);
                if (company == null)
                    throw exception;
                else
                    return company;                    
            }
        }

        public override void Add(Company company)
        {
            base.Add(company);
            this.CompanyCache.Store(company);
        }

        public override bool Update(Company company)
        {
            // make sure we are not updating a company which has originated from the cache
            if (company.FromCache)
                throw new Inaugura.Data.Caching.CacheException("Cannot save a object to a database which has originated from a cache");


            bool result = base.Update(company);
            if (result)
                this.CompanyCache.Store(company);
            return result;           
        }

        public override bool Remove(string id)
        {
            bool result = base.Remove(id);
            if (result)
                this.CompanyCache.Remove(id);
            return result;
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
                    // dispose any managed resources
                    this.mCompanyCache.Dispose();
                }
                // free any unmanaged resources
                this.mDisposed = true;
            }
        }
        #endregion
        #endregion

    }
}
