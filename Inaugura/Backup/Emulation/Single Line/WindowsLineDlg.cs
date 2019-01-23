using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Inaugura.Telephony.Emulation
{
	/// <summary>
	/// Summary description for WindowsLine.
	/// </summary>
	internal class WindowsLineDlg : System.Windows.Forms.Form
	{

		private delegate string GetValue();

		protected DigitBuffer mDigitBuffer = new DigitBuffer();		
		protected bool mOffHook = false;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;

		protected WindowsLine mLine;
		private System.Windows.Forms.Button mBtnOnOffLine;
		private System.Windows.Forms.TextBox mTxtBuffer;
		private System.Windows.Forms.Button mBtnPound;
		private System.Windows.Forms.Button mBtnZero;
		private System.Windows.Forms.Button mBtnStar;
		private System.Windows.Forms.Button mBtnNine;
		private System.Windows.Forms.Button mBtnEight;
		private System.Windows.Forms.Button mBtnSeven;
		private System.Windows.Forms.Button mBtnSix;
		private System.Windows.Forms.Button mBtnFive;
		private System.Windows.Forms.Button mBtnFour;
		private System.Windows.Forms.Button mBtnThree;
		private System.Windows.Forms.Button mBtnTwo;
		private System.Windows.Forms.Button mBtnOne;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox mTxtNumber;
		private System.Windows.Forms.TextBox mTxtName;
		public static System.Windows.Forms.TextBox TextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
		private IContainer components;



		public DigitBuffer DigitBuffer 
		{
			get
			{
				return mDigitBuffer;
			}			
		}

		public string CallerName
		{
			get
			{
				return this.mTxtName.Text;
			}
		}

		public string CallerNumber
		{
			get
			{
				return this.mTxtNumber.Text;
			}
		}

		public System.Windows.Forms.TextBox DigitBufferControl
		{
			get
			{
				return this.mTxtBuffer;
			}
		}

		public bool OffHook
		{
			get
			{
				return this.mOffHook;
			}
			set
			{
				this.mOffHook = value;

				if(value)
				{
					this.BeginInvoke(new MethodInvoker(this.SetOffHookText));
				}
				else
				{
					this.BeginInvoke(new MethodInvoker(this.SetOnHookText));
				}
			}
		}

		public WindowsLine Line
		{
			get
			{
				return this.mLine;
			}
			set
			{
				mLine = value;
			}
		}

		/// <summary>
		/// Required designer variable.
		/// </summary>

		public WindowsLineDlg()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			WindowsLineDlg.TextBox = this.textBox1;

			this.mDigitBuffer.BufferChanged += new EventHandler(this.OnDigitBufferChanged);
			
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
            this.components = new System.ComponentModel.Container();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.mBtnOnOffLine = new System.Windows.Forms.Button();
            this.mTxtBuffer = new System.Windows.Forms.TextBox();
            this.mBtnPound = new System.Windows.Forms.Button();
            this.mBtnZero = new System.Windows.Forms.Button();
            this.mBtnStar = new System.Windows.Forms.Button();
            this.mBtnNine = new System.Windows.Forms.Button();
            this.mBtnEight = new System.Windows.Forms.Button();
            this.mBtnSeven = new System.Windows.Forms.Button();
            this.mBtnSix = new System.Windows.Forms.Button();
            this.mBtnFive = new System.Windows.Forms.Button();
            this.mBtnFour = new System.Windows.Forms.Button();
            this.mBtnThree = new System.Windows.Forms.Button();
            this.mBtnTwo = new System.Windows.Forms.Button();
            this.mBtnOne = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.mTxtNumber = new System.Windows.Forms.TextBox();
            this.mTxtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2});
            this.menuItem1.Text = "Call";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.Text = "Xml";
            // 
            // mBtnOnOffLine
            // 
            this.mBtnOnOffLine.Location = new System.Drawing.Point(24, 376);
            this.mBtnOnOffLine.Name = "mBtnOnOffLine";
            this.mBtnOnOffLine.Size = new System.Drawing.Size(160, 24);
            this.mBtnOnOffLine.TabIndex = 1;
            this.mBtnOnOffLine.Text = "Off Hook";
            this.mBtnOnOffLine.Click += new System.EventHandler(this.mBtnOnOffLine_Click);
            // 
            // mTxtBuffer
            // 
            this.mTxtBuffer.Location = new System.Drawing.Point(16, 248);
            this.mTxtBuffer.Name = "mTxtBuffer";
            this.mTxtBuffer.ReadOnly = true;
            this.mTxtBuffer.Size = new System.Drawing.Size(152, 20);
            this.mTxtBuffer.TabIndex = 12;
            // 
            // mBtnPound
            // 
            this.mBtnPound.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mBtnPound.Location = new System.Drawing.Point(120, 184);
            this.mBtnPound.Name = "mBtnPound";
            this.mBtnPound.Size = new System.Drawing.Size(48, 48);
            this.mBtnPound.TabIndex = 11;
            this.mBtnPound.Text = "#";
            this.mBtnPound.Click += new System.EventHandler(this.mBtnPound_Click);
            // 
            // mBtnZero
            // 
            this.mBtnZero.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mBtnZero.Location = new System.Drawing.Point(64, 184);
            this.mBtnZero.Name = "mBtnZero";
            this.mBtnZero.Size = new System.Drawing.Size(48, 48);
            this.mBtnZero.TabIndex = 10;
            this.mBtnZero.Text = "0";
            this.mBtnZero.Click += new System.EventHandler(this.mBtnZero_Click);
            // 
            // mBtnStar
            // 
            this.mBtnStar.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mBtnStar.Location = new System.Drawing.Point(8, 184);
            this.mBtnStar.Name = "mBtnStar";
            this.mBtnStar.Size = new System.Drawing.Size(48, 48);
            this.mBtnStar.TabIndex = 9;
            this.mBtnStar.Text = "*";
            this.mBtnStar.Click += new System.EventHandler(this.mBtnStar_Click);
            // 
            // mBtnNine
            // 
            this.mBtnNine.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mBtnNine.Location = new System.Drawing.Point(120, 128);
            this.mBtnNine.Name = "mBtnNine";
            this.mBtnNine.Size = new System.Drawing.Size(48, 48);
            this.mBtnNine.TabIndex = 8;
            this.mBtnNine.Text = "9";
            this.mBtnNine.Click += new System.EventHandler(this.mBtnNine_Click);
            // 
            // mBtnEight
            // 
            this.mBtnEight.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mBtnEight.Location = new System.Drawing.Point(64, 128);
            this.mBtnEight.Name = "mBtnEight";
            this.mBtnEight.Size = new System.Drawing.Size(48, 48);
            this.mBtnEight.TabIndex = 7;
            this.mBtnEight.Text = "8";
            this.mBtnEight.Click += new System.EventHandler(this.mBtnEight_Click);
            // 
            // mBtnSeven
            // 
            this.mBtnSeven.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mBtnSeven.Location = new System.Drawing.Point(8, 128);
            this.mBtnSeven.Name = "mBtnSeven";
            this.mBtnSeven.Size = new System.Drawing.Size(48, 48);
            this.mBtnSeven.TabIndex = 6;
            this.mBtnSeven.Text = "7";
            this.mBtnSeven.Click += new System.EventHandler(this.mBtnSeven_Click);
            // 
            // mBtnSix
            // 
            this.mBtnSix.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mBtnSix.Location = new System.Drawing.Point(120, 72);
            this.mBtnSix.Name = "mBtnSix";
            this.mBtnSix.Size = new System.Drawing.Size(48, 48);
            this.mBtnSix.TabIndex = 5;
            this.mBtnSix.Text = "6";
            this.mBtnSix.Click += new System.EventHandler(this.mBtnSix_Click);
            // 
            // mBtnFive
            // 
            this.mBtnFive.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mBtnFive.Location = new System.Drawing.Point(64, 72);
            this.mBtnFive.Name = "mBtnFive";
            this.mBtnFive.Size = new System.Drawing.Size(48, 48);
            this.mBtnFive.TabIndex = 4;
            this.mBtnFive.Text = "5";
            this.mBtnFive.Click += new System.EventHandler(this.mBtnFive_Click);
            // 
            // mBtnFour
            // 
            this.mBtnFour.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mBtnFour.Location = new System.Drawing.Point(8, 72);
            this.mBtnFour.Name = "mBtnFour";
            this.mBtnFour.Size = new System.Drawing.Size(48, 48);
            this.mBtnFour.TabIndex = 3;
            this.mBtnFour.Text = "4";
            this.mBtnFour.Click += new System.EventHandler(this.mBtnFour_Click);
            // 
            // mBtnThree
            // 
            this.mBtnThree.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mBtnThree.Location = new System.Drawing.Point(120, 16);
            this.mBtnThree.Name = "mBtnThree";
            this.mBtnThree.Size = new System.Drawing.Size(48, 48);
            this.mBtnThree.TabIndex = 2;
            this.mBtnThree.Text = "3";
            this.mBtnThree.Click += new System.EventHandler(this.mBtnThree_Click);
            // 
            // mBtnTwo
            // 
            this.mBtnTwo.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mBtnTwo.Location = new System.Drawing.Point(64, 16);
            this.mBtnTwo.Name = "mBtnTwo";
            this.mBtnTwo.Size = new System.Drawing.Size(48, 48);
            this.mBtnTwo.TabIndex = 1;
            this.mBtnTwo.Text = "2";
            this.mBtnTwo.Click += new System.EventHandler(this.mBtnTwo_Click);
            // 
            // mBtnOne
            // 
            this.mBtnOne.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mBtnOne.Location = new System.Drawing.Point(8, 16);
            this.mBtnOne.Name = "mBtnOne";
            this.mBtnOne.Size = new System.Drawing.Size(48, 48);
            this.mBtnOne.TabIndex = 0;
            this.mBtnOne.Text = "1";
            this.mBtnOne.Click += new System.EventHandler(this.mBtnOne_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mTxtBuffer);
            this.groupBox1.Controls.Add(this.mBtnPound);
            this.groupBox1.Controls.Add(this.mBtnZero);
            this.groupBox1.Controls.Add(this.mBtnStar);
            this.groupBox1.Controls.Add(this.mBtnNine);
            this.groupBox1.Controls.Add(this.mBtnEight);
            this.groupBox1.Controls.Add(this.mBtnSeven);
            this.groupBox1.Controls.Add(this.mBtnSix);
            this.groupBox1.Controls.Add(this.mBtnFive);
            this.groupBox1.Controls.Add(this.mBtnFour);
            this.groupBox1.Controls.Add(this.mBtnThree);
            this.groupBox1.Controls.Add(this.mBtnTwo);
            this.groupBox1.Controls.Add(this.mBtnOne);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(184, 280);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Phone";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.mTxtNumber);
            this.groupBox2.Controls.Add(this.mTxtName);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(8, 296);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(184, 72);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Caller ID";
            // 
            // mTxtNumber
            // 
            this.mTxtNumber.Location = new System.Drawing.Point(64, 40);
            this.mTxtNumber.Name = "mTxtNumber";
            this.mTxtNumber.Size = new System.Drawing.Size(104, 20);
            this.mTxtNumber.TabIndex = 3;
            this.mTxtNumber.Text = "5196482358";
            // 
            // mTxtName
            // 
            this.mTxtName.Location = new System.Drawing.Point(64, 16);
            this.mTxtName.Name = "mTxtName";
            this.mTxtName.Size = new System.Drawing.Size(104, 20);
            this.mTxtName.TabIndex = 2;
            this.mTxtName.Text = "POWERS";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 0);
            this.label2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 0);
            this.label1.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 408);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(160, 29);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "textBox1";
            // 
            // WindowsLineDlg
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(199, 449);
            this.ControlBox = false;
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.mBtnOnOffLine);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Menu = this.mainMenu1;
            this.Name = "WindowsLineDlg";
            this.ShowInTaskbar = false;
            this.Text = "WindowsLine";
            this.TopMost = true;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.WindowsLineDlg_Closing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void mBtnOne_Click(object sender, System.EventArgs e)
		{
			this.Line.DigitPressed("1");
		}

		private void mBtnTwo_Click(object sender, System.EventArgs e)
		{
			this.Line.DigitPressed("2");
		}

		private void mBtnThree_Click(object sender, System.EventArgs e)
		{
			this.Line.DigitPressed("3");
		}

		private void mBtnFour_Click(object sender, System.EventArgs e)
		{
			this.Line.DigitPressed("4");
		}

		private void mBtnFive_Click(object sender, System.EventArgs e)
		{
			this.Line.DigitPressed("5");
		}

		private void mBtnSix_Click(object sender, System.EventArgs e)
		{
			this.Line.DigitPressed("6");
		}

		private void mBtnSeven_Click(object sender, System.EventArgs e)
		{
			this.Line.DigitPressed("7");
		}

		private void mBtnEight_Click(object sender, System.EventArgs e)
		{
			this.Line.DigitPressed("8");
		}

		private void mBtnNine_Click(object sender, System.EventArgs e)
		{
			this.Line.DigitPressed("9");
		}

		private void mBtnStar_Click(object sender, System.EventArgs e)
		{
			this.Line.DigitPressed("*");
		}

		private void mBtnZero_Click(object sender, System.EventArgs e)
		{
			this.Line.DigitPressed("0");
		}

		private void mBtnPound_Click(object sender, System.EventArgs e)
		{			
			this.Line.DigitPressed("#");
		}

		private void mBtnOnOffLine_Click(object sender, System.EventArgs e)
		{
			this.OffHook = !this.OffHook;			
		}

		private void WindowsLineDlg_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if(this.Line.HookState == HookState.OffHook)
				this.Line.OnHook();
		}		

		private void OnDigitBufferChanged(object sender, System.EventArgs args)
		{
			this.BeginInvoke(new MethodInvoker(DigitBufferChangedHandler), null);
		}
		private void DigitBufferChangedHandler()
		{
			this.mTxtBuffer.Text = this.DigitBuffer.CurrentBuffer;
		}

		private void SetOffHookText()
		{
			this.mBtnOnOffLine.Text = "Hang Up";
		}

		private void SetOnHookText()
		{
			this.mBtnOnOffLine.Text = "Off Hook";
		}

		public string GetCallerName()
		{
			return this.CallerName;
		}

		public string GetCallerNumber()
		{
			return this.CallerNumber;
		}
	}
}
