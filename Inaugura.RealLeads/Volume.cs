using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// Represents volume 
	/// </summary>
	public class Volume
	{
        public enum Resolution
        {
            Year = 0,
            Month = 1,
            Day = 2,
            Hour = 3,
            Minute = 4
        }

		#region Variables
		private DateTime mDate;
		private int mVolume;
		private string mAgentID;
		private string mListingID;
		#endregion

		#region Properties
		/// <summary>
		/// The date and time
		/// </summary>
		public DateTime Time
		{
			get
			{
				return this.mDate;
			}
			private set
			{
				this.mDate = value;
			}
		}

		/// <summary>
		/// The volume
		/// </summary>
		public int Value
		{
			get
			{
				return this.mVolume;
			}
			private set
			{
				this.mVolume = value;
			}
		}
       
		/// <summary>
		/// The ID of the AgentID
		/// </summary>
		public string AgentID
		{
			get
			{
				return this.mAgentID;
			}
			private set
			{
				this.mAgentID = value;
			}
		}

		/// <summary>
		/// The ID of the ListingID
		/// </summary>
		public string ListingID
		{
			get
			{
				return this.mListingID;
			}
			private set
			{
				this.mListingID = value;
			}
		}			
		#endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="time">The time</param>
        /// <param name="value">The volume value</param>
        /// <param name="agentID">The agent ID</param>
        /// <param name="listingID">The listing ID</param>
		public Volume(DateTime time, int value, string agentID, string listingID)
		{
            this.Time = time;
			this.Value = value;
			this.AgentID = agentID;
			this.ListingID = listingID;
		}
	}
}
