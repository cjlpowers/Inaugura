#region Using directives
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

using Inaugura.Maps;

using Inaugura.Data;
#endregion

namespace Inaugura.RealLeads.Data.Cached
{
    /// <summary>
    /// A cached implementation of the user store
    /// </summary>
    internal class UserStore : CachedStore, IUserStore
    {
        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="adaptor">The cached adaptor</param>
        public UserStore(CachedAdaptor adaptor)
            : base(adaptor)
        {
        }
        #endregion

        #region IUserStore Members

        public void Add(User user)
        {
            this.Data.UserStore.Add(user);
        }

        public User GetUser(Guid id)
        {
            Key key = CreateCacheKey(id);
            User item = Cache[key] as User;
            if (item == null)
            {
                item = this.Data.UserStore.GetUser(id);
                Cache[key] = item;
            }
            return item;
        }

        public User GetUserByEmail(string email)
        {
            Key key = CreateCacheKey("user",email);
            User item = Cache[key] as User;
            if (item == null)
            {
                item = this.Data.UserStore.GetUserByEmail(email);
                Cache[key] = item;
            }
            return item;
        }

        public void Update(User user)
        {
            this.Data.UserStore.Update(user);
            Key key = CreateCacheKey("user", user.Email);
            this.Cache.Remove(key);
            key = CreateCacheKey(user.ID);
            Cache.Remove(key);
        }

        public void Remove(User user)
        {
            Key key = CreateCacheKey("user", user.Email);
            this.Cache.Remove(key);
            key = CreateCacheKey(user.ID);
            Cache.Remove(key);
        }

        public User[] SearchUsers(UserSearch search)
        {
            return this.Data.UserStore.SearchUsers(search);
        }

        public void AddFile(string userId, File file)
        {
            this.Data.UserStore.AddFile(userId, file);
        }

        public File GetFile(string fileId)
        {
            return this.Data.UserStore.GetFile(fileId);
        }

        public bool RemoveFile(string fileId)
        {
            return this.RemoveFile(fileId);
        }

        public bool UpdateFile(File file)
        {
            return this.UpdateFile(file);
        }

        public Inaugura.Security.Role[] GetRoles()
        {
            Key key = CreateCacheKey("roles");
            Inaugura.Security.Role[] item = Cache[key] as Inaugura.Security.Role[];
            if (item == null)
            {
                item = this.Data.UserStore.GetRoles();
                Cache[key] = item;
            }
            return item;
        }

        public void AddRole(Inaugura.Security.Role role)
        {
            this.Data.UserStore.AddRole(role);
            Key key = CreateCacheKey("roles");
            Cache.Remove(key);
        }

        public void RemoveRole(Inaugura.Security.Role role)
        {
            this.Data.UserStore.RemoveRole(role);
            Key key = CreateCacheKey("roles");
            Cache.Remove(key);
        }

        public Message[] GetMessages(Guid userID)
        {
            return this.Data.UserStore.GetMessages(userID);
        }

        public Message GetMessage(Guid id)
        {
            return this.Data.UserStore.GetMessage(id);
        }

        public void AddMessage(Message message)
        {
            this.Data.UserStore.AddMessage(message);
        }

        public void UpdateMessage(Message message)
        {
            this.Data.UserStore.UpdateMessage(message);
        }

        public void RemoveMessage(string id)
        {
            this.Data.UserStore.RemoveMessage(id);
        }

        #endregion

        #region Promotions
        public Promotion[] GetPromotions()
        {
            return this.Data.UserStore.GetPromotions();
        }
       
        public Promotion GetPromotion(Guid id)
        {
            return this.Data.UserStore.GetPromotion(id);
        }
                
        public Promotion GetPromotionByCode(string code)
        {
            return this.Data.UserStore.GetPromotionByCode(code);
        }

        public void AddPromotion(Promotion promotion)
        {
            this.Data.UserStore.AddPromotion(promotion);
        }
                
        public void RemovePromotion(Guid id)
        {
            this.Data.UserStore.RemovePromotion(id);
        }

        public void UpdatePromotion(Promotion promotion)
        {
            this.Data.UserStore.UpdatePromotion(promotion);
        }
        #endregion
    }
}
