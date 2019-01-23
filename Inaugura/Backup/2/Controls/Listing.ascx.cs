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

public partial class Controls_Listing : System.Web.UI.UserControl, IPostBackEventHandler
{
    #region Properties
    private Guid ListingID
    {
        get
        {
            if (ViewState["listingID"] == null)
                return Guid.Empty;
            return new Guid(this.ViewState["listingID"].ToString());
        }
        set
        {
            this.ViewState["listingID"] = value.ToString();
        }
    }

    private Guid DefaultImageID
    {
        get
        {
            if (ViewState["deafultImage"] == null)
                return Guid.Empty;
            return new Guid(this.ViewState["deafultImage"].ToString());
        }
        set
        {
            this.ViewState["deafultImage"] = value.ToString();
        }
    }
    #endregion
        
    protected void Page_Init(object sender, EventArgs e)
    {
        System.Collections.Generic.Dictionary<string, string> replacements = null;
        replacements = new System.Collections.Generic.Dictionary<string, string>();
        string eventReference = this.Page.ClientScript.GetPostBackEventReference(this, "");        
        replacements.Add("$postbackEvent", eventReference.Replace("'')", string.Empty));
        string eventHyperlink = this.Page.ClientScript.GetPostBackClientHyperlink(this, "");
        replacements.Add("$postbackHyperlink", eventHyperlink.Replace("'')", string.Empty));
        this.mListingViewInfo.Replacements = replacements;
        this.mListingViewPhotos.Replacements = replacements;

        Guid listingID = this.ListingID;
        if (listingID != Guid.Empty)
        {
            Inaugura.RealLeads.Listing listing = Helper.API.ListingManager.GetListing(listingID);
            if (listing != null)
            {
                if (listing.CheckEditPolicy(SessionHelper.User))
                {
                    this.ShowListing(listing);
                    //this.PopulateAdminImages(listing);
                }
            }
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.mFrameImageUpload.Attributes["src"] = string.Format("ImageUpload.aspx?listing={0}", this.ListingID);
        
    }

    protected override void LoadViewState(object savedState)
    {
        base.LoadViewState(savedState);
        string editControl = this.ViewState["EditControl"] as string;
        if (!string.IsNullOrEmpty(editControl))
            ShowEditControl(this.LoadControl(editControl) as ListingEditControl);
    }

    #region Edit Controls
    private ListingEditControl LoadEditControl(string control)
    {
        ListingEditControl ctr = this.LoadControl(control) as ListingEditControl;
        if(ctr == null)
            throw new ArgumentException(string.Format("The control '{0}' could not be loaded", control));

        this.ViewState["EditControl"] = control;
        return ctr;
    }

    private void ShowEditControl(ListingEditControl editControl)
    {       
        this.mListingViewInfo.Visible = false;
        this.mPlaceHolderEdit.Controls.Clear();
        this.mPlaceHolderEdit.Controls.Add(editControl);
        editControl.Ok += new EventHandler(editControl_Ok);
        editControl.Cancel += new EventHandler(editControl_Cancel);
        this.mUpdateInfo.Update();
    }

    private void HideEditControl()
    {
        this.ViewState.Remove("EditControl"); 
        Inaugura.RealLeads.RentalPropertyListing listing = Helper.API.ListingManager.GetListing(this.ListingID) as Inaugura.RealLeads.RentalPropertyListing;
        this.mPlaceHolderEdit.Controls.Clear();
        this.ShowListingInfo(listing);
        this.mListingViewInfo.Visible = true;        
        this.mUpdateInfo.Update();
    }
    
    void editControl_Cancel(object sender, EventArgs e)
    {
        this.HideEditControl();
    }

    void editControl_Ok(object sender, EventArgs e)
    {
        this.HideEditControl();
    }
    #endregion

    #region Photos
    private void PopulateAdminImages(Inaugura.RealLeads.Listing listing)
    {
        if(listing.Images.DefaultImage != null)
            this.DefaultImageID = listing.Images.DefaultImage.ID;
        this.mRepeaterImages.DataSource = listing.Images;
        this.mRepeaterImages.DataBind();
    }
    #endregion

    public void ShowListing(Inaugura.RealLeads.Listing listing)
    {
        this.ListingID = listing.ID;
        if(listing.Images.DefaultImage != null)
            this.DefaultImageID = listing.Images.DefaultImage.ID;
        this.ShowListingInfo(listing);

        this.ShowImages(listing);      
        
        this.mListingViewMap.Listing = listing;
        this.mListingViewHeader.Listing = listing;    
    
        // try and get the listing owner
        Inaugura.RealLeads.User user = Helper.API.UserManager.GetUser(listing.UserID);

        if(user == null)
            throw new ApplicationException("The listing owner could not be found");

        // set the contact info
        this.mLbPhoneNumber.Text = string.Format("({0}) {1}-{2}", user.PhoneNumber.Substring(0,3),user.PhoneNumber.Substring(3,3),user.PhoneNumber.Substring(6,4));

        if (this.Tabs.ActiveTab == this.mTabMap)
            this.Tabs.ActiveTabIndex = this.Tabs.Tabs.IndexOf(this.mTabInfo);
    }

    public void ShowImages(Inaugura.RealLeads.Listing listing)
    {
        this.mTabPhotos.Visible = true;
        if (listing.CheckEditPolicy(Helper.Session.User))
        {
            this.PopulateAdminImages(listing);
            this.mPanelImageAdmin.Visible = true;            
            this.mListingViewPhotos.Visible = false;
        }
        else
        {
            this.mRepeaterImages.Visible = false;
            if (listing.Images.Count > 0)
            {
                this.mListingViewPhotos.Visible = true;
                this.mListingViewPhotos.Listing = listing;
                this.mPanelImageAdmin.Visible = false;
            }
            else
                this.Tabs.Tabs.Remove(this.mTabPhotos);
            this.mPanelImageUpload.Visible = false;
        }        
    }

    private void ShowListingInfo(Inaugura.RealLeads.Listing listing)
    {
        this.mListingViewInfo.Listing = listing;       
    }

    #region IPostBackEventHandler Members
    public void RaisePostBackEvent(string eventArgument)
    {
        this.mListingViewInfo.Visible = false;
        Arguments args = new Arguments(eventArgument);

        if(!string.IsNullOrEmpty(args.EditControl))
        {
            ListingEditControl ctr = this.LoadEditControl(args.EditControl);
            args.Remove(Arguments.EditControlKey);
            ctr.Arguments = args;
            this.ShowEditControl(ctr);
            ctr.Initialize();
        }

        if (!string.IsNullOrEmpty(args.Action))
            this.ProcessAction(args.Action, args);
     }
    #endregion
    
    protected void mBtnSend_Click(object sender, EventArgs e)
    {
        // make sure the users email address is valid
        if (!Inaugura.Validation.ValidateEmail(this.mTxtEmailAddress.Text))
            throw new ApplicationException("Please provide a valid email address.");

        if (string.IsNullOrEmpty(this.mTxtMessage.Text))
            throw new ApplicationException("Please enter your message in the text field below.");

        Inaugura.RealLeads.Listing listing = Helper.Request.Listing;
        Inaugura.RealLeads.User user = Helper.API.UserManager.GetUser(listing.UserID);

        string htmlContent = Helper.Content.LoadContent("Mailouts/ContactListingOwnerRentleads.htm");
        htmlContent = htmlContent.Replace("[emailAddress]", this.mTxtEmailAddress.Text);
        htmlContent = htmlContent.Replace("[emailMessage]", this.mTxtMessage.Text);
        htmlContent = htmlContent.Replace("[listingID]", listing.ID.ToString());
        htmlContent = htmlContent.Replace("[listingTitle]", listing.Title);

        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
        message.Sender = new System.Net.Mail.MailAddress(Helper.Configuration.SystemEmail);

        message.Subject = string.Format("Question regarding listing {0}", listing.Code);
        message.To.Add(user.Email);

        message.IsBodyHtml = true;
        message.Priority = System.Net.Mail.MailPriority.High;
        message.ReplyTo = new System.Net.Mail.MailAddress(this.mTxtEmailAddress.Text);
        message.From = message.ReplyTo;
        message.Body = htmlContent;

        Helper.SendMessage(message);
    }

    private void ProcessAction(string action, Arguments arguments)
    {
        Inaugura.RealLeads.Listing listing = Helper.API.ListingManager.GetListing(this.ListingID);
        if (listing == null)
            throw new ApplicationException("The listing was not found");
        listing.EnforceEditPolicy();

        if (action == "removeroom")
        {
            Inaugura.RealLeads.ResidentialPropertyListing propertyListing = listing as Inaugura.RealLeads.ResidentialPropertyListing;
            if (propertyListing == null)
                throw new ApplicationException("The listing was not found");
            propertyListing.RemoveRoom(arguments.RoomID);
            propertyListing.Update(Helper.API);
            this.ShowListingInfo(propertyListing);
            this.mListingViewInfo.Visible = true;
            this.mUpdateInfo.Update();
        }
        else if (action == "removelevel")
        {
            Inaugura.RealLeads.ResidentialPropertyListing propertyListing  = listing as Inaugura.RealLeads.ResidentialPropertyListing;
            if (propertyListing == null)
                throw new ApplicationException("The listing was not found");
            propertyListing.EnforceEditPolicy();
            propertyListing.Levels.Remove(propertyListing.Levels[arguments.LevelID]);
            propertyListing.Update(Helper.API);
            this.ShowListingInfo(propertyListing);
            this.mListingViewInfo.Visible = true;
            this.mUpdateInfo.Update();
        }
        else if (action == "refreshimages")
        {
            this.SavePhotoChanges();
            this.ShowImages(listing);
            this.mUpdatePhoto.Update();
            //this.mListingViewPhotos.Listing = listing;
            //this.mUpdatePhoto.Update();
        }
        else if (action == "removeimage")
        {
            listing.RemoveImage(Helper.API, arguments.ImageID);
            this.mRepeaterImages.DataSource = listing.Images;
            this.mRepeaterImages.DataBind();
            this.mUpdatePhoto.Update();
        }
        else
            throw new ArgumentException("The action was not supported");
    }

    #region UI Helper Methods
    public string GetImageUrl(Inaugura.RealLeads.Image image, Inaugura.Web.ImageHttpHandler.ImageMode mode)
    {
        string modeStr = string.Empty;
        if(mode == Inaugura.Web.ImageHttpHandler.ImageMode.Size80)
            modeStr = "80";
        else if(mode == Inaugura.Web.ImageHttpHandler.ImageMode.Size160)
            modeStr = "160";
        else 
            throw new NotImplementedException();

        return this.ResolveUrl(string.Format("~/ImageHandler.ashx?id={0}&mode={1}", image.FileID, modeStr));
    }

    protected string MainImageCheck(Inaugura.RealLeads.Image image)
    {
        if (image != null)
        {
            if (image.ID == this.DefaultImageID)
                return "checked=\"checked\"";
        }
        return string.Empty;
    }

    protected bool IsChecked(Inaugura.RealLeads.Image image)
    {
        if (image != null)
        {
            if (image.ID == this.DefaultImageID)
                return true;
        }
        return false;
    }
    #endregion
    protected void mBtnPhotosDone_Click(object sender, EventArgs e)
    {
        SavePhotoChanges();
    }

    private void SavePhotoChanges()
    {
        Inaugura.RealLeads.PropertyListing listing = Helper.API.ListingManager.GetListing(this.ListingID) as Inaugura.RealLeads.PropertyListing;

        if (listing == null)
            return;

        bool changed = false;
        bool defaultImageChanged = false;

        Guid defaultID = new Guid(this.Request.Form["mRbDefaultImage"]);

        foreach (RepeaterItem item in this.mRepeaterImages.Items)
        {
            HiddenField photoID = item.FindControl("mHiddenImageID") as HiddenField;
            TextBox photoDescription = item.FindControl("mTxtDescription") as TextBox;
            CheckBox photoDelete = item.FindControl("mChkDelete") as CheckBox;
            //Inaugura.Web.Controls.ListRadioButton photoDefault = item.FindControl("mRbDefaultImage") as Inaugura.Web.Controls.ListRadioButton;

            if (photoID != null)
            {
                Guid id = new Guid(photoID.Value);
                // get the image
                Inaugura.RealLeads.Image img = listing.Images[new Guid(photoID.Value)];
                if (img != null)
                {
                    if(photoDelete.Checked)
                    {
                        listing.RemoveImage(Helper.API, id);
                        changed = true;
                        continue;
                    }

                    if (photoDescription.Text != img.Caption)
                    {
                        img.Caption = photoDescription.Text;
                        changed = true;
                    }

                    if (id == defaultID && listing.Images.DefaultImage != img)
                    {
                        listing.Images.DefaultImage = img;
                        defaultImageChanged = true;
                    }
                }                
            }
        }
        if (changed || defaultImageChanged)
        {
            Helper.API.ListingManager.UpdateListing(listing);
            if (defaultImageChanged)
            {
                this.ShowListing(listing);
                this.mUpdateInfo.Update();
            }
            else
            {
                this.ShowImages(listing);
                this.mUpdatePhoto.Update();
            }
        }
    }
}
