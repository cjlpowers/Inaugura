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
    protected void Page_Load(object sender, EventArgs e)
    {
        this.mHeader.InnerText = "Property Listing Search";
        if (!this.IsPostBack)
        {
            //this.mRowPrice.Visible = this.mChkAdvancedSearch.Checked;
            //this.mRowPropertyType.Visible = this.mChkAdvancedSearch.Checked;
            //this.mRowBedBath.Visible = this.mChkAdvancedSearch.Checked;

            this.mBtnSearch.ImageUrl = Helper.Content.LocalizedContentPath("images/buttons/search.gif");
                       
            
            //this.LoadLocations();
            this.LoadPrices();
            this.LoadPropertyTypes();
            this.LoadBedrooms();
            this.LoadBathrooms();
        }
    }

    //private void LoadLocations()
    //{        
    //    this.mDlLocation.Items.Clear();
    //    this.mDlLocation.Items.Add(new ListItem("All Regions", "all"));
    //    this.mDlLocation.Items.Add("");
    //    //District[] districts = DataHelper.InauguraDataStore.ZoneStore.GetDistricts("34d9eade-c9bd-404d-85d3-8b8174c4cbf0");
    //    //foreach (District district in districts)
    //    //{
    //    //    this.mDlLocation.Items.Add(new ListItem(district.Name, district.ID));
    //    //}
    //}

    private void LoadPrices()
    {
        this.mDlPriceLow.Items.Clear();
        this.mDlPriceHigh.Items.Clear();

        this.mDlPriceLow.Items.Add(new ListItem(string.Format("{0:C0}", 0), "0"));
        for (int price = 150000; price <= 600000; price+=25000)
        {
            ListItem item = new ListItem(string.Format("{0:C0}", price), price.ToString());
            this.mDlPriceLow.Items.Add(item);
            this.mDlPriceHigh.Items.Add(item);
        }
        this.mDlPriceHigh.Items.Add(new ListItem("and above", "0"));
        this.mDlPriceHigh.SelectedIndex = this.mDlPriceHigh.Items.Count-1;
    }

    private void LoadPropertyTypes()
    {
        this.mDlPropertyType.Items.Clear();
        this.mDlPropertyType.Items.Add(new ListItem("All", Inaugura.RealLeads.PropertyType.NotSpecified.Value.ToString()));
        this.mDlPropertyType.SelectedIndex = 0;
        foreach (Inaugura.RealLeads.PropertyType type in Inaugura.RealLeads.PropertyType.All)
        {
            if (type != Inaugura.RealLeads.PropertyType.Other && type != Inaugura.RealLeads.PropertyType.NotSpecified)
                this.mDlPropertyType.Items.Add(new ListItem(type.Name, type.Value.ToString()));
        }
    }

    private void LoadBathrooms()
    {
        this.mDlBathrooms.Items.Clear();
        this.mDlBathrooms.Items.Add(new ListItem("Any", "0"));
        for (int i = 1; i <= 5; i++)
            this.mDlBathrooms.Items.Add(new ListItem(string.Format("{0} or more", i), i.ToString()));
    }

    private void LoadBedrooms()
    {
        this.mDlBedrooms.Items.Clear();
        this.mDlBedrooms.Items.Add(new ListItem("Any", "0"));
        for (int i = 1; i <= 5; i++)
            this.mDlBedrooms.Items.Add(new ListItem(string.Format("{0} or more", i), i.ToString()));
    }

    protected void mBtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        #region Add an address
        /*
        if (this.mTxtAddress.Text != string.Empty)
        {
            string warning = string.Empty;
            Address address = GeocodeHelper.Geocode(this.mTxtAddress.Text, out warning);
            if (warning != string.Empty)
                this.mLbGeocodeError.Text = warning;
            else
            {
                this.mLbGeocodeError.Text = string.Empty;
                if (address.Street == string.Empty && address.City != string.Empty)
                {
                    Country[] countries = DataHelper.InauguraDataStore.ZoneStore.GetCountries();
                    Country selectedCountry = null;
                    foreach (Country country in countries)
                    {
                        if (country.Name.ToLower() == address.Country.ToLower())
                        {
                            selectedCountry = country;
                            break;
                        }
                    }
                    if (selectedCountry == null)
                    {
                        selectedCountry = new Country(address.Country);
                        DataHelper.InauguraDataStore.ZoneStore.AddCountry(selectedCountry);
                    }

                    Province[] provinces = DataHelper.InauguraDataStore.ZoneStore.GetProvinces(selectedCountry.ID);
                    Province selectedProvince = null;
                    foreach (Province province in provinces)
                    {
                        if (province.Name.ToLower() == address.StateProv.ToLower())
                        {
                            selectedProvince = province;
                            break;
                        }
                    }
                    if (selectedProvince == null)
                    {
                        selectedProvince = new Province(address.StateProv, selectedCountry.ID);
                        DataHelper.InauguraDataStore.ZoneStore.AddProvince(selectedProvince);
                    }

                    City[] cities = DataHelper.InauguraDataStore.ZoneStore.GetCities(selectedProvince.ID);
                    City selectedCity = null;
                    foreach (City city in cities)
                    {
                        if (city.Name.ToLower() == address.City.ToLower())
                        {
                            selectedCity = city;
                            break;
                        }
                    }
                    if (selectedCity == null)
                    {
                        selectedCity = new City(address.City, selectedProvince.ID);
                        selectedCity.Longitude = address.Longitude;
                        selectedCity.Latitude = address.Latitude;
                        DataHelper.InauguraDataStore.ZoneStore.AddCity(selectedCity);
                    }
                }
            }
            return;
        }
         *    */
        #endregion

        Address address = null;
        if (this.mTxtAddress.Text != string.Empty)
        {
            string warning = string.Empty;
            address = GeocodeHelper.Geocode(this.mTxtAddress.Text, out warning);
            if (warning != string.Empty)
            {
                this.mLbGeocodeError.Text = warning;
                return;
            }
        }

        if (this.mDlCities.SelectedItem != null)
        {
            Inaugura.Maps.City city = Helper.API.AddressManager.GetCity(new Guid(this.mDlCities.SelectedValue));
            if (city != null)
            {
                address = new Address();
                address.Latitude = city.Latitude;
                address.Longitude = city.Longitude;
            }
        }

        Inaugura.RealLeads.RealEstateSearch search = new Inaugura.RealLeads.RealEstateSearch();

        if (address != null)
        {
            search.Address = address;            
        }
        else if (this.mDlLocale.SelectedItem != null)
        {
            Inaugura.Maps.Locale locale = Helper.API.AddressManager.GetLocale(new Guid(this.mDlLocale.SelectedValue));
            if (locale != null)
            {
                search.Address = locale.Address;
                search.Radius = locale.Radius;
            }
        }      

        search.Status = Inaugura.RealLeads.RealEstateListing.ListingStatus.Active;

        search.PriceLower = int.Parse(this.mDlPriceLow.SelectedValue);
        search.MinBedrooms = int.Parse(this.mDlBedrooms.SelectedValue);
        search.MinBathrooms = int.Parse(this.mDlBathrooms.SelectedValue);

        int priceHigh = int.Parse(this.mDlPriceHigh.SelectedValue);
        if (priceHigh != 0)
            search.PriceUpper = priceHigh;

        Inaugura.RealLeads.PropertyType type = Inaugura.RealLeads.PropertyType.FromValue(int.Parse(this.mDlPropertyType.SelectedValue));
        if (type != Inaugura.RealLeads.PropertyType.NotSpecified)
            search.PropertyType = type;

        //if(this.mDlLocation.SelectedValue != null && this.mDlLocation.SelectedValue != "all")
        //    search.DistrictID = this.mDlLocation.SelectedValue;
                
        CachedSearch cachedSearch = new CachedSearch(search);
        Helper.Session.Search = cachedSearch;
        this.Response.Redirect(string.Format("~/Search.aspx?search={0}",cachedSearch.Key));
    }

/*
    private void PerformSearch(Inaugura.RealLeads.Search search)
    {      
        string path = "~/Search.aspx?";

        //if(search.PriceLower != 0)
            path = this.AppendParam(path, "lprice", search.PriceLower.ToString());
        
        if(search.PriceUpper != 0)
            path = this.AppendParam(path, "uprice", search.PriceUpper.ToString());

        if(search.MinBedrooms != 0)
            path = this.AppendParam(path, "bedrooms", search.MinBedrooms.ToString());

        if (search.MinBathrooms != 0)
            path = this.AppendParam(path, "bathrooms", search.MinBathrooms.ToString());

        if(search.PropertyType != null)
            path = this.AppendParam(path,"propertytype",search.PropertyType.Value.ToString());

        if (search.DistrictID != null && search.DistrictID != string.Empty && search.DistrictID != "all")
            path = this.AppendParam(path, "district", search.DistrictID);

        this.Response.Redirect(path);
    }

    private string AppendParam(string url, string paramName, string paramValue)
    {
        if (!url.EndsWith("?") && !url.EndsWith("&"))
            url += "&";
        url += paramName + "=" + paramValue;
        return url;
    }
    */

    //protected void mChkAdvancedSearch_CheckedChanged(object sender, EventArgs e)
    //{
    //    this.mRowPrice.Visible = this.mChkAdvancedSearch.Checked;
    //    this.mRowPropertyType.Visible = this.mChkAdvancedSearch.Checked;
    //    this.mRowBedBath.Visible = this.mChkAdvancedSearch.Checked;
    //}
}
