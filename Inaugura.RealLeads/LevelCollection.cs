using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
    /// <summary>
    /// A collection of levels
    /// </summary>
	public class LevelCollection : System.Collections.CollectionBase, Inaugura.Xml.IXmlable
	{
		#region Variables
		#endregion

		#region Accessors
		/// <summary>
		/// Returns the level at the specified zero based index
		/// </summary>
		/// <param name="index">The zero based index into the collection</param>
		/// <returns>The level at the specified index</returns>
		public Level this[int index]
		{
			get
			{
				return this.List[index] as Level;
			}
		}

        /// <summary>
        /// Returns the level with a specific ID
        /// </summary>
        /// <param name="id">The ID of the level</param>
        /// <returns>The level with the specified ID</returns>
        [Obsolete()]
        public Level this[string id]
        {
            get
            {
                Guid guid = new Guid(id);
                return this[guid];
            }
        }

        /// <summary>
        /// Returns the level with a specific ID
        /// </summary>
        /// <param name="id">The ID of the level</param>
        /// <returns>The level with the specified ID</returns>
        public Level this[Guid id]
        {
            get
            {
                foreach (Level level in this)
                {
                    if (level.ID == id)
                        return level;
                }
                return null;
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
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Levels");
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
		public LevelCollection(XmlNode node)
		{
			this.PopulateInstance(node);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public LevelCollection()
		{
		}

		/// <summary>
		/// Adds a level to the collection
		/// </summary>
		/// <param name="level">The level to add</param>
		public void Add(Level level)
		{
			this.List.Add(level);
		}

		/// <summary>
		/// Removes a level from the collection
		/// </summary>
		/// <param name="level">The level to remove</param>
		public void Remove(Level level)
		{
			this.List.Remove(level);
		}

        /// <summary>
        /// Determins wheather the collection contains a level
        /// </summary>
        /// <param name="level">The level</param>
        /// <returns>True if the collection contains the level, false otherwise</returns>
        public bool Contains(Level level)
        {
            return this.List.Contains(level);
        }

        /// <summary>
        /// Gets a level which contains a specific room
        /// </summary>
        /// <param name="roomID">The id of the room</param>
        /// <returns>The level containing the room, otherwise null</returns>
        public Level GetLevelByRoom(Guid roomID)
        {
            foreach (Level level in this)
            {
                if (level.Rooms[roomID] != null)
                    return level;
            }
            return null;            
        }

        /// <summary>
        /// Moves a level up in the list
        /// </summary>
        /// <param name="levelID">The ID of the level</param>
        public void MoveUp(string levelID)
        {
            Level r = this[levelID];
            if (r != null)
                this.MoveUp(r);
        }

        /// <summary>
        /// Moves a level up in the list
        /// </summary>
        /// <param name="level">The level to move</param>
        public void MoveUp(Level level)
        {
            if (level == null)
                return;

            lock (this.List)
            {
                if (this.List.Contains(level))
                {
                    int index = this.List.IndexOf(level);
                    if (index != 0)
                    {
                        index--;
                        this.List.Remove(level);
                        this.List.Insert(index, level);
                    }
                }
            }
        }

        /// <summary>
        /// Moves a level down in the list
        /// </summary>
        /// <param name="levelID">The level ID</param>
        public void MoveDown(string levelID)
        {
            Level r = this[levelID];
            if (r != null)
                this.MoveDown(r);
        }

        /// <summary>
        /// Moves a room down in the list
        /// </summary>
        /// <param name="level">The level to move</param>
        public void MoveDown(Level level)
        {
            if (level == null)
                return;
            lock (this.List)
            {
                if (this.List.Contains(level))
                {
                    int index = this.List.IndexOf(level);
                    if (index != this.List.Count - 1)
                    {
                        index++;
                        this.List.Remove(level);
                        this.List.Insert(index, level);
                    }
                }
            }
        }

        /// <summary>
        /// Determins the index of a specific level in the list
        /// </summary>
        /// <param name="level">The level</param>
        /// <returns>The index</returns>
        public int IndexOf(Level level)
        {
            lock (this.List)
            {
                 return this.List.IndexOf(level);
            }
        }

		/// <summary>
		/// Populates a xml node with the objects data
		/// </summary>
		/// <param name="node">The  node to populate</param>
		public virtual void PopulateNode(XmlNode node)
		{
			foreach (Level level in this)
			{
				XmlNode levelNode = node.OwnerDocument.CreateElement("Level");
				level.PopulateNode(levelNode);
				node.AppendChild(levelNode);
			}
		}

		/// <summary>
		/// Populates the instance with the specifed xml data
		/// </summary>
		/// <param name="node">The xml node</param>
		public virtual void PopulateInstance(XmlNode node)
		{
			XmlNodeList nodes = node.SelectNodes("Level");
			foreach (XmlNode levelNode in nodes)
			{
				Level level = new Level(levelNode);
				this.Add(level);
			}			
		}
		#endregion			
	}
}
