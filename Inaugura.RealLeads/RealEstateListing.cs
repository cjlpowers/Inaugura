#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#endregion

namespace Inaugura.RealLeads
{
    /// <summary>
    /// A class representing a Real Estate Listing
    /// </summary>
	public class RealEstateListing : ResidentialPropertyListing
	{
		#region Variables
		private float mPrice;
		private float mAppraisal;
		private PropertyTax mPropertyTax;
		private WaterType mWaterType;
		private PropertyType mPropertyType;
		private RoofType mRoofType;
		private FoundationType mFoundationType;
		private HeatingType mHeatingPrimary;
		private HeatingType mHeatingSecondary;
		private ElectricalServiceType mElectricalServiceType;
		private BasementType mBasementType;
		private DrivewayType mDrivewayType;
		private ExteriorMaterial mExteriorPrimary;
		private ExteriorMaterial mExteriorSecondary;
		private StringCollection mRentalEquipment;
		#endregion

		#region Properties
		/// <summary>
		/// The price of the property
		/// </summary>
		public float Price
		{
			get
			{
				return this.mPrice;
			}
			set
			{
				this.mPrice = value;
			}
		}

		/// <summary>
		/// The appraisal value of the property
		/// </summary>
		public float Appraisal
		{
			get
			{
				return this.mAppraisal;
			}
			set
			{
				this.mAppraisal = value;
			}
		}

		/// <summary>
		/// The Open House Prompt
		/// </summary>
		/// <value></value>
		public string OpenHousePrompt
		{
			get
			{
				if (this.Details["OpenHousePrompt"] != null)
					return this.Details["OpenHousePrompt"];
				else
					return string.Empty;
			}
			set
			{
				this.Details["OpenHousePrompt"] = value;
			}
		}

		/// <summary>
		/// The property type
		/// </summary>
		public PropertyType PropertyType
		{
			get
			{
				return this.mPropertyType;
			}
			set
			{
				this.mPropertyType = value;
			}
		}

		/// <summary>
		/// The property tax
		/// </summary>
		public PropertyTax PropertyTax
		{
			get
			{
				return this.mPropertyTax;
			}
			set
			{
				this.mPropertyTax = value;
			}
		}

		/// <summary>
		/// Water information
		/// </summary>
		public WaterType WaterType
		{
			get
			{
				return this.mWaterType;
			}
			set
			{
				this.mWaterType = value;
			}
		}
			
		/// <summary>
		/// The roofing material used on the exterior of the building
		/// </summary>
		public RoofType RoofType
		{
			get
			{
				return this.mRoofType;
			}
			set
			{
				this.mRoofType = value;
			}
		}

		/// <summary>
		/// The foundation type
		/// </summary>
		public FoundationType FoundationType
		{
			get
			{
				return this.mFoundationType;
			}
			set
			{
				this.mFoundationType = value;
			}
		}

		/// <summary>
		/// The primary type of heating
		/// </summary>
		public HeatingType HeatingPrimary
		{
			get
			{
				return this.mHeatingPrimary;
			}
			set
			{
				this.mHeatingPrimary = value;
			}
		}

		/// <summary>
		/// The secondary type of heating
		/// </summary>
		public HeatingType HeatingSecondary
		{
			get
			{
				return this.mHeatingSecondary;
			}
			set
			{
				this.mHeatingSecondary = value;
			}
		}
		
		/// <summary>
		/// The primary exterior material
		/// </summary>
		public ExteriorMaterial ExteriorPrimary
		{
			get
			{
				return this.mExteriorPrimary;
			}
			set
			{
				this.mExteriorPrimary = value;
			}
		}

		/// <summary>
		/// The secondary exterior material
		/// </summary>
		public ExteriorMaterial ExteriorSecondary
		{
			get
			{
				return this.mExteriorSecondary;
			}
			set
			{
				this.mExteriorSecondary = value;
			}
		}

		/// <summary>
		/// The electrical service
		/// </summary>
		public ElectricalServiceType ElectricalService
		{
			get
			{
				return this.mElectricalServiceType;
			}
			set
			{
				this.mElectricalServiceType = value;
			}
		}

		/// <summary>
		/// The basement type
		/// </summary>
		public BasementType BasementType
		{
			get
			{
				return this.mBasementType;
			}
			set
			{
				this.mBasementType = value;
			}
		}

		/// <summary>
		/// The driveway type
		/// </summary>
		public DrivewayType DrivewayType
		{
			get
			{
				return this.mDrivewayType;
			}
			set
			{
				this.mDrivewayType = value;
			}
		}
	
		/// <summary>
		/// Rental Equipment
		/// </summary>
		public StringCollection RentalEquipment
		{
			get
			{
				return this.mRentalEquipment;
			}
			private set
			{
				this.mRentalEquipment = value;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Constructor
		/// </summary>
		public RealEstateListing() : this(PropertyType.NotSpecified)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public RealEstateListing(PropertyType type)
		{
			this.PropertyTax = new PropertyTax(0, DateTime.Now.Year, false);
			this.PropertyType = type;
			this.WaterType = WaterType.NotSpecified;
			this.RoofType = RoofType.NotSpecified;
			this.FoundationType = FoundationType.NotSpecified;
			this.HeatingPrimary = HeatingType.NotSpecified;
			this.HeatingSecondary = HeatingType.NotSpecified;
			this.ElectricalService = ElectricalServiceType.NotSpecified;
			this.BasementType = BasementType.NotSpecified;
			this.DrivewayType = DrivewayType.NotSpecified;
			this.ExteriorPrimary = ExteriorMaterial.NotSpecified;
			this.ExteriorSecondary = ExteriorMaterial.NotSpecified;
			this.RentalEquipment = new StringCollection();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="xml">The xml representation of the listing</param>
		public RealEstateListing(XmlNode listingNode) : base(listingNode)
		{			
		}

		/// <summary>
		/// Populates the instance with the specifed xml data
		/// </summary>
		/// <param name="node">The xml node</param>
		public override void PopulateInstance(XmlNode node)
		{
			base.PopulateInstance(node);
		
			if (node.Attributes["price"] != null)
				this.Price = float.Parse(Inaugura.Xml.Helper.GetAttribute(node, "price"));

			if (node.Attributes["appraisal"] != null)
				this.Appraisal = float.Parse(Inaugura.Xml.Helper.GetAttribute(node, "appraisal"));					                
					
			if (node["PropertyTax"] == null)
				throw new ArgumentException("The xml does not contain the PropertyTax node");			
			
			this.PropertyTax = new PropertyTax(node["PropertyTax"]);

			if (node["PropertyType"] != null)
				this.PropertyType = PropertyType.FromXml(node["PropertyType"]);
			else
				this.PropertyType = PropertyType.NotSpecified;

			if (node["FoundationType"] != null)
				this.FoundationType = FoundationType.FromXml(node["FoundationType"]);
			else
				this.FoundationType = FoundationType.NotSpecified;

			if (node["RoofType"] != null)
				this.RoofType = RoofType.FromXml(node["RoofType"]);
			else
				this.RoofType = RoofType.NotSpecified;

			if (node["DrivewayType"] != null)
				this.DrivewayType = DrivewayType.FromXml(node["DrivewayType"]);
			else
				this.DrivewayType = DrivewayType.NotSpecified;

			if (node["HeatingPrimary"] != null)
				this.HeatingPrimary = HeatingType.FromXml(node["HeatingPrimary"]);
			else
				this.HeatingPrimary = HeatingType.NotSpecified;

			if (node["HeatingSecondary"] != null)
				this.HeatingSecondary = HeatingType.FromXml(node["HeatingSecondary"]);
			else
				this.HeatingSecondary = HeatingType.NotSpecified;

			if (node["ExteriorPrimary"] != null)
				this.ExteriorPrimary = ExteriorMaterial.FromXml(node["ExteriorPrimary"]);
			else
				this.ExteriorPrimary = ExteriorMaterial.NotSpecified;

			if (node["ExteriorSecondary"] != null)
				this.ExteriorSecondary = ExteriorMaterial.FromXml(node["ExteriorSecondary"]);
			else
				this.ExteriorSecondary = ExteriorMaterial.NotSpecified;
			
			if (node["RentalEquipment"] != null)
				this.RentalEquipment = new StringCollection(node["RentalEquipment"]);
			else
				this.RentalEquipment = new StringCollection();

			if (node["ElectricalService"] != null)
				this.ElectricalService = ElectricalServiceType.FromXml(node["ElectricalService"]);
			else
				this.ElectricalService = ElectricalServiceType.NotSpecified;

			if (node["WaterType"] != null)
				this.WaterType = WaterType.FromXml(node["WaterType"]);
			else
				this.WaterType = WaterType.NotSpecified;

			if (node["BasementType"] != null)
				this.BasementType = BasementType.FromXml(node["BasementType"]);
			else
				this.BasementType = BasementType.NotSpecified;


		}

		/// <summary>
		/// Populates a xml node with the objects data
		/// </summary>
		/// <param name="node">The  node to populate</param>
		public override void PopulateNode(XmlNode node)
		{
			base.PopulateNode(node);
			if (this.Price != 0)
				Inaugura.Xml.Helper.SetAttribute(node, "price", this.Price.ToString());

			if (this.Appraisal != 0)
				Inaugura.Xml.Helper.SetAttribute(node, "appraisal", this.Appraisal.ToString());            
									
			XmlNode propertyTaxNode = node.OwnerDocument.CreateElement("PropertyTax");
			this.PropertyTax.PopulateNode(propertyTaxNode);
			node.AppendChild(propertyTaxNode);

			if (this.PropertyType != PropertyType.NotSpecified)
			{
				XmlNode propertyTypeNode = node.OwnerDocument.CreateElement("PropertyType");
				this.PropertyType.PopulateNode(propertyTypeNode);
				node.AppendChild(propertyTypeNode);
			}

			if (this.RoofType != RoofType.NotSpecified)
			{
				XmlNode roofTypeNode = node.OwnerDocument.CreateElement("RoofType");
				this.RoofType.PopulateNode(roofTypeNode);
				node.AppendChild(roofTypeNode);
			}

			if (this.FoundationType != FoundationType.NotSpecified)
			{
				XmlNode foundationTypeNode = node.OwnerDocument.CreateElement("FoundationType");
				this.FoundationType.PopulateNode(foundationTypeNode);
				node.AppendChild(foundationTypeNode);
			}

			if (this.HeatingPrimary != HeatingType.NotSpecified)
			{
				XmlNode primaryHeatingNode = node.OwnerDocument.CreateElement("HeatingPrimary");
				this.HeatingPrimary.PopulateNode(primaryHeatingNode);
				node.AppendChild(primaryHeatingNode);
			}

			if (this.HeatingSecondary != HeatingType.NotSpecified)
			{
				XmlNode secondaryHeatingNode = node.OwnerDocument.CreateElement("HeatingSecondary");
				this.HeatingSecondary.PopulateNode(secondaryHeatingNode);
				node.AppendChild(secondaryHeatingNode);
			}

			if (this.ExteriorPrimary != ExteriorMaterial.NotSpecified)
			{
				XmlNode exteriorPrimaryNode = node.OwnerDocument.CreateElement("ExteriorPrimary");
				this.ExteriorPrimary.PopulateNode(exteriorPrimaryNode);
				node.AppendChild(exteriorPrimaryNode);
			}

			if (this.ExteriorSecondary != ExteriorMaterial.NotSpecified)
			{
				XmlNode exteriorSecondaryNode = node.OwnerDocument.CreateElement("ExteriorSecondary");
                this.ExteriorSecondary.PopulateNode(exteriorSecondaryNode);
				node.AppendChild(exteriorSecondaryNode);
			}

			if (this.ElectricalService != ElectricalServiceType.NotSpecified)
			{
				XmlNode electricalServiceNode = node.OwnerDocument.CreateElement("ElectricalService");
				this.ElectricalService.PopulateNode(electricalServiceNode);
				node.AppendChild(electricalServiceNode);
			}

			if (this.WaterType != WaterType.NotSpecified)
			{
				XmlNode waterTypeNode = node.OwnerDocument.CreateElement("WaterType");
				this.WaterType.PopulateNode(waterTypeNode);
				node.AppendChild(waterTypeNode);
			}

			if (this.BasementType != BasementType.NotSpecified)
			{
				XmlNode basementTypeNode = node.OwnerDocument.CreateElement("BasementType");
				this.BasementType.PopulateNode(basementTypeNode);
				node.AppendChild(basementTypeNode);
			}

			if (this.DrivewayType != DrivewayType.NotSpecified)
			{
				XmlNode drivewayTypeNode = node.OwnerDocument.CreateElement("DrivewayType");
				this.DrivewayType.PopulateNode(drivewayTypeNode);
				node.AppendChild(drivewayTypeNode);
			}

			if (this.RentalEquipment.Count > 0)
			{
				XmlNode rentalEquipmentNode = node.OwnerDocument.CreateElement("RentalEquipment");
				this.RentalEquipment.PopulateNode(rentalEquipmentNode);
				node.AppendChild(rentalEquipmentNode);
			}			
		}
        		

        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();
       
            hashCode ^= this.mAppraisal.GetHashCode();
            if (this.mBasementType != null)
                hashCode ^= this.mBasementType.GetHashCode();
            if (this.mDrivewayType != null)
                hashCode ^= this.mDrivewayType.GetHashCode();
            if (this.mElectricalServiceType != null)
                hashCode ^= this.mElectricalServiceType.GetHashCode();
            if (this.mExteriorPrimary != null)
                hashCode ^= this.mExteriorPrimary.GetHashCode();
            if (this.mExteriorSecondary != null)
                hashCode ^= this.mExteriorSecondary.GetHashCode();
            if (this.mFoundationType != null)
                hashCode ^= this.mFoundationType.GetHashCode();
            if (this.mHeatingPrimary != null)
                hashCode ^= this.mHeatingPrimary.GetHashCode();
            if (this.mHeatingSecondary != null)
                hashCode ^= this.mHeatingSecondary.GetHashCode();
            hashCode ^= this.mPrice.GetHashCode();
            if (this.mPropertyTax != null)
                hashCode ^= this.mPropertyTax.GetHashCode();
            if (this.mPropertyType != null)
                hashCode ^= this.mPropertyType.GetHashCode();
            if (this.mRentalEquipment != null)
                hashCode ^= this.mRentalEquipment.GetHashCode();
            if (this.mRoofType != null)
                hashCode ^= this.mRoofType.GetHashCode();
            if (this.mWaterType != null)
                hashCode ^= this.mWaterType.GetHashCode();

            return hashCode;
        }
		#endregion
	}
}
