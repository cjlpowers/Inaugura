using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.RealLeads.Managers
{
    public class UserManager : Manager
    {
        #region Method
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="api">The RealLeads API object</param>
        /// <param name="dataAdaptor">The data adaptor</param>     
        internal UserManager(RealLeadsAPI api, Data.IRealLeadsDataAdaptor dataAdaptor)
            : base(api, dataAdaptor)
        {
        }

        /// <summary>
        /// Gets a list of all roles
        /// </summary>
        /// <returns></returns>
        public Inaugura.Security.Role[] GetRoles()
        {
            return this.Data.UserStore.GetRoles();
        }

        /// <summary>
        /// Adds a role
        /// </summary>
        /// <param name="role"></param>
        public void AddRole(Inaugura.Security.Role role)
        {
            this.Data.UserStore.AddRole(role);
        }

        /// <summary>
        /// Removes a role
        /// </summary>
        /// <param name="role"></param>
        public void RemoveRole(Inaugura.Security.Role role)
        {
            this.Data.UserStore.RemoveRole(role);
        }

        /// <summary>
        /// Gets a user based on their ID
        /// </summary>
        /// <param name="id">The ID of the user</param>
        public User GetUser(Guid id)
        {
            return this.Data.UserStore.GetUser(id);
        }

        /// <summary>
        /// Gets a user by their email address
        /// </summary>
        /// <param name="email">The email address of the user</param>
        public User GetUserByEmail(string email)
        {
            // only cache based on ID
            return this.Data.UserStore.GetUserByEmail(email);
        }

        /// <summary>
        /// Adds a new user
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(User user)
        {
            // add the user to the database
            this.Data.UserStore.Add(user);

            // TODO send a notification email
        }

        /// <summary>
        /// Updates a user
        /// </summary>
        /// <param name="user">The user to update</param>
        public void UpdateUser(User user)
        {
            this.Data.UserStore.Update(user);
        }

        /// <summary>
        /// Removes a user
        /// </summary>
        /// <param name="user"></param>
        public void RemoveUser(User user)
        {
            this.Data.UserStore.Remove(user);
        }


        /// <summary>
        /// Performs a search for users
        /// </summary>
        /// <param name="search">The search criteria</param>
        /// <returns>The users matching the search criteria</returns>
        public User[] SearchUsers(UserSearch search)
        {
            return this.Data.UserStore.SearchUsers(search);
        }


        /// <summary>
        /// Determins if a email is registered
        /// </summary>
        /// <param name="email">The email to check</param>
        /// <returns>True if registered, false otherwise</returns>
        public bool IsEmailRegistered(string email)
        {
            if (this.Data.UserStore.GetUserByEmail(email) != null)
                return true;

            return false;
        }

        #region Promotions
        /// <summary>
        /// Gets a list of promotions
        /// </summary>
        /// <returns>The list of promotion</returns>
        public Promotion[] GetPromotions()
        {
            return this.Data.UserStore.GetPromotions();
        }

        /// <summary>
        /// Gets a promotion
        /// </summary>
        /// <param name="id">The promition ID</param>
        /// <returns>The promotion if found, otherwise null</returns>
        public Promotion GetPromotion(Guid id)
        {
            return this.Data.UserStore.GetPromotion(id);
        }

        /// <summary>
        /// Gets a promotion with a specific code
        /// </summary>
        /// <param name="id">The promition code</param>
        /// <returns>The promotion if found, otherwise null</returns>
        public Promotion GetPromotionByCode(string code)
        {
            return this.Data.UserStore.GetPromotionByCode(code);
        }

        /// <summary>
        /// Adds a promotion
        /// </summary>
        /// <param name="promotion">The promotion to add</param>
        public void AddPromotion(Promotion promotion)
        {
            this.Data.UserStore.AddPromotion(promotion);
        }

        /// <summary>
        /// Updates a promotion
        /// </summary>
        /// <param name="promotion">The promotion to update</param>
        public void UpdatePromotion(Promotion promotion)
        {
            this.Data.UserStore.UpdatePromotion(promotion);
        }

        /// <summary>
        /// Removes a promotion
        /// </summary>
        /// <param name="promotion">The promotion to remove</param>
        public void RemovePromotion(Promotion promotion)
        {
            this.Data.UserStore.RemovePromotion(promotion.ID);
        }
        #endregion
        #endregion
    }
}
