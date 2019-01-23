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

	class TelephonyManagerEditorDlg: System.Windows.Forms.Form
	{
		private TelephonyManager mTelephonyManager;

		public TelephonyManagerEditorDlg(TelephonyManager obj)
		{
			InitializeComponent();
			this.mTelephonyManager = obj;
			if (this.mTelephonyManager.Hardware != null)
				this.mLbHardwareName.Text = this.mTelephonyManager.Hardware.Name;
		}

		private Button mBtnGetHardware;
		private Label mLbHardwareName;
		private GroupBox groupBox1;

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
			this.mBtnGetHardware = new System.Windows.Forms.Button();
			this.mLbHardwareName = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
// 
// mBtnGetHardware
// 
			this.mBtnGetHardware.Location = new System.Drawing.Point(303, 20);
			this.mBtnGetHardware.Name = "mBtnGetHardware";
			this.mBtnGetHardware.Size = new System.Drawing.Size(24, 23);
			this.mBtnGetHardware.TabIndex = 3;
			this.mBtnGetHardware.Text = "...";
			this.mBtnGetHardware.Click += new System.EventHandler(this.mBtnGetHardware_Click);
// 
// mLbHardwareName
// 
			this.mLbHardwareName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mLbHardwareName.ForeColor = System.Drawing.Color.Blue;
			this.mLbHardwareName.Location = new System.Drawing.Point(7, 20);
			this.mLbHardwareName.Name = "mLbHardwareName";
			this.mLbHardwareName.Size = new System.Drawing.Size(288, 23);
			this.mLbHardwareName.TabIndex = 2;
			this.mLbHardwareName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.mLbHardwareName.Click += new System.EventHandler(this.mLbHardwareName_Click);
// 
// groupBox1
// 
			this.groupBox1.Controls.Add(this.mBtnGetHardware);
			this.groupBox1.Controls.Add(this.mLbHardwareName);
			this.groupBox1.Location = new System.Drawing.Point(7, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(336, 57);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Hardware";
// 
// TelephonyManagerEditorDlg
// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(351, 71);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "TelephonyManagerEditorDlg";
			this.Text = "TelephonyManagerEditorDlg";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

	#endregion

		private void mBtnGetHardware_Click(object sender, EventArgs e)
		{
			GetHardwareDlg dlg = new GetHardwareDlg();		
			if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.mTelephonyManager.Hardware = dlg.Hardware;
				this.mLbHardwareName.Text = this.mTelephonyManager.Hardware.Name;
			}
		}

		private void mLbHardwareName_Click(object sender, EventArgs e)
		{
			if (this.mLbHardwareName.Text.Length != 0)
			{
                Inaugura.Windows.Forms.ConfigItemDlg dlg = new Inaugura.Windows.Forms.ConfigItemDlg(this.mTelephonyManager.Hardware);
                dlg.ShowDialog();
			}
		}
	}
	}