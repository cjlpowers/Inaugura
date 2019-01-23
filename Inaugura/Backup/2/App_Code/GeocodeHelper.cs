using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using Inaugura.Maps;

/// <summary>
/// Helper class for geocoding
/// </summary>
public class GeocodeHelper
{
    private const string AppID = "YahooDemo";


    public static Address Geocode(string address, out string resultMessage)
    {
        string url = GetGeocodeUrl(address);
        return RequestGeocode(url, out resultMessage);
    }

    public static Address Geocode(Address address, out string resultMessage)
    {
        string url = GetGeocodeUrl(address);
        return RequestGeocode(url, out resultMessage);
    }


    #region Yahoo
    private static string GetGeocodeUrl(string address)
    {
        return string.Format("http://api.local.yahoo.com/MapsService/V1/geocode?appid={0}&location={1}", System.Web.HttpUtility.UrlEncode(AppID), System.Web.HttpUtility.UrlEncode(address));
    }

    private static string GetGeocodeUrl(Address address)
    {
        return string.Format("http://api.local.yahoo.com/MapsService/V1/geocode?appid={0}&street={1}&city={2}&state={3}&country={4}", System.Web.HttpUtility.UrlEncode(AppID), System.Web.HttpUtility.UrlEncode(address.Street), System.Web.HttpUtility.UrlEncode(address.City), System.Web.HttpUtility.UrlEncode(address.StateProv), System.Web.HttpUtility.UrlEncode(address.Country));
    }

    private static Address RequestGeocode(string url, out string resultMessage)
    {
        //System.Net.HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://api.local.yahoo.com/MapsService/V1/geocode?appid=YahooDemo&street=1+university&city=waterloo&state=ON&country=ca");
        System.Net.HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.MediaType = "text/xml";
        request.Method = "GET";

        Address address = new Address();
        resultMessage = string.Empty;

        // execute the request
        try
        {
            HttpWebResponse response = (HttpWebResponse)
                request.GetResponse();

            using (System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(response.GetResponseStream()))
            {

                while (reader.Read())
                {
                    if (reader.NodeType == System.Xml.XmlNodeType.Element)
                    {
                        if (reader.Name == "Result" && reader.GetAttribute("warning") != null)
                            resultMessage = reader.GetAttribute("warning");
                        if (reader.Name == "Latitude")
                            address.Latitude = reader.ReadElementContentAsDouble();
                        if (reader.Name == "Longitude")
                            address.Longitude = reader.ReadElementContentAsDouble();
                        if (reader.Name == "Address")
                            address.Street = Helper.MakeFirstUpper(reader.ReadElementContentAsString());
                        if (reader.Name == "City")
                            address.City = Helper.MakeFirstUpper(reader.ReadElementContentAsString());
                        if (reader.Name == "State")
                            address.StateProv = GetLongStateName(reader.ReadElementContentAsString());
                        if (reader.Name == "Zip")
                            address.ZipPostal = reader.ReadElementContentAsString();
                        if (reader.Name == "Country")
                            address.Country = GetLongCountryName(reader.ReadElementContentAsString());
                    }
                }
            }
        }
        catch (WebException ex)
        {
            resultMessage = "The address could not be found";
            return null;
        }
        return address;
    }
    #endregion

    #region Geocoder.ca
    private static string GetGeocodeUrl_Geocoder(string address)
    {
        return string.Format("http://geocoder.ca/?locate?={1}", System.Web.HttpUtility.UrlEncode(AppID), System.Web.HttpUtility.UrlEncode(address));
    }

    private static string GetGeocodeUrl_Geocoder(Address address)
    {
        return string.Format("http://geocoder.ca/?addresst ={1}&city={2}&prov={3}", System.Web.HttpUtility.UrlEncode(AppID), System.Web.HttpUtility.UrlEncode(address.Street), System.Web.HttpUtility.UrlEncode(address.City), System.Web.HttpUtility.UrlEncode(address.StateProv), System.Web.HttpUtility.UrlEncode(address.Country));
    }

    private static Address RequestGeocode_Geocoder(string url, out string resultMessage)
    {
        //System.Net.HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://api.local.yahoo.com/MapsService/V1/geocode?appid=YahooDemo&street=1+university&city=waterloo&state=ON&country=ca");
        System.Net.HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.MediaType = "text/xml";
        request.Method = "GET";

        Address address = new Address();
        resultMessage = string.Empty;

        // execute the request
        try
        {
            HttpWebResponse response = (HttpWebResponse)
                request.GetResponse();

            using (System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(response.GetResponseStream()))
            {

                while (reader.Read())
                {
                    if (reader.NodeType == System.Xml.XmlNodeType.Element)
                    {
                        if (reader.Name == "Result" && reader.GetAttribute("warning") != null)
                            resultMessage = reader.GetAttribute("warning");
                        if (reader.Name == "Latitude")
                            address.Latitude = reader.ReadElementContentAsDouble();
                        if (reader.Name == "Longitude")
                            address.Longitude = reader.ReadElementContentAsDouble();
                        if (reader.Name == "Address")
                            address.Street = Helper.MakeFirstUpper(reader.ReadElementContentAsString());
                        if (reader.Name == "City")
                            address.City = Helper.MakeFirstUpper(reader.ReadElementContentAsString());
                        if (reader.Name == "State")
                            address.StateProv = GetLongStateName(reader.ReadElementContentAsString());
                        if (reader.Name == "Zip")
                            address.ZipPostal = reader.ReadElementContentAsString();
                        if (reader.Name == "Country")
                            address.Country = GetLongCountryName(reader.ReadElementContentAsString());
                    }
                }
            }
        }
        catch (WebException ex)
        {
            resultMessage = "The address could not be found";
            return null;
        }
        return address;
    }
    #endregion

    private static string GetLongStateName(string state)
    {
        string lowState = state.ToLower();
        switch (lowState)
        {
            case "on":
                return "Ontario";
                break;
            case "qc":
                return "Quebec";
                break;
            default:
                return state;
        }
    }

    private static string GetLongCountryName(string country)
    {
        string lowCountry = country.ToLower();
        switch (lowCountry)
        {
            case "ca":
                return "Canada";
                break;
            default:
                return country;
        }
    }
}
