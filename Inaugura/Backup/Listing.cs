#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#endregion

namespace Inaugura.RealLeads
{
    /// <summary>
    /// A class which represents a listing
    /// </summary>
    public abstract class Listing : Inaugura.Xml.IXmlable
    {
        #region Internal Definitions
        [FlagsAttribute]
        public enum ListingStatus : short
        {
            /// <summary>
            /// Preview
            /// </summary>
            Inactive = 1,
            /// <summary>
            /// Active
            /// </summary>
            Active = 2,
            /// <summary>
            /// Sold
            /// </summary>
            Sold = 4,           
            /// <summary>
            /// Suspended
            /// </summary>
            Suspended = 8,
            /// <summary>
            /// All
            /// </summary>
            All = Inactive | Active | Sold | Suspended
        }
        #endregion

        #region Variables
        private ListingStatus mStatus;
        private Details mDetails;
        private System.Collections.Hashtable mHashtable;
        private Guid mID;
        private string mCode;
        private Guid mUserID;
        private string mTitle;
        private string mDescription;
        private DateTime mExpirationDate = DateTime.MinValue;
        private ListingImageCollection mImages;
        private bool mFromCache;
        private StringCollection mFeatures;
        private StringCollection mLinks;
        #endregion

        #region IXmlable Members
        /// <summary>
        /// Gets the xml representation of the Listing
        /// </summary>
        /// <value></value>
        public XmlNode Xml
        {
            get
            {
                XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Listing");
                this.PopulateNode(node);
                return node;
            }
        }
        #endregion
                
        #region Properties
        /// <summary>
        /// The GUID for this listing
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
        /// The listing code
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
        /// The status of the listing
        /// </summary>
        public ListingStatus Status
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
        /// The id of the user who owns this listing
        /// </summary>
        public Guid UserID
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
        /// Additional persistant details specific to this listing
        /// </summary>
        public Details Details
        {
            get
            {
                return this.mDetails;
            }
            private set
            {
                this.mDetails = value;
            }
        }

        /// <summary>
        /// A general purpose hashtable which can contain non-persisted objects
        /// </summary>
        public System.Collections.Hashtable Objects
        {
            get
            {
                return this.mHashtable;
            }
            set
            {
                this.mHashtable = value;
            }
        }

        /// <summary>
        /// Gets/Sets the expiration date of the Listing
        /// </summary>
        public DateTime ExpirationDate
        {
            get
            {
                return this.mExpirationDate;
            }
            set
            {
                this.mExpirationDate = value;
            }
        }

        /// <summary>
        /// The title of the listing
        /// </summary>
        public string Title
        {
            get
            {
                if (this.mTitle != null && this.mTitle != string.Empty)
                    return this.mTitle;
                else
                    return string.Format("Listing {0}", this.Code);
            }
            set
            {
                this.mTitle = value;
            }
        }

        /// <summary>
        /// The description of this listing
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
        /// The listing features
        /// </summary>
        public StringCollection Features
        {
            get
            {
                return this.mFeatures;
            }
            set
            {
                this.mFeatures = value;
            }
        }

        /// <summary>
        /// External Links for this listing
        /// </summary>
        public StringCollection Links
        {
            get
            {
                return this.mLinks;
            }
            set
            {
                this.mLinks = value;
            }
        }

        /// <summary>
        /// The images of the listing
        /// </summary>
        public ListingImageCollection Images
        {
            get
            {
                return this.mImages;
            }
            set
            {
                this.mImages = value;
            }
        }

        /// <summary>
        /// Flag determining if the Listing has been setup by the agent (ie prompts recorded)
        /// </summary>
        public bool IsSetup
        {
            get
            {
                if (this.Details["IsSetup"] != null)
                    return bool.Parse(this.Details["IsSetup"]);
                else
                    return false;
            }
            set
            {
                this.Details["IsSetup"] = value.ToString();
            }
        }
        
        #region Prompts
        /// <summary>
        /// The Greeting Prompt File
        /// </summary>
        /// <value></value>
        public string GreetingPrompt
        {
            get
            {
                if (this.Details["GreetingPrompt"] != null)
                    return this.Details["GreetingPrompt"];
                else
                    return string.Empty;
            }
            set
            {
                this.Details["GreetingPrompt"] = value;
            }
        }

        /// <summary>
        /// The Information Prompt
        /// </summary>
        /// <value></value>
        public string InformationPrompt
        {
            get
            {
                if (this.Details["InformationPrompt"] != null)
                    return this.Details["InformationPrompt"];
                else
                    return string.Empty;
            }
            set
            {
                this.Details["InformationPrompt"] = value;
            }
        }

        /// <summary>
        /// The Information Prompt
        /// </summary>
        /// <value></value>
        public string VoiceMailGreetingPrompt
        {
            get
            {
                if (this.Details["VoiceMailGreetingPrompt"] != null)
                    return this.Details["VoiceMailGreetingPrompt"];
                else
                    return string.Empty;
            }
            set
            {
                this.Details["VoiceMailGreetingPrompt"] = value;
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="xml">The xml representation of the listing</param>
        public Listing(XmlNode listingNode)
            : this()
        {
            this.PopulateInstance(listingNode);            
        }

        /// <summary>
        /// Constructor
        /// </summary>		
        public Listing()
        {
            this.mHashtable = new System.Collections.Hashtable();
            this.ID = Guid.NewGuid();
            this.Title = string.Empty;
            this.Description = string.Empty;
            this.mCode = string.Empty;
            this.Status = ListingStatus.Inactive;
            this.Details = new Details();
            this.mFeatures = new StringCollection();
            this.Images = new ListingImageCollection();
        }

        /// <summary>
        /// Determins if a user can edit this listing
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>True if the listing can be editied by the user, otherwise false</returns>
        public bool CheckEditPolicy(Inaugura.RealLeads.User user)
        {
            if (user != null && (user.IsInRole(UserRoles.Administrator) || user.ID == this.UserID))
                return true;
            return false;
        }

        /// <summary>
        /// Checks the edit policy of this listing given the threads IPrinciple
        /// </summary>
        /// <returns></returns>
        public bool CheckEditPolicy()
        {
            if (Inaugura.RealLeads.Security.UserPrincipal.SecureContext)
                return this.CheckEditPolicy(Inaugura.RealLeads.Security.UserPrincipal.CurrentUser);
            else // we are not in a secure context so dont worry about it
                return true;
        }
        /// <summary>
        /// Enforces the listings edit policy by throwing an exception
        /// </summary>
        public void EnforceEditPolicy()
        {
            if (!CheckEditPolicy())
                throw new Inaugura.Security.SecurityException(Inaugura.Security.SecurityException.SecurityMessages.PermissionRequired);
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            if(this.mID != null)
                hashCode ^= this.mID.GetHashCode();
            if(this.mUserID != null)
                hashCode ^= this.mUserID.GetHashCode();
            if(this.mCode != null)
                hashCode ^= this.mCode.GetHashCode();
            if(this.mDescription != null)
                hashCode ^= this.mDescription.GetHashCode();
            if(this.mDetails != null)
                hashCode ^= this.mDetails.GetHashCode();
            if(this.mExpirationDate != null)
                hashCode ^= this.mExpirationDate.GetHashCode();
            if(this.mImages != null)
                hashCode ^= this.mImages.GetHashCode();
            hashCode ^= this.mStatus.GetHashCode();
            if(this.mTitle != null)
                hashCode ^= this.mTitle.GetHashCode();

            return hashCode;
            
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

            if (this.Code != null && this.Code != string.Empty)
                Inaugura.Xml.Helper.SetAttribute(node, "code", this.Code);

            Inaugura.Xml.Helper.SetAttribute(node, "userId", this.mUserID.ToString());
            if(this.ExpirationDate != DateTime.MaxValue)
                Inaugura.Xml.Helper.SetAttribute(node, "expiration", this.ExpirationDate.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture));
            Inaugura.Xml.Helper.SetAttribute(node, "status", ((int)this.Status).ToString());

            if (this.Title != null && this.Title != string.Empty)
            {
                XmlNode titleNode = node.OwnerDocument.CreateElement("Title");
                titleNode.InnerText = this.Title;
                node.AppendChild(titleNode);
            }

            if (!string.IsNullOrEmpty(this.mDescription))
            {
                string[] paras = this.mDescription.Split('\n');
                XmlNode descriptionNode = node.OwnerDocument.CreateElement("Description");
                foreach (string para in paras)
                {
                    if (!string.IsNullOrEmpty(para))
                    {
                        XmlNode paraNode = descriptionNode.OwnerDocument.CreateElement("p");
                        paraNode.InnerText = para;
                        descriptionNode.AppendChild(paraNode);
                    }
                }
                //descriptionNode.InnerText = this.Description;
                node.AppendChild(descriptionNode);
            }

            if (this.mDetails.Count > 0)
            {
                XmlNode detailsNode = node.OwnerDocument.CreateElement("Details");
                this.Details.PopulateNode(detailsNode);
                node.AppendChild(detailsNode);
            }

            if (this.mFeatures.Count > 0)
            {
                XmlNode featuresNode = node.OwnerDocument.CreateElement("Features");
                this.mFeatures.PopulateNode(featuresNode);
                node.AppendChild(featuresNode);
            }

            if (this.mLinks.Count > 0)
            {
                XmlNode linksNode = node.OwnerDocument.CreateElement("Links");
                this.mLinks.PopulateNode(linksNode);
                node.AppendChild(linksNode);
            }

            if (this.Images.Count > 0)
            {
                XmlNode imagesNode = node.OwnerDocument.CreateElement("Images");
                this.Images.PopulateNode(imagesNode);
                node.AppendChild(imagesNode);
            }
        }

        /// <summary>
        /// Populates the instance with the specifed xml data
        /// </summary>
        /// <param name="node">The xml node</param>
        public virtual void PopulateInstance(XmlNode node)
        {
            if (node.Attributes["id"] == null)
                throw new ArgumentException("The xml does not contain a id attribute");

            Inaugura.Xml.Helper.EnsureAttributeExists(node,"userId");

            if (node.Attributes["expiration"] == null)
                throw new ArgumentException("The xml does not contain expiration attribute");

            if (node.Attributes["status"] != null)
            {
                this.Status = (ListingStatus)Enum.Parse(typeof(ListingStatus), Inaugura.Xml.Helper.GetAttribute(node, "status"));
                //this.Status = (ListingStatus)short.Parse(Inaugura.Xml.Helper.GetAttribute(node, "status"));
                //this.Status = (ListingStatus)Enum.Parse(typeof(ListingStatus), Inaugura.Xml.Helper.GetAttribute(node, "status"));
            }


            this.ID = new Guid(Inaugura.Xml.Helper.GetAttribute(node, "id"));

            if (node.Attributes["code"] != null)
                this.Code = Inaugura.Xml.Helper.GetAttribute(node, "code");


            this.UserID = new Guid(Inaugura.Xml.Helper.GetAttribute(node, "userId"));

            
            this.ExpirationDate = DateTime.MaxValue;
            if (node.Attributes["expiration"] != null)
            {
                if (!DateTime.TryParseExact(node.Attributes["expiration"].Value, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeLocal, out this.mExpirationDate))
                {
                    //TODO can be removed once all the existing listings no longer use this format
                    DateTime.TryParseExact(node.Attributes["expiration"].Value, "o", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeLocal, out this.mExpirationDate);
                }
            }                            

            if (node["Title"] != null)
                this.Title = node["Title"].InnerText;

            if (node["Description"] != null)
            {
                StringBuilder str = new StringBuilder();
                XmlNodeList nodes = node["Description"].SelectNodes("p");
                foreach (XmlNode n in nodes)
                {
                    str.Append(n.InnerText + "\n");
                }
                //this.Description = node["Description"].InnerText;
                this.Description = str.ToString();
            }

            if (node["Details"] != null)
                this.Details = new Details(node["Details"]);

            if (node["Features"] != null)
                this.mFeatures = new StringCollection(node["Features"]);
            else
                this.mFeatures = new StringCollection();

            if (node["Links"] != null)
                this.mLinks = new StringCollection(node["Links"]);
            else
                this.mLinks = new StringCollection();

            if (node["Images"] != null)
                this.Images = new ListingImageCollection(node["Images"]);
        }
        
        #region API MEthods
        /// <summary>
        /// Adds a image file to a listing
        /// </summary>
        /// <param name="api">The API instance</param>
        /// <param name="listing">The listing</param>
        /// <param name="img">The image</param>
        /// <param name="title">The title</param>
        /// <param name="description">The description</param>
        public void AddImage(Inaugura.RealLeads.RealLeadsAPI api, System.Drawing.Image img, string caption)
        {
            int maxWidth = 800;
            int maxHeight = 600;

            if (img.Width < maxWidth)
                maxWidth = img.Width;
            if (img.Height < maxHeight)
                maxHeight = img.Height;

            img = Inaugura.Drawing.ImageHelper.Resize(img, maxWidth, maxHeight);

            File file = new File();
            using (System.IO.MemoryStream memStream = new System.IO.MemoryStream())
            {
                img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                file.FileName = file.ID + ".jpg";
                file.Data = memStream.ToArray();
            }

            // add the file to the database
            this.AddFile(api, file);

            Inaugura.RealLeads.Image newImage = new Inaugura.RealLeads.Image(file.ID);
            newImage.Caption = caption;
            this.Images.Add(newImage);

            // update the listing
            api.Data.ListingStore.Update(this);
        }


        /// <summary>
        /// Removes an image from a listing
        /// </summary>
        /// <param name="api">The API instance</param>
        /// <param name="imageID">The image ID</param>
        public void RemoveImage(Inaugura.RealLeads.RealLeadsAPI api, Guid imageID)
        {
            this.EnforceEditPolicy();
            Image img = this.Images[imageID];
            if (img == null)
                throw new ApplicationException("The image could not be found");

            this.Images.Remove(img);
            this.RemoveFile(api, img.FileID);
            api.ListingManager.UpdateListing(this);

        }

        /// <summary>
        /// Adds a file to a listing
        /// </summary>
        /// <param name="api">The API instance</param>
        /// <param name="file">The file to add</param>
        public void AddFile(Inaugura.RealLeads.RealLeadsAPI api, File file)
        {
            api.Data.ListingStore.AddFile(this.ID, file);
        }

        /// <summary>
        /// Removes a file from the listing
        /// </summary>
        /// <param name="api">The API instance</param>
        /// <param name="fileID">The file ID to remove</param>
        public void RemoveFile(Inaugura.RealLeads.RealLeadsAPI api, Guid fileID)
        {
            api.Data.ListingStore.RemoveFile(fileID);
        }
        #endregion
    }
}
