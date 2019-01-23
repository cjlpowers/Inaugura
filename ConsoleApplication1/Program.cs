using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("The distance is {0}", Inaugura.Address.CalculateDistance(new Inaugura.Address.GeographicPoint(45.3976, -75.6973), new Inaugura.Address.GeographicPoint(43.4744, -80.538)));
            Console.ReadLine();
        }
    }
}
