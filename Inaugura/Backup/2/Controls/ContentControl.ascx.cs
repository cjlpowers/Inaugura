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

public partial class ContentControl : System.Web.UI.UserControl
{
    #region Variables
    private string mContentKey;
    private string mContentPath;
    #endregion

    #region Properties
    /// <summary>
    /// The path to the content
    /// </summary>
    public string ContentPath
    {
        get
        {
            return this.mContentPath;
        }
        set
        {
            this.mContentPath = value;
            this.Editor1.Html = this.LoadContent();
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {       
    }

     protected void Page_Init(object sender, EventArgs e)
    {
        this.Editor1.SubmitClicked += new EventHandler(Editor1_SubmitClicked);               
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.Editor1.HasPermission = Helper.Content.CanEdit;
    }

    void Editor1_SubmitClicked(object sender, EventArgs e)
    {
        if (this.ContentPath == null)
            throw new ArgumentNullException("The content path has not been set");
        Helper.Content.SaveContent(this, this.Editor1.Html, this.ContentPath);
        this.Editor1.Editable = false;
    }

    private string LoadContent()
    {
        string key = Helper.Content.LocalizedID(this.ContentPath);
        object content = CacheHelper.Retrieve(key);
        if (content == null)
        {
            string strContent;
            try
            {
                strContent = Helper.Content.LoadContent(this.ContentPath);                
            }
            catch (System.IO.FileNotFoundException ex)
            {
                return ex.Message;
            }
            
            // cache it
            System.Web.Caching.CacheDependency dependency = new System.Web.Caching.CacheDependency(Helper.Content.FullPath(this.ContentPath));
            CacheHelper.Insert(key, strContent, dependency);
            return strContent;
        }
        else
            return content as string;
    }
}
