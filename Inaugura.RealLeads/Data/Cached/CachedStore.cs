using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.RealLeads.Data.Cached
{
    /// <summary>
    /// An abstract class representing a cached store
    /// </summary>
    internal abstract class CachedStore
    {
        #region Variables
        private CachedAdaptor mAdaptor;
        #endregion

        #region Properties
        /// <summary>
        /// The underlying cache object
        /// </summary>
        protected Inaugura.Caching.Cache Cache
        {
            get
            {
                return this.mAdaptor.Cache;
            }
        }

        /// <summary>
        /// The underlying data object
        /// </summary>
        protected IRealLeadsDataAdaptor Data
        {
            get
            {
                return this.mAdaptor.Data;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="adaptor">The cached adaptor</param>
        public CachedStore(CachedAdaptor adaptor)
        {
            this.mAdaptor = adaptor;
        }

        /// <summary>
        /// Creates a key for caching and initializes it using the managers type and data adaptor
        /// </summary>
        /// <param name="keyItems">The objects used to create the key</param>
        /// <returns>The key</returns>
        protected Key CreateCacheKey(params object[] keyItems)
        {
            Key key = new Key(this.GetType().GetHashCode(), this.Data.GetHashCode());
            foreach (object item in keyItems)
                key.Add(item);
            return key;
        }
        #endregion
    }
}
