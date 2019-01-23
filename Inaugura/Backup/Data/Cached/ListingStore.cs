#region Using directives
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

using Inaugura.Maps;

using Inaugura.Data;
#endregion

namespace Inaugura.RealLeads.Data.Cached
{
    /// <summary>
    /// A cached implementation of the address store
    /// </summary>
    internal class ListingStore : CachedStore, IListingStore
    {
        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="adaptor">The cached adaptor</param>
        public ListingStore(CachedAdaptor adaptor)
            : base(adaptor)
        {
        }
        #endregion

        #region IListingStore Members

        public Listing GetListing(Guid id)
        {
            Listing item = Cache[id.ToString()] as Listing;
            if (item == null)
            {
                item = this.Data.ListingStore.GetListing(id);
                Cache[id.ToString()] = item;
            }
            return item;
        }

        public Listing[] GetListings(Guid userId)
        {
            Key key = CreateCacheKey("listings",userId);
            Listing[] item = Cache[key] as Listing[];
            if (item == null)
            {
                item = this.Data.ListingStore.GetListings(userId);
                Cache[key] = item;
            }
            return item;
        }

        public Listing[] SearchListings(ListingSearch search)
        {
            return this.Data.ListingStore.SearchListings(search, this.Cache);
        }

        public Listing[] SearchListings(ListingSearch search, Inaugura.Caching.Cache cache)
        {
            return this.Data.ListingStore.SearchListings(search, cache);
        }

        public Listing GetFeaturedListing(Type listingType)
        {
            Key key = CreateCacheKey("feature", listingType);
            Listing item = Cache[key] as Listing;
            if (item == null)
            {
                item = this.Data.ListingStore.GetFeaturedListing(listingType);
                Cache[key] = item;
            }
            return item;
        }

        public void Add(Listing listing)
        {
            this.Data.ListingStore.Add(listing);
            Key key = CreateCacheKey("listings", listing.UserID);
            Cache.Remove(key);
        }

        public bool Remove(Guid id)
        {
            bool result = this.Data.ListingStore.Remove(id);       
            // todo remove the users listings from cache
            Cache.Remove(id.ToString());
            return result;
        }

        public bool Update(Listing listing)
        {
            bool result = this.Data.ListingStore.Update(listing);
            listing.Objects.Clear();
            Cache[listing.ID.ToString()] = listing;
            Key key = CreateCacheKey("listings", listing.UserID);
            Cache.Remove(key);
            return result;
        }

        public Listing GetListingByCode(string code)
        {
            return this.Data.ListingStore.GetListingByCode(code);
        }

        public string[] GetUnusedCodes(int maxCodes)
        {
            return this.Data.ListingStore.GetUnusedCodes(maxCodes);
        }

        public void AddFile(Guid listingID, File file)
        {
            this.Data.ListingStore.AddFile(listingID, file);
        }

        public File GetFile(Guid fileId)
        {
            return this.Data.ListingStore.GetFile(fileId);
        }

        public bool RemoveFile(Guid fileId)
        {
            return this.Data.ListingStore.RemoveFile(fileId);
        }

        public bool UpdateFile(File file)
        {
            return this.Data.ListingStore.UpdateFile(file);
        }

        public void AddValidCode(string code)
        {
            this.Data.ListingStore.AddValidCode(code);
        }

        public void AddArchivedListing(Listing listing)
        {
            this.Data.ListingStore.AddArchivedListing(listing);
        }

        #region Listing Abuse
        public AbuseNotification[] SearchAbuseNotifications(AbuseNotificationSearch searchCriteria)
        {
            return this.Data.ListingStore.SearchAbuseNotifications(searchCriteria);
        }

        public void AddAbuseNotification(AbuseNotification notification)
        {
            this.Data.ListingStore.AddAbuseNotification(notification);
        }

        public void UpdateAbuseNotification(AbuseNotification notification)
        {
            this.Data.ListingStore.UpdateAbuseNotification(notification);
        }

        public void RemoveAbuseNotification(Guid id)
        {
            this.Data.ListingStore.RemoveAbuseNotification(id);
        }
        #endregion

        #endregion
    }
}
