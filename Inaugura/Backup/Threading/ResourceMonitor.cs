#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

#endregion

namespace Inaugura.Threading
{
	/// <summary>
	/// Controls access to one or more resources
	/// </summary>
	public class ResourceMonitor
	{
		private System.Collections.ArrayList mList;

		public ResourceMonitor()
		{
			this.mList = new System.Collections.ArrayList();
		}

		/// <summary>
		/// Locks a resource. Any other threads requesting exclusive access to the resource will have to wait until the resource is unlocked
		/// </summary>
		/// <param name="resource">The resource to lock</param>
		public void Lock(object resource)
		{
			if (this.mList.Contains(resource))
				return;
			else
			{
				Monitor.Enter(resource);
				lock (this)
				{
					this.mList.Add(resource);
				}
			}
		}

		/// <summary>
		/// Unlocks a previously locked resource
		/// </summary>
		/// <param name="resource">The resource to unlock</param>
		public void Unlock(object resource)
		{
			if (!this.mList.Contains(resource))
				throw new ArgumentException("The resource does not exist in the list");
			else
			{
				Monitor.Exit(resource);
				lock (this)
				{
					this.mList.Remove(resource);
				}
			}
		}

		/// <summary>
		/// Waits for a specific resource to be unlocked
		/// </summary>
		/// <param name="resource">The resource to wait for</param>
		public void Wait(object resource)
		{
			if (this.mList.Contains(resource))
			{
				lock (resource)	{}
			}
		}

		/// <summary>
		/// Determins if a resource is currently locked
		/// </summary>
		/// <param name="resource">The resource</param>
		/// <returns>True if locked, false otherwise</returns>
		public bool IsLocked(object resource)
		{
			return this.mList.Contains(resource);
		}
	}
}
