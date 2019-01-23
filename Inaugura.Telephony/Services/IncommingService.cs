using System;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.Xml;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace Inaugura.Telephony.Services
{
	/// <summary>
	/// Base for all incomming services
	/// </summary>
	public abstract class IncommingService : Service
	{
		#region Variables		
		#endregion

		#region Properties
		/// <summary>
		/// The command to dial to log on
		/// </summary>
		/// <value></value>
		[Category("UCD")]		
		public string LogOnCommand
		{
			get
			{
				return this.Details["LogOnCommand"];
			}
			set
			{
				this.Details["LogOnCommand"] = value;
			}
		}

		/// <summary>
		/// The command to dial to log off
		/// </summary>
		/// <value></value>
		[Category("UCD")]
		public string LogOffCommand
		{
			get
			{
				return this.Details["LogOffCommand"];
			}
			set
			{
				this.Details["LogOffCommand"] = value;
			}
		}
		#endregion
	
		#region Methods
		public IncommingService()
		{
			this.Details.Add("LogOnCommand", "");
			this.Details.Add("LogOffCommand", "");
		}

		public IncommingService(XmlNode node) : base(node)
		{
		}
		#endregion		
	}
}
