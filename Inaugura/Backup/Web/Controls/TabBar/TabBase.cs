using System;
using System.Drawing;
using System.Collections;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Inaugura.Web.Controls
{
	/// <summary>
	/// Abstract Tab Base Class
	/// </summary>
	public abstract class TabBase : WebControl
	{
		#region Variables
		private string mCssClass = string.Empty;
		private string mMouseOverCssClass = string.Empty;
		private string mSelectedCssClass = string.Empty;
		private string mDisabledCssClass = string.Empty;
		private string mOnClickValue = string.Empty;
		private TabBar mTabBar;
		private bool mWrapText = true;
		private string mText = string.Empty;
		#endregion

		#region Properties	
		/// <summary>
		/// The parrent Tab bar
		/// </summary>
		[Browsable(false),
		DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)
		]
		internal TabBar TabBar
		{
			get
			{
				return this.mTabBar;
			}
			set
			{
				this.mTabBar = value;
			}
		}

		/// <summary>
		/// The Tab text
		/// </summary>
		[Category("Appearance")]
		public string Text
		{
			get
			{
				return this.mText;
			}
			set
			{
				this.mText = value;
			}
		}

		/*
		/// <summary>
		/// The Tab text
		/// </summary>
		public string Text
		{
			get
			{
				if(this.ViewState["Text"] == null)
					return string.Empty;
				return this.ViewState["Text"] as string;
			}
			set
			{
				this.ViewState["Text"] = value;
			}
		}
		*/

		/// <summary>
		/// Enables/Disables the wrapping of tab text
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
		
		/// <summary>
		/// The CssClass used when the Tab is disabled
		/// </summary>
		[Category("Appearance")]
		public string DisabledCssClass
		{
			get
			{
				return this.mDisabledCssClass;
			}
			set
			{
				this.mDisabledCssClass = value;
			}
		}

		/// <summary>
		/// The primary css class to style the Tab (the css class used when void of mouse interaction)
		/// </summary>
		[Category("Appearance")]
		public override string CssClass
		{
			get
			{
				if(this.Enabled)
					return this.mCssClass;					
				else
					return this.DisabledCssClass;
			}
			set
			{
				this.mCssClass = value;
			}
		}	
		#endregion

		public TabBase(string text, string tabID)
		{	
			this.ID = tabID;
			this.Attributes.Clear();				
			this.Text = text;
			this.PreRender += new EventHandler(this.Control_PreRender);
		}

		protected override void CreateChildControls()
		{
			this.BorderStyle = BorderStyle.None;
			this.Controls.Clear();
			this.Attributes.Clear();
			if(this.Visible)
			{	
				// Create the layoyt
				Table table = new Table();
				table.EnableViewState = false;
				this.Controls.Add(table);
				table.Attributes.Clear();
				table.Attributes.Add("width","100%");
				table.Attributes.Add("height","100%");
				table.Attributes.Add("cellpadding","0");
				table.Attributes.Add("cellspacing","0");
				table.BorderStyle = BorderStyle.None;

				TableRow row = new TableRow();
				TableCell cell = new TableCell();
				if(this.TabBar != null)
					cell.Wrap = this.TabBar.WrapText;
				cell.Text = this.Text;
				row.Cells.Add(cell);
				table.Rows.Add(row);                

				cell.Attributes.Clear();

				// set the styles
				if(this.CssClass != string.Empty)	
					cell.CssClass = this.CssClass;
			}				
		}

		public virtual void Control_PreRender(object sender, EventArgs args)
		{
		}

		/// <summary>
		/// Handles any postback data
		/// </summary>
		/// <param name="data">The postback data</param>
		public virtual void ProcessPostBackCommand(string data)
		{
		}

		
		/// <summary>
		/// Converts a color to the hex color string
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public static string ColorValue(Color color)
		{			
			return "#"+color.ToArgb().ToString("X").Substring(2);
		}

		/// <summary>
		/// Creates a post back for a click event
		/// </summary>
		/// <returns></returns>
		protected string GetClientPostBack(string command)
		{
			if(this.TabBar != null)
				return this.Page.ClientScript.GetPostBackClientHyperlink(this.TabBar,command+"_"+this.UniqueID);
			else
				return null;
		}

		protected static bool IsHandlerRegistered(Delegate del, Delegate handler)
		{
			if(del == null)
				return false;
			foreach(Delegate registeredDelegate in del.GetInvocationList())
			{
				if(registeredDelegate.Method.GetHashCode() == handler.Method.GetHashCode())
					return true;			
			}
			return false;
		}
	
	}
}
