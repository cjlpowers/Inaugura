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

public partial class MP3Player : System.Web.UI.UserControl
{
    private string mDescriptionFilePath;
    private string mOpenHouseFilePath;
    private Unit mWidth;
    private Unit mHeight;

    public string DescriptionFilePath
    {
        get
        {
            return this.mDescriptionFilePath;
        }
        set
        {
            this.mDescriptionFilePath = value;
        }
    }

    public string OpenHouseFilePath
    {
        get
        {
            return this.mOpenHouseFilePath;
        }
        set
        {
            this.mOpenHouseFilePath = value;
        }
    }

    public Unit Width
    {
        get
        {
            return this.mWidth;
        }
        set
        {
            this.mWidth = value;
        }
    }

    public Unit Height
    {
        get
        {
            return this.mHeight;
        }
        set
        {
            this.mHeight = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected override void Render(HtmlTextWriter writer)
    {
        writer.Write(string.Format("<object classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,0,0\" name=\"movie\" width=\"{0}\" height=\"{1}\" align=\"center\" id=\"movie\">",this.Width.ToString(), this.Height.ToString()));
        writer.Write("<param name=\"movie\" value=\"MP3Player.swf\" />");
        writer.Write("<param name=\"menu\" value=\"false\" />");
        writer.Write("<param name=\"quality\" value=\"high\" />");
        writer.Write("<param name=\"scale\" value=\"noborder\" />");
        writer.Write("<param name=\"wmode\" value=\"transparent\" />");
        writer.Write("<param name=\"bgcolor\" value=\"#000000\" />");

        string flashVarsValue = string.Empty;
        if (this.DescriptionFilePath != null)
            flashVarsValue = string.Format("description={0}", this.Server.UrlEncode(this.DescriptionFilePath));

        if (this.OpenHouseFilePath != null)
        {
            if (flashVarsValue.Length > 0)
                flashVarsValue += "&";

            flashVarsValue += string.Format("openhouse={0}", this.Server.UrlEncode(this.OpenHouseFilePath));
        }
        writer.Write(string.Format("<PARAM NAME=\"FlashVars\" VALUE=\"{0}\"/>", flashVarsValue));

        writer.Write(string.Format("<EMBED wmode=\"transparent\" src=\"MP3Player.swf\" width=\"{0}\" height=\"{1}\" FlashVars=\"{2}\"/>",this.Width.ToString(),this.Height.ToString(),flashVarsValue));
        writer.Write("</object>");        
    }
}
