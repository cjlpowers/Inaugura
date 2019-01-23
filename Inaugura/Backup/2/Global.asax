<%@ Application Language="C#" %>
<%@ Import Namespace="System.Security.Principal"%>

<script runat="server">

    protected void Application_BeginRequest(Object sender, EventArgs e)
    {  
        /*
        HttpRequest req = HttpContext.Current.Request;
       
        //req.UserLanguages;
        if (req != null && req.UserLanguages != null && req.UserLanguages.Length > 0)
        {
            Helper.Content.SetUserLocale(req.UserLanguages[0]);
        }
         * */
    }

    void Application_Start(Object sender, EventArgs e)
    {
        // create the cached data adaptors
        //DataHelper.RealLeadsDataStore = new Inaugura.RealLeads.Data.SQLAdaptor(System.Configuration.ConfigurationManager.ConnectionStrings["InauguraData"].ConnectionString);
    }

    void Application_End(Object sender, EventArgs e)
    {       
        //if (DataHelper.RealLeadsDataStore is IDisposable)
        //    ((IDisposable)DataHelper.RealLeadsDataStore).Dispose();
    }

    void Application_Error(Object sender, EventArgs e)
    {
        // log the error
        try
        {
            Exception ex = Server.GetLastError();
            if (ex != null)
            {
                Inaugura.RealLeads.Administration.ErrorInformation info = new Inaugura.RealLeads.Administration.ErrorInformation(ex);
                info.Details.Add("RequestUrl",HttpContext.Current.Request.Url.AbsoluteUri);                
                if (SessionHelper.User != null)
                    info.Details.Add("UserID", SessionHelper.User.ID.ToString());

                Helper.API.LogError(info);
            }
        }
        catch (Exception nex)
        {
        }
    }

    protected void Application_AuthenticateRequest(Object sender, EventArgs e)
    {
        // initialize the security context with the current users identity
    }  

    void Session_Start(Object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(Object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.        
    }       
</script>
