#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#endregion

namespace Inaugura.RealLeads
{
	public class ContactScheduleCollection : System.Collections.CollectionBase, Inaugura.Xml.IXmlable
	{
		#region Variables	
		#endregion

		#region Indexers
		/// <summary>
		/// Gets/Sets a ContactSchedule at a specific index
		/// </summary>
		/// <param name="index">The zero based index</param>
		/// <returns>The ContactSchedule at the index</returns>
		public ContactSchedule this[int index]
		{
			get
			{
				return (ContactSchedule)this.List[index];
			}
			set
			{
				this.RemoveAt(index);
				this.Insert(index, value);				
			}
		}

		/// <summary>
		/// Gets the Contact Schedule at a specific index
		/// </summary>
		/// <param name="id">The ID of the contact schedule</param>
		/// <returns>The contact schedule with the specified ID, otherwise null</returns>
		public ContactSchedule this[Guid id]
		{
			get
			{
				foreach (ContactSchedule schedule in this)
				{
					if (schedule.ID == id)
						return schedule;
				}

				// no schedule was found
				return null;
			}
		}

		/// <summary>
		/// Gets the contact schedule(s) for a specific date and time
		/// </summary>
		/// <param name="dateTime">The date and time</param>
		/// <returns>Returns the contact schedule(s) which contain the specified date and time, otherwise null</returns>
		public virtual ContactSchedule[] this[DateTime dateTime]
		{
			get
			{
                List<ContactSchedule> validSchedules = new List<ContactSchedule>();
				foreach (ContactSchedule schedule in this)
				{
                    if (schedule.Includes(dateTime))
                        validSchedules.Add(schedule);
				}
                return validSchedules.ToArray();
			}
		}
		#endregion

		#region Properties		
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
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("ContactSchedules");
				this.PopulateNode(node);
				return node;
			}			
		}
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="contactScheduleNode">The xml representation</param>
		public ContactScheduleCollection(XmlNode contactSchedulesNode) : this()
		{
			if (contactSchedulesNode == null)
				throw new ArgumentNullException("contactSchedulesNode", "The Xml definition may not be null");

			foreach(XmlNode node in contactSchedulesNode.SelectNodes("ContactSchedule"))
			{
				ContactSchedule schedule = new ContactSchedule(node);
				this.List.Add(schedule);
			}			
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public ContactScheduleCollection()
		{			
		}

		/// <summary>
		/// Populates a xml node with the objects data
		/// </summary>
		/// <param name="node">The  node to populate</param>
		public virtual void PopulateNode(XmlNode node)
		{
			foreach (ContactSchedule schedule in this)
			{
				XmlNode scheduleNode = node.OwnerDocument.CreateElement("ContactSchedule");
				schedule.PopulateNode(scheduleNode);
				node.AppendChild(scheduleNode);
			}
		}

		/// <summary>
		/// Populates the instance with the specifed xml data
		/// </summary>
		/// <param name="node">The xml node</param>
		public virtual void PopulateInstance(XmlNode node)
		{
			this.Clear();
			XmlNodeList nodes = node.SelectNodes("ContactSchedules");
			foreach (XmlNode scheduleNode in nodes)
			{
				ContactSchedule schedule = new ContactSchedule(scheduleNode);
				this.Add(schedule);
			}
		}
		
		/// <summary>
		/// Inserts a ContactSchedule into the collection
		/// </summary>
		/// <param name="index">The index where the ContactSchedule will inserted</param>
		/// <param name="schedule">The ContactSchedule</param>
		public void Insert(int index, ContactSchedule schedule)
		{
            /* Dont worry about overlapping
			// make sure the schedule being added does not overlap
			foreach (ContactSchedule s in this)
			{
                if (s.OverlapsWith(schedule))
                {
                    // only allow conflicting schedules to be added if they have a limited date scope                    
                    if(schedule.StartDate != DateTime.MinValue)
                        throw new ScheduleOverlapException("The schedule being added overlaps an existing schedule in the collection");
                }
			}
            */
			this.List.Insert(index, schedule);		
		}

		/// <summary>
		/// Adds a ContactSchedule to the collection
		/// </summary>
		/// <param name="schedule">The ContactSchedule</param>
		public void Add(ContactSchedule schedule)
		{
			this.Insert(this.Count, schedule);
		}

		/// <summary>
		/// Removes a ContactSchedule from the collection
		/// </summary>
		/// <param name="schedule"></param>
		public void Remove(ContactSchedule schedule)
		{
			int index = this.List.IndexOf(schedule);
			this.RemoveAt(index);
		}

		/// <summary>
		/// Determins if the collection contains a specific schedule
		/// </summary>
		/// <param name="schedule">The schedule</param>
		/// <returns>True if the collection contains the schedule, false otherwise</returns>
		public bool Contains(ContactSchedule schedule)
		{
			return this.List.Contains(schedule);
		}

        /// <summary>
        /// Returns the a overlapping schedule
        /// </summary>
        /// <param name="schedule">The schedule to test for overlap</param>
        /// <returns>The schedule in the collection which overlaps with the schedule of interest, otherwise null</returns>
        public ContactSchedule[] GetOverlappingSchedules(ContactSchedule schedule)
        {
            List<ContactSchedule> overlappingSchedules = new List<ContactSchedule>();
            foreach (ContactSchedule s in this)
            {
                if (s.ID != schedule.ID)
                    if (s.OverlapsWith(schedule))
                        overlappingSchedules.Add(s);
            }
            return overlappingSchedules.ToArray();
        }

        /// <summary>
        /// Tests if an existing schedule in the collection overlaps with a schedule of interest
        /// </summary>
        /// <param name="schedule">The schedule of interest</param>
        /// <returns>True if a schedule overlaps otherwise false</returns>
        public bool Overlaps(ContactSchedule schedule)
        {
            return this.GetOverlappingSchedules(schedule).Length > 0;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            foreach (ContactSchedule schedule in this)
            {
                hashCode ^= schedule.GetHashCode();
            }
            return hashCode;
        }
	}
}
