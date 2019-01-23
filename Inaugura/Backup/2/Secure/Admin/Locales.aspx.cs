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

public partial class Secure_Admin_Locales : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Master.SetBodyContent("Admin.html");
        if (!this.IsPostBack)
        {
            this.mDlType.DataSource = Enum.GetNames(typeof(Inaugura.Maps.Locale.LocaleType));
            this.mDlType.DataBind();

            this.FillLocales();
        }
    }

    protected void mBtnAdd_Click(object sender, EventArgs e)
    {
        // the result message
        string resultMsg = string.Empty;

        this.mLbGeocodeError.Text = string.Empty;

        if(this.mTxtName.Text.Length == 0)
        {
            resultMsg = "You must provide a name for the locale";
        }

        double radius = 0;
        if(!double.TryParse(this.mTxtRadius.Text, out radius))
        {
            resultMsg = "You must specify a radius for the locale";
        }
       
        if(resultMsg != string.Empty)
        {
            this.mLbGeocodeError.Text = resultMsg;
            return;
        }
        
        //  Try and get the address
        Inaugura.Maps.Address address = Helper.API.LocatePostal(this.mTxtPostal.Text);

        if (address == null)
        {
            this.Master.ShowMessage("Unable to locate postal code");
            return;
        }

        address.Street = this.mTxtStreet.Text;
        
        // create the Locale
        Inaugura.Maps.Locale locale = new Inaugura.Maps.Locale(this.mTxtName.Text, address, radius);
              
        Inaugura.Maps.Locale.LocaleType localeType = (Inaugura.Maps.Locale.LocaleType)Enum.Parse(typeof(Inaugura.Maps.Locale.LocaleType), this.mDlType.SelectedItem.Value);
        locale.Type = localeType;

        Helper.API.AddressManager.AddLocale(locale);        

        this.FillLocales();

        this.mTxtStreet.Text = string.Empty;
        this.mTxtName.Text = string.Empty;
        this.mTxtPostal.Text = string.Empty;
    }

    private void FillLocales()
    {
        Inaugura.Maps.Locale[] locales = Helper.API.AddressManager.GetLocales(Inaugura.Maps.Locale.LocaleType.All);
        this.mLstLocales.Items.Clear();

        foreach (Inaugura.Maps.Locale locale in locales)
        {
            this.mLstLocales.Items.Add(new ListItem(locale.Name, locale.ID));
        }
    }
}
