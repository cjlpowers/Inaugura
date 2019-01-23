using System;
using System.Threading;

using Inaugura.Telephony;

namespace Inaugura.Telephony.Services
{

	#region ServiceLineEventArgs
	/// <summary>
	/// The service line event args
	/// </summary>
	public class ServiceLineEventArgs : EventArgs
	{
		private ServiceLine mServiceLine; // The service line

		/// <summary>
		/// The Service Line
		/// </summary>
		/// <value></value>
		public ServiceLine ServiceLine
		{
			get { return mServiceLine; }
		}

		/// <summary>
		/// Constructor 
		/// </summary>
		/// <param name="line">The service line</param>
		public ServiceLineEventArgs(ServiceLine line)
		{
			if (line == null)
				throw new ArgumentNullException("line", "The line argument can not be null");
			this.mServiceLine = line;
		}
	}
	#endregion

	public delegate void ServiceLineHandler(object sender, ServiceLineEventArgs e);

	/// <summary>
	/// Summary description for ServiceLine.
	/// </summary>
	public abstract class ServiceLine: IStatusable
	{
		#region Events	
	
		public event ServiceLineHandler ServiceLineStarted;
		public event ServiceLineHandler ServiceLineStopped;
		public event ServiceLineHandler ServiceLineActive;
		public event ServiceLineHandler ServiceLineInactive;
		public event ServiceLineHandler ServiceLineTerminated;

		public event StatusHandler StatusChanged;


		#region Event Callers
		/// <summary>
		/// Calls the ServiceLineStartedevent
		/// </summary>
		private void OnServiceLineStartedEvent()
		{
			if (this.ServiceLineStarted != null)
			{
				ServiceLineEventArgs args = new ServiceLineEventArgs(this);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.ServiceLineStarted, false, new object[] { this, args });
			}		
		}

		/// <summary>
		/// Calls the ServiceLineStopped event
		/// </summary>
		private void OnServiceLineStoppedEvent()
		{
			if (this.ServiceLineStopped != null)
			{
				ServiceLineEventArgs args = new ServiceLineEventArgs(this);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.ServiceLineStopped, false, new object[] { this, args });
			}
		}

		/// <summary>
		/// Calls the ServiceLineActive event
		/// </summary>
		private void OnServiceLineActiveEvent()
		{			
			this.mActiveTime = DateTime.Now;
			if (this.ServiceLineActive != null)
			{
				ServiceLineEventArgs args = new ServiceLineEventArgs(this);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.ServiceLineActive, false, new object[] { this, args });
			}
		}

		/// <summary>
		/// Calls the ServiceLineInactive event
		/// </summary>
		private void OnServiceLineInactiveEvent()
		{		
			this.mInactiveTime = DateTime.Now;
			if (this.ServiceLineInactive != null)
			{
				ServiceLineEventArgs args = new ServiceLineEventArgs(this);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.ServiceLineInactive, false, new object[] { this, args });
			}
		}

		/// <summary>
		/// Calls the StatusChanged event
		/// </summary>
		private void OnStatusChangedEvent()
		{
			if(this.StatusChanged != null)
			{
				StatusEventArgs args = new StatusEventArgs(this);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.StatusChanged, false, new object[] { this, args });
			}
		}

		private void OnServiceLineTerminatedEvent()
		{
			if (this.ServiceLineTerminated != null)
			{
				ServiceLineEventArgs args = new ServiceLineEventArgs(this);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.ServiceLineTerminated, false, new object[] { this, args });
			}
		}
		#endregion

		#endregion

		#region Variables
		private TimeSpan mIdleTime = TimeSpan.MinValue;
		private DateTime mActiveTime = DateTime.MinValue;
		private DateTime mInactiveTime = DateTime.MinValue;
		private int mCallsProcessed = 0;		
		private bool mStarted = false;
		private bool mActive = false;
		private Thread mThread;
		private TelephonyLine mLine;
		private Service mService;
		private string mStatus;
		#endregion		

		#region Properties
		#region IServiceLine Members					
		/// <summary>
		/// The number of calls processed
		/// </summary>
		/// <value></value>
		public int CallsProcessed
		{
			get
			{	
				return this.mCallsProcessed;
			}
			set // internal
			{
				this.mCallsProcessed = value;
			}
		}

		/// <summary>
		/// The lines active state
		/// </summary>
		/// <value></value>
		public bool Active
		{
			get
			{
				return this.mActive;
			}
			protected set
			{
				if (value == true && this.mActive == false)
				{
					this.mActiveTime = DateTime.Now;
					this.mActive = true;
					this.OnServiceLineActiveEvent();				
				}
				else if (value == false && this.mActive == true)
				{
					this.mInactiveTime = DateTime.Now;
					this.mActive = false;
					this.OnServiceLineInactiveEvent();
				}
			}
		}

		/// <summary>
		/// The lines processing state
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
		/// The lines started state
		/// </summary>
		/// <value></value>
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
		/// The telephony line used by this service line instance
		/// </summary>
		/// <value></value>
		protected TelephonyLine Line
		{
			get
			{
				return this.mLine;
			}
			private set
			{
				this.mLine = value;
			}
		}
		/// <summary>
		/// The service to which this service line belongs
		/// </summary>
		/// <value></value>
		public Service Service
		{
			get
			{			
				return this.mService;
			}
			private set
			{
				this.mService = value;
			}
		}

		public TimeSpan IdleTime
		{
			get
			{
				if (this.mInactiveTime == DateTime.MinValue)
				{
					//Log.AddLog("1");
					return TimeSpan.MinValue;
				}
				else if (this.mActiveTime == DateTime.MinValue)
				{
					//Log.AddLog("2");
					return DateTime.Now - this.mInactiveTime;
				}
				// We are currently in idle time
				else if (this.mInactiveTime > this.mActiveTime)
				{
					if (DateTime.Now - this.mInactiveTime > this.mIdleTime)
					{
						//Log.AddLog("3");
						return DateTime.Now - this.mInactiveTime;
					}
					else
					{
						//Log.AddLog("4");
						return mIdleTime;
					}
				}
				// we are currently in active time
				else
				{
					//Log.AddLog("5");
					this.mIdleTime = this.mActiveTime - this.mInactiveTime;
					return this.mIdleTime;
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
			protected set
			{
				this.mStatus = value;
				this.OnStatusChangedEvent();				
			}
		}
		#endregion
		#endregion

		#region Methods
		public ServiceLine(Service service, TelephonyLine line)
		{				
			this.Service = service;
			this.Line = line;
			this.mThread = new Thread(new ThreadStart(this.ProcessLine));
		}

		public override string ToString()
		{
			return this.Line.Name;
		}


		/// <summary>
		/// Starts the service line
		/// </summary>
		public void Start()
		{
			if(this.Started)
				return;

			this.mThread.Start();
			this.Started = true;
			this.Status = "Started";
			this.OnServiceLineStartedEvent();
			this.mInactiveTime = DateTime.Now;
		}

		/// <summary>
		/// Stops the Service Line and waits until the thread is done processing
		/// </summary>
		protected virtual void RunStop()
		{
			this.Status = "Stopping";
			this.Started = false;

			this.mThread.Join();
			/*while(this.mThread.ThreadState != ThreadState.Stopped)
			{
				Thread.Sleep(100);
			}*/

			this.Status = "Stopped";

			this.OnServiceLineStoppedEvent();
		}

		/// <summary>
		/// Stops the service line
		/// </summary>
		public void Stop()
		{
			if(this.Started)
			{
				Thread t = new Thread(new ThreadStart(this.RunStop));
				t.Start();			
			}
		}


		public void Terminate()
		{
			Thread t = new Thread(new ThreadStart(this.RunTerminate));
			t.Start();
		}

		protected virtual void RunTerminate()
		{
			this.RunStop();
			this.OnServiceLineTerminatedEvent();
		}

		protected abstract void ProcessLine();
		#endregion		
	}
}
