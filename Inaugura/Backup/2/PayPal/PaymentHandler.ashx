<%@ WebHandler Language="C#" Class="PaymentHandler" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;
using System.IO;

public class PaymentHandler : BaseHttpHandler, IRequiresSessionState
{
    public override bool ValidateParameters(HttpContext context)
    {
        return true;
    }
    
    public override bool RequiresAuthentication
    {
        get
        {             
            return false; 
        }  
    }

    private string HttpPost(string URI, string Parameters)
    {
        System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
        //req.Proxy = new System.Net.WebProxy(ProxyString, true);

        req.ContentType = "application/x-www-form-urlencoded";
        req.Method = "POST";

        byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Parameters);
        req.ContentLength = bytes.Length;

        System.IO.Stream os = req.GetRequestStream();
        os.Write(bytes, 0, bytes.Length);
        os.Close();

        System.Net.WebResponse resp = req.GetResponse();
        if (resp == null) return null;

        System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
        return sr.ReadToEnd().Trim();
    }


    
    public override void HandleRequest(HttpContext context)
    {             
        StreamWriter txt = System.IO.File.CreateText(context.Server.MapPath(string.Format("~/Content/ipn_{0}.txt", DateTime.Now.Ticks)));
        txt.WriteLine(DateTime.Now.ToString());
        try
        {
            string strFormValues = context.Request.Form.ToString();
            string strNewValue;
            string strResponse;

            txt.WriteLine(strFormValues);

            // Create the request back
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.sandbox.paypal.com/cgi-bin/webscr");            

            // Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            strNewValue = strFormValues + "&cmd=_notify-validate";
            req.ContentLength = strNewValue.Length;
            // Write the request back IPN strings
            StreamWriter stOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
            stOut.Write(strNewValue);
            stOut.Close();

            // Do the request to PayPal and get the response
            StreamReader stIn = new StreamReader(req.GetResponse().GetResponseStream());
            strResponse = stIn.ReadToEnd();
            stIn.Close();

            txt.WriteLine(strResponse);

            // Confirm whether the IPN was VERIFIED or INVALID. If INVALID, just ignore the IPN
            if (strResponse == "VERIFIED")
            {
                // add the invoice     
                Inaugura.RealLeads.Invoice invoice = new Inaugura.RealLeads.Invoice();          
                
                string[] keys = context.Request.Form.AllKeys;
                foreach(string key in keys)
                {
                    invoice.Details.Add(key,context.Request.Form[key]);
                }

                Inaugura.RealLeads.Listing listing = null;
                string listingID = invoice.Details["item_number"];
                if (listingID != null)
                    listing = Helper.API.ListingManager.GetListing(new Guid(listingID));

                txt.WriteLine(string.Format("listing id = {0}", listingID));

                if (listing != null)
                {
                    // add 6 months to the expiration date
                    listing.ExpirationDate = listing.ExpirationDate.AddMonths(6);
                    listing.Status = Inaugura.RealLeads.Listing.ListingStatus.Active;
                    Helper.API.ListingManager.UpdateListing(listing);
                    invoice.Details.Add("listingID", listing.ID.ToString());
                }
                else
                {
                    txt.WriteLine("Listing was null");
                    throw new ApplicationException(string.Format("An attempt to activate a listing was made however the listing could not be found\n\n{0}", invoice.Xml.OuterXml));
                }                                             
                // store the invoice 
                //DataHelper.RealLeadsDataStore.InvoiceStore.Add(invoice);
                throw new NotImplementedException();
            }
        }
        catch (Exception ex)
        {
            Helper.API.LogError(ex);
        }
        finally
        {
            txt.Flush();
            txt.Close();
        }       
    }   
}
