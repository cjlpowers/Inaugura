using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.RealLeads
{
    public class UserPrincipal : System.Security.Principal.IPrincipal
    {
        #region Variables
        private UserIdentity mUserIdentity;
        #endregion

        #region Properties
        #endregion

        #region Methods
        public UserPrincipal(User user)
        {
            this.mUserIdentity = new UserIdentity(user);         
        }
        #endregion

        #region IPrincipal Members

        public System.Security.Principal.IIdentity Identity
        {
            get 
            {
                return this.mUserIdentity;
            }
        }

        public bool IsInRole(string role)
        {
            return this.mUserIdentity.IsInRole(role);
        }
        #endregion
    }
}
