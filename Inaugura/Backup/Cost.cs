using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// A class which represents a cost
	/// </summary>
	public class Cost
	{
		#region Varialbes
		private string mName;
		private string mDescription;
		private float mValue;
		#endregion

		#region Properties
		/// <summary>
		/// The cost item name
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
		/// The cost item description
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
		/// The cost item value
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

		#region IXmlable Members
		/// <summary>
		/// The xml representation of this instance
		/// </summary>
		public XmlNode Xml
		{
			get 
			{
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Cost");
				this.PopulateNode(node);
				return node;
			}
		}
		#endregion

		#endregion

		#region Methods
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">The name of the cost Item</param>
		/// <param name="description">The description of the cost item</param>
		/// <param name="value">The value of the cost item</param>
		public Cost(string name, string description, float value)
		{
			this.Name = name;
			this.Description = description;
			this.Value = value;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="node">The xml node which defines this instance</param>
		public Cost(XmlNode node) : this(string.Empty,string.Empty,0)
		{
			this.PopulateInstance(node);
		}

		/// <summary>
		/// Populates a xml node with the objects data
		/// </summary>
		/// <param name="node">The  node to populate</param>
		public virtual void PopulateNode(XmlNode node)
		{
			Inaugura.Xml.Helper.SetAttribute(node, "name", this.Name);
			Inaugura.Xml.Helper.SetAttribute(node, "description", this.Description);
			Inaugura.Xml.Helper.SetAttribute(node, "value", this.Value.ToString());
		}

		/// <summary>
		/// Populates the instance with the specifed xml data
		/// </summary>
		/// <param name="node">The xml node</param>
		public virtual void PopulateInstance(XmlNode node)
		{
			if (node.Attributes["name"] == null)
				throw new ArgumentException("The xml does not contain a name attribute");

			if (node.Attributes["description"] == null)
				throw new ArgumentException("The xml does not contain a description attribute");

			if (node.Attributes["value"] == null)
				throw new ArgumentException("The xml does not contain a value attribute");
					
			this.Name = Inaugura.Xml.Helper.GetAttribute(node, "name");
			this.Description = Inaugura.Xml.Helper.GetAttribute(node, "description");
			this.Value = float.Parse(Inaugura.Xml.Helper.GetAttribute(node, "value"));		
		}
		#endregion
	}
}
