using System;
using System.Windows.Forms;
using System.Drawing;

using Inaugura.Telephony.Services;

namespace OrbisSwitch
{
	/// <summary>
	/// Summary description for ServiceLineTreeNode.
	/// </summary>
	/// 
	

	public class ServiceTreeNode : StatusTreeNode
	{
		private delegate void FunctionDelegate();
		public ServiceTreeNode(Service service) : base(service)
		{
			this.ImageIndex = 1;
			this.SelectedImageIndex = 1;

			this.Tag = service;

			service.ServiceStarted += new ServiceHandler(service_ServiceStarted);
			service.ServiceStopped += new ServiceHandler(service_ServiceStopped);
			service.ServiceActive += new ServiceHandler(service_ServiceActive);
			service.ServiceInactive += new ServiceHandler(service_ServiceInactive);
			service.LineShortage += new ServiceHandler(service_LineShortage);
			service.LineSurplus += new ServiceHandler(service_LineSurplus);
		
			//service.LinesChangedEvent += new EventHandler(this.OnLinesChangedEvent);
			if(service.Started)				
				this.ForeColor = Color.Blue;
			else
				this.ForeColor = Color.DarkGray;
		}

		public void GenerateLineNodes()
		{	
			try
			{				
				this.Nodes.Clear();
				foreach(ServiceLine l in ((Service)this.Object))
				{
					
					/*
					ServiceLineTreeNode ln = new ServiceLineTreeNode(l);						
					// Adding an item to the list has to be done on the
					// main thread of the control. We can get to it by
					// setting up a delegate that we want to call,
					// and then calling Invoke() on the treeview control.
					AddDelegate addDelegate = new AddDelegate(this.Nodes.Add);
					// Bug: Invoke should be declared with params.
					this.TreeView.Invoke(addDelegate, new object[] {ln});	
					*/

				}
			}
			catch(Exception ex)
			{
				string str = ex.ToString();
			}
		}

		/*
		protected void OnLinesChangedEvent(object sender, EventArgs e)
		{
			Service service = (Service)sender;
			this.GenerateLineNodes();
			int numberOfLines = service.Lines.Count;
			if(numberOfLines == 1)
				this.Text = ((Service)this.Object).ToString()+string.Format(" ({0} Line)",numberOfLines);
			else
				this.Text = ((Service)this.Object).ToString()+string.Format(" ({0} Lines)",numberOfLines);
		}
		*/

		void service_ServiceStarted(object sender, ServiceEventArgs e)
		{
			this.TreeView.BeginInvoke(new FunctionDelegate(this.StartedHandler), null);
		}
		private void StartedHandler()
		{
			this.ForeColor = Color.DarkBlue;
		}

		void service_ServiceStopped(object sender, ServiceEventArgs e)
		{
			this.TreeView.BeginInvoke(new FunctionDelegate(this.StoppedHandler), null);
		}
		private void StoppedHandler()
		{
			this.ForeColor = Color.DarkGray;
		}

		void service_ServiceActive(object sender, ServiceEventArgs e)
		{			
			this.TreeView.BeginInvoke(new FunctionDelegate(this.ActiveHandler), null);
		}
		private void ActiveHandler()
		{
			this.NodeFont = new Font(this.TreeView.Font.FontFamily, this.TreeView.Font.Size, FontStyle.Bold);
		}

		void service_ServiceInactive(object sender, ServiceEventArgs e)
		{
			this.TreeView.BeginInvoke(new FunctionDelegate(this.InactiveHandler), null);
		}
		private void InactiveHandler()
		{
			this.NodeFont = new Font(this.TreeView.Font.FontFamily, this.TreeView.Font.Size, FontStyle.Regular);
		}

		void service_LineShortage(object sender, ServiceEventArgs e)
		{
			this.TreeView.BeginInvoke(new FunctionDelegate(this.ShortageHandler), null);
		}
		private void ShortageHandler()
		{
			this.ImageIndex = 4;
		}

		void service_LineSurplus(object sender, ServiceEventArgs e)
		{
			this.TreeView.BeginInvoke(new FunctionDelegate(this.SurplusHandler), null);
		}
		private void SurplusHandler()
		{
			this.ImageIndex = 3;
		}
	}
}
