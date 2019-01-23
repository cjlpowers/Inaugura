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

public partial class Signup : System.Web.UI.Page
{
    #region Properties
    private Inaugura.RealLeads.User User
    {
        get
        {          
                return this.Session["RegistrationUser"] as Inaugura.RealLeads.User;
        }
        set
        {
            this.Session["RegistrationUser"] = value;
        }
    }
    
    private string Password
    {
        get
        {
            return this.Session["RegistrationPassword"] as string;
        }
        set
        {
            this.Session["RegistrationPassword"] = value;
        }
    }

    private Inaugura.RealLeads.Listing Listing
    {
        get
        {
            if (this.Session["NewListing"] != null)
                return this.Session["NewListing"] as Inaugura.RealLeads.Listing;
            else
                return null;
        }
        set
        {
            this.Session["NewListing"] = value;
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {            
            try
            {
                this.FillCodes(10);
            }
            catch (Inaugura.Data.DataException exception)
            {
                Inaugura.Log.AddException(exception);
                SessionHelper.RedirectTemporaryError();
            }
        }
    }
   
    protected void Page_Init(object sender, EventArgs e)
    {
        this.Master.SetBodyContent("CreateListing.html");
    }

    protected void Wizard1_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        #region Login Information Step
        if (this.Wizard1.ActiveStep.ID == "Email")
        {
            this.mLbEmailErrorMsg.Text = string.Empty;
            // validate the email address           
            if (!Inaugura.Validation.ValidateEmail(this.mTxtEmail.Text))
            {
                e.Cancel = true;
                this.mLbEmailErrorMsg.Text = "Please enter a valid email address";
                return;
            }

            // make sure the email is not already registered
            Inaugura.RealLeads.User user = Helper.API.UserManager.GetUserByEmail(this.mTxtEmail.Text);
            if (user != null)
            {
                // see if the password box has been filld
                if (this.mTxtEmailPassword.Text.Length > 0)
                {
                    // check the passwords
                    if (user.Password.IsMatch(this.mTxtEmailPassword.Text))
                    {
                        this.User = user;
                        // see if the user already has a listing
                        Inaugura.RealLeads.Listing[] listings = Helper.API.ListingManager.GetListings(user);
                        if (listings.Length >= user.MaxListings)
                        {
                            this.mLbEmailErrorMsg.Text = string.Format("Our records indicate you have created {0} {1}. Your current account only allows you to have {2} listings.", listings.Length, listings.Length == 1 ? "listing" : "listings", user.MaxListings);
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            this.Wizard1.MoveTo(this.PostalCode);
                            return;
                        }
                    }
                    else
                    {
                        this.mLbEmailErrorMsg.Text = "Your login attempt was not successful. Please try again.";
                        e.Cancel = true;
                        return;
                    }                   
                }

                this.mRowEmailPassword.Visible = true;
                this.mLbEmailErrorMsg.Text = "The email address you entered has already been registered. If you have previously registered this email address enter your password now.";
                e.Cancel = true;
                return;
            }
        }
        #endregion

        #region Password
        else if (this.Wizard1.ActiveStep == StepPassword)
        {
            // make sure the passwords are correct
            if (!Inaugura.Validation.ValidatePassword(this.mTxtPassword.Text, 4, 10, false, false, false))
            {
                e.Cancel = true;
                this.mLbLoginErrorMsg.Text = "Please enter valid password containing at least 4 but no more then 10 characters";
                return;
            }
            if (this.mTxtPassword.Text != this.mTxtPassword2.Text)
            {
                e.Cancel = true;
                this.mLbLoginErrorMsg.Text = "The entered passwords do not match";
                return;
            }
            this.Password = this.mTxtPassword.Text;
            this.Wizard1.MoveTo(this.PostalCode);
        }
        #endregion

        #region Name
        else if (this.Wizard1.ActiveStep.ID == "Name")
        {
            // make sure the user supplied a first and last name
            if (this.mTxtFirstName.Text.Length == 0)
            {
                e.Cancel = true;
                this.mLbNameErrorMsg.Text = "Please enter your first name";
                return;
            }
            if (this.mTxtLastName.Text.Length == 0)
            {
                e.Cancel = true;
                this.mLbNameErrorMsg.Text = "Please enter your last name";
                return;
            }
        }
        #endregion

        #region Phone Number
        else if (this.Wizard1.ActiveStep.ID == "PhoneNumber")
        {
            this.mLbPhoneNumberErrorMsg.Text = string.Empty;
            if (!Inaugura.Validation.RegexPhone10Digit.IsMatch(this.mTxtPhoneNumber.Text))
            {
                e.Cancel = true;
                this.mLbPhoneNumberErrorMsg.Text = "Please enter your 10 digit phone number";
                return;
            }
        }
        #endregion

        #region Pincode
        else if (this.Wizard1.ActiveStep.ID == "Pincode")
        {
            this.mLbPincodeErrorMsg.Text = string.Empty;
            if (!Inaugura.Validation.ValidatePinCode(this.mTxtPincode.Text, 4))
            {
                e.Cancel = true;
                this.mLbPincodeErrorMsg.Text = "Please enter a 4 digit pincode";
                return;
            }
            Inaugura.RealLeads.User user = this.User;
            if (user == null)
            {
                // Create the user
                this.User = this.CreateUser();
                user = this.User;
            }
            else
            {
                // update the user
                user.PinCode = this.mTxtPincode.Text;
                this.User = user;
                Helper.API.UserManager.UpdateUser(user);                
            }
        }
        #endregion

        #region WeekdaySchedule
        else if (this.Wizard1.ActiveStep.ID == "ContactSettings")
        {
            Inaugura.RealLeads.User user = this.User;
            if (user == null)
                throw new NullReferenceException("The user object was null");

            user.ContactSchedules.Clear();
            Inaugura.RealLeads.ContactSchedule schedule = new Inaugura.RealLeads.ContactSchedule("My Contact Schedule");
            schedule.ContactNumber = this.mTxtContactNumber.Text;
            schedule.Days = this.mContactDays.DaysOfWeek;
            schedule.StartTime = this.mContactTimeStart.Time;
            schedule.StopTime = this.mContactTimeEnd.Time;
            user.ContactSchedules.Add(schedule);
            Helper.API.UserManager.UpdateUser(user);
        }
        #endregion

        #region Location
        else if (this.Wizard1.ActiveStep.ID == "PostalCode")
        {
            this.mLbPostalErrorMsg.Text = string.Empty;
            string postal = this.mTxtPostal.Text;
            postal.Replace(" ", "");
            if (!Inaugura.Validation.RegexPostalCode.IsMatch(postal))
            {
                this.mLbPostalErrorMsg.Text = "The postal code you have entered does not appear to be valid. Please try entering your postal code again.";
                e.Cancel = true;
                return;
            }

            Inaugura.Maps.Address address = Helper.API.AddressManager.LocatePostal(postal);
            if (address == null)
            {
                this.mLbPostalErrorMsg.Text = "The postal code you entered could not be located. Please ensure the postal code you entered is correct.";
                e.Cancel = true;
                return;
            }

            this.mLbCity.Text = address.City;
            this.mLbProvince.Text = address.StateProv;
            this.mLbCountry.Text = address.Country;
            this.mLbPostal.Text = address.ZipPostal;

            this.Session.Add("listingAddress", address);
        }

        else if (this.Wizard1.ActiveStep.ID == "StreetAddress")
        {
            string streetAddress = this.mTxtStreetAddress.Text;
            streetAddress = streetAddress.Trim();
            if (streetAddress == string.Empty)
            {
                this.mLbStreetAddressErrorMsg.Text = "Please enter the street address for your property.";
                e.Cancel = true;
                return;
            }

            Inaugura.Maps.Address address = this.Session["listingAddress"] as Inaugura.Maps.Address;
            if (address == null)
            {
                this.mLbStreetAddressErrorMsg.Text = "Your session has expired. Please restart this listing creation wizard.";
                e.Cancel = true;
                return;
            }
            address.Street = streetAddress;
        }
        #endregion
            
        #region Listing Details
        else if (this.Wizard1.ActiveStep.ID == "ListingCode")
        {
            if (this.User == null)
                this.User = this.CreateUser();

            Inaugura.RealLeads.User user = this.User;

            // setup the listing    
            Inaugura.RealLeads.PropertyListing listing = null;
            if (Helper.Configuration.Mode == Helper.Configuration.WebSiteMode.RealEstateListings)
            {
                listing = new Inaugura.RealLeads.RealEstateListing();
                listing.ExpirationDate = user.CalculateListingExpiration();
                listing.Status = Inaugura.RealLeads.Listing.ListingStatus.Inactive;
            }
            else if (Helper.Configuration.Mode == Helper.Configuration.WebSiteMode.RentalPropertyListings)
            {
                listing = new Inaugura.RealLeads.RentalPropertyListing();
                listing.ExpirationDate = user.CalculateListingExpiration();
                listing.Status = Inaugura.RealLeads.Listing.ListingStatus.Inactive;
            }
            else
                throw new NotSupportedException("The mode was not supported");

            listing.UserID = user.ID;
            listing.Code = this.mDlListingNumbers.SelectedValue.Trim();
            listing.Address = this.Session["listingAddress"] as Inaugura.Maps.Address;
            this.Listing = listing;
            Helper.API.ListingManager.AddListing(listing);
        }
        #endregion

        //#region Promotions
        //else if (this.Wizard1.ActiveStep.ID == "Promotions")
        //{
        //    this.mLbPromotionError.Text = string.Empty;

        //    if (this.mTxtPromotion.Text.Length > 0)
        //    {
        //        // try and get the promotion
        //        Inaugura.RealLeads.Promotion promotion = Helper.API.UserManager.GetPromotionByCode(this.mTxtPromotion.Text);
        //        if (promotion == null)
        //        {
        //            e.Cancel = true;
        //            this.mLbPromotionError.Text = "The promotion code you have entered did not exist.";
        //            return;
        //        }
        //        if (this.User != null)
        //            this.User.ApplyPromotion(Helper.API, promotion);
        //    }
        //}
        //#endregion
    }
          
    /// <summary>
    /// Fills the 
    /// </summary>
	/// <param name="zoneID">The zone ID</param>
	/// <param name="maxCodes">The maximum number of pincodes to show</param>
    private void FillCodes(int maxCodes)
    {
        this.mDlListingNumbers.Items.Clear();

		string[] codes = Helper.API.ListingManager.GetUnusedCodes(maxCodes);
        // TODO handle when there are no more unused pincodes 
		foreach (string code in codes)
        {
			ListItem item = new ListItem(code);
            this.mDlListingNumbers.Items.Add(item);
        }
    }

    protected void Wizard1_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {
        if(this.User != null && this.Listing != null)
        {
            Helper.Session.Login(this.User);
            this.Response.Redirect(string.Format("Listing.aspx?id={0}", this.Listing.ID),true);

            // create a new message
            try
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(Helper.Configuration.SystemEmail, Helper.Configuration.AdministratorEmail);
                mail.Subject = "New Listing";
                mail.Body = this.Listing.Code;
                Helper.SendMessage(mail);
            }
            catch (Exception ex)
            {
                Helper.API.LogError(ex);
            }
        }
    }  
    
    private Inaugura.RealLeads.User CreateUser()
    {
        Inaugura.RealLeads.User user = new Inaugura.RealLeads.User();
        user.Email = this.mTxtEmail.Text;
        user.PhoneNumber = this.mTxtPhoneNumber.Text;
        user.PhoneNumberPrivate = this.mChkHidePhoneNumber.Checked;
        user.FirstName = this.mTxtFirstName.Text;
        user.LastName = this.mTxtLastName.Text;
        user.SetPassword(this.Password);
        user.PinCode = this.mTxtPincode.Text;
        user.MaxListings = Helper.Configuration.MaxListings;
        user.MaxImages = Helper.Configuration.MaxImages;
        user.ListingExpiration = TimeSpan.FromDays(Helper.Configuration.ListingExpiration);
        
        // add email verification key
        user.Details.Add("emailVerificationKey", Guid.NewGuid().ToString());

        Helper.API.UserManager.AddUser(user);
        try
        {
            Helper.SendEmailVerificationMessage(user);
        }
        catch (Exception ex)
        {
            Master.ShowMessage("Unable to send confirmation email message to the address specified");
            Helper.API.LogError(ex);
        }        
        return user;
    }    
}
