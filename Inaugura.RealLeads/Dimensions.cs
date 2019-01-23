using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Inaugura.Measurement;

namespace Inaugura.RealLeads
{
	#region Unit Test
#if UnitTest
	using NUnit.Framework;
	/// <summary>Some simple Tests.</summary>
	/// 
	[TestFixture]
	public class DimensionsTest
	{
		private Dimensions mDimension1;
		private Dimensions mDimension2;
		/// <summary>
		/// Setups up the test
		/// </summary>
		[SetUp]
		public void Init()
		{
			this.mDimension1 = new Dimensions(new Measurement(100, Unit.StandardUnitLength), new Measurement(100,Unit.StandardUnitLength));
			this.mDimension2 = new Dimensions(new Measurement(200, Unit.StandardUnitLength), new Measurement(200, Unit.StandardUnitLength));
		}

		/// <summary>
		/// Tests the xml parsing functionality
		/// </summary>
		[Test]
		public void XmlParsing()
		{
			Dimensions xmlDimension = new Dimensions(this.mDimension1.Xml);
			
			// forced failure result == 5
			Assert.IsTrue(xmlDimension == this.mDimension1, "The dimensions are not equal");			
		}


		/// <summary>
		/// Tests the xml parsing functionality
		/// </summary>
		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void AcceptOnlySingleDimension()
		{
			Dimensions d1 = new Dimensions(new Measurement(100, Unit.StandardUnitArea), new Measurement(100, Unit.StandardUnitLength));			
		}
		
		[Test]
		public void UnitConversion()
		{
			Dimensions newMeasure = new Dimensions(this.mDimension1.Depth.Convert(Unit.Feet), this.mDimension1.Width.Convert(Unit.Feet));
			Assert.IsTrue(newMeasure.Width == this.mDimension1.Width, string.Format("The dimensions are not equal\n{0}\n{1}",newMeasure.Xml.OuterXml,this.mDimension1.Xml.OuterXml));			
		}

		[Test]
		public void Equality()
		{
			Dimensions d1 = new Dimensions(new Measurement(100, Unit.StandardUnitLength), new Measurement(100, Unit.StandardUnitLength));
			Dimensions d2 = new Dimensions(new Measurement(100, Unit.StandardUnitLength), new Measurement(100, Unit.StandardUnitLength));
			Assert.IsTrue(d1 == d2, "The dimensions should be equal");

			d1 = new Dimensions(new Measurement(d1.Width.Value+1,d1.Width.Unit),d1.Depth);
			Assert.IsFalse(d1 == d2, "The dimensions should not be equal");

			d1 = new Dimensions(new Measurement(100, Unit.Meter), new Measurement(200, Unit.Meter));
			d2 = new Dimensions(new Measurement(200, Unit.Meter), new Measurement(100, Unit.Meter));
			Assert.IsFalse(d1 == d2, "The dimensions should not be equal");
		}

		[Test]
		public void AreaTest()
		{
			Dimensions d1 = new Dimensions(new Measurement(100, Unit.Meter), new Measurement(200, Unit.Meter));
			Dimensions d2 = new Dimensions(new Measurement(200, Unit.Meter), new Measurement(100, Unit.Meter));

			Assert.IsTrue(d1.Area == d2.Area, "The dimensional areas should be equal");
		}

	}
#endif
	#endregion

    public sealed class Dimensions : Inaugura.Xml.IXmlable
	{
        /// <summary>
        /// Length units used in measuring interior dimensions
        /// </summary>
        public static Inaugura.Measurement.Unit.UnitCollection InteriorLengthUnits = new Inaugura.Measurement.Unit.UnitCollection(new Inaugura.Measurement.Unit[] { Inaugura.Measurement.Unit.Feet, Inaugura.Measurement.Unit.Meter });
        /// <summary>
        /// Length units used in measuring exterior dimensions
        /// </summary>
        public static Inaugura.Measurement.Unit.UnitCollection ExteriorLengthUnits = new Inaugura.Measurement.Unit.UnitCollection(new Inaugura.Measurement.Unit[] { Inaugura.Measurement.Unit.Feet, Inaugura.Measurement.Unit.Meter, Inaugura.Measurement.Unit.Yard });
        /// <summary>
        /// Area units used in measuring interior dimensions
        /// </summary>
        public static Inaugura.Measurement.Unit.UnitCollection InteriorAreaUnits = new Inaugura.Measurement.Unit.UnitCollection(new Inaugura.Measurement.Unit[] { Inaugura.Measurement.Unit.SquareFeet, Inaugura.Measurement.Unit.SquareMeter });
        /// <summary>
        /// Area units used in measuring exterior dimensions
        /// </summary>
        public static Inaugura.Measurement.Unit.UnitCollection ExteriorAreaUnits = new Inaugura.Measurement.Unit.UnitCollection(new Inaugura.Measurement.Unit[] { Inaugura.Measurement.Unit.SquareFeet, Inaugura.Measurement.Unit.SquareMeter, Inaugura.Measurement.Unit.Acre });

		#region Variables
		internal const string WidthNodeName = "Width";
		internal const string DepthNodeName = "Depth";
        private Inaugura.Measurement.Measurement mWidth;
        private Inaugura.Measurement.Measurement mDepth;
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
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Dimensions");
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
			XmlNode widthNode = node.OwnerDocument.CreateElement(WidthNodeName);
            mWidth.PopulateNode(widthNode);
			node.AppendChild(widthNode);

			XmlNode depthNode = node.OwnerDocument.CreateElement(DepthNodeName);
            mDepth.PopulateNode(depthNode);
			node.AppendChild(depthNode);

            Inaugura.Xml.Helper.SetAttribute(node, "area", this.Area.ToString());
		}

		/// <summary>
		/// Populates the instance with the specifed xml data
		/// </summary>
		/// <param name="node"></param>
		public void PopulateInstance(XmlNode node)
		{
			if (node[WidthNodeName] == null)
				throw new ArgumentException("The xml does not contain the width node");

			if(node[DepthNodeName] == null)
				throw new ArgumentException("The xml does not contain the depth node");

            this.Width = new Inaugura.Measurement.Measurement(node[WidthNodeName]);
            this.Depth = new Inaugura.Measurement.Measurement(node[DepthNodeName]);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the area
		/// </summary>
		/// <value></value>
        public Inaugura.Measurement.Measurement Area
		{
			get
			{
                Measurement.Measurement area = Measurement.Measurement.Empty;
                if (this.Width.Unit != this.mDepth.Unit)
                {
                    Inaugura.Measurement.Measurement width = this.Width.ToStandardUnit();
                    Inaugura.Measurement.Measurement depth = this.Depth.ToStandardUnit();

                    double value = width.Value * depth.Value;

                    area = new Inaugura.Measurement.Measurement(value, Inaugura.Measurement.Unit.SquareMeter);

                    if (this.Width.Unit.System == Inaugura.Measurement.Unit.UnitSystem.English)
                        area = area.Convert(Inaugura.Measurement.Unit.SquareFeet);
                }
                else // the units match
                {
                    double value = this.Width.Value * this.Depth.Value;
                    Inaugura.Measurement.Unit unit = null;
                    if (this.Width.Unit == Inaugura.Measurement.Unit.Feet)
                        unit = Inaugura.Measurement.Unit.SquareFeet;
                    else // meters
                        unit = Inaugura.Measurement.Unit.SquareMeter;
                    area = new Inaugura.Measurement.Measurement(value, unit);
                }
                area.Value = Math.Round(area.Value, 1);
                return area;
			}
		}

		/// <summary>
		/// The width measurement
		/// </summary>
        public Inaugura.Measurement.Measurement Width 
		{
			get
			{
				return this.mWidth;
			}
			set
			{
                if (value.Unit.Type != Inaugura.Measurement.Unit.UnitType.Length)
					throw new ArgumentException("The measurement must be a unit of length");

				this.mWidth = value;
                this.mWidth.Value = Math.Round(this.mWidth.Value, 1);
			}
		}
		
		/// <summary>
		/// The depth measurement
		/// </summary>
        public Inaugura.Measurement.Measurement Depth
		{
			get
			{
				return this.mDepth;
			}
            set
            {
                if (value.Unit.Type != Inaugura.Measurement.Unit.UnitType.Length)
                    throw new ArgumentException("The measurement must be a unit of length");

                this.mDepth = value;
                this.mDepth.Value = Math.Round(this.mDepth.Value, 1);
            }
		}	
		#endregion

		#region Methods
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dimensionsNode">The underlying xml node</param>
		public Dimensions(XmlNode dimensionsNode)
		{
			this.PopulateInstance(dimensionsNode);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="width">The width measurement</param>
		/// <param name="depth">The depth measurement</param>
        public Dimensions(Inaugura.Measurement.Measurement width, Inaugura.Measurement.Measurement depth)
		{
			this.Width = width;
			this.Depth = depth;
		}

		/// <summary>
		/// Constructor
		/// </summary>
        public Dimensions()
            : this(new Inaugura.Measurement.Measurement(0, Inaugura.Measurement.Unit.Feet), new Inaugura.Measurement.Measurement(0, Inaugura.Measurement.Unit.Feet))
		{
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
			Dimensions d = (Dimensions)obj;
			return (d.Width == this.Width) && (d.Depth == this.Depth);			
		}

		/// <summary>
		/// Serves as a hash function for a particular type
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return this.Depth.GetHashCode() ^ this.Width.GetHashCode();
		}

		/// <summary>
		/// The == operator
		/// </summary>
		/// <param name="d1">Dimensions 1</param>
		/// <param name="d2">Dimensions 2</param>
		/// <returns>Returns true if the measured quantities are the same</returns>
		public static bool operator ==(Dimensions d1, Dimensions d2)
		{
			return d1.Equals(d2);
		}

		/// <summary>
		/// The != operator
		/// </summary>
		/// <param name="d1">Dimensions 1</param>
		/// <param name="d2">Dimensions 2</param>
		/// <returns>Returns true if the measured quantities are not the same</returns>
		public static bool operator !=(Dimensions d1, Dimensions d2)
		{
			return !(d1 == d2);
		}
		#endregion
	}
}
