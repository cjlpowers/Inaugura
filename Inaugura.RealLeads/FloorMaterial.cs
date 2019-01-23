using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{

	public class FloorMaterial : Types, Inaugura.Xml.IXmlable
	{
		/// <summary>
		/// Other
		/// </summary>
		public static FloorMaterial Other = new FloorMaterial(-1, "Other");
		/// <summary>
		/// Not Specified
		/// </summary>
		public static FloorMaterial NotSpecified = new FloorMaterial(0, "Not Specified");
		/// <summary>
		/// Carpet
		/// </summary>
		public static FloorMaterial Carpet = new FloorMaterial(1, "Carpet");
		/// <summary>
		/// Hard wood
		/// </summary>
		public static FloorMaterial Hardwood = new FloorMaterial(2, "Hardwood");
		/// <summary>
		/// Ceramic
		/// </summary>
		public static FloorMaterial Ceramic = new FloorMaterial(3, "Ceramic");
		/// <summary>
		/// Porcelan
		/// </summary>
		public static FloorMaterial Porcelan = new FloorMaterial(4, "Porcelan");
		/// <summary>
		/// Vinyl
		/// </summary>
		public static FloorMaterial Vinyl = new FloorMaterial(5, "Vinyl ");        
		/// <summary>
		/// Cork
		/// </summary>
		public static FloorMaterial Cork = new FloorMaterial(6, "Cork");
        /// <summary>
        /// Laminate
        /// </summary>
        public static FloorMaterial Laminate = new FloorMaterial(7, "Laminate");

        public static FloorMaterial[] All = new FloorMaterial[] { NotSpecified, Carpet, Hardwood, Ceramic, Porcelan, Vinyl, Laminate, Cork, Other };


		#region Properties
		#region IXmlable Members
		/// <summary>
		/// Gets the xml representation of the floor material
		/// </summary>
		/// <value></value>
		public XmlNode Xml
		{
			get
			{
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("FloorMaterial");
				this.PopulateNode(node);
				return node;
			}
		}
		#endregion
		#endregion

		private FloorMaterial(int value, string name)
			: base(value, name)
		{
		}

		/// <summary>
		/// Creates a heating type instance from an xml node representation
		/// </summary>
		/// <param name="xml">The xml which represents the item</param>
		/// <returns>A floor material instance</returns>
		public static FloorMaterial FromXml(XmlNode xml)
		{
			return FromValue(Types.GetTypeValue(xml));
		}


		/// <summary>
		/// Gets the floor material associated with a specific value
		/// </summary>
		/// <param name="value">The interger value of the floor material</param>
		/// <returns>The floor material matching the integer value</returns>
		public static FloorMaterial FromValue(int value)
		{
			foreach (FloorMaterial type in All)
			{
				if (type.Value == value)
					return type;
			}
			throw new NotSupportedException(string.Format("The type value '{0}' was not supported", value));
		}		
	}
}
