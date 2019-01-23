using System;

namespace Inaugura.Windows.Forms
{
	/// <summary>
	/// Summary description for ManagerListItem.
	/// </summary>
	public class InstanceListItem : System.Windows.Forms.ListViewItem
	{
		private Object mObj = null;

		public Object Object
		{
			get
			{
				return this.mObj;
			}
		}

		public InstanceListItem(Object obj)
		{
			this.mObj = obj;
			this.Text = obj.ToString();
		}
	}
}
