using System;

namespace Inaugura.Telephony
{
	/// <summary>
	/// Summary description for DialCommand.
	/// </summary>
	public class DialCommandThread 
	{
		#region Variables
		private System.Threading.Thread mThread;
		private string mDialCommand = "";
		private Inaugura.Telephony.TelephonyLine mLine;
		#endregion

		public System.Threading.Thread Thread
		{
			get
			{
				return this.mThread;
			}
		}

		public DialCommandThread(ITelephonyLine line, string dialCommand)
		{
			this.mThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.ProcessDialCommand));
			this.mLine = line;
			this.mDialCommand = dialCommand;
		}

		public void Process()
		{
			this.mThread.Start();			
		}

		private void ProcessDialCommand()
		{
			this.mLine.OffHook();
			this.mLine.Dial(this.mDialCommand);
			this.mLine.OnHook();
		}
	}
}
