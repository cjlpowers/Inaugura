using System;
using System.Xml;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;


/// <summary>
/// Summary description for SessionHelper
/// </summary>
public static class SessionHelper
{
    #region Properties
    /// <summary>
    /// The active user
    /// </summary>
    static public Inaugura.RealLeads.User User
    {
        get
        {
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["User"] != null)
                return System.Web.HttpContext.Current.Session["User"] as Inaugura.RealLeads.User;
            else
                return null;
        }
        set
        {
            System.Web.HttpContext.Current.Session["User"] = value;
            // reset the menu
            Menu.MenuXml = null;
        }
    }

    /// <summary>
    /// The listing
    /// </summary>
    static public Inaugura.RealLeads.Listing Listing
    {
        get
        {
            if (System.Web.HttpContext.Current.Session["Listing"] != null)
                return (Inaugura.RealLeads.Listing)System.Web.HttpContext.Current.Session["Listing"];
            else
                return null;
        }
        set
        {
            System.Web.HttpContext.Current.Session["Listing"] = value;
        }
    }		
    #endregion

    #region Methods

    static public void RedirectTemporaryError()
    {
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        context.Response.Redirect("~/Content.aspx?target=actionunavailable");
        context.Response.End();
    }
    #endregion

    #region Internal Constructs

	/// <summary>
	/// A session helper class for agents
	/// </summary>
	public static class Agent
    {

        #region Internal Constructs
        /// <summary>
        /// A class which contains listings
        /// </summary>
        public class ListingCollection : System.Collections.Generic.List<Inaugura.RealLeads.Listing>
        {
            #region Accessors
            /// <summary>
            /// Accessor
            /// </summary>
            /// <param name="id">The id of the listing</param>
            /// <returns>The listing matching the ID otherwise null</returns>
            public Inaugura.RealLeads.Listing this[string id]
            {
                get
                {
                    Guid guid = new Guid(id);
                    foreach (Inaugura.RealLeads.Listing listing in this)
                    {
                        if (listing.ID == guid)
                            return listing;
                    }
                    return null;
                }
            }
            #endregion

            #region Methods
            public bool Contains(Guid listingID)
            {
                foreach (Inaugura.RealLeads.Listing listing in this)
                {
                    if (listing.ID == listingID)
                        return true;
                }
                return false;
            }
            #endregion
        }
        
        #endregion
      
		static public Inaugura.RealLeads.Listing ActiveListing
		{
			get
			{
				if (System.Web.HttpContext.Current.Session["Listing"] != null)
					return (Inaugura.RealLeads.Listing)System.Web.HttpContext.Current.Session["Listing"];
				else
					return null;
			}
			set
			{
				System.Web.HttpContext.Current.Session["Listing"] = value;
			}
		}		
	}

           

    /// <summary>
    /// A session helper class for menu functionality
    /// </summary>
    public static class Menu
    {
        private const string MenuKey = "Menu";

        #region Properties

        private static string MenuModePath
        {
            get
            {
                if (Helper.Configuration.Mode == Helper.Configuration.WebSiteMode.RealEstateListings)
                    return "RealLeads";
                else if (Helper.Configuration.Mode == Helper.Configuration.WebSiteMode.RentalPropertyListings)
                    return "RentLeads";
                else
                    throw new NotSupportedException("The website mode was not supported");
            }
        }

        private static XmlDocument CreateMenu()
        {
            // load the menu
            XmlDocument xmlDoc = LoadMenuXML("Menus/" + MenuModePath + "/Menu.xml");
            xmlDoc = xmlDoc.Clone() as XmlDocument;

            // figure out the users roles
            System.Collections.Generic.List<string> userRoles = new System.Collections.Generic.List<string>();

            if (SessionHelper.User == null)
                userRoles.Add("Anonymous");
            else
            {
                userRoles.Add("Authenticated");
                userRoles.AddRange(Roles.GetRolesForUser());
            }

            // now customize it
            ProcessChildNode(xmlDoc.DocumentElement, userRoles);
            return xmlDoc;
        }

        private static void ProcessChildNode(XmlNode node, System.Collections.Generic.List<string> userRoles)
        {
            System.Collections.Generic.List<XmlNode> nodesToRemove = new System.Collections.Generic.List<XmlNode>();
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Attributes != null && childNode.Attributes["roles"] != null)
                {
                    string[] nodeRoles = childNode.Attributes["roles"].Value.Split(';');
                    bool found = false;
                    foreach (string nodeRole in nodeRoles)
                    {
                        if (userRoles.Contains(nodeRole))
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                        nodesToRemove.Add(childNode);
                }
            }

            // remove the nodes
            foreach (XmlNode n in nodesToRemove)
            {
                node.RemoveChild(n);
            }

            foreach (XmlNode n in node.ChildNodes)
                ProcessChildNode(n, userRoles);
        }
       
      

        /// <summary>
        /// The menu for the active user
        /// </summary>
        static public XmlDocument MenuXml
        {
            get
            {
                XmlDocument xmlDoc = HttpContext.Current.Session[MenuKey] as XmlDocument;
                if (xmlDoc == null)
                {
                    xmlDoc = CreateMenu();
                    HttpContext.Current.Session.Add(MenuKey, xmlDoc);
                }
                return xmlDoc;                
            }
            set
            {
                HttpContext.Current.Session[MenuKey] = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Loads menu xml
        /// </summary>
        /// <param name="path">The non-localized path</param>
        static private XmlDocument LoadMenuXML(string path)
        {
            string key = Helper.Content.LocalizedID(path);
            object content = CacheHelper.Retrieve(key);
            if (content == null)
            {
                string strContent = Helper.Content.LoadContent(path);

                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(strContent);

                // cache it
                System.Web.Caching.CacheDependency dependency = new System.Web.Caching.CacheDependency(Helper.Content.FullPath(path));
                CacheHelper.Insert(key, xmlDoc, dependency);
                return xmlDoc;
            }
            else
                return content as System.Xml.XmlDocument;
        }
        #endregion
    }
    #endregion
}
