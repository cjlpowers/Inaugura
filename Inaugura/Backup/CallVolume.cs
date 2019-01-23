using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// Represents call volume 
	/// </summary>
	public class CallVolume : Volume
	{
		#region Variables
		private string mSwitchID;	
		#endregion

		#region Properties	
        /// <summary>
		/// The ID of the switch
		/// </summary>
		public string SwitchID
		{
			get
			{
				return this.mSwitchID;
			}
			private set
			{
				this.mSwitchID = value;
			}
		}
		#endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="time">The time</param>
        /// <param name="volume">The volume value</param>
        /// <param name="switchID">The switchID</param>
        /// <param name="agentID">The agentID</param>
        /// <param name="listingID">The listing ID</param>
		public CallVolume(DateTime time, int volume, string switchID, string agentID, string listingID) : base(time,volume,agentID,listingID)
		{
			this.SwitchID = switchID;
		}
	}
}
