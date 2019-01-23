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
using Inaugura.Maps;

public partial class RealEstateSearchPanel : System.Web.UI.UserControl
{
    private enum LocationType
    {
        Postal,
        City,
        Category
    }

    private LocationType SelectedLocationType
    {
        get
        {
            if (this.Session["SelectedLocationType"] == null)
                return LocationType.Postal;
            else
                return (LocationType) this.Session["SelectedLocationType"];
        }
        set
        {
            this.Session["SelectedLocationType"] = value;
        }
    }    

    protected void Page_Load(object sender, EventArgs e)
    {
        this.mHeader.InnerText = "Rental Listing Search";
        this.mLbAddressError.Text = string.Empty;
        this.mLbCategoryError.Text = string.Empty;
        this.mLbCityError.Text = string.Empty;

          CachedSearch cachedSearch = Helper.Session.Search;
          Inaugura.RealLeads.RentalPropertySearch search = null;
          if (cachedSearch != null && cachedSearch.Search is Inaugura.RealLeads.RentalPropertySearch)
              search = cachedSearch.Search as Inaugura.RealLeads.RentalPropertySearch;

          if (search != null)
          {
              if (!string.IsNullOrEmpty(search.Address.StateProvID))
                  this.mCascadingProvince.SelectedValue = search.Address.StateProvID;

              if (!string.IsNullOrEmpty(search.Address.CityID))
                  this.mCascadingCity.SelectedValue = search.Address.CityID;
          }

          if (!this.IsPostBack)
          {
              //this.mBtnSearch.ImageUrl = Helper.Content.LocalizedContentPath("images/buttons/search.gif");
              this.SetLocationType(this.SelectedLocationType);
              this.FillDates();
              this.LoadPrices();
              this.LoadPropertyTypes();
              this.LoadBedrooms();
              this.LoadParkingSpaces();         

              if (search != null)
              {
                  if (search.MinBedrooms != 0)
                      this.mDlBedrooms.SelectedValue = search.MinBedrooms.ToString();

                  if (search.AvailabilityStart != DateTime.MinValue)
                  {
                      this.mAvailabilityDate.SelectedValue = search.AvailabilityStart.ToOADate().ToString();

                      if (search.AvailabilityEnd != DateTime.MaxValue)
                      {
                          DateTime tempDate = search.AvailabilityStart;
                          int months = 0;
                          while (tempDate < search.AvailabilityEnd)
                          {
                              months++;
                              tempDate = tempDate.AddMonths(1);
                          }

                          ListItem item = this.mDlDuration.Items.FindByValue(months.ToString());
                          if (item != null)
                              this.mDlDuration.SelectedIndex = this.mDlDuration.Items.IndexOf(item);
                      }
                  }

                  if (!string.IsNullOrEmpty(search.Address.ZipPostal))
                      this.mTxtPostal.Text = search.Address.ZipPostal;

                  if (search.Radius != 0)
                      this.mPostalRadius.SelectedIndex = this.mPostalRadius.Items.IndexOf(this.mPostalRadius.Items.FindByValue(search.Radius.ToString()));

                  if (search.RentLower != 0)
                      this.mDlPriceLow.SelectedValue = search.RentLower.ToString();

                  if (search.RentUpper != 0)
                      this.mDlPriceHigh.SelectedValue = search.RentUpper.ToString();

                  if (search.MinParkingSpaces != 0)
                      this.mDlParkingSpaces.SelectedValue = search.MinParkingSpaces.ToString();

                  if (search.PropertyType != null)
                      this.mDlPropertyType.SelectedValue = search.PropertyType.Value.ToString();
                  
                  this.mChkPets.Checked = search.Pets;
                  this.mChkPool.Checked = search.Pool;
                  this.mChkElectricity.Checked = search.IncludesElectricity;
                  this.mChkHeating.Checked = search.IncludesHeating;
                  this.mChkLaundryService.Checked = search.LaundryService;
                  this.mChkInternet.Checked = search.InternetService;
                  this.mChkTelevision.Checked = search.TelevisionService;                  
              }
          }

          if (this.Request.Params["locale"] != null)
              this.SetLocale(this.Request.Params["locale"]);
    }

    private void SetLocale(string localeID)
    {
        Inaugura.Maps.Locale locale = CacheHelper.Retrieve(localeID) as Inaugura.Maps.Locale;
        if (locale == null)
        {
            locale = Helper.API.AddressManager.GetLocale(new Guid(localeID));
            if (locale != null)
                CacheHelper.Insert(locale);
        }

        if (locale == null)
            return;

        this.SetLocationType(LocationType.Category);
        this.mCascadingLocaleType.SelectedValue = ((int)locale.Type).ToString();
        this.mCascadingLocale.SelectedValue = locale.ID;        
    }

    private void SetLocationType(LocationType type)
    {
        this.SelectedLocationType = type;
        if (type == LocationType.City)
        {
            this.mBtnByCity.CssClass = "TabSelected";
            this.mBtnByCategory.CssClass = "Tab";
            this.mBtnByPostal.CssClass = "Tab";
            this.mPhByCity.Visible = true;
            this.mPhByCategory.Visible = false;
            this.mPhByPostal.Visible = false;
        }
        else if (type == LocationType.Category)
        {
            this.mBtnByCity.CssClass = "Tab";
            this.mBtnByCategory.CssClass = "TabSelected";
            this.mBtnByPostal.CssClass = "Tab";
                
            this.mPhByCity.Visible = false;
            this.mPhByCategory.Visible = true;
            this.mPhByPostal.Visible = false;
        }
        else if (type == LocationType.Postal)
        {
            this.mBtnByCity.CssClass = "Tab";
            this.mBtnByCategory.CssClass = "Tab";
            this.mBtnByPostal.CssClass = "TabSelected";

            this.mPhByCity.Visible = false;
            this.mPhByCategory.Visible = false;
            this.mPhByPostal.Visible = true;
        }
    }

    private void LoadPrices()
    {
        this.mDlPriceLow.Items.Clear();
        this.mDlPriceHigh.Items.Clear();

        this.mDlPriceLow.Items.Add(new ListItem(string.Format("{0:C0}", 0), "0"));
        for (int price = 100; price <= 1500; price+=100)
        {
            ListItem item = new ListItem(string.Format("{0:C0}", price), price.ToString());
            this.mDlPriceLow.Items.Add(item);
            item = new ListItem(string.Format("{0:C0}", price), price.ToString());
            this.mDlPriceHigh.Items.Add(item);
        }
        this.mDlPriceHigh.Items.Add(new ListItem("and above", "0"));
        this.mDlPriceHigh.SelectedIndex = this.mDlPriceHigh.Items.Count-1;
    }

    private void LoadPropertyTypes()
    {
        this.mDlPropertyType.Items.Clear();
        this.mDlPropertyType.Items.Add(new ListItem("All", Inaugura.RealLeads.RentalPropertyType.NotSpecified.Value.ToString()));
        this.mDlPropertyType.SelectedIndex = 0;
        foreach (Inaugura.RealLeads.RentalPropertyType type in Inaugura.RealLeads.RentalPropertyType.All)
        {
            if (type != Inaugura.RealLeads.RentalPropertyType.Other && type != Inaugura.RealLeads.RentalPropertyType.NotSpecified)
                this.mDlPropertyType.Items.Add(new ListItem(type.Name, type.Value.ToString()));
        }
    }

    private void LoadParkingSpaces()
    {
        this.mDlParkingSpaces.Items.Clear();
        this.mDlParkingSpaces.Items.Add(new ListItem("Any", "0"));
        for (int i = 1; i <= 5; i++)
            this.mDlParkingSpaces.Items.Add(new ListItem(string.Format("{0} or more", i), i.ToString()));
    }

    private void LoadBedrooms()
    {
        this.mDlBedrooms.Items.Clear();
        this.mDlBedrooms.Items.Add(new ListItem("Any", "0"));
        for (int i = 1; i <= 5; i++)
            this.mDlBedrooms.Items.Add(new ListItem(string.Format("{0} or more", i), i.ToString()));
    }

    public void FillDates()
    {
        this.mAvailabilityDate.Items.Clear();

        System.Collections.Generic.List<ListItem> itemList = new System.Collections.Generic.List<ListItem>();
        DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        itemList.Add(new ListItem("Unspecified", DateTime.MinValue.ToOADate().ToString()));
        for (int i = 0; i <= 12; i++)
        {
            itemList.Add(new ListItem(startDate.ToString("MMMM yyyy"), startDate.ToOADate().ToString()));
            startDate = startDate.AddMonths(1);
        }
        CacheHelper.Insert("Rental_Date", itemList, DateTime.Now.AddHours(5));
        this.mAvailabilityDate.Items.AddRange(itemList.ToArray());
    }


    protected void mBtnSearch_Click(object sender, EventArgs e)
    {
        Inaugura.RealLeads.RentalPropertySearch search = new Inaugura.RealLeads.RentalPropertySearch();        
        if (this.mPhByPostal.Visible)
        {
            if (!Inaugura.Validation.RegexPostalCode.IsMatch(this.mTxtPostal.Text))
            {
                this.mLbAddressError.Text = "Please enter a valid postal code";
                return;
            }

            Inaugura.Maps.Address address = Helper.API.LocatePostal(this.mTxtPostal.Text);
            if (address == null)
            {
                this.mLbAddressError.Text = "Unable to locate the postal code entered";
                return;
            }

            search.Address = new Address();
            search.Address.Label = address.Label;
            search.Address.Latitude = address.Latitude;
            search.Address.Longitude = address.Longitude;
            search.Radius = double.Parse(this.mPostalRadius.SelectedValue);
        }

        if (this.mPhByCity.Visible)
        {

            if (!string.IsNullOrEmpty(this.mDlCities.SelectedValue))
            {
                Inaugura.Maps.City city = Helper.API.AddressManager.GetCity(new Guid(this.mDlCities.SelectedValue));
                if (city != null)
                {                    
                    search.Address.CityID = city.ID;
                    search.Address.Longitude = city.Longitude;
                    search.Address.Latitude = city.Latitude;
                    search.Radius = 0;
                }
            }
            else
            {
                this.mLbCityError.Text = "Please select a city";
                return;
            }
        }

        if (this.mPhByCategory.Visible)
        {
            if (!string.IsNullOrEmpty(this.mDlLocale.SelectedValue))
            {
                Inaugura.Maps.Locale locale = Helper.API.AddressManager.GetLocale(new Guid(this.mDlLocale.SelectedValue));
                if (locale != null)
                {
                    search.Address.Label = locale.Address.Label;
                    search.Address.Latitude = locale.Latitude;
                    search.Address.Longitude = locale.Longitude;
                    search.Radius = locale.Radius;
                }
                else
                {
                    this.mLbCategoryError.Text = "Please make a selection";
                    return;
                }
            }
        }

        search.Status = Inaugura.RealLeads.Listing.ListingStatus.Active;

        search.RentLower = int.Parse(this.mDlPriceLow.SelectedValue);
        search.MinBedrooms = int.Parse(this.mDlBedrooms.SelectedValue);
        search.MinParkingSpaces = int.Parse(this.mDlParkingSpaces.SelectedValue);

        double availabilityStart = double.Parse(this.mAvailabilityDate.SelectedValue);
        if (availabilityStart == 0)
            search.AvailabilityStart = DateTime.MinValue;
        else
        {
            search.AvailabilityStart = DateTime.FromOADate(availabilityStart);

            int duration = int.Parse(this.mDlDuration.SelectedValue);
            if (duration == 0)
                search.AvailabilityEnd = DateTime.MaxValue;
            else
                search.AvailabilityEnd = search.AvailabilityStart.AddMonths(duration);
        }        
               
        int priceHigh = int.Parse(this.mDlPriceHigh.SelectedValue);
        if (priceHigh != 0)
            search.RentUpper = priceHigh;

        Inaugura.RealLeads.RentalPropertyType type = Inaugura.RealLeads.RentalPropertyType.FromValue(int.Parse(this.mDlPropertyType.SelectedValue));
        if (type != Inaugura.RealLeads.RentalPropertyType.NotSpecified)
            search.PropertyType = type;

        search.Pool = mChkPool.Checked;
        search.Pets = mChkPets.Checked;
        search.IncludesElectricity = this.mChkElectricity.Checked;
        search.IncludesHeating = this.mChkHeating.Checked;
        search.LaundryService = this.mChkLaundryService.Checked;
        search.InternetService = this.mChkInternet.Checked;
        search.TelevisionService = this.mChkTelevision.Checked;

        CachedSearch cachedSearch = new CachedSearch(search);
        Helper.Session.Search = cachedSearch;
        this.Response.Redirect(string.Format("~/Search.aspx?search={0}", cachedSearch.Key));
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        this.SetLocationType(LocationType.Category);
    }
    
    protected void mBtnByPostal_Click(object sender, EventArgs e)
    {
        this.SetLocationType(LocationType.Postal);
    }
    protected void mBtnByCity_Click(object sender, EventArgs e)
    {
        this.SetLocationType(LocationType.City);
    }
    protected void mBtnByCategory_Click(object sender, EventArgs e)
    {
        this.SetLocationType(LocationType.Category);
    }
}
