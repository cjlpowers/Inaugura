using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	public class RoomCollection : System.Collections.CollectionBase, Inaugura.Xml.IXmlable
	{
		#region Variables
		#endregion

		#region Accessors
		/// <summary>
		/// Returns the room at the specified zero based index
		/// </summary>
		/// <param name="index">The zero based index into the collection</param>
		/// <returns>The room at the specified index</returns>
		public Room this[int index]
		{
			get
			{
                lock (this.List)
                {
                    return this.List[index] as Room;
                }
			}
		}

        /// <summary>
        /// Returns the room with a specific ID
        /// </summary>
        /// <param name="id">The ID of the room</param>
        /// <returns>The room with the specified ID</returns>
        public Room this[Guid id]
        {
            get
            {
                lock (this.List)
                {
                    foreach (Room room in this)
                    {
                        if (room.ID == id)
                            return room;
                    }
                }
                return null;
            }
        }
		#endregion

		#region Properties
		/// <summary>
		/// The total floor area of all rooms in the collection
		/// </summary>
        public Inaugura.Measurement.Measurement Area
		{
			get
			{
                Measurement.Measurement sum = new Inaugura.Measurement.Measurement(0, Inaugura.Measurement.Unit.SquareMeter);
				foreach (Room room in this)
				{
                    sum += room.Dimensions.Area;
				}
                return sum;
			}
		}
		#endregion

		#region IXmlable Members
		/// <summary>
		/// The xml representation of the the instance
		/// </summary>
		public XmlNode Xml
		{
			get
			{
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Rooms");
				this.PopulateNode(node);
				return node;
			}
		}
		#endregion

		#region Method
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="node">The xml node which represents the instance</param>
		public RoomCollection(XmlNode node)
		{
			this.PopulateInstance(node);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public RoomCollection()
		{
		}

		/// <summary>
		/// Adds a room to the collection
		/// </summary>
		/// <param name="level">The room to add</param>
		public void Add(Room room)
		{
            lock (this.List)
            {
                this.List.Add(room);
            }
		}

		/// <summary>
		/// Removes a room from the collection
		/// </summary>
		/// <param name="level">The room to remove</param>
		public void Remove(Room room)
		{
            lock (this.List)
            {
                this.List.Remove(room);
            }
		}

        /// <summary>
        /// Moves a room up in the list
        /// </summary>
        /// <param name="roomID">The ID of the room</param>
        public void MoveUp(Guid roomID)
        {            
                Room r = this[roomID];
                if (r != null)
                    this.MoveUp(r);          
        }

        /// <summary>
        /// Moves a room up in the list
        /// </summary>
        /// <param name="room">The room to move</param>
        public void MoveUp(Room room)
        {
            if (room == null)
                return;

            lock (this.List)
            {
                if (this.List.Contains(room))
                {
                    int index = this.List.IndexOf(room);
                    if (index != 0)
                    {
                        index--;
                        this.List.Remove(room);
                        this.List.Insert(index, room);
                    }
                }
            }
        }

        /// <summary>
        /// Moves a room down in the list
        /// </summary>
        /// <param name="roomID"></param>
        public void MoveDown(Guid roomID)
        {
            Room r = this[roomID];
            if (r != null)
                this.MoveDown(r);          
        }

        /// <summary>
        /// Moves a room down in the list
        /// </summary>
        /// <param name="room">The room to move</param>
        public void MoveDown(Room room)
        {
            if (room == null)
                return;
            lock (this.List)
            {
                if (this.List.Contains(room))
                {
                    int index = this.List.IndexOf(room);
                    if (index != this.List.Count - 1)
                    {
                        index++;
                        this.List.Remove(room);
                        this.List.Insert(index, room);
                    }
                }
            }
        }

        /// <summary>
        /// Determins the index of a specific room in the list
        /// </summary>
        /// <param name="room">The room</param>
        /// <returns>The index</returns>
        public int IndexOf(Room room)
        {
            lock (this.List)
            {
                return this.List.IndexOf(room);
            }
        }

		/// <summary>
		/// Populates a xml node with the objects data
		/// </summary>
		/// <param name="node">The  node to populate</param>
		public virtual void PopulateNode(XmlNode node)
		{
			foreach (Room room in this)
			{
                if(this.Count > 0)
                    Inaugura.Xml.Helper.SetAttribute(node, "count", this.Count.ToString());

				XmlNode roomNode = node.OwnerDocument.CreateElement("Room");
				room.PopulateNode(roomNode);
				node.AppendChild(roomNode);
			}
		}

		/// <summary>
		/// Populates the instance with the specifed xml data
		/// </summary>
		/// <param name="node">The xml node</param>
		public virtual void PopulateInstance(XmlNode node)
		{
			XmlNodeList nodes = node.SelectNodes("Room");
			foreach (XmlNode roomNode in nodes)
			{
				Room room = new Room(roomNode);
				this.Add(room);
			}			
		}
		#endregion			
	}
}
