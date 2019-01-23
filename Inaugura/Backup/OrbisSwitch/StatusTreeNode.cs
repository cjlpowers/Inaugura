using System;

using Inaugura.Telephony;

namespace OrbisSwitch
{
	/// <summary>
	/// Summary description for ObjectNode.
	/// </summary>
	public abstract class StatusTreeNode : System.Windows.Forms.TreeNode
	{
		private delegate void StatusDelagate(IStatusable obj);

		#region Variables
		private IStatusable mObject;			// the status object
		#endregion

		#region Properties
		public IStatusable Object
		{
			get
			{
				return this.mObject;
			}
			private set
			{	
				this.mObject = value;
				this.UpdateStatus(value);
			}
		}
		#endregion

		#region Methods
		public StatusTreeNode(IStatusable obj)
		{				
			this.Object = obj;
			obj.StatusChanged += new StatusHandler(this.OnStatusChanged);
		}	

		private void OnStatusChanged(object sender, StatusEventArgs e)
		{
            if (this.TreeView != null)
            {
                this.TreeView.BeginInvoke(new StatusDelagate(this.UpdateStatus), new object[] { e.StatusSoruce});
            }
        }

		private void UpdateStatus(IStatusable obj)
		{
            if (obj != null)
            {
                string newText = obj.ToString() + ": " + obj.Status;
                if (this.Text != newText)
                    this.Text = newText;
            }
        }
		#endregion
	}
}
