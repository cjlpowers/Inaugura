using System;
using System.Xml;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

using Inaugura.Xml;
using Inaugura.Telephony.Services;


namespace Inaugura.Telephony
{
	#region ServiceEventArgs
	/// <summary>
	/// The telephony hardware event args
	/// </summary>
	public class SwitchEventArgs : EventArgs
	{
		private Switch mSwitch;

		/// <summary>
		/// The Service
		/// </summary>
		/// <value></value>
		public Switch Switch
		{
			get { return mSwitch; }
		}

		/// <summary>
		/// Constructor 
		/// </summary>
		/// <param name="sw">The switch</param>
		public SwitchEventArgs(Switch sw)
		{
			if (sw == null)
				throw new ArgumentNullException("sw", "The switch argument can not be null");
			this.mSwitch = sw;
		}
	}
	#endregion

	public delegate void SwitchHandler(object sender, SwitchEventArgs e);

	/// <summary>
	/// Summary description for Switch.
	/// </summary>
	public class Switch : IXmlable
	{
		#region Events
		public event SwitchHandler SwitchStarted;
		public event SwitchHandler SwitchStopped;
		#endregion

		#region Variables		
		private static string runDirectory = AppDomain.CurrentDomain.BaseDirectory;
		private static Switch activeSwitch;
		private static LanguageList languages;
		private string mId;
		private string mName;
		private bool mStarted = false;
		private TelephonyManager mTelephonyManager;
		private ServiceManager mServiceManager;
		
		#endregion

		#region Properties

		/// <summary>
		/// The directory in which the switch started
		/// </summary>
		/// <value></value>
		public static string RunDirectory
		{
			get
			{
				return Switch.runDirectory;
			}
		}

		/// <summary>
		/// Gets the currently running switch
		/// </summary>
		/// <value></value>
		[Browsable(false)]
		public static Switch ActiveSwitch
		{
			get
			{
				return Switch.activeSwitch;
			}
			private set
			{
				Switch.activeSwitch = value;
			}
		}

		/// <summary>
		/// The languages that the switch supports
		/// </summary>
		/// <value></value>
		public LanguageList Languages
		{
			get
			{
				return Switch.languages;
			}
			private set
			{
				Switch.languages = value;
			}
		}


		/// <summary>
		/// The switch GUID
		/// </summary>
		/// <value></value>
		public string Id
		{
			get
			{
				return this.mId;
			}
			private set
			{
				this.mId = value;
			}
		}

		/// <summary>
		/// The switch name
		/// </summary>
		/// <value></value>
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
		/// The started sate of the switch
		/// </summary>
		/// <value></value>
		[Browsable(false)]
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

		/// <summary>
		/// The service manager
		/// </summary>
		/// <value></value>
		public ServiceManager ServiceManager
		{
			get
			{
				return this.mServiceManager;
			}
			private set
			{
				this.mServiceManager = value;
			}
		}

		public TelephonyManager TelephonyManager
		{
			get
			{
				return this.mTelephonyManager;
			}
			private set
			{
				this.mTelephonyManager = value;
			}
		}
		#endregion

		#region IXmlable Members
		[Browsable(false)]
		public XmlNode Xml
		{
			get
			{
				XmlDocument xmlDoc = new XmlDocument();
				XmlElement pe = xmlDoc.CreateElement("Switch");

				if (this.GetType() != typeof(Switch))
					pe.SetAttribute("type", this.GetType().FullName);
				// if the type is located in another assembly also include the assembly file name
				if (this.GetType().Assembly != System.Reflection.Assembly.GetExecutingAssembly())
					pe.SetAttribute("assembly", this.GetType().Assembly.GetName().FullName);

				pe.SetAttribute("id", this.Id);
				pe.SetAttribute("name", this.Name);				

				pe.AppendChild(xmlDoc.ImportNode(this.ServiceManager.Xml, true));
				pe.AppendChild(xmlDoc.ImportNode(this.TelephonyManager.Xml, true));
				//pe.AppendChild(xmlDoc.ImportNode(this.Languages.Xml, true));

				xmlDoc.AppendChild(pe);

				return pe;
			}
			set
			{
				XmlNode node = value;

				if (node != null)
				{
					XmlAttribute a;

					if ((a = node.Attributes["id"]) != null)
						this.Id = a.Value;

					if ((a = node.Attributes["name"]) != null)
						this.Name = a.Value;				

					if (node["ServiceManager"] != null)
						this.ServiceManager = new ServiceManager(node["ServiceManager"]);

					if (node["TelephonyManager"] != null)
						this.TelephonyManager = new TelephonyManager(node["TelephonyManager"]);

					if (node["Languages"] != null)
					{
						foreach (XmlNode languageNode in node["Languages"].SelectNodes("Language"))
						{
							Language language = (Language)Enum.Parse(typeof(Language), languageNode.InnerText);
							if (language == Language.Unknown)
								throw new System.Configuration.ConfigurationException("The lanugage 'Unknown' is not supported");
							else
								this.Languages.Add(language);
						}
					}					
				}				
			}
		}

		#endregion

		#region Methods
		/// <summary>
		/// Constructor
		/// </summary>
		public Switch()
		{
			this.Id = Guid.NewGuid().ToString();			
			Switch.ActiveSwitch = this;
			this.Languages = new LanguageList();			
			this.TelephonyManager = new TelephonyManager();
			this.ServiceManager = new ServiceManager();
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="node">The xml node representation of the switch</param>
		public Switch(XmlNode node) : this()
		{
			if (node == null)
				throw new ArgumentNullException("node", "node can not be null");
			this.Xml = node;
		}

		public void Start()
		{
			if (this.Started)
				return;

			Log.AddLog("OrbisSwitch Starting...");
			Log.AddLog("Current Directory: " + System.IO.Directory.GetCurrentDirectory());
			Log.AddLog("Starting Managers...");
			Log.Right();

			this.TelephonyManager.Start();
			this.ServiceManager.Start();

			Log.Left();
			Log.AddLog("Managers Started...");
			Log.AddLog("OrbisSwitch Started...");
			this.Started = true;
			this.OnSwitchStartedEvent();

		}

		public void Stop()
		{
			if (!this.Started)
				return;

			Log.AddLog("OrbisSwitch Stopping...");
			Log.AddLog("Stopping Managers...");
			Log.Right();
			
			this.ServiceManager.Stop();
			this.TelephonyManager.Stop();

			Log.Left();
			Log.AddLog("Managers Stopped...");
			Log.AddLog("OrbisSwitch Stopped...");
			this.Started = false;
			this.OnSwitchStoppedEvent();
		}

		/// <summary>
		/// Restarts the server
		/// </summary>
		public void Restart()
		{
			if (this.Started)
			{
				this.Stop();
			}
			this.Start();
		}

		/// <summary>
		/// Conversion to string
		/// </summary>
		/// <returns>The string representation</returns>
		public override string ToString()
		{
			return this.Name;
		}

		#region Event Callers
		protected void OnSwitchStartedEvent()
		{
			if (this.SwitchStarted != null)
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.SwitchStarted, true, new object[] { this, new SwitchEventArgs(this) });
		}

		protected void OnSwitchStoppedEvent()
		{
			if(this.SwitchStopped != null)
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.SwitchStopped, true, new object[] { this, new SwitchEventArgs(this) });
		}
		#endregion
		#endregion		
	}
}
