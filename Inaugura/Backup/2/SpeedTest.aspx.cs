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

public partial class SpeedTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        Inaugura.RealLeads.Listing listing = Helper.API.ListingManager.GetFeaturedListing(typeof(Inaugura.RealLeads.RentalPropertyListing));


        System.Xml.XmlNode node = listing.Xml;

        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        watch.Start();

        for (int i = 0; i < 10000; i++)
        {  
            string str = Cache["XML"] as string;
            str = null;
            if (str == null)
            {
                //this.mLbResult.Text = DateTime.Now.ToString() + "\n";
                str = WebServices.ListingService.TransformListing(listing, null);
                Cache["XML"] = str;
            }
        }
        watch.Stop();
        this.mLbResult.Text += watch.Elapsed.ToString();
    }
}
