#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

using Inaugura;
using Inaugura.Telephony;
using Inaugura.RealLeads;

#endregion

namespace OrbisTel.Services.RealLeads
{
	internal abstract class CallHandler: IStatusable
	{
		#region Variables
		private RealLeadsIncommingLine mLine;
		private CallLog mCallDetails;
        private Company mCompany;
		private Customer mCustomer;
		private Agent mAgent;
		private Listing mListing;
		private String mStatus = String.Empty;
		#endregion

		#region Properties
		/// <summary>
		/// The parent real estate service
		/// </summary>
		/// <value></value>
		protected RealLeadsService Service
		{
			get
			{
				return this.Line.Service as RealLeadsService;
			}			
		}

		/// <summary>
		/// The Line used during this call
		/// </summary>
		/// <value></value>
		protected RealLeadsIncommingLine Line
		{
			get
			{
				return this.mLine;
			}
			private set
			{
				this.mLine = value;
			}
		}

		/// <summary>
		/// The details of the call
		/// </summary>
		protected CallLog CallDetails
		{
			get
			{
				return this.mCallDetails;
			}
		}
				
        /// <summary>
		/// The Company that was accessed during this call
		/// </summary>
		/// <value></value>
		public Company Company
		{
			get
			{
				return this.mCompany;
			}
			protected set
			{
				this.mCompany = value;
			}
		}

		/// <summary>
		/// The agent that was accessed during this call
		/// </summary>
		/// <value></value>
		public Agent Agent
		{
			get
			{
				return this.mAgent;
			}
			set
			{
				this.mAgent = value;
			}
		}

		/// <summary>
		/// The agent that was accessed during this call
		/// </summary>
		/// <value></value>
		public Customer Customer
		{
			get
			{
				return this.mCustomer;
			}
			set
			{
				this.mCustomer = value;
			}
		}

		/// <summary>
		/// The Listing that was accessed during this call
		/// </summary>
		/// <value></value>
		public Listing Listing
		{
			get
			{
				return this.mListing;
			}
			set
			{
				this.mListing = value;
			}
		}
		#endregion

		internal CallHandler(RealLeadsIncommingLine line, CallLog details)
		{
			this.Line = line;
			this.mCallDetails = details;
		}

		public abstract void ProcessCall();		
		
		#region IStatusable Members
		public event StatusHandler StatusChanged;				// The status changed event

		/// <summary>
		/// The status of the service
		/// </summary>
		/// <value></value>
		public string Status
		{
			get
			{
				return this.mStatus;
			}
			protected set
			{
				this.mStatus = value;
				this.CallStatusChangedEvent();
			}
		}

		/// <summary>
		/// Event Caller
		/// </summary>
		private void CallStatusChangedEvent()
		{
			if (this.StatusChanged != null)
				this.StatusChanged(this,new StatusEventArgs(this));
		}
		#endregion
	}
}
