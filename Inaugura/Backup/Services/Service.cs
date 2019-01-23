using System;
using System.Threading;
using System.Xml;

using Inaugura.Telephony;

namespace Inaugura.Telephony.Services
{

	#region ServiceEventArgs
	/// <summary>
	/// The telephony hardware event args
	/// </summary>
	public class ServiceEventArgs : EventArgs
	{
		private Service mService; // The 

		/// <summary>
		/// The Service
		/// </summary>
		/// <value></value>
		public Service Service
		{
			get { return mService; }
		}

		/// <summary>
		/// Constructor 
		/// </summary>
		/// <param name="service">The service</param>
		public ServiceEventArgs(Service service)
		{
			if (service == null)
				throw new ArgumentNullException("service", "The service argument can not be null");
			this.mService = service;
		}
	}
	#endregion

	public delegate void ServiceHandler(object sender, ServiceEventArgs e);

	/// <summary>
	/// Base class for all Services
	/// </summary>
	public abstract class Service: ServiceLineCollection, IStatusable, Inaugura.Xml.IXmlable
	{
		#region Internal Classes
		private class PseudoService : Service
		{
			private string mName;
			private string mId;
			private string mType = "";
			private string mAssembly = "";

			public PseudoService(XmlNode node) : base(node)
			{
			}

			public override string Id
			{
				get 
				{
					return this.mId;
				}
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
				
					if (this.mId != null)
						((XmlElement)node).SetAttribute("id", this.Id);
				
					if (this.mName != null)
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

						if ((a = node.Attributes["id"]) != null)
							this.mId = a.Value;

						if ((a = node.Attributes["name"]) != null)
							this.mName = a.Value;
					}
				}
			}

			protected override void ProcessService()
			{
				throw new NotImplementedException();
			}

			public override void SupplyLine(TelephonyLine line)
			{
				throw new NotImplementedException();
			}

		}
		#endregion

		#region Events
		public event ServiceHandler ServiceStarting;		// The starting event
		public event ServiceHandler ServiceStarted;		// The started event
		public event ServiceHandler ServiceStopping;		// The stopping event
		public event ServiceHandler ServiceStopped;		// The stopped event
		public event ServiceHandler ServiceActive;			// The active event
		public event ServiceHandler ServiceInactive;		// The inactive event
		
		public event ServiceHandler LineShortage;			// Called when the service wants lines
		public event ServiceHandler LineSurplus;				// Called when the service has too many lines

		public event Inaugura.Telephony.StatusHandler StatusChanged;	// The status changed event

		#region Event Callers
		/// <summary>
		/// Event Caller
		/// </summary>
		private void OnServiceStartingEvent()
		{
			if(this.ServiceStarting != null)
			{
				ServiceEventArgs args = new ServiceEventArgs(this);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.ServiceStarting, false, new object[] { this, args });
			}
		}

		/// <summary>
		/// Event Caller
		/// </summary>
		private void OnServiceStartedEvent()
		{
			if (this.ServiceStarted != null)
			{
				ServiceEventArgs args = new ServiceEventArgs(this);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.ServiceStarted, false, new object[] { this, args });
			}
		}

		/// <summary>
		/// Event Caller
		/// </summary>
		private void OnServiceStoppingEvent()
		{
			if(this.ServiceStopping != null)
			{
				ServiceEventArgs args = new ServiceEventArgs(this);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.ServiceStopping, false, new object[] { this, args });
			}
		}

		/// <summary>
		/// Event Caller
		/// </summary>
		private void OnServiceStoppedEvent()
		{
			if (this.ServiceStopped != null)
			{
				ServiceEventArgs args = new ServiceEventArgs(this);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.ServiceStopped, false, new object[] { this, args });
			}
		}

		/// <summary>
		/// Event Caller
		/// </summary>
		private void OnServiceActiveEvent()
		{
			if (this.ServiceActive != null)
			{
				ServiceEventArgs args = new ServiceEventArgs(this);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.ServiceActive, false, new object[] { this, args });
			}
		}

		/// <summary>
		/// Event Caller
		/// </summary>
		private void OnServiceInactiveEvent()
		{
			if (this.ServiceInactive != null)
			{
				ServiceEventArgs args = new ServiceEventArgs(this);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.ServiceInactive, false, new object[] { this, args });
			}
		}

		/// <summary>
		/// Event Caller
		/// </summary>
		private void OnStatusChangedEvent()
		{
			if (this.StatusChanged != null)
			{
				StatusEventArgs args = new StatusEventArgs(this);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.StatusChanged, false, new object[] { this, args });
			}
		}

		/// <summary>
		/// Event Caller
		/// </summary>
		private void OnLineShortageEvent()
		{
			if (this.LineShortage != null)
			{
				ServiceEventArgs args = new ServiceEventArgs(this);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.LineShortage, false, new object[] { this, args });
			}
		}

		/// <summary>
		/// Event Caller
		/// </summary>
		private void OnLineSurplusEvent()
		{
			if (this.LineSurplus != null)
			{
				ServiceEventArgs args = new ServiceEventArgs(this);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.LineSurplus, false, new object[] { this, args });
			}
		}
		#endregion

		#endregion

		#region Variables
		private bool mStarted = false;					// The started state
		private string mStatus = "";					// The service status
		private int mCallsProcessed = 0;				// The number of calls processed by this service
		private Thread mThread;							// The service thread
		private float mImportance;						// The importance of the service
		private int mMinLines = 1;						// The minimum number of lines
		private int mMaxLines = 20;						// The maximum number of lines
		private float mIdealIdleSeconds = 10;			// The ideal number of seconds a line should be idle
		private ServiceLineHandler mLineActive;			// The handler for a line active event
		private ServiceLineHandler mLineInactive;		// The handler for a line inactive event
		private ServiceLineHandler mLineTerminated;		// The handler for a line inactive event
		private Details mDetails;						// Additional space for service settings
		#endregion

		#region Properties

		/// <summary>
		/// The unique GUID for the service
		/// </summary>
		/// <value></value>
		public abstract string Id
		{
			get;
		}

		/// <summary>
		/// The name of the service
		/// </summary>
		/// <value></value>
		public abstract string Name
		{
			get;			
		}

		/// <summary>
		/// The number of calls processed by this service
		/// </summary>
		/// <value></value>
		public int CallsProcessed
		{
			get
			{
				return this.mCallsProcessed;
			}
			set // protected
			{
				this.mCallsProcessed = value;
			}
		}

		/// <summary>
		/// The services active state
		/// </summary>
		/// <value></value>
		public bool Active
		{
			get
			{
				foreach (ServiceLine line in this)
				{
					if (line.Active == true)
						return true;
				}
				return false;
			}			
		}

		/// <summary>
		/// The started state of the service
		/// </summary>
		/// <value></value>
		public bool Started
		{
			get
			{
				return this.mStarted;				
			}
			set // private
			{
				this.mStarted = value;
			}
		}		

		/// <summary>
		/// The state of the service processing thread
		/// </summary>
		/// <value></value>
		public ThreadState ThreadState
		{
			get
			{
				return this.mThread.ThreadState;
			}
		}
		
		/// <summary>
		/// The services importance value
		/// </summary>
		/// <value></value>
		public float Importance
		{
			get
			{
				return this.mImportance;
			}
			set
			{
				this.mImportance = value;
			}
		}

		/// <summary>
		/// The minimum number of lines this service may have at any given time
		/// </summary>
		/// <value></value>
		public int MinimumNumberOfLines
		{
			get
			{
				return this.mMinLines;
			}
			set
			{
				this.mMinLines = value;
			}
		}

		/// <summary>
		/// The maximum number of lines the service may have at any given time
		/// </summary>
		/// <value></value>
		public int MaximumNumberOfLines
		{
			get
			{
				return this.mMaxLines;
			}
			set
			{
				this.mMaxLines = value;
			}
		}

		/// <summary>
		/// The average amount of time that all lines have been idle
		/// </summary>
		/// <value></value>
		public virtual TimeSpan IdleTime
		{
			get
			{
				int count = 0;
				float idleSum = 0; 
				lock (this.List)
				{
					foreach(ServiceLine line in this)
					{
						if (line.IdleTime != TimeSpan.MinValue)
						{
							idleSum += (float)line.IdleTime.TotalSeconds;
							count++;
						}
					}					
				}

				if (count != 0)
				{					
					// if we are outside of the 10 percent grace
					float difference = idleSum - this.IdealIdleSeconds;
					if (Math.Abs(difference) > this.IdealIdleSeconds * ServiceManager.IdleGraceFactor)
					{
						if (difference < 0)
							this.OnLineShortageEvent();
						else if (difference > 0)
							this.OnLineSurplusEvent();
					}
					TimeSpan result = new TimeSpan(0, 0,(int)(idleSum / count));
					//this.Status = "Idle " + result.TotalSeconds.ToString();
					return result;
				}
				return TimeSpan.MinValue;				
			}
		}

		/// <summary>
		/// The ideal number of seconds that a line should be idle
		/// </summary>
		/// <value></value>
		public float IdealIdleSeconds
		{
			get
			{
				return this.mIdealIdleSeconds;
			}
			set
			{
				this.mIdealIdleSeconds = value;
			}
		}

		/// <summary>
		/// The status of the service
		/// </summary>
		/// <value></value>
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

		#region Methods
		/// <summary>
		/// Constructor
		/// </summary>
		protected Service()
		{
			this.mThread = new Thread(new ThreadStart(this.ProcessService));
			this.ServiceLineAdded +=new ServiceLineHandler(Service_ServiceLineAdded);
			this.ServiceLineRemoved += new ServiceLineHandler(Service_ServiceLineRemoved);

			this.mLineActive += new ServiceLineHandler(this.serviceLine_ServiceLineActive);
			this.mLineInactive += new ServiceLineHandler(this.serviceLine_ServiceLineInactive);
			this.mLineTerminated += new ServiceLineHandler(this.serviceLine_ServiceTerminated);

			this.Details = new Details();			
		}


		protected Service(XmlNode node) : this()
		{
			this.Xml = node;
		}


		protected abstract void ProcessService();

		/// <summary>
		/// Removes a ServiceLine from the list
		/// </summary>
		/// <param name="line">The line to remove</param>
		protected override void Remove(ServiceLine line)
		{
			// Todo either give the line back to the telephony manager here or let the service manager catch the event
			base.Remove(line);
		}

		/// <summary>
		/// Line added handler
		/// </summary>
		/// <param name="serviceLine">The service line added</param>
		private void Service_ServiceLineAdded(object sender, ServiceLineEventArgs e)
		{			
			e.ServiceLine.ServiceLineActive += this.mLineActive;
			e.ServiceLine.ServiceLineInactive += this.mLineInactive;
			e.ServiceLine.ServiceLineTerminated += this.mLineTerminated;

			if(this.Started)
				e.ServiceLine.Start();
		}
		/// <summary>
		/// Line removed handler
		/// </summary>
		/// <param name="serviceLine">The service line removed</param>
		private void Service_ServiceLineRemoved(object sender, ServiceLineEventArgs e)
		{
			e.ServiceLine.ServiceLineActive -= this.mLineActive;
			e.ServiceLine.ServiceLineInactive -= this.mLineInactive;
		}
		/// <summary>
		/// Line active handler
		/// </summary>
		/// <param name="serviceLine">The active service line</param>
		private void serviceLine_ServiceLineActive(object sender, ServiceLineEventArgs e)
		{
			lock (this.List)
			{
				foreach (ServiceLine line in this)
				{
					if (line.Active && e.ServiceLine != line)
					{
						return;
					}
				}

				// the service just became active 
				this.OnServiceActiveEvent();
			}
		}

		/// <summary>
		/// Line inactive handler 
		/// </summary>
		/// <param name="serviceLine">The line which became inactive</param>
		private void serviceLine_ServiceLineInactive(object sender, ServiceLineEventArgs e)
		{
			lock (this.List)
			{
				foreach (ServiceLine line in this)
				{
					if (line.Active)
					{
						return;
					}
				}

				// the service just became inactive 
				this.OnServiceInactiveEvent();
			}
		}


		/// <summary>
		/// Removes a stopped line from the list
		/// </summary>
		/// <param name="serviceLine">The line which has been stopped</param>
		private void serviceLine_ServiceTerminated(object sender, ServiceLineEventArgs e)
		{
			e.ServiceLine.ServiceLineTerminated -= this.mLineTerminated;
			this.Remove(e.ServiceLine);
		}		

		public override string ToString()
		{
			return this.Name;
		}

		/// <summary>
		/// Creats a service from an xml representation
		/// </summary>
		/// <param name="node">The xml node represenation</param>
		/// <returns>The service object</returns>
		public static Service FromXml(XmlNode node)
		{
			return Inaugura.Xml.Helper.GetIXmlableFromXml(node) as Service;			
		}


		/// <summary>
		/// Creats a pseudo service instance from an xml representation
		/// </summary>
		/// <param name="node">The xml node represenation</param>
		/// <returns>The pseudo service object</returns>
		public static Service GetPsuedoServiceFromXml(XmlNode node)
		{
			return new PseudoService(node);
		}

		#region IService Members	
		/// <summary>
		/// Starts the service
		/// </summary>				
		public virtual void Start()
		{			
			Thread t = new Thread(new ThreadStart(this.RunStart));
			t.Start();
			
		}

		private void RunStart()
		{			
			this.OnServiceStartingEvent();
			this.mThread.Start();

			lock (this.List)
			{
				foreach (ServiceLine line in this)
				{
					line.Start();
				}
			}
			this.Started = true;
			this.OnServiceStartedEvent();
		}

		/// <summary>
		/// Stops the service
		/// </summary>
		public virtual void Stop()
		{
			if (this.Started)
			{
				Thread t = new Thread(this.RunStop);
				t.Start();
			}
		}

		private void RunStop()
		{
				this.OnServiceStoppingEvent();
				lock (this.List)
				{
					foreach (ServiceLine line in this)
					{
						line.Terminate();						
					}
				}
		
				while (this.Count != 0)
				{
					Thread.Sleep(100);
				}

				this.mThread.Join();
				//while (this.mThread.ThreadState == ThreadState.Running)
				//{
				//	Thread.Sleep(100);
				//}

				this.Started = false;		
		}		

		public abstract void SupplyLine(TelephonyLine line);

		public virtual void FreeLines(int numberOfLinesToFree)
		{
			int count = numberOfLinesToFree;
			while (this.Count > this.MinimumNumberOfLines && count > 0)
			{
				// get the line with the greates idle time
				TimeSpan span = TimeSpan.MinValue;
				ServiceLine lineToRemove = null;
				lock (this.List)
				{
					foreach (ServiceLine line in this)
					{
						if (lineToRemove == null)
						{
							lineToRemove = line;
							span = line.IdleTime;
						}
						else
						{
							if (span == TimeSpan.MinValue)
							{
								span = line.IdleTime;
								lineToRemove = line;
							}
							else if (line.IdleTime != TimeSpan.MinValue)
							{
								if (span < line.IdleTime)
								{
									lineToRemove = line;
									span = line.IdleTime;
								}
							}
						}					
					}
				}

				if (lineToRemove != null)
					lineToRemove.Terminate();

				count--;
			}
		}
		
		#endregion
		#endregion

		#region Xml Get/Set		
		public virtual XmlNode Xml
		{
			get
			{
				XmlDocument xmlDoc = new XmlDocument();
				XmlElement pe = xmlDoc.CreateElement("Service");

				if (this.GetType() != typeof(Service))
					pe.SetAttribute("type", this.GetType().FullName);
				// if the type is located in another assembly also include the assembly file name
				if (this.GetType().Assembly != System.Reflection.Assembly.GetExecutingAssembly())
					pe.SetAttribute("assembly", this.GetType().Assembly.GetName().FullName);

				pe.SetAttribute("id",this.Id);
				pe.SetAttribute("name", this.Name);
				pe.SetAttribute("importance", this.Importance.ToString());
				pe.SetAttribute("idealIdleSeconds", this.IdealIdleSeconds.ToString());
				pe.SetAttribute("minimumLines", this.MinimumNumberOfLines.ToString());
				pe.SetAttribute("maximumLines", this.MaximumNumberOfLines.ToString());
						
				pe.AppendChild(xmlDoc.ImportNode(this.Details.Xml, true));

				xmlDoc.AppendChild(pe);

				return pe;
			}
			set
			{
				XmlNode node = value;

				if (node != null)
				{
					XmlAttribute a;

					if ((a = node.Attributes["importance"]) != null)
						this.Importance = float.Parse(a.Value);

					if ((a = node.Attributes["idealIdleSeconds"]) != null)
						this.IdealIdleSeconds = float.Parse(a.Value);

					if ((a = node.Attributes["minimumLines"]) != null)
						this.MinimumNumberOfLines = int.Parse(a.Value);

					if ((a = node.Attributes["maximumLines"]) != null)
						this.MaximumNumberOfLines = int.Parse(a.Value);
					
					//if (node["ReservedLines"] != null)
					//	this.ReservedLineNames = StringList.FromXml(node["ReservedLines"], "ReservedLines", "Line");

					//if (node["FileReferenceList"] != null)
					//	this.FileReferences = FileReferenceList.FromXml(node["FileReferenceList"]);

					if (node["Details"] != null)
						this.Details = new Details(node["Details"]);
				}
			}
		}
		#endregion
	}
}
