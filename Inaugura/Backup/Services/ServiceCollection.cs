using System;
using System.Collections;
using System.Xml;

using System.Drawing.Design;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace Inaugura.Telephony.Services
{
	/// <summary>
	/// Collection of IServices
	/// </summary>	
	[Editor(typeof(ServiceCollectionEditor),typeof(UITypeEditor))]
	public class ServiceCollection : CollectionBase, Inaugura.Xml.IXmlable
	{
		#region Accessors
		/// <summary>
		/// Accessor
		/// </summary>
		/// <value></value>
		public Service this [int index]
		{
			get
			{
				return (Service)this.List[index];				
			}
		}
		#endregion
			
		#region Methods

		/// <summary>
		/// Constructor
		/// </summary>
		public ServiceCollection()
		{			
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="node">The xml representation</param>
		protected ServiceCollection(XmlNode node)
		{
			this.Xml = node;
		}


		/// <summary>
		/// Creates a ServiceCollection from an Xml node
		/// </summary>
		/// <param name="node">The xml node representation</param>
		/// <returns>A ServiceCollection object</returns>
		public static ServiceCollection FromXml(XmlNode node)
		{
			return new ServiceCollection(node);
		}

		/// <summary>
		/// Removes a IService from the list
		/// </summary>
		/// <param name="service">The service to add</param>
		public void Add(Service service)
		{
			lock (this.List)
			{
				if (!this.Contains(service))
					this.List.Add(service);
			}
		}

		/// <summary>
		/// Checks if the collection contains a service
		/// </summary>
		/// <param name="service">The service to look for</param>
		/// <returns>True if the collection contains the service, False otherwise</returns>
		public bool Contains(Service service)
		{
			lock (this.List)
			{
				return this.List.Contains(service);
			}
		}

		/// <summary>
		/// Removes a service from the collection
		/// </summary>
		/// <param name="service">The service to remove</param>
		public void Remove(Service service)
		{
			lock (this.List)
			{
				this.List.Remove(service);
			}
		}		
		#endregion		

		#region Xml Get/Set
		[Browsable(false)]
		public virtual XmlNode Xml
		{
			get
			{
				XmlDocument xmlDoc = new XmlDocument();
				XmlElement pe = xmlDoc.CreateElement("Services");

				foreach (Service s in this)
				{
					pe.AppendChild(xmlDoc.ImportNode(s.Xml,true));
				}

				xmlDoc.AppendChild(pe);

				return pe;
			}
			set
			{
				XmlNode node = value;

				if (node != null)
				{
					
					XmlNodeList nl = node.SelectNodes("Service");
					foreach (XmlNode n in nl)
					{
						this.Add(Service.GetPsuedoServiceFromXml(n));
					}
				}
			}
		}
		#endregion
	}
}
