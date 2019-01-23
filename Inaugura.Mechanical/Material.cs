using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;

namespace Inaugura.Mechanical
{
    /// <summary>
    /// A class which represents a material
    /// </summary>
    public class Material: Inaugura.Xml.IXmlable
    {
        #region Material Definitions
        /// <summary>
        /// Aluminium
        /// </summary>
        public static Material Aluminium = new Material("Aluminium", new Inaugura.Measurement.Measurement(69, Inaugura.Measurement.Unit.GPa));
        /// <summary>
        /// Steel
        /// </summary>
        public static Material Steel = new Material("Steel", new Inaugura.Measurement.Measurement(200, Inaugura.Measurement.Unit.GPa));
        /// <summary>
        /// Glass
        /// </summary>
        public static Material Glass = new Material("Glass", new Inaugura.Measurement.Measurement(72, Inaugura.Measurement.Unit.GPa));

        /// <summary>
        /// The list of all materials
        /// </summary>
        public static Material[] AllMaterials = new Material[] { Aluminium, Steel, Glass };
        #endregion

        #region Variables
        private string mName;
        private Measurement.Measurement mElasticModulus;
        #endregion

        #region Properties
        /// <summary>
        /// The name of the material
        /// </summary>
        public string Name
        {
            get
            {
                return this.mName;
            }
        }

        /// <summary>
        /// The elastic modulus
        /// </summary>
        public Measurement.Measurement ElasticModulus
        {
            get
            {
                return this.mElasticModulus;
            }
            internal set
            {
                this.mElasticModulus = value;
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
                XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Material");
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
            Inaugura.Xml.Helper.SetAttribute(node, "name", this.mName);
            Inaugura.Xml.Helper.SetAttribute(node, "elasticModulus", this.ElasticModulus.ToString());
        }

        /// <summary>
        /// Populates the instance with the specifed xml data
        /// </summary>
        /// <param name="node">The xml node</param>
        public virtual void PopulateInstance(XmlNode node)
        {
            this.mName = Inaugura.Xml.Helper.GetAttribute(node, "name");
            this.mElasticModulus = Measurement.Measurement.Parse(Inaugura.Xml.Helper.GetAttribute(node, "elasticModulus"));
            if (this.mElasticModulus.Unit.System == Inaugura.Measurement.Unit.UnitSystem.English)
                this.mElasticModulus = this.mElasticModulus.Convert(Inaugura.Measurement.Unit.psi);
            else if (this.mElasticModulus.Unit.System == Inaugura.Measurement.Unit.UnitSystem.SI)
                this.mElasticModulus = this.mElasticModulus.Convert(Inaugura.Measurement.Unit.Pa);
            else
                throw new NotSupportedException("The unit was not supported");
        }
        #endregion

        #region Methods
        public Material(string name, Measurement.Measurement elasticModulus)
        {
            this.mName = name;

            // make sure the elastic modulus is not null
            if (elasticModulus == null)
                throw new ArgumentNullException("elasticModulus");

            // make sure the elastic modulus is a unit of pressure
            if (elasticModulus.Unit.Type != Inaugura.Measurement.Unit.UnitType.Pressure)
                throw new ArgumentException("The elastic modulus must be in units of pressure");

            this.mElasticModulus = elasticModulus;
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">The xml node</param>
        public Material(XmlNode node) : this(string.Empty, new Inaugura.Measurement.Measurement(0,Inaugura.Measurement.Unit.Pa))
        {
            this.PopulateInstance(node);
        }

        /// <summary>
        /// To string method
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Name, this.ElasticModulus);
        }
        #endregion
    }
}
