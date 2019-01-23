using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// An abstract class which represents a residential property listing
	/// </summary>
	public abstract class ResidentialPropertyListing : PropertyListing
	{
		#region Variables
		private Gararge mGarage;
		private LevelCollection mLevels;
		private bool mPool;
		private bool mFireplace;
		private bool mWaterfront;
		private bool mBackyardNeighbors;
        private StringCollection mAppliances;
		#endregion

		#region Properties
		/// <summary>
		/// The gararge 
		/// </summary>
		public Gararge Gararge
		{
			get
			{
				return this.mGarage;
			}
			set
			{
				this.mGarage = value;
			}
		}

		/// <summary>
		/// The collection of levels used
		/// </summary>
		public LevelCollection Levels
		{
			get
			{
				return this.mLevels;
			}
			set
			{
				this.mLevels = value;
			}
		}

		/// <summary>
		/// Flags whether the property has a pool
		/// </summary>
		public bool Pool
		{
			get
			{
				return this.mPool;
			}
			set
			{
				this.mPool = value;
			}
		}

		/// <summary>
		/// Flags whether the property hace a fireplace
		/// </summary>
		public bool Fireplace
		{
			get
			{
				return this.mFireplace;
			}
			set
			{
				this.mFireplace = value;
			}
		}

		/// <summary>
		/// Flags weather the property is on land considered to be waterfront
		/// </summary>
		public bool Waterfront
		{
			get
			{
				return this.mWaterfront;
			}
			set
			{
				this.mWaterfront = value;
			}
		}

		/// <summary>
		/// Flags weather the property has backyard neighbors
		/// </summary>
		public bool BackyardNeighbors
		{
			get
			{
				return this.mBackyardNeighbors;
			}
			set
			{
				this.mBackyardNeighbors = value;
			}
		}


		/// <summary>
		/// The flooring material(s)
		/// </summary>
		public FloorMaterial[] Flooring
		{
			get
			{
				List<FloorMaterial> flooringMaterials = new List<FloorMaterial>();
				foreach (Level level in this.Levels)
				{
					foreach (Room room in level.Rooms)
					{
						if (room.FloorMaterial != FloorMaterial.NotSpecified && !flooringMaterials.Contains(room.FloorMaterial))
							flooringMaterials.Add(room.FloorMaterial);
					}
				}
				return flooringMaterials.ToArray();
			}
		}

        /// <summary>
        /// The number of bedrooms
        /// </summary>
        public int NumberOfBedrooms
        {
            get
            {
                return this.GetRoomCount(RoomType.Bedroom);
            }
        }

        /// <summary>
        /// The number of bed rooms
        /// </summary>
        public int NumberOfBathrooms
        {
            get
            {
                return this.GetRoomCount(RoomType.Bathroom);
            }
        }

        /// <summary>
        /// The Appliances
        /// </summary>
        public StringCollection Appliances
        {
            get
            {
                return this.mAppliances;
            }
            set
            {
                this.mAppliances = value;
            }
        }
		#endregion

		#region Methods
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="node">The xml which defines the instance</param>
		public ResidentialPropertyListing(XmlNode node) : this()
		{
			this.PopulateInstance(node);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public ResidentialPropertyListing()
		{			
			this.Levels = new LevelCollection();
            this.Appliances = new StringCollection();
		}

        /// <summary>
        /// Gets the count of a specific room type
        /// </summary>
        /// <param name="roomType">The type of room</param>
        /// <returns>The count of the specific room type</returns>
        public int GetRoomCount(RoomType roomType)
        {
            int count = 0;
            foreach (Level level in this.Levels)
            {
                foreach (Room room in level.Rooms)
                {
                    if (room.Type == roomType)
                        count++;
                }
            }
            return count;
        }

		/// <summary>
		/// Populates the instance with data from xml
		/// </summary>
		/// <param name="node"></param>
		public override void PopulateInstance(XmlNode node)
		{
			base.PopulateInstance(node);

			if (node["Levels"] == null)
				throw new ArgumentException("The xml does not contains a Levels node");

			if (node["Gararge"] != null)
				this.Gararge = new Gararge(node["Gararge"]);

			if (node.Attributes["pool"] != null)
				this.Pool = bool.Parse(Inaugura.Xml.Helper.GetAttribute(node, "pool"));

			if (node.Attributes["fireplace"] != null)
				this.Fireplace = bool.Parse(Inaugura.Xml.Helper.GetAttribute(node, "fireplace"));

			if(node.Attributes["waterfront"] != null)
				this.Waterfront = bool.Parse(Inaugura.Xml.Helper.GetAttribute(node, "waterfront"));

			if (node.Attributes["backyardNeighbors"] != null)
				this.BackyardNeighbors = bool.Parse(Inaugura.Xml.Helper.GetAttribute(node, "backyardNeighbors"));

			this.Levels = new LevelCollection(node["Levels"]);

            if (node["Appliances"] != null)
                this.Appliances = new StringCollection(node["Appliances"]);
		}

		/// <summary>
		/// Populates a xml node with the property data
		/// </summary>
		/// <param name="node">The xml node to populate</param>
		public override void PopulateNode(XmlNode node)
		{
			base.PopulateNode(node);

            Inaugura.Xml.Helper.SetAttribute(node, "numberOfBedrooms", this.NumberOfBedrooms.ToString());
            Inaugura.Xml.Helper.SetAttribute(node, "numberOfBathrooms", this.NumberOfBathrooms.ToString());            

			if(this.Gararge != null && this.Gararge.ParkingSpaces != 0)
			{
				XmlNode garargeNode = node.OwnerDocument.CreateElement("Gararge");
				this.Gararge.PopulateNode(garargeNode);
				node.AppendChild(garargeNode);
			}

			XmlNode levelsNode = node.OwnerDocument.CreateElement("Levels");
			this.Levels.PopulateNode(levelsNode);
			node.AppendChild(levelsNode);

			if (this.Pool)
				Inaugura.Xml.Helper.SetAttribute(node, "pool", this.Pool.ToString());

			if (this.Fireplace)
				Inaugura.Xml.Helper.SetAttribute(node, "fireplace", this.Fireplace.ToString());

			if (this.Waterfront)
				Inaugura.Xml.Helper.SetAttribute(node, "waterfront", this.Waterfront.ToString());

			if (this.BackyardNeighbors)
				Inaugura.Xml.Helper.SetAttribute(node, "backyardNeighbors", this.BackyardNeighbors.ToString());

            if (this.Appliances.Count > 0)
            {
                XmlNode appliancesNode = node.OwnerDocument.CreateElement("Appliances");
                this.Appliances.PopulateNode(appliancesNode);
                node.AppendChild(appliancesNode);
            }			
		}


        /// <summary>
        /// Updates the listing
        /// </summary>
        /// <param name="api">The api instance to user to perform the operation</param>
        public void Update(RealLeadsAPI api)
        {
            this.EnforceEditPolicy();
            api.ListingManager.UpdateListing(this);
        }

        /// <summary>
        /// Removes a room with the specified ID
        /// </summary>
        /// <param name="roomID">The room ID</param>
        public void RemoveRoom(Guid roomID)
        {
            this.EnforceEditPolicy();
            foreach (Level level in this.mLevels)
            {
                Room room = level.Rooms[roomID];
                if (room != null)
                {
                    level.Rooms.Remove(room);
                    return;
                }
            }
        }
		#endregion
	}
}
