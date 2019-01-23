using System;
using System.Drawing;

using Inaugura.Telephony.Services;

namespace OrbisSwitch
{
	/// <summary>
	/// Summary description for ServiceLineTreeNode.
	/// </summary>
	public class ServiceLineTreeNode : StatusTreeNode
	{
		private delegate void FunctionDelegate();
		public ServiceLineTreeNode(ServiceLine line) : base(line)
		{
			this.ImageIndex = 2;
			this.SelectedImageIndex = 2;
			this.Tag = line;

			line.ServiceLineStarted += new ServiceLineHandler(this.OnStartedEvent);
			line.ServiceLineStopped += new ServiceLineHandler(this.OnStoppedEvent);
			line.ServiceLineActive += new ServiceLineHandler(this.OnActiveEvent);
			line.ServiceLineInactive += new ServiceLineHandler(this.OnInactiveEvent);
			if(line.Started)
				this.ForeColor = Color.DarkGreen;
			else
				this.ForeColor = Color.DarkGray;

			this.UpdateToolTip();
		}

		private void UpdateToolTip()
		{
			ServiceLine line = (ServiceLine)this.Tag;
			string toolTip = "Calls: "+line.CallsProcessed+"\r\n";
			toolTip += "IdleTime: " + line.IdleTime.ToString() + "\r\n";			

			this.ToolTipText = toolTip;
		}

		protected void OnStartedEvent(object sender, ServiceLineEventArgs e)
		{
			this.TreeView.BeginInvoke(new FunctionDelegate(this.StartedHandler), null);
		}
		private void StartedHandler()
		{
			//this.ForeColor = Color.DarkGreen;
		}

		protected void OnStoppedEvent(object sender, ServiceLineEventArgs e)
		{
			this.TreeView.BeginInvoke(new FunctionDelegate(this.StoppedHandler), null);
		}
		private void StoppedHandler()
		{
			//this.ForeColor = Color.DarkGray;
		}

		protected void OnActiveEvent(object sender, ServiceLineEventArgs e)
		{
			this.TreeView.BeginInvoke(new FunctionDelegate(this.ActiveHandler), null);
		}

		private void ActiveHandler()
		{
			if (this.TreeView != null)
			{
				this.NodeFont = new Font(this.TreeView.Font.FontFamily, this.TreeView.Font.Size, FontStyle.Underline);
			}
			this.UpdateToolTip();
		}

		protected void OnInactiveEvent(object sender, ServiceLineEventArgs e)
		{
			this.TreeView.BeginInvoke(new FunctionDelegate(this.InactiveHandler), null);
		}

		private void InactiveHandler()
		{
			if (this.TreeView != null)
				this.NodeFont = new Font(this.TreeView.Font.FontFamily, this.TreeView.Font.Size, FontStyle.Regular);
		}
	}
}
