using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;

namespace Inaugura.Web.Controls
{
	/// <summary>
	/// Summary description for TabBar.
	/// </summary>
	[	Designer(typeof(TabBarDesigner)),
		ControlBuilder(typeof(TabBarBuilder)),
		PersistChildren(false),
		ParseChildren(false),
		DefaultProperty("Text"), 
		ToolboxData("<{0}:TabBar runat=server></{0}:TabBar>")]
	public class TabBar : System.Web.UI.WebControls.WebControl, IPostBackEventHandler
	{
		public enum TabBarDirection
		{
			Accross,
			Down
		}

		[Category("Action")] 
		public event TabEventHandler TabClick;		// Defines a Tab Click event
		[Category("Action")] 
		public event TabEventHandler TabClose;		// Defines a Tab Close event
		[Category("Action")] 
		public event TabEventHandler TabSelect;	// Defines a Tab Select event
		[Category("Action")] 
		public event TabEventHandler TabUnSelect;	// Defines a Tab Unselect event

		#region Variables
		private TabCollection mTabCollection;
		private bool mCompressTabs = false;
		private bool mWrapText = false;
		private TabBarDirection mDirection = TabBarDirection.Accross;
		private int mMaxTabsPerLine = 10;
		private TabEventHandler mTabClick;
		private TabEventHandler mTabClose;
		private TabEventHandler mTabSelect;
		private TabEventHandler mTabUnSelect;
		#endregion

		#region Properties

		/// <summary>
		/// Enables/Disables the tab bar
		/// </summary>
		public override bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				base.Enabled = value;
				foreach(Tab tab in this.Tabs)
					tab.Enabled = value;
			}
		}

		public Inaugura.Web.Controls.Tab this [string id]
		{
			get
			{
				return this.GetTabById(id);
			}			
		}
		
		[	PersistenceMode(PersistenceMode.InnerProperty),
        	MergableProperty(false),
			Browsable(true) ]
		public Inaugura.Web.Controls.TabCollection Tabs
		{
			get
			{
				return this.mTabCollection;
			}
		}
	

		public Tab SelectedTab
		{
			get
			{
				foreach(Tab tab in this.Tabs)
				{
					if(tab.Selected)
						return tab;
				}
				return null;
			}
			set
			{
				foreach(Tab tab in this.Tabs)
				{
					if(tab.Selected && value != tab)
						tab.Selected = false;
					else if(tab == value && !tab.Selected)
						tab.Selected = true;
				}
			}
		}

		/// <summary>
		/// If true the control attempts to make Tabs as small as possible
		/// </summary>
		[Category("Appearance"), DefaultValue(false)] 
		public bool CompressTabs
		{
			get
			{
				return this.mCompressTabs;				
			}
			set
			{
				this.mCompressTabs = value;
			}
		}			

		/// <summary>
		/// If true Tabs text will be wrapped onto multiple lines
		/// </summary>
		[Category("Appearance"), DefaultValue(false)] 
		public bool WrapText
		{
			get
			{
				return this.mWrapText;
			}
			set
			{
				this.mWrapText = value;
			}
		}

		[Category("Appearance"), DefaultValue(TabBarDirection.Accross)]
		public TabBarDirection Direction
		{
			get
			{
				return this.mDirection;
			}
			set
			{
				this.mDirection = value;
			}
		}			

		/// <summary>
		/// The maxiumum number of tabs appearing in any one colunm or row
		/// </summary>
		[Category("Appearance"), DefaultValue(10)] 
		public int MaxTabsPerLine
		{
			get
			{
				return this.mMaxTabsPerLine;
			}
			set
			{
				this.mMaxTabsPerLine = value;
			}
		}			
		#endregion		

		public TabBar()
		{
			this.mTabCollection = new TabCollection();

			this.mTabClick = new TabEventHandler(this.Tab_Click);
			this.mTabClose = new TabEventHandler(this.Tab_Close);
			this.mTabSelect = new TabEventHandler(this.Tab_Select);
			this.mTabUnSelect = new TabEventHandler(this.Tab_UnSelect);

			this.mTabCollection.TabAdded += new TabEventHandler(this.TabCollection_TabAdded);
			this.mTabCollection.TabRemoved += new TabEventHandler(this.TabCollection_TabRemoved);
		}
		
		#region Rendering

		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			// dont write a begintag
			return;
		}

		public override void RenderEndTag(HtmlTextWriter writer)
		{
			// dont write a endtag
			return;
		}

		protected override void CreateChildControls()
		{
			// insert all the 
			this.Attributes.Clear();
			this.Controls.Clear();
			
			#region Drawing Accross
			if(this.Direction == TabBarDirection.Accross)
			{				
				ArrayList rows = new ArrayList();
				ArrayList rowOfTabs = null;
				
				for(int i =0; i < Tabs.Count; i++)
				{
					if(i % this.MaxTabsPerLine == 0) // we need a new row
					{
						if(rowOfTabs != null)
							rows.Add(rowOfTabs);
						rowOfTabs = new ArrayList();					
					}
					rowOfTabs.Add(Tabs[i]);					
				}
				// add the last row of Tabs (if there were any Tabs at all)
				if(rowOfTabs != null)
					rows.Add(rowOfTabs);

				// move any row with selected Tabs to the very end
				for(int i =0; i < rows.Count; i++)
				{
					ArrayList currentRow = rows[i] as ArrayList;

					bool selectedTabFound = false;
					foreach(Tab btn in currentRow)
					{
						if(btn.Selected)
						{
							selectedTabFound = true;
							break;
						}
					}

					if(selectedTabFound)
					{
						// do a row swap
						ArrayList selectedTabRow = rows[i] as ArrayList;
						rows[i] = rows[rows.Count-1] as ArrayList;
						rows[rows.Count-1] = selectedTabRow;
					}
				}
				
				// the rows are sorted so draw them
				foreach(ArrayList list in rows)
				{
					Table table = new Table();
					table.EnableViewState = false;
					table.CssClass = this.CssClass;
					table.Attributes.Add("width","100%");
					//table.Attributes.Add("height","100%");
					table.Attributes.Add("cellpadding","0");
					table.Attributes.Add("cellspacing","0");
					Tab[] TabsToDraw = TabBar.ConvertListToTabArray(list as ArrayList);
					TableRow tableRow = new TableRow();
					this.RenderTabRow(tableRow,TabsToDraw);
					table.Rows.Add(tableRow);
					this.Controls.Add(table);
				}				
			}
			#endregion
			#region Drawing Down
			else
			{
				throw new NotImplementedException();
			}
			#endregion
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{		
			output.WriteLine();
			output.WriteLineNoTabs("<!-- START OF TAB BAR -->");
			// Create the child controls again because the selected Tab
			// could have changed due to postback data
			this.CreateChildControls();
			base.Render(output);
			output.WriteLineNoTabs("<!-- END OF TAB BAR -->");
			output.WriteLine();
		}	

		private void RenderTabRow(TableRow row, Tab[] Tabs)
		{
			int percent = 100/this.MaxTabsPerLine;

			foreach(Tab btn in Tabs)
			{
				TableCell cell = new TableCell();
				cell.Attributes.Add("width",percent.ToString()+"%");

				cell.Controls.Add(btn);						
				row.Cells.Add(cell);
			}

			if(this.CompressTabs && Tabs.Length < this.MaxTabsPerLine)
			{
				// insert a place holder Tab to 'compress' the rest of the Tabs down to size
				TableCell cell = new TableCell();
				cell.Attributes.Add("width","100%");
				row.Cells.Add(cell);
				
				/*
					PlaceHolderTab btn = new PlaceHolderTab();
					TableCell cell = new TableCell();
					cell.Attributes.Add("width","100%");
					cell.Controls.Add(btn);	
					row.Cells.Add(cell);
					*/				
			}

		}


		#endregion

		#region Tab Management

		private void TabCollection_TabAdded(object sender, TabEventArgs args)
		{
			args.Tab.TabBar = this;
			args.Tab.Click += this.mTabClick;
			args.Tab.Select += this.mTabSelect;
			args.Tab.UnSelect += this.mTabUnSelect;
			args.Tab.Close += this.mTabClose;
		}

		private void TabCollection_TabRemoved(object sender, TabEventArgs args)
		{
			args.Tab.Click -= this.mTabClick;
			args.Tab.Select -= this.mTabSelect;
			args.Tab.UnSelect -= this.mTabUnSelect;
			args.Tab.Close -= this.mTabClose;
		}
		
		private Tab GetTabById(string id)
		{
			foreach(Tab btn in this.Tabs)
			{
				if(btn.UniqueID == id)
					return btn;
			}
			return null;
		}

		#endregion
				
		#region Event Notification

		// Implement the RaisePostBackEvent method from the
		// IPostBackEventHandler interface. 
		public void RaisePostBackEvent(string eventArgument)
		{
			this.EnsureChildControls();

			string command = eventArgument.Substring(0,eventArgument.IndexOf("_"));
			string Tab = eventArgument.Substring(eventArgument.IndexOf("_")+1);

			Tab btn = this.GetTabById(Tab);
			if(btn == null)
				throw new NullReferenceException("The button "+Tab+" could not be found");
			btn.ProcessPostBackCommand(command);		
		}	

		#region Event Handlers

		private void Tab_Click(object sender, TabEventArgs args)
		{
			this.OnTabClick(args.Tab);
		}

		private void Tab_Close(object sender, TabEventArgs args)
		{
			this.OnTabClose(args.Tab);
		}

		private void Tab_Select(object sender, TabEventArgs args)
		{
			this.OnTabSelect(args.Tab);
		}

		private void Tab_UnSelect(object sender, TabEventArgs args)
		{
			this.OnTabUnSelect(args.Tab);
		}

		protected void OnTabSelect(Tab tab)
		{			
			// unselect any previously selected items
			foreach(Tab t in this.Tabs)
			{
				if(t.Selected && t != tab)
					t.Selected = false;
			}			

			if(this.TabSelect != null)
				this.TabSelect(this,new TabEventArgs(tab));
		}

		protected void OnTabUnSelect(Tab tab)
		{
			if(this.TabUnSelect != null)
				this.TabUnSelect(this,new TabEventArgs(tab));
		}

		protected void OnTabClick(Tab tab)
		{
			if(this.TabClick != null)
				this.TabClick(this,new TabEventArgs(tab));	
		}

		protected void OnTabClose(Tab tab)
		{
			// remove the item from the tab bar
			this.Tabs.Remove(tab);

			if(this.TabClose != null)
				this.TabClose(this,new TabEventArgs(tab));	
		}		
		#endregion

		#endregion

		#region Helper Methods		
		private static Tab[] ConvertListToTabArray(ArrayList Tabs)
		{
			Tab[] TabArray = new Tab[Tabs.Count];
			for(int i =0; i < Tabs.Count; i++)
			{
				TabArray[i] = Tabs[i] as Tab;
			}

			return TabArray;
		}
		#endregion

		#region View State
		protected override void LoadViewState(object savedState) 
		{
			if (savedState != null) 
			{
				object[] myState = (object[])savedState;

				if (myState[0] != null)
					base.LoadViewState(myState[0]);
				if (myState[1] != null)
				{
					string selectedTab = myState[1] as string;
					foreach(Tab tab in this.Tabs)
					{
						if(tab.ID == selectedTab)
						{
							tab.Selected = true;
							break;
						}
					}				
				}
			
			}


			if(savedState != null)
			{
				
			}
		}		

		protected override object SaveViewState() 
		{
			object[] state = new object[2];
			object baseState = base.SaveViewState();
			object controlState = null;

			if(this.SelectedTab != null)
				controlState =  this.SelectedTab.ID;
					
			state[0] = baseState;
			state[1] = controlState;
			return state;
		}
		#endregion

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
		}

		protected override void AddParsedSubObject(object obj)
		{
			if (obj is Tab)
			{
				Tab tab = obj as Tab;
				this.Tabs.Add(tab);				
			}
			else
				base.AddParsedSubObject (obj);
		}



	}
}
