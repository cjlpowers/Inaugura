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

using Inaugura.RealLeads;

public partial class ContactScheduleCollectionPanel : System.Web.UI.UserControl
{
	#region Properties
	public Inaugura.RealLeads.ContactScheduleCollection ContactSchedules
	{
		set
		{           
			this.mRepeater.DataSource = value;
			this.mRepeater.DataBind();
		}
	}
	#endregion
      
	protected void Page_Load(object sender, EventArgs e)
	{
	}
}
