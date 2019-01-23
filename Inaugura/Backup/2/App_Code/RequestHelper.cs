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
/// Summary description for RequestHelper
/// </summary>
public static class RequestHelper
{
    public static Guid ID
    {
        get
        {
            string id = RequestHelper.GetValue("id");
            if (id == null)
                return Guid.Empty;
            return new Guid(id);
        }
    }

    public static string Code
    {
        get
        {
            return RequestHelper.GetValue("code");
        }
    }

    public static string Action
    {
        get
        {
            return RequestHelper.GetValue("action");
        }
    }

    public static string Target
    {
        get
        {
            return RequestHelper.GetValue("target");
        }
    }

    private static string GetValue(string param)
    {
        if (System.Web.HttpContext.Current.Request.Params[param] != null)
            return System.Web.HttpContext.Current.Request.Params[param].ToLower();
        else
            return null;     
    }
}
