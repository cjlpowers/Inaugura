using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	public class Log : Inaugura.Xml.IXmlable
	{
		#region Variables
        private Guid mID;
		private Details mDetails = null;      
        private DateTime mTime;
        private StringCollection mActivity;
        private TimeSpan mDuration;
		#endregion

		#region IXmlable Members
		/// <summary>
		/// Gets the xml representation of the Listing
		/// </summary>
		/// <value></value>
		public XmlNode Xml
		{
			get
			{
                XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Log");
                this.PopulateNode(node);
                return node;
			}
		}
		#endregion

		#region Properties
        /// <summary>
        /// The GUID 
        /// </summary>
        public Guid ID
        {
            get
            {
                return this.mID;
            }
            private set
            {
                this.mID = value;
            }
        }
        
		/// <summary>
		/// The GUID of the target user of the call
		/// </summary>
		/// <value></value>
		public string UserID
		{
			get
			{
                return this.Details["UserID"];
			}
			set
			{
                this.Details["UserID"] = value;
			}
		}

		/// <summary>
		/// The GUID of the target listing of the call
		/// </summary>
		/// <value></value>
		public string ListingID
		{
			get
			{
                return this.mDetails["ListingID"];
			}
			set
			{
                this.mDetails["ListingID"] = value;
			}
		}
        		
		/// <summary>
		/// Additional details specific to this listing
		/// </summary>
		/// <value></value>
		public Details Details
		{
			get
			{
				return this.mDetails;
			}
            set
            {
                this.mDetails = value;
            }
		}

		/// <summary>
		/// Gets/Sets the start time of the log
		/// </summary>
		/// <value></value>
		public DateTime Time
		{
			get
			{
                return this.mTime;
			}
			set
			{
                this.mTime = value;
			}
		}

        /// <summary>
        /// The duration
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                return this.mDuration;
            }
            set
            {
                this.mDuration = value;
            }
        }
        	
		/// <summary>
		/// Flag determining if the call was initiated by an agent
		/// </summary>
		/// <value>True if the listing has been setup, False otherwise</value>
		public bool IsOwner
		{
			get
			{
				if (this.Details["IsOwner"] != null)
					return bool.Parse(this.Details["IsOwner"]);
				else
					return false;
			}
			set
			{
                this.Details["IsOwner"] = value.ToString();
			}
		}
        		
        /// <summary>
        /// The call log
        /// </summary>
        public StringCollection Activity
        {
            get
            {
                return this.mActivity;
            }
            set
            {
                this.mActivity = value;
            }
        }
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>		
        /// <param name="callTime">The call time</param>
		public Log(DateTime time) 
		{
            this.ID = Guid.NewGuid();
            this.Details = new Details();
            this.Time = time;
            this.Duration = TimeSpan.FromSeconds(0);
            this.Activity = new StringCollection();
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">The xml node which represents the call details</param>
        public Log(XmlNode node) : this(DateTime.Now)
        {
            this.PopulateInstance(node);
        }

        /// <summary>
        /// Adds a time stamped item to the log
        /// </summary>
        /// <param name="item">The log item</param>
        public void AddActivityItem(string item)
        {
            TimeSpan stamp = DateTime.Now - this.Time;
            this.Activity.Add(string.Format("{0}:{1} - {2}", stamp.Minutes.ToString("00"), stamp.Seconds.ToString("00"),item));
        }

        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public void PopulateNode(XmlNode node)
        {
            Inaugura.Xml.Helper.SetAttribute(node, "id", this.ID.ToString());
            Inaugura.Xml.Helper.SetAttribute(node, "time", this.Time.ToUniversalTime().ToString("o", System.Globalization.CultureInfo.InvariantCulture));

            if(this.Duration.TotalSeconds > 0)
                Inaugura.Xml.Helper.SetAttribute(node, "duration", Convert.ToInt32(this.Duration.TotalSeconds).ToString());

            if (this.Details.Count > 0)
            {
                XmlNode detailsNode = node.OwnerDocument.CreateElement("Details");
                this.Details.PopulateNode(detailsNode);
                node.AppendChild(detailsNode);
            }

            if (this.Activity.Count > 0)
            {
                XmlNode logNode = node.OwnerDocument.CreateElement("Activity");
                this.Activity.PopulateNode(logNode);
                node.AppendChild(logNode);
            }           
        }

        /// <summary>
        /// Populates the instance with the specifed xml data
        /// </summary>
        /// <param name="node"></param>
        public void PopulateInstance(XmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException("The xml node may not be null");

            if (node.Attributes["id"] == null)
                throw new ArgumentNullException("The xml node must contain an id attribute");

            this.ID = new Guid(Inaugura.Xml.Helper.GetAttribute(node, "id"));
                                 
            if (node.Attributes["time"] != null)
                this.Time = DateTime.Parse(Inaugura.Xml.Helper.GetAttribute(node, "time"), System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.RoundtripKind);

            if (node.Attributes["duration"] != null)
                this.Duration = TimeSpan.FromSeconds(int.Parse(Inaugura.Xml.Helper.GetAttribute(node, "duration")));
                   
            if (node["Details"] != null)
                this.Details = new Details(node["Details"]);

            if (node["Activity"] != null)
                this.Activity = new StringCollection(node["Activity"]);                       
        }
	}
}
