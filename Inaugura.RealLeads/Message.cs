using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
    public class Message : Xml.IXmlable
    {
        #region Variables
        private Guid mID;
        private string mUserID;
        private string mFrom;
        private string mText;
        private DateTime mDate;
        private bool mNew;        
        #endregion

        #region Properties

        /// <summary>
        /// The ID of the message
        /// </summary>
        public Guid ID
        {
            get
            {
                return this.mID;
            }
            private set
            {
                this.mID = value;
            }
        }

        /// <summary>
        /// The ID of the user this message was sent to
        /// </summary>
        public string UserID
        {
            get
            {
                return this.mUserID;
            }
            set
            {
                this.mUserID = value;
            }
        }

        /// <summary>
        /// The email address of the person who sent the message
        /// </summary>
        public string From
        {
            get
            {
                return this.mFrom;
            }
            set
            {
                this.mFrom = value;
            }
        }

        /// <summary>
        /// The message text
        /// </summary>
        public string Text
        {
            get
            {
                return this.mText;
            }
            set
            {
                this.mText = value;
            }
        }

        /// <summary>
        /// The date and time the message was sent
        /// </summary>
        public DateTime Date
        {
            get
            {
                return this.mDate;
            }            
        }

        /// <summary>
        /// A flag which determins if the message is new or not
        /// </summary>
        public bool New
        {
            get
            {
                return this.mNew;
            }
            set
            {
                this.mNew = value;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userID">The id of the user this message was sent to</param>
        /// <param name="from">The address of the sender</param>
        /// <param name="text">The message text</param>
        public Message(string userID, string from, string text)
        {
            this.mUserID = userID;
            this.mNew = true;
            this.mDate = DateTime.Now;
            this.mID = Guid.NewGuid();
            this.mFrom = from;
            this.mText = text;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">The xml representation of the instance</param>
        public Message(XmlNode node)
        {
            this.PopulateInstance(node);
        }

        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public void PopulateNode(XmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node");
           
            Inaugura.Xml.Helper.SetAttribute(node, "id", this.mID.ToString());
            Inaugura.Xml.Helper.SetAttribute(node, "userID", this.mUserID);
            Inaugura.Xml.Helper.SetAttribute(node, "from", this.mFrom);
            Inaugura.Xml.Helper.SetAttribute(node, "date", this.mDate.ToString("MM/dd/yyyy  HH:mm", System.Globalization.CultureInfo.InvariantCulture));
            if(this.mNew)
                Inaugura.Xml.Helper.SetAttribute(node, "new", this.mNew.ToString());

            node.InnerText = this.mText;
        }

        /// <summary>
        /// Populates the instance with the specifed xml data
        /// </summary>
        /// <param name="node">The xml node</param>
        public void PopulateInstance(XmlNode node)
        {
            Inaugura.Xml.Helper.EnsureAttributeExists(node, "id");
            Inaugura.Xml.Helper.EnsureAttributeExists(node, "userID");
            Inaugura.Xml.Helper.EnsureAttributeExists(node, "from");
            Inaugura.Xml.Helper.EnsureAttributeExists(node, "date");

            this.mID = new Guid(Inaugura.Xml.Helper.GetAttribute(node, "id"));
            this.mUserID = Inaugura.Xml.Helper.GetAttribute(node, "userID");
            this.From = Inaugura.Xml.Helper.GetAttribute(node, "from");

            this.mDate = DateTime.ParseExact( Inaugura.Xml.Helper.GetAttribute(node, "date"), "MM/dd/yyyy  HH:mm", System.Globalization.CultureInfo.InvariantCulture);

            if (node.Attributes["new"] != null)
                bool.TryParse(node.Attributes["new"].Value, out this.mNew);

            this.Text = node.InnerText;
        }     
        #endregion

        #region IXmlable Members
        /// <summary>
        /// Gets the xml representation of the Message
        /// </summary>
        /// <value></value>
        public System.Xml.XmlNode Xml
        {
            get
            {
                XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Message");
                this.PopulateNode(node);
                return node;
            }
        }

        #endregion
    }
}

