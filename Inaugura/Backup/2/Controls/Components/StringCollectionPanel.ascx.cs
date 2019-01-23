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

public partial class StringCollectionPanel : System.Web.UI.UserControl
{
    public enum StringCollectionPanelMode
    {
        View,
        Edit
    }

    #region Variables
    private StringCollectionPanelMode mMode = StringCollectionPanelMode.View;
    #endregion

    #region Properties
    public Inaugura.RealLeads.StringCollection StringCollection
	{
		set
		{
			this.mRepeater.DataSource = value;
			this.mRepeater.DataBind();

            System.Text.StringBuilder str = new System.Text.StringBuilder();
            foreach (string item in value)
            {
                str.Append(item);
                str.Append("\n");
            }
            this.mTxtItems.Text = str.ToString();
		}
        get
        {
            Inaugura.RealLeads.StringCollection collection = new Inaugura.RealLeads.StringCollection();
            string[] items = this.mTxtItems.Text.Split('\n');
            foreach (string item in items)
            {
                string itemStr = item.Trim('\n', ' ', '\t');
                if (itemStr != string.Empty)
                    collection.Add(itemStr);
            }
            return collection;
        }
	}

    public StringCollectionPanelMode Mode
    {
        get
        {
            return mMode;
        }
        set
        {
            this.mMode = value;
        }
    }

    public bool Enabled
    {
        get
        {
            return this.mTxtItems.Enabled;
        }
        set
        {
            this.mTxtItems.Enabled = value;
        }
    }
	#endregion

	protected void Page_Load(object sender, EventArgs e)
	{
	}

    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.mAdmin.Visible = (this.Mode == StringCollectionPanelMode.Edit);
        this.mRepeater.Visible = !this.mAdmin.Visible;
    }   
}
