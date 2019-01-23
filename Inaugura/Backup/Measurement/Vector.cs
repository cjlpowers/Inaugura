using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Measurement
{
    /// <summary>
    /// A class representing a vector
    /// </summary>
    public class Vector : Point
    {

        #region Properties
        /// <summary>
        /// The length of the vector
        /// </summary>
        public Measurement Length
        {
            get
            {
                Measurement x = this.X.ToStandardUnit();
                Measurement y = this.Y.ToStandardUnit();
                Measurement z = this.Z.ToStandardUnit();

                double length = x.Value * x.Value + y.Value * y.Value + z.Value * z.Value;
                length = Math.Sqrt(length);
                Measurement m = new Measurement(length, x.Unit);
                if (this.X.Unit == this.Y.Unit && this.Y.Unit == this.Z.Unit)
                    return m.Convert(this.X.Unit);
                else
                    return m;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        public Vector(Point p1, Point p2)
            : this(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z)
        {
        }

          /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">The x value</param>
        /// <param name="y">The y value</param>
        /// <param name="z">The z value</param>
        /// <param name="unit">The unit</param>
        public Vector(double x, double y, double z, Inaugura.Measurement.Unit unit) : base(x,y,z,unit)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">The x value</param>
        /// <param name="y">The y value</param>
        /// <param name="z">The z value</param>
        public Vector(Measurement x, Measurement y, Measurement z)
            : base(x, y, z)
        {
        }

        #region Operators
        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector operator *(Vector v1, double scalar)
        {
            return new Vector(v1.X * scalar, v1.Y * scalar, v1.Z * scalar);
        }

        public static Vector operator *(Vector v1, Vector v2)
        {
            return new Vector(v1.Y * v2.Z - v1.Z*v2.Y, v1.Z*v2.X - v1.X*v2.Z, v1.X*v2.Y-v1.Y*v2.X);
        }
        #endregion
        #endregion
    }
}
