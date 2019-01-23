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

public partial class ScriptImport : System.Web.UI.UserControl
{
    #region Variables
    private string mScriptPath;
    #endregion

    /// <summary>
    /// The path to the script
    /// </summary>
    public string ScriptPath
    {
        get
        {
            return this.mScriptPath;
        }
        set
        {
            this.mScriptPath = value;
        }
    }

    protected override void Render(HtmlTextWriter writer)
    {
        writer.WriteBeginTag("script");
        writer.WriteAttribute("type","text/javascript");
        writer.WriteAttribute("language","javascript");
        writer.WriteAttribute("src",this.Page.ResolveClientUrl(this.mScriptPath));
        writer.Write(HtmlTextWriter.TagRightChar);
        writer.WriteEndTag("script");
    }
}
