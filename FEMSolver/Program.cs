using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;

namespace FEMSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(args[0]);

            //Inaugura.Mechanical.FEM.Structure structure = new Inaugura.Mechanical.FEM.Structure(xmlDoc["Structure"]);
            Inaugura.Mechanical.FEM.Structure<Inaugura.Mechanical.FEM.TrussElement1D> structure = new Inaugura.Mechanical.FEM.Structure<Inaugura.Mechanical.FEM.TrussElement1D>(xmlDoc["Structure"]);

            Console.WriteLine("Structure '{0}' Loaded ({1} nodes, {2} elements)", structure.Name, structure.Nodes.Length, structure.Elements.Length);

            foreach (Inaugura.Mechanical.FEM.Element element in structure.Elements)
            {
                Console.WriteLine("Computing Stiffness Matrix for '{0}'", element);

                Inaugura.Mechanical.FEM.TrussElement1D truss = element as Inaugura.Mechanical.FEM.TrussElement1D;
                if (truss != null)
                {

                    double coefficient = truss.Coefficient;
                    Console.WriteLine("[K] = {0:0.000e+00}", coefficient);
                    Console.WriteLine("x");
                    Console.WriteLine(truss.CoefficientMatrix());
                }
                else
                {
                    MathNet.Numerics.LinearAlgebra.Matrix k = element.CalculateStiffnessMatrix();
                    Console.WriteLine(k);
                }

                Console.WriteLine("Computing Transform Matrix for '{0}'", element);
                Console.WriteLine(element.CalcualteTransformMatrix());

            }


            Console.WriteLine("Computing Global Stiffness Matrix");
            MathNet.Numerics.LinearAlgebra.Matrix globalK = structure.CalculateStiffnessMatrix();
            Console.WriteLine(globalK);

            Console.WriteLine("Computing Force Matrix");
            MathNet.Numerics.LinearAlgebra.Matrix globalF = structure.CalculateForceMatrix();
            Console.WriteLine(globalF);

            Console.WriteLine("Computing Displacement Matrix");
            MathNet.Numerics.LinearAlgebra.Matrix globalQ = structure.CalculateDisplacementMatrix();
            Console.WriteLine(globalQ);
                       
            Console.WriteLine("Computing Nodal Displacements");
            structure.CalculateNodalDisplacements();
            foreach (Inaugura.Mechanical.FEM.Node node in structure.Nodes)
            {
                Console.WriteLine("{0}: {1}", node, node.Displacement);
            }

            Console.WriteLine("Computing Element Stresses");
            foreach (Inaugura.Mechanical.FEM.Element element in structure.Elements)
            {               
                MathNet.Numerics.LinearAlgebra.Matrix m = element.CalcualteLocalDisplacementMatrix();
                //Console.WriteLine("{0}: {1}", element, m);

                Inaugura.Mechanical.FEM.TrussElement2D truss = element as Inaugura.Mechanical.FEM.TrussElement2D;
                if (truss != null)
                {
                    double val = m[2, 0] - m[0, 0];
                    Console.WriteLine("Stress in {0} is {1}", element, element.Material.ElasticModulus.Value / truss.Length.Value * val);
                }
            }

            Console.WriteLine("Computing Reaction Forces");
            structure.CalculateReactionForces();
            foreach (Inaugura.Mechanical.FEM.Node node in structure.Nodes)
            {
                Console.WriteLine("{0}: {1}",node,node.ReactionForce);
            }

            

            Console.ReadLine();
        }
    }
}
