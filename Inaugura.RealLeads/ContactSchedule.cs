#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#endregion

namespace Inaugura.RealLeads
{

	[Flags]
	public enum DaysOfWeek
	{
		None = 0,
		Sunday = 1,
		Monday = 2,
		Tuesday = 4,
		Wednesday = 8,
		Thursday = 16,
		Friday = 32,
		Saturday = 64,
		WeekDays = Monday | Tuesday | Wednesday | Thursday | Friday,
		Weekends = Saturday | Sunday,
		All = WeekDays | Weekends		
	}


	public sealed class ContactSchedule: Inaugura.Xml.IXmlable
	{		
		#region Variables
        private Guid mID;
		private string mName;
		private string mContactNumber;
		private TimeSpan mStartTime;
		private TimeSpan mStopTime;
		private DateTime mStartDate;
		private DateTime mStopDate;
		private DaysOfWeek mDaysOfWeek = DaysOfWeek.All;
		private Details mDetails = null;
		private int mVoiceMailRings;
		#endregion

		#region Properties
		/// <summary>
		/// The GUID for this customer
		/// </summary>
		/// <value></value>
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
		/// The contact number for this schedule
		/// </summary>
		/// <value></value>
		public string ContactNumber
		{
			get
			{
				return this.mContactNumber;
			}
			set
			{
				this.mContactNumber = value;
			}
		}

		/// <summary>
		/// The starting time
		/// </summary>
		/// <value></value>
		public TimeSpan StartTime
		{
			get
			{
				return this.mStartTime;
			}
			set
			{
				this.mStartTime = value;
			}
		}

		/// <summary>
		/// The stopping time
		/// </summary>
		/// <value></value>
		public TimeSpan StopTime
		{
			get
			{
				return this.mStopTime;
			}
			set
			{
				this.mStopTime = value;
			}
		}

		/// <summary>
		/// The start date of the schedule
		/// </summary>
		public DateTime StartDate
		{
			get
			{
				return this.mStartDate;
			}
			set
			{
				this.mStartDate = value;
			}
		}

		/// <summary>
		/// The stop date of the schedule
		/// </summary>
		public DateTime StopDate
		{
			get
			{
				return this.mStopDate;
			}
			set
			{
				this.mStopDate = value;
			}
		}

		/// <summary>
		/// The days for which this schedule will be used
		/// </summary>
		/// <value></value>
		public DaysOfWeek Days
		{
			get
			{
				return this.mDaysOfWeek;
			}
			set
			{
				this.mDaysOfWeek = value;
			}
		}

		/// <summary>
		/// The name given to the schedule
		/// </summary>
		/// <value></value>
		public string Name
		{
			get
			{
				return this.mName;
			}
			set
			{
				this.mName = value;
			}
		}

		/// <summary>
		/// Other contact schedule details
		/// </summary>
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
		/// The number of rings before sending calls to voice mail
		/// </summary>
		public int VoiceMailRings
		{
			get
			{
				return this.mVoiceMailRings;
			}
			set
			{
				this.mVoiceMailRings = value;
			}
		}
		#endregion

		#region IXmlable Members
		/// <summary>
		/// Gets the xml representation of the Contact Schedule
		/// </summary>
		/// <value></value>
		public XmlNode Xml
		{
			get
			{
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("ContactSchedule");
				this.PopulateNode(node);
				return node;
			}		
		}
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="contactScheduleNode">The xml representation</param>
		public ContactSchedule(XmlNode contactScheduleNode) : this(string.Empty, Guid.Empty)
		{
			if (contactScheduleNode == null)
				throw new ArgumentNullException("contactScheduleNode","The Xml definition may not be null");

			this.PopulateInstance(contactScheduleNode);

		}

		/// <summary>
		/// Constructor
		/// </summary>
		public ContactSchedule(string name) : this(name, Guid.NewGuid())
		{            
		}

		/// <summary>
		/// Creates a Contact Schedule with a specific name and ID
		/// </summary>
		/// <param name="name">The name given to the schedule</param>
		/// <param name="id">The id of the schedule</param>
		internal ContactSchedule(string name, Guid id)
		{
			this.Name = name;
			this.ID = id;
			this.Details = new Details();
			this.StartTime = TimeSpan.Zero;
			this.StopTime = TimeSpan.Zero;
            this.StartDate = DateTime.MinValue;
            this.StopDate = DateTime.MaxValue;
			this.VoiceMailRings = 4;
		}

		/// <summary>
		/// Checks to see if there is overlap between schedules
		/// </summary>
		/// <param name="schedule">The schedule of interest</param>
		/// <returns>True if there is overlap between the schedules, false otherwise</returns>
		public bool OverlapsWith(ContactSchedule schedule)
		{
			if (this.StartDate <= schedule.StartDate && this.StopDate >= schedule.StopDate)
			{
				// does there exist an overlap in days
				if ((this.Days & schedule.Days) > 0)
				{
					DateTime startTime = DateTime.Today + this.StartTime;
					DateTime stopTime = DateTime.Today + this.StopTime;
					DateTime scheduleStartTime = DateTime.Today + schedule.StartTime;
					DateTime scheduleStopTime = DateTime.Today + schedule.StopTime;

					if (this.StartTime > this.StopTime) // overlaps midnight
						stopTime.AddDays(1);

					if (schedule.StartTime > schedule.StopTime) // overlaps midnight
						scheduleStopTime.AddDays(1);

					// see if the times overlap
					if (startTime <= scheduleStartTime && stopTime > scheduleStartTime || startTime < scheduleStopTime && stopTime > scheduleStopTime)
						return true;
					else if (scheduleStartTime <= startTime && scheduleStopTime > startTime || scheduleStartTime < stopTime && scheduleStopTime > stopTime)
						return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Determins whether a schedule contains a specific date and time
		/// </summary>
		/// <param name="dateTime">The date and time of interest</param>
		/// <returns>True if the schedule includes the date and time, false otherwise</returns>
		public bool Includes(DateTime dateTime)
		{
			if (this.StartDate > dateTime || this.StopDate < dateTime)
				return false;

			if (dateTime.DayOfWeek == DayOfWeek.Monday && (this.Days & DaysOfWeek.Monday) == 0)
				return false;
			else if (dateTime.DayOfWeek == DayOfWeek.Tuesday && (this.Days & DaysOfWeek.Tuesday) == 0)
				return false;
			else if (dateTime.DayOfWeek == DayOfWeek.Wednesday && (this.Days & DaysOfWeek.Wednesday) == 0)
				return false;
			else if (dateTime.DayOfWeek == DayOfWeek.Thursday && (this.Days & DaysOfWeek.Thursday) == 0)
				return false;
			else if (dateTime.DayOfWeek == DayOfWeek.Friday && (this.Days & DaysOfWeek.Friday) == 0)
				return false;
			else if (dateTime.DayOfWeek == DayOfWeek.Saturday && (this.Days & DaysOfWeek.Saturday) == 0)
				return false;
			else if (dateTime.DayOfWeek == DayOfWeek.Sunday && (this.Days & DaysOfWeek.Sunday) == 0)
				return false;

			// the day works... now see if the time is a match
			DateTime startTime = dateTime.Date+ this.StartTime;
			DateTime stopTime = dateTime.Date + this.StopTime;
			
			if (this.StartTime > this.StopTime) // overlaps midnight
				stopTime = stopTime.AddDays(1);

			if (startTime < dateTime && dateTime < stopTime)
				return true;
			else
				return false;

		}

		/// <summary>
		/// Populates a xml node with the objects data
		/// </summary>
		/// <param name="node">The  node to populate</param>
		public void PopulateNode(XmlNode node)
		{
			Inaugura.Xml.Helper.SetAttribute(node, "id", this.ID.ToString());
			Inaugura.Xml.Helper.SetAttribute(node, "name", this.Name);
			Inaugura.Xml.Helper.SetAttribute(node, "contactNumber", this.ContactNumber);
			Inaugura.Xml.Helper.SetAttribute(node, "voiceMailRings", this.VoiceMailRings.ToString());
			Inaugura.Xml.Helper.SetAttribute(node, "startTime", this.StartTime.ToString());
			Inaugura.Xml.Helper.SetAttribute(node, "stopTime", this.StopTime.ToString());

			if(this.StartDate != DateTime.MinValue)
				Inaugura.Xml.Helper.SetAttribute(node, "startDate", this.StartDate.ToUniversalTime().ToString("o", System.Globalization.CultureInfo.InvariantCulture));

			if(this.StopDate != DateTime.MaxValue)
				Inaugura.Xml.Helper.SetAttribute(node, "stopDate", this.StopDate.ToUniversalTime().ToString("o", System.Globalization.CultureInfo.InvariantCulture));

			Inaugura.Xml.Helper.SetAttribute(node, "Days", this.Days.ToString());

			if (this.Details.Count > 0)
			{
				XmlNode detailsNode = node.OwnerDocument.CreateElement("Details");
				this.Details.PopulateNode(detailsNode);
				node.AppendChild(detailsNode);
			}			
		}

		/// <summary>
		/// Populates the instance with the specifed xml data
		/// </summary>
		/// <param name="node">The xml node</param>
		public void PopulateInstance(XmlNode node)
		{
			/*
			if (node.Attributes["id"] == null)
				throw new ArgumentException("The xml does not contain a id attribute");
			 */

			if (node.Attributes["name"] == null)
				throw new ArgumentException("The xml does not contain a name attribute");

			if (node.Attributes["contactNumber"] == null)
				throw new ArgumentException("The xml does not contain a contactNumber attribute");

			if (node.Attributes["voiceMailRings"] == null)
				throw new ArgumentException("The xml does not contain a voiceMailRings attribute");

			if (node.Attributes["startTime"] == null)
				throw new ArgumentException("The xml does not contain a startTime attribute");

			if (node.Attributes["stopTime"] == null)
				throw new ArgumentException("The xml does not contain a stopTime attribute");

			if (node.Attributes["Days"] == null)
				throw new ArgumentException("The xml does not contain a Days attribute");

            this.ID = new Guid(Inaugura.Xml.Helper.GetAttribute(node, "id"));

			this.Name = Inaugura.Xml.Helper.GetAttribute(node, "name");
			this.ContactNumber = Inaugura.Xml.Helper.GetAttribute(node, "contactNumber");
			this.VoiceMailRings = int.Parse(Inaugura.Xml.Helper.GetAttribute(node, "voiceMailRings"));
			this.StartTime = TimeSpan.Parse(Inaugura.Xml.Helper.GetAttribute(node, "startTime"));
			this.StopTime = TimeSpan.Parse(Inaugura.Xml.Helper.GetAttribute(node, "stopTime"));
			this.Days = (DaysOfWeek)Enum.Parse(typeof(DaysOfWeek), Inaugura.Xml.Helper.GetAttribute(node, "Days"));

			if (node.Attributes["startDate"] != null)
				this.StartDate = DateTime.Parse(Inaugura.Xml.Helper.GetAttribute(node, "startDate"));
			else
				this.StartDate = DateTime.MinValue;

			if (node.Attributes["stopDate"] != null)
				this.StopDate = DateTime.Parse(Inaugura.Xml.Helper.GetAttribute(node, "stopDate"));
			else
				this.StopDate = DateTime.MaxValue;
			
			if (node["Details"] != null)
				this.Details = new Details(node["Details"]);
		}

        public override int GetHashCode()
        {
            int hashCode = 0;

            if (this.mContactNumber != null)
                hashCode ^= this.mContactNumber.GetHashCode();
            hashCode ^= this.mDaysOfWeek.GetHashCode();
            if (this.mDetails != null)
                hashCode ^= this.mDetails.GetHashCode();
            if (this.mID != null)
                hashCode ^= this.mID.GetHashCode();
            if (this.mName != null)
                hashCode ^= this.mName.GetHashCode();
            if (this.mStartDate != null)
                hashCode ^= this.mStartDate.GetHashCode();
            if (this.mStartTime != null)
                hashCode ^= this.mStartTime.GetHashCode();
            if (this.mStopDate != null)
                hashCode ^= this.mStopDate.GetHashCode();
            if (this.mStopTime != null)
                hashCode ^= this.mStopTime.GetHashCode();
            hashCode ^= this.mVoiceMailRings.GetHashCode();

            return hashCode;
        }

		#region static constructors
	
		#endregion
	}
}
