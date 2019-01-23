using System;
using System.Collections;

namespace Inaugura.Web.Controls
{
	/// <summary>
	/// Represents a collection of tabs
	/// </summary>
	public class TabCollection : CollectionBase
	{
		#region Events
		public event TabEventHandler TabAdded;
		public event TabEventHandler TabRemoved;
		#endregion

		/// <summary>
		/// Gets a meter component name by index
		/// </summary>
		public Tab this[int index]
		{
			get
			{
				return this.List[index] as Tab;
			}
		}

		/// <summary>
		/// Adds a tab to the list
		/// </summary>
		/// <param name="tab"></param>
		public void Add(Tab tab)
		{
			if(!this.List.Contains(tab))
			{
				this.List.Add(tab);
				this.OnAdd(tab);
			}
		}

		private void OnAdd(Tab tab)
		{
			if(this.TabAdded != null)
				this.TabAdded(this,new TabEventArgs(tab));
		}

		/// <summary>
		/// Removes a tab from the collection
		/// </summary>
		/// <param name="tab">The tab to remove</param>
		public void Remove(Tab tab)
		{
			if(this.List.Contains(tab))
			{
				this.OnRemove(tab);
				this.List.Remove(tab);			
			}
		}

		private void OnRemove(Tab tab)
		{
			if(this.TabRemoved != null)
				this.TabRemoved(this,new TabEventArgs(tab));
		}

		/// <summary>
		/// Determins wheather the tab collection contains a specific tab
		/// </summary>
		/// <param name="tab">The tab</param>
		/// <returns>True if the collection contains the tab, false otherwise</returns>
		public bool Contains(Tab tab)
		{
			return this.List.Contains(tab);
		}

		/// <summary>
		/// Removes all tabs from the collection
		/// </summary>
		new public  void Clear()
		{
			while(this.Count > 0)
			{
				Tab tab = this[0];
				this.Remove(tab);
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public TabCollection()
		{			
		}
	}
}
