using System;
using System.Collections;
using System.Xml;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Design;

using Inaugura;
using Inaugura.Xml;
using Inaugura.Telephony;


namespace Inaugura.Telephony
{
	/// <summary>
	/// 
	/// </summary>
	/// 

	[Editor(typeof(TelephonyManagerEditor),typeof(UITypeEditor))]
	public class TelephonyManager: TelephonyLineCollection, Inaugura.Xml.IXmlable, IStatusable
	{
		#region Events
		public delegate void TelephonyManagerHandler(TelephonyManager manager);
		
		public event TelephonyManagerHandler TelephonyManagerStarting;
		public event TelephonyManagerHandler TelephonyManagerStarted;
		public event TelephonyManagerHandler TelephonyManagerStopping;
		public event TelephonyManagerHandler TelephonyManagerStopped;
		
		public event TelephonyManagerHandler LineAvailable;

		public event TelephonyHardwareHandler HardwareCreated;
		public event TelephonyHardwareHandler HardwareDestroyed;
		

		public event StatusHandler StatusChanged;

		#region Event Callers
		/// <summary>
		/// Calls the starting event
		/// </summary>
		private void OnStartingEvent()
		{
			if (this.TelephonyManagerStarting != null)
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.TelephonyManagerStarting, false, new object[] { this });
		}

		/// <summary>
		/// Calls the started event
		/// </summary>
		private void OnStartedEvent()
		{
			if (this.TelephonyManagerStarted != null)
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.TelephonyManagerStarted, false, new object[] { this });
		}

		/// <summary>
		/// Calls the stopping event
		/// </summary>
		private void OnStoppingEvent()
		{
			if (this.TelephonyManagerStopping != null)
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.TelephonyManagerStopping, false, new object[] { this });
		}

		/// <summary>
		/// Calls the stopped event
		/// </summary>
		private void OnStoppedEvent()
		{
			if (this.TelephonyManagerStopped != null)
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.TelephonyManagerStopped, false, new object[] { this });
		}

		private void OnLineAvailableEvent()
		{
			if (this.LineAvailable != null)
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.LineAvailable, false, new object[] { this });
		}

		/// <summary>
		/// Calls the StatusChanged event
		/// </summary>
		private void OnStatusChangedEvent()
		{
			if (this.StatusChanged != null)
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.StatusChanged, false, new object[] { this });
		}

		/// <summary>
		/// Calls the hardware created event
		/// </summary>
		/// <param name="hardware">The hardware object that was created</param>
		private void OnHardwareCreated(TelephonyHardware hardware)
		{
			if (this.HardwareCreated != null)
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.HardwareCreated, false, new object[] { this, new TelephonyHardwareEventArgs(hardware) });
		}

		/// <summary>
		/// Calls the hardware destroyed event
		/// </summary>
		/// <param name="hardware">The hardware that was destroyed</param>
		private void OnHardwareDestroyed(TelephonyHardware hardware)
		{
			if (this.HardwareDestroyed != null)
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.HardwareDestroyed, false, new object[] { this, new TelephonyHardwareEventArgs(hardware) });
		}

		#endregion
		#endregion 		

		#region Variables		
		private TelephonyLineCollection mUsedLines;
		private TelephonyLineCollection mAvailableLines;
		
		private Inaugura.Telephony.TelephonyHardware mHardware;		
		private bool mStarted = false;
		private string mStatus = string.Empty;
		#endregion
	
		#region Properties
		[Browsable(false)]
		public XmlNode Xml
		{
			get
			{
				XmlDocument xmlDoc = new XmlDocument();
				XmlElement pe = xmlDoc.CreateElement("TelephonyManager");

				if (this.GetType() != typeof(TelephonyManager))
					pe.SetAttribute("type", this.GetType().FullName);
				// if the type is located in another assembly also include the assembly file name
				if (this.GetType().Assembly != System.Reflection.Assembly.GetExecutingAssembly())
					pe.SetAttribute("assembly", this.GetType().Assembly.GetName().FullName);

				if (this.Hardware != null && this.Hardware is IXmlable)
				{
					IXmlable xmlable = (IXmlable)this.Hardware;
					pe.AppendChild(xmlDoc.ImportNode(xmlable.Xml, true));
				}

				xmlDoc.AppendChild(pe);
				return pe;
			}
			set
			{
				XmlNode node = value;
				XmlDocument xmlDoc = node.OwnerDocument;

				if (node["TelephonyHardware"] != null)
					this.Hardware = Telephony.TelephonyHardware.GetPsuedoHardwareFromXml(node["TelephonyHardware"]);
			}
		}

		public TelephonyHardware Hardware
		{
			get
			{
				return this.mHardware;
			}
			set
			{
				this.mHardware = value;
			}
		}
		

		[Category("Settings")]
		[Browsable(false)]
		public TelephonyLineCollection AvailableLines
		{
			get
			{
				return this.mAvailableLines;
			}
			private set
			{
				this.mAvailableLines = value;
			}
		}

		[Browsable(false)]
		public TelephonyLineCollection UsedLines
		{
			get
			{
				return this.mUsedLines;
			}
			private set
			{
				this.mUsedLines = value;
			}
		}

		public bool Started
		{
			get
			{
				return this.mStarted;
			}
			private set
			{
				this.mStarted = value;
			}
		}

		#region IStatusable Members
		/// <summary>
		/// The ServiceLines status
		/// </summary>
		/// <value></value>
		public string Status
		{
			get
			{
				return this.mStatus;
			}
			set
			{
				this.mStatus = value;
				this.OnStatusChangedEvent();
			}
		}
			#endregion
		#endregion

		#region Methods
		public TelephonyManager()
		{
			this.AvailableLines = new TelephonyLineCollection();
			this.UsedLines = new TelephonyLineCollection();			
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="node">The xml node which defines this TelephonyManager</param>
		public TelephonyManager(XmlNode node) : this()
		{
			if (node == null)
				throw new ArgumentNullException("node", "node can not be null");
			this.Xml = node;
		}

		public TelephonyManager(TelephonyHardware hardware) : this()
		{		
			this.mHardware = hardware;			
		}

		public virtual void Start()
		{
			this.OnStartingEvent();

			Log.AddLog("Starting Telephony Manager");
			Log.Right();

			if (this.Hardware != null && this.Hardware is IXmlable)
			{
				Log.AddLog("Creating Hardware...");
				Log.Right();

				Log.AddLog("Loading Hardware (" + this.Hardware.Name + ")");

				// todo make sure we have all the needed files
				// create a real hardware object from the pseudo hardware object
				IXmlable xmlable = (IXmlable)this.Hardware;
				this.Hardware = TelephonyHardware.FromXml(xmlable.Xml);

				this.OnHardwareCreated(this.Hardware);

				this.AvailableLines.Clear();
				foreach (TelephonyLine l in this.Hardware.Start())
				{
					l.OpenLine();
					l.OnHook();
					Log.AddLog(l.Name + " Started");
					this.AvailableLines.Add(l);
				}
				Log.Left();
				Log.AddLog("Hardware Created");				
			}
			else
			{
				throw new ApplicationException("The telephony hardware has not been specified");
			}

			Log.Left();
			Log.AddLog("Telephony Manager Started");

			this.Started = true;
			this.OnStartedEvent();
		}

		public virtual void Stop()
		{
			this.OnStoppingEvent();
			Log.AddLog("Stopping Telephony Manager");
			Log.Right();

			this.AvailableLines.Clear();

			this.Hardware.Stop();	
			
			Log.Left();
			Log.AddLog("Telephony Manager Stopped");
			
			if (this.Hardware != null && this.Hardware is IXmlable)
			{
				TelephonyHardware oldHardware = this.Hardware;
				// create a pseudo hardware object from the hardware object
				IXmlable xmlable = (IXmlable)this.Hardware;
				this.Hardware = TelephonyHardware.GetPsuedoHardwareFromXml(xmlable.Xml);

				this.OnHardwareDestroyed(oldHardware);
			}
			
			this.OnStoppedEvent();
			this.Started = false;
		}

		public TelephonyLine GetLine()
		{
			if (this.AvailableLines.Count == 0)
				return null;

			return this.GetLine(this.AvailableLines[0].Name);
		}

		public TelephonyLine GetLine(string lineName)
		{
			TelephonyLine line = null;
			if (this.AvailableLines.Contains(lineName))
			{
				line = this.AvailableLines[lineName];
				this.AvailableLines.Remove(line);
				this.UsedLines.Add(line);
				return line;
			}
			return null;
		}

		public void ReturnLine(TelephonyLine line)
		{			
			if(this.UsedLines.Contains(line))
			{
				this.UsedLines.Remove(line);

				line.OnHook();
				line.CloseLine();

				this.AvailableLines.Add(line);
			}
		}
	
		public override string ToString()
		{
			if(this.Hardware != null)
				return "Telepyhony Manager ("+this.Hardware.Name+")";
			else
				return "Telepyhony Manager";
		}

	
		#endregion	
	}
}
