using System;

namespace Inaugura.Telephony
{
	#region StatusEventArgs
	/// <summary>
	/// The status event args
	/// </summary>
	public class StatusEventArgs : EventArgs
	{
		private IStatusable mStatusSource; // The 

		public IStatusable StatusSoruce
		{
			get { return mStatusSource; }
		}

		/// <summary>
		/// Constructor 
		/// </summary>
		/// <param name="statusSource">The object wich is the source of the status event</param>
		public StatusEventArgs(IStatusable statusSource)
		{
			if(statusSource == null)
				throw new ArgumentNullException("statusSource","The status source can not be null");
			this.mStatusSource = statusSource;
		}
	}
	#endregion

	public delegate void StatusHandler(object sender, StatusEventArgs e);

	/// <summary>
	/// Interface to support monitoring status 
	/// </summary>
	public interface IStatusable
	{		
		event StatusHandler StatusChanged;		
	
		/// <summary>
		/// The current status
		/// </summary>
		/// <value></value>
		string Status
		{
			get;
		}
	}
}
