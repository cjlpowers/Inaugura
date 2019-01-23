#region Using directives
using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

using Inaugura;
using Inaugura.Telephony;
using Inaugura.Telephony.Services;
using Inaugura.Windows.Forms;
#endregion

namespace OrbisSwitch
{

class OrbisSwitchDlg: System.Windows.Forms.Form
{
	private delegate int AddDelegate(TreeNode rn);
	#region Variables
	private MenuStrip menuStrip1;
	private ToolStripMenuItem fileToolStripMenuItem;
	private Panel panel2;
	private TabControl tabControl1;
	private TabPage tabPage1;
	private TabPage tabPage2;
	private SplitContainer splitContainer1;
	private GroupBox groupBox1;
	private TreeView mTreeServices;
	private TabControl tabControl2;
	private TabPage tabPage3;
	private TabPage tabPage4;
	private GroupBox groupBox2;
	private TreeView mTreeTelephony;
	private ImageList imageList1;
	private ImageList imageList2;
	private StatusStrip statusStrip1;
	private ImageList imgLstTelephony;

	private Switch mSwitch;
	private TextBox mTxtLog;
	private ToolStripMenuItem startToolStripMenuItem;
	private ToolStripMenuItem stopToolStripMenuItem;
	private ImageList mImgLstServices;
	private TextBox mTxtError;
	private TreeView treeView1;
	private ToolStripMenuItem toolsToolStripMenuItem;
	private ToolStripMenuItem optionsToolStripMenuItem;
	private SplitContainer splitContainer2;
	private bool mFileCreated = false;

	#endregion

	public Switch Switch
	{
		get
		{
			return this.mSwitch;
		}
		private set
		{
			this.mSwitch = value;
		}
	}

	public OrbisSwitchDlg()
	{
		InitializeComponent();

		try
		{
			// Log events
			Log.LogText += new LogHandler(Log_LogText);
			Log.LogError += new LogHandler(Log_LogError);
			string id = string.Empty;

			if(ConfigurationSettings.AppSettings["SwitchId"] == null)
			{
				throw new ConfigurationException("The configuration file does not contain a switch id");
			}
			else
			{
				id = ConfigurationSettings.AppSettings["SwitchId"];
			}

			// Get the data adaptor
			/*
			OrbisTel.Data.IDataAdaptor data;
			if (ConfigurationSettings.AppSettings["WebServiceUrl"] != null)
				data = new OrbisTel.Data.WebServiceAdaptor(ConfigurationSettings.AppSettings["WebServiceUrl"]);
			else if (ConfigurationSettings.AppSettings["ConnectionString"] != null)
				data = new OrbisTel.Data.DBAdaptor(ConfigurationSettings.AppSettings["ConnectionString"]);
			else
				throw new ConfigurationException("No connection specified in configuration file");


			OrbisTel.Data.DataAdaptor.CurrentDataAdaptor = data;
			this.Switch = data.SwitchStore.GetSwitch(id);

			
			// save the switch config incase we need to start it in the absense of the webservice
			OrbisSoftware.Helper.SaveFileContent("CurrentSwitchConfig.xml", this.Switch.Xml.OuterXml);
			*/

			#region Load the config
			try
			{
				System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
				xmlDoc.Load("Switch.config");
				this.Switch = new Switch(xmlDoc.DocumentElement);		
			}
			catch (Exception ex)
			{
				ExceptionDlg dlg = new ExceptionDlg(ex);
				dlg.ShowDialog(this);
			}
			#endregion

			this.Switch.SwitchStarted += new SwitchHandler(Switch_SwitchStarted);
			this.Switch.TelephonyManager.HardwareCreated += new TelephonyHardwareHandler(TelephonyManager_HardwareCreated);
			this.Switch.TelephonyManager.HardwareDestroyed += new TelephonyHardwareHandler(TelephonyManager_HardwareDestroyed);
			
			this.mTreeTelephony.Nodes.Clear();
			this.mTreeTelephony.Nodes.Add(new TelephonyManagerTreeNode(this.Switch.TelephonyManager));

			this.Switch.ServiceManager.ServiceCreated += new ServiceHandler(ServiceManager_ServiceCreated);
			this.Switch.ServiceManager.ServiceDestroyed += new ServiceHandler(ServiceManager_ServiceDestroyed);

			this.mTreeServices.Nodes.Clear();
			this.mTreeServices.Nodes.Add(new ServiceManagerTreeNode(this.Switch.ServiceManager));
			
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

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
        this.components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrbisSwitchDlg));
        System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Node0");
        this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.panel2 = new System.Windows.Forms.Panel();
        this.tabControl1 = new System.Windows.Forms.TabControl();
        this.tabPage1 = new System.Windows.Forms.TabPage();
        this.mTxtLog = new System.Windows.Forms.TextBox();
        this.tabPage2 = new System.Windows.Forms.TabPage();
        this.mTxtError = new System.Windows.Forms.TextBox();
        this.imageList2 = new System.Windows.Forms.ImageList(this.components);
        this.splitContainer1 = new System.Windows.Forms.SplitContainer();
        this.groupBox2 = new System.Windows.Forms.GroupBox();
        this.tabControl2 = new System.Windows.Forms.TabControl();
        this.tabPage3 = new System.Windows.Forms.TabPage();
        this.mTreeTelephony = new System.Windows.Forms.TreeView();
        this.imgLstTelephony = new System.Windows.Forms.ImageList(this.components);
        this.tabPage4 = new System.Windows.Forms.TabPage();
        this.treeView1 = new System.Windows.Forms.TreeView();
        this.imageList1 = new System.Windows.Forms.ImageList(this.components);
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.mTreeServices = new System.Windows.Forms.TreeView();
        this.mImgLstServices = new System.Windows.Forms.ImageList(this.components);
        this.statusStrip1 = new System.Windows.Forms.StatusStrip();
        this.splitContainer2 = new System.Windows.Forms.SplitContainer();
        this.menuStrip1.SuspendLayout();
        this.panel2.SuspendLayout();
        this.tabControl1.SuspendLayout();
        this.tabPage1.SuspendLayout();
        this.tabPage2.SuspendLayout();
        this.splitContainer1.Panel1.SuspendLayout();
        this.splitContainer1.Panel2.SuspendLayout();
        this.splitContainer1.SuspendLayout();
        this.groupBox2.SuspendLayout();
        this.tabControl2.SuspendLayout();
        this.tabPage3.SuspendLayout();
        this.tabPage4.SuspendLayout();
        this.groupBox1.SuspendLayout();
        this.statusStrip1.SuspendLayout();
        this.splitContainer2.Panel1.SuspendLayout();
        this.splitContainer2.Panel2.SuspendLayout();
        this.splitContainer2.SuspendLayout();
        this.SuspendLayout();
        // 
        // menuStrip1
        // 
        this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
        this.menuStrip1.Location = new System.Drawing.Point(0, 0);
        this.menuStrip1.Name = "menuStrip1";
        this.menuStrip1.Size = new System.Drawing.Size(718, 24);
        this.menuStrip1.TabIndex = 0;
        this.menuStrip1.Text = "menuStrip1";
        // 
        // fileToolStripMenuItem
        // 
        this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.stopToolStripMenuItem});
        this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        this.fileToolStripMenuItem.Text = "File";
        // 
        // startToolStripMenuItem
        // 
        this.startToolStripMenuItem.Name = "startToolStripMenuItem";
        this.startToolStripMenuItem.Text = "Start";
        this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
        // 
        // stopToolStripMenuItem
        // 
        this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
        this.stopToolStripMenuItem.Text = "Stop";
        this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
        // 
        // toolsToolStripMenuItem
        // 
        this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
        this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
        this.toolsToolStripMenuItem.Text = "Tools";
        // 
        // optionsToolStripMenuItem
        // 
        this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
        this.optionsToolStripMenuItem.Text = "Options";
        this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
        // 
        // panel2
        // 
        this.panel2.Controls.Add(this.tabControl1);
        this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panel2.Location = new System.Drawing.Point(0, 0);
        this.panel2.Name = "panel2";
        this.panel2.Size = new System.Drawing.Size(718, 313);
        this.panel2.TabIndex = 3;
        // 
        // tabControl1
        // 
        this.tabControl1.Controls.Add(this.tabPage1);
        this.tabControl1.Controls.Add(this.tabPage2);
        this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tabControl1.ImageList = this.imageList2;
        this.tabControl1.Location = new System.Drawing.Point(0, 0);
        this.tabControl1.Name = "tabControl1";
        this.tabControl1.SelectedIndex = 0;
        this.tabControl1.ShowToolTips = true;
        this.tabControl1.Size = new System.Drawing.Size(718, 313);
        this.tabControl1.TabIndex = 0;
        // 
        // tabPage1
        // 
        this.tabPage1.Controls.Add(this.mTxtLog);
        this.tabPage1.ImageIndex = 0;
        this.tabPage1.Location = new System.Drawing.Point(4, 31);
        this.tabPage1.Name = "tabPage1";
        this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage1.Size = new System.Drawing.Size(710, 278);
        this.tabPage1.TabIndex = 0;
        this.tabPage1.Text = "Log";
        // 
        // mTxtLog
        // 
        this.mTxtLog.Dock = System.Windows.Forms.DockStyle.Fill;
        this.mTxtLog.Font = new System.Drawing.Font("Bitstream Vera Sans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.mTxtLog.ForeColor = System.Drawing.Color.Green;
        this.mTxtLog.Location = new System.Drawing.Point(3, 3);
        this.mTxtLog.Multiline = true;
        this.mTxtLog.Name = "mTxtLog";
        this.mTxtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
        this.mTxtLog.Size = new System.Drawing.Size(704, 272);
        this.mTxtLog.TabIndex = 0;
        this.mTxtLog.WordWrap = false;
        // 
        // tabPage2
        // 
        this.tabPage2.Controls.Add(this.mTxtError);
        this.tabPage2.ImageIndex = 1;
        this.tabPage2.Location = new System.Drawing.Point(4, 31);
        this.tabPage2.Name = "tabPage2";
        this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage2.Size = new System.Drawing.Size(710, 98);
        this.tabPage2.TabIndex = 1;
        this.tabPage2.Text = "Errors";
        // 
        // mTxtError
        // 
        this.mTxtError.Dock = System.Windows.Forms.DockStyle.Fill;
        this.mTxtError.Font = new System.Drawing.Font("Bitstream Vera Sans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.mTxtError.ForeColor = System.Drawing.Color.Red;
        this.mTxtError.Location = new System.Drawing.Point(3, 3);
        this.mTxtError.Multiline = true;
        this.mTxtError.Name = "mTxtError";
        this.mTxtError.ScrollBars = System.Windows.Forms.ScrollBars.Both;
        this.mTxtError.Size = new System.Drawing.Size(704, 92);
        this.mTxtError.TabIndex = 1;
        this.mTxtError.WordWrap = false;
        // 
        // imageList2
        // 
        this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
        this.imageList2.Images.SetKeyName(0, "notebook.png");
        this.imageList2.Images.SetKeyName(1, "error.png");
        // 
        // splitContainer1
        // 
        this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.splitContainer1.Location = new System.Drawing.Point(0, 0);
        this.splitContainer1.Name = "splitContainer1";
        // 
        // splitContainer1.Panel1
        // 
        this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
        // 
        // splitContainer1.Panel2
        // 
        this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
        this.splitContainer1.Size = new System.Drawing.Size(718, 130);
        this.splitContainer1.SplitterDistance = 354;
        this.splitContainer1.TabIndex = 5;
        this.splitContainer1.Text = "splitContainer1";
        // 
        // groupBox2
        // 
        this.groupBox2.Controls.Add(this.tabControl2);
        this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.groupBox2.Location = new System.Drawing.Point(0, 0);
        this.groupBox2.Name = "groupBox2";
        this.groupBox2.Size = new System.Drawing.Size(354, 130);
        this.groupBox2.TabIndex = 1;
        this.groupBox2.TabStop = false;
        this.groupBox2.Text = "Managers";
        // 
        // tabControl2
        // 
        this.tabControl2.Controls.Add(this.tabPage3);
        this.tabControl2.Controls.Add(this.tabPage4);
        this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tabControl2.ImageList = this.imageList1;
        this.tabControl2.Location = new System.Drawing.Point(3, 16);
        this.tabControl2.Name = "tabControl2";
        this.tabControl2.SelectedIndex = 0;
        this.tabControl2.ShowToolTips = true;
        this.tabControl2.Size = new System.Drawing.Size(348, 111);
        this.tabControl2.TabIndex = 0;
        // 
        // tabPage3
        // 
        this.tabPage3.Controls.Add(this.mTreeTelephony);
        this.tabPage3.ImageIndex = 0;
        this.tabPage3.Location = new System.Drawing.Point(4, 31);
        this.tabPage3.Name = "tabPage3";
        this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage3.Size = new System.Drawing.Size(340, 76);
        this.tabPage3.TabIndex = 0;
        this.tabPage3.Text = "Telephony";
        // 
        // mTreeTelephony
        // 
        this.mTreeTelephony.Dock = System.Windows.Forms.DockStyle.Fill;
        this.mTreeTelephony.Font = new System.Drawing.Font("Bitstream Vera Sans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.mTreeTelephony.ImageIndex = 0;
        this.mTreeTelephony.ImageList = this.imgLstTelephony;
        this.mTreeTelephony.Location = new System.Drawing.Point(3, 3);
        this.mTreeTelephony.Name = "mTreeTelephony";
        this.mTreeTelephony.SelectedImageIndex = 0;
        this.mTreeTelephony.Size = new System.Drawing.Size(334, 70);
        this.mTreeTelephony.TabIndex = 0;
        // 
        // imgLstTelephony
        // 
        this.imgLstTelephony.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgLstTelephony.ImageStream")));
        this.imgLstTelephony.Images.SetKeyName(0, "PCI-card.png");
        this.imgLstTelephony.Images.SetKeyName(1, "cpu.png");
        this.imgLstTelephony.Images.SetKeyName(2, "");
        // 
        // tabPage4
        // 
        this.tabPage4.Controls.Add(this.treeView1);
        this.tabPage4.ImageIndex = 1;
        this.tabPage4.Location = new System.Drawing.Point(4, 31);
        this.tabPage4.Name = "tabPage4";
        this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage4.Size = new System.Drawing.Size(226, 260);
        this.tabPage4.TabIndex = 1;
        this.tabPage4.Text = "Media";
        // 
        // treeView1
        // 
        this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.treeView1.Location = new System.Drawing.Point(3, 3);
        this.treeView1.Name = "treeView1";
        treeNode2.Name = "Node0";
        treeNode2.Text = "Node0";
        treeNode2.ToolTipText = "Test Text";
        this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
        this.treeView1.Size = new System.Drawing.Size(220, 254);
        this.treeView1.TabIndex = 1;
        // 
        // imageList1
        // 
        this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
        this.imageList1.Images.SetKeyName(0, "PCI-card.png");
        this.imageList1.Images.SetKeyName(1, "loudspeaker.png");
        // 
        // groupBox1
        // 
        this.groupBox1.Controls.Add(this.mTreeServices);
        this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.groupBox1.Location = new System.Drawing.Point(0, 0);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Size = new System.Drawing.Size(360, 130);
        this.groupBox1.TabIndex = 0;
        this.groupBox1.TabStop = false;
        this.groupBox1.Text = "Services";
        // 
        // mTreeServices
        // 
        this.mTreeServices.Dock = System.Windows.Forms.DockStyle.Fill;
        this.mTreeServices.Font = new System.Drawing.Font("Bitstream Vera Sans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.mTreeServices.ImageIndex = 0;
        this.mTreeServices.ImageList = this.mImgLstServices;
        this.mTreeServices.Location = new System.Drawing.Point(3, 16);
        this.mTreeServices.Name = "mTreeServices";
        this.mTreeServices.SelectedImageIndex = 0;
        this.mTreeServices.Size = new System.Drawing.Size(354, 111);
        this.mTreeServices.TabIndex = 0;
        // 
        // mImgLstServices
        // 
        this.mImgLstServices.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("mImgLstServices.ImageStream")));
        this.mImgLstServices.Images.SetKeyName(0, "cubes_green.png");
        this.mImgLstServices.Images.SetKeyName(1, "cube_green.png");
        this.mImgLstServices.Images.SetKeyName(2, "cpu.png");
        this.mImgLstServices.Images.SetKeyName(3, "cube_green_delete.png");
        this.mImgLstServices.Images.SetKeyName(4, "cube_green_add.png");
        // 
        // statusStrip1
        // 
        this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
        this.statusStrip1.Location = new System.Drawing.Point(0, 471);
        this.statusStrip1.Name = "statusStrip1";
        this.statusStrip1.Size = new System.Drawing.Size(718, 24);
        this.statusStrip1.TabIndex = 6;
        this.statusStrip1.Text = "statusStrip1";

        // 
        // splitContainer2
        // 
        this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
        this.splitContainer2.Location = new System.Drawing.Point(0, 24);
        this.splitContainer2.Name = "splitContainer2";
        this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
        // 
        // splitContainer2.Panel1
        // 
        this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
        // 
        // splitContainer2.Panel2
        // 
        this.splitContainer2.Panel2.Controls.Add(this.panel2);
        this.splitContainer2.Size = new System.Drawing.Size(718, 447);
        this.splitContainer2.SplitterDistance = 130;
        this.splitContainer2.TabIndex = 7;
        // 
        // OrbisSwitchDlg
        // 
        this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
        this.ClientSize = new System.Drawing.Size(718, 495);
        this.Controls.Add(this.splitContainer2);
        this.Controls.Add(this.menuStrip1);
        this.Controls.Add(this.statusStrip1);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "OrbisSwitchDlg";
        this.Text = "OrbisSwitch";
        this.Load += new System.EventHandler(this.OrbisSwitchDlg_Load);
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OrbisSwitchDlg_FormClosing);
        this.menuStrip1.ResumeLayout(false);
        this.panel2.ResumeLayout(false);
        this.tabControl1.ResumeLayout(false);
        this.tabPage1.ResumeLayout(false);
        this.tabPage1.PerformLayout();
        this.tabPage2.ResumeLayout(false);
        this.tabPage2.PerformLayout();
        this.splitContainer1.Panel1.ResumeLayout(false);
        this.splitContainer1.Panel2.ResumeLayout(false);
        this.splitContainer1.ResumeLayout(false);
        this.groupBox2.ResumeLayout(false);
        this.tabControl2.ResumeLayout(false);
        this.tabPage3.ResumeLayout(false);
        this.tabPage4.ResumeLayout(false);
        this.groupBox1.ResumeLayout(false);
        this.statusStrip1.ResumeLayout(false);
        this.splitContainer2.Panel1.ResumeLayout(false);
        this.splitContainer2.Panel2.ResumeLayout(false);
        this.splitContainer2.ResumeLayout(false);
        this.ResumeLayout(false);
        this.PerformLayout();

	}
	#endregion


	#region Log Handlers
	void Log_LogText(object sender, Inaugura.LogEventArgs e)
	{
		object[] pList = { this, e };
		this.BeginInvoke(new EventHandler(this.AddLogText), pList);
	}

	void Log_LogError(object sender, Inaugura.LogEventArgs e)
	{		
		object[] pList = { this, e };
		this.BeginInvoke(new EventHandler(this.AddErrorText), pList);
	}

	private void AddLogText(object obj, EventArgs e)
	{
		Inaugura.LogEventArgs args = (Inaugura.LogEventArgs)e;
		string currentText = this.mTxtLog.Text;
		string logItem = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss - ") + args.Text;
		this.LogToFile(logItem);
		logItem += "\r\n";		
		currentText += logItem;		
		this.mTxtLog.Text = currentText;		
		this.mTxtLog.SelectionStart = this.mTxtLog.Text.Length;
		this.mTxtLog.ScrollToCaret();
	}

	private void AddErrorText(object obj, EventArgs e)
	{
		Inaugura.LogEventArgs args = (Inaugura.LogEventArgs)e;
		string currentText = this.mTxtError.Text;
		string logItem = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss - ") + args.Text;
		this.LogToFile(logItem);
		logItem += "\r\n";		
		currentText += logItem;		
		this.mTxtError.Text = currentText;
	}
	#endregion


	private void OrbisSwitchDlg_FormClosing(object sender, FormClosingEventArgs e)
	{
		if(this.Switch != null && this.Switch.Started)
		{
			e.Cancel = true;
			this.Switch.SwitchStopped += new SwitchHandler(Switch_SwitchStopped);
			Thread t = new Thread(this.RunSwitchStop);
			t.Start();
		}		
	}

	private void RunSwitchStop()
	{
		this.Switch.Stop();
	}

	private TreeNode GetHardwareTreeNode(TelephonyHardware hardware)
	{
		lock (this.mTreeTelephony)
		{
			foreach (TreeNode node in this.mTreeTelephony.Nodes[0].Nodes)
			{
				if (node.Tag == hardware)
				{
					return node;
				}
			}
			return null;
		}
	}

	private TreeNode GetServiceTreeNode(Service service)
	{
		lock (this.mTreeServices)
		{
			foreach (TreeNode node in this.mTreeServices.Nodes[0].Nodes)
			{
				if (node.Tag == service)
				{
					return node;
				}
			}
			return null;
		}
	}

	private TreeNode GetServiceLineTreeNode(ServiceLine serviceLine)
	{
		lock (this.mTreeServices)
		{
			TreeNode serviceNode = this.GetServiceTreeNode(serviceLine.Service);
			if (serviceNode != null)
			{
				foreach (TreeNode node in serviceNode.Nodes)
				{
					if (node.Tag == serviceLine)
						return node;
				}
			}			
			return null;
		}
	}

	private void startToolStripMenuItem_Click(object sender, EventArgs e)
	{
		this.StartSwitch();
	}

	private void stopToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (this.Switch.Started)
		{			
			Thread t = new Thread(this.RunSwitchStop);
			t.Start();
		}
	}

	private void OrbisSwitchDlg_Load(object sender, EventArgs e)
	{
		this.StartSwitch();
	}

	private void StartSwitch()
	{		
		this.Switch.Start();
	}

	private void LogToFile(string text)
	{
		lock (this)
		{
			System.IO.StreamWriter sw;
			if (!mFileCreated)
			{
				sw = System.IO.File.CreateText("Log.txt");
				sw.WriteLine("OrbisSwitch Log: " + DateTime.Now.ToString("G"));
				this.mFileCreated = true;
			}
			else
				sw = System.IO.File.AppendText("Log.txt");

			sw.WriteLine(text);

			sw.Close();
		}
	}

	private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (!this.Switch.Started)
		{
			ConfigItemDlg dlg = new ConfigItemDlg(this.Switch);
			dlg.StartPosition = FormStartPosition.CenterScreen;
			if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.Switch.Xml.OwnerDocument.Save("switch.config");
			}
		}
		else
		{
			MessageBox.Show("The switch must be stopped first");
		}
	}

	void TelephonyManager_HardwareCreated(object sender, TelephonyHardwareEventArgs e)
	{
		lock (this.mTreeTelephony)
		{
			TelephonyHardwareTreeNode node = new TelephonyHardwareTreeNode(e.Hardware);
			this.mTreeTelephony.Nodes[0].Nodes.Add(node);
			e.Hardware.LineCreated += new TelephonyLineHandler(hardware_LineCreated);
		}
	}

	void TelephonyManager_HardwareDestroyed(object sender, TelephonyHardwareEventArgs e)
	{
		lock (this.mTreeTelephony)
		{			
			TreeNode node = this.GetHardwareTreeNode(e.Hardware);
			if (node != null)
				node.Remove();
		}
	}

	private void hardware_LineCreated(object sender, TelephonyLineEventArgs e)
	{
		lock (this.mTreeTelephony)
		{
			TelephonyLineTreeNode node = new TelephonyLineTreeNode(e.Line);
			TreeNode hardwareNode = this.GetHardwareTreeNode(e.Line.Hardware);
			hardwareNode.Nodes.Add(node);
		}
	}

	void Switch_SwitchStarted(object sender, SwitchEventArgs e)
	{
		this.mTreeTelephony.ExpandAll();
		this.mTreeServices.ExpandAll();
	}


	void ServiceManager_ServiceCreated(object sender, ServiceEventArgs e)
	{
		lock (this.mTreeServices)
		{
			ServiceTreeNode node = new ServiceTreeNode(e.Service);
			this.mTreeServices.Nodes[0].Nodes.Add(node);

			e.Service.ServiceLineAdded += new ServiceLineHandler(Service_ServiceLineAdded);
			e.Service.ServiceLineRemoved += new ServiceLineHandler(service_ServiceLineRemoved);
		}
	}

	void ServiceManager_ServiceDestroyed(object sender, ServiceEventArgs e)
	{
		lock (this.mTreeServices)
		{
			TreeNode node = this.GetServiceTreeNode(e.Service);
			if (node != null)
				node.Remove();
		}
	}

	void Switch_SwitchStopped(object sender, SwitchEventArgs e)
	{
		this.Close();
	}

	void Service_ServiceLineAdded(object sender, ServiceLineEventArgs e)
	{
		TreeNode node = this.GetServiceTreeNode(e.ServiceLine.Service);
		if (node != null)
		{
			ServiceLineTreeNode sltn = new ServiceLineTreeNode(e.ServiceLine);
			AddDelegate addDelegate = new AddDelegate(node.Nodes.Add);
			this.mTreeServices.Invoke(addDelegate, new object[] { sltn });
		}
	}

	void service_ServiceLineRemoved(object sender, ServiceLineEventArgs e)
	{
		TreeNode node = this.GetServiceLineTreeNode(e.ServiceLine);
		if (node != null)
		{
			node.Remove();
		}
	}
}
}