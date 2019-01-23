using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Inaugura.Web.Controls
{

    /// <summary>
    /// Summary description for CustomPlaceHolder
    /// </summary>
    [
        AspNetHostingPermission(SecurityAction.Demand,
            Level = AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand,
            Level = AspNetHostingPermissionLevel.Minimal)        
        ]
    public class ContentInserter : PlaceHolder
    {
        #region Variables
        public string mContent;
        #endregion

        #region Properties
        [
            Bindable(true),
            Category("Content"),
            DefaultValue(""),
            Description("The content to insert.")
            ]
        public string Content
        {
            get
            {
                return this.mContent;
            }
            set
            {
                this.mContent = value;
            }
        }
        #endregion

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(Content);
        }
    }
}
