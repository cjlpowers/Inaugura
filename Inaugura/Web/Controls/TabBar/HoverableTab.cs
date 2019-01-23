using System;
using System.Drawing;
using System.Collections;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Inaugura.Web.Controls
{
	/// <summary>
	/// Abstract tab class containing members which enable hovering capabilities
	/// </summary>
	public abstract class HoverableTab : TabBase
	{
		#region Events		
		[Category("Appearance")] 
		public event TabEventHandler MouseOver;
		[Category("Appearance")] 
		public event TabEventHandler MouseOut;
		#endregion 

		#region Variables
		private string mMouseOverCssClass = string.Empty;
		#endregion

		#region Properties	
		/// <summary>
		/// The css class used when the mouse is over the control
		/// </summary>
		[Category("Appearance")]
		public string MouseOverCssClass
		{
			get
			{
				return this.mMouseOverCssClass;
			}
			set
			{
				this.mMouseOverCssClass = value;
			}			
		}
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="text">The tab text</param>
		public HoverableTab(string text, string tabID) : base(text, tabID)
		{				
		}		

		/// <summary>
		/// Handles any postback data
		/// </summary>
		/// <param name="data">The postback data</param>
		public override void ProcessPostBackCommand(string command)
		{
			if(command == "MouseOver")
				this.OnMouseOver();
			else if(command == "MouseOut")
				this.OnMouseOut();

			base.ProcessPostBackCommand(command);
		}

		/// <summary>
		/// Raises the mouse over event
		/// </summary>
		protected virtual void OnMouseOver()
		{
			if(this.MouseOver != null)
				this.MouseOver(this,new TabEventArgs((Tab)this));
		}

		/// <summary>
		/// Raises the moust out event
		/// </summary>
		protected virtual void OnMouseOut()
		{
			if(this.MouseOut != null)
				this.MouseOut(this,new TabEventArgs((Tab)this));
		}

		/// <summary>
		/// Creates a postback string that will cause the client to post back a hover event
		/// </summary>
		/// <returns>A postback string</returns>
		protected string GetMouseOverPostBack()
		{
			return this.GetClientPostBack("MouseOver");
		}

		/// <summary>
		/// Creates a postback string that will cause the client to post back mouse out event
		/// </summary>
		/// <returns>A postback string</returns>
		protected string GetMouseOutPostBack()
		{
			return this.GetClientPostBack("MouseOut");
		}


	}
}
