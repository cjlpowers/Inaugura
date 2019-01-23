#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#endregion

namespace Inaugura.Telephony
{
	class GetHardwareDlg: System.Windows.Forms.Form
	{
		public TelephonyHardware Hardware
		{
			get
			{
				return this.mHardware;
			}
			private set
			{
				this.mHardware = value;
			}	
		}

		public GetHardwareDlg()
		{
			InitializeComponent();
		}

		private Button mBtnCancel;
		private Button mBtnSelect;
		private Label label2;
		private ListBox mHardwareList;
		private Button mBtnBrowse;
		private Label label1;
		private TextBox mTxtFile;
		private TelephonyHardware mHardware;

		#region Windows forms designer code

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mBtnCancel = new System.Windows.Forms.Button();
			this.mBtnSelect = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.mHardwareList = new System.Windows.Forms.ListBox();
			this.mBtnBrowse = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.mTxtFile = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
// 
// mBtnCancel
// 
			this.mBtnCancel.Location = new System.Drawing.Point(224, 216);
			this.mBtnCancel.Name = "mBtnCancel";
			this.mBtnCancel.TabIndex = 20;
			this.mBtnCancel.Text = "Cancel";
			this.mBtnCancel.Click += new System.EventHandler(this.mBtnCancel_Click);
// 
// mBtnSelect
// 
			this.mBtnSelect.Enabled = false;
			this.mBtnSelect.Location = new System.Drawing.Point(142, 216);
			this.mBtnSelect.Name = "mBtnSelect";
			this.mBtnSelect.TabIndex = 19;
			this.mBtnSelect.Text = "Select";
			this.mBtnSelect.Click += new System.EventHandler(this.mBtnSelect_Click);
// 
// label2
// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(11, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(52, 14);
			this.label2.TabIndex = 22;
			this.label2.Text = "Hardware";
// 
// mHardwareList
// 
			this.mHardwareList.FormattingEnabled = true;
			this.mHardwareList.Location = new System.Drawing.Point(11, 68);
			this.mHardwareList.Name = "mHardwareList";
			this.mHardwareList.Size = new System.Drawing.Size(437, 134);
			this.mHardwareList.TabIndex = 21;
			this.mHardwareList.SelectedIndexChanged += new System.EventHandler(this.mHardwareList_SelectedIndexChanged);
// 
// mBtnBrowse
// 
			this.mBtnBrowse.Location = new System.Drawing.Point(423, 15);
			this.mBtnBrowse.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
			this.mBtnBrowse.Name = "mBtnBrowse";
			this.mBtnBrowse.Size = new System.Drawing.Size(25, 23);
			this.mBtnBrowse.TabIndex = 17;
			this.mBtnBrowse.Text = "...";
			this.mBtnBrowse.Click += new System.EventHandler(this.mBtnBrowse_Click);
// 
// label1
// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(11, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(22, 14);
			this.label1.TabIndex = 18;
			this.label1.Text = "File";
// 
// mTxtFile
// 
			this.mTxtFile.Location = new System.Drawing.Point(40, 16);
			this.mTxtFile.Margin = new System.Windows.Forms.Padding(3, 3, 1, 3);
			this.mTxtFile.Name = "mTxtFile";
			this.mTxtFile.Size = new System.Drawing.Size(381, 20);
			this.mTxtFile.TabIndex = 16;
// 
// GetHardwareDlg
// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(462, 251);
			this.Controls.Add(this.mTxtFile);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.mBtnBrowse);
			this.Controls.Add(this.mBtnCancel);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.mBtnSelect);
			this.Controls.Add(this.mHardwareList);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "GetHardwareDlg";
			this.Text = "GetHardwareDlg";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

	#endregion

		private void mBtnSelect_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Hardware = (TelephonyHardware)this.mHardwareList.SelectedItem;
			this.Close();
		}

		private void mBtnBrowse_Click(object sender, EventArgs e)
		{
			Type hardwareInterface = typeof(Inaugura.Telephony.TelephonyHardware);
			System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
			dlg.DefaultExt = "dll";
			dlg.Filter = "Assembly files (*.dll)|*.dll";

			if (System.Windows.Forms.DialogResult.OK == dlg.ShowDialog())
			{
				this.mHardwareList.Items.Clear();
				this.mTxtFile.Text = dlg.FileName;

				// now get all the types in the assembly that implement IService
				System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(this.mTxtFile.Text);
				Type[] types = assembly.GetTypes();

				foreach (Type t in types)
				{
					if (!t.IsAbstract)
					{
						Type[] supportedInterfaces = t.GetInterfaces();
						foreach (Type iface in supportedInterfaces)
						{
							if (iface == hardwareInterface)
							{
								object obj = Activator.CreateInstance(t);
								this.mHardwareList.DisplayMember = "Name";
								this.mHardwareList.Items.Add(obj);																
							}
						}
					}
				}
			}
		}

		private void mBtnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Hardware = null;
			this.Close();
		}

		private void mHardwareList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.mHardwareList.SelectedItems.Count != 0)
			{
				this.mBtnSelect.Enabled = true;
			}
			else
			{
				this.mBtnSelect.Enabled = false;
			}
		}
	}
}