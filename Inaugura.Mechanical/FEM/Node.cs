using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Inaugura.Measurement;

namespace Inaugura.Mechanical.FEM
{
    /// <summary>
    /// A class representing a node
    /// </summary>
    public class Node : Inaugura.Xml.IXmlable
    {
        #region Variables
        private int mNumber;
        private Point mPosition;
        private Vector mDisplacement;
        private Vector mInitialDisplacement;
        private Vector mForce;
        private Vector mReactionForce;
        #endregion        

        #region Properties

        /// <summary>
        /// The global node number
        /// </summary>
        public int Number
        {
            get
            {
                return this.mNumber;
            }
        }

        /// <summary>
        /// The position of the node
        /// </summary>
        public Point Position
        {
            get
            {
                return this.mPosition;
            }
            set
            {
                this.mPosition = value;
            }
        }

        /// <summary>
        /// The nodal displacement
        /// </summary>
        public Vector Displacement
        {
            get
            {
                return this.mDisplacement;
            }
            set
            {
                this.mDisplacement = value;
            }
        }

        /// <summary>
        /// The local force vector
        /// </summary>
        public Vector Force
        {
            get
            {
                return this.mForce;
            }
            set
            {
                this.mForce = value;
            }
        }

        /// <summary>
        /// The reaction force
        /// </summary>
        public Vector ReactionForce
        {
            get
            {
                return this.mReactionForce;
            }
            internal set
            {
                this.mReactionForce = value;
            }
        }

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
                XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Node");
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

            Inaugura.Xml.Helper.SetAttribute(node, "number", this.mNumber.ToString());

            if (this.mPosition != null)
            {
                XmlNode positionNode = node.OwnerDocument.CreateElement("Position");
                this.mPosition.PopulateNode(positionNode);
                node.AppendChild(positionNode);
            }

            if (this.mDisplacement != null)
            {
                XmlNode displacementNode = node.OwnerDocument.CreateElement("Displacement");
                this.mDisplacement.PopulateNode(displacementNode);
                node.AppendChild(displacementNode);
            }

            if (this.mForce != null)
            {
                XmlNode forceNode = node.OwnerDocument.CreateElement("Force");
                this.mForce.PopulateNode(forceNode);
                node.AppendChild(forceNode);
            }
        }

        /// <summary>
        /// Populates the instance with the specifed xml data
        /// </summary>
        /// <param name="node">The xml node</param>
        public virtual void PopulateInstance(XmlNode node)
        {
            Inaugura.Xml.Helper.EnsureAttributeExists(node, "number");

            this.mNumber = int.Parse(Inaugura.Xml.Helper.GetAttribute(node, "number"));

            if (node["Position"] != null)
                this.mPosition = new Inaugura.Measurement.Point(node["Position"]);

            if (node["Displacement"] != null)
            {
                this.mDisplacement = new Inaugura.Measurement.Vector(node["Displacement"]);
                this.mInitialDisplacement = mDisplacement;
            }

            if (node["Force"] != null)
            {
                this.mForce = new Inaugura.Measurement.Vector(node["Force"]);               
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public Node(int number)
        {
            this.mNumber = number;
            this.mForce = new Vector(Measurement.Measurement.Empty, Measurement.Measurement.Empty, Measurement.Measurement.Empty);
            this.mPosition = new Point(Measurement.Measurement.Empty, Measurement.Measurement.Empty, Measurement.Measurement.Empty);
            this.mDisplacement = new Vector(Measurement.Measurement.Empty, Measurement.Measurement.Empty, Measurement.Measurement.Empty);
            this.mInitialDisplacement = new Vector(Measurement.Measurement.Empty, Measurement.Measurement.Empty, Measurement.Measurement.Empty);
            this.mReactionForce = new Vector(Measurement.Measurement.Empty, Measurement.Measurement.Empty, Measurement.Measurement.Empty);
        }

        public Node(XmlNode node) : this(0)
        {
            this.PopulateInstance(node);
        }

        internal void Reset()
        {
            this.mDisplacement = this.mInitialDisplacement;
        }

        internal void SetUnitSystem(Inaugura.Measurement.Unit.UnitSystem unitSystem)
        {
            if (unitSystem == Unit.UnitSystem.SI)
            {
                this.mDisplacement = this.mDisplacement.Convert(Unit.Meter);
                this.mForce = this.mForce.Convert(Unit.N);
                this.mPosition = this.mPosition.Convert(Unit.Meter);
            }
            else if (unitSystem == Unit.UnitSystem.English)
            {
                this.mDisplacement = this.mDisplacement.Convert(Unit.Inch);
                this.mForce = this.mForce.Convert(Unit.lbf);
                this.mPosition = this.mPosition.Convert(Unit.Inch);
            }
            else
                throw new NotSupportedException();

            this.mInitialDisplacement = this.mDisplacement;
        }

        public override string ToString()
        {
            return string.Format("Node {0}", this.mNumber);
        }
       
        #endregion
    }
}
