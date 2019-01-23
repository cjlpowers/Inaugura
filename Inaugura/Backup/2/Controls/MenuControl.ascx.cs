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

public partial class MenuControl : System.Web.UI.UserControl
{
	private string mInnerControlPath = string.Empty;
	public ControlCollection InnerControls
	{
		get
		{
			
			if (this.PlaceHolder1 == null)
				return null;

			return this.PlaceHolder1.Controls;
		}
	}

	public string Title
	{
		get
		{
			return this.MenuHeaderControl1.Title;
		}
		set
		{
			this.MenuHeaderControl1.Title = value;
		}
	}

	public string Width
	{
		get
		{
			return "100%";
			//return this.table.Width;
		}
		set
		{
			//this.table.Width = value;
		}
	}

	public string InnerControlPath
	{
		get
		{
			return this.mInnerControlPath;
		}
		set
		{
			this.mInnerControlPath = value;
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{		

		if (this.InnerControlPath != string.Empty)
		{
			Control control = this.Page.LoadControl(this.InnerControlPath);
			this.InnerControls.Add(control);
		}
	}
}
