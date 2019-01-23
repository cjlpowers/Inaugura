using System;
using System.Threading;
using System.Xml;

namespace Inaugura.Telephony.Emulation
{
	/// <summary>
	/// Summary description for EmuHardware.
	/// </summary>
	public class WindowsHardware : Telephony.TelephonyHardware
	{
		private delegate void FunctionDelegate();
		private delegate System.Windows.Forms.DialogResult DialogDelegate();

		private WindowsLineDlg mDlg;
		protected Thread mDlgThread;
		protected TelephonyLine[] lines;

		#region Properties
		public override string Name
		{
			get
			{
				return "Windows Hardware";
			}
		}		
		#endregion

		public WindowsHardware()
		{
			mDlg = new WindowsLineDlg();
			lines = new TelephonyLine[1];
			lines[0] = new WindowsLine(this,mDlg);
			
			//this.FileReferences.Add(new FileReference("OrbisTel.Telephony.Emulator.dll","TelephonyHardware",System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));
		}

		public WindowsHardware(XmlNode node) : base(node)
		{
			mDlg = new WindowsLineDlg();
			lines = new TelephonyLine[1];
			lines[0] = new WindowsLine(this, mDlg);
		}

		public override TelephonyLine[] Start()
		{
			this.OnHardwareStartingEvent();
			mDlg.Show();			
			this.Status = "Started";
			this.OnHardwareStartedEvent();
			this.OnLineCreatedEvent(lines[0]);
			return lines;
		}		

		public override void Stop()
		{
			this.OnHardwareStoppingEvent();
			this.mDlg.BeginInvoke(new FunctionDelegate(mDlg.Close), null);
			this.Status = "Stopped";
			this.OnHardwareStoppedEvent();
		}

	
		public override string ToString()
		{
			return "Windows Emulator Hardware";
		}

	}
}
