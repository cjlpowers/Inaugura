using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{

	/// <summary>
	/// Exterior Materials
	/// </summary>
	public class ExteriorMaterial : Types, Inaugura.Xml.IXmlable
	{
		public static ExteriorMaterial Other = new ExteriorMaterial(-1, "Other");
		public static ExteriorMaterial NotSpecified = new ExteriorMaterial(0, "Not Specified");
		public static ExteriorMaterial Brick = new ExteriorMaterial(1, "Brick");
		public static ExteriorMaterial VinylSiding = new ExteriorMaterial(2, "Vinyl Siding");
		public static ExteriorMaterial CedarShakes = new ExteriorMaterial(3, "Cedar Shakes");

		public static ExteriorMaterial[] All = new ExteriorMaterial[] { NotSpecified, Brick, VinylSiding, CedarShakes, Other };

	
		#region Properties
		#region IXmlable Members
		/// <summary>
		/// Gets the xml representation of the exterior material
		/// </summary>
		/// <value></value>
		public XmlNode Xml
		{
			get
			{
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("ExteriorMaterial");
				this.PopulateNode(node);
				return node;
			}
		}
		#endregion
		#endregion

		private ExteriorMaterial(int value, string name)
			: base(value, name)
		{
		}

		/// <summary>
		/// Creates a heating type instance from an xml node representation
		/// </summary>
		/// <param name="xml">The xml which represents the item</param>
		/// <returns>A external material instance</returns>
		public static ExteriorMaterial FromXml(XmlNode xml)
		{
			return FromValue(Types.GetTypeValue(xml));
		}


		/// <summary>
		/// Gets the external material associated with a specific value
		/// </summary>
		/// <param name="value">The interger value of the external material</param>
		/// <returns>The external material matching the integer value</returns>
		public static ExteriorMaterial FromValue(int value)
		{
			foreach (ExteriorMaterial type in All)
			{
				if (type.Value == value)
					return type;
			}
			throw new NotSupportedException(string.Format("The type value '{0}' was not supported", value));
		}		
	}
}
