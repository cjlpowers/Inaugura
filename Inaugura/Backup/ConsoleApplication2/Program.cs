using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            Inaugura.RealLeads.User user = new Inaugura.RealLeads.User(Inaugura.Xml.Helper.GetDocumentNode("xmlfile1.xml"));
            Inaugura.RealLeads.Data.IRealLeadsDataAdaptor data = new Inaugura.RealLeads.Data.SQLAdaptor(System.Configuration.ConfigurationSettings.AppSettings["DB"], System.Configuration.ConfigurationSettings.AppSettings["Directory"]);
            Inaugura.RealLeads.Data.Cached.CachedAdaptor cachedData = new Inaugura.RealLeads.Data.Cached.CachedAdaptor(data, new Inaugura.Caching.WebCache(TimeSpan.FromMinutes(5)));
            Inaugura.RealLeads.RealLeadsAPI api = new Inaugura.RealLeads.RealLeadsAPI(cachedData);
            api.UserManager.AddUser(user);

            return;

            Inaugura.RealLeads.Promotion promotion = new Inaugura.RealLeads.Promotion();
            promotion.Description = "A test promotion";
            promotion.Actions.Add(new Inaugura.RealLeads.Promotion.Action("@maxListings","5"));
            promotion.Actions.Add(new Inaugura.RealLeads.Promotion.Action("@maxImages", "8"));
            Console.WriteLine(promotion.Xml.OuterXml);
          
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.Load("XmlFile1.xml");

            System.Xml.XmlNode node = promotion.Apply(xmlDoc.SelectSingleNode("User"));

            xmlDoc.Save("XmlFile2.xml");

            return;

            Inaugura.Maps.Address address = api.LocatePostal("N2L6G9");
            Inaugura.RealLeads.RentalPropertySearch search = new Inaugura.RealLeads.RentalPropertySearch();
            search.Pets = true;
            search.Address.Latitude = address.Latitude;
            search.Address.Longitude = address.Longitude;
            search.Radius = 100;

            search.StartIndex = 1;
            search.EndIndex = 200;

            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            Inaugura.RealLeads.Listing[] listings = api.ListingManager.SearchListings(search);
            watch.Stop();
            Console.WriteLine("First Call: {0}", watch.Elapsed);

            for (int i = 0; i < 10; i++)
            {
                watch.Reset();
                watch.Start();
                listings = api.ListingManager.SearchListings(search);
                watch.Stop();
                Console.WriteLine("\t{0}", watch.Elapsed);
                //Console.ReadLine();
            }

        }
    }
}
