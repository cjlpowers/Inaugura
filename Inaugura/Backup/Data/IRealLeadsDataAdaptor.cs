using System;

namespace Inaugura.RealLeads.Data
{
	/// <summary>
	/// RealLeads Data Source Interface
	/// </summary>
	public interface IRealLeadsDataAdaptor
	{
        IAdministrationStore AdministrationStore
        {
            get;
        }

        IAddressStore AddressStore
        {
            get;
        }

        IUserStore UserStore
        {
            get;
        }        

		IListingStore ListingStore
		{
			get;
		}

		IVoiceMailStore VoiceMailStore
		{
			get;
		}

		ICallLogStore CallLogStore
		{
			get;
		}

        IWebLogStore WebLogStore
        {
            get;
        }

        IInvoiceStore InvoiceStore
        {
            get;
        }

		/*
		IListingStore ListingStore
		{
			get;
		}

		IFileStore FileStore
		{
			get;
		}
		*/
	}
}
