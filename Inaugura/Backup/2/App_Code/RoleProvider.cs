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
/// Summary description for RoleProvider
/// </summary>
public class RoleProvider : System.Web.Security.RoleProvider
{
    public RoleProvider()
    {
    }

    public override void AddUsersToRoles(string[] usernames, string[] roleNames)
    {
        System.Collections.Generic.List<Inaugura.RealLeads.User> users = new System.Collections.Generic.List<Inaugura.RealLeads.User>();        
        foreach (string username in usernames)
        {
            Inaugura.RealLeads.User user = Helper.API.UserManager.GetUserByEmail(username);
            if (user == null)
                throw new Exception(string.Format("Unable to find the user {0}", username));

            users.Add(user);
        }

        Inaugura.Security.Role[] allRoles = Helper.API.UserManager.GetRoles();
        System.Collections.Generic.List<Inaugura.Security.Role> validRoles = new System.Collections.Generic.List<Inaugura.Security.Role>();

        foreach(string roleName in roleNames)
        {
           foreach(Inaugura.Security.Role role in allRoles)
           {
               if(role.Name == roleName)
               {
                   validRoles.Add(role);
                   break;
               }
           }
        }

        foreach(Inaugura.RealLeads.User user in users)
        {
            foreach (Inaugura.Security.Role role in validRoles)
                user.AddRole(role);

            Helper.API.UserManager.UpdateUser(user);
        }
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

    public override void CreateRole(string roleName)
    {
        Helper.API.UserManager.AddRole(new Inaugura.Security.Role(roleName));
    }

    public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
    {
        //TODO handle populated roles
        Helper.API.UserManager.RemoveRole(new Inaugura.Security.Role(roleName));
        return true;
    }

    public override string[] FindUsersInRole(string roleName, string usernameToMatch)
    {
        return new string[0];
        //throw new Exception("The method or operation is not implemented.");
    }

    public override string[] GetAllRoles()
    {
        Inaugura.Security.Role[] roles = Helper.API.UserManager.GetRoles();
        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        foreach (Inaugura.Security.Role role in roles)
            list.Add(role.Name);
        return list.ToArray();
    }

    public override string[] GetRolesForUser(string username)
    {
        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        Inaugura.RealLeads.User user = SessionHelper.User;
        if (user == null)
            user = Helper.API.UserManager.GetUserByEmail(username);
        if (user != null)
        {
            foreach (Inaugura.Security.Role role in user.Roles)
                list.Add(role.Name);
        }
        return list.ToArray();
    }

    public override string[] GetUsersInRole(string roleName)
    {
        return new string[0];
        //throw new Exception("The method or operation is not implemented.");
    }

    public override bool IsUserInRole(string username, string roleName)
    {
        Inaugura.RealLeads.User user = SessionHelper.User;
        if(user == null)
            user = Helper.API.UserManager.GetUserByEmail(username);
        if (user != null)
        {
            foreach (Inaugura.Security.Role role in user.Roles)
            {
                if (role.Name == roleName)
                    return true;
            }
        }
        return false;
    }

    public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
    {
        //throw new Exception("The method or operation is not implemented.");
    }

    public override bool RoleExists(string roleName)
    {
        string[] allRoles = GetAllRoles();
        foreach (string name in allRoles)
        {
            if (name == roleName)
                return true;
        }
        return false;
    }
}
