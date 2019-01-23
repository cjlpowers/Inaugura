using System;


namespace Inaugura.RealLeads.Data
{

	/// <summary>
	/// Summary description for IListingStore.
	/// </summary>
	public interface IWebLogStore
	{	
        /// <summary>
        /// Gets the web volume for a specific agent
        /// </summary>
        /// <param name="agentID">The id of the Agent</param>
        /// <param name="startDate">The start date</param>
        /// <param name="endDate">The end date</param>
        /// <param name="resolution">The resolution</param>
        /// <returns>The volume</returns>
        Volume[] GetAgentWebVolume(string agentID, DateTime startDate, DateTime endDate, Volume.Resolution resolution);

        /// <summary>
        /// Gets the web volume for a specific listing
        /// </summary>
        /// <param name="listingID">The listing ID</param>
        /// <param name="startDate">The start date</param>
        /// <param name="endDate">The end date</param>
        /// <param name="resolution">The resolution</param>
        /// <returns>The volume</returns>
        Volume[] GetListingWebVolume(string listingID, DateTime startDate, DateTime endDate, Volume.Resolution resolution);
        		
		/// <summary>
		/// Adds call details to the log
		/// </summary>
        /// <param name="webLog">The details of a web hit</param>
		void Add(Log webLog);

        /// <summary>
        /// Gets the web log for a specific hit
        /// </summary>
        /// <param name="id">The id of the log</param>
        /// <returns>The call details with the specified ID, otherwise null</returns>
        Log GetLog(string id);

		/// <summary>
		/// Removes all logs which occurd before a specific date
		/// </summary>
		/// <param name="dateTime">The date and time before which to remove all logs</param>
		void RemoveAll(DateTime dateTime);
	}
}
