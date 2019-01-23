using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Helper class for javascript
/// </summary>
public class ScriptHelper
{
    /// <summary>
    /// Registers a client script reference
    /// </summary>
    /// <param name="page">The page</param>
    /// <param name="relativeUrl">The relitive url</param>
    public static void RegisterScriptReference(Page page, string relativeUrl )
    {
        relativeUrl = relativeUrl.ToLower();
        if (!page.ClientScript.IsClientScriptIncludeRegistered(relativeUrl))
            page.ClientScript.RegisterClientScriptInclude(relativeUrl, page.ResolveUrl(relativeUrl));
    }

    public static string CreateAtlasScriptBlock(string javaScript)
    {
        if (!javaScript.EndsWith(";"))
            javaScript = javaScript + ";";
        return string.Format("<script type=\"text/xml-script\"><page xmlns:script=\"http://schemas.microsoft.com/xml-script/2005\">{0}</page></script>", javaScript);
    }

    public static string CreateJavaScriptBlock(string javaScript)
    {
        if (!javaScript.EndsWith(";"))
            javaScript = javaScript + ";";
        return string.Format("<script type=\"text/javascript\">{0}</script>", javaScript);
    }

    public static string CreateJavaScriptReference(string javaScriptPath)
    {
        return string.Format("<script type=\"text/javascript\" src=\"{0}\"/>", javaScriptPath);
    }
}
