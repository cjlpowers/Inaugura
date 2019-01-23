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
/// Summary description for MembershipProvider
/// </summary>
public class MembershipProvider : System.Web.Security.MembershipProvider
{
    #region Variables
    private string mPasswordStrengthRegex ="@\\\"(?=.{6,})";

    #endregion


    public MembershipProvider() 
    {      
    }

    public override string ApplicationName
    {
        get
        {
            return "RealLeads";
        }
        set
        {           
        }
    }

    public override bool ChangePassword(string username, string oldPassword, string newPassword)
    {
        OnValidatingPassword(new ValidatePasswordEventArgs(username, newPassword, false));

        System.Web.Security.MembershipUser u = GetUser(username, false);
        if (u == null)
            return false;

        MembershipUser user = u as MembershipUser;
        if (user.User.Password.IsMatch(oldPassword))
        {
            try
            {
                user.User.SetPassword(newPassword);

                Helper.API.UserManager.UpdateUser(user.User);
                return true;
            }
            catch (Inaugura.Security.SecurityException ex)
            {
                return false;
            }
        }
        return false;
    }

    public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
    {
        System.Web.Security.MembershipUser u = GetUser(username, false);
        if (u == null)
            return false;

        MembershipUser user = u as MembershipUser;
        if (user.User.Password.IsMatch(password))
        {
            user.User.PasswordQuestion = newPasswordQuestion;
            user.User.PasswordAnswer = newPasswordAnswer;
            Helper.API.UserManager.UpdateUser(user.User);
            return true;
        }
        return false;
    }

    public override System.Web.Security.MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool DeleteUser(string username, bool deleteAllRelatedData)
    {
        System.Web.Security.MembershipUser u = GetUser(username, false);
        if (u == null)
            return false;

        MembershipUser user = u as MembershipUser;
        Helper.API.UserManager.RemoveUser(user.User);
        return true;
    }

    public override bool EnablePasswordReset
    {
        get 
        {
            return true;
        }
    }

    public override bool EnablePasswordRetrieval
    {
        get 
        {
            return false;
        }
    }

    public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
    {
        totalRecords = 0;
        return new MembershipUserCollection();        
    }

    public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
    {
        totalRecords = 0;
        return new MembershipUserCollection();
    }

    public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
    {
        totalRecords = 0;
        return new MembershipUserCollection();
    }

    public override int GetNumberOfUsersOnline()
    {
        return 0;
    }

    public override string GetPassword(string username, string answer)
    {
        throw new System.Configuration.Provider.ProviderException("Password retrieval is not supported");
     }

    public override System.Web.Security.MembershipUser GetUser(string username, bool userIsOnline)
    {
        Inaugura.RealLeads.User user = Helper.API.UserManager.GetUserByEmail(username);
        if (user == null)
            return null;
        else
            return new MembershipUser(user);
    }

    public override System.Web.Security.MembershipUser GetUser(object providerUserKey, bool userIsOnline)
    {
        return GetUser(providerUserKey.ToString(), userIsOnline);
    }

    public override string GetUserNameByEmail(string email)
    {
        Inaugura.RealLeads.User user = Helper.API.UserManager.GetUserByEmail(email);
        if (user == null)
            return string.Empty;
        else
            return user.Email;
    }

    public override int MaxInvalidPasswordAttempts
    {
        get { return 5; }
    }

    public override int MinRequiredNonAlphanumericCharacters
    {
        get { return 0; }
    }

    public override int MinRequiredPasswordLength
    {
        get { return 6; }
    }

    public override int PasswordAttemptWindow
    {
        get { return 5; }
    }

    public override MembershipPasswordFormat PasswordFormat
    {
        get { return MembershipPasswordFormat.Hashed; }
    }

    public override string PasswordStrengthRegularExpression
    {
        get 
        {
            return this.mPasswordStrengthRegex;
        }      
    }

    public override bool RequiresQuestionAndAnswer
    {
        get { return true; }
    }

    public override bool RequiresUniqueEmail
    {
        get { return true; }
    }

    public override string ResetPassword(string username, string answer)
    {
        if (!this.EnablePasswordReset)
            throw new NotSupportedException();

        System.Web.Security.MembershipUser u = GetUser(username,false);
        if(u == null)
            throw new MembershipPasswordException("User not found");

        MembershipUser user = u as MembershipUser;

        if (answer.ToLower() != user.User.PasswordAnswer.ToLower())
            throw new MembershipPasswordException("Incorrect answer");

        return Membership.GeneratePassword(7, 0);
    }

    public override bool UnlockUser(string userName)
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void UpdateUser(System.Web.Security.MembershipUser user)
    {
        if (user is MembershipUser)
        {
            MembershipUser ms = user as MembershipUser;
            Helper.API.UserManager.UpdateUser(ms.User);
        }
    }

    public override bool ValidateUser(string username, string password)
    {
        System.Web.Security.MembershipUser u = this.GetUser(username, false);
        if (u is MembershipUser)
        {
            MembershipUser user = u as MembershipUser;
            return user.User.Password.IsMatch(password);
        }
        else
            return false;
    }
}
