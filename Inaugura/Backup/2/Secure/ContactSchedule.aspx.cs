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

public partial class ContactSchedule : System.Web.UI.Page
{
	protected void Page_Init(object sender, EventArgs e)
	{
        this.mBtnAddSchedule.Attributes["onclick"] = string.Format("javascript:OpenWindow('{0}','600','400','ScheduleEdit')", this.ResolveUrl(string.Format("~/Secure/PopupControls/EditSchedule.aspx?mode=create")));        

		this.Title = "RealLeads: Listing Images";
	}

	protected void Page_Load(object sender, EventArgs e)
	{		
        Inaugura.RealLeads.User user = SessionHelper.User;
        if (user == null)
        {
            Master.ShowMessage("Your session appears to have expired. Please login again");
        }
		else
		{
            try
            {
                // see if we are deleting a custom schedule
                if (this.Request.Params["delete"] != null)
                {
                    Guid id = new Guid(this.Request.Params["delete"]);
                    Inaugura.RealLeads.ContactSchedule schedule = user.ContactSchedules[id];
                    if (schedule != null)
                    {
                        user.ContactSchedules.Remove(schedule);
                        Helper.API.UserManager.UpdateUser(user);
                    }
                }
                // load the contact schedules into the control						
                this.mContactSchedules.ContactSchedules = user.ContactSchedules;			            
            }
            catch (Inaugura.Security.SecurityException ex)
            {
                this.Master.ShowMessage(ex.Message);
                Helper.API.LogError(ex);
            }            
		}
	}
}
