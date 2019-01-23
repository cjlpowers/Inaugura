using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace Inaugura.Measurement
{
    /// <summary>
    /// A class which represents a measurement
    /// </summary>
    public class Measurement : Inaugura.Xml.IXmlable
    {
        #region Variables
        internal const string UnitTag = "unit";
        internal const string SymbolTag = "symbol";
        internal const string ValueTag = "value";
        private Unit mUnit;
        private double mValue;
        #endregion

        #region Properties
        /// <summary>
        /// The unit of measure
        /// </summary>
        public Unit Unit
        {
            get
            {
                return this.mUnit;
            }
            private set
            {
                this.mUnit = value;
            }
        }

        /// <summary>
        /// The value
        /// </summary>
        public double Value
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
                XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Measurement");
                this.PopulateNode(node);
                return node;
            }
        }

        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public void PopulateNode(XmlNode node)
        {
            Inaugura.Xml.Helper.SetAttribute(node, Measurement.ValueTag, this.Value.ToString());
            Inaugura.Xml.Helper.SetAttribute(node, Measurement.UnitTag, this.Unit.ToString());
            Inaugura.Xml.Helper.SetAttribute(node, Measurement.SymbolTag, this.Unit.Symbol);
        }

        /// <summary>
        /// Populates the instance with the specifed xml data
        /// </summary>
        /// <param name="node"></param>
        public void PopulateInstance(XmlNode node)
        {
            this.mValue = float.Parse(Inaugura.Xml.Helper.GetAttribute(node, Measurement.ValueTag));
            this.mUnit = Unit.Parse(Inaugura.Xml.Helper.GetAttribute(node, Measurement.UnitTag));
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">The underlying xml node</param>
        public Measurement(XmlNode node)
        {
            this.PopulateInstance(node);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">The scalar value</param>
        /// <param name="unit">The unit</param>
        public Measurement(double value, Unit unit)
        {
            this.Value = value;
            this.Unit = unit;
        }

        /// <summary>
        /// Converts from one unit to another
        /// </summary>
        /// <param name="unit">The desired unit</param>
        /// <returns></returns>
        public Measurement Convert(Unit unit)
        {
            // are we already in the desired unit?
            if (this.Unit == unit)
                return new Measurement(this.Value, this.Unit);

            // is a standard unit conversion not defined
            if (this.Unit.StandardUnitConversion == null)
            {
                if (unit.StandardUnitConversion == null)
                {
                    if (unit != this.Unit)
                        throw new ArgumentException("Units do not share a common standard unit");
                }
                else if (unit.StandardUnitConversion.Unit != this.Unit)
                    throw new ArgumentException("Units do not share a common standard unit");
            }
            else if (unit.StandardUnitConversion == null)
            {
                if (this.Unit.StandardUnitConversion.Unit != unit)
                    throw new ArgumentException("Units do not share a common standard unit");
            }
            else if (unit.StandardUnitConversion.Unit != this.Unit.StandardUnitConversion.Unit)
                throw new ArgumentException("Units do not share a common standard unit");


            Unit endUnit = null;
            // perform the conversion
            double value = this.Value;
            if (this.Unit.StandardUnitConversion != null)
            {
                value = this.Unit.StandardUnitConversion.Convert(value);
                endUnit = this.Unit.StandardUnitConversion.Unit;
            }
            if (unit.StandardUnitConversion != null)
            {
                value = unit.StandardUnitConversion.Unconvert(value);
                endUnit = unit;
            }
            return new Measurement(value, endUnit);
        }

        /// <summary>
        /// Creates an equivelent measurement in the standard unit
        /// </summary>
        /// <returns>The equivelent measurement in the standard unit</returns>
        public Measurement ToStandardUnit()
        {
            Measurement m;
            if (this.Unit.StandardUnitConversion != null)
                m = this.Convert(this.Unit.StandardUnitConversion.Unit);
            else
                m = new Measurement(this.Value, this.Unit);
            return m;
        }

        /// <summary>
        /// Conversion to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} {1}", this.Value, this.Unit.Symbol);
        }

        /// <summary>
        /// A string containing conversions to different units
        /// </summary>
        /// <returns></returns>
        public string ToConvertedString()
        {
            List<Unit> units = new List<Unit>();
            foreach (Unit unit in Unit.AllUnits)
            {
                if (unit.Type == this.Unit.Type && this.Unit != unit)
                    units.Add(unit);
            }
            return this.ToConvertedString(units.ToArray());
        }

        /// <summary>
        /// A string containing conversions
        /// </summary>
        /// <param name="units">The units conversions to show</param>
        /// <returns></returns>
        public string ToConvertedString(Unit[] units)
        {
            StringBuilder str = new StringBuilder();
            foreach (Unit u in units)
                str.AppendFormat("{0}\n", this.Convert(u).ToString());
            return str.ToString();
        }

        /// <summary>
        /// Hashcode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.mUnit.GetHashCode() ^ this.mValue.GetHashCode();
        }
        #endregion

        #region Operators
        /// <summary>
        /// The == operator
        /// </summary>
        /// <param name="m1">Measurement 1</param>
        /// <param name="m2">Measurement 2</param>
        /// <returns>Returns true if the measured quantities are the same</returns>
        public static bool operator ==(Measurement m1, Measurement m2)
        {
            return m1.Equals(m2);
        }

        /// <summary>
        /// Determins if the specified object is equal to the current Measurement
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>True if the objects are equal</returns>
        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;
            Measurement m1 = (Measurement)obj;
            if (m1.Unit.Type != this.Unit.Type)
                return false;

            Measurement std1 = m1.ToStandardUnit();
            Measurement std2 = this.ToStandardUnit();

            if (std1.Unit == std2.Unit && Math.Round(std1.Value, 10) == Math.Round(std2.Value, 10))
                return true;
            else
                return false;
        }


        /// <summary>
        /// The != operator
        /// </summary>
        /// <param name="m1">Measurement 1</param>
        /// <param name="m2">Measurement 2</param>
        /// <returns>Returns true if the measured quantities are not the same</returns>
        public static bool operator !=(Measurement m1, Measurement m2)
        {
            return !(m1 == m2);
        }

        /// <summary>
        /// + Operator
        /// </summary>
        /// <param name="m1">Measurement 1</param>
        /// <param name="m2">Measurement 2</param>
        /// <returns>The resulting Measurement</returns>
        public static Measurement operator +(Measurement m1, Measurement m2)
        {
            if (m1.Unit == m2.Unit) // same unit
                return new Measurement(m1.Value + m2.Value, m1.Unit);
            else // different unit
            {
                Measurement m1Std = m1.ToStandardUnit();
                Measurement m2Std = m2.ToStandardUnit();
                return new Measurement(m1Std.Value + m2Std.Value, m1Std.Unit);
            }
        }

        /// <summary>
        /// - Operator
        /// </summary>
        /// <param name="m1">Measurement 1</param>
        /// <param name="m2">Measurement 2</param>
        /// <returns>The resulting measurement</returns>
        public static Measurement operator -(Measurement m1, Measurement m2)
        {
            if (m1.Unit == m2.Unit) // same unit
                return new Measurement(m1.Value - m2.Value, m1.Unit);
            else // different unit
            {
                Measurement m1Std = m1.ToStandardUnit();
                Measurement m2Std = m2.ToStandardUnit();
                return new Measurement(m1Std.Value - m2Std.Value, m1Std.Unit);
            }
        }

        /// <summary>
        /// / Operator
        /// </summary>
        /// <param name="m1">Measurement 1</param>
        /// <param name="m2">Measurement 2</param>
        /// <returns>The resulting measurement</returns>
        public static Measurement operator /(Measurement m1, Measurement m2)
        {
            if (m1.Unit == m2.Unit) // same unit
                return new Measurement(m1.Value / m2.Value, m1.Unit);
            else // different unit
            {
                Measurement m1Std = m1.ToStandardUnit();
                Measurement m2Std = m2.ToStandardUnit();
                return new Measurement(m1Std.Value / m2Std.Value, m1Std.Unit);
            }
        }


        /// <summary>
        /// * Operator
        /// </summary>
        /// <param name="m1">Measurement 1</param>
        /// <param name="m2">Measurement 2</param>
        /// <returns>The resulting Measurement</returns>
        public static Measurement operator *(Measurement m1, Measurement m2)
        {
            if (m1.Unit == m2.Unit) // same unit
                return new Measurement(m1.Value * m2.Value, m1.Unit);
            else // different unit
            {
                Measurement m1Std = m1.ToStandardUnit();
                Measurement m2Std = m2.ToStandardUnit();
                return new Measurement(m1Std.Value * m2Std.Value, m1Std.Unit);
            }
        }

        /// <summary>
        /// * Operator
        /// </summary>
        /// <param name="m1">The measurement</param>
        /// <param name="scalar">The scalar</param>
        /// <returns>The resulting measurement</returns>
        public static Measurement operator *(Measurement m1, double scalar)
        {
            return new Measurement(m1.Value * scalar, m1.Unit);
        }

        /// <summary>
        /// > Operator
        /// </summary>
        /// <param name="m1">Measurement 1</param>
        /// <param name="m2">Measurement 2</param>
        /// <returns>True if valid, otherwise false</returns>
        public static bool operator >(Measurement m1, Measurement m2)
        {
            if (m1 == null || m2 == null || !(m1 is Measurement && m2 is Measurement))
                return false;

            Measurement stdM1 = m1.ToStandardUnit();
            Measurement stdM2 = m2.ToStandardUnit();
            if (stdM1.mValue > stdM2.mValue)
                return true;
            else
                return false;
        }

        /// <summary>
        /// < Operator
        /// </summary>
        /// <param name="m1">Measurement 1</param>
        /// <param name="m2">Measurement 2</param>
        /// <returns>True if valid, otherwise false</returns>
        public static bool operator <(Measurement m1, Measurement m2)
        {
            if (m1 == null || m2 == null || !(m1 is Measurement && m2 is Measurement))
                return false;

            Measurement stdM1 = m1.ToStandardUnit();
            Measurement stdM2 = m2.ToStandardUnit();
            if (stdM1.mValue < stdM2.mValue)
                return true;
            else
                return false;
        }

        /// <summary>
        /// >= Operator
        /// </summary>
        /// <param name="m1">Measurement 1</param>
        /// <param name="m2">Measurement 2</param>
        /// <returns>True if valid, otherwise false</returns>
        public static bool operator >=(Measurement m1, Measurement m2)
        {
            return (m1 == m2 || m1 > m2);
        }

        /// <summary>
        /// <= Operator
        /// </summary>
        /// <param name="m1">Measurement 1</param>
        /// <param name="m2">Measurement 2</param>
        /// <returns>True if valid, otherwise false</returns>
        public static bool operator <=(Measurement m1, Measurement m2)
        {
            return (m1 == m2 || m1 < m2);
        }
        #endregion
    }
}
