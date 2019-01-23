using System;
using System.Collections;

namespace Inaugura.Telephony.Services
{
	/// <summary>
	/// Holds a collection of IServiceLine objects
	/// </summary>
	public class ServiceLineCollection : CollectionBase
	{
		#region Events
		public event ServiceLineHandler ServiceLineAdded;			// The line added event
		public event ServiceLineHandler ServiceLineRemoved;			// The line removed event

		#region Event Callers
		/// <summary>
		/// Calls the ServiceLineAdded event
		/// </summary>
		/// <param name="line">The line that was added</param>
		private void OnServiceLineAddedEvent(ServiceLine line)
		{
			if (this.ServiceLineAdded != null)
			{
				ServiceLineEventArgs args = new ServiceLineEventArgs(line);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.ServiceLineAdded, false, new object[] { this, args });
			}
		}

		/// <summary>
		/// Calls the ServiceLineRemoved event
		/// </summary>
		/// <param name="line">The line that was removed</param>
		private void OnServiceLineRemovedEvent(ServiceLine line)
		{
			if (this.ServiceLineRemoved != null)
			{
				ServiceLineEventArgs args = new ServiceLineEventArgs(line);
				Inaugura.Threading.Helper.ThreadSafeInvoke(this.ServiceLineRemoved, false, new object[] { this, args });
			}
		}
		#endregion

		#endregion
		
		#region Accessors
		/// <summary>
		/// Accessor
		/// </summary>
		/// <value></value>
		public ServiceLine this [int index]
		{
			get
			{
				return (ServiceLine)this.List[index];				
			}
		}
		#endregion		
		
		#region Methods

		/// <summary>
		/// Constructor
		/// </summary>
		public ServiceLineCollection()
		{			
		}

		/// <summary>
		/// Adds a ServiceLine to the list
		/// </summary>
		/// <param name="line">The line to add</param>
		protected virtual void Add(ServiceLine line)
		{
			lock (this)
			{
				if (!this.Contains(line))
				{
					this.List.Add(line);
					this.OnServiceLineAddedEvent(line);
				}
			}
		}

		/// <summary>
		/// Determins if the list contains a ServiceLine
		/// </summary>
		/// <param name="line">The Service Line</param>
		/// <returns>True if the line is contained in the list, False otherwise</returns>
		protected bool Contains(ServiceLine line)
		{
			lock (this)
			{
				return this.List.Contains(line);
			}
		}

		/// <summary>
		/// Removes a ServiceLine from the list
		/// </summary>
		/// <param name="line">The line to remove</param>
		protected virtual void Remove(ServiceLine line)
		{
			lock (this)
			{
				if (this.Contains(line))
				{
					this.List.Remove(line);
					this.OnServiceLineRemovedEvent(line);
				}
			}
		}
		#endregion
		
	}
}
