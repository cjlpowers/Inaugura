using System;
using System.Xml;
using System.Drawing.Design;
using System.ComponentModel;
using System.Collections.Generic;

namespace Inaugura.Telephony
{

	#region TelephonyHardwareEventArgs
	/// <summary>
	/// The telephony hardware event args
	/// </summary>
	public class TelephonyHardwareEventArgs : EventArgs
	{
		private TelephonyHardware mHardware; // The 

		/// <summary>
		/// The Telephony Hardware
		/// </summary>
		/// <value></value>
		public TelephonyHardware Hardware
		{
			get { return mHardware; }
		}

		/// <summary>
		/// Constructor 
		/// </summary>
		/// <param name="hardware">The telephony hardware</param>
		public TelephonyHardwareEventArgs(TelephonyHardware hardware)
		{
			if(hardware == null)
				throw new ArgumentNullException("hardware","The hardware argument can not be null");
			this.mHardware = hardware;
		}
	}
	#endregion

	public delegate void TelephonyHardwareHandler(object sender, TelephonyHardwareEventArgs e);

	/// <summary>
	/// 
	/// </summary>
	/// 	
	[Editor(typeof(TelephonyHardwareEditor), typeof(UITypeEditor))]
	public abstract class TelephonyHardware : IStatusable, Inaugura.Xml.IXmlable
	{
		#region Internal Classes
		private class PseudoTelephonyHardware: TelephonyHardware
		{
			private string mName;
			private string mType;
			private string mAssembly;

			public PseudoTelephonyHardware(XmlNode node) : base(node)
			{
			}

			public override TelephonyLine[] Start()
			{
				throw new NotImplementedException();
			}


			public override void Stop()
			{
				throw new NotImplementedException();
			}



			public override string Name
			{
				get 
				{
					return this.mName;
				}				
			}


			public override XmlNode Xml
			{
				get
				{
					XmlNode node = base.Xml;
					if (this.mType != null)
						((XmlElement)node).SetAttribute("type", this.mType);					

					if (this.mAssembly != null)
						((XmlElement)node).SetAttribute("assembly", this.mAssembly);

					if (this.mAssembly != null)
						((XmlElement)node).SetAttribute("name", this.Name);					

					return node;
				}
				set
				{
					base.Xml = value;
					XmlNode node = value;
					if (node != null)
					{
						XmlAttribute a;
						if ((a = node.Attributes["type"]) != null)
							this.mType = a.Value;

						if ((a = node.Attributes["assembly"]) != null)
							this.mAssembly = a.Value;
						
						if ((a = node.Attributes["name"]) != null)
							this.mName = a.Value;
					}
				}
			}


		}
		#endregion

		#region Events
		public event TelephonyHardwareHandler HardwareStarting;		// The starting event
		public event TelephonyHardwareHandler HardwareStarted;		// The started event
		public event TelephonyHardwareHandler HardwareStopping;		// The stopping event
		public event TelephonyHardwareHandler HardwareStopped;		// The stopped event

		public event TelephonyLineHandler LineCreated;				// The line created event

		public event StatusHandler StatusChanged;				// The status changed event

		#region Event Callers
		/// <summary>
		/// Event Caller
		/// </summary>
		protected void OnHardwareStartingEvent()
		{
			if (this.HardwareStarting != null)
			{
				TelephonyHardwareEventArgs args = new TelephonyHardwareEventArgs(this);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.HardwareStarting, false, new object[] { this, args });
			}
		}

		/// <summary>
		/// Event Caller
		/// </summary>
		protected void OnHardwareStartedEvent()
		{
			if (this.HardwareStarted != null)
			{
				TelephonyHardwareEventArgs args = new TelephonyHardwareEventArgs(this);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.HardwareStarted, false, new object[] { this, args });
			}
		}

		/// <summary>
		/// Event Caller
		/// </summary>
		protected void OnHardwareStoppingEvent()
		{
			if (this.HardwareStopping != null)
			{
				TelephonyHardwareEventArgs args = new TelephonyHardwareEventArgs(this);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.HardwareStopping, false, new object[] { this, args });
			}
		}

		/// <summary>
		/// Event Caller
		/// </summary>
		protected void OnHardwareStoppedEvent()
		{
			if (this.HardwareStopped != null)
			{
				TelephonyHardwareEventArgs args = new TelephonyHardwareEventArgs(this);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.HardwareStopped, false, new object[] { this, args });
			}
		}

		/// <summary>
		/// Event Caller
		/// </summary>
		protected void OnStatusChangedEvent()
		{
			if (this.StatusChanged != null)
			{
				StatusEventArgs args = new StatusEventArgs(this);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.StatusChanged, false, new object[] { this, args });
			}
		}

		/// <summary>
		/// Calls the line created event
		/// </summary>
		/// <param name="line">The line that was created</param>
		protected void OnLineCreatedEvent(TelephonyLine line)
		{
			if (this.LineCreated != null)
			{
				TelephonyLineEventArgs args = new TelephonyLineEventArgs(line);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.LineCreated, false, new object[] { this,args });
			}
		}
		#endregion

		#endregion

		#region Variables
		private string mStatus = string.Empty;			// The hardware status
		private Details mDetails;								// Additional space for service settings
		#endregion

		#region Properties

		#region Xml Get/Set
		public virtual XmlNode Xml
		{
			get
			{
				XmlDocument xmlDoc = new XmlDocument();
				XmlElement pe = xmlDoc.CreateElement("TelephonyHardware");
								
				if (this.GetType() != typeof(TelephonyHardware))
					pe.SetAttribute("type", this.GetType().FullName);
				// if the type is located in another assembly also include the assembly file name
				if (this.GetType().Assembly != System.Reflection.Assembly.GetExecutingAssembly())
					pe.SetAttribute("assembly", this.GetType().Assembly.GetName().FullName);
				
				pe.SetAttribute("name", this.Name);
				
				pe.AppendChild(xmlDoc.ImportNode(this.Details.Xml, true));

				xmlDoc.AppendChild(pe);

				return pe;
			}
			set
			{
				XmlNode node = value;

				if (node != null)
				{
					if (node["Details"] != null)
						this.Details = new Details(node["Details"]);
				}
			}
		}
		#endregion

		public abstract string Name
		{
			get;
		}

		public string Status
		{
			get
			{
				return this.mStatus;
			}
			protected set
			{
				this.mStatus = value;
				this.OnStatusChangedEvent();
			}
		}

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

		public TelephonyHardware()
		{			
			this.Details = new Details();			
		}

		protected TelephonyHardware(XmlNode node) : this()
		{
			this.Xml = node;
		}

		public static TelephonyHardware FromXml(XmlNode node)
		{
			return Inaugura.Xml.Helper.GetIXmlableFromXml(node) as TelephonyHardware;
		}

		public abstract TelephonyLine[] Start();
		public abstract void Stop();

		/// <summary>
		/// Creats a pseudo hardware instance from an xml representation
		/// </summary>
		/// <param name="node">The xml node represenation</param>
		/// <returns>The pseudo hardware object</returns>
		public static TelephonyHardware GetPsuedoHardwareFromXml(XmlNode node)
		{
			return new PseudoTelephonyHardware(node);
		}

	}
}
