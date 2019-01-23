using System;
using System.Threading;


namespace Inaugura.Telephony.Services
{
	/// <summary>
	/// Base class for all IncommingServices 
	/// </summary>
	public abstract class IncommingServiceLine : ServiceLine
	{
		#region Variables		
		private string mLogOnCommand = "";				// The command to dial to log on a UCD
		private string mLogOffCommand = "";				// The command to dial to log off a UCD
		private bool mLoggedOn = false;					// The logged on state
		#endregion

		#region Events
		public event ServiceLineHandler LoggedOnEvent;					// The loggegd on event
		public event ServiceLineHandler LoggedOffEvent;					// The logged off event
		
		#region Event Callers
		private void OnLoggedOnEvent()
		{
			if (this.LoggedOnEvent != null)
			{
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.LoggedOnEvent, false, new object[] { this });
			}
		}

		private void OnLoggedOffEvent()
		{
			if(this.LoggedOffEvent != null)
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.LoggedOffEvent, false, new object[] { this });
		}		
		#endregion

		#endregion
	
		#region Properties
		/// <summary>
		/// The logged on state
		/// </summary>
		/// <value></value>
		public bool LoggedOn
		{
			get
			{
				return this.mLoggedOn;
			}	
			set // private
			{
				this.mLoggedOn = value;
			}
		}
		
		/// <summary>
		/// The log on command
		/// </summary>
		/// <value></value>
		public String LogOnCommand
		{
			get
			{
				return this.mLogOnCommand;
			}
			set
			{
				this.mLogOnCommand = value;
			}
		}		

		/// <summary>
		/// The log off command
		/// </summary>
		/// <value></value>
		public String LogOffCommand
		{
			get
			{
				return this.mLogOffCommand;
			}
			set
			{
				this.mLogOffCommand = value;
			}			
		}	
		#endregion	

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="service">The Service</param>
		/// <param name="line">The Telephony Line</param>
		public IncommingServiceLine(Service service, TelephonyLine line) : base(service,line)
		{
			if (this.Service is IncommingService)
			{
				IncommingService s = (IncommingService)this.Service;
				this.LogOnCommand = s.LogOnCommand;
				this.LogOffCommand = s.LogOffCommand;
			}
		}			

		/// <summary>
		/// Handles incomming calls
		/// </summary>
		protected abstract void ProcessIncommingCall();
		
		/// <summary>
		/// The main processing function
		/// </summary>
		protected override void ProcessLine()
		{
			if(!this.LoggedOn)
			{
                Inaugura.Log.AddLog("Login on line");
				this.LogOn();
			}

			this.Status = "Waiting for Call";
			while(this.Started || this.LoggedOn)
			{
				try
				{

					this.Status = "Waiting for Call";
					if (this.Line.WaitRing(2, 8))
					{
						this.Active = true;
						try
						{
							this.Line.OffHook();							
							this.Status = "Incomming Call";
							this.ProcessIncommingCall();													
						}
						finally
						{
							this.Line.OnHook();
							this.Active = false;
						}						
					}
					if (!this.Started)
					{
						if (this.LogOff())
						{
							this.LoggedOn = false;
							continue; // get out of the loop
						}
					}
					Thread.Sleep(100);
				}
				catch (TelephonyException ex)
				{
					// this exception is expected so dont do anything
					this.Status = ex.Message;
				}
				catch (Exception ex)
				{
					Log.AddLog(ex.ToString());
				}
			}	
		}					

		/// <summary>
		/// Logs the line in using the LogOnCommand
		/// </summary>
		/// <returns>True if logged on, False otherwise</returns>
		private bool LogOn()
		{		
			try
			{
				if(this.LogOnCommand.Length != 0)
				{
                    Log.AddLog(this.Line.Name + " Loging onto UCD (" + this.LogOnCommand + ")");
					this.Status = "Logging onto UCD";
					this.Line.OffHook();
					this.Line.Dial(this.LogOnCommand);
					this.Line.OnHook();												
				}

				this.mLoggedOn = true;
				this.OnLoggedOnEvent();
				this.Status = "Waiting for Call";
				return true;

			}
			catch(Exception ex)
			{
				Log.AddLog(ex.Message);
			}

			return false;
		}

		/// <summary>
		/// Logs the ling out using the log out command
		/// </summary>
		/// <returns>True if logged out, False otherwise</returns>
		private bool LogOff()
		{
			if (this.LogOffCommand.Length != 0)
			{
				this.Status = "Logging off UCD";
				// get rid of a call			

				// wait a half second just to ensure any previous calls are disconnected
				Thread.Sleep(500);

				this.Line.OffHook();

				DialAnalysis anal = this.Line.Dial("L" + this.LogOffCommand, 2);
				if (anal != DialAnalysis.NoDialTone && anal != DialAnalysis.CallConnected)
				{
					Log.AddLog(this.Line.Name + " Logged Off " + anal.ToString());
					this.Line.OnHook();
					this.Status = "Logged off UCD";
					this.LoggedOn = false;
					this.OnLoggedOffEvent();
					return true;
				}
				else
				{
					Log.AddLog(this.Line.Name + " Not Logged Off. Processing Call. Reason: " + anal.ToString());
					this.Active = true;
					this.Status = "No Dial Tone Assuming Incomming Call";
					//this.mLoggedOn = false;
					this.ProcessIncommingCall();
					this.Active = false;
					this.Status = "Waiting for Call";
					this.Line.OnHook();
					return false;
				}
			}
			else // there is no log of command
			{
				this.LoggedOn = false;
				this.OnLoggedOffEvent();
				return true;
			}
		}
	}
}
