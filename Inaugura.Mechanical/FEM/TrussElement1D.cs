﻿using System;
using System.Collections.Generic;
using System.Text;
using Inaugura.Measurement;

using MathNet.Numerics.LinearAlgebra;

namespace Inaugura.Mechanical.FEM
{
    public class TrussElement1D : Element
    {
        #region Variables
        private Node[] mNodes;
        private Measurement.Measurement mArea;
        #endregion

        #region Properties
        public Inaugura.Measurement.Measurement Length
        {
            get
            {
                Vector vector = new Vector(this.mNodes[0].Position, mNodes[1].Position);
                return vector.Length;
            }
        }

        /// <summary>
        /// The nodes which define the element
        /// </summary>
        public override Node[] Nodes
        {
            get
            {
                return this.mNodes;
            }
        }

        /// <summary>
        /// The cross sectional area
        /// </summary>
        public Inaugura.Measurement.Measurement Area
        {
            get
            {
                return this.mArea;
            }
        }

        /// <summary>
        /// The stiffness matrix coefficient
        /// </summary>
        public double Coefficient
        {
            get
            {
                return this.Area.Value * this.Material.ElasticModulus.Value / this.Length.Value;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="number">The element number</param>
        /// <param name="material">The material</param>
        /// <param name="area">The cross sectional area</param>
        /// <param name="node1">The global number for node 1</param>
        /// <param name="node2">The global number for node 2</param>
        public TrussElement1D(int number, Material material, Measurement.Measurement area, Node node1, Node node2) 
            : base(number,material)
        {
            this.mArea = area;
            mNodes = new Node[2];
            mNodes[0] = node1;
            mNodes[1] = node2;
        }


        public MathNet.Numerics.LinearAlgebra.Matrix CoefficientMatrix()
        {
            Matrix k = new Matrix(2, 2);
            k[0, 0] = 1;
            k[0, 1] = -1;
            k[1, 0] = -1;
            k[1, 1] = 1;

            return k;
        }
        
        /// <summary>
        /// Calculates the local stiffness matrix
        /// </summary>
        /// <returns></returns>
        public override MathNet.Numerics.LinearAlgebra.Matrix CalculateStiffnessMatrix()
        {
            MathNet.Numerics.LinearAlgebra.Matrix k = this.CoefficientMatrix();
            
            double c = this.Coefficient;            
            k.Multiply(c);
            return k;
        }

        public override MathNet.Numerics.LinearAlgebra.Matrix CalcualteTransformMatrix()
        {            
            Matrix k = new Matrix(2, 2);
            k[0, 0] = 1;
            k[0, 1] = 0;             
            k[1, 0] = 0;
            k[1, 1] = 1;
            
            return k;
        }

        public override MathNet.Numerics.LinearAlgebra.Matrix CalcualteGlobalDisplacementMatrix()
        {
            MathNet.Numerics.LinearAlgebra.Matrix globalDisplacementMatrix = new Matrix(this.Nodes.Length * 2, 1);

            int row = 0;
            foreach (Node n in this.Nodes)
            {
                globalDisplacementMatrix[row, 0] = n.Displacement.X.Value;              
                row+=1;
            }

            return globalDisplacementMatrix;
        }

        internal override void SetUnitSystem(Inaugura.Measurement.Unit.UnitSystem unitSystem)
        {
            if (unitSystem == Unit.UnitSystem.SI)
            {
                this.mArea = this.mArea.Convert(Unit.SquareMeter);
            }
            else if (unitSystem == Unit.UnitSystem.English)
            {
                this.mArea = this.mArea.Convert(Unit.SquareInch);
            }
            else
                throw new NotSupportedException();

            base.SetUnitSystem(unitSystem);
        }
        #endregion
    }
}
