using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	public class CallLog : Log
	{
		#region Variables      
		#endregion
        
		#region Properties
      	/// <summary>
		/// The GUID of the switch which processed the call
		/// </summary>
		/// <value></value>
		public string SwitchID
		{
            get
            {
                return this.Details["SwitchID"];
            }
            set
            {
                this.Details["SwitchID"] = value;
            }
		}

		/// <summary>
		/// The ani caller ID
		/// </summary>
		/// <value></value>
		public string CallerID
		{
            get
            {
                return this.Details["CallerID"];
            }
            set
            {
                this.Details["CallerID"] = value;
            }
		}
    
		/// <summary>
		/// Flag determining if the call was accepted by an agent
		/// </summary>
		/// <value>False if the agent opted to not accept the call, True otherwise</value>
		public bool AgentAccepted
		{
			get
			{
				if (this.Details["AgentAccepted"] != null)
					return bool.Parse(this.Details["AgentAccepted"]);
				else
					return false;
			}
			set
			{
				this.Details["AgentAccepted"] = value.ToString();
			}
		}

		/// <summary>
		/// Flag determining if the call was sent to voice mail
		/// </summary>
		/// <value>True if the call was sent to voice mail, False otherwise</value>
		public bool SentToVoiceMail
		{
			get
			{
				if (this.Details["VoiceMail"] != null)
					return bool.Parse(this.Details["VoiceMail"]);
				else
					return false;
			}
			set
			{
				this.Details["VoiceMail"] = value.ToString();
			}
		}       
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>		
        /// <param name="time">The call time</param>
		public CallLog(DateTime time) : base(time)
		{
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">The xml node which represents the call details</param>
        public CallLog(XmlNode node) : base(node)
        {
        }        
	}
}
