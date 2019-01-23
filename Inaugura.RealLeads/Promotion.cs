using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
    /// <summary>
    /// A class representing a promotion
    /// </summary>
    public class Promotion : Inaugura.Xml.IXmlable
    {
        #region Internal Constructs
        /// <summary>
        /// A class defining a promotion action
        /// </summary>
        public class Action : Inaugura.Xml.IXmlable
        {
            #region Variables
            private string mXPath;
            private string mValue;
            #endregion

            #region Properties
            /// <summary>
            /// The xpath selector
            /// </summary>
            public string XPath
            {
                get
                {
                    return this.mXPath;
                }
                set
                {
                    this.mXPath = value;
                }
            }

            /// <summary>
            /// The value to apply
            /// </summary>
            public string Value
            {
                get
                {
                    return this.mValue;
                }
                set
                {
                    this.mValue = value;
                }
            }

            /// <summary>
            /// The xml representation
            /// </summary>
            public System.Xml.XmlNode Xml
            {
                get
                {
                    XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Action");
                    this.PopulateNode(node);
                    return node;
                }
            }
            #endregion

            #region Methods
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="xpath">The xpath expression</param>
            /// <param name="value">The value to apply</param>
            public Action(string xpath, string value)
            {
                this.mXPath = xpath;
                this.mValue = value;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="node">The xml node</param>
            public Action(XmlNode node)
            {
                this.PopulateInstance(node);
            }

            /// <summary>
            /// Populates a xml node with the objects data
            /// </summary>
            /// <param name="node">The  node to populate</param>
            public virtual void PopulateNode(XmlNode node)
            {
                Inaugura.Xml.Helper.SetAttribute(node, "xpath", this.mXPath);
                Inaugura.Xml.Helper.SetAttribute(node, "value", this.mValue);             
            }

            /// <summary>
            /// Populates the instance with the specifed xml data
            /// </summary>
            /// <param name="node">The xml node</param>
            public virtual void PopulateInstance(XmlNode node)
            {
                this.mXPath = Inaugura.Xml.Helper.GetAttribute(node, "xpath");
                this.mValue = Inaugura.Xml.Helper.GetAttribute(node, "value");
            }
            #endregion

        }
        #endregion

        #region Variables
        private Guid mID;
        private int mCount;
        private string mCode;
        private string mDescription;
        private List<Action> mActions;
        #endregion

        #region Properties
        /// <summary>
        /// The ID of the promotion
        /// </summary>
        public Guid ID
        {
            get
            {
                return this.mID;
            }
        }

        /// <summary>
        /// The number of times this promotion can be used
        /// </summary>
        public int Count
        {
            get
            {
                return this.mCount;
            }
            set
            {
                this.mCount = value;
            }
        }

        /// <summary>
        /// The promotion code
        /// </summary>
        public string Code
        {
            get
            {
                return this.mCode;
            }
            set
            {
                this.mCode = value;
            }
        }

        /// <summary>
        /// The description of the promotion
        /// </summary>
        public string Description
        {
            get
            {
                return this.mDescription;
            }
            set
            {
                this.mDescription = value;
            }
        }

        /// <summary>
        /// The list of actions
        /// </summary>
        public List<Action> Actions
        {
            get
            {
                return this.mActions;
            }
        }

        /// <summary>
        /// The xml representation
        /// </summary>
        public System.Xml.XmlNode Xml
        {
            get
            {
                XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Promotion");
                this.PopulateNode(node);
                return node;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        public Promotion()
        {
            this.mID = Guid.NewGuid();
            this.mActions = new List<Action>();
            this.mCode = Promotion.GenerateCode(5);
            this.mCount = 1;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">The xml node</param>
        public Promotion(XmlNode node) :this()
        {
            this.PopulateInstance(node);
        }

        /// <summary>
        /// Applys the promotion to xml
        /// </summary>
        /// <param name="node">The xml node</param>
        /// <returns>The xml with the promotion applied</returns>
        public System.Xml.XmlNode Apply(System.Xml.XmlNode node)
        {
            foreach (Action action in this.mActions)
            {
                XmlNodeList list = node.SelectNodes(action.XPath);
                foreach (XmlNode n in list)
                    n.Value = action.Value;
            }
            return node;
        }

        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public virtual void PopulateNode(XmlNode node)
        {
            Inaugura.Xml.Helper.SetAttribute(node, "id", this.mID.ToString());
            Inaugura.Xml.Helper.SetAttribute(node, "code", this.mCode);
            Inaugura.Xml.Helper.SetAttribute(node, "count", this.mCount.ToString());            
            if(!string.IsNullOrEmpty(this.mDescription))
                Inaugura.Xml.Helper.SetAttribute(node, "description", this.mDescription);  

            foreach (Action action in this.mActions)
            {
                XmlNode actionNode = node.OwnerDocument.CreateElement("Action");
                action.PopulateNode(actionNode);
                node.AppendChild(actionNode);
            }
        }

        /// <summary>
        /// Populates the instance with the specifed xml data
        /// </summary>
        /// <param name="node">The xml node</param>
        public virtual void PopulateInstance(XmlNode node)
        {
            this.mID = new Guid(node.Attributes["id"].Value);
            this.mCode = Inaugura.Xml.Helper.GetAttribute(node, "code");
            this.mCount = int.Parse(Inaugura.Xml.Helper.GetAttribute(node, "count"));
            this.mDescription = Inaugura.Xml.Helper.GetAttribute(node, "description");

            this.Actions.Clear();
            XmlNodeList nodes = node.SelectNodes("Action");
            foreach (XmlNode actionNode in nodes)
                this.Actions.Add(new Action(actionNode));
        }

        public static string GenerateCode(int numberOfCharacters)
        {
            string code = string.Empty;
            string letters = "23456789ABCDEFGHKMNQRSTUVWXYZ23456789";
            Random rand = new Random((int)DateTime.Now.Ticks);
            for (int i = 1; i <= numberOfCharacters; i++)
                code += letters[rand.Next(letters.Length)];

            return code;
        }
        #endregion
    }
}
