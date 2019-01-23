#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

#endregion

namespace OrbisTel.Services.RealLeads
{
	/// <summary>
	/// Controls access to multiple resources
	/// </summary>
	internal class ResourceLoader
	{
		#region Internal Classes
		private class Resource
		{
			public int locked;
			public Exception mException;
			public Exception Exception 
			{ 
				get { return this.mException; }
				set { this.mException = value; }
			}

			public Resource()
			{
				this.Exception = null;
			}
		}
		#endregion

		private System.Collections.Generic.Dictionary<string, Resource> mResourceList;

		/// <summary>		
		/// Constructor
		/// </summary>
		public ResourceLoader()
		{
			this.mResourceList = new System.Collections.Generic.Dictionary<string, Resource>();
		}

		/// <summary>
		/// Locks a resource
		/// </summary>
		/// <param name="resourceID">The resource ID</param>
		public void LockResource(string resourceID)
		{
			lock (this.mResourceList)
			{
				if (this.mResourceList.ContainsKey(resourceID))
					return;

				// else add the resource item				
				Resource res = new Resource();
				this.mResourceList.Add(resourceID, res);

				// now lock it
				Interlocked.Increment(ref res.locked);
			}
		}

		/// <summary>
		/// Unlocks a locked resource
		/// </summary>
		/// <param name="resourceID">The resource ID</param>
		public void UnlockResource(string resourceID)
		{
			lock (this.mResourceList)
			{
				if(!this.mResourceList.ContainsKey(resourceID))
					return;

				Resource res = this.mResourceList[resourceID];

				Interlocked.Decrement(ref res.locked);

				if (res.locked == 0)
					this.mResourceList.Remove(resourceID);
			}			
		}

		/// <summary>
		/// Waits on a potentially locked resource
		/// </summary>
		/// <param name="resourceID">The resource ID</param>
		public void Wait(string resourceID)
		{
			Resource res = this.GetResource(resourceID);
			if (res != null)
			{
				while (res.locked != 0)
					Thread.Sleep(50);

				if (res.Exception != null)
				{
					throw new Exception("Exception occured in ResourceLoader", res.Exception);
				}
			}
		}

		private Resource GetResource(string resourceID)
		{
			lock (this.mResourceList)
			{
				if(this.mResourceList.ContainsKey(resourceID))
					return this.mResourceList[resourceID];
				else
					return null;
			}
		}	

		public void SetException(string resourceID, Exception ex)
		{
			Resource res = this.GetResource(resourceID);
			res.Exception = ex;
		}
	}
}
