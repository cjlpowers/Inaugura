using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Inaugura.Web.Controls
{
	/// <summary>
	/// bstract tab class containing members which enable closing
	/// </summary>
	public abstract class CloseableTab : SelectableTab
	{
		public event TabEventHandler Close; // the close event		

		#region Variables
		private string mCloseBtnImageUrl = string.Empty;
		#endregion

		#region Properties	
		/// <summary>
		/// The url of the image used as the close button
		/// </summary>
		[Category("Appearance")]
		public string CloseTabImageUrl
		{
			get
			{
				return this.mCloseBtnImageUrl;
			}
			set
			{
				this.mCloseBtnImageUrl = value;
			}
		}
		#endregion
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="text">The Tab text</param>
		public CloseableTab(string text, string tabID) : base(text, tabID)
		{	
		}		

		/// <summary>
		/// Handles any postback data
		/// </summary>
		/// <param name="data">The postback data</param>
		public override void ProcessPostBackCommand(string command)
		{
			if(command == "Close")
				this.OnClose();

			base.ProcessPostBackCommand(command);
		}

		/// <summary>
		/// Raises the Close Event
		/// </summary>
		protected void OnClose()
		{
			if(this.Close != null)
				this.Close(this,new TabEventArgs((Tab)this));
		}

		/// <summary>
		/// Creates a postback string that will cause the client to post back a click event
		/// </summary>
		/// <returns>A postback string</returns>
		protected string GetClosePostBack()
		{
			return this.GetClientPostBack("Close");
		}
	}
}
