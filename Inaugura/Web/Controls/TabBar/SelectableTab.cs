using System;
using System.Drawing;
using System.Collections;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Inaugura.Web.Controls
{
	/// <summary>
	/// Abstract Tab Class
	/// </summary>
	public abstract class SelectableTab : ClickableTab
	{
		public event TabEventHandler Select; // the select event
		public event TabEventHandler UnSelect; // the unselect event

		#region Variables
		private string mSelectedCssClass = string.Empty;
		#endregion

		#region Properties	
		/// <summary>
		/// The selected state of the Tab
		/// </summary>
		[Category("Appearance"), DefaultValue(false)]
		public bool Selected
		{
			get
			{
				if(this.ViewState["Selected"] != null)
					return (bool)this.ViewState["Selected"];
				else
					return false;
			}
			set
			{	
				bool currentState = this.Selected;
				this.ViewState["Selected"] = value;
				if(!currentState && value)
					this.OnSelect();
				else if(currentState && !value)
					this.OnUnSelect();				
			}
		}

		/// <summary>
		/// The CssClass used when the Tab is selected
		/// </summary>
		[Category("Appearance")]
		public string SelectedCssClass
		{
			get
			{
				return this.mSelectedCssClass;
			}
			set
			{
				this.mSelectedCssClass = value;
			}
		}				
		#endregion

		public SelectableTab(string text, string tabID) : base(text, tabID)
		{				
			base.Click += new TabEventHandler(this.TabClicked);
		}

		/// <summary>
		/// Handles any postback data
		/// </summary>
		/// <param name="data">The postback data</param>
		public override void ProcessPostBackCommand(string command)
		{
			if(command == "Select")
				this.Selected = true;
			else if(command == "UnSelect")
				this.Selected = false;

			base.ProcessPostBackCommand(command);
		}


		/// <summary>
		/// Handles the tab clicked event
		/// </summary>
		private void TabClicked(object sender, TabEventArgs e)
		{
			// toggle the selected status of the item
			this.Selected = !this.Selected;
		}		

		/// <summary>
		/// Raises the Select Event
		/// </summary>
		protected void OnSelect()
		{					
				if(this.Select != null)
					this.Select(this,new TabEventArgs((Tab)this));		
		}

		/// <summary>
		/// Raises the UnSelect Event
		/// </summary>
		protected void OnUnSelect()
		{
				if(this.UnSelect != null)
					this.UnSelect(this,new TabEventArgs((Tab)this));
		}

		/// <summary>
		/// Creates a postback string that will cause the client to post back a select event
		/// </summary>
		/// <returns>A postback string</returns>
		protected string GetSelectPostBack()
		{
			return this.GetClientPostBack("Select");
		}

		/// <summary>
		/// Creates a postback string that will cause the client to post back a unselect event
		/// </summary>
		/// <returns>A postback string</returns>
		protected string GetUnSelectPostBack()
		{
			return this.GetClientPostBack("UnSelect");
		}
	}
}
