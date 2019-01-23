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

public partial class Secure_Inbox : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        this.Title = Helper.UI.Title("Inbox");        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (this.Request.Params["delete"] != null)
            {
                // TODO make sure that the message is owned by the user before deleting
                this.DeleteMessage(this.Request.Params["delete"]);
                this.ShowMessages(this.LoadMessages());
            }
            else
            {
                Inaugura.RealLeads.VoiceMail[] messages = this.LoadMessages();
                this.ShowMessages(messages);
            }
        }
    }

    private void DeleteMessage(string id)
    {
        throw new NotImplementedException();
    }

    private Inaugura.RealLeads.VoiceMail[] LoadMessages()
    {
        // get the voice mail messages
        throw new NotImplementedException();
        //Inaugura.RealLeads.Agent agent = SessionHelper.Agent.ActiveAgent;
        //Inaugura.RealLeads.VoiceMail[] mail = DataHelper.RealLeadsDataStore.VoiceMailStore.GetVoiceMails(agent.ID);
        //return mail;
    }

    private void ShowMessages(Inaugura.RealLeads.VoiceMail[] messages)
    {
        throw new NotImplementedException();
        /*
        // get the voice mail messages
        Inaugura.RealLeads.Agent agent = SessionHelper.Agent.ActiveAgent;
        Inaugura.RealLeads.VoiceMail[] mail = DataHelper.RealLeadsDataStore.VoiceMailStore.GetVoiceMails(agent.ID);

        if (mail.Length > 0)
        {

            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("ImageUrl", typeof(string));
            table.Columns.Add("Date", typeof(string));
            table.Columns.Add("Time", typeof(string));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("CallerID", typeof(string));
            table.Columns.Add("PlayImage", typeof(string));
            table.Columns.Add("PlayUrl", typeof(string));
            table.Columns.Add("DeleteImage", typeof(string));
            table.Columns.Add("DeleteUrl", typeof(string));


            foreach (Inaugura.RealLeads.VoiceMail message in mail)
            {
                DataRow row = table.NewRow();
                row["ID"] = message.ID;
                if (message.Status == Inaugura.RealLeads.VoiceMail.VoiceMailStatus.New)
                    row["ImageUrl"] = this.Page.ResolveClientUrl("~/Content/Images/message.gif");
                else
                    row["ImageUrl"] = this.Page.ResolveClientUrl("~/Content/Images/message_grey.gif");
                row["Date"] = message.Date.ToShortDateString();
                row["Time"] = message.Date.ToShortTimeString();
                row["Status"] = message.Status;
                row["CallerID"] = message.CallerID;
                row["PlayImage"] = this.Page.ResolveClientUrl("~/Content/Images/MiniPlay.gif");
                row["PlayUrl"] = this.Page.ResolveClientUrl("~/AudioHandler.ashx?mode=voicemail&id=" + message.ID);
                row["DeleteImage"] = this.Page.ResolveClientUrl("~/Content/Images/Minidelete.gif");
                row["DeleteUrl"] = this.Page.ResolveClientUrl("~/Secure/VoiceMail.aspx?delete=" + message.ID);
                table.Rows.Add(row);
            }

            this.GridView1.DataSource = table;
            this.GridView1.DataBind();
            this.GridView1.Visible = true;
        }
        else
            this.GridView1.Visible = false;

        this.mLbVoiceMailEmpty.Visible = !this.GridView1.Visible;
         */
    }
}
