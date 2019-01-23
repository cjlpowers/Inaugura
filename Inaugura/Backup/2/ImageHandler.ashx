<%@ WebHandler Language="C#" Class="ImageHandler" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Drawing.Imaging;
using System.Drawing;


// <summary>
/// A http handler which performs image loading
/// </summary>
public class ImageHandler : Inaugura.Web.ImageHttpHandler
{
    public ImageHandler() : base("~/Content/ListingImages/","~/Content/Images/Watermark.png", 0.6f)
    {
    }
    
    protected override Image GetImage(string imageName, ImageMode mode)
    {
        if (string.Compare(imageName, "noimage", StringComparison.OrdinalIgnoreCase) == 0)
        {            
            if (mode == ImageMode.Size480)
                imageName = "noimage480x360.jpg";
            else if(mode == ImageMode.Size320)
                imageName = "noimage320x240.jpg";
            else if (mode == ImageMode.Size160)
                imageName = "noimage160x120.jpg";
            else
                imageName = "noimage80x60.jpg";

            string imagePath = Helper.UI.Theme.GetThemedPath("RentLeads", "Images/" + imageName);
            return Image.FromFile(HttpContext.Current.Server.MapPath(imagePath));              
        }  
        else if (System.Web.HttpContext.Current.Session[imageName] != null)
        {
            // use png for the charts 
            this.OutputFormat = ImageFormat.Png;
            return this.GetImageFromSession(imageName);
        }      
        else
        {
            this.OutputFormat = ImageFormat.Jpeg;
            return base.GetImage(imageName, mode);
        }
    }

    protected override Image LoadImage(string imageName)
    {
        // the 'no image' image
         if (imageName == "noimage")
        {
            return Helper.Content.LoadImage("ImageUnavailable.jpg");
        }
        
        Image img = base.LoadImage(imageName);
        if (img != null)
            return img;

        Inaugura.RealLeads.File file = Helper.API.ListingManager.GetFile(new Guid(imageName));
         if (file == null)
             return null;

        using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(file.Data))
        {
            return System.Drawing.Image.FromStream(memoryStream);
        }
    }

    private System.Drawing.Image GetImageFromSession(string imageID)
    {
        System.Drawing.Image img = System.Web.HttpContext.Current.Session[imageID] as System.Drawing.Image;
        return img;
    }
}