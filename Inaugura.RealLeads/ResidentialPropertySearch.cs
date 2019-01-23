using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// A class which holds the listing search criteria
	/// </summary>
    public abstract class ResidentialPropertySearch : PropertySearch
	{
		#region Variables
        private bool mPool;
        private bool mFirePlace;
        private bool mWaterfront;
        private bool mBackyardNeighbors;
		#endregion

		#region Properties
        /// <summary>
        /// The property has a pool
        /// </summary>
        public bool Pool
        {
            get
            {
                return this.mPool;
            }
            set
            {
                this.mPool = value;
            }
        }        

        /// <summary>
        /// The property has a fire place
        /// </summary>
        public bool FirePlace
        {
            get
            {
                return this.mFirePlace;
            }
            set
            {
                this.mFirePlace = value;
            }
        }

        /// <summary>
        /// The property is on a waterfront
        /// </summary>
        public bool Waterfront
        {
            get
            {
                return this.mWaterfront;
            }
            set
            {
                this.mWaterfront = value;
            }
        }        
        #endregion
              
        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public ResidentialPropertySearch()
        {            
        }

        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public override void PopulateNode(XmlNode node)
        {
            base.PopulateNode(node);

            if (this.mPool)
                Inaugura.Xml.Helper.SetAttribute(node, "pool", this.mPool.ToString());

            if (this.mFirePlace)
                Inaugura.Xml.Helper.SetAttribute(node, "firePlace", this.mFirePlace.ToString());

            if(this.mWaterfront)
                Inaugura.Xml.Helper.SetAttribute(node, "waterfront", this.mWaterfront.ToString());            
        }

        /// <summary>
        /// Populates the instance with the specifed xml data
        /// </summary>
        /// <param name="node">The xml node</param>
        public override void PopulateInstance(XmlNode node)
        {
            base.PopulateInstance(node);

            if (node.Attributes["pool"] != null)
                this.mPool = bool.Parse(node.Attributes["pool"].Value);

            if (node.Attributes["firePlace"] != null)
                this.mFirePlace = bool.Parse(node.Attributes["firePlace"].Value);

            if (node.Attributes["waterfront"] != null)
                this.mWaterfront = bool.Parse(node.Attributes["waterfront"].Value);
        }       
        #endregion
    }
}
