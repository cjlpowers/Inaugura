using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class MenuHeaderControl : System.Web.UI.UserControl
{
	private string mTitle;

	public string Title
	{
		get
		{
			return mTitle;
		}
		set
		{
			this.mTitle = value;
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
			//this.mTdMenuFill.Attributes["style"] += string.Format("background-image:url('{0}');", this.Page.ResolveUrl("~/Content/Images/Layout/MenuHeaderFill.gif"));
	}
}
