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
                Measurement x = X;
                Measurement y = Y;
                Measurement z = Z;

                if (this.X.Unit == this.Y.Unit && Y.Unit == Z.Unit)
                {
                    x = this.X.ToStandardUnit();
                    y = this.Y.ToStandardUnit();
                    z = this.Z.ToStandardUnit();
                }

                double length = x.Value * x.Value + y.Value * y.Value + z.Value * z.Value;
                length = Math.Sqrt(length);
                return new Measurement(length, x.Unit);             
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node"></param>
        public Vector(System.Xml.XmlNode node)
            :base(node)
        {
        }

        /// <summary>
        /// Converts to another unit
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public Vector Convert(Unit unit)
        {
            Measurement x = Measurement.Empty;
            Measurement y = Measurement.Empty;
            Measurement z = Measurement.Empty;

            if(X != Measurement.Empty)
                x = this.X.Convert(unit);
            if (Y != Measurement.Empty)
                y = this.Y.Convert(unit);
            if (Z != Measurement.Empty)
                z = this.Z.Convert(unit);

            return new Vector(x, y, z);
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
            double valX = v1.Y * v2.Z - v1.Z*v2.Y;
            double valY = v1.Z*v2.X - v1.X*v2.Z;
            double valZ = v1.X*v2.Y-v1.Y*v2.X;

            return new Vector(valX, valY, valZ, Unit.Meter);
        }
        #endregion
        #endregion
    }
}
