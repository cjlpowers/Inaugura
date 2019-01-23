using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Mechanical.FEM
{
    /// <summary>
    /// An abstract class representing a element
    /// </summary>
    public abstract class Element
    {
        #region Variables
        private int mNumber;
        private Material mMaterial;
        #endregion

        #region Properties
        /// <summary>
        /// The element material
        /// </summary>
        public Material Material
        {
            get
            {
                return this.mMaterial;
            }
        }

        /// <summary>
        /// The nodes which define the element
        /// </summary>
        public abstract Node[] Nodes
        {
            get;
        }
        #endregion

        #region Methods
        public Element(int number, Material material)
        {
            this.mNumber = number;

            // Make sure the material is not null
            if (material == null)
                throw new ArgumentNullException("material");
            
            this.mMaterial = material;
        }

        public override string ToString()
        {
            return string.Format("Element {0}", this.mNumber);
        }


        /// <summary>
        /// Calculates the local stiffness matrix
        /// </summary>
        /// <returns></returns>
        public abstract MathNet.Numerics.LinearAlgebra.Matrix CalculateStiffnessMatrix();

        public abstract MathNet.Numerics.LinearAlgebra.Matrix CalcualteTransformMatrix();

        public abstract MathNet.Numerics.LinearAlgebra.Matrix CalcualteGlobalDisplacementMatrix();
        
        public virtual MathNet.Numerics.LinearAlgebra.Matrix CalcualteLocalDisplacementMatrix()
        {
            MathNet.Numerics.LinearAlgebra.Matrix transformMatrix = this.CalcualteTransformMatrix();

            MathNet.Numerics.LinearAlgebra.Matrix globalDisplacementMatrix = this.CalcualteGlobalDisplacementMatrix();

            return transformMatrix * globalDisplacementMatrix;
        }

        internal virtual void SetUnitSystem(Inaugura.Measurement.Unit.UnitSystem unitSystem)
        {
        }        
        #endregion
    }
}
