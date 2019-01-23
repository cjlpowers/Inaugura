using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.RealLeads.Security
{
    /// <summary>
    /// The BOSS implementation of the IIdentity internface
    /// </summary>
    public class UserIdentity : System.Security.Principal.IIdentity
    {
        #region Variables
        private User mUser;
        #endregion

        #region Properties
        /// <summary>
        /// The underlying RealLeads User
        /// </summary>
        internal User User
        {
            get
            {
                return this.mUser;
            }
        }
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
        /// <param name="context">The security context</param>
        /// <returns>True if in the role, otherwise false</returns>
        public bool IsInRole(string role)
        {
            if(mUser!=null)
                return this.mUser.IsInRole(role);
            return false;
        }
        #endregion

        #region IIdentity Members
        /// <summary>
        /// The authentication type
        /// </summary>
        public string AuthenticationType
        {
            get { return "RealLeads"; }
        }

        /// <summary>
        /// A flag which indicates if the user is authenticated
        /// </summary>
        public bool IsAuthenticated
        {
            get { return this.mUser.Email.Length > 0; }
        }

        /// <summary>
        /// The user name
        /// </summary>
        public string Name
        {
            get { return this.mUser.Email; }
        }       
        #endregion
    }
}
