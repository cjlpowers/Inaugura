using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	public class Room : Inaugura.Xml.IXmlable
	{
		#region Variables
        private Guid mID;
		private string mName;
		private string mDescription;
		private Dimensions mDimensions;
		private RoomType mRoomType;
		private StringCollection mFeatures;
		private FloorMaterial mFloorMaterial;
		#endregion

		#region Properties
        /// <summary>
        /// The GUID of the room
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
		/// The name of the room
		/// </summary>
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
		/// The description of the room
		/// </summary>
		public string Description
		{
			get
			{
				return this.mDescription;
			}
			set
			{
				this.mDescription = value;
			}
		}

		/// <summary>
		/// The dimensions of the room
		/// </summary>
		public Dimensions Dimensions
		{
			get
			{
				return this.mDimensions;
			}
			set
			{
				this.mDimensions = value;
			}
		}

		/// <summary>
		/// The room type
		/// </summary>
		public RoomType Type
		{
			get
			{
				return this.mRoomType;
			}
			set
			{
				this.mRoomType = value;
			}
		}

		/// <summary>
		/// The features of the room
		/// </summary>
		public StringCollection Features
		{
			get
			{
				return this.mFeatures;
			}
			set
			{
				this.mFeatures = value;
			}
		}

		/// <summary>
		/// The flooring in the room
		/// </summary>
		public FloorMaterial FloorMaterial
		{
			get
			{
				return this.mFloorMaterial;
			}
			set
			{
				this.mFloorMaterial = value;
			}
		}
		#endregion

		#region IXmlable Members
        /// <summary>
        /// Gets the xml representation of this instance
        /// </summary>
		public XmlNode Xml
		{
			get
			{
				XmlNode roomNode = Inaugura.Xml.Helper.NewNodeDocument("Room");
				this.PopulateNode(roomNode);
				return roomNode;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="node">The xml node which defines the room</param>
		public Room(XmlNode node) : this(string.Empty, RoomType.NotSpecified)
		{
			this.PopulateInstance(node);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">The name of the room</param>
		/// <param name="type">The room type</param>
		public Room(string name, RoomType type)
		{
            this.ID = Guid.NewGuid();
			this.Name = name;
			this.Dimensions = new Dimensions();
			this.Features = new StringCollection();
			this.Type = type;
			this.FloorMaterial = FloorMaterial.NotSpecified;
		}

        /// <summary>
        /// Constructor
        /// </summary>
        public Room()
            : this(string.Empty, RoomType.NotSpecified)
        {
        }

		/// <summary>
		/// Populates a xml node with the objects data
		/// </summary>
		/// <param name="node">The  node to populate</param>
		public virtual void PopulateNode(XmlNode node)
		{
			//Inaugura.Xml.Helper.SetTypeAttribute(node, this.GetType());
			//Inaugura.Xml.Helper.SetAssemblyAttribute(node, this.GetType());

            Inaugura.Xml.Helper.SetAttribute(node, "id", this.ID.ToString());
			Inaugura.Xml.Helper.SetAttribute(node, "name", this.Name);

            if (this.Type != RoomType.NotSpecified)
            {
                XmlNode roomTypeNode = node.OwnerDocument.CreateElement("RoomType");
                this.mRoomType.PopulateNode(roomTypeNode);
                node.AppendChild(roomTypeNode);
            }

            if (this.Description != string.Empty)
            {
                XmlNode descriptionNode = node.OwnerDocument.CreateElement("Description");
                descriptionNode.InnerText = this.Description;
                node.AppendChild(descriptionNode);
            }

            if (this.Dimensions.Width.Value != 0)
            {
                XmlNode dimensionsNode = node.OwnerDocument.CreateElement("Dimensions");
                this.Dimensions.PopulateNode(dimensionsNode);
                node.AppendChild(dimensionsNode);                
            }

            if (this.FloorMaterial != FloorMaterial.NotSpecified)
            {
                XmlNode floorMaterialNode = node.OwnerDocument.CreateElement("FloorMaterial");
                this.FloorMaterial.PopulateNode(floorMaterialNode);
                node.AppendChild(floorMaterialNode);
            }

            if (Features.Count > 0)
            {
                XmlNode featuresNode = node.OwnerDocument.CreateElement("Features");
                this.Features.PopulateNode(featuresNode);
                node.AppendChild(featuresNode);
            }
		}

		/// <summary>
		/// Populates the instance with the specifed xml data
		/// </summary>
		/// <param name="node">The xml node</param>
		public virtual void PopulateInstance(XmlNode node)
		{
            if (node.Attributes["id"] != null)
                this.ID = new Guid(node.Attributes["id"].Value);

			if (node.Attributes["name"] == null)
				throw new ArgumentException("The xml does not contain a name attribute");

            if (node["RoomType"] != null)
                this.Type = RoomType.FromXml(node["RoomType"]);
			else
				this.Type = RoomType.NotSpecified;
	
			this.Name = Inaugura.Xml.Helper.GetAttribute(node, "name");
            
            if (node["Description"] != null)
                this.Description = node["Description"].InnerText;
            else
                this.Description = string.Empty;

            if (node["Dimensions"] != null)
                this.Dimensions = new Dimensions(node["Dimensions"]);
            else
                this.Dimensions = new Dimensions();
            
            if (node["Features"] != null)
                this.Features = new StringCollection(node["Features"]);
            else
                this.Features = new StringCollection();

            if (node["FloorMaterial"] != null)
                this.FloorMaterial = FloorMaterial.FromXml(node["FloorMaterial"]);
            else
                this.FloorMaterial = FloorMaterial.NotSpecified;
		}

        public override string ToString()
        {
            return this.Name;
        }
		#endregion
	}
}
