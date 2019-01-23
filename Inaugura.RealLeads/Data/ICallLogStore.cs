using System;


namespace Inaugura.RealLeads.Data
{
	/// <summary>
	/// Summary description for IListingStore.
	/// </summary>
	public interface ICallLogStore
	{
		CallVolume[] GetAdminCallVolume(string agentID, DateTime startDate, DateTime endDate, Volume.Resolution resolution);
        CallVolume[] GetSwitchCallVolume(string switchID, DateTime startDate, DateTime endDate, Volume.Resolution resolution);
        CallVolume[] GetAgentCallVolume(string agentID, DateTime startDate, DateTime endDate, Volume.Resolution resolution);
        CallVolume[] GetListingCallVolume(string listingID, DateTime startDate, DateTime endDate, Volume.Resolution resolution);
		CallLog[] GetRecentCalls(string switchID, string agentID, string listingID, bool includeAgentCalls, int maxItems);

		/// <summary>
        /// Retreives a call log
		/// </summary>
		/// <param name="switchID">The switch ID</param>
		/// <param name="agentID">The agent ID</param>
		/// <param name="listingID">The listing ID</param>
		/// <param name="includeAgentCalls">Determines if calls from the agent should be included</param>
		/// <param name="startDate">The start date</param>
		/// <param name="endDate">The end date</param>
		/// <returns>The list of call details found</returns>
		CallLog[] GetCalls(string switchID, string agentID, string listingID, bool includeAgentCalls, DateTime startDate, DateTime endDate);
		
		/// <summary>
        /// Adds a call log
		/// </summary>
		/// <param name="callDetails">The details of a call</param>
		void Add(CallLog callDetails);

        /// <summary>
        /// Gets the call log for a specific call
        /// </summary>
        /// <param name="id">The id of the call log</param>
        /// <returns>The call log with the specified ID, otherwise null</returns>
        CallLog GetCall(string id);

		/// <summary>
        /// Removes all call logs which occurd before a specific date
		/// </summary>
        /// <param name="dateTime">The date and time before which to remove all logs</param>
		void RemoveAll(DateTime dateTime);
	}
}
