using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;

namespace Inaugura
{
	/// <summary>
	/// Summary description for FilterListDlg.
	/// </summary>
	internal class DetailsDlg : System.Windows.Forms.Form
	{
		class DetailViewItem : ListViewItem
		{			
			private string mValue = string.Empty;

			public string Value
			{
				get
				{
					return this.mValue;
				}
				set
				{
					this.mValue = value;
				}
			}
			public DetailViewItem(string key, string value) : base(key)
			{
				this.mValue = value;
			}
		}

		private Details mDetails;
		private ListView listView1;
		private ColumnHeader key;
		private ColumnHeader value;
		private ContextMenu contextMenu2;
		private MenuItem menuItem1;
		private MenuItem menuItem2;
		private MenuItem menuItem3;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DetailsDlg(Details details)
		{
			this.mDetails = details;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();			
			
			IDictionaryEnumerator enumerator = this.mDetails.GetEnumerator();

			while(enumerator.MoveNext())
			{
				DetailViewItem dvi = new DetailViewItem((string)enumerator.Key, (string)enumerator.Value);
				this.listView1.Items.Add(dvi);
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
			this.listView1 = new System.Windows.Forms.ListView();
			this.key = new System.Windows.Forms.ColumnHeader("");
			this.value = new System.Windows.Forms.ColumnHeader("");
			this.contextMenu2 = new System.Windows.Forms.ContextMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
// 
// listView1
// 
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.key,
            this.value});
			this.listView1.ContextMenu = this.contextMenu2;
			this.listView1.Location = new System.Drawing.Point(13, 16);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(442, 372);
			this.listView1.TabIndex = 1;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
// 
// key
// 
			this.key.Text = "Key";
			this.key.Width = 200;
// 
// value
// 
			this.value.Text = "Value";
			this.value.Width = 200;
// 
// contextMenu2
// 
			this.contextMenu2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2,
            this.menuItem3});
			this.contextMenu2.Name = "contextMenu2";
// 
// menuItem1
// 
			this.menuItem1.Index = 0;
			this.menuItem1.Name = "menuItem1";
			this.menuItem1.Text = "New";
			this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
// 
// menuItem2
// 
			this.menuItem2.Index = 1;
			this.menuItem2.Name = "menuItem2";
			this.menuItem2.Text = "Delete";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
// 
// menuItem3
// 
			this.menuItem3.Index = 2;
			this.menuItem3.Name = "menuItem3";
			this.menuItem3.Text = "Edit";
			this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
// 
// DetailsDlg
// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(467, 400);
			this.Controls.Add(this.listView1);
			this.Name = "DetailsDlg";
			this.Text = "Details Editor";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.StringListDlg_Closing);
			this.ResumeLayout(false);

		}
		#endregion

		
		private void StringListDlg_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.mDetails.Clear();
			foreach(DetailViewItem i in this.listView1.Items)
			{
				this.mDetails.Add(i.Text, i.Value);
			}
		}

		private void menuItem1_Click(object sender, EventArgs e)
		{
			DetailDlg dlg = new DetailDlg("key", "value");
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				if (this.mDetails.ContainsKey(dlg.Key))
				{
					MessageBox.Show("Key already exists");
					return;
				}

				DetailViewItem item = new DetailViewItem(dlg.Key, dlg.Value);
				this.listView1.Items.Add(item);
			}
		}
		private void menuItem2_Click(object sender, EventArgs e)
		{
			if (this.listView1.SelectedItems.Count != 0)
			{
				if (MessageBox.Show("Are you sure you want to delete the item?", "Delete Confirmation",MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					this.listView1.Items.RemoveAt(this.listView1.SelectedIndices[0]);
				}
			}
		}

		private void menuItem3_Click(object sender, EventArgs e)
		{
			if (this.listView1.SelectedItems.Count != 0)
			{	
				DetailViewItem item = (DetailViewItem)this.listView1.SelectedItems[0];
				DetailDlg dlg = new DetailDlg(item.Text, item.Value);
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					item.Value = dlg.Value;
					item.Text = dlg.Key;
				}
			}
		}

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count != null)
            {
                DetailViewItem item = (DetailViewItem)this.listView1.SelectedItems[0];
                DetailDlg dlg = new DetailDlg(item.Text, item.Value);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    item.Value = dlg.Value;
                    item.Text = dlg.Key;
                }
            }

        }

	}
}
