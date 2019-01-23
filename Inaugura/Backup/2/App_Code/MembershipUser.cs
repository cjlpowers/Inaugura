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
/// Summary description for MembershipUser
/// </summary>
public class MembershipUser : System.Web.Security.MembershipUser
{
    #region Variables
    private Inaugura.RealLeads.User mUser;
    #endregion

    #region Properties
    public Inaugura.RealLeads.User User
    {
        get
        {
            return this.mUser;
        }
    }

    public override string UserName
    {
        get
        {
            return this.mUser.Email;
        }
    }

    public override DateTime CreationDate
    {
        get
        {
            return this.mUser.CreationDate;
        }
    }

    public override string Email
    {
        get
        {
            return this.mUser.Email;
        }
        set
        {
            this.mUser.Email = value;
        }
    }

    public override string PasswordQuestion
    {
        get
        {
            return this.mUser.PasswordQuestion;
        }
    }
    #endregion

    #region Methods
    public MembershipUser(Inaugura.RealLeads.User user) : base()
    {
        this.mUser = user;
    }

    public override bool ChangePassword(string oldPassword, string newPassword)
    {
        return Membership.Provider.ChangePassword(this.UserName, oldPassword, newPassword);
    }
    #endregion
}
