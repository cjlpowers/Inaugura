using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Measurement
{
    /// <summary>
    /// A class representing a point in 3D space
    /// </summary>
    public class Point
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

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", this.X, this.Y, this.Z);
        }       
        #endregion
    }
}
