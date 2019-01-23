using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Inaugura.Web.Controls
{
    /// <summary>
    /// Summary description for ListRadioButton
    /// </summary>
    public class ListRadioButton : System.Web.UI.WebControls.RadioButton
    {
        #region Properties
        #endregion

        public ListRadioButton()          
        {          
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.WriteBeginTag("input");
            writer.WriteAttribute("id", this.ClientID);
            writer.WriteAttribute("value", this.ClientID);
            writer.WriteAttribute("type", "radio");
            writer.WriteAttribute("name", this.GroupName);

            if (Checked)
                writer.WriteAttribute("checked", "checked");

            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteEndTag("input");
        }
    }
}