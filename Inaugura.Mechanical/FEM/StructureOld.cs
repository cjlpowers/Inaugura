using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;

namespace Inaugura.Mechanical.FEM
{
    /// <summary>
    /// A class representing a structure
    /// </summary>
    public class Structure : Inaugura.Xml.IXmlable
    {
        #region Variables
        List<Material> mMaterials;
        List<Node> mNodes;
        List<Element> mElements;
        private string mName;
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
        public Element[] Elements
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
                    if (string.Compare(strMaterial, m.Name, StringComparison.OrdinalIgnoreCase)==0)
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

                this.mElements.Add(new TrussElement2D(elementNumber, material, area, node1, node2));
            }
        }
        #endregion

        #region Methods
        public Structure()
        {
            this.mNodes = new List<Node>();
            this.mMaterials = new List<Material>();
            this.mElements = new List<Element>();
        }

        public Structure(XmlNode node) : this()
        {
            this.PopulateInstance(node);
        }

        /// <summary>
        /// Calculates the global stiffness matrix
        /// </summary>
        /// <returns></returns>
        public MathNet.Numerics.LinearAlgebra.Matrix CalculateStiffnessMatrix()
        {
            MathNet.Numerics.LinearAlgebra.Matrix k = new MathNet.Numerics.LinearAlgebra.Matrix(this.mNodes.Count * 2, this.mNodes.Count * 2);

            foreach (Element element in this.mElements)
            {
                MathNet.Numerics.LinearAlgebra.Matrix elementK = element.CalculateStiffnessMatrix();

                //elementK.Multiply(1/((TrussElement2D)element).Coefficient);

                for (int i = 0; i < element.Nodes.Length; i++)
                {
                    int globalNumber = element.Nodes[i].Number - 1;

                    for (int r = 0; r < 2; r++)
                        for (int c = 0; c < 2; c++)
                            k[globalNumber * 2 + r, globalNumber * 2 + c] += elementK[i * 2 + r, i * 2 + c];

                    for (int j = 0; j < element.Nodes.Length; j++)
                        if (j != i)
                        {
                            int gNumber = element.Nodes[j].Number - 1;

                            for (int r = 0; r < 2; r++)
                                for (int c = 0; c < 2; c++)
                                    k[gNumber * 2 + r, globalNumber * 2 + c] += elementK[j * 2 + r, i * 2 + c];

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
            MathNet.Numerics.LinearAlgebra.Matrix f = new MathNet.Numerics.LinearAlgebra.Matrix(this.mNodes.Count * 2, 1);
            for (int r = 0; r < this.mNodes.Count; r++)
            {
                Node n = this.mNodes[r];
                if (n.Force.X != Measurement.Measurement.Empty)
                    f[r * 2, 0] = n.Force.X.Value;

                if (n.Force.Y != Measurement.Measurement.Empty)
                    f[r * 2 + 1, 0] = n.Force.Y.Value;
            }

            return f;
        }

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
                    globalK[globalNumber * 2, globalNumber * 2] += coeff;
                    globalF[globalNumber * 2, 0] += n.Displacement.X.Value * coeff;
                }

                if (n.Displacement.Y != Inaugura.Measurement.Measurement.Empty)
                {
                    globalK[globalNumber * 2 + 1, globalNumber * 2 + 1] += coeff;
                    globalF[globalNumber * 2 + 1, 0] += n.Displacement.X.Value * coeff;
                }
            }

            MathNet.Numerics.LinearAlgebra.Matrix inverseGlobalK = globalK.Inverse();
            MathNet.Numerics.LinearAlgebra.Matrix globalQ = inverseGlobalK * globalF;
            return globalQ;
        }

        public void CalculateNodalDisplacements()
        {
            MathNet.Numerics.LinearAlgebra.Matrix globalQ = this.CalculateDisplacementMatrix();

            foreach (Inaugura.Mechanical.FEM.Node n in mNodes)
            {
                Measurement.Measurement x = Measurement.Measurement.Empty;
                Measurement.Measurement y = Measurement.Measurement.Empty;

                int globalNumber = n.Number - 1;
                if (n.Displacement.X == Inaugura.Measurement.Measurement.Empty)
                    x = new Inaugura.Measurement.Measurement(globalQ[globalNumber*2, 0], Measurement.Unit.Inch);
                if (n.Displacement.Y == Inaugura.Measurement.Measurement.Empty)
                    y = new Inaugura.Measurement.Measurement(globalQ[globalNumber*2+1, 0], Measurement.Unit.Inch);
                    
                n.Displacement = new Inaugura.Measurement.Vector(x, y, Measurement.Measurement.Empty);
            }
        }
        
        public void CalculateReactionForces()
        {
            MathNet.Numerics.LinearAlgebra.Matrix globalK = this.CalculateStiffnessMatrix();
            MathNet.Numerics.LinearAlgebra.Matrix globalQ = this.CalculateDisplacementMatrix();

            List<int> rowsToRemove = new List<int>();
           
            foreach (Inaugura.Mechanical.FEM.Node n in mNodes)
            {
                int globalNumber = n.Number - 1;
                if (n.Displacement.X == Inaugura.Measurement.Measurement.Empty)
                    rowsToRemove.Add(globalNumber*2);
                if (n.Displacement.Y == Inaugura.Measurement.Measurement.Empty)
                    rowsToRemove.Add(globalNumber*2 + 1);
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

            rowCount = 0;
            foreach (Inaugura.Mechanical.FEM.Node n in mNodes)
            {                
                Measurement.Measurement x = Measurement.Measurement.Empty;
                Measurement.Measurement y = Measurement.Measurement.Empty;

                int globalNumber = n.Number - 1;
                if (n.Displacement.X != Inaugura.Measurement.Measurement.Empty)
                {
                    x = new Inaugura.Measurement.Measurement(globalR[rowCount,0], Measurement.Unit.lb);
                    rowCount++;
                }                
                if (n.Displacement.Y != Inaugura.Measurement.Measurement.Empty)
                {
                    y = new Inaugura.Measurement.Measurement(globalR[rowCount,0], Measurement.Unit.lb);
                    rowCount++;
                }

                n.ReactionForce = new Inaugura.Measurement.Vector(x, y, Measurement.Measurement.Empty);
            }
        }
        #endregion
    }
}
