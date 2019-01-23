#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#endregion

namespace Inaugura.Communication.Messaging
{
	public abstract class XmlMessage : XmlDocument
	{
		#region Properties
		public DateTime TimeStamp
		{
			get
			{
				if (this["XmlMessage"].Attributes["timeStamp"] == null)
					return DateTime.MinValue;
				else
					return DateTime.Parse(this["XmlMessage"].Attributes["timeStamp"].Value);
			}
			protected set
			{
				this["XmlMessage"].SetAttribute("timeStamp", value.ToUniversalTime().ToString());
			}
		}

		public string Type
		{
			get
			{
				if (this["XmlMessage"].Attributes["type"] == null)
					return null;
				else
					return this["XmlMessage"].Attributes["type"].Value;
			}
			protected set
			{
				this["XmlMessage"].SetAttribute("type", value);
			}
		}

		public string Assembly
		{
			get
			{
				if (this["XmlMessage"].Attributes["assembly"] == null)
					return null;
				else
					return this["XmlMessage"].Attributes["assembly"].Value;
			}
			protected set
			{
				this["XmlMessage"].SetAttribute("assembly", value);
			}

		}
		#endregion

		public XmlMessage()
		{
			XmlElement e = this.CreateElement("XmlMessage");
			this.AppendChild(e);

			this.TimeStamp = DateTime.Now;
			this.Type = this.GetType().FullName;
			this.Assembly = this.GetType().Assembly.FullName;
		}

		public XmlMessage(XmlDocument xmlDoc)
		{
			XmlNode node = this.ImportNode(xmlDoc["XmlMessage"], true);
			this.AppendChild(node);
		}

		static public XmlMessage FromXml(XmlDocument xmlDocument)
		{
			string typeStr = xmlDocument["XmlMessage"].Attributes["type"].Value;
			string assemblyStr = xmlDocument["XmlMessage"].Attributes["assembly"].Value;

			System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(assemblyStr);
			Type type = assembly.GetType(typeStr);

			XmlMessage msg = System.Activator.CreateInstance(type, new object[] { xmlDocument }) as XmlMessage;

			return msg;
		}

		static public XmlMessage FromXml(string xml)
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xml);
			return XmlMessage.FromXml(xmlDoc);
		}
	}
}
