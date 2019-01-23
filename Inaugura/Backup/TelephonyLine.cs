using System;

using Inaugura;

namespace Inaugura.Telephony
{

	#region Enumeration
	public enum DialAnalysis
	{
		Busy = 0,
		OperatorIntercept = 1,
		CallConnected = 2,
		Error = 3,
		FaxTone = 4,
		NoAnswer = 5,
		NoDialTone = 6,
		NoRingBack = 7,
	}

	public enum TelephonyResult
	{
		Terminated = 0,
		Completed = 1,
	}

	public enum HookState
	{
		OnHook = 0,
		OffHook = 1
	}
	#endregion

	#region TelephonyLineEventArgs
	/// <summary>
	/// The telephony line event args
	/// </summary>
	public class TelephonyLineEventArgs : EventArgs
	{
		private TelephonyLine mLine; // The 

		/// <summary>
		/// The Telephony Line
		/// </summary>
		/// <value></value>
		public TelephonyLine Line
		{
			get { return mLine; }
		}

		/// <summary>
		/// Constructor 
		/// </summary>
		/// <param name="line">The telephony line</param>
		public TelephonyLineEventArgs(TelephonyLine line)
		{
			if (line == null)
				throw new ArgumentNullException("line", "The line argument can not be null");
			this.mLine = line;
		}
	}
	#endregion

	public delegate void TelephonyLineHandler(object sender, TelephonyLineEventArgs e);

	/// <summary>
	/// 
	/// </summary>
	public abstract class TelephonyLine : IStatusable
	{
		#region Events
		public event StatusHandler StatusChanged;				// The status changed event

		#region Event Callers
		/// <summary>
		/// Raises the Status Changed Event
		/// </summary>
		protected void OnStatusChangedEvent()
		{
			if (this.StatusChanged != null)
			{
				StatusEventArgs args = new StatusEventArgs(this);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.StatusChanged, false, new object[] { this, args });
			}
		}
		#endregion

		#endregion

		#region Variables
		private string mName;
		private TelephonyHardware mHardware;
		private string mStatus = string.Empty;
		#endregion

		#region Properties
		public abstract CallerID CallerID
		{
			get;
		}	

		public abstract int NumberOfDigitsInBuffer
		{
			get;
		}

		public string Name
		{
			get
			{
				return this.mName;
			}
			private set
			{
				this.mName = value;
			}
		}	

		public abstract HookState HookState
		{
			get;
		}

		public TelephonyHardware Hardware
		{
			get
			{
				return this.mHardware;
			}
			private set
			{
				this.mHardware = value;
			}
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
		#endregion

		#region Methods
        public TelephonyLine(TelephonyHardware hardware, string lineName)
		{
			this.Hardware = hardware;
			this.Name = lineName;			
		}

		public override string ToString()
		{
			return this.Name;
		}
        
        /// <summary>
        /// Clears all digits from the digit buffer
        /// </summary>
		public abstract void ClearDigitBuffer();
		public abstract void Dial(string  phoneNumber);
		public abstract DialAnalysis Dial(string  phoneNumber, int numberOfRings);

		public string GetDigits(int maxDigits, TimeSpan timeOut, TimeSpan digitTimeOut)
		{
			return this.GetDigits(maxDigits, timeOut, digitTimeOut,string.Empty);
		}

		public string GetDigits(int maxDigits, TimeSpan timeOut, TimeSpan digitTimeOut, string termDigits)
		{
			string refTermDigits = string.Empty; // dont care about this
			return this.GetDigits(maxDigits, timeOut, digitTimeOut, termDigits, out refTermDigits);
		}

		public abstract string GetDigits(int maxDigits, TimeSpan timeOut, TimeSpan digitTimeOut, string termDigits, out string termDigit);
				public abstract void GiveDialTone();
		public abstract void OffHook();
		public abstract void OnHook();

		public abstract void Record(string fileName, TimeSpan timeOut, TimeSpan silenceTimeOut, bool terminate);

		public void Record(string fileName, TimeSpan timeOut, TimeSpan silenceTimeOut)
		{
			this.Record(fileName, timeOut, silenceTimeOut, false);
		}
		
		public void Record(string fileName, TimeSpan timeOut)
		{
			this.Record(fileName, timeOut, TimeSpan.FromSeconds(0));
		}

		public abstract void OpenLine();
		public abstract void CloseLine();

		public abstract void PlaySpecial(string fileName);

		public abstract void PlayFile(string fileName);
		public abstract TelephonyResult PlayFile(string fileName, bool terminate);
		public abstract TelephonyResult PlayFile(string fileName, string termDigits, out string termDigit);

		public abstract bool DetectRingback(TimeSpan timeOut);

		/*
		public abstract void PlayText(string voice, string text);
		public abstract TelephonyResult PlayText(string voice, string text, bool terminate);
		public abstract TelephonyResult PlayText(string voice, string text, string termDigits, ref string termDigit);
		public abstract string Reco();
		public abstract string Reco(string grammerXml);
		*/

		public abstract bool WaitRing(int numberOfRings, int waitTimer);
		#endregion
	}
}
