using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.RealLeads.Security
{
    /// <summary>
    /// The BOSS Implementation of the IPrincipal interface
    /// </summary>
    public class UserPrincipal : System.Security.Principal.IPrincipal
    {
        #region Variables
        private UserIdentity mUserIdentity;
        #endregion

        #region Properties
        /// <summary>
        /// Determins if the current principal of the active threading context is a RealLeads UserPrinciple instance.
        /// If we are not in such a context, security should not be enforced since it is likely a background process (ASP.NET for example) currently
        /// defines the context which in the program is run.
        /// </summary>
        /// <returns>True if in a secure context, false otherwise</returns>
        public static bool SecureContext
        {
            get
            {
                if (System.Threading.Thread.CurrentPrincipal is UserPrincipal)
                    return true;
                return false;
            }
        }       

        
        /// <summary>
        /// The underlying RealLeads User
        /// </summary>
        public static User CurrentUser
        {
            get
            {
                if(System.Threading.Thread.CurrentPrincipal is UserPrincipal)
                {
                    UserPrincipal up = System.Threading.Thread.CurrentPrincipal as UserPrincipal;
                    return up.mUserIdentity.User;
                }
                return null;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="context">The security context</param>
        public UserPrincipal(User user)
        {
            this.mUserIdentity = new UserIdentity(user);
        }

        /// <summary>
        /// Sets the identity of the current thread context
        /// </summary>
        /// <param name="user">The user</param>
        public static void SetIdentity(User user)
        {
            UserPrincipal principal  = new UserPrincipal(user);
            System.Threading.Thread.CurrentPrincipal = principal;
        }
        #endregion

        #region IPrincipal Members
        /// <summary>
        /// The Identity object
        /// </summary>
        public System.Security.Principal.IIdentity Identity
        {
            get 
            {
                return this.mUserIdentity;
            }
        }

        /// <summary>
        /// Determins if the user is in a specific role
        /// </summary>
        /// <param name="role">The role name</param>
        /// <returns>The role name</returns>
        public bool IsInRole(string role)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Checking if user is in role '{0}'",role));
            return this.mUserIdentity.IsInRole(role);
        }
        #endregion
    }
}
