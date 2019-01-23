using System;


namespace Inaugura.RealLeads.Data
{
	/// <summary>
	/// Summary description for IListingStore.
	/// </summary>
	public interface IListingStore
	{		
		// generic methods
		/// <summary>
		/// Gets a Listing
		/// </summary>
		/// <param name="id">The id of the Listing</param>
		/// <returns>The Listing</returns>
		Listing GetListing(Guid id);

		/// <summary>
		/// Gets a List of Listings
		/// </summary>
        /// <param name="userId">The ID of the user to which the Listings belong</param>
		/// <returns>The list of Listings</returns>
		Listing[] GetListings(Guid userId);

        /// <summary>
        /// Searches RealEstateListings
        /// </summary>
        /// <param name="search">The search criteria</param>
        /// <returns>The list of realestate listings meeting the search criteria</returns>
        Listing[] SearchListings(ListingSearch search);

        /// <summary>
        /// Searches RealEstateListings
        /// </summary>
        /// <param name="search">The search criteria</param>
        /// <param name="cache">The cache object</param>
        /// <returns>The list of realestate listings meeting the search criteria</returns>
        Listing[] SearchListings(ListingSearch search, Inaugura.Caching.Cache cache);

         /// <summary>
        /// Gets a featured listing
        /// </summary>
       /// <param name="listingType">The type of listing</param>
        /// <returns></returns>        
        Listing GetFeaturedListing(Type listingType);

		/// <summary>
		/// Adds a Listing
		/// </summary>
		/// <param name="listing">The Listing to add</param>
		void Add(Listing listing);

		/// <summary>
		/// Removes a Listing
		/// </summary>
		/// <param name="id">The ID of the Listing to remove</param>
        bool Remove(Guid id);

		/// <summary>
		/// Updates a Listing
		/// </summary>
		/// <param name="listing">The Listing to update</param>
		bool Update(Listing listing);

		#region Zones
		/// <summary>
		/// Gets a Listing
		/// </summary>
		/// <param name="zoneID">The ID of the Zone to which the Listing belongs</param>
		/// <param name="code">The code of the Listing</param>
		/// <returns></returns>
		Listing GetListingByCode(string code);

        /*
		/// <summary>
		/// Adds a code to a Listing's Profile
		/// </summary>
		/// <param name="listingID">The Listing ID</param>
		/// <param name="zoneID">The zone ID</param>
		/// <param name="code">The code</param>
		void AddCode(string listingID, string zoneID, string code);

		/// <summary>
		/// Removes a Code from a Listings Profile
		/// </summary>
		/// <param name="listingID">The Listing ID</param>
		/// <param name="zoneID">The Zone ID</param>
		/// <param name="code">The code</param>
		bool RemoveCode(string listingID, string zoneID, string code);

		/// <summary>
		/// Gets a list of zones inside which a listing currently operates
		/// </summary>
		/// <param name="listingID">The listing ID</param>
		/// <returns>The list of Zones</returns>
		Zone[] GetZonesForListing(string listingID);

		/// <summary>
		/// Gets the Listing's code for a given zone
		/// </summary>
		/// <param name="listingID">The Listing ID</param>
		/// <param name="zoneID">The Zone ID</param>
		string GetCodeForListing(string listingID, string zoneID);
         */

		/// <summary>
		/// Gets a list of unused listing codes
		/// </summary>
		/// <param name="maxPinCodes">The maximum number of codes to return</param>
		/// <returns>A list of unused listing codes</returns>
		string[] GetUnusedCodes(int maxCodes);
		#endregion

		#region Agent Files
		/// <summary>
		/// Adds a file to an Listing's profile
		/// </summary>
		/// <param name="listingID">The Listing ID</param>
		/// <param name="file">The File to add</param>
        void AddFile(Guid listingID, File file);

		/// <summary>
		/// Gets a File
		/// </summary>
		/// <param name="fileId">The ID of the File</param>
		/// <returns>The File with the specified ID</returns>
        File GetFile(Guid fileId);

		/// <summary>
		/// Removes a File
		/// </summary>
		/// <param name="fileId">The ID of the File</param>
		/// <returns>The File with the specified ID</returns>
        bool RemoveFile(Guid fileId);

		/// <summary>
		/// Updates a File
		/// </summary>
		/// <param name="file">The updated File</param>
		/// <returns></returns>
		bool UpdateFile(File file);
		#endregion

		#region Valid Codes
		void AddValidCode(string code);
		#endregion

        #region Listing Archive
        /// <summary>
        /// Adds a listing no longer in use to the listing archive
        /// </summary>
        /// <param name="listing">The listing</param>
        void AddArchivedListing(Listing listing);
        #endregion

        #region Listing Abuse
        /// <summary>
        /// Searches abuse notifications
        /// </summary>
        /// <param name="searchCriteria">The search criteria</param>
        /// <returns>The search criteria</returns>
        AbuseNotification[] SearchAbuseNotifications(AbuseNotificationSearch searchCriteria);

        /// <summary>
        /// Adds an abuse notification
        /// </summary>
        /// <param name="notification">The abuse notification to add</param>
        void AddAbuseNotification(AbuseNotification notification);

        /// <summary>
        /// Updates an abuse notification
        /// </summary>
        /// <param name="notification">The abuse notification to update</param>
        void UpdateAbuseNotification(AbuseNotification notification);

        /// <summary>
        /// Removes an abuse notification
        /// </summary>
        /// <param name="id">The ID of the abuse notification</param>
        void RemoveAbuseNotification(Guid id);
        #endregion
    }
}
