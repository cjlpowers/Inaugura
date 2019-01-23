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

public partial class ImageUpload : System.Web.UI.Page
{

    #region Properties
    /// <summary>
    /// The listing ID
    /// </summary>
    private Guid ListingID
    {
        get
        {
            if (this.Request.Params["listing"] == null)
                return Guid.Empty;
            return new Guid(this.Request.Params["listing"]);
        }
    }

    /// <summary>
    /// The listing
    /// </summary>
    protected Inaugura.RealLeads.Listing Listing
    {
        get
        {
            Guid id = this.ListingID;
            if (id != Guid.Empty)
                return Helper.API.ListingManager.GetListing(id);
            return null;
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        this.mLbError.Text = string.Empty;
        this.mPanelScript.Visible = false;
    }
   
    protected void mBtnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            Inaugura.RealLeads.Listing listing = this.Listing;
            if (listing != null)
            {
                // security check
                listing.EnforceEditPolicy();

                if (this.mFileUpload.FileContent.Length > Helper.Configuration.MaxImageUploadSize)
                {
                    this.mLbError.Text = "The image you have selected is to large. Please reduce the size of the image before uploading";
                    return;
                }

                try
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(this.mFileUpload.FileContent);
                    listing.AddImage(Helper.API, img, string.Empty);
                    this.mPanelScript.Visible = true;
                }
                catch (Exception ex)
                {
                    this.mLbError.Text = "The file you are uploading does not appear to be a image file. Please ensure the file you are trying to upload is a standard image format (.jpg, .bmp, .gif, .png).";
                    return;
                }
            }
            
        }
        catch (Exception ex)
        {
            this.mLbError.Text = ex.Message;
        }        
    }
}
