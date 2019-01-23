using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Inaugura.Windows.Forms
{
	/// <summary>
	/// Summary description for NewsItem.
	/// </summary>
	public class ConfigItemDlg : System.Windows.Forms.Form
	{
		protected internal System.Windows.Forms.PropertyGrid propertyGrid1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Panel panel1;
		private Button mBtnCancel;
		private Button mBtnSave;
		private bool mPropertyChanged = false;

		public bool PropertyChanged
		{
			get
			{
				return this.mPropertyChanged;
			}
			private set
			{
				this.mPropertyChanged = value;
			}
		}

		public ConfigItemDlg(object item)
		{
			
			//
			// Required for Windows Form Designer support
			//

			InitializeComponent();

			this.Text = item.ToString();
			this.propertyGrid1.SelectedObject = item;
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
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.panel1 = new System.Windows.Forms.Panel();
			this.mBtnSave = new System.Windows.Forms.Button();
			this.mBtnCancel = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
// 
// propertyGrid1
// 
			this.propertyGrid1.CommandsVisibleIfAvailable = true;
			this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(632, 461);
			this.propertyGrid1.TabIndex = 1;
			this.propertyGrid1.Text = "mPropertyGrid";
			this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
// 
// panel1
// 
			this.panel1.Controls.Add(this.mBtnCancel);
			this.panel1.Controls.Add(this.mBtnSave);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 421);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(632, 40);
			this.panel1.TabIndex = 2;
// 
// mBtnSave
// 
			this.mBtnSave.Location = new System.Drawing.Point(238, 9);
			this.mBtnSave.Name = "mBtnSave";
			this.mBtnSave.TabIndex = 0;
			this.mBtnSave.Text = "Save";
			this.mBtnSave.Click += new System.EventHandler(this.mBtnOK_Click);
// 
// mBtnCancel
// 
			this.mBtnCancel.Location = new System.Drawing.Point(320, 9);
			this.mBtnCancel.Name = "mBtnCancel";
			this.mBtnCancel.TabIndex = 1;
			this.mBtnCancel.Text = "Cancel";
			this.mBtnCancel.Click += new System.EventHandler(this.mBtnCancel_Click);
// 
// ConfigItemDlg
// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(632, 461);
			this.ControlBox = false;
			this.Controls.Add(this.propertyGrid1);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "ConfigItemDlg";
			this.Text = "NewsItem";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigItemDlg_FormClosing);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void mBtnOK_Click(object sender, System.EventArgs e)
		{
			this.PropertyChanged = true;
			this.Close();
		}

		private void mBtnCancel_Click(object sender, System.EventArgs e)
		{
			this.PropertyChanged = false;
			this.Close();
		}

		private void propertyGrid1_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			this.mPropertyChanged = true;
		}

		private void ConfigItemDlg_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.PropertyChanged)
				this.DialogResult = DialogResult.OK;
			else
				this.DialogResult = DialogResult.Cancel;
		}

	}
}
