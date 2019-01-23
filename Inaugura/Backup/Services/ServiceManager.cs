using System;
using System.Xml;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Design;
using System.Collections;
using System.Threading;

using Inaugura.Xml;


namespace Inaugura.Telephony.Services
{
	/// <summary>
	/// 
	/// </summary>
	/// 

	public delegate void ServiceManagerHandler();

	//[Editor(typeof(ServiceManagerEditor),typeof(UITypeEditor))]
	public class ServiceManager : ServiceCollection, IStatusable, Inaugura.Xml.IXmlable
	{
		#region Events

		public event ServiceHandler ServiceCreated;
		public event ServiceHandler ServiceDestroyed;

		public event StatusHandler StatusChanged;
		/// <summary>
		/// Calls the StatusChanged event
		/// </summary>
		private void OnStatusChangedEvent()
		{
			if (this.StatusChanged != null)
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.StatusChanged, false, new object[] { this});
		}


		/// <summary>
		/// Calls the service created event
		/// </summary>
		/// <param name="service">The service that was created</param>
		private void OnServiceCreated(Service service)
		{
			if (this.ServiceCreated != null)
			{
				ServiceEventArgs args = new ServiceEventArgs(service);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.ServiceCreated, false, new object[] { this, args });
			}
		}
			
		/// <summary>
		/// Calls the service destroyed event
		/// </summary>
		/// <param name="service">The service that was destroyed</param>
		private void OnServiceDestroyed(Service service)
		{
			if (this.ServiceDestroyed != null)
			{
				ServiceEventArgs args = new ServiceEventArgs(service);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.ServiceDestroyed, false, new object[] { this, args });
			}
		}

		#endregion

		#region Variables
		public const float IdleGraceFactor = 0.1F;
		private bool mStarted = false;
		private string mStatus = string.Empty;
		private bool mContinueProcessing = true;
		private Thread mLineManagementThread;
		#endregion

		#region Propreties
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
		/// Flag which determins if the worker threads should continue processing
		/// </summary>
		/// <value></value>
		private bool ContinueProcessing
		{
			get
			{
				return this.mContinueProcessing;
			}
			set
			{
				this.mContinueProcessing = value;
			}
		}

		#region Xml Get/Set
		[Browsable(false)]
		public override XmlNode Xml
		{
			get
			{
				XmlDocument xmlDoc = new XmlDocument();
				XmlElement pe = xmlDoc.CreateElement("ServiceManager");
				XmlAttribute a = xmlDoc.CreateAttribute("type");

				a.Value = this.GetType().FullName;
				pe.Attributes.Append(a);

				// if the type is located in another assembly also include the assembly file name
				if (this.GetType().Assembly != System.Reflection.Assembly.GetExecutingAssembly())
				{
					a = xmlDoc.CreateAttribute("assembly");
					a.Value = this.GetType().Assembly.GetName().FullName;
					pe.Attributes.Append(a);
				}



				XmlElement e = xmlDoc.CreateElement("Services");

				foreach (Service s in this)
				{
					e.AppendChild(xmlDoc.ImportNode(s.Xml, true));
				}
				pe.AppendChild(e);
				xmlDoc.AppendChild(pe);

				return pe;
			}
			set
			{
				XmlNode node = value;

				if (node != null)
				{
					XmlNodeList nl = node.SelectNodes("Services/Service");

					foreach (XmlNode n in nl)
					{
						this.Add(Service.GetPsuedoServiceFromXml(n));
					}
				}
			}
		}
			#endregion

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

		#region Events
		public event ServiceManagerHandler ServiceManagerStarting;
		public event ServiceManagerHandler ServiceManagerStarted;
		public event ServiceManagerHandler ServiceManagerStopping;
		public event ServiceManagerHandler ServiceManagerStopped;

		#region Event Callers
		/// <summary>
		/// Calls the starting event
		/// </summary>
		private void OnStartingEvent()
		{
			if(this.ServiceManagerStarting != null)
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.ServiceManagerStarting, false, new object[] {});
		}

		/// <summary>
		/// Calls the started event
		/// </summary>
		private void OnStartedEvent()
		{
			if(this.ServiceManagerStarted != null)
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.ServiceManagerStarted, false, new object[] {});
		}
        
		/// <summary>
		/// Calls the stopping event
		/// </summary>
		private void OnStoppingEvent()
		{
			if(this.ServiceManagerStopping != null)
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.ServiceManagerStopping, false, new object[] {});
		}

		/// <summary>
		/// Calls the stopped event
		/// </summary>
		private void OnStoppedEvent()
		{
			if (this.ServiceManagerStopped != null)
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.ServiceManagerStopped, false, new object[] { });
		}
		#endregion
		#endregion 		

		#region Methods
		/// <summary>
		/// Constructor
		/// </summary>
		public ServiceManager()
		{			
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="node">The xml node which defines this ServiceManager</param>
		public ServiceManager(XmlNode node) : this()
		{
			if (node == null)
				throw new ArgumentNullException("node", "node can not be null");
			this.Xml = node;
		}

		#region Generic Methods
		public override string ToString()
		{
			if (this.Count == 1)
			{
				return "Service Manager (1 service)";
			}
			else
			{
				return "Service Manager (" + this.Count.ToString() + " services)";
			}
		}		
		#endregion

		#region Starting and Stopping
		public virtual void Start()
		{
			this.OnStartingEvent();

			ArrayList list = new ArrayList();

			Log.AddLog("Starting Service Manager");
			Log.Right();

			ArrayList pseudoServiceList = new ArrayList();
			foreach (Service s in this)
			{
				pseudoServiceList.Add(s);
			}

			// clear out the pseudo services
			this.Clear();

			// now create the actual service objects
			foreach (Service s in pseudoServiceList)
			{
				Log.AddLog("Loading Service (" + s.Name + ")");

				// todo make sure we have all the needed files
				try
				{
					this.CheckFileReferences(s);
				}
				catch (Exception ex)
				{
					Log.AddError(ex.Message);
					Log.AddLog("Loading Service (" + s.Name + ") faild");
					continue;
				}

				// create a real service object from the pseudo service object
				IXmlable xmlable = (IXmlable)s;
				Service ns = Service.FromXml(xmlable.Xml);
				Log.AddLog("Service Loaded (" + ns.Name + ")");
				this.Add(ns);
				Log.AddLog("After Add");
				// call service created
				this.OnServiceCreated(ns);
				Log.AddLog("After OnServiceCreated");
			}

			// Start the services
			foreach (Service s in this)
			{
				Log.AddLog("Starting Service " + s.Name);
				Thread t = new Thread(new ThreadStart(s.Start));
				list.Add(t);
				t.Start();
				Log.AddLog("Service " + s.Name + " Started");
			}

			foreach (Thread t in list)
			{
				t.Join();
			}
			/*
			bool done = false;
			while (!done)
			{
				done = true;
				foreach (Thread t in list)
				{
					if (t.ThreadState == ThreadState.Running || t.ThreadState == ThreadState.Unstarted)
						done = false;
				}

				System.Threading.Thread.Sleep(100);
			}
			*/

			// start the line managment thread
			this.ContinueProcessing = true;
			this.mLineManagementThread = new Thread(this.ProcessLineManagment);
			this.mLineManagementThread.Start();
			
			// wait for the line managmenet process to fully start
			while (this.mLineManagementThread.ThreadState != ThreadState.Running)
				Thread.Sleep(10);

			Log.Left();
			Log.AddLog("Service Manager Started");
			this.OnStartedEvent();
			this.Started = true;			
		}

		public virtual void Stop()
		{
			RunStop();
		}

		private void RunStop()
		{
			this.OnStoppingEvent();

			ArrayList list = new ArrayList();
			ArrayList pseudoServiceList = new ArrayList();

			Log.AddLog("Stopping Service Manager");
			Log.Right();


			// Stop the line managment thread
			this.ContinueProcessing = false;
			this.mLineManagementThread.Join();
			//while (this.mLineManagementThread.ThreadState != ThreadState.Stopped)
			//	Thread.Sleep(100);
			
			Log.AddLog("Stopping Services");
			Log.Right();

			foreach (Service s in this)
			{
				Log.AddLog("Stopping Service " + s.Name);
				s.Stop();
			}		

			bool done = false;
			while (!done)
			{
				done = true;
				foreach (Service s in this)
				{
					if (s.Started)
						done = false;
				}
				System.Threading.Thread.Sleep(100);
			}
			
			Log.Left();
			Log.AddLog("Services Stopped");


			// convert the services back to pseudo service objects
			foreach (Service s in this)
			{
				Log.AddLog("Unloading Service (" + s.Name + ")");
				IXmlable xmlable = (IXmlable)s;
				pseudoServiceList.Add(Service.GetPsuedoServiceFromXml(xmlable.Xml));
				Log.AddLog("Unloaded Service (" + s.Name + ")");
				this.OnServiceDestroyed(s);
			}
			this.Clear();

			foreach (Service s in pseudoServiceList)
			{
				this.Add(s);
			}

			Log.Left();
			Log.AddLog("Service Manager Stopped");

			this.Started = false;
			this.OnStoppedEvent();
		}

		#endregion

		#region Line Management
		private void ProcessLineManagment()
		{
			Log.AddLog("Line Management Thread: Started");
			this.IssueReservedLines();
			this.IssueMinimumRequirements();
			// sleep for 10 seconds until so that the initial lines can start taking calls
			for (int i = 0; i < 10 && this.ContinueProcessing; i++)
			{
				Thread.Sleep(1000);
			}
			while (this.ContinueProcessing)
			{

				this.CheckServices();
				// wait 3 seconds before checking lines again
				for (int i = 0; i < 3 && this.ContinueProcessing; i++)
				{
					Thread.Sleep(1000);
				}
			}

			Log.AddLog("Line Management Thread: Terminated");
		}

		private void IssueReservedLines()
		{
			Log.AddLog("Line Management Thread: Issuing Reserved Lines");
			foreach (Service service in this)
			{
/*
				foreach (string lineName in service.ReservedLineNames)
				{
					TelephonyLine line = Switch.ActiveSwitch.TelephonyManager.GetLine(lineName);
					if (line == null)
						Log.AddError("Line Management Thread: Service " + service.Name + " reserved line " + lineName + ". However the line could not be retreived from the telephony manager");
					else
					{
						service.SupplyLine(line);
					}
				}
				*/
			}
			Log.AddLog("Line Management Thread: Issuing Reserved Lines Complete");
		}

		private void IssueMinimumRequirements()
		{
			Log.AddLog("Line Management Thread: Issuing Minimum Requirements");
			foreach (Service service in this)
			{
				Log.AddLog("Requesting line from TelephonyManager");
				int numberOfLinesNeeded = service.MinimumNumberOfLines - service.Count;
				for (int i = 0; i < numberOfLinesNeeded; i++)
				{
					TelephonyLine line = Switch.ActiveSwitch.TelephonyManager.GetLine();
					if (line != null)
					{
						service.SupplyLine(line);
					}
				}
			}
			Log.AddLog("Line Management Thread: Issuing Minimum Requirements Complete");
		}

		private void CheckServices()
		{
			ServiceCollection servicesNeedingLines = new ServiceCollection();
			ServiceCollection servicesGivingLines = new ServiceCollection();			

			foreach (Service service in this)
			{
				float idleGrace = service.IdealIdleSeconds * ServiceManager.IdleGraceFactor;
				//Log.AddLog("Service (" + service.Name + ") idle time = " + service.IdleTime.TotalSeconds.ToString());
				// does the service meet its minimum number of lines requirement
				if (service.Count < service.MinimumNumberOfLines)
				{
					servicesNeedingLines.Add(service);					
				}
				else if ((service.IdleTime.TotalSeconds - service.IdealIdleSeconds < -idleGrace) && (service.Count < service.MaximumNumberOfLines))
				{
					servicesNeedingLines.Add(service);
				}
				else if ((service.IdleTime.TotalSeconds - service.IdealIdleSeconds > idleGrace) && (service.Count > service.MinimumNumberOfLines))
				{
					servicesGivingLines.Add(service);
				}

				if(servicesNeedingLines.Count > 0 )
					this.RemoveLinesFromServices(servicesGivingLines);

				this.SupplyLinesToServices(servicesNeedingLines);
				
			}
		}


		private void SupplyLinesToServices(ServiceCollection services)
		{
			int numberOfLinesAvaliable = Switch.ActiveSwitch.TelephonyManager.AvailableLines.Count;			
			// Are there any lines avaliable, if no lines are availiable return
			if (numberOfLinesAvaliable == 0)
				return;

			// determine the priority sum
			float prioritySum = 0.0f;
			foreach (Service service in services)
			{
				prioritySum += ((float)service.IdleTime.TotalSeconds - service.IdealIdleSeconds)*service.Importance;
			}

			// supply lines to the services based on priority
			float priorityPerLine = prioritySum / numberOfLinesAvaliable;
			foreach (Service service in services)
			{

				float priority = ((float)service.IdleTime.TotalSeconds - service.IdealIdleSeconds) * service.Importance;
				int numberOfLinesNeeded = (int)Math.Floor(priority * priorityPerLine);

				for (int i = 0; i < numberOfLinesNeeded; i++)
				{
					TelephonyLine line = Switch.ActiveSwitch.TelephonyManager.GetLine();
					if (line != null)
						service.SupplyLine(line);
					else
						return;
				}
			}
		}

		private void RemoveLinesFromServices(ServiceCollection services)
		{
			foreach (Service service in services)
			{
				service.FreeLines(1);
			}
		}
		#endregion

		private void CheckFileReferences(Service s)
		{
			/*
			foreach (FileReference fr in s.FileReferences)
			{
				if (!System.IO.File.Exists(fr.Name))
				{
					Log.AddLog("Attempting to download (" + fr.Name+")");
					byte[] data = OrbisTel.Data.DataAdaptor.CurrentDataAdaptor.MediaStore.GetFile(fr.Name, fr.Folder, fr.Version);
					if (data == null)
					{
						Log.AddLog("File ("+fr.Name+") does not exist in the data store");
						throw new ApplicationException("Data Adaptor did not contain the file reference (" + fr.Name + ", " + fr.Folder + ", " + fr.Version + ") used by service (" + s.Name + ")");
					}
					else
					{
						OrbisSoftware.Helper.SaveFileContent(fr.Name, data);
						Log.AddLog("Downloaded " + fr.Name);
					}
				}
			}
			*/

		}

		#endregion

	
	}
}
