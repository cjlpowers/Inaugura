using System;

namespace Inaugura.RealLeads.Data
{
	/// <summary>
	/// Summary description for IAgentStore.
	/// </summary>
	public interface IInvoiceStore
	{
		#region Invoice Store
		/// <summary>
		/// Gets an Invoice
		/// </summary>
        /// <param name="invoiceID">The ID of the Invoice</param>
		/// <returns>The Invoice matching the specified ID, otherwise null</returns>
		Invoice GetInvoice(string invoiceID);
        		
		/// <summary>
		/// Adds an Invoice 
		/// </summary>
		/// <param name="invoice">The Invoice to add</param>
		void Add(Invoice invoice);

		/// <summary>
		/// Removes an Invoice
		/// </summary>
		/// <param name="id">The ID of the Invoice to remove</param>
		/// <returns>True if removed, false otherwise</returns>
		bool Remove(string id);        

		/// <summary>
		/// Updates an Invoice
		/// </summary>
		/// <param name="invoice">The Invoice to update</param>
		/// <returns>True if updated, false otherwise</returns>
		bool Update(Invoice invoice);        
        #endregion	
	}
}
