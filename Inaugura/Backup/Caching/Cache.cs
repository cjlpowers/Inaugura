using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Caching
{
    /// <summary>
    /// The base class for all Cache implementations
    /// </summary>
    public abstract class Cache
    {
        #region Variables
        private TimeSpan mDefaultAbosluteExpiration;
        #endregion

        #region Indexers
        /// <summary>
        /// Retreives or Inserts an item in the cache
        /// </summary>
        /// <param name="key">The key of the item</param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                return this.Retreive(key);
            }
            set
            {
                this.Insert(key, value);
            }
        }

        /// <summary>
        /// Retreives or Inserts an item in the cache
        /// </summary>
        /// <param name="key">The key of the item</param>
        /// <returns></returns>
        public object this[Key key]
        {
            get
            {
                return this[key.ToString()];
            }
            set
            {
                this[key.ToString()] = value;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// The number of items in the cache
        /// </summary>
        public abstract int Count
        {
            get;
        }

        /// <summary>
        /// Gets a string containing information about the status of the cache
        /// </summary>
        public abstract string Status
        {
            get;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="defaultAbsoluteExpiration">The default absolute expiration</param>
        public Cache(TimeSpan defaultAbsoluteExpiration)
        {
            this.mDefaultAbosluteExpiration = defaultAbsoluteExpiration;
        }      

        /// <summary>
        /// Inserts an item to the cache (overwrites existing items)
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The item to add</param>
        /// <param name="absoluteExpiration">The absolute expiration of the item</param>
        /// <param name="slidingExpiration">The sliding expiration of the item</param>
        public abstract void Insert(string key, object value, DateTime absoluteExpiration, TimeSpan slidingExpiration);

        /// <summary>
        /// Inserts an item to the cache (overwrites existing items)
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The item to add</param>
        /// <param name="absoluteExpiration">The absolute expiration of the item</param>
        /// <param name="slidingExpiration">The sliding expiration of the item</param>
        public void Insert(Key key, object value, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            Insert(key.ToString(), value, absoluteExpiration, slidingExpiration);
        }


        /// <summary>
        /// Inserts an item to the cache (overwrites existing items) 
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The item to add</param>
        /// <param name="absoluteExpiration">The absolute expiration of the item</param>
        public void Insert(string key, object value, DateTime absoluteExpiration)
        {
            this.Insert(key, value, absoluteExpiration, TimeSpan.MaxValue);
        }

        /// <summary>
        /// Inserts an item to the cache (overwrites existing items) 
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The item to add</param>
        /// <param name="absoluteExpiration">The absolute expiration of the item</param>
        public void Insert(Key key, object value, DateTime absoluteExpiration)
        {
            this.Insert(key.ToString(), value, absoluteExpiration);
        }

        /// <summary>
        /// Inserts an item to the cache using the default cache expiration (overwrites existing items) 
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The item to add</param>
        public void Insert(string key, object value)
        {
            this.Insert(key, value, DateTime.Now.Add(this.mDefaultAbosluteExpiration));
        }

        /// <summary>
        /// Inserts an item to the cache using the default cache expiration (overwrites existing items) 
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The item to add</param>
        public void Insert(Key key, object value)
        {
            Insert(key.ToString(), value);
        }

        /// <summary>
        /// Gets an item from the cache
        /// </summary>
        /// <param name="key">The key of the item to retreive</param>
        /// <returns>The item if found, otherwise null.</returns>
        public abstract object Retreive(string key);

        // <summary>
        /// Gets an item from the cache
        /// </summary>
        /// <param name="key">The key of the item to retreive</param>
        /// <returns>The item if found, otherwise null.</returns>
        public object Retreive(Key key)
        {
            return Retreive(key.ToString());
        }

        /// <summary>
        /// Removes a item from the cache
        /// </summary>
        /// <param name="key">The key of the item to remove</param>
        /// <returns>The item removed, otherwise null.</returns>
        public abstract object Remove(string key);

        /// <summary>
        /// Removes a item from the cache
        /// </summary>
        /// <param name="key">The key of the item to remove</param>
        /// <returns>The item removed, otherwise null.</returns>
        public object Remove(Key key)
        {
            return Remove(key.ToString());
        }
        #endregion
    }
}
