using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads.Administration
{
    /// <summary>
    /// A class representing error information
    /// </summary>
    public class ErrorInformation : Inaugura.Xml.IXmlable
    {
        #region Internal Constructs
        /// <summary>
        /// An enumeration that specifies the status of the error
        /// </summary>
        public enum ErrorStatus
        {            
            Unresolved = 0,
            Resolved = 1
        }
        #endregion

        #region Variables
        private Guid mID;
        private DateTime mTime;
        private ErrorStatus mStatus;
        private string mMessage;
        private Details mDetails;        
        #endregion

        #region Properties
        /// <summary>
        /// The guid
        /// </summary>
        public Guid ID
        {
            get
            {
                return this.mID;
            }
        }

        /// <summary>
        /// The date and time of the error
        /// </summary>
        public DateTime Time
        {
            get
            {
                return this.mTime;
            }
            set
            {
                this.mTime = value;
            }
        }

        /// <summary>
        /// The error status
        /// </summary>
        public ErrorStatus Status
        {
            get
            {
                return this.mStatus;
            }
            set
            {
                this.mStatus = value;
            }
        }

        /// <summary>
        /// The message
        /// </summary>
        public string Message
        {
            get
            {
                return this.mMessage;
            }
            set
            {
                this.mMessage = value;
            }
        }

        /// <summary>
        /// Additional details
        /// </summary>
        public Details Details
        {
            get
            {
                return this.mDetails;
            }
            set
            {
                this.mDetails = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ex">The exception</param>
        public ErrorInformation(Exception ex) : this(ex.ToString())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">The message</param>
        public ErrorInformation(string message)
        {
            this.mID = Guid.NewGuid();
            this.Status = ErrorStatus.Unresolved;
            this.mTime = DateTime.Now;            
            this.mDetails = new Details();
            this.mMessage = message;            
        }
        #endregion

        #region IXmlable Members
        public System.Xml.XmlNode Xml
        {
            get 
            {
                XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("ErrorInformation");
                this.PopulateNode(node);
                return node;
            }
        }

        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public virtual void PopulateNode(XmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            Inaugura.Xml.Helper.SetTypeAttribute(node, this.GetType());

            Inaugura.Xml.Helper.SetAttribute(node, "id", this.ID.ToString());
            Inaugura.Xml.Helper.SetAttribute(node, "status", ((int)this.Status).ToString());
            Inaugura.Xml.Helper.SetAttribute(node, "time", this.Time.ToUniversalTime().ToString("o", System.Globalization.CultureInfo.InvariantCulture));

            XmlNode messageNode = node.OwnerDocument.CreateElement("Message");
            messageNode.InnerText = this.Message;
            node.AppendChild(messageNode);

            if (this.Details.Count > 0)
            {
                XmlNode detailsNode = node.OwnerDocument.CreateElement("Details");
                this.Details.PopulateNode(detailsNode);
                node.AppendChild(detailsNode);
            }            
        }

        /// <summary>
        /// Populates the instance with the specifed xml data
        /// </summary>
        /// <param name="node">The xml node</param>
        public virtual void PopulateInstance(XmlNode node)
        {
            Inaugura.Xml.Helper.EnsureAttributeExists(node, "id");
            Inaugura.Xml.Helper.EnsureAttributeExists(node, "status");
            Inaugura.Xml.Helper.EnsureAttributeExists(node, "time");

            this.mID = new Guid(Inaugura.Xml.Helper.GetAttribute(node, "id"));

            if (node.Attributes["status"] != null)
                this.Status = (ErrorStatus)Enum.Parse(typeof(ErrorStatus), Inaugura.Xml.Helper.GetAttribute(node, "status"));

            this.Time = DateTime.Parse(Inaugura.Xml.Helper.GetAttribute(node, "time"));

            XmlNode messageNode = node.SelectSingleNode("Message");
            if (messageNode != null)
                this.Message = messageNode.InnerText;

            if (node["Details"] != null)
                this.Details = new Details(node["Details"]);            
        }     
        #endregion
    }
}
