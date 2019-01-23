using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Inaugura.Telephony
{
	/// <summary>
	/// Summary description for HardwareSelectorDlg.
	/// </summary>
	public class HardwareSelectorDlg : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label mHardwareName;
		private System.Windows.Forms.Button mBtnGetHardware;
		private Inaugura.Telephony.TelephonyHardware mHardware = null;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Inaugura.Telephony.TelephonyHardware Hardware
		{
			get
			{
				return this.mHardware;
			}
		}

		public HardwareSelectorDlg(TelephonyHardware hardware)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			if(hardware != null)
			{
				this.mHardware = hardware;
				this.mHardwareName.Text = hardware.ToString();
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
			this.mHardwareName = new System.Windows.Forms.Label();
			this.mBtnGetHardware = new System.Windows.Forms.Button();
			this.SuspendLayout();
// 
// mHardwareName
// 
			this.mHardwareName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mHardwareName.ForeColor = System.Drawing.Color.Blue;
			this.mHardwareName.Location = new System.Drawing.Point(8, 16);
			this.mHardwareName.Name = "mHardwareName";
			this.mHardwareName.Size = new System.Drawing.Size(288, 23);
			this.mHardwareName.TabIndex = 0;
			this.mHardwareName.Text = "label1";
			this.mHardwareName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.mHardwareName.DoubleClick += new System.EventHandler(this.label1_DoubleClick);
			this.mHardwareName.Click += new System.EventHandler(this.mHardwareName_Click);
// 
// mBtnGetHardware
// 
			this.mBtnGetHardware.Location = new System.Drawing.Point(304, 16);
			this.mBtnGetHardware.Name = "mBtnGetHardware";
			this.mBtnGetHardware.Size = new System.Drawing.Size(24, 23);
			this.mBtnGetHardware.TabIndex = 1;
			this.mBtnGetHardware.Text = "...";
			this.mBtnGetHardware.Click += new System.EventHandler(this.mBtnGetHardware_Click);
// 
// HardwareSelectorDlg
// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(336, 53);
			this.Controls.Add(this.mBtnGetHardware);
			this.Controls.Add(this.mHardwareName);
			this.Name = "HardwareSelectorDlg";
			this.Text = "HardwareSelectorDlg";
			this.ResumeLayout(false);

		}
		#endregion

		private void label1_DoubleClick(object sender, System.EventArgs e)
		{
			if(this.mHardware != null)
			{
				Inaugura.Windows.Forms.ConfigItemDlg dlg = new Inaugura.Windows.Forms.ConfigItemDlg(this.mHardware);
				dlg.ShowDialog();
			}		
		}

		private void mBtnGetHardware_Click(object sender, System.EventArgs e)
		{
			Inaugura.Windows.Forms.GetInstanceDlg dlg = new Inaugura.Windows.Forms.GetInstanceDlg(typeof(Inaugura.Telephony.TelephonyHardware));
			dlg.ShowDialog();

			if(dlg.Object != null)
			{
				this.mHardware = (Inaugura.Telephony.TelephonyHardware)dlg.Object;
				this.mHardwareName.Text = this.Hardware.ToString();
			}
		}

		private void mHardwareName_Click(object sender, EventArgs e)
		{
		
		}
	}
}
