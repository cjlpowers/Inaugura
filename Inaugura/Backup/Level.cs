using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// A class representing a level
	/// </summary>
	public class Level : Inaugura.Xml.IXmlable
	{
		#region Variables
        private Guid mID;
		private string mName;
		private string mDescription;
        private Inaugura.Measurement.Measurement mSize;
		private bool mAboveGrade;
		private StringCollection mFeatures;
		string mHeating;
		private RoomCollection mRooms;
		#endregion

		#region IXmlable Members
		/// <summary>
		/// The xml representation of this instance
		/// </summary>
		public XmlNode Xml
		{
			get 
			{
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Level");
				this.PopulateNode(node);
				return node;
			}
		}
		#endregion

		#region Properties
        /// <summary>
        /// The GUID of the level
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
		/// The name of the floor
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
		/// The description of the level
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
		/// The size of the level
		/// </summary>
        public Inaugura.Measurement.Measurement Size
		{
			get
			{
				return this.mSize;
			}
			set
			{
				this.mSize = value;
			}
		}

		/// <summary>
		/// True if the level is above grade, false otherwise
		/// </summary>
		public bool AboveGrade
		{
			get
			{
				return this.mAboveGrade;
			}
			set
			{
				this.mAboveGrade = value;
			}
		}

		/// <summary>
		/// The features of the level
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
		/// The heating details
		/// </summary>
		public string Heating
		{
			get
			{
				return this.mHeating;
			}
			set
			{
				this.mHeating = value;
			}
		}

		/// <summary>
		/// The rooms which are located on the level
		/// </summary>
		public RoomCollection Rooms
		{
			get
			{
				return this.mRooms;
			}
			set
			{
				this.mRooms = value;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Constructor
		/// </summary>
		public Level(string name)
		{
            this.ID = Guid.NewGuid();
			this.Description = string.Empty;
			this.Name = name;
			this.Features = new StringCollection();
            this.Size = new Inaugura.Measurement.Measurement(0, Inaugura.Measurement.Unit.SquareFeet);
			this.Rooms = new RoomCollection();            
		}

        /// <summary>
        /// Constructor
        /// </summary>
        public Level()
            : this(string.Empty)
        {
            this.Size = new Measurement.Measurement(0, Inaugura.Measurement.Unit.SquareFeet);
        }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="node">The xml node which defines this instance</param>
		public Level(XmlNode node) : this(string.Empty)
		{
			this.PopulateInstance(node);
		}

		/// <summary>
		/// Populates a xml node with the objects data
		/// </summary>
		/// <param name="node">The  node to populate</param>
		public virtual void PopulateNode(XmlNode node)
		{
            Inaugura.Xml.Helper.SetAttribute(node, "id", this.ID.ToString());
			Inaugura.Xml.Helper.SetAttribute(node, "name", this.Name);
			Inaugura.Xml.Helper.SetAttribute(node, "aboveGrade", this.AboveGrade.ToString());
		
			if(this.Heating != null && this.Heating != string.Empty)
				Inaugura.Xml.Helper.SetAttribute(node, "heating", this.Heating);

            XmlNode descriptionNode = node.OwnerDocument.CreateElement("Description");
            descriptionNode.InnerText = this.Description;
            node.AppendChild(descriptionNode);

            if (this.Size.Value != 0)
            {
                XmlNode sizeNode = node.OwnerDocument.CreateElement("Size");
                this.Size.PopulateNode(sizeNode);
                node.AppendChild(sizeNode);
            }

			XmlNode featuresNode = node.OwnerDocument.CreateElement("Features");
			this.Features.PopulateNode(featuresNode);
			node.AppendChild(featuresNode);

			if (this.Rooms.Count > 0)
			{
				XmlNode roomsNode = node.OwnerDocument.CreateElement("Rooms");
				this.Rooms.PopulateNode(roomsNode);
				node.AppendChild(roomsNode);
			}
		}

		/// <summary>
		/// Populates the instance with the specifed xml data
		/// </summary>
		/// <param name="node">The xml node</param>
		public virtual void PopulateInstance(XmlNode node)
		{
            this.ID = new Guid(Inaugura.Xml.Helper.GetAttribute(node, "id"));

			if (node.Attributes["name"] == null)
				throw new ArgumentException("The xml does not contain a name attribute");

			if (node.Attributes["aboveGrade"] == null)
				throw new ArgumentException("The xml does not contain a aboveGrade attribute");

            if (node["Description"] != null)
                this.Description = node["Description"].InnerText;
						
			if (node["Features"] == null)
				throw new ArgumentException("The xml does not contain the details node");

			this.Name = Inaugura.Xml.Helper.GetAttribute(node, "name");
			this.AboveGrade = bool.Parse(Inaugura.Xml.Helper.GetAttribute(node, "aboveGrade"));

			if (node.Attributes["heating"] != null)
				this.Heating = Inaugura.Xml.Helper.GetAttribute(node, "heating");

            XmlNode sizeNode = node["Size"];
            if (sizeNode != null)
                this.Size = new Inaugura.Measurement.Measurement(sizeNode);

			this.Features = new StringCollection(node["Features"]);

			if (node["Rooms"] != null)
				this.Rooms = new RoomCollection(node["Rooms"]);

		}

        public override string ToString()
        {
            return this.Name;
        }
		#endregion
	}
}
