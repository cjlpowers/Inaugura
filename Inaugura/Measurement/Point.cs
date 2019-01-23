using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.Measurement
{
    /// <summary>
    /// A class representing a point in 3D space
    /// </summary>
    public class Point: Inaugura.Xml.IXmlable
    {
        #region Variables
        private Inaugura.Measurement.Measurement mX;
        private Inaugura.Measurement.Measurement mY;
        private Inaugura.Measurement.Measurement mZ;
        #endregion

        #region Properties
        /// <summary>
        /// The x component
        /// </summary>
        public Inaugura.Measurement.Measurement X
        {
            get
            {
                return this.mX;
            }
            private set
            {
                this.mX = value;
            }
        }

        /// <summary>
        /// The y component
        /// </summary>
        public Inaugura.Measurement.Measurement Y
        {
            get
            {
                return this.mY;
            }
            private set
            {
                this.mY = value;
            }
        }

        /// <summary>
        /// The z component
        /// </summary>
        public Inaugura.Measurement.Measurement Z
        {
            get
            {
                return this.mZ;
            }
            private set
            {
                this.mZ = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">The x component</param>
        /// <param name="y">The y component</param>
        /// <param name="z">The z component</param>
        /// <param name="unit">The unit of measure</param>
        public Point(double x, double y, double z, Unit unit)
            : this(new Measurement(x, unit), new Measurement(y, unit), new Measurement(z, unit))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">The x component</param>
        /// <param name="y">The y component</param>
        public Point(double x, double y, Unit unit)
            : this(new Measurement(x, unit), new Measurement(y, unit), new Measurement(0, unit))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">The x component</param>
        /// <param name="y">The y component</param>
        public Point(Measurement x, Measurement y)
            : this(x, y, new Measurement(0, Unit.Meter))
        {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">The x component</param>
        /// <param name="y">The y component</param>
        /// <param name="z">The z component</param>
        public Point(Measurement x, Measurement y, Measurement z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node"></param>
        public Point(XmlNode node) : this(Measurement.Empty, Measurement.Empty, Measurement.Empty)
        {
            this.PopulateInstance(node);
        }

        /// <summary>
        /// Converts to another unit
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public Point Convert(Unit unit)
        {
            Measurement x = Measurement.Empty;
            Measurement y = Measurement.Empty;
            Measurement z = Measurement.Empty;

            if (X != Measurement.Empty)
                x = this.X.Convert(unit);
            if (Y != Measurement.Empty)
                y = this.Y.Convert(unit);
            if (Z != Measurement.Empty)
                z = this.Z.Convert(unit);

            return new Point(x, y, z);
        }


        /// <summary>
        /// ToString method
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", this.X, this.Y, this.Z);
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
                XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Point");
                this.PopulateNode(node);
                return node;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public virtual void PopulateNode(XmlNode node)
        {
            if (this.mX != null && this.mX != 0)
                Inaugura.Xml.Helper.SetAttribute(node, "x", this.mX.ToString());

            if (this.mY != null && this.mY != 0)
                Inaugura.Xml.Helper.SetAttribute(node, "y", this.mY.ToString());

            if (this.mZ != null && this.mZ != 0)
                Inaugura.Xml.Helper.SetAttribute(node, "z", this.mZ.ToString());
        }

        /// <summary>
        /// Populates the instance with the specifed xml data
        /// </summary>
        /// <param name="node">The xml node</param>
        public virtual void PopulateInstance(XmlNode node)
        {
            if (Inaugura.Xml.Helper.AttributeExists(node, "x"))
                this.mX = Measurement.Parse(Inaugura.Xml.Helper.GetAttribute(node, "x"));

            if (Inaugura.Xml.Helper.AttributeExists(node, "y"))
                this.mY = Measurement.Parse(Inaugura.Xml.Helper.GetAttribute(node, "y"));

            if (Inaugura.Xml.Helper.AttributeExists(node, "z"))
                this.mZ = Measurement.Parse(Inaugura.Xml.Helper.GetAttribute(node, "z"));
        }
        #endregion

    }
}
