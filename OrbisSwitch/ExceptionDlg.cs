using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace OrbisSwitch
{
	/// <summary>
	/// Summary description for ExceptionDlg.
	/// </summary>
	public class ExceptionDlg : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label mLbTitle;
		private System.Windows.Forms.PictureBox mTitlePicture;
		private System.Windows.Forms.TextBox mTxtException;
		private Bitmap mScreenShot;
		private Exception mException;
		private System.Windows.Forms.PictureBox mPicture;
		private System.Windows.Forms.Label mLbExceptionTitle;
		private System.Windows.Forms.Label mLbExceptionMessage;
		private System.Windows.Forms.LinkLabel mLbHelp;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region Properties
		protected Bitmap ScreenShot
		{
			get
			{
				return this.mScreenShot;
			}
			set
			{
				this.mScreenShot = value;
			}
		}
		
		protected Exception Exception
		{
			get
			{
				return this.mException;
			}
			set
			{
				this.mException = value;
			}
		}
		#endregion		

		public ExceptionDlg(Exception e)
		{
			this.ScreenShot = Inaugura.Drawing.ScreenCapture.CaptureScreen();
			this.Exception = e;

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.Exception.HelpLink = "www.orbistel.com";
			
			this.mPicture.Image = this.ScreenShot.GetThumbnailImage(this.mPicture.Width, this.mPicture.Height, null, IntPtr.Zero);
			this.mTxtException.Text = this.Exception.ToString();
			this.mLbExceptionTitle.Text = this.Exception.GetType().Name;
			this.mLbExceptionMessage.Text = this.Exception.Message;			
			if (this.Exception.HelpLink.Length != 0 && (this.Exception.HelpLink.StartsWith("http://") || this.Exception.HelpLink.StartsWith("www")))
			{
				this.mLbHelp.Text = "For help on this exception click here";
				this.mLbHelp.Links.Add(0, this.mLbHelp.Text.Length, this.Exception.HelpLink);
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ExceptionDlg));

			this.mTxtException = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.mTitlePicture = new System.Windows.Forms.PictureBox();
			this.mLbTitle = new System.Windows.Forms.Label();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.mLbHelp = new System.Windows.Forms.LinkLabel();
			this.mLbExceptionMessage = new System.Windows.Forms.Label();
			this.mLbExceptionTitle = new System.Windows.Forms.Label();
			this.mPicture = new System.Windows.Forms.PictureBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mTitlePicture)).BeginInit();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mPicture)).BeginInit();
			this.panel3.SuspendLayout();
			this.SuspendLayout();

			// 
			// mTxtException
			// 
			this.mTxtException.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mTxtException.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mTxtException.ForeColor = System.Drawing.SystemColors.WindowText;
			this.mTxtException.Location = new System.Drawing.Point(0, 0);
			this.mTxtException.Multiline = true;
			this.mTxtException.Name = "mTxtException";
			this.mTxtException.Size = new System.Drawing.Size(492, 302);
			this.mTxtException.TabIndex = 0;

			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Black;
			this.panel1.Controls.Add(this.mTitlePicture);
			this.panel1.Controls.Add(this.mLbTitle);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(492, 50);
			this.panel1.TabIndex = 1;

			// 
			// mTitlePicture
			// 
			this.mTitlePicture.BackColor = System.Drawing.Color.Transparent;
			this.mTitlePicture.Image = ((System.Drawing.Image)(resources.GetObject("mTitlePicture.Image")));
			this.mTitlePicture.Location = new System.Drawing.Point(3, 3);
			this.mTitlePicture.Name = "mTitlePicture";
			this.mTitlePicture.Size = new System.Drawing.Size(47, 45);
			this.mTitlePicture.TabIndex = 6;
			this.mTitlePicture.TabStop = false;
			this.mTitlePicture.WaitOnLoad = false;

			// 
			// mLbTitle
			// 
			this.mLbTitle.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mLbTitle.ForeColor = System.Drawing.Color.White;
			this.mLbTitle.Location = new System.Drawing.Point(46, 14);
			this.mLbTitle.Name = "mLbTitle";
			this.mLbTitle.Size = new System.Drawing.Size(396, 29);
			this.mLbTitle.TabIndex = 5;
			this.mLbTitle.Text = "Application Exception";
			this.mLbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 492);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(492, 57);
			this.flowLayoutPanel1.TabIndex = 2;

			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.mLbHelp);
			this.panel2.Controls.Add(this.mLbExceptionMessage);
			this.panel2.Controls.Add(this.mLbExceptionTitle);
			this.panel2.Controls.Add(this.mPicture);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 50);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(492, 140);
			this.panel2.TabIndex = 3;

			// 
			// mLbHelp
			// 
			this.mLbHelp.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mLbHelp.Links.Add(new System.Windows.Forms.LinkLabel.Link(0, 0));
			this.mLbHelp.Location = new System.Drawing.Point(7, 113);
			this.mLbHelp.Name = "mLbHelp";
			this.mLbHelp.Size = new System.Drawing.Size(477, 23);
			this.mLbHelp.TabIndex = 3;
			this.mLbHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.mLbHelp_LinkClicked);

			// 
			// mLbExceptionMessage
			// 
			this.mLbExceptionMessage.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mLbExceptionMessage.Location = new System.Drawing.Point(7, 29);
			this.mLbExceptionMessage.Name = "mLbExceptionMessage";
			this.mLbExceptionMessage.Size = new System.Drawing.Size(352, 76);
			this.mLbExceptionMessage.TabIndex = 2;
			this.mLbExceptionMessage.Text = "label2";

			// 
			// mLbExceptionTitle
			// 
			this.mLbExceptionTitle.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mLbExceptionTitle.Location = new System.Drawing.Point(7, 6);
			this.mLbExceptionTitle.Name = "mLbExceptionTitle";
			this.mLbExceptionTitle.Size = new System.Drawing.Size(342, 23);
			this.mLbExceptionTitle.TabIndex = 1;
			this.mLbExceptionTitle.Text = "label1";

			// 
			// mPicture
			// 
			this.mPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.mPicture.Location = new System.Drawing.Point(367, 6);
			this.mPicture.Name = "mPicture";
			this.mPicture.Size = new System.Drawing.Size(120, 102);
			this.mPicture.TabIndex = 0;
			this.mPicture.TabStop = false;
			this.mPicture.WaitOnLoad = false;

			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.mTxtException);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 190);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(492, 302);
			this.panel3.TabIndex = 4;

			// 
			// ExceptionDlg
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(492, 549);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "ExceptionDlg";
			this.Text = "An Exception has occured";
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.mTitlePicture)).EndInit();
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.mPicture)).EndInit();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.ResumeLayout(false);
		}
#endregion

		private void mLbHelp_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			if (e.Link.LinkData != null && e.Link.LinkData is string)
			{
				string data = (string)e.Link.LinkData;
				if (data.StartsWith("http://") || data.StartsWith("www"))
                    System.Diagnostics.Process.Start(data);
			}
		}
	}
}
