using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Drawing.Imaging;

/// <summary>
/// Summary description for Helper
/// </summary>
public static class Helper
{
    #region Internal Constructs
    /// <summary>
    /// A Request helper class
    /// </summary>
    public static class Request
    {
        /// <summary>
        /// The search key
        /// </summary>
        public static string SearchKey
        {
            get
            {
                return HttpContext.Current.Request.Params["search"];
            }
        }


        /// <summary>
        /// The Listing ID
        /// </summary>
        public static Guid ListingID
        {
            get
            {
                return new Guid(HttpContext.Current.Request.Params["listingId"]);
            }
        }

        /// <summary>
        /// Gets the listing currently defined in the request url
        /// </summary>
        public static Inaugura.RealLeads.Listing Listing
        {
            get
            {
                return Helper.API.ListingManager.GetListing(ListingID);
            }
        }

        /// <summary>
        /// Gets the user ID specified in the request url
        /// </summary>
        public static Guid UserID
        {
            get
            {
                return new Guid(HttpContext.Current.Request.Params["userId"]);                
            }
        }

        /// <summary>
        /// The user specified by the request url
        /// </summary>
        public static Inaugura.RealLeads.User User
        {
            get
            {
                return Helper.API.UserManager.GetUser(UserID);
            }
        }
    }

    /// <summary>
    /// A Session State helper class
    /// </summary>
    public static class Session
    {
        #region Variables
        private const string KeySearch = "Search";
        #endregion

        #region Properties
        /// <summary>
        /// The search object
        /// </summary>
        public static CachedSearch Search
        {
            get
            {
                object obj = HttpContext.Current.Session[KeySearch];
                if (obj != null)
                    return obj as CachedSearch;
                return null;
            }
            set
            {
                HttpContext.Current.Session[KeySearch] = value;
            }
        }

        /// <summary>
        /// The active user
        /// </summary>
        static public Inaugura.RealLeads.User User
        {
            get
            {
                return SessionHelper.User;
            }
            set
            {
                SessionHelper.User = value;
            }
        }
        #endregion

        #region Login/Logut Management
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email">The email address</param>
        /// <param name="password">The password</param>
        /// <returns>The user if the login was valid, otherwise null</returns>
        static public Inaugura.RealLeads.User Login(string email, string password)
        {
            Inaugura.RealLeads.User user = Helper.API.UserManager.GetUserByEmail(email);
            if (user != null)
            {
                if (user.Password.IsMatch(password))
                {
                    Login(user);
                    return user;
                }
            }
            return null;
        }

        static public void Login(Inaugura.RealLeads.User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            System.Web.Security.FormsAuthentication.Initialize();

            System.Text.StringBuilder strRoles = new System.Text.StringBuilder();
            foreach (Inaugura.Security.Role role in user.Roles)
                strRoles.Append(role.Name + ";");

            // Create a new ticket used for authentication
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
             1, // Ticket version
             user.Email, // Username associated with ticket
             DateTime.Now, // Date/time issued
             DateTime.Now.AddMinutes(30), // Date/time to expire
             true, // "true" for a persistent user cookie
             strRoles.ToString(), // User-data, in this case the roles
             FormsAuthentication.FormsCookiePath); // Path cookie valid for

            // Hash the cookie for transport
            string hash = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(
             FormsAuthentication.FormsCookieName, // Name of auth cookie
             hash); // Hashed ticket

            // Add the cookie to the list for outgoing response            
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);

            Helper.API.Login(user);

            SessionHelper.User = user;
        }

        static public void RedirectFromLogin()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            // Redirect to requested URL, or homepage if no previous page requested
            string returnUrl = context.Request.QueryString["ReturnUrl"];
            if (returnUrl == null)
                returnUrl = "~/Secure/default.aspx";

            // Don't call FormsAuthentication.RedirectFromLoginPage since it could
            // replace the authentication ticket (cookie) we just added
            context.Response.Redirect(returnUrl);
        }

        /// <summary>
        /// Logs the user out
        /// </summary>
        static public void Logout()
        {
            HttpContext.Current.Session.Clear();
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// Forces a login
        /// </summary>
        /// <param name="queryString"></param>
        static public void ForceLogin(string queryString)
        {
            Logout();
            FormsAuthentication.RedirectToLoginPage(queryString);
        }

        static public void ForceLogin()
        {
            ForceLogin(string.Empty);
        }

        #endregion
    }

    /// <summary>
    /// A Configuration helper class
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// The application title
        /// </summary>
        public static string ApplicationTitle
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ApplicationTitle"];
            }
        }

        /// <summary>
        /// The application slogan
        /// </summary>
        public static string ApplicationSlogan
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ApplicationSlogan"];
            }
        }

        /// <summary>
        /// The support email
        /// </summary>
        public static string SupportEmail
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["SupportEmail"];
            }
        }

        /// <summary>
        /// The system email
        /// </summary>
        public static string SystemEmail
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["SystemEmail"];
            }
        }

        /// <summary>
        /// The administrator email
        /// </summary>
        public static string AdministratorEmail
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["AdministratorEmail"];
            }
        }


        public enum WebSiteMode
        {
            RealEstateListings,
            RentalPropertyListings
        }

        /// <summary>
        /// The maximum number of open searches
        /// </summary>
        static public int MaxOpenSearches
        {
            get
            {
                return int.Parse(System.Configuration.ConfigurationManager.AppSettings["MaxOpenSearches"]);
            }
        }

        /// <summary>
        /// The search page size
        /// </summary>
        static public int SearchPageSize
        {
            get
            {
                return int.Parse(System.Configuration.ConfigurationManager.AppSettings["SearchPageSize"]);
            }
        }

        /// <summary>
        /// The website mode
        /// </summary>
        static public WebSiteMode Mode
        {
            get
            {
                int val = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Mode"]);
                if (val == 1)
                    return WebSiteMode.RealEstateListings;
                else if (val == 2)
                    return WebSiteMode.RentalPropertyListings;
                else
                    throw new NotSupportedException("Mode specified in the web config was not supported");
            }
        }

       
        /// <summary>
        /// The maximum image upload size in bytes
        /// </summary>
        static public int MaxImageUploadSize
        {
            get
            {
                return int.Parse(System.Configuration.ConfigurationManager.AppSettings["MaxImageUploadSize"]);
            }
        }

        /// <summary>
        /// The maximum number of listings a user may have
        /// </summary>
        static public int MaxListings
        {
            get
            {
                return int.Parse(System.Configuration.ConfigurationManager.AppSettings["MaxListings"]);
            }
        }

        /// <summary>
        /// The maximum number of images a user may in any one listing
        /// </summary>
        static public int MaxImages
        {
            get
            {
                return int.Parse(System.Configuration.ConfigurationManager.AppSettings["MaxImages"]);
            }
        }

        /// <summary>
        /// The listing expiration in days
        /// </summary>
        static public int ListingExpiration
        {
            get
            {
                return int.Parse(System.Configuration.ConfigurationManager.AppSettings["ListingExpiration"]);
            }
        }
    }

    /// <summary>
    /// A helper class containing common UI methods
    /// </summary>
    public static class UI
    {
        #region Internal Constructs
        public static class Theme
        {
            /// <summary>
            /// Gets a list of theme names
            /// </summary>
            /// <returns></returns>
            public static string[] GetThemeNames()
            {
                string[] names = HttpContext.Current.Cache["Themes"] as string[];
                if (names == null)
                {
                    names = System.IO.Directory.GetDirectories(HttpContext.Current.Server.MapPath("~/App_Themes/"));
                    HttpContext.Current.Cache.Add("Themes", names, null, DateTime.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                }
                return names;
            }

            /// <summary>
            /// Returns a path to a specific themed resource
            /// </summary>
            /// <param name="themeName">The theme name</param>
            /// <param name="subPath">The sub path to the themed resource</param>
            /// <returns>The path</returns>
            public static string GetThemedPath(string themeName, string subPath)
            {
                return System.IO.Path.Combine(string.Format("~/App_Themes/{0}/", themeName), subPath);
            }

            /// <summary>
            /// Returns a path to a specific themed resource
            /// </summary>
            /// <param name="page">The page instance</param>
            /// <param name="subPath">The sub path to the themed resource</param>
            /// <returns>The path</returns>
            public static string GetThemedPath(Page page, string subPath)
            {
                return GetThemedPath(page.Theme, subPath);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a title string
        /// </summary>
        /// <param name="subTitles">The sub titles</param>
        /// <returns>The title string</returns>
        public static string Title(params string[] subTitles)
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            str.Append(Helper.Configuration.ApplicationTitle);
            foreach (string item in subTitles)
            {
                str.Append(" : ");
                str.Append(item);
            }
            return str.ToString();
        }

        /// <summary>
        /// Fills a text box with a series of items
        /// </summary>
        /// <param name="items">The items</param>
        /// <param name="control">The text box control</param>
        public static void SetItems(string[] items, TextBox control)
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            for (int i = 0; i < items.Length; i++)
            {
                str.Append(items[i]);
                if (i < items.Length - 1)
                    str.Append("\n");
            }
            control.Text = str.ToString();
        }

        /// <summary>
        /// Gets a list of items from a text box control
        /// </summary>
        /// <param name="control">The text box control</param>
        /// <returns>Returns the list of row based items in a text box</returns>
        public static string[] GetItems(TextBox control)
        {
            string[] allItems = control.Text.Split('\n');
            List<string> items = new List<string>();
            foreach (string item in allItems)
            {
                string itemStr = item.Trim('\n', ' ', '\t','\r');
                if (itemStr != string.Empty)
                    items.Add(itemStr);                    
            }
            return items.ToArray();
        }
        #endregion
    }

    /// <summary>
    /// A helper class containing common Content methods
    /// </summary>
    public static class Content
    {
        #region Properties
        static public string LocalePrefix
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            }
        }
        #endregion


        /// <summary>
        /// Sets the user locale to a specific culture
        /// </summary>
        /// <param name="culture"></param>
        static public void SetUserLocale(string culture)
        {
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(culture);
            }
            catch (Exception ex)
            {
                // Dont worry about any exceptions thrown here. It if ever happens it is likley the result of neutral cultures
            }
        }

        #region Editing

        /// <summary>
        /// Determines if a user may edit content
        /// </summary>
        static public bool CanEdit
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["CanEdit"] == null || ((bool)System.Web.HttpContext.Current.Session["CanEdit"]) == false)
                    return false;
                else
                    return true;
            }
            set
            {
                System.Web.HttpContext.Current.Session["CanEdit"] = value;
            }
        }
        #endregion

        #region Loading and Caching of Content

        /// <summary>
        /// Determins if content exists
        /// </summary>
        /// <param name="contentPath">The content path</param>
        /// <returns>True if it exists, false otherwise</returns>
        static public bool ContentExists(string contentPath)
        {
            string filePath = Helper.Content.LocalizedContentPath(contentPath, Helper.Content.LocalePrefix);
            filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);
            return System.IO.File.Exists(filePath);
        }

        /// <summary>
        /// Loads content from a localized file.
        /// </summary>
        /// <param name="contentPath">The non-localized path to the content</param>
        /// <returns>The content</returns>
        static public string LoadContent(string contentPath)
        {
            string filePath = Helper.Content.LocalizedContentPath(contentPath, Helper.Content.LocalePrefix);
            filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);
            if (System.IO.File.Exists(filePath))
            {
                string contentText = System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8);

                #region XP Fix
                if (System.Web.HttpContext.Current.Request.Url.AbsoluteUri.ToLower().Contains("localhost"))
                {
                    // modify any urls to support XP based IIS
                    contentText = contentText.Replace("\"/Content", "\"/Realleads/Content");
                    contentText = contentText.Replace("\"/content", "\"/Realleads/content");
                }
                #endregion
                return contentText;
            }
            else
            {
                throw new System.IO.FileNotFoundException(string.Format("The content resource '{0}' does not exist", filePath), filePath);
                //return string.Format("<p>{0} could not be found</p>", filePath);
                //return null;
            }
        }

        /// <summary>
        /// Loads an image from the localized image content directory (Content/[lang]/Images/).
        /// </summary>
        /// <param name="imageFile">The image file name (ie Image1.jpg)</param>
        /// <returns>The image</returns>
        static public System.Drawing.Image LoadImage(string imageFile)
        {
            string filePath = Helper.Content.LocalizedContentPath("Images/" + imageFile, Helper.Content.LocalePrefix);
            filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);
            if (System.IO.File.Exists(filePath))
            {
                return System.Drawing.Image.FromFile(filePath);
            }
            else
            {
                throw new System.IO.FileNotFoundException(string.Format("The content resource '{0}' does not exist", filePath), filePath);
            }
        }


        /// <summary>
        /// Saves content given the current locale
        /// </summary>
        /// <param name="page">The host page</param>
        /// <param name="content">The content</param>
        /// <param name="contentPath">The content path</param>
        static public void SaveContent(System.Web.UI.Page page, string content, string contentPath)
        {
            Helper.Content.SaveContent(content, page.MapPath(Helper.Content.LocalizedContentPath(contentPath)));
        }

        /// <summary>
        /// Saves content given the current locale
        /// </summary>
        /// <param name="userControl">The host control</param>
        /// <param name="content">The content</param>
        /// <param name="contentPath">The content path</param>
        static public void SaveContent(System.Web.UI.UserControl userControl, string content, string contentPath)
        {
            string locale = Helper.Content.LocalePrefix;
            Helper.Content.SaveContent(content, userControl.MapPath(Helper.Content.LocalizedContentPath(contentPath)));
        }


        /// <summary>
        /// Saves static content to a file
        /// </summary>
        /// <param name="content">The content to save</param>
        /// <param name="filePath"></param>	
        static private void SaveContent(string content, string filePath)
        {
            string directory = System.IO.Path.GetDirectoryName(filePath);
            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);

            System.IO.File.WriteAllText(filePath, content);
        }

        /// <summary>
        /// Creates a localized ID
        /// </summary>
        /// <param name="id">The base ID</param>
        /// <returns>The localized ID</returns>
        static public string LocalizedID(string id)
        {
            return LocalizedID(id, Helper.Content.LocalePrefix);
        }

        /// <summary>
        /// Creates a localized ID
        /// </summary>
        /// <param name="id">The base id</param>
        /// <param name="locale">The local</param>
        /// <returns>The localized ID</returns>
        static private string LocalizedID(string id, string locale)
        {
            return id + "_" + locale;
        }
        #endregion

        #region Content Paths
        /// <summary>
        /// Generates a localized content path (ie ~/Content/en/....)
        /// </summary>
        /// <param name="subContentPath">The path within a localized directory (ie 'images/image.jpg' which is located within the localized directory ~/Content/en/)</param>
        /// <param name="localPrefix">The prefix of the local (ie 'en', 'fr')</param>
        /// <returns>The full path to the localized content (ie ~/Content/en/images/image.jpg)</returns>
        static public string LocalizedContentPath(string subContentPath, string localPrefix)
        {
            subContentPath.TrimStart('/', '\\');
            return "~/Content/" + localPrefix + "/" + subContentPath;
        }

        /// <summary>
        /// Maps a localized content path for the active local
        /// </summary>
        /// <param name="subContentPath">The path within a localized directory (ie 'images/image.jpg' which is located within the localized directory ~/Content/en/)</param>
        /// <returns>The full path to the localized content (ie ~/Content/en/images/image.jpg) assuming the current local prefix is english</returns>
        static public string LocalizedContentPath(string subContentPath)
        {
            return Helper.Content.LocalizedContentPath(subContentPath, Helper.Content.LocalePrefix);
        }

        /// <summary>
        /// Returns the full path for a specific content path using the active users local
        /// </summary>
        /// <param name="contentPath">The content path</param>
        /// <returns>The full mapped path</returns>
        static public string FullPath(string contentPath)
        {
            return System.Web.HttpContext.Current.Server.MapPath(Helper.Content.LocalizedContentPath(contentPath));
        }

        /// <summary>
        /// Creates a full content path to un localized content
        /// </summary>
        /// <param name="subContentPath">The path from within the content directory (ie 'images/image.jpg' which is located within the content directory ~/Content/</param>
        /// <returns>The full path to the content (ie ~/Content/Images/image.jpg</returns>
        static public string ContentPath(string subContentPath)
        {
            subContentPath.TrimStart('/', '\\');
            return "~/Content/" + subContentPath;
        }
        #endregion

        #region Xsl Loading
        /// <summary>
        /// Loads a XSL Transform
        /// </summary>
        /// <param name="transformPath">The transform path (non localized)</param>
        /// <returns>The xsl transform (either from cache or from file)</returns>
        static public System.Xml.Xsl.XslCompiledTransform LoadTransform(string transformPath)
        {
            string localizedID = Helper.Content.LocalizedID(transformPath);
            System.Xml.Xsl.XslCompiledTransform transform = CacheHelper.Retrieve(localizedID) as System.Xml.Xsl.XslCompiledTransform;
            if (transform == null)
            {
                // load the transform from file
                string filePath = Helper.Content.FullPath(transformPath);
                if (System.IO.File.Exists(filePath))
                {
                    System.Xml.XmlUrlResolver resolver = new System.Xml.XmlUrlResolver();
                    transform = new System.Xml.Xsl.XslCompiledTransform();
                    transform.Load(filePath, System.Xml.Xsl.XsltSettings.TrustedXslt, resolver);
                }
                else
                    throw new System.IO.FileNotFoundException(string.Format("The transform '{0}' could not be found", filePath), filePath);

                // put the transform in the cache so we dont create this object every time
                if (transform != null)
                {
                    string directory = System.IO.Path.GetDirectoryName(filePath);
                    string[] files = System.IO.Directory.GetFiles(directory, "*.xsl");
                    System.Web.Caching.CacheDependency dependency = new System.Web.Caching.CacheDependency(files);
                    CacheHelper.Insert(localizedID, transform, dependency);
                }
            }
            return transform;
        }
        #endregion

        #region Image Helper Methods
        /// <summary>
        /// Gets the binary data of an image
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="format">The format</param>
        /// <returns>The binary data</returns>
        public static byte[] GetImageData(System.Drawing.Image img, ImageFormat format)
        {
            using (System.IO.MemoryStream memStream = new System.IO.MemoryStream())
            {
                img.Save(memStream, format);
                return memStream.ToArray();
            }
        }



        /// <summary>
        /// Creates a scaled down image that is less then the specified dimensions without affecting the aspect ratio
        /// </summary>
        /// <param name="img">The source image</param>
        /// <param name="maxWidth">The new max width</param>
        /// <param name="maxHeight">The new max hight</param>
        /// <returns>The resized image</returns>
        public static System.Drawing.Image GetScaleDownImage(System.Drawing.Image img, int maxWidth, int maxHeight)
        {
            if (img.Width < maxWidth)
                maxWidth = img.Width;
            if (img.Height < maxHeight)
                maxHeight = img.Height;

            return Inaugura.Drawing.ImageHelper.Resize(img, maxWidth, maxHeight);
        }

        public enum ImageMode
        {
            Size80,
            Size160,
            Size200,
            Size320,
            Size480,
            Size640,
            Size800,
            Size1024
        }

        /// <summary>
        /// Returns the url of a listing image file
        /// </summary>
        /// <param name="fileID">The file ID</param>
        /// <returns>The url</returns>
        public static string GetListingImageUrl(string fileID, ImageMode mode)
        {
            string strMode = string.Empty;

            if (mode == ImageMode.Size80)
                strMode = "80";
            else if (mode == ImageMode.Size200)
                strMode = "200";
            else if (mode == ImageMode.Size320)
                strMode = "320";
            else if (mode == ImageMode.Size480)
                strMode = "480";
            else if (mode == ImageMode.Size640)
                strMode = "640";
            else if (mode == ImageMode.Size1024)
                strMode = "1024";
            else if (mode == ImageMode.Size160)
                strMode = "160";

            if (fileID != null)
                return string.Format("~/ImageHandler.ashx?id={0}&mode={1}", fileID, strMode);
            else
                return null;
        }

        /// <summary>
        /// Returns the url of a listing image
        /// </summary>
        /// <param name="img">The listing image</param>
        /// <returns>The url</returns>
        public static string GetListingImageUrl(Inaugura.RealLeads.Image img, ImageMode mode)
        {
            if (img != null)
                return Helper.Content.GetListingImageUrl(img.FileID.ToString(), mode);
            else
                return null;
        }

        /// <summary>
        /// Returns the url of a listing image
        /// </summary>
        /// <param name="listing">The listing</param>
        /// <returns>The url</returns>
        public static string GetListingImageUrl(Inaugura.RealLeads.Listing listing, ImageMode mode)
        {
            if (listing != null)
                return Helper.Content.GetListingImageUrl(listing.Images.DefaultImage, mode);
            else
                return null;
        }
        #endregion
    }
#endregion

    #region Variables
    private const string APIStoreKey = "API";
    #endregion

    #region Properties
    /// <summary>
    /// Gets the inaugura API
    /// </summary>
    /// <value></value>
    static public Inaugura.RealLeads.RealLeadsAPI API
    {
        get
        {
            Inaugura.RealLeads.RealLeadsAPI api = System.Web.HttpContext.Current.Application[APIStoreKey] as Inaugura.RealLeads.RealLeadsAPI;
            if (api == null)
            {
                Inaugura.RealLeads.Data.IRealLeadsDataAdaptor data = new Inaugura.RealLeads.Data.SQLAdaptor(System.Configuration.ConfigurationManager.ConnectionStrings["InauguraData"].ConnectionString, System.Configuration.ConfigurationManager.AppSettings["FileDirectory"]);
                Inaugura.RealLeads.Data.Cached.CachedAdaptor cachedData = new Inaugura.RealLeads.Data.Cached.CachedAdaptor(data, new Inaugura.Caching.WebCache(TimeSpan.FromSeconds(10)));
                api = new Inaugura.RealLeads.RealLeadsAPI(cachedData);
                HttpContext.Current.Application.Add(APIStoreKey, api);
            }
            return api;
        }
    }
    #endregion

    #region Methods
    public static string CloseAndRefreshResponse
    {
        get
        {
            return ScriptHelper.CreateJavaScriptBlock("opener.location.reload();self.close()");
        }
    }

    public static string OpenPopupResponse(string url, string windowName, string width, string height, bool toolBar, bool menuBar, bool locationBar)
    {
        string str = string.Format("newWindow = window.open(\"{0}\", \"{1}\", \"width={2},height={3}, toolbar={4},menubar={5},locationbar={6}\");", url, windowName, width, height, toolBar ? "1" : "0", menuBar ? "1" : "0", locationBar ? "1" : "0");
        str = str + "newWindow.focus();";
        return ScriptHelper.CreateJavaScriptBlock(str);
    }

    public static string OpenPopupResponse(string url, string windowName, string width, string height)
    {
        return OpenPopupResponse(url, windowName, width, height, false, false, false);
    }

    public static void SelectListItem(ListControl control, string value)
    {
        control.ClearSelection();
        ListItem item = control.Items.FindByValue(value);
        if (item != null)
        {
            item.Selected = true;
        }

        /*if (item != null)
        {
            int index = control.Items.IndexOf(item);
            control.SelectedIndex = index;
        }
         */
    }

    public static void FillTypes(DropDownList control, Inaugura.RealLeads.Types[] types)
    {
        control.Items.Clear();
        foreach (Inaugura.RealLeads.Types type in types)
        {
            ListItem item = new ListItem(type.Name, type.Value.ToString());
            control.Items.Add(item);
        }
    }

    public static void PopulateCell(System.Web.UI.Control control, string text)
    {
        string[] paras = text.Split('\n');
        foreach (string para in paras)
        {
            HtmlGenericControl ctrl = new HtmlGenericControl("p");
            ctrl.InnerText = para;
            control.Controls.Add(ctrl);
        }
    }

    public static void SetCookie(string name, string value, TimeSpan expiration)
    {
        // create cookies to store the username and datasource
        System.Web.HttpCookie cookie = new System.Web.HttpCookie(name, value);
        cookie.Expires = DateTime.Now + expiration;
        System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
    }

    public static string GetCookie(string name)
    {
        if (System.Web.HttpContext.Current.Request.Cookies[name] != null)
            return System.Web.HttpContext.Current.Request.Cookies[name].Value;
        else
            return null;
    }

    public static void SendMessage(System.Net.Mail.MailMessage message)
    {
        System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();                
        // if the from address is null, assume system email
        if (message.From == null)
            message.From = new System.Net.Mail.MailAddress(Helper.Configuration.SystemEmail);        
        client.Send(message);
    }

    /// <summary>
    /// Sends an email verification message to the users email address
    /// </summary>
    /// <param name="user">The users to send the email verification message to</param>
    public static void SendEmailVerificationMessage(Inaugura.RealLeads.User user)
    {    
        string htmlContent = Helper.Content.LoadContent("Mailouts/VerifyEmailAddress.htm");
        //TODO remove the rentleads reference somehow
        htmlContent = htmlContent.Replace("[confirmationLink]",string.Format("http://www.rentleads.ca/VerifyEmail.aspx?userId={0}&key={1}", user.ID, user.Details["emailVerificationKey"]));
        
        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

        message.Subject = "Email Address Confirmation";
        message.To.Add(user.Email);

        message.IsBodyHtml = true;
        message.Priority = System.Net.Mail.MailPriority.High;
        message.Body = htmlContent;
        Helper.SendMessage(message);
    }
       
    public static Inaugura.Threading.ManagedThreadPool ThreadPool
    {
        get
        {
            if (System.Web.HttpContext.Current.Application["threadPool"] == null)
                System.Web.HttpContext.Current.Application["threadPool"] = new Inaugura.Threading.ManagedThreadPool(5);

            return System.Web.HttpContext.Current.Application["threadPool"] as Inaugura.Threading.ManagedThreadPool;
        }
    }

    /// <summary>
    /// Creates a close and update response which caues the listing content section of the opener window to be updated
    /// </summary>
    /// <param name="listingID">The listing ID which was updated</param>
    /// <returns>The javascript string</returns>
    public static string GetCloseAndUpdateResponse(string listingID)
    {
        return ScriptHelper.CreateJavaScriptBlock(string.Format("UpdateContentOpenerWindow('{0}');self.close()", listingID));
    }


    public static string MakeFirstUpper(string str)
    {
        bool needUpperCase = true;
        char[] letters = str.ToLower().ToCharArray();
        for (int i = 0; i < letters.Length; i++)
        {
            char letter = letters[i];
            if (char.IsLetter(letter))
            {
                if (needUpperCase)
                {
                    letters[i] = char.ToUpper(letter);
                    needUpperCase = false;
                }
            }
            else
                needUpperCase = true;
        }
        return new string(letters);
    }
#endregion
}