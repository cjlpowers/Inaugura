#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#endregion


namespace Inaugura.RealLeads
{
	public class VoiceMail : Inaugura.Xml.IXmlable
	{
		public enum VoiceMailStatus
		{
			New = 1,
			Old = 2
		}

		#region Variables
		private Details mDetails = null;
		private XmlNode mXml = null;
		#endregion

		#region IXmlable Members
		/// <summary>
		/// Gets the xml representation of the Zone
		/// </summary>
		/// <value></value>
		public XmlNode Xml
		{
			get
			{
				return this.mXml.Clone();
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// The ID of the Voice Mail
		/// </summary>
		/// <value></value>		
		public string ID
		{
			get
			{
				return Inaugura.Xml.Helper.GetAttribute(this.mXml, "id");
			}
			private set
			{
				Inaugura.Xml.Helper.SetAttribute(this.mXml, "id", value);
			}
		}

	
		/// <summary>
		/// Additional details specific to this Voice Mail
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

		/// <summary>
		/// The owner GUID
		/// </summary>
		/// <value></value>
		public string AgentID
		{
			get
			{
				return Inaugura.Xml.Helper.GetAttribute(this.mXml, "agentID");
			}
			protected set
			{
				Inaugura.Xml.Helper.SetAttribute(this.mXml, "agentID", value);
			}
		}


		/// <summary>
		/// The date and time the voice mail was created
		/// </summary>
		/// <value></value>
		public DateTime Date
		{
			get
			{
				string value = Inaugura.Xml.Helper.GetAttribute(this.mXml, "date");
				return DateTime.Parse(value);
			}
			protected set
			{
				Inaugura.Xml.Helper.SetAttribute(this.mXml, "date", value.ToUniversalTime().ToString("o", System.Globalization.CultureInfo.InvariantCulture));
			}
		}

		/// <summary>
		/// The voice mail status
		/// </summary>
		/// <value></value>
		public VoiceMailStatus Status
		{
			get
			{
				string statusStr = Inaugura.Xml.Helper.GetAttribute(this.mXml, "status");
				return (VoiceMailStatus)Enum.Parse(typeof(VoiceMailStatus), statusStr);
			}
			set
			{
				Inaugura.Xml.Helper.SetAttribute(this.mXml, "status", value.ToString());
			}
		}

		/// <summary>
		/// The File ID of the Voice Mail
		/// </summary>
		/// <value></value>		
		public string FileID
		{
			get
			{
				return Inaugura.Xml.Helper.GetAttribute(this.mXml, "fileId");
			}
			protected set
			{
				Inaugura.Xml.Helper.SetAttribute(this.mXml, "fileId", value);
			}
		}

		/// <summary>
		/// The ani caller id of caller who left the message
		/// </summary>
		/// <value></value>
		public string CallerID
		{
			get
			{
				return Inaugura.Xml.Helper.GetAttribute(this.mXml, "callerId");
			}
			protected set
			{
				Inaugura.Xml.Helper.SetAttribute(this.mXml, "callerId", value);
			}
		}
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="xml">The xml representation of the Zone</param>
		public VoiceMail(XmlNode voiceMailNode)
		{

			if (voiceMailNode == null)
				throw new ArgumentNullException("The Xml definition may not be null");

			this.mXml = voiceMailNode;

			//make sure the xml definition specifies an ID
			if (this.ID == string.Empty)
				throw new ArgumentException("The Xml definition of the must specify a valid ID property.");

			Inaugura.Xml.Helper.SetTypeAttribute(this.mXml, this.GetType());

			// Set up the Details node
			XmlNode node = this.mXml.SelectSingleNode("Details");
			if (node == null)
			{
				node = this.mXml.OwnerDocument.CreateElement("Details");
				this.mXml.AppendChild(node);
			}
			this.mDetails = new Details(node);
		}

		/// <summary>
		/// Constructor
		/// </summary>		
		public VoiceMail(string agentID, string fileID, string callerID) : this(VoiceMail.GetBaseXml())
		{
			this.Status = VoiceMailStatus.New;
			this.Date = DateTime.Now;
			this.AgentID = agentID;
			this.FileID = fileID;
			this.CallerID = callerID;
		}

		private static XmlNode GetBaseXml()
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", String.Empty, String.Empty));
			XmlElement node = xmlDoc.CreateElement("VoiceMail");
			xmlDoc.AppendChild(node);
			Inaugura.Xml.Helper.SetAttribute(node, "id", Guid.NewGuid().ToString());
			return node;
		}

		/// <summary>
		/// Creates a Voice Mail instance from an xml node representation
		/// </summary>
		/// <param name="xml">The xml which represents the item</param>
		/// <returns>A VoiceMail instance</returns>
		public static VoiceMail FromXml(XmlNode xml)
		{
			return Inaugura.Xml.Helper.GetIXmlableFromXml(xml) as VoiceMail;
		}
	}
}
