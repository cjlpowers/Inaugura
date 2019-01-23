using System;
using Inaugura.RealLeads.Administration;


namespace Inaugura.RealLeads.Data
{

	/// <summary>
	/// Summary description for IListingStore.
	/// </summary>
    public interface IAdministrationStore
	{
        /// <summary>
        /// Adds error information to the store
        /// </summary>
        /// <param name="errorInformation">The error information</param>
        void AddErrorInformation(ErrorInformation errorInformation);

        /// <summary>
        /// Removes error information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool RemoveErrorInformation(string id);

        /// <summary>
        /// Updates error information
        /// </summary>
        /// <param name="errorInformation">The updated error information</param>
        /// <returns>True if the update was successful</returns>
        bool UpdateErrorInformation(ErrorInformation errorInformation);       
	}
}
