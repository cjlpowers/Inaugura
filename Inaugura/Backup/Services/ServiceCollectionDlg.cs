using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;

using Inaugura.Windows.Forms;

namespace Inaugura.Telephony.Services
{
	/// <summary>
	/// Summary description for FilterListDlg.
	/// </summary>
	public class ServiceCollectionDlg : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;

		private ServiceCollection mList;
		private System.Windows.Forms.ListBox mServices;
		private System.Windows.Forms.ListBox mSelectedServices; 
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ServiceCollectionDlg(ServiceCollection list)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.FillAvailServiceList();	

			mList = list;
			foreach (Service s in mList)
			{			
				this.mSelectedServices.Items.Add(s);				
			}		
		}

		private void FillAvailServiceList()
		{
			this.mServices.Items.Clear();

			/*
			ServiceCollection list = DataAdaptor.CurrentDataAdaptor.ServiceStore.GetServices();
			foreach (IService s in list)
			{
				this.mServices.Items.Add(s);
			}
			*/
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mSelectedServices = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.mServices = new System.Windows.Forms.ListBox();
			this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			this.SuspendLayout();
			// 
			// mSelectedServices
			// 
			this.mSelectedServices.Location = new System.Drawing.Point(248, 32);
			this.mSelectedServices.Name = "mSelectedServices";
			this.mSelectedServices.Size = new System.Drawing.Size(200, 238);
			this.mSelectedServices.TabIndex = 1;
			this.mSelectedServices.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mSelectedServices_KeyDown);
			this.mSelectedServices.DoubleClick += new System.EventHandler(this.mSelectedServices_DoubleClick);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(97, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Available Services";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(248, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(95, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Selected Services";
			// 
			// mServices
			// 
			this.mServices.Location = new System.Drawing.Point(8, 32);
			this.mServices.Name = "mServices";
			this.mServices.Size = new System.Drawing.Size(200, 238);
			this.mServices.TabIndex = 0;
			this.mServices.DoubleClick += new System.EventHandler(this.mServices_DoubleClick);
			// 
			// ServiceListDlg
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(464, 291);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.mSelectedServices);
			this.Controls.Add(this.mServices);
			this.Name = "ServiceListDlg";
			this.Text = "Services List";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.ServiceListDlg_Closing);
			this.ResumeLayout(false);

		}
		#endregion

		private void mServices_DoubleClick(object sender, System.EventArgs e)
		{
			if(this.mServices.SelectedIndex != -1)
			{
				Service s = (Service)this.mServices.SelectedItem;
				this.mSelectedServices.Items.Add(s);
			}
		}

		private void ServiceListDlg_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.mList.Clear();
			foreach(Service s in this.mSelectedServices.Items)
			{
				this.mList.Add(s);
			}
		}

		private void mSelectedServices_DoubleClick(object sender, System.EventArgs e)
		{
			if(this.mSelectedServices.SelectedIndex != -1)
			{
				ConfigItemDlg dlg = new ConfigItemDlg(this.mSelectedServices.SelectedItem);
				dlg.ShowDialog();
			}
		}

		private void mSelectedServices_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == System.Windows.Forms.Keys.Delete)
			{
				if(this.mSelectedServices.SelectedIndex != -1)
				{
					this.mSelectedServices.Items.RemoveAt(this.mSelectedServices.SelectedIndex);																		   
				}
			}
		}
	}
}
