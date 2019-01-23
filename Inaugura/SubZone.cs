#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#endregion

namespace Inaugura
{
    /// <summary>
    /// A class wich represents the 
    /// </summary>
	public class SubZone : Inaugura.Xml.IXmlable
	{
		#region Variables
        private string mID;
        private string mZoneID;
        private string mName;
		private Details mDetails;
		#endregion

		#region IXmlable Members
		/// <summary>
		/// Gets the xml representation of the SubZone
		/// </summary>
		/// <value></value>
		public XmlNode Xml
		{
			get
			{
                XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("SubZone");
                this.PopulateNode(node);
                return node;
    		}
		}
		#endregion

		#region Properties
		/// <summary>
		/// The ID of the Sub Zone
		/// </summary>
		/// <value></value>		
		public string ID
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
        /// The ID of the zone
        /// </summary>
        public string ZoneID
        {
            get
            {
                return this.mZoneID;
            }
            set
            {
                this.mZoneID = value;
            }
        }


		/// <summary>
		/// The name of the Zone
		/// </summary>
		/// <value></value>
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
		/// Additional details specific to this Zone
		/// </summary>
		/// <value></value>
		public Details Details
		{
			get
			{
				return this.mDetails;
			}
			private set
			{
				this.mDetails = value;
			}
		}		
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="subZoneNode">The xml representation of the SubZone</param>
		public SubZone(XmlNode subZoneNode) : this(string.Empty, string.Empty)
		{
			if (subZoneNode == null)
				throw new ArgumentNullException("subZoneNode","The Xml definition may not be null");

            this.PopulateInstance(subZoneNode);
		}

		/// <summary>
		/// Constructor
		/// </summary>		
		public SubZone(string zoneID, string name)
		{                 
            this.ID = Guid.NewGuid().ToString();
            this.ZoneID = zoneID;
            this.Name = name;
            this.Details = new Details();
		}

		/// <summary>
		/// Creates a Zone instance from an xml node representation
		/// </summary>
		/// <param name="xml">The xml which represents the item</param>
		/// <returns>A Zone instance</returns>
		public static Zone FromXml(XmlNode xml)
		{
			return Inaugura.Xml.Helper.GetIXmlableFromXml(xml, System.Reflection.Assembly.GetExecutingAssembly(), typeof(Zone)) as Zone;
		}

        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public void PopulateNode(XmlNode node)
        {            
            Inaugura.Xml.Helper.SetAttribute(node, "id", this.ID);
            Inaugura.Xml.Helper.SetAttribute(node, "zoneId", this.ZoneID);
            Inaugura.Xml.Helper.SetAttribute(node, "name", this.Name);

            if (this.Details.Count > 0)
            {
                XmlNode detailsNode = node.OwnerDocument.CreateElement("Details");
                this.Details.PopulateNode(detailsNode);
                node.AppendChild(detailsNode);
            }
        }

        /// <summary>
        /// Populates the instance with the specifed xml data
        /// </summary>
        /// <param name="node"></param>
        public void PopulateInstance(XmlNode node)
        {
            if (node.Attributes["id"] == null)
                throw new ArgumentException("The xml does not contain a id attribute");

            if (node.Attributes["name"] == null)
                throw new ArgumentException("The xml does not contain a name attribute");

			this.ID = Inaugura.Xml.Helper.GetAttribute(node, "id");
			this.ZoneID = Inaugura.Xml.Helper.GetAttribute(node, "zoneId");
			this.Name = Inaugura.Xml.Helper.GetAttribute(node, "name");

            if (node["Details"] != null)
                this.Details = new Details(node["Details"]);
        }
	}
}
