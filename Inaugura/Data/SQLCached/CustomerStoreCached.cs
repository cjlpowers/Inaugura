using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Data
{
    /// <summary>
    ///  A hybrid Customer Store which includes caching functionality
    /// </summary>
    public class CustomerStoreCached : CustomerStore, IDisposable
    {
        #region Varaibles
        private Inaugura.Data.Caching.Cache<Customer> mCustomerCache;
        private bool mDisposed = false;
        #endregion

        #region Properties
        /// <summary>
        /// The customer cache
        /// </summary>
        protected Inaugura.Data.Caching.Cache<Customer> CustomerCache
        {
            get
            {
                return this.mCustomerCache;
            }
            private set
            {
                this.mCustomerCache = value;
            }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="adaptor">The data adaptor</param>
        /// <param name="connectionString">The connection string</param>
        /// <param name="cacheDirectory">The cache directory</param>
        public CustomerStoreCached(IDataAdaptor adaptor, string connectionString, string cacheDirectory)
            : base(adaptor, connectionString)
        {
            this.CustomerCache = new Inaugura.Data.Caching.Cache<Customer>(cacheDirectory);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~CustomerStoreCached()
        {
            this.Dispose(false);
        }

        public override Customer GetCustomer(string customerID)
        {
            try
            {
                Customer customer =  base.GetCustomer(customerID);
                if(customer != null)
                    this.CustomerCache.Store(customer);
                return customer;
            }
            catch (Inaugura.Data.DataException exception) // some sort of data exception
            {
                Inaugura.Log.AddException(exception);

                // try to retrieve the customer from the cache
                Customer customer = this.CustomerCache.Retrieve(customerID);
                if (customer == null)
                    throw exception;
                return customer;
            }
        }

        public override Customer GetCustomerByEmail(string email)
        {
            try
            {
                email = email.ToLower();
                Customer customer = base.GetCustomerByEmail(email);
                if (customer != null)
                    this.CustomerCache.Store(customer);
                return customer;
            }
            catch (Inaugura.Data.DataException exception)
            {
                Inaugura.Log.AddException(exception);

                // try and get the customer from cache
                foreach (Customer cust in this.CustomerCache)
                {
                    if (cust.Email.ToLower() == email)
                        return cust;
                }
                throw exception;
            }
        }

        public override void Add(Customer customer)
        {
            base.Add(customer);
            this.CustomerCache.Store(customer);
        }

        public override bool Update(Customer customer)
        {
            // make sure we are not updating a customer which has originated from the cache
            if (customer.FromCache)
                throw new Inaugura.Data.Caching.CacheException("Cannot save a object to a database which has originated from a cache");

            bool result = base.Update(customer);            
            if(result)
                this.CustomerCache.Store(customer);
            return result;
        }

        public override bool Remove(string customerID)
        {
            bool result = base.Remove(customerID);
            if(result)
                this.CustomerCache.Remove(customerID);
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
                    this.mCustomerCache.Dispose();
                }
                
                // release any unmanaged resources

                this.mDisposed = true;
            }
        }

        #endregion
}
}
