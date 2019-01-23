using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.Security
{
    /// <summary>
    /// A class which defines a security role
    /// </summary>
    public class Role: Xml.IXmlable
    {
        #region Variables
        private string mName;
        #endregion

        #region Properties
        /// <summary>
        /// The name of the role
        /// </summary>
        public string Name
        {
            get
            {
                return this.mName;
            }
            protected set
            {
                this.mName = value;
            }
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the role</param>
        public Role(string name)
        {
            this.mName = name;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">The xml representation of the object</param>
        public Role(XmlNode node)
        {
            this.PopulateInstance(node);
        }

        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public virtual void PopulateNode(XmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node", "The Xml node may not be null");

            Inaugura.Xml.Helper.SetAttribute(node, "name", this.Name);
        }

        /// <summary>
        /// Populates the instance with the specifed xml data
        /// </summary>
        /// <param name="node"></param>
        public virtual void PopulateInstance(XmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node", "The Xml definition may not be null");

            Inaugura.Xml.Helper.EnsureAttributeExists(node, "name");            
            this.Name = Inaugura.Xml.Helper.GetAttribute(node, "name");
        }

        /// <summary>
        /// The string representation
        /// </summary>
        /// <returns>The string representation</returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Enforces a role by throwing an exception if the current principal is not a member of the specified role
        /// </summary>
        /// <param name="role">The required role</param>
        public static void EnforceRole(Inaugura.Security.Role role)
        {
            EnforceRole(role.Name);
        }

        /// <summary>
        /// Enforces a role by throwing an exception if the current principal is not a member of the specified role
        /// </summary>
        /// <param name="roleName">The required role</param>
        public static void EnforceRole(string roleName)
        {
            if(!IsInRole(roleName))
                throw new Inaugura.Security.SecurityException(string.Format("Access denied. The principal of the active thread context must be a member of the {0} role", roleName));
        }
        
        /// <summary>
        /// Determins if the current principal is in a specific role
        /// </summary>
        /// <param name="role">The specific role</param>
        /// <returns>True if the principal is in the specified role, otherwise false</returns>
        public static bool IsInRole(Inaugura.Security.Role role)
        {
            return IsInRole(role.Name);
        }

        /// <summary>
        /// Determins if the current principal is in a specific role
        /// </summary>
        /// <param name="roleName">The specific role</param>
        /// <returns>True if the principal is in the specified role, otherwise false</returns>
        public static bool IsInRole(string roleName)
        {
            return System.Threading.Thread.CurrentPrincipal.IsInRole(roleName);
        }
        #endregion

        #region IXmlable Members
        /// <summary>
        /// Gets the xml representation of the Address
        /// </summary>
        /// <value></value>
        public XmlNode Xml
        {
            get
            {
                XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Role");
                this.PopulateNode(node);
                return node;
            }
        }
        #endregion
    }
}
