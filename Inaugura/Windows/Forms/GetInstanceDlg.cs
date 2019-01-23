using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;

namespace Inaugura.Windows.Forms
{
	/// <summary>
	/// Summary description for GetManagerDlg.
	/// </summary>
	public class GetInstanceDlg : System.Windows.Forms.Form
	{
		private object mObj = null;
		private Type mType;
		private System.Windows.Forms.ListView mInstances;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public object Object
		{
			get
			{
				return this.mObj;
			}
		}


		public GetInstanceDlg(Type type)
		{
			//
			// Required for Windows Form Designer support
			//

			this.mType = type;

			InitializeComponent();

			this.FillInstanceList();
		}

		private void FillInstanceList()
		{
			this.mInstances.Clear();

			string[] files = System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory(),"*.dll");
			foreach(string file in files)
			{
				Assembly assembly = Assembly.LoadFrom(file);
				Type[] types = assembly.GetTypes();
				foreach(Type t in types)
				{			
					if(t.IsSubclassOf(this.mType) && !t.IsAbstract)
					{
						try
						{
							object obj = (object)Activator.CreateInstance(t);						
							InstanceListItem li = new InstanceListItem(obj);
							this.mInstances.Items.Add(li);
						}
						catch(Exception e)
						{
							string str = e.Message;
						}
					}			
				}					
			}	
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
			this.mInstances = new System.Windows.Forms.ListView();
			this.SuspendLayout();
			// 
			// mInstances
			// 
			this.mInstances.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mInstances.Location = new System.Drawing.Point(0, 0);
			this.mInstances.Name = "mInstances";
			this.mInstances.Size = new System.Drawing.Size(344, 205);
			this.mInstances.TabIndex = 0;
			this.mInstances.View = System.Windows.Forms.View.List;
			this.mInstances.DoubleClick += new System.EventHandler(this.mInstance_DoubleClick);
			// 
			// GetInstanceDlg
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(344, 205);
			this.Controls.Add(this.mInstances);
			this.Name = "GetInstanceDlg";
			this.Text = "Select an Instance";
			this.ResumeLayout(false);

		}
		#endregion

		private void mInstance_DoubleClick(object sender, System.EventArgs e)
		{
			InstanceListItem i = (InstanceListItem)this.mInstances.SelectedItems[0];
			this.mObj = i.Object;
			this.Close();
		}
	}
}
