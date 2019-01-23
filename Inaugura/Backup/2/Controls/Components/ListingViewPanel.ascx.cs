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

public partial class ListingViewPanel : System.Web.UI.UserControl
{
    #region Variables
    private Inaugura.RealLeads.Listing mListing;
    private string mMode;
    private System.Collections.Generic.Dictionary<string, string> mReplacements;
    #endregion

    #region Properties
    /// <summary>
    /// The listing to display
    /// </summary>
    public Inaugura.RealLeads.Listing Listing
	{
        get
        {
            return mListing;
        }
		set
		{            
            this.mListing = value;            
		}
	}

    /// <summary>
    /// The transform mode
    /// </summary>
    public string Mode
    {
        get
        {
            return this.mMode;
        }
        set
        {
            this.mMode = value;
        }
    }

    /// <summary>
    /// The values to replace in the output
    /// </summary>
    public System.Collections.Generic.Dictionary<string, string> Replacements
    {
        get
        {
            return this.mReplacements;
        }
        set
        {
            this.mReplacements = value;
        }
    }
    #endregion

    protected override void Render(HtmlTextWriter writer)
    {
        if (this.Listing != null)
        {
            if (this.Replacements != null && this.Replacements.Count > 0)
            {                
                System.Text.StringBuilder str = new System.Text.StringBuilder(WebServices.ListingService.TransformListing(this.Listing, this.Mode));                
                System.Collections.Generic.Dictionary<string, string>.Enumerator item = this.Replacements.GetEnumerator();
                while (item.MoveNext())
                    str.Replace(item.Current.Key, item.Current.Value);
                writer.Write(str.ToString());
            }
            else
                writer.Write(WebServices.ListingService.TransformListing(this.Listing, this.Mode));
        }
    }
}
