using System;
using System.Web;
using System.Collections;
using System.Collections.Specialized;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;


/// <summary>
/// Summary description for AddressService
/// </summary>
[System.Web.Script.Services.ScriptService()]
public class AddressService : System.Web.Services.WebService {


    public AddressService () 
    {        
    }

    [WebMethod]
    public AjaxControlToolkit.CascadingDropDownNameValue[] GetDropDownContents(string knownCategoryValues, string category)
    {        
        category = category.ToLower();
        if (knownCategoryValues != null && knownCategoryValues != string.Empty)
        {
            knownCategoryValues = knownCategoryValues.ToLower();
            knownCategoryValues = knownCategoryValues.Substring(knownCategoryValues.LastIndexOf(":") + 1);
            knownCategoryValues = knownCategoryValues.Trim(';');
        }
        System.Collections.Generic.List<AjaxControlToolkit.CascadingDropDownNameValue> list = new System.Collections.Generic.List<AjaxControlToolkit.CascadingDropDownNameValue>();

        #region Country, Province, City
        string cacheKey = knownCategoryValues + category;
        if (category == "country")
        {
            Inaugura.Maps.Country[] countries = Helper.API.AddressManager.GetCountries();            
            foreach (Inaugura.Maps.Country country in countries)
                list.Add(new AjaxControlToolkit.CascadingDropDownNameValue(country.Name, country.ID));            
        }
        else if (category == "province")
        {
            // automatically assume canada
            knownCategoryValues = "995C7A40-71EA-423F-86F6-6DAF8C25DAE2"; // canada
            if (knownCategoryValues != string.Empty)
            {
                Inaugura.Maps.Province[] provinces = Helper.API.AddressManager.GetProvinces(new Guid(knownCategoryValues));
                foreach (Inaugura.Maps.Province province in provinces)
                    list.Add(new AjaxControlToolkit.CascadingDropDownNameValue(province.Name, province.ID));
            }
        }
        else if (category == "city")
        {
            Inaugura.Maps.City[] cities = Helper.API.AddressManager.GetMajorCities(new Guid(knownCategoryValues));
            foreach (Inaugura.Maps.City city in cities)
                list.Add(new AjaxControlToolkit.CascadingDropDownNameValue(city.Name, city.ID));
        }
        #endregion

        #region Locale
        else if (category == "localetype")
        {
            list.Add(new AjaxControlToolkit.CascadingDropDownNameValue(Inaugura.Maps.Locale.LocaleType.University.ToString(), ((int)Inaugura.Maps.Locale.LocaleType.University).ToString()));
            list.Add(new AjaxControlToolkit.CascadingDropDownNameValue("Co-op Employer", ((int)Inaugura.Maps.Locale.LocaleType.Employer).ToString()));            
        }
        else if (category == "locale")
        {
            Inaugura.Maps.Locale[] locales = Helper.API.AddressManager.GetLocales((Inaugura.Maps.Locale.LocaleType)Enum.Parse(typeof(Inaugura.Maps.Locale.LocaleType), knownCategoryValues));
            foreach (Inaugura.Maps.Locale locale in locales)
                list.Add(new AjaxControlToolkit.CascadingDropDownNameValue(locale.Name, locale.ID));
        }
        #endregion
        return list.ToArray();
   }
    
}

