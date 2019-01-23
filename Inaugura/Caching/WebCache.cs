using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Caching
{
    /// <summary>
    /// An cache implemention using the ASP.NET runtime cache object
    /// </summary>
    public class WebCache : Cache
    {
        #region Properties
        /// <summary>
        /// The number of items in the cache
        /// </summary>
        public override int Count
        {
            get
            {
                return System.Web.HttpRuntime.Cache.Count;
            }
        }

        /// <summary>
        /// Gets a string containing information about the status of the cache
        /// </summary>
        public override string Status
        {
            get
            {
                return string.Format("{0}\nItem Count: {1}\nKilobytes available: {2}", System.Web.HttpRuntime.Cache.GetType().ToString(), System.Web.HttpRuntime.Cache.Count, System.Web.HttpRuntime.Cache.EffectivePrivateBytesLimit);
            }
        }
        #endregion

        #region Methods
          /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="defaultAbsoluteExpiration">The default absolute expiration</param>
        public WebCache(TimeSpan defaultAbsoluteExpiration) : base(defaultAbsoluteExpiration)
        {
        }
        
        /// <summary>
        /// Inserts an item to the cache (overwrites existing items)
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The item to add</param>
        /// <param name="absoluteExpiration">The absolute expiration of the item</param>
        /// <param name="slidingExpiration">The sliding expiration of the item</param>
        public override void Insert(string key, object value, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            if (value == null)
                return;

            System.Web.HttpRuntime.Cache.Insert(key, value, null, absoluteExpiration, slidingExpiration==TimeSpan.MaxValue?System.Web.Caching.Cache.NoSlidingExpiration:slidingExpiration);
            System.Diagnostics.Debug.WriteLine(string.Format("Inserted '{0}' value '{1}' into Cache",key,value.ToString()));
        }

        /// <summary>
        /// Gets an item from the cache
        /// </summary>
        /// <param name="key">The key of the item to retreive</param>
        /// <returns>The item if found, otherwise null.</returns>
        public override object Retreive(string key)
        {
            object obj = System.Web.HttpRuntime.Cache.Get(key);
            System.Diagnostics.Debug.WriteLine(string.Format("Retreiving '{0}' (key '{1}') from Cache", obj, key));
            return obj;
        }

        /// <summary>
        /// Removes a item from the cache
        /// </summary>
        /// <param name="key">The key of the item to remove</param>
        /// <returns>The item removed, otherwise null.</returns>
        public override object Remove(string key)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Removing '{0}' from Cache", key));
            return System.Web.HttpRuntime.Cache.Remove(key);
        }
        #endregion
    }
}
