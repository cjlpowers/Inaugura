using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Inaugura.Web.Controls
{

	/// <summary>
	/// Event arguments for tab notifications
	/// </summary>
	public class TabEventArgs : EventArgs
	{
		private Tab mTab;

		/// <summary>
		/// The tab object
		/// </summary>
		public Tab Tab
		{
			get
			{
				return this.mTab;
			}
		}

		/// <summary>
		/// Constuctor
		/// </summary>
		/// <param name="tab">The tab which is the focus of the event</param>
		public TabEventArgs(Tab tab)
		{
			this.mTab = tab;
		}
	}


	public delegate void TabEventHandler(object sender, TabEventArgs args);

	/// <summary>
	/// A generic tab which draws a box shaped tab and populates onclick,onclose,onmouseover and onmouseout events with specified values
	/// </summary>
	public class Tab : CloseableTab
	{
		#region Variables
		private bool mCloseable = false;
		private bool mSelectable = true;
		#endregion

		#region Properties

		/// <summary>
		/// Enables/Disables the closable nature of the control
		/// </summary>
		[Category("Behavior"), DefaultValue(false)] 
		public bool Closeable
		{
			get
			{
				return this.mCloseable;
			}
			set
			{
				this.mCloseable = value;
			}
		}

		/// <summary>
		/// Enables/Disables the selectable nature of the tab
		/// </summary>
		[Category("Behavior"), DefaultValue(true)]
		public bool Selectable
		{
			get
			{
				return this.mSelectable;
			}
			set
			{
				this.mSelectable = value;
			}
		}

		/// <summary>
		/// The html used for the tab click event
		/// </summary>		
		protected virtual string HtmlOnClick
		{
			get
			{
				if(this.Selectable && !this.Selected)
					return this.GetClickPostBack();
				else
					return null;
			}			
		}

		/// <summary>
		/// The html used for the tab close event
		/// </summary>
		protected virtual string HtmlOnClose
		{
			get
			{
				if(this.Closeable)
					return this.GetClosePostBack();
				else
					return null;
			}			
		}

		/// <summary>
		/// The html used for the tab mouse over event
		/// </summary>
		protected virtual string HtmlOnMouseOver
		{
			get
			{
				if(!this.Selected && this.Enabled)
					return "this.className='"+this.MouseOverCssClass+"';";
				else
					return null;
			}		
		}

		/// <summary>
		///  The html used for the mouse over event
		/// </summary>
		protected string HtmlOnMouseOut
		{
			get
			{
				if(!this.Selected && this.Enabled)			
					return  "this.className='"+this.CssClass+"';";
				else
					return null;
			}			
		}	
		#endregion

		public Tab() : base(string.Empty,string.Empty)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="text">The Tab text</param>
		public Tab(string text, string tabID) : base(text, tabID)
		{
			this.CssClass = "Button";
			this.SelectedCssClass = "ButtonSelected";
			this.MouseOverCssClass = "ButtonMouseOver";
			this.DisabledCssClass = "ButtonDisabled";
		}		

		protected override void CreateChildControls()
		{			
			string activeCss = this.CssClass;

			if(!this.Enabled)
				activeCss = this.DisabledCssClass;
			else if(this.Selected)
				activeCss = this.SelectedCssClass;

			this.Controls.Clear();
			this.Attributes.Clear();
			if(this.Visible)
			{	
				// Create the layoyt
				Table table = new Table();
				table.EnableViewState = false;
				this.Controls.Add(table);
				table.Attributes.Clear();
				//table.Style.Add("height","100%");
				table.Attributes.Add("width","100%");
				table.Attributes.Add("height","100%");
				table.Attributes.Add("cellpadding","0");
				table.Attributes.Add("cellspacing","0");

				if(this.ToolTip != null && this.ToolTip != string.Empty)
					table.ToolTip = this.ToolTip;

                TableRow row = new TableRow();
				//row.Style.Add("height","100%");
				TableCell cell = new TableCell();
				
				cell.CssClass = activeCss;				// set the class to carry over the font styles
				//cell.Style.Add("height","100%");
				cell.Text = this.Text;
				cell.Wrap = this.WrapText;

				// make the cell pritty much invisible
				cell.BackColor = System.Drawing.Color.Transparent;
				cell.BorderStyle = BorderStyle.None;
				cell.VerticalAlign = VerticalAlign.Middle;
				row.Cells.Add(cell);

				// if this tab should contain a close box we need to add a 
				// new cell
				if(this.Closeable && this.CloseTabImageUrl != string.Empty)
				{
					cell = new TableCell();
					row.Cells.Add(cell);
					cell.VerticalAlign = VerticalAlign.Top;
					Div div = new Div();
					cell.Controls.Add(div);
					div.Attributes.Add("align","right");
					div.Style.Add("left","-2px");
					div.Style.Add("top","2px");				
					div.Style.Add("position","relative");

					Image img = new Image();
					img.ToolTip = "Close";
					img.Enabled = this.Enabled;
					img.EnableViewState = false;
					img.ImageUrl = this.CloseTabImageUrl;
					if(this.Enabled)
					{						
						img.Style.Add("cursor","hand");												
						// add the post back command to the image
						if(this.HtmlOnClose != string.Empty)
							img.Attributes.Add("onclick",this.HtmlOnClose);										
					}
					div.Controls.Add(img);
				}				

				table.Rows.Add(row);                
				
				// set the styles
				if(activeCss != string.Empty)	
					table.CssClass = activeCss;

				if(this.Enabled && !this.Selected)
				{
					//img.Attributes.Add("onclick",Page.GetPostBackClientHyperlink(this.TabBar,"Close_"+this.UniqueID)+"; window.event.cancelBubble=true");

					// this.OnClickValue= Page.GetPostBackClientHyperlink(this.TabBar,"Click_"+this.UniqueID);
					// table.Attributes.Add("onMouseover","this.className='"+this.MouseOverCssClass+"';");				

					// table.Attributes.Add("onMouseout","this.className='"+this.CssClass+"';");				

					if(this.HtmlOnClick != null && this.HtmlOnClick != string.Empty)
						table.Attributes.Add("onclick",this.HtmlOnClick);

					if(this.HtmlOnMouseOver != null && this.HtmlOnMouseOver != string.Empty)
						table.Attributes.Add("onmouseover",this.HtmlOnMouseOver);

					if(this.HtmlOnMouseOut != null&& this.HtmlOnMouseOut != string.Empty  )
						table.Attributes.Add("onmouseout",this.HtmlOnMouseOut);					
				}						
			}								
		}

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

		/*
		public override void Control_PreRender(object sender, EventArgs args)
		{
		}
		*/
	}
}
