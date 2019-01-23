using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.RealLeads.Managers
{
    /// <summary>
    /// The listing manager
    /// </summary>
    public class ListingManager : Manager
    {
         /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="api">The RealLeads API object</param>
        /// <param name="dataAdaptor">The data adaptor</param>
        internal ListingManager(RealLeadsAPI api, Data.IRealLeadsDataAdaptor dataAdaptor)
            : base(api, dataAdaptor)
        {
        }


        /// <summary>
        /// Gets a listing with the specified ID
        /// </summary>
        /// <param name="id">The listing ID</param>
        /// <returns>The listing if found, otherwise null</returns>
        public Listing GetListing(Guid id)
        {
            return this.Data.ListingStore.GetListing(id);
        }

        /// <summary>
        /// Gets a listing which a specific code
        /// </summary>
        /// <param name="code">The listing code</param>
        /// <returns>The listing if found, otherwise null.</returns>
        public Listing GetListingByCode(string code)
        {
            return this.Data.ListingStore.GetListingByCode(code);
        }

        /// <summary>
        /// Adds a listing
        /// </summary>
        /// <param name="listing">The listing to add</param>
        public void AddListing(Listing listing)
        {
            listing.EnforceEditPolicy();

            this.Data.ListingStore.Add(listing);
        }

        /// <summary>
        /// Gets all the listings for a specific user
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>The listings owned by the user</returns>
        public Listing[] GetListings(User user)
        {
            return this.Data.ListingStore.GetListings(user.ID);            
        }

        /// <summary>
        /// Updates a listing
        /// </summary>
        /// <param name="listing">The listing to update</param>
        public void UpdateListing(Listing listing)
        {
            listing.EnforceEditPolicy();
            this.Data.ListingStore.Update(listing);
        }

        #region Search
        /// <summary>
        /// Performs a search for listings
        /// </summary>
        /// <param name="search">The search criteria</param>
        /// <returns>The search results</returns>
        public Listing[] SearchListings(ListingSearch search)
        {
            return this.Data.ListingStore.SearchListings(search);
        }
        #endregion

        /// <summary>
        /// Gets a featured listing
        /// </summary>
        /// <param name="listingType">The type of listing</param>
        /// <returns></returns>        
        public Listing GetFeaturedListing(Type listingType)
        {
            return this.Data.ListingStore.GetFeaturedListing(listingType);
        }

        /// <summary>
        /// Gets a list of unused codes
        /// </summary>
        /// <param name="maxCodes">The number of codes to return</param>
        /// <returns>The list of unused codes</returns>
        public string[] GetUnusedCodes(int maxCodes)
        {
            return this.Data.ListingStore.GetUnusedCodes(maxCodes);
        }

        private delegate void LogWebViewDelegate(Inaugura.RealLeads.Log item);
        /// <summary>
        /// Logs a viewing of a listing via the web
        /// </summary>
        /// <param name="log">The log item</param>
        public void LogWebView(Inaugura.RealLeads.Log log)
        {
            LogWebViewDelegate operation = delegate(Inaugura.RealLeads.Log item)
            {
                this.Data.WebLogStore.Add(item);
            };
            this.API.ThreadPool.QueueWorkItem(operation, log);
        }

        #region Files
        /// <summary>
        /// Gets a file
        /// </summary>
        /// <param name="fileID">The file ID</param>
        /// <returns></returns>
        public File GetFile(Guid fileID)
        {
            return this.Data.ListingStore.GetFile(fileID);
        }

        /// <summary>
        /// Adds a file
        /// </summary>
        /// <param name="listingID">The listing ID</param>
        /// <param name="file">The file</param>
        public void AddFile(Guid listingID, File file)
        {
            this.Data.ListingStore.AddFile(listingID, file);
        }

        /// <summary>
        /// Removes a file
        /// </summary>
        /// <param name="fileID">The file ID</param>
        public void RemoveFile(Guid fileID)
        {
            this.Data.ListingStore.RemoveFile(fileID);
        }
        #endregion

        #region Abuse
        /// <summary>
        /// Adds an abuse notification
        /// </summary>
        /// <param name="notification"></param>
        public void AddAbuseNotification(AbuseNotification notification)
        {
            this.Data.ListingStore.AddAbuseNotification(notification);
        }

        /// <summary>
        /// Performs a search for abuse notifications
        /// </summary>
        /// <param name="search">The search criteria</param>
        /// <returns>The search results</returns>
        public AbuseNotification[] SearchAbuseNotifications(AbuseNotificationSearch search)
        {
            return this.Data.ListingStore.SearchAbuseNotifications(search);
        }
        #endregion



        #region Maintenance
        /// <summary>
        /// Performs the required actions to maintain listings
        /// </summary>
        public void PerformListingMaintenance()
        {
                this.ArchiveExpiredListings(DateTime.Now);
        } 
       
        /// <summary>
        /// Archives Listings Expiring before a given date
        /// </summary>
        /// <param name="expiryDate">The expiration date</param>
        public void ArchiveExpiredListings(DateTime expiryDate)
        {
            // ensure the expiration date is not greater then todays date
            if (expiryDate > DateTime.Now)
                throw new ArgumentException(string.Format("The expiry date to archive listings must be in the past. The date {0} does not meet this criteria.", expiryDate));

            Inaugura.RealLeads.ListingSearch search = new ListingSearch();
            search.CalculateResultCount = false;
            search.ExpirationEnd = expiryDate;
            search.StartIndex = 1;
            search.EndIndex = 500;
            
            // perform the search            
            Inaugura.RealLeads.Listing[] expiredListings = this.SearchListings(search);
            foreach (Inaugura.RealLeads.Listing listing in expiredListings)
            {
                // move the listing to the archive
                this.Data.ListingStore.AddArchivedListing(listing);
                // remove the listing (and dump all images and other related data)
                this.Data.ListingStore.Remove(listing.ID);
            }
        }
        #endregion
    }
}
