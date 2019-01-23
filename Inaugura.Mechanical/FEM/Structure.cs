using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;

namespace Inaugura.Mechanical.FEM
{
    public class Structure<T> : Inaugura.Xml.IXmlable where T: Element
    {
        #region Variables
        private int mDOF;
        List<Material> mMaterials;
        List<Node> mNodes;
        List<T> mElements;
        private string mName;
        private Inaugura.Measurement.Unit.UnitSystem mUnitSystem = Inaugura.Measurement.Unit.UnitSystem.SI;
        #endregion

        #region Properties
        /// <summary>
        /// The nodes
        /// </summary>
        public Node[] Nodes
        {
            get
            {
                return this.mNodes.ToArray();
            }
        }

        /// <summary>
        /// The elements
        /// </summary>
        public T[] Elements
        {
            get
            {
                return this.mElements.ToArray();
            }
        }

        /// <summary>
        /// The name of the structure
        /// </summary>
        public string Name
        {
            get
            {
                return this.mName;
            }
            set
            {
                this.mName = value;
            }
        }

        private Inaugura.Measurement.Unit LengthUnit
        {
            get
            {
                if (this.mUnitSystem == Inaugura.Measurement.Unit.UnitSystem.SI)
                    return Inaugura.Measurement.Unit.Meter;
                else if (this.mUnitSystem == Inaugura.Measurement.Unit.UnitSystem.English)
                    return Inaugura.Measurement.Unit.Inch;
                else
                    throw new NotSupportedException();
            }
        }

        private Inaugura.Measurement.Unit ForceUnit
        {
            get
            {
                if (this.mUnitSystem == Inaugura.Measurement.Unit.UnitSystem.SI)
                    return Inaugura.Measurement.Unit.N;
                else if (this.mUnitSystem == Inaugura.Measurement.Unit.UnitSystem.English)
                    return Inaugura.Measurement.Unit.lb;
                else
                    throw new NotSupportedException();
            }
        }

        private Inaugura.Measurement.Unit PressureUnit
        {
            get
            {
                if (this.mUnitSystem == Inaugura.Measurement.Unit.UnitSystem.SI)
                    return Inaugura.Measurement.Unit.Pa;
                else if (this.mUnitSystem == Inaugura.Measurement.Unit.UnitSystem.English)
                    return Inaugura.Measurement.Unit.psi;
                else
                    throw new NotSupportedException();
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
                XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Structure");
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
            if (!string.IsNullOrEmpty(this.mName))
                Inaugura.Xml.Helper.SetAttribute(node, "name", this.mName);

            if (this.mNodes.Count > 0)
            {
                XmlNode nodesNode = node.OwnerDocument.CreateElement("Nodes");
                foreach (Node n in this.mNodes)
                {
                    XmlNode nodeNode = node.OwnerDocument.CreateElement("Node");
                    n.PopulateNode(nodeNode);
                    nodesNode.AppendChild(nodeNode);
                }
                node.AppendChild(nodesNode);
            }

            //if (this.mElements.Count > 0)
            //{
            //    XmlNode elementsNode = node.OwnerDocument.CreateElement("Elements");
            //    foreach (Element n in this.mElements)
            //    {
            //        XmlNode elementNode = node.OwnerDocument.CreateElement("Element");
            //        n.PopulateNode(nodeNode);
            //        elementsNode.AppendChild(elementNode);
            //    }
            //    node.AppendChild(elementsNode);
            //}
        }

        /// <summary>
        /// Populates the instance with the specifed xml data
        /// </summary>
        /// <param name="node">The xml node</param>
        public virtual void PopulateInstance(XmlNode node)
        {
            this.mName = Inaugura.Xml.Helper.GetAttribute(node, "name");
            
            string units = Inaugura.Xml.Helper.GetAttribute(node, "units");
            if (string.Compare(units, "SI", StringComparison.OrdinalIgnoreCase) == 0)
                this.mUnitSystem = Inaugura.Measurement.Unit.UnitSystem.SI;
            else if (string.Compare(units, "English", StringComparison.OrdinalIgnoreCase) == 0)
                this.mUnitSystem = Inaugura.Measurement.Unit.UnitSystem.English;

            this.mDOF = int.Parse(Inaugura.Xml.Helper.GetAttribute(node, "dof"));

            // load the materials
            this.mMaterials.Clear();
            XmlNodeList nodes = node.SelectNodes("Materials/Material");
            foreach (XmlNode n in nodes)
                this.mMaterials.Add(new Material(n));

            // load the nodes
            this.mNodes.Clear();
            nodes = node.SelectNodes("Nodes/Node");
            foreach (XmlNode n in nodes)
                this.mNodes.Add(new Node(n));

            // make sure the node numbers are unique
            this.mElements.Clear();
            XmlNodeList elementNodes = node.SelectNodes("Elements/Element");
            foreach (XmlNode n in elementNodes)
            {                
                int elementNumber = int.Parse(Inaugura.Xml.Helper.GetAttribute(n, "number"));

                string strMaterial = Inaugura.Xml.Helper.GetAttribute(n, "material");

                Material material = null;

                // look for the material in the materials list
                foreach (Material m in this.mMaterials)
                    if (string.Compare(strMaterial, m.Name, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        material = m;
                        break;
                    }

                if (material == null)
                    throw new ApplicationException("You must define a meterial for each element");

                int node1Number = int.Parse(Inaugura.Xml.Helper.GetAttribute(n, "node1"));
                int node2Number = int.Parse(Inaugura.Xml.Helper.GetAttribute(n, "node2"));

                // find the nodes
                Node node1 = null;
                Node node2 = null;

                foreach (Node structuralNode in mNodes)
                {
                    if (structuralNode.Number == node1Number)
                        node1 = structuralNode;
                    if (structuralNode.Number == node2Number)
                        node2 = structuralNode;
                }

                Measurement.Measurement area = Measurement.Measurement.Parse(Inaugura.Xml.Helper.GetAttribute(n, "area"));

                if (node1 == null)
                    throw new Exception(string.Format("The node '{0}' was not found", node1Number));

                if (node2 == null)
                    throw new Exception(string.Format("The node '{0}' was not found", node2Number));

                this.mElements.Add(new TrussElement1D(elementNumber, material, area, node1, node2) as T);
            }
        }
        #endregion

        #region Methods
        public Structure(int degreesOfFreedom)
        {
            this.mNodes = new List<Node>();
            this.mMaterials = new List<Material>();
            this.mElements = new List<T>();
            this.mDOF = degreesOfFreedom;
        }

        public Structure(XmlNode node)
            : this(0)
        {
            this.PopulateInstance(node);

            this.SetUnits();
        }

        public void SetUnits()
        {
            foreach (Material material in this.mMaterials)
            {
                if (this.mUnitSystem == Inaugura.Measurement.Unit.UnitSystem.SI)
                    material.ElasticModulus = material.ElasticModulus.Convert(Inaugura.Measurement.Unit.Pa);
                else if (this.mUnitSystem == Inaugura.Measurement.Unit.UnitSystem.English)
                    material.ElasticModulus = material.ElasticModulus.Convert(Inaugura.Measurement.Unit.psi);
                else
                    throw new NotSupportedException();
            }

            foreach (Node node in this.mNodes)
                node.SetUnitSystem(this.mUnitSystem);
        }

        /// <summary>
        /// Calculates the global stiffness matrix
        /// </summary>
        /// <returns></returns>
        public MathNet.Numerics.LinearAlgebra.Matrix CalculateStiffnessMatrix()
        {
            MathNet.Numerics.LinearAlgebra.Matrix k = new MathNet.Numerics.LinearAlgebra.Matrix(this.mNodes.Count * this.mDOF, this.mNodes.Count * this.mDOF);

            foreach (T element in this.mElements)
            {
                MathNet.Numerics.LinearAlgebra.Matrix elementK = element.CalculateStiffnessMatrix();

                for (int i = 0; i < element.Nodes.Length; i++)
                {
                    int globalNumber = element.Nodes[i].Number - 1;

                    for (int r = 0; r < this.mDOF; r++)
                        for (int c = 0; c < this.mDOF; c++)
                            k[globalNumber * this.mDOF + r, globalNumber * this.mDOF + c] += elementK[i * this.mDOF + r, i * this.mDOF + c];

                    for (int j = 0; j < element.Nodes.Length; j++)
                        if (j != i)
                        {
                            int gNumber = element.Nodes[j].Number - 1;

                            for (int r = 0; r < this.mDOF; r++)
                                for (int c = 0; c < this.mDOF; c++)
                                    k[gNumber * this.mDOF + r, globalNumber * this.mDOF + c] += elementK[j * this.mDOF + r, i * this.mDOF + c];
                        }
                }                
            }
            return k;
        }

        /// <summary>
        /// Calculates the global force matrix
        /// </summary>
        /// <returns></returns>
        public MathNet.Numerics.LinearAlgebra.Matrix CalculateForceMatrix()
        {
            // construct the force matrix
            MathNet.Numerics.LinearAlgebra.Matrix f = new MathNet.Numerics.LinearAlgebra.Matrix(this.mNodes.Count * this.mDOF, 1);
            for (int r = 0; r < this.mNodes.Count; r++)
            {
                Node n = this.mNodes[r];
                if (n.Force.X != Measurement.Measurement.Empty)
                    f[r * this.mDOF, 0] = n.Force.X.Value;

                if(this.mDOF >=2)
                    if (n.Force.Y != Measurement.Measurement.Empty)
                        f[r * this.mDOF + 1, 0] = n.Force.Y.Value;

                if(this.mDOF >= 3)
                    if (n.Force.Z != Measurement.Measurement.Empty)
                        f[r * this.mDOF + 2, 0] = n.Force.Z.Value;
            }
            return f;
        }

        /// <summary>
        /// Calculates the gobal displacement matrix
        /// </summary>
        /// <returns>The displacement matrix</returns>
        public MathNet.Numerics.LinearAlgebra.Matrix CalculateDisplacementMatrix()
        {
            foreach (Node n in this.mNodes)
                n.Reset();

            MathNet.Numerics.LinearAlgebra.Matrix globalK = this.CalculateStiffnessMatrix();
            MathNet.Numerics.LinearAlgebra.Matrix globalF = this.CalculateForceMatrix();

            // now use the penalty approach to apply boundary conditions
            double coeff = 0;
            for (int r = 0; r < globalK.RowCount; r++)
                for (int c = 0; c < globalK.ColumnCount; c++)
                    if (coeff < globalK[r, c])
                        coeff = globalK[r, c];

            coeff *= 10000;

            foreach (Inaugura.Mechanical.FEM.Node n in mNodes)
            {
                int globalNumber = n.Number - 1;
                if (n.Displacement.X != Inaugura.Measurement.Measurement.Empty)
                {
                    globalK[globalNumber * this.mDOF, globalNumber * this.mDOF] += coeff;
                    globalF[globalNumber * this.mDOF, 0] += n.Displacement.X.Value * coeff;
                }

                if (this.mDOF >= 2 && n.Displacement.Y != Inaugura.Measurement.Measurement.Empty)
                {
                    globalK[globalNumber * this.mDOF + 1, globalNumber * this.mDOF + 1] += coeff;
                    globalF[globalNumber * this.mDOF + 1, 0] += n.Displacement.Y.Value * coeff;
                }

                if (this.mDOF >= 3 && n.Displacement.Z != Inaugura.Measurement.Measurement.Empty)
                {
                    globalK[globalNumber * this.mDOF + 2, globalNumber * this.mDOF + 2] += coeff;
                    globalF[globalNumber * this.mDOF + 2, 0] += n.Displacement.Z.Value * coeff;
                }
            }

            MathNet.Numerics.LinearAlgebra.Matrix inverseGlobalK = globalK.Inverse();
            MathNet.Numerics.LinearAlgebra.Matrix globalQ = inverseGlobalK * globalF;
            return globalQ;
        }

        /// <summary>
        /// Calculates the displacement matrix
        /// </summary>
        public void CalculateNodalDisplacements()
        {
            MathNet.Numerics.LinearAlgebra.Matrix globalQ = this.CalculateDisplacementMatrix();

            Inaugura.Measurement.Unit lengthUnit = this.LengthUnit;
                       
            foreach (Inaugura.Mechanical.FEM.Node n in mNodes)
            {
                Measurement.Measurement x = Measurement.Measurement.Empty;
                Measurement.Measurement y = Measurement.Measurement.Empty;
                Measurement.Measurement z = Measurement.Measurement.Empty;

                int globalNumber = n.Number - 1;
                if (n.Displacement.X == Inaugura.Measurement.Measurement.Empty)
                    x = new Inaugura.Measurement.Measurement(globalQ[globalNumber * this.mDOF, 0], lengthUnit);
                if (this.mDOF >=2 && n.Displacement.Y == Inaugura.Measurement.Measurement.Empty)
                    y = new Inaugura.Measurement.Measurement(globalQ[globalNumber * this.mDOF + 1, 0], lengthUnit);
                if (this.mDOF >= 3 && n.Displacement.Z == Inaugura.Measurement.Measurement.Empty)
                    z = new Inaugura.Measurement.Measurement(globalQ[globalNumber * this.mDOF + 2, 0], lengthUnit);
                    
                n.Displacement = new Inaugura.Measurement.Vector(x, y, z);
            }
        }
        

        /// <summary>
        /// Calculates the reaction forces
        /// </summary>
        public void CalculateReactionForces()
        {
            MathNet.Numerics.LinearAlgebra.Matrix globalK = this.CalculateStiffnessMatrix();
            MathNet.Numerics.LinearAlgebra.Matrix globalQ = this.CalculateDisplacementMatrix();

            List<int> rowsToRemove = new List<int>();
           
            foreach (Inaugura.Mechanical.FEM.Node n in mNodes)
            {
                int globalNumber = n.Number - 1;
                if (n.Displacement.X == Inaugura.Measurement.Measurement.Empty)
                    rowsToRemove.Add(globalNumber*this.mDOF);
                if (this.mDOF >=2 && n.Displacement.Y == Inaugura.Measurement.Measurement.Empty)
                    rowsToRemove.Add(globalNumber*this.mDOF + 1);
                if (this.mDOF >=3 && n.Displacement.Y == Inaugura.Measurement.Measurement.Empty)
                    rowsToRemove.Add(globalNumber * this.mDOF + 2);
            }

            // remove the rows
            MathNet.Numerics.LinearAlgebra.Matrix newK = new MathNet.Numerics.LinearAlgebra.Matrix(globalK.RowCount - rowsToRemove.Count, globalK.ColumnCount);

            int rowCount =0;
            for (int r = 0; r < globalK.RowCount; r++)
            {
                if (!rowsToRemove.Contains(r))
                {
                    for (int c = 0; c < newK.ColumnCount; c++)
                        newK[rowCount, c] = globalK[r, c];                  
                    rowCount++;
                }
            }

            MathNet.Numerics.LinearAlgebra.Matrix globalR = newK * globalQ;            

            foreach (Node n in this.mNodes)
            {
                n.ReactionForce = new Inaugura.Measurement.Vector(Measurement.Measurement.Empty, Measurement.Measurement.Empty, Measurement.Measurement.Empty);
            }

            Inaugura.Measurement.Unit forceUnit = this.ForceUnit;

            rowCount = 0;
            foreach (Inaugura.Mechanical.FEM.Node n in mNodes)
            {                
                Measurement.Measurement x = Measurement.Measurement.Empty;
                Measurement.Measurement y = Measurement.Measurement.Empty;
                Measurement.Measurement z = Measurement.Measurement.Empty;

                int globalNumber = n.Number - 1;
                if (n.Displacement.X != Inaugura.Measurement.Measurement.Empty)
                {
                    x = new Inaugura.Measurement.Measurement(globalR[rowCount, 0], forceUnit);
                    rowCount++;
                }                
                if (this.mDOF >= 2 && n.Displacement.Y != Inaugura.Measurement.Measurement.Empty)
                {
                    y = new Inaugura.Measurement.Measurement(globalR[rowCount, 0], forceUnit);
                    rowCount++;
                }
                if (this.mDOF >= 3 && n.Displacement.Y != Inaugura.Measurement.Measurement.Empty)
                {
                    z = new Inaugura.Measurement.Measurement(globalR[rowCount, 0], forceUnit);
                    rowCount++;
                }
                n.ReactionForce = new Inaugura.Measurement.Vector(x, y, z);
            }
        }
        #endregion
    }
}
