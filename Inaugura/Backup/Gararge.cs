using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// A class representing a garage
	/// </summary>
	public class Gararge: Inaugura.Xml.IXmlable
	{
		#region Variables
		private bool mAttached;
		private string mDescription;
		private Dimensions mDimensions;
		private StringCollection mFeatures;
		private int mParkingSpaces;
		#endregion

		#region Properties
		/// <summary>
		/// The attached state of the gararge. True if the gararge is attached to the building, false otherwise.
		/// </summary>
		public bool Attached
		{
			get
			{
				return this.mAttached;
			}
			set
			{
				this.mAttached = value;
			}
		}

		/// <summary>
		/// The description of the garage
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
		/// The dimensions of the gararage
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
		/// The features of the garage
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
		/// The number of vehicles which can fit into the gararge
		/// </summary>
		public int ParkingSpaces
		{
			get
			{
				return this.mParkingSpaces;
			}
			set
			{
				this.mParkingSpaces = value;
			}
		}					
		#endregion

		#region IXmlable Members
		public System.Xml.XmlNode Xml
		{
			get 
			{
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Gararge");
				this.PopulateNode(node);
				return node;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="node">The xml node which defines the gararge</param>
		public Gararge(XmlNode node)
		{
			this.PopulateInstance(node);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public Gararge() : this(false, 0,new Dimensions(),new StringCollection())
		{			
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="attached">True if the gararge is attached to the building, false otherwise</param>
		/// <param name="parkingSpaces">The number of vehicles the gararge can hold</param>
		/// <param name="dimensions">The dimensions of the garage</param>
		/// <param name="features">The features of the gararge</param>
		public Gararge(bool attached, int parkingSpaces, Dimensions dimensions, StringCollection features)
		{
			this.Attached = attached;
			this.ParkingSpaces = parkingSpaces;
			this.Dimensions = dimensions;
			this.Features = features;
		}

		/// <summary>
		/// Populates a xml node with the objects data
		/// </summary>
		/// <param name="node">The  node to populate</param>
		public void PopulateNode(XmlNode node)
		{
			if (Attached)
				Inaugura.Xml.Helper.SetAttribute(node, "attached", this.Attached.ToString());

			Inaugura.Xml.Helper.SetAttribute(node, "parkingSpaces", this.ParkingSpaces.ToString());

			XmlNode descriptionNode = node.OwnerDocument.CreateElement("Description");
			descriptionNode.InnerText = this.Description;
			node.AppendChild(descriptionNode);

			XmlNode dimensionsNode = node.OwnerDocument.CreateElement("Dimensions");
			Inaugura.Xml.Helper.SetAttribute(dimensionsNode, "areaStdUnit", this.Dimensions.Area.ToStandardUnit().Value.ToString());
			this.Dimensions.PopulateNode(dimensionsNode);
			node.AppendChild(dimensionsNode);

			XmlNode featuresNode = node.OwnerDocument.CreateElement("Features");
			this.Features.PopulateNode(featuresNode);
			node.AppendChild(featuresNode);
		}

		/// <summary>
		/// Populates the instance with the specifed xml data
		/// </summary>
		/// <param name="node">The xml node</param>
		public void PopulateInstance(XmlNode node)
		{
			if (node.Attributes["attached"] != null)
				this.Attached = bool.Parse(Inaugura.Xml.Helper.GetAttribute(node, "attached"));

			if (node.Attributes["parkingSpaces"] == null)
				throw new ArgumentException("The xml does not contain a parkingSpaces attribute");
					
			if (node["Description"] == null)
				throw new ArgumentException("The xml does not contain the Description node");

			if (node["Dimensions"] == null)
				throw new ArgumentException("The xml does not contain the Dimensions node");

			if (node["Features"] == null)
				throw new ArgumentException("The xml does not contain the Features node");

			this.ParkingSpaces = int.Parse(Inaugura.Xml.Helper.GetAttribute(node, "parkingSpaces"));			
			this.Description = node["Description"].InnerText;
			this.Features = new StringCollection(node["Features"]);
			this.Dimensions = new Dimensions(node["Dimensions"]);
		}
		#endregion
	}
}
