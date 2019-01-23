using System;
using System.Collections;

namespace Inaugura.Telephony
{
	/// <summary>
	/// Holds a collection of IServiceLine objects
	/// </summary>
	public class TelephonyLineCollection : CollectionBase
	{
		#region Events
		public event TelephonyLineHandler LineAdded;			// The line added event
		public event TelephonyLineHandler LineRemoved;			// The line removed event

		#region Event Callers
		/// <summary>
		/// Calls the LineAdded event
		/// </summary>
		/// <param name="line">The line that was added</param>
		private void OnLineAddedEvent(TelephonyLine line)
		{
			if (this.LineAdded != null)
			{
				TelephonyLineEventArgs args = new TelephonyLineEventArgs(line);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.LineAdded, false, new object[] { this, args });
			}
		}

		/// <summary>
		/// Calls the LineRemoved event
		/// </summary>
		/// <param name="line">The line that was removed</param>
		private void OnLineRemoveEvent(TelephonyLine line)
		{
			if (this.LineRemoved != null)
			{
				TelephonyLineEventArgs args = new TelephonyLineEventArgs(line);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.LineRemoved, false, new object[] { this, args });
			}
		}
		#endregion

		#endregion
		
		#region Accessors
		/// <summary>
		/// Accessor
		/// </summary>
		/// <value></value>
		public TelephonyLine this [int index]
		{
			get
			{
				return (TelephonyLine)this.List[index];				
			}
		}

		/// <summary>
		/// Accessor
		/// </summary>
		/// <value></value>
		public TelephonyLine this[string lineName]
		{
			get
			{
				foreach (TelephonyLine line in this)
				{
					if (line.Name == lineName)
					{
						return line;
					}
				}
				return null;
			}
		}
		#endregion		
		
		#region Methods

		/// <summary>
		/// Constructor
		/// </summary>
		public TelephonyLineCollection()
		{			
		}

		/// <summary>
		/// Adds a TelephonyLine to the list
		/// </summary>
		/// <param name="line">The line to add</param>
		public void Add(TelephonyLine line)
		{
			lock (this)
			{
				if (!this.Contains(line))
				{
					this.List.Add(line);
					this.OnLineAddedEvent(line);
				}
			}
		}

		/// <summary>
		/// Determins if the list contains a TelephonyLine
		/// </summary>
		/// <param name="line">The ITelephonyLine</param>
		/// <returns>True if the line is contained in the list, False otherwise</returns>
		public bool Contains(TelephonyLine line)
		{
			lock (this)
			{
				return this.List.Contains(line);
			}
		}

		/// <summary>
		/// Determins if the list contains a ITelephonyLine
		/// </summary>
		/// <param name="line">The ITelephonyLine</param>
		/// <returns>True if the line is contained in the list, False otherwise</returns>
		public bool Contains(string lineName)
		{
			lock (this)
			{
				foreach (TelephonyLine line in this)
				{
					if (line.Name == lineName)
						return true;
				}
				return false;
			}

		}

		/// <summary>
		/// Removes a ITelephonyLine from the list
		/// </summary>
		/// <param name="line">The line to remove</param>
		public void Remove(TelephonyLine line)
		{
			lock (this)
			{
				if (this.Contains(line))
				{
					this.List.Remove(line);
					this.OnLineRemoveEvent(line);
				}
			}
		}
		#endregion
		
	}
}
