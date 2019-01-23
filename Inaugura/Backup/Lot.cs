using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// A class representing a Lot
	/// </summary>
	public class Lot : Inaugura.Xml.IXmlable
	{
        /// <summary>
        /// Area units used in measuring lots
        /// </summary>
        public static Inaugura.Measurement.Unit.UnitCollection AreaUnits = new Inaugura.Measurement.Unit.UnitCollection(new Inaugura.Measurement.Unit[] { Inaugura.Measurement.Unit.SquareFeet, Inaugura.Measurement.Unit.SquareMeter, Inaugura.Measurement.Unit.Acre });

		#region Variables
		private string mDescription;
        private Inaugura.Measurement.Measurement mSize;
		private StringCollection mFeatures;
		#endregion

		#region Properties
		/// <summary>
		/// The description of the lot
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
		/// The size of the lot
		/// </summary>
        public Inaugura.Measurement.Measurement Size
		{
			get
			{
				return this.mSize;
			}
			set
			{
                if (value.Unit.Type != Inaugura.Measurement.Unit.UnitType.Area)
					throw new Exception("The size measurement must be in two-dimensional units");

				this.mSize = value;
			}
		}

		/// <summary>
		/// The features of the lot
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
		#endregion

		#region IXmlable Members

		/// <summary>
		/// The xml which represents the instance
		/// </summary>
		public XmlNode Xml
		{
			get 
			{
				XmlNode lotNode = Inaugura.Xml.Helper.NewNodeDocument("Lot");
				this.PopulateNode(lotNode);
				return lotNode;
			}
		}

		/// <summary>
		/// Populates a xml node with the objects data
		/// </summary>
		/// <param name="node">The  node to populate</param>
		public virtual void PopulateNode(XmlNode node)
		{
            if (!string.IsNullOrEmpty(this.mDescription))
            {
                XmlNode descriptionNode = node.OwnerDocument.CreateElement("Description");
                descriptionNode.InnerText = this.Description;
                node.AppendChild(descriptionNode);
            }

            if (this.Size.Value != 0)
            {
                XmlNode sizeNode = node.OwnerDocument.CreateElement("Size");
                Inaugura.Xml.Helper.SetAttribute(sizeNode, "valueStdUnit", this.Size.ToStandardUnit().Value.ToString());
                this.Size.PopulateNode(sizeNode);
                node.AppendChild(sizeNode);
            }

            if (this.Features.Count > 0)
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
			if (node["Description"] == null)
				throw new ArgumentException("The xml does not contain the Description node");


			if (node["Size"] == null)
				throw new ArgumentException("The xml does not contain the Size node");

                        
			if (node["Features"] != null)
                this.Features = new StringCollection(node["Features"]);

            this.Description = node["Description"].InnerText;			
            this.Size = new Inaugura.Measurement.Measurement(node["Size"]);
		}
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="node">The xml node which defines the lot</param>
		public Lot(XmlNode node) : this()
		{
			this.PopulateInstance(node);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="size">The size of the lot</param>
		/// <param name="features">The features of the lot</param>
		/// <param name="description">The description of the lot</param>
        public Lot(Inaugura.Measurement.Measurement size, StringCollection features, string description)
		{
			this.Description = description;
			this.Features = features;
			this.Size = size;
		}

		/// <summary>
		/// Constructor
		/// </summary>
        public Lot()
            : this(new Inaugura.Measurement.Measurement(0, Inaugura.Measurement.Unit.SquareMeter), new StringCollection(), String.Empty)
		{
		}
	}
}
