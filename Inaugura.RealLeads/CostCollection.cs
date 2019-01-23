using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	public class CostCollection : System.Collections.CollectionBase, Inaugura.Xml.IXmlable
	{
		#region Variables
		#endregion

		#region Accessors
		/// <summary>
		/// Returns the cost at the specified zero based index
		/// </summary>
		/// <param name="index">The zero based index into the collection</param>
		/// <returns>The cost at the specified index</returns>
		public Cost this[int index]
		{
			get
			{
				return this.List[index] as Cost;
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// The sum of all costs in the collection
		/// </summary>
		public float TotalCost
		{
			get
			{
				float totalCost = 0;
				foreach (Cost cost in this)
				{
					totalCost += cost.Value;
				}
				return totalCost;
			}
		}
		#region IXmlable Members
		/// <summary>
		/// The xml representation of the the instance
		/// </summary>
		public XmlNode Xml
		{
			get
			{
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Costs");
				this.PopulateNode(node);
				return node;
			}
		}
		#endregion
		#endregion

		#region Method
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="node">The xml node which represents the instance</param>
		public CostCollection(XmlNode node)
		{
			this.PopulateInstance(node);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public CostCollection()
		{
		}

		/// <summary>
		/// Adds a cost to the collection
		/// </summary>
		/// <param name="level">The cost to add</param>
		public void Add(Cost cost)
		{
			this.List.Add(cost);
		}

		/// <summary>
		/// Removes a cost from the collection
		/// </summary>
		/// <param name="level">The cost to remove</param>
		public void Remove(Cost cost)
		{
			this.List.Remove(cost);
		}

		/// <summary>
		/// Populates a xml node with the objects data
		/// </summary>
		/// <param name="node">The  node to populate</param>
		public virtual void PopulateNode(XmlNode node)
		{
			foreach (Cost item in this)
			{
				XmlNode itemNode = node.OwnerDocument.CreateElement("Cost");
				item.PopulateNode(itemNode);
				node.AppendChild(itemNode);
			}
		}

		/// <summary>
		/// Populates the instance with the specifed xml data
		/// </summary>
		/// <param name="node">The xml node</param>
		public virtual void PopulateInstance(XmlNode node)
		{
			XmlNodeList nodes = node.SelectNodes("Cost");
			foreach (XmlNode itemNode in nodes)
			{
				Cost cost = new Cost(itemNode);
				this.Add(cost);
			}			
		}
		#endregion			
	}
}
