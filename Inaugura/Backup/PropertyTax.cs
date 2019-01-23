using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	public class PropertyTax : Inaugura.Xml.IXmlable
	{
		#region Variables
		private float mValue;
		private bool mEstimated;
		private int mYear;
		#endregion

		#region Properties
		/// <summary>
		/// The tax value
		/// </summary>
		public float Value
		{
			get
			{
				return this.mValue;
			}
			set
			{
				this.mValue = value;
			}
		}

		/// <summary>
		/// True if the ammount is estimated, false otherwise
		/// </summary>
		public bool Estimated
		{
			get
			{
				return this.mEstimated;
			}
			set
			{
				this.mEstimated = value;
			}
		}

		/// <summary>
		/// The year which the tax ammount applies
		/// </summary>
		public int Year
		{
			get
			{
				return this.mYear;
			}
			set
			{
				this.mYear = value;
			}
		}

		#endregion

		#region IXmlable Members
		public XmlNode Xml
		{
			get 
			{ 				
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("PropertyTax");
				this.PopulateNode(node);
				return node;
			}
		}

		/// <summary>
		/// Populates a xml node with the objects data
		/// </summary>
		/// <param name="node">The  node to populate</param>
		public void PopulateNode(XmlNode node)
		{
			Inaugura.Xml.Helper.SetAttribute(node, "value", this.Value.ToString());
			Inaugura.Xml.Helper.SetAttribute(node, "year", this.Year.ToString());
			Inaugura.Xml.Helper.SetAttribute(node,"estimated",this.Estimated.ToString());
		}

		/// <summary>
		/// Populates the instance with the specifed xml data
		/// </summary>
		/// <param name="node"></param>
		public void PopulateInstance(XmlNode node)
		{
			if(node.Attributes["value"] == null)
				throw new Exception("The xml does not contain a value attribute");

			if(node.Attributes["year"] == null)
				throw new Exception("The xml does not contain a year attribute");

			if(node.Attributes["estimated"] == null)
				throw new Exception("The xml does not contain a estimated attribute");

			this.mValue = float.Parse(Inaugura.Xml.Helper.GetAttribute(node, "value"));
			this.mYear = int.Parse(Inaugura.Xml.Helper.GetAttribute(node,"year"));
			this.mEstimated = bool.Parse(Inaugura.Xml.Helper.GetAttribute(node,"estimated"));			
		}
		#endregion

		#region Methods

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="node">The xml node which defines the property tax</param>
		public PropertyTax(XmlNode node)			
		{
			this.PopulateInstance(node);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="value">The property tax value</param>
		/// <param name="year">The property tax year</param>
		/// <param name="estimated">Whether or not the value is estimated</param>
		public PropertyTax(float value, int year, bool estimated)
		{
			this.Value = value;
			this.Year = year;
			this.Estimated = estimated;
		}

        public override int GetHashCode()
        {
            int hashCode = 0;

            hashCode ^= this.mEstimated.GetHashCode();
            hashCode ^= this.mValue.GetHashCode();
            hashCode ^= this.mValue.GetHashCode();
            
            return hashCode;
        }
		#endregion		
	}
}
