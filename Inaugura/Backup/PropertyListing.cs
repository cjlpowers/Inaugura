using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Inaugura.Maps;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// An abstract class representing a property listing
	/// </summary>
	public abstract class PropertyListing : Listing
	{
		#region Variables
        private int mYearBuilt;
        private int mAge;
		private Inaugura.Measurement.Measurement mSize;
		private string mDirections;
		private Address mAddress;
		private Lot mLot;
		private CostCollection mCosts;        
		#endregion

		#region Properties
		/// <summary>
		/// The size of the property
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
					throw new Exception("The size measurement must be a unit of area");

				this.mSize = value;
			}
		}

		/// <summary>
		/// The directions to the property
		/// </summary>
		public string Directions
		{
			get
			{
				return this.mDirections;
			}
			set
			{
				this.mDirections = value;
			}
		}

		/// <summary>
		/// The address of the property
		/// </summary>
		public Address Address
		{
			get
			{
				return this.mAddress;
			}
			set
			{
				this.mAddress = value;
			}
		}

		/// <summary>
		/// The lot on which the property resides
		/// </summary>
		public Lot Lot
		{
			get
			{
				return this.mLot;
			}
			set
			{
				this.mLot = value;
			}
		}

		/// <summary>
		/// The costs associated with the listing
		/// </summary>
		public CostCollection Costs
		{
			get
			{
				return this.mCosts;
			}
			private set
			{
				this.mCosts = value;
			}
		}

        /// <summary>
        /// The year the property was built
        /// </summary>
        /// <remarks>0 if not specified</remarks>
        public int YearBuilt
        {
            get
            {
                if (this.mYearBuilt != 0)
                    return this.mYearBuilt;
                else
                    return 0;
            }
            set
            {
                this.mYearBuilt = value;
            }
        }

        /// <summary>
        /// The age of the property in years
        /// </summary>
        /// <remarks>0 if not specified</remarks>
        public int Age
        {
            get
            {
                if (this.mAge != 0)
                    return this.mAge;
                else if (this.mYearBuilt != 0)
                    return DateTime.Now.Year - this.mYearBuilt;
                else
                    return 0;
            }
            set
            {
                this.mAge = value;
            }
        }

        /// <summary>
        /// The distance to the property (as determined by users search)
        /// </summary>
        public double Distance
        {
            get
            {
                if (this.Objects.ContainsKey("_distance"))
                    return (double)this.Objects["_distance"];
                else
                    return 0;
            }
            set
            {
                this.Objects["_distance"] = value;
            }
        }
		#endregion

		#region Methods
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="node">The node which defines the property listing</param>
		public PropertyListing(XmlNode node) :this()
		{
			this.PopulateInstance(node);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public PropertyListing() : base()
		{
			this.Size = new Inaugura.Measurement.Measurement(0, Inaugura.Measurement.Unit.SquareFeet);
			this.Address = new Address();
			this.Lot = new Lot();
			this.Costs = new CostCollection();
		}
		
		/// <summary>
		/// Populates the instance with data from an xml node
		/// </summary>
		/// <param name="node">The xml node containing the data</param>
		public override void PopulateInstance(System.Xml.XmlNode node)
		{
			base.PopulateInstance(node);

            if (node.Attributes["yearBuilt"] != null)
                this.YearBuilt = int.Parse(node.Attributes["yearBuilt"].Value);

            if (node.Attributes["age"] != null)
                this.Age = int.Parse(node.Attributes["age"].Value);

			if (node["Directions"] == null)
				throw new ArgumentException("The xml does not contain the directions node");
			

			//if(node["Size"] == null)
				//throw new ArgumentException("The xml does not contain the size node");
						
			if (node["Address"] == null)
				throw new ArgumentException("The xml does not contain the Address node");

            //if (node["Lot"] == null)
            //    throw new ArgumentException("The xml does not contain the Lot node");

			if (node["Costs"] != null)
			{
				this.Costs = new CostCollection(node["Costs"]);
			}

			this.Directions = node["Directions"].InnerText;

            if(node["Size"] != null)
                this.Size = new Inaugura.Measurement.Measurement(node["Size"]);

            if (node["InteriorFeatures"] != null)
                this.Features = StringCollection.Merge(this.Features, new StringCollection(node["InteriorFeatures"]));

			if (node["ExteriorFeatures"] != null)
                this.Features = StringCollection.Merge(this.Features, new StringCollection(node["ExteriorFeatures"]));
				
			if (node["LocationFeatures"] != null)
                this.Features = StringCollection.Merge(this.Features, new StringCollection(node["LocationFeatures"]));

			this.Address = new Address(node["Address"]);
            if (node["Lot"] != null)
			    this.Lot = new Lot(node["Lot"]);
		}

		/// <summary>
		/// Populates a xml node with instance data
		/// </summary>
		/// <param name="node">The xml node to populate</param>
		public override void PopulateNode(System.Xml.XmlNode node)
		{
			base.PopulateNode(node);

			XmlNode directionsNode = node.OwnerDocument.CreateElement("Directions");
			directionsNode.InnerText = this.Directions;
			node.AppendChild(directionsNode);

            if (this.mYearBuilt != 0)
                Inaugura.Xml.Helper.SetAttribute(node, "yearBuilt", this.mYearBuilt.ToString());

            if (this.mAge != 0)
                Inaugura.Xml.Helper.SetAttribute(node, "age", this.mAge.ToString());

            if (this.Size.Value != 0)
            {
                XmlNode sizeNode = node.OwnerDocument.CreateElement("Size");
                Inaugura.Xml.Helper.SetAttribute(sizeNode, "valueStdUnit", this.Size.ToStandardUnit().Value.ToString());
                this.Size.PopulateNode(sizeNode);
                node.AppendChild(sizeNode);
            }

			XmlNode addressNode = node.OwnerDocument.CreateElement("Address");
			this.Address.PopulateNode(addressNode);
			node.AppendChild(addressNode);

            if (this.mLot.Features.Count > 0 || this.mLot.Size.Value != 0 || !string.IsNullOrEmpty(this.mLot.Description))
            {
                XmlNode lotNode = node.OwnerDocument.CreateElement("Lot");
                this.Lot.PopulateNode(lotNode);
                node.AppendChild(lotNode);
            }

			if (this.Costs.Count > 0)
			{
				XmlNode costsNode = node.OwnerDocument.CreateElement("Costs");
				this.Costs.PopulateNode(costsNode);
				node.AppendChild(costsNode);
			}
		}
		#endregion
	}



}
