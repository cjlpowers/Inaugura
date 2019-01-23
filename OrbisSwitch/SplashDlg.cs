using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

using Inaugura;

namespace OrbisSwitch
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class SplashDlg : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListBox mListBox;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label mNameVersionLabel;
		private Inaugura.LogHandler mLogTextHandler;
		private System.Windows.Forms.Button mCloseBtn;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		public SplashDlg(string nameVersionText)
		{			
			//
			// Required for Windows Form Designer support
			//

			InitializeComponent();		
			
			Log.Font = this.mListBox.Font;
			Log.Color = this.mListBox.ForeColor;

			this.mLogTextHandler = new LogHandler(this.AddText);
			Log.LogText+= this.mLogTextHandler;

            this.mNameVersionLabel.Text = nameVersionText;
		}

		public void FadeIn()
		{
			Thread thread = new Thread(new ThreadStart(this.RunFadeIn));
			thread.Start();
		}

		public void FadeOut()
		{
			Thread thread = new Thread(new ThreadStart(this.RunFadeOut));
			thread.Start();
		}
        
		private void RunFadeIn()
		{
			for(double i =.1; i <= 1; i+=.1)
			{
				this.Opacity = i;
				Thread.Sleep(50);
			}
		}

		private void RunFadeOut()
		{
			for(double i =1; i > 0; i-=.1)
			{
				this.Opacity = i;
				Thread.Sleep(50);
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

		public void ShowAsWindow()
		{
			this.mCloseBtn.Visible = true;			
			//this.Opacity = 1;
			this.Visible = true;
			//this.Opacity = 0;
			//this.mCloseBtn.Visible = false;
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SplashDlg));
			this.mListBox = new System.Windows.Forms.ListBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.mNameVersionLabel = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.mCloseBtn = new System.Windows.Forms.Button();
			this.panel3.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// mListBox
			// 
			this.mListBox.BackColor = System.Drawing.Color.White;
			this.mListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.mListBox.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.mListBox.ForeColor = System.Drawing.SystemColors.WindowText;
			this.mListBox.IntegralHeight = false;
			this.mListBox.Location = new System.Drawing.Point(8, 244);
			this.mListBox.Name = "mListBox";
			this.mListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.mListBox.Size = new System.Drawing.Size(512, 172);
			this.mListBox.TabIndex = 1;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.label2);
			this.panel3.Controls.Add(this.mNameVersionLabel);
			this.panel3.Location = new System.Drawing.Point(8, 196);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(512, 44);
			this.panel3.TabIndex = 6;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(8, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(424, 16);
			this.label2.TabIndex = 7;
			this.label2.Text = "Copyright © 2000-2003  Orbis Software Solutions. All rights reserved ";
			// 
			// mNameVersionLabel
			// 
			this.mNameVersionLabel.AutoSize = true;
			this.mNameVersionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.mNameVersionLabel.Location = new System.Drawing.Point(8, 4);
			this.mNameVersionLabel.Name = "mNameVersionLabel";
			this.mNameVersionLabel.Size = new System.Drawing.Size(127, 19);
			this.mNameVersionLabel.TabIndex = 6;
			this.mNameVersionLabel.Text = "Name Version Text";
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(512, 181);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.panel3);
			this.panel1.Controls.Add(this.mListBox);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(528, 424);
			this.panel1.TabIndex = 7;
			// 
			// mCloseBtn
			// 
			this.mCloseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.mCloseBtn.Location = new System.Drawing.Point(472, 8);
			this.mCloseBtn.Name = "mCloseBtn";
			this.mCloseBtn.Size = new System.Drawing.Size(48, 23);
			this.mCloseBtn.TabIndex = 8;
			this.mCloseBtn.Text = "Close";
			this.mCloseBtn.Click += new System.EventHandler(this.mCloseBtn_Click);
			// 
			// SplashDlg
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(528, 424);
			this.ControlBox = false;
			this.Controls.Add(this.mCloseBtn);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "SplashDlg";
			this.ShowInTaskbar = false;
			this.Load += new System.EventHandler(this.SplashDlg_Load);
			this.panel3.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		public void AddText(object sender, LogEventArgs e)
		{			
			this.mListBox.Font = Log.Font;
			this.mListBox.ForeColor = Log.Color;
			this.mListBox.Items.Add(e.Text);
			this.mListBox.TopIndex =this.mListBox.Items.Count-1;
		}

		private void SplashDlg_Load(object sender, System.EventArgs e)
		{					
			this.mListBox.TopIndex =this.mListBox.Items.Count-1;
		}

		private void mCloseBtn_Click(object sender, System.EventArgs e)
		{
			this.Visible = false;
			//this.mCloseBtn.Visible = false;	
		}		
	}
}
