using System;
using System.Drawing;
using System.Collections;
using System.Web.UI.WebControls;

namespace Inaugura.Web.Controls
{
	/// <summary>
	/// Abstract tab class containing members which enable clicking capabilities
	/// </summary>
	public abstract class ClickableTab : HoverableTab
	{
		public event TabEventHandler Click; // the click event
	
		#region Variables
		#endregion

		#region Properties			
		#endregion

		public ClickableTab(string text, string tabID) : base(text, tabID)
		{	
		}

		/// <summary>
		/// Handles any postback data
		/// </summary>
		/// <param name="data">The postback data</param>
		public override void ProcessPostBackCommand(string command)
		{
			if(command == "Click")
			{
				this.OnClick();
				return;
			}

			base.ProcessPostBackCommand(command);
		}

		/// <summary>
		/// Raises the Click Event
		/// </summary>
		protected void OnClick()
		{
			if(this.Click != null)
				this.Click(this,new TabEventArgs((Tab)this));
		}

		/// <summary>
		/// Creates a postback string that will cause the client to post back a click event
		/// </summary>
		/// <returns>A postback string</returns>
		protected string GetClickPostBack()
		{
			return this.GetClientPostBack("Click");
		}

	}
}
