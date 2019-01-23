using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.RealLeads
{
    public class UserIdentity : System.Security.Principal.IIdentity
    {
        #region Variables
        private User mUser;
        #endregion
        
        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user">The user</param>
        public UserIdentity(User user)
        {
            this.mUser = user;
        }

        /// <summary>
        /// Test wheather the user is in a specific role
        /// </summary>
        /// <param name="role">The role</param>
        /// <returns>True if in the role, otherwise false</returns>
         public bool IsInRole(string role)
        {
            return this.mUser.IsInRole(role);
        }
        #endregion

        #region IIdentity Members
        public string AuthenticationType
        {
            get { return "RealLeads"; }
        }

        public bool IsAuthenticated
        {
            get { return this.mUser.Email.Length > 0; }
        }

        public string Name
        {
            get { return this.mUser.Email; }
        }

       
        #endregion
    }
}
