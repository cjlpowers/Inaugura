using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// A helper class for caching
/// </summary>
public static class CacheHelper
{
    #region Properties
    /// <summary>
    /// The underlying cache object
    /// </summary>
    static public System.Web.Caching.Cache Cache
    {
        get
        {
            return HttpRuntime.Cache;
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Inserts an object into the cache
    /// </summary>
    /// <param name="key">The Key</param>
    /// <param name="value">The value</param>
    /// <param name="dependencies">The dependencies</param>
    static public void Insert(string key, object value, System.Web.Caching.CacheDependency dependencies)
    {
        System.Diagnostics.Debug.WriteLine(string.Format("Caching item {0}", key));
        HttpRuntime.Cache.Insert(key, value, dependencies);
    }

    /// <summary>
    /// Inserts an object into the cache
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    static public void Insert(string key, object value)
    {
        System.Diagnostics.Debug.WriteLine(string.Format("Caching item {0} (no dependecny)", key));
        HttpRuntime.Cache.Insert(key, value);
    }

    /// <summary>
    /// Inserts an object into the cache
    /// </summary>
    /// <param name="key">The key</param>
    /// <param name="value">The value</param>
    /// <param name="absoluteExpiration">The time at which the item will be removed</param>
    static public void Insert(string key, object value, DateTime absoluteExpiration)
    {
        Insert(key, value, absoluteExpiration, System.Web.Caching.CacheItemPriority.Default);
    }

    /// <summary>
    /// Inserts an object into the cache
    /// </summary>
    /// <param name="key">The key</param>
    /// <param name="value">The value</param>
    /// <param name="absoluteExpiration">The time at which the item will be removed</param>
    static public void Insert(string key, object value, DateTime absoluteExpiration, System.Web.Caching.CacheItemPriority priority)
    {
        System.Diagnostics.Debug.WriteLine(string.Format("Caching item {0} (expiration {1})", key, absoluteExpiration));
        HttpRuntime.Cache.Insert(key, value, null, absoluteExpiration, TimeSpan.Zero, priority, null);
    }

    /// <summary>
    /// Inserts an object into the cache
    /// </summary>
    /// <param name="key">The key</param>
    /// <param name="value">The value</param>
    /// <param name="slidingExpiration">The interval between the time the inserted object is last accessed and the time at which that object expires.</param>
    static public void Insert(string key, object value, TimeSpan slidingExpiration)
    {
        System.Diagnostics.Debug.WriteLine(string.Format("Caching item {0} (sliding expiration {1})", key, slidingExpiration));
        HttpRuntime.Cache.Insert(key, value, null, DateTime.MaxValue, slidingExpiration);
    }

    /// <summary>
    /// Retrieves a object from cache
    /// </summary>
    /// <param name="key">The object key</param>
    /// <returns>The object if it is in the cache, otherwise null</returns>
    static public object Retrieve(string key)
    {
        return HttpRuntime.Cache[key];
    }


    #region Caching of specific objects
    /// <summary>
    /// Caches a listing
    /// </summary>
    /// <param name="listing">The Listing to cache</param>
    static public void Insert(Inaugura.RealLeads.Listing listing)
    {
        Insert(listing.ID.ToString(), listing, TimeSpan.FromMinutes(1));
    }

    /// <summary>
    /// Caches a user
    /// </summary>
    /// <param name="user">The user</param>
    static public void Insert(Inaugura.RealLeads.User user)
    {
        Insert(user.ID.ToString(), user, TimeSpan.FromMinutes(1));
    }
     
    /// <summary>
    /// Caches a customer
    /// </summary>
    /// <param name="customer">The customer</param>
    static public void Insert(Inaugura.Maps.Locale locale)
    {
        Insert(locale.ID, locale, TimeSpan.FromMinutes(10));
    }
    #endregion
    #endregion


}
